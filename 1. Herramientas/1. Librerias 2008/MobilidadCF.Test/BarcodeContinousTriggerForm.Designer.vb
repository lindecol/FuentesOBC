<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class BarcodeContinousTriggerForm
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
        Me.lblData = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblLength = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblData
        '
        Me.lblData.Location = New System.Drawing.Point(14, 39)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(212, 45)
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(14, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 20)
        Me.Label2.Text = "Barcode:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(14, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 20)
        Me.Label3.Text = "Type:"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(14, 127)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 20)
        Me.Label4.Text = "Length"
        '
        'lblLength
        '
        Me.lblLength.Location = New System.Drawing.Point(73, 127)
        Me.lblLength.Name = "lblLength"
        Me.lblLength.Size = New System.Drawing.Size(138, 20)
        '
        'lblType
        '
        Me.lblType.Location = New System.Drawing.Point(73, 93)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(100, 20)
        '
        'BarcodeContinousTriggerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(262, 185)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.lblLength)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblData)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BarcodeContinousTriggerForm"
        Me.Text = "BarcodeContinousTriggerForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblLength As System.Windows.Forms.Label
    Friend WithEvents lblType As System.Windows.Forms.Label
End Class
