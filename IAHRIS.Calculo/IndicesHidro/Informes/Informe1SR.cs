using MultiLangXML;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe1SR : Informe
    {

        public Informe1SR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
            
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);

            calculations.CalcularINTERAnual();
            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

            /** Cabecera **/
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();


            /** Datos **/
            int filaAños = 13;
            int colAños = 1;

            for (int i = 0; i <= dataset._AportacionNatAnualOrdAños.año.Length - 1; i++)
            {
                objSheet.Cells[filaAños + i + 1, colAños + 1].Value = dataset._AportacionNatAnualOrdAños.año[i].ToString() + "-" + (dataset._AportacionNatAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[filaAños + i + 1, colAños + 2].Value = dataset._AportacionNatAnualOrdAños.aportacion[i];
            }

            /** Resultados **/
            string sRango = "B14:C" + (14 + dataset._AportacionNatAnualOrdAños.año.Length - 1).ToString();

            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            /**Grafico **/
            objSheet.Calculate();
            ExcelChart ec = (ExcelChart)objSheet.Drawings[1];

            ec.Series[0].Series = "'Informe nº1 SR'!$Y$14:$Y$" + (filaAños + dataset._AportacionNatAnualOrdAños.año.Length);
            ec.Series[0].XSeries = "'Informe nº1 SR'!$W$14:$W$" + (filaAños + dataset._AportacionNatAnualOrdAños.año.Length);

           

        }
    }
}
