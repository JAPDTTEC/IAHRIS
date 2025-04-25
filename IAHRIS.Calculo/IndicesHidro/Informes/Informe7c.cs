using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe7c : Informe
    {
        public Informe7c(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
         
            calculations.CalcularIndicesHabitualesAgregados();
            calculations.CalcularIndiceAlteracionGlobalHabitualesAgregados();

            string s = "";
            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];
            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
            // objSheet.Cells["D15");
            // range.get_Resize(7, 2);

            //objSheet.Cells[15,4,22,6];

            int offsetRow = 16;
            int offsetCol = 3;

            if (dataset._IndicesHabitualesAgregados[0].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[0].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[0].valor[0]) + "15").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[0].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[0].valor[0]) + "17"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[0].valor[0]) + "17"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabitualesAgregados[0].valor[0];
                if (dataset._IndicesHabitualesAgregados[0].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabitualesAgregados[0].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = "#";
            }

            if (dataset._IndicesHabitualesAgregados[1].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[1].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[1].valor[0]) + "16").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[1].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[1].valor[0]) + "18"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[1].valor[0]) + "18"].Value = null;

                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._IndicesHabitualesAgregados[1].valor[0];
                if (dataset._IndicesHabitualesAgregados[1].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabitualesAgregados[1].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 2, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 2].Value = "#";
            }

            if (dataset._IndicesHabitualesAgregados[2].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[2].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[2].valor[0]) + "17").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[2].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[2].valor[0]) + "19"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[2].valor[0]) + "19"].Value = null;

                objSheet.Cells[offsetRow + 3, offsetCol + 1].Value = dataset._IndicesHabitualesAgregados[2].valor[0];
                if (dataset._IndicesHabitualesAgregados[2].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabitualesAgregados[2].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 3, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 3, offsetCol + 2].Value = "#";
            }

            if (dataset._IndicesHabitualesAgregados[3].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[3].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[3].valor[0]) + "18").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[3].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[3].valor[0]) + "20"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[3].valor[0]) + "20"].Value = null;


                objSheet.Cells[offsetRow + 4, offsetCol + 1].Value = dataset._IndicesHabitualesAgregados[3].valor[0];
                if (dataset._IndicesHabitualesAgregados[3].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabitualesAgregados[3].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 4, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 4, offsetCol + 2].Value = "#";
            }

            if (dataset._IndicesHabitualesAgregados[4].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[4].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[4].valor[0]) + "19").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[4].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[4].valor[0]) + "21"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[4].valor[0]) + "21"].Value = null;

                objSheet.Cells[offsetRow + 5, offsetCol + 1].Value = dataset._IndicesHabitualesAgregados[4].valor[0];
                if (dataset._IndicesHabitualesAgregados[4].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabitualesAgregados[4].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 5, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 5, offsetCol + 2].Value = "#";
            }

            if (dataset._IndicesHabitualesAgregados[5].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[5].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[5].valor[0]) + "20").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[5].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[5].valor[0]) + "22"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[5].valor[0]) + "22"].Value = null;

                objSheet.Cells[offsetRow + 6, offsetCol + 1].Value = dataset._IndicesHabitualesAgregados[5].valor[0];
                if (dataset._IndicesHabitualesAgregados[5].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabitualesAgregados[5].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 6, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 6, offsetCol + 2].Value = "#";
            }

            if (dataset._IndicesHabitualesAgregados[6].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[6].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(dataset._IndicesHabitualesAgregados[6].valor[0]) + "21").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[6].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[6].valor[0]) + "23"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabitualesAgregados[6].valor[0]) + "23"].Value = null;

                objSheet.Cells[offsetRow + 7, offsetCol + 1].Value = dataset._IndicesHabitualesAgregados[6].valor[0];
                if (dataset._IndicesHabitualesAgregados[6].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabitualesAgregados[6].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 7, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 7, offsetCol + 2].Value = "#";
            }

            objSheet.Cells["G46"].Value = dataset._IndiceIAG_Agregados;
            //((Worksheet)m_Excel.Sheets[17]).Select();
            //m_Excel.Cells[DarColumnaGlobales(dataset._IndiceIAG_Agregados) + "14").Select();
            //((Range)m_Excel.Selection).Copy();
            //m_Excel.Cells[DarColumnaGlobales(dataset._IndiceIAG_Agregados) + "44").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

            objSheet.Cells[DarColumnaGlobales(dataset._IndiceIAG_Agregados) + "16"].Copy(objSheet.Cells[DarColumnaGlobales(dataset._IndiceIAG_Agregados) + "46"]);
            objSheet.Cells[DarColumnaGlobales(dataset._IndiceIAG_Agregados) + "46"].Value = null;

            // objSheet.Cells["F27");
            // range.get_Resize(12, 3);

            //objSheet.Cells[27,6,39,9];
            offsetRow = 28;
            offsetCol = 5;
            for (int i = 0; i <= 11; i++)
            {
                s = "";
                if (dataset._IndiceM3Agregados.invertido[i] == true)
                {
                    s += "*";
                }

                if (dataset._IndiceM3Agregados.indeterminacion[i] == true)
                {
                    s += "**";
                }

                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = dataset._IndiceM3Agregados.valor[i];

                if (s != "")
                {
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = s;
                }

                s = "";
                if (dataset._IndiceV3Agregados.invertido[i] == true)
                {
                    s += "*";
                }

                if (dataset._IndiceV3Agregados.indeterminacion[i] == true)
                {
                    s += "**";
                }
                objSheet.Cells[offsetRow + i + 1, offsetCol + 3].Value = dataset._IndiceV3Agregados.valor[i];

                if (s != "")
                {
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 4].Value = s;

                }
            }

            // Escribir los meses
            // objSheet.Cells["D27");
            // range.get_Resize(12, 1);

            //objSheet.Cells[27,4,39,5];

            offsetCol = 3;
            for (int i = 0; i <= 11; i++)
                // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH,
                                                                            ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
        }

        private string DarColumna(float valor)
        {
            if (valor > 0.8d)
            {
                return "I";
            }
            else if (valor > 0.6d)
            {
                return "J";
            }
            else if (valor > 0.4d)
            {
                return "K";
            }
            else if (valor > 0.2d)
            {
                return "L";
            }
            else
            {
                return "M";
            }
        }

        private string DarColumnaGlobales(float valor)
        {
            if (valor > 0.64d)
            {
                return "I";
            }
            else if (valor > 0.36d)
            {
                return "J";
            }
            else if (valor > 0.16d)
            {
                return "K";
            }
            else if (valor > 0.04d)
            {
                return "L";
            }
            else
            {
                return "M";
            }
        }
    }
}
