
Public Class Form2

    Private WithEvents _bar As MovilidadCF.Symbol.BarcodeReader
    Private _ar As ArchivoError

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Eliminar.Click
        _ar.Buscar(Me.TextBox2.Text)
    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _ar = New ArchivoError
        _bar = New MovilidadCF.Symbol.BarcodeReader
        MovilidadCF.Symbol.BarcodeReader.InitReader()
        _bar.StartRead()
    End Sub

    Private Sub _bar_ScanComplete(ByVal e As Symbol.ScanCompleteArgs) Handles _bar.ScanComplete
        If Me.TextBox1.Focused Then
            Me.TextBox1.Text = e.Text
            _ar.Insertar(e.Text, e.Type.ToString)
        Else
            Me.TextBox2.Text = e.Text
            _ar.DeleteReg(e.Text)
        End If

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Form2_Closed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        _bar.StopRead()
        MovilidadCF.Symbol.BarcodeReader.EndReader()

    End Sub
End Class