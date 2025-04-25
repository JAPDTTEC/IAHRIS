using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using System.Linq;

namespace IAHRIS.Calculo.CaudalesEcologicos.Escenarios
{
    public class VMF : IMetodo
    {
        public readonly EscenarioDTO _vmf;
        public string Nombre { get; set; }
        public VMF () 
        {
            Nombre = "VMF";
        }

        override
        public string ToString()
        {
            return _vmf.Nombre.ToString();
        }


        /// <summary>
        /// Calcula los Caudales Ecologicos por cada mes, a partir de 
        /// un array con los caudales Medios mensuales utlilizando el método
        /// de VMF
        /// </summary>
        /// <param name="caudalesMensuales">Caudales medios mensuales.</param>
        /// <returns></returns>
        public double[] CalcularCaudalesEcologicos(double[][] caudales)
        {
            double[] caudalesEcologicos = new double[12];
            double[] qMM = new double[12];
            int mes = 0;

            //Calcular el caudal medio anual QMAn
            qMM = CalculoMetodosCEs.ObtenerQmmFromCaudales(caudales);
            double QMan = qMM.Average();


            //Si QMM > 0.8 * QMAn->RCE = 0.3 * QMM
            //Si QMM > 0.4 * QMAn Y 0,4 * QMM <= 0.8QMAn->RCE = 0.45 * QMM
            //Si QMM <= 0.4 * QMAn->RCE = 0.6 * QMM
            foreach (double valorQMM in qMM)
            {
                if (valorQMM > 0.8 * QMan)
                    caudalesEcologicos[mes] = 0.3 * valorQMM;

                if ((valorQMM > 0.4 * QMan) && (valorQMM <= 0.8 * QMan))
                    caudalesEcologicos[mes] = 0.45 * valorQMM;

                if ((valorQMM <= 0.4 * QMan))
                    caudalesEcologicos[mes] = 0.6 * valorQMM;

                mes++;
            }

            return caudalesEcologicos;

        }
    }
}