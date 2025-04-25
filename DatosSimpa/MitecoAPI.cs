using DatosSimpa.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace DatosSimpa.API
{



    public class MitecoAPI
    {

        private readonly string _rutaFicheroGuia;
        private readonly string _urlMiteco;



        public MitecoAPI()
        {
            _rutaFicheroGuia = @"C:\Users\jperera1\Documents\DescargaSIMPA\ficheroGuia.txt";
            _urlMiteco = @"https://ceh-flumen64.cedex.es/descargas/ERH_Entregamayo2019B";
        }


        public async void DescargarFichero(int añoInicio, string rutaDescarga)
        {
            string nombreFichero = "Aportacion_acumulada_";
            int añoFin = añoInicio + 10;
            nombreFichero = nombreFichero + añoInicio.ToString().Substring(2,2) + "_" + añoFin.ToString().Substring(2, 2) + ".rar";
            string rutaDescargaSIMPA = Path.Combine(_urlMiteco, nombreFichero);

            //using (StreamReader reader = new StreamReader(WebRequest.Create(rutaDescargaSIMPA)
            //        .GetResponse().GetResponseStream()))
            //{
            //    String content = reader.ReadToEnd();
            //    File.WriteAllText(rutaDescarga, content);
            //}

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(rutaDescargaSIMPA);
            }

            //DescomprimirFichero(rutaDescargaSIMPA);
        }

        public List<FicheroSimpaDTO> RecuperarFicheroGuia()
        {
            List<FicheroSimpaDTO> contenido = new List<FicheroSimpaDTO>();

            using (var sr = new StreamReader(_rutaFicheroGuia))
            {
                string linea;


                // Lee el archivo línea por línea
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] valores = linea.Split('\t'); //Dependera de la configuración del fichero para poder obtener los 3 campos.

                    contenido.Add(new FicheroSimpaDTO()
                    {
                        Año = Int32.Parse(valores[0]),
                        NombreFichero = valores[1],
                        Fecha_Actualizacion = DateTime.Parse(valores[2])
                    });
                }
            }

            return contenido;

        }
    }
}
