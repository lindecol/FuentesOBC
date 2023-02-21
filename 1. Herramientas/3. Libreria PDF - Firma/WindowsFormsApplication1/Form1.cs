using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerarPDF.frmFirma firma = new GenerarPDF.frmFirma();
            firma.ShowDialog();

            if (crearPDF("test", firma.Nombre, firma.Identificacion))
            {
                MessageBox.Show("Firma y DocumentoPDF Guardado");
            }
            else
            {
                MessageBox.Show("ERROR");
            }
            //this.Close();
            firma.Dispose();
        }

        public bool crearPDF(string pdfpath, string nombre, string identificacion)
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
            datosImprimir.EncabezadoDocumento.FechaElaboracion = System.DateTime.Now;
            datosImprimir.EncabezadoDocumento.FechaVencimiento = System.DateTime.Now;
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
            producto1.ValorTotal = "5000";
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

            datosImprimir.EncabezadoDocumento.SubTotal = "4545";
            datosImprimir.EncabezadoDocumento.Total = "454545";
            datosImprimir.EncabezadoDocumento.IVA = "4545";

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
            return generar.GenerarPDFDocumento("c:\\firma.png", true);

            
            


            //}else if(listBox1.Items.Count > 1){

            //    MessageBox.Show("Solo puede ingresar una firma");

            //}else if (listBox1.Items.Count == 0){

            //    MessageBox.Show("Ingrese una Firma");
            //}
        }
    }
}
