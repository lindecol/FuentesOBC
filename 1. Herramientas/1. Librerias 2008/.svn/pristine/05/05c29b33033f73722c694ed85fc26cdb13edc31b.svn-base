using System;
using System.Windows.Forms;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Clase utilizada para poder invocar metodos que actualizan la interfaz
	/// de usuario desde hilos adicionales
	/// </summary>
	internal class Invoker
	{
		/// <summary>
		/// Referencia a la forma que contiene los controles que se actualizarán
		/// </summary>
		private static Form m_Form;

		/// <summary>
		/// Propiedad que permite asignar la forma con los controles a acutalizar
		/// </summary>
		public static System.Windows.Forms.Form Form
		{
			set
			{
				m_Form = value;
			}
		}


		private Invoker()
		{
			// No se puede instanciar
		}

		/// <summary>
		/// Arreglo de variables que contiene los parametros que deben ser
		/// consultados por la función invocada en la forma
		/// </summary>
		public static object[] m_Params;


		/// <summary>
		/// Propieda que permite acceder a los parametros enviados a una función
		/// </summary>
		public static object[] Params 
		{
			get
			{
				return m_Params;
			}
		}

		/// <summary>
		/// Permite invocar una función y especificar los parametros de la misma
		/// </summary>
		/// <param name="function"></param>
		/// <param name="parameters"></param>
		public static void Invoke(System.Delegate function,  params object[] parameters)
		{
			try
			{
				m_Params = parameters;
				m_Form.Invoke(function);
			}
			catch
			{
			}
		}

		
	}
}
