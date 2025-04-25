using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe5SR : Informe
    {
        public Informe5SR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorAños(true);
            calculations.CalcularParametrosHabitualesAlterados();
            calculations.CalculoParametrosVariabilidadDIARIAHabitualAlterada();
            calculations.CalculoParametrosAvenidasAlteradosCASO6();
            calculations.CalculoParametrosSequiasAlteradosCASO6();



            ExcelWorksheet objSheet = (excel.Workbook.Worksheets[Index]);

            // Escribir cabecera

            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow;
            int offsetCol;

            // Escribir el periodo de retorno
            objSheet.Cells["R29"].Value = Math.Round(dataset._Ave2TAlt / 2f);
            objSheet.Cells["R30"].Value = Math.Round(dataset._Ave2TAlt);

            Dictionary<string, float> aportacionesMaximas = new Dictionary<string, float>();
            Dictionary<string, float> aportacionesMinimas = new Dictionary<string, float>();

            for (int i = 0; i <= 11; i++)
            {
                aportacionesMaximas.Add(traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3), dataset._HabEstacionalidadMensualAlt[0].ndias[i]);
            }

            for (int i = 0; i <= 11; i++)
            {
                aportacionesMinimas.Add(traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3), dataset._HabEstacionalidadMensualAlt[1].ndias[i]);
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

            offsetCol = 16;
            offsetRow = 25;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._HabVariabilidadDiaraAlt[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._HabVariabilidadDiaraAlt[1];

            // AVENIDAS
            offsetRow = 27;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._AveMagnitudAlt[0];

            if (dataset._AveMagnitudAlt[1] == -9999)
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._AveMagnitudAlt[1];


            objSheet.Cells[offsetRow + 3, offsetCol + 1].Value = dataset._AveMagnitudAlt[2];
            objSheet.Cells[offsetRow + 4, offsetCol + 1].Value = dataset._AveMagnitudAlt[3];

            offsetRow = 31;

            if (dataset._AveVariabilidadAlt[0] < 0f)
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._AveVariabilidadAlt[0];


            if (dataset._AveVariabilidadAlt[1] < 0f)
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._AveVariabilidadAlt[1];


            objSheet.Cells["Q35"].Value = dataset._AveDuracionAlt;

            offsetRow = 45;
            offsetCol = 4;
            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = dataset._AveEstacionalidadAlt.ndias[i];


            // SEQUIAS
            offsetRow = 35;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._SeqMagnitudAlt[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._SeqMagnitudAlt[1];

            offsetRow = 37;
            if (dataset._SeqVariabilidadAlt[0] < 0f)
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._SeqVariabilidadAlt[0];


            if (dataset._SeqVariabilidadAlt[1] < 0f)
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            else
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = dataset._SeqVariabilidadAlt[1];


            offsetRow = 46;
            offsetCol = 4;
            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = dataset._SeqEstacionalidadAlt.ndias[i];

            offsetRow = 40;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._SeqDuracionAlt[0];


            // Lista de los Q = 0 por meses
            offsetRow = 47;
            offsetCol = 4;

            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = dataset._SeqDuracionCerosMesAlt.ndias[i];

            // Escribir los meses
            offsetRow = 44;

            for (int i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);

        }
    }
}
