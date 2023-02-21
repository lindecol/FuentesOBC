Imports System.Windows.Forms
Imports System.Drawing

Public Class DataNavigationDialog


#Region " Propiedades y eventos que manejan la presentación y funcionalidad base"
    Private m_bCanNavigate As Boolean = True
    Private m_bCargar As Boolean = False
    Public Property CanNavigate() As Boolean
        Get
            Return m_bCanNavigate
        End Get
        Set(ByVal value As Boolean)
            m_bCanNavigate = value
            UpdateNavigatorBehavior()
        End Set
    End Property

    Private m_bCanEdit As Boolean = True
    Public Property CanEdit() As Boolean
        Get
            Return m_bCanEdit
        End Get
        Set(ByVal value As Boolean)
            m_bCanEdit = value

            ' Se deshabilita la edición en los controles que asi lo permitan
            For Each ctl As Control In ContentPanel.Controls
                If TypeOf ctl Is TextBox Then
                    CType(ctl, TextBox).ReadOnly = Not value
                Else
                    ctl.Enabled = value
                End If
            Next
            UpdateNavigatorBehavior()
        End Set
    End Property

    Private m_bCanDelete As Boolean = True
    Public Property CanDelete() As Boolean
        Get
            Return m_bCanDelete
        End Get
        Set(ByVal value As Boolean)
            m_bCanDelete = value
            UpdateNavigatorBehavior()
        End Set
    End Property

    Private m_bCanInsert As Boolean = True
    Public Property CanInsert() As Boolean
        Get
            Return m_bCanInsert
        End Get
        Set(ByVal value As Boolean)
            m_bCanInsert = value
            UpdateNavigatorBehavior()
        End Set
    End Property

    Private m_bCanDeshacer As Boolean = True
    Public Property CanDeshacer() As Boolean
        Get
            Return m_bCanDeshacer
        End Get
        Set(ByVal value As Boolean)
            m_bCanDeshacer = value
            UpdateNavigatorBehavior()
        End Set
    End Property

    Public ReadOnly Property RowDelete() As DataRow
        Get
            Return m_rwRegistroEliminado
        End Get
    End Property

    Public Event SaveCurrent(ByVal row As DataRow)
    Public Event SaveAll(ByVal dtChanges As DataTable)
    Public Event DeleteCurrent(ByVal row As DataRow)
    Public Event NewRowAdded(ByVal row As DataRow)


    Private Sub UpdateNavigatorBehavior()
        ctlNavigator.MoveFirstItem.Enabled = m_bCanNavigate
        ctlNavigator.MoveLastItem.Enabled = m_bCanNavigate
        ctlNavigator.MoveNextItem.Enabled = m_bCanNavigate
        ctlNavigator.MovePreviousItem.Enabled = m_bCanNavigate
        BindingNavigatorPositionItem.ReadOnly = Not m_bCanNavigate
        BindingNavigatorAddNewItem.Visible = m_bCanInsert
        BindingNavigatorDeleteItem.Visible = m_bCanDelete
        BindingNavigatorSaveAll.Visible = m_bCanNavigate
        BindingNavigatorSaveCurrent.Visible = m_bCanEdit Or m_bCanInsert
        'BindingNavigatorUndo.Visible = m_bCanEdit Or m_bCanInsert
        BindingNavigatorUndo.Visible = m_bCanDeshacer
        BindingNavigatorSeparator2.Visible = BindingNavigatorUndo.Visible
        BindingNavigatorSeparator3.Visible = BindingNavigatorAddNewItem.Visible Or BindingNavigatorDeleteItem.Visible
        BindingNavigatorSeparator4.Visible = BindingNavigatorSaveAll.Visible Or BindingNavigatorSaveCurrent.Visible
    End Sub

#End Region

#Region " Manejo del BindingSource "
    Public Property BindingSource() As BindingSource
        Get
            Return ctlNavigator.BindingSource
        End Get
        Set(ByVal value As BindingSource)
            ctlNavigator.BindingSource = value
            AddBindingSourceHandlers()
        End Set
    End Property

    Public WriteOnly Property CargaDatos() As Boolean
        Set(ByVal value As Boolean)
            m_bCargar = value
        End Set
    End Property

    Private Sub AddBindingSourceHandlers()
        If BindingSource IsNot Nothing Then
            AddHandler BindingSource.CurrentItemChanged, AddressOf BindingSource_CurrentItemChanged
            AddHandler BindingSource.AddingNew, AddressOf BindingSource_AddingNew
            AddHandler BindingSource.DataError, AddressOf BindingSource_DataError
        End If
    End Sub

    Private Sub BindingSource_CurrentItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If m_bCargar Then
            UpdateEstado()
        End If
    End Sub

    Private Sub BindingSource_DataError(ByVal sender As Object, ByVal e As BindingManagerDataErrorEventArgs)
        ShowError(e.Exception.Message)
    End Sub

    Private Sub BindingSource_AddingNew(ByVal sender As Object, ByVal e As System.ComponentModel.AddingNewEventArgs)
        Try
            BindingSource.EndEdit()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
        UpdateEstado()
    End Sub

#End Region

#Region " Rutinas Utilizadas en la clase "
    Private Sub OnInsertNewRow()
        ' Se validan los permisos
        If Not m_bCanInsert Then
            Exit Sub
        End If

        ' Se ejecuta la acción
        Dim bs As BindingSource = ctlNavigator.BindingSource
        Try
            bs.AddNew()
            Dim row As DataRow = DBUtils.GetCurrentRow(bs)
            RaiseEvent NewRowAdded(row)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub OnSaveCurrent()
        ' Se validan los permisos
        If Not m_bCanEdit Then
            Exit Sub
        End If

        ' Se ejecuta la acción
        Dim bs As BindingSource = ctlNavigator.BindingSource
        Try
            Me.Validate()
            bs.EndEdit()
            Dim row As DataRow = DBUtils.GetCurrentRow(bs)
            RaiseEvent SaveCurrent(row)
            UpdateEstado("Registro Guardado.")
        Catch ex As Exception
            ShowError(ManagerException.Exception(ex))
        End Try
    End Sub

    Private Sub OnSaveAll()
        ' Se validan los permisos
        If Not m_bCanEdit Then
            Exit Sub
        End If

        ' Se ejecuta la acción
        Dim bs As BindingSource = ctlNavigator.BindingSource
        Try
            Me.Validate()
            bs.EndEdit()
            Dim row As DataRow = DBUtils.GetCurrentRow(bs)

            If row IsNot Nothing Then
                RaiseEvent SaveAll(row.Table)
            End If

            UpdateEstado("Todos los cambios han sido guardados.")
        Catch ex As Exception
            ShowError(ManagerException.Exception(ex))
        End Try
    End Sub

    Private Sub OnUndoChanges()
        ' Se validan los permisos
        If Not m_bCanEdit Then
            Exit Sub
        End If

        ' Se ejecuta la acción
        Dim bs As BindingSource = ctlNavigator.BindingSource
        Try
            bs.CancelEdit()
            Dim row As DataRow = DBUtils.GetCurrentRow(bs)
            If row IsNot Nothing Then
                If row.RowState = DataRowState.Modified Then
                    row.RejectChanges()
                End If
            End If
        Catch ex As Exception
            ShowError(ManagerException.Exception(ex))
        End Try
    End Sub


    Private Sub OnDeleteCurrent()
        ' Se validan los permisos
        If Not m_bCanDelete Then
            Exit Sub
        End If

        ' Se ejecuta la acción
        Dim bs As BindingSource = ctlNavigator.BindingSource
        Try
            If Confirmar("¿Esta seguro de borrar el registro actual?") Then
                Dim row As DataRow = DBUtils.GetCurrentRow(bs)
                If row.RowState <> DataRowState.Detached And row.RowState <> DataRowState.Added Then
                    bs.RemoveCurrent()
                    RaiseEvent DeleteCurrent(row)
                    UpdateEstado("Registro Borrado.")
                Else
                    bs.RemoveCurrent()
                End If
            End If
        Catch ex As Exception
            ShowError(ManagerException.Exception(ex))
        End Try
    End Sub

    Private Sub UpdateEstado(Optional ByVal sMensaje As String = "")
        Dim row As DataRow
        Dim bs As BindingSource = ctlNavigator.BindingSource
        row = DBUtils.GetCurrentRow(bs)
        If row IsNot Nothing Then
            If sMensaje = "" Then
                If row.RowState = DataRowState.Unchanged Then
                    lblEstadoRegistro.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
                    lblEstadoRegistro.Text = "Registro sin modificaciones"
                ElseIf row.RowState = DataRowState.Added Or row.RowState = DataRowState.Detached Then
                    lblEstadoRegistro.ForeColor = Color.Red
                    lblEstadoRegistro.Text = "* Registro Nuevo"
                ElseIf row.RowState = DataRowState.Modified Then
                    lblEstadoRegistro.ForeColor = Color.Red
                    lblEstadoRegistro.Text = "* Registro Modificado"
                End If
            Else
                lblEstadoRegistro.Text = sMensaje
            End If
            Dim nRowsChanged As Integer = 0
            If row.Table IsNot Nothing Then
                Dim dtChanges As DataTable = row.Table.GetChanges()
                If dtChanges IsNot Nothing Then
                    nRowsChanged = row.Table.GetChanges().Rows.Count
                End If
            End If
            If nRowsChanged = 1 Then
                lblEstadoRegistro.ForeColor = Color.Red
                lblEstadoRegistro.Text &= String.Format(" ( 1 Registro pendientes )", nRowsChanged)
            ElseIf nRowsChanged > 1 Then
                lblEstadoRegistro.ForeColor = Color.Red
                lblEstadoRegistro.Text &= String.Format(" ( {0} Registros Pendientes )", nRowsChanged)
            End If
        End If
    End Sub

#End Region

#Region " Control eventos producidos por el usuario"

    Private Sub DataEditDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        KeyPreview = True
    End Sub

    Private Sub DataEditDialog_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        ' Teclas Rapidas
        If e.Shift Or e.Control Then
            If e.KeyCode = Keys.G Then
                If e.Shift And e.Control Then
                    OnSaveAll()
                ElseIf e.Control Then
                    OnSaveCurrent()
                End If
            ElseIf e.KeyCode = Keys.X And e.Control Then
                OnDeleteCurrent()
            ElseIf e.KeyCode = Keys.N And e.Control Then
                OnInsertNewRow()
            ElseIf e.KeyCode = Keys.Z And e.Control Then
                OnUndoChanges()
            End If
        End If
    End Sub

    Private Sub BindingNavigatorAddNewItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorAddNewItem.Click
        OnInsertNewRow()
    End Sub

    Private m_rwRegistroEliminado As DataRow

    Private Sub BindingNavigatorDeleteItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorDeleteItem.Click
        If ctlNavigator.BindingSource.Count > 0 Then
            m_rwRegistroEliminado = DBUtils.GetCurrentRow(ctlNavigator.BindingSource).Table.NewRow
            m_rwRegistroEliminado.ItemArray = DBUtils.GetCurrentRow(ctlNavigator.BindingSource).ItemArray
            OnDeleteCurrent()
        End If
    End Sub

    Private Sub BindingNavigatorSaveCurrent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorSaveCurrent.Click
        OnSaveCurrent()
    End Sub

    Private Sub BindingNavigatorSaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorSaveAll.Click

        OnSaveAll()


    End Sub

    Private Sub BindingNavigatorUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorUndo.Click
        OnUndoChanges()
    End Sub

#End Region

End Class