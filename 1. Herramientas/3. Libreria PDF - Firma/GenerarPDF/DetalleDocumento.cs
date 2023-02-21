using System;
using System.Collections.Generic;
using System.Text;

namespace GenerarPDF
{
    /// <summary>
    /// cssd
    /// </summary>
    public class DetalleDocumento
    {
        /// <summary>
        /// El Codigo de cada Producto Listado en el Documnto  
        /// </summary>
        public string CodigoProducto { get; set; }
        /// <summary>
        /// La Descripcion de cada Producto listado en el Documento
        /// </summary>
        public string DescripcionProducto { get; set; }
        /// <summary>
        /// La Cantidad del Producto listado en el Documento
        /// </summary>
        public string Cantidad { get; set; }
        /// <summary>
        /// La Unidad del Producto listado en el Documento
        /// </summary>
        public string Unidad { get; set; }
        /// <summary>
        /// El Valor de un Producto listado en el Documento
        /// </summary>
        public string ValorUnidad { get; set; }
        /// <summary>
        /// El Valor Total de los Productos listado en el Documento
        /// </summary>
        public string ValorTotal { get; set; }
    }
}
