Imports System.Reflection

Public Class ExcuteProcessSQLDialog
    Private m_RowGeneral As ExecuteProcessDataSet.GeneralRow

    Public Shared Function Run(ByVal dsCustom As ExecuteProcessDataSet) As Boolean
        Dim frm As New ExcuteProcessSQLDialog
        Dim bRes As Boolean
        frm.dsExecuteProcess = dsCustom
        bRes = (frm.ShowDialog() = System.Windows.Forms.DialogResult.OK)
        frm.Dispose()
        Return bRes
    End Function

    Private Sub CustomTaskDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.bsEjecutarTarea.DataSource = Me.dsExecuteProcess.General
        cmbTipoConexion.DataSource = GestorDatosFactory.GetConnectionTypeNames()
        If dsExecuteProcess.General.Count = 0 Then
            ' Se agrega la tarea y se configuran los valores por defecto
            bsEjecutarTarea.AddNew()
            m_RowGeneral = CType(DBUtils.GetCurrentRow(bsEjecutarTarea), ExecuteProcessDataSet.GeneralRow)
            m_RowGeneral.CadenaConexion = ""
            m_RowGeneral.SentenciaSQL = ""
            m_RowGeneral.TipoSentencia = ""
            bsEjecutarTarea.ResetCurrentItem()
        Else
            ' Se muestra la tarea actual
            m_RowGeneral = dsExecuteProcess.General(0)
            bsEjecutarTarea.Position = 0
            Me.cmbTipoProceso.Text = m_RowGeneral.TipoSentencia
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Me.cmbTipoProceso.SelectedIndex < 0 Then
                MsgBox("Debe Seleccionar Un tipo de proceso!", MsgBoxStyle.Critical, "Validación")
                Exit Sub
            End If
            'If Me.CadenaConexionTextBox.Text.Equals(String.Empty) Then
            '    MsgBox("Debe ingresar la cadena de connexión!", MsgBoxStyle.Critical, "Validación")
            '    Exit Sub
            'End If
            If Me.txtConsultaOrigen.Text.Equals(String.Empty) Then
                MsgBox("Debe ingresar la sentencia de la consulta!", MsgBoxStyle.Critical, "Validación")
                Exit Sub
            End If
            bsEjecutarTarea.EndEdit()
            dsExecuteProcess.AcceptChanges()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        bsEjecutarTarea.CancelEdit()
        If m_RowGeneral.RowState = DataRowState.Added Then
            m_RowGeneral.Delete()
        End If
        dsExecuteProcess.RejectChanges()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

   
End Class