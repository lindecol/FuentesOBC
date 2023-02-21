Imports MovilidadCF.Windows.Forms

Public Class ReportesForm
    Implements IModuloPrograma

    Private m_Reportes As GestorReportes
    Private m_ReportHojaRuta As GestorHojasRutas

    Private doc As PrinterDocument ' Obejto para crear el documento impreso

    Private Sub menuRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuRegresar.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Public Sub Run() Implements Common.IModuloPrograma.Run
        ' Se hacen las validaciones para dar inicio al cargue
        'If Nucleo.KilometrajeInicial <> " " And Nucleo.KilometrajeInicial <> "0" Then
        '    UIHandler.ShowError("La ruta ya se inicio!!")
        '    ' se muestra el resumen del camión
        '    ResumenCargaForm.Run()
        'Else
        UIHandler.StartWait()
        UIHandler.ShowDialog(Me)
        Me.Dispose()
        UIHandler.EndWait()
        'End If
    End Sub

    Private Sub ReportesForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not Nucleo.ProcessHotKeys(Me, e) Then
            If e.KeyCode = System.Windows.Forms.Keys.Escape Then
                menuRegresar_Click(Me, Nothing)
            End If
        End If
    End Sub

    Private Sub ReportesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        m_Reportes = New GestorReportes
        btnPedidosAnulados.Focus()
        UIHandler.EndWait()
    End Sub

    Private Sub btnPedidosAnulados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPedidosAnulados.Click
        Try
            Dim dt As DataTable = m_Reportes.GetPedidosAnulados()
            Dim report As New PrinterReport("PEDIDOS ANULADOS", dt)
            report.Columnas("Nombre").Total = "Total Registros: " & dt.Rows.Count.ToString()
            Nucleo.Imprimir(report)
        Catch
        End Try
    End Sub

    Private Sub btnDocumentosGenerados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocumentosGenerados.Click
        Try
            Dim dt As DataTable = m_Reportes.GetDocumentosGenerados
            Dim report As New PrinterReport("RESUMEN DOCUMENTOS GENERADOS", dt)
            report.Columnas("Valor").Total = CDbl(dt.Compute("SUM(Valor)", "")).ToString("#,##0.00")
            Nucleo.Imprimir(report)
        Catch ex As Exception
            UIHandler.ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnHojaRuta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHojaRuta.Click
        LLenarReporteHojaRuta()
    End Sub
    Public Sub LLenarReporteHojaRuta()
        Try
            '****** Validacion kilometraje
            If Nucleo.KilometrajeInicial = " " Or Nucleo.KilometrajeInicial = "0" Then
                UIHandler.ShowError("La ruta no se ha iniciado!!")
                Exit Sub
            End If

            '****** Iniciación  de variables
            doc = New PrinterDocument
            m_ReportHojaRuta = New GestorHojasRutas
            m_ReportHojaRuta.loadReporteHojaRuta()
            Dim diasemana As String
            With m_ReportHojaRuta.dsHojasRuta.ReporteHojaRuta(0)
                Dim strProducto As String = .CodProducto & " - " & .Descripcion
                Dim strTotalMcargados As String = CStr(Math.Round(((.PesoFinal - .PesoInicial) * .CoeficienteProducto), 2))
                Dim lnTotalHoras As String
                Dim lnTotalMin As String
                lnTotalHoras = CStr(DateDiff(DateInterval.Hour, .SalidaInicioRuta, .FechaCierre))
                lnTotalMin = Str(Math.Round((CDbl(DateDiff(DateInterval.Minute, .SalidaInicioRuta, .FechaCierre)) / 60), 2))
                If CDbl(lnTotalMin) > 1 Then
                    lnTotalMin = CStr(CDbl(lnTotalMin) - CDbl(lnTotalHoras))
                    lnTotalMin = CStr(Math.Round((CDbl(lnTotalMin) * 100), 0))
                End If
                lnTotalHoras = lnTotalHoras + " h " + lnTotalMin + " min"
                doc.SetInfoencabezadoHojaRuta(strProducto, strTotalMcargados, .Tanquero, .Cabezote, CStr(.SalidaInicioRuta.TimeOfDay.ToString), CStr(.FechaCierre.TimeOfDay.ToString), .KilometrajeIniicial, .KilometrajeFinal, CStr(.FechaCierre.Day & "/" & .FechaCierre.Month & "/" & .FechaCierre.Year), CStr(.SalidaInicioRuta.Day & "/" & .SalidaInicioRuta.Month & "/" & .SalidaInicioRuta.Year), CStr(.IdHojaRuta), Nucleo.CodigoTerminal, lnTotalHoras, (CDbl(.KilometrajeIniicial) - CDbl(.KilometrajeFinal)), .NombreChofer, .cedula, .NombreEmpresaTrasportadora, .CodigoPuntoVenta)
                'llenarprimer registro
                If .IsDiaSemanaNull Then
                    diasemana = "0"
                Else
                    diasemana = .DiaSemana.ToString
                End If
                diasemana = CStr(.SalidaInicioRuta.Day & "/" & .SalidaInicioRuta.Month)
                doc.LlenarDetalleHojaRuta(.CodLugarCargue & " - " & .Lugarcarga, .NumRemision, diasemana, .KilometrajeIniicial, CStr(.SalidaInicioRuta.TimeOfDay.ToString).Substring(0, 5), CStr(.LLegadaInicioRuta.TimeOfDay.ToString).Substring(0, 5), "   -", "    -", CStr(.PesoInicial), CStr(.PesoFinal), CStr(Math.Round(((.PesoFinal - .PesoInicial) * .CoeficienteProducto))))
            End With

            '****Detalle Reporte
            m_ReportHojaRuta.LoadDetalleHojaRuta()
            Dim rw As HojasRutaDataSet.ReporteDetalleHojaRutaRow
            Dim presionini As String
            Dim presionfin As String
            Dim nivelini As String
            Dim nivelfin As String
            'KUXAN: SOLUCION ERROR REPORTE RUTA
            Dim horaprogramada As String
            Dim horaatencion As String
            For Each rw In m_ReportHojaRuta.dsHojasRuta.ReporteDetalleHojaRuta
                If rw.IsNivelInicialNull Then
                    nivelini = "0"
                Else
                    nivelini = rw.NivelInicial.ToString
                End If
                If rw.IsNivelFinalNull Then
                    nivelfin = "0"
                Else
                    nivelfin = rw.NivelFinal.ToString
                End If
                If rw.IsPresionInicialNull Then
                    presionini = "0"
                Else
                    presionini = rw.PresionInicial.ToString
                End If
                If rw.IsPresionFinalNull Then
                    presionfin = "0"
                Else
                    presionfin = rw.PresionFinal.ToString
                End If
                diasemana = CStr(rw.Fecha.Day & "/" & rw.Fecha.Month)

                If rw.IsHoraProgramadaNull Then
                    horaprogramada = "00:00"
                Else
                    horaprogramada = rw.HoraProgramada
                End If

                If rw.IsHoraAtencionNull Then
                    If rw.IsHoraProgramadaNull Then
                        horaatencion = "00:00"
                    Else
                        horaatencion = rw.HoraProgramada
                    End If
                Else
                    horaatencion = rw.HoraAtencion
                End If

                doc.LlenarDetalleHojaRuta(rw.ClienteNombre & "-" & rw.tanque, rw.TipoDoc & "-" & rw.NoFactura, diasemana, rw.Kilometraje, horaprogramada, horaatencion, presionini, presionfin, nivelini, nivelfin, CStr(rw.Cantidad))
            Next

            Nucleo.ImprimirHojaRuta(doc)


        Catch ex As Exception
            UIHandler.ShowError(ex.Message)
        End Try
    End Sub
End Class