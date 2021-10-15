using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using global::Microsoft.VisualBasic.FileIO;
using IAHRIS.Calculo;
using IAHRIS.Rellenar;
using System.Globalization;

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
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _btnCargarLista.Name = "btnCargarLista";
            _btnExaminar.Name = "btnExaminar";

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
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _rutafichero = openFileDialog1.FileName;

                // ----------------------------------------------------------------
                // Esto tendria que sacarlo a una clase o modulo auxiliar...
                // Es mucho codigo para tan poco que hacer.
                // ----------------------------------------------------------------
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // ++++++++++++++ Leer CVS ++++++++++++++++++++++++++++++++++++++++
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                using (var parser = new TextFieldParser(_rutafichero))
                {
                    parser.SetDelimiters(delimiter);
                    fields = parser.ReadFields();

                    // CABECERA
                    // La cabecera puede tener entre 3 o 4 campos
                    // --- 4 Campos: Alterada
                    // --- 3 Campos: Natural
                    // ------------------------------------------------
                    if (fields.Length > 4 | fields.Length < 3)
                    {
                        MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeader"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    try
                    {
                        if (fields[0] == "DIARIO")
                        {
                            _cabecera.TipoFechas = true;
                            _cabecera.FormatoFecha = "dd/MM/yyyy";
                        }
                        else if (fields[0] == "MENSUAL")
                        {
                            _cabecera.TipoFechas = false;
                            _cabecera.FormatoFecha = "MM/yyyy";
                        }
                        else
                        {
                            // MessageBox.Show("Error en el campo de definición de tipo de fecha (Mensual/Diaria).")
                            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderDate"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }

                        if (fields[1] == "NATURAL")
                        {
                            _cabecera.IsNatural = true;
                        }
                        else if (fields[1] == "ALTERADO")
                        {
                            _cabecera.IsNatural = false;
                        }
                        else
                        {
                            // MessageBox.Show("Error en el campo de tipo de valores (Natural/Alterado).")
                            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderType"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }

                        _cabecera.FilePoint= fields[2];
                       
                        if (_cabecera.IsNatural == false & fields.Length != 4)
                        {
                            // MessageBox.Show("Error en la cabecera. No se encuentran todos los campos para" & vbCrLf _
                            // & "una serie alterada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderAlt"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }

                        if (_cabecera.IsNatural == false)
                        {
                            _cabecera.FileAlt = fields[3];
                            _cabecera.Clave_Alteracion = (ComboItem)cmbPuntos.SelectedItem; //fields[3];
                        }
                        else
                        {
                            _cabecera.Clave_Alteracion = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("Error en la cabecera, el formato de los campos no es correcto.", "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderGeneral"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    _rellenar = new Rellenar.RellenarForm(_cMDB);
                    var argcombo = cmbProyectos;
                    _rellenar.RellenarProyectos(ref argcombo);

                    grpboxDetalles.Enabled = true;

                    // Me.lblInfo.Text = "INFORMACIÓN: " & vbCrLf & vbCrLf & _
                    // "Tipo: " & fields(1) & vbCrLf & _
                    // "Periodicidad: " & fields(0) & vbCrLf & _
                    // "Punto: " & fields(2) & vbCrLf & vbCrLf
                    lblInfo.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo").ToUpper() + ": " + Constants.vbCrLf + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strType") + ": " + fields[1] + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPerio") + ": " + fields[0] + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPoint") + ": " + fields[2] + Constants.vbCrLf + Constants.vbCrLf;




                    if (_cabecera.IsNatural == false)
                    {
                        lblInfo.Text = lblInfo.Text + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAlt") + ": " + fields[3];
                        lblPunto.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAlt");
                    }
                    else { lblPunto.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPoint"); }

                    fields = parser.ReadFields();


                    // Testear que la alteracion (si es alteracion) pertenece al punto

                    var dt = default(DateTime);
                    // Testear fechas
                    if (_cabecera.TipoFechas)
                    {
                        if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, fields[0], ref dt))
                        {
                            // MessageBox.Show("Formato de la fechas DIARIAS no es correcto.")
                            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormatDaily"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }
                    }
                    else if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, fields[1] + "/" + fields[0], ref dt))
                    {
                        // MessageBox.Show("Formato de la fechas MENSUAL no es correcto.")
                        MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormatMonthly"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    // +++++ COMPROBAR SI LAS LISTAS ESTAN YA EN EL SISTEMA ++++++++++++++++++++
                    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    txtRuta.Text = _rutafichero;

                    cmbProyectos_SelectedIndexChanged(this, null);
                }
            }
        }

        private void btnCargarLista_Click(object sender, EventArgs e)
        {
            string[] fields;
            string delimiter = ";";
            DatosCSV[] datos;
            string sFechas;
            string sValor;
            int linea;
            long lonDatos;
            var fechaINI = default(DateTime);
            var fechaFIN = default(DateTime);
            linea = 1;
            lonDatos = 0L;
            datos = null;


            // Errores y su gestion
            // --------------------
            int nErrores = 0;
            var strErrores = new ArrayList();
            Enabled = false;
            Cursor = Cursors.WaitCursor;
            if (_cabecera.IsNatural)
            {
                _cabecera.Clave_Punto = (ComboItem)cmbPuntos.SelectedItem;
                if(_cabecera.Clave_Punto.Text.ToUpperInvariant().Trim()!= _cabecera.FilePoint.ToUpperInvariant().Trim())
                {
                    if( MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strWarningDifferentNamePoint"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)==DialogResult.Cancel)
                    {
                        Enabled = true;
                        Cursor = Cursors.Default;
                        return;
                    }
                }
            }
            else
            { _cabecera.Clave_Alteracion = (ComboItem)cmbPuntos.SelectedItem;
                if ((_cabecera.Clave_Alteracion.Text.ToUpperInvariant().Trim() != _cabecera.FileAlt.ToUpperInvariant().Trim())| (_cabecera.Clave_Punto.Text.ToUpperInvariant().Trim() != _cabecera.FilePoint.ToUpperInvariant().Trim()))
                {
                    
                    if (MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strWarningDifferentNamePoint"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        Enabled = true;
                        Cursor = Cursors.Default;
                        return;
                    }
                }
            }

            

            // ----------------------------------------------------------------
            // Esto tendria que sacarlo a una clase o modulo auxiliar...
            // Es mucho codigo para tan poco que hacer.
            // ----------------------------------------------------------------
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++++++++ Leer CVS ++++++++++++++++++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            using (var parser = new TextFieldParser(_rutafichero))
            {
                parser.SetDelimiters(delimiter);
                DataSet dsMesInicio;
                var mesInicio = default(int);
                while (!parser.EndOfData)
                {

                    // Read in the fields for the current line
                    fields = parser.ReadFields();
                    if (linea > 1)
                    {
                        // Campo de datos
                        Array.Resize(ref datos, (int)(lonDatos + 1));
                        if (_cabecera.TipoFechas)
                        {
                            sFechas = fields[0];
                            sValor = fields[1];
                        }
                        else
                        {
                            sFechas = fields[1] + "/" + fields[0];
                            sValor = fields[2];
                        }



                        // Dim s As String
                        // s = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator
                        // Dim iValor As Single
                        // ' Para que no de error por errores de punto/coma como separador decimal
                        // If (s = ".") Then
                        // iValor = Single.Parse(sValor.Replace(",", "."))
                        // Else
                        // iValor = Single.Parse(sValor.Replace(".", ","))
                        // End If

                        // ----------------------------------------------------------
                        // ----- Comprobar que el valor es válido  -----
                        // ----------------------------------------------------------
                        // If (iValor < 0) Then
                        // MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVValueNegative") & linea.ToString(), _
                        // Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                        // MessageBoxButtons.OK, MessageBoxIcon.Error)
                        // Me.Enabled = True
                        // Me.Cursor = Cursors.Default
                        // Return

                        // nErrores = nErrores + 1
                        // strErrores.Add("Error Line " + linea + ": " + sValor + " value is not valid.")
                        // End If

                        if (linea == 2)
                        {
                            // -------------------------------------------------------------------------------
                            // Generar tanto fechaINI como fechaFIN que son los inicios y fin TEORICOS.
                            // más adelante vamos a tener que comprobar que tenemos todos los datos reales.
                            // -------------------------------------------------------------------------------
                            bool okfecha = _tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref fechaINI);
                            if (!okfecha)
                            {
                                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") + linea.ToString(), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                                // MessageBox.Show("Error en la linea " & linea.ToString() & ". La fecha no esta en el formato correcto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                Enabled = true;
                                Cursor = Cursors.Default;
                                return;
                            }

                            okfecha = _tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref fechaFIN);
                            if (!okfecha)
                            {
                                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") + linea.ToString(), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                                // MessageBox.Show("Error en la linea " & linea.ToString() & ". La fecha no esta en el formato correcto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                Enabled = true;
                                Cursor = Cursors.Default;
                                return;
                            }

                            // ----------------------------------------
                            // Adecuar fecha INICIAL al año hidrologico
                            // ----------------------------------------
                            dsMesInicio = _cMDB.RellenarDataSet("Puntos", "SELECT mesInicio FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id);
                            mesInicio = Conversions.ToInteger(dsMesInicio.Tables[0].Rows[0][0]);

                            // If (fechaINI.Month <> 10 Or fechaINI.Day <> 1) Then
                            // If (fechaINI.Month < 10) Then
                            // fechaINI = New Date(fechaINI.Year - 1, 10, 1)
                            // Else
                            // fechaINI = New Date(fechaINI.Year, 10, 1)
                            // End If
                            // End If

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
                        }

                        if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref datos[(int)lonDatos].fechas))
                        {
                            // MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") & linea.ToString(), _
                            // Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                            // MessageBoxButtons.OK, MessageBoxIcon.Error)
                            // MessageBox.Show("Error en la linea " & linea.ToString() & ". La fecha no esta en el formato correcto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            // Me.Enabled = True
                            // Me.Cursor = Cursors.Default

                            // Return

                            nErrores = nErrores + 1;
                            strErrores.Add("Error Line " + linea + ": " + sFechas + " date is not valid.");
                            continue;
                        }

                        if (DateTime.Compare(fechaINI, datos[(int)lonDatos].fechas) > 0)
                        {
                            fechaINI = datos[(int)lonDatos].fechas;
                        }

                        if (DateTime.Compare(fechaFIN, datos[(int)lonDatos].fechas) < 0)
                        {
                            fechaFIN = datos[(int)lonDatos].fechas;
                        }


                        NumberFormatInfo ni = new NumberFormatInfo();
                        ni.NumberDecimalSeparator = ".";
                        // Hace un try para controlar una posible excepción
                        if (!float.TryParse(sValor, NumberStyles.AllowDecimalPoint,ni,out datos[(int)lonDatos].valores))
                        {
                            // MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVValueError") & linea.ToString(), _
                            // Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                            // MessageBoxButtons.OK, MessageBoxIcon.Error)
                            // MessageBox.Show("El formato no es correcto. Hay caracteres no reconocidos dentro del valor de la linea" & linea, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            // Me.Enabled = True
                            // Me.Cursor = Cursors.Default
                            // Return
                            nErrores = nErrores + 1;
                            strErrores.Add("Error Line " + linea + ": " + sValor + " value is not valid.");
                            continue;
                        }
                        else if (datos[(int)lonDatos].valores < 0f)
                        {
                            nErrores = nErrores + 1;
                            strErrores.Add("Error Line " + linea + ": " + sValor + " value is not valid.");
                            continue;
                        }

                        lonDatos = lonDatos + 1L;
                    }

                    linea = linea + 1;
                }

                // Si hay error muestro un mensaje de error
                if (nErrores > 0)
                {
                    if (MessageBox.Show("Se han encontrado " + nErrores + ". No se puede continuar. " + Constants.vbCrLf + "¿Desea un volcado con la información de los errores?", _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)

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
                int mesFin;
                mesFin = mesInicio - 1;
                if (mesFin <= 0)
                {
                    mesFin = 12;
                }

                int diaFin = DateTime.DaysInMonth(fechaFIN.Year, mesFin);

                // If (fechaFIN.Month <> 9 Or fechaFIN.Day <> 30) Then
                // If (fechaFIN.Month > 10) Then
                // fechaFIN = New Date(fechaFIN.Year + 1, 9, 30)
                // Else
                // fechaFIN = New Date(fechaFIN.Year, 9, 30)
                // End If
                // End If

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
                }
            }

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++++ Revisar por si hay errores o duplicidades +++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            DataSet dsPunto;
            DataSet dsAlt;
            // Dim dsNat As DataSet

            var idAlt = default(int);
            int idPunto;
            string nombrelista;

            // Se comienza ha hacer la transaccion
            _cMDB.ComenzarTransaccion();

            // Se comprueba que el punto existe y la relación con la alteración (si es una alteración) tambien existe.
            if (!ComprobarSerie())
            {
                Enabled = true;
                Cursor = Cursors.Default;
                return;
            }

            // Sacar nombre y id
            dsPunto = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id );
            var drAux = dsPunto.Tables[0].Rows[0];
            idPunto = Conversions.ToInteger(drAux[0]);
            nombrelista = _cabecera.Clave_Punto.ToString();

            // Comprobaciones que dependen si es una lista Alterada o Natural
            if (_cabecera.IsNatural == false)
            {
                nombrelista = nombrelista + "Alt";
                if (_cabecera.TipoFechas == true)
                {
                    nombrelista = nombrelista + "Diario";
                }
                else
                {
                    nombrelista = nombrelista + "Mensual";
                }

                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE ID_Alteracion =" + _cabecera.Clave_Alteracion.Id);
                var drAlt = dsAlt.Tables[0].Rows[0];
                idAlt = Conversions.ToInteger(drAlt[0]);
            }
            else
            {
                nombrelista = nombrelista + "Nat";
                if (_cabecera.TipoFechas == true)
                {
                    nombrelista = nombrelista + "Diario";
                }
                else
                {
                    nombrelista = nombrelista + "Mensual";
                }
            }

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++++ Insertar en la LISTA ++++++++++++++++++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            DataRow dr;
            dr = dsPunto.Tables[0].Rows[0];
            int i;
            var campos = new string[] { "ID_punto", "Tipo_lista", "ID_Alteracion", "Tipo_fechas", "fecha_INI", "fecha_FIN", "formato_fecha", "Nombre" };
            // if (
            var valores = new string[] { dr["id_punto"].ToString(), _cabecera.IsNatural.ToString(), idAlt.ToString(), _cabecera.TipoFechas.ToString(), fechaINI.ToString(_cabecera.FormatoFecha), fechaFIN.ToString(_cabecera.FormatoFecha), _cabecera.FormatoFecha, nombrelista + idAlt.ToString() };
            if (!_cMDB.InsertarRegistro("Lista", campos, valores))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDB") + linea.ToString(), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);


                // MessageBox.Show("Base de datos no accesible en este momento.")
                _cMDB.TerminarTransaccion(false);
                Enabled = true;
                Cursor = Cursors.Default;
                return;
            }

            // Truco: Saco la ultima que he insertado, asi se cual la lista a la que le doy valores
            dsPunto = _cMDB.RellenarDataSet("Lista", "SELECT TOP 1 id_lista FROM Lista ORDER BY id_lista DESC"); // Ultima que acabo de meter
            var id_lista = dsPunto.Tables[0].Rows[0]["id_lista"];
            campos = new string[3];
            valores = new string[3];
            string[][] mDatos;    // Lo preparo para la funcion de la base de datos
            mDatos = new string[(int)(lonDatos - 1L + 1)][];
            campos[0] = "ID_lista";
            campos[1] = "Valor";
            campos[2] = "Fecha";

            // Lista de datos a montar
            var loopTo1 = (int)(lonDatos - 1L);
            for (i = 0; i <= loopTo1; i++)
            {
                Array.Resize(ref mDatos[i], 3);
                mDatos[i][0] = Conversions.ToString(id_lista);
                mDatos[i][1] = datos[i].valores.ToString();
                mDatos[i][2] = Conversions.ToString(datos[i].fechas);
            }

            // Insercion de los valores en la base datos
            if (!_cMDB.InsertarRegistros("Valor", campos, mDatos))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDB") + linea.ToString(), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                // MessageBox.Show("Restaurando la base de datos al estado anterior a la importación.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                _cMDB.TerminarTransaccion(false);
                Enabled = true;
                Cursor = Cursors.Default;
                return;
            }

            _cMDB.TerminarTransaccion(true);
            Enabled = true;
            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_1") + Constants.vbCrLf + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_2") + ": " + linea + Constants.vbCrLf + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_3") + ": " + lonDatos, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), MessageBoxButtons.OK, MessageBoxIcon.Information);




            Cursor = Cursors.Default;
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */    /// <summary>
    /// Comprobaciones de existencia de la lista, y si es alteracion si pertence al punto
    /// </summary>
    /// <remarks></remarks>
        private bool ComprobarSerie()
        {

            // Si no hay nada, no hago nada :)
            if (_cabecera.Clave_Punto==null)
            {
                return false;
            }

            // PRIMERA COMPROBACION: ¿Esta el punto?
            // +++++++++++++++++++++++++++++++++++++
            DataSet dsPunto;
            DataSet dsAlt;
            DataRow drAux;
            int idPunto;
            var idAlt = default(int);
            dsPunto = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id);
            if (dsPunto.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointNotInDB") + " '" + _cabecera.Clave_Punto.ToString() + "' " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotInDB"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);



                return false;
            }
            else
            {
                drAux = dsPunto.Tables[0].Rows[0];
                idPunto = Conversions.ToInteger(drAux[0]);
            }

            if (_cabecera.Clave_Alteracion!=null)
            {

                // SEGUNDA COMPROBACION: ¿Existe la alteracion?
                // ++++++++++++++++++++++++++++++++++++++++++++
                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE ID_Alteracion =" + _cabecera.Clave_Alteracion.Id);
                if (dsAlt.Tables[0].Rows.Count != 1)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltNotInDB") + " '" + _cabecera.Clave_Alteracion.ToString() + "' " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotInDB"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);



                    return false;
                }
                else
                {
                    var drAlt = dsAlt.Tables[0].Rows[0];
                    idAlt = Conversions.ToInteger(drAlt[0]);
                }

                // TERCERA COMPROBACION: ¿Concuerdan Punto-Alteracion?
                // +++++++++++++++++++++++++++++++++++++++++++++++++++
                // Comprobar que la alteración pertence al punto
                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE id_alteracion=" + idAlt + " AND id_punto=" + idPunto);
                if (dsAlt.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltNotInDB") + " '" + _cabecera.Clave_Alteracion.ToString() + "' " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotAsociated") + " '" + _cabecera.Clave_Punto.ToString() + "'", _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);




                    return false;
                }
            }

            // CUARTA COMPROBACION: ¿Existe ya la lista en el sistema?
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            dsAlt = _cMDB.RellenarDataSet("LISTA", "SELECT id_lista FROM [Lista] WHERE id_alteracion=" + idAlt + " AND id_punto=" + idPunto + " AND Tipo_fechas=" + _cabecera.TipoFechas + " AND Tipo_lista=" + _cabecera.IsNatural);
            if (dsAlt.Tables[0].Rows.Count != 0)
            {
                if (MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strSerieOverwrite"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)

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
                    // Dim strError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPoint", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")
                    // Dim strCaptionError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")

                    // MessageBox.Show(strError, strCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);


                    return;
                }
                else
                {
                    _rellenar.RellenarPuntos(ref cmbPuntos, cbi.Id);
                    lblPuntoAs.Text = "";
                }
            }
            else //Regimen alterado
            {
                // Rellenar con los puntos del sistema
                var ds = _cMDB.RellenarDataSet("Alteracion", "SELECT * FROM Vista_AlteracionFull WHERE ID_Proyecto = " + cbi.Id );
                if (ds.Tables[0].Rows.Count == 0)
                {
                    // Dim strError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPoint", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")
                    // Dim strCaptionError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")

                    // MessageBox.Show(strError, strCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);


                    return;
                }
                else
                {
                    _rellenar.RellenarAlteracionesFromProj(ref cmbPuntos, cbi.Id);
                }
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
           
            if(cbi!=null) btnCargarLista.Enabled = true;

            lblPuntoAs.Text = "";
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    }
}