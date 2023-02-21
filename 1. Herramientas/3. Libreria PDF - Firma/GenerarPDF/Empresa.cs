using System;
using System.Collections.Generic;
using System.Text;

namespace GenerarPDF
{
    /// <summary>
    /// Clase para manejar los datos de la empresa
    /// </summary>
    public class Empresa
    {
        /// <summary>
        /// Nombre de la empres PRAXAIR
        /// </summary>
        /// 
        public string Nombre { get; set; }        
        /// <summary>
        /// Nit de la empresa o cedula del encargado 
        /// </summary>
        public string Identificacion { get; set; }
        /// <summary>
        /// Linea de Emergencia a la empresa
        /// </summary>
        public string LineaEmergencia { get; set; }

    }
}
