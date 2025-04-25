
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using MathNet.Numerics.Statistics;
using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    public class Informe2_RCE : Informe_RCE
    {
        public Informe2_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }


        public override void Escribir(ExcelPackage excel, SerieRCE serie)
        {
            try
            {
                ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];
                double[][] aportaciones = serie.Aportaciones_R_ALT.ToArray();
                double[][] caudales = serie.Caudales_R_ALT.ToArray();
                int mes = serie.Mes_Inicio;


                //Rellenar Aportaciones
                RellenarConValores(aportaciones, objSheet, 13, 2,true);

                string sRango = "B13:O";
                sRango += (13 + (int)(aportaciones.Length / 12) - 1).ToString();

                objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
                objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //Rellenar Caudales
                RellenarConValores(caudales, objSheet, 13, 17,false);

                sRango = "Q13:AC";
                sRango += (13 + (int)(caudales.Length / 12) - 1).ToString();

                objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
                objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                RellenarRN(caudales, serie.Mes_Inicio, objSheet);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }


        private void RellenarConValores(double[][] datos, ExcelWorksheet objSheet, int filaInicio, int columnaInicio,bool sum)
        {
            int fila = 0;
            int columna = 0;

            //Rellenar año Mes
            foreach (double[] valor in datos)
            {
                if (columna >= 12)
                {
                    fila++;
                    columna = 0;
                }

                if (columna == 0)
                {
                    int añoSiguiente = ((int)valor[0] + 1) % 100;
                    objSheet.Cells[filaInicio + fila, columnaInicio].Value = valor[0] + "-" + añoSiguiente;


                    string ea1 = objSheet.Cells[filaInicio + fila, columnaInicio + 1].Address;
                    string ea2 = objSheet.Cells[filaInicio + fila, columnaInicio + 12].Address;


                    if (sum) objSheet.Cells[filaInicio + fila, columnaInicio + 13].Formula = "=SUM(" + ea1 + ":" + ea2 + ")";
                }

                objSheet.Cells[filaInicio + fila, columnaInicio + 1 + columna].Value = Math.Round(valor[2], 2);

                columna++;
            }
        }
        private void RellenarRN(double[][] datos, int mes_inicio, ExcelWorksheet objSheet)
        {



            double[] RA_QMM = new double[12];
            double[] RA_P10 = new double[12];
            double[] RA_P50 = new double[12];
            double[] RA_P65 = new double[12];
            double[] RA_P70 = new double[12];
            double[] RA_P75 = new double[12];
            double[] RA_P80 = new double[12];
            double[] RA_P85 = new double[12];
            double[] RA_P90 = new double[12];
            double[] RA_P95 = new double[12];
            double[] RA_QMIN = new double[12];

            //QMM
            int[] count = new int[12];
            foreach (var dato in datos)
            {
                int mes = ((int)dato[1] + 12 - mes_inicio) % 12;
                RA_QMM[mes] += dato[2];
                count[mes]++;

            }


            for (int i = 0; i < RA_QMM.Length; i++)
            {
                if (count[i] != 0)
                {
                    RA_QMM[i] /= count[i];
                }
            }

            for (int i = 0; i < 12; i++)
            {
                if (count[i] != 0)
                {
                    double[] values = new double[count[i]];
                    int index = 0;
                    foreach (var dato in datos)
                    {
                        int mes = ((int)dato[1] + 12 - mes_inicio) % 12;
                        if (mes == i)
                        {
                            values[index++] = dato[2];
                        }
                    }
                    RA_P10[i] = values.QuantileCustom(.9, QuantileDefinition.Weibull);
                    RA_P50[i] = values.QuantileCustom(.5, QuantileDefinition.Weibull);
                    RA_P65[i] = values.QuantileCustom(.35, QuantileDefinition.Weibull);
                    RA_P70[i] = values.QuantileCustom(.3, QuantileDefinition.Weibull);
                    RA_P75[i] = values.QuantileCustom(.25, QuantileDefinition.Weibull);
                    RA_P80[i] = values.QuantileCustom(.2, QuantileDefinition.Weibull);
                    RA_P85[i] = values.QuantileCustom(.15, QuantileDefinition.Weibull);
                    RA_P90[i] = values.QuantileCustom(.1, QuantileDefinition.Weibull);
                    RA_P95[i] = values.QuantileCustom(.05, QuantileDefinition.Weibull);
                    //En series de menos de 20 años el P95 va a coincidir con el QMIN por la escasa disponibilidad de datos.
                    RA_QMIN[i] = values.Min();
                }
            }
            
            for (int i = 0; i < 12; i++)
            {
                objSheet.Cells[13, 33 + i].Value = Math.Round(RA_QMM[i], 3);
                objSheet.Cells[14, 33 + i].Value = Math.Round(RA_P10[i], 3);
                objSheet.Cells[15, 33 + i].Value = Math.Round(RA_P50[i], 3);
                objSheet.Cells[16, 33 + i].Value = Math.Round(RA_P65[i], 3);
                objSheet.Cells[17, 33 + i].Value = Math.Round(RA_P70[i], 3);
                objSheet.Cells[18, 33 + i].Value = Math.Round(RA_P75[i], 3);
                objSheet.Cells[19, 33 + i].Value = Math.Round(RA_P80[i], 3);
                objSheet.Cells[20, 33 + i].Value = Math.Round(RA_P85[i], 3);
                objSheet.Cells[21, 33 + i].Value = Math.Round(RA_P90[i], 3);
                objSheet.Cells[22, 33 + i].Value = Math.Round(RA_P95[i], 3);
                objSheet.Cells[23, 33 + i].Value = Math.Round(RA_QMIN[i], 3);
            }

        }
    }
}
