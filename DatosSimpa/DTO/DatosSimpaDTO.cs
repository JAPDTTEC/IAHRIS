namespace DatosSimpa
{
    public class DatosSimpaDTO
    {
        public int Año { get; set; }

        public int Mes { get; set; }

        public double Aportacion { get; set; }



        public DatosSimpaDTO(int año, int mes, double aportacion) 
        {
            Año = año;
            Mes = mes;
            Aportacion = aportacion;         
        }  


    }
}
