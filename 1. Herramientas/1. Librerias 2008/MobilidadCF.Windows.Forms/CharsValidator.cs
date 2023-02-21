using System;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// Permite Validar caracteres
	/// </summary>
	internal class CharsValidator
	{
		// Constantes utilizadas en la validaci�n
		private const string AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz��������������";
		private const string NumericChars = "1234567890";
		
		/// <summary>
		/// Constructor privado, no permite crear instancias
		/// </summary>
		private CharsValidator() {}
		

		/// <summary>
		/// Determina si el caracter pasado como argumento es un caracter 
		/// alfab�tico v�lido
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsLetter(char c)
		{
			return AlphaChars.IndexOf(c) >= 0;
		}

		/// <summary>
		/// Determina si el caracter pasado como argumento es un d�gito
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsDigit(char c)
		{
			return NumericChars.IndexOf(c) >= 0;
		}

		/// <summary>
		/// Determina si el caracter pasado como argumento es un d�gito
		/// o una letra v�lida
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsLetterOrDigit(char c)
		{
			return NumericChars.IndexOf(c) >= 0 || AlphaChars.IndexOf(c) >= 0;
		}



	}
}
