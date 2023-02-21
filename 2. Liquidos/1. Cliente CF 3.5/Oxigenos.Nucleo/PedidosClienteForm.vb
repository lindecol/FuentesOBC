Imports MovilidadCF.Windows.Forms

Public Class PedidosClienteForm

    Private m_rowCliente As NucleoDataSet.ClientesRow
    Private m_rowPedido As NucleoDataSet.PedidosRow

    'uso gprs
    Private m_Thread As OpenNETCF.Threading.Thread2
    Private m_QueryList As ListQuerysDescarga
    Private m_bUsarGPRS As Boolean
    Private m_sData As String = Nothing
    Private m_ws As OxigenosWS.DataAccess

    Public Shared Sub Run(ByVal rowCliente As NucleoDataSet.ClientesRow)
        UIHandler.StartWait()
        Dim frm As New PedidosClienteForm
        frm.m_rowCliente = rowCliente
        UIHandler.ShowDialog(frm)
        frm.Dispose()
        UIHandler.EndWait()
    End Sub


    Private Sub PedidosClienteForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Se inicializan los controles
        UiHandler1.Parent = Me
        bsCliente.DataSource = GestorNucleo.dsNucleo
        bsPedidos.DataSource = GestorNucleo.dsNucleo
        bsPedidos.Filter = "CodCliente='" & m_rowCliente.Codigo & "'"
        SetCurrentRow(bsCliente, m_rowCliente)

        ' Se configura el menú con los módulos de programa
        Programa.ModulosPedido.Clear()
        Programa.LoadInfoModulosPedidos(m_rowCliente)
        UIHandler.EndWait()
    End Sub

    Private Sub PedidosClienteForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not ProcessHotKeys(Me, e) Then
            If e.KeyCode = Keys.Escape Then ' Salir sin guardar
                menuRegresar_Click(Me, Nothing)
            ElseIf e.KeyCode = Keys.Enter Or e.KeyCode = Keys.A Then ' Acción predeterminada 
                menuAtencionPedido_Click(Me, Nothing)
            ElseIf e.KeyCode = Keys.N Then ' Nuevo Pedido
                menuNuevoPedido_Click(Me, Nothing)
            End If
        End If
    End Sub

    Private Sub menuRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuRegresar.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub nuevoPedido()
        Try
            Dim row As NucleoDataSet.PedidosRow
            bsPedidos.AddNew()
            row = CType(GetCurrentRow(bsPedidos), NucleoDataSet.PedidosRow)

            row.NoPedido = Nucleo.PedidoActual.ToString("0000000")
            row.CodSucursal = Nucleo.CodigoSucursal
            row.CodCliente = m_rowCliente.Codigo
            row.Direccion = m_rowCliente.Direccion.Trim
            row.Barrio = ""
            row.Observacion = ""
            row.Recoleccion = ""
            row.FechaPedido = Today()
            row.FechaProgramada = Today()
            row.HoraProgramada = Now.ToString("HH:MM")
            row.PrimerServicio = "0"
            row.Solicito = ""
            row.Estado = EstadosPedido.Normal
            row.Nuevo = "1"
            bsPedidos.EndEdit()
            GestorNucleo.UpdatePedido(row)
            Nucleo.PedidoActual += 1
        Catch ex As Exception
            Nucleo.PedidoActual += 1
            nuevoPedido()
        End Try
    End Sub
    Private Sub menuNuevoPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNuevoPedido.Click
        nuevoPedido()
    End Sub

    Private Sub menuAtencionPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAtencionPedido.Click
        Dim row As NucleoDataSet.PedidosRow
        If bsPedidos.Count > 0 Then
            row = CType(GetCurrentRow(bsPedidos), NucleoDataSet.PedidosRow)
            If row IsNot Nothing Then

                AtencionPedidoForm.Run(m_rowCliente, row)
                enviarDatosGprs()
            End If
        Else
            UIHandler.ShowError("No hay pedidos para este cliente")
        End If
    End Sub

    Private Sub DataGrid1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.DoubleClick
        Dim row As NucleoDataSet.PedidosRow
        If bsPedidos.Count > 0 Then
            row = CType(GetCurrentRow(bsPedidos), NucleoDataSet.PedidosRow)
            If row IsNot Nothing Then
                AtencionPedidoForm.Run(m_rowCliente, row)
                enviarDatosGprs()
            End If
        End If
    End Sub

    Private Sub enviarDatosGprs()
        Try
            'Dim lstQuery As New ListQuerysDescarga
            'Programa.LoadQuerysDescargaParcial(lstQuery)
            'DescargaDatosForm.Run(ProcesosComunicacion.DescargaParcial, "Descarga datos", lstQuery, True)
            Dim comunicacion As New Comunicacion
            comunicacion.ejecutarDescarga(GestorNucleo.CadenaConexion, "Parcial", ProcesosComunicacion.DescargaParcial, "Descarga datos")
        Catch ex As Exception
            Dim strError As String
            strError = ex.ToString
        End Try
    End Sub
End Class