using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{

    public class Informe1 : Informe
    {
        public Informe1(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel,ref DatosCalculo datos,ref IAHRISDataSet dataset, MultiLangXML.MultiIdiomasXML traductor  )
        {
            
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTERAnual();


            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 13;
            int offsetCol = 1;

            for (int i = 0; i <= dataset._AportacionNatAnualOrdAños.año.Length - 1; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = dataset._AportacionNatAnualOrdAños.año[i].ToString() + "-" + (dataset._AportacionNatAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = dataset._AportacionNatAnualOrdAños.aportacion[i];
            }

            string sRango = "B14:C";
            sRango += (14 + dataset._AportacionNatAnualOrdAños.año.Length - 1).ToString();

            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int nAñosH = 0;
            int nAñosM = 0;
            int nAñosS = 0;
            int[] añosH = null;
            int[] añosM = null;
            int[] añosS = null;
            float[] apH = null;
            float[] apM = null;
            float[] apS = null;

            for (int i = 0; i <= dataset._AportacionNatAnual.año.Length - 1; i++)
            {
                if (dataset._AportacionNatAnual.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    if (nAñosH == 0)
                    {
                        nAñosH = 1;
                        añosH = new int[1];
                        apH = new float[1];
                        añosH[0] = dataset._AportacionNatAnual.año[i];
                        apH[0] = dataset._AportacionNatAnual.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosH, nAñosH + 1);
                        Array.Resize(ref apH, nAñosH + 1);
                        añosH[nAñosH] = dataset._AportacionNatAnual.año[i];
                        apH[nAñosH] = dataset._AportacionNatAnual.aportacion[i];
                        nAñosH = nAñosH + 1;
                    }
                }
                else if (dataset._AportacionNatAnual.tipo[i] == TIPOAÑO.MEDIO)
                {
                    if (nAñosM == 0)
                    {
                        nAñosM = 1;
                        añosM = new int[1];
                        apM = new float[1];
                        añosM[0] = dataset._AportacionNatAnual.año[i];
                        apM[0] = dataset._AportacionNatAnual.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosM, nAñosM + 1);
                        Array.Resize(ref apM, nAñosM + 1);
                        añosM[nAñosM] = dataset._AportacionNatAnual.año[i];
                        apM[nAñosM] = dataset._AportacionNatAnual.aportacion[i];
                        nAñosM++;
                    }
                }
                else if (nAñosS == 0)
                {
                    nAñosS = 1;
                    añosS = new int[1];
                    apS = new float[1];
                    añosS[0] = dataset._AportacionNatAnual.año[i];
                    apS[0] = dataset._AportacionNatAnual.aportacion[i];
                }
                else
                {
                    Array.Resize(ref añosS, nAñosS + 1);
                    Array.Resize(ref apS, nAñosS + 1);
                    añosS[nAñosS] = dataset._AportacionNatAnual.año[i];
                    apS[nAñosS] = dataset._AportacionNatAnual.aportacion[i];
                    nAñosS++;
                }
            }

            Array.Sort(añosH, apH); Array.Sort(añosM, apM); Array.Sort(añosS, apS);

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

            ec.Series[0].Series = "'Informe nº1'!$Y$14:$Y$" + (13 + dataset._AportacionNatAnualOrdAños.año.Length);
            ec.Series[0].XSeries = "'Informe nº1'!$W$14:$W$" + (13 + dataset._AportacionNatAnualOrdAños.año.Length);

            ec.Series[1].Series = "'Informe nº1'!$X$14:$X$" + (13 + dataset._AportacionNatAnualOrdAños.año.Length);
            ec.Series[1].XSeries = "'Informe nº1'!$W$14:$W$" + (13 + dataset._AportacionNatAnualOrdAños.año.Length);
             
            ec.Series[2].Series = "'Informe nº1'!$Z$14:$Z$" + (13 + dataset._AportacionNatAnualOrdAños.año.Length);
            ec.Series[2].XSeries = "'Informe nº1'!$W$14:$W$" + (13 + dataset._AportacionNatAnualOrdAños.año.Length);


            //((Range)m_Excel.Selection).Copy();
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
