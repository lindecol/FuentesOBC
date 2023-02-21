Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports Desktop.Data.DBUtils


Public Class TextFileExportTaskDialog

    Private m_RowGeneral As TextFileExportTaskDataSet.GeneralRow
    Private m_NuevaTarea As Boolean
    Private m_Origen As GestorDatosBase
    Private m_Destino As GestorDatosBase
    Private m_dtTablasDestino As DataTable
    Private m_Task As TextFileExportTask
    Private m_dtOrigen As DataTable
    Private m_dtDestino As DataTable

    Public Shared Function Run(ByVal Task As TextFileExportTask) As Boolean
        Dim frm As New TextFileExportTaskDialog
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
            m_RowGeneral = CType(DBUtils.GetCurrentRow(bsGeneral), TextFileExportTaskDataSet.GeneralRow)
            m_RowGeneral.IncluirFilaTitulos = False
            m_RowGeneral.SeparadorCampos = ""
            m_RowGeneral.IgnorarErrores = True
            m_RowGeneral.UsaLongitudesFijas = False
            m_RowGeneral.SobreEscribir = True
            m_RowGeneral.IgnorarErrores = True
            m_RowGeneral.TieneImagenes = "False"
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
        dsConfig = CType(m_Task.dsConfig, Desktop.Data.Transformations.TextFileExportTaskDataSet)
        If dsConfig.Tables.Contains("MapeoCampos") Then
            If Not dsConfig.Tables("MapeoCampos").Columns.Contains("Orden") Then
                dsConfig.Tables("MapeoCampos").Columns.Add("Orden", Type.GetType("System.Int32"))
            End If
        End If
        bsGeneral.DataSource = dsConfig
        bsMapeoCampos.DataSource = dsConfig
        'revisar el Orden

        Dim numeroOrden As Integer = 0
        For Each mapeo As TextFileExportTaskDataSet.MapeoCamposRow In Me.dsConfig.Tables("MapeoCampos").Rows
            Try
                If IsDBNull(mapeo.Orden) Then
                    mapeo.Orden = numeroOrden
                    numeroOrden = numeroOrden + 1
                End If
            Catch
                mapeo.Orden = numeroOrden
                numeroOrden = numeroOrden + 1
            End Try
        Next
    End Sub

    'Private Sub LoadTablasDestino()
    '    Try
    '        m_Destino = GestorDatosFactory.CreateInstance(m_Task.Parent.ConnectionTypeName, m_Task.Parent.ConnectionString)
    '        m_Destino.OpenConnection()
    '        m_dtTablasDestino = m_Destino.GetTables()
    '        Dim dvTablasDestino As DataView = New DataView(m_dtTablasDestino)
    '        dvTablasDestino.Sort = "TABLE_NAME"
    '        For I As Integer = 0 To dvTablasDestino.Count - 1
    '            cbTablaDestino.Items.Add(dvTablasDestino(I)("TABLE_NAME"))
    '        Next
    '    Catch ex As Exception
    '        ShowError(ex.Message)
    '    Finally
    '        m_Destino.CloseConnection()
    '    End Try
    'End Sub

    Private Sub TabControl1_Deselecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.Deselecting
        If e.TabPage Is tpGeneral Then
            ' Se tratan de cargar los datos requeridos en los otros tabpages
            Try
                bsGeneral.EndEdit()
                LoadCamposOrigen()
                AjustarIUMapeo()
            Catch ex As Exception
                ShowError(ex.Message)
                e.Cancel = True
            End Try
        End If
    End Sub

    Private Sub AjustarIUMapeo()
        'If m_RowGeneral.UsaLongitudesFijas Then
        '    dgvcPosicion.HeaderText = "Pos. Ini"
        '    lblPosicion.Text = "Pos. Ini"
        'Else
        '    dgvcPosicion.HeaderText = "Campo No."
        '    lblPosicion.Text = "Campo No."
        'End If
    End Sub

    Private Sub CrearDestino()
        m_Destino = GestorDatosFactory.CreateInstance( _
            m_Task.Parent.ConnectionTypeName, _
            m_Task.Parent.ConnectionString)
    End Sub


    Private Sub LoadCamposOrigen()
        ' Se obtiene el esquema del datatable de destino
        CrearDestino()
        Dim daDestino As DbDataAdapter = m_Destino.CreateDataAdapter(dsConfig.General(0).ConsultaOrigen, False)

        m_dtDestino = New DataTable
        daDestino.FillSchema(m_dtDestino, SchemaType.Source)

        ' Se llenan los combobox que requieren la lista de campos
        cbCamposOrigen.Items.Clear()
        cbCamposOrigen.Items.Add("")
        For I As Integer = 0 To m_dtDestino.Columns.Count - 1
            cbCamposOrigen.Items.Add(m_dtDestino.Columns(I).ColumnName)
        Next
    End Sub


    Private Sub btnNuevoMapeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoMapeo.Click
        bsMapeoCampos.AddNew()
        cbCamposOrigen.Focus()
        'Dim rowMapeo As TextFileImportTaskDataSet.MapeoCamposRow
        'rowMapeo = DBUtils.GetCurrentRow(bsMapeoCampos)
        'rowMapeo.NoActualizar = False

    End Sub

    Private Sub btnGuardarMapeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarMapeo.Click
        Try
            If Not IsNullOrEmpty(GetCurrentRow(bsMapeoCampos)) Then
                If dsConfig.General(0).UsaLongitudesFijas And _
                                txtLongitud.Text.Trim() = "" Then
                    Throw New InvalidOperationException("Debe especificar la longitud del campo, al ser un archivo de longitudes fijas")
                End If
                If dsConfig.General(0).IncluirFilaTitulos And _
                    txtTitulo.Text.Trim() = "" Then
                    Throw New InvalidOperationException("Debe especificar el titulo del campo!")
                End If

                Dim mapeo As TextFileExportTaskDataSet.MapeoCamposRow

                mapeo = CType(GetCurrentRow(bsMapeoCampos), TextFileExportTaskDataSet.MapeoCamposRow)

                If cbCamposOrigen.Text = "" Then
                    mapeo.CampoOrigen = ""
                End If

                Dim orden As Object = Me.dsConfig.Tables("MapeoCampos").Compute("MAX(Orden)", "1=1")

                If orden Is Nothing Or IsDBNull(orden) Then
                    mapeo.Orden = Me.dsConfig.Tables("MapeoCampos").Rows.Count + 1
                Else
                    mapeo.Orden = CInt(orden) + 1
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
            bsGeneral.EndEdit()
            If Not dsConfig.General(0).UsaLongitudesFijas And _
                DBUtils.IsNullOrEmpty(dsConfig.General(0)("SeparadorCampos")) Then
                Throw New ArgumentException("El campo 'Separador Campos' no puede estar vacio si el archivo NO utiliza longitudes fijas")
            End If
            If TabControl1.TabPages.Count = 1 Then
                dsConfig.AcceptChanges()
                TabControl1.TabPages.Add(tpMapeoCampos)
                ShowAlert("Datos Almacenados!" & vbCrLf & _
                    "Por favor continue realizando el mapeo de campos...")
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
   
    Private Sub btnSubir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubir.Click
        Try
            If Not IsNullOrEmpty(GetCurrentRow(bsMapeoCampos)) Then
                Dim mapeo As TextFileExportTaskDataSet.MapeoCamposRow
                mapeo = CType(GetCurrentRow(bsMapeoCampos), TextFileExportTaskDataSet.MapeoCamposRow)

                For Each mapeos As TextFileExportTaskDataSet.MapeoCamposRow In dsConfig.MapeoCampos
                    If mapeos.Orden = mapeo.Orden - 1 Then
                        mapeos.Orden = mapeos.Orden + 1
                    End If
                Next

                If IsDBNull(mapeo.Orden) Then
                    mapeo.Orden = 1
                Else
                    If (mapeo.Orden > 1) Then
                        mapeo.Orden = mapeo.Orden - 1
                    End If
                End If
                bsMapeoCampos.EndEdit()
                dsConfig.AcceptChanges()
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnBajar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBajar.Click
        Try
            If Not IsNullOrEmpty(GetCurrentRow(bsMapeoCampos)) Then
                Dim mapeo As TextFileExportTaskDataSet.MapeoCamposRow
                mapeo = CType(GetCurrentRow(bsMapeoCampos), TextFileExportTaskDataSet.MapeoCamposRow)

                For Each mapeos As TextFileExportTaskDataSet.MapeoCamposRow In dsConfig.MapeoCampos
                    If mapeos.Orden = mapeo.Orden + 1 Then
                        mapeos.Orden = mapeos.Orden - 1
                    End If
                Next

                If IsDBNull(mapeo.Orden) Then
                    mapeo.Orden = Me.dsConfig.Tables("MapeoCampos").Rows.Count + 1
                Else
                    mapeo.Orden = mapeo.Orden + 1
                End If
                bsMapeoCampos.EndEdit()
                dsConfig.AcceptChanges()
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnVistaPrevia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVistaPrevia.Click
        Try
            CrearDestino()
            Dim dt As New DataTable
            m_Destino.Fill(dt, CommandType.Text, ConsultaOrigenTextBox.Text)
            DataPreviewDialog.Run(dt)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub
End Class