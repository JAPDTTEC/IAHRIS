using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe4SR : Informe
    {
        public Informe4SR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorAños(false);
            calculations.CalcularParametrosHabitualesCASO1();
            calculations.CalculoParametrosVariabilidadDIARIAHabitual();
            calculations.CalculoParametrosAvenidasCASO4();
            calculations.CalculoParametrosSequiasCASO4();

            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);


            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 15;
            int offsetCol = 17;
            
            //Media de las aportaciones anuales.
            objSheet.Cells[offsetRow, offsetCol].Value = dataset._HabMagnitudNat[3];

            //Media de la diferencia entre aportación mensual maxima y minima en el año.
            offsetRow = 19;
            objSheet.Cells[offsetRow, offsetCol].Value = dataset._HabVariabilidadNat[1];


            //Estacionalidad.

            /*
            //Mes de máxima aportación
            offsetRow = 23;
            objSheet.Cells[offsetRow, offsetCol].Value = dataset._HabEstacionalidadNat[0];

            //Mes de mínima aportación
            offsetRow = 25;
            objSheet.Cells[offsetRow, offsetCol].Value = dataset._HabEstacionalidadNat[1];
            */

            Dictionary<string, float> aportacionesMaximas = new Dictionary<string, float>();
            Dictionary<string, float> aportacionesMinimas = new Dictionary<string, float>();

            for (int i = 0; i <= 11; i++)
            {
                aportacionesMaximas.Add(traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3), dataset._HabEstacionalidadMensualNat[0].ndias[i]);
            }

            for (int i = 0; i <= 11; i++)
            {
                aportacionesMinimas.Add(traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3), dataset._HabEstacionalidadMensualNat[1].ndias[i]);
            }

            string mesMaxAportaciones = "";
            float cantMaxAportaciones = 0;

            foreach (var item in aportacionesMaximas)
            {
                if (item.Value > cantMaxAportaciones)
                {
                    cantMaxAportaciones = item.Value;
                    mesMaxAportaciones = item.Key.ToUpper();
                }
                else if (item.Value == cantMaxAportaciones)
                {
                    mesMaxAportaciones += "- " + item.Key.ToUpper();
                }
            }

            string mesMinAportaciones = "";
            float cantMinAportaciones = 0;

            foreach (var item in aportacionesMinimas)
            {
                if (item.Value > cantMinAportaciones)
                {
                    cantMinAportaciones = item.Value;
                    mesMinAportaciones = item.Key.ToUpper();
                }
                else if (item.Value == cantMinAportaciones)
                {
                    mesMinAportaciones += "- " + item.Key.ToUpper();
                }
            }


            objSheet.Cells["Q23"].Value = mesMaxAportaciones;
            objSheet.Cells["Q25"].Value = mesMinAportaciones;


            // Escribo los habituales diarios
            offsetRow = 26;
            offsetCol = 17;
            objSheet.Cells[offsetRow, offsetCol].Value = dataset._HabVariabilidadDiaraNat[0];
            objSheet.Cells[offsetRow + 1, offsetCol].Value = dataset._HabVariabilidadDiaraNat[1];

            // AVENIDAS
            offsetRow = 28;

            objSheet.Cells[offsetRow, offsetCol].Value = dataset._AveMagnitudNat[0];
            if (dataset._AveMagnitudNat[1] == -9999)
                objSheet.Cells[offsetRow + 1, offsetCol].Value = "**";
            else
                objSheet.Cells[offsetRow + 1, offsetCol].Value = dataset._AveMagnitudNat[1];

            objSheet.Cells[offsetRow + 2, offsetCol].Value = dataset._AveMagnitudNat[2];
            objSheet.Cells[offsetRow + 3, offsetCol].Value = dataset._AveMagnitudNat[3];

            // Escribir el periodo de retorno
            objSheet.Cells["R29"].Value = Math.Round(dataset._Ave2TNat / 2f);
            objSheet.Cells["R30"].Value = Math.Round(dataset._Ave2TNat);

            offsetRow = 31;

            if (dataset._AveVariabilidadNat[0] < 0f)
                objSheet.Cells[offsetRow + 1, offsetCol].Value = "**";
            else
                objSheet.Cells[offsetRow + 1, offsetCol].Value = dataset._AveVariabilidadNat[0];

            if (dataset._AveVariabilidadNat[1] < 0f)
                objSheet.Cells[offsetRow + 2, offsetCol].Value = "**";
            else
                objSheet.Cells[offsetRow + 2, offsetCol].Value = dataset._AveVariabilidadNat[1];

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
