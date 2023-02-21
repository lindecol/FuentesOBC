using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using OpenNETCF;
using OpenNETCF.WindowsCE;

namespace MovilidadCF.Licensing
{
    public class LicenseValidator
    {

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        public string IDDispositivo
        {
            get { return DeviceManagement.GetDeviceID(); }
        }

        private string PathLicenseFile
        {
            get { return "Application\\"; }
        }

        public LicenseValidator()
        {
        }


        public bool Check()
        {
            //Si el archivo no existe no es valida la licencia
            if (!File.Exists(PathLicenseFile + _productName + ".Lic"))
            {                             
                return false;
            }

            //Si el archivo existe se valida la licencia con el archivo
            else
            {
                StreamReader srFileLicense = new StreamReader(System.IO.Path.Combine(PathLicenseFile, _productName + ".Lic"));
                string lineFileLicense = srFileLicense.ReadToEnd();
                srFileLicense.Close();

                string sProductDevice = ProductName + "\n" + IDDispositivo;
                string sLicenseDevice = CryptoLicense(sProductDevice);

                if (!sLicenseDevice.Equals(lineFileLicense))
                {                    
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        
        public string CryptoLicense(string ProductIdDevice)
        {
            MD5CryptoServiceProvider md5Crypto = new MD5CryptoServiceProvider();
            byte[] sinHash = Encoding.Default.GetBytes(ProductIdDevice);
            byte[] conHash = md5Crypto.ComputeHash(sinHash);
            return Encoding.Default.GetString(conHash, 0, conHash.Length - 1);
        }

        public void GuardarLicencia(string LicenciaIngresada)
        {            
            if (System.IO.File.Exists(System.IO.Path.Combine(PathLicenseFile, _productName + ".Lic")))
            {
                System.IO.File.Delete(System.IO.Path.Combine(PathLicenseFile, _productName + ".Lic"));
            }
            StreamWriter sw = new StreamWriter(System.IO.Path.Combine(PathLicenseFile, _productName + ".Lic"));
            sw.WriteLine(LicenciaIngresada);
            sw.Close();
        }

    }
}
