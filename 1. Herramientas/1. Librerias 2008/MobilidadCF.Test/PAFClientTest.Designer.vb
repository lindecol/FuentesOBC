<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class PAFClientTest
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.Shape1 = New MovilidadCF.Windows.Forms.Shape
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtActor = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtParametros = New System.Windows.Forms.TextBox
        Me.txtResultados = New System.Windows.Forms.TextBox
        Me.btnEjecutar = New System.Windows.Forms.Button
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblTiempoRespuesta = New System.Windows.Forms.Label
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(1, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 20)
        Me.Label1.Text = "PAF Server:"
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(1, 24)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(168, 23)
        Me.txtServer.TabIndex = 2
        Me.txtServer.Text = "192.168.147.35"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(175, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 20)
        Me.Label2.Text = "Port"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(175, 24)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(60, 23)
        Me.txtPort.TabIndex = 4
        Me.txtPort.Text = "2500"
        '
        'Shape1
        '
        Me.Shape1.Enabled = False
        Me.Shape1.Location = New System.Drawing.Point(1, 51)
        Me.Shape1.Name = "Shape1"
        Me.Shape1.Size = New System.Drawing.Size(234, 10)
        Me.Shape1.Style = MovilidadCF.Windows.Forms.Shape.ShapeStyle.TopLine
        Me.Shape1.TabIndex = 6
        Me.Shape1.Text = "Shape1"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(1, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 20)
        Me.Label4.Text = "Actor:"
        '
        'txtActor
        '
        Me.txtActor.Location = New System.Drawing.Point(2, 87)
        Me.txtActor.Name = "txtActor"
        Me.txtActor.Size = New System.Drawing.Size(41, 23)
        Me.txtActor.TabIndex = 8
        Me.txtActor.Text = "22"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(48, 64)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 20)
        Me.Label5.Text = "Parametros:"
        '
        'txtParametros
        '
        Me.txtParametros.Location = New System.Drawing.Point(48, 87)
        Me.txtParametros.Name = "txtParametros"
        Me.txtParametros.Size = New System.Drawing.Size(162, 23)
        Me.txtParametros.TabIndex = 10
        Me.txtParametros.Text = "101|091103"
        '
        'txtResultados
        '
        Me.txtResultados.Location = New System.Drawing.Point(1, 114)
        Me.txtResultados.Multiline = True
        Me.txtResultados.Name = "txtResultados"
        Me.txtResultados.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtResultados.Size = New System.Drawing.Size(234, 119)
        Me.txtResultados.TabIndex = 11
        Me.txtResultados.WordWrap = False
        '
        'btnEjecutar
        '
        Me.btnEjecutar.Location = New System.Drawing.Point(213, 87)
        Me.btnEjecutar.Name = "btnEjecutar"
        Me.btnEjecutar.Size = New System.Drawing.Size(22, 23)
        Me.btnEjecutar.TabIndex = 12
        Me.btnEjecutar.Text = ">"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.Add(Me.MenuItem1)
        Me.MainMenu1.MenuItems.Add(Me.MenuItem2)
        '
        'MenuItem1
        '
        Me.MenuItem1.MenuItems.Add(Me.MenuItem3)
        Me.MenuItem1.Text = "Archiv"
        '
        'MenuItem2
        '
        Me.MenuItem2.MenuItems.Add(Me.MenuItem4)
        Me.MenuItem2.MenuItems.Add(Me.MenuItem5)
        Me.MenuItem2.Text = "Consulta"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(0, 240)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(130, 20)
        Me.Label3.Text = "Tiempo Respuesta:"
        '
        'lblTiempoRespuesta
        '
        Me.lblTiempoRespuesta.Location = New System.Drawing.Point(137, 240)
        Me.lblTiempoRespuesta.Name = "lblTiempoRespuesta"
        Me.lblTiempoRespuesta.Size = New System.Drawing.Size(100, 20)
        '
        'MenuItem3
        '
        Me.MenuItem3.Text = "Salir"
        '
        'MenuItem4
        '
        Me.MenuItem4.Checked = True
        Me.MenuItem4.Text = "Precios Exito"
        '
        'MenuItem5
        '
        Me.MenuItem5.Text = "Novedades Locatel"
        '
        'PAFClientTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 271)
        Me.Controls.Add(Me.lblTiempoRespuesta)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnEjecutar)
        Me.Controls.Add(Me.txtResultados)
        Me.Controls.Add(Me.txtParametros)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtActor)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Shape1)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtServer)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.MinimizeBox = False
        Me.Name = "PAFClientTest"
        Me.Text = "PAFClientTest"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents Shape1 As MovilidadCF.Windows.Forms.Shape
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtActor As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtParametros As System.Windows.Forms.TextBox
    Friend WithEvents txtResultados As System.Windows.Forms.TextBox
    Friend WithEvents btnEjecutar As System.Windows.Forms.Button
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTiempoRespuesta As System.Windows.Forms.Label
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
End Class
