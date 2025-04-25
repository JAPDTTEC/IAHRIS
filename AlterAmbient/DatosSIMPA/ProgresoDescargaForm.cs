using System;
using System.Net;
using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class ProgresoDescargaForm : Form
    {
        private MultiLangXML.MultiIdiomasXML _traductor;
        public ProgresoDescargaForm()
        {
            InitializeComponent();
        }
        public void SeleccionarPasosBarraProgreso(int pasos)
        {
            progressBarDownload.Step = pasos;
        }
        public void EjecutarPasoBarraProgreso()
        {
            progressBarDownload.PerformStep();
        }
        public void FinalizarProgreso()
        {
            progressBarDownload.Value = 100;
        }
        public void Valor(int valor)
        {
            progressBarDownload.Value = valor;
        }
        public void Minimo(int valor)
        {
            progressBarDownload.Minimum = valor;
        }
        public void Maximo(int valor)
        {
            progressBarDownload.Maximum = valor;
        }
        public void ActualizarEtiquetaArchivo(string archivo)
        {
            lblArchivo.Text = archivo;
        }
        public void ActualizarEtiquetaDescarga(string descarga)
        {
            lblDescarga.Text = descarga;
        }
        public void ActualizarEtiquetaPorcentaje(string porcentaje)
        {
            lblPorcentaje.Text = porcentaje;
        }

        public void eDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ActualizarEtiquetaPorcentaje(e.ProgressPercentage.ToString() + "%");
            ActualizarEtiquetaDescarga((e.BytesReceived / 1024 / 1024).ToString() + "MB/" + (e.TotalBytesToReceive / 1024 / 1024).ToString() + "MB");
            progressBarDownload.Maximum = (int)(e.TotalBytesToReceive / 1024);
            progressBarDownload.Value = (int)(e.BytesReceived / 1024);
        }
        private void ProgresoDescargaForm_Load(object sender, EventArgs e)
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
