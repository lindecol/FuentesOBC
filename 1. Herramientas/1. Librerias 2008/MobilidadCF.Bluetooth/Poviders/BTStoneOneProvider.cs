using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using OpenNETCF.Threading;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Descripción breve de BTStoneOneProvider.
	/// </summary>
	public class BTStoneOneProvider : IBluetoothProvider
	{

		#region Variables de uso privado

		int m_nBluetoothStackID = -1;
		int m_nRecordHandle = 0;
		int m_nSerialPortID = 0;
		string m_sRemoteAddres = "";
		private bool m_bAcceptingClient = false;
		private byte[] m_SingleByteBuffer = new byte[1];

		#endregion

		#region Declaraciones funciones externas implementadas en BTStoneOneLib
		
		[DllImport("BTStoneOneLib.DLL", EntryPoint="OpenBTStack", CharSet=CharSet.Unicode)]
		private static extern int API_OpenBTStack();

		[DllImport("BTStoneOneLib.DLL", EntryPoint="CloseBTStack", CharSet=CharSet.Unicode)]
		private static extern int API_CloseBTStack(int BluetoothStackID);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GAPInquiry", CharSet=CharSet.Unicode)]
		private static extern int API_GAPInquiry(int BluetoothStackID,int nTimeOut, int nMaxDevices, StringBuilder lpData);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GAPCancelInquiry", CharSet=CharSet.Unicode)]
		private static extern int API_GAPCancelInquiry(int BluetoothStackID);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetBdAddr", CharSet=CharSet.Unicode)]
		private static extern int API_GetBdAddr(int BluetoothStackID, StringBuilder szAddress);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="SetDeviceClass", CharSet=CharSet.Unicode)]
		private static extern int API_SetDeviceClass(int BluetoothStackID, uint COS);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetDeviceClass", CharSet=CharSet.Unicode)]
		private static extern int API_GetDeviceClass(int BluetoothStackID, byte[] lpDeviceClass);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="SetDeviceName", CharSet=CharSet.Unicode)]
		private static extern int API_SetDeviceName(int BluetoothStackID, string sDeviceName);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetDeviceName", CharSet=CharSet.Unicode)]
		private static extern int API_GetDeviceName(int BluetoothStackID, StringBuilder data);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="SetDiscoverabilityMode", CharSet=CharSet.Unicode)]
		private static extern int API_SetDiscoverabilityMode(int BluetoothStackID, int iMode);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetDiscoverabilityMode", CharSet=CharSet.Unicode)]
		private static extern int API_GetDiscoverabilityMode(int BluetoothStackID);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="SetConnectabilityMode", CharSet=CharSet.Unicode)]
		private static extern int API_SetConnectabilityMode(int BluetoothStackID, int iMode);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetConnectabilityMode", CharSet=CharSet.Unicode)]
		private static extern int API_GetConnectabilityMode(int BluetoothStackID);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetRemoteDeviceName", CharSet=CharSet.Unicode)]
		private static extern int API_GetRemoteDeviceName(int BluetoothStackID, string szAddress, StringBuilder wzName);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="OpenServer", CharSet=CharSet.Unicode)]
		private static extern int API_OpenServer(int BluetoothStackID, int PortID, ref int RecordHandle);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="Open", CharSet=CharSet.Unicode)]
		private static extern int API_Open(int BluetoothStackID, int PortID, string szRemoteAddress);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="ReadData", CharSet=CharSet.Unicode)]
		private static extern int API_ReadData(int BluetoothStackID, int SerialPortID, byte[] Data, int offset, int Length);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="WriteData", CharSet=CharSet.Unicode)]
		private static extern int API_WriteData(int BluetoothStackID, int SerialPortID, byte[] Data, int offset, int Length);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="CloseServer", CharSet=CharSet.Unicode)]
		private static extern int API_CloseServer(int BluetoothStackID, int SerialPortID, int RecordHandle);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="Close", CharSet=CharSet.Unicode)]
		private static extern int API_Close(int BluetoothStackID, int SerialPortID);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="AcceptClient", CharSet=CharSet.Unicode)]
		private static extern int API_AcceptClient(StringBuilder sbRemoteAddress);

		[DllImport("BTStoneOneLib.DLL", EntryPoint="CancelAcceptClient", CharSet=CharSet.Unicode)]
		private static extern void API_CancelAcceptClient();

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetAvailableData", CharSet=CharSet.Unicode)]
		private static extern int API_GetAvailableData();

		[DllImport("BTStoneOneLib.DLL", EntryPoint="GetConnectionStatus", CharSet=CharSet.Unicode)]
		private static extern int API_GetConnectionStatus();

		#endregion

		#region Constructor y Dispose

		public BTStoneOneProvider()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		
		#endregion

		#region Funciones privades adicionales requeridas

		private void CheckStack(bool bThrowException)
		{
			if ( m_nBluetoothStackID <= 0)
			{
				// Se abre el stack y se asigna el nombre del dispositivo
				m_nBluetoothStackID = API_OpenBTStack();
				if ( m_nBluetoothStackID > 0 )
					API_SetDeviceName(m_nBluetoothStackID, OpenNETCF.Environment2.MachineName);
			}
			if ( m_nBluetoothStackID <= 0 && bThrowException)
				throw new Exception("No es posible abrir el stack bluetooth");
		}

		private void CheckConnection()
		{
			if ( m_nBluetoothStackID <= 0)
				throw  new InvalidOperationException("No se ha inicializado el stack bluetooth");
			if ( API_GetConnectionStatus() !=  1 )
				throw new InvalidOperationException("No ha conexión activa");
		}

		#endregion

		#region Propiedades públicas de IBluetoothProvider

		public bool IsSupported 
		{
			get
			{				
				bool result = false;
				if ( System.IO.File.Exists("\\Windows\\HCICOMM.dll") )
					result = true;
				return result;
			}
		}


		public PowerMode PowerMode
		{
			get
			{
				CheckStack(true);
				int c = API_GetConnectabilityMode(m_nBluetoothStackID);
				int d = API_GetDiscoverabilityMode(m_nBluetoothStackID);
				PowerMode pm = PowerMode.Off;
				if ( c == 0 && d == 0)
					pm = PowerMode.Off;
				else if ( c == 1 && d == 0)
                    pm = PowerMode.Connectable;
				else if ( d == 1 )
					pm = PowerMode.Discoverable;
				return pm;
			}
			set
			{
				CheckStack(true);
				switch(value)
				{
					case PowerMode.Off:
						API_SetConnectabilityMode(m_nBluetoothStackID, 0);
						API_SetDiscoverabilityMode(m_nBluetoothStackID, 0);
						API_CloseBTStack(m_nBluetoothStackID);
						break;
					case PowerMode.Connectable:
						API_SetConnectabilityMode(m_nBluetoothStackID, 1);
						API_SetDiscoverabilityMode(m_nBluetoothStackID, 0);
						break;
					case PowerMode.Discoverable:
						API_SetConnectabilityMode(m_nBluetoothStackID, 1);
						API_SetDiscoverabilityMode(m_nBluetoothStackID, 1);
						break;
				}
			}
		}


		public bool DataAvailable
		{
			get
			{
				CheckConnection();
				return API_GetAvailableData() > 0;
			}
		}


		public bool IsConnected
		{
			get
			{
				return API_GetConnectionStatus() > 0;
			}
		}


		#endregion

		#region Implementación métodos IBluetoothProvider

		public BluetoothDeviceList DiscoverDevices(int nMaxDevices, int nTimeOut)
		{	
			string sLine = "";
			string[] values = null;
			BluetoothDevice dev = null;

			CheckStack(true);
			BluetoothDeviceList lstDevices = new BluetoothDeviceList();
			
			StringBuilder sb = new StringBuilder(new String('c', nMaxDevices * 80)); 
            int res = API_GAPInquiry(m_nBluetoothStackID, nTimeOut, nMaxDevices, sb);
			if ( res == 0)
			{
				StringReader sr = new StringReader(sb.ToString());
				sLine = sr.ReadLine();
				while (sLine != null )
				{
					values = sLine.Split('|');
					dev = new BluetoothDevice(values[0]);
					dev.Name = values[1];
					lstDevices.Add(dev);
					sLine = sr.ReadLine();
				}
				sr.Close();
			}
			//BluetoothDevice BluetoothDevice;
			                                                                                                                                                                                                                                                                                  return lstDevices;
		}

		public void Connect(BluetoothDevice RemoteDevice)
		{
			CheckStack(true);
			m_nSerialPortID = API_Open(m_nBluetoothStackID, 1, RemoteDevice.Address);
			if ( m_nSerialPortID <= 0 )
				throw new InvalidOperationException("No se pudo establecer conexión a dispositivo remoto: " + m_nSerialPortID);			
		}

		public void Listen()
		{
			CheckStack(true);
			m_nSerialPortID = API_OpenServer(m_nBluetoothStackID, 1, ref m_nRecordHandle);
			if ( m_nSerialPortID <= 0 )
				throw new InvalidOperationException("No se pudo iniciar el Puerto Servidor: " + m_nSerialPortID);
		}

		public void AcceptClient()
		{
			StringBuilder sb = new StringBuilder(50);
			CheckStack(true);
			if ( m_nRecordHandle > 0)
			{
				m_bAcceptingClient = true;
				if ( API_AcceptClient(sb) == 0 )
					throw new InvalidOperationException("AcceptClient Cancelado");
				m_bAcceptingClient = false;
				m_sRemoteAddres = sb.ToString();
			}
			else
				throw new InvalidOperationException("Servidor no ha sido abierto");
		}

		public void CancelAcceptClient()
		{
			CheckStack(true);
			if ( m_bAcceptingClient )
			{
				API_CancelAcceptClient();
				m_bAcceptingClient = false;
			}
		}

		public string GetRemoteDeviceName()
		{
			CheckStack(true);
			StringBuilder sb = new StringBuilder(256);
			API_GetRemoteDeviceName(m_nBluetoothStackID, m_sRemoteAddres, sb);
			return sb.ToString();
		}
	
		public void WriteByte(byte c)
		{
			m_SingleByteBuffer[0] = c;
			WriteData(m_SingleByteBuffer, 0, 1);			
		}

		public void WriteData(byte[] Data, int offset, int size)
		{
			int nWriteData = 0;
			int nTotalData = 0;
			CheckConnection();
			while( nTotalData < size )
			{
				nWriteData = API_WriteData(m_nBluetoothStackID, m_nSerialPortID, Data, offset, size);
				if ( nWriteData <= 0 )
					throw new Exception("Error enviando datos");
				nTotalData += nWriteData;
			}
		}

		public int ReadData(byte[] Data, int offset, int size)
		{
			CheckConnection();
			int nMaxRetries = 100;			
			int i = 0;
			int nAvailableData = 0;
			for (i = 0; i < nMaxRetries; i++)
			{
				nAvailableData = API_GetAvailableData();
				if ( nAvailableData >= size )
					break;
				else
					Thread2.Sleep(200);
			}
			if ( i == nMaxRetries )
				throw new System.Net.ProtocolViolationException("No han llegado datos suficientes");			

			int nBytesRead = API_ReadData(m_nBluetoothStackID, m_nSerialPortID, Data, offset, size);
			if ( nBytesRead < 0 )
				throw new InvalidOperationException("Ha ocurrido un error leyendo datos");
			return nBytesRead;
		}

		public byte ReadByte()
		{			
			m_SingleByteBuffer[0] = 0;
			ReadData(m_SingleByteBuffer, 0, 1);
			return m_SingleByteBuffer[0];
		}		

		public void Close()
		{
			if ( m_bAcceptingClient )
				API_CancelAcceptClient();
			if ( m_nRecordHandle > 0 && m_nSerialPortID >0 )
			{
				API_CloseServer(m_nBluetoothStackID, m_nSerialPortID, m_nRecordHandle);
				m_nRecordHandle = 0;
				m_nSerialPortID = 0;
			}
			else if ( m_nSerialPortID > 0)
			{
				// Espera para que se transmitar los últimos bytes
				API_Close(m_nBluetoothStackID, m_nSerialPortID);
				m_nSerialPortID = 0;
			}
			API_CloseBTStack(m_nBluetoothStackID);
			m_nBluetoothStackID = 0;
		}

		#endregion		

	}
}
