using IAHRIS.BBDD;
using IAHRIS.Calculo;
using IAHRIS.Calculo.CaudalesEcologicos;
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using IAHRIS.Calculo.CaudalesEcologicos.Tipologias;
using IAHRIS.Calculo.Tipologias;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using MultiLangXML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using XPTable.Models;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS
{
    public partial class FormInicial
    {
        private string _RutaBBDD;
        private OleDbDataBase _cMDB;
        private RellenarForm _rellenar;
        private TestFechas _testFechas;
        private Simulacion _simulacion;
        private SerieRCE _serieRCE;
        private GeneracionInformes _informes;
        private DatosCalculo _datos;
        private ComboItem _PtoSeleccionado;
        private ComboItem _AltSeleccionada;
        private string _strPto;
        private string _strAlt;
        private string _sVersion = "v2.0 BETA";
        private int _id_proy_selec = -1;
        private ReportController _reportController;

        // Para poder centrar el formulario
        private MultiIdiomasXML _traductor;


        private Escenarios _escenarios;

        private enum TIPO_BBDD
        {
            VERSION_OK,
            VERSION_ANT_V1,
            VERSION_DESC
        }

        
        public FormInicial()
        {
            Tipologia.TipologiaConstructor();
            _escenarios = new Escenarios();
            
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
            _ManualesToolStripMenuItem.Name = "ManualesToolStripMenuItem";
            _lstboxPuntos.Name = "lstboxPuntos";
            _cmbMesInicio.Name = "cmbMesInicio";
            _chkboxUsarCoeDiaria.Name = "chkboxUsarCoeDiaria";
            _chkboxUsarCoe.Name = "chkboxUsarCoe";
            _cmbListaAlteradasDiarias.Name = "cmbListaAlteradasDiarias";
            _btnCalcular.Name = "btnCalcular";
            _cbProyectos.Name = "cbProyectos";


            // Agregue cualquier inicialización después de la llamada a InitializeComponent().
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            dGrid_Escenarios.CellValueChanged += dGrid_Escenarios_CellValueChanged; 
            dGrid_Escenarios.CellClick += dGrid_Escenarios_CellClick;


        }


        #region Metodos Propios
        /// <summary>
        /// 
        /// </summary>
        public void ActualizarCombos()
        {
            //Rellenar combo Proyectos.
            var argcombo = cbProyectos;
            _rellenar.RellenarProyectos(ref argcombo, _id_proy_selec);
            cbProyectos = argcombo;

            //Rellenar combo Puntos
            var arglistbox = lstboxPuntos;
            _rellenar.RellenarPuntos(ref arglistbox, _id_proy_selec);
            lstboxPuntos = arglistbox;
        }
        

        /// <summary>
        /// Rellena la lista con los Informes que va a exportar en el Excel
        /// </summary>
        public void RellenarListaInformes(bool esCaudalEcologico)
        {
            lstBoxInformes.Items.Clear();
            bool esTipologiaNONE = false;
            List<string> nombreInformes = new List<string>();
            string nombreTipologia = "";
            TipologiaCE tipologiaCE=null;
            if (!esCaudalEcologico ) 
            {
                Tipologia tipologia = Tipologia.GetTipologia(_simulacion);
                esTipologiaNONE = tipologia.Nombre == enumTipologias.NONE.ToString();
                _simulacion.Tipologia = tipologia;
                nombreInformes = tipologia.GetInformes().Where(x => x.Active).Select(x => x.NombreInformeXML).ToList();
                nombreTipologia = tipologia.Nombre.Replace("Tipo", "") + "  " + _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strIndices");
            }
            else
            {
                
                int idpunt = _PtoSeleccionado.Id;
                int idalte;
                if (_AltSeleccionada != null)
                {
                    idalte = _AltSeleccionada.Id;
                }              
                else
                {
                    idalte = 0;
                }

                List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
                List<EscenarioDTO> listaEscenarios = _escenarios.GetEscenarioPorIDPunto(idpunt, idalte);

                foreach (DataGridViewRow row in dGrid_Escenarios.Rows)
                {
                    // Verifica si la celda del checkbox está seleccionada
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["Seleccionar"] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null && Convert.ToBoolean(checkBoxCell.FormattedValue) == true)
                    {
                        // Agrega la fila a la lista de filas seleccionadas
                        selectedRows.Add(row);
                    }
                }

                List<EscenarioDTO> escenariosSeleccionados = new List<EscenarioDTO>();
                List<int> idSeleccionados = selectedRows.Select(row => Convert.ToInt32(row.Cells["IdEscenario"].Value)).ToList();

                foreach (EscenarioDTO escenario in listaEscenarios)
                {
                    if (idSeleccionados.Contains(escenario.Id_Escenario))
                    {
                        escenario.Puntuacion = _escenarios.CalcularPuntuacion(escenario, _serieRCE.Mes_Inicio, _serieRCE.Caudales_R_NAT.ToArray());
                        escenariosSeleccionados.Add(escenario);
                    }
                }

                bool contieneEscenario = listaEscenarios.Any(escenario =>
                    escenario.Nombre == "R_NORM" &&
                    (escenario.Caudales_Ecologicos[0] != 0 || escenario.Caudales_Ecologicos[1] != 0 ||
                    escenario.Caudales_Ecologicos[2] != 0 || escenario.Caudales_Ecologicos[3] != 0 ||
                    escenario.Caudales_Ecologicos[4] != 0 || escenario.Caudales_Ecologicos[5] != 0 ||
                    escenario.Caudales_Ecologicos[6] != 0 || escenario.Caudales_Ecologicos[7] != 0 ||
                    escenario.Caudales_Ecologicos[8] != 0 || escenario.Caudales_Ecologicos[9] != 0 ||
                    escenario.Caudales_Ecologicos[10] != 0 || escenario.Caudales_Ecologicos[11] != 0));



                tipologiaCE = TipologiaCE.GetTipologia(_serieRCE.Aportaciones_R_NAT.ToArray(), _simulacion, contieneEscenario, escenariosSeleccionados.Count);
                _serieRCE.TipologiaCE=tipologiaCE;
                esTipologiaNONE = tipologiaCE.Nombre == enumTipologias.NONE.ToString();
                if(tipologiaCE.Nombre==TipologiaCE.enumTipologiasCE.Tipo0A.ToString()|| tipologiaCE.Nombre == TipologiaCE.enumTipologiasCE.Tipo0B.ToString())
                {
                    esTipologiaNONE = true;
                }
                nombreInformes = tipologiaCE.GetInformes().Where(x => x.Active).Select(x => x.NombreInformeXML).ToList();
                nombreTipologia = tipologiaCE.Nombre.Replace("Tipo", "") + " " + _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCaudalesEcologicos");
            }

            if (!esTipologiaNONE)
            {
                btnCalcular.Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCalcular") + " " + _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strType") + " " + nombreTipologia;
                btnCalcular.Enabled = true;
            }
            else
            {
                btnCalcular.Enabled = false;
                btnCalcular.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCalcular");
            }

            
            foreach(string nombre in nombreInformes)            
                lstBoxInformes.Items.Add(Utiles.ObtenerNombreInforme(nombre));
            if (tipologiaCE != null)
            {
                if (tipologiaCE.Tipologia == TipologiaCE.enumTipologiasCE.Tipo5A || tipologiaCE.Tipologia == TipologiaCE.enumTipologiasCE.Tipo5B || tipologiaCE.Tipologia == TipologiaCE.enumTipologiasCE.Tipo6A || tipologiaCE.Tipologia == TipologiaCE.enumTipologiasCE.Tipo6B)
                {
                    lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strWarningBrokenLinks"));
                }
                if (tipologiaCE.Nombre == TipologiaCE.enumTipologiasCE.Tipo0A.ToString())
                {
                    lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strTipCE0A1"));
                    lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strTipCE0A2"));
                    lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strTipCE0A3"));
                    lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strTipCE0A4"));
                }
                if (tipologiaCE.Nombre == TipologiaCE.enumTipologiasCE.Tipo0B.ToString())
                {
                    lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strTipCE0B1"));
                    lstBoxInformes.Items.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strTipCE0B2"));
                }
            }
        }


        /// <summary>
        /// Limpiar formulario inicial.
        /// </summary>
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


        #endregion

        #region Eventos

        private void FormInicial_Load(object sender, EventArgs e)
        {
            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            _traductor = MultiIdiomasXML.Instancia;
            _RutaBBDD = Application.StartupPath + @"\IAHRISv2.mdb";
            _cMDB = new OleDbDataBase("Base", _RutaBBDD);
            _rellenar = new RellenarForm(_cMDB);
            _reportController = new ReportController(_traductor);


            var argcombo = cbProyectos;
            _rellenar.RellenarProyectos(ref argcombo);
            cbProyectos = argcombo;

            _testFechas = new TestFechas(_cMDB);

            Cursor = Cursors.WaitCursor;

            // Cargar el menu de idiomas
            CargarMenuIdiomas();

            Cursor = Cursors.Default;           

            _informes = new GeneracionInformes(true);

            tabpEscenarios.Parent = null;
            tabpValoraciones.Parent = null;

            if (_PtoSeleccionado != null)
                InicializarTabEscenarios();

            rbIndices.Enabled = false;
            rbCaudalesEco.Enabled = false;

            ValidarConfigYBBDD();

            // ------- Tabla de datos -----------------
            InicializarTablaDatos();

            _traductor.TraducirForm(this);

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


            //Añadir nombres botones mapas, (como son signos, no tienen traducción y son globales.


        }

        private void CargarMenuIdiomas()
        {
            System.Collections.ObjectModel.ReadOnlyCollection<string> files = My.MyProject.Computer.FileSystem.GetFiles(Application.StartupPath + @"\lang", Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly, "*.xml");


            foreach (var ficheroConf in files)
            {
                string strIdioma = "";
                string ruta = "";

                if (!_traductor.testFormatXML(ficheroConf, ref strIdioma, ruta))
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorLoad") + ficheroConf, _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorLoadLang"));

                XmlDocument doc = new XmlDocument();
                doc.Load(ficheroConf);

                XmlNode idiomaNode = doc.SelectSingleNode("/language/idioma");

                string idiomaTraductor = idiomaNode.InnerText;        



                var mnuIdioma = new ToolStripMenuItem(idiomaTraductor);
                mnuIdioma.Tag = ficheroConf;
                mnuIdioma.Text = idiomaTraductor;
                mnuIdioma.Click += OpcionMenu_Click;
                IdiomasToolStripMenuItem.DropDownItems.Add(mnuIdioma);

            }

            if (IdiomasToolStripMenuItem.DropDownItems.Count > 0)
            {
                IdiomasToolStripMenuItem.Visible = true;
                IdiomasToolStripMenuItem.Enabled = true;
            }

        }



        private void ValidarConfigYBBDD()
        {
            // Testear el fichero modelo
            if (!My.MyProject.Computer.FileSystem.FileExists(_traductor.getRutaExcel))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoFoundXLS") + _traductor.getRutaExcel, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFatal"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
                Dispose();
                return;
            }

            if (!My.MyProject.Computer.FileSystem.FileExists(_traductor.getRutaExcelCE))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoFoundXLS") + _traductor.getRutaExcel, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFatal"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
                Dispose();
                return;
            }

            // Testear que hay una base de datos en el directorio.
            if (!My.MyProject.Computer.FileSystem.FileExists(_RutaBBDD))                                                          
            {
                Console.WriteLine(Application.StartupPath);
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoFoundDB") + Application.ExecutablePath, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFatal"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
                Dispose();
                return;
            }
        }

        private void InicializarTabEscenarios()
        {
            this.Cursor = Cursors.WaitCursor;
            
            LimpiarTabla();


            //Obtener todos los escenarios que tiene ese punto.
            List<EscenarioDTO> listaEscenarios;
            if(_AltSeleccionada!=null) 
            { 
                listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id,_AltSeleccionada.Id);
            }
            else
            {
                listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, 0);
            }
           
            
            
            _serieRCE = SerieRCE.ObtenerSerieRCEPorSimulacion(_simulacion, listaEscenarios);


            //Si no hay escenarios, Crear Escenarios.
            if (listaEscenarios.Count <= 0)
            {
                listaEscenarios = CrearEscenariosPorDefecto(_serieRCE);
                //Recargamos para tener los Ids.
                if (_AltSeleccionada != null)
                {
                    listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, _AltSeleccionada.Id);
                }
                else
                {
                    listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, 0);
                }

            }
            if (listaEscenarios == null) return;



            //Inicilizar tabla escenarios
            NombreColumnasMeses(_serieRCE.Mes_Inicio);//modificar la cabecera de las columnas con el mes Inicio.

            int rowindex = 0;
            foreach (EscenarioDTO esc in listaEscenarios)
            {
                double[] datosMeses = esc.Caudales_Ecologicos;
                if (esc.Nombre != "R_NORM")
                {
                    this.dGrid_Escenarios.Rows.Insert(rowindex, false, esc.Id_Escenario, esc.Nombre, datosMeses[0], datosMeses[1], datosMeses[2], datosMeses[3], datosMeses[4],
                                                        datosMeses[5], datosMeses[6], datosMeses[7], datosMeses[8], datosMeses[9], datosMeses[10],
                                                        datosMeses[11], esc.Puntuacion.Puntuacion_Total, esc.Demanda_Ambiental, esc.Eficiencia);
                }
                else
                {
                    this.dGrid_Escenarios.Rows.Insert(0, false, esc.Id_Escenario, esc.Nombre, datosMeses[0], datosMeses[1], datosMeses[2], datosMeses[3], datosMeses[4],
                                        datosMeses[5], datosMeses[6], datosMeses[7], datosMeses[8], datosMeses[9], datosMeses[10],
                                        datosMeses[11], esc.Puntuacion.Puntuacion_Total, esc.Demanda_Ambiental, esc.Eficiencia);
                }
                rowindex++;

            }

            DataGridViewRow row = dGrid_Escenarios.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["Escenario"].Value.ToString() == "R_NORM");

            if (row != null)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["Seleccionar"] as DataGridViewCheckBoxCell;
                checkBoxCell.Value = true;
                checkBoxCell.Style.ForeColor = Color.DarkGray;
                checkBoxCell.Style.BackColor = Color.LightGray;

                
                checkBoxCell.FlatStyle = FlatStyle.Flat;

            }
            bool contieneEscenario = listaEscenarios.Any(escenario =>
                    escenario.Nombre == "R_NORM");



            _serieRCE.TipologiaCE = TipologiaCE.GetTipologia(_serieRCE.Aportaciones_R_NAT.ToArray(), _simulacion, contieneEscenario, 0);

            if (contieneEscenario)
            {
                añadirRNORMToolStripMenuItem.Enabled = false;
                editarRNORMToolStripMenuItem.Enabled = true;
            }else
            {
                añadirRNORMToolStripMenuItem.Enabled = true;
                editarRNORMToolStripMenuItem.Enabled = false;
            }

            RellenarListaInformes(rbCaudalesEco.Checked);


            this.Cursor = Cursors.Default;
        }
        private void ActualizarGraficos()
        {
            if (_serieRCE.Caudales_R_NAT.Count == 0) return;
            double RN_QMM = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerQmmFromCaudales(_serieRCE.Caudales_R_NAT.ToArray()));
            double RN_P50 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 50));
            //double RN_P65 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 35));
            //double RN_P70 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 30));
            double RN_P75 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 25));
            //double RN_P80 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 20));
            double RN_P85 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 15));
            double RN_P90 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 10));
            double RN_P95 = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerPercentilCaudales(_serieRCE.Caudales_R_NAT.ToArray(), 5));
            double RN_QMIN = CalculoMetodosCEs.AportacionAnualFromCaudalEco(_serieRCE.Mes_Inicio, CalculoMetodosCEs.ObtenerCaudalMinimo(_serieRCE.Caudales_R_NAT.ToArray()));


            


            List<EscenarioDTO> lineas = new List<EscenarioDTO>();


            EscenarioDTO lineaRN_P50 = new EscenarioDTO();
            //EscenarioDTO lineaRN_P65 = new EscenarioDTO();
            //EscenarioDTO lineaRN_P70 = new EscenarioDTO();
            EscenarioDTO lineaRN_P75 = new EscenarioDTO();
            //EscenarioDTO lineaRN_P80 = new EscenarioDTO();
            EscenarioDTO lineaRN_P85 = new EscenarioDTO();
            EscenarioDTO lineaRN_P90 = new EscenarioDTO();
            EscenarioDTO lineaRN_P95 = new EscenarioDTO();
            EscenarioDTO lineaRN_QMIN = new EscenarioDTO();

            lineaRN_P50.Nombre = "Damb-P50RN";
            lineaRN_P50.Demanda_Ambiental = Math.Round(RN_P50 / RN_QMM, 3);

            //lineaRN_P65.Nombre = "Damb-P65RN";
            //lineaRN_P65.Demanda_Ambiental = Math.Round(RN_P65 / RN_QMM, 3);

            //lineaRN_P70.Nombre = "Damb-P70RN";
            //lineaRN_P70.Demanda_Ambiental = Math.Round(RN_P70 / RN_QMM, 3);

            lineaRN_P75.Nombre = "Damb-P75RN";
            lineaRN_P75.Demanda_Ambiental = Math.Round(RN_P75 / RN_QMM, 3);

            //lineaRN_P80.Nombre = "Damb-P80RN";
            //lineaRN_P80.Demanda_Ambiental = Math.Round(RN_P80 / RN_QMM, 3);

            lineaRN_P85.Nombre = "Damb-P85RN";
            lineaRN_P85.Demanda_Ambiental = Math.Round(RN_P85 / RN_QMM, 3);

            lineaRN_P90.Nombre = "Damb-P90RN";
            lineaRN_P90.Demanda_Ambiental = Math.Round(RN_P90 / RN_QMM, 3);
            if (RN_P95 != RN_QMIN)
            {
                lineaRN_P95.Nombre = "Damb-P95RN";
                lineaRN_P95.Demanda_Ambiental = Math.Round(RN_P95 / RN_QMM, 3); 
                lineas.Add(lineaRN_P95);
            }
            

            lineaRN_QMIN.Nombre = "Damb-APminRN";
            lineaRN_QMIN.Demanda_Ambiental = Math.Round(RN_QMIN / RN_QMM, 3);




            lineas.Add(lineaRN_P50);
            //lineas.Add(lineaRN_P65);
            //lineas.Add(lineaRN_P70);
            lineas.Add(lineaRN_P75);
            //lineas.Add(lineaRN_P80);
            lineas.Add(lineaRN_P85);
            lineas.Add(lineaRN_P90);
           
            lineas.Add(lineaRN_QMIN); 

            List<EscenarioDTO> escenariosPunto;


            if (_AltSeleccionada != null)
            {
                escenariosPunto = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, _AltSeleccionada.Id);
            }
            else
            {
                escenariosPunto = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, 0);
            }

            
            DibujoGraficos.GraficoValoracionesEscenario(plot1, escenariosPunto, lineas);
           
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ActualizarGraficos();
        }

        private List<EscenarioDTO> CrearEscenariosPorDefecto(SerieRCE serie)
        {
            List<EscenarioDTO> listaEscenarios = new List<EscenarioDTO>();
            List<IMetodo> listaMetodosDefecto = new List<IMetodo>()
            {
                new Tennant(), 
                new Tessman(),
                new VMF(),
                new NMTM(TipoNMTM.MINIMUN),
                new NMTM(TipoNMTM.FAIR),
                new NMTM(TipoNMTM.GOOD),
                new NMTM(TipoNMTM.EXCELLENT),
                new ADMM(_serieRCE.Mes_Inicio),
                new Iahris_PXX(90, _serieRCE.Mes_Inicio),
                new Iahris_PXX(85, _serieRCE.Mes_Inicio),
                new Iahris_PXX(80, _serieRCE.Mes_Inicio)
            };

            //Obtener el id de la lista, del array del idListas, obtendremos el unico valor que haya. 
            int idLista;
            try
            {
                idLista = _simulacion.idListas.First(x => x > 0);
            }
            catch (System.InvalidOperationException e)
            {
                return null;
            }

            foreach (IMetodo metodo in listaMetodosDefecto)
            {
                EscenarioDTO escenario;
                if(_AltSeleccionada != null) 
                {
                    escenario =_escenarios.CrearEscenario(_PtoSeleccionado.Id, _AltSeleccionada.Id, serie.Caudales_R_NAT.ToArray(), _simulacion.mesInicio, metodo);
                }
                else
                {
                    escenario = _escenarios.CrearEscenario(_PtoSeleccionado.Id, 0, serie.Caudales_R_NAT.ToArray(), _simulacion.mesInicio, metodo);
                }
     
                listaEscenarios.Add(escenario);
                _escenarios.GuardaEscenarioBD(escenario);
            }
            //Escenario R_NORM

            //int idalt;
            //if(_AltSeleccionada != null)
            //{
            //    idalt = _AltSeleccionada.Id;
            //}
            //else
            //{
            //    idalt = 0;
            //}
            
            //EscenarioDTO escenarioRNORM = new EscenarioDTO() 
            //{ 
            //    Nombre = "R_NORM",
            //    Descripcion = "Régimen normativo de caudales ecológicos mínimos",
            //    Id_Punto_Ref = _PtoSeleccionado.Id,      
            //    Id_Alteracion_Ref = idalt,             
            //    Por_Defecto = true,
            //    Caudales_Ecologicos = new double[12]
            //};

            //_escenarios.GuardaEscenarioBD(escenarioRNORM);

            return listaEscenarios;
        }

        private void NombreColumnasMeses(int mesIni)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (mesIni > 12) mesIni = 1;
                this.dGrid_Escenarios.Columns["Mes" + i].HeaderText = Utiles.ObtenerMes(mesIni);
                mesIni++;
            }
        }

        private void LimpiarTabla()
        {
            this.dGrid_Escenarios.Rows.Clear();
            this.dGrid_Escenarios.Refresh();
        }

        private void InicializarTablaDatos()
        {
            XPTablaListas.ColumnResizing = false;
            XPTablaListas.HeaderRenderer.Font = new Font(XPTablaListas.HeaderRenderer.Font.FontFamily, 7.5f);

            XPTablaListas.BeginUpdate();
            XPTablaListas.NoItemsText = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "noitem");

            // Columna de los años
            TextColumn colTexto = new TextColumn();
            colTexto = (TextColumn)CrearColumna(colTexto, false, true, false, 67, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "year"));

            ImageColumn imgColND = new ImageColumn();
            imgColND = (ImageColumn)CrearColumna(imgColND, false, false, false, 50, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynat"));

            CheckBoxColumn chkColND = new CheckBoxColumn();
            chkColND = (CheckBoxColumn)CrearColumna(chkColND, false, false, false, 50, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynatinter"));

            ImageColumn imgColAD = new ImageColumn();
            imgColAD = (ImageColumn)CrearColumna(imgColAD, false, false, true, 55, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyalt"));

            CheckBoxColumn chkColAD = new CheckBoxColumn();
            chkColAD = (CheckBoxColumn)CrearColumna(chkColAD, false, false, false, 50, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyaltinter"));

            ImageColumn imgColCD = new ImageColumn();
            imgColCD = (ImageColumn)CrearColumna(imgColCD, false, false, true, 50, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coedaily"));

            ImageColumn imgColNM = new ImageColumn();
            imgColNM = (ImageColumn)CrearColumna(imgColNM, false, false, true, 55, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynat"));

            CheckBoxColumn chkColNM = new CheckBoxColumn();
            chkColNM = (CheckBoxColumn)CrearColumna(chkColNM, false, false, false, 50, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynatinter"));

            ImageColumn imgColAM = new ImageColumn();
            imgColAM = (ImageColumn)CrearColumna(imgColAM, false, false, true, 55, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyalt"));

            CheckBoxColumn chkColAM = new CheckBoxColumn();
            chkColAM = (CheckBoxColumn)CrearColumna(chkColAM, false, false, false, 50, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyaltinter"));

            ImageColumn imgColCM = new ImageColumn();
            imgColCM = (ImageColumn)CrearColumna(imgColCM, false, false, true, 50, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coemonthly"));


            var cols = new Column[] { colTexto, imgColNM, chkColNM, imgColAM, chkColAM, imgColCM, imgColND, chkColND, imgColAD, chkColAD, imgColCD };
            ColumnModel1.Columns.AddRange(cols);
            XPTablaListas.EndUpdate();
        }

        private Column CrearColumna(Column column, bool editable, bool sortable, bool imageOnRight, int width, string text)
        {
            column.Editable = editable;
            column.Sortable = sortable;
            column.ImageOnRight = imageOnRight;
            column.Width = width;
            column.Text = text;

            return column;
        }

        private void ActualizarTablaIndicesHidro(int idpunto, int idAlteracion, int mesInicio)
        {
            // Rellenar la tabla con los años y los totales
            TestFechas.Simulacion localRellenarXPTable()
            {
                var argtabla = XPTablaListas;
                var ret = _rellenar.RellenarXPTable(ref argtabla, idAlteracion, idpunto, chkboxUsarCoe.Checked, chkboxUsarCoeDiaria.Checked);
                XPTablaListas = argtabla;
                return ret;
            }

            _simulacion = localRellenarXPTable();
            _simulacion.mesInicio = mesInicio;


            lblAñosNatMensual.Text = _simulacion.añosInterNat is object ? (_simulacion.listas[2].nValidos + _simulacion.añosInterNat.Length).ToString() :
                                                               _simulacion.listas[2].nValidos.ToString();

            lblAñosAltMensual.Text = _simulacion.añosInterAlt is object ? (_simulacion.listas[3].nValidos + _simulacion.añosInterAlt.Length).ToString() :
                                                                           _simulacion.listas[3].nValidos.ToString();


            lblAñosCoeMensual.Text = (_simulacion.añosInterCoe is object) ? (_simulacion.coe[1].nCoetaneos + _simulacion.añosInterCoe.Length).ToString() :
                                                                            _simulacion.coe[1].nCoetaneos.ToString();


            //Actualizar etiquetas con años de la serie.
            lblAñosHidro.Text = (_simulacion.fechaFIN - _simulacion.fechaINI).ToString();
            lblAñosNatDiario.Text = _simulacion.listas[0].nValidos.ToString();
            lblAñosNatDiarioUSO.Text = _simulacion.añosParaCalculo[0].nAños.ToString();

            lblAñosAltDiario.Text = _simulacion.listas[1].nValidos.ToString();
            lblAñosAltDiarioUSO.Text = _simulacion.añosParaCalculo[1].nAños.ToString();

            lblAñosCoeDiaria.Text = _simulacion.coe[0].nCoetaneos.ToString();
            lblAñosNatMensualUSO.Text = _simulacion.añosParaCalculo[2].nAños.ToString();
            lblAñosAltMensualUSO.Text = _simulacion.añosParaCalculo[3].nAños.ToString();

            // Cambiar colores si los años no llegan al minimo.
            lblAñosNatMensual.ForeColor = (int.Parse(lblAñosNatMensual.Text) < 15 & int.Parse(lblAñosNatMensual.Text) != 0) ? Color.Red : Color.Black;
            lblAñosAltMensual.ForeColor = (int.Parse(lblAñosAltMensual.Text) < 15 & int.Parse(lblAñosAltMensual.Text) != 0) ? Color.Red : Color.Black;
            lblAñosNatDiario.ForeColor = (int.Parse(lblAñosNatDiario.Text) < 15 & int.Parse(lblAñosNatDiario.Text) != 0) ? Color.Red : Color.Black;
            lblAñosAltDiario.ForeColor = (int.Parse(lblAñosAltDiario.Text) < 15 & int.Parse(lblAñosAltDiario.Text) != 0) ? Color.Red : Color.Black;
        }

        private void FormInicial_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _cMDB.Desconectar();
                _cMDB = null;
                Application.OpenForms["FormBienvenida"].Close();
            }
            catch (Exception)
            {
            }
        }

        private void AñadirProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var fanadirproyecto = new FormAnadirProyecto(_cMDB, false);
            fanadirproyecto.ShowDialog();
            ActualizarCombos();
        }

        private void EliminarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var feliminarproyecto = new FormEliminarProyecto(_cMDB);
            feliminarproyecto.ShowDialog();    
            ActualizarCombos();
            cbProyectos_SelectedIndexChanged(null, null);
        }

        private void EditarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_cbProyectos.SelectedIndex != -1)
            {
                var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");
                var dr = ds.Tables[0].Rows[cbProyectos.SelectedIndex];
                int p = Convert.ToInt32(dr[0]);

                ResetearFormPrincipal();
                var fanadirproyecto = new FormAnadirProyecto(_cMDB, true, p);
                fanadirproyecto.ShowDialog();
                ActualizarCombos();
            }
            else
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorProjectDontExist"));
            }

        }

        private void AñadirPuntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();

            // Enseño el formulario como un dialog
            var fanadir = new FormAnadirPunto(_cMDB, "Punto", false, 0);
            fanadir.ShowDialog();

            ActualizarCombos();
        }
        
        private void _EditarPuntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ResetearFormPrincipal();

                if (_PtoSeleccionado is null)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strErrorImportList7"));
                    return;
                }

                if (_PtoSeleccionado.Id == -1)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO,"strErrorImportList7"));
                    return;
                }

                var fanadir = new FormAnadirPunto(_cMDB, "Punto", true, _PtoSeleccionado.Id);
                fanadir.ShowDialog();

                ActualizarCombos();
            }
            catch (Exception)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strErrorImportList7"));
            }
        }

        private void AñadirAlteraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Enseño el formulario como un dialog
            ComboItem proySeleccionado = (ComboItem)cbProyectos.SelectedItem;

            if (_lstboxPuntos.Items.Count != 0)
            {
                var fanadir = new FormAnadirPunto(_cMDB, "Alteración", false, proySeleccionado.Id);
                fanadir.ShowDialog();
                ActualizarCombos();
                return;
            }

            MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"),
                            _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;

        }
        
        private void _EditarAlteraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbListaAlteradasDiarias.SelectedItem == null)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorAltNotSelected"));
                    return;
                }

                _AltSeleccionada = (ComboItem)cmbListaAlteradasDiarias.SelectedItem;
                if (_AltSeleccionada.Id == -1)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorAltNotSelected"));
                    return;
                }                  

                
                var fanadir = new FormAnadirPunto(_cMDB, "Alteración", true, _AltSeleccionada.Id);
                fanadir.ShowDialog();
                ActualizarCombos();
            }
            catch (Exception)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorAltNotSelected"));
            }

        }

        private void AñadirListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var fanadir = new FormAnadirListas(_cMDB);

            fanadir.ShowDialog();
            ActualizarCombos();

            _cMDB.Desconectar();

           // _cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
        }

        private void EliminarListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var feliminar = new FormEliminarLista(_cMDB);
            feliminar.ShowDialog();
            ActualizarCombos();
            _cMDB.Desconectar();

        }

        private void EliminarPuntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var feliminar = new FormEliminarPunto(_cMDB, "Punto");
            feliminar.ShowDialog();
            ActualizarCombos();

            _cMDB.Desconectar();

        }

        private void EliminarAlteraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();

            var feliminar = new FormEliminarPunto(_cMDB, "Alteracion");
            feliminar.ShowDialog();

            _cMDB.Desconectar();
            ActualizarCombos();
        }

        private void ImportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fimportar = new FormImportar(_cMDB);
            fimportar.ShowDialog();
            ActualizarCombos();
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
                if (sfd.ShowDialog() != DialogResult.OK) return;
                
                My.MyProject.Computer.FileSystem.CopyFile(_RutaBBDD, sfd.FileName, true);
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strExportacion") + Constants.vbCrLf + sfd.FileName, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
            }
            catch (Exception ex)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorExport") + Constants.vbCrLf + ex.Message, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void OpcionMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem OpcionSeleccionada = (ToolStripMenuItem)sender;

            // OpcionSeleccionada.Tag
            if (!_traductor.CambiarIdioma(Conversions.ToString(OpcionSeleccionada.Tag)))            
                return;
            

            _traductor = MultiIdiomasXML.Instancia; //Refrescar traductor para obtener el cambio de idioma.
            _traductor.TraducirForm(this);

            XPTablaListas.BeginUpdate();
            XPTablaListas.NoItemsText = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "noitem");
            XPTablaListas.ColumnModel.Columns[0].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "year");
            XPTablaListas.ColumnModel.Columns[6].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynat");
            XPTablaListas.ColumnModel.Columns[7].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynatinter");
            XPTablaListas.ColumnModel.Columns[8].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyalt");
            XPTablaListas.ColumnModel.Columns[9].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyaltinter");
            XPTablaListas.ColumnModel.Columns[10].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coedaily");
            XPTablaListas.ColumnModel.Columns[1].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynat");
            XPTablaListas.ColumnModel.Columns[2].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynatinter");
            XPTablaListas.ColumnModel.Columns[3].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyalt");
            XPTablaListas.ColumnModel.Columns[4].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyaltinter");
            XPTablaListas.ColumnModel.Columns[5].Text = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coemonthly");
            XPTablaListas.EndUpdate();
            if (lstBoxInformes.Items.Count > 0)
            {
                RellenarListaInformes(rbCaudalesEco.Checked);
            }

            var myBuildInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            Text += " - v" + myBuildInfo.FileMajorPart + "." + myBuildInfo.FileMinorPart;

            MessageBox.Show( _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strIdiomaCambiado"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNombreProyectos.Text = "";
            lblProyectoDesc.Text = "";

            int localRellenarProyectosDesc() 
            { 
                var arglabel = lblProyectoDesc; 
                var ret = _rellenar.RellenarProyectosDesc(ref arglabel, cbProyectos.SelectedIndex); 
                lblProyectoDesc = arglabel; return ret; 
            }
            int localRellenarProyectosNomb()
            {
                var arglabel = lblNombreProyectos;
                var ret = _rellenar.RellenarProyectosNomb(ref arglabel, cbProyectos.SelectedIndex);
                lblNombreProyectos = arglabel; return ret;
            }
            localRellenarProyectosNomb();
            _id_proy_selec = localRellenarProyectosDesc();

            var arglistbox = lstboxPuntos;
            _rellenar.RellenarPuntos(ref arglistbox, _id_proy_selec);
            lstboxPuntos = arglistbox;
            lstboxPuntos_SelectedIndexChanged(null, null);

            // Borro la tabla
            XPTablaListas.TableModel.Rows.Clear();
            cmbListaAlteradasDiarias_SelectedIndexChanged(null, null);

        }
 
        private void lstboxPuntos_SelectedIndexChanged(object sender, EventArgs e)
        {
            long id_punto;
            string nombre;
            int mesInicio;
            int nAlt;
            DataSet ds;
            DataRow dr;

            //Inicializar etiquetas
            lblSerieNatDiaria.Text = "--";
            lblSerieNatMensual.Text = "--";
            lblPuntosClave.Text = "";
            lblNombreRegNaturales.Text = "";
            lblAbrevRegNaturales.Text = "";
            lblAbrevRegAlterados.Text = "";
            lblPuntosNombre.Text = "";
            lblPuntosNListas.Text = "";
            cmbListaAlteradasDiarias.Items.Clear();
            _AltSeleccionada = null;

            if (lstboxPuntos.SelectedItem != null)
            {
                _PtoSeleccionado = (ComboItem)lstboxPuntos.SelectedItem;

                // Sacar nombre y id
                ds = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto, nombre, mesInicio FROM Punto WHERE ID_Punto=" + ((ComboItem)lstboxPuntos.SelectedItem).Id);
                dr = ds.Tables[0].Rows[0];
                id_punto = (int)dr["id_punto"];
                nombre = dr["nombre"].ToString();
                mesInicio = Int32.Parse(dr["mesInicio"].ToString());
                _strPto = _PtoSeleccionado + "-" + nombre;

                // Sacar el numero de alteraciones
                ds = _cMDB.RellenarDataSet("Alt", "SELECT DISTINCT id_alteracion FROM [alteracion] WHERE id_alteracion > 0 AND id_punto=" + id_punto);
                nAlt = ds.Tables[0].Rows.Count;

                // Rellenar nombres de las series en el combo lista
                bool hayDiaria = default, hayMensual = default;
                string stNone = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");
                string stSI = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "yes").ToUpper();
                string stNO = _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "no").ToUpper();

                var argcomboAltD = cmbListaAlteradasDiarias;
                _rellenar.RellenarListas(ref hayDiaria, ref hayMensual, ref argcomboAltD, (int)id_punto, stNone);
                cmbListaAlteradasDiarias = argcomboAltD;    

                lblSerieNatDiaria.Text = (hayDiaria) ? stSI : stNO;
                lblSerieNatMensual.Text = (hayMensual) ? stSI : stNO;

                lblPuntosClave.Text = lstboxPuntos.SelectedItem.ToString(); ;
                lblPuntosNombre.Text = nombre;
                lblPuntosNListas.Text = nAlt.ToString();
                cmbMesInicio.SelectedIndex = mesInicio - 1;
                _simulacion.mesInicio = mesInicio;

                var arglabel1 = lblNombreRegNaturales;
                var arglabel2 = lblAbrevRegNaturales;
                
                _rellenar.RellenarPersRegPunto(ref arglabel1, ref arglabel2, (int)id_punto);

                if (rbCaudalesEco.Checked)
                {
                    InicializarTabEscenarios();
                    ActualizarGraficos();
                }
                rbIndices.Enabled = true;
                rbCaudalesEco.Enabled = true;

              

                if (cmbListaAlteradasDiarias_FormerIndex < 1) //Esto quiere decir que no había seleccionada una alteración, por lo 
                                                              //que el refresco no va a ser gestionado por el evento del combo de alteracion
                {                                               //y tenemos que ahcerlo nosotros
                    RellenarXPTable(-1);
                    RellenarListaInformes(rbCaudalesEco.Checked);
                }
            }
            else
            {
                // Deshabilitar los RadioButtons si no se ha seleccionado ningún elemento
                rbIndices.Enabled = false;
                rbCaudalesEco.Enabled = false;
            }
        }

        private void chkboxUsarCoe_CheckedChanged(object sender, EventArgs e)
         {
            var idpunto = default(int);
            int idAlteracion = -1;
            var mesInicio = default(int);
            DataSet ds;
            DataRow dr;
            string stNone = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");

            if (cmbListaAlteradasDiarias.SelectedItem != null && cmbListaAlteradasDiarias.SelectedItem.ToString() != stNone)
            {
                idAlteracion = ((ComboItem)cmbListaAlteradasDiarias.SelectedItem).Id;
            }

            if (_PtoSeleccionado != null)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Id_punto =" + _PtoSeleccionado.Id);
                
                dr = ds.Tables[0].Rows[0];
                idpunto = Conversions.ToInteger(dr["id_punto"]);
                mesInicio = Conversions.ToInteger(dr["mesInicio"]);
            }


            // Rellenar la tabla con los años y los totales
            ActualizarTablaIndicesHidro(idpunto, idAlteracion, mesInicio);
            RellenarListaInformes(rbCaudalesEco.Checked);
        }

        private void chkboxUsarCoeDiaria_CheckedChanged(object sender, EventArgs e)
        {
            var idpunto = default(int);
            int idAlteracion = -1;
            var mesInicio = default(int);
            DataSet ds;
            DataRow dr;
            string stNone = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");
            
            if (cmbListaAlteradasDiarias.SelectedItem != null && cmbListaAlteradasDiarias.SelectedItem.ToString() != stNone)
            {
                idAlteracion = ((ComboItem)cmbListaAlteradasDiarias.SelectedItem).Id; 
            }


            if (_PtoSeleccionado != null)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Id_punto =" + _PtoSeleccionado.Id);

                dr = ds.Tables[0].Rows[0];
                idpunto = Conversions.ToInteger(dr["id_punto"]);
                mesInicio = Conversions.ToInteger(dr["mesInicio"]);
            }

            // Rellenar la tabla con los años y los totales
            ActualizarTablaIndicesHidro(idpunto, idAlteracion, mesInicio);
            RellenarListaInformes(rbCaudalesEco.Checked);
        }

        int cmbListaAlteradasDiarias_FormerIndex = 0;

        private void cmbListaAlteradasDiarias_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem sItem;
            int nAlt;
            DataSet ds;
            DataRow dr;

            if (cmbListaAlteradasDiarias_FormerIndex == cmbListaAlteradasDiarias.SelectedIndex)
                return;
            else
                cmbListaAlteradasDiarias_FormerIndex = cmbListaAlteradasDiarias.SelectedIndex;

            if (cmbListaAlteradasDiarias.SelectedIndex == -1)
            {
                btnCalcular.Enabled = false;
                lblNombreAlteraciones.Text = "---";
                lblCodigoAlt.Text = "---";
                lblNombreRegAlterados.Text = "---";
                lblAbrevRegAlterados.Text = "---";

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

            if ((cmbListaAlteradasDiarias.SelectedItem != null) && cmbListaAlteradasDiarias.SelectedItem.ToString() != stNone)
            {
                sItem = (ComboItem)cmbListaAlteradasDiarias.SelectedItem;
                _AltSeleccionada = sItem;


                // Saco el codigo de la alteracion
                ds = _cMDB.RellenarDataSet("Alt", "SELECT nombre,COD_Alteracion FROM [Alteracion] WHERE id_alteracion =" + sItem.Id);
                dr = ds.Tables[0].Rows[0];

                lblCodigoAlt.Text = dr[0].ToString(); ;
                lblNombreAlteraciones.Text = dr[1].ToString();
                _strAlt = sItem + "-" + lblCodigoAlt.Text;
                nAlt = sItem.Id;

                string stSI = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "yes").ToUpper();
                string stNO = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "no").ToUpper();
                ds = _cMDB.RellenarDataSet("Alt", "SELECT Nombre FROM [Lista] WHERE ID_Alteracion=" + nAlt + " AND Tipo_fechas=True");

                lblIDDiaria.Text = (ds.Tables[0].Rows.Count > 0) ? stSI : stNO;


                ds = _cMDB.RellenarDataSet("Alt", "SELECT Nombre FROM [Lista] WHERE ID_Alteracion=" + nAlt + " AND Tipo_fechas=false");
                lblIDMensual.Text = (ds.Tables[0].Rows.Count > 0) ? stSI : stNO;

                var arglabel1 = lblNombreRegAlterados;
                var arglabel2 = lblAbrevRegAlterados;

                _rellenar.RellenarPersRegAlteracion(ref arglabel1, ref arglabel2, sItem.Id);

            }
            else
            {
                lblNombreAlteraciones.Text = "---";
                lblCodigoAlt.Text = "---";
                lblNombreRegAlterados.Text = "---";
                lblAbrevRegAlterados.Text = "---";
                lblIDDiaria.Text = "---";
                lblIDMensual.Text = "---";
                _strAlt = "";
            }

            RellenarXPTable(nAlt);

            if (rbCaudalesEco.Checked)

            {
                List<EscenarioDTO> listaEscenarios;
                if (_AltSeleccionada != null)
                {
                    listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, _AltSeleccionada.Id);
                }
                else
                {
                    listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, 0);
                }

                _serieRCE = SerieRCE.ObtenerSerieRCEPorSimulacion(_simulacion, listaEscenarios);
                InicializarTabEscenarios();
                ActualizarGraficos();
            }
            RellenarListaInformes(rbCaudalesEco.Checked);

        }

        private void RellenarXPTable(int nAlt)
        {
            // ++++++++++++++++++++++++++++++++++++++++++
            // +++++++ Rellenar la XP table +++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++
            var idpunto = default(int);
            int idAlteracion = -1;
            var mesInicio = default(int);
            DataSet ds;
            DataRow dr;
            string stNone = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone");

            if (cmbListaAlteradasDiarias.SelectedItem.ToString() != stNone)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_alteracion FROM [Alteracion] WHERE Id_Alteracion=" + nAlt);
                dr = ds.Tables[0].Rows[0];
                idAlteracion = Conversions.ToInteger(dr[0]);
            }

            if (_PtoSeleccionado != null)
            {
                ds = _cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Id_punto=" + _PtoSeleccionado.Id);
                dr = ds.Tables[0].Rows[0];
                idpunto = Conversions.ToInteger(dr["id_punto"]);
                mesInicio = Conversions.ToInteger(dr["mesInicio"]);
            }

            // Rellenar la tabla con los años y los totales
            ActualizarTablaIndicesHidro(idpunto, idAlteracion, mesInicio);

            if (_simulacion.coe[1].nCoetaneos >= 15 & _simulacion.coe[1].nCoetaneos < int.Parse(lblAñosNatMensual.Text))
            {
                chkboxUsarCoe.Enabled = true;
                chkboxUsarCoe.Checked = true;
            }
            else if (_simulacion.coe[1].nCoetaneos >= 7 & _simulacion.coe[1].nCoetaneos < int.Parse(lblAñosNatMensual.Text))
            {
                chkboxUsarCoe.Enabled = false;
                chkboxUsarCoe.Checked = false;
            }
            else
            {
                chkboxUsarCoe.Enabled = false;
                if (_simulacion.coe[1].nCoetaneos == int.Parse(lblAñosNatMensual.Text) & _simulacion.coe[1].nCoetaneos > 0)
                    chkboxUsarCoe.Checked = true;
                else
                    chkboxUsarCoe.Checked = false;
            }

            if (_simulacion.coe[0].nCoetaneos >= 7 & _simulacion.coe[0].nCoetaneos < int.Parse(lblAñosNatDiario.Text))
            {
                chkboxUsarCoeDiaria.Enabled = true;
                chkboxUsarCoeDiaria.Checked = false;
            }
            else
            {
                chkboxUsarCoeDiaria.Enabled = false;
                if (_simulacion.coe[0].nCoetaneos == int.Parse(lblAñosNatDiario.Text) & _simulacion.coe[0].nCoetaneos > 0)
                    chkboxUsarCoeDiaria.Checked = true;
                else
                    chkboxUsarCoeDiaria.Checked = false;
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            _simulacion.sNombre = _strPto;
            _simulacion.sAlteracion = _strAlt;
            _simulacion.añosCoetaneosTotales = Conversions.ToInteger(lblAñosCoeMensual.Text);

            //Buscamos directamente el punto.
            var ds = _cMDB.RellenarDataSet("Punto", "SELECT nombreRegimen, abreviaturaRegimen FROM [Punto] WHERE ID_Punto = " + _PtoSeleccionado.Id );
            var dr = ds.Tables[0].Rows[0];

            _simulacion.nombreRegimenPunto = dr[0].ToString();
            _simulacion.abreviaturaRegimenPunto = dr[1].ToString();
            

            //Si tiene alteración
            if (_cmbListaAlteradasDiarias.SelectedIndex > 0)
            {
                ComboItem alterSeleccionada = (ComboItem) _cmbListaAlteradasDiarias.SelectedItem;

                var ds2 = _cMDB.RellenarDataSet("Alteracion", "SELECT nombreRegimen, abreviaturaRegimen FROM [Alteracion] WHERE ID_Alteracion = " + alterSeleccionada.Id);
                var dr2 = ds2.Tables[0].Rows[0];

                _simulacion.nombreRegimenAlteracion = dr2[0].ToString();
                _simulacion.abreviaturaRegimenAlteracion = dr2[1].ToString();
            }
            if (rbIndices.Checked)
            {
                var fCalcular = new FormCalculo(_simulacion, _informes, _cMDB);
                Enabled = false;
                Cursor = Cursors.WaitCursor;
                fCalcular.ShowDialog();
                Enabled = true;
                Cursor = Cursors.Default;
            }
            if (rbCaudalesEco.Checked)
            {
                List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();

                List<EscenarioDTO> escenariosPunto;
                if (_AltSeleccionada != null)
                {
                    escenariosPunto = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, _AltSeleccionada.Id);
                }
                else
                {
                    escenariosPunto = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, 0);
                }

                // Recorre todas las filas del DataGridView
                foreach (DataGridViewRow row in dGrid_Escenarios.Rows)
                {
                    // Verifica si la celda del checkbox está seleccionada
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["Seleccionar"] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null && Convert.ToBoolean(checkBoxCell.FormattedValue) == true)
                    {
                        // Agrega la fila a la lista de filas seleccionadas
                        selectedRows.Add(row);
                    }
                }
                
                List<EscenarioDTO> escenariosSeleccionados = new List<EscenarioDTO>();
                List<int> idSeleccionados = selectedRows.Select(row => Convert.ToInt32(row.Cells["IdEscenario"].Value)).ToList();

                foreach (EscenarioDTO escenario in escenariosPunto)
                {
                    if (idSeleccionados.Contains(escenario.Id_Escenario))                    
                        escenariosSeleccionados.Add(escenario);                    
                }

                _serieRCE.Nombre_Regimen_Nat = _simulacion.nombreRegimenPunto;
                _serieRCE.Abrev_Regimen_Nat = _simulacion.abreviaturaRegimenPunto;

                if (_simulacion.nombreRegimenAlteracion != null)
                {
                    _serieRCE.Abrev_Regimen_Alt = _simulacion.abreviaturaRegimenAlteracion;
                    _serieRCE.Nombre_Regimen_Alt = _simulacion.nombreRegimenAlteracion;
                }

                //obtenemos los años de la serie para calcular la tipologia.
                _serieRCE.Lista_Escenarios = escenariosPunto;

                _serieRCE.Lista_Escenarios_Seleccionados = escenariosSeleccionados;
                _serieRCE.Lista_Escenarios_Predefinidos = escenariosPunto.Where(x => x.Por_Defecto == true).ToList();

                bool contieneEscenario = escenariosPunto.Any(escenario =>
                    escenario.Nombre == "R_NORM" );
                




                _serieRCE.TipologiaCE = TipologiaCE.GetTipologia(_serieRCE.Aportaciones_R_NAT.ToArray(), _simulacion, contieneEscenario, _serieRCE.Lista_Escenarios_Seleccionados.Count);


                //TODO: Habrá que crear un metodo de Validar, para comprobar que seriesRCE contenga todo lo necesario antes de continuar
                // y devolver un mensaje de error en caso de que no ocurra.
                string error = ValidarSerieRCE(_serieRCE);
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Obtener ruta excel.
                string nombre = _PtoSeleccionado.Text + "-" + _serieRCE.Nombre_Regimen_Nat +"-"+_serieRCE.TipologiaCE.Nombre +"-"+DateTime.Now.ToShortDateString().Replace('/','-') + ".xlsx";
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Archivos Office Open XML Hoja de Cálculo (*.xlsx)|*.xlsx|Todos los ficheros (*.*)|*.*";
                    saveDialog.FilterIndex = 1;
                    saveDialog.FileName = nombre;
                    saveDialog.OverwritePrompt = false;

                    DialogResult result = saveDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(saveDialog.FileName))
                    {
                        string filePath = saveDialog.FileName;

                        // Verificar si el archivo ya existe
                        if (System.IO.File.Exists(filePath)) 
                        {
                            // Preguntar al usuario si desea sobrescribir el archivo existente
                            DialogResult overwriteResult = MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOverwrite"), 
                                                                            _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strSobrescribir)"),
                                                                            MessageBoxButtons.YesNo);

                            if (overwriteResult == DialogResult.Yes)
                            {
                                // Sobrescribir el archivo
                                
                                _reportController.GenerarInformeCE( _serieRCE.TipologiaCE, _simulacion,  _serieRCE, filePath);
                                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInformeGenerado") + " \n" + filePath); 
                            }
                        }
                        else
                        {
                            // El archivo no existe, generar el informe directamente
                            _reportController.GenerarInformeCE(_serieRCE.TipologiaCE, _simulacion, _serieRCE, filePath);
                            MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInformeGenerado") + " \n"+ filePath);
                        }
                    }
                }
            }
        }

        //TODO:Añadir con traductor mensajes en diferentes idiomas.
        private string ValidarSerieRCE(SerieRCE serieRCE)
        {

            if (serieRCE.Lista_Escenarios_Predefinidos == null || serieRCE.Lista_Escenarios_Predefinidos.Count == 0)
                return _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorSinEscenarioPredefinido");

            if (serieRCE.TipologiaCE.Nombre.Last()=='A')
            {
                if (serieRCE.Lista_Escenarios_Seleccionados.Count == 0)
                    return _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorSinEscenarioSeleccionado"); 
            }
            if (serieRCE.Aportaciones_R_NAT.Count == 0)
                return _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRegimenNaturalSinAportaciones");

            if (serieRCE.Mes_Inicio < 1 || serieRCE.Mes_Inicio > 12)
                return _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorMesInicioFueraRango") + " " +
                        _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, serieRCE.Mes_Inicio.ToString());

            return "";
        }

        private void ManualesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sInfo = new ProcessStartInfo("http://ambiental.cedex.es/caudales-ambientales.php");
            Process.Start(sInfo);
        }
        
        private void cmbMesInicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                //comprobar si hay punto seleccionado.
                if (_PtoSeleccionado == null)
                    return;
               

                // 1) Actualizar el mes de inicio del punto
                _cMDB.EjecutarSQL("UPDATE [Punto] SET mesInicio = " + (cmbMesInicio.SelectedIndex + 1).ToString() + " WHERE Id_punto =" + _PtoSeleccionado.Id);

                // 2) Actualizar las fechas de inicio y fin de las listas
                DataTable dtaux = _cMDB.GetTablaSQL("SELECT ID_punto, mesInicio FROM [Punto] WHERE Id_punto =" + _PtoSeleccionado.Id);

                DataTable dtauxLista;
                int idpunto = (int)(dtaux.Rows[0]["ID_punto"]);
                int mesInicio = int.Parse(dtaux.Rows[0]["mesInicio"].ToString());
                dtaux = _cMDB.GetTablaSQL("SELECT * FROM [Lista] WHERE ID_Punto = " + idpunto);
                DataRow dr;

                for (int i = 0; i <= dtaux.Rows.Count - 1; i++)
                {
                    dr = dtaux.Rows[i];
                    int idLista = Conversions.ToInteger(dr["ID_Lista"]);
                    dtauxLista = _cMDB.GetTablaSQL("SELECT TOP 1 Fecha FROM [Valor] WHERE ID_Lista = " + idLista + " ORDER BY Fecha DESC");
                    var fechaFIN = DateTime.Parse(Conversions.ToString(dtauxLista.Rows[0][0]));
                    dtauxLista = _cMDB.GetTablaSQL("SELECT TOP 1 Fecha FROM [Valor] WHERE ID_Lista = " + idLista + " ORDER BY Fecha ASC");
                    var fechaINI = DateTime.Parse(dtauxLista.Rows[0][0].ToString());

                    if (fechaINI.Month != mesInicio || fechaINI.Day != 1)
                    {
                        if (fechaINI.Month < mesInicio)                        
                            fechaINI = new DateTime(fechaINI.Year - 1, mesInicio, 1);                        
                        else                        
                            fechaINI = new DateTime(fechaINI.Year, mesInicio, 1);                        
                    }

                    int mesFin = mesInicio - 1 <= 0 ? 12 : mesInicio - 1;

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
                    }

                    _cMDB.EjecutarSQL("UPDATE [Lista] SET Fecha_Ini = #" + fechaINI.ToString("yyyy-MM-dd") + "#, Fecha_Fin = #" + fechaFIN.ToString("yyyy-MM-dd") + "# WHERE ID_Lista =" + idLista);
                }

                // 3) Refrescar la XPTable
                lstboxPuntos_SelectedIndexChanged(null, null);
            if (rbCaudalesEco.Checked) InicializarMesesEscenarios();
           
        }

        #endregion

        private void cargaMasivaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCerrarPorCargaMasiva"), "Información", 
                                                     MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK) 
            {
                //var process = Process.Start("CargaPorLotes.exe");
                var process = Process.Start("CargaMasiva.exe");
                Environment.Exit(0);  //Cerramos IAHRIS
                process.WaitForExit();
                ActualizarCombos();
            }

        }

        private void importarDatosSIMPAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetearFormPrincipal();
            var fdatossimpa = new DatosSimpa.FormGMaps();
            fdatossimpa.ShowDialog();
            ActualizarCombos();
        }

        private void InicializarMesesEscenarios()
        {
            //int mes = cmbMesInicio.SelectedIndex;

            //dGrid_Escenarios.Columns[0].Name = "Seleccionar";
            //dGrid_Escenarios.Columns[0].HeaderText = "Seleccionar";
            dGrid_Escenarios.Columns[0].Frozen = true;
            dGrid_Escenarios.Columns[0].Width = 75;

            //dGrid_Escenarios.Columns[1].Name = "Escenario";
            //dGrid_Escenarios.Columns[1].HeaderText = "Escenario";
            dGrid_Escenarios.Columns[1].Frozen = true;
            dGrid_Escenarios.Columns[1].Width = 75;


            //for (int i = 0; i < 12; i++)
            //{
            //    //dGrid_Escenarios.Columns[i + 2].Name = Utiles.ObtenerMes(mes+1);
            //    dGrid_Escenarios.Columns[i + 2].HeaderText = Utiles.ObtenerMes(mes+1);
            //    dGrid_Escenarios.Columns[i + 2].Width = 60;
            //    mes++;
            //    if (mes == 12) mes = 0;
            //}
        }

        private void rbIndices_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIndices.Checked)
            {
                tabpEscenarios.Parent = null;
                tabpValoraciones.Parent = null;
                gbCoetane.Visible = true;
                RellenarListaInformes(rbCaudalesEco.Checked);
            }
            else
            {
                tabpEscenarios.Parent = tabControl1;
                tabpValoraciones.Parent = tabControl1;
                gbCoetane.Visible = false;
            }
        }

        private void rbCaudalesEco_CheckedChanged(object sender, EventArgs e)
        {
            if (_PtoSeleccionado == null)
                return;

            if (!rbCaudalesEco.Checked) return;

            //int idLista = _simulacion.idListas.First(x => x > 0);

            //List<EscenarioDTO> listaEscenarios;
            //if (_AltSeleccionada != null)
            //{
            //    listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, _AltSeleccionada.Id);
            //}
            //else
            //{
            //    listaEscenarios = _escenarios.GetEscenarioPorIDPunto(_PtoSeleccionado.Id, 0);
            //}


            //_serieRCE = SerieRCE.ObtenerSerieRCEPorSimulacion(_simulacion, listaEscenarios);
 
            ////al ser cambio de opción, no habrá escenarios seleccionados.
            //TipologiaCE tipologia = TipologiaCE.GetTipologia(_serieRCE.Aportaciones_R_NAT.ToArray(), _simulacion, true, 0);

            InicializarTabEscenarios();
            _traductor.TraducirForm(this);
        }

        private void añadirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void dGrid_Escenarios_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            
            
            int checkedCount = 0;
            

                // Cuenta cuántos checkboxes están marcados
                foreach (DataGridViewRow row in dGrid_Escenarios.Rows)
                {
                    try 
                    {
                    //if (Convert.ToBoolean(row.Cells[e.ColumnIndex].FormattedValue))
                    if (Convert.ToBoolean(row.Cells[0].FormattedValue))
                    {
                            checkedCount++;
                        }
                    }catch(Exception ex)
                    {

                    }
                }
            


                // Si se intenta marcar más de 3 checkboxes, revierte el cambio
                if (checkedCount > 3)
                {
                    foreach (DataGridViewRow row in dGrid_Escenarios.Rows)                    
                        dGrid_Escenarios.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorMasde2Escenarios"));
                }
                
        }
        private void dGrid_Escenarios_CellClick(object sender, DataGridViewCellEventArgs e) 
        {
            if (e.ColumnIndex == -1|e.RowIndex==-1) return;
            // Obtiene la celda en la que se hizo clic
            DataGridViewCell cell = dGrid_Escenarios.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //Evito poder modificar la celda del escenario R_NORM
            if(dGrid_Escenarios.Rows[e.RowIndex].Cells["Escenario"].Value.ToString() != "R_NORM") 
            { 
                // Verifica si la celda es del tipo de columna DataGridViewCheckBoxColumn
                if (cell.GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    // Obtiene el valor actual de la celda
                    bool currentValue = (bool)cell.FormattedValue;

                    // Cambia el valor de la celda
                    cell.Value = !currentValue;
                }
            }

            int idpunt = _PtoSeleccionado.Id;
            int idalte;
            if (_AltSeleccionada != null)
            {
                idalte = _AltSeleccionada.Id;
            }
            else
            {
                idalte = 0;
            }



            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
            List<EscenarioDTO> listaEscenarios = _escenarios.GetEscenarioPorIDPunto(idpunt, idalte);

            foreach (DataGridViewRow row in dGrid_Escenarios.Rows)
            {
                // Verifica si la celda del checkbox está seleccionada
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["Seleccionar"] as DataGridViewCheckBoxCell;
                if (checkBoxCell != null && Convert.ToBoolean(checkBoxCell.FormattedValue) == true)
                {
                    // Agrega la fila a la lista de filas seleccionadas
                    selectedRows.Add(row);
                }
            }

            List<EscenarioDTO> escenariosSeleccionados = new List<EscenarioDTO>();
            List<int> idSeleccionados = selectedRows.Select(row => Convert.ToInt32(row.Cells["IdEscenario"].Value)).ToList();

            //TODO 1: Revisar porque no carga correctamente los IDs de escenarios seleccionados Tiene que ver con como se hace la carga. Revisar todo el proceso. Pasa a veces, y a veces, no. 

            //if (idSeleccionados.Count > 0)
            //    if (idSeleccionados[0] == 0)
            //    { int f = 0; }

            foreach (EscenarioDTO escenario in listaEscenarios)
            {
                if (idSeleccionados.Contains(escenario.Id_Escenario))
                {
                    escenario.Puntuacion = _escenarios.CalcularPuntuacion(escenario, _serieRCE.Mes_Inicio, _serieRCE.Caudales_R_NAT.ToArray());
                    escenariosSeleccionados.Add(escenario);
                }
            }

            bool contieneEscenario = listaEscenarios.Any(escenario =>
                escenario.Nombre == "R_NORM");



            TipologiaCE tipologiaCE = TipologiaCE.GetTipologia(_serieRCE.Aportaciones_R_NAT.ToArray(), _simulacion, contieneEscenario, escenariosSeleccionados.Count);
            _serieRCE.TipologiaCE = tipologiaCE;

            bool esTipologiaNONE = tipologiaCE.Nombre == enumTipologias.NONE.ToString();
            string nombreTipologia = tipologiaCE.Nombre.Replace("Tipo", "") + "\n Caudales Ecológicos";
        

            if (!esTipologiaNONE)
            {
                btnCalcular.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCalcular") + " " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strType") + " " + nombreTipologia;
                btnCalcular.Enabled = true;
            }
            else
            {
                btnCalcular.Enabled = false;
                btnCalcular.Text = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCalcular");
            }
            RellenarListaInformes(rbCaudalesEco.Checked);
        }

        private void añadirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            

            int idPunto = 0;
            int idAlteracion = 0;
            if (_PtoSeleccionado != null)
            {
                idPunto = _PtoSeleccionado.Id;
            }
            if (_AltSeleccionada != null)
            {
                idAlteracion = _AltSeleccionada.Id;
            }

            var ds = _cMDB.RellenarDataSet("Escenario", "SELECT * FROM [Escenario] WHERE Id_Punto = " + idPunto + " AND Id_Alteracion = " + idAlteracion + "AND Por_Defecto = False AND Nombre_Escenario <> 'R_NORM'");


            if (ds.Tables[0].Rows .Count > 3)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorMasde4Escenarios"),"Error" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
               



            //Enseño el formulario como un dialog
            var fanadir = new FormAnadirEscenario(_cMDB, _serieRCE, idPunto, idAlteracion);
            fanadir.ShowDialog();

            
            InicializarTabEscenarios();
        }

        private void añadirRNORMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            int idPunto = 0;
            int idAlteracion = 0;
            if (_PtoSeleccionado != null)
            {
                idPunto = _PtoSeleccionado.Id;
            }
            if (_AltSeleccionada != null)
            {
                idAlteracion = _AltSeleccionada.Id;
            }


            EscenarioDTO rnorm = null;

            FormAnadirEscenarioRNORM fanadir=null;
            List<EscenarioDTO> escenarios = _escenarios.GetEscenarioPorIDPunto(idPunto, idAlteracion);

            if (escenarios.Where(x => x.Nombre == "R_NORM").Count() > 0)
            {
                rnorm = escenarios.Where(x => x.Nombre == "R_NORM").First();
                fanadir = new FormAnadirEscenarioRNORM(_cMDB, _serieRCE, idPunto, idAlteracion, rnorm);
            }
            else
            {
                //Enseño el formulario como un dialog
                fanadir = new FormAnadirEscenarioRNORM(_cMDB, _serieRCE, idPunto, idAlteracion);
            }

                fanadir.ShowDialog();
           // _escenarios.EditarEscenarioBD(rnorm);
            
            InicializarTabEscenarios();
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idPunto = 0;
            int idAlteracion = 0;
            if (_PtoSeleccionado != null)
            {
                idPunto = _PtoSeleccionado.Id;
            }
            if (_AltSeleccionada != null)
            {
                idAlteracion = _AltSeleccionada.Id;
            }

            //Enseño el formulario como un dialog
            var ds = _cMDB.RellenarDataSet("Escenario", "SELECT Nombre_Escenario, Id_Escenario FROM [Escenario] WHERE Id_Punto = " + idPunto + " AND Id_Alteracion = " + idAlteracion + "AND Por_Defecto = False");

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorNoEscenarios"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                var feliminar = new FormEliminarEscenario(_cMDB, idPunto, idAlteracion);
                feliminar.ShowDialog();
            }

            InicializarTabEscenarios();
        }

        private void btnResetGraph_Click(object sender, EventArgs e)
        {
            DibujoGraficos.ResetZoomGrafico(plot1);
            
        }

        private void btnMinusX_Click(object sender, EventArgs e)
        {
            
            DibujoGraficos.ZoomOutX(plot1);
           
        }

        private void btnMoreX_Click(object sender, EventArgs e)
        {
            
            DibujoGraficos.ZoomInX(plot1);
            
        }

        private void btnMinusY_Click(object sender, EventArgs e)
        {
            
            DibujoGraficos.ZoomOutY(plot1);
            
        }

        private void btnMoreY_Click(object sender, EventArgs e)
        {      
            DibujoGraficos.ZoomInY(plot1);
             
        } 

        private void btnUp_Click(object sender, EventArgs e)
        {
            DibujoGraficos.MoveViewUp(plot1);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            DibujoGraficos.MoveViewDown(plot1);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            DibujoGraficos.MoveViewLeft(plot1);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            DibujoGraficos.MoveViewRight(plot1);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            DibujoGraficos.ZoomInX(plot1);
            DibujoGraficos.ZoomInY(plot1);
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            DibujoGraficos.ZoomOutX(plot1);
            DibujoGraficos.ZoomOutY(plot1);
        }

        private void editarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int idPunto = 0;
            int idAlteracion = 0;
            if (_PtoSeleccionado != null)
            {
                idPunto = _PtoSeleccionado.Id;
            }
            if (_AltSeleccionada != null)
            {
                idAlteracion = _AltSeleccionada.Id;
            }

            //Enseño el formulario como un dialog
            var ds = _cMDB.RellenarDataSet("Escenario", "SELECT Nombre_Escenario, Id_Escenario FROM [Escenario] WHERE Id_Punto = " + idPunto + " AND Id_Alteracion = " + idAlteracion + "AND Por_Defecto = False");

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorNoEscenarios"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                var feditar = new FormEditarEscenario(_cMDB, _serieRCE, idPunto, idAlteracion);
                feditar.ShowDialog();
            }




            InicializarTabEscenarios();
        }

        private void editarRNORMToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int idPunto = 0;
            int idAlteracion = 0;
            if (_PtoSeleccionado != null)
            {
                idPunto = _PtoSeleccionado.Id;
            }
            if (_AltSeleccionada != null)
            {
                idAlteracion = _AltSeleccionada.Id;
            }


            EscenarioDTO rnorm = null;

            FormAnadirEscenarioRNORM fanadir = null;
            List<EscenarioDTO> escenarios = _escenarios.GetEscenarioPorIDPunto(idPunto, idAlteracion);

            if (escenarios.Where(x => x.Nombre == "R_NORM").Count() > 0)
            {
                rnorm = escenarios.Where(x => x.Nombre == "R_NORM").First();
                fanadir = new FormAnadirEscenarioRNORM(_cMDB, _serieRCE, idPunto, idAlteracion, rnorm);
            }
            else
            {
                //Enseño el formulario como un dialog
                fanadir = new FormAnadirEscenarioRNORM(_cMDB, _serieRCE, idPunto, idAlteracion);
            }

            fanadir.ShowDialog();
            // _escenarios.EditarEscenarioBD(rnorm);

            InicializarTabEscenarios();
        }
    }
}