using System;

namespace IAHRIS
{
    public partial class FormAcercaDe
    {
        public FormAcercaDe()
        {
            InitializeComponent();
            _btnCerrar.Name = "btnCerrar";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}