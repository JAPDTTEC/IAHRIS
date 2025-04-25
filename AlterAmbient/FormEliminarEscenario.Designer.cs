namespace IAHRIS
{
    partial class FormEliminarEscenario
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
            this.lblEscenario = new System.Windows.Forms.Label();
            this.cmbEscenario = new System.Windows.Forms.ComboBox();
            this.btnEliminarEscenario = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblEscenario
            // 
            this.lblEscenario.AutoSize = true;
            this.lblEscenario.Location = new System.Drawing.Point(34, 20);
            this.lblEscenario.Name = "lblEscenario";
            this.lblEscenario.Size = new System.Drawing.Size(54, 13);
            this.lblEscenario.TabIndex = 40;
            this.lblEscenario.Text = "Escenario";
            // 
            // cmbEscenario
            // 
            this.cmbEscenario.FormattingEnabled = true;
            this.cmbEscenario.Location = new System.Drawing.Point(36, 40);
            this.cmbEscenario.Name = "cmbEscenario";
            this.cmbEscenario.Size = new System.Drawing.Size(121, 21);
            this.cmbEscenario.TabIndex = 39;
            this.cmbEscenario.SelectedIndexChanged += new System.EventHandler(this.cmbEscenario_SelectedIndexChanged);
            // 
            // btnEliminarEscenario
            // 
            this.btnEliminarEscenario.Location = new System.Drawing.Point(36, 90);
            this.btnEliminarEscenario.Name = "btnEliminarEscenario";
            this.btnEliminarEscenario.Size = new System.Drawing.Size(121, 37);
            this.btnEliminarEscenario.TabIndex = 38;
            this.btnEliminarEscenario.Text = "Eliminar Escenario";
            this.btnEliminarEscenario.UseVisualStyleBackColor = true;
            this.btnEliminarEscenario.Click += new System.EventHandler(this.btnEliminarEscenario_Click);
            // 
            // FormEliminarEscenario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 154);
            this.Controls.Add(this.lblEscenario);
            this.Controls.Add(this.cmbEscenario);
            this.Controls.Add(this.btnEliminarEscenario);
            this.Name = "FormEliminarEscenario";
            this.Text = "FormEliminarEscenario";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEscenario;
        private System.Windows.Forms.ComboBox cmbEscenario;
        private System.Windows.Forms.Button btnEliminarEscenario;
    }
}