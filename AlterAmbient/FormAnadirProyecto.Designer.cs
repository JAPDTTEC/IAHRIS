using System.Diagnostics;
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
            this.gbProyecto = new System.Windows.Forms.GroupBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this._btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this._btnEditar = new System.Windows.Forms.Button();
            this.gbProyecto.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbProyecto
            // 
            this.gbProyecto.Controls.Add(this.lblDescripcion);
            this.gbProyecto.Controls.Add(this.lblNombre);
            this.gbProyecto.Controls.Add(this.txtDescripcion);
            this.gbProyecto.Controls.Add(this.txtNombre);
            this.gbProyecto.Location = new System.Drawing.Point(16, 15);
            this.gbProyecto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProyecto.Name = "gbProyecto";
            this.gbProyecto.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbProyecto.Size = new System.Drawing.Size(316, 186);
            this.gbProyecto.TabIndex = 0;
            this.gbProyecto.TabStop = false;
            this.gbProyecto.Text = "Proyecto";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(6, 46);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(63, 13);
            this.lblDescripcion.TabIndex = 3;
            this.lblDescripcion.Text = "Descripción";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(6, 19);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(134, 53);
            this.txtDescripcion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(128, 94);
            this.txtDescripcion.TabIndex = 1;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(134, 20);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(128, 20);
            this.txtNombre.TabIndex = 0;
            // 
            // _btnAceptar
            // 
            this._btnAceptar.Location = new System.Drawing.Point(16, 208);
            this._btnAceptar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnAceptar.Name = "_btnAceptar";
            this._btnAceptar.Size = new System.Drawing.Size(75, 47);
            this._btnAceptar.TabIndex = 1;
            this._btnAceptar.Text = "Añadir";
            this._btnAceptar.UseVisualStyleBackColor = true;
            this._btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(232, 208);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 47);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // _btnEditar
            // 
            this._btnEditar.Location = new System.Drawing.Point(124, 208);
            this._btnEditar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnEditar.Name = "_btnEditar";
            this._btnEditar.Size = new System.Drawing.Size(75, 47);
            this._btnEditar.TabIndex = 3;
            this._btnEditar.Text = "Editar";
            this._btnEditar.UseVisualStyleBackColor = true;
            this._btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // FormAnadirProyecto
            // 
            this.AcceptButton = this._btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(340, 270);
            this.ControlBox = false;
            this.Controls.Add(this._btnEditar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this._btnAceptar);
            this.Controls.Add(this.gbProyecto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormAnadirProyecto";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormAnadirProyecto";
            this.gbProyecto.ResumeLayout(false);
            this.gbProyecto.PerformLayout();
            this.ResumeLayout(false);

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
        private Button _btnEditar;
    }
}