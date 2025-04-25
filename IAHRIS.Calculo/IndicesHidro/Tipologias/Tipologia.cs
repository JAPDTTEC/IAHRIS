
using IAHRIS.Calculo.IndicesHidro.Informes;
using MathNet.Numerics.Distributions;
using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS.Calculo.Tipologias
{
    public enum enumTipologias
    {
        NONE,
        Tipo1,
        Tipo2,
        Tipo3,
        Tipo4,
        Tipo5,
        Tipo6,
        Tipo6A,
        Tipo6B,
        Tipo7,
        Tipo8,
        Tipo8A,
        Tipo8B,
        Tipo1SR,
        Tipo2SR,
        Tipo3SR,
        Tipo4SR,
        Tipo7SR,
        Tipo8SR,
        Tipo8ASR,
        Tipo8BSR,

    }

    public class Tipologia
    {
        public string Nombre;

        public static List<Informe> Informes;

        static readonly List<Tipologia> ListaTipologias = new List<Tipologia>();


        public static void TipologiaConstructor()
        {
            //recorrer todas las tipologias.
            foreach (enumTipologias tipologia in Enum.GetValues(typeof(enumTipologias)))
            {
                if(tipologia != enumTipologias.NONE)
                    ListaTipologias.Add(new Tipologia(tipologia, ObtenerInformes(tipologia)));
            }
        }

        private static List<Informe> ObtenerInformes(enumTipologias tipologia)
        {
            List<Informe> informes = new List<Informe>()
            {
                new Informe1(false, 1, "informe1", "informe nº1"),
                new Informe1SR(false, 2, "informe1sr", "informe nº1 SR"),
                new Informe1a(false, 3, "informe1a", "informe nº1a"),
                new Informe1aSR(false, 4, "informe1asr", "informe nº1a SR"),
                new Informe1b(false, 5, "informe1b", "informe nº 1b"),
                new Informe1bSR(false, 6, "informe1bsr", "informe nº1b SR"),

                //orden importante
                new Informe2(false, 7, "informe2", "informe nº 2"),
                new Informe2a(false, 8, "informe2a", "informe nº2a"),
                
                //orden importante
                new Informe3(false, 9, "informe3", "informe nº3"),
                new Informe3a(false, 10, "informe3a", "informe nº3a"),
                
                
                new Informe3b(false, 11, "informe3b", "informe nº 3b"),
                new Informe3bSR(false, 12, "informe3bsr", "informe nº3b SR"),
                new Informe3c(false, 13, "informe3c", "informe nº3c"),

                new Informe4(false, 14, "informe4", "informe nº4"),
                new Informe4SR(false, 15,"informe4sr", "informe nº4 SR"),
                new Informe4a(false, 16, "informe4a", "informe nº4a"),
                new Informe4aSR(false, 17, "informe4asr", "informe nº4a SR"),

                new Informe5(false, 18, "informe5", "informe nº5"),
                new Informe5SR(false, 19, "informe5sr", "informe nº5 SR"),
                new Informe5a(false, 20, "informe5a", "informe nº5a"),
                new Informe5aSR(false, 21, "informe5asr", "informe nº5a SR"),
                new Informe5b(false, 22, "informe5b", "informe nº5b"),                
                new Informe5bSR(false, 23, "informe5bsr", "informe nº5b SR"),    
                
                new Informe6(false, 24, "informe6", "informe nº 6"),
                new Informe6a(false, 25, "informe6a", "informe nº 6a"),
                new Informe6b(false, 26, "informe6b", "informe nº6b"),
                new Informe6c(false, 27, "informe6c", "informe nº6c"),
                new Informe6d(false, 28, "informe6d", "informe nº6d"),
                new Informe6e(false, 29, "informe6e", "informe nº6e"),

                new Informe7a(false, 30, "informe7a", "informe nº 7a"),
                new Informe7b(false, 31, "informe7b", "informe nº 7b"),
                new Informe7c(false, 32, "informe7c", "informe nº 7c"),
                new Informe7d(false, 33, "informe7d", "informe nº 7d"),

                new Informe8(false, 34, "informe8", "informe nº 8"),
                new Informe8a(false, 35, "informe8a", "informe nº8a"),
                new Informe8b(false, 36, "informe8b", "informe nº8b"),
                new Informe8c(false, 37, "informe8c", "informe nº8c"),
                new Informe8d(false, 38, "informe8d", "informe nº8d"),

                //new Informe9(false, 39, "informe9", "informe nº 9"),
                //new Informe9a(false, 40, "informe9a", "informe nº9a"),
                //new Informe9b(false, 41, "informe9b", "informe nº9b"),

                new Informe10a(false, 39, "informe10a", "informe nº10a"),
                new Informe10b(false, 40, "informe10b", "informe nº10b"),
                new Informe10c(false, 41, "informe10c", "informe nº 10c"),
                new Informe10d(false, 42, "informe10d", "informe nº10d"),
            };

            List<string> informesTipologia = new List<string>(); 
            switch (tipologia)
            {
                case enumTipologias.Tipo1: //1, 2, 2a, 4, 6, 6c, 9, 9b
                    informesTipologia = new List<string>() {"informe1", 
                                                            "informe2", "informe2a", 
                                                            "informe4", 
                                                            "informe6", "informe6c",
                                                            "informe9", "informe9b"};
                    break;

                case enumTipologias.Tipo2: //1, 2, 2a, 4a, 9a
                    informesTipologia = new List<string>() {"informe1", 
                                                            "informe2", "informe2a", 
                                                            "informe4a", 
                                                            "informe9a"};
                    break;
                case enumTipologias.Tipo3: // 1a, 3, 3a, 5b, 6a, 6d
                    informesTipologia = new List<string>() {"informe1a", 
                                                            "informe3a", "informe3", 
                                                            "informe5b", 
                                                            "informe6a", "informe6d"};
                    break;
                case enumTipologias.Tipo4: //1a, 3, 3a, 5a
                    informesTipologia = new List<string>() {"informe1a",
                                                            "informe3a", "informe3",
                                                            "informe5a" };
                    break;
                case enumTipologias.Tipo5: //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4, 5, 6, 6a, 6b, 6c, 6d, 6e, 7a, 7d, 8, 8a, 9, 9b, 10a
                    informesTipologia = new List<string>() {"informe1", "informe1a", "informe1b", 
                                                            "informe2", "informe2a", 
                                                            "informe3a", "informe3", "informe3b","informe3c",
                                                            "informe4", 
                                                            "informe5", 
                                                            "informe6", "informe6a", "informe6b", "informe6c", "informe6d", "informe6e",
                                                            "informe7a", "informe7d", 
                                                            "informe8", "informe8a", 
                                                            "informe10a"};
                    break;
                case enumTipologias.Tipo6:  //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4a, 5a, 7b, 8, 8c, 9a, 10b

                    informesTipologia = new List<string>() {"informe1","informe1a","informe1b",
                                                            "informe2","informe2a",
                                                            "informe3a","informe3","informe3b","informe3c",
                                                            "informe4a",
                                                            "informe5a",
                                                            "informe7b",
                                                            "informe8","informe8c",
                                                            "informe10b" };
                    break;
                case enumTipologias.Tipo6A:  //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4, 5a, 6, 6c, 7b, 8, 8c, 9, 9b, 10b
                    informesTipologia = new List<string>() {"informe1","informe1a","informe1b",
                                                            "informe2","informe2a",
                                                            "informe3a","informe3","informe3b","informe3c",
                                                            "informe4",
                                                            "informe5a",
                                                            "informe6","informe6c",
                                                            "informe7b",
                                                            "informe8","informe8c",
                                                            "informe10b" };
                    break;
                case enumTipologias.Tipo6B: //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4a, 5b, 6a, 6d, 7b, 8, 8c, 9a, 10b                                     
                    informesTipologia = new List<string>() {"informe1","informe1a","informe1b",
                                                            "informe2","informe2a",
                                                            "informe3a","informe3","informe3b","informe3c",
                                                            "informe4a",
                                                            "informe5b",
                                                            "informe6a", "informe6d",
                                                            "informe7b", 
                                                            "informe8", "informe8c",
                                                            "informe10b" };
                    break;
                case enumTipologias.Tipo7: //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4, 5, 6, 6a, 6b, 6c, 6d, 6e, 7c, 7d, 8, 8b, 9, 9b, 10c
                    informesTipologia = new List<string>() {"informe1","informe1a","informe1b",
                                                            "informe2","informe2a",
                                                            "informe3a","informe3","informe3b","informe3c",
                                                            "informe4",
                                                            "informe5",
                                                            "informe6","informe6a","informe6b","informe6c","informe6d","informe6e",
                                                            "informe7c","informe7d",
                                                            "informe8","informe8b", 
                                                            "informe10c" };
                    break;
                case enumTipologias.Tipo8: //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4a, 5a, 7c, 8, 8d, 9a, 10d
                    informesTipologia = new List<string>() {"informe1","informe1a","informe1b",
                                                            "informe2","informe2a",
                                                            "informe3a","informe3","informe3b","informe3c",
                                                            "informe4a",
                                                            "informe5a",
                                                            "informe7c",
                                                            "informe8","informe8d",
                                                            "informe10d" };
                    break;

                case enumTipologias.Tipo8A: //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4, 5a, 6, 6c, 7c, 8, 8d, 9, 9b, 10d
                    informesTipologia = new List<string>() {"informe1","informe1a","informe1b",
                                                            "informe2","informe2a", 
                                                            "informe3a","informe3","informe3b","informe3c",
                                                            "informe4",
                                                            "informe5a",
                                                            "informe6","informe6c",
                                                            "informe7c",
                                                            "informe8","informe8d",
                                                            "informe10d" };
                    break;
                case enumTipologias.Tipo8B: //1, 1a, 1b, 2, 2a, 3, 3a, 3b, 4a, 5b, 6a, 6d, 7c, 8, 8d, 9a, 10d
                    informesTipologia = new List<string>() {"informe1","informe1a","informe1b",
                                                            "informe2","informe2a",
                                                            "informe3a","informe3","informe3b","informe3c",
                                                            "informe4a",
                                                            "informe5b",
                                                            "informe6a","informe6d",
                                                            "informe7c",
                                                            "informe8","informe8d",
                                                            "informe10d" };
                    break;

                case enumTipologias.Tipo1SR: //1SR, 2, 4SR, 6, 6c 
                    informesTipologia = new List<string>() { "informe1sr" ,
                                                            "informe2" ,
                                                            "informe4sr",
                                                            "informe6","informe6c" };
                    break;
                case enumTipologias.Tipo2SR: //1SR, 2, 4SR
                    informesTipologia = new List<string>() {"informe1sr",
                                                            "informe2",
                                                            "informe4asr" };
                    break;
                case enumTipologias.Tipo3SR: //1aSR, 3, 5bSR, 6a, 6d 
                    informesTipologia = new List<string>() {"informe1asr",
                                                            "informe3",
                                                            "informe5bsr",
                                                            "informe6a","informe6d" };
                    break;
                case enumTipologias.Tipo4SR: //1aSR, 3, 5aSR
                    informesTipologia = new List<string>() {"informe1asr",
                                                            "informe3",
                                                            "informe5asr" };
                    break;
                case enumTipologias.Tipo7SR: //1SR, 1asr, 1bSR, 2, 3, 3bSR, 3c, 4SR, 5SR, 6, 6a, 6b, 6c, 6d, 6e, 7c, 7d, 10c
                    informesTipologia = new List<string>() {"informe1sr","informe1asr","informe1bsr",
                                                            "informe2",
                                                            "informe3","informe3bsr","informe3c",
                                                            "informe4sr",
                                                            "informe5sr",
                                                            "informe6","informe6a","informe6b","informe6c","informe6d","informe6e",
                                                            "informe7c","informe7d",
                                                            "informe10c"  };
                    break;
                case enumTipologias.Tipo8SR: //1SR, 1aSR, 1bSR, 2, 3, 3bSR, 3c, 4aSR, 5aSR 7c, 10d
                    informesTipologia = new List<string>() {"informe1sr","informe1asr","informe1bsr",
                                                            "informe2",
                                                            "informe3","informe3bsr","informe3c",
                                                            "informe4asr",
                                                            "informe5asr",
                                                            "informe7c",
                                                            "informe10d"  };
                    break;
                case enumTipologias.Tipo8ASR: // 1SR, 1aSR, 1bSR, 2, 3, 3bSR, 3c, 4SR, 5aSR, 6, 6c, 7c, 10d
                    informesTipologia = new List<string>() {"informe1sr","informe1asr","informe1bsr",
                                                            "informe2",
                                                            "informe3","informe3bsr","informe3c",
                                                            "informe4sr",
                                                            "informe5asr",
                                                            "informe6","informe6c",
                                                            "informe7c",
                                                            "informe10d"  };
                    break;
                case enumTipologias.Tipo8BSR: // 1SR, 1aSR, 1bSR, 2, 3, 3bSR, 3c, 4aSR, 5bSR, 6a, 6d, 7c, 10d
                    informesTipologia = new List<string>() {"informe1sr","informe1asr","informe1bsr",
                                                            "informe2",
                                                            "informe3","informe3bsr","informe3c",
                                                            "informe4asr",
                                                            "informe5bsr",
                                                            "informe6a","informe6d",
                                                            "informe7c",
                                                            "informe10d"  };
                    break;
            }

            foreach (Informe inf in informes)
                if (informesTipologia.Contains(inf.NombreInformeXML))
                    inf.Active = true;


            return informes;
        }

        public Tipologia(enumTipologias nombre, List<Informe> informes)
        {
            Nombre = nombre.ToString();
            Informes = informes;
        }

        public List<Informe> GetInformes()
        {
            return Informes;
        }

        public static Tipologia GetTipologia (Simulacion simulacion)
        {
            bool isNatDia = false;
            bool isAltDia = false;
            bool isNatMens = false;
            bool isAltMens = false;
            bool isNat = false;
            bool isAlt = false;
            bool usarCoeDia = false;
            bool usarCoeMen = false;

            bool esSerieReducida = false;

            //El minimo de años admisible para las series es de 7 años.
            //si las series tienen menos de 15 años validos, las consideramos reducidas.


            /// listas[0] -> Nat diaria
            if (simulacion.listas[0].nValidos > 6)
            {
                isNatDia = true;
                isNat = true;
                esSerieReducida = simulacion.listas[0].nValidos < 15;
            }

            /// listas[1] -> Alt diaria
            if (simulacion.listas[1].nValidos > 6)
            {
                isAltDia = true;
                isAlt = true;
                esSerieReducida = simulacion.listas[1].nValidos < 15;
            }

            /// listas[2] -> Nat mensual
            if (simulacion.listas[2].nValidos > 6)
            {
                isNatMens = true;
                isNat = true;
                esSerieReducida = simulacion.listas[2].nValidos < 15;
            }
            /// listas[3] -> Alt mensual
            if (simulacion.listas[3].nValidos > 6)
            {
                isAltMens = true;
                isAlt = true;

                if(esSerieReducida == false)  
                    esSerieReducida = simulacion.listas[3].nValidos < 15;
                
            }

            //Régimen natural y alterado con datos diarios, basta con que uno de los dos tenga <15 años
            if (isNatDia && isAltDia)            
                esSerieReducida = simulacion.listas[0].nValidos < 15 || simulacion.listas[1].nValidos < 15;

            //Régimen natural y alterado con datos mensuales, basta con que uno de los dos tenga<15 años
            if (isNatMens && isAltMens)
                esSerieReducida = simulacion.listas[2].nValidos < 15 || simulacion.listas[3].nValidos < 15;

            if (simulacion.usarCoeDiara)            
                usarCoeDia = true;
            
            if (simulacion.usarCoe)            
                usarCoeMen = true;


            //Determinación de tipologías
            if (!esSerieReducida)
            {
                if (isNatDia & !isAlt) return new Tipologia(enumTipologias.Tipo1, ObtenerInformes(enumTipologias.Tipo1));
                if (isNatMens & !isAlt) return new Tipologia(enumTipologias.Tipo2, ObtenerInformes(enumTipologias.Tipo2));
                if (isAltDia & !isNat) return new Tipologia(enumTipologias.Tipo3, ObtenerInformes(enumTipologias.Tipo3));
                if (isAltMens & !isNat) return new Tipologia(enumTipologias.Tipo4, ObtenerInformes(enumTipologias.Tipo4));
                if (isNatDia & isAltDia & usarCoeDia) return new Tipologia(enumTipologias.Tipo5, ObtenerInformes(enumTipologias.Tipo5));
                if (isNatMens & isAltMens & usarCoeMen) return new Tipologia(enumTipologias.Tipo6, ObtenerInformes(enumTipologias.Tipo6));
                if (isNatDia & isAltMens & usarCoeMen) return new Tipologia(enumTipologias.Tipo6A, ObtenerInformes(enumTipologias.Tipo6A));
                if (isNatMens & isAltDia & usarCoeMen) return new Tipologia(enumTipologias.Tipo6B, ObtenerInformes(enumTipologias.Tipo6B));
                if (isNatDia & isAltDia & !usarCoeDia) return new Tipologia(enumTipologias.Tipo7, ObtenerInformes(enumTipologias.Tipo7));
                if (isNatMens & isAltMens & !usarCoeMen) return new Tipologia(enumTipologias.Tipo8, ObtenerInformes(enumTipologias.Tipo8));
                if (isNatDia & isAltMens & !usarCoeMen) return new Tipologia(enumTipologias.Tipo8A, ObtenerInformes(enumTipologias.Tipo8A));
                if (isNatMens & isAltDia & !usarCoeMen) return new Tipologia(enumTipologias.Tipo8B, ObtenerInformes(enumTipologias.Tipo8B));
            }
            else
            {
                //*********************************************
                if (isNatDia & !isAlt) return new Tipologia(enumTipologias.Tipo1SR, ObtenerInformes(enumTipologias.Tipo1SR));
                if (isNatMens & !isAlt) return new Tipologia(enumTipologias.Tipo2SR, ObtenerInformes(enumTipologias.Tipo2SR));
                if (isAltDia & !isNat) return new Tipologia(enumTipologias.Tipo3SR, ObtenerInformes(enumTipologias.Tipo3SR));
                if (isAltMens & !isNat) return new Tipologia(enumTipologias.Tipo4SR, ObtenerInformes(enumTipologias.Tipo4SR));
                if (isNatDia & isAltDia & !usarCoeDia) return new Tipologia(enumTipologias.Tipo7SR, ObtenerInformes(enumTipologias.Tipo7SR));
                if (isNatMens & isAltMens & !usarCoeMen) return new Tipologia(enumTipologias.Tipo8SR, ObtenerInformes(enumTipologias.Tipo8SR));
                if (isNatDia & isAltMens & !usarCoeMen) return new Tipologia(enumTipologias.Tipo8ASR, ObtenerInformes(enumTipologias.Tipo8ASR));
                if (isNatMens & isAltDia & !usarCoeMen) return new Tipologia(enumTipologias.Tipo8BSR, ObtenerInformes(enumTipologias.Tipo8BSR));

                //*********************************************
            }


            return new Tipologia(enumTipologias.NONE, ObtenerInformes(enumTipologias.NONE));
        }


        public void ProcesarInformes(ExcelPackage excel, MultiIdiomasXML traductor, DatosCalculo datos, ref IAHRISDataSet dataset)
        {
            foreach(Informe informe in Informes)
            {
                if (informe.Active)
                    informe.Escribir(excel, ref datos, ref dataset, traductor);
            }

            foreach (Informe informe in Informes)
                if (!informe.Active)  informe.Borrar(excel);
        }

    }
}
