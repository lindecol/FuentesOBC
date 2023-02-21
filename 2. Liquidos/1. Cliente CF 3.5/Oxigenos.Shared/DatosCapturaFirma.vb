Public Class DatosCapturaFirma
    Private m_PathFirma As String
    Public Property PathFirma() As String
        Get
            Return m_PathFirma
        End Get
        Set(ByVal value As String)
            m_PathFirma = value
        End Set
    End Property

    Private m_IdentificacionFirma As String
    Public Property IdentificacionFirma() As String
        Get
            Return m_IdentificacionFirma
        End Get
        Set(ByVal value As String)
            m_IdentificacionFirma = value
        End Set
    End Property

    Private m_NombreFirma As String
    Public Property NombreFirma() As String
        Get
            Return m_NombreFirma
        End Get
        Set(ByVal value As String)
            m_NombreFirma = value
        End Set
    End Property
End Class
