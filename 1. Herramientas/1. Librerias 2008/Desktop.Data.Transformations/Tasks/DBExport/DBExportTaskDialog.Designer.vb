<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBExportTaskDialog
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
        Dim DestinoLabel As System.Windows.Forms.Label
        Dim OrigenLabel As System.Windows.Forms.Label
        Dim CadenaConexionLabel As System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tpGeneral = New System.Windows.Forms.TabPage
        Me.ComandoFinalizacionTextBox = New System.Windows.Forms.TextBox
        Me.bsGeneral = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsTask = New Desktop.Data.Transformations.DBExportTaskDataSet
        Me.Shape3 = New Desktop.Windows.Forms.Shape
        Me.cbTipoConexion = New System.Windows.Forms.ComboBox
        Me.btnVistaPrevia = New System.Windows.Forms.Button
        Me.CadenaConexionTextBox = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Shape2 = New Desktop.Windows.Forms.Shape
        Me.Shape1 = New Desktop.Windows.Forms.Shape
        Me.btnObtenerTablasDestino = New System.Windows.Forms.Button
        Me.cbTablaDestino = New System.Windows.Forms.ComboBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.IgnorarErroresCheckBox = New System.Windows.Forms.CheckBox
        Me.InsertNewRowsCheckBox = New System.Windows.Forms.CheckBox
        Me.UpdateExistingRowsCheckBox = New System.Windows.Forms.CheckBox
        Me.txtConsultaOrigen = New System.Windows.Forms.TextBox
        Me.tpMapeoCampos = New System.Windows.Forms.TabPage
        Me.btnBorrarMapeo = New System.Windows.Forms.Button
        Me.btnBorrarMapeos = New System.Windows.Forms.Button
        Me.gboxDefinicionMapeo = New System.Windows.Forms.GroupBox
        Me.cbCamposDestino = New System.Windows.Forms.ComboBox
        Me.bsMapeoCampos = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.btnGuardarMapeo = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cbCamposOrigen = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnNuevoMapeo = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.dgvMapeoCamposDestino = New System.Windows.Forms.DataGridView
        Me.CampoDestinoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CampoOrigenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ValorEstaticoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FormatoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ActualizarDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnAceptar = New System.Windows.Forms.Button
        DestinoLabel = New System.Windows.Forms.Label
        OrigenLabel = New System.Windows.Forms.Label
        CadenaConexionLabel = New System.Windows.Forms.Label
        Me.TabControl1.SuspendLayout()
        Me.tpGeneral.SuspendLayout()
        CType(Me.bsGeneral, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsTask, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.tpMapeoCampos.SuspendLayout()
        Me.gboxDefinicionMapeo.SuspendLayout()
        CType(Me.bsMapeoCampos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvMapeoCamposDestino, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DestinoLabel
        '
        DestinoLabel.AutoSize = True
        DestinoLabel.Location = New System.Drawing.Point(19, 239)
        DestinoLabel.Name = "DestinoLabel"
        DestinoLabel.Size = New System.Drawing.Size(76, 13)
        DestinoLabel.TabIndex = 4
        DestinoLabel.Text = "Tabla Destino:"
        '
        'OrigenLabel
        '
        OrigenLabel.AutoSize = True
        OrigenLabel.Location = New System.Drawing.Point(16, 21)
        OrigenLabel.Name = "OrigenLabel"
        OrigenLabel.Size = New System.Drawing.Size(136, 13)
        OrigenLabel.TabIndex = 1
        OrigenLabel.Text = "Consulta SQL datos origen:"
        '
        'CadenaConexionLabel
        '
        CadenaConexionLabel.AutoSize = True
        CadenaConexionLabel.Location = New System.Drawing.Point(19, 186)
        CadenaConexionLabel.Name = "CadenaConexionLabel"
        CadenaConexionLabel.Size = New System.Drawing.Size(94, 13)
        CadenaConexionLabel.TabIndex = 17
        CadenaConexionLabel.Text = "Cadena Conexion:"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpGeneral)
        Me.TabControl1.Controls.Add(Me.tpMapeoCampos)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(594, 422)
        Me.TabControl1.TabIndex = 1
        '
        'tpGeneral
        '
        Me.tpGeneral.AutoScroll = True
        Me.tpGeneral.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tpGeneral.Controls.Add(Me.ComandoFinalizacionTextBox)
        Me.tpGeneral.Controls.Add(Me.Shape3)
        Me.tpGeneral.Controls.Add(Me.cbTipoConexion)
        Me.tpGeneral.Controls.Add(Me.btnVistaPrevia)
        Me.tpGeneral.Controls.Add(Me.CadenaConexionTextBox)
        Me.tpGeneral.Controls.Add(CadenaConexionLabel)
        Me.tpGeneral.Controls.Add(Me.Label8)
        Me.tpGeneral.Controls.Add(Me.Shape2)
        Me.tpGeneral.Controls.Add(Me.Shape1)
        Me.tpGeneral.Controls.Add(Me.btnObtenerTablasDestino)
        Me.tpGeneral.Controls.Add(Me.cbTablaDestino)
        Me.tpGeneral.Controls.Add(DestinoLabel)
        Me.tpGeneral.Controls.Add(Me.GroupBox3)
        Me.tpGeneral.Controls.Add(OrigenLabel)
        Me.tpGeneral.Controls.Add(Me.txtConsultaOrigen)
        Me.tpGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tpGeneral.Name = "tpGeneral"
        Me.tpGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tpGeneral.Size = New System.Drawing.Size(586, 396)
        Me.tpGeneral.TabIndex = 0
        Me.tpGeneral.Text = "General"
        '
        'ComandoFinalizacionTextBox
        '
        Me.ComandoFinalizacionTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsGeneral, "ComandoFinalizacion", True))
        Me.ComandoFinalizacionTextBox.Location = New System.Drawing.Point(15, 369)
        Me.ComandoFinalizacionTextBox.Name = "ComandoFinalizacionTextBox"
        Me.ComandoFinalizacionTextBox.Size = New System.Drawing.Size(340, 20)
        Me.ComandoFinalizacionTextBox.TabIndex = 26
        '
        'bsGeneral
        '
        Me.bsGeneral.DataMember = "General"
        Me.bsGeneral.DataSource = Me.dsTask
        '
        'dsTask
        '
        Me.dsTask.DataSetName = "ODBCTaskDataSet"
        Me.dsTask.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Shape3
        '
        Me.Shape3.Enabled = False
        Me.Shape3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Shape3.Location = New System.Drawing.Point(8, 346)
        Me.Shape3.Name = "Shape3"
        Me.Shape3.Size = New System.Drawing.Size(561, 19)
        Me.Shape3.Style = Desktop.Windows.Forms.Shape.ShapeStyle.GroupSeparator
        Me.Shape3.TabIndex = 25
        Me.Shape3.Text = "Comando Finalización"
        '
        'cbTipoConexion
        '
        Me.cbTipoConexion.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsGeneral, "TipoConexion", True))
        Me.cbTipoConexion.FormattingEnabled = True
        Me.cbTipoConexion.Location = New System.Drawing.Point(390, 179)
        Me.cbTipoConexion.Name = "cbTipoConexion"
        Me.cbTipoConexion.Size = New System.Drawing.Size(164, 21)
        Me.cbTipoConexion.TabIndex = 19
        '
        'btnVistaPrevia
        '
        Me.btnVistaPrevia.Location = New System.Drawing.Point(390, 205)
        Me.btnVistaPrevia.Name = "btnVistaPrevia"
        Me.btnVistaPrevia.Size = New System.Drawing.Size(164, 23)
        Me.btnVistaPrevia.TabIndex = 20
        Me.btnVistaPrevia.Text = "Vista previa de datos..."
        Me.btnVistaPrevia.UseVisualStyleBackColor = True
        '
        'CadenaConexionTextBox
        '
        Me.CadenaConexionTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsGeneral, "CadenaConexion", True))
        Me.CadenaConexionTextBox.Location = New System.Drawing.Point(22, 179)
        Me.CadenaConexionTextBox.Multiline = True
        Me.CadenaConexionTextBox.Name = "CadenaConexionTextBox"
        Me.CadenaConexionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.CadenaConexionTextBox.Size = New System.Drawing.Size(361, 49)
        Me.CadenaConexionTextBox.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(387, 163)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(78, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Tipo Conexión:"
        '
        'Shape2
        '
        Me.Shape2.Enabled = False
        Me.Shape2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Shape2.Location = New System.Drawing.Point(8, 154)
        Me.Shape2.Name = "Shape2"
        Me.Shape2.Size = New System.Drawing.Size(561, 15)
        Me.Shape2.Style = Desktop.Windows.Forms.Shape.ShapeStyle.GroupSeparator
        Me.Shape2.TabIndex = 2
        Me.Shape2.Text = " Datos de destino     "
        '
        'Shape1
        '
        Me.Shape1.Enabled = False
        Me.Shape1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Shape1.Location = New System.Drawing.Point(6, 3)
        Me.Shape1.Name = "Shape1"
        Me.Shape1.Size = New System.Drawing.Size(563, 19)
        Me.Shape1.Style = Desktop.Windows.Forms.Shape.ShapeStyle.GroupSeparator
        Me.Shape1.TabIndex = 0
        Me.Shape1.Text = "Datos de Origen      "
        '
        'btnObtenerTablasDestino
        '
        Me.btnObtenerTablasDestino.Location = New System.Drawing.Point(465, 234)
        Me.btnObtenerTablasDestino.Name = "btnObtenerTablasDestino"
        Me.btnObtenerTablasDestino.Size = New System.Drawing.Size(95, 23)
        Me.btnObtenerTablasDestino.TabIndex = 8
        Me.btnObtenerTablasDestino.Text = "ObtenerTablas"
        Me.btnObtenerTablasDestino.UseVisualStyleBackColor = True
        '
        'cbTablaDestino
        '
        Me.cbTablaDestino.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsGeneral, "TablaDestino", True))
        Me.cbTablaDestino.FormattingEnabled = True
        Me.cbTablaDestino.Location = New System.Drawing.Point(98, 236)
        Me.cbTablaDestino.Name = "cbTablaDestino"
        Me.cbTablaDestino.Size = New System.Drawing.Size(361, 21)
        Me.cbTablaDestino.TabIndex = 7
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.IgnorarErroresCheckBox)
        Me.GroupBox3.Controls.Add(Me.InsertNewRowsCheckBox)
        Me.GroupBox3.Controls.Add(Me.UpdateExistingRowsCheckBox)
        Me.GroupBox3.Location = New System.Drawing.Point(22, 265)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(545, 75)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Opciones"
        '
        'IgnorarErroresCheckBox
        '
        Me.IgnorarErroresCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.bsGeneral, "IgnorarErrores", True))
        Me.IgnorarErroresCheckBox.Location = New System.Drawing.Point(15, 48)
        Me.IgnorarErroresCheckBox.Name = "IgnorarErroresCheckBox"
        Me.IgnorarErroresCheckBox.Size = New System.Drawing.Size(256, 24)
        Me.IgnorarErroresCheckBox.TabIndex = 2
        Me.IgnorarErroresCheckBox.Text = "Ignorar errores en filas"
        '
        'InsertNewRowsCheckBox
        '
        Me.InsertNewRowsCheckBox.AutoSize = True
        Me.InsertNewRowsCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.bsGeneral, "InsertarFilasNuevas", True))
        Me.InsertNewRowsCheckBox.Location = New System.Drawing.Point(288, 25)
        Me.InsertNewRowsCheckBox.Name = "InsertNewRowsCheckBox"
        Me.InsertNewRowsCheckBox.Size = New System.Drawing.Size(219, 17)
        Me.InsertNewRowsCheckBox.TabIndex = 1
        Me.InsertNewRowsCheckBox.Text = "Insertar nuevos registros en tabla destino"
        '
        'UpdateExistingRowsCheckBox
        '
        Me.UpdateExistingRowsCheckBox.AutoSize = True
        Me.UpdateExistingRowsCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.bsGeneral, "ActualizarFilasActuales", True))
        Me.UpdateExistingRowsCheckBox.Location = New System.Drawing.Point(15, 25)
        Me.UpdateExistingRowsCheckBox.Name = "UpdateExistingRowsCheckBox"
        Me.UpdateExistingRowsCheckBox.Size = New System.Drawing.Size(242, 17)
        Me.UpdateExistingRowsCheckBox.TabIndex = 0
        Me.UpdateExistingRowsCheckBox.Text = "Actualizar registros existentes en tabla destino"
        '
        'txtConsultaOrigen
        '
        Me.txtConsultaOrigen.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsGeneral, "ConsultaOrigen", True))
        Me.txtConsultaOrigen.Location = New System.Drawing.Point(19, 37)
        Me.txtConsultaOrigen.Multiline = True
        Me.txtConsultaOrigen.Name = "txtConsultaOrigen"
        Me.txtConsultaOrigen.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtConsultaOrigen.Size = New System.Drawing.Size(532, 114)
        Me.txtConsultaOrigen.TabIndex = 6
        '
        'tpMapeoCampos
        '
        Me.tpMapeoCampos.AutoScroll = True
        Me.tpMapeoCampos.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tpMapeoCampos.Controls.Add(Me.btnBorrarMapeo)
        Me.tpMapeoCampos.Controls.Add(Me.btnBorrarMapeos)
        Me.tpMapeoCampos.Controls.Add(Me.gboxDefinicionMapeo)
        Me.tpMapeoCampos.Controls.Add(Me.btnNuevoMapeo)
        Me.tpMapeoCampos.Controls.Add(Me.GroupBox1)
        Me.tpMapeoCampos.Controls.Add(Me.dgvMapeoCamposDestino)
        Me.tpMapeoCampos.Location = New System.Drawing.Point(4, 22)
        Me.tpMapeoCampos.Name = "tpMapeoCampos"
        Me.tpMapeoCampos.Padding = New System.Windows.Forms.Padding(3)
        Me.tpMapeoCampos.Size = New System.Drawing.Size(586, 396)
        Me.tpMapeoCampos.TabIndex = 1
        Me.tpMapeoCampos.Text = "Mapeo Campos"
        '
        'btnBorrarMapeo
        '
        Me.btnBorrarMapeo.Location = New System.Drawing.Point(347, 313)
        Me.btnBorrarMapeo.Name = "btnBorrarMapeo"
        Me.btnBorrarMapeo.Size = New System.Drawing.Size(168, 23)
        Me.btnBorrarMapeo.TabIndex = 2
        Me.btnBorrarMapeo.Text = "Borrar mapeo seleccionado"
        Me.btnBorrarMapeo.UseVisualStyleBackColor = True
        '
        'btnBorrarMapeos
        '
        Me.btnBorrarMapeos.Location = New System.Drawing.Point(347, 342)
        Me.btnBorrarMapeos.Name = "btnBorrarMapeos"
        Me.btnBorrarMapeos.Size = New System.Drawing.Size(168, 23)
        Me.btnBorrarMapeos.TabIndex = 3
        Me.btnBorrarMapeos.Text = "Borrar todos los mapeos"
        Me.btnBorrarMapeos.UseVisualStyleBackColor = True
        '
        'gboxDefinicionMapeo
        '
        Me.gboxDefinicionMapeo.Controls.Add(Me.cbCamposDestino)
        Me.gboxDefinicionMapeo.Controls.Add(Me.Label1)
        Me.gboxDefinicionMapeo.Controls.Add(Me.TextBox2)
        Me.gboxDefinicionMapeo.Controls.Add(Me.btnGuardarMapeo)
        Me.gboxDefinicionMapeo.Controls.Add(Me.TextBox1)
        Me.gboxDefinicionMapeo.Controls.Add(Me.Label4)
        Me.gboxDefinicionMapeo.Controls.Add(Me.Label3)
        Me.gboxDefinicionMapeo.Controls.Add(Me.cbCamposOrigen)
        Me.gboxDefinicionMapeo.Controls.Add(Me.Label2)
        Me.gboxDefinicionMapeo.Location = New System.Drawing.Point(6, 266)
        Me.gboxDefinicionMapeo.Name = "gboxDefinicionMapeo"
        Me.gboxDefinicionMapeo.Size = New System.Drawing.Size(335, 123)
        Me.gboxDefinicionMapeo.TabIndex = 2
        Me.gboxDefinicionMapeo.TabStop = False
        Me.gboxDefinicionMapeo.Text = "Definición mapeo campos"
        '
        'cbCamposDestino
        '
        Me.cbCamposDestino.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsMapeoCampos, "CampoDestino", True))
        Me.cbCamposDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCamposDestino.FormattingEnabled = True
        Me.cbCamposDestino.Location = New System.Drawing.Point(87, 20)
        Me.cbCamposDestino.Name = "cbCamposDestino"
        Me.cbCamposDestino.Size = New System.Drawing.Size(233, 21)
        Me.cbCamposDestino.TabIndex = 9
        '
        'bsMapeoCampos
        '
        Me.bsMapeoCampos.DataMember = "MapeoCampos"
        Me.bsMapeoCampos.DataSource = Me.dsTask
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Campo Destino:"
        '
        'TextBox2
        '
        Me.TextBox2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsMapeoCampos, "Formato", True))
        Me.TextBox2.Location = New System.Drawing.Point(87, 94)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(156, 20)
        Me.TextBox2.TabIndex = 3
        '
        'btnGuardarMapeo
        '
        Me.btnGuardarMapeo.Location = New System.Drawing.Point(249, 93)
        Me.btnGuardarMapeo.Name = "btnGuardarMapeo"
        Me.btnGuardarMapeo.Size = New System.Drawing.Size(71, 23)
        Me.btnGuardarMapeo.TabIndex = 4
        Me.btnGuardarMapeo.Text = "Guardar"
        Me.btnGuardarMapeo.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsMapeoCampos, "ValorEstatico", True))
        Me.TextBox1.Location = New System.Drawing.Point(87, 70)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(233, 20)
        Me.TextBox1.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Formato:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Valor Estático:"
        '
        'cbCamposOrigen
        '
        Me.cbCamposOrigen.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsMapeoCampos, "CampoOrigen", True))
        Me.cbCamposOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCamposOrigen.FormattingEnabled = True
        Me.cbCamposOrigen.Location = New System.Drawing.Point(87, 45)
        Me.cbCamposOrigen.Name = "cbCamposOrigen"
        Me.cbCamposOrigen.Size = New System.Drawing.Size(233, 21)
        Me.cbCamposOrigen.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Campo Origen:"
        '
        'btnNuevoMapeo
        '
        Me.btnNuevoMapeo.Location = New System.Drawing.Point(347, 284)
        Me.btnNuevoMapeo.Name = "btnNuevoMapeo"
        Me.btnNuevoMapeo.Size = New System.Drawing.Size(168, 23)
        Me.btnNuevoMapeo.TabIndex = 1
        Me.btnNuevoMapeo.Text = "Nuevo Mapeo"
        Me.btnNuevoMapeo.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(144, 310)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(8, 8)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'dgvMapeoCamposDestino
        '
        Me.dgvMapeoCamposDestino.AllowUserToAddRows = False
        Me.dgvMapeoCamposDestino.AutoGenerateColumns = False
        Me.dgvMapeoCamposDestino.BackgroundColor = System.Drawing.SystemColors.Window
        Me.dgvMapeoCamposDestino.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dgvMapeoCamposDestino.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CampoDestinoDataGridViewTextBoxColumn, Me.CampoOrigenDataGridViewTextBoxColumn, Me.ValorEstaticoDataGridViewTextBoxColumn, Me.FormatoDataGridViewTextBoxColumn, Me.ActualizarDataGridViewCheckBoxColumn})
        Me.dgvMapeoCamposDestino.DataSource = Me.bsMapeoCampos
        Me.dgvMapeoCamposDestino.EnableHeadersVisualStyles = False
        Me.dgvMapeoCamposDestino.Location = New System.Drawing.Point(6, 6)
        Me.dgvMapeoCamposDestino.MultiSelect = False
        Me.dgvMapeoCamposDestino.Name = "dgvMapeoCamposDestino"
        Me.dgvMapeoCamposDestino.ReadOnly = True
        Me.dgvMapeoCamposDestino.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dgvMapeoCamposDestino.RowHeadersWidth = 20
        Me.dgvMapeoCamposDestino.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMapeoCamposDestino.Size = New System.Drawing.Size(570, 257)
        Me.dgvMapeoCamposDestino.TabIndex = 0
        '
        'CampoDestinoDataGridViewTextBoxColumn
        '
        Me.CampoDestinoDataGridViewTextBoxColumn.DataPropertyName = "CampoDestino"
        Me.CampoDestinoDataGridViewTextBoxColumn.HeaderText = "CampoDestino"
        Me.CampoDestinoDataGridViewTextBoxColumn.Name = "CampoDestinoDataGridViewTextBoxColumn"
        Me.CampoDestinoDataGridViewTextBoxColumn.ReadOnly = True
        '
        'CampoOrigenDataGridViewTextBoxColumn
        '
        Me.CampoOrigenDataGridViewTextBoxColumn.DataPropertyName = "CampoOrigen"
        Me.CampoOrigenDataGridViewTextBoxColumn.HeaderText = "CampoOrigen"
        Me.CampoOrigenDataGridViewTextBoxColumn.Name = "CampoOrigenDataGridViewTextBoxColumn"
        Me.CampoOrigenDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ValorEstaticoDataGridViewTextBoxColumn
        '
        Me.ValorEstaticoDataGridViewTextBoxColumn.DataPropertyName = "ValorEstatico"
        Me.ValorEstaticoDataGridViewTextBoxColumn.HeaderText = "ValorEstatico"
        Me.ValorEstaticoDataGridViewTextBoxColumn.Name = "ValorEstaticoDataGridViewTextBoxColumn"
        Me.ValorEstaticoDataGridViewTextBoxColumn.ReadOnly = True
        '
        'FormatoDataGridViewTextBoxColumn
        '
        Me.FormatoDataGridViewTextBoxColumn.DataPropertyName = "Formato"
        Me.FormatoDataGridViewTextBoxColumn.HeaderText = "Formato"
        Me.FormatoDataGridViewTextBoxColumn.Name = "FormatoDataGridViewTextBoxColumn"
        Me.FormatoDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ActualizarDataGridViewCheckBoxColumn
        '
        Me.ActualizarDataGridViewCheckBoxColumn.DataPropertyName = "Actualizar"
        Me.ActualizarDataGridViewCheckBoxColumn.HeaderText = "Actualizar"
        Me.ActualizarDataGridViewCheckBoxColumn.Name = "ActualizarDataGridViewCheckBoxColumn"
        Me.ActualizarDataGridViewCheckBoxColumn.ReadOnly = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(527, 436)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 18
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(446, 436)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 17
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'DBExportTaskDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.ClientSize = New System.Drawing.Size(617, 464)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "DBExportTaskDialog"
        Me.Text = "Exportación base de datos"
        Me.TabControl1.ResumeLayout(False)
        Me.tpGeneral.ResumeLayout(False)
        Me.tpGeneral.PerformLayout()
        CType(Me.bsGeneral, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsTask, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.tpMapeoCampos.ResumeLayout(False)
        Me.gboxDefinicionMapeo.ResumeLayout(False)
        Me.gboxDefinicionMapeo.PerformLayout()
        CType(Me.bsMapeoCampos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvMapeoCamposDestino, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tpGeneral As System.Windows.Forms.TabPage
    Friend WithEvents Shape2 As Desktop.Windows.Forms.Shape
    Friend WithEvents Shape1 As Desktop.Windows.Forms.Shape
    Friend WithEvents btnObtenerTablasDestino As System.Windows.Forms.Button
    Friend WithEvents cbTablaDestino As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents IgnorarErroresCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents InsertNewRowsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents UpdateExistingRowsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents txtConsultaOrigen As System.Windows.Forms.TextBox
    Friend WithEvents tpMapeoCampos As System.Windows.Forms.TabPage
    Friend WithEvents btnBorrarMapeo As System.Windows.Forms.Button
    Friend WithEvents btnBorrarMapeos As System.Windows.Forms.Button
    Friend WithEvents gboxDefinicionMapeo As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents btnGuardarMapeo As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbCamposOrigen As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnNuevoMapeo As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents dgvMapeoCamposDestino As System.Windows.Forms.DataGridView
    Friend WithEvents dsTask As Desktop.Data.Transformations.DBExportTaskDataSet
    Friend WithEvents bsGeneral As System.Windows.Forms.BindingSource
    Friend WithEvents bsMapeoCampos As System.Windows.Forms.BindingSource
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents CampoDestinoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CampoOrigenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ValorEstaticoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FormatoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActualizarDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents cbCamposDestino As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbTipoConexion As System.Windows.Forms.ComboBox
    Friend WithEvents btnVistaPrevia As System.Windows.Forms.Button
    Friend WithEvents CadenaConexionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComandoFinalizacionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Shape3 As Desktop.Windows.Forms.Shape
End Class
