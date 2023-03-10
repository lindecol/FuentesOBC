Imports MovilidadCF.Windows.Forms

Public Class InicioRutaForm
    Implements IModuloPrograma
    Private m_rowHojaRuta As HojasRutaDataSet.HojasRutaRow
    Private m_DatosTanque As VentaDataSet.TanquesClienteDataTable
    Private m_bUpdateNumeroMovimiento As Boolean = False

    Private Sub InicioRutaForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not Nucleo.ProcessHotKeys(Me, e) Then
            If e.KeyCode = Keys.Escape Then
                menuCancelar_Click(Me, Nothing)
                If e.KeyCode = Keys.Enter And e.Shift Then
                    menuGuardar_Click(Me, Nothing)
                End If
            End If
        End If
    End Sub

    Private Sub InicioRutaForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        LoadData()
        cbProducto.Focus()
        UIHandler.EndWait()
    End Sub

    Public Sub Run() Implements Common.IModuloPrograma.Run
        Dim sMensaje As String = ""
        HojasRuta.LoadHojaRutaActual()
        If HojasRuta.dsHojasRuta.HojasRuta.Count > 0 Then
            If HojasRuta.dsHojasRuta.HojasRuta(0).Estado = EstadosHojaRuta.Iniciada Then
                UIHandler.ShowError("La hoja de ruta ya fue iniciada, ya no se puede modificar!")
                Exit Sub
            End If
        End If
        UIHandler.StartWait()
        UIHandler.ShowDialog(Me)
        Me.Dispose()
        UIHandler.EndWait()
    End Sub

    Public Sub LoadData()
        dsHojasRuta = HojasRuta.dsHojasRuta
        dsProductos = Productos.dsProductos
        bsChoferes.DataSource = dsHojasRuta
        bsHojasRuta.DataSource = dsHojasRuta
        bsLugaresCarga.DataSource = dsHojasRuta
        bsProductos.DataSource = dsProductos
        bsTanqueros.DataSource = dsHojasRuta
        bsTractores.DataSource = dsHojasRuta
        Productos.LoadProductos()
        HojasRuta.LoadChoferes()
        HojasRuta.LoadLugaresCarga()
        HojasRuta.LoadTractores()
        If CDbl(Nucleo.NumeroMovimiento) <= HojasRuta.NumerohojarutaActual() Then
            Nucleo.NumeroMovimiento = CStr(CInt(HojasRuta.NumerohojarutaActual()) + 1)
        End If
        If bsHojasRuta.Count = 0 Then
            bsHojasRuta.AddNew()
            m_rowHojaRuta = CType(GetCurrentRow(bsHojasRuta), HojasRutaDataSet.HojasRutaRow)

            'Nucleo.NumeroMovimiento = CStr(CInt(Nucleo.NumeroMovimiento) + 1)
            m_rowHojaRuta.IdHojaRuta = CInt(Nucleo.NumeroMovimiento)
            m_rowHojaRuta.LLegadaInicioRuta = Now()
            m_rowHojaRuta.SalidaInicioRuta = Now()
            m_bUpdateNumeroMovimiento = True
        Else
            bsHojasRuta.MoveFirst()
            m_rowHojaRuta = CType(GetCurrentRow(bsHojasRuta), HojasRutaDataSet.HojasRutaRow)
            If Not IsNullOrEmpty(m_rowHojaRuta("Tanquero")) Then
                HojasRuta.LoadTanqueros(m_rowHojaRuta.CodProducto)
            End If
        End If
        bsHojasRuta.ResetCurrentItem()
    End Sub

    Private Sub menuCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCancelar.Click
        UIHandler.StartWait()
        bsHojasRuta.CancelEdit()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    ''' <summary>
    ''' Graba los datos de la hoja ruta
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub guardarDatos()

        'm_rowHojaRuta.Estado = EstadosHojaRuta.Iniciada
        Try

            m_rowHojaRuta.Estado = EstadosHojaRuta.Iniciada
            ' Se guarda la hoja de ruta
            HojasRuta.OpenConnection()

            HojasRuta.BeginTransaction()

            HojasRuta.UpdateHojaRuta(m_rowHojaRuta)

            ' Se calcula la existencia inicial y se almacena en el kardex
            Dim nExistencias As Decimal = m_rowHojaRuta.PesoFinal - m_rowHojaRuta.PesoInicial
            Dim rowProducto As ProductosDataSet.ProductosRow
            rowProducto = dsProductos.Productos.FindByCodigo(m_rowHojaRuta.CodProducto)
            If rowProducto.UnidadMedidaVenta = UnidadesMedida.Kilogramos Then
                nExistencias = CDec(nExistencias * rowProducto.CoeficienteProducto)
            End If

            Productos.InitKardex(m_rowHojaRuta.IdHojaRuta, m_rowHojaRuta.CodProducto, nExistencias)

            ' Se guardan los datos adicionales
            Dim rowConductor As HojasRutaDataSet.ChoferesRow
            rowConductor = CType(GetCurrentRow(bsChoferes), HojasRutaDataSet.ChoferesRow)
            Nucleo.RowParametros("CodigoChofer") = m_rowHojaRuta.CodConductor
            Nucleo.RowParametros("NombreChofer") = rowConductor.Nombre
            If Not IsNullOrEmpty(m_rowHojaRuta("KilometrajeIniicial")) Then
                Nucleo.KilometrajeInicial = m_rowHojaRuta.KilometrajeIniicial
            End If
            If Not IsNullOrEmpty(m_rowHojaRuta("KilometrajeFinal")) Then
                Nucleo.KiloMetrajeFinal = m_rowHojaRuta.KilometrajeFinal
            End If

            'If m_bUpdateNumeroMovimiento Then
            Nucleo.NumeroMovimiento = CStr(CInt(Nucleo.NumeroMovimiento) + 1)
            'End If

            HojasRuta.CommitTransaction()
        Catch ex As Exception
            HojasRuta.RollbackTransaction()
            Throw New Exception(ex.ToString)
        Finally
            HojasRuta.CloseConnection()
        End Try
    End Sub

    Private Sub menuGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuGuardar.Click
        Try


            menuIniciarRuta_Click(sender, e)

            DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            UIHandler.EndWait()
            UIHandler.ShowError(ex.Message)
        End Try
    End Sub

    Private Sub menuIniciarRuta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuIniciarRuta.Click
        Try
            UIHandler.StartWait()
            bsHojasRuta.EndEdit()
            If IsNullOrEmpty(m_rowHojaRuta("Cabezote")) Then
                Throw New Exception("Debe especificar la placa del Cabezote")
            End If
            If IsNullOrEmpty(m_rowHojaRuta("Tanquero")) Then
                Throw New Exception("Debe especificar la placa del Tanquero")
            End If
            If IsNullOrEmpty(m_rowHojaRuta("CodConductor")) Then
                Throw New Exception("Debe seleccionar un conductor")
            End If
            If IsNullOrEmpty(m_rowHojaRuta("CodLugarCargue")) Then
                Throw New Exception("Debe seleccionar el lugar de cargue")
            End If
            If IsNullOrEmpty(m_rowHojaRuta("CodProducto")) Then
                Throw New Exception("Debe seleccionar el producto")
            End If
            If IsNullOrEmpty(m_rowHojaRuta("PesoInicial")) Then
                Throw New Exception("Debe especificar el peso inicial")
            End If
            If IsNullOrEmpty(m_rowHojaRuta("PesoFinal")) Then
                Throw New Exception("Debe especificar el peso final")
            End If
            If IsNullOrEmpty(m_rowHojaRuta("KilometrajeIniicial")) Then
                Throw New Exception("Debe espececificar el Kilomentra Inicial")
            End If

            If CInt(m_rowHojaRuta("KilometrajeIniicial")) < CInt(Nucleo.KiloMetrajeFinal) Then
                If MessageBox.Show("El kilometraje inicial es menor al ultimo ingresado " & CStr(Nucleo.KiloMetrajeFinal) & " Desea continuar?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.No Then
                    Throw New Exception("El kilometraje inicial debe ser mayor al ?ltimo kilometraje egistrado")
                End If
            End If

            If IsNullOrEmpty(m_rowHojaRuta("NumRemision")) Then
                Throw New Exception("Debe especificar el n?mero de la remisi?n del lugar de cargue")
            End If
            If DateTimePicker1.Value < CDate(Nucleo.FechaCarga.Month & "/" & Nucleo.FechaCarga.Day & "/" & Nucleo.FechaCarga.Year & " 12:00:01 AM") Or DateTimePicker2.Value < CDate(Nucleo.FechaCarga.Month & "/" & Nucleo.FechaCarga.Day & "/" & Nucleo.FechaCarga.Year & " 12:00:01 AM") Then
                Throw New Exception("Las Fechas de Llegada y Salida deben ser superiores a la fecha de carga " & Nucleo.FechaCarga)
            End If
            If DateTimePicker1.Value > DateTimePicker2.Value Then
                Throw New Exception("La fecha de llegada no puede ser superior a la fecha de salida")
            End If
            If CDbl(NumericInputBox1.Text) > CDbl(NumericInputBox2.Text) Then
                Throw New Exception("La pesada inicial no puede ser mayor a la pesada final")
            End If

            'datos del tanque            
            m_DatosTanque = Venta.LoadTanqueCliente(m_rowHojaRuta("Tanquero").ToString(), m_rowHojaRuta("Tanquero").ToString(), m_rowHojaRuta("CodProducto").ToString())
            Dim fila As DataRow
            For Each fila In m_DatosTanque.Rows
                If Val(fila("CAPACIDAD")) < (Val(m_rowHojaRuta("PesoInicial").ToString())) Then
                    Throw New Exception("El nivel inicial del tanque es superior a la capacidad del tanque")
                End If

                If Val(fila("CAPACIDAD")) < (Val(m_rowHojaRuta("PesoFinal").ToString())) Then
                    Throw New Exception("El nivel final del tanque es superior a la capacidad del tanque")
                End If

                If Val(fila("CAPACIDAD")) < (Val(m_rowHojaRuta("PesoFinal").ToString()) - Val(m_rowHojaRuta("PesoInicial").ToString())) Then
                    Throw New Exception("La carga del tanque no puede ser superior a la capacidad del mismo")
                End If
            Next

            ''graba la hoja ruta
            guardarDatos()

            'se envian datos por gprs
            enviarDatosGprs()

            DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            UIHandler.EndWait()
            UIHandler.ShowError(ex.Message)
        End Try
    End Sub

    Private Sub cbProducto_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbProducto.SelectedValueChanged
        UIHandler.StartWait()
        If cbProducto.SelectedIndex >= 0 Then
            HojasRuta.LoadTanqueros(CStr(cbProducto.SelectedValue))
        Else
            bsTanqueros.SuspendBinding()
            dsHojasRuta.Tanqueros.Rows.Clear()
            bsTanqueros.ResumeBinding()
        End If
        UIHandler.EndWait()
    End Sub

    Private Sub TextInputBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextInputBox1.TextChanged

    End Sub

    Private Sub enviarDatosGprs()
        Try
            'Dim lstQuery As New ListQuerysDescarga
            'Programa.LoadQuerysDescargaParcial(lstQuery)
            'DescargaDatosForm.Run(ProcesosComunicacion.DescargaParcial, "Descarga datos", lstQuery, True)
            Nucleo.DescargarDatos(2, "Descarga datos", "Parcial")
        Catch ex As Exception
            Dim strError As String
            strError = ex.ToString
        End Try
    End Sub

End Class