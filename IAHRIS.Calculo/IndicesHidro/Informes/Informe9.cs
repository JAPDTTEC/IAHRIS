using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe9 : Informe
    {
        public Informe9(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel,ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularReferencias();

            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[(int)Index]);

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 99;
            int offsetCol = 2;

            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._1QMin;
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._7QMin;
            objSheet.Cells[offsetRow + 3, offsetCol + 1].Value = dataset._15QMin;
            objSheet.Cells[offsetRow + 4, offsetCol + 1].Value = dataset._7QRetorno[0];
            objSheet.Cells[offsetRow + 5, offsetCol + 1].Value = dataset._7QRetorno[1];
            objSheet.Cells[offsetRow + 6, offsetCol + 1].Value = dataset._7QRetorno[2];
            objSheet.Cells[offsetRow + 7, offsetCol + 1].Value = dataset._10QRetorno[0];
            objSheet.Cells[offsetRow + 8, offsetCol + 1].Value = dataset._10QRetorno[1];
            objSheet.Cells[offsetRow + 9, offsetCol + 1].Value = dataset._10QRetorno[2];
            objSheet.Cells[offsetRow + 10, offsetCol + 1].Value = dataset._mnQ[0];
            objSheet.Cells[offsetRow + 11, offsetCol + 1].Value = dataset._mnQ[1];
            objSheet.Cells[offsetRow + 12, offsetCol + 1].Value = dataset._mnQ[2];

            // Escribir los meses
            objSheet.Protection.IsProtected = true;
            objSheet.Protection.AllowSelectLockedCells = false;
        }
    }
}
