Imports MovilidadCF.Data.PAFClient
Imports System.Data
Imports OpenNETCF

Public Class PAFClientTest

    Private Sub btnEjecutar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEjecutar.Click
        Dim tiempoInicial As Integer = Environment2.TickCount
        Dim tiempoFinal As Integer = 0
        Dim con As New PAFConnection()
        con.PrimaryHost = txtServer.Text
        con.PrimaryPort = CInt(txtPort.Text)
        ' Actor Historial Novedades
        Try
            Dim a As New PAFActor(CInt(txtActor.Text), con)
            txtResultados.Text = a.ExecuteQuery(txtParametros.Text)
            tiempoFinal = Environment2.TickCount
            lblTiempoRespuesta.Text = (tiempoFinal - tiempoInicial).ToString()
        Catch ex As Exception
            MsgBox(ex.Message)
            lblTiempoRespuesta.Text = ""
        End Try
    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        txtServer.Text = "192.168.147.23"
        txtActor.Text = "10"
        txtParametros.Text = "770209002300"
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click
        txtServer.Text = "192.168.147.35"
        txtActor.Text = "22"
        txtParametros.Text = "101|091103"
    End Sub
End Class