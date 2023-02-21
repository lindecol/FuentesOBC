using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;
using System.IO;

namespace PruebaWM
{
    public class PDF
    {
        private int anchoDocumento = 100;
        //Unicamente va a tener un registro y contiene la información de PRAX: Dirección, telefono
        private DataTable dtTitulo;
        public DataTable TituloFactura 
        {   
            set
            { 
                this.dtTitulo = value;
            }
            get
            {
                return this.dtTitulo;
            }
        }

        //Unicamente un registro, los datos del cliente, datos conductor y el documento
        private DataTable dtEncabezadoFactura;
        public DataTable EncabezadoFactura
        {
            set
            {
                this.dtEncabezadoFactura = value;
            }
            get
            {
                return this.dtEncabezadoFactura;
            }
        }

        //Todos los productos
        private DataTable dtDetalleFactura;
        public DataTable DetalleFactura
        {
            set
            {
                this.dtDetalleFactura = value;
            }
            get
            {
                return this.dtDetalleFactura;
            }
        }

        private string rutaFirma;
        public string RutaFirma
        {
            set
            {
                this.rutaFirma = value;
            }
            get
            {
                return this.rutaFirma;
            }
        }

        public PDF(DataTable dtTitulo, DataTable dtEncabezado, DataTable dtDetalle, string pathFirma)
        {
            this.TituloFactura = dtTitulo;
            this.EncabezadoFactura = dtEncabezado;
            this.DetalleFactura = dtDetalle;
            this.RutaFirma = pathFirma;
        }

        

        public bool DibujarPDF()
        {

            string rutpdf="";
            
            iTextSharp.text.Document documento = new iTextSharp.text.Document();
            rutaPDF(ref rutpdf);
            
            PdfWriter writer = PdfWriter.GetInstance(documento, new System.IO.FileStream(rutpdf, System.IO.FileMode.Create));
            documento.Open();         
            TablaTitulo(documento);
            AdicionarLinea(documento);          
            documento.Close();

            return true;
        }

        private bool rutaPDF(ref string rutPdf)
        {
            int e=0;
            for (int i = 1; i != e; i++)
            {

                rutPdf = @"\pdf\Pdfrecibo"+i+".pdf";
                if (File.Exists(rutPdf))
                {

                }
                else {

                    e = 1+i;
                }

            }
            return true;
        }


        private bool AdicionarImagen(string sFilename, iTextSharp.text.Document doc)
        {
            bool bReturn = false;
            iTextSharp.text.Image img;
            try
            {
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
                float fWidth = doc.Right - doc.Left - doc.RightMargin - doc.LeftMargin;
                float fHeight = doc.Top - doc.Bottom - doc.TopMargin - doc.BottomMargin;
                if ((myBitmap.Width > fWidth) || (myBitmap.Height > fHeight))
                    img.ScaleToFit(fWidth, fHeight);

#else
                img = iTextSharp.text.Image.GetInstance(sFilename);
#endif
                doc.Add(img);
                //doc.NewPage(); //used to create a new page for every image
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

        private bool AdicionarTextoSinCuadro(string texto, iTextSharp.text.Document doc)
        {
            Paragraph p1 = new Paragraph(new Chunk(texto, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)));
            doc.Add(p1);
            return true;
        }

        private bool TablaTitulo(iTextSharp.text.Document doc)
        {
            iTextSharp.text.Font Neg = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN,10,iTextSharp.text.Font.BOLD);
            iTextSharp.text.Table Tablatitul = new Table(2,1);
            iTextSharp.text.Cell Celda1 = new Cell(new Paragraph(new Phrase(new Chunk(
            @"OXIGENOS DE COLOMBIA LTDA "+"\n"+
            @"SEÑOR(ES): OXIGENOS DE COLOMBIA LTDA"+"\n"+
            @"DIRECCION/TELEFONO: CALLE 35 N° 53-21 MEDELLIN"+"\n"+
            @"6900777"+"\n"+
            @"BARRIO: MADELENA"+"\n"+
            @"NIT/CEDULA: 1111111111-1"+"\n"+
            @"LINEA EMERGENCIA 018000-510-003" + "\n" + "\n", Neg))));
            Celda1.BorderWidth = 0;
            Tablatitul.AddCell(Celda1,0,0);
            iTextSharp.text.Image prax = iTextSharp.text.Image.GetInstance("\\logoprax.jpg");
            prax.ScaleAbsolute(200f, 50f);
            iTextSharp.text.Cell Celda2 = new Cell(prax);
            Celda2.HorizontalAlignment = Element.ALIGN_CENTER;
            Celda2.VerticalAlignment = Element.ALIGN_CENTER;
            Celda2.BorderWidth = 0;
            Tablatitul.AddCell(Celda2,0,1);
            Tablatitul.Width = anchoDocumento;
            Tablatitul.BorderWidth = 0;
            doc.Add(Tablatitul);
            return true;
        }


        private bool AdicionarLinea(iTextSharp.text.Document doc)
        {
            string linea = "";
            iTextSharp.text.Font fNegrita = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fNormal = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL);

            iTextSharp.text.Table tabla1 = new Table(6, 6);
            
            Paragraph p1 = new Paragraph(new Chunk(linea, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
            iTextSharp.text.Cell celda = new Cell(new Paragraph(new Phrase(new Chunk(" LINEA NACIONAL 018000-527-527 y en BOGOTA PBX 7052000 - 4013000"+"\n"+"\n", fNegrita))));       
            celda.HorizontalAlignment = Element.ALIGN_MIDDLE;
            celda.Colspan = 6;
            tabla1.AddCell(celda,0,0);
            iTextSharp.text.Cell celda2 = new Cell(new Paragraph(new Phrase(new Chunk(" REMISION - 913-4171"+"\n"+"\n", fNegrita))));
            celda2.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda2.Colspan = 6;
            tabla1.AddCell(celda2,1,0);
            iTextSharp.text.Cell celda3 = new Cell(new Paragraph(new Phrase(new Chunk(" FECHA ELABORACION(aaaa-mm-dd)" + "\n" + "\n", fNegrita))));
            
            celda3.Colspan = 3;
            tabla1.AddCell(celda3, 2, 0);         
            iTextSharp.text.Cell celda4 = new Cell(new Phrase(new Chunk(" FECHA VENCIMIENTO(aaaa-mm-dd)",fNegrita)));
            celda4.Colspan = 3;
            tabla1.AddCell(celda4, 2, 3);
            //tabla2.Width = anchoDocumento;
            //doc.Add(tabla2);
            //iTextSharp.text.Table tabla3 = new Table(6, 1);
            iTextSharp.text.Cell celda5 = new Cell(new Phrase(new Chunk("2019",fNormal)));
            celda5.HorizontalAlignment = Element.ALIGN_CENTER;
            celda5.VerticalAlignment = Cell.ALIGN_TOP;
            celda5.Add(new Paragraph("\n"+"\n"));
            tabla1.AddCell(celda5, 3, 0);
            iTextSharp.text.Cell celda6 = new Cell(new Phrase(new Chunk("10",fNormal)));
            celda6.HorizontalAlignment = Element.ALIGN_CENTER;
            celda6.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda6.Add(new Paragraph("\n" + "\n"));
            tabla1.AddCell(celda6, 3, 1);
            iTextSharp.text.Cell celda7 = new Cell(new Phrase(new Chunk("24",fNormal)));
            celda7.HorizontalAlignment = Element.ALIGN_CENTER;
            celda7.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda7.Add(new Paragraph("\n" + "\n"));
            tabla1.AddCell(celda7, 3, 2);
            iTextSharp.text.Cell celda8 = new Cell(new Phrase(new Chunk("2019",fNormal)));
            celda8.HorizontalAlignment = Element.ALIGN_CENTER;
            celda8.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda8.Add(new Paragraph("\n" + "\n"));
            tabla1.AddCell(celda8, 3, 3);
            iTextSharp.text.Cell celda9= new Cell(new Phrase(new Chunk("10",fNormal)));
            celda9.HorizontalAlignment = Element.ALIGN_CENTER;
            celda9.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda9.Add(new Paragraph("\n" + "\n"));
            tabla1.AddCell(celda9, 3, 4);
            iTextSharp.text.Cell celda10 = new Cell(new Phrase(new Chunk("24",fNormal)));
            celda10.HorizontalAlignment = Element.ALIGN_CENTER;
            celda10.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda10.Add(new Paragraph("\n" + "\n"));
            tabla1.AddCell(celda10, 3, 5);
            iTextSharp.text.Cell celda11 = new Cell(new Phrase(new Chunk(" CODIGO CLIENTE",fNegrita)));
            celda11.Add(new Paragraph("\n" + "\n"));
            celda11.Colspan = 2;
            tabla1.AddCell(celda11, 4,0);
            iTextSharp.text.Cell celda12 = new Cell(new Phrase(new Chunk(" ORDEN COMPRA",fNegrita)));
            celda12.Add(new Paragraph("\n"));
            celda12.Colspan = 2;
            tabla1.AddCell(celda12,4,2);
            iTextSharp.text.Cell celda13 = new Cell(new Phrase(new Chunk(" N° PEDIDO",fNegrita)));
            celda13.Add(new Paragraph("\n"));
            celda13.Colspan = 2;
            tabla1.AddCell(celda13,4,4);
            iTextSharp.text.Cell celda14 = new Cell(new Phrase(new Chunk("0021526",fNormal)));
            celda14.Add(new Paragraph("\n" + "\n"));
            celda14.Colspan = 2;
            celda14.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda14.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla1.AddCell(celda14,5,0);
            iTextSharp.text.Cell celda15 = new Cell(new Phrase(new Chunk("123456",fNormal)));
            celda15.Add(new Paragraph("\n" + "\n"));
            celda15.Colspan = 2;
            celda15.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda15.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla1.AddCell(celda15,5,2);
            iTextSharp.text.Cell celda16 = new Cell(new Phrase(new Chunk("0000116",fNormal)));
            celda16.Add(new Paragraph("\n" + "\n"));
            celda16.Colspan = 2;
            celda16.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda16.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla1.AddCell(celda16,5,4);
            tabla1.Width = anchoDocumento;
      
            doc.Add(tabla1);

            iTextSharp.text.Table tabla2 = new Table(6,3);
            iTextSharp.text.Cell celda17 = new Cell(new Phrase(new Chunk(" CODIGO",fNegrita)));
            tabla2.AddCell(celda17,0,0);           
            Phrase phrase = new Phrase();
            phrase.Add( new Chunk("DESCRIPCION PRODUCTO", fNegrita) );

            iTextSharp.text.Cell celda18 = new Cell(new Paragraph(new Phrase(new Chunk(" DESCRIPCION"+"\n"+" PRODUCTO"+"\n", fNegrita))));
            tabla2.AddCell(celda18,0,1);
            iTextSharp.text.Cell celda19 = new Cell(new Phrase(new Chunk(" CANTIDAD",fNegrita)));
            tabla2.AddCell(celda19,0,2);
            iTextSharp.text.Cell celda20 = new Cell(new Phrase(new Chunk(" UNIDAD",fNegrita)));
            tabla2.AddCell(celda20,0,3);
            iTextSharp.text.Cell celda21 = new Cell(new Phrase(new Chunk(" Vlr.Unit",fNegrita)));
            tabla2.AddCell(celda21,0,4);
            iTextSharp.text.Cell celda22 = new Cell(new Phrase(new Chunk(" TOTAL",fNegrita)));
            tabla2.AddCell(celda22,0,5);
            iTextSharp.text.Cell celda23 = new Cell(new Phrase(new Chunk(" 0102101",fNormal)));
            tabla2.AddCell(celda23,1,0);
            iTextSharp.text.Cell celda24 = new Cell(new Paragraph(new Phrase(new Chunk(" NITROGENO" + "\n" + " LIQUIDO EN" + "\n" + "\n", fNormal))));
            tabla2.AddCell(celda24,1,1);
            iTextSharp.text.Cell celda25 = new Cell(new Phrase(new Chunk(" 6.638",fNormal)));
            tabla2.AddCell(celda25,1,2);
            iTextSharp.text.Cell celda26 = new Cell(new Phrase(new Chunk(" m3",fNormal)));
            tabla2.AddCell(celda26,1,3);
            iTextSharp.text.Cell celda27 = new Cell(new Phrase(new Chunk(" 0",fNormal)));
            tabla2.AddCell(celda27,1,4);
            iTextSharp.text.Cell celda28 = new Cell(new Phrase(new Chunk(" 0",fNormal)));
            tabla2.AddCell(celda28,1,5);            
            iTextSharp.text.Cell celda29 = new Cell(new Phrase(new Chunk(" SUBTOTAL: 0.00",fNegrita)));
            celda29.Colspan = 2;
            
            tabla2.AddCell(celda29, 2, 0);
            iTextSharp.text.Cell celda30 = new Cell(new Phrase(new Chunk(" IVA: 0.00",fNegrita)));
            celda30.Add(new Paragraph("\n"+"\n"));
            celda30.Colspan = 2;           
            tabla2.AddCell(celda30, 2, 2);
            iTextSharp.text.Cell celda31 = new Cell(new Phrase(new Chunk("TOTAL: 0.00",fNegrita)));
            celda31.Add(new Paragraph("\n" + "\n"));
            celda31.Colspan = 2;          
            tabla2.AddCell(celda31, 2, 4);            
            tabla2.Width = anchoDocumento;

            doc.Add(tabla2);

            iTextSharp.text.Table tabla3 = new Table(1,3);
            iTextSharp.text.Cell celda32 = new Cell(new Phrase(new Chunk("REMISIONES TERMOS CILINDROS - OBSERVACIONES",fNegrita)));
            //celda32.Colspan = 6;
            celda32.Add(new Paragraph("\n" + "\n"));
            celda32.HorizontalAlignment = Element.ALIGN_CENTER;         
            tabla3.AddCell(celda32,0,0);
            iTextSharp.text.Cell celda33 = new Cell(new Phrase(new Chunk(" Pres.Ini: 55,  Pres.Fin: 60,  Nivel Ini: 6,  Nivel Fin: 109",fNormal)));
            celda33.Add(new Paragraph("\n" + "\n" + "\n"));
            //celda33.Colspan = 6;
            tabla3.AddCell(celda33,1,0);
            iTextSharp.text.Cell celda34= new Cell(new Phrase(new Chunk(" [RESOLUCION2]",fNegrita)));
            celda34.Add(new Paragraph("\n" + "\n"));
            tabla3.AddCell(celda34,2,0);
            tabla3.Width= anchoDocumento;

            doc.Add(tabla3);

            iTextSharp.text.Table tabla4 = new Table(2,6);
            iTextSharp.text.Cell celda35 = new Cell(new Paragraph(new Phrase(new Chunk("\n" + " OPERADOR: LUIS ALEJANDRO LEMUS FORERO" + "\n" + " RUTA: LQ_0008" + "\n" + " HORA: 18:29" + "\n" + "\n", fNegrita))));
            celda35.Colspan = 2;
            tabla4.AddCell(celda35,0,0); 
            //celda35.Rowspan = 3;
            iTextSharp.text.Cell celda36 = new Cell(new Paragraph(new Phrase(new Chunk(" ACEPTADO POR:" + "\n" + " FIRMA CLIENTE: "+ "\n" + " NOMBRE: JOSE MAPE" + "\n" + " IDENTIFICACIÓN: 1234567891" + "\n" + "\n", fNegrita))));
           
           // celda36.Rowspan = 3;
            tabla4.AddCell(celda36,3,0);
            
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(RutaFirma);
            img.ScaleAbsolute(250f, 40f);
            //img.ScalePercent(3.3f);
            img.SetAbsolutePosition(20f,40f);
            //img.ScaleToFit(30, 10);
            
            //img.Width = 40;
            
            //img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            //iTextSharp.text.Cell celda38 = new Cell(new Paragraph(new Phrase(new Chunk("FIRMA CLIENTE",fNegrita))));
            //tabla4.AddCell(celda38,2,1);
            

            iTextSharp.text.Cell celda37= new Cell(img);
            celda37.VerticalAlignment=Element.ALIGN_MIDDLE;
            celda37.HorizontalAlignment = Element.ALIGN_CENTER;
            //celda37.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            

            tabla4.AddCell(celda37,3,1);
            tabla4.Width= anchoDocumento;

            doc.Add(tabla4);      
      
            doc.Add(p1);            
            return true;
        }

        private bool AdicionarTextoCuadradoUnico(string texto, iTextSharp.text.Document doc)
        {
            string linea = "|";
            int anchoTexto = texto.Length;

            if (anchoTexto > this.anchoDocumento)
            {
                texto = texto.Substring(0,this.anchoDocumento-1);
                linea = linea + texto + "|";
                Paragraph p1 = new Paragraph(new Chunk(linea, FontFactory.GetFont(FontFactory.HELVETICA, 18)));
                doc.Add(p1);
                return true;
            }
            else
            {
                int numeroEspacios = this.anchoDocumento - (anchoTexto + 2);
                string espacios = "";
                for (int j=0; j<numeroEspacios; j++)
                {
                    espacios = espacios + " ";
                }

                linea = linea + texto + espacios + "|";
                Paragraph p1 = new Paragraph(new Chunk(linea, FontFactory.GetFont(FontFactory.HELVETICA, 18)));
                doc.Add(p1);
                return true;
            }            
        }
    }
}
