Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports System.Collections.Specialized

Public Class TextFileExportTask
    Inherits TaskBase

    Private m_dsConfig As New TextFileExportTaskDataSet
    Private m_File As StreamWriter
    Private m_Origen As GestorDatosBase
    Private m_CurrentLine As String
    Private m_CurrentLineValues As String()
    Private m_nCurrentLine As Integer = 0
    Private m_daOrigen As DbDataAdapter
    Private m_dtOrigen As DataTable
    Private m_drOrigen As DbDataReader
    Private m_iConteoImagenes As Integer = 1
    Private m_iConteoArchivos As Integer = 1
    Private m_sDirectorioTrabajoTarea As String = String.Empty
    Private _archivoActual As String = ""
    Private _registro As Integer = 1

    Public Overrides ReadOnly Property dsConfig() As System.Data.DataSet
        Get
            Return m_dsConfig
        End Get
    End Property

    Public Overrides Function ShowConfigDialog() As Boolean
        Return TextFileExportTaskDialog.Run(Me)
    End Function

    Public Overrides Sub Execute(ByVal pc As DataTransformation.IProgressController)
        Dim archivoAbierto As Boolean = False
        Dim tieneMascara As Boolean = False

        ' Se valida que este la configuración básica
        If m_dsConfig.General.Rows.Count = 0 _
        OrElse m_dsConfig.MapeoCampos.Rows.Count = 0 Then
            Throw New InvalidOperationException( _
                "Tarea Importación archivo no configurada correctamente.")
        End If
        Try
            ' Se abre el archivo especificado
            CreateDBObjects()


            If m_dsConfig.General(0).ArchivoDestino.Contains("<") Then
                tieneMascara = True
            End If

            If Not tieneMascara Then
                OpenFile(False)
            End If

            ' abrir origen de datos
            For Each rwMapeo As TextFileExportTaskDataSet.MapeoCamposRow In m_dsConfig.MapeoCampos
                If rwMapeo.IsOrdenNull Then
                    rwMapeo.Orden = 0
                End If
            Next
            If m_dsConfig.General(0).IncluirFilaTitulos Then
                WriteTitulos()
            End If

            Dim nTotalRows As Long = m_drOrigen.RecordsAffected
            While m_drOrigen.Read()
                Try
                    If tieneMascara Then
                        OpenFile(True)
                    End If

                    ProcessedRows += 1
                    WriteDataToTarget()
                Catch ex As Exception
                    RowsWithErros += 1
                    pc.TaskError("Error procesando fila " & _
                        ProcessedRows.ToString() & ": " & ex.Message)
                    If Not m_dsConfig.General(0).IgnorarErrores Then
                        Throw ex
                    End If
                End Try
            End While

            _registro = 1
        Finally
            If m_drOrigen IsNot Nothing Then
                If Not m_drOrigen.IsClosed Then
                    m_drOrigen.Close()
                End If
            End If
            m_Origen.CloseConnection()
            If m_File IsNot Nothing Then
                m_File.Close()
            End If
        End Try
    End Sub

    Private Function OpenFile(ByVal tieneMascara As Boolean) As Boolean
        Dim coding As Encoding
        With m_dsConfig.General(0)
            If Not DBUtils.IsNullOrEmpty(.Item("PaginaCodigos")) Then
                If IsNumeric(.PaginaCodigos) Then
                    coding = Encoding.GetEncoding(CInt(.PaginaCodigos))
                Else
                    coding = Encoding.GetEncoding(.PaginaCodigos)
                End If
            Else
                coding = Encoding.Default
            End If
            Dim filemode As FileMode = filemode.Append
            If .SobreEscribir Then
                filemode = IO.FileMode.Create
            End If
            If tieneMascara Then
                'OBTENER NOMBRE DE ARCHIVO CON MASCARA
                Dim nombreArchivo As String = String.Empty
                Dim sbLine As New StringBuilder
                'Dim rowMapeo As TextFileExportTaskDataSet.MapeoCamposRow

                nombreArchivo = GetNombreArchivoMascara(.ArchivoDestino, m_dsConfig.MapeoCampos)

                If nombreArchivo <> _archivoActual Then
                    If m_File IsNot Nothing Then
                        m_File.Close()
                    End If

                    m_File = New StreamWriter(New FileStream(nombreArchivo, filemode, FileAccess.Write, _
                                                            FileShare.Read), coding)
                    _archivoActual = nombreArchivo
                End If

            Else
                If .IsUtilizarFechaNombreNull Then
                    m_File = New StreamWriter( _
                                    New FileStream(Me.GetNombreArchivo(.ArchivoDestino, False), filemode, FileAccess.Write, _
                                        FileShare.Read), coding)
                Else
                    m_File = New StreamWriter( _
                    New FileStream(Me.GetNombreArchivo(.ArchivoDestino, .UtilizarFechaNombre), filemode, FileAccess.Write, _
                        FileShare.Read), coding)
                End If
            End If


        End With

        Return True
    End Function

    Private Function GetNombreArchivo(ByVal sArchivoDestino As String, ByVal bUtilizarFecha As Boolean) As String
        m_sDirectorioTrabajoTarea = Path.GetDirectoryName(sArchivoDestino)

        If bUtilizarFecha Then
            If (sArchivoDestino.IndexOf(".") >= 0) Then
                Dim sExtension As String = sArchivoDestino.Substring(sArchivoDestino.IndexOf("."))
                sArchivoDestino = sArchivoDestino.Substring(0, sArchivoDestino.IndexOf("."))
                Return Path.Combine(Me.m_WorkDirectory, sArchivoDestino + Now.ToString("ddMMyyHHmmss") + sExtension)
            Else
                Return Path.Combine(Me.m_WorkDirectory, sArchivoDestino + Now.ToString("ddMMyyHHmmss") + ".txt")
            End If
        Else
            Return Path.Combine(Me.m_WorkDirectory, sArchivoDestino)
        End If
    End Function

    Private Function GetNombreArchivoMascara(ByVal sArchivoDestino As String, ByVal rowMapeo As TextFileExportTaskDataSet.MapeoCamposDataTable) As String
        Dim rutaArchivo As String() = sArchivoDestino.Split(CChar("\"))
        Dim nombreArchivo As String = rutaArchivo(rutaArchivo.Length - 1)
        Dim mensaje As String = String.Empty
        Dim MascaraProcesada As List(Of String)

        MascaraProcesada = ProcesarMascaraArchivo(nombreArchivo, mensaje)
        nombreArchivo = ""

        If MascaraProcesada Is Nothing Then
            Throw New Exception(mensaje)
        Else
            For Each sMascara As String In MascaraProcesada
                Dim sValorColumna As String = String.Empty
                If sMascara.StartsWith("|") Then
                    sValorColumna = sMascara.Substring(1)
                ElseIf sMascara.StartsWith("@") Then
                    If sMascara.Substring(1) = TipoEtiqueta.CONSECUTIVO Then
                        sValorColumna = m_iConteoArchivos.ToString
                        m_iConteoArchivos = m_iConteoArchivos + 1
                    ElseIf sMascara.Substring(1).ToUpper.StartsWith(TipoEtiqueta.SFECHA) Then
                        Dim fecha As String()
                        fecha = sMascara.Substring(1).Split(CChar(":"))
                        If fecha.Length > 1 Then
                            sValorColumna = Now.ToString(fecha(1))
                        Else
                            sValorColumna = Now.ToString("ddMMyy")
                        End If
                    ElseIf sMascara.Substring(1).ToUpper.StartsWith(TipoEtiqueta.FECHA) Then
                        Dim fecha As String()
                        fecha = sMascara.Substring(1).Split(CChar(":"))
                        If fecha.Length > 1 Then
                            sValorColumna = Now.ToString(fecha(1))
                        Else
                            sValorColumna = Now.ToString("ddMMyyHHmmss")
                        End If
                    End If
                Else
                    sValorColumna = DBUtils.ToString(m_drOrigen.GetValue(m_drOrigen.GetOrdinal(sMascara)), String.Empty)
                End If
                nombreArchivo = nombreArchivo + sValorColumna
            Next
        End If

        Dim directorioTrabajoTarea As String = ""
        For I As Integer = 0 To rutaArchivo.Length - 2
            directorioTrabajoTarea += rutaArchivo(I) + "\"
        Next

        Return (Path.Combine(directorioTrabajoTarea, nombreArchivo))
    End Function


    Private Sub IniciarDatosOrigen()
        
    End Sub

    Private Sub CreateDBObjects()
        ' Se crea el origen de datos y se llena el esquema de la tabla correspondiente
        m_Origen = GestorDatosFactory.CreateInstance(Me.Parent.ConnectionTypeName, _
            Me.Parent.ConnectionString)
        m_Origen.OpenConnection()
        m_dtOrigen = New DataTable
        m_daOrigen = m_Origen.CreateDataAdapter(m_dsConfig.General(0).ConsultaOrigen, False)
        m_daOrigen.FillSchema(m_dtOrigen, SchemaType.Source)
        m_drOrigen = m_Origen.ExecuteReader(CommandType.Text, m_dsConfig.General(0).ConsultaOrigen)
    End Sub

    Private Function GetSafeName(ByVal sName As String) As String
        If sName.IndexOf(" "c) > 0 Then
            Return "[" & sName & "]"
        Else
            Return sName
        End If
    End Function


    Private Function GetValorCampo(ByVal rowMapeo As TextFileExportTaskDataSet.MapeoCamposRow) As String
        Dim retVal As String = ""
        Dim bValorAsignado As Boolean = True
        Dim sFormato As String = ""

        ' Se obtiene el valor de la linea leida
        If Not DBUtils.IsNullOrEmpty(rowMapeo("ValorEstatico")) Then
            If rowMapeo.ValorEstatico.Contains("@") Then
                If rowMapeo.ValorEstatico.StartsWith("@CONSEC") Then
                    retVal = CStr(_registro)
                Else
                    Throw New InvalidOperationException("El valor estatico para la Columna " & rowMapeo.CampoOrigen & _
                    " no tiene un formato valido. Formato actual: " & rowMapeo.ValorEstatico)
                End If
            Else
                retVal = rowMapeo.ValorEstatico
            End If
        Else
            ' Se realiza el parse del campo dependiendo del tipo de objeto
            ' Obtener el valor del campo origen
            Dim columType As System.Type = m_dtOrigen.Columns(rowMapeo.CampoOrigen).DataType
            Dim colValue As Object = m_drOrigen.GetValue(m_drOrigen.GetOrdinal(rowMapeo.CampoOrigen))
            If columType Is GetType(System.String) Then
                retVal = colValue.ToString().Replace(vbCrLf, " ").Replace(vbCr, " "c)
            ElseIf columType Is GetType(System.Byte()) Then
                If colValue.GetType Is GetType(System.DBNull) Then
                    Return ""
                Else
                    'se crea el archivo tipo .jgep y se retorna el path del mismo
                    Dim ms As IO.MemoryStream = New IO.MemoryStream(CType(colValue, Byte()))
                    Dim image1 As New Bitmap(ms)

                    Dim MascaraProcesada As List(Of String)
                    Dim sMensaje As String = String.Empty
                    MascaraProcesada = Me.ProcesarMascara(rowMapeo.MascaraImagenes, sMensaje)

                    If MascaraProcesada Is Nothing Then
                        Throw New Exception(sMensaje)
                    Else
                        Dim nombreArchivoImagen As String = ""
                        For Each sMascara As String In MascaraProcesada
                            Dim sValorColumna As String = String.Empty
                            If sMascara.StartsWith("|") Then
                                sValorColumna = sMascara.Substring(1)
                            ElseIf sMascara.StartsWith("@") Then
                                If sMascara.Substring(1) = TipoEtiqueta.CONSECUTIVO Then
                                    sValorColumna = m_iConteoImagenes.ToString
                                    m_iConteoImagenes = m_iConteoImagenes + 1
                                ElseIf sMascara.Substring(1) = TipoEtiqueta.FECHA Then
                                    sValorColumna = Now.ToString("ddMMyyHHmmss")
                                ElseIf sMascara.Substring(1) = TipoEtiqueta.SFECHA Then
                                    sValorColumna = Now.ToString("ddMMyy")
                                End If
                            Else
                                sValorColumna = DBUtils.ToString(m_drOrigen.GetValue(m_drOrigen.GetOrdinal(sMascara)), String.Empty)
                            End If
                            nombreArchivoImagen = nombreArchivoImagen + sValorColumna
                        Next
                        nombreArchivoImagen = nombreArchivoImagen & ".jpeg"
                        'Se crea el archivo en el PathWorkDirectory asignado
                        If m_sDirectorioTrabajoTarea <> String.Empty Then
                            image1.Save(IO.Path.Combine(IO.Path.Combine(m_WorkDirectory, m_sDirectorioTrabajoTarea), nombreArchivoImagen), Drawing.Imaging.ImageFormat.Jpeg)
                            Return IO.Path.Combine(IO.Path.Combine(m_WorkDirectory, m_sDirectorioTrabajoTarea), nombreArchivoImagen)
                        Else
                            image1.Save(IO.Path.Combine(m_WorkDirectory, nombreArchivoImagen), Drawing.Imaging.ImageFormat.Jpeg)
                            Return IO.Path.Combine(m_WorkDirectory, nombreArchivoImagen)
                        End If


                    End If
                End If

            Else
                If Not DBUtils.IsNullOrEmpty(rowMapeo("Formato")) Then
                    sFormato = "{0:" & rowMapeo.Formato & "}"
                Else
                    sFormato = "{0}"
                End If
                retVal = String.Format(sFormato, colValue)
            End If
        End If
        If m_dsConfig.General(0).UsaLongitudesFijas Then
            If DBUtils.ToDouble(rowMapeo.Longitud, 0) > 0 Then
                retVal = LSet(retVal, DBUtils.ToInt(rowMapeo.Longitud, 0))
            Else
                retVal = RSet(retVal, -DBUtils.ToInt(rowMapeo.Longitud, 0))
            End If
        End If
        Return retVal
    End Function

    Private Sub WriteTitulos()
        Dim sbLine As New StringBuilder
        Dim rowMapeo As TextFileExportTaskDataSet.MapeoCamposRow
        Dim colTitle As String
        With m_dsConfig.General(0)
            Dim mapeoOrden() As DataRow = m_dsConfig.MapeoCampos.Select("1=1", "Orden")
            For I As Integer = 0 To mapeoOrden.Length - 1
                rowMapeo = CType(mapeoOrden(I), TextFileExportTaskDataSet.MapeoCamposRow)
                colTitle = rowMapeo.TituloCampo.Trim()
                If m_dsConfig.General(0).UsaLongitudesFijas Then
                    If DBUtils.ToDouble(rowMapeo.Longitud, 0) > 0 Then
                        colTitle = LSet(colTitle, DBUtils.ToInt(rowMapeo.Longitud, 0))
                    Else
                        colTitle = RSet(colTitle, -DBUtils.ToInt(rowMapeo.Longitud, 0))
                    End If
                End If
                sbLine.Append(colTitle)
                If I < m_dsConfig.MapeoCampos.Count - 1 Then
                    sbLine.Append(UnescapeString(.SeparadorCampos))
                End If
            Next
        End With
        m_File.WriteLine(sbLine.ToString())
    End Sub

    Private Sub WriteDataToTarget()
        ' Se intenta insertar los datos
        Dim sbLine As New StringBuilder
        Dim rowMapeo As TextFileExportTaskDataSet.MapeoCamposRow
        Dim contadorImagenes As Integer = 1

        With m_dsConfig.General(0)
            'Generar el archivo en Orden
            Dim mapeoOrden() As DataRow = m_dsConfig.MapeoCampos.Select("1=1", "Orden")
            For I As Integer = 0 To mapeoOrden.Length - 1
                rowMapeo = CType(mapeoOrden(I), TextFileExportTaskDataSet.MapeoCamposRow)

                sbLine.Append(GetValorCampo(rowMapeo))

                If I < m_dsConfig.MapeoCampos.Count - 1 Then
                    sbLine.Append(UnescapeString(.SeparadorCampos))
                End If
            Next
        End With
        m_File.WriteLine(sbLine.ToString())

        _registro += 1
    End Sub

    Private Function ProcesarMascara(ByVal Mascara As String, ByRef sMensaje As String) As List(Of String)
        're[dsdsa]dsd[re][ew]ss[s]
        Mascara = Mascara.Replace("][", "|-")
        're[dsdsa]dsd[re|-ew]ss[s]
        Mascara = Mascara.Replace("[", "|-")
        're|-dsdsa]dsd|-re|-ew]ss|-s]
        Mascara = Mascara.Replace("]", "|")
        're|-dsdsa|dsd|-re|-ew|ss|-s|
        Dim sDatosMascara() As String = Mascara.Split("|"c)
        Dim MascaraProcesada As New List(Of String)

        If sDatosMascara.Length = 0 Then
            sMensaje = "NO SE HA PARAMETRIZADO LA MASCARA"
            Return Nothing
        Else
            For Each sDato As String In sDatosMascara
                'COLUMNA
                If sDato.StartsWith("-") Then
                    If sDato.StartsWith("-@") Then
                        If (sDato.Substring(2).ToUpper = TipoEtiqueta.CONSECUTIVO) Or (sDato.Substring(2).ToUpper = TipoEtiqueta.FECHA) Or (sDato.Substring(2).ToUpper = TipoEtiqueta.SFECHA) Then
                            MascaraProcesada.Add(sDato.Substring(1).ToUpper)
                        Else
                            sMensaje = "LA COLUMNA " + sDato.Substring(2) + " CONSTANTE NO ESTA PARAMETRIZADA"
                            Return Nothing
                        End If
                    Else
                        If Me.ColumnaExiste(sDato.Substring(1)) Then
                            MascaraProcesada.Add(sDato.Substring(1))
                        Else
                            sMensaje = "LA COLUMNA " + sDato.Substring(1) + " DE LA MASCARA NO EXISTE"
                            Return Nothing
                        End If
                    End If
                Else
                    If sDato <> String.Empty Then
                        MascaraProcesada.Add("|" + sDato)
                    End If
                End If
            Next
        End If
        Return MascaraProcesada
    End Function

    Private Function ColumnaExiste(ByVal NombreColumna As String) As Boolean
        Dim Existe As Boolean = False

        For Each rowMapeo As DataColumn In m_dtOrigen.Columns

            If rowMapeo.ColumnName = NombreColumna Then
                Existe = True
            End If
        Next

        Return Existe
    End Function

    Private Function ProcesarMascaraArchivo(ByVal Mascara As String, ByRef sMensaje As String) As List(Of String)
        're<dsdsa>dsd<re><ew>.<ss><Fecha:s>
        Mascara = Mascara.Replace("><", "|-")
        're<dsdsa>dsd<re|-ew>.<ss|-Fecha:s>
        Mascara = Mascara.Replace("<", "|-")
        're|-dsdsa>dsd|-re|-ew>.|-ss|-Fecha:s>
        Mascara = Mascara.Replace(">", "|")
        're|-dsdsa|dsd|-re|-ew|.|-ss|-Fecha:s|

        Dim sDatosMascara() As String = Mascara.Split("|"c)
        Dim MascaraProcesada As New List(Of String)

        If sDatosMascara.Length = 0 Then
            sMensaje = "NO SE HA PARAMETRIZADO LA MASCARA"
            Return Nothing
        Else
            For Each sDato As String In sDatosMascara
                'COLUMNA
                If sDato.StartsWith("-") Then
                    If sDato.StartsWith("-@") Then
                        If (sDato.Substring(2).ToUpper = TipoEtiqueta.CONSECUTIVO) Then
                            MascaraProcesada.Add(sDato.Substring(1).ToUpper)
                        ElseIf (sDato.Substring(2).ToUpper.StartsWith(TipoEtiqueta.FECHA)) Then
                            MascaraProcesada.Add(sDato.Substring(1))
                        Else
                            sMensaje = "LA COLUMNA " + sDato.Substring(2) + " CONSTANTE NO ESTA PARAMETRIZADA"
                            Return Nothing
                        End If
                    Else
                        If Me.ColumnaExiste(sDato.Substring(1)) Then
                            MascaraProcesada.Add(sDato.Substring(1))
                        Else
                            sMensaje = "LA COLUMNA " + sDato.Substring(1) + " DE LA MASCARA NO EXISTE"
                            Return Nothing
                        End If
                    End If
                Else
                    If sDato <> String.Empty Then
                        MascaraProcesada.Add("|" + sDato)
                    End If
                End If
            Next
        End If
        Return MascaraProcesada
    End Function
End Class
