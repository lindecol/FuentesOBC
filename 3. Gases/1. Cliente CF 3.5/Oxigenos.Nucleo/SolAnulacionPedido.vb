Imports Oxigenos.Principal.GestorAnulacionPedidos
Imports MovilidadCF.Windows.Forms

Public Class SolAnulacionPedido
    Private _idpedido As String
    Private _nomcliente As String

    Public Shared Sub Run(ByVal idpedido As String, ByVal NomCliente As String)
        UIHandler.StartWait()
        Dim form As New SolAnulacionPedido
        form._idpedido = idpedido
        form._nomcliente = NomCliente
        UIHandler.ShowDialog(form)
        UIHandler.EndWait()
        form.Dispose()
    End Sub


    Private Sub SolAnulacionPedido_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.UiHandler1.Parent = Me
        Me.lblpedido.Text = Me._idpedido
        Me.lblcliente.Text = Me._nomcliente
        Try
            GestorAnulaPedidos = New GestorAnulacionPedidos()
            Me.bsMotivosAnulacion.DataSource = GestorAnulaPedidos.dsAnulacionPedidos
        Catch

        End Try
        Me.bsMotivosAnulacion.DataSource = GestorAnulaPedidos.obtenerMotivos()


        UIHandler.EndWait()
    End Sub

    Private Sub btnsolanula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsolanula.Click
        GestorAnulaPedidos = New GestorAnulacionPedidos()
        If Not GestorAnulaPedidos.crearSolicitudDeAnulacion(_idpedido, Me.cmbmotivos.SelectedValue.ToString()) Then
            UIHandler.ShowError("No fue posible solicitar la anulaci�n del pedido, comuniquese con el administrador del sistema!")
            Exit Sub
        Else
            MsgBox("Solicitud de Anulaci�n Registrada con �xito y Pendiente de Confirmaci�n por Prax", MsgBoxStyle.Information, "Anulaci�n")
            UIHandler.StartWait()
            DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub btncan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub mnuRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRegresar.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub
End Class