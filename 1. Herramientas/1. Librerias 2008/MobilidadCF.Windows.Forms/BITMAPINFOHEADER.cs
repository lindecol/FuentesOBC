using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// BITMAPINFOHEADER class is an implementation of the associated Windows
	/// structure required to save a bitmap to a file, as well as create a DIB
	/// section for a control.
	/// </summary>
	public class BITMAPINFOHEADER
	{
		// Constants used in the info data structure
		const uint kBitmapInfoHeaderSize = 40;

		/// <summary>
		/// Creates a BITMAPINFOHEADER instance based on a the pixel bit depth, width,
		/// and height of the bitmap.
		/// </summary>
		/// <param name="bpp">Bits per pixel</param>
		/// <param name="w">Bitmap width (pixels)</param>
		/// <param name="h">Bitmap height (pixels)</param>
		public BITMAPINFOHEADER(short bpp, int w, int h)
		{
#if DEBUG
			// For simplicity of the example, only 16 and 24 bit direct formats
			// are supported
			if (bpp !=1 && bpp != 16 && bpp != 24)
				MessageBox.Show("Error: Non-supported bitmap pixel depth sepcified");
#endif
			biSize = kBitmapInfoHeaderSize;
			biWidth = w;			// Set the width
			biHeight = h;			// Set the height
			biPlanes = 1;			// Only use 1 color plane
			biBitCount = bpp;		// Set the bpp
			biCompression = 0;		// No compression for file bitmaps
			biSizeImage = 0;		// No compression so this can be 0
			biXPelsPerMeter = 0;	// Not used
			biYPelsPerMeter = 0;	// Not used
			biClrUsed = 0;			// Not used
			biClrImportant = 0;		// Not used
		}

		/// <summary>
		/// Write the class data to a binary stream.
		/// </summary>
		/// <param name="bw">Target stream writer</param>
		/// <param name="bFromDIB">Is this a memory DIB in memory?</param>
		/// true: Memory DIB so use compression to format pixels if 16-bit
		/// false: File DIB so do not use compression
		public void Store(BinaryWriter bw, bool bFromDIB)
		{
			// Must, obviously, maintain the proper order for file writing
			bw.Write(biSize);
			bw.Write(biWidth);
			bw.Write(biHeight);
			bw.Write(biPlanes);
			bw.Write(biBitCount);
				
			// Only use compression for memory DIB (file loads choke if this is used)
			if (bFromDIB && biBitCount == 16)
				bw.Write(Windows.BI_BITFIELDS);
			else
				bw.Write(biCompression);

			bw.Write(biSizeImage);
			bw.Write(biXPelsPerMeter);
			bw.Write(biYPelsPerMeter);
			bw.Write(biClrUsed);
			bw.Write(biClrImportant);

			// RGBQUAD bmiColors[0]
			if ( biBitCount == 1 )
			{
				// Colores blanco y negro
				bw.Write((uint)0x00000000);
				bw.Write((uint)0x00FFFFFF);				
			}
			else if (bFromDIB && biBitCount == 16)
			{
				bw.Write((uint)0x7c00);		// red
				bw.Write((uint)0x03e0);		// green
				bw.Write((uint)0x001f);		// blue
			}			
		}

		public uint		biSize;				// Size of this structure
		public int		biWidth;			// Width of bitmap (pixels)
		public int		biHeight;			// Height of bitmap (pixels)
		public short	biPlanes;			// Number of color planes
		public short	biBitCount;			// Pixel bit depth
		public uint		biCompression;		// Compression type
		public uint		biSizeImage;		// Size of uncompressed image
		public int		biXPelsPerMeter;	// Horizontal pixels per meter
		public int		biYPelsPerMeter;	// Vertical pixels per meter
		public uint		biClrUsed;			// Number of colors used
		public uint		biClrImportant;		// Important colors
	}

}
