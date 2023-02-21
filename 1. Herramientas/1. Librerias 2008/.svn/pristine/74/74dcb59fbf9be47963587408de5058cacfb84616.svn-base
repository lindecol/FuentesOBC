using System;
using System.Data;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MovilidadCF.Windows.Forms;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Clase que permite hacer uso a alto nivel de tarjetas Bluetooth que
	/// implemente el stack de Microsoft o de StoneOne. 
	/// </summary>
	public class BTClient
	{

		private IBluetoothProvider Provider = null;
			
		/// <summary>
		/// Constructor, busca y crea el proveedor que funcione con la terminal
		/// </summary>
		public BTClient()
		{
			CheckProvider(false);
		}


		public void CheckProvider(bool bThrowException)
		{
			IBluetoothProvider btProvider = new BTStoneOneProvider();
			if ( btProvider.IsSupported )
				Provider = btProvider;
			else
			{
				btProvider = new BTMicrosoftProvider();
				if ( btProvider.IsSupported )
					Provider = btProvider;
				else
					Provider = null;
			}
			if ( Provider == null && bThrowException )
			{
				throw new InvalidOperationException("Dispositivo no tiene tarjeta Bluetooth, o no es compatible!");
			}
		}

		/// <summary>
		/// Indica si la tajeta bluetooth esta soportada en el dispositivo
		/// </summary>
		public bool IsSupported
		{
			get
			{
				CheckProvider(false);
				if ( Provider != null )
					return Provider.IsSupported;
				else
					return false;		
			}
		}

		public PowerMode PowerMode
		{
			get
			{
				CheckProvider(true);
				return Provider.PowerMode;
			}
			set
			{
				CheckProvider(true);
				Provider.PowerMode = value;
			}
		}	

		
		/// <summary>
		/// Permite imprimir un texto. La rutina internamente permite
		/// buscar y seleccionar el dispositivo destino e imprimir
		/// </summary>
		/// <param name="sPrintText"></param>
		/// <returns></returns>
		public bool PrintText(string sPrintText)
		{
			bool bRes = false;
			CheckProvider(true);
			bRes = (FormPrint.Run(Provider, sPrintText) == DialogResult.OK);
			Provider.PowerMode = PowerMode.Off;
			return bRes;
		}


		/// <summary>
		/// Permite recibir un archivo de otro dispositivo, utilizando un protocolo
		/// de comunicación propietario
		/// </summary>
		/// <param name="sWorkDir"></param>
		/// <param name="sSenderDevice"></param>
		/// <param name="sReceivedFile"></param>
		/// <returns></returns>
		public bool ReceiveFile(string sWorkDir, ref string sSenderDevice, 
			ref string sReceivedFile)
		{
			CheckProvider(true);
			FormReceiveFile.Resultado res = FormReceiveFile.Run(Provider, sWorkDir);
			Provider.PowerMode = PowerMode.Off;
			if ( res.Recibido )
			{
				sSenderDevice = res.SourceDevice;
				sReceivedFile = res.ReceivedFile;
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Permite enviar un archivo a otra PDA utlizando un protocolo de 
		/// comunicación propietario
		/// </summary>
		/// <param name="sFileName"></param>
		/// <param name="sTargetDevice"></param>
		/// <returns></returns>
		public bool SendFile(string sFileName, ref string sTargetDevice)
		{
			CheckProvider(true);
			FormSendFile.Resultado res = FormSendFile.Run(Provider,sFileName);
			Provider.PowerMode = PowerMode.Off;
			if ( res.Enviado )
			{
				sTargetDevice = res.TargetDevice;
				return true;
			}
			else			
				return false;
		}
		
	}
}
