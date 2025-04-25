using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAHRIS
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FormEliminarPunto : Form
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
            this._lstboxPuntos = new System.Windows.Forms.ListBox();
            this._lstboxAlt = new System.Windows.Forms.ListBox();
            this._btnBorrar = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblPunto = new System.Windows.Forms.Label();
            this.lblAlteracion = new System.Windows.Forms.Label();
            this.cmbProyectos = new System.Windows.Forms.ComboBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _lstboxPuntos
            // 
            this._lstboxPuntos.FormattingEnabled = true;
            this._lstboxPuntos.ItemHeight = 16;
            this._lstboxPuntos.Location = new System.Drawing.Point(16, 85);
            this._lstboxPuntos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._lstboxPuntos.Name = "_lstboxPuntos";
            this._lstboxPuntos.Size = new System.Drawing.Size(95, 199);
            this._lstboxPuntos.TabIndex = 0;
            this._lstboxPuntos.SelectedIndexChanged += new System.EventHandler(this.lstboxPuntos_SelectedIndexChanged);
            // 
            // _lstboxAlt
            // 
            this._lstboxAlt.FormattingEnabled = true;
            this._lstboxAlt.ItemHeight = 16;
            this._lstboxAlt.Location = new System.Drawing.Point(201, 85);
            this._lstboxAlt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._lstboxAlt.Name = "_lstboxAlt";
            this._lstboxAlt.Size = new System.Drawing.Size(96, 199);
            this._lstboxAlt.TabIndex = 1;
            this._lstboxAlt.SelectedIndexChanged += new System.EventHandler(this.lstboxAlt_SelectedIndexChanged);
            // 
            // _btnBorrar
            // 
            this._btnBorrar.Location = new System.Drawing.Point(202, 340);
            this._btnBorrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnBorrar.Name = "_btnBorrar";
            this._btnBorrar.Size = new System.Drawing.Size(128, 42);
            this._btnBorrar.TabIndex = 2;
            this._btnBorrar.Text = "Borrar";
            this._btnBorrar.UseVisualStyleBackColor = true;
            this._btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(162, 186);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(16, 13);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "->";
            // 
            // lblPunto
            // 
            this.lblPunto.AutoSize = true;
            this.lblPunto.Location = new System.Drawing.Point(9, 47);
            this.lblPunto.Name = "lblPunto";
            this.lblPunto.Size = new System.Drawing.Size(41, 16);
            this.lblPunto.TabIndex = 4;
            this.lblPunto.Text = "_____Punto_____";
            // 
            // lblAlteracion
            // 
            this.lblAlteracion.AutoSize = true;
            this.lblAlteracion.Location = new System.Drawing.Point(199, 58);
            this.lblAlteracion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlteracion.Name = "lblAlteracion";
            this.lblAlteracion.Size = new System.Drawing.Size(67, 16);
            this.lblAlteracion.TabIndex = 5;
            this.lblAlteracion.Text = "Alteración";
            this.lblAlteracion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmbProyectos
            // 
            this.cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProyectos.FormattingEnabled = true;
            this.cmbProyectos.Location = new System.Drawing.Point(85, 15);
            this.cmbProyectos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbProyectos.Name = "cmbProyectos";
            this.cmbProyectos.Size = new System.Drawing.Size(245, 24);
            this.cmbProyectos.TabIndex = 19;
            this.cmbProyectos.SelectedIndexChanged += new System.EventHandler(this.cmbProyectos_SelectedIndexChanged);
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Location = new System.Drawing.Point(12, 18);
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(61, 16);
            this.lblProyecto.TabIndex = 18;
            this.lblProyecto.Text = "Proyecto";
            // 
            // FormEliminarPunto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 393);
            this.Controls.Add(this.cmbProyectos);
            this.Controls.Add(this.lblProyecto);
            this.Controls.Add(this.lblAlteracion);
            this.Controls.Add(this.lblPunto);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this._btnBorrar);
            this.Controls.Add(this._lstboxAlt);
            this.Controls.Add(this._lstboxPuntos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormEliminarPunto";
            this.ShowInTaskbar = false;
            this.Text = "FormEliminarAlteracion";
            this.Load += new System.EventHandler(this.FormEliminarAlteracion_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEliminarAlteracion_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ListBox _lstboxPuntos;

        internal ListBox lstboxPuntos
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lstboxPuntos;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lstboxPuntos != null)
                {
                    _lstboxPuntos.SelectedIndexChanged -= lstboxPuntos_SelectedIndexChanged;
                }

                _lstboxPuntos = value;
                if (_lstboxPuntos != null)
                {
                    _lstboxPuntos.SelectedIndexChanged += lstboxPuntos_SelectedIndexChanged;
                }
            }
        }

        private ListBox _lstboxAlt;

        internal ListBox lstboxAlt
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lstboxAlt;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lstboxAlt != null)
                {
                    _lstboxAlt.SelectedIndexChanged -= lstboxAlt_SelectedIndexChanged;
                }

                _lstboxAlt = value;
                if (_lstboxAlt != null)
                {
                    _lstboxAlt.SelectedIndexChanged += lstboxAlt_SelectedIndexChanged;
                }
            }
        }

        private Button _btnBorrar;

        internal Button btnBorrar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnBorrar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnBorrar != null)
                {
                    _btnBorrar.Click -= btnBorrar_Click;
                }

                _btnBorrar = value;
                if (_btnBorrar != null)
                {
                    _btnBorrar.Click += btnBorrar_Click;
                }
            }
        }

        internal Label Label1;
        internal Label lblPunto;
        internal Label lblAlteracion;
        internal ComboBox cmbProyectos;
        internal Label lblProyecto;
    }
}