// PrintDirect.cs
// Shows how to write data directly to the printer using Win32 API's
// Written 17th October 2002 By J O'Donnell - csharpconsulting@hotmail.com
// Adapted from Microsoft Support article Q298141
// This code assumes you have a printer at share \\192.168.1.101\hpl
// This code sends Hewlett Packard PCL5 codes to the printer to print
// out a rectangle in the middle of the page.
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Desktop.Printing
{

    [StructLayout(LayoutKind.Sequential)]
    internal struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDocName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDataType;
    }
    public class PrintDirect
    {
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        private static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        private static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern long StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern long EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern long EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern long ClosePrinter(IntPtr hPrinter);

        public static bool PrintText(string PrinterName, string Text)
        {
            System.IntPtr hPrinter = IntPtr.Zero;
            DOCINFO docInfo = new DOCINFO();
            int writenBytes = 0;
            docInfo.pDocName = "Direct Text Print";
            docInfo.pDataType = "RAW";
            OpenPrinter(PrinterName, ref hPrinter, 0);
            if (hPrinter.ToInt32() == 0)
                return false;

            StartDocPrinter(hPrinter, 1, ref docInfo);
            StartPagePrinter(hPrinter);
            WritePrinter(hPrinter, Text, Text.Length, ref writenBytes);
            EndPagePrinter(hPrinter);
            EndDocPrinter(hPrinter);
            ClosePrinter(hPrinter);
            return (writenBytes == Text.Length);
        }
    }

}
