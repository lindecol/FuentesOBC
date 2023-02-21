using System;
namespace MovilidadCF.Symbol
{
	/// <summary>
	/// Enumarción tipos de código de barra soportados
	/// </summary>
	public enum BarcodeType
	{
		UPCE0,			// The UPC-E0 symbology. 
		UPCE1,			// The UPC-E1 symbology. 
		UPCA,			// The UPC-A symbology. 
		MSI,			// The MSI symbology. 
		EAN8,			// The EAN-8 symbology. 
		EAN13,			// The EAN-13 symbology. 
		CODABAR,		// The CODABAR symbology. 
		CODE39,			// The CODE-39 symbology. 
		D2OF5,			// The Discrete 2 of 5 symbology. 
		I2OF5,			// The Interleaved 2 of 5 symbology. 
		CODE11,			// The CODE-11 symbology. 
		CODE93,			// The CODE-93 symbology. 
		CODE128,		// The CODE-128 symbology. 
		EAN128,			// The EAN-128 symbology. 
		PDF417,			// The PDF 417 symbology. 
		ISBT128,		// The ISBT 128 symbology. 
		TRIOPTIC39,		// The TRIOPTIC 3 of 9 symbology. 
		COUPON,			// The COUPON CODE symbology. 
		BOOKLAND,		// The BOOKLAND EAN symbology. 
		MICROPDF,		// The MICRO PDF symbology. 
		CODE32,			// The CODE-32 symbology. 
		MACROPDF,		// The MACRO PDF symbology. 
		MAXICODE,		// The MAXICODE symbology. 
		DATAMATRIX,		// The DATAMATRIX symbology. 
		QRCODE,			// The QRCODE symbology. 
		MACROMICROPDF,  // The MACRO MICRO PDF symbology. 
		RSS14,			// The RSS-14 symbology. 
		RSSLIM,			// The RSS limited symbology. 
		RSSEXP,			// The RSS expanded symbology. 
		POINTER,		// Pointer label type. 
		IMAGE,			// Image label type. 
		SIGNATURE,		// Signature label type. 
		RESERVED_53,	// RESERVED. 
		WEBCODE,		// The Scanlet WEBCODE symbology. 
		CUECODE,		// The CUE CAT CODE symbology. 
		COMPOSITE_AB,	// The COMPOSITE AB symbology. 
		COMPOSITE_C,	// The COMPOSITE C symbology. 
		TLC39,			// The TCIF Linked CODE 39 symbology. 
		USPOSTNET,		// The US POSTNET symbology. 
		USPLANET,		// The US PLANET symbology. 
		UKPOSTAL,		// The UK POSTAL symbology. 
		JAPPOSTAL,		// The JAPANESE POSTAL symbology. 
		AUSPOSTAL,		// The AUSTRALIAN POSTAL symbology. 
		DUTCHPOSTAL,	// The DUTCH POSTAL symbology. 
		CANPOSTAL,		// The CANADIAN POSTAL symbology. 
		UNKNOWN			// The symbology or labeltype is unknown. 
	}
	
	/// <summary>
	/// Descripción breve de BarcodeReaderEventArgs.
	/// </summary>
	public class ScanCompleteArgs
	{
		public string Text = "";		
		public int Length = 0;
		public BarcodeType Type = BarcodeType.UNKNOWN;
	}

}
