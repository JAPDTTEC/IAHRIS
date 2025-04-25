using System;
using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class Form1 : Form
    {
        private MultiLangXML.MultiIdiomasXML _traductor;
        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRecuperarDatos_Click(object sender, EventArgs e)
        {
            //Validar Datos
            string respuesta = ValidarDatos();

            if(!string.IsNullOrEmpty(respuesta))
                MessageBox.Show(respuesta, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                this.DialogResult = DialogResult.OK;
        }

        
        private string ValidarDatos()
        {
            //Validar Datos             
            if (string.IsNullOrEmpty(this.tbCodigoPunto.Text))
                return _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataSIMPA1");

            if (string.IsNullOrEmpty(this.tbProyecto.Text))
                return _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataSIMPA2");

            if (this.tbProyecto.Text.Length > 20)
                return _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataSIMPA3");

            return "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");

        }
    }
}
