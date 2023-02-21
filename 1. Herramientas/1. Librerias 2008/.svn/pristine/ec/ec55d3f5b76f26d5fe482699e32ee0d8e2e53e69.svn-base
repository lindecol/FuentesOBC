using System;
using InTheHand.Net;
using System.Collections;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Clase que encapsula la información de un dispositivo 
	/// Bluetooth con el cual se puede establecer comunicación
	/// </summary>
	public class BluetoothDevice 
	{
		private string m_sAddress;
		private string m_sName;
		private DeviceType m_Type;

		public BluetoothDevice(string sAddress)
		{
			m_sAddress = sAddress;
			m_sName = "";
			m_Type = DeviceType.Unknown;
		}

		public BluetoothDevice(string sAddress, string sDeviceName)
		{
			m_sAddress = sAddress;
			m_sName = sDeviceName;
			m_Type = DeviceType.Unknown;
		}
		
		public string Address
		{
			get
			{
				return m_sAddress;
			}
		}

		public string Name 
		{
			get
			{
				return m_sName;
			}
			set
			{
				m_sName = value;
			}
		}
	
		public DeviceType Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
			}
		}
		
	}

	


}
