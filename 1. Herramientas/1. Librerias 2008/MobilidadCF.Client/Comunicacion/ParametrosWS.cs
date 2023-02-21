using System;
using System.Collections.Generic;
using System.Text;

namespace MovilidadCF.Client.Comunicacion
{
    internal class ParametrosWS
    {
        public ParametrosWS(string pPrograma, string pProceso, string pIdTerminal, string pIdSistema, bool pComprimir, bool pUpdateCurrentRows,  bool pUsarGPRS, string pDescripcionProceso, params object[] pParametros)
        {
            this.Programa = pPrograma;
            this.Proceso = pProceso;
            this.IdTerminal = pIdTerminal;
            this.IdSistema = pIdSistema;            
            this.Comprimir = pComprimir;
            this.Parametros = pParametros;
            this.UpdateCurrentRows = pUpdateCurrentRows;
            this.UsarGPRS = pUsarGPRS;
            this.DescripcionProceso = pDescripcionProceso;
        }
        private string sPrograma;

        public string Programa
        {
            get { return sPrograma; }
            set { sPrograma = value; }
        }
        private string sProceso;

        public string Proceso
        {
            get { return sProceso; }
            set { sProceso = value; }
        }
        private string sIdTerminal;

        public string IdTerminal
        {
            get { return sIdTerminal; }
            set { sIdTerminal = value; }
        }
        private string sIdSistema;

        public string IdSistema
        {
            get { return sIdSistema; }
            set { sIdSistema = value; }
        }
        private string sIdComunicaciones;

        public string IdComunicaciones
        {
            get { return sIdComunicaciones; }
            set { sIdComunicaciones = value; }
        }
        private object[] rParametros;

        public object[] Parametros
        {
            get { return rParametros; }
            set { rParametros = value; }
        }

        private bool bComprimir;

        public bool Comprimir
        {
            get { return bComprimir; }
            set { bComprimir = value; }
        }

        private bool bUpdateCurrentRows = false;

        public bool UpdateCurrentRows
        {
            get { return bUpdateCurrentRows; }
            set { bUpdateCurrentRows = value; }
        }

        private bool bUsarGPRS = false;

        public bool UsarGPRS
        {
            get { return bUsarGPRS; }
            set { bUsarGPRS = value; }
        }

        private string sDescripcionProceso;

        public string DescripcionProceso
        {
            get { return sDescripcionProceso; }
            set { sDescripcionProceso = value; }
        }
    }
}
