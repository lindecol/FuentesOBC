<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DefinirFiltrosDialog
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DefinirFiltrosDialog))
        Me.dgvFiltros = New System.Windows.Forms.DataGridView
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.CampoColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Comparacion = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Valor = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.bsFiltros = New System.Windows.Forms.BindingSource(Me.components)
        Me.tbtnBorrarFiltros = New System.Windows.Forms.ToolStripButton
        Me.tbtnAceptar = New System.Windows.Forms.ToolStripButton
        Me.tbtnCancelar = New System.Windows.Forms.ToolStripButton
        CType(Me.dgvFiltros, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.bsFiltros, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvFiltros
        '
        Me.dgvFiltros.AutoGenerateColumns = False
        Me.dgvFiltros.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvFiltros.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvFiltros.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dgvFiltros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFiltros.ColumnHeadersVisible = False
        Me.dgvFiltros.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CampoColumn, Me.Comparacion, Me.Valor})
        Me.dgvFiltros.DataSource = Me.bsFiltros
        Me.dgvFiltros.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgvFiltros.EnableHeadersVisualStyles = False
        Me.dgvFiltros.GridColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.dgvFiltros.Location = New System.Drawing.Point(0, 24)
        Me.dgvFiltros.Name = "dgvFiltros"
        Me.dgvFiltros.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSteelBlue
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(49, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(197, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFiltros.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvFiltros.RowHeadersWidth = 21
        Me.dgvFiltros.Size = New System.Drawing.Size(539, 167)
        Me.dgvFiltros.TabIndex = 3
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbtnBorrarFiltros, Me.tbtnAceptar, Me.tbtnCancelar})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(539, 25)
        Me.ToolStrip1.TabIndex = 5
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'CampoColumn
        '
        Me.CampoColumn.DataPropertyName = "Campo"
        Me.CampoColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CampoColumn.HeaderText = "Campo"
        Me.CampoColumn.Name = "CampoColumn"
        Me.CampoColumn.Width = 200
        '
        'Comparacion
        '
        Me.Comparacion.DataPropertyName = "Comparacion"
        Me.Comparacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Comparacion.HeaderText = "Comparacion"
        Me.Comparacion.Items.AddRange(New Object() {"=", ">", ">=", "<", "<=", "Like"})
        Me.Comparacion.Name = "Comparacion"
        Me.Comparacion.Width = 60
        '
        'Valor
        '
        Me.Valor.DataPropertyName = "Valor"
        Me.Valor.HeaderText = "Valor"
        Me.Valor.Name = "Valor"
        Me.Valor.Width = 250
        '
        'tbtnBorrarFiltros
        '
        Me.tbtnBorrarFiltros.Image = Global.Desktop.Framework.My.Resources.Resources.Delete
        Me.tbtnBorrarFiltros.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbtnBorrarFiltros.Name = "tbtnBorrarFiltros"
        Me.tbtnBorrarFiltros.Size = New System.Drawing.Size(89, 22)
        Me.tbtnBorrarFiltros.Text = "Borrar Filtros"
        '
        'tbtnAceptar
        '
        Me.tbtnAceptar.Image = Global.Desktop.Framework.My.Resources.Resources.OK
        Me.tbtnAceptar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbtnAceptar.Name = "tbtnAceptar"
        Me.tbtnAceptar.Size = New System.Drawing.Size(65, 22)
        Me.tbtnAceptar.Text = "Aceptar"
        '
        'tbtnCancelar
        '
        Me.tbtnCancelar.Image = CType(resources.GetObject("tbtnCancelar.Image"), System.Drawing.Image)
        Me.tbtnCancelar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbtnCancelar.Name = "tbtnCancelar"
        Me.tbtnCancelar.Size = New System.Drawing.Size(69, 22)
        Me.tbtnCancelar.Text = "Cancelar"
        '
        'DefinirFiltrosDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(539, 192)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvFiltros)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "DefinirFiltrosDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Definir Filtros"
        CType(Me.dgvFiltros, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.bsFiltros, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvFiltros As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tbtnBorrarFiltros As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbtnAceptar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tbtnCancelar As System.Windows.Forms.ToolStripButton
    Friend WithEvents CampoColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Comparacion As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Valor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bsFiltros As System.Windows.Forms.BindingSource
End Class
