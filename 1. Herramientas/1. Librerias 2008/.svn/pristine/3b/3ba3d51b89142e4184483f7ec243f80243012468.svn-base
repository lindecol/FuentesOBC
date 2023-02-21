Friend Class PerfilesUsuariosView

    Private Datos As New GestorSeguridad

    Private Sub PerfilesUsuariosView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.dsSeguridad = Datos.dsSeguridad
        Me.bsPerfiles.DataSource = Me.dsSeguridad
        Datos.LoadPerfiles()
        AjustarSeguridad()
    End Sub

    Private Sub AjustarSeguridad()
        If Not PermiteBorrar Then
            btnEliminar.Enabled = False
        End If

        If Not PermiteEditar Then
            btnEditar.Enabled = False
        End If

        If Not PermiteInsertar Then
            btnAgregar.Enabled = False
        End If

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        EditarPerfilDialog.Nuevo(Datos)
    End Sub

    Private Sub btnEditarModulo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim rowPerfil As DataRow
        If bsPerfiles.Count > 0 Then
            rowPerfil = CType(DBUtils.GetCurrentRow(bsPerfiles), DataRow)
            EditarPerfilDialog.Editar(Datos, rowPerfil)
        End If
    End Sub

    Private Sub btnEliminarModulo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            Dim rowPerfil As DataRow
            If bsPerfiles.Count > 0 Then
                If Confirmar("Esta seguro de eliminar el perfil seleccionado?") Then
                    rowPerfil = CType(DBUtils.GetCurrentRow(bsPerfiles), DataRow)
                    rowPerfil.Delete()
                    Datos.UpdatePerfiles()
                End If
            End If
        Catch ex As Exception
            ShowError(ManagerException.Exception(ex))
        End Try
        
    End Sub
End Class
