using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class ProgresoDatosForm : Form
    {
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
    }
}
