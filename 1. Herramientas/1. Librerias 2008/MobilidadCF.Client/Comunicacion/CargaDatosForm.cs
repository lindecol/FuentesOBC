using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MovilidadCF.Windows.Forms;
using System.Net;
using OpenNETCF.Net;
using OpenNETCF.WindowsCE;
using MovilidadCF.Data;
using MovilidadCF.Compression;
using System.Data.SqlServerCe;

namespace MovilidadCF.Client.Comunicacion
{
    internal partial class CargaDatosForm : Form, MovilidadCF.Data.DataSetSerializer.IEstadoCarga
    {
        private MovilidadCF.Client.WSComunicaciones.DataAccess m_ws;       
        private OpenNETCF.Threading.Thread2 m_Thread;
        //private System.Threading.Thread m_Thread;
        private string m_sData = null;
        private bool m_bCancelado = false;
        private delegate void ActualizarMensajeHandler(string sData);
        private delegate void SetProgresoHandler(int Progreso);
        private bool m_bCerrarVentana = false;        
        private ParametrosWS m_rwParametros;
        private static ConfiguracionServidorWS m_ConfiguracionWS;
        private string m_sConexionLocal = null;

        public CargaDatosForm()
        {
            InitializeComponent();
        }

        public static void ConfiguracionServidorWS(string pIPServidor, string pPuertoServidor, string pVirtualDirectory, string pUsuarioWebService, string pClaveWebService, int pTimeOutWebServices)
        {
            m_ConfiguracionWS = new ConfiguracionServidorWS(pIPServidor, pPuertoServidor, pVirtualDirectory, pUsuarioWebService, pClaveWebService, pTimeOutWebServices);
        }        

        public static bool Run(string sPrograma, string sProceso, string sIdTerminal, string sIdSistema, bool bComprimir, bool bUpdateCurrentRows, bool bUsarGPRS, bool bCerrarVentana, string sNombreProceso, string sConexionGPRS, string sConexionLocal, params object[] rParametros)
        {

            bool result = false;

            // Si asi se especifica se intenta primero establecer la conexión GPRS
            UIHandler.StartWait();
            if (bUsarGPRS)
            {
                if (!(GPRS.Conectar(sConexionGPRS)))
                {
                    UIHandler.EndWait();
                    return false;
                }
            }

            // Controlar las Excepciones
            try
            {
                // Se crea la instancia de la forma y se inicia el proceso
                CargaDatosForm frm = new CargaDatosForm();
                frm.m_rwParametros = new ParametrosWS(sPrograma, sProceso, sIdTerminal, sIdSistema, bComprimir, bUpdateCurrentRows, bUsarGPRS, sNombreProceso, rParametros);
                frm.m_bCerrarVentana = bCerrarVentana;
                frm.m_sConexionLocal = sConexionLocal;
                //result = (UIHandler.ShowDialog(frm) == System.Windows.Forms.DialogResult.OK);
                result = (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK);
                frm.Dispose();
                // Finalmente se cierra la conexión
                if (bUsarGPRS)
                {
                    GPRS.Desconectar();
                }
                return result;
            }
            catch
            {
                return false;
            }
            finally
            {
                UIHandler.EndWait();
            }
        }



        #region Miembros de IEstadoCarga

        public bool Cancelado
        {
            get { return m_bCancelado; }
        }

        public int ProgresoTabla
        {
            set
            {
                SetProgresoHandler funcHandler = new SetProgresoHandler(this.SetProgresosTabla);
                this.Invoke(funcHandler, value);
            }
        }

        public int ProgresoTotal
        {
            set
            {
                SetProgresoHandler funcHandler = new SetProgresoHandler(this.SetProgresoTotal);
                this.Invoke(funcHandler, value);
            }
        }

        public void IniciarTabla(string sNombreTabla)
        {
            ActualizarMensajeHandler funcHandler = new ActualizarMensajeHandler(this.InternalIniciarTabla);
            this.Invoke(funcHandler, sNombreTabla);
        }

        private void InternalIniciarTabla(string sNombreTabla)
        {
            pbAvanceTabla.Value = 0;
            lblTabla.Text = "Procesando " + sNombreTabla;
        }

        private void SetProgresosTabla(int Progreso)
        {
            pbAvanceTabla.Value = Progreso;
        }

        private void SetProgresoTotal(int Progreso)
        {
            pbAvanceTotal.Value = Progreso;
        }

        #endregion

        private void CargaDatosForm_Load(object sender, EventArgs e)
        {
            // Se inicializan controles
            UiHandler1.Parent = this;
            pnlInfoTablas.Visible = false;
            this.lblProceso.Text = this.m_rwParametros.DescripcionProceso;

            // Se inicia el hilo de ejecución de la integración de datos y se habilita el timer
            // de reset de estado Idle del sistema
            timerResetIdleState.Enabled = true;
            m_Thread = new OpenNETCF.Threading.Thread2(IntegrarDatosThread);
            m_Thread.Start();
            this.CargarImagenes();
            UIHandler.EndWait();
        }

        private void ConfigurarWebService()
        {
            m_ws = new MovilidadCF.Client.WSComunicaciones.DataAccess();
            if (this.m_rwParametros.UsarGPRS)
            {
                m_ws.Url = "http://" + m_ConfiguracionWS.IPServidor + ":" + m_ConfiguracionWS.PuertoServidor + "/" + m_ConfiguracionWS.VirtualDirectory + "/DataAccess.asmx";
            }
            else
            {
                m_ws.Url = "http://" + m_ConfiguracionWS.IPServidor + ":" + m_ConfiguracionWS.PuertoServidor + "/" + m_ConfiguracionWS.VirtualDirectory + "/DataAccess.asmx";
            }
            m_ws.Timeout = m_ConfiguracionWS.TimeOutWebServices;
            m_ConfiguracionWS.TimeOutWebServices = m_ws.Timeout;
            if (m_ConfiguracionWS.UsuarioWebService != "")
            {
                m_ws.Credentials = new NetworkCredential(m_ConfiguracionWS.UsuarioWebService, m_ConfiguracionWS.ClaveWebService);
            }
        }

        private void IntegrarDatosThread()
        {
            string sFecha = null;
            try
            {
                IniciarObtenerDatos("Obteniendo datos del servidor...");
                ConfigurarWebService();

                // Se sincroniza la hora del sistema con la hora del servidor
                m_ws.Timeout = 10000;
                sFecha = m_ws.GetFechaSistema();
                DateTimeHelper.LocalTime = DateTime.ParseExact(sFecha, "dd/MM/yyyy HH:mm:ss", null);
                m_ws.Timeout = m_ConfiguracionWS.TimeOutWebServices;
                m_ConfiguracionWS.TimeOutWebServices = m_ws.Timeout;
                string sIdComunicaciones = "";
                m_sData = m_ws.CargarDatos(m_rwParametros.Programa, m_rwParametros.Proceso, m_rwParametros.IdTerminal, m_rwParametros.IdSistema, m_rwParametros.Comprimir, ref sIdComunicaciones, m_rwParametros.Parametros);
                m_rwParametros.IdComunicaciones = sIdComunicaciones;
                IniciarIntegracionDatos("Integrando datos...");
                if (!this.Cancelado)
                {
                    //m_GestorIntegracion.LoadPrimaryKeysInfo();
                    DataSetSerializer dsDatos = new DataSetSerializer(new MovilidadCF.Data.SqlServerCe.GestorDatosBase(m_sConexionLocal));                    
                    if (m_rwParametros.Comprimir)
                    {
                        //m_sData = MovilidadCF.Compression.DataCompression.UnCompress(Convert.FromBase64String(m_sData), Convert.FromBase64String(m_sData).Length);
                        //dsDatos.SaveCompressToDatabase(this.m_sData, m_rwParametros.UpdateCurrentRows, this, null);
                        this.IntegrarDatos(this.m_sData, m_rwParametros.UpdateCurrentRows, null, true);
                    }
                    else
                    {
                        dsDatos.SaveToDatabase(this.m_sData, m_rwParametros.UpdateCurrentRows, this, null);                    
                    }                                        
                }
                m_ws.ConfirmarCargaDatos(m_rwParametros.Programa, m_rwParametros.Proceso, m_rwParametros.IdTerminal, m_rwParametros.IdSistema, m_rwParametros.IdComunicaciones, null);
                MostrarProcesoTerminado("Proceso de carga de datos terminado exitosamente");
            }
            catch (Exception ex)
            {
                if (m_rwParametros.IdComunicaciones != null)
                {
                    //CONTROLAR POR QUE EL CATCH PUEDE SER POR COMUNICACION Y SE BLOQUEA
                    try
                    {
                        m_ws.ConfirmarCargaDatos(m_rwParametros.Programa, m_rwParametros.Proceso, m_rwParametros.IdTerminal, m_rwParametros.IdSistema, m_rwParametros.IdComunicaciones, ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        WriteLog(ex2);
                    }
                }
                MostrarError(ex.GetType().ToString() + "\r\n" + ex.Message);
                WriteLog(ex);
            }
        }

        private void InternalMostrarProcesoTerminado(string sMensaje)
        {
            pnlInfoTablas.Visible = false;
            pnlTerminado.Visible = true;
            pnlTerminado.BringToFront();
            lblMensajeFinal.Text = sMensaje;
            btnTerminar.Focus();
            if (m_bCerrarVentana)
            {
                this.btnTerminar_Click(this, null);
            }
        }

        private void MostrarProcesoTerminado(string sMensaje)
        {
            ActualizarMensajeHandler funcHandler = new ActualizarMensajeHandler(InternalMostrarProcesoTerminado);
            this.Invoke(funcHandler, sMensaje);
        }

        private void IniciarObtenerDatos(string sMensaje)
        {
            ActualizarMensajeHandler funcHandler = new ActualizarMensajeHandler(InternalIniciarObtenerDatos);
            this.Invoke(funcHandler, sMensaje);
        }

        private void IniciarIntegracionDatos(string sMensaje)
        {
            ActualizarMensajeHandler funcHandler = new ActualizarMensajeHandler(InternalIniciarIntegracionDatos);
            this.Invoke(funcHandler, sMensaje);
        }

        private void InternalIniciarObtenerDatos(string sMensaje)
        {
            lblFase.Text = sMensaje;
            pnlInfoTablas.Visible = false;
            pnlObtenerDatos.Visible = true;
            AnimateCtl1.StartAnimation();
        }

        private void InternalIniciarIntegracionDatos(string sMensaje)
        {
            lblFase.Text = sMensaje;
            AnimateCtl1.StopAnimation();
            pnlObtenerDatos.Visible = false;
            pnlInfoTablas.Visible = true;
        }

        private void MostrarError(string sMensaje)
        {
            ActualizarMensajeHandler funcHandler = new ActualizarMensajeHandler(InternalMostrarError);
            this.Invoke(funcHandler, sMensaje);
        }

        private void InternalMostrarError(string sMensaje)
        {
            pnlError.Visible = true;
            pnlError.BringToFront();
            lblError.Text = sMensaje;
            btnCerrarByError.Focus();
        }

        private void timerResetIdleState_Tick(object sender, EventArgs e)
        {
            OpenNETCF.WindowsCE.PowerManagement.ResetSystemIdleTimer();
        }

        private void btnTerminar_Click(object sender, EventArgs e)
        {
            UIHandler.StartWait();
            timerResetIdleState.Enabled = false;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            m_bCancelado = true;
            lblFase.Text = "Cancelando...";
            pnlInfoTablas.Visible = false;
            pbAvanceTotal.Visible = false;
            btnCancelar.Enabled = false;
            Application.DoEvents();
            //if (m_Thread != null && m_Thread.State != OpenNETCF.Threading.ThreadState.Stopped)
            if (m_Thread != null)
            {
                if (!(m_Thread.Join(5000)))
                {
                    try
                    {
                        m_Thread.Abort();
                    }
                    catch
                    {
                    }
                }
            }
            UIHandler.StartWait();
            timerResetIdleState.Enabled = false;
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnCerrarByError_Click(object sender, EventArgs e)
        {
            UIHandler.StartWait();
            timerResetIdleState.Enabled = false;
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public static void WriteLog(Exception ex)
        {
            // Se guarda la información del error en el archivo
            System.IO.StreamWriter stream = new System.IO.StreamWriter("\\ErroresIntegracion.TXT", true);
            System.Text.StringBuilder bld = new System.Text.StringBuilder();
            Exception inner = ex.InnerException;

            stream.WriteLine("==============================================================================");
            stream.WriteLine(System.DateTime.Now.ToString() + "Ha ocurrido una excepción: " + ex.GetType().FullName);
            stream.WriteLine(ex.Message);
            if (inner != null)
            {
                stream.WriteLine("Inner Exception: " + inner.ToString());
            }
            if (ex.GetType() == typeof(System.Data.SqlServerCe.SqlCeException))
            {
                System.Data.SqlServerCe.SqlCeException sqlex = null;
                //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
                //			System.Data.SqlServerCe.SqlCeError err = null;
                sqlex = (System.Data.SqlServerCe.SqlCeException)ex;

                // Enumerate each error to a message box.
                foreach (System.Data.SqlServerCe.SqlCeError err in sqlex.Errors)
                {
                    stream.WriteLine(" Error Code: " + err.HResult.ToString("X"));
                    stream.WriteLine(" Message   : " + err.Message);
                    stream.WriteLine(" Minor Err.: " + err.NativeError);
                    stream.WriteLine(" Source    : " + err.Source);

                    // Retrieve the error parameter numbers for each error.
                    //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
                    //				int numPar = 0;
                    foreach (int numPar in err.NumericErrorParameters)
                    {
                        if (0 != numPar)
                        {
                            stream.WriteLine(" Num. Par. : " + numPar);
                        }
                    }

                    // Retrieve the error parameters for each error.
                    //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
                    //				string errPar = null;
                    foreach (string errPar in err.ErrorParameters)
                    {
                        if (string.Empty != errPar)
                        {
                            bld.Append(("\r" + " Err. Par. : " + errPar));
                        }
                    }
                }
            }
            stream.WriteLine("Stack Trace: " + ex.StackTrace.ToString());
            stream.Close();
        }

        private void CargarImagenes()
        {
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();
            this.AnimateCtl1.Image = new System.Drawing.Bitmap(_assembly.GetManifestResourceStream("MovilidadCF.Client.Imagenes.AnimateCtl1.Image.jpg"));              
        }

        SqlCeConnection m_Connection = null;
        int m_OpenCount = 0;
        private DataTable m_dtPK = null;
        private DataView m_dvPK = null;

        public SqlCeConnection Connection
        {
            get 
            {                
                if (m_Connection == null)
                {
                    m_Connection = new SqlCeConnection(m_sConexionLocal);
                }
                return m_Connection;
            }
        }    

        public void OpenConnection()
        {
            if (m_OpenCount == 0)
            {
                if (Connection != null)
                {
                    Connection.Open();
                }
                else
                {
                    throw new Exception("Error creando conexiones");
                }
            }
            m_OpenCount++;            
        }

        public void CloseConnection()
        {            
            m_OpenCount--;
            if (m_OpenCount == 0)
                Connection.Close();         
        }

        internal object GetColumnValue(string sFieldType, string sValue, string sColumna)
        {
            try
            {
                if (sValue == "(null)")
                {
                    return DBNull.Value;
                }
                else
                {
                    if (!sFieldType.StartsWith("System."))
                    {
                        sFieldType = "System." + sFieldType;
                    }
                    switch (sFieldType)
                    {
                        case "System.String":
                            StringBuilder sbTexto = new StringBuilder(Convert.ToString(sValue));
                            sbTexto.Replace("<CRLF>", "\r\n");
                            sbTexto.Replace("<CR>", "\r");
                            sbTexto.Replace("<LF>", "\n");
                            return sbTexto.ToString();
                        case "System.DateTime":
                            try
                            {
                                return DateTime.ParseExact(sValue, DataSetSerializer.FormatoFecha, null);
                            }
                            catch
                            {
                                return DateTime.ParseExact(sValue, DataSetSerializer.FormatoFecha.Replace("/", "-"), null);
                            }

                        case "System.Byte[]":
                            return Convert.FromBase64String(sValue);
                        case "System.Int32":
                            return Int32.Parse(sValue);
                        case "System.Double":
                            if (m_FormatoNumerico == null)
                            {
                                return Double.Parse(sValue);
                            }
                            else
                            {
                                return Double.Parse(sValue, m_FormatoNumerico);
                            }
                        case "System.Decimal":
                            if (m_FormatoNumerico == null)
                            {
                                return Decimal.Parse(sValue);
                            }
                            else
                            {
                                return Decimal.Parse(sValue, m_FormatoNumerico);
                            }
                        case "System.Int16":
                            return Int16.Parse(sValue);
                        case "System.Boolean":
                            if (sValue == DataSetSerializer.ValorTrue)
                            {
                                return true;
                            }
                            else
                            {
                                if (sValue == DataSetSerializer.ValorFalse)
                                {
                                    return false;
                                }
                                else
                                {
                                    if (sValue == "")
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        return bool.Parse(sValue);
                                    }

                                }
                            }
                        default:
                            return sValue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error convirtiendo datos, Tipo: " + sFieldType + "F" + this.FormatoFecha + " Valor:" + sValue + " Columna:" + sColumna);
            }
        }

        internal object GetColumnValue(string sFieldType, string sValue)
        {
            return this.GetColumnValue(sFieldType, sValue, string.Empty);
        }

        private string m_sValorTrue = "true";

        public string ValorTrue
        {
            get { return m_sValorTrue; }
            set { m_sValorTrue = value; }
        }

        private string m_sValorFalse = "false";

        public string ValorFalse
        {
            get { return m_sValorFalse; }
            set { m_sValorFalse = value; }
        }

        private string m_sFormatoFecha = "yyyy/MM/dd HH:mm:ss";

        public string FormatoFecha
        {
            get { return m_sFormatoFecha; }
            set { m_sFormatoFecha = value; }
        }

        private string m_sSeparadorDecimales = ".";
        private System.Globalization.NumberFormatInfo m_FormatoNumerico = null;

        public string SeparadorDecimales
        {
            get { return m_sSeparadorDecimales; }
            set
            {
                m_sSeparadorDecimales = value;
                //Instanciar el formato númerico
                if (m_sSeparadorDecimales == ".")
                {
                    System.Globalization.CultureInfo culturaActual = System.Globalization.CultureInfo.CurrentCulture;
                    m_FormatoNumerico = (System.Globalization.NumberFormatInfo)culturaActual.NumberFormat.Clone();
                    m_FormatoNumerico.NumberDecimalSeparator = ".";
                    m_FormatoNumerico.NumberGroupSeparator = ",";

                }
                else
                {
                    System.Globalization.CultureInfo culturaActual = System.Globalization.CultureInfo.CurrentCulture;
                    m_FormatoNumerico = (System.Globalization.NumberFormatInfo)culturaActual.NumberFormat.Clone();
                    m_FormatoNumerico.NumberDecimalSeparator = ",";
                    m_FormatoNumerico.NumberGroupSeparator = ".";
                }
            }
        }


        private void IntegrarDatos(string sSerializedData, bool bUpdateCurrentRows, DataSet ds, bool LanzarExcepcion)                    
        {
            Byte[] bDatosEnviados = Convert.FromBase64String(sSerializedData);
            DataCompression.UnCompressText(bDatosEnviados, bDatosEnviados.Length);
            string sLine = null;
            string[] sFields = null;
            string[] sFieldsTypes = null;
            string[] sValues = null;
            SqlCeResultSet rs = null;
            SqlCeUpdatableRecord record = null;
            int I = 0;
            int J = 0;
            int nIndex = 0;
            int nTableCount = 0;
            int nRowCount = 0;
            int nTotalRowCount = 0;
            int nRows = 0;
            int nTotalRows = 0;            
            DataTable dtNucleo = null;
            DataRow row = null;
            object FieldValue = null;
            bool bUpdateOriginal = bUpdateCurrentRows;
            try
            {
                // Se lee la liena con el número de tablas serializadas y el numero total de filas a procesar
                sLine = DataCompression.ReadLine();
                nTableCount = System.Convert.ToInt32(sLine.Substring(12));
                sLine = DataCompression.ReadLine();
                nTotalRowCount = System.Convert.ToInt32(sLine.Substring(15));
                nTotalRows = 0;

                this.OpenConnection();
                if (bUpdateCurrentRows)
                    //LoadPrimaryKeysInfo();

                while (!this.Cancelado)
                {
                    // Se obtiene el nombre y cantidad de registros de cada tabla serializada
                    string sTableName = null;
                    sLine = DataCompression.ReadLine();
                    if (sLine == null || sLine.Trim() == string.Empty)
                    {
                        break;
                    }
                    sTableName = sLine.Substring(7).Trim();
                     sLine = DataCompression.ReadLine();
                    nRowCount = System.Convert.ToInt32(sLine.Substring(10));
                    if (nRowCount > 0)
                    {
                        nRows = 0;
                        this.IniciarTabla(sTableName);
                        bUpdateCurrentRows = bUpdateOriginal;

                        // Se revisa si es una tabla del nucleo y se actualiza
                        if (ds != null)
                        {
                            if (ds.Tables.IndexOf(sTableName) >= 0)
                            {
                                dtNucleo = ds.Tables[sTableName];
                            }
                            else
                            {
                                dtNucleo = null;
                            }
                        }
                        else
                        {
                            dtNucleo = null;
                        }

                        if (bUpdateCurrentRows)
                        {
                            // Se filtra la información del indice de llave primario, para la busqueda de
                            // de las filas actuales
                            m_dvPK.RowFilter = "TABLE_NAME = '" + sTableName + "'";
                        }
                        else
                        {
                            // Si es una tabla del nucleo si eliminan las filas actuales
                            if (dtNucleo != null)
                            {
                                dtNucleo.Rows.Clear();
                            }
                        }


                        // Se obtiene el objeto ResultSet por medio del cual se hará la actualización
                        // especificando el indice de llave primaria de la tabla
                        SqlCeCommand cmd = new SqlCeCommand();
                        cmd.Connection = this.Connection;
                        cmd.CommandType = CommandType.TableDirect;
                        cmd.CommandText = sTableName;                        
                        if (bUpdateCurrentRows)
                        {
                            //Si no tiene llave primaria insertar el registro
                            if (m_dvPK.Count > 0)
                            {
                                cmd.IndexName = System.Convert.ToString(m_dvPK[0]["CONSTRAINT_NAME"]);
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Sensitive | ResultSetOptions.Scrollable);
                            }
                            else
                            {
                                bUpdateCurrentRows = false;
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                            }                            
                        }
                        else
                        {
                            rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                        }

                        // se obtienen los nombres de los campos
                        sLine = DataCompression.ReadLine();
                        sFields = sLine.Split('|');

                        // se obtienen los tipos de datos de las columnas
                        sLine = DataCompression.ReadLine();
                        sFieldsTypes = sLine.Split('|');

                        // Se procesa cada fila que venga serializada en la cadena
                        sLine = DataCompression.ReadLine();

                        bool bInsertRecord = false;
                        while ((sLine != null) & (!this.Cancelado))
                        {
                            if (sLine.Trim() == string.Empty)
                            {
                                break;
                            }

                            // Se obtienen los valores que vienen en el registro
                            sValues = sLine.Split('|');

                            // Se obtienen los valores de llave primaria del registro
                            // Se crea la matriz de objetos para guardar los valores de la llave primaria de cada registro
                            bInsertRecord = true;
                            if (bUpdateCurrentRows)
                            {

                                // Se obtiene la llave primaria del registro
                                object[] RecordKey = new object[m_dvPK.Count];
                                for (I = 0; I < m_dvPK.Count; I++)
                                {
                                    for (J = 0; J < sFields.GetUpperBound(0); J++)
                                    {
                                        if (System.Convert.ToString(m_dvPK[I]["COLUMN_NAME"]).ToUpper() == sFields[J].ToUpper())
                                        {
                                            RecordKey[I] = GetColumnValue(sFieldsTypes[J], sValues[J]);
                                        }
                                    }
                                }

                                // se busca el registro actual y luego se actualizan los datos
                                // si no se encuentra se inserta un nuevo registro
                                if (rs.Seek(DbSeekOptions.FirstEqual, RecordKey))
                                {
                                    bInsertRecord = false;

                                    // Se obtiene la fila a modificar
                                    rs.Read();
                                    if (dtNucleo != null)
                                    {
                                        row = dtNucleo.Rows.Find(RecordKey);
                                    }

                                    // Se actualizan los valores de cada columna en el registro en la base de datos y si
                                    // se esta procesando una tabla del nucleo tambien se actualiza en memoria
                                    if (dtNucleo != null && row != null)
                                    {
                                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                                nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                                if (nIndex >= 0)
                                                {
                                                    row[nIndex] = FieldValue;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    rs.Update();
                                }
                            }

                            if (bInsertRecord)
                            {
                                // Se crea el nuevo registro
                                record = rs.CreateRecord();
                                if (dtNucleo != null)
                                {
                                    row = dtNucleo.NewRow();
                                }
                                else
                                {
                                    row = null;
                                }

                                // Se actualizan los valores de cada columna en el registro
                                if (dtNucleo != null && row != null)
                                {
                                    for (I = 0; I < sFields.GetUpperBound(0); I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                            nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                            if (nIndex >= 0)
                                            {
                                                row[nIndex] = FieldValue;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    for (I = 0; I < sFields.GetUpperBound(0); I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }

                                // Se almacena el nuevo registro
                                try
                                {
                                    rs.Insert(record, DbInsertOptions.KeepCurrentPosition);
                                    if (dtNucleo != null && row != null)
                                    {
                                        dtNucleo.Rows.Add(row);
                                        row.AcceptChanges();
                                    }
                                }
                                catch (Exception ex)
                                {                                    
                                    if (LanzarExcepcion)
                                    {
                                        //object[] values = new object[rs.FieldCount + 1];
                                        //record.GetValues(values);
                                        throw ex;
                                    }
                                }
                            }

                            // Se registra el avance de la tabla
                            nRows += 1;
                            if (nRows == 448)
                            {
                                nRows = 448;
                            }
                            nTotalRows += 1;
                            if ((nRows % 100) == 0 || nRows == nRowCount)
                            {
                                this.ProgresoTabla = System.Convert.ToInt32((nRows * 100 / (double)nRowCount));
                                this.ProgresoTotal = System.Convert.ToInt32(nTotalRows * 100 / (double)nTotalRowCount);
                            }

                            // Se se lee el siguiente registro
                            sLine = DataCompression.ReadLine();
                        }                        
                    }
                }
            }
            finally
            {
                this.CloseConnection();
                DataCompression.Close();
            }            
        }       

    }
}