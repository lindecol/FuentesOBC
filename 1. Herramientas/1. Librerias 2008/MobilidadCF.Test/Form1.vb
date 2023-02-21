Imports MovilidadCF.Location
Imports MovilidadCF.Logging
Imports MovilidadCF.Windows.Forms
Imports MovilidadCF.Symbol
Imports MovilidadCF.Client
Imports MovilidadCF.Data



Public Class Form1
    Implements MovilidadCF.Data.DataSetSerializer.IEstadoCarga

    Private Sub btnGPSTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGPSTest.Click
        Try
            Dim dlg As New GPSCaptureDialog
            dlg.ShowDialog()
        Catch ex As Exception
            Logger.Write(ex)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        BarcodeContinousTriggerForm.Run()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        UIHandler.StartWait()
        BarcodeReader.InitReader()
        Dim frm As New BarcodeForm1
        UIHandler.ShowDialog(frm)
        frm.Dispose()
        BarcodeReader.EndReader()
        UIHandler.EndWait()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        Logger.Write("Mensaje de prueba")
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim cn As New Data.SqlServerCe.GestorDatosBase("Data Source=\FedeArroz.sdf")
        Dim s As New MovilidadCF.Data.DataSetSerializer(cn)
        Dim f As New IO.StreamReader("\1.txt", System.Text.Encoding.UTF8)
        Dim sDatos As String = f.ReadToEnd
        f.Close()
        Try
            Cursor.Current = Cursors.WaitCursor
            's.SaveCompressToDatabase(sDatos, True, Me, Nothing)
            MovilidadCF.Data.DataSetSerializer.ValorTrue = "1"
            MovilidadCF.Data.DataSetSerializer.ValorFalse = "0"

            s.SaveToDatabase(sDatos, True, Me, Nothing)
            MsgBox("Datos Procesados con Exito!!")
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Public ReadOnly Property Cancelado() As Boolean Implements Data.DataSetSerializer.IEstadoCarga.Cancelado
        Get

        End Get
    End Property

    Public Sub IniciarTabla(ByVal sNombreTabla As String) Implements Data.DataSetSerializer.IEstadoCarga.IniciarTabla

    End Sub

    Public WriteOnly Property ProgresoTabla() As Integer Implements Data.DataSetSerializer.IEstadoCarga.ProgresoTabla
        Set(ByVal value As Integer)

        End Set
    End Property

    Public WriteOnly Property ProgresoTotal() As Integer Implements Data.DataSetSerializer.IEstadoCarga.ProgresoTotal
        Set(ByVal value As Integer)

        End Set
    End Property

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim gestor As New MovilidadCF.Data.DataSetSerializer(New MovilidadCF.Data.SqlServerCe.GestorDatosBase("Data Source = \Liquidos.sdf"))
        Dim m_QueryList As New MovilidadCF.Data.DataSetSerializer.QueryList
        m_QueryList.Add("param", "SELECT COTIZACIONDOLAR from Parametros")

        If m_QueryList IsNot Nothing Then
            MovilidadCF.Data.DataSetSerializer.SeparadorDecimales = ","
            Dim m_sData As String = gestor.GetFromDatabase(m_QueryList, Me)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim gestor As New MovilidadCF.Data.SQLClient.GestorDatosBase("Data Source = \Liquidos.sdf")
        gestor.OpenConnection()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        MovilidadCF.Client.Nucleo.ConfigurarServicioWebDescarga("200.21.174.102", "80", "wsquickmobilepruebas", "", "", 90000)
        If MovilidadCF.Client.Nucleo.DescargarNuevaVersion("2", "v2") Then
            Me.Close()
        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        MovilidadCF.Client.Nucleo.ConfigurarServicioWeb("200.21.174.102", "80", "WSQuickMobile", "", "", 90000)
        MovilidadCF.Client.Nucleo.DescargarDatos("3", "3", "8", OpenNETCF.WindowsCE.DeviceManagement.GetDeviceID(), True, True, False, False, "Prueba", "", "Data Source = \AlceTransporte.sdf", Nothing)
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim file As New IO.StreamReader("\datos.txt", System.Text.Encoding.UTF8)
        Dim sDato As String = file.ReadToEnd
        file.Close()
        Dim rwEncriptar As System.IO.MemoryStream = MovilidadCF.Compression.DataCompression.Compress(sDato)
        sDato = MovilidadCF.Compression.DataCompression.UnCompress(rwEncriptar.GetBuffer, CInt(rwEncriptar.Length))

    End Sub
End Class

