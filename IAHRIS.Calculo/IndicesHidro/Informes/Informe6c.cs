using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe6c : Informe
    {
        public Informe6c(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularCurvaMensualClasificadaCudales(false);

            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 17;
            int offsetCol = 19;

            for (int m = 0; m < 12; m++)
            {
                int mpos = (m + 3) % 12;
                for (int k = 0; k < 31; k++)
                {
                    try
                    {
                        objSheet.Cells[offsetRow + k + 1, offsetCol + mpos + 1].Value =
                            dataset.CurvaClasificadaMensualCaudalNatural.SerieEstructurada[m].SerieClasificada.ValoresK[k].Media;
                    }
                    catch (ArgumentOutOfRangeException)
                    { }
                }
            }
        }
    }
}
