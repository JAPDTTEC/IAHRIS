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
            this.gbProyectos = new System.Windows.Forms.GroupBox();
            this.lblPuntos = new System.Windows.Forms.Label();
            this.lblPuntosAso = new System.Windows.Forms.Label();
            this.lblProyecto = new System.Windows.Forms.Label();
            this._cbProyectos = new System.Windows.Forms.ComboBox();
            this._btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gbProyectos.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbProyectos
            // 
            this.gbProyectos.Controls.Add(this.lblPuntos);
            this.gbProyectos.Controls.Add(this.lblPuntosAso);
            this.gbProyectos.Controls.Add(this.lblProyecto);
            this.gbProyectos.Controls.Add(this._cbProyectos);
            this.gbProyectos.Location = new System.Drawing.Point(16, 15);
            this.gbProyectos.Margin = new System.Windows.Forms.Padding(4);
            this.gbProyectos.Name = "gbProyectos";
            this.gbProyectos.Padding = new System.Windows.Forms.Padding(4);
            this.gbProyectos.Size = new System.Drawing.Size(385, 86);
            this.gbProyectos.TabIndex = 0;
            this.gbProyectos.TabStop = false;
            this.gbProyectos.Text = "GroupBox1";
            // 
            // lblPuntos
            // 
            this.lblPuntos.AutoSize = true;
            this.lblPuntos.Location = new System.Drawing.Point(136, 59);
            this.lblPuntos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPuntos.Name = "lblPuntos";
            this.lblPuntos.Size = new System.Drawing.Size(14, 16);
            this.lblPuntos.TabIndex = 3;
            this.lblPuntos.Text = "0";
            // 
            // lblPuntosAso
            // 
            this.lblPuntosAso.AutoSize = true;
            this.lblPuntosAso.Location = new System.Drawing.Point(8, 59);
            this.lblPuntosAso.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPuntosAso.Name = "lblPuntosAso";
            this.lblPuntosAso.Size = new System.Drawing.Size(118, 16);
            this.lblPuntosAso.TabIndex = 2;
            this.lblPuntosAso.Text = "Puntos asociados:";
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Location = new System.Drawing.Point(8, 27);
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(71, 16);
            this.lblProyecto.TabIndex = 1;
            this.lblProyecto.Text = "Proyectos:";
            // 
            // _cbProyectos
            // 
            this._cbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbProyectos.FormattingEnabled = true;
            this._cbProyectos.Location = new System.Drawing.Point(140, 23);
            this._cbProyectos.Margin = new System.Windows.Forms.Padding(4);
            this._cbProyectos.Name = "_cbProyectos";
            this._cbProyectos.Size = new System.Drawing.Size(236, 24);
            this._cbProyectos.TabIndex = 0;
            this._cbProyectos.SelectedIndexChanged += new System.EventHandler(this.cbProyectos_SelectedIndexChanged);
            // 
            // _btnAceptar
            // 
            this._btnAceptar.Location = new System.Drawing.Point(301, 108);
            this._btnAceptar.Margin = new System.Windows.Forms.Padding(4);
            this._btnAceptar.Name = "_btnAceptar";
            this._btnAceptar.Size = new System.Drawing.Size(100, 28);
            this._btnAceptar.TabIndex = 1;
            this._btnAceptar.Text = "Aceptar";
            this._btnAceptar.UseVisualStyleBackColor = true;
            this._btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(16, 108);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 28);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormEliminarProyecto
            // 
            this.AcceptButton = this._btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(417, 147);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this._btnAceptar);
            this.Controls.Add(this.gbProyectos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormEliminarProyecto";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormEliminarProyecto";
            this.gbProyectos.ResumeLayout(false);
            this.gbProyectos.PerformLayout();
            this.ResumeLayout(false);

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