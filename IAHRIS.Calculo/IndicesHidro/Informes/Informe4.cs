using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe4 : Informe
    {
        public Informe4(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorAños(false);
            calculations.CalcularParametrosHabitualesCASO1();
            calculations.CalculoParametrosVariabilidadDIARIAHabitual();
            calculations.CalculoParametrosAvenidasCASO4();
            calculations.CalculoParametrosSequiasCASO4();

            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribo los Valores Habituales Mensuales
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

            // Escribo los habituales diarios
            offsetRow = 25;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._HabVariabilidadDiaraNat[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._HabVariabilidadDiaraNat[1];

            // AVENIDAS
            offsetRow = 27;

            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._AveMagnitudNat[0];
            if (dataset._AveMagnitudNat[1] == -9999)
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._AveMagnitudNat[1];

            objSheet.Cells[offsetRow + 3, offsetCol + 1].Value = dataset._AveMagnitudNat[2];
            objSheet.Cells[offsetRow + 4, offsetCol + 1].Value = dataset._AveMagnitudNat[3];

            // Escribir el periodo de retorno
            objSheet.Cells["R29"].Value = Math.Round(dataset._Ave2TNat / 2f);
            objSheet.Cells["R30"].Value = Math.Round(dataset._Ave2TNat);

            offsetRow = 31;

            if (dataset._AveVariabilidadNat[0] < 0f)
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._AveVariabilidadNat[0];

            if (dataset._AveVariabilidadNat[1] < 0f)
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._AveVariabilidadNat[1];

            objSheet.Cells["Q35"].Value = dataset._AveDuracionNat;

            offsetRow = 45;
            offsetCol = 4;
            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, i + offsetCol + 1].Value = dataset._AveEstacionalidadNat.ndias[i];

            // Escribir la media del año
            offsetRow = 35;
            offsetCol = 16;

            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._SeqMagnitudNat[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._SeqMagnitudNat[1];


            offsetRow = 37;

            if (dataset._SeqVariabilidadNat[0] < 0f)
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._SeqVariabilidadNat[0];


            if (dataset._SeqVariabilidadNat[1] < 0f)
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._SeqVariabilidadNat[1];
            }

            offsetRow = 46;
            offsetCol = 4;

            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = dataset._SeqEstacionalidadNat.ndias[i];

            // Escribir la media dell año
            offsetRow = 40;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._SeqDuracionNat[0];


            // Lista de los Q = 0 por meses
            offsetRow = 47;
            offsetCol = 4;

            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = dataset._SeqDuracionCerosMesNat.ndias[i];

            // Escribir los meses
            offsetRow = 44;
            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);

            
        }
    }
}
