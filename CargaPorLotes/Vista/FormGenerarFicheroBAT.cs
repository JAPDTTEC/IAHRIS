using CargaPorLotes.Logica;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CargaPorLotes.Vista
{
    public partial class FormGenerarFicheroBAT : Form
    {
        private string _rutaFichero;
        private string _rutaExportar;
        private List<ArchivoDatos> _datos;
        private GenerarFicherosBAT _generarFichero;
        private bool sobreescribirInformes = false;
        private MultiLangXML.MultiIdiomasXML _traductor;

        public FormGenerarFicheroBAT()
        {
            InitializeComponent();
        }

        public void setRutaFichero(string rutaFichero) 
        {
            _rutaFichero = rutaFichero;
            lblRutaFichero.Text = rutaFichero;
        }

        public void setDatosFichero(List<ArchivoDatos> datos)
        {
            _datos = datos;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                if (Procesar(false))
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfoCMFichero1"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCMFichero")  + ex.Message , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Procesar(true))
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfoCMFichero2"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCMFichero") + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Procesar(bool ejecutar)
        {
            try
            {
                string resultado = ValidarEjecutar();
                _generarFichero = new GenerarFicherosBAT(_rutaFichero, cbSobreescribir.Checked, cbExportar.Checked, _rutaExportar);

                if (!string.IsNullOrWhiteSpace(resultado))
                {
                    MessageBox.Show(resultado, "Info",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }                   



                _generarFichero.CrearFicheroBat(_datos, tbNomProyecto.Text, tbDescrProyecto.Text, cbCoeValoresHabituales.Checked, 
                                                cbCoeAvenidasSequias.Checked, cbLogs.Checked);


                if (ejecutar)
                {                
                    var procesoEjecutar = EjecutarFicheroBat.EjecutarAsync(_rutaFichero);
                    
                    DeshabilitarForm(false);

                    while (!procesoEjecutar.IsCompleted)
                    {
                        if (this.progressBar1.Value > 99)
                            this.progressBar1.Value = 0;

                        System.Threading.Thread.Sleep(250);
                        this.progressBar1.Value += 5;
                    }

                    DeshabilitarForm(true);
                    this.progressBar1.Value = 100;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;                  
        }

        private string ValidarEjecutar()
        {
            //Comprobar que el valor del nombre del proyecto
            if (string.IsNullOrEmpty(tbNomProyecto.Text))
                return _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataCM1");          

            if (tbNomProyecto.Text.Length > 20)            
                return _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataCM2"); 

            if (cbExportar.Checked && string.IsNullOrWhiteSpace(tbRutaExportar.Text))
                return _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataCM3");

            if (!cbExportar.Checked)
                _rutaExportar = "";

            return "";
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBr_RutaFichero.ShowDialog() == DialogResult.OK)
            {
                _rutaExportar = folderBr_RutaFichero.SelectedPath;
                this.tbRutaExportar.Text = _rutaExportar;
            }
        }

        private void tbRutaExportar_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbRutaExportar.Text))
            {
                cbCoeAvenidasSequias.Enabled = true;
                cbCoeValoresHabituales.Enabled = true;
                btnGuardar.Enabled = true;
                btnEjecutar.Enabled = true;
            }
        }


        private void cbExportar_CheckedChanged(object sender, EventArgs e)
        {
            this.tbRutaExportar.Enabled = cbExportar.Checked; 
            this.cbCoeAvenidasSequias.Enabled = cbExportar.Checked;
            this.cbCoeValoresHabituales.Enabled = cbExportar.Checked;
            this.btnExaminarRutaExp.Enabled = cbExportar.Checked;  
            this.cbSobreescribir.Enabled = cbExportar.Checked;
        }

        private void DeshabilitarForm (bool habilitar)
        {
            foreach (Control c in this.Controls)
                c.Enabled = habilitar;
        }

        private void FormGenerarFicheroBAT_Load(object sender, EventArgs e)
        {
            _traductor = MultiLangXML.MultiIdiomasXML.Instancia;
            _traductor.TraducirForm(this);
        }

        private void lblRutaFichero_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblRutaFichero.Text);
            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCopiadoPortapapeles"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
