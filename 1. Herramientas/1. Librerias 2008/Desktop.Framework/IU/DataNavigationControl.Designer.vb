<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataNavigationControl
    Inherits System.Windows.Forms.UserControl

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DataNavigationDialog))
        Me.ctlNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorUndo = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorSaveCurrent = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSaveAll = New System.Windows.Forms.ToolStripButton
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel5 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ctlStatus = New System.Windows.Forms.StatusStrip
        Me.lblEstadoRegistro = New System.Windows.Forms.ToolStripStatusLabel
        Me.TopPanel = New System.Windows.Forms.Panel
        Me.ContentPanel = New System.Windows.Forms.Panel
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.ctlNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctlNavigator.SuspendLayout()
        Me.ctlStatus.SuspendLayout()
        Me.TopPanel.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ctlNavigator
        '
        Me.ctlNavigator.AddNewItem = Nothing
        Me.ctlNavigator.CountItem = Me.BindingNavigatorCountItem
        Me.ctlNavigator.DeleteItem = Nothing
        Me.ctlNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ctlNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorUndo, Me.BindingNavigatorSeparator3, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem, Me.BindingNavigatorSeparator4, Me.BindingNavigatorSaveCurrent, Me.BindingNavigatorSaveAll})
        Me.ctlNavigator.Location = New System.Drawing.Point(0, 0)
        Me.ctlNavigator.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.ctlNavigator.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.ctlNavigator.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.ctlNavigator.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.ctlNavigator.Name = "ctlNavigator"
        Me.ctlNavigator.Padding = New System.Windows.Forms.Padding(3, 0, 1, 0)
        Me.ctlNavigator.PositionItem = Me.BindingNavigatorPositionItem
        Me.ctlNavigator.Size = New System.Drawing.Size(436, 25)
        Me.ctlNavigator.TabIndex = 0
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(38, 22)
        Me.BindingNavigatorCountItem.Text = "de {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Número total de elementos"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Mover primero"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Mover anterior"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Posición"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Posición actual"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Mover siguiente"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Mover último"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorUndo
        '
        Me.BindingNavigatorUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorUndo.Image = CType(resources.GetObject("BindingNavigatorUndo.Image"), System.Drawing.Image)
        Me.BindingNavigatorUndo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BindingNavigatorUndo.Name = "BindingNavigatorUndo"
        Me.BindingNavigatorUndo.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorUndo.Text = "Deshacer"
        Me.BindingNavigatorUndo.ToolTipText = "Deshacer (Ctrl + Z)"
        '
        'BindingNavigatorSeparator3
        '
        Me.BindingNavigatorSeparator3.Name = "BindingNavigatorSeparator3"
        Me.BindingNavigatorSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorAddNewItem
        '
        Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorAddNewItem.Image = CType(resources.GetObject("BindingNavigatorAddNewItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
        Me.BindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorAddNewItem.Text = "Agregar nuevo"
        Me.BindingNavigatorAddNewItem.ToolTipText = "Nuevo Registro (Ctrl + N)"
        '
        'BindingNavigatorDeleteItem
        '
        Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
        Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorDeleteItem.Text = "Eliminar"
        Me.BindingNavigatorDeleteItem.ToolTipText = "Eliminar (Ctrl + X)"
        '
        'BindingNavigatorSeparator4
        '
        Me.BindingNavigatorSeparator4.Name = "BindingNavigatorSeparator4"
        Me.BindingNavigatorSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorSaveCurrent
        '
        Me.BindingNavigatorSaveCurrent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorSaveCurrent.Image = CType(resources.GetObject("BindingNavigatorSaveCurrent.Image"), System.Drawing.Image)
        Me.BindingNavigatorSaveCurrent.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BindingNavigatorSaveCurrent.Name = "BindingNavigatorSaveCurrent"
        Me.BindingNavigatorSaveCurrent.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorSaveCurrent.Text = "BindingNavigatorSaveCurrent"
        Me.BindingNavigatorSaveCurrent.ToolTipText = "Guardar (Ctrl-G)"
        '
        'BindingNavigatorSaveAll
        '
        Me.BindingNavigatorSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorSaveAll.Image = CType(resources.GetObject("BindingNavigatorSaveAll.Image"), System.Drawing.Image)
        Me.BindingNavigatorSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BindingNavigatorSaveAll.Name = "BindingNavigatorSaveAll"
        Me.BindingNavigatorSaveAll.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorSaveAll.Text = "BindingNavigatorSaveAll"
        Me.BindingNavigatorSaveAll.ToolTipText = "Guardar Todo (Ctrl+Shit+G)"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(0, 17)
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(0, 17)
        Me.ToolStripStatusLabel3.Text = "-"
        '
        'ToolStripStatusLabel5
        '
        Me.ToolStripStatusLabel5.Name = "ToolStripStatusLabel5"
        Me.ToolStripStatusLabel5.Size = New System.Drawing.Size(0, 17)
        '
        'ctlStatus
        '
        Me.ctlStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(157, Byte), Integer), CType(CType(185, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.ctlStatus.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.ctlStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel2, Me.ToolStripStatusLabel3, Me.ToolStripStatusLabel5, Me.lblEstadoRegistro})
        Me.ctlStatus.Location = New System.Drawing.Point(0, 195)
        Me.ctlStatus.Name = "ctlStatus"
        Me.ctlStatus.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode
        Me.ctlStatus.Size = New System.Drawing.Size(436, 22)
        Me.ctlStatus.SizingGrip = False
        Me.ctlStatus.TabIndex = 1
        Me.ctlStatus.Text = "StatusStrip1"
        '
        'lblEstadoRegistro
        '
        Me.lblEstadoRegistro.Name = "lblEstadoRegistro"
        Me.lblEstadoRegistro.Size = New System.Drawing.Size(421, 17)
        Me.lblEstadoRegistro.Spring = True
        Me.lblEstadoRegistro.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TopPanel
        '
        Me.TopPanel.Controls.Add(Me.ctlNavigator)
        Me.TopPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopPanel.Name = "TopPanel"
        Me.TopPanel.Size = New System.Drawing.Size(436, 24)
        Me.TopPanel.TabIndex = 2
        '
        'ContentPanel
        '
        Me.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContentPanel.Location = New System.Drawing.Point(0, 24)
        Me.ContentPanel.Name = "ContentPanel"
        Me.ContentPanel.Size = New System.Drawing.Size(436, 171)
        Me.ContentPanel.TabIndex = 3
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'DataEditDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(436, 217)
        Me.Controls.Add(Me.ContentPanel)
        Me.Controls.Add(Me.TopPanel)
        Me.Controls.Add(Me.ctlStatus)
        Me.Name = "DataEditDialog"
        Me.Text = "DataEditDialog"
        CType(Me.ctlNavigator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctlNavigator.ResumeLayout(False)
        Me.ctlNavigator.PerformLayout()
        Me.ctlStatus.ResumeLayout(False)
        Me.ctlStatus.PerformLayout()
        Me.TopPanel.ResumeLayout(False)
        Me.TopPanel.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Protected WithEvents BindingNavigatorAddNewItem As System.Windows.Forms.ToolStripButton
    Protected WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Protected WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
    Protected WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Protected WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Protected WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Protected WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Protected WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Protected WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Protected WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Protected WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Protected WithEvents BindingNavigatorSeparator4 As System.Windows.Forms.ToolStripSeparator
    Protected WithEvents BindingNavigatorSaveCurrent As System.Windows.Forms.ToolStripButton
    Protected WithEvents BindingNavigatorSaveAll As System.Windows.Forms.ToolStripButton
    Protected WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Protected WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Protected WithEvents ToolStripStatusLabel5 As System.Windows.Forms.ToolStripStatusLabel
    Protected WithEvents TopPanel As System.Windows.Forms.Panel
    Protected WithEvents ContentPanel As System.Windows.Forms.Panel
    Friend WithEvents lblEstadoRegistro As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents ctlStatus As System.Windows.Forms.StatusStrip
    Private WithEvents ctlNavigator As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorUndo As System.Windows.Forms.ToolStripButton
    Protected WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
End Class
