<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainConfiguracionForm
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainConfiguracionForm))
		Me.ToolStripPanel1 = New System.Windows.Forms.ToolStripPanel
		Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
		Me.ArchivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.DefiniciónEmpresasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.DefiniciónProgramasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.ConfiguraciónMódulosYCategoriasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.ConfiguraciónReportesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
		Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
		Me.btnEmpresas = New System.Windows.Forms.ToolStripButton
		Me.btnProgramas = New System.Windows.Forms.ToolStripButton
		Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
		Me.btnModulos = New System.Windows.Forms.ToolStripButton
		Me.btnReportes = New System.Windows.Forms.ToolStripButton
		Me.btnCrearUsuarioInicial = New System.Windows.Forms.ToolStripButton
		Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
		Me.btnGuardar = New System.Windows.Forms.ToolStripButton
		Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
		Me.btnSalir = New System.Windows.Forms.ToolStripButton
		Me.ToolStripPanel1.SuspendLayout()
		Me.MenuStrip1.SuspendLayout()
		Me.ToolStrip1.SuspendLayout()
		Me.SuspendLayout()
		'
		'ToolStripPanel1
		'
		Me.ToolStripPanel1.Controls.Add(Me.MenuStrip1)
		Me.ToolStripPanel1.Controls.Add(Me.ToolStrip1)
		Me.ToolStripPanel1.Dock = System.Windows.Forms.DockStyle.Top
		Me.ToolStripPanel1.Location = New System.Drawing.Point(0, 0)
		Me.ToolStripPanel1.Name = "ToolStripPanel1"
		Me.ToolStripPanel1.Orientation = System.Windows.Forms.Orientation.Horizontal
		Me.ToolStripPanel1.RowMargin = New System.Windows.Forms.Padding(0)
		Me.ToolStripPanel1.Size = New System.Drawing.Size(699, 49)
		'
		'MenuStrip1
		'
		Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
		Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem})
		Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.MenuStrip1.Name = "MenuStrip1"
		Me.MenuStrip1.Size = New System.Drawing.Size(699, 24)
		Me.MenuStrip1.TabIndex = 0
		Me.MenuStrip1.Text = "MenuStrip1"
		'
		'ArchivoToolStripMenuItem
		'
		Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DefiniciónEmpresasToolStripMenuItem, Me.DefiniciónProgramasToolStripMenuItem, Me.ConfiguraciónMódulosYCategoriasToolStripMenuItem, Me.ConfiguraciónReportesToolStripMenuItem, Me.ToolStripMenuItem1, Me.SalirToolStripMenuItem})
		Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
		Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
		Me.ArchivoToolStripMenuItem.Text = "Archivo"
		'
		'DefiniciónEmpresasToolStripMenuItem
		'
		Me.DefiniciónEmpresasToolStripMenuItem.Name = "DefiniciónEmpresasToolStripMenuItem"
		Me.DefiniciónEmpresasToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
		Me.DefiniciónEmpresasToolStripMenuItem.Text = "Definición Empresas..."
		'
		'DefiniciónProgramasToolStripMenuItem
		'
		Me.DefiniciónProgramasToolStripMenuItem.Name = "DefiniciónProgramasToolStripMenuItem"
		Me.DefiniciónProgramasToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
		Me.DefiniciónProgramasToolStripMenuItem.Text = "Definición Programas...."
		'
		'ConfiguraciónMódulosYCategoriasToolStripMenuItem
		'
		Me.ConfiguraciónMódulosYCategoriasToolStripMenuItem.Name = "ConfiguraciónMódulosYCategoriasToolStripMenuItem"
		Me.ConfiguraciónMódulosYCategoriasToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
		Me.ConfiguraciónMódulosYCategoriasToolStripMenuItem.Text = "Configuración Módulos y Categorias...."
		'
		'ConfiguraciónReportesToolStripMenuItem
		'
		Me.ConfiguraciónReportesToolStripMenuItem.Name = "ConfiguraciónReportesToolStripMenuItem"
		Me.ConfiguraciónReportesToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
		Me.ConfiguraciónReportesToolStripMenuItem.Text = "Configuración Reportes..."
		'
		'ToolStripMenuItem1
		'
		Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
		Me.ToolStripMenuItem1.Size = New System.Drawing.Size(277, 6)
		'
		'SalirToolStripMenuItem
		'
		Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
		Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
		Me.SalirToolStripMenuItem.Text = "Salir"
		'
		'ToolStrip1
		'
		Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
		Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(18, 18)
		Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnEmpresas, Me.btnProgramas, Me.ToolStripSeparator2, Me.btnModulos, Me.btnReportes, Me.btnCrearUsuarioInicial, Me.ToolStripSeparator1, Me.btnGuardar, Me.ToolStripSeparator3, Me.btnSalir})
		Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
		Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
		Me.ToolStrip1.Name = "ToolStrip1"
		Me.ToolStrip1.Size = New System.Drawing.Size(627, 25)
		Me.ToolStrip1.TabIndex = 1
		'
		'btnEmpresas
		'
		Me.btnEmpresas.Image = CType(resources.GetObject("btnEmpresas.Image"), System.Drawing.Image)
		Me.btnEmpresas.ImageTransparentColor = System.Drawing.Color.White
		Me.btnEmpresas.Name = "btnEmpresas"
		Me.btnEmpresas.Size = New System.Drawing.Size(88, 22)
		Me.btnEmpresas.Text = "Empresas..."
		'
		'btnProgramas
		'
		Me.btnProgramas.Image = CType(resources.GetObject("btnProgramas.Image"), System.Drawing.Image)
		Me.btnProgramas.ImageTransparentColor = System.Drawing.Color.White
		Me.btnProgramas.Name = "btnProgramas"
		Me.btnProgramas.Size = New System.Drawing.Size(95, 22)
		Me.btnProgramas.Text = "Programas..."
		'
		'ToolStripSeparator2
		'
		Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
		Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 23)
		'
		'btnModulos
		'
		Me.btnModulos.Image = CType(resources.GetObject("btnModulos.Image"), System.Drawing.Image)
		Me.btnModulos.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btnModulos.Name = "btnModulos"
		Me.btnModulos.Size = New System.Drawing.Size(85, 22)
		Me.btnModulos.Text = "Módulos..."
		'
		'btnReportes
		'
		Me.btnReportes.Image = CType(resources.GetObject("btnReportes.Image"), System.Drawing.Image)
		Me.btnReportes.ImageTransparentColor = System.Drawing.Color.White
		Me.btnReportes.Name = "btnReportes"
		Me.btnReportes.Size = New System.Drawing.Size(84, 22)
		Me.btnReportes.Text = "Reportes..."
		'
		'btnCrearUsuarioInicial
		'
        Me.btnCrearUsuarioInicial.Image = Global.Desktop.Framework.My.Resources.Resources.ImportFromDatabase
		Me.btnCrearUsuarioInicial.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btnCrearUsuarioInicial.Name = "btnCrearUsuarioInicial"
		Me.btnCrearUsuarioInicial.Size = New System.Drawing.Size(103, 22)
		Me.btnCrearUsuarioInicial.Text = "Usuario Inicial"
		'
		'ToolStripSeparator1
		'
		Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
		Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 23)
		'
		'btnGuardar
		'
		Me.btnGuardar.Image = CType(resources.GetObject("btnGuardar.Image"), System.Drawing.Image)
		Me.btnGuardar.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btnGuardar.Name = "btnGuardar"
		Me.btnGuardar.Size = New System.Drawing.Size(71, 22)
		Me.btnGuardar.Text = "Guardar"
		'
		'ToolStripSeparator3
		'
		Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
		Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 23)
		'
		'btnSalir
		'
		Me.btnSalir.Image = CType(resources.GetObject("btnSalir.Image"), System.Drawing.Image)
		Me.btnSalir.ImageTransparentColor = System.Drawing.Color.White
		Me.btnSalir.Name = "btnSalir"
		Me.btnSalir.Size = New System.Drawing.Size(51, 22)
		Me.btnSalir.Text = "Salir"
		'
		'MainConfiguracionForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(699, 468)
		Me.Controls.Add(Me.ToolStripPanel1)
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.IsMdiContainer = True
		Me.MainMenuStrip = Me.MenuStrip1
		Me.Margin = New System.Windows.Forms.Padding(4)
		Me.Name = "MainConfiguracionForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Herramienta configuración MovilidadCF 1.0.0.0"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		Me.ToolStripPanel1.ResumeLayout(False)
		Me.ToolStripPanel1.PerformLayout()
		Me.MenuStrip1.ResumeLayout(False)
		Me.MenuStrip1.PerformLayout()
		Me.ToolStrip1.ResumeLayout(False)
		Me.ToolStrip1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ToolStripPanel1 As System.Windows.Forms.ToolStripPanel
	Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
	Friend WithEvents btnEmpresas As System.Windows.Forms.ToolStripButton
	Friend WithEvents btnModulos As System.Windows.Forms.ToolStripButton
	Friend WithEvents btnProgramas As System.Windows.Forms.ToolStripButton
	Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
	Friend WithEvents btnReportes As System.Windows.Forms.ToolStripButton
	Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
	Friend WithEvents ArchivoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents btnSalir As System.Windows.Forms.ToolStripButton
	Friend WithEvents DefiniciónEmpresasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents DefiniciónProgramasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ConfiguraciónMódulosYCategoriasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ConfiguraciónReportesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
	Friend WithEvents SalirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
	Friend WithEvents btnCrearUsuarioInicial As System.Windows.Forms.ToolStripButton
	Friend WithEvents btnGuardar As System.Windows.Forms.ToolStripButton
	Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
End Class
