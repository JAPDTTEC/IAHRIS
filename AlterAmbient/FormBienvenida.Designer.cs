using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    [DesignerGenerated()]
    public partial class FormBienvenida : Form
    {

        // Form reemplaza a Dispose para limpiar la lista de componentes.
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

        // Requerido por el Diseñador de Windows Forms
        private System.ComponentModel.IContainer components;

        // NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
        // Se puede modificar usando el Diseñador de Windows Forms.  
        // No lo modifique con el editor de código.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBienvenida));
            this._pbPrograma = new System.Windows.Forms.PictureBox();
            this.lblVersionES = new System.Windows.Forms.Label();
            this.lblVersionEN = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._pbPrograma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // _pbPrograma
            // 
            this._pbPrograma.Image = ((System.Drawing.Image)(resources.GetObject("_pbPrograma.Image")));
            this._pbPrograma.Location = new System.Drawing.Point(692, 308);
            this._pbPrograma.Name = "_pbPrograma";
            this._pbPrograma.Size = new System.Drawing.Size(102, 53);
            this._pbPrograma.TabIndex = 0;
            this._pbPrograma.TabStop = false;
            this._pbPrograma.Click += new System.EventHandler(this.pbPrograma_Click);
            this._pbPrograma.MouseLeave += new System.EventHandler(this.pbPrograma_MouseLeave);
            this._pbPrograma.MouseHover += new System.EventHandler(this.pbPrograma_MouseHover);
            // 
            // lblVersionES
            // 
            this.lblVersionES.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(232)))), ((int)(((byte)(231)))));
            this.lblVersionES.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionES.Location = new System.Drawing.Point(314, 147);
            this.lblVersionES.Name = "lblVersionES";
            this.lblVersionES.Size = new System.Drawing.Size(224, 17);
            this.lblVersionES.TabIndex = 1;
            this.lblVersionES.Text = "v.2.3";
            // 
            // lblVersionEN
            // 
            this.lblVersionEN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(232)))), ((int)(((byte)(231)))));
            this.lblVersionEN.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionEN.Location = new System.Drawing.Point(558, 147);
            this.lblVersionEN.Name = "lblVersionEN";
            this.lblVersionEN.Size = new System.Drawing.Size(224, 17);
            this.lblVersionEN.TabIndex = 2;
            this.lblVersionEN.Text = "v.2.3";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(306, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(142, 66);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(165, 116);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // FormBienvenida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(794, 388);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblVersionEN);
            this.Controls.Add(this.lblVersionES);
            this.Controls.Add(this._pbPrograma);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBienvenida";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this._pbPrograma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        private PictureBox _pbPrograma;
        private Label lblVersionES;
        private Label lblVersionEN;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;

        internal PictureBox pbPrograma
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _pbPrograma;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_pbPrograma != null)
                {
                    _pbPrograma.Click -= pbPrograma_Click;
                    _pbPrograma.MouseHover -= pbPrograma_MouseHover;
                    _pbPrograma.MouseLeave -= pbPrograma_MouseLeave;
                }

                _pbPrograma = value;
                if (_pbPrograma != null)
                {
                    _pbPrograma.Click += pbPrograma_Click;
                    _pbPrograma.MouseHover += pbPrograma_MouseHover;
                    _pbPrograma.MouseLeave += pbPrograma_MouseLeave;
                }
            }
        }
    }
}