Imports System.Data
Imports MovilidadCF.Windows.Forms

Public Class AtencionPedidoForm

    Private m_rowCliente As NucleoDataSet.ClientesRow
    Private m_rowPedido As NucleoDataSet.PedidosRow

    Public Shared Sub Run(ByVal rowCliente As NucleoDataSet.ClientesRow, ByVal rowPedido As NucleoDataSet.PedidosRow)
        ' Se valida el estado del pédido
        If rowPedido.Estado <> EstadosPedido.Normal Then
            UIHandler.ShowError("Pedido ya fue atendido!")
            Exit Sub
        End If

        ' Se crea el formulario de atención del pédido
        UIHandler.StartWait()
        Dim form As New AtencionPedidoForm
        form.m_rowCliente = rowCliente
        form.m_rowPedido = rowPedido
        UIHandler.ShowDialog(form)
        UIHandler.EndWait()
        form.Dispose()
    End Sub

    Private Sub InformacionPedido_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        dsNucleo = GestorNucleo.dsNucleo
        bsClientes.DataSource = GestorNucleo.dsNucleo
        bsPedidos.DataSource = GestorNucleo.dsNucleo
        SetCurrentRow(bsClientes, m_rowCliente)
        SetCurrentRow(bsPedidos, m_rowPedido)
        Programa.ModulosPedido.Clear()
        Programa.LoadInfoModulosPedidos(m_rowCliente)
        CombinarMenusModulos(menuPedido, Programa.ModulosPedido, AddressOf menuModulosPedido_Click)
        Programa.InicioAtencionPedido(m_rowCliente, m_rowPedido)
        UIHandler.EndWait()
    End Sub

    Private Sub AtencionPedidoForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not ProcessHotKeys(Me, e) Then
            If e.KeyCode = Keys.Escape Then ' Salir sin guardar
                menuRegresar_Click(Me, Nothing)
            ElseIf e.KeyCode = Keys.A Then ' Anular Pedido
                menuAnularPedido_Click(Me, Nothing)
            Else
                ' Se procesan los menús adicionales del pedido
                Dim modulo As InfoModulo
                For Each modulo In Programa.ModulosPedido
                    If modulo.HotKey = e.KeyCode Then
                        EjecutarModulo(modulo)
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub menuRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuRegresar.Click
        If Programa.CerrarAtencionPedido(m_rowCliente, m_rowPedido) Then
            CerrarAtencionPedido()
        End If
    End Sub

    Private Sub CerrarAtencionPedido()
        'Dim rows() As DataRow
        Dim comunicacion As New Comunicacion
        'If dsNucleo.Parametros(0).IntervaloNovedades > 0 Then

        'comunicacion.ejecutarDescarga(GestorNucleo.ConnectionString , "Parcial",ProcesosComunicacion.DescargaParcial, "Envio de Descarga"

        'se quito por ahora se implementa nueva descarga de novedades
        'rows = dsNucleo.Pedidos.Select(String.Format("Estado <> '{0}' AND Estado <> '{1}'", _
        '    EstadosPedido.Normal, EstadosPedido.Enviado))
        'If rows.Length > 0 Then
        '    If DescargaDatosForm.Run(ProcesosComunicacion.DescargaNovedades, "Descarga Novedades", rows, "NoPedido,Estado", "PedidosAtendidos", True) Then
        '        UIHandler.StartWait()
        '        'TODO: Validar cambio de estado
        '        'For Each row In rows
        '        '    row("Estado") = EstadosPedido.Enviado
        '        'Next
        '        GestorNucleo.UpdatePedidos(rows)
        '        DialogResult = System.Windows.Forms.DialogResult.OK
        '        Exit Sub
        '    End If
        'End If
        'End If
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    ' Maneja el evento click en los menús de los modulos personalizados de la aplicación y ejecuta
    ' el módulo correspondiente
    Private Sub menuModulosPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Menu As ModuloMenuItem = CType(sender, ModuloMenuItem)
        EjecutarModulo(Menu.InfoModulo)
    End Sub

    Private Sub EjecutarModulo(ByVal modulo As InfoModulo)
        EjecutarModuloPedido(modulo, m_rowCliente, m_rowPedido)
        If m_rowCliente.RowState = Data.DataRowState.Modified Then
            GestorNucleo.UpdateCliente(m_rowCliente)
        End If
        If m_rowPedido.RowState = Data.DataRowState.Modified Then
            GestorNucleo.UpdatePedido(m_rowPedido)
        End If
        If m_rowPedido.Estado <> EstadosPedido.Normal Then
            ' Si el estado del pedido ha cambiado se sale la forma de atención
            CerrarAtencionPedido()
        End If
    End Sub

    Private Sub menuAnularPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAnularPedido.Click
        If UIHandler.Confirm("Esta seguro de anular el pedido actual?") Then
            m_rowPedido.Estado = EstadosPedido.AnuladoEnTerminal
            m_rowPedido.FechaAtencion = Today()
            m_rowPedido.HoraAtencion = Now.ToString("HH:mm")
            GestorNucleo.UpdatePedido(m_rowPedido)
            CerrarAtencionPedido()
        End If
    End Sub
End Class