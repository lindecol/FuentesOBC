using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;

namespace MovilidadCF.Client
{
    class InstallHelper
    {
        [DllImport("Coredll.dll")]
        private extern static int KernelIoControl(int dwIoControlCode, IntPtr lpInBuf,
            int nInBufSize, IntPtr lpOutBuf, int nOutBufSize, ref int lpBytesReturned);


        [DllImport("Coredll.dll")]
        private extern static void SetCleanRebootFlag();

        public static void HardReset()
        {
            ResetDevice(true);
        }

        public static void SoftReset()
        {
            ResetDevice(false);
        }

        private static void ResetDevice(bool IsHardReset)
        {
            int IOCTL_HAL_REBOOT = 0x101003C;
            int bytesReturned = 0;
            if (IsHardReset == true)
            {
                SetCleanRebootFlag();
            }
            KernelIoControl(IOCTL_HAL_REBOOT, IntPtr.Zero, 0, IntPtr.Zero, 0, ref bytesReturned);
        }

        private static void SetSecurityFlag(int flagValue)
        {
            try
            {
                RegistryKey rKey = Registry.LocalMachine.OpenSubKey(@"Security\Policies\Policies", true);

                rKey.SetValue("0000101a", flagValue, RegistryValueKind.DWord);
                rKey.Close();
            }
            catch { /*silent exception, for Windows CE 2003 */ }
        }

        public static void InstallCabFile(string FileName)
        {
            SetSecurityFlag(1);
            Process updater = Process.Start("Wceload", string.Concat("/silent /noui \"", FileName, "\""));
            //updater.WaitForExit(60000);
        }

    }
}
