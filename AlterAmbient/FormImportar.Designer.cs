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
            _btnExaminar.Location = new Point(260, 51);
            _btnExaminar.Name = "_btnExaminar";
            _btnExaminar.Size = new Size(95, 20);
            _btnExaminar.TabIndex = 6;
            _btnExaminar.Text = "Examinar...";
            _btnExaminar.UseVisualStyleBackColor = true;
            // 
            // txtRuta
            // 
            txtRuta.Location = new Point(15, 29);
            txtRuta.Name = "txtRuta";
            txtRuta.ReadOnly = true;
            txtRuta.Size = new Size(340, 20);
            txtRuta.TabIndex = 5;
            // 
            // gbPuntos
            // 
            gbPuntos.Controls.Add(lblProyectoOrigen);
            gbPuntos.Controls.Add(_btnCrearProy);
            gbPuntos.Controls.Add(cmbProyPuntos);
            gbPuntos.Controls.Add(chklstPuntos);
            gbPuntos.Controls.Add(_btnImportarPuntos);
            gbPuntos.Location = new Point(12, 128);
            gbPuntos.Name = "gbPuntos";
            gbPuntos.Size = new Size(343, 170);
            gbPuntos.TabIndex = 8;
            gbPuntos.TabStop = false;
            gbPuntos.Text = "Importar Puntos a un Proyecto:";
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
            _btnCrearProy.Location = new Point(6, 55);
            _btnCrearProy.Name = "_btnCrearProy";
            _btnCrearProy.Size = new Size(119, 34);
            _btnCrearProy.TabIndex = 3;
            _btnCrearProy.Text = "Crear Proyecto Nuevo";
            _btnCrearProy.UseVisualStyleBackColor = true;
            // 
            // cmbProyPuntos
            // 
            cmbProyPuntos.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProyPuntos.FormattingEnabled = true;
            cmbProyPuntos.Location = new Point(6, 31);
            cmbProyPuntos.Name = "cmbProyPuntos";
            cmbProyPuntos.Size = new Size(119, 21);
            cmbProyPuntos.TabIndex = 2;
            // 
            // chklstPuntos
            // 
            chklstPuntos.FormattingEnabled = true;
            chklstPuntos.Location = new Point(131, 22);
            chklstPuntos.Name = "chklstPuntos";
            chklstPuntos.Size = new Size(203, 139);
            chklstPuntos.TabIndex = 1;
            // 
            // btnImportarPuntos
            // 
            _btnImportarPuntos.Location = new Point(6, 124);
            _btnImportarPuntos.Name = "_btnImportarPuntos";
            _btnImportarPuntos.Size = new Size(119, 36);
            _btnImportarPuntos.TabIndex = 0;
            _btnImportarPuntos.Text = "Importar";
            _btnImportarPuntos.UseVisualStyleBackColor = true;
            // 
            // gbProyectos
            // 
            gbProyectos.Controls.Add(_btnImportarProyecto);
            gbProyectos.Controls.Add(_cmbProyectos);
            gbProyectos.Location = new Point(12, 71);
            gbProyectos.Name = "gbProyectos";
            gbProyectos.Size = new Size(343, 51);
            gbProyectos.TabIndex = 9;
            gbProyectos.TabStop = false;
            gbProyectos.Text = "Importar un Proyecto completo";
            // 
            // btnImportarProyecto
            // 
            _btnImportarProyecto.Location = new Point(212, 19);
            _btnImportarProyecto.Name = "_btnImportarProyecto";
            _btnImportarProyecto.Size = new Size(125, 21);
            _btnImportarProyecto.TabIndex = 1;
            _btnImportarProyecto.Text = "Importar";
            _btnImportarProyecto.UseVisualStyleBackColor = true;
            // 
            // cmbProyectos
            // 
            _cmbProyectos.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbProyectos.FormattingEnabled = true;
            _cmbProyectos.Location = new Point(6, 19);
            _cmbProyectos.Name = "_cmbProyectos";
            _cmbProyectos.Size = new Size(176, 21);
            _cmbProyectos.TabIndex = 0;
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
            lbInfo.FormattingEnabled = true;
            lbInfo.HorizontalScrollbar = true;
            lbInfo.Location = new Point(12, 331);
            lbInfo.Name = "lbInfo";
            lbInfo.Size = new Size(343, 108);
            lbInfo.TabIndex = 11;
            // 
            // PictureBox1
            // 
            PictureBox1.Image = My.Resources.Resources.wait30trans;
            PictureBox1.Location = new Point(173, 298);
            PictureBox1.Name = "PictureBox1";
            PictureBox1.Size = new Size(30, 30);
            PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            PictureBox1.TabIndex = 12;
            PictureBox1.TabStop = false;
            // 
            // FormImportar
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(368, 445);
            Controls.Add(PictureBox1);
            Controls.Add(lbInfo);
            Controls.Add(lblInformacion);
            Controls.Add(gbProyectos);
            Controls.Add(gbPuntos);
            Controls.Add(lblSeleccion);
            Controls.Add(_btnExaminar);
            Controls.Add(txtRuta);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormImportar";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormImportar";
            gbPuntos.ResumeLayout(false);
            gbPuntos.PerformLayout();
            gbProyectos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
            Load += new EventHandler(FormImportar_Load);
            ResumeLayout(false);
            PerformLayout();
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