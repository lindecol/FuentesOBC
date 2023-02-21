Friend Class LoginControl
    Public Event Login As EventHandler(Of LoginEventArgs)
    Public Event Cancel As EventHandler(Of EventArgs)


    Private Sub LoginControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtUsuario.Focus()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        IniciarSesion()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Dim arg As New EventArgs
        RaiseEvent Cancel(Me, arg)
    End Sub

    Private Sub txtClave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtClave.KeyPress
        If e.KeyChar = vbCr And txtClave.Text <> "" Then
            IniciarSesion()
        End If
    End Sub

    Private Sub IniciarSesion()
        If txtUsuario.Text = "" Or txtClave.Text = "" Then
            ShowError("Debe ingresar el usuario y contraseña")
            txtUsuario.Focus()
            Exit Sub
        End If
        Try
            FrameworkApp.Sesion.LoadUsuario(txtUsuario.Text)
            If FrameworkApp.Sesion.dsSesion.Tables("Usuarios").Rows.Count > 0 Then
                If CBool(FrameworkApp.Sesion.dsSesion.Tables("Usuarios").Rows(0)("Activo")) Then
                    If CStr(FrameworkApp.Sesion.dsSesion.Tables("Usuarios").Rows(0)("Clave")) = txtClave.Text Then
                        Dim args As New LoginEventArgs
                        RaiseEvent Login(Me, args)
                        txtUsuario.Text = ""
                        txtClave.Text = ""
                        Exit Sub
                    Else
                        ShowError("Nombre de usuario o clave incorrectas")
                    End If
                Else
                    ShowError("Usuario Inactivo")
                End If
            Else
                ShowError("Nombre de usuario o clave incorrectas")
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub
End Class