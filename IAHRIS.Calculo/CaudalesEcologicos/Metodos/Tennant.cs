using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using System.Linq;

namespace IAHRIS.Calculo.CaudalesEcologicos.Escenarios
{
    public class Tennant : IMetodo
    {
        public readonly EscenarioDTO _teenant;
        public string Nombre { get; set; }
        public Tennant() 
        {
            Nombre = "TENNANT";
        }



        /// <summary>
        /// Calcula los Caudales Ecologicos por cada mes, a partir de 
        /// un array con los caudales Medios mensuales utlilizando el metodo
        /// de Tennant
        /// </summary>
        /// <param name="caudalesMensuales">Caudales medios mensuales.</param>
        /// <returns></returns>
        public double[] CalcularCaudalesEcologicos(double[][] caudales)
        {

            //Obtener array con caudales mensuales
            double[] caudalesEcologicos = new double[12];
            double[] qMM = new double[12];

            int mes = 0;

            qMM = CalculoMetodosCEs.ObtenerQmmFromCaudales(caudales);
            double QMan = qMM.Average();


            //Comparamos QMM > QMAn Si Ok -> A Sino -> B
            foreach (double valorQMM in qMM)
            {
                //Para A: RCE= 0.4 * QMan     Para B: RCE= 0.2 * QMan
                if (valorQMM > QMan)
                    caudalesEcologicos[mes] = 0.4 * QMan;
                else
                    caudalesEcologicos[mes] = 0.2 * QMan;

                mes++;
            }

            return caudalesEcologicos;
        }
    }
}
