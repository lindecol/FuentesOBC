using System;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Implementa las funciones que permiten manejar tarjetas bluetooth que implementa
	/// el stack de Microsoft.
	/// </summary>
	public class BTMicrosoftProvider : IBluetoothProvider
	{
		private BluetoothClient m_Client = null;
		private BluetoothListener m_Listener = null;
		private NetworkStream m_Stream = null;
		private byte[] m_SingleByteBuffer = new byte[1];
		
		public BTMicrosoftProvider()
		{

		}

		public bool IsSupported 
		{
			get
			{				
				bool result = false;
				try
				{
                    return BluetoothRadio.IsSupported;
				}
				catch
				{
				}
				return result;
			}
		}

		public PowerMode PowerMode
		{
			get
			{
				switch(BluetoothRadio.PrimaryRadio.Mode)
				{
					case RadioMode.PowerOff:
						return PowerMode.Off;
					case RadioMode.Connectable:
						return PowerMode.Connectable;
					case RadioMode.Discoverable:
						return PowerMode.Discoverable;
					default:
						return PowerMode.Off;
				}
			}
			set
			{
				switch(value)
				{
					case PowerMode.Off:
						BluetoothRadio.PrimaryRadio.Mode = RadioMode.PowerOff;
						break;
					case PowerMode.Connectable:
						BluetoothRadio.PrimaryRadio.Mode = RadioMode.Connectable;
						break;
					case PowerMode.Discoverable:
						BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;
						break;
				}
			}
		}

		public bool IsConnected
		{
			get
			{
				CheckClient(true);
				return m_Client.Connected;
			}
		}

		public bool DataAvailable
		{
			get
			{
				CheckClient(true);
				if ( m_Stream != null )
					return m_Stream.DataAvailable;
				else
					throw new InvalidOperationException("Conexión no ha sido abierta");
			}
		}

		private void CheckClient(bool bThrowException)
		{
			if ( m_Client == null )
				m_Client = new BluetoothClient();
			if ( m_Client == null && bThrowException )
				throw new InvalidOperationException("No se pudo abrir el cliente bluetooth");
		}

		public BluetoothDeviceList DiscoverDevices(int nMaxDevices, int nTimeOut)
		{			
			CheckClient(true);
			BluetoothDeviceInfo[] Devices;			
			BluetoothDeviceList lstDevices = new BluetoothDeviceList();
			BluetoothDevice BluetoothDevice;
			m_Client.InquiryLength = TimeSpan.FromSeconds(nTimeOut);		
			Devices = m_Client.DiscoverDevices(nMaxDevices, false, true, true);
			if ( Devices != null )
			{
				for(int i=0; i < Devices.Length; i++)
				{
					try
					{
						BluetoothDevice = new BluetoothDevice(Devices[i].DeviceAddress.ToString());
						BluetoothDevice.Name = Devices[i].DeviceName;
						if ( BluetoothDevice.Name == "" )
							BluetoothDevice.Name = BluetoothDevice.Address;
						lstDevices.Add(BluetoothDevice);
					}
					catch
					{
					}
				}
			}
			return lstDevices;
		}

		public void Connect(BluetoothDevice RemoteDevice)
		{
			CheckClient(true);
			m_Client.Connect(new BluetoothEndPoint(
				BluetoothAddress.Parse(RemoteDevice.Address), 
				BluetoothService.SerialPort) );
			m_Stream = (NetworkStream)m_Client.GetStream();
		}

		public void Listen()
		{
			m_Listener = new BluetoothListener(BluetoothService.SerialPort);
			m_Listener.Start();
		}

		public void AcceptClient()
		{
			m_Client = m_Listener.AcceptBluetoothClient();
			if ( m_Client == null )
				throw new InvalidOperationException("AccepptClient fue cancelado");
			m_Stream = (NetworkStream)m_Client.GetStream();
		}

		public void CancelAcceptClient()
		{
			if ( m_Listener != null )
				m_Listener.Stop();
		}

		public string GetRemoteDeviceName()
		{
			CheckClient(true);
			return m_Client.RemoteMachineName;
		}

		public void WriteByte(byte c)
		{
			m_SingleByteBuffer[0] = c;
			WriteData(m_SingleByteBuffer, 0, 1);			
		}

		public void WriteData(byte[] Data, int offset, int size)
		{
			CheckClient(true);
			m_Stream.Write(Data, offset, size);
		}

		public int ReadData(byte[] Data, int offset, int size)
		{
			CheckClient(true);
			return m_Stream.Read(Data, offset, size);
		}

		public byte ReadByte()
		{
			m_SingleByteBuffer[0] = 0;
			ReadData(m_SingleByteBuffer, 0, 1);
			return m_SingleByteBuffer[0];
		}		

		public void Close()
		{
			if ( m_Stream != null )
			{
				m_Stream.Close();
				m_Stream = null;
			}
			if ( m_Client != null )
			{
				m_Client.Close();
				m_Client = null;
			}
			if ( m_Listener != null )
			{
				m_Listener.Stop();
				m_Listener = null;
			}
		}
	}
}
