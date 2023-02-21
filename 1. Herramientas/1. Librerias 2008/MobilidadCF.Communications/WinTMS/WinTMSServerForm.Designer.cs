namespace MovilidadCF.Communications.WinTMS
{
    partial class WinTMSServerForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.ctlProgress = new OpenNETCF.Windows.Forms.ProgressBar2();
            this.txtRegistro = new System.Windows.Forms.TextBox();
            this.uiHandler1 = new MovilidadCF.Windows.Forms.UIHandler();
            this.SuspendLayout();
            // 
            // ctlProgress
            // 
            this.ctlProgress.BarColor = System.Drawing.SystemColors.Desktop;
            this.ctlProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ctlProgress.Location = new System.Drawing.Point(0, 140);
            this.ctlProgress.Maximum = 100;
            this.ctlProgress.Name = "ctlProgress";
            this.ctlProgress.ShowPercentValueText = true;
            this.ctlProgress.Size = new System.Drawing.Size(211, 21);
            this.ctlProgress.Step = 0;
            this.ctlProgress.TabIndex = 0;
            // 
            // txtRegistro
            // 
            this.txtRegistro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegistro.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRegistro.Location = new System.Drawing.Point(0, 0);
            this.txtRegistro.Multiline = true;
            this.txtRegistro.Name = "txtRegistro";
            this.txtRegistro.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRegistro.Size = new System.Drawing.Size(211, 140);
            this.txtRegistro.TabIndex = 1;
            // 
            // uiHandler1
            // 
            this.uiHandler1.HelpControl = null;
            this.uiHandler1.Parent = null;
            // 
            // WinTMSServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(211, 161);
            this.Controls.Add(this.txtRegistro);
            this.Controls.Add(this.ctlProgress);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinTMSServerForm";
            this.Text = "Servidor WinTMS";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WinTMSServerForm_Closing);
            this.Load += new System.EventHandler(this.WinTMSServerForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenNETCF.Windows.Forms.ProgressBar2 ctlProgress;
        private System.Windows.Forms.TextBox txtRegistro;
        private MovilidadCF.Windows.Forms.UIHandler uiHandler1;
    }
}