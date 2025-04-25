using IAHRIS.BBDD;
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Calculo.CaudalesEcologicos.Informes;
using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using IAHRIS.Calculo.IndicesHidro.Informes;
using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS.Calculo.CaudalesEcologicos.Tipologias
{
    public class TipologiaCE 
    {      
        public string Nombre;
        public enumTipologiasCE Tipologia;
        public static List<Informe_RCE> Informes;

        static readonly List<TipologiaCE> ListaTipologias = new List<TipologiaCE>();
        
        public enum enumTipologiasCE
        {
            NONE,
            Tipom1,
            Tipo0,
            Tipo0A,
            Tipo0B,
            Tipo1A,
            Tipo1B,
            Tipo2A,
            Tipo2B,
            Tipo3A,
            Tipo3B,
            Tipo4A,
            Tipo4B,
            Tipo5A,
            Tipo5B,
            Tipo6A,
            Tipo6B,
        }

        public static void TipologiaConstructor()
        {
            //recorrer todas las tipologias.
            foreach (enumTipologiasCE tipologiaCE in Enum.GetValues(typeof(enumTipologiasCE)))
            {
                if (tipologiaCE != enumTipologiasCE.NONE)
                    ListaTipologias.Add(new TipologiaCE(tipologiaCE, ObtenerInformes(tipologiaCE)));
            }
        }
        public TipologiaCE(enumTipologiasCE nombre, List<Informe_RCE> informes)
        {
            Nombre = nombre.ToString();
            Tipologia = nombre;
            Informes = informes;
        }
        public TipologiaCE()
        {
            Nombre = enumTipologiasCE.NONE.ToString();
            Tipologia = enumTipologiasCE.NONE;
            Informes = new List<Informe_RCE>();
        }
        private static List<Informe_RCE> ObtenerInformes(enumTipologiasCE tipologiaCE)
        {
            List<Informe_RCE> informes = new List<Informe_RCE>()
            {
                new Informe1_RCE(false, 1, "informe1rce", "Informe nº1_RCE"),
                new Informe2_RCE(false, 2, "informe2rce", "Informe nº2_RCE"),
                new Informe3_RCE(false, 3, "informe3rce", "Informe nº3_RCE"),
                new Informe4_RCE(false, 4, "informe4rce", "Informe nº4_RCE"),
                new Informe5_RCE(false, 5, "informe5rce", "Informe nº5_RCE"),
                new Informe6_RCE(false, 6, "informe6rce", "Informe nº6_RCE"),
                new Informe7_RCE(false, 7, "informe7rce", "Informe nº7_RCE")
            };

            List<string> informesTipologia = new List<string>();
            
            switch (tipologiaCE)
            {
                case enumTipologiasCE.Tipo0A:
                    informesTipologia = new List<string>() {};
                    break;
                case enumTipologiasCE.Tipo0B:
                    informesTipologia = new List<string>() {};
                    break;
                case enumTipologiasCE.Tipo1A:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce",
                                                            "informe6rce",
                                                            "informe7rce"};
                    break;
                case enumTipologiasCE.Tipo1B:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce",
                                                            "informe6rce",
                                                            "informe7rce"};
                    break;
                case enumTipologiasCE.Tipo2A:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce",
                                                            "informe6rce",
                                                            "informe7rce"};
                    break;
                case enumTipologiasCE.Tipo2B:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce"};
                    break;
                case enumTipologiasCE.Tipo3A:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce"};
                    break;
                case enumTipologiasCE.Tipo3B:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce"};
                    break;
                case enumTipologiasCE.Tipo4A:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce"};
                    break;
                case enumTipologiasCE.Tipo4B:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe2rce",
                                                            "informe3rce",
                                                            "informe4rce"};
                    break;
                case enumTipologiasCE.Tipo5A:
                    informesTipologia = new List<string>() {"informe1rce",                                                           
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce"};
                    break;
                case enumTipologiasCE.Tipo5B:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce"};
                    break;
                case enumTipologiasCE.Tipo6A:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe3rce",
                                                            "informe4rce",
                                                            "informe5rce"};
                    break;
                case enumTipologiasCE.Tipo6B:
                    informesTipologia = new List<string>() {"informe1rce",
                                                            "informe3rce",
                                                            "informe4rce"};
                    break;
            }

            foreach (Informe_RCE inf in informes)
                if (informesTipologia.Contains(inf.NombreInformeXML))
                    inf.Active = true;


            return informes;
        }

        public List<Informe_RCE> GetInformes()
        {
            return Informes;
        }

        public static TipologiaCE GetTipologia(double[][] aportacionesRN, Simulacion simulacion , bool R_NORM, int escSelect)
        {
            int idLista;
            try
            {
                idLista = simulacion.idListas.First(x => x > 0);
            }catch(System.InvalidOperationException e)
            {
                return null;
            }

            //int yearsRN = simulacion.añosInterNat == null ? 0 : simulacion.añosInterNat.Length;
            //int yearsRA = simulacion.añosInterAlt == null ? 0 : simulacion.añosInterAlt.Length;

            int yearsRN = 0;
            if(simulacion.listas[2].nValidos>0)
            {
                //Quiere decir que hay datos mensuales naturales
                yearsRN = simulacion.listas[2].nValidos;
            }
            else
            {
                //Vemos si hay interpolación
                yearsRN = simulacion.añosInterNat == null ? 0 : simulacion.añosInterNat.Length;
            }

            int yearsRA = 0;
            if (simulacion.listas[3].nValidos > 0)
            {
                //Quiere decir que hay datos mensuales Alterados
                yearsRA = simulacion.listas[3].nValidos;
            }
            else
            {
                //Vemos si hay interpolación
                yearsRA = simulacion.añosInterAlt == null ? 0 : simulacion.añosInterAlt.Length;
            }

            double[] p80s = CalculoMetodosCEs.ObtenerPercentilCaudales(aportacionesRN, 80);
            double[] p90s = CalculoMetodosCEs.ObtenerPercentilCaudales(aportacionesRN, 90);

            int trigger1 = 0;
            int trigger2 = 0;


            //Tipologías 0
            foreach (double percentil in p90s)
            {
                if (percentil == 0)
                {
                    trigger1++;
                }
            }
            if (trigger1 >= 2) {
                return new TipologiaCE(enumTipologiasCE.Tipo0A, ObtenerInformes(enumTipologiasCE.Tipo0A));
            }
            else
            {
                foreach (double percentil in p80s)
                {
                    if (percentil == 0)
                    {
                        trigger2++;
                    }
                }
                if (trigger2 >= 1)
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo0A, ObtenerInformes(enumTipologiasCE.Tipo0A));
                }
            }

            if (yearsRN < 15)
            {
                return new TipologiaCE(enumTipologiasCE.Tipo0B, ObtenerInformes(enumTipologiasCE.Tipo0B));
            }

            //Tipologias 5
            if (yearsRA == 0 && R_NORM == true)
            {
                if (escSelect > 1)
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo5A, ObtenerInformes(enumTipologiasCE.Tipo5A));
                }
                else
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo5B, ObtenerInformes(enumTipologiasCE.Tipo5B));
                }
                
            }
            //Tipologías 6
            if (yearsRA == 0 && R_NORM == false)
            {
                if (escSelect > 0)
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo6A, ObtenerInformes(enumTipologiasCE.Tipo6A));
                }
                else
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo6B, ObtenerInformes(enumTipologiasCE.Tipo6B));
                }

            }
            //Tipologias 1
            else if (yearsRA >= 7 && R_NORM == true) 
            {
                if (escSelect > 1)
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo1A, ObtenerInformes(enumTipologiasCE.Tipo1A));
                }
                else
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo1B, ObtenerInformes(enumTipologiasCE.Tipo1B));
                }
            }
            //Tipologias 2
            else if (yearsRA >= 7 && R_NORM == false)
            {
                if (escSelect > 0)
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo2A, ObtenerInformes(enumTipologiasCE.Tipo2A));
                }
                else
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo2B, ObtenerInformes(enumTipologiasCE.Tipo2B));
                }
            }
            //Tipologias 3
            else if (yearsRA < 7 && R_NORM == true)
            {
                if (escSelect > 1)
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo3A, ObtenerInformes(enumTipologiasCE.Tipo3A));
                }
                else
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo3B, ObtenerInformes(enumTipologiasCE.Tipo3B));
                }
            }
            //Tipologias 4
            else if (yearsRA < 7 && R_NORM == false)
            {
                if (escSelect > 0)
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo4A, ObtenerInformes(enumTipologiasCE.Tipo4A));
                }
                else
                {
                    return new TipologiaCE(enumTipologiasCE.Tipo4B, ObtenerInformes(enumTipologiasCE.Tipo4B));
                }
            }
            return new TipologiaCE(enumTipologiasCE.NONE, ObtenerInformes(enumTipologiasCE.NONE));
        }

        public void ProcesarInformes(ExcelPackage excel, SerieRCE serie)
        {
            //Escribir cabecera
            
            foreach (Informe_RCE informe in Informes)
            {
                if (informe.Active)
                    informe.Escribir(excel, serie);
            }

            foreach (Informe_RCE informe in Informes)
                if (!informe.Active) informe.Borrar(excel);
        }
        
    }
}
