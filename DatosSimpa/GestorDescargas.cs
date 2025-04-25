using DatosSimpa.API;
using DatosSimpa.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatosSimpa
{
    public class GestorDescargas
    {

        private readonly MitecoAPI _mitecoAPI;
        private readonly string _remoteUri;


        public static ProgresoDescargaForm _progresoDescargaForm = new ProgresoDescargaForm();


        public GestorDescargas() 
        {
            _mitecoAPI = new MitecoAPI();
            _remoteUri = @"https://ceh-flumen64.cedex.es/descargas/ERH_Entregamayo2019B/";
        }


        /// <summary>
        /// Con los años de inicio y final, compone el nombre de los ficheros que necesita
        /// </summary>
        /// <param name="añoInicio">año Inicio de rango</param>
        /// <param name="añoFinal">año Fin del rango.</param>
        /// <returns></returns>
        public List<string> GetRutaFichero(DateTime añoInicio, DateTime añoFinal)
        {
            string rutaFichero = Application.StartupPath + "\\RAR"; //Variable global donde guardemos el fichero.
       
            string nombreCarpeta = "Aportacion_acumulada_";
            
            List<string> listaRutas = new List<string>();

            //Obtener decadas inicio y fin
            int decadaIni = (añoInicio.Year - añoInicio.Year % 10);
            int decadaFin = (añoFinal.Year - añoFinal.Year % 10);

            //Si es primer año de decada y mes menor que Oct, van en la decada anterior.
            if (añoInicio.Year % 10 == 0 && añoInicio.Month < 10)
                decadaIni -= 10;

            //decada fin
            if (añoFinal.Year % 10 == 0 && añoFinal.Month < 10)
                decadaFin -= 10;

            for (int decada = decadaIni; decada <= decadaFin; decada += 10)
            {           
                int finFich = decada + 10;

                if(decada != 2010)
                {
                    listaRutas.Add(Path.Combine(rutaFichero, nombreCarpeta + decada.ToString().Substring(2, 2) + "_" + finFich.ToString().Substring(2, 2),
                        string.Format(nombreCarpeta + "{0}_{1}", decada.ToString().Substring(2, 2), finFich.ToString().Substring(2, 2) + ".rar")
                        ));
                }
                else
                {
                    listaRutas.Add(Path.Combine(rutaFichero, nombreCarpeta + decada.ToString() + "_" + finFich.ToString().Substring(2, 2),
                       string.Format(nombreCarpeta + "{0}_{1}", decada.ToString(), finFich.ToString().Substring(2, 2) + ".rar")
                       ));
                }
                
                
            }

            return listaRutas;
        }

        //metodo Obsoleto
        //public bool FicheroActualizado_Obsoleto(string ficheroLocal)
        //{
        //    //try
        //    //{
        //    //    _mitecoAPI.DescargarFicheroGuia();

        //    //    List<DatoFicheroGuia> contenidoGuia = _mitecoAPI.RecuperarFicheroGuia();
        //    //    FileInfo fichero = new FileInfo(ficheroLocal);

        //    //    string nomFichero = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fichero.FullName));

        //    //    //FicheroSimpaDTO linea = contenidoGuia.Find(x => x.NombreFichero.Contains(nomFichero));
        //    //    DatoFicheroGuia linea = contenidoGuia.Find(x => x.Ruta.EndsWith(nomFichero));

        //    //    if (linea == null)
        //    //        throw new Exception("Fichero no encontrado");

        //    //    //Para que esté actualizado, la fecha de escritura debe ser mayor que la fecha de actualización indicada
        //    //    return fichero.LastWriteTime > linea.FechaActualizacion;
        //    //}
        //    //catch(Exception ex) 
        //    //{
        //    //    throw ex;
        //    //}
        //}

        public bool FicheroActualizado(string rutaFichero)
        {
            try
            {
                _mitecoAPI.DescargarFicheroGuia();
                List<DatoFicheroGuia> contenidoGuia = _mitecoAPI.RecuperarFicheroGuia();
                FileInfo fichero = new FileInfo(rutaFichero);

                string nomFichero = Path.GetFileName(Path.GetFileName(fichero.FullName));

                //FicheroSimpaDTO linea = contenidoGuia.Find(x => x.NombreFichero.Contains(nomFichero));
                DatoFicheroGuia linea = contenidoGuia.Find(x => x.Ruta.EndsWith(nomFichero));

                if (linea == null)
                    throw new Exception("Fichero no encontrado");

                //Para que esté actualizado, la fecha de escritura debe ser mayor que la fecha de actualización indicada
                bool resultado = fichero.CreationTime > linea.FechaActualizacion;
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task IniciarDescarga(string fichero)
        {
            _progresoDescargaForm.Show();
            _progresoDescargaForm.UseWaitCursor = true;
            WebClient myWebClient = new WebClient();
            Uri myStringWebResource;
           
            string nombreFichero = fichero.Split('\\')[fichero.Split('\\').Length - 1];            
            
            string nombreCarpeta = nombreFichero.Remove(nombreFichero.Length-4);
            myStringWebResource = new Uri(_remoteUri + nombreFichero);

            if (!Directory.Exists(Application.StartupPath + "\\RAR"))            
                Directory.CreateDirectory(Application.StartupPath + "\\RAR");
            

            if (!Directory.Exists(Application.StartupPath + "\\RAR\\" + nombreCarpeta))            
                Directory.CreateDirectory(Application.StartupPath + "\\RAR\\" + nombreCarpeta);
            

            string archivo = Application.StartupPath + "\\RAR\\" + nombreCarpeta + "\\" + nombreFichero;
            myWebClient.DownloadProgressChanged += myWebClient_DownloadProgressChanged;

            _progresoDescargaForm.ActualizarEtiquetaArchivo("Descargando: "+ nombreFichero);

            await myWebClient.DownloadFileTaskAsync(myStringWebResource, archivo); 
        }

        void myWebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _progresoDescargaForm.ActualizarEtiquetaPorcentaje(e.ProgressPercentage.ToString()+"%");
            _progresoDescargaForm.ActualizarEtiquetaDescarga((e.BytesReceived / 1024/1024).ToString()+"MB/"+(e.TotalBytesToReceive / 1024/1024).ToString()+"MB");
            _progresoDescargaForm.progressBarDownload.Maximum = (int)(e.TotalBytesToReceive / 1024);
           _progresoDescargaForm.progressBarDownload.Value = (int)(e.BytesReceived/1024);           
        }
        /*
        public void DescargarListaXML()
        {
            WebClient myWebClient = new WebClient();
            Uri myStringWebResource = new Uri("");
            myWebClient.DownloadFile(myStringWebResource, ruta);

            return ruta;
        }
        */
    }
}
