Imports MovilidadCF.Windows.Forms

Public Class GeneracionForm
    Private m_DetallePedido As VentaDataSet.DetallePedidoDataTable
    Private GeneroDocumentos As Boolean = False
    Private m_RowCliente As DataRow
    Private m_RowPedido As DataRow
    Private m_RowFactura() As DataRow
    Private m_rowTalonario As DataRow = Nothing
    Private m_sTipoMovimiento As String
    Private doc As PrinterDocument ' Obejto para crear el documento impreso

    Public Shared Function Run(ByVal rowCliente As System.Data.DataRow, ByRef rowPedido As System.Data.DataRow) As Boolean
        UIHandler.StartWait()
        Dim form As New GeneracionForm
        form.m_RowCliente = rowCliente
        form.m_RowPedido = rowPedido
        Dim bRes As Boolean = UIHandler.ShowDialog(form) = System.Windows.Forms.DialogResult.OK
        form.Dispose()
        UIHandler.EndWait()
        Return bRes
    End Function

    Private Sub GeneracionForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not Nucleo.ProcessHotKeys(Me, e) Then
            If e.KeyCode = Keys.Escape Then
                menuCancelar_Click(Me, Nothing)
            End If
        End If
    End Sub

    Private Sub GeneracionForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.UiHandler1.Parent = Me

        Me.dsVenta = Venta.dsVenta
        Me.bsDetalleFactura.DataSource = Me.dsVenta
        Me.bsFactura.DataSource = Me.dsVenta

        ' Se obtiene el taloraio

        '------------
        If CStrDBNull(m_RowCliente("TipoPago")) = TipoPago.Contado Then
            m_sTipoMovimiento = TiposMovimiento.Factura
        ElseIf CStrDBNull(m_RowCliente("TipoPago")) = TipoPago.Credito Then
            If CStrDBNull(m_RowCliente("FrecuenciaMensual")) = DatosCliente.FrecuenciaMensual Then
                m_sTipoMovimiento = TiposMovimiento.Remision
            Else
                m_sTipoMovimiento = TiposMovimiento.Factura
            End If
        End If
        'si el total del documento es cero se debe de generar remisión
        If CDec(dsVenta.DetallePedido.Compute("SUM(PrecioTotal)", "")) = 0 Then
            m_sTipoMovimiento = TiposMovimiento.Remision
        End If

        ObtenerTalonario()

        'KUXAN ERROR EN TALONARIO 05-12-2019
        If m_rowTalonario Is Nothing Then
            UIHandler.ShowError("No se puede terminar la venta por que no existen talonarios validos")
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Return
        End If

        existeDocumento()
        GenerarDocumento()
        '------------
        UIHandler.EndWait()
    End Sub

    Private Sub PedirTipoDocumento()
        ' Se ocultan los paneles
        pnTipoDocumento.Visible = True
        pnDetalleDocumento.Visible = False
        Me.Menu = New MainMenu()
    End Sub

    Private Sub ObtenerTalonario()
        If m_sTipoMovimiento = TiposMovimiento.Factura Then
            m_rowTalonario = Nucleo.GetTalonarioActual(TiposDocumento.FacturaAutomatica)
        Else
            m_rowTalonario = Nucleo.GetTalonarioActual(TiposDocumento.RemitoAutomatico)
        End If
    End Sub

    Private Function ObtenerInformacionSucursal() As String
        Return CStrDBNull(Nucleo.RowParametros("NombreSucursal")) + " TELEFONO: " + CStrDBNull(Nucleo.RowParametros("TelefonoTransportador")) + " " + CStrDBNull(Nucleo.RowParametros("NitTrasportadora"))
    End Function

    Private Function ObtenerTelefono1() As String
        Return CStrDBNull(CStrDBNull(Nucleo.RowParametros("NombreSucursal")))
    End Function

    Private Function ObtenerTelefono2() As String
        Return CStrDBNull(CStrDBNull(Nucleo.RowParametros("TelefonoTransportador")))
    End Function

    ''' <summary>
    ''' Genera el documento a grabar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenerarDocumento()
        Dim sTitulo As String
        Dim nMontoTotal As Decimal
        Dim nSubTotal As Decimal
        Dim nTotalIVA As Decimal
        Dim nTotalDescuento As Decimal
        Dim sTipoDocumento As String
        Dim dtFechaVencimiento As DateTime
        Dim rowProducto As New ProductosDataSet.ProductosDataTable

        doc = New PrinterDocument

        ' Se ajusta el titulo de la ventana y se pide el talonario para el tipo de documento
        If m_sTipoMovimiento = TiposMovimiento.Factura Then
            lblTitulo.Text = "Generar Factura"
            sTitulo = "FACTURA DE VENTA"
            sTipoDocumento = TipoDocumentos.Factura
            dtFechaVencimiento = DateAdd(DateInterval.Day, CInt(m_RowCliente("DiasCredito")), Today())
            ' Se calcula los montos totales de la factura
            nSubTotal = CDec(dsVenta.DetallePedido.Compute("SUM(PrecioTotal)", ""))
            nTotalIVA = CDec(dsVenta.DetallePedido.Compute("SUM(MontoImpuesto)", ""))
            nTotalDescuento = CDec(dsVenta.DetallePedido.Compute("SUM(MontoDescuento)", ""))
            nMontoTotal = nSubTotal + nTotalIVA
            lblSubTotal.Text = nSubTotal.ToString("#,##0.00")
        Else
            lblTitulo.Text = "Generar Remisión"
            sTitulo = "REMISION"
            sTipoDocumento = TipoDocumentos.Remision
            dtFechaVencimiento = Today()
            ' Se calcula los montos totales de la factura
            nSubTotal = CDec(0)
            nTotalIVA = CDec(0)
            nTotalDescuento = CDec(0)
            nMontoTotal = nSubTotal + nTotalIVA
            lblSubTotal.Text = nSubTotal.ToString("#,##0.00")
            Venta.dsVenta.DetallePedido(0).PrecioUnitario = 0
        End If
        pnTipoDocumento.Visible = False
        pnDetalleDocumento.Visible = True
        Me.Menu = mainMenu1

        m_RowPedido("Estado") = EstadosPedido.Atendido
        m_RowPedido("FechaAtencion") = Today()
        m_RowPedido("HoraAtencion") = Now.ToString("HH:mm")

        ' Se crean el encabezado del documento
        Dim rowDetalleFactura As VentaDataSet.DetalleFacturaRow
        Venta.dsVenta.MaestroFacturas.AddMaestroFacturasRow(m_sTipoMovimiento, _
            CStrDBNull(m_rowTalonario("Actual")), CStrDBNull(m_rowTalonario("Prefijo")), _
            CStrDBNull(m_RowCliente("CodSucursal")), CStrDBNull(m_RowCliente("Codigo")), _
            Today(), "", "", Nucleo.RutaPrincipal, "", HojasRuta.HojaActual.CodConductor, _
            HojasRuta.HojaActual.Cabezote, CStr(HojasRuta.HojaActual.IdHojaRuta), TipoPago.Credito, _
            CShortDBNull(m_RowCliente("DiasCredito")), "", nMontoTotal, 0, _
            nTotalDescuento, nTotalIVA, "", CStrDBNull(m_RowPedido("NoPedido")), EstadoFactura.Entregado)

        Dim rowGuiaDocumento As VentaDataSet.Guia_DocumentoRow
        'rowGuiaDocumento = Nothing
        rowGuiaDocumento = Venta.dsVenta.Guia_Documento.AddGuia_DocumentoRow(HojasRuta.HojaActual.IdHojaRuta, _
                Convert.ToDecimal(m_rowTalonario("Prefijo")), Convert.ToDecimal(m_rowTalonario("Actual")), "N", DateTime.Now, m_sTipoMovimiento)

        doc.SetInfoDocumento(CStrDBNull(m_rowTalonario("Prefijo")) & "-" & CStrDBNull(m_rowTalonario("Actual")), _
            CStrDBNull(m_RowPedido("NoPedido")), sTitulo, sTipoDocumento, "CREDITO", Today(), dtFechaVencimiento, Me.ObtenerInformacionSucursal(), "", m_sTipoMovimiento, CStrDBNull(m_rowTalonario("Actual")), CStrDBNull(m_rowTalonario("Prefijo")))
        'Obrener telefonos
        doc.Telefono1 = ObtenerTelefono1()
        doc.Telefono2 = ObtenerTelefono2()

        doc.SetInfoCliente(CStrDBNull(m_RowCliente("Codigo")), CStrDBNull(m_RowCliente("Nit")), _
            CStrDBNull(m_RowCliente("Nombre")), CStrDBNull(m_RowPedido("Direccion")), _
            CStrDBNull(m_RowPedido("Barrio")), CStrDBNull(m_RowCliente("Telefono")), "")
        If m_sTipoMovimiento = TiposMovimiento.Factura Then
            doc.SetInfoResolucion(CStrDBNull(m_rowTalonario("NumeroResolucion")), CStrDBNull(m_rowTalonario("ResolucionDesde")), _
                  CStrDBNull(m_rowTalonario("ResolucionHasta")), CDate(m_rowTalonario("FechaInicioResolucion")))
        End If

        For Each rowDetalle As VentaDataSet.DetallePedidoRow In dsVenta.DetallePedido

            Productos.dtaProductos.FillBy(rowProducto, rowDetalle.CodProducto)

            ' Se crea el único item que lleva el documento   
            If Not m_sTipoMovimiento = TiposMovimiento.Factura Then
                rowDetalle.PrecioUnitario = 0
                rowDetalle.PrecioTotal = 0
            End If

            rowDetalleFactura = Venta.dsVenta.DetalleFactura.AddDetalleFacturaRow(m_sTipoMovimiento, CStrDBNull(m_rowTalonario("Actual")), _
                CStrDBNull(m_rowTalonario("Prefijo")), rowDetalle.CodProducto, "", "", "", _
                Productos.ProductoActual.UnidadMedidaVenta, rowDetalle.Cantidad, rowDetalle.PrecioUnitario, _
                0, rowDetalle.MontoDescuento, rowDetalle.MontoImpuesto, rowDetalle.PrecioTotal, rowProducto(0).Descripcion)

            SetCurrentRow(bsDetalleFactura, rowDetalleFactura)

            Dim sInfo As String = ""
            If Not rowDetalle.CodProducto.Substring(0, 1).ToString.Equals("1") Then
                Select Case dsVenta.DetallePedido(0).MetodoEntrega
                    Case MetodosEntrega.PorCaudalimetro, MetodosEntrega.PorNiveles
                        sInfo = String.Format("Pres.Ini: {0}, Pres.Fin: {1}, Nivel Ini: {2}, Nivel Fin: {3}", _
                            Venta.dsVenta.DetallePedido(0).PresionInicial, _
                            Venta.dsVenta.DetallePedido(0).PresionFinal, _
                            Venta.dsVenta.DetallePedido(0).NivelInicial, _
                            Venta.dsVenta.DetallePedido(0).NivelFinal)
                    Case Else
                        sInfo = String.Format("Pres.Ini: {0}, Pres.Fin: {1}, Peso Ini: {2}, Peso Fin: {3}", _
                            Venta.dsVenta.DetallePedido(0).PresionInicial, _
                            Venta.dsVenta.DetallePedido(0).PresionFinal, _
                            Venta.dsVenta.DetallePedido(0).PesoInicial, _
                            Venta.dsVenta.DetallePedido(0).PesoFinal)
                End Select
            End If

            doc.AddItem(rowDetalle.CodProducto, rowProducto(0).Descripcion, rowDetalle.Cantidad, _
                    rowDetalle.PrecioUnitario, Productos.ProductoActual.UnidadMedidaVenta, rowDetalle.PrecioTotal, sInfo)

        Next

        doc.SetInfoTotales(nSubTotal, nTotalIVA)

        ' Se ajustan los data bindings para la presentación en pantalla
        bsFactura.Position = dsVenta.MaestroFacturas.Count - 1
        bsFactura.ResetCurrentItem()
        bsDetalleFactura.ResetCurrentItem()
        bsDetalleFactura.Position = 1
        HScrollBar2.Visible = True
        HScrollBar2.Minimum = 1
        HScrollBar2.Maximum = bsDetalleFactura.Count
        HScrollBar2.Value = bsDetalleFactura.Position

        HScrollBar2.Refresh()

        GenerarEncuesta()

    End Sub

    Private Sub menuGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub menuCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCancelar.Click, btnCancelarTipoDoc.Click
        UIHandler.StartWait()
        Venta.dsVenta.MaestroFacturas.Rows.Clear()
        Venta.dsVenta.DetalleFactura.Rows.Clear()
        Venta.dsVenta.Guia_Documento.Rows.Clear()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If rbFactura.Checked Then
            m_sTipoMovimiento = TiposMovimiento.Factura
        Else
            m_sTipoMovimiento = TiposMovimiento.Remision
        End If
        ObtenerTalonario()
        GenerarDocumento()




    End Sub

    Private Sub GenerarEncuesta()

        'Dim frmEncuestaNiso As New Oxigenos.EncuestaNiso.FrmEncuestaNiso(CStrDBNull(m_RowCliente("Codigo")), 3)
        'frmEncuestaNiso.ShowDialog()
    End Sub


    Private Sub HScrollBar2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScrollBar2.ValueChanged
        bsDetalleFactura.Position = HScrollBar2.Value - 1
        'lblDescripcionRemision.Text = Productos.NombreProducto(lblProductoRemision)
        lblPosicionRemision.Text = CStr(HScrollBar2.Value) & " de " & CStr(bsDetalleFactura.Count)
    End Sub

    ''' <summary>
    ''' valida si existe el documento y si existe lo incrementa
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub existeDocumento()
        If (IsDBNull(m_rowTalonario)) Or m_rowTalonario Is Nothing Then
            Exit Sub
        End If
        'valida si el documento ya existe para correrlo de nuevo
        Dim row_MaestroFacturas As VentaDataSet.MaestroFacturasRow
        row_MaestroFacturas = dsVenta.MaestroFacturas.FindByTipoFacturaNoFacturaPrefijo(m_sTipoMovimiento, CStr(m_rowTalonario("Actual")), CStr(m_rowTalonario("PREFIJO")))
        While Not row_MaestroFacturas Is Nothing
            IncrementarNumeroDocumento(m_rowTalonario)
        End While
    End Sub

    Private Sub mnuGrabar1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGrabar1.Click
        'KUXAN 05-15-2019 CAPTURAR FIRMA Y GUARDAR PDF
        Dim DatosFirma As DatosCapturaFirma = Nothing

        Dim capturarFirma As New GenerarPDF.frmFirma()
        If capturarFirma.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            DatosFirma = New DatosCapturaFirma
            DatosFirma.PathFirma = capturarFirma.PathFirma
            DatosFirma.NombreFirma = capturarFirma.Nombre
            DatosFirma.IdentificacionFirma = capturarFirma.Identificacion
        Else
            DatosFirma = Nothing
        End If
        capturarFirma.Dispose()

        If DatosFirma Is Nothing Then
            Return
        End If

        'graba la venta
        Venta.SaveVenta()

        'UIHandler.StartWait()

        'se incrementa el talonario
        IncrementarNumeroDocumento(m_rowTalonario)

        'actualiza el talonario
        Nucleo.UpdateTalonario(m_rowTalonario)

        'confirma los cambios del pedido
        m_RowPedido.AcceptChanges()

        While Not Nucleo.Imprimir(doc, DatosFirma)
            ' Se anula el documento y se vuelve a generar
            'IncrementarNumeroDocumento(m_rowTalonario)
            'dsVenta.MaestroFacturas(dsVenta.MaestroFacturas.Count - 1).EstadoFactura = EstadoFactura.Anulado
            'dsVenta.DetalleFactura.Rows.Clear()
            'GenerarDocumento()
        End While

        'KUXAN ELIMINAR FIRMAS
        EliminarFirmas(DatosFirma.PathFirma)
        DialogResult = System.Windows.Forms.DialogResult.OK


    End Sub

    Private Sub EliminarFirmas(ByVal pathFirma As String)
        Try
            Dim carpeta As String = System.IO.Path.GetDirectoryName(pathFirma)

            For Each archivo As String In System.IO.Directory.GetFiles(carpeta)
                System.IO.File.Delete(archivo)
            Next
        Catch ex As Exception
            MovilidadCF.Logging.Logger.Write("Error eliminando firmas " + ex.Message)
        End Try
    End Sub
End Class