using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAHRIS.Calculo
{
    public class IAHRISDataSet
    {
        // Series y aportaciones
        public SerieAnual _SerieNatAnual;
        public SerieMensual _SerieNatMensualCalculada;   // Si no hay es la interpolacion. Si no es la que nos dan.
        public SerieMensual _SerieAltMensualCalculada;
        // Aportaciones
        public AportacionAnual _AportacionNatAnual;
        public AportacionAnual _AportacionNatAnualOrdAños;
        public AportacionAnual _AportacionAltAnual;
        public AportacionAnual _AportacionAltAnualOrdAños;
        public AportacionMensual _AportacionNatMen;
        public AportacionMensual _AportacionAltMen;

        // Limites para  InterAnual
        public float _limHumNat;
        public float _limSecNat;
        public float _limHumAlt;
        public float _limSecAlt;
        // Interanual 
        public GraficoAlterada[] _graficaInterAlt;
        // Intranual
        public float[][] _IntraAnualNat;
        public float[][] _IntraAnualAlt;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // Parametros Habituales
        public float[] _HabMagnitudNat;
        public float[] _HabVariabilidadNat;
        public string[] _HabEstacionalidadNat;
        public float _HabMagnitudNatReducido;
        public float _HabVariabilidadNatReducido;
        public float _HabEstacionalidadNatReducido;
        public float[] _HabVariabilidadDiaraNat;
        public float[] _HabMagnitudAnualNat;
        public float[] _HabMagnitudMensualNat;
        public TablaEstacionalidad[] _HabMagnitudMensualTablaNat;
        public TablaEstacionalidad[] _HabEstacionalidadMensualNat;

        // Parametros Alterados 
        public float[] _HabMagnitudAnualAlt;
        public float[] _HabMagnitudMensualAlt;
        public TablaEstacionalidad[] _HabMagnitudMensualTablaAlt;
        public TablaEstacionalidad[] _HabEstacionalidadMensualAlt;
        public float[] _HabMagnitudAlt;
        public float[] _HabVariabilidadAlt;
        public string[] _HabEstacionalidadAlt;
        public float[] _HabVariabilidadDiaraAlt;

        // Parametros de Avenidas
        public float[] _AveMagnitudNat;
        public float[] _AveVariabilidadNat;
        public TablaEstacionalidad _AveEstacionalidadNat; // Esto es una tabla de 12 meses
        public float _AveDuracionNat;
        // 
        public float[] _AveMagnitudAlt;
        public float[] _AveVariabilidadAlt;
        public TablaEstacionalidad _AveEstacionalidadAlt; // Esto es una tabla de 12 meses
        public float _AveDuracionAlt;
        // Extras
        public float _Ave2TNat;
        public float _Ave2TAlt;

        // Parametros de Sequia
        public float[] _SeqMagnitudNat;
        public float[] _SeqVariabilidadNat;
        public TablaEstacionalidad _SeqEstacionalidadNat;
        public float[] _SeqDuracionNat;
        public TablaEstacionalidad _SeqDuracionCerosMesNat;
        // 
        public float[] _SeqMagnitudAlt;
        public float[] _SeqVariabilidadAlt;
        public TablaEstacionalidad _SeqEstacionalidadAlt;
        public float[] _SeqDuracionAlt;
        public TablaEstacionalidad _SeqDuracionCerosMesAlt;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // Indices Habituales
        public Indices[] _IndicesHabituales;
        public Indices[] _IndicesHabitualesAgregados;
        public Indices _IndiceM3Agregados;
        public Indices _IndiceV3Agregados;
        public Indices[] _IndicesAvenidas;
        public float[] _IndicesAvenidasI16Meses;
        public float[] _IndicesAvenidasI16MesesInversos;
        public Indices[] _IndicesSequias;
        public float[] _IndicesSequiasI23Meses;
        public float[] _IndicesSequiasI23MesesInversos;
        public float[] _IndicesSequiasI24Meses;
        public float[] _IndicesSequiasI24MesesInversos;

        // Dim _IndiceHabitualI3 As Indices
        public float[] _IndiceIAG;
        public float _IndiceIAG_Agregados;
        public float _IndiceIAG_Ave;
        public float _IndiceIAG_Seq;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public float[] _percentil10;
        public float[] _percentil90;
        public float[] _medianaMenNat;
        public float[] _medianaMenAlt;
        public int[] _mesesQueCumplen;
        public float _percentil10Anual;
        public float _percentil90Anual;
        public float _medianaAnualNat;
        public float _medianaAnualAlt;
        public int _anyosQueCumplen;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public float _1QMin;
        public float _7QMin;
        public float _15QMin;
        public float[] _7QRetorno;
        public float[] _10QRetorno;
        public float[] _mnQ;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */        // Datos auxiliares
        public TablaCQC _TablaCQCNat;
        public TablaCQC _TablaCQCAlt;
        // Dim _TablaEstaNat As TablaEstacionalidad
        public int[] _nDiasNulosNat;
        public int[] _nDiasNulosAlt;
        // Para calcular el i23 modificado
        // Dim _i23Simplificado As Boolean = False
        // Para los calculos de indices agregados
        public TablaFrecuencias _TablaFrecuenciaMaxMin;
        internal SerieCaudalMensualCurvaClasificado CurvaClasificadaMensualCaudalNatural;
        internal SerieCaudalMensualCurvaClasificado CurvaClasificadaMensualCaudalAlterada;

        public List<SerieAportacionMensualClasificada> MensualCaracterizadaNatural { get; internal set; }
        public List<SerieAportacionMensualClasificada> MensualCaracterizadaAlterada { get; internal set; }
        public IAH2 IAH2 { get; internal set; }
    }
}
