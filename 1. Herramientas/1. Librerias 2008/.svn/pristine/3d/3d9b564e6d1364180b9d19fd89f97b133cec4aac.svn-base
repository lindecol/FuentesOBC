Friend Class EditarPerfilDialog

    Private Datos As GestorSeguridad
    Private RowPerfil As DataRow
    Private NuevoPerfil As Boolean = False
    Private m_bCargarCombos As Boolean = False

    Public Shared Sub Nuevo(ByVal Datos As GestorSeguridad)
        Dim frm As New EditarPerfilDialog
        frm.Datos = Datos
        frm.NuevoPerfil = True
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Public Shared Sub Editar(ByVal Datos As GestorSeguridad, ByVal rowPerfil As DataRow)
        Dim frm As New EditarPerfilDialog
        frm.Datos = Datos
        frm.RowPerfil = rowPerfil
        frm.NuevoPerfil = False
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub EditarPerfilDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dsSeguridad = Datos.dsSeguridad
        bsPerfiles.DataSource = dsSeguridad
        Me.bsPerfiles.DataMember = "Perfiles"
        bsPermisosModulo.DataSource = dsSeguridad
        Me.bsPermisosModulo.DataMember = "PermisosModulo"
        Me.bsPermisosModulo.Sort = "orden ASC, Nombre ASC"
        bsProgramas.DataSource = dsSeguridad
        Me.bsProgramas.DataMember = "Programas"
        Me.bsProgramas.Sort = "IdPrograma"
        LoadData()
        bsPerfiles.AllowNew = True
        If Me.RowPerfil Is Nothing Then
            bsPerfiles.AddNew()
            RowPerfil = CType(DBUtils.GetCurrentRow(bsPerfiles), DataRow)
            RowPerfil("Activo") = True
            RowPerfil("Sistema") = False
            RowPerfil("Nombre") = "Nombre nuevo perfil"
            bsPerfiles.ResetCurrentItem()
            Me.Text = "Nuevo perfil de usuario"
        Else
            Me.Text = "Editando perfil de usuario"
            DBUtils.SetCurrentRow(bsPerfiles, RowPerfil)
        End If
        ShowPermisosPerfil()
        Me.ActivoCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.bsPerfiles, "Activo", True))
        Me.NombreTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsPerfiles, "Nombre", True))
        m_bCargarCombos = True
    End Sub

    Private Sub LoadData()
        cbPrograma.Enabled = False
        If dsSeguridad.Tables("Programas").Rows.Count = 0 Then
            Datos.LoadProgramas()
            dsSeguridad.Tables("Programas").Rows.Add(0, "(Modulos del sistema)", "")
        End If
        If dsSeguridad.Tables("Modulos").Rows.Count = 0 Then
            Datos.LoadCategorias()
            Datos.LoadModulos()
            Datos.LoadPermisosEspeciales()
        End If
        If Not Me.NuevoPerfil Then
            Datos.LoadModulosPerfil(RowPerfil)
        Else
            dsSeguridad.Tables("ModulosPerfil").Rows.Clear()
        End If
        cbPrograma.Enabled = True
        cbPrograma.SelectedIndex = 0
    End Sub

    Private Sub ShowPermisosPerfil()
        bsPermisosModulo.SuspendBinding()
        dsSeguridad.Tables("PermisosModulo").Rows.Clear()
        Dim rowsModulos() As DataRow
        Dim rowModulo As DataRow
        Dim rowModuloPerfil As DataRow
        If cbPrograma.SelectedIndex = 0 Then
            rowsModulos = dsSeguridad.Tables("Modulos").Select("IdPrograma IS NULL")
        Else
            Dim IdPrograma As Integer = CInt(cbPrograma.SelectedValue)
            rowsModulos = dsSeguridad.Tables("Modulos").Select("IdPrograma = " & IdPrograma.ToString())
        End If
        Dim sNombreCategoria As String = ""
        Dim iOrden As Integer = 0
        For I As Integer = 0 To rowsModulos.Length - 1
            rowModulo = CType(rowsModulos(I), DataRow)
            rowModuloPerfil = dsSeguridad.Tables("ModulosPerfil").Rows.Find(New Object() {RowPerfil("IdPerfil"), rowModulo("IdModulo")})

            Dim rwCategorias As DataRow = Nothing

            If Not rowModulo("IdCategoria").Equals(System.DBNull.Value) Then
                rwCategorias = dsSeguridad.Tables("Categorias").Rows.Find(CStr(rowModulo("IdCategoria")))
            End If

            If rwCategorias IsNot Nothing Then
                sNombreCategoria = CStr(rwCategorias("Nombre"))
                iOrden = CInt(rwCategorias("Orden"))
            Else
                sNombreCategoria = "Sin Categoria"
                iOrden = 0
            End If

            If rowModuloPerfil Is Nothing Then
                dsSeguridad.Tables("PermisosModulo").Rows.Add(rowModulo("IdModulo"), _
                    rowModulo("Nombre"), False, False, False, False, False, "", sNombreCategoria, iOrden)
            Else
                dsSeguridad.Tables("PermisosModulo").Rows.Add(rowModulo("IdModulo"), _
                    rowModulo("Nombre"), True, rowModuloPerfil("PermitirInsertar"), rowModuloPerfil("PermitirModificar"), _
                    rowModuloPerfil("PermitirBorrar"), rowModuloPerfil("PermitirImprimir"), rowModuloPerfil("PermisosEspeciales"), sNombreCategoria, iOrden)
            End If
        Next
        bsPermisosModulo.ResumeBinding()
    End Sub

    ' Actualiza los permisos del perfil de acuerdo a los datos del DataGrid
    Private Sub UpdatePermisosPerfil()
        Dim rowPermiso As DataRow
        Dim rowModuloPerfil As DataRow

        For I As Integer = 0 To dsSeguridad.Tables("PermisosModulo").Rows.Count - 1
            rowPermiso = dsSeguridad.Tables("PermisosModulo").Rows(I)
            If CBool(rowPermiso("PermitirAcceso")) Then
                ' Si no esta se inserta, si esta se actualiza
                rowModuloPerfil = dsSeguridad.Tables("ModulosPerfil").Rows.Find( _
                    New Object() {RowPerfil("IdPerfil"), rowPermiso("IdModulo")})
                If rowModuloPerfil Is Nothing Then
                    rowModuloPerfil = dsSeguridad.Tables("ModulosPerfil").NewRow
                    rowModuloPerfil("IdModulo") = rowPermiso("IdModulo")
                    rowModuloPerfil("IdPerfil") = RowPerfil("IdPerfil")
                    rowModuloPerfil("PermitirInsertar") = rowPermiso("PermiteInsertar")
                    rowModuloPerfil("PermitirModificar") = rowPermiso("PermitirModificar")
                    rowModuloPerfil("PermitirBorrar") = rowPermiso("PermitirBorrar")
                    rowModuloPerfil("PermitirImprimir") = rowPermiso("PermitirImprimir")
                    rowModuloPerfil("PermisosEspeciales") = rowPermiso("PermisosEspeciales")
                    dsSeguridad.Tables("ModulosPerfil").Rows.Add(rowModuloPerfil)
                Else
                    rowModuloPerfil("PermitirInsertar") = rowPermiso("PermiteInsertar")
                    rowModuloPerfil("PermitirModificar") = rowPermiso("PermitirModificar")
                    rowModuloPerfil("PermitirBorrar") = rowPermiso("PermitirBorrar")
                    rowModuloPerfil("PermitirImprimir") = rowPermiso("PermitirImprimir")
                    rowModuloPerfil("PermisosEspeciales") = rowPermiso("PermisosEspeciales")
                End If
            Else
                ' Si ya esta se elimina
                rowModuloPerfil = dsSeguridad.Tables("ModulosPerfil").Rows.Find( _
                    New Object() {RowPerfil("IdPerfil"), rowPermiso("IdModulo")})
                If rowModuloPerfil IsNot Nothing Then
                    rowModuloPerfil.Delete()
                End If
            End If
        Next
    End Sub

    Private Sub cbPrograma_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPrograma.SelectionChangeCommitted
        If dsSeguridad.Tables("PermisosModulo").Rows.Count > 0 Then
            UpdatePermisosPerfil()
        End If
        ShowPermisosPerfil()
    End Sub

    Private Sub dgvPermisosModulos_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPermisosModulos.CellContentDoubleClick
        If e.ColumnIndex = 0 And e.RowIndex >= 0 Then
            ' Se cambian las propieadades de toda la fila
            Dim row As DataRow
            row = CType(CType(dgvPermisosModulos.Rows(e.RowIndex).DataBoundItem, DataRowView).Row, DataRow)
            Dim bNuevoValor As Boolean = Not CBool(row("PermitirAcceso"))
            row("PermitirAcceso") = bNuevoValor
            row("PermiteInsertar") = bNuevoValor
            row("PermitirBorrar") = bNuevoValor
            row("PermitirImprimir") = bNuevoValor
            row("PermitirModificar") = bNuevoValor
            UpdateRowValues(e.RowIndex)
        ElseIf e.ColumnIndex > 0 And e.RowIndex = -1 Then
            ' Se debe cambiar las propiedades de toda la columna
            Dim row As DataRow
            Dim sColumName As String = dgvPermisosModulos.Columns(e.ColumnIndex).DataPropertyName
            row = CType(CType(dgvPermisosModulos.Rows(0).DataBoundItem, DataRowView).Row, DataRow)

            If row(sColumName).GetType().Name.Equals("Boolean") Then



                Dim bNuevoValor As Boolean = Not CBool(row(sColumName))
                For I As Integer = 0 To dgvPermisosModulos.RowCount - 1
                    row = CType(CType(dgvPermisosModulos.Rows(I).DataBoundItem, DataRowView).Row, DataRow)
                    row(sColumName) = bNuevoValor
                Next
                bsPerfiles.ResetBindings(False)

            End If

        End If
    End Sub

    Private Sub dgvPermisosModulos_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPermisosModulos.CellValueChanged
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then
            If e.ColumnIndex = 1 Then ' Columna Permitir Acceso
                dgvPermisosModulos.EndEdit()
                Dim row As DataRow
                row = CType(CType(dgvPermisosModulos.Rows(e.RowIndex).DataBoundItem, DataRowView).Row, DataRow)
                If Not CBool(row("PermitirAcceso")) Then
                    row("PermiteInsertar") = False
                    row("PermitirBorrar") = False
                    row("PermitirImprimir") = False
                    row("PermitirModificar") = False
                    row("PermisosEspeciales") = ""
                    UpdateRowValues(e.RowIndex)
                End If
            End If
        End If
    End Sub

    Private Sub UpdateRowValues(ByVal nRowIndex As Integer)
        For I As Integer = 0 To dgvPermisosModulos.ColumnCount - 1
            dgvPermisosModulos.UpdateCellValue(I, nRowIndex)
        Next
    End Sub

#Region "Código relativo al manejo de permisos especiales"

    Private Sub bsPermisosModulo_CurrentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsPermisosModulo.CurrentChanged
        If m_bCargarCombos Then
            LoadPermisosEspecialesModulo()
        End If
    End Sub

    Private Sub LoadPermisosEspecialesModulo()
        Dim IdModulo As Integer
        Dim rowsPermisos() As DataRow
        Dim rowPermiso As DataRow
        Dim Permiso As PermisoEspecial
        lstPermisosEspeciales.Items.Clear()
        IdModulo = CInt(DBUtils.GetCurrentRow(bsPermisosModulo)("IdModulo"))
        rowsPermisos = dsSeguridad.Tables("PermisosEspeciales").Select("IdModulo = " & IdModulo.ToString())
        Dim PermisosActuales As List(Of Integer) = GetPermisosActuales()
        For I As Integer = 0 To rowsPermisos.Length - 1
            rowPermiso = CType(rowsPermisos(I), DataRow)
            Permiso = New PermisoEspecial(CInt(rowPermiso("IdPermiso")), CStr(rowPermiso("Descripcion")))
            lstPermisosEspeciales.Items.Add(Permiso, PermisosActuales.Contains(CInt(rowPermiso("IdPermiso"))))
        Next
    End Sub

    Private Sub UpdatePermisosEspecialesModulo()
        Dim rowPermisosModulo As DataRow
        rowPermisosModulo = CType(DBUtils.GetCurrentRow(bsPermisosModulo), DataRow)
        Dim Permiso As PermisoEspecial
        Dim I, Count As Integer
        Count = lstPermisosEspeciales.CheckedItems.Count
        rowPermisosModulo("PermisosEspeciales") = ""
        For I = 0 To Count - 1
            Permiso = CType(lstPermisosEspeciales.CheckedItems(I), PermisoEspecial)
            If I = Count - 1 Then
                rowPermisosModulo("PermisosEspeciales") = CStr(rowPermisosModulo("PermisosEspeciales")) & Permiso.IdPermiso.ToString()
            Else
                rowPermisosModulo("PermisosEspeciales") = CStr(rowPermisosModulo("PermisosEspeciales")) & Permiso.IdPermiso.ToString() & "|"
            End If
        Next
    End Sub

    Private Function GetPermisosActuales() As List(Of Integer)
        Dim Permisos As New List(Of Integer)
        Dim row As DataRow = DBUtils.GetCurrentRow(bsPermisosModulo)
        If row IsNot Nothing AndAlso Not DBUtils.IsNullOrEmpty(row("PermisosEspeciales")) Then
            Dim actuales, s As String
            actuales = CStr(row("PermisosEspeciales"))
            For Each s In actuales.Split("|"c)
                If Not DBUtils.IsNullOrEmpty(s) Then
                    Permisos.Add(CInt(s))
                End If
            Next
        End If
        Return Permisos
    End Function

    Private Sub lstPermisosEspeciales_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPermisosEspeciales.Leave
        UpdatePermisosEspecialesModulo()
    End Sub

#End Region

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        ' Se aplican los últimos cambios realizados por el usuario al dataset
        Try
            UpdatePermisosPerfil()
            bsPerfiles.EndEdit()
        Catch ex As Exception
            ShowError(ex.Message)
            Exit Sub
        End Try

        ' Se almacenan los datos en la base de datos
        Try
            Datos.BeginTransaction()
            Datos.UpdatePerfiles()
            If Me.NuevoPerfil Then
                ' Actualizar el Id de perfil en la tabla de permisos del módulo
                For I As Integer = 0 To dsSeguridad.Tables("ModulosPerfil").Rows.Count - 1
                    dsSeguridad.Tables("ModulosPerfil").Rows(I)("IdPerfil") = RowPerfil("IdPerfil")
                Next
            End If
            Datos.UpdateModulosPerfil()
            Datos.CommitTransaction()
            DialogResult = DialogResult.OK
        Catch ex As Exception
            Datos.RollbackTransaction()
            ShowError(ex.Message)
		End Try

		'Datos.LoadPerfiles()
		'Datos.LoadModulosPerfil(RowPerfil)
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        bsPerfiles.CancelEdit()
        dsSeguridad.Tables("Perfiles").RejectChanges()
        dsSeguridad.Tables("ModulosPerfil").RejectChanges()
        DialogResult = DialogResult.Cancel
    End Sub

End Class