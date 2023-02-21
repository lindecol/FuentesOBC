using System;
using OpenNETCF.Threading;
using System.Threading;
using System.IO;
using OpenNETCF.Win32;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Hilo de ejecución que realiza el envío de archivos
	/// </summary>
	internal class SendFileThread
	{
		// Identificador del servicio de transmisión de archivos
		private Guid FileTransferService = new Guid("{9C79A0C7-A2D4-4b8e-9240-5D7C7661D0D7}");

		// Variables privadas
		private Thread2 m_Thread;
		private bool m_bMustStop = false;
		private BluetoothDevice m_btTarget;
		private	string m_sFileName;
		private const int BUFFER_SIZE = 24576;
		private byte[] m_Buffer = new byte[BUFFER_SIZE];
		private IBluetoothProvider m_btProvider = null;
		

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="btTarget"></param>
		/// <param name="sFileName"></param>
		public SendFileThread(IBluetoothProvider btProvider, BluetoothDevice btTarget, string sFileName)
		{
			m_btTarget = btTarget;
			m_sFileName = sFileName;
			m_btProvider = btProvider;
		}

		/// <summary>
		/// Inicia el hilo de ejecución
		/// </summary>
		public void Start()
		{
			m_bMustStop = false;
			m_Thread = new Thread2(new ThreadStart(Execute));			
			m_Thread.Start();
		}

		/// <summary>
		/// Detiene el hilo de ejecución
		/// </summary>
		public void Stop()
		{
			if ( m_Thread.State == ThreadState.Running )
			{
				m_bMustStop = true;
				m_Thread.Join(2000);
				//m_Thread = null;
			}			
		}

		// Eventos del hilo		
		public event EventHandler StateChanged;
		public event EventHandler ProgressChanged;
		//public event EventHandler Error;
		public event EventHandler SendComplete;

		/// <summary>
		/// Rutina de transferencia de archivos ejecutada en el hilo
		/// </summary>
		private void Execute()
		{
			FileStream file = null;
			Protocol protocol = null;
			bool bSuccess = false;
			try
			{
				// Se inicializa la barra de progreso
				Invoker.Invoke(ProgressChanged, 0);
				
				// Se abre la conexión con el dispositivo seleccionado
				Invoker.Invoke(StateChanged, "Conectando...");
				m_btProvider.PowerMode = PowerMode.Connectable;
				m_btProvider.Connect(m_btTarget);

				// Se envia identificacion de la terminal 
				Invoker.Invoke(StateChanged, "Enviando ID Terminal...");

				// Se abre el archivo 
				protocol = new Protocol(m_btProvider);
				string sFileName = Path.GetFileName(m_sFileName);
				Invoker.Invoke(StateChanged,"Abriendo archivo...");
				file = new FileStream(m_sFileName, FileMode.Open, FileAccess.Read);
				
				// Se envia información del archivo 
				Invoker.Invoke(StateChanged, "Enviando información archivo...");
				protocol.WriteString("DEVICEID " +  OpenNETCF.Environment2.MachineName);
				protocol.WriteString("FILE " + sFileName);
				protocol.WriteString("SIZE " + file.Length.ToString());
				string sResponse = protocol.ReadString();

				// Se espera respuesta...
				Invoker.Invoke(StateChanged,"Esperando respuesta...");
				if ( sResponse == "OK" )
				{
					// Se abre el archivo y envian bloques de 
					// binarios hasta completarlo
					Invoker.Invoke(StateChanged, "Enviando contenido archivo...");
					int nBlockSize = 0;
					long nSendBytes = 0;
					long nFileSize = file.Length;
					Invoker.Invoke(ProgressChanged, 0);
					while( (nSendBytes < nFileSize) && !m_bMustStop )
					{
						nBlockSize = file.Read(m_Buffer, 0, BUFFER_SIZE);
						protocol.WriteString("BLOCK " + nBlockSize.ToString());
						protocol.WriteBlock(m_Buffer, nBlockSize);
						nSendBytes += nBlockSize;
						protocol.WaitForData();
						sResponse = protocol.ReadString();
						if ( sResponse != "OK" ) 
							throw new System.Net.ProtocolViolationException("No se recibio respuesta enviando bloque: " + sResponse);
						Invoker.Invoke(ProgressChanged,(int) (nSendBytes*100/nFileSize));
					}
					if ( nSendBytes >= nFileSize)
					{
						Invoker.Invoke(StateChanged,"Archivo Enviado!");
						bSuccess = true;
					}
					Invoker.Invoke(ProgressChanged, 0);
				}
			}
			catch  ( Exception ex)
			{
				StreamWriter sw = new StreamWriter("\\BTLog.TXT", true);
				sw.WriteLine("Excepción: " + ex.GetType().ToString() + "\r\n" + ex.Message);
				sw.Close();				
			}
			finally
			{
				if ( protocol != null )
					protocol.Close();
				if ( file != null )
					file.Close();						
				Invoker.Invoke(SendComplete,m_btTarget, m_sFileName, bSuccess);
			}
		}
	}
					
	
}
