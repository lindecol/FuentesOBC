Public Class UsuarioView

    Private Seguridad As New GestorSeguridad

    Protected Enum Permisos As Integer
        Perfiles = 1
    End Enum

    Protected Overrides Sub LoadPermisosEspeciales()
        m_PermisosEspeciales.Add(New PermisoEspecial( _
        Permisos.Perfiles, "Permite editar los perfiles"))
    End Sub


    Private Sub UsuarioView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.dsSeguridad = Seguridad.dsSeguridad
        Me.bsUsuarios.DataSource = Me.dsSeguridad
        Seguridad.LoadEmpresas()
        Seguridad.LoadUsuarios()
        AjustarSeguridad()
    End Sub

    Private Sub AjustarSeguridad()
        If Not PermiteBorrar Then
            btnEliminarUsuario.Enabled = False
        End If

        If Not PermiteEditar Then
            btnEditarUsuario.Enabled = False
        End If

        If Not PermiteInsertar Then
            btnAgregarUsuario.Enabled = False
        End If

        If Not PermitePermisoEspecial(Permisos.Perfiles) Then
            btnPerfilesUsuario.Enabled = False
        End If
    End Sub

    Private Sub btnPerfilesUsuario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPerfilesUsuario.Click
        If bsUsuarios.Count > 0 Then
            Dim rowUsuario As DataRow
            rowUsuario = CType(DBUtils.GetCurrentRow(bsUsuarios), DataRow)
            PerfilesUsuarioDialog.Run(Seguridad, rowUsuario)
        End If
    End Sub
    Private Sub btnEditarUsuario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarUsuario.Click
        Dim Row As DataRow
        Dim Rowusuario As DataRow
        Row = DBUtils.GetCurrentRow(bsUsuarios)
        Rowusuario = CType(Row, DataRow)
        EditarUsuarioDialog.Run(Seguridad, Rowusuario)
    End Sub

    Private Sub btnAgregarUsuario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarUsuario.Click
        EditarUsuarioDialog.Run(Seguridad)

		Seguridad.dsSeguridad.Usuarios.Rows.Clear()

		Seguridad.LoadUsuarios()
		AjustarSeguridad()
    End Sub
    Private Sub btnEliminarUsuario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarUsuario.Click
        Try

            Dim drUsuario As DataRow
            Dim RowUsuario As DataRow

            drUsuario = DBUtils.GetCurrentRow(bsUsuarios)
            RowUsuario = CType(drUsuario, DataRow)

            If drUsuario IsNot Nothing Then


                If Confirmar("Esta seguro de borrar el usuario seleccionado?") Then

                    'If Not Contabilidad.CheckMovimientosByIdUsuario(Nucleo.EmpresaActual.IdEmpresa, RowUsuario.IdUsuario) Then
                    If Not Seguridad.CheckPerfilByIdUsuario(CInt(RowUsuario("IdUsuario"))) Then
                        bsUsuarios.RemoveCurrent()
                        Seguridad.UpdateUsuarios()
                    Else
                        ShowError("El usuario tiene perfiles relacionados, por lo tanto no se puede eliminar")
                    End If
                    'Else
                    'ShowError("El usuario tiene movimientos relacionados en el modulo de contabilidad, por lo tanto no se puede eliminar")
                    'End If
                End If
            End If

        Catch ex As Exception
            ShowError(ex.Message)
            Seguridad.LoadUsuarios()
        End Try
    End Sub
    Private Sub btnCambiarClave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCambiarClave.Click
        Dim Row As DataRow
        Dim Rowusuario As DataRow
        Row = DBUtils.GetCurrentRow(bsUsuarios)
        Rowusuario = CType(Row, DataRow)
        CambiarClaveDialog.Run(Seguridad, Rowusuario)
    End Sub
End Class
