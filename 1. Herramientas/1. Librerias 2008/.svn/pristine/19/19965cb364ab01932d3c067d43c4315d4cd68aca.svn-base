Friend Class ConfigurarModulosForm

    Private Datos As New GestorConfiguracion
    Private m_rowCurrentCategoria As DataRow = Nothing

    Private Class NodeSorter
        Implements IComparer

        ' Compare the length of the strings, or the strings
        ' themselves, if they are the same length.
        Public Function Compare(ByVal x As Object, ByVal y As Object) _
            As Integer Implements IComparer.Compare
            Dim tx As TreeNode = CType(x, TreeNode)
            Dim ty As TreeNode = CType(y, TreeNode)
            Dim rx As DataRow = CType(tx.Tag, DataRow)
            Dim ry As DataRow = CType(ty.Tag, DataRow)
            If rx Is Nothing Then
                Return -2
            ElseIf ry Is Nothing Then
                Return 2
            Else
                Return CInt(rx("Orden")) - CInt(ry("Orden"))
            End If
        End Function
    End Class

    Private Shared m_frmModulos As ConfigurarModulosForm

    Public Shared Sub Run()
        If m_frmModulos Is Nothing Then
            m_frmModulos = New ConfigurarModulosForm
            m_frmModulos.MdiParent = MainConfiguracionForm.Current
        End If
        'CREAR EL GESTOR
        m_frmModulos.Show()
        m_frmModulos.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub ConfigurarModulosForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If dsConfig.HasChanges() Then
            Datos.BeginTransaction()
            Try
                Datos.UpdatePermisosEspeciales(DataRowState.Deleted)
                Datos.UpdatePermisosEspeciales(DataRowState.Modified)
                Datos.UpdateModulos(DataRowState.Deleted)
                Datos.UpdateModulos(DataRowState.Modified)
                Datos.UpdateCategorias(DataRowState.Added)
                Datos.UpdateModulos(DataRowState.Added)
                Datos.CommitTransaction()
                Datos.BeginTransaction()
                Datos.UpdatePermisosEspeciales(DataRowState.Added)
                Datos.UpdateCategorias(DataRowState.Modified)
                Datos.UpdateCategorias(DataRowState.Deleted)
                Datos.CommitTransaction()
            Catch ex As Exception
                Datos.RollbackTransaction()
                ShowError(ex.Message)
            End Try
        End If
        m_frmModulos = Nothing
    End Sub

    Private Sub ConfigurarModulos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dsConfig = Datos.dsConfig
        Datos.LoadCategorias()
        Datos.LoadProgramas()
        Datos.LoadTiposModulos()
        Datos.LoadModulos()
        Datos.LoadPermisosEspeciales()
        Me.bsModulos.DataSource = dsConfig
        Me.bsModulos.DataMember = "Modulos"
        FillTreeViewCategorias()
    End Sub

    Private Sub FillTreeViewCategorias()
        Dim tnCategoria As TreeNode
        Dim I As Integer

        tvCategorias.Nodes.Clear()
        tvCategorias.TreeViewNodeSorter = New NodeSorter()
        tnCategoria = tvCategorias.Nodes.Add("(Sin categoria)")
        tnCategoria.ImageIndex = 1
        tnCategoria.SelectedImageIndex = 1

        'tnCategoria.SelectedImageIndex = -1
        Dim rows() As DataRow
        rows = dsConfig.Tables("Categorias").Select("IdCategoriaPadre IS NULL")
        For I = 0 To rows.Length - 1
            tnCategoria = GetNodoCategoria(CType(rows(I), DataRow))
            tvCategorias.Nodes.Add(tnCategoria)
        Next
        tvCategorias.Sort()
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
        rows = dsConfig.Tables("Categorias").Select("IdCategoriaPadre = " & rowCategoria("IdCategoria").ToString(), "Nombre")
        For I = 0 To rows.Length - 1
            rowCategoria = CType(rows(I), ConfiguracionDataSet.CategoriasRow)
            tnCategoriaHija = GetNodoCategoria(rowCategoria)
            If tnCategoriaHija IsNot Nothing Then
                tnCategoria.Nodes.Add(tnCategoriaHija)
            End If
        Next
        Return tnCategoria
    End Function

    Private Sub menuAgregarCategoriaRaiz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAgregarCategoriaRaiz.Click
        AgregarNodo(Nothing)
    End Sub

    Private Sub menuAgregarCategoriaHijo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAgregarCategoriaHijo.Click
        Dim tn As TreeNode = GetSelectedNode()
        If tn IsNot Nothing Then
            AgregarNodo(tn)
        End If
    End Sub

    Private Function GetSelectedNode(Optional ByVal bShowError As Boolean = True) As TreeNode
        Dim tn As TreeNode = tvCategorias.SelectedNode
        If tn Is Nothing Then
            If bShowError Then
                ShowError("Debe seleccionar un nodo padre, para agregar la nueva categoria")
            End If
            Return Nothing
        End If
        If tn.Tag Is Nothing Then
            If bShowError Then
                ShowError("Debe seleccionar un nodo padre, para agregar la nueva categoria")
            End If
            Return Nothing
        End If
        Return tn
    End Function

    Private Sub AgregarNodo(ByVal Parent As TreeNode)
        Dim sCategoria As String
        Dim row, rowParent As DataRow
        sCategoria = InputBox("Por favor especifique el nombre de la nueva categoria:", "Nueva Categoría!")
        If Not DBUtils.IsNullOrEmpty(sCategoria) Then
            row = dsConfig.Tables("Categorias").NewRow
            row("Nombre") = sCategoria
            Dim tnCategoria As New TreeNode(CStr(row("Nombre")))
            tnCategoria.Name = CStr(row("Nombre"))
            tnCategoria.Tag = row
            If Parent Is Nothing Then
                row("Orden") = tvCategorias.Nodes.Count + 1
                tvCategorias.Nodes.Add(tnCategoria)
                tvCategorias.SelectedNode = tnCategoria
            Else
                rowParent = CType(Parent.Tag, DataRow)
                row("Orden") = Parent.Nodes.Count + 1
                row("IdCategoriaPadre") = rowParent("IdCategoria")
                Parent.Nodes.Add(tnCategoria)
                Parent.Expand()
                tvCategorias.SelectedNode = Parent
            End If
            dsConfig.Tables("Categorias").Rows.Add(row)
        End If
    End Sub

    Private Sub btnBorrarCategoria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrarCategoria.Click
        Dim tn As TreeNode = GetSelectedNode()
        If tn IsNot Nothing Then
            If Confirmar("Esta seguro de borrar la categoria seleccionada?") Then
                BorrarCategoria(tn)
                tn.Remove()
            End If
        End If
    End Sub

    Private Sub BorrarCategoria(ByVal tn As TreeNode)
        Dim I As Integer
        If tn.Nodes.Count > 0 Then
            For I = 0 To tn.Nodes.Count - 1
                BorrarCategoria(tn.Nodes(I))
            Next
            tn.Nodes.Clear()
        End If
        If tn.Tag IsNot Nothing Then
            Dim row As DataRow
            row = CType(tn.Tag, DataRow)
            row.Delete()
        End If
    End Sub

    Private Sub btnSubirCategoria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubirCategoria.Click
        Dim tn As TreeNode
        Dim Parent As TreeNode
        Dim nIndex As Integer
        tn = GetSelectedNode(False)
        If tn IsNot Nothing Then
            Parent = tn.Parent
            nIndex = tn.Index
            If (Parent Is Nothing AndAlso nIndex > 1) _
            OrElse (Parent IsNot Nothing AndAlso nIndex > 0) Then
                Dim row, rowPrev As DataRow
                row = CType(tn.Tag, DataRow)
                rowPrev = CType(tn.PrevNode.Tag, DataRow)
                row("Orden") = rowPrev("Orden")
                rowPrev("Orden") = CInt(row("Orden")) + 1
                tn.Remove()
                If Parent Is Nothing Then
                    tvCategorias.Nodes.Insert(nIndex - 1, tn)
                Else
                    Parent.Nodes.Insert(nIndex - 1, tn)
                End If
                tvCategorias.SelectedNode = tn
            End If
        End If
    End Sub

    Private Sub btnBajarCategoria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBajarCategoria.Click
        Dim tn As TreeNode
        Dim Parent As TreeNode
        Dim nIndex As Integer
        tn = GetSelectedNode(False)
        If tn IsNot Nothing Then
            Parent = tn.Parent
            nIndex = tn.Index
            If (Parent Is Nothing AndAlso nIndex < tvCategorias.Nodes.Count - 1) _
            OrElse (Parent IsNot Nothing AndAlso nIndex < Parent.Nodes.Count - 1) Then
                Dim row, rowNext As DataRow
                row = CType(tn.Tag, DataRow)
                rowNext = CType(tn.NextNode.Tag, DataRow)
                row("Orden") = rowNext("Orden")
                rowNext("Orden") = CInt(row("Orden")) - 1
                tn.Remove()
                If Parent Is Nothing Then
                    tvCategorias.Nodes.Insert(nIndex + 1, tn)
                Else
                    Parent.Nodes.Insert(nIndex - 1, tn)
                End If
                tvCategorias.SelectedNode = tn
            End If
        End If
    End Sub

	Private Sub btnAgregarModulo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarModulo.Click
		Dim dt As DataTable = dsConfig.Tables("Categorias").GetChanges(System.Data.DataRowState.Added)

		If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
			ShowError("Debe Guardar los cambios realizados")
		Else
			EditarModuloDialog.Run(Datos, m_rowCurrentCategoria)
		End If

	End Sub

    Private Sub tvCategorias_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvCategorias.AfterSelect
        Dim tn As TreeNode = GetSelectedNode(False)
        If tn Is Nothing OrElse tn.Tag Is Nothing Then
            m_rowCurrentCategoria = Nothing
            bsModulos.Filter = "IDCATEGORIA IS NULL"
        Else
            m_rowCurrentCategoria = CType(tn.Tag, DataRow)
            bsModulos.Filter = "IDCATEGORIA = " & m_rowCurrentCategoria("IdCategoria").ToString
        End If
    End Sub

    Private Sub btnCambiarCategoria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCambiarCategoria.Click
        Dim rowModulo As DataRow
        Dim rowCategoria As DataRow
        If dgvModulos.SelectedRows.Count > 0 Then
            rowCategoria = SelecionarCategoriaDialog.Run(Datos)
            If rowCategoria IsNot Nothing Then
                For I As Integer = 0 To dgvModulos.SelectedRows.Count - 1
                    rowModulo = CType(dgvModulos.SelectedRows(0).DataBoundItem, DataRowView).Row
                    rowModulo("IdCategoria") = rowCategoria("IdCategoria")
                Next
            End If
        End If
    End Sub

    Private Sub btnEditarModulo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarModulo.Click
        Dim rowModulo As DataRow
        rowModulo = DBUtils.GetCurrentRow(bsModulos)
        If rowModulo IsNot Nothing Then
            EditarModuloDialog.Run(Datos, m_rowCurrentCategoria, rowModulo)
        End If
    End Sub

    Private Sub btnEliminarModulo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarModulo.Click
        Dim rowModulo As DataRow
        rowModulo = DBUtils.GetCurrentRow(bsModulos)
        If rowModulo IsNot Nothing Then
            If Confirmar("Esta seguro de borrar el Módulo seleccionado?") Then
                bsModulos.RemoveCurrent()
            End If
        End If
    End Sub

    Private Sub btnEditarCategoria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarCategoria.Click
        Dim tn As TreeNode = GetSelectedNode()
        If tn IsNot Nothing Then
            Dim rowCategoria As DataRow
            Dim sCategoria As String
            rowCategoria = CType(tn.Tag, DataRow)
            sCategoria = InputBox("Indique el nuevo nombre para la categoría:", "Modificar Categorìa", CStr(rowCategoria("Nombre")))
            If Not DBUtils.IsNullOrEmpty(sCategoria) Then
                rowCategoria("Nombre") = sCategoria
                tn.Text = sCategoria
            End If
        End If
    End Sub

	Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
		Try
			Datos.BeginTransaction()
			Try
				Datos.UpdatePermisosEspeciales(DataRowState.Deleted)
				Datos.UpdatePermisosEspeciales(DataRowState.Modified)
				Datos.UpdateModulos(DataRowState.Deleted)
				Datos.UpdateModulos(DataRowState.Modified)
				Datos.UpdateCategorias(DataRowState.Added)
				Datos.UpdateModulos(DataRowState.Added)
				Datos.CommitTransaction()
				Datos.BeginTransaction()
				Datos.UpdatePermisosEspeciales(DataRowState.Added)
				Datos.UpdateCategorias(DataRowState.Modified)
				Datos.UpdateCategorias(DataRowState.Deleted)
				Datos.CommitTransaction()
			Catch ex As Exception
				Datos.RollbackTransaction()
				ShowError(ex.Message)
			End Try

			dsConfig.AcceptChanges()

			Datos.dsConfig.Clear()

			Datos.LoadCategorias()
			Datos.LoadProgramas()
			Datos.LoadTiposModulos()
			Datos.LoadModulos()
			Datos.LoadPermisosEspeciales()

			FillTreeViewCategorias()
		Catch ex As Exception
			ShowError(ex.Message)
		End Try
	End Sub
End Class

