using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class ProgresoDescompresionForm : Form
    {
        public ProgresoDescompresionForm()
        {
            InitializeComponent();
        }
        public void SeleccionarPasosBarraProgreso(int pasos)
        {
            progressBarDesc.Step = pasos;
        }
        public void EjecutarPasoBarraProgreso()
        {
            progressBarDesc.PerformStep();
        }
        public void FinalizarProgreso()
        {
            progressBarDesc.Value = 100;
        }
        public void Valor(int valor)
        {
            progressBarDesc.Value = valor;
        }
        public void Minimo(int valor)
        {
            progressBarDesc.Minimum = valor;
        }
        public void Maximo(int valor)
        {
            progressBarDesc.Maximum = valor;
        }
        public void ActualizarEtiquetaArchivo(string archivo)
        {
            lblArchivo.Text = archivo;
        }
        public void ActualizarEtiquetaDescompresion(string descarga)
        {
            lblDescompresion.Text = descarga;
        }
        public void ActualizarEtiquetaPorcentaje(string porcentaje)
        {
            lblPorcentaje.Text = porcentaje;
        }
    }
}
