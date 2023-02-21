Imports System.Reflection
Imports System.IO
Imports System.Runtime.InteropServices

Friend Class MainForm
    Implements INucleo


    Private CurrentView As BaseViewControl = Nothing
    Private MergeMenuItem As ToolStripMenuItem = Nothing
    Private MergeToolStrip As ToolStrip = Nothing


    Public Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Try
        Me.Text = FrameworkApp.ApplicationName + " Versión: " + FrameworkApp.ApplicationVersion
        Me.NavegationPanelControl1.Visible = False
        Me.LoginControl1.Visible = True
        dsSesion = FrameworkApp.Sesion.dsSesion
        FrameworkApp.Initialize(Me)
        If FrameworkApp.ConnectionType = ConnectionTypes.SqlServerCe Then
            lblServidor.Text = "Servidor: " & FrameworkApp.ConnectionString.Replace("Data Source", "").Replace("=", "").Replace("""", "") & "    "
            lblDataBase.Text = ""
        Else
            '"Data Source=SRVDESARROLLO;Initial Catalog=QuickERP;Integrated Security=False;User ID=sa;Password=Desktop"
            Dim sAux As String = ""
            If FrameworkApp.ConnectionString.IndexOf("Data Source") >= 0 Then
                sAux = FrameworkApp.ConnectionString.Substring(FrameworkApp.ConnectionString.IndexOf("Data Source") + 12)
                If sAux.IndexOf(";") >= 0 Then
                    sAux = sAux.Substring(0, sAux.IndexOf(";")).Replace("=", "").Trim
                Else
                    sAux = ""
                End If
                lblServidor.Text = "Servidor: " & sAux & Space(8)
                sAux = FrameworkApp.ConnectionString.Substring(FrameworkApp.ConnectionString.IndexOf("Initial Catalog") + 15)
                If sAux.IndexOf(";") >= 0 Then
                    sAux = sAux.Substring(0, sAux.IndexOf(";")).Replace("=", "").Trim
                Else
                    sAux = ""
                End If
                lblDataBase.Text = "Base Datos: " & sAux
            End If
        End If
        GestorSesion.LoadUsuarios(Me.dsSesion.Tables("Usuarios"))
        If Me.dsSesion.Tables("Usuarios").Rows.Count = 0 Then
            If FrameworkApp.CrearUsuarioInicial Then
                If Not MainConfiguracionForm.CrearUsuarioInicial() Then
                    MsgBox("Error creando Usuario inicial!!", MsgBoxStyle.Critical, "Validación")
                    Me.Close()
                End If
            End If
        End If
        'IniciarSesion()
        Me.btnCerrarSesion.Visible = False
        Me.btnCambiarClave.Visible = False


        Me.mnuExportarConfiguracion.Visible = FrameworkApp.ExportarConfiguracion
        Me.mnuImportarConfiguracion.Visible = FrameworkApp.ImportarConfiguracion

        If FrameworkApp.ExpanderArbol Then
            Me.NavegationPanelControl1.Arbol.ExpandAll()
        Else
            Me.NavegationPanelControl1.Arbol.CollapseAll()
        End If



    End Sub

    Private Sub LoginControl_CancelarClic(ByVal sender As System.Object, ByVal e As EventArgs) Handles LoginControl1.Cancel
        Me.Close()
    End Sub

    Public Sub IniciarSesion()
        If SeleccionarEmpresaDialog.Run() Then
            FrameworkApp.Sesion.LoadInfoInicioSesion()

            If FrameworkApp.listImages IsNot Nothing And FrameworkApp.imagesMenu IsNot Nothing Then
                NavegationPanelControl1.imagesMenu = FrameworkApp.imagesMenu
                NavegationPanelControl1.configImages = FrameworkApp.listImages
            End If
            NavegationPanelControl1.Inicializar()
            Me.NavegationPanelControl1.Visible = True
            Me.LoginControl1.Visible = False
            DBUtils.SetCurrentRow(bsUsuarios, FrameworkApp.Sesion.UsuarioActual)
            DBUtils.SetCurrentRow(bsEmpresas, FrameworkApp.Sesion.EmpresaActual)
            Me.lblEmpresa.Text = CStr(FrameworkApp.Sesion.EmpresaActual("NombreCorto"))
            Me.lblUsuario.Text = CStr(FrameworkApp.Sesion.UsuarioActual("Nombre"))
            pnlSesion.Visible = True
            Me.btnCerrarSesion.Visible = True
            Me.btnCambiarClave.Visible = FrameworkApp.CambiarClaveUsuario

            HoraInicioSesion = Now
        End If
    End Sub

    Private HoraInicioSesion As DateTime

    Private Sub SetView(ByVal objAssembly As Assembly, ByVal ViewType As System.Type)
        Dim bCreateInstance As Boolean = True

        If CurrentView IsNot Nothing Then
            If CurrentView.GetType() IsNot ViewType Then
                If MergeMenuItem IsNot Nothing Then
                    MenuStrip1.Items.Remove(MergeMenuItem)
                    MergeMenuItem = Nothing
                End If
                If MergeToolStrip IsNot Nothing Then
                    ToolStripContainer1.TopToolStripPanel.Controls.Remove(MergeToolStrip)
                    MergeToolStrip = Nothing
                End If
                CurrentView.Close()
                CurrentView.Dispose()
            Else
                bCreateInstance = False
            End If
        End If

        If bCreateInstance Then
            CurrentView = DirectCast(objAssembly.CreateInstance(ViewType.FullName), Desktop.Framework.BaseViewControl)
            CurrentView.Dock = DockStyle.Fill
            CurrentView.Parent = pnlModulos

            If CurrentView.MergeMenuStrip IsNot Nothing Then
                MergeMenuItem = New ToolStripMenuItem
                MergeMenuItem.Text = CurrentView.MergeMenuStrip.Text
                MergeMenuItem.DropDown = CurrentView.MergeMenuStrip
                MenuStrip1.Items.Insert(1, MergeMenuItem)
            Else
                MergeMenuItem = Nothing
            End If

            If CurrentView.MergeToolStrip IsNot Nothing Then
                MergeToolStrip = CurrentView.MergeToolStrip
                MergeToolStrip.Dock = DockStyle.None
                MergeToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
                MergeToolStrip.Location = New System.Drawing.Point(ToolStrip1.Width + 4, ToolStrip1.Height)
                ToolStripContainer1.TopToolStripPanel.Controls.Add(MergeToolStrip)
            Else
                MergeToolStrip = Nothing
            End If
        End If
    End Sub

    Private Sub LoginControl1_Login(ByVal sender As Object, ByVal e As LoginEventArgs) Handles LoginControl1.Login
        IniciarSesion()
    End Sub

    Private Sub NavegationPanelControl1_SelectionChanged(ByVal rowModulo As DataRow) Handles NavegationPanelControl1.SelectionChanged
        ' Se revisa la informaciòn configurada del mòdulo y dependiendo del tipo de mòdulo se
        ' crea la instancia correspondiente
        'SOLO MOSTRAR MODULOS
        If (rowModulo.GetType.Name = "CategoriasRow") Then
            'HACER CLI EN EL PRIMER ITEM
            Exit Sub
        End If
        Dim sFileName As String = ""
        Try
            Dim ViewType As Type


            Select Case CInt(rowModulo("IdTipoModulo"))
                Case TiposModulo.ModuloNucleo
                    ViewType = Assembly.GetCallingAssembly().GetType(CStr(rowModulo("ClassURL")))
                    If ViewType IsNot Nothing Then
                        SetView(Assembly.GetCallingAssembly(), ViewType)
                    End If
                Case TiposModulo.ModuloPrograma
                    If dsSesion.Tables("Programas").Rows.Count >= 0 Then
                        Dim objAssembly As Assembly
                        Dim rowPrograma As DataRow
                        rowPrograma = dsSesion.Tables("Programas").Rows.Find(rowModulo("IdPrograma"))
                        'sFileName = Path.Combine(Path.GetDirectoryName(FrameworkApp.PathEnsambladoNucleo), CStr(rowPrograma("Ensamblado")))
                        sFileName = Path.Combine(Application.StartupPath, CStr(rowPrograma("Ensamblado")))
                        objAssembly = Assembly.LoadFile(sFileName)
                        ViewType = objAssembly.GetType(CStr(rowModulo("ClassURL")))
                        If ViewType IsNot Nothing Then
                            SetView(objAssembly, ViewType)
                        End If
                    End If
                Case TiposModulo.ModuloWeb

            End Select

        Catch ex As Exception
            Using sw As StreamWriter = New StreamWriter("ErrorContabilidad280808.txt")
                sw.WriteLine(sFileName)
                sw.WriteLine(ex.GetType().FullName & vbCrLf & ex.Message)
                sw.Close()
            End Using
            MsgBox(sFileName)
            ShowError(ex.GetType().FullName & vbCrLf & ex.Message)
        End Try


    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Salir()
    End Sub

    Private Sub btnCerrarSesion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrarSesion.Click
        CerrarSesion()
    End Sub

    Private Sub AcercaDeCentauroToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcercaDeCentauroToolStripMenuItem.Click
        AcerdaDe()
    End Sub

    Private Sub SalirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalirToolStripMenuItem.Click
        Salir()
    End Sub

    Private Sub Salir()
        If Confirmar("Esta seguro que desea salir de la aplicación") Then
            Me.Close()
        End If
    End Sub

    Private Sub CerrarSesion()
        If Confirmar("Esta seguro que desea cerrar sesión?") Then
            Me.btnCambiarClave.Visible = False
            Me.NavegationPanelControl1.Visible = False
            Me.LoginControl1.Visible = True
            Me.pnlSesion.Visible = False
            FrameworkApp.Sesion.EmpresaActual = Nothing
            CloseCurrentView()
            Me.btnCerrarSesion.Visible = False
            FrameworkApp.CerrarSession()
            HoraInicioSesion = DateTime.MinValue
        End If
    End Sub

    Private Sub CloseCurrentView()
        If CurrentView IsNot Nothing Then
            CurrentView.Close()
            CurrentView.Parent = Nothing
            CurrentView.Dispose()
            CurrentView = Nothing
        End If
    End Sub

    Private Sub AcerdaDe()
        AcercaDeDialog.Run()
    End Sub

    Private Sub CerrarSesiónToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CerrarSesiónToolStripMenuItem.Click
        CerrarSesion()
    End Sub

    Public ReadOnly Property EmpresaActual() As System.Data.DataRow Implements INucleo.EmpresaActual
        Get
            Return FrameworkApp.Sesion.EmpresaActual
        End Get
    End Property

    Public ReadOnly Property UsuarioActual() As System.Data.DataRow Implements INucleo.UsuarioActual
        Get
            Return FrameworkApp.Sesion.UsuarioActual
        End Get
    End Property

    Public Function GetIdModulo(ByVal ctlView As BaseViewControl) As Integer Implements INucleo.GetIdModulo
        Dim rows() As DataRow
        rows = FrameworkApp.Sesion.dsSesion.Tables("Modulos").Select(String.Format("ClassURL = '{0}'", ctlView.GetType().FullName.ToString()), "")
        If rows.Length > 0 Then
            Return CInt(rows(0)("IdModulo"))
        Else
            Return -1
        End If
    End Function

    Public Function PermiteBorrar(ByVal IdModulo As Integer) As Boolean Implements INucleo.PermiteBorrar
        If CBool(FrameworkApp.Sesion.UsuarioActual("SuperUser")) Then
            Return True
        Else
            Dim rows() As DataRow
            rows = dsSesion.Tables("ModulosPerfil").Select(String.Format("IdModulo = {0} AND PermitirBorrar = 1", IdModulo), "")
            Return rows.Length > 0
        End If
    End Function

    Public Function PermiteEditar(ByVal IdModulo As Integer) As Boolean Implements INucleo.PermiteEditar
        If CBool(FrameworkApp.Sesion.UsuarioActual("SuperUser")) Then
            Return True
        Else
            Dim rows() As DataRow
            rows = dsSesion.Tables("ModulosPerfil").Select(String.Format("IdModulo = {0} AND PermitirModificar = 1", IdModulo), "")
            Return rows.Length > 0
        End If
    End Function

    Public Function PermiteImprimir(ByVal IdModulo As Integer) As Boolean Implements INucleo.PermiteImprimir
        If CBool(FrameworkApp.Sesion.UsuarioActual("SuperUser")) Then
            Return True
        Else
            Dim rows() As DataRow
            rows = dsSesion.Tables("ModulosPerfil").Select(String.Format("IdModulo = {0} AND PermitirImprimir = 1", IdModulo), "")
            Return rows.Length > 0
        End If
    End Function

    Public Function PermiteInsertar(ByVal IdModulo As Integer) As Boolean Implements INucleo.PermiteInsertar
        If CBool(FrameworkApp.Sesion.UsuarioActual("SuperUser")) Then
            Return True
        Else
            Dim rows() As DataRow
            rows = dsSesion.Tables("ModulosPerfil").Select(String.Format("IdModulo = {0} AND PermitirInsertar = 1", IdModulo), "")
            Return rows.Length > 0
        End If
    End Function

    Public Function PermitePermisoEspecial(ByVal IdModulo As Integer, ByVal IdPermiso As Integer) As Boolean Implements INucleo.PermitePermisoEspecial
        If CBool(FrameworkApp.Sesion.UsuarioActual("SuperUser")) Then
            Return True
        Else
            Dim rows() As DataRow
            Dim Permisos As List(Of Integer)
            rows = dsSesion.Tables("ModulosPerfil").Select(String.Format("IdModulo = {0} AND PermisosEspeciales IS NOT NULL", IdModulo), "")
            For I As Integer = 0 To rows.Length - 1
                Permisos = GetPermisosActuales(rows(I))
                If Permisos.Contains(IdPermiso) Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function




    Private Function GetPermisosActuales(ByVal row As DataRow) As List(Of Integer)
        Dim Permisos As New List(Of Integer)
        If row IsNot Nothing AndAlso Not DBUtils.IsNullOrEmpty(row("PermisosEspeciales")) Then
            Dim actuales, s As String
            actuales = CStr(row("PermisosEspeciales"))
            For Each s In actuales.Split("|"c)
                If Not DBUtils.IsNullOrEmpty(s) Then
                    Permisos.Add(CInt(s))
                End If
            Next
        End If
        Return Permisos
    End Function

    Public Sub RegisterModalForm(ByVal frm As System.Windows.Forms.Form) Implements INucleo.RegisterModalForm

    End Sub

    Private Sub btnCambiarClave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCambiarClave.Click
        If Desktop.Framework.FrameworkApp.UsuarioActual.Usuario IsNot Nothing Then
            If Not DBUtils.IsNullOrEmpty(Desktop.Framework.FrameworkApp.UsuarioActual.Usuario) Then
                If Not FrameworkApp.Sesion.EmpresaActual Is Nothing Then
                    CambiarClaveDialog.Run(Desktop.Framework.FrameworkApp.UsuarioActual.Usuario)
                End If
            End If
        End If
    End Sub

    Private Sub mnuExportarConfiguracion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportarConfiguracion.Click
        Me.svDialogo.ShowDialog()
        If Me.svDialogo.FileName IsNot Nothing Then
            If Me.svDialogo.FileName <> "" Then
                Dim gDatos As New GestorConfiguracion
                gDatos.LoadConfiguracion()
                gDatos.dsConfig.WriteXml(svDialogo.FileName)
                MsgBox("Configuración Exportada Correctamente!!", MsgBoxStyle.Information, "Configuracion")
            End If
        End If
    End Sub

    Private Sub mnuImportarConfiguracion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuImportarConfiguracion.Click
        Me.opDialogo.ShowDialog()
        If Me.opDialogo.FileName IsNot Nothing Then
            If Me.opDialogo.FileName <> "" Then
                Dim gDatos As New GestorConfiguracion
                gDatos.ImportConfiguracion(Me.opDialogo.FileName)
            End If
        End If
    End Sub

    Private Sub MainForm_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        FrameworkApp.Closed()
    End Sub

    Public ReadOnly Property InicioSesion() As Date Implements INucleo.InicioSesion
        Get
            Return HoraInicioSesion
        End Get
    End Property
End Class