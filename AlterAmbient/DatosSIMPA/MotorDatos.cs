using DatosSimpa.Logica;
using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;

namespace DatosSimpa
{
    /// <summary>
    /// Clase encargada de recuperar todos los ficheros de SIMPA 
    /// comprendidos entre las fechas indicadas, de los que leera y obtendrá los valores
    /// con las aportaciones para un punto dado por el usuario.
    /// </summary>
    public class MotorDatos
    {
        private readonly GestorDescargas _gestorDescargas;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MotorDatos() 
        { 
            _gestorDescargas = new GestorDescargas();
        }

        /// <summary>
        /// Busca los ficheros con datos comprendidos entre las fechas indicadas por el usuario.
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<string> ObtenerFicheros(DateTime fechaInicio, DateTime fechaFin)
        {
            //ObtenerFicheros
            List<string> rutas = _gestorDescargas.GetRutaFichero(fechaInicio, fechaFin);

            return rutas;
        }

        /// <summary>
        /// Lee un raster de datos y obtiene el valor para la coordinada especificada
        /// </summary>
        /// <param name="punto">Coordinada o punto del raster del que recuperar el valor.</param>
        /// <param name="ruta">Ruta en la que se encuentra el fichero raster</param>
        /// <returns></returns>
        public double GetAportacionesRaster(UTMCoordinate punto, string ruta)
        {
            //Raster que tenemos. Proyeccion: UTM, Zona: 30, Datum: AI_ETRS89 
            GdalConfiguration.ConfigureGdal();
            Gdal.AllRegister();
            Ogr.RegisterAll();           

            using (Dataset dataset = Gdal.Open(ruta, Access.GA_ReadOnly))
            { 
                double[] geoTransform = new double[6];
                dataset.GetGeoTransform(geoTransform);

                //obtener pixeles del fichero a partir de las coordenadas.
                int pixelX = (int)((punto.X - geoTransform[0]) / geoTransform[1]);
                int pixelY = (int)((geoTransform[3] - punto.Y) / -geoTransform[5]);

                //Obtener tamaño del raster
                int width = dataset.RasterXSize;
                int height = dataset.RasterYSize;

                double[] buffer = new double[1];                

                Band pInBand = dataset.GetRasterBand(1);
                pInBand.ReadRaster(pixelX, pixelY, 1, 1, buffer, 1, 1, 0, 0);
                
                double valorObtenido = buffer[0];
                return valorObtenido;
            }
        }
    }
}
