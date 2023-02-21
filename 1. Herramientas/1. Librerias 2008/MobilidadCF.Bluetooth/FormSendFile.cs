using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MovilidadCF.Windows.Forms;

namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Forma que realiza el envío de archivos de una PDA y muestra
	/// el estado del avance de la transmisión.
	/// </summary>
	internal class FormSendFile : System.Windows.Forms.Form
	{
#region Código generado por el Diseñador de Windows Forms

		private System.Windows.Forms.ListBox lstDevices;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnEnviar;
		private System.Windows.Forms.Label lblArchivo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.Button btnBuscar;
		private System.Windows.Forms.Button btnCancelar;
		private System.Windows.Forms.Label label1;

		private const int BUFFER_SIZE = 32768;
		private System.Windows.Forms.Panel pnlAvance;
		private System.Windows.Forms.Panel pnlInicio;
		private System.Windows.Forms.Label lblEstado;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ProgressBar ctlProgress;
		private System.Windows.Forms.Button btnCancelarEnvio;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblDestino;
		private System.Windows.Forms.Label lblArchivo2;
        private MovilidadCF.Windows.Forms.UIHandler uiHandler1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private byte[] m_Buffer = new byte[BUFFER_SIZE];
	
		private FormSendFile()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSendFile));
            this.label1 = new System.Windows.Forms.Label();
            this.lstDevices = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlAvance = new System.Windows.Forms.Panel();
            this.lblArchivo2 = new System.Windows.Forms.Label();
            this.lblDestino = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ctlProgress = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancelarEnvio = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.pnlInicio = new System.Windows.Forms.Panel();
            this.uiHandler1 = new MovilidadCF.Windows.Forms.UIHandler();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlAvance.SuspendLayout();
            this.pnlInicio.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(32, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 16);
            this.label1.Text = "Enviar Archivo";
            // 
            // lstDevices
            // 
            this.lstDevices.Location = new System.Drawing.Point(8, 67);
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(224, 86);
            this.lstDevices.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 16);
            this.label2.Text = "Seleccione el equipo destino:";
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(88, 160);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(64, 20);
            this.btnEnviar.TabIndex = 2;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(160, 160);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(72, 20);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblArchivo
            // 
            this.lblArchivo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblArchivo.Location = new System.Drawing.Point(8, 25);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(216, 15);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(216, 16);
            this.label3.Text = "Archivo a enviar:";
            // 
            // pnlAvance
            // 
            this.pnlAvance.Controls.Add(this.lblArchivo2);
            this.pnlAvance.Controls.Add(this.lblDestino);
            this.pnlAvance.Controls.Add(this.label9);
            this.pnlAvance.Controls.Add(this.label7);
            this.pnlAvance.Controls.Add(this.label5);
            this.pnlAvance.Controls.Add(this.ctlProgress);
            this.pnlAvance.Controls.Add(this.label4);
            this.pnlAvance.Controls.Add(this.btnCancelarEnvio);
            this.pnlAvance.Controls.Add(this.lblEstado);
            this.pnlAvance.Location = new System.Drawing.Point(0, 32);
            this.pnlAvance.Name = "pnlAvance";
            this.pnlAvance.Size = new System.Drawing.Size(240, 200);
            // 
            // lblArchivo2
            // 
            this.lblArchivo2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblArchivo2.Location = new System.Drawing.Point(72, 72);
            this.lblArchivo2.Name = "lblArchivo2";
            this.lblArchivo2.Size = new System.Drawing.Size(152, 20);
            // 
            // lblDestino
            // 
            this.lblDestino.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblDestino.Location = new System.Drawing.Point(72, 40);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(152, 20);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(8, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(152, 20);
            this.label9.Text = "Estado Transmisión:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 20);
            this.label7.Text = "Archivo:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 20);
            this.label5.Text = "Destino:";
            // 
            // ctlProgress
            // 
            this.ctlProgress.Location = new System.Drawing.Point(8, 154);
            this.ctlProgress.Name = "ctlProgress";
            this.ctlProgress.Size = new System.Drawing.Size(224, 16);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(224, 20);
            this.label4.Text = "Datos Transmisión";
            // 
            // btnCancelarEnvio
            // 
            this.btnCancelarEnvio.Location = new System.Drawing.Point(84, 176);
            this.btnCancelarEnvio.Name = "btnCancelarEnvio";
            this.btnCancelarEnvio.Size = new System.Drawing.Size(72, 20);
            this.btnCancelarEnvio.TabIndex = 7;
            this.btnCancelarEnvio.Text = "Cancelar";
            this.btnCancelarEnvio.Click += new System.EventHandler(this.btnCancelarEnvio_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(8, 136);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(224, 16);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(8, 160);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(72, 20);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // pnlInicio
            // 
            this.pnlInicio.Controls.Add(this.lstDevices);
            this.pnlInicio.Controls.Add(this.btnBuscar);
            this.pnlInicio.Controls.Add(this.btnEnviar);
            this.pnlInicio.Controls.Add(this.btnCancelar);
            this.pnlInicio.Controls.Add(this.label2);
            this.pnlInicio.Controls.Add(this.label3);
            this.pnlInicio.Controls.Add(this.lblArchivo);
            this.pnlInicio.Location = new System.Drawing.Point(0, 32);
            this.pnlInicio.Name = "pnlInicio";
            this.pnlInicio.Size = new System.Drawing.Size(240, 200);
            // 
            // uiHandler1
            // 
            this.uiHandler1.HelpControl = null;
            this.uiHandler1.Parent = null;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(8, 8);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            // 
            // FormSendFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(236, 270);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlInicio);
            this.Controls.Add(this.pnlAvance);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormSendFile";
            this.Text = "Enviar Archivo";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSendFile_Closing);
            this.Load += new System.EventHandler(this.frmSendFile_Load);
            this.pnlAvance.ResumeLayout(false);
            this.pnlInicio.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Variable que guarda la referencia a la instancia del proveedor 
		/// de bluetooth que debe ser utilizado
		/// </summary>
		IBluetoothProvider m_btProvider = null;

		/// <summary>
		/// Inicializa los controles y datos requeridos
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmSendFile_Load(object sender, System.EventArgs e)
		{
			this.uiHandler1.Parent = this;
			lblArchivo.Text = Path.GetFileName(m_FileName);
			pnlAvance.Visible = false;
			pnlInicio.Visible = true;
			this.Refresh();
			btnBuscar.Focus();
			BuscarDispositivos();
			UIHandler.EndWait();
		}

		/// <summary>
		/// Dispositivo seleccionado
		/// </summary>
		public BluetoothDevice SelectedDevice = null;

		/// <summary>
		/// Dispositivos encontrados en la busqueda
		/// </summary>
		private BluetoothDeviceList m_Devices;		

		/// <summary>
		/// Referencia al hilo de transmisión
		/// </summary>
		private SendFileThread m_Thread;

		/// <summary>
		/// Realiza busqueda de dispositivos
		/// </summary>
		private void BuscarDispositivos()
		{
			lstDevices.Items.Clear();

			// Se buscan los dispositivos cercanos y se agregan a la lista
			m_btProvider.PowerMode = PowerMode.Connectable;
			m_Devices = m_btProvider.DiscoverDevices(10, 10);
			if ( m_Devices != null )
			{
				try 
				{
					for(int i=0; i < m_Devices.Count; i++)
					{
						lstDevices.Items.Add(m_Devices[i].Name);
					}
					if ( m_Devices.Count> 0 )
						lstDevices.SelectedIndex = 0;
				}
				catch 
				{
					lstDevices.Items.Clear();
				}
			}
			if ( lstDevices.Items.Count > 0 )
				lstDevices.Focus();
		}

		/// <summary>
		/// Ejecuta la busqueda de dispositivos
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnBuscar_Click(object sender, System.EventArgs e)
		{
			UIHandler.StartWait();
			BuscarDispositivos();
			UIHandler.EndWait();
		}

		private string m_FileName;

		/// <summary>
		/// Clase para retornar los resultados de la transmisión
		/// </summary>
		internal class Resultado
		{
			public bool Enviado = false;
			public string TargetDevice = "";
		}

		/// <summary>
		/// Crea y muestra la forma
		/// </summary>
		/// <param name="sFileName"></param>
		/// <returns></returns>
		public static Resultado Run(IBluetoothProvider btProvider, string sFileName)
		{
			Resultado res = new Resultado();
			FormSendFile frm = new FormSendFile();
			frm.m_FileName = sFileName;
			frm.m_btProvider = btProvider;
			if ( UIHandler.ShowDialog(frm) == DialogResult.OK)
			{
				res.Enviado = true;
				res.TargetDevice = frm.TargetDevice;
			}
			frm.Dispose();
			return res;
		}


		/// <summary>
		/// Si hay un dispositivo seleccionado inicia el hilo de transmisión del archivo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnEnviar_Click(object sender, System.EventArgs e)
		{
			int nIndex = lstDevices.SelectedIndex;
			if ( nIndex >= 0 )
			{
				UIHandler.StartWait();
				lblDestino.Text = m_Devices[nIndex].Name;
				lblArchivo2.Text = lblArchivo.Text;
				pnlInicio.Visible = false;
				pnlAvance.Visible = true;
				Invoker.Form = this;
				m_Thread = new SendFileThread(m_btProvider, m_Devices[nIndex], m_FileName);
				m_Thread.StateChanged += new EventHandler(SendStateChanged);
				m_Thread.ProgressChanged += new EventHandler(SendProgressChanged);
				m_Thread.SendComplete += new EventHandler(SendComplete);
				//m_Thread.Error += new EventHandler(ErrorReceived);
				m_Thread.Start();						
				btnCancelarEnvio.Focus();
				UIHandler.EndWait();
			}
			else
			{
				UIHandler.ShowError("Debe seleccionar un dispositivo de destino");
			}
		}

		/// <summary>
		/// Permite cancelar el envío del archivo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancelar_Click(object sender, System.EventArgs e)
		{
			UIHandler.StartWait();
			DialogResult = DialogResult.Cancel;
		}

		/// <summary>
		/// Actualiza el estado de la transmisión
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendStateChanged(object sender, System.EventArgs e)
		{			
			lblEstado.Text = (string)Invoker.Params[0];
		}

		/// <summary>
		/// Actualiza el progreso del envío del archivo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendProgressChanged(object sender, System.EventArgs e)
		{
			int avance = (int)Invoker.Params[0];
			if ( avance < 0 )
				avance = 0;
			if ( avance > 100)
				avance = 100;
			ctlProgress.Value = avance;
		}

		// Variables de retorno
		public string TargetDevice = "";

		/// <summary>
		/// Función llamada al terminar el envío del archivo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendComplete(object sender, System.EventArgs e)
		{
			BluetoothDevice Target = (BluetoothDevice) Invoker.Params[0];
			string sFileName = (string)Invoker.Params[1];
			bool bSuccess = (bool)Invoker.Params[2];
			if ( bSuccess )
			{
				TargetDevice = Target.Name;
				DialogResult = DialogResult.OK;
			}
			else
			{
				UIHandler.ShowError("Error enviando archivo");
				pnlInicio.Visible = true;
				pnlAvance.Visible = false;
			}
		}

		/// <summary>
		/// Termina el hilo de transmisión al cerrar la forma
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmSendFile_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( m_Thread != null )
				m_Thread.Stop();
			if ( m_btProvider != null )
				m_btProvider.Close();
			m_Thread = null;
		}		

		/// <summary>
		/// Permite cancelar el envío del archivo cuando esta ya esta en marcha
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancelarEnvio_Click(object sender, System.EventArgs e)
		{
			UIHandler.StartWait();
			m_Thread.Stop();			
			UIHandler.ShowError("Envio cancelado por usuario");
			pnlInicio.Visible = true;	
			pnlAvance.Visible = false;	
			lstDevices.Focus();
			UIHandler.EndWait();
		}		

		private void ErrorReceived(object sender, EventArgs e)
		{
			string sMessage = (string)Invoker.Params[0];
			UIHandler.ShowError(sMessage);
		}
	}


}
