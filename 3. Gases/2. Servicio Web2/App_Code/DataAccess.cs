using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using Datascan.TextSerializer;
using Datascan.Compression;


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class DataAccess : System.Web.Services.WebService
{
    public DataAccess()
    {
        //Eliminar la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    #region "Implementación de Metodos publicados WEB"

   
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
        
    /// <summary>
    /// FUNCION PARA ENVIAR ARCHIVOS A LA TERMINAL
    /// </summary>
    /// <param name="sTerminal">Codigo de la terminal que hace la peticion</param>
    /// <param name="iCodigoProceso">Codigo del proceso a ejecutar</param>
    /// <param name="iCodigoPrograma">Codigo del programa</param>
    /// <returns>DEVUELVE UNA CADENA DE CARACTERES QUE CONTIENE LA ESTRUCTURA DE UN ARCHIVO XML CON TODOS LOS DATOS QUE REQUIERE LA TERMINAL</returns>
    [WebMethod()]
    public string GetDatosCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma)
    {
        //string sFileLog = " ";
        //MIEMBRO PARA ALMACENAR EL NOMBRE DE LAS TABLAS QUE SE DEBEN ENVIAR A LA TERMINAL
        DataTable dtTablasConsultar;
        DataSet dsEnvioTerminal = null;        
        WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
        //VARIABLE PARA CONTROLAR EL ESTADO DE EJECUCION DEL PROCEDIMIENTO ALMACENADO        
        DataRow rwRegistroTablaAuditoria = null;
        GestorDatos gestorDatosCarga = null;
        bool bActualizarRegistro = true;
        try
        {
            gestorDatosCarga = new GestorDatos();
            gestorDatosCarga.ConfigurarGestor(Context.Request.PhysicalApplicationPath, wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle), iCodigoProceso, iCodigoPrograma, Convert.ToInt32(wsConfiguraciones.CodigoEmpresa), Convert.ToInt32(wsConfiguraciones.CodigoGrupo));
            //LANZAR UNA EXCEPTION INDICANDO QUE EL PROGRAMA ESTA ERRADO
            if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
            {
                throw new Exception("Código Programa Errado");
            }            
            //VALIDAR LA TERMINAL
            gestorDatosCarga.ValidarTerminal(sTerminal);
            //VALIDAR PROCEDIMIENTO
                gestorDatosCarga.ValidarProcedimiento(false, false);
                        
            bool bEjecutarProcedimientoAlmacenado = true;
            if (iCodigoProceso != 1)
            {
                //OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
                gestorDatosCarga.ObtenerPuntoVentaRutaPrincipal();
            }
            else
            {
                //SOLO PARA LA CARGA PRINCIPAL
                //ABRIL 2010: CAMBIO REVISION EN LA TABLA DE AUDITORIA DE PEDIDOS
                //REVISO EL ULTIMO REGISTRO                
                rwRegistroTablaAuditoria = gestorDatosCarga.ExisteRegistroAuditoriaCarga();
                if (rwRegistroTablaAuditoria == null)
                {
                    //INSERTO UN NUEVO REGISTRO E INICIO LA EJECUCION DEL PROCEDIMIENTO ALMACENADO
                    rwRegistroTablaAuditoria = gestorDatosCarga.InsertarRegistroAuditoriaCarga();
                    bEjecutarProcedimientoAlmacenado = true;
                }
                else
                {
                    if (Convert.ToInt32(rwRegistroTablaAuditoria["EstadoProceso"]) == Convert.ToInt32(EstadoEjecucionProcedimientoAlmacenado.InicioEjecucion))
                    {
                        //REVISO SI LLEVA MUCHO TIEMPO ESPERANDO CON EL UMBRAL
                        TimeSpan tiempoEspera = DateTime.Now - Convert.ToDateTime(rwRegistroTablaAuditoria["FechaInicioProcesamiento"]);
                        if (tiempoEspera.TotalMinutes > (wsConfiguraciones.UmbralFechaInicio))
                        {
                            //CIERRO EL REGISTRO E INICIO A EJECUTAR DE NUEVO EL PROCEDIMIENTO ALMACENADO
                            gestorDatosCarga.CerrarRegistroAuditoriaCarga(Convert.ToInt32(rwRegistroTablaAuditoria["IdProcesoCarga"]), EstadoEjecucionProcedimientoAlmacenado.CerradoFechaInicioMayorUmbral, "Cerrado Fecha Inicio Mayor Umbral");
                            rwRegistroTablaAuditoria = gestorDatosCarga.InsertarRegistroAuditoriaCarga();
                            bEjecutarProcedimientoAlmacenado = true;
                        }
                        else
                        {
                            bActualizarRegistro = false;
                            throw new Exception("El procedimiento almacenado se esta ejecutando DEBE esperar que termine");
                        }
                    }
                    else
                    {
                        //REVISO SI TERMINO CORRECTO
                        if (Convert.ToInt32(rwRegistroTablaAuditoria["EstadoProceso"]) == Convert.ToInt32(EstadoEjecucionProcedimientoAlmacenado.ProcesoTerminadoExitoso))
                        {
                            //REVISO SI NO SE HA PASADO LA HORA DE UMBRAL
                            TimeSpan tiempoFinalizar = DateTime.Now - Convert.ToDateTime(rwRegistroTablaAuditoria["FechaFinProcesamiento"]);
                            if (System.Math.Abs(tiempoFinalizar.TotalMinutes) > (wsConfiguraciones.UmbralFechaFin))
                            {
                                //CIERRO EL REGISTRO E INICIO A EJECUTAR DE NUEVO EL PROCEDIMIENTO ALMACENADO
                                gestorDatosCarga.CerrarRegistroAuditoriaCarga(Convert.ToInt32(rwRegistroTablaAuditoria["IdProcesoCarga"]), EstadoEjecucionProcedimientoAlmacenado.CerradoFechaFinalizacionMayorUmbral, "Cerrado Fecha Finalizacion Mayor Umbral");
                                rwRegistroTablaAuditoria = gestorDatosCarga.InsertarRegistroAuditoriaCarga();
                                bEjecutarProcedimientoAlmacenado = true;
                            }
                            else
                            {
                                //EL UNICO CAMINO PARA NO EJECUTAR EL PROCEDIMIENTO ALMACENADO
                                bEjecutarProcedimientoAlmacenado = false;
                            }
                        }
                        else
                        {
                            rwRegistroTablaAuditoria = gestorDatosCarga.InsertarRegistroAuditoriaCarga();
                            bEjecutarProcedimientoAlmacenado = true;
                        }
                    }
                }
            }

            //EJECUTAR EL PROCEDIMIENTO ALMACENADO
            if (bEjecutarProcedimientoAlmacenado)
            {
                bActualizarRegistro = false;
                gestorDatosCarga.EjecutarProcedimientoAlmacenado(rwRegistroTablaAuditoria, true);
            }

            bActualizarRegistro = true;
            //SI EL PROCESO ES 1 AHORA SI CONSULTAR PARAMETROS
            //VOLVER A CARGAR OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
            if (iCodigoProceso == 1)
            {
                gestorDatosCarga.ObtenerPuntoVentaRutaPrincipal();
            }

            //OBTENER LAS TABLAS A REVISAR
            dtTablasConsultar = gestorDatosCarga.ObtenerTablasConsultar();
            //CREAR LA ESTRUCTURA DEL DATASET DE RESPUESTA
            dsEnvioTerminal = gestorDatosCarga.LlenarDatasetDescarga(dtTablasConsultar);
            //REVISAR SI EXISTEN DATOS DE RESPUESTA
            if (dsEnvioTerminal != null)
            {
                //ENVIAR EL STRING CON LOS DATOS REQUERIDOS                
                string sRespuesta = TextSerializer.Serialize(dsEnvioTerminal);
                
                gestorDatosCarga.GuardarAuditoriaDatos(dsEnvioTerminal, sRespuesta);
                string rutaParametros = string.Empty;
                if (dsEnvioTerminal.Tables.Contains("Parametros"))
                {
                    rutaParametros = Convert.ToString(dsEnvioTerminal.Tables["Parametros"].Rows[0]["RutaPrincipal"]);
                }
                gestorDatosCarga.InsertarRegistroResultadoComunicacion(DateTime.Now, sRespuesta, "V", rutaParametros);                
                //return sRespuesta;
                //*****Compresion de datos
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
            //SI OCURRIO ALGUN ERROR ENVIAR LA EXCEPCION
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
    /// <param name="sTerminal">Codigo Terminal</param>
    /// <param name="iCodigoProceso">Codigo Proceso ejecutado</param>
    /// <param name="iCodigoPrograma">Codigo del programa</param>
    [WebMethod()]
    public void ConfirmarCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma)
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
            //VALIDAR LA TERMINAL
            gestorDatosCarga.ValidarTerminal(sTerminal);
            //VALIDAR PROCEDIMIENTO
            gestorDatosCarga.ValidarProcedimiento(false, true);
            //OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
            gestorDatosCarga.ObtenerPuntoVentaRutaPrincipal();
            
            //EJECUTAR EL PROCEDIMIENTO ALMACENADO            
            gestorDatosCarga.EjecutarProcedimientoAlmacenado(null, true);
            //ACTUALIZAR EL REGISTRO DE WS_RESULTADOS COMUNICACION
            gestorDatosCarga.ActualizarRegistroResultadoComunicacion(DateTime.Now, "C", "");            
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
    [WebMethod()]
    public void SendDatos(string sTerminal, int iCodigoPrograma, int iCodigoProceso, string sDatosEnviados)
    {        
        WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
        GestorDatos gestorDatosCarga = null;
        try
        {
            gestorDatosCarga = new GestorDatos();
            gestorDatosCarga.ConfigurarGestor(Context.Request.PhysicalApplicationPath, wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle), wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle), iCodigoProceso, iCodigoPrograma, Convert.ToInt32(wsConfiguraciones.CodigoEmpresa), Convert.ToInt32(wsConfiguraciones.CodigoGrupo));
            if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
            {
                throw new Exception("Código Programa Errado");
            }
            //VALIDAR LA TERMINAL
            gestorDatosCarga.ValidarTerminal(sTerminal);
            //VALIDAR PROCEDIMIENTO
            gestorDatosCarga.ValidarProcedimiento(true, false);
            //OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
            gestorDatosCarga.ObtenerPuntoVentaRutaPrincipal();
            //VOLVER DATASET LO QUE SE ENVIA
            byte[] btData = Convert.FromBase64String(sDatosEnviados);
            sDatosEnviados = DataCompression.UnCompress(btData, btData.Length);
            DataSet dsGuardarDatos = TextSerializer.GetDataSet(sDatosEnviados);
            gestorDatosCarga.GuardarBackupDescarga(dsGuardarDatos, sDatosEnviados);
            gestorDatosCarga.DescargaDatosPRAX2000(sDatosEnviados, wsConfiguraciones.BackupTalonarios == "S", dsGuardarDatos);                 
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
    #endregion
    
}

