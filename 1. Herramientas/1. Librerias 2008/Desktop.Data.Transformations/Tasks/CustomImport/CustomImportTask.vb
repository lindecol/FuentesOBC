Imports System.Reflection
Public Class CustomImportTask
    Inherits TaskBase

    Private m_dsConfig As CustomImporTaskDataSet

    Public Overrides ReadOnly Property dsConfig() As System.Data.DataSet
        Get
            If m_dsConfig Is Nothing Then
                m_dsConfig = New CustomImporTaskDataSet
            End If
            Return m_dsConfig
        End Get
    End Property

    Public Overrides Sub Execute(ByVal pc As DataTransformation.IProgressController)

        'Dim f As New IntegracionIndupalma.InterfazComunicacion
        'f.IntegrarDatosSQLServerSanidad()

        If m_dsConfig.General.Count > 0 Then
            Dim objAssembly As Assembly
            objAssembly = Assembly.LoadFile(m_dsConfig.General(0).UbicacionEnsamblado)
            For Each Metodo As MethodInfo In objAssembly.GetType(m_dsConfig.General(0).Clase).GetMethods
                If Metodo.Name = m_dsConfig.General(0).Metodo Then
                    Dim objClass As Object = objAssembly.CreateInstance(m_dsConfig.General(0).Clase)
                    Metodo.Invoke(objClass, Nothing)
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Public Overrides Function ShowConfigDialog() As Boolean
        Return CustomTaskDialog.Run(CType(Me.dsConfig, CustomImporTaskDataSet))
    End Function
End Class
