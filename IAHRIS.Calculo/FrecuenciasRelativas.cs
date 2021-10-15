using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo
{
    public class FrecuenciasRelativas
    {
        public float[] Minimos = new float[12];
        public float[] Maximos = new float[12];
        public FrecuenciasRelativas(List<SerieAportacionMensualClasificada> datos, int mesInicio)
        {
            List<float>[] maximuns = new List<float>[12];
            List<float>[] minimuns = new List<float>[12];

            for (int x = 0; x < 12; x++)
            {
                maximuns[x] = new List<float>();
                minimuns[x] = new List<float>();
            }
            
            int añoinicio = datos[mesInicio-1].DatosPorAños[0].Año;
            int añofin = datos[11].DatosPorAños.Last().Año;
            for (int a = añoinicio; a < añofin + 1; a++)
            {
                bool hueco = false;
                float[] añodatos = new float[12];
                for (int x = 0; x < 12; x++)
                {
                    if (((x + mesInicio - 1) % 12) < mesInicio-1)
                    {
                        try
                        {
                            añodatos[x] = datos[((x + mesInicio - 1) % 12)].DatosPorAños.Where(y => y.Año == a + 1).First().Aportacion;
                        }
                        catch {
                            hueco = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            añodatos[x] = datos[((x + mesInicio - 1) % 12)].DatosPorAños.Where(y => y.Año == a).First().Aportacion;
                        }
                        catch {
                            hueco = true;
                        }
                    }
                }
                if (hueco) continue;
                float max = 0;
                float min = float.MaxValue;
                int minmonth = 0;
                int maxmonth = 0;
                for (int m = 0; m < 12; m++)
                {
                    float dat = añodatos[m];
                    if (dat > max)
                    {
                        max = dat;
                        maxmonth = m;
                    }
                    if (dat < min)
                    {
                        min = dat;
                        minmonth = m;
                    }
                }
                maximuns[maxmonth].Add(max);
                minimuns[minmonth].Add(min);
            }

            int años = añofin - añoinicio+1;
            for (int x = 0; x < 12; x++)
            {
                Minimos[x] = (float)minimuns[x].Count() / años;
                Maximos[x] = (float)maximuns[x].Count() / años;
            }
        }
    }
}
