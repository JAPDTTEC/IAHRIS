
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using MathNet.Numerics.Statistics;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    public class Informe4_RCE : Informe_RCE
    {
        public Informe4_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }
      
        public override void Escribir(ExcelPackage excel, SerieRCE serie)
        {

            try
            {
                ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);
                List<EscenarioDTO> escenarios = serie.Lista_Escenarios;
                List<EscenarioDTO> escenariosPredefinidos = serie.Lista_Escenarios_Predefinidos;
                List<EscenarioDTO> escenariosUsuario = serie.Lista_Escenarios_Usuario;
                Escenarios.Escenarios escenarioBD = new Escenarios.Escenarios();


                objSheet.Cells["E6"].Value = DateTime.Now.ToShortDateString();

                string[] orden = {
                "TENNANT", "TESSMAN", "VMF", "NMTM_P90", "NMTM_FAIR",
                "NMTM_GOOD", "NMTM_EXCELLENT", "ADMM","IAHRIS_P90",
                "IAHRIS_P85", "IAHRIS_P80", "R_NORM"
            };
                try
                {
                    escenariosPredefinidos.Add(serie.Lista_Escenarios.First(x => x.Nombre == "R_NORM"));
                }
                catch (Exception e) { }
                List<EscenarioDTO> escenariosPredefinidosOrdenados = escenariosPredefinidos.OrderBy(e => Array.IndexOf(orden, e.Nombre)).ToList();


                //Puntuaciones Metodos.
                int offsetRow = 15;
                foreach (EscenarioDTO escenario in escenariosPredefinidosOrdenados)
                {
                    
                    if (escenario.Nombre != "R_NORM")
                    {
                        objSheet.Cells[offsetRow, 2].Value = escenario.Nombre;
                    }
                    else
                    {
                        objSheet.Cells[offsetRow, 3].Value = escenario.Nombre;
                    }


                    objSheet.Cells[offsetRow, 5].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.Cumple;
                    objSheet.Cells[offsetRow, 6].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.NoCumple;
                    objSheet.Cells[offsetRow, 7].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 8].Value = escenario.Puntuacion.Puntuacion_Magnitud.MenorQMin;
                    objSheet.Cells[offsetRow, 9].Value = escenario.Puntuacion.Puntuacion_Magnitud.Qmin_P95;
                    objSheet.Cells[offsetRow, 10].Value = escenario.Puntuacion.Puntuacion_Magnitud.P95_P85;
                    objSheet.Cells[offsetRow, 11].Value = escenario.Puntuacion.Puntuacion_Magnitud.P85_P75;
                    objSheet.Cells[offsetRow, 12].Value = escenario.Puntuacion.Puntuacion_Magnitud.P75_P65;
                    objSheet.Cells[offsetRow, 13].Value = escenario.Puntuacion.Puntuacion_Magnitud.P65_P50;
                    objSheet.Cells[offsetRow, 14].Value = escenario.Puntuacion.Puntuacion_Magnitud.MayorP10;
                    objSheet.Cells[offsetRow, 15].Value = escenario.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 16].Value = escenario.Puntuacion.Puntuacion_Variabilidad.MenorP95;
                    objSheet.Cells[offsetRow, 17].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P95_P85;
                    objSheet.Cells[offsetRow, 18].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P85_P75;
                    objSheet.Cells[offsetRow, 19].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P75_P65;
                    objSheet.Cells[offsetRow, 20].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P65_P50;
                    objSheet.Cells[offsetRow, 21].Value = escenario.Puntuacion.Puntuacion_Variabilidad.MayorP10;
                    objSheet.Cells[offsetRow, 22].Value = escenario.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial;

                    //Tabla Valoracion Final
                    objSheet.Cells[offsetRow, 24].Value = escenario.Puntuacion.Puntuacion_Total;
                    objSheet.Cells[offsetRow, 25].Value = escenario.Demanda_Ambiental;
                    objSheet.Cells[offsetRow, 26].Value = escenario.Eficiencia;

                    offsetRow++;
                }

                offsetRow = 27;



                foreach (EscenarioDTO escenario in escenariosUsuario.FindAll(escenario => escenario.Nombre != "R_NORM"))
                {

                    objSheet.Cells[offsetRow, 3].Value = escenario.Nombre;

                    objSheet.Cells[offsetRow, 5].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.Cumple;
                    objSheet.Cells[offsetRow, 6].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.NoCumple;
                    objSheet.Cells[offsetRow, 7].Value = escenario.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 8].Value = escenario.Puntuacion.Puntuacion_Magnitud.MenorQMin;
                    objSheet.Cells[offsetRow, 9].Value = escenario.Puntuacion.Puntuacion_Magnitud.Qmin_P95;
                    objSheet.Cells[offsetRow, 10].Value = escenario.Puntuacion.Puntuacion_Magnitud.P95_P85;
                    objSheet.Cells[offsetRow, 11].Value = escenario.Puntuacion.Puntuacion_Magnitud.P85_P75;
                    objSheet.Cells[offsetRow, 12].Value = escenario.Puntuacion.Puntuacion_Magnitud.P75_P65;
                    objSheet.Cells[offsetRow, 13].Value = escenario.Puntuacion.Puntuacion_Magnitud.P65_P50;
                    objSheet.Cells[offsetRow, 14].Value = escenario.Puntuacion.Puntuacion_Magnitud.MayorP10;
                    objSheet.Cells[offsetRow, 15].Value = escenario.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 16].Value = escenario.Puntuacion.Puntuacion_Variabilidad.MenorP95;
                    objSheet.Cells[offsetRow, 17].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P95_P85;
                    objSheet.Cells[offsetRow, 18].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P85_P75;
                    objSheet.Cells[offsetRow, 19].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P75_P65;
                    objSheet.Cells[offsetRow, 20].Value = escenario.Puntuacion.Puntuacion_Variabilidad.P65_P50;
                    objSheet.Cells[offsetRow, 21].Value = escenario.Puntuacion.Puntuacion_Variabilidad.MayorP10;
                    objSheet.Cells[offsetRow, 22].Value = escenario.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial;

                    //Tabla Valoracion Final
                    objSheet.Cells[offsetRow, 24].Value = escenario.Puntuacion.Puntuacion_Total;
                    objSheet.Cells[offsetRow, 25].Value = escenario.Demanda_Ambiental;
                    objSheet.Cells[offsetRow, 26].Value = escenario.Eficiencia;

                    offsetRow++;
                }

                offsetRow = 54;
                int offsetColum = 18;

                List<EscenarioDTO> escenariosSelec=serie.Lista_Escenarios_Seleccionados.FindAll(escenario => escenario.Nombre != "R_NORM");
                //try
                //{
                //    if (serie.Lista_Escenarios_Seleccionados.First(x => x.Nombre == "R_NORM").Caudales_Ecologicos.All(valor => valor == 0))
                //    {
                //        escenariosSelec = serie.Lista_Escenarios_Seleccionados.FindAll(escenario => escenario.Nombre != "R_NORM");
                //    }
                //    else
                //    {
                //        escenariosSelec = serie.Lista_Escenarios_Seleccionados;
                //    }
                //}
                //catch { }
                foreach (EscenarioDTO escenario in escenariosSelec)
                {

                    objSheet.Cells[offsetRow, offsetColum].Value = escenario.Nombre;
                    for (int i = 0; i < escenario.Caudales_Ecologicos.Length; i++)
                    {
                        objSheet.Cells[offsetRow, offsetColum + i + 1].Value = escenario.Caudales_Ecologicos[i];
                    }
                    offsetRow++;
                }

                offsetRow = 37;
                offsetColum = 2;

                foreach (EscenarioDTO escenario in escenariosUsuario)
                {
                    if (escenario.Nombre != "R_NORM")
                    {
                        objSheet.Cells[offsetRow, offsetColum].Value = escenario.Nombre;
                        objSheet.Cells[offsetRow, offsetColum + 1].Value = escenario.Descripcion;
                        offsetRow++;
                    }
                }
                if(serie.Caudales_R_ALT.Count!=0)
                {
                    
                    EscenarioDTO escenarioRA_P50 = new EscenarioDTO()
                    {
                        Nombre = "RA_P50",
                        Descripcion = "Escenario generado considerando para cada mes la mediana de los caudales registrados en régimen alterado",
                        Id_Punto_Ref = escenariosPredefinidos[0].Id_Punto_Ref,
                        Id_Alteracion_Ref = escenariosPredefinidos[0].Id_Alteracion_Ref,
                        Por_Defecto = true,
                        Caudales_Ecologicos = RellenarRA_QMM(serie.Caudales_R_ALT.ToArray(), serie.Mes_Inicio),
                        
                    };
                    escenarioRA_P50.Puntuacion = escenarioBD.CalcularPuntuacion(escenarioRA_P50, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray());
                    escenarioRA_P50.Eficiencia = Math.Round(escenarioBD.CalcularEficiencia(escenarioRA_P50, serie.Mes_Inicio),3);
                    escenarioRA_P50.Demanda_Ambiental = Math.Round(escenarioBD.CalcularDemandaAmbiental(escenarioRA_P50, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray()),3);

                    offsetRow = 31;

                    objSheet.Cells[offsetRow, 5].Value = escenarioRA_P50.Puntuacion.Puntuacion_Estacionalidad.Cumple;
                    objSheet.Cells[offsetRow, 6].Value = escenarioRA_P50.Puntuacion.Puntuacion_Estacionalidad.NoCumple;
                    objSheet.Cells[offsetRow, 7].Value = escenarioRA_P50.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 8].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.MenorQMin;
                    objSheet.Cells[offsetRow, 9].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.Qmin_P95;
                    objSheet.Cells[offsetRow, 10].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.P95_P85;
                    objSheet.Cells[offsetRow, 11].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.P85_P75;
                    objSheet.Cells[offsetRow, 12].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.P75_P65;
                    objSheet.Cells[offsetRow, 13].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.P65_P50;
                    objSheet.Cells[offsetRow, 14].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.MayorP10;
                    objSheet.Cells[offsetRow, 15].Value = escenarioRA_P50.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 16].Value = escenarioRA_P50.Puntuacion.Puntuacion_Variabilidad.MenorP95;
                    objSheet.Cells[offsetRow, 17].Value = escenarioRA_P50.Puntuacion.Puntuacion_Variabilidad.P95_P85;
                    objSheet.Cells[offsetRow, 18].Value = escenarioRA_P50.Puntuacion.Puntuacion_Variabilidad.P85_P75;
                    objSheet.Cells[offsetRow, 19].Value = escenarioRA_P50.Puntuacion.Puntuacion_Variabilidad.P75_P65;
                    objSheet.Cells[offsetRow, 20].Value = escenarioRA_P50.Puntuacion.Puntuacion_Variabilidad.P65_P50;
                    objSheet.Cells[offsetRow, 21].Value = escenarioRA_P50.Puntuacion.Puntuacion_Variabilidad.MayorP10;
                    objSheet.Cells[offsetRow, 22].Value = escenarioRA_P50.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial;

                    //Tabla Valoracion Final
                    objSheet.Cells[offsetRow, 24].Value = escenarioRA_P50.Puntuacion.Puntuacion_Total;
                    objSheet.Cells[offsetRow, 25].Value = escenarioRA_P50.Demanda_Ambiental;
                    objSheet.Cells[offsetRow, 26].Value = escenarioRA_P50.Eficiencia;
                }
                

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //TODO 2: Basar la devolucióin del escenario en QMM en lugar de en P50

        /*
         *  //QMM
       
         */
        private double[] RellenarRA_QMM(double[][] datos, int mes_inicio)
        {
            double[] RA_QMM = new double[12];

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



            
            


            return RA_QMM;

        }
    }
}
