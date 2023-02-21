<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataPreviewDialog
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
        Me.dgvData = New System.Windows.Forms.DataGridView
        Me.rbtnVerDatos = New System.Windows.Forms.RadioButton
        Me.rbtnVerEsquema = New System.Windows.Forms.RadioButton
        Me.btnCerrar = New System.Windows.Forms.Button
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvData
        '
        Me.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvData.BackgroundColor = System.Drawing.Color.White
        Me.dgvData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvData.Location = New System.Drawing.Point(12, 14)
        Me.dgvData.Name = "dgvData"
        Me.dgvData.ReadOnly = True
        Me.dgvData.RowHeadersVisible = False
        Me.dgvData.Size = New System.Drawing.Size(659, 355)
        Me.dgvData.TabIndex = 0
        '
        'rbtnVerDatos
        '
        Me.rbtnVerDatos.AutoSize = True
        Me.rbtnVerDatos.Checked = True
        Me.rbtnVerDatos.Location = New System.Drawing.Point(12, 375)
        Me.rbtnVerDatos.Name = "rbtnVerDatos"
        Me.rbtnVerDatos.Size = New System.Drawing.Size(98, 17)
        Me.rbtnVerDatos.TabIndex = 1
        Me.rbtnVerDatos.TabStop = True
        Me.rbtnVerDatos.Text = "Visualizar datos"
        Me.rbtnVerDatos.UseVisualStyleBackColor = True
        '
        'rbtnVerEsquema
        '
        Me.rbtnVerEsquema.AutoSize = True
        Me.rbtnVerEsquema.Location = New System.Drawing.Point(116, 375)
        Me.rbtnVerEsquema.Name = "rbtnVerEsquema"
        Me.rbtnVerEsquema.Size = New System.Drawing.Size(115, 17)
        Me.rbtnVerEsquema.TabIndex = 2
        Me.rbtnVerEsquema.Text = "Visualizar esquema"
        Me.rbtnVerEsquema.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Location = New System.Drawing.Point(596, 375)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(75, 23)
        Me.btnCerrar.TabIndex = 3
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'DataPreviewDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.ClientSize = New System.Drawing.Size(683, 404)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.rbtnVerEsquema)
        Me.Controls.Add(Me.rbtnVerDatos)
        Me.Controls.Add(Me.dgvData)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DataPreviewDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Vista previa de datos"
        CType(Me.dgvData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvData As System.Windows.Forms.DataGridView
    Friend WithEvents rbtnVerDatos As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnVerEsquema As System.Windows.Forms.RadioButton
    Friend WithEvents btnCerrar As System.Windows.Forms.Button

End Class
