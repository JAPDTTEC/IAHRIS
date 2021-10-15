using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAHRIS.Calculo
{
    public class IAHRISConstants
    {
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int xlDiagonalDown = 5;
        public const int xlPasteFormats = -4122;
        public const int xlCenter = -4108;
        public const int xlGeneral = 1;
        public const int xlUnderlineStyleNone = -4142;
        public const int xlAutomatic = -4105;
        public const int xlNone = -4142;
        public const int xlContinuous = 1;
        public const int xlThin = 2;
        public const int xlDiagonalUp = 6;
        public const int xlEdgeLeft = 7;
        public const int xlEdgeTop = 8;
        public const int xlEdgeBottom = 9;
        public const int xlEdgeRight = 10;
        public const int xlInsideVertical = 11;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
    public enum TIPOAÑO
    {
        HUMEDO,
        MEDIO,
        SECO,
        NotSet
    }

    public enum STRING_MES
    {
        OCT = 0,
        NOV,
        DIC,
        ENE,
        FEB,
        MAR,
        ABR,
        MAY,
        JUN,
        JUL,
        AGO,
        SEP
    }

    public enum STRING_MES_ORD
    {
        ENE = 0,
        FEB,
        MAR,
        ABR,
        MAY,
        JUN,
        JUL,
        AGO,
        SEP,
        OCT,
        NOV,
        DIC
    }

    public enum STRING_MES_COMPLETO
    {
        Enero = 0,
        Febrero,
        Marzo,
        Abril,
        Mayo,
        Junio,
        Julio,
        Agosto,
        Septiembre,
        Octubre,
        Noviembre,
        Diciembre
    }
    public struct SerieAnual
    {
        public int[] año;
        public float[] caudalAnual;
        public int nAños;
    }

    public struct SerieMensual
    {
        public DateTime[] mes;
        public float[] caudalMensual;
        public int nMeses;
        public int nAños;
    }

    public struct SerieDiaria
    {
        public DateTime[] dia;
        public float[] caudalDiaria;
        public int nAños;
    }

    public struct AportacionMensual
    {
        public DateTime[] mes;
        public float[] aportacion;
    }

    public struct AportacionAnual
    {
        public int[] año;
        public float[] aportacion;
        public TIPOAÑO[] tipo;
    }

    public struct GraficoAlterada
    {
        public int año;
        public TIPOAÑO tipo;
        public float apNat;
        public float apAlt;
        public float porcentaje;
    }

    public struct Indices
    {
        public float[] valor;
        public bool[] invertido;
        public bool[] indeterminacion;
        public bool calculado;
    }

    /// <summary>
    /// Datos necesarios para el analisis Kruskal-Wallis
    /// </summary>
    /// <remarks></remarks>
    public struct DatosKS
    {
        public float[][] datosMes;
        public TIPOAÑO[][] tipoMes;
        public TIPOAÑO[][] tipoMesKS;
    }

    /// <summary>
    /// Datos iniciales para realizar el calculo
    /// </summary>
    /// <remarks></remarks>
    public struct DatosCalculo
    {
        public TestFechas.Simulacion _simulacion;
        public string sNombre;
        public string sAlteracion;
        public SerieDiaria SerieNatDiaria;
        public SerieMensual SerieNatMensual;
        public SerieDiaria SerieAltDiaria;
        public SerieMensual SerieAltMensual;
        public int nAnyosCoe;
        public int mesInicio;
    }

    /// <summary>
    /// Tabla donde se almacena la tabla de calculos asociados a los percentiles
    /// </summary>
    /// <remarks></remarks>
    public struct TablaCQC
    {
        public float[] pe;
        public float[] dia;
        public float[][] caudales;
        public float[] añomedio;
    }



    public struct TablaFrecuencias
    {
        public float[] nat;
        public float[] alt;
        public float[] minNat;
        public float[] minAlt;
        public bool[] posMaxNat;
        public bool[] posMinNat;
        public bool[] posMaxAlt;
        public bool[] posMinAlt;
    }

    /// <summary>
    /// Tabla que representa la tabla que hay que mostrar en estacionalidad
    /// </summary>
    /// <remarks></remarks>
    public struct TablaEstacionalidad
    {
        public float[] ndias;
        public string[] mes;
        public float mediaAño;
    }


}

