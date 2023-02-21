<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PerfilesUsuariosView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PerfilesUsuariosView))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.btnAgregar = New System.Windows.Forms.ToolStripButton
        Me.btnEditar = New System.Windows.Forms.ToolStripButton
        Me.btnEliminar = New System.Windows.Forms.ToolStripButton
        Me.HeaderStrip1 = New Desktop.Windows.Forms.HeaderStrip
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.dgvModulos = New System.Windows.Forms.DataGridView
        Me.NombreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ActivoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.SistemaDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.bsPerfiles = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsSeguridad = New Desktop.Framework.SeguridadDataset
        Me.ToolStrip2.SuspendLayout()
        Me.HeaderStrip1.SuspendLayout()
        CType(Me.dgvModulos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsPerfiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip2
        '
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregar, Me.btnEditar, Me.btnEliminar})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 25)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(693, 25)
        Me.ToolStrip2.TabIndex = 8
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'btnAgregar
        '
        Me.btnAgregar.Image = CType(resources.GetObject("btnAgregar.Image"), System.Drawing.Image)
        Me.btnAgregar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(66, 22)
        Me.btnAgregar.Text = "Agregar"
        '
        'btnEditar
        '
        Me.btnEditar.Image = Global.Desktop.Framework.My.Resources.EditInformation
        Me.btnEditar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(63, 22)
        Me.btnEditar.Text = "Editar.."
        '
        'btnEliminar
        '
        Me.btnEliminar.Image = CType(resources.GetObject("btnEliminar.Image"), System.Drawing.Image)
        Me.btnEliminar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(63, 22)
        Me.btnEliminar.Text = "Eliminar"
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
        Me.HeaderStrip1.TabIndex = 7
        Me.HeaderStrip1.Text = "HeaderStrip1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(304, 22)
        Me.ToolStripLabel1.Text = "Administración de Perfiles de Usuarios"
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
        Me.dgvModulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvModulos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NombreDataGridViewTextBoxColumn, Me.ActivoDataGridViewCheckBoxColumn, Me.SistemaDataGridViewCheckBoxColumn})
        Me.dgvModulos.DataSource = Me.bsPerfiles
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
        Me.dgvModulos.TabIndex = 9
        '
        'NombreDataGridViewTextBoxColumn
        '
        Me.NombreDataGridViewTextBoxColumn.DataPropertyName = "Nombre"
        Me.NombreDataGridViewTextBoxColumn.HeaderText = "Nombre"
        Me.NombreDataGridViewTextBoxColumn.Name = "NombreDataGridViewTextBoxColumn"
        Me.NombreDataGridViewTextBoxColumn.ReadOnly = True
        Me.NombreDataGridViewTextBoxColumn.Width = 400
        '
        'ActivoDataGridViewCheckBoxColumn
        '
        Me.ActivoDataGridViewCheckBoxColumn.DataPropertyName = "Activo"
        Me.ActivoDataGridViewCheckBoxColumn.HeaderText = "Activo"
        Me.ActivoDataGridViewCheckBoxColumn.Name = "ActivoDataGridViewCheckBoxColumn"
        Me.ActivoDataGridViewCheckBoxColumn.ReadOnly = True
        '
        'SistemaDataGridViewCheckBoxColumn
        '
        Me.SistemaDataGridViewCheckBoxColumn.DataPropertyName = "Sistema"
        Me.SistemaDataGridViewCheckBoxColumn.HeaderText = "Sistema"
        Me.SistemaDataGridViewCheckBoxColumn.Name = "SistemaDataGridViewCheckBoxColumn"
        Me.SistemaDataGridViewCheckBoxColumn.ReadOnly = True
        '
        'bsPerfiles
        '
        Me.bsPerfiles.DataMember = "Perfiles"
        Me.bsPerfiles.DataSource = Me.dsSeguridad
        '
        'dsSeguridad
        '
        Me.dsSeguridad.DataSetName = "SeguridadDataset"
        Me.dsSeguridad.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PerfilesUsuariosView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.dgvModulos)
        Me.Controls.Add(Me.ToolStrip2)
        Me.Controls.Add(Me.HeaderStrip1)
        Me.Name = "PerfilesUsuariosView"
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.HeaderStrip1.ResumeLayout(False)
        Me.HeaderStrip1.PerformLayout()
        CType(Me.dgvModulos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsPerfiles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminar As System.Windows.Forms.ToolStripButton
    Friend WithEvents HeaderStrip1 As Desktop.Windows.Forms.HeaderStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents dgvModulos As System.Windows.Forms.DataGridView
    Friend WithEvents NombreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActivoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents SistemaDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents bsPerfiles As System.Windows.Forms.BindingSource
    Friend WithEvents dsSeguridad As DataSet

End Class
