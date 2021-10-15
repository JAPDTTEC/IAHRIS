using IAHRIS.BBDD;
using System;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormAnadirProyecto
    {
        public FormAnadirProyecto()
        {
            InitializeComponent();
            _btnAceptar.Name = "btnAceptar";
        }

        public FormAnadirProyecto(OleDbDataBase MDB)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            _cMDB = MDB;

            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _btnAceptar.Name = "btnAceptar";
        }

        private BBDD.OleDbDataBase _cMDB;
        private MultiLangXML.MultiIdiomasXML _traductor;

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombre.Text) & !string.IsNullOrEmpty(txtDescripcion.Text))
            {
                if (txtNombre.Text.Length > 20)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongName"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // MessageBox.Show("El campo NOMBRE no puede ser mayor de 20 caracteres", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    return;
                }

                var valores = new[] { txtNombre.Text, txtDescripcion.Text };
                var campos = new[] { "nombre", "descripcion" };
                if (_cMDB.InsertarRegistro("Proyecto", campos, valores))
                {
                    Dispose();
                }
                else
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertProj"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
        }
    }
}