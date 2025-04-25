using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    [DesignerGenerated()]
    public partial class FormInicial : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            XPTable.Models.DataSourceColumnBinder dataSourceColumnBinder1 = new XPTable.Models.DataSourceColumnBinder();
            XPTable.Renderers.DragDropRenderer dragDropRenderer1 = new XPTable.Renderers.DragDropRenderer();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInicial));
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.GestiónDeProyectosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditarProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestiónDePuntosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirPuntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EditarPuntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarPuntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestiónDeAlteracionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirAlteraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EditarAlteraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarAlteraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestiónDeListasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirListaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarListaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestBBDDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ImportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ExportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilidadesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargaMasivaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarDatosSIMPAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ManualesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IdiomasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._lstboxPuntos = new System.Windows.Forms.ListBox();
            this.gbSeriesValoresAlt = new System.Windows.Forms.GroupBox();
            this.lblASerieDiaria = new System.Windows.Forms.Label();
            this.lblASerieMensual = new System.Windows.Forms.Label();
            this.lblIDDiaria = new System.Windows.Forms.Label();
            this.lblIDMensual = new System.Windows.Forms.Label();
            this.gbSeriesValoresNat = new System.Windows.Forms.GroupBox();
            this.lblDiario = new System.Windows.Forms.Label();
            this.lblMensual = new System.Windows.Forms.Label();
            this.lblSerieNatDiaria = new System.Windows.Forms.Label();
            this.lblSerieNatMensual = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabpSeries = new System.Windows.Forms.TabPage();
            this.gbCoetane = new System.Windows.Forms.GroupBox();
            this._chkboxUsarCoe = new System.Windows.Forms.CheckBox();
            this._chkboxUsarCoeDiaria = new System.Windows.Forms.CheckBox();
            this.grpboxLeyenda = new System.Windows.Forms.GroupBox();
            this.lblLeyenda2 = new System.Windows.Forms.Label();
            this.lblLeyenda6 = new System.Windows.Forms.Label();
            this.lblLeyenda5 = new System.Windows.Forms.Label();
            this.lblLeyenda4 = new System.Windows.Forms.Label();
            this.lblLeyenda3 = new System.Windows.Forms.Label();
            this.lblLeyenda7 = new System.Windows.Forms.Label();
            this.lblLeyenda1 = new System.Windows.Forms.Label();
            this.lblDatosMensuales = new System.Windows.Forms.Label();
            this.lblDatosDiarios = new System.Windows.Forms.Label();
            this.XPTablaListas = new XPTable.Models.Table();
            this.ColumnModel1 = new XPTable.Models.ColumnModel();
            this.TableModel1 = new XPTable.Models.TableModel();
            this.lblTotalAños = new System.Windows.Forms.Label();
            this.lblAñosAltDiarioUSO = new System.Windows.Forms.Label();
            this.lblAñosHidro = new System.Windows.Forms.Label();
            this.lblAñosNatDiarioUSO = new System.Windows.Forms.Label();
            this.lblAñosNatDiario = new System.Windows.Forms.Label();
            this.lblAñosAltMensualUSO = new System.Windows.Forms.Label();
            this.lblAñosAltDiario = new System.Windows.Forms.Label();
            this.lblAñosNatMensualUSO = new System.Windows.Forms.Label();
            this.lblAñosAltMensual = new System.Windows.Forms.Label();
            this.lblAñosCoeDiaria = new System.Windows.Forms.Label();
            this.lblAñosCoeMensual = new System.Windows.Forms.Label();
            this.lblAñosNatMensual = new System.Windows.Forms.Label();
            this.tabpEscenarios = new System.Windows.Forms.TabPage();
            this.dGrid_Escenarios = new System.Windows.Forms.DataGridView();
            this.Seleccionar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IdEscenario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Escenario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mes12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Puntuacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Demanda_Ambiental = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Eficiencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.añadirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.añadirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.añadirRNORMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editarRNORMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabpValoraciones = new System.Windows.Forms.TabPage();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnMoreX = new System.Windows.Forms.Button();
            this.btnMinusX = new System.Windows.Forms.Button();
            this.btnMoreY = new System.Windows.Forms.Button();
            this.btnMinusY = new System.Windows.Forms.Button();
            this.btnResetGraph = new System.Windows.Forms.Button();
            this.plot1 = new OxyPlot.WindowsForms.PlotView();
            this.gbAlteraciones = new System.Windows.Forms.GroupBox();
            this.lblNombreAlteraciones = new System.Windows.Forms.Label();
            this.lblNombreAlteracion = new System.Windows.Forms.Label();
            this.lblAltNombreEstatico = new System.Windows.Forms.Label();
            this.lblNombreRegAlterados = new System.Windows.Forms.Label();
            this.lblCodigoAlt = new System.Windows.Forms.Label();
            this.lblNombreRegAlterado = new System.Windows.Forms.Label();
            this.lblAbrevRegAlterado = new System.Windows.Forms.Label();
            this.lblAbrevRegAlterados = new System.Windows.Forms.Label();
            this.lblSelectorAlteracion = new System.Windows.Forms.Label();
            this._cmbListaAlteradasDiarias = new System.Windows.Forms.ComboBox();
            this._btnCalcular = new System.Windows.Forms.Button();
            this.gbInforme = new System.Windows.Forms.GroupBox();
            this.lstBoxInformes = new System.Windows.Forms.ListBox();
            this.gbProyecto = new System.Windows.Forms.GroupBox();
            this.lblNombreProyectos = new System.Windows.Forms.Label();
            this.lblNombreProyecto = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblProyectoDesc = new System.Windows.Forms.Label();
            this._cbProyectos = new System.Windows.Forms.ComboBox();
            this.lblPuntoNombre = new System.Windows.Forms.Label();
            this.lblPuntoClave = new System.Windows.Forms.Label();
            this.lblPuntoNListas = new System.Windows.Forms.Label();
            this.lblPuntosClave = new System.Windows.Forms.Label();
            this.lblPuntosNombre = new System.Windows.Forms.Label();
            this.lblPuntosNListas = new System.Windows.Forms.Label();
            this.lblAbrevRegNaturales = new System.Windows.Forms.Label();
            this.lblAbrevRegNatural = new System.Windows.Forms.Label();
            this.lblMesInicio = new System.Windows.Forms.Label();
            this._cmbMesInicio = new System.Windows.Forms.ComboBox();
            this.lblNombreRegNatural = new System.Windows.Forms.Label();
            this.lblNombreRegNaturales = new System.Windows.Forms.Label();
            this.gbPuntos = new System.Windows.Forms.GroupBox();
            this.lblSelectorPunto = new System.Windows.Forms.Label();
            this.lblSelectorProyecto = new System.Windows.Forms.Label();
            this.tabcDatos = new System.Windows.Forms.TabControl();
            this.tab_Selectores = new System.Windows.Forms.TabPage();
            this.rbCaudalesEco = new System.Windows.Forms.RadioButton();
            this.rbIndices = new System.Windows.Forms.RadioButton();
            this.tab_Datos = new System.Windows.Forms.TabPage();
            this.MenuStrip1.SuspendLayout();
            this.gbSeriesValoresAlt.SuspendLayout();
            this.gbSeriesValoresNat.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabpSeries.SuspendLayout();
            this.gbCoetane.SuspendLayout();
            this.grpboxLeyenda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XPTablaListas)).BeginInit();
            this.tabpEscenarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGrid_Escenarios)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.tabpValoraciones.SuspendLayout();
            this.gbAlteraciones.SuspendLayout();
            this.gbInforme.SuspendLayout();
            this.gbProyecto.SuspendLayout();
            this.gbPuntos.SuspendLayout();
            this.tabcDatos.SuspendLayout();
            this.tab_Selectores.SuspendLayout();
            this.tab_Datos.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GestiónDeProyectosToolStripMenuItem,
            this.GestiónDePuntosToolStripMenuItem,
            this.GestiónDeAlteracionesToolStripMenuItem,
            this.GestiónDeListasToolStripMenuItem,
            this.GestBBDDToolStripMenuItem,
            this.utilidadesToolStripMenuItem,
            this._ManualesToolStripMenuItem,
            this.IdiomasToolStripMenuItem});
            this.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.MenuStrip1.Size = new System.Drawing.Size(1260, 23);
            this.MenuStrip1.TabIndex = 0;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // GestiónDeProyectosToolStripMenuItem
            // 
            this.GestiónDeProyectosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._AñadirProyectoToolStripMenuItem,
            this.EditarProyectoToolStripMenuItem,
            this._EliminarProyectoToolStripMenuItem});
            this.GestiónDeProyectosToolStripMenuItem.Name = "GestiónDeProyectosToolStripMenuItem";
            this.GestiónDeProyectosToolStripMenuItem.Size = new System.Drawing.Size(130, 19);
            this.GestiónDeProyectosToolStripMenuItem.Text = "Gestión de proyectos";
            // 
            // _AñadirProyectoToolStripMenuItem
            // 
            this._AñadirProyectoToolStripMenuItem.Name = "_AñadirProyectoToolStripMenuItem";
            this._AñadirProyectoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._AñadirProyectoToolStripMenuItem.Text = "Añadir proyecto";
            this._AñadirProyectoToolStripMenuItem.Click += new System.EventHandler(this.AñadirProyectoToolStripMenuItem_Click);
            // 
            // EditarProyectoToolStripMenuItem
            // 
            this.EditarProyectoToolStripMenuItem.Name = "EditarProyectoToolStripMenuItem";
            this.EditarProyectoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.EditarProyectoToolStripMenuItem.Text = "Editar Proyecto";
            this.EditarProyectoToolStripMenuItem.Click += new System.EventHandler(this.EditarProyectoToolStripMenuItem_Click);
            // 
            // _EliminarProyectoToolStripMenuItem
            // 
            this._EliminarProyectoToolStripMenuItem.Name = "_EliminarProyectoToolStripMenuItem";
            this._EliminarProyectoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._EliminarProyectoToolStripMenuItem.Text = "Eliminar proyecto";
            this._EliminarProyectoToolStripMenuItem.Click += new System.EventHandler(this.EliminarProyectoToolStripMenuItem_Click);
            // 
            // GestiónDePuntosToolStripMenuItem
            // 
            this.GestiónDePuntosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._AñadirPuntoToolStripMenuItem,
            this._EditarPuntoToolStripMenuItem,
            this._EliminarPuntoToolStripMenuItem});
            this.GestiónDePuntosToolStripMenuItem.Name = "GestiónDePuntosToolStripMenuItem";
            this.GestiónDePuntosToolStripMenuItem.Size = new System.Drawing.Size(115, 19);
            this.GestiónDePuntosToolStripMenuItem.Text = "Gestión de Puntos";
            // 
            // _AñadirPuntoToolStripMenuItem
            // 
            this._AñadirPuntoToolStripMenuItem.Name = "_AñadirPuntoToolStripMenuItem";
            this._AñadirPuntoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this._AñadirPuntoToolStripMenuItem.Text = "Añadir Punto";
            this._AñadirPuntoToolStripMenuItem.Click += new System.EventHandler(this.AñadirPuntoToolStripMenuItem_Click);
            // 
            // _EditarPuntoToolStripMenuItem
            // 
            this._EditarPuntoToolStripMenuItem.Name = "_EditarPuntoToolStripMenuItem";
            this._EditarPuntoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this._EditarPuntoToolStripMenuItem.Text = "Editar Punto";
            this._EditarPuntoToolStripMenuItem.Click += new System.EventHandler(this._EditarPuntoToolStripMenuItem_Click);
            // 
            // _EliminarPuntoToolStripMenuItem
            // 
            this._EliminarPuntoToolStripMenuItem.Name = "_EliminarPuntoToolStripMenuItem";
            this._EliminarPuntoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this._EliminarPuntoToolStripMenuItem.Text = "Eliminar Punto";
            this._EliminarPuntoToolStripMenuItem.Click += new System.EventHandler(this.EliminarPuntoToolStripMenuItem_Click);
            // 
            // GestiónDeAlteracionesToolStripMenuItem
            // 
            this.GestiónDeAlteracionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._AñadirAlteraciónToolStripMenuItem,
            this._EditarAlteraciónToolStripMenuItem,
            this._EliminarAlteraciónToolStripMenuItem});
            this.GestiónDeAlteracionesToolStripMenuItem.Name = "GestiónDeAlteracionesToolStripMenuItem";
            this.GestiónDeAlteracionesToolStripMenuItem.Size = new System.Drawing.Size(143, 19);
            this.GestiónDeAlteracionesToolStripMenuItem.Text = "Gestión de Alteraciones";
            // 
            // _AñadirAlteraciónToolStripMenuItem
            // 
            this._AñadirAlteraciónToolStripMenuItem.Name = "_AñadirAlteraciónToolStripMenuItem";
            this._AñadirAlteraciónToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this._AñadirAlteraciónToolStripMenuItem.Text = "Añadir Alteración";
            this._AñadirAlteraciónToolStripMenuItem.Click += new System.EventHandler(this.AñadirAlteraciónToolStripMenuItem_Click);
            // 
            // _EditarAlteraciónToolStripMenuItem
            // 
            this._EditarAlteraciónToolStripMenuItem.Name = "_EditarAlteraciónToolStripMenuItem";
            this._EditarAlteraciónToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this._EditarAlteraciónToolStripMenuItem.Text = "Editar Alteración";
            this._EditarAlteraciónToolStripMenuItem.Click += new System.EventHandler(this._EditarAlteraciónToolStripMenuItem_Click);
            // 
            // _EliminarAlteraciónToolStripMenuItem
            // 
            this._EliminarAlteraciónToolStripMenuItem.Name = "_EliminarAlteraciónToolStripMenuItem";
            this._EliminarAlteraciónToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this._EliminarAlteraciónToolStripMenuItem.Text = "Eliminar Alteración";
            this._EliminarAlteraciónToolStripMenuItem.Click += new System.EventHandler(this.EliminarAlteraciónToolStripMenuItem_Click);
            // 
            // GestiónDeListasToolStripMenuItem
            // 
            this.GestiónDeListasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._AñadirListaToolStripMenuItem,
            this._EliminarListaToolStripMenuItem});
            this.GestiónDeListasToolStripMenuItem.Name = "GestiónDeListasToolStripMenuItem";
            this.GestiónDeListasToolStripMenuItem.Size = new System.Drawing.Size(108, 19);
            this.GestiónDeListasToolStripMenuItem.Text = "Gestión de Series";
            // 
            // _AñadirListaToolStripMenuItem
            // 
            this._AñadirListaToolStripMenuItem.Name = "_AñadirListaToolStripMenuItem";
            this._AñadirListaToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this._AñadirListaToolStripMenuItem.Text = "Añadir Serie";
            this._AñadirListaToolStripMenuItem.Click += new System.EventHandler(this.AñadirListaToolStripMenuItem_Click);
            // 
            // _EliminarListaToolStripMenuItem
            // 
            this._EliminarListaToolStripMenuItem.Name = "_EliminarListaToolStripMenuItem";
            this._EliminarListaToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this._EliminarListaToolStripMenuItem.Text = "Eliminar Serie";
            this._EliminarListaToolStripMenuItem.Click += new System.EventHandler(this.EliminarListaToolStripMenuItem_Click);
            // 
            // GestBBDDToolStripMenuItem
            // 
            this.GestBBDDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._ImportarToolStripMenuItem,
            this._ExportarToolStripMenuItem});
            this.GestBBDDToolStripMenuItem.Name = "GestBBDDToolStripMenuItem";
            this.GestBBDDToolStripMenuItem.Size = new System.Drawing.Size(134, 19);
            this.GestBBDDToolStripMenuItem.Text = "Gestión base de datos";
            // 
            // _ImportarToolStripMenuItem
            // 
            this._ImportarToolStripMenuItem.Name = "_ImportarToolStripMenuItem";
            this._ImportarToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this._ImportarToolStripMenuItem.Text = "Importar";
            this._ImportarToolStripMenuItem.Click += new System.EventHandler(this.ImportarToolStripMenuItem_Click);
            // 
            // _ExportarToolStripMenuItem
            // 
            this._ExportarToolStripMenuItem.Name = "_ExportarToolStripMenuItem";
            this._ExportarToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this._ExportarToolStripMenuItem.Text = "Exportar";
            this._ExportarToolStripMenuItem.Click += new System.EventHandler(this.ExportarToolStripMenuItem_Click);
            // 
            // utilidadesToolStripMenuItem
            // 
            this.utilidadesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cargaMasivaToolStripMenuItem,
            this.importarDatosSIMPAToolStripMenuItem});
            this.utilidadesToolStripMenuItem.Name = "utilidadesToolStripMenuItem";
            this.utilidadesToolStripMenuItem.Size = new System.Drawing.Size(71, 19);
            this.utilidadesToolStripMenuItem.Text = "Utilidades";
            // 
            // cargaMasivaToolStripMenuItem
            // 
            this.cargaMasivaToolStripMenuItem.Name = "cargaMasivaToolStripMenuItem";
            this.cargaMasivaToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.cargaMasivaToolStripMenuItem.Text = "Carga Masiva";
            this.cargaMasivaToolStripMenuItem.Click += new System.EventHandler(this.cargaMasivaToolStripMenuItem_Click);
            // 
            // importarDatosSIMPAToolStripMenuItem
            // 
            this.importarDatosSIMPAToolStripMenuItem.Name = "importarDatosSIMPAToolStripMenuItem";
            this.importarDatosSIMPAToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.importarDatosSIMPAToolStripMenuItem.Text = "Importar datos SIMPA";
            this.importarDatosSIMPAToolStripMenuItem.Click += new System.EventHandler(this.importarDatosSIMPAToolStripMenuItem_Click);
            // 
            // _ManualesToolStripMenuItem
            // 
            this._ManualesToolStripMenuItem.Name = "_ManualesToolStripMenuItem";
            this._ManualesToolStripMenuItem.Size = new System.Drawing.Size(70, 19);
            this._ManualesToolStripMenuItem.Text = "Manuales";
            this._ManualesToolStripMenuItem.Visible = false;
            this._ManualesToolStripMenuItem.Click += new System.EventHandler(this.ManualesToolStripMenuItem_Click);
            // 
            // IdiomasToolStripMenuItem
            // 
            this.IdiomasToolStripMenuItem.Enabled = false;
            this.IdiomasToolStripMenuItem.Name = "IdiomasToolStripMenuItem";
            this.IdiomasToolStripMenuItem.Size = new System.Drawing.Size(113, 19);
            this.IdiomasToolStripMenuItem.Text = "Idioma/Language";
            // 
            // _lstboxPuntos
            // 
            this._lstboxPuntos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lstboxPuntos.FormattingEnabled = true;
            this._lstboxPuntos.Location = new System.Drawing.Point(8, 79);
            this._lstboxPuntos.Name = "_lstboxPuntos";
            this._lstboxPuntos.ScrollAlwaysVisible = true;
            this._lstboxPuntos.Size = new System.Drawing.Size(180, 184);
            this._lstboxPuntos.Sorted = true;
            this._lstboxPuntos.TabIndex = 1;
            this._lstboxPuntos.TabStop = false;
            this._lstboxPuntos.SelectedIndexChanged += new System.EventHandler(this.lstboxPuntos_SelectedIndexChanged);
            // 
            // gbSeriesValoresAlt
            // 
            this.gbSeriesValoresAlt.Controls.Add(this.lblASerieDiaria);
            this.gbSeriesValoresAlt.Controls.Add(this.lblASerieMensual);
            this.gbSeriesValoresAlt.Controls.Add(this.lblIDDiaria);
            this.gbSeriesValoresAlt.Controls.Add(this.lblIDMensual);
            this.gbSeriesValoresAlt.Location = new System.Drawing.Point(6, 89);
            this.gbSeriesValoresAlt.Margin = new System.Windows.Forms.Padding(2);
            this.gbSeriesValoresAlt.Name = "gbSeriesValoresAlt";
            this.gbSeriesValoresAlt.Padding = new System.Windows.Forms.Padding(2);
            this.gbSeriesValoresAlt.Size = new System.Drawing.Size(135, 60);
            this.gbSeriesValoresAlt.TabIndex = 60;
            this.gbSeriesValoresAlt.TabStop = false;
            this.gbSeriesValoresAlt.Text = "Valores Alterados";
            // 
            // lblASerieDiaria
            // 
            this.lblASerieDiaria.AutoSize = true;
            this.lblASerieDiaria.Location = new System.Drawing.Point(5, 16);
            this.lblASerieDiaria.Name = "lblASerieDiaria";
            this.lblASerieDiaria.Size = new System.Drawing.Size(61, 13);
            this.lblASerieDiaria.TabIndex = 28;
            this.lblASerieDiaria.Text = "Serie Diaria";
            // 
            // lblASerieMensual
            // 
            this.lblASerieMensual.AutoSize = true;
            this.lblASerieMensual.Location = new System.Drawing.Point(5, 35);
            this.lblASerieMensual.Name = "lblASerieMensual";
            this.lblASerieMensual.Size = new System.Drawing.Size(74, 13);
            this.lblASerieMensual.TabIndex = 29;
            this.lblASerieMensual.Text = "Serie Mensual";
            // 
            // lblIDDiaria
            // 
            this.lblIDDiaria.AutoSize = true;
            this.lblIDDiaria.BackColor = System.Drawing.Color.White;
            this.lblIDDiaria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblIDDiaria.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lblIDDiaria.Location = new System.Drawing.Point(79, 16);
            this.lblIDDiaria.Name = "lblIDDiaria";
            this.lblIDDiaria.Size = new System.Drawing.Size(27, 15);
            this.lblIDDiaria.TabIndex = 30;
            this.lblIDDiaria.Text = "---";
            // 
            // lblIDMensual
            // 
            this.lblIDMensual.AutoSize = true;
            this.lblIDMensual.BackColor = System.Drawing.Color.White;
            this.lblIDMensual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblIDMensual.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lblIDMensual.Location = new System.Drawing.Point(79, 35);
            this.lblIDMensual.Name = "lblIDMensual";
            this.lblIDMensual.Size = new System.Drawing.Size(27, 15);
            this.lblIDMensual.TabIndex = 31;
            this.lblIDMensual.Text = "---";
            // 
            // gbSeriesValoresNat
            // 
            this.gbSeriesValoresNat.Controls.Add(this.lblDiario);
            this.gbSeriesValoresNat.Controls.Add(this.lblMensual);
            this.gbSeriesValoresNat.Controls.Add(this.lblSerieNatDiaria);
            this.gbSeriesValoresNat.Controls.Add(this.lblSerieNatMensual);
            this.gbSeriesValoresNat.Location = new System.Drawing.Point(6, 6);
            this.gbSeriesValoresNat.Margin = new System.Windows.Forms.Padding(2);
            this.gbSeriesValoresNat.Name = "gbSeriesValoresNat";
            this.gbSeriesValoresNat.Padding = new System.Windows.Forms.Padding(2);
            this.gbSeriesValoresNat.Size = new System.Drawing.Size(135, 60);
            this.gbSeriesValoresNat.TabIndex = 59;
            this.gbSeriesValoresNat.TabStop = false;
            this.gbSeriesValoresNat.Text = "Valores Naturales";
            // 
            // lblDiario
            // 
            this.lblDiario.AutoSize = true;
            this.lblDiario.Location = new System.Drawing.Point(5, 16);
            this.lblDiario.Name = "lblDiario";
            this.lblDiario.Size = new System.Drawing.Size(61, 13);
            this.lblDiario.TabIndex = 25;
            this.lblDiario.Text = "Serie Diaria";
            // 
            // lblMensual
            // 
            this.lblMensual.AutoSize = true;
            this.lblMensual.Location = new System.Drawing.Point(5, 35);
            this.lblMensual.Name = "lblMensual";
            this.lblMensual.Size = new System.Drawing.Size(74, 13);
            this.lblMensual.TabIndex = 26;
            this.lblMensual.Text = "Serie Mensual";
            // 
            // lblSerieNatDiaria
            // 
            this.lblSerieNatDiaria.AutoSize = true;
            this.lblSerieNatDiaria.BackColor = System.Drawing.Color.White;
            this.lblSerieNatDiaria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSerieNatDiaria.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerieNatDiaria.Location = new System.Drawing.Point(79, 16);
            this.lblSerieNatDiaria.Name = "lblSerieNatDiaria";
            this.lblSerieNatDiaria.Size = new System.Drawing.Size(27, 15);
            this.lblSerieNatDiaria.TabIndex = 34;
            this.lblSerieNatDiaria.Text = "---";
            // 
            // lblSerieNatMensual
            // 
            this.lblSerieNatMensual.AutoSize = true;
            this.lblSerieNatMensual.BackColor = System.Drawing.Color.White;
            this.lblSerieNatMensual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSerieNatMensual.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lblSerieNatMensual.Location = new System.Drawing.Point(79, 35);
            this.lblSerieNatMensual.Name = "lblSerieNatMensual";
            this.lblSerieNatMensual.Size = new System.Drawing.Size(27, 15);
            this.lblSerieNatMensual.TabIndex = 35;
            this.lblSerieNatMensual.Text = "---";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabpSeries);
            this.tabControl1.Controls.Add(this.tabpEscenarios);
            this.tabControl1.Controls.Add(this.tabpValoraciones);
            this.tabControl1.Location = new System.Drawing.Point(226, 26);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(782, 455);
            this.tabControl1.TabIndex = 57;
            // 
            // tabpSeries
            // 
            this.tabpSeries.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabpSeries.Controls.Add(this.gbCoetane);
            this.tabpSeries.Controls.Add(this.gbSeriesValoresAlt);
            this.tabpSeries.Controls.Add(this.grpboxLeyenda);
            this.tabpSeries.Controls.Add(this.gbSeriesValoresNat);
            this.tabpSeries.Controls.Add(this.lblDatosMensuales);
            this.tabpSeries.Controls.Add(this.lblDatosDiarios);
            this.tabpSeries.Controls.Add(this.XPTablaListas);
            this.tabpSeries.Controls.Add(this.lblTotalAños);
            this.tabpSeries.Controls.Add(this.lblAñosAltDiarioUSO);
            this.tabpSeries.Controls.Add(this.lblAñosHidro);
            this.tabpSeries.Controls.Add(this.lblAñosNatDiarioUSO);
            this.tabpSeries.Controls.Add(this.lblAñosNatDiario);
            this.tabpSeries.Controls.Add(this.lblAñosAltMensualUSO);
            this.tabpSeries.Controls.Add(this.lblAñosAltDiario);
            this.tabpSeries.Controls.Add(this.lblAñosNatMensualUSO);
            this.tabpSeries.Controls.Add(this.lblAñosAltMensual);
            this.tabpSeries.Controls.Add(this.lblAñosCoeDiaria);
            this.tabpSeries.Controls.Add(this.lblAñosCoeMensual);
            this.tabpSeries.Controls.Add(this.lblAñosNatMensual);
            this.tabpSeries.Location = new System.Drawing.Point(4, 22);
            this.tabpSeries.Margin = new System.Windows.Forms.Padding(2);
            this.tabpSeries.Name = "tabpSeries";
            this.tabpSeries.Padding = new System.Windows.Forms.Padding(2);
            this.tabpSeries.Size = new System.Drawing.Size(774, 429);
            this.tabpSeries.TabIndex = 0;
            this.tabpSeries.Text = "Series";
            // 
            // gbCoetane
            // 
            this.gbCoetane.Controls.Add(this._chkboxUsarCoe);
            this.gbCoetane.Controls.Add(this._chkboxUsarCoeDiaria);
            this.gbCoetane.Location = new System.Drawing.Point(6, 172);
            this.gbCoetane.Margin = new System.Windows.Forms.Padding(2);
            this.gbCoetane.Name = "gbCoetane";
            this.gbCoetane.Padding = new System.Windows.Forms.Padding(2);
            this.gbCoetane.Size = new System.Drawing.Size(135, 60);
            this.gbCoetane.TabIndex = 61;
            this.gbCoetane.TabStop = false;
            this.gbCoetane.Text = "Coetaneidad";
            // 
            // _chkboxUsarCoe
            // 
            this._chkboxUsarCoe.AutoSize = true;
            this._chkboxUsarCoe.Enabled = false;
            this._chkboxUsarCoe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._chkboxUsarCoe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this._chkboxUsarCoe.Location = new System.Drawing.Point(3, 18);
            this._chkboxUsarCoe.Name = "_chkboxUsarCoe";
            this._chkboxUsarCoe.Size = new System.Drawing.Size(111, 17);
            this._chkboxUsarCoe.TabIndex = 10;
            this._chkboxUsarCoe.Text = "Valores Habituales";
            this._chkboxUsarCoe.UseVisualStyleBackColor = true;
            this._chkboxUsarCoe.CheckedChanged += new System.EventHandler(this.chkboxUsarCoe_CheckedChanged);
            // 
            // _chkboxUsarCoeDiaria
            // 
            this._chkboxUsarCoeDiaria.AutoSize = true;
            this._chkboxUsarCoeDiaria.Enabled = false;
            this._chkboxUsarCoeDiaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._chkboxUsarCoeDiaria.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this._chkboxUsarCoeDiaria.Location = new System.Drawing.Point(3, 35);
            this._chkboxUsarCoeDiaria.Name = "_chkboxUsarCoeDiaria";
            this._chkboxUsarCoeDiaria.Size = new System.Drawing.Size(118, 17);
            this._chkboxUsarCoeDiaria.TabIndex = 37;
            this._chkboxUsarCoeDiaria.Text = "Avenidas y Sequías";
            this._chkboxUsarCoeDiaria.UseVisualStyleBackColor = true;
            this._chkboxUsarCoeDiaria.CheckedChanged += new System.EventHandler(this.chkboxUsarCoeDiaria_CheckedChanged);
            // 
            // grpboxLeyenda
            // 
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda2);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda6);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda5);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda4);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda3);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda7);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda1);
            this.grpboxLeyenda.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.grpboxLeyenda.Location = new System.Drawing.Point(6, 255);
            this.grpboxLeyenda.Name = "grpboxLeyenda";
            this.grpboxLeyenda.Size = new System.Drawing.Size(135, 130);
            this.grpboxLeyenda.TabIndex = 36;
            this.grpboxLeyenda.TabStop = false;
            this.grpboxLeyenda.Text = "Leyenda";
            // 
            // lblLeyenda2
            // 
            this.lblLeyenda2.AutoSize = true;
            this.lblLeyenda2.Location = new System.Drawing.Point(4, 29);
            this.lblLeyenda2.Name = "lblLeyenda2";
            this.lblLeyenda2.Size = new System.Drawing.Size(90, 13);
            this.lblLeyenda2.TabIndex = 3;
            this.lblLeyenda2.Text = "I: Año Incompleto";
            // 
            // lblLeyenda6
            // 
            this.lblLeyenda6.AutoSize = true;
            this.lblLeyenda6.Location = new System.Drawing.Point(4, 93);
            this.lblLeyenda6.Name = "lblLeyenda6";
            this.lblLeyenda6.Size = new System.Drawing.Size(118, 13);
            this.lblLeyenda6.TabIndex = 6;
            this.lblLeyenda6.Text = "NCO: Año no coetáneo";
            // 
            // lblLeyenda5
            // 
            this.lblLeyenda5.AutoSize = true;
            this.lblLeyenda5.Location = new System.Drawing.Point(4, 77);
            this.lblLeyenda5.Name = "lblLeyenda5";
            this.lblLeyenda5.Size = new System.Drawing.Size(95, 13);
            this.lblLeyenda5.TabIndex = 5;
            this.lblLeyenda5.Text = "CO: Año coetáneo";
            // 
            // lblLeyenda4
            // 
            this.lblLeyenda4.AutoSize = true;
            this.lblLeyenda4.Location = new System.Drawing.Point(4, 61);
            this.lblLeyenda4.Name = "lblLeyenda4";
            this.lblLeyenda4.Size = new System.Drawing.Size(92, 13);
            this.lblLeyenda4.TabIndex = 4;
            this.lblLeyenda4.Text = "SD: Año sin datos";
            // 
            // lblLeyenda3
            // 
            this.lblLeyenda3.AutoSize = true;
            this.lblLeyenda3.Location = new System.Drawing.Point(4, 45);
            this.lblLeyenda3.Name = "lblLeyenda3";
            this.lblLeyenda3.Size = new System.Drawing.Size(129, 13);
            this.lblLeyenda3.TabIndex = 2;
            this.lblLeyenda3.Text = "CD: Calculado con diarios";
            // 
            // lblLeyenda7
            // 
            this.lblLeyenda7.AutoSize = true;
            this.lblLeyenda7.Location = new System.Drawing.Point(4, 109);
            this.lblLeyenda7.Name = "lblLeyenda7";
            this.lblLeyenda7.Size = new System.Drawing.Size(110, 13);
            this.lblLeyenda7.TabIndex = 1;
            this.lblLeyenda7.Text = "NS: Sin serie cargada";
            // 
            // lblLeyenda1
            // 
            this.lblLeyenda1.AutoSize = true;
            this.lblLeyenda1.Location = new System.Drawing.Point(4, 13);
            this.lblLeyenda1.Name = "lblLeyenda1";
            this.lblLeyenda1.Size = new System.Drawing.Size(86, 13);
            this.lblLeyenda1.TabIndex = 0;
            this.lblLeyenda1.Text = "C: Año Completo";
            // 
            // lblDatosMensuales
            // 
            this.lblDatosMensuales.AutoSize = true;
            this.lblDatosMensuales.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatosMensuales.Location = new System.Drawing.Point(228, 7);
            this.lblDatosMensuales.Name = "lblDatosMensuales";
            this.lblDatosMensuales.Size = new System.Drawing.Size(238, 13);
            this.lblDatosMensuales.TabIndex = 43;
            this.lblDatosMensuales.Text = "                         Datos mensuales                         ";
            this.lblDatosMensuales.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblDatosDiarios
            // 
            this.lblDatosDiarios.AutoSize = true;
            this.lblDatosDiarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatosDiarios.Location = new System.Drawing.Point(485, 7);
            this.lblDatosDiarios.Name = "lblDatosDiarios";
            this.lblDatosDiarios.Size = new System.Drawing.Size(251, 13);
            this.lblDatosDiarios.TabIndex = 44;
            this.lblDatosDiarios.Text = "                                Datos diarios                             ";
            this.lblDatosDiarios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // XPTablaListas
            // 
            this.XPTablaListas.BorderColor = System.Drawing.Color.Black;
            this.XPTablaListas.ColumnModel = this.ColumnModel1;
            this.XPTablaListas.DataMember = null;
            this.XPTablaListas.DataSourceColumnBinder = dataSourceColumnBinder1;
            dragDropRenderer1.ForeColor = System.Drawing.Color.Red;
            this.XPTablaListas.DragDropRenderer = dragDropRenderer1;
            this.XPTablaListas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XPTablaListas.GridLinesContrainedToData = false;
            this.XPTablaListas.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XPTablaListas.Location = new System.Drawing.Point(151, 23);
            this.XPTablaListas.Name = "XPTablaListas";
            this.XPTablaListas.NoItemsText = "Seleccione un punto de la lista para analizar sus series asociadas.";
            this.XPTablaListas.Size = new System.Drawing.Size(613, 364);
            this.XPTablaListas.TabIndex = 9;
            this.XPTablaListas.TableModel = this.TableModel1;
            this.XPTablaListas.Text = "XPTablaListas";
            this.XPTablaListas.UnfocusedBorderColor = System.Drawing.Color.Black;
            // 
            // ColumnModel1
            // 
            this.ColumnModel1.HeaderHeight = 40;
            // 
            // lblTotalAños
            // 
            this.lblTotalAños.AutoSize = true;
            this.lblTotalAños.Location = new System.Drawing.Point(126, 393);
            this.lblTotalAños.Name = "lblTotalAños";
            this.lblTotalAños.Size = new System.Drawing.Size(45, 13);
            this.lblTotalAños.TabIndex = 17;
            this.lblTotalAños.Text = "TOTAL:";
            // 
            // lblAñosAltDiarioUSO
            // 
            this.lblAñosAltDiarioUSO.AutoSize = true;
            this.lblAñosAltDiarioUSO.Location = new System.Drawing.Point(642, 393);
            this.lblAñosAltDiarioUSO.Name = "lblAñosAltDiarioUSO";
            this.lblAñosAltDiarioUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltDiarioUSO.TabIndex = 42;
            this.lblAñosAltDiarioUSO.Text = "Label3";
            // 
            // lblAñosHidro
            // 
            this.lblAñosHidro.AutoSize = true;
            this.lblAñosHidro.Location = new System.Drawing.Point(185, 393);
            this.lblAñosHidro.Name = "lblAñosHidro";
            this.lblAñosHidro.Size = new System.Drawing.Size(39, 13);
            this.lblAñosHidro.TabIndex = 18;
            this.lblAñosHidro.Text = "Label1";
            // 
            // lblAñosNatDiarioUSO
            // 
            this.lblAñosNatDiarioUSO.AutoSize = true;
            this.lblAñosNatDiarioUSO.Location = new System.Drawing.Point(535, 393);
            this.lblAñosNatDiarioUSO.Name = "lblAñosNatDiarioUSO";
            this.lblAñosNatDiarioUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatDiarioUSO.TabIndex = 41;
            this.lblAñosNatDiarioUSO.Text = "Label3";
            // 
            // lblAñosNatDiario
            // 
            this.lblAñosNatDiario.AutoSize = true;
            this.lblAñosNatDiario.Location = new System.Drawing.Point(484, 393);
            this.lblAñosNatDiario.Name = "lblAñosNatDiario";
            this.lblAñosNatDiario.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatDiario.TabIndex = 19;
            this.lblAñosNatDiario.Text = "Label2";
            // 
            // lblAñosAltMensualUSO
            // 
            this.lblAñosAltMensualUSO.AutoSize = true;
            this.lblAñosAltMensualUSO.Location = new System.Drawing.Point(383, 393);
            this.lblAñosAltMensualUSO.Name = "lblAñosAltMensualUSO";
            this.lblAñosAltMensualUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltMensualUSO.TabIndex = 40;
            this.lblAñosAltMensualUSO.Text = "Label3";
            // 
            // lblAñosAltDiario
            // 
            this.lblAñosAltDiario.AutoSize = true;
            this.lblAñosAltDiario.Location = new System.Drawing.Point(590, 393);
            this.lblAñosAltDiario.Name = "lblAñosAltDiario";
            this.lblAñosAltDiario.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltDiario.TabIndex = 20;
            this.lblAñosAltDiario.Text = "Label3";
            // 
            // lblAñosNatMensualUSO
            // 
            this.lblAñosNatMensualUSO.AutoSize = true;
            this.lblAñosNatMensualUSO.Location = new System.Drawing.Point(274, 393);
            this.lblAñosNatMensualUSO.Name = "lblAñosNatMensualUSO";
            this.lblAñosNatMensualUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatMensualUSO.TabIndex = 39;
            this.lblAñosNatMensualUSO.Text = "Label3";
            // 
            // lblAñosAltMensual
            // 
            this.lblAñosAltMensual.AutoSize = true;
            this.lblAñosAltMensual.Location = new System.Drawing.Point(331, 393);
            this.lblAñosAltMensual.Name = "lblAñosAltMensual";
            this.lblAñosAltMensual.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltMensual.TabIndex = 23;
            this.lblAñosAltMensual.Text = "Label6";
            // 
            // lblAñosCoeDiaria
            // 
            this.lblAñosCoeDiaria.AutoSize = true;
            this.lblAñosCoeDiaria.Location = new System.Drawing.Point(693, 393);
            this.lblAñosCoeDiaria.Name = "lblAñosCoeDiaria";
            this.lblAñosCoeDiaria.Size = new System.Drawing.Size(39, 13);
            this.lblAñosCoeDiaria.TabIndex = 38;
            this.lblAñosCoeDiaria.Text = "Label3";
            // 
            // lblAñosCoeMensual
            // 
            this.lblAñosCoeMensual.AutoSize = true;
            this.lblAñosCoeMensual.Location = new System.Drawing.Point(431, 393);
            this.lblAñosCoeMensual.Name = "lblAñosCoeMensual";
            this.lblAñosCoeMensual.Size = new System.Drawing.Size(39, 13);
            this.lblAñosCoeMensual.TabIndex = 24;
            this.lblAñosCoeMensual.Text = "Label7";
            // 
            // lblAñosNatMensual
            // 
            this.lblAñosNatMensual.AutoSize = true;
            this.lblAñosNatMensual.Location = new System.Drawing.Point(230, 393);
            this.lblAñosNatMensual.Name = "lblAñosNatMensual";
            this.lblAñosNatMensual.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatMensual.TabIndex = 22;
            this.lblAñosNatMensual.Text = "Label5";
            // 
            // tabpEscenarios
            // 
            this.tabpEscenarios.Controls.Add(this.dGrid_Escenarios);
            this.tabpEscenarios.Controls.Add(this.menuStrip2);
            this.tabpEscenarios.Location = new System.Drawing.Point(4, 22);
            this.tabpEscenarios.Margin = new System.Windows.Forms.Padding(2);
            this.tabpEscenarios.Name = "tabpEscenarios";
            this.tabpEscenarios.Padding = new System.Windows.Forms.Padding(2);
            this.tabpEscenarios.Size = new System.Drawing.Size(774, 429);
            this.tabpEscenarios.TabIndex = 1;
            this.tabpEscenarios.Text = "Escenarios";
            this.tabpEscenarios.UseVisualStyleBackColor = true;
            // 
            // dGrid_Escenarios
            // 
            this.dGrid_Escenarios.AllowUserToAddRows = false;
            this.dGrid_Escenarios.AllowUserToDeleteRows = false;
            this.dGrid_Escenarios.AllowUserToResizeColumns = false;
            this.dGrid_Escenarios.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Format = "N3";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGrid_Escenarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGrid_Escenarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGrid_Escenarios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seleccionar,
            this.IdEscenario,
            this.Escenario,
            this.Mes1,
            this.Mes2,
            this.Mes3,
            this.Mes4,
            this.Mes5,
            this.Mes6,
            this.Mes7,
            this.Mes8,
            this.Mes9,
            this.Mes10,
            this.Mes11,
            this.Mes12,
            this.Puntuacion,
            this.Demanda_Ambiental,
            this.Eficiencia});
            this.dGrid_Escenarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGrid_Escenarios.Location = new System.Drawing.Point(2, 26);
            this.dGrid_Escenarios.Name = "dGrid_Escenarios";
            this.dGrid_Escenarios.ReadOnly = true;
            this.dGrid_Escenarios.RowHeadersWidth = 51;
            this.dGrid_Escenarios.Size = new System.Drawing.Size(770, 401);
            this.dGrid_Escenarios.TabIndex = 1;
            // 
            // Seleccionar
            // 
            this.Seleccionar.FalseValue = "";
            this.Seleccionar.Frozen = true;
            this.Seleccionar.HeaderText = "Seleccionar";
            this.Seleccionar.IndeterminateValue = "";
            this.Seleccionar.MinimumWidth = 6;
            this.Seleccionar.Name = "Seleccionar";
            this.Seleccionar.ReadOnly = true;
            this.Seleccionar.TrueValue = "";
            this.Seleccionar.Width = 35;
            // 
            // IdEscenario
            // 
            this.IdEscenario.Frozen = true;
            this.IdEscenario.HeaderText = "IdEscenario";
            this.IdEscenario.MinimumWidth = 6;
            this.IdEscenario.Name = "IdEscenario";
            this.IdEscenario.ReadOnly = true;
            this.IdEscenario.Visible = false;
            this.IdEscenario.Width = 125;
            // 
            // Escenario
            // 
            this.Escenario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Escenario.Frozen = true;
            this.Escenario.HeaderText = "Escenario; QMM (m3/s)";
            this.Escenario.MinimumWidth = 6;
            this.Escenario.Name = "Escenario";
            this.Escenario.ReadOnly = true;
            this.Escenario.Width = 145;
            // 
            // Mes1
            // 
            dataGridViewCellStyle2.Format = "N3";
            this.Mes1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Mes1.HeaderText = "Mes1";
            this.Mes1.MinimumWidth = 6;
            this.Mes1.Name = "Mes1";
            this.Mes1.ReadOnly = true;
            this.Mes1.Width = 75;
            // 
            // Mes2
            // 
            dataGridViewCellStyle3.Format = "N3";
            this.Mes2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Mes2.HeaderText = "Mes2";
            this.Mes2.MinimumWidth = 6;
            this.Mes2.Name = "Mes2";
            this.Mes2.ReadOnly = true;
            this.Mes2.Width = 75;
            // 
            // Mes3
            // 
            dataGridViewCellStyle4.Format = "N3";
            this.Mes3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Mes3.HeaderText = "Mes3";
            this.Mes3.MinimumWidth = 6;
            this.Mes3.Name = "Mes3";
            this.Mes3.ReadOnly = true;
            this.Mes3.Width = 75;
            // 
            // Mes4
            // 
            dataGridViewCellStyle5.Format = "N3";
            this.Mes4.DefaultCellStyle = dataGridViewCellStyle5;
            this.Mes4.HeaderText = "Mes4";
            this.Mes4.MinimumWidth = 6;
            this.Mes4.Name = "Mes4";
            this.Mes4.ReadOnly = true;
            this.Mes4.Width = 75;
            // 
            // Mes5
            // 
            dataGridViewCellStyle6.Format = "N3";
            this.Mes5.DefaultCellStyle = dataGridViewCellStyle6;
            this.Mes5.HeaderText = "Mes5";
            this.Mes5.MinimumWidth = 6;
            this.Mes5.Name = "Mes5";
            this.Mes5.ReadOnly = true;
            this.Mes5.Width = 75;
            // 
            // Mes6
            // 
            dataGridViewCellStyle7.Format = "N3";
            this.Mes6.DefaultCellStyle = dataGridViewCellStyle7;
            this.Mes6.HeaderText = "Mes6";
            this.Mes6.MinimumWidth = 6;
            this.Mes6.Name = "Mes6";
            this.Mes6.ReadOnly = true;
            this.Mes6.Width = 75;
            // 
            // Mes7
            // 
            dataGridViewCellStyle8.Format = "N3";
            this.Mes7.DefaultCellStyle = dataGridViewCellStyle8;
            this.Mes7.HeaderText = "Mes7";
            this.Mes7.MinimumWidth = 6;
            this.Mes7.Name = "Mes7";
            this.Mes7.ReadOnly = true;
            this.Mes7.Width = 75;
            // 
            // Mes8
            // 
            dataGridViewCellStyle9.Format = "N3";
            this.Mes8.DefaultCellStyle = dataGridViewCellStyle9;
            this.Mes8.HeaderText = "Mes8";
            this.Mes8.MinimumWidth = 6;
            this.Mes8.Name = "Mes8";
            this.Mes8.ReadOnly = true;
            this.Mes8.Width = 75;
            // 
            // Mes9
            // 
            dataGridViewCellStyle10.Format = "N3";
            this.Mes9.DefaultCellStyle = dataGridViewCellStyle10;
            this.Mes9.HeaderText = "Mes9";
            this.Mes9.MinimumWidth = 6;
            this.Mes9.Name = "Mes9";
            this.Mes9.ReadOnly = true;
            this.Mes9.Width = 75;
            // 
            // Mes10
            // 
            dataGridViewCellStyle11.Format = "N3";
            this.Mes10.DefaultCellStyle = dataGridViewCellStyle11;
            this.Mes10.HeaderText = "Mes10";
            this.Mes10.MinimumWidth = 6;
            this.Mes10.Name = "Mes10";
            this.Mes10.ReadOnly = true;
            this.Mes10.Width = 75;
            // 
            // Mes11
            // 
            dataGridViewCellStyle12.Format = "N3";
            this.Mes11.DefaultCellStyle = dataGridViewCellStyle12;
            this.Mes11.HeaderText = "Mes11";
            this.Mes11.MinimumWidth = 6;
            this.Mes11.Name = "Mes11";
            this.Mes11.ReadOnly = true;
            this.Mes11.Width = 75;
            // 
            // Mes12
            // 
            dataGridViewCellStyle13.Format = "N3";
            this.Mes12.DefaultCellStyle = dataGridViewCellStyle13;
            this.Mes12.HeaderText = "Mes12";
            this.Mes12.MinimumWidth = 6;
            this.Mes12.Name = "Mes12";
            this.Mes12.ReadOnly = true;
            this.Mes12.Width = 75;
            // 
            // Puntuacion
            // 
            this.Puntuacion.HeaderText = "Puntuación";
            this.Puntuacion.MinimumWidth = 6;
            this.Puntuacion.Name = "Puntuacion";
            this.Puntuacion.ReadOnly = true;
            this.Puntuacion.Width = 125;
            // 
            // Demanda_Ambiental
            // 
            dataGridViewCellStyle14.Format = "N3";
            this.Demanda_Ambiental.DefaultCellStyle = dataGridViewCellStyle14;
            this.Demanda_Ambiental.HeaderText = "Demanda Ambiental";
            this.Demanda_Ambiental.MinimumWidth = 6;
            this.Demanda_Ambiental.Name = "Demanda_Ambiental";
            this.Demanda_Ambiental.ReadOnly = true;
            this.Demanda_Ambiental.Width = 125;
            // 
            // Eficiencia
            // 
            dataGridViewCellStyle15.Format = "N3";
            this.Eficiencia.DefaultCellStyle = dataGridViewCellStyle15;
            this.Eficiencia.HeaderText = "Eficiencia";
            this.Eficiencia.MinimumWidth = 6;
            this.Eficiencia.Name = "Eficiencia";
            this.Eficiencia.ReadOnly = true;
            this.Eficiencia.Width = 125;
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.añadirToolStripMenuItem,
            this.editarToolStripMenuItem,
            this.eliminarToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(2, 2);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip2.Size = new System.Drawing.Size(770, 24);
            this.menuStrip2.TabIndex = 2;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // añadirToolStripMenuItem
            // 
            this.añadirToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.añadirToolStripMenuItem1,
            this.añadirRNORMToolStripMenuItem});
            this.añadirToolStripMenuItem.Name = "añadirToolStripMenuItem";
            this.añadirToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.añadirToolStripMenuItem.Text = "Añadir...";
            this.añadirToolStripMenuItem.Click += new System.EventHandler(this.añadirToolStripMenuItem_Click);
            // 
            // añadirToolStripMenuItem1
            // 
            this.añadirToolStripMenuItem1.Name = "añadirToolStripMenuItem1";
            this.añadirToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.añadirToolStripMenuItem1.Text = "Añadir";
            this.añadirToolStripMenuItem1.Click += new System.EventHandler(this.añadirToolStripMenuItem1_Click);
            // 
            // añadirRNORMToolStripMenuItem
            // 
            this.añadirRNORMToolStripMenuItem.Name = "añadirRNORMToolStripMenuItem";
            this.añadirRNORMToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.añadirRNORMToolStripMenuItem.Text = "Añadir R_NORM";
            this.añadirRNORMToolStripMenuItem.Click += new System.EventHandler(this.añadirRNORMToolStripMenuItem_Click);
            // 
            // editarToolStripMenuItem
            // 
            this.editarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editarToolStripMenuItem1,
            this.editarRNORMToolStripMenuItem});
            this.editarToolStripMenuItem.Name = "editarToolStripMenuItem";
            this.editarToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.editarToolStripMenuItem.Text = "Editar";
            this.editarToolStripMenuItem.Click += new System.EventHandler(this.editarToolStripMenuItem_Click);
            // 
            // editarToolStripMenuItem1
            // 
            this.editarToolStripMenuItem1.Name = "editarToolStripMenuItem1";
            this.editarToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.editarToolStripMenuItem1.Text = "Editar";
            this.editarToolStripMenuItem1.Click += new System.EventHandler(this.editarToolStripMenuItem1_Click);
            // 
            // editarRNORMToolStripMenuItem
            // 
            this.editarRNORMToolStripMenuItem.Name = "editarRNORMToolStripMenuItem";
            this.editarRNORMToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.editarRNORMToolStripMenuItem.Text = "Editar R_NORM";
            this.editarRNORMToolStripMenuItem.Click += new System.EventHandler(this.editarRNORMToolStripMenuItem_Click);
            // 
            // eliminarToolStripMenuItem
            // 
            this.eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            this.eliminarToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.eliminarToolStripMenuItem.Text = "Eliminar";
            this.eliminarToolStripMenuItem.Click += new System.EventHandler(this.eliminarToolStripMenuItem_Click);
            // 
            // tabpValoraciones
            // 
            this.tabpValoraciones.BackColor = System.Drawing.SystemColors.Control;
            this.tabpValoraciones.Controls.Add(this.btnZoomIn);
            this.tabpValoraciones.Controls.Add(this.btnZoomOut);
            this.tabpValoraciones.Controls.Add(this.btnUp);
            this.tabpValoraciones.Controls.Add(this.btnDown);
            this.tabpValoraciones.Controls.Add(this.btnRight);
            this.tabpValoraciones.Controls.Add(this.btnLeft);
            this.tabpValoraciones.Controls.Add(this.btnMoreX);
            this.tabpValoraciones.Controls.Add(this.btnMinusX);
            this.tabpValoraciones.Controls.Add(this.btnMoreY);
            this.tabpValoraciones.Controls.Add(this.btnMinusY);
            this.tabpValoraciones.Controls.Add(this.btnResetGraph);
            this.tabpValoraciones.Controls.Add(this.plot1);
            this.tabpValoraciones.Location = new System.Drawing.Point(4, 22);
            this.tabpValoraciones.Margin = new System.Windows.Forms.Padding(2);
            this.tabpValoraciones.Name = "tabpValoraciones";
            this.tabpValoraciones.Size = new System.Drawing.Size(774, 429);
            this.tabpValoraciones.TabIndex = 2;
            this.tabpValoraciones.Text = "Valoraciones";
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.Location = new System.Drawing.Point(3, 15);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(41, 41);
            this.btnZoomIn.TabIndex = 11;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.Location = new System.Drawing.Point(3, 59);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(41, 41);
            this.btnZoomOut.TabIndex = 10;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnUp
            // 
            this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnUp.Location = new System.Drawing.Point(3, 207);
            this.btnUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(41, 41);
            this.btnUp.TabIndex = 9;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnDown.Location = new System.Drawing.Point(3, 251);
            this.btnDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(41, 41);
            this.btnDown.TabIndex = 8;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnRight
            // 
            this.btnRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnRight.Location = new System.Drawing.Point(179, 383);
            this.btnRight.Margin = new System.Windows.Forms.Padding(2);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(41, 41);
            this.btnRight.TabIndex = 7;
            this.btnRight.Text = "→";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnLeft.Location = new System.Drawing.Point(135, 383);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(2);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(41, 41);
            this.btnLeft.TabIndex = 6;
            this.btnLeft.Text = "←";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnMoreX
            // 
            this.btnMoreX.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoreX.Location = new System.Drawing.Point(91, 383);
            this.btnMoreX.Name = "btnMoreX";
            this.btnMoreX.Size = new System.Drawing.Size(41, 41);
            this.btnMoreX.TabIndex = 5;
            this.btnMoreX.Text = "+";
            this.btnMoreX.UseVisualStyleBackColor = true;
            this.btnMoreX.Click += new System.EventHandler(this.btnMoreX_Click);
            // 
            // btnMinusX
            // 
            this.btnMinusX.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinusX.Location = new System.Drawing.Point(47, 383);
            this.btnMinusX.Name = "btnMinusX";
            this.btnMinusX.Size = new System.Drawing.Size(41, 41);
            this.btnMinusX.TabIndex = 4;
            this.btnMinusX.Text = "-";
            this.btnMinusX.UseVisualStyleBackColor = true;
            this.btnMinusX.Click += new System.EventHandler(this.btnMinusX_Click);
            // 
            // btnMoreY
            // 
            this.btnMoreY.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoreY.Location = new System.Drawing.Point(3, 295);
            this.btnMoreY.Name = "btnMoreY";
            this.btnMoreY.Size = new System.Drawing.Size(41, 41);
            this.btnMoreY.TabIndex = 3;
            this.btnMoreY.Text = "+";
            this.btnMoreY.UseVisualStyleBackColor = true;
            this.btnMoreY.Click += new System.EventHandler(this.btnMoreY_Click);
            // 
            // btnMinusY
            // 
            this.btnMinusY.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinusY.Location = new System.Drawing.Point(3, 339);
            this.btnMinusY.Name = "btnMinusY";
            this.btnMinusY.Size = new System.Drawing.Size(41, 41);
            this.btnMinusY.TabIndex = 2;
            this.btnMinusY.Text = "-";
            this.btnMinusY.UseVisualStyleBackColor = true;
            this.btnMinusY.Click += new System.EventHandler(this.btnMinusY_Click);
            // 
            // btnResetGraph
            // 
            this.btnResetGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.btnResetGraph.Location = new System.Drawing.Point(3, 383);
            this.btnResetGraph.Margin = new System.Windows.Forms.Padding(0);
            this.btnResetGraph.Name = "btnResetGraph";
            this.btnResetGraph.Size = new System.Drawing.Size(41, 41);
            this.btnResetGraph.TabIndex = 1;
            this.btnResetGraph.Text = "RESET";
            this.btnResetGraph.UseVisualStyleBackColor = true;
            this.btnResetGraph.Click += new System.EventHandler(this.btnResetGraph_Click);
            // 
            // plot1
            // 
            this.plot1.Location = new System.Drawing.Point(29, 0);
            this.plot1.Margin = new System.Windows.Forms.Padding(2);
            this.plot1.Name = "plot1";
            this.plot1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plot1.Size = new System.Drawing.Size(745, 402);
            this.plot1.TabIndex = 0;
            this.plot1.Text = "plotView1";
            this.plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // gbAlteraciones
            // 
            this.gbAlteraciones.Controls.Add(this.lblNombreAlteraciones);
            this.gbAlteraciones.Controls.Add(this.lblNombreAlteracion);
            this.gbAlteraciones.Controls.Add(this.lblAltNombreEstatico);
            this.gbAlteraciones.Controls.Add(this.lblNombreRegAlterados);
            this.gbAlteraciones.Controls.Add(this.lblCodigoAlt);
            this.gbAlteraciones.Controls.Add(this.lblNombreRegAlterado);
            this.gbAlteraciones.Controls.Add(this.lblAbrevRegAlterado);
            this.gbAlteraciones.Controls.Add(this.lblAbrevRegAlterados);
            this.gbAlteraciones.Location = new System.Drawing.Point(4, 312);
            this.gbAlteraciones.Name = "gbAlteraciones";
            this.gbAlteraciones.Size = new System.Drawing.Size(210, 110);
            this.gbAlteraciones.TabIndex = 56;
            this.gbAlteraciones.TabStop = false;
            this.gbAlteraciones.Text = "Datos de la Alteración";
            // 
            // lblNombreAlteraciones
            // 
            this.lblNombreAlteraciones.BackColor = System.Drawing.Color.White;
            this.lblNombreAlteraciones.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNombreAlteraciones.Location = new System.Drawing.Point(11, 35);
            this.lblNombreAlteraciones.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombreAlteraciones.MaximumSize = new System.Drawing.Size(750, 812);
            this.lblNombreAlteraciones.Name = "lblNombreAlteraciones";
            this.lblNombreAlteraciones.Size = new System.Drawing.Size(85, 18);
            this.lblNombreAlteraciones.TabIndex = 57;
            this.lblNombreAlteraciones.Text = "---";
            this.lblNombreAlteraciones.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNombreAlteracion
            // 
            this.lblNombreAlteracion.AutoSize = true;
            this.lblNombreAlteracion.Location = new System.Drawing.Point(7, 18);
            this.lblNombreAlteracion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombreAlteracion.Name = "lblNombreAlteracion";
            this.lblNombreAlteracion.Size = new System.Drawing.Size(44, 13);
            this.lblNombreAlteracion.TabIndex = 56;
            this.lblNombreAlteracion.Text = "Nombre";
            // 
            // lblAltNombreEstatico
            // 
            this.lblAltNombreEstatico.AutoSize = true;
            this.lblAltNombreEstatico.Location = new System.Drawing.Point(102, 18);
            this.lblAltNombreEstatico.Name = "lblAltNombreEstatico";
            this.lblAltNombreEstatico.Size = new System.Drawing.Size(63, 13);
            this.lblAltNombreEstatico.TabIndex = 33;
            this.lblAltNombreEstatico.Text = "Descripcion";
            // 
            // lblNombreRegAlterados
            // 
            this.lblNombreRegAlterados.BackColor = System.Drawing.Color.White;
            this.lblNombreRegAlterados.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNombreRegAlterados.Location = new System.Drawing.Point(10, 81);
            this.lblNombreRegAlterados.Name = "lblNombreRegAlterados";
            this.lblNombreRegAlterados.Size = new System.Drawing.Size(85, 18);
            this.lblNombreRegAlterados.TabIndex = 55;
            this.lblNombreRegAlterados.Text = "---";
            this.lblNombreRegAlterados.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblCodigoAlt
            // 
            this.lblCodigoAlt.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCodigoAlt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCodigoAlt.Location = new System.Drawing.Point(105, 35);
            this.lblCodigoAlt.Name = "lblCodigoAlt";
            this.lblCodigoAlt.Size = new System.Drawing.Size(85, 18);
            this.lblCodigoAlt.TabIndex = 32;
            this.lblCodigoAlt.Text = "---";
            this.lblCodigoAlt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNombreRegAlterado
            // 
            this.lblNombreRegAlterado.AutoSize = true;
            this.lblNombreRegAlterado.Location = new System.Drawing.Point(8, 62);
            this.lblNombreRegAlterado.Name = "lblNombreRegAlterado";
            this.lblNombreRegAlterado.Size = new System.Drawing.Size(89, 13);
            this.lblNombreRegAlterado.TabIndex = 54;
            this.lblNombreRegAlterado.Text = "Nombre Régimen";
            // 
            // lblAbrevRegAlterado
            // 
            this.lblAbrevRegAlterado.AutoSize = true;
            this.lblAbrevRegAlterado.Location = new System.Drawing.Point(103, 62);
            this.lblAbrevRegAlterado.Name = "lblAbrevRegAlterado";
            this.lblAbrevRegAlterado.Size = new System.Drawing.Size(83, 13);
            this.lblAbrevRegAlterado.TabIndex = 51;
            this.lblAbrevRegAlterado.Text = "Abrev. Régimen";
            // 
            // lblAbrevRegAlterados
            // 
            this.lblAbrevRegAlterados.AutoEllipsis = true;
            this.lblAbrevRegAlterados.BackColor = System.Drawing.Color.White;
            this.lblAbrevRegAlterados.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAbrevRegAlterados.Location = new System.Drawing.Point(105, 81);
            this.lblAbrevRegAlterados.Name = "lblAbrevRegAlterados";
            this.lblAbrevRegAlterados.Size = new System.Drawing.Size(85, 18);
            this.lblAbrevRegAlterados.TabIndex = 53;
            this.lblAbrevRegAlterados.Text = "---";
            this.lblAbrevRegAlterados.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSelectorAlteracion
            // 
            this.lblSelectorAlteracion.AutoSize = true;
            this.lblSelectorAlteracion.Location = new System.Drawing.Point(8, 275);
            this.lblSelectorAlteracion.Name = "lblSelectorAlteracion";
            this.lblSelectorAlteracion.Size = new System.Drawing.Size(54, 13);
            this.lblSelectorAlteracion.TabIndex = 27;
            this.lblSelectorAlteracion.Text = "Alteración";
            // 
            // _cmbListaAlteradasDiarias
            // 
            this._cmbListaAlteradasDiarias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbListaAlteradasDiarias.FormattingEnabled = true;
            this._cmbListaAlteradasDiarias.Location = new System.Drawing.Point(8, 293);
            this._cmbListaAlteradasDiarias.Name = "_cmbListaAlteradasDiarias";
            this._cmbListaAlteradasDiarias.Size = new System.Drawing.Size(180, 21);
            this._cmbListaAlteradasDiarias.TabIndex = 13;
            this._cmbListaAlteradasDiarias.SelectedIndexChanged += new System.EventHandler(this.cmbListaAlteradasDiarias_SelectedIndexChanged);
            // 
            // _btnCalcular
            // 
            this._btnCalcular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCalcular.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnCalcular.ForeColor = System.Drawing.SystemColors.ControlText;
            this._btnCalcular.Location = new System.Drawing.Point(834, 19);
            this._btnCalcular.Name = "_btnCalcular";
            this._btnCalcular.Size = new System.Drawing.Size(151, 158);
            this._btnCalcular.TabIndex = 4;
            this._btnCalcular.Text = "Calcular";
            this._btnCalcular.UseVisualStyleBackColor = true;
            this._btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // gbInforme
            // 
            this.gbInforme.Controls.Add(this.lstBoxInformes);
            this.gbInforme.Controls.Add(this._btnCalcular);
            this.gbInforme.Location = new System.Drawing.Point(9, 486);
            this.gbInforme.Name = "gbInforme";
            this.gbInforme.Size = new System.Drawing.Size(995, 192);
            this.gbInforme.TabIndex = 10;
            this.gbInforme.TabStop = false;
            this.gbInforme.Text = "Informes a realizar:";
            // 
            // lstBoxInformes
            // 
            this.lstBoxInformes.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstBoxInformes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstBoxInformes.ColumnWidth = 310;
            this.lstBoxInformes.FormattingEnabled = true;
            this.lstBoxInformes.Location = new System.Drawing.Point(12, 19);
            this.lstBoxInformes.MultiColumn = true;
            this.lstBoxInformes.Name = "lstBoxInformes";
            this.lstBoxInformes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstBoxInformes.Size = new System.Drawing.Size(810, 158);
            this.lstBoxInformes.TabIndex = 5;
            // 
            // gbProyecto
            // 
            this.gbProyecto.Controls.Add(this.lblNombreProyectos);
            this.gbProyecto.Controls.Add(this.lblNombreProyecto);
            this.gbProyecto.Controls.Add(this.lblDescripcion);
            this.gbProyecto.Controls.Add(this.lblProyectoDesc);
            this.gbProyecto.Location = new System.Drawing.Point(4, 0);
            this.gbProyecto.Name = "gbProyecto";
            this.gbProyecto.Size = new System.Drawing.Size(210, 135);
            this.gbProyecto.TabIndex = 11;
            this.gbProyecto.TabStop = false;
            this.gbProyecto.Text = "Datos del Proyecto";
            // 
            // lblNombreProyectos
            // 
            this.lblNombreProyectos.BackColor = System.Drawing.Color.White;
            this.lblNombreProyectos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNombreProyectos.Location = new System.Drawing.Point(10, 36);
            this.lblNombreProyectos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombreProyectos.Name = "lblNombreProyectos";
            this.lblNombreProyectos.Size = new System.Drawing.Size(180, 18);
            this.lblNombreProyectos.TabIndex = 49;
            // 
            // lblNombreProyecto
            // 
            this.lblNombreProyecto.AutoSize = true;
            this.lblNombreProyecto.Location = new System.Drawing.Point(10, 18);
            this.lblNombreProyecto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombreProyecto.Name = "lblNombreProyecto";
            this.lblNombreProyecto.Size = new System.Drawing.Size(44, 13);
            this.lblNombreProyecto.TabIndex = 48;
            this.lblNombreProyecto.Text = "Nombre";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(10, 59);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(63, 13);
            this.lblDescripcion.TabIndex = 47;
            this.lblDescripcion.Text = "Descripción";
            // 
            // lblProyectoDesc
            // 
            this.lblProyectoDesc.BackColor = System.Drawing.Color.White;
            this.lblProyectoDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProyectoDesc.Location = new System.Drawing.Point(10, 77);
            this.lblProyectoDesc.Name = "lblProyectoDesc";
            this.lblProyectoDesc.Size = new System.Drawing.Size(180, 45);
            this.lblProyectoDesc.TabIndex = 46;
            // 
            // _cbProyectos
            // 
            this._cbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbProyectos.FormattingEnabled = true;
            this._cbProyectos.Location = new System.Drawing.Point(8, 32);
            this._cbProyectos.Name = "_cbProyectos";
            this._cbProyectos.Size = new System.Drawing.Size(180, 21);
            this._cbProyectos.TabIndex = 0;
            this._cbProyectos.SelectedIndexChanged += new System.EventHandler(this.cbProyectos_SelectedIndexChanged);
            // 
            // lblPuntoNombre
            // 
            this.lblPuntoNombre.AutoSize = true;
            this.lblPuntoNombre.Location = new System.Drawing.Point(103, 18);
            this.lblPuntoNombre.Name = "lblPuntoNombre";
            this.lblPuntoNombre.Size = new System.Drawing.Size(63, 13);
            this.lblPuntoNombre.TabIndex = 3;
            this.lblPuntoNombre.Text = "Descripción";
            // 
            // lblPuntoClave
            // 
            this.lblPuntoClave.AutoSize = true;
            this.lblPuntoClave.Location = new System.Drawing.Point(10, 18);
            this.lblPuntoClave.Name = "lblPuntoClave";
            this.lblPuntoClave.Size = new System.Drawing.Size(40, 13);
            this.lblPuntoClave.TabIndex = 4;
            this.lblPuntoClave.Text = "Código";
            // 
            // lblPuntoNListas
            // 
            this.lblPuntoNListas.AutoSize = true;
            this.lblPuntoNListas.Location = new System.Drawing.Point(8, 116);
            this.lblPuntoNListas.Name = "lblPuntoNListas";
            this.lblPuntoNListas.Size = new System.Drawing.Size(145, 13);
            this.lblPuntoNListas.TabIndex = 5;
            this.lblPuntoNListas.Text = "Nº de alteraciones asociadas";
            // 
            // lblPuntosClave
            // 
            this.lblPuntosClave.BackColor = System.Drawing.Color.White;
            this.lblPuntosClave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPuntosClave.Location = new System.Drawing.Point(10, 40);
            this.lblPuntosClave.Name = "lblPuntosClave";
            this.lblPuntosClave.Size = new System.Drawing.Size(85, 18);
            this.lblPuntosClave.TabIndex = 45;
            // 
            // lblPuntosNombre
            // 
            this.lblPuntosNombre.BackColor = System.Drawing.Color.White;
            this.lblPuntosNombre.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPuntosNombre.Location = new System.Drawing.Point(103, 40);
            this.lblPuntosNombre.Name = "lblPuntosNombre";
            this.lblPuntosNombre.Size = new System.Drawing.Size(85, 18);
            this.lblPuntosNombre.TabIndex = 46;
            // 
            // lblPuntosNListas
            // 
            this.lblPuntosNListas.BackColor = System.Drawing.Color.White;
            this.lblPuntosNListas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPuntosNListas.Location = new System.Drawing.Point(11, 138);
            this.lblPuntosNListas.Name = "lblPuntosNListas";
            this.lblPuntosNListas.Size = new System.Drawing.Size(180, 18);
            this.lblPuntosNListas.TabIndex = 47;
            // 
            // lblAbrevRegNaturales
            // 
            this.lblAbrevRegNaturales.AutoEllipsis = true;
            this.lblAbrevRegNaturales.BackColor = System.Drawing.Color.White;
            this.lblAbrevRegNaturales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAbrevRegNaturales.Location = new System.Drawing.Point(103, 89);
            this.lblAbrevRegNaturales.Name = "lblAbrevRegNaturales";
            this.lblAbrevRegNaturales.Size = new System.Drawing.Size(85, 18);
            this.lblAbrevRegNaturales.TabIndex = 52;
            // 
            // lblAbrevRegNatural
            // 
            this.lblAbrevRegNatural.AutoSize = true;
            this.lblAbrevRegNatural.Location = new System.Drawing.Point(103, 67);
            this.lblAbrevRegNatural.Name = "lblAbrevRegNatural";
            this.lblAbrevRegNatural.Size = new System.Drawing.Size(83, 13);
            this.lblAbrevRegNatural.TabIndex = 50;
            this.lblAbrevRegNatural.Text = "Abrev. Régimen";
            // 
            // lblMesInicio
            // 
            this.lblMesInicio.AutoSize = true;
            this.lblMesInicio.Location = new System.Drawing.Point(8, 322);
            this.lblMesInicio.Name = "lblMesInicio";
            this.lblMesInicio.Size = new System.Drawing.Size(165, 13);
            this.lblMesInicio.TabIndex = 48;
            this.lblMesInicio.Text = "Mes inicio año hidro. para cálculo";
            // 
            // _cmbMesInicio
            // 
            this._cmbMesInicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMesInicio.FormattingEnabled = true;
            this._cmbMesInicio.Location = new System.Drawing.Point(8, 341);
            this._cmbMesInicio.Name = "_cmbMesInicio";
            this._cmbMesInicio.Size = new System.Drawing.Size(180, 21);
            this._cmbMesInicio.TabIndex = 49;
            this._cmbMesInicio.SelectedIndexChanged += new System.EventHandler(this.cmbMesInicio_SelectedIndexChanged);
            // 
            // lblNombreRegNatural
            // 
            this.lblNombreRegNatural.AutoSize = true;
            this.lblNombreRegNatural.Location = new System.Drawing.Point(8, 67);
            this.lblNombreRegNatural.Name = "lblNombreRegNatural";
            this.lblNombreRegNatural.Size = new System.Drawing.Size(92, 13);
            this.lblNombreRegNatural.TabIndex = 53;
            this.lblNombreRegNatural.Text = "Nombre Régimen ";
            // 
            // lblNombreRegNaturales
            // 
            this.lblNombreRegNaturales.BackColor = System.Drawing.Color.White;
            this.lblNombreRegNaturales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNombreRegNaturales.Location = new System.Drawing.Point(10, 89);
            this.lblNombreRegNaturales.Name = "lblNombreRegNaturales";
            this.lblNombreRegNaturales.Size = new System.Drawing.Size(85, 18);
            this.lblNombreRegNaturales.TabIndex = 54;
            // 
            // gbPuntos
            // 
            this.gbPuntos.Controls.Add(this.lblNombreRegNaturales);
            this.gbPuntos.Controls.Add(this.lblNombreRegNatural);
            this.gbPuntos.Controls.Add(this.lblAbrevRegNatural);
            this.gbPuntos.Controls.Add(this.lblAbrevRegNaturales);
            this.gbPuntos.Controls.Add(this.lblPuntosNListas);
            this.gbPuntos.Controls.Add(this.lblPuntosNombre);
            this.gbPuntos.Controls.Add(this.lblPuntosClave);
            this.gbPuntos.Controls.Add(this.lblPuntoNListas);
            this.gbPuntos.Controls.Add(this.lblPuntoClave);
            this.gbPuntos.Controls.Add(this.lblPuntoNombre);
            this.gbPuntos.Location = new System.Drawing.Point(4, 139);
            this.gbPuntos.Name = "gbPuntos";
            this.gbPuntos.Size = new System.Drawing.Size(210, 169);
            this.gbPuntos.TabIndex = 2;
            this.gbPuntos.TabStop = false;
            this.gbPuntos.Text = "Datos del Punto";
            // 
            // lblSelectorPunto
            // 
            this.lblSelectorPunto.AutoSize = true;
            this.lblSelectorPunto.Location = new System.Drawing.Point(8, 59);
            this.lblSelectorPunto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectorPunto.Name = "lblSelectorPunto";
            this.lblSelectorPunto.Size = new System.Drawing.Size(35, 13);
            this.lblSelectorPunto.TabIndex = 2;
            this.lblSelectorPunto.Text = "Punto";
            // 
            // lblSelectorProyecto
            // 
            this.lblSelectorProyecto.AutoSize = true;
            this.lblSelectorProyecto.Location = new System.Drawing.Point(8, 12);
            this.lblSelectorProyecto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectorProyecto.Name = "lblSelectorProyecto";
            this.lblSelectorProyecto.Size = new System.Drawing.Size(49, 13);
            this.lblSelectorProyecto.TabIndex = 1;
            this.lblSelectorProyecto.Text = "Proyecto";
            // 
            // tabcDatos
            // 
            this.tabcDatos.Controls.Add(this.tab_Selectores);
            this.tabcDatos.Controls.Add(this.tab_Datos);
            this.tabcDatos.Location = new System.Drawing.Point(9, 26);
            this.tabcDatos.Margin = new System.Windows.Forms.Padding(2);
            this.tabcDatos.Name = "tabcDatos";
            this.tabcDatos.SelectedIndex = 0;
            this.tabcDatos.Size = new System.Drawing.Size(210, 455);
            this.tabcDatos.TabIndex = 58;
            // 
            // tab_Selectores
            // 
            this.tab_Selectores.BackColor = System.Drawing.Color.Transparent;
            this.tab_Selectores.Controls.Add(this.rbCaudalesEco);
            this.tab_Selectores.Controls.Add(this.rbIndices);
            this.tab_Selectores.Controls.Add(this.lblSelectorPunto);
            this.tab_Selectores.Controls.Add(this.lblSelectorProyecto);
            this.tab_Selectores.Controls.Add(this._cmbListaAlteradasDiarias);
            this.tab_Selectores.Controls.Add(this.lblSelectorAlteracion);
            this.tab_Selectores.Controls.Add(this._cmbMesInicio);
            this.tab_Selectores.Controls.Add(this._lstboxPuntos);
            this.tab_Selectores.Controls.Add(this._cbProyectos);
            this.tab_Selectores.Controls.Add(this.lblMesInicio);
            this.tab_Selectores.Location = new System.Drawing.Point(4, 22);
            this.tab_Selectores.Margin = new System.Windows.Forms.Padding(2);
            this.tab_Selectores.Name = "tab_Selectores";
            this.tab_Selectores.Padding = new System.Windows.Forms.Padding(2);
            this.tab_Selectores.Size = new System.Drawing.Size(202, 429);
            this.tab_Selectores.TabIndex = 0;
            this.tab_Selectores.Text = "Selección";
            // 
            // rbCaudalesEco
            // 
            this.rbCaudalesEco.AutoSize = true;
            this.rbCaudalesEco.Location = new System.Drawing.Point(11, 400);
            this.rbCaudalesEco.Name = "rbCaudalesEco";
            this.rbCaudalesEco.Size = new System.Drawing.Size(124, 17);
            this.rbCaudalesEco.TabIndex = 51;
            this.rbCaudalesEco.Text = "Caudales Ecológicos";
            this.rbCaudalesEco.UseVisualStyleBackColor = true;
            this.rbCaudalesEco.CheckedChanged += new System.EventHandler(this.rbCaudalesEco_CheckedChanged);
            // 
            // rbIndices
            // 
            this.rbIndices.AutoSize = true;
            this.rbIndices.Checked = true;
            this.rbIndices.Location = new System.Drawing.Point(11, 376);
            this.rbIndices.Name = "rbIndices";
            this.rbIndices.Size = new System.Drawing.Size(59, 17);
            this.rbIndices.TabIndex = 50;
            this.rbIndices.TabStop = true;
            this.rbIndices.Text = "Índices";
            this.rbIndices.UseVisualStyleBackColor = true;
            this.rbIndices.CheckedChanged += new System.EventHandler(this.rbIndices_CheckedChanged);
            // 
            // tab_Datos
            // 
            this.tab_Datos.BackColor = System.Drawing.Color.Transparent;
            this.tab_Datos.Controls.Add(this.gbProyecto);
            this.tab_Datos.Controls.Add(this.gbPuntos);
            this.tab_Datos.Controls.Add(this.gbAlteraciones);
            this.tab_Datos.Location = new System.Drawing.Point(4, 22);
            this.tab_Datos.Margin = new System.Windows.Forms.Padding(2);
            this.tab_Datos.Name = "tab_Datos";
            this.tab_Datos.Padding = new System.Windows.Forms.Padding(2);
            this.tab_Datos.Size = new System.Drawing.Size(202, 429);
            this.tab_Datos.TabIndex = 1;
            this.tab_Datos.Text = "Datos";
            // 
            // FormInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1260, 853);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tabcDatos);
            this.Controls.Add(this.gbInforme);
            this.Controls.Add(this.MenuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MenuStrip1;
            this.MaximizeBox = false;
            this.Name = "FormInicial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IAHRIS (Índices de Alteración hidrológica en RIoS) ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInicial_FormClosing);
            this.Load += new System.EventHandler(this.FormInicial_Load);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.gbSeriesValoresAlt.ResumeLayout(false);
            this.gbSeriesValoresAlt.PerformLayout();
            this.gbSeriesValoresNat.ResumeLayout(false);
            this.gbSeriesValoresNat.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabpSeries.ResumeLayout(false);
            this.tabpSeries.PerformLayout();
            this.gbCoetane.ResumeLayout(false);
            this.gbCoetane.PerformLayout();
            this.grpboxLeyenda.ResumeLayout(false);
            this.grpboxLeyenda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XPTablaListas)).EndInit();
            this.tabpEscenarios.ResumeLayout(false);
            this.tabpEscenarios.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGrid_Escenarios)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tabpValoraciones.ResumeLayout(false);
            this.gbAlteraciones.ResumeLayout(false);
            this.gbAlteraciones.PerformLayout();
            this.gbInforme.ResumeLayout(false);
            this.gbProyecto.ResumeLayout(false);
            this.gbProyecto.PerformLayout();
            this.gbPuntos.ResumeLayout(false);
            this.gbPuntos.PerformLayout();
            this.tabcDatos.ResumeLayout(false);
            this.tab_Selectores.ResumeLayout(false);
            this.tab_Selectores.PerformLayout();
            this.tab_Datos.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal MenuStrip MenuStrip1;
        internal ToolStripMenuItem GestiónDePuntosToolStripMenuItem;
        private ToolStripMenuItem _AñadirPuntoToolStripMenuItem;

        internal ToolStripMenuItem AñadirPuntoToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AñadirPuntoToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AñadirPuntoToolStripMenuItem != null)
                {
                    _AñadirPuntoToolStripMenuItem.Click -= AñadirPuntoToolStripMenuItem_Click;
                }

                _AñadirPuntoToolStripMenuItem = value;
                if (_AñadirPuntoToolStripMenuItem != null)
                {
                    _AñadirPuntoToolStripMenuItem.Click += AñadirPuntoToolStripMenuItem_Click;
                }
            }
        }

        private ListBox _lstboxPuntos;

        internal ListBox lstboxPuntos
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lstboxPuntos;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lstboxPuntos != null)
                {
                    _lstboxPuntos.SelectedIndexChanged -= lstboxPuntos_SelectedIndexChanged;
                }

                _lstboxPuntos = value;
                if (_lstboxPuntos != null)
                {
                    _lstboxPuntos.SelectedIndexChanged += lstboxPuntos_SelectedIndexChanged;
                }
            }
        }

        internal ToolStripMenuItem GestiónDeListasToolStripMenuItem;
        private ToolStripMenuItem _AñadirListaToolStripMenuItem;

        internal ToolStripMenuItem AñadirListaToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AñadirListaToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AñadirListaToolStripMenuItem != null)
                {
                    _AñadirListaToolStripMenuItem.Click -= AñadirListaToolStripMenuItem_Click;
                }

                _AñadirListaToolStripMenuItem = value;
                if (_AñadirListaToolStripMenuItem != null)
                {
                    _AñadirListaToolStripMenuItem.Click += AñadirListaToolStripMenuItem_Click;
                }
            }
        }
        private ToolStripMenuItem _EliminarPuntoToolStripMenuItem;

        internal ToolStripMenuItem EliminarPuntoToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _EliminarPuntoToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_EliminarPuntoToolStripMenuItem != null)
                {
                    _EliminarPuntoToolStripMenuItem.Click -= EliminarPuntoToolStripMenuItem_Click;
                }

                _EliminarPuntoToolStripMenuItem = value;
                if (_EliminarPuntoToolStripMenuItem != null)
                {
                    _EliminarPuntoToolStripMenuItem.Click += EliminarPuntoToolStripMenuItem_Click;
                }
            }
        }

        private ToolStripMenuItem _EliminarListaToolStripMenuItem;

        internal ToolStripMenuItem EliminarListaToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _EliminarListaToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_EliminarListaToolStripMenuItem != null)
                {
                    _EliminarListaToolStripMenuItem.Click -= EliminarListaToolStripMenuItem_Click;
                }

                _EliminarListaToolStripMenuItem = value;
                if (_EliminarListaToolStripMenuItem != null)
                {
                    _EliminarListaToolStripMenuItem.Click += EliminarListaToolStripMenuItem_Click;
                }
            }
        }
        internal XPTable.Models.Table XPTablaListas;
        internal XPTable.Models.ColumnModel ColumnModel1;
        internal XPTable.Models.TableModel TableModel1;
        private Button _btnCalcular;

        internal Button btnCalcular
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCalcular;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCalcular != null)
                {
                    _btnCalcular.Click -= btnCalcular_Click;
                }

                _btnCalcular = value;
                if (_btnCalcular != null)
                {
                    _btnCalcular.Click += btnCalcular_Click;
                }
            }
        }

        private ComboBox _cmbListaAlteradasDiarias;

        internal ComboBox cmbListaAlteradasDiarias
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbListaAlteradasDiarias;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbListaAlteradasDiarias != null)
                {
                    _cmbListaAlteradasDiarias.SelectedIndexChanged -= cmbListaAlteradasDiarias_SelectedIndexChanged;
                }

                _cmbListaAlteradasDiarias = value;
                if (_cmbListaAlteradasDiarias != null)
                {
                    _cmbListaAlteradasDiarias.SelectedIndexChanged += cmbListaAlteradasDiarias_SelectedIndexChanged;
                }
            }
        }
        internal Label lblAñosCoeMensual;
        internal Label lblAñosAltMensual;
        internal Label lblAñosNatMensual;
        internal Label lblAñosAltDiario;
        internal Label lblAñosNatDiario;
        internal Label lblAñosHidro;
        internal Label lblTotalAños;
        internal Label lblSelectorAlteracion;
        internal Label lblMensual;
        internal Label lblDiario;
        internal GroupBox gbInforme;
        internal Label lblASerieMensual;
        internal Label lblASerieDiaria;
        internal ToolStripMenuItem GestiónDeAlteracionesToolStripMenuItem;
        private ToolStripMenuItem _AñadirAlteraciónToolStripMenuItem;

        internal ToolStripMenuItem AñadirAlteraciónToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AñadirAlteraciónToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AñadirAlteraciónToolStripMenuItem != null)
                {
                    _AñadirAlteraciónToolStripMenuItem.Click -= AñadirAlteraciónToolStripMenuItem_Click;
                }

                _AñadirAlteraciónToolStripMenuItem = value;
                if (_AñadirAlteraciónToolStripMenuItem != null)
                {
                    _AñadirAlteraciónToolStripMenuItem.Click += AñadirAlteraciónToolStripMenuItem_Click;
                }
            }
        }

        private ToolStripMenuItem _EliminarAlteraciónToolStripMenuItem;

        internal ToolStripMenuItem EliminarAlteraciónToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _EliminarAlteraciónToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_EliminarAlteraciónToolStripMenuItem != null)
                {
                    _EliminarAlteraciónToolStripMenuItem.Click -= EliminarAlteraciónToolStripMenuItem_Click;
                }

                _EliminarAlteraciónToolStripMenuItem = value;
                if (_EliminarAlteraciónToolStripMenuItem != null)
                {
                    _EliminarAlteraciónToolStripMenuItem.Click += EliminarAlteraciónToolStripMenuItem_Click;
                }
            }
        }

        internal Label lblIDMensual;
        internal Label lblIDDiaria;
        internal Label lblAltNombreEstatico;
        internal Label lblCodigoAlt;
        private CheckBox _chkboxUsarCoe;

        internal CheckBox chkboxUsarCoe
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkboxUsarCoe;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkboxUsarCoe != null)
                {
                    _chkboxUsarCoe.CheckedChanged -= chkboxUsarCoe_CheckedChanged;
                }

                _chkboxUsarCoe = value;
                if (_chkboxUsarCoe != null)
                {
                    _chkboxUsarCoe.CheckedChanged += chkboxUsarCoe_CheckedChanged;
                }
            }
        }

        internal Label lblSerieNatMensual;
        internal Label lblSerieNatDiaria;
        internal GroupBox grpboxLeyenda;
        private CheckBox _chkboxUsarCoeDiaria;

        internal CheckBox chkboxUsarCoeDiaria
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkboxUsarCoeDiaria;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkboxUsarCoeDiaria != null)
                {
                    _chkboxUsarCoeDiaria.CheckedChanged -= chkboxUsarCoeDiaria_CheckedChanged;
                }

                _chkboxUsarCoeDiaria = value;
                if (_chkboxUsarCoeDiaria != null)
                {
                    _chkboxUsarCoeDiaria.CheckedChanged += chkboxUsarCoeDiaria_CheckedChanged;
                }
            }
        }

        internal Label lblAñosCoeDiaria;
        internal Label lblAñosAltDiarioUSO;
        internal Label lblAñosNatDiarioUSO;
        internal Label lblAñosAltMensualUSO;
        internal Label lblAñosNatMensualUSO;
        internal Label lblDatosDiarios;
        internal Label lblDatosMensuales;
        internal ListBox lstBoxInformes;
        internal Label lblLeyenda3;
        internal Label lblLeyenda7;
        internal Label lblLeyenda1;
        internal Label lblLeyenda2;
        internal Label lblLeyenda4;
        internal Label lblLeyenda5;
        internal Label lblLeyenda6;
        internal ToolStripMenuItem IdiomasToolStripMenuItem;
        private ToolStripMenuItem _ManualesToolStripMenuItem;

        internal ToolStripMenuItem ManualesToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ManualesToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ManualesToolStripMenuItem != null)
                {
                    _ManualesToolStripMenuItem.Click -= ManualesToolStripMenuItem_Click;
                }

                _ManualesToolStripMenuItem = value;
                if (_ManualesToolStripMenuItem != null)
                {
                    _ManualesToolStripMenuItem.Click += ManualesToolStripMenuItem_Click;
                }
            }
        }

        internal GroupBox gbProyecto;
        internal Label lblDescripcion;
        internal Label lblProyectoDesc;
        private ComboBox _cbProyectos;

        internal ComboBox cbProyectos
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cbProyectos;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cbProyectos != null)
                {
                    _cbProyectos.SelectedIndexChanged -= cbProyectos_SelectedIndexChanged;
                }

                _cbProyectos = value;
                if (_cbProyectos != null)
                {
                    _cbProyectos.SelectedIndexChanged += cbProyectos_SelectedIndexChanged;
                }
            }
        }

        internal ToolStripMenuItem GestiónDeProyectosToolStripMenuItem;
        private ToolStripMenuItem _AñadirProyectoToolStripMenuItem;

        internal ToolStripMenuItem AñadirProyectoToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AñadirProyectoToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AñadirProyectoToolStripMenuItem != null)
                {
                    _AñadirProyectoToolStripMenuItem.Click -= AñadirProyectoToolStripMenuItem_Click;
                }

                _AñadirProyectoToolStripMenuItem = value;
                if (_AñadirProyectoToolStripMenuItem != null)
                {
                    _AñadirProyectoToolStripMenuItem.Click += AñadirProyectoToolStripMenuItem_Click;
                }
            }
        }

        private ToolStripMenuItem _EliminarProyectoToolStripMenuItem;

        internal ToolStripMenuItem EliminarProyectoToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _EliminarProyectoToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_EliminarProyectoToolStripMenuItem != null)
                {
                    _EliminarProyectoToolStripMenuItem.Click -= EliminarProyectoToolStripMenuItem_Click;
                }

                _EliminarProyectoToolStripMenuItem = value;
                if (_EliminarProyectoToolStripMenuItem != null)
                {
                    _EliminarProyectoToolStripMenuItem.Click += EliminarProyectoToolStripMenuItem_Click;
                }
            }
        }

        internal ToolStripMenuItem GestBBDDToolStripMenuItem;
        private ToolStripMenuItem _ImportarToolStripMenuItem;

        internal ToolStripMenuItem ImportarToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ImportarToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ImportarToolStripMenuItem != null)
                {
                    _ImportarToolStripMenuItem.Click -= ImportarToolStripMenuItem_Click;
                }

                _ImportarToolStripMenuItem = value;
                if (_ImportarToolStripMenuItem != null)
                {
                    _ImportarToolStripMenuItem.Click += ImportarToolStripMenuItem_Click;
                }
            }
        }

        private ToolStripMenuItem _ExportarToolStripMenuItem;

        internal ToolStripMenuItem ExportarToolStripMenuItem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ExportarToolStripMenuItem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ExportarToolStripMenuItem != null)
                {
                    _ExportarToolStripMenuItem.Click -= ExportarToolStripMenuItem_Click;
                }

                _ExportarToolStripMenuItem = value;
                if (_ExportarToolStripMenuItem != null)
                {
                    _ExportarToolStripMenuItem.Click += ExportarToolStripMenuItem_Click;
                }
            }
        }

        internal ComboBox cmbMesInicio
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbMesInicio;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbMesInicio != null)
                {
                    _cmbMesInicio.SelectedIndexChanged -= cmbMesInicio_SelectedIndexChanged;
                }

                _cmbMesInicio = value;
                if (_cmbMesInicio != null)
                {
                    _cmbMesInicio.SelectedIndexChanged += cmbMesInicio_SelectedIndexChanged;
                }
            }
        }
        private ToolStripMenuItem EditarProyectoToolStripMenuItem;
        internal Label lblAbrevRegAlterado;
        private Label lblAbrevRegAlterados;
        private GroupBox gbAlteraciones;
        private Label lblNombreRegAlterados;
        private Label lblNombreRegAlterado;
        private ToolStripMenuItem _EditarPuntoToolStripMenuItem;
        private ToolStripMenuItem _EditarAlteraciónToolStripMenuItem;
        private ToolStripMenuItem utilidadesToolStripMenuItem;
        private ToolStripMenuItem cargaMasivaToolStripMenuItem;
        private ToolStripMenuItem importarDatosSIMPAToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabpSeries;
        private TabPage tabpEscenarios;
        private TabPage tabpValoraciones;
        private DataGridView dGrid_Escenarios;
        internal Label lblPuntoNombre;
        internal Label lblPuntoClave;
        internal Label lblPuntoNListas;
        internal Label lblPuntosClave;
        internal Label lblPuntosNombre;
        internal Label lblPuntosNListas;
        private Label lblAbrevRegNaturales;
        internal Label lblAbrevRegNatural;
        internal Label lblMesInicio;
        private ComboBox _cmbMesInicio;
        private Label lblNombreRegNatural;
        private Label lblNombreRegNaturales;
        internal GroupBox gbPuntos;
        private Label lblSelectorPunto;
        private Label lblSelectorProyecto;
        private GroupBox gbSeriesValoresAlt;
        private GroupBox gbSeriesValoresNat;
        private Label lblNombreAlteraciones;
        private Label lblNombreAlteracion;
        private Label lblNombreProyecto;
        private Label lblNombreProyectos;
        private TabControl tabcDatos;
        private TabPage tab_Selectores;
        private TabPage tab_Datos;
        private GroupBox gbCoetane;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem eliminarToolStripMenuItem;
        private ToolStripMenuItem añadirToolStripMenuItem;
        private ToolStripMenuItem editarToolStripMenuItem;
        private RadioButton rbCaudalesEco;
        private RadioButton rbIndices;
        private OxyPlot.WindowsForms.PlotView plot1;
        private ToolStripMenuItem añadirToolStripMenuItem1;
        private ToolStripMenuItem añadirRNORMToolStripMenuItem;
        private Button btnResetGraph;
        private Button btnMoreX;
        private Button btnMinusX;
        private Button btnMoreY;
        private Button btnMinusY;
        private Button btnUp;
        private Button btnDown;
        private Button btnRight;
        private Button btnLeft;
        private Button btnZoomIn;
        private Button btnZoomOut;
        private ToolStripMenuItem editarToolStripMenuItem1;
        private ToolStripMenuItem editarRNORMToolStripMenuItem;
        private DataGridViewCheckBoxColumn Seleccionar;
        private DataGridViewTextBoxColumn IdEscenario;
        private DataGridViewTextBoxColumn Escenario;
        private DataGridViewTextBoxColumn Mes1;
        private DataGridViewTextBoxColumn Mes2;
        private DataGridViewTextBoxColumn Mes3;
        private DataGridViewTextBoxColumn Mes4;
        private DataGridViewTextBoxColumn Mes5;
        private DataGridViewTextBoxColumn Mes6;
        private DataGridViewTextBoxColumn Mes7;
        private DataGridViewTextBoxColumn Mes8;
        private DataGridViewTextBoxColumn Mes9;
        private DataGridViewTextBoxColumn Mes10;
        private DataGridViewTextBoxColumn Mes11;
        private DataGridViewTextBoxColumn Mes12;
        private DataGridViewTextBoxColumn Puntuacion;
        private DataGridViewTextBoxColumn Demanda_Ambiental;
        private DataGridViewTextBoxColumn Eficiencia;
    }
}