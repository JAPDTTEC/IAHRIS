using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe6a : Informe
    {
        public Informe6a(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularTablaCQC(true);

            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 17;
            int offsetCol = 11;

            for (int i = 0; i <= 364; i++)
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = dataset._TablaCQCAlt.añomedio[i];
        }
    }
}
