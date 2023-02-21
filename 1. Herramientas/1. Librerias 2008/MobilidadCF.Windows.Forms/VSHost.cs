using System;
using System.Collections.Generic;
using System.Text;

namespace MovilidadCF.Windows.Forms
{
    internal class VSHost
    {
        public static bool DesignMode()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }
    }
}
