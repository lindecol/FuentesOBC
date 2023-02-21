Public Class MainConfiguracionForm

    Private Shared m_Current As MainConfiguracionForm
    Public Shared ReadOnly Property Current() As MainConfiguracionForm
        Get
            Return m_Current
        End Get
    End Property

    Private Sub ConfiguracionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Se habilita el gestor de configuración para que lea el archivo de configuración
        m_Current = Me
        Me.Text = FrameworkApp.ApplicationName + " Versión: " + FrameworkApp.ApplicationVersion
        Dim sCurrentDirectory As String = Environment.CurrentDirectory.ToLower()
        If sCurrentDirectory.Contains("centauro.configuracion\bin") Then
            sCurrentDirectory = sCurrentDirectory.Replace("centauro.configuracion\bin", "centauro.Principal\bin")
            Environment.CurrentDirectory = sCurrentDirectory
        End If
    End Sub

    Private Sub btnEmpresas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmpresas.Click, DefiniciónEmpresasToolStripMenuItem.Click
        EmpresasForm.Run()
    End Sub

    Private Sub btnProgramas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProgramas.Click, DefiniciónProgramasToolStripMenuItem.Click
        RegistroProgramasDialog.Run()
    End Sub

    Private Sub btnModulos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModulos.Click, ConfiguraciónMódulosYCategoriasToolStripMenuItem.Click
        ConfigurarModulosForm.Run()
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click, SalirToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub btnCrearUsuarioInicial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrearUsuarioInicial.Click
        CrearUsuarioInicial()
    End Sub

    Public Shared Function CrearUsuarioInicial() As Boolean
        'REVISAR SI EXISTE EL USUARIO
        Dim Datos As New GestorConfiguracion
        If Not Datos.ExisteUsuarioInicial() Then
            Datos.LoadEmpresas()
            Dim IdEmpresa As Integer = Datos.NumeroEmpresas
            If IdEmpresa = -1 Then
                'CREAR UNA EMPRESA INICIAL
                If Not Datos.CrearEmpresaInicial() Then
                    MsgBox("Error creando empresa Inicial", MsgBoxStyle.Critical, "Validación")
                    Return False
                End If
                IdEmpresa = Datos.NumeroEmpresas
            End If
            If IdEmpresa >= 0 Then
                If Datos.CrearUsuarioInicial(IdEmpresa) Then
                    MsgBox("Usuario Creado", MsgBoxStyle.Exclamation, "Validación")
                    Return True
                Else
                    MsgBox("Error creando usuario Inicial", MsgBoxStyle.Critical, "Validación")
                    Return False
                End If
            Else
                MsgBox("Error creando usuario Inicial", MsgBoxStyle.Critical, "Validación")
                Return False
            End If
        Else
            MsgBox("Ya existe el usuario Inicial", MsgBoxStyle.Information, "Validación")
            Return False
        End If
    End Function

	Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

	End Sub
End Class