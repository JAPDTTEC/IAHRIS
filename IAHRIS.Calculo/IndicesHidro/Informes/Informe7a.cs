using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe7a : Informe
    {
        public Informe7a(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,  ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalcularIndicesHabitualesCASO3();
            calculations.CalcularIndiceHabitual_I3();
            calculations.CalcularIndiceAlteracionGlobalHabituales();

            ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];

           
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 1: Magnitud aportaciones anuales +++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            int offsetRow = 16;
            int offsetCol = 3;

            string s = "";
            if (dataset._IndicesHabituales[0].calculado)
            {
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[0]) + "17"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[0]) + "17"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[0].valor[0];
                if (dataset._IndicesHabituales[0].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[0].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                offsetRow = 22;

                s = "";
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[1]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[1]) + "23"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[1]) + "23"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[0].valor[1];
                if (dataset._IndicesHabituales[0].invertido[1])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[0].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                offsetRow = 28;

                s = "";
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[2]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[2]) + "29"]);

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[2]) + "29"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[0].valor[2];
                if (dataset._IndicesHabituales[0].invertido[2])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[0].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                offsetRow = 34;

                s = "";
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[0].valor[3];
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[3]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[3]) + "35"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[0].valor[3]) + "35"].Value = null;
                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells["E15"].Value = ("#");
                objSheet.Cells["E21"].Value = ("#");
                objSheet.Cells["E27"].Value = ("#");
                objSheet.Cells["E33"].Value = ("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 2: Magnitud aportaciones mensuales +
            // +++++++++++++++++++++++++++++++++++++++++++++++
            offsetRow = 17;

            s = "";
            if (dataset._IndicesHabituales[1].calculado)
            {
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[0]) + "18"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[0]) + "18"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[1].valor[0];
                if (dataset._IndicesHabituales[1].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[1].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                offsetRow = 23;

                s = "";
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[1]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[1]) + "24"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[1]) + "24"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[1].valor[1];

                if (dataset._IndicesHabituales[1].invertido[1])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[1].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                offsetRow = 29;

                s = "";
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[2]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[2]) + "30"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[2]) + "30"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[1].valor[2];
                if (dataset._IndicesHabituales[1].invertido[2])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[1].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                offsetRow = 35;


                s = "";
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[3]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[3]) + "36"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[1].valor[3]) + "36"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[1].valor[3];
                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;

                offsetCol = 21;
                offsetRow = 18;


                for (int i = 0; i <= 11; i++)
                {
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Hum.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 3].Value = GetInf7Sign(dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Hum.Inverso, dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Hum.Indeterminado, dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Hum.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 4].Value = dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Med.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 5].Value = GetInf7Sign(dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Med.Inverso, dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Med.Indeterminado, dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Med.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 6].Value = dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Sec.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 7].Value = GetInf7Sign(dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Sec.Inverso, dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Sec.Indeterminado, dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Sec.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 8].Value = dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Pond.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 9].Value = (dataset.IAH2.Mensual[(i + datos.mesInicio - 1) % 12].IAH2Pond.DistribucionAtipica ? "$" : "");


                }
            }
            else
            {
                objSheet.Cells["E16"].Value = ("#");
                objSheet.Cells["E22"].Value = ("#");
                objSheet.Cells["E28"].Value = ("#");
                objSheet.Cells["E34"].Value = ("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 3: Variabilidad Habitual  ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            offsetRow = 18;
            offsetCol = 3;
            s = "";
            if (dataset._IndicesHabituales[2].calculado)
            {
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[0]) + "19"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[0]) + "19"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[2].valor[0];
                if (dataset._IndicesHabituales[2].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[2].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                offsetRow = 24;
                s = "";

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[1]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[1]) + "25"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[1]) + "25"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[2].valor[1];
                if (dataset._IndicesHabituales[2].invertido[1])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[2].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // ----------------------------------------------------------------------------
                offsetRow = 30;
                s = "";

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[2]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[2]) + "31"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[2]) + "31"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[2].valor[2];
                if (dataset._IndicesHabituales[2].invertido[2])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[2].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // ------------------------------------------------------------------------------
                offsetRow = 36;
                s = "";

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[3]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[3]) + "37"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[2].valor[3]) + "37"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[2].valor[3];
                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells["E17"].Value = ("#");
                objSheet.Cells["E23"].Value = ("#");
                objSheet.Cells["E29"].Value = ("#");
                objSheet.Cells["E35"].Value = ("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 4: Variabilidad Extrema  +++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            offsetRow = 19;
            s = "";
            if (dataset._IndicesHabituales[3].calculado)
            {
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[0]) + "20"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[0]) + "20"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[3].valor[0];
                if (dataset._IndicesHabituales[3].invertido[0])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[3].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // --------------------------------------------------------------------
                offsetRow = 25;
                s = "";

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[1]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[1]) + "26"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[1]) + "26"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[3].valor[1];
                if (dataset._IndicesHabituales[3].invertido[1])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[3].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // ---------------------------------------------------------------------
                offsetRow = 31;
                s = "";

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[2]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[2]) + "32"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[2]) + "32"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[3].valor[2];
                if (dataset._IndicesHabituales[3].invertido[2])
                {
                    s = "*";
                }

                if (dataset._IndicesHabituales[3].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // -----------------------------------------------------------------------
                offsetRow = 37;
                s = "";

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[3]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[3]) + "38"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[3].valor[3]) + "38"].Value = null;


                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[3].valor[3];
                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells["E18"].Value = ("#");
                objSheet.Cells["E24"].Value = ("#");
                objSheet.Cells["E30"].Value = ("#");
                objSheet.Cells["E36"].Value = ("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ [IAH5] Indice 6: Estacionalidad Maximos ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            offsetRow = 20;
            // s = ""
            if (dataset._IndicesHabituales[5].calculado)
            {
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[0]) + "21"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[0]) + "21"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[5].valor[0];
                offsetRow = 26;
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[1]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[1]) + "27"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[1]) + "27"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[5].valor[1];
                offsetRow = 32;

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[2]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[2]) + "33"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[2]) + "33"].Value = null;
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[5].valor[2];

                offsetRow = 38;
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[3]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[3]) + "39"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[5].valor[3]) + "39"].Value = null;
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[5].valor[3];
            }
            else
            {
                objSheet.Cells["E19"].Value = ("#");
                objSheet.Cells["E25"].Value = ("#");
                objSheet.Cells["E31"].Value = ("#");
                objSheet.Cells["E37"].Value = ("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ [IAH6] Indice 7: Estacionalidad Minimos ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            offsetRow = 21;
            s = "";
            if (dataset._IndicesHabituales[6].calculado)
            {
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[0]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[0]) + "22"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[0]) + "22"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[6].valor[0];
                offsetRow = 27;

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[1]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[1]) + "28"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[1]) + "28"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[6].valor[1];
                offsetRow = 33;

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[2]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[2]) + "34"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[2]) + "34"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[6].valor[2];
                offsetRow = 39;

                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[3]) + "16"].Copy(objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[3]) + "40"]);
                objSheet.Cells[DarColumna(dataset._IndicesHabituales[6].valor[3]) + "40"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = dataset._IndicesHabituales[6].valor[3];
            }
            else
            {
                objSheet.Cells["E20"].Value = ("#");
                objSheet.Cells["E26"].Value = ("#");
                objSheet.Cells["E32"].Value = ("#");
                objSheet.Cells["E38"].Value = ("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++
            // +++++ Indices IAG +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++
            offsetRow = 44;
            offsetCol = 6;

            int auxCol = 45;
            for (int i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = dataset._IndiceIAG[i];
                objSheet.Cells[DarColumnaGlobales(dataset._IndiceIAG[i]) + "16"].Copy(objSheet.Cells[DarColumnaGlobales(dataset._IndiceIAG[i]) + (auxCol + i).ToString()]);
                objSheet.Cells[DarColumnaGlobales(dataset._IndiceIAG[i]) + (auxCol + i).ToString()].Value = null;
            }
        }
        private string DarColumna(float valor)
        {
            if (valor > 0.8d)
            {
                return "I";
            }
            else if (valor > 0.6d)
            {
                return "J";
            }
            else if (valor > 0.4d)
            {
                return "K";
            }
            else if (valor > 0.2d)
            {
                return "L";
            }
            else
            {
                return "M";
            }
        }

        private string DarColumnaGlobales(float valor)
        {
            if (valor > 0.64d)
            {
                return "I";
            }
            else if (valor > 0.36d)
            {
                return "J";
            }
            else if (valor > 0.16d)
            {
                return "K";
            }
            else if (valor > 0.04d)
            {
                return "L";
            }
            else
            {
                return "M";
            }
        }

        private string GetInf7Sign(bool inverso, bool indeterminado, bool noCalculado)
        {
            if (noCalculado) return "#";
            if (inverso & !indeterminado) return "*";
            if (!inverso & indeterminado) return "**";
            if (inverso & indeterminado) return "***";
            return "";
        }


    }
}
