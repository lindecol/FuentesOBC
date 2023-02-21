' Interfaz que permite a las libreria de aplicación utilizar funcionalidad
' Implementada en el nucleo
Public Interface INucleo

    ReadOnly Property CodigoEmpresa() As String

    ReadOnly Property CodigoPuntoVenta() As String

    ReadOnly Property NitPraxair() As String

    ReadOnly Property CodigoSucursal() As String

    ReadOnly Property NitTrasportadora() As String

    ReadOnly Property CodigoVehiculo() As String

    ReadOnly Property CodigoTerminal() As String

    Property KilometrajeInicial() As String

    Property KiloMetrajeFinal() As String

    Property FechaCarga() As DateTime

    Property PedidoActual() As Integer

    Property NumeroMovimiento() As String

    ReadOnly Property CodigoChofer() As String

    ReadOnly Property CodigoTrasportadora() As String

    ReadOnly Property RutaPrincipal() As String

    Property ConsecutivoDepositos() As String

    Property ConsecutivoAsignaciones() As String

    Property ConsecutivoAlquileres() As String

    ReadOnly Property ProductoCopago() As String

    ReadOnly Property ProductoCuota() As String

    ReadOnly Property RowParametros() As DataRow

    Function GetInfoModulo(ByVal Modulos As ModulosNucleo) As InfoModulo

    Function GetTalonarioActual(ByVal CodTipoDocumento As Integer) As DataRow

    Sub UpdateTalonario(ByVal rowTalonario As DataRow)

    Function CargarDatos(ByVal IdProceso As Integer, ByVal sNombreProceso As String, _
        ByVal bUpdateCurrentRows As Boolean, ByVal bUsarGPRS As Boolean) As Boolean

    Function DescargarDatos(ByVal IdProceso As Integer, ByVal sNombreProceso As String, ByVal Tipo As String) As Boolean

    Function CargaNovedades() As Boolean

    Function Imprimir(ByVal Document As PrinterDocument, ByVal DatosFirma As DatosCapturaFirma) As Boolean

    Function Imprimir(ByVal Report As PrinterReport) As Boolean

    Function ImprimirHojaRuta(ByVal Document As PrinterDocument) As Boolean

    Function ProcessHotKeys(ByVal frm As Windows.Forms.Form, ByVal e As Windows.Forms.KeyEventArgs) As Boolean

End Interface
