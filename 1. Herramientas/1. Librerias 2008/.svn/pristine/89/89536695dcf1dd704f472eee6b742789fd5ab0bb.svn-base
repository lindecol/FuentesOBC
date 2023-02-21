Imports MovilidadCF.Symbol
Imports MovilidadCF.Windows.Forms


Public Class BarcodeForm2
    Private WithEvents m_Scanner As BarcodeReader

    Private Sub BarcodeForm2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        Threading.Thread.Sleep(2500)
        m_Scanner = New BarcodeReader()
        m_Scanner.StartRead(Me)
        UIHandler.EndWait()
    End Sub

    Private Sub m_Scanner_ScanComplete(ByVal e As Symbol.ScanCompleteArgs) Handles m_Scanner.ScanComplete
        lblDato.Text = e.Text
        lblSimbologia.Text = e.Type.ToString()
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub
End Class