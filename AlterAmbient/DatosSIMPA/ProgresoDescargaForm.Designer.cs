namespace DatosSimpa
{
    partial class ProgresoDescargaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.lblDescarga = new System.Windows.Forms.Label();
            this.lblPorcentaje = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Location = new System.Drawing.Point(16, 94);
            this.progressBarDownload.Margin = new System.Windows.Forms.Padding(4);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(584, 32);
            this.progressBarDownload.TabIndex = 0;
            // 
            // lblArchivo
            // 
            this.lblArchivo.AutoSize = true;
            this.lblArchivo.Location = new System.Drawing.Point(16, 11);
            this.lblArchivo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(44, 16);
            this.lblArchivo.TabIndex = 1;
            this.lblArchivo.Text = "label1";
            this.lblArchivo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDescarga
            // 
            this.lblDescarga.AutoSize = true;
            this.lblDescarga.Location = new System.Drawing.Point(16, 60);
            this.lblDescarga.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescarga.Name = "lblDescarga";
            this.lblDescarga.Size = new System.Drawing.Size(44, 16);
            this.lblDescarga.TabIndex = 2;
            this.lblDescarga.Text = "label1";
            // 
            // lblPorcentaje
            // 
            this.lblPorcentaje.AutoSize = true;
            this.lblPorcentaje.Location = new System.Drawing.Point(267, 103);
            this.lblPorcentaje.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPorcentaje.Name = "lblPorcentaje";
            this.lblPorcentaje.Size = new System.Drawing.Size(44, 16);
            this.lblPorcentaje.TabIndex = 3;
            this.lblPorcentaje.Text = "label1";
            // 
            // ProgresoDescargaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 160);
            this.ControlBox = false;
            this.Controls.Add(this.lblPorcentaje);
            this.Controls.Add(this.lblDescarga);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.progressBarDownload);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgresoDescargaForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Descargando...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ProgresoDescargaForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.Label lblDescarga;
        public System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.Label lblPorcentaje;
    }
}