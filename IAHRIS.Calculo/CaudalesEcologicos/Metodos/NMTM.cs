namespace IAHRIS.Calculo.CaudalesEcologicos.Metodos
{

    public enum TipoNMTM
    {
        MINIMUN=1, //P90
        FAIR = 2,
        GOOD = 3,
        EXCELLENT  = 4,
        OUTSTANDING = 5,
        OPTIMUN = 6
    }

    public class NMTM : IMetodo
    {
        private readonly TipoNMTM _tipoNMTM;

        public string Nombre { get; set; }

        public NMTM(TipoNMTM tipo) 
        {
            _tipoNMTM = tipo;
            
            if (tipo != TipoNMTM.MINIMUN)
            {
                Nombre = "NMTM_" + tipo.ToString();
            }
            else
            {
                Nombre = "NMTM_P90";
            }
        }

        public NMTM()
        {
            //Valor por defecto.
            _tipoNMTM = TipoNMTM.GOOD;
        }


        public double[] CalcularCaudalesEcologicos(double[][] caudales)
        {
            double[] caudalesEcologicos = new double[12];

            double [] qMM = CalculoMetodosCEs.ObtenerQmmFromCaudales(caudales);
            double [] qp50 = CalculoMetodosCEs.ObtenerPercentilCaudales(caudales, 50);
            double [] qp90 = CalculoMetodosCEs.ObtenerPercentilCaudales(caudales, 10);

            for (int i = 0; i < 12;  i++)
            {
                double factor = ((int)_tipoNMTM - 1) ;
                caudalesEcologicos[i] = qp90[i] +  factor / 9 * (qp50[i] - qp90[i]);
            }

            return caudalesEcologicos;
        }
    }
}
