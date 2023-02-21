using System;
using System.Collections.Generic;
using System.Text;

namespace GenerarPDF
{
    /// <summary>
    /// Clase para almacenar los datos que necesitamos para el documento PDF
    /// </summary>
    public class Documento
    {
        /// <summary>
        /// Guardar el path en donde se almacena el archivo en la terminal //PDF//Otro..
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path de la Firma
        /// </summary>
        public string PathFirma { get; set; }

        /// <summary>
        /// Almacena la fecha y hora de creación del documento
        /// </summary>
        public DateTime FechaCreacion { get; set; }
        /// <summary>
        /// Manejo de los datos de los Productos
        /// </summary>
        public List<DetalleDocumento> DetalleDocumento { get; set; }

        /// <summary>
        /// Manejo de los datos de la Empresa
        /// </summary>
        public Empresa Empresa { get; set; }
        /// <summary>
        /// Manejo de los datos del Cliente
        /// </summary>
        public Cliente Cliente { get; set; }
        /// <summary>
        /// Manejo de los Datos del Encabezado
        /// </summary>
        public EncabezadoDocumento EncabezadoDocumento { get; set; }
        /// <summary>
        /// Manejo de los datos del Operador
        /// </summary>
        public Operador Operador { get; set; }

        public string NombreFirma { get; set; }

        public string IdentificacionFirma { get; set; }

        public string TipoDocumentoPDF { get; set; }
        public string PrefijoDocumentoPDF { get; set; }
        public string ConsecutivoDocumentoPDF { get; set; }
        public string CodigoClientePDF { get; set; }

        public Documento()
        {
            
            this.Empresa = new Empresa();
            this.DetalleDocumento = new List<DetalleDocumento>();
            this.Cliente = new Cliente();
            this.EncabezadoDocumento = new EncabezadoDocumento();
            this.Operador = new Operador();
           

        }
    }
}
