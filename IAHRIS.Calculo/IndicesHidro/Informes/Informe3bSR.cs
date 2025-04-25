using MultiLangXML;
using OfficeOpenXml;
using System;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe3bSR : Informe
    {
        public Informe3bSR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);


            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }
    }
}