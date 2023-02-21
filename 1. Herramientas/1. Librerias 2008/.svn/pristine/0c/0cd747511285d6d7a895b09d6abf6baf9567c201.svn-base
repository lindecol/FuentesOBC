Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.Diagnostics
Imports System.Windows.Forms.Design
Imports System.Resources

Friend Class NavigationPanelControl

    Private dsSesion As DataSet

    Public WriteOnly Property imagesMenu() As ImageList
        Set(ByVal value As ImageList)
            Me.ImageList1 = value
        End Set
    End Property

    Private _listImages As List(Of String) = Nothing

    Public WriteOnly Property configImages() As List(Of String)
        Set(ByVal value As List(Of String))
            Me._listImages = value
        End Set
    End Property


    Public Event SelectionChanged(ByVal rowModulo As DataRow)


#Region " Codigo Personalización visual del control "

    Private _visibleCount As Integer = -1
    Private _lastCount As Integer = -1
    Private _maxHeight As Integer

    Private ReadOnly Property StackStripEnabledItemCount() As Integer
        Get
            Dim nEnabledCount As Integer = 0
            For J As Integer = 0 To StackStrip.ItemCount - 1
                If StackStrip.Items(J).Tag IsNot Nothing Then
                    nEnabledCount += 1
                End If
            Next
            Return nEnabledCount
        End Get
    End Property

    Private Sub AddOverflowItems()

        Dim sItem As ToolStripButton
        Dim tItem As ToolStripButton
        Dim trans As Color = Color.FromArgb(238, 238, 238)

        Me.OverFlowStrip.Items.Clear()
        Me.OverFlowStrip.Visible = True
        ' Add items to overflow
        For idx As Integer = Me.StackStrip.Items.Count - 1 To 0 Step -1
            sItem = CType(Me.StackStrip.Items(idx), ToolStripButton)
            tItem = New ToolStripButton()
            tItem.Image = sItem.Image
            tItem.ImageTransparentColor = sItem.ImageTransparentColor
            tItem.Tag = sItem.Tag
            tItem.ToolTipText = sItem.Text
            tItem.Alignment = ToolStripItemAlignment.Right
            Me.OverFlowStrip.Items.Add(tItem)

            ' Se configura el manejador del evento click
            AddHandler sItem.Click, AddressOf BotonCategoriaRaiz_Click
            AddHandler tItem.Click, AddressOf BotonCategoriaRaiz_Click
        Next

        Me.OverFlowStrip.Refresh()
    End Sub

    Private Function GetBitmapResource(ByVal name As String) As Bitmap
        Return CType(My.Resources.ResourceManager.GetObject(name), Bitmap)
    End Function

    Private Sub SplitContainer1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles SplitContainer1.Paint

        Dim pct As New ProfessionalColorTable
        Dim bounds As Rectangle = CType(sender, SplitContainer).SplitterRectangle

        Dim squares As Integer
        Dim maxSquares As Integer = 9
        Dim squareSize As Integer = 4
        Dim boxSize As Integer = 2
        Dim idx As Integer

        ' Make sure we need to do work
        If ((bounds.Width > 0) And (bounds.Height > 0)) Then

            Dim g As Graphics = e.Graphics

            ' Setup colors from the provided renderer
            Dim begin As Color = pct.OverflowButtonGradientMiddle
            Dim final As Color = pct.OverflowButtonGradientEnd

            ' Make sure we need to do work
            Using b As New LinearGradientBrush(bounds, begin, final, LinearGradientMode.Vertical)
                g.FillRectangle(b, bounds)
            End Using

            ' Calculate squares
            If ((bounds.Width > squareSize) And (bounds.Height > squareSize)) Then

                squares = CInt(Math.Min((bounds.Width / squareSize), maxSquares))

                ' Calculate start
                Dim start As Integer = CInt((bounds.Width - (squares * squareSize)) / 2)
                Dim Y As Integer = bounds.Y + 1

                '' Get brushes
                Dim dark As New SolidBrush(pct.GripDark)
                Dim middle As New SolidBrush(pct.ToolStripBorder)
                Dim light As New SolidBrush(pct.ToolStripDropDownBackground)

                ' Draw
                For idx = 0 To squares - 1
                    ' Draw grips
                    g.FillRectangle(dark, start, Y, boxSize, boxSize)
                    g.FillRectangle(light, start + 1, Y + 1, boxSize, boxSize)
                    g.FillRectangle(middle, start + 1, Y + 1, 1, 1)
                    start += squareSize
                Next

                dark.Dispose()
                middle.Dispose()
                light.Dispose()
            End If
        End If
    End Sub

    Private Sub stackStrip1_ItemHeightChanged(ByVal sender As Object, ByVal e As EventArgs)
        InitializeSplitter()
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As SplitterEventArgs) Handles SplitContainer1.SplitterMoved

        Dim idx As Integer
        If ((_maxHeight > 0) And ((Me.SplitContainer1.Height - e.SplitY) > _maxHeight)) Then
            ' Limit to max height
            Me.SplitContainer1.SplitterDistance = Me.SplitContainer1.Height - _maxHeight

            ' Set to max
            _visibleCount = Me.StackStripEnabledItemCount
        ElseIf ((_visibleCount >= 0) And (Me.StackStripEnabledItemCount > 0)) Then
            'Make sure overflow is correct
            _visibleCount = CInt((Me.StackStrip.Height / Me.StackStrip.ItemHeight)) - 1

            ' See if me changed
            If (_lastCount <> _visibleCount) Then

                Dim count As Integer = Me.OverFlowStrip.Items.Count

                ' Update overflow items
                For idx = 0 To count - 1
                    Me.OverFlowStrip.Items(idx).Visible = (idx < (count - _visibleCount))
                Next

                ' Update last
                _lastCount = _visibleCount
            End If
        End If
    End Sub

    Private Sub InitializeSplitter()
        ' Set slider increment
        If (Me.StackStrip.ItemHeight > 0) Then
            Me.SplitContainer1.SplitterIncrement = Me.StackStrip.ItemHeight

            ' Find visible count
            If (_visibleCount < 0) Then
                _visibleCount = Me.StackStripEnabledItemCount
            End If

            ' Setup BaseStackStrip
            Me.OverFlowStrip.Height = Me.StackStrip.ItemHeight


            ' Set splitter distance
            Dim min As Integer = Me.StackStrip.ItemHeight + Me.OverFlowStrip.Height
            Dim distance As Integer = Me.SplitContainer1.Height - Me.SplitContainer1.SplitterWidth - ((_visibleCount * Me.StackStrip.ItemHeight) + Me.OverFlowStrip.Height)

            ' Set Max
            _maxHeight = (Me.StackStrip.ItemHeight * Me.StackStripEnabledItemCount) + Me.OverFlowStrip.Height + Me.SplitContainer1.SplitterWidth

            ' In case it's sized too small on startup
            If (distance < 0) Then
                distance = min
            End If

            ' Set Min/Max
            Me.SplitContainer1.SplitterDistance = distance
            Me.SplitContainer1.Panel1MinSize = min
        End If
    End Sub

#End Region

#Region "Control Contenido de la barra de navegación "


    Public Sub Inicializar()
        Dim I, J As Integer
        Dim tnCategoria As TreeNode

        dsSesion = FrameworkApp.Sesion.dsSesion
        FrameworkApp.Sesion.LoadInfoInicioSesion()
        If dsSesion.Tables("Categorias").Rows.Count = 0 Then
            Exit Sub
        End If
        tnCategoria = GetNodoCategoria(dsSesion.Tables("Categorias").Rows(I))
        Dim rows() As DataRow
        Dim rowCategoria As DataRow
        rows = dsSesion.Tables("Categorias").Select("IdCategoriaPadre IS NULL", "Orden")
        Dim rowModulo() As DataRow
        Dim rwCategoriaHija() As DataRow
        Me.StackStrip.Items.Clear()
        For I = 0 To rows.Length - 1
            rowCategoria = CType(rows(I), DataRow)
            rowModulo = dsSesion.Tables("Modulos").Select("IdCategoria = " & rowCategoria("IdCategoria").ToString(), "Nombre")
            Dim bInsertar As Boolean = False
            If rowModulo.Length = 0 Then
                rwCategoriaHija = dsSesion.Tables("Categorias").Select("IdCategoriaPadre = " & rowCategoria("IdCategoria").ToString(), "Orden")
                For Each rw As DataRow In rwCategoriaHija
                    rowModulo = dsSesion.Tables("Modulos").Select("IdCategoria = " & rw("IdCategoria").ToString(), "Nombre")
                    If rowModulo.Length > 0 Then
                        bInsertar = True
                        Exit For
                    End If
                Next
            End If
            If rowModulo.Length > 0 Or bInsertar Then
                'CREAR TOOLSTRIPBUTON
                StackStrip.ImageList = Me.ImageList1
                If I = 0 Then
                    Dim tsCategoria As New ToolStripButton(rowCategoria("Nombre").ToString)
                    If (_listImages IsNot Nothing) Then
                        tsCategoria.ImageIndex = CalcularIndiceImagen(CInt(rowCategoria("IdCategoria")))
                    Else
                        tsCategoria.ImageIndex = 0
                    End If
                    tsCategoria.Tag = rowCategoria
                    tsCategoria.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
                    tsCategoria.Alignment = ToolStripItemAlignment.Left
                    tsCategoria.ImageAlign = ContentAlignment.MiddleLeft
                    tsCategoria.TextAlign = ContentAlignment.MiddleLeft
                    tsCategoria.Margin = New Padding(0)
                    Me.StackStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {tsCategoria})
                Else
                    Dim tsCategoria2 As New ToolStripButton(rowCategoria("Nombre").ToString)
                    If (_listImages IsNot Nothing) Then
                        tsCategoria2.ImageIndex = CalcularIndiceImagen(CInt(rowCategoria("IdCategoria")))
                    Else
                        tsCategoria2.ImageIndex = 0
                    End If
                    tsCategoria2.Tag = rowCategoria
                    tsCategoria2.Alignment = ToolStripItemAlignment.Left
                    tsCategoria2.ImageAlign = ContentAlignment.MiddleLeft
                    tsCategoria2.TextAlign = ContentAlignment.MiddleLeft
                    tsCategoria2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
                    tsCategoria2.Margin = New Padding(0)
                    Me.StackStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {tsCategoria2})
                End If
            End If
        Next

        For J = 0 To StackStrip.ItemCount - 1
            If StackStrip.Items(J).Tag Is Nothing Then
                StackStrip.Items(J).Enabled = False
                StackStrip.Items(J).Visible = False
            Else
                StackStrip.Items(J).Enabled = True
                StackStrip.Items(J).Visible = True
            End If
        Next
        InitializeSplitter()
        SplitContainer1.SplitterDistance = 0

        ' Add Overflow items
        AddOverflowItems()
        BotonCategoriaRaiz_Click(StackStrip.Items(0), Nothing)
    End Sub

    'FUNCION PARA CALCULAR EL INDICE DE LA IMAGEN DE LA CATEGORIA
    Private Function CalcularIndiceImagen(ByVal IdCategoria As Integer) As Integer
        If _listImages Is Nothing Then
            Return 0
        End If
        For Each index As String In _listImages
            If index.StartsWith(IdCategoria.ToString() + "-") Then
                Return CInt(index.Substring(index.IndexOf("-") + 1))
            End If
        Next
        Return 0
    End Function

    'FUNCION PARA CALCULAR EL INDICE DE LA IMAGEN DE LA CATEGORIA
    Private Function CalcularIndiceImagenModulo(ByVal IdModulo As Integer) As Integer
        If _listImages Is Nothing Then
            Return 0
        End If
        For Each index As String In _listImages
            If index.StartsWith("M" + IdModulo.ToString() + "-") Then
                Return CInt(index.Substring(index.IndexOf("-") + 1))
            End If
        Next
        Return 0
    End Function


    Private Function GetNodoCategoria(ByVal rowCategoria As DataRow) As TreeNode
        ' Se agrega cada categoria, En la consulta, primero vienen las categorias sin padres
        Dim I, J As Integer
        Dim tnCategoria, tnCategoriaHija, tnModulo As TreeNode
        Dim rows() As DataRow
        Dim rowModulo As DataRow


        ' TODO: Indicar imagenes para los diferentes nodos del treeview
        tnCategoria = New TreeNode(CStr(rowCategoria("Nombre")))
        tnCategoria.Text = CStr(rowCategoria("Nombre"))
        tnCategoria.Name = CStr(rowCategoria("Nombre"))
        tnCategoria.Tag = rowCategoria

        ' Agregar los modulos que pertenecen a la categoria
        rows = dsSesion.Tables("Modulos").Select("IdCategoria = " & rowCategoria("IdCategoria").ToString(), "Nombre")
        If rows.Length > 0 Then
            For J = 0 To rows.Length - 1
                rowModulo = CType(rows(J), DataRow)
                ' TODO: Indicar imagenes para los diferentes nodos del treeview
                Dim iIndiceImagen As Integer = Me.CalcularIndiceImagenModulo(CInt(rowModulo("IdModulo")))
                tnModulo = New TreeNode(CStr(rowModulo("Nombre")), iIndiceImagen, iIndiceImagen)
                tnModulo.Tag = rowModulo
                tnCategoria.Nodes.Add(tnModulo)
            Next
        End If

        ' Agregar las categorias hijas
        rows = dsSesion.Tables("Categorias").Select("IdCategoriaPadre = " & rowCategoria("IdCategoria").ToString(), "Orden")
        For I = 0 To rows.Length - 1
            rowCategoria = CType(rows(I), DataRow)
            tnCategoriaHija = GetNodoCategoria(rowCategoria)
            Dim iIndiceImagen As Integer = Me.CalcularIndiceImagen(CInt(rowCategoria("IdCategoria")))
            tnCategoriaHija.ImageIndex = iIndiceImagen
            tnCategoriaHija.SelectedImageIndex = iIndiceImagen
            If tnCategoriaHija.Nodes.Count > 0 Then
                tnCategoria.Nodes.Add(tnCategoriaHija)
            End If
        Next

        Return tnCategoria
    End Function

    Private Sub NavigationPanelControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Setup
        AddHandler Me.StackStrip.ItemHeightChanged, AddressOf stackStrip1_ItemHeightChanged
        ' Add Overflow items
        'AddOverflowItems()
        'Set Height
        InitializeSplitter()
        ' Set parent padding
        Me.Parent.Padding = New Padding(0, 0, 0, 0)
    End Sub

    Private Sub BotonCategoriaRaiz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As ToolStripButton = CType(sender, ToolStripButton)
        If btn.Text = "" Then
            lblGrupoActual.Text = btn.ToolTipText
        Else
            lblGrupoActual.Text = btn.Text
        End If
        tvModulos.Nodes.Clear()
        tvModulos.ImageList = Me.ImageList1
        btn.Checked = True
        If btn.Tag IsNot Nothing Then
            If TypeOf btn.Tag Is DataRow Then
                Dim rowCategoria As DataRow
                rowCategoria = CType(btn.Tag, DataRow)
                If rowCategoria IsNot Nothing Then
                    Dim tn As TreeNode
                    tn = GetNodoCategoria(rowCategoria)
                    For I As Integer = 0 To tn.Nodes.Count - 1
                        tvModulos.Nodes.Add(tn.Nodes(I))
                    Next
                    If tvModulos.Nodes.Count > 0 Then
                        tvModulos.SelectedNode = tvModulos.Nodes(0)
                    End If
                End If
            End If
        End If

        If FrameworkApp.ExpanderArbol Then
            tvModulos.ExpandAll()
        End If
    End Sub

#End Region


    Private Sub tvModulos_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvModulos.AfterSelect
        'If TypeOf e.Node.Tag Is DataRow Then
        'RaiseEvent SelectionChanged(CType(e.Node.Tag, DataRow))
        'End If
        RaiseEvent SelectionChanged(CType(e.Node.Tag, DataRow))
    End Sub

    Private Sub tvModulos_BeforeSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvModulos.BeforeSelect
        'e.Cancel = True
        'If TypeOf e.Node.Tag Is DataRow Then
        'e.Cancel = True
        'End If
    End Sub

    Public Property Arbol() As TreeView
        Get
            Return tvModulos
        End Get
        Set(ByVal value As TreeView)
            tvModulos = value
        End Set
    End Property

End Class
