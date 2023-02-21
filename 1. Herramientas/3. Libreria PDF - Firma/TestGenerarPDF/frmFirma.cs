using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace PruebaWM
{
    public partial class frmFirma : Form
    {
        Image imagen = null;
        public string img1 = "";
        
        public frmFirma(string tipo)
        {
            //btnCreatePDF_Click_1(null,null,ref tipo);
            img1 = tipo;
            InitializeComponent();
        }

        //private void btnCreatePDF_Click(object sender, EventArgs e)
        //{

        //}

       

        private void btnCreatePDF_Click_1(object sender, EventArgs e)
        {
            //MemoryStream ms = new MemoryStream(ctlFirma.GetSignatureEx());
            //Bitmap bitm = new Bitmap(ms);
            //imagen = bitm;
            //saveFileDialog1.Filter = "Jpg|*.jpg|PNG|*.png";
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
          
                Bitmap img = ctlFirma.ToBitmap();
                imagen = img;
                DateTime fechahoy = DateTime.Now;
                string dia = Convert.ToString(fechahoy.ToString("dd"));
                string mes = Convert.ToString(fechahoy.ToString("MM"));
                string year = Convert.ToString(fechahoy.ToString("yy"));
                string hora = Convert.ToString(fechahoy.ToString("hh"));
                string minutos = Convert.ToString(fechahoy.ToString("mm"));

                string path = img1 + "-" + dia + mes + year + hora + minutos;
                
                imagen.Save("\\FirmasDocumentos\\"+path+".png", System.Drawing.Imaging.ImageFormat.Png);
                //string pathimg = Path.GetFullPath(saveFileDialog1.FileName);
                
                //frm.listBox1.Items.Add(saveFileDialog1.FileName);
                /*

                using (MemoryStream ms = new MemoryStream(ctlFirma.GetSignatureEx()))
                {
                    Bitmap img = new Bitmap(ms);
                    img.Save("\firma.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);                
                }*/
                //MessageBox.Show("Firma y Documento Guardado");
                /*Form1 frm = new Form1();
                frm.crearPDF(path);*/
                
                
            //}
            
            //SaveFileDialog Buscar = new SaveFileDialog();
            //Buscar.Filter = "Jpg|*.jpg|PNG|*.png"; 

            //Buscar.ShowDialog();
            //Buscar.Dispose();

            //openFileDialog1.InitialDirectory = "\\My Documents\\My Pictures";
            //openFileDialog1.Filter = "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIF (*.tif)|*.tif|all (*.*)|*.*";
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //    listBox1.Items.Add(openFileDialog1.FileName);

            //string ruta = Path.GetFullPath(Buscar.FileName);
            //imagen.Save(Buscar.FileName);
            //this.Close();
            //cOMO PASAR BYTEARRAY[] A UNA IMAGEN
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ctlFirma.Clear();
        }

        private void ctlFirma_Click(object sender, EventArgs e)
        {

        }

        private void frmFirma_Load(object sender, EventArgs e)
        {

        }
    }
}