#define WINCE
using System;
using System.Runtime.InteropServices;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// This class contains a bunch of useful PInvoke calls. Since WindowsCE and Windows
	/// implement the functions here in different dlls, you can toggle the WINCE define
	/// at the top of the file to build for WindowsCE or desktop.
	/// </summary>
	public class Windows
	{
        /// <summary>
        /// PInvoke for GetLastError().
        /// </summary>
        /// <returns>An interger representing the error code. Use winerror.h from the
        /// platform SDK to find out what the error code means.</returns>
#if WINCE
        [DllImport("CoreDll.dll", EntryPoint="GetLastError")]
#else
        [DllImport("kernel32.dll", EntryPoint="GetLastError")]
#endif
        public static extern int GetLastError();		
        
        
        /// <summary>
        /// PInvoke for CopyMemory. Windows CE doesn't export CopyMemory, but it does
        /// export memcpy which has the same prototype.
        /// </summary>
        /// <param name="pDest">Pointer to the starting address of the copied block's destination</param>
        /// <param name="pSrc">Poipnter to the starting address of the block of memory to copy</param>
        /// <param name="length">Size of the block of memory to copy, in bytes</param>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="memcpy")]
#else
        [DllImport("kernel32.dll", EntryPoint="CopyMemory")]
#endif
        public unsafe static extern void CopyMemory(byte *pDest, byte *pSrc, int length);
        

        /// <summary>
        /// PInvoke for GetCapture()
        /// </summary>
        /// <returns>The return value is a handle to the capture window associated with the
        /// current thread. If no window in the thread has captured the mouse, the return
        /// value is NULL.</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="GetCapture")]
#else
        [DllImport("user32.dll", EntryPoint="GetCapture")]
#endif
        public static extern IntPtr GetCapture();


        /// <summary>
        /// PInvoke for GetDC()
        /// </summary>
        /// <param name="hwnd">Handle to the window whose DC is to be retrieved.</param>
        /// <returns>A handle to the DC for the specified window's client area</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="GetDC")]
#else
        [DllImport("user32.dll", EntryPoint="GetDC")]
#endif
        public static extern IntPtr GetDC(IntPtr hwnd);



        /// <summary>
        /// PInvoke for ReleaseDC
        /// </summary>
        /// <param name="hwnd">Handle to the window whose DC is to be released.</param>
        /// <param name="hdc">Handle to the DC to be released</param>
        /// <returns>The return value indicates whether the DC was released. If the
        /// DC was released, the return value is 1. If the DC was not released, the return
        /// value is zero.</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="ReleaseDC")]
#else
        [DllImport("user32.dll", EntryPoint="ReleaseDC")]
#endif
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);


        /// <summary>
        /// PInvoke for CreateCompatibleDC
        /// </summary>
        /// <param name="hdc">Handle to an existing DC.</param>
        /// <returns>The handle to a memory DC.</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="CreateCompatibleDC")]
#else
        [DllImport("gdi32.dll", EntryPoint="CreateCompatibleDC")]
#endif
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);


        /// <summary>
        /// PInvoke for DeleteDC
        /// </summary>
        /// <param name="hdc">Handle to the device context</param>
        /// <returns>True if the function succeeds; false otherwise</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="DeleteDC")]
#else
        [DllImport("gdi32.dll", EntryPoint="DeleteDC")]
#endif
        public static extern bool DeleteDC(IntPtr hdc);



        /// <summary>
        /// PInvoke for DeleteObject
        /// </summary>
        /// <param name="hObject">HAndle to a logical pen, brush, font, bitmap, region or palette</param>
        /// <returns>True if the function succeedes; false otherwise</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="DeleteObject")]
#else
        [DllImport("gdi32.dll", EntryPoint="DeleteObject")]
#endif
        public static extern bool DeleteObject(IntPtr hObject);


        /// <summary>
        /// PInvoke for SelectObject
        /// </summary>
        /// <param name="hdc">Handle to the DC</param>
        /// <param name="hgdiobj">Handle to the object (bitmap, brush, font, pen or region)
        /// to be selected.</param>
        /// <returns>If the function succeeds, the return value is a handle to the object
        /// being replaced.</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="SelectObject")]
#else
        [DllImport("gdi32.dll", EntryPoint="SelectObject")]
#endif
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);


        /// <summary>
        /// Used in the dwROP parameter for BitBlt.
        /// Copies the source rectangle directly to the destination rectangle.
        /// </summary>
        public const int SRCCOPY = 0x00CC0020;

        /// <summary>
        /// PInvoke for BitBlt
        /// </summary>
        /// <param name="hdcDest">Handle to the destination device context</param>
        /// <param name="nXDest">Specifies the x-coordinate, in logical units, of
        /// the upper-left corner of the destination rectangle.</param>
        /// <param name="nYDest">Specifies the y-coordinate, in logical units, of
        /// the upper-left corner of the destination rectangle.</param>
        /// <param name="nWidth">Specifies the width, in logical units, of the
        /// source and destination rectangles.</param>
        /// <param name="nHeight">Specifies the height, in logical units, of the
        /// source and destination rectangles.</param>
        /// <param name="hdcSrc">Handle to the source device context.</param>
        /// <param name="nXSrc">Specifies the x-coordinate, in logical units, of
        /// the upper-left corner of the source rectangle.</param>
        /// <param name="nYSrc">Specifies the y-coordinate, in logical units, of
        /// the upper-left corner of the source rectangle.</param>
        /// <param name="dwROP">Specifies a raster-operation code. These codes define
        /// how the color data for the source rectangle is to be combined with the
        /// color data for the destination rectangle to achieve the final color.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="BitBlt")]
#else
        [DllImport("gdi32.dll", EntryPoint="BitBlt")]
#endif
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwROP);


        /// <summary>
        /// Used in the iUsage parameter for CreateDIBSection.
        /// The BITMAPINFO structure contains an array of literal RGB values.
        /// </summary>
        public const int DIB_RGB_COLORS = 0;

        /// <summary>
        /// PInvoke for CreateDIBSection.
        /// </summary>
        /// <param name="hdc">Handle to a device context.</param>
        /// <param name="pbmi">Pointer to a BITMAPINFO structure that specifies various attributes
        /// of the DIB, including the bitmap dimensions and colors.</param>
        /// <param name="iUsage">Specifies the type of data contained in the bmiColors array member
        /// of the BITMAPINFO structure pointed to by pbmi.</param>
        /// <param name="ppvBits">Pointer to a variable that receives a pointer to the location of the DIB bit values.</param>
        /// <param name="hSection">This parameter must be NULL on WindowsCE</param>
        /// <param name="dwOffset">This parameter should be 0 on WindowsCE</param>
        /// <returns></returns>
#if WINCE
        [DllImport("coredll.dll", EntryPoint="CreateDIBSection", SetLastError=true)]
#else
        [DllImport("gdi32.dll", EntryPoint="CreateDIBSection", SetLastError=true)]
#endif
        public unsafe static extern IntPtr CreateDIBSection(IntPtr hdc, byte* pbmi, uint iUsage,
                                                            byte** ppvBits, IntPtr hSection, uint dwOffset);
        
        /// <summary>
        /// An uncompressed format.
        /// </summary>
        public const int BI_RGB = 0;
        /// <summary>
        /// Specifies that the bitmap is not compressed and that the color table consists of three
        /// DWORD color masks that specify the red, green, and blue components, respectively, of
        /// each pixel. This is valid when used with 16bpp and 32bpp bitmaps.
        /// </summary>
        public const int BI_BITFIELDS = 3;
    }
}
