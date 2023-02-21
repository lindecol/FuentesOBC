using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;
using System.Net;
using System.IO;

namespace MovilidadCF.Client.Comunicacion
{
    internal class DescargaVersiones
    {
        private string _ArchivoDescarga = "";
        private string _URLDescarga = "";
        private double m_NumeroBytes = 0;

        public double NumeroBytes
        {
            get { return m_NumeroBytes; }
            set { m_NumeroBytes = value; }
        }
        private static ConfiguracionServidorWS m_ConfiguracionWS;
        private MovilidadCF.Client.WSComunicaciones.DataAccess m_ws;       

        public static void ConfiguracionServidorWS(string pIPServidor, string pPuertoServidor, string pVirtualDirectory, string pUsuarioWebService, string pClaveWebService, int pTimeOutWebServices)
        {
            m_ConfiguracionWS = new ConfiguracionServidorWS(pIPServidor, pPuertoServidor, pVirtualDirectory, pUsuarioWebService, pClaveWebService, pTimeOutWebServices);
        }

        private void ConfigurarWebService()
        {
            m_ws = new MovilidadCF.Client.WSComunicaciones.DataAccess();
            m_ws.Url = "http://" + m_ConfiguracionWS.IPServidor + ":" + m_ConfiguracionWS.PuertoServidor + "/" + m_ConfiguracionWS.VirtualDirectory + "/DataAccess.asmx";            
            m_ws.Timeout = m_ConfiguracionWS.TimeOutWebServices;
            m_ConfiguracionWS.TimeOutWebServices = m_ws.Timeout;
            if (m_ConfiguracionWS.UsuarioWebService != "")
            {
                m_ws.Credentials = new NetworkCredential(m_ConfiguracionWS.UsuarioWebService, m_ConfiguracionWS.ClaveWebService);
            }
        }

        public string CabInstalacion()
        {
            return Path.Combine(@"\Application", _ArchivoDescarga);
        }

        public Boolean RevisarVersiones(String VersionActual, String CodigoPrograma)
        {
            try
            {
                if (m_ConfiguracionWS != null)
                {
                    ConfigurarWebService();
                    _URLDescarga = m_ws.DescargarNuevaVersion(CodigoPrograma, VersionActual);
                    if (_URLDescarga != null)
                    {
                        return true;   
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }                
            }                
            catch (Exception ex)
            {
                MovilidadCF.Logging.Logger.Write(ex);
                return false;
            }
        }

        public Boolean DescargarVersion()
        {
            try
            {
                string[] ComposicionRuta = _URLDescarga.Split(Convert.ToChar("/"));
                if (ComposicionRuta.Length > 0)
                {
                    _ArchivoDescarga = ComposicionRuta[ComposicionRuta.Length - 1];
                    if (File.Exists(CabInstalacion()))
                    {
                        File.Delete(CabInstalacion());
                    }
                    DownloadFile(CabInstalacion(), _URLDescarga, m_ConfiguracionWS.UsuarioWebService, m_ConfiguracionWS.ClaveWebService);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                MovilidadCF.Logging.Logger.Write(ex);
                return false;
            }
        }

        public void BorrarArchivoTemporal()
        {
            try
            {                
                if (File.Exists(CabInstalacion()))
                {
                    File.Delete(CabInstalacion());
                }             
            }
            catch (Exception ex)
            {
                MovilidadCF.Logging.Logger.Write(ex);                
            }
        }

        private void DownloadFile(string sNombreArchivoLocal, string sUrl, string sUsuario, string sContrasena)
        {
            HttpWebRequest Req = ((HttpWebRequest)WebRequest.Create(sUrl));
            Req.Credentials = new NetworkCredential(sUsuario, sContrasena);
            Req.Method = "GET";

            Req.ContentType = "text/html";

            HttpWebResponse Res = null;
            FileStream fArchivoRecibio = null;
            try
            {

                Res = (HttpWebResponse)Req.GetResponse();

                Stream Resf = Res.GetResponseStream();
                fArchivoRecibio = new FileStream(sNombreArchivoLocal, FileMode.Create);
                byte[] nDatos = new byte[4097];
                NumeroBytes = 4096;
                int iBytesLeidos = Resf.Read(nDatos, 0, nDatos.Length);
                while (iBytesLeidos > 0)
                {
                    fArchivoRecibio.Write(nDatos, 0, iBytesLeidos);
                    iBytesLeidos = Resf.Read(nDatos, 0, nDatos.Length);
                    NumeroBytes = NumeroBytes + 4096;
                }
            }
            catch (Exception EX)
            {
                throw EX;
            }
            finally
            {
                if (Res != null)
                {
                    Res.Close();
                }
                if (fArchivoRecibio != null)
                {
                    fArchivoRecibio.Close();
                }
            }
        }
    }
}
