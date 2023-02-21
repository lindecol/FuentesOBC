

Public Class CargaNovedadesGprsForm
    Implements IModuloPrograma

    Public Sub Run() Implements Common.IModuloPrograma.Run
        Dim OBJ As New Object
        Dim E As New System.EventArgs
        CargaNovedadesForm_Load(OBJ, E)
        Me.Dispose()
    End Sub

    Private Sub CargaNovedadesForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'envio gprs parcial
        'Dim Comunicacion As New comu

        Dim lstQuery As New ListQuerysDescarga
        Dim objPrograma As New Programa

        'objPrograma.LoadQuerysDescargaParcial(lstQuery)
        Nucleo.CargaNovedades()
        'Nucleo.CargarDatos(3, "Carga de Novedades", "Descarga datos")

    End Sub

    Private Sub menuCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCancelar.Click
        Me.Dispose()
    End Sub
End Class