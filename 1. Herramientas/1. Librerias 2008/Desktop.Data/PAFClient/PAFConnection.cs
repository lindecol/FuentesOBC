using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;

#if NETCF20
namespace MovilidadCF.Data.PAFClient
{
#else
namespace Desktop.Data.PAFClient
{
#endif

    public class PAFConnection
    {

        #region "propiedades públicas de la clase"
        private const int BUFFER_SIZE = 4098;

        byte PAF_KEEPALIVE = Convert.ToByte('#');
                        
        private string m_PrimaryHost;
        private int _codePage = 1252;

        public int CodePage
        {
            get { return _codePage; }
            set { _codePage = value; }
        }

        private Encoding _encoding;

        public Encoding Encoding
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

        public string PrimaryHost
        {
            get { return m_PrimaryHost; }
            set { m_PrimaryHost = value; }
        }

        private int m_PrimaryPort;
        public int PrimaryPort
        {
            get { return m_PrimaryPort; }
            set { m_PrimaryPort = value; }
        }

        private string m_SecondaryHost;
        public string SecondaryHost
        {
            get { return m_SecondaryHost; }
            set { m_SecondaryHost = value; }
        }

        private int m_SecondaryPort;
        public int SecondaryPort
        {
            get { return m_SecondaryPort; }
            set { m_SecondaryPort = value; }
        }

        private string m_IDSistema = "0000";
        public string IDSistema
        {
            get { return m_IDSistema; }
            set
            {
                if (value.Length > 4)
                    value = value.Substring(0, 4);
                else if (value.Length < 4)
                {
                    value = value.PadLeft(4);
                }
                m_IDSistema = value;
            }
        }

        private int _queryTimeOut = 40;
        public int QueryTimeOut
        {
            get { return _queryTimeOut; }
            set { _queryTimeOut = value; }
        }


        #endregion

        #region "variables privadas"
        private Socket client;
        #endregion



        public PAFConnection()
        {
        }

        public PAFConnection(string IP, int port)
        {
            m_PrimaryHost = IP;
            m_PrimaryPort = port;
        }


        internal void OpenConnection()
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(new IPEndPoint(IPAddress.Parse(m_PrimaryHost),
                    Convert.ToInt32(m_PrimaryPort)));
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, 0);
            }
            catch (Exception ex)
            {
                if (!DBUtils.IsNullOrEmpty(m_SecondaryPort))
                {
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(new IPEndPoint(IPAddress.Parse(m_SecondaryHost),
                    Convert.ToInt32(m_SecondaryPort)));
                }
                else
                    throw ex;
            }
        }

        internal void CloseConnection()
        {
            if (client != null)
            {
                try
                {
                    client.Close();
                }
                catch
                {
                    // no se requiere manejo de esta excepcion. 
                }                
                client = null;
            }
        }

        internal void SendData(string sData)
        {
            byte[] data = Encoding.GetBytes(sData);
            client.Send(data, 0, data.Length, SocketFlags.None);
        }

        internal string GetData(bool UseDataSizeHeader)
        {
            if (UseDataSizeHeader)
            {
                return GetBlockData();
            }
            else
            {
                return GetData();
            }
        }

        private byte[] ReadBlock(int Size, bool DiscardKeepAlive)
        {
            byte[] data = new byte[Size];
            byte[] buffer = new byte[1];
            int readSize = 0;
            int totalSize = 0;
            int i = 0;
            while (client.Connected)
            {
                readSize = client.Receive(buffer, 0, 1, SocketFlags.None);
                if (readSize > 0)
                {
                    if (totalSize == 0 && DiscardKeepAlive && buffer[0] == PAF_KEEPALIVE)
                    {
                        continue;
                    }
                    else
                    {
                        data[totalSize] = buffer[0];
                        totalSize++;
                        if (totalSize == Size)
                        {
                            return data;
                        }
                    }
                }
            }
            throw new ProtocolViolationException("El servidor se desconecto antes de tiempo");           
        }


        private string GetBlockData()
        {
            byte[] data;
            bool tryReadData = false;
            StringBuilder sb = new StringBuilder();
            
            // Se lee el bloque del header
            data = ReadBlock(16, true);
            sb.Append(Encoding.GetString(data, 0, 16));

            // Se lee el bloque de tamaño de datos
            // @DATASIZE=00004096|
            data = ReadBlock(19, false);
            string sData = Encoding.GetString(data, 0, 19);
            int dataSize = Convert.ToInt32(sData.Substring(10, 8));

            // se lee el bloque de datos
            data = ReadBlock(dataSize, false);
            sb.Append(Encoding.GetString(data, 0, dataSize));
            
            return sb.ToString();
          
        }

        private string GetData()
        {
            StringBuilder sb = new StringBuilder();
            byte[] data = new byte[BUFFER_SIZE];
            int totalDataSize = 0;
            int readDataSize = 0;
            int retries = 0;
            int dataLen = 0;
            int availableData = 0;
            int index = 0;
            int maxRetriesForInitialData = _queryTimeOut * 1000 / 50;
            bool tryReadData = false;
            while (client.Connected)
            {
                availableData = client.Available;
                if (availableData > 0)
                {

                    if (totalDataSize == 0 && availableData < 15)
                    {
                        dataLen = 15;
                    }
                    else
                    {
                        if (availableData <= BUFFER_SIZE)
                            dataLen = availableData;
                        else
                            dataLen = BUFFER_SIZE;
                    }
                    
                    //if ((totalDataSize == 0 && availableData > 15) ||
                    //    (totalDataSize > 0 && availableData > 0))
                    //{
                    //    if (availableData <= BUFFER_SIZE)
                    //        dataLen = availableData;
                    //    else
                    //        dataLen = BUFFER_SIZE;
                    //}
                    
                    readDataSize = client.Receive(data, dataLen, SocketFlags.None);
                    if (readDataSize > 0)
                    {
                        retries = 0;
                        index = 0;                       
                        if (totalDataSize == 0)
                        {
                            // se ignoran los caracteres de KEEP_ALIVE enviados por PAF.
                            while (data[index] == PAF_KEEPALIVE)
                            {
                                index++;
                            }
                        }
                        totalDataSize += readDataSize - index;
                        sb.Append(Encoding.GetString(data, index, readDataSize - index));
                    }
                    else
                    {
                        throw new ProtocolViolationException("Error leyendo datos de servidor PAF");
                    }
                }
                else
                {
                    retries++;
                    if (totalDataSize == 0)
                    {
                        if (retries > maxRetriesForInitialData)
                        {
                            throw new TimeoutException("El servidor PAF no ha respondido en el tiempo limite permitido");
                        }
                    }
                    else
                    {
                        if (retries > 3)
                            break;
                    }
                    Thread.Sleep(_queryTimeOut);
                }
            }
            if (totalDataSize < 15)
            {
                throw new TimeoutException("No se recibio una trama de datos correcta");
            }
            return sb.ToString();
        }
    }
    
}
