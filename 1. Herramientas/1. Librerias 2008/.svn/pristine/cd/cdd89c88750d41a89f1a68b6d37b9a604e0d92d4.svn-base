Imports MovilidadCF.Symbol
Imports MovilidadCF.Windows.Forms

Public Class BarcodeForm1

    Private WithEvents m_Scanner As BarcodeReader

    Private Sub BarcodeForm1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BarcodeReader.InitReader()
        UiHandler1.Parent = Me
        m_Scanner = New BarcodeReader()
        m_Scanner.StartRead(Me)
        UIHandler.EndWait()
    End Sub

    Private Sub m_Scanner_ScanComplete(ByVal e As Symbol.ScanCompleteArgs) Handles m_Scanner.ScanComplete
        lblDato.Text = e.Text
        lblSimbologia.Text = e.Type.ToString()
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        'UIHandler.StartWait()
        m_Scanner.StopRead()
        BarcodeReader.EndReader()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnNuevoFormulario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoFormulario.Click
        UIHandler.StartWait()
        m_Scanner.StopRead()
        Dim frm As New BarcodeForm2
        UIHandler.ShowDialog(frm)
        frm.Dispose()
        m_Scanner.StartRead(Me)
        UIHandler.EndWait()
    End Sub
End Class