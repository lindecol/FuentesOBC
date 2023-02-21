Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports Desktop.Data.DBUtils

Public Class DBImportTaskDialog

    Private m_RowGeneral As DBImportTaskDataSet.GeneralRow
    Private m_NuevaTarea As Boolean
    Private m_Origen As GestorDatosBase
    Private m_Destino As GestorDatosBase
    Private m_dtTablasDestino As DataTable
    Private m_Task As DBImportTask
    Private m_dtOrigen As DataTable
    Private m_dtDestino As DataTable

    Public Shared Function Run(ByVal Task As DBImportTask) As Boolean
        Dim frm As New DBImportTaskDialog
        Dim bRes As Boolean
        frm.m_NuevaTarea = False
        frm.m_Task = Task
        bRes = (frm.ShowDialog() = System.Windows.Forms.DialogResult.OK)
        frm.Dispose()
        Return bRes
    End Function

    Private Sub DBImportTaskDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitData()
        If dsTask.General.Count = 0 Then
            ' Se agrega la tarea y se configuran los valores por defecto
            bsGeneral.AddNew()
            m_RowGeneral = CType(DBUtils.GetCurrentRow(bsGeneral), DBImportTaskDataSet.GeneralRow)
            m_RowGeneral.InsertarFilasNuevas = True
            m_RowGeneral.ActualizarFilasActuales = True
            m_RowGeneral.IgnorarErrores = True
            bsGeneral.ResetCurrentItem()
            TabControl1.TabPages.Remove(tpMapeoCampos)
        Else
            ' Se muestra la tarea actual
            m_RowGeneral = dsTask.General(0)
            bsGeneral.Position = 0
            bsGeneral.ResetCurrentItem()
        End If
    End Sub

    Private Sub InitData()
        dsTask = CType(m_Task.dsConfig, Desktop.Data.Transformations.DBImportTaskDataSet)
        bsGeneral.DataSource = dsTask
        bsMapeoCampos.DataSource = dsTask
        cbTipoConexion.DataSource = GestorDatosFactory.GetConnectionTypeNames()
    End Sub

    Private Sub LoadTablasDestino()
        Try
            m_Destino = GestorDatosFactory.CreateInstance(m_Task.Parent.ConnectionTypeName, m_Task.Parent.ConnectionString)
            m_Destino.OpenConnection()
            m_dtTablasDestino = m_Destino.GetTables()
            Dim dvTablasDestino As DataView = New DataView(m_dtTablasDestino)
            dvTablasDestino.Sort = "TABLE_NAME"

            cbTablaDestino.Items.Clear()

            For I As Integer = 0 To dvTablasDestino.Count - 1
                cbTablaDestino.Items.Add(dvTablasDestino(I)("TABLE_NAME"))
            Next
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            m_Destino.CloseConnection()
        End Try
    End Sub

    Private Sub TabControl1_Deselecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.Deselecting
        If e.TabPage Is tpGeneral Then
            ' Se tratan de cargar los datos requeridos en los otros tabpages
            Try
                CrearOrigen()
                LoadCamposOrigen()
                LoadCamposDestino()
            Catch ex As Exception
                ShowError(ex.Message)
                e.Cancel = True
            End Try
        End If
    End Sub

    Private Sub CrearOrigen()
        m_Origen = GestorDatosFactory.CreateInstance( _
            dsTask.General(0).TipoConexion, _
            dsTask.General(0).CadenaConexion)
    End Sub

    Private Sub CrearDestino()
        m_Destino = GestorDatosFactory.CreateInstance( _
            m_Task.Parent.ConnectionTypeName, _
            m_Task.Parent.ConnectionString)
    End Sub

    Private Sub LoadCamposOrigen()
        ' Se obtiene el esquema del datatable de origen
        CrearOrigen()
        Dim daOrigen As DbDataAdapter = m_Origen.CreateDataAdapter()
        daOrigen.SelectCommand = m_Origen.CreateCommand()
        daOrigen.SelectCommand.Connection = m_Origen.Connection
        daOrigen.SelectCommand.CommandType = CommandType.Text
        'daOrigen.SelectCommand.CommandText = dsTask.General(0).ConsultaOrigen
        daOrigen.SelectCommand.CommandText = ProcesarConsulta(dsTask.General(0).ConsultaOrigen.ToUpper())

        m_dtOrigen = New DataTable
        daOrigen.Fill(m_dtOrigen)
        'daOrigen.FillSchema(m_dtOrigen, SchemaType.Source)

        ' Se llenan los combobox que requieren la lista de campos
        cbCamposOrigen.Items.Clear()
        cbCamposOrigen.Items.Add("")
        For I As Integer = 0 To m_dtOrigen.Columns.Count - 1
            cbCamposOrigen.Items.Add(m_dtOrigen.Columns(I).ColumnName)
        Next
    End Sub

    Private Sub LoadCamposDestino()
        ' Se obtiene el esquema del datatable de destino
        CrearDestino()
        Dim daDestino As DbDataAdapter = m_Destino.CreateDataAdapter()
        daDestino.SelectCommand = m_Destino.CreateCommand()
        daDestino.SelectCommand.Connection = m_Destino.Connection
        daDestino.SelectCommand.CommandType = CommandType.Text
        daDestino.SelectCommand.CommandText = "SELECT * FROM " & dsTask.General(0).TablaDestino
        m_dtDestino = New DataTable
        daDestino.FillSchema(m_dtDestino, SchemaType.Source)

        ' Se llenan los combobox que requieren la lista de campos
        cbCamposDestino.Items.Clear()
        cbCamposDestino.Items.Add("")
        For I As Integer = 0 To m_dtDestino.Columns.Count - 1
            cbCamposDestino.Items.Add(m_dtDestino.Columns(I).ColumnName)
        Next
    End Sub


    Private Sub btnNuevoMapeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoMapeo.Click
        bsMapeoCampos.AddNew()
        cbCamposDestino.Focus()
    End Sub

    Private Sub btnGuardarMapeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarMapeo.Click
        Try
            If Not IsNullOrEmpty(GetCurrentRow(bsMapeoCampos)) Then
                Dim mapeo As DBImportTaskDataSet.MapeoCamposRow

                mapeo = CType(GetCurrentRow(bsMapeoCampos), DBImportTaskDataSet.MapeoCamposRow)

                If cbCamposOrigen.Text = "" Then
                    If cbCamposDestino.Text = "" Then
                        ShowError("debe seleccionar campo origen o destino")
                    End If
                    mapeo.CampoOrigen = ""
                ElseIf cbCamposDestino.Text = "" Then
                    mapeo.CampoDestino = ""
                End If

                bsMapeoCampos.EndEdit()
                bsMapeoCampos.ResetCurrentItem()
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        bsGeneral.CancelEdit()
        bsMapeoCampos.CancelEdit()
        If m_RowGeneral.RowState = DataRowState.Added Then
            m_RowGeneral.Delete()
        End If
        dsTask.RejectChanges()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub gboxDefinicionMapeo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gboxDefinicionMapeo.Leave
        bsMapeoCampos.CancelEdit()
        bsMapeoCampos.ResetCurrentItem()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            bsGeneral.EndEdit()
            If TabControl1.TabPages.Count = 1 Then
                dsTask.AcceptChanges()
                If cbTablaDestino.Text <> "" Then
                    TabControl1.TabPages.Add(tpMapeoCampos)
                    ShowAlert("Datos Almacenados!" & vbCrLf & _
                        "Por favor continue realizando el mapeo de campos...")
                Else
                    ShowError("Debe seleccionar la tabla destino para poder pasar al mapeo de los campos")
                End If
            Else
                bsMapeoCampos.EndEdit()
                dsTask.AcceptChanges()
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnBorrarMapeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrarMapeo.Click
        If bsMapeoCampos.Count > 0 Then
            If Confirmar("Borrar mapeo seleccionado?") Then
                bsMapeoCampos.RemoveCurrent()
                bsMapeoCampos.EndEdit()
                bsMapeoCampos.ResetCurrentItem()
            End If
        End If
    End Sub

    Private Sub btnBorrarMapeos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrarMapeos.Click
        If bsMapeoCampos.Count > 0 Then
            If Confirmar("Esta seguro de borrar TODOS los mapeos configurados?") Then
                Dim i As Integer = 0
                Dim cantidadMapeos As Integer = bsMapeoCampos.Count

                While i < cantidadMapeos
                    bsMapeoCampos.RemoveCurrent()
                    bsMapeoCampos.EndEdit()
                    bsMapeoCampos.ResetCurrentItem()
                    i += 1
                End While
                'bsMapeoCampos.Clear()
                'dsTask.MapeoCampos.AcceptChanges()
            End If
        End If
    End Sub
  
    Private Sub btnObtenerTablasDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnObtenerTablasDestino.Click
        LoadTablasDestino()
    End Sub
 
    Private Sub btnVistaPrevia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVistaPrevia.Click
        Try
            CrearOrigen()
            Dim dt As New DataTable
            m_Origen.Fill(dt, CommandType.Text, txtConsultaOrigen.Text)
            DataPreviewDialog.Run(dt)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Funcion que agrega la condicion 1=0 al where de la consulta SQL que se desea mapear, para que retorne el esquema sin datos
    ''' </summary>
    ''' <param name="Consulta">consulta SQL original</param>
    ''' <returns>consulta SQL procesada</returns>
    ''' <remarks></remarks>
    Private Function ProcesarConsulta(ByVal Consulta As String) As String
        Dim PosicionOrderBy As Integer = 0
        Dim PosicionGroupBy As Integer = 0
        Dim Posicion As Integer = 0

        If Consulta.Contains("ORDER BY") Then
            PosicionOrderBy = Consulta.IndexOf("ORDER BY")
        End If

        If Consulta.Contains("GROUP BY") Then
            PosicionGroupBy = Consulta.IndexOf("GROUP BY")
        End If

        If PosicionGroupBy <> 0 Then
            If PosicionOrderBy <> 0 Then
                If PosicionGroupBy < PosicionOrderBy Then
                    Posicion = PosicionGroupBy
                Else
                    Posicion = PosicionOrderBy
                End If
            Else
                Posicion = PosicionGroupBy
            End If
        ElseIf PosicionOrderBy <> 0 Then
            Posicion = PosicionOrderBy
		ElseIf Consulta.Contains("WHERE") Then
			Dim partesConsulta As String()
			partesConsulta = Consulta.Split(ChrW(13))

			Consulta = partesConsulta(0)

			For i As Integer = 1 To partesConsulta.Length - 1
				If partesConsulta(i).Contains("WHERE") Then
					partesConsulta(i) = partesConsulta(i).Replace("WHERE", "WHERE 1=0 AND ")
				End If
				Consulta += partesConsulta(i)
			Next
        End If

        Dim primeraParteConsulta As String = ""
        Dim segundaParteConsulta As String = ""

        If Posicion <> 0 Then
			primeraParteConsulta = Consulta.Substring(0, Posicion)

			If primeraParteConsulta.Contains("WHERE") Then
				primeraParteConsulta += " AND 1=0"
			Else
				primeraParteConsulta += " WHERE 1=0"
			End If
			segundaParteConsulta = Consulta.Substring(Posicion, (Consulta.Length - Posicion))



			Consulta = primeraParteConsulta + " " + segundaParteConsulta
		End If

		Return Consulta
    End Function
End Class