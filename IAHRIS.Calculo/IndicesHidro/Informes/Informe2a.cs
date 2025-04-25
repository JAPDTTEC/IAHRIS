using MultiLangXML;
using OfficeOpenXml;
using System;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe2a : Informe
    {
        public Informe2a(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularINTRAnualPorMeses(false);

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir los meses
            int offsetRow = 17;
            int offsetCol = 2;


            for (int i = 0; i <= 11; i++)
            {
                // ((i + datos.mesInicio - 1) % 12) = Los datos en la estructura están almacenados por mes en orden natural, siendo el índice 0 Enero y el 11 Diciembre. Esta 
                //conversión permite obtener el mes natural correcto a partir del índice i, que representa el mes ordenado en función al mes de inicio (Normalmente octubre=0).
                //La fórmula normal sería  ((i + datos.mesInicio - 1) % 12 + 1), pero se elimina el último +1 debido a que el índice es base 0, no base 1.

                if (float.IsNaN(dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].MedianaHumeda))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].MedianaHumeda;
                }
                if (float.IsNaN(dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].MedianaMedia))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].MedianaMedia;
                }
                if (float.IsNaN(dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].MedianaSeca))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 3].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 3].Value = dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].MedianaSeca;
                }

               // objSheet.Cells[offsetRow + i, offsetCol + 8].Value = dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].MedianaPonderada;
                if (dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].DistribucionAtipica)
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 7].Value = dataset.MensualCaracterizadaNatural[((i + datos.mesInicio - 1) % 12)].DistrAtipDescriptor;
                }
            }
        }
    }
}
