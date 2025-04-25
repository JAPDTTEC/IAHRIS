using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using System.Linq;

namespace IAHRIS.Calculo.CaudalesEcologicos.Escenarios
{
    public class Tessman : IMetodo
    {
        public readonly EscenarioDTO _tessman;
        public string Nombre { get; set; }
        public Tessman () 
        {
            Nombre = "TESSMAN";
        }

   

        /// <summary>
        /// Calcula los Caudales Ecologicos por cada mes, a partir de 
        /// un array con los caudales Medios mensuales utlilizando el método
        /// de Tessman
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

            //Si QMM > 0.4*QMAn Y 0,4*QMM > 0.4QMAn  -> RCE= 0.4 * QMM
            //Si QMM > 0.4*QMAn Y 0,4*QMM <= 0.4QMAn -> RCE= 0.4 * QMan
            //Si QMM <= 0.4*QMAn                     -> RCE= QMM
            foreach (double valorQMM in qMM)
            {
                if ((valorQMM > 0.4 * QMan) && (0.4 * valorQMM > 0.4 * QMan))
                    caudalesEcologicos[mes] = 0.4 * valorQMM;

                if ((valorQMM > 0.4 * QMan) && (0.4 * valorQMM <= 0.4 * QMan))
                    caudalesEcologicos[mes] = 0.4 * QMan;

                if ((valorQMM <= 0.4 * QMan))
                    caudalesEcologicos[mes] = valorQMM;

                mes++;
            }

            return caudalesEcologicos;

        }
    }
}
