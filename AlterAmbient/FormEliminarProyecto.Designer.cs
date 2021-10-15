using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace IAHRIS
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FormEliminarProyecto : Form
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
            gbProyectos = new GroupBox();
            lblPuntos = new Label();
            lblPuntosAso = new Label();
            lblProyecto = new Label();
            _cbProyectos = new ComboBox();
            _cbProyectos.SelectedIndexChanged += new EventHandler(cbProyectos_SelectedIndexChanged);
            _btnAceptar = new Button();
            _btnAceptar.Click += new EventHandler(btnAceptar_Click);
            btnCancelar = new Button();
            gbProyectos.SuspendLayout();
            SuspendLayout();
            // 
            // gbProyectos
            // 
            gbProyectos.Controls.Add(lblPuntos);
            gbProyectos.Controls.Add(lblPuntosAso);
            gbProyectos.Controls.Add(lblProyecto);
            gbProyectos.Controls.Add(_cbProyectos);
            gbProyectos.Location = new Point(12, 12);
            gbProyectos.Name = "gbProyectos";
            gbProyectos.Size = new Size(289, 70);
            gbProyectos.TabIndex = 0;
            gbProyectos.TabStop = false;
            gbProyectos.Text = "GroupBox1";
            // 
            // lblPuntos
            // 
            lblPuntos.AutoSize = true;
            lblPuntos.Location = new Point(102, 48);
            lblPuntos.Name = "lblPuntos";
            lblPuntos.Size = new Size(13, 13);
            lblPuntos.TabIndex = 3;
            lblPuntos.Text = "0";
            // 
            // lblPuntosAso
            // 
            lblPuntosAso.AutoSize = true;
            lblPuntosAso.Location = new Point(6, 48);
            lblPuntosAso.Name = "lblPuntosAso";
            lblPuntosAso.Size = new Size(94, 13);
            lblPuntosAso.TabIndex = 2;
            lblPuntosAso.Text = "Puntos asociados:";
            // 
            // lblProyecto
            // 
            lblProyecto.AutoSize = true;
            lblProyecto.Location = new Point(6, 22);
            lblProyecto.Name = "lblProyecto";
            lblProyecto.Size = new Size(57, 13);
            lblProyecto.TabIndex = 1;
            lblProyecto.Text = "Proyectos:";
            // 
            // cbProyectos
            // 
            _cbProyectos.DropDownStyle = ComboBoxStyle.DropDownList;
            _cbProyectos.FormattingEnabled = true;
            _cbProyectos.Location = new Point(105, 19);
            _cbProyectos.Name = "_cbProyectos";
            _cbProyectos.Size = new Size(178, 21);
            _cbProyectos.TabIndex = 0;
            // 
            // btnAceptar
            // 
            _btnAceptar.Location = new Point(12, 88);
            _btnAceptar.Name = "_btnAceptar";
            _btnAceptar.Size = new Size(75, 23);
            _btnAceptar.TabIndex = 1;
            _btnAceptar.Text = "Aceptar";
            _btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(226, 88);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 23);
            btnCancelar.TabIndex = 2;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormEliminarProyecto
            // 
            AcceptButton = _btnAceptar;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(313, 113);
            Controls.Add(btnCancelar);
            Controls.Add(_btnAceptar);
            Controls.Add(gbProyectos);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormEliminarProyecto";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "FormEliminarProyecto";
            gbProyectos.ResumeLayout(false);
            gbProyectos.PerformLayout();
            ResumeLayout(false);
        }

        internal GroupBox gbProyectos;
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
        internal Label lblPuntos;
        internal Label lblPuntosAso;
        internal Label lblProyecto;
        private ComboBox _cbProyectos;

        internal ComboBox cbProyectos
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cbProyectos;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cbProyectos != null)
                {
                    _cbProyectos.SelectedIndexChanged -= cbProyectos_SelectedIndexChanged;
                }

                _cbProyectos = value;
                if (_cbProyectos != null)
                {
                    _cbProyectos.SelectedIndexChanged += cbProyectos_SelectedIndexChanged;
                }
            }
        }
    }
}