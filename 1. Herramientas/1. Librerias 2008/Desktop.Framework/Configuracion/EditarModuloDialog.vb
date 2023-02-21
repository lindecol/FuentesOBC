Imports System.Reflection
Imports System.IO

Friend Class EditarModuloDialog

    Private Datos As GestorConfiguracion
    Private m_Nuevo As Boolean = False
    Private m_rowModulo As DataRow
    Private m_rowCategoia As DataRow
    Private m_bCargar As Boolean = False

    Public Shared Sub Run(ByVal Datos As GestorConfiguracion, ByVal rowCategoria As DataRow, Optional ByVal rowModulo As DataRow = Nothing)
        Dim frm As New EditarModuloDialog
        'CREAR EL GESTOR
        frm.Datos = Datos
        frm.m_rowCategoia = CType(rowCategoria, DataRow)
        frm.m_rowModulo = CType(rowModulo, DataRow)
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub EditarModuloForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Se reconfigura el databinding
        dsConfig = Datos.dsConfig
        m_bCargar = False
        bsTiposModulo.DataSource = dsConfig
        bsProgramas.DataSource = dsConfig
        bsModulos.DataSource = dsConfig
        Me.bsModulos.DataMember = "Modulos"
        Me.bsProgramas.DataMember = "Programas"
        Me.bsTiposModulo.DataMember = "TiposModulo"
        Me.cbTipoPrograma.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.bsModulos, "IdTipoModulo", True))
        Me.cbClase.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsModulos, "ClassURL", True))
        Me.txtURL.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsModulos, "ClassURL", True))
        Me.NombreTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsModulos, "Nombre", True))
        Me.cbPrograma.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.bsModulos, "IdPrograma", True))
        If m_rowModulo Is Nothing Then
            m_Nuevo = True
            bsModulos.AddNew()
            m_rowModulo = CType(DBUtils.GetCurrentRow(bsModulos), DataRow)
            m_rowModulo("Activo") = True
            If m_rowCategoia IsNot Nothing Then
                m_rowModulo("IdCategoria") = m_rowCategoia("IdCategoria")
            End If
            btnActualizarPermisosEspeciales.Visible = False
        Else
            m_Nuevo = False
            DBUtils.SetCurrentRow(bsModulos, m_rowModulo)
            btnActualizarPermisosEspeciales.Visible = True
        End If
        m_bCargar = True
    End Sub

    Private Sub cbTipoPrograma_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTipoPrograma.SelectedValueChanged
        If cbTipoPrograma.SelectedValue Is Nothing Then
            pnlProgramaClase.Visible = False
            pnlURL.Visible = False
        Else
            If m_bCargar Then
                Select Case CInt(cbTipoPrograma.SelectedValue)
                    Case TiposModulo.ModuloNucleo
                        pnlURL.Visible = False
                        pnlProgramaClase.Visible = True
                        pnlPrograma.Visible = False
                        pnlClase.Visible = True
                        cbPrograma.SelectedIndex = -1
                        LoadClasesModulosNucleo()
                        cbClase.Text = ""
                    Case TiposModulo.ModuloPrograma
                        pnlURL.Visible = False
                        pnlProgramaClase.Visible = True

                        pnlPrograma.Visible = True
                        pnlClase.Visible = True
                        cbPrograma.SelectedIndex = -1
                        cbClase.Items.Clear()
                        cbClase.Text = ""
                        'Dim rowPrograma As ConfiguracionDataSet.ProgramasRow
                        'rowPrograma = dsConfig.Programas.FindByIdPrograma(CInt(cbPrograma.SelectedValue))
                        'LoadClasesModulos(rowPrograma.Ensamblado)
                    Case TiposModulo.ModuloWeb
                        pnlURL.Visible = True
                        pnlProgramaClase.Visible = False

                        cbPrograma.SelectedIndex = -1
                        cbClase.Items.Clear()
                        cbClase.Text = ""
                        txtURL.Text = ""

                    Case TiposModulo.ModuloMovil

                        pnlProgramaClase.Visible = True
                        pnlURL.Visible = False
                        pnlClase.Visible = False
                        cbPrograma.SelectedIndex = -1
                        cbClase.Items.Clear()
                        cbClase.Text = ""
                        txtURL.Text = ""

                End Select
            End If
        End If
    End Sub

    Private Sub LoadClasesModulosNucleo()
        Try
            LoadClasesModulos(FrameworkApp.NombreEnsamblado)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub LoadClasesModulos(ByVal sFileName As String)
        Dim objAssembly As Assembly
        Dim objType, objTypes() As System.Type
        sFileName = Path.Combine(Application.StartupPath, sFileName)
        cbClase.Items.Clear()
        objAssembly = Assembly.LoadFile(sFileName)
        objTypes = objAssembly.GetTypes()
        For Each objType In objTypes
            If objType.IsSubclassOf(GetType(BaseViewControl)) Then
                cbClase.Items.Add(objType.FullName)
            End If
        Next
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        If m_Nuevo Then
            bsModulos.RemoveCurrent()
        End If
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub cbPrograma_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbPrograma.SelectedValueChanged
        cbClase.Items.Clear()
        Try
            If m_bCargar Then
                If Not DBUtils.IsNullOrEmpty(cbPrograma.SelectedValue) Then
                    Dim IdPrograma As Integer = CInt(cbPrograma.SelectedValue)
                    Dim rowPrograma As DataRow
                    rowPrograma = dsConfig.Tables("Programas").Rows.Find(IdPrograma)
                    If rowPrograma IsNot Nothing Then
                        LoadClasesModulos(CStr(rowPrograma("Ensamblado")))
                    End If
                End If
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
		Try
			Try
				bsModulos.EndEdit()

				Datos.BeginTransaction()
				Datos.UpdateModulos()
				Datos.CommitTransaction()
			Catch ex As Exception
				Datos.RollbackTransaction()
				ShowError(ex.Message)
			End Try

			Datos.dsConfig.PermisosEspeciales.Rows.Clear()
			Datos.dsConfig.Modulos.Rows.Clear()
			Datos.LoadModulos()
			Datos.LoadPermisosEspeciales()

			bsModulos.Position = bsModulos.Find("Nombre", NombreTextBox.Text)

			m_rowModulo = CType(DBUtils.GetCurrentRow(bsModulos), DataRow)

			If m_Nuevo Then
				ActualizarPermisosEspeciales()
			End If
			DialogResult = DialogResult.Cancel

		Catch ex As Exception
			ShowError(ex.Message)
		End Try
    End Sub

    Private Sub btnActualizarPermisosEspeciales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizarPermisosEspeciales.Click
        Try
            bsModulos.EndEdit()
            ActualizarPermisosEspeciales()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Sub ActualizarPermisosEspeciales()
        Dim ctlView As BaseViewControl
        ctlView = CreateViewInstance()
        If ctlView IsNot Nothing Then
            Dim Permisos As List(Of PermisoEspecial)
            Permisos = ctlView.PermisosEspeciales

            ' Se insertan o actualizan los permisos del módulo
            Dim sPermisos As String = ""
            Dim rowPermiso As DataRow
            Dim I As Integer
            For I = 0 To Permisos.Count - 1
                rowPermiso = dsConfig.Tables("PermisosEspeciales").Rows.Find(New Object() {m_rowModulo("IdModulo"), Permisos(I).IdPermiso})
                If rowPermiso Is Nothing Then
                    '[IdModulo] [int] NOT NULL,
                    '[IdPermiso] [int] NOT NULL,
                    '[Descripcion] [nvarchar](60) NOT NULL,
                    dsConfig.Tables("PermisosEspeciales").Rows.Add( _
                        m_rowModulo("IdModulo"), Permisos(I).IdPermiso, Mid(Permisos(I).Descripcion, 1, 60))
                Else
                    rowPermiso("Descripcion") = Mid(Permisos(I).Descripcion, 1, 60)
                End If

                ' Se actualiza el filtro para buscar los permisos ya no usados
                If I = Permisos.Count - 1 Then
                    sPermisos &= Permisos(I).IdPermiso
                Else
                    sPermisos &= Permisos(I).IdPermiso & ", "
                End If
            Next

            ' Se borran las filas que ya no se usan
            If sPermisos <> "" Then
                Dim PermisosBorrar() As DataRow
                PermisosBorrar = dsConfig.Tables("PermisosEspeciales").Select(String.Format( _
                    "IdModulo = {0} AND IdPermiso NOT IN ( {1} )", _
                    m_rowModulo("IdModulo"), sPermisos))
                For I = 0 To PermisosBorrar.Length - 1
                    PermisosBorrar(I).Delete()
                Next
            End If
            ctlView.Dispose()
        End If
    End Sub

    Private Function CreateViewInstance() As BaseViewControl
        Dim sFileName As String
        Dim rowPrograma As DataRow
        If CInt(m_rowModulo("IdTipoModulo")) = TiposModulo.ModuloNucleo Then
            sFileName = Path.Combine(Application.StartupPath, FrameworkApp.NombreEnsamblado)
        ElseIf CInt(m_rowModulo("IdTipoModulo")) = TiposModulo.ModuloPrograma Then
            rowPrograma = dsConfig.Tables("Programas").Rows.Find(m_rowModulo("IdPrograma"))
            sFileName = Path.Combine(Application.StartupPath, CStr(rowPrograma("Ensamblado")))
        Else
            Return Nothing
        End If
        Dim ctlView As BaseViewControl
        Dim ViewType As Type
        Dim objAssembly As Assembly
        objAssembly = Assembly.LoadFile(sFileName)
        ViewType = objAssembly.GetType(CStr(m_rowModulo("ClassURL")))
        ctlView = CType(objAssembly.CreateInstance(ViewType.FullName), BaseViewControl)
        Return ctlView
    End Function

   
End Class