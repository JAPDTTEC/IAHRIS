using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAHRIS
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FormAnadirPunto : Form
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
            this._btnAceptar = new System.Windows.Forms.Button();
            this._txtClave = new System.Windows.Forms.TextBox();
            this._txtNombre = new System.Windows.Forms.TextBox();
            this._btnCancelar = new System.Windows.Forms.Button();
            this.gbAnadirPunto = new System.Windows.Forms.GroupBox();
            this.lblNombreRegimen = new System.Windows.Forms.Label();
            this._txtNombreReg = new System.Windows.Forms.TextBox();
            this._chkboxPersReg = new System.Windows.Forms.CheckBox();
            this._txtAbreviatura = new System.Windows.Forms.TextBox();
            this.lblAbrev = new System.Windows.Forms.Label();
            this.cmbProyectos = new System.Windows.Forms.ComboBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.cmbPuntos = new System.Windows.Forms.ComboBox();
            this.lblPunto = new System.Windows.Forms.Label();
            this.lblDescrip = new System.Windows.Forms.Label();
            this.lblCodigo = new System.Windows.Forms.Label();
            this._btnEditar = new System.Windows.Forms.Button();
            this.gbAnadirPunto.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnAceptar
            // 
            this._btnAceptar.Location = new System.Drawing.Point(11, 292);
            this._btnAceptar.Margin = new System.Windows.Forms.Padding(4);
            this._btnAceptar.Name = "_btnAceptar";
            this._btnAceptar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._btnAceptar.Size = new System.Drawing.Size(116, 52);
            this._btnAceptar.TabIndex = 0;
            this._btnAceptar.Text = "Aceptar";
            this._btnAceptar.UseVisualStyleBackColor = true;
            this._btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // _txtClave
            // 
            this._txtClave.Location = new System.Drawing.Point(176, 27);
            this._txtClave.Margin = new System.Windows.Forms.Padding(4);
            this._txtClave.Name = "_txtClave";
            this._txtClave.Size = new System.Drawing.Size(205, 22);
            this._txtClave.TabIndex = 6;
            this._txtClave.TextChanged += new System.EventHandler(this.txtClave_TextChanged);
            // 
            // _txtNombre
            // 
            this._txtNombre.Location = new System.Drawing.Point(176, 59);
            this._txtNombre.Margin = new System.Windows.Forms.Padding(4);
            this._txtNombre.Name = "_txtNombre";
            this._txtNombre.Size = new System.Drawing.Size(205, 22);
            this._txtNombre.TabIndex = 7;
            this._txtNombre.TextChanged += new System.EventHandler(this.txtNombre_TextChanged);
            // 
            // _btnCancelar
            // 
            this._btnCancelar.BackColor = System.Drawing.SystemColors.Control;
            this._btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancelar.Location = new System.Drawing.Point(290, 292);
            this._btnCancelar.Margin = new System.Windows.Forms.Padding(4);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(116, 52);
            this._btnCancelar.TabIndex = 8;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = false;
            this._btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // gbAnadirPunto
            // 
            this.gbAnadirPunto.Controls.Add(this.lblNombreRegimen);
            this.gbAnadirPunto.Controls.Add(this._txtNombreReg);
            this.gbAnadirPunto.Controls.Add(this._chkboxPersReg);
            this.gbAnadirPunto.Controls.Add(this._txtAbreviatura);
            this.gbAnadirPunto.Controls.Add(this.lblAbrev);
            this.gbAnadirPunto.Controls.Add(this.cmbProyectos);
            this.gbAnadirPunto.Controls.Add(this.lblProyecto);
            this.gbAnadirPunto.Controls.Add(this.cmbPuntos);
            this.gbAnadirPunto.Controls.Add(this.lblPunto);
            this.gbAnadirPunto.Controls.Add(this.lblDescrip);
            this.gbAnadirPunto.Controls.Add(this.lblCodigo);
            this.gbAnadirPunto.Controls.Add(this._txtClave);
            this.gbAnadirPunto.Controls.Add(this._txtNombre);
            this.gbAnadirPunto.Location = new System.Drawing.Point(11, 15);
            this.gbAnadirPunto.Margin = new System.Windows.Forms.Padding(4);
            this.gbAnadirPunto.Name = "gbAnadirPunto";
            this.gbAnadirPunto.Padding = new System.Windows.Forms.Padding(4);
            this.gbAnadirPunto.Size = new System.Drawing.Size(395, 267);
            this.gbAnadirPunto.TabIndex = 10;
            this.gbAnadirPunto.TabStop = false;
            this.gbAnadirPunto.Text = "Datos del ";
            // 
            // lblNombreRegimen
            // 
            this.lblNombreRegimen.AutoSize = true;
            this.lblNombreRegimen.Location = new System.Drawing.Point(8, 126);
            this.lblNombreRegimen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombreRegimen.Name = "lblNombreRegimen";
            this.lblNombreRegimen.Size = new System.Drawing.Size(130, 16);
            this.lblNombreRegimen.TabIndex = 17;
            this.lblNombreRegimen.Text = "Nombre del régimen";
            // 
            // _txtNombreReg
            // 
            this._txtNombreReg.Enabled = false;
            this._txtNombreReg.Location = new System.Drawing.Point(176, 122);
            this._txtNombreReg.Margin = new System.Windows.Forms.Padding(4);
            this._txtNombreReg.Name = "_txtNombreReg";
            this._txtNombreReg.Size = new System.Drawing.Size(205, 22);
            this._txtNombreReg.TabIndex = 11;
            // 
            // _chkboxPersReg
            // 
            this._chkboxPersReg.AutoSize = true;
            this._chkboxPersReg.Location = new System.Drawing.Point(12, 91);
            this._chkboxPersReg.Margin = new System.Windows.Forms.Padding(4);
            this._chkboxPersReg.Name = "_chkboxPersReg";
            this._chkboxPersReg.Size = new System.Drawing.Size(221, 20);
            this._chkboxPersReg.TabIndex = 15;
            this._chkboxPersReg.Text = "Personalizar nombre de régimen";
            this._chkboxPersReg.UseVisualStyleBackColor = true;
            this._chkboxPersReg.CheckedChanged += new System.EventHandler(this._chkboxPersReg_CheckedChanged);
            // 
            // _txtAbreviatura
            // 
            this._txtAbreviatura.Enabled = false;
            this._txtAbreviatura.Location = new System.Drawing.Point(176, 154);
            this._txtAbreviatura.Margin = new System.Windows.Forms.Padding(4);
            this._txtAbreviatura.Name = "_txtAbreviatura";
            this._txtAbreviatura.Size = new System.Drawing.Size(205, 22);
            this._txtAbreviatura.TabIndex = 12;
            this._txtAbreviatura.TextChanged += new System.EventHandler(this.txtAbreviatura_TextChanged);
            // 
            // lblAbrev
            // 
            this.lblAbrev.AutoSize = true;
            this.lblAbrev.Location = new System.Drawing.Point(8, 158);
            this.lblAbrev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAbrev.Name = "lblAbrev";
            this.lblAbrev.Size = new System.Drawing.Size(76, 16);
            this.lblAbrev.TabIndex = 14;
            this.lblAbrev.Text = "Abreviatura";
            // 
            // cmbProyectos
            // 
            this.cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProyectos.FormattingEnabled = true;
            this.cmbProyectos.Location = new System.Drawing.Point(176, 194);
            this.cmbProyectos.Margin = new System.Windows.Forms.Padding(4);
            this.cmbProyectos.Name = "cmbProyectos";
            this.cmbProyectos.Size = new System.Drawing.Size(205, 24);
            this.cmbProyectos.TabIndex = 13;
            this.cmbProyectos.SelectedIndexChanged += new System.EventHandler(this.cmbProyectos_SelectedIndexChanged);
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Location = new System.Drawing.Point(8, 198);
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(121, 16);
            this.lblProyecto.TabIndex = 12;
            this.lblProyecto.Text = "Proyecto asociado";
            // 
            // cmbPuntos
            // 
            this.cmbPuntos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPuntos.FormattingEnabled = true;
            this.cmbPuntos.Location = new System.Drawing.Point(176, 228);
            this.cmbPuntos.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPuntos.Name = "cmbPuntos";
            this.cmbPuntos.Size = new System.Drawing.Size(205, 24);
            this.cmbPuntos.TabIndex = 11;
            // 
            // lblPunto
            // 
            this.lblPunto.AutoSize = true;
            this.lblPunto.Location = new System.Drawing.Point(8, 231);
            this.lblPunto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPunto.Name = "lblPunto";
            this.lblPunto.Size = new System.Drawing.Size(101, 16);
            this.lblPunto.TabIndex = 10;
            this.lblPunto.Text = "Punto asociado";
            // 
            // lblDescrip
            // 
            this.lblDescrip.AutoSize = true;
            this.lblDescrip.Location = new System.Drawing.Point(8, 63);
            this.lblDescrip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescrip.Name = "lblDescrip";
            this.lblDescrip.Size = new System.Drawing.Size(85, 16);
            this.lblDescrip.TabIndex = 9;
            this.lblDescrip.Text = "Descripción  ";
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(8, 31);
            this.lblCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(57, 16);
            this.lblCodigo.TabIndex = 8;
            this.lblCodigo.Text = "Código  ";
            // 
            // _btnEditar
            // 
            this._btnEditar.Location = new System.Drawing.Point(151, 292);
            this._btnEditar.Margin = new System.Windows.Forms.Padding(4);
            this._btnEditar.Name = "_btnEditar";
            this._btnEditar.Size = new System.Drawing.Size(116, 52);
            this._btnEditar.TabIndex = 11;
            this._btnEditar.Text = "Editar";
            this._btnEditar.UseVisualStyleBackColor = true;
            this._btnEditar.Click += new System.EventHandler(this._btnEditar_Click);
            // 
            // FormAnadirPunto
            // 
            this.AcceptButton = this._btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancelar;
            this.ClientSize = new System.Drawing.Size(431, 393);
            this.ControlBox = false;
            this.Controls.Add(this._btnEditar);
            this.Controls.Add(this.gbAnadirPunto);
            this.Controls.Add(this._btnCancelar);
            this.Controls.Add(this._btnAceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormAnadirPunto";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAnadirPunto";
            this.Load += new System.EventHandler(this.FormAnadirPunto_Load);
            this.Shown += new System.EventHandler(this.FormAnadirPunto_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormAnadirPunto_KeyDown);
            this.gbAnadirPunto.ResumeLayout(false);
            this.gbAnadirPunto.PerformLayout();
            this.ResumeLayout(false);

        }

        private Button _btnAceptar;

        internal Button btnAceptar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAceptar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAceptar != null)
                {
                    _btnAceptar.Click -= btnAceptar_Click;
                }

                _btnAceptar = value;
                if (_btnAceptar != null)
                {
                    _btnAceptar.Click += btnAceptar_Click;
                }
            }
        }

        private TextBox _txtClave;

        internal TextBox txtClave
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtClave;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtClave != null)
                {
                    _txtClave.TextChanged -= txtClave_TextChanged;
                }

                _txtClave = value;
                if (_txtClave != null)
                {
                    _txtClave.TextChanged += txtClave_TextChanged;
                }
            }
        }

        private TextBox _txtNombre;

        internal TextBox txtNombre
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtNombre;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtNombre != null)
                {
                    _txtNombre.TextChanged -= txtNombre_TextChanged;
                }

                _txtNombre = value;
                if (_txtNombre != null)
                {
                    _txtNombre.TextChanged += txtNombre_TextChanged;
                }
            }
        }

        private Button _btnCancelar;

        internal Button btnCancelar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCancelar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCancelar != null)
                {
                    _btnCancelar.Click -= btnCancelar_Click;
                }

                _btnCancelar = value;
                if (_btnCancelar != null)
                {
                    _btnCancelar.Click += btnCancelar_Click;
                }
            }
        }

        internal GroupBox gbAnadirPunto;
        internal Label lblDescrip;
        internal Label lblCodigo;
        internal ComboBox cmbPuntos;
        internal Label lblPunto;
        internal ComboBox cmbProyectos;
        internal Label lblProyecto;
        private TextBox _txtAbreviatura;
        private Label lblAbrev;
        private CheckBox _chkboxPersReg;
        private Label lblNombreRegimen;
        private TextBox _txtNombreReg;
        private Button _btnEditar;
    }
}