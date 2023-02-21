Public Class GestorVenta

    Public KilometrajeFinal As String

    Public Sub LoadTanquesCliente(ByVal m_rowCliente As Data.DataRow)
        Me.dtaTanquesCliente.Connection = Me.Connection
        Me.dtaTanquesCliente.Fill(Me.dsVenta.TanquesCliente, CStr(m_rowCliente("Codigo")), _
            HojasRuta.dsHojasRuta.HojasRuta(0).CodProducto, HojasRuta.dsHojasRuta.HojasRuta(0).CodProducto)
    End Sub

    ''' <summary>
    ''' Obtiene los datos del tanque del cliente
    ''' </summary>
    ''' <param name="NoTanque">No tanque</param>
    ''' <param name="CodCliente">Cliente</param>
    ''' <param name="CodProducto">Producto</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadTanqueCliente(ByVal NoTanque As String, ByVal CodCliente As String, ByVal CodProducto As String) As VentaDataSet.TanquesClienteDataTable
        Me.dtaTanquesCliente.Connection = Me.Connection
        Return Me.dtaTanquesCliente.TanqueCliente(NoTanque, CodCliente, CodProducto, CodProducto)
    End Function


    Public Sub LoadAforos(ByVal rowTanque As VentaDataSet.TanquesClienteRow)
        Me.dtaAforos.Connection = Me.Connection
        Me.dtaAforos.Fill(Me.dsVenta.Aforos, rowTanque.IdFabricante, _
            rowTanque.IdTipo, rowTanque.IdFamilia, rowTanque.IdGrupoRecipiente)
    End Sub

    Public Sub SaveVenta()
        Try

            Me.OpenConnection()
            Me.BeginTransaction()

            Me.dtaDetallePedido.Connection = Me.Connection
            Me.dtaDetalleFactura.Connection = Me.Connection
            Me.dtaMaestroFacturas.Connection = Me.Connection
            Me.dtaGuia_Documento.Connection = Me.Connection

            Me.dtaDetallePedido.Update(Me.dsVenta.DetallePedido)
            Me.dtaMaestroFacturas.Update(Me.dsVenta.MaestroFacturas)
            Me.dtaDetalleFactura.Update(Me.dsVenta.DetalleFactura)
            Me.dtaGuia_Documento.Update(Me.dsVenta.Guia_Documento)

            Me.dsVenta.DetallePedido.AcceptChanges()
            Me.dsVenta.Guia_Documento.AcceptChanges()
            Me.dsVenta.MaestroFacturas.AcceptChanges()
            Me.dsVenta.DetalleFactura.AcceptChanges()

            Nucleo.KiloMetrajeFinal = KilometrajeFinal

            'actualiza el kardex del camion
            Productos.UpdateKardex()

            Me.CommitTransaction()

            Me.CloseConnection()
        Catch ex As Exception
            Me.RollbackTransaction()
            Throw New Exception(ex.ToString())
        End Try
    End Sub

End Class
