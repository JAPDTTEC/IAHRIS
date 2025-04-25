namespace DatosSimpa
{
    partial class ProgresoDatosForm
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
            this.lblArchivo = new System.Windows.Forms.Label();
            this.lblDatos = new System.Windows.Forms.Label();
            this.lblPorcentaje = new System.Windows.Forms.Label();
            this.progressBarDatos = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblArchivo
            // 
            this.lblArchivo.AutoSize = true;
            this.lblArchivo.Location = new System.Drawing.Point(16, 11);
            this.lblArchivo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(44, 16);
            this.lblArchivo.TabIndex = 0;
            this.lblArchivo.Text = "label1";
            // 
            // lblDatos
            // 
            this.lblDatos.AutoSize = true;
            this.lblDatos.Location = new System.Drawing.Point(16, 60);
            this.lblDatos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDatos.Name = "lblDatos";
            this.lblDatos.Size = new System.Drawing.Size(44, 16);
            this.lblDatos.TabIndex = 1;
            this.lblDatos.Text = "label2";
            // 
            // lblPorcentaje
            // 
            this.lblPorcentaje.AutoSize = true;
            this.lblPorcentaje.Location = new System.Drawing.Point(267, 104);
            this.lblPorcentaje.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPorcentaje.Name = "lblPorcentaje";
            this.lblPorcentaje.Size = new System.Drawing.Size(44, 16);
            this.lblPorcentaje.TabIndex = 2;
            this.lblPorcentaje.Text = "label3";
            // 
            // progressBarDatos
            // 
            this.progressBarDatos.Location = new System.Drawing.Point(16, 95);
            this.progressBarDatos.Margin = new System.Windows.Forms.Padding(4);
            this.progressBarDatos.Name = "progressBarDatos";
            this.progressBarDatos.Size = new System.Drawing.Size(584, 32);
            this.progressBarDatos.TabIndex = 3;
            // 
            // ProgresoDatosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 160);
            this.Controls.Add(this.lblPorcentaje);
            this.Controls.Add(this.lblDatos);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.progressBarDatos);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ProgresoDatosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Obteniendo Datos...";
            this.Load += new System.EventHandler(this.ProgresoDatosForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.Label lblDatos;
        private System.Windows.Forms.Label lblPorcentaje;
        public System.Windows.Forms.ProgressBar progressBarDatos;
    }
}