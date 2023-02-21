Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports System.ComponentModel

<Description("Tarea importación de base de datos")> _
Public Class DBImportTask
    Inherits TaskBase

    Private m_dsConfig As DBImportTaskDataSet
    Private m_SourceConnectionString As String
    Private m_SourceConnectionType As String
    Private m_TargetConnectionString As String
    'Private m_TargetConnectionType

    Public Overrides ReadOnly Property dsConfig() As System.Data.DataSet
        Get
            If m_dsConfig Is Nothing Then
                m_dsConfig = New DBImportTaskDataSet
            End If
            Return m_dsConfig
        End Get
    End Property

    Public Overrides Function ShowConfigDialog() As Boolean
        Return DBImportTaskDialog.Run(Me)
    End Function

    Public Overrides Sub Execute(ByVal pc As DataTransformation.IProgressController)
        Dim Destino As GestorDatosBase
        Dim Origen As GestorDatosBase
        Dim sSqlOrigen As New System.Text.StringBuilder

        ' Se valida que este la configuración básica
        If m_dsConfig.General.Rows.Count = 0 _
        OrElse m_dsConfig.MapeoCampos.Rows.Count = 0 Then
            Throw New InvalidOperationException( _
                "Tarea Importación datos no configurada correctamente.")
        End If

        ' Se crean los gestores de conexion a la base datos
        Destino = GestorDatosFactory.CreateInstance( _
            Parent.ConnectionTypeName, Parent.ConnectionString)
        Origen = GestorDatosFactory.CreateInstance( _
            m_dsConfig.General(0).TipoConexion, m_dsConfig.General(0).CadenaConexion)

        ' Se consultan los datos de origen
        Dim daOrigen As DbDataAdapter = Origen.CreateDataAdapter()
        daOrigen.SelectCommand = Origen.CreateCommand()
        daOrigen.SelectCommand.Connection = Origen.Connection
        daOrigen.SelectCommand.CommandType = CommandType.Text
        daOrigen.SelectCommand.CommandText = m_dsConfig.General(0).ConsultaOrigen
        Dim dtOrigen As New DataTable
        'daOrigen.FillSchema(dtOrigen, SchemaType.Source)
        daOrigen.Fill(dtOrigen)
        dtOrigen.Constraints.Clear()
        dtOrigen.PrimaryKey = Nothing

        ' Se consulta la tabla destino
        Dim daDestino As DbDataAdapter = Destino.CreateDataAdapter("SELECT * FROM " & m_dsConfig.General(0).TablaDestino)
        Dim dtDestino As New DataTable
        daDestino.FillSchema(dtDestino, SchemaType.Source)
        daDestino.Fill(dtDestino)

        Me.RevisarEstructuraTablas(dtDestino, dtOrigen)
        ' MsgBox("Consulta datos destino realizada! Filas = " & dtDestino.Rows.Count)
        ProcessedRows = dtOrigen.Rows.Count
        ProcessRows(dtOrigen, dtDestino)

        ' Se actualiza la base de datos dentro de una transaccion
        Dim nRegs As Integer = 0
        Dim nErrors As Integer = 0
        Try
            'Se aplican los cambios a la base de datos destino
            Destino.BeginTransaction()
            Destino.PrepareAdapter(daDestino)
            daDestino.ContinueUpdateOnError = m_dsConfig.General(0).IgnorarErrores
            daDestino.Update(dtDestino)
            'MsgBox("Se han aplicado los cambios a la tabja origen. Filas = " & ResultadoProceso.RegistrosImportados.ToString())

            ' Se determina si hay filas con errores
            Dim rowsInError As DataRow() = dtDestino.GetErrors()

            If rowsInError.Length > 0 Then
                'REVISAR LOS TIPOS DE DATOS
                For I As Integer = 0 To rowsInError.GetUpperBound(0)
                    If mensajeErrores.IndexOf(rowsInError(I).RowError) < 0 Then
                        mensajeErrores = mensajeErrores + vbCr + " Error procesando fila: " & rowsInError(I).RowError & vbCrLf
                    End If
                    'sMsg = "   Datos: " & rowsInError(I).ItemArray.ToString()
                    Me.RowsWithErros += 1
                    Me.ProcessedRows -= 1
                Next
            End If

            If Not DBUtils.IsNullOrEmpty(m_dsConfig.General(0).Item("ComandoFinalizacion")) Then
                Try
                    ' Ejecucion del Comando de Finalizacion de la tabla destino

                    Dim ComandoFinalizacion As String() = m_dsConfig.General(0).ComandoFinalizacion.Split()
                    Destino.OpenConnection()
                    If ComandoFinalizacion.Length > 1 Then
                        Destino.ExecuteNonQuery(CommandType.Text, m_dsConfig.General(0).ComandoFinalizacion)
                    Else
                        Destino.ExecuteNonQuery(CommandType.StoredProcedure, m_dsConfig.General(0).ComandoFinalizacion)
                    End If
                Finally
                    Destino.CloseConnection()
                End Try

            End If

            Destino.CommitTransaction()
        Catch ex As Exception
            Destino.RollbackTransaction()
            Throw ex
        End Try
    End Sub

    Private Sub RevisarEstructuraTablas(ByVal dtDestino As DataTable, ByVal dtOrigen As DataTable)
        Dim J As Integer
        For J = 0 To m_dsConfig.MapeoCampos.Rows.Count - 1
            ' Se comprueba que este definida la columna de destino
            If m_dsConfig.MapeoCampos(J).CampoDestino IsNot Nothing Then
                If m_dsConfig.MapeoCampos(J).CampoDestino <> "" Then
                    If m_dsConfig.MapeoCampos(J).CampoOrigen IsNot Nothing Then
                        If m_dsConfig.MapeoCampos(J).CampoOrigen <> "" Then
                            If dtDestino.Columns(m_dsConfig.MapeoCampos(J).CampoDestino).DataType.Name <> dtOrigen.Columns(m_dsConfig.MapeoCampos(J).CampoOrigen).DataType.Name Then
                                If mensajeErrores.IndexOf("La Columna " + dtDestino.Columns(m_dsConfig.MapeoCampos(J).CampoDestino).ColumnName + "Difiere en el tipo con la Columna Origen " + dtOrigen.Columns(m_dsConfig.MapeoCampos(J).CampoOrigen).DataType.ToString) < 0 Then
                                    mensajeErrores = mensajeErrores + " La Columna " + dtDestino.Columns(m_dsConfig.MapeoCampos(J).CampoDestino).ColumnName + "  " + dtDestino.Columns(m_dsConfig.MapeoCampos(J).CampoDestino).DataType.Name + " Difiere en el tipo con la Columna Origen " + dtOrigen.Columns(m_dsConfig.MapeoCampos(J).CampoOrigen).ColumnName + "   " + dtOrigen.Columns(m_dsConfig.MapeoCampos(J).CampoOrigen).DataType.Name + vbCr
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Next
    End Sub



    Private Function GetValorCampo(ByVal rowOrigen As DataRow, _
    ByVal rowMapeo As DBImportTaskDataSet.MapeoCamposRow, _
    ByVal dtDestino As DataTable) As Object
        Dim retVal As Object = DBNull.Value
        Dim bValorAsignado As Boolean = True

        If dtDestino.Columns(rowMapeo.CampoDestino).DataType IsNot dtDestino.Columns(rowMapeo.CampoDestino).DataType Then
            If mensajeErrores.IndexOf("La Columna " + dtDestino.Columns(rowMapeo.CampoDestino).ColumnName + "Difiere en el tipo con la Columna Origen " + dtDestino.Columns(rowMapeo.CampoDestino).DataType.ToString) < 0 Then
                mensajeErrores = mensajeErrores + " La Columna " + dtDestino.Columns(rowMapeo.CampoDestino).ColumnName + "Difiere en el tipo con la Columna Origen " + dtDestino.Columns(rowMapeo.CampoDestino).DataType.ToString
            End If
        End If

        If Not DBUtils.IsNullOrEmpty(rowMapeo("CampoOrigen")) Then
            ' Obtener el valor del campo origen
            If dtDestino.Columns(rowMapeo.CampoDestino).DataType Is GetType(System.String) Then
                If rowOrigen.IsNull(rowMapeo.CampoOrigen) Then
                    retVal = DBNull.Value
                ElseIf rowOrigen.Table.Columns(rowMapeo.CampoOrigen).DataType Is GetType(System.String) Then
                    retVal = Mid(CStr(rowOrigen(rowMapeo.CampoOrigen)), 1, _
                        dtDestino.Columns(rowMapeo.CampoDestino).MaxLength).Trim()
                ElseIf Not DBUtils.IsNullOrEmpty(rowMapeo("Formato")) Then
                    retVal = Mid(Format(rowOrigen(rowMapeo.CampoOrigen), rowMapeo.Formato), _
                        1, dtDestino.Columns(rowMapeo.CampoDestino).MaxLength).Trim()
                Else
                    retVal = Mid(CStr(rowOrigen(rowMapeo.CampoOrigen)), 1, _
                        dtDestino.Columns(rowMapeo.CampoDestino).MaxLength).Trim()
                End If
            Else
                If rowOrigen.Table.Columns(rowMapeo.CampoOrigen).DataType Is GetType(System.String) _
                AndAlso Not DBUtils.IsNullOrEmpty(rowMapeo("Formato")) Then
                    If dtDestino.Columns(rowMapeo.CampoDestino).DataType Is GetType(System.DateTime) Then
                        retVal = DateTime.ParseExact(DBUtils.ToString(rowOrigen(rowMapeo.CampoOrigen), String.Empty), rowMapeo.Formato, Nothing)
                    Else
                        ' TODO: Complementar manejo para otros tipos de datos
                        retVal = rowOrigen(rowMapeo.CampoOrigen)
                    End If
                Else
                    retVal = rowOrigen(rowMapeo.CampoOrigen)
                End If
            End If
        ElseIf Not DBUtils.IsNullOrEmpty(rowMapeo("ValorEstatico")) Then
            If dtDestino.Columns(rowMapeo.CampoDestino).DataType Is GetType(System.String) Then
                retVal = Mid(rowMapeo.ValorEstatico, 1, dtDestino.Columns(rowMapeo.CampoDestino).MaxLength)
            ElseIf dtDestino.Columns(rowMapeo.CampoDestino).DataType Is GetType(System.Boolean) Then
                retVal = CBool(rowMapeo.ValorEstatico)
            ElseIf dtDestino.Columns(rowMapeo.CampoDestino).DataType Is GetType(System.DateTime) Then
                If Not DBUtils.IsNullOrEmpty(rowMapeo("Formato")) Then
                    retVal = DateTime.ParseExact(rowMapeo.ValorEstatico, rowMapeo.Formato, Nothing)
                Else
                    retVal = CDate(rowMapeo.ValorEstatico)
                End If
            Else
                retVal = rowMapeo.ValorEstatico
            End If
        End If
        Return retVal
    End Function



    Private Sub ProcessRows(ByVal dtOrigen As DataTable, ByVal dtDestino As DataTable)

        ' Se llenan los datos del datatable de acuerdo al mapeo de campos
        Dim row As DataRow = Nothing
        Dim I, J, K As Integer
        Dim sCampoDestino As String
        Dim PrimaryKey(dtDestino.PrimaryKey.Length - 1) As Object

        ' TODO: Validar que los campos definidos en el mapeo si existan en las correspondientes tablas
        For I = 0 To dtOrigen.Rows.Count - 1

            ' Se arma un arreglo con los datos de la llave primaria
            If dtDestino.PrimaryKey.Length > 0 Then
                For J = 0 To PrimaryKey.Length - 1
                    For K = 0 To m_dsConfig.MapeoCampos.Rows.Count - 1
                        If m_dsConfig.MapeoCampos(K).CampoDestino.ToUpper() = _
                            dtDestino.PrimaryKey(J).ColumnName.ToUpper() Then
                            PrimaryKey(J) = GetValorCampo(dtOrigen.Rows(I), _
                                m_dsConfig.MapeoCampos(K), dtDestino)
                            Exit For
                        End If
                    Next
                Next
            End If

            ' Se busca si el registro ya existe en el datatable de destino, de lo
            ' contrario se crea una fila nueva
            row = Nothing
            Try
                row = dtDestino.Rows.Find(PrimaryKey)
            Catch
            End Try
            If row Is Nothing Then
                If Not m_dsConfig.General(0).InsertarFilasNuevas Then
                    Continue For
                End If
                row = dtDestino.NewRow()
            Else
                If Not m_dsConfig.General(0).ActualizarFilasActuales Then
                    Continue For
                End If
            End If

            ' Se asignan los campos en la fila de la tabla de destino de acuerdo al mapeo de campos
            For J = 0 To m_dsConfig.MapeoCampos.Rows.Count - 1
                ' Se comprueba que este definida la columna de destino
                If m_dsConfig.MapeoCampos(J).CampoDestino <> "" Then
                    sCampoDestino = m_dsConfig.MapeoCampos(J).CampoDestino
                    row(sCampoDestino) = GetValorCampo(dtOrigen.Rows(I), _
                        m_dsConfig.MapeoCampos(J), dtDestino)
                End If
            Next
            ' Si la fila es una fila nueva se agrega al datatable de destino
            If row.RowState = DataRowState.Detached Then
                dtDestino.Rows.Add(row)
            End If
        Next
    End Sub
End Class
