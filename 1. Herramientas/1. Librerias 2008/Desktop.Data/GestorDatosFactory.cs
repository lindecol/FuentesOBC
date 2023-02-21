using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;

#if NETCF20
namespace MovilidadCF.Data
{
#else
namespace Desktop.Data
{
#endif

    public sealed class ConnectionTypes
    {
        private ConnectionTypes() { }
        public const string SqlClient = "SqlClient";
        public const string SqlServerCe = "SqlServerCe";
        public const string Odbc = "Odbc";
        public const string Oracle = "Oracle";
    }
    
    public sealed class  GestorDatosFactory
    {
        private GestorDatosFactory()  { }
        
        public static GestorDatosBase CreateInstance(
            String connectionTypeName,
            String connectionString )
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            object[] args = new object[] { connectionString };
            GestorDatosBase gestor = (GestorDatosBase)assembly.CreateInstance(
                "Desktop.Data." + connectionTypeName + ".GestorDatosBase");
            gestor.ConnectionString = connectionString;
            return gestor;
        }

        private static List<string> m_AvailabeConnectionTypes;
        public static List<string> GetConnectionTypeNames()  
        {
            if (m_AvailabeConnectionTypes == null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                m_AvailabeConnectionTypes = new List<string>();
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass && type.IsSubclassOf(typeof(GestorDatosBase)))
                    {
                        string sName = type.FullName.Replace("Desktop.Data.", "");
                        sName = sName.Replace(".GestorDatosBase", "");
                        m_AvailabeConnectionTypes.Add(sName);
                    }
                }
            }
            return m_AvailabeConnectionTypes;
        }

        public static void prueba()
        {
            System.Data.SqlClient.SqlConnection dbCon = new System.Data.SqlClient.SqlConnection();
            System.Data.IDbConnection con = dbCon;
            con.ConnectionString = "";
        }
    }
}
