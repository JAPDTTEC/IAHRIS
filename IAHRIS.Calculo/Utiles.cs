using IAHRIS.BBDD;
using MultiLangXML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS.Calculo
{
    public static class Utiles
    {


        //Devuelve el literal de un mes en el idioma escogido
        //los meses iran desde 1: Enero a 12:Diciembre
        public static string ObtenerMes(int mes)
        {
            Form form = new Form(); 
            MultiIdiomasXML _traductor = new MultiIdiomasXML(ref form);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");

            return _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (mes).ToString());         

        }


        //Devuelve el literal de un informe XML recibido en el idioma del conf.
        public static string ObtenerNombreInforme(string nombreInformeXML)
        {
            Form form = new Form();
            MultiIdiomasXML _traductor = new MultiIdiomasXML(ref form);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");

            return _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, nombreInformeXML);

        }



        public static float InterpolarMes(int año, int mes, int idLista, OleDbDataBase cMDB)
        {
            try
            {
       
                DateTime fechai = new DateTime(año, mes, 1);
                DateTime fechaf = new DateTime(fechai.Year, fechai.Month, 1).AddMonths(1).AddDays(-1);

                //obtener valores diarios del mes recibido
                DataSet ds = cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + idLista +
                                                             " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") +
                                                             "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");


                // Hay que leer de la BBDD y interpolar
                float acum = 0f;
                DateTime fechaDia;
                float valor;

                //Recorro todos los valores del mes.
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    fechaDia = DateTime.Parse(dr["fecha"].ToString());
                    valor = float.Parse(dr["valor"].ToString());

                    acum += valor;
                }



                return 86400f * acum / 1000000f;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static float ObtenerValorMes(int año, int mes, int idLista, OleDbDataBase cMDB)
        {

            try
            {

                DateTime fechai = new DateTime(año, mes, 1);
                DateTime fechaf = fechai.AddMonths(1);

                //obtener valores del mes recibido
                DataSet ds = cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + idLista +
                                                             " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") +
                                                             "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");


                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    return float.Parse(ds.Tables[0].Rows[0]["Valor"].ToString());

                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Obtiene las aportaciones para la idLista recibida.
        //Si contiene valores diarios, los interpola para obtener la aportación mensual.
        public static List<double[]> ObtenerAportacion(Simulacion sim, EstadoLista lista, int mesInicio, int idLista, bool isNAT)
        {

            List<double[]> resultado = new List<double[]>();
            int cont = 0;
            string _RutaBBDD = Application.StartupPath + @"\IAHRISv2.mdb";
            OleDbDataBase _cMDB = new OleDbDataBase("Base", _RutaBBDD);

            try
            {
                int[] añosInterpolables = null;
                if (isNAT)
                {
                    if (sim.añosInterNat != null) añosInterpolables = sim.añosInterNat;
                }
                else
                    if (sim.añosInterAlt != null) añosInterpolables = sim.añosInterAlt;


                //Años
                for (int año = lista.Año[0]; año <= lista.Año[lista.Año.Length - 1]; año++, cont++)
                {
                    //Si el año no es valido, pasamos al siguiente.
                    if (!lista.validos[cont])
                        continue;


                    //Creo el año hidrologico.
                    DateTime fechaIni = new DateTime(año, mesInicio, 1);
                    DateTime fechaFin = fechaIni.AddYears(1).AddDays(-1);

                    //Recorro año hidrologico.
                    while (fechaIni < fechaFin)
                    {
                        double[] aport = new double[3];
                        aport[0] = fechaIni.Year;
                        double valor = 0;
                        if (isNAT)
                        {
                            if (añosInterpolables != null && sim.añosInterNat != null && sim.añosInterNat.Contains(año))
                                valor = InterpolarMes(fechaIni.Year, fechaIni.Month, idLista, _cMDB);
                            else
                                valor = ObtenerValorMes(fechaIni.Year, fechaIni.Month, idLista, _cMDB);
                        }
                        else
                        {
                            if (añosInterpolables != null && sim.añosInterAlt != null && sim.añosInterAlt.Contains(año))
                                valor = InterpolarMes(fechaIni.Year, fechaIni.Month, idLista, _cMDB);
                            else
                                valor = ObtenerValorMes(fechaIni.Year, fechaIni.Month, idLista, _cMDB);
                        }
                        aport[1] = fechaIni.Month;
                        aport[2] = Math.Round(valor, 3);

                        resultado.Add(aport);
                        fechaIni = fechaIni.AddMonths(1);

                    }

                }
                _cMDB.Desconectar();
                return resultado;


            }
            catch (Exception ex) 
            {
                _cMDB.Desconectar();
                throw ex;
            }   



        }

        public static SerieMensual ObtenerSerieMensualSinInterpolar (int mesInicial, int año , int idLista )
        {
            string _RutaBBDD = Application.StartupPath + @"\IAHRISv2.mdb";
            OleDbDataBase _cMDB = new OleDbDataBase("Base", _RutaBBDD);
            SerieMensual serieMensual = new SerieMensual();


            try
            {
                _cMDB.AbrirConexion();

                // Definir el año hidrológico
                int mesfinal = mesInicial - 1;
                int añofinal = año + 1;

                if (mesInicial == 1)
                {
                    mesfinal = 12;
                    añofinal = año;
                }

                DateTime fechai = new DateTime(año, mesInicial, 1);
                DateTime fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));

                DataSet ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + idLista +
                                                                                    " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") +
                                                                                    "# AND #" + fechaf.ToString("yyyy-MM-dd") +
                                                                                    "# ORDER BY fecha ASC");

                _cMDB.Desconectar();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (serieMensual.mes is null)
                    {
                        serieMensual.caudalMensual = new float[1];
                        serieMensual.mes = new DateTime[1];
                    }
                    else
                    {
                        Array.Resize(ref serieMensual.caudalMensual, serieMensual.caudalMensual.Length + 1);
                        Array.Resize(ref serieMensual.mes, serieMensual.mes.Length + 1);
                    }

                    serieMensual.nMeses = serieMensual.nMeses + 1;
                    serieMensual.caudalMensual[serieMensual.caudalMensual.Length - 1] = float.Parse(dr["valor"].ToString());
                    serieMensual.mes[serieMensual.mes.Length - 1] = DateTime.Parse(dr["fecha"].ToString());
                }

                return serieMensual;

            }
            catch (Exception ex)
            {
                _cMDB.Desconectar();
                throw ex;
            }

        }


        public static SerieDiaria ObtenerSerieDiaria(int mesInicial, int año, int idLista)
        {
            string _RutaBBDD = Application.StartupPath + @"\IAHRISv2.mdb";
            OleDbDataBase _cMDB = new OleDbDataBase("Base", _RutaBBDD);
            SerieDiaria serieDiaria = new SerieDiaria();


            try
            {
                _cMDB.AbrirConexion();

                // Definir el año hidrológico
                int mesfinal = mesInicial - 1;
                int añofinal = año + 1;

                if (mesInicial == 1)
                {
                    mesfinal = 12;
                    añofinal = año;
                }

                DateTime fechai = new DateTime(año, mesInicial, 1);
                DateTime fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));

                DataSet ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + idLista + 
                                                                                    " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + 
                                                                                    "# AND #" + fechaf.ToString("yyyy-MM-dd") 
                                                                                    + "# ORDER BY fecha ASC");


                _cMDB.Desconectar();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (serieDiaria.dia is null)
                    {
                        serieDiaria.caudalDiaria = new float[1];
                        serieDiaria.dia = new DateTime[1];
                    }
                    else
                    {
                        Array.Resize(ref serieDiaria.caudalDiaria, serieDiaria.caudalDiaria.Length + 1);
                        Array.Resize(ref serieDiaria.dia, serieDiaria.dia.Length + 1);
                    }

                    serieDiaria.caudalDiaria[serieDiaria.caudalDiaria.Length - 1] = float.Parse(dr["valor"].ToString());
                    serieDiaria.dia[serieDiaria.caudalDiaria.Length - 1] = DateTime.Parse(dr["fecha"].ToString());
                }

                return serieDiaria;

            }
            catch (Exception ex)
            {
                _cMDB.Desconectar();
                throw ex;
            }

        }
    }
}
