using System;
using System.Collections.Generic;
using System.Text;

namespace MovilidadCF.Client.Comunicacion
{
    public class ConfiguracionServidorWS
    {
        public ConfiguracionServidorWS(string pIPServidor, string pPuertoServidor, string pVirtualDirectory, string pUsuarioWebService, string pClaveWebService, int pTimeOutWebServices)
        {
            this.IPServidor = pIPServidor;
            this.PuertoServidor = pPuertoServidor;
            this.VirtualDirectory = pVirtualDirectory;
            this.UsuarioWebService = pUsuarioWebService;
            this.ClaveWebService = pClaveWebService;
            this.TimeOutWebServices = pTimeOutWebServices;            
        }

        private string sIPServidor;

        public string IPServidor
        {
            get { return sIPServidor; }
            set { sIPServidor = value; }
        }
        private string sPuertoServidor;

        public string PuertoServidor
        {
            get { return sPuertoServidor; }
            set { sPuertoServidor = value; }
        }
        private string sVirtualDirectory;

        public string VirtualDirectory
        {
            get { return sVirtualDirectory; }
            set { sVirtualDirectory = value; }
        }
        private string sUsuarioWebService;

        public string UsuarioWebService
        {
            get { return sUsuarioWebService; }
            set { sUsuarioWebService = value; }
        }
        private string sClaveWebService;

        public string ClaveWebService
        {
            get { return sClaveWebService; }
            set { sClaveWebService = value; }
        }

        private int iTimeOutWebServices;

        public int TimeOutWebServices
        {
            get { return iTimeOutWebServices; }
            set { iTimeOutWebServices = value; }
        }

        private string sConexionGPRS;

        public string ConexionGPRS
        {
            get { return sConexionGPRS; }
            set { sConexionGPRS = value; }
        }
    }
}
