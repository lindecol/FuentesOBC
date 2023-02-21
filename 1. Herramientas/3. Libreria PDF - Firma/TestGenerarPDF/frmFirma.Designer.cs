namespace PruebaWM
{
    partial class frmFirma
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.ctlFirma = new OpenNETCF.Windows.Forms.Signature();
            this.btnCreateIMG = new System.Windows.Forms.Button();
            this.Limpiar = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // ctlFirma
            // 
            this.ctlFirma.BackColor = System.Drawing.SystemColors.Window;
            this.ctlFirma.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ctlFirma.Location = new System.Drawing.Point(3, 3);
            this.ctlFirma.Name = "ctlFirma";
            this.ctlFirma.PenWidth = 5F;
            this.ctlFirma.Size = new System.Drawing.Size(234, 169);
            this.ctlFirma.TabIndex = 0;
            this.ctlFirma.Text = "ctlFirma";
            this.ctlFirma.Click += new System.EventHandler(this.ctlFirma_Click);
            // 
            // btnCreateIMG
            // 
            this.btnCreateIMG.BackColor = System.Drawing.SystemColors.Window;
            this.btnCreateIMG.Location = new System.Drawing.Point(59, 221);
            this.btnCreateIMG.Name = "btnCreateIMG";
            this.btnCreateIMG.Size = new System.Drawing.Size(129, 25);
            this.btnCreateIMG.TabIndex = 5;
            this.btnCreateIMG.Text = "Guardar Firma";
            this.btnCreateIMG.Click += new System.EventHandler(this.btnCreatePDF_Click_1);
            // 
            // Limpiar
            // 
            this.Limpiar.BackColor = System.Drawing.SystemColors.Window;
            this.Limpiar.Location = new System.Drawing.Point(74, 178);
            this.Limpiar.Name = "Limpiar";
            this.Limpiar.Size = new System.Drawing.Size(99, 25);
            this.Limpiar.TabIndex = 6;
            this.Limpiar.Text = "Limpiar";
            this.Limpiar.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmFirma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.Limpiar);
            this.Controls.Add(this.btnCreateIMG);
            this.Controls.Add(this.ctlFirma);
            this.Menu = this.mainMenu1;
            this.Name = "frmFirma";
            this.Text = "frmFirma";
            this.Load += new System.EventHandler(this.frmFirma_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenNETCF.Windows.Forms.Signature ctlFirma;
        private System.Windows.Forms.Button btnCreateIMG;
        private System.Windows.Forms.Button Limpiar;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}