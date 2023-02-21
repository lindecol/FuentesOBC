Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports Desktop.Data.DBUtils

Public Class TextFileImportTaskDialog

    Private m_RowGeneral As TextFileImportTaskDataSet.GeneralRow
    Private m_NuevaTarea As Boolean
    Private m_Origen As GestorDatosBase
    Private m_Destino As GestorDatosBase
    Private m_dtTablasDestino As DataTable
    Private m_Task As TextFileImportTask
    Private m_dtOrigen As DataTable
    Private m_dtDestino As DataTable

    Public Shared Function Run(ByVal Task As TextFileImportTask) As Boolean
        Dim frm As New TextFileImportTaskDialog
        Dim bRes As Boolean
        frm.m_NuevaTarea = False
        frm.m_Task = Task
        bRes = (frm.ShowDialog() = System.Windows.Forms.DialogResult.OK)
        frm.Dispose()
        Return bRes
    End Function

    Private Sub DBImportTaskDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitData()
        If dsConfig.General.Count = 0 Then
            ' Se agrega la tarea y se configuran los valores por defecto
            bsGeneral.AddNew()
            m_RowGeneral = CType(DBUtils.GetCurrentRow(bsGeneral), TextFileImportTaskDataSet.GeneralRow)
            m_RowGeneral.InsertarFilasNuevas = True
            m_RowGeneral.ActualizarFilasActuales = True
            m_RowGeneral.IgnorarErrores = True
            m_RowGeneral.UsaLongitudesFijas = False
            m_RowGeneral.ContieneFilaTitulos = False
            m_RowGeneral.IgnorarErrores = True
            bsGeneral.ResetCurrentItem()
            TabControl1.TabPages.Remove(tpMapeoCampos)
        Else
            ' Se muestra la tarea actual
            m_RowGeneral = dsConfig.General(0)
            bsGeneral.Position = 0
            bsGeneral.ResetCurrentItem()
        End If
    End Sub

    Private Sub InitData()
        dsConfig = CType(m_Task.dsConfig, Desktop.Data.Transformations.TextFileImportTaskDataSet)
        bsGeneral.DataSource = dsConfig
        bsMapeoCampos.DataSource = dsConfig
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
                bsGeneral.EndEdit()
                LoadCamposDestino()
                AjustarIUMapeo()
            Catch ex As Exception
                ShowError(ex.Message)
                e.Cancel = True
            End Try
        End If
    End Sub

    Private Sub AjustarIUMapeo()
        If m_RowGeneral.UsaLongitudesFijas Then
            dgvcPosicion.HeaderText = "Pos. Ini"
            lblPosicion.Text = "Pos. Ini"
        Else
            dgvcPosicion.HeaderText = "Campo No."
            lblPosicion.Text = "Campo No."
        End If
    End Sub

    Private Sub CrearDestino()
        m_Destino = GestorDatosFactory.CreateInstance( _
            m_Task.Parent.ConnectionTypeName, _
            m_Task.Parent.ConnectionString)
    End Sub

    
    Private Sub LoadCamposDestino()
        ' Se obtiene el esquema del datatable de destino
        CrearDestino()
        Dim daDestino As DbDataAdapter = m_Destino.CreateDataAdapter()
        daDestino.SelectCommand = m_Destino.CreateCommand()
        daDestino.SelectCommand.Connection = m_Destino.Connection
        daDestino.SelectCommand.CommandType = CommandType.Text
        daDestino.SelectCommand.CommandText = "SELECT * FROM " & dsConfig.General(0).TablaDestino
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
        'Dim rowMapeo As TextFileImportTaskDataSet.MapeoCamposRow
        'rowMapeo = DBUtils.GetCurrentRow(bsMapeoCampos)
        'rowMapeo.NoActualizar = False

    End Sub

    Private Sub btnGuardarMapeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarMapeo.Click
        Try
            If Not IsNullOrEmpty(GetCurrentRow(bsMapeoCampos)) Then
                Dim mapeo As TextFileImportTaskDataSet.MapeoCamposRow

                mapeo = CType(GetCurrentRow(bsMapeoCampos), TextFileImportTaskDataSet.MapeoCamposRow)

                If cbCamposDestino.Text = "" Then
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
        dsConfig.RejectChanges()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub gboxDefinicionMapeo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gboxDefinicionMapeo.Leave
        bsMapeoCampos.CancelEdit()
        bsMapeoCampos.ResetCurrentItem()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
			' Se validan los datos
			If txtSeparadorMiles.Text.Trim = "" Then
				txtSeparadorMiles.Text = "."
			End If

			If txtSeparadorDecimales.Text.Trim = "" Then
				txtSeparadorDecimales.Text = "."
			End If

            bsGeneral.EndEdit()
            If Not dsConfig.General(0).UsaLongitudesFijas And _
                DBUtils.IsNullOrEmpty(dsConfig.General(0)("SeparadoresCampos")) Then
                Throw New ArgumentException("El campo 'Separadores Campos' no puede estar vacio si el archivo no utiliza longitudes fijas")
            End If
            If TabControl1.TabPages.Count = 1 Then
                If cbTablaDestino.Text <> "" Then
                    TabControl1.TabPages.Add(tpMapeoCampos)
                    ShowAlert("Datos Almacenados!" & vbCrLf & _
                        "Por favor continue realizando el mapeo de campos...")
                Else
                    ShowError("Debe seleccionar la tabla destino para poder pasar al mapeo de los campos")
                End If
            Else
                bsMapeoCampos.EndEdit()
                dsConfig.AcceptChanges()
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
            End If
        End If
    End Sub

    Private Sub btnObtenerTablasDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnObtenerTablasDestino.Click
        LoadTablasDestino()
    End Sub


    Private Sub btnBuscarArchivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarArchivo.Click
        If dlgOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            txtCadenaConexion.Text = dlgOpenFile.FileName
        End If
    End Sub

    Private Sub chkNumero_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkvalidarNumero.CheckedChanged
        If chkvalidarNumero.Checked Then
            lblInicioNumerico.Visible = True
            txtInicioNumerico.Visible = True
            lblLongitudNumerico.Visible = True
            txtLongitudNumerico.Visible = True
        Else
            lblInicioNumerico.Visible = False
            txtInicioNumerico.Visible = False
            lblLongitudNumerico.Visible = False
            txtLongitudNumerico.Visible = False
        End If
    End Sub
End Class