<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExcuteProcessSQLDialog
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
        Dim lblTitulo As System.Windows.Forms.Label
        Dim CadenaConexionLabel As System.Windows.Forms.Label
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.dsExecuteProcess = New Desktop.Data.Transformations.ExecuteProcessDataSet
        Me.cmbTipoProceso = New System.Windows.Forms.ComboBox
        Me.bsEjecutarTarea = New System.Windows.Forms.BindingSource(Me.components)
        Me.CadenaConexionTextBox = New System.Windows.Forms.TextBox
        Me.txtConsultaOrigen = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbTipoConexion = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        lblTitulo = New System.Windows.Forms.Label
        CadenaConexionLabel = New System.Windows.Forms.Label
        CType(Me.dsExecuteProcess, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsEjecutarTarea, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        lblTitulo.AutoSize = True
        lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblTitulo.Location = New System.Drawing.Point(9, 39)
        lblTitulo.Name = "lblTitulo"
        lblTitulo.Size = New System.Drawing.Size(82, 13)
        lblTitulo.TabIndex = 28
        lblTitulo.Text = "Sentencia SQL:"
        '
        'CadenaConexionLabel
        '
        CadenaConexionLabel.AutoSize = True
        CadenaConexionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CadenaConexionLabel.Location = New System.Drawing.Point(389, 12)
        CadenaConexionLabel.Name = "CadenaConexionLabel"
        CadenaConexionLabel.Size = New System.Drawing.Size(94, 13)
        CadenaConexionLabel.TabIndex = 33
        CadenaConexionLabel.Text = "Cadena Conexión:"
        CadenaConexionLabel.Visible = False
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(324, 232)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 32
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(243, 232)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 31
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'dsExecuteProcess
        '
        Me.dsExecuteProcess.DataSetName = "ExecuteProcessDataSet"
        Me.dsExecuteProcess.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cmbTipoProceso
        '
        Me.cmbTipoProceso.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEjecutarTarea, "TipoSentencia", True))
        Me.cmbTipoProceso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoProceso.FormattingEnabled = True
        Me.cmbTipoProceso.Items.AddRange(New Object() {"Sentencia SQL Texto", "Procedimiento Almacenado"})
        Me.cmbTipoProceso.Location = New System.Drawing.Point(104, 9)
        Me.cmbTipoProceso.Name = "cmbTipoProceso"
        Me.cmbTipoProceso.Size = New System.Drawing.Size(295, 21)
        Me.cmbTipoProceso.TabIndex = 35
        '
        'bsEjecutarTarea
        '
        Me.bsEjecutarTarea.DataMember = "General"
        Me.bsEjecutarTarea.DataSource = Me.dsExecuteProcess
        '
        'CadenaConexionTextBox
        '
        Me.CadenaConexionTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEjecutarTarea, "CadenaConexion", True))
        Me.CadenaConexionTextBox.Location = New System.Drawing.Point(403, 38)
        Me.CadenaConexionTextBox.Multiline = True
        Me.CadenaConexionTextBox.Name = "CadenaConexionTextBox"
        Me.CadenaConexionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.CadenaConexionTextBox.Size = New System.Drawing.Size(387, 59)
        Me.CadenaConexionTextBox.TabIndex = 34
        Me.CadenaConexionTextBox.Visible = False
        '
        'txtConsultaOrigen
        '
        Me.txtConsultaOrigen.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEjecutarTarea, "SentenciaSQL", True))
        Me.txtConsultaOrigen.Location = New System.Drawing.Point(12, 58)
        Me.txtConsultaOrigen.Multiline = True
        Me.txtConsultaOrigen.Name = "txtConsultaOrigen"
        Me.txtConsultaOrigen.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtConsultaOrigen.Size = New System.Drawing.Size(387, 168)
        Me.txtConsultaOrigen.TabIndex = 36
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 13)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Tipo Proceso:"
        '
        'cmbTipoConexion
        '
        Me.cmbTipoConexion.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEjecutarTarea, "TipoConexion", True))
        Me.cmbTipoConexion.FormattingEnabled = True
        Me.cmbTipoConexion.Items.AddRange(New Object() {"Sentencia SQL Texto", "Procedimiento Almacenado"})
        Me.cmbTipoConexion.Location = New System.Drawing.Point(403, 12)
        Me.cmbTipoConexion.Name = "cmbTipoConexion"
        Me.cmbTipoConexion.Size = New System.Drawing.Size(295, 21)
        Me.cmbTipoConexion.TabIndex = 38
        Me.cmbTipoConexion.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(389, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 13)
        Me.Label1.TabIndex = 39
        Me.Label1.Text = "Tipo Conexión:"
        Me.Label1.Visible = False
        '
        'ExcuteProcessSQLDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.ClientSize = New System.Drawing.Size(409, 266)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmbTipoConexion)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbTipoProceso)
        Me.Controls.Add(Me.CadenaConexionTextBox)
        Me.Controls.Add(lblTitulo)
        Me.Controls.Add(Me.txtConsultaOrigen)
        Me.Controls.Add(CadenaConexionLabel)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExcuteProcessSQLDialog"
        Me.Text = "Configuración Tarea Ejecutar Proceso"
        CType(Me.dsExecuteProcess, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsEjecutarTarea, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents dsExecuteProcess As Desktop.Data.Transformations.ExecuteProcessDataSet
    Friend WithEvents cmbTipoProceso As System.Windows.Forms.ComboBox
    Friend WithEvents CadenaConexionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents txtConsultaOrigen As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents bsEjecutarTarea As System.Windows.Forms.BindingSource
    Friend WithEvents cmbTipoConexion As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
