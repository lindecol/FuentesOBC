Friend Class CambiarClaveDialog

    Public Seguridad As GestorSeguridad
    Private m_RowUsuario As DataRow
    Private m_Usuario As String = String.Empty

    Public Shared Sub Run(ByVal Datos As GestorSeguridad, ByVal RowUsuario As DataRow)
        Dim frm As New CambiarClaveDialog
        frm.Seguridad = Datos
        frm.m_RowUsuario = RowUsuario
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Public Shared Sub Run(ByVal sUsuario As String)
        Dim frm As New CambiarClaveDialog
        frm.m_Usuario = sUsuario
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub CambiarClaveDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Seguridad Is Nothing Then
            Seguridad = New GestorSeguridad
        End If
        dsSeguridad = Seguridad.dsSeguridad

        If m_Usuario = String.Empty Then
            bsUsuarios.DataSource = dsSeguridad
            DBUtils.SetCurrentRow(bsUsuarios, m_RowUsuario)
        Else
            Dim Row() As DataRow
            Dim RowUsuario As DataRow

            Seguridad.LoadUsuarios()
            bsUsuarios.DataSource = dsSeguridad

            Row = Seguridad.dsSeguridad.Tables("Usuarios").Select("Usuario = '" & m_Usuario & "'")
            RowUsuario = CType(Row(0), DataRow)

            DBUtils.SetCurrentRow(bsUsuarios, RowUsuario)

        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If Not DBUtils.IsNullOrEmpty(txtClaveNueva.Text.Trim()) And Not DBUtils.IsNullOrEmpty(txtConfirmacion.Text.Trim()) Then
            If txtClaveNueva.Text.Trim() = txtConfirmacion.Text.Trim() Then
                Editar()
            Else
                ShowError("La confirmación de la clave esta erronea, por favor verifique que la clave ingresada sea la misma en ambos cuadros de texto")
            End If
        Else
            ShowError("Es neceario ingresar la nueva clave con su respectiva confirmación")
        End If

    End Sub

    Private Sub Editar()
        bsUsuarios.EndEdit()

        Dim drUsuario As DataRow
        Dim rowUsuario As DataRow

        drUsuario = DBUtils.GetCurrentRow(bsUsuarios)
        rowUsuario = CType(drUsuario, DataRow)
        rowUsuario("Clave") = txtClaveNueva.Text.Trim()
        Seguridad.UpdateUsuarios()
        DialogResult = DialogResult.OK
    End Sub
End Class