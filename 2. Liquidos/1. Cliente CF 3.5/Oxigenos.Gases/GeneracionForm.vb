Imports MovilidadCF.Windows.Forms

Public Class GeneracionForm
    Private m_DetallePedido As PedidosDataSet.DetallePedidoDataTable
    Private m_CilindrosLeidos As VentaDataSet.CilindrosLeidosDataTable
    Private GeneraFactura As Boolean = False
    Private GeneraRemision As Boolean = False
    Private GeneraAsignacion As Boolean = False
    Private GeneraPagoDeuda As Boolean = False
    Private GeneraRecoleccion As Boolean = False
    Private GeneroRecoleccionAjeno As Integer = 0
    Private GeneraVenta As Boolean = False
    Private GeneraEntregaAjeno As Boolean = False
    Private GeneraDeposito As Boolean = False
    Private GeneroDocumentos As Boolean = False
    Private m_RowCliente As DataRow
    Private m_RowPedido As DataRow

    Private m_RowTalonarioFactura As DataRow = Nothing
    Private m_RowTalonarioRemision As DataRow = Nothing
    Private m_RowTalonarioAsignacion As DataRow = Nothing
    Private m_RowTalonarioRecoleccion As DataRow = Nothing
    Private m_RowTalonarioDeposito As DataRow = Nothing

    Private m_RowFactura() As DataRow
    Private m_RowRemision() As DataRow


#Region "Eventos Panel GeneracionDocumentos"

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If rbFactura.Checked Then
            GeneraFactura = True
        ElseIf rbRemision.Checked Then
            GeneraRemision = True
        End If
        pnTipoDocs.Enabled = True
        pnTipoDocumento.Visible = False
        pnTipoDocs.BringToFront()

        ' Valida si se generan recolecciones
        If CIntDBNull(m_DetallePedido.Compute("Count(UnidadesReales)", "UnidadesReales < Recolecciones"), 0) > 0 Or _
        CIntDBNull(m_CilindrosLeidos.Compute("Count(Secuencial)", "Secuencial = '' And CodTipoProducto = ''"), 0) > 0 Then
            GeneraRecoleccion = True
        End If

        ' Se valida si se generan asignaciones 
        If CIntDBNull(m_DetallePedido.Compute("Count(Asignaciones)", "Asignaciones > 0"), 0) > 0 Then
            GeneraAsignacion = True
        End If
        MostrarDocumentosGenerar()
    End Sub

    Private Sub btnContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        'If GeneraVenta Then
        '    If GeneroDocumentos = False Or dsVenta.MaestroFacturas.Rows.Count = 0 Then
        '        If MsgBox("Esta seguro de generar los documentos?", MsgBoxStyle.YesNo, "Confirmación") = MsgBoxResult.Yes Then
        '            If Not GenerarTablasDescarga() Then
        '                UIHandler.ShowError("Error generando documentos!!")
        '                Exit Sub
        '            End If
        '            GeneroDocumentos = True
        '        End If
        '    End If
        'Else
        '    If GeneroDocumentos = False Or dsVenta.DetalleGuiaAsignacionesRecolecciones.Rows.Count = 0 Then
        '        If MsgBox("Esta seguro de generar los documentos?", MsgBoxStyle.YesNo, "Confirmación") = MsgBoxResult.Yes Then
        '            If Not GenerarTablasDescargaRecoleccionPura() Then
        '                UIHandler.ShowError("Error generando documentos!!")
        '                Exit Sub
        '            End If
        '            GeneroDocumentos = True
        '            'ImprimirDocumentos()
        '        End If
        '    End If
        'End If

        If MsgBox("Esta seguro de generar los documentos?", MsgBoxStyle.YesNo, "Confirmación") = MsgBoxResult.Yes Then
            If GeneraFactura Then
                MostrarDetalleFactura()
                If GeneraRemision Then
                    Me.btnGrabar.Text = "&Continuar"
                End If
            ElseIf GeneraRemision Then
                MostrarDetalleRemision()
            Else
                ImprimirDocumentos()
            End If
        Else
            Productos.dsProductos.RejectChanges()
            Venta.dsVenta.RejectChanges()
            Pacientes.dsPacientes.RejectChanges()
            Pedidos.dsPedidos.RejectChanges()
            UIHandler.StartWait()
            DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub btnRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegresar.Click
        Productos.dsProductos.RejectChanges()
        Venta.dsVenta.RejectChanges()
        Pacientes.dsPacientes.RejectChanges()
        Pedidos.dsPedidos.RejectChanges()
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancelarTipoDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelarTipoDoc.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

#End Region

#Region "Metodos para la generacion de documentos"

    Private Function GenerarRecoleccionPura() As Boolean
        Dim RowKardex As ProductosDataSet.KardexCamionRow
        Dim Row() As DataRow

        Row = dsVenta.CilindrosLeidos.Select("Secuencial = '' And CodTipoProducto = ''")
        If Row.Length > 0 Then
            For i As Integer = 0 To Row.Length - 1
                RowKardex = Productos.dsProductos.KardexCamion.FindByCodProductoCapacidadCodTipoProducto(cstrDBNULL(Row(i)("CodProducto")), _
                cstrDBNULL(Row(i)("Capacidad")), TipoProducto.Activo)

                If RowKardex IsNot Nothing Then
                    If cstrDBNULL(Row(i)("Pertenencia")) = Pertenencia.Cliente Then
                        RowKardex("SaldoCliente") = CShort(RowKardex("SaldoCliente")) + CShort(1)
                    Else
                        RowKardex("SaldoPraxair") = CShort(RowKardex("SaldoPraxair")) + CShort(1)
                    End If
                Else
                    If cstrDBNULL(Row(i)("Pertenencia")) = Pertenencia.Cliente Then
                        Productos.dsProductos.KardexCamion.AddKardexCamionRow(cstrDBNULL(Row(i)("CodProducto")), _
                        cstrDBNULL(Row(i)("Capacidad")), 0, 0, CShortDBNull(Row(i)("Cantidad")), CShortDBNull(Row(i)("Cantidad")), _
                        Nucleo.CodigoSucursal, TipoProducto.Activo, CInt(cstrDBNULL(Row(i)("Capacidad"))))
                    Else
                        Productos.dsProductos.KardexCamion.AddKardexCamionRow(cstrDBNULL(Row(i)("CodProducto")), _
                        cstrDBNULL(Row(i)("Capacidad")), CShortDBNull(Row(i)("Cantidad")), CShortDBNull(Row(i)("Cantidad")), _
                        0, 0, Nucleo.CodigoSucursal, TipoProducto.Activo, CInt(cstrDBNULL(Row(i)("Capacidad"))))
                    End If
                End If

                'G R A B A R   G U I A  D E  R E C O L E C C I O N E S  Y  R E C O L E C C I O N E S  A J E N O S
                If Not GrabarRecolecciones(cstrDBNULL(Row(i)("CodProducto")), cstrDBNULL(Row(i)("Capacidad")), _
                cstrDBNULL(Row(i)("Pertenencia")), m_RowTalonarioRecoleccion, CShortDBNull(Row(i)("Cantidad")), _
                cstrDBNULL(Row(i)("SecuencialAjeno")), cstrDBNULL(Row(i)("UnidadMedida"))) Then
                    Return False
                    Exit Function
                End If
            Next
        End If
        Return True
    End Function

    Private Function GenerarEntregaAjeno() As Boolean
        Dim Row() As DataRow
        Row = dsVenta.CilindrosLeidos.Select("Secuencial = '' And SecuencialAjeno <> '' And CodTipoProducto = '5'")
        If Row.Length > 0 Then
            For i As Integer = 0 To Row.Length - 1
                If InsertEncabezadoFacturaRemision(0, 0, 0, 0, TipoMovimientos.Remision, m_RowTalonarioRemision, _
                    TiposDocumento.RemitoAutomatico) Then

                    InsertEncabezadoGuia(TipoMovimientos.Remision, m_RowTalonarioRemision)

                    InsertDetalleFactura(cstrDBNULL(Row(i)("CodProducto")), "0", _
                    cstrDBNULL(Row(i)("Pertenencia")), cstrDBNULL(Row(i)("UnidadMedida")), 0, 0, 0, 0, m_RowTalonarioRemision, _
                    TiposDocumento.RemitoAutomatico, TipoMovimientos.Remision, CShortDBNull(Row(i)("Cantidad")), 0, "")

                    InsertDetalleGuiaFacturasRemisiones(cstrDBNULL(Row(i)("CodProducto")), "0", _
                    cstrDBNULL(Row(i)("Pertenencia")), m_RowTalonarioRemision, TipoMovimientos.Remision, _
                    cstrDBNULL(Row(i)("Secuencial")), cstrDBNULL(Row(i)("SecuencialAjeno")))

                    ' S E  A C T U A L I Z A  C A R G U E  Y  K A R D E X  D E  C A M I O N 
                    Productos.ActualizarCargueKardex(cstrDBNULL(Row(i)("CodProducto")), cstrDBNULL(Row(i)("CodSucursal")), _
                    cstrDBNULL(Row(i)("SecuencialAjeno")), cstrDBNULL(Row(i)("Secuencial")), cstrDBNULL(Row(i)("CodTipoProducto")), _
                    cstrDBNULL(Row(i)("Pertenencia")), "0", CShortDBNull(Row(i)("Cantidad")))
                Else
                    Return False
                    Exit Function
                End If
            Next
        End If
        Return True
    End Function

    Private Function GenerarTablasDescargaRecoleccionPura() As Boolean
        If GeneraRecoleccion Then
            If Not GenerarRecoleccionPura() Then
                UIHandler.EndWait()
                Return False
                Exit Function
            End If
        End If

        If GeneraEntregaAjeno Then
            If Not GenerarEntregaAjeno() Then
                UIHandler.EndWait()
                Return False
                Exit Function
            End If
        End If

        ' Se valida si se generaron cancelación de deudas
        If GeneraPagoDeuda Then
            Dim Monto As Decimal
            Dim Impuesto As Decimal

            Monto = CDec(Pacientes.dsPacientes.DeudasPagadas.Compute("SUM(SubTotal)", "1=1"))
            Impuesto = CDec(Pacientes.dsPacientes.DeudasPagadas.Compute("SUM(MontoIva)", "1=1"))

            ' Se crea el encabezado de la factura
            If InsertEncabezadoFacturaRemision(Monto, Impuesto, 0, 0, TipoMovimientos.Factura, m_RowTalonarioFactura, _
            TiposDocumento.FacturaAutomatica) Then
                InsertEncabezadoGuia(TipoMovimientos.Factura, m_RowTalonarioFactura)

                For Each Row As PacientesDataSet.DeudasPagadasRow In Pacientes.dsPacientes.DeudasPagadas.Rows
                    ActualizarAlquileres(Row)
                    ' Se agrega el detalle de la factura
                    InsertDetalleFactura(Row.CodProducto, Row.Capacidad, Row.Pertenencia, Row.UnidadVenta, _
                    0, 0, CDecDBNull(Row.MontoIva, 0), CDecDBNull(Row.SubTotal, 0), m_RowTalonarioFactura, _
                    TiposDocumento.FacturaAutomatica, TipoMovimientos.Factura, CShortDBNull(Row.DiasCancelados, 0), _
                    CDecDBNull(Row.Precio, 0), "")

                    ' Se agrega el detalle de la guia
                    InsertDetalleGuiaFacturasRemisiones(Row.CodProducto, Row.Capacidad, Row.Pertenencia, m_RowTalonarioFactura, _
                    TipoMovimientos.Factura, "", "")
                Next
            Else
                UIHandler.EndWait()
                Return False
                Exit Function
            End If
        End If

        Return True
        UIHandler.EndWait()
    End Function

    ' Determina que documentos van a ser generados y se muestran en pantalla
    Private Sub ValidarDocumentosGenerar()

        ' Valida si se genero venta
        If CIntDBNull(m_CilindrosLeidos.Compute("Count(Secuencial)", "Secuencial <> '' And CodTipoProducto <> ''"), 0) > 0 Or _
        CIntDBNull(Pedidos.dsPedidos.DetallePedido.Compute("Count(NoPedido)", "UnidadesReales > 0"), 0) > 0 Then
            ' Se genero venta
            GeneraVenta = True
            If CIntDBNull(m_CilindrosLeidos.Compute("Count(SecuencialAjeno)", "Secuencial = '' And SecuencialAjeno <> '' And CodTipoProducto = '5'"), 0) > 0 Then
                GeneraEntregaAjeno = True
            End If
            If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                ' Se valida si se genero factura y/o remisión
                If CIntDBNull(m_DetallePedido.Compute("Sum(UnidadesVendidasContado)", "1=1"), 0) > 0 Then
                    GeneraFactura = True
                End If
                If CIntDBNull(m_DetallePedido.Compute("Sum(UnidadesVendidasCredito)", "1=1"), 0) > 0 Then
                    GeneraRemision = True
                End If
                ' Valida si se generaron recolecciones
                If CIntDBNull(m_CilindrosLeidos.Compute("Count(Secuencial)", "Secuencial = '' And CodTipoProducto = ''"), 0) > 0 Then
                    GeneraRecoleccion = True
                End If
                ' Se valida si se generaron Depositos
                If Pacientes.dsPacientes.DepositosGarantia.Rows.Count > 0 Then
                    GeneraDeposito = True
                End If

                ' Se valida si se generan pago de deudas
                If Pacientes.dsPacientes.DeudasPagadas.Rows.Count > 0 Then
                    GeneraPagoDeuda = True
                End If
            Else
                If cstrDBNULL(m_RowCliente("TipoPago")) = TipoPago.Contado Then
                    GeneraFactura = True
                ElseIf cstrDBNULL(m_RowCliente("TipoPago")) = TipoPago.Credito Then
                    If cstrDBNULL(m_RowCliente("FrecuenciaMensual")) = DatosCliente.FrecuenciaMensual Then
                        pnTipoDocs.Enabled = False
                        pnTipoDocumento.Visible = True
                        pnTipoDocumento.BringToFront()
                        Exit Sub
                    Else
                        GeneraFactura = True
                    End If
                End If

                ' Valida si se generan recolecciones
                If CIntDBNull(m_DetallePedido.Compute("Count(UnidadesReales)", "UnidadesReales < Recolecciones"), 0) > 0 Or _
                CIntDBNull(m_CilindrosLeidos.Compute("Count(Secuencial)", "Secuencial = '' And CodTipoProducto = ''"), 0) > 0 Then
                    GeneraRecoleccion = True
                End If
            End If

            ' Se valida si se generan asignaciones
            If CIntDBNull(m_DetallePedido.Compute("Count(Asignaciones)", "Asignaciones > 0"), 0) > 0 Then
                GeneraAsignacion = True
            End If
        Else
            ' Genera Recoleccion pura
            If CIntDBNull(m_CilindrosLeidos.Compute("Count(Secuencial)", "Secuencial = '' And CodTipoProducto = ''"), 0) > 0 Then
                GeneraRecoleccion = True
            End If

            If CIntDBNull(m_CilindrosLeidos.Compute("Count(SecuencialAjeno)", "Secuencial = '' And SecuencialAjeno <> '' And CodTipoProducto = '5'"), 0) > 0 Then
                GeneraEntregaAjeno = True
            End If

            If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                ' Se valida si se generan pago de deudas
                If Pacientes.dsPacientes.DeudasPagadas.Rows.Count > 0 Then
                    GeneraPagoDeuda = True
                    GeneraFactura = True
                End If
            Else
                GeneraFactura = False
            End If

            GeneraRemision = False
            GeneraAsignacion = False
            GeneraDeposito = False
        End If


        If GeneraVenta Then
            If GeneroDocumentos = False Or dsVenta.MaestroFacturas.Rows.Count = 0 Then
                If Not GenerarTablasDescarga() Then
                    UIHandler.ShowError("Error generando documentos!!")
                    Exit Sub
                End If
                GeneroDocumentos = True
            End If
        Else
            If GeneroDocumentos = False Or dsVenta.DetalleGuiaAsignacionesRecolecciones.Rows.Count = 0 Then
                If Not GenerarTablasDescargaRecoleccionPura() Then
                    UIHandler.ShowError("Error generando documentos!!")
                    Exit Sub
                End If
                GeneroDocumentos = True
            End If
        End If

        MostrarDocumentosGenerar()
    End Sub

    Private Sub MostrarDocumentosGenerar()
        If GeneraFactura Then
            ckFactura.Checked = True
        End If
        If GeneraRemision Or GeneraEntregaAjeno Then
            ckRemision.Checked = True
        End If
        If GeneraAsignacion Then
            ckAsignacion.Checked = True
        End If
        If GeneraRecoleccion Then
            ckRecoleccion.Checked = True
        End If
        If GeneraDeposito Then
            ckDeposito.Checked = True
        End If
    End Sub

    Private Function GenerarTablasDescarga() As Boolean
        Dim MontoFlete As Double
        Dim MontoTotalItem As Double
        Dim RowLeidos() As DataRow

        ' Se inicializan las variables
        GeneraRecoleccion = False
        GeneraAsignacion = False

        ' O B T E N C I O N  D A T O S  T A L O N A R I O
        UIHandler.StartWait()
        If GeneraFactura Then
            If m_RowTalonarioFactura Is Nothing Then
                If Not ObtenerNoFactura(m_RowTalonarioFactura, TiposDocumento.FacturaAutomatica) Then
                    UIHandler.EndWait()
                    Return False
                    Exit Function
                End If
            End If
        End If

        If GeneraRemision Then
            If m_RowTalonarioRemision Is Nothing Then
                If Not ObtenerNoFactura(m_RowTalonarioRemision, TiposDocumento.RemitoAutomatico) Then
                    UIHandler.EndWait()
                    Return False
                    Exit Function
                End If
            End If
        End If

        Try
            Venta.OpenConnection()
            ' E N C A B E Z A D O S  F A C T U R A S  Y  M A E S T R O  D E  G U I A S 
            If GeneraFactura Then
                If Not GenerarEncabezadosFactura(m_RowTalonarioFactura) Then
                    UIHandler.EndWait()
                    Return False
                    Exit Function
                End If
            End If
            If GeneraRemision Then
                If Not GenerarEncabezadosRemision(m_RowTalonarioRemision) Then
                    UIHandler.EndWait()
                    Return False
                    Exit Function
                End If
            End If

            For Each Row As PedidosDataSet.DetallePedidoRow In m_DetallePedido.Rows
                If Row.UnidadesReales > 0 Then

                    ' D E T A L L E S  F A C T U R A  Y  R E M I S I O N
                    If GeneraFactura Then
                        If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                            If CIntDBNull(Row.UnidadesVendidasContado, 0) > 0 Then
                                If Row.TipoProducto = TipoProducto.Flete Then
                                    MontoFlete = CIntDBNull(Row.SubTotalContado, 0)
                                Else
                                    MontoFlete = 0
                                End If
                                MontoTotalItem = CDblDBNull(Row.MontoTotalContado, 0)

                                InsertDetalleFactura(Row.CodProducto, Row.Capacidad, "", Row.UnidadMedidaVenta, _
                                CDecDBNull(MontoFlete, 0), CDecDBNull(Row.TotalDescuentoContado, 0), CDecDBNull(Row.TotalIvaContado, 0), _
                                CDecDBNull(MontoTotalItem, 0), m_RowTalonarioFactura, TiposDocumento.FacturaAutomatica, TipoMovimientos.Factura, _
                                CShortDBNull(Row.UnidadesVendidasContado, 0), CDecDBNull(Row.PrecioContado, 0), Row.DescripcionProducto)
                            End If
                        Else
                            If Row.TipoProducto = TipoProducto.Flete Then
                                MontoFlete = CDblDBNull(Row.MontoTotalContado, 0) + CDblDBNull(Row.MontoTotalCredito, 0)
                            Else
                                MontoFlete = 0
                            End If
                            MontoTotalItem = CDblDBNull(Row.MontoTotalContado, 0) + CDblDBNull(Row.MontoTotalCredito, 0)

                            InsertDetalleFactura(Row.CodProducto, Row.Capacidad, "", Row.UnidadMedidaVenta, _
                            CDecDBNull(MontoFlete, 0), CDecDBNull(Row.TotalDescuentoContado, 0) + _
                            CDecDBNull(Row.TotalDescuentoCredito, 0), CDecDBNull(Row.TotalIvaCredito, 0) + _
                            CDecDBNull(Row.TotalIvaContado, 0), CDecDBNull(MontoTotalItem, 0), m_RowTalonarioFactura, _
                            TiposDocumento.FacturaAutomatica, TipoMovimientos.Factura, CShortDBNull(Row.UnidadesReales, 0), _
                            Row.PrecioContado + Row.PrecioCredito, Row.DescripcionProducto)
                        End If
                    End If

                    If GeneraRemision Then
                        'G E N E R A C I O N  R E M I S I O N  P A R A  H O M E C A R E
                        If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                            If CIntDBNull(Row.UnidadesVendidasCredito, 0) > 0 Then
                                If Row.TipoProducto = TipoProducto.Flete Then
                                    MontoFlete = CIntDBNull(Row.MontoTotalCredito, 0)
                                Else
                                    MontoFlete = 0
                                End If
                                MontoTotalItem = CIntDBNull(Row.MontoTotalCredito, 0)

                                InsertDetalleFactura(Row.CodProducto, Row.Capacidad, "", Row.UnidadMedidaVenta, _
                                CDecDBNull(MontoFlete, 0), CDecDBNull(Row.TotalDescuentoCredito, 0), _
                                CDecDBNull(Row.TotalIvaCredito, 0), CDecDBNull(MontoTotalItem, 0), _
                                m_RowTalonarioRemision, TiposDocumento.RemitoAutomatico, TipoMovimientos.Remision, _
                                CShortDBNull(Row.UnidadesVendidasCredito), CDecDBNull(Row.PrecioCredito, 0), _
                                Row.DescripcionProducto)

                                'D E T A L L E S  C O P A G O S  

                                If Row("MontoTotalCopago") IsNot DBNull.Value Then
                                    If Row.MontoTotalCopago > 0 Then
                                        ' Se inserta el encabezado de la factura
                                        If InsertEncabezadoFacturaRemision(Row.MontoTotalCopago, 0, 0, 0, TipoMovimientos.Factura, _
                                        m_RowTalonarioFactura, TiposDocumento.FacturaAutomatica) Then
                                            ' se inserta el encabezado de la guia
                                            InsertEncabezadoGuia(TipoMovimientos.Factura, m_RowTalonarioFactura)
                                            ' Se inserta el detalle de la factura
                                            InsertDetalleFactura(Nucleo.ProductoCopago, "", Pertenencia.Praxair, "cu", _
                                            0, 0, 0, Row.MontoTotalCopago, m_RowTalonarioFactura, TiposDocumento.FacturaAutomatica, _
                                            TipoMovimientos.Factura, 1, Row.MontoTotalCopago, "")
                                            ' se inserta el detalle de la guia
                                            InsertDetalleGuiaFacturasRemisiones(Nucleo.ProductoCopago, "", Pertenencia.Praxair, m_RowTalonarioFactura, _
                                            TipoMovimientos.Factura, "", "")

                                            ' Se guarda el copago en la tabla de movimientos
                                            Dim RowCopagos As PacientesDataSet.MovimientoCopagosCuotasRow
                                            RowCopagos = Pacientes.dsPacientes.MovimientoCopagosCuotas.FindByTipoTipoDocumentoNoDocumento(Autorizaciones.Copago, _
                                            TipoMovimientos.CopagoRemision, Row.IdDetalleAutorizacion)
                                            If RowCopagos IsNot Nothing Then
                                                RowCopagos.Monto += Row.MontoTotalCopago
                                            Else
                                                Pacientes.dsPacientes.MovimientoCopagosCuotas.AddMovimientoCopagosCuotasRow(Autorizaciones.Copago, _
                                                TipoMovimientos.CopagoRemision, Row.IdDetalleAutorizacion, Row.MontoTotalCopago, TipoDocumentos.Remision, _
                                                cstrDBNULL(m_RowTalonarioFactura("Actual")), "0")
                                            End If
                                            GeneraFactura = True
                                        Else
                                            UIHandler.EndWait()
                                            Return False
                                            Exit Function
                                        End If
                                    End If
                                End If

                                ' SE INSERTA EN LA TABLA DE AUTORIZACION REMISION
                                dsPacientes.AutorizacionRemision.AddAutorizacionRemisionRow(Row.IdDetalleAutorizacion, cstrDBNULL(m_RowTalonarioRemision("Actual")), _
                                Row.CodProducto, Row.Capacidad, Row.UnidadesVendidasCredito, "1")

                            End If
                        Else
                            ' G E N E R A C I O N  R E M I S I O N  P A R A  I N D U S T R I A L

                            If Row.TipoProducto = TipoProducto.Flete Then
                                MontoFlete = CDblDBNull(Row.MontoTotalContado, 0) + CDblDBNull(Row.MontoTotalCredito, 0)
                            Else
                                MontoFlete = 0
                            End If
                            MontoTotalItem = CDblDBNull(Row.MontoTotalContado, 0) + CDblDBNull(Row.MontoTotalCredito, 0)

                            InsertDetalleFactura(Row.CodProducto, Row.Capacidad, "", Row.UnidadMedidaVenta, _
                            CDecDBNull(MontoFlete, 0), CDecDBNull(Row.TotalDescuentoContado, 0), _
                            CDecDBNull(Row.TotalDescuentoContado, 0), CDecDBNull(MontoTotalItem, 0), _
                            m_RowTalonarioRemision, TiposDocumento.RemitoAutomatico, TipoMovimientos.Remision, _
                            CShortDBNull(Row.UnidadesReales, 0), CDecDBNull(Row.PrecioContado, 0), Row.DescripcionProducto)
                        End If
                    End If
                End If

                'A S I G N A C I O N E S
                If Row.UnidadesReales > Row.Recolecciones And Row.Lastro = Lastro.Controla And Row.RequiereAsignacion = Asignacion.RequiereAsignacion Then
                    GeneraAsignacion = True

                    ' Se obtiene el Numero del documento para la asignacion
                    If m_RowTalonarioAsignacion Is Nothing Then
                        If Not ObtenerNoFactura(m_RowTalonarioAsignacion, TiposDocumento.AsignacionAutomatica) Then
                            UIHandler.EndWait()
                            Return False
                            Exit Function
                        End If
                    End If

                    If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                        If Not GenerarAsignacionPaciente(Row, m_RowTalonarioFactura, CInt(Row.UnidadesReales - Row.Recolecciones), _
                        m_RowTalonarioRemision) Then
                            UIHandler.EndWait()
                            Return False
                            Exit Function
                        End If
                    Else
                        dsVenta.DetalleGuiaAsignacionesRecolecciones.AddDetalleGuiaAsignacionesRecoleccionesRow(Nucleo.NumeroMovimiento, _
                        TipoGuias.Asignacion, TipoMovimientos.AsignacionDetalleguia, Row.CodProducto, Row.Capacidad, Pertenencia.Praxair, _
                        CShort(Row.UnidadesReales - Row.Recolecciones), cstrDBNULL(m_RowTalonarioAsignacion("Actual")), _
                        cstrDBNULL(m_RowTalonarioAsignacion("Prefijo")), Row.UnidadMedidaVenta)
                    End If
                End If

                ' R E C O L E C C I O N E S  G E N E R A D A  A P A R T I R  D E   L A   V E N T A
                If cstrDBNULL(m_RowCliente("CodTipoCliente")) <> TiposCliente.Paciente Then
                    If Row.UnidadesReales < Row.Recolecciones Then
                        GeneraRecoleccion = True

                        If m_RowTalonarioRecoleccion Is Nothing Then
                            If Not ObtenerNoFactura(m_RowTalonarioRecoleccion, TiposDocumento.RecoleccionAutomatico) Then
                                UIHandler.EndWait()
                                Return False
                                Exit Function
                            End If
                        End If

                        ' M A E S T  R O  D E  G U I A S
                        InsertEncabezadoGuia(TipoMovimientos.Recoleccion, m_RowTalonarioRecoleccion)

                        ' D E T A L L E  D E   G U I A S  A S I G N A C I O N E S / R E C O L E C C I O N E S
                        dsVenta.DetalleGuiaAsignacionesRecolecciones.AddDetalleGuiaAsignacionesRecoleccionesRow(Nucleo.NumeroMovimiento, _
                        TipoGuias.Recojo, TipoMovimientos.RecojoVacios, Row.CodProducto, Row.Capacidad, Pertenencia.Praxair, _
                        CShort(Row.Recolecciones) - CShort(Row.UnidadesReales), cstrDBNULL(m_RowTalonarioRecoleccion("Actual")), _
                        cstrDBNULL(m_RowTalonarioRecoleccion("Prefijo")), Row.UnidadMedidaVenta)

                        ' Se Actualiza el kardex para los activos de praxair
                        Dim RowKardex As ProductosDataSet.KardexCamionRow
                        RowKardex = Productos.dsProductos.KardexCamion.FindByCodProductoCapacidadCodTipoProducto(Row.CodProducto, _
                        Row.Capacidad, TipoProducto.Activo)
                        If RowKardex IsNot Nothing Then
                            RowKardex.SaldoPraxair += CShort(1)
                        End If
                    End If
                End If

                'D E T A L L E   G U I A S  F A C T U R A S  Y  R E M I S I O N E S

                RowLeidos = m_CilindrosLeidos.Select("CodProducto =" & Row.CodProducto & "And Capacidad =" & Row.Capacidad)
                If RowLeidos.Length > 0 Then
                    For i As Integer = 0 To RowLeidos.Length - 1
                        InsertarDetalleGuiasFacturasRemisiones(m_RowTalonarioFactura, _
                        m_RowTalonarioRemision, Row.UnidadesVendidasCredito, Row.UnidadesVendidasContado, _
                        cstrDBNULL(RowLeidos(i)("CodProducto")), cstrDBNULL(RowLeidos(i)("Pertenencia")), _
                        cstrDBNULL(RowLeidos(i)("Capacidad")), cstrDBNULL(RowLeidos(i)("SecuencialAjeno")), _
                        cstrDBNULL(RowLeidos(i)("Secuencial")), CShortDBNull(RowLeidos(i)("Cantidad")), _
                        cstrDBNULL(RowLeidos(i)("CodSucursal")), cstrDBNULL(RowLeidos(i)("CodTipoProducto")), _
                        cstrDBNULL(RowLeidos(i)("Credito")))
                    Next
                Else
                    If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                        ' Se inserta el Flete o el servicio
                        If CDecDBNull(Row.UnidadesVendidasContado, 0) > 0 Then
                            InsertDetalleGuiaFacturasRemisiones(Row.CodProducto, Row.Capacidad, _
                            Pertenencia.Praxair, m_RowTalonarioFactura, TipoMovimientos.Factura, _
                            "", "")
                        End If

                        If CDecDBNull(Row.UnidadesVendidasCredito, 0) > 0 Then
                            InsertDetalleGuiaFacturasRemisiones(Row.CodProducto, Row.Capacidad, _
                            Pertenencia.Praxair, m_RowTalonarioRemision, TipoMovimientos.Remision, _
                            "", "")
                        End If
                    Else
                        If GeneraFactura Then
                            InsertDetalleGuiaFacturasRemisiones(Row.CodProducto, Row.Capacidad, _
                            Pertenencia.Praxair, m_RowTalonarioFactura, TipoMovimientos.Factura, _
                            "", "")
                        ElseIf GeneraRemision Then
                            InsertDetalleGuiaFacturasRemisiones(Row.CodProducto, Row.Capacidad, _
                           Pertenencia.Praxair, m_RowTalonarioRemision, TipoMovimientos.Remision, _
                           "", "")
                        End If
                    End If
                End If
            Next

            ' Se valida si se genero cancelación de deudas
            If GeneraPagoDeuda Then
                Dim Monto As Decimal
                Dim Impuesto As Decimal

                Monto = CDec(Pacientes.dsPacientes.DeudasPagadas.Compute("SUM(SubTotal)", "1=1"))
                Impuesto = CDec(Pacientes.dsPacientes.DeudasPagadas.Compute("SUM(MontoIva)", "1=1"))

                ' Se crea el encabezado de la factura
                If InsertEncabezadoFacturaRemision(Monto, Impuesto, 0, 0, TipoMovimientos.Factura, _
                m_RowTalonarioFactura, TiposDocumento.FacturaAutomatica) Then
                    InsertEncabezadoGuia(TipoMovimientos.Factura, m_RowTalonarioFactura)

                    For Each Row As PacientesDataSet.DeudasPagadasRow In Pacientes.dsPacientes.DeudasPagadas.Rows
                        ActualizarAlquileres(Row)
                        ' Se agrega el detalle de la factura
                        InsertDetalleFactura(Row.CodProducto, Row.Capacidad, Row.Pertenencia, Row.UnidadVenta, _
                        0, 0, CDecDBNull(Row.MontoIva, 0), CDecDBNull(Row.SubTotal, 0), m_RowTalonarioFactura, _
                        TiposDocumento.FacturaAutomatica, TipoMovimientos.Factura, CShortDBNull(Row.DiasCancelados, 0), _
                        CDecDBNull(Row.Precio, 0), Row.Descripcion)

                        ' Se agrega el detalle de la guia
                        InsertDetalleGuiaFacturasRemisiones(Row.CodProducto, Row.Capacidad, Row.Pertenencia, _
                        m_RowTalonarioFactura, TipoMovimientos.Factura, "", "")
                    Next
                Else
                    UIHandler.EndWait()
                    Return False
                    Exit Function
                End If
            End If

            'Se valida si se realizo recolección Pura
            If CIntDBNull(m_CilindrosLeidos.Compute("Count(Secuencial)", "Secuencial = '' And CodTipoProducto = ''"), 0) > 0 Then
                If Not GenerarRecoleccionPura() Then
                    UIHandler.EndWait()
                    Return False
                    Exit Function
                End If
                GeneraRecoleccion = True
            End If

            If GeneraEntregaAjeno Then
                Dim RowAjenos() As DataRow
                RowAjenos = m_CilindrosLeidos.Select("Secuencial = '' And SecuencialAjeno <> '' And  CodTipoProducto = '5'")
                If RowAjenos.Length > 0 Then
                    For i As Integer = 0 To RowAjenos.Length - 1
                        If InsertEncabezadoFacturaRemision(0, 0, 0, 0, TipoMovimientos.Remision, m_RowTalonarioRemision, _
                            TiposDocumento.RemitoAutomatico) Then

                            InsertEncabezadoGuia(TipoMovimientos.Remision, m_RowTalonarioRemision)

                            InsertDetalleFactura(cstrDBNULL(RowAjenos(i)("CodProducto")), cstrDBNULL(RowAjenos(i)("Capacidad")), _
                            cstrDBNULL(RowAjenos(i)("Pertenencia")), cstrDBNULL(RowAjenos(i)("UnidadMedida")), 0, 0, 0, 0, _
                            m_RowTalonarioRemision, TiposDocumento.RemitoAutomatico, TipoMovimientos.Remision, _
                            CShortDBNull(RowAjenos(i)("Cantidad")), 0, "")

                            InsertDetalleGuiaFacturasRemisiones(cstrDBNULL(RowAjenos(i)("CodProducto")), cstrDBNULL(RowAjenos(i)("Capacidad")), _
                            cstrDBNULL(RowAjenos(i)("Pertenencia")), m_RowTalonarioRemision, TipoMovimientos.Remision, _
                            cstrDBNULL(RowAjenos(i)("Secuencial")), cstrDBNULL(RowAjenos(i)("SecuencialAjeno")))

                            ' S E  A C T U A L I Z A  C A R G U E  Y  K A R D E X  D E  C A M I O N 
                            Productos.ActualizarCargueKardex(cstrDBNULL(RowAjenos(i)("CodProducto")), cstrDBNULL(RowAjenos(i)("CodSucursal")), _
                            cstrDBNULL(RowAjenos(i)("SecuencialAjeno")), cstrDBNULL(RowAjenos(i)("Secuencial")), cstrDBNULL(RowAjenos(i)("CodTipoProducto")), _
                            cstrDBNULL(RowAjenos(i)("Pertenencia")), cstrDBNULL(RowAjenos(i)("Capacidad")), CShortDBNull(RowAjenos(i)("Cantidad")))
                        Else
                            UIHandler.EndWait()
                            Return False
                            Exit Function
                        End If
                    Next
                    GeneraRemision = True
                End If
            End If
            UIHandler.EndWait()
            Return True

        Finally
            Venta.CloseConnection()
        End Try
    End Function

    Public Function InsertEncabezadoFacturaRemision(ByVal MontoTotal As Decimal, ByVal MontoImpuesto As Decimal, _
    ByVal MontoFlete As Decimal, ByVal MontoDescuento As Decimal, ByVal sTipoMovimiento As String, _
    ByRef RowTalonario As DataRow, ByVal TipoDocumento As Short) As Boolean
        Dim Row As DataRow

        If RowTalonario Is Nothing Then
            ' Se obtiene el talonario
            If Not ObtenerNoFactura(RowTalonario, TipoDocumento) Then
                Return False
                Exit Function
            End If
        End If

        Row = dsVenta.MaestroFacturas.FindByTipoFacturaNoFacturaPrefijo(sTipoMovimiento, cstrDBNULL(RowTalonario("Actual")), _
        cstrDBNULL(RowTalonario("Prefijo")))

        If Row Is Nothing Then
            dsVenta.MaestroFacturas.AddMaestroFacturasRow(sTipoMovimiento, cstrDBNULL(RowTalonario("Actual")), _
            cstrDBNULL(RowTalonario("Prefijo")), Nucleo.CodigoSucursal, cstrDBNULL(m_RowCliente("Codigo")), _
            Today, TipoMoneda.Nacional, "", Nucleo.RutaPrincipal, Nucleo.CodigoTrasportadora, "001", _
            Nucleo.CodigoVehiculo, "1", cstrDBNULL(m_RowCliente("TipoPago")), CShortDBNull(m_RowCliente("DiasCredito")), _
            "", "", MontoTotal, MontoFlete, MontoDescuento, MontoImpuesto, cstrDBNULL(m_RowPedido("CodEntidad")), _
            cstrDBNULL(m_RowPedido("NoPedido")))
        Else
            Row("MontoFactura") = CDec(Row("MontoFactura")) + CDec(MontoTotal)
            Row("ImpuestoTotal") = CDec(Row("ImpuestoTotal")) + CDec(MontoImpuesto)
            Row("MontoFlete") = CDec(Row("MontoFlete")) + CDec(MontoFlete)
            Row("Descuento") = CDec(Row("Descuento")) + CDec(MontoDescuento)
        End If

        Return True
    End Function

    Public Sub InsertEncabezadoGuia(ByVal sTipoMovimiento As String, ByVal RowTalonario As DataRow)
        Dim Row As VentaDataSet.MaestroGuiasRow
        Row = dsVenta.MaestroGuias.FindByNoMovimientoTipoDocumento(Nucleo.NumeroMovimiento, sTipoMovimiento)
        If Row Is Nothing Then
            dsVenta.MaestroGuias.AddMaestroGuiasRow(Nucleo.NumeroMovimiento, Nucleo.CodigoSucursal, "", _
            cstrDBNULL(m_RowCliente("Codigo")), Today, sTipoMovimiento, cstrDBNULL(RowTalonario("Actual")), _
            Nucleo.RutaPrincipal, Nucleo.CodigoTrasportadora, "001", Nucleo.CodigoVehiculo, "", "1", _
            cstrDBNULL(m_RowPedido("NoPedido")), cstrDBNULL(RowTalonario("Prefijo")))
        End If
    End Sub

    Private Sub InsertDetalleGuiaFacturasRemisiones(ByVal sCodProducto As String, ByVal sCapacidad As String, _
    ByVal sPertenencia As String, ByVal RowTalonario As DataRow, ByVal TipoMovimiento As String, _
    ByVal Secuencial As String, ByVal SerialAjeno As String)
        Dim Row As VentaDataSet.DetalleGuiaFacturasRemisionesRow

        Row = dsVenta.DetalleGuiaFacturasRemisiones.FindByNoMovimientoTipoMovimientoCodSucursalCodProductoCapacidadSecuencialPertenenciaSerialAjenoPrefijoNoGuia(Nucleo.NumeroMovimiento, _
        TipoMovimiento, Nucleo.CodigoSucursal, sCodProducto, sCapacidad, Secuencial, sPertenencia, SerialAjeno, _
        cstrDBNULL(RowTalonario("Prefijo")), cstrDBNULL(RowTalonario("Actual")))

        If Row Is Nothing Then
            dsVenta.DetalleGuiaFacturasRemisiones.AddDetalleGuiaFacturasRemisionesRow(Nucleo.NumeroMovimiento, TipoMovimiento, _
            Nucleo.CodigoSucursal, sCodProducto, sCapacidad, Secuencial, sPertenencia, SerialAjeno, cstrDBNULL(RowTalonario("Prefijo")), _
            cstrDBNULL(RowTalonario("Actual")))
        End If
    End Sub

    Private Sub InsertDetalleFactura(ByVal sCodProducto As String, ByVal sCapacidad As String, _
        ByVal sPertenencia As String, ByVal UnidadMedida As String, ByVal MontoFlete As Decimal, _
        ByVal MontoDescuento As Decimal, ByVal MontoImpuesto As Decimal, ByVal MontoTotal As Decimal, _
        ByVal RowTalonario As DataRow, ByVal TipoDocumento As Short, ByVal TipoMovimiento As String, _
        ByVal Cantidad As Short, ByVal PrecioUnitario As Decimal, ByVal Descripcion As String)
        Dim Row As VentaDataSet.DetalleFacturaRow

        Row = dsVenta.DetalleFactura.FindByTipoFacturaNoFacturaPrefijoCodProductoCapacidadPertenencia(TipoMovimiento, _
        cstrDBNULL(RowTalonario("Actual")), cstrDBNULL(RowTalonario("Prefijo")), sCodProducto, sCapacidad, sPertenencia)
        If Row IsNot Nothing Then
            ' Se actualiza
            If Not sCodProducto = Nucleo.ProductoCopago Then
                Row.Cantidad += Cantidad
            Else
                Row.Cantidad = Cantidad
                Row.PrecioUnitario += Math.Round(PrecioUnitario)
            End If

            Row.MontoFlete += Math.Round(MontoFlete)
            Row.MontoDescuento += Math.Round(MontoDescuento)
            Row.MontoImpuesto += Math.Round(MontoImpuesto)
            Row.MontoTotalItem += Math.Round(MontoTotal)
        Else
            ' Se inserta
            If Descripcion = "" Then
                Descripcion = Productos.NombreProducto(sCodProducto).TrimEnd
            End If

            dsVenta.DetalleFactura.AddDetalleFacturaRow(TipoMovimiento, cstrDBNULL(RowTalonario("Actual")), _
            cstrDBNULL(RowTalonario("Prefijo")), sCodProducto, sCapacidad, sPertenencia, UnidadMedida, sCapacidad, _
            UnidadMedida, Cantidad, Math.Round(PrecioUnitario), Math.Round(MontoFlete), Math.Round(MontoDescuento), _
            Math.Round(MontoImpuesto), 0, 0, "", Math.Round(MontoTotal), Descripcion)
        End If
    End Sub

    Public Sub InsertarDetalleGuiasFacturasRemisiones(ByVal RowTalonarioFactura As DataRow, _
    ByVal RowTalonarioRemision As DataRow, ByVal UnidadesVendidasCredito As Short, _
    ByVal UnidadesVendidasContado As Short, ByVal sCodProducto As String, ByVal sPertenencia As String, _
    ByVal sCapacidad As String, ByVal sSecuencialAjeno As String, ByVal sSecuencial As String, _
    ByVal Cantidad As Short, ByVal sSucursal As String, ByVal sCodTipoProducto As String, ByVal Credito As String)
        If GeneraFactura Then
            If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                If UnidadesVendidasContado > 0 And Credito = "N" Then
                    InsertDetalleGuiaFacturasRemisiones(sCodProducto, sCapacidad, sPertenencia, _
                    m_RowTalonarioFactura, TipoMovimientos.Factura, sSecuencial, sSecuencialAjeno)

                    If sCodTipoProducto = TipoProducto.Producto Or sCodTipoProducto = TipoProducto.Activo Then
                        ' S E  A C T U A L I Z A  C A R G U E  Y  K A R D E X  D E  C A M I O N 
                        Productos.ActualizarCargueKardex(sCodProducto, sSucursal, sSecuencialAjeno, sSecuencial, _
                        sCodTipoProducto, sPertenencia, sCapacidad, Cantidad)
                    End If
                End If
            Else
                InsertDetalleGuiaFacturasRemisiones(sCodProducto, sCapacidad, sPertenencia, _
                m_RowTalonarioFactura, TipoMovimientos.Factura, sSecuencial, sSecuencialAjeno)

                If sCodTipoProducto = TipoProducto.Producto Or sCodTipoProducto = TipoProducto.Activo Then
                    ' S E  A C T U A L I Z A  C A R G U E  Y  K A R D E X  D E  C A M I O N 
                    Productos.ActualizarCargueKardex(sCodProducto, sSucursal, sSecuencialAjeno, sSecuencial, _
                    sCodTipoProducto, sPertenencia, sCapacidad, Cantidad)
                End If
            End If
        End If

        If GeneraRemision Then
            If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                If UnidadesVendidasCredito > 0 And Credito = "S" Or UnidadesVendidasCredito > 0 And Credito = "N" Then
                    InsertDetalleGuiaFacturasRemisiones(sCodProducto, sCapacidad, sPertenencia, _
                    m_RowTalonarioRemision, TipoMovimientos.Remision, sSecuencial, sSecuencialAjeno)

                    If sCodTipoProducto = TipoProducto.Producto Or sCodTipoProducto = TipoProducto.Activo Then
                        ' S E  A C T U A L I Z A  C A R G U E  Y  K A R D E X  D E  C A M I O N 
                        Productos.ActualizarCargueKardex(sCodProducto, sSucursal, sSecuencialAjeno, sSecuencial, _
                        sCodTipoProducto, sPertenencia, sCapacidad, Cantidad)
                    End If
                End If
            Else
                InsertDetalleGuiaFacturasRemisiones(sCodProducto, sCapacidad, sPertenencia, _
                m_RowTalonarioRemision, TipoMovimientos.Remision, sSecuencial, sSecuencialAjeno)

                If sCodTipoProducto = TipoProducto.Producto Or sCodTipoProducto = TipoProducto.Activo Then
                    ' S E  A C T U A L I Z A  C A R G U E  Y  K A R D E X  D E  C A M I O N 
                    Productos.ActualizarCargueKardex(sCodProducto, sSucursal, sSecuencialAjeno, sSecuencial, _
                    sCodTipoProducto, sPertenencia, sCapacidad, Cantidad)
                End If
            End If
        End If
    End Sub

    Public Sub ActualizarAlquileres(ByVal Row As PacientesDataSet.DeudasPagadasRow)
        'Dim RowAlquileresPendientes As PacientesDataSet.AlquileresPendientesRow
        Dim RowAlquileresPagados As PacientesDataSet.AlquileresPagadosRow

        '' Se actualiza la tabla de Alquileres pendientes
        'RowAlquileresPendientes = Pacientes.dsPacientes.AlquileresPendientes.FindByNoAlquiler(Row.NoAlquiler)
        'If RowAlquileresPendientes IsNot Nothing Then
        '    If RowAlquileresPendientes.Dias < Row.DiasCancelados Then
        '        RowAlquileresPendientes.Dias = RowAlquileresPendientes.Dias - Row.DiasCancelados
        '    Else
        '        RowAlquileresPendientes.Dias = 0
        '    End If
        'End If

        ' Se actualiza la tabla de Alquileres Pagados
        RowAlquileresPagados = Pacientes.dsPacientes.AlquileresPagados.FindByNoAsignacionTipoDocumento(Row.NoAsignacion, TipoDocumentos.Factura)
        If RowAlquileresPagados Is Nothing Then
            Pacientes.dsPacientes.AlquileresPagados.AddAlquileresPagadosRow(Row.NoAsignacion, TipoDocumentos.Factura, _
            cstrDBNULL(m_RowTalonarioFactura("Actual")), Row.FechaInicio, Row.FechaInicio.AddDays(CDbl(Row.DiasCancelados)), _
            Row.DiasCancelados)
        Else

        End If
    End Sub

    Public Function GenerarEncabezadosFactura(ByVal RowTalonario As DataRow) As Boolean
        Dim MontoTotal As Double
        Dim MontoTotalFlete As Double
        Dim DescuentoTotal As Double
        Dim ImpuestoTotal As Double
        Dim MontoTotalCopago As Double = 0

        If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
            ' Se calculan los totales
            MontoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalContado)", "TipoProducto <> '4'"))
            MontoTotalFlete = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalContado)", "TipoProducto = 4"))
            DescuentoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalDescuentoContado)", "1=1"))
            ImpuestoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalIvaContado)", "1=1"))
            MontoTotalCopago = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalCopago)", "1=1"))
        Else
            MontoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalContado)", "TipoProducto <> '4'")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalCredito)", "TipoProducto <> '4'"))
            MontoTotalFlete = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalContado)", "TipoProducto = 4")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalCredito)", "TipoProducto = 4"))
            DescuentoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalDescuentoContado)", "1=1")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(TotalDescuentoCredito)", "1=1"))
            ImpuestoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalIvaContado)", "1=1")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(TotalIvaCredito)", "1=1"))
        End If

        ' Se genera el maestro de facturas
        If InsertEncabezadoFacturaRemision(CDec(MontoTotal + MontoTotalFlete), CDec(ImpuestoTotal), _
        CDec(MontoTotalFlete), CDec(DescuentoTotal), TipoMovimientos.Factura, _
        m_RowTalonarioFactura, TiposDocumento.FacturaAutomatica) Then
            ' Se genera el maestro de guias
            InsertEncabezadoGuia(TipoMovimientos.Factura, m_RowTalonarioFactura)
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function

    Public Function GenerarEncabezadosRemision(ByVal RowTalonario As DataRow) As Boolean
        Dim MontoTotal As Double
        Dim MontoTotalFlete As Double
        Dim DescuentoTotal As Double
        Dim ImpuestoTotal As Double

        If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
            ' Se calculan los totales
            MontoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalCredito)", "TipoProducto <> '4'"))
            MontoTotalFlete = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalCredito)", "TipoProducto = 4"))
            DescuentoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalDescuentoCredito)", "1=1"))
            ImpuestoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalIvaCredito)", "1=1"))
        Else
            MontoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalContado)", "TipoProducto <> '4'")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalCredito)", "TipoProducto <> '4'"))

            MontoTotalFlete = CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalContado)", "TipoProducto = 4")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(MontoTotalCredito)", "TipoProducto = 4"))

            DescuentoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalDescuentoContado)", "1=1")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(TotalDescuentoCredito)", "1=1"))

            ImpuestoTotal = CDblDBNull(m_DetallePedido.Compute("Sum(TotalIvaContado)", "1=1")) + _
            CDblDBNull(m_DetallePedido.Compute("Sum(TotalIvaCredito)", "1=1"))
        End If

        ' Se genera el maestro de facturas
        If InsertEncabezadoFacturaRemision(CDec(MontoTotal + MontoTotalFlete), CDec(ImpuestoTotal), _
        CDec(MontoTotalFlete), CDec(DescuentoTotal), TipoMovimientos.Remision, m_RowTalonarioRemision, _
        TiposDocumento.RemitoAutomatico) Then
            ' Se genera el maestro de guias
            InsertEncabezadoGuia(TipoMovimientos.Remision, m_RowTalonarioRemision)
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function

    Public Function GrabarRecolecciones(ByVal sCodProducto As String, ByVal sCapacidad As String, _
    ByVal sPertenencia As String, ByRef RowTalonarioRecolecciones As DataRow, ByVal Cantidad As Short, _
    ByVal sSecuencialAjeno As String, ByVal sUnidadVenta As String) As Boolean
        Dim rowRecolecciones As VentaDataSet.DetalleGuiaAsignacionesRecoleccionesRow

        If RowTalonarioRecolecciones Is Nothing Then
            If Not ObtenerNoFactura(RowTalonarioRecolecciones, TiposDocumento.RecoleccionAutomatico) Then
                Return False
                Exit Function
            End If
        End If

        ' Se graba el encabezado de la guia
        InsertEncabezadoGuia(TipoMovimientos.Recoleccion, RowTalonarioRecolecciones)

        rowRecolecciones = dsVenta.DetalleGuiaAsignacionesRecolecciones.FindByNoMovimientoTipoGuiaTipoMovimientoCodProductoCapacidadPertenenciaNoGuiaPrefijo(Nucleo.NumeroMovimiento, TipoGuias.Recojo, _
        TipoMovimientos.RecojoVacios, sCodProducto, sCapacidad, sPertenencia, cstrDBNULL(RowTalonarioRecolecciones("Actual")), cstrDBNULL(RowTalonarioRecolecciones("Prefijo")))

        If rowRecolecciones IsNot Nothing Then
            rowRecolecciones.Cantidad += Cantidad
        Else
            dsVenta.DetalleGuiaAsignacionesRecolecciones.AddDetalleGuiaAsignacionesRecoleccionesRow(Nucleo.NumeroMovimiento, TipoGuias.Recojo, _
            TipoMovimientos.RecojoVacios, sCodProducto, sCapacidad, sPertenencia, Cantidad, cstrDBNULL(RowTalonarioRecolecciones("Actual")), _
            cstrDBNULL(RowTalonarioRecolecciones("Prefijo")), sUnidadVenta)
        End If

        If sPertenencia = Pertenencia.Cliente Then
            ' Si es Recolección Ajena se graba el detalle de recolecciones ajenas
            dsVenta.DetalleGuiaRecoleccionesAjenos.AddDetalleGuiaRecoleccionesAjenosRow(Nucleo.NumeroMovimiento, cstrDBNULL(RowTalonarioRecolecciones("Actual")), TipoMovimientos.RecojoVacios, _
            sCodProducto, sCapacidad, sSecuencialAjeno, Nucleo.CodigoSucursal, "", cstrDBNULL(RowTalonarioRecolecciones("Prefijo")))
        End If

        Return True
    End Function

    Public Function GenerarAsignacionPaciente(ByVal Row As PedidosDataSet.DetallePedidoRow, _
    ByRef RowTalonarioFactura As DataRow, ByVal Cantidad As Integer, ByRef RowTalonarioRemision As DataRow) As Boolean

        Dim CodAlquiler As String = ""
        Dim sCodTipoAsignacion As String = ""
        Dim ConsecutivoAsignacion As String = ""

        Dim RowAutorizacion() As DataRow
        Dim RowTalonarioCopago As DataRow = Nothing
        Dim IdDetalleAutorizacion As String = ""
        Dim IdAutorizacion As String = ""
        Dim TipoPago As String = ""

        Dim DiasMinimos As Short
        Dim Credito As Boolean = False

        Dim CodProductoCopago As String = ""
        Dim MontoDeposito As Decimal = 0


        ' T I P O   D E  A S I G N A C I O N
        Pacientes.GetTipoAsignacion(Row.CodProducto, Row.Capacidad)
        If dsPacientes.DetallesTipoAsignacion.Rows.Count > 0 Then
            sCodTipoAsignacion = cstrDBNULL(dsPacientes.DetallesTipoAsignacion.Rows(0)("Codigo"))
        End If

        ' Se busca el alquiler en la tabla de productos
        CodAlquiler = "2" & cstrDBNULL(Row.CodProducto).Substring(1)
        Productos.LoadProducto(CodAlquiler)

        ' G U A R D A  E L  D E T A L L E   D E  L A  G U I A  D E  A S I G N A C I O N E S
        If CIntDBNull(Row.UnidadesVendidasContado) > 0 Then
            dsVenta.DetalleGuiaAsignacionesRecolecciones.AddDetalleGuiaAsignacionesRecoleccionesRow(Nucleo.NumeroMovimiento, _
            TipoGuias.Asignacion, TipoMovimientos.AsignacionDetalleguia, Row.CodProducto, Row.Capacidad, Pertenencia.Praxair, _
            CShort(Cantidad), cstrDBNULL(m_RowTalonarioAsignacion("Actual")), cstrDBNULL(m_RowTalonarioAsignacion("Prefijo")), _
            Row.UnidadMedidaVenta)
        Else
            dsVenta.DetalleGuiaAsignacionesRecolecciones.AddDetalleGuiaAsignacionesRecoleccionesRow(Nucleo.NumeroMovimiento, _
            TipoGuias.Asignacion, TipoMovimientos.AsignacionDetalleguia, Row.CodProducto, Row.Capacidad, Pertenencia.Praxair, _
            CShort(Cantidad), cstrDBNULL(m_RowTalonarioAsignacion("Actual")), cstrDBNULL(m_RowTalonarioAsignacion("Prefijo")), _
            Row.UnidadMedidaVenta)
        End If
        ConsecutivoAsignacion = Nucleo.ConsecutivoAsignaciones

        For i As Integer = 1 To Cantidad
            Credito = False

            ' A U T O R I Z A C I O N  D E  A L Q U I L E R E S
            RowAutorizacion = Pacientes.dsPacientes.DetalleAutorizaciones.Select("CodProducto =" & CodAlquiler.Trim)
            If RowAutorizacion Is Nothing Then

                ' A L Q U I L E R E S  D E  C O N T A D O
                If Not GuardarAlquilerContado(bRespetaPrecio, CodAlquiler, sCodTipoAsignacion, RowTalonarioFactura, _
                ConsecutivoAsignacion, cstrDBNULL(Row.CodProducto)) Then
                    Return False
                    Exit Function
                End If

            ElseIf RowAutorizacion.Length > 0 Then
                IdDetalleAutorizacion = cstrDBNULL(RowAutorizacion(0)("IdDetalleAutorizacion"))
                IdAutorizacion = cstrDBNULL(RowAutorizacion(0)("NoAutorizacion"))
                TipoPago = cstrDBNULL(RowAutorizacion(0)("TipoPago"))

                ' A L Q U I L E R E S  D I S P O N I B L E S
                Dim RowAlquileres() As DataRow = Nothing
                RowAlquileres = Pacientes.dsPacientes.Alquileres.Select("IdDetalleAutorizacion = " & IdDetalleAutorizacion & "And Estado = '0'")

                If RowAlquileres Is Nothing Then
                    ' A L Q U I L E R E S  D E  C O N T A D O
                    If Not GuardarAlquilerContado(bRespetaPrecio, CodAlquiler, sCodTipoAsignacion, RowTalonarioFactura, _
                    ConsecutivoAsignacion, cstrDBNULL(Row.CodProducto)) Then
                        Return False
                        Exit Function
                    End If

                ElseIf RowAlquileres.Length = 0 Then
                    If Not GuardarAlquilerContado(bRespetaPrecio, CodAlquiler, sCodTipoAsignacion, RowTalonarioFactura, _
                    ConsecutivoAsignacion, cstrDBNULL(Row.CodProducto)) Then
                        Return False
                        Exit Function
                    End If
                Else
                    Credito = True
                    If Not GuardarAlquilerCredito(bRespetaPrecio, CodAlquiler, sCodTipoAsignacion, RowTalonarioRemision, _
                    ConsecutivoAsignacion, CDblDBNull(dsProductos.Productos.Rows(0)("Iva")), _
                    RowTalonarioFactura, RowAlquileres(0), cstrDBNULL(Row.CodProducto)) Then
                        Return False
                        Exit Function
                    End If

                    ' Se actualiza el alquiler para que no vuelva a ser usado
                    RowAlquileres(0)("CodTipoAsignacion") = sCodTipoAsignacion
                    RowAlquileres(0)("Estado") = Asignacion.AsignadoNormal
                    'Pacientes.UpdateAlquileres()

                    ' Se incrementa el consecutivo del alquiler
                    Nucleo.ConsecutivoAlquileres = CStr(CInt(Nucleo.ConsecutivoAlquileres) + 1)
                End If
            Else
                ' A L Q U I L E R E S  D E  C O N T A D O
                If Not GuardarAlquilerContado(bRespetaPrecio, CodAlquiler, sCodTipoAsignacion, RowTalonarioFactura, _
                ConsecutivoAsignacion, cstrDBNULL(Row.CodProducto)) Then
                    Return False
                    Exit Function
                End If
            End If

            ' G U A R D A  L A  A S I G N A C I O N
            Dim RowAsigna As PacientesDataSet.AsignacionesRow
            RowAsigna = dsPacientes.Asignaciones.NewAsignacionesRow()
            RowAsigna.NoAsignacion = ConsecutivoAsignacion
            RowAsigna.CodCliente = cstrDBNULL(m_RowCliente("Codigo"))
            RowAsigna.CodEntidad = cstrDBNULL(m_RowPedido("CodEntidad"))
            If Credito Then
                RowAsigna.TipoPago = Asignacion.AsignacionCredito
            Else
                RowAsigna.TipoPago = Asignacion.AsignacionContado
                RowAsigna.FechaCorte = Today.AddDays(DiasMinimos)
            End If
            RowAsigna.NoDocAsignacion = cstrDBNULL(m_RowTalonarioAsignacion("Actual"))
            RowAsigna.Nuevo = "2"
            RowAsigna.Modificado = "0"
            RowAsigna.CodTipoAsignacion = sCodTipoAsignacion
            RowAsigna.SolicitadaRecoger = "0"
            RowAsigna.FechaInicio = Today
            dsPacientes.Asignaciones.AddAsignacionesRow(RowAsigna)

            ' A U T O R I Z A C I O N  -  A S I G N A C I O N
            dsPacientes.AutorizacionAsignacion.AddAutorizacionAsignacionRow(IdDetalleAutorizacion, ConsecutivoAsignacion)
            ConsecutivoAsignacion = CStr(CInt(ConsecutivoAsignacion) + 1)
        Next

        Nucleo.ConsecutivoAsignaciones = ConsecutivoAsignacion
        Return True
    End Function

    Public Function GuardarAlquilerContado(ByVal bRespetaPrecio As Boolean, ByVal sCodAlquiler As String, ByVal sCodTipoAsignacion As String, _
    ByRef RowTalonarioFactura As DataRow, ByVal ConsecutivoAsignacion As String, ByVal CodProducto As String) As Boolean

        Dim PrecioNeto As Double
        Dim PorcentajeDescuento As Double = 0
        Dim TotalDescuento As Double = 0
        Dim MontoDeposito As Decimal
        Dim DiasMinimos As Short
        Dim RowAlquiler As ProductosDataSet.ProductosRow = Nothing
        Dim SubTotal As Double = 0
        Dim Iva As Double = 0
        Dim MontoTotal As Double = 0
        Dim PorcentajeIva As Double


        RowAlquiler = Productos.GetProducto(sCodAlquiler)
        If RowAlquiler IsNot Nothing Then
            PorcentajeIva = RowAlquiler.Iva
        End If

        ' S E  C O N S U L T A   E L   T I P O   D E  A S I G N AC I O  N
        Pacientes.GetDepositosTipoAsignacion(sCodTipoAsignacion)
        If dsPacientes.TipoAsignaciones.Rows.Count = 0 Then
            MsgBox("No se encontró el tipo de asignación no se puede grabar el alquiler!!")
            Return False
            Exit Function
        End If

        MontoDeposito = CDec(dsPacientes.TipoAsignaciones.Rows(0)("MontoDeposito"))
        DiasMinimos = CShortDBNull(dsPacientes.TipoAsignaciones.Rows(0)("DiasMinimosAlquiler"))

        ' D E P O S I T O S
        If bPagoDeposito Then
            GuardarDepositoContado(sCodTipoAsignacion, ConsecutivoAsignacion, _
            CodProducto, cstrDBNULL(dsPacientes.TipoAsignaciones.Rows(0)("Descripcion")), _
            MontoDeposito, "1", "1")
        End If

        ' S E  O B T I E N E   E L  P R E C I O  D E L   A L Q U I L E R
        Venta.ObtenerPrecio(m_RowCliente, cstrDBNULL(m_RowPedido("CodEntidad")), False, _
        bRespetaPrecio, sCodAlquiler, PrecioNeto, PorcentajeDescuento, TotalDescuento, _
        RowAlquiler)

        ' Se calculan los totales
        SubTotal = PrecioNeto * DiasMinimos
        Iva = SubTotal * (PorcentajeIva / 100)
        MontoTotal = SubTotal + Iva


        ' S E  G E N E R A  L A  F A C T U R A C I O N   D E L  A L Q U I L E R

        If InsertEncabezadoFacturaRemision(CDec(MontoTotal), CDec(Iva), 0, _
        CDec(TotalDescuento), TipoMovimientos.Factura, _
            RowTalonarioFactura, TiposDocumento.FacturaAutomatica) Then

            InsertEncabezadoGuia(TipoMovimientos.Factura, RowTalonarioFactura)

            InsertDetalleFactura(sCodAlquiler, Servicio.Capacidad, Pertenencia.Praxair, _
            "cu", 0, CDec(TotalDescuento), CDec(Iva), CDec(MontoTotal), _
            RowTalonarioFactura, TiposDocumento.FacturaAutomatica, _
            TipoMovimientos.Factura, DiasMinimos, CDec(PrecioNeto), RowAlquiler.Descripcion)

            InsertDetalleGuiaFacturasRemisiones(sCodAlquiler, "", Pertenencia.Praxair, _
            RowTalonarioFactura, TipoMovimientos.Factura, "", "")
        Else
            Return False
            Exit Function
        End If

        ' S E  G U A R D A N  L O S  A L Q U I L E R E S  P A G A D O S
        dsPacientes.AlquileresPagados.AddAlquileresPagadosRow(ConsecutivoAsignacion, TipoDocumentos.Factura, _
        cstrDBNULL(RowTalonarioFactura("Actual")), Today, Today.AddDays(DiasMinimos), CShort(DiasMinimos))

        Return True
    End Function

    Public Function GuardarAlquilerCredito(ByVal bRespetaPrecio As Boolean, ByVal sCodAlquiler As String, ByVal sCodTipoAsignacion As String, _
    ByRef RowTalonarioRemision As DataRow, ByVal ConsecutivoAsignacion As String, ByVal PorcentajeIva As Double, _
    ByRef RowTalonarioFactura As DataRow, ByVal RowAlquileres As DataRow, ByVal CodProducto As String) As Boolean

        Dim PrecioNeto As Double
        Dim PorcentajeDescuento As Double = 0
        Dim TotalDescuento As Double = 0
        Dim DiasMinimos As Short
        Dim SubTotal As Double = 0
        Dim Iva As Double = 0
        Dim MontoTotal As Double = 0
        Dim MontoCopago As Double = 0
        Dim FechaInicial As DateTime
        Dim FechaFinal As DateTime
        GeneraRemision = True


        ' S E  O B T I E N E   E L  P R E C I O  D E L   A L Q U I L E R
        Venta.ObtenerPrecio(m_RowCliente, cstrDBNULL(m_RowPedido("CodEntidad")), True, _
        bRespetaPrecio, sCodAlquiler, PrecioNeto, PorcentajeDescuento, TotalDescuento, _
        CType(Productos.dsProductos.Productos.Rows(0), ProductosDataSet.ProductosRow))

        FechaInicial = Date.ParseExact(Format(RowAlquileres("FechaInicial"), "dd/MM/yyyy"), "dd/MM/yyyy", Nothing)
        FechaFinal = Date.ParseExact(Format(RowAlquileres("FechaFinal"), "dd/MM/yyyy"), "dd/MM/yyyy", Nothing)

        DiasMinimos = CShort(DateDiff(DateInterval.Day, FechaInicial, FechaFinal))

        SubTotal = PrecioNeto * DiasMinimos
        Iva = SubTotal * (PorcentajeIva / 100)
        MontoTotal = SubTotal + Iva


        ' S E  G U A R D A   E L  A L Q U I L E R  E N  E L  D E T A L L E  D E  L A  R E M I S I O N

        If InsertEncabezadoFacturaRemision(CDec(MontoTotal), CDec(Iva), _
        0, CDec(TotalDescuento), TipoMovimientos.Remision, _
            RowTalonarioRemision, TiposDocumento.RemitoAutomatico) Then

            InsertEncabezadoGuia(TipoMovimientos.Remision, RowTalonarioRemision)

            InsertDetalleFactura(sCodAlquiler, Servicio.Capacidad, Pertenencia.Praxair, _
            "cu", 0, CDec(TotalDescuento), CDec(Iva), _
            CDec(MontoTotal), RowTalonarioRemision, TiposDocumento.RemitoAutomatico, _
            TipoMovimientos.Remision, DiasMinimos, CDec(PrecioNeto), Productos.NombreProducto(sCodAlquiler))

            InsertDetalleGuiaFacturasRemisiones(sCodAlquiler, "", Pertenencia.Praxair, _
            RowTalonarioRemision, TipoMovimientos.Remision, "", "")

            GeneraRemision = True
        Else
            Return False
            Exit Function
        End If

        ' S E  I N S E R T  A  E N   L A  T A B L A   D E   A U T O R I Z A C I O N  R E M I S I O N
        Dim Row As PacientesDataSet.AutorizacionRemisionRow
        Row = dsPacientes.AutorizacionRemision.FindByIdDetalleAutorizacionNoRemisionCodProductoCapacidad(cstrDBNULL(RowAlquileres("IdDetalleAutorizacion")), cstrDBNULL(RowTalonarioRemision("Actual")), _
        sCodAlquiler, "")
        If Row IsNot Nothing Then
            Row.Unidades += DiasMinimos
        Else
            dsPacientes.AutorizacionRemision.AddAutorizacionRemisionRow(cstrDBNULL(RowAlquileres("IdDetalleAutorizacion")), cstrDBNULL(RowTalonarioRemision("Actual")), _
            sCodAlquiler, "", DiasMinimos, "1")
        End If

        MontoCopago = CDblDBNull(RowAlquileres("MontoBase"))

        If MontoCopago > 0 Then
            ' Se verifica si el copago esta autorizado
            Dim RowAutCopago() As DataRow
            RowAutCopago = Pacientes.dsPacientes.DetalleAutorizaciones.Select("CodProducto =" & Nucleo.ProductoCopago)
            If Not RowAutCopago Is Nothing Then

                'No esta autorizado el copago se graba de contado

                If RowAutCopago.Length = 0 Then
                    If InsertEncabezadoFacturaRemision(CDec(MontoCopago), 0, 0, 0, TipoMovimientos.Factura, _
                        RowTalonarioFactura, TiposDocumento.FacturaAutomatica) Then

                        InsertEncabezadoGuia(TipoMovimientos.Factura, RowTalonarioFactura)

                        InsertDetalleFactura(Nucleo.ProductoCopago, "", Pertenencia.Praxair, "cu", 0, _
                        0, 0, CDec(MontoCopago), RowTalonarioFactura, TiposDocumento.FacturaAutomatica, _
                        TipoMovimientos.Factura, 1, CDec(MontoCopago), Productos.NombreProducto(Nucleo.ProductoCopago))

                        InsertDetalleGuiaFacturasRemisiones(Nucleo.ProductoCopago, "", Pertenencia.Praxair, _
                        RowTalonarioFactura, TipoMovimientos.Factura, "", "")

                        GeneraFactura = True
                    Else
                        Return False
                        Exit Function
                    End If

                    ' Se guarda el copago en la tabla de movimientos
                    Dim RowCopagos As PacientesDataSet.MovimientoCopagosCuotasRow
                    RowCopagos = Pacientes.dsPacientes.MovimientoCopagosCuotas.FindByTipoTipoDocumentoNoDocumento(Autorizaciones.Copago, _
                    TipoMovimientos.CopagoRemision, cstrDBNULL(RowAlquileres("IdDetalleAutorizacion")))
                    If RowCopagos IsNot Nothing Then
                        RowCopagos.Monto += Math.Round(CDec(MontoCopago))
                    Else
                        Pacientes.dsPacientes.MovimientoCopagosCuotas.AddMovimientoCopagosCuotasRow(Autorizaciones.Copago, TipoMovimientos.CopagoAsignacion, _
                        cstrDBNULL(RowAlquileres("IdDetalleAutorizacion")), Math.Round(CDec(MontoCopago)), TipoDocumentos.Remision, cstrDBNULL(RowTalonarioFactura("Actual")), "0")
                    End If

                Else
                    If InsertEncabezadoFacturaRemision(CDec(MontoCopago), 0, 0, 0, TipoMovimientos.Remision, _
                    RowTalonarioRemision, TiposDocumento.RemitoAutomatico) Then

                        InsertEncabezadoGuia(TipoMovimientos.Remision, RowTalonarioRemision)

                        InsertDetalleFactura(Nucleo.ProductoCopago, "", Pertenencia.Praxair, "cu", 0, _
                        0, 0, CDec(MontoCopago), RowTalonarioRemision, TiposDocumento.RemitoAutomatico, _
                        TipoMovimientos.Remision, 1, CDec(MontoCopago), Productos.NombreProducto(Nucleo.ProductoCopago))

                        InsertDetalleGuiaFacturasRemisiones(Nucleo.ProductoCopago, "", Pertenencia.Praxair, _
                        RowTalonarioRemision, TipoMovimientos.Remision, "", "")
                    Else
                        Return False
                        Exit Function
                    End If
                End If
            End If
        End If

        ' D E P O S I T O S
        If bPagoDeposito Then
            GuardarDepositoCredito(sCodTipoAsignacion, ConsecutivoAsignacion, _
            CodProducto, cstrDBNULL(dsPacientes.TipoAsignaciones.Rows(0)("Descripcion")), _
            "1", "1")
        End If

        Return True
    End Function


    Public Sub GuardarDepositoCredito(ByVal sCodTipoAsignacion As String, ByVal ConsecutivoAsignacion As String, _
    ByVal Producto As String, ByVal Descripcion As String, ByVal Prefijo As String, ByVal NoDocumento As String)
        Dim MontoDeposito As Decimal

        GeneraDeposito = True

        ' Se busca en la tabla de EntidadCliente si existe el monto
        Pacientes.DepositosEntidad(cstrDBNULL(m_RowPedido("CodEntidad")), sCodTipoAsignacion)
        If dsPacientes.DepositosEntidad.Rows.Count > 0 Then
            MontoDeposito = CDec(dsPacientes.DepositosEntidad.Rows(0)("Monto"))
        Else
            Pacientes.GetDepositosTipoAsignacion(sCodTipoAsignacion)
            If dsPacientes.TipoAsignaciones.Rows.Count > 0 Then
                MontoDeposito = dsPacientes.TipoAsignaciones(0).MontoDeposito
            Else
                MsgBox("No se encontró el tipo de asignación no se puede grabar el Deposito para el alquiler!!")
                Exit Sub
            End If
        End If

        ' Se Inserta en la tabla de depositos
        dsPacientes.DepositosGarantia.AddDepositosGarantiaRow(Nucleo.ConsecutivoDepositos, cstrDBNULL(m_RowCliente("Codigo")), _
        cstrDBNULL(m_RowPedido("CodEntidad")), MontoDeposito, ConsecutivoAsignacion, "1", " ", sCodTipoAsignacion, "0", "0", _
        Producto, Descripcion, Prefijo, NoDocumento)

        Nucleo.ConsecutivoDepositos = CStr(CInt(Nucleo.ConsecutivoDepositos) + 1)
    End Sub

    Public Sub GuardarDepositoContado(ByVal sCodTipoAsignacion As String, ByVal ConsecutivoAsignacion As String, _
    ByVal Producto As String, ByVal Descripcion As String, ByVal Monto As Double, ByVal Prefijo As String, _
    ByVal NoDocumento As String)
        GeneraDeposito = True
        dsPacientes.DepositosGarantia.AddDepositosGarantiaRow(Nucleo.ConsecutivoDepositos, cstrDBNULL(m_RowCliente("Codigo")), _
        cstrDBNULL(m_RowPedido("CodEntidad")), CDec(Monto), ConsecutivoAsignacion, "1", " ", sCodTipoAsignacion, "0", "0", _
        Producto, Descripcion, Prefijo, NoDocumento)

        Nucleo.ConsecutivoDepositos = CStr(CInt(Nucleo.ConsecutivoDepositos) + 1)
    End Sub

    Public Sub MostrarDetalleRemision()
        ' Se muestra el detalle de la remisión en pantalla
        pnDetalleRemision.Visible = True
        pnResumenFactura.Visible = False
        pnTipoDocs.Visible = False
        pnTipoDocumento.Visible = False
        pnDetalleRemision.BringToFront()
        If bRemisionValorizada Then
            bsDetalleRemision.DataSource = Me.dsVenta.DetalleFactura
            bsRemision.DataSource = Me.dsVenta.MaestroFacturas
            bsDetalleRemision.Position = 0
            bsRemision.Position = 0
            Me.lblDescripcionRemision.Text = Productos.NombreProducto(Me.lblProductoRemision.Text)
            Me.lblPosicionRemision.Text = "1 de " & CStr(bsDetalleRemision.Count)
            Me.HScrollBar2.Minimum = 1
            Me.HScrollBar2.Maximum = bsDetalleRemision.Count
        End If
    End Sub

    Public Sub MostrarDetalleFactura()
        pnTipoDocs.Visible = False
        pnDetalleRemision.Visible = False
        pnTipoDocumento.Visible = False
        pnResumenFactura.Visible = True
        pnResumenFactura.BringToFront()
        bsDetalleFactura.DataSource = Me.dsVenta.DetalleFactura
        bsFactura.DataSource = Me.dsVenta.MaestroFacturas
        bsDetalleFactura.Position = 0
        bsFactura.Position = 0
        Me.lblDescripcion.Text = Productos.NombreProducto(Me.lblProducto.Text)
        Me.lblPosicion.Text = "1 de " & CStr(bsDetalleFactura.Count)
        Me.HScrollBar1.Minimum = 1
        Me.HScrollBar1.Maximum = bsDetalleFactura.Count
    End Sub

    Public Function AnularFacturaRemision(ByVal TipoDocumento As Short, _
    ByVal Movimiento As String, ByRef m_RowTalonario As DataRow, _
    ByRef RowFactura As DataRow) As Boolean
        Dim NoFactura As String = ""
        Dim NoMovimientoActual As String = ""

        NoMovimientoActual = Nucleo.NumeroMovimiento
        NoFactura = cstrDBNULL(m_RowTalonario("Actual"))

        ' Se incrementa el numero del movimiento
        Nucleo.NumeroMovimiento = CStr(CInt(Nucleo.NumeroMovimiento) + 1)
        NoFactura = cstrDBNULL(RowFactura("NoFactura"))

        If Not ObtenerNoFactura(m_RowTalonario, TipoDocumento) Then
            Return False
            Exit Function
        End If

        ' S E  A N U L A  E L  E N C A B E Z A D O   D E  L A  F A C T U R A 


        RowFactura("EstadoFactura") = EstadoFactura.Anulado

        InsertEncabezadoFacturaRemision(CDec(RowFactura("MontoFactura")), CDec(RowFactura("ImpuestoTotal")), _
        CDec(RowFactura("MontoFlete")), CDec(RowFactura("Descuento")), Movimiento, m_RowTalonario, _
        TipoDocumento)

        Venta.UpdateMaestroFacturas()

        ' Se anula el maestro de guias
        Dim Row() As DataRow
        Row = dsVenta.MaestroGuias.Select("NoFactura = '" & NoFactura & "'")
        If Row IsNot Nothing Then
            If Row.Length > 0 Then
                'If CStr(Row(0)("NoMovimiento")) = NoMovimientoActual Then
                Row(0)("NoMovimiento") = Nucleo.NumeroMovimiento
                Row(0)("TipoDocumento") = "G"

                InsertEncabezadoGuia(Movimiento, m_RowTalonario)
                Venta.UpdateMaestroGuias()
                'End If
            End If
        End If

        ' Se actualiza el tableAdapter del Detalle de la factura
        For Each RowDetalle As VentaDataSet.DetalleFacturaRow In dsVenta.DetalleFactura.Rows
            If RowDetalle.TipoFactura = Movimiento And RowDetalle.NoFactura = NoFactura Then
                RowDetalle.NoFactura = cstrDBNULL(m_RowTalonario("Actual"))
                RowDetalle.Prefijo = cstrDBNULL(m_RowTalonario("Prefijo"))
            End If
        Next
        Venta.UpdateDetalleFactura()

        ' Se actualiza el tableAdapter del detalle de la guia de facturas/remisiones
        For Each RowGuia As VentaDataSet.DetalleGuiaFacturasRemisionesRow In dsVenta.DetalleGuiaFacturasRemisiones.Rows
            If RowGuia.NoGuia = NoFactura Then
                RowGuia.NoMovimiento = Nucleo.NumeroMovimiento
                RowGuia.NoGuia = cstrDBNULL(m_RowTalonario("Actual"))
                RowGuia.Prefijo = cstrDBNULL(m_RowTalonario("Prefijo"))
            End If
        Next
        Venta.UpdateDetalleGuiaFacturasRemisiones()

        ' Se actualiza el table adapter del detalle de asignaciones/recolecciones
        For Each RowGuiaAsignaciones As VentaDataSet.DetalleGuiaAsignacionesRecoleccionesRow In dsVenta.DetalleGuiaAsignacionesRecolecciones.Rows
            RowGuiaAsignaciones.NoMovimiento = Nucleo.NumeroMovimiento
        Next

        ' Se actualiza el table adapter de la tabla AutorizacionRemision
        For Each RowAutorizacionRemision As PacientesDataSet.AutorizacionRemisionRow In dsPacientes.AutorizacionRemision.Rows
            If RowAutorizacionRemision.NoRemision = NoFactura Then
                RowAutorizacionRemision.NoRemision = cstrDBNULL(m_RowTalonario("Actual"))
            End If
        Next
        Return True
    End Function

    Public Function AnularAsignacionRecolecciones(ByVal TipoDocumento As Short, _
    ByVal Movimiento As String, ByRef m_RowTalonario As DataRow) As Boolean
        Dim NoGuia As String = ""
        Dim NoMovimientoActual As String = ""

        NoMovimientoActual = Nucleo.NumeroMovimiento
        NoGuia = cstrDBNULL(m_RowTalonario("Actual"))

        Nucleo.NumeroMovimiento = CStr(CInt(Nucleo.NumeroMovimiento) + 1)

        If Not ObtenerNoFactura(m_RowTalonario, TipoDocumento) Then
            Return False
            Exit Function
        End If

        ' Se anula el maestro de guias
        Dim Row() As DataRow
        Row = dsVenta.MaestroGuias.Select("NoFactura = '" & NoGuia & "'")
        If Row IsNot Nothing Then
            If Row.Length > 0 Then
                Row(0)("TipoDocumento") = "G"
                ' Se inserta la nueva con los mismos datos
                InsertEncabezadoGuia(Movimiento, m_RowTalonario)
                Venta.UpdateMaestroGuias()
            End If
        End If

        ' Se actualiza el detalle de guia de facturas / remisiones
        For Each RowDetalle As VentaDataSet.DetalleGuiaFacturasRemisionesRow In dsVenta.DetalleGuiaFacturasRemisiones.Rows
            If RowDetalle.NoMovimiento = NoMovimientoActual Then
                RowDetalle.NoMovimiento = Nucleo.NumeroMovimiento
            End If
        Next

        ' Se actualiza el tableAdapter del detalle de la guia de Asignaciones/Recolecciones
        For Each RowGuia As VentaDataSet.DetalleGuiaAsignacionesRecoleccionesRow In dsVenta.DetalleGuiaAsignacionesRecolecciones.Rows
            If RowGuia.TipoGuia = Movimiento And RowGuia.NoGuia = NoGuia Then
                RowGuia.NoMovimiento = Nucleo.NumeroMovimiento
                RowGuia.NoGuia = cstrDBNULL(m_RowTalonario("Actual"))
                RowGuia.Prefijo = cstrDBNULL(m_RowTalonario("Prefijo"))
            End If
        Next

        ' Se actualiza la tabla de asignaciones
        For Each RowAsignacion As PacientesDataSet.AsignacionesRow In dsPacientes.Asignaciones.Rows
            If RowAsignacion.NoDocAsignacion = NoGuia Then
                RowAsignacion.NoDocAsignacion = cstrDBNULL(m_RowTalonario("Actual"))
            End If
        Next

        If Movimiento = TipoMovimientos.Recoleccion Then
            ' Si se anula recoleccion se debe cambiar el No de recoleccion en las asignaciones
            For Each RowAsignacion As PacientesDataSet.AsignacionesRow In dsPacientes.Asignaciones.Rows
                If RowAsignacion.NoRecoleccion = NoGuia Then
                    RowAsignacion.NoRecoleccion = cstrDBNULL(m_RowTalonario("Actual"))
                End If
            Next

            ' Se actualiza el tableadapter del detalle de recoleccion de ajenos si aplica
            For Each RowRecolecciones As VentaDataSet.DetalleGuiaRecoleccionesAjenosRow In dsVenta.DetalleGuiaRecoleccionesAjenos.Rows
                If RowRecolecciones.NoGuia = NoGuia Then
                    RowRecolecciones.NoMovimiento = Nucleo.NumeroMovimiento
                    RowRecolecciones.NoGuia = cstrDBNULL(m_RowTalonario("Actual"))
                    RowRecolecciones.Prefijo = cstrDBNULL(m_RowTalonario("Prefijo"))
                End If
            Next
        End If

        Return True
    End Function

    Public Function AnularDepositos(ByVal TipoDocumento As Short, _
   ByVal Movimiento As String, ByRef m_RowTalonario As DataRow, _
   ByRef Prefijo As String, ByRef Documento As String, ByVal CodProducto As String) As Boolean

        Dim NoMovimientoActual As String = ""

        Dim Row() As DataRow
        Row = dsPacientes.DepositosGarantia.Select("Estado = ''" & "And CodProducto = '" & CodProducto & "'")
        If Row IsNot Nothing Then
            If Row.Length > 0 Then
                Row(0)("IndAnulacion") = "1"
                Row(0)("Estado") = "0"
                Row(0)("NoPrefijo") = Prefijo
                Row(0)("NoDocumento") = Documento

                If Not ObtenerNoFactura(m_RowTalonario, TipoDocumento) Then
                    Return False
                    Exit Function
                End If

                ' Se inserta la nueva con los mismos datos
                dsPacientes.DepositosGarantia.AddDepositosGarantiaRow(Nucleo.ConsecutivoDepositos, cstrDBNULL(m_RowCliente("Codigo")), _
                cstrDBNULL(m_RowPedido("CodEntidad")), CDec(Row(0)("Monto")), cstrDBNULL(Row(0)("NoAsignacion")), "1", " ", cstrDBNULL(Row(0)("TipoAsignacion")), _
                cstrDBNULL(Row(0)("IndAsignacion")), "0", cstrDBNULL(Row(0)("CodProducto")), cstrDBNULL(Row(0)("Descripcion")), _
                cstrDBNULL(m_RowTalonario("Prefijo")), cstrDBNULL(m_RowTalonario("Actual")))

                Nucleo.ConsecutivoDepositos = CStr(CInt(Nucleo.ConsecutivoDepositos) + 1)
                Pacientes.UpdateDepositosGarantia()

                Prefijo = CStr(cstrDBNULL(m_RowTalonario("Prefijo")))
                Documento = cstrDBNULL(m_RowTalonario("Actual"))
            End If
        End If
        Return True
    End Function

#End Region

#Region "Eventos Panel Resumen Factura"

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        ' Se valida que el monto de la factura no sobrepase el limite de
        'credito para industrial
        If cstrDBNULL(m_RowCliente("TipoCliente")) = TiposCliente.Industrial Then
            If CDblDBNull(m_RowCliente("CreditoDisponible"), 0) < CDbl(lblTotalFactura.Text) Then
                UIHandler.ShowError("Venta excede el límite de crédito!!")
                Exit Sub
            End If
        End If

        If GeneraRemision Then
            MostrarDetalleRemision()
        Else
            ImprimirDocumentos()
        End If
    End Sub

    Private Sub HScrollBar1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScrollBar1.ValueChanged
        bsDetalleFactura.Position = Me.HScrollBar1.Value - 1
        lblDescripcion.Text = Productos.NombreProducto(lblProducto.Text)
        Me.lblPosicion.Text = CStr(Me.HScrollBar1.Value) & " de " & CStr(bsDetalleFactura.Count)
    End Sub

    Private Sub btnDetalleRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetalleRegresar.Click
        pnResumenFactura.Visible = False
        pnTipoDocumento.Visible = False
        pnDetalleRemision.Visible = False
        pnTipoDocs.Visible = True
        pnTipoDocs.BringToFront()
    End Sub

#End Region

#Region "Eventos Panel Resumen Remision"

    Private Sub HScrollBar2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScrollBar2.ValueChanged
        bsDetalleRemision.Position = Me.HScrollBar2.Value - 1
        lblDescripcionRemision.Text = Productos.NombreProducto(lblProductoRemision.Text)
        Me.lblPosicionRemision.Text = CStr(Me.HScrollBar2.Value) & " de " & CStr(bsDetalleRemision.Count)
    End Sub

    Private Sub btnGrabarRemision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabarRemision.Click
        If cstrDBNULL(m_RowCliente("TipoCliente")) = TiposCliente.Industrial Or cstrDBNULL(m_RowCliente("TipoCliente")) = TiposCliente.Entidad Then
            If CDblDBNull(m_RowCliente("CreditoDisponible"), 0) < CDbl(lblTotalFactura.Text) Then
                UIHandler.ShowError("Venta excede el límite de crédito!!")
                Exit Sub
            End If
        End If
        If Not ImprimirDocumentos() Then
            UIHandler.ShowError("No se pudo generar la impresión!!")
        End If
    End Sub

    Private Sub btnRegresarRemision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegresarRemision.Click
        If GeneraFactura Then
            MostrarDetalleFactura()
        Else
            pnDetalleRemision.Visible = False
            pnResumenFactura.Visible = False
            pnTipoDocumento.Visible = False
            pnTipoDocs.Visible = True
            pnTipoDocs.BringToFront()
        End If
    End Sub

#End Region

    Private Sub GeneracionForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not Nucleo.ProcessHotKeys(Me, e) Then
            If e.KeyCode = System.Windows.Forms.Keys.Escape Then
                btnRegresar_Click(Me, Nothing)
            ElseIf e.KeyCode = System.Windows.Forms.Keys.C Then
                If pnResumenFactura.Visible Then
                    btnGrabar_Click(Me, Nothing)
                Else
                    btnCancelarTipoDoc_Click(Me, Nothing)
                End If
            ElseIf e.KeyCode = System.Windows.Forms.Keys.A Then
                btnAceptar_Click(Me, Nothing)
            ElseIf e.KeyCode = System.Windows.Forms.Keys.G Then
                If pnDetalleRemision.Visible Then
                    btnGrabarRemision_Click(Me, Nothing)
                ElseIf pnResumenFactura.Visible Then
                    btnGrabar_Click(Me, Nothing)
                End If
            ElseIf e.KeyCode = System.Windows.Forms.Keys.S Then
                btnContinuar_Click(Me, Nothing)
            End If
        End If
    End Sub

    Private Sub GeneracionForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.UiHandler1.Parent = Me
        Me.dsVenta = Venta.dsVenta
        Me.dsProductos = Productos.dsProductos
        Me.dsPacientes = Pacientes.dsPacientes

        ' Se ocultan los paneles
        pnResumenFactura.Visible = False
        pnDetalleRemision.Visible = False
        pnTipoDocumento.Visible = False
        pnTipoDocs.Visible = True
        pnTipoDocs.BringToFront()

        ' Se cargan en memoria la carga y el kardex de camión

        If Not (Me.dsProductos.Carga.Rows.Count > 0 And Me.dsProductos.KardexCamion.Rows.Count > 0) Then
            Productos.LoadCarga()
            Productos.LoadKardexCamion()
        End If

        ckFactura.Checked = False
        ckRecoleccion.Checked = False
        ckAsignacion.Checked = False
        ckDeposito.Checked = False
        ckRemision.Checked = False

        ValidarDocumentosGenerar()
        UIHandler.EndWait()
    End Sub

    Public Shared Sub Run(ByVal rowCliente As System.Data.DataRow, ByRef rowPedido As System.Data.DataRow)
        UIHandler.StartWait()
        Dim form As New GeneracionForm
        form.m_DetallePedido = Pedidos.dsPedidos.DetallePedido
        form.m_CilindrosLeidos = Venta.dsVenta.CilindrosLeidos
        form.m_RowCliente = rowCliente
        form.m_RowPedido = rowPedido
        UIHandler.ShowDialog(form)
        form.Dispose()
        UIHandler.EndWait()
    End Sub

    Public Function ImprimirDocumentos() As Boolean
        Dim Documento As PrinterDocument
        Dim FechaVencimiento As Date
        Dim sTipoPago As String = ""
        Dim Cantidad As Decimal
        Dim SubTotal As Decimal
        Dim ImpresionOk As Boolean = False
        Dim Descripcion As String = ""
        Dim Seriales As String = ""
        Dim NombreEntidad As String = ""

        Try
            Venta.OpenConnection()
            If MsgBox("Se imprimirá y grabará la atención, esta seguro?", MsgBoxStyle.YesNo, "Confirmación") = MsgBoxResult.Yes Then

                If GeneraFactura Then
                    If CStr(m_RowCliente("TipoPago")) = TipoPago.Credito Then
                        sTipoPago = "CREDITO"
                        FechaVencimiento = Today.AddDays(CDbl(m_RowCliente("DiasCredito")))
                    Else
                        sTipoPago = "CONTADO"
                        FechaVencimiento = Today()
                    End If

                    ImpresionOk = False
                    While Not ImpresionOk
                        Documento = New PrinterDocument

                        'I N F O R M A C I O N  D E  L A  F A C T U R A
                        m_RowFactura = dsVenta.MaestroFacturas.Select("TipoFactura = '" & TipoMovimientos.Factura & "'" & "AND EstadoFactura = ''")
                        If m_RowFactura IsNot Nothing Then
                            If m_RowFactura.Length > 0 Then
                                Documento.SetInfoDocumento(cstrDBNULL(m_RowFactura(0)("Prefijo")) & "-" & _
                                cstrDBNULL(m_RowFactura(0)("NoFactura")), cstrDBNULL(m_RowPedido("NoPedido")), _
                                "FACTURA CAMBIARIA DE COMPRAVENTA", TipoDocumentos.Factura, sTipoPago, Today(), _
                                FechaVencimiento, Me.ObtenerInformacionSucursal(), "")

                                'I N F O R M A C I O N  T O T A L E S  D E  L A  F A C T U R A
                                SubTotal = CDec(m_RowFactura(0)("MontoFactura")) - CDec(m_RowFactura(0)("ImpuestoTotal"))
                                Documento.SetInfoTotales(SubTotal, Math.Round(CDec(m_RowFactura(0)("ImpuestoTotal"))))
                            Else
                                Return False
                                Exit Function
                            End If
                        Else
                            Return False
                            Exit Function
                        End If

                        'I N F O R M A C I O N  D E L  C L I E N T E

                        'If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                        '    If Pacientes.dsPacientes.Entidades.Rows.Count > 0 Then
                        '        NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad")) & "-" & cstrDBNULL(Pacientes.dsPacientes.Entidades.Rows(0)("Nombre"))
                        '    Else
                        '        NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad"))
                        '    End If
                        'Else
                        '    NombreEntidad = ""
                        'End If

                        NombreEntidad = ""

                        Documento.SetInfoCliente(cstrDBNULL(m_RowCliente("Codigo")), cstrDBNULL(m_RowCliente("Nit")), _
                        cstrDBNULL(m_RowCliente("Nombre")), cstrDBNULL(m_RowCliente("Direccion")), cstrDBNULL(m_RowCliente("Barrio")), _
                        "", NombreEntidad)

                        ' D E T A L L E  D E   L A  F A C T U R A

                        For Each Row As VentaDataSet.DetalleFacturaRow In dsVenta.DetalleFactura.Rows
                            Seriales = ""
                            Descripcion = ""

                            If Row.TipoFactura = TipoMovimientos.Factura And Row.NoFactura = cstrDBNULL(m_RowFactura(0)("NoFactura")) Then
                                If Row.UnidadMedidaVenta.ToLower = "cu" Then
                                    Cantidad = Row.Cantidad
                                Else
                                    If Row.Capacidad <> "" Then
                                        Cantidad = CDec(Row.Cantidad) * CDec(Row.Capacidad)
                                    Else
                                        Cantidad = Row.Cantidad
                                    End If
                                End If

                                Dim RowCilindros() As DataRow
                                RowCilindros = dsVenta.CilindrosLeidos.Select("CodProducto ='" & Row.CodProducto & "' And Capacidad ='" & Row.Capacidad & "' And Secuencial <> '' And CodTipoProducto <> ''")
                                If RowCilindros.Length > 0 Then
                                    For i As Integer = 0 To RowCilindros.Length - 1
                                        If cstrDBNULL(RowCilindros(i)("SecuencialAjeno")) <> "" Then
                                            If Seriales = "" Then
                                                Seriales = Seriales & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                            Else
                                                Seriales = Seriales & "," & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                            End If
                                        End If
                                    Next
                                End If

                                If Row.UnidadMedidaVenta.ToLower = "cu" Then
                                    Descripcion = Row.Descripcion
                                Else
                                    Descripcion = Row.Descripcion & " (" & Row.Cantidad & "X" & CStr(CInt(Row.Capacidad) / 1000) & " " & Row.UnidadMedidaVenta & ")"
                                End If

                                Documento.AddItem(Row.CodProducto, Descripcion, Cantidad, Row.PrecioUnitario, _
                                Row.UnidadMedidaVenta, Row.MontoTotalItem - Row.MontoImpuesto, Seriales)
                            End If
                        Next

                        ' I N F O R M A C I O N  D E  R E S O L U C I O N  F A C T U  R A
                        Documento.SetInfoResolucion(cstrDBNULL(m_RowTalonarioFactura("NumeroResolucion")), _
                        cstrDBNULL(m_RowTalonarioFactura("Prefijo")) & "-" & cstrDBNULL(m_RowTalonarioFactura("ResolucionDesde")), _
                        cstrDBNULL(m_RowTalonarioFactura("Prefijo")) & "-" & cstrDBNULL(m_RowTalonarioFactura("ResolucionHasta")), _
                        CDate(m_RowTalonarioFactura("FechaInicioResolucion")))

                        ImpresionOk = Nucleo.Imprimir(Documento, "TODO:FIRMA")
                        If Not ImpresionOk Then
                            AnularFacturaRemision(TiposDocumento.FacturaAutomatica, TipoMovimientos.Factura, _
                            m_RowTalonarioFactura, CType(m_RowFactura(0), DataRow))
                        End If
                    End While
                End If

                If GeneraRemision Or GeneraEntregaAjeno Then
                    If CStr(m_RowCliente("TipoPago")) = TipoPago.Credito Then
                        sTipoPago = "CREDITO"
                        FechaVencimiento = Today.AddDays(CDbl(m_RowCliente("DiasCredito")))
                    Else
                        sTipoPago = "CONTADO"
                        FechaVencimiento = Today()
                    End If

                    If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                        sTipoPago = ""
                    End If

                    ImpresionOk = False
                    While Not ImpresionOk
                        Documento = New PrinterDocument

                        'I N F O R M A C I O N  D E  L A  F A C T U R A
                        m_RowRemision = dsVenta.MaestroFacturas.Select("TipoFactura = '" & TipoMovimientos.Remision & "'" & "AND EstadoFactura = ''")
                        If m_RowRemision IsNot Nothing Then
                            If m_RowRemision.Length > 0 Then
                                Documento.SetInfoDocumento(cstrDBNULL(m_RowRemision(0)("Prefijo")) & "-" & _
                                cstrDBNULL(m_RowRemision(0)("NoFactura")), cstrDBNULL(m_RowPedido("NoPedido")), _
                                "REMISION", TipoDocumentos.Remision, sTipoPago, Today(), FechaVencimiento, Me.ObtenerInformacionSucursal(), "")

                                If bRemisionValorizada Then
                                    'I N F O R M A C I O N  T O T A L E S  D E  L A  F A C T U R A
                                    SubTotal = CDec(m_RowRemision(0)("MontoFactura")) - CDec(m_RowRemision(0)("ImpuestoTotal"))
                                    Documento.SetInfoTotales(SubTotal, Math.Round(CDec(m_RowFactura(0)("ImpuestoTotal"))))
                                End If
                            Else
                                Return False
                                Exit Function
                            End If
                        Else
                            Return False
                            Exit Function
                        End If

                        'I N F O R M A C I O N  D E L  C L I E N T E

                        If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                            If Pacientes.dsPacientes.Entidades.Rows.Count > 0 Then
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad")) & "-" & cstrDBNULL(Pacientes.dsPacientes.Entidades.Rows(0)("Nombre"))
                            Else
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad"))
                            End If
                        Else
                            NombreEntidad = ""
                        End If

                        Documento.SetInfoCliente(cstrDBNULL(m_RowCliente("Codigo")), cstrDBNULL(m_RowCliente("Nit")), _
                        cstrDBNULL(m_RowCliente("Nombre")), cstrDBNULL(m_RowCliente("Direccion")), cstrDBNULL(m_RowCliente("Barrio")), _
                        "", NombreEntidad)

                        ' D E T A L L E  D E   L A  R E M I S I O N

                        For Each Row As VentaDataSet.DetalleFacturaRow In dsVenta.DetalleFactura.Rows
                            If Row.TipoFactura = TipoMovimientos.Remision And Row.NoFactura = cstrDBNULL(m_RowRemision(0)("NoFactura")) Then
                                Seriales = ""
                                Descripcion = ""
                                If Row.UnidadMedidaVenta.ToLower = "cu" Then
                                    Cantidad = Row.Cantidad
                                Else
                                    If Row.Capacidad <> "" Then
                                        Cantidad = CDec(Row.Cantidad) * CDec(Row.Capacidad)
                                    Else
                                        Cantidad = Row.Cantidad
                                    End If
                                End If

                                Dim RowCilindros() As DataRow = Nothing

                                If GeneraEntregaAjeno And GeneraRemision Then
                                    RowCilindros = dsVenta.CilindrosLeidos.Select("Secuencial = '' And SecuencialAjeno <> '' And CodTipoProducto = '5'")
                                    If RowCilindros.Length > 0 Then
                                        For i As Integer = 0 To RowCilindros.Length - 1
                                            If cstrDBNULL(RowCilindros(i)("SecuencialAjeno")) <> "" Then
                                                If Seriales = "" Then
                                                    Seriales = Seriales & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                                Else
                                                    Seriales = Seriales & "," & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                                End If
                                            End If
                                        Next
                                    End If

                                    RowCilindros = dsVenta.CilindrosLeidos.Select("CodProducto ='" & Row.CodProducto & "' And Capacidad ='" & Row.Capacidad & "' And Secuencial <> '' And CodTipoProducto <> ''")
                                    If RowCilindros.Length > 0 Then
                                        For i As Integer = 0 To RowCilindros.Length - 1
                                            If cstrDBNULL(RowCilindros(i)("SecuencialAjeno")) <> "" Then
                                                If Seriales = "" Then
                                                    Seriales = Seriales & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                                Else
                                                    Seriales = Seriales & "," & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                                End If
                                            End If
                                        Next
                                    End If
                                Else
                                    If GeneraRemision Then
                                        RowCilindros = dsVenta.CilindrosLeidos.Select("CodProducto ='" & Row.CodProducto & "' And Capacidad ='" & Row.Capacidad & "' And Secuencial <> '' And CodTipoProducto <> ''")
                                    ElseIf GeneraEntregaAjeno Then
                                        RowCilindros = dsVenta.CilindrosLeidos.Select("Secuencial = '' And SecuencialAjeno <> '' And CodTipoProducto = '5'")
                                    End If
                                    If RowCilindros.Length > 0 Then
                                        For i As Integer = 0 To RowCilindros.Length - 1
                                            If cstrDBNULL(RowCilindros(i)("SecuencialAjeno")) <> "" Then
                                                If Seriales = "" Then
                                                    Seriales = Seriales & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                                Else
                                                    Seriales = Seriales & "," & Productos.GetSerialAjeno(cstrDBNULL(RowCilindros(i)("SecuencialAjeno")))
                                                End If
                                            End If
                                        Next
                                    End If
                                End If

                                If Row.UnidadMedidaVenta.ToLower = "cu" Then
                                    Descripcion = Row.Descripcion
                                Else
                                    Descripcion = Row.Descripcion & " (" & Row.Cantidad & "X" & CStr(CInt(Row.Capacidad) / 1000) & " " & Row.UnidadMedidaVenta & ")"
                                End If

                                If bRemisionValorizada Then
                                    Documento.AddItem(Row.CodProducto, Descripcion, Cantidad, Row.PrecioUnitario, _
                                    Row.UnidadMedidaVenta, Row.MontoTotalItem - Row.MontoImpuesto, Seriales)
                                Else
                                    Documento.AddItem(Row.CodProducto, Descripcion, Cantidad, Row.UnidadMedidaVenta, Seriales)
                                End If
                            End If
                        Next

                        ImpresionOk = Nucleo.Imprimir(Documento, "TODO:FIRMA")
                        If Not ImpresionOk Then
                            AnularFacturaRemision(TiposDocumento.RemitoAutomatico, TipoMovimientos.Remision, _
                            m_RowTalonarioRemision, CType(m_RowRemision(0), DataRow))
                        End If
                    End While
                End If

                If GeneraAsignacion Then
                    Dim RowAsignacion() As DataRow

                    ImpresionOk = False
                    While Not ImpresionOk
                        Documento = New PrinterDocument

                        RowAsignacion = dsVenta.DetalleGuiaAsignacionesRecolecciones.Select("TipoGuia = '" & TipoMovimientos.Asignacion & "'")
                        If RowAsignacion.Length > 0 Then
                            Documento.SetInfoDocumento(cstrDBNULL(RowAsignacion(0)("Prefijo")) & "-" & _
                            cstrDBNULL(RowAsignacion(0)("NoGuia")), cstrDBNULL(m_RowPedido("NoPedido")), _
                            "ASIGNACION", "ASIGNACION", "", Today(), Today(), Me.ObtenerInformacionSucursal(), "")
                        End If

                        If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                            If Pacientes.dsPacientes.Entidades.Rows.Count > 0 Then
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad")) & "-" & cstrDBNULL(Pacientes.dsPacientes.Entidades.Rows(0)("Nombre"))
                            Else
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad"))
                            End If
                        Else
                            NombreEntidad = ""
                        End If

                        Documento.SetInfoCliente(cstrDBNULL(m_RowCliente("Codigo")), cstrDBNULL(m_RowCliente("Nit")), _
                        cstrDBNULL(m_RowCliente("Nombre")), cstrDBNULL(m_RowCliente("Direccion")), cstrDBNULL(m_RowCliente("Barrio")), _
                        "", NombreEntidad)

                        For Each Row As VentaDataSet.DetalleGuiaAsignacionesRecoleccionesRow In dsVenta.DetalleGuiaAsignacionesRecolecciones.Rows
                            Descripcion = Productos.NombreProducto(Row.CodProducto)
                            If Row.TipoGuia = TipoMovimientos.Asignacion Then
                                Documento.AddItem(Row.CodProducto, Descripcion & " (" & CStr(CInt(Row.Capacidad) / 1000) & " " & Row.UnidadVenta & ")", Row.Cantidad, "", "")
                            End If
                        Next

                        ImpresionOk = Nucleo.Imprimir(Documento, "TODO:FIRMA")
                        If Not ImpresionOk Then
                            AnularAsignacionRecolecciones(TiposDocumento.AsignacionAutomatica, TipoMovimientos.Asignacion, _
                            m_RowTalonarioAsignacion)
                        End If
                    End While
                End If

                If GeneraRecoleccion Then
                    Dim RowAsignacion() As DataRow

                    ImpresionOk = False
                    While Not ImpresionOk
                        Documento = New PrinterDocument

                        RowAsignacion = dsVenta.DetalleGuiaAsignacionesRecolecciones.Select("TipoGuia = '" & TipoGuias.Recojo & "'")
                        If RowAsignacion.Length > 0 Then
                            Documento.SetInfoDocumento(cstrDBNULL(RowAsignacion(0)("Prefijo")) & "-" & _
                            cstrDBNULL(RowAsignacion(0)("NoGuia")), cstrDBNULL(m_RowPedido("NoPedido")), _
                            "RECOLECCION", "RECOLECCION", "", Today(), Today(), Me.ObtenerInformacionSucursal(), "")
                        End If

                        If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                            If Pacientes.dsPacientes.Entidades.Rows.Count > 0 Then
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad")) & "-" & cstrDBNULL(Pacientes.dsPacientes.Entidades.Rows(0)("Nombre"))
                            Else
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad"))
                            End If
                        Else
                            NombreEntidad = ""
                        End If

                        'I N F O R M A C I O N   D E L   C L I E N T E
                        Documento.SetInfoCliente(cstrDBNULL(m_RowCliente("Codigo")), cstrDBNULL(m_RowCliente("Nit")), _
                        cstrDBNULL(m_RowCliente("Nombre")), cstrDBNULL(m_RowCliente("Direccion")), cstrDBNULL(m_RowCliente("Barrio")), _
                        "", NombreEntidad)

                        For Each Row As VentaDataSet.DetalleGuiaAsignacionesRecoleccionesRow In dsVenta.DetalleGuiaAsignacionesRecolecciones.Rows
                            If Row.TipoMovimiento = TipoMovimientos.RecojoVacios Then
                                Descripcion = Productos.NombreProducto(Row.CodProducto)
                                If Row.Pertenencia = Pertenencia.Cliente Then
                                    Seriales = ""
                                    Dim RowCilindros() As DataRow
                                    RowCilindros = dsVenta.CilindrosLeidos.Select("CodProducto ='" & Row.CodProducto & "' And Capacidad ='" & Row.Capacidad & "' And Secuencial = '' And CodTipoProducto = ''")
                                    If RowCilindros.Length > 0 Then
                                        For i As Integer = 0 To RowCilindros.Length - 1
                                            If cstrDBNULL(RowCilindros(i)("SecuencialAjeno")) <> "" Then
                                                If Seriales = "" Then
                                                    Seriales = Seriales & cstrDBNULL(RowCilindros(i)("SecuencialAjeno"))
                                                Else
                                                    Seriales = Seriales & "," & cstrDBNULL(RowCilindros(i)("SecuencialAjeno"))
                                                End If
                                            End If
                                        Next
                                    Else
                                        Seriales = ""
                                    End If
                                Else
                                    Seriales = ""
                                End If
                                Documento.AddItem(Row.CodProducto, Descripcion & " (" & CStr(CInt(Row.Capacidad) / 1000) & " " & Row.UnidadVenta & ")", Row.Cantidad, "", Seriales)
                            End If
                        Next

                        ImpresionOk = Nucleo.Imprimir(Documento, "TODO:FIRMA")
                        If Not ImpresionOk Then
                            ' Se anula el documento y se envia la impresion
                            AnularAsignacionRecolecciones(TiposDocumento.RecoleccionAutomatico, TipoMovimientos.Recoleccion, _
                            m_RowTalonarioRecoleccion)
                        End If
                    End While
                End If


                ' Imprime los depositos
                If dsPacientes.DepositosGarantia.Rows.Count > 0 Then
                    Dim Prefijo As String = ""
                    Dim NoDoc As String = ""
                    Dim Cant, i, idx As Integer

                    m_RowTalonarioDeposito = Nothing
                    Cant = dsPacientes.DepositosGarantia.Rows.Count

                    For i = 0 To Cant - 1
                        ImpresionOk = False
                        While Not ImpresionOk
                            If m_RowTalonarioDeposito Is Nothing Then
                                If Not ObtenerNoFactura(m_RowTalonarioDeposito, TiposDocumento.DepositoAutomatico) Then
                                    UIHandler.EndWait()
                                    Return False
                                    Exit Function
                                End If

                                Prefijo = CStr(m_RowTalonarioDeposito("Prefijo"))
                                NoDoc = CStr(m_RowTalonarioDeposito("Actual"))
                            End If

                            Documento = New PrinterDocument
                            Documento.SetInfoDocumento(Prefijo & "-" & NoDoc, _
                            cstrDBNULL(m_RowPedido("NoPedido")), "DEPOSITOS", "DEPOSITOS", "Contado", Today(), Today(), Me.ObtenerInformacionSucursal(), "")

                            If Pacientes.dsPacientes.Entidades.Rows.Count > 0 Then
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad")) & "-" & cstrDBNULL(Pacientes.dsPacientes.Entidades.Rows(0)("Nombre"))
                            Else
                                NombreEntidad = cstrDBNULL(m_RowPedido("CodEntidad"))
                            End If

                            Documento.SetInfoCliente(cstrDBNULL(m_RowCliente("Codigo")), cstrDBNULL(m_RowCliente("Nit")), _
                            cstrDBNULL(m_RowCliente("Nombre")), cstrDBNULL(m_RowCliente("Direccion")), cstrDBNULL(m_RowCliente("Barrio")), _
                            "", NombreEntidad)

                            Documento.AddItem(CStr(dsPacientes.DepositosGarantia.Rows(i)("CodProducto")), _
                            CStr(dsPacientes.DepositosGarantia.Rows(i)("Descripcion")), 1, _
                            CDec(dsPacientes.DepositosGarantia.Rows(i)("Monto")), "", _
                            CDec(dsPacientes.DepositosGarantia.Rows(i)("Monto")), "")

                            ImpresionOk = Nucleo.Imprimir(Documento, "TODO:FIRMA")
                            If Not ImpresionOk Then
                                AnularDepositos(TiposDocumento.DepositoAutomatico, TipoMovimientos.Deposito, _
                                m_RowTalonarioDeposito, Prefijo, NoDoc, CStr(dsPacientes.DepositosGarantia.Rows(i)("CodProducto")))
                            Else
                                If Not ObtenerNoFactura(m_RowTalonarioDeposito, TiposDocumento.DepositoAutomatico) Then
                                    UIHandler.EndWait()
                                    Return False
                                    Exit Function
                                End If
                                idx = dsPacientes.DepositosGarantia.Rows.Count - 1
                                dsPacientes.DepositosGarantia(idx).Estado = "0"
                                dsPacientes.DepositosGarantia(idx).NoPrefijo = Prefijo
                                dsPacientes.DepositosGarantia(idx).NoDocumento = NoDoc

                                Prefijo = CStr(m_RowTalonarioDeposito("Prefijo"))
                                NoDoc = CStr(m_RowTalonarioDeposito("Actual"))
                            End If
                        End While
                    Next
                End If

                UpdateDataSets()
                UIHandler.StartWait()
                DialogResult = System.Windows.Forms.DialogResult.OK
            End If
            Return True
        Finally
            Venta.CloseConnection()
        End Try
    End Function

    Private Sub UpdateDataSets()
        '' Se actualizan los datasets
        UIHandler.StartWait()
        Try
            Venta.OpenConnection()

            ' Se incrementa el numero del movimiento
            Nucleo.NumeroMovimiento = CStr(CInt(Nucleo.NumeroMovimiento) + 1)

            If GeneraVenta Then

                If m_RowTalonarioFactura IsNot Nothing Then
                    Dim Row As VentaDataSet.MaestroFacturasRow
                    Row = dsVenta.MaestroFacturas.FindByTipoFacturaNoFacturaPrefijo(TipoMovimientos.Factura, cstrDBNULL(m_RowTalonarioFactura("Actual")), cstrDBNULL(m_RowTalonarioFactura("PRefijo")))
                    If Row IsNot Nothing Then
                        Row.EstadoFactura = "E"
                    End If
                End If

                If m_RowTalonarioRemision IsNot Nothing Then
                    Dim Row As VentaDataSet.MaestroFacturasRow
                    Row = dsVenta.MaestroFacturas.FindByTipoFacturaNoFacturaPrefijo(TipoMovimientos.Remision, cstrDBNULL(m_RowTalonarioRemision("Actual")), cstrDBNULL(m_RowTalonarioRemision("PRefijo")))
                    If Row IsNot Nothing Then
                        Row.EstadoFactura = "E"
                    End If
                End If
            End If

            ' Se actualizan las tablas de carga y
            'kardex de camión
            Productos.UpdateCarga()
            Productos.UpdateKardexCamion()

            ' Se actualiza el Estado del Pedido
            m_RowPedido("Estado") = EstadosPedido.Atendido
            m_RowPedido("FechaAtencion") = Today
            m_RowPedido("HoraAtencion") = CStr(Now.Hour) & ":" & CStr(Now.Minute)

            ' Se actualiza el detalle del pedido
            Pedidos.UpdateDetallePedido(m_DetallePedido)

            ' Se actualiza la tabla de asignaciones
            If m_RowTalonarioRecoleccion IsNot Nothing Then
                For Each Row As PacientesDataSet.AsignacionesRow In dsPacientes.Asignaciones.Rows
                    If Row.NoRecoleccion = "Actual" Then
                        Row.NoRecoleccion = cstrDBNULL(m_RowTalonarioRecoleccion("Actual"))
                    End If
                Next
            End If


            Venta.UpdateMaestroFacturas()
            Venta.UpdateDetalleFactura()
            Venta.UpdateDetalleGuiaFacturasRemisiones()
            Venta.UpdateMaestroGuias()
            Venta.UpdateDetalleGuiaAsignacionesRecolecciones()
            Venta.UpdateDetalleGuiaRecoleccionesAjenos()
            Pacientes.UpdateDetalleAutorizaciones()
            Pacientes.UpdateAlquileresPagados()
            Pacientes.UpdateAlquileresPendientes()
            Pacientes.UpdateAutorizacionRemision()
            Pacientes.UpdateAutorizacionAsignaciones()
            Pacientes.UpdateDetalleAutorizaciones()
            Pacientes.UpdateAlquileres()
            Pacientes.UpdateDepositoGarantia()
            Pacientes.UpdateAsignaciones()
            Pacientes.UpdateMovimientoCopagos()
        Catch ex As Exception
            Venta.CloseConnection()
            WriteLog(ex)
        Finally
            UIHandler.StartWait()
        End Try

        UIHandler.StartWait()
    End Sub

    Private Function ObtenerInformacionSucursal() As String
        Return cstrDBNULL(Nucleo.RowParametros("NombreSucursal")) + " TELEFONO: " + cstrDBNULL(Nucleo.RowParametros("TelefonoTransportador")) + " " + cstrDBNULL(Nucleo.RowParametros("NitPraxair"))
    End Function
End Class