<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.lstLog = New System.Windows.Forms.ListBox
        Me.TransformationEditControl1 = New Desktop.Data.Transformations.TransformationEditControl
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(0, 225)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Ejecutar..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(93, 225)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'lstLog
        '
        Me.lstLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstLog.FormattingEnabled = True
        Me.lstLog.HorizontalScrollbar = True
        Me.lstLog.Location = New System.Drawing.Point(0, 247)
        Me.lstLog.Name = "lstLog"
        Me.lstLog.ScrollAlwaysVisible = True
        Me.lstLog.Size = New System.Drawing.Size(876, 303)
        Me.lstLog.TabIndex = 3
        '
        'TransformationEditControl1
        '
        Me.TransformationEditControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TransformationEditControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TransformationEditControl1.Location = New System.Drawing.Point(0, 0)
        Me.TransformationEditControl1.Name = "TransformationEditControl1"
        Me.TransformationEditControl1.Size = New System.Drawing.Size(876, 219)
        Me.TransformationEditControl1.TabIndex = 0
        Me.TransformationEditControl1.Transformation = Nothing
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(328, 217)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(876, 562)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.lstLog)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TransformationEditControl1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TransformationEditControl1 As Desktop.Data.Transformations.TransformationEditControl
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents lstLog As System.Windows.Forms.ListBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
End Class
