using MathNet.Numerics.Statistics;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    public class Informe1_RCE : Informe_RCE
    {
        public Informe1_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {

        }



        public override void Escribir(ExcelPackage excel,SerieRCE serie)
        {
            try
            {
                ExcelWorksheet objSheet = excel.Workbook.Worksheets[Index];
                double[][] aportaciones = serie.Aportaciones_R_NAT.ToArray();
                double[][] caudales = serie.Caudales_R_NAT.ToArray();
                int mes = serie.Mes_Inicio;

                //Rellenar meses
                for (int i = 1; i <= 12; i++)
                {
                    if (mes > 12) mes = 1;

                    objSheet.Cells[12, 2 + i].Value = Utiles.ObtenerMes(mes);
                    mes++;
                }


                //Rellenar Aportaciones
                RellenarConValores(aportaciones, objSheet, 13, 2, true);

                string sRango = "B13:O";
                sRango += (13 + (int)(aportaciones.Length/12) - 1).ToString();

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


                RellenarRN(caudales,serie.Mes_Inicio, objSheet);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }

        }
  

        private void RellenarConValores(double[][] datos, ExcelWorksheet objSheet, int filaInicio, int columnaInicio, bool sum)
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
                    int añoSiguiente = ((int)valor[0] + 1 ) % 100;
                    objSheet.Cells[filaInicio + fila, columnaInicio].Value = valor[0] + "-" + añoSiguiente;
 
                   string ea1 = objSheet.Cells[filaInicio + fila, columnaInicio + 1].Address;
                    string ea2 = objSheet.Cells[filaInicio + fila, columnaInicio + 12].Address;


                    if (sum) objSheet.Cells[filaInicio + fila, columnaInicio + 13].Formula = "=SUM(" + ea1 + ":" + ea2 + ")";
                }

                objSheet.Cells[filaInicio + fila, columnaInicio+1 + columna].Value = Math.Round(valor[2],3);

                columna++;
            }
            objSheet.Calculate();
        }

        private void RellenarRN(double[][] datos, int mes_inicio, ExcelWorksheet objSheet)
        {


           
            double[] RN_QMM = new double[12];
            double[] RN_P10 = new double[12];
            double[] RN_P50 = new double[12];
            double[] RN_P65 = new double[12];
            double[] RN_P70 = new double[12];
            double[] RN_P75 = new double[12];
            double[] RN_P80 = new double[12];
            double[] RN_P85 = new double[12];
            double[] RN_P90 = new double[12];
            double[] RN_P95 = new double[12];
            double[] RN_QMIN = new double[12];

            //QMM
            int[] count = new int[12];
            foreach (var dato in datos)
            {
                int mes = ((int)dato[1] + 12 - mes_inicio) % 12;
                RN_QMM[mes] += dato[2];
                count[mes]++;
                
            }

           
            for (int i = 0; i < RN_QMM.Length; i++)
            {
                if (count[i] != 0)
                {
                    RN_QMM[i] /= count[i];
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
                    RN_P10[i] = values.QuantileCustom(.9, QuantileDefinition.Weibull);
                    RN_P50[i] = values.QuantileCustom(.5, QuantileDefinition.Weibull);
                    RN_P65[i] = values.QuantileCustom(.35, QuantileDefinition.Weibull);
                    RN_P70[i] = values.QuantileCustom(.3, QuantileDefinition.Weibull);
                    RN_P75[i] = values.QuantileCustom(.25, QuantileDefinition.Weibull);
                    RN_P80[i] = values.QuantileCustom(.2, QuantileDefinition.Weibull);
                    RN_P85[i] = values.QuantileCustom(.15, QuantileDefinition.Weibull);
                    RN_P90[i] = values.QuantileCustom(.1, QuantileDefinition.Weibull);
                    RN_P95[i] = values.QuantileCustom(.05, QuantileDefinition.Weibull);
                    //En series de menos de 20 años el P95 va a coincidir con el QMIN por la escasa disponibilidad de datos.
                    RN_QMIN[i] = values.Min();
                }
            }
            
            for (int i = 0; i < 12; i++)
            {
                objSheet.Cells[13, 33+i].Value = Math.Round(RN_QMM[i],3);
                objSheet.Cells[14, 33 + i].Value = Math.Round(RN_P10[i],3);
                objSheet.Cells[15, 33 + i].Value = Math.Round(RN_P50[i], 3);
                objSheet.Cells[16, 33 + i].Value = Math.Round(RN_P65[i], 3);
                objSheet.Cells[17, 33 + i].Value = Math.Round(RN_P70[i], 3);
                objSheet.Cells[18, 33 + i].Value = Math.Round(RN_P75[i], 3);
                objSheet.Cells[19, 33 + i].Value = Math.Round(RN_P80[i], 3);
                objSheet.Cells[20, 33 + i].Value = Math.Round(RN_P85[i], 3);
                objSheet.Cells[21, 33 + i].Value = Math.Round(RN_P90[i], 3);
                objSheet.Cells[22, 33 + i].Value = Math.Round(RN_P95[i], 3);
                objSheet.Cells[23, 33 + i].Value = Math.Round(RN_QMIN[i], 3);
            }

        }
        

    }
}
