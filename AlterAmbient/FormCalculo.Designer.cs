using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    [DesignerGenerated()]
    public partial class FormCalculo : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.lblCalculo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCalculo
            // 
            this.lblCalculo.AutoSize = true;
            this.lblCalculo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCalculo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalculo.Location = new System.Drawing.Point(176, 15);
            this.lblCalculo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCalculo.Name = "lblCalculo";
            this.lblCalculo.Size = new System.Drawing.Size(129, 50);
            this.lblCalculo.TabIndex = 1;
            this.lblCalculo.Text = "Calculando...\r\n\r\nEspere por favor.";
            // 
            // FormCalculo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(389, 85);
            this.Controls.Add(this.lblCalculo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCalculo";
            this.Text = "FormCalculo";
            this.Load += new System.EventHandler(this.FormCalculo_Load);
            this.Shown += new System.EventHandler(this.FormCalculo_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal Label lblCalculo;
    }
}