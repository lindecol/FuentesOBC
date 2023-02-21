Imports MovilidadCF.Windows.Forms

Public Class VentaForm
    Implements IModuloPedido

    Private m_rowCliente As Data.DataRow
    Private m_rowPedido As Data.DataRow
    Private m_Venta As GestorVenta
    Private FilaSeleccionada As DataRow
    Private rowTanque As VentaDataSet.TanquesClienteRow
    Private m_rowDetalle As VentaDataSet.DetallePedidoRow

    Public Sub Run(ByVal rowCliente As System.Data.DataRow, ByVal rowPedido As System.Data.DataRow) Implements Common.IModuloPedido.Run
        UIHandler.StartWait()
        Me.m_rowPedido = rowPedido
        Me.m_rowCliente = rowCliente
        UIHandler.ShowDialog(Me)
        Me.Dispose()
        UIHandler.EndWait()
    End Sub

    Private Sub VentaForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not Nucleo.ProcessHotKeys(Me, e) Then
            If e.KeyCode = Keys.Escape Then
                menuCancelar_Click(Me, Nothing)
            ElseIf e.KeyCode = Keys.N Then
                menuNuevaEntrega_Click(Me, Nothing)
            End If
        End If
    End Sub

    Private Sub VentaForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        cargarControlesVenta()
        LoadData()
        UIHandler.EndWait()
    End Sub

    Private Sub menuTerminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuTerminar.Click
        If dsVenta.DetallePedido.Count > 0 Then
            If GeneracionForm.Run(m_rowCliente, m_rowPedido) Then
                UIHandler.StartWait()
                m_rowPedido("Estado") = EstadosPedido.Atendido
                m_rowPedido("FechaAtencion") = Today()
                m_rowPedido("HoraAtencion") = Now.ToString("HH:mm")
                DialogResult = System.Windows.Forms.DialogResult.OK
            End If
        Else
            UIHandler.StartWait()
            DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub menuNuevaEntrega_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNuevaEntrega.Click
        If dsVenta.DetallePedido.Count = 0 Then
            pnlVenta.Visible = True
            pnlVenta.BringToFront()
            Me.Menu = New MainMenu()
            cbTanque.Focus()
        Else
            UIHandler.ShowError("Ya se generó una venta para este pedido")
        End If
    End Sub

    Private Sub LoadData()
        Venta.dsVenta = Me.dsVenta
        Venta.LoadTanquesCliente(m_rowCliente)
        Venta.KilometrajeFinal = Nucleo.KiloMetrajeFinal
    End Sub

    Private Sub cargarControlesVenta()
        Me.pnlVenta.Controls.Clear()
        Me.pnlVenta.Controls.Add(Me.Label4)
        Me.pnlVenta.Controls.Add(Me.cbTanque)
        Me.pnlVenta.Controls.Add(Me.btnCancelar)
        Me.pnlVenta.Controls.Add(Me.btnContinuar)

        rbtnPorPesoTanque.Checked = False
        rbtnPorCaudalimetro.Checked = False
        rbtnPorPesoCamion.Checked = False
        rbtnNiveles.Checked = False
        If CStrDBNull(Nucleo.RowParametros("CodigoEmpresa")) = "21" Then
            rbtnNiveles.Checked = True
            Me.pnlVenta.Controls.Add(Me.rbtnPorCaudalimetro)
            Me.pnlVenta.Controls.Add(Me.rbtnPorPesoTanque)
            Me.pnlVenta.Controls.Add(Me.rbtnPorPesoCamion)
            Me.pnlVenta.Controls.Add(Me.rbtnNiveles)
        ElseIf CStrDBNull(Nucleo.RowParametros("CodigoEmpresa")) = "22" Then
            rbtnPorPesoTanque.Checked = True
            Me.pnlVenta.Controls.Add(Me.rbtnPorPesoTanque)
        End If

        Me.pnlVenta.Controls.Add(Me.Label3)
    End Sub

    Private Sub btnContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        If cbTanque.SelectedIndex >= 0 Then

            rowTanque = CType(GetCurrentRow(bsTanquesCliente), VentaDataSet.TanquesClienteRow)

            ' Se debe despachar utilizando el producto del tanque
            Productos.LoadProducto(rowTanque.ProductoTanque)

            pnlVenta.Visible = False
            Me.Menu = mainMenu1
            If rbtnNiveles.Checked Then
                VentaPorNivelesForm.Run(m_rowCliente, m_rowPedido, rowTanque)
            ElseIf rbtnPorCaudalimetro.Checked Then
                VentaPorCaudalimetroForm.Run(m_rowCliente, m_rowPedido, rowTanque)
            ElseIf rbtnPorPesoCamion.Checked Then
                VentaPorPesadaCamionForm.Run(m_rowCliente, m_rowPedido, rowTanque)
            ElseIf rbtnPorPesoTanque.Checked Then
                VentaPorPesadaTanqueForm.Run(m_rowCliente, m_rowPedido, rowTanque)
            End If
            If DialogResult <> System.Windows.Forms.DialogResult.Cancel Then
                CobroFlete()
            End If
        Else
            UIHandler.ShowError("Debe seleccionar un tanque")
        End If
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        'pnlVenta.Visible = False
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub menuCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCancelar.Click
        If dsVenta.DetallePedido.Count > 0 Then
            If Not UIHandler.Confirm("La información capturada se perdera. Esta seguro?", "¿Cancelar?") Then
                Exit Sub
            End If
        End If
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click

    End Sub

    Private Sub CobroFlete()
        If CStrDBNull(m_rowCliente("Acarreo")) = "S" Then
            AgregarFleteMunicipios()
        End If
    End Sub
    Private Sub AgregarFleteMunicipios()
        Dim CantidadFlete As Short = 0
        Dim sCodProducto As String = ""
        Dim sCapacidad As String = ""
        Dim UnidadMedida As String = ""
        Dim IdDetalleAutorizacion As String = ""
        Dim RowAutorizacion() As DataRow = Nothing
        Dim CantidadContado As Short = 0
        Dim CantidadCredito As Short = 0
        Dim Precio As PrecioProducto

        CantidadFlete = 0
        If Productos.ProductoActual.Flete = "S" Then
            Precio = Productos.GetPrecio("1" + Productos.ProductoActual.Codigo.Substring(1), CStr(m_rowCliente("Codigo")), CStr(m_rowCliente("CodSucursal")))
            If Not Precio.Precio = 0 Then
                dsVenta = Venta.dsVenta
                bsDetallePedido.DataSource = dsVenta
                If bsDetallePedido.Count = 0 Then
                    Exit Sub
                End If
                bsDetallePedido.AddNew()
                m_rowDetalle = CType(GetCurrentRow(bsDetallePedido), VentaDataSet.DetallePedidoRow)
                m_rowDetalle.NoPedido = CStr(m_rowPedido("NoPedido"))
                m_rowDetalle.CodTanque = rowTanque.NoTanque
                m_rowDetalle.CodProducto = "1" + Productos.ProductoActual.Codigo.Substring(1)
                m_rowDetalle.PesoInicial = 0
                m_rowDetalle.Kilometraje = Venta.KilometrajeFinal

                If rbtnNiveles.Checked Then
                    m_rowDetalle.MetodoEntrega = MetodosEntrega.PorNiveles
                ElseIf rbtnPorCaudalimetro.Checked Then
                    m_rowDetalle.MetodoEntrega = MetodosEntrega.PorCaudalimetro
                ElseIf rbtnPorPesoCamion.Checked Then
                    m_rowDetalle.MetodoEntrega = MetodosEntrega.PorPesoCamion
                ElseIf rbtnPorPesoTanque.Checked Then
                    m_rowDetalle.MetodoEntrega = MetodosEntrega.PorPesoTanque
                End If

                m_rowDetalle.PesoFinal = 0
                HojasRuta.LoadHojaRutaActual()
                m_rowDetalle.IdHojaRuta = HojasRuta.dsHojasRuta.HojasRuta(0).IdHojaRuta
                m_rowDetalle.Cantidad = Convert.ToDecimal(m_rowDetalle.Table.Compute("sum(CANTIDAD)", "1=1"))
                m_rowDetalle.PrecioUnitario = Precio.Precio
                m_rowDetalle.PrecioTotal = Precio.Precio * m_rowDetalle.Cantidad
                m_rowDetalle.PocentajeDescuento = Precio.PorcentajeDescuento
                m_rowDetalle.PorcentajeImpuesto = Precio.PorcentajeIVA
                m_rowDetalle.MontoDescuento = CDec((Precio.MontoDescuento * m_rowDetalle.Cantidad) / 100)
                m_rowDetalle.MontoImpuesto = CDec((Precio.PorcentajeIVA * m_rowDetalle.PrecioTotal) / 100)
                m_rowPedido("HoraAtencion") = Format(DateTime.Now, "HH:mm")
                bsDetallePedido.EndEdit()
                bsDetallePedido.ResetCurrentItem()
            End If
        End If
    End Sub
End Class