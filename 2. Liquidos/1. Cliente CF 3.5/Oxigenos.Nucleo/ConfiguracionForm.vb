Imports OpenNETCF
Imports OpenNETCF.Net
Imports MovilidadCF.Windows.Forms

Public Class ConfiguracionForm

    Public Shared Sub Run()
        UIHandler.StartWait()
        Dim frm As New ConfiguracionForm
        UIHandler.ShowDialog(frm)
        frm.Dispose()
        UIHandler.EndWait()
    End Sub

    Private Sub Form_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not ProcessHotKeys(Me, e) Then
            If e.Shift And e.KeyCode = Keys.Enter Then ' Guardar
                UIHandler.StartWait()
                SaveConfigData()
                DialogResult = System.Windows.Forms.DialogResult.OK
            ElseIf e.KeyCode = Keys.Escape Then ' Salir sin guardar
                UIHandler.StartWait()
                DialogResult = System.Windows.Forms.DialogResult.Cancel
            End If
        End If
    End Sub

    Private Sub ConfiguracionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Shape1.Style = MovilidadCF.Windows.Forms.Shape.ShapeStyle.GroupSeparator
        Me.txtIDTerminal.CaseStyle = MovilidadCF.Windows.Forms.CaseStyle.None
        Me.txtServerAddress.CaseStyle = MovilidadCF.Windows.Forms.CaseStyle.None
        Me.txtUserWebService.CaseStyle = MovilidadCF.Windows.Forms.CaseStyle.None
        Me.txtClaveWebService.CaseStyle = MovilidadCF.Windows.Forms.CaseStyle.None
        Me.txtPathWebService.CaseStyle = MovilidadCF.Windows.Forms.CaseStyle.None
        Me.txtServerAddressGPRS.CaseStyle = MovilidadCF.Windows.Forms.CaseStyle.None
        UiHandler1.Parent = Me
        LoadListData()
        LoadConfigData()
        tcConfiguracion.TabIndex = 0
        txtIDTerminal.Focus()
        UIHandler.EndWait()
    End Sub

    Private Sub LoadListData()
        ' Se carga la lista de conexiones disponibles
        Dim cm As New ConnectionManager
        Dim di As DestinationInfo
        cbConexiones.Items.Add("")
        For Each di In cm.EnumDestinations()
            cbConexiones.Items.Add(di.Description)
        Next

        ' Se inicializa la lista de programas
        cbPrograma.EnumType = GetType(Programas)
    End Sub

    Private Sub LoadConfigData()
        txtIDTerminal.Text = Settings.IDTerminal
        cbDeviceModel.SelectedIndex = Settings.DeviceModel
        cbPrograma.SelectedValue = Settings.Programa
        txtPathWebService.Text = Settings.VirtualDirectory
        txtServerAddress.Text = Settings.IPServidor
        Me.chkHTTPS.Checked = Settings.UsaHTTPS
        txtServerPort.Text = Settings.PuertoServidor.ToString()
        txtUserWebService.Text = Settings.UsuarioWebService
        txtClaveWebService.Text = Settings.ClaveWebService
        cbConexiones.SelectedItem = Settings.ConexionGPRS
        cbPuerto.SelectedItem = Settings.PrinterPort
        cbImpresora.SelectedIndex = Settings.PrinterModel
        cbTipoPuerto.SelectedIndex = Settings.PrinterPortType
        txtServerAddressGPRS.Text = Settings.IPServidorGPRS
    End Sub

    Private Sub menuCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCancelar.Click
        UIHandler.StartWait()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub menuGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuGuardar.Click
        UIHandler.StartWait()
        SaveConfigData()
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub SaveConfigData()
        Settings.IDTerminal = txtIDTerminal.Text
        Settings.DeviceModel = CType(cbDeviceModel.SelectedIndex, DeviceModels)
        Settings.Programa = CType(cbPrograma.SelectedValue, Programas)
        Settings.PrinterModel = CType(cbImpresora.SelectedIndex, PrinterModels)
        Settings.PrinterPort = cbPuerto.SelectedItem.ToString()
        Settings.PrinterPortType = CType(cbTipoPuerto.SelectedIndex, PrinterPortTypes)
        Settings.VirtualDirectory = txtPathWebService.Text
        Settings.IPServidor = txtServerAddress.Text
        Settings.UsaHTTPS = Me.chkHTTPS.Checked
        Settings.PuertoServidor = CInt(txtServerPort.Text)
        Settings.UsuarioWebService = txtUserWebService.Text
        Settings.ClaveWebService = txtClaveWebService.Text
        Settings.ConexionGPRS = cbConexiones.SelectedItem.ToString()
        Settings.IPServidorGPRS = txtServerAddressGPRS.Text
        Settings.SaveConfig()
    End Sub

    Private Sub btnProbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProbar.Click
        Dim pm As New ImprimirComprobantes(Settings.PrinterModel)
        
        If pm.ImprimirPrueba() Then
            ' Se confirma con el usuario si la impresion fue correcta
            UIHandler.ShowAlert("El documento fue impreso correctamente?", "Impresi?n")
        Else
            UIHandler.ShowError("Error imprimiendo?", "Impresi?n")
        End If
    End Sub
End Class