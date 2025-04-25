using MathNet.Numerics.Statistics;
using System;
using System.Linq;

namespace IAHRIS.Calculo.CaudalesEcologicos.Metodos
{
    public static class CalculoMetodosCEs
    {

        public static double[] ObtenerQmmFromCaudales(double[][] caudales)
        {
            double[] qMM = new double[12];
            double[] caudalesMensuales = new double[12];
            double mesIni = caudales[0][1];

            for (int i = 0; i < 12; i++)
            {
                if (mesIni > 12) mesIni = 1;

                caudalesMensuales[i] = caudales.Where(x => x[1] == mesIni).Sum(v => v[2]);
                qMM[i] = Math.Round( caudales.Where(x => x[1] == mesIni).Average(v => v[2]), 3 );

                mesIni++;
            }

            return qMM;
        }



        public static double[] ObtenerPercentilCaudales(double[][] caudales, double percentil)
        {
            double mesIni = caudales[0][1];
            double[] percentilMensuales = new double[12];

            for (int i = 0; i < 12; i++)
            {
                if (mesIni > 12) mesIni = 1;

                double[] caudalesMensuales = caudales.Where(x => x[1] == mesIni).Select(x => x[2]).ToArray();
                percentilMensuales[i] = Math.Round ( caudalesMensuales.QuantileCustom((percentil/100), QuantileDefinition.Weibull), 3);
                
                mesIni++;
            }

            return percentilMensuales;
        }


        public static double[] ObtenerCaudalMinimo(double[][] caudales)
        {
            double mesIni = caudales[0][1];
            double[] caudalesMinimos = new double[12];

            for (int i = 0; i < 12; i++)
            {
                if (mesIni > 12) mesIni = 1;

                double[] caudalesMensuales = caudales.Where(x => x[1] == mesIni).Select(x => x[2]).ToArray();
                caudalesMinimos[i] = Math.Round(caudalesMensuales.Min(), 3);

                mesIni++;
            }

            return caudalesMinimos;
        }




        /// <summary>
        /// Obtener caudal a partir de la aportación recibida.
        /// </summary>
        /// <param name="aportacion"></param>
        /// <param name="año"></param>
        /// <param name="mes">mes correspondiente. Enero será el mes 1 y diciembre el 12</param>
        /// <returns></returns>
        public static double ObtenerCaudalFromAportacion(double aportacion, int año,  int mes)
        {
            double factor = DateTime.DaysInMonth(año, mes) * 3600 * 24;
            double caudal = aportacion * Math.Pow(10, 6) / factor;

            return Math.Round (caudal, 3);
        }

        /// <summary>
        /// Obtener Aportación desde un Caudal.
        /// </summary>
        /// <param name="caudal"></param>
        /// <param name="año"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static double ObtenerAportacionFromCaudal (double caudal, int año, int mes) 
        {
            double factor = DateTime.DaysInMonth(año, mes) * 3600 * 24;
            double aportacion = factor * caudal / Math.Pow(10, 6);

            return Math.Round( aportacion, 3);
        }



        public static double[][] ObtenerCoefVariabInteranual(double[][] caudalesMedios, int mesInicial)
        {


            int mesFinalSerie = mesInicial == 1 ? 12 : mesInicial - 1;
            double[] añosSeries = caudalesMedios.Select(x => x[0]).ToList().Distinct().ToArray();
            double[][] cvInteranual = new double[caudalesMedios.Length][];
            int fila = 0;

            foreach (double añoSerie in añosSeries)
            {

                int añoFinal = mesInicial == 1 ? (int)añoSerie : (int)añoSerie + 1;
                double[][] series = caudalesMedios.Where(x => (x[0] == añoSerie && x[1] >= mesInicial) ||
                                                            (x[0] == añoFinal && x[1] <= mesFinalSerie)).ToArray();

                if (series.Select(x => x[2]).Count() == 0)
                    continue;

                double min = series.Select(x=> x[2]).Min();

                foreach (double[]serie in series)
                {
                    double val = Math.Round( serie[2] / min , 3);
                    cvInteranual[fila] = new double[] { serie[0], serie[1], val };
                    fila++;
                }

            }
                
            return cvInteranual;


        }
        public static double AportacionAnualFromCaudalEco(int mesInicial, double[] caudalEco)
        {
            double aportacionAnual = 0;
            int mes = mesInicial;
            for (int i = 1; i < 13; i++)
            {
                if (mes == 13) mes = 1;

                switch (mes)
                {
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        //aportacionAnual += Math.Round(3600 * 24 * 30 * caudalEco[i - 1] / Math.Pow(10, 6), 3);
                        aportacionAnual += 3600 * 24 * 30 * caudalEco[i - 1] / Math.Pow(10, 6);
                        //Meses de 30 dias
                        break;
                    case 2:
                        //aportacionAnual += Math.Round(3600 * 24 * 28 * caudalEco[i - 1] / Math.Pow(10, 6), 3);
                        aportacionAnual += 3600 * 24 * 28 * caudalEco[i - 1] / Math.Pow(10, 6);
                        // Considerando un año tipo.
                        break;
                    default:
                        //aportacionAnual += Math.Round(3600 * 24 * 31 * caudalEco[i - 1] / Math.Pow(10, 6), 3);
                        aportacionAnual += 3600 * 24 * 31 * caudalEco[i - 1] / Math.Pow(10, 6);
                        // Meses de 31 dias
                        break;

                }

                mes++;
            }

            return aportacionAnual;
        }
    }
}
