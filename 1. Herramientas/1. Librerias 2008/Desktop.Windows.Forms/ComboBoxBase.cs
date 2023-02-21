using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
#if NETCFDESIGNTIME
using OpenNETCF;
#endif

namespace Desktop.Windows.Forms
{
	/// <summary>
	/// Descripción breve de ValidableComboBox.
	/// </summary>
	public abstract class ComboBoxBase : System.Windows.Forms.ComboBox, IInputControl
	{
		public ComboBoxBase()
		{
            
        }

		#region Miembros de IInputControl

		private bool m_bRequired = false; 
		private string m_sErrorMessage = "";
        private string m_sHelpText = "";
		
		public bool Required
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

        public virtual bool IsValid
		{
			get
			{
                return (m_sErrorMessage != "");
			}
		}

        public virtual string ErrorMessage
		{
			get
			{	
				return m_sErrorMessage;
			}
            set
            {
                m_sErrorMessage = "";
            }
		}		

        private int m_nLastSelectedIndex = -1;
        public virtual bool HasInputChanged()
        {
            bool bHasInputChanged = m_nLastSelectedIndex != SelectedIndex;
			m_nLastSelectedIndex = SelectedIndex;
            return bHasInputChanged;
        }

        public virtual bool IsInputValid()
        {
            return true;
        }

        public virtual bool IsInputEmpty()
        {
            return true;
        }

        public virtual string HelpText
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

        #endregion
    }
}
