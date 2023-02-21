<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditarPerfilDialog
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.NombreTextBox = New System.Windows.Forms.TextBox
        Me.bsPerfiles = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsSeguridad = New Desktop.Framework.SeguridadDataset
        Me.dgvPermisosModulos = New System.Windows.Forms.DataGridView
        Me.bsPermisosModulo = New System.Windows.Forms.BindingSource(Me.components)
        Me.gbPermisos = New System.Windows.Forms.GroupBox
        Me.lstPermisosEspeciales = New System.Windows.Forms.CheckedListBox
        Me.Shape1 = New Desktop.Windows.Forms.Shape
        Me.Label1 = New System.Windows.Forms.Label
        Me.cbPrograma = New System.Windows.Forms.ComboBox
        Me.bsProgramas = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.ActivoCheckBox = New System.Windows.Forms.CheckBox
        Me.NombreDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NombreCategoria = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PermitirAccesoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.PermiteInsertar = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.PermitirModificarDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.PermitirBorrarDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.PermitirImprimirDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        NombreLabel = New System.Windows.Forms.Label
        CType(Me.bsPerfiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPermisosModulos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsPermisosModulo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPermisos.SuspendLayout()
        CType(Me.bsProgramas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'NombreLabel
        '
        NombreLabel.AutoSize = True
        NombreLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        NombreLabel.Location = New System.Drawing.Point(12, 26)
        NombreLabel.Name = "NombreLabel"
        NombreLabel.Size = New System.Drawing.Size(54, 13)
        NombreLabel.TabIndex = 1
        NombreLabel.Text = "Nombre:"
        '
        'NombreTextBox
        '
        Me.NombreTextBox.Location = New System.Drawing.Point(72, 23)
        Me.NombreTextBox.Name = "NombreTextBox"
        Me.NombreTextBox.Size = New System.Drawing.Size(410, 20)
        Me.NombreTextBox.TabIndex = 2
        '
        'bsPerfiles
        '
        Me.bsPerfiles.DataSource = Me.dsSeguridad
        '
        'dsSeguridad
        '
        Me.dsSeguridad.DataSetName = "SeguridadDataset"
        Me.dsSeguridad.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'dgvPermisosModulos
        '
        Me.dgvPermisosModulos.AllowUserToAddRows = False
        Me.dgvPermisosModulos.AllowUserToDeleteRows = False
        Me.dgvPermisosModulos.AllowUserToResizeColumns = False
        Me.dgvPermisosModulos.AllowUserToResizeRows = False
        Me.dgvPermisosModulos.AutoGenerateColumns = False
        Me.dgvPermisosModulos.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvPermisosModulos.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvPermisosModulos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(49, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(197, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPermisosModulos.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPermisosModulos.ColumnHeadersHeight = 32
        Me.dgvPermisosModulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvPermisosModulos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NombreDataGridViewTextBoxColumn, Me.NombreCategoria, Me.PermitirAccesoDataGridViewCheckBoxColumn, Me.PermiteInsertar, Me.PermitirModificarDataGridViewCheckBoxColumn, Me.PermitirBorrarDataGridViewCheckBoxColumn, Me.PermitirImprimirDataGridViewCheckBoxColumn})
        Me.dgvPermisosModulos.DataSource = Me.bsPermisosModulo
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(235, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(227, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPermisosModulos.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvPermisosModulos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgvPermisosModulos.EnableHeadersVisualStyles = False
        Me.dgvPermisosModulos.GridColor = System.Drawing.Color.FromArgb(CType(CType(122, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.dgvPermisosModulos.Location = New System.Drawing.Point(15, 45)
        Me.dgvPermisosModulos.MultiSelect = False
        Me.dgvPermisosModulos.Name = "dgvPermisosModulos"
        Me.dgvPermisosModulos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(49, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(197, Byte), Integer))
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPermisosModulos.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvPermisosModulos.RowHeadersVisible = False
        Me.dgvPermisosModulos.RowHeadersWidth = 20
        Me.dgvPermisosModulos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvPermisosModulos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPermisosModulos.Size = New System.Drawing.Size(745, 342)
        Me.dgvPermisosModulos.TabIndex = 10
        '
        'bsPermisosModulo
        '
        Me.bsPermisosModulo.DataSource = Me.dsSeguridad
        '
        'gbPermisos
        '
        Me.gbPermisos.Controls.Add(Me.lstPermisosEspeciales)
        Me.gbPermisos.Controls.Add(Me.Shape1)
        Me.gbPermisos.Controls.Add(Me.Label1)
        Me.gbPermisos.Controls.Add(Me.cbPrograma)
        Me.gbPermisos.Controls.Add(Me.dgvPermisosModulos)
        Me.gbPermisos.Location = New System.Drawing.Point(14, 73)
        Me.gbPermisos.Name = "gbPermisos"
        Me.gbPermisos.Padding = New System.Windows.Forms.Padding(5)
        Me.gbPermisos.Size = New System.Drawing.Size(768, 530)
        Me.gbPermisos.TabIndex = 11
        Me.gbPermisos.TabStop = False
        Me.gbPermisos.Text = "Control de Acceso y Seguridad"
        '
        'lstPermisosEspeciales
        '
        Me.lstPermisosEspeciales.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lstPermisosEspeciales.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstPermisosEspeciales.CheckOnClick = True
        Me.lstPermisosEspeciales.FormattingEnabled = True
        Me.lstPermisosEspeciales.Location = New System.Drawing.Point(15, 423)
        Me.lstPermisosEspeciales.Name = "lstPermisosEspeciales"
        Me.lstPermisosEspeciales.Size = New System.Drawing.Size(606, 90)
        Me.lstPermisosEspeciales.TabIndex = 14
        '
        'Shape1
        '
        Me.Shape1.Enabled = False
        Me.Shape1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Shape1.Location = New System.Drawing.Point(12, 402)
        Me.Shape1.Name = "Shape1"
        Me.Shape1.Size = New System.Drawing.Size(748, 23)
        Me.Shape1.Style = Desktop.Windows.Forms.Shape.ShapeStyle.GroupSeparator
        Me.Shape1.TabIndex = 13
        Me.Shape1.Text = "Permisos Especiales Módulo seleccionado:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Programa:"
        '
        'cbPrograma
        '
        Me.cbPrograma.DataSource = Me.bsProgramas
        Me.cbPrograma.DisplayMember = "Nombre"
        Me.cbPrograma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrograma.FormattingEnabled = True
        Me.cbPrograma.Location = New System.Drawing.Point(73, 18)
        Me.cbPrograma.Name = "cbPrograma"
        Me.cbPrograma.Size = New System.Drawing.Size(523, 21)
        Me.cbPrograma.TabIndex = 11
        Me.cbPrograma.ValueMember = "IdPrograma"
        '
        'bsProgramas
        '
        Me.bsProgramas.DataSource = Me.dsSeguridad
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(626, 609)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 12
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(707, 609)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 13
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ActivoCheckBox)
        Me.GroupBox2.Controls.Add(NombreLabel)
        Me.GroupBox2.Controls.Add(Me.NombreTextBox)
        Me.GroupBox2.Location = New System.Drawing.Point(14, 9)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(768, 60)
        Me.GroupBox2.TabIndex = 14
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Datos Básicos"
        '
        'ActivoCheckBox
        '
        Me.ActivoCheckBox.Location = New System.Drawing.Point(492, 21)
        Me.ActivoCheckBox.Name = "ActivoCheckBox"
        Me.ActivoCheckBox.Size = New System.Drawing.Size(58, 24)
        Me.ActivoCheckBox.TabIndex = 3
        Me.ActivoCheckBox.Text = "Activo"
        '
        'NombreDataGridViewTextBoxColumn
        '
        Me.NombreDataGridViewTextBoxColumn.DataPropertyName = "Nombre"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.NombreDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.NombreDataGridViewTextBoxColumn.HeaderText = "Nombre"
        Me.NombreDataGridViewTextBoxColumn.Name = "NombreDataGridViewTextBoxColumn"
        Me.NombreDataGridViewTextBoxColumn.ReadOnly = True
        Me.NombreDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.NombreDataGridViewTextBoxColumn.Width = 230
        '
        'NombreCategoria
        '
        Me.NombreCategoria.DataPropertyName = "NombreCategoria"
        Me.NombreCategoria.HeaderText = "Categoria"
        Me.NombreCategoria.Name = "NombreCategoria"
        Me.NombreCategoria.ReadOnly = True
        Me.NombreCategoria.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.NombreCategoria.Width = 150
        '
        'PermitirAccesoDataGridViewCheckBoxColumn
        '
        Me.PermitirAccesoDataGridViewCheckBoxColumn.DataPropertyName = "PermitirAcceso"
        Me.PermitirAccesoDataGridViewCheckBoxColumn.HeaderText = "Permitir Acceso"
        Me.PermitirAccesoDataGridViewCheckBoxColumn.Name = "PermitirAccesoDataGridViewCheckBoxColumn"
        Me.PermitirAccesoDataGridViewCheckBoxColumn.Width = 70
        '
        'PermiteInsertar
        '
        Me.PermiteInsertar.DataPropertyName = "PermiteInsertar"
        Me.PermiteInsertar.HeaderText = "Permite Insertar"
        Me.PermiteInsertar.Name = "PermiteInsertar"
        Me.PermiteInsertar.Width = 70
        '
        'PermitirModificarDataGridViewCheckBoxColumn
        '
        Me.PermitirModificarDataGridViewCheckBoxColumn.DataPropertyName = "PermitirModificar"
        Me.PermitirModificarDataGridViewCheckBoxColumn.HeaderText = "Permitir Modificar"
        Me.PermitirModificarDataGridViewCheckBoxColumn.Name = "PermitirModificarDataGridViewCheckBoxColumn"
        Me.PermitirModificarDataGridViewCheckBoxColumn.Width = 70
        '
        'PermitirBorrarDataGridViewCheckBoxColumn
        '
        Me.PermitirBorrarDataGridViewCheckBoxColumn.DataPropertyName = "PermitirBorrar"
        Me.PermitirBorrarDataGridViewCheckBoxColumn.HeaderText = "Permitir Borrar"
        Me.PermitirBorrarDataGridViewCheckBoxColumn.Name = "PermitirBorrarDataGridViewCheckBoxColumn"
        Me.PermitirBorrarDataGridViewCheckBoxColumn.Width = 70
        '
        'PermitirImprimirDataGridViewCheckBoxColumn
        '
        Me.PermitirImprimirDataGridViewCheckBoxColumn.DataPropertyName = "PermitirImprimir"
        Me.PermitirImprimirDataGridViewCheckBoxColumn.HeaderText = "Permitir Imprimir"
        Me.PermitirImprimirDataGridViewCheckBoxColumn.Name = "PermitirImprimirDataGridViewCheckBoxColumn"
        Me.PermitirImprimirDataGridViewCheckBoxColumn.Width = 70
        '
        'EditarPerfilDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(794, 646)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.gbPermisos)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "EditarPerfilDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Editar perfil usuario"
        CType(Me.bsPerfiles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsSeguridad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPermisosModulos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsPermisosModulo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPermisos.ResumeLayout(False)
        Me.gbPermisos.PerformLayout()
        CType(Me.bsProgramas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dsSeguridad As DataSet
    Friend WithEvents bsPerfiles As System.Windows.Forms.BindingSource
    Friend WithEvents NombreTextBox As System.Windows.Forms.TextBox
    Friend WithEvents bsPermisosModulo As System.Windows.Forms.BindingSource
    Friend WithEvents dgvPermisosModulos As System.Windows.Forms.DataGridView
    Friend WithEvents gbPermisos As System.Windows.Forms.GroupBox
    Friend WithEvents cbPrograma As System.Windows.Forms.ComboBox
    Friend WithEvents lstPermisosEspeciales As System.Windows.Forms.CheckedListBox
    Friend WithEvents Shape1 As Desktop.Windows.Forms.Shape
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents bsProgramas As System.Windows.Forms.BindingSource
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ActivoCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents NombreDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NombreCategoria As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PermitirAccesoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents PermiteInsertar As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents PermitirModificarDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents PermitirBorrarDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents PermitirImprimirDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
End Class
