using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAHRIS
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FormEliminarLista : Form
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
            this._lstboxListas = new System.Windows.Forms.ListBox();
            this._btnAltDiaria = new System.Windows.Forms.Button();
            this._btnBorrarNatDiaria = new System.Windows.Forms.Button();
            this._btnBorrarNatMensual = new System.Windows.Forms.Button();
            this._cmboxAlt = new System.Windows.Forms.ComboBox();
            this._btnAltMensual = new System.Windows.Forms.Button();
            this.gbNaturales = new System.Windows.Forms.GroupBox();
            this.gbAlteradas = new System.Windows.Forms.GroupBox();
            this.lblSelecAlt = new System.Windows.Forms.Label();
            this.lblSeleccion = new System.Windows.Forms.Label();
            this.cmbProyectos = new System.Windows.Forms.ComboBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.gbNaturales.SuspendLayout();
            this.gbAlteradas.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lstboxListas
            // 
            this._lstboxListas.FormattingEnabled = true;
            this._lstboxListas.ItemHeight = 16;
            this._lstboxListas.Location = new System.Drawing.Point(16, 111);
            this._lstboxListas.Margin = new System.Windows.Forms.Padding(4);
            this._lstboxListas.Name = "_lstboxListas";
            this._lstboxListas.Size = new System.Drawing.Size(156, 260);
            this._lstboxListas.TabIndex = 0;
            this._lstboxListas.SelectedIndexChanged += new System.EventHandler(this.lboxListas_SelectedIndexChanged);
            // 
            // _btnAltDiaria
            // 
            this._btnAltDiaria.Location = new System.Drawing.Point(27, 82);
            this._btnAltDiaria.Margin = new System.Windows.Forms.Padding(4);
            this._btnAltDiaria.Name = "_btnAltDiaria";
            this._btnAltDiaria.Size = new System.Drawing.Size(176, 46);
            this._btnAltDiaria.TabIndex = 6;
            this._btnAltDiaria.Text = "Borrar Serie Alteración Diaria";
            this._btnAltDiaria.UseVisualStyleBackColor = true;
            this._btnAltDiaria.Click += new System.EventHandler(this.Button1_Click);
            // 
            // _btnBorrarNatDiaria
            // 
            this._btnBorrarNatDiaria.Location = new System.Drawing.Point(27, 33);
            this._btnBorrarNatDiaria.Margin = new System.Windows.Forms.Padding(4);
            this._btnBorrarNatDiaria.Name = "_btnBorrarNatDiaria";
            this._btnBorrarNatDiaria.Size = new System.Drawing.Size(176, 47);
            this._btnBorrarNatDiaria.TabIndex = 9;
            this._btnBorrarNatDiaria.Text = "Borrar Serie Natural Diaria";
            this._btnBorrarNatDiaria.UseVisualStyleBackColor = true;
            this._btnBorrarNatDiaria.Click += new System.EventHandler(this.btnBorrarNatDiaria_Click);
            // 
            // _btnBorrarNatMensual
            // 
            this._btnBorrarNatMensual.Location = new System.Drawing.Point(211, 33);
            this._btnBorrarNatMensual.Margin = new System.Windows.Forms.Padding(4);
            this._btnBorrarNatMensual.Name = "_btnBorrarNatMensual";
            this._btnBorrarNatMensual.Size = new System.Drawing.Size(176, 47);
            this._btnBorrarNatMensual.TabIndex = 10;
            this._btnBorrarNatMensual.Text = "Borrar Serie Natural Mensual";
            this._btnBorrarNatMensual.UseVisualStyleBackColor = true;
            this._btnBorrarNatMensual.Click += new System.EventHandler(this.btnBorrarNatMensual_Click);
            // 
            // _cmboxAlt
            // 
            this._cmboxAlt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmboxAlt.FormattingEnabled = true;
            this._cmboxAlt.Location = new System.Drawing.Point(211, 34);
            this._cmboxAlt.Margin = new System.Windows.Forms.Padding(4);
            this._cmboxAlt.Name = "_cmboxAlt";
            this._cmboxAlt.Size = new System.Drawing.Size(175, 24);
            this._cmboxAlt.TabIndex = 11;
            this._cmboxAlt.SelectedIndexChanged += new System.EventHandler(this.cmboxAlt_SelectedIndexChanged);
            // 
            // _btnAltMensual
            // 
            this._btnAltMensual.Location = new System.Drawing.Point(211, 82);
            this._btnAltMensual.Margin = new System.Windows.Forms.Padding(4);
            this._btnAltMensual.Name = "_btnAltMensual";
            this._btnAltMensual.Size = new System.Drawing.Size(176, 46);
            this._btnAltMensual.TabIndex = 12;
            this._btnAltMensual.Text = "Borrar Serie Alteración Mensual";
            this._btnAltMensual.UseVisualStyleBackColor = true;
            this._btnAltMensual.Click += new System.EventHandler(this.btnAltMensual_Click);
            // 
            // gbNaturales
            // 
            this.gbNaturales.Controls.Add(this._btnBorrarNatDiaria);
            this.gbNaturales.Controls.Add(this._btnBorrarNatMensual);
            this.gbNaturales.Location = new System.Drawing.Point(200, 95);
            this.gbNaturales.Margin = new System.Windows.Forms.Padding(4);
            this.gbNaturales.Name = "gbNaturales";
            this.gbNaturales.Padding = new System.Windows.Forms.Padding(4);
            this.gbNaturales.Size = new System.Drawing.Size(412, 100);
            this.gbNaturales.TabIndex = 13;
            this.gbNaturales.TabStop = false;
            this.gbNaturales.Text = "Series Naturales";
            // 
            // gbAlteradas
            // 
            this.gbAlteradas.Controls.Add(this.lblSelecAlt);
            this.gbAlteradas.Controls.Add(this._cmboxAlt);
            this.gbAlteradas.Controls.Add(this._btnAltMensual);
            this.gbAlteradas.Controls.Add(this._btnAltDiaria);
            this.gbAlteradas.Location = new System.Drawing.Point(200, 224);
            this.gbAlteradas.Margin = new System.Windows.Forms.Padding(4);
            this.gbAlteradas.Name = "gbAlteradas";
            this.gbAlteradas.Padding = new System.Windows.Forms.Padding(4);
            this.gbAlteradas.Size = new System.Drawing.Size(412, 148);
            this.gbAlteradas.TabIndex = 14;
            this.gbAlteradas.TabStop = false;
            this.gbAlteradas.Text = "Series Alteradas";
            // 
            // lblSelecAlt
            // 
            this.lblSelecAlt.AutoSize = true;
            this.lblSelecAlt.Location = new System.Drawing.Point(35, 38);
            this.lblSelecAlt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelecAlt.Name = "lblSelecAlt";
            this.lblSelecAlt.Size = new System.Drawing.Size(137, 16);
            this.lblSelecAlt.TabIndex = 13;
            this.lblSelecAlt.Text = "Seleccione alteración";
            // 
            // lblSeleccion
            // 
            this.lblSeleccion.Location = new System.Drawing.Point(16, 57);
            this.lblSeleccion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeleccion.Name = "lblSeleccion";
            this.lblSeleccion.Size = new System.Drawing.Size(157, 50);
            this.lblSeleccion.TabIndex = 15;
            this.lblSeleccion.Text = "Seleccione Punto al que eliminar series asociadas:";
            // 
            // cmbProyectos
            // 
            this.cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProyectos.FormattingEnabled = true;
            this.cmbProyectos.Location = new System.Drawing.Point(239, 7);
            this.cmbProyectos.Margin = new System.Windows.Forms.Padding(4);
            this.cmbProyectos.Name = "cmbProyectos";
            this.cmbProyectos.Size = new System.Drawing.Size(217, 24);
            this.cmbProyectos.TabIndex = 19;
            this.cmbProyectos.SelectedIndexChanged += new System.EventHandler(this.cmbProyectos_SelectedIndexChanged);
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Location = new System.Drawing.Point(16, 11);
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(121, 16);
            this.lblProyecto.TabIndex = 18;
            this.lblProyecto.Text = "Proyecto asociado";
            // 
            // FormEliminarLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 384);
            this.Controls.Add(this.cmbProyectos);
            this.Controls.Add(this.lblProyecto);
            this.Controls.Add(this.lblSeleccion);
            this.Controls.Add(this.gbAlteradas);
            this.Controls.Add(this.gbNaturales);
            this.Controls.Add(this._lstboxListas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormEliminarLista";
            this.ShowInTaskbar = false;
            this.Text = "Eliminar Serie";
            this.Load += new System.EventHandler(this.FormEliminarLista_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEliminarLista_KeyDown);
            this.gbNaturales.ResumeLayout(false);
            this.gbAlteradas.ResumeLayout(false);
            this.gbAlteradas.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ListBox _lstboxListas;

        internal ListBox lstboxListas
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lstboxListas;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lstboxListas != null)
                {
                    _lstboxListas.SelectedIndexChanged -= lboxListas_SelectedIndexChanged;
                }

                _lstboxListas = value;
                if (_lstboxListas != null)
                {
                    _lstboxListas.SelectedIndexChanged += lboxListas_SelectedIndexChanged;
                }
            }
        }

        private Button _btnAltDiaria;

        internal Button btnAltDiaria
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAltDiaria;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAltDiaria != null)
                {
                    _btnAltDiaria.Click -= Button1_Click;
                }

                _btnAltDiaria = value;
                if (_btnAltDiaria != null)
                {
                    _btnAltDiaria.Click += Button1_Click;
                }
            }
        }

        private Button _btnBorrarNatDiaria;

        internal Button btnBorrarNatDiaria
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnBorrarNatDiaria;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnBorrarNatDiaria != null)
                {
                    _btnBorrarNatDiaria.Click -= btnBorrarNatDiaria_Click;
                }

                _btnBorrarNatDiaria = value;
                if (_btnBorrarNatDiaria != null)
                {
                    _btnBorrarNatDiaria.Click += btnBorrarNatDiaria_Click;
                }
            }
        }

        private Button _btnBorrarNatMensual;

        internal Button btnBorrarNatMensual
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnBorrarNatMensual;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnBorrarNatMensual != null)
                {
                    _btnBorrarNatMensual.Click -= btnBorrarNatMensual_Click;
                }

                _btnBorrarNatMensual = value;
                if (_btnBorrarNatMensual != null)
                {
                    _btnBorrarNatMensual.Click += btnBorrarNatMensual_Click;
                }
            }
        }

        private ComboBox _cmboxAlt;

        internal ComboBox cmboxAlt
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmboxAlt;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmboxAlt != null)
                {
                    _cmboxAlt.SelectedIndexChanged -= cmboxAlt_SelectedIndexChanged;
                }

                _cmboxAlt = value;
                if (_cmboxAlt != null)
                {
                    _cmboxAlt.SelectedIndexChanged += cmboxAlt_SelectedIndexChanged;
                }
            }
        }

        private Button _btnAltMensual;

        internal Button btnAltMensual
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAltMensual;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAltMensual != null)
                {
                    _btnAltMensual.Click -= btnAltMensual_Click;
                }

                _btnAltMensual = value;
                if (_btnAltMensual != null)
                {
                    _btnAltMensual.Click += btnAltMensual_Click;
                }
            }
        }

        internal GroupBox gbNaturales;
        internal GroupBox gbAlteradas;
        internal Label lblSeleccion;
        internal Label lblSelecAlt;
        internal ComboBox cmbProyectos;
        internal Label lblProyecto;
    }
}