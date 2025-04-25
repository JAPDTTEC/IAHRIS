using DatosSimpa.DTO;
using DatosSimpa.Logica;
using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.IO;

namespace DatosSimpa
{

    public class MotorDatos
    {
        private readonly GestorDescargas _gestorDescargas;


        public MotorDatos() 
        { 
            _gestorDescargas = new GestorDescargas();
        }

        
        public List<string> ObtenerFicheros(DateTime fechaInicio, DateTime fechaFin)
        {
            //ObtenerFicheros
            List<string> rutas = _gestorDescargas.GetRutaFichero(fechaInicio, fechaFin);

            return rutas;
        }

        //Método no utilizado
        /*
        public void ProcesarRaster(List<string> rutasRaster, UTMCoordinate punto, string codigoPunto,
                                        DateTime fechaInicio, DateTime fechaFin)
        {
            List<DatosSimpaDTO>datosSimpa = new List<DatosSimpaDTO>();

            foreach (string ruta in rutasRaster)        
            {
                List<FicheroSimpaDTO> ficherosDeRar = Descomprimir.DescomprimirRarCache(ruta);

                foreach (FicheroSimpaDTO fichero in ficherosDeRar)
                {
                    double aportacion = ObtenerAportacion(punto, fichero);
                    DatosSimpaDTO dato = new DatosSimpaDTO(fichero.Año, fichero.Mes, aportacion);

                    datosSimpa.Add(dato);
                }
            }           
                
            //_exportarFichero.Exportar(datosSimpa, codigoPunto, "csv");       
        }
        */

        //Método no utilizado
        /*
        public double ObtenerAportacion(UTMCoordinate punto, FicheroSimpaDTO fichero)
        {
            GdalConfiguration.ConfigureGdal();

            // Registrar los controladores de GDAL
            Gdal.AllRegister();
            Ogr.RegisterAll();

            // Abrir el archivo de mapa raster
            string rutaTemporal = Path.Combine(Path.GetTempPath(), "temp_shapefile.shp");

            using (FileStream fileStream = new FileStream(rutaTemporal, FileMode.Create))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = fichero.ContenidoRaster.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }

            Dataset ds = Gdal.Open(rutaTemporal, Access.GA_ReadOnly);

            if (ds == null)            
                throw new Exception("No se pudo abrir el archivo de mapa raster.");            

            int width = ds.RasterXSize;
            float[] valor = new float[width];

            Band band = ds.GetRasterBand(1);
            band.ReadRaster((int)punto.X, (int)punto.Y, width, 1, valor, width, 1, 0, 0);

            // Cerrar el conjunto de datos
            ds.Dispose();

            if (File.Exists(rutaTemporal))  
                File.Delete(rutaTemporal);  

            return valor[(int)punto.X];
        }
        */

        public double ObtAportaciones(UTMCoordinate punto, string ruta)
        {
            //Raster que tenemos. Proyeccion: UTM, Zona: 30, Datum: AI_ETRS89 

            GdalConfiguration.ConfigureGdal();
            Gdal.AllRegister();
            Ogr.RegisterAll();           

            using (Dataset dataset = Gdal.Open(ruta, Access.GA_ReadOnly))
            { 
                double[] geoTransform = new double[6];
                dataset.GetGeoTransform(geoTransform);

                double utmX = punto.X; 
                double utmY = punto.Y;

                //utmX = 177142;
                //utmY = 4307941;

                //utmY = 4474273;
                //utmX = 440307;

                int pixelX = (int)((utmX - geoTransform[0]) / geoTransform[1]);
                int pixelY = (int)((geoTransform[3] - utmY) / -geoTransform[5]);

                int width = dataset.RasterXSize;
                int height = dataset.RasterYSize;

                double[] buffer = new double[1];                

                Band pInBand = dataset.GetRasterBand(1);
                pInBand.ReadRaster(pixelX, pixelY, 1, 1, buffer, 1, 1, 0, 0);
                
                double retorno = buffer[0];
                return retorno;
               
            }
        }

        //Método no utilizado
        /*
        public static string[] ReadFirstLines(string filePath, int numLines)
        {
            // Verificar si el archivo existe
            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo no existe.");
                return null;
            }

            // Leer las primeras N líneas del archivo
            string[] lines = new string[numLines];
            using (StreamReader reader = new StreamReader(filePath))
            {
                for (int i = 0; i < numLines && !reader.EndOfStream; i++)
                {
                    lines[i] = reader.ReadLine();
                }
            }

            return lines;
        }
        */

        //Método no utilizado
        /*
        static double GetDoubleValueFromHeader(string[] lines, string keyword)
        {
            foreach (var line in lines)
            {
             
                if (line.StartsWith(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 && double.TryParse(parts[1], out double value))
                    {
                        return value;
                    }
                }
            }

            
            return -1;
        }
        */
    }
}
