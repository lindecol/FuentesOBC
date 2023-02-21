using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Reflection;

#if NETCF20
using OpenNETCF.Reflection;
namespace MovilidadCF.Configuration
{
#else
namespace Desktop.Configuration
{
#endif
    public abstract class Settings
	{
		private DataSet m_ds;
        private string _aplicationPath;
        //Controla si por defecto se utiliza la carpeta de Aplication Data
        private bool _utilizarAplicationData = true;

        public string AplicationPath
        {
            get { 
#if NETCF20  
                if (_aplicationPath == string.Empty)
                {                    
                    _aplicationPath = "\\Application";
                }
                return _aplicationPath; 
#else
                return _aplicationPath; 
#endif

            }
            set { _aplicationPath = value; }
        }
        private string _fileName;

        private string FileName
        {
            get { 
#if NETCF20  
                    if (_fileName == string.Empty)
                    {
                        _fileName = Assembly2.GetEntryAssembly().GetName().Name;
                    }
                    return _fileName;                    
#else
                    if (_fileName == string.Empty)
                    {
                        _fileName = this.GetType().Name;
                    }
                    return _fileName;                    
#endif
                }
            set { _fileName = value; }
        }

        public Settings()
        {            
            _aplicationPath = string.Empty;
            _fileName = string.Empty;
            AutoSave = false;
            _utilizarAplicationData = true;
            LoadConfig();
        }

        ~Settings()
        {            
            SaveConfig();
        }

		public Settings(string AplicationPath)
		{
			_aplicationPath = AplicationPath;
            _fileName = "";
            AutoSave = false;
            _utilizarAplicationData = true;
			LoadConfig();
		}

        public Settings(string AplicationPath, bool bAutoSave)
        {
            _aplicationPath = AplicationPath;
            _fileName = "";
            AutoSave = bAutoSave;
            _utilizarAplicationData = true;
            LoadConfig();
        }

		public Settings(string AplicationPath, string FileName) 
		{
            _aplicationPath = AplicationPath;
            _fileName = FileName;
            AutoSave = false;
            _utilizarAplicationData = true;
			LoadConfig();
		}

        public Settings(string AplicationPath, string FileName, bool bAutoSave, bool utilizarAplicationData)
        {
            _aplicationPath = AplicationPath;
            _fileName = FileName;
            AutoSave = bAutoSave;
            _utilizarAplicationData = utilizarAplicationData;
            LoadConfig();
        }

		protected virtual string DataSetName
		{
			get
			{
				return "Configuration";
			}
		}

		private string m_sTableName = "";
		protected virtual string TableName
		{
			get
			{
				if ( m_sTableName == "" )
					m_sTableName = this.GetType().Name;
				return m_sTableName;
			}
		}	

        private string m_sConfigFile = "";
		public string ConfigFile
		{            
			get
			{
               string pathConfig = "";
               if ( m_sConfigFile == "" )
               {
#if NETCF20  		
                   pathConfig = AplicationPath;
#else
                   if (_utilizarAplicationData)
                   {
                       pathConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _aplicationPath);
                   }
                   else
                   {
                       pathConfig = _aplicationPath;
                   }
#endif
                   if (!Directory.Exists(pathConfig))
                   {
                       Directory.CreateDirectory(pathConfig);
                   }
                   m_sConfigFile = Path.Combine(pathConfig, FileName + ".XML");
               }
			   return m_sConfigFile;
			}
		}



        private bool m_bAutoSave;
		protected bool AutoSave
		{
			get
			{
				return m_bAutoSave;
			}
			set
			{
				m_bAutoSave = value;
			}
		}

        public void LoadConfig()
        {
            if (m_ds == null)
            {
                if (File.Exists(ConfigFile))
                {

                    m_ds = new DataSet(DataSetName);


#if NETCF20  		
                m_ds.ReadXml(ConfigFile);
#else
                    FileStream ArchivoCompartido = new FileStream(ConfigFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    m_ds.ReadXml(ArchivoCompartido);
                    ArchivoCompartido.Close();
#endif


                }
                else
                    m_ds = new DataSet(DataSetName);


                if (!m_ds.Tables.Contains(TableName))
                {
                    DataTable dt = new DataTable(TableName);
                    dt.Columns.Add("Key");
                    dt.Columns.Add("Value");
                    m_ds.Tables.Add(dt);
                }

                m_ds.Tables[TableName].PrimaryKey = new DataColumn[] { m_ds.Tables[0].Columns["Key"] };
            }
        }

        public void SaveConfig()
        {
            if (File.Exists(ConfigFile))
            {
                File.Delete(ConfigFile);
                m_ds.WriteXml(ConfigFile);
                //FileStream ArchivoCompartido = new FileStream(ConfigFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                //m_ds.WriteXml(ArchivoCompartido);
                //ArchivoCompartido.Close();
            }
            else
            {
                m_ds.WriteXml(ConfigFile);
            }
        }		
		public string GetValue(string Key, string DefualtValue)
		{
			DataRow row;
			row = m_ds.Tables[TableName].Rows.Find(Key);
			if (row == null)
			{
				return DefualtValue;
			}
			else
			{
				return System.Convert.ToString(row["Value"]);
			}
		}
		
		public void SetValue (string Key, string Value)
		{
			DataRow row;
			row = m_ds.Tables[TableName].Rows.Find(Key);
			if (row == null)
			{
				row = m_ds.Tables[0].NewRow();
				row["Key"] = Key;
				row["Value"] = Value;
				m_ds.Tables[0].Rows.Add(row);
			}
			else
			{
				row["Value"] = Value;
			}
			m_ds.AcceptChanges();
			if (AutoSave)
			{
				//SaveConfig();
			}
		}
	}
}
