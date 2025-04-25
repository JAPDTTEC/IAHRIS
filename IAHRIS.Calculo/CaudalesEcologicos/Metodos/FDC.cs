using MathNet.Numerics.Statistics;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.CaudalesEcologicos.Metodos
{
    public class FDC : IMetodo
    {
        public string Nombre { get; set; }

        public double[] CalcularCaudalesEcologicos(double[][] caudales)
        {
            double[] pe = new double[caudales.Length];
            double[] qmmOrd = new double[caudales.Length];
            double[] caudalesOrd = caudales.Select(x => x[2]).ToArray();
            double[] kFormula = new double[] { 0, 0.01, 0.10, 1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 99.90, 99.99 };
            double[] percFDC = new double[kFormula.Length];

            Array.Sort(caudalesOrd);   

            for (int i = 1; i <= caudales.Length; i++)
            {
                pe[i - 1] = i * caudales.Length / caudales.Length;
                qmmOrd[i - 1] = caudalesOrd[i - 1];
            }
            
            for (int i = 3; i <= kFormula.Length; i++)
                percFDC[i] = qmmOrd.Percentile((int)kFormula[i]/100);//No usar Percentile. usar QuantileCustom(.75, QuantileDefinition.Weibull);

            //Este metodo no está terminado y actualmente no se usa. Si más adelante se decide usar Hay que sustituir el Percentile por QuantileCustom(tau, QuantileDefinition.Weibull);

            //fase 2


            return null;
        }
    }
}
