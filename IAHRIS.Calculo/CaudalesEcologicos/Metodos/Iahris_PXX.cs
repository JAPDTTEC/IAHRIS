using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System;
using System.Linq;

namespace IAHRIS.Calculo.CaudalesEcologicos.Metodos
{
    public class Iahris_PXX : IMetodo
    {

        public int Percentil { get; set; }
        public int MesInicio { get; set; }
        public string Nombre { get; set; }

        //si se inicializa sin indicar percentil , por defecto se crea en 50
        public Iahris_PXX() 
        {
            Percentil = 50;
            MesInicio = 10;
        }

        public Iahris_PXX(int percentil, int mesInicio)
        {
            Percentil = percentil;
            MesInicio = mesInicio;
            Nombre = "IAHRIS_P" + percentil.ToString();
        }


        public double[] CalcularCaudalesEcologicos(double[][] caudales)
        {
  
            double[] p50Mes = new double[12];
            double[] p95Mes = new double[12];
            double[] p90Mes = new double[12];
            double[] pPercentil = new double[12];
            double[] qMin = new double[12];
            int mes = MesInicio;

            for (int i = 0; i < 12; i++) 
            {
                if (mes > 12) mes = 1;

                p50Mes[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().QuantileCustom(.50, QuantileDefinition.Weibull).Round(3);
                p95Mes[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().QuantileCustom(.05, QuantileDefinition.Weibull).Round(3);
                pPercentil[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().QuantileCustom(((double)100 - Percentil)/100, QuantileDefinition.Weibull).Round(3);
                p90Mes[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().QuantileCustom(.1, QuantileDefinition.Weibull).Round(3);
                qMin[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().Min().Round(3);

                mes++;
            }

            double promedio = p50Mes.Average().Round(3);
            double promedioPercentil = pPercentil.Average().Round(3);
            double factor = Math.Min(p50Mes.Min(), promedio).Round(3);

            //Calculo de los factores de variabilidad (FV) y del factor de reducción (K)
            double[] factoresVariabilidadEC1 = new double[12];
            double[] factoresVariabilidadEC2 = new double[12];

            for (int i =0; i < 12; i++)
            {
                factoresVariabilidadEC1[i] = (p50Mes[i] / factor).Round(3);
                factoresVariabilidadEC2[i] = (Math.Pow(factoresVariabilidadEC1[i], (1 / 1.1))).Round(3);
            }

            double media = factoresVariabilidadEC2.Average().Round(3);

            double kPercentil = (promedioPercentil / (factor * media)).Round(3);

            double[] resultadoProv = new double[12];
            double[] resultado = new double[12];

            for (int i = 0; i < 12; i++)
            {
                resultadoProv[i] = (factor * factoresVariabilidadEC2[i] * kPercentil).Round(3);
                resultado[i] = (resultadoProv[i] > qMin[i] ? resultadoProv[i] : p95Mes[i]).Round(3);
            }


            return resultado;
        }


    }
}
