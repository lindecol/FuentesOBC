Imports Desktop.Data.PAFClient

Public Class PAFClientTest

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim c As New PAFConnection()
        c.PrimaryHost = txtServidor.Text
        c.PrimaryPort = txtPuerto.Text

        ' Actor Historial Novedades
        Try
			Dim a As New PAFActor(CInt(txtActor.Text), c)
			a.UseDataSizeHeader = chkEncabezado.Checked
            txtRespuesta.Text = a.ExecuteQuery(txtParametros.Text)
            lblLineas.Text = txtRespuesta.Lines.Length
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        


      
    End Sub

    Private Sub PAFClientTest_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub
End Class