using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Web;


	public class WebAplicationSettings
	{
		
		#region "Miembros de la Clase"
		//MIEMBRO PARA CARGAR EN MEMORIA LOS DATOS DEL ARCHIVO XML
		private DataSet m_ds;
		//TABLA QUE ALMACENA LA INFORMACIN DE LOS DATOS DE CONFIGURACIN DEL SERVICIO WEB
		private DataTable m_dtConfig;
		//CONSTANTE QUE ALMACENA LA LLAVE DE ENCRIPCION DE LOS DATOS
		private string m_sLLave = "PRAXAIROraLlave" ;
		//TIPO DE ALGORITMO PARA LA ENCRIPCION DE LOS DATOS
		Encryption.Symmetric.Provider m_AlgoritmoEncripcion = Encryption.Symmetric.Provider.Rijndael;
		#endregion
		
		#region "Implementacin de Metodos de la Clase"
		
		// TIPO FUNCION : METODO DE LA CLASE
		// DESCRIPCION: CONSTRUCTOR DE LA CLASE
		// PARAMETROS: NINGUNO
		public WebAplicationSettings() {
			m_ds = new DataSet("AppSettings");
			ReadConfig();
		}
		
		// TIPO FUNCION : METODO DE LA CLASE
		// DESCRIPCION: FUNCION PARA LEER LOS DATOS ALMACENADOS EN EL ARCHIVO XML
		// PARAMETROS: NINGUNO
		private void ReadConfig ()
		{
			string sFile = FileName;
			if (File.Exists(sFile))
			{
				m_ds.ReadXml(sFile);
				m_dtConfig = m_ds.Tables[0];
			}
			else
			{
				m_dtConfig = new DataTable("Configuracion");
				m_dtConfig.Columns.Add("Key");
				m_dtConfig.Columns.Add("Value");
				m_ds.Tables.Add(m_dtConfig);
			}
			m_dtConfig.PrimaryKey = new DataColumn[] { m_dtConfig.Columns["Key"] };
		}
		
		// TIPO FUNCION : METODO DE LA CLASE
		// DESCRIPCION: FUNCION PARA ALMACENAR LOS DATOS EN EL ARCHIVO XML
		// PARAMETROS: NINGUNO
		private void SaveConfig ()
		{
			string sFile = FileName;
			m_dtConfig.AcceptChanges();
			m_ds.WriteXml(sFile);
		}
		
		// TIPO FUNCION : METODO DE LA CLASE
		// DESCRIPCION: FUNCION PARA ENCRIPTAR LOS DATOS
		// PARAMETROS: RECIBE LA CADENA DE CARACTERES QUE SE QUIERE ENCRIPTAR
		public Encryption.Data Encriptar(string sValor)
		{
			Encryption.Symmetric sym = new Encryption.Symmetric(m_AlgoritmoEncripcion,true);
			Encryption.Data key = new Encryption.Data(m_sLLave);
			Encryption.Data encryptedData;
			encryptedData = sym.Encrypt(new Encryption.Data(sValor), key);
			return encryptedData;
		}
		
		// TIPO FUNCION : METODO DE LA CLASE
		// DESCRIPCION: FUNCION PARA DESENCRIPTAR LOS DATOS
		// PARAMETROS: RECIBE LA CADENA DE CARACTERES ENCRIPTADOS
		// RETORNO: DEVUELVE UNA CADENA DE CARACTERES CON LOS DATOS DESENCRIPTADOS
		public string DesEncriptar(string sValor)
		{
			Encryption.Symmetric sym = new Encryption.Symmetric(m_AlgoritmoEncripcion,true);
			Encryption.Data key = new Encryption.Data(m_sLLave);
			Encryption.Data encryptedData = new Encryption.Data();
			encryptedData.Base64 = sValor;
			Encryption.Data decryptedData;
			decryptedData = sym.Decrypt(encryptedData, key);
			return decryptedData.Text;
		}
		
		// TIPO FUNCION : METODO DE LA CLASE
		// DESCRIPCION: FUNCION PARA COMPARAR UN VALOR DE ENTRADA CON UN VALOR ENCRIPTADO
		// PARAMETROS: RECIBE DOS CADENAS, UNA ES EL VALOR ENCRIPTADO Y LA OTRA ES EL VALOR A COMPARAR
		// RETORNO: DEVUELVE TRUE - FALSE SEGUN SEA EL RESULTADO DE LA COMPARACION
		public bool Revisarinformacion(string sValorEncriptado, string sValorDigitado)
		{
			if (sValorEncriptado == Encriptar(sValorDigitado).ToBase64())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion
		
		#region "Implementacin de las propiedades de la Clase"
		// TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL NOMBRE DEL ARCHIVO XML QUE ALMACENA LA INFORMACIN DE CONFIGURACIN
		protected string FileName
		{
			get{
				return Path.Combine(PathFile, "Config.XML");
			}
		}
		
		// TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL PATH DEL ARCHIVO XML QUE ALMACENA LA INFORMACIN DE CONFIGURACIN
		public string PathFile
		{
			get{
                return HttpContext.Current.Request.PhysicalApplicationPath;
			}
		}
		
		// TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL NOMBRE DEL USUARIO NUMERO UNO
		public int CodigoPrograma
		{
			get{
				DataRow row;
                row = m_dtConfig.Rows.Find("CodigoPrograma");
				if (row == null)
				{
					return 1;
				}
				else
				{
					return System.Convert.ToInt16(row["Value"]);
				}
			}
			set
			{
				DataRow row;
                row = m_dtConfig.Rows.Find("CodigoPrograma");
				if (row == null)
				{
					row = m_dtConfig.NewRow();
                    row["Key"] = "CodigoPrograma";
					row["Value"] = value;
					m_dtConfig.Rows.Add(row);
				}
				else
				{
					row["Value"] = value;
				}
				SaveConfig();
			}
        }

            // TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL NOMBRE DEL USUARIO NUMERO UNO
        public string CodigoEmpresa
        {
            get
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("CodigoEmpresa");
                if (row == null)
                {
                    return "21";
                }
                else
                {
                    return System.Convert.ToString(row["Value"]);
                }
            }
            set
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("CodigoEmpresa");
                if (row == null)
                {
                    row = m_dtConfig.NewRow();
                    row["Key"] = "CodigoEmpresa";
                    row["Value"] = value;
                    m_dtConfig.Rows.Add(row);
                }
                else
                {
                    row["Value"] = value;
                }
                SaveConfig();
            }
        }

            // TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL NOMBRE DEL USUARIO NUMERO UNO
		public string CodigoGrupo
		{
			get{
				DataRow row;
                row = m_dtConfig.Rows.Find("CodigoGrupo");
				if (row == null)
				{
					return "1";
				}
				else
				{
                    return System.Convert.ToString(row["Value"]);
				}
			}
			set
			{
				DataRow row;
                row = m_dtConfig.Rows.Find("CodigoGrupo");
				if (row == null)
				{
					row = m_dtConfig.NewRow();
                    row["Key"] = "CodigoGrupo";
					row["Value"] = value;
					m_dtConfig.Rows.Add(row);
				}
				else
				{
					row["Value"] = value;
				}
				SaveConfig();
			}
		}
		
		// TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL NOMBRE DEL DSN DE LA CONEXION ORACLE
		public string ConexionOracle
		{
			get{
				DataRow row;
				row = m_dtConfig.Rows.Find("ConexionOracle");
				if (row == null)
				{
					return "";
				}
				else
				{
					return System.Convert.ToString(row["Value"]);
				}
			}
			set
			{
				DataRow row;
				row = m_dtConfig.Rows.Find("ConexionOracle");
				if (row == null)
				{
					row = m_dtConfig.NewRow();
					row["Key"] = "ConexionOracle";
					row["Value"] = value;
					m_dtConfig.Rows.Add(row);
				}
				else
				{
					row["Value"] = value;
				}
				SaveConfig();
			}
		}
		
		// TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL NOMBRE DEL USUARIO ORACLE VALIDO
		public string UsuarioOracle
		{
			get{
				DataRow row;
				row = m_dtConfig.Rows.Find("UsuarioOracle");
				if (row == null)
				{
					return "";
				}
				else
				{
					return System.Convert.ToString(row["Value"]);
				}
			}
			set
			{
				DataRow row;
				row = m_dtConfig.Rows.Find("UsuarioOracle");
				if (row == null)
				{
					row = m_dtConfig.NewRow();
					row["Key"] = "UsuarioOracle";
					row["Value"] = value;
					m_dtConfig.Rows.Add(row);
				}
				else
				{
					row["Value"] = value;
				}
				SaveConfig();
			}
		}
		
		// TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER LA CONTRASEA DEL USUARIO ORACLE VALIDO
		public string ContrasenaOracle
		{
			get{
				DataRow row;
				row = m_dtConfig.Rows.Find("ContrasenaOracle");
				if (row == null)
				{
					return "";
				}
				else
				{
					return System.Convert.ToString(row["Value"]);
				}
			}
			set
			{
				DataRow row;
				row = m_dtConfig.Rows.Find("ContrasenaOracle");
				if (row == null)
				{
					row = m_dtConfig.NewRow();
					row["Key"] = "ContrasenaOracle";
					row["Value"] = value;
					m_dtConfig.Rows.Add(row);
				}
				else
				{
					row["Value"] = value;
				}
				SaveConfig();
			}
		}
		
		
		// TIPO FUNCION : PROPIEDAD DE LA CLASE
		// DESCRIPCION: PROPIEDAD PARA OBTENER EL PATH EN DONDE SE ALMACENA EL ARCHIVO DE LOG DE ERRORES
		public string PathErrores
		{
			get{
				DataRow row;
				row = m_dtConfig.Rows.Find("PathErrores");
				if (row == null)
				{
					return "";
				}
				else
				{
					return System.Convert.ToString(row["Value"]);
				}
			}
			set
			{
				DataRow row;
				row = m_dtConfig.Rows.Find("PathErrores");
				if (row == null)
				{
					row = m_dtConfig.NewRow();
					row["Key"] = "PathErrores";
					row["Value"] = value;
					m_dtConfig.Rows.Add(row);
				}
				else
				{
					row["Value"] = value;
				}
				SaveConfig();
			}
		}

        // TIPO FUNCION : PROPIEDAD DE LA CLASE
        // DESCRIPCION: PROPIEDAD PARA OBTENER SI SE GUARDA EL BACKUP DE LOS TALONARIOS
        public string BackupTalonarios
        {
            get
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("BackupTalonarios");
                if (row == null)
                {
                    return "N";
                }
                else
                {
                    return System.Convert.ToString(row["Value"]);
                }
            }
            set
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("BackupTalonarios");
                if (row == null)
                {
                    row = m_dtConfig.NewRow();
                    row["Key"] = "BackupTalonarios";
                    row["Value"] = value;
                    m_dtConfig.Rows.Add(row);
                }
                else
                {
                    row["Value"] = value;
                }
                SaveConfig();
            }
        }

        // TIPO FUNCION : PROPIEDAD DE LA CLASE
        // DESCRIPCION: PROPIEDAD PARA OBTENER EL UMBRAL EN MINUTOS DE LA HORA INICIO
        public int UmbralFechaInicio
        {
            get
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("UmbralFechaInicio");
                if (row == null)
                {
                    return 180;
                }
                else
                {
                    return System.Convert.ToInt32(row["Value"]);
                }
            }
            set
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("UmbralFechaInicio");
                if (row == null)
                {
                    row = m_dtConfig.NewRow();
                    row["Key"] = "UmbralFechaInicio";
                    row["Value"] = value;
                    m_dtConfig.Rows.Add(row);
                }
                else
                {
                    row["Value"] = value;
                }
                SaveConfig();
            }
        }

        // TIPO FUNCION : PROPIEDAD DE LA CLASE
        // DESCRIPCION: PROPIEDAD PARA OBTENER EL UMBRAL EN MINUTOS DE LA HORA INICIO
        public int UmbralFechaFin
        {
            get
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("UmbralFechaFin");
                if (row == null)
                {
                    return 180;
                }
                else
                {
                    return System.Convert.ToInt32(row["Value"]);
                }
            }
            set
            {
                DataRow row;
                row = m_dtConfig.Rows.Find("UmbralFechaFin");
                if (row == null)
                {
                    row = m_dtConfig.NewRow();
                    row["Key"] = "UmbralFechaFin";
                    row["Value"] = value;
                    m_dtConfig.Rows.Add(row);
                }
                else
                {
                    row["Value"] = value;
                }
                SaveConfig();
            }
        }


		#endregion
		
	}
	

