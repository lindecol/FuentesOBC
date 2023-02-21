using System;
using OpenNETCF.IO.Serial;
using System.Threading;
using System.IO;
using System.Text;

namespace MovilidadCF.Communications.WinTMS
{
	/// <summary>
	/// Descripción breve de TMSClient.
	/// </summary>
	public class TMSClient
	{

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
		
		private Port m_Port;
		private CRCTool m_CRCHelper;

		private byte[] szCommData = new byte[BUF_SIZE +10];
		private byte[] szFileData = new byte[BUF_SIZE +10];
		private int nFileDataLen = 0;
		private int nComDataLen = 0;


		public TMSClient()
		{
			m_CRCHelper = new CRCTool();
			m_CRCHelper.Init(CRCTool.CRCCode.CRC16);
		}

		public void OpenConnection()
		{
			HandshakeNone portSettings = new HandshakeNone();
			portSettings.BasicSettings.BaudRate = BaudRates.CBR_38400;
			portSettings.BasicSettings.ByteSize = 8;
			portSettings.BasicSettings.Parity = Parity.none;
			portSettings.BasicSettings.StopBits = StopBits.one;
			m_Port = new Port("COM1:", portSettings);
			m_Port.Open();
		}

		public void CloseConnection()

		{
			m_Port.Close();
			m_Port = null;
		}

		private UInt16 CalcCRC16(byte[] buffer, int offset, int len)
		{
			uint crc = (uint)m_CRCHelper.crctablefast(buffer, offset, len);
			byte lByte = (byte) ( (crc & 0xFF00) >> 8);
			byte hByte = (byte) ( (crc & 0x00FF) );
			if ( lByte == 0)
				lByte = 0xFF;
			if ( hByte == 0)
				hByte = 0xFF;
			return (UInt16)(lByte  << 8 | hByte);
		}

		private bool WriteData(byte[] Data)
		{
			m_Port.Output = Data;
			m_LastDataSent = Data;
			while (m_Port.OutBufferCount > 0)
				Thread.Sleep(100);
			return true;				
		}

		byte[] m_LastDataSent;

		private bool WriteData(byte[] Data, int len)
		{
			byte[] Buffer = new byte[len];
			Array.Copy(Data, 0, Buffer, 0, len);
			return WriteData(Buffer);
		}

		private void WaitForData()
		{
			for(int i=0; i < 30;i++)
			{
				if ( m_Port.InBufferCount > 0)
					break;
				Thread.Sleep(100);					
			}
		}

		private byte[] ReadData()
		{
			if ( m_Port.InBufferCount > 0)
				return m_Port.Input;
			else
				throw new ApplicationException("No se recibieron datos esperados");
		}

		private byte[] m_BufferByte = new byte[1];
		private void WriteByte(byte c)
		{
			m_BufferByte[0] = c;
			WriteData(m_BufferByte);
		}

		private byte[] m_InputBuffer;
		private int m_LastReadBytePos = 0;
		private byte ReadByte()
		{
			if ( m_InputBuffer == null || m_LastReadBytePos >= m_InputBuffer.Length)
			{
				m_InputBuffer = ReadData();
				m_LastReadBytePos = 0;
			}
			return m_InputBuffer[m_LastReadBytePos++];            
		}



		private bool GetData()
		{
			byte c = 0;
			int i=0, j=0;
			bool result = false;
			int nVeces = 0;
			int bETX = 0;
			bool bOK = false;
			bool bDLE = false;
			byte[] szAck = new byte[] {0x10, 0x06};
			byte[] szNak = new byte[] {0x10, 0x15};
			

			// Se reciben los datos y se guardan en el buffer, 
			// este proceso se repite hasta máximo tres veces
			//
			for ( nVeces = 0; nVeces < 3; nVeces++ )
			{
				result = false;
				if ( nVeces >= 3 )
	    			return false;
				j= 0;
				i = 0;
				bOK = false;
				bETX = 0;
				bDLE = false;
				c = 0;
			    
				for(;;)
				{
	    			// Se leen datos del puerto byte a byte  y se acumulan en el buffer
	    			// de datos
					c = ReadByte();
    				
    				if ( bETX > 0 )
    				{
    					szCommData[j++] = c;
    					bETX++;
    					if ( bETX == 3 )
    					{
    						bOK = true;
    						break;
    					}
    					else
    						continue;
    				}
    				else 
    				{
    					if ( bDLE )
	    				{
	    					bDLE = false;
	    					if ( c == STX )
	    					{
	    						j = 0;
	    						continue;
	    					}
	    					else if ( c == ETX )
	    						bETX = 1;
	    				}
	    				else if  ( c == DLE) 
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
					UInt16 lByte = (UInt16)szCommData[j-2];
					UInt16 hByte = (UInt16)szCommData[j-1];                    
					UInt16 rcvCRC = (UInt16)(hByte << 8 | lByte);
	    			UInt16 calcCRC = CalcCRC16(szCommData,0, j-2);					
	    			if ( rcvCRC != calcCRC )
						throw new ApplicationException("Bad CRC");
	   			}
	   			else
				{
	   				throw new ApplicationException("Bad Data");
				}
	   			if ( bOK )
	   			{
	   				i = 0;
	   				j = 0;
					for(i=0; i < nComDataLen-4;i++)
					{
						szFileData[j++] = szCommData[i];
    				}
    				nFileDataLen = j;
    				if ( WriteData(szAck) )
    					result = true;
    				else
    					return false;
   					break;
				}
				else
				{
	    			if ( WriteData(szNak) )
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
			byte c=0;
			byte[] szResponse = new byte[2];
			int i, j;
			bool result = false;
			szCommData[0] = DLE;
			szCommData[1] = STX;
			j = 2;
			for(i=0; i < nFileDataLen; i++)
			{
				szCommData[j++] = szFileData[i];
			}
			szCommData[j++] = DLE;
			szCommData[j++] = ETX;
			UInt16 CRC = CalcCRC16(szCommData, 2, j-2);
			byte lByte = (byte) ( (CRC & 0xFF00) >> 8);
			byte hByte = (byte) ( (CRC & 0x00FF) );
			szCommData[j++] = hByte;
			szCommData[j++] = lByte;
			nComDataLen = j;
		    
			// Se envia el buffer
			//
			for ( i = 0; i < 3; i++)
			{		    		
    			if ( ! WriteData(szCommData, nComDataLen) )
    				break;
			
    			c = 0;
				WaitForData();
				c = ReadByte();
				if ( c == DLE )
    			{
					c = ReadByte();
					{
    					szResponse[0] = DLE;
    					szResponse[1] = c;
    				}
				}
				if ( szResponse[0] == DLE && szResponse[1] == ACK )
				{
    				result = true;
    				break;
				}
				else if ( szResponse[0] != DLE || szResponse[1] != NAK )
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
			if ( GetData() )
				return GetString(szFileData, 0, nFileDataLen);
			else
				return "";
		}

		public void SendFile(string sOrigen, string sDestino)
		{
			long nBytes;
			long nFileSize;
			string sCommand, sResponse;


			// Se abre el puerto y archivo de entrada
			//
			OpenConnection();			
			try
			{

				// Se abre el archivo y se obtiene el tamaño del mismo
				//
				FileInfo fInfo = new FileInfo(sOrigen);
				nFileSize = fInfo.Length;
				FileStream file = new FileStream(sOrigen, FileMode.Open, FileAccess.Read);

				try
				{
					// Se envia el encabezado
					//
					sCommand = String.Format("C{0} {1}", sDestino, nFileSize);
					SendData(sCommand);
					WaitForData();
					sResponse = GetResponse();
					if ( sResponse != "OK" )
						throw new ApplicationException("Archivo no disponible en servidor");

					// Se envian los datos
					//
					nFileDataLen = file.Read(szFileData, 0, PACKET_SIZE);
					nBytes = 0;
					while( nBytes < nFileSize &&  nFileDataLen > 0)
					{
						// se eliminan caracteres nulos y extraños antes de enviar
						//
						nBytes += nFileDataLen;
						for (int i=0 ;i<nFileDataLen;i++)
						{
							if ( szFileData[i] == 0 )
								szFileData[i] = Convert.ToByte(' ');
						}
						if ( !SendData() )
							throw new ApplicationException("Error de comunicaciones");
						nFileDataLen = file.Read(szFileData, 0, PACKET_SIZE);
					}
				}
				finally 
				{
					file.Close();
				}

			}
			finally
			{
				CloseConnection();
			}
		}

	}	
}
