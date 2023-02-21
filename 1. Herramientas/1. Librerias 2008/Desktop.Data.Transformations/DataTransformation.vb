Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports Desktop.Data
Imports System.Collections.Specialized
Imports System.Reflection
Imports Desktop
Imports Desktop.Logging

Public Class DataTransformation
    Public dsConfig As New TransformationDataSet
    Private m_ConnectionString As String
    Private m_ConnectionTypeName As String
    Private m_WorkDirectory As String = PathWorkDirectory.PATH


    Public Interface IProgressController
        Sub TaskStarted(ByVal TaskName As String, ByVal TaskIndex As Integer)
        Sub UpdateCurrentTaskProgress(ByVal nPercentageProgress As Integer)
        Sub TaskError(ByVal ErrorMessage As String)
        Sub TaskEnded(ByVal RowsProcessed As Integer, ByVal nRowsWithErrors As Integer, ByVal MensajeError As String)
    End Interface

    Public Sub New(ByVal ConnectionTypeName As String, ByVal ConnectionString As String)
		m_ConnectionString = ConnectionString
        m_ConnectionTypeName = ConnectionTypeName
    End Sub

    Public ReadOnly Property ConnectionString() As String
        Get
            Return m_ConnectionString
        End Get
    End Property

    Public Function GetConfig() As String
        Dim sw As New IO.StringWriter()
        Dim res As String
        dsConfig.WriteXml(sw)
        res = sw.ToString()
        sw.Close()
        Return res
    End Function

    Public Sub SetConfig(ByVal sConfig As String)
        If Not DBUtils.IsNullOrEmpty(sConfig) Then
            If sConfig <> """" Then
                Dim sr As New IO.StringReader(sConfig)
                dsConfig.ReadXml(sr)
                sr.Close()
                dsConfig.AcceptChanges()
            End If
        End If
    End Sub

    Public ReadOnly Property ConnectionTypeName() As String
        Get
            Return m_ConnectionTypeName
        End Get
    End Property

    Public ReadOnly Property TaskCount() As Integer
        Get
            Return dsConfig.Tareas.Count
        End Get
    End Property

    Public Property WorkDirectory() As String
        Get
            Return m_WorkDirectory
        End Get
        Set(ByVal value As String)
            m_WorkDirectory = value
        End Set
    End Property

    Public Sub SubirOrdenTarea(ByVal rowTarea As TransformationDataSet.TareasRow)
        ' Si ya es la primera tarea no se puede subir màs
        If rowTarea.ExecutionOrder = 1 Then
            Exit Sub
        End If

        ' Se busca la fila con la cual intercambiar el orden de ejecución
        Dim rows() As DataRow
        Dim sFiltro As String = String.Format( _
            "ExecutionOrder < {0}", _
            rowTarea.ExecutionOrder, rowTarea.IdTarea)
        rows = rowTarea.Table.Select(sFiltro, "ExecutionOrder DESC")

        ' Se intercambia el orden de ejecución entre la fila actual y la 
        ' primera de las filas encontradas en la busqueda anterior
        If rows.Length > 0 Then
            Dim nOrdenTemp As Integer = rowTarea.ExecutionOrder
            rowTarea.ExecutionOrder = CInt(rows(0)("ExecutionOrder"))
            rows(0)("ExecutionOrder") = nOrdenTemp
        End If
    End Sub

    Public Sub BajarOrdenTarea(ByVal rowTarea As TransformationDataSet.TareasRow)
        ' Se busca la fila con la cual intercambiar el orden de ejecución
        Dim rows() As DataRow
        rows = rowTarea.Table.Select(String.Format("ExecutionOrder > {0}", _
            rowTarea.ExecutionOrder, "ExecutionOrder ASC"))

        ' Se intercambia el orden de ejecución entre la fila actual y la 
        ' primera de las filas encontradas en la busqueda anterior
        If rows.Length > 0 Then
            Dim nOrdenTemp As Integer = rowTarea.ExecutionOrder
            rowTarea.ExecutionOrder = CInt(rows(0)("ExecutionOrder"))
            rows(0)("ExecutionOrder") = nOrdenTemp
        End If
    End Sub

    Public Sub Execute(ByVal pc As IProgressController)

        ' Se crea el directorio de trabajo si no existe
        Directory.CreateDirectory(WorkDirectory)

        ' Se ejecutan las tareas configuradas en el orden definido por el usuario
        Dim rows() As TransformationDataSet.TareasRow
        Dim Task As TaskBase
        rows = CType(dsConfig.Tareas.Select("", "ExecutionOrder ASC"), TransformationDataSet.TareasRow())
        For I As Integer = 0 To rows.Length - 1
            Try
                pc.TaskStarted(rows(I).Description, I)
                Task = CreateTask(rows(I).TaskTypeName)
                Task.SetConfig(rows(I).Configuration)
                Task.SetDirectory(Me.WorkDirectory)
                Task.Execute(pc)
				pc.TaskEnded(Task.ProcessedRows, Task.RowsWithErros, Task.mensajeErrores)
			Catch ex As Exception
                Dim exc As New ApplicationException( _
                "Error Ejecutando Tarea: " & rows(I).Description & vbCrLf & _
                "Mensaje: " & ex.Message)
				Logger.Write(exc.Message)
				Logger.Write(ex)
				Continue For
            End Try
        Next
    End Sub

    Friend Function CreateTask(ByVal TaskName As String) As TaskBase
        Dim oAssembly As Assembly = Assembly.GetExecutingAssembly()
        Dim oTask As TaskBase
        oTask = CType(oAssembly.CreateInstance("Desktop.Data.Transformations." & TaskName), TaskBase)
        oTask.Parent = Me
        Return oTask
    End Function

    Private Shared m_AvailableTask As StringCollection
    Friend Function GetAvailableTasks() As StringCollection
        If m_AvailableTask Is Nothing Then
            m_AvailableTask = New StringCollection()
            Dim oAssembly As Assembly = Assembly.GetExecutingAssembly()
            Dim taskBaseType As Type = GetType(TaskBase)
            For Each oType As System.Type In oAssembly.GetTypes()
                If oType.IsClass AndAlso _
                    oType.IsSubclassOf(taskBaseType) Then
                    m_AvailableTask.Add(oType.Name)
                End If
            Next
        End If
        Return m_AvailableTask
    End Function




End Class
