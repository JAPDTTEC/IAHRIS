using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo
{
    /// <summary>
    /// Clase que realiza los cálculos de IAH2 a nivel Mensual, con clasificación mensual
    /// </summary>
    public class IAH2
    {
        List<SerieIAH2Mensual> DatosMensuales;

        public List<SerieIAH2Mensual> Mensual
        {
            get { return DatosMensuales; }
        }

        public IAH2Result IAH2Hum;
        public IAH2Result IAH2Med;
        public IAH2Result IAH2Sec;
        public IAH2Result IAH2Pond;
        /// <summary>
        /// Constructor de la clase de cálculos de IAH2
        /// </summary>
        /// <param name="datosOrigen">datos mensuales tipificados</param>
        public IAH2(List<SerieAportacionMensualClasificada> datosNat, List<SerieAportacionMensualClasificada> datosAlt)
        {
            DatosMensuales = new List<SerieIAH2Mensual>();
            for(int x=0; x<12; x++)
            {
                DatosMensuales.Add(new SerieIAH2Mensual(x+1));
            }

            foreach(SerieAportacionMensualClasificada samc in datosNat)
            {
                foreach(DatoAportacionMensualClasificada damc in samc.DatosPorAños)
                {
                    float aportAlt = 0;
                    IEnumerable<SerieAportacionMensualClasificada> tmp= datosAlt.Where(x => x.Mes == damc.Mes && x.DatosPorAños.Where(y => y.Año == damc.Año).Count() > 0);
                    aportAlt= tmp.First().DatosPorAños.Where(y => y.Año == damc.Año).First().Aportacion;
                    AddData(damc.Año, damc.Mes, damc.TipodeAñoClasificado, damc.Aportacion, aportAlt);
                }
            }
        }

        private void AddData(int año, int mes, TIPOAÑO tipodeAñoClasificado, float aportacion, float aportAlt)
        {
            DatosMensuales[mes - 1].AddData(año, mes, tipodeAñoClasificado, aportacion, aportAlt);
        }
        public void Calculate()
        {
            List<float> Hum = new List<float>();
            bool InvHum = false;
            bool IndHum = false;
            List<float> Med = new List<float>();
            bool InvMed = false;
            bool IndMed = false;
            List<float> Sec = new List<float>();
            bool InvSec = false;
            bool IndSec = false;

            List<float> Pond = new List<float>();
            bool AtipPond = false;

            foreach (SerieIAH2Mensual sim in DatosMensuales)
            {
                sim.Calculate();


                if (sim.IAH2Hum.Indeterminado)
                {
                    IndHum = true;
                }
                else
                {
                    Hum.Add(sim.IAH2Hum.IAH2Ratio);
                }
                if (sim.IAH2Hum.Inverso) InvHum = true;

                if (sim.IAH2Med.Indeterminado)
                {
                    IndMed = true;
                }
                else
                {
                    Med.Add(sim.IAH2Med.IAH2Ratio);
                }
                if (sim.IAH2Med.Inverso) InvMed = true;

                if (sim.IAH2Sec.Indeterminado)
                {
                    IndSec = true;
                }
                else
                {
                    Sec.Add(sim.IAH2Sec.IAH2Ratio);
                }
                if (sim.IAH2Sec.Inverso) InvSec = true;

                if (!sim.IAH2Pond.NoCalculado)
                {
                    Pond.Add(sim.IAH2Pond.IAH2Ratio);
                    if (sim.IAH2Pond.DistribucionAtipica) AtipPond = true;

                }
            
            }

            IAH2Pond.IAH2Ratio = (float)Pond.Mean();
            IAH2Pond.DistribucionAtipica = AtipPond;

            IAH2Hum.IAH2Ratio = (float)Hum.Mean();
            if (IndHum) IAH2Hum.Indeterminado = true;
            if (InvHum) IAH2Hum.Inverso = true;

            IAH2Med.IAH2Ratio = (float)Med.Mean();
            if (IndMed) IAH2Med.Indeterminado = true;
            if (InvMed) IAH2Med.Inverso = true;

            IAH2Sec.IAH2Ratio = (float)Sec.Mean();
            if (IndSec) IAH2Sec.Indeterminado = true;
            if (InvSec) IAH2Sec.Inverso = true;
        }
    }

    public struct IAH2Result
    {
        public float IAH2Ratio;
        public bool Inverso;
        public bool Indeterminado;
        public bool NoCalculado;
        public bool DistribucionAtipica;
    }

    /// <summary>
    /// Colección de caudales que pertenece a un mes específico, con todos los años de la serie para ese mes
    /// </summary>
    public class SerieIAH2Mensual
    {
        public List<DatoIAH2Mensual> DatosPorAños;
        /// <summary>
        /// Mes natural, base 1 (enero=1, Diciembre=12).
        /// </summary>
        public int Mes;

        public IAH2Result IAH2Hum;
        public IAH2Result IAH2Med;
        public IAH2Result IAH2Sec;
        public IAH2Result IAH2Pond;

        public SerieIAH2Mensual(int mes)
        {
            Mes = mes + 1;
            DatosPorAños = new List<DatoIAH2Mensual>();
        }

        internal void AddData(int año, int mes, TIPOAÑO tipodeAñoClasificado, float aportacion, float aportAlt)
        {
            DatosPorAños.Add(new DatoIAH2Mensual(aportacion, aportAlt, año, mes, tipodeAñoClasificado));
        }

        internal void Calculate()
        {
            List<float> Hum = new List<float>();
            bool InvHum = false;
            bool IndHum = false;
            List<float> Med = new List<float>();
            bool InvMed = false;
            bool IndMed = false;
            List<float> Sec = new List<float>();
            bool InvSec = false;
            bool IndSec = false;

            float Pond = 0;

            int countH = 0;
            int countM = 0;
            int countS = 0;

            foreach (DatoIAH2Mensual diam in DatosPorAños)
            {
                diam.Calculate();
                switch(diam.TipodeAñoClasificado)
                {
                    case TIPOAÑO.HUMEDO:
                        if (diam.IAH2.Indeterminado)
                        {
                            IndHum = true;
                        }
                        else
                        {
                            Hum.Add(diam.IAH2.IAH2Ratio);
                        }
                        countH++;
                        if (diam.IAH2.Inverso) InvHum = true;
                        break;
                    case TIPOAÑO.MEDIO:
                        if (diam.IAH2.Indeterminado)
                        {
                            IndMed = true;
                        }
                        else
                        {
                            Med.Add(diam.IAH2.IAH2Ratio);
                        }
                        countM++;
                        if (diam.IAH2.Inverso) InvMed = true;
                        break;
                    case TIPOAÑO.SECO:
                        if (diam.IAH2.Indeterminado)
                        {
                            IndSec = true;
                        }
                        else
                        {
                            Sec.Add(diam.IAH2.IAH2Ratio);
                        }
                        countS++;
                        if (diam.IAH2.Inverso) InvSec = true;
                        break;
                }


            }

            //Calcular valores distribución típica
            int h =(int) Math.Round(DatosPorAños.Count * .25);
            int s = h;
            int m = DatosPorAños.Count - h - s;

           

            if (Hum.Count == 0) IAH2Hum.NoCalculado = true;
            else
            {
                IAH2Hum.IAH2Ratio = (float)Hum.Mean();
                if (InvHum) IAH2Hum.Inverso = true;
                if (IndHum) IAH2Hum.Indeterminado = true;
                Pond += IAH2Hum.IAH2Ratio*Hum.Count;
            }
            if (Med.Count == 0) IAH2Med.NoCalculado = true;
            else
            {
                IAH2Med.IAH2Ratio = (float)Med.Mean();
                if (InvMed) IAH2Med.Inverso = true;
                if (IndMed) IAH2Med.Indeterminado = true;
                Pond += IAH2Med.IAH2Ratio * Med.Count;
            }
            if (Sec.Count == 0) IAH2Sec.NoCalculado = true;
            else
            {
                IAH2Sec.IAH2Ratio = (float)Sec.Mean();
                if (InvSec) IAH2Sec.Inverso = true;
                if (IndSec) IAH2Sec.Indeterminado = true;
                Pond += IAH2Sec.IAH2Ratio * Sec.Count;
            }

            if (countH != h) IAH2Pond.DistribucionAtipica = true;
            if (countS!= s) IAH2Pond.DistribucionAtipica = true;
            if (countM!= m) IAH2Pond.DistribucionAtipica = true;

            Pond = Pond / DatosPorAños.Count;
            IAH2Pond.IAH2Ratio = Pond;
        }
    }
    /// <summary>
    /// Dato unitario de caudal mensual con clasificación
    /// </summary>
    public class DatoIAH2Mensual
    {
        public float AportacionNatural;
        public float AportacionAlterada;
        public int Año;
        public int Mes;
        public TIPOAÑO TipodeAñoClasificado;

        public IAH2Result IAH2;

        public DatoIAH2Mensual(float caudalNatural, float caudalAlterado, int año, int mes, TIPOAÑO tipodeAñoClasificado)
        {
            AportacionNatural = caudalNatural;
            AportacionAlterada = caudalAlterado;
            Año = año;
            Mes = mes;
            TipodeAñoClasificado = tipodeAñoClasificado;
        }

        internal void Calculate()
        {
            if (AportacionAlterada == AportacionNatural)
            {
                IAH2.IAH2Ratio = 1;
            }
            else if (AportacionNatural == 0)
            {
                IAH2.IAH2Ratio = float.NaN;
                IAH2.Indeterminado = true;
            }
            else if (AportacionNatural > AportacionAlterada)
            {
                IAH2.IAH2Ratio = AportacionAlterada / AportacionNatural;
            }
            else
            {
                IAH2.IAH2Ratio = AportacionNatural / AportacionAlterada;
                IAH2.Inverso = true;
            }
        }
    }
}
