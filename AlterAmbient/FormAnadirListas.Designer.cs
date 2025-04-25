using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAHRIS
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FormAnadirListas : Form
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
            this._btnCargarLista = new System.Windows.Forms.Button();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this._btnExaminar = new System.Windows.Forms.Button();
            this.grpboxDetalles = new System.Windows.Forms.GroupBox();
            this.lblPuntoAs = new System.Windows.Forms.Label();
            this.cmbProyectos = new System.Windows.Forms.ComboBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.cmbPuntos = new System.Windows.Forms.ComboBox();
            this.lblPunto = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblSeleccion = new System.Windows.Forms.Label();
            this.grpboxDetalles.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnCargarLista
            // 
            this._btnCargarLista.Enabled = false;
            this._btnCargarLista.Location = new System.Drawing.Point(315, 137);
            this._btnCargarLista.Margin = new System.Windows.Forms.Padding(4);
            this._btnCargarLista.Name = "_btnCargarLista";
            this._btnCargarLista.Size = new System.Drawing.Size(135, 107);
            this._btnCargarLista.TabIndex = 0;
            this._btnCargarLista.Text = "Cargar";
            this._btnCargarLista.UseVisualStyleBackColor = true;
            this._btnCargarLista.Click += new System.EventHandler(this.btnCargarLista_Click);
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(16, 31);
            this.txtRuta.Margin = new System.Windows.Forms.Padding(4);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(321, 22);
            this.txtRuta.TabIndex = 1;
            // 
            // _btnExaminar
            // 
            this._btnExaminar.Location = new System.Drawing.Point(347, 22);
            this._btnExaminar.Margin = new System.Windows.Forms.Padding(4);
            this._btnExaminar.Name = "_btnExaminar";
            this._btnExaminar.Size = new System.Drawing.Size(127, 39);
            this._btnExaminar.TabIndex = 2;
            this._btnExaminar.Text = "Examinar...";
            this._btnExaminar.UseVisualStyleBackColor = true;
            this._btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // grpboxDetalles
            // 
            this.grpboxDetalles.Controls.Add(this.lblPuntoAs);
            this.grpboxDetalles.Controls.Add(this.cmbProyectos);
            this.grpboxDetalles.Controls.Add(this.lblProyecto);
            this.grpboxDetalles.Controls.Add(this.cmbPuntos);
            this.grpboxDetalles.Controls.Add(this.lblPunto);
            this.grpboxDetalles.Controls.Add(this.lblInfo);
            this.grpboxDetalles.Controls.Add(this._btnCargarLista);
            this.grpboxDetalles.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.grpboxDetalles.Location = new System.Drawing.Point(16, 69);
            this.grpboxDetalles.Margin = new System.Windows.Forms.Padding(4);
            this.grpboxDetalles.Name = "grpboxDetalles";
            this.grpboxDetalles.Padding = new System.Windows.Forms.Padding(4);
            this.grpboxDetalles.Size = new System.Drawing.Size(457, 256);
            this.grpboxDetalles.TabIndex = 3;
            this.grpboxDetalles.TabStop = false;
            this.grpboxDetalles.Text = "Detalles del Fichero";
            // 
            // lblPuntoAs
            // 
            this.lblPuntoAs.AutoSize = true;
            this.lblPuntoAs.Location = new System.Drawing.Point(8, 103);
            this.lblPuntoAs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPuntoAs.Name = "lblPuntoAs";
            this.lblPuntoAs.Size = new System.Drawing.Size(101, 16);
            this.lblPuntoAs.TabIndex = 18;
            this.lblPuntoAs.Text = "Punto asociado";
            // 
            // cmbProyectos
            // 
            this.cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProyectos.FormattingEnabled = true;
            this.cmbProyectos.Location = new System.Drawing.Point(231, 23);
            this.cmbProyectos.Margin = new System.Windows.Forms.Padding(4);
            this.cmbProyectos.Name = "cmbProyectos";
            this.cmbProyectos.Size = new System.Drawing.Size(217, 24);
            this.cmbProyectos.TabIndex = 17;
            this.cmbProyectos.SelectedIndexChanged += new System.EventHandler(this.cmbProyectos_SelectedIndexChanged);
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Location = new System.Drawing.Point(8, 27);
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(121, 16);
            this.lblProyecto.TabIndex = 16;
            this.lblProyecto.Text = "Proyecto asociado";
            // 
            // cmbPuntos
            // 
            this.cmbPuntos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPuntos.FormattingEnabled = true;
            this.cmbPuntos.Location = new System.Drawing.Point(231, 63);
            this.cmbPuntos.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPuntos.Name = "cmbPuntos";
            this.cmbPuntos.Size = new System.Drawing.Size(217, 24);
            this.cmbPuntos.TabIndex = 15;
            this.cmbPuntos.SelectedIndexChanged += new System.EventHandler(this.cmbPuntos_SelectedIndexChanged);
            // 
            // lblPunto
            // 
            this.lblPunto.AutoSize = true;
            this.lblPunto.Location = new System.Drawing.Point(8, 66);
            this.lblPunto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPunto.Name = "lblPunto";
            this.lblPunto.Size = new System.Drawing.Size(101, 16);
            this.lblPunto.TabIndex = 14;
            this.lblPunto.Text = "Punto asociado";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(8, 137);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(103, 16);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "INFORMACION:";
            // 
            // lblSeleccion
            // 
            this.lblSeleccion.AutoSize = true;
            this.lblSeleccion.Location = new System.Drawing.Point(16, 11);
            this.lblSeleccion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeleccion.Name = "lblSeleccion";
            this.lblSeleccion.Size = new System.Drawing.Size(279, 16);
            this.lblSeleccion.TabIndex = 4;
            this.lblSeleccion.Text = "Seleccione la Serie de datos en formato CSV:";
            // 
            // FormAnadirListas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 340);
            this.Controls.Add(this.lblSeleccion);
            this.Controls.Add(this.grpboxDetalles);
            this.Controls.Add(this._btnExaminar);
            this.Controls.Add(this.txtRuta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormAnadirListas";
            this.ShowInTaskbar = false;
            this.Text = "Añadir Series de Datos";
            this.Activated += new System.EventHandler(this.FormAnadirListas_Activated);
            this.Load += new System.EventHandler(this.FormAnadirListas_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormAnadirListas_KeyDown);
            this.grpboxDetalles.ResumeLayout(false);
            this.grpboxDetalles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button _btnCargarLista;

        internal Button btnCargarLista
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCargarLista;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCargarLista != null)
                {
                    _btnCargarLista.Click -= btnCargarLista_Click;
                }

                _btnCargarLista = value;
                if (_btnCargarLista != null)
                {
                    _btnCargarLista.Click += btnCargarLista_Click;
                }
            }
        }

        internal TextBox txtRuta;
        private Button _btnExaminar;

        internal Button btnExaminar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnExaminar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnExaminar != null)
                {
                    _btnExaminar.Click -= btnExaminar_Click;
                }

                _btnExaminar = value;
                if (_btnExaminar != null)
                {
                    _btnExaminar.Click += btnExaminar_Click;
                }
            }
        }

        internal GroupBox grpboxDetalles;
        internal Label lblInfo;
        internal Label lblSeleccion;
        internal ComboBox cmbProyectos;
        internal Label lblProyecto;
        internal ComboBox cmbPuntos;
        internal Label lblPunto;
        internal Label lblPuntoAs;
    }
}