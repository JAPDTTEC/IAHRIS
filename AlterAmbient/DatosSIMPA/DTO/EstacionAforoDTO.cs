using DatosSimpa.Logica;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace DatosSimpa.DTO
{

    [XmlRoot(ElementName = "obtenerEstacionAforoResult")]
    public class ObtenerEstacionAforoResult
    {
        public ObtenerEstacionAforoResult() { Estaciones = new List<EstacionAforoDTO>(); }

        [XmlElement("EstacionAforo")]
        public List<EstacionAforoDTO> Estaciones { get; set; }
    }


    [XmlRoot(ElementName = "EstacionAforo")]
    public class EstacionAforoDTO
    {
        [XmlElement(ElementName = "Id", Namespace = "")]
        public int Codigo { get; set; }

        [XmlElement(ElementName = "nombreEstacion", Namespace = "")]
        public string Nombre { get; set; }

        [XmlElement(ElementName = "UTMX_H30_ETRS89", Namespace = "")]
        public double UTMX_H30_ETRS89 { get; set; }

        [XmlElement(ElementName = "UTMY_H30_ETRS89", Namespace = "")]
        public double UTMY_H30_ETRS89 { get; set; }

        [XmlElement(ElementName = "nombreConfederacion", Namespace = "")]
        public string Confederacion { get; set; }

        [XmlElement(ElementName = "estado", Namespace = "")]
        public string Estado { get; set; }

        [XmlElement(ElementName = "situacionEstacion", Namespace = "")]
        public string Situacion_Estacion { get; set; }


        public EstacionAforoDTO(int codigo, string nombre, UTMCoordinate coordenada)
        {
            Codigo = codigo;
            Nombre = nombre;
        }

        public EstacionAforoDTO()
        { 
        }

  
        public static List<EstacionAforoDTO> CargarListaXml()
        {
            string inputString = @"./DatosSIMPA/Datos/EstacionesHidrograficas.xml";
            ObtenerEstacionAforoResult listaXMl = new ObtenerEstacionAforoResult();

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "obtenerEstacionAforoResult";
            //xRoot.Namespace = "http://ims4.mapa.mapya.es/confvisor";
            xRoot.IsNullable = true;

            XmlSerializer serializer = new XmlSerializer(typeof(ObtenerEstacionAforoResult), xRoot);

            using (FileStream stream = File.OpenRead(inputString))
            {
                listaXMl = (ObtenerEstacionAforoResult)serializer.Deserialize(stream);
            }

            return listaXMl.Estaciones;
        }

        public UTMCoordinate ObtenerCoordenadasUTM()
        {
            //Usar un calcular zona para el huso
            return new UTMCoordinate(UTMX_H30_ETRS89, UTMY_H30_ETRS89, 30);
        }
        
        public override string ToString()
        {
            return Codigo + " - " + Nombre;
        }


    }


}
