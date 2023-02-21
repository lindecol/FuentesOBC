<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ArchivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CerrarSesiónToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExportarConfiguracion = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImportarConfiguracion = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AyudaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AcercaDeCentauroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.lblServidor = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblDataBase = New System.Windows.Forms.ToolStripStatusLabel
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.NavegationPanelControl1 = New Desktop.Framework.NavigationPanelControl
        Me.LoginControl1 = New Desktop.Framework.LoginControl
        Me.pnlModulos = New System.Windows.Forms.Panel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.btnPrevious = New System.Windows.Forms.ToolStripButton
        Me.btnNext = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripSeparator
        Me.btnCambiarClave = New System.Windows.Forms.ToolStripButton
        Me.btnCerrarSesion = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.btnSalir = New System.Windows.Forms.ToolStripButton
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripSeparator
        Me.btnAbout = New System.Windows.Forms.ToolStripButton
        Me.pnlSesion = New System.Windows.Forms.Panel
        Me.lblUsuario = New System.Windows.Forms.Label
        Me.bsUsuarios = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsSesion = New Desktop.Framework.SesionDataset
        Me.lblEmpresa = New System.Windows.Forms.Label
        Me.bsEmpresas = New System.Windows.Forms.BindingSource(Me.components)
        Me.svDialogo = New System.Windows.Forms.SaveFileDialog
        Me.opDialogo = New System.Windows.Forms.OpenFileDialog
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.pnlSesion.SuspendLayout()
        CType(Me.bsUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsSesion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsEmpresas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem, Me.AyudaToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1016, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ArchivoToolStripMenuItem
        '
        Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CerrarSesiónToolStripMenuItem, Me.ToolStripMenuItem2, Me.mnuExportarConfiguracion, Me.mnuImportarConfiguracion, Me.ToolStripMenuItem1, Me.SalirToolStripMenuItem})
        Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
        Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
        Me.ArchivoToolStripMenuItem.Text = "Archivo"
        '
        'CerrarSesiónToolStripMenuItem
        '
        Me.CerrarSesiónToolStripMenuItem.Name = "CerrarSesiónToolStripMenuItem"
        Me.CerrarSesiónToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.CerrarSesiónToolStripMenuItem.Text = "Cerrar sesión"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(205, 6)
        '
        'mnuExportarConfiguracion
        '
        Me.mnuExportarConfiguracion.Name = "mnuExportarConfiguracion"
        Me.mnuExportarConfiguracion.Size = New System.Drawing.Size(208, 22)
        Me.mnuExportarConfiguracion.Text = "Exportar Configuracion..."
        '
        'mnuImportarConfiguracion
        '
        Me.mnuImportarConfiguracion.Name = "mnuImportarConfiguracion"
        Me.mnuImportarConfiguracion.Size = New System.Drawing.Size(208, 22)
        Me.mnuImportarConfiguracion.Text = "Importar Configuracion..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(205, 6)
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.SalirToolStripMenuItem.Text = "Salir"
        '
        'AyudaToolStripMenuItem
        '
        Me.AyudaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AcercaDeCentauroToolStripMenuItem})
        Me.AyudaToolStripMenuItem.Name = "AyudaToolStripMenuItem"
        Me.AyudaToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.AyudaToolStripMenuItem.Text = "Ayuda"
        '
        'AcercaDeCentauroToolStripMenuItem
        '
        Me.AcercaDeCentauroToolStripMenuItem.Name = "AcercaDeCentauroToolStripMenuItem"
        Me.AcercaDeCentauroToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.AcercaDeCentauroToolStripMenuItem.Text = "Acerca de ERP Software"
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.StatusStrip1)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainer1)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(1016, 663)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(1016, 710)
        Me.ToolStripContainer1.TabIndex = 2
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStrip1)
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblServidor, Me.lblDataBase})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1016, 22)
        Me.StatusStrip1.TabIndex = 0
        '
        'lblServidor
        '
        Me.lblServidor.Name = "lblServidor"
        Me.lblServidor.Size = New System.Drawing.Size(52, 17)
        Me.lblServidor.Text = "SERVER: "
        Me.lblServidor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDataBase
        '
        Me.lblDataBase.Name = "lblDataBase"
        Me.lblDataBase.Size = New System.Drawing.Size(66, 17)
        Me.lblDataBase.Text = "DATABASE: "
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.NavegationPanelControl1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LoginControl1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SplitContainer1.Panel2.Controls.Add(Me.pnlModulos)
        Me.SplitContainer1.Size = New System.Drawing.Size(1016, 663)
        Me.SplitContainer1.SplitterDistance = 257
        Me.SplitContainer1.SplitterWidth = 2
        Me.SplitContainer1.TabIndex = 0
        Me.SplitContainer1.TabStop = False
        '
        'NavegationPanelControl1
        '
        Me.NavegationPanelControl1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.NavegationPanelControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NavegationPanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NavegationPanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.NavegationPanelControl1.Margin = New System.Windows.Forms.Padding(0)
        Me.NavegationPanelControl1.Name = "NavegationPanelControl1"
        Me.NavegationPanelControl1.Size = New System.Drawing.Size(257, 663)
        Me.NavegationPanelControl1.TabIndex = 0
        '
        'LoginControl1
        '
        Me.LoginControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LoginControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LoginControl1.Location = New System.Drawing.Point(0, 0)
        Me.LoginControl1.Name = "LoginControl1"
        Me.LoginControl1.Size = New System.Drawing.Size(257, 663)
        Me.LoginControl1.TabIndex = 1
        Me.LoginControl1.Visible = False
        '
        'pnlModulos
        '
        Me.pnlModulos.BackColor = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.pnlModulos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlModulos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlModulos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlModulos.Location = New System.Drawing.Point(0, 0)
        Me.pnlModulos.Name = "pnlModulos"
        Me.pnlModulos.Size = New System.Drawing.Size(757, 663)
        Me.pnlModulos.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnPrevious, Me.btnNext, Me.ToolStripButton4, Me.btnCambiarClave, Me.btnCerrarSesion, Me.ToolStripSeparator1, Me.btnSalir, Me.ToolStripLabel1, Me.btnAbout})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(290, 25)
        Me.ToolStrip1.TabIndex = 0
        '
        'btnPrevious
        '
        Me.btnPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPrevious.Enabled = False
        Me.btnPrevious.Image = CType(resources.GetObject("btnPrevious.Image"), System.Drawing.Image)
        Me.btnPrevious.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(23, 22)
        Me.btnPrevious.Text = "ToolStripButton2"
        Me.btnPrevious.Visible = False
        '
        'btnNext
        '
        Me.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnNext.Enabled = False
        Me.btnNext.Image = CType(resources.GetObject("btnNext.Image"), System.Drawing.Image)
        Me.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(23, 22)
        Me.btnNext.Text = "ToolStripButton3"
        Me.btnNext.Visible = False
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(6, 25)
        Me.ToolStripButton4.Visible = False
        '
        'btnCambiarClave
        '
        Me.btnCambiarClave.Image = Global.Desktop.Framework.My.Resources.Resources.ico_16_securityrole
        Me.btnCambiarClave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCambiarClave.Name = "btnCambiarClave"
        Me.btnCambiarClave.Size = New System.Drawing.Size(106, 22)
        Me.btnCambiarClave.Text = "Cambiar clave..."
        '
        'btnCerrarSesion
        '
        Me.btnCerrarSesion.Image = Global.Desktop.Framework.My.Resources.Resources.ico_18_secure
        Me.btnCerrarSesion.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCerrarSesion.Name = "btnCerrarSesion"
        Me.btnCerrarSesion.Size = New System.Drawing.Size(96, 22)
        Me.btnCerrarSesion.Text = "Cerrrar Sesión"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        Me.ToolStripSeparator1.Visible = False
        '
        'btnSalir
        '
        Me.btnSalir.Image = CType(resources.GetObject("btnSalir.Image"), System.Drawing.Image)
        Me.btnSalir.ImageTransparentColor = System.Drawing.Color.White
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(47, 22)
        Me.btnSalir.Text = "Salir"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(6, 25)
        '
        'btnAbout
        '
        Me.btnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAbout.Image = CType(resources.GetObject("btnAbout.Image"), System.Drawing.Image)
        Me.btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.Size = New System.Drawing.Size(23, 22)
        Me.btnAbout.Text = "Acerca de..."
        '
        'pnlSesion
        '
        Me.pnlSesion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSesion.BackColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.pnlSesion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlSesion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSesion.Controls.Add(Me.lblUsuario)
        Me.pnlSesion.Controls.Add(Me.lblEmpresa)
        Me.pnlSesion.Location = New System.Drawing.Point(768, 0)
        Me.pnlSesion.Name = "pnlSesion"
        Me.pnlSesion.Size = New System.Drawing.Size(249, 50)
        Me.pnlSesion.TabIndex = 3
        Me.pnlSesion.Visible = False
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsUsuarios, "Nombre", True))
        Me.lblUsuario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsuario.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblUsuario.Location = New System.Drawing.Point(3, 25)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(124, 13)
        Me.lblUsuario.TabIndex = 1
        Me.lblUsuario.Text = "Néstor Javier Pira Lemus"
        '
        'bsUsuarios
        '
        Me.bsUsuarios.DataMember = "Usuarios"
        Me.bsUsuarios.DataSource = Me.dsSesion
        '
        'dsSesion
        '
        Me.dsSesion.DataSetName = "SesionDataset"
        Me.dsSesion.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'lblEmpresa
        '
        Me.lblEmpresa.AutoSize = True
        Me.lblEmpresa.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEmpresas, "NombreCompleto", True))
        Me.lblEmpresa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmpresa.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblEmpresa.Location = New System.Drawing.Point(3, 7)
        Me.lblEmpresa.Name = "lblEmpresa"
        Me.lblEmpresa.Size = New System.Drawing.Size(0, 13)
        Me.lblEmpresa.TabIndex = 0
        '
        'bsEmpresas
        '
        Me.bsEmpresas.DataMember = "Empresas"
        Me.bsEmpresas.DataSource = Me.dsSesion
        '
        'svDialogo
        '
        Me.svDialogo.Filter = "Archivo XML|*.xml"
        '
        'opDialogo
        '
        Me.opDialogo.FileName = "OpenFileDialog1"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 734)
        Me.Controls.Add(Me.pnlSesion)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainForm"
        Me.Text = "MovilidadCF Manager"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.pnlSesion.ResumeLayout(False)
        Me.pnlSesion.PerformLayout()
        CType(Me.bsUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsSesion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsEmpresas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ArchivoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SalirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AyudaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AcercaDeCentauroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents LoginControl1 As Desktop.Framework.LoginControl
    Friend WithEvents NavegationPanelControl1 As Desktop.Framework.NavigationPanelControl
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnCerrarSesion As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnNext As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnAbout As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSalir As System.Windows.Forms.ToolStripButton
    Friend WithEvents pnlModulos As System.Windows.Forms.Panel
    Friend WithEvents CerrarSesiónToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrevious As System.Windows.Forms.ToolStripButton
    Friend WithEvents pnlSesion As System.Windows.Forms.Panel
    Friend WithEvents lblUsuario As System.Windows.Forms.Label
    Friend WithEvents lblEmpresa As System.Windows.Forms.Label
    Friend WithEvents bsEmpresas As System.Windows.Forms.BindingSource
    Friend WithEvents bsUsuarios As System.Windows.Forms.BindingSource
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblServidor As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblDataBase As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnCambiarClave As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuExportarConfiguracion As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents svDialogo As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuImportarConfiguracion As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents opDialogo As System.Windows.Forms.OpenFileDialog
    Friend WithEvents dsSesion As Desktop.Framework.SesionDataset
End Class
