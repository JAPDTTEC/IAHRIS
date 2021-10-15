using System;
using System.Diagnostics;
using System.Drawing;
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
            this.cmbPuntos = new System.Windows.Forms.ComboBox();
            this.lblPunto = new System.Windows.Forms.Label();
            this.lblDescrip = new System.Windows.Forms.Label();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.cmbProyectos = new System.Windows.Forms.ComboBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.gbAnadirPunto.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnAceptar
            // 
            this._btnAceptar.Location = new System.Drawing.Point(8, 176);
            this._btnAceptar.Name = "_btnAceptar";
            this._btnAceptar.Size = new System.Drawing.Size(116, 42);
            this._btnAceptar.TabIndex = 0;
            this._btnAceptar.Text = "Aceptar";
            this._btnAceptar.UseVisualStyleBackColor = true;
            this._btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // _txtClave
            // 
            this._txtClave.Location = new System.Drawing.Point(132, 22);
            this._txtClave.Name = "_txtClave";
            this._txtClave.Size = new System.Drawing.Size(164, 20);
            this._txtClave.TabIndex = 6;
            this._txtClave.TextChanged += new System.EventHandler(this.txtClave_TextChanged);
            // 
            // _txtNombre
            // 
            this._txtNombre.Location = new System.Drawing.Point(132, 45);
            this._txtNombre.Name = "_txtNombre";
            this._txtNombre.Size = new System.Drawing.Size(164, 20);
            this._txtNombre.TabIndex = 7;
            this._txtNombre.TextChanged += new System.EventHandler(this.txtNombre_TextChanged);
            // 
            // _btnCancelar
            // 
            this._btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancelar.Location = new System.Drawing.Point(201, 176);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(113, 42);
            this._btnCancelar.TabIndex = 8;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // gbAnadirPunto
            // 
            this.gbAnadirPunto.Controls.Add(this.cmbProyectos);
            this.gbAnadirPunto.Controls.Add(this.lblProyecto);
            this.gbAnadirPunto.Controls.Add(this.cmbPuntos);
            this.gbAnadirPunto.Controls.Add(this.lblPunto);
            this.gbAnadirPunto.Controls.Add(this.lblDescrip);
            this.gbAnadirPunto.Controls.Add(this.lblCodigo);
            this.gbAnadirPunto.Controls.Add(this._txtClave);
            this.gbAnadirPunto.Controls.Add(this._txtNombre);
            this.gbAnadirPunto.Location = new System.Drawing.Point(8, 12);
            this.gbAnadirPunto.Name = "gbAnadirPunto";
            this.gbAnadirPunto.Size = new System.Drawing.Size(306, 158);
            this.gbAnadirPunto.TabIndex = 10;
            this.gbAnadirPunto.TabStop = false;
            this.gbAnadirPunto.Text = "Datos del ";
            // 
            // cmbPuntos
            // 
            this.cmbPuntos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPuntos.FormattingEnabled = true;
            this.cmbPuntos.Location = new System.Drawing.Point(132, 121);
            this.cmbPuntos.Name = "cmbPuntos";
            this.cmbPuntos.Size = new System.Drawing.Size(164, 21);
            this.cmbPuntos.TabIndex = 11;
            // 
            // lblPunto
            // 
            this.lblPunto.AutoSize = true;
            this.lblPunto.Location = new System.Drawing.Point(6, 124);
            this.lblPunto.Name = "lblPunto";
            this.lblPunto.Size = new System.Drawing.Size(81, 13);
            this.lblPunto.TabIndex = 10;
            this.lblPunto.Text = "Punto asociado";
            // 
            // lblDescrip
            // 
            this.lblDescrip.AutoSize = true;
            this.lblDescrip.Location = new System.Drawing.Point(6, 48);
            this.lblDescrip.Name = "lblDescrip";
            this.lblDescrip.Size = new System.Drawing.Size(69, 13);
            this.lblDescrip.TabIndex = 9;
            this.lblDescrip.Text = "Descripción  ";
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(6, 25);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(46, 13);
            this.lblCodigo.TabIndex = 8;
            this.lblCodigo.Text = "Código  ";
            // 
            // cmbProyectos
            // 
            this.cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProyectos.FormattingEnabled = true;
            this.cmbProyectos.Location = new System.Drawing.Point(132, 89);
            this.cmbProyectos.Name = "cmbProyectos";
            this.cmbProyectos.Size = new System.Drawing.Size(164, 21);
            this.cmbProyectos.TabIndex = 13;
            this.cmbProyectos.SelectedIndexChanged += new System.EventHandler(this.cmbProyectos_SelectedIndexChanged);
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Location = new System.Drawing.Point(6, 92);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(95, 13);
            this.lblProyecto.TabIndex = 12;
            this.lblProyecto.Text = "Proyecto asociado";
            // 
            // FormAnadirPunto
            // 
            this.AcceptButton = this._btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancelar;
            this.ClientSize = new System.Drawing.Size(322, 224);
            this.ControlBox = false;
            this.Controls.Add(this.gbAnadirPunto);
            this.Controls.Add(this._btnCancelar);
            this.Controls.Add(this._btnAceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
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
    }
}