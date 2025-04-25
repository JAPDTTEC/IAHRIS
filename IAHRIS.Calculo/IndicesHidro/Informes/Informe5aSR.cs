using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    internal class Informe5aSR : Informe
    {
        public Informe5aSR(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorAños(true);
            calculations.CalcularParametrosHabitualesAlterados();


            ExcelWorksheet objSheet = (excel.Workbook.Worksheets[Index]);

            // Escribir cabecera

            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
          
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
                if(item.Value >cantMaxAportaciones)
                {
                    cantMaxAportaciones = item.Value;
                    mesMaxAportaciones = item.Key;
                }
                else if (item.Value == cantMaxAportaciones)
                {
                    mesMaxAportaciones += ", "+item.Key;
                }
            }

            string mesMinAportaciones = "";
            float cantMinAportaciones = 0;

            foreach (var item in aportacionesMinimas)
            {
                if (item.Value > cantMinAportaciones)
                {
                    cantMinAportaciones = item.Value;
                    mesMinAportaciones = item.Key;
                }
                else if (item.Value == cantMinAportaciones)
                {
                    mesMinAportaciones += ", " + item.Key;
                }
            }

            
            objSheet.Cells["O23"].Value = mesMaxAportaciones;
            objSheet.Cells["O25"].Value = mesMinAportaciones;
        }
    }
}
