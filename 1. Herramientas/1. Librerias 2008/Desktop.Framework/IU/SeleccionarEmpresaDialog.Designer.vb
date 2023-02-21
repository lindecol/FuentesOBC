<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SeleccionarEmpresaDialog
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.NombreLabel1 = New System.Windows.Forms.Label
        Me.dgvEmpresa = New System.Windows.Forms.DataGridView
        Me.NITDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NombreCompletoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.bsEmpresas = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsSesion = New Desktop.Framework.SesionDataset
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        NombreLabel = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        CType(Me.dgvEmpresa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsEmpresas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsSesion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NombreLabel
        '
        NombreLabel.AutoSize = True
        NombreLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        NombreLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        NombreLabel.Location = New System.Drawing.Point(12, 7)
        NombreLabel.Name = "NombreLabel"
        NombreLabel.Size = New System.Drawing.Size(117, 16)
        NombreLabel.TabIndex = 16
        NombreLabel.Text = "Empresa Actual"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(NombreLabel)
        Me.Panel1.Controls.Add(Me.NombreLabel1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(652, 52)
        Me.Panel1.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(12, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(333, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Seleccione la empresa con de la cual desea consultar la información:"
        '
        'NombreLabel1
        '
        Me.NombreLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NombreLabel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.NombreLabel1.Location = New System.Drawing.Point(81, 7)
        Me.NombreLabel1.Name = "NombreLabel1"
        Me.NombreLabel1.Size = New System.Drawing.Size(489, 23)
        Me.NombreLabel1.TabIndex = 17
        '
        'dgvEmpresa
        '
        Me.dgvEmpresa.AllowUserToAddRows = False
        Me.dgvEmpresa.AllowUserToDeleteRows = False
        Me.dgvEmpresa.AllowUserToOrderColumns = True
        Me.dgvEmpresa.AllowUserToResizeColumns = False
        Me.dgvEmpresa.AllowUserToResizeRows = False
        Me.dgvEmpresa.AutoGenerateColumns = False
        Me.dgvEmpresa.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvEmpresa.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.dgvEmpresa.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(235, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(49, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(197, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvEmpresa.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEmpresa.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NITDataGridViewTextBoxColumn, Me.NombreCompletoDataGridViewTextBoxColumn})
        Me.dgvEmpresa.DataSource = Me.bsEmpresas
        Me.dgvEmpresa.EnableHeadersVisualStyles = False
        Me.dgvEmpresa.GridColor = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.dgvEmpresa.Location = New System.Drawing.Point(12, 58)
        Me.dgvEmpresa.MultiSelect = False
        Me.dgvEmpresa.Name = "dgvEmpresa"
        Me.dgvEmpresa.ReadOnly = True
        Me.dgvEmpresa.RowHeadersVisible = False
        Me.dgvEmpresa.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEmpresa.Size = New System.Drawing.Size(623, 249)
        Me.dgvEmpresa.TabIndex = 22
        '
        'NITDataGridViewTextBoxColumn
        '
        Me.NITDataGridViewTextBoxColumn.DataPropertyName = "NIT"
        Me.NITDataGridViewTextBoxColumn.HeaderText = "NIT"
        Me.NITDataGridViewTextBoxColumn.Name = "NITDataGridViewTextBoxColumn"
        Me.NITDataGridViewTextBoxColumn.ReadOnly = True
        Me.NITDataGridViewTextBoxColumn.Width = 120
        '
        'NombreCompletoDataGridViewTextBoxColumn
        '
        Me.NombreCompletoDataGridViewTextBoxColumn.DataPropertyName = "NombreCompleto"
        Me.NombreCompletoDataGridViewTextBoxColumn.HeaderText = "Nombre Empresa"
        Me.NombreCompletoDataGridViewTextBoxColumn.Name = "NombreCompletoDataGridViewTextBoxColumn"
        Me.NombreCompletoDataGridViewTextBoxColumn.ReadOnly = True
        Me.NombreCompletoDataGridViewTextBoxColumn.Width = 500
        '
        'bsEmpresas
        '
        Me.bsEmpresas.DataMember = "Empresas"
        Me.bsEmpresas.DataSource = Me.dsSesion
        '
        'dsSesion
        '
        Me.dsSesion.DataSetName = "SesionDataset"
        Me.dsSesion.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(479, 313)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 23
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(560, 313)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 24
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'SeleccionarEmpresaDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(652, 350)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.dgvEmpresa)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "SeleccionarEmpresaDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Seleccionar Empresa"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvEmpresa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsEmpresas, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsSesion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NombreLabel1 As System.Windows.Forms.Label
    Friend WithEvents dgvEmpresa As System.Windows.Forms.DataGridView
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents bsEmpresas As System.Windows.Forms.BindingSource
    Friend WithEvents NITDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NombreCompletoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents dsSesion As Desktop.Framework.SesionDataset
End Class
