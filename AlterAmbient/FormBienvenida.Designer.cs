using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAHRIS
{
    [DesignerGenerated()]
    public partial class FormBienvenida : Form
    {

        /// <summary>
        /// Form reemplaza a Dispose para limpiar la lista de componentes.
        /// </summary>
        /// <param name="disposing"></param>
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
            this.pbMITECO = new System.Windows.Forms.PictureBox();
            this.pbUPM = new System.Windows.Forms.PictureBox();
            this.pbSiglasESP = new System.Windows.Forms.PictureBox();
            this.pbNombre = new System.Windows.Forms.PictureBox();
            this.pbSiglasENG = new System.Windows.Forms.PictureBox();
            this.pbIcono = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._pbPrograma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMITECO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSiglasESP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNombre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSiglasENG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcono)).BeginInit();
            this.SuspendLayout();
            // 
            // _pbPrograma
            // 
            this._pbPrograma.BackColor = System.Drawing.Color.Transparent;
            this._pbPrograma.Image = global::IAHRIS.My.Resources.Resources.iconoacceso;
            this._pbPrograma.Location = new System.Drawing.Point(731, 449);
            this._pbPrograma.Margin = new System.Windows.Forms.Padding(4);
            this._pbPrograma.Name = "_pbPrograma";
            this._pbPrograma.Size = new System.Drawing.Size(85, 79);
            this._pbPrograma.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._pbPrograma.TabIndex = 0;
            this._pbPrograma.TabStop = false;
            this._pbPrograma.Click += new System.EventHandler(this.pbPrograma_Click);
            this._pbPrograma.MouseLeave += new System.EventHandler(this.pbPrograma_MouseLeave);
            this._pbPrograma.MouseHover += new System.EventHandler(this.pbPrograma_MouseHover);
            // 
            // lblVersionES
            // 
            this.lblVersionES.BackColor = System.Drawing.SystemColors.Control;
            this.lblVersionES.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionES.Location = new System.Drawing.Point(280, 199);
            this.lblVersionES.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersionES.Name = "lblVersionES";
            this.lblVersionES.Size = new System.Drawing.Size(276, 28);
            this.lblVersionES.TabIndex = 1;
            this.lblVersionES.Text = "v.4.0";
            this.lblVersionES.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersionEN
            // 
            this.lblVersionEN.BackColor = System.Drawing.SystemColors.Control;
            this.lblVersionEN.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionEN.Location = new System.Drawing.Point(556, 199);
            this.lblVersionEN.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersionEN.Name = "lblVersionEN";
            this.lblVersionEN.Size = new System.Drawing.Size(276, 28);
            this.lblVersionEN.TabIndex = 2;
            this.lblVersionEN.Text = "v.4.0";
            this.lblVersionEN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbMITECO
            // 
            this.pbMITECO.Image = global::IAHRIS.My.Resources.Resources.Logo_MITECO;
            this.pbMITECO.Location = new System.Drawing.Point(-1, -4);
            this.pbMITECO.Margin = new System.Windows.Forms.Padding(4);
            this.pbMITECO.Name = "pbMITECO";
            this.pbMITECO.Size = new System.Drawing.Size(283, 138);
            this.pbMITECO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMITECO.TabIndex = 3;
            this.pbMITECO.TabStop = false;
            // 
            // pbUPM
            // 
            this.pbUPM.Image = global::IAHRIS.My.Resources.Resources.LOGOTIPO_UPM;
            this.pbUPM.Location = new System.Drawing.Point(0, 133);
            this.pbUPM.Margin = new System.Windows.Forms.Padding(4);
            this.pbUPM.Name = "pbUPM";
            this.pbUPM.Size = new System.Drawing.Size(283, 94);
            this.pbUPM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbUPM.TabIndex = 4;
            this.pbUPM.TabStop = false;
            // 
            // pbSiglasESP
            // 
            this.pbSiglasESP.Image = ((System.Drawing.Image)(resources.GetObject("pbSiglasESP.Image")));
            this.pbSiglasESP.Location = new System.Drawing.Point(281, 133);
            this.pbSiglasESP.Margin = new System.Windows.Forms.Padding(4);
            this.pbSiglasESP.Name = "pbSiglasESP";
            this.pbSiglasESP.Size = new System.Drawing.Size(276, 66);
            this.pbSiglasESP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSiglasESP.TabIndex = 5;
            this.pbSiglasESP.TabStop = false;
            // 
            // pbNombre
            // 
            this.pbNombre.Image = global::IAHRIS.My.Resources.Resources.IAHRIS_TITULO;
            this.pbNombre.Location = new System.Drawing.Point(409, -1);
            this.pbNombre.Margin = new System.Windows.Forms.Padding(4);
            this.pbNombre.Name = "pbNombre";
            this.pbNombre.Size = new System.Drawing.Size(423, 135);
            this.pbNombre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbNombre.TabIndex = 6;
            this.pbNombre.TabStop = false;
            // 
            // pbSiglasENG
            // 
            this.pbSiglasENG.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbSiglasENG.Image = ((System.Drawing.Image)(resources.GetObject("pbSiglasENG.Image")));
            this.pbSiglasENG.Location = new System.Drawing.Point(556, 133);
            this.pbSiglasENG.Margin = new System.Windows.Forms.Padding(4);
            this.pbSiglasENG.Name = "pbSiglasENG";
            this.pbSiglasENG.Size = new System.Drawing.Size(276, 66);
            this.pbSiglasENG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSiglasENG.TabIndex = 7;
            this.pbSiglasENG.TabStop = false;
            // 
            // pbIcono
            // 
            this.pbIcono.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbIcono.Image = global::IAHRIS.My.Resources.Resources.LOGO1;
            this.pbIcono.Location = new System.Drawing.Point(281, -1);
            this.pbIcono.Margin = new System.Windows.Forms.Padding(4);
            this.pbIcono.Name = "pbIcono";
            this.pbIcono.Size = new System.Drawing.Size(135, 135);
            this.pbIcono.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbIcono.TabIndex = 8;
            this.pbIcono.TabStop = false;
            // 
            // FormBienvenida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::IAHRIS.My.Resources.Resources.chorreras_021;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(832, 543);
            this.Controls.Add(this.pbMITECO);
            this.Controls.Add(this.pbIcono);
            this.Controls.Add(this.pbSiglasENG);
            this.Controls.Add(this.pbNombre);
            this.Controls.Add(this.pbSiglasESP);
            this.Controls.Add(this.pbUPM);
            this.Controls.Add(this.lblVersionEN);
            this.Controls.Add(this.lblVersionES);
            this.Controls.Add(this._pbPrograma);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBienvenida";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormBienvenida_Load);
            ((System.ComponentModel.ISupportInitialize)(this._pbPrograma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMITECO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSiglasESP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNombre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSiglasENG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcono)).EndInit();
            this.ResumeLayout(false);

        }

        private PictureBox _pbPrograma;
        private Label lblVersionES;
        private Label lblVersionEN;
        private PictureBox pbMITECO;
        private PictureBox pbUPM;
        private PictureBox pbSiglasESP;
        private PictureBox pbNombre;
        private PictureBox pbSiglasENG;
        private PictureBox pbIcono;

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