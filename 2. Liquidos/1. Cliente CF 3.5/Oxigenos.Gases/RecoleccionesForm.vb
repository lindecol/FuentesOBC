Imports MovilidadCF.Symbol
Imports MovilidadCF.Windows.Forms

Public Class RecoleccionesForm
    Implements IModuloPedido
    Private WithEvents m_BarcodeReader As BarcodeReader
    Public RowProducto As ProductosDataSet.ProductosRow
    Public RowLeidos As DataRow
    Public m_RowCliente As DataRow
    Public m_RowPedido As DataRow
    Public sPertenencia As String = ""
    Public bCapturaManual As Boolean = False
    Public sSecuencialAjeno As String = ""
    Private Recoleccion As String = "R"
    Private CantidadAsignaciones As Integer = 0
    Private TipoAsignacion As String = ""

#Region "Eventos"

    Private Sub RecoleccionesForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not Nucleo.ProcessHotKeys(Me, e) Then
            If e.KeyCode = System.Windows.Forms.Keys.Escape Then
                btnRegresar_Click(Me, Nothing)
            End If
        End If
    End Sub

    Private Sub RecoleccionesForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        ' Se inicializa el scanner
        MovilidadCF.Symbol.BarcodeReader.InitReader()
        m_BarcodeReader = New BarcodeReader
        m_BarcodeReader.StartRead(Me)
        Barcode.LoadData()
        LimpiarControles()
        Me.dsVenta = Venta.dsVenta
        Me.dsProductos = Productos.dsProductos
        Me.cbPertenencia.Focus()
        UIHandler.EndWait()
    End Sub

    Private Sub txtSerial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSerial.KeyPress
        If e.KeyChar = vbCr Then
            If cbPertenencia.SelectedIndex >= 0 And Me.txtProducto.Text <> "" And _
            Me.lblDescripcionProducto.Text <> "" And Me.txtCapacidad.Text <> "" Then
                If Me.txtSerial.Text <> "" Then
                    sSecuencialAjeno = Me.txtSerial.Text
                    GuardarDatos(sSecuencialAjeno)
                End If
            Else
                UIHandler.ShowError("Ingrese los datos solicitados!!")
            End If
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        ' Se llama la forma para seleccionar el producto
        Dim Row As DataRow
        Dim CodProducto As String = ""
        Row = BuscarProductoForm.Run("", Nothing, True)
        If Row IsNot Nothing Then
            ' Se trae el codigo y la descripción del producto
            Me.txtProducto.Text = CStr(Row("Codigo"))
            Me.lblDescripcionProducto.Text = CStr(Row("Descripcion"))
        End If
    End Sub

    Private Sub btnRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegresar.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.Yes
    End Sub

    Private Sub cbPertenencia_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPertenencia.SelectedIndexChanged
        Dim nIndex As Integer = 0
        nIndex = cbPertenencia.SelectedIndex
        If nIndex >= 0 Then
            If cbPertenencia.Text = "CLIENTE" Then
                sPertenencia = Pertenencia.Cliente
                Me.lblSerial.Visible = True
                Me.txtSerial.Visible = True
                Me.lblCantidad.Visible = False
                Me.txtCantidad.Visible = False
                Me.txtCantidad.Text = "1"
            ElseIf cbPertenencia.Text = "PRAXAIR" Then
                sPertenencia = Pertenencia.Praxair
                Me.lblSerial.Visible = False
                Me.txtSerial.Visible = False
                Me.lblCantidad.Visible = True
                Me.txtCantidad.Visible = True
                Me.txtSerial.Text = ""
            End If
        End If
    End Sub

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress
        If e.KeyChar = vbCr Then
            sSecuencialAjeno = ""
            If cbPertenencia.SelectedIndex >= 0 And Me.txtProducto.Text <> "" And _
            Me.lblDescripcionProducto.Text <> "" And Me.txtCapacidad.Text <> "" And _
            Me.txtCantidad.Text <> "" Then
                If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente Then
                    If Not VerAsignacionesForm.Run(TipoAsignacion, CInt(Me.txtCantidad.Text)) Then
                        Me.txtCapacidad.Text = ""
                        Me.txtCantidad.Text = ""
                        Me.txtCapacidad.Focus()
                        Exit Sub
                    End If
                End If
                GuardarDatos(sSecuencialAjeno)
            Else
                UIHandler.ShowError("Ingrese los datos solicitados!!")
            End If
        End If
    End Sub

    Private Sub txtCapacidad_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCapacidad.GotFocus
        If Me.txtCapacidad.Text <> "" Then
            Me.txtCapacidad.Text.Replace(",", "")
        End If
    End Sub

    Private Sub txtCapacidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCapacidad.KeyPress
        If e.KeyChar = vbCr And txtCapacidad.Text <> "" Then
            If Me.txtCapacidad.Text.Length <= 2 Then
                If cstrDBNULL(m_RowCliente("CodTipoCliente")) = TiposCliente.Paciente And _
                    cbPertenencia.Text = "PRAXAIR" Then
                    If Me.txtProducto.Text = "" Then
                        UIHandler.ShowAlert("Ingrese el código del producto!!")
                        Exit Sub
                    End If

                    ' Se busca el tipo de asignacion
                    Pacientes.GetTipoAsignacion(Me.txtProducto.Text, (CDblDBNull(QuitarFormato(Me.txtCapacidad.Text)) * 1000).ToString("##0"))
                    If Pacientes.dsPacientes.DetallesTipoAsignacion.Rows.Count > 0 Then

                        TipoAsignacion = cstrDBNULL(Pacientes.dsPacientes.DetallesTipoAsignacion.Rows(0)("Codigo"))

                        ' Se buscan las asignaciones pendientes
                        Pacientes.GetAsignaciones(cstrDBNULL(m_RowCliente("Codigo")), cstrDBNULL(m_RowPedido("CodEntidad")), _
                        TipoAsignacion)

                        CantidadAsignaciones = Pacientes.dsPacientes.Asignaciones.Rows.Count
                        Me.txtCapacidad.Text = Format(CLng(Me.txtCapacidad.Text), "###,##0")

                    Else
                        UIHandler.ShowError("No se encontró el tipo de asignación!!")
                        Me.txtCapacidad.Text = ""
                        Me.txtCapacidad.Focus()
                    End If
                End If
            Else
                UIHandler.ShowError("Error en Capacidad!!")
                Me.txtCapacidad.Text = ""
                Me.txtCapacidad.Focus()
            End If
        End If
    End Sub

    Private Sub txtProducto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtProducto.KeyPress
        If e.KeyChar = vbCr Then
            If Me.txtProducto.Text <> "" Then
                RowProducto = Productos.GetProducto(Me.txtProducto.Text)
                If RowProducto IsNot Nothing Then
                    Me.lblDescripcionProducto.Text = RowProducto.Descripcion
                Else
                    UIHandler.ShowError("Código no existe!!")
                    Me.lblDescripcionProducto.Text = ""
                    Me.txtProducto.Text = ""
                    Me.txtProducto.Focus()
                End If
            Else
                UIHandler.ShowError("Ingrese el producto!!")
                Me.txtProducto.Focus()
            End If
        End If
    End Sub

#End Region

    Public Sub Run(ByVal rowCliente As System.Data.DataRow, ByVal rowPedido As System.Data.DataRow) Implements Common.IModuloPedido.Run
        Me.m_RowCliente = rowCliente
        Me.m_RowPedido = rowPedido
        UIHandler.StartWait()
        UIHandler.ShowDialog(Me)
        Me.Dispose()
        UIHandler.EndWait()
    End Sub

    Private Sub m_BarcodeReader_ScanComplete(ByVal e As MovilidadCF.Symbol.ScanCompleteArgs) Handles m_BarcodeReader.ScanComplete
        Try
            Barcode.Parse(e.Text, e.Type)
            If Not Barcode.NoDefinido Is Nothing Then
                UIHandler.ShowAlert("Código no Definido!!")
            ElseIf Not Barcode.Serial Is Nothing Then
                If txtSerial.Visible Then
                    Me.txtSerial.Text = Barcode.Serial
                    If cbPertenencia.SelectedIndex >= 0 And Me.txtProducto.Text <> "" And _
                    Me.lblDescripcionProducto.Text <> "" And Me.txtCapacidad.Text <> "" Then
                        sSecuencialAjeno = Barcode.Serial
                        GuardarDatos(sSecuencialAjeno)
                    Else
                        UIHandler.ShowError("Ingrese los datos solicitados!!")
                    End If
                Else
                    UIHandler.ShowAlert("Código Inválido!!")
                End If
            End If
        Catch ex As Exception
            UIHandler.ShowError(ex.Message)
        End Try
    End Sub

    Public Sub LimpiarControles()
        Me.txtProducto.Text = ""
        Me.txtCapacidad.Text = ""
        Me.txtSerial.Text = ""
        Me.txtCantidad.Text = ""
        Me.lblDescripcionProducto.Text = ""
        Me.cbPertenencia.SelectedIndex = -1
    End Sub

    Public Sub GuardarDatos(ByVal Secuencial As String)
        Dim RowCilindroCliente As DataRow
        Dim sCapacidad As String
        ' Si se lee serial se debe validar que éste no se encuentre
        ' en la tabla de cilindros cliente
        If Secuencial <> "" Then
            RowCilindroCliente = Productos.GetSecuencialAjenoVacio(Secuencial)
            If RowCilindroCliente IsNot Nothing Then
                UIHandler.ShowError("Error el serial leído es para entrega!!")
                Me.txtSerial.Text = ""
                Me.txtSerial.Focus()
                Exit Sub
            End If
        End If
        sCapacidad = (CDblDBNull(QuitarFormato(Me.txtCapacidad.Text)) * 1000).ToString("##0")
        RowLeidos = Venta.dsVenta.CilindrosLeidos.FindByCodSucursalCodProductoCapacidadSecuencialPertenenciaCodTipoProductoSecuencialAjeno(Nucleo.CodigoSucursal, _
        Me.txtProducto.Text, sCapacidad, "", sPertenencia, "", Secuencial)

        If RowLeidos IsNot Nothing Then
            UIHandler.ShowError("Cilindro ya Registrado!!")
            Exit Sub
        End If

        Productos.LoadProducto(Me.txtProducto.Text)

        'Se inserta en el datatable de Leídos
        Venta.dsVenta.CilindrosLeidos.AddCilindrosLeidosRow(Nucleo.CodigoSucursal, Me.txtProducto.Text, sCapacidad, _
         "", sPertenencia, "", Secuencial, CShort(txtCantidad.Text), cstrDBNULL(Me.dsProductos.Productos.Rows(0)("UnidadMedida")), "N", 0)

        ' Se limpian los controles
        Me.lblItems.Text = CStr(CInt(Me.lblItems.Text) + 1)
        If sPertenencia = Pertenencia.Cliente Then
            Me.txtSerial.Text = ""
            Me.txtSerial.Focus()
        Else
            Me.txtProducto.Text = ""
            Me.lblDescripcionProducto.Text = ""
            Me.txtCapacidad.Text = ""
            Me.txtCantidad.Text = ""
            Me.txtProducto.Focus()
        End If
    End Sub

End Class