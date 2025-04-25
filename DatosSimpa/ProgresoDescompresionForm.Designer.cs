namespace DatosSimpa
{
    partial class ProgresoDescompresionForm
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
            this.progressBarDesc = new System.Windows.Forms.ProgressBar();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.lblDescompresion = new System.Windows.Forms.Label();
            this.lblPorcentaje = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarDesc
            // 
            this.progressBarDesc.Location = new System.Drawing.Point(12, 78);
            this.progressBarDesc.Name = "progressBarDesc";
            this.progressBarDesc.Size = new System.Drawing.Size(438, 26);
            this.progressBarDesc.TabIndex = 0;
            // 
            // lblArchivo
            // 
            this.lblArchivo.AutoSize = true;
            this.lblArchivo.Location = new System.Drawing.Point(12, 9);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(35, 13);
            this.lblArchivo.TabIndex = 1;
            this.lblArchivo.Text = "label1";
            // 
            // lblDescompresion
            // 
            this.lblDescompresion.AutoSize = true;
            this.lblDescompresion.Location = new System.Drawing.Point(12, 49);
            this.lblDescompresion.Name = "lblDescompresion";
            this.lblDescompresion.Size = new System.Drawing.Size(35, 13);
            this.lblDescompresion.TabIndex = 2;
            this.lblDescompresion.Text = "label2";
            // 
            // lblPorcentaje
            // 
            this.lblPorcentaje.AutoSize = true;
            this.lblPorcentaje.Location = new System.Drawing.Point(200, 85);
            this.lblPorcentaje.Name = "lblPorcentaje";
            this.lblPorcentaje.Size = new System.Drawing.Size(35, 13);
            this.lblPorcentaje.TabIndex = 3;
            this.lblPorcentaje.Text = "label3";
            // 
            // ProgresoDescompresionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 130);
            this.Controls.Add(this.lblPorcentaje);
            this.Controls.Add(this.lblDescompresion);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.progressBarDesc);
            this.Name = "ProgresoDescompresionForm";
            this.Text = "Descomprimiendo...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.Label lblDescompresion;
        private System.Windows.Forms.Label lblPorcentaje;
        public System.Windows.Forms.ProgressBar progressBarDesc;
    }
}