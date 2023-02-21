Imports System.Data
Public Class GestorVenta

    Public Sub ObtenerPrecio(ByVal RowCliente As DataRow, ByVal sCodEntidad As String, _
    ByVal bAutorizado As Boolean, ByVal bRespertaPrecio As Boolean, ByVal sCodProducto As String, _
    ByRef PrecioNeto As Double, ByRef PorcentajeDescuento As Double, ByVal TotalDescuento As Double, _
    ByRef Row As ProductosDataSet.ProductosRow)

        Dim dt As VentaDataSet.PreciosDataTable
        Dim Result As Boolean = True
        Me.dtaPrecios.Connection = Connection

        If bAutorizado Then
            ' Se busca el precio por entidad
            dt = Me.dtaPrecios.GetPrecio(Nucleo.CodigoSucursal, sCodEntidad, sCodProducto)
            If dt.Rows.Count > 0 Then
                If Row.Codigo.Substring(0, 1) = "1" Then
                    If cstrDBNULL(dt(0)("Acarreo")).ToUpper.TrimEnd = "S" Then
                        PorcentajeDescuento = dt(0).PorcentajeDescuento
                        PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                        TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                    Else
                        PorcentajeDescuento = 0
                        PrecioNeto = 0
                        TotalDescuento = 0
                    End If
                Else
                    PorcentajeDescuento = dt(0).PorcentajeDescuento
                    PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                    TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                End If
            Else
                ' Se toma el precio full de la tabla de productos
                If Row.Codigo.Substring(0, 1) = "1" Then
                    If cstrDBNULL(RowCliente("Acarreo")).ToUpper.TrimEnd = "S" Then
                        PorcentajeDescuento = 0
                        PrecioNeto = (Row.Precio * (1 - PorcentajeDescuento / 100))
                        TotalDescuento = Row.Precio - PrecioNeto
                    Else
                        PorcentajeDescuento = 0
                        PrecioNeto = 0
                        TotalDescuento = 0
                    End If
                Else
                    PorcentajeDescuento = 0
                    PrecioNeto = (Row.Precio * (1 - PorcentajeDescuento / 100))
                    TotalDescuento = Row.Precio - PrecioNeto
                End If
            End If
        Else
            If bRespertaPrecio Then
                ' Se busca por la entidad
                dt = Me.dtaPrecios.GetPrecio(Nucleo.CodigoSucursal, sCodEntidad, sCodProducto)
                If dt.Rows.Count > 0 Then
                    If Row.Codigo.Substring(0, 1) = "1" Then
                        If cstrDBNULL(dt(0)("Acarreo")).ToUpper.TrimEnd = "S" Then
                            PorcentajeDescuento = dt(0).PorcentajeDescuento
                            PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                            TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                        Else
                            PorcentajeDescuento = 0
                            PrecioNeto = 0
                            TotalDescuento = 0
                        End If
                    Else
                        PorcentajeDescuento = dt(0).PorcentajeDescuento
                        PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                        TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                    End If
                Else
                    'Se busca por paciente
                    dt = Me.dtaPrecios.GetPrecio(Nucleo.CodigoSucursal, cstrDBNULL(RowCliente("Codigo")), sCodProducto)
                    If dt.Rows.Count > 0 Then
                        If Row.Codigo.Substring(0, 1) = "1" Then
                            If cstrDBNULL(dt(0)("Acarreo")).ToUpper.TrimEnd = "S" Then
                                PorcentajeDescuento = dt(0).PorcentajeDescuento
                                PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                                TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                            Else
                                PorcentajeDescuento = 0
                                PrecioNeto = 0
                                TotalDescuento = 0
                            End If
                        Else
                            PorcentajeDescuento = dt(0).PorcentajeDescuento
                            PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                            TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                        End If
                    Else
                        ' Se toma el precio full de la tabla de productos
                        If Row.Codigo.Substring(0, 1) = "1" Then
                            If cstrDBNULL(RowCliente("Acarreo")).ToUpper.TrimEnd = "S" Then
                                PorcentajeDescuento = 0
                                PrecioNeto = (Row.Precio * (1 - PorcentajeDescuento / 100))
                                TotalDescuento = Row.Precio - PrecioNeto
                            Else
                                PorcentajeDescuento = 0
                                PrecioNeto = 0
                                TotalDescuento = 0
                            End If
                        Else
                            PorcentajeDescuento = 0
                            PrecioNeto = (Row.Precio * (1 - PorcentajeDescuento / 100))
                            TotalDescuento = Row.Precio - PrecioNeto
                        End If
                    End If
                End If
            Else
                ' Se busca por cliente/paciente
                dt = Me.dtaPrecios.GetPrecio(Nucleo.CodigoSucursal, cstrDBNULL(RowCliente("Codigo")), sCodProducto)
                If dt.Rows.Count > 0 Then
                    If Row.Codigo.Substring(0, 1) = "1" Then
                        If cstrDBNULL(dt(0)("Acarreo")).ToUpper.TrimEnd = "S" Then
                            PorcentajeDescuento = dt(0).PorcentajeDescuento
                            PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                            TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                        Else
                            PorcentajeDescuento = 0
                            PrecioNeto = 0
                            TotalDescuento = 0
                        End If
                    Else
                        PorcentajeDescuento = dt(0).PorcentajeDescuento
                        PrecioNeto = (dt(0).PrecioNeto * (1 - PorcentajeDescuento / 100))
                        TotalDescuento = dt(0).PrecioNeto - PrecioNeto
                    End If
                Else
                    ' Se toma el precio full de la tabla de productos
                    If Row.Codigo.Substring(0, 1) = "1" Then
                        If cstrDBNULL(RowCliente("Acarreo")).ToUpper.TrimEnd = "S" Then
                            PorcentajeDescuento = 0
                            PrecioNeto = (Row.Precio * (1 - PorcentajeDescuento / 100))
                            TotalDescuento = Row.Precio - PrecioNeto
                        Else
                            PorcentajeDescuento = 0
                            PrecioNeto = 0
                            TotalDescuento = 0
                        End If
                    Else
                        PorcentajeDescuento = 0
                        PrecioNeto = (Row.Precio * (1 - PorcentajeDescuento / 100))
                        TotalDescuento = Row.Precio - PrecioNeto
                    End If
                End If
            End If
        End If
    End Sub

    Public Function ObtenerFactura(ByVal Factura As String) As Boolean
        Dim dt As VentaDataSet.MaestroFacturasDataTable
        Me.dtaMaestroFacturas.Connection = Connection
        dt = Me.dtaMaestroFacturas.GetFactura(Factura)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

#Region "Generacion de Documentos"

    Public Sub UpdateMaestroFacturas()
        Me.dtaMaestroFacturas.Connection = Connection
        Me.dtaMaestroFacturas.Update(dsVenta.MaestroFacturas)
        dsVenta.MaestroFacturas.AcceptChanges()
    End Sub

    Public Sub UpdateDetalleFactura()
        Me.dtaDetalleFactura.Connection = Connection
        Me.dtaDetalleFactura.Update(dsVenta.DetalleFactura)
        dsVenta.DetalleFactura.AcceptChanges()
    End Sub

    Public Sub UpdateMaestroGuias()
        Me.dtaMaestroGuias.Connection = Connection
        Me.dtaMaestroGuias.Update(dsVenta.MaestroGuias)
        dsVenta.MaestroGuias.AcceptChanges()
    End Sub

    Public Sub UpdateDetalleGuiaAsignacionesRecolecciones()
        Me.dtaDetalleGuiaAsignacionesRecolecciones.Connection = Connection
        Me.dtaDetalleGuiaAsignacionesRecolecciones.Update(dsVenta.DetalleGuiaAsignacionesRecolecciones)
        dsVenta.DetalleGuiaAsignacionesRecolecciones.AcceptChanges()
    End Sub

    Public Sub UpdateDetalleGuiaFacturasRemisiones()
        Me.dtaDetalleGuiaFacturasRemisiones.Connection = Connection
        Me.dtaDetalleGuiaFacturasRemisiones.Update(dsVenta.DetalleGuiaFacturasRemisiones)
        dsVenta.DetalleGuiaFacturasRemisiones.AcceptChanges()
    End Sub

    Public Sub UpdateDetalleGuiaRecoleccionesAjenos()
        Me.dtaDetalleGuiaRecoleccionesAjenos.Connection = Connection
        Me.dtaDetalleGuiaRecoleccionesAjenos.Update(dsVenta.DetalleGuiaRecoleccionesAjenos)
        dsVenta.DetalleGuiaRecoleccionesAjenos.AcceptChanges()
    End Sub

    Public Sub UpdateFacturasManuales()
        Me.dtaFacturasManuales.Connection = Connection
        Me.dtaFacturasManuales.Update(dsVenta.FacturasManuales)
        dsVenta.FacturasManuales.AcceptChanges()
    End Sub

#End Region

End Class
