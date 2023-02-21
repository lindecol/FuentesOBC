Imports System.Reflection
Imports System.IO
Imports System.Data
Imports System.Threading
Imports MovilidadCF.Windows.Forms

Public Class MainForm
    Implements INucleo

#Region " Propiedades con informaci�n de la terminal o la ruta"
    Public ReadOnly Property CodigoEmpresa() As String Implements INucleo.CodigoEmpresa
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).CodigoEmpresa
        End Get
    End Property

    Public ReadOnly Property CodigoPuntoVenta() As String Implements INucleo.CodigoPuntoVenta
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).CodigoPuntoVenta
        End Get
    End Property

    Public ReadOnly Property NitPraxair() As String Implements INucleo.NitPraxair
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).NitPraxair
        End Get
    End Property

    Public ReadOnly Property CodigoSucursal() As String Implements INucleo.CodigoSucursal
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).CodigoSucursal
        End Get
    End Property

    Public ReadOnly Property CodigoTerminal() As String Implements INucleo.CodigoTerminal
        Get
            Return Settings.IDTerminal
        End Get
    End Property

    Public ReadOnly Property NitTrasportadora() As String Implements INucleo.NitTrasportadora
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).NitTrasportadora
        End Get
    End Property

    Public ReadOnly Property CodigoVehiculo() As String Implements INucleo.CodigoVehiculo
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).CodigoVehiculo
        End Get
    End Property

    Public ReadOnly Property RowParametros() As DataRow Implements INucleo.RowParametros
        Get
            Return GestorNucleo.dsNucleo.Parametros(0)
        End Get
    End Property

    Public Property KilometrajeInicial() As String Implements INucleo.KilometrajeInicial
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).KilometrajeInicial
        End Get
        Set(ByVal value As String)
            GestorNucleo.dsNucleo.Parametros(0).KilometrajeInicial = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property

    Public Property KiloMetrajeFinal() As String Implements INucleo.KiloMetrajeFinal
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).KilometrajeFinal
        End Get
        Set(ByVal value As String)
            GestorNucleo.dsNucleo.Parametros(0).KilometrajeFinal = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property

    Public Property FechaCarga() As DateTime Implements INucleo.FechaCarga
        Get

            If GestorNucleo.dsNucleo.Parametros(0).FechaDocumentos > #1/1/2000# Then
                Return GestorNucleo.dsNucleo.Parametros(0).FechaDocumentos
            Else
                Return Today()
            End If
        End Get
        Set(ByVal value As DateTime)
            GestorNucleo.dsNucleo.Parametros(0).FechaDocumentos = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property
    Public Property PedidoActual() As Integer Implements INucleo.PedidoActual
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).PedidoActual
        End Get
        Set(ByVal value As Integer)
            GestorNucleo.dsNucleo.Parametros(0).PedidoActual = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property

    Public Property NumeroMovimiento() As String Implements Common.INucleo.NumeroMovimiento
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).NoMovimiento
        End Get
        Set(ByVal value As String)
            GestorNucleo.dsNucleo.Parametros(0).NoMovimiento = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property

    Public ReadOnly Property RutaPrincipal() As String Implements INucleo.RutaPrincipal
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).RutaPrincipal
        End Get
    End Property

    Public ReadOnly Property CodigoChofer() As String Implements INucleo.CodigoChofer
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).CodigoChofer
        End Get
    End Property

    Public ReadOnly Property CodigoTrasportadora() As String Implements INucleo.CodigoTrasportadora
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).CodigoTrasportadora
        End Get
    End Property

    Public Property ConsecutivoDepositos() As String Implements INucleo.ConsecutivoDepositos
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).ConsecutivoDepositos
        End Get
        Set(ByVal value As String)
            GestorNucleo.dsNucleo.Parametros(0).ConsecutivoDepositos = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property

    Public Property ConsecutivoAsignaciones() As String Implements INucleo.ConsecutivoAsignaciones
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).ConsecutivoAsignaciones
        End Get
        Set(ByVal value As String)
            GestorNucleo.dsNucleo.Parametros(0).ConsecutivoAsignaciones = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property

    Public Property ConsecutivoAlquileres() As String Implements INucleo.ConsecutivoAlquileres
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).ConsecutivoAlquileres
        End Get
        Set(ByVal value As String)
            GestorNucleo.dsNucleo.Parametros(0).ConsecutivoAlquileres = value
            GestorNucleo.UpdateParametros()
        End Set
    End Property

    Public ReadOnly Property ProductoCuota() As String Implements INucleo.ProductoCuota
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).ProductoCuota
        End Get
    End Property

    Public ReadOnly Property ProductoCopago() As String Implements INucleo.ProductoCopago
        Get
            Return GestorNucleo.dsNucleo.Parametros(0).ProductoCopago
        End Get
    End Property



#End Region

#Region " Funciones Inicio de la aplicaci�n "
    Public Shared Sub Main()
        Try
            Dim hws As IntPtr
            Dim ceroIntPtr As New IntPtr(0)
            hws = FindWindow(Nothing, "Praxair - Ver. 2.5.11")
            If Not hws.Equals(ceroIntPtr) Then
                UIHandler.ShowError("La aplicacci�n ya se encuentra abierta")
                Application.Exit()
                Exit Sub
            End If

            ' Se inicializan otros objetos requeridos

            MovilidadCF.Symbol.BarcodeReader.InitReader()

            ' Se crea el objeto globlal del Nucelo y se inicia la aplicaci�n
            Dim frmMain As New MainForm
            UIHandler.StartWait()
            Nucleo = frmMain
            Application2.Run(frmMain)
        Catch ex As Exception
            ' Se realiza el registro de errores
            UIHandler.ShowError("Ha ocurrido un error no controlado!. Por favor revise el Log de la aplicaci�n para m�s informaci�n")
            WriteLog(ex)
        Finally
            ' Se liberan recursos que ya no sean necesarios
            MovilidadCF.Symbol.BarcodeReader.EndReader()
        End Try
    End Sub



    Public Shared Sub WriteLog(ByVal ex As Exception)
        ' Se guarda la informaci�n del error en el archivo
        Dim stream As New System.IO.StreamWriter(LogFile, True)
        Dim bld As New System.Text.StringBuilder()
        Dim inner As Exception = ex.InnerException

        stream.WriteLine("==============================================================================")
        stream.WriteLine(Now.ToString() & "Ha ocurrido una excepci�n: " & ex.GetType().FullName)
        stream.WriteLine(ex.Message)
        If Not inner Is Nothing Then
            stream.WriteLine("Inner Exception: " & inner.ToString())
        End If
        If ex.GetType() Is GetType(System.Data.SqlServerCe.SqlCeException) Then
            Dim sqlex As System.Data.SqlServerCe.SqlCeException
            Dim err As System.Data.SqlServerCe.SqlCeError
            sqlex = DirectCast(ex, System.Data.SqlServerCe.SqlCeException)

            ' Enumerate each error to a message box.
            For Each err In sqlex.Errors
                stream.WriteLine(" Error Code: " & err.HResult.ToString("X"))
                stream.WriteLine(" Message   : " & err.Message)
                stream.WriteLine(" Minor Err.: " & err.NativeError)
                stream.WriteLine(" Source    : " & err.Source)

                ' Retrieve the error parameter numbers for each error.
                Dim numPar As Integer
                For Each numPar In err.NumericErrorParameters
                    If 0 <> numPar Then
                        stream.WriteLine(" Num. Par. : " & numPar)
                    End If
                Next numPar

                ' Retrieve the error parameters for each error.
                Dim errPar As String
                For Each errPar In err.ErrorParameters
                    If String.Empty <> errPar Then
                        bld.Append((ControlChars.Cr & " Err. Par. : " & errPar))
                    End If
                Next errPar
            Next err
        End If
        stream.WriteLine("Stack Trace: " & ex.StackTrace.ToString())
        stream.Close()
    End Sub

    Private Shared ReadOnly Property LogFile() As String
        Get
            Return "\Log" & Now.ToString("yyyyMMdd") & ".TXT"
        End Get
    End Property

    Public Function GetInfoModulo(ByVal Modulo As ModulosNucleo) As InfoModulo Implements INucleo.GetInfoModulo
        Dim modInfo As New InfoModulo
        Select Case Modulo
            Case ModulosNucleo.Rutero
                modInfo.Nombre = "Rutero"
                modInfo.IconIndex = IconosModulos.Clientes
                modInfo.ClassType = GetType(RuteroForm)
                'Case ModulosNucleo.CargaDatos
                '    modInfo.Nombre = "Carga Datos"
                '    modInfo.IconIndex = IconosModulos.CargaDatos
                '    modInfo.ClassType = GetType(CargaDatosForm)
                'Case ModulosNucleo.DescargaDatos
                '    modInfo.Nombre = "Descarga Datos"
                '    modInfo.IconIndex = IconosModulos.DescargaDatos
                '    modInfo.ClassType = GetType(DescargaDatosForm)
        End Select
        Return modInfo
    End Function


#End Region

#Region " Funciones reutilizables por los programas "
    ' Obtiene el documento actual disponible para el tipo de documento solicitado
    Public Function GetTalonarioActual(ByVal CodTipoDocumento As Integer) As DataRow _
        Implements INucleo.GetTalonarioActual
        Return GestorNucleo.GetDocumentoActual(CodTipoDocumento)
    End Function

    ' Incrementa el documento actual del tipo de documento especificado
    Public Sub UpdateTalonario(ByVal rowTalonario As DataRow) _
        Implements INucleo.UpdateTalonario
        GestorNucleo.UpdateDocumentoActual(CType(rowTalonario, NucleoDataSet.TalonariosRow))
    End Sub

    Public Function CargarDatos(ByVal IdProceso As Integer, ByVal sNombreProceso As String, _
    ByVal bUpdateCurrentRows As Boolean, ByVal bUsarGPRS As Boolean) As Boolean Implements Common.INucleo.CargarDatos
        Dim Comunicacion As New Comunicacion
        If IdProceso > 10 Then
            ' Return CargaDatosForm.Run(IdProceso, sNombreProceso, bUpdateCurrentRows, bUsarGPRS)
            Return Comunicacion.ejecutarCarga(GestorNucleo.CadenaConexion, sNombreProceso, ProcesosComunicacion.CargaCompleta)
        Else
            UIHandler.ShowError("Los IDs de proceso del 1 al 10 estan reservados para el Nucleo")
        End If
        Return False
    End Function

    Public Function cargaNovedades() As Boolean Implements Common.INucleo.CargaNovedades
        Dim Comunicacion As New Comunicacion
        Return Comunicacion.ejecutarCarga(GestorNucleo.CadenaConexion, "Carga de Novedades", ProcesosComunicacion.CargaNovedades)
    End Function

    Public Function DescargarDatos(ByVal IdProceso As Integer, ByVal sNombreProceso As String, ByVal Tipo As String) As Boolean Implements Common.INucleo.DescargarDatos
        Dim comunicacion As New Comunicacion
        If IdProceso > 10 Then
            'Return DescargaDatosForm.Run(IdProceso, sNombreProceso, QuerysDescarga, bUsarGPRS)
            Return comunicacion.ejecutarDescarga(GestorNucleo.CadenaConexion, Tipo, ProcesosComunicacion.DescargaParcial, sNombreProceso)
        ElseIf IdProceso = 6 Or IdProceso = 2 Then
            If IdProceso = 6 Then
                Return comunicacion.ejecutarDescarga(GestorNucleo.CadenaConexion, Tipo, ProcesosComunicacion.DescargaParcial, sNombreProceso)
            Else
                Return comunicacion.ejecutarDescarga(GestorNucleo.CadenaConexion, Tipo, ProcesosComunicacion.DescargaCompleta, sNombreProceso)
            End If
            'Return DescargaDatosForm.Run(IdProceso, sNombreProceso, QuerysDescarga, bUsarGPRS)
        Else
            UIHandler.ShowError("Los IDs de proceso del 1 al 10 estan reservados para el Nucleo")
        End If
        Return False
    End Function

    Public Function Imprimir(ByVal Document As PrinterDocument, ByVal DatosImprimir As DatosCapturaFirma) As Boolean Implements Common.INucleo.Imprimir
        Dim sMsg As String
        ' Se pide al usuario que aliste la impresora
        If Settings.PrinterPortType = PrinterPortTypes.Bluetooth Then
            sMsg = "La impresora debe estar encendida, con papel disponible y una distancia no mayor 10 metros"
        Else
            sMsg = "La impresora debe estar encendida, con papel disponible y conectada con el CABLE esta terminal"
        End If
        UIHandler.ShowAlert(sMsg & vbCrLf & vbCrLf & "Pulse ENTER cuando este LISTO!", "Imprimir " & Document.Nombre)

        ' Se envia la impresi�n
        Dim pm As New ImprimirComprobantes(Settings.PrinterModel)
        If pm.Print(Document, DatosImprimir) Then
            ' Se confirma con el usuario si la impresion fue correcta
            Return UIHandler.Confirm("Si el documento no imprimio correctamente ser� anulado y tendra que imprimir nuevamente" & _
                vbCrLf & vbCrLf & "El documento fue impreso correctamente?", "Impresi�n Correcta?")
        Else
            Return False
        End If
    End Function


    Public Function Imprimir(ByVal Report As Common.PrinterReport) As Boolean Implements Common.INucleo.Imprimir
        Dim sMsg As String
        ' Se pide al usuario que aliste la impresora
        If Settings.PrinterPortType = PrinterPortTypes.Bluetooth Then
            sMsg = "La impresora debe estar encendida, con papel disponible y una distancia no mayor 10 metros"
        Else
            sMsg = "La impresora debe estar encendida, con papel disponible y conectada con el CABLE esta terminal"
        End If
        UIHandler.ShowAlert(sMsg & vbCrLf & vbCrLf & "Pulse ENTER cuando este LISTO!", "Imprimir Reporte")
        Dim pm As New ImprimirComprobantes(Settings.PrinterModel)
        Return pm.Print(Report)
    End Function

#End Region

    Private m_InitAppThread As Thread
    Private m_bInitApplication As Boolean = True

    Private Sub InitLoadDataApplication()
        Try
            ' Se leee la configuraci�n de la aplicaci�n
            If Settings Is Nothing Then
                Settings = New NucleoSettings
            End If

            If Programa Is Nothing Then
                ' Se inicializa el objeto controlador del programa configurado
                ' KUXAN: Siempre liquidos
                Select Case Settings.Programa
                    Case Programas.Gases
                        Programa = New Oxigenos.Liquidos.Programa
                    Case Programas.Liquidos
                        Programa = New Oxigenos.Liquidos.Programa
                End Select

                ' Se pide al programa que revise y cree la base de datos si es necesario
                Programa.CheckDatabase()
                Programa.CargarDatosIniciales()
            End If

            ' Se configura el manejador de teclado para el tipo de terminal configurado
            If Settings.DeviceModel = DeviceModels.MC9090 Then
                UIHandler.SIPEnabled = False
                UIHandler.TabKey.Change(&H6A, Chr(&H2A))
                UIHandler.DropDownComboBoxes = False
            ElseIf Settings.DeviceModel = DeviceModels.PPT8800 Then
                UIHandler.DropDownComboBoxes = True
                UIHandler.SIPEnabled = True
            End If

            ' Se crea el gestor de datos del nucleo, 
            GestorNucleo = New NucleoDataAccess
            CargarDatosBasicos()

            Me.Invoke(New EventHandler(AddressOf EndLoadDataApplication))


        Catch ex As Exception
            WriteLog(ex)
            Me.Invoke(New EventHandler(AddressOf ErrorLoadDataApplication))
        End Try
    End Sub

    Private Sub ErrorLoadDataApplication(ByVal sender As Object, ByVal e As EventArgs)
        UIHandler.ShowError("Ha ocurrido un error no controlado iniciando la aplicaci�n. Por favor revise el Log de errores")
        Me.Close()
    End Sub

    Private Sub EndLoadDataApplication(ByVal sender As Object, ByVal e As EventArgs)
        Me.Menu = MainMenu1
        pnlIniciando.Visible = False
        MostrarInformacionGeneral()
        CargarListaModulos()
        AnimateCtl1.StopAnimation()
        If lstModulos.Items.Count > 0 Then
            lstModulos.Items(0).Selected = True
        End If
        lstModulos.Focus()
    End Sub

    <System.Runtime.InteropServices.DllImport("coredll.dll", _
            EntryPoint:="FindWindow")> _
    Private Shared Function FindWindow( _
            ByVal lpClassName As String, _
            ByVal lpWindowName As String) As IntPtr
    End Function

    Private Sub MainForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

    End Sub

    Private Sub MainForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If Not e.Handled Then
            If Not ProcessHotKeys(Me, e) Then
                ' Se procesan las teclas personalizadas de la forma
            End If
        End If
    End Sub

    Private Sub frmprincipal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UiHandler1.Parent = Me
        UIHandler.EndWait()
        LoadApplicationData()
        Me.Text = "Ver." & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.Major _
                        & "." & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.Minor _
                        & "." & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.Build
    End Sub

    Private Sub LoadApplicationData(Optional ByVal bCargaInicial As Boolean = True)
        pnlIniciando.Visible = True
        Me.Menu = Nothing
        If Not bCargaInicial Then
            lblMensajeProceso.Text = "Cargando datos ..."
        End If
        AnimateCtl1.StartAnimation()
        m_InitAppThread = New Thread(AddressOf InitLoadDataApplication)
        m_InitAppThread.Start()
    End Sub

    Private Sub CargarListaModulos()
        ' Se configura la apariencia del control de lista
        lstModulos.View = View.LargeIcon
        lstModulos.Left = 0
        lstModulos.Width = Me.Width
        lstModulos.Height = (Me.Height - lstModulos.Top)

        ' Se agregan los modulos a la lista
        Dim lvItem As ListViewItem
        Dim modulo As InfoModulo
        lstModulos.Items.Clear()
        For Each modulo In Programa.ModulosPrograma
            lvItem = New ListViewItem(modulo.Nombre)
            lvItem.ImageIndex = modulo.IconIndex
            lvItem.Tag = modulo
            lstModulos.Items.Add(lvItem)
        Next
    End Sub

    Private Sub MostrarInformacionGeneral()
        lblIDTerminal.Text = Settings.IDTerminal
        lblPrograma.Text = Programa.Nombre
        If GestorNucleo.HayDatosCargados() Then
            lblRutaPrincipal.Text = GestorNucleo.dsNucleo.Parametros(0).RutaPrincipal
            lstModulos.Enabled = True
        Else
            UIHandler.ShowAlert("No se ha realizado la carga de datos. No podra acceder a la funcionalidad de la aplicaci�n")
            lstModulos.Enabled = False
        End If
    End Sub

    Private Sub CargarDatosBasicos()
        GestorNucleo.OpenConnection()
        GestorNucleo.LoadPrimaryKeysInfo()
        GestorNucleo.LoadParametros()
        GestorNucleo.LoadDocumentos()
        GestorNucleo.LoadTiposCliente()
        GestorNucleo.LoadTiposPago()
        GestorNucleo.LoadEntidades()
        If Not Programa.UseQueryFilterInRutero Then
            GestorNucleo.LoadClientes()
            GestorNucleo.LoadPedidos()
        End If
        GestorNucleo.CloseConnection()
        GestorNucleo.ActualizarVersionParametros()
    End Sub

    Private Sub LstPrincipal_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstModulos.ItemActivate
        ' Se obtiene la informaci�n del m�dulo y se ejecuta la informaci�n
        If Me.lstModulos.SelectedIndices.Count > 0 Then
            Dim lvItem As ListViewItem = Me.lstModulos.Items(Me.lstModulos.SelectedIndices(0))
            Dim modInfo As InfoModulo = CType(lvItem.Tag, InfoModulo)
            EjecutarModuloPrograma(modInfo)
        End If
    End Sub

    Private Sub menuSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSalir.Click
        Close()
    End Sub

    Private Sub menuConfiguracion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuConfiguracion.Click
        ConfiguracionForm.Run()
    End Sub

    Private Sub menuCargarDatos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCargarDatos.Click
        Try
            Dim bRealizarCarga As Boolean = True
            Dim Comunicaciones As New Comunicacion
            If GestorNucleo.dsNucleo.Parametros.Count > 0 Then
                If GestorNucleo.dsNucleo.Parametros(0).IsDescargaRealizadaNull() OrElse _
                    GestorNucleo.dsNucleo.Parametros(0).DescargaRealizada = False Then
                    If GestorNucleo.HayPedidosProcesados() Then
                        UIHandler.ShowError("No puede realizar la carga de datos sin haber realizado la descarga de la ruta actual!")
                        bRealizarCarga = False
                    Else
                        bRealizarCarga = UIHandler.Confirm("Al realizar la carga de datos, se borraran todos los datos actuales." & vbCrLf & "Esta seguro?", "Confirmar?")
                    End If
                End If
            End If
            If bRealizarCarga Then
                Programa.CreateDatabase()
                GestorNucleo = New NucleoDataAccess
                Comunicaciones.ejecutarCarga(GestorNucleo.CadenaConexion.ToString, "Carga Datos", ProcesosComunicacion.CargaCompleta)
                'CargaDatosForm.Run(ProcesosComunicacion.CargaCompleta, "Carga Datos", False, False)
                ActualizarNucleo()
                MostrarInformacionGeneral()
            End If
        Catch ex As Exception
            MovilidadCF.Logging.Logger.Write(ex)
            UIHandler.ShowAlert(ex.Message)
        End Try
    End Sub
    Private Sub ActualizarNucleo()
        GestorNucleo = New NucleoDataAccess
        GestorNucleo.LoadClientes()
        GestorNucleo.LoadParametros()
        GestorNucleo.ActualizarVersionParametros()
    End Sub
    Private Sub menuDescargarDatos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuDescargarDatos.Click
        Dim Comunicacion As New Comunicacion
        If GestorNucleo.dsNucleo.Parametros(0).IsDescargaRealizadaNull() OrElse _
            GestorNucleo.dsNucleo.Parametros(0).DescargaRealizada = False Then
            If UIHandler.Confirm("Al realizar la descarga, no podra modificar nuevamente la informaci�n." & vbCrLf & "Esta seguro?") Then
                'Dim lstQuery As New ListQuerysDescarga
                'Programa.LoadQuerysDescargaParcial(lstQuery)
                'If DescargaDatosForm.Run(ProcesosComunicacion.DescargaCompleta, "Descarga datos", lstQuery, False) Then
                If Comunicacion.ejecutarDescarga(GestorNucleo.CadenaConexion.ToString, "Parcial", ProcesosComunicacion.DescargaCompleta, "Descarga datos") Then
                    GestorNucleo.dsNucleo.Parametros(0).DescargaRealizada = True
                    GestorNucleo.UpdateParametros()
                End If
            End If
        Else
            UIHandler.ShowError("La descarga ya fue realizada completamente, no puede volver a descarga datos")
        End If
    End Sub

    Public Function ProcessHotKeys(ByVal frm As System.Windows.Forms.Form, ByVal e As System.Windows.Forms.KeyEventArgs) As Boolean Implements Common.INucleo.ProcessHotKeys
        Return Globales.ProcessHotKeys(frm, e)
    End Function

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lstQuery As New ListQuerysDescarga
        Dim Comunicacion As New Comunicacion
        Programa.LoadQuerysDescargaParcial(lstQuery)
        'DescargaDatosForm.Run(ProcesosComunicacion.DescargaParcial, "Descarga datos", lstQuery, True)
        Comunicacion.ejecutarDescarga(GestorNucleo.CadenaConexion, "Parcial", ProcesosComunicacion.DescargaParcial, "Descarga datos")
    End Sub

    Public Function ImprimirHojaRuta(ByVal Document As Common.PrinterDocument) As Boolean Implements Common.INucleo.ImprimirHojaRuta
        Dim sMsg As String
        ' Se pide al usuario que aliste la impresora
        If Settings.PrinterPortType = PrinterPortTypes.Bluetooth Then
            sMsg = "La impresora debe estar encendida, con papel disponible y una distancia no mayor 10 metros"
        Else
            sMsg = "La impresora debe estar encendida, con papel disponible y conectada con el CABLE esta terminal"
        End If
        UIHandler.ShowAlert(sMsg & vbCrLf & vbCrLf & "Pulse ENTER cuando este LISTO!", "Imprimir " & Document.Nombre)

        ' Se envia la impresi�n
        Dim pm As New ImprimirComprobantes(Settings.PrinterModel)
        If pm.PrintPortatilHojaRuta(Document) Then
            ' Se confirma con el usuario si la impresion fue correcta
            Return UIHandler.Confirm("El documento fue impreso correctamente?", "Impresi�n Correcta?")
        Else
            Return False
        End If
    End Function

End Class
