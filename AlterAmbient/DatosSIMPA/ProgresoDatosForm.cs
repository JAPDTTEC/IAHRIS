using System;
using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class ProgresoDatosForm : Form
    {
        private MultiLangXML.MultiIdiomasXML _traductor;
        public ProgresoDatosForm()
        {
            InitializeComponent();
        }
        public void SeleccionarPasosBarraProgreso(int pasos)
        {
            progressBarDatos.Step = pasos;
        }
        public void EjecutarPasoBarraProgreso()
        {
            progressBarDatos.PerformStep();
        }
        public void FinalizarProgreso()
        {
            progressBarDatos.Value = 100;
        }
        public void Valor(int valor)
        {
            progressBarDatos.Value = valor;
        }
        public void Minimo(int valor)
        {
            progressBarDatos.Minimum = valor;
        }
        public void Maximo(int valor)
        {
            progressBarDatos.Maximum = valor;
        }
        public void ActualizarEtiquetaArchivo(string archivo)
        {
            lblArchivo.Text = archivo;
        }
        public void ActualizarEtiquetaDatos(string datos)
        {
            lblDatos.Text = datos;
        }
        public void ActualizarEtiquetaPorcentaje(string porcentaje)
        {
            lblPorcentaje.Text = porcentaje;
        }
        private void ProgresoDatosForm_Load(object sender, EventArgs e)
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
