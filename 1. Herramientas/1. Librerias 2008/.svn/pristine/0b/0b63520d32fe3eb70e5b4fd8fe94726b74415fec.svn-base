using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;
using OpenNETCF.Windows.Forms;
using OpenNETCF.Threading;
using OpenNETCF.Media;
using OpenNETCF.Win32;
using OpenNETCF.ComponentModel;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// Descripción breve de ValidatorProvider.
	/// </summary>
    public class UIHandler : System.ComponentModel.Component
	{
	
		#region  Código generado por el Diseñador de componentes

		// Required designer variable.
		private System.ComponentModel.Container components = null;

		// Método necesario para admitir el Diseñador. No se puede modificar
		// el contenido del método con el editor de código.
		private void InitializeComponent()
		{			
		}
		#endregion

		#region Variables miembro y propiedades privadas de la clase

        private static UIHandler m_ActiveUIHandler = null;
		
		#endregion
		
		#region Constructor y Dispose

		public UIHandler()
		{
			// Requerido para la compatibilidad con el Diseñador de composiciones de clases Windows.Forms
			InitializeComponent();

            if ( ! VSHost.DesignMode() )
                CheckMessageFilter();
        }

		protected override void Dispose(bool disposing)
		{
			if ( this.components != null )
				this.components.Dispose();
			base.Dispose (disposing);
		}

		#endregion
		
		#region Propieades estaticas de configuración

		private static KeyInfo m_EscapeKey = new KeyInfo((int)Keys.Escape, (char)Keys.Escape);
		private static KeyInfo m_ActionKey = new KeyInfo((int) Keys.Enter, '\r');
		private static KeyInfo m_TabKey = new KeyInfo((int) Keys.Tab, '\t');
		private static KeyInfo m_MenuKey = new KeyInfo(0, (char) 0 );
		private static KeyInfo m_PageNavigationKey = new KeyInfo( (int) 0, (char) 0);
		private static bool m_bDropDownComboBoxes = false;
		
        	
		public static KeyInfo TabKey
		{
			get
			{
				return m_TabKey;
			}
			set
			{
				if ( value != null )
					m_TabKey	= value;
			}
		}

		public static KeyInfo EscapeKey
		{
			get
			{
				return m_EscapeKey;
			}
			set
			{
				if ( value != null )
					m_EscapeKey	= value;
			}
		}

		public static KeyInfo MenuKey
		{
			get
			{
				return m_MenuKey;
			}
			set
			{
				if ( value != null )
					m_MenuKey	= value;
			}
		}

		public static KeyInfo ActionKey
		{
			get
			{
				return m_ActionKey;
			}
			set
			{
				if ( value != null )
					m_ActionKey	= value;
			}
		}

		public static KeyInfo PageNavigationKey
		{
			get
			{
				return m_PageNavigationKey;
			}
			set
			{
				if ( value != null )
					m_PageNavigationKey	= value;
			}
		}

		public static bool DropDownComboBoxes
		{
			get
			{
				return m_bDropDownComboBoxes;
			}
			set
			{
				m_bDropDownComboBoxes = value;
			}
		}

        private static bool m_bSIPEnabled = false;
        private static InputPanel m_InputPanel;


        public static bool SIPEnabled
        {
            get
            {
                return m_bSIPEnabled;
            }
            set
            {
                m_bSIPEnabled = value;
                if (m_bSIPEnabled)
                {
                    if (m_InputPanel == null)
                    {
                        m_InputPanel = new InputPanel();
                        m_InputPanel.EnabledChanged += new EventHandler(SIPEnabledChangedHandler);
                    }
                }
                else
                {
                    if (m_InputPanel != null)
                    {
                        m_InputPanel.Dispose();
                        m_InputPanel = null;
                    }
                }
            }
        }

        //PROPIEDAD PARA CONFIGURAR EL TIPO DE ERROR
        private static ErrorMessageStyles m_ErrorMessageStyle = ErrorMessageStyles.No;
        public static ErrorMessageStyles ErrorMessageStyle
        {
            get { return m_ErrorMessageStyle; }
            set { m_ErrorMessageStyle = value; }
        }

        
		#endregion

		#region Propiedades publicos del control

        // Referencia a la forma que contiene el componente
        private System.Windows.Forms.Form m_ParentForm = null;
        public System.Windows.Forms.Form Parent
		{
			get
			{
				return m_ParentForm;
			}
			set
			{
                if (value != null)
                {
                    if (m_ParentForm != null && m_ParentForm != value)
                        DesregistrarEventosControles(m_ParentForm);
                    m_ParentForm = value;
                    m_ActiveUIHandler = this;
                    if (m_ParentForm != null)
                    {
                        // Se registran los eventos de la forma
                        m_ParentForm = value;
                        m_ParentForm.GotFocus += new EventHandler(ParentForm_GotFocusHandler);
                        m_ParentForm.KeyDown += new KeyEventHandler(KeyDownHandler);
                        m_ParentForm.KeyUp += new KeyEventHandler(KeyUpHandler);
                        m_ParentForm.KeyPress += new KeyPressEventHandler(KeyPressHandler);
                        RegistrarEventosControles(m_ParentForm);

                        // Se inicializan propiedades de la forma requeridas:
                        m_ParentForm.AutoValidate = AutoValidate.EnableAllowFocusChange;
                        m_ParentForm.KeyPreview = true;
                    }
                }
			}
		}

        // Referencia al control que tiene el foco en la forma actual
        private Control m_ActiveControl = null;
        public System.Windows.Forms.Control ActiveControl
        {
            get
            {
                return m_ActiveControl;
            }
        }

        // Referencia a la ventana activa
        private static System.Windows.Forms.Form m_ActiveForm = null;

        public static System.Windows.Forms.Form ActiveForm
        {
            get
            {
                return m_ActiveForm;
            }
        }

        // Referencia al control utilizado para mostrar la ayuda
        private Control m_HelpControl = null;
        public System.Windows.Forms.Control HelpControl
        {
            get
            {
                return m_HelpControl;
            }
            set
            {
                m_HelpControl = value;
            }
        }

        #endregion

        #region Propiedades y métodos para la Validación de controles 
                
        private static bool m_bValidateOnEnter = true;
        
        public static bool ValidateOnEnter
        {
            get { return m_bValidateOnEnter; }
            set { m_bValidateOnEnter = value; }
        }       

        /// <summary>
        /// Indica si al cambiar si los control son validado aún cuando 
        /// no haya cambiado su valor
        /// </summary>
        private static bool m_bValidateOnlyOnChanges;

        /// <summary>
        /// Indica si al cambiar si un control es validado solo cuando su valor
        /// ha cambiado. 
        /// </summary>
        public static bool ValidateOnlyOnChanges
        {
            get { return m_bValidateOnlyOnChanges; }
            set { m_bValidateOnlyOnChanges = value; }
        }


        private bool ValidateControl(IInputControl ctl)
        {
            if (!m_bValidateOnlyOnChanges || ctl.HasInputChanged())
            {
                if (ctl.Required && ctl.IsInputEmpty())
                {
                    if (ctl.ErrorMessage == "")
                    {
                        ctl.ErrorMessage = "Entrada no puede estar vacia";
                    }
                    return false;
                }
                else if (!ctl.IsInputValid())
                {
                    if (ctl.ErrorMessage == "")
                    {
                        ctl.ErrorMessage = "Entrada no válida";
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return ctl.ErrorMessage != "";
            }
        }

        // FUNCION PARA VALIDAR TODOS LOS CONTROLES
        public bool Validate()
        {
           Control[] ControlesForm = new Control[m_ParentForm.Controls.Count];
           m_ParentForm.Controls.CopyTo(ControlesForm,0); 
           return Validate(ControlesForm);
        }

        public bool Validate(params Control[] controls)
        {
            //SE DEBEN RECORRER TODOS LOS CONTROLES PARA VALIDARLOS
            int iNumControles = 0;
            for(iNumControles=0;iNumControles <= controls.Length-1;iNumControles ++)
            {
                //REVISAR SI EL CONTROL IMPLEMENTA LA INTERFAZ IInputControl
                if (controls[iNumControles] is IInputControl)
                {
                    //REVISAR SI EL CONTROL ESTA VISIBLE Y HABILITADO
                    if (controls[iNumControles].Visible && controls[iNumControles].Enabled)
                    {
                        //REVISAR SI EL CONTROL TIENE HIJOS
                        if (controls[iNumControles].Controls.Count > 0)
                        {
                            Control[] ControlesForm = new Control[controls[iNumControles].Controls.Count];
                            controls[iNumControles].Controls.CopyTo(ControlesForm, 0);
                              //LLAMAR RECURSIVAMENTE VALIDATE
                            Validate(ControlesForm);
                        }
                        else
                        {
                            //VALIDAR EL CONTROL
                            if (!ValidateControl((IInputControl)controls[iNumControles]))
                            {
                                IInputControl ctl;
                                ctl = (IInputControl)controls[iNumControles];
                                if (ctl.ErrorMessage != "")
                                {
                                    ShowValidationError(ctl.ErrorMessage);
                                }
                                controls[iNumControles].Focus();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region Métodos publicos de la clase para manejo interfaz de usuario

        public void ShowSIP()
		{
            if (m_bSIPEnabled)
                m_InputPanel.Enabled = true;
		}

		public void HideSIP()
		{
            if (m_bSIPEnabled)
				m_InputPanel.Enabled = false;
    	}

        /// <summary>
        /// Permite mostrar una ventana en forma modal, sin perjudicar la 
        /// funcionalidad de componente.
        /// Para cerrar la forma se debe asignar a la propiedad "DialogResult"
        /// un valor diferente a "DialogResult.None". 
        /// No se debe llamar al metodo Close().
        /// </summary>
        /// <param name="form"></param>
        public static DialogResult ShowDialog(System.Windows.Forms.Form form)
        {
            DialogResult res = DialogResult.None;
            
            // Se deshabilita la forma anterior 
            System.Windows.Forms.Form frmParent = m_ActiveForm;
            if (frmParent != null)
                frmParent.Enabled = false;

            // Se muestra la nueva forma
            form.Show();

            // Se ingresa al ciclo de procesamiento de eventos
            for (; ; )
            {
                if (!m_bDoWait)
                    Application2.DoEvents();
                if (form.DialogResult != DialogResult.None)
                {
                    res = form.DialogResult;
                    break;
                }
                if (!m_bDoWait)
                    Thread2.Sleep(100);
            }

            // Se cierra la forma y se da el foco a la ventana anterior y
            // se vuelve a habilitar
            if (frmParent != null)
                frmParent.Enabled = true;
            form.Close();
            if (frmParent != null)
                SetForegroundWindow(frmParent);
            return res;
        }

        public static bool Confirm(string sMsg)
        {
            return Confirm(sMsg, "Confirmar!", true);
        }

        public static bool Confirm(string sMsg, string sTitle)
        {
            return Confirm(sMsg, sTitle, true);
        }

        public static bool Confirm(string sMsg, string sTitle, bool bDefaultYes)
        {
            bool bWaiting = m_bDoWait;
            if (bWaiting)
                EndWait();
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1;
            if (!bDefaultYes)
                defaultButton = MessageBoxDefaultButton.Button2;
            SystemSounds.Question.Play();
            bool bRes = MessageBox.Show(sMsg, sTitle, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, defaultButton) == DialogResult.Yes;
            if (bWaiting)
                StartWait();
            return bRes;
        }


        public static void ShowAlert(string sMsg)
        {
            ShowAlert(sMsg, "Advertencia!");
        }

        public static void ShowAlert(string sMsg, string sTitle)
        {
            bool bWaiting = m_bDoWait;
            if (bWaiting)
                EndWait();
            SystemSounds.Beep.Play();
            MessageBox.Show(sMsg, sTitle, MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            if (bWaiting)
                StartWait();
        }

        public static void ShowError(string sMsg)
        {
            ShowError(sMsg, "Error!");
        }

        public static void ShowError(string sMsg, string sTitle)
        {
            bool bWaiting = m_bDoWait;
            if (bWaiting)
                EndWait();
            SystemSounds.Hand.Play();
            MessageBox.Show(sMsg, sTitle, MessageBoxButtons.OK,
            MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                if (bWaiting)
                    StartWait();
        }

        public static void MoveForm(System.Windows.Forms.Form frm, int Left, int Top, int Width, int Height)
        {
            Win32Window.MoveWindow(frm.Handle, Left, Top, Width, Height);
        }

        public static void MoveForm(System.Windows.Forms.Form frm, int Left, int Top)
        {
            Win32Window.MoveWindow(frm.Handle, Left, Top, frm.Width, frm.Left);
        }
        #endregion

        #region Funciones para mensajes estáticos en pantalla

        /// <summary>
        /// Referencia al control de barra de estado creado por la función
        /// DisplayMessage
        /// </summary>
        private System.Windows.Forms.Control m_StatusBar = null;
        
        private void ShowValidationError(string sMsg)
        {
            if (m_ErrorMessageStyle == ErrorMessageStyles.No)
            {
                SystemSounds.Hand.Play();
                return;
            }
            if (m_ErrorMessageStyle == ErrorMessageStyles.StatusBar)
            {
                DisplayMessage(sMsg, true);
            }
            else
            {
                ShowError(sMsg,"Error");
            }
        }

        private void DisplayAlert(string sMsg)
        {
            if (m_ErrorMessageStyle == ErrorMessageStyles.No)
            {
                SystemSounds.Beep.Play();
                return;
            }
            if (m_ErrorMessageStyle == ErrorMessageStyles.StatusBar)
            {
                DisplayMessage(sMsg, false);
            }
            else
            {
                ShowAlert(sMsg, "Alerta");
            }
        }

        /// <summary>
        /// Función encargada de mostrar un mensaje estatico en pantalla
        /// </summary>
        /// <param name="sMsg">Mensaje a motrar en pantalla</param>
        /// <param name="Type">Tipo de mensaje que se mostrara</param>
        private void DisplayMessage(string sMsg, bool bIsError)
        {
            if (m_StatusBar == null)
            {
                m_StatusBar = new System.Windows.Forms.StatusBar();
                m_StatusBar.KeyDown += new KeyEventHandler(StatusBar_KeyDown);
                m_StatusBar.LostFocus += new EventHandler(StatusBar_LostFocus);
            }
            m_StatusBar.Location = new Point(10,219);
            m_StatusBar.Size = new Size(238, 24);
            if (bIsError)
            {
                m_StatusBar.BackColor = Color.Red;
                m_StatusBar.ForeColor = Color.White;
            }
            else
            {
                m_StatusBar.BackColor = Color.SeaGreen;
                m_StatusBar.ForeColor = Color.White;
            }
            m_StatusBar.Text = sMsg;
            m_StatusBar.Parent = this.Parent;
            m_StatusBar.Visible = true;
            m_StatusBar.BringToFront();
            m_StatusBar.Focus();

            m_ParentForm.Refresh();
            
            if (bIsError)
                SystemSounds.Hand.Play();
            else
                SystemSounds.Asterisk.Play();                
        }

        /// <summary>
		/// Manejador del evento KeyDown registrado para la barra de estado 
		/// utilizada en DisplayMessage para capturar el ENTER del usuario
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void StatusBar_KeyDown(object sender, KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.Enter )
			{
                m_StatusBar.Visible = false;
                m_StatusBar.Parent = null;
                if (m_ActiveControl != null)
                    m_ActiveControl.Focus();
			}
		}

		/// <summary>
		/// Manejador del evento LostFoucus registrado para la barra de estado
		/// utilizada en DisplayMessage, y que evita que el control pierda el
		/// foco.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void StatusBar_LostFocus(object sender, EventArgs e)
		{
            m_StatusBar.Visible = false;
            m_StatusBar.Parent = null;
		}
		#endregion		

        #region Funciones para el manejo del filtro de mensajes y funciones de espera

        // Referencia al filtro de mensajes creado por la clase
        private static UIHandlerMessageFilter m_MsgFilter = null;

        // Crea y registra el filtro de mensajes
        private static void CheckMessageFilter()
        {
            if ( m_MsgFilter == null )
            {
                m_MsgFilter = new UIHandlerMessageFilter();
                Application2.AddMessageFilter(m_MsgFilter);
            }
        }

        // Funciones para mostrar y ocultar el cursor de espera 
        // e ignorar eventos de teclado y mouse/touch screen
        static internal bool m_bDoWait = false;
        static internal bool m_bMenuLoop = false;


        /// <summary>
        /// Muestra el cursor de reloj e Inicia el modo de espera en el cual
        /// se envita que los mensajes de teclado y mouse se acumulen y lleguen 
        /// a la aplicación
        /// </summary>
        public static void StartWait()
        {
            if (!m_bDoWait)
            {
                m_bDoWait = true;
                Cursor.Current = Cursors.WaitCursor;
                CheckMessageFilter();
                Application2.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
            }
        }

        /// <summary>
        /// Termina el modo de espra y oculta el cursor de reloj.
        /// </summary>
        /// <param name="bDoEvents">Indica si debe llamar a la función DoEvents</param>
        public static void EndWait(bool bDoEvents)
        {
            if (m_bDoWait )
            {
                CheckMessageFilter();
                if (bDoEvents)
                {
                    Application2.DoEvents();
                }
                m_bDoWait = false;
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Terminal el modo de espera ejecutando la función DoEvents
        /// </summary>
        public static void EndWait()
        {
            EndWait(true);
        }

        #endregion

		#region Registro y manejo de eventos

		// Registra manejadores a los eventos requeridos de los controles
		private void RegistrarEventosControles(Control control)
		{
			foreach(Control ctl in control.Controls)
			{
				ctl.GotFocus += new EventHandler(ControlGotFocusHandler);
                ctl.LostFocus += new EventHandler(ControlLostFocusHandler);
				if ( ctl.Controls.Count > 0 )
					RegistrarEventosControles(ctl);
			}
		}
		
         
		// Registra manejadores a los eventos requeridos de los controles
		private void DesregistrarEventosControles(Control control)
		{
			foreach(Control ctl in control.Controls)
			{
				ctl.GotFocus -= new EventHandler(ControlGotFocusHandler);
				if ( ctl.Controls.Count > 0 )
					DesregistrarEventosControles(ctl);
			}
		}	

		// Permite agregar controles al manejador cuando estos son creados 
		// dinamicamente
		public void AddControl(Control ctl)
		{
			RegistrarEventosControles(ctl);
		}

		private void ParentForm_GotFocusHandler(object sender, EventArgs e)
		{
            m_ActiveUIHandler = this;
            m_ActiveForm = this.Parent;            
            if ( m_ActiveControl != null )
			{
                try
                {
                    //m_ActiveControl.Focus();
                }
                catch
                {
                }
			}
		}

        private void ParentForm_ClosedHandler(object sender, EventArgs e)
        {
            m_ActiveForm = null;
            if (m_ParentForm != null)
                DesregistrarEventosControles(m_ParentForm);
        }

		private void KeyDownHandler(object sender, KeyEventArgs e)
		{
            e.Handled = false;
			if ( !e.Alt && !e.Control && !e.Shift)
			{
				if  ( (int) e.KeyCode == m_TabKey.KeyCode )
				{
					if ( e.KeyCode != Keys.Tab )
					{
						SendTabKey();
						e.Handled = true;
					}
				}
				else if  ( (int) e.KeyCode == m_EscapeKey.KeyCode )
				{
                    if (m_ActiveControl is TextBoxBase)
					{
                        ((TextBox)m_ActiveControl).Text = "";
						e.Handled = true;
					}
                    else if (m_ActiveControl is ComboBox)
					{
                        ((ComboBox)m_ActiveControl).SelectedIndex = -1;
						e.Handled = true;
					}
				
				}
				else if ( (int) e.KeyCode == m_PageNavigationKey.KeyCode )
				{
                    Control ctl = (Control)m_ActiveControl;
                    if (ctl.Parent != null)
					{
                        TabControl ctlTab = GetTabControl(m_ActiveControl);
						if ( ctlTab != null )
						{
							if ( ctlTab.SelectedIndex < ctlTab.TabPages.Count -1 )
								ctlTab.SelectedIndex++;
							else
								ctlTab.SelectedIndex = 0;
							if ( ctlTab.Focused )
								SendTabKey();
							else
								ctlTab.Focus();
							e.Handled = true;
						}
					}
				}
				if ( (int)e.KeyCode == m_ActionKey.KeyCode) 
				{
				    //PREGUNTAR POR EL FOCO
                    if (m_ActiveControl is System.Windows.Forms.ButtonBase)
					{
                        PerformMouseClick(2, 2, m_ActiveControl);
						e.Handled = true;
					}
                    else if (m_ActiveControl is MovilidadCF.Windows.Forms.IInputControl)
                    {
                        IInputControl ictl = (IInputControl)m_ActiveControl;
                        if (ValidateControl(ictl))
                        {
                            if ( ictl.TabOnEnter)
                                SendTabKey();
                        }
                        else
                        {
                            if (ictl.ErrorMessage != "")
                            {
                                ShowValidationError(ictl.ErrorMessage);
                            }                            
                        }
                    }
				}
				else if ( (int)e.KeyCode == m_MenuKey.KeyCode )
				{
					if ( Parent != null )
					{
						if ( Parent.Menu != null )
						{
							PerformMouseClick(Parent.Left + 5, Parent.Height + 5, Parent);
							e.Handled = true;
						}
					}
				}
	
			}			
		}

		private void KeyUpHandler(object sender, KeyEventArgs e)
		{
			
			if ( !e.Alt && !e.Control && !e.Shift)
			{
				int KeyCode = (int) e.KeyCode;
				if  ( KeyCode == m_TabKey.KeyCode )
				{
					if ( e.KeyCode != Keys.Tab )
					{
						e.Handled = true;
					}
				}
				else if ( KeyCode == m_ActionKey.KeyCode || 
					KeyCode == m_MenuKey.KeyCode ||
					KeyCode == m_EscapeKey.KeyCode || 
					KeyCode == m_PageNavigationKey.KeyCode ) 
				{
                    e.Handled = true;
				}
			}			
		}

		private void KeyPressHandler(object sender, KeyPressEventArgs e)
		{					
			if ( !e.Handled )
			{
				if ( e.KeyChar == m_TabKey.KeyChar )
				{
					if ( e.KeyChar != '\t' )
						e.Handled = true;
				}
				if ( e.KeyChar == m_ActionKey.KeyChar ||
					e.KeyChar == m_MenuKey.KeyChar ||
					e.KeyChar == m_EscapeKey.KeyChar || 
					e.KeyChar == m_PageNavigationKey.KeyChar ) 
				{
                    if (!Char.IsControl(e.KeyChar))
					    e.Handled = true;
				}
			}            
		}

        private static void SIPEnabledChangedHandler(object sender, EventArgs e)
        {
            if (m_InputPanel.Enabled == true) 
            {
                if (m_ActiveUIHandler != null)
                {
                    if (m_ActiveUIHandler.ActiveControl != null)
                    {
                        int DesplazamientoY = 80;
                        int DesplazamientoX = 0;
                        int MaxCoordenadaY = 162;

                        if (m_ActiveUIHandler.ActiveControl.Location.Y > (int)MaxCoordenadaY)
                        {
                            m_ActiveUIHandler.Parent.AutoScrollMargin = new System.Drawing.Size(m_ActiveUIHandler.Parent.Width, m_ActiveUIHandler.Parent.Height);
                            m_ActiveUIHandler.Parent.AutoScrollPosition = new System.Drawing.Point(m_ActiveUIHandler.Parent.Width, m_ActiveUIHandler.Parent.Height);
                            m_ActiveUIHandler.Parent.AutoScroll = true;
                            m_ActiveUIHandler.Parent.AutoScrollMargin = new System.Drawing.Size((int) DesplazamientoX, (int)DesplazamientoY);
                            m_ActiveUIHandler.Parent.AutoScrollPosition = new System.Drawing.Point((int) DesplazamientoX, (int)DesplazamientoY);
                        }
                   }
                }                
            }
            else
            {
                if (m_ActiveUIHandler != null)
                {
                    m_ActiveUIHandler.Parent.AutoScroll = false;
                }
            }
        }

        private void ControlLostFocusHandler(object sender, EventArgs e)
        {
            Control ctl = (Control)sender;
            Font oldFont = null;
            if (m_HelpControl != null)
                m_HelpControl.Text = "";
            if (sender is System.Windows.Forms.ButtonBase )
            {
                // Se quita el subrayado
                if (ctl is System.Windows.Forms.Button)
                {
                    // se restablece el color
                    ctl.BackColor = SystemColors.Control;
                    oldFont = ctl.Font;
                    ctl.Font = new Font(oldFont.Name, oldFont.Size, FontStyle.Bold);
                    oldFont.Dispose();
                }
                else if ( sender is System.Windows.Forms.TabControl)
                {
                    oldFont = ctl.Font;
                    ctl.Font = new Font(oldFont.Name, oldFont.Size, FontStyle.Regular);
                    oldFont.Dispose();
                }
            }
            else if (sender is System.Windows.Forms.ComboBox)
            {
                if (m_bDropDownComboBoxes)
                {
                    // se cierra la lista
                    Win32Window wnd = Win32Window.FromControl(ctl);
                    Win32Window.SendMessage(wnd, (int)CB.SHOWDROPDOWN, 0, 0);
                }
            }
            else if (sender is System.Windows.Forms.ListBox)
            {
                ctl.BackColor = Color.White;
            }
            else if (sender is System.Windows.Forms.TextBox)
            {
                if (m_bSIPEnabled)
                    HideSIP();
            }
        }

		private void ControlGotFocusHandler(object sender, EventArgs e)
		{
			Control ctl = (Control) sender;
			m_ActiveControl = ctl;
            if (ctl is IInputControl)
            {
                if (m_HelpControl != null)
                {
                    m_HelpControl.Text = ((IInputControl)ctl).HelpText;
                }

            }
			if ( ctl is System.Windows.Forms.ButtonBase)
			{
				// Se utiliza el subrayado para resaltar cual control tiene el foco
				ctl.Font = new Font(ctl.Font.Name, ctl.Font.Size,FontStyle.Bold | FontStyle.Underline);
				if ( sender is Button )
				{
					// Si es un boton básico se cambia adicionalemente el color de
					// fondo para resaltarlo más en la interfaz.
					ctl.BackColor = Color.LightSteelBlue;
                }
			}
			else if ( sender is TabControl )
			{
				// No puede obtener el foco, se pasa al siguiente en la lista
				ctl.Font = new Font(ctl.Font.Name, ctl.Font.Size,FontStyle.Underline);

			}
			else if ( sender is ComboBox )
			{
				// Se abre automáticamente la lista
				if ( m_bDropDownComboBoxes )
				{
					Win32Window wnd = Win32Window.FromControl(ctl);
					Win32Window.SendMessage(wnd, (int)CB.SHOWDROPDOWN, 1, 0);
				}

			}
			else if ( sender is ListBox )
			{
				ctl.BackColor = Color.LightGray;
			}
			else if ( sender is TextBox )
			{
				TextBox txtBox = (TextBox)sender;
				txtBox.SelectAll();
                if (m_bSIPEnabled)
                    ShowSIP();
			}
		}         

#endregion

		#region Funciones utilitarias
		
		// función externa para habilitar/deshabilitar dispositivos
		[DllImport("coredll", EntryPoint="DevicePowerNotify", CharSet=CharSet.Unicode)]
		protected static extern long DevicePowerNotify (string Dispositivo, long i, long j);

		// función externa para el envio de mensajes de mouse
		[DllImport("coredll", EntryPoint="mouse_event", CharSet=CharSet.Unicode)]
		protected static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		// Constantes e imports utilizadas para el envio de mensajes windows
		const int MOUSEEVENTF_MOVE = 1;
		const int MOUSEEVENTF_LEFTDOWN = 2;
		const int MOUSEEVENTF_LEFTUP = 4; 
		const int MOUSEEVENTF_ABSOLUTE = 0x8000;		

		private static void SendTabKey()
		{
            SendKeys.Send("\t");
		}

		private static System.Windows.Forms.Form GetParentForm(Control ctl)
		{
			if ( ctl.Parent != null )
			{
				if ( ctl.Parent is System.Windows.Forms.Form )
					return (System.Windows.Forms.Form)ctl.Parent;
				else
					return GetParentForm(ctl.Parent);
			}
			return null;
		}

		private void PerformMouseClick(int aX, int aY, Control form)
		{
			Point p = form.PointToScreen(new Point(aX, aY));
			int m1 = (int)(65535 / Screen.PrimaryScreen.Bounds.Width);
			int m2 =  (int)(65535 / Screen.PrimaryScreen.Bounds.Height);
			int x = m1 * p.X;
			int y = m2 * p.Y;
			mouse_event(2|MOUSEEVENTF_ABSOLUTE, x, y, 0, 0);
			mouse_event(4|MOUSEEVENTF_ABSOLUTE , x, y, 0, 0);
		}
	
		private static TabControl GetTabControl(Control ctl)
		{
			if ( ctl is TabControl )
				return (TabControl)ctl;
			else if ( ctl.Parent != null )
			{
				if ( ctl.Parent is TabControl )
					return (TabControl)ctl.Parent;
				else
					return GetTabControl(ctl.Parent);
			}
			return null;
		}

        /// <summary>
        /// Permite enviar al frente una ventana especfica. Se utiliza para 
        /// que al regresar de un ShowDialog no se pierda el usuario y regresar
        /// siempre a la ventana adecuada
        /// </summary>
        /// <param name="frm"></param>
        public static void SetForegroundWindow(System.Windows.Forms.Form frm)
        {
            Win32Window.SetForegroundWindow(frm.Handle);
        }
	
		#endregion

	}
}
