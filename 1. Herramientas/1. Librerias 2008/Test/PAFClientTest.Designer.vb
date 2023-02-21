<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PAFClientTest
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
		Me.txtServidor = New System.Windows.Forms.TextBox
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.txtPuerto = New System.Windows.Forms.TextBox
		Me.txtRespuesta = New System.Windows.Forms.TextBox
		Me.txtActor = New System.Windows.Forms.TextBox
		Me.Label4 = New System.Windows.Forms.Label
		Me.lblLineas = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.txtParametros = New System.Windows.Forms.TextBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.chkEncabezado = New System.Windows.Forms.CheckBox
		Me.SuspendLayout()
		'
		'Button1
		'
		Me.Button1.Location = New System.Drawing.Point(355, 66)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(76, 22)
		Me.Button1.TabIndex = 0
		Me.Button1.Text = "Ejecutar"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'txtServidor
		'
		Me.txtServidor.Location = New System.Drawing.Point(29, 26)
		Me.txtServidor.Name = "txtServidor"
		Me.txtServidor.Size = New System.Drawing.Size(127, 20)
		Me.txtServidor.TabIndex = 2
		Me.txtServidor.Text = "192.168.147.35"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(29, 9)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(46, 13)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "Servidor"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(162, 9)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(38, 13)
		Me.Label3.TabIndex = 4
		Me.Label3.Text = "Puerto"
		'
		'txtPuerto
		'
		Me.txtPuerto.Location = New System.Drawing.Point(165, 26)
		Me.txtPuerto.Name = "txtPuerto"
		Me.txtPuerto.Size = New System.Drawing.Size(91, 20)
		Me.txtPuerto.TabIndex = 5
		Me.txtPuerto.Text = "2500"
		'
		'txtRespuesta
		'
		Me.txtRespuesta.Location = New System.Drawing.Point(29, 115)
		Me.txtRespuesta.Multiline = True
		Me.txtRespuesta.Name = "txtRespuesta"
		Me.txtRespuesta.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.txtRespuesta.Size = New System.Drawing.Size(578, 173)
		Me.txtRespuesta.TabIndex = 7
		Me.txtRespuesta.WordWrap = False
		'
		'txtActor
		'
		Me.txtActor.Location = New System.Drawing.Point(262, 26)
		Me.txtActor.Name = "txtActor"
		Me.txtActor.Size = New System.Drawing.Size(87, 20)
		Me.txtActor.TabIndex = 8
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(26, 301)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(68, 13)
		Me.Label4.TabIndex = 10
		Me.Label4.Text = "Total Lineas:"
		'
		'lblLineas
		'
		Me.lblLineas.AutoSize = True
		Me.lblLineas.Location = New System.Drawing.Point(100, 301)
		Me.lblLineas.Name = "lblLineas"
		Me.lblLineas.Size = New System.Drawing.Size(13, 13)
		Me.lblLineas.TabIndex = 11
		Me.lblLineas.Text = "0"
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(29, 51)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(63, 13)
		Me.Label5.TabIndex = 12
		Me.Label5.Text = "Parametros:"
		'
		'txtParametros
		'
		Me.txtParametros.Location = New System.Drawing.Point(29, 67)
		Me.txtParametros.Name = "txtParametros"
		Me.txtParametros.Size = New System.Drawing.Size(320, 20)
		Me.txtParametros.TabIndex = 13
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(259, 9)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(32, 13)
		Me.Label1.TabIndex = 9
		Me.Label1.Text = "Actor"
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(29, 97)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(63, 13)
		Me.Label6.TabIndex = 14
		Me.Label6.Text = "Resultados:"
		'
		'chkEncabezado
		'
		Me.chkEncabezado.AutoSize = True
		Me.chkEncabezado.Location = New System.Drawing.Point(349, 295)
		Me.chkEncabezado.Name = "chkEncabezado"
		Me.chkEncabezado.Size = New System.Drawing.Size(109, 17)
		Me.chkEncabezado.TabIndex = 15
		Me.chkEncabezado.Text = "usar Encabezado"
		Me.chkEncabezado.UseVisualStyleBackColor = True
		'
		'PAFClientTest
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(616, 328)
		Me.Controls.Add(Me.chkEncabezado)
		Me.Controls.Add(Me.Label6)
		Me.Controls.Add(Me.txtParametros)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.lblLineas)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.txtActor)
		Me.Controls.Add(Me.txtRespuesta)
		Me.Controls.Add(Me.txtPuerto)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.txtServidor)
		Me.Controls.Add(Me.Button1)
		Me.Name = "PAFClientTest"
		Me.Text = "PAF Client Test"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents Button1 As System.Windows.Forms.Button
	Friend WithEvents txtServidor As System.Windows.Forms.TextBox
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents txtPuerto As System.Windows.Forms.TextBox
	Friend WithEvents txtRespuesta As System.Windows.Forms.TextBox
	Friend WithEvents txtActor As System.Windows.Forms.TextBox
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents lblLineas As System.Windows.Forms.Label
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents txtParametros As System.Windows.Forms.TextBox
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents chkEncabezado As System.Windows.Forms.CheckBox
End Class
