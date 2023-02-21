using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MovilidadCF.Windows.Forms;


namespace MovilidadCF.Bluetooth
{
	/// <summary>
	/// Permite seleccionar la impresora bluetooth entre una lista de impresoras
	/// previamente buscadas y envia el texto de impresión a la misma
	/// </summary>
	internal class FormPrint : System.Windows.Forms.Form
	{

		#region "Código generado por el Diseñador de Windows Forms"
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.ToolBarButton toolBarButton2;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuBuscarImpresora;
		private System.Windows.Forms.Button btnAceptar;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lstPrinters;
		private System.Windows.Forms.MenuItem menuQuitarImpresora;
        private MovilidadCF.Windows.Forms.UIHandler uiHandler1;
		private System.Windows.Forms.Button btnCancelar;

		public FormPrint()
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
		///
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrint));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuBuscarImpresora = new System.Windows.Forms.MenuItem();
            this.menuQuitarImpresora = new System.Windows.Forms.MenuItem();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstPrinters = new System.Windows.Forms.ListBox();
            this.uiHandler1 = new MovilidadCF.Windows.Forms.UIHandler();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuBuscarImpresora);
            this.menuItem1.MenuItems.Add(this.menuQuitarImpresora);
            this.menuItem1.Text = "Impresora";
            // 
            // menuBuscarImpresora
            // 
            this.menuBuscarImpresora.Text = "Nueva impresora...";
            this.menuBuscarImpresora.Click += new System.EventHandler(this.menuBuscarImpresora_Click);
            // 
            // menuQuitarImpresora
            // 
            this.menuQuitarImpresora.Text = "Eliminar Impresora...";
            this.menuQuitarImpresora.Click += new System.EventHandler(this.menuQuitarImpresora_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(88, 200);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(72, 20);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Text = "Imprimir";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(168, 200);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(64, 20);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cerrar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(32, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.Text = "Imprimir";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 16);
            this.label2.Text = "Seleccione Impresora";
            // 
            // lstPrinters
            // 
            this.lstPrinters.Location = new System.Drawing.Point(8, 64);
            this.lstPrinters.Name = "lstPrinters";
            this.lstPrinters.Size = new System.Drawing.Size(232, 128);
            this.lstPrinters.TabIndex = 3;
            // 
            // uiHandler1
            // 
            this.uiHandler1.HelpControl = null;
            this.uiHandler1.Parent = null;
            // 
            // FormPrint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(242, 270);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstPrinters);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.Menu = this.mainMenu1;
            this.Name = "FormPrint";
            this.Text = "Imprimir";
            this.Load += new System.EventHandler(this.frmPrint_Load);
            this.ResumeLayout(false);

		}
		#endregion

		// Variables privadas
		BluetoothDeviceList m_Printers;
		private string m_sPrintText;
		private IBluetoothProvider m_btProvider;

		/// <summary>
		/// Indicador de cuando la impresión fue realizada correctamente
		/// </summary>
		private bool m_bPrinted = false;		



		/// <summary>
		/// Crea y muestra la forma
		/// </summary>
		/// <param name="sPrintText"></param>
		/// <returns></returns>
 		public static DialogResult Run(IBluetoothProvider btProvider, string sPrintText)
		{	
			DialogResult result;
			FormPrint frm = new FormPrint();
			frm.m_sPrintText = sPrintText;
			frm.m_btProvider = btProvider;
			result = UIHandler.ShowDialog(frm);
			frm.Dispose();
			return result;			
		}
		

		/// <summary>
		/// Inicializa los controles y datos requeridos
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmPrint_Load(object sender, System.EventArgs e)
		{			 
			// Se Obtiene y muestran los dispositivos previamente registrados			
			LoadPrinters();
			this.uiHandler1.Parent = this;
			lstPrinters.Focus();
			if ( lstPrinters.Items.Count > 0 )
				lstPrinters.SelectedIndex = 0;
			UIHandler.EndWait();
		}


		/// <summary>
		/// Carga la lista de impresoras previamente configuradas
		/// </summary>
		private void LoadPrinters()
		{
			m_Printers = new BluetoothDeviceList();
			if ( File.Exists("\\Windows\\BTPrinters.CFG") )
			{
				StreamReader file = new StreamReader("\\Windows\\BTPrinters.CFG");
				string sPrinter = file.ReadLine();
				while ( sPrinter!= null && sPrinter != "" )
				{
					string[] sCampos = sPrinter.Split('|');
					m_Printers.Add(new BluetoothDevice(sCampos[0], sCampos[1]));
					sPrinter = file.ReadLine();
				}
				file.Close();
				foreach(BluetoothDevice printer  in m_Printers)
				{
					if(!lstPrinters.Items.Contains(printer.Name))
					{
						lstPrinters.Items.Add(printer.Name);
					}
				}				
			}
		}

		/// <summary>
		/// Guarda los datos de las impresoras configuradas en un archivo
		/// </summary>
		private void SavePrinters()
		{
			StreamWriter file = new StreamWriter("\\Windows\\BTPrinters.CFG", false);
			foreach(BluetoothDevice printer in m_Printers)
			{
				file.WriteLine(printer.Address + "|" + printer.Name);
			}
			file.Close();
		}
	
		/// <summary>
		/// Muestra la forma de busqueda de dispositivos bluetooth para permitir
		/// encontrar una impresora cercana.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuBuscarImpresora_Click(object sender, System.EventArgs e)
		{
			UIHandler.StartWait();
			BluetoothDevice device = FormFindDevice.Run(m_btProvider);
			if (device != null)
			{
				if(!lstPrinters.Items.Contains(device.Name))
				{
					m_Printers.Add(device);
					int nIndex = lstPrinters.Items.Add(device.Name);
					lstPrinters.SelectedIndex = nIndex;
				}
			}
			lstPrinters.Focus();
			UIHandler.EndWait();
		}

		/// <summary>
		/// Llama a la rutina de envia datos a la impresora seleccionada
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAceptar_Click(object sender, System.EventArgs e)
		{
			// si la impresión es satisfactoria, se activa la variable de control
			if ( Imprimir() )
				m_bPrinted = true;
			else
				UIHandler.ShowError("Por favor revise que la impresora este prendida y vuelva a intentar!");
		}

		/// <summary>
		/// Envia los datos a la impresora seleccionada
		/// </summary>
		/// <returns></returns>
		private bool Imprimir()
		{
			int nIndex = lstPrinters.SelectedIndex;
            bool result = false;
			UIHandler.StartWait();
			if ( nIndex >= 0 )
			{
				try 
				{					
					BluetoothDevice printer = m_Printers[nIndex];
					m_btProvider.PowerMode = PowerMode.Connectable;
					m_btProvider.Connect(printer);
					byte[] Data = Encoding.Default.GetBytes(m_sPrintText);
					m_btProvider.WriteData(Data, 0, Data.Length);
					
					// Hacer una pausa dependiendo de la cantidad de datos enviados
					System.Threading.Thread.Sleep(600 * (Data.Length / 1024));					

					result = true;
				}
				catch// (Exception ex)
				{
					// UIHandler.ShowError(ex.Message, ex.GetType().ToString());
				}
				finally
				{
					m_btProvider.Close();
				}
			}
			UIHandler.EndWait();
			return result;
		}

		/// <summary>
		/// Cancela la impresión y cierra la forma
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCancelar_Click(object sender, System.EventArgs e)
		{
			UIHandler.StartWait();
			SavePrinters();
			m_btProvider.PowerMode = PowerMode.Off;

			// El valor de cierre depende de si se realizo una impresión satisfactoria o no.
			if ( m_bPrinted )
				DialogResult = DialogResult.OK;
			else
				DialogResult = DialogResult.Cancel;

		}

		/// <summary>
		/// Permite quitar una impresora impresora de la lista de impresoras configuradas
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuQuitarImpresora_Click(object sender, System.EventArgs e)
		{
			int nIndex = lstPrinters.SelectedIndex;
			if ( nIndex >= 0 )
			{
				if ( UIHandler.Confirm("Esta seguro de quitar la impresora seleccionada?") )
				{
					lstPrinters.Items.RemoveAt(nIndex);
					m_Printers.RemoveAt(nIndex);
				}	
			}
		}		
	}
}
