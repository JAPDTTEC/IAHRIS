using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosSimpa.DTO
{
    public class DatoFicheroGuia
    {
        public string Ruta { get; set; }

        public string FechaDatos { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public string Proyeccion { get; set; }
        public DatoFicheroGuia()
        {
       
        }
        public DatoFicheroGuia(string ruta, string fechaDatos, DateTime fechaActualizacion,string proyeccion)
        {
            Ruta = ruta;
            FechaDatos = fechaDatos;
            FechaActualizacion = fechaActualizacion;
            Proyeccion = proyeccion;
        }
    }
}
