using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// BITMAPFILEHEADER class is an implementation of the associated Windows
	/// structure required to save a bitmap to a file.
	/// </summary>
	public class BITMAPFILEHEADER
	{
		// Constants used in the file data structure
		protected const ushort fBitmapFileDesignator = 19778;
		protected const uint fBitmapFileOffsetToDataColor = 54;
		protected const uint fBitmapFileOffsetToDataMono = 62;

		/// <summary>
		/// Creates a BITMAPFILEHEADER class instance from the information provided
		/// by a BITMAPINFOHEADER instance.
		/// </summary>
		/// <param name="infoHdr">Filled in bitmap info header</param>
		public BITMAPFILEHEADER(BITMAPINFOHEADER infoHdr)
		{
			// These are constant for our example
			bfType = fBitmapFileDesignator;
						bfReserved1 = 0;
			bfReserved2 = 0;

			// Determine the size of the pixel data given the bit depth, width, and
			// height of the bitmap.  Note: Bitmap pixel data is always aligned to 4 byte
			// boundaries.
			sizeOfImageData = 0;
			if ( infoHdr.biBitCount == 1)
			{
				bfOffBits = bfOffBits = fBitmapFileOffsetToDataMono;
				uint bytesPerLine = (uint)Math.Ceiling( (double)infoHdr.biWidth / 8 );
				uint extraBytes = (uint)(bytesPerLine % 4);
				if ( extraBytes != 0)
					bytesPerLine += ( 4 - extraBytes );
				sizeOfImageData = (uint)(infoHdr.biHeight * bytesPerLine);
				infoHdr.biSizeImage = sizeOfImageData;
			}
			else
			{
				bfOffBits = fBitmapFileOffsetToDataColor;
				uint bytesPerPixel = (uint)(infoHdr.biBitCount >> 3);
				uint extraBytes = ((uint)infoHdr.biWidth * bytesPerPixel) % 4;
				uint adjustedLineSize = bytesPerPixel * ((uint)infoHdr.biWidth + extraBytes);
					
				// Store the size of the pixel data
				sizeOfImageData = (uint)(infoHdr.biHeight) * adjustedLineSize;
			}

			// Store the total file size
			bfSize = bfOffBits + sizeOfImageData;
		}

		/// <summary>
		/// Write the class data to a binary stream.
		/// </summary>
		/// <param name="bw">Target stream writer</param>
		public void Store(BinaryWriter bw)
		{
			// Must, obviously, maintain the proper order for file writing
			bw.Write(bfType);
			bw.Write(bfSize);
			bw.Write(bfReserved1);
			bw.Write(bfReserved2);
			bw.Write(bfOffBits);
		}

		public uint		sizeOfImageData;	// Size of the pixel data
		public ushort	bfType;				// File type designator "BM"
		public uint		bfSize;				// File size in bytes
		public short	bfReserved1;		// Unused
		public short	bfReserved2;		// Unused
		public uint		bfOffBits;			// Offset to get to pixel info
	}

}
