using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo
{

    /// <summary>
    /// Dato unitario de caudal mensual con clasificación
    /// </summary>
    public class DatoAportacionMensualClasificada
    {
        public float Aportacion;
        public int Año;
        public int Mes;
        public TIPOAÑO TipodeAñoClasificado;

        public DatoAportacionMensualClasificada(float caudal, int año, int mes)
        {
            Aportacion = caudal;
            Año = año;
            Mes = mes;
            TipodeAñoClasificado = TIPOAÑO.NotSet;
        }
    }

    /// <summary>
    /// Colección de caudales que pertenece a un mes específico, con todos los años de la serie para ese mes
    /// </summary>
    public class SerieAportacionMensualClasificada
    {
        public List<DatoAportacionMensualClasificada> DatosPorAños;

        internal float GetAportacion(int año, int mes)
        {
            if (mes != Mes) throw new KeyNotFoundException("Mes incorrecto");
            IEnumerable<DatoAportacionMensualClasificada> tmp=DatosPorAños.Where(x => x.Año==año);
            if(tmp.Count()==0) throw new KeyNotFoundException("Año no encontrado");
            return tmp.First().Aportacion;
        }

        /// <summary>
        /// Mes natural, base 1 (enero=1, Diciembre=12).
        /// </summary>
        public int Mes;
        /// <summary>
        /// Cuartil 25% de Weibull
        /// </summary>
        public float Q25;
        /// <summary>
        /// Cuartil 75% de Weibull
        /// </summary>
        public float Q75;

        public float MedianaHumeda;
        public float MedianaMedia;
        public float MedianaSeca;
        public float MedianaPonderada;
        public bool DistribucionAtipica;
        public string DistrAtipDescriptor;


        public SerieAportacionMensualClasificada(int mes)
        {
            Mes = mes+1;
            DatosPorAños = new List<DatoAportacionMensualClasificada>();
        }

        private void SortAportacionesPorMes()
        {
            DatosPorAños.Sort(delegate (DatoAportacionMensualClasificada x, DatoAportacionMensualClasificada y)
            {
                if (x.Aportacion == y.Aportacion) return 0;
                if (x.Aportacion < y.Aportacion) return 1;
                else return -1;
            });
        }

        /// <summary>
        /// Cálculo de cuartiles usando las aportaciones con distribución de Weibull (Equivalente a PERCENTILE.EXC de excel). 
        /// Se debe ejecutar este método para obtener los valores de las propiedades Q25 y Q75
        /// </summary>
        private void CalculateQuantile()
        {
            //Extraemos los datos de la estructura en una lista de flotantes, para el cálculo
            List<float> Datos = new List<float>();
            for (int x=0; x<DatosPorAños.Count;x++)
            {
                Datos.Add(DatosPorAños[x].Aportacion);
            }

            Q25 = Datos.QuantileCustom(.25, QuantileDefinition.Weibull);
            Q75 = Datos.QuantileCustom(.75, QuantileDefinition.Weibull); //Comprobar la ordenación que realiza esta función (Usar directo o complementario)

        }

        /// <summary>
        /// Método que Caracteriza los años en Húmedos, Medios y Secos usando los Cuartiles Q25 y Q75. 
        /// </summary>
        private void CharacterizeYears()
        {
            
            CalculateQuantile(); 
            for (int x = 0; x < DatosPorAños.Count; x++) //Asegurarnos que la clasificación la hace bien
            {
                if (DatosPorAños[x].Aportacion <= Q25) DatosPorAños[x].TipodeAñoClasificado = TIPOAÑO.SECO;
                else if (DatosPorAños[x].Aportacion >= Q75) DatosPorAños[x].TipodeAñoClasificado = TIPOAÑO.HUMEDO;
                else DatosPorAños[x].TipodeAñoClasificado = TIPOAÑO.MEDIO;
            }
        }

        /// <summary>
        /// Método que calcula las Medianas de los años caracterizados. Realiza todos los cálculos necesarios. Solo invocar cuando los datos estén compeltamente introducidos.
        /// Esta función, aparte de las medianas, también calcula los cuaqrtiles de weibull y caracteriza los datos dentro de la estructura.
        /// </summary>
        public void CalculateCharacterizedMedians()
        {

            CharacterizeYears();
            List<float> datHum = new List<float>();
            List<float> datMed = new List<float>();
            List<float> datSec = new List<float>();
            //List<float> datPond = new List<float>();

            //Calcular valores distribución típica
            int h = (int)Math.Round(DatosPorAños.Count * .25);
            int s = h;
            int m = DatosPorAños.Count - h - s;

            int countH = 0;
            int countM = 0;
            int countS = 0;

            foreach (DatoAportacionMensualClasificada cad in DatosPorAños)
            {
               

                switch (cad.TipodeAñoClasificado)
                {
                    case TIPOAÑO.HUMEDO:
                        datHum.Add(cad.Aportacion);
                       // datPond.Add(cad.Aportacion);
                        countH++;
                        break;
                    case TIPOAÑO.MEDIO:
                        datMed.Add(cad.Aportacion);
                       // datPond.Add(cad.Aportacion);
                        countM++;
                        break;
                    case TIPOAÑO.SECO:
                        datSec.Add(cad.Aportacion);
                       // datPond.Add(cad.Aportacion);
                        countS++;
                        break;
                    default: throw new InvalidOperationException("Se ha intentado definir la mediana de una lista de datos no caracterizados");
                }
            }

            if (countH != h) DistribucionAtipica = true;
            if (countS != s) DistribucionAtipica = true;
            if (countM != m) DistribucionAtipica = true;

            if (DistribucionAtipica) DistrAtipDescriptor = countH + "|" + countM + "|" + countS;


            MedianaHumeda = datHum.Median();
            MedianaMedia = datMed.Median();
            MedianaSeca = datSec.Median();
            MedianaPonderada = (MedianaHumeda * countH + MedianaMedia * countM + MedianaSeca * countS) / (countH + countM + countS);
            //if (float.IsNaN(MedianaHumeda)) MedianaHumeda = 0;
            //if (float.IsNaN(MedianaMedia)) MedianaMedia = 0;
        }
    }
}
