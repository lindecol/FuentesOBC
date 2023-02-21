using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GenerarPDF
{
    public partial class frmFirma : Form
    {
        public string PathFirma { get; set; }

        public bool FirmaCapturada { get; set; }

        public string Nombre 
        {
            get
            {
                return txtNombre.Text;
            }
        }

        public string Identificacion
        {
            get
            {
                return txtIdentificacion.Text;
            }
        }
        
        public frmFirma()
        {            
            InitializeComponent();
        }

        private void ctlFirma_Click(object sender, EventArgs e)
        {

        }        

        private void menuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (this.txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el nombre de la persona que firma", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                this.txtNombre.Focus();
                this.ctlFirma.BorderColor = System.Drawing.Color.Black;
                this.ctlFirma.Refresh();
                return;
            }

            if (this.txtIdentificacion.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la identificación de la persona que firma", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                this.txtIdentificacion.Focus();
                this.ctlFirma.BorderColor = System.Drawing.Color.Black;
                this.ctlFirma.Refresh();
                return;
            }

            if (!this.FirmaCapturada)
            {                
                MessageBox.Show("Debe capturar la firma del cliente", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                this.ctlFirma.BorderColor = System.Drawing.Color.Black;
                this.ctlFirma.Refresh();
                return;
            }

            Image imagen = null;
            try
            {
                Bitmap img = ctlFirma.ToBitmap();
                imagen = img;

                if (!Directory.Exists("\\FirmasDocumentos"))
                {
                    Directory.CreateDirectory("\\FirmasDocumentos");
                }
                this.PathFirma = "Firma" + DateTime.Now.ToString("ddMMyyhhmmss") + ".png";
                this.PathFirma = Path.Combine("\\FirmasDocumentos", this.PathFirma);

                if (!Directory.Exists("\\DocumentosPDF"))
                {
                    Directory.CreateDirectory("\\DocumentosPDF");
                }

                imagen.Save(this.PathFirma, System.Drawing.Imaging.ImageFormat.Png);

                
                this.DialogResult = DialogResult.OK;
            }
            catch
            {

            }
        }

      

        private void menuItem5_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.FirmaCapturada = false;
            this.ctlFirma.Click += new System.EventHandler(this.ctlFirma_Click_1);
            ctlFirma.Clear();
        }

        private void ctlFirma_Click_1(object sender, EventArgs e)
        {
            this.FirmaCapturada = true;
            this.ctlFirma.Click -= new System.EventHandler(this.ctlFirma_Click_1);
        }

        private void frmFirma_Load(object sender, EventArgs e)
        {
            this.FirmaCapturada = false;
            this.txtNombre.Focus();
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            string firmaZPL = GenerarPDF.ZPLConverterHelper.CrearFirma(@"\FirmasDocumentos\Firma210820034553.png", 31, 1300);



            MessageBox.Show(firmaZPL);
        }
    }    
}