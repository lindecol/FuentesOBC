Public MustInherit Class TaskBase
    Private m_Parent As DataTransformation

    Public MustOverride ReadOnly Property dsConfig() As DataSet

    Friend ProcessedRows As Integer = 0
    Friend RowsWithErros As Integer = 0
    Friend m_WorkDirectory As String = PathWorkDirectory.PATH
    Friend mensajeErrores As String = ""

    Public Sub New()

    End Sub

    Public Property Parent() As DataTransformation
        Get
            Return m_Parent
        End Get
        Set(ByVal value As DataTransformation)
            m_Parent = value
        End Set
    End Property

    Public MustOverride Sub Execute(ByVal pc As DataTransformation.IProgressController)

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
            Dim sr As New IO.StringReader(sConfig)
            dsConfig.ReadXml(sr)
            sr.Close()
        End If
    End Sub

    Public Sub SetDirectory(ByVal sWorkDirectory As String)
        m_WorkDirectory = sWorkDirectory
    End Sub

    Public MustOverride Function ShowConfigDialog() As Boolean

    



End Class
