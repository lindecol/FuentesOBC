using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MovilidadCF.Windows.Forms;
using System.Net;
using OpenNETCF.Net;
using OpenNETCF.WindowsCE;
using MovilidadCF.Data;
using MovilidadCF.Compression;

namespace PraxairComunicaciones.Comunicacion
{
    public class TrustAllCertificatePolicy : ICertificatePolicy
    {
        public TrustAllCertificatePolicy()
        {
        }

        public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest req, int problem)
        {
            return true;
        }
    }
}
