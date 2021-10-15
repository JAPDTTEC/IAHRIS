using System;
using global::System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using global::XPTable.Models;
using IAHRIS.BBDD;
using IAHRIS.Calculo;
using IAHRIS.Rellenar;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS
{
    public partial class FormInicial
    {


        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public FormInicial()
        {

            // Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent();
            _AñadirProyectoToolStripMenuItem.Name = "AñadirProyectoToolStripMenuItem";
            _EliminarProyectoToolStripMenuItem.Name = "EliminarProyectoToolStripMenuItem";
            _AñadirPuntoToolStripMenuItem.Name = "AñadirPuntoToolStripMenuItem";
            _EliminarPuntoToolStripMenuItem.Name = "EliminarPuntoToolStripMenuItem";
            _AñadirAlteraciónToolStripMenuItem.Name = "AñadirAlteraciónToolStripMenuItem";
            _EliminarAlteraciónToolStripMenuItem.Name = "EliminarAlteraciónToolStripMenuItem";
            _AñadirListaToolStripMenuItem.Name = "AñadirListaToolStripMenuItem";
            _EliminarListaToolStripMenuItem.Name = "EliminarListaToolStripMenuItem";
            _ImportarToolStripMenuItem.Name = "ImportarToolStripMenuItem";
            _ExportarToolStripMenuItem.Name = "ExportarToolStripMenuItem";
            //_ManualesToolStripMenuItem.Name = "ManualesToolStripMenuItem";
            _lstboxPuntos.Name = "lstboxPuntos";
            _cmbMesInicio.Name = "cmbMesInicio";
            _chkboxUsarCoeDiaria.Name = "chkboxUsarCoeDiaria";
            _chkboxUsarCoe.Name = "chkboxUsarCoe";
            _cmbListaAlteradasDiarias.Name = "cmbListaAlteradasDiarias";
            _btnCalcular.Name = "btnCalcular";
            _cbProyectos.Name = "cbProyectos";

            // Agregue cualquier inicialización después de la llamada a InitializeComponent().

        }

        private string _RutaBBDD;
        private OleDbDataBase _cMDB;
        private Rellenar.RellenarForm _rellenar;
        private TestFechas _testFechas;
        private TestFechas.Simulacion _simulacion;
        private TestFechas.GeneracionInformes _informes;
        private ComboItem _PtoSeleccionado;
        private ComboItem _AltSeleccionada;
        private string _strPto;
        private string _strAlt;
        private string _sVersion = "v2.0 BETA";
        private int _id_proy_selec = -1;

        // Para poder centrar el formulario
        private int __alto;
        private int __ancho;
        private MultiLangXML.MultiIdiomasXML _traductor;
        private string _rutaXML;

        // Ñapa para no refrescar todo el formulario

        private enum TIPO_BBDD
        {
            VERSION_OK,
            VERSION_ANT_V1,
            VERSION_DESC
        }

        private void FormInicial_Load(object sender, EventArgs e)
        {
            // Me.Text = "IAHRIS (Índices de Alteración Hidrológica de RÍoS)                        [" & Me._sVersion & "]"

            __alto = Height;
            __ancho = Width;


            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");



            // Testear el fichero modelo
            if (!My.MyProject.Computer.FileSystem.FileExists(_traductor.getRutaExcel))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoFoundXLS") + _traductor.getRutaExcel, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFatal"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
                Dispose();
                return;
            }

            // Testear que hay una base de datos en el directorio.
            if (!My.MyProject.Computer.FileSystem.FileExists(@".\IAHRISv2.mdb"))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoFoundDB") + Application.ExecutablePath, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFatal"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
                Dispose();
                return;
            }
            else
            {
                _RutaBBDD = Application.StartupPath + @"\IAHRISv2.mdb";
                _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
                _rellenar = new Rellenar.RellenarForm(_cMDB);
                var argcombo = cbProyectos;
                _rellenar.RellenarProyectos(ref argcombo);
                cbProyectos = argcombo;

                // Me._rellenar.RellenarPuntos(Me.lstboxPuntos)

                _testFechas = new TestFechas(_cMDB);
            }

            Cursor = Cursors.WaitCursor;
            // Cargar el menu de idiomas
            ReadOnlyCollection<string> files;
            files = My.MyProject.Computer.FileSystem.GetFiles(Application.StartupPath + @"\lang", Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly, "*.xml");
            for (int i = 0, loopTo = files.Count - 1; i <= loopTo; i++)
            {
                string strIdioma = "";
                string ruta = "";
                if (_traductor.testFormatXML(files[i], ref strIdioma, ruta))
                {
                    var mnuIdioma = new ToolStripMenuItem(strIdioma);
                    mnuIdioma.Tag = files[i];
                    mnuIdioma.Click += OpcionMenu_Click;
                    IdiomasToolStripMenuItem.DropDownItems.Add(mnuIdioma);
                }
                else if (string.IsNullOrEmpty(ruta))
                {
                    MessageBox.Show("Error al cargar: " + files[i] + Constants.vbCrLf + "Error no puedo encontrar el fichero excel", "Error al cargar idioma");
                }
                else if (string.IsNullOrEmpty(strIdioma))
                {
                    MessageBox.Show("Error al cargar: " + files[i] + Constants.vbCrLf + "Error encuentro el identificador de idioma correcto", "Error al cargar idioma");
                }
                else
                {
                    MessageBox.Show("Error al cargar: " + files[i], "Error al cargar idioma");
                }
            }

            Cursor = Cursors.Default;


            // ----------------------------------------
            // ------- Tabla de datos -----------------
            // ----------------------------------------
            XPTablaListas.ColumnResizing = false;
            XPTablaListas.HeaderRenderer.Trimming = StringTrimming.None;
            XPTablaListas.HeaderRenderer.Font = new Font(XPTablaListas.HeaderRenderer.Font.FontFamily, 7.5f);
            XPTablaListas.BeginUpdate();

            // Me.XPTablaListas.NoItemsText = "Seleccione un punto de la lista para analizar sus series asociadas."
            XPTablaListas.NoItemsText = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "noitem");

            // Columna de los años
            var colTexto = new TextColumn();
            colTexto.Editable = false;
            colTexto.Sortable = true;
            colTexto.Width = 67;
            // colTexto.Text = "Año" & vbCrLf & "hidrológico"
            colTexto.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "year");
            var imgColND = new ImageColumn();
            imgColND.Editable = false;
            imgColND.Sortable = false;
            imgColND.Width = 55;
            // imgColND.Text = "Natural" & vbCrLf & "Diaria" & vbCrLf & "Serie"
            imgColND.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynat");
            var chkColND = new CheckBoxColumn();
            chkColND.Editable = false;
            chkColND.Sortable = false;
            chkColND.Width = 50;
            // chkColND.Text = "Natural" & vbCrLf & "Diaria" & vbCrLf & "Cálculo"
            chkColND.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynatinter");
            var imgColAD = new ImageColumn();
            imgColAD.Editable = false;
            imgColAD.Sortable = false;
            imgColAD.ImageOnRight = true;
            imgColAD.Width = 55;
            // imgColAD.Text = "Alterada" & vbCrLf & "Diaria" & vbCrLf & "Serie"
            imgColAD.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyalt");
            var chkColAD = new CheckBoxColumn();
            chkColAD.Editable = false;
            chkColAD.Sortable = false;
            chkColAD.Width = 50;
            chkColAD.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyaltinter");
            var imgColCD = new ImageColumn();
            imgColCD.Editable = false;
            imgColCD.Sortable = false;
            imgColCD.ImageOnRight = true;
            imgColCD.Width = 50;
            imgColCD.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coedaily");
            var imgColNM = new ImageColumn();
            imgColNM.Editable = false;
            imgColNM.Sortable = false;
            imgColNM.ImageOnRight = true;
            imgColNM.Width = 55;
            imgColNM.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynat");
            var chkColNM = new CheckBoxColumn();
            chkColNM.Editable = false;
            chkColNM.Sortable = false;
            chkColNM.Width = 50;
            chkColNM.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynatinter");
            var imgColAM = new ImageColumn();
            imgColAM.Editable = false;
            imgColAM.Sortable = false;
            imgColAM.ImageOnRight = true;
            imgColAM.Width = 55;
            imgColAM.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyalt");
            var chkColAM = new CheckBoxColumn();
            chkColAM.Editable = false;
            chkColAM.Sortable = false;
            chkColAM.Width = 50;
            chkColAM.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyaltinter");
            var imgColCM = new ImageColumn();
            imgColCM.Editable = false;
            imgColCM.Sortable = false;
            imgColCM.ImageOnRight = true;
            imgColCM.Width = 50;
            imgColCM.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coemonthly");
            var cols = new Column[] { colTexto, imgColNM, chkColNM, imgColAM, chkColAM, imgColCM, imgColND, chkColND, imgColAD, chkColAD, imgColCD };
            ColumnModel1.Columns.AddRange(cols);
            XPTablaListas.EndUpdate();


            // Cambiar los label de los años
            lblAñosHidro.Text = 0.ToString();
            lblAñosCoeDiaria.Text = 0.ToString();
            lblAñosCoeMensual.Text = 0.ToString();
            lblAñosNatDiario.Text = 0.ToString();
            lblAñosNatDiarioUSO.Text = 0.ToString();
            lblAñosNatMensual.Text = 0.ToString();
            lblAñosNatMensualUSO.Text = 0.ToString();
            lblAñosAltDiario.Text = 0.ToString();
            lblAñosAltDiarioUSO.Text = 0.ToString();
            lblAñosAltMensual.Text = 0.ToString();
            lblAñosAltMensualUSO.Text = 0.ToString();
            btnCalcular.Enabled = false;
            string myBuildInfo = Application.ProductVersion;//FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            Text += " - v" + myBuildInfo;
        }

        private void FormInicial_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _cMDB.Desconectar();
                _cMDB = null;
            }
            catch (Exception ex)
            {
            }

            Application.OpenForms["FormBienvenida"].Close();
        }

        private void FormInicial_Activated(object sender, EventArgs e)
        {
            // Refrescar listas cuando se pone el form activo

            var argcombo = cbProyectos;
            _rellenar.RellenarProyectos(ref argcombo, _id_proy_selec);
            cbProyectos = argcombo;
            var arglistbox = lstboxPuntos;
            _rellenar.RellenarPuntos(ref arglistbox, _id_proy_selec);
            lstboxPuntos = arglistbox;


        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void AñadirProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var fanadirproyecto = new FormAnadirProyecto(_cMDB);
            fanadirproyecto.ShowDialog();
        }

        private void EliminarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var feliminarproyecto = new FormEliminarProyecto(_cMDB);
            feliminarproyecto.ShowDialog();
            cbProyectos_SelectedIndexChanged(null, null);
            _cMDB.Desconectar();
            DBUtils.DataBaseUtils.CompactAccessDB("Base", _RutaBBDD);
            _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
        }

        private void AñadirPuntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();

            // Enseño el formulario como un dialog
            var fanadir = new FormAnadirPunto(_cMDB, "Punto");
            fanadir.ShowDialog();
        }

        private void AñadirAlteraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();

            // Enseño el formulario como un dialog
            var fanadir = new FormAnadirPunto(_cMDB, "Alteración");
            fanadir.ShowDialog();
        }

        private void AñadirListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var fanadir = new FormAnadirListas(_cMDB);
            fanadir.ShowDialog();
            _cMDB.Desconectar();
            DBUtils.DataBaseUtils.CompactAccessDB("Base", _RutaBBDD);
            _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
        }

        private void EliminarListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var feliminar = new FormEliminarLista(_cMDB);
            feliminar.ShowDialog();
            _cMDB.Desconectar();
            DBUtils.DataBaseUtils.CompactAccessDB("Base", _RutaBBDD);
            _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
        }

        private void EliminarPuntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var feliminar = new FormEliminarPunto(_cMDB, "Punto");
            feliminar.ShowDialog();
            _cMDB.Desconectar();
            DBUtils.DataBaseUtils.CompactAccessDB("Base", _RutaBBDD);
            _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
        }

        private void EliminarAlteraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var feliminar = new FormEliminarPunto(_cMDB, "Alteracion");
            feliminar.ShowDialog();
            _cMDB.Desconectar();
            DBUtils.DataBaseUtils.CompactAccessDB("Base", _RutaBBDD);
            _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
        }

        private void ImportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fimportar = new FormImportar(_cMDB);
            fimportar.ShowDialog();
        }

        private void ExportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            _cMDB.Desconectar();
            sfd.Filter = "Base de datos ACCESS (*.mdb)|*.mdb";
            sfd.FilterIndex = 1;
            sfd.CheckFileExists = false;
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            try
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    My.MyProject.Computer.FileSystem.CopyFile(_RutaBBDD, sfd.FileName, true);
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strExportacion") + Constants.vbCrLf + sfd.FileName, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorExport") + Constants.vbCrLf + ex.Message, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
        }

        private void OpcionMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem OpcionSeleccionada = (ToolStripMenuItem)sender;

            // OpcionSeleccionada.Tag
            if (!_traductor.cambiarIdioma(Conversions.ToString(OpcionSeleccionada.Tag)))
            {
                return;
            }

            _traductor.traducirForm(Conversions.ToString(OpcionSeleccionada.Tag), "");
            XPTablaListas.BeginUpdate();
            XPTablaListas.NoItemsText = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "noitem");
            XPTablaListas.ColumnModel.Columns[0].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "year");
            XPTablaListas.ColumnModel.Columns[6].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynat");
            XPTablaListas.ColumnModel.Columns[7].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynatinter");
            XPTablaListas.ColumnModel.Columns[8].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyalt");
            XPTablaListas.ColumnModel.Columns[9].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyaltinter");
            XPTablaListas.ColumnModel.Columns[10].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coedaily");
            XPTablaListas.ColumnModel.Columns[1].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynat");
            XPTablaListas.ColumnModel.Columns[2].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynatinter");
            XPTablaListas.ColumnModel.Columns[3].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyalt");
            XPTablaListas.ColumnModel.Columns[4].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyaltinter");
            XPTablaListas.ColumnModel.Columns[5].Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coemonthly");
            XPTablaListas.EndUpdate();
            if (lstBoxInformes.Items.Count > 0)
            {
                RellenarInformes();
            }

            var myBuildInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            Text += " - v" + myBuildInfo.FileMajorPart + "." + myBuildInfo.FileMinorPart;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void cbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProyectoDesc.Text = "";
            int localRellenarProyectosDesc() { var arglabel = lblProyectoDesc; var ret = _rellenar.RellenarProyectosDesc(ref arglabel, cbProyectos.SelectedIndex); lblProyectoDesc = arglabel; return ret; }

            _id_proy_selec = localRellenarProyectosDesc();
            var arglistbox = lstboxPuntos;
            _rellenar.RellenarPuntos(ref arglistbox, _id_proy_selec);
            lstboxPuntos = arglistbox;
            lstboxPuntos_SelectedIndexChanged(null, null);
            // Borro la tabla
            XPTablaListas.TableModel.Rows.Clear();
            cmbListaAlteradasDiarias_SelectedIndexChanged(null, null);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void lstboxPuntos_SelectedIndexChanged(object sender, EventArgs e)
        {
            long id_punto;
            string nombre;
            int mesInicio;
            int nListas;
            int nAlt;
            DataSet ds;
            DataRow dr;
            string clavePunto;
            if (lstboxPuntos.SelectedItem!=null)
            {
                // Clave del punto
                //clavePunto = lstboxPuntos.SelectedItem.ToString();
                _PtoSeleccionado = (ComboItem)lstboxPuntos.SelectedItem;

                // Sacar nombre y id
                ds = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto, nombre, mesInicio FROM Punto WHERE ID_Punto=" + ((ComboItem)lstboxPuntos.SelectedItem).Id );
                dr = ds.Tables[0].Rows[0];
                id_punto = Conversions.ToLong(dr["id_punto"]);
                nombre = Conversions.ToString(dr["nombre"]);
                mesInicio = Conversions.ToInteger(dr["mesInicio"]);
                clavePunto = lstboxPuntos.SelectedItem.ToString();
                _strPto = _PtoSeleccionado + "-" + nombre;

                // Sacar el numero de alteraciones
                ds = _cMDB.RellenarDataSet("Alt", "SELECT DISTINCT id_alteracion FROM [alteracion] WHERE id_alteracion > 0 AND id_punto=" + id_punto);
                nAlt = ds.Tables[0].Rows.Count;

                // Sacar el numero de listas
                ds = _cMDB.RellenarDataSet("Listas", "Select Count(*) FROM [Lista] WHERE id_punto=" + id_punto + "");
                dr = ds.Tables[0].Rows[0];
                nListas = int.Parse(Conversions.ToString(dr[0]));

                // Rellenar nombres de las series en el combo lista
                bool hayDiaria = default, hayMensual = default;
                string stNone = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");
                var argcomboAltD = cmbListaAlteradasDiarias;
                _rellenar.RellenarListas(ref hayDiaria, ref hayMensual, ref argcomboAltD, (int)id_punto, stNone);
                cmbListaAlteradasDiarias = argcomboAltD;
                string stSI = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "yes").ToUpper();
                string stNO = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "no").ToUpper();
                if (hayDiaria)
                {
                    lblSerieNatDiaria.Text = stSI;
                }
                else
                {
                    lblSerieNatDiaria.Text = stNO;
                }

                if (hayMensual)
                {
                    lblSerieNatMensual.Text = stSI;
                }
                else
                {
                    lblSerieNatMensual.Text = stNO;
                }

                lblPuntosClave.Text = clavePunto;
                lblPuntosNombre.Text = nombre;
                lblPuntosNListas.Text = nAlt.ToString();
                cmbMesInicio.SelectedIndex = mesInicio - 1;
                _simulacion.mesInicio = mesInicio;
            }
            else
            {
                lblSerieNatDiaria.Text = "--";
                lblSerieNatMensual.Text = "--";
                lblPuntosClave.Text = "";
                lblPuntosNombre.Text = "";
                lblPuntosNListas.Text = "";
                cmbListaAlteradasDiarias.Items.Clear();
            }
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void chkboxUsarCoe_CheckedChanged(object sender, EventArgs e)
        {
            int ordD;
            var idpunto = default(int);
            int idAlteracion;
            var mesInicio = default(int);
            DataSet ds;
            DataRow dr;
            string stNone = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");
            if (cmbListaAlteradasDiarias.SelectedItem !=null && cmbListaAlteradasDiarias.SelectedItem.ToString()!= stNone)
            {
                //ds = _cMDB.RellenarDataSet("Listas", "Select id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" + cmbListaAlteradasDiarias.SelectedItem.ToString() + "'");
                //dr = ds.Tables[0].Rows[0];
                idAlteracion = ((ComboItem)cmbListaAlteradasDiarias.SelectedItem).Id;
            }
            else
            {
                idAlteracion = -1;
            }

            if (_PtoSeleccionado != null)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Id_punto =" + _PtoSeleccionado.Id);

                dr = ds.Tables[0].Rows[0];
                idpunto = Conversions.ToInteger(dr["id_punto"]);
                mesInicio = Conversions.ToInteger(dr["mesInicio"]);
            }
            else
            {
                ordD = -1;
            }

            // Rellenar la tabla con los años y los totales
            TestFechas.Simulacion localRellenarXPTable() { var argtabla = XPTablaListas; var ret = _rellenar.RellenarXPTable(ref argtabla, idAlteracion, idpunto, chkboxUsarCoe.Checked, chkboxUsarCoeDiaria.Checked); XPTablaListas = argtabla; return ret; }

            _simulacion = localRellenarXPTable();
            _simulacion.mesInicio = mesInicio;
            lblAñosHidro.Text = (_simulacion.fechaFIN - _simulacion.fechaINI).ToString();
            lblAñosNatDiario.Text = _simulacion.listas[0].nValidos.ToString();
            lblAñosAltDiario.Text = _simulacion.listas[1].nValidos.ToString();
            lblAñosCoeDiaria.Text = _simulacion.coe[0].nCoetaneos.ToString();
            if (_simulacion.añosInterNat is object)
            {
                lblAñosNatMensual.Text = (_simulacion.listas[2].nValidos + _simulacion.añosInterNat.Length).ToString();
            }
            else
            {
                lblAñosNatMensual.Text = _simulacion.listas[2].nValidos.ToString();
            }

            if (_simulacion.añosInterAlt is object)
            {
                lblAñosAltMensual.Text = (_simulacion.listas[3].nValidos + _simulacion.añosInterAlt.Length).ToString();
            }
            else
            {
                lblAñosAltMensual.Text = _simulacion.listas[3].nValidos.ToString();
            }

            if (_simulacion.añosInterCoe is object)
            {
                lblAñosCoeMensual.Text = (_simulacion.coe[1].nCoetaneos + _simulacion.añosInterCoe.Length).ToString();
            }
            else
            {
                lblAñosCoeMensual.Text = _simulacion.coe[1].nCoetaneos.ToString();
            }

            // Cambiar colores
            if (int.Parse(lblAñosNatMensual.Text) < 15 & int.Parse(lblAñosNatMensual.Text) != 0)
            {
                lblAñosNatMensual.ForeColor = Color.Red;
            }
            else
            {
                lblAñosNatMensual.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosAltMensual.Text) < 15 & int.Parse(lblAñosAltMensual.Text) != 0)
            {
                lblAñosAltMensual.ForeColor = Color.Red;
            }
            else
            {
                lblAñosAltMensual.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosNatDiario.Text) < 15 & int.Parse(lblAñosNatDiario.Text) != 0)
            {
                lblAñosNatDiario.ForeColor = Color.Red;
            }
            else
            {
                lblAñosNatDiario.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosAltDiario.Text) < 15 & int.Parse(lblAñosAltDiario.Text) != 0)
            {
                lblAñosAltDiario.ForeColor = Color.Red;
            }
            else
            {
                lblAñosAltDiario.ForeColor = Color.Black;
            }

            lblAñosNatDiarioUSO.Text = _simulacion.añosParaCalculo[0].nAños.ToString();
            lblAñosNatMensualUSO.Text = _simulacion.añosParaCalculo[2].nAños.ToString();
            lblAñosAltDiarioUSO.Text = _simulacion.añosParaCalculo[1].nAños.ToString();
            lblAñosAltMensualUSO.Text = _simulacion.añosParaCalculo[3].nAños.ToString();
            RellenarInformes();
        }

        private void chkboxUsarCoeDiaria_CheckedChanged(object sender, EventArgs e)
        {
            int ordD;
            var idpunto = default(int);
            int idAlteracion;
            var mesInicio = default(int);
            DataSet ds;
            DataRow dr;
            string stNone = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");
            if (cmbListaAlteradasDiarias.SelectedItem!=null&& cmbListaAlteradasDiarias.SelectedItem.ToString()!= stNone)
            {
                //ds = _cMDB.RellenarDataSet("Listas", "Select id_alteracion FROM [Alteracion] WHERE _Alteracion='" + cmbListaAlteradasDiarias.SelectedItem + "'");
                //dr = ds.Tables[0].Rows[0];
                idAlteracion = ((ComboItem)cmbListaAlteradasDiarias.SelectedItem).Id;
            }
            else
            {
                idAlteracion = -1;
            }

            if (_PtoSeleccionado != null)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Id_punto =" + _PtoSeleccionado.Id);

                dr = ds.Tables[0].Rows[0];
                idpunto = Conversions.ToInteger(dr["id_punto"]);
                mesInicio = Conversions.ToInteger(dr["mesInicio"]);
            }
            else
            {
                ordD = -1;
            }

            // Rellenar la tabla con los años y los totales
            TestFechas.Simulacion localRellenarXPTable() { var argtabla = XPTablaListas; var ret = _rellenar.RellenarXPTable(ref argtabla, idAlteracion, idpunto, chkboxUsarCoe.Checked, chkboxUsarCoeDiaria.Checked); XPTablaListas = argtabla; return ret; }

            _simulacion = localRellenarXPTable();
            _simulacion.mesInicio = mesInicio;
            lblAñosHidro.Text = (_simulacion.fechaFIN - _simulacion.fechaINI).ToString();
            lblAñosNatDiario.Text = _simulacion.listas[0].nValidos.ToString();
            lblAñosAltDiario.Text = _simulacion.listas[1].nValidos.ToString();
            lblAñosCoeDiaria.Text = _simulacion.coe[0].nCoetaneos.ToString();
            if (_simulacion.añosInterNat is object)
            {
                lblAñosNatMensual.Text = (_simulacion.listas[2].nValidos + _simulacion.añosInterNat.Length).ToString();
            }
            else
            {
                lblAñosNatMensual.Text = _simulacion.listas[2].nValidos.ToString();
            }

            if (_simulacion.añosInterAlt is object)
            {
                lblAñosAltMensual.Text = (_simulacion.listas[3].nValidos + _simulacion.añosInterAlt.Length).ToString();
            }
            else
            {
                lblAñosAltMensual.Text = _simulacion.listas[3].nValidos.ToString();
            }

            if (_simulacion.añosInterCoe is object)
            {
                lblAñosCoeMensual.Text = (_simulacion.coe[1].nCoetaneos + _simulacion.añosInterCoe.Length).ToString();
            }
            else
            {
                lblAñosCoeMensual.Text = _simulacion.coe[1].nCoetaneos.ToString();
            }

            lblAñosNatDiarioUSO.Text = _simulacion.añosParaCalculo[0].nAños.ToString();
            lblAñosNatMensualUSO.Text = _simulacion.añosParaCalculo[2].nAños.ToString();
            lblAñosAltDiarioUSO.Text = _simulacion.añosParaCalculo[1].nAños.ToString();
            lblAñosAltMensualUSO.Text = _simulacion.añosParaCalculo[3].nAños.ToString();

            // Cambiar colores
            if (int.Parse(lblAñosNatMensual.Text) < 15 & int.Parse(lblAñosNatMensual.Text) != 0)
            {
                lblAñosNatMensual.ForeColor = Color.Red;
            }
            else
            {
                lblAñosNatMensual.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosAltMensual.Text) < 15 & int.Parse(lblAñosAltMensual.Text) != 0)
            {
                lblAñosAltMensual.ForeColor = Color.Red;
            }
            else
            {
                lblAñosAltMensual.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosNatDiario.Text) < 15 & int.Parse(lblAñosNatDiario.Text) != 0)
            {
                lblAñosNatDiario.ForeColor = Color.Red;
            }
            else
            {
                lblAñosNatDiario.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosAltDiario.Text) < 15 & int.Parse(lblAñosAltDiario.Text) != 0)
            {
                lblAñosAltDiario.ForeColor = Color.Red;
            }
            else
            {
                lblAñosAltDiario.ForeColor = Color.Black;
            }

            RellenarInformes();
        }

        private void cmbListaAlteradasDiarias_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem sItem;
            int nAlt;
            DataSet ds;
            DataRow dr;
            if (cmbListaAlteradasDiarias.SelectedIndex == -1)
            {
                btnCalcular.Enabled = false;
                lblCodigoAlt.Text = "---";
                lblIDDiaria.Text = "---";
                lblIDMensual.Text = "---";
                _strAlt = "";
                lblAñosHidro.Text = "";
                lblAñosNatDiario.Text = "";
                lblAñosAltDiario.Text = "";
                lblAñosCoeDiaria.Text = "";
                lblAñosNatMensual.Text = "";
                lblAñosAltMensual.Text = "";
                lblAñosCoeMensual.Text = "";
                lblAñosNatDiarioUSO.Text = "";
                lblAñosNatMensualUSO.Text = "";
                lblAñosAltDiarioUSO.Text = "";
                lblAñosAltMensualUSO.Text = "";
                chkboxUsarCoe.Enabled = false;
                chkboxUsarCoeDiaria.Enabled = false;
                lstBoxInformes.Items.Clear();
                return;
            }
            nAlt = -1;
           
            string stNone = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");
            if ((cmbListaAlteradasDiarias.SelectedItem != null) && cmbListaAlteradasDiarias.SelectedItem.ToString()!= stNone)
            {
                sItem = (ComboItem)cmbListaAlteradasDiarias.SelectedItem;
                _AltSeleccionada = sItem;
                string COD;

                // sAux = sItem.Split(" ")
                // nAlt = Integer.Parse(sAux(sAux.Length - 1))

                // Saco el codigo de la alteracion
                ds = _cMDB.RellenarDataSet("Alt", "SELECT nombre FROM [Alteracion] WHERE id_alteracion =" + sItem.Id );
                dr = ds.Tables[0].Rows[0];
                COD =dr[0].ToString();
                lblCodigoAlt.Text = COD;
                _strAlt = Conversions.ToString(Operators.ConcatenateObject(sItem + "-", COD));
                nAlt = sItem.Id;
                string stSI = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "yes").ToUpper();
                string stNO = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "no").ToUpper();
                ds = _cMDB.RellenarDataSet("Alt", "SELECT Nombre FROM [Lista] WHERE ID_Alteracion=" + nAlt + " AND Tipo_fechas=True");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblIDDiaria.Text = stSI;
                }
                else
                {
                    lblIDDiaria.Text = stNO;
                }

                ds = _cMDB.RellenarDataSet("Alt", "SELECT Nombre FROM [Lista] WHERE ID_Alteracion=" + nAlt + " AND Tipo_fechas=false");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblIDMensual.Text = stSI;
                }
                else
                {
                    lblIDMensual.Text = stNO;
                }
            }
            else
            {
                lblCodigoAlt.Text = "---";
                lblIDDiaria.Text = "---";
                lblIDMensual.Text = "---";
                _strAlt = "";
            }

            // ++++++++++++++++++++++++++++++++++++++++++
            // +++++++ Rellenar la XP table +++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++
            int ordD;
            var idpunto = default(int);
            int idAlteracion;
            var mesInicio = default(int);
            if (cmbListaAlteradasDiarias.SelectedItem.ToString()!= stNone)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_alteracion FROM [Alteracion] WHERE Id_Alteracion=" + nAlt );
                dr = ds.Tables[0].Rows[0];
                idAlteracion = Conversions.ToInteger(dr[0]);
            }
            else
            {
                idAlteracion = -1;
            }

            if (_PtoSeleccionado != null)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Id_punto=" + _PtoSeleccionado.Id );
                dr = ds.Tables[0].Rows[0];
                idpunto = Conversions.ToInteger(dr["id_punto"]);
                mesInicio = Conversions.ToInteger(dr["mesInicio"]);
            }
            else
            {
                ordD = -1;
            }

            // Rellenar la tabla con los años y los totales
            TestFechas.Simulacion localRellenarXPTable() { var argtabla = XPTablaListas; var ret = _rellenar.RellenarXPTable(ref argtabla, idAlteracion, idpunto, chkboxUsarCoe.Checked, chkboxUsarCoeDiaria.Checked); XPTablaListas = argtabla; return ret; }

            _simulacion = localRellenarXPTable();
            _simulacion.mesInicio = mesInicio;
            lblAñosHidro.Text = (_simulacion.fechaFIN - _simulacion.fechaINI).ToString();
            lblAñosNatDiario.Text = _simulacion.listas[0].nValidos.ToString();
            lblAñosAltDiario.Text = _simulacion.listas[1].nValidos.ToString();
            lblAñosCoeDiaria.Text = _simulacion.coe[0].nCoetaneos.ToString();
            if (_simulacion.añosInterNat is object)
            {
                lblAñosNatMensual.Text = (_simulacion.listas[2].nValidos + _simulacion.añosInterNat.Length).ToString();
            }
            else
            {
                lblAñosNatMensual.Text = _simulacion.listas[2].nValidos.ToString();
            }

            if (_simulacion.añosInterAlt is object)
            {
                lblAñosAltMensual.Text = (_simulacion.listas[3].nValidos + _simulacion.añosInterAlt.Length).ToString();
            }
            else
            {
                lblAñosAltMensual.Text = _simulacion.listas[3].nValidos.ToString();
            }

            if (_simulacion.añosInterCoe is object)
            {
                lblAñosCoeMensual.Text = (_simulacion.coe[1].nCoetaneos + _simulacion.añosInterCoe.Length).ToString();
            }
            else
            {
                lblAñosCoeMensual.Text = _simulacion.coe[1].nCoetaneos.ToString();
            }

            lblAñosNatDiarioUSO.Text = _simulacion.añosParaCalculo[0].nAños.ToString();
            lblAñosNatMensualUSO.Text = _simulacion.añosParaCalculo[2].nAños.ToString();
            lblAñosAltDiarioUSO.Text = _simulacion.añosParaCalculo[1].nAños.ToString();
            lblAñosAltMensualUSO.Text = _simulacion.añosParaCalculo[3].nAños.ToString();

            // Cambiar colores
            if (int.Parse(lblAñosNatMensual.Text) < 15 & int.Parse(lblAñosNatMensual.Text) != 0)
            {
                lblAñosNatMensual.ForeColor = Color.Red;
            }
            else
            {
                lblAñosNatMensual.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosAltMensual.Text) < 15 & int.Parse(lblAñosAltMensual.Text) != 0)
            {
                lblAñosAltMensual.ForeColor = Color.Red;
            }
            else
            {
                lblAñosAltMensual.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosNatDiario.Text) < 15 & int.Parse(lblAñosNatDiario.Text) != 0)
            {
                lblAñosNatDiario.ForeColor = Color.Red;
            }
            else
            {
                lblAñosNatDiario.ForeColor = Color.Black;
            }

            if (int.Parse(lblAñosAltDiario.Text) < 15 & int.Parse(lblAñosAltDiario.Text) != 0)
            {
                lblAñosAltDiario.ForeColor = Color.Red;
            }
            else
            {
                lblAñosAltDiario.ForeColor = Color.Black;
            }

            if (_simulacion.coe[1].nCoetaneos >= 15 & _simulacion.coe[1].nCoetaneos < int.Parse(lblAñosNatMensual.Text))
            {
                chkboxUsarCoe.Enabled = true;
                chkboxUsarCoe.Checked = true;
            }
            else
            {
                chkboxUsarCoe.Enabled = false;
                if (_simulacion.coe[1].nCoetaneos == int.Parse(lblAñosNatMensual.Text) & _simulacion.coe[1].nCoetaneos > 0)
                {
                    chkboxUsarCoe.Checked = true;
                }
                else
                {
                    chkboxUsarCoe.Checked = false;
                }
            }

            if (_simulacion.coe[0].nCoetaneos >= 15 & _simulacion.coe[0].nCoetaneos < int.Parse(lblAñosNatDiario.Text))
            {
                chkboxUsarCoeDiaria.Enabled = true;
                chkboxUsarCoeDiaria.Checked = false;
            }
            else
            {
                chkboxUsarCoeDiaria.Enabled = false;
                if (_simulacion.coe[0].nCoetaneos == int.Parse(lblAñosNatDiario.Text) & _simulacion.coe[0].nCoetaneos > 0)
                {
                    chkboxUsarCoeDiaria.Checked = true;
                }
                else
                {
                    chkboxUsarCoeDiaria.Checked = false;
                }
            }

            RellenarInformes();
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            _simulacion.sNombre = _strPto;
            _simulacion.sAlteracion = _strAlt;
            _simulacion.añosCoetaneosTotales = Conversions.ToInteger(lblAñosCoeMensual.Text);
            var fCalcular = new FormCalculo(_simulacion, _informes, _cMDB);
            Enabled = false;
            Cursor = Cursors.WaitCursor;
            fCalcular.ShowDialog();
            Enabled = true;
            Cursor = Cursors.Default;
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public void RellenarInformes()
        {
            lstBoxInformes.Items.Clear();
            int nAñosIntNat;
            int nAñosIntAlt;
            bool okNatComp = false;
            bool okAltComp = false;
            bool okNatDComp = false;
            bool okAltDComp = false;
            bool okOnlyAlt = false;
            bool okOnlyAltMensual = false;
            bool usarCoeMen = false;
            bool usarCoeDia = false;
            _informes.inf1 = false;
            _informes.inf1a = false;
            _informes.inf1b = false;
            _informes.inf2 = false;
            _informes.inf2a = false;
            _informes.inf3 = false;
            _informes.inf3a = false;
            _informes.inf3b = false;
            _informes.inf4 = false;
            _informes.inf4a = false;
            _informes.inf5 = false;
            _informes.inf5a = false;
            _informes.inf5b = false;
            _informes.inf6 = false;
            _informes.inf6a = false;
            _informes.inf6b = false;
            _informes.inf6c = false;
            _informes.inf6d = false;
            _informes.inf6e = false;
            _informes.inf7d = false;
            _informes.inf7a = false;
            _informes.inf7b = false;
            _informes.inf7c = false;
            _informes.inf8 = false;
            _informes.inf8a = false;
            _informes.inf8b = false;
            _informes.inf8c = false;
            _informes.inf8d = false;
            _informes.inf9 = false;
            _informes.inf9a = false;
            _informes.inf9b = false;
            _informes.inf10a = false;
            _informes.inf10b = false;
            _informes.inf10c = false;
            _informes.inf10d = false;


            bool isNatDia = false;
            bool isAltDia = false;
            bool isNatMens = false;
            bool isAltMens = false;
            bool isNat = false;
            bool isAlt = false;

            if(_simulacion.listas[0].nValidos>14)
            {
                isNatDia = true;
                isNat = true;
            }
            if (_simulacion.listas[1].nValidos > 14)
            {
                isAltDia = true;
                isAlt = true;
            }
            if (_simulacion.listas[2].nValidos > 14)
            {
                isNatMens = true;
                isNat = true;
            }
            if (_simulacion.listas[3].nValidos > 14)
            {
                isAltMens = true;
                isAlt = true;
            }

            if (_simulacion.usarCoeDiara)
            {
                usarCoeDia = true;
            }
            if (_simulacion.usarCoe)
            {
                usarCoeMen = true;
            }

            //Determinación de tipologías

            Tipologias Tipologia = new Tipologias();
            if (isNatDia & !isAlt) Tipologia = Tipologias.Tipo1;
            if (isNatMens & !isAlt) Tipologia = Tipologias.Tipo2;
            if (isAltDia & !isNat) Tipologia = Tipologias.Tipo3;
            if (isAltMens & !isNat) Tipologia = Tipologias.Tipo4;
            if (isNatDia & isAltDia & usarCoeDia) Tipologia = Tipologias.Tipo5;
            if (isNatMens & isAltMens & usarCoeMen) Tipologia = Tipologias.Tipo6;
            if (isNatDia & isAltMens & usarCoeMen) Tipologia = Tipologias.Tipo6A;
            if (isNatMens & isAltDia & usarCoeMen) Tipologia = Tipologias.Tipo6B;
            if (isNatDia & isAltDia & !usarCoeDia) Tipologia = Tipologias.Tipo7;
            if (isNatMens & isAltMens & !usarCoeMen) Tipologia = Tipologias.Tipo8;
            if (isNatDia & isAltMens & !usarCoeMen) Tipologia = Tipologias.Tipo8A;
            if (isNatMens & isAltDia & !usarCoeMen) Tipologia = Tipologias.Tipo8B;

            if (Tipologia != Tipologias.NONE)
            {
                btnCalcular.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCalcular") + " " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strType") + " " + Tipologia.ToString().Replace("Tipo", "");
                btnCalcular.Enabled = true;
                _simulacion.Tipologia = Tipologia;
            }
            else
            {
                btnCalcular.Enabled = false;
                btnCalcular.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCalcular");
            }

                //Asignar Informes
                switch (Tipologia)
                {
                    case Tipologias.Tipo1:
                        _informes.inf1 = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf4 = true;
                        _informes.inf6 = true;
                        _informes.inf6c = true;
                        _informes.inf9 = true;
                        _informes.inf9b = true;
                        break;

                    case Tipologias.Tipo2:
                        _informes.inf1 = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf4a = true;
                        _informes.inf9a = true;
                        break;
                    case Tipologias.Tipo3:

                        _informes.inf1a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf5b = true;
                        _informes.inf6a = true;
                        _informes.inf6d = true;
                        break;
                    case Tipologias.Tipo4:
                        _informes.inf1a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf5a = true;
                        break;
                    case Tipologias.Tipo5:
                        _informes.inf1 = true;
                        _informes.inf1a = true;
                        _informes.inf1b = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf3b = true;
                        _informes.inf4 = true;
                        _informes.inf5 = true;
                        _informes.inf6 = true;
                        _informes.inf6a = true;
                        _informes.inf6b = true;
                        _informes.inf6c = true;
                        _informes.inf6d = true;
                        _informes.inf6e = true;
                        _informes.inf7a = true;
                        _informes.inf7d = true;
                        _informes.inf8 = true;
                        _informes.inf8a = true;
                        _informes.inf9 = true;
                        _informes.inf9b = true;
                        _informes.inf10a= true;
                    break;
                case Tipologias.Tipo6:
                    _informes.inf1 = true;
                    _informes.inf1a = true;
                    _informes.inf1b = true;
                    _informes.inf2 = true;
                    _informes.inf2a = true;
                    _informes.inf3 = true;
                    _informes.inf3a = true;
                    _informes.inf3b = true;
                    _informes.inf4a = true;
                    _informes.inf5a = true;
                    _informes.inf7b = true;
                    _informes.inf8 = true;
                    _informes.inf8c = true;
                    _informes.inf9a = true;
                    _informes.inf10b = true;
                    break;
                case Tipologias.Tipo6A:
                        _informes.inf1 = true;
                        _informes.inf1a = true;
                        _informes.inf1b = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf3b = true;
                        _informes.inf4 = true;
                        _informes.inf5a = true;
                        _informes.inf6 = true;
                        _informes.inf6c = true;
                        _informes.inf7b = true;
                        _informes.inf8 = true;
                       // _informes.inf8a = true;
                        _informes.inf8c = true;
                        _informes.inf9 = true;
                        _informes.inf9b = true;
                        _informes.inf10b = true;
                    break;
                    case Tipologias.Tipo6B:
                        _informes.inf1 = true;
                        _informes.inf1a = true;
                        _informes.inf1b = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf3b = true;
                        _informes.inf4a = true;
                        _informes.inf5b = true;
                        _informes.inf6a = true;
                        _informes.inf6d = true;
                        _informes.inf7b = true;
                        _informes.inf8 = true;
                        _informes.inf8c = true;
                        _informes.inf9a = true;
                        _informes.inf10b = true;
                    break;
                    case Tipologias.Tipo7:
                        _informes.inf1 = true;
                        _informes.inf1a = true;
                  //      _informes.inf1b = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf3b = true;
                        _informes.inf4 = true;
                        _informes.inf5 = true;
                        _informes.inf6 = true;
                        _informes.inf6a = true;
                        _informes.inf6b = true;
                        _informes.inf6c = true;
                        _informes.inf6d = true;
                        _informes.inf6e = true;
                        _informes.inf7c = true;
                        _informes.inf7d = true;
                        _informes.inf8 = true;
                        _informes.inf8b = true;
                        _informes.inf9 = true;
                    _informes.inf9b = true;
                    _informes.inf10c = true;
                    break;
                case Tipologias.Tipo8:
                    _informes.inf1 = true;
                    _informes.inf1a = true;
           //         _informes.inf1b = true;
                    _informes.inf2 = true;
                    _informes.inf2a = true;
                    _informes.inf3 = true;
                    _informes.inf3a = true;
                    _informes.inf3b = true;
                    _informes.inf4a = true;
                    _informes.inf5a = true;
                    _informes.inf7c = true;
                    _informes.inf8 = true;
                    _informes.inf8d = true;
                    _informes.inf9a = true;
                    _informes.inf10d = true;
                    break;

                case Tipologias.Tipo8A:
                        _informes.inf1 = true;
                        _informes.inf1a = true;
          //              _informes.inf1b = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf3b = true;
                        _informes.inf4 = true;
                        _informes.inf5a = true;
                        _informes.inf6 = true;
                        _informes.inf6c = true;
                        _informes.inf7c = true;
                        _informes.inf8 = true;
                        _informes.inf8d = true;
                        _informes.inf9 = true;
                    _informes.inf9b = true;
                    _informes.inf10d = true;
                    break;
                    case Tipologias.Tipo8B:
                        _informes.inf1 = true;
                        _informes.inf1a = true;
           //             _informes.inf1b = true;
                        _informes.inf2 = true;
                        _informes.inf2a = true;
                        _informes.inf3 = true;
                        _informes.inf3a = true;
                        _informes.inf3b = true;
                        _informes.inf4a = true;
                        _informes.inf5b = true;
                        _informes.inf6a = true;
                        _informes.inf6d = true;
                        _informes.inf7c = true;
                        _informes.inf8 = true;
                        _informes.inf8d = true;
                        _informes.inf9a = true;
                    _informes.inf10d = true;
                    break;

                }
            //if (_simulacion.añosInterNat is null)
            //{
            //    nAñosIntNat = 0;
            //}
            //else
            //{
            //    nAñosIntNat = _simulacion.añosInterNat.Length;
            //}

            //if (_simulacion.añosInterAlt is null)
            //{
            //    nAñosIntAlt = 0;
            //}
            //else
            //{
            //    nAñosIntAlt = _simulacion.añosInterAlt.Length;
            //    _informes.inf8 = true;
            //}

            //if (_simulacion.listas[2].nValidos + nAñosIntNat >= 15)
            //{
            //    okNatComp = true;
            //    // apartado 2
            //    if (_simulacion.listas[3].nValidos + nAñosIntAlt >= 15)
            //    {
            //        okAltComp = true;
            //        if (_simulacion.usarCoe)
            //        {
            //            usarCoeMen = true;
            //        }
            //    }

            //    // apartado 3
            //    if (_simulacion.listas[0].nValidos >= 15)
            //    {
            //        okNatDComp = true;
            //        // apartado 4
            //        if (_simulacion.listas[1].nValidos >= 15)
            //        {
            //            okAltDComp = true;
            //            if (_simulacion.usarCoeDiara)
            //            {
            //                usarCoeDia = true;
            //            }
            //        }
            //    }
            //}
            //if (_simulacion.listas[3].nValidos + nAñosIntAlt >= 15 && nAñosIntNat == 0)
            //{
            //    okOnlyAlt = true;
            //    if(_simulacion.listas[1].nValidos<15)
            //    {
            //        okOnlyAltMensual = true;
            //    }
            //}

           // ResetInformes();

            //if (okOnlyAlt)
            //{
            //    btnCalcular.Enabled = true;

            //    _informes.inf1a = true;
            //    _informes.inf3 = true;
            //    _informes.inf3a = true;
            //    _informes.inf5b = true;
            //    _informes.inf6a = true;
            //    if (okOnlyAltMensual)
            //    {
            //        _informes.inf5a = true;
            //        _informes.inf5b = false;
            //    }

            //}
            //else if (okNatComp) //Mensuales
            //{
            //    btnCalcular.Enabled = true;
            //    _informes.inf1 = true;
            //    _informes.inf2 = true;
            //    _informes.inf2a = true;

            //    _informes.inf4a = true;
            //    _informes.inf9a = true; // Nuevo
            //    if (okAltComp)
            //    {
            //        _informes.inf6a = true;
            //        _informes.inf6d = true;
            //        _informes.inf1b = true;
            //        _informes.inf3b = true;
            //        _informes.inf6b = true;
            //        _informes.inf6e = true;
            //        _informes.inf8 = true; // Nuevo
            //        if (usarCoeMen)
            //        {
            //            _informes.inf1a = true;
            //            _informes.inf3 = true;
            //            _informes.inf3a = true;
            //            _informes.inf5b = true;
            //            _informes.inf7b = true;
            //            _informes.inf8c = true; // Nuevo
            //        }
            //        else
            //        {
            //            //_informes.inf4b = true; // Nuevo
            //            _informes.inf5a = true;
            //            _informes.inf7c = true;
            //            _informes.inf8d = true;
            //        } // Nuevo
            //    }
            //    // Diarios
            //    if (okNatDComp)
            //    {
            //        _informes.inf4 = true;
            //        _informes.inf4a = false;
            //        _informes.inf6 = true;
            //        _informes.inf6c = true;
            //        _informes.inf9a = false;
            //        _informes.inf9 = true;
            //        if (okAltDComp)
            //        {
            //            _informes.inf6b = true;
            //            _informes.inf6d = true;
            //            _informes.inf6e = true;
            //            _informes.inf6a = true;
            //            _informes.inf7d = true;
            //            // Me._informes.inf5b = False

            //            if (_informes.inf5a)
            //            {
            //                _informes.inf5a = false;
            //                //_informes.inf5c = true;
            //            }

            //            if (_informes.inf5b)
            //            {
            //                _informes.inf5b = false;
            //                _informes.inf5 = true;
            //            }

            //            if (_informes.inf7b)
            //            {
            //                _informes.inf7b = false;
            //                _informes.inf7a = true;
            //            }

            //            if (_informes.inf8c)
            //            {
            //                _informes.inf8c = false;
            //                _informes.inf8a = true;
            //            }

            //            if (_informes.inf8d)
            //            {
            //                _informes.inf8d = false;
            //                _informes.inf8b = true;
            //            }
            //        }
            //    }
            //}

            // If (okNatDComp) Then
            // 'Me.ResetInformes()
            // Me._informes.inf1 = True
            // Me._informes.inf2 = True
            // Me._informes.inf4 = True
            // Me._informes.inf6a = True
            // Me._informes.inf9 = True
            // If (okAltDComp) Then
            // Me._informes.inf6a = False
            // Me._informes.inf6 = True
            // Me._informes.inf7d = True
            // Me._informes.inf8 = True
            // If (usarCoeDia) Then
            // Me._informes.inf1b = True
            // Me._informes.inf3 = True
            // Me._informes.inf5 = True
            // Me._informes.inf7a = True
            // Me._informes.inf8a = True
            // Else
            // Me._informes.inf4b = True
            // Me._informes.inf5c = True
            // Me._informes.inf7c = True
            // Me._informes.inf8b = True
            // End If
            // End If
            // End If
         

            if (_informes.inf1)
            {
                // Me.lstBoxInformes.Items.Add("INFORME 1: VARIABILIDAD INTERANUAL RÉGIMEN NATURAL")
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1"));
            }

            if (_informes.inf1a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1a"));
            }
            if (_informes.inf1b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1b"));
            }
        
            if (_informes.inf2)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe2"));
            }

            if (_informes.inf2a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe2a"));
            }

            if (_informes.inf3)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe3"));
            }
            if (_informes.inf3a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe3a"));
            }
            if (_informes.inf3b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe3b"));
            }
            if (_informes.inf4)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4"));
            }

            if (_informes.inf4a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4a"));
            }

            if (_informes.inf5)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5"));
            }

            if (_informes.inf5a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5a"));
            }

            if (_informes.inf5b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5b"));
            }

            if (_informes.inf6)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6"));
            }

            if (_informes.inf6a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6a"));
            }
            if (_informes.inf6b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6b"));
            }
            if (_informes.inf6c)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6c"));
            }
            if (_informes.inf6d)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6d"));
            }
            if (_informes.inf6e)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6e"));
            }
            if (_informes.inf7a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7a"));
            }

            if (_informes.inf7b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7b"));
            }

            if (_informes.inf7c)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7c"));
            }

            if (_informes.inf7d)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7d"));
            }

            if (_informes.inf8)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8"));
            }

            if (_informes.inf8a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8a"));
            }

            if (_informes.inf8b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8b"));
            }

            if (_informes.inf8c)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8c"));
            }

            if (_informes.inf8d)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8d"));
            }

            if (_informes.inf9)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9"));
            }

            if (_informes.inf9a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9a"));
            }
            if (_informes.inf9b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9b"));
            }
            if (_informes.inf10a)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe10a"));
            }
            if (_informes.inf10b)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe10b"));
            }
            if (_informes.inf10c)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe10c"));
            }
            if (_informes.inf10d)
            {
                lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe10d"));
            }
        }

        //public void ResetInformes()
        //{
        //    // Reset de los informes por seguridad
        //    _informes.inf1 = false;
        //    _informes.inf1a = false;
        //    _informes.inf1b = false;
        //    _informes.inf2 = false;
        //    _informes.inf3 = false;
        //    _informes.inf3a = false;
        //    _informes.inf3b = false;
        //    _informes.inf4 = false;
        //    _informes.inf4a = false;
        //  //  _informes.inf4b = false;
        //    _informes.inf5 = false;
        //    _informes.inf5a = false;
        //    _informes.inf5b = false;
        //    //_informes.inf5c = false;
        //    _informes.inf6 = false;
        //    _informes.inf6a = false;
        //    _informes.inf6b = false;
        //    _informes.inf6c = false;
        //    _informes.inf6d = false;
        //    _informes.inf6e = false;
        //    _informes.inf7a = false;
        //    _informes.inf7b = false;
        //    _informes.inf7c = false;
        //    _informes.inf7d = false;
        //    _informes.inf8 = false;
        //    _informes.inf8a = false;
        //    _informes.inf8b = false;
        //    _informes.inf8c = false;
        //    _informes.inf8d = false;
        //    _informes.inf9 = false;
        //    _informes.inf9a = false;
        //}

        public void ResetearFormPrincipal()
        {
            XPTablaListas.TableModel.Rows.Clear();
            lstboxPuntos.SelectedIndex = -1;
            cmbListaAlteradasDiarias.Items.Clear();
            chkboxUsarCoe.Enabled = false;
            chkboxUsarCoeDiaria.Enabled = false;
            btnCalcular.Enabled = false;
            lstBoxInformes.Items.Clear();
            cbProyectos.Items.Clear();
        }

        private int GetVersionBBDD()
        {
            string sVer;
            try
            {
                var ds = _cMDB.RellenarDataSet("Conf", "SELECT * FROM [Punto]");
            }
            catch (Exception ex)
            {
                return -1;
            }

            try
            {
                var ds = _cMDB.RellenarDataSet("Conf", "SELECT * FROM [Configuracion]");
                var dr = ds.Tables[0].Rows[0];
                sVer = Conversions.ToString(dr["version"]);
            }
            catch (Exception ex)
            {
                sVer = 1.ToString();
            }

            return Conversions.ToInteger(sVer);
        }

        private TIPO_BBDD ValidarVersionBBDD(ref string message)
        {
            switch (GetVersionBBDD())
            {
                case 1:
                    {
                        message = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDBv1");
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

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */    // Private Sub AcercaDeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IdiomasToolStripMenuItem.Click
                                                           // Dim fAcercaDe As New FormAcercaDe()

        // fAcercaDe.ShowDialog()
        // End Sub

        // Private Sub ManualDeReferenciaDeUsuarioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        // Try
        // System.Diagnostics.Process.Start(".\Manual\Manual Usuario.pdf")
        // Catch ex As Exception
        // MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        // End Try
        // End Sub

        // Private Sub ManualDeReferenicaMetodológicaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        // Try
        // System.Diagnostics.Process.Start(".\Manual\Manual Referencia.pdf")
        // Catch ex As Exception
        // MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        // End Try
        // End Sub

        private void ManualesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sInfo = new ProcessStartInfo("http://ambiental.cedex.es/caudales-ambientales.php");
            Process.Start(sInfo);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        private void cmbMesInicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                // 3 Pasos:

                // 1) Actualizar el mes de inicio del punto
                // -----------------------------------------
                //string clavePunto = lstboxPuntos.SelectedItem.ToString();
                //clavePunto = _PtoSeleccionado;
                _cMDB.EjecutarSQL("UPDATE [Punto] SET mesInicio = " + (cmbMesInicio.SelectedIndex + 1).ToString() + " WHERE Id_punto =" + _PtoSeleccionado.Id);

                // 2) Actualizar las fechas de inicio y fin de las listas
                // -------------------------------------------------------
                var dtaux = _cMDB.GetTablaSQL("SELECT ID_punto, mesInicio FROM [Punto] WHERE Id_punto =" + _PtoSeleccionado.Id);

                DataTable dtauxLista;
                int idpunto = Conversions.ToInteger(dtaux.Rows[0]["ID_punto"]);
                int mesInicio = Conversions.ToInteger(dtaux.Rows[0]["mesInicio"]);
                dtaux = _cMDB.GetTablaSQL("SELECT * FROM [Lista] WHERE ID_Punto = " + idpunto);
                DataRow dr;
                for (int i = 0, loopTo = dtaux.Rows.Count - 1; i <= loopTo; i++)
                {
                    dr = dtaux.Rows[i];
                    int idLista = Conversions.ToInteger(dr["ID_Lista"]);
                    dtauxLista = _cMDB.GetTablaSQL("SELECT TOP 1 Fecha FROM [Valor] WHERE ID_Lista = " + idLista + " ORDER BY Fecha DESC");
                    var fechaFIN = DateTime.Parse(Conversions.ToString(dtauxLista.Rows[0][0]));
                    dtauxLista = _cMDB.GetTablaSQL("SELECT TOP 1 Fecha FROM [Valor] WHERE ID_Lista = " + idLista + " ORDER BY Fecha ASC");
                    var fechaINI = DateTime.Parse(Conversions.ToString(dtauxLista.Rows[0][0]));
                    if (fechaINI.Month != mesInicio | fechaINI.Day != 1)
                    {
                        if (fechaINI.Month < mesInicio)
                        {
                            fechaINI = new DateTime(fechaINI.Year - 1, mesInicio, 1);
                        }
                        else
                        {
                            fechaINI = new DateTime(fechaINI.Year, mesInicio, 1);
                        }
                    }

                    int mesFin;
                    mesFin = mesInicio - 1;
                    if (mesFin <= 0)
                    {
                        mesFin = 12;
                    }

                    int diaFin;
                    if (fechaFIN.Month != mesFin)
                    {
                        if (fechaFIN.Month > mesFin)
                        {
                            diaFin = DateTime.DaysInMonth(fechaFIN.Year + 1, mesFin);
                            fechaFIN = new DateTime(fechaFIN.Year + 1, mesFin, diaFin);
                        }
                        else
                        {
                            diaFin = DateTime.DaysInMonth(fechaFIN.Year, mesFin);
                            fechaFIN = new DateTime(fechaFIN.Year, mesFin, diaFin);
                        }
                        // fechaFIN = fechaINI.AddYears(1)
                        // fechaFIN = fechaFIN.AddDays(-1)
                    }

                    _cMDB.EjecutarSQL("UPDATE [Lista] SET Fecha_Ini = #" + fechaINI.ToString("yyyy-MM-dd") + "#, Fecha_Fin = #" + fechaFIN.ToString("yyyy-MM-dd") + "# WHERE ID_Lista =" + idLista);
                }

                // 3) Refrescar la XPTable
                // -----------------------
                lstboxPuntos_SelectedIndexChanged(null, null);
            }
            // Me.lstboxPuntos.SelectedIndex = Me.lstboxPuntos.SelectedIndex

            catch (Exception ex)
            {
            }
        }
    }
}