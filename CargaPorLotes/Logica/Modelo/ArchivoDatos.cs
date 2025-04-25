using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace CargaPorLotes.Logica
{
    public class ArchivoDatos
    {
        //ruta del Fichero
        public string Ruta { get; set; }
        //Punto o Alteracion
        public bool EsAlteracion { get; set; }
        //Nombre del punto
        public string NombrePunto { get; set; }
        //Lista de las alteraciones
        public List<ArchivoDatos> Alteraciones { get; set; }
        //Datos del fichero
        //public List<string[]> datos;
        //Procesado
        public bool Procesado { get; set; }
        //Almacena si el fichero tiene un error al cargar
        public string Resultado { get; set; }
        //Descripción del punto o alteración
        public string Descripcion { get; set; }
        //Nombre del Regimen Natural
        public string NombreRegimen { get; set; }
        //Nombre del Regimen Alterado
        public string AbreviaturaRegimen { get; set; } 

        public static ArchivoDatos GetPunto(string rutaFichero, string nombrePun, string descripcion)
        {
            return new ArchivoDatos
            {
                EsAlteracion = false,
                Ruta = rutaFichero,
                NombrePunto = nombrePun,
                Alteraciones = new List<ArchivoDatos>(),
                Procesado = false,
                Descripcion = descripcion,
                AbreviaturaRegimen = "nat",
                NombreRegimen = "Natural",
                Resultado = ""
            };
        }


        public static ArchivoDatos GetFicheroNoProcesado (string rutaFichero, string error)
        {
            return new ArchivoDatos()
            {
                EsAlteracion = false,
                Ruta = rutaFichero,
                Alteraciones = new List<ArchivoDatos>(),
                Procesado = false,
                Resultado = error
            };
        }



        public static ArchivoDatos GetAlteracion(string rutaFichero, string nombreAlt, string descripcion)
        {
            return new ArchivoDatos
            {
                EsAlteracion = true,
                Ruta = rutaFichero,
                NombrePunto = nombreAlt,
                Alteraciones = null,
                Procesado = false,
                Descripcion = descripcion,
                AbreviaturaRegimen = "alt",
                NombreRegimen = "Alterado",
                Resultado = ""
            };
        }



    }
}
