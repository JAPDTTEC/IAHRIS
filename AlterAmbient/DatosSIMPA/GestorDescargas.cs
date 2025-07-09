using DatosSimpa.API;
using DatosSimpa.DTO;
using MultiLangXML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatosSimpa
{
    /// <summary>
    /// Clase encargada de comunicarse con la web del ministerio y realizar las descargas de ficheros
    /// necearias para la obtención de Datos de SIMPA.    /// 
    /// </summary>
    public class GestorDescargas
    {

        private readonly MitecoAPI _mitecoAPI;
        private readonly string _remoteUri;
        private readonly MultiIdiomasXML _traductor;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GestorDescargas() 
        {
            _mitecoAPI = new MitecoAPI();
            _remoteUri = @"https://ceh-flumen64.cedex.es/descargas/ERH_Entregamayo2019B/";
            _traductor = MultiIdiomasXML.Instancia;
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

                if(decada != 2010 & decada !=2020)
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

        /// <summary>
        /// comprueba si el fichero de la ruta recibida está ya cacheado en local o hay una versión posterior.
        /// </summary>
        /// <param name="rutaFichero">ruta del fichero</param>
        /// <returns>Booleano que indica si el fichero está actualizado</returns>
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
                    throw new FileNotFoundException(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strFicheroNoEncontrado"));

                //Para que esté actualizado, la fecha de escritura debe ser mayor que la fecha de actualización indicada
                bool resultado = fichero.CreationTime > linea.FechaActualizacion;
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Descarga de SIMPA el fichero indicado por parametro. Crea un cliente de descarga por defecto aunque puede
        /// recibir uno por parametro que tenga asociados los eventos deseados por el usuario.
        /// </summary>
        /// <param name="fichero">ruta del fichero que descarga</param>
        /// <param name="myWebClient">WebClient personalidazo del usuario o crea uno por defecto si no está indicado</param>
        /// <returns></returns>
        public async Task IniciarDescarga(string fichero, WebClient myWebClient = null)
        {
            //Comprueba si el cliente es nulo y si es asi, crea uno por defecto.
            if (myWebClient == null)
                myWebClient = new WebClient();

            string nombreFichero = fichero.Split('\\')[fichero.Split('\\').Length - 1];
            string nombreCarpeta = nombreFichero.Remove(nombreFichero.Length - 4);
            Uri myStringWebResource = new Uri(_remoteUri + nombreFichero);

            //Valida rutas
            if (!Directory.Exists(Application.StartupPath + "\\RAR"))
                Directory.CreateDirectory(Application.StartupPath + "\\RAR");

            if (!Directory.Exists(Application.StartupPath + "\\RAR\\" + nombreCarpeta))
                Directory.CreateDirectory(Application.StartupPath + "\\RAR\\" + nombreCarpeta);

            //Compone el fichero e inicia la descarga.
            string archivo = Application.StartupPath + "\\RAR\\" + nombreCarpeta + "\\" + nombreFichero;
            try
            {
                await myWebClient.DownloadFileTaskAsync(myStringWebResource, archivo);
            }
            catch (Exception e)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "srtEspacioDiscoInsuficiente"), 
                                _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "srtEspacioInsuficiente"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                try 
                { 
                    File.Delete(archivo);
                } 
                catch { }
                    throw new OutOfMemoryException(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "srtEspacioInsuficiente"), e);
            }
        }


    }
}
