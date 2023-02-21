using System.Diagnostics;
using System.Configuration;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using OpenNETCF.Diagnostics;


namespace MovilidadCF.Communications.MobilityCentral
{
	internal class MobilityCentralSocket
	{
		private const byte STX = 0x2;
		private const byte ETX = 0x3;
		private const byte SOH = 0x1;
		private const byte EOT = 0x4;
		private const byte ENQ = 0x5;
		private const byte ACK = 0x6;
		private const byte NAK = 0x15;
		private const int BUF_SIZE = 1024;
		
		private Socket m_Socket;
		private string m_sHost;
		private int m_nPort;
		
		public MobilityCentralSocket(string Host, int Port) {
			TimeOut = 30;
			
			m_sHost = Host;
			m_nPort = Port;
			IPAddress ipHost = null;
			try
			{
				ipHost = IPAddress.Parse(Host);
			}
			catch
			{
			}
			if (ipHost == null)
			{
				try
				{
					IPHostEntry ipHostEntry = Dns.GetHostEntry(Host);
					if (ipHostEntry != null)
					{
                        ipHost = ipHostEntry.AddressList[0];
					}
				}
				catch (Exception)
				{
				}
			}
			
			if (ipHost == null)
			{
				throw (new MobilityCentralException("Direccin de servidor no vlida"));
			}
			
			m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_Socket.Connect(new IPEndPoint(ipHost, m_nPort));
			// m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000)
		}
		
		public string GetData()
		{
			byte c;
			byte[] Data = new byte[BUF_SIZE + 1];
			int nCount = 0;
			WaitForData();
			do
			{
				// Se lee cada caracter
				c = ReadByte();
				if (c == ACK | c == NAK | c == EOT)
				{
					break;
				}
				else if (c == ETX)
				{
					c = ReadByte();
					break;
				}
				Data[nCount] = c;
				nCount += 1;
			} while (DataAvailable);
			if (nCount > 0)
			{
				if (Data[0] == STX)
				{
					WriteByte(ACK);
					return System.Text.Encoding.Default.GetString(Data, 1, nCount - 1);
				}
				else
				{
					WriteByte(NAK);
				}
			}
			return "";
		}
		
		public void SendData (string sData)
		{
			byte c;
			int len = sData.Length + 3;
			byte[] Data = new byte[len+1 + 1];
			byte lrc = 0;
			bool bResult = false;
			int I;
			
			while (! bResult)
			{
				// Se arma la cadena en el formato adecuado y se envia
				Data[0] = STX;
				System.Text.Encoding.Default.GetBytes(sData).CopyTo(Data, 1);
				Data[len - 2] = ETX;
				Data[len] = EOT;
				for (I = 1; I <= len - 1; I++)
				{
					lrc = (byte)(lrc ^ Data[I]);
				}
				Data[len - 1] = lrc;
				if (m_Socket.Send(Data, len, SocketFlags.None) != len)
				{
					throw (new MobilityCentralException("Error enviando datos"));
				}
				
				// Se espera respuesta positiva
				for (I = 1; I <= 3; I++)
				{
					c = ReadByte();
					if (c == ACK)
					{
						bResult = true;
						//WriteByte(EOT)
						break;
					}
					else if (c == NAK)
					{
						break;
					}
					Thread.Sleep(100);
				}
			}
			//Return bResult
		}
		
		public bool DataAvailable
		{
			get{
				return m_Socket.Available > 0;
			}
		}
		
		// Tiempo de espera mximo (en segundos). Por defecto 5 segundos
		public int TimeOut;
		public void WaitForData ()
		{
			DateTime t1;
			DateTime t2;
			t1 = DateTime.Now;
			while (Connected & ! DataAvailable)
			{
				Thread.Sleep(100);
				t2 = DateTime.Now;
				if (t2.Subtract(t1).Seconds > TimeOut)
				{
					throw (new ProtocolViolationException("TimeOut"));
				}
			}
		}
		
		public bool Connected
		{
			get{
				return m_Socket.Connected;
			}
		}
		
		public void Close ()
		{
			m_Socket.Shutdown(SocketShutdown.Both);
			m_Socket.Close();
			m_Socket = null;
		}
		
		static byte[] ReadByte_Data = new byte[2];
		private byte ReadByte()
		{
			m_Socket.Receive(ReadByte_Data, 1, SocketFlags.None);
			return ReadByte_Data[0];
		}
		
		static byte[] WriteByte_Data = new byte[2];
		private void WriteByte (byte c)
		{
			WriteByte_Data[0] = c;
			if (m_Socket.Send(WriteByte_Data, 1, SocketFlags.None) != 1)
			{
				throw (new MobilityCentralException("Error enviando datos"));
			}
		}
		
		public void WriteBlock (byte[] Data, int Len)
		{
			if (m_Socket.Send(Data, Len, SocketFlags.None) != Len)
			{
				throw (new MobilityCentralException("Error enviando datos"));
			}
			WaitForData();
			if (ReadByte() == ACK)
			{
				return;
			}
			throw new System.Net.ProtocolViolationException();
		}
		
		public void ReadBlock (byte[] data, int nSize)
		{
			int nTotalLeidos = 0;
			int nLeidos = 0;
			int nFaltantes = 0;
			while (nTotalLeidos < nSize)
			{
				WaitForData();
				nFaltantes = nSize - nTotalLeidos;
				nLeidos = m_Socket.Receive(data, nTotalLeidos, nFaltantes, SocketFlags.None);
				nTotalLeidos += nLeidos;
			}
			WriteByte(ACK);
		}
		
		public void ReceiveData (string ParamName, ref int Value)
		{
			string sValue = null;
			ReceiveData(ParamName, ref sValue);
            try
            {
                Value = System.Convert.ToInt32(sValue);
            }
            catch
			{
				throw new System.Net.ProtocolViolationException();
			}			
		}
		
		public void ReceiveData (string ParamName, ref string Value)
		{
			string sData;
			
			sData = GetData();
			if (sData.StartsWith(ParamName))
			{
				sData = sData.Remove(0, ParamName.Length + 1).Trim();
				Value = sData;
			}
			else if (sData.StartsWith("ERROR"))
			{
				throw (new MobilityCentralException(sData.Remove(0, 6).Trim()));
			}
			else
			{
				throw new System.Net.ProtocolViolationException();
			}
		}
		
		public void CheckResponse ()
		{
			string sResponse;
			sResponse = GetData();
			if (sResponse.StartsWith("ERROR"))
			{
				throw (new MobilityCentralException(sResponse.Remove(0, 6).Trim(null)));
			}
			else if (! sResponse.StartsWith("OK"))
			{
				throw new System.Net.ProtocolViolationException();
			}
		}
		
	}
	
}
