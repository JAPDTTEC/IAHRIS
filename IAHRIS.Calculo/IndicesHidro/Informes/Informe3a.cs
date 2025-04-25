using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe3a : Informe
    {
        public Informe3a(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorMeses(true);
            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir los meses
            int offsetRow = 17;
            int offsetCol = 2;


            for (int i = 0; i <= 11; i++)
            {

                if (float.IsNaN(dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].MedianaHumeda))
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = "#";
                else
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].MedianaHumeda;


                if (float.IsNaN(dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].MedianaMedia))
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = "#";
                else
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].MedianaMedia;


                if (float.IsNaN(dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].MedianaSeca))
                    objSheet.Cells[offsetRow + i, offsetCol + 3].Value = "#";
                else
                    objSheet.Cells[offsetRow + i, offsetCol + 3].Value = dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].MedianaSeca;

               // objSheet.Cells[offsetRow + i, offsetCol + 8].Value = dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].MedianaPonderada;


                if (dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].DistribucionAtipica)
                    objSheet.Cells[offsetRow + i, offsetCol + 7].Value = dataset.MensualCaracterizadaAlterada[((i + datos.mesInicio - 1) % 12)].DistrAtipDescriptor;


            }
        }
    }
}
