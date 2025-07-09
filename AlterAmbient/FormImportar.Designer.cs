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
            this.lblSeleccion = new System.Windows.Forms.Label();
            this._btnExaminar = new System.Windows.Forms.Button();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.gbPuntos = new System.Windows.Forms.GroupBox();
            this.lblProyectoOrigen = new System.Windows.Forms.Label();
            this._btnCrearProy = new System.Windows.Forms.Button();
            this.cmbProyPuntos = new System.Windows.Forms.ComboBox();
            this.chklstPuntos = new System.Windows.Forms.CheckedListBox();
            this._btnImportarPuntos = new System.Windows.Forms.Button();
            this.gbProyectos = new System.Windows.Forms.GroupBox();
            this._btnImportarProyecto = new System.Windows.Forms.Button();
            this._cmbProyectos = new System.Windows.Forms.ComboBox();
            this.lblInformacion = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.ListBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbPuntos.SuspendLayout();
            this.gbProyectos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSeleccion
            // 
            this.lblSeleccion.AutoSize = true;
            this.lblSeleccion.Location = new System.Drawing.Point(9, 11);
            this.lblSeleccion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSeleccion.Name = "lblSeleccion";
            this.lblSeleccion.Size = new System.Drawing.Size(196, 13);
            this.lblSeleccion.TabIndex = 7;
            this.lblSeleccion.Text = "Seleccione la Base de Datos a importar:";
            // 
            // _btnExaminar
            // 
            this._btnExaminar.Location = new System.Drawing.Point(260, 51);
            this._btnExaminar.Name = "_btnExaminar";
            this._btnExaminar.Size = new System.Drawing.Size(95, 20);
            this._btnExaminar.TabIndex = 6;
            this._btnExaminar.Text = "Examinar...";
            this._btnExaminar.UseVisualStyleBackColor = true;
            this._btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(15, 29);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(340, 20);
            this.txtRuta.TabIndex = 5;
            // 
            // gbPuntos
            // 
            this.gbPuntos.Controls.Add(this.lblProyectoOrigen);
            this.gbPuntos.Controls.Add(this._btnCrearProy);
            this.gbPuntos.Controls.Add(this.cmbProyPuntos);
            this.gbPuntos.Controls.Add(this.chklstPuntos);
            this.gbPuntos.Controls.Add(this._btnImportarPuntos);
            this.gbPuntos.Location = new System.Drawing.Point(12, 128);
            this.gbPuntos.Name = "gbPuntos";
            this.gbPuntos.Size = new System.Drawing.Size(343, 170);
            this.gbPuntos.TabIndex = 8;
            this.gbPuntos.TabStop = false;
            this.gbPuntos.Text = "Importar Puntos a un Proyecto:";
            // 
            // lblProyectoOrigen
            // 
            this.lblProyectoOrigen.AutoSize = true;
            this.lblProyectoOrigen.Location = new System.Drawing.Point(4, 12);
            this.lblProyectoOrigen.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProyectoOrigen.Name = "lblProyectoOrigen";
            this.lblProyectoOrigen.Size = new System.Drawing.Size(125, 13);
            this.lblProyectoOrigen.TabIndex = 4;
            this.lblProyectoOrigen.Text = "Proyecto donde importar:";
            // 
            // _btnCrearProy
            // 
            this._btnCrearProy.Location = new System.Drawing.Point(6, 55);
            this._btnCrearProy.Name = "_btnCrearProy";
            this._btnCrearProy.Size = new System.Drawing.Size(119, 34);
            this._btnCrearProy.TabIndex = 3;
            this._btnCrearProy.Text = "Crear Proyecto Nuevo";
            this._btnCrearProy.UseVisualStyleBackColor = true;
            this._btnCrearProy.Click += new System.EventHandler(this.btnCrearProy_Click);
            // 
            // cmbProyPuntos
            // 
            this.cmbProyPuntos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProyPuntos.FormattingEnabled = true;
            this.cmbProyPuntos.Location = new System.Drawing.Point(6, 31);
            this.cmbProyPuntos.Name = "cmbProyPuntos";
            this.cmbProyPuntos.Size = new System.Drawing.Size(119, 21);
            this.cmbProyPuntos.TabIndex = 2;
            // 
            // chklstPuntos
            // 
            this.chklstPuntos.FormattingEnabled = true;
            this.chklstPuntos.Location = new System.Drawing.Point(135, 22);
            this.chklstPuntos.Name = "chklstPuntos";
            this.chklstPuntos.Size = new System.Drawing.Size(203, 139);
            this.chklstPuntos.TabIndex = 1;
            // 
            // _btnImportarPuntos
            // 
            this._btnImportarPuntos.Location = new System.Drawing.Point(6, 128);
            this._btnImportarPuntos.Name = "_btnImportarPuntos";
            this._btnImportarPuntos.Size = new System.Drawing.Size(119, 36);
            this._btnImportarPuntos.TabIndex = 0;
            this._btnImportarPuntos.Text = "Importar";
            this._btnImportarPuntos.UseVisualStyleBackColor = true;
            this._btnImportarPuntos.Click += new System.EventHandler(this.btnImportarPuntos_Click);
            // 
            // gbProyectos
            // 
            this.gbProyectos.Controls.Add(this._btnImportarProyecto);
            this.gbProyectos.Controls.Add(this._cmbProyectos);
            this.gbProyectos.Location = new System.Drawing.Point(12, 71);
            this.gbProyectos.Name = "gbProyectos";
            this.gbProyectos.Size = new System.Drawing.Size(343, 51);
            this.gbProyectos.TabIndex = 9;
            this.gbProyectos.TabStop = false;
            this.gbProyectos.Text = "Importar un Proyecto completo";
            // 
            // _btnImportarProyecto
            // 
            this._btnImportarProyecto.Location = new System.Drawing.Point(212, 19);
            this._btnImportarProyecto.Name = "_btnImportarProyecto";
            this._btnImportarProyecto.Size = new System.Drawing.Size(125, 21);
            this._btnImportarProyecto.TabIndex = 1;
            this._btnImportarProyecto.Text = "Importar";
            this._btnImportarProyecto.UseVisualStyleBackColor = true;
            this._btnImportarProyecto.Click += new System.EventHandler(this.btnImportarProyecto_Click);
            // 
            // _cmbProyectos
            // 
            this._cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbProyectos.FormattingEnabled = true;
            this._cmbProyectos.Location = new System.Drawing.Point(6, 19);
            this._cmbProyectos.Name = "_cmbProyectos";
            this._cmbProyectos.Size = new System.Drawing.Size(176, 21);
            this._cmbProyectos.TabIndex = 0;
            this._cmbProyectos.SelectedIndexChanged += new System.EventHandler(this.cmbProyectos_SelectedIndexChanged);
            // 
            // lblInformacion
            // 
            this.lblInformacion.AutoSize = true;
            this.lblInformacion.Location = new System.Drawing.Point(9, 315);
            this.lblInformacion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInformacion.Name = "lblInformacion";
            this.lblInformacion.Size = new System.Drawing.Size(68, 13);
            this.lblInformacion.TabIndex = 10;
            this.lblInformacion.Text = "Información: ";
            // 
            // lbInfo
            // 
            this.lbInfo.FormattingEnabled = true;
            this.lbInfo.HorizontalScrollbar = true;
            this.lbInfo.Location = new System.Drawing.Point(12, 331);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(343, 108);
            this.lbInfo.TabIndex = 11;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::IAHRIS.My.Resources.Resources.wait30trans;
            this.PictureBox1.Location = new System.Drawing.Point(173, 298);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(30, 30);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox1.TabIndex = 12;
            this.PictureBox1.TabStop = false;
            // 
            // FormImportar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 445);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.lblInformacion);
            this.Controls.Add(this.gbProyectos);
            this.Controls.Add(this.gbPuntos);
            this.Controls.Add(this.lblSeleccion);
            this.Controls.Add(this._btnExaminar);
            this.Controls.Add(this.txtRuta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
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