Imports system.Windows
Imports Desktop.Data

Friend Class SeleccionarEmpresaDialog
    Public Shared Function Run() As Boolean
        Dim bResult As Boolean = True
        If CBool(FrameworkApp.Sesion.UsuarioActual("MultiEmpresa")) Then
            FrameworkApp.Sesion.LoadEmpresas()
            If FrameworkApp.Sesion.dsSesion.Tables("Empresas").Rows.Count > 1 Then
                Dim frm As New SeleccionarEmpresaDialog
                bResult = (frm.ShowDialog() = Forms.DialogResult.OK)
                If bResult Then
                    FrameworkApp.Sesion.EmpresaActual = frm.EmpresaSeleccionada
                End If
            Else
                FrameworkApp.Sesion.EmpresaActual = FrameworkApp.Sesion.dsSesion.Tables("Empresas").Rows(0)
            End If
        Else
            FrameworkApp.Sesion.LoadEmpresaUsuarioActual()
        End If
        Return bResult
    End Function

    Private EmpresaSeleccionada As SesionDataset.EmpresasRow

    Private Sub SeleccionarEmpresaDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dsSesion = FrameworkApp.Sesion.dsSesion
        bsEmpresas.DataSource = dsSesion
        If FrameworkApp.Sesion.EmpresaActual Is Nothing Then
            DBUtils.SetCurrentRow(bsEmpresas, dsSesion.Tables("Empresas").Rows.Find(CInt(FrameworkApp.Sesion.UsuarioActual("IdEmpresa"))))
        Else
            DBUtils.SetCurrentRow(bsEmpresas, FrameworkApp.Sesion.EmpresaActual)
        End If
        NombreLabel1.Focus()



    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        EmpresaSeleccionada = CType(DBUtils.GetCurrentRow(bsEmpresas), SesionDataset.EmpresasRow)
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        EmpresaSeleccionada = Nothing
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub dgvEmpresa_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvEmpresa.KeyDown
        If e.KeyValue = Keys.Enter Then
            bsEmpresas.Position = bsEmpresas.Position
            EmpresaSeleccionada = CType(DBUtils.GetCurrentRow(bsEmpresas), SesionDataset.EmpresasRow)
            DialogResult = DialogResult.OK
        End If
        
    End Sub

End Class