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
            this.lblArchivo.Location = new System.Drawing.Point(12, 9);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(35, 13);
            this.lblArchivo.TabIndex = 0;
            this.lblArchivo.Text = "label1";
            // 
            // lblDatos
            // 
            this.lblDatos.AutoSize = true;
            this.lblDatos.Location = new System.Drawing.Point(12, 49);
            this.lblDatos.Name = "lblDatos";
            this.lblDatos.Size = new System.Drawing.Size(35, 13);
            this.lblDatos.TabIndex = 1;
            this.lblDatos.Text = "label2";
            // 
            // lblPorcentaje
            // 
            this.lblPorcentaje.AutoSize = true;
            this.lblPorcentaje.Location = new System.Drawing.Point(200, 85);
            this.lblPorcentaje.Name = "lblPorcentaje";
            this.lblPorcentaje.Size = new System.Drawing.Size(35, 13);
            this.lblPorcentaje.TabIndex = 2;
            this.lblPorcentaje.Text = "label3";
            // 
            // progressBarDatos
            // 
            this.progressBarDatos.Location = new System.Drawing.Point(12, 78);
            this.progressBarDatos.Name = "progressBarDatos";
            this.progressBarDatos.Size = new System.Drawing.Size(438, 26);
            this.progressBarDatos.TabIndex = 3;
            // 
            // ProgresoDatosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 130);
            this.Controls.Add(this.lblPorcentaje);
            this.Controls.Add(this.lblDatos);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.progressBarDatos);
            this.Name = "ProgresoDatosForm";
            this.Text = "Obteniendo Datos...";
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