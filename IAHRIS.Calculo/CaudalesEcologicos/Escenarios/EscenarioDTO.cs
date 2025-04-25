namespace IAHRIS.Calculo.CaudalesEcologicos.Escenarios
{
    public class PuntuacionMagnitudDTO
    {
        public int MenorQMin { get; set; }
        public int Qmin_P95 { get; set; }
        public int P95_P85 { get; set; }
        public int P85_P75 { get; set; }
        public int P75_P65 { get; set; }
        public int P65_P50 { get; set; }
        public int P50_P10 { get; set; }
        public int MayorP10 { get; set; }
        public int Puntuacion_Parcial { get; set; }

    }

    public class PuntuacionEstacionalidadDTO
    {
        public int Cumple { get; set; }
        public int NoCumple { get; set; }
        public int Puntuacion_Parcial { get;set; }
    }

    public class PuntuacionVariabilidadDTO
    {
        public int MenorP95 { get; set; }
        public int P95_P85 { get; set; }
        public int P85_P75 { get; set; }
        public int P75_P65 { get; set; }
        public int P65_P50 { get; set; }
        public int P50_P10 { get; set; }
        public int MayorP10 { get; set; }
        public int Puntuacion_Parcial { get; set; }

    }

    public class PuntuacionDTO
    {
        public PuntuacionMagnitudDTO Puntuacion_Magnitud { get; set; }

        public PuntuacionEstacionalidadDTO Puntuacion_Estacionalidad { get; set; }

        public PuntuacionVariabilidadDTO Puntuacion_Variabilidad { get; set; }

        public int Puntuacion_Total { get; set; }

        public PuntuacionDTO()
        {
            Puntuacion_Magnitud = new PuntuacionMagnitudDTO();
            Puntuacion_Variabilidad = new PuntuacionVariabilidadDTO();
            Puntuacion_Estacionalidad = new PuntuacionEstacionalidadDTO();
        }
    }


    public class EscenarioDTO
    {
        public int Id_Escenario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Id_Punto_Ref { get; set; }
        public int Id_Alteracion_Ref { get; set; }
        public bool Por_Defecto { get; set; }
        public double[] Caudales_Ecologicos { get; set; }
        public PuntuacionDTO Puntuacion { get; set; }
        public double Demanda_Ambiental{ get; set; }
        public double Eficiencia{ get; set; }



        override
        public string ToString()
        {
            return Nombre.ToString();
        }

        public EscenarioDTO()
        {
            Puntuacion = new PuntuacionDTO();
        }


    }
}
