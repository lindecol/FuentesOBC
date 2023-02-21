﻿using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Diagnostics;
using System.IO;
using Datascan.TextSerializer;
using Datascan.Compression;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    public Service () {

        //Eliminar la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    /// <summary>
    /// Funcion para obtener la fecha del servidor y sincronizarla con la terminal
    /// </summary>
    /// <returns>Fecha en formato string</returns>
    [WebMethod()]
    public string GetFechaSistema()
    {
        string sFecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        sFecha = sFecha.Replace(Convert.ToChar("-"), Convert.ToChar("/"));
        return sFecha;
    }

    [WebMethod()]
    public DataSet GetGrupos()
    {
        WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
        GestorDatos gestorDatosCarga = null;
        try
        {
            gestorDatosCarga = new GestorDatos();
            gestorDatosCarga.ConfigurarGestor(Context.Request.PhysicalApplicationPath, wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle), 0, 2, Convert.ToInt32(wsConfiguraciones.CodigoEmpresa), Convert.ToInt32(wsConfiguraciones.CodigoGrupo));
            DataSet dsGrupos = gestorDatosCarga.RecuperaGrupos();
            return dsGrupos;
        }
        catch (Exception ex)
        {
            gestorDatosCarga.GuardarErrores(ex.Message);            
            throw (ex);
        }
        finally
        {
            gestorDatosCarga.Desconectar();
        }

    }

    [WebMethod()]
    public string GetNombreGrupo(string idGrupo)
    {
        WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
        GestorDatos gestorDatosCarga = null;
        try
        {
            gestorDatosCarga = new GestorDatos();
            gestorDatosCarga.ConfigurarGestor(Context.Request.PhysicalApplicationPath, wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle), 0, 2, Convert.ToInt32(wsConfiguraciones.CodigoEmpresa), Convert.ToInt32(wsConfiguraciones.CodigoGrupo));
            DataSet dsGrupos = gestorDatosCarga.RecuperaNombreGrupo(idGrupo);
            return dsGrupos.Tables[0].Rows[0].ItemArray[0].ToString() ;
        }
        catch (Exception ex)
        {
            gestorDatosCarga.GuardarErrores(ex.Message);
            throw (ex);
            
        }
        finally
        {
            gestorDatosCarga.Desconectar();
        }

    }

    [WebMethod()]
    public string GetDatosCarga(string sGrupoTerminal, int iCodigoProceso, int iCodigoPrograma)
    {
        DataTable dtTablasConsultar;
        DataSet dsEnvioTerminal = null;
        WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
        DataRow rwRegistroTablaAuditoria = null;
        GestorDatos gestorDatosCarga = null;
        bool bActualizarRegistro = true;

        try
        {
            gestorDatosCarga = new GestorDatos();
            gestorDatosCarga.ConfigurarGestor(Context.Request.PhysicalApplicationPath, wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle), iCodigoProceso, iCodigoPrograma, Convert.ToInt32(wsConfiguraciones.CodigoEmpresa), Convert.ToInt32(wsConfiguraciones.CodigoGrupo));
            if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
            {
                throw new Exception("Código Programa Errado");
            }
            //Validar el grupo de terminales
            gestorDatosCarga.ValidarGrupoTerminal(sGrupoTerminal);
            //Validar el procedimiento
            gestorDatosCarga.ValidarProcedimiento(false, false);

            bActualizarRegistro = false;
            gestorDatosCarga.EjecutarProcedimientoAlmacenado(rwRegistroTablaAuditoria, true);
            bActualizarRegistro = true;

            //OBTENER LAS TABLAS A REVISAR
            dtTablasConsultar = gestorDatosCarga.ObtenerTablasConsultar();

            dsEnvioTerminal = gestorDatosCarga.LlenarDatasetDescarga(dtTablasConsultar);

            //REVISAR SI EXISTEN DATOS DE RESPUESTA
            if (dsEnvioTerminal != null)
            {
                //ENVIAR EL STRING CON LOS DATOS REQUERIDOS                
                string sRespuesta = TextSerializer.Serialize(dsEnvioTerminal);

                gestorDatosCarga.GuardarAuditoriaDatos(dsEnvioTerminal, sRespuesta);                
                
                gestorDatosCarga.InsertarRegistroResultadoComunicacionGrupo(sRespuesta,"V");
                
                byte[] btData = DataCompression.Compress(sRespuesta).ToArray();
                return Convert.ToBase64String(btData);
            }
            else
            {
                return null;
            }
            
        }
        catch (Exception ex)
        {            
            gestorDatosCarga.GuardarErrores(ex.Message);
            if (bActualizarRegistro && rwRegistroTablaAuditoria != null)
            {
                gestorDatosCarga.CerrarRegistroAuditoriaCarga(Convert.ToInt32(rwRegistroTablaAuditoria["IdProcesoCarga"]), EstadoEjecucionProcedimientoAlmacenado.ErrorEjcucionWebServices, ex.Message);
            }
            throw (ex);
        }
        finally
        {
            gestorDatosCarga.Desconectar();
        }

    }

    /// <summary>
    /// Funcion para confirmar los procesos de comunicacion desde la terminal
    /// </summary>
    /// <param name="sGrupoTerminal">Codigo Grupo Terminal</param>
    /// <param name="iCodigoProceso">Codigo Proceso ejecutado</param>
    /// <param name="iCodigoPrograma">Codigo del programa</param>
    [WebMethod()]
    public void ConfirmarCarga(string sGrupoTerminal, int iCodigoProceso, int iCodigoPrograma)
    {
        WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
        GestorDatos gestorDatosCarga = null;
        try
        {
            gestorDatosCarga = new GestorDatos();
            gestorDatosCarga.ConfigurarGestor(Context.Request.PhysicalApplicationPath, wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle), iCodigoProceso, iCodigoPrograma, Convert.ToInt32(wsConfiguraciones.CodigoEmpresa), Convert.ToInt32(wsConfiguraciones.CodigoGrupo));
            //LANZAR UNA EXCEPTION INDICANDO QUE EL PROGRAMA ESTA ERRADO
            if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
            {
                throw new Exception("Código Programa Errado");
            }
            //Validar el grupo de terminales
            gestorDatosCarga.ValidarGrupoTerminal(sGrupoTerminal);
            //VALIDAR PROCEDIMIENTO
            gestorDatosCarga.ValidarProcedimiento(false, true);

            //EJECUTAR EL PROCEDIMIENTO ALMACENADO            
            gestorDatosCarga.EjecutarProcedimientoAlmacenado(null, true);
            //ACTUALIZAR EL REGISTRO DE WS_RESULTADOS COMUNICACION
            gestorDatosCarga.ActualizarRegistroResultadoComunicacionGrupo(DateTime.Now, "C", "");            

        }
        catch (Exception ex)
        {
            //SI OCURRIO ALGUN ERROR ENVIAR LA EXCEPCION
            gestorDatosCarga.GuardarErrores(ex.Message);
            throw (ex);
        }
        finally
        {
            gestorDatosCarga.Desconectar();
        }
    }

    /// <summary>
    /// FUNCION PARA PROCESAR LOS DATOS ENVIADOS DESDE LA TERMINAL
    /// </summary>
    /// <param name="sTerminal">Codigo de la terminal que envia los datos</param>
    /// <param name="iCodigoPrograma">Codigo del programa</param>
    /// <param name="iCodigoProceso">Codigo del proceso</param>
    /// <param name="sDatosEnviados">Datos Enviados por la terminal en un string</param>
    /// <param name="sTerminal">Codigo del </param>
    [WebMethod()]
    public void SendDatos(string sGrupoTerminal, int iCodigoPrograma, int iCodigoProceso, string sDatosEnviados)
    {
        WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
        GestorDatos gestorDatosCarga = null;
        string sDatosEnviados_tmp = sDatosEnviados;
        try
        {
            gestorDatosCarga = new GestorDatos();
            gestorDatosCarga.ConfigurarGestor(Context.Request.PhysicalApplicationPath, wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle), iCodigoProceso, iCodigoPrograma, Convert.ToInt32(wsConfiguraciones.CodigoEmpresa), Convert.ToInt32(wsConfiguraciones.CodigoGrupo));
            if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
            {
                throw new Exception("Código Programa Errado");
            }
            //VALIDAR LA TERMINAL
            //Validar el grupo de terminales
            gestorDatosCarga.ValidarGrupoTerminal(sGrupoTerminal);
            //VALIDAR PROCEDIMIENTO
            gestorDatosCarga.ValidarProcedimiento(true, false);

            byte[] btData = Convert.FromBase64String(sDatosEnviados);
            sDatosEnviados = DataCompression.UnCompress(btData, btData.Length);
            DataSet dsGuardarDatos = TextSerializer.GetDataSet(sDatosEnviados);
            gestorDatosCarga.GuardarBackupDescarga(dsGuardarDatos, sDatosEnviados);
            gestorDatosCarga.DescargaDatosPRAX2000Grupo(sDatosEnviados, wsConfiguraciones.BackupTalonarios == "S", dsGuardarDatos);                 


        }
        catch (Exception ex)
        {
            //SI OCURRIO ALGUN ERROR ENVIAR LA EXCEPCION
            gestorDatosCarga.GuardarErrores(ex.ToString() + ' ' + sDatosEnviados_tmp);
            throw (ex);
        }
        finally
        {
            gestorDatosCarga.Desconectar();
        }
    }
}
