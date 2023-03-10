Imports System.Data.SqlServerCe
Imports System.IO
Imports OpenNETCF.Windows.Forms
Imports Oxigenos
Imports MovilidadCF.Windows.Forms

Public Class Programa
    Inherits ProgramaBase

    Public Overrides ReadOnly Property Codigo() As Integer
        Get
            Return 2
        End Get
    End Property

    Public Overrides ReadOnly Property Nombre() As String
        Get
            Return "Liquidos"
        End Get
    End Property

    Protected Overrides ReadOnly Property DatabaseFile() As String
        Get
            Return "\Liquidos.SDF"
        End Get
    End Property

    Public Overrides Sub CargarDatosIniciales()
        'Copiar label e imagen imprimir
        'KUXAN: 14/01/2020
        Dim original As String = Path.Combine(Application2.StartupPath, "Horizontal.lbl")
        MovilidadCF.Logging.Logger.Write(original)
        If File.Exists(original) Then
            Try
                File.Copy(original, "\Application\Horizontal.lbl", True)
            Catch ex As Exception
                MovilidadCF.Logging.Logger.Write(ex)
            End Try
        End If

        original = Path.Combine(Application2.StartupPath, "logoprax.jpg")
        MovilidadCF.Logging.Logger.Write(original)
        If File.Exists(original) Then
            Try
                File.Copy(original, "\logoprax.jpg", True)
            Catch ex As Exception
                MovilidadCF.Logging.Logger.Write(ex)
            End Try

        End If

        original = Path.Combine(Application2.StartupPath, "HojaRuta.lbl")
        MovilidadCF.Logging.Logger.Write(original)
        If File.Exists(original) Then
            Try
                File.Copy(original, "\Application\HojaRuta.lbl", True)
            Catch ex As Exception
                MovilidadCF.Logging.Logger.Write(ex)
            End Try
        End If

    End Sub

    Public Overrides Sub CreateDatabase()
        Dim original As String = Path.Combine(Application2.StartupPath, "Liquidos.SDF")

        MovilidadCF.Logging.Logger.Write(original)
        If File.Exists(original) Then
            Try
                File.Copy(original, DatabaseFile, True)
            Catch ex As Exception
                MovilidadCF.Logging.Logger.Write(ex)
            End Try
        End If

      
    End Sub

    Protected Overrides Sub InternalCheckDatabase()
        ' TODO: Implementar validaciones adicionales en la base de datos
    End Sub

    Public Overrides Sub LoadInfoModulosPrograma()
        m_lstModulosPrograma.AddModuloPrograma("Inicio Ruta", IconosModulos.InicioRuta, GetType(InicioRutaForm), Nothing)
        m_lstModulosPrograma.AddModuloNucleo(ModulosNucleo.Rutero, "Rutero", Nothing)
        m_lstModulosPrograma.AddModuloPrograma("Reportes", IconosModulos.Impresora, GetType(ReportesForm), Nothing)
        m_lstModulosPrograma.AddModuloPrograma("Envio Datos GPRS", IconosModulos.DescargaDatos, GetType(EnvioDatosGprsForm), Nothing)
        m_lstModulosPrograma.AddModuloPrograma("Carga Novedades GPRS", IconosModulos.CargaDatos, GetType(CargaNovedadesGprsForm), Nothing)
        m_lstModulosPrograma.AddModuloPrograma("Fin de Ruta", IconosModulos.FinRuta, GetType(FinRutaForm), Nothing)
        ' TODO: Agregar otros m?dulos que se muestran despu?s del rutero
    End Sub

    Public Overrides Sub LoadInfoModulosCliente(ByVal rowCliente As System.Data.DataRow)

    End Sub

    Public Overrides Sub LoadInfoModulosRutero()
        'm_lstModulosRutero.AddModuloPrograma("Documentos Manuales", GetType(DocumentosManualesForm), System.Windows.Forms.Keys.D)
    End Sub


    Public Overrides Sub LoadInfoModulosPedidos(ByVal rowCliente As System.Data.DataRow)
        m_lstModulosPedido.AddModuloPrograma("&Venta", GetType(VentaForm), System.Windows.Forms.Keys.V)
    End Sub

    Public Overrides ReadOnly Property UseQueryFilterInRutero() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property FilterQueryClientes() As String
        Get
            Try
                Return String.Format("WHERE Clientes.ESTADOCLI = '01' AND EXISTS (SELECT CodigoCliente FROM ClienteTanque " & _
                    "WHERE ( ClienteTanque.ESTADOTANQUE != 'B' AND ClienteTanque.ProductoTanque IN (SELECT codhijo " & _
                    "FROM productosasociados WHERE (codpadre = '{0}')) OR " & _
                    "ClienteTanque.ProductoTanque = '{0}') AND (ClienteTanque.CodigoCliente = Clientes.Codigo)) ", _
                    HojasRuta.HojaActual.CodProducto)
            Catch
                Return ""
            End Try
        End Get
    End Property

    Public Overrides Sub LoadQuerysDescarga(ByVal lstQuerys As Common.ListQuerysDescarga)
        lstQuerys.Add("DetalleFactura", "DetalleFactura")
        lstQuerys.Add("HojasRuta", "HojasRuta")
        lstQuerys.Add("KardexCamionLiquidos", "KardexCamion")
        lstQuerys.Add("DetallePedidoLiquidos", "DetallePedido")
        lstQuerys.Add("MaestroFacturas", "MaestroFacturas")
        lstQuerys.Add("Parametros_Descarga", "Parametros")
        lstQuerys.Add("Precintos", "Precintos")
        lstQuerys.Add("TalonariosDescarga", "Talonarios")
        lstQuerys.Add("PedidosLiquidos", "Pedidos")
        lstQuerys.Add("Guia_Documento", "Guia_Documento")
    End Sub

    Public Overrides Sub LoadQuerysDescargaParcial(ByVal lstQuerys As Common.ListQuerysDescarga)
        lstQuerys.Add("DetalleFactura", "SELECT * from DetalleFactura " & sqlFiltroEnviados())
        lstQuerys.Add("HojasRuta", "HojasRuta")
        lstQuerys.Add("KardexCamionLiquidos", "KardexCamion")
        lstQuerys.Add("DetallePedidoLiquidos", "SELECT * from DetallePedido where (nopedido) in (select nopedido from MaestroFacturas " & sqlFiltroEnviados() & ")")
        lstQuerys.Add("MaestroFacturas", "SELECT * from MaestroFacturas	 " & sqlFiltroEnviados())
        lstQuerys.Add("Parametros_Descarga", "Parametros")
        lstQuerys.Add("Precintos", "Precintos")
        lstQuerys.Add("TalonariosDescarga", "Talonarios")
        lstQuerys.Add("PedidosLiquidos", "SELECT * from Pedidos where (nopedido) in (select nopedido from MaestroFacturas " & sqlFiltroEnviados() & ")")
        lstQuerys.Add("Guia_Documento", "SELECT * from Guia_Documento where transmitido_sn = 'N'")

    End Sub

    Private Function sqlFiltroEnviados() As String
        Return " where exists (select num_documento from Guia_Documento where transmitido_sn = 'N' and nofactura = num_documento and prefijo_documento =  PREFIJO )"
    End Function

    Public Overrides Function ValidarInicioRutero(ByRef sMensaje As String) As Boolean

        'valida la fecha de la pocket
        If Today() < Nucleo.FechaCarga Then
            sMensaje = "La fecha de la terminal es inferior a la fecha de carga!!" & vbCrLf & "Fecha de la terminal: " & Today() & vbCrLf & "Fecha de Carga: " & Nucleo.FechaCarga
            UIHandler.EndWait()
            Return False
        End If

        ' Se realizan las validaciones necesarias
        HojasRuta.LoadHojaRutaActual()
        If HojasRuta.dsHojasRuta.HojasRuta.Count > 0 Then
            If HojasRuta.dsHojasRuta.HojasRuta(0).Estado = EstadosHojaRuta.Creada Then
                sMensaje = "La hoja de ruta no ha sido iniciada, no se puede iniciar el rutero!"
                Return False
            End If
        Else
            sMensaje = "No se ha creado la hoja de ruta, no se puede iniciar el rutero"
            Return False
        End If

        ' Se carga la informaci?n compartida que ser? utilizada
        Productos.LoadProducto(HojasRuta.dsHojasRuta.HojasRuta(0).CodProducto)
        Productos.LoadExistencias(HojasRuta.dsHojasRuta.HojasRuta(0).IdHojaRuta, _
            HojasRuta.dsHojasRuta.HojasRuta(0).CodProducto)
        Return True
    End Function

    Public Overrides Function CerrarAtencionPedido(ByVal RowCliente As System.Data.DataRow, ByVal RowPedido As System.Data.DataRow) As Boolean
        Return True
    End Function

    Public Overrides Sub InicioAtencionPedido(ByVal RowCliente As System.Data.DataRow, ByVal RowPedido As System.Data.DataRow)

    End Sub

End Class
