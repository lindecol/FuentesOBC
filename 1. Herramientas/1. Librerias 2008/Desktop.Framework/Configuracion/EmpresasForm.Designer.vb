<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EmpresasForm
    Inherits Desktop.Framework.DataNavigationDialog


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
        Dim NITLabel As System.Windows.Forms.Label
        Dim NombreCortoLabel As System.Windows.Forms.Label
        Dim NombreCompletoLabel As System.Windows.Forms.Label
        Dim DireccionLabel As System.Windows.Forms.Label
        Dim CiudadLabel As System.Windows.Forms.Label
        Dim PaisLabel As System.Windows.Forms.Label
        Me.bsEmpresas = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsConfiguracion = New System.Data.DataSet
        Me.NITTextBox = New System.Windows.Forms.TextBox
        Me.NombreCortoTextBox = New System.Windows.Forms.TextBox
        Me.NombreCompletoTextBox = New System.Windows.Forms.TextBox
        Me.DireccionTextBox = New System.Windows.Forms.TextBox
        Me.CiudadTextBox = New System.Windows.Forms.TextBox
        Me.PaisTextBox = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExaminar = New System.Windows.Forms.Button
        Me.pcImagen = New System.Windows.Forms.PictureBox
        Me.txtLogo = New System.Windows.Forms.TextBox
        NITLabel = New System.Windows.Forms.Label
        NombreCortoLabel = New System.Windows.Forms.Label
        NombreCompletoLabel = New System.Windows.Forms.Label
        DireccionLabel = New System.Windows.Forms.Label
        CiudadLabel = New System.Windows.Forms.Label
        PaisLabel = New System.Windows.Forms.Label
        Me.ContentPanel.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bsEmpresas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsConfiguracion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pcImagen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TopPanel
        '
        Me.TopPanel.Size = New System.Drawing.Size(563, 28)
        '
        'ContentPanel
        '
        Me.ContentPanel.Controls.Add(Me.pcImagen)
        Me.ContentPanel.Controls.Add(Me.btnExaminar)
        Me.ContentPanel.Controls.Add(Me.Label1)
        Me.ContentPanel.Controls.Add(Me.txtLogo)
        Me.ContentPanel.Controls.Add(Me.PaisTextBox)
        Me.ContentPanel.Controls.Add(Me.CiudadTextBox)
        Me.ContentPanel.Controls.Add(Me.DireccionTextBox)
        Me.ContentPanel.Controls.Add(Me.NombreCompletoTextBox)
        Me.ContentPanel.Controls.Add(Me.NITTextBox)
        Me.ContentPanel.Controls.Add(Me.NombreCortoTextBox)
        Me.ContentPanel.Controls.Add(NITLabel)
        Me.ContentPanel.Controls.Add(NombreCortoLabel)
        Me.ContentPanel.Controls.Add(NombreCompletoLabel)
        Me.ContentPanel.Controls.Add(DireccionLabel)
        Me.ContentPanel.Controls.Add(CiudadLabel)
        Me.ContentPanel.Controls.Add(PaisLabel)
        Me.ContentPanel.Location = New System.Drawing.Point(0, 28)
        Me.ContentPanel.Size = New System.Drawing.Size(563, 306)
        '
        'NITLabel
        '
        NITLabel.AutoSize = True
        NITLabel.Location = New System.Drawing.Point(12, 17)
        NITLabel.Name = "NITLabel"
        NITLabel.Size = New System.Drawing.Size(28, 13)
        NITLabel.TabIndex = 1
        NITLabel.Text = "NIT:"
        '
        'NombreCortoLabel
        '
        NombreCortoLabel.AutoSize = True
        NombreCortoLabel.Location = New System.Drawing.Point(118, 17)
        NombreCortoLabel.Name = "NombreCortoLabel"
        NombreCortoLabel.Size = New System.Drawing.Size(75, 13)
        NombreCortoLabel.TabIndex = 3
        NombreCortoLabel.Text = "Nombre Corto:"
        '
        'NombreCompletoLabel
        '
        NombreCompletoLabel.AutoSize = True
        NombreCompletoLabel.Location = New System.Drawing.Point(12, 65)
        NombreCompletoLabel.Name = "NombreCompletoLabel"
        NombreCompletoLabel.Size = New System.Drawing.Size(94, 13)
        NombreCompletoLabel.TabIndex = 5
        NombreCompletoLabel.Text = "Nombre Completo:"
        '
        'DireccionLabel
        '
        DireccionLabel.AutoSize = True
        DireccionLabel.Location = New System.Drawing.Point(12, 114)
        DireccionLabel.Name = "DireccionLabel"
        DireccionLabel.Size = New System.Drawing.Size(55, 13)
        DireccionLabel.TabIndex = 7
        DireccionLabel.Text = "Dirección:"
        '
        'CiudadLabel
        '
        CiudadLabel.AutoSize = True
        CiudadLabel.Location = New System.Drawing.Point(12, 192)
        CiudadLabel.Name = "CiudadLabel"
        CiudadLabel.Size = New System.Drawing.Size(43, 13)
        CiudadLabel.TabIndex = 9
        CiudadLabel.Text = "Ciudad:"
        '
        'PaisLabel
        '
        PaisLabel.AutoSize = True
        PaisLabel.Location = New System.Drawing.Point(135, 192)
        PaisLabel.Name = "PaisLabel"
        PaisLabel.Size = New System.Drawing.Size(30, 13)
        PaisLabel.TabIndex = 11
        PaisLabel.Text = "Pais:"
        '
        'bsEmpresas
        '
        Me.bsEmpresas.DataSource = Me.dsConfiguracion
        Me.bsEmpresas.Position = 0
        '
        'dsConfiguracion
        '
        Me.dsConfiguracion.DataSetName = "ConfiguracionDataSet"
        '
        'NITTextBox
        '
        Me.NITTextBox.Location = New System.Drawing.Point(15, 33)
        Me.NITTextBox.Name = "NITTextBox"
        Me.NITTextBox.Size = New System.Drawing.Size(100, 20)
        Me.NITTextBox.TabIndex = 2
        '
        'NombreCortoTextBox
        '
        Me.NombreCortoTextBox.Location = New System.Drawing.Point(121, 33)
        Me.NombreCortoTextBox.Name = "NombreCortoTextBox"
        Me.NombreCortoTextBox.Size = New System.Drawing.Size(315, 20)
        Me.NombreCortoTextBox.TabIndex = 4
        '
        'NombreCompletoTextBox
        '
        Me.NombreCompletoTextBox.Location = New System.Drawing.Point(15, 81)
        Me.NombreCompletoTextBox.Name = "NombreCompletoTextBox"
        Me.NombreCompletoTextBox.Size = New System.Drawing.Size(526, 20)
        Me.NombreCompletoTextBox.TabIndex = 6
        '
        'DireccionTextBox
        '
        Me.DireccionTextBox.Location = New System.Drawing.Point(15, 130)
        Me.DireccionTextBox.Multiline = True
        Me.DireccionTextBox.Name = "DireccionTextBox"
        Me.DireccionTextBox.Size = New System.Drawing.Size(526, 48)
        Me.DireccionTextBox.TabIndex = 8
        '
        'CiudadTextBox
        '
        Me.CiudadTextBox.Location = New System.Drawing.Point(15, 208)
        Me.CiudadTextBox.Name = "CiudadTextBox"
        Me.CiudadTextBox.Size = New System.Drawing.Size(100, 20)
        Me.CiudadTextBox.TabIndex = 10
        '
        'PaisTextBox
        '
        Me.PaisTextBox.Location = New System.Drawing.Point(138, 208)
        Me.PaisTextBox.Name = "PaisTextBox"
        Me.PaisTextBox.Size = New System.Drawing.Size(147, 20)
        Me.PaisTextBox.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 242)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Logo:"
        '
        'btnExaminar
        '
        Me.btnExaminar.Location = New System.Drawing.Point(193, 242)
        Me.btnExaminar.Name = "btnExaminar"
        Me.btnExaminar.Size = New System.Drawing.Size(99, 23)
        Me.btnExaminar.TabIndex = 15
        Me.btnExaminar.Text = "Cambiar"
        Me.btnExaminar.UseVisualStyleBackColor = True
        '
        'pcImagen
        '
        Me.pcImagen.BackColor = System.Drawing.Color.White
        Me.pcImagen.Location = New System.Drawing.Point(52, 242)
        Me.pcImagen.Name = "pcImagen"
        Me.pcImagen.Size = New System.Drawing.Size(135, 43)
        Me.pcImagen.TabIndex = 17
        Me.pcImagen.TabStop = False
        '
        'txtLogo
        '
        Me.txtLogo.Location = New System.Drawing.Point(193, 242)
        Me.txtLogo.Name = "txtLogo"
        Me.txtLogo.Size = New System.Drawing.Size(63, 20)
        Me.txtLogo.TabIndex = 13
        Me.txtLogo.Visible = False
        '
        'EmpresasForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BindingSource = Me.bsEmpresas
        Me.ClientSize = New System.Drawing.Size(563, 356)
        Me.Name = "EmpresasForm"
        Me.Text = "Definición Empresas"
        Me.ContentPanel.ResumeLayout(False)
        Me.ContentPanel.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bsEmpresas, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsConfiguracion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pcImagen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dsConfiguracion As DataSet
    Friend WithEvents bsEmpresas As System.Windows.Forms.BindingSource
    Friend WithEvents NITTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NombreCortoTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NombreCompletoTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DireccionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CiudadTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PaisTextBox As System.Windows.Forms.TextBox
    Friend WithEvents btnExaminar As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pcImagen As System.Windows.Forms.PictureBox
    Friend WithEvents txtLogo As System.Windows.Forms.TextBox
End Class
