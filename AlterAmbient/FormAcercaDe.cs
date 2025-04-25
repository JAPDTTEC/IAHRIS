using System;

namespace IAHRIS
{
    /// <summary>
    /// Formulario Acerca De
    /// </summary>
    public partial class FormAcercaDe
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FormAcercaDe()
        {
           // System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-ES");

            InitializeComponent();
            _btnCerrar.Name = "btnCerrar";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}