using System;
using System.Collections.Generic;
using System.Text;
using Symbol;
using Symbol.MagStripe;
using MovilidadCF.Logging;

namespace MovilidadCF.Symbol
{

    public delegate void ReadCompleteHandler(string data);

    public class MagneticStripReader
    {

        private static Reader m_Reader;
        private static ReaderData m_ReaderData;
        private static EventHandler m_ReaderNotifyHandler;
        private static int m_Instances = 0;


        public static void InitReader()
        {
            if (m_Instances == 0)
            {

                // Se crea la instancia del controlador y se asigna valores
                // por defecto
                try
                {
                    m_Reader = new Reader();
                    m_ReaderData = new ReaderData();
                    m_Reader.Actions.Enable();
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
            try
            {
                m_Instances--;
                if (m_Instances == 0)
                {
                    // Se borran las instancias del controlador
                    if (m_ReaderData != null)
                    {
                        m_ReaderData.Dispose();
                        m_ReaderData = null;
                    }
                    if (m_Reader != null)
                    {
                        m_Reader.Actions.Flush();
                        m_Reader.Actions.Disable();
                        m_Reader.Dispose();
                        m_Reader = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
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
                        m_ReaderNotifyHandler = new EventHandler(Reader_ReadNotify);
                        m_Reader.ReadNotify += m_ReaderNotifyHandler;
                        m_bEnabled = true;
                    }
                    m_Reader.Actions.Read(m_ReaderData);
                }
            }
            catch
            {
                m_bEnabled = false;
            }

        }

        public void StopRead()
        {
            if (m_Reader != null)
            {
                m_Reader.ReadNotify -= m_ReaderNotifyHandler;
                m_Reader.Actions.Flush();
            }
            m_bEnabled = false;
        }

        public event ReadCompleteHandler ReadComplete;

        private void Reader_ReadNotify(object sender, EventArgs e)
        {
            ReaderData Data = m_Reader.GetNextReaderData();
            if (Data.Result == Results.SUCCESS)
            {
                // Se dispara el evento
                if (ReadComplete != null)
                {
                    ReadComplete(Data.Text);
                }
            }
            StartRead();
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
