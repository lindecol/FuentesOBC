#define TEST
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PruebaWM.Capa_Datos;
using System.Data.SqlClient;


namespace PruebaWM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "\\My Documents\\My Pictures";
            openFileDialog1.Filter = "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIF (*.tif)|*.tif|all (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                listBox1.Items.Add(openFileDialog1.FileName);

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private bool addText(string texto, iTextSharp.text.Document doc)        
        {

            Paragraph p1 = new Paragraph(new Chunk(texto, FontFactory.GetFont(FontFactory.HELVETICA, 18)));
            doc.Add(p1);


            Paragraph p13 = new Paragraph(new Chunk("_________________________________________", FontFactory.GetFont(FontFactory.HELVETICA, 18)));
            doc.Add(p13);


            Paragraph p12 = new Paragraph(new Chunk(texto, FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            doc.Add(p12);

            
            return true;
        }

        private bool addImage(string sFilename, iTextSharp.text.Document doc)
        {
            bool bReturn = false;
            iTextSharp.text.Image img;
            try
            {
                Paragraph p1 = new Paragraph(new Chunk(sFilename, FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                doc.Add(p1);

#if !TEST
                Bitmap myBitmap = new Bitmap(sFilename);
                if (sFilename.ToLower().EndsWith("jpg"))
                    img = iTextSharp.text.Image.GetInstance(myBitmap, System.Drawing.Imaging.ImageFormat.Jpeg);
                else if (sFilename.ToLower().EndsWith("gif"))
                    img = iTextSharp.text.Image.GetInstance(myBitmap, System.Drawing.Imaging.ImageFormat.Gif);
                else if (sFilename.ToLower().EndsWith("bmp"))
                    img = iTextSharp.text.Image.GetInstance(myBitmap, System.Drawing.Imaging.ImageFormat.Bmp);
                else if (sFilename.ToLower().EndsWith("png"))
                    img = iTextSharp.text.Image.GetInstance(myBitmap, System.Drawing.Imaging.ImageFormat.Png);
                else
                    throw new NotSupportedException("Unsupported image format");
                //is the image to wide or to high?
                float fWidth=doc.Right - doc.Left - doc.RightMargin - doc.LeftMargin;
                float fHeight=doc.Top - doc.Bottom - doc.TopMargin - doc.BottomMargin;
                if ((myBitmap.Width > fWidth) || (myBitmap.Height>fHeight))
                    img.ScaleToFit(fWidth, fHeight);

#else
                img = iTextSharp.text.Image.GetInstance(sFilename);
#endif
                doc.Add(img);
                doc.NewPage(); //used to create a new page for every image
                bReturn = true;
            }
            catch (iTextSharp.text.BadElementException bx)
            {
                System.Diagnostics.Debug.WriteLine("BadElementException in doc.add() for '" + sFilename + "': " + bx.Message);
            }
            catch (iTextSharp.text.DocumentException dx)
            {
                System.Diagnostics.Debug.WriteLine("DocumentException in doc.add() for '" + sFilename + "': " + dx.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception in doc.add() for '" + sFilename + "': " + ex.Message);
            }
            return bReturn;
        }
        //private void btnCreatePDF_Click(object sender, EventArgs e)
        //{
        //    string myPDFfile = "";
        //    if (listBox1.Items.Count > 0)
        //    {
        //        saveFileDialog1.FileName = "\\My Documents\\MyPDF.pdf";
        //        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
        //            myPDFfile = saveFileDialog1.FileName;
        //            if (!myPDFfile.ToLower().EndsWith(".pdf"))
        //                if(myPDFfile.EndsWith("."))
        //                    myPDFfile += "pdf";
        //                else
        //                    myPDFfile += ".pdf";
        //        }
                //else
                //    return;
                //textBox1.Text = "Saving to " + myPDFfile + "\r\n";
                //iTextSharp.text.Document doc = new Document();
                //PdfWriter writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(myPDFfile, System.IO.FileMode.Create));

                //doc.Open();
                //addText("Esto es una prueba", doc);
                

                
                //string currentImageName;
                ////Uri currentURI; //will not work locally                
                //for (int i = 0; i < listBox1.Items.Count; i++)
                //{
            //        currentImageName = listBox1.Items[i].ToString();
            //        //accessing local file using WebRequest does not work in CF!
            //        //currentURI = new Uri(currentImageName,UriKind.Relative);
            //        textBox1.Text += currentImageName;
            //        if (addImage(currentImageName, doc))
            //            textBox1.Text += " inserted OK\r\n";
            //        else
            //            textBox1.Text += " insertion failed\r\n";
            //    }

            //    doc.Close();


            //    textBox1.Text += "finished";
            //}
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            //ClsFirma Fir= new ClsFirma();
            //dataGridFir.DataSource = Fir.ListarPrax();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //frmFirma frm = new frmFirma();
            GenerarPDF.frmFirma firma = new GenerarPDF.frmFirma();
            firma.ShowDialog();

            //crearPDF("test", firma.Nombre, firma.Identificacion);
            //this.Close();
            //firma.Dispose();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_ParentChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 1)
            {

                            
                
                    string ruta = listBox1.Items[0].ToString();
                


                PDF test = new PDF(null, null, null, ruta);
                test.DibujarPDF();
                MessageBox.Show("Creado");
            }
            if (listBox1.Items.Count > 1)
            {
                MessageBox.Show("Ingrese solo una Firma");
            }
            else if (listBox1.Items.Count == 0) { MessageBox.Show("Ingrese una Firma"); }
        }

        private void dataGridFir_CurrentCellChanged(object sender, EventArgs e)
        {
            
            


        }

        private void btnCreatePDF_Click(object sender, EventArgs e)
        {
            GenerarPDF.Documento doc = new GenerarPDF.Documento();
            GenerarPDF.frmFirma frm = new GenerarPDF.frmFirma();
            frm.ShowDialog();
            frm.Dispose();            
        }

        public void crearPDF(string pdfpath, string nombre, string identificacion)
        {
            //if(listBox1.Items.Count == 1){
            string rutaimg = "\\FirmasDocumentos\\" + pdfpath + ".png";
            string rutapdf = "\\DocumentosPDF\\" + pdfpath + ".pdf";
            //string rutaimg = listBox1.Items[0].ToString();
            
            GenerarPDF.Documento datosImprimir = new GenerarPDF.Documento();
            //Datos Zona 1            
            datosImprimir.Empresa.Nombre = "OXIGENOS DE COLOMBIA LTDA";
            datosImprimir.Cliente.Direccion = "CALLE 35 N° 53-21";
            datosImprimir.Cliente.Ciudad = "MEDELLIN";
            datosImprimir.Cliente.Barrio = "MADELENA";
            datosImprimir.Cliente.Telefono = "6900777";
            datosImprimir.Empresa.Identificacion = "1111111111";
            datosImprimir.Empresa.LineaEmergencia = "018000-510-003";
            //Datos Zona 2
            datosImprimir.EncabezadoDocumento.Tipo = "REMISION - 913-4171";
            datosImprimir.EncabezadoDocumento.Numero = "018000-527-527";
            //datosImprimir.EncabezadoDocumento.Contacto = "7052000 - 4013000";
            datosImprimir.EncabezadoDocumento.FechaElaboracion = Convert.ToDateTime("2/3/19 16:25:00");
            datosImprimir.EncabezadoDocumento.FechaVencimiento = Convert.ToDateTime("10/23/19 18:00:00");
            datosImprimir.Cliente.CodigoCliente = "0021526";
            datosImprimir.EncabezadoDocumento.OrdenCompra = "123546";
            datosImprimir.EncabezadoDocumento.NuPedido = "0000116";

            //5 productos
            //Datos Zona 3
            GenerarPDF.DetalleDocumento producto1 = new GenerarPDF.DetalleDocumento();
            producto1.CodigoProducto = "012020220";
            producto1.DescripcionProducto = "NITROGENO LIQUIDO EN";
            producto1.Cantidad = "103";
            producto1.Unidad = "m3";
            producto1.ValorUnidad = "60";
            producto1.ValorTotal = "producto1.Cantidad * producto1.ValorUnidad";
            datosImprimir.DetalleDocumento.Add(producto1);

            GenerarPDF.DetalleDocumento producto2 = new GenerarPDF.DetalleDocumento();
            producto2.CodigoProducto = "123467789";
            producto2.DescripcionProducto = "OXIGENO";
            producto2.Cantidad = "562";
            producto2.Unidad = "d4";
            producto2.ValorUnidad = "";
            producto2.ValorTotal = "";
            datosImprimir.DetalleDocumento.Add(producto2);

            GenerarPDF.DetalleDocumento producto3 = new GenerarPDF.DetalleDocumento();
            producto3.CodigoProducto = "987654321";
            producto3.DescripcionProducto = "CO2";
            producto3.Cantidad = "264";
            producto3.Unidad = "a9";
            producto3.ValorUnidad = "50";
            producto3.ValorTotal = "";
            datosImprimir.DetalleDocumento.Add(producto3);

            //Datos Zona 3 Totales

            datosImprimir.EncabezadoDocumento.SubTotal = "";
            datosImprimir.EncabezadoDocumento.Total = producto1.ValorTotal + producto2.ValorTotal + producto3.ValorTotal;
            datosImprimir.EncabezadoDocumento.IVA = "";

            //Datos Zona 4
            datosImprimir.EncabezadoDocumento.Remisiones = "TERMOS CILINDROS";
            datosImprimir.EncabezadoDocumento.Observaciones = "Pres.Ini: 55, Pres.Fin: 60, Nivel.Ini: 6, Nivel.Fin: 109";
            datosImprimir.EncabezadoDocumento.Resolucion = "[RESOLUCION2]";
            //Datos Zona 5
            datosImprimir.Operador.Nombre = "LUIS ALEJANDRO LEMUS FORERO";
            datosImprimir.Operador.Ruta = "LQ_0008";
            //Datos Zona 6
            datosImprimir.Cliente.Nombre = "JOSE MAPE";
            datosImprimir.Cliente.Identificacion = "1234567891";

            datosImprimir.IdentificacionFirma = identificacion;
            datosImprimir.NombreFirma = nombre;

            datosImprimir.PrefijoDocumentoPDF = "PP";
            datosImprimir.TipoDocumentoPDF = "PDF";
            datosImprimir.CodigoClientePDF = "1";
            datosImprimir.ConsecutivoDocumentoPDF = "1";

            //Datos Paths
            datosImprimir.PathFirma = rutaimg;
            datosImprimir.Path = rutapdf;

            GenerarPDF.GeneradorDocumentos generar = new GenerarPDF.GeneradorDocumentos();
            generar.DatosImprimir = datosImprimir;
            generar.GenerarPDFDocumento("", true);
            
            MessageBox.Show("Firma y DocumentoPDF Guardado");
            this.Show();
            

            //}else if(listBox1.Items.Count > 1){

            //    MessageBox.Show("Solo puede ingresar una firma");

            //}else if (listBox1.Items.Count == 0){

            //    MessageBox.Show("Ingrese una Firma");
            //}
        }
    }
}