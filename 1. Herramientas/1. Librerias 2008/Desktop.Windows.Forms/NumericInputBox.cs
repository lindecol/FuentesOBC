using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Media;

namespace Desktop.Windows.Forms
{
	
	/// <summary>
	/// TextBox Especializado para la entrada de datos númericos
	/// </summary>		
    public class NumericInputBox : InputBoxBase
    {			
		#region Constructor, Dispose y Código Requerido por el diseñador

		/// <summary>
		/// Required designer variable.
		/// </summary> 
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public NumericInputBox() 
		{
			// This call is required by the Windows.Forms Designer.
            InitializeComponent();			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) 
		{
			if( disposing ) 
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the Code Editor.
		/// </summary>
		private void InitializeComponent() 
		{

		}

		#endregion

		#region Propiedades del control

		private bool m_bAllowNegative = false;
		private bool m_bAcceptZero = false;
		private int m_nDecimals = 0;
		private string m_sFormat = "";
        private char m_separadorDecimales = ObtenerCaracterDecimales();

    	public bool AllowNegative
		{
			get
			{
				return m_bAllowNegative;
			}
			set
			{
				m_bAllowNegative = value;
			}
		}

		public bool AcceptZero
		{
			get
			{
				return m_bAcceptZero;
			}
			set
			{
				m_bAcceptZero = value;
			}
		}
		public int Decimals
		{
			get
			{
				return m_nDecimals;
			}
			set
			{
				if ( value < 0 )
					value = 0;
				m_nDecimals = value;
			}
		}

		protected static string m_sValidFormatChars = "#0,.";

		public string Format
		{
			get
			{
				return m_sFormat;
			}
			set
			{
				m_sFormat = value;			

				// se eliminan los caracteres no validos
				int i = 0;
				while(i < m_sFormat.Length)
				{
					if ( m_sValidFormatChars.IndexOf(m_sFormat[i]) == -1) 
						m_sFormat = m_sFormat.Remove(i, 1);
					else
						i++;
				}					
			}
        }
        public string UnformatText
		{
			get
			{
                int nDotPos = Text.IndexOf(m_separadorDecimales);
				if ( nDotPos >= 0)
					Text = Text.TrimEnd('0');
				if ( Text.Length > 0 )
				{
                    if (Text[Text.Length - 1] == m_separadorDecimales)
						Text = Text.Remove(Text.Length-1, 1);			
				}
				return Text;
			}
        }

        public string FormatText
		{
			get 
			{
				string sText = UnformatText;
				if ( sText == "" )
					return "";
				else
                    if (Format == string.Empty)
                    {
                        return sText;
                    }
                    else
                    {
                        return Double.Parse(sText).ToString(Format);
                    }
			}
		}

		// These design time attributes affect appearance of this property in the property grid.
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{                
				if ( value != null )
                    if ( value.StartsWith(this.GetType().Name) )
					    value = "";
				base.Text = value;
			}
		}
		#endregion
	
		#region Implementacion funcionalidad del control

		/// <summary>
		/// Se encarga de quitar el formato de texto antes de permitir la edición
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(EventArgs e)
		{
			Text = UnformatText;	
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Coloca los separadores de miles cuando se pierde el foco
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{	
			Text = FormatText;
			base.OnLostFocus (e);
		}

		/// <summary>
		/// Valida los caracteres que pueden ser utilizados por el usuario
		/// </summary>
		/// <param name="e"></param>
		protected  override void OnKeyPress(KeyPressEventArgs e)
		{            
			e.Handled = true;
			if ( Char.IsControl(e.KeyChar) )
				e.Handled = false;
			else if ( Char.IsDigit(e.KeyChar) )
			{
				if ( Decimals <= 0 )
					e.Handled = false;
				else
				{
                    int nDotPos = Text.IndexOf(m_separadorDecimales);
                    if (nDotPos < 0)
                    {
                        //REVISAR EL NUMERO DE ENTEROS
                        int nCurrentEnteros = Text.Length;
                        int nReplaceEnteros = SelectionLength;
                        if ((nCurrentEnteros - nReplaceEnteros) < (MaxLength - Decimals - 1))
                            e.Handled = false;
                    }
                    else
                    {
                        int nInsertPos = SelectionStart;
                        if (nInsertPos <= nDotPos)
                        {
                            //REVISAR EL NUMERO DE ENTEROS
                            int nCurrentEnteros = nDotPos;
                            int nReplaceEnteros = SelectionLength;
                            if ((nCurrentEnteros - nReplaceEnteros) < (MaxLength - Decimals - 1))
                                e.Handled = false;
                        }
                        else
                        {
                            int nCurrentDecimals = Text.Length - nDotPos - 1;
                            int nReplaceDecimals = SelectionLength;
                            if (nCurrentDecimals - nReplaceDecimals < Decimals)
                                e.Handled = false;
                        }
                    }

				}
			}
			else if ( e.KeyChar == '-' )
			{
				if ( AllowNegative )
				{
					if ( Text == "")
						e.Handled = false;
					else if ( SelectionStart == 0  && Text[0] != '-')
						e.Handled = false;
					else if ( SelectionStart == 0 && SelectionLength > 0)
						e.Handled = false;
				}
			}
			else if ( e.KeyChar == '.' )
			{
                if ( Decimals > 0 )
				{
                    if (Text.IndexOf(m_separadorDecimales) == -1)
                    {
                        e.KeyChar = m_separadorDecimales;
                        e.Handled = false;
                    }
                    
				}
			}
			if ( e.Handled )
				SystemSounds.Beep.Play();
            
			base.OnKeyPress (e);
		}

        private static char ObtenerCaracterDecimales()
        {
            return Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }

        #endregion

        #region Implementacion interfaz IInputControl

        public override bool IsInputValid()
        {
            if (Text.Trim() != "")
            {
                try
                {
                    Double.Parse(UnformatText);
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        #endregion
	}
}
