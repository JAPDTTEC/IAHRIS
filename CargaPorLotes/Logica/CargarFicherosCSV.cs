using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CargaPorLotes.Logica
{
    public class CargarFicherosCSV
    {

        public CargarFicherosCSV() { }

        /// <summary>
        /// Devuelve un punto o alteracion de un fichero csv.
        /// Devolverá error si el fichero no tiene el formato correcto.
        /// </summary>
        /// <param name="fichero"></param>
        /// <returns></returns>
        public ArchivoDatos ObtenerPuntos(string fichero)
        {
            ArchivoDatos fich = new ArchivoDatos();
          
            fich.Ruta = fichero;
            string error = "";
            string[] titulo;
            string tipoPeriodo;
            bool formatoCorrecto = true;

            try
            {
                using (StreamReader sr = new StreamReader(fichero))
                {
                    //obtener datos
                    titulo = sr.ReadLine().Split(';');
                    tipoPeriodo = titulo[0];

                    //Validaciones
                    if (titulo.Length != 3 && titulo.Length != 4)
                        error = "Formato de fichero incorrecto.";

                     if (!formatoCorrecto)
                        error = "No todas las lineas tienen el formato de fichero incorrecto.";
                }


                //Genera el Archivo de de en memoria si ha pasado las validaciones previas.
                if (titulo.Length == 3)
                    fich = ArchivoDatos.GetPunto(fichero, titulo[2], "Punto Carga Masiva");
 
                
                if (!string.IsNullOrEmpty(error))
                {
                    fich.Resultado = error;
                    fich.Procesado = false;
                }
                
            }
            catch (Exception ex)
            {
                fich.Resultado = ex.Message;
                return fich;
            }

            return fich;
        }

        public ArchivoDatos ObtenerAlteracion(string fichero)
        {
            ArchivoDatos alteracion = new ArchivoDatos();
            string[] titulo;
            bool formatoCorrecto = true;
            string tipoPeriodo;
            List<string[]> datos = new List<string[]>();


            using (StreamReader sr = new StreamReader(fichero))
            {
                //obtener datos
                titulo = sr.ReadLine().Split(';');
                tipoPeriodo = titulo[0];

                while (sr.Peek() != -1)
                    datos.Add(sr.ReadLine().Split(';'));


                if (tipoPeriodo == "DIARIO")
                    datos.All(x => formatoCorrecto = x.Length == 2);
                else
                    datos.All(x => formatoCorrecto = x.Length == 3);


                if (titulo.Length == 4)
                        alteracion = ArchivoDatos.GetAlteracion(fichero, titulo[3], "Alt Carga Masiva");

            }

            return alteracion;

        }

        public ArchivoDatos AñadirAlteracion(ArchivoDatos punto, ArchivoDatos alteracion)
        {
            punto.Alteraciones.Add(alteracion);
            return punto;
        }
    }
}
