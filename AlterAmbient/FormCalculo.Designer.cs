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
            lblCalculo = new Label();
            SuspendLayout();
            // 
            // lblCalculo
            // 
            lblCalculo.AutoSize = true;
            lblCalculo.BorderStyle = BorderStyle.FixedSingle;
            lblCalculo.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
            lblCalculo.Location = new Point(132, 12);
            lblCalculo.Name = "lblCalculo";
            lblCalculo.Size = new Size(130, 50);
            lblCalculo.TabIndex = 1;
            lblCalculo.Text = "Calculando..." + '\r' + '\n' + '\r' + '\n' + "Espere por favor.";
            // 
            // FormCalculo
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(292, 69);
            Controls.Add(lblCalculo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormCalculo";
            Text = "FormCalculo";
            Load += new EventHandler(FormCalculo_Load);
            Shown += new EventHandler(FormCalculo_Shown);
            ResumeLayout(false);
            PerformLayout();
        }

        internal Label lblCalculo;
    }
}