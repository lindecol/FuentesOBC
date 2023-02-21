using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using OpenNETCF.Threading;
using System.Threading;
using MovilidadCF.Windows.Forms;

namespace MovilidadCF.Bluetooth
{

	/// <summary>
	/// Forma que realiza la recepción de archivos de una PDA y muestra
	/// el estado del avance de la transmisión.
	/// </summary>
	internal class FormReceiveFile : System.Windows.Forms.Form
	{

		#region Código generado por el Diseñador de Windows Forms

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnCancelar;
		private System.Windows.Forms.Label lblWorkDir;
		private System.Windows.Forms.ProgressBar ctlProgress;
		private System.Windows.Forms.Label lblEstado;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblArchivo;
		private System.Windows.Forms.Label lblTamaño;
		private System.Windows.Forms.Label lblOrigen;
		private System.Windows.Forms.Label label10;
        private MovilidadCF.Windows.Forms.UIHandler uiHandler1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
	
		private FormReceiveFile()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReceiveFile));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWorkDir = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ctlProgress = new System.Windows.Forms.ProgressBar();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.lblTamaño = new System.Windows.Forms.Label();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.uiHandler1 = new MovilidadCF.Windows.Forms.UIHandler();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(32, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.Text = "Recibir Archivo";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 20);
            this.label2.Text = "Destino archivos recibidos:";
            // 
            // lblWorkDir
            // 
            this.lblWorkDir.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblWorkDir.Location = new System.Drawing.Point(8, 48);
            this.lblWorkDir.Name = "lblWorkDir";
            this.lblWorkDir.Size = new System.Drawing.Size(224, 20);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.Text = "Tamaño:";
            // 
            // ctlProgress
            // 
            this.ctlProgress.Location = new System.Drawing.Point(8, 216);
            this.ctlProgress.Name = "ctlProgress";
            this.ctlProgress.Size = new System.Drawing.Size(224, 20);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(80, 240);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(72, 20);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(8, 200);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(224, 16);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(8, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(216, 20);
            this.label3.Text = "Datos transmisión:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 20);
            this.label5.Text = "Origen:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 20);
            this.label6.Text = "Archivo:";
            // 
            // lblArchivo
            // 
            this.lblArchivo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblArchivo.Location = new System.Drawing.Point(72, 120);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(160, 20);
            // 
            // lblTamaño
            // 
            this.lblTamaño.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblTamaño.Location = new System.Drawing.Point(72, 144);
            this.lblTamaño.Name = "lblTamaño";
            this.lblTamaño.Size = new System.Drawing.Size(160, 20);
            // 
            // lblOrigen
            // 
            this.lblOrigen.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblOrigen.Location = new System.Drawing.Point(72, 96);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(160, 20);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(8, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 20);
            this.label10.Text = "Estado Transmisión:";
            // 
            // uiHandler1
            // 
            this.uiHandler1.HelpControl = null;
            this.uiHandler1.Parent = null;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            // 
            // FormReceiveFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblOrigen);
            this.Controls.Add(this.lblTamaño);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.ctlProgress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblWorkDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "FormReceiveFile";
            this.Text = "Recibir Archivo";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmReceiveFile_Closing);
            this.Load += new System.EventHandler(this.frmReceiveFile_Load);
            this.ResumeLayout(false);

		}
		#endregion
        
		/// <summary>
		/// Referencia al hilo de del proceso de transferencia de archivos
		/// </summary>
		private ReceiveFileThread m_ReceiveThread;


		/// <summary>
		/// Variable que guarda la referencia a la instancia del proveedor 
		/// de bluetooth que debe ser utilizado
		/// </summary>
		IBluetoothProvider m_btProvider = null;


		/// <summary>
		/// Directorio de trabajo donde quedara el archivo
		/// </summary>
		private string m_sWorkDir;		

		// Clase con información del resultado de la transmisión
		internal class Resultado
		{			
			public bool Recibido = false;
			public string ReceivedFile = "";
			public string SourceDevice = "";			
		}

		/// <summary>
		/// Crea y muestra la forma
		/// </summary>
		/// <param name="sWorkDir"></param>
		/// <returns></returns>
		public static Resultado Run(IBluetoothProvider btProvider, string sWorkDir)
		{
			Resultado res = new Resultado();
			FormReceiveFile frm = new FormReceiveFile();
			frm.m_sWorkDir = sWorkDir;
			frm.m_btProvider = btProvider;
			if ( UIHandler.ShowDialog(frm) == DialogResult.OK )
			{
				res.Recibido = true;
				res.ReceivedFile = frm.ReceivedFile;
				res.SourceDevice = frm.SourceDevice;
			}
			frm.Dispose();
			return res;
		}
		
		/// <summary>
		/// Carga la forma y crea el hilo de transmisión que queda inicialmente
		/// esperando que una terminal se conecte
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmReceiveFile_Load(object sender, System.EventArgs e)
		{
			lblWorkDir.Text = m_sWorkDir;
			this.uiHandler1.Parent = this;

			// Se iniciliza la tarjeta en el modo correspondiente
			m_btProvider.PowerMode = PowerMode.Discoverable;
            
			// Se crea e inicia el hilo para recibir el archivo
			Invoker.Form = this;
			m_ReceiveThread = new ReceiveFileThread(m_btProvider, m_sWorkDir);
			m_ReceiveThread.StateChanged += new EventHandler(UpdateEstado);
			m_ReceiveThread.ProgressChanged += new EventHandler(UpdateProgress);
			m_ReceiveThread.Completed += new EventHandler(ReceiveComplete);
			m_ReceiveThread.ReceiveInit += new EventHandler(ReceiveInit);
			//m_ReceiveThread.Error += new EventHandler(ErrorReceived);
			m_ReceiveThread.Start();			

			btnCancelar.Focus();
			UIHandler.EndWait();
		}

		/// <summary>
		/// Terminal el hilo de transmisión si aún esta activo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmReceiveFile_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( m_ReceiveThread != null )
				m_ReceiveThread.Stop();
			if ( m_btProvider != null )
				m_btProvider.Close();
		}
		
		/// <summary>
		/// Permite cancelar la transmisión
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
		private void UpdateEstado(object sender, EventArgs e)
		{
			lblEstado.Text = (string)Invoker.Params[0];
		}

		/// <summary>
		/// Actualiza el progreso de la transmisión
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateProgress(object sender, EventArgs e)
		{
			int nAvance = (int) Invoker.Params[0];
			if ( nAvance < 0)
				nAvance = 0;
			if ( nAvance > 100)
				nAvance = 100;
			ctlProgress.Value = nAvance;
		}

        /// <summary>
        /// Actualiza los datos de archivo a recibir cuando se inicia la transmisión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ReceiveInit(object sender, EventArgs e)
		{
			lblOrigen.Text = (string) Invoker.Params[0];
			lblArchivo.Text = (string) Invoker.Params[1];
			lblTamaño.Text = Invoker.Params[2].ToString();
		}		

		// Variables privadas con información del archivo recibido
		private string ReceivedFile = "";
		private string SourceDevice = "";						
		
		/// <summary>
		/// Llamado al completar la transmisión del archivo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReceiveComplete(object sender, EventArgs e)
		{
			string sSourceDevice = (string)Invoker.Params[0];
			string sFileName = (string)Invoker.Params[1];
			bool bSuccess = (bool)Invoker.Params[2];
			if ( bSuccess )
			{
				ReceivedFile = sFileName;
				SourceDevice = sSourceDevice;
				UIHandler.StartWait();
				DialogResult = DialogResult.OK;
			}
			else
			{
				lblOrigen.Text = "";
				lblArchivo.Text = "";
				lblTamaño.Text = "";
			}
		}	

		private void ErrorReceived(object sender, EventArgs e)
		{
			string sMessage = (string)Invoker.Params[0];
			UIHandler.ShowError(sMessage);
		}
	}
}
