using GMap.NET;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;

namespace DatosSimpa.Logica
{
    public class CoordenadaWGS84
    {
        public double X { get; set; }

        public double Y { get; set; }

        public CoordenadaWGS84(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class UTMCoordinate
    {
        public double X { get; set; }

        public double Y { get; set; }

        public int Zona { get;set; }

        public UTMCoordinate(double x, double y, int zona)
        {
            this.X = x;
            this.Y = y;
            this.Zona = zona;
        }

        public UTMCoordinate()
        {

        }

    }

    public static class Coordenadas {

        //Este metodo funciona correctamente.
        public static PointLatLng ConvertUTMToLatLng(double UTMEasting, double UTMNorthing, int UTMZoneNumber, bool isNorth)
        {
            ProjectedCoordinateSystem utmCoordinateSystem = (ProjectedCoordinateSystem)ProjectedCoordinateSystem.WGS84_UTM(UTMZoneNumber, isNorth);

            // Crear la transformación entre UTM y geográficas
            CoordinateTransformationFactory ctFactory = new CoordinateTransformationFactory();
            CoordinateTransformation utmToLatLon = (CoordinateTransformation)ctFactory.CreateFromCoordinateSystems(utmCoordinateSystem, GeographicCoordinateSystem.WGS84);

            // Transformar coordenadas UTM a geográficas (latitud y longitud)
            double[] input = { UTMEasting, UTMNorthing };
            double[] output = utmToLatLon.MathTransform.Transform(input);

            return new PointLatLng(output[1], output[0]);
        }


        //Funciona correctamente.
        public static UTMCoordinate ConvertLatLngToUTM(double latitude, double longitude)
        {
            if (latitude < -80 || longitude > 84)
                throw new Exception();

            //int zone = GetZone(latitude, longitude);
            int zone = 30;

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

            var wgs84 = GeographicCoordinateSystem.WGS84;

            CoordinateSystem utmCoordinateSystem = (CoordinateSystem)ProjectedCoordinateSystem.WGS84_UTM(zone, true);
            CoordinateTransformation trans = (CoordinateTransformation)ctfac.CreateFromCoordinateSystems(wgs84, utmCoordinateSystem);

            double[] result = trans.MathTransform.Transform(new double[] { longitude, latitude });

            return new UTMCoordinate(result[0], result[1], zone);

        }


    }      
}
