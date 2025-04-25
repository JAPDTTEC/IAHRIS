using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    internal class CabeceraRCE : Informe_RCE
    {
        public CabeceraRCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public void Escribir(ExcelPackage excel, SerieRCE serie, Simulacion simulacion)
        {
            ExcelWorksheet objSheet = excel.Workbook.Worksheets[0];


            objSheet.Cells["AA1"].Value = "R_" + serie.Abrev_Regimen_Nat;
            objSheet.Cells["AB1"].Value = "Régimen " + serie.Nombre_Regimen_Nat;

            if (serie.Nombre_Regimen_Alt != null)
            {
                objSheet.Cells["AA2"].Value = "R_" + serie.Abrev_Regimen_Alt;
                objSheet.Cells["AB2"].Value = "Régimen " + serie.Nombre_Regimen_Alt;
            }
            else
            {
                objSheet.Cells["AA2"].Value = "";
                objSheet.Cells["AB2"].Value = "";
                objSheet.Cells["E8"].Value = "";
            }


            objSheet.Cells["E9"].Value = DateTime.Now.ToShortDateString();
            objSheet.Cells["K14"].Value = Utiles.ObtenerMes(serie.Mes_Inicio);

            objSheet.Cells["E4"].Value = "Versión " + Application.ProductVersion;
            DateTime fileDate = new FileInfo(Application.ExecutablePath).LastWriteTime;
            objSheet.Cells["E3"].Value = Utiles.ObtenerMes(fileDate.Month) + " " + fileDate.Year.ToString();


            //Lista que contiene los informes a escribir, con su posición en la hoja excel
            Dictionary<string, string> inflist = new Dictionary<string, string>();
            int row = 16;
            string col = "B";
            string secCol = "H";

            List<Informe_RCE> informes = serie.TipologiaCE.GetInformes();

            foreach (Informe_RCE inf in informes)
            {
                if (inf.Active)
                {
                    inflist.Add(inf.NombreInformeXML, col + row.ToString());
                    row++;
                    if (row > 27)
                    {
                        col = secCol;
                        row = 16;
                    }
                }
            }

            foreach (KeyValuePair<string, string> entry in inflist)
                objSheet.Cells[entry.Value].Value = Utiles.ObtenerNombreInforme(entry.Key);

            int offsetRow = 33;
            int i = 0;
            int j = 0;
            int pos = 0;

            var loopTo = offsetRow + simulacion.fechaFIN - 1;
            for (i = offsetRow + simulacion.fechaINI; i <= loopTo; i++)
            {
                int year = i - 33;
                int indice = i - simulacion.fechaINI;
                objSheet.Cells[indice + 1, 2].Value = year.ToString() + "-" + (year + 1).ToString().Substring(2);


                // MENSUALES NATURAL
                if (simulacion.listas[2].Año is null)
                {
                    if (simulacion.añosInterNat != null)
                    {
                        var loopTo1 = simulacion.añosInterNat.Length - 1;
                        for (j = 0; j <= loopTo1; j++)
                        {
                            if (year == simulacion.añosInterNat[j])
                            {
                                objSheet.Cells[indice + 1, 3].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 2) = "SD"
                            }
                        }
                    }
                }
                else
                {
                    pos = Array.BinarySearch(simulacion.listas[2].Año, year);
                    if (pos >= 0)
                    {
                        if (simulacion.listas[2].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 3].Value = "X";
                            // Else
                            // range(indice + 1, 2) = "NC"
                        }
                        // Else
                        // range(indice + 1, 2) = "SD"
                    }

                    if (simulacion.añosInterNat != null)
                    {
                        var loopTo2 = simulacion.añosInterNat.Length - 1;
                        for (j = 0; j <= loopTo2; j++)
                        {
                            if (year == simulacion.añosInterNat[j])
                            {
                                objSheet.Cells[indice + 1, 3].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 2) = "SD"
                            }
                        }
                    }
                }

                if ((objSheet.Cells[indice + 1, 3]).Value != null)
                {
                    if (simulacion.añosParaCalculo[2].año != null)
                    {
                        var loopTo3 = simulacion.añosParaCalculo[2].año.Length - 1;
                        for (j = 0; j <= loopTo3; j++)
                        {
                            if (year == simulacion.añosParaCalculo[2].año[j])
                            {
                                objSheet.Cells[indice + 1, 4].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // MENSUALES ALTERADO
                if (simulacion.listas[3].Año is null)
                {
                    if (simulacion.añosInterAlt != null)
                    {
                        var loopTo4 = simulacion.añosInterAlt.Length - 1;
                        for (j = 0; j <= loopTo4; j++)
                        {
                            if (year == simulacion.añosInterAlt[j])
                            {
                                objSheet.Cells[indice + 1, 5].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 4) = "SD"
                            }
                        }
                    }
                }
                else
                {
                    pos = Array.BinarySearch(simulacion.listas[3].Año, year);
                    if (pos >= 0)
                    {
                        if (simulacion.listas[3].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 5].Value = "X";
                            // Else
                            // range(indice + 1, 4) = "NC"
                        }
                        // Else
                        // range(indice + 1, 4) = "SD"
                    }

                    if (simulacion.añosInterAlt != null)
                    {
                        var loopTo5 = simulacion.añosInterAlt.Length - 1;
                        for (j = 0; j <= loopTo5; j++)
                        {
                            if (year == simulacion.añosInterAlt[j])
                            {
                                objSheet.Cells[indice + 1, 5].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 4) = "SD"
                            }
                        }
                    }
                }

                if ((objSheet.Cells[indice + 1, 5].Value) != null)
                {
                    if (simulacion.añosParaCalculo[3].año != null)
                    {
                        var loopTo6 = simulacion.añosParaCalculo[3].año.Length - 1;
                        for (j = 0; j <= loopTo6; j++)
                        {
                            if (year == simulacion.añosParaCalculo[3].año[j])
                            {
                                objSheet.Cells[indice + 1, 6].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // COETANEOS MENSUALES
                //if (objSheet.Cells[indice + 1, 4].Value != null & (objSheet.Cells[indice + 1, 6].Value != null))
                //{
                //    objSheet.Cells[indice + 1, 7].Value = "X";
                //}

                // DIARIO NATURAL
                if (simulacion.listas[0].Año is null)
                {
                }
                // range(indice + 1, 7) = "SD"
                else
                {
                    pos = Array.BinarySearch(simulacion.listas[0].Año, year);
                    if (pos >= 0)
                    {
                        if (simulacion.listas[0].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 7].Value = "X";
                        }
                    }
                }

                if ((objSheet.Cells[indice + 1, 7].Value) != null)
                {
                    if (simulacion.añosParaCalculo[0].año != null)
                    {
                        var loopTo7 = simulacion.añosParaCalculo[0].año.Length - 1;
                        for (j = 0; j <= loopTo7; j++)
                        {
                            if (year == simulacion.añosParaCalculo[0].año[j])
                            {
                                objSheet.Cells[indice + 1, 8].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // DIARIO ALTERADO
                if (simulacion.listas[1].Año is null)
                {
                }
                // range(indice + 1, 9) = "SD"
                else
                {
                    pos = Array.BinarySearch(simulacion.listas[1].Año, year);
                    if (pos >= 0)
                    {
                        if (simulacion.listas[1].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 9].Value = "X";
                            // Else
                            // range(indice + 1, 9) = "NC"
                        }
                        // Else
                        // range(indice + 1, 9) = "SD"
                    }
                }

                if ((objSheet.Cells[indice + 1, 9].Value) != null)
                {
                    if (simulacion.añosParaCalculo[1].año != null)
                    {
                        var loopTo8 = simulacion.añosParaCalculo[1].año.Length - 1;
                        for (j = 0; j <= loopTo8; j++)
                        {
                            if (year == simulacion.añosParaCalculo[1].año[j])
                            {
                                objSheet.Cells[indice + 1, 10].Value = "X";
                                break;
                            }
                        }
                    }
                }


                //// COETANEOS DIARIOS
                //if ((objSheet.Cells[indice + 1, 9].Value) != null & (objSheet.Cells[indice + 1, 11].Value) != null)
                //{
                //    objSheet.Cells[indice + 1, 12].Value = "X";
                //}
            }

            // Escribir resumen con años
            int filaNumAños = offsetRow + (simulacion.fechaFIN - simulacion.fechaINI) + 1;
            objSheet.Cells[filaNumAños, 2].Value = "Total";

            // Mensuales > Natural 
            if (simulacion.añosInterNat != null)
                objSheet.Cells[filaNumAños, 3].Value = (simulacion.listas[2].nValidos + simulacion.añosInterNat.Length).ToString();
            else
                objSheet.Cells[filaNumAños, 3].Value = simulacion.listas[2].nValidos.ToString();


            objSheet.Cells[filaNumAños, 4].Value = simulacion.añosParaCalculo[2].nAños;

            // Mensual > Alterado
            if (simulacion.añosInterAlt != null)
                objSheet.Cells[filaNumAños, 5].Value = (simulacion.listas[3].nValidos + simulacion.añosInterAlt.Length).ToString();
            else
                objSheet.Cells[filaNumAños, 5].Value = simulacion.listas[3].nValidos.ToString();


            objSheet.Cells[filaNumAños, 6].Value = simulacion.añosParaCalculo[3].nAños;

            //// Mensuales > Coetaniedad
            //if (simulacion.añosInterCoe != null)
            //    objSheet.Cells[filaNumAños, 7].Value = (simulacion.coe[1].nCoetaneos + simulacion.añosInterCoe.Length).ToString();
            //else
            //    objSheet.Cells[filaNumAños, 7].Value = simulacion.coe[1].nCoetaneos.ToString();


            objSheet.Cells[filaNumAños, 7].Value = simulacion.listas[0].nValidos.ToString();
            objSheet.Cells[filaNumAños, 8].Value = simulacion.añosParaCalculo[0].nAños.ToString();
            objSheet.Cells[filaNumAños, 9].Value = simulacion.listas[1].nValidos.ToString();
            objSheet.Cells[filaNumAños, 10].Value = simulacion.añosParaCalculo[1].nAños.ToString();
            //objSheet.Cells[filaNumAños, 12].Value = simulacion.coe[0].nCoetaneos.ToString();

            string sRango;

            ((ExcelWorksheet)excel.Workbook.Worksheets[0]).Select();
            ExcelWorksheet wrk = ((ExcelWorksheet)excel.Workbook.Worksheets[0]);

            //TODO: USADO PARA COPIAR FORMATO.  REPROGRAMARLO CORRECTAMENTE.

            string filaFin = filaNumAños.ToString();
            sRango = "B34:J" + filaFin;

            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            // Poner una linea mas gruesa
            objSheet.Cells["B" + filaFin + ":J" + filaFin].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;// Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;


           

            //Datos de escenarios creados por el usuario

            int mes = serie.Mes_Inicio;
            for (int colMes = 13; colMes < 25; colMes++)
            {
                if (mes > 12) mes = 1;
                objSheet.Cells[32, colMes].Value = Utiles.ObtenerMes(mes);
                mes++;
            }
            offsetRow = 33;
            int cont;
            if (serie.Lista_Escenarios_Seleccionados.Where(x => x.Nombre == "R_NORM").Count() > 0)
            {
                EscenarioDTO escenarioRNORM = serie.Lista_Escenarios_Seleccionados.Where(x => x.Nombre == "R_NORM").ToList()[0];



               


                for (cont = 13; cont <= 24; cont++)
                {
                    if (escenarioRNORM.Caudales_Ecologicos[cont - 13] != 0)
                    {
                        objSheet.Cells[offsetRow, cont].Value = escenarioRNORM.Caudales_Ecologicos[cont - 13];
                    }

                }

            }


            List<EscenarioDTO> escenariosUsuario = serie.Lista_Escenarios_Usuario.Where(x => x.Por_Defecto == false & x.Nombre!="R_NORM").ToList();
            if (escenariosUsuario.Count == 0)
                return;

            offsetRow = 35;

            foreach (EscenarioDTO escenario in escenariosUsuario)
            {
                objSheet.Cells[offsetRow, 12].Value = escenario.Nombre;

                for (cont = 13; cont <= 24; cont++)
                {
                    objSheet.Cells[offsetRow, cont].Value = escenario.Caudales_Ecologicos[cont - 13];
                }
                objSheet.Cells[offsetRow, 25].Value = escenario.Descripcion;
                offsetRow += 2;
            }


            ((ExcelWorksheet)excel.Workbook.Worksheets[0]).Select();
            wrk = ((ExcelWorksheet)excel.Workbook.Worksheets[0]);


        }


        public override void Escribir(ExcelPackage excel, SerieRCE serie)
        {
            throw new NotImplementedException();
        }

        //public override void Escribir(ExcelPackage excel, SerieRCE serie)
        //{
        //    throw new NotImplementedException();
        //}

        ////Diferencias entre año completo y año utilizado?
        ////Devuelve true si es el año es completo para poder aumentar el contador a la hora de mostrar el total.
        //private bool esAñoCompleto(int año, ExcelRange celda,  List<double[]> lista)
        //{
        //    List<double[]> valoresAñoNAT = new List<double[]>();
        //    ///Mensuales NAT.
        //    valoresAñoNAT = lista.Where(x => x[0] == año).ToList();

        //    if (valoresAñoNAT.Count == 12)
        //    {
        //        celda.Value = "X";
        //        return true;
        //    }

        //    celda.Value =  "";
        //    return false;
        //}

        //private bool esAñoCompleto(int año, ExcelRange celda, int[] lista)
        //{
        //    List<double[]> valoresAñoNAT = new List<double[]>();
        //    ///Mensuales NAT.


        //    if (lista!=null && lista.Contains(año))
        //    {
        //        celda.Value = "X";
        //        return true;
        //    }

        //    celda.Value = "";
        //    return false;
        //}
    }
}
