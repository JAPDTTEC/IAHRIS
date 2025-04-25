using MultiLangXML;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe1bSR : Informe
    {
        public Informe1bSR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            
           
            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);

            // Escribir cabecera

            objSheet.Cells["F7"].Value = DateTime.Now.ToShortDateString();


            //1
            int offsetRow = 13;
            int offsetCol = 1;

            int MinAño = int.MaxValue;
            int MaxAño = 0;

            //Determinamos el año inicial y el año final absoluto de la combinación de Nat y alt

            for (int i = 0; i <= dataset._AportacionNatAnualOrdAños.año.Length - 1; i++)
            {
                if (dataset._AportacionNatAnualOrdAños.año[i] < MinAño) MinAño = dataset._AportacionNatAnualOrdAños.año[i];
                if (dataset._AportacionNatAnualOrdAños.año[i] > MaxAño) MaxAño = dataset._AportacionNatAnualOrdAños.año[i];
            }
            for (int i = 0; i <= dataset._AportacionAltAnualOrdAños.año.Length - 1; i++)
            {
                if (dataset._AportacionAltAnualOrdAños.año[i] < MinAño) MinAño = dataset._AportacionAltAnualOrdAños.año[i];
                if (dataset._AportacionAltAnualOrdAños.año[i] > MaxAño) MaxAño = dataset._AportacionAltAnualOrdAños.año[i];
            }



            int rowLength = 0; //Valor que lleva la cuenta de las row que se han escrito.

            //Recorremos todos los años del intervalo
            for (int a = MinAño; a < MaxAño + 1; a++)
            {
                bool YearHasData = false;
                for (int i = 0; i <= dataset._AportacionNatAnualOrdAños.año.Length - 1; i++)
                {
                    //Comprobamos si hay valor natural para ese año
                    if (dataset._AportacionNatAnualOrdAños.año[i] == a)
                    {
                        objSheet.Cells[offsetRow + rowLength + 1, offsetCol + 2].Value = dataset._AportacionNatAnualOrdAños.aportacion[i];
                        YearHasData = true;
                        break;
                    }
                }
                for (int i = 0; i <= dataset._AportacionAltAnualOrdAños.año.Length - 1; i++)
                {
                    //Comprobamos si hay valor Alterado para ese año
                    if (dataset._AportacionAltAnualOrdAños.año[i] == a)
                    {
                        objSheet.Cells[offsetRow + rowLength + 1, offsetCol + 3].Value = dataset._AportacionAltAnualOrdAños.aportacion[i];
                        YearHasData = true;
                        break;
                    }
                }
                if (YearHasData) //Si alguno de los regímenes tenía datos, escribimos el año en cabecera
                {
                    objSheet.Cells[offsetRow + rowLength + 1, offsetCol + 1].Value = a.ToString() + "-" + (a + 1).ToString().Substring(2);
                    rowLength++; //Y pasamos row.
                }
            }




            //Formateo de todas las celdas de la tabla.
            string sRango = "B14:D";
            sRango = sRango + (13 + rowLength).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            // objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


            objSheet.Calculate();


            ExcelChart ec = (ExcelChart)objSheet.Drawings[0];

            ec.Series[0].Series = "'Informe nº1b SR'!$C$14:$C$" + (13 + rowLength);
            ec.Series[0].XSeries = "'Informe nº1b SR'!$Z$14:$Z$" + (13 + rowLength);
            ec.Series[1].Series = "'Informe nº1b SR'!$D$14:$D$" + (13 + rowLength);
            ec.Series[1].XSeries = "'Informe nº1b SR'!$Z$14:$Z$" + (13 + rowLength);

            //Ponemos los valores personalizados del gráfico, necesarios para que se visualize correctamente

            ec = (ExcelChart)objSheet.Drawings[1];

            ec.Series[0].Series = "'Informe nº1b SR'!$C$14:$C$" + (13 + rowLength);
            ec.Series[0].XSeries = "'Informe nº1b SR'!$Z$14:$Z$" + (13 + rowLength);

            ec.Series[1].Series = "'Informe nº1b SR'!$D$14:$D$" + (13 + rowLength);
            ec.Series[1].XSeries = "'Informe nº1b SR'!$Z$14:$Z$" + (13 + rowLength);

           
        }
    }
}
