using System;
using OpenNETCF.Threading;
using System.Threading;
using System.IO;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Hilo de ejecución que realiza la recpeción de archivos 
	/// </summary>
	public class ReceiveFileThread
	{
		
		// Variables Privadas
		private Thread2 m_Thread;
		private IBluetoothProvider m_btProvider;
		private bool m_bMustStop = false;
		private const int BUFFER_SIZE = 24576;
		private byte[] m_Buffer = new byte[BUFFER_SIZE];		
		private string m_sWorkDir;
		
	
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sWorkDir"></param>
		public ReceiveFileThread(IBluetoothProvider btProvider, string sWorkDir)
		{			
			m_sWorkDir = sWorkDir;
			m_btProvider = btProvider;
		}

		/// <summary>
		/// Crea el hilo y lo inicia
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
			if ( m_Thread != null )
			{
				m_btProvider.CancelAcceptClient();
				if ( m_Thread.State == ThreadState.Running )
				{
					m_bMustStop = true;
					m_Thread.Join(2000);
				}			
			}
		}

		// Eventos del hilo		
		public event EventHandler StateChanged;
		public event EventHandler ProgressChanged;
		public event EventHandler ReceiveInit;
		public event EventHandler Completed;
		//public event EventHandler Error;
		
		/// <summary>
		/// Rutina de comunicaciones ejecutada en el hilo
		/// </summary>
		private void Execute()
		{
			
			string sFileName = "";
			string sSenderDevice = "";
			bool bSuccess = false;
			Protocol protocol = null; 
			FileStream file = null;
						
			while ( !m_bMustStop )
			{		
				try 
				{	

					// Se inicializa la barra de progreso
                    Invoker.Invoke(ProgressChanged, 0);

					// Se espera por conexión
					Invoker.Invoke(StateChanged, "Abriendo Servicio...");
					m_btProvider.PowerMode = PowerMode.Discoverable;
					m_btProvider.Listen();

					Invoker.Invoke(StateChanged, "Esperando Conexión...");
					m_btProvider.AcceptClient();
					protocol = new Protocol(m_btProvider);
					
					// Se espera por conexión
					Invoker.Invoke(StateChanged, "Esperando información archivo...");

					// Se obtiene el nombre y tamaño del archivo y se crea para recibir datos
					sSenderDevice = protocol.ReadCommandValue("DEVICEID");
					sFileName = protocol.ReadCommandValue("FILE");
					long nFileSize = Convert.ToInt32(protocol.ReadCommandValue("SIZE"));
					Invoker.Invoke(ReceiveInit, sSenderDevice, sFileName, nFileSize);
					Invoker.Invoke(StateChanged, "Abriendo Archivo!");
					file = new FileStream(Path.Combine(m_sWorkDir, sFileName), FileMode.Create);
					

					// Se envia respuesta positiva y se esperan los datos del archivo
					Invoker.Invoke(StateChanged, "Enviando Respuesta...");
					protocol.WriteString("OK");
					long nReceiveBytes = 0;
					int nBlockSize = 0;

					// Bucle de recibo de datos
					Invoker.Invoke(StateChanged, "Recibiendo " + sFileName + "...");
					Invoker.Invoke(ProgressChanged, 0);
					while ( !m_bMustStop && nReceiveBytes < nFileSize )
					{
						nBlockSize = Convert.ToInt32(protocol.ReadCommandValue("BLOCK"));
						protocol.ReadBlock(m_Buffer, nBlockSize);
						file.Write(m_Buffer, 0, nBlockSize);
						protocol.WriteString("OK");
						nReceiveBytes += nBlockSize;
						Invoker.Invoke(ProgressChanged, (int)(nReceiveBytes*100/nFileSize));
					}
					Invoker.Invoke(ProgressChanged, 0);
					if ( nReceiveBytes >= nFileSize)
					{
						Invoker.Invoke(StateChanged, "Archivo Recibido!");
						bSuccess = true;
					}
					file.Close();
				}
				catch// ( Exception ex)
				{
									//System.Windows.Forms.MessageBox.Show(ex.Message);
//					Invoker.Invoke(Error, "Error recibiendo archivo:\r\n"  
//						+ ex.GetType().ToString() + "\r\n" + ex.Message);
				}
				finally
				{
					// Se cierran todos los recursos utilizados 
					if ( protocol != null )
						protocol.Close();
					if ( file != null )
					{
						file.Close();
						if ( !bSuccess )
							File.Delete(file.Name);
					}
					Invoker.Invoke(Completed, sSenderDevice, sFileName, bSuccess);
					m_bMustStop = true;
				}
			}		

		}
	}
}
