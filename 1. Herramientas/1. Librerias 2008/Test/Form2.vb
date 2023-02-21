Public Class Form2

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim g As New prueba
        g.ProbarProcedimiento()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim g As New prueba
        g.ProbarSelect()
    End Sub
    Dim ds As New DataSet("ej")
    Dim dt As New DataTable("Ej")
    Private Sub Form2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        dt.Columns.Add("1")
        dt.Columns.Add("2")
        dt.Columns.Add("3")
        dt.PrimaryKey = New DataColumn() {dt.Columns(0)}
        dt.Rows.Add("1", "2", "Z")
        dt.Rows.Add("3", "4", "X")
        dt.Rows.Add("5", "6", "A")
        dt.Rows.Add("7", "8", "1")
        dt.Rows.Add("9", "10", ".")
        dt.Rows.Add("11", "12", "2")
        ds.Tables.Add(dt)
        Me.bsEjemplo.DataSource = ds
        Me.bsEjemplo.DataMember = "Ej"
        Me.bsEjemplo.Sort = "3"
        Me.DataGridView1.DataSource = Me.bsEjemplo
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim rw As DataRow = Desktop.Data.DBUtils.GetCurrentRow(Me.bsEjemplo)
        If rw IsNot Nothing Then
            rw.Delete()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim rwFind As DataRow = dt.Rows.Item(Me.TextBox1.Text)
        If rwFind IsNot Nothing Then
            Desktop.Data.DBUtils.SetCurrentRow(Me.bsEjemplo, rwFind)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim sTexto As String = "1"
        sTexto = sTexto + "1234" + "7891" + "111111"
        sTexto = sTexto + "01/10/2009" + "0101".PadRight(15)
        sTexto = sTexto + "Producto 1".PadLeft(255)
        sTexto = sTexto + "51644251".PadLeft(40)
        sTexto = sTexto + "213001000005".PadLeft(15)
        sTexto = sTexto + "1011" + "0001" + "10.2".PadLeft(17)
        sTexto = sTexto + "15.25".PadLeft(35)
        sTexto = sTexto + "12.2".PadLeft(23)
        sTexto = sTexto + "123"
        sTexto = sTexto + "LOTE".PadLeft(20)
        sTexto = sTexto + "01/01/2009"
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "1234"
        sTexto = sTexto + "ENTRADA".PadLeft(40)
        sTexto = sTexto + "TERCERO ENTRADA".PadLeft(40)
        sTexto = sTexto + "SUCURSAL E".PadLeft(15)
        sTexto = sTexto + "CENTRO C E".PadLeft(15)
        sTexto = sTexto + "CODIGO P E".PadLeft(15)
        sTexto = sTexto + "CODIGO A E".PadLeft(15)
        sTexto = sTexto + "5555"
        sTexto = sTexto + "555"
        sTexto = sTexto + "SERIE 1".PadLeft(20)


        Dim fArcEntrada As New IO.StreamWriter("C:\AntradaActivos.txt")
        fArcEntrada.WriteLine(sTexto)


        sTexto = "1"
        sTexto = sTexto + "1234" + "7891" + "222222"
        sTexto = sTexto + "01/10/2009" + "0101".PadRight(15)
        sTexto = sTexto + "Producto 2".PadLeft(255)
        sTexto = sTexto + "51644251".PadLeft(40)
        sTexto = sTexto + "213002000002".PadLeft(15)
        sTexto = sTexto + "1011" + "0001" + "10.2".PadLeft(17)
        sTexto = sTexto + "15.25".PadLeft(35)
        sTexto = sTexto + "12.2".PadLeft(23)
        sTexto = sTexto + "123"
        sTexto = sTexto + "LOTE".PadLeft(20)
        sTexto = sTexto + "01/01/2009"
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "010101".PadLeft(15)
        sTexto = sTexto + "1234"
        sTexto = sTexto + "ENTRADA".PadLeft(40)
        sTexto = sTexto + "TERCERO ENTRADA".PadLeft(40)
        sTexto = sTexto + "SUCURSAL E".PadLeft(15)
        sTexto = sTexto + "CENTRO C E".PadLeft(15)
        sTexto = sTexto + "CODIGO P E".PadLeft(15)
        sTexto = sTexto + "CODIGO A E".PadLeft(15)
        sTexto = sTexto + "5555"
        sTexto = sTexto + "555"
        sTexto = sTexto + "SERIE 2".PadLeft(20)

        fArcEntrada.WriteLine(sTexto)

        fArcEntrada.Close()

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim f As New IO.StreamReader("c:\PruebaPrax.txt")
        Dim sDatosEnviados As String = f.ReadToEnd
        f.Close()
        Dim dsGuardarDatos As DataSet = New DataSet()
        Desktop.Data.DataSetSerializer.SeparadorDecimales = "."
        Desktop.Data.DataSetSerializer.Unserialize(dsGuardarDatos, sDatosEnviados)
    End Sub

    Private fw As ICSharpCode.SharpZipLib.GZip.GZipInputStream = Nothing

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Dim rDatosComprimidos As System.IO.MemoryStream = Desktop.Compression.DataCompression.Compress("có")
        Dim sDataSetComprimido As String = Convert.ToBase64String(rDatosComprimidos.ToArray())
        MsgBox(sDataSetComprimido)

        Dim sDatosNormales As String = DescomprimirDatos(sDataSetComprimido)
        MsgBox(sDatosNormales)
        'Desktop.Compression.DataCompression.UnCompressText(Convert.FromBase64String(Me.TextBox3.Text), Convert.FromBase64String(Me.TextBox3.Text).Length)
        'MsgBox(Desktop.Compression.DataCompression.ReadLine())
    End Sub

    Public Function DescomprimirDatos(ByVal sDatosComprimidos As String) As String
        Dim rDatosEnviados As Byte() = Convert.FromBase64String(sDatosComprimidos)
        Return Desktop.Compression.DataCompression.UnCompress(rDatosEnviados, rDatosEnviados.Length)
    End Function


    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        'Dim dsDatosEnviados As DataSet = New DataSet("DatosEnviados")
        'dsDatosEnviados.ReadXml("C:\WorkSpace\Indupalma\Comunicacion\Documentos\04-02-10_15-07-01.036.txt")
        'Dim sDatosSerializados As String = Desktop.Data.DataSetSerializer.Serialize(dsDatosEnviados)

        Dim fArchivo As New IO.StreamReader("C:\Documents and Settings\lavila\Escritorio\Indupalma\Mantenimiento\PruebaDecimales.txt")
        Dim sDatosSerializados As String = fArchivo.ReadToEnd
        fArchivo.Close()
        Dim sDatosDescomprimidos As String = DescomprimirDatos(sDatosSerializados)
        Dim gD As New Desktop.Data.SqlClient.GestorDatosBase("Data Source=SRVDESARROLLO;Initial Catalog=IND_SANIDAD;User Id=sa;password=Desktop;")
        gD.ValorTrue = 1
        gD.ValorFalse = 0
        gD.FormatoFecha = "yyyyMMdd"
        gD.SeparadorDecimales = "."
        Dim ds As New DataSet

        'Desktop.Data.DataSetSerializer.ValorTrue = 1
        'Desktop.Data.DataSetSerializer.ValorFalse = 0
        'Desktop.Data.DataSetSerializer.FormatoFecha = "yyyy/MM/dd"
        Desktop.Data.DataSetSerializer.SeparadorDecimales = "."

        Desktop.Data.DataSetSerializer.Unserialize(ds, sDatosDescomprimidos)
        'Dim fa As New Desktop.Data.DataSetSerializer(gD)
        'fa.SaveCompressToDatabase(s, True)
        'fa.SaveToDatabase(sDatosSerializados, True, True)
        MsgBox("OK")
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim gD As New Desktop.Data.SqlClient.GestorDatosBase("Data Source=SRVDESARROLLO;Initial Catalog=IND_SANIDAD;User Id=sa;password=Desktop;")
        Dim sSQL As String = "select [atr-cod] AS CodAtributoEnfermedadPlaga,[ple-cod] AS CodPlagaEnfermedad,[ple-tip] AS Tipo,[atr-des] AS Descripcion from atributosenfermedadplaga where [suc-cod] = @Sucursal"
        Dim pr As IDataParameter() = Nothing
        Dim p1 As New SqlClient.SqlParameter("Sucursal", "09090")
        Dim lstParametros As List(Of IDataParameter) = New List(Of IDataParameter)
        lstParametros.Add(p1)
        pr = lstParametros.ToArray()
        Dim dt = New DataTable()
        gD.Fill(dt, CommandType.Text, sSQL, pr)
        sSQL = "select [est-neg-cod] AS EstructuraNegocio,[est-neg-cor] AS CodParcela,[ter-sin] AS SinonimoNit,[con-nro] AS Contrato,[lab-cod] AS Labor,[cla-cod] AS Clase,[suc-cod] AS Sucursal,[ter-nit] AS Nit,[con-val-uni] AS ValorUnitario,[con-val-sal] AS SaldoContrato from contratos where [suc-cod] = @Sucursal"
        pr = Nothing
        Dim p2 As New SqlClient.SqlParameter("Sucursal", "09090")
        Dim lstParametros2 As List(Of IDataParameter) = New List(Of IDataParameter)
        lstParametros2.Add(p2)
        pr = lstParametros2.ToArray()
        Dim dt2 = New DataTable()
        gD.Fill(dt2, CommandType.Text, sSQL, pr)
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Dim ds As New DataSet
        ds.Tables.Add("prueba")
        ds.Tables(0).Columns.Add("numero", Type.GetType("System.Decimal"))
        ds.Tables(0).Columns.Add("numero2", Type.GetType("System.Double"))
        Dim rw As DataRow = ds.Tables(0).NewRow
        rw(0) = 1.34
        rw(1) = 1.35
        ds.Tables(0).Rows.Add(rw)
        Dim rw2 As DataRow = ds.Tables(0).NewRow
        rw2(0) = 1.34
        rw2(1) = 1.35
        ds.Tables(0).Rows.Add(rw2)
        Desktop.Data.DataSetSerializer.SeparadorDecimales = "."

        Dim sRespuesta As String = Desktop.Data.DataSetSerializer.Serialize(ds)
    End Sub
End Class


Public Class prueba
    Inherits Desktop.Data.SqlClient.GestorDatosBase

    Public Sub New()
        MyBase.New("Data Source=SRVDESARROLLO;Initial Catalog=IND_COMUNICACIONES;User Id=sa;password=Desktop;")
    End Sub

    Public Sub ProbarProcedimiento()
        Try
            Dim dt As New DataTable
            Me.Fill(dt, CommandType.StoredProcedure, "INV_AcumuladoPorPruducto", 25, 23, "SHOW")
            MsgBox(dt.Rows.Count)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub ProbarSelect()
        Try
            Dim dt As New DataTable
            Me.Fill(dt, CommandType.Text, "SELECT * FROM Inv_ConteoDetalle WHERE IDEMPRESA = @P1", 25)
            MsgBox(dt.Rows.Count)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class