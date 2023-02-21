using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PruebaWM.Capa_Datos
{
    class ClsFirma
    {
        ConexionBD Conexion = new ConexionBD();
        SqlCommand Comando = new SqlCommand();

        public DataTable ListarPrax()
        {
            DataTable Tabla = new DataTable();
            Comando.Connection = Conexion.AbrirConexion();
            Comando.CommandText = @"select MUN.NOM_MUN,BAV.NOM_BAV,CLI.CODIGO_CLIENTE,COM.ORDEN_COMPRA,COM.NUME_PEDIDO,CILIN.CODIGO_PRODUCTO,
            CILIN.DESCR_PROD,COM.CANTIDAD_PRODUC,COM.UNIDAD,CILIN.VALOR,CILIN.PRESION_INICIAL,CILIN.PRESION_FINAL,
            CILIN.NIVEL_INICIAL,CILIN.NIVEL_FINAL,OPE.NOM_OPERADOR,CLI.NOM_CLIENTE,CLI.CEDULA
            from COMPRA COM
            JOIN MUNICIPIOS MUN ON MUN.ID_MUN=COM.ID_MUN_COM AND MUN.ID_DEP_MUN=COM.ID_DEP_COM
            JOIN DEPARTAMENTOS DEP ON DEP.ID_DEP= COM.ID_DEP_COM
            JOIN BARRIO BAV ON BAV.ID_BAV= COM.ID_BAV_COM
            JOIN CLIENTE CLI ON CLI.ID_CLIENTE=COM.ID_CLIEN_COM AND CLI.ID_BAV_CLI=BAV.ID_BAV
            JOIN OPERADOR OPE ON OPE.ID_OPE=COM.ID_OPE_COM
            JOIN CILINDROS CILIN ON CILIN.ID_CILINDRO=COM.iD_CILIN_COM";
            Comando.CommandType = CommandType.Text;
            SqlDataAdapter adaptador= new SqlDataAdapter(Comando);
            adaptador.Fill(Tabla);
            return Tabla;
        }

    }
}
