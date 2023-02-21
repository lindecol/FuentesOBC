Imports System.Reflection

Public Class CustomTaskDialog
    Private m_RowGeneral As CustomImporTaskDataSet.GeneralRow

    Public Shared Function Run(ByVal dsCustom As CustomImporTaskDataSet) As Boolean
        Dim frm As New CustomTaskDialog
        Dim bRes As Boolean
        frm.dsCustomImporTask = dsCustom
        bRes = (frm.ShowDialog() = System.Windows.Forms.DialogResult.OK)
        frm.Dispose()
        Return bRes
    End Function

    Private Sub CustomTaskDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CargarDatos()
        If dsCustomImporTask.General.Count = 0 Then
            ' Se agrega la tarea y se configuran los valores por defecto
            bsEnsamblado.AddNew()
            m_RowGeneral = CType(DBUtils.GetCurrentRow(bsEnsamblado), CustomImporTaskDataSet.GeneralRow)
            m_RowGeneral.UbicacionEnsamblado = ""
            m_RowGeneral.Clase = ""
            m_RowGeneral.Metodo = ""
            bsEnsamblado.ResetCurrentItem()
            Me.pnInformacionEnsamblado.Enabled = False
        Else
            ' Se muestra la tarea actual
            m_RowGeneral = dsCustomImporTask.General(0)
            bsEnsamblado.Position = 0
            'bsEnsamblado.ResetCurrentItem()
            If Not m_RowGeneral.IsUbicacionEnsambladoNull Then
                If m_RowGeneral.UbicacionEnsamblado <> String.Empty Then
                    Me.LoadClasesMetodos(m_RowGeneral.UbicacionEnsamblado)
                End If
            Else
                Me.pnInformacionEnsamblado.Enabled = False
            End If
            Me.cmbClase.SelectedValue = m_RowGeneral.Clase
            Me.cmbMetodos.SelectedValue = m_RowGeneral.Metodo
        End If
    End Sub

    Private Sub CargarDatos()
        Me.bsClase.DataSource = Me.dsCustomImporTask.Clase
        Me.bsMetodo.DataSource = Me.bsClase
        Me.bsEnsamblado.DataSource = Me.dsCustomImporTask.General
    End Sub

    Private Sub LoadClasesMetodos(ByVal sFileName As String)
        If Not IO.File.Exists(sFileName) Then
            Exit Sub
        End If
        Me.dsCustomImporTask.Clase.Clear()
        Me.dsCustomImporTask.Metodo.Clear()
        Dim objAssembly As Assembly
        Dim objType, objTypes() As System.Type
        objAssembly = Assembly.LoadFile(sFileName)
        objTypes = objAssembly.GetTypes()
        Dim iMetodo As Int16 = 0
        For Each objType In objTypes
            Me.dsCustomImporTask.Clase.AddClaseRow(objType.FullName)
            iMetodo = 0
            For Each Metodo As MethodInfo In objType.GetMethods()
                iMetodo = iMetodo + CShort(1)
                Dim rwMetodo As CustomImporTaskDataSet.MetodoRow
                rwMetodo = Me.dsCustomImporTask.Metodo.NewMetodoRow()
                rwMetodo.NombreClase = objType.FullName
                rwMetodo.Firma = Metodo.Name
                rwMetodo.Consecutivo = iMetodo
                Me.dsCustomImporTask.Metodo.AddMetodoRow(rwMetodo)
            Next
        Next
        Me.pnInformacionEnsamblado.Enabled = True
    End Sub


    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Me.cmbClase.SelectedIndex < 0 Then
                MsgBox("Debe Seleccionar Una clase del ensamblado!", MsgBoxStyle.Critical, "Validación")
                Exit Sub
            End If
            If Me.cmbMetodos.SelectedIndex < 0 Then
                MsgBox("Debe Seleccionar Un metodo de la clase!", MsgBoxStyle.Critical, "Validación")
                Exit Sub
            End If
            bsEnsamblado.EndEdit()
            dsCustomImporTask.AcceptChanges()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        bsEnsamblado.CancelEdit()
        If m_RowGeneral.RowState = DataRowState.Added Then
            m_RowGeneral.Delete()
        End If
        dsCustomImporTask.RejectChanges()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If Me.opEnsamblado.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Me.txtEnsamblado.Text = Me.opEnsamblado.FileName
        End If
    End Sub

    Private Sub btnCargarEnsamblado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargarEnsamblado.Click
        Me.LoadClasesMetodos(Me.txtEnsamblado.Text)
    End Sub

End Class