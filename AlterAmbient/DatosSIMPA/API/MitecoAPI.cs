using DatosSimpa.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace DatosSimpa.API
{



    public class MitecoAPI
    {

        private readonly string _rutaFicheroGuia;
        private readonly string _nombreArchivo;



        public MitecoAPI()
        {
            
            _rutaFicheroGuia = @"https://ceh.cedex.es/iahris/iahris.csv";           
            _nombreArchivo = @"iahris.csv";
        }

        public class datosMiteco { 
        }

        public string DescargarFicheroGuia()
        {
            WebClient myWebClient = new WebClient();
            Uri myStringWebResource = new Uri(_rutaFicheroGuia);
            myWebClient.DownloadFile(myStringWebResource, _nombreArchivo);

            return _nombreArchivo;
        }
        

        public List<DatoFicheroGuia> RecuperarFicheroGuia()
        {
            List<DatoFicheroGuia> contenido = new List<DatoFicheroGuia>();

            
            using (var sr = new StreamReader(Application.StartupPath +"\\"+ _nombreArchivo))
            {
                string linea;
                // Lee el archivo línea por línea
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] valores = linea.Split(';'); 

                    if (valores[0] == "URL")
                    {
                        continue;
                    }
                    contenido.Add(new DatoFicheroGuia()
                    {
                        Ruta = valores[0],
                        FechaDatos = valores[1],
                        FechaActualizacion = DateTime.Parse(valores[2]),
                        Proyeccion = valores[3],
                        
                    });
                }
            }

            return contenido;

        }
    }
}
