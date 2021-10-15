using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MathNet.Numerics.Statistics;


namespace IAHRIS.Calculo
{
    public class Calculations
    {
        private DatosCalculo _datos;
        private IAHRISDataSet _dataSet;

        private MultiLangXML.MultiIdiomasXML _traductor;

        public Calculations(DatosCalculo datos, MultiLangXML.MultiIdiomasXML traductor,  IAHRISDataSet dataset)
        {
            _datos = datos;
            _dataSet = dataset;
            _traductor = traductor;
        }

        public void CalcularCurvaMensualClasificadaCudales(bool alterada)
        {
            SerieCaudalMensualCurvaClasificado datos = new  SerieCaudalMensualCurvaClasificado();
            
            //Obtenemos datos de origen
            SerieDiaria datOrigen;

            if (!alterada)
            {
                datOrigen = _datos.SerieNatDiaria;
            }
            else
            {
                datOrigen = _datos.SerieAltDiaria;
            }

            //Introducimos datos en estructura

            for (int x = 0; x < datOrigen.caudalDiaria.Length; x++)
            {
                datos.AddData(datOrigen.dia[x], datOrigen.caudalDiaria[x]);
            }

            //Calculamos
            datos.Calcular();

            if (!alterada)
            {
                _dataSet.CurvaClasificadaMensualCaudalNatural= datos;
            }
            else
            {
                _dataSet.CurvaClasificadaMensualCaudalAlterada = datos;
            }
        }

        public void CalcularINTERAnual()
        {
            SerieMensual aux;
            int i, j;
            int años;
            // Dim desp As Integer

            if (_datos.SerieNatMensual.caudalMensual is null)
            {
                _dataSet._SerieNatMensualCalculada = CalcularSerieMENSUAL();
                aux = _dataSet._SerieNatMensualCalculada;
            }
            else
            {
                // Me._dataSet._SerieAltMensualCalculada = Me._datos.SerieNatMensual
                aux = _datos.SerieNatMensual;
            }

            CalcularAportacionMENSUAL();

            // años = (aux.caudalMensual.Length / 12)
            años = aux.nAños;
            _dataSet._AportacionNatAnual.aportacion = new float[años];
            _dataSet._AportacionNatAnual.año = new int[años];
            _dataSet._AportacionNatAnual.tipo = new TIPOAÑO[años];

            // Sacar la aportacion anual
            var loopTo = años - 1;
            for (i = 0; i <= loopTo; i++)
            {
                _dataSet._AportacionNatAnual.año[i] = aux.mes[i * 12].Year;
                for (j = 0; j <= 11; j++)
                    _dataSet._AportacionNatAnual.aportacion[i] = _dataSet._AportacionNatAnual.aportacion[i] + _dataSet._AportacionNatMen.aportacion[i * 12 + j];
            }

            _dataSet._AportacionNatAnualOrdAños.año = new int[_dataSet._AportacionNatAnual.año.Length];
            _dataSet._AportacionNatAnualOrdAños.aportacion = new float[_dataSet._AportacionNatAnual.aportacion.Length];
            _dataSet._AportacionNatAnualOrdAños.tipo = new TIPOAÑO[_dataSet._AportacionNatAnual.tipo.Length];

            // Salvaguarda de las aportaciones por años
            Array.Copy(_dataSet._AportacionNatAnual.aportacion, _dataSet._AportacionNatAnualOrdAños.aportacion, _dataSet._AportacionNatAnual.aportacion.Length);

            // Ordenar
            Array.Sort(_dataSet._AportacionNatAnual.aportacion, _dataSet._AportacionNatAnual.año);

            // Clasificar

            float pos25p;
            float pos75p;
            pos25p = (float)(0.25d * (años + 1));
            pos75p = (float)(0.75d * (años + 1));
            var p = new float[años];
            
            for (i = 0; i <= años - 1; i++)
            {
                p[i] = (float)(1d - (i + 1) / (double)(años + 1));
                if (p[i] <= 0.25d)
                {
                    _dataSet._AportacionNatAnual.tipo[i] = TIPOAÑO.HUMEDO;
                }
                else if (p[i] >= 0.75d)
                {
                    _dataSet._AportacionNatAnual.tipo[i] = TIPOAÑO.SECO;
                }
                else
                {
                    _dataSet._AportacionNatAnual.tipo[i] = TIPOAÑO.MEDIO;
                }
            }

            // Salvaguarda de las aportaciones ordenadas por años.
            Array.Copy(_dataSet._AportacionNatAnual.año, _dataSet._AportacionNatAnualOrdAños.año, _dataSet._AportacionNatAnual.año.Length);
            Array.Copy(_dataSet._AportacionNatAnual.tipo, _dataSet._AportacionNatAnualOrdAños.tipo, _dataSet._AportacionNatAnual.tipo.Length);

            Array.Sort(_dataSet._AportacionNatAnualOrdAños.año, _dataSet._AportacionNatAnualOrdAños.tipo); // Para ordenar por años

            // Interpolacion lineal
            //float v0, v1;
            //float p0, p1;
            //int x0, x1;
            //x0 = (int)Conversion.Int(pos25p - 1f);
            //x1 = x0 + 1;
            //v0 = _dataSet._AportacionNatAnual.aportacion[x0];
            //v1 = _dataSet._AportacionNatAnual.aportacion[x1];
            //p0 = p[x0];
            //p1 = p[x1];
            //float aportacion25p = (float)(v0 + (v1 - v0) * ((0.75d - p0) / (p1 - p0)));
            //x0 = (int)Conversion.Int(pos75p - 1f);
            //x1 = x0 + 1;
            //v0 = _dataSet._AportacionNatAnual.aportacion[x0];
            //v1 = _dataSet._AportacionNatAnual.aportacion[x1];
            //p0 = p[x0];
            //p1 = p[x1];
            //float aportacion75p = (float)(v0 + (v1 - v0) * ((0.25d - p0) / (p1 - p0)));
            //_dataSet._limHumNat = aportacion75p;
            //_dataSet._limSecNat = aportacion25p;
        }
        /// <summary>
        /// Series interanuales alterada
        /// </summary>
        /// <remarks>Informe 1b</remarks>
        public void CalcularINTERAnualAlterada()
        {
            SerieMensual aux;
            int i, j;
            int años;
            // Dim desp As Integer

            if (_datos.SerieAltMensual.caudalMensual is null)
            {
                _dataSet._SerieAltMensualCalculada = CalcularSerieMENSUALAlterada();     // Funcion dependiente
                aux = _dataSet._SerieAltMensualCalculada;
            }
            else
            {
                // Me._dataSet._SerieAltMensualCalculada = Me._datos.SerieNatMensual
                aux = _datos.SerieAltMensual;
            }

            CalcularAportacionMENSUALAlterada();     // Funcion dependiente
            años = aux.nAños;
            _dataSet._AportacionAltAnual.aportacion = new float[años];
            _dataSet._AportacionAltAnual.año = new int[años];
            _dataSet._AportacionAltAnual.tipo = new TIPOAÑO[años];

            // Sacar la aportacion anual
            var loopTo = años - 1;
            for (i = 0; i <= loopTo; i++)
            {
                _dataSet._AportacionAltAnual.año[i] = aux.mes[i * 12].Year;
                for (j = 0; j <= 11; j++)
                    _dataSet._AportacionAltAnual.aportacion[i] = _dataSet._AportacionAltAnual.aportacion[i] + _dataSet._AportacionAltMen.aportacion[i * 12 + j];
            }

            _dataSet._AportacionAltAnualOrdAños.año = new int[_dataSet._AportacionAltAnual.año.Length];
            _dataSet._AportacionAltAnualOrdAños.aportacion = new float[_dataSet._AportacionAltAnual.aportacion.Length];
            _dataSet._AportacionAltAnualOrdAños.tipo = new TIPOAÑO[_dataSet._AportacionAltAnual.tipo.Length];
            var loopTo1 = años - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                _dataSet._AportacionAltAnualOrdAños.año[i] = _dataSet._AportacionAltAnual.año[i];
                _dataSet._AportacionAltAnualOrdAños.aportacion[i] = _dataSet._AportacionAltAnual.aportacion[i];
                _dataSet._AportacionAltAnualOrdAños.tipo[i] = _dataSet._AportacionAltAnual.tipo[i];
            }
            // Salvaguarda de las aportaciones por años
            Array.Copy(_dataSet._AportacionAltAnual.aportacion, _dataSet._AportacionAltAnualOrdAños.aportacion, _dataSet._AportacionAltAnual.aportacion.Length);

            // Ordenar
            Array.Sort(_dataSet._AportacionAltAnual.aportacion, _dataSet._AportacionAltAnual.año);

            var p = new float[años];
            loopTo1 = años - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                p[i] = (float)(1d - (i + 1) / (double)(años + 1));
                if (p[i] <= 0.25d)
                {
                    _dataSet._AportacionAltAnual.tipo[i] = TIPOAÑO.HUMEDO;
                }
                else if (p[i] >= 0.75d)
                {
                    _dataSet._AportacionAltAnual.tipo[i] = TIPOAÑO.SECO;
                }
                else
                {
                    _dataSet._AportacionAltAnual.tipo[i] = TIPOAÑO.MEDIO;
                }
            }
            Array.Copy(_dataSet._AportacionAltAnual.año, _dataSet._AportacionAltAnualOrdAños.año, _dataSet._AportacionAltAnual.año.Length);
            Array.Copy(_dataSet._AportacionAltAnual.tipo, _dataSet._AportacionAltAnualOrdAños.tipo, _dataSet._AportacionAltAnual.tipo.Length);
            Array.Sort(_dataSet._AportacionAltAnualOrdAños.año, _dataSet._AportacionAltAnualOrdAños.tipo); // Para ordenar por años
        }
        /// <summary>
        /// Se usa en el caso 3
        /// </summary>
        /// <remarks></remarks>
        public void CalcularINTERAnualAltGrafica()
        {
            SerieMensual aux;
            int i, j;
            int años;
            // Dim desp As Integer

            if (_datos.SerieAltMensual.caudalMensual is null)
            {
                _dataSet._SerieAltMensualCalculada = CalcularSerieMENSUALAlterada();     // Funcion dependiente
                aux = _dataSet._SerieAltMensualCalculada;
            }
            else
            {
                // Me._dataSet._SerieAltMensualCalculada = Me._datos.SerieNatMensual
                aux = _datos.SerieAltMensual;
            }

            CalcularAportacionMENSUALAlterada();     // Funcion dependiente
            años = (int)(aux.caudalMensual.Length / 12d);
            _dataSet._AportacionAltAnual.aportacion = new float[años];
            _dataSet._AportacionAltAnual.año = new int[años];
            _dataSet._AportacionAltAnual.tipo = new TIPOAÑO[años];

            // Sacar la aportacion anual
            var loopTo = años - 1;
            for (i = 0; i <= loopTo; i++)
            {
                _dataSet._AportacionAltAnual.año[i] = aux.mes[i * 12].Year;
                for (j = 0; j <= 11; j++)
                    _dataSet._AportacionAltAnual.aportacion[i] = _dataSet._AportacionAltAnual.aportacion[i] + _dataSet._AportacionAltMen.aportacion[i * 12 + j];
            }

            _dataSet._AportacionAltAnualOrdAños.año = new int[_dataSet._AportacionAltAnual.año.Length];
            _dataSet._AportacionAltAnualOrdAños.aportacion = new float[_dataSet._AportacionAltAnual.aportacion.Length];
            _dataSet._AportacionAltAnualOrdAños.tipo = new TIPOAÑO[_dataSet._AportacionAltAnual.tipo.Length];

            // Salvagurada de las aportaciones ordenadas por año
            Array.Copy(_dataSet._AportacionAltAnual.aportacion, _dataSet._AportacionAltAnualOrdAños.aportacion, _dataSet._AportacionAltAnual.aportacion.Length);

            // Ordenar
            Array.Sort(_dataSet._AportacionAltAnual.aportacion, _dataSet._AportacionAltAnual.año);

            // Clasificar

            float pos25p;
            float pos75p;
            pos25p = (float)(0.25d * (años + 1));
            pos75p = (float)(0.75d * (años + 1));
            var p = new float[años];
            var loopTo1 = años - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                p[i] = (float)(1d - (i + 1) / (double)(años + 1));
                if (p[i] <= 0.25d)
                {
                    _dataSet._AportacionAltAnual.tipo[i] = TIPOAÑO.HUMEDO;
                }
                else if (p[i] >= 0.75d)
                {
                    _dataSet._AportacionAltAnual.tipo[i] = TIPOAÑO.SECO;
                }
                else
                {
                    _dataSet._AportacionAltAnual.tipo[i] = TIPOAÑO.MEDIO;
                }
            }

            // Salvaguarda de las aportaciones ordenadas por años.
            Array.Copy(_dataSet._AportacionAltAnual.año, _dataSet._AportacionAltAnualOrdAños.año, _dataSet._AportacionAltAnual.año.Length);
            // Array.Copy(Me._dataSet._AportacionAltAnual.aportacion, Me._dataSet._AportacionAltAnualOrdAños.aportacion, Me._dataSet._AportacionAltAnual.aportacion.Length)
            Array.Copy(_dataSet._AportacionAltAnual.tipo, _dataSet._AportacionAltAnualOrdAños.tipo, _dataSet._AportacionAltAnual.tipo.Length);
            Array.Sort(_dataSet._AportacionAltAnualOrdAños.año, _dataSet._AportacionAltAnualOrdAños.tipo);
            _dataSet._graficaInterAlt = new GraficoAlterada[_dataSet._AportacionAltAnualOrdAños.año.Length + 1];
            var loopTo2 = _dataSet._AportacionAltAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                _dataSet._graficaInterAlt[i].año = _dataSet._AportacionAltAnualOrdAños.año[i];
                _dataSet._graficaInterAlt[i].tipo = _dataSet._AportacionAltAnualOrdAños.tipo[i];
                _dataSet._graficaInterAlt[i].apAlt = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                _dataSet._graficaInterAlt[i].apNat = _dataSet._AportacionNatAnualOrdAños.aportacion[i];
                _dataSet._graficaInterAlt[i].porcentaje = _dataSet._graficaInterAlt[i].apAlt / _dataSet._graficaInterAlt[i].apNat * 100f;
            }

            var loopTo3 = años - 1;
            for (i = 0; i <= loopTo3; i++)
                _dataSet._AportacionAltAnual.tipo[i] = _dataSet._AportacionNatAnualOrdAños.tipo[Array.BinarySearch(_dataSet._AportacionNatAnualOrdAños.año, _dataSet._AportacionAltAnual.año[i])];


            // Interpolacion lineal
            // Dim v0, v1 As Single
            // Dim p0, p1 As Single
            // Dim x0, x1 As Integer

            // x0 = Int(pos25p)
            // x1 = x0 + 1

            // v0 = Me._dataSet._AportacionAltAnual.aportacion(x0)
            // v1 = Me._dataSet._AportacionAltAnual.aportacion(x1)
            // p0 = p(x0 - 1)
            // p1 = p(x1 - 1)

            // Dim aportacion25p As Single = v0 + (v1 - v0) * ((0.75 - p0) / (p1 - p0))


            // x0 = Int(pos75p)
            // x1 = x0 + 1

            // v0 = Me._dataSet._AportacionAltAnual.aportacion(x0)
            // v1 = Me._dataSet._AportacionAltAnual.aportacion(x1)
            // p0 = p(x0 - 1)
            // p1 = p(x1 - 1)

            // Dim aportacion75p As Single = v0 + (v1 - v0) * ((0.25 - p0) / (p1 - p0))

        }


        public void CalcularINTRAnualPorMeses(bool alterada)
        {
            
            //Establecemos estructura de salida:

            List<SerieAportacionMensualClasificada> datos = new List<SerieAportacionMensualClasificada>(12);
            for(int x=0; x<12; x++)
            {
                datos.Add( new SerieAportacionMensualClasificada(x));
            }

            //Obtenemos datos de origen
            AportacionMensual datOrigen;

            if(!alterada)
            {
                datOrigen = _dataSet._AportacionNatMen;
            }
            else
            {
                datOrigen = _dataSet._AportacionAltMen;
            }

            //Introducimos datos en estructura

            for(int x=0; x< datOrigen.aportacion.Length;x++)
            {
                datos[datOrigen.mes[x].Month-1].DatosPorAños.Add(new DatoAportacionMensualClasificada(datOrigen.aportacion[x], datOrigen.mes[x].Year, datOrigen.mes[x].Month));
            }

            //Caracterizamos cada mes.
            foreach(SerieAportacionMensualClasificada d in datos)
            {
                d.CalculateCharacterizedMedians();
            }

            if (!alterada)
            {
                _dataSet.MensualCaracterizadaNatural = datos;
            }
            else
            {
                _dataSet.MensualCaracterizadaAlterada = datos;
            }



        }
            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            /* TODO ERROR: Skipped RegionDirectiveTrivia */        
            /// <summary>
            /// Series INTRAnual
            /// </summary>
            /// <param name="alterada">Si usamos los datos alterados</param>
            /// <remarks>Informe 2 y 3</remarks>
            public void CalcularINTRAnualPorAños(bool alterada)
        {
            int i, j;
            int longLista;

            // Valores divididos en su tipo
            List<Dictionary<DateTime, float>> valoresMenH = new List<Dictionary<DateTime, float>>();
            List<Dictionary<DateTime, float>> valoresMenM = new List<Dictionary<DateTime, float>>();
            List<Dictionary<DateTime, float>> valoresMenS = new List<Dictionary<DateTime, float>>();
            for (i = 0; i <= 11; i++)
            {
                valoresMenH.Add(new Dictionary<DateTime, float>());
                valoresMenM.Add(new Dictionary<DateTime, float>());
                valoresMenS.Add(new Dictionary<DateTime, float>());

            }

            // ------------------------------------------------------------------------------------------
            // -------- Meter los datos segun natural/alterada ------------------------------------------
            // ------------------------------------------------------------------------------------------
            // Meter los valores por tipo de año.
            if (!alterada)
            {
                longLista = _dataSet._AportacionNatAnual.año.Length;
                // Calcular las medianas de cada año.
                // Tengo que recorrer los 12 meses de cada año y meterlo en su sitio
                var loopTo = _dataSet._AportacionNatAnualOrdAños.año.Length - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    for (j = 0; j <= 11; j++)
                    {
                        if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                        {
                            valoresMenH[j].Add(_dataSet._AportacionNatMen.mes[i * 12 + j], _dataSet._AportacionNatMen.aportacion[i * 12 + j]);
                        }
                        else if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                        {
                            valoresMenM[j].Add(_dataSet._AportacionNatMen.mes[i * 12 + j], _dataSet._AportacionNatMen.aportacion[i * 12 + j]);
                        }
                        else
                        {
                            valoresMenS[j].Add(_dataSet._AportacionNatMen.mes[i * 12 + j], _dataSet._AportacionNatMen.aportacion[i * 12 + j]);
                        }
                    }
                }
            }
            else
            {
                // -------------------------------------------
                // Segun el caso 3:
                // SE USAN LOS TIPO CALCULADOS EN REG. NATURAL
                // -------------------------------------------
                longLista = _dataSet._AportacionAltAnual.año.Length;
                // Calcular las medianas de cada año.
                // Tengo que recorrer los 12 meses de cada año y meterlo en su sitio
                var loopTo1 = _dataSet._AportacionAltAnualOrdAños.año.Length - 1;
                for (i = 0; i <= loopTo1; i++)
                {
                    for (j = 0; j <= 11; j++)
                    {
                        if (_dataSet._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                        {
                            valoresMenH[j].Add(_dataSet._AportacionAltMen.mes[i * 12 + j], _dataSet._AportacionAltMen.aportacion[i * 12 + j]);
                        }
                        else if (_dataSet._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                        {
                            valoresMenM[j].Add(_dataSet._AportacionAltMen.mes[i * 12 + j], _dataSet._AportacionAltMen.aportacion[i * 12 + j]);
                        }
                        else
                        {
                            valoresMenS[j].Add(_dataSet._AportacionAltMen.mes[i * 12 + j], _dataSet._AportacionAltMen.aportacion[i * 12 + j]);
                        }
                    }
                }
            }


         

            
            // +++++++++++++++++++++++++++++++++++++
            // tabla de medianas y su inicializacion
            // +++++++++++++++++++++++++++++++++++++
            // 0 -> medianas de meses Humedos
            // 1 -> medianas de meses Medios
            // 2 -> medianas de meses Secos
            var tablamedianas = new float[12][];
            for (i = 0; i <= 11; i++)
                tablamedianas[i] = new float[3];

            //Calculamos la mediana para cada mes:

            for (i = 0; i <= 11; i++)
            {
                List<float> HCalc = new List<float>();
                foreach(KeyValuePair<DateTime,float> kvp in valoresMenH[i])
                {
                    HCalc.Add(kvp.Value);
                }
                List<float> MCalc = new List<float>();
                foreach (KeyValuePair<DateTime, float> kvp in valoresMenM[i])
                {
                    MCalc.Add(kvp.Value);
                }
                List<float> SCalc = new List<float>();
                foreach (KeyValuePair<DateTime, float> kvp in valoresMenS[i])
                {
                    SCalc.Add(kvp.Value);
                }
                //Húmedos
                tablamedianas[i][0] = HCalc.Median();
                tablamedianas[i][1] = MCalc.Median();
                tablamedianas[i][2] = SCalc.Median();

            }



           

                // Asignacion en las variables que necesito segun el calculo que realizo.
            if (!alterada)
            {
                _dataSet._IntraAnualNat = tablamedianas;
            }
            else
            {
                _dataSet._IntraAnualAlt = tablamedianas;
            }
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        /// <summary>
                                                               /// Parametro Habituales de regimen alterado
                                                               /// </summary>
                                                               /// <remarks>Informe 5</remarks>
        public void CalcularParametrosHabitualesAlterados()
        {
            int i, j;

            // +++++++++++++++++++++++++++++++++++++++
            // ++++++++++ Calculo de la Magnitud +++++
            // +++++++++++++++++++++++++++++++++++++++
            _dataSet._HabMagnitudAlt = new float[4]; // Redimension de las variables donde almacenaré los resultados
            int nAñoH, nAñoM, nAñoS;
            nAñoH = 0;
            nAñoM = 0;
            nAñoS = 0;

            // Hay que usar los tipos de los años Naturales
            var loopTo = _dataSet._AportacionAltAnual.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (_dataSet._AportacionAltAnual.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    _dataSet._HabMagnitudAlt[0] = _dataSet._HabMagnitudAlt[0] + _dataSet._AportacionAltAnual.aportacion[i];
                    nAñoH = nAñoH + 1;
                }
                else if (_dataSet._AportacionAltAnual.tipo[i] == TIPOAÑO.MEDIO)
                {
                    _dataSet._HabMagnitudAlt[1] = _dataSet._HabMagnitudAlt[1] + _dataSet._AportacionAltAnual.aportacion[i];
                    nAñoM = nAñoM + 1;
                }
                else
                {
                    _dataSet._HabMagnitudAlt[2] = _dataSet._HabMagnitudAlt[2] + _dataSet._AportacionAltAnual.aportacion[i];
                    nAñoS = nAñoS + 1;
                }
            }

            _dataSet._HabMagnitudAlt[3] = (_dataSet._HabMagnitudAlt[0] + _dataSet._HabMagnitudAlt[1] + _dataSet._HabMagnitudAlt[2]) / (nAñoH + nAñoM + nAñoS);
            _dataSet._HabMagnitudAlt[0] = _dataSet._HabMagnitudAlt[0] / nAñoH;
            _dataSet._HabMagnitudAlt[1] = _dataSet._HabMagnitudAlt[1] / nAñoM;
            _dataSet._HabMagnitudAlt[2] = _dataSet._HabMagnitudAlt[2] / nAñoS;

            // Me._dataSet._HabMagnitudAlt(0) = Me._dataSet._HabMagnitudAlt(0) / nAñoH
            // Me._dataSet._HabMagnitudAlt(1) = Me._dataSet._HabMagnitudAlt(1) / nAñoM
            // Me._dataSet._HabMagnitudAlt(2) = Me._dataSet._HabMagnitudAlt(2) / nAñoS

            // Me._dataSet._HabMagnitudAlt(3) = (Me._dataSet._HabMagnitudAlt(0) + Me._dataSet._HabMagnitudAlt(1) + Me._dataSet._HabMagnitudAlt(2)) / 3

            // +++++++++++++++++++++++++++++++++++++++++++
            // +++++++++ Calculo de la Variabilidad ++++++
            // +++++++++++++++++++++++++++++++++++++++++++
            _dataSet._HabVariabilidadAlt = new float[4];

            // diferencias
            float[] dif;
            dif = new float[_dataSet._AportacionAltAnualOrdAños.año.Length];
            float acH;
            float acM;
            float acS;
            acH = 0f;
            acM = 0f;
            acS = 0f;
            var loopTo1 = _dataSet._AportacionAltAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                float min, max;
                min = 99999f;
                max = -1;
                if (_dataSet._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    for (j = 0; j <= 11; j++)
                    {
                        float apor;
                        apor = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        if (apor >= max)
                        {
                            max = apor;
                        }

                        if (apor <= min)
                        {
                            min = apor;
                        }
                    }

                    dif[i] = max - min;
                    acH = acH + dif[i];
                }
                else if (_dataSet._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                {
                    for (j = 0; j <= 11; j++)
                    {
                        float apor;
                        apor = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        if (apor >= max)
                        {
                            max = apor;
                        }

                        if (apor <= min)
                        {
                            min = apor;
                        }
                    }

                    dif[i] = max - min;
                    acM = acM + dif[i];
                }
                else
                {
                    for (j = 0; j <= 11; j++)
                    {
                        float apor;
                        apor = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        if (apor >= max)
                        {
                            max = apor;
                        }

                        if (apor <= min)
                        {
                            min = apor;
                        }
                    }

                    dif[i] = max - min;
                    acS = acS + dif[i];
                }
            }

            _dataSet._HabVariabilidadAlt[3] = (acH + acM + acS) / (nAñoS + nAñoM + nAñoH);
            _dataSet._HabVariabilidadAlt[0] = acH / nAñoH;
            _dataSet._HabVariabilidadAlt[1] = acM / nAñoM;
            _dataSet._HabVariabilidadAlt[2] = acS / nAñoS;

            // Me._dataSet._HabVariabilidadAlt(0) = acH / 12
            // Me._dataSet._HabVariabilidadAlt(1) = acM / 12
            // Me._dataSet._HabVariabilidadAlt(2) = acS / 12
            // Me._dataSet._HabVariabilidadAlt(3) = (Me._dataSet._HabVariabilidadAlt(0) + Me._dataSet._HabVariabilidadAlt(1) + Me._dataSet._HabVariabilidadAlt(2)) / 3


            // +++++++++++++++++++++++++++++++++++++++++++
            // +++++++++ Calculo de la Estacionalidad ++++
            // +++++++++++++++++++++++++++++++++++++++++++
            _dataSet._HabEstacionalidadAlt = new string[3];
            float minH, maxH, minM, maxM, minS, maxS;
            STRING_MES_ORD mesMinH, mesMaxH, mesMinM, mesMaxM, mesMinS, mesMaxS;
            minH = 99999f;
            maxH = -1;
            minM = 99999f;
            maxM = -1;
            minS = 99999f;
            maxS = -1;
            mesMinH = (STRING_MES_ORD)99999;
            mesMaxH = (STRING_MES_ORD)(-1);
            mesMinM = (STRING_MES_ORD)99999;
            mesMaxM = (STRING_MES_ORD)(-1);
            mesMinS = (STRING_MES_ORD)99999;
            mesMaxS = (STRING_MES_ORD)(-1);
            var loopTo2 = _dataSet._IntraAnualAlt.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                if (maxH < _dataSet._IntraAnualAlt[i][0])
                {
                    maxH = _dataSet._IntraAnualAlt[i][0];
                    mesMaxH = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (minH > _dataSet._IntraAnualAlt[i][0])
                {
                    minH = _dataSet._IntraAnualAlt[i][0];
                    mesMinH = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (maxM < _dataSet._IntraAnualAlt[i][1])
                {
                    maxM = _dataSet._IntraAnualAlt[i][1];
                    mesMaxM = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (minM > _dataSet._IntraAnualAlt[i][1])
                {
                    minM = _dataSet._IntraAnualAlt[i][1];
                    mesMinM = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (maxS < _dataSet._IntraAnualAlt[i][2])
                {
                    maxS = _dataSet._IntraAnualAlt[i][2];
                    mesMaxS = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (minS > _dataSet._IntraAnualAlt[i][2])
                {
                    minS = _dataSet._IntraAnualAlt[i][2];
                    mesMinS = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }
            }

            // Me._dataSet._HabEstacionalidadAlt(0) = mesMaxH.ToString() + "-" + mesMinH.ToString()
            // Me._dataSet._HabEstacionalidadAlt(1) = mesMaxM.ToString() + "-" + mesMinM.ToString()
            // Me._dataSet._HabEstacionalidadAlt(2) = mesMaxS.ToString() + "-" + mesMinS.ToString()

            _dataSet._HabEstacionalidadAlt[0] = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMaxH + 1).ToString()).Substring(0, 3).ToUpper() + "-" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMinH + 1).ToString()).Substring(0, 3).ToUpper();

            _dataSet._HabEstacionalidadAlt[1] = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMaxM + 1).ToString()).Substring(0, 3).ToUpper() + "-" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMinM + 1).ToString()).Substring(0, 3).ToUpper();

            _dataSet._HabEstacionalidadAlt[2] = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMaxS + 1).ToString()).Substring(0, 3).ToUpper() + "-" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMinS + 1).ToString()).Substring(0, 3).ToUpper();

        }
        /// <summary>
        /// Parámetros Habituales
        /// </summary>
        /// <remarks>Informe 4a y 4</remarks>
        public void CalcularParametrosHabitualesCASO1()
        {
            int i, j;
            _dataSet._HabMagnitudNat = new float[4];
            int nAñoH, nAñoM, nAñoS;

            // +++++++++++++++++++++++++++++++++++++++
            // ++++++++++ Calculo de la Magnitud +++++
            // +++++++++++++++++++++++++++++++++++++++
            nAñoH = 0;
            nAñoM = 0;
            nAñoS = 0;
            var loopTo = _dataSet._AportacionNatAnual.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (_dataSet._AportacionNatAnual.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    _dataSet._HabMagnitudNat[0] = _dataSet._HabMagnitudNat[0] + _dataSet._AportacionNatAnual.aportacion[i];
                    nAñoH = nAñoH + 1;
                }
                else if (_dataSet._AportacionNatAnual.tipo[i] == TIPOAÑO.MEDIO)
                {
                    _dataSet._HabMagnitudNat[1] = _dataSet._HabMagnitudNat[1] + _dataSet._AportacionNatAnual.aportacion[i];
                    nAñoM = nAñoM + 1;
                }
                else
                {
                    _dataSet._HabMagnitudNat[2] = _dataSet._HabMagnitudNat[2] + _dataSet._AportacionNatAnual.aportacion[i];
                    nAñoS = nAñoS + 1;
                }
            }

            _dataSet._HabMagnitudNat[3] = (_dataSet._HabMagnitudNat[0] + _dataSet._HabMagnitudNat[1] + _dataSet._HabMagnitudNat[2]) / (nAñoH + nAñoM + nAñoS);
            _dataSet._HabMagnitudNat[0] = _dataSet._HabMagnitudNat[0] / nAñoH;
            _dataSet._HabMagnitudNat[1] = _dataSet._HabMagnitudNat[1] / nAñoM;
            _dataSet._HabMagnitudNat[2] = _dataSet._HabMagnitudNat[2] / nAñoS;



            // +++++++++++++++++++++++++++++++++++++++++++
            // +++++++++ Calculo de la Variabilidad ++++++
            // +++++++++++++++++++++++++++++++++++++++++++
            _dataSet._HabVariabilidadNat = new float[4];

            // diferencias
            float[] dif;
            dif = new float[_dataSet._AportacionNatAnualOrdAños.año.Length];
            float acH;
            float acM;
            float acS;
            acH = 0f;
            acM = 0f;
            acS = 0f;
            var loopTo1 = _dataSet._AportacionNatAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                float min, max;
                min = 999f;
                max = -1;
                if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    for (j = 0; j <= 11; j++)
                    {
                        float apor;
                        apor = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        if (apor >= max)
                        {
                            max = apor;
                        }

                        if (apor <= min)
                        {
                            min = apor;
                        }
                    }

                    dif[i] = max - min;
                    acH = acH + dif[i];
                }
                else if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                {
                    for (j = 0; j <= 11; j++)
                    {
                        float apor;
                        apor = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        if (apor >= max)
                        {
                            max = apor;
                        }

                        if (apor <= min)
                        {
                            min = apor;
                        }
                    }

                    dif[i] = max - min;
                    acM = acM + dif[i];
                }
                else
                {
                    for (j = 0; j <= 11; j++)
                    {
                        float apor;
                        apor = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        if (apor >= max)
                        {
                            max = apor;
                        }

                        if (apor <= min)
                        {
                            min = apor;
                        }
                    }

                    dif[i] = max - min;
                    acS = acS + dif[i];
                }
            }

            _dataSet._HabVariabilidadNat[3] = (acH + acM + acS) / (nAñoS + nAñoM + nAñoH);
            _dataSet._HabVariabilidadNat[0] = acH / nAñoH;
            _dataSet._HabVariabilidadNat[1] = acM / nAñoM;
            _dataSet._HabVariabilidadNat[2] = acS / nAñoS;

            // +++++++++++++++++++++++++++++++++++++++++++
            // +++++++++ Calculo de la Estacionalidad ++++
            // +++++++++++++++++++++++++++++++++++++++++++
            _dataSet._HabEstacionalidadNat = new string[3];
            float minH, maxH, minM, maxM, minS, maxS;
            STRING_MES_ORD mesMinH, mesMaxH, mesMinM, mesMaxM, mesMinS, mesMaxS;
            minH = 999999f;
            maxH = -1;
            minM = 999999f;
            maxM = -1;
            minS = 999999f;
            maxS = -1;
            mesMinH = (STRING_MES_ORD)999;
            mesMaxH = (STRING_MES_ORD)(-1);
            mesMinM = (STRING_MES_ORD)999;
            mesMaxM = (STRING_MES_ORD)(-1);
            mesMinS = (STRING_MES_ORD)999;
            mesMaxS = (STRING_MES_ORD)(-1);
            var loopTo2 = _dataSet._IntraAnualNat.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                if (maxH < _dataSet._IntraAnualNat[i][0])
                {
                    maxH = _dataSet._IntraAnualNat[i][0];
                    mesMaxH = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (minH > _dataSet._IntraAnualNat[i][0])
                {
                    minH = _dataSet._IntraAnualNat[i][0];
                    mesMinH = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (maxM < _dataSet._IntraAnualNat[i][1])
                {
                    maxM = _dataSet._IntraAnualNat[i][1];
                    mesMaxM = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (minM > _dataSet._IntraAnualNat[i][1])
                {
                    minM = _dataSet._IntraAnualNat[i][1];
                    mesMinM = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (maxS < _dataSet._IntraAnualNat[i][2])
                {
                    maxS = _dataSet._IntraAnualNat[i][2];
                    mesMaxS = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }

                if (minS > _dataSet._IntraAnualNat[i][2])
                {
                    minS = _dataSet._IntraAnualNat[i][2];
                    mesMinS = (STRING_MES_ORD)((i + _datos.mesInicio - 1) % 12);
                }
            }

            // Me._dataSet._HabEstacionalidadNat(0) = mesMaxH.ToString() + "-" + mesMinH.ToString()
            // Me._dataSet._HabEstacionalidadNat(1) = mesMaxM.ToString() + "-" + mesMinM.ToString()
            // Me._dataSet._HabEstacionalidadNat(2) = mesMaxS.ToString() + "-" + mesMinS.ToString()

            _dataSet._HabEstacionalidadNat[0] = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMaxH + 1).ToString()).Substring(0, 3).ToUpper() + "-" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMinH + 1).ToString()).Substring(0, 3).ToUpper();

            _dataSet._HabEstacionalidadNat[1] = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMaxM + 1).ToString()).Substring(0, 3).ToUpper() + "-" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMinM + 1).ToString()).Substring(0, 3).ToUpper();

            _dataSet._HabEstacionalidadNat[2] = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMaxS + 1).ToString()).Substring(0, 3).ToUpper() + "-" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)mesMinS + 1).ToString()).Substring(0, 3).ToUpper();

        }

        public void CalcularParametrosHabitualesCASO3()
        {

            // Los valores habituales de las series naturales se calculan igual
            CalcularParametrosHabitualesCASO1();

            // Calculo de los alterados
            CalcularParametrosHabitualesAlterados();
        }
        /// <summary>
        /// Calculo de los parametros habituales Alterados sin cotaeniedad
        /// </summary>
        /// <remarks>Se calcula en el informe 5a y 5c</remarks>
        public void CalcularParametrosHabitualesReducidos()
        {
            int i, j;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ CAMBIOS 9/1/08 ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Se cambian los informes 5A y 5C   +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._HabMagnitudAnualAlt = new float[3];
            _dataSet._HabMagnitudMensualAlt = new float[2];
            _dataSet._HabMagnitudMensualTablaAlt = new TablaEstacionalidad[3];
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // ++++++++++++++++++++++++++++++++++++
            // ++++++++++ Magnitud Anual ++++++++++
            // ++++++++++++++++++++++++++++++++++++
            // ---- Alterados -----
            _dataSet._HabMagnitudAlt = new float[1];
            float acum = 0f;

            // Comprobar que la lista de aportaciones anuales alteradas existe
            if (_dataSet._AportacionAltAnual.año is null)
            {
                CalcularINTERAnualAlterada();
            }

            var loopTo = _dataSet._AportacionAltAnual.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
                acum = acum + _dataSet._AportacionAltAnual.aportacion[i];

            // Media de aportaciones anuales
            // -----------------------------
            _dataSet._HabMagnitudAlt[0] = acum / _dataSet._AportacionAltAnual.año.Length;

            if (_dataSet._AportacionNatAnual.año != null)
            {
                // ---- Naturales ----
                acum = 0f;
                var loopTo1 = _dataSet._AportacionNatAnual.año.Length - 1;
                for (i = 0; i <= loopTo1; i++)
                    acum = acum + _dataSet._AportacionNatAnual.aportacion[i];
                _dataSet._HabMagnitudNatReducido = acum / _dataSet._AportacionNatAnual.año.Length;
            }
            // +++++++++++++++++++++++++++++++++++++++
            // +++++ CAMBIOS 9/1/08 ++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++
            // Se cambian los informes 5A y 5C   +++
            // +++++++++++++++++++++++++++++++++++++++

            // +++++++++++++ Parametros Habituales Alterados ANUALES +++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Dejo la variable donde se espera y me la copio en un nuevo array para
            // poder luego representarla.
            _dataSet._HabMagnitudAnualAlt[0] = _dataSet._HabMagnitudAlt[0];

            // +++++++ Mediana +++++++
            // +++++++++++++++++++++++
            float[] aporOrd;
            aporOrd = (float[])_dataSet._AportacionAltAnual.aportacion.Clone();
            Array.Sort(aporOrd);
            if (aporOrd.Length % 2 == 0)
            {
                _dataSet._HabMagnitudAnualAlt[1] = (aporOrd[(int)Math.Round(aporOrd.Length / 2d - 1d)] + aporOrd[(int)Math.Round(aporOrd.Length / 2d)]) / 2f;
            }
            else
            {
                _dataSet._HabMagnitudAnualAlt[1] = aporOrd[(int)Math.Round((aporOrd.Length - 1) / 2d)];
            }

            // +++++ Desv Estan +++++++
            // ++++++++++++++++++++++++
            float aux1, aux2;
            float desvEst;
            aux1 = 0f;
            aux2 = 0f;
            var loopTo2 = aporOrd.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                aux1 = (float)(aux1 + Math.Pow(aporOrd[i], 2d));
                aux2 = aux2 + aporOrd[i];
            }

            desvEst = (float)Math.Sqrt((aporOrd.Length * aux1 - Math.Pow(aux2, 2d)) / (aporOrd.Length * (aporOrd.Length - 1)));

            // +++++ Coe de Variación +++++
            // ++++++++++++++++++++++++++++
            _dataSet._HabMagnitudAnualAlt[2] = desvEst / _dataSet._HabMagnitudAnualAlt[0];

            // +++++++++++++ Parametros Habituales Alterados MENSUALES +++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._HabMagnitudMensualTablaAlt[0].ndias = new float[12];
            _dataSet._HabMagnitudMensualTablaAlt[1].ndias = new float[12];
            _dataSet._HabMagnitudMensualTablaAlt[2].ndias = new float[12];
            int auxMes;
            float[][] auxMediana;
            float[] auxDesv;
            auxMediana = new float[12][];
            auxDesv = new float[12];
            for (i = 0; i <= 11; i++)
            {
                _dataSet._HabMagnitudMensualTablaAlt[0].ndias[11] = 0f;
                _dataSet._HabMagnitudMensualTablaAlt[1].ndias[11] = 0f;
                _dataSet._HabMagnitudMensualTablaAlt[2].ndias[11] = 0f;
                auxMediana[i] = new float[_datos.SerieAltMensual.nAños];
            }

            // ++++++ Calculo de la media ++++++++++++
            // +++++++++++++++++++++++++++++++++++++++
            var loopTo3 = _dataSet._AportacionAltMen.aportacion.Length - 1;
            for (i = 0; i <= loopTo3; i++)
            {
                auxMes = _dataSet._AportacionAltMen.mes[i].Month - 1;
                _dataSet._HabMagnitudMensualTablaAlt[0].ndias[auxMes] = _dataSet._HabMagnitudMensualTablaAlt[0].ndias[auxMes] + _dataSet._AportacionAltMen.aportacion[i];
            }

            for (i = 0; i <= 11; i++)
                _dataSet._HabMagnitudMensualTablaAlt[0].ndias[i] = _dataSet._HabMagnitudMensualTablaAlt[0].ndias[i] / _datos.SerieAltMensual.nAños;

            // +++++ Calculo de la mediana +++++++++++
            // +++++++++++++++++++++++++++++++++++++++
            int añoIni;
            añoIni = _dataSet._AportacionAltMen.mes[0].Year;
            j = 0;
            var loopTo4 = _dataSet._AportacionAltMen.aportacion.Length - 1;
            for (i = 0; i <= loopTo4; i++)
            {
                if (añoIni != _dataSet._AportacionAltMen.mes[i].Year & _dataSet._AportacionAltMen.mes[i].Month == _datos.mesInicio)
                {
                    j = j + 1;
                }

                auxMediana[_dataSet._AportacionAltMen.mes[i].Month - 1][j] = _dataSet._AportacionAltMen.aportacion[i];
            }

            for (i = 0; i <= 11; i++)
            {
                Array.Sort(auxMediana[i]);
                if (_datos.SerieAltMensual.nAños == 0)
                {
                    _dataSet._HabMagnitudMensualTablaAlt[1].ndias[i] = (float)((auxMediana[i][(int)Math.Round(_datos.SerieAltMensual.nAños / 2d - 1d)] + _datos.SerieAltMensual.nAños / 2d) / 2d);
                }
                else
                {
                    _dataSet._HabMagnitudMensualTablaAlt[1].ndias[i] = auxMediana[i][(int)Math.Round((_datos.SerieAltMensual.nAños - 1) / 2d)];
                }
            }


            // +++++ Calculo del coeficiente +++++++++
            // +++++++++++++++++++++++++++++++++++++++
            // PREVIO: Calculo de las desv. estandar
            for (i = 0; i <= 11; i++)
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo5 = _datos.SerieAltMensual.nAños - 1;
                for (j = 0; j <= loopTo5; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(auxMediana[i][j], 2d));
                    aux2 = aux2 + auxMediana[i][j];
                }

                auxDesv[i] = (float)Math.Sqrt((_datos.SerieAltMensual.nAños * aux1 - Math.Pow(aux2, 2d)) / (_datos.SerieAltMensual.nAños * (_datos.SerieAltMensual.nAños - 1)));
            }
            // Calculo del coeficiente
            for (i = 0; i <= 11; i++)
                _dataSet._HabMagnitudMensualTablaAlt[2].ndias[i] = auxDesv[i] / _dataSet._HabMagnitudMensualTablaAlt[0].ndias[i];


            // +++++++++++++++++++++++++++++++
            // +++++ Estacionalidad ++++++++++
            // +++++++++++++++++++++++++++++++
            // 
            _dataSet._HabEstacionalidadMensualAlt = new TablaEstacionalidad[2];

            // ----------------------------------------------------------------
            // > Valores habituales > Aportaciones mensuales > Estacionalidad
            // ----------------------------------------------------------------
            // Busqueda de aparicion de maximo / minimos en los meses del año
            _dataSet._HabEstacionalidadMensualAlt[0].ndias = new float[12];
            _dataSet._HabEstacionalidadMensualAlt[1].ndias = new float[12];

            FrecuenciasRelativas fr = new FrecuenciasRelativas(_dataSet.MensualCaracterizadaAlterada, _datos.mesInicio);

            _dataSet._HabEstacionalidadMensualAlt[0].ndias = fr.Maximos;
            _dataSet._HabEstacionalidadMensualAlt[1].ndias = fr.Minimos;
            //int[] auxMes1;
            //int[] auxMes2;
            //auxMes1 = new int[1];
            //auxMes2 = new int[1];
            //int numMaximo = 0;
            //int numMinimo = 0;
            //añoIni = _dataSet._AportacionAltMen.mes[0].Year;
            //aux1 = -1;
            //aux2 = 99999f;
            //int nveces = 0;
            //var loopTo6 = _dataSet._AportacionAltMen.aportacion.Length - 1;
            //for (i = 0; i <= loopTo6; i++)
            //{
            //    // Se acaba el año, y meto todo los max/min encontrados en la lista final
            //    if (añoIni != _dataSet._AportacionAltMen.mes[i].Year & _dataSet._AportacionAltMen.mes[i].Month == _datos.mesInicio)
            //    {
            //        añoIni = _dataSet._AportacionAltMen.mes[i].Year;
            //        aux1 = -1;
            //        aux2 = 99999f;
            //        nveces += 1;
            //        var loopTo7 = auxMes1.Length - 2;
            //        for (j = 0; j <= loopTo7; j++)
            //            _dataSet._HabEstacionalidadMensualAlt[0].ndias[auxMes1[j] - 1] = _dataSet._HabEstacionalidadMensualAlt[0].ndias[auxMes1[j] - 1] + 1f;
            //        var loopTo8 = auxMes2.Length - 2;
            //        for (j = 0; j <= loopTo8; j++)
            //            _dataSet._HabEstacionalidadMensualAlt[1].ndias[auxMes2[j] - 1] = _dataSet._HabEstacionalidadMensualAlt[1].ndias[auxMes2[j] - 1] + 1f;

            //        // Contabilizar el numero total de maximos/minimos que se encuentran a lo largo de los años
            //        numMaximo += auxMes1.Length - 1;
            //        numMinimo += auxMes2.Length - 1;
            //    }

            //    if (aux1 <= _dataSet._AportacionAltMen.aportacion[i])
            //    {
            //        // Si es igual lo añado a la lista
            //        if (aux1 == _dataSet._AportacionAltMen.aportacion[i])
            //        {
            //            auxMes1[auxMes1.Length - 1] = _dataSet._AportacionAltMen.mes[i].Month;
            //            Array.Resize(ref auxMes1, auxMes1.Length + 1);
            //        }
            //        else
            //        {
            //            auxMes1 = new int[2];
            //            auxMes1[0] = _dataSet._AportacionAltMen.mes[i].Month;
            //        }

            //        aux1 = _dataSet._AportacionAltMen.aportacion[i];
            //    }

            //    if (aux2 >= _dataSet._AportacionAltMen.aportacion[i])
            //    {
            //        if (aux2 == _dataSet._AportacionAltMen.aportacion[i])
            //        {
            //            auxMes2[auxMes2.Length - 1] = _dataSet._AportacionAltMen.mes[i].Month;
            //            Array.Resize(ref auxMes2, auxMes2.Length + 1);
            //        }
            //        else
            //        {
            //            auxMes2 = new int[2];
            //            auxMes2[0] = _dataSet._AportacionAltMen.mes[i].Month;
            //        }

            //        aux2 = _dataSet._AportacionAltMen.aportacion[i];
            //    }
            //}

            //// ERROR DOC 27/08/09 - CA XXX
            //// -- Fallo en caso 6: Guadiana
            //// ----------------------------
            //// Ultima ejecución
            //var loopTo9 = auxMes1.Length - 2;
            //for (j = 0; j <= loopTo9; j++)
            //    _dataSet._HabEstacionalidadMensualAlt[0].ndias[auxMes1[j] - 1] = _dataSet._HabEstacionalidadMensualAlt[0].ndias[auxMes1[j] - 1] + 1f;
            //var loopTo10 = auxMes2.Length - 2;
            //for (j = 0; j <= loopTo10; j++)
            //    _dataSet._HabEstacionalidadMensualAlt[1].ndias[auxMes2[j] - 1] = _dataSet._HabEstacionalidadMensualAlt[1].ndias[auxMes2[j] - 1] + 1f;
            //numMaximo += auxMes1.Length - 1;
            //numMinimo += auxMes2.Length - 1;
            //for (i = 0; i <= 11; i++)
            //{
            //    // Me._dataSet._HabEstacionalidadMensualAlt(0).ndias(i) = Me._dataSet._HabEstacionalidadMensualAlt(0).ndias(i) / Me._datos.SerieAltMensual.nAños
            //    // Me._dataSet._HabEstacionalidadMensualAlt(1).ndias(i) = Me._dataSet._HabEstacionalidadMensualAlt(1).ndias(i) / Me._datos.SerieAltMensual.nAños
            //    _dataSet._HabEstacionalidadMensualAlt[0].ndias[i] = _dataSet._HabEstacionalidadMensualAlt[0].ndias[i] / numMaximo;
            //    _dataSet._HabEstacionalidadMensualAlt[1].ndias[i] = _dataSet._HabEstacionalidadMensualAlt[1].ndias[i] / numMinimo;
            //}

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Apor Mensuales > Magnitud > Variabilidad extrema 
            // ++++++++++++++++++++++++++++++++++++++++++++++++

            // ---- Alterados -----
            _dataSet._HabVariabilidadAlt = new float[1];
            var nMaximosAlt = new int[12];
            var nMaximosNat = new int[12];
            var nMinimosAlt = new int[12];
            var nMinimosNat = new int[12];
            for (i = 0; i <= 11; i++)
            {
                nMaximosAlt[i] = 0;
                nMaximosNat[i] = 0;
                nMinimosAlt[i] = 0;
                nMinimosNat[i] = 0;
            }

            int max;
            int min;
            var pos = default(int);
            acum = 0f;
            var loopTo11 = _dataSet._AportacionAltAnual.año.Length - 1;
            for (i = 0; i <= loopTo11; i++)
            {
                max = -1;
                min = 999999999;
                pos = i * 12;
                for (j = 0; j <= 11; j++)
                {
                    if (max < _dataSet._AportacionAltMen.aportacion[pos + j])
                    {
                        max = (int)Math.Round(_dataSet._AportacionAltMen.aportacion[pos + j]);
                        nMaximosAlt[j] = nMaximosAlt[j] + 1;
                    }

                    if (min > _dataSet._AportacionAltMen.aportacion[pos + j])
                    {
                        min = (int)Math.Round(_dataSet._AportacionAltMen.aportacion[pos + j]);
                        nMinimosAlt[j] = nMinimosAlt[j] + 1;
                    }
                }

                acum = acum + (max - min);
            }

            _dataSet._HabVariabilidadAlt[0] = acum / _dataSet._AportacionAltAnual.año.Length;

            // ---- Naturales -----
            if (_dataSet._AportacionNatAnual.año != null)
            {
                acum = 0f;
                var loopTo12 = _dataSet._AportacionNatAnual.año.Length - 1;
                for (i = 0; i <= loopTo12; i++)
                {
                    max = -1;
                    min = 999999999;
                    pos = i * 12;
                    for (j = 0; j <= 11; j++)
                    {
                        if (max < _dataSet._AportacionNatMen.aportacion[pos + j])
                        {
                            max = (int)Math.Round(_dataSet._AportacionNatMen.aportacion[pos + j]);
                            nMaximosNat[j] = nMaximosNat[j] + 1;
                        }

                        if (min > _dataSet._AportacionNatMen.aportacion[pos + j])
                        {
                            min = (int)Math.Round(_dataSet._AportacionNatMen.aportacion[pos + j]);
                        }
                    }

                    acum = acum + (max - min);
                }

                _dataSet._HabVariabilidadNatReducido = acum / _dataSet._AportacionNatAnual.año.Length;
            }
            // +++++++ Estacionalidad ++++++++++
            // +++++++++++++++++++++++++++++++++

            // ¿Que hay que hacer aqui?
            STRING_MES_ORD sMax = default, sMin = default;
            float EstMax, EstMin;

            // Calcular la tabla de frecuencias

            _dataSet._TablaFrecuenciaMaxMin.nat = new float[12];
            _dataSet._TablaFrecuenciaMaxMin.alt = new float[12];
            _dataSet._TablaFrecuenciaMaxMin.minNat = new float[12];
            _dataSet._TablaFrecuenciaMaxMin.minAlt = new float[12];
            _dataSet._TablaFrecuenciaMaxMin.posMaxAlt = new bool[12];
            _dataSet._TablaFrecuenciaMaxMin.posMaxNat = new bool[12];
            _dataSet._TablaFrecuenciaMaxMin.posMinNat = new bool[12];
            _dataSet._TablaFrecuenciaMaxMin.posMinAlt = new bool[12];

            // Dim max As Single = 0
            // Dim pos As Single = 0

            for (i = 0; i <= 11; i++)
            {
                _dataSet._TablaFrecuenciaMaxMin.nat[i] = 0f;
                _dataSet._TablaFrecuenciaMaxMin.alt[i] = 0f;
                _dataSet._TablaFrecuenciaMaxMin.minNat[i] = 0f;
                _dataSet._TablaFrecuenciaMaxMin.minAlt[i] = 0f;
                _dataSet._TablaFrecuenciaMaxMin.posMaxAlt[i] = false;
                _dataSet._TablaFrecuenciaMaxMin.posMaxNat[i] = false;
                _dataSet._TablaFrecuenciaMaxMin.posMinNat[i] = false;
                _dataSet._TablaFrecuenciaMaxMin.posMinAlt[i] = false;
            }
            if (_dataSet._AportacionNatAnual.año != null)
            {
                var loopTo13 = _dataSet._AportacionNatAnual.año.Length - 1;
                for (i = 0; i <= loopTo13; i++)
                {
                    max = 0;
                    for (j = 0; j <= 11; j++)
                    {
                        if (max < _dataSet._AportacionNatMen.aportacion[i * 12 + j])
                        {
                            max = (int)Math.Round(_dataSet._AportacionNatMen.aportacion[i * 12 + j]);
                            pos = j;
                        }
                    }

                    _dataSet._TablaFrecuenciaMaxMin.nat[pos] = _dataSet._TablaFrecuenciaMaxMin.nat[pos] + 1f;
                }
            }
            EstMax = 0f;
            EstMin = 99999f;
            var loopTo14 = _dataSet._AportacionAltAnual.año.Length - 1;
            for (i = 0; i <= loopTo14; i++)
            {
                max = 0;
                for (j = 0; j <= 11; j++)
                {
                    if (max < _dataSet._AportacionAltMen.aportacion[i * 12 + j])
                    {
                        max = (int)Math.Round(_dataSet._AportacionAltMen.aportacion[i * 12 + j]);
                        pos = j;
                        if (max > EstMax)
                        {
                            EstMax = max;
                            sMax = (STRING_MES_ORD)((j + _datos.mesInicio - 1) % 12);
                        }
                    }

                    if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] < EstMin)
                    {
                        EstMin = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        sMin = (STRING_MES_ORD)((j + _datos.mesInicio - 1) % 12);
                    }
                }

                _dataSet._TablaFrecuenciaMaxMin.alt[pos] = _dataSet._TablaFrecuenciaMaxMin.alt[pos] + 1f;
            }

            // Tratamiento de mínimos
            if (_dataSet._AportacionNatAnual.año != null)
            {
                var loopTo15 = _dataSet._AportacionNatAnual.año.Length - 1;
                for (i = 0; i <= loopTo15; i++)
                {
                    min = 999999;
                    for (j = 0; j <= 11; j++)
                    {
                        if (min > _dataSet._AportacionNatMen.aportacion[i * 12 + j])
                        {
                            min = (int)Math.Round(_dataSet._AportacionNatMen.aportacion[i * 12 + j]);
                            pos = j;
                        }
                    }

                    _dataSet._TablaFrecuenciaMaxMin.minNat[pos] = _dataSet._TablaFrecuenciaMaxMin.minNat[pos] + 1f;
                }
            }
            var loopTo16 = _dataSet._AportacionAltAnual.año.Length - 1;
            for (i = 0; i <= loopTo16; i++)
            {
                min = 999999;
                for (j = 0; j <= 11; j++)
                {
                    if (min > _dataSet._AportacionAltMen.aportacion[i * 12 + j])
                    {
                        min = (int)Math.Round(_dataSet._AportacionAltMen.aportacion[i * 12 + j]);
                        pos = j;
                    }
                }

                _dataSet._TablaFrecuenciaMaxMin.minAlt[pos] = _dataSet._TablaFrecuenciaMaxMin.minAlt[pos] + 1f;
            }

            max = 0;
            // min = 9999999
            min = 0;
            for (i = 0; i <= 11; i++)
            {
                if (max < _dataSet._TablaFrecuenciaMaxMin.nat[i])
                {
                    max = (int)Math.Round(_dataSet._TablaFrecuenciaMaxMin.nat[i]);
                }
                // If (min > Me._dataSet._TablaFrecuenciaMaxMin.nat(i)) Then
                // min = Me._dataSet._TablaFrecuenciaMaxMin.nat(i)
                // End If
                if (min < _dataSet._TablaFrecuenciaMaxMin.minNat[i])
                {
                    min = (int)Math.Round(_dataSet._TablaFrecuenciaMaxMin.minNat[i]);
                }
            }

            for (i = 0; i <= 11; i++)
            {
                if (max == _dataSet._TablaFrecuenciaMaxMin.nat[i])
                {
                    _dataSet._TablaFrecuenciaMaxMin.posMaxNat[i] = true;
                }
                // If (min = Me._dataSet._TablaFrecuenciaMaxMin.nat(i)) Then
                if (min == _dataSet._TablaFrecuenciaMaxMin.minNat[i])
                {
                    _dataSet._TablaFrecuenciaMaxMin.posMinNat[i] = true;
                }
            }

            max = 0;
            // min = 9999999
            min = 0;
            for (i = 0; i <= 11; i++)
            {
                if (max < _dataSet._TablaFrecuenciaMaxMin.alt[i])
                {
                    max = (int)Math.Round(_dataSet._TablaFrecuenciaMaxMin.alt[i]);
                }
                // If (min > Me._dataSet._TablaFrecuenciaMaxMin.alt(i)) Then
                // min = Me._dataSet._TablaFrecuenciaMaxMin.alt(i)
                // End If
                if (min < _dataSet._TablaFrecuenciaMaxMin.minAlt[i])
                {
                    min = (int)Math.Round(_dataSet._TablaFrecuenciaMaxMin.minAlt[i]);
                }
            }

            for (i = 0; i <= 11; i++)
            {
                if (max == _dataSet._TablaFrecuenciaMaxMin.alt[i])
                {
                    _dataSet._TablaFrecuenciaMaxMin.posMaxAlt[i] = true;
                }
                // If (min = Me._dataSet._TablaFrecuenciaMaxMin.alt(i)) Then
                if (min == _dataSet._TablaFrecuenciaMaxMin.minAlt[i])
                {
                    _dataSet._TablaFrecuenciaMaxMin.posMinAlt[i] = true;
                }
            }

            _dataSet._HabEstacionalidadAlt = new string[1];

            // Me._dataSet._HabEstacionalidadAlt(0) = sMax.ToString() + "-" + sMin.ToString()
            _dataSet._HabEstacionalidadAlt[0] = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)sMax + 1).ToString()).Substring(0, 3).ToUpper() + "-" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((int)sMin + 1).ToString()).Substring(0, 3).ToUpper();

        }
        /// <summary>
        /// Calculo de los parametros habituales Naturales sin cotaeniedad
        /// </summary>
        /// <remarks>Se calcula en el informe 4b</remarks>
        public void CalcularParametrosNaturalesHabitualesReducidos()
        {

            // Este calculo es una copia de lo que se hace para el informe 5
            
            int i, j;

            // Definir las variables
            _dataSet._HabMagnitudAnualNat = new float[3];
            _dataSet._HabMagnitudMensualNat = new float[1];
            _dataSet._HabMagnitudMensualTablaNat = new TablaEstacionalidad[3];
            _dataSet._HabEstacionalidadMensualNat = new TablaEstacionalidad[2];

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Parametros Habituales Naturales ANUALES ++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Comprobar que la lista de aportaciones anuales NATURALES existe
            if (_dataSet._AportacionNatAnual.año is null)
            {
                // Me.CalcularINTERAnualAlterada()
                CalcularINTERAnual();
            }

            // +++++ Apor Anuales > Magnitud > Media +++++
            // +++++++++++++++++++++++++++++++++++++++++++
            // ReDim Me._dataSet._HabMagnitudNat(0)
            float acum = 0f;

            // Saco la media (ya esta calculada previamente pero se vuelve a calcular)
            var loopTo = _dataSet._AportacionNatAnual.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
                acum = acum + _dataSet._AportacionNatAnual.aportacion[i];
            // Me._dataSet._HabMagnitudNat(0) = acum / Me._dataSet._AportacionAltAnual.año.Length
            _dataSet._HabMagnitudAnualNat[0] = acum / _dataSet._AportacionNatAnual.año.Length;

            // Extra que se usará en un futuro
            acum = 0f;
            var loopTo1 = _dataSet._AportacionNatAnual.año.Length - 1;
            for (i = 0; i <= loopTo1; i++)
                acum = acum + _dataSet._AportacionNatAnual.aportacion[i];
            _dataSet._HabMagnitudNatReducido = acum / _dataSet._AportacionNatAnual.año.Length;

            // +++++++ Aport Anuales > Magnitud >Mediana +++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++
            float[] aporOrd;
            aporOrd = (float[])_dataSet._AportacionNatAnual.aportacion.Clone();
            Array.Sort(aporOrd);
            if (aporOrd.Length % 2 == 0)
            {
                _dataSet._HabMagnitudAnualNat[1] = (aporOrd[(int)Math.Round(aporOrd.Length / 2d - 1d)] + aporOrd[(int)Math.Round(aporOrd.Length / 2d)]) / 2f;
            }
            else
            {
                _dataSet._HabMagnitudAnualNat[1] = aporOrd[(int)Math.Round((aporOrd.Length - 1) / 2d)];
            }

            // +++++ Aport Anuales > Magnitud > Coe de Variación +++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Calculo la desviacion estandar
            float aux1, aux2;
            float desvEst;
            aux1 = 0f;
            aux2 = 0f;
            var loopTo2 = aporOrd.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                aux1 = (float)(aux1 + Math.Pow(aporOrd[i], 2d));
                aux2 = aux2 + aporOrd[i];
            }

            desvEst = (float)Math.Sqrt((aporOrd.Length * aux1 - Math.Pow(aux2, 2d)) / (aporOrd.Length * (aporOrd.Length - 1)));
            _dataSet._HabMagnitudAnualNat[2] = desvEst / _dataSet._HabMagnitudAnualNat[0];

            // +++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Parametros Habituales Alterados MENSUALES ++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++

            // Defino las variables
            _dataSet._HabMagnitudMensualTablaNat[0].ndias = new float[12];
            _dataSet._HabMagnitudMensualTablaNat[1].ndias = new float[12];
            _dataSet._HabMagnitudMensualTablaNat[2].ndias = new float[12];
            int auxMes;
            float[][] auxMediana;
            float[] auxDesv;
            auxMediana = new float[12][];
            auxDesv = new float[12];
            for (i = 0; i <= 11; i++)
            {
                _dataSet._HabMagnitudMensualTablaNat[0].ndias[11] = 0f;
                _dataSet._HabMagnitudMensualTablaNat[1].ndias[11] = 0f;
                _dataSet._HabMagnitudMensualTablaNat[2].ndias[11] = 0f;
                auxMediana[i] = new float[_datos.SerieNatMensual.nAños];
            }

            // ++++ Aport Mensual > Magnitud > media ++++
            // ++++++++++++++++++++++++++++++++++++++++++
            var loopTo3 = _dataSet._AportacionNatMen.aportacion.Length - 1;
            for (i = 0; i <= loopTo3; i++)
            {
                auxMes = _dataSet._AportacionNatMen.mes[i].Month - 1;
                _dataSet._HabMagnitudMensualTablaNat[0].ndias[auxMes] = _dataSet._HabMagnitudMensualTablaNat[0].ndias[auxMes] + _dataSet._AportacionNatMen.aportacion[i];
            }

            for (i = 0; i <= 11; i++)
                _dataSet._HabMagnitudMensualTablaNat[0].ndias[i] = _dataSet._HabMagnitudMensualTablaNat[0].ndias[i] / _datos.SerieNatMensual.nAños;

            // ++++ Aport Mensual > Magnitud > mediana ++++
            // ++++++++++++++++++++++++++++++++++++++++++++
            int añoIni;
            añoIni = _dataSet._AportacionNatMen.mes[0].Year;
            j = 0;
            var loopTo4 = _dataSet._AportacionNatMen.aportacion.Length - 1;
            for (i = 0; i <= loopTo4; i++)
            {
                if (añoIni != _dataSet._AportacionNatMen.mes[i].Year & _dataSet._AportacionNatMen.mes[i].Month == _datos.mesInicio)
                {
                    j = j + 1;
                }

                auxMediana[_dataSet._AportacionNatMen.mes[i].Month - 1][j] = _dataSet._AportacionNatMen.aportacion[i];
            }

            for (i = 0; i <= 11; i++)
            {
                Array.Sort(auxMediana[i]);
                if (_datos.SerieNatMensual.nAños == 0)
                {
                    _dataSet._HabMagnitudMensualTablaNat[1].ndias[i] = (float)((auxMediana[i][(int)Math.Round(_datos.SerieNatMensual.nAños / 2d - 1d)] + _datos.SerieNatMensual.nAños / 2d) / 2d);
                }
                else
                {
                    _dataSet._HabMagnitudMensualTablaNat[1].ndias[i] = auxMediana[i][(int)Math.Round((_datos.SerieNatMensual.nAños - 1) / 2d)];
                }
            }


            // +++ Aport Mensual > Magnitud > Coef. de var. +++
            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // PREVIO: Calculo de las desv. estandar
            for (i = 0; i <= 11; i++)
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo5 = _datos.SerieNatMensual.nAños - 1;
                for (j = 0; j <= loopTo5; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(auxMediana[i][j], 2d));
                    aux2 = aux2 + auxMediana[i][j];
                }

                auxDesv[i] = (float)Math.Sqrt((_datos.SerieNatMensual.nAños * aux1 - Math.Pow(aux2, 2d)) / (_datos.SerieNatMensual.nAños * (_datos.SerieNatMensual.nAños - 1)));
            }
            // Calculo del coeficiente
            for (i = 0; i <= 11; i++)
                _dataSet._HabMagnitudMensualTablaNat[2].ndias[i] = auxDesv[i] / _dataSet._HabMagnitudMensualTablaNat[0].ndias[i];


            // +++++++++++++++++++++++++++++++
            // +++++ Estacionalidad ++++++++++
            // +++++++++++++++++++++++++++++++
            // 
            // ----------------------------------------------------------------
            // > Valores habituales > Aportaciones mensuales > Estacionalidad
            // ----------------------------------------------------------------
            // Busqueda de aparicion de maximo / minimos en los meses del año
            _dataSet._HabEstacionalidadMensualNat[0].ndias = new float[12];
            _dataSet._HabEstacionalidadMensualNat[1].ndias = new float[12];

            FrecuenciasRelativas fr = new FrecuenciasRelativas(_dataSet.MensualCaracterizadaNatural, _datos.mesInicio);

            _dataSet._HabEstacionalidadMensualNat[0].ndias = fr.Maximos;
            _dataSet._HabEstacionalidadMensualNat[1].ndias = fr.Minimos;
            //int[] auxMes1;
            //int[] auxMes2;
            //auxMes1 = new int[1];
            //auxMes2 = new int[1];
            //int numMaximo = 0;
            //int numMinimo = 0;
            //añoIni = _dataSet._AportacionNatMen.mes[0].Year;
            //aux1 = -1;
            //aux2 = 99999f;
            //int nveces = 0;
            //var loopTo6 = _dataSet._AportacionNatMen.aportacion.Length - 1;
            //for (i = 0; i <= loopTo6; i++)
            //{
            //    // Se acaba el año, y meto todo los max/min encontrados en la lista final
            //    if (añoIni != _dataSet._AportacionNatMen.mes[i].Year & _dataSet._AportacionNatMen.mes[i].Month == _datos.mesInicio)
            //    {
            //        añoIni = _dataSet._AportacionNatMen.mes[i].Year;
            //        aux1 = -1;
            //        aux2 = 99999f;
            //        nveces += 1;
            //        var loopTo7 = auxMes1.Length - 2;
            //        for (j = 0; j <= loopTo7; j++)
            //            _dataSet._HabEstacionalidadMensualNat[0].ndias[auxMes1[j] - 1] = _dataSet._HabEstacionalidadMensualNat[0].ndias[auxMes1[j] - 1] + 1f;
            //        var loopTo8 = auxMes2.Length - 2;
            //        for (j = 0; j <= loopTo8; j++)
            //            _dataSet._HabEstacionalidadMensualNat[1].ndias[auxMes2[j] - 1] = _dataSet._HabEstacionalidadMensualNat[1].ndias[auxMes2[j] - 1] + 1f;

            //        // Contabilizar el numero total de maximos/minimos que se encuentran a lo largo de los años
            //        numMaximo += auxMes1.Length - 1;
            //        numMinimo += auxMes2.Length - 1;
            //    }

            //    if (aux1 <= _dataSet._AportacionNatMen.aportacion[i])
            //    {
            //        // Si es igual lo añado a la lista
            //        if (aux1 == _dataSet._AportacionNatMen.aportacion[i])
            //        {
            //            auxMes1[auxMes1.Length - 1] = _dataSet._AportacionNatMen.mes[i].Month;
            //            Array.Resize(ref auxMes1, auxMes1.Length + 1);
            //        }
            //        else
            //        {
            //            auxMes1 = new int[2];
            //            auxMes1[0] = _dataSet._AportacionNatMen.mes[i].Month;
            //        }

            //        aux1 = _dataSet._AportacionNatMen.aportacion[i];
            //    }

            //    if (aux2 >= _dataSet._AportacionNatMen.aportacion[i])
            //    {
            //        if (aux2 == _dataSet._AportacionNatMen.aportacion[i])
            //        {
            //            auxMes2[auxMes2.Length - 1] = _dataSet._AportacionNatMen.mes[i].Month;
            //            Array.Resize(ref auxMes2, auxMes2.Length + 1);
            //        }
            //        else
            //        {
            //            auxMes2 = new int[2];
            //            auxMes2[0] = _dataSet._AportacionNatMen.mes[i].Month;
            //        }

            //        aux2 = _dataSet._AportacionNatMen.aportacion[i];
            //    }
            //}

            //// ERROR DOC 27/08/09 - CA XXX
            //// -- Fallo en caso 6: Guadiana
            //// ----------------------------
            //// Ultima ejecución
            //var loopTo9 = auxMes1.Length - 2;
            //for (j = 0; j <= loopTo9; j++)
            //    _dataSet._HabEstacionalidadMensualNat[0].ndias[auxMes1[j] - 1] = _dataSet._HabEstacionalidadMensualNat[0].ndias[auxMes1[j] - 1] + 1f;
            //var loopTo10 = auxMes2.Length - 2;
            //for (j = 0; j <= loopTo10; j++)
            //    _dataSet._HabEstacionalidadMensualNat[1].ndias[auxMes2[j] - 1] = _dataSet._HabEstacionalidadMensualNat[1].ndias[auxMes2[j] - 1] + 1f;
            //numMaximo += auxMes1.Length - 1;
            //numMinimo += auxMes2.Length - 1;
            //for (i = 0; i <= 11; i++)
            //{
            //    // Me._dataSet._HabEstacionalidadMensualAlt(0).ndias(i) = Me._dataSet._HabEstacionalidadMensualAlt(0).ndias(i) / Me._datos.SerieAltMensual.nAños
            //    // Me._dataSet._HabEstacionalidadMensualAlt(1).ndias(i) = Me._dataSet._HabEstacionalidadMensualAlt(1).ndias(i) / Me._datos.SerieAltMensual.nAños
            //    _dataSet._HabEstacionalidadMensualNat[0].ndias[i] = _dataSet._HabEstacionalidadMensualNat[0].ndias[i] / numMaximo;
            //    _dataSet._HabEstacionalidadMensualNat[1].ndias[i] = _dataSet._HabEstacionalidadMensualNat[1].ndias[i] / numMinimo;
            //}

            // +++ Aport Mensual > Magnitud > Varia. extrema +++
            // +++++++++++++++++++++++++++++++++++++++++++++++++
            // ---- Alterados -----
            _dataSet._HabVariabilidadAlt = new float[1];
            // Dim nMaximosAlt(11) As Integer
            var nMaximosNat = new int[12];
            // Dim nMinimosAlt(11) As Integer
            var nMinimosNat = new int[12];
            for (i = 0; i <= 11; i++)
            {
                // nMaximosAlt(i) = 0
                nMaximosNat[i] = 0;
                // nMinimosAlt(i) = 0
                nMinimosNat[i] = 0;
            }

            int max;
            int min;
            int pos;
            acum = 0;

            for (i = 0; i <= _dataSet._AportacionNatAnual.año.Length - 1; i++)
            {
                max = -1;
                min = 999999999;
                pos = i * 12;
                for (j = 0; j <= 11; j++)
                {
                    if (max < _dataSet._AportacionNatMen.aportacion[pos + j])
                    {
                        max = (int)(Math.Round(_dataSet._AportacionNatMen.aportacion[pos + j]));
                        nMaximosNat[j] = nMaximosNat[j] + 1;
                        Debug.Print(i + "-" + j + "-" + max);
                    }

                    if (min > _dataSet._AportacionNatMen.aportacion[pos + j])
                    {
                        min = (int)(Math.Round(_dataSet._AportacionNatMen.aportacion[pos + j]));
                        nMinimosNat[j] = nMinimosNat[j] + 1;
                    }
                }

                acum = acum + (max - min);
                
            }
            // Me._dataSet._HabVariabilidadAlt(0) = acum / Me._dataSet._AportacionAltAnual.año.Length
            _dataSet._HabMagnitudMensualNat[0] = acum / _dataSet._AportacionNatAnual.año.Length;

            // ' ---- Naturales -----
            // acum = 0

            // For i = 0 To Me._dataSet._AportacionNatAnual.año.Length - 1
            // max = -1
            // min = 999999999
            // pos = i * 12
            // For j = 0 To 11
            // If (max < Me._dataSet._AportacionNatMen.aportacion(pos + j)) Then
            // max = Me._dataSet._AportacionNatMen.aportacion(pos + j)
            // nMaximosNat(j) = nMaximosNat(j) + 1
            // End If
            // If (min > Me._dataSet._AportacionNatMen.aportacion(pos + j)) Then
            // min = Me._dataSet._AportacionNatMen.aportacion(pos + j)
            // End If
            // Next
            // acum = acum + (max - min)
            // Next
            // Me._dataSet._HabVariabilidadNatReducido = acum / Me._dataSet._AportacionNatAnual.año.Length

            // ' +++++++ Estacionalidad ++++++++++
            // ' +++++++++++++++++++++++++++++++++

            // ' ¿Que hay que hacer aqui?
            // Dim sMax, sMin As STRING_MES_ORD
            // Dim EstMax, EstMin As Single

            // ' Calcular la tabla de frecuencias

            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.nat(11)
            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.alt(11)
            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.minNat(11)
            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.minAlt(11)
            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.posMaxAlt(11)
            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.posMaxNat(11)
            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.posMinNat(11)
            // ReDim Me._dataSet._TablaFrecuenciaMaxMin.posMinAlt(11)

            // 'Dim max As Single = 0
            // 'Dim pos As Single = 0

            // For i = 0 To 11
            // Me._dataSet._TablaFrecuenciaMaxMin.nat(i) = 0
            // Me._dataSet._TablaFrecuenciaMaxMin.alt(i) = 0
            // Me._dataSet._TablaFrecuenciaMaxMin.minNat(i) = 0
            // Me._dataSet._TablaFrecuenciaMaxMin.minAlt(i) = 0
            // Me._dataSet._TablaFrecuenciaMaxMin.posMaxAlt(i) = False
            // Me._dataSet._TablaFrecuenciaMaxMin.posMaxNat(i) = False
            // Me._dataSet._TablaFrecuenciaMaxMin.posMinNat(i) = False
            // Me._dataSet._TablaFrecuenciaMaxMin.posMinAlt(i) = False
            // Next


            // For i = 0 To Me._dataSet._AportacionNatAnual.año.Length - 1
            // max = 0
            // For j = 0 To 11
            // If (max < Me._dataSet._AportacionNatMen.aportacion(i * 12 + j)) Then
            // max = Me._dataSet._AportacionNatMen.aportacion(i * 12 + j)
            // pos = j
            // End If

            // Next
            // Me._dataSet._TablaFrecuenciaMaxMin.nat(pos) = Me._dataSet._TablaFrecuenciaMaxMin.nat(pos) + 1
            // Next

            // EstMax = 0
            // EstMin = 99999
            // For i = 0 To Me._dataSet._AportacionAltAnual.año.Length - 1
            // max = 0
            // For j = 0 To 11
            // If (max < Me._dataSet._AportacionAltMen.aportacion(i * 12 + j)) Then
            // max = Me._dataSet._AportacionAltMen.aportacion(i * 12 + j)
            // pos = j
            // If (max > EstMax) Then
            // EstMax = max
            // sMax = (j + Me._datos.mesInicio - 1) Mod 12
            // End If
            // End If
            // If (Me._dataSet._AportacionAltMen.aportacion(i * 12 + j) < EstMin) Then
            // EstMin = Me._dataSet._AportacionAltMen.aportacion(i * 12 + j)
            // sMin = (j + Me._datos.mesInicio - 1) Mod 12
            // End If
            // Next
            // Me._dataSet._TablaFrecuenciaMaxMin.alt(pos) = Me._dataSet._TablaFrecuenciaMaxMin.alt(pos) + 1
            // Next

            // ' Tratamiento de mínimos
            // For i = 0 To Me._dataSet._AportacionNatAnual.año.Length - 1
            // min = 999999
            // For j = 0 To 11
            // If (min > Me._dataSet._AportacionNatMen.aportacion(i * 12 + j)) Then
            // min = Me._dataSet._AportacionNatMen.aportacion(i * 12 + j)
            // pos = j
            // End If
            // Next
            // Me._dataSet._TablaFrecuenciaMaxMin.minNat(pos) = Me._dataSet._TablaFrecuenciaMaxMin.minNat(pos) + 1

            // Next

            // For i = 0 To Me._dataSet._AportacionAltAnual.año.Length - 1
            // min = 999999
            // For j = 0 To 11
            // If (min > Me._dataSet._AportacionAltMen.aportacion(i * 12 + j)) Then
            // min = Me._dataSet._AportacionAltMen.aportacion(i * 12 + j)
            // pos = j
            // End If
            // Next
            // Me._dataSet._TablaFrecuenciaMaxMin.minAlt(pos) = Me._dataSet._TablaFrecuenciaMaxMin.minAlt(pos) + 1
            // Next

            // max = 0
            // 'min = 9999999
            // min = 0
            // For i = 0 To 11
            // If (max < Me._dataSet._TablaFrecuenciaMaxMin.nat(i)) Then
            // max = Me._dataSet._TablaFrecuenciaMaxMin.nat(i)
            // End If
            // 'If (min > Me._dataSet._TablaFrecuenciaMaxMin.nat(i)) Then
            // ' min = Me._dataSet._TablaFrecuenciaMaxMin.nat(i)
            // 'End If
            // If (min < Me._dataSet._TablaFrecuenciaMaxMin.minNat(i)) Then
            // min = Me._dataSet._TablaFrecuenciaMaxMin.minNat(i)
            // End If
            // Next
            // For i = 0 To 11
            // If (max = Me._dataSet._TablaFrecuenciaMaxMin.nat(i)) Then
            // Me._dataSet._TablaFrecuenciaMaxMin.posMaxNat(i) = True
            // End If
            // 'If (min = Me._dataSet._TablaFrecuenciaMaxMin.nat(i)) Then
            // If (min = Me._dataSet._TablaFrecuenciaMaxMin.minNat(i)) Then
            // Me._dataSet._TablaFrecuenciaMaxMin.posMinNat(i) = True
            // End If
            // Next
            // max = 0
            // 'min = 9999999
            // min = 0
            // For i = 0 To 11
            // If (max < Me._dataSet._TablaFrecuenciaMaxMin.alt(i)) Then
            // max = Me._dataSet._TablaFrecuenciaMaxMin.alt(i)
            // End If
            // 'If (min > Me._dataSet._TablaFrecuenciaMaxMin.alt(i)) Then
            // 'min = Me._dataSet._TablaFrecuenciaMaxMin.alt(i)
            // 'End If
            // If (min < Me._dataSet._TablaFrecuenciaMaxMin.minAlt(i)) Then
            // min = Me._dataSet._TablaFrecuenciaMaxMin.minAlt(i)
            // End If
            // Next
            // For i = 0 To 11
            // If (max = Me._dataSet._TablaFrecuenciaMaxMin.alt(i)) Then
            // Me._dataSet._TablaFrecuenciaMaxMin.posMaxAlt(i) = True
            // End If
            // 'If (min = Me._dataSet._TablaFrecuenciaMaxMin.alt(i)) Then
            // If (min = Me._dataSet._TablaFrecuenciaMaxMin.minAlt(i)) Then
            // Me._dataSet._TablaFrecuenciaMaxMin.posMinAlt(i) = True
            // End If
            // Next

            // ReDim Me._dataSet._HabEstacionalidadAlt(0)

            // Me._dataSet._HabEstacionalidadAlt(0) = sMax.ToString() + "-" + sMin.ToString()
            // 'Me._dataSet._HabEstacionalidadAlt(0) = 0

        }
        /// <summary>
        /// Parametros Variabilidad Diaria Habitual
        /// </summary>
        /// <remarks>Informe 4</remarks>
        public void CalculoParametrosVariabilidadDIARIAHabitual()
        {
            int i;
            _dataSet._HabVariabilidadDiaraNat = new float[2];
            CalcularTablaCQC(false); // Esto calcula la Tabla CQC

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCNat.pe[i] < 10f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._HabVariabilidadDiaraNat[0] = (10f - _dataSet._TablaCQCNat.pe[i - 1]) / (_dataSet._TablaCQCNat.pe[i] - _dataSet._TablaCQCNat.pe[i - 1]) * (_dataSet._TablaCQCNat.añomedio[i] - _dataSet._TablaCQCNat.añomedio[i - 1]) + _dataSet._TablaCQCNat.añomedio[i - 1];
            i = 0;
            while (_dataSet._TablaCQCNat.pe[i] < 90f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._HabVariabilidadDiaraNat[1] = (90f - _dataSet._TablaCQCNat.pe[i - 1]) / (_dataSet._TablaCQCNat.pe[i] - _dataSet._TablaCQCNat.pe[i - 1]) * (_dataSet._TablaCQCNat.añomedio[i] - _dataSet._TablaCQCNat.añomedio[i - 1]) + _dataSet._TablaCQCNat.añomedio[i - 1];
        }
        /// <summary>
        /// Parametros Variabilidad Diaria Habitual Alterada
        /// </summary>
        /// <remarks>Informe 5</remarks>
        public void CalculoParametrosVariabilidadDIARIAHabitualAlterada()
        {
            int i;
            _dataSet._HabVariabilidadDiaraAlt = new float[2];
            CalcularTablaCQC(true); // Esto calcula la Tabla CQC

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 10f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._HabVariabilidadDiaraAlt[0] = (10f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.añomedio[i] - _dataSet._TablaCQCAlt.añomedio[i - 1]) + _dataSet._TablaCQCAlt.añomedio[i - 1];
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 90f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._HabVariabilidadDiaraAlt[1] = (90f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.añomedio[i] - _dataSet._TablaCQCAlt.añomedio[i - 1]) + _dataSet._TablaCQCAlt.añomedio[i - 1];
        }

        /// <summary>
        /// Parametros de avenidas
        /// </summary>
        /// <remarks>Informe 4</remarks>
        public void CalculoParametrosAvenidasCASO4()
        {
            int nAños = _datos.SerieNatDiaria.nAños;
            int i, j;

            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Sacar la lista Qc +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // Media de los maximos caudales diarios anuales
            float[] listaMaxDiarios;
            listaMaxDiarios = new float[nAños];
            int añoActual;
            int pos;
            pos = 0;

            // Primer año
            añoActual = _datos.SerieNatDiaria.dia[0].Year;
            var loopTo = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Si el año es diferente cambio donde almacenar el maximo
                if (_datos.SerieNatDiaria.dia[i].Day == 1 & _datos.SerieNatDiaria.dia[i].Month == _datos.mesInicio & _datos.SerieNatDiaria.dia[i].Year != añoActual)
                {
                    pos = pos + 1;
                    listaMaxDiarios[pos] = 0f;
                    añoActual = _datos.SerieNatDiaria.dia[i].Year;
                }

                if (listaMaxDiarios[pos] < _datos.SerieNatDiaria.caudalDiaria[i])
                {
                    listaMaxDiarios[pos] = _datos.SerieNatDiaria.caudalDiaria[i];
                }
            }

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular la media (Qc)  ++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Sin problema de no poderse calcular
            _dataSet._AveMagnitudNat = new float[4];
            _dataSet._AveMagnitudNat[0] = 0f;
            var loopTo1 = nAños - 1;
            for (i = 0; i <= loopTo1; i++)
                _dataSet._AveMagnitudNat[0] = _dataSet._AveMagnitudNat[0] + listaMaxDiarios[i];
            _dataSet._AveMagnitudNat[0] = _dataSet._AveMagnitudNat[0] / nAños;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Generador de Lecho (Qgl) ++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // CON problema de no poderse calcular
            var desv = default(float);
            float media;
            float aux1, aux2;
            media = _dataSet._AveMagnitudNat[0]; // La media es el parametro 1
            if (media == 0f)
            {
                _dataSet._AveMagnitudNat[1] = -9999;
            }
            else
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo2 = nAños - 1;
                for (i = 0; i <= loopTo2; i++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMaxDiarios[i], 2d));
                    aux2 = aux2 + listaMaxDiarios[i];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desv = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveMagnitudNat[1] = (float)(_dataSet._AveMagnitudNat[0] * (0.7d + 0.6d * (desv / media)));
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Caudal Conectividad (Qconec) +++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Sin problema de no poderse calcular
            float alfa;
            float mu;
            float T;
            alfa = (float)(Math.Sqrt(6d) / Math.PI * desv);
            mu = (float)(media - 0.5772d * alfa);

            // Aux1 == F(X)
            aux1 = (float)Math.Exp(-Math.Exp(-(_dataSet._AveMagnitudNat[1] - mu) / alfa));
            T = 1f / (1f - aux1);

            // Calculo el nuevo periodo de retorno
            T = 2f * T;
            // Para calculos posteriores
            _dataSet._Ave2TNat = T;
            aux1 = 1f - 1f / T;
            _dataSet._AveMagnitudNat[2] = (float)(mu - alfa * Math.Log(-Math.Log(aux1)));


            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Avenida Habitual (Q5%) ++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Sin problema de no poderse calcular
            CalcularTablaCQC(false); // Esto calcula la Tabla CQC

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCNat.pe[i] < 5f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._AveMagnitudNat[3] = (5f - _dataSet._TablaCQCNat.pe[i - 1]) / (_dataSet._TablaCQCNat.pe[i] - _dataSet._TablaCQCNat.pe[i - 1]) * (_dataSet._TablaCQCNat.añomedio[i] - _dataSet._TablaCQCNat.añomedio[i - 1]) + _dataSet._TablaCQCNat.añomedio[i - 1];


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Coe de variacion de los maximos CV(Qc) ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // CON problema de no poderse calcular
            _dataSet._AveVariabilidadNat = new float[2];
            float mediaMax = 0f;
            float desvMax = 0f;
            var loopTo3 = nAños - 1;
            for (i = 0; i <= loopTo3; i++)
                mediaMax = mediaMax + listaMaxDiarios[i];
            mediaMax = mediaMax / nAños;
            if (mediaMax == 0f)
            {
                _dataSet._AveVariabilidadNat[0] = 0f;
            }
            else
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo4 = nAños - 1;
                for (j = 0; j <= loopTo4; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMaxDiarios[j], 2d));
                    aux2 = aux2 + listaMaxDiarios[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvMax = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveVariabilidadNat[0] = desvMax / mediaMax;
            }



            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Coe de variacion de la serie CV(Q5) ++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            float[] listaQ5;
            listaQ5 = new float[nAños];
            i = 0;
            while (_dataSet._TablaCQCNat.pe[i] < 5f)
                i = i + 1;
            var loopTo5 = nAños - 1;
            for (j = 0; j <= loopTo5; j++)
                listaQ5[j] = (5f - _dataSet._TablaCQCNat.pe[i - 1]) / (_dataSet._TablaCQCNat.pe[i] - _dataSet._TablaCQCNat.pe[i - 1]) * (_dataSet._TablaCQCNat.caudales[j][i] - _dataSet._TablaCQCNat.caudales[j][i - 1]) + _dataSet._TablaCQCNat.caudales[j][i - 1];
            var mediaQ5 = default(float);
            var loopTo6 = nAños - 1;
            for (j = 0; j <= loopTo6; j++)
                mediaQ5 = mediaQ5 + listaQ5[j];
            mediaQ5 = mediaQ5 / nAños;

            // ===== ¿SE PUEDE CALCULAR? =====
            if (mediaQ5 == 0f)
            {
                _dataSet._AveVariabilidadNat[1] = 0f;
            }
            else
            {
                float desvQ5;
                aux1 = 0f;
                aux2 = 0f;
                var loopTo7 = nAños - 1;
                for (j = 0; j <= loopTo7; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaQ5[j], 2d));
                    aux2 = aux2 + listaQ5[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvQ5 = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveVariabilidadNat[1] = desvQ5 / mediaQ5;
            }



            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calculo de la duracion        +++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            var diasSeguidosMaxQ5 = new int[nAños];
            float[][] ordEnAños;
            ordEnAños = new float[nAños][];
            int acum;

            // Relleno los caudales
            acum = 0;
            var loopTo8 = nAños - 1;
            for (i = 0; i <= loopTo8; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = _datos.SerieNatDiaria.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = _datos.SerieNatDiaria.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    Array.Resize(ref ordEnAños[i], 366);
                }
                else
                {
                    Array.Resize(ref ordEnAños[i], 365);
                }

                var loopTo9 = ordEnAños[i].Length - 1;
                for (j = 0; j <= loopTo9; j++)
                    ordEnAños[i][j] = _datos.SerieNatDiaria.caudalDiaria[acum + j];
                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    acum = acum + 366;
                }
                else
                {
                    acum = acum + 365;
                }
            }

            bool seguido;
            int nSeguido;
            var loopTo10 = nAños - 1;
            for (i = 0; i <= loopTo10; i++)
            {
                seguido = false;
                nSeguido = 0;
                diasSeguidosMaxQ5[i] = 0;
                for (j = 0; j <= 364; j++)
                {
                    // Si el caudal del dia es mayor que Q5%
                    if (ordEnAños[i][j] > _dataSet._AveMagnitudNat[3])
                    {
                        nSeguido = nSeguido + 1;
                        if (nSeguido > diasSeguidosMaxQ5[i])
                        {
                            diasSeguidosMaxQ5[i] = nSeguido;
                        }
                    }
                    // nSeguido = nSeguido + 1
                    else
                    {
                        nSeguido = 0;
                    }
                }
            }

            _dataSet._AveDuracionNat = 0f;
            var loopTo11 = nAños - 1;
            for (i = 0; i <= loopTo11; i++)
                _dataSet._AveDuracionNat = _dataSet._AveDuracionNat + diasSeguidosMaxQ5[i];
            _dataSet._AveDuracionNat = _dataSet._AveDuracionNat / nAños;


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._AveEstacionalidadNat.ndias = new float[12];
            _dataSet._AveEstacionalidadNat.mes = new string[12];
            int max;
            int mesAct;
            int posArray;
            for (i = 0; i <= 11; i++)
            {
                _dataSet._AveEstacionalidadNat.ndias[i] = 0f;
                _dataSet._AveEstacionalidadNat.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._AveEstacionalidadNat.mes[i]) == 0d)
                {
                    _dataSet._AveEstacionalidadNat.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieNatDiaria.dia[0].Month;
            max = 0;
            posArray = 0;
            var loopTo12 = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo12; i++)
            {
                if (mesAct != _datos.SerieNatDiaria.dia[i].Month)
                {
                    _dataSet._AveEstacionalidadNat.ndias[posArray] = _dataSet._AveEstacionalidadNat.ndias[posArray] + max;
                    mesAct = _datos.SerieNatDiaria.dia[i].Month;
                    max = 0;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }

                if (_datos.SerieNatDiaria.caudalDiaria[i] > _dataSet._AveMagnitudNat[3])  // AveMagNat(3) = Q5
                {
                    max = max + 1;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._AveEstacionalidadNat.ndias[i] = _dataSet._AveEstacionalidadNat.ndias[i] / nAños;
            _dataSet._AveEstacionalidadNat.mediaAño = 0f;
            for (i = 0; i <= 11; i++)
                _dataSet._AveEstacionalidadNat.mediaAño = _dataSet._AveEstacionalidadNat.mediaAño + _dataSet._AveEstacionalidadNat.ndias[i];
            _dataSet._AveEstacionalidadNat.mediaAño = _dataSet._AveEstacionalidadNat.mediaAño / 12f;
        }
        /// <summary>
        /// Parametros de sequias
        /// </summary>
        /// <remarks>Informe 4</remarks>
        public void CalculoParametrosSequiasCASO4()
        {
            int nAños = _datos.SerieNatDiaria.nAños;
            int i, j;
            _dataSet._SeqMagnitudNat = new float[2];

            // Calcular Qs
            float[] listaMinDiarios;
            listaMinDiarios = new float[nAños];
            int añoActual;
            int pos;
            pos = 0;

            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Sacar la lista Qs +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // Media de los minimos caudales diarios anuales
            listaMinDiarios[0] = 999999999f;
            añoActual = _datos.SerieNatDiaria.dia[0].Year;
            var loopTo = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Si el año es diferente cambio donde almacenar el maximo
                if (_datos.SerieNatDiaria.dia[i].Day == 1 & _datos.SerieNatDiaria.dia[i].Month == _datos.mesInicio & _datos.SerieNatDiaria.dia[i].Year != añoActual)
                {
                    pos = pos + 1;
                    listaMinDiarios[pos] = 999999999f;
                    añoActual = _datos.SerieNatDiaria.dia[i].Year;
                }

                if (listaMinDiarios[pos] > _datos.SerieNatDiaria.caudalDiaria[i])
                {
                    listaMinDiarios[pos] = _datos.SerieNatDiaria.caudalDiaria[i];
                }
            }
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular la media (Qs)  ++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqMagnitudNat[0] = 0f;
            var loopTo1 = nAños - 1;
            for (i = 0; i <= loopTo1; i++)
                _dataSet._SeqMagnitudNat[0] = _dataSet._SeqMagnitudNat[0] + listaMinDiarios[i];
            _dataSet._SeqMagnitudNat[0] = _dataSet._SeqMagnitudNat[0] / nAños;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Sequia Habitual (Q95%) ++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            if (_dataSet._TablaCQCNat.caudales is null)
            {
                CalcularTablaCQC(false); // Esto calcula la Tabla CQC
            }

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCNat.pe[i] < 95f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._SeqMagnitudNat[1] = (95f - _dataSet._TablaCQCNat.pe[i - 1]) / (_dataSet._TablaCQCNat.pe[i] - _dataSet._TablaCQCNat.pe[i - 1]) * (_dataSet._TablaCQCNat.añomedio[i] - _dataSet._TablaCQCNat.añomedio[i - 1]) + _dataSet._TablaCQCNat.añomedio[i - 1];


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Coe de variacion de los minimos CV(Qs) ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqVariabilidadNat = new float[2];
            float mediaMin = 0f;
            float desvMin = 0f;
            float aux1 = 0f;
            float aux2 = 0f;
            var loopTo2 = nAños - 1;
            for (i = 0; i <= loopTo2; i++)
                mediaMin = mediaMin + listaMinDiarios[i];
            mediaMin = mediaMin / nAños;
            // Si no se puede calcular lo marco
            if (mediaMin == 0f)
            {
                _dataSet._SeqVariabilidadNat[0] = 0f;
            }
            // Cambio realizado por Eduardo 21/11/07
            // -------------------------------------
            // Me._dataSet._SeqVariabilidadNat(0) = -9999
            else
            {
                var loopTo3 = nAños - 1;
                for (j = 0; j <= loopTo3; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMinDiarios[j], 2d));
                    aux2 = aux2 + listaMinDiarios[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvMin = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._SeqVariabilidadNat[0] = desvMin / mediaMin;
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Coe de variacion de la serie CV(Q95) +++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            float[] listaQ95;
            listaQ95 = new float[nAños];
            i = 0;
            while (_dataSet._TablaCQCNat.pe[i] < 95f)
                i = i + 1;
            var loopTo4 = nAños - 1;
            for (j = 0; j <= loopTo4; j++)
                listaQ95[j] = (95f - _dataSet._TablaCQCNat.pe[i - 1]) / (_dataSet._TablaCQCNat.pe[i] - _dataSet._TablaCQCNat.pe[i - 1]) * (_dataSet._TablaCQCNat.caudales[j][i] - _dataSet._TablaCQCNat.caudales[j][i - 1]) + _dataSet._TablaCQCNat.caudales[j][i - 1];
            var mediaQ95 = default(float);
            var loopTo5 = nAños - 1;
            for (j = 0; j <= loopTo5; j++)
                mediaQ95 = mediaQ95 + listaQ95[j];
            mediaQ95 = mediaQ95 / nAños;
            if (mediaQ95 == 0f)
            {
                _dataSet._SeqVariabilidadNat[1] = 0f;
            }
            else
            {
                float desvQ95;
                aux1 = 0f;
                aux2 = 0f;
                var loopTo6 = nAños - 1;
                for (j = 0; j <= loopTo6; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaQ95[j], 2d));
                    aux2 = aux2 + listaQ95[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvQ95 = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._SeqVariabilidadNat[1] = desvQ95 / mediaQ95;
            }


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            // INFORME 4
            // Valores extremos > Caudales minimos (sequias) > Estacionalidad
            // 
            _dataSet._SeqEstacionalidadNat.ndias = new float[12];
            _dataSet._SeqEstacionalidadNat.mes = new string[12];
            int max;
            int mesAct;
            int posArray;
            for (i = 0; i <= 11; i++)
            {
                _dataSet._SeqEstacionalidadNat.ndias[i] = 0f;
                _dataSet._SeqEstacionalidadNat.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._SeqEstacionalidadNat.mes[i]) == 0d)
                {
                    _dataSet._SeqEstacionalidadNat.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieNatDiaria.dia[0].Month;
            max = 0;
            posArray = 0;
            var loopTo7 = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo7; i++)
            {
                if (mesAct != _datos.SerieNatDiaria.dia[i].Month)
                {
                    _dataSet._SeqEstacionalidadNat.ndias[posArray] = _dataSet._SeqEstacionalidadNat.ndias[posArray] + max;
                    mesAct = _datos.SerieNatDiaria.dia[i].Month;
                    max = 0;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }
                // 
                // ERROR DOC 27/08/09 - CA XXX
                // -- Fallo en caso 5: Guadiana
                // ----------------------------
                // If (Me._datos.SerieNatDiaria.caudalDiaria(i) < Me._dataSet._SeqMagnitudNat(1)) Then
                if (_datos.SerieNatDiaria.caudalDiaria[i] <= _dataSet._SeqMagnitudNat[1])
                {
                    max = max + 1;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._SeqEstacionalidadNat.ndias[i] = _dataSet._SeqEstacionalidadNat.ndias[i] / nAños;
            _dataSet._SeqEstacionalidadNat.mediaAño = 0f;
            for (i = 0; i <= 11; i++)
                _dataSet._SeqEstacionalidadNat.mediaAño = _dataSet._SeqEstacionalidadNat.mediaAño + _dataSet._SeqEstacionalidadNat.ndias[i];
            _dataSet._SeqEstacionalidadNat.mediaAño = _dataSet._SeqEstacionalidadNat.mediaAño / 12f;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calculo de la duracion        +++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            // INFORME 4
            // Valores extremos > Caudales minimos (sequias) > Duración
            // 
            _dataSet._SeqDuracionNat = new float[2];
            var diasSeguidosMinQ95 = new int[nAños];
            float[][] ordEnAños;
            ordEnAños = new float[nAños][];
            int acum;

            // Relleno los caudales
            acum = 0;
            var loopTo8 = nAños - 1;
            for (i = 0; i <= loopTo8; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = _datos.SerieNatDiaria.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = _datos.SerieNatDiaria.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    Array.Resize(ref ordEnAños[i], 366);
                }
                else
                {
                    Array.Resize(ref ordEnAños[i], 365);
                }

                var loopTo9 = ordEnAños[i].Length - 1;
                for (j = 0; j <= loopTo9; j++)
                    ordEnAños[i][j] = _datos.SerieNatDiaria.caudalDiaria[acum + j];
                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    acum = acum + 366;
                }
                else
                {
                    acum = acum + 365;
                }
            }

            bool seguido;
            int nSeguido;
            var loopTo10 = nAños - 1;
            for (i = 0; i <= loopTo10; i++)
            {
                seguido = false;
                nSeguido = 0;
                diasSeguidosMinQ95[i] = 0;
                for (j = 0; j <= 364; j++)
                {
                    // Si el caudal del dia es mayor que Q5%
                    // 
                    // ERROR DOC 27/08/09 - CA XXX
                    // -- Fallo en caso 5: Guadiana
                    // ----------------------------
                    // If (ordEnAños(i)(j) < Me._dataSet._SeqMagnitudNat(1)) Then
                    if (ordEnAños[i][j] <= _dataSet._SeqMagnitudNat[1])
                    {
                        nSeguido = nSeguido + 1;
                        if (nSeguido > diasSeguidosMinQ95[i])
                        {
                            diasSeguidosMinQ95[i] = nSeguido;
                        }
                    }
                    // nSeguido = nSeguido + 1
                    else
                    {
                        nSeguido = 0;
                    }
                }
            }

            _dataSet._SeqDuracionNat[0] = 0f;
            var loopTo11 = nAños - 1;
            for (i = 0; i <= loopTo11; i++)
                _dataSet._SeqDuracionNat[0] = _dataSet._SeqDuracionNat[0] + diasSeguidosMinQ95[i];
            _dataSet._SeqDuracionNat[0] = _dataSet._SeqDuracionNat[0] / nAños;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++ Calculo de la Duracion (dias a nulo) +++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            int acumNulos = 0;
            // Dim acumAUX As Integer = 0
            // ReDim Me._dataSet._nDiasNulosNat(nAños)

            acum = 0;
            var loopTo12 = nAños - 1;
            for (i = 0; i <= loopTo12; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = _datos.SerieNatDiaria.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = _datos.SerieNatDiaria.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    for (j = 0; j <= 365; j++)
                    {
                        if (_datos.SerieNatDiaria.caudalDiaria[acum + j] == 0f)
                        {
                            acumNulos = acumNulos + 1;
                        }
                    }

                    acum = acum + 366;
                }
                else
                {
                    for (j = 0; j <= 364; j++)
                    {
                        if (_datos.SerieNatDiaria.caudalDiaria[acum + j] == 0f)
                        {
                            acumNulos = acumNulos + 1;
                        }
                    }
                    // ¿Posible error?
                    // acum = acum + 366
                    acum = acum + 365;
                }
                // Para calculos posteriores
                // Me._dataSet._nDiasNulosNat(i) = acumNulos
                // acumAUX = 0
            }

            _dataSet._SeqDuracionNat[1] = (float)(acumNulos / (double)nAños);

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++ Calculo por MES de los dias nulos +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqDuracionCerosMesNat.ndias = new float[12];
            _dataSet._SeqDuracionCerosMesNat.mes = new string[12];
            for (i = 0; i <= 11; i++)
            {
                _dataSet._SeqDuracionCerosMesNat.ndias[i] = 0f;
                _dataSet._SeqDuracionCerosMesNat.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._SeqDuracionCerosMesNat.mes[i]) == 0d)
                {
                    _dataSet._SeqDuracionCerosMesNat.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieNatDiaria.dia[0].Month;
            posArray = 0;
            var loopTo13 = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo13; i++)
            {
                if (mesAct != _datos.SerieNatDiaria.dia[i].Month)
                {
                    mesAct = _datos.SerieNatDiaria.dia[i].Month;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }

                if (_datos.SerieNatDiaria.caudalDiaria[i] == 0f) //TODO: Modificación del umbral 0
                {
                    _dataSet._SeqDuracionCerosMesNat.ndias[posArray] = _dataSet._SeqDuracionCerosMesNat.ndias[posArray] + 1f;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._SeqDuracionCerosMesNat.ndias[i] = _dataSet._SeqDuracionCerosMesNat.ndias[i] / nAños;
        }

        /// <summary>
        /// Parametros Avenidas Alterada
        /// </summary>
        /// <remarks>Informe 5</remarks>
        public void CalculoParametrosAvenidasAlteradosCASO6()
        {
            int nAños = _datos.SerieAltDiaria.nAños;
            int i, j;

            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Sacar la lista Qc +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++

            float[] listaMaxDiarios;
            listaMaxDiarios = new float[nAños];
            int añoActual;
            int pos;
            pos = 0;

            // Primer año
            añoActual = _datos.SerieAltDiaria.dia[0].Year;
            var loopTo = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Si el año es diferente cambio donde almacenar el maximo
                if (_datos.SerieAltDiaria.dia[i].Day == 1 & _datos.SerieAltDiaria.dia[i].Month == _datos.mesInicio & _datos.SerieAltDiaria.dia[i].Year != añoActual)

                {
                    pos = pos + 1;
                    listaMaxDiarios[pos] = 0f;
                    añoActual = _datos.SerieAltDiaria.dia[i].Year;
                }

                if (listaMaxDiarios[pos] < _datos.SerieAltDiaria.caudalDiaria[i])
                {
                    listaMaxDiarios[pos] = _datos.SerieAltDiaria.caudalDiaria[i];
                }
            }

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular la media (Qc)  ++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++

            _dataSet._AveMagnitudAlt = new float[4];
            _dataSet._AveMagnitudAlt[0] = 0f;
            var loopTo1 = nAños - 1;
            for (i = 0; i <= loopTo1; i++)
                _dataSet._AveMagnitudAlt[0] = _dataSet._AveMagnitudAlt[0] + listaMaxDiarios[i];
            _dataSet._AveMagnitudAlt[0] = _dataSet._AveMagnitudAlt[0] / nAños;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Generador de Lecho (Qgl) ++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            var desv = default(float);
            float media;
            float aux1, aux2;
            media = _dataSet._AveMagnitudAlt[0]; // La media es el parametro 1

            // ======= Puede que no se pueda calcular ======
            if (media == 0f)
            {
                _dataSet._AveMagnitudAlt[1] = -9999;
            }
            else
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo2 = nAños - 1;
                for (i = 0; i <= loopTo2; i++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMaxDiarios[i], 2d));
                    aux2 = aux2 + listaMaxDiarios[i];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desv = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveMagnitudAlt[1] = (float)(_dataSet._AveMagnitudAlt[0] * (0.7d + 0.6d * (desv / media)));
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Caudal Conectividad (Qconec) +++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++

            float alfa;
            float mu;
            float T;
            alfa = (float)(Math.Sqrt(6d) / Math.PI * desv);
            mu = (float)(media - 0.5772d * alfa);

            // Saco F(X)
            aux1 = (float)Math.Exp(-Math.Exp(-(_dataSet._AveMagnitudAlt[1] - mu) / alfa));
            T = 1f / (1f - aux1);
            // Calculo el nuevo periodo de retorno
            T = 2f * T;

            // Para calculos posteriores
            _dataSet._Ave2TAlt = T;
            aux1 = 1f - 1f / T;
            _dataSet._AveMagnitudAlt[2] = (float)(mu - alfa * Math.Log(-Math.Log(aux1)));

            // Este cambio viene para el IAH9 que ha cambiado
            // En este caso se guarda T[Qconec ALT]GUMBEL ALT
            // pero ahora se hace     T[Qconec NAT]CUMBEL ALT
            // Saco F(X)
            aux1 = (float)Math.Exp(-Math.Exp(-(_dataSet._AveMagnitudNat[2] - mu) / alfa));
            T = 1f / (1f - aux1);
            // Calculo el nuevo periodo de retorno
            // T = 2 * T
            // Para calculos posteriores
            _dataSet._Ave2TAlt = T;


            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Avenida Habitual (Q5%) ++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++

            CalcularTablaCQC(true); // Esto calcula la Tabla CQC

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 5f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._AveMagnitudAlt[3] = (5f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.añomedio[i] - _dataSet._TablaCQCAlt.añomedio[i - 1]) + _dataSet._TablaCQCAlt.añomedio[i - 1];


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Coe de variacion de los maximos CV(Qc) ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._AveVariabilidadAlt = new float[2];
            float mediaMax = 0f;
            float desvMax = 0f;
            var loopTo3 = nAños - 1;
            for (i = 0; i <= loopTo3; i++)
                mediaMax = mediaMax + listaMaxDiarios[i];
            mediaMax = mediaMax / nAños;
            if (mediaMax == 0f)
            {
                _dataSet._AveVariabilidadAlt[0] = 0f;
            }
            else
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo4 = nAños - 1;
                for (j = 0; j <= loopTo4; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMaxDiarios[j], 2d));
                    aux2 = aux2 + listaMaxDiarios[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvMax = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveVariabilidadAlt[0] = desvMax / mediaMax;
            }



            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Coe de variacion de la serie CV(Q5) ++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            float[] listaQ5;
            listaQ5 = new float[nAños];
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 5f)
                i = i + 1;
            var loopTo5 = nAños - 1;
            for (j = 0; j <= loopTo5; j++)
                listaQ5[j] = (5f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.caudales[j][i] - _dataSet._TablaCQCAlt.caudales[j][i - 1]) + _dataSet._TablaCQCAlt.caudales[j][i - 1];
            var mediaQ5 = default(float);
            var loopTo6 = nAños - 1;
            for (j = 0; j <= loopTo6; j++)
                mediaQ5 = mediaQ5 + listaQ5[j];
            mediaQ5 = mediaQ5 / nAños;
            if (mediaQ5 == 0f)
            {
                _dataSet._AveVariabilidadAlt[1] = 0f;
            }
            else
            {
                float desvQ5;
                aux1 = 0f;
                aux2 = 0f;
                var loopTo7 = nAños - 1;
                for (j = 0; j <= loopTo7; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaQ5[j], 2d));
                    aux2 = aux2 + listaQ5[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvQ5 = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveVariabilidadAlt[1] = desvQ5 / mediaQ5;
            }



            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calculo de la duracion        +++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            var diasSeguidosMaxQ5 = new int[nAños];
            float[][] ordEnAños;
            ordEnAños = new float[nAños][];
            int acum;

            // Relleno los caudales
            acum = 0;
            var loopTo8 = nAños - 1;
            for (i = 0; i <= loopTo8; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    Array.Resize(ref ordEnAños[i], 366);
                }
                else
                {
                    Array.Resize(ref ordEnAños[i], 365);
                }

                var loopTo9 = ordEnAños[i].Length - 1;
                for (j = 0; j <= loopTo9; j++)
                    ordEnAños[i][j] = _datos.SerieAltDiaria.caudalDiaria[acum + j];
                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    acum = acum + 366;
                }
                else
                {
                    acum = acum + 365;
                }
            }

            bool seguido;
            int nSeguido;
            var loopTo10 = nAños - 1;
            for (i = 0; i <= loopTo10; i++)
            {
                seguido = false;
                nSeguido = 0;
                diasSeguidosMaxQ5[i] = 0;
                for (j = 0; j <= 364; j++)
                {
                    // Si el caudal del dia es mayor que Q5%
                    // OJO QUE ES LA NATURAL
                    if (ordEnAños[i][j] > _dataSet._AveMagnitudNat[3])
                    {
                        nSeguido = nSeguido + 1;
                        if (nSeguido > diasSeguidosMaxQ5[i])
                        {
                            diasSeguidosMaxQ5[i] = nSeguido;
                        }
                    }
                    // nSeguido = nSeguido + 1
                    else
                    {
                        nSeguido = 0;
                    }
                }
            }

            _dataSet._AveDuracionAlt = 0f;
            var loopTo11 = nAños - 1;
            for (i = 0; i <= loopTo11; i++)
                _dataSet._AveDuracionAlt = _dataSet._AveDuracionAlt + diasSeguidosMaxQ5[i];
            _dataSet._AveDuracionAlt = _dataSet._AveDuracionAlt / nAños;


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            // ATENCION: El Q5 se usa el de las series NATURALES
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            _dataSet._AveEstacionalidadAlt.ndias = new float[12];
            _dataSet._AveEstacionalidadAlt.mes = new string[12];
            int max;
            int mesAct;
            int posArray;
            for (i = 0; i <= 11; i++)
            {
                _dataSet._AveEstacionalidadAlt.ndias[i] = 0f;
                _dataSet._AveEstacionalidadAlt.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._AveEstacionalidadAlt.mes[i]) == 0d)
                {
                    _dataSet._AveEstacionalidadAlt.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieAltDiaria.dia[0].Month;
            max = 0;
            posArray = 0;
            var loopTo12 = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo12; i++)
            {
                if (mesAct != _datos.SerieAltDiaria.dia[i].Month)
                {
                    _dataSet._AveEstacionalidadAlt.ndias[posArray] = _dataSet._AveEstacionalidadAlt.ndias[posArray] + max;
                    mesAct = _datos.SerieAltDiaria.dia[i].Month;
                    max = 0;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }
                // OJO -> Aqui va el cambio: Mira la Q5 Natural
                if (_datos.SerieAltDiaria.caudalDiaria[i] > _dataSet._AveMagnitudNat[3])
                {
                    max = max + 1;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._AveEstacionalidadAlt.ndias[i] = _dataSet._AveEstacionalidadAlt.ndias[i] / nAños;
            _dataSet._AveEstacionalidadAlt.mediaAño = 0f;
            for (i = 0; i <= 11; i++)
                _dataSet._AveEstacionalidadAlt.mediaAño = _dataSet._AveEstacionalidadAlt.mediaAño + _dataSet._AveEstacionalidadAlt.ndias[i];
            _dataSet._AveEstacionalidadAlt.mediaAño = _dataSet._AveEstacionalidadAlt.mediaAño / 12f;
        }
        /// <summary>
        /// Parametros Sequias Alterada
        /// </summary>
        /// <remarks>Informe 5</remarks>
        public void CalculoParametrosSequiasAlteradosCASO6()
        {
            int nAños = _datos.SerieAltDiaria.nAños;
            int i, j;
            _dataSet._SeqMagnitudAlt = new float[2];

            // Calcular Qc
            float[] listaMinDiarios;
            listaMinDiarios = new float[nAños];
            int añoActual;
            int pos;
            pos = 0;

            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Sacar la lista Qs +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // Primer año
            listaMinDiarios[0] = 999999999f;
            añoActual = _datos.SerieAltDiaria.dia[0].Year;
            var loopTo = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Si el año es diferente cambio donde almacenar el maximo
                if (_datos.SerieAltDiaria.dia[i].Day == 1 & _datos.SerieAltDiaria.dia[i].Month == _datos.mesInicio & _datos.SerieAltDiaria.dia[i].Year != añoActual)
                {
                    pos = pos + 1;
                    listaMinDiarios[pos] = 999999999f;
                    añoActual = _datos.SerieAltDiaria.dia[i].Year;
                }

                if (listaMinDiarios[pos] > _datos.SerieAltDiaria.caudalDiaria[i])
                {
                    listaMinDiarios[pos] = _datos.SerieAltDiaria.caudalDiaria[i];
                }
            }
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular la media (Qs)  ++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqMagnitudAlt[0] = 0f;
            var loopTo1 = nAños - 1;
            for (i = 0; i <= loopTo1; i++)
                _dataSet._SeqMagnitudAlt[0] = _dataSet._SeqMagnitudAlt[0] + listaMinDiarios[i];
            _dataSet._SeqMagnitudAlt[0] = _dataSet._SeqMagnitudAlt[0] / nAños;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Sequia Habitual (Q95%) ++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            if (_dataSet._TablaCQCAlt.caudales is null)
            {
                CalcularTablaCQC(true); // Esto calcula la Tabla CQC
            }

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 95f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._SeqMagnitudAlt[1] = (95f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.añomedio[i] - _dataSet._TablaCQCAlt.añomedio[i - 1]) + _dataSet._TablaCQCAlt.añomedio[i - 1];


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Coe de variacion de los minimos CV(Qs) ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqVariabilidadAlt = new float[2];
            float mediaMin = 0f;
            float desvMin = 0f;
            float aux1 = 0f;
            float aux2 = 0f;
            var loopTo2 = nAños - 1;
            for (i = 0; i <= loopTo2; i++)
                mediaMin = mediaMin + listaMinDiarios[i];
            mediaMin = mediaMin / nAños;
            // Si no se puede calcular lo marco
            if (mediaMin == 0f)
            {
                _dataSet._SeqVariabilidadAlt[0] = 0f;
            }
            else
            {
                var loopTo3 = nAños - 1;
                for (j = 0; j <= loopTo3; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMinDiarios[j], 2d));
                    aux2 = aux2 + listaMinDiarios[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvMin = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._SeqVariabilidadAlt[0] = desvMin / mediaMin;
            }


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Coe de variacion de la serie CV(Q95) +++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            float[] listaQ95;
            listaQ95 = new float[nAños];
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 95f)
                i = i + 1;
            var loopTo4 = nAños - 1;
            for (j = 0; j <= loopTo4; j++)
                listaQ95[j] = (95f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.caudales[j][i] - _dataSet._TablaCQCAlt.caudales[j][i - 1]) + _dataSet._TablaCQCAlt.caudales[j][i - 1];
            var mediaQ95 = default(float);
            var loopTo5 = nAños - 1;
            for (j = 0; j <= loopTo5; j++)
                mediaQ95 = mediaQ95 + listaQ95[j];
            mediaQ95 = mediaQ95 / nAños;
            if (mediaQ95 == 0f)
            {
                _dataSet._SeqVariabilidadAlt[1] = 0f;
            }
            else
            {
                float desvQ95;
                aux1 = 0f;
                aux2 = 0f;
                var loopTo6 = nAños - 1;
                for (j = 0; j <= loopTo6; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaQ95[j], 2d));
                    aux2 = aux2 + listaQ95[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvQ95 = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._SeqVariabilidadAlt[1] = desvQ95 / mediaQ95;
            }


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // INFORME 5 y 5c
            // Valores extremos > Caudales minimos (sequias) > Estacionalidad
            // 
            // OJO: Cambio el Q95 por el de NATURAL
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            _dataSet._SeqEstacionalidadAlt.ndias = new float[12];
            _dataSet._SeqEstacionalidadAlt.mes = new string[12];
            int max;
            int mesAct;
            int posArray;
            for (i = 0; i <= 11; i++)
            {
                _dataSet._SeqEstacionalidadAlt.ndias[i] = 0f;
                _dataSet._SeqEstacionalidadAlt.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._SeqEstacionalidadAlt.mes[i]) == 0d)
                {
                    _dataSet._SeqEstacionalidadAlt.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieAltDiaria.dia[0].Month;
            max = 0;
            posArray = 0;
            var loopTo7 = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo7; i++)
            {
                if (mesAct != _datos.SerieAltDiaria.dia[i].Month)
                {
                    _dataSet._SeqEstacionalidadAlt.ndias[posArray] = _dataSet._SeqEstacionalidadAlt.ndias[posArray] + max;
                    mesAct = _datos.SerieAltDiaria.dia[i].Month;
                    max = 0;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }
                // OJO: Se usa el Q95 NATURAL
                // ---------------------------
                // 
                // ERROR DOC 27/08/09 - CA XXX
                // -- Fallo en caso 5: Guadiana
                // ----------------------------
                // If (Me._datos.SerieAltDiaria.caudalDiaria(i) < Me._dataSet._SeqMagnitudNat(1)) Then
                if (_datos.SerieAltDiaria.caudalDiaria[i] <= _dataSet._SeqMagnitudNat[1])
                {
                    max = max + 1;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._SeqEstacionalidadAlt.ndias[i] = _dataSet._SeqEstacionalidadAlt.ndias[i] / nAños;
            _dataSet._SeqEstacionalidadAlt.mediaAño = 0f;
            for (i = 0; i <= 11; i++)
                _dataSet._SeqEstacionalidadAlt.mediaAño = _dataSet._SeqEstacionalidadAlt.mediaAño + _dataSet._SeqEstacionalidadAlt.ndias[i];
            _dataSet._SeqEstacionalidadAlt.mediaAño = _dataSet._SeqEstacionalidadAlt.mediaAño / 12f;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calculo de la duracion        +++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            // Informe 5 y 5c
            // Valores extremos > Caudales minimos (sequias) > Duracion > "máximo dias consecutivos < Q95%"
            // 
            _dataSet._SeqDuracionAlt = new float[2];
            var diasSeguidosMinQ95 = new int[nAños];
            float[][] ordEnAños;
            ordEnAños = new float[nAños][];
            int acum;

            // Relleno los caudales
            acum = 0;
            var loopTo8 = nAños - 1;
            for (i = 0; i <= loopTo8; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    Array.Resize(ref ordEnAños[i], 366);
                }
                else
                {
                    Array.Resize(ref ordEnAños[i], 365);
                }

                var loopTo9 = ordEnAños[i].Length - 1;
                for (j = 0; j <= loopTo9; j++)
                    ordEnAños[i][j] = _datos.SerieAltDiaria.caudalDiaria[acum + j];
                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    acum = acum + 366;
                }
                else
                {
                    acum = acum + 365;
                }
            }

            bool seguido;
            int nSeguido;
            var loopTo10 = nAños - 1;
            for (i = 0; i <= loopTo10; i++)
            {
                seguido = false;
                nSeguido = 0;
                diasSeguidosMinQ95[i] = 0;
                for (j = 0; j <= 364; j++)
                {
                    // Si el caudal del dia es mayor que Q95%
                    // ERROR DOC 27/08/09 - CA XXX
                    // -- Fallo en caso 5: Guadiana
                    // ----------------------------
                    // If (ordEnAños(i)(j) < Me._dataSet._SeqMagnitudNat(1)) Then
                    if (ordEnAños[i][j] <= _dataSet._SeqMagnitudNat[1])
                    {
                        nSeguido = nSeguido + 1;
                        if (nSeguido > diasSeguidosMinQ95[i])
                        {
                            diasSeguidosMinQ95[i] = nSeguido;
                        }
                    }
                    // nSeguido = nSeguido + 1
                    else
                    {
                        nSeguido = 0;
                    }
                }
            }

            _dataSet._SeqDuracionAlt[0] = 0f;
            var loopTo11 = nAños - 1;
            for (i = 0; i <= loopTo11; i++)
                _dataSet._SeqDuracionAlt[0] = _dataSet._SeqDuracionAlt[0] + diasSeguidosMinQ95[i];
            _dataSet._SeqDuracionAlt[0] = _dataSet._SeqDuracionAlt[0] / nAños;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++ Calculo de la Duracion (dias a nulo) +++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._nDiasNulosAlt = new int[nAños];
            int acumNulos = 0;
            int acumAUX = 0;
            acum = 0;
            var loopTo12 = nAños - 1;
            for (i = 0; i <= loopTo12; i++)
            {
                if (DateTime.IsLeapYear(_datos.SerieAltDiaria.dia[acum].Year + 1) == true)
                {
                    for (j = 0; j <= 365; j++)
                    {
                        if (_datos.SerieAltDiaria.caudalDiaria[j] == 0f)
                        {
                            acumNulos = acumNulos + 1;
                            acumAUX = acumAUX + 1;
                        }
                    }

                    acum = acum + 366;
                }
                else
                {
                    for (j = 0; j <= 364; j++)
                    {
                        if (_datos.SerieAltDiaria.caudalDiaria[j] == 0f)
                        {
                            acumNulos = acumNulos + 1;
                            acumAUX = acumAUX + 1;
                        }
                    }

                    acum = acum + 366;
                }
                // Para calculos posteriores
                _dataSet._nDiasNulosAlt[i] = acumAUX;
                acumAUX = 0;
            }

            _dataSet._SeqDuracionAlt[1] = (float)(acumNulos / (double)nAños);


            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++ Calculo por MES de los dias nulos +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqDuracionCerosMesAlt.ndias = new float[12];
            _dataSet._SeqDuracionCerosMesAlt.mes = new string[12];
            for (i = 0; i <= 11; i++)
            {
                _dataSet._SeqDuracionCerosMesAlt.ndias[i] = 0f;
                _dataSet._SeqDuracionCerosMesAlt.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._SeqDuracionCerosMesAlt.mes[i]) == 0d)
                {
                    _dataSet._SeqDuracionCerosMesAlt.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieAltDiaria.dia[0].Month;
            posArray = 0;
            var loopTo13 = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo13; i++)
            {
                if (mesAct != _datos.SerieAltDiaria.dia[i].Month)
                {
                    mesAct = _datos.SerieAltDiaria.dia[i].Month;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }

                if (_datos.SerieAltDiaria.caudalDiaria[i] == 0f)
                {
                    _dataSet._SeqDuracionCerosMesAlt.ndias[posArray] = _dataSet._SeqDuracionCerosMesAlt.ndias[posArray] + 1f;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._SeqDuracionCerosMesAlt.ndias[i] = _dataSet._SeqDuracionCerosMesAlt.ndias[i] / nAños;
        }

        /// <summary>
        /// Parametros Avenidas Alterada Exclusivo Alterado
        /// </summary>
        /// <remarks>Informe 5</remarks>
        public void CalculoParametrosAvenidasSoloAlteradosCASO6()
        {
            int nAños = _datos.SerieAltDiaria.nAños;
            int i, j;

            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Sacar la lista Qc +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++

            float[] listaMaxDiarios;
            listaMaxDiarios = new float[nAños];
            int añoActual;
            int pos;
            pos = 0;

           // listaMaxDiarios = new float[_datos.SerieAltDiaria.nAños * 12];


            // Primer año
            añoActual = _datos.SerieAltDiaria.dia[0].Year;
            var loopTo = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Si el año es diferente cambio donde almacenar el maximo
                if (_datos.SerieAltDiaria.dia[i].Day == 1 & _datos.SerieAltDiaria.dia[i].Month == _datos.mesInicio & _datos.SerieAltDiaria.dia[i].Year != añoActual)

                {
                    pos = pos + 1;
                    listaMaxDiarios[pos] = 0f;
                    añoActual = _datos.SerieAltDiaria.dia[i].Year;
                }

                if (listaMaxDiarios[pos] < _datos.SerieAltDiaria.caudalDiaria[i])
                {
                    listaMaxDiarios[pos] = _datos.SerieAltDiaria.caudalDiaria[i];
                }
            }

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular la media (Qc)  ++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++

            _dataSet._AveMagnitudAlt = new float[4];
            _dataSet._AveMagnitudAlt[0] = 0f;
            var loopTo1 = nAños - 1;
            for (i = 0; i <= loopTo1; i++)
                _dataSet._AveMagnitudAlt[0] = _dataSet._AveMagnitudAlt[0] + listaMaxDiarios[i];
            _dataSet._AveMagnitudAlt[0] = _dataSet._AveMagnitudAlt[0] / nAños;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Generador de Lecho (Qgl) ++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            var desv = default(float);
            float media;
            float aux1, aux2;
            media = _dataSet._AveMagnitudAlt[0]; // La media es el parametro 1

            // ======= Puede que no se pueda calcular ======
            if (media == 0f)
            {
                _dataSet._AveMagnitudAlt[1] = -9999;
            }
            else
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo2 = nAños - 1;
                for (i = 0; i <= loopTo2; i++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMaxDiarios[i], 2d));
                    aux2 = aux2 + listaMaxDiarios[i];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desv = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveMagnitudAlt[1] = (float)(_dataSet._AveMagnitudAlt[0] * (0.7d + 0.6d * (desv / media)));
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Caudal Conectividad (Qconec) +++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++

            float alfa;
            float mu;
            float T;
            alfa = (float)(Math.Sqrt(6d) / Math.PI * desv);
            mu = (float)(media - 0.5772d * alfa);

            // Saco F(X)
            aux1 = (float)Math.Exp(-Math.Exp(-(_dataSet._AveMagnitudAlt[1] - mu) / alfa));
            T = 1f / (1f - aux1);
            // Calculo el nuevo periodo de retorno
            T = 2f * T;

            // Para calculos posteriores
            _dataSet._Ave2TAlt = T;
            aux1 = 1f - 1f / T;
            _dataSet._AveMagnitudAlt[2] = (float)(mu - alfa * Math.Log(-Math.Log(aux1)));

            // Este cambio viene para el IAH9 que ha cambiado
            // En este caso se guarda T[Qconec ALT]GUMBEL ALT
            // pero ahora se hace     T[Qconec NAT]CUMBEL ALT
            // Saco F(X)
                aux1 = (float)Math.Exp(-Math.Exp(-(_dataSet._AveMagnitudAlt[1] - mu) / alfa));
                T = 1f / (1f - aux1);
                // Calculo el nuevo periodo de retorno
                // T = 2 * T
                // Para calculos posteriores
                _dataSet._Ave2TAlt = T;
            
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Avenida Habitual (Q5%) ++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++

            CalcularTablaCQC(true); // Esto calcula la Tabla CQC

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 5f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._AveMagnitudAlt[3] = (5f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.añomedio[i] - _dataSet._TablaCQCAlt.añomedio[i - 1]) + _dataSet._TablaCQCAlt.añomedio[i - 1];


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Coe de variacion de los maximos CV(Qc) ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._AveVariabilidadAlt = new float[2];
            float mediaMax = 0f;
            float desvMax = 0f;
            var loopTo3 = nAños - 1;
            for (i = 0; i <= loopTo3; i++)
                mediaMax = mediaMax + listaMaxDiarios[i];
            mediaMax = mediaMax / nAños;
            if (mediaMax == 0f)
            {
                _dataSet._AveVariabilidadAlt[0] = 0f;
            }
            else
            {
                aux1 = 0f;
                aux2 = 0f;
                var loopTo4 = nAños - 1;
                for (j = 0; j <= loopTo4; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMaxDiarios[j], 2d));
                    aux2 = aux2 + listaMaxDiarios[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvMax = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveVariabilidadAlt[0] = desvMax / mediaMax;
            }



            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Coe de variacion de la serie CV(Q5) ++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            float[] listaQ5;
            listaQ5 = new float[nAños];
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 5f)
                i = i + 1;
            var loopTo5 = nAños - 1;
            for (j = 0; j <= loopTo5; j++)
                listaQ5[j] = (5f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.caudales[j][i] - _dataSet._TablaCQCAlt.caudales[j][i - 1]) + _dataSet._TablaCQCAlt.caudales[j][i - 1];
            var mediaQ5 = default(float);
            var loopTo6 = nAños - 1;
            for (j = 0; j <= loopTo6; j++)
                mediaQ5 = mediaQ5 + listaQ5[j];
            mediaQ5 = mediaQ5 / nAños;
            if (mediaQ5 == 0f)
            {
                _dataSet._AveVariabilidadAlt[1] = 0f;
            }
            else
            {
                float desvQ5;
                aux1 = 0f;
                aux2 = 0f;
                var loopTo7 = nAños - 1;
                for (j = 0; j <= loopTo7; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaQ5[j], 2d));
                    aux2 = aux2 + listaQ5[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvQ5 = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._AveVariabilidadAlt[1] = desvQ5 / mediaQ5;
            }



            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calculo de la duracion        +++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            var diasSeguidosMaxQ5 = new int[nAños];
            float[][] ordEnAños;
            ordEnAños = new float[nAños][];
            int acum;

            // Relleno los caudales
            acum = 0;
            var loopTo8 = nAños - 1;
            for (i = 0; i <= loopTo8; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    Array.Resize(ref ordEnAños[i], 366);
                }
                else
                {
                    Array.Resize(ref ordEnAños[i], 365);
                }

                var loopTo9 = ordEnAños[i].Length - 1;
                for (j = 0; j <= loopTo9; j++)
                    ordEnAños[i][j] = _datos.SerieAltDiaria.caudalDiaria[acum + j];
                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    acum = acum + 366;
                }
                else
                {
                    acum = acum + 365;
                }
            }

            bool seguido;
            int nSeguido;
            var loopTo10 = nAños - 1;
            for (i = 0; i <= loopTo10; i++)
            {
                seguido = false;
                nSeguido = 0;
                diasSeguidosMaxQ5[i] = 0;
                for (j = 0; j <= 364; j++)
                {
                    // Si el caudal del dia es mayor que Q5%
                    // OJO QUE ES LA NATURAL
                    if (ordEnAños[i][j] > _dataSet._AveMagnitudAlt[3])
                    {
                        nSeguido = nSeguido + 1;
                        if (nSeguido > diasSeguidosMaxQ5[i])
                        {
                            diasSeguidosMaxQ5[i] = nSeguido;
                        }
                    }
                    // nSeguido = nSeguido + 1
                    else
                    {
                        nSeguido = 0;
                    }
                }
            }

            _dataSet._AveDuracionAlt = 0f;
            var loopTo11 = nAños - 1;
            for (i = 0; i <= loopTo11; i++)
                _dataSet._AveDuracionAlt = _dataSet._AveDuracionAlt + diasSeguidosMaxQ5[i];
            _dataSet._AveDuracionAlt = _dataSet._AveDuracionAlt / nAños;


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            // ATENCION: El Q5 se usa el de las series NATURALES
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            _dataSet._AveEstacionalidadAlt.ndias = new float[12];
            _dataSet._AveEstacionalidadAlt.mes = new string[12];
            int max;
            int mesAct;
            int posArray;
            for (i = 0; i <= 11; i++)
            {
                _dataSet._AveEstacionalidadAlt.ndias[i] = 0f;
                _dataSet._AveEstacionalidadAlt.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._AveEstacionalidadAlt.mes[i]) == 0d)
                {
                    _dataSet._AveEstacionalidadAlt.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieAltDiaria.dia[0].Month;
            max = 0;
            posArray = 0;
            var loopTo12 = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo12; i++)
            {
                if (mesAct != _datos.SerieAltDiaria.dia[i].Month)
                {
                    _dataSet._AveEstacionalidadAlt.ndias[posArray] = _dataSet._AveEstacionalidadAlt.ndias[posArray] + max;
                    mesAct = _datos.SerieAltDiaria.dia[i].Month;
                    max = 0;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }
                // OJO -> Aqui va el cambio: Mira la Q5 Natural
                if (_datos.SerieAltDiaria.caudalDiaria[i] > _dataSet._AveMagnitudAlt[3]) //Cambio de NAT
                {
                    max = max + 1;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._AveEstacionalidadAlt.ndias[i] = _dataSet._AveEstacionalidadAlt.ndias[i] / nAños;
            _dataSet._AveEstacionalidadAlt.mediaAño = 0f;
            for (i = 0; i <= 11; i++)
                _dataSet._AveEstacionalidadAlt.mediaAño = _dataSet._AveEstacionalidadAlt.mediaAño + _dataSet._AveEstacionalidadAlt.ndias[i];
            _dataSet._AveEstacionalidadAlt.mediaAño = _dataSet._AveEstacionalidadAlt.mediaAño / 12f;
        }
        /// <summary>
        /// Parametros Sequias Alterada Exclusivo alterado
        /// </summary>
        /// <remarks>Informe 5</remarks>
        public void CalculoParametrosSequiasSoloAlteradosCASO6()
        {
            int nAños = _datos.SerieAltDiaria.nAños;
            int i, j;
            _dataSet._SeqMagnitudAlt = new float[2];

            // Calcular Qc
            float[] listaMinDiarios;
            listaMinDiarios = new float[nAños];
            int añoActual;
            int pos;
            pos = 0;

            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++ Sacar la lista Qs +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++
            // Primer año
            listaMinDiarios[0] = 999999999f;
            añoActual = _datos.SerieAltDiaria.dia[0].Year;
            var loopTo = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Si el año es diferente cambio donde almacenar el maximo
                if (_datos.SerieAltDiaria.dia[i].Day == 1 & _datos.SerieAltDiaria.dia[i].Month == _datos.mesInicio & _datos.SerieAltDiaria.dia[i].Year != añoActual)
                {
                    pos = pos + 1;
                    listaMinDiarios[pos] = 999999999f;
                    añoActual = _datos.SerieAltDiaria.dia[i].Year;
                }

                if (listaMinDiarios[pos] > _datos.SerieAltDiaria.caudalDiaria[i])
                {
                    listaMinDiarios[pos] = _datos.SerieAltDiaria.caudalDiaria[i];
                }
            }
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular la media (Qs)  ++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqMagnitudAlt[0] = 0f;
            var loopTo1 = nAños - 1;
            for (i = 0; i <= loopTo1; i++)
                _dataSet._SeqMagnitudAlt[0] = _dataSet._SeqMagnitudAlt[0] + listaMinDiarios[i];
            _dataSet._SeqMagnitudAlt[0] = _dataSet._SeqMagnitudAlt[0] / nAños;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Sequia Habitual (Q95%) ++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++
            if (_dataSet._TablaCQCAlt.caudales is null)
            {
                CalcularTablaCQC(true); // Esto calcula la Tabla CQC
            }

            // Buscar el 5%, 10%, 90% y 95%
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 95f)
                i = i + 1;
            // Interpolar el valor
            _dataSet._SeqMagnitudAlt[1] = (95f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.añomedio[i] - _dataSet._TablaCQCAlt.añomedio[i - 1]) + _dataSet._TablaCQCAlt.añomedio[i - 1];


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Coe de variacion de los minimos CV(Qs) ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqVariabilidadAlt = new float[2];
            float mediaMin = 0f;
            float desvMin = 0f;
            float aux1 = 0f;
            float aux2 = 0f;
            var loopTo2 = nAños - 1;
            for (i = 0; i <= loopTo2; i++)
                mediaMin = mediaMin + listaMinDiarios[i];
            mediaMin = mediaMin / nAños;
            // Si no se puede calcular lo marco
            if (mediaMin == 0f)
            {
                _dataSet._SeqVariabilidadAlt[0] = 0f;
            }
            else
            {
                var loopTo3 = nAños - 1;
                for (j = 0; j <= loopTo3; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaMinDiarios[j], 2d));
                    aux2 = aux2 + listaMinDiarios[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvMin = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._SeqVariabilidadAlt[0] = desvMin / mediaMin;
            }


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calcular Coe de variacion de la serie CV(Q95) +++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++
            float[] listaQ95;
            listaQ95 = new float[nAños];
            i = 0;
            while (_dataSet._TablaCQCAlt.pe[i] < 95f)
                i = i + 1;
            var loopTo4 = nAños - 1;
            for (j = 0; j <= loopTo4; j++)
                listaQ95[j] = (95f - _dataSet._TablaCQCAlt.pe[i - 1]) / (_dataSet._TablaCQCAlt.pe[i] - _dataSet._TablaCQCAlt.pe[i - 1]) * (_dataSet._TablaCQCAlt.caudales[j][i] - _dataSet._TablaCQCAlt.caudales[j][i - 1]) + _dataSet._TablaCQCAlt.caudales[j][i - 1];
            var mediaQ95 = default(float);
            var loopTo5 = nAños - 1;
            for (j = 0; j <= loopTo5; j++)
                mediaQ95 = mediaQ95 + listaQ95[j];
            mediaQ95 = mediaQ95 / nAños;
            if (mediaQ95 == 0f)
            {
                _dataSet._SeqVariabilidadAlt[1] = 0f;
            }
            else
            {
                float desvQ95;
                aux1 = 0f;
                aux2 = 0f;
                var loopTo6 = nAños - 1;
                for (j = 0; j <= loopTo6; j++)
                {
                    aux1 = (float)(aux1 + Math.Pow(listaQ95[j], 2d));
                    aux2 = aux2 + listaQ95[j];
                }

                aux2 = (float)Math.Pow(aux2, 2d);
                desvQ95 = (float)Math.Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)));
                _dataSet._SeqVariabilidadAlt[1] = desvQ95 / mediaQ95;
            }


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // INFORME 5 y 5c
            // Valores extremos > Caudales minimos (sequias) > Estacionalidad
            // 
            // OJO: Cambio el Q95 por el de NATURAL
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            _dataSet._SeqEstacionalidadAlt.ndias = new float[12];
            _dataSet._SeqEstacionalidadAlt.mes = new string[12];
            int max;
            int mesAct;
            int posArray;
            for (i = 0; i <= 11; i++)
            {
                _dataSet._SeqEstacionalidadAlt.ndias[i] = 0f;
                _dataSet._SeqEstacionalidadAlt.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._SeqEstacionalidadAlt.mes[i]) == 0d)
                {
                    _dataSet._SeqEstacionalidadAlt.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieAltDiaria.dia[0].Month;
            max = 0;
            posArray = 0;
            var loopTo7 = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo7; i++)
            {
                if (mesAct != _datos.SerieAltDiaria.dia[i].Month)
                {
                    _dataSet._SeqEstacionalidadAlt.ndias[posArray] = _dataSet._SeqEstacionalidadAlt.ndias[posArray] + max;
                    mesAct = _datos.SerieAltDiaria.dia[i].Month;
                    max = 0;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }
                // OJO: Se usa el Q95 NATURAL
                // ---------------------------
                // 
                // ERROR DOC 27/08/09 - CA XXX
                // -- Fallo en caso 5: Guadiana
                // ----------------------------
                // If (Me._datos.SerieAltDiaria.caudalDiaria(i) < Me._dataSet._SeqMagnitudNat(1)) Then
                if (_datos.SerieAltDiaria.caudalDiaria[i] <= _dataSet._SeqMagnitudAlt[1])
                {
                    max = max + 1;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._SeqEstacionalidadAlt.ndias[i] = _dataSet._SeqEstacionalidadAlt.ndias[i] / nAños;
            _dataSet._SeqEstacionalidadAlt.mediaAño = 0f;
            for (i = 0; i <= 11; i++)
                _dataSet._SeqEstacionalidadAlt.mediaAño = _dataSet._SeqEstacionalidadAlt.mediaAño + _dataSet._SeqEstacionalidadAlt.ndias[i];
            _dataSet._SeqEstacionalidadAlt.mediaAño = _dataSet._SeqEstacionalidadAlt.mediaAño / 12f;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Calculo de la duracion        +++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // 
            // Informe 5 y 5c
            // Valores extremos > Caudales minimos (sequias) > Duracion > "máximo dias consecutivos < Q95%"
            // 
            _dataSet._SeqDuracionAlt = new float[2];
            var diasSeguidosMinQ95 = new int[nAños];
            float[][] ordEnAños;
            ordEnAños = new float[nAños][];
            int acum;

            // Relleno los caudales
            acum = 0;
            var loopTo8 = nAños - 1;
            for (i = 0; i <= loopTo8; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = _datos.SerieAltDiaria.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    Array.Resize(ref ordEnAños[i], 366);
                }
                else
                {
                    Array.Resize(ref ordEnAños[i], 365);
                }

                var loopTo9 = ordEnAños[i].Length - 1;
                for (j = 0; j <= loopTo9; j++)
                    ordEnAños[i][j] = _datos.SerieAltDiaria.caudalDiaria[acum + j];
                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    acum = acum + 366;
                }
                else
                {
                    acum = acum + 365;
                }
            }

            bool seguido;
            int nSeguido;
            var loopTo10 = nAños - 1;
            for (i = 0; i <= loopTo10; i++)
            {
                seguido = false;
                nSeguido = 0;
                diasSeguidosMinQ95[i] = 0;
                for (j = 0; j <= 364; j++)
                {
                    // Si el caudal del dia es mayor que Q95%
                    // ERROR DOC 27/08/09 - CA XXX
                    // -- Fallo en caso 5: Guadiana
                    // ----------------------------
                    // If (ordEnAños(i)(j) < Me._dataSet._SeqMagnitudNat(1)) Then
                    if (ordEnAños[i][j] <= _dataSet._SeqMagnitudAlt[1])
                    {
                        nSeguido = nSeguido + 1;
                        if (nSeguido > diasSeguidosMinQ95[i])
                        {
                            diasSeguidosMinQ95[i] = nSeguido;
                        }
                    }
                    // nSeguido = nSeguido + 1
                    else
                    {
                        nSeguido = 0;
                    }
                }
            }

            _dataSet._SeqDuracionAlt[0] = 0f;
            var loopTo11 = nAños - 1;
            for (i = 0; i <= loopTo11; i++)
                _dataSet._SeqDuracionAlt[0] = _dataSet._SeqDuracionAlt[0] + diasSeguidosMinQ95[i];
            _dataSet._SeqDuracionAlt[0] = _dataSet._SeqDuracionAlt[0] / nAños;

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++ Calculo de la Duracion (dias a nulo) +++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._nDiasNulosAlt = new int[nAños];
            int acumNulos = 0;
            int acumAUX = 0;
            acum = 0;
            var loopTo12 = nAños - 1;
            for (i = 0; i <= loopTo12; i++)
            {
                if (DateTime.IsLeapYear(_datos.SerieAltDiaria.dia[acum].Year + 1) == true)
                {
                    for (j = 0; j <= 365; j++)
                    {
                        if (_datos.SerieAltDiaria.caudalDiaria[j] == 0f)
                        {
                            acumNulos = acumNulos + 1;
                            acumAUX = acumAUX + 1;
                        }
                    }

                    acum = acum + 366;
                }
                else
                {
                    for (j = 0; j <= 364; j++)
                    {
                        if (_datos.SerieAltDiaria.caudalDiaria[j] == 0f)
                        {
                            acumNulos = acumNulos + 1;
                            acumAUX = acumAUX + 1;
                        }
                    }

                    acum = acum + 366;
                }
                // Para calculos posteriores
                _dataSet._nDiasNulosAlt[i] = acumAUX;
                acumAUX = 0;
            }

            _dataSet._SeqDuracionAlt[1] = (float)(acumNulos / (double)nAños);


            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++ Calculo por MES de los dias nulos +++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._SeqDuracionCerosMesAlt.ndias = new float[12];
            _dataSet._SeqDuracionCerosMesAlt.mes = new string[12];
            for (i = 0; i <= 11; i++)
            {
                _dataSet._SeqDuracionCerosMesAlt.ndias[i] = 0f;
                _dataSet._SeqDuracionCerosMesAlt.mes[i] = ((i + _datos.mesInicio) % 12).ToString();
                if (Conversions.ToDouble(_dataSet._SeqDuracionCerosMesAlt.mes[i]) == 0d)
                {
                    _dataSet._SeqDuracionCerosMesAlt.mes[i] = 12.ToString();
                }
            }

            mesAct = _datos.SerieAltDiaria.dia[0].Month;
            posArray = 0;
            var loopTo13 = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo13; i++)
            {
                if (mesAct != _datos.SerieAltDiaria.dia[i].Month)
                {
                    mesAct = _datos.SerieAltDiaria.dia[i].Month;
                    posArray = posArray + 1;
                    if (posArray == 12)
                    {
                        posArray = 0;
                    }
                }

                if (_datos.SerieAltDiaria.caudalDiaria[i] == 0f)
                {
                    _dataSet._SeqDuracionCerosMesAlt.ndias[posArray] = _dataSet._SeqDuracionCerosMesAlt.ndias[posArray] + 1f;
                }
            }

            for (i = 0; i <= 11; i++)
                _dataSet._SeqDuracionCerosMesAlt.ndias[i] = _dataSet._SeqDuracionCerosMesAlt.ndias[i] / nAños;
        }



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        /// <summary>
        /// Valores de Indices habituales
        /// </summary>
        /// <remarks>Informes 7a y 7b</remarks>
        public void CalcularIndicesHabitualesCASO3()
        {
            int i, j;
            _dataSet._IndicesHabituales = new Indices[7];
            _dataSet._IndicesHabituales[0].calculado = false;
            _dataSet._IndicesHabituales[1].calculado = false;
            _dataSet._IndicesHabituales[2].calculado = false;
            _dataSet._IndicesHabituales[3].calculado = false;
            _dataSet._IndicesHabituales[4].calculado = false;
            _dataSet._IndicesHabituales[5].calculado = false;
            _dataSet._IndicesHabituales[6].calculado = false;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++ IAH1 - Indice 1: Magnitud de las aportaciones Anuales ++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            float acH, acM, acS;
            int nAñosH;
            int nAñosM;
            int nAñosS;
            int nCalInvertidosH = 0;
            int nCalInvertidosM = 0;
            int nCalInvertidosS = 0;
            acH = 0f;
            acM = 0f;
            acS = 0f;
            nAñosH = 0;
            nAñosM = 0;
            nAñosS = 0;
            object nAños = _dataSet._AportacionNatAnualOrdAños.año.Length;
            Array.Resize(ref _dataSet._IndicesHabituales[0].invertido, 4);
            Array.Resize(ref _dataSet._IndicesHabituales[0].indeterminacion, 4);
            for (i = 0; i <= 3; i++)
            {
                _dataSet._IndicesHabituales[0].invertido[i] = false;
                _dataSet._IndicesHabituales[0].indeterminacion[i] = false;
            }

            // Cambio importante: Los indices no se marcan como inverso hasta que el 50% de los
            // años no sean inversos
            var loopTo = Conversions.ToInteger(Operators.SubtractObject(nAños, 1));
            for (i = 0; i <= loopTo; i++)
            {
                if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    nAñosH = nAñosH + 1;
                    if (_dataSet._AportacionNatAnualOrdAños.aportacion[i] == 0f)
                    {
                        _dataSet._IndicesHabituales[0].indeterminacion[0] = true;
                        if (_dataSet._AportacionAltAnualOrdAños.aportacion[i] == 0f)
                        {
                            acH = acH + 1f;
                        }
                        else
                        {
                            acH = acH + 0f;
                        }
                    }
                    else if (_dataSet._AportacionAltAnualOrdAños.aportacion[i] > _dataSet._AportacionNatAnualOrdAños.aportacion[i])
                    {
                        acH = acH + _dataSet._AportacionNatAnualOrdAños.aportacion[i] / _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                        _dataSet._IndicesHabituales[0].invertido[0] = true;
                        nCalInvertidosH += 1;
                    }
                    else
                    {
                        acH = acH + _dataSet._AportacionAltAnualOrdAños.aportacion[i] / _dataSet._AportacionNatAnualOrdAños.aportacion[i];
                    }
                }
                else if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                {
                    nAñosM = nAñosM + 1;
                    if (_dataSet._AportacionNatAnualOrdAños.aportacion[i] == 0f)
                    {
                        _dataSet._IndicesHabituales[0].indeterminacion[1] = true;
                        if (_dataSet._AportacionAltAnualOrdAños.aportacion[i] == 0f)
                        {
                            acM = acM + 1f;
                        }
                        else
                        {
                            acM = acM + 0f;
                        }
                    }
                    else if (_dataSet._AportacionAltAnualOrdAños.aportacion[i] > _dataSet._AportacionNatAnualOrdAños.aportacion[i])
                    {
                        acM = acM + _dataSet._AportacionNatAnualOrdAños.aportacion[i] / _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                        _dataSet._IndicesHabituales[0].invertido[1] = true;
                        nCalInvertidosM += 1;
                    }
                    else
                    {
                        acM = acM + _dataSet._AportacionAltAnualOrdAños.aportacion[i] / _dataSet._AportacionNatAnualOrdAños.aportacion[i];
                    }
                }
                else
                {
                    nAñosS = nAñosS + 1;
                    if (_dataSet._AportacionNatAnualOrdAños.aportacion[i] == 0f)
                    {
                        _dataSet._IndicesHabituales[0].indeterminacion[2] = true;
                        if (_dataSet._AportacionAltAnualOrdAños.aportacion[i] == 0f)
                        {
                            acS = acS + 1f;
                        }
                        else
                        {
                            acS = acS + 0f;
                        }
                    }
                    else if (_dataSet._AportacionAltAnualOrdAños.aportacion[i] > _dataSet._AportacionNatAnualOrdAños.aportacion[i])
                    {
                        acS = acS + _dataSet._AportacionNatAnualOrdAños.aportacion[i] / _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                        _dataSet._IndicesHabituales[0].invertido[2] = true;
                        nCalInvertidosS += 1;
                    }
                    else
                    {
                        acS = acS + _dataSet._AportacionAltAnualOrdAños.aportacion[i] / _dataSet._AportacionNatAnualOrdAños.aportacion[i];
                    }
                }
            }

            acH = acH / nAñosH;
            acM = acM / nAñosM;
            acS = acS / nAñosS;

            // Ver si es invertido cada uno de los indices
            if (nCalInvertidosH / (double)nAñosH > 0.5d)
            {
                _dataSet._IndicesHabituales[0].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[0].invertido[0] = false;
            }

            if (nCalInvertidosM / (double)nAñosM > 0.5d)
            {
                _dataSet._IndicesHabituales[0].invertido[1] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[0].invertido[1] = false;
            }

            if (nCalInvertidosS / (double)nAñosS > 0.5d)
            {
                _dataSet._IndicesHabituales[0].invertido[2] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[0].invertido[2] = false;
            }

            _dataSet._IndicesHabituales[0].invertido[3] = _dataSet._IndicesHabituales[0].invertido[0] | _dataSet._IndicesHabituales[0].invertido[1] | _dataSet._IndicesHabituales[0].invertido[2];

            _dataSet._IndicesHabituales[0].indeterminacion[3] = _dataSet._IndicesHabituales[0].indeterminacion[0] | _dataSet._IndicesHabituales[0].indeterminacion[1] | _dataSet._IndicesHabituales[0].indeterminacion[2];

            Array.Resize(ref _dataSet._IndicesHabituales[0].valor, 4);
            _dataSet._IndicesHabituales[0].valor[0] = acH;
            _dataSet._IndicesHabituales[0].valor[1] = acM;
            _dataSet._IndicesHabituales[0].valor[2] = acS;
            _dataSet._IndicesHabituales[0].valor[3] = (float)(0.25d * acH + 0.5d * acM + 0.25d * acS);
            _dataSet._IndicesHabituales[0].calculado = true;


            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++ IAH2 - Indice 2: magnitud de aportaciones mensuales +++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            IAH2 iah2 = new IAH2(_dataSet.MensualCaracterizadaNatural, _dataSet.MensualCaracterizadaAlterada);

            iah2.Calculate();

            Array.Resize(ref _dataSet._IndicesHabituales[1].valor, 4);
            Array.Resize(ref _dataSet._IndicesHabituales[1].indeterminacion, 4);
            Array.Resize(ref _dataSet._IndicesHabituales[1].invertido, 4);

            _dataSet.IAH2 = iah2;

            _dataSet._IndicesHabituales[1].valor[0] = iah2.IAH2Hum.IAH2Ratio;
            _dataSet._IndicesHabituales[1].indeterminacion[0] = iah2.IAH2Hum.Indeterminado;
            _dataSet._IndicesHabituales[1].invertido[0] = iah2.IAH2Hum.Inverso;

            _dataSet._IndicesHabituales[1].valor[1] = iah2.IAH2Med.IAH2Ratio;
            _dataSet._IndicesHabituales[1].indeterminacion[1] = iah2.IAH2Med.Indeterminado;
            _dataSet._IndicesHabituales[1].invertido[1] = iah2.IAH2Med.Inverso;

            _dataSet._IndicesHabituales[1].valor[2] = iah2.IAH2Sec.IAH2Ratio;
            _dataSet._IndicesHabituales[1].indeterminacion[2] = iah2.IAH2Sec.Indeterminado;
            _dataSet._IndicesHabituales[1].invertido[2] = iah2.IAH2Sec.Inverso;

            _dataSet._IndicesHabituales[1].valor[3] = iah2.IAH2Pond.IAH2Ratio;
            _dataSet._IndicesHabituales[1].indeterminacion[3] = iah2.IAH2Pond.Indeterminado;
            _dataSet._IndicesHabituales[1].invertido[3] = iah2.IAH2Pond.Inverso;

            _dataSet._IndicesHabituales[1].calculado = true;
            //float acHm, acMm, acSm;
            //acH = 0f;
            //acM = 0f;
            //acS = 0f;
            //acHm = 0f;
            //acSm = 0f;
            //acMm = 0f;
            //nCalInvertidosH = 0;
            //nCalInvertidosM = 0;
            //nCalInvertidosS = 0;
            //nAñosH = 0;
            //nAñosM = 0;
            //nAñosS = 0;
            //Array.Resize(ref _dataSet._IndicesHabituales[1].invertido, 4);
            //Array.Resize(ref _dataSet._IndicesHabituales[1].indeterminacion, 4);
            //for (i = 0; i <= 3; i++)
            //{
            //    _dataSet._IndicesHabituales[1].invertido[i] = false;
            //    _dataSet._IndicesHabituales[1].indeterminacion[i] = false;
            //}

            //// Me._dataSet._IndicesHabituales(1).invertido = False

            //var loopTo1 = Conversions.ToInteger(Operators.SubtractObject(nAños, 1));
            //for (i = 0; i <= loopTo1; i++)
            //{
            //    if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
            //    {
            //        for (j = 0; j <= 11; j++)
            //        {
            //            if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] == 0f)
            //            {
            //                _dataSet._IndicesHabituales[1].indeterminacion[0] = true;
            //                if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] == 0f)
            //                {
            //                    acHm = acHm + 1f;
            //                }
            //                else
            //                {
            //                    acHm = acHm + 0f;
            //                }
            //            }
            //            else if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] > _dataSet._AportacionNatMen.aportacion[i * 12 + j])
            //            {
            //                acHm = acHm + _dataSet._AportacionNatMen.aportacion[i * 12 + j] / _dataSet._AportacionAltMen.aportacion[i * 12 + j];
            //                _dataSet._IndicesHabituales[1].invertido[0] = true;
            //                nCalInvertidosH += 1;
            //            }
            //            else
            //            {
            //                acHm = acHm + _dataSet._AportacionAltMen.aportacion[i * 12 + j] / _dataSet._AportacionNatMen.aportacion[i * 12 + j];
            //            }
            //        }

            //        nAñosH = nAñosH + 1;
            //        acHm = acHm / 12f;
            //        acH = acH + acHm;
            //        acHm = 0f;
            //    }
            //    else if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
            //    {
            //        for (j = 0; j <= 11; j++)
            //        {
            //            if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] == 0f)
            //            {
            //                _dataSet._IndicesHabituales[1].indeterminacion[1] = true;
            //                if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] == 0f)
            //                {
            //                    acMm = acMm + 1f;
            //                }
            //                else
            //                {
            //                    acMm = acMm + 0f;
            //                }
            //            }
            //            else if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] > _dataSet._AportacionNatMen.aportacion[i * 12 + j])
            //            {
            //                acMm = acMm + _dataSet._AportacionNatMen.aportacion[i * 12 + j] / _dataSet._AportacionAltMen.aportacion[i * 12 + j];
            //                _dataSet._IndicesHabituales[1].invertido[1] = true;
            //                nCalInvertidosM += 1;
            //            }
            //            else
            //            {
            //                acMm = acMm + _dataSet._AportacionAltMen.aportacion[i * 12 + j] / _dataSet._AportacionNatMen.aportacion[i * 12 + j];
            //            }
            //        }

            //        nAñosM = nAñosM + 1;
            //        acMm = acMm / 12f;
            //        acM = acM + acMm;
            //        acMm = 0f;
            //    }
            //    else
            //    {
            //        for (j = 0; j <= 11; j++)
            //        {
            //            if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] == 0f)
            //            {
            //                _dataSet._IndicesHabituales[1].indeterminacion[2] = true;
            //                if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] == 0f)
            //                {
            //                    acSm = acSm + 1f;
            //                }
            //                else
            //                {
            //                    acSm = acSm + 0f;
            //                }
            //            }
            //            else if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] > _dataSet._AportacionNatMen.aportacion[i * 12 + j])
            //            {
            //                acSm = acSm + _dataSet._AportacionNatMen.aportacion[i * 12 + j] / _dataSet._AportacionAltMen.aportacion[i * 12 + j];
            //                _dataSet._IndicesHabituales[1].invertido[2] = true;
            //                nCalInvertidosS += 1;
            //            }
            //            else
            //            {
            //                acSm = acSm + _dataSet._AportacionAltMen.aportacion[i * 12 + j] / _dataSet._AportacionNatMen.aportacion[i * 12 + j];
            //            }
            //        }

            //        nAñosS = nAñosS + 1;
            //        acSm = acSm / 12f;
            //        acS = acS + acSm;
            //        acSm = 0f;
            //    }
            //}

            //acH = acH / nAñosH;
            //acM = acM / nAñosM;
            //acS = acS / nAñosS;

            //// Ver si es invertido cada uno de los indices
            //if (nCalInvertidosH / (double)nAñosH > 0.5d)
            //{
            //    _dataSet._IndicesHabituales[1].invertido[0] = true;
            //}
            //else
            //{
            //    _dataSet._IndicesHabituales[1].invertido[0] = false;
            //}

            //if (nCalInvertidosM / (double)nAñosM > 0.5d)
            //{
            //    _dataSet._IndicesHabituales[1].invertido[1] = true;
            //}
            //else
            //{
            //    _dataSet._IndicesHabituales[1].invertido[1] = false;
            //}

            //if (nCalInvertidosS / (double)nAñosS > 0.5d)
            //{
            //    _dataSet._IndicesHabituales[1].invertido[2] = true;
            //}
            //else
            //{
            //    _dataSet._IndicesHabituales[1].invertido[2] = false;
            //}

            //_dataSet._IndicesHabituales[1].invertido[3] = _dataSet._IndicesHabituales[1].invertido[0] | _dataSet._IndicesHabituales[1].invertido[1] | _dataSet._IndicesHabituales[1].invertido[2];

            //_dataSet._IndicesHabituales[1].indeterminacion[3] = _dataSet._IndicesHabituales[1].indeterminacion[0] | _dataSet._IndicesHabituales[1].indeterminacion[1] | _dataSet._IndicesHabituales[1].indeterminacion[2];

            //Array.Resize(ref _dataSet._IndicesHabituales[1].valor, 4);
            //_dataSet._IndicesHabituales[1].valor[0] = acH;
            //_dataSet._IndicesHabituales[1].valor[1] = acM;
            //_dataSet._IndicesHabituales[1].valor[2] = acS;
            //_dataSet._IndicesHabituales[1].valor[3] = (float)(0.25d * acH + 0.5d * acM + 0.25d * acS);
            //_dataSet._IndicesHabituales[1].calculado = true;

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ Indice 3 : Variabilidad habitual++++++++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // NO SE CALCULA AQUI -> Tiene su propia funcion

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++ IAH 4 - Indice 4 : Variabilidad extrema ++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Array.Resize(ref _dataSet._IndicesHabituales[3].invertido, 4);
            Array.Resize(ref _dataSet._IndicesHabituales[3].indeterminacion, 4);
            for (i = 0; i <= 3; i++)
            {
                _dataSet._IndicesHabituales[3].invertido[i] = false;
                _dataSet._IndicesHabituales[3].indeterminacion[i] = false;
            }

            float maxN, minN, maxA, minA;
            acH = 0f;
            acM = 0f;
            acS = 0f;

            // acHm = 0
            // acSm = 0
            // acMm = 0

            nCalInvertidosH = 0;
            nCalInvertidosM = 0;
            nCalInvertidosS = 0;
            nAñosH = 0;
            nAñosM = 0;
            nAñosS = 0;
            Array.Resize(ref _dataSet._IndicesHabituales[3].invertido, 4);
            Array.Resize(ref _dataSet._IndicesHabituales[3].indeterminacion, 4);
            var loopTo2 = Conversions.ToInteger(Operators.SubtractObject(nAños, 1));
            for (i = 0; i <= loopTo2; i++)
            {
                if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    maxN = -1;
                    minN = 99999f;
                    maxA = -1;
                    minA = 99999f;
                    for (j = 0; j <= 11; j++)
                    {
                        if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] > maxN)
                        {
                            maxN = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] < minN)
                        {
                            minN = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] > maxA)
                        {
                            maxA = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] < minA)
                        {
                            minA = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        }
                    }

                    if (maxN - minN == 0f)
                    {
                        _dataSet._IndicesHabituales[3].invertido[0] = true;
                        nCalInvertidosH += 1;
                        if (maxA - minA == 0f)
                        {
                            acH = acH + 1f;
                        }
                        else
                        {
                            acH = acH;
                        }
                    }
                    else if (maxA - minA > maxN - minN)
                    {
                        _dataSet._IndicesHabituales[3].indeterminacion[0] = true;
                        acH = acH + (maxN - minN) / (maxA - minA);
                    }
                    else
                    {
                        acH = acH + (maxA - minA) / (maxN - minN);
                    }

                    // acH = acH + ((maxA - minA) / (maxN - minN))

                    nAñosH = nAñosH + 1;
                }
                else if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                {
                    maxN = -1;
                    minN = 99999f;
                    maxA = -1;
                    minA = 99999f;
                    for (j = 0; j <= 11; j++)
                    {
                        if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] > maxN)
                        {
                            maxN = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] < minN)
                        {
                            minN = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] > maxA)
                        {
                            maxA = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] < minA)
                        {
                            minA = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        }
                    }

                    if (maxN - minN == 0f)
                    {
                        _dataSet._IndicesHabituales[3].invertido[1] = true;
                        nCalInvertidosM += 1;
                        if (maxA - minA == 0f)
                        {
                            acM = acM + 1f;
                        }
                        else
                        {
                            acM = acM;
                        }
                    }
                    else if (maxA - minA > maxN - minN)
                    {
                        _dataSet._IndicesHabituales[3].indeterminacion[1] = true;
                        acM = acM + (maxN - minN) / (maxA - minA);
                    }
                    else
                    {
                        acM = acM + (maxA - minA) / (maxN - minN);
                    }
                    // acM = acM + ((maxA - minA) / (maxN - minN))

                    nAñosM = nAñosM + 1;
                }
                else
                {
                    maxN = -1;
                    minN = 99999f;
                    maxA = -1;
                    minA = 99999f;
                    for (j = 0; j <= 11; j++)
                    {
                        if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] > maxN)
                        {
                            maxN = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionNatMen.aportacion[i * 12 + j] < minN)
                        {
                            minN = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] > maxA)
                        {
                            maxA = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        }

                        if (_dataSet._AportacionAltMen.aportacion[i * 12 + j] < minA)
                        {
                            minA = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        }
                    }

                    if (maxN - minN == 0f)
                    {
                        _dataSet._IndicesHabituales[3].invertido[2] = true;
                        nCalInvertidosS += 1;
                        if (maxA - minA == 0f)
                        {
                            acS = acS + 1f;
                        }
                        else
                        {
                            acS = acS;
                        }
                    }
                    else if (maxA - minA > maxN - minN)
                    {
                        _dataSet._IndicesHabituales[3].indeterminacion[2] = true;
                        acS = acS + (maxN - minN) / (maxA - minA);
                    }
                    else
                    {
                        acS = acS + (maxA - minA) / (maxN - minN);
                    }

                    // acS = acS + ((maxA - minA) / (maxN - minN))

                    nAñosS = nAñosS + 1;
                }
            }

            acH = acH / nAñosH;
            acM = acM / nAñosM;
            acS = acS / nAñosS;

            // Ver si es invertido cada uno de los indices
            if (nCalInvertidosH / (double)nAñosH > 0.5d)
            {
                _dataSet._IndicesHabituales[3].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[3].invertido[0] = false;
            }

            if (nCalInvertidosM / (double)nAñosM > 0.5d)
            {
                _dataSet._IndicesHabituales[3].invertido[1] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[3].invertido[1] = false;
            }

            if (nCalInvertidosS / (double)nAñosS > 0.5d)
            {
                _dataSet._IndicesHabituales[3].invertido[2] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[3].invertido[2] = false;
            }

            _dataSet._IndicesHabituales[3].invertido[3] = _dataSet._IndicesHabituales[3].invertido[0] | _dataSet._IndicesHabituales[3].invertido[1] | _dataSet._IndicesHabituales[3].invertido[2];

            _dataSet._IndicesHabituales[3].indeterminacion[3] = _dataSet._IndicesHabituales[3].indeterminacion[0] | _dataSet._IndicesHabituales[3].indeterminacion[1] | _dataSet._IndicesHabituales[3].indeterminacion[2];

            Array.Resize(ref _dataSet._IndicesHabituales[3].valor, 4);
            _dataSet._IndicesHabituales[3].valor[0] = acH;
            _dataSet._IndicesHabituales[3].valor[1] = acM;
            _dataSet._IndicesHabituales[3].valor[2] = acS;
            _dataSet._IndicesHabituales[3].valor[3] = (float)(0.25d * acH + 0.5d * acM + 0.25d * acS);
            _dataSet._IndicesHabituales[3].calculado = true;


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++ Indice 5: +++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // No se calcula

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++ IAH 5 - Indice 6: Estacionalidad de Máximos+++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++            

            // Señalar los maximos y minimos de cada año

            // Array de todos los meses
            bool[] mesesMaximosNat;
            mesesMaximosNat = new bool[Conversions.ToInteger((int)Operators.SubtractObject(Operators.MultiplyObject(nAños, 12), 1) + 1)];
            bool[] mesesMaximosAlt;
            mesesMaximosAlt = new bool[Conversions.ToInteger((int)Operators.SubtractObject(Operators.MultiplyObject(nAños, 12), 1) + 1)];
            var loopTo3 = Conversions.ToInteger(Operators.SubtractObject(Operators.MultiplyObject(nAños, 12), 1));
            for (i = 0; i <= loopTo3; i++)
            {
                mesesMaximosNat[i] = false;
                mesesMaximosAlt[i] = false;
            }

            float maximo;
            int pos;
            int[] mesesMax = null;
            int diferencia;
            pos = 0;
            acH = 0f;
            acM = 0f;
            acS = 0f;

            // ++++++++++ Max Naturales ++++++++++++++++
            // Se busca el maximo anual (unico) para cada año
            var loopTo4 = Conversions.ToInteger(Operators.SubtractObject(nAños, 1));
            for (i = 0; i <= loopTo4; i++)
            {
                maximo = -1;
                for (j = 0; j <= 11; j++)
                {
                    if (maximo < _dataSet._AportacionNatMen.aportacion[i * 12 + j])
                    {
                        maximo = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        // Guardar y borrar los posibles anteriores maximos
                        mesesMax = new int[1];
                        mesesMax[0] = i * 12 + j;
                    }
                    else if (maximo == _dataSet._AportacionNatMen.aportacion[i * 12 + j])
                    {
                        Array.Resize(ref mesesMax, mesesMax.Length + 1);
                        mesesMax[mesesMax.Length - 1] = i * 12 + j;
                    }
                }
                // For j = 0 To mesesMax.Length - 1
                mesesMaximosNat[mesesMax[0]] = true;
                // Next
            }

            mesesMax = new int[1]; // Por seguridad
            pos = 0;

            // ++++++++++ Max Alteradas ++++++++++++++++
            // Se busca el maximo anual (multiple) para cada año
            var loopTo5 = Conversions.ToInteger(Operators.SubtractObject(nAños, 1));
            for (i = 0; i <= loopTo5; i++)
            {
                maximo = -1;
                for (j = 0; j <= 11; j++)
                {
                    if (maximo < _dataSet._AportacionAltMen.aportacion[i * 12 + j])
                    {
                        maximo = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        // Guardar y borrar los posibles anteriores maximos
                        mesesMax = new int[1];
                        mesesMax[0] = i * 12 + j;
                    }
                    else if (maximo == _dataSet._AportacionAltMen.aportacion[i * 12 + j])
                    {
                        Array.Resize(ref mesesMax, mesesMax.Length + 1);
                        mesesMax[mesesMax.Length - 1] = i * 12 + j;
                    }
                }

                var loopTo6 = mesesMax.Length - 1;
                for (j = 0; j <= loopTo6; j++)
                    mesesMaximosAlt[mesesMax[j]] = true;
            }


            // +++++++ Determinacion de ventanas
            var loopTo7 = mesesMaximosNat.Length - 1;
            for (i = 0; i <= loopTo7; i++)
            {

                // Encuentro maximo en los naturales
                if (mesesMaximosNat[i])
                {
                    int limInf = -1;
                    int limSup = -1;
                    int posMaxVentA = -1;
                    int posMaxVentB = -1;
                    var fechaMax = _dataSet._AportacionNatMen.mes[i];  // Saco la fecha del mes maximo
                    var fechaVentA = fechaMax.AddMonths(-6);     // Inicio de la ventana
                    var fechaVentB = fechaMax.AddMonths(6);      // Fin de la ventana

                    // Encontrar en que posicion en el array de meses de las fechas de los limites de ventana
                    var loopTo8 = _dataSet._AportacionNatMen.mes.Length - 1;
                    for (j = 0; j <= loopTo8; j++)
                    {
                        if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaVentA) == 0)
                        {
                            limInf = j;
                        }

                        if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaVentB) == 0)
                        {
                            limSup = j;
                        }
                    }

                    // Si no se han encontrado los limites es que no tenemos datos. Se pone el año como defecto.
                    // -> Inf 1/mesInicio       -> Sup dia/mesInicio-1
                    if (limInf < 0)
                    {
                        if (fechaMax.Month > _datos.mesInicio - 1) // If (fechaMax.Month > 9) Then
                        {
                            // limInf = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMax.Year, 10, 1))
                            var loopTo9 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo9; j++)
                            {
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], new DateTime(fechaMax.Year, _datos.mesInicio, 1)) == 0)
                                {
                                    limInf = j;
                                }
                            }
                        }
                        else
                        {
                            // limInf = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMax.Year - 1, 10, 1))
                            var loopTo10 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo10; j++)
                            {
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], new DateTime(fechaMax.Year - 1, _datos.mesInicio, 1)) == 0)
                                {
                                    limInf = j;
                                }
                            }
                        }
                    }

                    if (limSup < 0)
                    {
                        DateTime fechaSup;
                        int mesSup;
                        if (_datos.mesInicio == 1)
                        {
                            mesSup = 12;
                        }
                        else
                        {
                            mesSup = _datos.mesInicio - 1;
                        }

                        if (fechaMax.Month > _datos.mesInicio - 1) // If (fechaMax.Month > 9) Then
                        {
                            // limSup = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMax.Year + 1, 9, 30))
                            var loopTo11 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo11; j++)
                            {
                                fechaSup = new DateTime(fechaMax.Year + 1, mesSup, DateTime.DaysInMonth(fechaMax.Year + 1, mesSup));
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaSup) == 0)
                                {
                                    limSup = j;
                                }
                            }
                        }
                        else
                        {
                            // limSup = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMax.Year, 9, 30))
                            var loopTo12 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo12; j++)
                            {
                                fechaSup = new DateTime(fechaMax.Year, mesSup, DateTime.DaysInMonth(fechaMax.Year, mesSup));
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaSup) == 0)
                                {
                                    limSup = j;
                                }
                            }
                        }
                    }

                    // Buscar en las ventanas definidas
                    var loopTo13 = i;
                    for (j = limInf; j <= loopTo13; j++)
                    {
                        if (mesesMaximosAlt[j])
                        {
                            posMaxVentA = j;
                            break;
                        }
                    }

                    var loopTo14 = limSup;
                    for (j = i; j <= loopTo14; j++)
                    {
                        if (mesesMaximosAlt[j])
                        {
                            posMaxVentB = j;
                            break;
                        }
                    }

                    if (posMaxVentA != -1 & posMaxVentB != -1)
                    {
                        // i es el mes del maximo natural
                        if (i - posMaxVentA > posMaxVentB - i)
                        {
                            diferencia = i - posMaxVentA;
                        }
                        else
                        {
                            diferencia = posMaxVentB - i;
                        }
                    }
                    else if (posMaxVentA != -1 & posMaxVentB == -1)
                    {
                        diferencia = i - posMaxVentA;
                    }
                    else if (posMaxVentA == -1 & posMaxVentB != -1) // La mas usual
                    {
                        diferencia = posMaxVentB - i;
                    }
                    else
                    {
                        diferencia = 6;
                    }

                    // El año almacenado en la lista es el i, no el i+1
                    int añoBuscar;
                    if (fechaMax.Month > _datos.mesInicio - 1)
                    {
                        añoBuscar = fechaMax.Year;
                    }
                    else
                    {
                        añoBuscar = fechaMax.Year - 1;
                    }

                    // Posicion del año en la lista.
                    int posTipo = Array.BinarySearch(_dataSet._AportacionNatAnualOrdAños.año, añoBuscar);


                    // Mirar el tipo del año señalado en la lista.
                    if (_dataSet._AportacionNatAnualOrdAños.tipo[posTipo] == TIPOAÑO.HUMEDO)
                    {
                        acH = acH + diferencia;
                    }
                    else if (_dataSet._AportacionNatAnualOrdAños.tipo[posTipo] == TIPOAÑO.MEDIO)
                    {
                        acM = acM + diferencia;
                    }
                    else
                    {
                        acS = acS + diferencia;
                    }
                } // Encontrado maximo
            }

            acH = acH / nAñosH;
            acM = acM / nAñosM;
            acS = acS / nAñosS;
            acH = (float)(1d - 1d / 6d * acH);
            acM = (float)(1d - 1d / 6d * acM);
            acS = (float)(1d - 1d / 6d * acS);
            Array.Resize(ref _dataSet._IndicesHabituales[5].valor, 4);
            _dataSet._IndicesHabituales[5].valor[0] = acH;
            _dataSet._IndicesHabituales[5].valor[1] = acM;
            _dataSet._IndicesHabituales[5].valor[2] = acS;
            _dataSet._IndicesHabituales[5].calculado = true;
            // Me._dataSet._IndicesHabituales(5).invertido = False
            _dataSet._IndicesHabituales[5].valor[3] = (float)(0.25d * acH + 0.5d * acM + 0.25d * acS);


            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++ IAH 6 - Indice 7: Estacionalidad de Mínimos ++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Señalar los maximos y minimos de cada año

            // Array de todos los meses
            bool[] mesesMinimosNat;
            mesesMinimosNat = new bool[Conversions.ToInteger((int)Operators.SubtractObject(Operators.MultiplyObject(nAños, 12), 1) + 1)];
            bool[] mesesMinimosAlt;
            mesesMinimosAlt = new bool[Conversions.ToInteger((int)Operators.SubtractObject(Operators.MultiplyObject(nAños, 12), 1) + 1)];
            var loopTo15 = Conversions.ToInteger(Operators.SubtractObject(Operators.MultiplyObject(nAños, 12), 1));
            for (i = 0; i <= loopTo15; i++)
            {
                mesesMinimosNat[i] = false;
                mesesMinimosAlt[i] = false;
            }

            float minimo;
            // Dim pos As Integer
            int[] mesesMin = null;
            // Dim diferencia As Integer

            acH = 0f;
            acM = 0f;
            acS = 0f;
            pos = 0;

            // ++++++++++ Min Naturales ++++++++++++++++
            var loopTo16 = Conversions.ToInteger(Operators.SubtractObject(nAños, 1));
            for (i = 0; i <= loopTo16; i++)
            {
                minimo = 9999999f;
                for (j = 0; j <= 11; j++)
                {
                    if (minimo > _dataSet._AportacionNatMen.aportacion[i * 12 + j])
                    {
                        minimo = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        // Guardar y borrar los posibles anteriores maximos
                        mesesMin = new int[1];
                        mesesMin[0] = i * 12 + j;
                    }
                    else if (minimo == _dataSet._AportacionNatMen.aportacion[i * 12 + j])
                    {
                        Array.Resize(ref mesesMin, mesesMin.Length + 1);
                        mesesMin[mesesMin.Length - 1] = i * 12 + j;
                    }
                }
                // For j = 0 To mesesMin.Length - 1
                mesesMinimosNat[mesesMin[0]] = true;
                // Next
            }

            mesesMin = new int[1]; // Por seguridad
            pos = 0;

            // ++++++++++ Min Alteradas ++++++++++++++++
            var loopTo17 = Conversions.ToInteger(Operators.SubtractObject(nAños, 1));
            for (i = 0; i <= loopTo17; i++)
            {
                minimo = 999999f;
                for (j = 0; j <= 11; j++)
                {
                    if (minimo > _dataSet._AportacionAltMen.aportacion[i * 12 + j])
                    {
                        minimo = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                        // Guardar y borrar los posibles anteriores maximos
                        mesesMin = new int[1];
                        mesesMin[0] = i * 12 + j;
                    }
                    else if (minimo == _dataSet._AportacionAltMen.aportacion[i * 12 + j])
                    {
                        Array.Resize(ref mesesMin, mesesMin.Length + 1);
                        mesesMin[mesesMin.Length - 1] = i * 12 + j;
                    }
                }

                var loopTo18 = mesesMin.Length - 1;
                for (j = 0; j <= loopTo18; j++)
                    mesesMinimosAlt[mesesMin[j]] = true;
            }


            // +++++++ Determinacion de ventanas
            var loopTo19 = mesesMinimosNat.Length - 1;
            for (i = 0; i <= loopTo19; i++)
            {
                // Encuentro maximo en los naturales
                if (mesesMinimosNat[i])
                {
                    int limInf = -1;
                    int limSup = -1;
                    int posMaxVentA = -1;
                    int posMaxVentB = -1;
                    var fechaMin = _dataSet._AportacionNatMen.mes[i]; // Saco la fecha del mes maximo
                    var fechaVentA = fechaMin.AddMonths(-6);
                    var fechaVentB = fechaMin.AddMonths(6);

                    // Buscar los limites de la ventana en la lista de meses validos
                    var loopTo20 = _dataSet._AportacionNatMen.mes.Length - 1;
                    for (j = 0; j <= loopTo20; j++)
                    {
                        if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaVentA) == 0)
                        {
                            limInf = j;
                        }

                        if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaVentB) == 0)
                        {
                            limSup = j;
                        }
                    }

                    // Comprobar que el mes existe en mi lista (si no esta es que no es año valido/coetaneo) 
                    if (limInf < 0)
                    {
                        if (fechaMin.Month > _datos.mesInicio - 1) // If (fechaMin.Month > 9) Then
                        {
                            // limInf = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMin.Year, 10, 1))
                            var loopTo21 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo21; j++)
                            {
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], new DateTime(fechaMin.Year, _datos.mesInicio, 1)) == 0)
                                {
                                    limInf = j;
                                }
                            }
                        }
                        else
                        {
                            // limInf = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMin.Year - 1, 10, 1))
                            var loopTo22 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo22; j++)
                            {
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], new DateTime(fechaMin.Year - 1, _datos.mesInicio, 1)) == 0)
                                {
                                    limInf = j;
                                }
                            }
                        }
                    }

                    if (limSup < 0)
                    {
                        DateTime fechaSup;
                        int mesSup;
                        if (_datos.mesInicio == 1)
                        {
                            mesSup = 12;
                        }
                        else
                        {
                            mesSup = _datos.mesInicio - 1;
                        }

                        if (fechaMin.Month > _datos.mesInicio - 1) // If (fechaMin.Month > 9) Then
                        {
                            // limSup = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMin.Year + 1, 9, 30))
                            var loopTo23 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo23; j++)
                            {
                                fechaSup = new DateTime(fechaMin.Year + 1, mesSup, DateTime.DaysInMonth(fechaMin.Year + 1, mesSup));
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaSup) == 0)
                                {
                                    limSup = j;
                                }
                            }
                        }
                        else
                        {
                            // limSup = Array.BinarySearch(Me._dataSet._AportacionNatMen.mes, New Date(fechaMin.Year, 9, 30))
                            var loopTo24 = _dataSet._AportacionNatMen.mes.Length - 1;
                            for (j = 0; j <= loopTo24; j++)
                            {
                                fechaSup = new DateTime(fechaMin.Year, mesSup, DateTime.DaysInMonth(fechaMin.Year, mesSup));
                                if (DateTime.Compare(_dataSet._AportacionNatMen.mes[j], fechaSup) == 0)
                                {
                                    limSup = j;
                                }
                            }
                        }
                    }

                    // Buscar en las ventanas definidas
                    var loopTo25 = i;
                    for (j = limInf; j <= loopTo25; j++)
                    {
                        if (mesesMinimosAlt[j])
                        {
                            posMaxVentA = j;
                            break;
                        }
                    }

                    var loopTo26 = limSup;
                    for (j = i; j <= loopTo26; j++)
                    {
                        if (mesesMinimosAlt[j])
                        {
                            posMaxVentB = j;
                            // Exit For
                        }
                    }

                    if (posMaxVentA != -1 & posMaxVentB != -1)
                    {
                        // i es el mes del maximo natural
                        if (i - posMaxVentA > posMaxVentB - i)
                        {
                            diferencia = i - posMaxVentA;
                        }
                        else
                        {
                            diferencia = posMaxVentB - i;
                        }
                    }
                    else if (posMaxVentA != -1 & posMaxVentB == -1)
                    {
                        diferencia = i - posMaxVentA;
                    }
                    else if (posMaxVentA == -1 & posMaxVentB != -1) // La mas usual
                    {
                        diferencia = posMaxVentB - i;
                    }
                    else
                    {
                        diferencia = 6;
                    }

                    // El año almacenado en la lista es el i, no el i+1
                    int añoBuscar;
                    if (fechaMin.Month > _datos.mesInicio - 1)
                    {
                        añoBuscar = fechaMin.Year;
                    }
                    else
                    {
                        añoBuscar = fechaMin.Year - 1;
                    }

                    // Posicion del año en la lista.
                    int posTipo = Array.BinarySearch(_dataSet._AportacionNatAnualOrdAños.año, añoBuscar);

                    // Mirar el tipo del año señalado en la lista.
                    if (_dataSet._AportacionNatAnualOrdAños.tipo[posTipo] == TIPOAÑO.HUMEDO)
                    {
                        acH = acH + diferencia;
                    }
                    else if (_dataSet._AportacionNatAnualOrdAños.tipo[posTipo] == TIPOAÑO.MEDIO)
                    {
                        acM = acM + diferencia;
                    }
                    else
                    {
                        acS = acS + diferencia;
                    }
                } // Encontrado maximo
            }

            acH = acH / nAñosH;
            acM = acM / nAñosM;
            acS = acS / nAñosS;
            acH = (float)(1d - 1d / 6d * acH);
            acM = (float)(1d - 1d / 6d * acM);
            acS = (float)(1d - 1d / 6d * acS);
            Array.Resize(ref _dataSet._IndicesHabituales[6].valor, 4);
            _dataSet._IndicesHabituales[6].valor[0] = acH;
            _dataSet._IndicesHabituales[6].valor[1] = acM;
            _dataSet._IndicesHabituales[6].valor[2] = acS;
            _dataSet._IndicesHabituales[6].calculado = true;
            // Me._dataSet._IndicesHabituales(6).invertido = False
            _dataSet._IndicesHabituales[6].valor[3] = (float)(0.25d * acH + 0.5d * acM + 0.25d * acS);

            // +++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++++ Calculo de indice IAG +++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++
            // ReDim Me._dataSet._IndiceIAG(3)
            // ' Para Humedo, Medio y Seco
            // For i = 0 To 2
            // Me._dataSet._IndiceIAG(i) = (Pow(Me._dataSet._IndicesHabituales(0).valor(i) + Me._dataSet._IndicesHabituales(1).valor(i) + Me._dataSet._IndicesHabituales(3).valor(i) + Me._dataSet._IndicesHabituales(5).valor(i) + Me._dataSet._IndicesHabituales(6).valor(i), 2) - _
            // (Pow(Me._dataSet._IndicesHabituales(0).valor(i), 2) + Pow(Me._dataSet._IndicesHabituales(1).valor(i), 2) + Pow(Me._dataSet._IndicesHabituales(3).valor(i), 2) + Pow(Me._dataSet._IndicesHabituales(5).valor(i), 2) + Pow(Me._dataSet._IndicesHabituales(6).valor(i), 2))) / 20
            // Next

            // Me._dataSet._IndiceIAG(3) = (Me._dataSet._IndiceIAG(0) + Me._dataSet._IndiceIAG(1) + Me._dataSet._IndiceIAG(2)) / 3

        }

        public void CalcularIndiceHabitual_I3()
        {
            int i;
            int pos10;
            int pos90;
            var Q10 = new float[3];
            var Q90 = new float[3];
            var acum = new float[3];
            var naños = new int[3];
            int nCalInvertidosH = 0;
            int nCalInvertidosM = 0;
            int nCalInvertidosS = 0;
            if (_dataSet._IndicesHabituales is null)
            {
                _dataSet._IndicesHabituales = new Indices[7];
            }
            // Redimension del indice
            _dataSet._IndicesHabituales[2].valor = new float[4];
            _dataSet._IndicesHabituales[2].invertido = new bool[4];
            _dataSet._IndicesHabituales[2].indeterminacion = new bool[4];
            pos10 = 0;
            while (_dataSet._TablaCQCNat.pe[pos10] < 10f)
                pos10 = pos10 + 1;
            pos90 = 0;
            while (_dataSet._TablaCQCNat.pe[pos90] < 90f)
                pos90 = pos90 + 1;
            for (i = 0; i <= 2; i++)
            {
                acum[i] = 0f;
                naños[i] = 0;
            }

            for (i = 0; i <= 3; i++)
            {
                _dataSet._IndicesHabituales[2].invertido[i] = false;
                _dataSet._IndicesHabituales[2].invertido[i] = false;
            }

            // ERROR DOC 27/08/09 - CA 001
            // -- Fallo en caso 2: Guadarrama
            // ----------------------------
            // For i = 0 To Me._datos.SerieAltDiaria.nAños - 1
            var loopTo = _dataSet._AportacionNatAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    Q10[0] = (10f - _dataSet._TablaCQCNat.pe[pos10 - 1]) / (_dataSet._TablaCQCNat.pe[pos10] - _dataSet._TablaCQCNat.pe[pos10 - 1]) * (_dataSet._TablaCQCNat.caudales[i][pos10] - _dataSet._TablaCQCNat.caudales[i][pos10 - 1]) + _dataSet._TablaCQCNat.caudales[i][pos10 - 1];


                    Q90[0] = (90f - _dataSet._TablaCQCNat.pe[pos90 - 1]) / (_dataSet._TablaCQCNat.pe[pos90] - _dataSet._TablaCQCNat.pe[pos90 - 1]) * (_dataSet._TablaCQCNat.caudales[i][pos90] - _dataSet._TablaCQCNat.caudales[i][pos90 - 1]) + _dataSet._TablaCQCNat.caudales[i][pos90 - 1];


                    Q10[1] = (10f - _dataSet._TablaCQCAlt.pe[pos10 - 1]) / (_dataSet._TablaCQCAlt.pe[pos10] - _dataSet._TablaCQCAlt.pe[pos10 - 1]) * (_dataSet._TablaCQCAlt.caudales[i][pos10] - _dataSet._TablaCQCAlt.caudales[i][pos10 - 1]) + _dataSet._TablaCQCAlt.caudales[i][pos10 - 1];


                    Q90[1] = (90f - _dataSet._TablaCQCAlt.pe[pos90 - 1]) / (_dataSet._TablaCQCAlt.pe[pos90] - _dataSet._TablaCQCAlt.pe[pos90 - 1]) * (_dataSet._TablaCQCAlt.caudales[i][pos90] - _dataSet._TablaCQCAlt.caudales[i][pos90 - 1]) + _dataSet._TablaCQCAlt.caudales[i][pos90 - 1];


                    if (Q10[0] - Q90[0] == 0f & Q10[1] - Q90[1] == 0f)
                    {
                        _dataSet._IndicesHabituales[2].indeterminacion[0] = true;
                        acum[0] = acum[0] + 1f;
                    }
                    else if (Q10[0] - Q90[0] == 0f & Q10[1] - Q90[1] != 0f)
                    {
                        _dataSet._IndicesHabituales[2].indeterminacion[0] = true;
                        acum[0] = acum[0] + 0f;
                    }
                    else if (Q10[0] - Q90[0] < Q10[1] - Q90[1])
                    {
                        _dataSet._IndicesHabituales[2].invertido[0] = true;
                        nCalInvertidosH += 1;
                        acum[0] = acum[0] + (Q10[0] - Q90[0]) / (Q10[1] - Q90[1]);
                    }
                    else
                    {
                        acum[0] = acum[0] + (Q10[1] - Q90[1]) / (Q10[0] - Q90[0]);
                    }

                    naños[0] = naños[0] + 1;
                }
                else if (_dataSet._AportacionNatAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                {
                    Q10[0] = (10f - _dataSet._TablaCQCNat.pe[pos10 - 1]) / (_dataSet._TablaCQCNat.pe[pos10] - _dataSet._TablaCQCNat.pe[pos10 - 1]) * (_dataSet._TablaCQCNat.caudales[i][pos10] - _dataSet._TablaCQCNat.caudales[i][pos10 - 1]) + _dataSet._TablaCQCNat.caudales[i][pos10 - 1];


                    Q90[0] = (90f - _dataSet._TablaCQCNat.pe[pos90 - 1]) / (_dataSet._TablaCQCNat.pe[pos90] - _dataSet._TablaCQCNat.pe[pos90 - 1]) * (_dataSet._TablaCQCNat.caudales[i][pos90] - _dataSet._TablaCQCNat.caudales[i][pos90 - 1]) + _dataSet._TablaCQCNat.caudales[i][pos90 - 1];


                    Q10[1] = (10f - _dataSet._TablaCQCAlt.pe[pos10 - 1]) / (_dataSet._TablaCQCAlt.pe[pos10] - _dataSet._TablaCQCAlt.pe[pos10 - 1]) * (_dataSet._TablaCQCAlt.caudales[i][pos10] - _dataSet._TablaCQCAlt.caudales[i][pos10 - 1]) + _dataSet._TablaCQCAlt.caudales[i][pos10 - 1];


                    Q90[1] = (90f - _dataSet._TablaCQCAlt.pe[pos90 - 1]) / (_dataSet._TablaCQCAlt.pe[pos90] - _dataSet._TablaCQCAlt.pe[pos90 - 1]) * (_dataSet._TablaCQCAlt.caudales[i][pos90] - _dataSet._TablaCQCAlt.caudales[i][pos90 - 1]) + _dataSet._TablaCQCAlt.caudales[i][pos90 - 1];


                    if (Q10[0] - Q90[0] == 0f & Q10[1] - Q90[1] == 0f)
                    {
                        _dataSet._IndicesHabituales[2].indeterminacion[1] = true;
                        acum[1] = acum[1] + 1f;
                    }
                    else if (Q10[0] - Q90[0] == 0f & Q10[1] - Q90[1] != 0f)
                    {
                        _dataSet._IndicesHabituales[2].indeterminacion[1] = true;
                        acum[1] = acum[1] + 0f;
                    }
                    else if (Q10[0] - Q90[0] < Q10[1] - Q90[1])
                    {
                        _dataSet._IndicesHabituales[2].invertido[1] = true;
                        nCalInvertidosM += 1;
                        acum[1] = acum[1] + (Q10[0] - Q90[0]) / (Q10[1] - Q90[1]);
                    }
                    else
                    {
                        acum[1] = acum[1] + (Q10[1] - Q90[1]) / (Q10[0] - Q90[0]);
                    }

                    naños[1] = naños[1] + 1;
                }
                else
                {
                    Q10[0] = (10f - _dataSet._TablaCQCNat.pe[pos10 - 1]) / (_dataSet._TablaCQCNat.pe[pos10] - _dataSet._TablaCQCNat.pe[pos10 - 1]) * (_dataSet._TablaCQCNat.caudales[i][pos10] - _dataSet._TablaCQCNat.caudales[i][pos10 - 1]) + _dataSet._TablaCQCNat.caudales[i][pos10 - 1];


                    Q90[0] = (90f - _dataSet._TablaCQCNat.pe[pos90 - 1]) / (_dataSet._TablaCQCNat.pe[pos90] - _dataSet._TablaCQCNat.pe[pos90 - 1]) * (_dataSet._TablaCQCNat.caudales[i][pos90] - _dataSet._TablaCQCNat.caudales[i][pos90 - 1]) + _dataSet._TablaCQCNat.caudales[i][pos90 - 1];


                    Q10[1] = (10f - _dataSet._TablaCQCAlt.pe[pos10 - 1]) / (_dataSet._TablaCQCAlt.pe[pos10] - _dataSet._TablaCQCAlt.pe[pos10 - 1]) * (_dataSet._TablaCQCAlt.caudales[i][pos10] - _dataSet._TablaCQCAlt.caudales[i][pos10 - 1]) + _dataSet._TablaCQCAlt.caudales[i][pos10 - 1];


                    Q90[1] = (90f - _dataSet._TablaCQCAlt.pe[pos90 - 1]) / (_dataSet._TablaCQCAlt.pe[pos90] - _dataSet._TablaCQCAlt.pe[pos90 - 1]) * (_dataSet._TablaCQCAlt.caudales[i][pos90] - _dataSet._TablaCQCAlt.caudales[i][pos90 - 1]) + _dataSet._TablaCQCAlt.caudales[i][pos90 - 1];


                    if (Q10[0] - Q90[0] == 0f & Q10[1] - Q90[1] == 0f)
                    {
                        _dataSet._IndicesHabituales[2].indeterminacion[2] = true;
                        acum[2] = acum[2] + 1f;
                    }
                    else if (Q10[0] - Q90[0] == 0f & Q10[1] - Q90[1] != 0f)
                    {
                        _dataSet._IndicesHabituales[2].indeterminacion[2] = true;
                        acum[2] = acum[2] + 0f;
                    }
                    else if (Q10[0] - Q90[0] < Q10[1] - Q90[1])
                    {
                        _dataSet._IndicesHabituales[2].invertido[2] = true;
                        nCalInvertidosS += 1;
                        acum[2] = acum[2] + (Q10[0] - Q90[0]) / (Q10[1] - Q90[1]);
                    }
                    else
                    {
                        acum[2] = acum[2] + (Q10[1] - Q90[1]) / (Q10[0] - Q90[0]);
                    }
                    // ¿Porque no se hacia como el resto?
                    // ERROR DOC 27/08/09 - CA 003
                    // -- Fallo en caso 1: La cierva
                    // ----------------------------
                    naños[2] = naños[2] + 1;
                }
            }

            // Ver si es invertido cada uno de los indices
            if (nCalInvertidosH / (double)naños[0] > 0.5d)
            {
                _dataSet._IndicesHabituales[2].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[2].invertido[0] = false;
            }

            if (nCalInvertidosM / (double)naños[1] > 0.5d)
            {
                _dataSet._IndicesHabituales[2].invertido[1] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[2].invertido[1] = false;
            }

            if (nCalInvertidosS / (double)naños[2] > 0.5d)
            {
                _dataSet._IndicesHabituales[2].invertido[2] = true;
            }
            else
            {
                _dataSet._IndicesHabituales[2].invertido[2] = false;
            }

            _dataSet._IndicesHabituales[2].calculado = true;
            // Generar los i3 finales
            for (i = 0; i <= 2; i++)
                _dataSet._IndicesHabituales[2].valor[i] = (float)(1d / naños[i] * acum[i]);
            _dataSet._IndicesHabituales[2].invertido[3] = _dataSet._IndicesHabituales[2].invertido[0] | _dataSet._IndicesHabituales[2].invertido[1] | _dataSet._IndicesHabituales[2].invertido[2];

            _dataSet._IndicesHabituales[2].indeterminacion[3] = _dataSet._IndicesHabituales[2].indeterminacion[0] | _dataSet._IndicesHabituales[2].indeterminacion[1] | _dataSet._IndicesHabituales[2].indeterminacion[2];

            _dataSet._IndicesHabituales[2].valor[3] = (_dataSet._IndicesHabituales[2].valor[0] + _dataSet._IndicesHabituales[2].valor[1] + _dataSet._IndicesHabituales[2].valor[2]) / 3f;
        }
        /// <summary>
        /// Indices M1,M2,M3,V1,V2,V3
        /// </summary>
        /// <remarks>Informe 7c</remarks>
        public void CalcularIndicesHabitualesAgregados()
        {
            int i, j;
            _dataSet._IndicesHabitualesAgregados = new Indices[7];

            // +++++++++++++++++++++++++++++++
            // ++++ Magnitud +++++++++++++++++
            // +++++++++++++++++++++++++++++++

            // +++++++++++++++++++  M1  ++++++++++++++++++++
            // +++ Magnitud de las aportaciones anuales ++++
            // +++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._IndicesHabitualesAgregados[0].valor = new float[1];
            _dataSet._IndicesHabitualesAgregados[0].invertido = new bool[1];
            _dataSet._IndicesHabitualesAgregados[0].indeterminacion = new bool[1];
            if (_dataSet._HabMagnitudAlt[3] == 0f & _dataSet._HabMagnitudNatReducido == 0f)
            {
                _dataSet._IndicesHabitualesAgregados[0].indeterminacion[0] = true;
                _dataSet._IndicesHabitualesAgregados[0].valor[0] = 1f;
            }
            else if (_dataSet._HabMagnitudAlt[3] != 0f & _dataSet._HabMagnitudNatReducido == 0f)
            {
                _dataSet._IndicesHabitualesAgregados[0].indeterminacion[0] = true;
                _dataSet._IndicesHabitualesAgregados[0].valor[0] = 0f;
            }
            else if (_dataSet._HabMagnitudAlt[3] > _dataSet._HabMagnitudNatReducido)
            {
                _dataSet._IndicesHabitualesAgregados[0].invertido[0] = true;
                _dataSet._IndicesHabitualesAgregados[0].valor[0] = _dataSet._HabMagnitudNatReducido / _dataSet._HabMagnitudAlt[3];
            }
            else
            {
                _dataSet._IndicesHabitualesAgregados[0].valor[0] = _dataSet._HabMagnitudAlt[3] / _dataSet._HabMagnitudNatReducido;
            }

            _dataSet._IndicesHabitualesAgregados[0].calculado = true;

            // +++++++++++++++++++++ M2+M3 ++++++++++++++++++++
            // +++ Magnitud de las aportaciones mensuales +++++
            // +++ Magnitud de las aportaciones por mes   +++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._IndicesHabitualesAgregados[1].valor = new float[1];
            _dataSet._IndicesHabitualesAgregados[1].invertido = new bool[1];
            _dataSet._IndicesHabitualesAgregados[1].indeterminacion = new bool[1];
            _dataSet._IndiceM3Agregados.valor = new float[12];
            _dataSet._IndiceM3Agregados.invertido = new bool[12];
            _dataSet._IndiceM3Agregados.indeterminacion = new bool[12];
            for (i = 0; i <= 11; i++)
            {
                _dataSet._IndiceM3Agregados.invertido[i] = false;
                _dataSet._IndiceM3Agregados.indeterminacion[i] = false;
            }

            float acum;
            float[] mediasMesNat;
            mediasMesNat = new float[12];
            float[] mediasMesAlt;
            mediasMesAlt = new float[12];
            acum = 0f;
            var loopTo = _dataSet._AportacionNatAnual.aportacion.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                for (j = 0; j <= 11; j++)
                    mediasMesNat[j] = mediasMesNat[j] + _dataSet._AportacionNatMen.aportacion[i * 12 + j];
            }

            for (i = 0; i <= 11; i++)
                mediasMesNat[i] = mediasMesNat[i] / 12f;
            var loopTo1 = _dataSet._AportacionAltAnual.aportacion.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                for (j = 0; j <= 11; j++)
                    mediasMesAlt[j] = mediasMesAlt[j] + _dataSet._AportacionAltMen.aportacion[i * 12 + j];
            }

            for (i = 0; i <= 11; i++)
                mediasMesAlt[i] = mediasMesAlt[i] / 12f;

            // Calculo del M3
            for (i = 0; i <= 11; i++)
            {
                if (mediasMesAlt[i] == 0f & mediasMesNat[i] == 0f)
                {
                    _dataSet._IndiceM3Agregados.indeterminacion[i] = true;
                    _dataSet._IndiceM3Agregados.valor[i] = 1f;
                }
                else if (mediasMesAlt[i] != 0f & mediasMesNat[i] == 0f)
                {
                    _dataSet._IndiceM3Agregados.indeterminacion[i] = true;
                    _dataSet._IndiceM3Agregados.valor[i] = 0f;
                }
                else if (mediasMesAlt[i] > mediasMesNat[i])
                {
                    _dataSet._IndiceM3Agregados.valor[i] = mediasMesNat[i] / mediasMesAlt[i];
                    _dataSet._IndiceM3Agregados.invertido[i] = true;
                }
                else
                {
                    _dataSet._IndiceM3Agregados.valor[i] = mediasMesAlt[i] / mediasMesNat[i];
                }
            }

            // Calculo del M2
            int nCalInvertidos = 0;
            for (i = 0; i <= 11; i++)
            {
                if (mediasMesAlt[i] == 0f & mediasMesNat[i] == 0f)
                {
                    _dataSet._IndicesHabitualesAgregados[1].indeterminacion[0] = true;
                    acum = acum + 1f;
                }
                else if (mediasMesAlt[i] != 0f & mediasMesNat[i] == 0f)
                {
                    _dataSet._IndicesHabitualesAgregados[1].indeterminacion[0] = true;
                    acum = acum + 0f;
                }
                else if (mediasMesAlt[i] > mediasMesNat[i])
                {
                    // Me._dataSet._IndicesHabitualesAgregados(1).invertido(0) = True
                    nCalInvertidos += 1;
                    acum = acum + mediasMesNat[i] / mediasMesAlt[i];
                }
                else
                {
                    acum = acum + mediasMesAlt[i] / mediasMesNat[i];
                }
            }

            // Ver si es invertido cada uno de los indices
            if (nCalInvertidos / 12d > 0.5d)
            {
                _dataSet._IndicesHabitualesAgregados[1].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesHabitualesAgregados[1].invertido[0] = false;
            }

            _dataSet._IndicesHabitualesAgregados[1].valor[0] = acum / 12f;
            _dataSet._IndicesHabitualesAgregados[1].calculado = true;


            // ++++++++++++++++++++++++++++++
            // ++++++ Variabilidad ++++++++++
            // ++++++++++++++++++++++++++++++

            // +++++++++++++++++++ V1 +++++++++++++++++++++++++
            // +++ Variabilidad de las aportaciones anuales +++
            // ++++++++++++++++++++++++++++++++++++++++++++++++

            _dataSet._IndicesHabitualesAgregados[2].valor = new float[1];
            _dataSet._IndicesHabitualesAgregados[2].invertido = new bool[1];
            _dataSet._IndicesHabitualesAgregados[2].indeterminacion = new bool[1];
            float aux1, aux2;
            float CVNat, CVAlt;
            int naños = _dataSet._AportacionNatAnual.aportacion.Length;
            aux1 = 0f;
            aux2 = 0f;
            CVNat = 0f;
            var loopTo2 = naños - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                aux1 = (float)(aux1 + Math.Pow(_dataSet._AportacionNatAnual.aportacion[i], 2d));
                aux2 = aux2 + _dataSet._AportacionNatAnual.aportacion[i];
            }

            CVNat = (float)((naños * aux1 - Math.Pow(aux2, 2d)) / (naños * (naños - 1)));
            CVNat = (float)Math.Sqrt(CVNat);
            naños = _dataSet._AportacionAltAnual.aportacion.Length;
            aux1 = 0f;
            aux2 = 0f;
            CVAlt = 0f;
            var loopTo3 = naños - 1;
            for (i = 0; i <= loopTo3; i++)
            {
                aux1 = (float)(aux1 + Math.Pow(_dataSet._AportacionAltAnual.aportacion[i], 2d));
                aux2 = aux2 + _dataSet._AportacionAltAnual.aportacion[i];
            }

            CVAlt = (float)((naños * aux1 - Math.Pow(aux2, 2d)) / (naños * (naños - 1)));
            CVAlt = (float)Math.Sqrt(CVAlt);
            if (CVAlt == 0f & CVNat == 0f)
            {
                _dataSet._IndicesHabitualesAgregados[2].indeterminacion[0] = true;
                _dataSet._IndicesHabitualesAgregados[2].valor[0] = 1f;
            }
            else if (CVAlt != 0f & CVNat == 0f)
            {
                _dataSet._IndicesHabitualesAgregados[2].indeterminacion[0] = true;
                _dataSet._IndicesHabitualesAgregados[2].valor[0] = 0f;
            }
            else if (CVAlt > CVNat)
            {
                _dataSet._IndicesHabitualesAgregados[2].invertido[0] = true;
                _dataSet._IndicesHabitualesAgregados[2].valor[0] = CVNat / CVAlt;
            }
            else
            {
                _dataSet._IndicesHabitualesAgregados[2].valor[0] = CVAlt / CVNat;
            }

            _dataSet._IndicesHabitualesAgregados[2].calculado = true;

            // +++++++++++++++++++++ V2+ V3 +++++++++++++++++++++
            // +++ Variabilidad de las aportaciones mensuales +++
            // +++ Variabilidad de las aportaciones por mes   +++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._IndicesHabitualesAgregados[3].valor = new float[1];
            _dataSet._IndicesHabitualesAgregados[3].invertido = new bool[1];
            _dataSet._IndicesHabitualesAgregados[3].indeterminacion = new bool[1];
            _dataSet._IndiceV3Agregados.valor = new float[12];
            _dataSet._IndiceV3Agregados.invertido = new bool[12];
            _dataSet._IndiceV3Agregados.indeterminacion = new bool[12];
            for (i = 0; i <= 11; i++)
            {
                _dataSet._IndiceV3Agregados.invertido[i] = false;
                _dataSet._IndiceV3Agregados.indeterminacion[i] = false;
            }

            var desviacionMesNat = new float[12];
            var desviacionMesAlt = new float[12];
            var CVMesNat = new float[12];
            var CVMesAlt = new float[12];
            var auxLista1 = new float[12];
            var auxLista2 = new float[12];
            for (i = 0; i <= 11; i++)
            {
                auxLista1[i] = 0f;
                auxLista2[i] = 0f;
            }

            naños = _dataSet._AportacionNatAnual.aportacion.Length;
            var loopTo4 = naños - 1;
            for (i = 0; i <= loopTo4; i++)
            {
                for (j = 0; j <= 11; j++)
                {
                    auxLista1[j] = (float)(auxLista1[j] + Math.Pow(_dataSet._AportacionNatMen.aportacion[i * 12 + j], 2d));
                    auxLista2[j] = auxLista2[j] + _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                }
            }

            for (i = 0; i <= 11; i++)
            {
                desviacionMesNat[i] = (float)((naños * auxLista1[i] + Math.Pow(auxLista2[i], 2d)) / (naños * (naños - 1)));
                desviacionMesNat[i] = (float)Math.Sqrt(desviacionMesNat[i]);
            }

            for (i = 0; i <= 11; i++)
            {
                auxLista1[i] = 0f;
                auxLista2[i] = 0f;
            }

            naños = _dataSet._AportacionAltAnual.aportacion.Length;
            var loopTo5 = naños - 1;
            for (i = 0; i <= loopTo5; i++)
            {
                for (j = 0; j <= 11; j++)
                {
                    auxLista1[j] = (float)(auxLista1[j] + Math.Pow(_dataSet._AportacionAltMen.aportacion[i * 12 + j], 2d));
                    auxLista2[j] = auxLista2[j] + _dataSet._AportacionAltMen.aportacion[i * 12 + j];
                }
            }

            for (i = 0; i <= 11; i++)
            {
                desviacionMesAlt[i] = (float)((naños * auxLista1[i] + Math.Pow(auxLista2[i], 2d)) / (naños * (naños - 1)));
                desviacionMesAlt[i] = (float)Math.Sqrt(desviacionMesAlt[i]);
            }

            for (i = 0; i <= 11; i++)
            {
                CVMesNat[i] = desviacionMesNat[i] / mediasMesNat[i];
                CVMesAlt[i] = desviacionMesAlt[i] / mediasMesAlt[i];
            }

            // Calculo de V3
            for (i = 0; i <= 11; i++)
            {
                if (CVMesAlt[i] == 0f & CVMesNat[i] == 0f)
                {
                    _dataSet._IndiceV3Agregados.indeterminacion[i] = true;
                    _dataSet._IndiceV3Agregados.valor[i] = 1f;
                }
                else if (CVMesAlt[i] != 0f & CVMesNat[i] == 0f)
                {
                    _dataSet._IndiceV3Agregados.indeterminacion[i] = true;
                    _dataSet._IndiceV3Agregados.valor[i] = 0f;
                }
                else if (CVMesAlt[i] > CVMesNat[i])
                {
                    _dataSet._IndiceV3Agregados.invertido[i] = true;
                    _dataSet._IndiceV3Agregados.valor[i] = CVMesNat[i] / CVMesAlt[i];
                }
                else
                {
                    _dataSet._IndiceV3Agregados.valor[i] = CVMesAlt[i] / CVMesNat[i];
                }
            }

            // Calculo del V2
            acum = 0f;
            nCalInvertidos = 0;
            for (i = 0; i <= 11; i++)
            {
                if (CVMesAlt[i] == 0f & CVMesNat[i] == 0f)
                {
                    _dataSet._IndicesHabitualesAgregados[3].indeterminacion[0] = true;
                    acum = acum + 1f;
                }
                else if (CVMesAlt[i] != 0f & CVMesNat[i] == 0f)
                {
                    _dataSet._IndicesHabitualesAgregados[3].indeterminacion[0] = true;
                    acum = acum + 0f;
                }
                else if (CVMesAlt[i] > CVMesNat[i])
                {
                    _dataSet._IndicesHabitualesAgregados[3].invertido[0] = true;
                    nCalInvertidos += 1;
                    acum = acum + CVMesNat[i] / CVMesAlt[i];
                }
                else
                {
                    acum = acum + CVMesAlt[i] / CVMesNat[i];
                }
            }

            // Ver si es invertido cada uno de los indices
            if (nCalInvertidos / 12d > 0.5d)
            {
                _dataSet._IndicesHabitualesAgregados[3].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesHabitualesAgregados[3].invertido[0] = false;
            }

            _dataSet._IndicesHabitualesAgregados[3].valor[0] = acum / 12f;
            _dataSet._IndicesHabitualesAgregados[3].calculado = true;

            // ++++  V4 ++++++++
            _dataSet._IndicesHabitualesAgregados[4].valor = new float[1];
            _dataSet._IndicesHabitualesAgregados[4].invertido = new bool[1];
            _dataSet._IndicesHabitualesAgregados[4].indeterminacion = new bool[1];
            if (_dataSet._HabVariabilidadAlt[3] == 0f & _dataSet._HabVariabilidadNatReducido == 0f)
            {
                _dataSet._IndicesHabitualesAgregados[4].indeterminacion[0] = true;
                _dataSet._IndicesHabitualesAgregados[4].valor[0] = 1f;
            }
            else if (_dataSet._HabVariabilidadAlt[3] != 0f & _dataSet._HabVariabilidadNatReducido == 0f)
            {
                _dataSet._IndicesHabitualesAgregados[4].indeterminacion[0] = true;
                _dataSet._IndicesHabitualesAgregados[4].valor[0] = 0f;
            }
            else if (_dataSet._HabVariabilidadAlt[3] > _dataSet._HabVariabilidadNatReducido)
            {
                _dataSet._IndicesHabitualesAgregados[4].invertido[0] = true;
                _dataSet._IndicesHabitualesAgregados[4].valor[0] = _dataSet._HabVariabilidadNatReducido / _dataSet._HabVariabilidadAlt[3];
            }
            else
            {
                _dataSet._IndicesHabitualesAgregados[4].valor[0] = _dataSet._HabVariabilidadAlt[3] / _dataSet._HabVariabilidadNatReducido;
            }

            _dataSet._IndicesHabitualesAgregados[4].calculado = true;

            // +++++++++++++++++++++++++++++++++
            // +++++++++ Estacionalidad ++++++++
            // +++++++++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++ E1 (Estacionalidad de Maximos ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._IndicesHabitualesAgregados[5].valor = new float[1];
            _dataSet._IndicesHabitualesAgregados[5].invertido = new bool[1];
            _dataSet._IndicesHabitualesAgregados[5].indeterminacion = new bool[1];
            var desfase = default(float);
            var desfase2 = default(float);
            var desfaseDef = default(float);

            int maxMonthRN=-1;
            float maxData = 0;

            for (i = 0; i <= 11; i++)
                if(_dataSet._HabEstacionalidadMensualNat[0].ndias[i]>maxData)
                {
                    maxData = _dataSet._HabEstacionalidadMensualNat[0].ndias[i];
                    maxMonthRN = i;
                }

            int maxMonthRA=-1;
            maxData = 0;

            for (i = 0; i <= 11; i++)
                if (_dataSet._HabEstacionalidadMensualAlt[0].ndias[i] > maxData)
                {
                    maxData = _dataSet._HabEstacionalidadMensualAlt[0].ndias[i];
                    maxMonthRA = i;
                }

            if (maxMonthRN == maxMonthRA) desfase = 0;
            else if (maxMonthRN > maxMonthRA) desfase = maxMonthRN - maxMonthRA;
            else desfase = maxMonthRA - maxMonthRN;

            desfase2 = 12 - desfase;

            if (desfase == desfase2)
            {
                desfaseDef = desfase;
            }
            else if (desfase > desfase2)
            {
                if (desfase > 6)
                {
                    desfaseDef = desfase2;
                }
                else
                {
                    desfaseDef = desfase;
                }
            }
            else
            {
                if (desfase2 > 6)
                {
                    desfaseDef = desfase;
                }
                else
                {
                    desfaseDef = desfase2;
                }
            }
            

            desfase = 1f - desfaseDef / 6f;
            desfase = Math.Abs(desfase);

            // If (desfase = 0) Then
            // desfase = 1
            // ElseIf (desfase = 1) Then
            // desfase = 0.83
            // ElseIf (desfase = 2) Then
            // desfase = 0.67
            // ElseIf (desfase = 3) Then
            // desfase = 0.5
            // ElseIf (desfase = 4) Then
            // desfase = 0.33
            // ElseIf (desfase = 5) Then
            // desfase = 0.17
            // Else
            // desfase = 0
            // End If



            _dataSet._IndicesHabitualesAgregados[5].valor[0] = desfase;
            _dataSet._IndicesHabitualesAgregados[5].calculado = true;

            // +++++++++++++++++++++++++++++++++
            // ++++++++ E2 (Estacionalidad++++++++++
            _dataSet._IndicesHabitualesAgregados[6].valor = new float[1];
            _dataSet._IndicesHabitualesAgregados[6].invertido = new bool[1];
            _dataSet._IndicesHabitualesAgregados[6].indeterminacion = new bool[1];


            int minMonthRN = -1;
            float minData = 0;

            for (i = 0; i <= 11; i++)
                if (_dataSet._HabEstacionalidadMensualNat[1].ndias[i] > minData)
                {
                    minData = _dataSet._HabEstacionalidadMensualNat[1].ndias[i];
                    minMonthRN = i;
                }

            int minMonthRA = -1;
            minData = 0;

            for (i = 0; i <= 11; i++)
                if (_dataSet._HabEstacionalidadMensualAlt[1].ndias[i] > minData)
                {
                    minData = _dataSet._HabEstacionalidadMensualAlt[1].ndias[i];
                    minMonthRA = i;
                }

            if (minMonthRN == minMonthRA) desfase = 0;
            else if (minMonthRN > minMonthRA) desfase = minMonthRN - minMonthRA;
            else desfase = minMonthRA - minMonthRN;

            if (desfase == desfase2)
                {
                    desfaseDef = desfase;
                }
                else if (desfase > desfase2)
                {
                    if (desfase > 6)
                    {
                        desfaseDef = desfase2;
                    }
                    else
                    {
                        desfaseDef = desfase;
                    }
                }
                else
                {
                    if (desfase2 > 6)
                    {
                        desfaseDef = desfase;
                    }
                    else
                    {
                        desfaseDef = desfase2;
                    }
                }

                desfase = 1 - (desfaseDef / 6);
            desfase = Math.Abs(desfase);

            // If (desfase = 0) Then
            // desfase = 1
            // ElseIf (desfase = 1) Then
            // desfase = 0.83
            // ElseIf (desfase = 2) Then
            // desfase = 0.67
            // ElseIf (desfase = 3) Then
            // desfase = 0.5
            // ElseIf (desfase = 4) Then
            // desfase = 0.33
            // ElseIf (desfase = 5) Then
            // desfase = 0.17
            // Else
            // desfase = 0
            // End If

            _dataSet._IndicesHabitualesAgregados[6].valor[0] = desfase;
            _dataSet._IndicesHabitualesAgregados[6].calculado = true;
        }

        /// <summary>
        /// Indices en avenidas
        /// </summary>
        /// <remarks>Informe 7d</remarks>
        public void CalcularIndicesAvenidasCASO6()
        {
            // Casi todos los calculos se basan en parametros, por lo que apenas hay que hacer calculos salvo saber
            // que datos anteriores usar.

            _dataSet._IndicesAvenidas = new Indices[8];
            int i;

            // +++++++++++++++++++++++++++++++++++++
            // +++++++ IAH7 - I8 +++++++++++++++++++
            // +++ Magnitud de avenidas maximas ++++
            // +++++++++++++++++++++++++++++++++++++
            _dataSet._IndicesAvenidas[0].valor = new float[1];
            _dataSet._IndicesAvenidas[0].invertido = new bool[1];
            _dataSet._IndicesAvenidas[0].indeterminacion = new bool[1];
            for (i = 0; i <= 7; i++)
                _dataSet._IndicesAvenidas[i].calculado = false;
            _dataSet._IndicesAvenidas[0].invertido[0] = false;
            _dataSet._IndicesAvenidas[0].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[0].calculado = true;
            if (_dataSet._AveMagnitudAlt[0] == 0f & _dataSet._AveMagnitudNat[0] == 0f)
            {
                _dataSet._IndicesAvenidas[0].valor[0] = 1f;
                _dataSet._IndicesAvenidas[0].indeterminacion[0] = true;
            }
            else if (_dataSet._AveMagnitudAlt[0] != 0f & _dataSet._AveMagnitudNat[0] == 0f)
            {
                _dataSet._IndicesAvenidas[0].valor[0] = 0f;
                _dataSet._IndicesAvenidas[0].indeterminacion[0] = true;
            }
            else if (_dataSet._AveMagnitudAlt[0] > _dataSet._AveMagnitudNat[0])
            {
                _dataSet._IndicesAvenidas[0].valor[0] = _dataSet._AveMagnitudNat[0] / _dataSet._AveMagnitudAlt[0];
                _dataSet._IndicesAvenidas[0].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesAvenidas[0].valor[0] = _dataSet._AveMagnitudAlt[0] / _dataSet._AveMagnitudNat[0];
            }

            // +++++++++++++++++++++++++++++++++++++++++++++
            // +++++++ IAH 8 - I9 ++++++++++++++++++++++++++
            // +++ Magnitud de caudal generador de lecho +++
            // +++++++++++++++++++++++++++++++++++++++++++++
            _dataSet._IndicesAvenidas[1].valor = new float[1];
            _dataSet._IndicesAvenidas[1].invertido = new bool[1];
            _dataSet._IndicesAvenidas[1].indeterminacion = new bool[1];
            _dataSet._IndicesAvenidas[1].invertido[0] = false;
            _dataSet._IndicesAvenidas[1].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[1].calculado = true;

            // OJO que puede que no se calcule nada
            if (_dataSet._AveMagnitudNat[1] == -9999)
            {
                _dataSet._IndicesAvenidas[1].valor[0] = -9999;
                _dataSet._IndicesAvenidas[1].calculado = false;
            }
            else if (_dataSet._AveMagnitudAlt[1] == 0f & _dataSet._AveMagnitudNat[1] == 0f)
            {
                _dataSet._IndicesAvenidas[1].valor[0] = 1f;
                _dataSet._IndicesAvenidas[1].indeterminacion[0] = true;
            }
            else if (_dataSet._AveMagnitudAlt[1] != 0f & _dataSet._AveMagnitudNat[1] == 0f)
            {
                _dataSet._IndicesAvenidas[1].valor[0] = 0f;
                _dataSet._IndicesAvenidas[1].indeterminacion[0] = true;
            }
            else if (_dataSet._AveMagnitudAlt[1] > _dataSet._AveMagnitudNat[1])
            {
                _dataSet._IndicesAvenidas[1].valor[0] = (float)Math.Pow(_dataSet._AveMagnitudNat[1] / _dataSet._AveMagnitudAlt[1], 0.5d);
                _dataSet._IndicesAvenidas[1].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesAvenidas[1].valor[0] = (float)Math.Pow(_dataSet._AveMagnitudAlt[1] / _dataSet._AveMagnitudNat[1], 0.5d);
            }

            // +++++++++++++++++++++++++++++++
            // ++++++++ IAH 9 - I10 ++++++++++
            // +++++++++++++++++++++++++++++++
            // Frecuencia de caudal de conectividad

            // T[Qconec NAT] en ALT
            // ---------------------
            // T[Qconec NAT] en NAT

            _dataSet._IndicesAvenidas[2].valor = new float[1];
            _dataSet._IndicesAvenidas[2].invertido = new bool[1];
            _dataSet._IndicesAvenidas[2].indeterminacion = new bool[1];
            _dataSet._IndicesAvenidas[2].invertido[0] = false;
            _dataSet._IndicesAvenidas[2].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[2].calculado = true;
            if (_dataSet._Ave2TAlt == 0f & _dataSet._Ave2TNat == 0f)
            {
                _dataSet._IndicesAvenidas[2].valor[0] = 1f;
                _dataSet._IndicesAvenidas[2].indeterminacion[0] = true;
            }
            else if (_dataSet._Ave2TAlt != 0f & _dataSet._Ave2TNat == 0f)
            {
                _dataSet._IndicesAvenidas[2].valor[0] = 0f;
                _dataSet._IndicesAvenidas[2].indeterminacion[0] = true;
            }
            else if (_dataSet._Ave2TAlt > _dataSet._Ave2TNat)
            {
                _dataSet._IndicesAvenidas[2].valor[0] = _dataSet._Ave2TNat / _dataSet._Ave2TAlt;
                _dataSet._IndicesAvenidas[2].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesAvenidas[2].valor[0] = _dataSet._Ave2TAlt / _dataSet._Ave2TNat;
            }

            // +++++++++++++++++++++++
            // ++++++++ I11 ++++++++++
            // +++++++++++++++++++++++
            _dataSet._IndicesAvenidas[3].valor = new float[1];
            _dataSet._IndicesAvenidas[3].invertido = new bool[1];
            _dataSet._IndicesAvenidas[3].indeterminacion = new bool[1];
            _dataSet._IndicesAvenidas[3].invertido[0] = false;
            _dataSet._IndicesAvenidas[3].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[3].calculado = true;
            if (_dataSet._AveMagnitudAlt[3] == 0f & _dataSet._AveMagnitudNat[3] == 0f)
            {
                _dataSet._IndicesAvenidas[3].valor[0] = 1f;
                _dataSet._IndicesAvenidas[3].indeterminacion[0] = true;
            }
            else if (_dataSet._AveMagnitudAlt[3] != 0f & _dataSet._AveMagnitudNat[3] == 0f)
            {
                _dataSet._IndicesAvenidas[3].valor[0] = 0f;
                _dataSet._IndicesAvenidas[3].indeterminacion[0] = true;
            }
            else if (_dataSet._AveMagnitudAlt[3] > _dataSet._AveMagnitudNat[3])
            {
                _dataSet._IndicesAvenidas[3].valor[0] = _dataSet._AveMagnitudNat[3] / _dataSet._AveMagnitudAlt[3];
                _dataSet._IndicesAvenidas[3].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesAvenidas[3].valor[0] = _dataSet._AveMagnitudAlt[3] / _dataSet._AveMagnitudNat[3];
            }

            // +++++++++++++++++++++++
            // ++++++++ I12 ++++++++++
            // +++++++++++++++++++++++
            _dataSet._IndicesAvenidas[4].valor = new float[1];
            _dataSet._IndicesAvenidas[4].invertido = new bool[1];
            _dataSet._IndicesAvenidas[4].indeterminacion = new bool[1];
            _dataSet._IndicesAvenidas[4].invertido[0] = false;
            _dataSet._IndicesAvenidas[4].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[4].calculado = true;
            if (_dataSet._AveVariabilidadAlt[0] < 0f | _dataSet._AveVariabilidadNat[0] < 0f)
            {
                _dataSet._IndicesAvenidas[4].valor[0] = -9999;
                _dataSet._IndicesAvenidas[4].calculado = false;
            }
            else if (_dataSet._AveVariabilidadAlt[0] == 0f & _dataSet._AveVariabilidadNat[0] == 0f)
            {
                _dataSet._IndicesAvenidas[4].valor[0] = 1f;
                _dataSet._IndicesAvenidas[4].indeterminacion[0] = true;
            }
            else if (_dataSet._AveVariabilidadAlt[0] != 0f & _dataSet._AveVariabilidadNat[0] == 0f)
            {
                _dataSet._IndicesAvenidas[4].valor[0] = 0f;
                _dataSet._IndicesAvenidas[4].indeterminacion[0] = true;
            }
            else if (_dataSet._AveVariabilidadAlt[0] > _dataSet._AveVariabilidadNat[0])
            {
                _dataSet._IndicesAvenidas[4].valor[0] = _dataSet._AveVariabilidadNat[0] / _dataSet._AveVariabilidadAlt[0];
                _dataSet._IndicesAvenidas[4].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesAvenidas[4].valor[0] = _dataSet._AveVariabilidadAlt[0] / _dataSet._AveVariabilidadNat[0];
            }

            // +++++++++++++++++++++++
            // ++++++++ I13 ++++++++++
            // +++++++++++++++++++++++
            _dataSet._IndicesAvenidas[5].valor = new float[1];
            _dataSet._IndicesAvenidas[5].invertido = new bool[1];
            _dataSet._IndicesAvenidas[5].indeterminacion = new bool[1];
            _dataSet._IndicesAvenidas[5].invertido[0] = false;
            _dataSet._IndicesAvenidas[5].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[5].calculado = true;
            if (_dataSet._AveVariabilidadAlt[1] < 0f | _dataSet._AveVariabilidadNat[1] < 0f)
            {
                _dataSet._IndicesAvenidas[5].valor[0] = -9999;
                _dataSet._IndicesAvenidas[5].calculado = false;
            }
            else if (_dataSet._AveVariabilidadAlt[1] == 0f & _dataSet._AveVariabilidadNat[1] == 0f)
            {
                _dataSet._IndicesAvenidas[5].valor[0] = 1f;
                _dataSet._IndicesAvenidas[5].indeterminacion[0] = true;
            }
            else if (_dataSet._AveVariabilidadAlt[1] != 0f & _dataSet._AveVariabilidadNat[1] == 0f)
            {
                _dataSet._IndicesAvenidas[5].valor[0] = 0f;
                _dataSet._IndicesAvenidas[5].indeterminacion[0] = true;
            }
            else if (_dataSet._AveVariabilidadAlt[1] > _dataSet._AveVariabilidadNat[1])
            {
                _dataSet._IndicesAvenidas[5].valor[0] = _dataSet._AveVariabilidadNat[1] / _dataSet._AveVariabilidadAlt[1];
                _dataSet._IndicesAvenidas[5].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesAvenidas[5].valor[0] = _dataSet._AveVariabilidadAlt[1] / _dataSet._AveVariabilidadNat[1];
            }


            // +++++++++++++++++++++++
            // ++++++++ I15 ++++++++++
            // +++++++++++++++++++++++
            _dataSet._IndicesAvenidas[6].valor = new float[1];
            _dataSet._IndicesAvenidas[6].invertido = new bool[1];
            _dataSet._IndicesAvenidas[6].indeterminacion = new bool[1];
            _dataSet._IndicesAvenidas[6].invertido[0] = false;
            _dataSet._IndicesAvenidas[6].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[6].calculado = true;
            if (_dataSet._AveDuracionAlt == 0f & _dataSet._AveDuracionNat == 0f)
            {
                _dataSet._IndicesAvenidas[6].valor[0] = 1f;
                _dataSet._IndicesAvenidas[6].indeterminacion[0] = true;
            }
            else if (_dataSet._AveDuracionAlt != 0f & _dataSet._AveDuracionNat == 0f)
            {
                _dataSet._IndicesAvenidas[6].valor[0] = 0f;
                _dataSet._IndicesAvenidas[6].indeterminacion[0] = true;
            }
            else if (_dataSet._AveDuracionAlt > _dataSet._AveDuracionNat)
            {
                _dataSet._IndicesAvenidas[6].valor[0] = _dataSet._AveDuracionNat / _dataSet._AveDuracionAlt;
                _dataSet._IndicesAvenidas[6].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesAvenidas[6].valor[0] = _dataSet._AveDuracionAlt / _dataSet._AveDuracionNat;
            }

            // ++++++++++++++++++++++++++++++++++
            // ++++++++ IAH14 - I16 +++++++++++++
            // ++++++++++++++++++++++++++++++++++
            // ++ Nº medio de dias con Q > Q5% ++
            // ++++++++++++++++++++++++++++++++++
            // +++ Modificacion del 7/2/08: Cambiar umbral de 10 a 5
            // ++++++++++++++++++++++++++++++++++
            _dataSet._IndicesAvenidas[7].valor = new float[1];
            _dataSet._IndicesAvenidas[7].invertido = new bool[1];
            _dataSet._IndicesAvenidas[7].indeterminacion = new bool[1];
            _dataSet._IndicesAvenidas[7].invertido[0] = false;
            _dataSet._IndicesAvenidas[7].indeterminacion[0] = false;
            _dataSet._IndicesAvenidas[7].calculado = true;

            // +++++++++++++++++++++
            // Modificación Enero 08
            // +++++++++++++++++++++
            _dataSet._IndicesAvenidasI16Meses = new float[12];
            _dataSet._IndicesAvenidasI16MesesInversos = new float[12];
            float acum;
            acum = 0f;
            for (i = 0; i <= 11; i++)
            {
                // If (Me._dataSet._AveEstacionalidadAlt.ndias(i) = 0) And (Me._dataSet._AveEstacionalidadNat.ndias(i) = 0) Then
                // acum = acum + 1
                // Me._dataSet._IndicesAvenidas(7).indeterminacion(0) = True
                // 'ElseIf (Me._dataSet._AveEstacionalidadAlt.ndias(i) <> 0) And (Me._dataSet._AveEstacionalidadNat.ndias(i) = 0) Then
                // '    acum = acum + 0
                // '    Me._dataSet._IndicesAvenidas(7).indeterminacion(0) = True
                // 'ElseIf (Me._dataSet._AveEstacionalidadAlt.ndias(i) > Me._dataSet._AveEstacionalidadNat.ndias(i)) Then
                // 'acum = acum + Me._dataSet._AveEstacionalidadNat.ndias(i) / Me._dataSet._AveEstacionalidadAlt.ndias(i)
                // 'Me._dataSet._IndicesAvenidas(7).invertido(0) = True
                // Else
                // +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
                // +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // acum = acum + Me._dataSet._AveEstacionalidadAlt.ndias(i) / Me._dataSet._AveEstacionalidadNat.ndias(i)
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                float dif = Math.Abs(_dataSet._AveEstacionalidadNat.ndias[i] - _dataSet._AveEstacionalidadAlt.ndias[i]);
                if (_dataSet._AveEstacionalidadAlt.ndias[i] > _dataSet._AveEstacionalidadNat.ndias[i])
                {
                    _dataSet._IndicesAvenidasI16MesesInversos[i] = Conversions.ToSingle(true);
                }
                else
                {
                    _dataSet._IndicesAvenidasI16MesesInversos[i] = Conversions.ToSingle(false);
                }

                // Cambio Enero 08
                _dataSet._IndicesAvenidasI16Meses[i] = 0f;
                if (dif <= 5f)
                {
                    _dataSet._IndicesAvenidasI16Meses[i] = (5f - dif) / 5f;
                    acum = acum + (5f - dif) / 5f;
                }
                // End If
            }

            _dataSet._IndicesAvenidas[7].valor[0] = (float)(1d / 12d * acum);
        }

        public void CalcularIndicesSequiasCASO6()
        {
            _dataSet._IndicesSequias = new Indices[7];
            int i;

            // +++++++++++++++++++++
            // +++++++ I17 +++++++++
            // +++++++++++++++++++++
            _dataSet._IndicesSequias[0].valor = new float[1];
            _dataSet._IndicesSequias[0].invertido = new bool[1];
            _dataSet._IndicesSequias[0].indeterminacion = new bool[1];
            for (i = 0; i <= 6; i++)
                _dataSet._IndicesSequias[i].calculado = false;
            _dataSet._IndicesSequias[0].invertido[0] = false;
            _dataSet._IndicesSequias[0].indeterminacion[0] = false;
            _dataSet._IndicesSequias[0].calculado = true;
            if (_dataSet._SeqMagnitudAlt[0] == 0f & _dataSet._SeqMagnitudNat[0] == 0f)
            {
                _dataSet._IndicesSequias[0].valor[0] = 1f;
                _dataSet._IndicesSequias[0].indeterminacion[0] = true;
            }
            else if (_dataSet._SeqMagnitudAlt[0] != 0f & _dataSet._SeqMagnitudNat[0] == 0f)
            {
                _dataSet._IndicesSequias[0].valor[0] = 0f;
                _dataSet._IndicesSequias[0].indeterminacion[0] = true;
            }
            else if (_dataSet._SeqMagnitudAlt[0] > _dataSet._SeqMagnitudNat[0])
            {
                _dataSet._IndicesSequias[0].valor[0] = _dataSet._SeqMagnitudNat[0] / _dataSet._SeqMagnitudAlt[0];
                _dataSet._IndicesSequias[0].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesSequias[0].valor[0] = _dataSet._SeqMagnitudAlt[0] / _dataSet._SeqMagnitudNat[0];
            }

            // +++++++++++++++++++++
            // +++++++ I18 +++++++++
            // +++++++++++++++++++++
            _dataSet._IndicesSequias[1].valor = new float[1];
            _dataSet._IndicesSequias[1].invertido = new bool[1];
            _dataSet._IndicesSequias[1].indeterminacion = new bool[1];
            _dataSet._IndicesSequias[1].invertido[0] = false;
            _dataSet._IndicesSequias[1].indeterminacion[0] = false;
            _dataSet._IndicesSequias[1].calculado = true;
            if (_dataSet._SeqMagnitudAlt[1] == 0f & _dataSet._SeqMagnitudNat[1] == 0f)
            {
                _dataSet._IndicesSequias[1].valor[0] = 1f;
                _dataSet._IndicesSequias[1].indeterminacion[0] = true;
            }
            else if (_dataSet._SeqMagnitudAlt[1] != 0f & _dataSet._SeqMagnitudNat[1] == 0f)
            {
                _dataSet._IndicesSequias[1].valor[0] = 0f;
                _dataSet._IndicesSequias[1].indeterminacion[0] = true;
            }
            else if (_dataSet._SeqMagnitudAlt[1] > _dataSet._SeqMagnitudNat[1])
            {
                _dataSet._IndicesSequias[1].valor[0] = _dataSet._SeqMagnitudNat[1] / _dataSet._SeqMagnitudAlt[1];
                _dataSet._IndicesSequias[1].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesSequias[1].valor[0] = _dataSet._SeqMagnitudAlt[1] / _dataSet._SeqMagnitudNat[1];
            }

            // +++++++++++++++++++++
            // +++++++ I19 +++++++++
            // +++++++++++++++++++++
            _dataSet._IndicesSequias[2].valor = new float[1];
            _dataSet._IndicesSequias[2].invertido = new bool[1];
            _dataSet._IndicesSequias[2].indeterminacion = new bool[1];
            _dataSet._IndicesSequias[2].invertido[0] = false;
            _dataSet._IndicesSequias[2].indeterminacion[0] = false;
            _dataSet._IndicesSequias[2].calculado = true;
            if (_dataSet._SeqVariabilidadAlt[0] != -9999 & _dataSet._SeqVariabilidadNat[0] != -9999)
            {
                if (_dataSet._SeqVariabilidadAlt[0] == 0f & _dataSet._SeqVariabilidadNat[0] == 0f)
                {
                    _dataSet._IndicesSequias[2].valor[0] = 1f;
                    _dataSet._IndicesSequias[2].indeterminacion[0] = true;
                }
                else if (_dataSet._SeqVariabilidadAlt[0] != 0f & _dataSet._SeqVariabilidadNat[0] == 0f)
                {
                    _dataSet._IndicesSequias[2].valor[0] = 0f;
                    _dataSet._IndicesSequias[2].indeterminacion[0] = true;
                }
                else if (_dataSet._SeqVariabilidadAlt[0] > _dataSet._SeqVariabilidadNat[0])
                {
                    _dataSet._IndicesSequias[2].valor[0] = _dataSet._SeqVariabilidadNat[0] / _dataSet._SeqVariabilidadAlt[0];
                    _dataSet._IndicesSequias[2].invertido[0] = true;
                }
                else
                {
                    _dataSet._IndicesSequias[2].valor[0] = _dataSet._SeqVariabilidadAlt[0] / _dataSet._SeqVariabilidadNat[0];
                }
            }
            else
            {
                _dataSet._IndicesSequias[2].valor[0] = -9999;
                _dataSet._IndicesSequias[2].calculado = false;
            }

            // +++++++++++++++++++++
            // +++++++ I20 +++++++++
            // +++++++++++++++++++++
            _dataSet._IndicesSequias[3].valor = new float[1];
            _dataSet._IndicesSequias[3].invertido = new bool[1];
            _dataSet._IndicesSequias[3].indeterminacion = new bool[1];
            _dataSet._IndicesSequias[3].invertido[0] = false;
            _dataSet._IndicesSequias[3].indeterminacion[0] = false;
            _dataSet._IndicesSequias[3].calculado = true;
            if (_dataSet._SeqVariabilidadAlt[1] != -9999 & _dataSet._SeqVariabilidadNat[1] != -9999)
            {
                if (_dataSet._SeqVariabilidadAlt[1] == 0f & _dataSet._SeqVariabilidadNat[1] == 0f)
                {
                    _dataSet._IndicesSequias[3].valor[0] = 1f;
                    _dataSet._IndicesSequias[3].indeterminacion[0] = true;
                }
                else if (_dataSet._SeqVariabilidadAlt[1] != 0f & _dataSet._SeqVariabilidadNat[1] == 0f)
                {
                    _dataSet._IndicesSequias[3].valor[0] = 0f;
                    _dataSet._IndicesSequias[3].indeterminacion[0] = true;
                }
                else if (_dataSet._SeqVariabilidadAlt[1] > _dataSet._SeqVariabilidadNat[1])
                {
                    _dataSet._IndicesSequias[3].valor[0] = _dataSet._SeqVariabilidadNat[1] / _dataSet._SeqVariabilidadAlt[1];
                    _dataSet._IndicesSequias[3].invertido[0] = true;
                }
                else
                {
                    _dataSet._IndicesSequias[3].valor[0] = _dataSet._SeqVariabilidadAlt[1] / _dataSet._SeqVariabilidadNat[1];
                }
            }
            else
            {
                _dataSet._IndicesSequias[3].valor[0] = -9999;
                _dataSet._IndicesSequias[3].calculado = false;
            }


            // +++++++++++++++++++++
            // +++++++ I22 +++++++++
            // +++++++++++++++++++++
            _dataSet._IndicesSequias[4].valor = new float[1];
            _dataSet._IndicesSequias[4].invertido = new bool[1];
            _dataSet._IndicesSequias[4].indeterminacion = new bool[1];
            _dataSet._IndicesSequias[4].invertido[0] = false;
            _dataSet._IndicesSequias[4].indeterminacion[0] = false;
            _dataSet._IndicesSequias[4].calculado = true;
            if (_dataSet._SeqDuracionAlt[0] == 0f & _dataSet._SeqDuracionNat[0] == 0f)
            {
                _dataSet._IndicesSequias[4].valor[0] = 1f;
                _dataSet._IndicesSequias[4].indeterminacion[0] = true;
            }
            else if (_dataSet._SeqDuracionAlt[0] != 0f & _dataSet._SeqDuracionNat[0] == 0f)
            {
                _dataSet._IndicesSequias[4].valor[0] = 0f;
                _dataSet._IndicesSequias[4].indeterminacion[0] = true;
            }
            else if (_dataSet._SeqDuracionAlt[0] > _dataSet._SeqDuracionNat[0])
            {
                _dataSet._IndicesSequias[4].valor[0] = _dataSet._SeqDuracionNat[0] / _dataSet._SeqDuracionAlt[0];
                _dataSet._IndicesSequias[4].invertido[0] = true;
            }
            else
            {
                _dataSet._IndicesSequias[4].valor[0] = _dataSet._SeqDuracionAlt[0] / _dataSet._SeqDuracionNat[0];
            }

            // ++++++++++++++++++++++++++++
            // +++++++ IAH 20 - I23 +++++++
            // ++++++++++++++++++++++++++++
            // +++ Modificacion del 7/2/08: Cambiar umbral de 10 a 5
            // ++++++++++++++++++++++++++++++++++
            _dataSet._IndicesSequias[5].valor = new float[1];
            _dataSet._IndicesSequias[5].invertido = new bool[1];
            _dataSet._IndicesSequias[5].indeterminacion = new bool[1];
            _dataSet._IndicesSequias[5].invertido[0] = false;
            _dataSet._IndicesSequias[5].indeterminacion[0] = false;
            _dataSet._IndicesSequias[5].calculado = true;
            _dataSet._IndicesSequiasI23Meses = new float[12];
            _dataSet._IndicesSequiasI23MesesInversos = new float[12];

            // If (Not Me._usarCoe) Then
            // If ((365 - Me._dataSet._SeqDuracionAlt(1)) = 0) And ((365 - Me._dataSet._SeqDuracionNat(1)) = 0) Then
            // Me._dataSet._IndicesSequias(5).valor(0) = 1
            // Me._dataSet._IndicesSequias(5).indeterminacion(0) = True
            // ElseIf ((365 - Me._dataSet._SeqDuracionAlt(1)) <> 0) And ((365 - Me._dataSet._SeqDuracionNat(1)) = 0) Then
            // Me._dataSet._IndicesSequias(5).valor(0) = 0
            // Me._dataSet._IndicesSequias(5).indeterminacion(0) = True
            // ElseIf ((365 - Me._dataSet._SeqDuracionAlt(1)) > (365 - Me._dataSet._SeqDuracionNat(1))) Then
            // Me._dataSet._IndicesSequias(5).valor(0) = (365 - Me._dataSet._SeqDuracionNat(1)) / (365 - Me._dataSet._SeqDuracionAlt(1))
            // Me._dataSet._IndicesSequias(5).invertido(0) = True
            // Else
            // Me._dataSet._IndicesSequias(5).valor(0) = (365 - Me._dataSet._SeqDuracionAlt(1)) / (365 - Me._dataSet._SeqDuracionNat(1))
            // End If
            // Else
            // Dim sumNulos As Integer

            // sumNulos = 0
            // For i = 0 To Me._datos.SerieNatDiaria.nAños - 1
            // If ((365 - Me._dataSet._nDiasNulosAlt(i)) = 0) And ((365 - Me._dataSet._nDiasNulosNat(i)) = 0) Then
            // Me._dataSet._IndicesSequias(5).valor(0) = 1
            // Me._dataSet._IndicesSequias(5).indeterminacion(0) = True
            // ElseIf ((365 - Me._dataSet._nDiasNulosAlt(i)) <> 0) And ((365 - Me._dataSet._nDiasNulosNat(i)) = 0) Then
            // Me._dataSet._IndicesSequias(5).valor(0) = 0
            // Me._dataSet._IndicesSequias(5).indeterminacion(0) = True
            // ElseIf ((365 - Me._dataSet._nDiasNulosAlt(i)) > (365 - Me._dataSet._nDiasNulosNat(i))) Then
            // sumNulos = sumNulos + (365 - Me._dataSet._nDiasNulosNat(i)) / (365 - Me._dataSet._nDiasNulosAlt(i))
            // Me._dataSet._IndicesSequias(5).invertido(0) = True
            // Else
            // sumNulos = sumNulos + (365 - Me._dataSet._nDiasNulosAlt(i)) / (365 - Me._dataSet._nDiasNulosNat(i))
            // End If
            // 'sumNulos = sumNulos + (365 - Me._dataSet._nDiasNulosAlt(i)) / (365 - Me._dataSet._nDiasNulosNat(i))
            // Next

            // Me._dataSet._IndicesSequias(5).valor(0) = (1 / Me._datos.SerieNatDiaria.nAños) * sumNulos
            // End If

            // +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
            // +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            var acum = default(float);
            for (i = 0; i <= 11; i++)
            {
                // If (Me._dataSet._SeqDuracionCerosMesAlt.ndias(i) = 0) And (Me._dataSet._SeqDuracionCerosMesNat.ndias(i) = 0) Then
                // acum = acum + 1
                // Me._dataSet._IndicesSequias(5).indeterminacion(0) = True
                // ElseIf (Me._dataSet._SeqDuracionCerosMesAlt.ndias(i) <> 0) And (Me._dataSet._SeqDuracionCerosMesNat.ndias(i) = 0) Then
                // acum = acum + 0
                // Me._dataSet._IndicesSequias(5).indeterminacion(0) = True
                // Else
                // +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
                // +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                float dif = Math.Abs(_dataSet._SeqDuracionCerosMesNat.ndias[i] - _dataSet._SeqDuracionCerosMesAlt.ndias[i]);
                if (_dataSet._SeqDuracionCerosMesAlt.ndias[i] > _dataSet._SeqDuracionCerosMesNat.ndias[i])
                {
                    _dataSet._IndicesSequiasI23MesesInversos[i] = Conversions.ToSingle(true);
                }
                else
                {
                    _dataSet._IndicesSequiasI23MesesInversos[i] = Conversions.ToSingle(false);
                }

                _dataSet._IndicesSequiasI23Meses[i] = 0f;
                if (dif <= 5f)
                {
                    _dataSet._IndicesSequiasI23Meses[i] = (5f - dif) / 5f;
                    acum = acum + (5f - dif) / 5f;
                }
                // End If
            }

            _dataSet._IndicesSequias[5].valor[0] = (float)(1d / 12d * acum);

            // +++++++++++++++++++
            // +++++++ I24 +++++++
            // +++++++++++++++++++
            // +++ Modificacion del 7/2/08: Cambiar umbral de 10 a 5
            // ++++++++++++++++++++++++++++++++++
            _dataSet._IndicesSequias[6].valor = new float[1];
            _dataSet._IndicesSequias[6].invertido = new bool[1];
            _dataSet._IndicesSequias[6].indeterminacion = new bool[1];
            _dataSet._IndicesSequias[6].invertido[0] = false;
            _dataSet._IndicesSequias[6].indeterminacion[0] = false;
            _dataSet._IndicesSequias[6].calculado = true;
            _dataSet._IndicesSequiasI24Meses = new float[12];
            _dataSet._IndicesSequiasI24MesesInversos = new float[12];
            acum = 0f;
            for (i = 0; i <= 11; i++)
            {
                // If (Me._dataSet._SeqEstacionalidadAlt.ndias(i) = 0) And (Me._dataSet._SeqEstacionalidadNat.ndias(i) = 0) Then
                // acum = acum + 1
                // Me._dataSet._IndicesSequias(6).indeterminacion(0) = True
                // 'ElseIf (Me._dataSet._SeqEstacionalidadAlt.ndias(i) <> 0) And (Me._dataSet._SeqEstacionalidadNat.ndias(i) = 0) Then
                // '    acum = acum + 0
                // '    Me._dataSet._IndicesSequias(6).indeterminacion(0) = True
                // 'ElseIf (Me._dataSet._SeqEstacionalidadAlt.ndias(i) > Me._dataSet._SeqEstacionalidadNat.ndias(i)) Then
                // 'acum = acum + Me._dataSet._SeqEstacionalidadNat.ndias(i) / Me._dataSet._SeqEstacionalidadAlt.ndias(i)
                // 'Me._dataSet._IndicesSequias(6).invertido(0) = True
                // Else
                // +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
                // +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                // +++++++ CAMBIO: 28/12/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // +++ Otro documento, en que se cambia el umbral de 20 por otro de 10
                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                float dif = Math.Abs(_dataSet._SeqEstacionalidadNat.ndias[i] - _dataSet._SeqEstacionalidadAlt.ndias[i]);
                if (_dataSet._SeqEstacionalidadAlt.ndias[i] > _dataSet._SeqEstacionalidadNat.ndias[i])
                {
                    _dataSet._IndicesSequiasI24MesesInversos[i] = Conversions.ToSingle(true);
                }
                else
                {
                    _dataSet._IndicesSequiasI24MesesInversos[i] = Conversions.ToSingle(false);
                }

                _dataSet._IndicesSequiasI24Meses[i] = 0f;

                // If (dif <= 20) Then
                // acum = acum + ((20 - dif) / 20)
                // End If
                if (dif <= 5f)
                {
                    _dataSet._IndicesSequiasI24Meses[i] = (5f - dif) / 5f;
                    acum = acum + (5f - dif) / 5f;
                }

                // End If
            }

            _dataSet._IndicesSequias[6].valor[0] = (float)(1d / 12d * acum);
        }
        /// <summary>
        /// Esto es el IAG
        /// </summary>
        /// <remarks></remarks>
        public void CalcularIndiceAlteracionGlobalHabituales()
        {
            int i, j;
            float[] aux1, aux2;
            int nCal;
            aux1 = new float[3];
            aux2 = new float[3];
            _dataSet._IndiceIAG = new float[4];
            for (i = 0; i <= 2; i++)
            {
                aux1[i] = 0f;
                aux2[i] = 0f;
            }

            nCal = 0;
            for (i = 0; i <= 6; i++)
            {
                if (_dataSet._IndicesHabituales[i].calculado)
                {
                    nCal = nCal + 1;
                    for (j = 0; j <= 2; j++)
                    {
                        aux1[j] = aux1[j] + _dataSet._IndicesHabituales[i].valor[j];
                        aux2[j] = (float)(aux2[j] + Math.Pow(_dataSet._IndicesHabituales[i].valor[j], 2d));
                    }
                }
            }

            for (i = 0; i <= 2; i++)
                _dataSet._IndiceIAG[i] = (float)((Math.Pow(aux1[i], 2d) - aux2[i]) / (nCal * (nCal - 1)));
            _dataSet._IndiceIAG[3] = (_dataSet._IndiceIAG[0] + _dataSet._IndiceIAG[1] + _dataSet._IndiceIAG[2]) / 3f;
        }

        public void CalcularIndiceAlteracionGlobalHabitualesAgregados()
        {
            int i;
            float aux1, aux2;
            int nCal;
            nCal = 0;
            aux1 = 0f;
            aux2 = 0f;
            for (i = 0; i <= 6; i++)
            {
                if (_dataSet._IndicesHabitualesAgregados[i].calculado)
                {
                    nCal = nCal + 1;
                    // For j = 0 To 2
                    aux1 = aux1 + _dataSet._IndicesHabitualesAgregados[i].valor[0];
                    aux2 = (float)(aux2 + Math.Pow(_dataSet._IndicesHabitualesAgregados[i].valor[0], 2d));
                    // Next
                }
            }

            _dataSet._IndiceIAG_Agregados = (float)((Math.Pow(aux1, 2d) - aux2) / (nCal * (nCal - 1)));
        }

        public void CalcularIndiceAlteracionGlobalAvenidas()
        {
            int i;
            float aux1 = default, aux2 = default;
            int nCal;
            nCal = 0;
            for (i = 0; i <= 6; i++)
            {
                if (_dataSet._IndicesAvenidas[i].calculado)
                {
                    nCal = nCal + 1;
                    aux1 = aux1 + _dataSet._IndicesAvenidas[i].valor[0];
                    aux2 = (float)(aux2 + Math.Pow(_dataSet._IndicesAvenidas[i].valor[0], 2d));
                }
            }

            _dataSet._IndiceIAG_Ave = (float)((Math.Pow(aux1, 2d) - aux2) / (nCal * (nCal - 1)));
        }

        public void CalcularIndiceAlteracionGlobalSequias()
        {
            int i;
            float aux1 = default, aux2 = default;
            int nCal;
            nCal = 0;
            for (i = 0; i <= 6; i++)
            {
                if (_dataSet._IndicesSequias[i].calculado)
                {
                    nCal = nCal + 1;
                    aux1 = aux1 + _dataSet._IndicesSequias[i].valor[0];
                    aux2 = (float)(aux2 + Math.Pow(_dataSet._IndicesSequias[i].valor[0], 2d));
                }
            }

            _dataSet._IndiceIAG_Seq = (float)((Math.Pow(aux1, 2d) - aux2) / (nCal * (nCal - 1)));
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public void CalcularRegimenNatural()
        {
            float[] percentil10;
            float[] percentil90;
            float[] mediana;
            float[][] aportacionesMen;
            aportacionesMen = new float[12][];
            for (int i = 0; i <= 11; i++)
                aportacionesMen[i] = new float[_dataSet._AportacionNatAnual.año.Length];
            for (int i = 0, loopTo = _dataSet._AportacionNatAnual.año.Length - 1; i <= loopTo; i++)
            {
                for (int j = 0; j <= 11; j++)
                    aportacionesMen[j][i] = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
            }

            for (int i = 0; i <= 11; i++)
            {
                Array.Sort(aportacionesMen[i]);
                Array.Reverse(aportacionesMen[i]);
            }

            var ultimos = new ArrayList();
            var primeros = new ArrayList();

            // 10 es el mes de inicio, lo que nos permite 
            // variar el mes de inicio del año hidrologico
            for (int i = 0, loopTo1 = 12 - _datos.mesInicio; i <= loopTo1; i++)
                ultimos.Add(aportacionesMen[i]);
            for (int i = 12 - _datos.mesInicio + 1; i <= 11; i++)
                primeros.Add(aportacionesMen[i]);
            primeros.AddRange(ultimos);
            int p10 = -1;
            int p90 = -1;
            float aux;
            float aux2;
            for (int i = 0, loopTo2 = _dataSet._AportacionNatAnual.año.Length - 1; i <= loopTo2; i++)
            {
                aux = (float)(i / (double)_dataSet._AportacionNatAnual.año.Length * 100d);
                if (p10 == -1)
                {
                    if (aux >= 10f)
                    {
                        p10 = i - 1;
                    }
                }

                if (aux >= 90f)
                {
                    p90 = i - 1;
                    break;
                }
            }

            percentil10 = new float[12];
            percentil90 = new float[12];
            mediana = new float[12];
            int p0;
            int p1;
            for (int i = 0; i <= 11; i++)
            {
                aux = (float)(p10 / (double)_dataSet._AportacionNatAnual.año.Length * 100d);
                aux2 = (float)((p10 + 1) / (double)_dataSet._AportacionNatAnual.año.Length * 100d);
                percentil10[i] = (10f - aux) / (aux2 - aux) * (aportacionesMen[i][p10 + 1] - aportacionesMen[i][p10]) + aportacionesMen[i][p10];
                aux = (float)(p90 / (double)_dataSet._AportacionNatAnual.año.Length * 100d);
                aux2 = (float)((p90 + 1) / (double)_dataSet._AportacionNatAnual.año.Length * 100d);
                percentil90[i] = (90f - aux) / (aux2 - aux) * (aportacionesMen[i][p90 + 1] - aportacionesMen[i][p90]) + aportacionesMen[i][p90];
                p0 = (int)(_dataSet._AportacionNatAnual.año.Length / 2d - 1d);
                p1 = p0 + 1;
                if (_dataSet._AportacionNatAnual.año.Length % 2 != 0)
                {
                    mediana[i] = aportacionesMen[i][(int)Math.Round(_dataSet._AportacionNatAnual.año.Length / 2d)];
                }
                else
                {
                    mediana[i] = (aportacionesMen[i][p0] + aportacionesMen[i][p1]) / 2f;
                }
            }

            _dataSet._percentil10 = percentil10;
            _dataSet._percentil90 = percentil90;
            _dataSet._medianaMenNat = mediana;
        }

        public void CalcularRegimenAlterado()
        {
            float[] mediana;
            int[] cumple;
            float[][] aportacionesMen;
            aportacionesMen = new float[12][];
            for (int i = 0; i <= 11; i++)
                aportacionesMen[i] = new float[_dataSet._AportacionAltAnual.año.Length];
            for (int i = 0, loopTo = _dataSet._AportacionAltAnual.año.Length - 1; i <= loopTo; i++)
            {
                for (int j = 0; j <= 11; j++)
                    aportacionesMen[j][i] = _dataSet._AportacionAltMen.aportacion[i * 12 + j];
            }

            for (int i = 0; i <= 11; i++)
            {
                Array.Sort(aportacionesMen[i]);
                Array.Reverse(aportacionesMen[i]);
            }

            var ultimos = new ArrayList();
            var primeros = new ArrayList();

            // 10 es el mes de inicio, lo que nos permite 
            // variar el mes de inicio del año hidrologico
            for (int i = 0, loopTo1 = 12 - _datos.mesInicio; i <= loopTo1; i++)
                ultimos.Add(aportacionesMen[i]);
            for (int i = 12 - _datos.mesInicio + 1; i <= 11; i++)
                primeros.Add(aportacionesMen[i]);
            primeros.AddRange(ultimos);
            mediana = new float[12];
            int p0;
            int p1;
            for (int i = 0; i <= 11; i++)
            {
                p0 = (int)(_dataSet._AportacionAltAnual.año.Length / 2d - 1d);
                p1 = p0 + 1;
                if (_dataSet._AportacionAltAnual.año.Length % 2 != 0)
                {
                    mediana[i] = aportacionesMen[i][(int)Math.Round(_dataSet._AportacionAltAnual.año.Length / 2d)];
                }
                else
                {
                    mediana[i] = (aportacionesMen[i][p0] + aportacionesMen[i][p1]) / 2f;
                }
            }

            cumple = new int[12];
            for (int i = 0, loopTo2 = _dataSet._AportacionAltAnual.año.Length - 1; i <= loopTo2; i++)
            {
                for (int j = 0; j <= 11; j++)
                {
                    if (_dataSet._percentil10[j] >= aportacionesMen[j][i] & aportacionesMen[j][i] >= _dataSet._percentil90[j])
                    {
                        cumple[j] += 1;
                    }
                }
            }

            _dataSet._mesesQueCumplen = cumple;
            _dataSet._medianaMenAlt = mediana;
        }

        public void CalcularRegimenNaturalAnual()
        {
            float medianaNat;
            float percentil10;
            float percentil90;
            float[] aporOrd;
            aporOrd = (float[])_dataSet._AportacionNatAnual.aportacion.Clone();
            Array.Sort(aporOrd);
            Array.Reverse(aporOrd);

            // If ((aporOrd.Length Mod 2) = 0) Then
            // medianaNat = (aporOrd((aporOrd.Length / 2) - 1) + aporOrd(aporOrd.Length / 2)) / 2
            // Else
            // medianaNat = aporOrd((aporOrd.Length - 1) / 2)
            // End If

            int p10 = -1;
            int p90 = -1;
            float aux;
            float aux2;
            for (int i = 0, loopTo = aporOrd.Length - 1; i <= loopTo; i++)
            {
                aux = (float)(i / (double)aporOrd.Length * 100d);
                if (p10 == -1)
                {
                    if (aux >= 10f)
                    {
                        p10 = i - 1;
                    }
                }

                if (aux >= 90f)
                {
                    p90 = i - 1;
                    break;
                }
            }

            aux = (float)(p10 / (double)aporOrd.Length * 100d);
            aux2 = (float)((p10 + 1) / (double)aporOrd.Length * 100d);
            percentil10 = (10f - aux) / (aux2 - aux) * (aporOrd[p10 + 1] - aporOrd[p10]) + aporOrd[p10];
            aux = (float)(p90 / (double)aporOrd.Length * 100d);
            aux2 = (float)((p90 + 1) / (double)aporOrd.Length * 100d);
            percentil90 = (90f - aux) / (aux2 - aux) * (aporOrd[p90 + 1] - aporOrd[p90]) + aporOrd[p90];
            int p0;
            int p1;
            p0 = (int)(aporOrd.Length / 2d - 1d);
            p1 = p0 + 1;
            if (_dataSet._AportacionNatAnual.año.Length % 2 != 0)
            {
                medianaNat = aporOrd[(int)Math.Round(_dataSet._AportacionNatAnual.año.Length / 2d)];
            }
            else
            {
                medianaNat = (aporOrd[p0] + aporOrd[p1]) / 2f;
            }

            _dataSet._percentil10Anual = percentil10;
            _dataSet._percentil90Anual = percentil90;
            _dataSet._medianaAnualNat = medianaNat;
        }

        public void CalcularRegimenAlteradoAnual()
        {
            float medianaAlt;
            int cumple;
            float[] aporOrd;
            aporOrd = (float[])_dataSet._AportacionAltAnual.aportacion.Clone();
            Array.Sort(aporOrd);
            Array.Reverse(aporOrd);
            if (aporOrd.Length % 2 == 0)
            {
                medianaAlt = (aporOrd[(int)Math.Round(aporOrd.Length / 2d - 1d)] + aporOrd[(int)Math.Round(aporOrd.Length / 2d)]) / 2f;
            }
            else
            {
                medianaAlt = aporOrd[(int)Math.Round((aporOrd.Length - 1) / 2d)];
            }

            cumple = 0;
            for (int i = 0, loopTo = aporOrd.Length - 1; i <= loopTo; i++)
            {
                if (_dataSet._percentil10Anual >= aporOrd[i] & aporOrd[i] >= _dataSet._percentil90Anual)
                {
                    cumple += 1;
                }
            }

            _dataSet._anyosQueCumplen = cumple;
            _dataSet._medianaAnualAlt = medianaAlt;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public void CalcularReferencias()
        {
            int nAños = _datos.SerieNatDiaria.nAños;
            // Dim _dataSet._1QMin As Single
            // Dim _dataSet._7QMin As Single
            // Dim _dataSet._10QMin As Single

            // Calcular Minimos
            float[] listaMinDiarios;
            listaMinDiarios = new float[nAños];
            int añoActual;
            int pos;

            // ---------------------------------------------------------
            // -------  Calculo de Qmin  -------------------------------
            // ---------------------------------------------------------

            // Calcular 1Qmin
            // --------------
            pos = 0;
            listaMinDiarios[0] = 999999999f;
            añoActual = _datos.SerieNatDiaria.dia[0].Year;
            int i;
            int j;
            var loopTo = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // --------------------------------------------------------
                // Usa la ventana de 1 dia
                // Si el año es diferente cambio donde almacenar el maximo
                // --------------------------------------------------------
                if (_datos.SerieNatDiaria.dia[i].Day == 1 & _datos.SerieNatDiaria.dia[i].Month == _datos.mesInicio & _datos.SerieNatDiaria.dia[i].Year != añoActual)
                {
                    pos = pos + 1;
                    listaMinDiarios[pos] = 999999999f;
                    añoActual = _datos.SerieNatDiaria.dia[i].Year;
                }

                if (listaMinDiarios[pos] > _datos.SerieNatDiaria.caudalDiaria[i])
                {
                    listaMinDiarios[pos] = _datos.SerieNatDiaria.caudalDiaria[i];
                }
            }

            _dataSet._1QMin = 999999999f;
            var loopTo1 = listaMinDiarios.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                if (_dataSet._1QMin > listaMinDiarios[i])
                {
                    _dataSet._1QMin = listaMinDiarios[i];
                }
            }

            // Calcular 7Qmin
            // --------------
            // Ahora se usan ventanas de 7 dias para calcular el minimo
            pos = 0;
            float[] listaMinDiarios7;
            listaMinDiarios7 = new float[nAños];
            listaMinDiarios7[0] = 999999999f;
            añoActual = _datos.SerieNatDiaria.dia[0].Year;
            float media;
            var loopTo2 = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                if (i + 6 > _datos.SerieNatDiaria.dia.Length - 1)
                {
                    break;
                }
                // Si el año es diferente al final de la ventana cambio donde almacenar el maximo
                if (_datos.SerieNatDiaria.dia[i + 6].Day == 1 & _datos.SerieNatDiaria.dia[i + 6].Month == _datos.mesInicio & _datos.SerieNatDiaria.dia[i + 6].Year != añoActual)
                {
                    pos = pos + 1;
                    listaMinDiarios7[pos] = 999999999f;
                    añoActual = _datos.SerieNatDiaria.dia[i].Year;
                }
                // Hado la media de la ventana de 7 dias
                media = 0f;
                for (j = 0; j <= 6; j++)
                    media += _datos.SerieNatDiaria.caudalDiaria[i + j];
                media = media / 7f;
                if (listaMinDiarios7[pos] > media)
                {
                    listaMinDiarios7[pos] = media;
                }
            }

            _dataSet._7QMin = 999999999f;
            var loopTo3 = listaMinDiarios7.Length - 1;
            for (i = 0; i <= loopTo3; i++)
            {
                if (_dataSet._7QMin > listaMinDiarios7[i])
                {
                    _dataSet._7QMin = listaMinDiarios7[i];
                }
            }

            // Calcular 15Qmin
            // --------------
            // Ahora se usan ventanas de 10 dias para calcular el minimo
            pos = 0;
            float[] listaMinDiarios15;
            listaMinDiarios15 = new float[nAños];
            listaMinDiarios15[0] = 999999999f;
            añoActual = _datos.SerieNatDiaria.dia[0].Year;
            // Dim media As Single
            var loopTo4 = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo4; i++)
            {
                if (i + 14 > _datos.SerieNatDiaria.dia.Length - 1)
                {
                    break;
                }
                // Si el año es diferente al final de la ventana cambio donde almacenar el maximo
                if (_datos.SerieNatDiaria.dia[i + 14].Day == 1 & _datos.SerieNatDiaria.dia[i + 14].Month == _datos.mesInicio & _datos.SerieNatDiaria.dia[i + 14].Year != añoActual)
                {
                    pos = pos + 1;
                    listaMinDiarios15[pos] = 999999999f;
                    añoActual = _datos.SerieNatDiaria.dia[i].Year;
                }
                // Hado la media de la ventana de 15 dias
                media = 0f;
                for (j = 0; j <= 14; j++)
                    media += _datos.SerieNatDiaria.caudalDiaria[i + j];
                media = media / 14f;
                if (listaMinDiarios15[pos] > media)
                {
                    listaMinDiarios15[pos] = media;
                }
            }

            _dataSet._15QMin = 999999999f;
            var loopTo5 = listaMinDiarios15.Length - 1;
            for (i = 0; i <= loopTo5; i++)
            {
                if (_dataSet._15QMin > listaMinDiarios15[i])
                {
                    _dataSet._15QMin = listaMinDiarios15[i];
                }
            }

            // -----------------------------------------------------
            // ---- Calculo de Q de retorno ------------------------
            // -----------------------------------------------------
            // Dim _dataSet._7QRetorno() As Single
            // Dim _dataSet._10QRetorno() As Single
            _dataSet._7QRetorno = new float[1];
            _dataSet._10QRetorno = new float[1];
            AjusteLogPearsonIII(listaMinDiarios7.Length, listaMinDiarios7, ref _dataSet._7QRetorno);
            AjusteLogPearsonIII(listaMinDiarios15.Length, listaMinDiarios15, ref _dataSet._10QRetorno);

            // Arreglar el tema de la posicion 0 del array
            _dataSet._7QRetorno[0] = _dataSet._7QRetorno[1];
            _dataSet._7QRetorno[1] = _dataSet._7QRetorno[2];
            _dataSet._7QRetorno[2] = _dataSet._7QRetorno[3];
            Array.Resize(ref _dataSet._7QRetorno, 3);
            _dataSet._10QRetorno[0] = _dataSet._10QRetorno[1];
            _dataSet._10QRetorno[1] = _dataSet._10QRetorno[2];
            _dataSet._10QRetorno[2] = _dataSet._10QRetorno[3];
            Array.Resize(ref _dataSet._10QRetorno, 3);

            // ---------------------------------------------------------
            // ------- Calculo de MnQMin -------------------------------
            // ---------------------------------------------------------
            // Dim _mnQ() As Single
            _dataSet._mnQ = new float[1];
            // Dim _mnQ5 As Single
            // Dim _mnQ10 As Single

            float[] _mnAños;
            _mnAños = new float[_dataSet._AportacionNatAnualOrdAños.año.Length];
            float min;
            var iMin = default(int);
            int iDias;
            int esCoe;
            var loopTo6 = _dataSet._AportacionNatAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo6; i++)
            {
                // For i = 0 To Me._datos.SerieNatDiaria.nAños - 1
                esCoe = Conversions.ToInteger(false);
                for (int aux = 0, loopTo7 = _datos.SerieNatDiaria.dia.Length - 1; aux <= loopTo7; aux++)
                {
                    if (_datos.SerieNatDiaria.dia[aux].Year == _dataSet._AportacionNatAnualOrdAños.año[i] & _datos.SerieNatDiaria.dia[aux].Month == _datos.mesInicio)
                    {
                        esCoe = Conversions.ToInteger(true);
                        break;
                    }
                }

                if (Conversions.ToBoolean(~esCoe))
                {
                    continue;
                }

                min = 9999999f;
                // Mes de menor aportacion
                for (j = 0; j <= 11; j++)
                {
                    if (min > _dataSet._AportacionNatMen.aportacion[i * 12 + j])
                    {
                        min = _dataSet._AportacionNatMen.aportacion[i * 12 + j];
                        iMin = j;
                    }
                }
                // Ya conozco el mes minimo
                // 
                // Ahora busco el mes/año dentro de la lista de datos diarios (que es una ristra de numeros)
                iDias = 0;
                while (_datos.SerieNatDiaria.dia[iDias].Year != _dataSet._AportacionNatAnualOrdAños.año[i])
                    iDias += 1;
                while (_datos.SerieNatDiaria.dia[iDias].Month != _dataSet._AportacionNatMen.mes[i * 12 + iMin].Month)
                    iDias += 1;
                // Saco los valores diarios (28-29-30-31 dias) posibles de un mes
                var valoresMesMinimo = new ArrayList();
                while (_datos.SerieNatDiaria.dia[iDias].Month == _dataSet._AportacionNatMen.mes[i * 12 + iMin].Month)
                {
                    valoresMesMinimo.Add(_datos.SerieNatDiaria.caudalDiaria[iDias]);
                    iDias += 1;
                    if (iDias == _datos.SerieNatDiaria.dia.Length)
                    {
                        break;
                    }
                }
                // Hallar la mediana
                int posMediana = (int)Math.Round(valoresMesMinimo.Count / 2d);
                _mnAños[i] = Conversions.ToSingle(valoresMesMinimo[posMediana]);
            }

            AjusteLogPearsonIII(_mnAños.Length, _mnAños, ref _dataSet._mnQ);
            _dataSet._mnQ[0] = _dataSet._mnQ[1];
            _dataSet._mnQ[1] = _dataSet._mnQ[2];
            _dataSet._mnQ[2] = _dataSet._mnQ[3];
            Array.Resize(ref _dataSet._mnQ, 3);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

        /* TODO ERROR: Skipped RegionDirectiveTrivia */        /// <summary>
        /// Calcula la aportacion Mensual de los caudales
        /// </summary>
        /// <remarks></remarks>
        public void CalcularAportacionMENSUAL()
        {
            SerieMensual aux;
            int i;
            if (_datos.SerieNatMensual.caudalMensual is null)
            {
                aux = _dataSet._SerieNatMensualCalculada;
            }
            else
            {
                aux = _datos.SerieNatMensual;
            }

            _dataSet._AportacionNatMen.mes = new DateTime[aux.mes.Length];
            _dataSet._AportacionNatMen.aportacion = new float[aux.caudalMensual.Length];
            var loopTo = aux.mes.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                _dataSet._AportacionNatMen.mes[i] = aux.mes[i];
                _dataSet._AportacionNatMen.aportacion[i] = aux.caudalMensual[i]; // (86400 * aux.caudalMensual(i)) / 1000000
            }
        }

        /// <summary>
        /// Calcula la aportacion Mensual de los caudales Alterada
        /// </summary>
        /// <remarks></remarks>
        public void CalcularAportacionMENSUALAlterada()
        {
            SerieMensual aux;
            int i;
            if (_datos.SerieAltMensual.caudalMensual is null)
            {
                aux = _dataSet._SerieAltMensualCalculada;
            }
            else
            {
                aux = _datos.SerieAltMensual;
            }

            _dataSet._AportacionAltMen.mes = new DateTime[aux.mes.Length];
            _dataSet._AportacionAltMen.aportacion = new float[aux.caudalMensual.Length];
            var loopTo = aux.mes.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                _dataSet._AportacionAltMen.mes[i] = aux.mes[i];
                _dataSet._AportacionAltMen.aportacion[i] = aux.caudalMensual[i]; // (86400 * aux.caudalMensual(i)) / 1000000
            }
        }

        /// <summary>
        /// Interpola los valores de caudal mensuales
        /// </summary>
        /// <returns>Devuelve una serie mensual</returns>
        /// <remarks>Espere que datos tenga al menos los diarios</remarks>
        private SerieMensual CalcularSerieMENSUAL()
        {
            SerieMensual salida;
            int i;

            // Dim diasMes As Integer
            int mes = -1;
            int año = -1;
            int pos = 0;      // Posicion en la lista de mese
            DateTime d;
            salida = default;
            var loopTo = _datos.SerieNatDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Cambio de mes
                if (mes != _datos.SerieNatDiaria.dia[i].Month)
                {
                    mes = _datos.SerieNatDiaria.dia[i].Month;
                    if (salida.caudalMensual is null)
                    {
                        Array.Resize(ref salida.caudalMensual, 1);
                        Array.Resize(ref salida.mes, 1);
                    }
                    else
                    {
                        Array.Resize(ref salida.caudalMensual, salida.caudalMensual.Length + 1);
                        Array.Resize(ref salida.mes, salida.mes.Length + 1);
                        // pos = pos + 1
                    }

                    salida.nMeses = salida.nMeses + 1;
                    pos = salida.caudalMensual.Length - 1;
                    if (año != _datos.SerieNatDiaria.dia[i].Year)
                    {
                        año = _datos.SerieNatDiaria.dia[i].Year;
                        salida.nAños = salida.nAños + 1;
                    }

                    d = new DateTime(año, mes, 1);
                    salida.mes[pos] = d;
                    salida.caudalMensual[pos] = 0f;
                }

                salida.caudalMensual[pos] = salida.caudalMensual[pos] + _datos.SerieNatDiaria.caudalDiaria[i]; // / Date.DaysInMonth(Me._datos.SerieNatDiaria.dia(i).Year, Me._datos.SerieNatDiaria.dia(i).Month)
            }

            return salida;
        }

        /// <summary>
        /// Interpola los valores de caudal mensuales Alterados
        /// </summary>
        /// <returns>Devuelve una serie mensual</returns>
        /// <remarks>Espere que datos tenga al menos los diarios</remarks>
        private SerieMensual CalcularSerieMENSUALAlterada()
        {
            SerieMensual salida;
            int i;

            // Dim diasMes As Integer
            int mes = -1;
            int año = -1;
            int pos = 0;      // Posicion en la lista de mese
            DateTime d;
            salida = default;
            var loopTo = _datos.SerieAltDiaria.dia.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // Cambio de mes
                if (mes != _datos.SerieAltDiaria.dia[i].Month)
                {
                    mes = _datos.SerieAltDiaria.dia[i].Month;
                    if (salida.caudalMensual is null)
                    {
                        Array.Resize(ref salida.caudalMensual, 1);
                        Array.Resize(ref salida.mes, 1);
                    }
                    else
                    {
                        Array.Resize(ref salida.caudalMensual, salida.caudalMensual.Length + 1);
                        Array.Resize(ref salida.mes, salida.mes.Length + 1);
                        // pos = pos + 1
                    }

                    salida.nMeses = salida.nMeses + 1;
                    pos = salida.caudalMensual.Length - 1;
                    if (año != _datos.SerieAltDiaria.dia[i].Year)
                    {
                        año = _datos.SerieAltDiaria.dia[i].Year;
                        salida.nAños = salida.nAños + 1;
                    }

                    d = new DateTime(año, mes, 1);
                    salida.mes[pos] = d;
                    salida.caudalMensual[pos] = 0f;
                }

                salida.caudalMensual[pos] = salida.caudalMensual[pos] + _datos.SerieAltDiaria.caudalDiaria[i]; // / Date.DaysInMonth(Me._datos.SerieNatDiaria.dia(i).Year, Me._datos.SerieNatDiaria.dia(i).Month)
            }

            return salida;
        }

        /// <summary>
        /// Calcula la tabla CQC
        /// </summary>
        /// <param name="usarAlterados">Si es para alterados o para naturales</param>
        /// <remarks>NO comprueba si esta calculada anteriormente</remarks>
        public void CalcularTablaCQC(bool usarAlterados)
        {
            int i, j;
            int nAños;

            // Se elige la serie que se va a usar
            SerieDiaria serieAux;
            if (usarAlterados)
            {
                serieAux = _datos.SerieAltDiaria;
                nAños = _datos.SerieAltDiaria.nAños;
            }
            else
            {
                serieAux = _datos.SerieNatDiaria;
                nAños = _datos.SerieNatDiaria.nAños;
            }

            // Tabla auxiliar donde vamos a almacenar
            TablaCQC tablaAux;

            // Definicion de la estructura donde almacenaré la tabla CQC
            tablaAux.pe = new float[365];
            tablaAux.dia = new float[365];
            tablaAux.añomedio = new float[365];
            tablaAux.caudales = new float[nAños][];

            // Relleno los dias incluyendo los bisiestos
            for (j = 0; j <= 364; j++)
            {
                tablaAux.pe[j] = (float)((j + 1) / 365d * 100d);
                tablaAux.dia[j] = j + 1;
            }

            int acum;

            // Relleno los caudales
            acum = 0;
            var loopTo = nAños - 1;
            for (i = 0; i <= loopTo; i++)
            {
                int posibleBisiesto;
                if (_datos.mesInicio > 2)
                {
                    posibleBisiesto = serieAux.dia[acum].Year + 1;
                }
                else
                {
                    posibleBisiesto = serieAux.dia[acum].Year;
                }

                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    Array.Resize(ref tablaAux.caudales[i], 366);
                }
                else
                {
                    Array.Resize(ref tablaAux.caudales[i], 365);
                }

                var loopTo1 = tablaAux.caudales[i].Length - 1;
                for (j = 0; j <= loopTo1; j++)
                    tablaAux.caudales[i][j] = serieAux.caudalDiaria[acum + j];
                if (DateTime.IsLeapYear(posibleBisiesto) == true)
                {
                    acum = acum + 366;
                }
                else
                {
                    acum = acum + 365;
                }
            }

            // Ordenar los caudales
            var loopTo2 = nAños - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                Array.Sort(tablaAux.caudales[i]);
                Array.Reverse(tablaAux.caudales[i]);
            }

            // Generar la media
            for (i = 0; i <= 364; i++)
            {
                tablaAux.añomedio[i] = 0f;
                var loopTo3 = nAños - 1;
                for (j = 0; j <= loopTo3; j++)
                    tablaAux.añomedio[i] = tablaAux.añomedio[i] + tablaAux.caudales[j][i];
                tablaAux.añomedio[i] = tablaAux.añomedio[i] / nAños;
            }

            if (usarAlterados)
            {
                _dataSet._TablaCQCAlt = tablaAux;
            }
            else
            {
                _dataSet._TablaCQCNat = tablaAux;
            }
        }

        /// <summary>
        /// Calcula el ajuste de LGIII
        /// </summary>
        /// <returns>Si todo esta correcto</returns>
        /// <remarks></remarks>
        private bool AjusteLogPearsonIII(int nAños, float[] val, ref float[] valAjus)
        {
            var ut = new float[4];
            var t = new float[4];
            var ck = new float[4];
            float[] PROB;
            float[] ESC;
            float[] x;
            int i; // , j As Integer, k As Integer
            float xm;
            float xs;
            float xg;
            float cst;
            float Vn;
            var ckk = default(float);

            // OJO que no usa la posicion 0 de los arrays.

            // blnajus = True
            ut[1] = 0.0f;
            ut[2] = (float)-0.842d;
            ut[3] = (float)-1.282d;
            t[1] = 2.0f;
            t[2] = 5.0f;
            t[3] = 10.0f;
            PROB = new float[nAños + 1];
            ESC = new float[nAños + 1];
            x = new float[nAños + 1];
            int ii = 1;
            var loopTo = nAños - 1;
            for (i = 0; i <= loopTo; i++)
            {
                x[ii] = miLog10(val[i]);
                ii += 1;
            }

            Vn = nAños;
            xm = 0.0f;
            xs = 0.0f;
            xg = 0.0f;
            var loopTo1 = nAños;
            for (i = 1; i <= loopTo1; i++)
            {
                xm = xm + x[i] / nAños;
                xs = (float)(xs + Math.Pow(x[i], 2d) / nAños);
            }

            xs = (float)(xs - Math.Pow(xm, 2d));
            xs = (float)(xs * Vn / (Vn - 1.0d));
            var loopTo2 = nAños;
            for (i = 1; i <= loopTo2; i++)
                xg = (float)(xg + Math.Pow(x[i] - xm, 3d));
            if (xs != 0.0d)
            {
                cst = (float)(xg * Vn / ((Vn - 1.0d) * (Vn - 2.0d) * Math.Pow(xs, 1.5d)));
            }
            else
            {
                cst = 0.0f;
            }

            valAjus = new float[4];
            for (i = 1; i <= 3; i++)
            {
                kt(ut[i], cst, ref ckk);
                valAjus[i] = (float)(xm + ckk * Math.Pow(xs, 0.5d));
                valAjus[i] = (float)Math.Pow(10.0d, valAjus[i]);
                if (cst == 0.0d)
                {
                    if (xm != 0.0d)
                    {
                        valAjus[i] = (float)Math.Pow(10.0d, xm);
                    }
                    else
                    {
                        valAjus[i] = 0.0f;
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ut"></param>
        /// <param name="xg"></param>
        /// <param name="dk"></param>
        /// <remarks></remarks>
        public void kt(float ut, float xg, ref float dk)
        {
            float Dk1;
            float Dk2;
            Dk1 = (float)(ut + (Math.Pow(ut, 2d) - 1.0d) * (xg / 6f) + 1d / 3d * (Math.Pow(ut, 3d) - 6f * ut) * Math.Pow(xg / 6f, 2d));
            Dk2 = (float)(-(Math.Pow(ut, 2d) - 1.0d) * Math.Pow(xg / 6f, 3d) + ut * Math.Pow(xg / 6f, 4d) + 1d / 3d * Math.Pow(xg / 6f, 5d));
            dk = Dk1 + Dk2;
        }

        private float miLog10(float x)
        {
            float valor;
            if (x > 0.0d)
            {
                valor = (float)(Math.Log(x) / Math.Log(10.0d));
            }
            else
            {
                valor = 0.0f;
            }

            return valor;
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */

    }
}
