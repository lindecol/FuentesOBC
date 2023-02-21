//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.IO;
using System.Globalization;
#if NETCF20
using MovilidadCF.Data;
namespace MovilidadCF.Data
{
#else
using Desktop.Data;
namespace Desktop.Data
{
#endif

    public class DataSetSerializer
    {
        private GestorDatosBase m_Gestor = null;


        // Interfaz que permite mostrar al usuario el estado de un proceso
        // de comunicaciones. La clase que implemente esta interfaz debe
        // Proporcionar formas visuales de mostrar los camibos en las
        // propiedades o mostrar la información de nuevas actividades 
        // La clase también debe implementar la propiedades que dan
        // información sobre el programa y correria sobre los cuales
        // aplica el proceso de comunicaciones.

        #region Tipos de datos adicionales utilizados por la clase

        public interface IEstadoCarga
        {
            // Propiedad que determina si el usuario decidio cancelar el proceso
            bool Cancelado { get;}

            // Progeso actividad actual en porcentaje
            int ProgresoTabla { set;}

            // Progreso total
            int ProgresoTotal { set;}

            // Indica el inicio de proceso de una nueva tabla
            void IniciarTabla(string sNombreTabla);

        }


        public class Query
        {
            public string m_TableName;
            public string m_Query;

            public Query(string TableName, string Query)
            {
                m_TableName = TableName;
                m_Query = Query;
            }

            public string TableName
            {
                get
                {
                    return m_TableName;
                }
            }

            public string SQLCommand
            {
                get
                {
                    return m_Query;
                }
            }

        }

        public class QueryList : List<Query>
        {
            public void Add(string TableName, string SQLCommand)
            {
                Query q = new Query(TableName, SQLCommand);
                this.Add(q);
            }
        }

        #endregion

        public DataSetSerializer()
        {

        }

        public DataSetSerializer(GestorDatosBase gestor)
        {
            this.m_Gestor = gestor;
        }

        public static string Serialize(DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;
            int I = 0;
            int nTableCount = 0;
            int nTotalRowsCount = 0;

            for (I = 0; I < ds.Tables.Count; I++)
            {
                if (ds.Tables[I].Rows.Count > 0)
                {
                    nTableCount += 1;
                    nTotalRowsCount += ds.Tables[I].Rows.Count;
                }
            }

            sb.Append("TABLECOUNT: " + nTableCount.ToString() + "\r\n");
            sb.Append("TOTALROWCOUNT: " + nTotalRowsCount.ToString() + "\r\n");

            //ds.WriteXml("D:\\Indupalma\\p.xml");

            for (I = 0; I < ds.Tables.Count; I++)
            {
                dt = ds.Tables[I];
                if (dt.Rows.Count > 0)
                {
                    sb.Append("TABLE: " + dt.TableName + "\r\n");
                    sb.Append("ROWCOUNT: " + dt.Rows.Count.ToString() + "\r\n");

                    // Se agrega una primer linea con los nombres de los campos
                    foreach (DataColumn col in dt.Columns)
                    {
                        sb.Append(col.ColumnName + "|");
                    }
                    sb.Append("\r\n");

                    // Se agrega una segunda linea con los tipos de datos
                    foreach (DataColumn col in dt.Columns)
                    {
                        sb.Append(col.DataType.ToString() + "|");
                    }
                    sb.Append("\r\n");

                    // Se agregan lineas con los datos de todos los registros
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int J = 0; J < dt.Columns.Count; J++)
                        {
                            if (row.IsNull(J))
                            {
                                sb.Append("(null)|");
                            }
                            else
                            {
                                sb.Append(GetColumnValueSerializar(dt.Columns[J].DataType.Name, row[J], dt.Columns[J].ColumnName) + "|");                                                                
                            }
                        }
                        sb.Append("\r\n");
                    }

                    if ( I < ds.Tables.Count - 1 )
                        sb.Append("\r\n"); // Se agrega una linea adicional para delimitar los datos de la tabla
                }
            }
            return sb.ToString();
        }

        public static DataSet Unserialize(DataSet ds, string sSerializedData)
        {
            StringReader sr = new StringReader(sSerializedData);
            string sLine = null;
            string[] sFields = null;
            string[] sFieldsTypes = null;
            string[] sValues = null;
            string sTableName = null;
            int nTableCount = 0;
            int nTotalRowCount = 0;
            int nRowCount = 0;
            DataRow row = null;
            int I = 0;
            try
            {
                 #if !NETCF20
                    //Personalizar Hilo Ejecucion
                    //PersonalizarCulturaHiloEjecucion();
                #endif

                // Se lee la liena con el número de tablas serializadas
                sLine = sr.ReadLine();
                nTableCount = System.Convert.ToInt32(sLine.Substring(12));
                sLine = sr.ReadLine();
                nTotalRowCount = System.Convert.ToInt32(sLine.Substring(15));

                do
                {
                    // Se obtiene el nombre de cada tabla serializada y el número de registros de cada una
                    sLine = sr.ReadLine();
                    if (sLine == null)
                    {
                        break;
                    }
                    sTableName = sLine.Substring(7);
                    sLine = sr.ReadLine();
                    nRowCount = System.Convert.ToInt32(sLine.Substring(10));


                    // se obtienen los nombres de los campos
                    sLine = sr.ReadLine();
                    sFields = sLine.Split('|');

                    // se obtienen los tipos de datos
                    sLine = sr.ReadLine();
                    sFieldsTypes = sLine.Split('|');

                    DataTable dt = null;
                    if (ds.Tables.IndexOf(sTableName) >= 0)
                    {
                        dt = ds.Tables[sTableName];
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.TableName = sTableName;
                        // Se configuran los campos del datatable
                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                        {
                            dt.Columns.Add(sFields[I], Type.GetType(sFieldsTypes[I]));
                        }
                    }

                    // Se obtienen las datos de las filas y se agregan
                    sLine = sr.ReadLine();
                    while (sLine != null)
                    {
                        if (sLine.Trim() == string.Empty)
                        {
                            break;
                        }
                        sValues = sLine.Split('|');
                        row = dt.NewRow();
                        for (I = 0; I < sFields.GetUpperBound(0); I++)
                        {
                            if (sValues[I] != "(null)")
                            {
                                row[sFields[I]] = GetColumnValue(sFieldsTypes[I], sValues[I], sFields[I]);                                
                            }
                        }
                        dt.Rows.Add(row);
                        sLine = sr.ReadLine();
                    }                    
                    if (dt.DataSet == null)
                        ds.Tables.Add(dt);
                } while (true);
            }
            finally
            {
                #if !NETCF20
                    //ReestablecerCulturaHiloEjecucion();
                #endif
                sr.Close();
            }
            return ds;
        }
        
        // Permite realizar la integración de datos de a partir de un dataset serializado
        // El dataset debe contener tablas con nombres y campos iguales a los creados en la base de
        // datos
        public bool SaveToDatabase(string sSerializedData, bool bUpdateCurrentRows, IEstadoCarga Estado, DataSet ds)
        {
            return m_Gestor.SaveToDatabase(sSerializedData, bUpdateCurrentRows, Estado, ds);
        }

        public bool SaveToDatabase(string sSerializedData, bool bUpdateCurrentRows)
        {
            return m_Gestor.SaveToDatabase(sSerializedData, bUpdateCurrentRows, false);
        }

        public bool SaveToDatabase(string sSerializedData, bool bUpdateCurrentRows, bool bDespreciarTablas)
        {
            return m_Gestor.SaveToDatabase(sSerializedData, bUpdateCurrentRows, bDespreciarTablas);
        }

        public bool SaveCompressToDatabase(string sSerializedData, bool bUpdateCurrentRows, IEstadoCarga Estado, DataSet ds, bool LanzarExcepcion)
        {
            return m_Gestor.SaveCompressToDatabase(sSerializedData, bUpdateCurrentRows, Estado, ds, LanzarExcepcion);
        }

        public bool SaveCompressToDatabase(string sSerializedData, bool bUpdateCurrentRows, IEstadoCarga Estado, DataSet ds)
        {
            return m_Gestor.SaveCompressToDatabase(sSerializedData, bUpdateCurrentRows, Estado, ds);
        }
        
        public bool SaveCompressToDatabase(string sSerializedData, bool bUpdateCurrentRows)
        {
            return m_Gestor.SaveCompressToDatabase(sSerializedData, bUpdateCurrentRows);
        }

        public string GetFromDatabase(QueryList lstQuery, IEstadoCarga Estado)
        {
            StringBuilder sb = new StringBuilder();
            IDataReader dr = null;
            IDbCommand cmd = m_Gestor.CreateCommand();
            int I = 0;
            int J = 0;
            int nIndexRowCountReplace = 0;
            int nTableCount = 0;
            int nRowCount = 0;
            int nTotalRowsCount = 0;

            // Se adicionan las 2 primeras lineas con tags para poder remplazarlos al final
            sb.Append("TABLECOUNT: <<TABLECOUNT>>" + "\r\n");
            sb.Append("TOTALROWCOUNT: <<TOTALROWCOUNT>>" + "\r\n");
            nTableCount = 0;
            nTotalRowsCount = 0;
            m_Gestor.OpenConnection();
            try
            {
                for (I = 0; I < lstQuery.Count; I++)
                {
                    if (Estado.Cancelado)
                    {
                        break;
                    }
                    Estado.IniciarTabla(lstQuery[I].TableName.ToUpper());
                    cmd.Connection = m_Gestor.Connection;
                    cmd.CommandText = lstQuery[I].SQLCommand;
                    if (cmd.CommandText.TrimStart().StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
                    {
                        cmd.CommandType = CommandType.Text;
                    }
                    else
                    {
                        cmd.CommandType = CommandType.TableDirect;
                    }                    
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        nTableCount += 1;
                        sb.Append("TABLE: " + lstQuery[I].TableName + "\r\n");
                        nIndexRowCountReplace = sb.Length;
                        sb.Append("ROWCOUNT: <<ROWCOUNT>> " + "\r\n");

                        // Se agrega una primer linea con los nombres de los campos
                        for (J = 0; J < dr.FieldCount; J++)
                        {
                            sb.Append(dr.GetName(J) + "|");
                        }
                        sb.Append("\r\n");

                        // Se agrega una segunda linea con los tipos de datos
                        for (J = 0; J < dr.FieldCount; J++)
                        {
                            sb.Append(dr.GetFieldType(J).ToString() + "|");
                        }
                        sb.Append("\r\n");

                        // Se agregan lineas con los datos de todos los registros
                        nRowCount = 0;
                        do
                        {
                            if (Estado.Cancelado)
                            {
                                break;
                            }
                            for (J = 0; J < dr.FieldCount; J++)
                            {
                                /*if (dr.IsDBNull(J))
                                {
                                    sb.Append("(null)|");
                                }
                                else
                                {
                                    if (dr.GetFieldType(J) == typeof(System.DateTime))
                                    {
                                        sb.Append(dr.GetDateTime(J).ToString("yyyy/MM/dd HH:mm:ss") + "|");
                                    }
                                    else
                                    {
                                        sb.Append(System.Convert.ToString(dr.GetValue(J)) + "|");
                                    }
                                }*/
                                if (dr.IsDBNull(J))
                                {
                                    //sb.Append(" |");
                                    sb.Append("(null)|");
                                }
                                else
                                {
                                    if (dr.GetValue(J).ToString().Trim() == string.Empty)
                                    {
                                        sb.Append(GetColumnValueSerializar(dr.GetFieldType(J).Name, " ", dr.GetName(J)) + "|");
                                    }
                                    else
                                    {
                                        sb.Append(GetColumnValueSerializar(dr.GetFieldType(J).Name, dr.GetValue(J), dr.GetName(J)) + "|");
                                    }
                                }
                            }
                            sb.Append("\r\n");
                            nRowCount += 1;
                            nTotalRowsCount += 1;
                        } while (dr.Read());
                        dr.Close();
                        sb.Replace("<<ROWCOUNT>>", nRowCount.ToString(), nIndexRowCountReplace, 40);
                        if (I < lstQuery.Count - 1)
                            sb.Append("\r\n"); // Se agrega una linea adicional para delimitar los datos de la tabla
                    }
                }
                sb.Replace("<<TABLECOUNT>>", nTableCount.ToString(), 0, 40);
                if (sb.Length < 60)
                {
                    sb.Replace("<<TOTALROWCOUNT>>", nTotalRowsCount.ToString(), 0, sb.Length);
                }
                else
                {
                    sb.Replace("<<TOTALROWCOUNT>>", nTotalRowsCount.ToString(), 0, 60);
                }
                //Si tiene 0 registros devolver null
                if (nTotalRowsCount == 0)
                {
                    return null;
                }


            }
            finally
            {
                m_Gestor.CloseConnection();
            }
            return sb.ToString();
        }

        #region "Metodos para manejar la configuración regional"        

        internal static string GetColumnValueSerializar(string sFieldType, object Value, string sColumna)
        {
            string valorNumero;
            try
            {
                if (!sFieldType.StartsWith("System."))
                {
                    sFieldType = "System." + sFieldType;
                }                

                switch (sFieldType)
                {
                    case "System.String":
                        StringBuilder sbTexto = new StringBuilder(Convert.ToString(Value));
                        sbTexto.Replace("\r\n", "<CRLF>");
                        sbTexto.Replace("\r", "<CR>");
                        sbTexto.Replace("\n", "<LF>");
                        return sbTexto.ToString();
                    case "System.DateTime":
                        return Convert.ToDateTime(Value).ToString("yyyy/MM/dd HH:mm:ss");                            
                    case "System.Byte[]":
                        return Convert.ToBase64String((byte[])Value);
                    case "System.Int32":
                        return Convert.ToString(Value);
                    case "System.Double":
                        valorNumero = Value.ToString();
                        if (m_sSeparadorDecimales == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                        {
                            return valorNumero;
                        }
                        else
                        {
                            int posicionDecimales = valorNumero.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                            string Entero = "";
                            string Decimal = "";
                            if (posicionDecimales > 0)
                            {
                                Entero = valorNumero.Substring(0, posicionDecimales);
                                Decimal = valorNumero.Substring(posicionDecimales + 1);
                                valorNumero = Entero + m_sSeparadorDecimales + Decimal;
                            }                                
                            return valorNumero;
                        }                        
                    case "System.Decimal":
                        valorNumero = Value.ToString();
                        if (m_sSeparadorDecimales == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                        {                               
                            return valorNumero;
                        }
                        else
                        {
                            int posicionDecimales = valorNumero.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                            string Entero = "";
                            string Decimal = "";
                            if (posicionDecimales > 0)
                            {
                                Entero = valorNumero.Substring(0, posicionDecimales);
                                Decimal = valorNumero.Substring(posicionDecimales + 1);
                                valorNumero = Entero + m_sSeparadorDecimales + Decimal;
                            }                                
                            return valorNumero;
                        }                        
                    case "System.Int16":
                        return Convert.ToString(Value);
                    case "System.Boolean":
                        if (Convert.ToString(Value) == ValorTrue)
                        {
                            return "true";
                        }
                        else
                        {
                            if (Convert.ToString(Value) == ValorFalse)
                            {
                                return "false";
                            }
                            else
                            {
                                if (Convert.ToString(Value) == "")
                                {
                                    return "false";
                                }
                                else
                                {
                                    return Convert.ToString(Value);
                                }
                            }
                        }
                    default:
                        return Convert.ToString(Value);
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("Error convirtiendo datos Serializar, Tipo: " + sFieldType + " Valor:" + Value + " Columna:" + sColumna);
            }
        }

        /// <summary>
        /// Funcion para retornar el valor especifico de acuerdo al tipo
        /// </summary>
        /// <param name="sFieldType">Tipo Dato</param>
        /// <param name="sValue">Valor</param>
        /// <param name="sColumna">NombreColumna</param>
        /// <returns>El valor a ser insertado en el dataset</returns>
        internal static object GetColumnValue(string sFieldType, string sValue, string sColumna)
        {
            try
            {
                if (sValue == "(null)")
                {
                    return DBNull.Value;
                }
                else
                {
                    if (!sFieldType.StartsWith("System."))
                    {
                        sFieldType = "System." + sFieldType;
                    }
                    switch (sFieldType)
                    {
                        case "System.String":
                            StringBuilder sbTexto = new StringBuilder(Convert.ToString(sValue));
                            sbTexto.Replace("<CRLF>", "\r\n");
                            sbTexto.Replace("<CR>", "\r");
                            sbTexto.Replace("<LF>", "\n" );
                            return sbTexto.ToString();
                        case "System.DateTime":
                            return DateTime.ParseExact(sValue, FormatoFecha, null);
                        case "System.Byte[]":
                            return Convert.FromBase64String(sValue);
                        case "System.Int32":
                            return Int32.Parse(sValue);
                        case "System.Double":
                            if (m_sSeparadorDecimales == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                            {
                                return Double.Parse(sValue);
                            }
                            else
                            {
                                int posicionDecimales = sValue.IndexOf(m_sSeparadorDecimales);
                                string Entero = "";
                                string Decimal = "";
                                if (posicionDecimales > 0)
                                {
                                    Entero = sValue.Substring(0, posicionDecimales);
                                    Decimal = sValue.Substring(posicionDecimales + 1);
                                    sValue = Entero + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + Decimal;
                                }
                                return Double.Parse(sValue);
                            }                                                     
                        case "System.Decimal":
                            if (m_sSeparadorDecimales == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                            {
                                return Double.Parse(sValue);
                            }
                            else
                            {
                                int posicionDecimales = sValue.IndexOf(m_sSeparadorDecimales);
                                string Entero = "";
                                string Decimal = "";
                                if (posicionDecimales > 0)
                                {
                                    Entero = sValue.Substring(0, posicionDecimales);
                                    Decimal = sValue.Substring(posicionDecimales + 1);
                                    sValue = Entero + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + Decimal;
                                }
                                return Double.Parse(sValue);
                            }                                                                                 
                        case "System.Int16":
                            return Int16.Parse(sValue);
                        case "System.Boolean":
                            if (sValue == ValorTrue)
                            {
                                return true;
                            }
                            else
                            {
                                if (sValue == ValorFalse)
                                {
                                    return false;
                                }
                                else
                                {
                                    if (sValue == "")
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        return bool.Parse(sValue);
                                    }
                                }
                            }
                        default:
                            return sValue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error convirtiendo datos GetColumnValue, Tipo: " + sFieldType + " Valor:" + sValue + " Columna:" + sColumna);
            }
        }
    
        /// <summary>
        /// Miembro para utilizar formatos de fecha estandar
        /// </summary>
        private static string m_sFormatoFecha = "yyyy/MM/dd HH:mm:ss";

        public static string FormatoFecha
        {
            get { return m_sFormatoFecha; }
            set { m_sFormatoFecha = value; }
        }

        /// <summary>
        /// Miembro para parametrizar como se van a almacenar los datos boleanos
        /// </summary>
        private static string m_sValorTrue = "true";

        public static string ValorTrue
        {
            get { return m_sValorTrue; }
            set { m_sValorTrue = value; }
        }

        private static string m_sValorFalse = "false";

        /// <summary>
        /// Miembro para parametrizar como se van a almacenar los datos boleanos
        /// </summary>
        public static string ValorFalse
        {
            get { return m_sValorFalse; }
            set { m_sValorFalse = value; }
        }        
        
        /// <summary>
        /// Miembro para configurar el separador de decimales
        /// </summary>
        //private static System.Globalization.NumberFormatInfo m_FormatoNumerico = null;
        private static string m_sSeparadorDecimales = ".";
        //private static System.Globalization.CultureInfo m_InformacionCulturaAnterior = null;

        public static string SeparadorDecimales
        {
            get { return m_sSeparadorDecimales; }
            set
            {
                m_sSeparadorDecimales = value;
                //Instanciar el formato númerico
                /*if (m_sSeparadorDecimales == ".")
                {
                    System.Globalization.CultureInfo culturaActual = System.Globalization.CultureInfo.CurrentCulture;
                    m_FormatoNumerico = (System.Globalization.NumberFormatInfo)culturaActual.NumberFormat.Clone();
                    m_FormatoNumerico.NumberDecimalSeparator = ".";
                    m_FormatoNumerico.NumberGroupSeparator = ",";

                }
                else
                {
                    System.Globalization.CultureInfo culturaActual = System.Globalization.CultureInfo.CurrentCulture;
                    m_FormatoNumerico = (System.Globalization.NumberFormatInfo)culturaActual.NumberFormat.Clone();
                    m_FormatoNumerico.NumberDecimalSeparator = ",";
                    m_FormatoNumerico.NumberGroupSeparator = ".";
                }*/
            }
        }         

#endregion

    }



} //end of root namespace