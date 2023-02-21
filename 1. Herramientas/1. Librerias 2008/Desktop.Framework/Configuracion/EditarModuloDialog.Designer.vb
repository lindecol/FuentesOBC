<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditarModuloDialog
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
        Me.components = New System.ComponentModel.Container
        Dim NombreLabel As System.Windows.Forms.Label
        Dim IdTipoModuloLabel As System.Windows.Forms.Label
        Dim ClassURLLabel As System.Windows.Forms.Label
        Dim ClassURLLabel1 As System.Windows.Forms.Label
        Dim IdProgramaLabel As System.Windows.Forms.Label
        Me.dsConfig = New System.Data.DataSet
        Me.bsModulos = New System.Windows.Forms.BindingSource(Me.components)
        Me.NombreTextBox = New System.Windows.Forms.TextBox
        Me.cbTipoPrograma = New System.Windows.Forms.ComboBox
        Me.bsTiposModulo = New System.Windows.Forms.BindingSource(Me.components)
        Me.cbClase = New System.Windows.Forms.ComboBox
        Me.txtURL = New System.Windows.Forms.TextBox
        Me.cbPrograma = New System.Windows.Forms.ComboBox
        Me.bsProgramas = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.Shape1 = New Desktop.Windows.Forms.Shape
        Me.pnlPrograma = New System.Windows.Forms.Panel
        Me.pnlClase = New System.Windows.Forms.Panel
        Me.pnlProgramaClase = New System.Windows.Forms.FlowLayoutPanel
        Me.pnlURL = New System.Windows.Forms.Panel
        Me.btnActualizarPermisosEspeciales = New System.Windows.Forms.Button
        NombreLabel = New System.Windows.Forms.Label
        IdTipoModuloLabel = New System.Windows.Forms.Label
        ClassURLLabel = New System.Windows.Forms.Label
        ClassURLLabel1 = New System.Windows.Forms.Label
        IdProgramaLabel = New System.Windows.Forms.Label
        CType(Me.dsConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsModulos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsTiposModulo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsProgramas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPrograma.SuspendLayout()
        Me.pnlClase.SuspendLayout()
        Me.pnlProgramaClase.SuspendLayout()
        Me.pnlURL.SuspendLayout()
        Me.SuspendLayout()
        '
        'NombreLabel
        '
        NombreLabel.AutoSize = True
        NombreLabel.Location = New System.Drawing.Point(16, 13)
        NombreLabel.Name = "NombreLabel"
        NombreLabel.Size = New System.Drawing.Size(47, 13)
        NombreLabel.TabIndex = 1
        NombreLabel.Text = "Nombre:"
        '
        'IdTipoModuloLabel
        '
        IdTipoModuloLabel.AutoSize = True
        IdTipoModuloLabel.Location = New System.Drawing.Point(15, 58)
        IdTipoModuloLabel.Name = "IdTipoModuloLabel"
        IdTipoModuloLabel.Size = New System.Drawing.Size(69, 13)
        IdTipoModuloLabel.TabIndex = 2
        IdTipoModuloLabel.Text = "Tipo Módulo:"
        '
        'ClassURLLabel
        '
        ClassURLLabel.AutoSize = True
        ClassURLLabel.Location = New System.Drawing.Point(3, 8)
        ClassURLLabel.Name = "ClassURLLabel"
        ClassURLLabel.Size = New System.Drawing.Size(36, 13)
        ClassURLLabel.TabIndex = 4
        ClassURLLabel.Text = "Clase:"
        '
        'ClassURLLabel1
        '
        ClassURLLabel1.AutoSize = True
        ClassURLLabel1.Location = New System.Drawing.Point(4, 3)
        ClassURLLabel1.Name = "ClassURLLabel1"
        ClassURLLabel1.Size = New System.Drawing.Size(78, 13)
        ClassURLLabel1.TabIndex = 6
        ClassURLLabel1.Text = "URL Sitio Web"
        '
        'IdProgramaLabel
        '
        IdProgramaLabel.AutoSize = True
        IdProgramaLabel.Location = New System.Drawing.Point(1, 9)
        IdProgramaLabel.Name = "IdProgramaLabel"
        IdProgramaLabel.Size = New System.Drawing.Size(55, 13)
        IdProgramaLabel.TabIndex = 8
        IdProgramaLabel.Text = "Programa:"
        '
        'dsConfig
        '
        Me.dsConfig.DataSetName = "ConfiguracionDataSet"
        '
        'bsModulos
        '
        Me.bsModulos.DataSource = Me.dsConfig
        Me.bsModulos.Position = 0
        '
        'NombreTextBox
        '
        Me.NombreTextBox.Location = New System.Drawing.Point(19, 29)
        Me.NombreTextBox.Name = "NombreTextBox"
        Me.NombreTextBox.Size = New System.Drawing.Size(489, 20)
        Me.NombreTextBox.TabIndex = 2
        '
        'cbTipoPrograma
        '
        Me.cbTipoPrograma.DataSource = Me.bsTiposModulo
        Me.cbTipoPrograma.DisplayMember = "Descripcion"
        Me.cbTipoPrograma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTipoPrograma.FormattingEnabled = True
        Me.cbTipoPrograma.Location = New System.Drawing.Point(90, 55)
        Me.cbTipoPrograma.Name = "cbTipoPrograma"
        Me.cbTipoPrograma.Size = New System.Drawing.Size(419, 21)
        Me.cbTipoPrograma.TabIndex = 3
        Me.cbTipoPrograma.ValueMember = "IdTipoModulo"
        '
        'bsTiposModulo
        '
        Me.bsTiposModulo.DataSource = Me.dsConfig
        Me.bsTiposModulo.Position = 0
        '
        'cbClase
        '
        Me.cbClase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbClase.FormattingEnabled = True
        Me.cbClase.Location = New System.Drawing.Point(62, 5)
        Me.cbClase.Name = "cbClase"
        Me.cbClase.Size = New System.Drawing.Size(432, 21)
        Me.cbClase.TabIndex = 5
        '
        'txtURL
        '
        Me.txtURL.Location = New System.Drawing.Point(6, 19)
        Me.txtURL.Multiline = True
        Me.txtURL.Name = "txtURL"
        Me.txtURL.Size = New System.Drawing.Size(494, 49)
        Me.txtURL.TabIndex = 7
        '
        'cbPrograma
        '
        Me.cbPrograma.DataSource = Me.bsProgramas
        Me.cbPrograma.DisplayMember = "Nombre"
        Me.cbPrograma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrograma.FormattingEnabled = True
        Me.cbPrograma.Location = New System.Drawing.Point(62, 6)
        Me.cbPrograma.Name = "cbPrograma"
        Me.cbPrograma.Size = New System.Drawing.Size(434, 21)
        Me.cbPrograma.TabIndex = 9
        Me.cbPrograma.ValueMember = "IdPrograma"
        '
        'bsProgramas
        '
        Me.bsProgramas.DataSource = Me.dsConfig
        Me.bsProgramas.Position = 0
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(357, 175)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 10
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(438, 175)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 11
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'Shape1
        '
        Me.Shape1.Enabled = False
        Me.Shape1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(61, Byte), Integer), CType(CType(149, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Shape1.Location = New System.Drawing.Point(12, 164)
        Me.Shape1.Name = "Shape1"
        Me.Shape1.Size = New System.Drawing.Size(501, 4)
        Me.Shape1.Style = Desktop.Windows.Forms.Shape.ShapeStyle.TopLine
        Me.Shape1.TabIndex = 12
        Me.Shape1.Text = "Shape1"
        '
        'pnlPrograma
        '
        Me.pnlPrograma.Controls.Add(IdProgramaLabel)
        Me.pnlPrograma.Controls.Add(Me.cbPrograma)
        Me.pnlPrograma.Location = New System.Drawing.Point(0, 0)
        Me.pnlPrograma.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlPrograma.Name = "pnlPrograma"
        Me.pnlPrograma.Size = New System.Drawing.Size(501, 34)
        Me.pnlPrograma.TabIndex = 13
        '
        'pnlClase
        '
        Me.pnlClase.Controls.Add(Me.cbClase)
        Me.pnlClase.Controls.Add(ClassURLLabel)
        Me.pnlClase.Location = New System.Drawing.Point(0, 34)
        Me.pnlClase.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlClase.Name = "pnlClase"
        Me.pnlClase.Size = New System.Drawing.Size(500, 34)
        Me.pnlClase.TabIndex = 14
        '
        'pnlProgramaClase
        '
        Me.pnlProgramaClase.Controls.Add(Me.pnlPrograma)
        Me.pnlProgramaClase.Controls.Add(Me.pnlClase)
        Me.pnlProgramaClase.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.pnlProgramaClase.Location = New System.Drawing.Point(12, 80)
        Me.pnlProgramaClase.Name = "pnlProgramaClase"
        Me.pnlProgramaClase.Size = New System.Drawing.Size(501, 77)
        Me.pnlProgramaClase.TabIndex = 15
        Me.pnlProgramaClase.Visible = False
        '
        'pnlURL
        '
        Me.pnlURL.Controls.Add(Me.txtURL)
        Me.pnlURL.Controls.Add(ClassURLLabel1)
        Me.pnlURL.Location = New System.Drawing.Point(12, 80)
        Me.pnlURL.Name = "pnlURL"
        Me.pnlURL.Size = New System.Drawing.Size(501, 77)
        Me.pnlURL.TabIndex = 16
        Me.pnlURL.Visible = False
        '
        'btnActualizarPermisosEspeciales
        '
        Me.btnActualizarPermisosEspeciales.Location = New System.Drawing.Point(161, 175)
        Me.btnActualizarPermisosEspeciales.Name = "btnActualizarPermisosEspeciales"
        Me.btnActualizarPermisosEspeciales.Size = New System.Drawing.Size(190, 23)
        Me.btnActualizarPermisosEspeciales.TabIndex = 17
        Me.btnActualizarPermisosEspeciales.Text = "Actualizar Permisos Especiales"
        Me.btnActualizarPermisosEspeciales.UseVisualStyleBackColor = True
        '
        'EditarModuloDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(529, 207)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnActualizarPermisosEspeciales)
        Me.Controls.Add(Me.Shape1)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(IdTipoModuloLabel)
        Me.Controls.Add(Me.cbTipoPrograma)
        Me.Controls.Add(NombreLabel)
        Me.Controls.Add(Me.NombreTextBox)
        Me.Controls.Add(Me.pnlProgramaClase)
        Me.Controls.Add(Me.pnlURL)
        Me.Name = "EditarModuloDialog"
        Me.Text = "Editar Información Módulo"
        CType(Me.dsConfig, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsModulos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsTiposModulo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsProgramas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPrograma.ResumeLayout(False)
        Me.pnlPrograma.PerformLayout()
        Me.pnlClase.ResumeLayout(False)
        Me.pnlClase.PerformLayout()
        Me.pnlProgramaClase.ResumeLayout(False)
        Me.pnlURL.ResumeLayout(False)
        Me.pnlURL.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dsConfig As DataSet
    Friend WithEvents bsModulos As System.Windows.Forms.BindingSource
    Friend WithEvents NombreTextBox As System.Windows.Forms.TextBox
    Friend WithEvents cbTipoPrograma As System.Windows.Forms.ComboBox
    Friend WithEvents cbClase As System.Windows.Forms.ComboBox
    Friend WithEvents txtURL As System.Windows.Forms.TextBox
    Friend WithEvents cbPrograma As System.Windows.Forms.ComboBox
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents Shape1 As Desktop.Windows.Forms.Shape
    Friend WithEvents pnlPrograma As System.Windows.Forms.Panel
    Friend WithEvents pnlClase As System.Windows.Forms.Panel
    Friend WithEvents pnlProgramaClase As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlURL As System.Windows.Forms.Panel
    Friend WithEvents bsTiposModulo As System.Windows.Forms.BindingSource
    Friend WithEvents bsProgramas As System.Windows.Forms.BindingSource
    Friend WithEvents btnActualizarPermisosEspeciales As System.Windows.Forms.Button
End Class
