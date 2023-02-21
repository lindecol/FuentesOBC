using System;
using System.Collections.Generic;
using System.Text;

namespace MovilidadCF.Client
{
    public class Nucleo
    {
        public static void ConfigurarServicioWeb (string sIp, string sPuertoTCP, string sCarpetaVirtual, string sUsuarioWS, string sContrasenaWS, int iTimeOut){
            Comunicacion.CargaDatosForm.ConfiguracionServidorWS(sIp,sPuertoTCP,sCarpetaVirtual,sUsuarioWS,sContrasenaWS,iTimeOut);
            Comunicacion.DescargarDatosForm.ConfiguracionServidorWS(sIp, sPuertoTCP, sCarpetaVirtual, sUsuarioWS, sContrasenaWS, iTimeOut);
        }

        public static bool CargarDatos (string sCodPrograma, string sCodProceso, string sIdTerminal, string sDeviceId, bool bComprimir, bool bActualizarRegistros,
            bool bUsarGPRS, bool bCerrarVentana, string sNombreProceso, string sConexionGPRS, string sDataSource, object[] prParametros)
        {
            return Comunicacion.CargaDatosForm.Run(sCodPrograma,sCodProceso,sIdTerminal,sDeviceId,bComprimir,bActualizarRegistros,bUsarGPRS,bCerrarVentana,sNombreProceso,sConexionGPRS,sDataSource, true ,prParametros);
        }

        public static bool CargarDatos(string sCodPrograma, string sCodProceso, string sIdTerminal, string sDeviceId, bool bComprimir, bool bActualizarRegistros,
            bool bUsarGPRS, bool bCerrarVentana, string sNombreProceso, string sConexionGPRS, string sDataSource, bool LanzarExcepcion, object[] prParametros)
        {
            return Comunicacion.CargaDatosForm.Run(sCodPrograma, sCodProceso, sIdTerminal, sDeviceId, bComprimir, bActualizarRegistros, bUsarGPRS, bCerrarVentana, sNombreProceso, sConexionGPRS, sDataSource, LanzarExcepcion, prParametros);
        }

        public static bool DescargarDatos(string sCodPrograma, string sCodProceso, string sIdTerminal, string sDeviceId, bool bComprimir, bool bActualizarRegistros,
            bool bUsarGPRS, bool bCerrarVentana, string sNombreProceso, string sConexionGPRS, string sDataSource, object[] prParametros)
        {
            return Comunicacion.DescargarDatosForm.Run(sCodPrograma, sCodProceso, sIdTerminal, sDeviceId, bComprimir, bActualizarRegistros, bUsarGPRS, bCerrarVentana, sNombreProceso, sConexionGPRS, sDataSource, prParametros);
        }

        public static bool DescargarNuevaVersion(string CodigoPrograma, string VersionActual)
        {
            return Comunicacion.ActualizarVersiones.Run(CodigoPrograma, VersionActual);
        }

        public static void ConfigurarServicioWebDescarga(string sIp, string sPuertoTCP, string sCarpetaVirtual, string sUsuarioWS, string sContrasenaWS, int iTimeOut)
        {
            Comunicacion.DescargaVersiones.ConfiguracionServidorWS(sIp, sPuertoTCP, sCarpetaVirtual, sUsuarioWS, sContrasenaWS, iTimeOut);            
        }        
            
    }
}
