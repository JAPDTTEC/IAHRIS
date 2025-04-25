using IAHRIS.BBDD;
using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace IAHRIS.Calculo.CaudalesEcologicos.Escenarios
{
    public class Escenarios : IEscenario
    {
        public readonly EscenarioDTO _escenario;
        private OleDbDataBase _cMDB;            
        private readonly string _RutaBBDD = Application.StartupPath + @"\IAHRISv2.mdb";

        public Escenarios() 
        {
            _cMDB = new OleDbDataBase("Base", _RutaBBDD);     
        }

        public void CargarDatos(double[] datos)
        {
            _escenario.Caudales_Ecologicos = datos;   
        }


        public EscenarioDTO GetEscenario(int idEscenario)
        {
            try
            {

                _cMDB.AbrirConexion();
                string query = string.Format("SELECT TOP 1 [Id_Escenario], [Nombre_Escenario], [Id_Punto], [Id_Alteracion]," +
                                            " [Por_Defecto], [Mes0], [Mes1], [Mes2], [Mes3], [Mes4], [Mes5], [Mes6]," +
                                            " [Mes7], [Mes8], [Mes9], [Mes10], [Mes11], [Puntuacion], [Demanda_Ambiental], [Eficiencia]" +
                                            " FROM [Escenario] WHERE [Id_Escenario] = {0}", idEscenario);

                DataSet ds = _cMDB.RellenarDataSet("Esc", query);

                _cMDB.Desconectar();

                if (ds == null)
                    return null;


                return new EscenarioDTO
                {
                    Id_Escenario = (int)ds.Tables[0].Rows[0]["Id_Escenario"],
                    Nombre = ds.Tables[0].Rows[0]["Nombre_Escenario"].ToString(),
                    Id_Punto_Ref = (int)ds.Tables[0].Rows[0]["Id_Punto"],
                    Por_Defecto = (bool)ds.Tables[0].Rows[0]["Por_Defecto"],
                };

            }
            catch
            {
                if (_cMDB!= null)
                    _cMDB.Desconectar();

                return null;
            }
        }

        public bool GuardaEscenarioBD(EscenarioDTO escenario)
        {
            try
            {
                _cMDB.AbrirConexion();

                string caudales = string.Join(",", escenario.Caudales_Ecologicos.Select(x => x.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)));
                string query = string.Format("INSERT INTO [Escenario] (Nombre_Escenario, Descripcion_Escenario, Id_Punto, Id_Alteracion, Por_Defecto, Mes0,Mes1,Mes2,Mes3,Mes4,Mes5,Mes6,Mes7,Mes8,Mes9,Mes10,Mes11,Magnitud_MenorQmin,Magnitud_Qmin_P95,Magnitud_P95_P85,Magnitud_P85_P75,Magnitud_P75_P65,Magnitud_P65_P50,Magnitud_P50_P10,Magnitud_MayorP10,Magnitud_Puntuacion_Parcial,Estacionalidad_Cumple,Estacionalidad_NoCumple,Estacionalidad_Puntuacion_Parcial,Variabilidad_MenorP95,Variabilidad_P95_P85,Variabilidad_P85_P75,Variabilidad_P75_P65,Variabilidad_P65_P50,Variabilidad_P50_P10,Variabilidad_MayorP10,Variabilidad_Puntuacion_Parcial,Puntuacion, Demanda_Ambiental, Eficiencia ) Select '{0}','{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}",
                                                    escenario.Nombre, 
                                                    escenario.Descripcion , 
                                                    escenario.Id_Punto_Ref,
                                                    escenario.Id_Alteracion_Ref, 
                                                    escenario.Por_Defecto, 
                                                    caudales,

                                                    escenario.Puntuacion.Puntuacion_Magnitud.MenorQMin,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.Qmin_P95,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.P95_P85,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.P85_P75,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.P75_P65,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.P65_P50,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.P50_P10,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.MayorP10,
                                                    escenario.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial,
                                                    escenario.Puntuacion.Puntuacion_Estacionalidad.Cumple,
                                                    escenario.Puntuacion.Puntuacion_Estacionalidad.NoCumple,
                                                    escenario.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.MenorP95,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.P95_P85,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.P85_P75,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.P75_P65,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.P65_P50,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.P50_P10,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.MayorP10,
                                                    escenario.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial,

                                                    escenario.Puntuacion.Puntuacion_Total.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                                                    escenario.Demanda_Ambiental.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                                                    escenario.Eficiencia.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));

                int filas = _cMDB.EjecutarSQL(query);

                _cMDB.Desconectar();

                return filas > 0;
            }
            catch(Exception) 
            {
                if (_cMDB != null)
                    _cMDB.Desconectar();

                return false;
            }
        }
        public bool EditarEscenarioBD(EscenarioDTO escenario)
        {
            try
            {
                _cMDB.AbrirConexion();

                string update = string.Format("UPDATE [Escenario] SET Nombre_Escenario = '{38}',Descripcion_Escenario = '{39}',Mes0 = {0},Mes1 = {1},Mes2 = {2},Mes3 = {3},Mes4 = {4},Mes5 = {5},Mes6 = {6},Mes7 = {7}," +
                    "Mes8 = {8},Mes9 = {9},Mes10 = {10},Mes11 = {11},Magnitud_MenorQmin = {12},Magnitud_Qmin_P95 = {13},Magnitud_P95_P85 = {14},Magnitud_P85_P75 = {15},Magnitud_P75_P65 = {16},Magnitud_P65_P50 = {17},Magnitud_P50_P10 = {18},Magnitud_MayorP10 = {19},Magnitud_Puntuacion_Parcial = {20},Estacionalidad_Cumple = {21},Estacionalidad_NoCumple = {22},Estacionalidad_Puntuacion_Parcial = {23},Variabilidad_MenorP95 = {24},Variabilidad_P95_P85 = {25},Variabilidad_P85_P75 = {26},Variabilidad_P75_P65 = {27},Variabilidad_P65_P50 = {28},Variabilidad_P50_P10 = {29},Variabilidad_MayorP10 = {30},Variabilidad_Puntuacion_Parcial = {31}," +
                    "Puntuacion = {32}, Demanda_Ambiental = {33}, Eficiencia = {34} " +
                    "WHERE Id_Punto = {35} AND Id_Alteracion = {36} AND Id_Escenario = {37}", 
                    escenario.Caudales_Ecologicos[0].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Caudales_Ecologicos[1].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Caudales_Ecologicos[2].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),
                    escenario.Caudales_Ecologicos[3].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Caudales_Ecologicos[4].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Caudales_Ecologicos[5].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Caudales_Ecologicos[6].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),
                    escenario.Caudales_Ecologicos[7].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Caudales_Ecologicos[8].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),
                    escenario.Caudales_Ecologicos[9].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),
                    escenario.Caudales_Ecologicos[10].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),
                    escenario.Caudales_Ecologicos[11].ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),

                    escenario.Puntuacion.Puntuacion_Magnitud.MenorQMin,
                    escenario.Puntuacion.Puntuacion_Magnitud.Qmin_P95,
                    escenario.Puntuacion.Puntuacion_Magnitud.P95_P85,
                    escenario.Puntuacion.Puntuacion_Magnitud.P85_P75,
                    escenario.Puntuacion.Puntuacion_Magnitud.P75_P65,
                    escenario.Puntuacion.Puntuacion_Magnitud.P65_P50,
                    escenario.Puntuacion.Puntuacion_Magnitud.P50_P10,
                    escenario.Puntuacion.Puntuacion_Magnitud.MayorP10,
                    escenario.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial,
                    escenario.Puntuacion.Puntuacion_Estacionalidad.Cumple,
                    escenario.Puntuacion.Puntuacion_Estacionalidad.NoCumple,
                    escenario.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial,
                    escenario.Puntuacion.Puntuacion_Variabilidad.MenorP95,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P95_P85,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P85_P75,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P75_P65,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P65_P50,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P50_P10,
                    escenario.Puntuacion.Puntuacion_Variabilidad.MayorP10,
                    escenario.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial,
                    escenario.Puntuacion.Puntuacion_Total.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Demanda_Ambiental.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), 
                    escenario.Eficiencia.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),
                    
                    escenario.Id_Punto_Ref, 
                    escenario.Id_Alteracion_Ref,
                    escenario.Id_Escenario,

                    escenario.Nombre, 
                    escenario.Descripcion
                    );


                int filas = _cMDB.EjecutarSQL(update);

                _cMDB.Desconectar();

                return filas > 0;
            }
            catch (Exception)
            {
                if (_cMDB != null)
                    _cMDB.Desconectar();

                return false;
            }
        }
        public bool EditarRNORM_BD(EscenarioDTO escenario)
        {
            try
            {
                _cMDB.AbrirConexion();

                string update = string.Format("UPDATE [Escenario] SET Mes0 = {0},Mes1 = {1},Mes2 = {2},Mes3 = {3},Mes4 = {4},Mes5 = {5},Mes6 = {6},Mes7 = {7}," +
                    "Mes8 = {8},Mes9 = {9},Mes10 = {10},Mes11 = {11},Magnitud_MenorQmin = {12},Magnitud_Qmin_P95 = {13},Magnitud_P95_P85 = {14},Magnitud_P85_P75 = {15},Magnitud_P75_P65 = {16},Magnitud_P65_P50 = {17},Magnitud_P50_P10 = {18},Magnitud_MayorP10 = {19},Magnitud_Puntuacion_Parcial = {20},Estacionalidad_Cumple = {21},Estacionalidad_NoCumple = {22},Estacionalidad_Puntuacion_Parcial = {23},Variabilidad_MenorP95 = {24},Variabilidad_P95_P85 = {25},Variabilidad_P85_P75 = {26},Variabilidad_P75_P65 = {27},Variabilidad_P65_P50 = {28},Variabilidad_P50_P10 = {29},Variabilidad_MayorP10 = {30},Variabilidad_Puntuacion_Parcial = {31}," + 
                    "Puntuacion = {32}, Demanda_Ambiental = {33}, Eficiencia = {34} " +
                    "WHERE Nombre_Escenario = 'R_NORM' AND Id_Punto = {35} AND Id_Alteracion = {36}", 
                    escenario.Caudales_Ecologicos[0].ToString().Replace(',','.'), 
                    escenario.Caudales_Ecologicos[1].ToString().Replace(',', '.'), 
                    escenario.Caudales_Ecologicos[2].ToString().Replace(',', '.'),
                    escenario.Caudales_Ecologicos[3].ToString().Replace(',', '.'), 
                    escenario.Caudales_Ecologicos[4].ToString().Replace(',', '.'), 
                    escenario.Caudales_Ecologicos[5].ToString().Replace(',', '.'), 
                    escenario.Caudales_Ecologicos[6].ToString().Replace(',', '.'),
                    escenario.Caudales_Ecologicos[7].ToString().Replace(',', '.'), 
                    escenario.Caudales_Ecologicos[8].ToString().Replace(',', '.'), 
                    escenario.Caudales_Ecologicos[9].ToString().Replace(',', '.'), 
                    escenario.Caudales_Ecologicos[10].ToString().Replace(',', '.'),
                    escenario.Caudales_Ecologicos[11].ToString().Replace(',', '.'),
                    escenario.Puntuacion.Puntuacion_Magnitud.MenorQMin,
                    escenario.Puntuacion.Puntuacion_Magnitud.Qmin_P95,
                    escenario.Puntuacion.Puntuacion_Magnitud.P95_P85,
                    escenario.Puntuacion.Puntuacion_Magnitud.P85_P75,
                    escenario.Puntuacion.Puntuacion_Magnitud.P75_P65,
                    escenario.Puntuacion.Puntuacion_Magnitud.P65_P50,
                    escenario.Puntuacion.Puntuacion_Magnitud.P50_P10,
                    escenario.Puntuacion.Puntuacion_Magnitud.MayorP10,
                    escenario.Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial,
                    escenario.Puntuacion.Puntuacion_Estacionalidad.Cumple,
                    escenario.Puntuacion.Puntuacion_Estacionalidad.NoCumple,
                    escenario.Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial,
                    escenario.Puntuacion.Puntuacion_Variabilidad.MenorP95,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P95_P85,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P85_P75,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P75_P65,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P65_P50,
                    escenario.Puntuacion.Puntuacion_Variabilidad.P50_P10,
                    escenario.Puntuacion.Puntuacion_Variabilidad.MayorP10,
                    escenario.Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial,
                    escenario.Puntuacion.Puntuacion_Total.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), escenario.Demanda_Ambiental.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture), escenario.Eficiencia.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture),
                    escenario.Id_Punto_Ref,escenario.Id_Alteracion_Ref);


                int filas = _cMDB.EjecutarSQL(update);

                _cMDB.Desconectar();

                return filas > 0;
            }
            catch (Exception)
            {
                if (_cMDB != null)
                    _cMDB.Desconectar();

                return false;
            }
        }

        public bool EliminarEscenarioBD(int idEscenario)
        {
            try
            {
                _cMDB.AbrirConexion();

                string delete = "DELETE FROM [Escenario] WHERE Id_Escenario = " + idEscenario;


                int filas = _cMDB.EjecutarSQL(delete);

                _cMDB.Desconectar();

                return filas > 0;
            }
            catch (Exception)
            {
                if (_cMDB != null)
                    _cMDB.Desconectar();

                return false;
            }
        }
        public List<EscenarioDTO> GetEscenarioPorIDPunto(int id_punto, int id_alter)
        {
            List<EscenarioDTO> escenariosPunto = new List<EscenarioDTO>();

            try
            {
                _cMDB.AbrirConexion();

                DataSet ds = _cMDB.RellenarDataSet("Esc", "SELECT DISTINCT * FROM [Escenario] WHERE Id_Punto=" + id_punto +" AND Id_Alteracion = " + id_alter);

                _cMDB.Desconectar();

                if (ds == null)
                    return null;

                int numFila = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    double[] datos = {
                        Convert.ToDouble(dr["Mes0"]),
                        Convert.ToDouble(dr["Mes1"]),
                        Convert.ToDouble(dr["Mes2"]),
                        Convert.ToDouble(dr["Mes3"]),
                        Convert.ToDouble(dr["Mes4"]),
                        Convert.ToDouble(dr["Mes5"]),
                        Convert.ToDouble(dr["Mes6"]),
                        Convert.ToDouble(dr["Mes7"]),
                        Convert.ToDouble(dr["Mes8"]),
                        Convert.ToDouble(dr["Mes9"]),
                        Convert.ToDouble(dr["Mes10"]),
                        Convert.ToDouble(dr["Mes11"])
                    };

                    PuntuacionDTO puntuacion = new PuntuacionDTO {
                        Puntuacion_Total = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Puntuacion"]),
                        Puntuacion_Estacionalidad = new PuntuacionEstacionalidadDTO
                        {
                            Cumple = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Estacionalidad_Cumple"]),
                            NoCumple = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Estacionalidad_NoCumple"]),
                            Puntuacion_Parcial = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Estacionalidad_Puntuacion_Parcial"])
                        },
                        Puntuacion_Variabilidad = new PuntuacionVariabilidadDTO
                        {
                            MenorP95 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_MenorP95"]),
                            P95_P85 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_P95_P85"]),
                            P85_P75 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_P85_P75"]),
                            P75_P65 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_P75_P65"]),
                            P65_P50 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_P65_P50"]),
                            P50_P10 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_P50_P10"]),
                            MayorP10 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_MayorP10"]),
                            Puntuacion_Parcial = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Variabilidad_Puntuacion_Parcial"])
                        },
                        Puntuacion_Magnitud = new PuntuacionMagnitudDTO
                        {
                            MenorQMin = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_MenorQmin"]),
                            Qmin_P95 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_Qmin_P95"]),
                            P95_P85 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_P95_P85"]),
                            P85_P75 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_P85_P75"]),
                            P75_P65 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_P75_P65"]),
                            P65_P50 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_P65_P50"]),
                            P50_P10 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_P50_P10"]),
                            MayorP10 = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_MayorP10"]),
                            Puntuacion_Parcial = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Magnitud_Puntuacion_Parcial"])
                        }
                    };


                    EscenarioDTO escenario = new EscenarioDTO()
                    {
                        Id_Escenario = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Id_Escenario"]),
                        Nombre = ds.Tables[0].Rows[numFila]["Nombre_Escenario"].ToString(),
                        Descripcion = ds.Tables[0].Rows[numFila]["Descripcion_Escenario"].ToString(),
                        Id_Punto_Ref = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Id_Punto"]),
                        Id_Alteracion_Ref = Convert.ToInt32(ds.Tables[0].Rows[numFila]["Id_Alteracion"]),
                        Por_Defecto = Convert.ToBoolean(ds.Tables[0].Rows[numFila]["Por_Defecto"]),
                        Caudales_Ecologicos = datos,
                        Puntuacion = puntuacion,
                        Demanda_Ambiental = Convert.ToDouble(ds.Tables[0].Rows[numFila]["Demanda_Ambiental"]),
                        Eficiencia = Convert.ToDouble(ds.Tables[0].Rows[numFila]["Eficiencia"])
                    };

                    escenariosPunto.Add(escenario);
                    numFila++;
                }

                return escenariosPunto;
            }
            catch (Exception ex)
            {
                if (_cMDB != null)
                    _cMDB.Desconectar();

                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public EscenarioDTO CrearEscenario (int idPunto, int idAlter,  double[][] caudales, int mesInicio, IMetodo metodoEscenario)
        {
            EscenarioDTO _escenario = new EscenarioDTO();

            try
            {
                _escenario.Id_Punto_Ref = idPunto;
                _escenario.Id_Alteracion_Ref = idAlter;
                _escenario.Nombre = metodoEscenario.Nombre;
                _escenario.Por_Defecto = true;
                _escenario.Caudales_Ecologicos = metodoEscenario.CalcularCaudalesEcologicos(caudales);
                _escenario.Puntuacion = CalcularPuntuacion(_escenario, mesInicio, caudales);
                _escenario.Eficiencia = CalcularEficiencia(_escenario, mesInicio);
                _escenario.Demanda_Ambiental = CalcularDemandaAmbiental(_escenario, mesInicio, caudales);
                return _escenario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PuntuacionDTO CalcularPuntuacion (EscenarioDTO escenario, int mesInicio,  double[][] caudales)
        {
            try
            {
                PuntuacionDTO puntuacion = new PuntuacionDTO();
                PuntuacionMagnitudDTO puntMagnitud = new PuntuacionMagnitudDTO();
                PuntuacionVariabilidadDTO puntVariabilidad = new PuntuacionVariabilidadDTO();
                PuntuacionEstacionalidadDTO puntEstacionalidad = new PuntuacionEstacionalidadDTO();

                //necesario para la obtención de la puntuacion por Variabilidad
                double[][] cavInteranual = CalculoMetodosCEs.ObtenerCoefVariabInteranual(caudales, mesInicio);

                int mes = mesInicio;
                int mesAnterior;
                if (mes==1)mesAnterior = 12;
                else mesAnterior = mesInicio - 1;

                for (int i = 0; i < 12; i++)
                {
                    if (mes > 12) mes = 1;
                    if (mesAnterior > 12) mesAnterior = 1;            

                    //Datos necesarios para obtener puntuaciones
                    double[] caudalesMes = caudales.Where(x => x[1] == mes).Select(y => y[2]).ToArray();
                    double[] cvInterMes = cavInteranual.Where(x => x[1] == mes).Select(y => y[2]).ToArray();
                    double rceMesActual = escenario.Caudales_Ecologicos[i];
                    double rceMesAnterior = i == 0 ? escenario.Caudales_Ecologicos[11] : escenario.Caudales_Ecologicos[i - 1];
                    double[] caudalesMesAnterior = caudales.Where(x => x[1] == mesAnterior).Select(y => y[2]).ToArray();

                    //Calculos Puntuaciones
                    puntMagnitud = ObtenerPuntuacionMagnitud(rceMesActual, caudalesMes, puntMagnitud);
                    puntVariabilidad = ObtenerPuntuacionVariabilidad(rceMesActual, escenario.Caudales_Ecologicos, cvInterMes, puntVariabilidad);
                    puntEstacionalidad = ObtenerPuntuacionEstacionalidad(caudalesMes, caudalesMesAnterior, rceMesActual, rceMesAnterior, puntEstacionalidad);

                    mes++;
                    mesAnterior++;
                }
                puntVariabilidad.Puntuacion_Parcial+=4;
                puntVariabilidad.MenorP95 -= 1;


                return new PuntuacionDTO()
                {
                    Puntuacion_Magnitud = puntMagnitud,
                    Puntuacion_Variabilidad = puntVariabilidad,
                    Puntuacion_Estacionalidad = puntEstacionalidad,
                    Puntuacion_Total=puntMagnitud.Puntuacion_Parcial + puntVariabilidad.Puntuacion_Parcial + puntEstacionalidad.Puntuacion_Parcial
                };
            }
            catch (Exception ex) 
            {
                throw ex;   
            }

        }


        private PuntuacionMagnitudDTO ObtenerPuntuacionMagnitud(double rce, double[] caudales, PuntuacionMagnitudDTO punMagnitud)
        {
            double QMin = caudales.Min();

            try
            {
                if (rce < QMin)
                {
                    punMagnitud.MenorQMin += 1;//1
                    punMagnitud.Puntuacion_Parcial += -4;
                }
                if (QMin <= rce && rce < caudales.QuantileCustom(.05, QuantileDefinition.Weibull))
                {
                    punMagnitud.Qmin_P95 += 1;//2
                    punMagnitud.Puntuacion_Parcial += -1;
                }
                if (caudales.QuantileCustom(.05, QuantileDefinition.Weibull) <= rce && rce < caudales.QuantileCustom(.15, QuantileDefinition.Weibull))
                {
                    punMagnitud.P95_P85 += 1;//3
                    punMagnitud.Puntuacion_Parcial += 1;
                }
                if (caudales.QuantileCustom(.15, QuantileDefinition.Weibull) <= rce && rce < caudales.QuantileCustom(.25, QuantileDefinition.Weibull))
                {
                    punMagnitud.P85_P75 += 1;//4
                    punMagnitud.Puntuacion_Parcial += 2;                    
                }
                if (caudales.QuantileCustom(.25, QuantileDefinition.Weibull) <= rce && rce < caudales.QuantileCustom(.35, QuantileDefinition.Weibull))
                {
                    punMagnitud.P75_P65 += 1; //5
                    punMagnitud.Puntuacion_Parcial += 3;
                }
                if (caudales.QuantileCustom(.35, QuantileDefinition.Weibull) <= rce && rce <= caudales.QuantileCustom(.5, QuantileDefinition.Weibull))
                {
                    punMagnitud.P65_P50 += 1; //6
                    punMagnitud.Puntuacion_Parcial += 4;
                }
                if (caudales.QuantileCustom(.5, QuantileDefinition.Weibull) <= rce && rce <= caudales.QuantileCustom(.9, QuantileDefinition.Weibull))
                {
                    punMagnitud.P50_P10 += 1; //7
                    punMagnitud.Puntuacion_Parcial += 0;
                }                
                if (rce >= caudales.QuantileCustom(.9, QuantileDefinition.Weibull))
                {
                    punMagnitud.MayorP10 += 1; //8
                    punMagnitud.Puntuacion_Parcial += -4;
                }

                return punMagnitud;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private PuntuacionVariabilidadDTO ObtenerPuntuacionVariabilidad(double caudalEco, double[] caudalesEcologicos, double[] cvInterMes, PuntuacionVariabilidadDTO puntVariabilidad)
        {
            double cvCauldaEco = caudalEco / caudalesEcologicos.Min();
            int categoria = 0 ;

            //= SI(B116 <= B$108; "1";                      B116: RCE: cvCaudalEco B108: P95CV
            //SI(B116 < B$107; "2";                         B107: P85RN
            //SI(B116 < B$106; "3";                         B106: P75RN
            //SI(B116 < B$105; "4";                         B105: P65RN
            //SI(B116 <= B$104; "5";                        B104: P50RN
            //SI(B116 < B$103; "6"; "7")					B103: P10RN
            try 
            {
                if (cvCauldaEco <= cvInterMes.QuantileCustom(.05, QuantileDefinition.Weibull))
                {
                    puntVariabilidad.MenorP95 += 1;
                    categoria = 1;
                }
                else
                {
                    if (cvCauldaEco < cvInterMes.QuantileCustom(.15, QuantileDefinition.Weibull))
                    {
                        puntVariabilidad.P95_P85 += 1;
                        categoria = 2;
                    }
                    else
                    {
                        if (cvCauldaEco < cvInterMes.QuantileCustom(.25, QuantileDefinition.Weibull))
                        {
                            puntVariabilidad.P85_P75 += 1;
                            categoria = 3;
                        }
                        else
                        {
                            if (cvCauldaEco < cvInterMes.QuantileCustom(.35, QuantileDefinition.Weibull))
                            {
                                puntVariabilidad.P75_P65 += 1;
                                categoria = 4;
                            }
                            else
                            {
                                if (cvCauldaEco <= cvInterMes.QuantileCustom(.5, QuantileDefinition.Weibull))
                                {
                                    puntVariabilidad.P65_P50 += 1;
                                    categoria = 5;
                                }
                                else
                                {
                                    if (cvCauldaEco < cvInterMes.QuantileCustom(.9, QuantileDefinition.Weibull))
                                    {
                                        puntVariabilidad.P50_P10 += 1;
                                        categoria = 6;
                                    }
                                    else
                                    {
                                        if (cvCauldaEco >= cvInterMes.QuantileCustom(.9, QuantileDefinition.Weibull))
                                        {
                                            puntVariabilidad.MayorP10 += 1;
                                            categoria = 7;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                switch (categoria)
                {
                    case 1: puntVariabilidad.Puntuacion_Parcial += -4; break;
                    case 2: puntVariabilidad.Puntuacion_Parcial += 1; break;
                    case 3: puntVariabilidad.Puntuacion_Parcial += 2; break;
                    case 4: puntVariabilidad.Puntuacion_Parcial += 3; break;
                    case 5: puntVariabilidad.Puntuacion_Parcial += 4; break;
                    case 6: puntVariabilidad.Puntuacion_Parcial += 0; break;
                    case 7: puntVariabilidad.Puntuacion_Parcial += -4; break;

                }
            
               
                
                
                
               

                return puntVariabilidad;
            }
            catch (Exception ex)
            { 
                throw ex;
            }

        }


        private PuntuacionEstacionalidadDTO ObtenerPuntuacionEstacionalidad(double[] caudalMes, double[] caudalMesAnterior, double caudalRCE, double caudalRCEmesAnterior, PuntuacionEstacionalidadDTO puntEstacionalidad)
        {
            try
            {
                double rnP50_mesActual = caudalMes.Median().Round(3);
                double rnP50_mesAnterior = caudalMesAnterior.Median().Round(3);

                int estacionalidadRN = 0;
                int estacionalidadRCE = 0;

                //Estacionalidad mes Anterior.
                if (rnP50_mesActual > rnP50_mesAnterior) estacionalidadRN = 1;
                if (rnP50_mesActual < rnP50_mesAnterior) estacionalidadRN = -1;

                //Estacionalidad RCE
                if (caudalRCE > caudalRCEmesAnterior) estacionalidadRCE = 1;
                if (caudalRCE < caudalRCEmesAnterior) estacionalidadRCE = -1;

                
                //Si los signos (=+-) son iguales, suma. Si son diferentes, resta
                if(estacionalidadRCE == 0 & estacionalidadRN==0)
                {
                    puntEstacionalidad.Cumple += 1;
                    puntEstacionalidad.Puntuacion_Parcial += 1;
                }
                else {
                    if (estacionalidadRCE<0 & estacionalidadRN < 0)
                    {
                        puntEstacionalidad.Cumple += 1;
                        puntEstacionalidad.Puntuacion_Parcial += 1;
                    }
                    else
                    {
                        if(estacionalidadRCE > 0 & estacionalidadRN > 0)
                        {
                            puntEstacionalidad.Cumple += 1;
                            puntEstacionalidad.Puntuacion_Parcial += 1;

                        }
                        else
                        {
                            puntEstacionalidad.NoCumple += 1;
                            puntEstacionalidad.Puntuacion_Parcial += -1;
                        }
                    }
                }

              
                return puntEstacionalidad;
            }
            catch(Exception ex) 
            { 
                throw ex;
            }

        }

        public double CalcularDemandaAmbiental(EscenarioDTO escenario, int mesInicio,  double[][] caudales) 
        {
            double[] qMM = new double[12];
            qMM = CalculoMetodosCEs.ObtenerQmmFromCaudales(caudales);
            



            int mes = mesInicio;
            int mesAnterior;
            if (mes == 1) mesAnterior = 12;
            else mesAnterior = mesInicio - 1;

            double meses30 = 0;
            double meses31 = 0;
            double mes28 = 0;

            double QMMmeses30 = 0;
            double QMMmeses31 = 0;
            double QMMmes28 = 0;

            for (int i = 0; i < 12; i++)
            {
                if (mes > 12) mes = 1;
                if (mesAnterior > 12) mesAnterior = 1;

                switch (mes)
                {
                    case 1:  // Enero
                    case 3:  // Marzo
                    case 5:  // Mayo
                    case 7:  // Julio
                    case 8:  // Agosto
                    case 10: // Octubre
                    case 12: // Diciembre
                        meses31 += escenario.Caudales_Ecologicos[i];
                        QMMmeses31 += qMM[i].Round(3);
                        break;
                    case 4:  // Abril
                    case 6:  // Junio
                    case 9:  // Septiembre
                    case 11: // Noviembre
                        meses30 += escenario.Caudales_Ecologicos[i];
                        QMMmeses30 += qMM[i].Round(3);
                        break;
                    case 2:  // Febrero
                        mes28 += escenario.Caudales_Ecologicos[i];
                        QMMmes28 += qMM[i].Round(3);
                        break;
                }




                mes++;
                mesAnterior++;
            }

            double ApAnualEsc = (((meses31 * 2678400) + (meses30 * 2592000) + (mes28 * 2419200))/ 1000000).Round(3);
            double ApAnualRN = ((QMMmeses31 * 2.6784) + (QMMmeses30 * 2.592) + (QMMmes28 * 2.4192)).Round(3);
            return (ApAnualEsc / ApAnualRN).Round(3);
        }
        public double CalcularEficiencia(EscenarioDTO escenario, int mesInicio)
        {

            int mes = mesInicio;
            int mesAnterior;
            if (mes == 1) mesAnterior = 12;
            else mesAnterior = mesInicio - 1;

            double meses30 = 0;
            double meses31 = 0;
            double mes28 = 0;

            for (int i = 0; i < 12; i++)
            {
                if (mes > 12) mes = 1;
                if (mesAnterior > 12) mesAnterior = 1;

                switch (mes)
                {
                    case 1:  // Enero
                    case 3:  // Marzo
                    case 5:  // Mayo
                    case 7:  // Julio
                    case 8:  // Agosto
                    case 10: // Octubre
                    case 12: // Diciembre
                        meses31 += escenario.Caudales_Ecologicos[i];
                        break;
                    case 4:  // Abril
                    case 6:  // Junio
                    case 9:  // Septiembre
                    case 11: // Noviembre
                        meses30 += escenario.Caudales_Ecologicos[i];
                        break;
                    case 2:  // Febrero
                        mes28 += escenario.Caudales_Ecologicos[i];
                        break;                    
                }

                

                
                mes++;
                mesAnterior++;
            }

            double ApAnual = (meses31 * 2.6784) + (meses30 * 2.592) + (mes28 * 2.4192);

            double eficiencia = (escenario.Puntuacion.Puntuacion_Total / ApAnual).Round(3);
;
            if(eficiencia.IsFinite()) 
            {
                return eficiencia;
            }
            else
            {
                return 0;
            }
            
        }
    }
}
