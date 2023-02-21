using System;
using System.Windows.Forms;
using System.Drawing;
#if NETCFDESIGNTIME
using OpenNETCF.Windows.Forms;
#endif

namespace MovilidadCF.Windows.Forms
{
	
	
	/// <summary>
	/// TextBox Especializado para la entrada de datos númericos
	/// </summary>		
	public abstract class InputBoxBase : TextBox, IInputControl
    {			
		#region Constructor, Dispose y Código Requerido por el diseñador
	
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public InputBoxBase() 
		{			
            
		}

	
		#endregion

		#region Implementación IInputControl
        
        private string m_sErrorMessage = "";
        private string m_LastValue = "";
        private bool m_bRequired = false;
        private string m_sHelpText;
        private bool m_bTabOnEnter = true;

		public virtual bool Required
		{
			get
			{
				return m_bRequired;
			}
			set
			{
				m_bRequired = value;
			}
		}

        bool IInputControl.IsValid
        {
            get
            {
                return m_sErrorMessage != null;
            }
        }

        public bool TabOnEnter
        {
            get {
                return m_bTabOnEnter;
            }
            set{
                m_bTabOnEnter = value;
            }
        }

		public string ErrorMessage
		{
			get
			{
				return m_sErrorMessage;
			}
            set
            {
                m_sErrorMessage = value;
            }
		}

        public string HelpText
        {
            get
            {
                return m_sHelpText;
            }
            set
            {
                m_sHelpText = value;
            }
        }

        public virtual bool HasInputChanged()
        {
            if (m_LastValue != this.Text)
                return true;
            else
                return false;
        }

        public bool IsInputEmpty()
        {
            if (this.Text == "")
                return true;
            else
                return false;
        }

        public abstract bool IsInputValid();

        

        

        #endregion
			
		#region Implementacion funcionalidad del control

		/// <summary>
		/// Inserta un caracter en la posición de inserción actual
		/// del control
		/// </summary>
		/// <param name="c"></param>
		protected void InsertChar(char c)
		{
			int nInsertPos = SelectionStart;
			int nSelectLen = SelectionLength;
			string sText = Text;
			if ( SelectionLength > 0 )
				sText = sText.Remove(nInsertPos, nSelectLen);
			if ( Text.Length < MaxLength )
			{
				sText = sText.Insert(nInsertPos, c.ToString());
				Text = sText;
				SelectionStart = nInsertPos + 1;
			}	
		}

		#endregion

     }
}
