using System;
using System.Collections.Generic;
using System.Text;

namespace GenerarPDF
{
    public class EncabezadoDocumento
    {
        /// <summary>
        /// Especifica el tipo de documento que se va a crear
        /// </summary>
        public string Tipo { get; set; }
        /// <summary>
        /// Numero de Linea Nacional
        /// </summary>
        public string Numero { get; set; }
        /// <summary>
        /// Numero a Bogota 
        /// </summary>
        //public string Contacto { get; set; }
        /// <summary>
        /// La Fecha de Elaboracion del Documento
        /// </summary>
        public DateTime FechaElaboracion { get; set; }
        /// <summary>
        /// La Fecha de Vencimiento del Documento
        /// </summary>
        public DateTime FechaVencimiento { get; set; }
        /// <summary>
        /// El orden de la Compra en el Documento
        /// </summary>
        public string OrdenCompra { get; set; }
        /// <summary>
        /// El Numero de Pedido en el Documento
        /// </summary>
        public string NuPedido { get; set; }
        /// <summary>
        /// El SubTotal de todos los Productos
        /// </summary>
        public string SubTotal { get; set; }
        /// <summary>
        /// El IVA del Total de los Productos
        /// </summary>
        public string IVA { get; set; }
        /// <summary>
        /// El Total de Todos los Productos
        /// </summary>
        public string Total { get; set; }
        /// <summary>
        /// Remision De la zona 4
        /// </summary>
        public string Remisiones { get; set; }
        /// <summary>
        /// Observaciones del producto
        /// </summary>
        public string Observaciones { get; set; }
        /// <summary>
        /// Tipo de Resolucion
        /// </summary>
        public string Resolucion { get; set; }
        /// <summary>
        /// Resolucion de pagos
        /// </summary>
        public string Resolucion2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string A1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string A2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string L1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string L2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string L3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string L4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string L5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string L6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Copia { get; set; }
 
    }
}
