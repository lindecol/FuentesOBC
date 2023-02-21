<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomTaskDialog
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
        Dim DestinoLabel As System.Windows.Forms.Label
        Dim Label1 As System.Windows.Forms.Label
        Dim lblTitulo As System.Windows.Forms.Label
        Dim Label3 As System.Windows.Forms.Label
        Me.txtEnsamblado = New System.Windows.Forms.TextBox
        Me.bsEnsamblado = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsCustomImporTask = New Desktop.Data.Transformations.CustomImporTaskDataSet
        Me.cmbClase = New System.Windows.Forms.ComboBox
        Me.bsClase = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnBuscar = New System.Windows.Forms.Button
        Me.pnInformacionEnsamblado = New System.Windows.Forms.Panel
        Me.cmbMetodos = New System.Windows.Forms.ComboBox
        Me.bsMetodo = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnCargarEnsamblado = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.opEnsamblado = New System.Windows.Forms.OpenFileDialog
        DestinoLabel = New System.Windows.Forms.Label
        Label1 = New System.Windows.Forms.Label
        lblTitulo = New System.Windows.Forms.Label
        Label3 = New System.Windows.Forms.Label
        CType(Me.bsEnsamblado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsCustomImporTask, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsClase, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnInformacionEnsamblado.SuspendLayout()
        CType(Me.bsMetodo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DestinoLabel
        '
        DestinoLabel.AutoSize = True
        DestinoLabel.Location = New System.Drawing.Point(9, 9)
        DestinoLabel.Name = "DestinoLabel"
        DestinoLabel.Size = New System.Drawing.Size(119, 13)
        DestinoLabel.TabIndex = 25
        DestinoLabel.Text = "Ubicacion Ensamblado:"
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(16, 28)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(36, 13)
        Label1.TabIndex = 27
        Label1.Text = "Clase:"
        '
        'lblTitulo
        '
        lblTitulo.AutoSize = True
        lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblTitulo.Location = New System.Drawing.Point(7, 8)
        lblTitulo.Name = "lblTitulo"
        lblTitulo.Size = New System.Drawing.Size(149, 13)
        lblTitulo.TabIndex = 28
        lblTitulo.Text = "Información Ensamblado:"
        '
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(16, 55)
        Label3.Name = "Label3"
        Label3.Size = New System.Drawing.Size(46, 13)
        Label3.TabIndex = 30
        Label3.Text = "Metodo:"
        '
        'txtEnsamblado
        '
        Me.txtEnsamblado.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEnsamblado, "UbicacionEnsamblado", True))
        Me.txtEnsamblado.Location = New System.Drawing.Point(12, 25)
        Me.txtEnsamblado.Name = "txtEnsamblado"
        Me.txtEnsamblado.Size = New System.Drawing.Size(361, 20)
        Me.txtEnsamblado.TabIndex = 27
        '
        'bsEnsamblado
        '
        Me.bsEnsamblado.DataMember = "General"
        Me.bsEnsamblado.DataSource = Me.dsCustomImporTask
        '
        'dsCustomImporTask
        '
        Me.dsCustomImporTask.DataSetName = "CustomImporTaskDataSet"
        Me.dsCustomImporTask.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cmbClase
        '
        Me.cmbClase.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.bsEnsamblado, "Clase", True))
        Me.cmbClase.DataSource = Me.bsClase
        Me.cmbClase.DisplayMember = "Nombre"
        Me.cmbClase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbClase.FormattingEnabled = True
        Me.cmbClase.Location = New System.Drawing.Point(83, 28)
        Me.cmbClase.Name = "cmbClase"
        Me.cmbClase.Size = New System.Drawing.Size(298, 21)
        Me.cmbClase.TabIndex = 26
        Me.cmbClase.ValueMember = "Nombre"
        '
        'bsClase
        '
        Me.bsClase.DataMember = "Clase"
        Me.bsClase.DataSource = Me.dsCustomImporTask
        Me.bsClase.Sort = "Nombre"
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(379, 25)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(34, 20)
        Me.btnBuscar.TabIndex = 28
        Me.btnBuscar.Text = "..."
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'pnInformacionEnsamblado
        '
        Me.pnInformacionEnsamblado.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnInformacionEnsamblado.Controls.Add(Label3)
        Me.pnInformacionEnsamblado.Controls.Add(Me.cmbMetodos)
        Me.pnInformacionEnsamblado.Controls.Add(lblTitulo)
        Me.pnInformacionEnsamblado.Controls.Add(Label1)
        Me.pnInformacionEnsamblado.Controls.Add(Me.cmbClase)
        Me.pnInformacionEnsamblado.Location = New System.Drawing.Point(12, 80)
        Me.pnInformacionEnsamblado.Name = "pnInformacionEnsamblado"
        Me.pnInformacionEnsamblado.Size = New System.Drawing.Size(401, 97)
        Me.pnInformacionEnsamblado.TabIndex = 29
        '
        'cmbMetodos
        '
        Me.cmbMetodos.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.bsEnsamblado, "Metodo", True))
        Me.cmbMetodos.DataSource = Me.bsMetodo
        Me.cmbMetodos.DisplayMember = "Firma"
        Me.cmbMetodos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetodos.FormattingEnabled = True
        Me.cmbMetodos.Location = New System.Drawing.Point(83, 55)
        Me.cmbMetodos.Name = "cmbMetodos"
        Me.cmbMetodos.Size = New System.Drawing.Size(298, 21)
        Me.cmbMetodos.TabIndex = 29
        Me.cmbMetodos.ValueMember = "Firma"
        '
        'bsMetodo
        '
        Me.bsMetodo.DataMember = "Clase_Metodo"
        Me.bsMetodo.DataSource = Me.bsClase
        Me.bsMetodo.Sort = "Firma"
        '
        'btnCargarEnsamblado
        '
        Me.btnCargarEnsamblado.Location = New System.Drawing.Point(279, 51)
        Me.btnCargarEnsamblado.Name = "btnCargarEnsamblado"
        Me.btnCargarEnsamblado.Size = New System.Drawing.Size(134, 23)
        Me.btnCargarEnsamblado.TabIndex = 30
        Me.btnCargarEnsamblado.Text = "Cargar Ensamblado"
        Me.btnCargarEnsamblado.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(338, 183)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 32
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(257, 183)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 31
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'opEnsamblado
        '
        Me.opEnsamblado.Filter = "Ensamblados|*.dll|Ejecutables|*.exe"
        '
        'CustomTaskDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.ClientSize = New System.Drawing.Size(424, 218)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.btnCargarEnsamblado)
        Me.Controls.Add(Me.pnInformacionEnsamblado)
        Me.Controls.Add(Me.btnBuscar)
        Me.Controls.Add(Me.txtEnsamblado)
        Me.Controls.Add(DestinoLabel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CustomTaskDialog"
        Me.Text = "Configuración Tarea Personalizada."
        CType(Me.bsEnsamblado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsCustomImporTask, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsClase, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnInformacionEnsamblado.ResumeLayout(False)
        Me.pnInformacionEnsamblado.PerformLayout()
        CType(Me.bsMetodo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtEnsamblado As System.Windows.Forms.TextBox
    Friend WithEvents cmbClase As System.Windows.Forms.ComboBox
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents pnInformacionEnsamblado As System.Windows.Forms.Panel
    Friend WithEvents btnCargarEnsamblado As System.Windows.Forms.Button
    Friend WithEvents cmbMetodos As System.Windows.Forms.ComboBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents dsCustomImporTask As Desktop.Data.Transformations.CustomImporTaskDataSet
    Friend WithEvents bsEnsamblado As System.Windows.Forms.BindingSource
    Friend WithEvents bsClase As System.Windows.Forms.BindingSource
    Friend WithEvents bsMetodo As System.Windows.Forms.BindingSource
    Friend WithEvents opEnsamblado As System.Windows.Forms.OpenFileDialog
End Class
