<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NavigationPanelControl
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NavigationPanelControl))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.tvModulos = New System.Windows.Forms.TreeView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.HeaderStrip1 = New Desktop.Windows.Forms.HeaderStrip
        Me.lblGrupoActual = New System.Windows.Forms.ToolStripLabel
        Me.OverFlowStrip = New Desktop.Windows.Forms.BaseStackStrip
        Me.StackStrip = New Desktop.Windows.Forms.StackStrip
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.HeaderStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.Color.Transparent
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.HeaderStrip1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.OverFlowStrip)
        Me.SplitContainer1.Panel2.Controls.Add(Me.StackStrip)
        Me.SplitContainer1.Size = New System.Drawing.Size(224, 630)
        Me.SplitContainer1.SplitterDistance = 347
        Me.SplitContainer1.SplitterWidth = 7
        Me.SplitContainer1.TabIndex = 0
        Me.SplitContainer1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.tvModulos)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(224, 322)
        Me.Panel1.TabIndex = 4
        '
        'tvModulos
        '
        Me.tvModulos.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tvModulos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvModulos.FullRowSelect = True
        Me.tvModulos.HideSelection = False
        Me.tvModulos.ImageIndex = 0
        Me.tvModulos.ImageList = Me.ImageList1
        Me.tvModulos.Location = New System.Drawing.Point(0, 0)
        Me.tvModulos.Name = "tvModulos"
        Me.tvModulos.SelectedImageIndex = 0
        Me.tvModulos.ShowLines = False
        Me.tvModulos.Size = New System.Drawing.Size(224, 322)
        Me.tvModulos.TabIndex = 4
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Black
        Me.ImageList1.Images.SetKeyName(0, "Archive.bmp")
        Me.ImageList1.Images.SetKeyName(1, "Genericfile.bmp")
        Me.ImageList1.Images.SetKeyName(2, "HTMLPageHS.bmp")
        Me.ImageList1.Images.SetKeyName(3, "book_reportHS.bmp")
        '
        'HeaderStrip1
        '
        Me.HeaderStrip1.AutoSize = False
        Me.HeaderStrip1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.HeaderStrip1.ForeColor = System.Drawing.Color.White
        Me.HeaderStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.HeaderStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblGrupoActual})
        Me.HeaderStrip1.Location = New System.Drawing.Point(0, 0)
        Me.HeaderStrip1.Name = "HeaderStrip1"
        Me.HeaderStrip1.Size = New System.Drawing.Size(224, 25)
        Me.HeaderStrip1.TabIndex = 0
        Me.HeaderStrip1.Text = "HeaderStrip1"
        '
        'lblGrupoActual
        '
        Me.lblGrupoActual.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrupoActual.Name = "lblGrupoActual"
        Me.lblGrupoActual.Padding = New System.Windows.Forms.Padding(2)
        Me.lblGrupoActual.Size = New System.Drawing.Size(84, 22)
        Me.lblGrupoActual.Text = "Inventario"
        '
        'OverFlowStrip
        '
        Me.OverFlowStrip.AutoSize = False
        Me.OverFlowStrip.CanOverflow = False
        Me.OverFlowStrip.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.OverFlowStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.OverFlowStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.OverFlowStrip.Location = New System.Drawing.Point(0, 251)
        Me.OverFlowStrip.Name = "OverFlowStrip"
        Me.OverFlowStrip.Padding = New System.Windows.Forms.Padding(2)
        Me.OverFlowStrip.Size = New System.Drawing.Size(224, 25)
        Me.OverFlowStrip.TabIndex = 1
        '
        'StackStrip
        '
        Me.StackStrip.AutoSize = False
        Me.StackStrip.CanOverflow = False
        Me.StackStrip.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StackStrip.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.StackStrip.GripMargin = New System.Windows.Forms.Padding(0)
        Me.StackStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.StackStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StackStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.StackStrip.Location = New System.Drawing.Point(0, 0)
        Me.StackStrip.Name = "StackStrip"
        Me.StackStrip.Padding = New System.Windows.Forms.Padding(0)
        Me.StackStrip.Size = New System.Drawing.Size(224, 276)
        Me.StackStrip.TabIndex = 0
        Me.StackStrip.Text = "StackStrip1"
        '
        'NavigationPanelControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "NavigationPanelControl"
        Me.Size = New System.Drawing.Size(224, 630)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.HeaderStrip1.ResumeLayout(False)
        Me.HeaderStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents StackStrip As Desktop.Windows.Forms.StackStrip
    Friend WithEvents HeaderStrip1 As Desktop.Windows.Forms.HeaderStrip
    Friend WithEvents lblGrupoActual As System.Windows.Forms.ToolStripLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents tvModulos As System.Windows.Forms.TreeView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents OverFlowStrip As Desktop.Windows.Forms.BaseStackStrip

End Class
