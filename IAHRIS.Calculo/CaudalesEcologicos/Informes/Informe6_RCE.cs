
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using MultiLangXML;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    public class Informe6_RCE : Informe_RCE
    {
        public Informe6_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }
        public override void Escribir(ExcelPackage excel, SerieRCE serie)
        {  
            try
            {
                ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);
                EscenarioDTO R_NORM = null;
                try
                {
                    R_NORM = serie.Lista_Escenarios_Seleccionados.Where(x => x.Nombre == "R_NORM").First();
                }
                catch (Exception e) { }
                List<EscenarioDTO> EscenariosUser = serie.Lista_Escenarios_Seleccionados.Where(x => x.Nombre != "R_NORM").ToList();

                List<double[]> CaudalesRA_RNORM = new List<double[]>();
                List<double[]> CaudalesRA_User1 = new List<double[]>();
                List<double[]> CaudalesRA_User2 = new List<double[]>();

                if (R_NORM!=null && R_NORM.Caudales_Ecologicos.All(valor => valor != 0))
                {
                    foreach (double[] caudal in serie.Caudales_R_ALT)
                    {
                        int año = (int)caudal[0];
                        int mes = (int)caudal[1];
                        double caudalValor = caudal[2];

                        // Obtener el índice correspondiente en R_NORM.Caudales_Ecologicos
                        int indiceMes = (mes + 12 - serie.Mes_Inicio) % 12;

                        // Comparar los caudales
                        if (caudalValor > R_NORM.Caudales_Ecologicos[indiceMes])
                        {
                            // Si el caudal es mayor, guardar el valor de serie.Caudales_R_ALT
                            CaudalesRA_RNORM.Add(new double[] { año, mes, caudalValor });
                        }
                        else
                        {
                            // Si el caudal es menor o igual, guardar el valor de R_NORM.Caudales_Ecologicos
                            CaudalesRA_RNORM.Add(new double[] { año, mes, R_NORM.Caudales_Ecologicos[indiceMes] });
                        }

                    }
                }


                if (EscenariosUser.Count != 0)
                {
                    foreach (double[] caudal in serie.Caudales_R_ALT)
                    {
                        int año = (int)caudal[0];
                        int mes = (int)caudal[1];
                        double caudalValor = caudal[2];


                        int indiceMes = (mes + 12 - serie.Mes_Inicio) % 12;


                        if (caudalValor > EscenariosUser[0].Caudales_Ecologicos[indiceMes])
                        {

                            CaudalesRA_User1.Add(new double[] { año, mes, caudalValor });
                        }
                        else
                        {

                            CaudalesRA_User1.Add(new double[] { año, mes, EscenariosUser[0].Caudales_Ecologicos[indiceMes] });
                        }
                    }
                }

                if (EscenariosUser.Count == 2)
                {
                    foreach (double[] caudal in serie.Caudales_R_ALT)
                    {
                        int año = (int)caudal[0];
                        int mes = (int)caudal[1];
                        double caudalValor = caudal[2];


                        int indiceMes = (mes + 12 - serie.Mes_Inicio) % 12;


                        if (caudalValor > EscenariosUser[1].Caudales_Ecologicos[indiceMes])
                        {

                            CaudalesRA_User2.Add(new double[] { año, mes, caudalValor });
                        }
                        else
                        {

                            CaudalesRA_User2.Add(new double[] { año, mes, EscenariosUser[1].Caudales_Ecologicos[indiceMes] });
                        }

                    }

                }



                
                int fila = 0;
                int columna = 0;
                int filaInicio = 20;
                int columnaInicio = 4;

                if (R_NORM != null && R_NORM.Caudales_Ecologicos.All(valor => valor != 0))
                {
                    foreach (double[] valor in CaudalesRA_RNORM)
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
                        }

                        objSheet.Cells[filaInicio + fila, columnaInicio + 1 + columna].Value = Math.Round(valor[2], 3);

                        columna++;
                    }

                    string sRango = "D20:P";
                    sRango += (filaInicio + (int)(CaudalesRA_RNORM.Count / 12) - 1).ToString();

                    objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
                    objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                fila = 0;
                columna = 0;
                filaInicio = 20;
                columnaInicio = 19;

                if (CaudalesRA_User1.Count!=0)
                {
                    foreach (double[] valor in CaudalesRA_User1)
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
                        }

                        objSheet.Cells[filaInicio + fila, columnaInicio + 1 + columna].Value = Math.Round(valor[2], 3);

                        columna++;
                    }


                    string sRango = "S20:AE";
                    sRango += (filaInicio + (int)(CaudalesRA_User1.Count / 12) - 1).ToString();

                    objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
                    objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                fila = 0;
                columna = 0;
                filaInicio = 20;
                columnaInicio = 34;

                if (CaudalesRA_User2.Count != 0)
                {
                    foreach (double[] valor in CaudalesRA_User2)
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
                        }

                        objSheet.Cells[filaInicio + fila, columnaInicio + 1 + columna].Value = Math.Round(valor[2], 3);

                        columna++;
                    }
                    string sRango = "AI20:AT";
                    sRango += (filaInicio + (int)(CaudalesRA_User2.Count / 12) - 1).ToString();

                    objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
                    objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
    }
}
