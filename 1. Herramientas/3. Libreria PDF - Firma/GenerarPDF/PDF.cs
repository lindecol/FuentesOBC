using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GenerarPDF
{

    /// <summary>
    /// Esta clase se encarga de generar el PDF, y recibe los datos
    /// </summary>
    public class PDF
    {
        /// <summary>
        /// Todos los datos va a estar en esta propiedad
        /// </summary>
        private Documento Documento { get; set; }
        
        private int anchoDocumento = 70;
        iTextSharp.text.Font fGrande = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font fNegrita = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 7, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font fNormal = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL);
        iTextSharp.text.Font fpequena = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 5, iTextSharp.text.Font.NORMAL);
        iTextSharp.text.Font fCodigoBarras = new iTextSharp.text.Font(iTextSharp.text.Font.ZAPFDINGBATS, 10, iTextSharp.text.Font.NORMAL);
        double ancho = 0.5;
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

        public PDF(Documento datosImprimir)//Documento datosImprimir
        {
            this.Documento = datosImprimir;         
        }

        public bool DibujarPDF()
        {
            string pru = Documento.Empresa.Nombre;
            string rutpdf = Documento.Path;

            iTextSharp.text.Document documento = new iTextSharp.text.Document();

            bool resultado = false;

            if (File.Exists(rutpdf))
            {
                File.Delete(rutpdf);
            }

            PdfWriter writer = PdfWriter.GetInstance(documento, new System.IO.FileStream(rutpdf, System.IO.FileMode.Create));
            documento.Open();

            if (ImprimirZona1(documento))
            {
                if (ImprimirZona2(documento))
                {
                    if (ImprimirZona3(documento))
                    {
                        if (ImprimirZona4(documento))
                        {
                            if (ImprimirZona5(documento))
                            {
                                if (ImprimirZona6(documento))
                                {
                                    resultado = true;
                                }
                            }
                        }
                    }
                }
            }

            Paragraph par = new Paragraph(new Phrase(new Chunk(" "+Documento.EncabezadoDocumento.Copia, fNegrita)));
            par.Alignment = Element.ALIGN_BOTTOM;

            documento.Add(par);


            documento.Close();
            return resultado;
        }

        public bool DibujarPDFGases()
        {
            string pru = Documento.Empresa.Nombre;
            string rutpdf = Documento.Path;

            iTextSharp.text.Document documento = new iTextSharp.text.Document();

            bool resultado = false;

            if (File.Exists(rutpdf))
            {
                File.Delete(rutpdf);
            }

            PdfWriter writer = PdfWriter.GetInstance(documento, new System.IO.FileStream(rutpdf, System.IO.FileMode.Create));
            documento.Open();

            if (ImprimirZona1Gases(documento))
            {
                if (ImprimirZona2(documento))
                {
                    if (ImprimirZona3(documento))
                    {
                        if (ImprimirZona4(documento))
                        {
                            if (ImprimirZona5(documento))
                            {
                                if (ImprimirZona6(documento))
                                {
                                    resultado = true;
                                }
                            }
                        }
                    }
                }
            }

            Paragraph par = new Paragraph(new Phrase(new Chunk(" " + Documento.EncabezadoDocumento.Copia, fNegrita)));
            par.Alignment = Element.ALIGN_BOTTOM;

            documento.Add(par);


            documento.Close();
            return resultado;
        }
     
        private bool ImprimirZona1(iTextSharp.text.Document doc1)
        {
            try
            {                
                iTextSharp.text.Table Tablatitul = new Table(2, 3);
                iTextSharp.text.Cell Celda1 = new Cell(new Paragraph(new Phrase(new Chunk(
                Documento.Empresa.Nombre + "\n" +
                @"SEÑOR(ES): " + Documento.Cliente.Nombre + "\n" +
                @"DIRECCION/TELEFONO: " + Documento.Cliente.Direccion + " " + Documento.Cliente.Ciudad
                + "\n", fNegrita))));
                Celda1.BorderWidth = 0;
                Tablatitul.AddCell(Celda1, 0, 0);
                if (File.Exists("\\logoprax.jpg"))
                {
                    iTextSharp.text.Image prax = iTextSharp.text.Image.GetInstance("\\logoprax.jpg");
                    prax.ScaleAbsolute(200f, 40f);
                    iTextSharp.text.Cell Celda2 = new Cell(prax);
                    Celda2.HorizontalAlignment = Element.ALIGN_CENTER;
                    Celda2.VerticalAlignment = Element.ALIGN_CENTER;
                    Celda2.BorderWidth = 0;
                    Tablatitul.AddCell(Celda2, 0, 1);
                }
                iTextSharp.text.Cell Celda5 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.Cliente.Telefono, fNegrita))));
                Celda5.BorderWidth = 0;
                Tablatitul.AddCell(Celda5,1,0);
                iTextSharp.text.Cell Celda6 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.Empresa.Identificacion,fNegrita))));
                Celda6.BorderWidth = 0;
                Tablatitul.AddCell(Celda6,1,1);
                iTextSharp.text.Cell Celda3 = new Cell(new Paragraph(new Phrase(new Chunk(@"BARRIO: " + Documento.Cliente.Barrio + "\n" +
                @"NIT/CEDULA: " + Documento.Cliente.Identificacion + "\n" +
                Documento.Empresa.LineaEmergencia + "\n" + "\n", fNegrita))));
                Celda3.BorderWidth = 0;
                Tablatitul.AddCell(Celda3,2,0);
                iTextSharp.text.Cell Celda4 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.EncabezadoDocumento.L1 + " " + Documento.EncabezadoDocumento.L2 + " " + Documento.EncabezadoDocumento.L3 + "\n" + Documento.EncabezadoDocumento.L4 + "\n" + Documento.EncabezadoDocumento.L5 + "\n" + Documento.EncabezadoDocumento.L6, fNormal))));
                Celda4.BorderWidth = 0;
                Celda4.HorizontalAlignment = Element.ALIGN_CENTER;
                Celda4.VerticalAlignment = Element.ALIGN_MIDDLE;
                Tablatitul.AddCell(Celda4,2,1);

                Tablatitul.Width = anchoDocumento;
                Tablatitul.BorderWidth = 0;
                doc1.Add(Tablatitul);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ImprimirZona1Gases(iTextSharp.text.Document doc1)
        {
            try
            {
                iTextSharp.text.Table tabla1 = new Table(2, 4);

                iTextSharp.text.Cell celda1 = new Cell(new Paragraph(new Phrase(new Chunk(
                @"xyzxyzzyzxyxyzxyzxyxyzxyzxyyzxyxyzxyzxyxyzxyzxyyz"
                + "\n", fCodigoBarras))));
                celda1.BorderWidth = 0;
                tabla1.AddCell(celda1, 0, 0);

                iTextSharp.text.Cell celda2 = new Cell(new Paragraph(new Phrase(new Chunk(                
                @"SEÑOR(ES): " + Documento.Cliente.Nombre + "\n" +
                @"DIRECCION/TELEFONO: " + Documento.Cliente.Direccion + " " + Documento.Cliente.Telefono + "\n" +
                @"ENTIDAD: " + Documento.Cliente.Entidad + "\n" +
                @"SUB-DIVISION: " + Documento.Cliente.Subdivision  
                + "\n", fNegrita))));
                celda2.BorderWidth = 0;
                tabla1.AddCell(celda2, 1, 0);

                

                if (File.Exists("\\logoprax.jpg"))
                {
                    iTextSharp.text.Image prax = iTextSharp.text.Image.GetInstance("\\logoprax.jpg");
                    prax.ScaleAbsolute(200f, 40f);
                    iTextSharp.text.Cell celda3 = new Cell(prax);
                    celda3.HorizontalAlignment = Element.ALIGN_CENTER;
                    celda3.VerticalAlignment = Element.ALIGN_CENTER;
                    celda3.BorderWidth = 0;
                    tabla1.AddCell(celda3, 1, 1);
                }
                
                iTextSharp.text.Cell celda4 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.Empresa.Nombre + "\n" + Documento.Empresa.Identificacion, fpequena))));
                celda4.BorderWidth = 0;
                tabla1.AddCell(celda4, 2, 1);

                iTextSharp.text.Cell celda5 = new Cell(new Paragraph(new Phrase(new Chunk(@"BARRIO: " + Documento.Cliente.Barrio + "\n" +
                @"NIT/CEDULA: " + Documento.Cliente.Identificacion + "\n" +
                @"TIPO PAGO: " + Documento.Cliente.TipoPago + "\n" + "\n", fNegrita))));
                celda5.BorderWidth = 0;
                tabla1.AddCell(celda5, 3, 0);

                iTextSharp.text.Cell celda6 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.EncabezadoDocumento.L1 + " " + Documento.EncabezadoDocumento.L2 + " " + Documento.EncabezadoDocumento.L3 + "\n" + Documento.EncabezadoDocumento.L4 + "\n" + Documento.EncabezadoDocumento.L5 + "\n" + Documento.EncabezadoDocumento.L6, fNormal))));
                celda6.BorderWidth = 0;
                celda6.HorizontalAlignment = Element.ALIGN_CENTER;
                celda6.VerticalAlignment = Element.ALIGN_MIDDLE;
                tabla1.AddCell(celda6, 3, 1);

                tabla1.Width = anchoDocumento;
                tabla1.BorderWidth = 0;
                doc1.Add(tabla1);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ImprimirZona2(iTextSharp.text.Document doc2)
        {
            try
            {
                iTextSharp.text.Table tabla1 = new Table(1);
                iTextSharp.text.Cell celda1 = new Cell(new Paragraph(new Phrase(new Chunk(" " + Documento.EncabezadoDocumento.Numero + "\n" + "\n", fNegrita))));
                celda1.VerticalAlignment = Element.ALIGN_MIDDLE;                
                tabla1.AddCell(celda1);
                iTextSharp.text.Cell celda2 = new Cell(new Paragraph(new Phrase(new Chunk(" " + Documento.EncabezadoDocumento.Tipo + "\n" + "\n", fGrande))));
                celda2.VerticalAlignment = Element.ALIGN_MIDDLE;                
                tabla1.AddCell(celda2);
                tabla1.Width = anchoDocumento;
                tabla1.BorderWidth = (float)ancho;
                doc2.Add(tabla1);

                iTextSharp.text.Table tabla2 = new Table(6, 2);
                int[] widths = new int[] { 13, 13, 14, 20, 20, 20 };
                tabla2.SetWidths(widths);
                tabla2.Width = anchoDocumento;
                iTextSharp.text.Cell celda3 = new Cell(new Paragraph(new Phrase(new Chunk(" FECHA ELABORACION(aaaa-mm-dd)" + "\n" + "\n", fNegrita))));
                celda3.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda3.Colspan = 3;
                tabla2.AddCell(celda3, 0, 0);
                iTextSharp.text.Cell celda4 = new Cell(new Paragraph(new Phrase(new Chunk(" FECHA VENCIMIENTO(aaaa-mm-dd)" + "\n" + "\n", fNegrita))));
                celda4.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda4.Colspan = 3;
                tabla2.AddCell(celda4, 0, 3);
                iTextSharp.text.Cell celda5 = new Cell(new Paragraph(new Phrase(new Chunk(Convert.ToDateTime(Documento.EncabezadoDocumento.FechaElaboracion).ToString("yyyy") + "\n" + "\n", fNormal))));
                celda5.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda5.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla2.AddCell(celda5, 1, 0);
                iTextSharp.text.Cell celda6 = new Cell(new Paragraph(new Phrase(new Chunk(Convert.ToDateTime(Documento.EncabezadoDocumento.FechaElaboracion).ToString("MM") + "\n" + "\n", fNormal))));
                celda6.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda6.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla2.AddCell(celda6, 1, 1);
                iTextSharp.text.Cell celda7 = new Cell(new Paragraph(new Phrase(new Chunk(Convert.ToDateTime(Documento.EncabezadoDocumento.FechaElaboracion).ToString("dd") + "\n" + "\n", fNormal))));
                celda7.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda7.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla2.AddCell(celda7, 1, 2);
                iTextSharp.text.Cell celda8 = new Cell(new Paragraph(new Phrase(new Chunk(Convert.ToDateTime(Documento.EncabezadoDocumento.FechaVencimiento).ToString("yyyy") + "\n" + "\n", fNormal))));
                celda8.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda8.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla2.AddCell(celda8, 1, 3);
                iTextSharp.text.Cell celda9 = new Cell(new Paragraph(new Phrase(new Chunk(Convert.ToDateTime(Documento.EncabezadoDocumento.FechaVencimiento).ToString("MM") + "\n" + "\n", fNormal))));
                celda9.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda9.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla2.AddCell(celda9, 1, 4);
                iTextSharp.text.Cell celda10 = new Cell(new Paragraph(new Phrase(new Chunk(Convert.ToDateTime(Documento.EncabezadoDocumento.FechaVencimiento).ToString("dd") + "\n" + "\n", fNormal))));
                celda10.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda10.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla2.AddCell(celda10, 1, 5);
                tabla2.BorderWidth = (float)ancho;
                doc2.Add(tabla2);

                iTextSharp.text.Table tabla3 = new Table(3, 2);
                int[] widths2 = new int[] { 33, 34, 33 };
                tabla3.SetWidths(widths2);
                tabla3.Width = anchoDocumento;
                iTextSharp.text.Cell celda11 = new Cell(new Paragraph(new Phrase(new Chunk(" CODIGO CLIENTE" + "\n" + "\n", fNegrita))));
                celda11.VerticalAlignment = Element.ALIGN_MIDDLE;                
                tabla3.AddCell(celda11, 0, 0);
                iTextSharp.text.Cell celda12 = new Cell(new Paragraph(new Phrase(new Chunk(" ORDEN COMPRA" + "\n" + "\n", fNegrita))));
                celda12.VerticalAlignment = Element.ALIGN_MIDDLE;
                tabla3.AddCell(celda12, 0, 1);
                iTextSharp.text.Cell celda13 = new Cell(new Paragraph(new Phrase(new Chunk(" N° PEDIDO" + "\n" + "\n", fNegrita))));
                celda13.VerticalAlignment = Element.ALIGN_MIDDLE;                
                tabla3.AddCell(celda13, 0, 2);
                iTextSharp.text.Cell celda14 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.Cliente.CodigoCliente + "\n" + "\n", fNormal))));
                celda14.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda14.HorizontalAlignment = Element.ALIGN_CENTER;                
                tabla3.AddCell(celda14, 1, 0);
                iTextSharp.text.Cell celda15 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.EncabezadoDocumento.OrdenCompra + "\n" + "\n", fNormal))));
                celda15.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda15.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda15, 1, 1);
                iTextSharp.text.Cell celda16 = new Cell(new Paragraph(new Phrase(new Chunk(Documento.EncabezadoDocumento.NuPedido + "\n" + "\n", fNormal))));
                celda16.VerticalAlignment = Element.ALIGN_MIDDLE;
                celda16.HorizontalAlignment = Element.ALIGN_CENTER;                
                tabla3.AddCell(celda16, 1, 2);
                tabla3.Width = anchoDocumento;
                tabla3.BorderWidth = (float)ancho;
                doc2.Add(tabla3);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ImprimirZona3(iTextSharp.text.Document doc3)
        {
            try
            {
                int[] widths = new int[] { 12, 40, 12, 10, 10, 16 };
                iTextSharp.text.Table tabla3 = new Table(6);
                tabla3.SetWidths(widths);
                iTextSharp.text.Cell Celda1 = new Cell(new Phrase(new Chunk(" CODIGO", fNegrita)));
                tabla3.AddCell(Celda1, 0, 0);
                iTextSharp.text.Cell Celda2 = new Cell(new Phrase(new Chunk(" DESCRIPCION PRODUCTO" + "\n" + "\n", fNegrita)));
                tabla3.AddCell(Celda2, 0, 1);
                iTextSharp.text.Cell Celda3 = new Cell(new Phrase(new Chunk(" CANTIDAD" , fNegrita)));
                tabla3.AddCell(Celda3, 0, 2);
                iTextSharp.text.Cell Celda4 = new Cell(new Phrase(new Chunk(" UNIDAD" , fNegrita)));
                tabla3.AddCell(Celda4, 0, 3);
                iTextSharp.text.Cell Celda5 = new Cell(new Phrase(new Chunk(" Vlr.Unit" , fNegrita)));
                tabla3.AddCell(Celda5, 0, 4);
                iTextSharp.text.Cell Celda6 = new Cell(new Phrase(new Chunk(" TOTAL", fNegrita)));
                tabla3.AddCell(Celda6, 0, 5);
                tabla3.Width = anchoDocumento;
                tabla3.BorderWidth = (float)ancho;
                doc3.Add(tabla3);                
                
                Documento.DetalleDocumento.Add(new DetalleDocumento() { Cantidad = "", DescripcionProducto = "", ValorTotal = "", ValorUnidad = "", Unidad = "", CodigoProducto = "" });
                foreach (DetalleDocumento detalle in this.Documento.DetalleDocumento)
                {
                    iTextSharp.text.Table tabla4 = new Table(6);
                    tabla4.SetWidths(widths);
                    tabla4.BorderWidth = (float)ancho;
                    tabla4.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    tabla4.BorderWidthBottom = 0;

                    tabla4.BorderColorTop = iTextSharp.text.Color.WHITE;
                    tabla4.BorderWidthTop = 0;
                                     
                    iTextSharp.text.Cell celda1 = new Cell(new Phrase(new Chunk(" " + detalle.CodigoProducto + "\n" , fNormal)));
                    celda1.HorizontalAlignment = Element.ALIGN_LEFT;
                    celda1.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda1.BorderWidthBottom = 0;
                    celda1.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda1.BorderWidthTop = 0;
                    celda1.BorderColorRight = iTextSharp.text.Color.BLACK;
                    celda1.BorderWidthRight = (float)ancho;
                    celda1.Width = "20%";

                    tabla4.AddCell(celda1, 0, 0);
                    iTextSharp.text.Cell celda2 = new Cell(new Phrase(new Chunk(" " + detalle.DescripcionProducto + "\n", fNormal)));
                    celda2.HorizontalAlignment = Element.ALIGN_LEFT;
                    celda2.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda2.BorderWidthBottom = 0;
                    celda2.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda2.BorderWidthTop = 0;
                    celda2.BorderColorRight = iTextSharp.text.Color.BLACK;
                    celda2.BorderWidthRight = (float)ancho;

                    tabla4.AddCell(celda2, 0, 1);
                    iTextSharp.text.Cell celda3 = new Cell(new Phrase(new Chunk(detalle.Cantidad + "\n" , fNormal)));
                    celda3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    celda3.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda3.BorderWidthBottom = 0;
                    celda3.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda3.BorderWidthTop = 0;
                    celda3.BorderColorRight = iTextSharp.text.Color.BLACK;
                    celda3.BorderWidthRight = (float)ancho;

                    tabla4.AddCell(celda3, 0, 2);
                    iTextSharp.text.Cell celda4 = new Cell(new Phrase(new Chunk(detalle.Unidad + "\n" , fNormal)));
                    celda4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    celda4.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda4.BorderWidthBottom = 0;
                    celda4.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda4.BorderWidthTop = 0;
                    celda4.BorderColorRight = iTextSharp.text.Color.BLACK;
                    celda4.BorderWidthRight = (float)ancho;

                    tabla4.AddCell(celda4, 0, 3);
                    iTextSharp.text.Cell celda5 = new Cell(new Phrase(new Chunk(detalle.ValorUnidad + "\n" , fNormal)));
                    celda5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    celda5.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda5.BorderWidthBottom = 0;
                    celda5.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda5.BorderWidthTop = 0;
                    celda5.BorderColorRight = iTextSharp.text.Color.BLACK;
                    celda5.BorderWidthRight = (float)ancho;

                    tabla4.AddCell(celda5, 0, 4);
                    iTextSharp.text.Cell celda6 = new Cell(new Phrase(new Chunk(detalle.ValorTotal + "\n" , fNormal)));
                    celda6.HorizontalAlignment = Element.ALIGN_RIGHT;
                    celda6.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda6.BorderWidthBottom = 0;
                    celda6.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda6.BorderWidthTop = 0;
                    celda6.BorderColorRight = iTextSharp.text.Color.BLACK;
                    celda6.BorderWidthRight = (float)ancho;

                    tabla4.AddCell(celda6, 0, 5);
                    tabla4.Width = anchoDocumento;

                    doc3.Add(tabla4);
                }

                iTextSharp.text.Table tabla5 = new Table(6);
                iTextSharp.text.Cell celd1 = new Cell(new Phrase(new Chunk(" SUBTOTAL: " + Documento.EncabezadoDocumento.SubTotal + "\n" + "\n", fGrande)));
                celd1.Colspan = 2;
                tabla5.AddCell(celd1, 0, 0);
                iTextSharp.text.Cell celd2 = new Cell(new Phrase(new Chunk(" IVA: " + Documento.EncabezadoDocumento.IVA + "\n" + "\n", fGrande)));
                celd2.Colspan = 2;
                tabla5.AddCell(celd2, 0, 2);
                iTextSharp.text.Cell celd3 = new Cell(new Phrase(new Chunk(" TOTAL: " + Documento.EncabezadoDocumento.Total + "\n" + "\n", fGrande)));
                celd3.Colspan = 2;
                tabla5.AddCell(celd3, 0, 4);
                tabla5.Width = anchoDocumento;
                tabla5.BorderWidth = (float)ancho;
                doc3.Add(tabla5);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ImprimirZona4(iTextSharp.text.Document doc4)
        {
            try
            {
                iTextSharp.text.Table tabla1 = new Table(1);
                tabla1.BorderColorTop = iTextSharp.text.Color.WHITE;
                tabla1.BorderWidthTop = 0;
                tabla1.BorderColorBottom = iTextSharp.text.Color.WHITE;
                tabla1.BorderWidthBottom = 0;
                tabla1.Width = anchoDocumento;
                tabla1.BorderWidth = (float)ancho;                

                iTextSharp.text.Cell celda1 = new Cell(new Phrase(new Chunk("REMISIONES TERMOS CILINDROS - OBSERVACIONES" + "\n" + "\n", fNegrita)));
                celda1.HorizontalAlignment = Element.ALIGN_CENTER;
                celda1.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda1.BorderWidthBottom = 0;
                celda1.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda1.BorderWidthTop = 0;
                tabla1.AddCell(celda1, 0, 0);

                iTextSharp.text.Cell celda2 = new Cell(new Phrase(new Chunk(" " + Documento.EncabezadoDocumento.Observaciones + "\n" + "\n" , fNormal)));
                celda2.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda2.BorderWidthTop = 0;
                celda2.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda2.BorderWidthBottom = (float)ancho;                                
                tabla1.AddCell(celda2, 1, 0);


                iTextSharp.text.Cell celda3 = new Cell(new Phrase(new Chunk(" " + Documento.EncabezadoDocumento.Resolucion + "\n" + " " + Documento.EncabezadoDocumento.Resolucion2 + "\n" , fNegrita)));
                celda3.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda3.BorderWidthTop = 0;
                celda3.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda3.BorderWidthBottom = (float)ancho;                                
                tabla1.AddCell(celda3, 2, 0);
                
                doc4.Add(tabla1);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ImprimirZona5(iTextSharp.text.Document doc5)
        {
            try
            {
                int[] widths = new int[] { 20, 80 };

                iTextSharp.text.Table tabla1 = new Table(2, 3);
                tabla1.BorderColorTop = iTextSharp.text.Color.WHITE;
                tabla1.BorderWidthTop = 0;
                tabla1.BorderColorBottom = iTextSharp.text.Color.WHITE;
                tabla1.BorderWidthBottom = 0;
                tabla1.Width = anchoDocumento;
                tabla1.BorderWidth = (float)ancho;
                tabla1.SetWidths(widths);

                iTextSharp.text.Cell celda1 = new Cell(new Phrase(new Chunk(" "+Documento.EncabezadoDocumento.A1+"\n"+ " " +Documento.EncabezadoDocumento.A2+ "\n" , fNegrita)));
                celda1.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda1.BorderWidthBottom = 0;
                celda1.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda1.BorderWidthTop = 0;
                celda1.Colspan = 2;
                tabla1.AddCell(celda1, 0, 0);

                iTextSharp.text.Cell celda2 = new Cell(new Phrase(new Chunk(" OPERADOR: " + "\n", fNegrita)));
                celda2.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda2.BorderWidthBottom = 0;
                celda2.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda2.BorderWidthTop = 0;                
                tabla1.AddCell(celda2, 1, 0);

                iTextSharp.text.Cell celda3 = new Cell(new Phrase(new Chunk(Documento.Operador.Nombre + "\n" , fNegrita)));
                celda3.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda3.BorderWidthBottom = (float)ancho;
                celda3.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda3.BorderWidthTop = 0;                
                tabla1.AddCell(celda3, 1, 1);

                iTextSharp.text.Cell celda4 = new Cell(new Phrase(new Chunk(" RUTA:" + "\n", fNegrita)));
                celda4.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda4.BorderWidthBottom = 0;
                celda4.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda4.BorderWidthTop = 0;                
                tabla1.AddCell(celda4, 2, 0);

                iTextSharp.text.Cell celda5 = new Cell(new Phrase(new Chunk(Documento.Operador.Ruta + "\n", fNegrita)));
                celda5.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda5.BorderWidthBottom = (float)ancho;
                celda5.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda5.BorderWidthTop = 0;                
                tabla1.AddCell(celda5, 2, 1);

                iTextSharp.text.Cell celda6 = new Cell(new Phrase(new Chunk(" HORA: " + "\n" , fNegrita)));
                celda6.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda6.BorderWidthBottom = 0;
                celda6.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda6.BorderWidthTop = 0;                
                tabla1.AddCell(celda6, 3, 0);

                iTextSharp.text.Cell celda7 = new Cell(new Phrase(new Chunk(Documento.Operador.Hora + "\n" , fNegrita)));
                celda7.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda7.BorderWidthBottom = (float)ancho;
                celda7.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda7.BorderWidthTop = 0;                
                tabla1.AddCell(celda7, 3, 1);
                
                
                doc5.Add(tabla1);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ImprimirZona6(iTextSharp.text.Document doc6)
        {
            try
            {
                int[] widths = new int[] { 40, 60 };
                iTextSharp.text.Table tabla1 = new Table(2, 5);
                
                tabla1.BorderColorBottom = iTextSharp.text.Color.WHITE;
                tabla1.BorderWidthBottom = 0;
                tabla1.Width = anchoDocumento;
                tabla1.BorderWidth = (float)ancho;
                tabla1.SetWidths(widths);

                iTextSharp.text.Cell celda1 = new Cell(new Phrase(new Chunk(" ACEPTADO POR: " + " " + "\n" , fNegrita)));
                celda1.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda1.BorderWidthBottom = 0;
                celda1.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda1.BorderWidthTop = 0;
                celda1.Colspan = 2;
                tabla1.AddCell(celda1, 0, 0);

                iTextSharp.text.Cell celda2 = new Cell(new Phrase(new Chunk(" FIRMA CLIENTE: " + "\n", fNegrita)));
                celda2.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda2.BorderWidthBottom = 0;
                celda2.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda2.BorderWidthTop = 0;
                celda2.Colspan = 2;
                tabla1.AddCell(celda2, 1, 0);

                if (File.Exists(Documento.PathFirma))
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Documento.PathFirma);
                    img.ScaleAbsolute(250f, 40f);
                    img.SetAbsolutePosition(20f, 40f);
                    iTextSharp.text.Cell celda3 = new Cell(img);
                    celda3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celda3.HorizontalAlignment = Element.ALIGN_CENTER;
                    celda3.Colspan = 2;
                    celda3.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda3.BorderWidthBottom = 0;
                    celda3.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda3.BorderWidthTop = 0;
                    tabla1.AddCell(celda3, 2, 0);
                }
                else
                {
                    iTextSharp.text.Cell celda3 = new Cell(new Phrase(new Chunk("\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n", fNegrita)));
                    celda3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celda3.HorizontalAlignment = Element.ALIGN_CENTER;
                    celda3.Colspan = 2;
                    celda3.BorderColorBottom = iTextSharp.text.Color.WHITE;
                    celda3.BorderWidthBottom = 0;
                    celda3.BorderColorTop = iTextSharp.text.Color.WHITE;
                    celda3.BorderWidthTop = 0;
                    tabla1.AddCell(celda3, 2, 0);
                }


                iTextSharp.text.Cell celda5 = new Cell(new Phrase(new Chunk(" NOMBRE: " + "\n", fNegrita)));
                celda5.BorderColorBottom = iTextSharp.text.Color.WHITE;
                celda5.BorderWidthBottom = 0;
                celda5.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda5.BorderWidthTop = 0;
                tabla1.AddCell(celda5, 3, 0);

                iTextSharp.text.Cell celda6 = new Cell(new Phrase(new Chunk(Documento.NombreFirma + "\n", fNegrita)));
                celda6.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda6.BorderWidthBottom = (float) ancho;
                celda6.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda6.BorderWidthTop = 0;
                tabla1.AddCell(celda6, 3, 1);

                iTextSharp.text.Cell celda7 = new Cell(new Phrase(new Chunk(" IDENTIFICACION: " + "\n" + "\n", fNegrita)));
                celda7.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda7.BorderWidthBottom = (float)ancho;
                celda7.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda7.BorderWidthTop = 0;
                tabla1.AddCell(celda7, 4, 0);

                iTextSharp.text.Cell celda8 = new Cell(new Phrase(new Chunk(Documento.IdentificacionFirma + "\n", fNegrita)));
                celda8.BorderColorBottom = iTextSharp.text.Color.BLACK;
                celda8.BorderWidthBottom = (float)ancho;
                celda8.BorderColorTop = iTextSharp.text.Color.WHITE;
                celda8.BorderWidthTop = 0;
                tabla1.AddCell(celda8, 4, 1);
                doc6.Add(tabla1);
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
