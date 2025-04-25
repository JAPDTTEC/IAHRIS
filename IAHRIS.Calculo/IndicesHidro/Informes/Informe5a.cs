using MultiLangXML;
using OfficeOpenXml;
using System;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe5a : Informe
    {
        public Informe5a(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorAños(true);
            calculations.CalcularParametrosHabitualesAlterados();

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 14;
            int offsetCol = 16;

            int pos = 0;
            for (int i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow + pos + 1, offsetCol + 1].Value = dataset._HabMagnitudAlt[i];
                pos++;
            }

            for (int i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow + pos + 1, offsetCol + 1].Value = dataset._HabVariabilidadAlt[i];
                pos++;
            }

            for (int i = 0; i <= 2; i++)
            {
                objSheet.Cells[offsetRow + pos + 1, offsetCol + 1].Value = dataset._HabEstacionalidadAlt[i];
                pos++;
            }
        }
    }
}
