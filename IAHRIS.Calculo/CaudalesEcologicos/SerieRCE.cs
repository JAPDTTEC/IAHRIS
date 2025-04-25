using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using IAHRIS.Calculo.CaudalesEcologicos.Tipologias;
using System.Collections.Generic;
using System.Linq;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS.Calculo.CaudalesEcologicos
{
    public class SerieRCE
    {
        public List<double[]> Aportaciones_R_NAT { get; set; }
        public List<double[]> Caudales_R_NAT { get; set; }
        public List<double[]> Aportaciones_R_ALT { get; set; }
        public List<double[]> Caudales_R_ALT { get; set; }
        public List<EscenarioDTO> Lista_Escenarios { get; set; }
        public List<EscenarioDTO> Lista_Escenarios_Predefinidos { get; set; }
        public List<EscenarioDTO> Lista_Escenarios_Seleccionados { get; set; }
        public List<EscenarioDTO> Lista_Escenarios_Usuario { get; set; }
        public TipologiaCE TipologiaCE { get; set; }
        public int Mes_Inicio { get; set; }
        public string Nombre_Regimen_Nat { get; set; }  
        public string Nombre_Regimen_Alt { get; set; }  
        public string Abrev_Regimen_Nat { get; set; }  
        public string Abrev_Regimen_Alt { get; set; }
        public EstadoLista[] Listas_Serie { get; set; }


        public SerieRCE()
        {
            Aportaciones_R_NAT = new List<double[]>();
            Caudales_R_NAT = new List<double[]>();
            Aportaciones_R_ALT = new List<double[]>();
            Caudales_R_ALT = new List<double[]>();
            Lista_Escenarios = new List<EscenarioDTO>();
            Lista_Escenarios_Predefinidos = new List<EscenarioDTO>();
            Lista_Escenarios_Seleccionados = new List<EscenarioDTO>();
            Lista_Escenarios_Usuario = new List<EscenarioDTO>();

            TipologiaCE = new TipologiaCE();
        }

        public static SerieRCE ObtenerSerieRCEPorSimulacion (Simulacion simulacion, List<EscenarioDTO> escenariosPunto)
        {
            /* Simulacion Tipo Listas 
                 *  [0] Sacar datos validos en NAT DIARIOS
                 *  [1] Sacar datos validos en ALT DIARIOS
                 *  [2] Sacar datos validos en NAT MENSUAL
                 *  [3] Sacar datos validos en ALT MENSUAL
             * */

            SerieRCE serieRCE = new SerieRCE();

            serieRCE.Nombre_Regimen_Nat = simulacion.nombreRegimenPunto;
            serieRCE.Abrev_Regimen_Nat = simulacion.abreviaturaRegimenPunto;
            serieRCE.Nombre_Regimen_Alt = simulacion.nombreRegimenAlteracion;
            serieRCE.Abrev_Regimen_Alt = simulacion.abreviaturaRegimenAlteracion;

            serieRCE.Mes_Inicio = simulacion.mesInicio;
            serieRCE.Lista_Escenarios = escenariosPunto;
            serieRCE.Lista_Escenarios_Predefinidos = escenariosPunto.Where(x => x.Por_Defecto == true).ToList();
            serieRCE.Lista_Escenarios_Usuario = escenariosPunto.Where(x => x.Por_Defecto == false).ToList();
            serieRCE.Listas_Serie = simulacion.listas;


            //Listas
            EstadoLista lista = simulacion.listas[0];
            if (lista.nValidos > 0)
                serieRCE.Aportaciones_R_NAT.AddRange(
                                    Utiles.ObtenerAportacion(simulacion, lista, simulacion.mesInicio, simulacion.idListas[0],true));

            lista = simulacion.listas[2];
            if (lista.nValidos > 0)
                serieRCE.Aportaciones_R_NAT.AddRange(
                                    Utiles.ObtenerAportacion(simulacion, lista, simulacion.mesInicio, simulacion.idListas[2],true));

            lista = simulacion.listas[1];
            if (lista.nValidos > 0)
                serieRCE.Aportaciones_R_ALT.AddRange(
                                    Utiles.ObtenerAportacion(simulacion, lista, simulacion.mesInicio, simulacion.idListas[1],false));

            lista = simulacion.listas[3];
            if (lista.nValidos > 0)
                serieRCE.Aportaciones_R_ALT.AddRange(
                                    Utiles.ObtenerAportacion(simulacion, lista, simulacion.mesInicio, simulacion.idListas[3],false));


            //Obtener caudales
            if (serieRCE.Caudales_R_NAT.Count == 0)
            {
                foreach (double[] aportacion in serieRCE.Aportaciones_R_NAT)
                {
                    int año = (int)aportacion[0];
                    int mes = (int)aportacion[1];

                    double[] caudal = { aportacion[0],
                                        aportacion[1],
                                        CalculoMetodosCEs.ObtenerCaudalFromAportacion(aportacion[2], año, mes)
                                       };

                    serieRCE.Caudales_R_NAT.Add(caudal);
                }
            }


            if (serieRCE.Aportaciones_R_ALT.Count > 0)
            {
                foreach (double[] aportacion in serieRCE.Aportaciones_R_ALT)
                {
                    int año = (int)aportacion[0];
                    int mes = (int)aportacion[1];

                    double[] caudal = { aportacion[0],
                                        aportacion[1],
                                        CalculoMetodosCEs.ObtenerCaudalFromAportacion(aportacion[2], año, mes)
                                       };

                    serieRCE.Caudales_R_ALT.Add(caudal);
                }
            }



            //no hay escenarios seleccionados al inicializar, actualizar Tipologia al seleccionar Escenarios.
            serieRCE.TipologiaCE = TipologiaCE.GetTipologia(serieRCE.Aportaciones_R_NAT.ToArray(), simulacion, false, 0);


            return serieRCE;

        }
    }
}
