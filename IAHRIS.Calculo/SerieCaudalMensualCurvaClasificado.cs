using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo
{
    public class SerieCaudalMensualCurvaClasificado
    {
        public List<SerieMensualCaudalDiario> SerieEstructurada;

        public SerieCaudalMensualCurvaClasificado()
        {
            SerieEstructurada = new List<SerieMensualCaudalDiario>();
            for(int x=0;x<12;x++)
            {
                SerieEstructurada.Add(new SerieMensualCaudalDiario(x));
            }
        }

        public void AddData(DateTime fecha, float caudal)
        {
            if (fecha.Month == 2 && fecha.Day == 29) return; //Eliminamos el 29 de Febrero
            SerieEstructurada[fecha.Month - 1].AddData(fecha, caudal);
        }

        public void Calcular()
        {
            foreach(SerieMensualCaudalDiario smcd in SerieEstructurada)
            {
                smcd.GetMedias();
            }
        }

    }

    public class SerieClasificadaPorK
    {
        public double Media;
        public int Mes;
        public int Año;
        public int K;

        public SerieClasificadaPorK(double media, int mes, int k)
        {
            Media = media;
            Mes = mes;
            K = k;
        }
    }
    public class SerieClasificadaPorMes
    {
        public List<SerieClasificadaPorK> ValoresK;
        public int Mes;

        public SerieClasificadaPorMes(int mes)
        {
            Mes = mes;
            ValoresK = new List<SerieClasificadaPorK>();
        }
    }

    public class DatoCaudalDiario
    {
        public float Caudal;
        public DateTime Fecha;
        public int K;

        public DatoCaudalDiario(float caudal, DateTime fecha)
        {
            Caudal = caudal;
            Fecha = fecha;
        }
    }

    /// <summary>
    /// Clase que contiene todos los días de un mes de un determinado año.
    /// </summary>
    public class ListaAnualCaudal
    {
        public List<DatoCaudalDiario> SerieMensual;
        public int Año;

        public ListaAnualCaudal(int año)
        {
            Año = año;
            SerieMensual = new List<DatoCaudalDiario>();
        }
        internal void AddData(DateTime fecha, float caudal)
        {
            SerieMensual.Add(new DatoCaudalDiario(caudal, fecha));
        }
        internal void OrdenarSeriePorMagnitud()
        {
            this.SerieMensual.Sort(delegate (DatoCaudalDiario x, DatoCaudalDiario y)
            {
                if (x.Caudal == y.Caudal) return 0;
                if (x.Caudal < y.Caudal) return 1;
                else return -1;
            });
            //Establecer K en cada elemento (Coincide con el índice + 1)
            for (int x = 0; x < SerieMensual.Count; x++)
            {
                SerieMensual[x].K = x+1;
            }
        }

       
    }

    /// <summary>
    /// Serie que contiene todos los años de un determinado mes
    /// </summary>
    public class SerieMensualCaudalDiario
    {
        public List<ListaAnualCaudal> ListaAnual;
        public SerieClasificadaPorMes SerieClasificada;
        public int Mes;

        public SerieMensualCaudalDiario(int mes)
        {
            Mes = mes+1;
            ListaAnual = new List<ListaAnualCaudal>();
            SerieClasificada = new SerieClasificadaPorMes(Mes);
        }
        internal void AddData(DateTime fecha, float caudal)
        {
            ListaAnualCaudal tmpLista;
            List<ListaAnualCaudal> tmpyear = ListaAnual.Where(x => x.Año == fecha.Year).ToList();
            if(tmpyear.Count==0)
            {
                tmpLista = new ListaAnualCaudal(fecha.Year);
                ListaAnual.Add(tmpLista);
            }
            else
            {
                tmpLista = tmpyear[0];
            }

            tmpLista.AddData(fecha, caudal);
        }
        //Método que ordena los datos y obtienes los resultados, que se almacenan en SerieClasificada
        internal void GetMedias()
        {
            foreach(ListaAnualCaudal la in ListaAnual)
            {
                la.OrdenarSeriePorMagnitud();
            }
            for (int d = 0; d < ListaAnual[0].SerieMensual.Count; d++)
            {
                float[] lstMedia = new float[ListaAnual.Count];
                for (int a = 0; a < ListaAnual.Count; a++)
                {
                   
                        lstMedia[a] = ListaAnual[a].SerieMensual[d].Caudal;
                  
                }

                double tmpMedia = lstMedia.Mean();
                SerieClasificada.ValoresK.Add(new SerieClasificadaPorK(tmpMedia, Mes, d));
            }
        }

    }
}
