Imports PraxairComunicaciones
Imports MovilidadCF
Imports OpenNETCF
Imports System.Data
Imports OpenNETCF.Net


Public Class Comunicacion
    Private m_Rows() As DataRow
    Private Timeout As Integer = 100000
    Private Gprs As Boolean
    Private ListQuery As New MovilidadCF.Data.DataSetSerializer.QueryList

    Public Sub New()
        If Settings.ConexionGPRS.ToString().Trim = "" Then
            Gprs = False
        Else
            Gprs = True
        End If

    End Sub
    Public Function ejecutarCarga(ByVal Coneccion As String, ByVal Mensaje As String, ByVal TipoCarga As ProcesosComunicacion) As Boolean

        Dim oParametros As Object()
        NucleoCOM.ConfigurarServicioWeb(Settings.IPServidor, Settings.PuertoServidor.ToString(), Settings.VirtualDirectory, Settings.UsuarioWebService, Settings.ClaveWebService, Timeout, Settings.UsaHTTPS)
        Return NucleoCOM.CargarDatos(2, TipoCarga, Settings.IDTerminal, "", True, True, Gprs, True, Mensaje, Settings.ConexionGPRS, Coneccion, ListQuery, oParametros)

    End Function
    Public Function ejecutarDescarga(ByVal Coneccion As String, ByVal iTipoDescarga As String, ByVal TipoCarga As ProcesosComunicacion, ByVal sMensaje As String) As Boolean
        Try
            LoadQuerysDescarga(iTipoDescarga)
            Dim oParametros As Object()
            NucleoCOM.ConfigurarServicioWeb(Settings.IPServidor, Settings.PuertoServidor.ToString(), Settings.VirtualDirectory, Settings.UsuarioWebService, Settings.ClaveWebService, Timeout, Settings.UsaHTTPS)
            Return NucleoCOM.DescargarDatos(2, TipoCarga, Settings.IDTerminal, "", True, True, Gprs, True, sMensaje, Settings.ConexionGPRS, Coneccion, ListQuery, oParametros)
        Catch ex As Exception
            Return False
        End Try


    End Function


    Public Function EliminacionTablas(ByVal iTipoDescarga As String) As MovilidadCF.Data.DataSetSerializer.QueryList
        Try
            LoadQuerysDescarga(iTipoDescarga)
            Return ListQuery
        Catch ex As Exception
            Return Nothing
        End Try


    End Function

    Public Sub LoadQuerysDescarga(ByVal Tipo As String)

        ListQuery.Clear()
        If Tipo = "Parcial" Then
            ListQuery.Add("DetalleFactura", "SELECT * from DetalleFactura " & sqlFiltroEnviados())
            ListQuery.Add("HojasRuta", "HojasRuta")
            ListQuery.Add("KardexCamionLiquidos", "KardexCamion")
            ListQuery.Add("DetallePedidoLiquidos", "SELECT * from DetallePedido where (nopedido) in (select nopedido from MaestroFacturas " & sqlFiltroEnviados() & ")")
            ListQuery.Add("MaestroFacturas", "SELECT * from MaestroFacturas	 " & sqlFiltroEnviados())
            ListQuery.Add("Parametros_Descarga", "Parametros")
            ListQuery.Add("Precintos", "Precintos")
            ListQuery.Add("TalonariosDescarga", "Talonarios")
            ListQuery.Add("PedidosLiquidos", "SELECT * from Pedidos where (nopedido) in (select nopedido from MaestroFacturas " & sqlFiltroEnviados() & ")")
            ListQuery.Add("Guia_Documento", "SELECT * from Guia_Documento where transmitido_sn = 'N'")
        ElseIf Tipo = "CargueCamion" Then
            ListQuery.Add("KardexCamion", "KardexCamion")
            ListQuery.Add("Carga", "Carga")
        Else
            ListQuery.Add("DetalleFactura", "DetalleFactura")
            ListQuery.Add("HojasRuta", "HojasRuta")
            ListQuery.Add("KardexCamionLiquidos", "KardexCamion")
            ListQuery.Add("DetallePedidoLiquidos", "DetallePedido")
            ListQuery.Add("MaestroFacturas", "MaestroFacturas")
            ListQuery.Add("Parametros_Descarga", "Parametros")
            ListQuery.Add("Precintos", "Precintos")
            ListQuery.Add("TalonariosDescarga", "Talonarios")
            ListQuery.Add("PedidosLiquidos", "Pedidos")
            ListQuery.Add("Guia_Documento", "Guia_Documento")

        End If
    End Sub
    Private Function sqlFiltroEnviados() As String
        Return " where exists (select num_documento from Guia_Documento where transmitido_sn = 'N' and nofactura = num_documento and prefijo_documento =  PREFIJO and Tipo_Documento = TipoFactura )"
    End Function
End Class
