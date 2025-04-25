using System;
using System.IO;

namespace DatosSimpa.DTO
{
    public class FicheroSimpaDTO
    {
        public string NombreFichero { get; set; }

        public int Año { get; set; }

        public int Mes { get; set; }

        public DateTime Fecha_Actualizacion { get; set; }

        public MemoryStream ContenidoRaster{ get; set; }
    }
}
