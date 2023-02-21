using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GenerarPDF
{
    public class GeneradorDocumentos
    {
        public Documento DatosImprimir { get; set; }

        public string PathDocumento { get; set; }

        public GeneradorDocumentos()
        {
            this.DatosImprimir = new Documento();
        }

        public void CargarDatosPrueba()
        {
            //Datos Zona 1            
            this.DatosImprimir.Empresa.Nombre = "OXIGENOS DE COLOMBIA LTDA";
            this.DatosImprimir.Cliente.Direccion = "CALLE 35 N° 53-21";
            this.DatosImprimir.Cliente.Ciudad = "MEDELLIN";
            this.DatosImprimir.Cliente.Barrio = "MADELENA";
            this.DatosImprimir.Cliente.Telefono = "6900777";
            this.DatosImprimir.Empresa.Identificacion = "1111111111";
            this.DatosImprimir.Empresa.LineaEmergencia = "018000-510-003";
            this.DatosImprimir.EncabezadoDocumento.L5 = "SOMOS RETENEDORES DE IVA";
            this.DatosImprimir.EncabezadoDocumento.L6 = "AGENTE RETENEDOR DE IMPUESTOS SOBRE LAS VENTAS";
            //Datos Zona 2
            this.DatosImprimir.EncabezadoDocumento.Tipo = "REMISION - 913-4171";
            this.DatosImprimir.EncabezadoDocumento.Numero = "018000-527-527";
            //this.DatosImprimir.EncabezadoDocumento.Contacto = "7052000 - 4013000";
            this.DatosImprimir.EncabezadoDocumento.FechaElaboracion = Convert.ToDateTime("2/3/19 16:25:00");
            this.DatosImprimir.EncabezadoDocumento.FechaVencimiento = Convert.ToDateTime("10/23/19 18:00:00");
            this.DatosImprimir.Cliente.CodigoCliente = "0021526";
            this.DatosImprimir.EncabezadoDocumento.OrdenCompra = "123546";
            this.DatosImprimir.EncabezadoDocumento.NuPedido = "0000116";

            //5 productos
            //Datos Zona 3
            GenerarPDF.DetalleDocumento producto1 = new GenerarPDF.DetalleDocumento();
            producto1.CodigoProducto = "012020220";
            producto1.DescripcionProducto = "NITROGENO LIQUIDO EN";
            producto1.Cantidad = "103";
            producto1.Unidad = "m3";
            producto1.ValorUnidad = "60";
            producto1.ValorTotal = "";
            this.DatosImprimir.DetalleDocumento.Add(producto1);

            GenerarPDF.DetalleDocumento producto2 = new GenerarPDF.DetalleDocumento();
            producto2.CodigoProducto = "123467789";
            producto2.DescripcionProducto = "OXIGENO";
            producto2.Cantidad = "562";
            producto2.Unidad = "d4";
            producto2.ValorUnidad = "80";
            producto2.ValorTotal = "";
            this.DatosImprimir.DetalleDocumento.Add(producto2);

            GenerarPDF.DetalleDocumento producto3 = new GenerarPDF.DetalleDocumento();
            producto3.CodigoProducto = "987654321";
            producto3.DescripcionProducto = "CO2";
            producto3.Cantidad = "";
            producto3.Unidad = "a9";
            producto3.ValorUnidad = "";
            producto3.ValorTotal = "";
            this.DatosImprimir.DetalleDocumento.Add(producto3);

            //Datos Zona 3 Totales
            this.DatosImprimir.EncabezadoDocumento.SubTotal = "";
            this.DatosImprimir.EncabezadoDocumento.Total = "";
            this.DatosImprimir.EncabezadoDocumento.IVA = "";

            //Datos Zona 4
            this.DatosImprimir.EncabezadoDocumento.Remisiones = "TERMOS CILINDROS";
            this.DatosImprimir.EncabezadoDocumento.Observaciones = "Pres.Ini: 55, Pres.Fin: 60, Nivel.Ini: 6, Nivel.Fin: 109";
            this.DatosImprimir.EncabezadoDocumento.Resolucion = "[RESOLUCION2]";

            //Datos Zona 5
            this.DatosImprimir.Operador.Nombre = "LUIS ALEJANDRO LEMUS FORERO";
            this.DatosImprimir.Operador.Ruta = "LQ_0008";

            //Datos Zona 6
            this.DatosImprimir.EncabezadoDocumento.A1 = "Esta factura de venta se asimila en todos sus efectos a letra de cambio(Art. 774 codigo comercio)";
            this.DatosImprimir.EncabezadoDocumento.A2 = "y como tal se acepta por el comprador.";
            this.DatosImprimir.Cliente.Nombre = "JOSE MAPE";
            this.DatosImprimir.Cliente.Identificacion = "1234567891";
            this.DatosImprimir.EncabezadoDocumento.Copia = "ORIGINAL";

        }

        public bool GenerarPDFDocumento(string pathFirma, bool esGases)
        {
            this.PathDocumento = this.NombreArchivo();

            if (!Directory.Exists("\\DocumentosPDF"))
            {
                Directory.CreateDirectory("\\DocumentosPDF");
            }

            this.PathDocumento = Path.Combine("\\DocumentosPDF", this.PathDocumento);

            //Datos Paths
            this.DatosImprimir.PathFirma = pathFirma;
            this.DatosImprimir.Path = this.PathDocumento;

            GenerarPDF.PDF pdf = new GenerarPDF.PDF(this.DatosImprimir);

            if (esGases)
            {
                return pdf.DibujarPDFGases();
            }
            else
            {
                return pdf.DibujarPDF();
            }

        }

        private string NombreArchivo()
        {
            //- TIPO_DOCUMENTO 2
            //- PREFIJO LOGITUD 5
            //- CONSECUTIVO 8
            //- CODIGO CLEINTE 7 
            //- YYYYMMDD 8

            string tipoDocumento;
            if (this.DatosImprimir.TipoDocumentoPDF.Length == 2)
            {
                tipoDocumento = this.DatosImprimir.TipoDocumentoPDF;
            }
            else if (this.DatosImprimir.TipoDocumentoPDF.Length > 2)
            {
                tipoDocumento = this.DatosImprimir.TipoDocumentoPDF.Substring(0, 2);
            }
            else
            {
                tipoDocumento = this.DatosImprimir.TipoDocumentoPDF.PadLeft(2, '0');
            }

            //Prefijo
            string prefijo;
            if (this.DatosImprimir.PrefijoDocumentoPDF.Length == 5)
            {
                prefijo = this.DatosImprimir.PrefijoDocumentoPDF;
            }
            else if (this.DatosImprimir.PrefijoDocumentoPDF.Length > 5)
            {
                prefijo = this.DatosImprimir.PrefijoDocumentoPDF.Substring(0, 5);
            }
            else
            {
                prefijo = this.DatosImprimir.PrefijoDocumentoPDF.PadLeft(5, '0');
            }

            //Consecutivo
            string consecutivo;
            if (this.DatosImprimir.ConsecutivoDocumentoPDF.Length == 8)
            {
                consecutivo = this.DatosImprimir.ConsecutivoDocumentoPDF;
            }
            else if (this.DatosImprimir.ConsecutivoDocumentoPDF.Length > 8)
            {
                consecutivo = this.DatosImprimir.ConsecutivoDocumentoPDF.Substring(0, 8);
            }
            else
            {
                consecutivo = this.DatosImprimir.ConsecutivoDocumentoPDF.PadLeft(8, '0');
            }

            //Cliente
            string cliente;
            if (this.DatosImprimir.CodigoClientePDF.Length == 7)
            {
                cliente = this.DatosImprimir.CodigoClientePDF;
            }
            else if (this.DatosImprimir.CodigoClientePDF.Length > 7)
            {
                cliente = this.DatosImprimir.CodigoClientePDF.Substring(0, 7);
            }
            else
            {
                cliente = this.DatosImprimir.CodigoClientePDF.PadLeft(7, '0');
            }

            return tipoDocumento + prefijo + consecutivo + cliente + System.DateTime.Now.ToString("yyyyMMdd") + ".pdf";


        }
    }
}
