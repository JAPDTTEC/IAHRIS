using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    public class Informe5_RCE : Informe_RCE
    {
        public Informe5_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }



        public override void Escribir(ExcelPackage excel, SerieRCE serie)
        {
            try
            {
                ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);
                List<EscenarioDTO> escenariosSeleccionados = serie.Lista_Escenarios_Seleccionados;

                objSheet.Cells["E6"].Value = DateTime.Now.ToShortDateString();



                //if (serie.Lista_Escenarios_Seleccionados.Count == 1)
                //    return;




                List<EscenarioDTO> escenariosSeleccionadosUsuario = serie.Lista_Escenarios_Seleccionados.FindAll(escenario => escenario.Nombre != "R_NORM");

                int offsetFila = 14; //Fila 14
                int offsetColum = 3; //Columna C
                foreach (EscenarioDTO escenario in escenariosSeleccionadosUsuario)
                {
                    objSheet.Cells[offsetFila, offsetColum].Value = escenario.Nombre;
                    for (int i = 2; i < escenario.Caudales_Ecologicos.Length + 2; i++)
                    {
                        objSheet.Cells[offsetFila, offsetColum + i].Value = escenario.Caudales_Ecologicos[i - 2];
                    }
                    offsetFila++;
                }




                // Aportaciones Anuales

                offsetFila = 14; //Fila 14
                offsetColum = 17; //Columna Q

                foreach (EscenarioDTO escenario in escenariosSeleccionadosUsuario)
                {
                    objSheet.Cells[offsetFila, offsetColum].Value = CalculoMetodosCEs.AportacionAnualFromCaudalEco(serie.Mes_Inicio, escenario.Caudales_Ecologicos);
                    offsetFila++;
                }


                //Descripcion de los escenarios
                offsetFila = 33; //Fila 33
                offsetColum = 3; //Columna C

                foreach (EscenarioDTO escenario in escenariosSeleccionadosUsuario)
                {
                    objSheet.Cells[offsetFila, offsetColum].Value = escenario.Descripcion;
                    offsetFila++;
                }

                //Tabla Valoracion Final

                offsetFila = 25; //Fila 25
                offsetColum = 5;


                foreach (var escenario in escenariosSeleccionadosUsuario)
                {
                    //Estacionalidad
                    objSheet.Cells[offsetFila, 5].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.Cumple;
                    objSheet.Cells[offsetFila, 6].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.NoCumple;
                    objSheet.Cells[offsetFila, 7].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial;

                    //Magnitud
                    objSheet.Cells[offsetFila, 8].Value = escenario.Puntuacion.Puntuacion_Magnitud.MenorQMin;
                    objSheet.Cells[offsetFila, 9].Value = escenario.Puntuacion.Puntuacion_Magnitud.Qmin_P95;
                    objSheet.Cells[offsetFila, 10].Value = escenario.Puntuacion.Puntuacion_Magnitud.P95_P85;
                    objSheet.Cells[offsetFila, 11].Value = escenario.Puntuacion.Puntuacion_Magnitud.P85_P75;
                    objSheet.Cells[offsetFila, 12].Value = escenario.Puntuacion.Puntuacion_Magnitud.P75_P65;
                    objSheet.Cells[offsetFila, 13].Value = escenario.Puntuacion.Puntuacion_Magnitud.P65_P50;
                    objSheet.Cells[offsetFila, 14].Value = escenario.Puntuacion.Puntuacion_Magnitud.MayorP10;
                    objSheet.Cells[offsetFila, 15].Value = escenario.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial;

                    //Variabilidad
                    objSheet.Cells[offsetFila, 16].Value = escenario.Puntuacion.Puntuacion_Variabilidad.MenorP95;
                    objSheet.Cells[offsetFila, 17].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P95_P85;
                    objSheet.Cells[offsetFila, 18].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P85_P75;
                    objSheet.Cells[offsetFila, 19].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P75_P65;
                    objSheet.Cells[offsetFila, 20].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P65_P50;
                    objSheet.Cells[offsetFila, 21].Value = escenario.Puntuacion.Puntuacion_Variabilidad.MayorP10;
                    objSheet.Cells[offsetFila, 22].Value = escenario.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial;


                    //Valoracion Final
                    objSheet.Cells[offsetFila, 24].Value = escenario.Nombre;
                    objSheet.Cells[offsetFila, 25].Value = escenario.Puntuacion.Puntuacion_Total;
                    objSheet.Cells[offsetFila, 26].Value = escenario.Demanda_Ambiental;
                    objSheet.Cells[offsetFila, 27].Value = escenario.Eficiencia;


                    offsetFila++;
                }
            }catch(Exception ex) 
            { 
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

         }
   
    }
}
