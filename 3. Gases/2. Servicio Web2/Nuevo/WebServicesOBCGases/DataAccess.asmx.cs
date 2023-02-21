using Datascan.Praxair.UtilidadGestorDatos;
using Datascan.TextSerializer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebServicesOBCGases.Helpers;

namespace WebServicesOBCGases
{
    /// <summary>
    /// Descripción breve de DataAccess
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class DataAccess : System.Web.Services.WebService
    {

        public DataAccess()
        {

            //Eliminar la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
            //InitializeComponent(); 
        }

        #region "Implementación de Metodos publicados WEB"

        /*[WebMethod()]
        public string prueba()
        {
            return LeerConfiguracionIni.GetKeyConfiguracionOxigenos("NmbBD");
        }*/

        [WebMethod()]
        public string GetFechaSistema()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA ENVIAR ARCHIVOS A LA TERMINAL
        // PARAMETROS:   sFileName = NOMBRE DE ARCHIVO DE PETICIN
        //               sFileType = TIPO DE ARCHIVO DE LA PETICION (DEFAULT XML)
        //               Param1 = PARAMETRO OPCIONAL UNO (CONFIGURACIN SI SE NECESITA EN LA APLICACIN DE LA TERMINAL)
        //               Param2 = PARAMETRO OPCIONAL DOS (CONFIGURACIN SI SE NECESITA EN LA APLICACIN DE LA TERMINAL)
        //RETORNO: DEVUELCE UNA CADENA DE CARACTERES QUE CONTIENE LA ESTRUCTURA DE UN ARCHIVO XML
        //         CON TODOS LOS DATOS QUE REQUIERE LA TERMINAL
        [WebMethod()]
        public string GetDatosCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma)
        {
            /*SOLO PARA PRUEBAS*/
            StreamReader fe = new StreamReader("D:\\OBC\\Carga.txt");
            string sRespuestaTemp = fe.ReadToEnd();
            fe.Close();
            return sRespuestaTemp;
            string sFileLog = " ";
            //MIEMBRO PARA ALMACENAR EL NOMBRE DE LAS TABLAS QUE SE DEBEN ENVIAR A LA TERMINAL
            DataTable dtTablasConsultar;
            DataSet dsEnvioTerminal = null;
            OracleConnection ConexionOracle = null;
            OracleTransaction TransaccionOracle = null;
            WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
            ProcesamientoQueryOracle dtOracle = new ProcesamientoQueryOracle();
            //VARIABLES PARA CONTROLAR EL TIMEOUT
            bool bEjecutarProcedimientoAlmacenado = false;
            int iNumeroVecesObtener;
            EstadoTerminal iEstadoCargaTerminal;
            try
            {
                iEstadoCargaTerminal = this.ExisteRegistroTemporalCarga(sTerminal);
                if (iEstadoCargaTerminal == EstadoTerminal.NoExisteTerminal)
                {
                    bEjecutarProcedimientoAlmacenado = true;
                }
                else
                {
                    if (iEstadoCargaTerminal == EstadoTerminal.ErrorProceso)
                    {
                        bEjecutarProcedimientoAlmacenado = true;
                    }
                    else
                    {
                        if (iEstadoCargaTerminal == EstadoTerminal.ProcesoTerminado)
                        {
                            bEjecutarProcedimientoAlmacenado = false;
                        }
                        else
                        {
                            iNumeroVecesObtener = 0;
                            while (iNumeroVecesObtener <= 600)
                            {
                                System.Threading.Thread.Sleep(1000);
                                if (this.ExisteRegistroTemporalCarga(sTerminal) == EstadoTerminal.ProcesoTerminado)
                                {
                                    bEjecutarProcedimientoAlmacenado = false;
                                    iNumeroVecesObtener = 601;
                                }
                                else
                                {
                                    iNumeroVecesObtener++;
                                }
                            }
                        }
                    }
                }
                //CONECTAR CON EL SERVIDOR ORACLE
                //TODO: CONFIGURACION PAGINA WEB)
                string sUsuario = wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle); //"ExpePrax";
                string sContrasena = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle); //"expecoox";
                string sBaseDatos = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle);//"OXIGENOS";

                dtOracle.PathGuardarErrores = Context.Request.PhysicalApplicationPath;
                dtOracle.Conectar(ref ConexionOracle, sUsuario, sContrasena, sBaseDatos, ref TransaccionOracle);



                //LANZAR UNA EXCEPTION INDICANDO QUE EL PROGRAMA ESTA ERRADO
                if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
                {
                    throw new Exception("Código Programa Errado");
                }

                DataTable dtDatosConfiguracion = ValidarTerminal(sTerminal, ConexionOracle, TransaccionOracle, dtOracle);
                string sCodigoSucursal = "";
                string sCodigoTerminal = "";
                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("Terminal No Registrada");
                }
                else
                {
                    sCodigoSucursal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CodigoSucursal"]);
                    sCodigoTerminal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CODIGOTERMINAL"]);
                }


                //CREAR UN OBJETO PARA LOS PARAMETROS
                ParametrosEjecucion prEjecucion = new ParametrosEjecucion();
                prEjecucion.CodigoTerminal = sCodigoTerminal;
                prEjecucion.CodigoProceso = iCodigoProceso;
                prEjecucion.CodigoPrograma = iCodigoPrograma;
                prEjecucion.CodigoSucursal = sCodigoSucursal;
                prEjecucion.CodigoEmpresa = wsConfiguraciones.CodigoEmpresa;
                prEjecucion.CodigoGrupo = wsConfiguraciones.CodigoGrupo;


                //OBTENER NOMBRE DEL PROCEDIMIENTO ALMACENADO PARA EJEJCUTAR
                dtDatosConfiguracion = this.ValidarProcedimiento(iCodigoPrograma, iCodigoProceso, ConexionOracle, TransaccionOracle, dtOracle);

                string sSentenciaSQL = "";
                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("NO Existe parametrización Proceso Carga");
                }
                else
                {
                    sSentenciaSQL = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOINICIO"]);
                }

                //OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
                if (iCodigoProceso != 1)
                {
                    dtDatosConfiguracion = this.ObtenerPuntoVentaRutaPrincipal(prEjecucion, ConexionOracle, TransaccionOracle, dtOracle);
                    if (dtDatosConfiguracion.Rows.Count == 0)
                    {
                        //LANZAR UNA EXCEPTION INDICANDO QUE NO EXISTE LA RUTA PRINCIPAL
                        throw new Exception("No existe la ruta principal!!");

                    }
                    else
                    {
                        prEjecucion.RutaPrincipal = Convert.ToString(dtDatosConfiguracion.Rows[0]["RUTAPRINCIPAL"]);
                        prEjecucion.PuntoVenta = Convert.ToString(dtDatosConfiguracion.Rows[0]["PUNTOVENTA"]);
                    }
                }

                //TEMPORAL: GUARDAR DATOS EN EL LOG ERRORES
                dtOracle.GuardarErrores("T:" + prEjecucion.CodigoTerminal + " C:" + prEjecucion.CodigoProceso + " P:" + prEjecucion.CodigoPrograma + " S:" + prEjecucion.CodigoSucursal + " E:" + prEjecucion.CodigoEmpresa + " G:" + prEjecucion.CodigoGrupo + " V:" + prEjecucion.PuntoVenta + " R:" + prEjecucion.RutaPrincipal);

                //EJECUTAR EL PROCEDIMIENTO ALMACENADO
                OracleParameter[] prEjecucionOracle;
                if (bEjecutarProcedimientoAlmacenado)
                {
                    try
                    {
                        this.GuardarRegistroTemporalCarga(sTerminal, EstadoTerminal.EjecutandoProceso);
                        dtOracle.IniciarTransaccion(ref TransaccionOracle, ConexionOracle);
                        prEjecucionOracle = dtOracle.ObtenerParametrosSentencia(ref sSentenciaSQL, prEjecucion);
                        dtOracle.EjecutarQueryParametros("BEGIN " + sSentenciaSQL + "; END;", prEjecucionOracle, ConexionOracle, TransaccionOracle);
                        dtOracle.CommitTransaccion(ref TransaccionOracle);
                        this.GuardarRegistroTemporalCarga(sTerminal, EstadoTerminal.ProcesoTerminado);
                    }
                    catch (Exception ex)
                    {
                        this.GuardarRegistroTemporalCarga(sTerminal, EstadoTerminal.ErrorProceso);
                        dtOracle.RollbackTransaccion(ref TransaccionOracle);
                        throw new Exception("Error ejecutando procedimiento de inicio " + ex.Message);
                    }
                }

                //SI EL PROCESO ES 1 AHIRA SI CONSULTAR PARAMETROS
                //OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
                if (iCodigoProceso == 1)
                {
                    dtDatosConfiguracion = this.ObtenerPuntoVentaRutaPrincipal(prEjecucion, ConexionOracle, TransaccionOracle, dtOracle);
                    if (dtDatosConfiguracion.Rows.Count == 0)
                    {
                        //LANZAR UNA EXCEPTION INDICANDO QUE NO EXISTE LA RUTA PRINCIPAL
                        throw new Exception("No existe la datos de rutas!!");

                    }
                    else
                    {
                        prEjecucion.RutaPrincipal = Convert.ToString(dtDatosConfiguracion.Rows[0]["RUTAPRINCIPAL"]);
                        prEjecucion.PuntoVenta = Convert.ToString(dtDatosConfiguracion.Rows[0]["PUNTOVENTA"]);
                    }
                }

                //OBTENER LAS TABLAS A REVISAR
                dtTablasConsultar = ObtenerTablasConsultar(iCodigoPrograma, iCodigoProceso, ConexionOracle, TransaccionOracle, dtOracle);
                //CREAR LA ESTRUCTURA DEL DATASET DE RESPUESTA
                dsEnvioTerminal = LlenarDatasetDescarga(iCodigoProceso, iCodigoPrograma, dtTablasConsultar, ConexionOracle, TransaccionOracle, prEjecucion, dtOracle);
                //REVISAR SI EXISTEN DATOS DE RESPUESTA
                if (dsEnvioTerminal != null)
                {
                    //GUARDAR LOS DATOS DE RESPUESTA EN UNA VARIABLE STRING
                    StringWriter fs = new StringWriter();
                    dsEnvioTerminal.WriteXml(fs);
                    string sRespuesta = fs.ToString();
                    fs.Close();
                    //GUARDAR LOS DATOS DE RESPUESTA EN UN ARCHIVO DE LOG
                    string sFileAux;
                    sFileAux = "Proceso";
                    sFileAux = sFileAux + DateTime.Now.ToString("ddMMyyyyhhmmss");
                    sFileLog = dtOracle.GuardarArchivosXML(sRespuesta, "BK-C-" + sFileAux + ".xml", false);
                    //ENVIAR EL STRING CON LOS DATOS REQUERIDOS
                    //SOLO PRUEBAS
                    /*dsEnvioTerminal.Clear();
                    dsEnvioTerminal.ReadXml("D:\\Pruebas OXI\\BK.XML");*/
                    sRespuesta = TextSerializer.Serialize(dsEnvioTerminal);
                    sFileLog = dtOracle.GuardarArchivosXML(sRespuesta, "BK-C-" + sFileAux + ".txt", false);
                    /*SOLO PARA PRUEBAS*/
                    /*StreamReader fe = new StreamReader("D:\\Pruebas OXI\\Carga.txt");
                    sRespuesta = fe.ReadToEnd();
                    fe.Close();*/
                    OracleParameter[] prParametrosOracle = null;
                    if (prEjecucion.RutaPrincipal == "" || prEjecucion.RutaPrincipal == null)
                    {
                        prEjecucion.RutaPrincipal = Convert.ToString(dsEnvioTerminal.Tables["Parametros"].Rows[0]["RutaPrincipal"]);
                    }
                    prParametrosOracle = this.InsertarRegistroResultadoComunicacion(ref sSentenciaSQL, prEjecucion, DateTime.Now, sRespuesta, "V", this.ObtenerMaximoIdProceso(prEjecucion.CodigoTerminal, ConexionOracle, TransaccionOracle, dtOracle));
                    dtOracle.EjecutarQueryParametros(sSentenciaSQL, prParametrosOracle, ConexionOracle, TransaccionOracle);

                    //byte[] btData = DataCompression.Compress(sRespuesta).ToArray();
                    //return Convert.ToBase64String(btData);
                    return sRespuesta;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                //SI OCURRIO ALGUN ERROR ENVIAR LA EXCEPCION
                dtOracle.GuardarErrores(ex.Message);
                throw (ex);
            }
            finally
            {
                dtOracle.Desconectar(ref ConexionOracle);
            }
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA GUARDAR UN REGISTRO EN LA TABLA TEMPORAL DE CARGAS
        // PARAMETROS:   sNumeroTerminal = NUMERO DE TERMINAL
        public void GuardarRegistroTemporalCarga(string sTerminal, EstadoTerminal iEstadoTerminal)
        {
            if (this.ExisteRegistroTemporalCarga(sTerminal) != EstadoTerminal.NoExisteTerminal)
            {
                this.Application.Remove(sTerminal);
            }
            this.Application.Add(sTerminal, iEstadoTerminal);
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA REVISAR SI EXISTE EL OBJETO EN LA COLECCIÓN
        // PARAMETROS:   sNumeroTerminal = NUMERO DE TERMINAL
        public EstadoTerminal ExisteRegistroTemporalCarga(string sTerminal)
        {
            Object objBusqueda = this.Application.Get(sTerminal);
            if (objBusqueda != null)
            {
                return (EstadoTerminal)objBusqueda;
            }
            else
            {
                return EstadoTerminal.NoExisteTerminal;
            }
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA ELIMINAR EL REGISTRO DE LA TERMINAL
        // PARAMETROS:   sNumeroTerminal = NUMERO DE TERMINAL
        public void ElimnarRegistroTemporalCarga(string sTerminal)
        {
            this.Application.Remove(sTerminal);
        }


        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA ENVIAR ARCHIVOS A LA TERMINAL
        // PARAMETROS:   sFileName = NOMBRE DE ARCHIVO DE PETICIN
        //               sFileType = TIPO DE ARCHIVO DE LA PETICION (DEFAULT XML)
        //               Param1 = PARAMETRO OPCIONAL UNO (CONFIGURACIN SI SE NECESITA EN LA APLICACIN DE LA TERMINAL)
        //               Param2 = PARAMETRO OPCIONAL DOS (CONFIGURACIN SI SE NECESITA EN LA APLICACIN DE LA TERMINAL)
        //RETORNO: DEVUELCE UNA CADENA DE CARACTERES QUE CONTIENE LA ESTRUCTURA DE UN ARCHIVO XML
        //         CON TODOS LOS DATOS QUE REQUIERE LA TERMINAL
        [WebMethod()]
        public void ConfirmarCarga(string sTerminal, int iCodigoProceso, int iCodigoPrograma)
        {
            //MIEMBRO PARA ALMACENAR EL NOMBRE DE LAS TABLAS QUE SE DEBEN ENVIAR A LA TERMINAL
            OracleConnection ConexionOracle = null;
            OracleTransaction TransaccionOracle = null;
            WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
            ProcesamientoQueryOracle dtOracle = new ProcesamientoQueryOracle();
            try
            {
                //CONECTAR CON EL SERVIDOR ORACLE
                //TODO: VALIDAR DE DONDE SALEN LOS DATOS (ARCHIVO CONFIGURACION)
                string sUsuario = wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle); //"ExpePrax";
                string sContrasena = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle); //"expecoox";
                string sBaseDatos = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle);//"OXIGENOS";
                dtOracle.Conectar(ref ConexionOracle, sUsuario, sContrasena, sBaseDatos, ref TransaccionOracle);

                dtOracle.PathGuardarErrores = Context.Request.PhysicalApplicationPath;

                //LANZAR UNA EXCEPTION INDICANDO QUE EL PROGRAMA ESTA ERRADO
                if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
                {
                    throw new Exception("Código Programa Errado");
                }

                DataTable dtDatosConfiguracion = ValidarTerminal(sTerminal, ConexionOracle, TransaccionOracle, dtOracle);
                string sCodigoSucursal = "";
                string sCodigoTerminal = "";
                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("Terminal No Registrada");
                }
                else
                {
                    sCodigoSucursal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CodigoSucursal"]);
                    sCodigoTerminal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CODIGOTERMINAL"]);
                }


                //CREAR UN OBJETO PARA LOS PARAMETROS
                ParametrosEjecucion prEjecucion = new ParametrosEjecucion();
                prEjecucion.CodigoTerminal = sCodigoTerminal;
                prEjecucion.CodigoProceso = iCodigoProceso;
                prEjecucion.CodigoPrograma = iCodigoPrograma;
                prEjecucion.CodigoSucursal = sCodigoSucursal;
                prEjecucion.CodigoEmpresa = wsConfiguraciones.CodigoEmpresa;
                prEjecucion.CodigoGrupo = wsConfiguraciones.CodigoGrupo;


                //OBTENER NOMBRE DEL PROCEDIMIENTO ALMACENADO PARA EJEJCUTAR
                dtDatosConfiguracion = this.ValidarProcedimiento(iCodigoPrograma, iCodigoProceso, ConexionOracle, TransaccionOracle, dtOracle);

                string sSentenciaSQL = "";
                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("NO Existe el procedimiento de cierre");
                }
                else
                {
                    sSentenciaSQL = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOCIERRE"]);
                }

                //OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
                dtDatosConfiguracion = this.ObtenerPuntoVentaRutaPrincipal(prEjecucion, ConexionOracle, TransaccionOracle, dtOracle);

                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("No existe la ruta principal!!");
                }
                else
                {
                    prEjecucion.RutaPrincipal = Convert.ToString(dtDatosConfiguracion.Rows[0]["RUTAPRINCIPAL"]);
                    prEjecucion.PuntoVenta = Convert.ToString(dtDatosConfiguracion.Rows[0]["PUNTOVENTA"]);
                }

                //EJECUTAR EL PROCEDIMIENTO ALMACENADO
                OracleParameter[] prEjecucionOracle;
                try
                {
                    dtOracle.IniciarTransaccion(ref TransaccionOracle, ConexionOracle);
                    prEjecucionOracle = dtOracle.ObtenerParametrosSentencia(ref sSentenciaSQL, prEjecucion);
                    dtOracle.EjecutarQueryParametros("BEGIN " + sSentenciaSQL + "; END;", prEjecucionOracle, ConexionOracle, TransaccionOracle);
                    OracleParameter[] prParametrosOracle;
                    prParametrosOracle = this.ActualizarRegistroResultadoComunicacion(ref sSentenciaSQL, prEjecucion, DateTime.Now, "C", "", ConexionOracle, TransaccionOracle, dtOracle);
                    dtOracle.EjecutarQueryParametros(sSentenciaSQL, prParametrosOracle, ConexionOracle, TransaccionOracle);
                    dtOracle.CommitTransaccion(ref TransaccionOracle);
                }
                catch (Exception ex)
                {
                    dtOracle.RollbackTransaccion(ref TransaccionOracle);
                    throw new Exception("Error ejecutando procedimiento de cierre " + ex.Message);
                }
                this.ElimnarRegistroTemporalCarga(sTerminal);
            }
            catch (Exception ex)
            {
                //SI OCURRIO ALGUN ERROR ENVIAR LA EXCEPCION
                dtOracle.GuardarErrores(ex.Message);
                throw (ex);
            }
            finally
            {
                dtOracle.Desconectar(ref ConexionOracle);
            }
        }


        [WebMethod()]
        public string DeserializarDatos(string pathArchivo)
        {
            if (File.Exists(pathArchivo))
            {
                StreamReader reader = new StreamReader(pathArchivo);
                string sDatosEnviados = reader.ReadToEnd();
                reader.Close();
                
                DataSet dsGuardarDatos = TextSerializer.GetDataSet(sDatosEnviados);
                StringWriter fs = new StringWriter();
                dsGuardarDatos.WriteXml(fs);
                string sRespuesta = fs.ToString();
                fs.Close();
                return sRespuesta;
            }
            else
            {
                return "No existe el archivo";
            }

        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA PROCESAR LOS DATOS ENVIADOS DESDE LA TERMINAL
        // PARAMETROS:   sFileName = NOMBRE DE ARCHIVO ENVIADO
        //               sFileContent = CONTENIDO DE LOS DATOS ENVIADOS EN FORMATO CML EN UNA CADENA DE CARACTERES
        //               Param1 = PARAMETRO OPCIONAL UNO (CONFIGURACIN SI SE NECESITA EN LA APLICACIN DE LA TERMINAL)
        //               Param2 = PARAMETRO OPCIONAL DOS (CONFIGURACIN SI SE NECESITA EN LA APLICACIN DE LA TERMINAL)
        [WebMethod()]
        public void SendDatos(string sTerminal, int iCodigoPrograma, int iCodigoProceso, string sDatosEnviados)
        {
            string sFileLog = " ";
            OracleConnection ConexionOracle = null;
            OracleTransaction TransaccionOracle = null;
            WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
            ProcesamientoQueryOracle dtOracle = new ProcesamientoQueryOracle();
            try
            {
                //PREUEBAS
                /*string sFileAux2;
                sFileAux2 = sTerminal + DateTime.Now.ToString("ddMMyyyyhhmmss");
                //VOLVER DATASET LO QUE SE ENVIA
                dtOracle.PathGuardarErrores = Context.Request.PhysicalApplicationPath;
                dtOracle.GuardarArchivosXML(sDatosEnviados, "BK-D-" + sFileAux2 + ".txt", true);

                DataSet dsGuardarDatos2 = TextSerializer.GetDataSet(sDatosEnviados);*/

                //char ant = Convert.ToChar(10);
                //char act = Convert.ToChar(13);
                //string sCam = ant.ToString() + act;
                //sDatosEnviados = sDatosEnviados.Replace(ant.ToString(), sCam);
                //fTemp.Close();

                //LANZAR UNA EXCEPTION INDICANDO QUE EL PROGRAMA ESTA ERRADO
                //CONECTAR AL SERVIDOR
                //TODO: VALIDAR DE DONDE SALEN LOS DATOS (ARCHIVO CONFIGURACION)
                string sUsuario = wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle); //"ExpePrax";
                string sContrasena = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle); //"expecoox";
                string sBaseDatos = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle);//"OXIGENOS";
                
                dtOracle.Conectar(ref ConexionOracle, sUsuario, sContrasena, sBaseDatos, ref TransaccionOracle);

                dtOracle.PathGuardarErrores = Context.Request.PhysicalApplicationPath;
                if (wsConfiguraciones.CodigoPrograma != iCodigoPrograma)
                {
                    throw new Exception("Código Programa Errado");
                }
                
                DataTable dtDatosConfiguracion = ValidarTerminal(sTerminal, ConexionOracle, TransaccionOracle, dtOracle);
                string sCodigoSucursal = "";
                string sCodigoTerminal = "";
                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("Terminal No Registrada");
                }
                else
                {
                    sCodigoSucursal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CodigoSucursal"]);
                    sCodigoTerminal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CODIGOTERMINAL"]);
                }

                //OBTENER NOMBRE DEL PROCEDIMIENTO ALMACENADO PARA EJEJCUTAR
                dtDatosConfiguracion = this.ValidarProcedimiento(iCodigoPrograma, iCodigoProceso, ConexionOracle, TransaccionOracle, dtOracle);

                string sProcesoDescarga = null;
                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("NO Existe parametrización Proceso Descarga");
                }
                else
                {
                    if (Convert.ToInt16(dtDatosConfiguracion.Rows[0]["EJECUTARINMEDIATO"]) == 1)
                    {
                        sProcesoDescarga = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOINICIO"]);
                    }
                    //SI ES DESCARGA EJECUTAR TALONARIOS PROCESO 2
                    else
                    {
                        if (iCodigoProceso == 2)
                        {
                            sProcesoDescarga = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOCIERRE"]);
                        }
                    }
                }

                //CREAR UN OBJETO PARA LOS PARAMETROS
                ParametrosEjecucion prEjecucion = new ParametrosEjecucion();
                prEjecucion.CodigoTerminal = sCodigoTerminal;
                prEjecucion.CodigoProceso = iCodigoProceso;
                prEjecucion.CodigoPrograma = iCodigoPrograma;
                prEjecucion.CodigoSucursal = sCodigoSucursal;
                prEjecucion.CodigoEmpresa = wsConfiguraciones.CodigoEmpresa;
                prEjecucion.CodigoGrupo = wsConfiguraciones.CodigoGrupo;

                //OBTENER DATOS PUNTO VENTA - RUTAPRINCIPAL
                dtDatosConfiguracion = this.ObtenerPuntoVentaRutaPrincipal(prEjecucion, ConexionOracle, TransaccionOracle, dtOracle);

                if (dtDatosConfiguracion.Rows.Count == 0)
                {
                    //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
                    throw new Exception("No existe la ruta principal!!");
                }
                else
                {
                    prEjecucion.RutaPrincipal = Convert.ToString(dtDatosConfiguracion.Rows[0]["RUTAPRINCIPAL"]);
                    prEjecucion.PuntoVenta = Convert.ToString(dtDatosConfiguracion.Rows[0]["PUNTOVENTA"]);
                }

                try
                {
                    dtOracle.IniciarTransaccion(ref TransaccionOracle, ConexionOracle);
                    string sSentenciaSQL = "";
                    OracleParameter[] prParametrosOracle = null;
                    //NOMBRE DEL ARCHIVO DE BACKUP
                    string sFileAux;
                    sFileAux = sTerminal + DateTime.Now.ToString("ddMMyyyyhhmmss");
                    //VOLVER DATASET LO QUE SE ENVIA
                    dtOracle.GuardarArchivosXML(sDatosEnviados, "BK-D-" + sFileAux + ".txt", true);
                    DataSet dsGuardarDatos = TextSerializer.GetDataSet(sDatosEnviados);
                    StringWriter fs = new StringWriter();
                    dsGuardarDatos.WriteXml(fs);
                    string sRespuesta = fs.ToString();
                    fs.Close();

                    //GUARDAR LOS DATOS ENVIADOS EN UN ARCHIVO DE BACKUP
                    sFileLog = dtOracle.GuardarArchivosXML(sRespuesta, "BK-D-" + sFileAux + ".xml", true);

                    DateTime dtFechaProceso = DateTime.Now;
                    prParametrosOracle = this.InsertarRegistroResultadoComunicacion(ref sSentenciaSQL, prEjecucion, dtFechaProceso, sDatosEnviados, "G", this.ObtenerMaximoIdProceso(prEjecucion.CodigoTerminal, ConexionOracle, TransaccionOracle, dtOracle));
                    dtOracle.EjecutarQueryParametros(sSentenciaSQL, prParametrosOracle, ConexionOracle, TransaccionOracle);

                    dtOracle.CommitTransaccion(ref TransaccionOracle);

                    /*ACTUALIZAR TALONARIOS*/
                    if (iCodigoProceso == 2 && sProcesoDescarga != null)
                    {
                        try
                        {
                            DataSet dsTalonarios = new DataSet("Talonarios");
                            DataTable dtAux;
                            if (dsGuardarDatos.Tables.Contains("FACTURASMANUALES"))
                            {
                                dtAux = dsGuardarDatos.Tables["FACTURASMANUALES"];
                                dsGuardarDatos.Tables.Remove("FACTURASMANUALES");
                                dsTalonarios.Tables.Add(dtAux);
                            }
                            if (dsGuardarDatos.Tables.Contains("MAESTROFACTURAS"))
                            {
                                dtAux = dsGuardarDatos.Tables["MAESTROFACTURAS"];
                                dsGuardarDatos.Tables.Remove("MAESTROFACTURAS");
                                dsTalonarios.Tables.Add(dtAux);
                            }
                            if (dsGuardarDatos.Tables.Contains("DETALLEGUIAASIGREC"))
                            {
                                dtAux = dsGuardarDatos.Tables["DETALLEGUIAASIGREC"];
                                dsGuardarDatos.Tables.Remove("DETALLEGUIAASIGREC");
                                dsTalonarios.Tables.Add(dtAux);
                            }
                            if (dsGuardarDatos.Tables.Contains("TALONARIOSDESCARGA"))
                            {
                                dtAux = dsGuardarDatos.Tables["TALONARIOSDESCARGA"];
                                dsGuardarDatos.Tables.Remove("TALONARIOSDESCARGA");
                                dsTalonarios.Tables.Add(dtAux);
                            }

                            /*INSERTAR LOS DATOS*/
                            dtOracle.IniciarTransaccion(ref TransaccionOracle, ConexionOracle);
                            dtOracle.EjecutarDescargaTalonarios(prEjecucion, "User Id=" + sUsuario + ";Password=" + sContrasena + ";Data Source=" + sBaseDatos, dsTalonarios, sProcesoDescarga);
                            dtOracle.CommitTransaccion(ref TransaccionOracle);
                        }
                        catch (Exception ex)
                        {
                            dtOracle.RollbackTransaccion(ref TransaccionOracle);
                            prParametrosOracle = this.ActualizarRegistroResultadoComunicacion(ref sProcesoDescarga, prEjecucion, dtFechaProceso, "E", "Error ejecutando procedimiento almacenado: " + ex.Message, ConexionOracle, TransaccionOracle, dtOracle);
                            dtOracle.EjecutarQueryParametros(sProcesoDescarga, prParametrosOracle, ConexionOracle, TransaccionOracle);
                            throw ex;
                        }
                    }
                    else
                    {
                        /*REVISAR SI SE EJECUTA DE INMEDIATO EL PROCEDIMIENTO ALMACENADO (IMPLICA PROCESAR DATOS)*/
                        if (sProcesoDescarga != null)
                        {
                            try
                            {
                                /*INSERTAR LOS DATOS*/
                                dtOracle.IniciarTransaccion(ref TransaccionOracle, ConexionOracle);
                                dtOracle.ProcesarDataSetDescarga(dsGuardarDatos, ConexionOracle, prEjecucion, TransaccionOracle);
                                prParametrosOracle = dtOracle.ObtenerParametrosSentencia(ref sProcesoDescarga, prEjecucion);
                                dtOracle.EjecutarQueryParametros("BEGIN " + sProcesoDescarga + "; END;", prParametrosOracle, ConexionOracle, TransaccionOracle);
                                prParametrosOracle = this.ActualizarRegistroResultadoComunicacion(ref sProcesoDescarga, prEjecucion, dtFechaProceso, "A", "", ConexionOracle, TransaccionOracle, dtOracle);
                                dtOracle.EjecutarQueryParametros(sProcesoDescarga, prParametrosOracle, ConexionOracle, TransaccionOracle);
                                dtOracle.CommitTransaccion(ref TransaccionOracle);
                            }
                            catch (Exception ex)
                            {
                                dtOracle.RollbackTransaccion(ref TransaccionOracle);
                                prParametrosOracle = this.ActualizarRegistroResultadoComunicacion(ref sProcesoDescarga, prEjecucion, dtFechaProceso, "E", "Error ejecutando procedimiento almacenado: " + ex.Message, ConexionOracle, TransaccionOracle, dtOracle);
                                dtOracle.EjecutarQueryParametros(sProcesoDescarga, prParametrosOracle, ConexionOracle, TransaccionOracle);
                                //NO LANZAR LANZAR EXCEPCION
                                //throw new Exception();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    dtOracle.RollbackTransaccion(ref TransaccionOracle);
                    throw new Exception("Error procesando descarga: " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                //SI OCURRIO ALGUN ERROR ENVIAR LA EXCEPCION
                dtOracle.GuardarErrores(ex.Message);
                throw (ex);
            }
            finally
            {
                dtOracle.Desconectar(ref ConexionOracle);
            }
        }

        [WebMethod]
        public string GuardarLogDescargaArchivo(string nombreArchivo, string terminal)
        {
            OracleConnection ConexionOracle = null;
            OracleTransaction TransaccionOracle = null;
            WebAplicationSettings wsConfiguraciones = new WebAplicationSettings();
            ProcesamientoQueryOracle dtOracle = new ProcesamientoQueryOracle();

            try
            {
                string sUsuario = wsConfiguraciones.DesEncriptar(wsConfiguraciones.UsuarioOracle); //"ExpePrax";
                string sContrasena = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ContrasenaOracle); //"expecoox";
                string sBaseDatos = wsConfiguraciones.DesEncriptar(wsConfiguraciones.ConexionOracle);//"OXIGENOS";

                dtOracle.PathGuardarErrores = Context.Request.PhysicalApplicationPath;
                dtOracle.Conectar(ref ConexionOracle, sUsuario, sContrasena, sBaseDatos, ref TransaccionOracle);

                string sqlInsertar = System.String.Format("INSERT INTO DTSC_LOG_DESCARGA_PDF(Terminal,Archivo) VALUES('{0}','{1}')", terminal, nombreArchivo);

                dtOracle.EjecutarTablaSinParametros(sqlInsertar, ConexionOracle, TransaccionOracle);
                return "OK Guardar LOG";
            }
            catch (Exception ex)
            {
                Datascan.Logging.Logger.Write(ex);
                return "Error guardando Log";
            }
        }
        [WebMethod]
        public string ObtenerPathGuardarPDF()
        {
            string pathArchivoPDF = "";

            if (System.Configuration.ConfigurationManager.AppSettings["PathAchivoPDF"] != null)
            {
                pathArchivoPDF = System.Configuration.ConfigurationManager.AppSettings["PathAchivoPDF"];
            }
            else
            {
                pathArchivoPDF = "No esta configurado";
            }

            return pathArchivoPDF;
        }

        /// <summary>
        /// Metodo para guardar archivo PDF en el servidor
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>    
        /// <param name="bytes">Contenido Archivo</param>
        [WebMethod]
        public bool UploadArchivoPDF(string nombreArchivo, byte[] bytes, string terminal)
        {
            try
            {
                //Save the Byte Array as File.

                string pathArchivoPDF = "";

                if (System.Configuration.ConfigurationManager.AppSettings["PathAchivoPDF"] != null)
                {
                    pathArchivoPDF = System.Configuration.ConfigurationManager.AppSettings["PathAchivoPDF"];
                }
                else
                {
                    pathArchivoPDF = "C:\\Uploads";
                }

                if (!Directory.Exists(pathArchivoPDF))
                {
                    Directory.CreateDirectory(pathArchivoPDF);
                }
                                
                string filePath = "";
                Int16 i = 1;
                nombreArchivo = Path.GetFileName(nombreArchivo);
                string nombreOriginal = Path.GetFileNameWithoutExtension(nombreArchivo);
                while (true)
                {
                    filePath = Path.Combine(pathArchivoPDF, nombreArchivo);
                    if (!File.Exists(filePath))
                    {
                        break;
                    }
                    nombreArchivo = nombreOriginal + "(" + i.ToString() + ").pdf";
                    i++;
                }
                File.WriteAllBytes(filePath, bytes);
                GuardarLogDescargaArchivo(nombreArchivo, terminal);
                return true;
            }
            catch (Exception ex)
            {
                Datascan.Logging.Logger.Write(ex);
                return false;
            }
        }


        #endregion

        #region "Implementación de Metodos"

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA EL MANEJO DE ERRORES, REGISTRA EL ERROR EN EL MANEJADOR DE SUCESOS DEL SISTEMA OPERATIVO
        // PARAMETROS:   EX = EXECPCION LANZADA POR LA APLICACIN
        private void HandleException(Exception ex)
        {
            //LogErrores.WriteEntry("Ha ocurrido un error en la aplicacin: " & vbCrLf &         'ex.Message & vbCrLf & "Stack Trace: " & vbCrLf & ex.StackTrace)
            //Throw ex
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA OBTENER UN LISTADO DE LAS TABLAS A LAS QUE SE LES DEBE HACER UNA CONSULTA
        // PARAMETROS:   sCodigoProceso = NUMERO DE PROCESO A REALIZAR
        //               sCodigoSubProceso = NUMERO DE SUBPROCESO A REALIZAR
        //               sMaestra =  INDICA SI LA TABLA HACE PARTE DE LA CARGA DE MAESTROS
        // RETORNO: UN DATATABLE CON EL NOMBRE DE LAS TABLAS A CONSULTAR
        private DataTable ValidarTerminal(string sTerminal, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ProcesamientoQueryOracle dtOracle)
        {
            string sSenteciaSQL;
            //OBTENER TABLAS A CONSULTAR
            sSenteciaSQL = " SELECT CODIGOTERMINAL, DESCRIPCION, CODIGOSUCURSAL " +
                           " FROM WS_TERMINALESSYMBOL " +
                           " WHERE DESCRIPCION = '" + sTerminal + "'";
            return dtOracle.EjecutarTablaSinParametros(sSenteciaSQL, ConexionOracle, TransaccionOracle);
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA OBTENER UN LISTADO DE LAS TABLAS A LAS QUE SE LES DEBE HACER UNA CONSULTA
        // PARAMETROS:   sCodigoProceso = NUMERO DE PROCESO A REALIZAR
        //               sCodigoSubProceso = NUMERO DE SUBPROCESO A REALIZAR
        //               sMaestra =  INDICA SI LA TABLA HACE PARTE DE LA CARGA DE MAESTROS
        // RETORNO: UN DATATABLE CON EL NOMBRE DE LAS TABLAS A CONSULTAR
        private DataTable ValidarProcedimiento(int iCodigoPrograma, int iCodigoProcedimiento, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ProcesamientoQueryOracle dtOracle)
        {
            string sSenteciaSQL;
            //OBTENER TABLAS A CONSULTAR
            sSenteciaSQL = " SELECT PROCEDIMIENTOINICIO, PROCEDIMIENTOCIERRE, EJECUTARINMEDIATO, TIPOPROCESO " +
                           " FROM WS_PROCESOSCOMUNICACION " +
                           " WHERE CODIGOPROCESO = " + iCodigoProcedimiento.ToString() +
                           " AND CODIGOPROGRAMA = " + iCodigoPrograma.ToString();
            return dtOracle.EjecutarTablaSinParametros(sSenteciaSQL, ConexionOracle, TransaccionOracle);
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA OBTENER UN LISTADO DE LAS TABLAS A LAS QUE SE LES DEBE HACER UNA CONSULTA
        // PARAMETROS:   sCodigoProceso = NUMERO DE PROCESO A REALIZAR
        //               sCodigoSubProceso = NUMERO DE SUBPROCESO A REALIZAR
        //               sMaestra =  INDICA SI LA TABLA HACE PARTE DE LA CARGA DE MAESTROS
        // RETORNO: UN DATATABLE CON EL NOMBRE DE LAS TABLAS A CONSULTAR
        private DataTable ObtenerPuntoVentaRutaPrincipal(ParametrosEjecucion prParametros, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ProcesamientoQueryOracle dtOracle)
        {
            string sSenteciaSQL;
            //OBTENER TABLAS A CONSULTAR
            sSenteciaSQL = " SELECT RUTAPRINCIPAL, PUNTOVENTA " +
            " FROM DTSC_PARAMETROS_TERMINAL " +
            " WHERE CODIGOEMPRESA = '" + prParametros.CodigoEmpresa + "' AND " +
            " CODIGOGRUPO = '" + prParametros.CodigoGrupo + "' AND " +
            " CODIGOSUCURSAL = '" + prParametros.CodigoSucursal + "' AND " +
            " CODIGOTERMINAL = '" + prParametros.CodigoTerminal + "'";
            return dtOracle.EjecutarTablaSinParametros(sSenteciaSQL, ConexionOracle, TransaccionOracle);
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA CARGAR TODOS LOS DATOS AL DATASET DE RESPUESTA, EJECUTA UNA SENTENCIA SQL Y LA RESPUESTA LA ADICIONA AL DATASET
        // PARAMETROS:   bMaestro = INDICA SI LA CARGA QUE SE DEBE HACER ES DE MAESTROS
        private DataSet LlenarDatasetDescarga(int iCodigoProceso, int iCodigoPrograma, DataTable dtTablasConsultar, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ParametrosEjecucion prEjecucion, ProcesamientoQueryOracle dtOracle)
        {
            int i;
            string sSentenciaSQL;
            DataTable dtRespuesta;
            DataSet dsEnvioTerminal = null;
            OracleParameter[] prEjecucionOracle;

            if (dsEnvioTerminal == null)
            {
                //COLOCAR EL NOMBRE DEL DATASET
                dsEnvioTerminal = new DataSet("PRAXAIR OBC");
            }

            // HACE UN RECORRIDO POR LAS TABLAS QUE TIENE QUE CONSULTAR
            if (dtTablasConsultar != null)
            {
                for (i = 0; i <= dtTablasConsultar.Rows.Count - 1; i++)
                {
                    //CONSULTAR SENTANCIA SQL
                    sSentenciaSQL = Convert.ToString(dtTablasConsultar.Rows[i]["CONSULTASQL"]);
                    if (sSentenciaSQL != null)
                    {
                        if (sSentenciaSQL.Trim() != "")
                        {
                            prEjecucionOracle = dtOracle.ObtenerParametrosSentencia(ref sSentenciaSQL, prEjecucion);
                            dtRespuesta = dtOracle.EjecutarTablaParametros(sSentenciaSQL, prEjecucionOracle, ConexionOracle, TransaccionOracle);
                            //SI LA CONSULTA OBTUVO DATOS ADICIONA EL EL DATASET DE RESPUESTA
                            if (dtRespuesta != null)
                            {
                                dtRespuesta.TableName = Convert.ToString(dtTablasConsultar.Rows[i]["NOMBRETABLA"]);
                                dsEnvioTerminal.Tables.Add(dtRespuesta);
                            }
                        }
                    }
                }
            }
            return dsEnvioTerminal;
        }

        private DataTable ObtenerTablasConsultar(int iCodigoPrograma, int iCodigoProcedimiento, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ProcesamientoQueryOracle dtOracle)
        {
            string sSenteciaSQL;
            //OBTENER TABLAS A CONSULTAR
            sSenteciaSQL = " SELECT NOMBRETABLA, CONSULTASQL  " +
                           " FROM WS_PROCESOSTABLAS " +
                           " WHERE CODIGOPROCESO = " + iCodigoProcedimiento.ToString() +
                           " AND CODIGOPROGRAMA = " + iCodigoPrograma.ToString() +
                           " AND HABILITADA = 'S'" +
                           " ORDER BY ORDEN";
            Datascan.Logging.Logger.Write("sentencia sql tablas: " + sSenteciaSQL);
            return dtOracle.EjecutarTablaSinParametros(sSenteciaSQL, ConexionOracle, TransaccionOracle);
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA RETORNAR EL TIPO DE DATO QUE SE UTILIZA EN EL QUERY
        // PARAMETROS:   sTipoDato = TIPO DE DATO DEL PARAMETRO
        // RETORNO: EL TIPO DE DATO DE LA CLASE OracleClient.OracleType EQUIVALENTE
        private OracleParameter[] InsertarRegistroResultadoComunicacion(ref string sSentenciaSQL, ParametrosEjecucion prParametros, DateTime dtFechaProceso, string sDatosRecibidos, string sEstadoRuta, int IdCodigoProceso)
        {
            //CARGAR LA SENTENCIA SQL
            //REVISAR CUANTOS PARAMETROS TRAE LA SENTANCIA
            OracleParameter[] prResultado;
            sSentenciaSQL = "INSERT INTO WS_RESULTADOSCOMUNICACION (CODIGOPROGRAMA,CODIGOTERMINAL,CODIGORUTA,FECHAINSERCION,FECHAPROCESAMIENTO, " +
                             " ESTADOCOMUNICACION, OBSERVACION, DATOSENVIADOS, CODIGOPROCESO, IDPROCESOCOMUNICACION) VALUES (:0,:1,:2,:3,:4,:5,:6,:7,:8,:9)";

            prResultado = new OracleParameter[10];
            prResultado[0] = new OracleParameter("0", prParametros.CodigoPrograma);
            prResultado[1] = new OracleParameter("1", prParametros.CodigoTerminal);
            prResultado[2] = new OracleParameter("2", prParametros.RutaPrincipal);
            prResultado[3] = new OracleParameter("3", dtFechaProceso);
            prResultado[4] = new OracleParameter("4", System.DBNull.Value);
            /*EXISTEN TRES ESTADOS C=CARGADO, G=GUARDADO, P=PROCESADO*/
            prResultado[5] = new OracleParameter("5", sEstadoRuta);
            prResultado[6] = new OracleParameter("6", "");
            prResultado[7] = new OracleParameter("7", sDatosRecibidos);
            prResultado[8] = new OracleParameter("8", prParametros.CodigoProceso);
            prResultado[9] = new OracleParameter("9", IdCodigoProceso);

            return prResultado;
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA RETORNAR EL TIPO DE DATO QUE SE UTILIZA EN EL QUERY
        // PARAMETROS:   sTipoDato = TIPO DE DATO DEL PARAMETRO
        // RETORNO: EL TIPO DE DATO DE LA CLASE OracleClient.OracleType EQUIVALENTE
        private OracleParameter[] ActualizarRegistroResultadoComunicacion(ref string sSentenciaSQL, ParametrosEjecucion prParametros, DateTime dtFechaProceso, string sEstadoRuta, string sObservacion, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ProcesamientoQueryOracle dtOracle)
        {
            //CARGAR LA SENTENCIA SQL
            //REVISAR CUANTOS PARAMETROS TRAE LA SENTANCIA
            OracleParameter[] prResultado;
            sSentenciaSQL = "UPDATE WS_RESULTADOSCOMUNICACION SET ESTADOCOMUNICACION = :0, OBSERVACION = :1, FECHAPROCESAMIENTO = :2 " +
                            " WHERE IDPROCESOCOMUNICACION = :3 AND CODIGOTERMINAL = :4";

            prResultado = new OracleParameter[5];
            prResultado[0] = new OracleParameter("0", sEstadoRuta);
            prResultado[1] = new OracleParameter("1", sObservacion);
            prResultado[2] = new OracleParameter("2", dtFechaProceso);
            prResultado[3] = new OracleParameter("3", this.ObtenerIdProcesoActualizar(prParametros.CodigoTerminal, prParametros.CodigoProceso, ConexionOracle, TransaccionOracle, dtOracle));
            prResultado[4] = new OracleParameter("4", prParametros.CodigoTerminal);


            return prResultado;
        }


        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA OBTENER UN LISTADO DE LAS TABLAS A LAS QUE SE LES DEBE HACER UNA CONSULTA
        // PARAMETROS:   sCodigoProceso = NUMERO DE PROCESO A REALIZAR
        //               sCodigoSubProceso = NUMERO DE SUBPROCESO A REALIZAR
        //               sMaestra =  INDICA SI LA TABLA HACE PARTE DE LA CARGA DE MAESTROS
        // RETORNO: UN DATATABLE CON EL NOMBRE DE LAS TABLAS A CONSULTAR
        private int ObtenerMaximoIdProceso(string sTerminal, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ProcesamientoQueryOracle dtOracle)
        {
            string sSenteciaSQL;
            //OBTENER TABLAS A CONSULTAR
            sSenteciaSQL = " SELECT NVL(MAX(IDPROCESOCOMUNICACION),0) + 1 " +
                           " FROM WS_RESULTADOSCOMUNICACION " +
                           " WHERE CODIGOTERMINAL = " + sTerminal;

            DataTable dtIdProceso = dtOracle.EjecutarTablaSinParametros(sSenteciaSQL, ConexionOracle, TransaccionOracle);
            return Convert.ToInt32(dtIdProceso.Rows[0][0]);
        }

        // TIPO FUNCION : METODO DE LA CLASE
        // DESCRIPCION: FUNCION PARA OBTENER UN LISTADO DE LAS TABLAS A LAS QUE SE LES DEBE HACER UNA CONSULTA
        // PARAMETROS:   sCodigoProceso = NUMERO DE PROCESO A REALIZAR
        //               sCodigoSubProceso = NUMERO DE SUBPROCESO A REALIZAR
        //               sMaestra =  INDICA SI LA TABLA HACE PARTE DE LA CARGA DE MAESTROS
        // RETORNO: UN DATATABLE CON EL NOMBRE DE LAS TABLAS A CONSULTAR
        private int ObtenerIdProcesoActualizar(string sTerminal, int iCodigoProceso, OracleConnection ConexionOracle, OracleTransaction TransaccionOracle, ProcesamientoQueryOracle dtOracle)
        {
            string sSenteciaSQL;
            //OBTENER TABLAS A CONSULTAR
            sSenteciaSQL = " SELECT NVL(MAX(IDPROCESOCOMUNICACION),0) " +
                           " FROM WS_RESULTADOSCOMUNICACION " +
                           " WHERE CODIGOTERMINAL = " + sTerminal +
                           " AND CODIGOPROCESO = " + Convert.ToString(iCodigoProceso);

            DataTable dtIdProceso = dtOracle.EjecutarTablaSinParametros(sSenteciaSQL, ConexionOracle, TransaccionOracle);
            return Convert.ToInt32(dtIdProceso.Rows[0][0]);
        }
    }
}

#endregion
public enum EstadoTerminal { EjecutandoProceso = 1, ProcesoTerminado = 2, ErrorProceso = 3, NoExisteTerminal = 4 }