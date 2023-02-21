Friend Class PerfilesUsuarioDialog

    Private Datos As GestorSeguridad
    Private RowUsuario As DataRow

    Public Class PerfilUsuario
        Public IdPerfil As Integer
        Public Nombre As String

        Public Sub New(ByVal IdPerfil As Integer, ByVal Nombre As String)
            Me.IdPerfil = IdPerfil
            Me.Nombre = Nombre
        End Sub

        Public Overrides Function ToString() As String
            Return Nombre
        End Function
    End Class

    Public Shared Sub Run(ByVal Datos As GestorSeguridad, ByVal rowUsuario As DataRow)
        Dim frm As New PerfilesUsuarioDialog
        frm.Datos = Datos
        frm.RowUsuario = rowUsuario
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub EditarUsuarioDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dsSeguridad = Datos.dsSeguridad
        bsUsuarios.DataSource = dsSeguridad
        DBUtils.SetCurrentRow(bsUsuarios, Me.RowUsuario)
        Datos.LoadPerfiles()
        Datos.LoadPerfilesUsuario(Me.RowUsuario)
        ShowPerfilesUsuario()
    End Sub

    Private Sub ShowPerfilesUsuario()
        For I As Integer = 0 To dsSeguridad.Tables("Perfiles").Rows.Count - 1
            If dsSeguridad.Tables("PerfilesUsuarios").Rows.Find( _
                New Object() {RowUsuario("IdUsuario"), dsSeguridad.Tables("Perfiles").Rows(I)("IdPerfil")}) Is Nothing Then
                lstPerfiles.Items.Add(New PerfilUsuario(CInt(dsSeguridad.Tables("Perfiles").Rows(I)("IdPerfil")), CStr(dsSeguridad.Tables("Perfiles").Rows(I)("Nombre"))), False)
            Else
                lstPerfiles.Items.Add(New PerfilUsuario(CInt(dsSeguridad.Tables("Perfiles").Rows(I)("IdPerfil")), CStr(dsSeguridad.Tables("Perfiles").Rows(I)("Nombre"))), True)
            End If
        Next
    End Sub

    Private Sub UpdatePerfilesUsuario()
        Dim Perfil As PerfilUsuario
        Dim rowPerfilUsuario As DataRow
        For I As Integer = 0 To lstPerfiles.Items.Count - 1
            Perfil = CType(lstPerfiles.Items(I), PerfilUsuario)
            rowPerfilUsuario = dsSeguridad.Tables("PerfilesUsuarios").Rows.Find( _
                    New Object() {RowUsuario("IdUsuario"), Perfil.IdPerfil})

            ' Se actualiza la tabla perfiles usuarios
            If lstPerfiles.CheckedItems.Contains(Perfil) Then
                ' Agregar si no existe
                If rowPerfilUsuario Is Nothing Then
                    rowPerfilUsuario = dsSeguridad.Tables("PerfilesUsuarios").NewRow
                    rowPerfilUsuario("IdPerfil") = Perfil.IdPerfil
                    rowPerfilUsuario("IdUsuario") = RowUsuario("IdUsuario")
                    dsSeguridad.Tables("PerfilesUsuarios").Rows.Add(rowPerfilUsuario)
                End If
            Else
                ' Borrar si existe
                If rowPerfilUsuario IsNot Nothing Then
                    rowPerfilUsuario.Delete()
                End If
            End If
        Next
        Datos.UpdatePerfilesUsuario()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Datos.BeginTransaction()
            UpdatePerfilesUsuario()
            Datos.CommitTransaction()
            DialogResult = DialogResult.OK
        Catch ex As Exception
            Datos.RollbackTransaction()
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        dsSeguridad.RejectChanges()
        DialogResult = DialogResult.Cancel
    End Sub
End Class