Imports system.Windows

Friend Class SelecionarCategoriaDialog

    Private Datos As GestorConfiguracion
    Private m_SelectedRow As DataRow

    Public Shared Function Run(ByVal Datos As GestorConfiguracion) As DataRow
        Dim row As DataRow = Nothing
        Dim frm As New SelecionarCategoriaDialog
        frm.Datos = Datos
        If frm.ShowDialog() = DialogResult.OK Then
            row = frm.m_SelectedRow
        End If
        frm.Dispose()
        Return row
    End Function

    Private Class NodeSorter
        Implements IComparer

        ' Compare the length of the strings, or the strings
        ' themselves, if they are the same length.
        Public Function Compare(ByVal x As Object, ByVal y As Object) _
            As Integer Implements IComparer.Compare
            Dim tx As TreeNode = CType(x, TreeNode)
            Dim ty As TreeNode = CType(y, TreeNode)
            Dim rx As ConfiguracionDataSet.CategoriasRow = CType(tx.Tag, ConfiguracionDataSet.CategoriasRow)
            Dim ry As ConfiguracionDataSet.CategoriasRow = CType(ty.Tag, ConfiguracionDataSet.CategoriasRow)
            If rx Is Nothing Then
                Return -2
            ElseIf ry Is Nothing Then
                Return 2
            Else
                Return rx.Orden - ry.Orden
            End If
        End Function
    End Class

    Private Sub SelecionarCategoriaForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FillTreeViewCategorias()
    End Sub

    Private Sub FillTreeViewCategorias()
        Dim tnCategoria As TreeNode
        Dim I As Integer

        Dim rows() As DataRow
        rows = Datos.dsConfig.Tables("Categorias").Select("IdCategoriaPadre IS NULL")
        For I = 0 To rows.Length - 1
            tnCategoria = GetNodoCategoria(CType(rows(I), DataRow))
            tvCategorias.Nodes.Add(tnCategoria)
        Next
        tvCategorias.Sort()
        'tvCategorias.TreeViewNodeSorter = Nothing
    End Sub

    Private Function GetNodoCategoria(ByVal rowCategoria As DataRow) As TreeNode
        ' Se agrega cada categoria, En la consulta, primero vienen las categorias sin padres
        Dim I As Integer
        Dim tnCategoria, tnCategoriaHija As TreeNode
        Dim rows() As DataRow


        ' TODO: Indicar imagenes para los diferentes nodos del treeview
        tnCategoria = New TreeNode(CStr(rowCategoria("Nombre")))
        tnCategoria.Text = CStr(rowCategoria("Nombre"))
        tnCategoria.Name = CStr(rowCategoria("Nombre"))
        tnCategoria.Tag = rowCategoria

        ' Agregar las categorias hijas
        rows = Datos.dsConfig.Tables("Categorias").Select("IdCategoriaPadre = " & rowCategoria("IdCategoria").ToString(), "Nombre")
        For I = 0 To rows.Length - 1
            rowCategoria = CType(rows(I), ConfiguracionDataSet.CategoriasRow)
            tnCategoriaHija = GetNodoCategoria(rowCategoria)
            If tnCategoriaHija IsNot Nothing Then
                tnCategoria.Nodes.Add(tnCategoriaHija)
            End If
        Next
        Return tnCategoria
    End Function


    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If tvCategorias.SelectedNode IsNot Nothing Then
            m_SelectedRow = CType(tvCategorias.SelectedNode.Tag, DataRow)
            DialogResult = DialogResult.OK
        Else
            ShowError("No hay ninguna categoria seleccionada")
        End If
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        m_SelectedRow = Nothing
        DialogResult = DialogResult.Cancel
    End Sub
End Class