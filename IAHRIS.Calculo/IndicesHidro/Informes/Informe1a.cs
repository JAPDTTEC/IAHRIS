using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe1a : Informe
    {
        

        public Informe1a(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiLangXML.MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTERAnualAlterada();

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();


            int offsetRow = 13;
            int offsetCol = 1;

            for (int i = 0; i <= dataset._AportacionAltAnualOrdAños.año.Length - 1; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = dataset._AportacionAltAnualOrdAños.año[i] + "-" + (dataset._AportacionAltAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = dataset._AportacionAltAnualOrdAños.aportacion[i];
            }

            string sRango = "B14:C";
            sRango = sRango + (14 + dataset._AportacionAltAnualOrdAños.año.Length - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


            // Escribir los años por tipos
            int nAñosH = 0;
            int nAñosM = 0;
            int nAñosS = 0;
            int[] añosH = null;
            int[] añosM = null;
            int[] añosS = null;
            float[] apH = null;
            float[] apM = null;
            float[] apS = null;


            // Esto solo se calculo con las series mensuales COETANEOS -> Pueso usar NAT o ALt indistintamente
            for (int i = 0; i <= dataset._AportacionAltAnualOrdAños.año.Length - 1; i++)
            {
                if (dataset._AportacionAltAnualOrdAños.tipo != null && dataset._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    if (nAñosH == 0)
                    {
                        nAñosH = 1;
                        añosH = new int[1];
                        apH = new float[1];
                        añosH[0] = dataset._AportacionAltAnualOrdAños.año[i];
                        apH[0] = dataset._AportacionAltAnualOrdAños.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosH, nAñosH + 1);
                        Array.Resize(ref apH, nAñosH + 1);
                        añosH[nAñosH] = dataset._AportacionAltAnualOrdAños.año[i];
                        apH[nAñosH] = dataset._AportacionAltAnualOrdAños.aportacion[i];
                        nAñosH++;
                    }
                }
                else if (dataset._AportacionAltAnualOrdAños.tipo != null && dataset._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                {
                    if (nAñosM == 0)
                    {
                        nAñosM = 1;
                        añosM = new int[1];
                        apM = new float[1];
                        añosM[0] = dataset._AportacionAltAnualOrdAños.año[i];
                        apM[0] = dataset._AportacionAltAnualOrdAños.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosM, nAñosM + 1);
                        Array.Resize(ref apM, nAñosM + 1);
                        añosM[nAñosM] = dataset._AportacionAltAnualOrdAños.año[i];
                        apM[nAñosM] = dataset._AportacionAltAnualOrdAños.aportacion[i];
                        nAñosM++;
                    }
                }
                else if (nAñosS == 0)
                {
                    nAñosS = 1;
                    añosS = new int[1];
                    apS = new float[1];
                    añosS[0] = dataset._AportacionAltAnualOrdAños.año[i];
                    apS[0] = dataset._AportacionAltAnualOrdAños.aportacion[i];
                }
                else
                {
                    Array.Resize(ref añosS, nAñosS + 1);
                    Array.Resize(ref apS, nAñosS + 1);
                    añosS[nAñosS] = dataset._AportacionAltAnualOrdAños.año[i];
                    apS[nAñosS] = dataset._AportacionAltAnualOrdAños.aportacion[i];
                    nAñosS++;
                }
            }

            Array.Sort(añosH, apH);
            Array.Sort(añosM, apM);
            Array.Sort(añosS, apS);

            offsetCol = 13;
            offsetRow = 15;

            for (int i = 0; i <= añosH.Length - 1; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosH[i] + "-" + (añosH[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apH[i];
            }

            offsetCol = 15;
            for (int i = 0; i <= añosM.Length - 1; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosM[i] + "-" + (añosM[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apM[i];
            }


            offsetCol = 17;
            for (int i = 0; i <= añosS.Length - 1; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosS[i] + "-" + (añosS[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apS[i];
            }

            objSheet.Calculate();

            ExcelChart ec = (ExcelChart)objSheet.Drawings[0];

            ec.Series[0].Series = "'Informe nº1a'!$Z$14:$Z$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);
            ec.Series[0].XSeries = "'Informe nº1a'!$Y$14:$Y$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);

            ec.Series[1].Series = "'Informe nº1a'!$AA$14:$AA$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);
            ec.Series[1].XSeries = "'Informe nº1a'!$Y$14:$Y$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);

            ec.Series[2].Series = "'Informe nº1a'!$AB$14:$AB$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);
            ec.Series[2].XSeries = "'Informe nº1a'!$Y$14:$Y$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);

            sRango = "N16:O";
            sRango = sRango + (16 + nAñosH - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            sRango = "P16:Q";
            sRango = sRango + (16 + nAñosM - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            sRango = "R16:S";
            sRango = sRango + (16 + nAñosS - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        }
    }
}
