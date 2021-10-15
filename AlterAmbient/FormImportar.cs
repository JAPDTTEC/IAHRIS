using System;
using System.Data;
using System.Windows.Forms;
using IAHRIS.BBDD;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    public partial class FormImportar
    {
        public FormImportar()
        {
            InitializeComponent();
            _btnExaminar.Name = "btnExaminar";
            _btnCrearProy.Name = "btnCrearProy";
            _btnImportarPuntos.Name = "btnImportarPuntos";
            _btnImportarProyecto.Name = "btnImportarProyecto";
            _cmbProyectos.Name = "cmbProyectos";
        }

        public FormImportar(OleDbDataBase MDB)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            _cMDB = MDB;

            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _btnExaminar.Name = "btnExaminar";
            _btnCrearProy.Name = "btnCrearProy";
            _btnImportarPuntos.Name = "btnImportarPuntos";
            _btnImportarProyecto.Name = "btnImportarProyecto";
            _cmbProyectos.Name = "cmbProyectos";
        }

        private string _rutaMDB;
        private BBDD.OleDbDataBase _cMDBImportar;
        private BBDD.OleDbDataBase _cMDB;
        private string _sError = "";
        private Rellenar.RellenarForm _rellenarImportar;
        private Rellenar.RellenarForm _rellenar;
        private int _id_proy_selec;
        private int _id_proy_selec_punto;
        private MultiLangXML.MultiIdiomasXML _traductor;

        private enum TIPO_BBDD
        {
            VERSION_OK,
            VERSION_ANT_V1,
            VERSION_DESC
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Base de datos (*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            cmbProyectos.Items.Clear();
            chklstPuntos.Items.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _rutaMDB = openFileDialog1.FileName;
                _cMDBImportar = new BBDD.OleDbDataBase("BaseImportar", _rutaMDB);
                txtRuta.Text = _rutaMDB;
                _rellenarImportar = new Rellenar.RellenarForm(_cMDBImportar);
                _rellenar = new Rellenar.RellenarForm(_cMDB);
                try
                {
                    TIPO_BBDD tipobbdd;
                    tipobbdd = ValidarVersionBBDD(ref _sError);
                    if (tipobbdd == TIPO_BBDD.VERSION_DESC)
                    {
                        MessageBox.Show(_sError, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    else if (tipobbdd == TIPO_BBDD.VERSION_OK)
                    {
                        // Importar por proyectos/puntos
                        var argcombo = cmbProyectos;
                        _rellenarImportar.RellenarProyectos(ref argcombo);
                        cmbProyectos = argcombo;
                        var argcombo1 = cmbProyPuntos;
                        _rellenar.RellenarProyectos(ref argcombo1);
                        cmbProyPuntos = argcombo1;
                        gbProyectos.Enabled = true;
                        gbPuntos.Enabled = true;
                    }
                    else if (tipobbdd == TIPO_BBDD.VERSION_ANT_V1)
                    {
                        // Importar por puntos
                        // Se deshabilita los proyectos ya que no existen en la versión 1
                        gbProyectos.Enabled = false;
                        gbPuntos.Enabled = true;
                        MessageBox.Show(_sError, "Actualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var argchecklistbox = chklstPuntos;
                        _rellenarImportar.RellenarPuntos(ref argchecklistbox);
                        chklstPuntos = argchecklistbox;
                        var argcombo2 = cmbProyPuntos;
                        _rellenar.RellenarProyectos(ref argcombo2);
                        cmbProyPuntos = argcombo2;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_sError + Constants.vbCrLf + ex.Message, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private int GetVersionBBDD()
        {
            string sVer;
            try
            {
                var ds = _cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Punto]");
                ds = _cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Lista]");
                ds = _cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Alteracion]");
                ds = _cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Valor]");
            }
            catch (Exception ex)
            {
                return -1;
            }

            try
            {
                var ds = _cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Configuracion]");
                var dr = ds.Tables[0].Rows[0];
                sVer = Conversions.ToString(dr["version"]); 
            }
            catch (Exception ex)
            {
                sVer = 1.ToString();
            }

            return Conversions.ToInteger(sVer);
        }

        /// <summary>
    /// Esta función nos dice que tipo de versión tenemos al importar la bbdd
    /// </summary>
    /// <param name="message">Mensaje de error o de información</param>
    /// <returns></returns>
    /// <remarks></remarks>
        private TIPO_BBDD ValidarVersionBBDD(ref string message)
        {
            switch (GetVersionBBDD())
            {
                case 1:
                    {
                        message = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDBv1_2");
                        return TIPO_BBDD.VERSION_ANT_V1;
                    }

                case 2:
                    {
                        message = "";
                        return TIPO_BBDD.VERSION_OK;
                    }

                default:
                    {
                        message = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDBNotValid");
                        return TIPO_BBDD.VERSION_DESC;
                    }
            }
        }

        private void FormImportar_Load(object sender, EventArgs e)
        {
            gbPuntos.Enabled = false;
            gbProyectos.Enabled = false;
            PictureBox1.Visible = false;
            Application.DoEvents();
        }

        private void cmbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label arglabel = null;
            _id_proy_selec = _rellenarImportar.RellenarProyectosDesc(ref arglabel, cmbProyectos.SelectedIndex);
            var argclistbox = chklstPuntos;
            _rellenarImportar.RellenarPuntos(ref argclistbox, _id_proy_selec);
            chklstPuntos = argclistbox;
        }

        private void btnImportarProyecto_Click(object sender, EventArgs e)
        {
            if (cmbProyectos.SelectedIndex == -1)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            _cMDB.ComenzarTransaccion();

            // Deshabilitar formulario
            // Me.Enabled = False
            btnExaminar.Enabled = false;
            gbProyectos.Enabled = false;
            gbPuntos.Enabled = false;
            PictureBox1.Visible = true;
            var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT COUNT(*) FROM [Proyecto] WHERE nombre=\"" + cmbProyectos.SelectedItem.ToString() + "\"");
            string nombreProy = cmbProyectos.SelectedItem.ToString();

            // Comprobar que el proyecto no tiene el nombre de otro ya en la bbdd
            var dr = ds.Tables[0].Rows[0];
            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(dr[0], 0, false)))
            {
                string newName = Interaction.InputBox(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNameProject"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(cmbProyectos.SelectedItem, "_"), DateTime.Now.ToShortDateString())));

                if (string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strInvalidName"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _cMDB.TerminarTransaccion(false);
                    btnExaminar.Enabled = true;
                    gbProyectos.Enabled = true;
                    gbPuntos.Enabled = true;
                    PictureBox1.Visible = false;
                    Cursor = Cursors.Default;
                    return;
                }

                nombreProy = newName;
            }

            lbInfo.Items.Add("[INFO] Importando de " + _rutaMDB);
            lbInfo.Items.Add(Constants.vbTab + "el proyecto " + nombreProy);
            lbInfo.TopIndex = lbInfo.Items.Count - 1;
            Application.DoEvents();

            // Insertar el proyecto
            // --------------------
            ds = _cMDBImportar.RellenarDataSet("Proyectos", "SELECT ID_Proyecto, nombre, descripcion FROM [Proyecto] WHERE nombre=\"" + cmbProyectos.SelectedItem.ToString() + "\"");
            dr = ds.Tables[0].Rows[0];
            bool ok = _cMDB.InsertarRegistro("Proyecto", new string[] { "nombre", "descripcion" }, new string[] { nombreProy, (string)dr["descripcion"] });
            if (!ok)
            {
                lbInfo.Items.Add("[ERROR] El nombre del proyecto ya existe en la base de datos");
                lbInfo.TopIndex = lbInfo.Items.Count - 1;
                btnExaminar.Enabled = true;
                gbProyectos.Enabled = true;
                gbPuntos.Enabled = true;
                PictureBox1.Visible = false;
                _cMDB.TerminarTransaccion(false);
                Cursor = Cursors.Default;
                return;
            }

            int idproyectoImportar = Conversions.ToInteger(dr["ID_Proyecto"]);

            // Insertar los puntos
            // -------------------
            // Sacar el id de proyecto del nuevo proyecto
            ds = _cMDB.RellenarDataSet("Proyectos", "SELECT TOP 1 ID_Proyecto FROM [Proyecto] ORDER BY ID_Proyecto DESC");
            dr = ds.Tables[0].Rows[0];
            int idproyecto = Conversions.ToInteger(dr["ID_Proyecto"]);
            lbInfo.Items.Add("[INFO] Proyecto importado con id " + idproyecto);
            lbInfo.TopIndex = lbInfo.Items.Count - 1;
            Application.DoEvents();
            ds = _cMDBImportar.RellenarDataSet("Puntos", "SELECT * FROM [Punto] WHERE ID_Proyecto=" + idproyectoImportar);
            DataSet dsPunto;
            DataSet dsListaImportar;
            DataSet dsLista;
            DataSet dsAlteracionImportar;
            DataSet dsAlteracion;
            DataSet dsValorImportar;
            var camposPunto = new string[] { "Clave_punto", "Nombre", "ID_proyecto", "mesInicio" };
            var camposLista = new string[] { "Tipo_Lista", "Nombre", "Tipo_fechas", "Fecha_Ini", "Fecha_Fin", "Formato_Fecha", "ID_Punto", "ID_Alteracion" };
            var camposAlteracion = new string[] { "COD_Alteracion", "Nombre", "ID_Punto" };
            var camposValor = new string[] { "ID_Lista", "Valor", "Fecha" };
            string[] valoresPunto;
            string[] valoresAlteracion;
            string[] valoresLista;
            string[][] valoresValor = null;
            string clavePuntoImportar;
            string idPuntoImportar;
            string idPunto;
            string claveAlteracion;
            string idAlteracion;
            string idLista;
            string idListaImportar;
            lbInfo.Items.Add("[INFO] Se importan " + ds.Tables[0].Rows.Count + " puntos asociados al proyecto");
            lbInfo.TopIndex = lbInfo.Items.Count - 1;
            Application.DoEvents();

            // ¿Como meter la información asociada?:
            // Se mete un punto -> luego una lista -> si es alteracion -> luego valores
            // -> Volvemos a empezar
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                // ¿El punto ya tiene un nombre igual al que se va ha insertar?
                dsPunto = _cMDB.RellenarDataSet("Punto", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("SELECT Count(*) FROM [Punto] WHERE Clave_Punto=\"", row["Clave_punto"]), "\"")));
                clavePuntoImportar = Conversions.ToString(row["Clave_punto"]);
                dr = dsPunto.Tables[0].Rows[0];
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(dr[0], 0, false)))
                {
                    clavePuntoImportar += "_" + idproyecto;
                }

                valoresPunto = new string[] { clavePuntoImportar, (string)row["Nombre"], idproyecto.ToString(), "1" };
                ok = _cMDB.InsertarRegistro("Punto", camposPunto, valoresPunto);
                if (!ok)
                {
                    lbInfo.Items.Add("[ERROR] Fallo al insertar el punto.");
                    _cMDB.TerminarTransaccion(false);
                    btnExaminar.Enabled = true;
                    gbProyectos.Enabled = true;
                    gbPuntos.Enabled = true;
                    PictureBox1.Visible = false;
                    Cursor = Cursors.Default;
                    return;
                }

                dsPunto = _cMDB.RellenarDataSet("Proyectos", "SELECT TOP 1 ID_Punto FROM [Punto] ORDER BY ID_Punto DESC");
                dr = dsPunto.Tables[0].Rows[0];
                idPunto = Conversions.ToString(dr["ID_Punto"]);
                idPuntoImportar = Conversions.ToString(row["ID_Punto"]);
                lbInfo.Items.Add("[INFO] Punto " + clavePuntoImportar + " esta siendo importado");
                lbInfo.TopIndex = lbInfo.Items.Count - 1;
                Application.DoEvents();

                // Importar las listas
                // Tengo que comprobar si es o no una alteracion
                dsListaImportar = _cMDBImportar.RellenarDataSet("Lista", "SELECT * FROM Lista WHERE ID_Punto=" + idPuntoImportar + " ORDER BY ID_Lista DESC");
                lbInfo.Items.Add("[INFO] Punto " + clavePuntoImportar + " tiene asociadas " + dsListaImportar.Tables[0].Rows.Count + " series");
                lbInfo.TopIndex = lbInfo.Items.Count - 1;
                Application.DoEvents();

                // Sacar el mes de inicio de simulación que será el usado en la simulación 
                var fechaIni = DateTime.Parse(Conversions.ToString(dsListaImportar.Tables[0].Rows[0]["Fecha_Ini"]));
                ok = Conversions.ToBoolean(_cMDB.EjecutarSQL("UPDATE [Punto] SET mesInicio=" + fechaIni.Month + " WHERE ID_Punto=" + idPunto));
                if (ok)
                {
                }
                else
                {
                    lbInfo.Items.Add("[ERROR] Al actualizar el mes de inicio de año del punto.");
                    _cMDB.TerminarTransaccion(false);
                    btnExaminar.Enabled = true;
                    gbProyectos.Enabled = true;
                    gbPuntos.Enabled = true;
                    PictureBox1.Visible = false;
                    Cursor = Cursors.Default;
                    return;
                }

                foreach (DataRow rowLista in dsListaImportar.Tables[0].Rows)
                {

                    // Lista original
                    idListaImportar = Conversions.ToString(rowLista["ID_Lista"]);
                    idAlteracion = "0";

                    // Es una alteración o no. Se tiene que meter la alteracion
                    if (rowLista["Tipo_lista"].ToString().ToLower() != "true")
                    {
                        dsAlteracionImportar = _cMDBImportar.RellenarDataSet("Alteracion", Conversions.ToString(Operators.ConcatenateObject("SELECT * FROM [Alteracion] WHERE ID_Punto=" + idPuntoImportar + " AND ID_Alteracion=", rowLista["ID_Alteracion"])));
                        var drAlt = dsAlteracionImportar.Tables[0].Rows[0];
                        claveAlteracion = Conversions.ToString(drAlt["COD_Alteracion"]);
                        dsAlteracion = _cMDB.RellenarDataSet("Alteracion", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("SELECT Count(*) FROM [Alteracion] WHERE COD_Alteracion=\"", drAlt["COD_Alteracion"]), "\"")));
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(dsAlteracion.Tables[0].Rows[0][0], 0, false)))
                        {
                            claveAlteracion += "_" + idproyecto;
                        }

                        valoresAlteracion = new string[] { claveAlteracion, (string)dsAlteracionImportar.Tables[0].Rows[0]["Nombre"], idPunto };

                        ok = _cMDB.InsertarRegistro("Alteracion", camposAlteracion, valoresAlteracion);
                        if (!ok)
                        {
                            lbInfo.Items.Add("[ERROR] Fallo al insertar la alteración.");
                            _cMDB.TerminarTransaccion(false);
                            btnExaminar.Enabled = true;
                            gbProyectos.Enabled = true;
                            gbPuntos.Enabled = true;
                            PictureBox1.Visible = false;
                            Cursor = Cursors.Default;
                            return;
                        }

                        dsAlteracion = _cMDB.RellenarDataSet("Alteracion", "SELECT TOP 1 ID_Alteracion FROM [Alteracion] ORDER BY ID_Alteracion DESC");
                        idAlteracion = Conversions.ToString(dsAlteracion.Tables[0].Rows[0][0]);
                        lbInfo.Items.Add("[INFO] La serie que se importa es una serie alterada");
                        lbInfo.TopIndex = lbInfo.Items.Count - 1;
                        Application.DoEvents();
                    }

                    // Insertar la lista
                    valoresLista = new string[] { (string)rowLista["Tipo_lista"], (string)Operators.ConcatenateObject(rowLista["Nombre"], idproyecto), (string)rowLista["Tipo_fechas"], (string)rowLista["Fecha_Ini"], (string)rowLista["Fecha_Fin"], (string)rowLista["Formato_Fecha"], idPunto, idAlteracion };

                    ok = _cMDB.InsertarRegistro("Lista", camposLista, valoresLista);
                    if (!ok)
                    {
                        lbInfo.Items.Add("[ERROR] Fallo al insertar la lista.");
                        _cMDB.TerminarTransaccion(false);
                        btnExaminar.Enabled = true;
                        gbProyectos.Enabled = true;
                        gbPuntos.Enabled = true;
                        PictureBox1.Visible = false;
                        Cursor = Cursors.Default;
                        return;
                    }

                    dsLista = _cMDB.RellenarDataSet("Lista", "SELECT TOP 1 ID_Lista FROM [Lista] ORDER BY ID_Lista DESC");
                    idLista = Conversions.ToString(dsLista.Tables[0].Rows[0][0]);

                    // Incluir los valores de la lista
                    dsValorImportar = _cMDBImportar.RellenarDataSet("Valor", "SELECT * FROM [Valor] WHERE ID_Lista=" + idListaImportar + " ORDER BY ID_Valor ASC");
                    valoresValor = null;
                    lbInfo.Items.Add("[INFO] La serie tiene " + dsValorImportar.Tables[0].Rows.Count + " valores");
                    lbInfo.TopIndex = lbInfo.Items.Count - 1;
                    Application.DoEvents();
                    int inc = 0;
                    int resto = 0;
                    foreach (DataRow rowValor in dsValorImportar.Tables[0].Rows)
                    {
                        if (valoresValor is null)
                        {
                            valoresValor = new string[1][];
                        }
                        else
                        {
                            Array.Resize(ref valoresValor, valoresValor.Length + 1);
                        }

                        valoresValor[valoresValor.Length - 1] = new string[] { idLista, (string)rowValor["Valor"], (string)rowValor["Fecha"] };
                        inc += 1;
                        Math.DivRem(inc, 100, out resto);
                        if (resto == 0)
                        {
                            PictureBox1.Invalidate();
                            PictureBox1.Refresh();
                            Application.DoEvents();
                        }
                    }

                    ok = _cMDB.InsertarRegistros("Valor", camposValor, valoresValor);
                    if (!ok)
                    {
                        lbInfo.Items.Add("[ERROR] Fallo al insertar los valores.");
                        _cMDB.TerminarTransaccion(false);
                        btnExaminar.Enabled = true;
                        gbProyectos.Enabled = true;
                        gbPuntos.Enabled = true;
                        PictureBox1.Visible = false;
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                lbInfo.Items.Add("[INFO] Punto " + clavePuntoImportar + " importado");
                lbInfo.TopIndex = lbInfo.Items.Count - 1;
                Application.DoEvents();
            }

            _cMDB.TerminarTransaccion(true);
            Cursor = Cursors.Default;
            Application.DoEvents();
            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strImportProjEnd") + "\"" + nombreProy + "\"", _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOperationEnd"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Me.Enabled = True

            btnExaminar.Enabled = true;
            gbProyectos.Enabled = true;
            gbPuntos.Enabled = true;
            PictureBox1.Visible = false;
            Close();
        }

        private void btnImportarPuntos_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _cMDB.ComenzarTransaccion();

            // Me.Enabled = False
            btnExaminar.Enabled = false;
            gbProyectos.Enabled = false;
            gbPuntos.Enabled = false;
            PictureBox1.Visible = true;
            string[] ptosSelec = null;
            for (int i = 0, loopTo = chklstPuntos.Items.Count - 1; i <= loopTo; i++)
            {
                if (chklstPuntos.GetItemCheckState(i) == CheckState.Checked)
                {
                    if (ptosSelec is null)
                    {
                        ptosSelec = new string[1];
                    }
                    else
                    {
                        Array.Resize(ref ptosSelec, ptosSelec.Length + 1);
                    }

                    ptosSelec[ptosSelec.Length - 1] = Conversions.ToString(chklstPuntos.Items[i]);
                }
            }

            if (ptosSelec is null)
            {
                lbInfo.Items.Add("[ERROR] No hay punto seleccionado.");
                _cMDB.TerminarTransaccion(false);
                btnExaminar.Enabled = true;
                gbProyectos.Enabled = true;
                gbPuntos.Enabled = true;
                PictureBox1.Visible = false;
                Cursor = Cursors.Default;
                return;
            }

            Label arglabel = null;
            _id_proy_selec_punto = _rellenar.RellenarProyectosDesc(ref arglabel, cmbProyPuntos.SelectedIndex);
            bool ok;
            var camposPunto = new string[] { "Clave_punto", "Nombre", "ID_proyecto", "mesInicio" };
            var camposLista = new string[] { "Tipo_Lista", "Nombre", "Tipo_fechas", "Fecha_Ini", "Fecha_Fin", "Formato_Fecha", "ID_Punto", "ID_Alteracion" };
            var camposAlteracion = new string[] { "COD_Alteracion", "Nombre", "ID_Punto" };
            var camposValor = new string[] { "ID_Lista", "Valor", "Fecha" };
            string[] valoresPunto;
            string[] valoresAlteracion;
            string[] valoresLista;
            string[][] valoresValor = null;
            DataRow dr;
            DataRow drImportar;
            DataSet dsPunto;
            DataSet dsPuntoImportar;
            DataSet dsLista;
            DataSet dsListaImportar;
            DataSet dsAlteracion;
            DataSet dsAlteracionImportar;
            DataSet dsValorImportar;
            string clavePuntoImportar;
            string claveAlteracion;
            int idPunto;
            int idPuntoImportar;
            int idAlteracion;
            int idLista;
            int idListaImportar;

            // Importar cada uno de los puntos
            foreach (string strPunto in ptosSelec)
            {
                dsPunto = _cMDB.RellenarDataSet("Punto", "SELECT Count(*) FROM [Punto] WHERE Clave_Punto=\"" + strPunto + "\"");
                dsPuntoImportar = _cMDBImportar.RellenarDataSet("Punto", "SELECT * FROM [Punto] WHERE Clave_Punto=\"" + strPunto + "\"");
                drImportar = dsPuntoImportar.Tables[0].Rows[0];
                clavePuntoImportar = strPunto;
                dr = dsPunto.Tables[0].Rows[0];
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(dr[0], 0, false)))
                {
                    clavePuntoImportar += "_" + _id_proy_selec_punto;
                    if (clavePuntoImportar.Length > 12)
                    {
                        clavePuntoImportar = clavePuntoImportar.Substring(0, 10) + "_" + _id_proy_selec_punto;
                    }
                }

                valoresPunto = new string[] { clavePuntoImportar, (string)drImportar["Nombre"], _id_proy_selec_punto.ToString(), "1" };
                ok = _cMDB.InsertarRegistro("Punto", camposPunto, valoresPunto);
                if (!ok)
                {
                    lbInfo.Items.Add("[ERROR] Error al insertar el punto.");
                    _cMDB.TerminarTransaccion(false);
                    btnExaminar.Enabled = true;
                    gbProyectos.Enabled = true;
                    gbPuntos.Enabled = true;
                    PictureBox1.Visible = false;
                    Cursor = Cursors.Default;
                    return;
                }

                dsPunto = _cMDB.RellenarDataSet("Proyectos", "SELECT TOP 1 ID_Punto FROM [Punto] ORDER BY ID_Punto DESC");
                dr = dsPunto.Tables[0].Rows[0];
                idPunto = Conversions.ToInteger(dr["ID_Punto"]);
                idPuntoImportar = Conversions.ToInteger(drImportar["ID_Punto"]);
                lbInfo.Items.Add("[INFO] Punto " + clavePuntoImportar + " esta siendo importado");
                lbInfo.TopIndex = lbInfo.Items.Count - 1;
                Application.DoEvents();

                // Importar las listas
                // Tengo que comprobar si es o no una alteracion
                dsListaImportar = _cMDBImportar.RellenarDataSet("Lista", "SELECT * FROM Lista WHERE ID_Punto=" + idPuntoImportar + " ORDER BY ID_Lista DESC");
                lbInfo.Items.Add("[INFO] Punto " + clavePuntoImportar + " tiene asociadas " + dsListaImportar.Tables[0].Rows.Count + " series");
                lbInfo.TopIndex = lbInfo.Items.Count - 1;
                Application.DoEvents();
                foreach (DataRow rowLista in dsListaImportar.Tables[0].Rows)
                {

                    // Lista original
                    idListaImportar = Conversions.ToInteger(rowLista["ID_Lista"]);
                    idAlteracion = Conversions.ToInteger("0");

                    // Es una alteración o no. Se tiene que meter la alteracion
                    if (rowLista["Tipo_lista"].ToString().ToLower() != "true")
                    {
                        dsAlteracionImportar = _cMDBImportar.RellenarDataSet("Alteracion", Conversions.ToString(Operators.ConcatenateObject("SELECT * FROM [Alteracion] WHERE ID_Punto=" + idPuntoImportar + " AND ID_Alteracion=", rowLista["ID_Alteracion"])));
                        var drAlt = dsAlteracionImportar.Tables[0].Rows[0];
                        claveAlteracion = Conversions.ToString(drAlt["COD_Alteracion"]);
                        dsAlteracion = _cMDB.RellenarDataSet("Alteracion", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("SELECT Count(*) FROM [Alteracion] WHERE COD_Alteracion=\"", drAlt["COD_Alteracion"]), "\"")));
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(dsAlteracion.Tables[0].Rows[0][0], 0, false)))
                        {
                            claveAlteracion += "_" + _id_proy_selec_punto;
                        }

                        valoresAlteracion = new string[] { claveAlteracion, (string)dsAlteracionImportar.Tables[0].Rows[0]["Nombre"], idPunto.ToString() };

                        ok = _cMDB.InsertarRegistro("Alteracion", camposAlteracion, valoresAlteracion);
                        if (!ok)
                        {
                            lbInfo.Items.Add("[ERROR] Fallo al insertar la alteración.");
                            _cMDB.TerminarTransaccion(false);
                            btnExaminar.Enabled = true;
                            gbProyectos.Enabled = true;
                            gbPuntos.Enabled = true;
                            PictureBox1.Visible = false;
                            Cursor = Cursors.Default;
                            return;
                        }

                        dsAlteracion = _cMDB.RellenarDataSet("Alteracion", "SELECT TOP 1 ID_Alteracion FROM [Alteracion] ORDER BY ID_Alteracion DESC");
                        idAlteracion = Conversions.ToInteger(dsAlteracion.Tables[0].Rows[0][0]);
                        lbInfo.Items.Add("[INFO] La serie que se importa es una serie alterada");
                        lbInfo.TopIndex = lbInfo.Items.Count - 1;
                        Application.DoEvents();
                    }

                    // Insertar la lista
                    valoresLista = new string[] { (string)rowLista["Tipo_lista"], (string)Operators.ConcatenateObject(rowLista["Nombre"], _id_proy_selec_punto), (string)rowLista["Tipo_fechas"], (string)rowLista["Fecha_Ini"], (string)rowLista["Fecha_Fin"], (string)rowLista["Formato_Fecha"], idPunto.ToString(), idAlteracion.ToString() };

                    ok = _cMDB.InsertarRegistro("Lista", camposLista, valoresLista);
                    if (!ok)
                    {
                        lbInfo.Items.Add("[ERROR] Fallo al insertar la lista.");
                        _cMDB.TerminarTransaccion(false);
                        btnExaminar.Enabled = true;
                        gbProyectos.Enabled = true;
                        gbPuntos.Enabled = true;
                        PictureBox1.Visible = false;
                        Cursor = Cursors.Default;
                        return;
                    }

                    dsLista = _cMDB.RellenarDataSet("Lista", "SELECT TOP 1 ID_Lista FROM [Lista] ORDER BY ID_Lista DESC");
                    idLista = Conversions.ToInteger(dsLista.Tables[0].Rows[0][0]);

                    // Incluir los valores de la lista
                    dsValorImportar = _cMDBImportar.RellenarDataSet("Valor", "SELECT * FROM [Valor] WHERE ID_Lista=" + idListaImportar + " ORDER BY ID_Valor ASC");
                    valoresValor = null;
                    lbInfo.Items.Add("[INFO] La serie tiene " + dsValorImportar.Tables[0].Rows.Count + " valores");
                    lbInfo.TopIndex = lbInfo.Items.Count - 1;
                    Application.DoEvents();
                    int inc = 0;
                    int resto;
                    foreach (DataRow rowValor in dsValorImportar.Tables[0].Rows)
                    {
                        if (valoresValor is null)
                        {
                            valoresValor = new string[1][];
                        }
                        else
                        {
                            Array.Resize(ref valoresValor, valoresValor.Length + 1);
                        }

                        valoresValor[valoresValor.Length - 1] = new string[] { idLista.ToString(), (string)rowValor["Valor"], (string)rowValor["Fecha"] };
                        inc += 1;
                        Math.DivRem(inc, 100, out resto);
                        if (resto == 0)
                        {
                            PictureBox1.Invalidate();
                            PictureBox1.Refresh();
                            Application.DoEvents();
                        }
                    }

                    ok = _cMDB.InsertarRegistros("Valor", camposValor, valoresValor);
                    if (!ok)
                    {
                        lbInfo.Items.Add("[ERROR] Fallo al insertar la valores.");
                        _cMDB.TerminarTransaccion(false);
                        btnExaminar.Enabled = true;
                        gbProyectos.Enabled = true;
                        gbPuntos.Enabled = true;
                        PictureBox1.Visible = false;
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                lbInfo.Items.Add("[INFO] Punto " + clavePuntoImportar + " importado");
                lbInfo.TopIndex = lbInfo.Items.Count - 1;
                Application.DoEvents();
            }

            _cMDB.TerminarTransaccion(true);
            Cursor = Cursors.Default;
            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strImportPointsEnd") + "\"" + cmbProyPuntos.SelectedItem.ToString() + "\"", _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOperationEnd"), MessageBoxButtons.OK, MessageBoxIcon.Information);


            // Me.Enabled = True
            btnExaminar.Enabled = true;
            gbProyectos.Enabled = true;
            gbPuntos.Enabled = true;
            PictureBox1.Visible = false;
            Close();
        }

        private void btnCrearProy_Click(object sender, EventArgs e)
        {
            var fanadirproyecto = new FormAnadirProyecto(_cMDB);
            fanadirproyecto.ShowDialog();
            var argchecklistbox = chklstPuntos;
            _rellenarImportar.RellenarPuntos(ref argchecklistbox);
            chklstPuntos = argchecklistbox;
            var argcombo = cmbProyPuntos;
            _rellenar.RellenarProyectos(ref argcombo);
            cmbProyPuntos = argcombo;
        }
    }
}