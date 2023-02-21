using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Clase que maneja el stream de transmisión de datos vía bluetooth
	/// Implementa un protocolo propietario basado en ACK/NAK
	/// </summary>
	internal class Protocol
	{
		// Variables y constantes 
		const byte STX = 0x02;
		const byte ETX = 0x03;
		const int TIMEOUT = 30;

		private IBluetoothProvider m_Provider;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="stream"></param>
		public Protocol(IBluetoothProvider Provider)
		{
			m_Provider =  Provider;
		}

		/// <summary>
		/// Envia una cadena a traves del stream de conexión bluetooth
		/// utilizando el protocolo propietario
		/// </summary>
		/// <param name="s"></param>
		public void WriteString(string s)
		{
			byte[] data = Encoding.Default.GetBytes(s);
			m_Provider.WriteByte(STX);
			m_Provider.WriteData(data, 0, data.Length);
			m_Provider.WriteByte(ETX);
		}

		/// <summary>
		/// Lee una cadena del stream de conexión utilizando el protocolo propietario
		/// </summary>
		/// <returns></returns>
		public string ReadString()
		{
			int c = 0;
			int size = 0;
			byte[] data = new byte[300];

			WaitForData(TIMEOUT);

			// Se reciben todos los bytes que componen la cadena
			for(int i=0; i < 300; i++)
			{
				c = m_Provider.ReadByte();
				if ( c == STX )
					size = 0;
				else if( c == ETX || c == -1)
					break;
				else
				{
					data[size] = (byte)c;
					size++;
				}
			}
			
			// Se retorna la cadena recibida
			string s = "";
			if ( size > 0 ) 
				s = Encoding.Default.GetString(data, 0, size);
			return s;
		}

		/// <summary>
		/// Escibe un bloque de datos binarios en el stream
		/// </summary>
		/// <param name="data"></param>
		/// <param name="nSize"></param>
		public void WriteBlock(byte[] data, int nSize)
		{
			m_Provider.WriteData(data, 0, nSize);
		}

		/// <summary>
		/// Lee un bloque de datos binarios en el stream
		/// </summary>
		/// <param name="data"></param>
		/// <param name="nSize"></param>
		public void ReadBlock(byte[] data,int nSize)
		{
			
			int nTotalLeidos = 0;
			int nLeidos = 0;
			int nFaltantes = 0;
			while ( nTotalLeidos < nSize )
			{
				WaitForData(TIMEOUT);
				nFaltantes = nSize - nTotalLeidos;
				nLeidos = m_Provider.ReadData(data, nTotalLeidos, nFaltantes);
				nTotalLeidos += nLeidos;
			}
		}

		/// <summary>
		/// Cierra el stream de conexión
		/// </summary>
		public void Close()
		{
			m_Provider.Close();
		}

		/// <summary>
		/// Lee y verifica un comando enviado por el otro dispositivo
		/// </summary>
		/// <param name="sCommmandTag"></param>
		/// <returns></returns>
		public string ReadCommandValue(string sCommmandTag) 
		{
			string sCommand = ReadString();
			string sValue = "";
			if ( sCommand.StartsWith(sCommmandTag) )
				sValue = sCommand.Remove(0, sCommmandTag.Length+1).Trim();
			else
				throw new System.Net.ProtocolViolationException("Comando no corresponde: " + sCommand);
			return sValue;				
		}

		public void WaitForData()
		{
			WaitForData(TIMEOUT);
		}
	
		/// <summary>
		/// Espera por disponibilidad de datos 
		/// </summary>
		private void WaitForData(int nTimeOut)
		{
			DateTime t1, t2;
			TimeSpan dif;
			t1 = DateTime.Now;
			while( !m_Provider.DataAvailable )
			{
				Thread.Sleep(100);
				t2 = DateTime.Now;
				dif = t2 - t1;
				if ( dif.Seconds > nTimeOut)
					throw new ProtocolViolationException("TimeOut");
			}
		}
	}
}
