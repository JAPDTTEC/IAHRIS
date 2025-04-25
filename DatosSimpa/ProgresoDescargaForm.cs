using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class ProgresoDescargaForm : Form
    {
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
    }
}
