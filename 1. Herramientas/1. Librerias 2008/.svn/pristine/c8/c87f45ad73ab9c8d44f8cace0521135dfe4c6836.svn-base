Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports System.ComponentModel

<Description("Tarea exportación de base de datos")> _
Public Class DBExportTask
    Inherits TaskBase

    Private m_dsConfig As DBExportTaskDataSet
    Private m_SourceConnectionString As String
    Private m_SourceConnectionType As String
    Private m_TargetConnectionString As String
    'Private m_TargetConnectionType

    Public Overrides ReadOnly Property dsConfig() As System.Data.DataSet
        Get
            If m_dsConfig Is Nothing Then
                m_dsConfig = New DBExportTaskDataSet
            End If
            Return m_dsConfig
        End Get
    End Property

    Public Overrides Function ShowConfigDialog() As Boolean
        Return DBExportTaskDialog.Run(Me)
    End Function

    Public Overrides Sub Execute(ByVal pc As DataTransformation.IProgressController)
        Dim Destino As GestorDatosBase
        Dim Origen As GestorDatosBase
        Dim sSqlDestino As New System.Text.StringBuilder

        ' Se valida que este la configuración básica
        If m_dsConfig.General.Rows.Count = 0 _
        OrElse m_dsConfig.MapeoCampos.Rows.Count = 0 Then
            Throw New InvalidOperationException( _
                "Tarea Exportación datos no configurada correctamente.")
        End If

        ' Se crean los gestores de conexion a la base datos
        Origen = GestorDatosFactory.CreateInstance( _
            Parent.ConnectionTypeName, Parent.ConnectionString)
        Destino = GestorDatosFactory.CreateInstance( _
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

		Dim daDestino As DbDataAdapter
		' Se consulta la tabla destino
		If m_dsConfig.General(0).TipoConexion.ToUpper = "ODBC" Then
			If m_dsConfig.General(0).TablaDestino.StartsWith("[") Then
				daDestino = Destino.CreateDataAdapter("SELECT * FROM " & m_dsConfig.General(0).TablaDestino)
			Else
				daDestino = Destino.CreateDataAdapter("SELECT * FROM [" & m_dsConfig.General(0).TablaDestino & "]")
			End If
		Else
			daDestino = Destino.CreateDataAdapter("SELECT * FROM " & m_dsConfig.General(0).TablaDestino)
		End If

		Dim dtDestino As New DataTable
		daDestino.FillSchema(dtDestino, SchemaType.Source)

		If m_dsConfig.General(0).TipoConexion.ToUpper = "ODBC" Then
			dtDestino.TableName = "[" & dtDestino.TableName & "]"
		End If

		daDestino.Fill(dtDestino)

		' MsgBox("Consulta datos destino realizada! Filas = " & dtDestino.Rows.Count)
		ProcessedRows = dtOrigen.Rows.Count
		ProcessRows(dtOrigen, dtDestino)

		' Se actualiza la base de datos dentro de una transaccion
		Dim nRegs As Integer = 0
		Dim nErrors As Integer = 0
		Try
			'Se aplican los cambios a la base de datos destino
			If m_dsConfig.General(0).TipoConexion.ToUpper <> "ODBC" Then
				Destino.BeginTransaction()
			End If

			Destino.PrepareAdapter(daDestino)
			If m_dsConfig.General(0).TipoConexion.ToUpper = "ODBC" Then
				Dim insert As String = daDestino.InsertCommand.CommandText

				Dim cadena As String = m_dsConfig.General(0).TablaDestino.Replace("[", "").Replace("]", "")

				daDestino.InsertCommand.CommandText = insert.Replace(cadena, m_dsConfig.General(0).TablaDestino)

				If daDestino.UpdateCommand IsNot Nothing Then
					Dim update As String = daDestino.UpdateCommand.CommandText
					daDestino.UpdateCommand.CommandText = update.Replace(cadena, m_dsConfig.General(0).TablaDestino)
				End If
			End If

			daDestino.ContinueUpdateOnError = m_dsConfig.General(0).IgnorarErrores

			daDestino.Update(dtDestino)

			'MsgBox("Se han aplicado los cambios a la tabja origen. Filas = " & ResultadoProceso.RegistrosExportados.ToString())

			' Se determina si hay filas con errores
			Dim rowsInError As DataRow() = dtDestino.GetErrors()
			Dim sMsg As String
			If rowsInError.Length > 0 Then
				For I As Integer = 0 To rowsInError.GetUpperBound(0)
					sMsg = "Error procesando fila: " & rowsInError(I).RowError & vbCrLf
					sMsg = "   Datos: " & rowsInError(I).ItemArray.ToString()

					Me.RowsWithErros = Me.RowsWithErros + 1
					Me.ProcessedRows = Me.ProcessedRows - 1
				Next
			End If

			If Not DBUtils.IsNullOrEmpty(m_dsConfig.General(0).Item("ComandoFinalizacion")) Then
				' Ejecucion del Comando de Finalizacion de la tabla destino
				Dim ComandoFinalizacion As String() = m_dsConfig.General(0).ComandoFinalizacion.Split()

				If ComandoFinalizacion.Length > 1 Then
					Destino.ExecuteNonQuery(CommandType.Text, m_dsConfig.General(0).ComandoFinalizacion)
				Else
					Destino.ExecuteNonQuery(CommandType.StoredProcedure, m_dsConfig.General(0).ComandoFinalizacion)
				End If
			End If

			If m_dsConfig.General(0).TipoConexion.ToUpper <> "ODBC" Then
				Destino.CommitTransaction()
			End If
		Catch ex As Exception
			Destino.RollbackTransaction()
			Throw ex
		End Try
	End Sub

    Private Function GetValorCampo(ByVal rowOrigen As DataRow, _
    ByVal rowMapeo As DBExportTaskDataSet.MapeoCamposRow, _
    ByVal dtDestino As DataTable) As Object
        Dim retVal As Object = DBNull.Value
        Dim bValorAsignado As Boolean = True

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
