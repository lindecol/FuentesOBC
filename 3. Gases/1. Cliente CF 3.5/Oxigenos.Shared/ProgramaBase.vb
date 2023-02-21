Imports System.IO

Public MustInherit Class ProgramaBase
    Protected m_lstModulosPrograma As New ListInfoModulos
    Protected m_lstModulosRutero As New ListInfoModulos
    Protected m_lstModulosCliente As New ListInfoModulos
    Protected m_lstModulosPedido As New ListInfoModulos

    Public Sub New()
        LoadInfoModulosPrograma()
    End Sub

    Protected MustOverride ReadOnly Property DatabaseFile() As String

    Public MustOverride ReadOnly Property Codigo() As Integer

    Public MustOverride ReadOnly Property Nombre() As String

    Public ReadOnly Property ModulosPrograma() As ListInfoModulos
        Get
            Return m_lstModulosPrograma
        End Get
    End Property

    Public ReadOnly Property ModulosRutero() As ListInfoModulos
        Get
            Return m_lstModulosRutero
        End Get
    End Property

    Public ReadOnly Property ModulosPedido() As ListInfoModulos
        Get
            Return m_lstModulosPedido
        End Get
    End Property

    Public ReadOnly Property ModulosCliente() As ListInfoModulos
        Get
            Return m_lstModulosCliente
        End Get
    End Property

    Public Overridable ReadOnly Property UseQueryFilterInRutero() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overridable ReadOnly Property FilterQueryClientes() As String
        Get
            Return ""
        End Get
    End Property

    Public Overridable ReadOnly Property FilterQueryPedidos() As String
        Get
            Return ""
        End Get
    End Property

    ' Funci�n llamada por el nucleo para que el programa verifique la base de datos
    ' y se defina el objeto de conexi�n a la misma que debe ser utilizado
    Public Sub CheckDatabase()

        GestorBase.SetDatabaseFile(DatabaseFile)
        If Not File.Exists(DatabaseFile) Then
            Programa.CreateDatabase()
        End If

        ' Se pide al programa que haga validaciones adicionales si es requerido
        Programa.InternalCheckDatabase()
    End Sub

    ' LLamada por el n�cleo para que el programa cree el archivo de base de datos
    Public MustOverride Sub CreateDatabase()

    Public MustOverride Function ValidarInicioRutero(ByRef sMensaje As String) As Boolean

    ' Funcion que debe sobrecargar la clase hijo para inicializar la informaci�n 
    ' de los modulos que tendra el programa, debe devolver un objeto de conexi�n
    ' a la base de datos cerrado.
    Protected MustOverride Sub InternalCheckDatabase()

    ' Funci�n que debe llenar la lista con la informaci�n de los m�dulos que estaran
    ' disponibles en la ventana principal
    Public MustOverride Sub LoadInfoModulosPrograma()

    ' Funci�n que debe llenar la lista con la informaci�n de los m�dulos que estaran
    ' disponibles en la ventana del rutero
    Public MustOverride Sub LoadInfoModulosRutero()

    ' Funci�n que debe llenar la lista con la informaci�n de los m�dulos que estaran
    ' disponibles para el cliente del cual se pasa la informaci�n
    Public MustOverride Sub LoadInfoModulosCliente(ByVal rowCliente As DataRow)

    ' Funci�n que debe llenar la lista con la informaci�n de los m�dulos de manejo de
    ' pedidos que estaran disponibles para el cliente del cual se pasa la informaci�n
    Public MustOverride Sub LoadInfoModulosPedidos(ByVal rowCliente As DataRow)

    Public MustOverride Sub LoadQuerysDescarga(ByVal lstQuerys As ListQuerysDescarga)

    Public MustOverride Sub InicioAtencionPedido(ByVal RowCliente As DataRow, ByVal RowPedido As DataRow)

    Public MustOverride Function CerrarAtencionPedido(ByVal RowCliente As DataRow, ByVal RowPedido As DataRow) As Boolean

    'Funcion que valida que se finalice la ruta para hacer la descarga
    Public MustOverride Function ValidarFindeRuta(ByRef sMensaje As String) As Boolean

    'KUXAN DEBE CARGAR LOS DATOS DE LBL Y LOGO
    Public MustOverride Sub CargarDatosIniciales()
End Class
