Imports MovilidadCF.Windows.Forms

Public Class ReportesForm
    Implements IModuloPrograma

    Private m_Reportes As GestorReportes

    Private Sub menuRegresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuRegresar.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Public Sub Run() Implements Common.IModuloPrograma.Run
        If Nucleo.KiloMetrajeFinal = " " Or Nucleo.KiloMetrajeFinal = "0" Then
            UIHandler.ShowError("La ruta no se ha finalizado!!")
        Else
            UIHandler.StartWait()
            UIHandler.ShowDialog(Me)
            Me.Dispose()
            UIHandler.EndWait()
        End If
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
            Dim dt As DataTable = m_Reportes.GetDocumentosGenerados()
            Dim report As New PrinterReport("RESUMEN DOCUMENTOS GENERADOS", dt)
            report.Columnas("CREDITO").Total = CDbl(dt.Compute("SUM(Credito)", "")).ToString("#,##0.00")
            report.Columnas("CONTADO").Total = CDbl(dt.Compute("SUM(Contado)", "")).ToString("#,##0.00")
            Nucleo.Imprimir(report)
        Catch ex As Exception
            UIHandler.ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnControlEnvases_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnControlEnvases.Click
        Try
            Dim dt As DataTable = m_Reportes.GetControlEnvases()
            Dim report As New PrinterReport("RESUMEN CONTROL ENVASES", dt)
            report.Columnas("SalidaLlenos").Total = CDbl(dt.Compute("SUM(SalidaLlenos)", "")).ToString("#,##0.00")
            report.Columnas("SalidaVacios").Total = CDbl(dt.Compute("SUM(SalidaVacios)", "")).ToString("#,##0.00")
            report.Columnas("Ventas").Total = CDbl(dt.Compute("SUM(Ventas)", "")).ToString("#,##0.00")
            report.Columnas("Asignaciones").Total = CDbl(dt.Compute("SUM(Asignaciones)", "")).ToString("#,##0.00")
            report.Columnas("Recolecciones").Total = CDbl(dt.Compute("SUM(Recolecciones)", "")).ToString("#,##0.00")
            report.Columnas("VaciosEntregados").Total = CDbl(dt.Compute("SUM(VaciosEntregados)", "")).ToString("#,##0.00")
            report.Columnas("RetornoLlenos").Total = CDbl(dt.Compute("SUM(RetornoLlenos)", "")).ToString("#,##0.00")
            report.Columnas("RetornoVacios").Total = CDbl(dt.Compute("SUM(RetornoVacios)", "")).ToString("#,##0.00")
            Nucleo.Imprimir(report)
        Catch ex As Exception
            UIHandler.ShowError(ex.Message)
        End Try
    End Sub
End Class