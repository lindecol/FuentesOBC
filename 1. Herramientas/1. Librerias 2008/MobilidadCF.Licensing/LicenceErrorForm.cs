using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MovilidadCF.Licensing
{
    public partial class LicenceErrorForm : Form
    {
        private string _nombreProducto = "";
        private string _idDispositivo = "";
        private string _mensajeError = "";
        public static int _rtnRespuesta;
        public int _tipoTerminal;

        public LicenceErrorForm()
        {
            InitializeComponent();
        }

        public static string Run(string Producto, string IdDispositivo, string Mensaje, int tipoTerminal)
        {
            LicenceErrorForm form = new LicenceErrorForm();
            form._nombreProducto = Producto;
            form._idDispositivo = IdDispositivo;
            form._mensajeError = Mensaje;
            form._tipoTerminal = tipoTerminal;
            form.ShowDialog();
            string NumeroLicencia = "";
            form.Dispose();
            return NumeroLicencia;
        }              

        private void btnAceptar_Click_1(object sender, EventArgs e)
        {
            //if (this.txtIDLicencia.Text == string.Empty)
            //{
            //    MessageBox.Show("Debe ingresar la licencia del producto!", "Licencia");
            //    this.txtIDLicencia.Focus();
            //    return;
            //}
            DialogResult = DialogResult.OK;
        }

        private void LicenceErrorForm_Load_1(object sender, EventArgs e)
        {
            //MC3090 = 1
            //MC9090 = 2
            //MC1000 = 3
            if (_tipoTerminal == 2)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Menu = null;               
            } 
            lbNombreProducto.Text = _nombreProducto;
            lblIdDispositivo.Text = _idDispositivo;            
        }

        

    }
}