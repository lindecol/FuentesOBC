Public Class PrinterDocument

    Public Class InfoCliente
        Public Codigo As String
        Public NIT As String
        Public Nombre As String
        Public Direccion As String
        Public Barrio As String
        Public Telefono As String
        Public Entidad As String
    End Class

    Public Class DocumentItem
        Public CodProducto As String
        Public Descripcion As String
        Public Cantidad As Decimal
        Public PrecioUnitario As Decimal
        Public UnidadMedida As String
        Public PrecioTotal As Decimal
        Public UsaPrecios As Boolean
        Public InfoAdicional As String
    End Class


    Public Class InfoResolucion
        Public Numero As String
        Public RangoIni As String
        Public RangoFin As String
        Public Fecha As DateTime
    End Class

    Public MostarTotales As Boolean = False
    Public Consecutivo As String
    Public NumPedido As String
    Public Nombre As String
    Public TipoDocumento As String
    Public TipoPago As String
    Public FechaElaboracion As DateTime
    Public FechaVencimiento As DateTime
    Public Cliente As New InfoCliente
    Public Resolucion As InfoResolucion
    Public Items As New List(Of DocumentItem)
    Public DetalleHojaruta As New List(Of DetalleReporteHojaRuta)
    Public Subtotal As Decimal
    Public TotalIVA As Decimal
    Public Total As Decimal
    Public Sucursal As String
    Public Telefono1 As String
    Public Telefono2 As String
    Public OrdenCompra As String

    Public TipoFacturaPDF As String
    Public NumeroFaturaPDF As String
    Public PrefijoFacturaPDF As String

#Region "Documento hoja de ruta"

    '****  Documento Hoja de ruta ************************--------------------------------------------------------
    Public Producto As String
    Public Totalm3 As String
    Public Tanquero As String
    Public Cabezote As String
    Public HoraSalida As String
    Public HoraEntrada As String
    Public KmSalida As String
    Public KmEntrada As String
    Public FechaEntrada As String
    Public FechaSalida As String
    Public HojaRuta As String
    Public Terminal As String
    Public Totalhoras As String
    Public Totalkm As Double
    Public NombreConductor As String
    Public CodConductor As String
    Public Nombretransportadora As String
    Public PuntoVenta As String
    '-*-*-*- Detalle Reporte_**************************--------------------------------------------------
    Public Class DetalleReporteHojaRuta
        Public RelacionCargue As String
        Public NoRemision As String
        Public Dia As String
        Public KmLLegada As String
        Public HoraLlegada As String
        Public HoraTerminada As String
        Public PresionInicial As String
        Public PresionFinal As String
        Public NivelInicial As String
        Public NivelFinal As String
        Public TotalCargado As String
    End Class


    Public Sub SetInfoencabezadoHojaRuta(ByVal producto As String, ByVal Totalm3 As String, ByVal Tanquero As String, ByVal Cabezote As String, ByVal HoraSalida As String, ByVal HoraEntrada As String, ByVal KmSalida As String, ByVal KmEntrada As String, ByVal FechaEntrada As String, ByVal FechaSalida As String, ByVal HojaRuta As String, ByVal Terminal As String, ByVal Totalhoras As String, ByVal Totalkm As Double, ByVal NombreConductor As String, ByVal CodConductor As String, ByVal nombretransportadora As String, ByVal PuntoVenta As String)
        Me.Producto = producto
        Me.Totalm3 = Totalm3
        Me.Tanquero = Tanquero
        Me.Cabezote = Cabezote
        Me.HoraSalida = HoraSalida
        Me.HoraEntrada = HoraEntrada
        Me.KmSalida = KmSalida
        Me.KmEntrada = KmEntrada
        Me.FechaEntrada = FechaEntrada
        Me.FechaSalida = FechaSalida
        Me.HojaRuta = HojaRuta
        Me.Terminal = Terminal
        Me.Totalkm = Totalkm
        Me.Totalhoras = Totalhoras
        Me.NombreConductor = NombreConductor
        Me.CodConductor = CodConductor
        Me.Nombretransportadora = nombretransportadora
        Me.PuntoVenta = PuntoVenta
    End Sub
    Public Sub LlenarDetalleHojaRuta(ByVal RelacionCargue As String, ByVal NoRemision As String, ByVal Dia As String, ByVal KmLLegada As String, ByVal HoraLlegada As String, ByVal HoraTerminada As String, ByVal PresionInicial As String, ByVal PresionFinal As String, ByVal NivelInicial As String, ByVal NivelFinal As String, ByVal TotalCargado As String)
        Dim det As New DetalleReporteHojaRuta
        det.RelacionCargue = RelacionCargue
        det.NoRemision = NoRemision
        det.Dia = Dia
        det.KmLLegada = KmLLegada
        det.HoraLlegada = HoraLlegada
        det.HoraTerminada = HoraTerminada
        det.PresionInicial = PresionInicial
        det.NivelInicial = NivelInicial
        det.TotalCargado = TotalCargado
        det.PresionFinal = PresionFinal
        det.NivelFinal = NivelFinal
        DetalleHojaruta.Add(det)
    End Sub
    '****  //Documento Hoja de ruta *************---------------------------------------------------------------------
#End Region

    Public Sub SetInfoDocumento(ByVal Consecutivo As String, ByVal NumPedido As String, _
    ByVal Nombre As String, ByVal TipoDocumento As String, ByVal TipoPago As String, _
    ByVal FechaElaboracion As DateTime, ByVal FechaVencimiento As DateTime, ByVal Sucursal As String, ByVal OrdenCompra As String, _
    ByVal TipoFacturaPDF As String, ByVal NumeroFaturaPDF As String, ByVal PrefijoFacturaPDF As String)

        Me.Consecutivo = Consecutivo
        Me.Nombre = Nombre
        Me.NumPedido = NumPedido
        Me.TipoDocumento = TipoDocumento
        Me.TipoPago = TipoPago
        Me.FechaElaboracion = FechaElaboracion
        Me.FechaVencimiento = FechaVencimiento
        Me.Sucursal = Sucursal
        Me.OrdenCompra = OrdenCompra

        'KUXAN AJUSTE DE DOCUMENTOS
        If TipoFacturaPDF = "B" Then
            Me.TipoFacturaPDF = "8"
        Else
            Me.TipoFacturaPDF = "1"
        End If
        
        Me.NumeroFaturaPDF = NumeroFaturaPDF
        Me.PrefijoFacturaPDF = PrefijoFacturaPDF

    End Sub


    Public Sub SetInfoResolucion(ByVal NumResolucion As String, ByVal RangoIni As String, ByVal RangoFin As String, _
    ByVal FechaResolucion As DateTime)
        Me.Resolucion = New InfoResolucion
        Me.Resolucion.Numero = NumResolucion
        Me.Resolucion.RangoIni = RangoIni
        Me.Resolucion.RangoFin = RangoFin
        Me.Resolucion.Fecha = FechaResolucion
    End Sub

    Public Sub SetInfoCliente(ByVal Codigo As String, ByVal NIT As String, ByVal Nombre As String, _
    ByVal Direccion As String, ByVal Barrio As String, ByVal Telefono As String, ByVal Entidad As String)
        Me.Cliente.Codigo = Codigo
        Me.Cliente.NIT = NIT
        Me.Cliente.Nombre = Nombre
        Me.Cliente.Direccion = Direccion
        Me.Cliente.Barrio = Barrio
        Me.Cliente.Telefono = Telefono
        Me.Cliente.Entidad = Entidad
    End Sub

    Public Sub AddItem(ByVal CodProducto As String, ByVal Descripcion As String, _
    ByVal Cantidad As Decimal, ByVal UnidadMedida As String, ByVal InfoAdicional As String)
        Dim item As New DocumentItem
        item.CodProducto = CodProducto
        item.Descripcion = Descripcion
        item.Cantidad = Cantidad
        item.UnidadMedida = UnidadMedida
        item.UsaPrecios = False
        item.InfoAdicional = InfoAdicional
        Items.Add(item)
    End Sub

    Public Sub AddItem(ByVal CodProducto As String, ByVal Descripcion As String, ByVal Cantidad As Decimal, _
    ByVal PrecioUnitario As Decimal, ByVal UnidadMedida As String, ByVal PrecioTotal As Decimal, ByVal InfoAdicional As String)
        Dim item As New DocumentItem
        item.CodProducto = CodProducto
        item.Descripcion = Descripcion
        item.Cantidad = Cantidad
        item.UnidadMedida = UnidadMedida
        item.PrecioUnitario = PrecioUnitario
        item.PrecioTotal = PrecioTotal
        item.UsaPrecios = True
        item.InfoAdicional = InfoAdicional
        Items.Add(item)
    End Sub

    Public Sub SetInfoTotales(ByVal Subtotal As Decimal, ByVal IVA As Decimal)
        Me.Subtotal = Subtotal
        Me.TotalIVA = IVA
        Me.Total = Subtotal + IVA
        Me.MostarTotales = True
    End Sub

End Class

