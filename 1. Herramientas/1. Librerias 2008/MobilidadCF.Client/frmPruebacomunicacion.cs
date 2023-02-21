using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MovilidadCF.Client
{
    internal partial class frmPruebacomunicacion : Form, MovilidadCF.Data.DataSetSerializer.IEstadoCarga
    {
        private int iPrueba;



        public frmPruebacomunicacion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Comunicacion.CargaDatosForm.ConfiguracionServidorWS("192.168.148.170", "80", "WSComunicaciones", string.Empty, string.Empty, 90000);
            //Comunicacion.CargaDatosForm.ConfiguracionServidorWS("192.168.148.66", "80", "ChequeoPreciosWS", string.Empty, string.Empty, 90000);
            Object[] oParametros = null;
            oParametros = new Object[] {"02010"};
            Comunicacion.CargaDatosForm.Run("2", "2", "1", OpenNETCF.WindowsCE.DeviceManagement.GetDeviceID(), true, true, false, false, "Cargar Datos", "", "Data Source = \\Mantenimiento.sdf",true, oParametros);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.UiHandler1.Parent = this;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Comunicacion.DescargarDatosForm.ConfiguracionServidorWS("192.168.148.170", "80", "WSComunicaciones", string.Empty, string.Empty, 90000);
            Comunicacion.DescargarDatosForm.Run("1", "3", "1", OpenNETCF.WindowsCE.DeviceManagement.GetDeviceID(), true, true, false, false, "Cargar Datos", "", "Data Source = \\Indupalma.sdf", "02010");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MovilidadCF.Data.SqlServerCe.GestorDatosBase g = new MovilidadCF.Data.SqlServerCe.GestorDatosBase("Data Source= \\ChequeoPrecios.sdf");
            g.SeparadorDecimales = ",";
            g.PersonalizarCulturaHiloEjecucion();
            MovilidadCF.Data.DataSetSerializer nw = new MovilidadCF.Data.DataSetSerializer(g);

            string sDatos = "TABLECOUNT: 1 \r\n" +
            "TOTALROWCOUNT: 1\r\n" +
            "TABLE: Parametros\r\n" +
            "ROWCOUNT: 1\r\n" +
            "usaragenda|rangolongitud|rangolatitud|frecuenciaenvio|\r\n" +
            "System.Boolean|System.Decimal|System.Decimal|System.Int32|\r\n" +
            "False|5,0200000000|5,0300000000|5|\r\n";
            try
            {
                nw.SaveToDatabase(sDatos, true, this, null);
                MessageBox.Show("Exitoso");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                
            }


            
        }

        public int IPrueba
        {
            get { return iPrueba; }
            set { iPrueba = value; }
        }

        #region Miembros de IEstadoCarga

        public bool Cancelado
        {
            get { return false; }
        }

        public int ProgresoTabla
        {
            set { int iPos = 0; }
        }

        public int ProgresoTotal
        {
            set { int iPos = 0; }
        }

        public void IniciarTabla(string sNombreTabla)
        {
            sNombreTabla = sNombreTabla;
        }

        #endregion
    }

         
 }

    