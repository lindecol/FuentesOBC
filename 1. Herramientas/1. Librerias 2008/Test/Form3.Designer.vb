<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
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
        Me.ListBar1 = New Desktop.Windows.Forms.ListBar
        Me.SuspendLayout()
        '
        'ListBar1
        '
        Me.ListBar1.AllowDragGroups = True
        Me.ListBar1.AllowDragItems = True
        Me.ListBar1.DrawStyle = Desktop.Windows.Forms.ListBarDrawStyle.ListBarDrawStyleOfficeXP
        Me.ListBar1.LargeImageList = Nothing
        Me.ListBar1.Location = New System.Drawing.Point(338, 45)
        Me.ListBar1.Name = "ListBar1"
        Me.ListBar1.SelectOnMouseDown = False
        Me.ListBar1.Size = New System.Drawing.Size(261, 321)
        Me.ListBar1.SmallImageList = Nothing
        Me.ListBar1.TabIndex = 0
        Me.ListBar1.ToolTip = Nothing
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(633, 396)
        Me.Controls.Add(Me.ListBar1)
        Me.Name = "Form3"
        Me.Text = "Form3"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListBar1 As Desktop.Windows.Forms.ListBar
End Class
