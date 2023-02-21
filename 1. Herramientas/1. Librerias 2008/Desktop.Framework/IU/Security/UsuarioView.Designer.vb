<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UsuarioView
    Inherits Desktop.Framework.BaseViewControl

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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UsuarioView))
        Me.HeaderStrip1 = New Desktop.Windows.Forms.HeaderStrip
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.dgvModulos = New System.Windows.Forms.DataGridView
        Me.UsuarioDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NombreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EmpresaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ActivoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.SuperUserDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.MultiEmpresaDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.FechaCreacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.bsUsuarios = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsSeguridad = New Desktop.Framework.SeguridadDataset
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.btnAgregarUsuario = New System.Windows.Forms.ToolStripButton
        Me.btnEditarUsuario = New System.Windows.Forms.ToolStripButton
        Me.btnEliminarUsuario = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.btnCambiarClave = New System.Windows.Forms.ToolStripButton
        Me.btnPerfilesUsuario = New System.Windows.Forms.ToolStripButton
        Me.HeaderStrip1.SuspendLayout()
        CType(Me.dgvModulos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'HeaderStrip1
        '
        Me.HeaderStrip1.AutoSize = False
        Me.HeaderStrip1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.HeaderStrip1.ForeColor = System.Drawing.Color.White
        Me.HeaderStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.HeaderStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1})
        Me.HeaderStrip1.Location = New System.Drawing.Point(0, 0)
        Me.HeaderStrip1.Name = "HeaderStrip1"
        Me.HeaderStrip1.Size = New System.Drawing.Size(693, 25)
        Me.HeaderStrip1.TabIndex = 0
        Me.HeaderStrip1.Text = "HeaderStrip1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(220, 22)
        Me.ToolStripLabel1.Text = "Administraciòn de Usuarios"
        '
        'dgvModulos
        '
        Me.dgvModulos.AllowUserToAddRows = False
        Me.dgvModulos.AllowUserToDeleteRows = False
        Me.dgvModulos.AllowUserToResizeRows = False
        Me.dgvModulos.AutoGenerateColumns = False
        Me.dgvModulos.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvModulos.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvModulos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(49, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(197, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvModulos.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvModulos.ColumnHeadersHeight = 30
        Me.dgvModulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvModulos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.UsuarioDataGridViewTextBoxColumn, Me.NombreDataGridViewTextBoxColumn, Me.EmpresaDataGridViewTextBoxColumn, Me.ActivoDataGridViewCheckBoxColumn, Me.SuperUserDataGridViewCheckBoxColumn, Me.MultiEmpresaDataGridViewCheckBoxColumn, Me.FechaCreacionDataGridViewTextBoxColumn})
        Me.dgvModulos.DataSource = Me.bsUsuarios
        Me.dgvModulos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvModulos.EnableHeadersVisualStyles = False
        Me.dgvModulos.GridColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.dgvModulos.Location = New System.Drawing.Point(0, 50)
        Me.dgvModulos.Name = "dgvModulos"
        Me.dgvModulos.ReadOnly = True
        Me.dgvModulos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dgvModulos.RowHeadersVisible = False
        Me.dgvModulos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvModulos.Size = New System.Drawing.Size(693, 456)
        Me.dgvModulos.TabIndex = 7
        '
        'UsuarioDataGridViewTextBoxColumn
        '
        Me.UsuarioDataGridViewTextBoxColumn.DataPropertyName = "Usuario"
        Me.UsuarioDataGridViewTextBoxColumn.HeaderText = "Usuario"
        Me.UsuarioDataGridViewTextBoxColumn.Name = "UsuarioDataGridViewTextBoxColumn"
        Me.UsuarioDataGridViewTextBoxColumn.ReadOnly = True
        Me.UsuarioDataGridViewTextBoxColumn.Width = 80
        '
        'NombreDataGridViewTextBoxColumn
        '
        Me.NombreDataGridViewTextBoxColumn.DataPropertyName = "Nombre"
        Me.NombreDataGridViewTextBoxColumn.HeaderText = "Nombre"
        Me.NombreDataGridViewTextBoxColumn.Name = "NombreDataGridViewTextBoxColumn"
        Me.NombreDataGridViewTextBoxColumn.ReadOnly = True
        Me.NombreDataGridViewTextBoxColumn.Width = 200
        '
        'EmpresaDataGridViewTextBoxColumn
        '
        Me.EmpresaDataGridViewTextBoxColumn.DataPropertyName = "Empresa"
        Me.EmpresaDataGridViewTextBoxColumn.HeaderText = "Empresa"
        Me.EmpresaDataGridViewTextBoxColumn.Name = "EmpresaDataGridViewTextBoxColumn"
        Me.EmpresaDataGridViewTextBoxColumn.ReadOnly = True
        Me.EmpresaDataGridViewTextBoxColumn.Width = 150
        '
        'ActivoDataGridViewCheckBoxColumn
        '
        Me.ActivoDataGridViewCheckBoxColumn.DataPropertyName = "Activo"
        Me.ActivoDataGridViewCheckBoxColumn.HeaderText = "Activo"
        Me.ActivoDataGridViewCheckBoxColumn.Name = "ActivoDataGridViewCheckBoxColumn"
        Me.ActivoDataGridViewCheckBoxColumn.ReadOnly = True
        Me.ActivoDataGridViewCheckBoxColumn.Width = 44
        '
        'SuperUserDataGridViewCheckBoxColumn
        '
        Me.SuperUserDataGridViewCheckBoxColumn.DataPropertyName = "SuperUser"
        Me.SuperUserDataGridViewCheckBoxColumn.HeaderText = "Super User"
        Me.SuperUserDataGridViewCheckBoxColumn.Name = "SuperUserDataGridViewCheckBoxColumn"
        Me.SuperUserDataGridViewCheckBoxColumn.ReadOnly = True
        Me.SuperUserDataGridViewCheckBoxColumn.Width = 44
        '
        'MultiEmpresaDataGridViewCheckBoxColumn
        '
        Me.MultiEmpresaDataGridViewCheckBoxColumn.DataPropertyName = "MultiEmpresa"
        Me.MultiEmpresaDataGridViewCheckBoxColumn.HeaderText = "Multi Empresa"
        Me.MultiEmpresaDataGridViewCheckBoxColumn.Name = "MultiEmpresaDataGridViewCheckBoxColumn"
        Me.MultiEmpresaDataGridViewCheckBoxColumn.ReadOnly = True
        Me.MultiEmpresaDataGridViewCheckBoxColumn.Width = 54
        '
        'FechaCreacionDataGridViewTextBoxColumn
        '
        Me.FechaCreacionDataGridViewTextBoxColumn.DataPropertyName = "FechaCreacion"
        DataGridViewCellStyle2.Format = "dd/MM/yyyy"
        Me.FechaCreacionDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.FechaCreacionDataGridViewTextBoxColumn.HeaderText = "Fecha Creación"
        Me.FechaCreacionDataGridViewTextBoxColumn.Name = "FechaCreacionDataGridViewTextBoxColumn"
        Me.FechaCreacionDataGridViewTextBoxColumn.ReadOnly = True
        Me.FechaCreacionDataGridViewTextBoxColumn.Width = 70
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
        'ToolStrip2
        '
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarUsuario, Me.btnEditarUsuario, Me.btnEliminarUsuario, Me.ToolStripSeparator1, Me.btnCambiarClave, Me.btnPerfilesUsuario})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 25)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(693, 25)
        Me.ToolStrip2.TabIndex = 6
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'btnAgregarUsuario
        '
        Me.btnAgregarUsuario.Image = CType(resources.GetObject("btnAgregarUsuario.Image"), System.Drawing.Image)
        Me.btnAgregarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarUsuario.Name = "btnAgregarUsuario"
        Me.btnAgregarUsuario.Size = New System.Drawing.Size(66, 22)
        Me.btnAgregarUsuario.Text = "Agregar"
        '
        'btnEditarUsuario
        '
        Me.btnEditarUsuario.Image = Global.Desktop.Framework.My.Resources.Resources.EditInformation
        Me.btnEditarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarUsuario.Name = "btnEditarUsuario"
        Me.btnEditarUsuario.Size = New System.Drawing.Size(63, 22)
        Me.btnEditarUsuario.Text = "Editar.."
        '
        'btnEliminarUsuario
        '
        Me.btnEliminarUsuario.Image = CType(resources.GetObject("btnEliminarUsuario.Image"), System.Drawing.Image)
        Me.btnEliminarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarUsuario.Name = "btnEliminarUsuario"
        Me.btnEliminarUsuario.Size = New System.Drawing.Size(63, 22)
        Me.btnEliminarUsuario.Text = "Eliminar"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btnCambiarClave
        '
        Me.btnCambiarClave.Image = Global.Desktop.Framework.My.Resources.Resources.ico_16_securityrole
        Me.btnCambiarClave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCambiarClave.Name = "btnCambiarClave"
        Me.btnCambiarClave.Size = New System.Drawing.Size(108, 22)
        Me.btnCambiarClave.Text = "Cambiar Clave..."
        '
        'btnPerfilesUsuario
        '
        Me.btnPerfilesUsuario.Image = Global.Desktop.Framework.My.Resources.Resources.ico_16_4500
        Me.btnPerfilesUsuario.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPerfilesUsuario.Name = "btnPerfilesUsuario"
        Me.btnPerfilesUsuario.Size = New System.Drawing.Size(74, 22)
        Me.btnPerfilesUsuario.Text = "Perfiles..."
        '
        'UsuarioView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.dgvModulos)
        Me.Controls.Add(Me.ToolStrip2)
        Me.Controls.Add(Me.HeaderStrip1)
        Me.Name = "UsuarioView"
        Me.HeaderStrip1.ResumeLayout(False)
        Me.HeaderStrip1.PerformLayout()
        CType(Me.dgvModulos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HeaderStrip1 As Desktop.Windows.Forms.HeaderStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents dgvModulos As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregarUsuario As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarUsuario As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminarUsuario As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCambiarClave As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnPerfilesUsuario As System.Windows.Forms.ToolStripButton
    Friend WithEvents bsUsuarios As System.Windows.Forms.BindingSource
    Friend WithEvents dsSeguridad As DataSet
    Friend WithEvents UsuarioDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NombreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EmpresaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActivoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents SuperUserDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents MultiEmpresaDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents FechaCreacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
