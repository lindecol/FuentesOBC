Imports System.Threading
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.IO


Public Class FrameworkApp
    Private Shared m_Nucleo As INucleo
    Public Shared Event Closed_MainForm()
    Public Shared Event Cerrar_Sesion()

#Region "Funciones y Propiedades de uso interno (Friend o Private)"

    Friend Shared Sub Initialize(ByVal objNucleo As INucleo)
        m_Nucleo = objNucleo
    End Sub

    Private Shared Function GetStringValue(ByVal row As DataRow, ByVal FieldName As String) As String
        If row Is Nothing Then
            Return String.Empty
        End If
        Dim value As Object = row(FieldName)
        If (value Is Nothing) OrElse (value Is DBNull.Value) Then
            Return String.Empty
        Else
            Return CStr(value)
        End If
    End Function

    Private Shared Function GetInt32Value(ByVal row As DataRow, ByVal FieldName As String) As Integer
        If row Is Nothing Then
            Return -1
        End If
        Dim value As Object = row(FieldName)
        If (value Is Nothing) OrElse (value Is DBNull.Value) Then
            Return -1
        Else
            Return CInt(value)
        End If
    End Function

    Private Shared Sub CheckInitialization()
        If m_Nucleo Is Nothing Then
            Throw New InvalidOperationException("Nucleo no ha sido inicializado")
        End If
    End Sub

    Friend Shared Function PermiteInsertar(ByVal IdModulo As Integer) As Boolean
        CheckInitialization()
        Return m_Nucleo.PermiteInsertar(IdModulo)
    End Function

    Friend Shared Function PermiteEditar(ByVal IdModulo As Integer) As Boolean
        CheckInitialization()
        Return m_Nucleo.PermiteEditar(IdModulo)
    End Function

    Friend Shared Function PermiteBorrar(ByVal IdModulo As Integer) As Boolean
        CheckInitialization()
        Return m_Nucleo.PermiteBorrar(IdModulo)
    End Function

    Friend Shared Function PermiteImprimir(ByVal IdModulo As Integer) As Boolean
        CheckInitialization()
        Return m_Nucleo.PermiteImprimir(IdModulo)
    End Function

    Friend Shared Function PermitePermisoEspecial(ByVal IdModulo As Integer, ByVal IdPermiso As Integer) As Boolean
        CheckInitialization()
        Return m_Nucleo.PermitePermisoEspecial(IdModulo, IdPermiso)
    End Function

    Public Shared Function ObtenerUsuarios() As DataTable
        Dim Seguridad As New GestorSeguridad
        Seguridad.LoadUsuarios()
        Return Seguridad.dsSeguridad.Usuarios
    End Function

#End Region

    Private Shared m_Sesion As GestorSesion
    Private Shared m_bCrearUsuarioInicial As Boolean = True

    'FORMATERAR ICONOS DEL PANEL
    Private Shared _imagesMenu As ImageList = Nothing
    Private Shared _listImages As List(Of String) = Nothing

    Public Shared Property imagesMenu() As ImageList
        Get
            Return _imagesMenu
        End Get
        Set(ByVal value As ImageList)
            _imagesMenu = value
        End Set
    End Property

    Public Shared Property listImages() As List(Of String)
        Get
            Return _listImages
        End Get
        Set(ByVal value As List(Of String))
            _listImages = value
        End Set
    End Property


    Public Shared Property CrearUsuarioInicial() As Boolean
        Get
            Return m_bCrearUsuarioInicial
        End Get
        Set(ByVal value As Boolean)
            m_bCrearUsuarioInicial = value
        End Set
    End Property


    Public Shared ReadOnly Property Sesion() As GestorSesion
        Get
            If m_Sesion Is Nothing Then
                m_Sesion = New GestorSesion()
            End If
            Return m_Sesion
        End Get
    End Property

    Public Class UsuarioActual

        Public Shared ReadOnly Property IdUsuario() As Integer
            Get
                CheckInitialization()
                Return GetInt32Value(m_Nucleo.UsuarioActual, "IdUsuario")
            End Get
        End Property

        Public Shared ReadOnly Property Nombre() As String
            Get
                CheckInitialization()
                Return GetStringValue(m_Nucleo.UsuarioActual, "Nombre")
            End Get
        End Property

        Public Shared ReadOnly Property Usuario() As String
            Get
                CheckInitialization()
                Return GetStringValue(m_Nucleo.UsuarioActual, "Usuario")
            End Get
        End Property
    End Class

    Public Class EmpresaActual
        Public Shared ReadOnly Property IdEmpresa() As Integer
            Get
                CheckInitialization()
                Return GetInt32Value(m_Nucleo.EmpresaActual, "IdEmpresa")
            End Get
        End Property

        Public Shared ReadOnly Property Nombre() As String
            Get
                CheckInitialization()
                Return GetStringValue(m_Nucleo.EmpresaActual, "NombreCorto")
            End Get
        End Property

        Public Shared ReadOnly Property Nit() As String
            Get
                CheckInitialization()
                Return GetStringValue(m_Nucleo.EmpresaActual, "NIT")
            End Get
        End Property

        Public Shared ReadOnly Property NombreCompleto() As String
            Get
                CheckInitialization()
                Return GetStringValue(m_Nucleo.EmpresaActual, "NombreCompleto")
            End Get
        End Property

    End Class

    Public Class HoraInicioSesion
        Public Shared ReadOnly Property FechaHora() As DateTime
            Get
                Return m_Nucleo.InicioSesion
            End Get
        End Property
    End Class

    Public Shared Function GetIdModulo(ByVal ctlView As BaseViewControl) As Integer
        CheckInitialization()
        Return m_Nucleo.GetIdModulo(ctlView)
    End Function

    Public Shared Sub RegisterModalForm(ByVal frm As System.Windows.Forms.Form)
        CheckInitialization()
        m_Nucleo.RegisterModalForm(frm)
    End Sub

    Private Shared m_sApplicationName As String
    Public Shared Property ApplicationName() As String
        Get
            Return m_sApplicationName
        End Get
        Set(ByVal value As String)
            m_sApplicationName = value
        End Set
    End Property

    Private Shared m_sApplicationVersion As String
    Public Shared Property ApplicationVersion() As String
        Get
            Return m_sApplicationVersion
        End Get
        Set(ByVal value As String)
            m_sApplicationVersion = value
        End Set
    End Property


    'Private Shared Function CrearBaseDatosSQLmobile(ByVal sPath As String, ByVal sScript As String, ByVal sTablaPrueba As String, Optional ByVal sPassword As String = "") As Boolean
    '    Return GestorConfiguracion.CrearDaseDatosSQLMobile(sPath, sScript, sTablaPrueba, sPassword)
    'End Function

    'GUARDAR LA CONFIGURACION
    Private Shared m_ConnectionType As String = Desktop.Data.ConnectionTypes.SqlClient
    Public Shared Property ConnectionType() As String
        Get
            Return m_ConnectionType
        End Get
        Set(ByVal value As String)
            m_ConnectionType = value
        End Set
    End Property

    Private Shared m_ConnectionString As String
    Public Shared Property ConnectionString() As String
        Get
            Return m_ConnectionString
        End Get
        Set(ByVal value As String)
            m_ConnectionString = value
        End Set
    End Property

    Private Shared m_bMultiEmpresa As Boolean = False
    Public Shared Property MultiEmpresa() As Boolean
        Get
            Return m_bMultiEmpresa
        End Get
        Set(ByVal value As Boolean)
            m_bMultiEmpresa = value
        End Set
    End Property

    Public Shared Sub Configure()
        'CHEQUEAR LA BASE DE DATOS
        Dim sMensaje As String = GestorConfiguracion.CheckBaseDatos()
        If sMensaje IsNot Nothing Then
            MsgBox("Error creando base de datos: " + sMensaje + "!!", MsgBoxStyle.Critical, "Error Creando Base Datos")
            Exit Sub
        End If

        ' Se incializa el soporte a temas de windows 
        If OSFeature.Feature.IsPresent(OSFeature.Themes) Then
            System.Windows.Forms.Application.EnableVisualStyles()
        End If
        Application.DoEvents() 'This must be here, otherwise buttons won't stylize

        ' Se obtiene la linea de comandos y se inicia la forma correspondiente
        Dim args() As String
        args = Environment.GetCommandLineArgs()
        Dim m_SplashThread As New Thread(AddressOf ShowSplashScreen)
        m_SplashThread.Start()
        Application.Run(New MainConfiguracionForm)
    End Sub

    Public Shared Sub Start()
        'CHEQUEAR LA BASE DE DATOS
        Dim sMensaje As String = GestorConfiguracion.CheckBaseDatos()
        If sMensaje IsNot Nothing Then
            MsgBox("Error creando base de datos: " + sMensaje + "!!", MsgBoxStyle.Critical, "Error Creando Base Datos")
            Exit Sub
        End If


        ' Se incializa el soporte a temas de windows 
        If OSFeature.Feature.IsPresent(OSFeature.Themes) Then
            System.Windows.Forms.Application.EnableVisualStyles()
        End If
        Application.DoEvents() 'This must be here, otherwise buttons won't stylize

        ' Se obtiene la linea de comandos y se inicia la forma correspondiente
        Dim args() As String
        args = Environment.GetCommandLineArgs()
        Dim m_SplashThread As New Thread(AddressOf ShowSplashScreen)
        m_SplashThread.Start()
        Application.Run(New MainForm)

    End Sub

    Public Shared Sub ShowSplashScreen()
        'SplashScreen.ShowDialog()
        'SplashScreen.Refresh()
    End Sub

    Public Shared ReadOnly Property NombreEnsamblado() As String
        Get
            Return "Desktop.Framework.DLL"
        End Get
    End Property

    Public Shared Sub ApplyStyle(ByVal ctlGridView As DataGridView)
        ' Se aplica estilo del grid
        ctlGridView.BorderStyle = BorderStyle.None
        ctlGridView.EnableHeadersVisualStyles = False
        ctlGridView.GridColor = Color.SteelBlue

        ' Propiedades headers columnas y filas
        ctlGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        ctlGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.GradientInactiveCaption)
        ctlGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        ctlGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ctlGridView.AllowUserToResizeRows = False

        ' Propiedades especificas para algunos tipos de columna
        Dim I As Integer
        Dim cbCell As DataGridViewComboBoxCell
        For I = 0 To ctlGridView.Columns.Count - 1
            If ctlGridView.Columns(I).CellType Is GetType(DataGridViewComboBoxCell) Then
                cbCell = DirectCast(ctlGridView.Columns(I).CellTemplate, DataGridViewComboBoxCell)
                cbCell.FlatStyle = FlatStyle.Standard
                cbCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
                cbCell.DisplayStyleForCurrentCellOnly = True
            End If
        Next
    End Sub


    'PROPIEDADES PARA CREAR LAS EMPRESA
    Private Shared m_sEmpresaAplicacion As String
    Public Shared Property EmpresaAplicacion() As String
        Get
            Return m_sEmpresaAplicacion
        End Get
        Set(ByVal value As String)
            m_sEmpresaAplicacion = value
        End Set
    End Property

    Private Shared m_sNITEmpresaAplicacion As String
    Public Shared Property NITEmpresaAplicacion() As String
        Get
            Return m_sNITEmpresaAplicacion
        End Get
        Set(ByVal value As String)
            m_sNITEmpresaAplicacion = value
        End Set
    End Property


    'Propiedades para habilitar Importar y Exportar Configuracion
    Private Shared m_bImportarConfiguracion As Boolean
    Public Shared Property ImportarConfiguracion() As Boolean
        Get
            Return m_bImportarConfiguracion
        End Get
        Set(ByVal value As Boolean)
            m_bImportarConfiguracion = value
        End Set
    End Property


    Private Shared m_bExportarConfiguracion As Boolean
    Public Shared Property ExportarConfiguracion() As Boolean
        Get
            Return m_bExportarConfiguracion
        End Get
        Set(ByVal value As Boolean)
            m_bExportarConfiguracion = value
        End Set
    End Property

    Private Shared m_bExpanderArbol As Boolean = False
    Public Shared Property ExpanderArbol() As Boolean
        Get
            Return m_bExpanderArbol
        End Get
        Set(ByVal value As Boolean)
            m_bExpanderArbol = value
        End Set
    End Property

    Private Shared m_bCambiarClaveUsuario As Boolean = True
    Public Shared Property CambiarClaveUsuario() As Boolean
        Get
            Return m_bCambiarClaveUsuario
        End Get
        Set(ByVal value As Boolean)
            m_bCambiarClaveUsuario = value
        End Set
    End Property

    Public Shared Sub CerrarSession()
        RaiseEvent Cerrar_Sesion()
    End Sub

    Public Shared Sub Closed()
        RaiseEvent Closed_MainForm()
    End Sub

End Class