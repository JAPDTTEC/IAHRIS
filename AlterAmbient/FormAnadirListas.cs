using global::Microsoft.VisualBasic.FileIO;
using IAHRIS.Calculo;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormAnadirListas
    {
        public FormAnadirListas()
        {
            InitializeComponent();
            _btnCargarLista.Name = "btnCargarLista";
            _btnExaminar.Name = "btnExaminar";
        }

        public FormAnadirListas(BBDD.OleDbDataBase MDB)
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            _cMDB = MDB;
            _tFechas = new TestFechas(MDB);

            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            _traductor = MultiLangXML.MultiIdiomasXML.Instancia;

            _btnCargarLista.Name = "btnCargarLista";
            _btnExaminar.Name = "btnExaminar";

            _traductor.TraducirForm(this);
            lblPuntoAs.Text = "";
        }

        public struct CabeceraCSV
        {
            public bool TipoFechas;
            public ComboItem Clave_Punto;
            public bool IsNatural;
            public ComboItem Clave_Alteracion;
            public string FilePoint;
            public string FormatoFecha;

            public string FileAlt { get; internal set; }
        }

        public struct DatosCSV
        {
            public DateTime fechas;
            public float valores;
        }

        /// <summary>
        /// Clase mapeo de la Tabla Valor.
        /// </summary>
        public struct TablaValorDTO
        {
            public int id_lista;
            public float valor;
            public DateTime fecha;
        }


        private BBDD.OleDbDataBase _cMDB;
        private TestFechas _tFechas;
        private string _rutafichero;
        private CabeceraCSV _cabecera;
        private MultiLangXML.MultiIdiomasXML _traductor;
        private RellenarForm _rellenar;

        private void FormAnadirListas_Activated(object sender, EventArgs e)
        {
            // Centrar el form
            Left = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Width - Width));
            Top = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Height - Height));
        }

        private void FormAnadirListas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                // Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, "Añadir serie")
            }
        }

        private void FormAnadirListas_Load(object sender, EventArgs e)
        {
            grpboxDetalles.Enabled = false;

        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void btnExaminar_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            string[] fields;
            string delimiter = ";";

            openFileDialog1.Filter = "Listas de datos (*.csv)|*.csv|Todos los ficheros (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.RestoreDirectory = true;

            //leer fichero
            TextFieldParser parser;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _rutafichero = openFileDialog1.FileName;

                //leer fichero
                parser = new TextFieldParser(_rutafichero);

                try
                {

                    parser.SetDelimiters(delimiter);

                    fields = parser.ReadFields(); //Cabecera

                    //validar fichero
                    if (validarFichero(fields) != "")
                        return;

                    //Crear cabecera
                    _cabecera = obtenerCabecera(fields, (ComboItem)cmbPuntos.SelectedItem);

                    if (_cabecera.IsNatural == false & fields.Length != 4)
                    {
                        MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderAlt"),
                                        _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"),
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderGeneral"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Fin Validaciones y generación de cabeceraDTO

                _rellenar = new Rellenar.RellenarForm(_cMDB);
                var argcombo = cmbProyectos;
                _rellenar.RellenarProyectos(ref argcombo);

                grpboxDetalles.Enabled = true;

                // Cargar Label Información
                lblInfo.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo").ToUpper() + ": " + Constants.vbCrLf + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strType") + ": " + fields[1] + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPerio") + ": " + fields[0] + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPoint") + ": " + fields[2] + Constants.vbCrLf + Constants.vbCrLf;

                if (_cabecera.IsNatural == false)
                {
                    lblInfo.Text = lblInfo.Text + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAlt") + ": " + fields[3];
                    lblPunto.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAlt");
                }
                else
                {
                    lblPunto.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPoint");
                }

                fields = parser.ReadFields(); //Primera linea.

                // Testear que la alteracion (si es alteracion) pertenece al punto

                // Testear fechas
                var dt = default(DateTime);
                string fecha = _cabecera.TipoFechas ? fields[0] : fields[1] + "/" + fields[0];
                string error = _cabecera.TipoFechas ? "strErrorCSVDateFormatDaily" : "strErrorCSVDateFormatMonthly";

                if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, fecha, ref dt))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, error), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // +++++ COMPROBAR SI LAS LISTAS ESTAN YA EN EL SISTEMA ++++++++++++++++++++
                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                txtRuta.Text = _rutafichero;

                //cmbProyectos_SelectedIndexChanged(this, null);

            }
        }

        private CabeceraCSV obtenerCabecera(string[] fields, ComboItem elemento)
        {
            CabeceraCSV cabecera = new CabeceraCSV();
            cabecera.TipoFechas = fields[0] == "DIARIO";
            cabecera.FormatoFecha = fields[0] == "DIARIO" ? "dd/MM/yyyy" : "MM/yyyy";
            cabecera.IsNatural = fields[1] == "NATURAL";
            cabecera.FilePoint = fields[2];

            if (cabecera.IsNatural == false)
            {
                cabecera.FileAlt = fields[3];
                cabecera.Clave_Alteracion = elemento; //fields[3];
            }

            return cabecera;
        }

        private string validarFichero(string[] fields)
        {
            // CABECERA
            // La cabecera puede tener entre 3 o 4 campos
            // --- 4 Campos: Alterada
            // --- 3 Campos: Natural
            // ------------------------------------------------

            if (fields.Length > 4 | fields.Length < 3)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeader"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "strErrorRead";
            }

            if (fields[0] != "MENSUAL" && fields[0] != "DIARIO")
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderDate"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "strErrorRead";
            }

            if (fields[1] == "NATURAL" && fields[1] == "ALTERADO")
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderType"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "strErrorRead";
            }

            return "";

        }

        private void btnCargarLista_Click(object sender, EventArgs e)
        {
            string[] fields;
            string delimiter = ";";
            DatosCSV[] datos = null;
            string sFechas,sValor;
            int linea;
            long lonDatos;
            var fechaINI = default(DateTime);
            var fechaFIN = default(DateTime);
            linea = 0;
            lonDatos = 0L;


            // Errores y su gestion
            // --------------------
            int nErrores = 0;
            var strErrores = new ArrayList();
            Enabled = false;
            Cursor = Cursors.WaitCursor;

            //Validar clave texto
            string clavePuntoCSV = _cabecera.FilePoint.ToUpperInvariant().Trim();
            string claveAlterCSV = null;
            if (_cabecera.IsNatural == false) { 
                claveAlterCSV = _cabecera.FileAlt.ToUpperInvariant().Trim();
            }
            bool coincideText = true;
            if (_cabecera.IsNatural == true) { 
                _cabecera.Clave_Punto = (ComboItem)cmbPuntos.SelectedItem;
            }
            else
            {
                _cabecera.Clave_Alteracion = (ComboItem)cmbPuntos.SelectedItem;
            }

            //Validar datos
            if (_cabecera.Clave_Punto.Text.ToUpperInvariant().Trim() != clavePuntoCSV)
                coincideText = false;

            if (!_cabecera.IsNatural && _cabecera.Clave_Alteracion.Text.ToUpperInvariant().Trim() != claveAlterCSV) 
                coincideText = false;

            if (!coincideText)
            {
                if (MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strWarningDifferentNamePoint"),
                        _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }
            }

            if (_cabecera.IsNatural)
                _cabecera.Clave_Punto = (ComboItem)cmbPuntos.SelectedItem;
            else
                _cabecera.Clave_Alteracion = (ComboItem)cmbPuntos.SelectedItem;


            // ----------------------------------------------------------------
            // Esto tendria que sacarlo a una clase o modulo auxiliar...
            // Es mucho codigo para tan poco que hacer.
            // ----------------------------------------------------------------
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++++++++ Leer CSV ++++++++++++++++++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            using (var parser = new TextFieldParser(_rutafichero))
            {
                parser.SetDelimiters(delimiter);
                NumberFormatInfo ni = new NumberFormatInfo();
                ni.NumberDecimalSeparator = ".";

                //Obtener mes de inicio del punto.
                DataSet dsMesInicio = _cMDB.RellenarDataSet("Puntos", "SELECT mesInicio FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id);
                var mesInicio = Conversions.ToInteger(dsMesInicio.Tables[0].Rows[0][0]);


                if (!ComprobarSerie())
                {
                    Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }

                while (!parser.EndOfData)
                {
                    // Read in the fields for the current line
                    fields = parser.ReadFields();
                    linea++;

                    if (linea <= 1) //no tener en cuenta cabecera.
                        continue;

                    // Añadir fila al array
                    Array.Resize(ref datos, (int)(lonDatos + 1));

                    sFechas = _cabecera.TipoFechas ? fields[0] : fields[1] + "/" + fields[0];
                    sValor = _cabecera.TipoFechas ? fields[1] : fields[2];

                    if (linea == 2)
                    {
                        // -------------------------------------------------------------------------------
                        // Generar tanto fechaINI como fechaFIN que son los inicios y fin TEORICOS.
                        // más adelante vamos a tener que comprobar que tenemos todos los datos reales.
                        // -------------------------------------------------------------------------------
                        if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref fechaINI) ||
                            !_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref fechaFIN))
                        {
                            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") + linea.ToString(), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                            Enabled = true;
                            Cursor = Cursors.Default;
                            return;
                        }


                        // ----------------------------------------
                        // Adecuar fecha INICIAL al año hidrologico
                        // ----------------------------------------
                        if (fechaINI.Month != mesInicio | fechaINI.Day != 1)
                        {
                            if (fechaINI.Month < mesInicio)//Si el mes de inico es menor que el mes de inicio. Arranca la serie con el mes de inicio del año anterior
                                fechaINI = new DateTime(fechaINI.Year-1, mesInicio, 1);
                            else
                                fechaINI = new DateTime(fechaINI.Year, mesInicio, 1); 

                            
                          
                        }

                    }

                    if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref datos[(int)lonDatos].fechas))
                    {
                        nErrores++;
                        strErrores.Add("Error Line " + linea + ": " + sFechas + " date is not valid.");
                        continue;
                    }

                    if (DateTime.Compare(fechaINI, datos[(int)lonDatos].fechas) > 0)
                        fechaINI = datos[(int)lonDatos].fechas;


                    if (DateTime.Compare(fechaFIN, datos[(int)lonDatos].fechas) < 0)
                        fechaFIN = datos[(int)lonDatos].fechas;


                    // Validar que el numeros sea valido y mayor que 0.
                    if (!float.TryParse(sValor, NumberStyles.AllowDecimalPoint, ni, out datos[(int)lonDatos].valores) ||
                        datos[(int)lonDatos].valores < 0f)
                    {
                        nErrores++;
                        strErrores.Add("Error Line " + linea + ": " + sValor + " value is not valid.");
                        continue;
                    }

                    lonDatos++;
                }

                // Si hay error muestro un mensaje de error
                if (nErrores > 0)
                {
                    if (MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorReadList1") + nErrores + 
                                        _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorReadList2"), 
                                        _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)

                    {
                        var ofd = new SaveFileDialog();
                        ofd.AddExtension = true;
                        ofd.Filter = "Listas de datos (*.log)|*.log|Todos los ficheros (*.*)|*.*";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            var fichLog = new System.IO.StreamWriter(ofd.OpenFile());
                            for (int ind = 0, loopTo = strErrores.Count - 1; ind <= loopTo; ind++)
                                fichLog.WriteLine(strErrores[ind]);
                            fichLog.Flush();
                            fichLog.Close();
                        }
                    }

                    nErrores = 0;
                    strErrores = null;
                    Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }

                // ----------------------------------------
                // Adecuar fecha FINAL al año hidrologico
                // ----------------------------------------
                int mesFin = mesInicio - 1;
                if (mesFin <= 0) mesFin = 12;

                int diaFin = DateTime.DaysInMonth(fechaFIN.Year, mesFin);

                if (fechaFIN.Month != mesFin)
                {
                    diaFin = DateTime.DaysInMonth(fechaFIN.Year, mesFin);
                    fechaFIN = new DateTime(fechaFIN.Year, mesFin, diaFin);

                    if (fechaFIN.Month > mesFin)
                    {
                        diaFin = DateTime.DaysInMonth(fechaFIN.Year + 1, mesFin);
                        fechaFIN = new DateTime(fechaFIN.Year + 1, mesFin, diaFin);
                    }
                }

                if (!InsertarListayValores(datos, fechaINI, fechaFIN))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDB") + linea.ToString(),
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }


                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_1") +
                                    Constants.vbCrLf + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_2") + ": " +
                                    linea + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_3") + ": " +
                                    lonDatos, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                Enabled = true;
                Cursor = Cursors.Default;
                return;
            }
        }


        /// <summary>
        /// Comprobaciones de existencia de la lista, y si es alteracion si pertence al punto
        /// </summary>
        /// <remarks></remarks>
        private bool ComprobarSerie()
        {
            DataSet dsPunto;
            DataSet dsAlt;

            // Si no hay nada, no hago nada :)
            if (_cabecera.Clave_Punto == null)
            {
                return false;
            }

            // PRIMERA COMPROBACION: ¿Esta el punto?
            // +++++++++++++++++++++++++++++++++++++
            dsPunto = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id);
            if (dsPunto.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointNotInDB") + " '" + _cabecera.Clave_Punto.ToString() + "' " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotInDB"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_cabecera.Clave_Alteracion != null)
            {
                // SEGUNDA COMPROBACION: ¿Existe la alteracion?
                // ++++++++++++++++++++++++++++++++++++++++++++
                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE ID_Alteracion =" + _cabecera.Clave_Alteracion.Id);
                if (dsAlt.Tables[0].Rows.Count != 1)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltNotInDB") + " '" + _cabecera.Clave_Alteracion.ToString() + "' " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotInDB"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }


                // TERCERA COMPROBACION: ¿Concuerdan Punto-Alteracion?
                // +++++++++++++++++++++++++++++++++++++++++++++++++++
                // Comprobar que la alteración pertence al punto
                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE id_alteracion=" + _cabecera.Clave_Alteracion.Id + " AND id_punto=" + _cabecera.Clave_Punto.Id);
                if (dsAlt.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltNotInDB") + " '" + 
                                    _cabecera.Clave_Alteracion.ToString() + "' " + 
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotAsociated") + " '" + 
                                    _cabecera.Clave_Punto.ToString() + "'", 
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // CUARTA COMPROBACION: ¿Existe ya la lista en el sistema?
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //        
            if(_cabecera.IsNatural == false) { 
                dsAlt = _cMDB.RellenarDataSet("LISTA", "SELECT id_lista FROM [Lista] WHERE id_alteracion=" + _cabecera.Clave_Alteracion.Id + " AND id_punto=" + _cabecera.Clave_Punto.Id + " AND Tipo_fechas=" + _cabecera.TipoFechas + " AND Tipo_lista=" + _cabecera.IsNatural);
            }
            else
            {
                dsAlt = _cMDB.RellenarDataSet("LISTA", "SELECT id_lista FROM [Lista] WHERE id_alteracion=0" + " AND id_punto=" + _cabecera.Clave_Punto.Id + " AND Tipo_fechas=" + _cabecera.TipoFechas + " AND Tipo_lista=" + _cabecera.IsNatural);
            }

            if (dsAlt.Tables[0].Rows.Count != 0)
            {
                if (MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strSerieOverwrite"), 
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"),
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return false;
                }
                else
                {
                    var drAlt = dsAlt.Tables[0].Rows[0];
                    _cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_lista = " + drAlt[0].ToString());
                }
            }

            return true;
        }

        private void cmbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem cbi = (ComboItem)cmbProyectos.SelectedItem;
            if (_cabecera.IsNatural) //Regimen natural
            {
                // Rellenar con los puntos del sistema
                var ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM Punto WHERE ID_Proyecto = " + cbi.Id + " ORDER BY clave_punto ASC");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    //limpiar combo de puntos si no existen asociados. 
                    this.cmbPuntos.Items.Clear();
                    this.cmbPuntos.Enabled = false;
                    btnCargarLista.Enabled = false;
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"),
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.cmbPuntos.Enabled = true;
                    _rellenar.RellenarPuntos(ref cmbPuntos, cbi.Id);
                    lblPuntoAs.Text = "";
                    this.cmbPuntos.SelectedIndex = 0;
                }
                return;
            }
            else //Regimen alterado
            {
                // Rellenar con los puntos del sistema
                var ds = _cMDB.RellenarDataSet("Alteracion", "SELECT * FROM Vista_AlteracionFull WHERE ID_Proyecto = " + cbi.Id);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    this.cmbPuntos.Items.Clear();
                    this.cmbPuntos.Enabled = false;
                    btnCargarLista.Enabled = false;
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"),
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.cmbPuntos.Enabled = true;
                    _rellenar.RellenarAlteracionesFromProj(ref cmbPuntos, cbi.Id);
                }
                return;
            }
        }

        private void cmbPuntos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem cbi = (ComboItem)cmbPuntos.SelectedItem;
            if (!_cabecera.IsNatural) //Regimen alterado
            {
                var ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM Vista_AlteracionFull WHERE ID_Alteracion = " + cbi.Id + " ORDER BY clave_punto ASC");
                lblPuntoAs.Text = "Punto asociado: " + ds.Tables[0].Rows[0]["Clave_punto"].ToString();

                _cabecera.Clave_Punto = new ComboItem(ds.Tables[0].Rows[0]["Clave_punto"].ToString(), (int)ds.Tables[0].Rows[0]["Id_punto"]);


            }

            if (cbi != null) btnCargarLista.Enabled = true; else btnCargarLista.Enabled = false;

            lblPuntoAs.Text = "";
        }


        /// <summary>
        /// Con los datos leidos del fichero, genera e inserta una lista a la que añade los datos del fichero.
        /// </summary>
        /// <param name="datos">datos leídos en el fichero.</param>
        /// <param name="fechaINI">fecha Inicio serie.</param>
        /// <param name="fechaFIN">fecha Fin serie.</param>
        /// <returns></returns>
        private bool InsertarListayValores(DatosCSV[] datos, DateTime fechaINI, DateTime fechaFIN)
        {
            string[] camposTablaLista = new string[] { "ID_punto", "Tipo_lista", "ID_Alteracion", "Tipo_fechas", "fecha_INI", "fecha_FIN", "formato_fecha", "Nombre" };
            string[] valoresTablaLista;

            string[] camposTablaValor = new string[] { "ID_lista", "Valor", "Fecha" };
            string[][] valorTablaValor;

            string nombrelista = "";
            int id_lista;

            // Se comienza a hacer la transaccion
            _cMDB.ComenzarTransaccion();

            //Crear Lista            
            nombrelista = _cabecera.Clave_Punto.ToString();

            // Generar nombre en función de si la lista es natural o alterada y diaria o mensual
            nombrelista = _cabecera.IsNatural ? nombrelista + "Nat" : nombrelista + "Alt";
            nombrelista = _cabecera.TipoFechas ? nombrelista + "Diario" : nombrelista + "Mensual";

            //Insertar Lista:
            if(_cabecera.IsNatural == false) { 
                valoresTablaLista = new string[] { _cabecera.Clave_Punto.Id.ToString(), _cabecera.IsNatural.ToString(), _cabecera.Clave_Alteracion.Id.ToString(), _cabecera.TipoFechas.ToString(), fechaINI.ToString(_cabecera.FormatoFecha), fechaFIN.ToString(_cabecera.FormatoFecha), _cabecera.FormatoFecha, nombrelista + _cabecera.Clave_Alteracion.Id.ToString() };
            }
            else
            {
                valoresTablaLista = new string[] { _cabecera.Clave_Punto.Id.ToString(), _cabecera.IsNatural.ToString(), "0", _cabecera.TipoFechas.ToString(), fechaINI.ToString(_cabecera.FormatoFecha), fechaFIN.ToString(_cabecera.FormatoFecha), _cabecera.FormatoFecha, nombrelista + "0" };
            }

            if (!_cMDB.InsertarRegistro("Lista", camposTablaLista, valoresTablaLista))
            {
                _cMDB.TerminarTransaccion(false);
                return false;
            }

            //Obtener id de la lista creada
            DataSet dsLista = _cMDB.RellenarDataSet("Lista", "SELECT TOP 1 id_lista FROM Lista ORDER BY id_lista DESC"); // Ultima que acabo de meter
            id_lista = Int32.Parse(dsLista.Tables[0].Rows[0]["id_lista"].ToString());


            CultureInfo ci = CultureInfo.CreateSpecificCulture("es-ES");
            // Insercion de los valores en la base datos. Crear variable con valores obtenidos del fichero y el id lista generado.
            valorTablaValor = datos.Select(item => new string[] { id_lista.ToString(), item.valores.ToString(ci), item.fechas.ToString(_cabecera.FormatoFecha) }).ToArray();

            if (!_cMDB.InsertarRegistros("Valor", camposTablaValor, valorTablaValor))
            {
                _cMDB.TerminarTransaccion(false);
                return false;
            }

            _cMDB.TerminarTransaccion(true);
            return true;
        }
    }
}