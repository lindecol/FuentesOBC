﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class BarcodeForm2
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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.btnSalir = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblSimbologia = New System.Windows.Forms.Label
        Me.lblDato = New System.Windows.Forms.Label
        Me.UiHandler1 = New MovilidadCF.Windows.Forms.UIHandler
        Me.SuspendLayout()
        '
        'btnSalir
        '
        Me.btnSalir.Location = New System.Drawing.Point(15, 174)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(112, 20)
        Me.btnSalir.TabIndex = 11
        Me.btnSalir.Text = "Salir"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(15, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 20)
        Me.Label4.Text = "Simbologia"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(15, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 20)
        Me.Label3.Text = "Dato:"
        '
        'lblSimbologia
        '
        Me.lblSimbologia.Location = New System.Drawing.Point(15, 123)
        Me.lblSimbologia.Name = "lblSimbologia"
        Me.lblSimbologia.Size = New System.Drawing.Size(235, 20)
        '
        'lblDato
        '
        Me.lblDato.Location = New System.Drawing.Point(15, 33)
        Me.lblDato.Name = "lblDato"
        Me.lblDato.Size = New System.Drawing.Size(247, 38)
        '
        'UiHandler1
        '
        Me.UiHandler1.HelpControl = Nothing
        Me.UiHandler1.Parent = Nothing
        '
        'BarcodeForm2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(294, 242)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblSimbologia)
        Me.Controls.Add(Me.lblDato)
        Me.Menu = Me.mainMenu1
        Me.Name = "BarcodeForm2"
        Me.Text = "BarcodeForm2"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblSimbologia As System.Windows.Forms.Label
    Friend WithEvents lblDato As System.Windows.Forms.Label
    Friend WithEvents UiHandler1 As MovilidadCF.Windows.Forms.UIHandler
End Class
