using System;
using global::System.Data;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS.Calculo
{
    public class TestFechas
    {
        private BBDD.OleDbDataBase _cMDB;

        public struct EstadoLista
        {
            public bool[] validos;
            public int nValidos;
            public int[] Año;
        }

        public struct Coetaniedad
        {
            public int añoINI;
            public int añoFIN;
            public int tamaño;
            public int nCoetaneos;
            public int[] añosCoetaneos;
        }

        public struct añosCalculo
        {
            public int[] año;
            public int nAños;
        }
        public enum Tipologias
        {
            NONE,
            Tipo1,
            Tipo2,
            Tipo3,
            Tipo4,
            Tipo5,
            Tipo6,
            Tipo6A,
            Tipo6B,
            Tipo7,
            Tipo8,
            Tipo8A,
            Tipo8B
        }
        public struct Simulacion
        {
            public string sNombre;
            public string sAlteracion;
            public int fechaINI;
            public int fechaFIN;
            /// <summary>
            /// 0 -> Nat diaria
            /// 1 -> Alt diaria
            /// 2 -> Nat mensual
            /// 3 -> Alt mensual
            /// </summary>
            public EstadoLista[] listas;
            public int[] idListas;
            public Coetaniedad[] coe;
            public int[] añosInterNat;
            public int[] añosInterAlt;
            public int[] añosInterCoe;
            public bool usarCoe;
            public bool usarCoeDiara;
            public añosCalculo[] añosParaCalculo;
            public int añosCoetaneosTotales;
            public int mesInicio;
            public Tipologias Tipologia;
        }

        public struct GeneracionInformes
        {
            public bool inf1;
            public bool inf1a;
            public bool inf1b;
            public bool inf2;
            public bool inf2a;
            public bool inf3;
            public bool inf3a;
            public bool inf3b;
            public bool inf4;
            public bool inf4a;
            //public bool inf4b; // Nuevo 12/4/10
            public bool inf5;
            public bool inf5a;
            public bool inf5b;
            public bool inf6;
            public bool inf6a;
            public bool inf6b;
            public bool inf6c;
            public bool inf6d;
            public bool inf6e;
            public bool inf7a;
            public bool inf7b;
            public bool inf7c;
            public bool inf7d;
            public bool inf8;
            public bool inf8a; // Nuevo 12/4/10
            public bool inf8b; // Nuevo 12/4/10
            public bool inf8c; // Nuevo 12/4/10
            public bool inf8d; // Nuevo 12/4/10
            public bool inf9;
            public bool inf9a; // Nuevo 12/4/10
            public bool inf9b;
            public bool inf10a;
            public bool inf10b;
            public bool inf10c;
            public bool inf10d;
        }

        public TestFechas(BBDD.OleDbDataBase MDB)
        {
            _cMDB = MDB;
        }

        public EstadoLista TestDiasAno(string tabla, string campo, DateTime fechaInicial, DateTime fechaFinal, int idlista)
        {
            int i;
            // Dim dr As DataRow
            DateTime fechaINI;
            DateTime fechaFIN;
            int nRevisar;
            int[] año;
            bool[] valido;
            EstadoLista estado;
            int diasEnAno;


            // ¿Cuantos años hay que mirar?
            nRevisar = fechaFinal.Year - fechaInicial.Year;
            año = new int[nRevisar];
            valido = new bool[nRevisar];


            // Calculo las primeras fechas
            fechaFIN = fechaInicial.AddDays(-1);
            estado.nValidos = 0;
            var loopTo = nRevisar - 1;
            for (i = 0; i <= loopTo; i++)
            {
                fechaINI = fechaFIN.AddDays(1d);
                fechaFIN = fechaFIN.AddYears(1);
                if (DateTime.IsLeapYear(fechaFIN.Year) & fechaFIN.Month == 2)
                {
                    fechaFIN = fechaFIN.AddDays(1d);
                }
                // fechaFIN = fechaFIN.AddDays(-1)

                diasEnAno = 365;

                // quitar fecha y poner campo
                string sSQL = "SELECT " + campo + " FROM " + tabla + " WHERE " + campo + " BETWEEN #" + fechaINI.ToString("yyyy-MM-dd") + "# and #" + fechaFIN.ToString("yyyy-MM-dd") + "# AND id_lista=" + idlista + " ORDER BY " + campo + " DESC";
                var ds = _cMDB.RellenarDataSet("tabla", sSQL);

                // Si la fecha es de año Bisiesto, se suma un dia
                // ----------------------------------------------
                int posibleAñoBisiesto;
                if (fechaINI.Month <= 2)
                {
                    posibleAñoBisiesto = fechaINI.Year;
                }
                else
                {
                    posibleAñoBisiesto = fechaFIN.Year;
                }

                if (DateTime.IsLeapYear(posibleAñoBisiesto))
                {
                    diasEnAno = 366;
                }

                año[i] = fechaINI.Year;
                if (ds.Tables[0].Rows.Count == diasEnAno)
                {
                    valido[i] = true;
                    estado.nValidos = estado.nValidos + 1;
                }
                else
                {
                    valido[i] = false;
                }
            }

            estado.Año = año;
            estado.validos = valido;
            return estado;
        }

        public EstadoLista TestMesAño(string tabla, string campo, DateTime fechaInicial, DateTime fechaFinal, int idlista)
        {
            int i;
            EstadoLista estado;
            int nRevisar;
            int[] año;
            bool[] valido;
            DateTime fechaINI;
            DateTime fechaFIN;
            // Dim añoAct As Integer
            // Dim añoAnt As Integer
            // Dim mesAct As Integer
            // Dim ok As Boolean

            estado = default;
            nRevisar = fechaFinal.Year - fechaInicial.Year;
            año = new int[nRevisar];
            valido = new bool[nRevisar];
            fechaFIN = fechaInicial.AddDays(-1);
            estado.nValidos = 0;
            var loopTo = nRevisar - 1;
            for (i = 0; i <= loopTo; i++)
            {
                fechaINI = fechaFIN.AddDays(1d);
                fechaFIN = fechaFIN.AddYears(1);

                // quitar fecha y poner campo
                string sSQL = "SELECT " + campo + " FROM " + tabla + " WHERE " + campo + " BETWEEN #" + fechaINI.ToString("yyyy-MM-dd") + "# and #" + fechaFIN.ToString("yyyy-MM-dd") + "# AND id_lista=" + idlista + " ORDER BY " + campo + " DESC";
                var ds = _cMDB.RellenarDataSet("tabla", sSQL);
                if (ds.Tables[0].Rows.Count != 12)
                {
                    valido[i] = false;
                }
                else
                {
                    valido[i] = true;
                    estado.nValidos = estado.nValidos + 1;
                }

                año[i] = fechaINI.Year;
            }

            estado.Año = año;
            estado.validos = valido;
            return estado;
        }

        public bool ComprobarFechasCSV(bool tipoFechas, string fecha, ref DateTime fechaDate)
        {
            DateTime dt;
            if (tipoFechas)
            {
                try
                {
                    dt = DateTime.ParseExact(fecha, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    dt = DateTime.ParseExact(fecha, "MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    try
                    {
                        dt = DateTime.ParseExact(fecha, "M/yyyy", null);
                    }
                    catch (Exception ext)
                    {
                        return false;
                    }
                }
            }

            fechaDate = dt;
            return true;
        }

        public Coetaniedad calcularcoetaniedad(EstadoLista l1, EstadoLista l2, int[] interNat, int[] interAlt)
        {
            Coetaniedad salida;
            int i;

            // If (l1.Año Is Nothing) Or (l2.Año Is Nothing) Then
            // salida.nCoetaneos = 0
            // salida.tamaño = 0
            // salida.añosCoetaneos = Nothing
            // Return salida
            // End If

            salida.añoINI = 99999;
            salida.añoFIN = 0;
            salida.nCoetaneos = 0;
            salida.tamaño = 0;
            salida.añosCoetaneos = null;
            if (l1.Año is object)
            {
                if (l1.Año[l1.Año.Length - 1] > salida.añoFIN)
                {
                    salida.añoFIN = l1.Año[l1.Año.Length - 1];
                }

                if (l1.Año[0] < salida.añoINI)
                {
                    salida.añoINI = l1.Año[0];
                }
            }

            if (l2.Año is object)
            {
                if (l2.Año[l2.Año.Length - 1] > salida.añoFIN)
                {
                    salida.añoFIN = l2.Año[l2.Año.Length - 1];
                }

                if (l2.Año[0] < salida.añoINI)
                {
                    salida.añoINI = l2.Año[0];
                }
            }

            if (interNat is object)
            {
                if (interNat[interNat.Length - 1] > salida.añoFIN)
                {
                    salida.añoFIN = interNat[interNat.Length - 1];
                }

                if (interNat[0] < salida.añoINI)
                {
                    salida.añoINI = interNat[0];
                }
            }

            if (interAlt is object)
            {
                if (interAlt[interAlt.Length - 1] > salida.añoFIN)
                {
                    salida.añoFIN = interAlt[interAlt.Length - 1];
                }

                if (interAlt[0] < salida.añoINI)
                {
                    salida.añoINI = interAlt[0];
                }
            }

            // If (l1.Año(0) > l2.Año(0)) Then
            // salida.añoINI = l2.Año(0)
            // Else
            // salida.añoINI = l1.Año(0)
            // End If

            // If (l1.Año(l1.Año.Length - 1) > l2.Año(l1.Año.Length - 1)) Then
            // salida.añoFIN = l1.Año(l1.Año.Length - 1)
            // Else
            // salida.añoFIN = l2.Año(l1.Año.Length - 1)
            // End If

            salida.tamaño = salida.añoFIN - salida.añoINI + 1;

            // ReDim salida.añosCoetaneos(salida.tamaño - 1)
            salida.nCoetaneos = 0;
            var loopTo = salida.añoFIN;
            for (i = salida.añoINI; i <= loopTo; i++)
            {
                bool escoe = false;
                int pos;
                if (l1.Año is object)
                {
                    pos = Array.BinarySearch(l1.Año, i);
                }
                else
                {
                    pos = -1;
                }

                int pos2;
                if (l2.Año is object)
                {
                    pos2 = Array.BinarySearch(l2.Año, i);
                }
                else
                {
                    pos2 = -1;
                }
                // ¿Es el año NAT interpolado?
                int pos3;
                if (interNat is object)
                {
                    pos3 = Array.BinarySearch(interNat, i);
                }
                else
                {
                    pos3 = -1;
                }
                // ¿Es el año ALT interpolado?
                int pos4;
                if (interAlt is object)
                {
                    pos4 = Array.BinarySearch(interAlt, i);
                }
                else
                {
                    pos4 = -1;
                }

                if (pos > -1 & pos2 > -1)
                {
                    if (l1.validos[pos] & l2.validos[pos2])
                    {
                        escoe = true;
                    }
                }
                else if (pos3 > -1 & pos2 > -1)
                {
                    if (l2.validos[pos2])
                    {
                        escoe = true;
                    }
                }
                else if (pos > -1 & pos4 > -1)
                {
                    if (l1.validos[pos])
                    {
                        escoe = true;
                    }
                }
                else if (pos3 > -1 & pos4 > -1)
                {
                    escoe = true;
                }

                if (escoe)
                {
                    Array.Resize(ref salida.añosCoetaneos, salida.nCoetaneos + 1);
                    salida.añosCoetaneos[salida.nCoetaneos] = i;
                    salida.nCoetaneos = salida.nCoetaneos + 1;
                }
            }

            return salida;
        }

        public Coetaniedad CalcularCoetaniedad(int idPunto)
        {

            // Saco las lista asociadas a ese punto
            string sSQL = "SELECT id_Lista, fecha_ini, fecha_fin FROM [Lista] WHERE id_punto=" + idPunto;
            var ds = _cMDB.RellenarDataSet("listas", sSQL);
            DateTime fechaINI;
            DateTime fechaFIN;
            int idlista;
            Coetaniedad salida = default;
            EstadoLista[] estado;
            int i = 0;
            int j = 0;
            estado = new EstadoLista[ds.Tables[0].Rows.Count];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                fechaINI = DateTime.Parse(Conversions.ToString(dr["fecha_ini"]));
                fechaFIN = DateTime.Parse(Conversions.ToString(dr["fecha_fin"]));
                idlista = int.Parse(Conversions.ToString(dr["id_lista"]));
                estado[i] = TestDiasAno("valor", "fecha", fechaINI, fechaFIN, idlista);
                i = i + 1;
            }

            int tamaño = 0;
            int añoIni = 99999;
            int añoFin = 0;
            var loopTo = estado.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (añoIni > estado[i].Año[0])
                {
                    añoIni = estado[i].Año[0];
                }

                if (añoFin < estado[i].Año[estado[i].Año.Length - 1])
                {
                    añoFin = estado[i].Año[estado[i].Año.Length - 1];
                }
            }

            tamaño = añoFin - añoIni + 1;
            salida.añoINI = añoIni;
            salida.añoFIN = añoFin;
            salida.tamaño = tamaño;
            var loopTo1 = tamaño - 1;
            for (i = 0; i <= loopTo1; i++) // Recorro todos los años
            {
                bool coe = true;
                var loopTo2 = estado.Length - 1;
                for (j = 0; j <= loopTo2; j++)
                {
                    int pos = Array.BinarySearch(estado[j].Año, añoIni);
                    if (pos == -1)
                    {
                        coe = false;
                        break;
                    }
                    else
                    {
                        coe = coe & estado[j].validos[pos];
                        if (coe == false)
                        {
                            break;
                        }
                    }
                }

                if (coe)
                {
                    Array.Resize(ref salida.añosCoetaneos, salida.nCoetaneos + 1);
                    salida.nCoetaneos = salida.añosCoetaneos.Length;
                    salida.añosCoetaneos[salida.añosCoetaneos.Length - 1] = añoIni;
                }

                añoIni = añoIni + 1;
            }

            return salida;
        }
    }
}