using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    [DesignerGenerated()]
    public partial class FormImportar : Form
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
            lblSeleccion = new Label();
            _btnExaminar = new Button();
            _btnExaminar.Click += new EventHandler(btnExaminar_Click);
            txtRuta = new TextBox();
            gbPuntos = new GroupBox();
            lblProyectoOrigen = new Label();
            _btnCrearProy = new Button();
            _btnCrearProy.Click += new EventHandler(btnCrearProy_Click);
            cmbProyPuntos = new ComboBox();
            chklstPuntos = new CheckedListBox();
            _btnImportarPuntos = new Button();
            _btnImportarPuntos.Click += new EventHandler(btnImportarPuntos_Click);
            gbProyectos = new GroupBox();
            _btnImportarProyecto = new Button();
            _btnImportarProyecto.Click += new EventHandler(btnImportarProyecto_Click);
            _cmbProyectos = new ComboBox();
            _cmbProyectos.SelectedIndexChanged += new EventHandler(cmbProyectos_SelectedIndexChanged);
            lblInformacion = new Label();
            lbInfo = new ListBox();
            PictureBox1 = new PictureBox();
            gbPuntos.SuspendLayout();
            gbProyectos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblSeleccion
            // 
            lblSeleccion.AutoSize = true;
            lblSeleccion.Location = new Point(12, 13);
            lblSeleccion.Name = "lblSeleccion";
            lblSeleccion.Size = new Size(196, 13);
            lblSeleccion.TabIndex = 7;
            lblSeleccion.Text = "Seleccione la Base de Datos a importar:";
            // 
            // btnExaminar
            // 
            this._btnExaminar.Location = new System.Drawing.Point(347, 63);
            this._btnExaminar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnExaminar.Name = "_btnExaminar";
            this._btnExaminar.Size = new System.Drawing.Size(127, 25);
            this._btnExaminar.TabIndex = 6;
            this._btnExaminar.Text = "Examinar...";
            this._btnExaminar.UseVisualStyleBackColor = true;
            this._btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(20, 36);
            this.txtRuta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(452, 22);
            this.txtRuta.TabIndex = 5;
            // 
            // gbPuntos
            // 
            this.gbPuntos.Controls.Add(this.lblProyectoOrigen);
            this.gbPuntos.Controls.Add(this._btnCrearProy);
            this.gbPuntos.Controls.Add(this.cmbProyPuntos);
            this.gbPuntos.Controls.Add(this.chklstPuntos);
            this.gbPuntos.Controls.Add(this._btnImportarPuntos);
            this.gbPuntos.Location = new System.Drawing.Point(16, 158);
            this.gbPuntos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPuntos.Name = "gbPuntos";
            this.gbPuntos.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPuntos.Size = new System.Drawing.Size(457, 209);
            this.gbPuntos.TabIndex = 8;
            this.gbPuntos.TabStop = false;
            this.gbPuntos.Text = "Importar Puntos a un Proyecto:";
            // 
            // lblProyectoOrigen
            // 
            lblProyectoOrigen.AutoSize = true;
            lblProyectoOrigen.Location = new Point(6, 15);
            lblProyectoOrigen.Name = "lblProyectoOrigen";
            lblProyectoOrigen.Size = new Size(125, 13);
            lblProyectoOrigen.TabIndex = 4;
            lblProyectoOrigen.Text = "Proyecto donde importar:";
            // 
            // btnCrearProy
            // 
            this._btnCrearProy.Location = new System.Drawing.Point(8, 68);
            this._btnCrearProy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnCrearProy.Name = "_btnCrearProy";
            this._btnCrearProy.Size = new System.Drawing.Size(159, 42);
            this._btnCrearProy.TabIndex = 3;
            this._btnCrearProy.Text = "Crear Proyecto Nuevo";
            this._btnCrearProy.UseVisualStyleBackColor = true;
            this._btnCrearProy.Click += new System.EventHandler(this.btnCrearProy_Click);
            // 
            // cmbProyPuntos
            // 
            this.cmbProyPuntos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProyPuntos.FormattingEnabled = true;
            this.cmbProyPuntos.Location = new System.Drawing.Point(8, 38);
            this.cmbProyPuntos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbProyPuntos.Name = "cmbProyPuntos";
            this.cmbProyPuntos.Size = new System.Drawing.Size(157, 24);
            this.cmbProyPuntos.TabIndex = 2;
            // 
            // chklstPuntos
            // 
            this.chklstPuntos.FormattingEnabled = true;
            this.chklstPuntos.Location = new System.Drawing.Point(180, 27);
            this.chklstPuntos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chklstPuntos.Name = "chklstPuntos";
            this.chklstPuntos.Size = new System.Drawing.Size(269, 174);
            this.chklstPuntos.TabIndex = 1;
            // 
            // btnImportarPuntos
            // 
            this._btnImportarPuntos.Location = new System.Drawing.Point(8, 157);
            this._btnImportarPuntos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnImportarPuntos.Name = "_btnImportarPuntos";
            this._btnImportarPuntos.Size = new System.Drawing.Size(159, 44);
            this._btnImportarPuntos.TabIndex = 0;
            this._btnImportarPuntos.Text = "Importar";
            this._btnImportarPuntos.UseVisualStyleBackColor = true;
            this._btnImportarPuntos.Click += new System.EventHandler(this.btnImportarPuntos_Click);
            // 
            // gbProyectos
            // 
            this.gbProyectos.Controls.Add(this._btnImportarProyecto);
            this.gbProyectos.Controls.Add(this._cmbProyectos);
            this.gbProyectos.Location = new System.Drawing.Point(16, 87);
            this.gbProyectos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProyectos.Name = "gbProyectos";
            this.gbProyectos.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProyectos.Size = new System.Drawing.Size(457, 63);
            this.gbProyectos.TabIndex = 9;
            this.gbProyectos.TabStop = false;
            this.gbProyectos.Text = "Importar un Proyecto completo";
            // 
            // btnImportarProyecto
            // 
            this._btnImportarProyecto.Location = new System.Drawing.Point(283, 23);
            this._btnImportarProyecto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnImportarProyecto.Name = "_btnImportarProyecto";
            this._btnImportarProyecto.Size = new System.Drawing.Size(167, 26);
            this._btnImportarProyecto.TabIndex = 1;
            this._btnImportarProyecto.Text = "Importar";
            this._btnImportarProyecto.UseVisualStyleBackColor = true;
            this._btnImportarProyecto.Click += new System.EventHandler(this.btnImportarProyecto_Click);
            // 
            // cmbProyectos
            // 
            this._cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbProyectos.FormattingEnabled = true;
            this._cmbProyectos.Location = new System.Drawing.Point(8, 23);
            this._cmbProyectos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._cmbProyectos.Name = "_cmbProyectos";
            this._cmbProyectos.Size = new System.Drawing.Size(233, 24);
            this._cmbProyectos.TabIndex = 0;
            this._cmbProyectos.SelectedIndexChanged += new System.EventHandler(this.cmbProyectos_SelectedIndexChanged);
            // 
            // lblInformacion
            // 
            lblInformacion.AutoSize = true;
            lblInformacion.Location = new Point(12, 314);
            lblInformacion.Name = "lblInformacion";
            lblInformacion.Size = new Size(68, 13);
            lblInformacion.TabIndex = 10;
            lblInformacion.Text = "Información: ";
            // 
            // lbInfo
            // 
            this.lbInfo.FormattingEnabled = true;
            this.lbInfo.HorizontalScrollbar = true;
            this.lbInfo.ItemHeight = 16;
            this.lbInfo.Location = new System.Drawing.Point(16, 407);
            this.lbInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(456, 132);
            this.lbInfo.TabIndex = 11;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::IAHRIS.My.Resources.Resources.wait30trans;
            this.PictureBox1.Location = new System.Drawing.Point(231, 367);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(30, 30);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox1.TabIndex = 12;
            this.PictureBox1.TabStop = false;
            // 
            // FormImportar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 548);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.lblInformacion);
            this.Controls.Add(this.gbProyectos);
            this.Controls.Add(this.gbPuntos);
            this.Controls.Add(this.lblSeleccion);
            this.Controls.Add(this._btnExaminar);
            this.Controls.Add(this.txtRuta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormImportar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormImportar";
            this.Load += new System.EventHandler(this.FormImportar_Load);
            this.gbPuntos.ResumeLayout(false);
            this.gbPuntos.PerformLayout();
            this.gbProyectos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal Label lblSeleccion;
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

        internal TextBox txtRuta;
        internal GroupBox gbPuntos;
        internal CheckedListBox chklstPuntos;
        private Button _btnImportarPuntos;

        internal Button btnImportarPuntos
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnImportarPuntos;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnImportarPuntos != null)
                {
                    _btnImportarPuntos.Click -= btnImportarPuntos_Click;
                }

                _btnImportarPuntos = value;
                if (_btnImportarPuntos != null)
                {
                    _btnImportarPuntos.Click += btnImportarPuntos_Click;
                }
            }
        }

        internal GroupBox gbProyectos;
        private ComboBox _cmbProyectos;

        internal ComboBox cmbProyectos
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbProyectos;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbProyectos != null)
                {
                    _cmbProyectos.SelectedIndexChanged -= cmbProyectos_SelectedIndexChanged;
                }

                _cmbProyectos = value;
                if (_cmbProyectos != null)
                {
                    _cmbProyectos.SelectedIndexChanged += cmbProyectos_SelectedIndexChanged;
                }
            }
        }

        private Button _btnImportarProyecto;

        internal Button btnImportarProyecto
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnImportarProyecto;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnImportarProyecto != null)
                {
                    _btnImportarProyecto.Click -= btnImportarProyecto_Click;
                }

                _btnImportarProyecto = value;
                if (_btnImportarProyecto != null)
                {
                    _btnImportarProyecto.Click += btnImportarProyecto_Click;
                }
            }
        }

        internal Label lblInformacion;
        internal ListBox lbInfo;
        internal Label lblProyectoOrigen;
        private Button _btnCrearProy;

        internal Button btnCrearProy
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCrearProy;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCrearProy != null)
                {
                    _btnCrearProy.Click -= btnCrearProy_Click;
                }

                _btnCrearProy = value;
                if (_btnCrearProy != null)
                {
                    _btnCrearProy.Click += btnCrearProy_Click;
                }
            }
        }

        internal ComboBox cmbProyPuntos;
        internal PictureBox PictureBox1;
    }
}