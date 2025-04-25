using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe3 : Informe
    {
        public Informe3(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTERAnualAlterada();
            calculations.CalcularINTRAnualPorMeses(true);
            calculations.CalcularParametrosHabitualesReducidos();

            ExcelWorksheet objSheet = (excel.Workbook.Worksheets[Index]);

            // Escribir cabecera
           
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir los meses
            int offsetRow = 13;
            int offsetCol = 2;


            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);

            int years = 0;
            for (int i = 0; i < dataset._AportacionAltMen.aportacion.Length; i++)
            {
                int month = dataset._AportacionAltMen.mes[i].Month;
                int year = dataset._AportacionAltMen.mes[i].Year;
                int posm = month - datos.mesInicio;

                if (posm < 0) posm = 12 + posm;

                string hyear = "";
                if (month == datos.mesInicio)
                {
                    years++;
                    if (datos.mesInicio == 1)
                        hyear = year.ToString();
                    else
                        hyear = year.ToString() + "-" + (year + 1).ToString();
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Value = hyear;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.Fill.SetBackground(Color.FromArgb(220, 230, 241), OfficeOpenXml.Style.ExcelFillStyle.Solid);
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    int line = years + 14;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + 13].Formula = "= MAX(C" + line + ": N" + line + ") - MIN(C" + line + ": N" + line + ")";
                   

                }

                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Value = dataset._AportacionAltMen.aportacion[i];
                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Style.Border.Bottom.Color.SetColor(Color.FromArgb(79, 129, 189));


            }
            objSheet.Cells["B15:N" + (14 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            objSheet.Cells["B15:B" + (14 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            objSheet.Cells["O15:O" + (14 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);

            offsetRow = 17;
            offsetCol = 19;


            for (int i = 0; i <= 11; i++)
            {
                objSheet.Cells[offsetRow, offsetCol + i + 1].Value = dataset._HabEstacionalidadMensualAlt[0].ndias[i];
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = dataset._HabEstacionalidadMensualAlt[1].ndias[i];
            }

        }
    }
}
