using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Reflection;
using System.IO;

#if NETCF20
using MovilidadCF.Compression;
using Newtonsoft.Json.Linq;
namespace MovilidadCF.Data.SqlServerCe
{
    public class GestorDatosBase : MovilidadCF.Data.GestorDatosBase
    {
#else
using Desktop.Compression;
namespace Desktop.Data.SqlServerCe
{
    public class GestorDatosBase : Desktop.Data.GestorDatosBase
    {
#endif
        private DataTable m_dtPK = null;
        private DataView m_dvPK = null;


        public GestorDatosBase() : base() { }

        
        public GestorDatosBase(String ConnectionString)
            : base(ConnectionString) { }

        internal override DbConnection CreateConnection(string ConnectionString)
        {        
            return new SqlCeConnection(ConnectionString);
        }

        public override DbCommand CreateCommand()
        {
            return new SqlCeCommand();
        }

        public override void DeriveParameters(DbCommand cmd)
        {
            throw new NotSupportedException("DeriveParameters no es soportado en SQL Server CE");
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new SqlCeDataAdapter();
        }
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new SqlCeCommandBuilder();
        }

        public new SqlCeConnection Connection
        {
            get
            {
                return (SqlCeConnection)base.Connection;
            }
        }

        public new SqlCeTransaction Transaction
        {
            get
            {
                return (SqlCeTransaction)base.Transaction;
            }
            set
            {
                base.Transaction = value;
            }
        }

        protected override void SetCommandParameters(DbCommand cmd, object[] Parameters)
        {
            SqlCeCommand cmdSqlCe = (SqlCeCommand)cmd;
            for (int I = 0; I < Parameters.Length; I++)
            {
                cmdSqlCe.Parameters.Add(String.Format("@p{0}", (I + 1)), Parameters[I]);
            }
        }

        public override DataTable GetTables()
        {
            DataTable dt = new DataTable("TABLES");
            Fill(dt, CommandType.Text, "SELECT * FROM INFORMATION_SCHEMA.Tables");
            return dt;
        }

        /*private int CalcularTotalRegistroJSON(JArray datosServicio)
        {
            int resultado = 0;
            foreach (JObject content in datosServicio.Children<JObject>())
            {
                string nombreTable = content["tabla"].ToString();
                JArray array = JArray.Parse(content["lista"].ToString());
                resultado += array.Count;                                
            }

            return resultado;
        }*/

        private string NormalizarString(string stringJSON)
        {
            stringJSON = stringJSON.Replace(Convert.ToChar(34), ' ');
            return stringJSON.Trim();
        }

        /// <summary>
        /// Inserta los datos de un servicio JSON
        /// </summary>
        /// <param name="sSerializedData"></param>
        /// <param name="bUpdateCurrentRows"></param>
        /// <param name="Estado"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        /*public override bool SaveToDatabase(JArray datosServicio, bool bUpdateCurrentRows, DataSetSerializer.IEstadoCarga Estado, DataSet ds)
        {            
            string[] sFields = null;
            string[] sFieldsTypes = null;
            string[] sValues = null;
            SqlCeResultSet rs = null;
            SqlCeUpdatableRecord record = null;
            int I = 0;
            int J = 0;
            int nIndex = 0;
            int nTableCount = 0;
            int nRowCount = 0;
            int nTotalRowCount = 0;
            int nRows = 0;
            int nTotalRows = 0;
            DataTable dtNucleo = null;
            DataRow row = null;
            object FieldValue = null;
            bool bUpdateOriginal = bUpdateCurrentRows;
            try
            {
                // Se lee la liena con el n?mero de tablas y el numero total de filas a procesar                
                nTableCount = datosServicio.Count;                
                nTotalRowCount = this.CalcularTotalRegistroJSON(datosServicio);
                nTotalRows = 0;

                this.OpenConnection();
                if (bUpdateCurrentRows)
                    LoadPrimaryKeysInfo();
                
                    // Se obtiene el nombre y cantidad de registros de cada tabla
                foreach (JObject content in datosServicio.Children<JObject>())
                {
                    if (Estado.Cancelado)
                    {
                        break;
                    }

                    string sTableName = null;
                    sTableName = NormalizarString(content["tabla"].ToString());
                    JArray array = JArray.Parse(content["lista"].ToString());
                    nRowCount = array.Count;
                    if (nRowCount > 0)
                    {
                        nRows = 0;
                        Estado.IniciarTabla(sTableName);
                        bUpdateCurrentRows = bUpdateOriginal;

                        // Se revisa si es una tabla del nucleo y se actualiza
                        if (ds != null)
                        {
                            if (ds.Tables.IndexOf(sTableName) >= 0)
                            {
                                dtNucleo = ds.Tables[sTableName];
                            }
                            else
                            {
                                dtNucleo = null;
                            }
                        }
                        else
                        {
                            dtNucleo = null;
                        }

                        if (bUpdateCurrentRows)
                        {
                            // Se filtra la informaci?n del indice de llave primario, para la busqueda de
                            // de las filas actuales
                            m_dvPK.RowFilter = "TABLE_NAME = '" + sTableName + "'";
                        }
                        else
                        {
                            // Si es una tabla del nucleo si eliminan las filas actuales
                            if (dtNucleo != null)
                            {
                                dtNucleo.Rows.Clear();
                            }
                        }


                        // Se obtiene el objeto ResultSet por medio del cual se har? la actualizaci?n
                        // especificando el indice de llave primaria de la tabla
                        SqlCeCommand cmd = new SqlCeCommand();
                        cmd.Connection = this.Connection;
                        cmd.CommandType = CommandType.TableDirect;
                        cmd.CommandText = sTableName;
                        if (bUpdateCurrentRows)
                        {
                            //Si no tiene llave primaria insertar el registro
                            if (m_dvPK.Count > 0)
                            {
                                cmd.IndexName = System.Convert.ToString(m_dvPK[0]["CONSTRAINT_NAME"]);
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Sensitive | ResultSetOptions.Scrollable);
                            }
                            else
                            {
                                bUpdateCurrentRows = false;
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                            }
                        }
                        else
                        {
                            rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                        }

                        // se obtienen los nombres de los campos
                        // se obtienen los tipos de datos de las columnas
                        List<string> propiedades = new List<string>();
                        List<string> tipoDatos = new List<string>();

                        foreach (var llave in array.First)
                        {
                            propiedades.Add(((Newtonsoft.Json.Linq.JProperty)(llave)).Name);
                            tipoDatos.Add(llave.GetType().Name);
                        }

                        sFields = propiedades.ToArray();
                        sFieldsTypes = tipoDatos.ToArray();

                        bool bInsertRecord = false;
                        foreach (JToken valor in array)
                        {
                            if (Estado.Cancelado)
                            {
                                break;
                            }

                            List<string> valores = new List<string>();
                            foreach (string property in propiedades)
                            {
                                valores.Add(NormalizarString(valor[property].ToString()));
                            }

                            // Se obtienen los valores que vienen en el registro
                            sValues = valores.ToArray();

                            // Se obtienen los valores de llave primaria del registro
                            // Se crea la matriz de objetos para guardar los valores de la llave primaria de cada registro
                            bInsertRecord = true;
                            if (bUpdateCurrentRows)
                            {

                                // Se obtiene la llave primaria del registro
                                object[] RecordKey = new object[m_dvPK.Count];
                                for (I = 0; I < m_dvPK.Count; I++)
                                {
                                    for (J = 0; J < sFields.Length; J++)
                                    {
                                        if (System.Convert.ToString(m_dvPK[I]["COLUMN_NAME"]).ToUpper() == sFields[J].ToUpper())
                                        {
                                            RecordKey[I] = GetColumnValue(sFieldsTypes[J], sValues[J]);
                                        }
                                    }
                                }

                                // se busca el registro actual y luego se actualizan los datos
                                // si no se encuentra se inserta un nuevo registro
                                if (rs.Seek(DbSeekOptions.FirstEqual, RecordKey))
                                {
                                    bInsertRecord = false;

                                    // Se obtiene la fila a modificar
                                    rs.Read();
                                    if (dtNucleo != null)
                                    {
                                        row = dtNucleo.Rows.Find(RecordKey);
                                    }

                                    // Se actualizan los valores de cada columna en el registro en la base de datos y si
                                    // se esta procesando una tabla del nucleo tambien se actualiza en memoria
                                    if (dtNucleo != null && row != null)
                                    {
                                        for (I = 0; I < sFields.Length; I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                                nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                                if (nIndex >= 0)
                                                {
                                                    row[nIndex] = FieldValue;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (I = 0; I < sFields.Length; I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    rs.Update();
                                }
                            }

                            if (bInsertRecord)
                            {
                                // Se crea el nuevo registro
                                record = rs.CreateRecord();
                                if (dtNucleo != null)
                                {
                                    row = dtNucleo.NewRow();
                                }
                                else
                                {
                                    row = null;
                                }

                                // Se actualizan los valores de cada columna en el registro
                                if (dtNucleo != null && row != null)
                                {
                                    for (I = 0; I < sFields.Length; I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                            nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                            if (nIndex >= 0)
                                            {
                                                row[nIndex] = FieldValue;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    for (I = 0; I < sFields.Length; I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }

                                // Se almacena el nuevo registro
                                try
                                {
                                    rs.Insert(record, DbInsertOptions.KeepCurrentPosition);
                                    if (dtNucleo != null && row != null)
                                    {
                                        dtNucleo.Rows.Add(row);
                                        row.AcceptChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    object[] values = new object[rs.FieldCount + 1];
                                    record.GetValues(values);
                                    throw ex;
                                }
                            }

                            // Se registra el avance de la tabla
                            nRows += 1;
                            nTotalRows += 1;
                            if ((nRows % 100) == 0 || nRows == nRowCount)
                            {
                                Estado.ProgresoTabla = System.Convert.ToInt32((nRows * 100 / (double)nRowCount));
                                Estado.ProgresoTotal = System.Convert.ToInt32(nTotalRows * 100 / (double)nTotalRowCount);
                            }

                            // Se se lee el siguiente registro                                
                        }
                        rs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error procesando datos " + ex.Message);
            }
            finally
            {
                if (rs != null)
                {
                    if (!rs.IsClosed)
                    {
                        rs.Close();
                        rs = null;
                    }
                }
                this.CloseConnection();                
            }
            return true;
        }*/

        // Permite realizar la integraci?n de datos de a partir de un dataset serializado
        // El dataset debe contener tablas con nombres y campos iguales a los creados en la base de
        // datos
        internal override bool SaveToDatabase(string sSerializedData, bool bUpdateCurrentRows, DataSetSerializer.IEstadoCarga Estado, DataSet ds)
        {
            StringReader sr = new StringReader(sSerializedData);

            string sLine = null;
            string[] sFields = null;
            string[] sFieldsTypes = null;
            string[] sValues = null;
            SqlCeResultSet rs = null;
            SqlCeUpdatableRecord record = null;
            int I = 0;
            int J = 0;
            int nIndex = 0;
            int nTableCount = 0;
            int nRowCount = 0;
            int nTotalRowCount = 0;
            int nRows = 0;
            int nTotalRows = 0;            
            DataTable dtNucleo = null;
            DataRow row = null;
            object FieldValue = null;
            bool bUpdateOriginal = bUpdateCurrentRows;
            try
            {
                // Se lee la liena con el n?mero de tablas serializadas y el numero total de filas a procesar
                sLine = sr.ReadLine();
                nTableCount = System.Convert.ToInt32(sLine.Substring(12));
                sLine = sr.ReadLine();
                nTotalRowCount = System.Convert.ToInt32(sLine.Substring(15));
                nTotalRows = 0;

                this.OpenConnection();
                if (bUpdateCurrentRows)
                    LoadPrimaryKeysInfo();

                while (!Estado.Cancelado)
                {
                    // Se obtiene el nombre y cantidad de registros de cada tabla serializada
                    string sTableName = null;
                    sLine = sr.ReadLine();
                    if (sLine == null)
                    {
                        break;
                    }
                    sTableName = sLine.Substring(7);
                    sLine = sr.ReadLine();
                    nRowCount = System.Convert.ToInt32(sLine.Substring(10));
                    if (nRowCount > 0)
                    {
                        nRows = 0;
                        Estado.IniciarTabla(sTableName);
                        bUpdateCurrentRows = bUpdateOriginal;

                        // Se revisa si es una tabla del nucleo y se actualiza
                        if (ds != null)
                        {
                            if (ds.Tables.IndexOf(sTableName) >= 0)
                            {
                                dtNucleo = ds.Tables[sTableName];
                            }
                            else
                            {
                                dtNucleo = null;
                            }
                        }
                        else
                        {
                            dtNucleo = null;
                        }

                        if (bUpdateCurrentRows)
                        {
                            // Se filtra la informaci?n del indice de llave primario, para la busqueda de
                            // de las filas actuales
                            m_dvPK.RowFilter = "TABLE_NAME = '" + sTableName + "'";
                        }
                        else
                        {
                            // Si es una tabla del nucleo si eliminan las filas actuales
                            if (dtNucleo != null)
                            {
                                dtNucleo.Rows.Clear();
                            }
                        }


                        // Se obtiene el objeto ResultSet por medio del cual se har? la actualizaci?n
                        // especificando el indice de llave primaria de la tabla
                        SqlCeCommand cmd = new SqlCeCommand();
                        cmd.Connection = this.Connection;
                        cmd.CommandType = CommandType.TableDirect;
                        cmd.CommandText = sTableName;                        
                        if (bUpdateCurrentRows)
                        {
                            //Si no tiene llave primaria insertar el registro
                            if (m_dvPK.Count > 0)
                            {
                                cmd.IndexName = System.Convert.ToString(m_dvPK[0]["CONSTRAINT_NAME"]);
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Sensitive | ResultSetOptions.Scrollable);
                            }
                            else
                            {
                                bUpdateCurrentRows = false;
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                            }     
                        }
                        else
                        {
                            rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                        }

                        // se obtienen los nombres de los campos
                        sLine = sr.ReadLine();
                        sFields = sLine.Split('|');

                        // se obtienen los tipos de datos de las columnas
                        sLine = sr.ReadLine();
                        sFieldsTypes = sLine.Split('|');

                        // Se procesa cada fila que venga serializada en la cadena
                        sLine = sr.ReadLine();

                        bool bInsertRecord = false;
                        while ((sLine != null) & (!Estado.Cancelado))
                        {
                            if (sLine.Trim() == string.Empty)
                            {
                                break;
                            }

                            // Se obtienen los valores que vienen en el registro
                            sValues = sLine.Split('|');

                            // Se obtienen los valores de llave primaria del registro
                            // Se crea la matriz de objetos para guardar los valores de la llave primaria de cada registro
                            bInsertRecord = true;
                            if (bUpdateCurrentRows)
                            {

                                // Se obtiene la llave primaria del registro
                                object[] RecordKey = new object[m_dvPK.Count];
                                for (I = 0; I < m_dvPK.Count; I++)
                                {
                                    for (J = 0; J < sFields.GetUpperBound(0); J++)
                                    {
                                        if (System.Convert.ToString(m_dvPK[I]["COLUMN_NAME"]).ToUpper() == sFields[J].ToUpper())
                                        {
                                            RecordKey[I] = GetColumnValue(sFieldsTypes[J], sValues[J]);
                                        }
                                    }
                                }

                                // se busca el registro actual y luego se actualizan los datos
                                // si no se encuentra se inserta un nuevo registro
                                if (rs.Seek(DbSeekOptions.FirstEqual, RecordKey))
                                {
                                    bInsertRecord = false;

                                    // Se obtiene la fila a modificar
                                    rs.Read();
                                    if (dtNucleo != null)
                                    {
                                        row = dtNucleo.Rows.Find(RecordKey);
                                    }

                                    // Se actualizan los valores de cada columna en el registro en la base de datos y si
                                    // se esta procesando una tabla del nucleo tambien se actualiza en memoria
                                    if (dtNucleo != null && row != null)
                                    {
                                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                                nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                                if (nIndex >= 0)
                                                {
                                                    row[nIndex] = FieldValue;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    rs.Update();
                                }
                            }

                            if (bInsertRecord)
                            {
                                // Se crea el nuevo registro
                                record = rs.CreateRecord();
                                if (dtNucleo != null)
                                {
                                    row = dtNucleo.NewRow();
                                }
                                else
                                {
                                    row = null;
                                }

                                // Se actualizan los valores de cada columna en el registro
                                if (dtNucleo != null && row != null)
                                {
                                    for (I = 0; I < sFields.GetUpperBound(0); I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                            nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                            if (nIndex >= 0)
                                            {
                                                row[nIndex] = FieldValue;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    for (I = 0; I < sFields.GetUpperBound(0); I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }

                                // Se almacena el nuevo registro
                                try
                                {
                                    rs.Insert(record, DbInsertOptions.KeepCurrentPosition);
                                    if (dtNucleo != null && row != null)
                                    {
                                        dtNucleo.Rows.Add(row);
                                        row.AcceptChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    object[] values = new object[rs.FieldCount + 1];
                                    record.GetValues(values);
                                    throw ex;
                                }
                            }

                            // Se registra el avance de la tabla
                            nRows += 1;
                            nTotalRows += 1;
                            if ((nRows % 100) == 0 || nRows == nRowCount)
                            {
                                Estado.ProgresoTabla = System.Convert.ToInt32((nRows * 100 / (double)nRowCount));
                                Estado.ProgresoTotal = System.Convert.ToInt32(nTotalRows * 100 / (double)nTotalRowCount);
                            }

                            // Se se lee el siguiente registro
                            sLine = sr.ReadLine();
                        }
                        rs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error procesando datos " + ex.Message);
            }
            finally
            {
                if (rs != null)
                {
                    if (!rs.IsClosed)
                    {
                        rs.Close();
                        rs = null;
                    }
                }
                this.CloseConnection();
                sr.Close();
            }
            return true;
        }

        internal override bool SaveCompressToDatabase(string sSerializedData, bool bUpdateCurrentRows, DataSetSerializer.IEstadoCarga Estado, DataSet ds)
        {
            return SaveCompressToDatabase(sSerializedData, bUpdateCurrentRows, Estado, ds, true);
        }

        internal override bool SaveCompressToDatabase(string sSerializedData, bool bUpdateCurrentRows, DataSetSerializer.IEstadoCarga Estado, DataSet ds, bool LanzarExcepcion)
        {
            Byte[] bDatosEnviados = Convert.FromBase64String(sSerializedData);
            DataCompression.UnCompressText(bDatosEnviados, bDatosEnviados.Length);
            string sLine = null;
            string[] sFields = null;
            string[] sFieldsTypes = null;
            string[] sValues = null;
            SqlCeResultSet rs = null;
            SqlCeUpdatableRecord record = null;
            int I = 0;
            int J = 0;
            int nIndex = 0;
            int nTableCount = 0;
            int nRowCount = 0;
            int nTotalRowCount = 0;
            int nRows = 0;
            int nTotalRows = 0;            
            DataTable dtNucleo = null;
            DataRow row = null;
            object FieldValue = null;
            bool bUpdateOriginal = bUpdateCurrentRows;
            try
            {
                // Se lee la liena con el n?mero de tablas serializadas y el numero total de filas a procesar
                sLine = DataCompression.ReadLine();
                nTableCount = System.Convert.ToInt32(sLine.Substring(12));
                sLine = DataCompression.ReadLine();
                nTotalRowCount = System.Convert.ToInt32(sLine.Substring(15));
                nTotalRows = 0;

                this.OpenConnection();
                if (bUpdateCurrentRows)
                    LoadPrimaryKeysInfo();

                while (true)//(!Estado.Cancelado)
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
                        Estado.IniciarTabla(sTableName);
                        bUpdateCurrentRows = bUpdateOriginal;

                        // Se revisa si es una tabla del nucleo y se actualiza
                        if (ds != null)
                        {
                            if (ds.Tables.IndexOf(sTableName) >= 0)
                            {
                                dtNucleo = ds.Tables[sTableName];
                            }
                            else
                            {
                                dtNucleo = null;
                            }
                        }
                        else
                        {
                            dtNucleo = null;
                        }

                        if (bUpdateCurrentRows)
                        {
                            // Se filtra la informaci?n del indice de llave primario, para la busqueda de
                            // de las filas actuales
                            m_dvPK.RowFilter = "TABLE_NAME = '" + sTableName + "'";
                        }
                        else
                        {
                            // Si es una tabla del nucleo si eliminan las filas actuales
                            if (dtNucleo != null)
                            {
                                dtNucleo.Rows.Clear();
                            }
                        }


                        // Se obtiene el objeto ResultSet por medio del cual se har? la actualizaci?n
                        // especificando el indice de llave primaria de la tabla
                        SqlCeCommand cmd = new SqlCeCommand();
                        cmd.Connection = this.Connection;
                        cmd.CommandType = CommandType.TableDirect;
                        cmd.CommandText = sTableName;                        
                        if (bUpdateCurrentRows)
                        {
                            //Si no tiene llave primaria insertar el registro
                            if (m_dvPK.Count > 0)
                            {
                                cmd.IndexName = System.Convert.ToString(m_dvPK[0]["CONSTRAINT_NAME"]);
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Sensitive | ResultSetOptions.Scrollable);
                            }
                            else
                            {
                                bUpdateCurrentRows = false;
                                rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                            }                            
                        }
                        else
                        {
                            rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable);
                        }

                        // se obtienen los nombres de los campos
                        sLine = DataCompression.ReadLine();
                        sFields = sLine.Split('|');

                        // se obtienen los tipos de datos de las columnas
                        sLine = DataCompression.ReadLine();
                        sFieldsTypes = sLine.Split('|');

                        // Se procesa cada fila que venga serializada en la cadena
                        sLine = DataCompression.ReadLine();

                        bool bInsertRecord = false;
                        while ((sLine != null))// & (!Estado.Cancelado))
                        {
                            if (sLine.Trim() == string.Empty)
                            {
                                break;
                            }

                            // Se obtienen los valores que vienen en el registro
                            sValues = sLine.Split('|');

                            // Se obtienen los valores de llave primaria del registro
                            // Se crea la matriz de objetos para guardar los valores de la llave primaria de cada registro
                            bInsertRecord = true;
                            if (bUpdateCurrentRows)
                            {

                                // Se obtiene la llave primaria del registro
                                object[] RecordKey = new object[m_dvPK.Count];
                                for (I = 0; I < m_dvPK.Count; I++)
                                {
                                    for (J = 0; J < sFields.GetUpperBound(0); J++)
                                    {
                                        if (System.Convert.ToString(m_dvPK[I]["COLUMN_NAME"]).ToUpper() == sFields[J].ToUpper())
                                        {
                                            RecordKey[I] = GetColumnValue(sFieldsTypes[J], sValues[J]);
                                        }
                                    }
                                }

                                // se busca el registro actual y luego se actualizan los datos
                                // si no se encuentra se inserta un nuevo registro
                                if (rs.Seek(DbSeekOptions.FirstEqual, RecordKey))
                                {
                                    bInsertRecord = false;

                                    // Se obtiene la fila a modificar
                                    rs.Read();
                                    if (dtNucleo != null)
                                    {
                                        row = dtNucleo.Rows.Find(RecordKey);
                                    }

                                    // Se actualizan los valores de cada columna en el registro en la base de datos y si
                                    // se esta procesando una tabla del nucleo tambien se actualiza en memoria
                                    if (dtNucleo != null && row != null)
                                    {
                                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                                nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                                if (nIndex >= 0)
                                                {
                                                    row[nIndex] = FieldValue;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                                        {
                                            try
                                            {
                                                nIndex = rs.GetOrdinal(sFields[I]);
                                                FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                                rs.SetValue(nIndex, FieldValue);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                    rs.Update();
                                }
                            }

                            if (bInsertRecord)
                            {
                                // Se crea el nuevo registro
                                record = rs.CreateRecord();
                                if (dtNucleo != null)
                                {
                                    row = dtNucleo.NewRow();
                                }
                                else
                                {
                                    row = null;
                                }

                                // Se actualizan los valores de cada columna en el registro
                                if (dtNucleo != null && row != null)
                                {
                                    for (I = 0; I < sFields.GetUpperBound(0); I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                            nIndex = row.Table.Columns.IndexOf(sFields[I]);
                                            if (nIndex >= 0)
                                            {
                                                row[nIndex] = FieldValue;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    for (I = 0; I < sFields.GetUpperBound(0); I++)
                                    {
                                        try
                                        {
                                            nIndex = rs.GetOrdinal(sFields[I]);
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues[I]);
                                            record.SetValue(nIndex, FieldValue);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new InvalidOperationException("Field: " + sFields[I] + "\r\n" + "Type: " + rs.GetFieldType(nIndex).ToString() + "\r\n" + "Value: " + sValues[I] + "\r\n" + ex.Message);
                                        }
                                    }
                                }

                                // Se almacena el nuevo registro
                                try
                                {
                                    rs.Insert(record, DbInsertOptions.KeepCurrentPosition);
                                    if (dtNucleo != null && row != null)
                                    {
                                        dtNucleo.Rows.Add(row);
                                        row.AcceptChanges();
                                    }
                                }
                                catch (Exception ex)
                                {                                    
                                    if (LanzarExcepcion)
                                    {
                                        //object[] values = new object[rs.FieldCount + 1];
                                        //record.GetValues(values);
                                        throw ex;
                                    }
                                }
                            }

                            // Se registra el avance de la tabla
                            nRows += 1;
                            if (nRows == 448)
                            {
                                nRows = 448;
                            }
                            nTotalRows += 1;
                            if ((nRows % 100) == 0 || nRows == nRowCount)
                            {
                                Estado.ProgresoTabla = System.Convert.ToInt32((nRows * 100 / (double)nRowCount));
                                Estado.ProgresoTotal = System.Convert.ToInt32(nTotalRows * 100 / (double)nTotalRowCount);
                            }

                            // Se se lee el siguiente registro
                            sLine = DataCompression.ReadLine();
                        }                        
                    }
                }
            }
            finally
            {
                this.CloseConnection();
                DataCompression.Close();
            }
            return true;
        }

        public void LoadPrimaryKeysInfo()
        {
            // Se obtiene la informaci?n de los indices de llave primaria de las tablas en 
            // la base de datoss
            m_dtPK = new DataTable("Indices");            
            try
            {
                OpenConnection();

                Fill(m_dtPK, CommandType.Text,
                    "SELECT TABLE_NAME, CONSTRAINT_NAME, COLUMN_NAME, ORDINAL_POSITION " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                    "WHERE CONSTRAINT_NAME LIKE 'PK%' ORDER BY TABLE_NAME, ORDINAL_POSITION");
                m_dvPK = new DataView(m_dtPK);
            }
            catch (Exception ex)
            {
                //MovilidadCF.Logging.Logger.Write(ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Function utilitaria utilizada por la funci?n de integraci?n para llenar los parametros
        // de un objeto SqlCommand obtenido de un datadapter
        protected static void FillParameters(SqlCeCommand cmd, DataRow row)
        {
            int I = 0;
            for (I = 0; I < cmd.Parameters.Count; I++)
            {
                cmd.Parameters[I].Value = row[cmd.Parameters[I].SourceColumn];
            }
        }    

    }
}
