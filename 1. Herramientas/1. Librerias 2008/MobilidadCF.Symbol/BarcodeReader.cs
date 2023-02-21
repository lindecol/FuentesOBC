using System;
using System.ComponentModel;
using OpenNETCF;
using Symbol;
using Symbol.Barcode;
using System.Text;

namespace MovilidadCF.Symbol
{
    public delegate void ScanCompleteHandler(ScanCompleteArgs e);

    /// <summary>
    /// Maneja el lector de código de barras de terminales Symbol
    /// </summary>
    public class BarcodeReader : Component
    {
        private static Reader m_Reader;
        private static ReaderData m_ReaderData;
        private static EventHandler m_ReaderNotifyHandler;
        private static EventHandler m_ReaderStatusHandler;
        private static int m_Instances = 0;
        private static bool m_bContinousTrigger = false;
        //propiedad para controlar si se registran los eventos
        private static bool m_bRegistrarEventos = true;
        private int _codePage = 1252;

        private static string _deviceName = null;
        private static string _friendlyName = null;

        public int CodePage
        {
            get { return _codePage; }
            set { _codePage = value; }
        }


        private Encoding _encoding;

        private Encoding Encoding
        {
            get
            {
                if (_encoding != null && _encoding.CodePage != _codePage)
                {
                    _encoding = null;
                }
                if (_encoding == null)
                {
                    _encoding = Encoding.GetEncoding(_codePage);
                }
                return _encoding;
            }
        }

        public static bool RegistrarEventos
        {
            get { return BarcodeReader.m_bRegistrarEventos; }
            set { BarcodeReader.m_bRegistrarEventos = value; }
        }

        public static bool ContinousTrigger
        {
            get
            {
                return m_bContinousTrigger;
            }
            set
            {
                m_bContinousTrigger = value;
            }
        }


        private static System.Windows.Forms.Form m_OwnerForm;

        public BarcodeReader()
        {           
            InitializeComponent();
        }

        #region Código generado por el Diseñador de componentes

        // Required designer variable.
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        private EventHandler OwnerForm_ActivatedHandler;
        private EventHandler OwnerForm_DeactivateHandler;
        private EventHandler OwnerForm_ClosedHandler;

        private System.Windows.Forms.Form Parent
        {
            get
            {
                return m_OwnerForm;
            }
            set
            {
                if (m_OwnerForm != null)
                {
                    if (RegistrarEventos)
                    {
                        m_OwnerForm.Activated -= OwnerForm_ActivatedHandler;
                        m_OwnerForm.Deactivate -= OwnerForm_DeactivateHandler;
                        m_OwnerForm.Closed -= OwnerForm_ClosedHandler;
                    }
                }
                // Se asigna la forma padra y se enlanzan los eventos necesarios
                m_OwnerForm = value;
                if (RegistrarEventos)
                {
                    OwnerForm_ActivatedHandler = new EventHandler(OwnerForm_Activated);
                    OwnerForm_DeactivateHandler = new EventHandler(OwnerForm_Deactivate);
                    OwnerForm_ClosedHandler = new EventHandler(OwnerForm_Closed);
                    m_OwnerForm.Activated += OwnerForm_ActivatedHandler;
                    m_OwnerForm.Deactivate += OwnerForm_DeactivateHandler;
                    m_OwnerForm.Closed += OwnerForm_ClosedHandler;
                }
            }
        }

        public static void InitReader(string DeviceName, string FriendlyName)
        {
            _deviceName = DeviceName;
            _friendlyName = FriendlyName;
            InitReader();
        }


        public static void InitReader()
        {
            if (m_Instances == 0)
            {

                // Se crea la instancia del controlador y se asigna valores
                // por defecto
                try
                {
                    if (m_Reader == null)
                    {
                        if (_deviceName == null)
                        {
                            m_Reader = new Reader();
                        }
                        else
                        {
                            Device Dispositivo = new Device(_deviceName, _friendlyName);
                            m_Reader = new Reader(Dispositivo);
                        }
                    }
                    m_ReaderData = new ReaderData(ReaderDataTypes.Binary, ReaderDataLengths.MaximumLabel);
                    m_Reader.Actions.Enable();
                    m_Reader.Parameters.Feedback.Success.WaveFile = "";
                    m_Reader.Parameters.Feedback.Success.BeepFrequency = 2670;
                    m_Reader.Parameters.Feedback.Success.BeepTime = 300;
                    m_Reader.Parameters.Feedback.Success.LedTime = 2000;
                }
                catch
                {
                    if (m_ReaderData != null)
                    {
                        m_ReaderData.Dispose();
                        m_ReaderData = null;
                    }
                    if (m_Reader != null)
                    {
                        m_Reader.Actions.Flush();
                        m_Reader.Actions.Dispose();
                        m_Reader.Dispose();
                        m_Reader = null;
                    }
                }
            }
            m_Instances++;
        }

        public static void EndReader()
        {
            m_Instances--;
            if (m_Instances == 0)
            {
                // Se borran las instancias del controlador
                if (m_Reader != null)
                {
                    m_Reader.Actions.Flush();
                    m_Reader.Actions.Disable();
                    m_Reader.Dispose();
                    m_Reader = null;
                }
                if (m_ReaderData != null)
                {
                    m_ReaderData.Dispose();
                    m_ReaderData = null;
                }
            }
        }

		//ESTAS FUNCIONES SE DEBEN ANALIZAR PORQUE EL LLAMADO A LOS EVENTOS PARA ACTIVAR Y DESACTIVAR EL ESCANER 
		//SON HECHOS EXPLICITAMENTE DENTRO DEL CÓDIGO
		private void OwnerForm_Activated(object sender, EventArgs e)
		{
			StartRead();
		}

		private void OwnerForm_Deactivate(object sender, EventArgs e)
		{
			StopRead();
		}

		private void OwnerForm_Closed(object sender, EventArgs e)
		{
			StopRead();
		}

        bool m_bEnabled = false;
        private bool Enabled
        {
            get
            {
                return m_bEnabled;
            }
            set
            {
                if (value && !m_bEnabled)
                    StartRead();
                if (!value && m_bEnabled)
                    StopRead();
            }
        }

        public void StartRead()
        {
            try
            {
                if (m_Reader != null && m_ReaderData != null)
                {
                    if (!m_bEnabled)
                    {
                        //Logging.Logger.Write("Enlazar Escaner");
                        m_ReaderNotifyHandler = new EventHandler(Reader_ReadNotify);
                        m_ReaderStatusHandler = new EventHandler(Reader_StatusNotify);
                        m_Reader.ReadNotify += m_ReaderNotifyHandler;
                        m_Reader.StatusNotify += m_ReaderStatusHandler;
                        m_bEnabled = true;
                    }
                    if (!m_ReaderData.IsPending)
                        m_Reader.Actions.Read(m_ReaderData);
                }
            }
            catch
            {
                m_bEnabled = false;
            }
        }

        public void StartRead(System.Windows.Forms.Form frmOwner)
        {
			//Logging.Logger.Write("StartRead(" + frmOwner + ")");
            this.Parent = frmOwner;
            StartRead();
        }

        public void StopRead()
        {
            if (m_bEnabled)
            {
                if (m_Reader != null)
                {
                    //Controlar error al parar el lector
                    try
                    {
                        //Logging.Logger.Write("Desenlazar Escaner");
                        m_Reader.ReadNotify -= m_ReaderNotifyHandler;
                        m_Reader.StatusNotify -= m_ReaderStatusHandler;
                        if (m_ReaderData != null)
                            if (m_ReaderData.IsPending)
                                m_Reader.Actions.CancelRead(m_ReaderData);
                        m_Reader.Actions.Flush();
                    }
                    catch
                    {
                    }
                }
                m_bEnabled = false;
            }
        }

        public event ScanCompleteHandler ScanComplete;

        private void Reader_ReadNotify(object sender, EventArgs e)
        {
            ReaderData Data = m_Reader.GetNextReaderData();
            if (Data.Result == Results.SUCCESS)
            {
                // Se dispara el evento
                if (ScanComplete != null)
                {
                    ScanCompleteArgs args = new ScanCompleteArgs();
                    //args.Data = Data.RawData;
                    args.Text = Encoding.GetString(Data.RawData, 0, Data.Length);
                    string sBarcodeType = Enum2.GetName(typeof(DecoderTypes), Data.Type);
                    args.Type = (BarcodeType)Enum2.Parse(typeof(BarcodeType), sBarcodeType);
                    //args.DataLength = Data.Length;
                    args.Length = args.Text.Length;
                    ScanComplete(args);                    
                }
            }
            StartRead();
        }

        private void Reader_StatusNotify(object sender, EventArgs e)
        {
            BarcodeStatus status = Reader.GetNextStatus();
            //Logging.Logger.Write("Status Reader Notify:" + status.State.ToString());
            if (status.Type == EventTypes.STATE_CHANGE && status.State == States.WAITING)
            {
                if (m_bContinousTrigger && m_Reader != null)
                {
                    if (m_ReaderData.IsPending)
                        m_Reader.Actions.CancelRead(m_ReaderData);
                    m_Reader.Actions.Flush();
                    m_Reader.Actions.ToggleSoftTrigger();
                }
            }
        }

        public static Reader Reader
        {
            get
            {
                return m_Reader;
            }
        }

    }

}
