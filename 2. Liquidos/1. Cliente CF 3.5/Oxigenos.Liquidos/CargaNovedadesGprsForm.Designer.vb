<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CargaNovedadesGprsForm
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
        Me.components = New System.ComponentModel.Container
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.menuCancelar = New System.Windows.Forms.MenuItem
        Me.UiHandler1 = New MovilidadCF.Windows.Forms.UIHandler
        Me.bsHojasRuta = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsHojasRuta = New Oxigenos.Liquidos.HojasRutaDataSet
        Me.bsLugaresCarga = New System.Windows.Forms.BindingSource(Me.components)
        Me.HojasRutaTableAdapter = New Oxigenos.Liquidos.HojasRutaDataSetTableAdapters.HojasRutaTableAdapter
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.menuCancelar)
        '
        'menuCancelar
        '
        Me.menuCancelar.Text = "Cancelar"
        '
        'UiHandler1
        '
        Me.UiHandler1.HelpControl = Nothing
        Me.UiHandler1.Parent = Nothing
        '
        'bsHojasRuta
        '
        Me.bsHojasRuta.DataMember = "HojasRuta"
        Me.bsHojasRuta.DataSource = Me.dsHojasRuta
        '
        'dsHojasRuta
        '
        Me.dsHojasRuta.DataSetName = "HojasRutaDataSet"
        Me.dsHojasRuta.Prefix = ""
        Me.dsHojasRuta.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'bsLugaresCarga
        '
        Me.bsLugaresCarga.DataMember = "LugaresCarga"
        Me.bsLugaresCarga.DataSource = Me.dsHojasRuta
        '
        'HojasRutaTableAdapter
        '
        Me.HojasRutaTableAdapter.ClearBeforeFill = True
        '
        'EnvioDatosGprsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Menu = Me.mainMenu1
        Me.MinimizeBox = False
        Me.Name = "EnvioDatosGprsForm"
        Me.Text = "Praxair"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UiHandler1 As MovilidadCF.Windows.Forms.UIHandler
    Friend WithEvents dsHojasRuta As Oxigenos.Liquidos.HojasRutaDataSet
    Friend WithEvents bsHojasRuta As System.Windows.Forms.BindingSource
    Friend WithEvents bsLugaresCarga As System.Windows.Forms.BindingSource
    Friend WithEvents menuCancelar As System.Windows.Forms.MenuItem
    Friend WithEvents HojasRutaTableAdapter As Oxigenos.Liquidos.HojasRutaDataSetTableAdapters.HojasRutaTableAdapter
End Class
