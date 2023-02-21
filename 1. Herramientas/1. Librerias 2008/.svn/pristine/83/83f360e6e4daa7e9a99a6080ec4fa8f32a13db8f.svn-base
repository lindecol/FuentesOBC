Imports System.Data
Imports System.Data.Common
Imports Desktop.Data

Friend Class GestorConfiguracion

    Private m_Datos As Desktop.Data.GestorDatosBase
    Public dsConfig As New ConfiguracionDataSet
    Private dtaEmpresas As DbDataAdapter
    Private dtaCategorias As DbDataAdapter
    Private dtaProgramas As DbDataAdapter
    Private dtaModulos As DbDataAdapter
    Private dtaTiposModulo As DbDataAdapter
    Private dtaPermisosEspeciales As DbDataAdapter
    Private dtaUsuarios As DbDataAdapter

    Public Sub New()
        m_Datos = GestorDatosFactory.CreateInstance( _
            FrameworkApp.ConnectionType, _
            FrameworkApp.ConnectionString)

        dtaEmpresas = m_Datos.CreateDataAdapter("Empresas")
        dtaCategorias = m_Datos.CreateDataAdapter("Categorias")
        dtaProgramas = m_Datos.CreateDataAdapter("Programas")
        dtaModulos = m_Datos.CreateDataAdapter("Modulos")
        dtaTiposModulo = m_Datos.CreateDataAdapter("TiposModulo")
        dtaPermisosEspeciales = m_Datos.CreateDataAdapter("PermisosEspeciales")
        dtaUsuarios = m_Datos.CreateDataAdapter("Usuarios")
    End Sub

	Public Sub LoadCategorias()
		m_Datos.PrepareAdapter(dtaCategorias)
		dtaCategorias.Fill(dsConfig.Categorias)
	End Sub

	Public Sub LoadUsuarios()
		m_Datos.PrepareAdapter(dtaUsuarios)
		dtaUsuarios.Fill(dsConfig.Usuarios)
	End Sub

    Public Sub LoadEmpresas()
        m_Datos.PrepareAdapter(dtaEmpresas)
        dtaEmpresas.Fill(dsConfig.Empresas)
    End Sub

	Public Sub LoadModulos()
		m_Datos.PrepareAdapter(dtaModulos)
		dtaModulos.Fill(dsConfig.Modulos)
	End Sub

    Public Sub LoadPermisosEspeciales()
        m_Datos.PrepareAdapter(dtaPermisosEspeciales)
        dtaPermisosEspeciales.Fill(dsConfig.PermisosEspeciales)
    End Sub

    Public Sub LoadProgramas()
        m_Datos.PrepareAdapter(dtaProgramas)
        dtaProgramas.Fill(dsConfig.Programas)
    End Sub

    Public Sub LoadTiposModulos()
        m_Datos.PrepareAdapter(dtaTiposModulo)
        dtaTiposModulo.Fill(dsConfig.TiposModulo)
    End Sub

    Public Sub UpdateCategorias1()
        m_Datos.PrepareAdapter(dtaCategorias)
        dtaCategorias.Update(dsConfig.Categorias)
    End Sub

	Public Sub UpdateCategorias(ByVal RowState As System.Data.DataRowState)
		Try
			m_Datos.PrepareAdapter(dtaCategorias)
			Dim dt As DataTable = dsConfig.Categorias.GetChanges(RowState)
			If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
				dtaCategorias.Update(CType(dt, ConfiguracionDataSet.CategoriasDataTable))
			End If

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

    'Public Sub UpdateEmpresa(ByVal rowEmpresa As DataRow)
    '    m_Datos.PrepareAdapter(dtaEmpresas)
    '    dtaEmpresas.Update(rowEmpresa)
    'End Sub

    Public Sub UpdateEmpresas()
        m_Datos.PrepareAdapter(dtaEmpresas)
        dtaEmpresas.Update(dsConfig.Empresas)
        dsConfig.Empresas.AcceptChanges()
    End Sub

    Public Sub UpdateEmpresas(ByVal dtEmpresas As System.Data.DataTable)
        m_Datos.PrepareAdapter(dtaEmpresas)
        Dim dt As ConfiguracionDataSet.EmpresasDataTable
        dt = CType(dtEmpresas, ConfiguracionDataSet.EmpresasDataTable)
        dtaEmpresas.Update(dt)
        dsConfig.Empresas.AcceptChanges()
    End Sub

    Public Sub UpdateModulos()
        m_Datos.PrepareAdapter(dtaModulos)
        dtaModulos.Update(dsConfig.Modulos)
    End Sub

    Public Sub UpdateModulos(ByVal RowState As System.Data.DataRowState)
        m_Datos.PrepareAdapter(dtaModulos)
        Dim dt As DataTable = dsConfig.Modulos.GetChanges(RowState)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dtaModulos.Update(CType(dt, ConfiguracionDataSet.ModulosDataTable))
        End If
    End Sub

    Public Sub UpdatePermisosEspeciales(ByVal RowState As System.Data.DataRowState)
        m_Datos.PrepareAdapter(dtaPermisosEspeciales)
        Dim dt As DataTable = dsConfig.PermisosEspeciales.GetChanges(RowState)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dtaPermisosEspeciales.Update(CType(dt, ConfiguracionDataSet.PermisosEspecialesDataTable))
        End If
    End Sub

    Public Sub UpdateProgramas()
        m_Datos.PrepareAdapter(dtaProgramas)
        dtaProgramas.Update(dsConfig.Programas)
    End Sub

    Public ReadOnly Property dsConfigClient() As DataSet
        Get
            Return Me.dsConfig
        End Get
    End Property


    Public Shared Function CheckBaseDatos() As String
        Dim DatosInicio As Desktop.Data.GestorDatosBase = GestorDatosFactory.CreateInstance( _
                 FrameworkApp.ConnectionType, _
                 FrameworkApp.ConnectionString)
        Try
            DatosInicio.CreateDataAdapter("Empresas")
            Return Nothing
        Catch ex As Exception
            'CREAR EL ESQUEMA
            'CARGAR ARCHIVO SQL SCRIPTA
            Dim sScript As String = ""
            Dim fResoruce As New Resources.ResourceManager("Desktop.Framework.Resources", System.Reflection.Assembly.GetExecutingAssembly)
            If FrameworkApp.ConnectionType = Desktop.Data.ConnectionTypes.SqlClient Then
                Try
                    sScript = fResoruce.GetString("ScriptSQLServer")
                    If sScript Is Nothing Then
                        Return "No existe el archivo de Script de la base de datos!"
                    End If
                    If sScript.Trim.Length > 0 Then
                        Return EjecutarScript(DatosInicio, sScript)
                    Else
                        Return "No existe el archivo de Script de la base de datos!"
                    End If
                    Return Nothing
                Catch ex1 As Exception
                    Return ex.Message
                End Try
            ElseIf FrameworkApp.ConnectionType = Desktop.Data.ConnectionTypes.SqlServerCe Then
                Try
                    sScript = fResoruce.GetString("ScriptSQLMobile")
                    If sScript Is Nothing Then
                        Return "No existe el archivo de Script de la base de datos!"
                    End If
                    If sScript.Trim.Length > 0 Then
                        'CREAR LA BASE DE DATOS
                        Return CrearDaseDatosSQLMobile(DatosInicio, sScript)
                    Else
                        Return "No existe el archivo de Script de la base de datos!"
                    End If
                    Return Nothing
                Catch ex1 As Exception
                    Return ex.Message
                End Try
            Else
                Return Nothing
            End If
        End Try
    End Function

    'FUNCION PARA EJECUTAR EL SCRIPT
    Private Shared Function EjecutarScript(ByVal cnDatos As GestorDatosBase, ByVal sScript As String) As String
        Try
            cnDatos.ExecuteNonQuery(CommandType.Text, sScript)
            Return Nothing
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function ExisteUsuarioInicial() As Boolean
        Dim sSql As String
        sSql = "SELECT * FROM Usuarios WHERE (Usuario = @p1)"
        m_Datos.Fill(dsConfig.Usuarios, CommandType.Text, sSql, "admin")
        Return Me.dsConfig.Usuarios.Rows.Count > 0
    End Function

    Public Function CrearUsuarioInicial(ByVal IdEmpresa As Integer) As Boolean
        m_Datos.PrepareAdapter(Me.dtaUsuarios)
        Try
            Me.dsConfig.Usuarios.AddUsuariosRow("admin", "admin", "Administrador", IdEmpresa, "", True, True, FrameworkApp.MultiEmpresa, Now, True)
            Me.dtaUsuarios.Update(Me.dsConfig.Usuarios)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function CrearEmpresaInicial() As Boolean
        m_Datos.PrepareAdapter(Me.dtaEmpresas)
        Try
            Me.dsConfig.Empresas.AddEmpresasRow(FrameworkApp.NITEmpresaAplicacion, FrameworkApp.EmpresaAplicacion, FrameworkApp.EmpresaAplicacion, "", "", "", "", "", Nothing)
            Me.dtaEmpresas.Update(Me.dsConfig.Empresas)
            Me.dsConfig.Clear()
            Me.LoadEmpresas()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function NumeroEmpresas() As Integer
        If Me.dsConfig.Empresas.Rows.Count > 0 Then
            Return Me.dsConfig.Empresas(0).IdEmpresa
        Else
            Return -1
        End If
    End Function

    Public Sub DeleteUsuariosEmpresas(ByVal IdEmpresa As Integer)
        Dim sSql As String
        sSql = "DELETE FROM Usuarios WHERE (IdEmpresa = @IdEmpresa)"
        m_Datos.ExecuteNonQuery(CommandType.Text, sSql)
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

    Public Sub LoadConfiguracion()
        Me.LoadEmpresas()
        Me.LoadCategorias()
        Me.LoadProgramas()
        Me.LoadModulos()
        Me.LoadTiposModulos()
        Me.LoadPermisosEspeciales()
        Me.LoadUsuarios()
    End Sub

    Private Sub EliminarRegistros(ByVal dtTabla As DataTable)
        For Each rw As DataRow In dtTabla.Rows
            rw.Delete()
        Next
    End Sub

    Public Sub UpdatePermisosEspeciales()
        m_Datos.PrepareAdapter(dtaPermisosEspeciales)
        dtaPermisosEspeciales.Update(dsConfig.PermisosEspeciales)
    End Sub

    Public Sub UpdateTiposModulos()
        m_Datos.PrepareAdapter(dtaTiposModulo)
        dtaTiposModulo.Update(dsConfig.TiposModulo)
    End Sub

    Public Sub UpdateUsuarios()
        m_Datos.PrepareAdapter(dtaUsuarios)
        dtaUsuarios.Update(dsConfig.Usuarios)
    End Sub

    Public Sub ImportConfiguracion(ByVal sNombreArchivo As String)
        Dim fTemp As IO.StreamReader = New IO.StreamReader(sNombreArchivo)
        Try
            'TEMPORAL CARGA DE ARCHIVO PARA PRUEBAS
            Dim sDatosConfiguracion As String = ""
            fTemp = New IO.StreamReader(sNombreArchivo)
            Me.LoadEmpresas()
            Me.EliminarRegistros(Me.dsConfig.Empresas)
            Me.UpdateEmpresas()
            Me.LoadCategorias()
            Me.EliminarRegistros(Me.dsConfig.Categorias)
            Me.UpdateCategorias1()
            Me.LoadProgramas()
            Me.EliminarRegistros(Me.dsConfig.Programas)
            Me.UpdateProgramas()
            Me.LoadModulos()
            Me.EliminarRegistros(Me.dsConfig.Modulos)
            Me.UpdateModulos()
            Me.LoadTiposModulos()
            Me.EliminarRegistros(Me.dsConfig.TiposModulo)
            Me.UpdateTiposModulos()
            Me.LoadPermisosEspeciales()
            Me.EliminarRegistros(Me.dsConfig.PermisosEspeciales)
            Me.UpdatePermisosEspeciales()
            'Me.LoadUsuarios()
            'Me.EliminarRegistros(Me.dsConfig.Usuarios)
            'Me.UpdateUsuarios()
            'CARGAR CONFIGURACION
            Me.dsConfig.ReadXml(fTemp)
            Me.UpdateEmpresas()
            Me.UpdateCategorias1()
            Me.UpdateProgramas()
            Me.UpdateModulos()
            Me.UpdateTiposModulos()
            Me.UpdatePermisosEspeciales()
            Me.UpdateUsuarios()
            MsgBox("Configuración Importada Correctamente!!", MsgBoxStyle.Information, "Configuracion")
        Catch ex As Exception
            MsgBox("ERROR: Importando Configuración -- " + ex.Message, MsgBoxStyle.Critical, "Configuracion")
        Finally
            fTemp.Close()
        End Try
    End Sub

    Public Shared Function CrearDaseDatosSQLMobile(ByVal cnDatos As GestorDatosBase, ByVal sScript As String) As String
        'CREAR LA BASE DE DATOS
        Dim devBaseDatos As New System.Data.SqlServerCe.SqlCeEngine
        Try
            devBaseDatos.LocalConnectionString = FrameworkApp.ConnectionString
            devBaseDatos.CreateDatabase()
            Dim sCrearTablas() As String = sScript.Split(";"c)
            cnDatos.OpenConnection()
            For Each sTabla As String In sCrearTablas
                If sTabla.Trim.Length > 0 Then
                    cnDatos.ExecuteNonQuery(CommandType.Text, sTabla)
                End If
            Next
            Return Nothing
        Catch ex1 As Exception
            MsgBox(ex1.Message)
            Return ex1.Message
        Finally
            cnDatos.CloseConnection()
        End Try
    End Function

	Public Function ObtenerUltimaCategoria() As Integer
		Dim sSql As String
		sSql = "SELECT MAX(IdCategoria) FROM Categoria"
		Return CInt(m_Datos.ExecuteScalar(CommandType.Text, sSql))
	End Function

	Public Function ObtenerUltimoModulo() As Integer
		Dim sSql As String
		sSql = "SELECT MAX(IdModulo) FROM Modulos"
		Return CInt(m_Datos.ExecuteScalar(CommandType.Text, sSql))
	End Function
End Class

