
Public Class EnvioDatosGprsForm
    Implements IModuloPrograma

    Public Sub Run() Implements Common.IModuloPrograma.Run
        Dim OBJ As New Object
        Dim E As New System.EventArgs
        EnvioDatosGprsForm_Load(OBJ, E)
        Me.Dispose()
    End Sub

    Private Sub EnvioDatosGprsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'envio gprs parcial

        Dim lstQuery As New ListQuerysDescarga
        Dim objPrograma As New Programa
        'objPrograma.LoadQuerysDescargaParcial(lstQuery)
        'Nucleo.DescargarDatos(2, "Descarga datos", True, lstQuery)
        Nucleo.DescargarDatos(2, "Descarga datos", "Parcial")


    End Sub

    Private Sub menuCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCancelar.Click
        Me.Dispose()
    End Sub
End Class