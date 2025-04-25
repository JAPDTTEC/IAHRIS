using MultiLangXML;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Cabecera : Informe
    {
        public Cabecera(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[0];

            if ((datos._simulacion.listas[0].nValidos < 15 && datos._simulacion.listas[0].nValidos > 6)
                || (datos._simulacion.listas[1].nValidos < 15 && datos._simulacion.listas[1].nValidos > 6)
                || (datos._simulacion.listas[2].nValidos < 15 && datos._simulacion.listas[2].nValidos > 6)
                || (datos._simulacion.listas[3].nValidos < 15 && datos._simulacion.listas[3].nValidos > 6))
            {


                objSheet.Cells["N22:Q22"].Merge = true;
                objSheet.Cells["N23:Q27"].Merge = true;

                objSheet.Cells["N22"].Value = "AVISO:";
                objSheet.Cells["N22"].Style.Font.Color.SetColor(Color.Red);
                objSheet.Cells["N22"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                objSheet.Cells["N22"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                objSheet.Cells["N22"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                objSheet.Cells["N23"].Value = "El usuario debe tener presente la incertidumbre asociada a la escasa disponibilidad de datos";
                objSheet.Cells["N23"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                objSheet.Cells["N23"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                objSheet.Cells["N23"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }






            objSheet.Cells["AA1"].Value = "R_" + datos._simulacion.abreviaturaRegimenPunto.ToUpper();
            objSheet.Cells["AB1"].Value = "Régimen " + datos._simulacion.nombreRegimenPunto;

            if (datos._simulacion.nombreRegimenAlteracion != null)
            {
                objSheet.Cells["AA2"].Value = "R_" + datos._simulacion.abreviaturaRegimenAlteracion.ToUpper();
                objSheet.Cells["AB2"].Value = "Régimen " + datos._simulacion.nombreRegimenAlteracion;
            }
            else
            {
                objSheet.Cells["AA2"].Value = "";
                objSheet.Cells["AB2"].Value = "";
                objSheet.Cells["E8"].Value = "";
            }


            objSheet.Cells["E9"].Value = DateTime.Now.ToShortDateString();
            objSheet.Cells["K14"].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, datos._simulacion.mesInicio.ToString());

            objSheet.Cells["E3"].Value = "Versión " + Application.ProductVersion;
            DateTime fileDate = new FileInfo(Application.ExecutablePath).LastWriteTime;
            objSheet.Cells["E4"].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, fileDate.Month.ToString()) + " " + fileDate.Year.ToString();
            int i, j;
            int pos;

            //Lista que contiene los informes a escribir, con su posición en la hoja excel
            Dictionary<string, string> inflist = new Dictionary<string, string>();
            int row = 16;
            string col = "B";
            string secCol = "H";

            List<Informe> informes = datos._simulacion.Tipologia.GetInformes();
            foreach (Informe inf in informes)
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
            {
                objSheet.Cells[entry.Value].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, entry.Key);
            }

            int offsetRow = 33;

            //range. .get_Resize(sim.fechaFIN - sim.fechaINI, 11);
            var loopTo = offsetRow + datos._simulacion.fechaFIN - 1;
            for (i = offsetRow + datos._simulacion.fechaINI; i <= loopTo; i++)
            {
                int year = i - 33;
                int indice = i - datos._simulacion.fechaINI;
                objSheet.Cells[indice + 1, 2].Value = year.ToString() + "-" + (year + 1).ToString().Substring(2);


                // MENSUALES NATURAL
                if (datos._simulacion.listas[2].Año is null)
                {
                    if (datos._simulacion.añosInterNat != null)
                    {
                        var loopTo1 = datos._simulacion.añosInterNat.Length - 1;
                        for (j = 0; j <= loopTo1; j++)
                        {
                            if (year == datos._simulacion.añosInterNat[j])
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
                    pos = Array.BinarySearch(datos._simulacion.listas[2].Año, year);
                    if (pos >= 0)
                    {
                        if (datos._simulacion.listas[2].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 3].Value = "X";
                            // Else
                            // range(indice + 1, 2) = "NC"
                        }
                        // Else
                        // range(indice + 1, 2) = "SD"
                    }

                    if (datos._simulacion.añosInterNat != null)
                    {
                        var loopTo2 = datos._simulacion.añosInterNat.Length - 1;
                        for (j = 0; j <= loopTo2; j++)
                        {
                            if (year == datos._simulacion.añosInterNat[j])
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
                    if (datos._simulacion.añosParaCalculo[2].año != null)
                    {
                        var loopTo3 = datos._simulacion.añosParaCalculo[2].año.Length - 1;
                        for (j = 0; j <= loopTo3; j++)
                        {
                            if (year == datos._simulacion.añosParaCalculo[2].año[j])
                            {
                                objSheet.Cells[indice + 1, 4].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // MENSUALES ALTERADO
                if (datos._simulacion.listas[3].Año is null)
                {
                    if (datos._simulacion.añosInterAlt != null)
                    {
                        var loopTo4 = datos._simulacion.añosInterAlt.Length - 1;
                        for (j = 0; j <= loopTo4; j++)
                        {
                            if (year == datos._simulacion.añosInterAlt[j])
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
                    pos = Array.BinarySearch(datos._simulacion.listas[3].Año, year);
                    if (pos >= 0)
                    {
                        if (datos._simulacion.listas[3].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 5].Value = "X";
                            // Else
                            // range(indice + 1, 4) = "NC"
                        }
                        // Else
                        // range(indice + 1, 4) = "SD"
                    }

                    if (datos._simulacion.añosInterAlt != null)
                    {
                        var loopTo5 = datos._simulacion.añosInterAlt.Length - 1;
                        for (j = 0; j <= loopTo5; j++)
                        {
                            if (year == datos._simulacion.añosInterAlt[j])
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
                    if (datos._simulacion.añosParaCalculo[3].año != null)
                    {
                        var loopTo6 = datos._simulacion.añosParaCalculo[3].año.Length - 1;
                        for (j = 0; j <= loopTo6; j++)
                        {
                            if (year == datos._simulacion.añosParaCalculo[3].año[j])
                            {
                                objSheet.Cells[indice + 1, 6].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // COETANEOS MENSUALES
                if (objSheet.Cells[indice + 1, 4].Value != null & (objSheet.Cells[indice + 1, 6].Value != null))
                {
                    objSheet.Cells[indice + 1, 7].Value = "X";
                }

                // DIARIO NATURAL
                if (datos._simulacion.listas[0].Año is null)
                {
                }
                // range(indice + 1, 7) = "SD"
                else
                {
                    pos = Array.BinarySearch(datos._simulacion.listas[0].Año, year);
                    if (pos >= 0)
                    {
                        if (datos._simulacion.listas[0].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 8].Value = "X";
                        }
                    }
                }

                if ((objSheet.Cells[indice + 1, 8].Value) != null)
                {
                    if (datos._simulacion.añosParaCalculo[0].año != null)
                    {
                        var loopTo7 = datos._simulacion.añosParaCalculo[0].año.Length - 1;
                        for (j = 0; j <= loopTo7; j++)
                        {
                            if (year == datos._simulacion.añosParaCalculo[0].año[j])
                            {
                                objSheet.Cells[indice + 1, 9].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // DIARIO ALTERADO
                if (datos._simulacion.listas[1].Año is null)
                {
                }
                // range(indice + 1, 9) = "SD"
                else
                {
                    pos = Array.BinarySearch(datos._simulacion.listas[1].Año, year);
                    if (pos >= 0)
                    {
                        if (datos._simulacion.listas[1].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 10].Value = "X";
                            // Else
                            // range(indice + 1, 9) = "NC"
                        }
                        // Else
                        // range(indice + 1, 9) = "SD"
                    }
                }

                if ((objSheet.Cells[indice + 1, 10].Value) != null)
                {
                    if (datos._simulacion.añosParaCalculo[1].año != null)
                    {
                        var loopTo8 = datos._simulacion.añosParaCalculo[1].año.Length - 1;
                        for (j = 0; j <= loopTo8; j++)
                        {
                            if (year == datos._simulacion.añosParaCalculo[1].año[j])
                            {
                                objSheet.Cells[indice + 1, 11].Value = "X";
                                break;
                            }
                        }
                    }
                }


                // COETANEOS DIARIOS
                if ((objSheet.Cells[indice + 1, 9].Value) != null & (objSheet.Cells[indice + 1, 11].Value) != null)
                {
                    objSheet.Cells[indice + 1, 12].Value = "X";
                }
            }

            // Escribir resumen con años
            int filaNumAños = offsetRow + (datos._simulacion.fechaFIN - datos._simulacion.fechaINI) + 1;
            objSheet.Cells[filaNumAños, 2].Value = "Total";

            // Mensuales > Natural 
            if (datos._simulacion.añosInterNat != null)
                objSheet.Cells[filaNumAños, 3].Value = (datos._simulacion.listas[2].nValidos + datos._simulacion.añosInterNat.Length).ToString();
            else
                objSheet.Cells[filaNumAños, 3].Value = datos._simulacion.listas[2].nValidos.ToString();


            objSheet.Cells[filaNumAños, 4].Value = datos._simulacion.añosParaCalculo[2].nAños;

            // Mensual > Alterado
            if (datos._simulacion.añosInterAlt != null)
                objSheet.Cells[filaNumAños, 5].Value = (datos._simulacion.listas[3].nValidos + datos._simulacion.añosInterAlt.Length).ToString();
            else
                objSheet.Cells[filaNumAños, 5].Value = datos._simulacion.listas[3].nValidos.ToString();


            objSheet.Cells[filaNumAños, 6].Value = datos._simulacion.añosParaCalculo[3].nAños;

            // Mensuales > Coetaniedad
            if (datos._simulacion.añosInterCoe != null)
                objSheet.Cells[filaNumAños, 7].Value = (datos._simulacion.coe[1].nCoetaneos + datos._simulacion.añosInterCoe.Length).ToString();
            else
                objSheet.Cells[filaNumAños, 7].Value = datos._simulacion.coe[1].nCoetaneos.ToString();


            objSheet.Cells[filaNumAños, 8].Value = datos._simulacion.listas[0].nValidos.ToString();
            objSheet.Cells[filaNumAños, 9].Value = datos._simulacion.añosParaCalculo[0].nAños.ToString();
            objSheet.Cells[filaNumAños, 10].Value = datos._simulacion.listas[1].nValidos.ToString();
            objSheet.Cells[filaNumAños, 11].Value = datos._simulacion.añosParaCalculo[1].nAños.ToString();
            objSheet.Cells[filaNumAños, 12].Value = datos._simulacion.coe[0].nCoetaneos.ToString();

            string sRango;

            ((ExcelWorksheet)excel.Workbook.Worksheets[0]).Select();
            ExcelWorksheet wrk = ((ExcelWorksheet)excel.Workbook.Worksheets[0]);

            //TODO: USADO PARA COPIAR FORMATO.  REPROGRAMARLO CORRECTAMENTE.

            string filaFin = filaNumAños.ToString();
            sRango = "B34:L" + filaFin;

            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            // Poner una linea mas gruesa
            objSheet.Cells["B" + filaFin + ":L" + filaFin].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;// Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
        }
    }
}
