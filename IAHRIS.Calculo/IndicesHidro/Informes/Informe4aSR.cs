using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe4aSR : Informe
    {
        public Informe4aSR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorAños(false);
            calculations.CalcularParametrosHabitualesCASO1();

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetCol = 15;

            //Magnitud
            int offsetRow = 15;
            objSheet.Cells[offsetRow, offsetCol].Value = dataset._HabMagnitudNat[3];

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
                    mesMaxAportaciones += "-" + item.Key.ToUpper();
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
                    mesMinAportaciones += "-" + item.Key.ToUpper();
                }
            }


            objSheet.Cells["O23"].Value = mesMaxAportaciones;
            objSheet.Cells["O25"].Value = mesMinAportaciones;

        }
    }
}
