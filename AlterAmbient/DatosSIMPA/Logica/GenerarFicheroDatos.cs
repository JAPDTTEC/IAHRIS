using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace DatosSimpa.Logica
{
    public class GenerarFicheroDatos
    {

        
        public GenerarFicheroDatos() 
        {
            
        }


        //Clase preparada por si el dia de mañana se incluyen nuevos formatos.
        //sino se podría meter aquí la lógica.
        //
        
        public string Exportar(List<DatosSimpaDTO> datos, string codigoPunto, string formato, string ruta)
        {
            //Obtener contenido del fichero independientemente del formato en el que se exporte.
            StringBuilder contenidoFichero = ObtenerDatosFichero(datos, codigoPunto);

            

            if (formato == "csv")            
                return ExportarCSV(contenidoFichero, ruta);    
            
            //Añadir nuevos formatos si fuera necesario en un futuro.
            


            return string.Empty;
        }


        private string ExportarCSV(StringBuilder contenidoCSV, string ruta)
        {
            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine(contenidoCSV.ToString());
            }

            return Path.Combine(ruta);
        }

        private StringBuilder ObtenerDatosFichero(List<DatosSimpaDTO> datos, string codigoPunto)
        {
            StringBuilder contenido = new StringBuilder();

            
            contenido.AppendLine("MENSUAL;NATURAL;" + codigoPunto);
            

            foreach (DatosSimpaDTO dato in datos)
                contenido.AppendLine(string.Format("{0};{1};{2}", dato.Año, dato.Mes, dato.Aportacion.ToString().Replace(',', '.')));

            return contenido;
        }

    }
}
