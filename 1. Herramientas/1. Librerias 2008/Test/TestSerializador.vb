Public Class TestSerializador

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ds As New DataSet
        Dim datos As New Desktop.Data.SqlClient.GestorDatosBase(My.Settings.ConnectionString)
        Dim dt As New DataTable("Funciones")
        datos.Fill(dt, CommandType.TableDirect, "Funciones")
        ds.Tables.Add(dt)
        Dim serializer As New Desktop.Data.DataSetSerializer()
        TextBox1.Text = Desktop.Data.DataSetSerializer.Serialize(ds)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim resultado As New DataSet()
        Desktop.Data.DataSetSerializer.FormatoFecha = "yyyy-MM-dd HH:mm:ss.F"
        Desktop.Data.DataSetSerializer.ValorFalse = "false"
        Desktop.Data.DataSetSerializer.ValorTrue = "true"
        Desktop.Data.DataSetSerializer.Unserialize(resultado, Me.TextBox1.Text)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

    End Sub
End Class