Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports System.Collections.Specialized

Public Class TextFileImportTask
    Inherits TaskBase

    Private m_dsConfig As New TextFileImportTaskDataSet
    Private m_File As StreamReader
    Private m_Destino As GestorDatosBase
    Private m_CurrentLine As String
    Private m_CurrentLineValues As String()
    Private m_nCurrentLine As Integer = 0
    Private m_FileSize As Long = 0
    Private m_ReadBytes As Long = 0
    Private m_daDestino As DbDataAdapter
    Private m_dtDestino As DataTable
    Private _fileInfo As IO.FileInfo
    Private _registro As Integer = 1

    Public Overrides ReadOnly Property dsConfig() As System.Data.DataSet
        Get
            Return m_dsConfig
        End Get
    End Property

    Public Overrides Function ShowConfigDialog() As Boolean
        Return TextFileImportTaskDialog.Run(Me)
    End Function

    Public Overrides Sub Execute(ByVal pc As DataTransformation.IProgressController)

        ' Se valida que este la configuración básica
        If m_dsConfig.General.Rows.Count = 0 _
        OrElse m_dsConfig.MapeoCampos.Rows.Count = 0 Then
            Throw New InvalidOperationException( _
                "Tarea Importación archivo no configurada correctamente.")
        End If
        Try
            ' Se abre el archivo especificado
            CreateDBObjects()
            m_Destino.BeginTransaction()

            If m_dsConfig.General(0).ArchivoOrigen.Contains("*") Then

                Dim strFileSize As String = ""
                Dim rutaArchivo As String() = m_dsConfig.General(0).ArchivoOrigen.Split(CChar("\"))

                Dim directorioTrabajoTarea As String = ""
                For I As Integer = 0 To rutaArchivo.Length - 2
                    directorioTrabajoTarea += rutaArchivo(I) + "\"
                Next

                Dim di As New IO.DirectoryInfo(directorioTrabajoTarea)

                Dim archivos As IO.FileInfo() = di.GetFiles(rutaArchivo(rutaArchivo.Length - 1))
                Dim Archivo As IO.FileInfo
                Dim paginasCodigos As String = ""

                If Not DBUtils.IsNullOrEmpty(m_dsConfig.General(0).Item("PaginaCodigos")) Then
                    paginasCodigos = m_dsConfig.General(0).PaginaCodigos
                End If

                For Each Archivo In archivos
                    OpenFile(paginasCodigos, Archivo)
                    ProcesarArchivo(pc)

                    If m_dsConfig.General(0).EliminarArchivo Then
                        Dim nombreArchivo As String = ""
                        Dim nombreArchivoCopia As String = ""
                        Dim nombresArchivo As String() = Archivo.Name.Split(CChar("."))

                        For J As Integer = 0 To nombresArchivo.Length - 1
                            If J < nombresArchivo.Length - 1 Then
                                If J = 0 Then
                                    nombreArchivo += nombresArchivo(J)
                                Else
                                    nombreArchivo += "." + nombresArchivo(J)
                                End If
                            Else
                                nombreArchivoCopia = nombreArchivo + Date.Now.ToString("yyyyMMddHHmmss") + "." + nombresArchivo(J)
                                nombreArchivo += "." + nombresArchivo(J)
                            End If
                        Next

                        IO.File.Copy(Archivo.FullName, Archivo.DirectoryName + "\Backup\" + nombreArchivoCopia)

                        If m_File IsNot Nothing Then
                            m_File.Close()
                        End If

                        IO.File.Delete(Archivo.FullName)
                    Else
                        If m_File IsNot Nothing Then
                            m_File.Close()
                        End If
                    End If


                    _registro = 1
                Next
            Else
                OpenFile()
                ProcesarArchivo(pc)

                If m_dsConfig.General(0).EliminarArchivo Then
                    Dim rutaArchivo As String() = m_dsConfig.General(0).ArchivoOrigen.Split(CChar("\"))

                    Dim directorioTrabajoTarea As String = ""
                    Dim nombreArchivo As String = ""
                    Dim nombreArchivoCopia As String = ""

                    For I As Integer = 0 To rutaArchivo.Length
                        If I < rutaArchivo.Length - 1 Then
                            directorioTrabajoTarea += rutaArchivo(I) + "\"
                        ElseIf I = rutaArchivo.Length - 1 Then
                            Dim nombresArchivo As String() = rutaArchivo(I).Split(CChar("."))

                            For J As Integer = 0 To nombresArchivo.Length - 1
                                If J < nombresArchivo.Length - 1 Then
                                    If J = 0 Then
                                        nombreArchivo += nombresArchivo(J)
                                    Else
                                        nombreArchivo += "." + nombresArchivo(J)
                                    End If
                                Else
                                    nombreArchivoCopia = nombreArchivo + Date.Now.ToString("yyyyMMddHHmmss") + "." + nombresArchivo(J)
                                    nombreArchivo += "." + nombresArchivo(J)
                                End If
                            Next
                        End If
                    Next

                    IO.File.Copy(m_dsConfig.General(0).ArchivoOrigen, directorioTrabajoTarea + "Backup\" + nombreArchivoCopia)

                    If m_File IsNot Nothing Then
                        m_File.Close()
                    End If

                    IO.File.Delete(m_dsConfig.General(0).ArchivoOrigen)
                End If

                _registro = 1
            End If

            If Not DBUtils.IsNullOrEmpty(m_dsConfig.General(0).Item("ComandoFinalizacion")) Then

                ' Ejecucion del Comando de Finalizacion de la tabla destino
                Dim ComandoFinalizacion As String() = m_dsConfig.General(0).ComandoFinalizacion.Split()

                If ComandoFinalizacion.Length > 1 Then
                    m_Destino.ExecuteNonQuery(CommandType.Text, m_dsConfig.General(0).ComandoFinalizacion)
                Else
                    m_Destino.ExecuteNonQuery(CommandType.StoredProcedure, m_dsConfig.General(0).ComandoFinalizacion)
                End If
            End If

            m_Destino.CommitTransaction()
        Catch ex As Exception
            m_Destino.RollbackTransaction()
            Throw ex
        Finally
            If m_File IsNot Nothing Then
                m_File.Close()
            End If
        End Try
    End Sub

    Private Sub ProcesarArchivo(ByVal pc As DataTransformation.IProgressController)
        While Not m_File.EndOfStream
            ReadLineAndValues()

            Dim filaInicio As Decimal = 0

            If Not IsDBNull(m_dsConfig.General(0).IgnorarFilas) Then
                filaInicio = m_dsConfig.General(0).IgnorarFilas
            End If


            If m_dsConfig.General(0).ContieneFilaTitulos And m_nCurrentLine = 1 + filaInicio Then
                Continue While
            End If

            If filaInicio >= m_nCurrentLine Then
                Continue While
            End If

            If m_dsConfig.General(0).VerificarNumerico Then
                If m_CurrentLine.ToString().Length >= m_dsConfig.General(0).LongitudNumerico Then
                    If Not IsNumeric(m_CurrentLine.ToString().Substring(m_dsConfig.General(0).InicioNumerico, m_dsConfig.General(0).LongitudNumerico)) Then
                        Continue While
                    End If
                Else
                    If Not IsNumeric(m_CurrentLine.ToString()) Then
                        Continue While
                    End If
                End If
            End If

            ' Asignan los prametros de los comandos del DataAdapter solucionado
            Try
                SetCommandParameters(m_daDestino.InsertCommand)
                If m_daDestino.UpdateCommand IsNot Nothing Then
                    SetCommandParameters(m_daDestino.UpdateCommand)
                End If
                m_Destino.PrepareAdapter(m_daDestino)

                _registro += 1
                ProcessedRows += 1
                WriteDataToTarget()

            Catch ex As Exception
                RowsWithErros += 1
                pc.TaskError("Error procesando fila " & _
                    m_nCurrentLine.ToString() & ": Tipo:" & ex.GetType.Name & "-" & ex.Message)
                If Not m_dsConfig.General(0).IgnorarErrores Then
                    Throw ex
                End If
            End Try

        End While
    End Sub


    Private Sub OpenFile()
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
            m_File = New StreamReader(.ArchivoOrigen, coding)
            _fileInfo = New FileInfo(.ArchivoOrigen)
            m_FileSize = _fileInfo.Length
            m_ReadBytes = 0
        End With
    End Sub

    Private Sub OpenFile(ByVal PaginaCodigos As String, ByVal ArchivoOrigen As IO.FileInfo)
        Dim coding As Encoding

        'Dim rutaArchivo As String() = ArchivoOrigen.Split(CChar("\"))
        'ArchivoOrigen = rutaArchivo(rutaArchivo.Length - 1)

        If Not PaginaCodigos = "" Then
            If IsNumeric(PaginaCodigos) Then
                coding = Encoding.GetEncoding(CInt(PaginaCodigos))
            Else
                coding = Encoding.GetEncoding(PaginaCodigos)
            End If
        Else
            coding = Encoding.Default
        End If
        m_File = New StreamReader(ArchivoOrigen.FullName, coding)
        _fileInfo = New FileInfo(ArchivoOrigen.FullName)
        m_FileSize = _fileInfo.Length
        m_ReadBytes = 0
    End Sub

    Private Sub CreateDBObjects()
        Dim I As Integer
        m_Destino = GestorDatosFactory.CreateInstance(Me.Parent.ConnectionTypeName, _
            Me.Parent.ConnectionString)

        ' Se crea la consulta de manera dinámica y luego el dataadapter correspondiente
        Dim sbSelect As New StringBuilder
        sbSelect.Append("SELECT ")
        Dim rowMapeo As TextFileImportTaskDataSet.MapeoCamposRow
        For I = 0 To m_dsConfig.MapeoCampos.Count - 1
            rowMapeo = m_dsConfig.MapeoCampos(I)
            sbSelect.Append(GetSafeName(rowMapeo.CampoDestino))
            If I = m_dsConfig.MapeoCampos.Count - 1 Then
                sbSelect.Append(" ")
            Else
                sbSelect.Append(", ")
            End If
        Next I

        sbSelect.Append("FROM " & GetSafeName(m_dsConfig.General(0).TablaDestino))
        m_daDestino = m_Destino.CreateDataAdapter(sbSelect.ToString())
        m_dtDestino = New DataTable
        m_daDestino.FillSchema(m_dtDestino, SchemaType.Source)

        ' Se modifica la clausula update para solo modificar los campos configurados
        ' para tal fin.
        If m_daDestino.UpdateCommand IsNot Nothing Then
            Dim rowMapeosUpdate() As TextFileImportTaskDataSet.MapeoCamposRow
            rowMapeosUpdate = CType(m_dsConfig.MapeoCampos.Select("NoActualizar = False"), TextFileImportTaskDataSet.MapeoCamposRow())
            Dim sbUpdate As New StringBuilder
            sbUpdate.Append("SELECT ")
            For I = 0 To rowMapeosUpdate.GetUpperBound(0)
                rowMapeo = rowMapeosUpdate(I)
                sbUpdate.Append(GetSafeName(rowMapeo.CampoDestino))
                If I = rowMapeosUpdate.GetUpperBound(0) Then
                    sbUpdate.Append(" ")
                Else
                    sbUpdate.Append(", ")
                End If
            Next I
            sbUpdate.Append("FROM " & GetSafeName(m_dsConfig.General(0).TablaDestino))
            Dim daUpdate As DbDataAdapter = m_Destino.CreateDataAdapter(sbUpdate.ToString())
            m_daDestino.UpdateCommand = daUpdate.UpdateCommand
        End If
       
    End Sub

    Private Function GetSafeName(ByVal sName As String) As String
        If sName.IndexOf(" "c) > 0 Then
            Return "[" & sName & "]"
        Else
            Return sName
        End If
    End Function


    Private Sub ReadLineAndValues()
        With m_dsConfig.General(0)
            m_CurrentLine = m_File.ReadLine
            m_ReadBytes = m_File.BaseStream.Position
            If .UsaLongitudesFijas Then
                m_CurrentLineValues = Nothing
            Else
                m_CurrentLineValues = m_CurrentLine.Split(UnescapeString(.SeparadoresCampos).ToCharArray())
            End If
        End With
        m_nCurrentLine += 1
    End Sub

    Private Sub SetCommandParameters(ByVal cmd As DbCommand)
        Dim rowMapeo As TextFileImportTaskDataSet.MapeoCamposRow
        For I As Integer = 0 To cmd.Parameters.Count - 1
            rowMapeo = m_dsConfig.MapeoCampos.FindByCampoDestino(cmd.Parameters(I).SourceColumn)
            If rowMapeo Is Nothing Then
                Throw New InvalidOperationException("Columna " & cmd.Parameters(I).SourceColumn & " no configurada!")
            End If
            cmd.Parameters(I).Value = GetValorCampo(rowMapeo)
        Next
    End Sub

    Private Function GetValorCampo(ByVal rowMapeo As TextFileImportTaskDataSet.MapeoCamposRow) As Object
        Dim retVal As Object = DBNull.Value
        Dim bValorAsignado As Boolean = True

        ' Se obtiene el valor de la linea leida
        Dim sValor As String
        If Not DBUtils.IsNullOrEmpty(rowMapeo("ValorEstatico")) Then
            If rowMapeo.ValorEstatico.Contains("@") Then
                Dim partesNombreArchivo As String() = _fileInfo.Name.Split(CChar("."))
                If rowMapeo.ValorEstatico.StartsWith("@FILENAME") Then

                    Dim nombreArchivo As String = partesNombreArchivo(0)
                    Dim formato As String() = rowMapeo.ValorEstatico.Split(CChar(","))

                    If formato.Length < 3 Then
                        Throw New InvalidOperationException("El valor estatico para la Columna " & rowMapeo.CampoDestino & _
                                            " no tiene un formato valido. Formato actual: " & rowMapeo.ValorEstatico)
                    Else
                        Try
                            sValor = nombreArchivo.Substring(CInt(formato(1)), CInt(formato(2)))
                        Catch ex As Exception
                            Throw New InvalidOperationException("El valor estatico para la Columna " & rowMapeo.CampoDestino & _
                                                                        " no tiene un formato valido. Formato actual: " & rowMapeo.ValorEstatico)
                        End Try
                    End If
                ElseIf rowMapeo.ValorEstatico.StartsWith("@FILEEXT") Then

                    Dim extensionArchivo As String = partesNombreArchivo(partesNombreArchivo.Length - 1)
                    Dim formato As String() = rowMapeo.ValorEstatico.Split(CChar(","))

                    If formato.Length < 3 Then
                        Throw New InvalidOperationException("El valor estatico para la Columna " & rowMapeo.CampoDestino & _
                                            " no tiene un formato valido. Formato actual: " & rowMapeo.ValorEstatico)
                    Else
                        Try
                            sValor = extensionArchivo.Substring(CInt(formato(1)), CInt(formato(2)))
                        Catch ex As Exception
                            Throw New InvalidOperationException("El valor estatico para la Columna " & rowMapeo.CampoDestino & _
                                                                        " no tiene un formato valido. Formato actual: " & rowMapeo.ValorEstatico)
                        End Try
                    End If
                ElseIf rowMapeo.ValorEstatico.StartsWith("@CONSEC") Then
                    sValor = CStr(_registro)
                ElseIf rowMapeo.ValorEstatico.StartsWith("@FECHA") Then
                    sValor = Date.Now.ToString
                Else
                    Throw New InvalidOperationException("El valor estatico para la Columna " & rowMapeo.CampoDestino & _
                    " no tiene un formato valido. Formato actual: " & rowMapeo.ValorEstatico)
                End If
            Else
                sValor = rowMapeo.ValorEstatico
            End If
        ElseIf m_dsConfig.General(0).UsaLongitudesFijas Then
            sValor = Mid(m_CurrentLine, DBUtils.ToInt(rowMapeo.Posicion, 0), DBUtils.ToInt(rowMapeo.Longitud, 0)).Trim()
        Else
            sValor = m_CurrentLineValues(DBUtils.ToInt(rowMapeo.Posicion, 0) - 1).Trim()
        End If

        ' Se realiza el parse del campo dependiendo del tipo de objeto
        ' Obtener el valor del campo origen
        Dim columType As System.Type = m_dtDestino.Columns(rowMapeo.CampoDestino).DataType
        If columType Is GetType(System.String) Then
            retVal = sValor
        ElseIf columType Is GetType(System.DateTime) Then
            If DBUtils.IsNullOrEmpty(rowMapeo("Formato")) Then
                Throw New InvalidOperationException("Columna " & rowMapeo.CampoDestino & " no tiene un formato de entrada definido")
            Else
                Try
                    Dim cadenaFormato As String() = rowMapeo("Formato").ToString.ToUpper.Split(CChar("/"))
                    Dim dia As String = ""
                    Dim mes As String = ""
                    Dim año As String = ""
                    Dim posicionDia As Integer = -1
                    Dim posicionMes As Integer = -1
                    Dim posicionAño As Integer = -1

                    Dim cadenaFecha As String() = sValor.Split(CChar("/"))

                    Dim formatoPersonalizado As Boolean = False

                    Dim posicion As Integer = 0

                    For i As Integer = 0 To cadenaFormato.Length - 1
                        If cadenaFormato(i).StartsWith("@DIA") Then
                            dia = CStr(Date.Now.Day)
                            posicionDia = i
                            formatoPersonalizado = True
                        ElseIf cadenaFormato(i).StartsWith("@MES") Then
                            mes = CStr(Date.Now.Month)
                            posicionMes = i
                            formatoPersonalizado = True
                        ElseIf cadenaFormato(i).StartsWith("@AÑO") Then
                            año = CStr(Date.Now.Year)
                            posicionAño = i
                            formatoPersonalizado = True
                        ElseIf cadenaFormato(i).StartsWith("D") Or cadenaFormato(i).StartsWith("DD") Then
                            dia = cadenaFecha(posicion)
                            posicion += 1
                            posicionDia = i
                        ElseIf cadenaFormato(i).StartsWith("M") Or cadenaFormato(i).StartsWith("MM") Then
                            mes = cadenaFecha(posicion)
                            posicion += 1
                            posicionMes = i
                        ElseIf cadenaFormato(i).StartsWith("YY") Or cadenaFormato(i).StartsWith("YYYY") Then
                            año = cadenaFecha(posicion)
                            posicion += 1
                            posicionAño = i
                        End If
                    Next

                    sValor = "{0}/{1}/{2}"

                    sValor = sValor.Replace("{" & posicionDia & "}", dia)
                    sValor = sValor.Replace("{" & posicionMes & "}", mes)
                    sValor = sValor.Replace("{" & posicionAño & "}", año)

                    Dim Formato As String = "{0}/{1}/{2}"

                    Formato = Formato.Replace("{" & posicionDia & "}", "dd")
                    Formato = Formato.Replace("{" & posicionMes & "}", "MM")
                    Formato = Formato.Replace("{" & posicionAño & "}", "yyyy")

                    If formatoPersonalizado Then
                        retVal = Date.ParseExact(sValor, Formato, Nothing)
                    Else
                        retVal = Date.ParseExact(sValor, rowMapeo.Formato, Nothing)
                    End If

                Catch ex As Exception
                    If (Not sValor.Equals("")) Then
                        Throw New InvalidOperationException("Valor:" & sValor & " para la Columna " & rowMapeo.CampoDestino & _
                        " no tiene un formato valido. Formato actual: " & rowMapeo.Formato)
                    Else
                        retVal = Nothing
                    End If
                End Try
            End If
        ElseIf columType Is GetType(System.Int16) Then
            If sValor = "" Then
                retVal = 0
            Else
                retVal = CShort(sValor)
            End If
        ElseIf columType Is GetType(System.Int32) Then
            If sValor = "" Then
                retVal = 0
            Else
                retVal = CInt(sValor)
            End If
		ElseIf columType Is GetType(System.Int64) Then

			Dim culturaActual As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture

			Dim separadorMiles As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator
			Dim separadorDecimales As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator

			Try
				System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-CO", False)

				If m_dsConfig.General(0).SeparadorMiles Is String.Empty Then
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = "."
				Else
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = m_dsConfig.General(0).SeparadorMiles
				End If

				If m_dsConfig.General(0).SeparadorDecimales Is String.Empty Then
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ","
				Else
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = m_dsConfig.General(0).SeparadorDecimales
				End If

				If sValor = "" Then
					retVal = 0
				Else
					retVal = CLng(sValor)
				End If

				System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = separadorMiles
				System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = separadorDecimales

			Finally

				System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(culturaActual.Name)
			End Try

		ElseIf columType Is GetType(System.Double) Then
			Dim culturaActual As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture

			Dim separadorMiles As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator
			Dim separadorDecimales As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator

			Try
				System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-CO", False)

				If m_dsConfig.General(0).SeparadorMiles Is String.Empty Then
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = "."
				Else
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = m_dsConfig.General(0).SeparadorMiles
				End If

				If m_dsConfig.General(0).SeparadorDecimales Is String.Empty Then
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ","
				Else
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = m_dsConfig.General(0).SeparadorDecimales
				End If

				If sValor = "" Then
					retVal = 0
				Else
					retVal = CDbl(sValor)
				End If

				System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = separadorMiles
				System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = separadorDecimales

			Finally

				System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(culturaActual.Name)
			End Try

		ElseIf columType Is GetType(System.Decimal) Then
			Dim culturaActual As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture

			Dim separadorMiles As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator
			Dim separadorDecimales As String = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator


			Try

				System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-CO", False)

				If m_dsConfig.General(0).SeparadorMiles Is String.Empty Then
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = "."
				Else
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = m_dsConfig.General(0).SeparadorMiles
				End If

				If m_dsConfig.General(0).SeparadorDecimales Is String.Empty Then
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ","
				Else
					System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = m_dsConfig.General(0).SeparadorDecimales
				End If

				If sValor = "" Then
					retVal = 0
				Else
					retVal = CDec(sValor)
				End If

				System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = separadorMiles
				System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = separadorDecimales

			Finally

				System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(culturaActual.Name)
			End Try

		ElseIf columType Is GetType(System.Boolean) Then
				If sValor = "" Then
					retVal = False
				Else
					retVal = CBool(sValor)
				End If
		End If
		Return retVal
    End Function

    Private Sub WriteDataToTarget()
        ' Se intenta insertar los datos
        Dim bTryUpdate As Boolean = False
        Dim InsertException As Exception = Nothing

        If m_dsConfig.General(0).InsertarFilasNuevas Then
            ' Se intenta insertar la fila
            Try
                m_daDestino.InsertCommand.ExecuteNonQuery()
            Catch ex As Exception
                If Not m_dsConfig.General(0).ActualizarFilasActuales _
                Or m_daDestino.UpdateCommand Is Nothing Then
                    Throw ex
                Else
                    InsertException = ex
                    bTryUpdate = True
                End If
            End Try
        End If

        If (bTryUpdate Or m_dsConfig.General(0).ActualizarFilasActuales) _
            And m_daDestino.UpdateCommand IsNot Nothing Then
            Dim AffectedRows As Integer

            ' Se intenta actualizar la fila
            Try
                AffectedRows = m_daDestino.UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Throw ex
            End Try

            ' Si el update no arrojo filas y la insercion arrojo excepcion se lanza nuevamente la excepcion
            If AffectedRows = 0 And InsertException IsNot Nothing Then
                Throw InsertException
            End If
        End If
    End Sub

End Class
