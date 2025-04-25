using MultiLangXML;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe1aSR : Informe
    {
        public Informe1aSR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTERAnualAlterada();

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];


            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();


            int offsetRow = 14;
            int offsetCol = 2;

            for (int i = 0; i <= dataset._AportacionAltAnualOrdAños.año.Length - 1; i++)
            {
                objSheet.Cells[offsetRow + i, offsetCol].Value = dataset._AportacionAltAnualOrdAños.año[i] + "-" + (dataset._AportacionAltAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i, offsetCol + 1].Value = dataset._AportacionAltAnualOrdAños.aportacion[i];
            }

            string sRango = "B14:C";
            sRango = sRango + (14 + dataset._AportacionAltAnualOrdAños.año.Length - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;



            objSheet.Calculate();

            ExcelChart ec = (ExcelChart)objSheet.Drawings[0];

            ec.Series[0].Series = "'Informe nº1a SR'!$Z$14:$Z$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);
            ec.Series[0].XSeries = "'Informe nº1a SR'!$Y$14:$Y$" + (13 + dataset._AportacionAltAnualOrdAños.año.Length);

        }
    }
}
