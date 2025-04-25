using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    [DesignerGenerated()]
    public partial class FormAcercaDe : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAcercaDe));
            _btnCerrar = new Button();
            _btnCerrar.Click += new EventHandler(btnCerrar_Click);
            SuspendLayout();
            // 
            // btnCerrar
            // 
            _btnCerrar.Font = new Font("Arial", 15.75f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
            _btnCerrar.Location = new Point(12, 460);
            _btnCerrar.Name = "_btnCerrar";
            _btnCerrar.Size = new Size(116, 92);
            _btnCerrar.TabIndex = 0;
            _btnCerrar.Text = "Volver";
            _btnCerrar.UseVisualStyleBackColor = true;
            // 
            // FormAcercaDe
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1060, 564);
            Controls.Add(_btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormAcercaDe";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }

        private Button _btnCerrar;

        internal Button btnCerrar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCerrar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCerrar != null)
                {
                    _btnCerrar.Click -= btnCerrar_Click;
                }

                _btnCerrar = value;
                if (_btnCerrar != null)
                {
                    _btnCerrar.Click += btnCerrar_Click;
                }
            }
        }
    }
}