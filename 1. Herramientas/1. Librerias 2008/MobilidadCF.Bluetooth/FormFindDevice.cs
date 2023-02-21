using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MovilidadCF.Windows.Forms;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Forma que permite realizar la busqueda y seleccionar un dispositivo
	/// con el cual se va a establecer comunicacion
	/// </summary>
	internal class FormFindDevice : System.Windows.Forms.Form
	{
	
#region Código generado por el Diseñador de Windows Forms
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.ListBox lstDevices;
		private System.Windows.Forms.Button btnReintentar;
		private System.Windows.Forms.Button btnAceptar;
		private System.Windows.Forms.Button btnCancelar;
        private MovilidadCF.Windows.Forms.UIHandler uiHandler1;
		private System.Windows.Forms.PictureBox pictureBox1;

		private FormFindDevice()
		{
			//
			// Necesario para admitir el Diseñador de Windows Forms
			//
			InitializeComponent();

			//
			// TODO: agregar código de constructor después de llamar a InitializeComponent
			//
		}

		/// <summary>
		/// Limpiar los recursos que se estén utilizando.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}


		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFindDevice));
            this.label1 = new System.Windows.Forms.Label();
            this.lstDevices = new System.Windows.Forms.ListBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.btnReintentar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiHandler1 = new MovilidadCF.Windows.Forms.UIHandler();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(32, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 16);
            this.label1.Text = "Buscando Dispositivos";
            // 
            // lstDevices
            // 
            this.lstDevices.Location = new System.Drawing.Point(8, 40);
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(224, 156);
            this.lstDevices.TabIndex = 2;
            // 
            // btnReintentar
            // 
            this.btnReintentar.Location = new System.Drawing.Point(8, 208);
            this.btnReintentar.Name = "btnReintentar";
            this.btnReintentar.Size = new System.Drawing.Size(72, 20);
            this.btnReintentar.TabIndex = 3;
            this.btnReintentar.Text = "Reintentar";
            this.btnReintentar.Click += new System.EventHandler(this.btnReintentar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(88, 208);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(64, 20);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(160, 208);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(72, 20);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(11, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            // 
            // uiHandler1
            // 
            this.uiHandler1.HelpControl = null;
            this.uiHandler1.Parent = null;
            // 
            // FormFindDevice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstDevices);
            this.Controls.Add(this.btnReintentar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.Menu = this.mainMenu1;
            this.Name = "FormFindDevice";
            this.Text = "Buscar Dispositivo";
            this.Load += new System.EventHandler(this.frmFindDevice_Load);
            this.ResumeLayout(false);

		}
		#endregion

		// Dispositivo seleccionado
		public BluetoothDevice SelectedDevice = null;		
		private BluetoothDeviceList m_Devices;
		private  IBluetoothProvider m_btProvider;

		/// <summary>
		/// Crea y muestra la forma
		/// </summary>
		/// <returns></returns>
		public static BluetoothDevice Run(IBluetoothProvider btProvider)
		{
			FormFindDevice frm = new FormFindDevice();
			BluetoothDevice device = null;
			frm.m_btProvider = btProvider;
			if ( UIHandler.ShowDialog(frm) == DialogResult.OK )
				device = frm.SelectedDevice;
			frm.Dispose();
			return device;			
		}
	
		// Realiza busqueda de dispositivos
		private void BuscarDispositivos()
		{
			lstDevices.Items.Clear();

			// Se buscan los dispositivos cercanos y se agregan a la lista
			m_btProvider.PowerMode = PowerMode.Connectable;
			m_Devices = m_btProvider.DiscoverDevices(10, 10);					
			if ( m_Devices != null )
			{
				for(int i=0; i < m_Devices.Count; i++)
					lstDevices.Items.Add(m_Devices[i].Name);
				if ( m_Devices.Count > 0 )
					lstDevices.SelectedIndex = 0;
				lstDevices.Focus();
			}
		}

		/// <summary>
		/// Inicializa los controles y realiza una primera búsqueda
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmFindDevice_Load(object sender, System.EventArgs e)
		{
			this.uiHandler1.Parent = this;
			this.Refresh();			
			BuscarDispositivos();
			if ( lstDevices.Items.Count > 0 )
				lstDevices.Focus();
			else
				btnReintentar.Focus();
			UIHandler.EndWait();
		}

		/// <summary>
		/// Realiza una nueva búsqueda de dispositivos cercanos
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReintentar_Click(object sender, System.EventArgs e)
		{
			UIHandler.StartWait();
			BuscarDispositivos();
			UIHandler.EndWait();
		}

		/// <summary>
		/// Cancela la busqueda de dispositivos
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancelar_Click(object sender, System.EventArgs e)
		{
			UIHandler.StartWait();
			SelectedDevice = null;
			DialogResult = DialogResult.Cancel;
			UIHandler.EndWait();
		}

		/// <summary>
		/// Si hay un dispositivo seleccionado cierra la forma y asigna la
		/// variable con la información del dispositivo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAceptar_Click(object sender, System.EventArgs e)
		{
			//Cambio Debido a que el valor de indice NO es el Valor de la posicion //Del Vector
			int nIndex = BuscarPosicionDispositivo(lstDevices.SelectedItem.ToString());
			
			if ( nIndex >= 0 )
			{
				UIHandler.StartWait();
				SelectedDevice = new BluetoothDevice(m_Devices[nIndex].Address, 
					m_Devices[nIndex].Name);
				DialogResult = DialogResult.OK;
			}
			else
			{
				if ( nIndex == -2 )
				{
					UIHandler.ShowAlert("El dispositivo seleccionado se encuentra apagado!");
				}
				else
				{
					UIHandler.ShowAlert("Seleccione un dispositivo!");
				}
			}
		}	

		/// <summary>
		/// Funcion Para Extreer la posicion Real en el Vector de Dispositivos
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private int BuscarPosicionDispositivo(string sNombreDispositivo)
		{
			for(int i=0; i < m_Devices.Count; i++)
			{
				try
				{
					if ( sNombreDispositivo == m_Devices[i].Name)
					{
						return i;
					}
				}
				catch
				{
				}
			}
			return -2;
		}
	}
}
