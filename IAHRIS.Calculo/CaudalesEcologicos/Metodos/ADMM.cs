using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IAHRIS.Calculo.CaudalesEcologicos.Metodos
{
    public class ADMM : IMetodo
    {

        public int Mes_Inicio { get; set; }
        public string Nombre { get; set; }

        public ADMM() 
        {
            Mes_Inicio = 10;
        }   

        public ADMM(int mesInicio)
        {
            Mes_Inicio = mesInicio;
            Nombre = "ADMM";
        }



        public double[] CalcularCaudalesEcologicos(double[][] caudales)
        {
            try
            {
                double[] qMM = new double[12];
                double[] p95 = new double[12];
                double[] qMin = new double[12];
                double qMM_min = 0.0;
                int mes = Mes_Inicio;

                for (int i = 0; i < 12; i++)
                {
                    if (mes > 12) mes = 1;

                    qMM[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().Average().Round(3); 
                    p95[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().QuantileCustom(.05, QuantileDefinition.Weibull).Round(3); 
                    qMin[i] = caudales.Where(x => x[1] == mes).Select(x => x[2]).ToList().Min().Round(3);


                    //Se coge un año tipo de ejemplo (1990), que no sea bisiesto, que devuelva 31, 30 o 28 días según el mes.
                    //si se quiere ajustar mas, añadir Año del caudal en propiedades, para que calcule con 29 dias si el año es bisiesto.
                    qMM_min += DateTime.DaysInMonth(1990, mes) * qMin[i];

                    mes++;
                }

                double qMM_media = qMM.Average().Round(3);
                qMM_min = (qMM_min / 365).Round(3);

                double factorReduccion = (qMM_min / qMM_media).Round(3);

                double[] resultadoProv = new double[12];
                double[] resultado = new double[12];

                for (int i = 0; i < 12; i++)
                {
                    resultadoProv[i] = qMM[i] * factorReduccion;
                    resultado[i] = resultadoProv[i] > qMin[i] ? Math.Round(resultadoProv[i], 3) : Math.Round(p95[i], 3);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
