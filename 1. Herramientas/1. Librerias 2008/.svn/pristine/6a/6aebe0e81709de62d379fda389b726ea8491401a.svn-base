using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using MovilidadCF.Windows.Forms;

namespace MovilidadCF.Communications.WinTMS
{
    public partial class WinTMSServerForm : Form
    {
        // Variables privadas
        private string m_sRutaArchivos;
        private string m_sCommPort = "COM1";
        private string m_sFilePrefix = "";
        private SerialPort  m_Port;
        private CRCTool m_CRCHelper;
        private byte[] szCommData = new byte[BUF_SIZE + 10];
        private byte[] szFileData = new byte[BUF_SIZE + 10];
        private int nFileDataLen = 0;
        private int nComDataLen = 0;
        private bool bStopThread = false;
        private Thread m_Thread = null;

        // Constantes utilizadas
        private const int BUF_SIZE = 8192;
        private const int PACKET_SIZE = 256;
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte SOH = 0x01;
        private const byte EOT = 0x04;
        private const byte ENQ = 0x05;
        private const byte ACK = 0x06;
        private const byte NAK = 0x15;
        private const byte DLE = 0x10;
       

        public WinTMSServerForm()
        {
            InitializeComponent();
            m_CRCHelper = new CRCTool();
            m_CRCHelper.Init(CRCTool.CRCCode.CRC16);
        }

        private void CheckStopThreadCondition()
        {
            if (bStopThread)
            {
                Thread.CurrentThread.Abort();
            }
        }
       
        public static void Run(string sFilePrefix, string sComPort, string sRutaArchivos)
        {
            UIHandler.StartWait();
            WinTMSServerForm frm = new WinTMSServerForm();
            frm.m_sRutaArchivos = sRutaArchivos;
            frm.m_sFilePrefix = sFilePrefix;
            frm.m_sCommPort = sComPort;
            UIHandler.ShowDialog(frm);
            frm.Dispose();
            UIHandler.EndWait();
        }

        private void InitThread()
        {
            bStopThread = false;
            m_Thread = new Thread(new ThreadStart(ThreadProc));
            m_Thread.Start();
        }

        private void StopThread()
        {
            bStopThread = true;
            if (!m_Thread.Join(3000))
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

        public void ThreadProc()
        {
            string sHeader = "";
            OpenConnection();
            while (!bStopThread)
            {
                try
                {
                    if (WaitForData())
                    {
                        sHeader = GetResponse();
                        if (sHeader[0] == 'C')
                        {
                            ReceiveFile(sHeader);
                        }
                        else
                        {
                            AddLog("Error Comando no Soportado: " + sHeader);
                            SendData("ERROR");
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    CloseConnection();
                    break;
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    AddLog("ERROR: " + ex.Message);
                    break;
                }
            }
            CloseConnection();
        }

        private void OpenConnection()
        {
            m_Port = new  SerialPort();
            m_Port.PortName = m_sCommPort;
            m_Port.Parity = Parity.None;
            m_Port.Handshake = Handshake.None;
            m_Port.BaudRate = 38400;
            m_Port.DataBits = 8;
            m_Port.StopBits = StopBits.One;
            m_Port.ReadTimeout = 2000;
            m_Port.Open();
        }

        private void CloseConnection()
        {
            if (m_Port != null)
            {
                m_Port.Close();
                m_Port = null;
            }
        }

        private UInt16 CalcCRC16(byte[] buffer, int offset, int len)
        {
            uint crc = (uint)m_CRCHelper.crctablefast(buffer, offset, len);
            byte lByte = (byte)((crc & 0xFF00) >> 8);
            byte hByte = (byte)((crc & 0x00FF));
            if (lByte == 0)
                lByte = 0xFF;
            if (hByte == 0)
                hByte = 0xFF;
            return (UInt16)(lByte << 8 | hByte);
        }

        private bool WriteData(byte[] Data, int len)
        {
            m_Port.Write(Data, 0, len);
            return true;
        }

        private bool WaitForData()
        {
            for (int i = 0; i < 30; i++)
            {
                if (m_Port.BytesToRead > 0)
                    break;
               CheckStopThreadCondition();
               Thread.Sleep(100);
            }
            return m_Port.BytesToRead > 0;
        }

        private int ReadData(byte[] buffer, int nMaxLen)
        {
           return m_Port.Read(buffer, 0,  nMaxLen);                
        }

        private byte[] m_BufferByte = new byte[1];
        private void WriteByte(byte c)
        {
            m_BufferByte[0] = c;
            WriteData(m_BufferByte, 1);
        }

        private byte ReadByte()
        {
            return (byte)m_Port.ReadByte();
        }


        private bool GetData()
        {
            byte c = 0;
            int i = 0, j = 0;
            bool result = false;
            int nVeces = 0;
            int bETX = 0;
            bool bOK = false;
            bool bDLE = false;
            byte[] szAck = new byte[] { 0x10, 0x06 };
            byte[] szNak = new byte[] { 0x10, 0x15 };


            // Se reciben los datos y se guardan en el buffer, 
            // este proceso se repite hasta máximo tres veces
            //
            for (nVeces = 0; nVeces < 3; nVeces++)
            {
                result = false;
                if (nVeces >= 3)
                    return false;
                j = 0;
                i = 0;
                bOK = false;
                bETX = 0;
                bDLE = false;
                c = 0;

                for (; ; )
                {
                    // Se leen datos del puerto byte a byte  y se acumulan en el buffer
                    // de datos
                    //WaitForData();
                    c = ReadByte();

                    if (bETX > 0)
                    {
                        szCommData[j++] = c;
                        bETX++;
                        if (bETX == 3)
                        {
                            bOK = true;
                            break;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (bDLE)
                        {
                            bDLE = false;
                            if (c == STX)
                            {
                                j = 0;
                                continue;
                            }
                            else if (c == ETX)
                                bETX = 1;
                        }
                        else if (c == DLE)
                            bDLE = true;
                        szCommData[j++] = c;


                    }
                } // End For Lectura Puerto


                // Se calcula el CRC de los datos recibidos y se compara con el de 
                // los datos enviados y se envia la respuesta correspondiente.
                // Si la respuesta es ACK. se finaliza la rutina y se retorna 0,
                // pasando previamente los datos a szFileData.
                // si la respuesta es NAK se hace un nuevo intento.
                //
                if (bOK)
                {
                    nComDataLen = j;
                    /*rcvCRC.lByte = szCommData[j-2];
                    rcvCRC.hByte = szCommData[j-1];*/
                    UInt16 lByte = (UInt16)szCommData[j - 2];
                    UInt16 hByte = (UInt16)szCommData[j - 1];
                    UInt16 rcvCRC = (UInt16)(hByte << 8 | lByte);
                    UInt16 calcCRC = CalcCRC16(szCommData, 0, j - 2);
                    if (rcvCRC != calcCRC)
                        throw new ApplicationException("Bad CRC");
                }
                else
                {
                    throw new ApplicationException("Bad Data");
                }
                if (bOK)
                {
                    i = 0;
                    j = 0;
                    for (i = 0; i < nComDataLen - 4; i++)
                    {
                        if (szCommData[i] == '\n')
                            m_nFileLineCount++;
                        szFileData[j++] = szCommData[i];
                    }
                    nFileDataLen = j;
                    if (WriteData(szAck, 2))
                        result = true;
                    else
                        return false;
                    break;
                }
                else
                {
                    if (WriteData(szNak, 2))
                        break;
                }
            }
            return result;
        }

        // Esta función envia los datos con el formato apropiado y espera la respuesta.
        // Se realizan hasta 3 intentos de envio de los datos si la respuesta es un NAK
        // 
        private bool SendData()
        {
            byte c = 0;
            byte[] szResponse = new byte[2];
            int i, j;
            bool result = false;
            szCommData[0] = DLE;
            szCommData[1] = STX;
            j = 2;
            for (i = 0; i < nFileDataLen; i++)
            {
                szCommData[j++] = szFileData[i];
            }
            szCommData[j++] = DLE;
            szCommData[j++] = ETX;
            UInt16 CRC = CalcCRC16(szCommData, 2, j - 2);
            byte lByte = (byte)((CRC & 0xFF00) >> 8);
            byte hByte = (byte)((CRC & 0x00FF));
            szCommData[j++] = hByte;
            szCommData[j++] = lByte;
            nComDataLen = j;

            // Se envia el buffer
            //
            for (i = 0; i < 3; i++)
            {
                if (!WriteData(szCommData, nComDataLen))
                    break;

                c = 0;
                WaitForData();
                c = ReadByte();
                if (c == DLE)
                {
                    c = ReadByte();
                    {
                        szResponse[0] = DLE;
                        szResponse[1] = c;
                    }
                }
                if (szResponse[0] == DLE && szResponse[1] == ACK)
                {
                    result = true;
                    break;
                }
                else if (szResponse[0] != DLE || szResponse[1] != NAK)
                    throw new ApplicationException("Respuesta no valida: Se espera ACK o NAK");
            } // En for envio datos

            return result;
        }

        private byte[] GetBytes(string sData)
        {
            return Encoding.GetEncoding(1252).GetBytes(sData);
        }

        private string GetString(byte[] Data, int offset, int len)
        {
            return Encoding.GetEncoding(1252).GetString(Data, offset, len);
        }

        private bool SendData(string sData)
        {
            byte[] Data = GetBytes(sData);
            Array.Copy(Data, 0, szFileData, 0, Data.Length);
            nFileDataLen = Data.Length;
            return SendData();
        }

        private string GetResponse()
        {
            if (GetData())
                return GetString(szFileData, 0, nFileDataLen);
            else
                return "";
        }

        private int m_nFileLineCount = 0;
        private void ReceiveFile(string sHeader)
		{
			long nBytes = 0;
			long nFileSize = 0;
            string sFileName;
                        
            string[] sParams = sHeader.Remove(0, 1).Split(' ');

            sFileName = Path.Combine(m_sRutaArchivos, m_sFilePrefix + "." + DateTime.Now.ToString("yyyyMMddHHmmss" + ".DSC"));
            nFileSize = Convert.ToInt32(sParams[1]);
            AddLog("Recibiendo {0}, Tamaño: {1}", sFileName, nFileSize);
            if ( nFileSize < 0)
            {
                AddLog("ERROR: Tamaño de archivo no válido");
                return;
            }


            // Se abre el archivo local donde se reciben los datos enviados
            FileStream file = null;
            try
            {
			    file = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                SendData("OK");
            }
            catch ( Exception ex)
            {
                if (file != null)
                    file.Close();
                AddLog("ERROR Abriendo archivo {0}, {1}", sFileName, ex.Message);
                SendData("ERROR");
                return;
            }

            try
            {	        
                // se reciben los datos enviados y se guardan en el archivo local hasta terminar.
           	    nBytes = 0;
                m_nFileLineCount = 0;
                ShowProgressHandler funcShowProgress = new ShowProgressHandler(ShowProgress);
			    while( nBytes < nFileSize &&  nFileDataLen > 0)
			    {
                    if ( WaitForData() )
                    {
                        if (!GetData())
                        {
                            AddLog("Error recibiendo datos archivo");
                            break;
                        }
                        file.Write(szFileData, 0, nFileDataLen);
                        nBytes += nFileDataLen;
                        this.Invoke(funcShowProgress, (int)((nBytes * 100) / nFileSize));
                    }
                    else
                    {
                        AddLog("Error: Timeout");
                        break;
                    }                    
			    }
                if (nBytes == nFileSize)
                {
                    AddLog("Archivo recibido! {0} Lineas", m_nFileLineCount);

                }
		    }
            finally
            {
	            file.Close();
                if ( nBytes < nFileSize )
                    File.Delete(sFileName);
            }                
		}

        private delegate void ShowProgressHandler(int nProgress);
        private void ShowProgress(int nProgress)
        {
            ctlProgress.Value = nProgress;              
        }

        private void AddLog(string sMsg, params object[] args)
        {
            if (args.Length > 0)
                sMsg = String.Format(sMsg, args);
            if ( m_AddLogItem == null )
                m_AddLogItem = new AddLogItemHandler(AddLogItem);
            this.Invoke(m_AddLogItem, sMsg);
        }


        private delegate void AddLogItemHandler(string sMsg);
        private AddLogItemHandler m_AddLogItem;
        private void AddLogItem(string sMsg)
        {
            txtRegistro.Text = txtRegistro.Text + sMsg + "\r\n";
        }

        private void WinTMSServerForm_Load(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Maximized;
            uiHandler1.Parent = this;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;            
            InitThread();
            UIHandler.EndWait();
        }

        private void WinTMSServerForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            UIHandler.StartWait();
            StopThread();
            DialogResult = DialogResult.Cancel;            
        }


    }
}