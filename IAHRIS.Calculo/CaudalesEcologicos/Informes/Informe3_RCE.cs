
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    public class Informe3_RCE : Informe_RCE
    {
        public Informe3_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }


        public override void Escribir(ExcelPackage excel, SerieRCE serie)
        {
            try { 
            
                ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);
                List<EscenarioDTO> escenarios = serie.Lista_Escenarios_Predefinidos;
                List<EscenarioDTO> escenarios_Usuario = serie.Lista_Escenarios_Usuario.Where(x => x.Por_Defecto == false).ToList();


                objSheet.Cells["E6"].Value = DateTime.Now.ToShortDateString();

                string[] orden = {
                    "TENNANT", "TESSMAN", "VMF", "NMTM_P90", "NMTM_FAIR",
                    "NMTM_GOOD", "NMTM_EXCELLENT", "ADMM", "IAHRIS_P90", "IAHRIS_P85", "IAHRIS_P80","R_NORM"
                };

                List<EscenarioDTO> escenariosPorDefecto = serie.Lista_Escenarios_Predefinidos;
                List<EscenarioDTO> escenariosPorDefectoOrdenados = serie.Lista_Escenarios_Predefinidos.OrderBy(e => Array.IndexOf(orden, e.Nombre)).ToList();
                         

                //Caudales medios diarios mensuales (m3/s)											
                //TODO: Consultar si son de verdad QMM o serian caudales ecologicos. Duda. De momento esta hecho con caudales ecologicos.

                int offsetFila = 14; //Fila 14
                int offsetColum = 5; //Columna E

                foreach (EscenarioDTO escenario in escenariosPorDefectoOrdenados)
                {
                    for (int i = 0; i < escenario.Caudales_Ecologicos.Length; i++)                    
                        objSheet.Cells[offsetFila, offsetColum+i].Value = escenario.Caudales_Ecologicos[i];
                    

                    objSheet.Cells[offsetFila, 18].Value = Math.Round(CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, escenario.Caudales_Ecologicos),3);
                    objSheet.Cells[offsetFila, 19].Value = escenario.Demanda_Ambiental;

                    offsetFila++;
                }

                //Añadir Regimen Normativo y escenarios de usuario si tiene.

                ////Regimen Normativo.
                //offsetFila = 25;
                //offsetColum = 5;


                //Escenarios de usuario.
                offsetFila = 25;
               
                
                offsetColum = 5;
                if (escenarios_Usuario.Count > 0)
                {
                    //Nos aseguramos que R_NORM sea el primero.
                    int normindex = 0;
                    for (int i = 0; i < escenarios_Usuario.Count; i++)
                    {
                        if (i == 0 && escenarios_Usuario[i].Nombre == "R_NORM") break;
                        else if (escenarios_Usuario[i].Nombre == "R_NORM") { normindex = i; break; }
                    }
                    if (normindex!=0)
                    {
                        EscenarioDTO norm = escenarios_Usuario[normindex];

                        for (int i = normindex; i > 0; i--)
                        {

                            escenarios_Usuario[i] = escenarios_Usuario[i - 1];

                        }
                        escenarios_Usuario[0] = norm;
                    }

                    if (escenarios_Usuario[0].Nombre != "R_NORM") offsetFila = 26;
                    foreach (EscenarioDTO escenario in escenarios_Usuario)
                    {
                        bool escenarioLlenoFlag = false;
                        
                        for (int i = 0; i < escenario.Caudales_Ecologicos.Length; i++) 
                        {
                            if (escenario.Caudales_Ecologicos[i] != 0) { escenarioLlenoFlag = true;break; }
                        }
                        if (!escenarioLlenoFlag) { 
                            for (int i = 0; i < escenario.Caudales_Ecologicos.Length; i++)   
                            objSheet.Cells[offsetFila, offsetColum + i].Value = escenario.Caudales_Ecologicos[i];
                        }
                        objSheet.Cells[offsetFila, 18].Value = Math.Round(CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, escenario.Caudales_Ecologicos), 3);
                        objSheet.Cells[offsetFila, 19].Value = escenario.Demanda_Ambiental;
                        offsetFila++;
                    }

                }

                //Regimen Natural      
                objSheet.Cells[45, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerQmmFromCaudales(serie.Caudales_R_NAT.ToArray()));
                objSheet.Cells[46, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 50));
                objSheet.Cells[47, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 35));
                objSheet.Cells[48, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 30));
                objSheet.Cells[49, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 25));
                objSheet.Cells[50, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 20));
                objSheet.Cells[51, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 15));
                objSheet.Cells[52, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 10));
                objSheet.Cells[53, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(serie.Caudales_R_NAT.ToArray(), 5));
                objSheet.Cells[54, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerCaudalMinimo(serie.Caudales_R_NAT.ToArray()));

                //Regimen Alterado RA_QMM
                if (serie.Caudales_R_ALT.Count!=0) 
                {
                    objSheet.Cells[59, 18].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, CalculoMetodosCEs.ObtenerQmmFromCaudales(serie.Caudales_R_ALT.ToArray()));

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
    }
}
