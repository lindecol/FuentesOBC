Imports Desktop.Data
Imports System.Data
Imports System.Data.Common

Friend Class GestorSeguridad

    Private m_Datos As Desktop.Data.GestorDatosBase
    Public dsSeguridad As New SeguridadDataset
    Private taPerfiles As DbDataAdapter
    Private taProgramas As DbDataAdapter
    Private taModulos As DbDataAdapter
    Private taModulosPerfil As DbDataAdapter
    Private taPermisosEspeciales As DbDataAdapter
    Private taEmpresas As DbDataAdapter
    Private taUsuarios As DbDataAdapter
    Private taPerfilesUsuarios As DbDataAdapter
    Private taCategorias As DbDataAdapter

    Public Sub New()
        m_Datos = GestorDatosFactory.CreateInstance( _
            FrameworkApp.ConnectionType, _
            FrameworkApp.ConnectionString)
        taPerfiles = m_Datos.CreateDataAdapter("Perfiles")
        taProgramas = m_Datos.CreateDataAdapter("Programas")
        taModulos = m_Datos.CreateDataAdapter("Modulos")
        taModulosPerfil = m_Datos.CreateDataAdapter("ModulosPerfil")
        taPermisosEspeciales = m_Datos.CreateDataAdapter("PermisosEspeciales")
        taEmpresas = m_Datos.CreateDataAdapter("Empresas")
        taUsuarios = m_Datos.CreateDataAdapter("Usuarios")
        taPerfilesUsuarios = m_Datos.CreateDataAdapter("PerfilesUsuarios")
        taCategorias = m_Datos.CreateDataAdapter("Categorias")
    End Sub

    Public Sub LoadPerfiles()
        m_Datos.PrepareAdapter(taPerfiles)
        taPerfiles.Fill(dsSeguridad.Perfiles)
    End Sub

    Public Sub UpdatePerfiles()
        m_Datos.PrepareAdapter(taPerfiles)
        taPerfiles.Update(dsSeguridad.Perfiles)
    End Sub

    Public Sub UpdatePerfiles(ByVal RowState As DataRowState)
        m_Datos.PrepareAdapter(taPerfiles)
        Dim dt As DataTable = dsSeguridad.Perfiles.GetChanges(RowState)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            taPerfiles.Update(dt.Select())
        End If
    End Sub

    Public Function CheckPerfilByIdUsuario(ByVal nIdUsuario As Integer) As Boolean
        Dim sSql As String
        sSql = "SELECT * FROM PerfilesUsuarios WHERE (IdUsuario = @p1) "
        Dim dtRes As New DataTable
        m_Datos.Fill(dtRes, CommandType.Text, sSql, nIdUsuario)
        Return dtRes.Rows.Count > 0
    End Function

    Public Sub LoadCategorias()
        m_Datos.PrepareAdapter(taCategorias)
        taCategorias.Fill(dsSeguridad.Categorias)
    End Sub

    Public Sub LoadProgramas()
        m_Datos.PrepareAdapter(taProgramas)
        taProgramas.Fill(dsSeguridad.Programas)
    End Sub

    Public Sub LoadModulos()
        m_Datos.PrepareAdapter(taModulos)
        taModulos.Fill(dsSeguridad.Modulos)
    End Sub

    Public Sub LoadModulosPerfil(ByVal rowPerfil As DataRow)
        Dim sSql As String
        sSql = "SELECT * FROM ModulosPerfil WHERE (IdPerfil = @p1)"
        m_Datos.Fill(dsSeguridad.ModulosPerfil, CommandType.Text, sSql, CInt(rowPerfil("IdPerfil")))
    End Sub

    Public Sub UpdateModulosPerfil()
        m_Datos.PrepareAdapter(taModulosPerfil)
        taModulosPerfil.Update(dsSeguridad.ModulosPerfil)
    End Sub

    Public Sub UpdateModulosPerfil(ByVal RowState As DataRowState)
        m_Datos.PrepareAdapter(taModulosPerfil)
        Dim dt As DataTable = dsSeguridad.ModulosPerfil.GetChanges(RowState)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            taModulosPerfil.Update(dt.Select())
        End If
    End Sub

    Public Sub UpdateUsuarios()
        Try
            m_Datos.BeginTransaction()
            m_Datos.PrepareAdapter(taUsuarios)

            taUsuarios.Update(dsSeguridad.Usuarios)

            m_Datos.CommitTransaction()
        Catch ex As Exception
            m_Datos.RollbackTransaction()
        End Try
    End Sub


    'Public Sub UpdateUsuarios(ByVal RowUsuario As DataRow)
    '    Try
    '        m_Datos.BeginTransaction()
    '        m_Datos.PrepareAdapter(taUsuarios)
    '        taUsuarios.Update(RowUsuario)

    '        m_Datos.CommitTransaction()
    '    Catch ex As Exception
    '        m_Datos.RollbackTransaction()
    '    End Try

    'End Sub

    Public Function CheckUsuarioByUsuario(ByVal sUsuario As String) As Boolean
        Dim sSql As String
        sSql = "SELECT COUNT(*) FROM Usuarios where usuario = @p1"
        Return CBool(m_Datos.ExecuteScalar(CommandType.Text, sSql, sUsuario))
    End Function

    Public Sub LoadPermisosEspeciales()
        m_Datos.PrepareAdapter(taPermisosEspeciales)
        taPermisosEspeciales.Fill(dsSeguridad.PermisosEspeciales)
    End Sub

    Public Sub LoadUsuarios()
        m_Datos.PrepareAdapter(taUsuarios)
        taUsuarios.Fill(dsSeguridad.Usuarios)
    End Sub

    Public Sub LoadEmpresas()
        m_Datos.PrepareAdapter(taEmpresas)
        taEmpresas.Fill(dsSeguridad.Empresas)
    End Sub

    Public Sub LoadPerfilesUsuario(ByVal rowUsuario As DataRow)
        Dim sSql As String
        sSql = "SELECT IdUsuario, IdPerfil FROM PerfilesUsuarios " & _
            "WHERE (IdUsuario = @p1) "
        m_Datos.Fill(dsSeguridad.PerfilesUsuarios, CommandType.Text, sSql, CInt(rowUsuario("IdUsuario")))
    End Sub

    Public Sub UpdatePerfilesUsuario()
        m_Datos.PrepareAdapter(taPerfilesUsuarios)
        taPerfilesUsuarios.Update(dsSeguridad.PerfilesUsuarios)
    End Sub

    Public Sub BeginTransaction()
        m_Datos.BeginTransaction()
    End Sub

    Public Sub CommitTransaction()
        m_Datos.CommitTransaction()
    End Sub

    Public Sub RollbackTransaction()
        m_Datos.RollbackTransaction()
    End Sub

End Class
