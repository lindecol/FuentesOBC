namespace MovilidadCF.Communications.MobilityCentral
{
    partial class EstadoForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnCerrar;
        private OpenNETCF.Windows.Forms.ListBox2 lstRegistro;
        internal System.Windows.Forms.Label lblAccion;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.ImageList ImageList1;
        internal OpenNETCF.Windows.Forms.ProgressBar2 ctlProgress;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EstadoForm));
            this.lblAccion = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.ImageList1 = new System.Windows.Forms.ImageList();
            this.ctlProgress = new OpenNETCF.Windows.Forms.ProgressBar2();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lstRegistro = new OpenNETCF.Windows.Forms.ListBox2();
            this.SuspendLayout();
            // 
            // lblAccion
            // 
            this.lblAccion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAccion.Location = new System.Drawing.Point(8, 193);
            this.lblAccion.Name = "lblAccion";
            this.lblAccion.Size = new System.Drawing.Size(613, 32);
            this.lblAccion.Text = "Inactivo";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(8, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(149, 16);
            this.Label1.Text = "Registro comunicacion:";
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.Label2.Location = new System.Drawing.Point(8, 176);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(64, 16);
            this.Label2.Text = "Estado:";
            this.ImageList1.Images.Clear();
            this.ImageList1.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            this.ImageList1.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
            // 
            // ctlProgress
            // 
            this.ctlProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlProgress.BarColor = System.Drawing.SystemColors.Highlight;
            this.ctlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlProgress.Location = new System.Drawing.Point(8, 232);
            this.ctlProgress.Name = "ctlProgress";
            this.ctlProgress.ShowPercentValueText = true;
            this.ctlProgress.Size = new System.Drawing.Size(613, 16);
            this.ctlProgress.Step = 0;
            this.ctlProgress.TabIndex = 1;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(77, 228);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(469, 24);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.Visible = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lstRegistro
            // 
            this.lstRegistro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRegistro.BackColor = System.Drawing.SystemColors.Window;
            this.lstRegistro.DataSource = null;
            this.lstRegistro.DisplayMember = null;
            this.lstRegistro.EvenItemColor = System.Drawing.SystemColors.Control;
            this.lstRegistro.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lstRegistro.ImageList = this.ImageList1;
            this.lstRegistro.ItemHeight = 18;
            this.lstRegistro.LineColor = System.Drawing.SystemColors.ControlText;
            this.lstRegistro.Location = new System.Drawing.Point(8, 30);
            this.lstRegistro.Name = "listBox21";
            this.lstRegistro.SelectedIndex = -1;
            this.lstRegistro.ShowLines = true;
            this.lstRegistro.ShowScrollbar = true;
            this.lstRegistro.Size = new System.Drawing.Size(616, 126);
            this.lstRegistro.TabIndex = 0;
            this.lstRegistro.TopIndex = 0;
            this.lstRegistro.WrapText = false;
            // 
            // EstadoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.ctlProgress);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblAccion);
            this.Controls.Add(this.lstRegistro);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EstadoForm";
            this.Text = "Estado Comunicaciones";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.EstadoForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        
    }
}