Imports MovilidadCF.Symbol


Public Class BarcodeContinousTriggerForm

    Private WithEvents m_Scanner As New BarcodeReader()

    Public Shared Sub Run()
        Dim frm As New BarcodeContinousTriggerForm
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub BarcodeContinousTriggerForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        m_Scanner.StopRead()
        BarcodeReader.EndReader()
    End Sub

    Private Sub BarcodeContinousTriggerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BarcodeReader.InitReader()
        BarcodeReader.ContinousTrigger = True
        m_Scanner.StartRead(Me)
    End Sub

    Private Sub m_Scanner_ScanComplete(ByVal e As Symbol.ScanCompleteArgs) Handles m_Scanner.ScanComplete
        lblData.Text = e.Text
        lblType.Text = e.Type.ToString()
        lblLength.Text = e.Length.ToString()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub
End Class