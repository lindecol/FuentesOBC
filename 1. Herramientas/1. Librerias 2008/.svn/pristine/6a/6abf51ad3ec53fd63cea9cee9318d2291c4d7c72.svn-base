using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
#if NETCF20
using OpenNETCF;
#endif

#if NETCF20
namespace MovilidadCF.Data
{
#else
namespace Desktop.Data
{
#endif
    public class DBUtils
    {
        private DBUtils() { }

        public static DataRow GetCurrentRow(BindingSource bs)
        {
            DataRow row = null;
            if (bs.Position >= 0)
                row = ((DataRowView)bs.Current).Row;
            return row;
        }

        public static int SetCurrentRow(BindingSource bs, DataRow row)
        {
            //EL ENFOQUE ESTA HERRADO POR QUE AL ORGANIZAR LOS DATOS
            //EL INDICE DE LA VISTA ES DIFERENTE AL INDICE DEL BINDING SOURCE
            /*DataView dv = (DataView)bs.List;
            bs.Sort = GetPrimaryKeyColumsString(row.Table);
            int Position = dv.Find(GetPrimaryKeyValues(row));
            bs.Position = Position;
            bs.Sort = sPreviousSort;
            return bs.Position;*/
            string sPreviousSort = bs.Sort;
            int Position = -1;
            //SI NO HAY ORDEN, SE PUEDE UTILIZAR LA TABLA
            DataTable dt = null;
            if (bs.DataSource.GetType().FullName == "System.Data.DataSet")
            {                
                dt = ((DataSet)bs.DataSource).Tables[row.Table.TableName];
            }

            dt = ((DataSet)bs.DataSource).Tables[row.Table.TableName];

            if (dt == null)
            {
                return -1;
            }
            if (GetPrimaryKeyColumsString(dt)==string.Empty)
            {
                return -1;
            }
            object[] ValuePrimary = GetPrimaryKeyValues(row);
            DataRow rwFind = dt.Rows.Find(ValuePrimary);
            if (rwFind == null)
            {
                return -1;
            }
            
            if (DBUtils.IsNullOrEmpty(sPreviousSort))
            {
                Position = dt.Rows.IndexOf(rwFind);
                bs.Position = Position;
                return bs.Position;             
            }
            else
            {                
                int iIndice = 0;
                foreach (DataRow rwReg in dt.Select("1=1", sPreviousSort))
                {
                    if (rwReg == rwFind)
                    {
                        bs.Position = iIndice;                            
                        return iIndice;
                    }
                    iIndice++;
                }
                return -1;               
            }                           
        }

        private static object[] GetPrimaryKeyValues(DataRow row)
        {
            int KeyFieldsCount = row.Table.PrimaryKey.Length;
            object[] KeyValues = new object[KeyFieldsCount];
            for (int I = 0; I < KeyFieldsCount; I++)
                KeyValues[I] = row[row.Table.PrimaryKey[I].ColumnName].ToString();
            return KeyValues;
        }

        private static string GetPrimaryKeyColumsString(DataTable Table)
        {
            int KeyFieldsCount = Table.PrimaryKey.Length - 1;
            string sPrimaryKeyString = "";
            for (int i = 0; i <= KeyFieldsCount; i++)
            {
                if (i > 0)
                    sPrimaryKeyString += ", ";
                sPrimaryKeyString += Table.PrimaryKey[i].ColumnName;
            }
            return sPrimaryKeyString;
        }

        public static bool IsNullOrEmpty(Object FieldValue)
        {
            bool bResult = true;
            if (FieldValue != null)
            {
                if (FieldValue != DBNull.Value)
                {
                    if (FieldValue is System.String)
                    {
                        if (FieldValue.ToString().Trim() != "")
                            bResult = false;
                    }
                    else
                        bResult = false;
                }
             }
             return bResult;
        }

        public static bool IsNull(Object FieldValue)
        {
            bool bResult = true;
            if (FieldValue != null)
            {
                if (FieldValue != DBNull.Value)
                {
                    bResult = false;                    
                }
            }
            return bResult;
        }

        public static bool IsEmpty(Object FieldValue)
        {
            bool bResult = false;
            if (FieldValue != null)
            {
                if (FieldValue.ToString().Trim() == String.Empty)
                {
                    bResult = true;
                }
            }
            return bResult;
        }

        public static DataTable GetEnumerationTable(Type type)
        {
            DataTable dt = null;
            if (type != null)
            {
				if (type.IsEnum)
				{
					dt = new DataTable();
                    dt.Columns.Add("Value", typeof(System.Int32));
                    dt.Columns.Add("Name", typeof(System.String));
#if NETCF20
                    string[] Names = Enum2.GetNames(type);
                    Enum[] Values = Enum2.GetValues(type);
#else
                    string[] Names = Enum.GetNames(type);
                    int[] Values = (int[])Enum.GetValues(type);
#endif

                    for (int i = 0; i < Values.Length; i++)
						dt.Rows.Add(Values[i], Names[i]);
				}
				else
                    throw new ArgumentException("El tipo especificado debe ser una enumeración");
            }
            return dt;
        }

        public static void SetListSource(ListControl lstControl, Type enumeration)
        {
            DataTable dt = GetEnumerationTable(enumeration);
            lstControl.DisplayMember = "Name";
            lstControl.ValueMember = "Value";
            lstControl.DataSource = dt;            
        }

        public static void SetListSource(ListControl lstControl,string values, string names)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(System.Int32));
            dt.Columns.Add("Name", typeof(System.String));
            string[] valuelist = values.Split(',',';','|');
            string[] namelist = names.Split(',', ';', '|');
            for (int i = 0; i < valuelist.Length - 1; i++)
                dt.Rows.Add(valuelist[i], namelist[i]);
            lstControl.DataSource = dt;
            lstControl.DisplayMember = "Name";
            lstControl.ValueMember = "Value";
        }

        public static string ToString(Object FieldValue, string Default)
        {
            if (IsNullOrEmpty(FieldValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToString(FieldValue);
            }
        }

        public static int ToInt(Object FieldValue, int Default)
        {
            if (IsNullOrEmpty(FieldValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToInt32(FieldValue);
            }
        }

        public static double ToDouble(Object FieldValue, double Default)
        {
            if (IsNullOrEmpty(FieldValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToDouble(FieldValue);
            }
        }

        public static decimal ToDecimal(Object FieldValue, Decimal Default)
        {
            if (IsNullOrEmpty(FieldValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToDecimal(FieldValue);
            }
        }

        public static bool ToBoolean(Object FieldValue, bool Default)
        {
            if (IsNullOrEmpty(FieldValue))
            {
                return Default;
            }
            else
            {
                return Convert.ToBoolean(FieldValue);
            }
        }

        public static bool ValidarCambiosDataTable(DataTable Table, int nColumna)
        {

            DataView dvOriginal = new DataView(Table, "", "", DataViewRowState.OriginalRows);
            DataView dvCurrent = new DataView(Table, "", "", DataViewRowState.CurrentRows);

            if (dvOriginal.Count > 0 & dvCurrent.Count > 0)
            {

                if (dvOriginal.Count != dvCurrent.Count)
                {
                    return true;
                }


                for (int I = 0; I < dvOriginal.Count; I++)
                {
                    if (dvOriginal[I][nColumna].ToString() != dvCurrent[I][nColumna].ToString())
                    {
                        return true;
                    }
                }


                return false;
            }
            else
            {
                return false;
            }

        }

        public static bool ValidarCambiosDataTable(DataTable Table)
        {

            DataView dvOriginal = new DataView(Table, "", "", DataViewRowState.OriginalRows);
            DataView dvCurrent = new DataView(Table, "", "", DataViewRowState.CurrentRows);

            if (dvOriginal.Count > 0 & dvCurrent.Count > 0)
            {

                if (dvOriginal.Count != dvCurrent.Count)
                {
                    return true;
                }


                for (int I = 0; I < dvOriginal.Count; I++)
                {
                    for (int J = 0; J < dvOriginal[I].Row.ItemArray.Length; J++)
                    {
                        if (dvOriginal[I][J].ToString() != dvCurrent[I][J].ToString())
                        {
                            return true;
                        }
                    }
                }


                return false;
            }
            else
            {
                return false;
            }

        }

    }
}
