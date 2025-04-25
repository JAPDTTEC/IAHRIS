
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MathNet.Numerics.Statistics;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    
    public partial class Informe7_RCE : Informe_RCE
    {
        public Informe7_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
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
                

                EscenarioDTO RA_RNORM = new EscenarioDTO();
                EscenarioDTO RA_User1 = new EscenarioDTO();
                EscenarioDTO RA_User2 = new EscenarioDTO();


                List<double[]> CaudalesRA_RNORM = new List<double[]>();
                List<double[]> CaudalesRA_User1 = new List<double[]>();
                List<double[]> CaudalesRA_User2 = new List<double[]>();

                double[] CERA_RNORM = new double[12];
                double[] CERA_User1 = new double[12];
                double[] CERA_User2 = new double[12];

                if (R_NORM != null && R_NORM.Caudales_Ecologicos.All(valor => valor != 0)) { 
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

                    for (int x = 0; x < 12; x++)
                    {
                        List<double> monthvals = new List<double>();
                        int indiceMes = (x + 1 + 12 - serie.Mes_Inicio) % 12;

                        foreach (double[] d in CaudalesRA_RNORM.Where(b => b[1] == x + 1))
                        {
                            monthvals.Add(d[2]);
                        }
                        CERA_RNORM[indiceMes] = ArrayStatistics.Mean(monthvals.ToArray());
                        monthvals.Clear();
                        foreach (double[] d in CaudalesRA_User1.Where(b => b[1] == x + 1))
                        {
                            monthvals.Add(d[2]);
                        }
                        CERA_User1[indiceMes] = ArrayStatistics.Mean(monthvals.ToArray());
                        monthvals.Clear();
                        foreach (double[] d in CaudalesRA_User2.Where(b => b[1] == x + 1))
                        {
                            monthvals.Add(d[2]);
                        }
                        CERA_User2[indiceMes] = ArrayStatistics.Mean(monthvals.ToArray());

                    

                }


                Escenarios.Escenarios escenarioBD = new Escenarios.Escenarios();
                int offsetRow = 27;

                if (R_NORM != null && R_NORM.Caudales_Ecologicos.All(valor => valor != 0))
                {

                    RA_RNORM.Id_Punto_Ref = R_NORM.Id_Punto_Ref;
                    RA_RNORM.Id_Alteracion_Ref = R_NORM.Id_Alteracion_Ref;
                    RA_RNORM.Nombre = "RA+R_NORM";
                    RA_RNORM.Descripcion = "RA+R_NORM";
                    RA_RNORM.Por_Defecto = true;
                    RA_RNORM.Caudales_Ecologicos = CERA_RNORM;
                    RA_RNORM.Puntuacion = escenarioBD.CalcularPuntuacion(RA_RNORM, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray());
                    RA_RNORM.Eficiencia = Math.Round(escenarioBD.CalcularEficiencia(RA_RNORM, serie.Mes_Inicio), 3);
                    RA_RNORM.Demanda_Ambiental = Math.Round(escenarioBD.CalcularDemandaAmbiental(RA_RNORM, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray()), 3);

                    objSheet.Cells[offsetRow, 6].Value = RA_RNORM.Puntuacion.Puntuacion_Estacionalidad.Cumple;
                    objSheet.Cells[offsetRow, 7].Value = RA_RNORM.Puntuacion.Puntuacion_Estacionalidad.NoCumple;
                    objSheet.Cells[offsetRow, 8].Value = RA_RNORM.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 9].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.MenorQMin;
                    objSheet.Cells[offsetRow, 10].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.Qmin_P95;
                    objSheet.Cells[offsetRow, 11].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.P95_P85;
                    objSheet.Cells[offsetRow, 12].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.P85_P75;
                    objSheet.Cells[offsetRow, 13].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.P75_P65;
                    objSheet.Cells[offsetRow, 14].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.P65_P50;
                    objSheet.Cells[offsetRow, 15].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.MayorP10;
                    objSheet.Cells[offsetRow, 16].Value = RA_RNORM.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 17].Value = RA_RNORM.Puntuacion.Puntuacion_Variabilidad.MenorP95;
                    objSheet.Cells[offsetRow, 18].Value = RA_RNORM.Puntuacion.Puntuacion_Variabilidad.P95_P85;
                    objSheet.Cells[offsetRow, 19].Value = RA_RNORM.Puntuacion.Puntuacion_Variabilidad.P85_P75;
                    objSheet.Cells[offsetRow, 20].Value = RA_RNORM.Puntuacion.Puntuacion_Variabilidad.P75_P65;
                    objSheet.Cells[offsetRow, 21].Value = RA_RNORM.Puntuacion.Puntuacion_Variabilidad.P65_P50;
                    objSheet.Cells[offsetRow, 22].Value = RA_RNORM.Puntuacion.Puntuacion_Variabilidad.MayorP10;
                    objSheet.Cells[offsetRow, 23].Value = RA_RNORM.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial;

                    //Tabla Valoracion Final
                    objSheet.Cells[offsetRow, 28].Value = RA_RNORM.Puntuacion.Puntuacion_Total;
                    objSheet.Cells[offsetRow, 29].Value = RA_RNORM.Demanda_Ambiental;
                    objSheet.Cells[offsetRow, 30].Value = RA_RNORM.Eficiencia;
                }
                if (EscenariosUser.Count >= 1)
                {


                    RA_User1.Id_Punto_Ref = EscenariosUser[0].Id_Punto_Ref;
                    RA_User1.Id_Alteracion_Ref = EscenariosUser[0].Id_Alteracion_Ref;
                    RA_User1.Nombre = "RA+" + EscenariosUser[0].Nombre;
                    RA_User1.Descripcion = EscenariosUser[0].Descripcion;
                    RA_User1.Por_Defecto = true;
                    RA_User1.Caudales_Ecologicos = CERA_User1;
                    RA_User1.Puntuacion = escenarioBD.CalcularPuntuacion(RA_User1, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray());
                    RA_User1.Eficiencia = Math.Round(escenarioBD.CalcularEficiencia(RA_User1, serie.Mes_Inicio), 3);
                    RA_User1.Demanda_Ambiental = Math.Round(escenarioBD.CalcularDemandaAmbiental(RA_User1, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray()), 3);

                    objSheet.Cells["E44"].Value = RA_User1.Nombre;

                    offsetRow = 25;

                    objSheet.Cells[offsetRow, 6].Value = RA_User1.Puntuacion.Puntuacion_Estacionalidad.Cumple;
                    objSheet.Cells[offsetRow, 7].Value = RA_User1.Puntuacion.Puntuacion_Estacionalidad.NoCumple;
                    objSheet.Cells[offsetRow, 8].Value = RA_User1.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 9].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.MenorQMin;
                    objSheet.Cells[offsetRow, 10].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.Qmin_P95;
                    objSheet.Cells[offsetRow, 11].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.P95_P85;
                    objSheet.Cells[offsetRow, 12].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.P85_P75;
                    objSheet.Cells[offsetRow, 13].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.P75_P65;
                    objSheet.Cells[offsetRow, 14].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.P65_P50;
                    objSheet.Cells[offsetRow, 15].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.MayorP10;
                    objSheet.Cells[offsetRow, 16].Value = RA_User1.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 17].Value = RA_User1.Puntuacion.Puntuacion_Variabilidad.MenorP95;
                    objSheet.Cells[offsetRow, 18].Value = RA_User1.Puntuacion.Puntuacion_Variabilidad.P95_P85;
                    objSheet.Cells[offsetRow, 19].Value = RA_User1.Puntuacion.Puntuacion_Variabilidad.P85_P75;
                    objSheet.Cells[offsetRow, 20].Value = RA_User1.Puntuacion.Puntuacion_Variabilidad.P75_P65;
                    objSheet.Cells[offsetRow, 21].Value = RA_User1.Puntuacion.Puntuacion_Variabilidad.P65_P50;
                    objSheet.Cells[offsetRow, 22].Value = RA_User1.Puntuacion.Puntuacion_Variabilidad.MayorP10;
                    objSheet.Cells[offsetRow, 23].Value = RA_User1.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial;

                    //Tabla Valoracion Final
                    objSheet.Cells[offsetRow, 28].Value = RA_User1.Puntuacion.Puntuacion_Total;
                    objSheet.Cells[offsetRow, 29].Value = RA_User1.Demanda_Ambiental;
                    objSheet.Cells[offsetRow, 30].Value = RA_User1.Eficiencia;
                }
                if (EscenariosUser.Count == 2)
                {
                    RA_User2.Id_Punto_Ref = EscenariosUser[1].Id_Punto_Ref;
                    RA_User2.Id_Alteracion_Ref = EscenariosUser[1].Id_Alteracion_Ref;
                    RA_User2.Nombre = "RA+" + EscenariosUser[1].Nombre;
                    RA_User2.Descripcion = EscenariosUser[1].Descripcion;
                    RA_User2.Por_Defecto = true;
                    RA_User2.Caudales_Ecologicos = CERA_User2;
                    RA_User2.Puntuacion = escenarioBD.CalcularPuntuacion(RA_User2, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray());
                    RA_User2.Eficiencia = Math.Round(escenarioBD.CalcularEficiencia(RA_User2, serie.Mes_Inicio), 3);
                    RA_User2.Demanda_Ambiental = Math.Round(escenarioBD.CalcularDemandaAmbiental(RA_User2, serie.Mes_Inicio, serie.Caudales_R_NAT.ToArray()), 3);

                    objSheet.Cells["E45"].Value = RA_User2.Nombre;

                    offsetRow = 26;

                    objSheet.Cells[offsetRow, 6].Value = RA_User2.Puntuacion.Puntuacion_Estacionalidad.Cumple;
                    objSheet.Cells[offsetRow, 7].Value = RA_User2.Puntuacion.Puntuacion_Estacionalidad.NoCumple;
                    objSheet.Cells[offsetRow, 8].Value = RA_User2.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 9].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.MenorQMin;
                    objSheet.Cells[offsetRow, 10].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.Qmin_P95;
                    objSheet.Cells[offsetRow, 11].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.P95_P85;
                    objSheet.Cells[offsetRow, 12].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.P85_P75;
                    objSheet.Cells[offsetRow, 13].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.P75_P65;
                    objSheet.Cells[offsetRow, 14].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.P65_P50;
                    objSheet.Cells[offsetRow, 15].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.MayorP10;
                    objSheet.Cells[offsetRow, 16].Value = RA_User2.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial;

                    objSheet.Cells[offsetRow, 17].Value = RA_User2.Puntuacion.Puntuacion_Variabilidad.MenorP95;
                    objSheet.Cells[offsetRow, 18].Value = RA_User2.Puntuacion.Puntuacion_Variabilidad.P95_P85;
                    objSheet.Cells[offsetRow, 19].Value = RA_User2.Puntuacion.Puntuacion_Variabilidad.P85_P75;
                    objSheet.Cells[offsetRow, 20].Value = RA_User2.Puntuacion.Puntuacion_Variabilidad.P75_P65;
                    objSheet.Cells[offsetRow, 21].Value = RA_User2.Puntuacion.Puntuacion_Variabilidad.P65_P50;
                    objSheet.Cells[offsetRow, 22].Value = RA_User2.Puntuacion.Puntuacion_Variabilidad.MayorP10;
                    objSheet.Cells[offsetRow, 23].Value = RA_User2.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial;

                    //Tabla Valoracion Final
                    objSheet.Cells[offsetRow, 28].Value = RA_User2.Puntuacion.Puntuacion_Total;
                    objSheet.Cells[offsetRow, 29].Value = RA_User2.Demanda_Ambiental;
                    objSheet.Cells[offsetRow, 30].Value = RA_User2.Eficiencia;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
