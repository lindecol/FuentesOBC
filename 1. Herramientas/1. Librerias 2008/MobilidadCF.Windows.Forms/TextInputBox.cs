using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using OpenNETCF.Windows.Forms;
using OpenNETCF.Win32;
using OpenNETCF.Media;

namespace MovilidadCF.Windows.Forms
{
#if NETCFDESIGNTIME
	[ToolboxItemFilter("NETCF",ToolboxItemFilterType.Require),
	ToolboxItemFilter("System.CF.Windows.Forms", ToolboxItemFilterType.Custom)]
#endif
	/// <summary>
	/// TextBox Especializado para la entrada de datos texto con caracteristicas avanzadas 
	/// </summary>		
	public class TextInputBox : InputBoxBase
	{			
		#region Constructor, Dispose y Código Requerido por el diseñador

		/// <summary>
		/// Required designer variable.
		/// </summary> 
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public TextInputBox() 
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

		private bool m_bAcceptLetters = true;
		private bool m_bAcceptNumbers = true;
		private bool m_bAcceptSpaces = true;
		private bool m_bAcceptSymbols = true;
		private bool m_bAcceptPunctuations = true;
		private CaseStyle m_CaseStyle = CaseStyle.None;

		public bool AcceptLetters
		{
			get
			{
				return m_bAcceptLetters;
			}
			set
			{
				m_bAcceptLetters = value;
			}
		}

		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		#endif
		public bool AcceptNumbers
		{
			get
			{
				return m_bAcceptNumbers;
			}
			set
			{
				m_bAcceptNumbers = value;
			}
		}

		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Comportamiento")]
		[System.ComponentModel.DefaultValueAttribute(true)]
		[System.ComponentModel.Description("Indica si se permite entrada caracteres de simbolos")]
		#endif
		public bool AcceptSymbols
		{
			get
			{
				return m_bAcceptSymbols;
			}
			set
			{
				m_bAcceptSymbols = value;
			}
		}

		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Comportamiento")]
		[System.ComponentModel.DefaultValueAttribute(true)]
		[System.ComponentModel.Description("Indica si se permite entrada de espacios")]
		#endif
		public bool AcceptSpaces
		{
			get
			{
				return m_bAcceptSpaces;
			}
			set
			{
				m_bAcceptSpaces = value;
			}
		}


		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Comportamiento")]
		[System.ComponentModel.DefaultValueAttribute(true)]
		[System.ComponentModel.Description("Indica si se permite entrada caracteres de puntuación")]
		#endif
		public bool AcceptPunctuations
		{
			get
			{
				return m_bAcceptPunctuations;
			}
			set
			{
				m_bAcceptPunctuations = value;
			}
		}

		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Comportamiento")]
		[System.ComponentModel.DefaultValueAttribute(CaseStyle.None)]
		#endif
		public CaseStyle CaseStyle
		{
			get
			{
				return m_CaseStyle;
			}
			set
			{
				m_CaseStyle = value;
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if ( value.ToUpper().StartsWith(this.GetType().Name.ToUpper()) )
					value = "";
			base.Text = value;
			}
		}

        /// <summary>
        /// Caracteres que serán aceptados independientemente de su clasificación
        /// </summary>
        private string m_sValidChars = "";
        public string ValidChars
        {
            get
            {
                return m_sValidChars;
            }
            set
            {
                m_sValidChars = value;
            }
        }

        /// <summary>
        /// Caracteres que será rechazados independientemente de su clasificación
        /// </summary>
        private string m_sInvalidChars = "";
        public string InvalidChars
        {
            get
            {
                return m_sInvalidChars;
            }
            set
            {
                m_sInvalidChars = value;
            }
        }


		#endregion
	
		#region Implementacion funcionalidad del control

		
		protected string m_LastValue = "";
				
		/// <summary>
		/// Valida los caracteres que pueden ser utilizados por el usuario
		/// </summary>
		protected  override void OnKeyPress(KeyPressEventArgs e)
		{
			e.Handled = true;
            if (e.KeyChar == '\r' && AcceptsReturn == false)
                e.Handled = true;
            else if (Char.IsControl(e.KeyChar))
                e.Handled = false;
            else if (CharsValidator.IsLetter(e.KeyChar))
            {
                if (AcceptLetters)
                {
                    e.Handled = false;
                    switch (CaseStyle)
                    {
                        case CaseStyle.UpperCase:
                            if (Char.IsLower(e.KeyChar))
                            {
                                e.Handled = true;
                                InsertChar(Char.ToUpper(e.KeyChar));
                            }
                            break;
                        case CaseStyle.LowerCase:
                            if (Char.IsUpper(e.KeyChar))
                            {
                                e.Handled = true;
                                InsertChar(Char.ToLower(e.KeyChar));
                            }
                            break;
                        case CaseStyle.CapitalCase:
                            if (SelectionStart == 0 || Char.IsWhiteSpace(Text[SelectionStart - 1]))
                            {
                                if (Char.IsLower(e.KeyChar))
                                {
                                    e.Handled = true;
                                    InsertChar(Char.ToUpper(e.KeyChar));
                                }
                            }
                            else
                            {
                                if (Char.IsUpper(e.KeyChar))
                                {
                                    e.Handled = true;
                                    InsertChar(Char.ToLower(e.KeyChar));
                                }
                            }
                            break;
                    }
                }
            }
            else if (CharsValidator.IsDigit(e.KeyChar))
                e.Handled = !AcceptNumbers;
            else if (e.KeyChar == ' ')
                e.Handled = !AcceptSpaces;
            else if (Char.IsPunctuation(e.KeyChar))
                e.Handled = !AcceptPunctuations;
            else
                e.Handled = !AcceptSymbols;

            if (ValidChars.IndexOf(e.KeyChar) >= 0)
                e.Handled = false;
            if (InvalidChars.IndexOf(e.KeyChar) >= 0)
                e.Handled = true;
            if ( e.Handled )
				SystemSounds.Beep.Play();

			base.OnKeyPress (e);
		}	

		

		#endregion

        #region Implementacion interfaz IInputControl

        public override bool IsInputValid()
        {
            //PARA ESTE CONTROL TODAS LAS ENTRADAS DE TEXTO SON VALIDAS
            return true;
        }
        
        #endregion
    
    }
}
