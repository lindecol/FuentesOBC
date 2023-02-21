Public Class NucleoSettings
    Inherits MovilidadCF.Configuration.Settings

    Public Property IDTerminal() As String
        Get
            Return GetValue("IDTerminal", "")
        End Get
        Set(ByVal value As String)
            SetValue("IDTerminal", value)
        End Set
    End Property

    Public Property Programa() As Programas
        Get
            Me.LoadConfig()
            Return CType(CInt(GetValue("Programa", "0")), Programas)
        End Get
        Set(ByVal value As Programas)
            SetValue("Programa", CStr(CInt(value)))
        End Set
    End Property

    Public Property IPServidor() As String
        Get
            Return GetValue("IPServidor", "")
        End Get
        Set(ByVal value As String)
            SetValue("IPServidor", value)
        End Set
    End Property

    Public Property UsaHTTPS() As Boolean
        Get
            If GetValue("UsaHTTPS", "0") = "0" Then
                Return False
            Else
                Return True
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetValue("UsaHTTPS", "1")
            Else
                SetValue("UsaHTTPS", "0")
            End If
        End Set
    End Property

    Public Property PuertoServidor() As Integer
        Get
            Return CInt(GetValue("PuertoServidor", "80"))
        End Get
        Set(ByVal value As Integer)
            SetValue("PuertoServidor", CStr(value))
        End Set
    End Property

    Public Property VirtualDirectory() As String
        Get
            Return GetValue("PathWebService", "")
        End Get
        Set(ByVal value As String)
            SetValue("PathWebService", value)
        End Set
    End Property

    Public Property ConexionGPRS() As String
        Get
            Return GetValue("ConexionGPRS", "")
        End Get
        Set(ByVal value As String)
            SetValue("ConexionGPRS", value)
        End Set
    End Property

    Public Property UsuarioWebService() As String
        Get
            Return GetValue("UsuarioWebService", "")
        End Get
        Set(ByVal value As String)
            SetValue("UsuarioWebService", value)
        End Set
    End Property

    Public Property ClaveWebService() As String
        Get
            Return GetValue("ClaveWebService", "")
        End Get
        Set(ByVal value As String)
            SetValue("ClaveWebService", value)
        End Set
    End Property

    Public Property IPServidorGPRS() As String
        Get
            Return GetValue("IPServidorGPRS", "")
        End Get
        Set(ByVal value As String)
            SetValue("IPServidorGPRS", value)
        End Set
    End Property

    Public Property DeviceModel() As DeviceModels
        Get
            Return CType(CInt(GetValue("DeviceModel", "0")), DeviceModels)
        End Get
        Set(ByVal value As DeviceModels)
            SetValue("DeviceModel", CInt(value).ToString)
        End Set
    End Property

    Public Property PrinterModel() As PrinterModels
        Get
            Return CType(CInt(GetValue("PrinterModel", "0")), PrinterModels)
        End Get
        Set(ByVal value As PrinterModels)
            SetValue("PrinterModel", CInt(value).ToString)
        End Set
    End Property

    Public Property PrinterPort() As String
        Get
            Return GetValue("PrinterPort", "COM9:")
        End Get
        Set(ByVal value As String)
            SetValue("PrinterPort", value)
        End Set
    End Property

    Public Property PrinterPortType() As PrinterPortTypes
        Get
            Return CType(CInt(GetValue("PrinterPortType", "0")), PrinterPortTypes)
        End Get
        Set(ByVal value As PrinterPortTypes)
            SetValue("PrinterPortType", CInt(value).ToString)
        End Set
    End Property

    Public Property Etiqueta() As String
        Get
            Return GetValue("Etiqueta", "\Application\Horizontal.LBL")
        End Get
        Set(ByVal value As String)
            SetValue("Etiqueta", value)
        End Set
    End Property
    Public Property EtiquetaHojaRuta() As String
        Get
            Return GetValue("EtiquetaHojaRuta", "\Application\HojaRuta.LBL")
        End Get
        Set(ByVal value As String)
            SetValue("EtiquetaHojaRuta", value)
        End Set
    End Property

    Public Property NumeroEtiquetas() As String
        Get
            Return GetValue("NumeroEtiquetas", "3")
        End Get
        Set(ByVal value As String)
            SetValue("NumeroEtiquetas", value)
        End Set
    End Property

End Class
