Imports Desktop.Data

Friend Class EditarUsuarioDialog

    Private Seguridad As GestorSeguridad
    Private m_RowUsuario As DataRow
    Private m_Nuevo As Boolean


    Public Shared Sub Run(ByVal Datos As GestorSeguridad)
        Dim frm As New EditarUsuarioDialog
        frm.Text = "Agregar usuario"
        frm.Seguridad = Datos
        frm.ShowDialog()
        frm.Dispose()
    End Sub


    Public Shared Sub Run(ByVal Datos As GestorSeguridad, ByVal rowUsuario As DataRow)
        Dim frm As New EditarUsuarioDialog
        frm.Text = "Editar usuario"
        frm.m_RowUsuario = rowUsuario
        frm.Seguridad = Datos
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub EditarUsuarioDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            dsSeguridad = Seguridad.dsSeguridad
            bsUsuarios.DataSource = dsSeguridad

            Me.MultiEmpresaCheckBox.Visible = FrameworkApp.MultiEmpresa

            Seguridad.LoadEmpresas()
            bsEmpresa.DataSource = dsSeguridad

            UsuarioTextBox.Focus()

            If m_RowUsuario Is Nothing Then
                Seguridad.LoadUsuarios()
                m_Nuevo = True
                bsUsuarios.AddNew()
                m_RowUsuario = CType(DBUtils.GetCurrentRow(bsUsuarios), DataRow)
                SetControls()
                UsuarioTextBox.ReadOnly = False
            Else
                m_Nuevo = False
                DBUtils.SetCurrentRow(bsUsuarios, m_RowUsuario)
                UsuarioTextBox.ReadOnly = True
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

    Private Sub Agregar()
        m_RowUsuario("Usuario") = UsuarioTextBox.Text.Trim()
        m_RowUsuario("Clave") = UsuarioTextBox.Text.Trim()
        m_RowUsuario("Nombre") = NombreTextBox.Text.Trim()
        m_RowUsuario("IdEmpresa") = CInt(IdEmpresaComboBox.SelectedValue)
        m_RowUsuario("Email") = EmailTextBox.Text.Trim()
        m_RowUsuario("Sistema") = False
        m_RowUsuario("SuperUser") = SuperUserCheckBox.Checked
        m_RowUsuario("MultiEmpresa") = MultiEmpresaCheckBox.Checked
        m_RowUsuario("FechaCreacion") = Date.Now
        m_RowUsuario("Activo") = ActivoCheckBox.Checked
        Me.dsSeguridad.Tables("Usuarios").Rows.Add(m_RowUsuario)
        Seguridad.UpdateUsuarios()
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Editar()
        bsUsuarios.EndEdit()

        Dim drUsuario As DataRow
        Dim rowUsuario As DataRow

        drUsuario = DBUtils.GetCurrentRow(bsUsuarios)
        rowUsuario = CType(drUsuario, DataRow)

        Seguridad.UpdateUsuarios()
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Not DBUtils.IsNullOrEmpty(UsuarioTextBox.Text) _
            And Not DBUtils.IsNullOrEmpty(NombreTextBox.Text) _
            And Not DBUtils.IsNullOrEmpty(EmailTextBox.Text) Then
                If m_Nuevo Then
                    If Not Seguridad.CheckUsuarioByUsuario(UsuarioTextBox.Text.Trim()) Then
                        Agregar()
                    Else
                        ShowError("El usuario ya existe")
                    End If
                Else
                    Editar()
                End If
            Else
                ShowError("Es necesario ingresar los campos obligatorios")
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub SetControls()
        MultiEmpresaCheckBox.Checked = False
        SuperUserCheckBox.Checked = False
        ActivoCheckBox.Checked = False
        UsuarioTextBox.Text = ""
        NombreTextBox.Text = ""
        EmailTextBox.Text = ""
        IdEmpresaComboBox.SelectedIndex = 0
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
    End Sub


End Class