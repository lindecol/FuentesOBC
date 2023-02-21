<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditTareaDialog
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
        Dim TipoTareaLabel As System.Windows.Forms.Label
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.Shape1 = New Desktop.Windows.Forms.Shape
        Me.dsConfig = New Desktop.Data.Transformations.TransformationDataSet
        Me.bsTareas = New System.Windows.Forms.BindingSource(Me.components)
        Me.NombreTextBox = New System.Windows.Forms.TextBox
        Me.cbTipoTarea = New System.Windows.Forms.ComboBox
        NombreLabel = New System.Windows.Forms.Label
        TipoTareaLabel = New System.Windows.Forms.Label
        CType(Me.dsConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsTareas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NombreLabel
        '
        NombreLabel.AutoSize = True
        NombreLabel.Location = New System.Drawing.Point(12, 18)
        NombreLabel.Name = "NombreLabel"
        NombreLabel.Size = New System.Drawing.Size(47, 13)
        NombreLabel.TabIndex = 6
        NombreLabel.Text = "Nombre:"
        '
        'TipoTareaLabel
        '
        TipoTareaLabel.AutoSize = True
        TipoTareaLabel.Location = New System.Drawing.Point(12, 69)
        TipoTareaLabel.Name = "TipoTareaLabel"
        TipoTareaLabel.Size = New System.Drawing.Size(62, 13)
        TipoTareaLabel.TabIndex = 7
        TipoTareaLabel.Text = "Tipo Tarea:"
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(348, 131)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 4
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(267, 131)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 3
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'Shape1
        '
        Me.Shape1.Enabled = False
        Me.Shape1.ForeColor = System.Drawing.SystemColors.InactiveCaption
        Me.Shape1.Location = New System.Drawing.Point(16, 120)
        Me.Shape1.Name = "Shape1"
        Me.Shape1.Size = New System.Drawing.Size(408, 10)
        Me.Shape1.Style = Desktop.Windows.Forms.Shape.ShapeStyle.TopLine
        Me.Shape1.TabIndex = 5
        Me.Shape1.Text = "Shape1"
        '
        'dsConfig
        '
        Me.dsConfig.DataSetName = "TransformationDataSet"
        Me.dsConfig.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'bsTareas
        '
        Me.bsTareas.DataMember = "Tareas"
        Me.bsTareas.DataSource = Me.dsConfig
        '
        'NombreTextBox
        '
        Me.NombreTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsTareas, "Description", True))
        Me.NombreTextBox.Location = New System.Drawing.Point(15, 36)
        Me.NombreTextBox.Name = "NombreTextBox"
        Me.NombreTextBox.Size = New System.Drawing.Size(408, 20)
        Me.NombreTextBox.TabIndex = 7
        '
        'cbTipoTarea
        '
        Me.cbTipoTarea.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsTareas, "TaskTypeName", True))
        Me.cbTipoTarea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTipoTarea.FormattingEnabled = True
        Me.cbTipoTarea.Location = New System.Drawing.Point(15, 85)
        Me.cbTipoTarea.Name = "cbTipoTarea"
        Me.cbTipoTarea.Size = New System.Drawing.Size(258, 21)
        Me.cbTipoTarea.TabIndex = 8
        '
        'EditTareaDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.ClientSize = New System.Drawing.Size(443, 164)
        Me.ControlBox = False
        Me.Controls.Add(Me.cbTipoTarea)
        Me.Controls.Add(NombreLabel)
        Me.Controls.Add(Me.NombreTextBox)
        Me.Controls.Add(Me.Shape1)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(TipoTareaLabel)
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditTareaDialog"
        Me.Text = "EditTareaDialog"
        CType(Me.dsConfig, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsTareas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents Shape1 As Desktop.Windows.Forms.Shape
    Friend WithEvents dsConfig As Desktop.Data.Transformations.TransformationDataSet
    Friend WithEvents bsTareas As System.Windows.Forms.BindingSource
    Friend WithEvents NombreTextBox As System.Windows.Forms.TextBox
    Friend WithEvents cbTipoTarea As System.Windows.Forms.ComboBox
End Class
