/* DESARROLLADO CON FINES EDUCATIVOS POR Y PARA MOBILE NET USERS GROUP 2008
 * URL: WWW.MOBILENUG.COM, WWW.MOBILENUG.ES, WWW.DYNAMICSMOBILE.ES
 * AUTOR: Jose Antonio Gallego
 * 
 * DESCRIPCIÓN: Ejemplo de cliente FTP para ser integrado en
 *              Compact Framework 2.0
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR 
 * THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */ 

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MovilidadCF.Communications.FTP
{
    public class FTPClient
    {
        #region Credenciales
        private string host = "tech-desk.net";
        private string username = "user";
        private string password = "MTD2009";
        private string LocalPath = "";
        private string RemotePath = "";
        #endregion

        public FTPClient(string sHost, string sUser, string sPass, string sLocalPath, string sRemotePath)
        {
            host = sHost;
            username = sUser;
            password = sPass;
            LocalPath = sLocalPath;
            RemotePath = sRemotePath;
        }


        /// <summary>
        /// Dirección del servidor ftp
        /// </summary>
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <summary>
        /// Nombre de usuario (por defecto anonymous)
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Contraseña de usuario (por defecto en blanco)
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

      

        #region  Declaracion de las funciones de la libreria Wininet.dll
        private const int MaxErrorBufferSize = 1024;
        private const int INTERNET_FLAG_TRANSFER_BINARY = 2;
        private const int INTERNET_SERVICE_FTP = 1;
        private const int INTERNET_FLAG_PASSIVE = 0x08000000;
        private const uint INTERNET_FLAG_RELOAD = 0x080000000;
        private const int FILE_ATTRIBUTE_NORMAL = 0x8;

        [DllImport("wininet.dll",SetLastError=true)]
        private static extern uint InternetConnect(uint hInternet, string lpszServerName, int nServerPort, string lpszUserName, string lpszPassword, Int32 dwService,int dwFlags, int dwContext);

        //[DllImport("wininet.dll",SetLastError=true,CharSet=CharSet.Unicode)]
        //private static extern bool FtpGetFile(int hConnect, string lpszRemoteFile,string lpszNewFile, int fFailIfExists, uint dwFlagsAndAttributes, uint dwFlags, int dwContext);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool FtpGetFile(uint hConnect, string lpszRemoteFile, string lpszNewFile, int fFailIfExists, uint dwFlagsAndAttributes, uint dwFlags,ref uint dwContext);

        [DllImport("wininet.dll",SetLastError=true)]
        private static extern bool FtpPutFile(uint hFtpSession, string lpszLocalFile,string lpszRemoteFile,int dwFlags,int dwContext);

        [DllImport("wininet.dll",SetLastError=true)]
        private static extern bool FtpDeleteFile(uint hConnect, string lpszRemoteFile);
 
        [DllImport("wininet.dll",SetLastError=true)]
        private static extern bool InternetCloseHandle(ref uint hInternet);

        [DllImport("wininet.dll",SetLastError=true)]
        private static extern uint InternetOpen(string sAgent, int  lAccessType, string sProxyName, string  sProxyBypass, int lFlags);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        public static extern uint FormatMessage(uint dwFlags, uint lpSource, uint dwMessageId, uint dwLanguageId,
            byte[] lpBuffer, uint nSize, uint Arguments);


        
        #endregion

        public  enum FTPCommand{Upload,Download,Delete};

        /// <summary>
        /// Permite ejecutar comandos FTP desde nuestra aplicación movil
        /// </summary>
        /// <param name="comando">Especificamos la acción a realizar, enviar, descargar o borrar un archivo del servidor Ftp</param>
        /// <param name="Archivo">Nombre del archivo.</param>
        public bool  Execute(FTPCommand command,string filename)
        {
            //TODO: No se ha agregado el control de errores...

            if (host == "") throw new Exception("No se ha inicializado el host");
            if (username == "") throw new Exception("No se ha especificado el usuario");

            //Variables para almacenar los handles de las conexiones
            uint iNet = 0;
            uint iNetConn;

            bool res = false;
            
            iNet = InternetOpen("test", 1,null, null, 0);

            if (iNet == 0) throw new Exception("No se pudo conectar a internet");

            //Ojo si la conexion no utiliza el modo pasivo en lugar de pasar INTERNET_FLAG_PASSIVE pasaremos 0
            iNetConn = InternetConnect(iNet, host, 21, username, password, INTERNET_SERVICE_FTP, INTERNET_FLAG_PASSIVE, 0);

            if (iNetConn == 0) throw new Exception("No se pudo conectar al servidor FTP");

            switch (command)
            {
                case FTPCommand.Upload:
                    res = FtpPutFile(iNetConn, filename, Path.GetFileName(filename), INTERNET_FLAG_TRANSFER_BINARY, 0);
                    break;
                case FTPCommand.Download:
                   //res = FtpGetFile((IntPtr)iNetConn, Archivo, Archivo,false , FILE_ATTRIBUTE_NORMAL, INTERNET_FLAG_TRANSFER_BINARY + INTERNET_FLAG_RELOAD, 0);                    
                    uint Context = 0;
                    res = FtpGetFile(iNetConn,RemotePath + filename,LocalPath + filename ,0,0,(uint)0x00000001, ref Context);                   
                   break;
                case FTPCommand.Delete:
                    res = FtpDeleteFile(iNetConn, filename);
                    break;
                default:
                    break;
            }

            //Cerramos las conexiones
            InternetCloseHandle(ref iNetConn);
            InternetCloseHandle(ref iNet);

            return res;
            //if (!res) throw new Exception("No se ejecuto el comando con exito");

        }       
    }
}
