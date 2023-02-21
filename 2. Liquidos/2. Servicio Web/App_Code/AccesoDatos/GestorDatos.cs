using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Data.OracleClient;
using Datascan.Praxair.UtilidadGestorDatos;
using System.IO;

/// <summary>
/// Clase utilizadad para realizar las consultas sobre el servidor Oracle de Prax2000
/// </summary>
public class GestorDatos
{
    OracleConnection _conexionOracle = null;
    OracleTransaction _transaccionOracle = null; 
    ProcesamientoQueryOracle _procesamientoOracle = null;
    //CREAR UN OBJETO PARA LOS PARAMETROS
    ParametrosEjecucion _parametrosEjecucion = null;
    //NOMBRE PROCEDIMIENTO ALMACENADO INICIO
    string _procedimientoAlmacenadoInicio = string.Empty;
    string _procedimientoDescarga = null;
    bool _EjecutarInmediato = false;
    string _DescripcionTerminal = string.Empty;
    string _cadenaConexion = string.Empty;

    //Contructor 
    public GestorDatos()
    {
        _conexionOracle = null;
        _transaccionOracle = null;
        _procesamientoOracle = null;
        _parametrosEjecucion = null;
        _procedimientoAlmacenadoInicio = string.Empty;
        _procedimientoDescarga = null;
        _EjecutarInmediato = false;
        _DescripcionTerminal = string.Empty;
        _cadenaConexion = string.Empty;
    }



    public void ConfigurarGestor(string pathGuardarErrores, string usuarioOracle, string contrasenaOracle, string baseDatosOracle, int codigoProceso, int CodigoPrograma, int codigoEmpresa, int codigoGrupo)
    {
        //TODO SIN CONEXION:
        return;
        _procesamientoOracle = new ProcesamientoQueryOracle();
        _parametrosEjecucion = new ParametrosEjecucion();
        //CONECTAR CON EL SERVIDOR ORACLE      
        _procesamientoOracle.PathGuardarErrores = pathGuardarErrores;
        _procesamientoOracle.Conectar(ref _conexionOracle, usuarioOracle, contrasenaOracle, baseDatosOracle, ref _transaccionOracle);
        _parametrosEjecucion.CodigoProceso = codigoProceso;
        _parametrosEjecucion.CodigoPrograma = CodigoPrograma;
        _parametrosEjecucion.CodigoEmpresa = Convert.ToString(codigoEmpresa);
        _parametrosEjecucion.CodigoGrupo = Convert.ToString(codigoGrupo);
        _cadenaConexion = "User Id=" + usuarioOracle + ";Password=" + contrasenaOracle + ";Data Source=" + baseDatosOracle;
    }
    #region "Metodos generales"
       
    /// <summary>
    /// Funcion para validar que la terminal se encuentre registrada en la table WS_TERMINALESSYMBOL de Oracle
    /// </summary>
    /// <param name="sTerminal">Codigo de la terminal que realiza la peticion</param>        
    public void ValidarTerminal(string sTerminal)
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        sSenteciaSQL = " SELECT CODIGOTERMINAL, DESCRIPCION, CODIGOSUCURSAL " +
                       " FROM WS_TERMINALESSYMBOL " +
                       " WHERE DESCRIPCION = '" + sTerminal + "'";
        DataTable dtDatosConfiguracion = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);      
        if (dtDatosConfiguracion.Rows.Count == 0)
        {
            //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
            throw new Exception("Terminal No Registrada");
        }
        else
        {
            _parametrosEjecucion.CodigoSucursal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CodigoSucursal"]);
            _parametrosEjecucion.CodigoTerminal = Convert.ToString(dtDatosConfiguracion.Rows[0]["CODIGOTERMINAL"]);
        }
        _DescripcionTerminal = sTerminal;
    }

    /// <summary>
    /// Funcion para validar que existe el grupo registrado en la tabla SPB_TERMINAL_EMPRESA de Oracle
    /// </summary>
    /// <param name="sTerminal">Codigo de la terminal que realiza la peticion</param>        
    public void ValidarGrupoTerminal(string sGrupoTerminal)
    {
        try
        {
            string sSenteciaSQL;
            //OBTENER TABLAS A CONSULTAR
            sSenteciaSQL = String.Format("SELECT TMECODGRUPO FROM SPB_TERMINAL_EMPRESA WHERE TMECODGRUPO = {0}", sGrupoTerminal);

            //TODO SIN CONEXION:
            return;

          DataTable dtDatosConfiguracion = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
        
        
        if (dtDatosConfiguracion.Rows.Count == 0)
        {
            //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
            throw new Exception("Grupo Terminal No Registrado");
        }
        else
        {
            _parametrosEjecucion.CodigoTerminal = Convert.ToString(dtDatosConfiguracion.Rows[0]["TMECODGRUPO"]);
        }
    }
    catch (Exception ex)
    {
        throw new Exception("Error validando la terminal nro " + sGrupoTerminal + ". Error: " + ex.Message);
    } 
        //_parametrosEjecucion.CodigoTerminal = sGrupoTerminal;
    }

    /// <summary>
    /// Funcion Para Validar que el proceso de ejecucion este parametrizado en WS_PROCESOSCOMUNICACION
    /// </summary>    
    public void ValidarProcedimiento(bool descargaDatos, bool confirmarCarga)
    {
        //TODO SIN CONEXION:
        return;

        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        sSenteciaSQL = " SELECT PROCEDIMIENTOINICIO, PROCEDIMIENTOCIERRE, EJECUTARINMEDIATO, TIPOPROCESO " +
                       " FROM WS_PROCESOSCOMUNICACION " +
                       " WHERE CODIGOPROCESO = " + _parametrosEjecucion.CodigoProceso.ToString() +
                       " AND CODIGOPROGRAMA = " + _parametrosEjecucion.CodigoPrograma.ToString();
        DataTable dtDatosConfiguracion = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);

        //OBTENER NOMBRE DEL PROCEDIMIENTO ALMACENADO PARA EJEJCUTAR
        if (dtDatosConfiguracion.Rows.Count == 0)
        {
            //LANZAR UNA EXCEPTION INDICANDO QUE LA TERMINAL NO ESTA REGISTRADA
            throw new Exception("NO Existe parametrización Proceso Carga");
        }
        else
        {            
            if (descargaDatos)
            {
                if (Convert.ToInt16(dtDatosConfiguracion.Rows[0]["EJECUTARINMEDIATO"]) == 1)
                {
                    _EjecutarInmediato = true;
                    _procedimientoDescarga = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOINICIO"]);
                }
                //SI ES DESCARGA EJECUTAR TALONARIOS PROCESO 2
                else
                {
                    if (_parametrosEjecucion.CodigoProceso == 2 || _parametrosEjecucion.CodigoProceso == 12)
                    {
                        _procedimientoDescarga = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOCIERRE"]);
                    }
                }
            }
            else
            {
                if (!confirmarCarga)
                {
                    _procedimientoAlmacenadoInicio = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOINICIO"]);
                }
                else
                {
                    _procedimientoAlmacenadoInicio = Convert.ToString(dtDatosConfiguracion.Rows[0]["PROCEDIMIENTOCIERRE"]);
                }
            }
        }

    }

    /// <summary>
    /// Funcion Para Cargar Los Datos de Punto de Venta Necesarios para la ejecucion de Procedimientos en Oracle
    /// </summary>
    public void ObtenerPuntoVentaRutaPrincipal()
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR 'CODIGOPUNTOVENTA se cambio mientrastanto PUNTOVENTA  29/04/2010
        sSenteciaSQL = " SELECT RUTAPRINCIPAL, PUNTOVENTA " + 
        " FROM DTSC_PARAMETROS_TERMINAL " +
        " WHERE CODIGOEMPRESA = '" + _parametrosEjecucion.CodigoEmpresa + "' AND " +
        " CODIGOGRUPO = '" + _parametrosEjecucion.CodigoGrupo + "' AND " +
        " CODIGOSUCURSAL = '" + _parametrosEjecucion.CodigoSucursal + "' AND " +
        " CODIGOTERMINAL = '" + _parametrosEjecucion.CodigoTerminal + "'";
        DataTable dtDatosConfiguracion = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
        if (dtDatosConfiguracion.Rows.Count == 0)
        {
            //LANZAR UNA EXCEPTION INDICANDO QUE NO EXISTE LA RUTA PRINCIPAL
            throw new Exception("No existe la ruta principal!!");

        }
        else
        {
            _parametrosEjecucion.RutaPrincipal = Convert.ToString(dtDatosConfiguracion.Rows[0]["RUTAPRINCIPAL"]);
           //CODIGOPUNTOVENTA se cambio mientrastanto PUNTOVENTA
            _parametrosEjecucion.PuntoVenta = Convert.ToString(dtDatosConfiguracion.Rows[0]["PUNTOVENTA"]);
        }
    }

    /// <summary>
    /// Metodo Para Ejecutar El procedimiento Almacenado de Inicio en la carga
    /// </summary>
    /// <param name="rwRegistroTablaAuditoria">Registro de la tabla de auditoria de Prcedimientos</param>
    public void EjecutarProcedimientoAlmacenado(DataRow rwRegistroTablaAuditoria, bool cargarDatos)
    {
        //TODO SIN CONEXION:
        return;

        if (_procedimientoAlmacenadoInicio == string.Empty)
        {
            return;
        }
        //EJECUTAR EL PROCEDIMIENTO ALMACENADO
        OracleParameter[] prEjecucionOracle;
        try
        {
            _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);
            if (cargarDatos)
            {
                prEjecucionOracle = _procesamientoOracle.ObtenerParametrosSentencia(ref _procedimientoAlmacenadoInicio, _parametrosEjecucion);
                _procesamientoOracle.EjecutarQueryParametros("BEGIN " + _procedimientoAlmacenadoInicio + "; END;", prEjecucionOracle, _conexionOracle, _transaccionOracle);
            }
            _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);
        }
        catch (Exception ex)
        {
            _procesamientoOracle.RollbackTransaccion(ref _transaccionOracle);
            if (rwRegistroTablaAuditoria != null)
            {
                this.CerrarRegistroAuditoriaCarga(Convert.ToInt32(rwRegistroTablaAuditoria["IdProcesoCarga"]), EstadoEjecucionProcedimientoAlmacenado.ErrorSQLProceso, ex.Message);
            }
            throw new Exception("Error ejecutando procedimiento de inicio " + ex.Message);
        }       
     }
     
    /// <summary>
    /// Funcion para Insertar registros en la tabla WS_ResultadosComunicacion
    /// </summary>
    /// <param name="dtFechaProceso">Fecha Proceso</param>
    /// <param name="sDatosRecibidos">Datos Transmitidos</param>
    /// <param name="sEstadoRuta">Estado de la ruta</param>
     public void InsertarRegistroResultadoComunicacion(DateTime dtFechaProceso, string sDatosRecibidos, string sEstadoRuta, string rutaPrincipal)
     {
         //CARGAR LA SENTENCIA SQL
         //REVISAR CUANTOS PARAMETROS TRAE LA SENTANCIA
         OracleParameter[] prResultado;
         string sSentenciaSQL = "INSERT INTO WS_RESULTADOSCOMUNICACION (CODIGOPROGRAMA,CODIGOTERMINAL,CODIGORUTA,FECHAINSERCION,FECHAPROCESAMIENTO, " +
                          " ESTADOCOMUNICACION, OBSERVACION, DATOSENVIADOS, CODIGOPROCESO, IDPROCESOCOMUNICACION) VALUES (:0,:1,:2,:3,:4,:5,:6,:7,:8,:9)";

         prResultado = new OracleParameter[10];
         prResultado[0] = new OracleParameter("0", _parametrosEjecucion.CodigoPrograma);
         prResultado[1] = new OracleParameter("1", _parametrosEjecucion.CodigoTerminal);
         prResultado[2] = new OracleParameter("2", _parametrosEjecucion.RutaPrincipal);
         prResultado[3] = new OracleParameter("3", dtFechaProceso);
         prResultado[4] = new OracleParameter("4", System.DBNull.Value);
         /*EXISTEN TRES ESTADOS C=CARGADO, G=GUARDADO, P=PROCESADO*/
         prResultado[5] = new OracleParameter("5", sEstadoRuta);
         prResultado[6] = new OracleParameter("6", "");
         prResultado[7] = new OracleParameter("7", sDatosRecibidos);
         prResultado[8] = new OracleParameter("8", _parametrosEjecucion.CodigoProceso);
         prResultado[9] = new OracleParameter("9", this.ObtenerMaximoIdProceso());

         if (_parametrosEjecucion.RutaPrincipal == "" || _parametrosEjecucion.RutaPrincipal == null)
         {
             _parametrosEjecucion.RutaPrincipal = rutaPrincipal;
         }
         _procesamientoOracle.EjecutarQueryParametros(sSentenciaSQL, prResultado, _conexionOracle, _transaccionOracle);
     }

     /// <summary>
     /// Funcion para Insertar registros en la tabla WS_ResultadosComunicacion
     /// </summary>
     /// <param name="dtFechaProceso">Fecha Proceso</param>
     /// <param name="sDatosRecibidos">Datos Transmitidos</param>
     /// <param name="sEstadoRuta">Estado de la ruta</param>
     public void InsertarRegistroResultadoComunicacionGrupo(string sDatosRecibidos, string sEstadoRuta)
     {
         OracleParameter[] prResultado;
         string sSentenciaSQL="";
                          
         sSentenciaSQL = "INSERT INTO WS_RESULTADOSCOMUNICACION (CODIGOPROGRAMA,TMECODCRUPO,FECHAINSERCION,FECHAPROCESAMIENTO, " +
         " ESTADOCOMUNICACION, OBSERVACION, DATOSENVIADOS, CODIGOPROCESO, IDPROCESOCOMUNICACION, CODIGOTERMINAL, CODIGORUTA) VALUES (:0,:1,:2,:3,:4,:5,:6,:7,:8,:9 * 10000,:10)";
 
         prResultado = new OracleParameter[11];
         prResultado[0] = new OracleParameter("0", 2);
         prResultado[1] = new OracleParameter("1", _parametrosEjecucion.CodigoTerminal);
         prResultado[2] = new OracleParameter("2", DateTime.Now);
         prResultado[3] = new OracleParameter("3", System.DBNull.Value);
         prResultado[4] = new OracleParameter("4", sEstadoRuta);
         prResultado[5] = new OracleParameter("5", "");
         prResultado[6] = new OracleParameter("6", sDatosRecibidos);
         prResultado[7] = new OracleParameter("7", _parametrosEjecucion.CodigoProceso);
         prResultado[8] = new OracleParameter("8", ObtenerMaximoIdGrupoProceso());
         prResultado[9] = new OracleParameter("9", _parametrosEjecucion.CodigoTerminal);
         prResultado[10] = new OracleParameter("10", "N/A");

         _procesamientoOracle.EjecutarQueryParametros(sSentenciaSQL, prResultado, _conexionOracle, _transaccionOracle);
         
     }

    
     /// <summary>
     /// Funcion para obtener el id de la transaccion en la tabla ws_resultados comunicacion
     /// </summary>    
     /// <returns>Numero de la transaccion para la tabla ws_resultados comunicacion</returns>

    private int ObtenerMaximoIdGrupoProceso()
     {
         string sSenteciaSQL = "";
         
         sSenteciaSQL = " SELECT NVL(MAX(IDPROCESOCOMUNICACION),0) + 1 " +
                        " FROM WS_RESULTADOSCOMUNICACION " +
                        " WHERE TMECODCRUPO = " + _parametrosEjecucion.CodigoTerminal.ToString();
         

         DataTable dtIdProceso = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
         return Convert.ToInt32(dtIdProceso.Rows[0][0]);
     }

     /// <summary>
     /// Funcion para obtener el id de la transaccion en la tabla ws_resultados comunicacion
     /// </summary>    
     /// <returns>Numero de la transaccion para la tabla ws_resultados comunicacion</returns>

     private int ObtenerMaximoIdProceso()
     {
         string sSenteciaSQL;
         //OBTENER TABLAS A CONSULTAR
         sSenteciaSQL = " SELECT NVL(MAX(IDPROCESOCOMUNICACION),0) + 1 " +
                        " FROM WS_RESULTADOSCOMUNICACION " +
                        " WHERE CODIGOTERMINAL = " + _parametrosEjecucion.CodigoTerminal;

         DataTable dtIdProceso = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
         return Convert.ToInt32(dtIdProceso.Rows[0][0]);
     }
    /// <summary>
    /// Funcion para actualizar el resultado de la comunicacion en la tabla WS_ResultadosComunicacion
    /// </summary>
    /// <param name="dtFechaProceso">Fecha Finalizacion Proceso</param>
    /// <param name="sEstadoRuta">Estado de la Ruta</param>
    /// <param name="sObservacion">Observacion</param>    
     public void ActualizarRegistroResultadoComunicacion(DateTime dtFechaProceso, string sEstadoRuta, string sObservacion)
     {
         //CARGAR LA SENTENCIA SQL
         //REVISAR CUANTOS PARAMETROS TRAE LA SENTANCIA
         OracleParameter[] prResultado;
         string sSentenciaSQL = "UPDATE WS_RESULTADOSCOMUNICACION SET ESTADOCOMUNICACION = :0, OBSERVACION = :1, FECHAPROCESAMIENTO = :2 " +
                         " WHERE IDPROCESOCOMUNICACION = :3 AND CODIGOTERMINAL = :4";

         prResultado = new OracleParameter[5];
         prResultado[0] = new OracleParameter("0", sEstadoRuta);
         prResultado[1] = new OracleParameter("1", sObservacion);
         prResultado[2] = new OracleParameter("2", dtFechaProceso);
         prResultado[3] = new OracleParameter("3", this.ObtenerIdProcesoActualizar());
         prResultado[4] = new OracleParameter("4", _parametrosEjecucion.CodigoTerminal);        
         _procesamientoOracle.EjecutarQueryParametros(sSentenciaSQL, prResultado, _conexionOracle, _transaccionOracle);
     }

     /// <summary>
     /// Funcion para actualizar el resultado de la comunicacion en la tabla WS_ResultadosComunicacion
     /// </summary>
     /// <param name="dtFechaProceso">Fecha Finalizacion Proceso</param>
     /// <param name="sEstadoRuta">Estado de la Ruta</param>
     /// <param name="sObservacion">Observacion</param>    
     public void ActualizarRegistroResultadoComunicacionGrupo(DateTime dtFechaProceso, string sEstadoRuta, string sObservacion)
     {
         //CARGAR LA SENTENCIA SQL
         //REVISAR CUANTOS PARAMETROS TRAE LA SENTANCIA
         OracleParameter[] prResultado;
         string sSentenciaSQL = "BEGIN SPPQ_PROCESO_LIQUIDOS.SPSP_ACTUALIZA_RESULTADOS_PROC(:0,:1,:2,:3); END;"; 
         
         prResultado = new OracleParameter[4];
         prResultado[0] = new OracleParameter("0", _parametrosEjecucion.CodigoTerminal);
         prResultado[1] = new OracleParameter("1", sEstadoRuta);
         prResultado[2] = new OracleParameter("2", _parametrosEjecucion.CodigoProceso);
         prResultado[3] = new OracleParameter("3", sObservacion);         
         _procesamientoOracle.EjecutarQueryParametros(sSentenciaSQL, prResultado, _conexionOracle, _transaccionOracle);
     }

     /// <summary>
     /// Funcion para obtener el Id del proceso de carga de la tabla WS_resultados comunicacion que se necesita actualizar
     /// </summary>
     /// <returns>IdProceso de Comunicacion</returns>
     private int ObtenerIdProcesoActualizar()
     {
         string sSenteciaSQL;
         //OBTENER TABLAS A CONSULTAR
         sSenteciaSQL = " SELECT NVL(MAX(IDPROCESOCOMUNICACION),0) " +
                        " FROM WS_RESULTADOSCOMUNICACION " +
                        " WHERE CODIGOTERMINAL = " + _parametrosEjecucion.CodigoTerminal +
                        " AND CODIGOPROCESO = " + Convert.ToString(_parametrosEjecucion.CodigoProceso);

         DataTable dtIdProceso = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
         return Convert.ToInt32(dtIdProceso.Rows[0][0]);
     }


    public void GuardarErrores(string sError)
    {
        _procesamientoOracle.GuardarErrores(sError);
    }

    public void Desconectar()
    {
        //TODO SIN CONEXION:
        return;
        _procesamientoOracle.Desconectar(ref _conexionOracle);
    }

    #endregion

    #region "Metodos Para la Carga de Datos"
    
    /// <summary>
    /// FUNCION PARA REVISAR SI EXISTE EL REGISTRO EN LA TABLA DTS_AUDITOPEDIDOS EN ESTADO 0,1
    /// </summary>    
    /// <returns>Registro Datos de la tabla de Auditoria</returns>
    public DataRow ExisteRegistroAuditoriaCarga()
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        //LA TABLA FUNCIONA COMO UNA COLA, OBTENGO EL ULTIMO
        sSenteciaSQL = " SELECT IdProcesoCarga, IdTerminal, PuntoVenta, FechaInicioProcesamiento, FechaFinProcesamiento, EstadoProceso " +
                       " FROM DTSC_AUDITPROCESOCARGA " +
                       " WHERE IdTerminal = '" + _parametrosEjecucion.CodigoTerminal + "'" +                       
                       " AND IdProcesoCarga IN (SELECT NVL(MAX(IdProcesoCarga), 0) " +
                       " FROM DTSC_AUDITPROCESOCARGA " +
                       " WHERE IdTerminal = '" + _parametrosEjecucion.CodigoTerminal + "')";                       
        DataTable dtAuditoriaProcedimiento = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
        if (dtAuditoriaProcedimiento.Rows.Count > 0)
        {
            return dtAuditoriaProcedimiento.Rows[0];
        }
        else
        {
            return null;
        }        
    }
        
    /// <summary>
    /// FUNCION PARA INSERTAR UN REGISTRO EN LA TABLA DE AUTIDORIA
    /// </summary>
    /// <returns>REGISTRO EN LA TABLA AUDITORIA</returns>     
    public DataRow InsertarRegistroAuditoriaCarga()
    {
        //CARGAR LA SENTENCIA SQL
        //REVISAR CUANTOS PARAMETROS TRAE LA SENTANCIA
        OracleParameter[] prResultado;
        string sSentenciaSQL = "INSERT INTO DTSC_AUDITPROCESOCARGA (IdProcesoCarga, IdTerminal, PuntoVenta, FechaInicioProcesamiento, FechaFinProcesamiento, EstadoProceso) " +
                        " VALUES (:0,:1,:2,:3,:4,:5)";

        prResultado = new OracleParameter[6];
        prResultado[0] = new OracleParameter("0", this.ObtenerIdProcesoCarga());
        prResultado[1] = new OracleParameter("1", _parametrosEjecucion.CodigoTerminal);
        if (_parametrosEjecucion.PuntoVenta == string.Empty)
        {
            prResultado[2] = new OracleParameter("2", _DescripcionTerminal);
        }
        else
        {
            prResultado[2] = new OracleParameter("2", _parametrosEjecucion.PuntoVenta);
        }
        prResultado[3] = new OracleParameter("3", DateTime.Now);
        prResultado[4] = new OracleParameter("4", System.DBNull.Value);
        prResultado[5] = new OracleParameter("5", Convert.ToInt32(EstadoEjecucionProcedimientoAlmacenado.InicioEjecucion));
        _procesamientoOracle.EjecutarQueryParametros(sSentenciaSQL, prResultado, _conexionOracle, _transaccionOracle);
        DataRow rwRespuesta = this.ExisteRegistroAuditoriaCarga();
        if (rwRespuesta == null)
        {
            throw new Exception("Error Insertando Registro en la tabla de Auditoria");
        }
        else
        {
            return rwRespuesta;
        }
    }


    /// <summary>
    /// Funcion para cerrar un registro en la tabla de Auditoria de procedimientos
    /// </summary>
    /// <param name="IdRegistroCarga">Numero Id registro de la Carga</param>
    /// <param name="EstadoEjecucion">Estado para cerrar el registro</param>    
    /// <param name="observacion">Detalle del mensaje de error</param>    
    public void CerrarRegistroAuditoriaCarga(int IdRegistroCarga, EstadoEjecucionProcedimientoAlmacenado EstadoEjecucion, string observacion)
    {
        //CARGAR LA SENTENCIA SQL
        //REVISAR CUANTOS PARAMETROS TRAE LA SENTANCIA
        OracleParameter[] prResultado;
        string sSentenciaSQL = "UPDATE DTSC_AUDITPROCESOCARGA SET FechaFinProcesamiento = :0, EstadoProceso= :1, " +
                        " OBSERVACION = :3 WHERE IdProcesoCarga = :2 AND IdTerminal = :4";
        
        prResultado = new OracleParameter[5];
        prResultado[0] = new OracleParameter("0", DateTime.Now);
        prResultado[1] = new OracleParameter("1", Convert.ToInt32(EstadoEjecucion));
        prResultado[2] = new OracleParameter("2", IdRegistroCarga);
        prResultado[3] = new OracleParameter("3", observacion);
        prResultado[4] = new OracleParameter("4", _parametrosEjecucion.CodigoTerminal);
        _procesamientoOracle.EjecutarQueryParametros(sSentenciaSQL, prResultado, _conexionOracle, _transaccionOracle);
    }

    // TIPO FUNCION : METODO DE LA CLASE
    // DESCRIPCION: FUNCION PARA REVISAR SI EXISTE EL REGISTRO EN LA TABLA DTS_AUDITOPEDIDOS EN ESTADO 0,1
    // PARAMETROS:   sNumeroTerminal = NUMERO DE TERMINAL
    //               sPuntoVenta = PUNTO DE VENTA
    private Int32 ObtenerIdProcesoCarga()
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        //LA TABLA FUNCIONA COMO UNA COLA, OBTENGO EL ULTIMO
        sSenteciaSQL = " SELECT NVL(MAX(IdProcesoCarga),0) + 1 " +
                       " FROM DTSC_AUDITPROCESOCARGA " +
                       " WHERE IdTerminal = '" + _parametrosEjecucion.CodigoTerminal + "'";                       
        DataTable dtAuditoriaProcedimiento = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
        if (dtAuditoriaProcedimiento.Rows.Count > 0)
        {
            return Convert.ToInt32(dtAuditoriaProcedimiento.Rows[0][0]);
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// Metodo Para Ibtener las setencias SQL de las tablas a enviar a la base de datos
    /// </summary>
    /// <returns>DataTable con las consultas SQL</returns>
    public DataTable ObtenerTablasConsultar()
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        sSenteciaSQL = " SELECT NOMBRETABLA, CONSULTASQL  " +
                       " FROM EXPEPRAX.SPV_PROCESOS_TABLAS_1 " +
                       " WHERE CODIGOPROCESO = " + _parametrosEjecucion.CodigoProceso.ToString() +
                       " AND CODIGOPROGRAMA = " + _parametrosEjecucion.CodigoPrograma.ToString() +
                       " --AND HABILITADA = 'S'" +
                       " ORDER BY ORDEN";
        return _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
    }
    
    /// <summary>
    /// FUNCION PARA CARGAR TODOS LOS DATOS AL DATASET DE RESPUESTA, EJECUTA UNA SENTENCIA SQL Y LA RESPUESTA LA ADICIONA AL DATASET
    /// </summary>    
    /// <param name="dtTablasConsultar"></param>    
    /// <returns>DataSet con los datos consultados</returns>
    public DataSet LlenarDatasetDescarga(DataTable dtTablasConsultar)
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
                        prEjecucionOracle = _procesamientoOracle.ObtenerParametrosSentencia(ref sSentenciaSQL, _parametrosEjecucion);
                        dtRespuesta = _procesamientoOracle.EjecutarTablaParametros(sSentenciaSQL, prEjecucionOracle, _conexionOracle, _transaccionOracle);
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

    /// <summary>
    /// Guardar archivos con la auditoria de los datos enviados por la terminal
    /// </summary>
    /// <param name="dsEnvioTerminal">dataset con los datos enviados</param>
    /// <param name="respuesta">dataset de los datos en formato string</param>
    public void GuardarAuditoriaDatos(DataSet dsEnvioTerminal, string respuesta)
    {
        //GUARDAR LOS DATOS DE RESPUESTA EN UNA VARIABLE STRING
        StringWriter fs = new StringWriter();
        dsEnvioTerminal.WriteXml(fs);
        string datosXML = fs.ToString();
        fs.Close();
        //GUARDAR LOS DATOS DE RESPUESTA EN UN ARCHIVO DE LOG
        string sFileAux;
        sFileAux = "Proceso";
        sFileAux = sFileAux + DateTime.Now.ToString("ddMMyyyyhhmmss");
        _procesamientoOracle.GuardarArchivosXML(datosXML, "BK-C-" + sFileAux + ".xml", false);
        //ENVIAR EL STRING CON LOS DATOS REQUERIDOS                        
        _procesamientoOracle.GuardarArchivosXML(respuesta, "BK-C-" + sFileAux + ".txt", false);
    }
    #endregion

#region "Metodos Descarga Datos"

    /// <summary>
    /// Guardar archivos con la auditoria de los datos enviados por la terminal
    /// </summary>
    /// <param name="dsGuardarDatos">dataset con los datos enviados</param>
    /// <param name="sDatosEnviados">dataset de los datos en formato string</param>
    public void GuardarBackupDescarga(DataSet dsGuardarDatos, string sDatosEnviados)
    {
        //NOMBRE DEL ARCHIVO DE BACKUP
        string sFileAux;
        sFileAux = _parametrosEjecucion.CodigoTerminal + DateTime.Now.ToString("ddMMyyyyhhmmss");
        //VOLVER DATASET LO QUE SE ENVIA
        _procesamientoOracle.GuardarArchivosXML(sDatosEnviados, "BK-D-" + sFileAux + ".txt", true);
        StringWriter fs = new StringWriter();
        dsGuardarDatos.WriteXml(fs);
        string sRespuesta = fs.ToString();
        fs.Close();
        //GUARDAR LOS DATOS ENVIADOS EN UN ARCHIVO DE BACKUP
        _procesamientoOracle.GuardarArchivosXML(sRespuesta, "BK-D-" + sFileAux + ".xml", true);
    }

    /// <summary>
    /// Metodo que maneja la logica para la descarga de los datos
    /// </summary>
    /// <param name="sDatosEnviados">String con los datos enviados</param>
    /// <param name="backupTalonarios">Bandera para revisar si se realiza backup de los talonarios</param>
    /// <param name="dsGuardarDatos">DataSet con los datos enviados</param>
    public void DescargaDatosPRAX2000(string sDatosEnviados, bool backupTalonarios, DataSet dsGuardarDatos)
    {
        try
        {
            _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);
            this.InsertarRegistroResultadoComunicacion(DateTime.Now, sDatosEnviados, "G", _parametrosEjecucion.RutaPrincipal);
            _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);

            //MODIFICACION POR SEGURIDAD GUARDAR LOS TALONARIOS DESCARGADOS
            if (backupTalonarios)
            {
                this.GuardarTalonariosBackup(dsGuardarDatos);
            }

            //ACTUALIZAR TALONARIOS
            if ((_parametrosEjecucion.CodigoProceso == 2 || _parametrosEjecucion.CodigoProceso == 12) && _procedimientoDescarga != null && _EjecutarInmediato == false)
            {
                this.ActualizarTalonarios(dsGuardarDatos);
            }
            else
            {
                //REVISAR SI SE EJECUTA DE INMEDIATO EL PROCEDIMIENTO ALMACENADO (IMPLICA PROCESAR DATOS)
                if (_procedimientoDescarga != null)
                {
                    this.InsertarDatosDescarga(dsGuardarDatos);
                }
            }
        }
        catch (Exception ex)
        {
            _procesamientoOracle.RollbackTransaccion(ref _transaccionOracle);
            throw new Exception("Error procesando descarga: " + ex.Message);
        }
    }
    /// <summary>
    /// Metodo que maneja la logica para la descarga de los datos
    /// </summary>
    /// <param name="sDatosEnviados">String con los datos enviados</param>
    /// <param name="backupTalonarios">Bandera para revisar si se realiza backup de los talonarios</param>
    /// <param name="dsGuardarDatos">DataSet con los datos enviados</param>
    public void DescargaDatosPRAX2000Grupo(string sDatosEnviados, bool backupTalonarios, DataSet dsGuardarDatos)
    {
        try
        {
            _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);
            this.InsertarRegistroResultadoComunicacionGrupo(sDatosEnviados, "G");            
            _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);

            //MODIFICACION POR SEGURIDAD GUARDAR LOS TALONARIOS DESCARGADOS
            if (backupTalonarios)
            {
                this.GuardarTalonariosBackup(dsGuardarDatos);
            }

            //ACTUALIZAR TALONARIOS
            if ((_parametrosEjecucion.CodigoProceso == 2 || _parametrosEjecucion.CodigoProceso == 12) && _procedimientoDescarga != null && _EjecutarInmediato == false)
            {
                this.ActualizarTalonarios(dsGuardarDatos);   
            }
            else
            {
                //REVISAR SI SE EJECUTA DE INMEDIATO EL PROCEDIMIENTO ALMACENADO (IMPLICA PROCESAR DATOS)
                if (_procedimientoDescarga != null)
                {
                    //this.InsertarDatosDescarga(dsGuardarDatos);
                    this.InsertarDatosDescargaGrupo(dsGuardarDatos);                    
                }
            }
        }
        catch (Exception ex)
        {
            _procesamientoOracle.RollbackTransaccion(ref _transaccionOracle);
            throw new Exception("Error procesando descarga: " + ex.Message);
        }
    }

    /// <summary>
    /// Metodo para guardar en la table DTSC_BCKTALDESCARGA los numeros de los talonarios utilizados    
    /// </summary>
    /// <param name="dsGuardarDatos">DataSet con los datos descargados</param>
    private void GuardarTalonariosBackup(DataSet dsGuardarDatos)
    {
        if (dsGuardarDatos.Tables.Contains("TALONARIOSDESCARGA"))
        {
            try
            {
                _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);
                DataSet dsTalonariosAuditar = new DataSet("Talonarios");
                DataTable dtAuxAuditar;
                dtAuxAuditar = dsGuardarDatos.Tables["TALONARIOSDESCARGA"].Copy();
                dtAuxAuditar.TableName = "BCKTALDESCARGA";
                dsTalonariosAuditar.Tables.Add(dtAuxAuditar);
                //INSERTAR LOS DATOS                
                _procesamientoOracle.EjecutarDescargaTalonarios(_parametrosEjecucion, _cadenaConexion, dsTalonariosAuditar, "BACKUP");
                _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);
            }
            catch (Exception ex)
            {
                _procesamientoOracle.RollbackTransaccion(ref _transaccionOracle);
                this.ActualizarRegistroResultadoComunicacionGrupo(DateTime.Now, "E", "Error ejecutando procedimiento almacenado: " + ex.Message);
                throw ex;
            }
        }
    }

    /// <summary>
    /// Metodo para realizar correctamente la actualizacion de los talonarios en la tabla M_PuntosVenta
    /// </summary>
    /// <param name="dsGuardarDatos">DataSet con los datos descargados</param>
    private void ActualizarTalonarios(DataSet dsGuardarDatos)
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

            //INSERTAR LOS DATOS
            _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);
            _procesamientoOracle.EjecutarDescargaTalonarios(_parametrosEjecucion, _cadenaConexion, dsTalonarios, _procedimientoDescarga);
            _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);
        }
        catch (Exception ex)
        {
            _procesamientoOracle.RollbackTransaccion(ref _transaccionOracle);
            this.ActualizarRegistroResultadoComunicacionGrupo(DateTime.Now, "E", "Error ejecutando procedimiento almacenado: " + ex.Message);            
            throw ex;
        }
    }

    /// <summary>
    /// Metodo para realizar la descarga completa de los datos
    /// </summary>
    /// <param name="dsGuardarDatos">DataSet con los datos descargados</param>
    private void InsertarDatosDescarga(DataSet dsGuardarDatos)
    {
        try
        {
            //INSERTAR LOS DATOS
            _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);
            _procesamientoOracle.ProcesarDataSetDescarga(dsGuardarDatos, _conexionOracle, _parametrosEjecucion, _transaccionOracle);
            OracleParameter[] prParametrosOracle = _procesamientoOracle.ObtenerParametrosSentencia(ref _procedimientoDescarga, _parametrosEjecucion);
            _procesamientoOracle.EjecutarQueryParametros("BEGIN " + _procedimientoDescarga + "; END;", prParametrosOracle, _conexionOracle, _transaccionOracle);
            //this.ActualizarRegistroResultadoComunicacion(DateTime.Now, "A", "");
            this.ActualizarRegistroResultadoComunicacionGrupo(DateTime.Now, "A", "");
            _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);
        }
        catch (Exception ex)
        {
            _procesamientoOracle.RollbackTransaccion(ref _transaccionOracle);
            this.ActualizarRegistroResultadoComunicacionGrupo(DateTime.Now, "E", "Error ejecutando procedimiento almacenado: " + ex.Message);                        
            throw ex;
        }
    }

    /// <summary>
    /// Metodo para realizar la descarga completa de los datos
    /// </summary>
    /// <param name="dsGuardarDatos">DataSet con los datos descargados</param>
    private void InsertarDatosDescargaGrupo(DataSet dsGuardarDatos)
    {
        try
        {
            string sSQl= "";
            string GrupoTerminal = _parametrosEjecucion.CodigoTerminal;
            string CodigoEmpresa = _parametrosEjecucion.CodigoEmpresa;
            string CodigoGrupo = _parametrosEjecucion.CodigoGrupo;
            string CodigoSucursal = _parametrosEjecucion.CodigoSucursal;
            string CodigoPuntoVenta = _parametrosEjecucion.PuntoVenta;

            string Alias="";

            string sSenteciaSQL;

            sSenteciaSQL = " SELECT TMEEMPID,TMECODIGOTERMINAL1 FROM SPB_TERMINAL_EMPRESA WHERE TMECODGRUPO = " + GrupoTerminal;
                           
            DataTable dt = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);

            DataSet dsFlag = new DataSet();
            DataTable dtAux;
            DataView dv;

           // int contador = 0;

            foreach (DataRow dr in dt.Rows)
            {
                //contador = contador + 1;
                dtAux = new DataTable();
                dsFlag = new DataSet();

                _parametrosEjecucion.CodigoEmpresa = dr[0].ToString();
                
                _parametrosEjecucion.CodigoTerminal = dr[1].ToString();
       
                sSQl = string.Format("SELECT SPPQ_PROCESO_LIQUIDOS.FN_RECUPERA_ALIAS({0}) FROM DUAL" , _parametrosEjecucion.CodigoEmpresa );
                Alias = _procesamientoOracle.EjecutarTablaSinParametros(sSQl,_conexionOracle,_transaccionOracle).Rows[0].ItemArray[0].ToString();
                
                sSQl = string.Format("SELECT SPPQ_PROCESO_LIQUIDOS.FN_RECUPERA_GRUPO({0},'{1}') FROM DUAL" , _parametrosEjecucion.CodigoEmpresa,Alias );
                _parametrosEjecucion.CodigoGrupo = _procesamientoOracle.EjecutarTablaSinParametros(sSQl,_conexionOracle,_transaccionOracle).Rows[0].ItemArray[0].ToString();
       
                sSQl = string.Format("SELECT SPPQ_PROCESO_LIQUIDOS.FN_RECUPERA_SUCURSAL({0},'{1}') FROM DUAL" , _parametrosEjecucion.CodigoTerminal,Alias );
                _parametrosEjecucion.CodigoSucursal = _procesamientoOracle.EjecutarTablaSinParametros(sSQl,_conexionOracle,_transaccionOracle).Rows[0].ItemArray[0].ToString();

                sSQl = string.Format("SELECT SPPQ_PROCESO_LIQUIDOS.FN_RECUPERA_PUNTOVENTA({0},{1},'{2}',{3},{4},'{5}') FROM DUAL", _parametrosEjecucion.CodigoEmpresa, _parametrosEjecucion.CodigoGrupo, _parametrosEjecucion.CodigoSucursal, _parametrosEjecucion.CodigoPrograma, _parametrosEjecucion.CodigoTerminal, Alias);
                _parametrosEjecucion.PuntoVenta = _procesamientoOracle.EjecutarTablaSinParametros(sSQl,_conexionOracle,_transaccionOracle).Rows[0].ItemArray[0].ToString();

                if (dsGuardarDatos.Tables.Contains("DetalleFactura"))
                {
                    dtAux = dsGuardarDatos.Tables["DetalleFactura"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }

                if (dsGuardarDatos.Tables.Contains("HojasRuta"))
                {
                    dtAux = dsGuardarDatos.Tables["HojasRuta"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                if (dsGuardarDatos.Tables.Contains("KardexCamionLiquidos"))
                {
                    dtAux = dsGuardarDatos.Tables["KardexCamionLiquidos"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                /*
                if (dsGuardarDatos.Tables.Contains("HojasRuta") && contador == 1 )
                {
                    dtAux = dsGuardarDatos.Tables["HojasRuta"];
                    dsFlag.Tables.Add(dtAux.Copy());
                }
                if (dsGuardarDatos.Tables.Contains("KardexCamionLiquidos") && contador == 1)
                {
                    dtAux = dsGuardarDatos.Tables["KardexCamionLiquidos"];
                    dsFlag.Tables.Add(dtAux.Copy());
                }
                  */
                if (dsGuardarDatos.Tables.Contains("DetallePedidoLiquidos"))
                {
                    dtAux = dsGuardarDatos.Tables["DetallePedidoLiquidos"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                if (dsGuardarDatos.Tables.Contains("MaestroFacturas"))
                {
                    dtAux = dsGuardarDatos.Tables["MaestroFacturas"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                if (dsGuardarDatos.Tables.Contains("Parametros_Descarga"))
                {
                    dtAux = dsGuardarDatos.Tables["Parametros_Descarga"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                if (dsGuardarDatos.Tables.Contains("Precintos"))
                {
                    dtAux = dsGuardarDatos.Tables["Precintos"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                if (dsGuardarDatos.Tables.Contains("TalonariosDescarga"))
                {
                    dtAux = dsGuardarDatos.Tables["TalonariosDescarga"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                if (dsGuardarDatos.Tables.Contains("PedidosLiquidos"))
                {
                    dtAux = dsGuardarDatos.Tables["PedidosLiquidos"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                if (dsGuardarDatos.Tables.Contains("Guia_Documento"))
                {
                    dtAux = dsGuardarDatos.Tables["Guia_Documento"];
                    dv = new DataView(dtAux, "EMPRESA = " + _parametrosEjecucion.CodigoEmpresa, "", DataViewRowState.CurrentRows);
                    dsFlag.Tables.Add(dv.ToTable());
                }
                _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);   
                _procesamientoOracle.ProcesarDataSetDescarga(dsFlag, _conexionOracle, _parametrosEjecucion, _transaccionOracle);
                _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);    
                
            }

            _parametrosEjecucion.CodigoTerminal = GrupoTerminal;
            _parametrosEjecucion.CodigoEmpresa = CodigoEmpresa;
            _parametrosEjecucion.CodigoGrupo = CodigoGrupo;
            _parametrosEjecucion.CodigoSucursal = CodigoGrupo;
            _parametrosEjecucion.PuntoVenta = CodigoPuntoVenta;
        
            _procesamientoOracle.IniciarTransaccion(ref _transaccionOracle, _conexionOracle);   
            OracleParameter[] prParametrosOracle = _procesamientoOracle.ObtenerParametrosSentencia(ref _procedimientoDescarga, _parametrosEjecucion);
            _procesamientoOracle.EjecutarQueryParametros("BEGIN " + _procedimientoDescarga + "; END;", prParametrosOracle, _conexionOracle, _transaccionOracle);
            this.ActualizarRegistroResultadoComunicacionGrupo(DateTime.Now, "A", "");
            _procesamientoOracle.CommitTransaccion(ref _transaccionOracle);    
      
        }
        catch (Exception ex)
        {
            _procesamientoOracle.RollbackTransaccion(ref _transaccionOracle);
            this.ActualizarRegistroResultadoComunicacionGrupo(DateTime.Now, "E", "Error ejecutando procedimiento almacenado: " + ex.Message);
            throw ex;
        }
    }
         
    public DataSet RecuperaGrupos()
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        sSenteciaSQL = "SELECT GLCODGRUPO,GLNOMGRUPO FROM SPB_GRUPO_LIQ WHERE GLESTADO = 1 ORDER BY GLNOMGRUPO";
        DataTable dtDatos = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
        DataSet ds = new DataSet();
        ds.Tables.Add(dtDatos);
        return ds;
    }
    public DataSet RecuperaNombreGrupo(string idGrupo)
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        sSenteciaSQL = String.Format("SELECT GLNOMGRUPO FROM SPB_GRUPO_LIQ WHERE GLCODGRUPO = {0}",idGrupo);
        DataTable dtDatos = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);
        DataSet ds = new DataSet();
        ds.Tables.Add(dtDatos);
        return ds;
    }

    public void GrabaError(string sTerminal, int iCodigoPrograma, string sMensaje, string sStack)
    {
        string sSenteciaSQL;
        //OBTENER TABLAS A CONSULTAR
        sSenteciaSQL = String.Format("INSERT INTO SPB_ERRORES_TERMINAL(IDTERMINAL1,CODIGOPROGRAMA,FECHA,MENSAJE,STACK) VALUES ({0},{1},SYSDATE,'{2}','{3}') ", sTerminal, iCodigoPrograma, sMensaje, sStack);
        DataTable dtDatos = _procesamientoOracle.EjecutarTablaSinParametros(sSenteciaSQL, _conexionOracle, _transaccionOracle);        
    }

#endregion
}

public enum EstadoEjecucionProcedimientoAlmacenado
{
    InicioEjecucion = 0,
    ProcesoTerminadoExitoso = 1,
    ErrorSQLProceso = 2,
    CerradoFechaInicioMayorUmbral = 3,
    CerradoFechaFinalizacionMayorUmbral = 4,
    ErrorEjcucionWebServices = 5
}
