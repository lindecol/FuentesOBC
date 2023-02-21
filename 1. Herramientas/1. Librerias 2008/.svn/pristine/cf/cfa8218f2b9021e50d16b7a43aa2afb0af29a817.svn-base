using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;


#if NETCF20
using OpenNETCF.WindowsCE;
using OpenNETCF.Reflection;

namespace MovilidadCF.Logging
{
#else
namespace Desktop.Logging
{
#endif

    public sealed class Logger
    {
#if NETCF20
        private static string logDirectory = "\\MovilidadCF\\Logs";
        private static long _logSizeLimit = 512000;
#else
        private static string logDirectory = "C:\\MovilidadCF\\Logs";
        private static long _logSizeLimit = 5120000;
#endif
        public static string LogDirectory
        {
            get { return Logger.logDirectory; }
            set { Logger.logDirectory = value; }
        }

        public static long LogSizeLimit
        {
            get { return _logSizeLimit; }
            set { _logSizeLimit = value; }
        }

        private static string _logFileName = string.Empty;
        public static string LogFileName
        {
            get
            {
                if (_logFileName == string.Empty)
                {
#if NETCF20
                    _logFileName = Assembly2.GetEntryAssembly().GetName().Name;
#else
                    Assembly assembly = Assembly.GetEntryAssembly();
                    if (assembly == null)
                        assembly = Assembly.GetCallingAssembly();
                    _logFileName = assembly.GetName().Name;
#endif
                    _logFileName = _logFileName + ".log";
                }
                return _logFileName;
            }
            set
            {
                _logFileName = value;
            }
        }

        private static bool enabled = true;
        public static bool Enabled
        {
            get { return Logger.enabled; }
            set { Logger.enabled = value; }
        }

        private Logger() { }

        public static void Write(string mensaje)
        {
            WriteLog(mensaje);
        }

        public static void Write(Exception ex)
        {
            string mensaje = "Ha ocurrido una Excepcion  " + ex.GetType().ToString() + "\n"
                + ex.Message + "\nStack Trace:" + ex.StackTrace.ToString();
            WriteLog(mensaje);
        }

        private static void WriteLog(string mensaje)
        {
            if (!Enabled)
                return;

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
                       
            //Se arma el nombre del archivo
            string fileName = Path.Combine(LogDirectory, LogFileName);
            long currentLength = 0;
            
            lock (fileName)
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read));
                    sw.AutoFlush = true;
                    sw.WriteLine(string.Format("----->{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss fff")));
                    #if NETCF20
                    sw.WriteLine("Memoria Disponible:" + MemoryManagement.AvailablePhysicalMemory.ToString());
                    sw.WriteLine("Memoria Total:" + MemoryManagement.TotalPhysicalMemory.ToString());
                    #endif
                    sw.WriteLine(mensaje);
                    sw.WriteLine("----------------------");
                    currentLength = sw.BaseStream.Length;
                }
                catch { }
                finally
                {
                    if (sw != null)
                        sw.Close();
                }
                if (currentLength > _logSizeLimit)
                {
                    string fileBackup = Path.ChangeExtension(fileName, ".bak");
                    File.Delete(fileBackup);
                    File.Move(fileName, fileBackup);
                }
            }
        }

    }
}
