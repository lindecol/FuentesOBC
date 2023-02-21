using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PruebaWM.Capa_Datos
{
    class ConexionBD
    {
        static private string CadenaConexion = "Data Source=DESKTOP-BHKIR24\\SQLEXPRESS;Initial Catalog=PraxairPrueba;User ID=sa;Password=123456";
        private SqlConnection Conexion = new SqlConnection(CadenaConexion);

        public SqlConnection AbrirConexion() 
        {
            if (Conexion.State == ConnectionState.Closed)
                Conexion.Open();
            return Conexion;
            
        }

        public SqlConnection CerrarConexion()
        {
            if (Conexion.State == ConnectionState.Open)
                Conexion.Close();
            return Conexion;
        }
        
    }
}
