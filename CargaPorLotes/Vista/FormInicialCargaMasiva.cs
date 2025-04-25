using CargaPorLotes.Logica;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;

namespace CargaPorLotes.Vista
{
    public partial class FormInicialCargaMasiva : Form
    {
        private readonly CargarFicherosCSV _cargarFicherosCSV;
        private List<ArchivoDatos> datosFichero;
        private MultiLangXML.MultiIdiomasXML _traductor;

        public FormInicialCargaMasiva()
        {
            _cargarFicherosCSV = new CargarFicherosCSV();
            InitializeComponent();
        }

        private void btnCargarDir_Click(object sender, EventArgs e)
        {
            string rutaDir = textBox1.Text;
            string[] titulo;
            ArchivoDatos punto = new ArchivoDatos();
            ArchivoDatos alteracion = new ArchivoDatos();
            datosFichero = new List<ArchivoDatos>();
            List<ArchivoDatos> ficheroNoProcesados = new List<ArchivoDatos>();
            int ficherosTotales = 0, puntosTotales = 0, altTotales = 0;
            string[] ficheros;


            LimpiarGridView();

            try
            {
                
                
                //Validar Ruta
                if (!Directory.Exists(rutaDir))
                    MessageBox.Show(rutaDir + " " +_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strHasntBeenFound"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK);

                //Obtener ficheros
                SearchOption searchOption = SearchOption.TopDirectoryOnly;
                if (this.cbSubDirectorios.Checked)
                    searchOption = SearchOption.AllDirectories;

                ficheros = Directory.GetFiles(rutaDir, "*.csv", searchOption);
                ficherosTotales = ficheros.Length;

                //obtener puntos Primer Barrido.
                foreach (string fich in ficheros)
                {
                    using (StreamReader sr = new StreamReader(fich))
                    {
                        try
                        {
                            titulo = sr.ReadLine().Split(';');

                            string resultado = ValidarFichero(titulo, fich);
                            if (!string.IsNullOrEmpty(resultado))
                                ficheroNoProcesados.Add(ArchivoDatos.GetFicheroNoProcesado(fich, resultado));

                            if (titulo.Length == 3)
                            {
                                punto = _cargarFicherosCSV.ObtenerPuntos(fich);
                                punto.Procesado = true;
                                datosFichero.Add(punto);
                                puntosTotales++;
                            }
                        }
                        catch (Exception ex)
                        {
                            punto = ArchivoDatos.GetFicheroNoProcesado(fich, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError") + ": " + ex.Message);
                            ficheroNoProcesados.Add(punto);
                            continue;
                        }
                    }
                }

                //obtener alteracion segundo barrido.
                foreach (string fich in ficheros)
                {
                    using (StreamReader sr = new StreamReader(fich))
                    {
                        ArchivoDatos puntoConAlt = new ArchivoDatos();
                        try
                        {
                            titulo = sr.ReadLine().Split(';');

                            if (titulo.Length == 4)
                            {
                                alteracion = _cargarFicherosCSV.ObtenerAlteracion(fich);
                                ArchivoDatos puntoAlt = datosFichero.Find(x => x.NombrePunto.Equals(titulo[2]));

                                //Creamos un nuevo punto sin ruta si no existe,
                                //si existe, eliminamos el indice de la lista para insertarlo despues actualizado.
                                if (puntoAlt == null)
                                    puntoAlt = ArchivoDatos.GetPunto("", titulo[2], "Punto Carga Masiva");
                                else
                                    datosFichero.Remove(puntoAlt);

                                puntoConAlt = _cargarFicherosCSV.AñadirAlteracion(puntoAlt, alteracion);
                                puntoConAlt.Procesado = true;
                                datosFichero.Add(puntoConAlt);
                                altTotales++;

                            }
                        }
                        catch (Exception ex)
                        {
                            alteracion = ArchivoDatos.GetFicheroNoProcesado(fich, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError")+": " + ex.Message);
                            ficheroNoProcesados.Add(alteracion);
                            continue;
                        }
                    }
                }

                //Cargar Resumen Inferior
                this.lblResFicherosProc.Text = ficherosTotales.ToString();
                this.lblResPuntosProc.Text = puntosTotales.ToString();
                this.lblResAltProc.Text = altTotales.ToString();
                this.lblResErroneos.Text = ficheroNoProcesados.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorLoadingFiles") + " " + ex.Message);
                btnGenerarBat.Enabled = false;
                btnGuardarReporte.Enabled = false;
                return;
            }

            

            //Mostrar log de los ficheros leidos.
            foreach (ArchivoDatos dato in datosFichero)
            {
                string[] datosGrid = new string[5];

                datosGrid[0] = string.IsNullOrEmpty(dato.Ruta) ? "Alteración sin punto" : Path.GetFileName(dato.Ruta);
                datosGrid[1] = dato.NombrePunto;

                if (dato.Alteraciones != null)
                    foreach (ArchivoDatos alt in dato.Alteraciones)
                        datosGrid[2] += alt.NombrePunto + Environment.NewLine;

                datosGrid[3] = dato.Procesado ? "Procesado" : "No Procesado";
                datosGrid[4] = dato.Resultado;

                this.gridLogs.Rows.Add(datosGrid);
            }

            //mostrar en Log ficheros no procesados.
            foreach (ArchivoDatos fichNoProcesado in ficheroNoProcesados)
            {
                string[] datosGrid = new string[5];

                datosGrid[0] = Path.GetFileName(fichNoProcesado.Ruta);
                datosGrid[1] = fichNoProcesado.NombrePunto != null ? fichNoProcesado.NombrePunto : "";

                datosGrid[3] = fichNoProcesado.Procesado ? "Procesado" : "No Procesado";
                datosGrid[4] = fichNoProcesado.Resultado;

                this.gridLogs.Rows.Add(datosGrid);
            }

            MessageBox.Show(string.Format(" {0} ficheros leidos.\n", ficheros.Count()),
                            "Info", MessageBoxButtons.OK);

            if (ficherosTotales > 0)
                btnGenerarBat.Enabled = true;
                btnGuardarReporte.Enabled = true;
        }

        private string ValidarFichero(string[] titulo, string fich)
        {
            if (titulo.Length != 3 && titulo.Length != 4)
                return "Cabecera con formato erróneo";

            if (string.IsNullOrEmpty(titulo[2]))
                return "Fichero sin Punto con Nombre";

            return "";
        }

        private void LimpiarGridView()
        {
            this.gridLogs.Rows.Clear();
        }


        private void btnExaminar_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.textBox1.Text = folderBrowserDialog1.SelectedPath;
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("Error message: " + ex.Message);
                }
            }
        }



        private void btnGenerarBat_Click(object sender, EventArgs e)
        {         
            saveFileDialog1.Filter = "(*.bat)|*.bat";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {               
                string rutaFichero = Path.GetFullPath(saveFileDialog1.FileName);                       

                FormGenerarFicheroBAT _formGenerarFichero = new FormGenerarFicheroBAT();
                _formGenerarFichero.setRutaFichero(rutaFichero);
                _formGenerarFichero.setDatosFichero(datosFichero);
                _formGenerarFichero.ShowDialog();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool existeDir = Directory.Exists(textBox1.Text);

            if (!string.IsNullOrEmpty(this.textBox1.Text) && existeDir)
                btnCargarDir.Enabled = true;
            else
                btnCargarDir.Enabled = false;
        }

        private void btnBorrarTodo_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Empty; 
            LimpiarGridView();
            datosFichero = new List<ArchivoDatos>();
            btnGenerarBat.Enabled = false;
            btnGuardarReporte.Enabled = false;
            lblResErroneos.Text = "0";
            lblResFicherosProc.Text = "0";
            lblResPuntosProc.Text = "0";
            lblResAltProc.Text = "0";
            this.cbSubDirectorios.Checked = false;  
        }

        private void btnGuardarReporte_Click(object sender, EventArgs e)
        {
            saveFileDialog2.Filter = "(*.xlsx)|*.xlsx";
            saveFileDialog2.DefaultExt = "xlsx";
            saveFileDialog2.AddExtension = true;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string rutaFichero = Path.GetFullPath(saveFileDialog2.FileName);
                ExportarExcel.GenerarExcel(rutaFichero, datosFichero);               
            }
            
        }

        private void FormInicialCargaMasiva_Load(object sender, EventArgs e)
        {
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");   

        }
    }
}
