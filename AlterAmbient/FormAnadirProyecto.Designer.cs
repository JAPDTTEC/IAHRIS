using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAHRIS
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FormAnadirProyecto : Form
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
            gbProyecto = new GroupBox();
            lblDescripcion = new Label();
            lblNombre = new Label();
            txtDescripcion = new TextBox();
            txtNombre = new TextBox();
            _btnAceptar = new Button();
            _btnAceptar.Click += new EventHandler(btnAceptar_Click);
            btnCancelar = new Button();
            gbProyecto.SuspendLayout();
            SuspendLayout();
            // 
            // gbProyecto
            // 
            gbProyecto.Controls.Add(lblDescripcion);
            gbProyecto.Controls.Add(lblNombre);
            gbProyecto.Controls.Add(txtDescripcion);
            gbProyecto.Controls.Add(txtNombre);
            gbProyecto.Location = new Point(12, 12);
            gbProyecto.Name = "gbProyecto";
            gbProyecto.Size = new Size(237, 100);
            gbProyecto.TabIndex = 0;
            gbProyecto.TabStop = false;
            gbProyecto.Text = "Proyecto";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(6, 46);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(63, 13);
            lblDescripcion.TabIndex = 3;
            lblDescripcion.Text = "Descripción";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(6, 19);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(44, 13);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(101, 43);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(128, 38);
            txtDescripcion.TabIndex = 1;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(101, 16);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(128, 20);
            txtNombre.TabIndex = 0;
            // 
            // btnAceptar
            // 
            _btnAceptar.Location = new Point(12, 118);
            _btnAceptar.Name = "_btnAceptar";
            _btnAceptar.Size = new Size(75, 47);
            _btnAceptar.TabIndex = 1;
            _btnAceptar.Text = "Aceptar";
            _btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(174, 118);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 47);
            btnCancelar.TabIndex = 2;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormAnadirProyecto
            // 
            AcceptButton = _btnAceptar;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(247, 164);
            ControlBox = false;
            Controls.Add(btnCancelar);
            Controls.Add(_btnAceptar);
            Controls.Add(gbProyecto);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FormAnadirProyecto";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "FormAnadirProyecto";
            gbProyecto.ResumeLayout(false);
            gbProyecto.PerformLayout();
            ResumeLayout(false);
        }

        internal GroupBox gbProyecto;
        internal Label lblDescripcion;
        internal Label lblNombre;
        internal TextBox txtDescripcion;
        internal TextBox txtNombre;
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

        internal Button btnCancelar;
    }
}