using System.Diagnostics;
using System.Configuration;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Globalization;


namespace MovilidadCF.Communications.MobilityCentral
{
	delegate void SetRowDataDelegate(string Field, string Value);
	
	public class MobilityCentralException : ApplicationException
	{
		public MobilityCentralException(string message) : base(message) {
		}
	}
	
	public class MobilityCentralClient
	{
		private MobilityCentralSocket m_Client;
		private EstadoForm m_frmEstado;
		private const int BUFFER_SIZE = 65536;
		private byte[] m_Buffer;
		
		public MobilityCentralClient() {
			m_Buffer = new byte[BUFFER_SIZE + 1];
			
			m_frmEstado = new EstadoForm();
			m_frmEstado.Show();
			m_frmEstado.ControlBox = false;
			Application.DoEvents();
		}
		
		private FileStream OpenFile(string LocalFile, System.IO.FileMode Mode)
		{
			try
			{
				FileStream fs = new FileStream(LocalFile, Mode);
				return fs;
			}
			catch (Exception)
			{
				throw (new MobilityCentralException("No es posible abrir archivo local: " + LocalFile));
			}
		}
		
		private void SetProgress (int Value)
		{
			m_frmEstado.ctlProgress.Value = Value;
			Application.DoEvents();
		}
		
		private void CheckConnection ()
		{
			if (m_Client == null)
			{
				throw (new MobilityCentralException("No se ha establecido conexin con el servidor!"));
			}
			if (! m_Client.Connected)
			{
				throw (new MobilityCentralException("No se ha establecido conexin con el servidor!"));
			}
		}
		
		public void Connect (string Host, int Port)
		{
			m_frmEstado.InitAction("Conexin a servidor", false);
			try
			{
				m_Client = new MobilityCentralSocket(Host, Port);
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				throw (new MobilityCentralException("No fue posible establecer conexin con el servidor"));
			}
		}
		
		public void Authorize (string DeviceID)
		{
			m_frmEstado.InitAction("Autorizando Conexin", false);
			CheckConnection();
			try
			{
				m_Client.SendData("AUTHORIZE");
				m_Client.SendData("DEVICE " + DeviceID);
				m_Client.CheckResponse();
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Conexin no autorizada por servidor: " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error autorizando conexin"));
				}
			}
		}
		
		public void Disconnect ()
		{
			try
			{
				if (m_Client != null)
				{
					m_Client.SendData("CLOSE");
					CloseConnection();
				}
			}
			catch
			{
			}
			m_frmEstado.ctlProgress.Visible = false;
			m_frmEstado.lblAccion.Text = "Transmisin Terminada!";
			m_frmEstado.ControlBox = true;
			m_frmEstado.Close();
		}
		
		public void SyncDateTime ()
		{
			string sData = "";
			DateTime t;
			m_frmEstado.InitAction("Sincronizando Reloj", false);
			CheckConnection();
			try
			{
				m_Client.SendData("DATE");
				m_Client.ReceiveData("DATETIME", ref sData);
				t = DateTime.ParseExact(sData, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                OpenNETCF.WindowsCE.DateTimeHelper.LocalTime = t;
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Error sincronizando fecha y hora con el servidor: " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error sincronizando fecha y hora con el servidor"));
				}
			}
		}
		
		public void SendFile (string LocalFile, string RemoteFile)
		{
			FileStream fs;
			int nBytesSend = 0;
			int nFileSize;
			int nBlockSize;
			
			m_frmEstado.InitAction("Enviando " + RemoteFile, true);
			try
			{
				CheckConnection();
				m_Client.SendData("PUT");
				m_Client.SendData("FILE " + RemoteFile);
				m_Client.CheckResponse();
                
				fs = OpenFile(LocalFile, FileMode.Open);
				try
				{
                    nFileSize = (int)fs.Length;
                    m_Client.SendData("FILESIZE " + fs.Length.ToString());
					while (nBytesSend < nFileSize)
					{
						// Se envia el el tamao del bloque, y los datos correspondients
						nBlockSize = fs.Read(m_Buffer, 0, BUFFER_SIZE);
						m_Client.SendData("BLOCK " + nBlockSize.ToString());
						m_Client.WriteBlock(m_Buffer, nBlockSize);
						nBytesSend += nBlockSize;
						SetProgress(System.Convert.ToInt32(nBytesSend * 100 / nFileSize));
					}
					m_Client.CheckResponse();
				}
				finally
				{
					fs.Close();
				}
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Error enviando archivo " + LocalFile + ": " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error enviando archivo " + LocalFile));
				}
			}
		}
		
		
		
		public void ReceiveFile (string LocalFile, string RemoteFile)
		{
			FileStream fs;
			int nFileSize = 0;
			int nWriteBytes = 0;
			int nBlockSize = 0;
			m_frmEstado.InitAction("Recibiendo " + RemoteFile, true);
			CheckConnection();
			try
			{
				m_Client.SendData("GET");
				m_Client.SendData("FILE " + RemoteFile);
				m_Client.CheckResponse();
				m_Client.WaitForData();
				m_Client.ReceiveData("FILESIZE", ref nFileSize);
				
				// Se reciben los datos del archivo y se guarda
				fs = OpenFile(LocalFile, FileMode.Create);
				try
				{
					while (nWriteBytes < nFileSize)
					{
						m_Client.ReceiveData("BLOCK", ref nBlockSize);
						m_Client.ReadBlock(m_Buffer, nBlockSize);
						fs.Write(m_Buffer, 0, nBlockSize);
						nWriteBytes += nBlockSize;
						SetProgress(System.Convert.ToInt32(nWriteBytes * 100 / nFileSize));
						Application.DoEvents();
					}
					m_Client.SendData("OK");
				}
				finally
				{
					fs.Close();
				}
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Error recibiendo archivo " + LocalFile + ": " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error recibiendo archivo " + LocalFile));
				}
			}
		}
		
		// Confirma el exito o fracaso en la recepcin del archivo, el parametro sMsg es
		// opcional y solo es utilizado si bSuccess = False
		public void ConfirmReceive (bool bSuccess, string sMsg)
		{
			try
			{
				if (bSuccess)
				{
					m_Client.SendData("OK");
				}
				else
				{
					m_Client.SendData("ERROR " + sMsg);
				}
			}
			catch
			{
				throw (new MobilityCentralException("Error enviando confirmacin recibo archivo"));
			}
		}
		
		private void CloseConnection ()
		{
			if (m_Client != null&& m_Client.Connected)
			{
				m_Client.Close();
				m_Client = null;
			}
		}
		
		//Funcion para Zip Archivos
		public void ZipFile (string RemoteZipFile, bool bDeleteFiles, bool bCrearEjecutable, params string[] FilesToZip)
		{
			string sCadenaArchivos = "";
			string sNombreArchivo;
			foreach (string tempLoopVar_sNombreArchivo in FilesToZip)
			{
				sNombreArchivo = tempLoopVar_sNombreArchivo;
				sCadenaArchivos = sCadenaArchivos + sNombreArchivo + "|";
			}
			sCadenaArchivos = sCadenaArchivos.Substring(0, sCadenaArchivos.Length - 1);
			m_frmEstado.InitAction("Enviando Nombre de Archivos a Comprimir ", true);
			try
			{
				CheckConnection();
				m_Client.SendData("ZIP");
				if (bDeleteFiles)
				{
					m_Client.SendData("DELETE 1");
				}
				else
				{
					m_Client.SendData("DELETE 0");
				}
				m_Client.CheckResponse();
				
				if (bCrearEjecutable)
				{
					m_Client.SendData("EXEC 1");
				}
				else
				{
					m_Client.SendData("EXEC 0");
				}
				
				m_Client.CheckResponse();
				m_Client.SendData("FILE " + sCadenaArchivos);
				m_Client.CheckResponse();
				m_Client.SendData("NAME " + RemoteZipFile);
				m_Client.CheckResponse();
				m_Client.SendData("END 1");
				m_Client.CheckResponse();
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Error Comprimiendo Archivos " + ": " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error Comprimiendo Archivos"));
				}
			}
		}
		
		//Funcion para UnZip Archivos
		public void UnZipFile (string RemoteZipFile, string sPathUnzip)
		{
			m_frmEstado.InitAction("Enviando Nombre de Archivos a Descomprimir ", true);
			try
			{
				CheckConnection();
				m_Client.SendData("UNZIP");
				m_Client.SendData("PATH " + sPathUnzip);
				m_Client.CheckResponse();
				m_Client.SendData("FILE " + RemoteZipFile);
				m_Client.CheckResponse();
				m_Client.SendData("END 1");
				m_Client.CheckResponse();
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Error Comprimiendo Archivos " + ": " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error Comprimiendo Archivos"));
				}
			}
		}
		
		public void DeleteFile (string RemoteFile)
		{
			m_frmEstado.InitAction("Enviando Nombre de Archivos a Eliminar ", true);
			try
			{
				CheckConnection();
				m_Client.SendData("DELETE");
				m_Client.SendData("FILE " + RemoteFile);
				m_Client.CheckResponse();
				m_Client.SendData("END 1");
				m_Client.CheckResponse();
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Error Eliminando Archivos " + ": " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error Eliminando Archivos"));
				}
			}
		}
		
		public void RemoteExecute (string RemoteProgramFile, string Parameters, bool bSynchronize, bool bOneInstance)
		{
			m_frmEstado.InitAction("Ejecutando programa remoto", true);
			try
			{
				CheckConnection();
				m_Client.SendData("EXECUTE");
				m_Client.SendData("PROGRAM " + RemoteProgramFile);
				m_Client.SendData("PARAMETERS " + Parameters);
				m_Client.SendData("SYNCHRONIZE " + bSynchronize.ToString().ToUpper());
				m_Client.SendData("ONEINSTANCE " + bOneInstance.ToString().ToUpper());
				m_Client.CheckResponse();
				m_Client.CheckResponse();
				m_frmEstado.EndAction();
			}
			catch (Exception ex)
			{
				CloseConnection();
				m_frmEstado.CancelAction(ex);
				if (ex is MobilityCentralException)
				{
					throw (new MobilityCentralException("Error ejecutando programa remoto " + ": " + "\n" + ex.Message));
				}
				else
				{
					throw (new MobilityCentralException("Error ejecutando programa remoto "));
				}
			}
		}
		
		
	}
	
	
	
}
