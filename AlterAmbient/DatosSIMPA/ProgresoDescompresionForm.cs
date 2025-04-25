using SharpCompress.Common;
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
        private MultiLangXML.MultiIdiomasXML _traductor;

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

        public void ActualizarCampos(int porcentaje)
        {
            long completed = 0;


           // var percentage = completed / totalSize;

            ActualizarEtiquetaPorcentaje((int)(porcentaje * 100) + "%");
            //ActualizarEtiquetaDescompresion((completed / 1024 / 1024).ToString() + "MB/" + ((int)(totalSize / 1024 / 1024)).ToString() + "MB");
            progressBarDesc.Maximum = 100;
            progressBarDesc.Value = (int)porcentaje;
            Update();
        }
        private void ProgresoDescompresionForm_Load(object sender, EventArgs e)
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
