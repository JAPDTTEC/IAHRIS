using CargaPorLotes.Logica;
using DatosSimpa.Logica;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class Form1 : Form
    {
       
        public static ProgresoDatosForm _progresoDatosForm = new ProgresoDatosForm();       



        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        }

        private void btnRecuperarDatos_Click(object sender, EventArgs e)
        {
            //Validar Datos 
            //Comprobar aqui que los nombres que se introducen son correctos. (Hay nombre de punto, nombre proyecto < 20, etc..)

            return;






            //Codigo Anterior
            //1º Obtener punto y rango fechas seleccionadas.

            //    1.1 Recorrer todos los ficheros que cubren ese rango de años. Habra que ir mes a mes
            //        Para cada fichero comprobar si tenemos el fichero y si está actualizado para esas fechas.
            //         LLamar al fichero guía y comprobar eso. (LLamar al gestor de descargas si es necesario)
            //        Una vez obtenido el fichero actualizado, buscar el punto dentro del mismo.
            //        obtener la aportación correspondiente al punto.
            //        
            //2º Crear objeto en memoria con datos obtenidos
            //       El csv obtenido de simpa será mensual y natural. (MENSUAL; NATURAL; NUM_PUNTO)
            //                                                           Año;Mes;Aportacion
            //3º Exportar a formato deseado, csv en este caso, dejar abierto por si en un futuro 
            //  se quiere mapear a mas formatos. (json, xml...)

        //    try
        //    {
                
        //        DateTime fechaIni = DateTime.Parse(this.dtPickerFechaIni.Value.Month+"/"+ this.dtPickerFechaIni.Value.Year);
        //        DateTime fechaFin = DateTime.Parse(this.dtPickerFechaFin.Value.Month + "/" + this.dtPickerFechaFin.Value.Year);
                
        //        //Descargar fichero guia CSV


                
                
                
        //        //obtener todas las rutas de ficheros rar.
        //        List<string> rutasFicheros = _motorDatos.ObtenerFicheros(fechaIni, fechaFin);

        //        //Comprobar si cada fichero está actualizado.
        //        await CheckFicherosActualizados(rutasFicheros);

        //        foreach (var rutaFichero in rutasFicheros)
        //            await Descomprimir.DescomprimirRar(rutaFichero);

        //        Descomprimir._progresoDescompresionForm.Hide();

        //        Dictionary<string, DateTime> filePathsAndDates = new Dictionary<string, DateTime>();

        //        string pathData = Path.Combine(Directory.GetCurrentDirectory(), "DATA");


                
        //            string[] files = Directory.GetFiles(pathData, "*.asc", SearchOption.TopDirectoryOnly);

        //            foreach (var file in files)
        //            {
        //                filePathsAndDates.Add(file, DateTime.Parse(file.Split('\\')[file.Split('\\').Length - 1].Remove(0, 6).Split('.')[0].Replace('_', '/')));
        //            }               
                
                
        //        filePathsAndDates = filePathsAndDates.Where(kv => kv.Value >= fechaIni && kv.Value <= fechaFin).ToDictionary(pair => pair.Key, pair => pair.Value);
        //        filePathsAndDates = filePathsAndDates.OrderBy(kv => kv.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        //        int completed = 0;
        //        _progresoDatosForm.Show();
        //        _progresoDatosForm.UseWaitCursor = true;

        //        List<DatosSimpaDTO> datosSimpaDTOs = new List<DatosSimpaDTO>();
        //        puntoSeleccionado = new UTMCoordinate(Double.Parse(tb_PuntoX.Text), Double.Parse(tb_puntoY.Text), 30);
        //        foreach (var item in filePathsAndDates)
        //        {

        //            datosSimpaDTOs.Add(
        //                new DatosSimpaDTO(item.Value.Year, item.Value.Month, _motorDatos.ObtAportaciones(puntoSeleccionado, item.Key))
        //              );

        //            double percentage = (double)completed/filePathsAndDates.Count;

        //            completed++;

        //            _progresoDatosForm.ActualizarEtiquetaArchivo(item.Key.Split('\\')[item.Key.Split('\\').Length-1]);
        //            _progresoDatosForm.ActualizarEtiquetaPorcentaje((int)(percentage*100) + "%");
        //            _progresoDatosForm.ActualizarEtiquetaDatos(completed.ToString() + "/" + filePathsAndDates.Count.ToString());
        //            _progresoDatosForm.progressBarDatos.Maximum = filePathsAndDates.Count;
        //            _progresoDatosForm.progressBarDatos.Value = (int)(completed);
        //            _progresoDatosForm.Update();

        //        }
        //        _progresoDatosForm.Hide();

                
        //        string rutaFicheroBat = AppDomain.CurrentDomain.BaseDirectory + "temp.bat";
        //        GenerarFicheroDatos _generarFicheroDatos = new GenerarFicheroDatos();
                
        //        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        //        saveFileDialog1.Filter = "Listas de datos (*.csv)|*.csv";
        //        saveFileDialog1.Title = "Guardar archivo CSV";
        //        saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //        saveFileDialog1.FileName = tbCodigoPunto.Text + "_DatosSimpa_" + datosSimpaDTOs.First().Mes + "-" + datosSimpaDTOs.First().Año + "_" + datosSimpaDTOs.Last().Mes + "-" + datosSimpaDTOs.Last().Año + ".csv";

        //        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
                    

        //            string rutaFicheroDatos = _generarFicheroDatos.Exportar(datosSimpaDTOs, tbCodigoPunto.Text, tbNomAlt.Text, chbAlteracion.Checked, "csv", saveFileDialog1.FileName);

        //            GenerarFicherosBAT generarFicheroBat = new GenerarFicherosBAT(rutaFicheroBat, true, false);

        //            ArchivoDatos punto = ArchivoDatos.GetPunto(chbAlteracion.Checked ? "" : rutaFicheroDatos,
        //                                                  tbCodigoPunto.Text.ToUpperInvariant(), "Punto Datos Simpa");
        //            if (chbAlteracion.Checked)
        //                punto.Alteraciones.Add(ArchivoDatos.GetAlteracion(rutaFicheroDatos, tbNomAlt.Text.ToUpperInvariant(), "Alter. Datos Simpa"));

        //            List<ArchivoDatos> listaPuntos = new List<ArchivoDatos>() { punto };

        //            generarFicheroBat.CrearFicheroBat(listaPuntos, tbProyecto.Text.ToUpperInvariant(), "Proy. Datos Simpa", false, false, true);

        //            var procesoEjecutar = EjecutarFicheroBat.Ejecutar(rutaFicheroBat);
        //            File.Delete(rutaFicheroBat);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + ":" + ex.StackTrace);
        //    }
        //}
        //private async Task CheckFicherosActualizados(List<string> rutasFicheros)
        //{
        //    List<string> ficherosSinActualizar = new List<string>();
        //    DialogResult result = DialogResult.None;

        //    try
        //    {
        //        foreach (string ruta in rutasFicheros)
        //            if (!_gestorDescargas.FicheroActualizadoV2(ruta))
        //                ficherosSinActualizar.Add(ruta);

        //        //Si hay ficheros sin actualizar, preguntar si se desea actualizarlos.
        //        if (ficherosSinActualizar.Count > 0)
        //        {
        //            string mensajeFicheros = "";

        //            foreach (var item in ficherosSinActualizar)
        //            {
        //                int i = item.LastIndexOf('\\');
        //                mensajeFicheros += item.Substring(i + 1) + "\n";
        //                //mensajeFicheros += item.ToString()+"\n";
        //            }


        //            result = MessageBox.Show("Ficheros no actualizados:\n" + mensajeFicheros +
        //                            "¿Desea Actualizarlos?", "Información", MessageBoxButtons.OKCancel,
        //                            MessageBoxIcon.Information);
        //        }

        //        //Si pulsa OK -> actualiza ficheros.
        //        //Si cancela, continua con ficheros desactualizados.
        //        if (result == DialogResult.OK)
        //            await (Descargar(ficherosSinActualizar));
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }

        }
        /*
        private async Task Descargar(List<string> archivos)
        {
            foreach (var item in archivos)
                await _gestorDescargas.IniciarDescarga(item);


            GestorDescargas._progresoDescargaForm.Dispose();
        }
        */
        /*
        public static double Procesar(string filePath, UTMCoordinate punto)
        {
            return Descomprimir.DatosArchivos(filePath,punto);
        }
        */
        private void btnCerrar_Click(object sender, EventArgs e)
        {        
            this.Close();
        }
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            
        }

       
        
    }
}
