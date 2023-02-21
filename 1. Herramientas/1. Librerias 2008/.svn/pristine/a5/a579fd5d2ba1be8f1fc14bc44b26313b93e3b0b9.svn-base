Public Class DataTestForm

    Private Sub DataTestForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim a As New Desktop.Data.SqlClient.GestorDatosBase(My.Settings.FrameworkConnectionString())
        Dim conection As New SqlClient.SqlConnection(a.ConnectionString)

        Dim b As Integer

        b = a.ExecuteNonQuery(CommandType.StoredProcedure, "PRUEBA", 29, _
                        "a", "b", "c", _
                        "d", CDate("01/01/2010"), CDec("190"), CDec("222"), _
                        CDec("333"), True, CInt(88))

        Console.Write(b.ToString())
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim gestor As New Desktop.Data.GestorDatosBase("Data Source = '\Liquidos.SDF'")
        gestor.OpenConnection()

    End Sub
End Class