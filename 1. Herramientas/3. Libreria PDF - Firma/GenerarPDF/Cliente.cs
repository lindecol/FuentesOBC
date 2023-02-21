using System;

using System.Collections.Generic;
using System.Text;

namespace GenerarPDF
{
    public class Cliente
    {
        /// <summary>
        /// El Nombre del Cliente
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// El Codigo del Cliente
        /// </summary>
        public string CodigoCliente { get; set; }
        /// <summary>
        /// La identificacion del Cliente
        /// </summary>
        public string Identificacion { get; set; }
        /// <summary>
        /// El Barrio del Cliente
        /// </summary>
        public string Barrio { get; set; }
        /// <summary>
        /// El telefono del Cliente
        /// </summary>
        public string Telefono { get; set; }
        /// <summary>
        /// El Nombre de quien Acepta el Documento
        /// </summary>
        public string NombreAceptado { get; set; }
        /// <summary>
        /// Direccion de la Cliente 
        /// </summary>
        public string Direccion { get; set; }
        /// <summary>
        /// Ciudad de la Cliente
        /// </summary>
        public string Ciudad { get; set; }
        /// <summary>
        /// Entidad de la Cliente
        /// </summary>
        public string Entidad { get; set; }
        /// <summary>
        /// Ciudad de la Cliente
        /// </summary>
        public string Subdivision { get; set; }
        /// <summary>
        /// Ciudad de la Cliente
        /// </summary>
        public string TipoPago { get; set; }

    }
}
