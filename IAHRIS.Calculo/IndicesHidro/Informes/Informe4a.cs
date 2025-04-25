using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe4a : Informe
    {
        public Informe4a(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorAños(false);
            calculations.CalcularParametrosHabitualesCASO1();

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 14;
            int offsetCol = 16;
            for (int i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._HabMagnitudNat[i];
                offsetRow++;
            }

            for (int i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._HabVariabilidadNat[i];
                offsetRow++;
            }

            for (int i = 0; i <= 2; i++)
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._HabEstacionalidadNat[i];
                offsetRow++;
            }
        }
    }
}
