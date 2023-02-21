using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;

#if NETCF20
namespace MovilidadCF.Data.SqlClient
{
    public class GestorDatosBase : MovilidadCF.Data.GestorDatosBase
    {
#else
using Desktop.Compression;
namespace Desktop.Data.SqlClient
{
    public class GestorDatosBase : Desktop.Data.GestorDatosBase
    {
#endif
        public GestorDatosBase() : base() { }

        public GestorDatosBase(String ConnectionString)
            : base(ConnectionString) { }

        internal override DbConnection CreateConnection(string ConnectionString)
        {
            return (DbConnection)new SqlConnection(ConnectionString);
        }

        public override DbCommand CreateCommand()
        {
            return (DbCommand)new SqlCommand();
        }

        public override void DeriveParameters(DbCommand cmd)
        {
            SqlCommandBuilder.DeriveParameters((SqlCommand)cmd);
            
            //Quito el parametro de retorno y se deja en la ultima posicion

            if (cmd.Parameters.Count > 0)
            {
                if (cmd.Parameters[0].Direction == ParameterDirection.ReturnValue)
                {
                    DbParameter parameterRetun = cmd.Parameters[0];
                    cmd.Parameters.RemoveAt(0);
                    cmd.Parameters.Add(parameterRetun);
                }                
            }
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return (DbCommandBuilder)new SqlCommandBuilder();
        }

        public new SqlConnection Connection
        {
            get
            {
                return (SqlConnection)base.Connection;
            }
        }

        public new SqlTransaction Transaction
        {
            get
            {
                return (SqlTransaction)base.Transaction;
            }
            set
            {
                base.Transaction = value;
            }
        }

        protected override void SetCommandParameters(DbCommand cmd, object[] Parameters)
        {
            SqlCommand cmdSql = (SqlCommand)cmd;
            for (int I = 0; I < Parameters.Length; I++)
            {
                cmdSql.Parameters.AddWithValue(String.Format("@p{0}", (I + 1)), Parameters[I]);
            }
        }

        internal override bool SaveCompressToDatabase(string sSerializedData, bool bUpdateCurrentRows)
        {
            DataCompression.UnCompressText(Convert.FromBase64String(sSerializedData), Convert.FromBase64String(sSerializedData).Length);
            string sLine = null;
            string[] sFields = null;
            Dictionary<string, Int32> sListaColumnas = null;
            string[] sFieldsTypes = null;
            string[] sValues = null;
            SqlCommandBuilder qrEngine;            
            int nTableCount = 0;
            int nRowCount = 0;
            int nTotalRowCount = 0;            
            int nRows = 0;
            int nTotalRows = 0;            
            try
            {
                // Se lee la liena con el número de tablas serializadas y el numero total de filas a procesar
                sLine = DataCompression.ReadLine();
                nTableCount = System.Convert.ToInt32(sLine.Substring(12));
                sLine = DataCompression.ReadLine();
                nTotalRowCount = System.Convert.ToInt32(sLine.Substring(15));                
                nTotalRows = 0;

                this.OpenConnection();

                while (true)
                {
                    // Se obtiene el nombre y cantidad de registros de cada tabla serializada
                    string sTableName = null;
                    sLine = DataCompression.ReadLine();
                    if (sLine == null || sLine.Trim() == string.Empty)
                    {
                        break;
                    }
                    sTableName = sLine.Substring(7).Trim();
                    sLine = DataCompression.ReadLine();
                    nRowCount = System.Convert.ToInt32(sLine.Substring(10));
                    if (nRowCount > 0)
                    {                        
                        nRows = 0;

                        // Cargamos un data Adapter para obtener los sqlcomandos
                        // Obtenemos la Estructura de la tabla
                        SqlDataAdapter daTabla = new SqlDataAdapter("SELECT * FROM " + sTableName, this.Connection);
                        DataTable dtEsquema = new DataTable(sTableName);
                        daTabla.FillSchema(dtEsquema, SchemaType.Source);
                        qrEngine = new SqlCommandBuilder(daTabla);
                        SqlCommand cmdSelect = new SqlCommand();
                        cmdSelect.Connection = this.Connection;
                        SqlCommand cmdInsert = null;
                        SqlCommand cmdUpdate = null;
                        //COMANDO INSERCION                            
                        cmdInsert = qrEngine.GetInsertCommand(true);
                        if (bUpdateCurrentRows)
                        {
                            //COMANDO SELECT UTILIZA PARA SABER SI SE HACE INSERT - UPDATE                            
                            cmdSelect = this.ObtenerSentenciaSelect(dtEsquema);
                            //COMANDO UPDATE
                            cmdUpdate = this.ObtenerSentenciaUpdate(dtEsquema);
                        }
                        else
                        {
                            //BORRAR TODOS LOS DATOS
                            //SOLO INSERCIONES
                            cmdSelect.CommandText = "DELETE FROM " + sTableName;
                            cmdSelect.ExecuteNonQuery();
                        }

                        // se obtienen los nombres de los campos
                        sLine = DataCompression.ReadLine();
                        sFields = sLine.Split('|');

                        if (sListaColumnas == null)
                        {
                            sListaColumnas = new Dictionary<string, Int32>();
                        }
                        sListaColumnas.Clear();

                        for (int iPos = 0; iPos <= sFields.Length - 1; iPos++)
                        {
                            sListaColumnas.Add(sFields[iPos], iPos);
                        }

                        // se obtienen los tipos de datos de las columnas
                        sLine = DataCompression.ReadLine();
                        sFieldsTypes = sLine.Split('|');

                        // Se procesa cada fila que venga serializada en la cadena
                        sLine = DataCompression.ReadLine();

                        bool bInsertRecord = false;
                        while ((sLine != null))
                        {
                            if (sLine.Trim() == string.Empty)
                            {
                                break;
                            }

                            // Se obtienen los valores que vienen en el registro
                            sValues = sLine.Split('|');

                            //Revisar si se realiza un Updata/Insert
                            bInsertRecord = true;
                            if (bUpdateCurrentRows)
                            {
                                if (dtEsquema.PrimaryKey.Length > 0)
                                {
                                    this.InicializarParametrosComando(cmdSelect);
                                    this.ConfigurarParametrosComando(cmdSelect, sValues, sListaColumnas, sTableName, sFieldsTypes);
                                    if (Convert.ToInt32(cmdSelect.ExecuteScalar()) > 0)
                                    {
                                        bInsertRecord = false;
                                    }
                                }
                            }

                            if (bInsertRecord)
                            {
                                try
                                {
                                    this.ConfigurarParametrosComando(cmdInsert, sValues, sListaColumnas, sTableName, sFieldsTypes);
                                    cmdInsert.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Error Ejecutando Insert para la tabla: " + sTableName + " Error: " + ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (cmdUpdate == null)
                                    {
                                        throw new Exception("No existe el comando Update para la tabla: " + sTableName);
                                    }
                                    else
                                    {
                                        if (cmdUpdate.CommandText != "NO")
                                        {
                                            this.ConfigurarParametrosComando(cmdUpdate, sValues, sListaColumnas, sTableName, sFieldsTypes);
                                            cmdUpdate.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Error Ejecutando update para la tabla: " + sTableName + " Error: " + ex.Message);
                                }
                            }

                            // Se registra el avance de la tabla
                            nRows += 1;
                            nTotalRows += 1;

                            // Se se lee el siguiente registro
                            sLine = DataCompression.ReadLine();
                        }                        
                    }
                }
                DataCompression.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Guardando datos: " + ex.Message);
            }
            finally
            {
                this.CloseConnection();
                DataCompression.Close();
            }
            return true;
        }

        internal override bool SaveToDatabase(string sSerializedData, bool bUpdateCurrentRows, bool bDespreciarTablas)
        {
            StringReader sr = new StringReader(sSerializedData); 
            string sLine = null;
            string[] sFields = null;
            Dictionary<string, Int32> sListaColumnas = null;
            string[] sFieldsTypes = null;
            string[] sValues = null;
            SqlCommandBuilder qrEngine;
            int nTableCount = 0;
            int nRowCount = 0;
            int nTotalRowCount = 0;
            int nRows = 0;
            int nTotalRows = 0;            
            try
            {
                //Configuramos el hilo de ejecucion
                this.PersonalizarCulturaHiloEjecucion();
                // Se lee la liena con el número de tablas serializadas y el numero total de filas a procesar
                sLine = sr.ReadLine();
                nTableCount = System.Convert.ToInt32(sLine.Substring(12));
                sLine = sr.ReadLine();
                nTotalRowCount = System.Convert.ToInt32(sLine.Substring(15));
                nTotalRows = 0;

                this.OpenConnection();
                this.BeginTransaction();

                bool bTablaEntontrada = false;
                while (true)
                {
                    // Se obtiene el nombre y cantidad de registros de cada tabla serializada
                    string sTableName = null;
                    sLine = sr.ReadLine();
                    if (sLine == null)
                    {
                        break;
                    }
                    else
                    {
                        if (sLine.Trim() == string.Empty)
                        {
                            break;
                        }
                    }
                    sTableName = sLine.Substring(7).Trim();
                    sLine = sr.ReadLine();
                    nRowCount = System.Convert.ToInt32(sLine.Substring(10));
                    if (nRowCount > 0)
                    {
                        nRows = 0;
                        bTablaEntontrada = false;
                        // Cargamos un data Adapter para obtener los sqlcomandos
                        // Obtenemos la Estructura de la tabla
                        SqlDataAdapter daTabla = new SqlDataAdapter("SELECT * FROM " + sTableName, this.Connection);
                        DataTable dtEsquema = null;
                        SqlCommand cmdSelect = null;
                        SqlCommand cmdInsert = null;
                        SqlCommand cmdUpdate = null;
                        daTabla.SelectCommand.Transaction = this.Transaction;
                        try
                        {
                            dtEsquema = new DataTable(sTableName);
                            daTabla.FillSchema(dtEsquema, SchemaType.Source);
                            qrEngine = new SqlCommandBuilder(daTabla);
                            cmdSelect = new SqlCommand();
                            cmdSelect.Connection = this.Connection;
                            cmdSelect.Transaction = this.Transaction;                            
                            //COMANDO INSERCION                            
                            cmdInsert = qrEngine.GetInsertCommand(true);
                            cmdInsert.Connection = this.Connection;
                            cmdInsert.Transaction = this.Transaction;
                            if (bUpdateCurrentRows)
                            {
                                //COMANDO SELECT UTILIZA PARA SABER SI SE HACE INSERT - UPDATE                            
                                cmdSelect = this.ObtenerSentenciaSelect(dtEsquema);
                                //COMANDO UPDATE
                                cmdUpdate = this.ObtenerSentenciaUpdate(dtEsquema);
                                cmdUpdate.Connection = this.Connection;
                                cmdUpdate.Transaction = this.Transaction;
                            }
                            else
                            {
                                //BORRAR TODOS LOS DATOS
                                //SOLO INSERCIONES
                                cmdSelect.CommandText = "DELETE FROM " + sTableName;
                                cmdSelect.ExecuteNonQuery();
                            }

                            // se obtienen los nombres de los campos
                            sLine = sr.ReadLine();
                            sFields = sLine.Split('|');

                            if (sListaColumnas == null)
                            {
                                sListaColumnas = new Dictionary<string, Int32>();
                            }
                            sListaColumnas.Clear();
                            // se obtienen los tipos de datos de las columnas
                            sLine = sr.ReadLine();
                            sFieldsTypes = null;
                            sFieldsTypes = new string[sFields.Length];
                            int iPosicionTipo = 0;

                            for (int iPos = 0; iPos <= sFields.Length - 1; iPos++)
                            {
                                if (sFields[iPos] != string.Empty)
                                {
                                    //Excepcion si el campo no existe
                                    if (dtEsquema.Columns.Contains(sFields[iPos]))
                                    {
                                        sListaColumnas.Add(sFields[iPos], iPos);
                                        sFieldsTypes[iPosicionTipo] = dtEsquema.Columns[sFields[iPos]].DataType.Name;
                                        iPosicionTipo++;
                                    }
                                    else
                                    {
                                        bTablaEntontrada = false;
                                        throw new Exception("ERROR ColumnaNOExiste, La columna: " + sFields[iPos] + " NO Existe en la tabla: " + dtEsquema.TableName);
                                    }
                                }
                            }                           
                            bTablaEntontrada = true;
                        }
                        catch (Exception ex)
                        {
                            if (!bDespreciarTablas)
                            {
                                throw ex;
                            }
                            else
                            {
                                if (ex.Message.StartsWith("ERROR ColumnaNOExiste"))
                                {
                                    throw ex;
                                }
                            }
                        }

                        // Se procesa cada fila que venga serializada en la cadena
                        sLine = sr.ReadLine();
                        
                        bool bInsertRecord = false;
                        while ((sLine != null))
                        {
                            if (sLine.Trim() == string.Empty)
                            {
                                break;
                            }
                            if (bTablaEntontrada)
                            {                                
                                // Se obtienen los valores que vienen en el registro
                                sValues = sLine.Split('|');

                                //Revisar si se realiza un Updata/Insert
                                bInsertRecord = true;
                                if (bUpdateCurrentRows)
                                {
                                    if (dtEsquema.PrimaryKey.Length > 0)
                                    {
                                        this.InicializarParametrosComando(cmdSelect);
                                        this.ConfigurarParametrosComando(cmdSelect, sValues, sListaColumnas, sTableName, sFieldsTypes);
                                        cmdSelect.Transaction = this.Transaction;
                                        try
                                        {
                                            if (Convert.ToInt32(cmdSelect.ExecuteScalar()) > 0)
                                            {
                                                bInsertRecord = false;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception("Error Ejecutando Select para la tabla: " + sTableName + " Error: " + ex.Message);
                                        }
                                    }
                                }

                                if (bInsertRecord)
                                {
                                    try
                                    {
                                        this.ConfigurarParametrosComando(cmdInsert, sValues, sListaColumnas, sTableName, sFieldsTypes);
                                        cmdInsert.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Error Ejecutando Insert para la tabla: " + sTableName + " Error: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (cmdUpdate == null)
                                        {
                                            throw new Exception("No existe el comando Update para la tabla: " + sTableName);
                                        }
                                        else
                                        {
                                            if (cmdUpdate.CommandText != "NO")
                                            {
                                                this.ConfigurarParametrosComando(cmdUpdate, sValues, sListaColumnas, sTableName, sFieldsTypes);
                                                cmdUpdate.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Error Ejecutando update para la tabla: " + sTableName + " Error: " + ex.Message);
                                    }
                                }

                                // Se registra el avance de la tabla
                                nRows += 1;
                                nTotalRows += 1;                                
                            }
                            // Se se lee el siguiente registro
                            sLine = sr.ReadLine();
                        }
                    }
                }
                sr.Close();
                this.CommitTransaction();
            }            
            catch (Exception ex)
            {
                this.RollbackTransaction();
                throw new Exception("Error Guardando datos: " + ex.Message);
            }
            finally
            {
                this.ReestablecerCulturaHiloEjecucion();
                this.CloseConnection();
                sr.Close();
            }
            return true;
        }

        private void InicializarParametrosComando(SqlCommand cmdComando)
        {
            foreach (SqlParameter prCom in cmdComando.Parameters)
            {
                prCom.Value = DBNull.Value;
            }
        }

        private void ConfigurarParametrosComando(SqlCommand cmdComando, string[] sValores, Dictionary<string, Int32> sColumnas, string sNombreTabla, string[] sTipoDatos)
        {
            foreach (SqlParameter prCom in cmdComando.Parameters)
            {
                //REVISAR SI EL PARAMETRO ESTA EN LA TABLA
                if (sColumnas.ContainsKey(prCom.SourceColumn))
                {
                    if (sColumnas[prCom.SourceColumn] > sValores.Length)
                    {
                        throw new Exception("El indice de la Columna " + prCom.ParameterName + " es mayor que el arreglo de valores ");
                    }
                    else
                    {
                        if (DBUtils.IsNull(sValores[sColumnas[prCom.SourceColumn]]))
                        {
                            if (!prCom.IsNullable)
                            {
                                throw new Exception("El valor de la Columna " + prCom.ParameterName + " De la tabla " + sNombreTabla + " NO puede ser nulo! ");
                            }
                            else
                            {
                                prCom.Value = DBNull.Value;
                            }
                        }
                        else
                        {
                            prCom.Value = GetColumnValue(sTipoDatos[sColumnas[prCom.SourceColumn]], sValores[sColumnas[prCom.SourceColumn]], prCom.SourceColumn);                        
                        }    
                    }
                }
                else
                {
                    //Enviar DBNUll
                    prCom.Value = DBNull.Value;                    
                }
            }
        }        

        private SqlCommand ObtenerSentenciaSelect(DataTable dtEsquema)
        {
            if (dtEsquema.PrimaryKey.Length == 0)
            {
                return null;
            }
            else
            {
                string sWhere = "";
                SqlCommand cnSelect = new SqlCommand();
                foreach (DataColumn cl in dtEsquema.PrimaryKey)
                {
                    sWhere = sWhere + "[" + cl.ColumnName + "] = @" + cl.ColumnName.Replace("-","").Replace(".","") + " AND ";
                    SqlParameter prParam = new SqlParameter("@" + cl.ColumnName.Replace("-", "").Replace(".", ""), cl.DataType);
                    prParam.SourceColumn = cl.ColumnName;
                    cnSelect.Parameters.Add(prParam);

                }
                sWhere = sWhere.Substring(0, sWhere.Length - 5);
                string sSentencia = "SELECT COUNT(*) FROM " + dtEsquema.TableName + " WHERE " + sWhere;
                cnSelect.CommandText = sSentencia;
                cnSelect.Connection = this.Connection;
                return cnSelect;
            }

        }

        private bool EsLLavePrimaria(DataColumn[] dtColumnas, DataColumn clColumna)
        {
            foreach (DataColumn clBuscar in dtColumnas)
            {
                if (clBuscar.ColumnName == clColumna.ColumnName)
                {
                    return true;
                }
            }
            return false;
        }

        private SqlCommand ObtenerSentenciaUpdate(DataTable dtEsquema)
        {
            string sColumnas = "";
            SqlCommand cnUpdate = new SqlCommand();
            foreach (DataColumn cl in dtEsquema.Columns)
            {
                if (!EsLLavePrimaria(dtEsquema.PrimaryKey, cl))
                {
                    sColumnas = sColumnas + "[" + cl.ColumnName + "] = @" + cl.ColumnName.Replace("-", "").Replace(".", "") + ",";
                    SqlParameter prParam = new SqlParameter("@" + cl.ColumnName.Replace("-", "").Replace(".", ""), cl.DataType);
                    prParam.SourceColumn = cl.ColumnName;
                    cnUpdate.Parameters.Add(prParam);
                }

            }
            if (sColumnas.Length > 0)
            {
                sColumnas = sColumnas.Substring(0, sColumnas.Length - 1);
            }
            else
            {
                cnUpdate.CommandText = "NO";
                return cnUpdate;                
            }
            cnUpdate.Connection = this.Connection;
            string sSentencia = "UPDATE " + dtEsquema.TableName + " SET " + sColumnas;
            if (dtEsquema.PrimaryKey.Length == 0)
            {                
                cnUpdate.CommandText = sSentencia;
                return cnUpdate;
            }
            else
            {
                string sWhere = "";
                SqlCommand cnSelect = new SqlCommand();
                foreach (DataColumn cl in dtEsquema.PrimaryKey)
                {
                    sWhere = sWhere + "[" + cl.ColumnName + "] = @" + cl.ColumnName.Replace("-", "").Replace(".", "") + " AND ";
                    SqlParameter prParam = new SqlParameter("@" + cl.ColumnName.Replace("-", "").Replace(".", ""), cl.DataType);
                    prParam.SourceColumn = cl.ColumnName;
                    cnUpdate.Parameters.Add(prParam);

                }
                sWhere = sWhere.Substring(0, sWhere.Length - 5);
                sSentencia = sSentencia + " WHERE " + sWhere;
                cnUpdate.CommandText = sSentencia;
                return cnUpdate;
            }

        }

    }
}
