using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using global::XPTable.Models;
using IAHRIS.BBDD;
using IAHRIS.Calculo;

namespace IAHRIS.Rellenar
{
    class RellenarForm
    {
        private OleDbDataBase _cMDB;
        private TestFechas _Fechas;

        public RellenarForm(OleDbDataBase MDB)
        {
            _cMDB = MDB;
            _Fechas = new TestFechas(_cMDB);
        }

        public void RellenarAlteracionesFromProj(ref ComboBox combo, int id)
        {
            // Sacar nombre de las listas
            var ds = _cMDB.RellenarDataSet("Listas", "Select COD_alteracion,ID_Alteracion FROM Vista_AlteracionFull WHERE ID_Proyecto = " + id + " order by cod_alteracion ASC");
            combo.Items.Clear();
            combo.Text = string.Empty;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                combo.Items.Add(it);
            }
            if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        public void RellenarAlteraciones(ref ComboBox combo, int id)
        {
            // Sacar nombre de las listas
            var ds = _cMDB.RellenarDataSet("Listas", "Select COD_alteracion,ID_Alteracion FROM [Alteracion] WHERE id_punto=" + id + " order by cod_alteracion ASC");
            combo.Items.Clear();
            combo.Text = string.Empty;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                combo.Items.Add(it);
            }
            if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        public void RellenarAlteraciones(ref ListBox listbox, int id)
        {
            // Sacar nombre de las listas
            var ds = _cMDB.RellenarDataSet("Listas", "Select COD_alteracion, ID_Alteracion FROM [Alteracion] WHERE id_punto=" + id + " order by cod_alteracion ASC");
            listbox.Items.Clear();
            // listbox.Text = String.Empty

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                listbox.Items.Add(it);
            }
            // If (listbox.Items.Count > 0) Then
            // listbox.SelectedIndex = 0
            // End If
        }

        public void RellenarPuntos(ref ListBox listbox)
        {
            var ds = _cMDB.RellenarDataSet("Puntos", "Select Clave_punto, ID_Punto From Punto order by clave_punto asc");

            // Dim dr As DataRow
            listbox.Items.Clear(); // Limpiar el listbox
            foreach (DataRow dr in ds.Tables[0].Rows)
            // Console.WriteLine(dr("nombre").ToString())
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                listbox.Items.Add(it);
            }
        }

        public void RellenarPuntos(ref CheckedListBox checklistbox)
        {
            var ds = _cMDB.RellenarDataSet("Puntos", "Select Clave_punto,ID_punto From Punto order by clave_punto asc");

            // Dim dr As DataRow
            checklistbox.Items.Clear(); // Limpiar el listbox
            foreach (DataRow dr in ds.Tables[0].Rows)
            // Console.WriteLine(dr("nombre").ToString())
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                checklistbox.Items.Add(it);
            }
        }

        public void RellenarPuntos(ref ListBox listbox, int id_proy)
        {
            DataSet ds;
            //if (id_proy == -1)
            //{
            //    ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] ORDER BY clave_punto ASC");
            //}
            //else
            //{
            ds = _cMDB.RellenarDataSet("Puntos", "SELECT Clave_punto, ID_punto FROM [Punto] WHERE ID_proyecto=" + id_proy + " ORDER BY clave_punto ASC");
            //}

            listbox.Items.Clear(); // Limpiar el listbox
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                listbox.Items.Add(it);
            }
        }

        public void RellenarPuntos(ref ComboBox cmb, int id_proy)
        {
            DataSet ds;
            //if (id_proy == -1)
            //{
            //    ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] ORDER BY clave_punto ASC");
            //}
            //else
            //{
                ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] WHERE ID_proyecto=" + id_proy + " ORDER BY clave_punto ASC");
            //}

            cmb.Items.Clear(); // Limpiar el listbox
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr["Clave_punto"].ToString(),(int) dr["ID_punto"]);
                cmb.Items.Add(it);
            }
        }

        public void RellenarPuntos(ref CheckedListBox clistbox, int id_proy)
        {
            DataSet ds;
            //if (id_proy == -1)
            //{
            //    ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] ORDER BY clave_punto ASC");
            //}
            //else
            //{
                ds = _cMDB.RellenarDataSet("Puntos", "SELECT Clave_punto,ID_Punto FROM [Punto] WHERE ID_proyecto=" + id_proy + " ORDER BY clave_punto ASC");
            //}

            clistbox.Items.Clear(); // Limpiar el listbox
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                clistbox.Items.Add(it);
            }
        }

        public void RellenarListas(ref ComboBox combo, int id)
        {
            // Sacar nombre de las listas
            var ds = _cMDB.RellenarDataSet("Listas", "Select nombre.ID_Lista FROM [Lista] WHERE id_punto=" + id + "");
            combo.Items.Clear();
            combo.Text = string.Empty;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                combo.Items.Add(it);
            }
            if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        public void RellenarListas(ref ListBox listbox, int id)
        {
            // Sacar nombre de las listas
            DataSet ds;
            //if (id != -1)
            //{
                ds = _cMDB.RellenarDataSet("Listas", "Select nombre,ID_Lista FROM [Lista] WHERE id_punto=" + id + "");
            //}
            //else
            //{
            //    ds = _cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista]");
            //}

            listbox.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(), (int)dr[1]);
                listbox.Items.Add(it);
            }
        }

        public void RellenarListas(ref bool haySerieDiaria, ref bool haySerieMensual, ref ComboBox comboAltD, int id, string stNone)
        {

            // Sacar que listas Naturales hay
            var dsNat = _cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista] WHERE id_punto=" + id + " AND tipo_lista=true AND tipo_fechas=true");
            if (dsNat.Tables[0].Rows.Count > 0)
            {
                haySerieDiaria = true;
            }
            else
            {
                haySerieDiaria = false;
            }

            // Sacar que lista Alteradas hay
            dsNat = _cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista] WHERE id_punto=" + id + " AND tipo_lista=true AND tipo_fechas=false");
            if (dsNat.Tables[0].Rows.Count > 0)
            {
                haySerieMensual = true;
            }
            else
            {
                haySerieMensual = false;
            }


            // Sacar alteraciones y rellenar el combobox
            // Dim dsAlt As DataSet = Me._cMDB.RellenarDataSet("Alt", "SELECT DISTINCT id_alteracion FROM [Lista] WHERE id_punto=" & id & " AND tipo_lista=false")
            var dsAlt = _cMDB.RellenarDataSet("Alt", "SELECT DISTINCT id_alteracion, COD_Alteracion FROM [alteracion] WHERE id_punto=" + id);
            int idAlt;
            // Dim dsAltaux As DataSet
            // Dim dralt As DataRow

            comboAltD.Items.Clear();
            comboAltD.Items.Add(stNone);
            foreach (DataRow dr in dsAlt.Tables[0].Rows)
            {
                idAlt = int.Parse(Conversions.ToString(dr["id_alteracion"]));
                // dsAltaux = Me._cMDB.RellenarDataSet("AltAux", "SELECT COD_Alteracion FROM [Alteracion] WHERE id_alteracion=" & idAlt)
                // OJO ERROR
                // If (dsAltaux.Tables(0).Rows.Count = 0) Then
                // MessageBox.Show("No se encuentra la alteración en el sistema, pero si existe una lista en ella... (?)")
                // Exit Sub
                // End If

                // dralt = dsAltaux.Tables(0).Rows(0)
                ComboItem it = new ComboItem(dr["COD_Alteracion"].ToString(), idAlt);
                comboAltD.Items.Add(it);
            }

            if (comboAltD.Items.Count > 0)
            {
                comboAltD.SelectedIndex = 0;
            }
        }

        public TestFechas.Simulacion RellenarXPTable(ref Table tabla, int idalt, int idpunto, bool usarCoe, bool usarCoeDiaria)
        {
            DataSet dsAltD;
            DataSet dsAltM;
            DataSet dsNatD;
            DataSet dsNatM;
            DateTime fechaI;
            DateTime fechaF;
            int añoMin = 99999;
            int añoMax = 0;
            int i, j;
            int[] añosInterAlt = null;
            int[] añosInterNat = null;
            int[] añosInterCoe = null;
            TestFechas.EstadoLista validosND = default;
            TestFechas.EstadoLista validosNM = default;
            TestFechas.EstadoLista validosAD = default;
            TestFechas.EstadoLista validosAM = default;
            TestFechas.Coetaniedad coeD = default;
            TestFechas.Coetaniedad coeM = default;
            TestFechas.Simulacion salida = default;
            salida.idListas = new int[4];

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++ Logica de Calculo +++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            validosND.Año = null;
            validosND.validos = null;
            validosNM.Año = null;
            validosNM.validos = null;

            // Sacar datos validos en diarios
            dsNatD = _cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" + idpunto + " AND Tipo_Lista=true AND tipo_fechas=true");
            if (dsNatD.Tables[0].Rows.Count > 0)
            {
                var dr = dsNatD.Tables[0].Rows[0];
                fechaI = DateTime.Parse(Conversions.ToString(dr["fecha_ini"]));
                fechaF = DateTime.Parse(Conversions.ToString(dr["fecha_fin"]));
                if (añoMin > fechaI.Year)
                {
                    añoMin = fechaI.Year;
                }

                if (añoMax < fechaF.Year)
                {
                    añoMax = fechaF.Year;
                }

                validosND = _Fechas.TestDiasAno("Valor", "fecha", fechaI, fechaF, Conversions.ToInteger(dr["id_lista"]));
                salida.idListas[0] = Conversions.ToInteger(dr["id_lista"]);
            }

            // Sacar años validos mensuales
            dsNatM = _cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" + idpunto + " AND Tipo_Lista=true AND tipo_fechas=false");
            if (dsNatM.Tables[0].Rows.Count > 0)
            {
                var dr = dsNatM.Tables[0].Rows[0];
                fechaI = DateTime.Parse(Conversions.ToString(dr["fecha_ini"]));
                fechaF = DateTime.Parse(Conversions.ToString(dr["fecha_fin"]));
                if (añoMin > fechaI.Year)
                {
                    añoMin = fechaI.Year;
                }

                if (añoMax < fechaF.Year)
                {
                    añoMax = fechaF.Year;
                }

                validosNM = _Fechas.TestMesAño("Valor", "fecha", fechaI, fechaF, Conversions.ToInteger(dr["id_lista"]));
                salida.idListas[2] = Conversions.ToInteger(dr["id_lista"]);
            }

            // Calcular posibles interpolaciones de diario a mensual y rellanar la lista que nos
            // indica que hay posibles interpolaciones
            // ----
            // Si no hay validos en diario no hay nada que interpolar
            if (validosND.nValidos != 0)
            {
                // No hay datos mensuales, interpolo todo
                if (validosNM.nValidos == 0)
                {
                    var loopTo = validosND.Año.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        if (validosND.validos[i])
                        {
                            if (añosInterNat is null)
                            {
                                añosInterNat = new int[1];
                                añosInterNat[0] = validosND.Año[i];
                            }
                            else
                            {
                                Array.Resize(ref añosInterNat, añosInterNat.Length + 1);
                                añosInterNat[añosInterNat.Length - 1] = validosND.Año[i];
                            }
                        }
                    }
                }
                else
                {
                    // Hay datos mensuales asi que tengo que comprobar si es necesario interpolar
                    var loopTo1 = validosND.Año.Length - 1;
                    for (i = 0; i <= loopTo1; i++)
                    {
                        int pos = Array.BinarySearch(validosNM.Año, validosND.Año[i]);
                        // Si el año no es valido o no existe en la lista de Natural Mensual
                        if (pos < 0)
                        {
                            if (validosND.validos[i])
                            {
                                if (añosInterNat is null)
                                {
                                    añosInterNat = new int[1];
                                    añosInterNat[0] = validosND.Año[i];
                                }
                                else
                                {
                                    Array.Resize(ref añosInterNat, añosInterNat.Length + 1);
                                    añosInterNat[añosInterNat.Length - 1] = validosND.Año[i];
                                }
                            }
                        }
                        else if (!validosNM.validos[pos])
                        {
                            if (validosND.validos[i])
                            {
                                if (añosInterNat is null)
                                {
                                    añosInterNat = new int[1];
                                    añosInterNat[0] = validosND.Año[i];
                                }
                                else
                                {
                                    Array.Resize(ref añosInterNat, añosInterNat.Length + 1);
                                    añosInterNat[añosInterNat.Length - 1] = validosND.Año[i];
                                }
                            }
                        }
                    }
                }
            }

            if (idalt != -1)
            {
                dsAltD = _cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" + idpunto + " AND Tipo_Lista=false AND tipo_fechas=true AND id_alteracion=" + idalt);
                dsAltM = _cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" + idpunto + " AND Tipo_Lista=false AND tipo_fechas=false AND id_alteracion=" + idalt);
                DataRow dr;
                if (dsAltD.Tables[0].Rows.Count > 0)
                {
                    dr = dsAltD.Tables[0].Rows[0];
                    fechaI = DateTime.Parse(Conversions.ToString(dr["fecha_ini"]));
                    fechaF = DateTime.Parse(Conversions.ToString(dr["fecha_fin"]));
                    if (añoMin > fechaI.Year)
                    {
                        añoMin = fechaI.Year;
                    }

                    if (añoMax < fechaF.Year)
                    {
                        añoMax = fechaF.Year;
                    }

                    validosAD = _Fechas.TestDiasAno("Valor", "fecha", fechaI, fechaF, Conversions.ToInteger(dr["id_lista"]));
                    salida.idListas[1] = Conversions.ToInteger(dr["id_lista"]);
                }

                if (dsAltM.Tables[0].Rows.Count > 0)
                {
                    dr = dsAltM.Tables[0].Rows[0];
                    fechaI = DateTime.Parse(Conversions.ToString(dr["fecha_ini"]));
                    fechaF = DateTime.Parse(Conversions.ToString(dr["fecha_fin"]));
                    if (añoMin > fechaI.Year)
                    {
                        añoMin = fechaI.Year;
                    }

                    if (añoMax < fechaF.Year)
                    {
                        añoMax = fechaF.Year;
                    }

                    validosAM = _Fechas.TestMesAño("Valor", "fecha", fechaI, fechaF, Conversions.ToInteger(dr["id_lista"]));
                    salida.idListas[3] = Conversions.ToInteger(dr["id_lista"]);
                }

                // Buscar posibles interpolaciones
                // 
                if (validosAD.nValidos != 0)
                {
                    // No hay datos mensuales, interpolo todo
                    if (validosAM.nValidos == 0)
                    {
                        var loopTo2 = validosAD.Año.Length - 1;
                        for (i = 0; i <= loopTo2; i++)
                        {
                            if (validosAD.validos[i])
                            {
                                if (añosInterAlt is null)
                                {
                                    añosInterAlt = new int[1];
                                    añosInterAlt[0] = validosAD.Año[i];
                                }
                                else
                                {
                                    Array.Resize(ref añosInterAlt, añosInterAlt.Length + 1);
                                    añosInterAlt[añosInterAlt.Length - 1] = validosAD.Año[i];
                                }
                            }
                        }
                    }
                    else
                    {
                        // Hay datos mensuales asi que tengo que comprobar si es necesario interpolar
                        var loopTo3 = validosAD.Año.Length - 1;
                        for (i = 0; i <= loopTo3; i++)
                        {
                            int pos = Array.BinarySearch(validosAM.Año, validosAD.Año[i]);
                            // Si el año no es valido o no existe en la lista de Natural Mensual
                            if (pos < 0)
                            {
                                if (validosAD.validos[i])
                                {
                                    if (añosInterAlt is null)
                                    {
                                        añosInterAlt = new int[1];
                                        añosInterAlt[0] = validosAD.Año[i];
                                    }
                                    else
                                    {
                                        Array.Resize(ref añosInterAlt, añosInterAlt.Length + 1);
                                        añosInterAlt[añosInterAlt.Length - 1] = validosAD.Año[i];
                                    }
                                }
                            }
                            else if (!validosAM.validos[pos])
                            {
                                if (validosAD.validos[i])
                                {
                                    if (añosInterAlt is null)
                                    {
                                        añosInterAlt = new int[1];
                                        añosInterAlt[0] = validosAD.Año[i];
                                    }
                                    else
                                    {
                                        Array.Resize(ref añosInterAlt, añosInterAlt.Length + 1);
                                        añosInterAlt[añosInterAlt.Length - 1] = validosAD.Año[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Calcular coetaniedad
            coeD = _Fechas.calcularcoetaniedad(validosND, validosAD, null, null);
            coeM = _Fechas.calcularcoetaniedad(validosNM, validosAM, añosInterNat, añosInterAlt);
            salida.añosInterAlt = añosInterAlt;
            salida.añosInterNat = añosInterNat;
            // salida.añosInterCoe = añosInterCoe

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++ Rellenar el XPTable +++++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // Años que usare en el calculo
            salida.añosParaCalculo = new TestFechas.añosCalculo[4];
            for (i = 0; i <= 3; i++)
                salida.añosParaCalculo[i].nAños = 0;

            // Borro la tabla
            tabla.TableModel.Rows.Clear();

            // -----------------------------
            // Bucle de años
            // -----------------------------
            var loopTo4 = añoMax - 1;
            for (i = añoMin; i <= loopTo4; i++)
            {

                // Definicion de las celdas
                Cell[] cells;
                cells = new Cell[11];
                for (j = 0; j <= 10; j++)
                {
                    cells[j] = new Cell("");
                    cells[j].Checked = false;
                }

                cells[0] = new Cell(i.ToString() + "-" + (i + 1).ToString()); // Celda del año


                // +++++++++++++++++
                // Naturales diarios
                // +++++++++++++++++
                if (validosND.Año is null)
                {
                    cells[6] = new Cell("NS", My.Resources.Resources.delete2);
                }
                else
                {
                    int pos = Array.BinarySearch(validosND.Año, i);
                    if (pos > -1)
                    {
                        if (validosND.validos[pos])
                        {
                            cells[6] = new Cell("C", My.Resources.Resources.check);
                        }
                        else
                        {
                            cells[6] = new Cell("I", My.Resources.Resources.no_information);
                        }
                    }
                    else
                    {
                        cells[6] = new Cell("SD", My.Resources.Resources.delete2);
                    }
                }

                // ++++++++++++++++
                // Alterados Diario
                // ++++++++++++++++
                if (validosAD.Año is null)
                {
                    cells[8] = new Cell("NS", My.Resources.Resources.delete2);
                }
                else
                {
                    int pos = Array.BinarySearch(validosAD.Año, i);
                    if (pos > -1)
                    {
                        if (validosAD.validos[pos])
                        {
                            cells[8] = new Cell("C", My.Resources.Resources.check);
                        }
                        else
                        {
                            cells[8] = new Cell("I", My.Resources.Resources.no_information);
                        }
                    }
                    else
                    {
                        cells[8] = new Cell("SD", My.Resources.Resources.delete2);
                    }
                }

                // +++++++++++++++++++++++++++
                // +++++++ Coe Diarios +++++++
                // +++++++++++++++++++++++++++
                if (coeD.nCoetaneos == 0)
                {
                    cells[10] = new Cell("", My.Resources.Resources.delete2);
                }
                else
                {
                    int pos = Array.BinarySearch(coeD.añosCoetaneos, i);
                    if (pos > -1)
                    {
                        cells[10] = new Cell("CO", My.Resources.Resources.check);
                    }
                    else
                    {
                        cells[10] = new Cell("NCO", My.Resources.Resources.delete2);
                    }
                }

                // +++++++++++++++++++
                // Naturales mensuales
                // +++++++++++++++++++
                // Incluido la posible interpolacion de los diarios!!!
                if (validosNM.Año is null)
                {
                    if (cells[6].Text == "C")
                    {
                        // If (añosInterNat Is Nothing) Then
                        // ReDim añosInterNat(0)
                        // añosInterNat(0) = i
                        // Else
                        // ReDim Preserve añosInterNat(añosInterNat.Length)
                        // añosInterNat(añosInterNat.Length - 1) = i
                        // End If
                        cells[1] = new Cell("CD", My.Resources.Resources.check);
                    }
                    else
                    {
                        cells[1] = new Cell("SD", My.Resources.Resources.delete2);
                    }
                }
                else
                {
                    int pos = Array.BinarySearch(validosNM.Año, i);
                    if (pos > -1)
                    {
                        if (validosNM.validos[pos])
                        {
                            cells[1] = new Cell("C", My.Resources.Resources.check);
                        }
                        else if (validosND.validos is object)
                        {
                            pos = Array.BinarySearch(validosND.Año, i);
                            if (pos > -1)
                            {
                                if (validosND.validos[pos])
                                {
                                    // If (añosInterNat Is Nothing) Then
                                    // ReDim añosInterNat(0)
                                    // añosInterNat(0) = i
                                    // Else
                                    // ReDim Preserve añosInterNat(añosInterNat.Length)
                                    // añosInterNat(añosInterNat.Length - 1) = i
                                    // End If
                                    cells[1] = new Cell("CD", My.Resources.Resources.check);
                                }
                                else
                                {
                                    cells[1] = new Cell("I", My.Resources.Resources.no_information);
                                }
                            }
                            else
                            {
                                cells[1] = new Cell("I", My.Resources.Resources.no_information);
                            }
                        }
                        else
                        {
                            cells[1] = new Cell("SD", My.Resources.Resources.delete2);
                        }
                    }
                    else if (cells[6].Text == "C")
                    {
                        // If (añosInterNat Is Nothing) Then
                        // ReDim añosInterNat(0)
                        // añosInterNat(0) = i
                        // Else
                        // ReDim Preserve añosInterNat(añosInterNat.Length)
                        // añosInterNat(añosInterNat.Length - 1) = i
                        // End If
                        cells[1] = new Cell("CD", My.Resources.Resources.check);
                    }
                    else
                    {
                        cells[1] = new Cell("SD", My.Resources.Resources.delete2);
                    }
                }

                // +++++++++++++++++
                // Alterados Mensual
                // +++++++++++++++++
                if (validosAM.Año is null)
                {
                    if (cells[8].Text == "C")
                    {
                        // If (añosInterAlt Is Nothing) Then
                        // ReDim añosInterAlt(0)
                        // añosInterAlt(0) = i
                        // Else
                        // ReDim Preserve añosInterAlt(añosInterAlt.Length)
                        // añosInterAlt(añosInterAlt.Length - 1) = i
                        // End If
                        cells[3] = new Cell("CD", My.Resources.Resources.check);
                    }
                    else
                    {
                        cells[3] = new Cell("SD", My.Resources.Resources.delete2);
                    }
                }
                else
                {
                    int pos = Array.BinarySearch(validosAM.Año, i);
                    if (pos > -1)
                    {
                        if (validosAM.validos[pos])
                        {
                            cells[3] = new Cell("C", My.Resources.Resources.check);
                        }
                        // If (Not validosND.validos Is Nothing) Then
                        // pos = Array.BinarySearch(validosND.Año, i)
                        else if (validosAD.validos is object)
                        {
                            pos = Array.BinarySearch(validosAD.Año, i);
                            if (pos > -1)
                            {
                                if (validosAD.validos[pos])
                                {
                                    // If (añosInterAlt Is Nothing) Then
                                    // ReDim añosInterAlt(0)
                                    // añosInterAlt(0) = i
                                    // Else
                                    // ReDim Preserve añosInterAlt(añosInterAlt.Length)
                                    // añosInterAlt(añosInterAlt.Length - 1) = i
                                    // End If
                                    cells[3] = new Cell("CD", My.Resources.Resources.check);
                                }
                                else
                                {
                                    cells[3] = new Cell("I", My.Resources.Resources.no_information);
                                }
                            }
                            else
                            {
                                cells[3] = new Cell("I", My.Resources.Resources.no_information);
                            }
                        }
                        else
                        {
                            cells[3] = new Cell("SD", My.Resources.Resources.delete2);
                        }
                    }
                    // pos = Array.BinarySearch(validosAD.Año, i)
                    else if (cells[8].Text == "C")
                    {
                        // If (validosAD.validos(pos)) Then
                        // If (añosInterAlt Is Nothing) Then
                        // ReDim añosInterAlt(0)
                        // añosInterAlt(0) = i
                        // Else
                        // ReDim Preserve añosInterAlt(añosInterAlt.Length)
                        // añosInterAlt(añosInterAlt.Length - 1) = i
                        // End If
                        cells[3] = new Cell("CD", My.Resources.Resources.check);
                    }
                    else
                    {
                        cells[3] = new Cell("SD", My.Resources.Resources.delete2);
                        // Else
                        // cells(8) = New Cell("Sin datos", My.Resources.delete2)
                        // End If
                    }
                }


                // +++++++++++++++++++
                // Coetaneos mensuales
                // +++++++++++++++++++
                if (coeM.nCoetaneos == 0)
                {
                    if (cells[1].Text == "CD" & cells[3].Text == "CD")
                    {
                        // If (añosInterCoe Is Nothing) Then
                        // ReDim añosInterCoe(0)
                        // añosInterCoe(0) = i
                        // Else
                        // ReDim Preserve añosInterCoe(añosInterCoe.Length)
                        // añosInterCoe(añosInterCoe.Length - 1) = i
                        // End If
                        cells[5] = new Cell("CO", My.Resources.Resources.check);
                    }
                    else if (cells[1].Text == "C" & cells[3].Text == "CD")
                    {
                        // If (añosInterCoe Is Nothing) Then
                        // ReDim añosInterCoe(0)
                        // añosInterCoe(0) = i
                        // Else
                        // ReDim Preserve añosInterCoe(añosInterCoe.Length)
                        // añosInterCoe(añosInterCoe.Length - 1) = i
                        // End If
                        cells[5] = new Cell("CO", My.Resources.Resources.check);
                    }
                    else if (cells[1].Text == "CD" & cells[3].Text == "C")
                    {
                        // If (añosInterCoe Is Nothing) Then
                        // ReDim añosInterCoe(0)
                        // añosInterCoe(0) = i
                        // Else
                        // ReDim Preserve añosInterCoe(añosInterCoe.Length)
                        // añosInterCoe(añosInterCoe.Length - 1) = i
                        // End If
                        cells[5] = new Cell("CO", My.Resources.Resources.check);
                    }
                    else
                    {
                        cells[5] = new Cell("", My.Resources.Resources.delete2);
                    }
                }
                else
                {
                    int pos = Array.BinarySearch(coeM.añosCoetaneos, i);
                    if (pos > -1)
                    {
                        cells[5] = new Cell("CO", My.Resources.Resources.check);
                    }
                    else
                    {
                        cells[5] = new Cell("NCO", My.Resources.Resources.delete2);
                    }
                }

                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // +++++++++++ Ahora estudiar que años se usan +++++++++++++++++++++
                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                int nAñosIntN = 0;
                int nAñosIntA = 0;
                if (añosInterNat is object)
                {
                    nAñosIntN = añosInterNat.Length;
                }

                if (añosInterAlt is object)
                {
                    nAñosIntA = añosInterAlt.Length;
                }

                // La serie donde se almacenan los años que participan en el calculo
                // tienen el siguiente orden:
                // 0 -> Nat diaria
                // 1 -> Alt diaria
                // 2 -> Nat mensual
                // 3 -> Alt mensual

                if (validosNM.nValidos + nAñosIntN >= 15)
                {
                    if (validosND.nValidos >= 15)
                    {
                        if (usarCoeDiaria)
                        {
                            if (cells[10].Text == "CO")
                            {
                                if (cells[6].Text == "C")
                                {
                                    cells[7].Text = "";
                                    cells[7].Checked = true;
                                    // Añadir a la lista de años para calcular
                                    Array.Resize(ref salida.añosParaCalculo[0].año, salida.añosParaCalculo[0].nAños + 1);
                                    salida.añosParaCalculo[0].año[salida.añosParaCalculo[0].nAños] = i;
                                    salida.añosParaCalculo[0].nAños = salida.añosParaCalculo[0].nAños + 1;
                                }

                                if (validosAD.nValidos >= 15)
                                {
                                    if (cells[8].Text == "C")
                                    {
                                        cells[9].Text = "";
                                        cells[9].Checked = true;
                                        // Añadir a la lista de años para calcular
                                        Array.Resize(ref salida.añosParaCalculo[1].año, salida.añosParaCalculo[1].nAños + 1);
                                        salida.añosParaCalculo[1].año[salida.añosParaCalculo[1].nAños] = i;
                                        salida.añosParaCalculo[1].nAños = salida.añosParaCalculo[1].nAños + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cells[6].Text == "C")
                            {
                                cells[7].Text = "";
                                cells[7].Checked = true;
                                // Añadir a la lista de años para calcular
                                Array.Resize(ref salida.añosParaCalculo[0].año, salida.añosParaCalculo[0].nAños + 1);
                                salida.añosParaCalculo[0].año[salida.añosParaCalculo[0].nAños] = i;
                                salida.añosParaCalculo[0].nAños = salida.añosParaCalculo[0].nAños + 1;
                            }

                            if (validosAD.nValidos >= 15)
                            {
                                if (cells[8].Text == "C")
                                {
                                    cells[9].Text = "";
                                    cells[9].Checked = true;
                                    // Añadir a la lista de años para calcular
                                    Array.Resize(ref salida.añosParaCalculo[1].año, salida.añosParaCalculo[1].nAños + 1);
                                    salida.añosParaCalculo[1].año[salida.añosParaCalculo[1].nAños] = i;
                                    salida.añosParaCalculo[1].nAños = salida.añosParaCalculo[1].nAños + 1;
                                }
                            }
                        }
                    }

                    if (usarCoe)
                    {
                        if (cells[5].Text == "CO")
                        {
                            if (cells[1].Text == "C" | cells[1].Text == "CD")
                            {
                                cells[2].Text = "";
                                cells[2].Checked = true;
                                // Añadir a la lista de años para calcular
                                Array.Resize(ref salida.añosParaCalculo[2].año, salida.añosParaCalculo[2].nAños + 1);
                                salida.añosParaCalculo[2].año[salida.añosParaCalculo[2].nAños] = i;
                                salida.añosParaCalculo[2].nAños = salida.añosParaCalculo[2].nAños + 1;
                            }

                            if (validosAM.nValidos + nAñosIntA >= 15)
                            {
                                if (cells[3].Text == "C" | cells[3].Text == "CD")
                                {
                                    cells[4].Text = "";
                                    cells[4].Checked = true;
                                    // Añadir a la lista de años para calcular
                                    Array.Resize(ref salida.añosParaCalculo[3].año, salida.añosParaCalculo[3].nAños + 1);
                                    salida.añosParaCalculo[3].año[salida.añosParaCalculo[3].nAños] = i;
                                    salida.añosParaCalculo[3].nAños = salida.añosParaCalculo[3].nAños + 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cells[1].Text == "C" | cells[1].Text == "CD")
                        {
                            cells[2].Text = "";
                            cells[2].Checked = true;
                            // Añadir a la lista de años para calcular
                            Array.Resize(ref salida.añosParaCalculo[2].año, salida.añosParaCalculo[2].nAños + 1);
                            salida.añosParaCalculo[2].año[salida.añosParaCalculo[2].nAños] = i;
                            salida.añosParaCalculo[2].nAños = salida.añosParaCalculo[2].nAños + 1;
                        }

                        if (validosAM.nValidos + nAñosIntA >= 15)
                        {
                            if (cells[3].Text == "C" | cells[3].Text == "CD")
                            {
                                cells[4].Text = "";
                                cells[4].Checked = true;
                                // Añadir a la lista de años para calcular
                                Array.Resize(ref salida.añosParaCalculo[3].año, salida.añosParaCalculo[3].nAños + 1);
                                salida.añosParaCalculo[3].año[salida.añosParaCalculo[3].nAños] = i;
                                salida.añosParaCalculo[3].nAños = salida.añosParaCalculo[3].nAños + 1;
                            }
                        }
                    }
                }
                else if (validosAM.nValidos + nAñosIntA >= 15)
                {
                    if (cells[6].Text == "C")
                    {
                        cells[7].Text = "";
                        cells[7].Checked = true;
                        // Añadir a la lista de años para calcular
                        Array.Resize(ref salida.añosParaCalculo[0].año, salida.añosParaCalculo[0].nAños + 1);
                        salida.añosParaCalculo[0].año[salida.añosParaCalculo[0].nAños] = i;
                        salida.añosParaCalculo[0].nAños = salida.añosParaCalculo[0].nAños + 1;
                    }

                    if (validosAD.nValidos >= 15)
                    {
                        if (cells[8].Text == "C")
                        {
                            cells[9].Text = "";
                            cells[9].Checked = true;
                            // Añadir a la lista de años para calcular
                            Array.Resize(ref salida.añosParaCalculo[1].año, salida.añosParaCalculo[1].nAños + 1);
                            salida.añosParaCalculo[1].año[salida.añosParaCalculo[1].nAños] = i;
                            salida.añosParaCalculo[1].nAños = salida.añosParaCalculo[1].nAños + 1;
                        }
                    }
                    if (cells[1].Text == "C" | cells[1].Text == "CD")
                    {
                        cells[2].Text = "";
                        cells[2].Checked = true;
                        // Añadir a la lista de años para calcular
                        Array.Resize(ref salida.añosParaCalculo[2].año, salida.añosParaCalculo[2].nAños + 1);
                        salida.añosParaCalculo[2].año[salida.añosParaCalculo[2].nAños] = i;
                        salida.añosParaCalculo[2].nAños = salida.añosParaCalculo[2].nAños + 1;
                    }

                    if (validosAM.nValidos + nAñosIntA >= 15)
                    {
                        if (cells[3].Text == "C" | cells[3].Text == "CD")
                        {
                            cells[4].Text = "";
                            cells[4].Checked = true;
                            // Añadir a la lista de años para calcular
                            Array.Resize(ref salida.añosParaCalculo[3].año, salida.añosParaCalculo[3].nAños + 1);
                            salida.añosParaCalculo[3].año[salida.añosParaCalculo[3].nAños] = i;
                            salida.añosParaCalculo[3].nAños = salida.añosParaCalculo[3].nAños + 1;
                        }
                    }
                }

                 
                tabla.TableModel.Rows.Add(new Row(cells));
            }

            // Crear la salida de la simulacion
            salida.fechaINI = añoMin;
            salida.fechaFIN = añoMax;
            salida.usarCoe = usarCoe;
            salida.usarCoeDiara = usarCoeDiaria;
            var c = new TestFechas.Coetaniedad[] { coeD, coeM };
            var v = new TestFechas.EstadoLista[] { validosND, validosAD, validosNM, validosAM };
            salida.coe = c;
            salida.listas = v;
            return salida;
        }

        public void RellenarProyectos(ref ComboBox combo)
        {
            var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT nombre, ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");
            combo.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            { 
                ComboItem item = new ComboItem(dr[0].ToString(), (int)dr[1]) ;
                combo.Items.Add(item);
            }
            if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        public void RellenarProyectos(ref ComboBox combo, int id)
        {
            var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT nombre, ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");
            int index;
            int i;
            combo.Items.Clear();
            index = 0;
            i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr[0].ToString(),(int) dr[1]);
                combo.Items.Add(it);
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(dr["ID_Proyecto"], id, false)))
                {
                    index = i;
                }

                i += 1;
            }

            if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = index;
            }
        }

        public int RellenarProyectosDesc(ref Label label, int id)
        {
            if (id == -1)
            {
                return -1;
            }

            var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT descripcion, ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");
            var dr = ds.Tables[0].Rows[id];
            if (label is object)
            {
                label.Text = dr[0].ToString();
            }

            return Conversions.ToInteger(dr[1]);
        }
    }

    public class ComboItem
    {
        string text;
        int id;

        public ComboItem(string text, int id)
        {
            this.text = text ?? throw new ArgumentNullException(nameof(text));
            this.id = id;
        }

        public string Text { get => text; set => text = value; }
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            return text;
        }
    }
}