using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

#if NETCF20
namespace MovilidadCF.Data.PAFClient
{
#else
namespace Desktop.Data.PAFClient
{
#endif
    public class PAFActor
    {

        public enum PageControlMethods
        {
            Header,
            Body
        }

        public int ActorNumber;
        public string FieldSeparator = "|";
        public string TagValueSeparator = "=";
        public bool UseFieldNameTags = true;
        public string ParameterSeparator = "|";
        private PAFConnection Connection = null;
        public string DateFormat = "yyMMdd";
        public int VirtualDecimals = 2;
        public int HeaderFields = 0;
        public int NumberFields = 1;
        public PageControlMethods PageControlMethod = PageControlMethods.Header;
        public bool UseDataSizeHeader = false;

        public PAFActor(int ActorNumber, PAFConnection Connection)
        {
            if (ActorNumber < 0 || ActorNumber > 99)
                throw new ArgumentOutOfRangeException(
                    "ActorNumber debe estar entre 0 y 99. Valor enviado: "
                    + ActorNumber.ToString());
            this.ActorNumber = ActorNumber;
            this.Connection = Connection;
        }

        private string GetQueryString(string[] parametros)
        {
            return "0000000" + Connection.IDSistema + ActorNumber.ToString("00") + "00|"
                + String.Join(this.ParameterSeparator, parametros) + "|";
        }

        public string ExecuteQuery(params string[] parametros)
        {
            string sQuery = GetQueryString(parametros);
            StringBuilder sbResponse = new StringBuilder();
            string sData = "";
            int nSegmentoActual, nTotalSegmentos;
            for (; ; )
            {
                try
                {
                    Connection.OpenConnection();
                    Connection.SendData(sQuery);
                    sData = Connection.GetData(UseDataSizeHeader);
										
                    if (sData.Length < 16)
                        throw new System.Net.ProtocolViolationException(
                            "La trama devuelta por PAF no tiene el tamaño minimo requerido");
                    sbResponse.Append(sData.Remove(sData.Length - 1, 1).Remove(0, 16));
                    if (this.PageControlMethod == PageControlMethods.Header)
                    {
                        nSegmentoActual = Convert.ToInt32(sData.Substring(3, 2));
                        nTotalSegmentos = Convert.ToInt32(sData.Substring(5, 2));
                    }
                    else
                    {
                        nSegmentoActual = Convert.ToInt32(sData.Substring(sData.IndexOf("@PAGE=") + 5, 6));
                        nTotalSegmentos = Convert.ToInt32(sData.Substring(sData.IndexOf("@PAGECOUNT=") + 11, 6));
                    }
                    if (nSegmentoActual >= nTotalSegmentos)
                        break;
                }
                catch (Exception ex)
                {
                    //Guardar Los datos que se cappturaron
                    if (sData != null)
                    {
                        #if NETCF20                        
                            MovilidadCF.Logging.Logger.Write("Error: " + ex.Message + " Datos: " + sData);
                        #else
                            Desktop.Logging.Logger.Write("Error: " + ex.Message + " Datos: " + sData);
#endif
                        }
                    throw ex;
                }
                finally
                {
                    Connection.CloseConnection();
                }
            }
            return sbResponse.ToString();
        }

        public void Fill(DataTable dt, params string[] parametros)
        {
            string sData = ExecuteQuery(parametros);
            Fill(dt, sData);            
        }

        public void Fill(DataTable dt, string sData)
        {
            string[] sFields;
            string sFieldValue, sFieldName;
            DataRow row;
            DataColumn col;
            int nIndex, nColIndex;

            dt.Rows.Clear();

            if (sData.IndexOf("Exception", 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                return;
            if (sData.IndexOfAny(FieldSeparator.ToCharArray()) < 0)
                return;

            StringReader sr = new StringReader(sData);

            string sLine = sr.ReadLine();

            while (sLine != null)
            {
                sFields = sLine.Split(FieldSeparator.ToCharArray());
                row = dt.NewRow();
                string sFirstField = "";
                for (int i = 0; i < sFields.Length; i++)
                {
                    if (UseFieldNameTags)
                    {
                        // se optiene el nombre del campo y se verifica que no 
                        // se procesan los campos especiales (inician con @)
                        nIndex = sFields[i].IndexOf(TagValueSeparator);
                        if (nIndex == -1)
                            continue;
                        sFieldName = sFields[i].Substring(0, nIndex).Trim();
                        if (sFieldName.StartsWith("@"))
                            continue;
                        if (HeaderFields == i && sFirstField == "")
                            sFirstField = sFieldName;
                        else if (sFieldName == sFirstField)
                        {
                            dt.Rows.Add(row);
                            row = dt.NewRow();
                            for (int j = 0; j < HeaderFields; j++)
                                row[j] = dt.Rows[0][j];
                        }
                        sFieldValue = sFields[i].Substring(nIndex + 1).Trim();
                        nColIndex = dt.Columns.IndexOf(sFieldName);
                    }
                    else
                    {
                        sFieldValue = sFields[i].Trim();
                        nColIndex = i % NumberFields;
                        if (i != 0 && nColIndex == 0)
                        {
                            dt.Rows.Add(row);
                            row = dt.NewRow();
                        }
                    }
                    if (nColIndex >= 0)
                    {
                        col = dt.Columns[nColIndex];
                        try
                        {
                            if (col.DataType == typeof(System.DateTime))
                            {
                                if (sFieldValue.Length > DateFormat.Length)
                                    sFieldValue = sFieldValue.Substring(0, this.DateFormat.Length);
                                row[nColIndex] = DateTime.ParseExact(sFieldValue, this.DateFormat, null);
                            }
                            else if (col.DataType == typeof(System.Double) && sFieldValue != "")
                            {
                                if (VirtualDecimals > 0)
                                    row[nColIndex] = Convert.ToDouble(sFieldValue) / Math.Pow(10, VirtualDecimals);
                                else
                                    row[nColIndex] = Convert.ToDouble(sFieldValue);
                            }
                            else if (col.DataType == typeof(System.Decimal) && sFieldValue != "")
                            {
                                if (VirtualDecimals > 0)
                                    row[nColIndex] = Convert.ToDecimal(sFieldValue) / Convert.ToDecimal(Math.Pow(10, VirtualDecimals));
                                else
                                    row[nColIndex] = Convert.ToDecimal(sFieldValue);
                            }
                            else if (col.DataType == typeof(System.Int32) && sFieldValue != "")
                                row[nColIndex] = Convert.ToInt32(sFieldValue);
                            else
                                row[nColIndex] = sFieldValue;
                        }
                        catch
                        {
                            // si hay un error el campo queda en null
                        }
                    }
                }
                dt.Rows.Add(row);
                sLine = sr.ReadLine();
            }
            dt.AcceptChanges();
        }

    }

}