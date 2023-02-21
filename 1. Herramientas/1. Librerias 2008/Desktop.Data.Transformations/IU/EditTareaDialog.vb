Public Class EditTareaDialog

    Private m_Transformation As DataTransformation
    Private m_Nuevo As Boolean
    Private m_rowTarea As TransformationDataSet.TareasRow

    Public Shared Sub Run(ByVal Transformation As DataTransformation, _
    Optional ByVal rowTarea As TransformationDataSet.TareasRow = Nothing)
        Dim frm As New EditTareaDialog
        frm.m_Nuevo = rowTarea Is Nothing
        frm.m_rowTarea = rowTarea
        frm.m_Transformation = Transformation
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub EditTareaDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitData()
        If m_Nuevo Then
            Me.Text = "Nueva tarea"
            ' Se agrega la tarea y se configuran los valores por defecto
            bsTareas.AddNew()
            cbTipoTarea.DataSource = m_Transformation.GetAvailableTasks()
            m_rowTarea = CType(DBUtils.GetCurrentRow(bsTareas), TransformationDataSet.TareasRow)
            Dim ExecutionOrder As Object = dsConfig.Tareas.Compute("MAX(ExecutionOrder)", "")
            If ExecutionOrder Is DBNull.Value Then
                ExecutionOrder = 1
            Else
                ExecutionOrder = CInt(ExecutionOrder) + 1
            End If
            m_rowTarea.ExecutionOrder = CInt(ExecutionOrder)
        Else
            Me.Text = "Editar tarea"
            DBUtils.SetCurrentRow(bsTareas, m_rowTarea)
            cbTipoTarea.DataSource = m_Transformation.GetAvailableTasks()
            Me.cbTipoTarea.Text = m_rowTarea.TaskTypeName
        End If
    End Sub

    Private Sub InitData()
        Me.dsConfig = m_Transformation.dsConfig
        bsTareas.DataSource = Me.m_Transformation.dsConfig
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Me.Validate()
        Try
            bsTareas.EndEdit()
            DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        bsTareas.CancelEdit()
        If m_rowTarea.RowState = DataRowState.Detached Then
            m_rowTarea.Delete()
        End If
        dsConfig.Tareas.RejectChanges()
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub
End Class
