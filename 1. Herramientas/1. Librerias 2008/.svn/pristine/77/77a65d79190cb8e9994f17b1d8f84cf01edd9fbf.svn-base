using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// BitmapFile class - Demonstrates the functionality required to save a
	/// control image to a file.
	/// </summary>
	public class BitmapFile
	{		
		/// <summary>
		/// Copies the pixel data from the specified control to the byte array
		/// </summary>
		/// <param name="hwnd">Handle to a control used as copy source</param>
		/// <param name="bytes">Destination buffer of entire file</param>
		/// <param name="fileHdr">Valid bitmap info header instance</param>
		/// <param name="infoHdr">Valid bitmap file header instance</param>
		/// <returns>Result: true if succeeded, false otherwise</returns>
		static public bool CopyImageSource(IntPtr hwnd, byte[] bytes, BITMAPFILEHEADER fileHdr, BITMAPINFOHEADER infoHdr)
		{
			// Get the DeviceContext
			IntPtr hdc = Windows.GetDC(hwnd);

			// Create a compatible (memory) hDC that we can blt onto
			IntPtr hdcComp = Windows.CreateCompatibleDC(hdc);

			// Create and write out bitmap info for a 555 BI_BITFIELDS compressed image
			// BITMAPINFOHEADER = 40 bytes
			// 3 RGBQUADS for bit masking = 12 bytes
			byte[] dummyBitmapInfo = new byte[52];
			MemoryStream msDummy = new MemoryStream(dummyBitmapInfo);
			BinaryWriter bwDummy = new BinaryWriter(msDummy);
			infoHdr.Store(bwDummy, true);

			try
			{
				unsafe
				{
					// pBitmapInfo points to the locked location of the bitmap info structure
					// pPixelDest points to the pixel data destination
					fixed (byte* pBitmapInfo = &dummyBitmapInfo[0], pPixelDest = &bytes[fileHdr.bfOffBits])
					{
						// Pointer to pixel data source
						byte* pPixelSource;

						// Create a DIBSection using the dummyBitmapInfo structure. This acquires a
						// pointer (pPixelSource) to the actual bit data for the image
						IntPtr hDibSect = Windows.CreateDIBSection(hdc, pBitmapInfo, Windows.DIB_RGB_COLORS, &pPixelSource, (IntPtr)0, (uint)0);
#if DEBUG
						// if something failed creating the DIBSection, put up a MessageBox with
						// the value from GetLastError().
						if (hDibSect == (IntPtr)0)
						{
							MessageBox.Show(Windows.GetLastError().ToString());
							Windows.DeleteObject(hDibSect);
							return false;
						}
#endif
						// Select the DIBSection into the memory DC
						IntPtr hbmOld = Windows.SelectObject(hdcComp, hDibSect);

						// BitBlt from pImage's hDC into the memory DC
						Windows.BitBlt(hdcComp, 0, 0, infoHdr.biWidth, infoHdr.biHeight, hdc, 0, 0, Windows.SRCCOPY);
						
						// Copy the data from the pPixelSource pointer into the actual bitmap data (pPixelDest)
						// which will be writen to disk
						Windows.CopyMemory(pPixelDest, pPixelSource, (int)fileHdr.sizeOfImageData);
						
						// Replace the bitmap object in the memory DC with what was originally there
						Windows.SelectObject(hdcComp, hbmOld);
						
						// Delete the DIBSection because we are done with it
						Windows.DeleteObject(hDibSect);
					}
				}
			}
			finally
			{
				// Close the dummy BinaryWriter and the stream it was based on
				bwDummy.Close();
				msDummy.Close();
			}

			// Delete the compatible DC because we are done with it
			Windows.DeleteDC(hdcComp);
			
			// Release the pImage's hDC
			Windows.ReleaseDC(hwnd, hdc);

			return true;
		}

		/// <summary>
		/// Saves the image on the specified control to the specified path as a bitmap
		/// </summary>
		/// <param name="cont">Control whose image is to be saved</param>
		/// <param name="fileName">Name of destination file</param>
		/// <param name="bpp">Bits per pixel</param>
		/// <param name="width">Width of bitmap (pixels)</param>
		/// <param name="height">Height of bitmap (pixels)</param>
		/// <returns>Result: true if succeeded, false otherwise</returns>
		static public bool SaveToFile(Control cont, String fileName, short bpp, int width, int height)
		{
			// File and data streaming
			BinaryWriter bw = null;
			MemoryStream ms = null;
			FileStream fs = null;

			try
			{
				// Create the necessary bitmap file and info headers (must do info first)
				BITMAPINFOHEADER infoHdr = new BITMAPINFOHEADER(bpp, width, height);
				BITMAPFILEHEADER fileHdr = new BITMAPFILEHEADER(infoHdr);

				// Create an array of bytes to hold the actual bitmap data.
				byte[] bytes = new byte[fileHdr.bfSize];

				// Map that array of bytes to a MemoryStream and create a BinaryWriter
				ms = new MemoryStream(bytes);
				bw = new BinaryWriter(ms);

				// Write the file and info headers to the binary stream
				fileHdr.Store(bw);
				infoHdr.Store(bw, false);

				// Get the HWND of the Control
				bool captureState = cont.Capture;
				cont.Capture = true;
				IntPtr hwnd = Windows.GetCapture();
				cont.Capture = captureState;

				// Write the pixel info to the byte stream (bypassing bw)
				if (!CopyImageSource(hwnd, bytes, fileHdr, infoHdr))
				{
					return false;
				}

				// Create a file stream to the path provided
				fs = File.Create(fileName);

				// Write out our bitmap data to the file stream
				fs.Write(bytes, 0, (int)fileHdr.bfSize);
			}
			finally
			{
				// Close streams still laying around.
				if (fs != null)
					fs.Close();
				if (bw != null)
					bw.Close();
				if (ms != null)
					ms.Close();
			}

			return true;
		}

		/// <summary>
		/// Saves an Image object as a .bmp file - very slowly!
		/// </summary>
		/// <param name="image">Image object to be saved</param>
		/// <param name="fileName">Name of destination file</param>
		/// <param name="bpp">Bits per pixel of target bitmap (16,24)</param>
		/// <returns></returns>
		static public bool SaveToFile(Image image, String fileName, short bpp)
		{
			// File and data streaming
			BinaryWriter bw = null;
			MemoryStream ms = null;
			FileStream fs = null;

			try
			{
				// Create the necessary bitmap file and info headers (must do info first)
				BITMAPINFOHEADER infoHdr = new BITMAPINFOHEADER(bpp, image.Width, image.Height);
				BITMAPFILEHEADER fileHdr = new BITMAPFILEHEADER(infoHdr);

				// Create an array of bytes to hold the actual bitmap data.
				byte[] bytes = new byte[fileHdr.bfSize];

				// Map that array of bytes to a MemoryStream and create a BinaryWriter
				ms = new MemoryStream(bytes);
				bw = new BinaryWriter(ms);

				// Write the file and info headers to the binary stream
				fileHdr.Store(bw);
				infoHdr.Store(bw, false);

				// Write the pixel info to the byte stream
				// Remember that the bitmap file is stored upside-down
				Bitmap bm = (Bitmap)image;
				for (int r = image.Height - 1; r >= 0; r--)
				{
					for (int c = 0; c < image.Width; c++)
					{
						int color = bm.GetPixel(c, r).ToArgb();

						// 24 bit bmp is BGR format but color is ARGB
						uint red = (byte)((color & 0x00ff0000) >> 16);
						uint green = (byte)((color & 0x0000ff00) >> 8);
						uint blue = (byte)(color & 0x000000ff);

						if (bpp == 16)
						{
							// We are losing some resolution going to 16-bit
							// Assume 555 format for simplicity
							red = red >> 3;
							green = green >> 3;
							blue = blue >> 3;

							// Construct the pixel
							ushort dstColor = (ushort)((red << 10) | (green << 5) | blue);
							bw.Write(dstColor);
						}
						else
						{
							// 24 bit bmp is BGR format but color is ARGB
							bw.Write((byte)blue);
							bw.Write((byte)green);
							bw.Write((byte)red);
						}
					}
				}

				// Create a file stream to the path provided
				fs = File.Create(fileName);

				// Write out our bitmap data to the file stream
				fs.Write(bytes, 0, (int)fileHdr.bfSize);
			}
			finally
			{
				// Close streams still laying around.
				if (fs != null)
					fs.Close();
				if (bw != null)
					bw.Close();
				if (ms != null)
					ms.Close();
			}

			return true;
		}

	}
}