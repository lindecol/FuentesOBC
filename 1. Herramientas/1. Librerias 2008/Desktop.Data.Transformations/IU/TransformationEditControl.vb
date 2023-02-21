Public Class TransformationEditControl

    Private m_Transformation As DataTransformation

    Public Event ConfigurationChanged(ByVal t As DataTransformation)

    Public Property Transformation() As DataTransformation
        Get
            Return m_Transformation
        End Get
        Set(ByVal value As DataTransformation)
            m_Transformation = value
            If value Is Nothing Then
                dsConfig.Clear()
            Else
                dsConfig = m_Transformation.dsConfig
                bsTareas.DataSource = dsConfig
            End If
        End Set
    End Property

    Private Sub TransformationEditControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Style.ApplyStyle(dgvTareas)
    End Sub

    Private Sub tbtnNuevaTarea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnNueva.Click

        EditTareaDialog.Run(Me.Transformation)
        OnConfigurationChanged()
    End Sub

    Private Sub EditarTareaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnEditar.Click
        Dim row As TransformationDataSet.TareasRow
        If bsTareas.Count > 0 Then
            row = CType(DBUtils.GetCurrentRow(bsTareas), TransformationDataSet.TareasRow)
            EditTareaDialog.Run(m_Transformation, row)
            OnConfigurationChanged()
        Else
            ShowError("No hay tareas configuradas para el proceso!")
        End If
    End Sub

    Private Sub mitemSubirOrdenEjecucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnSubir.Click
        Dim row As TransformationDataSet.TareasRow
        If bsTareas.Count > 0 Then
            row = CType(DBUtils.GetCurrentRow(bsTareas), TransformationDataSet.TareasRow)
            Transformation.SubirOrdenTarea(row)
            OnConfigurationChanged()
        End If
    End Sub

    Private Sub mitemBajarOrdenEjecucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnBajar.Click
        Dim row As TransformationDataSet.TareasRow
        If bsTareas.Count > 0 Then
            row = CType(DBUtils.GetCurrentRow(bsTareas), TransformationDataSet.TareasRow)
            Transformation.BajarOrdenTarea(row)
            OnConfigurationChanged()
        End If
    End Sub

    Public Sub ConfigurarTareas(ByVal Ancho As Integer, ByVal alto As Integer)
        Me.dgvTareas.Size = New System.Drawing.Size(Ancho, alto)
    End Sub

    Private Sub tbtnEjecutarAhora_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim row As TransformationDataSet.TareasRow
            If bsTareas.Count > 0 Then
                If Confirmar("Esta seguro de ejecutar el proceso seleccionado en este momento?") Then
                    row = CType(DBUtils.GetCurrentRow(bsTareas), TransformationDataSet.TareasRow)
                    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    System.Windows.Forms.Cursor.Current = Cursors.Default
                End If
            Else
                ShowError("No hay procesos configurados!")
            End If
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                ShowError(ex.Message)
            Else
                ShowError(ex.Message + vbCrLf + ex.InnerException.Message)
            End If
        End Try
    End Sub

    Private Sub tbtnConfigurar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnConfigurar.Click
        Dim row As TransformationDataSet.TareasRow
        If bsTareas.Count > 0 Then
            row = CType(DBUtils.GetCurrentRow(bsTareas), TransformationDataSet.TareasRow)
            Dim task As TaskBase = m_Transformation.CreateTask(row.TaskTypeName)
            If Not row.IsConfigurationNull() Then
                task.SetConfig(row.Configuration)
            End If
            If task.ShowConfigDialog() Then
                row.Configuration = task.GetConfig()
                OnConfigurationChanged()
            End If
        End If
    End Sub

    Private Sub tbtnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnEliminar.Click
        If bsTareas.Count > 0 Then
            If Confirmar("Esta seguro de borrar la tarea seleccionada?") Then
                Dim row As TransformationDataSet.TareasRow
                row = CType(DBUtils.GetCurrentRow(bsTareas), TransformationDataSet.TareasRow)
                row.Delete()
                OnConfigurationChanged()
            End If
        End If
    End Sub

    Private Sub OnConfigurationChanged()
        If Me.Transformation IsNot Nothing Then
            If Me.Transformation.dsConfig.GetChanges() IsNot Nothing Then
                Me.Transformation.dsConfig.AcceptChanges()
                RaiseEvent ConfigurationChanged(Me.Transformation)
            End If
        End If
    End Sub
    
End Class
