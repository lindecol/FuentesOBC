Imports System.Data
Imports Desktop.Data
Imports System.Data.Common

Public Class GestorSesion

    Private m_Datos As Desktop.Data.GestorDatosBase
    Public dsSesion As New SesionDataset
    Private daUsuarios As DataAdapter

    Public Sub New()
        m_Datos = Desktop.Data.GestorDatosFactory.CreateInstance( _
            FrameworkApp.ConnectionType, _
            FrameworkApp.ConnectionString)
        daUsuarios = m_Datos.CreateDataAdapter("Usuarios")
    End Sub

    Public ReadOnly Property UsuarioActual() As System.Data.DataRow
        Get
            If dsSesion.Usuarios.Count > 0 Then
                Return dsSesion.Usuarios(0)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private m_rowEmpresaActual As SesionDataset.EmpresasRow
    Public Property EmpresaActual() As System.Data.DataRow
        Get
            Return m_rowEmpresaActual
        End Get
        Set(ByVal value As System.Data.DataRow)
            m_rowEmpresaActual = CType(value, SesionDataset.EmpresasRow)
        End Set
    End Property

    Public Sub LoadUsuario(ByVal sUsuario As String)
        Dim sSql As String
        sSql = "SELECT * FROM Usuarios WHERE (Usuario = @p1)  "
        m_Datos.Fill(dsSesion.Usuarios, CommandType.Text, sSql, sUsuario)
    End Sub

    Public Sub LoadUsuarios()
        Dim sSql As String
        sSql = "SELECT * FROM Usuarios "
        m_Datos.Fill(dsSesion.Usuarios, CommandType.Text, sSql)
    End Sub

    Public Shared Sub LoadUsuarios(ByRef dtUsuarios As DataTable)
        Dim Datos As Desktop.Data.GestorDatosBase = Desktop.Data.GestorDatosFactory.CreateInstance( _
          FrameworkApp.ConnectionType, _
          FrameworkApp.ConnectionString)
        Dim sSql As String
        sSql = "SELECT * FROM Usuarios "
        Datos.Fill(dtUsuarios, CommandType.Text, sSql)
    End Sub

    Public Sub LoadProgramas()
        Dim sSql As String
        sSql = "SELECT * FROM Programas "
        m_Datos.Fill(dsSesion.Programas, CommandType.Text, sSql)
    End Sub

    Public Sub LoadEmpresas()
        Dim sSql As String
        sSql = "SELECT * FROM Empresas "
        dsSesion.Empresas.Clear()
        m_Datos.Fill(dsSesion.Empresas, CommandType.Text, sSql)
    End Sub

    Public Sub LoadEmpresaUsuarioActual()
        Dim sSql As String
        sSql = "SELECT * FROM Empresas WHERE IdEmpresa = @p1"
        m_Datos.Fill(dsSesion.Empresas, CommandType.Text, sSql, dsSesion.Usuarios(0).IdEmpresa)
        If dsSesion.Empresas.Rows.Count > 0 Then
            EmpresaActual = dsSesion.Empresas(0)
        End If
    End Sub

    Private Sub LoadCategorias()
        Dim sSql As String
        sSql = "SELECT * FROM Categorias ORDER BY IdCategoriaPadre,Orden, Nombre "
        m_Datos.Fill(dsSesion.Categorias, CommandType.Text, sSql)
    End Sub

    Private Sub LoadModulos()
        Dim sSql As String
        sSql = "SELECT * FROM Modulos WHERE (Activo = 1) AND IdTipoModulo<>" & CStr(TiposModulo.ModuloMovil) & " ORDER BY Nombre"
        m_Datos.Fill(dsSesion.Modulos, CommandType.Text, sSql)
    End Sub

    Private Sub LoadModulosUsuario(ByVal IdUsuario As Integer)
        Dim sSql As String
        sSql = "SELECT * FROM Modulos " & _
            "WHERE (Activo = 1) AND IdTipoModulo<>" & CStr(TiposModulo.ModuloMovil) & " AND (IdModulo IN ( " & _
            "  SELECT ModulosPerfil.IdModulo " & _
            "  FROM ModulosPerfil INNER JOIN PerfilesUsuarios " & _
            "  ON ModulosPerfil.IdPerfil = PerfilesUsuarios.IdPerfil " & _
            "  WHERE (PerfilesUsuarios.IdUsuario = @p1))) " & _
            "ORDER BY Nombre"
        m_Datos.Fill(dsSesion.Modulos, CommandType.Text, sSql, IdUsuario)
    End Sub

    Private Sub LoadPermisosUsuario(ByVal IdUsuario As Integer)
        Dim sSql As String
        sSql = "SELECT ModulosPerfil.* FROM ModulosPerfil INNER JOIN PerfilesUsuarios " & _
            "ON ModulosPerfil.IdPerfil = PerfilesUsuarios.IdPerfil " & _
            "WHERE (PerfilesUsuarios.IdUsuario = @p1) "
        m_Datos.Fill(dsSesion.ModulosPerfil, CommandType.Text, sSql, IdUsuario)
    End Sub

    Public Sub LoadInfoInicioSesion()
        LoadProgramas()
        LoadCategorias()
        If dsSesion.Usuarios(0).SuperUser Then
            LoadModulos()
        Else
            LoadModulosUsuario(dsSesion.Usuarios(0).IdUsuario)
            LoadPermisosUsuario(dsSesion.Usuarios(0).IdUsuario)
        End If
        'LoadEmpresas()
        'If dsSesion.Empresas.Rows.Count > 0 Then
        '    EmpresaActual = dsSesion.Empresas(0)
        'End If
    End Sub
End Class
