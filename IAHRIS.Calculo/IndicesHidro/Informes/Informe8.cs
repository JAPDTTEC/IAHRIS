using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe8 : Informe
    {
        public Informe8(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularRegimenNatural();
            calculations.CalcularRegimenAlterado();
            calculations.CalcularRegimenNaturalAnual();
            calculations.CalcularRegimenAlteradoAnual();

            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);

            // Escribir cabecera
            
            objSheet.Cells["D7"].Value = DateTime.Now.ToShortDateString();

            // Escribir años usados
            int offsetRow = 37;
            int offsetCol = 1;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._AportacionNatAnual.año.Length;
            objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = dataset._AportacionAltAnual.año.Length;
            objSheet.Cells[offsetRow + 1, offsetCol + 3].Value = datos.nAnyosCoe;

            // Escribir los meses
            offsetRow = 15;
            offsetCol = 1;

            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString());

            objSheet.Protection.IsProtected = true;
            objSheet.Protection.AllowSelectLockedCells = false;
        }
    }
}
