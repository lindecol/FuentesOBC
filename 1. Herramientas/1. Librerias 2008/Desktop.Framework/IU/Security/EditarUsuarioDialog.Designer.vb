<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditarUsuarioDialog
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
        Dim UsuarioLabel As System.Windows.Forms.Label
        Dim NombreLabel As System.Windows.Forms.Label
        Dim EmailLabel As System.Windows.Forms.Label
        Dim IdEmpresaLabel As System.Windows.Forms.Label
        Me.UsuarioTextBox = New System.Windows.Forms.TextBox
        Me.bsUsuarios = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsSeguridad = New Desktop.Framework.SeguridadDataset
        Me.NombreTextBox = New System.Windows.Forms.TextBox
        Me.EmailTextBox = New System.Windows.Forms.TextBox
        Me.SuperUserCheckBox = New System.Windows.Forms.CheckBox
        Me.MultiEmpresaCheckBox = New System.Windows.Forms.CheckBox
        Me.ActivoCheckBox = New System.Windows.Forms.CheckBox
        Me.IdEmpresaComboBox = New System.Windows.Forms.ComboBox
        Me.bsEmpresa = New System.Windows.Forms.BindingSource(Me.components)
        Me.Shape1 = New Desktop.Windows.Forms.Shape
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.bsEmpresasUsuarios = New System.Windows.Forms.BindingSource(Me.components)
        Me.FKUsuariosEmpresasBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.FKUsuariosEmpresasBindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        UsuarioLabel = New System.Windows.Forms.Label
        NombreLabel = New System.Windows.Forms.Label
        EmailLabel = New System.Windows.Forms.Label
        IdEmpresaLabel = New System.Windows.Forms.Label
        CType(Me.bsUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsEmpresa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsEmpresasUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FKUsuariosEmpresasBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FKUsuariosEmpresasBindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UsuarioLabel
        '
        UsuarioLabel.AutoSize = True
        UsuarioLabel.Location = New System.Drawing.Point(23, 26)
        UsuarioLabel.Name = "UsuarioLabel"
        UsuarioLabel.Size = New System.Drawing.Size(46, 13)
        UsuarioLabel.TabIndex = 1
        UsuarioLabel.Text = "Usuario:"
        '
        'NombreLabel
        '
        NombreLabel.AutoSize = True
        NombreLabel.Location = New System.Drawing.Point(22, 52)
        NombreLabel.Name = "NombreLabel"
        NombreLabel.Size = New System.Drawing.Size(47, 13)
        NombreLabel.TabIndex = 2
        NombreLabel.Text = "Nombre:"
        '
        'EmailLabel
        '
        EmailLabel.AutoSize = True
        EmailLabel.Location = New System.Drawing.Point(34, 78)
        EmailLabel.Name = "EmailLabel"
        EmailLabel.Size = New System.Drawing.Size(35, 13)
        EmailLabel.TabIndex = 4
        EmailLabel.Text = "Email:"
        '
        'IdEmpresaLabel
        '
        IdEmpresaLabel.AutoSize = True
        IdEmpresaLabel.Location = New System.Drawing.Point(18, 104)
        IdEmpresaLabel.Name = "IdEmpresaLabel"
        IdEmpresaLabel.Size = New System.Drawing.Size(51, 13)
        IdEmpresaLabel.TabIndex = 12
        IdEmpresaLabel.Text = "Empresa:"
        '
        'UsuarioTextBox
        '
        Me.UsuarioTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsUsuarios, "Usuario", True))
        Me.UsuarioTextBox.Location = New System.Drawing.Point(75, 23)
        Me.UsuarioTextBox.MaxLength = 10
        Me.UsuarioTextBox.Name = "UsuarioTextBox"
        Me.UsuarioTextBox.Size = New System.Drawing.Size(175, 20)
        Me.UsuarioTextBox.TabIndex = 2
        '
        'bsUsuarios
        '
        Me.bsUsuarios.DataMember = "Usuarios"
        Me.bsUsuarios.DataSource = Me.dsSeguridad
        '
        'dsSeguridad
        '
        Me.dsSeguridad.DataSetName = "SeguridadDataset"
        Me.dsSeguridad.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'NombreTextBox
        '
        Me.NombreTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsUsuarios, "Nombre", True))
        Me.NombreTextBox.Location = New System.Drawing.Point(75, 49)
        Me.NombreTextBox.MaxLength = 200
        Me.NombreTextBox.Name = "NombreTextBox"
        Me.NombreTextBox.Size = New System.Drawing.Size(479, 20)
        Me.NombreTextBox.TabIndex = 3
        '
        'EmailTextBox
        '
        Me.EmailTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsUsuarios, "Email", True))
        Me.EmailTextBox.Location = New System.Drawing.Point(75, 75)
        Me.EmailTextBox.MaxLength = 200
        Me.EmailTextBox.Name = "EmailTextBox"
        Me.EmailTextBox.Size = New System.Drawing.Size(479, 20)
        Me.EmailTextBox.TabIndex = 5
        '
        'SuperUserCheckBox
        '
        Me.SuperUserCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.bsUsuarios, "SuperUser", True))
        Me.SuperUserCheckBox.Location = New System.Drawing.Point(185, 134)
        Me.SuperUserCheckBox.Name = "SuperUserCheckBox"
        Me.SuperUserCheckBox.Size = New System.Drawing.Size(104, 24)
        Me.SuperUserCheckBox.TabIndex = 7
        Me.SuperUserCheckBox.Text = "Super Usuario"
        '
        'MultiEmpresaCheckBox
        '
        Me.MultiEmpresaCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.bsUsuarios, "MultiEmpresa", True))
        Me.MultiEmpresaCheckBox.Location = New System.Drawing.Point(322, 134)
        Me.MultiEmpresaCheckBox.Name = "MultiEmpresaCheckBox"
        Me.MultiEmpresaCheckBox.Size = New System.Drawing.Size(104, 24)
        Me.MultiEmpresaCheckBox.TabIndex = 9
        Me.MultiEmpresaCheckBox.Text = "Multi empresa"
        '
        'ActivoCheckBox
        '
        Me.ActivoCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.bsUsuarios, "Activo", True))
        Me.ActivoCheckBox.Location = New System.Drawing.Point(75, 134)
        Me.ActivoCheckBox.Name = "ActivoCheckBox"
        Me.ActivoCheckBox.Size = New System.Drawing.Size(104, 24)
        Me.ActivoCheckBox.TabIndex = 11
        Me.ActivoCheckBox.Text = "Activo"
        '
        'IdEmpresaComboBox
        '
        Me.IdEmpresaComboBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.bsUsuarios, "IdEmpresa", True))
        Me.IdEmpresaComboBox.DataSource = Me.bsEmpresa
        Me.IdEmpresaComboBox.DisplayMember = "NombreCorto"
        Me.IdEmpresaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.IdEmpresaComboBox.FormattingEnabled = True
        Me.IdEmpresaComboBox.Location = New System.Drawing.Point(75, 101)
        Me.IdEmpresaComboBox.Name = "IdEmpresaComboBox"
        Me.IdEmpresaComboBox.Size = New System.Drawing.Size(479, 21)
        Me.IdEmpresaComboBox.TabIndex = 13
        Me.IdEmpresaComboBox.ValueMember = "IdEmpresa"
        '
        'bsEmpresa
        '
        Me.bsEmpresa.DataMember = "Empresas"
        Me.bsEmpresa.DataSource = Me.dsSeguridad
        '
        'Shape1
        '
        Me.Shape1.Enabled = False
        Me.Shape1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Shape1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.Shape1.Location = New System.Drawing.Point(21, 164)
        Me.Shape1.Name = "Shape1"
        Me.Shape1.Size = New System.Drawing.Size(533, 10)
        Me.Shape1.Style = Desktop.Windows.Forms.Shape.ShapeStyle.TopLine
        Me.Shape1.TabIndex = 14
        Me.Shape1.Text = "Permisos Especiales Módulo seleccionado:"
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(386, 180)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 15
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(479, 180)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 16
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'bsEmpresasUsuarios
        '
        Me.bsEmpresasUsuarios.DataMember = "FK_Usuarios_Empresas"
        Me.bsEmpresasUsuarios.DataSource = Me.bsEmpresa
        '
        'FKUsuariosEmpresasBindingSource
        '
        Me.FKUsuariosEmpresasBindingSource.DataMember = "FK_Usuarios_Empresas"
        Me.FKUsuariosEmpresasBindingSource.DataSource = Me.bsEmpresa
        '
        'FKUsuariosEmpresasBindingSource1
        '
        Me.FKUsuariosEmpresasBindingSource1.DataMember = "FK_Usuarios_Empresas"
        Me.FKUsuariosEmpresasBindingSource1.DataSource = Me.bsEmpresa
        '
        'EditarUsuarioDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(582, 216)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.Shape1)
        Me.Controls.Add(IdEmpresaLabel)
        Me.Controls.Add(Me.IdEmpresaComboBox)
        Me.Controls.Add(Me.ActivoCheckBox)
        Me.Controls.Add(Me.MultiEmpresaCheckBox)
        Me.Controls.Add(Me.SuperUserCheckBox)
        Me.Controls.Add(EmailLabel)
        Me.Controls.Add(Me.EmailTextBox)
        Me.Controls.Add(NombreLabel)
        Me.Controls.Add(Me.NombreTextBox)
        Me.Controls.Add(UsuarioLabel)
        Me.Controls.Add(Me.UsuarioTextBox)
        Me.Name = "EditarUsuarioDialog"
        Me.Text = "EditarUsuarioDialog"
        CType(Me.bsUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsEmpresa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsEmpresasUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FKUsuariosEmpresasBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FKUsuariosEmpresasBindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bsUsuarios As System.Windows.Forms.BindingSource
    Friend WithEvents UsuarioTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NombreTextBox As System.Windows.Forms.TextBox
    Friend WithEvents EmailTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SuperUserCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents MultiEmpresaCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ActivoCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IdEmpresaComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Shape1 As Desktop.Windows.Forms.Shape
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents bsEmpresa As System.Windows.Forms.BindingSource
    Friend WithEvents bsEmpresasUsuarios As System.Windows.Forms.BindingSource
    Friend WithEvents FKUsuariosEmpresasBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents FKUsuariosEmpresasBindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents dsSeguridad As DataSet
End Class
