using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;

namespace MovilidadCF.Communications.MobilityCentral
{
    public partial class EstadoForm : System.Windows.Forms.Form
    {
        public EstadoForm()
        {
            InitializeComponent();
        }

        private void EstadoForm_Load(object sender, EventArgs e)
        {

        }

        public enum IconImage
        {
            OK = 0,
            Error = 1,
            Alert = 2
        }

        public void InitAction(string sAccion, bool bShowProgress)
        {
            if (bShowProgress)
            {
                this.ctlProgress.Minimum = 0;
                this.ctlProgress.Value = 0;
                this.ctlProgress.Maximum = 100;
                this.ctlProgress.Visible = true;
            }
            else
            {
                this.ctlProgress.Visible = false;
            }
            this.lblAccion.Text = sAccion;
            Application2.DoEvents();
        }

        public void EndAction()
        {
            ctlProgress.Visible = false;
            AddLog(lblAccion.Text, IconImage.OK);
            lblAccion.Text = "";
            Application2.DoEvents();
        }

        public void CancelAction(Exception ex)
        {
            ctlProgress.Visible = false;
            AddLog(lblAccion.Text, IconImage.Error);
            Application2.DoEvents();
        }

        public void AddLog(string sMsg, IconImage Icon)
        {
            ListItem lItem = new ListItem(sMsg);
            lItem.ImageIndex = Convert.ToInt32(Icon);
            switch (Icon)
            {
                case IconImage.OK:
                    lItem.ForeColor = Color.Black;
                    break;

                case IconImage.Alert:

                    lItem.ForeColor = Color.Black;
                    break;
                case IconImage.Error:

                    lItem.ForeColor = Color.Red;
                    break;
            }
            lstRegistro.Items.Add(lItem);
        }

        private void btnCerrar_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}