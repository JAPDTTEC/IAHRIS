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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInicial));
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.GestiónDeProyectosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestiónDePuntosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirPuntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarPuntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestiónDeAlteracionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirAlteraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarAlteraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestiónDeListasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._AñadirListaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._EliminarListaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GestBBDDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ImportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._ExportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IdiomasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._lstboxPuntos = new System.Windows.Forms.ListBox();
            this.gbPuntos = new System.Windows.Forms.GroupBox();
            this._cmbMesInicio = new System.Windows.Forms.ComboBox();
            this.lblMesInicio = new System.Windows.Forms.Label();
            this.lblPuntosNListas = new System.Windows.Forms.Label();
            this.lblPuntosNombre = new System.Windows.Forms.Label();
            this.lblPuntosClave = new System.Windows.Forms.Label();
            this.lblPuntoNListas = new System.Windows.Forms.Label();
            this.lblPuntoClave = new System.Windows.Forms.Label();
            this.lblPuntoNombre = new System.Windows.Forms.Label();
            this.gbListas = new System.Windows.Forms.GroupBox();
            this.lblDatosDiarios = new System.Windows.Forms.Label();
            this.lblDatosMensuales = new System.Windows.Forms.Label();
            this.lblAñosAltDiarioUSO = new System.Windows.Forms.Label();
            this.lblAñosNatDiarioUSO = new System.Windows.Forms.Label();
            this.lblAñosAltMensualUSO = new System.Windows.Forms.Label();
            this.lblAñosNatMensualUSO = new System.Windows.Forms.Label();
            this.lblAñosCoeDiaria = new System.Windows.Forms.Label();
            this.lblAñosNatMensual = new System.Windows.Forms.Label();
            this._chkboxUsarCoeDiaria = new System.Windows.Forms.CheckBox();
            this.grpboxLeyenda = new System.Windows.Forms.GroupBox();
            this.lblLeyenda6 = new System.Windows.Forms.Label();
            this.lblLeyenda5 = new System.Windows.Forms.Label();
            this.lblLeyenda4 = new System.Windows.Forms.Label();
            this.lblLeyenda2 = new System.Windows.Forms.Label();
            this.lblLeyenda3 = new System.Windows.Forms.Label();
            this.lblLeyenda7 = new System.Windows.Forms.Label();
            this.lblLeyenda1 = new System.Windows.Forms.Label();
            this.lblSerieNatMensual = new System.Windows.Forms.Label();
            this._chkboxUsarCoe = new System.Windows.Forms.CheckBox();
            this.lblSerieNatDiaria = new System.Windows.Forms.Label();
            this.lblAltNombreEstatico = new System.Windows.Forms.Label();
            this.lblCodigoAlt = new System.Windows.Forms.Label();
            this.lblIDMensual = new System.Windows.Forms.Label();
            this.lblIDDiaria = new System.Windows.Forms.Label();
            this.lblASerieMensual = new System.Windows.Forms.Label();
            this.lblASerieDiaria = new System.Windows.Forms.Label();
            this.lblADiario = new System.Windows.Forms.Label();
            this.lblMensual = new System.Windows.Forms.Label();
            this.lblDiario = new System.Windows.Forms.Label();
            this.lblAñosCoeMensual = new System.Windows.Forms.Label();
            this.lblAñosAltMensual = new System.Windows.Forms.Label();
            this.lblAñosAltDiario = new System.Windows.Forms.Label();
            this.lblAñosNatDiario = new System.Windows.Forms.Label();
            this.lblAñosHidro = new System.Windows.Forms.Label();
            this.lblTotalAños = new System.Windows.Forms.Label();
            this._cmbListaAlteradasDiarias = new System.Windows.Forms.ComboBox();
            this.lblListasAlteradas = new System.Windows.Forms.Label();
            this.lblListasNaturales = new System.Windows.Forms.Label();
            this.XPTablaListas = new XPTable.Models.Table();
            this.ColumnModel1 = new XPTable.Models.ColumnModel();
            this.TableModel1 = new XPTable.Models.TableModel();
            this._btnCalcular = new System.Windows.Forms.Button();
            this.gbInforme = new System.Windows.Forms.GroupBox();
            this.lstBoxInformes = new System.Windows.Forms.ListBox();
            this.gbProyecto = new System.Windows.Forms.GroupBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblProyectoDesc = new System.Windows.Forms.Label();
            this._cbProyectos = new System.Windows.Forms.ComboBox();
            this.MenuStrip1.SuspendLayout();
            this.gbPuntos.SuspendLayout();
            this.gbListas.SuspendLayout();
            this.grpboxLeyenda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XPTablaListas)).BeginInit();
            this.gbInforme.SuspendLayout();
            this.gbProyecto.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GestiónDeProyectosToolStripMenuItem,
            this.GestiónDePuntosToolStripMenuItem,
            this.GestiónDeAlteracionesToolStripMenuItem,
            this.GestiónDeListasToolStripMenuItem,
            this.GestBBDDToolStripMenuItem,
            this.IdiomasToolStripMenuItem});
            this.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(1036, 23);
            this.MenuStrip1.TabIndex = 0;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // GestiónDeProyectosToolStripMenuItem
            // 
            this.GestiónDeProyectosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._AñadirProyectoToolStripMenuItem,
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
            this._EliminarAlteraciónToolStripMenuItem});
            this.GestiónDeAlteracionesToolStripMenuItem.Name = "GestiónDeAlteracionesToolStripMenuItem";
            this.GestiónDeAlteracionesToolStripMenuItem.Size = new System.Drawing.Size(143, 19);
            this.GestiónDeAlteracionesToolStripMenuItem.Text = "Gestión de Alteraciones";
            // 
            // _AñadirAlteraciónToolStripMenuItem
            // 
            this._AñadirAlteraciónToolStripMenuItem.Name = "_AñadirAlteraciónToolStripMenuItem";
            this._AñadirAlteraciónToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this._AñadirAlteraciónToolStripMenuItem.Text = "Añadir Alteración";
            this._AñadirAlteraciónToolStripMenuItem.Click += new System.EventHandler(this.AñadirAlteraciónToolStripMenuItem_Click);
            // 
            // _EliminarAlteraciónToolStripMenuItem
            // 
            this._EliminarAlteraciónToolStripMenuItem.Name = "_EliminarAlteraciónToolStripMenuItem";
            this._EliminarAlteraciónToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
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
            // IdiomasToolStripMenuItem
            // 
            this.IdiomasToolStripMenuItem.Enabled = false;
            this.IdiomasToolStripMenuItem.Name = "IdiomasToolStripMenuItem";
            this.IdiomasToolStripMenuItem.Size = new System.Drawing.Size(113, 19);
            this.IdiomasToolStripMenuItem.Text = "Idioma/Language";
            // 
            // _lstboxPuntos
            // 
            this._lstboxPuntos.FormattingEnabled = true;
            this._lstboxPuntos.Location = new System.Drawing.Point(12, 19);
            this._lstboxPuntos.Name = "_lstboxPuntos";
            this._lstboxPuntos.ScrollAlwaysVisible = true;
            this._lstboxPuntos.Size = new System.Drawing.Size(160, 121);
            this._lstboxPuntos.Sorted = true;
            this._lstboxPuntos.TabIndex = 1;
            this._lstboxPuntos.TabStop = false;
            this._lstboxPuntos.SelectedIndexChanged += new System.EventHandler(this.lstboxPuntos_SelectedIndexChanged);
            // 
            // gbPuntos
            // 
            this.gbPuntos.Controls.Add(this._cmbMesInicio);
            this.gbPuntos.Controls.Add(this.lblMesInicio);
            this.gbPuntos.Controls.Add(this.lblPuntosNListas);
            this.gbPuntos.Controls.Add(this.lblPuntosNombre);
            this.gbPuntos.Controls.Add(this.lblPuntosClave);
            this.gbPuntos.Controls.Add(this._lstboxPuntos);
            this.gbPuntos.Controls.Add(this.lblPuntoNListas);
            this.gbPuntos.Controls.Add(this.lblPuntoClave);
            this.gbPuntos.Controls.Add(this.lblPuntoNombre);
            this.gbPuntos.Location = new System.Drawing.Point(12, 122);
            this.gbPuntos.Name = "gbPuntos";
            this.gbPuntos.Size = new System.Drawing.Size(182, 304);
            this.gbPuntos.TabIndex = 2;
            this.gbPuntos.TabStop = false;
            this.gbPuntos.Text = "Datos del Punto";
            // 
            // _cmbMesInicio
            // 
            this._cmbMesInicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMesInicio.FormattingEnabled = true;
            this._cmbMesInicio.Location = new System.Drawing.Point(12, 273);
            this._cmbMesInicio.Name = "_cmbMesInicio";
            this._cmbMesInicio.Size = new System.Drawing.Size(160, 21);
            this._cmbMesInicio.TabIndex = 49;
            this._cmbMesInicio.SelectedIndexChanged += new System.EventHandler(this.cmbMesInicio_SelectedIndexChanged);
            // 
            // lblMesInicio
            // 
            this.lblMesInicio.AutoSize = true;
            this.lblMesInicio.Location = new System.Drawing.Point(9, 257);
            this.lblMesInicio.Name = "lblMesInicio";
            this.lblMesInicio.Size = new System.Drawing.Size(165, 13);
            this.lblMesInicio.TabIndex = 48;
            this.lblMesInicio.Text = "Mes inicio año hidro. para cálculo";
            // 
            // lblPuntosNListas
            // 
            this.lblPuntosNListas.BackColor = System.Drawing.Color.White;
            this.lblPuntosNListas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPuntosNListas.Location = new System.Drawing.Point(12, 234);
            this.lblPuntosNListas.Name = "lblPuntosNListas";
            this.lblPuntosNListas.Size = new System.Drawing.Size(160, 18);
            this.lblPuntosNListas.TabIndex = 47;
            // 
            // lblPuntosNombre
            // 
            this.lblPuntosNombre.BackColor = System.Drawing.Color.White;
            this.lblPuntosNombre.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPuntosNombre.Location = new System.Drawing.Point(12, 196);
            this.lblPuntosNombre.Name = "lblPuntosNombre";
            this.lblPuntosNombre.Size = new System.Drawing.Size(160, 18);
            this.lblPuntosNombre.TabIndex = 46;
            // 
            // lblPuntosClave
            // 
            this.lblPuntosClave.BackColor = System.Drawing.Color.White;
            this.lblPuntosClave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPuntosClave.Location = new System.Drawing.Point(12, 158);
            this.lblPuntosClave.Name = "lblPuntosClave";
            this.lblPuntosClave.Size = new System.Drawing.Size(160, 18);
            this.lblPuntosClave.TabIndex = 45;
            // 
            // lblPuntoNListas
            // 
            this.lblPuntoNListas.AutoSize = true;
            this.lblPuntoNListas.Location = new System.Drawing.Point(9, 219);
            this.lblPuntoNListas.Name = "lblPuntoNListas";
            this.lblPuntoNListas.Size = new System.Drawing.Size(145, 13);
            this.lblPuntoNListas.TabIndex = 5;
            this.lblPuntoNListas.Text = "Nº de alteraciones asociadas";
            // 
            // lblPuntoClave
            // 
            this.lblPuntoClave.AutoSize = true;
            this.lblPuntoClave.Location = new System.Drawing.Point(9, 143);
            this.lblPuntoClave.Name = "lblPuntoClave";
            this.lblPuntoClave.Size = new System.Drawing.Size(88, 13);
            this.lblPuntoClave.TabIndex = 4;
            this.lblPuntoClave.Text = "Código del Punto";
            // 
            // lblPuntoNombre
            // 
            this.lblPuntoNombre.AutoSize = true;
            this.lblPuntoNombre.Location = new System.Drawing.Point(9, 181);
            this.lblPuntoNombre.Name = "lblPuntoNombre";
            this.lblPuntoNombre.Size = new System.Drawing.Size(111, 13);
            this.lblPuntoNombre.TabIndex = 3;
            this.lblPuntoNombre.Text = "Descripción del Punto";
            // 
            // gbListas
            // 
            this.gbListas.Controls.Add(this.lblDatosDiarios);
            this.gbListas.Controls.Add(this.lblDatosMensuales);
            this.gbListas.Controls.Add(this.lblAñosAltDiarioUSO);
            this.gbListas.Controls.Add(this.lblAñosNatDiarioUSO);
            this.gbListas.Controls.Add(this.lblAñosAltMensualUSO);
            this.gbListas.Controls.Add(this.lblAñosNatMensualUSO);
            this.gbListas.Controls.Add(this.lblAñosCoeDiaria);
            this.gbListas.Controls.Add(this.lblAñosNatMensual);
            this.gbListas.Controls.Add(this._chkboxUsarCoeDiaria);
            this.gbListas.Controls.Add(this.grpboxLeyenda);
            this.gbListas.Controls.Add(this.lblSerieNatMensual);
            this.gbListas.Controls.Add(this._chkboxUsarCoe);
            this.gbListas.Controls.Add(this.lblSerieNatDiaria);
            this.gbListas.Controls.Add(this.lblAltNombreEstatico);
            this.gbListas.Controls.Add(this.lblCodigoAlt);
            this.gbListas.Controls.Add(this.lblIDMensual);
            this.gbListas.Controls.Add(this.lblIDDiaria);
            this.gbListas.Controls.Add(this.lblASerieMensual);
            this.gbListas.Controls.Add(this.lblASerieDiaria);
            this.gbListas.Controls.Add(this.lblADiario);
            this.gbListas.Controls.Add(this.lblMensual);
            this.gbListas.Controls.Add(this.lblDiario);
            this.gbListas.Controls.Add(this.lblAñosCoeMensual);
            this.gbListas.Controls.Add(this.lblAñosAltMensual);
            this.gbListas.Controls.Add(this.lblAñosAltDiario);
            this.gbListas.Controls.Add(this.lblAñosNatDiario);
            this.gbListas.Controls.Add(this.lblAñosHidro);
            this.gbListas.Controls.Add(this.lblTotalAños);
            this.gbListas.Controls.Add(this._cmbListaAlteradasDiarias);
            this.gbListas.Controls.Add(this.lblListasAlteradas);
            this.gbListas.Controls.Add(this.lblListasNaturales);
            this.gbListas.Controls.Add(this.XPTablaListas);
            this.gbListas.Location = new System.Drawing.Point(200, 33);
            this.gbListas.Name = "gbListas";
            this.gbListas.Size = new System.Drawing.Size(825, 393);
            this.gbListas.TabIndex = 3;
            this.gbListas.TabStop = false;
            this.gbListas.Text = "Datos de Series asociadas al Punto";
            // 
            // lblDatosDiarios
            // 
            this.lblDatosDiarios.AutoSize = true;
            this.lblDatosDiarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatosDiarios.Location = new System.Drawing.Point(533, 17);
            this.lblDatosDiarios.Name = "lblDatosDiarios";
            this.lblDatosDiarios.Size = new System.Drawing.Size(251, 13);
            this.lblDatosDiarios.TabIndex = 44;
            this.lblDatosDiarios.Text = "                                Datos diarios                             ";
            this.lblDatosDiarios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDatosMensuales
            // 
            this.lblDatosMensuales.AutoSize = true;
            this.lblDatosMensuales.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatosMensuales.Location = new System.Drawing.Point(277, 17);
            this.lblDatosMensuales.Name = "lblDatosMensuales";
            this.lblDatosMensuales.Size = new System.Drawing.Size(238, 13);
            this.lblDatosMensuales.TabIndex = 43;
            this.lblDatosMensuales.Text = "                         Datos mensuales                         ";
            this.lblDatosMensuales.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblAñosAltDiarioUSO
            // 
            this.lblAñosAltDiarioUSO.AutoSize = true;
            this.lblAñosAltDiarioUSO.Location = new System.Drawing.Point(692, 327);
            this.lblAñosAltDiarioUSO.Name = "lblAñosAltDiarioUSO";
            this.lblAñosAltDiarioUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltDiarioUSO.TabIndex = 42;
            this.lblAñosAltDiarioUSO.Text = "Label3";
            // 
            // lblAñosNatDiarioUSO
            // 
            this.lblAñosNatDiarioUSO.AutoSize = true;
            this.lblAñosNatDiarioUSO.Location = new System.Drawing.Point(589, 327);
            this.lblAñosNatDiarioUSO.Name = "lblAñosNatDiarioUSO";
            this.lblAñosNatDiarioUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatDiarioUSO.TabIndex = 41;
            this.lblAñosNatDiarioUSO.Text = "Label3";
            // 
            // lblAñosAltMensualUSO
            // 
            this.lblAñosAltMensualUSO.AutoSize = true;
            this.lblAñosAltMensualUSO.Location = new System.Drawing.Point(429, 327);
            this.lblAñosAltMensualUSO.Name = "lblAñosAltMensualUSO";
            this.lblAñosAltMensualUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltMensualUSO.TabIndex = 40;
            this.lblAñosAltMensualUSO.Text = "Label3";
            // 
            // lblAñosNatMensualUSO
            // 
            this.lblAñosNatMensualUSO.AutoSize = true;
            this.lblAñosNatMensualUSO.Location = new System.Drawing.Point(324, 327);
            this.lblAñosNatMensualUSO.Name = "lblAñosNatMensualUSO";
            this.lblAñosNatMensualUSO.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatMensualUSO.TabIndex = 39;
            this.lblAñosNatMensualUSO.Text = "Label3";
            // 
            // lblAñosCoeDiaria
            // 
            this.lblAñosCoeDiaria.AutoSize = true;
            this.lblAñosCoeDiaria.Location = new System.Drawing.Point(745, 327);
            this.lblAñosCoeDiaria.Name = "lblAñosCoeDiaria";
            this.lblAñosCoeDiaria.Size = new System.Drawing.Size(39, 13);
            this.lblAñosCoeDiaria.TabIndex = 38;
            this.lblAñosCoeDiaria.Text = "Label3";
            // 
            // lblAñosNatMensual
            // 
            this.lblAñosNatMensual.AutoSize = true;
            this.lblAñosNatMensual.Location = new System.Drawing.Point(273, 327);
            this.lblAñosNatMensual.Name = "lblAñosNatMensual";
            this.lblAñosNatMensual.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatMensual.TabIndex = 22;
            this.lblAñosNatMensual.Text = "Label5";
            // 
            // _chkboxUsarCoeDiaria
            // 
            this._chkboxUsarCoeDiaria.AutoSize = true;
            this._chkboxUsarCoeDiaria.Enabled = false;
            this._chkboxUsarCoeDiaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._chkboxUsarCoeDiaria.Location = new System.Drawing.Point(542, 359);
            this._chkboxUsarCoeDiaria.Name = "_chkboxUsarCoeDiaria";
            this._chkboxUsarCoeDiaria.Size = new System.Drawing.Size(229, 17);
            this._chkboxUsarCoeDiaria.TabIndex = 37;
            this._chkboxUsarCoeDiaria.Text = "Usar coetaneidad para Avenidas y Sequías";
            this._chkboxUsarCoeDiaria.UseVisualStyleBackColor = true;
            this._chkboxUsarCoeDiaria.CheckedChanged += new System.EventHandler(this.chkboxUsarCoeDiaria_CheckedChanged);
            // 
            // grpboxLeyenda
            // 
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda6);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda5);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda4);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda2);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda3);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda7);
            this.grpboxLeyenda.Controls.Add(this.lblLeyenda1);
            this.grpboxLeyenda.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpboxLeyenda.Location = new System.Drawing.Point(9, 272);
            this.grpboxLeyenda.Name = "grpboxLeyenda";
            this.grpboxLeyenda.Size = new System.Drawing.Size(140, 112);
            this.grpboxLeyenda.TabIndex = 36;
            this.grpboxLeyenda.TabStop = false;
            this.grpboxLeyenda.Text = "Leyenda";
            // 
            // lblLeyenda6
            // 
            this.lblLeyenda6.AutoSize = true;
            this.lblLeyenda6.Location = new System.Drawing.Point(9, 82);
            this.lblLeyenda6.Name = "lblLeyenda6";
            this.lblLeyenda6.Size = new System.Drawing.Size(100, 12);
            this.lblLeyenda6.TabIndex = 6;
            this.lblLeyenda6.Text = "NCO: Año no coetáneo";
            // 
            // lblLeyenda5
            // 
            this.lblLeyenda5.AutoSize = true;
            this.lblLeyenda5.Location = new System.Drawing.Point(9, 69);
            this.lblLeyenda5.Name = "lblLeyenda5";
            this.lblLeyenda5.Size = new System.Drawing.Size(81, 12);
            this.lblLeyenda5.TabIndex = 5;
            this.lblLeyenda5.Text = "CO: Año coetáneo";
            // 
            // lblLeyenda4
            // 
            this.lblLeyenda4.AutoSize = true;
            this.lblLeyenda4.Location = new System.Drawing.Point(9, 56);
            this.lblLeyenda4.Name = "lblLeyenda4";
            this.lblLeyenda4.Size = new System.Drawing.Size(79, 12);
            this.lblLeyenda4.TabIndex = 4;
            this.lblLeyenda4.Text = "SD: Año sin datos";
            // 
            // lblLeyenda2
            // 
            this.lblLeyenda2.AutoSize = true;
            this.lblLeyenda2.Location = new System.Drawing.Point(9, 29);
            this.lblLeyenda2.Name = "lblLeyenda2";
            this.lblLeyenda2.Size = new System.Drawing.Size(78, 12);
            this.lblLeyenda2.TabIndex = 3;
            this.lblLeyenda2.Text = "I: Año Incompleto";
            // 
            // lblLeyenda3
            // 
            this.lblLeyenda3.AutoSize = true;
            this.lblLeyenda3.Location = new System.Drawing.Point(9, 43);
            this.lblLeyenda3.Name = "lblLeyenda3";
            this.lblLeyenda3.Size = new System.Drawing.Size(111, 12);
            this.lblLeyenda3.TabIndex = 2;
            this.lblLeyenda3.Text = "CD: Calculado con diarios";
            // 
            // lblLeyenda7
            // 
            this.lblLeyenda7.AutoSize = true;
            this.lblLeyenda7.Location = new System.Drawing.Point(9, 96);
            this.lblLeyenda7.Name = "lblLeyenda7";
            this.lblLeyenda7.Size = new System.Drawing.Size(93, 12);
            this.lblLeyenda7.TabIndex = 1;
            this.lblLeyenda7.Text = "NS: Sin serie cargada";
            // 
            // lblLeyenda1
            // 
            this.lblLeyenda1.AutoSize = true;
            this.lblLeyenda1.Location = new System.Drawing.Point(9, 16);
            this.lblLeyenda1.Name = "lblLeyenda1";
            this.lblLeyenda1.Size = new System.Drawing.Size(76, 12);
            this.lblLeyenda1.TabIndex = 0;
            this.lblLeyenda1.Text = "C: Año Completo";
            // 
            // lblSerieNatMensual
            // 
            this.lblSerieNatMensual.AutoSize = true;
            this.lblSerieNatMensual.BackColor = System.Drawing.Color.White;
            this.lblSerieNatMensual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSerieNatMensual.Location = new System.Drawing.Point(90, 75);
            this.lblSerieNatMensual.Name = "lblSerieNatMensual";
            this.lblSerieNatMensual.Size = new System.Drawing.Size(18, 15);
            this.lblSerieNatMensual.TabIndex = 35;
            this.lblSerieNatMensual.Text = "---";
            // 
            // _chkboxUsarCoe
            // 
            this._chkboxUsarCoe.AutoSize = true;
            this._chkboxUsarCoe.Enabled = false;
            this._chkboxUsarCoe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._chkboxUsarCoe.Location = new System.Drawing.Point(293, 359);
            this._chkboxUsarCoe.Name = "_chkboxUsarCoe";
            this._chkboxUsarCoe.Size = new System.Drawing.Size(222, 17);
            this._chkboxUsarCoe.TabIndex = 10;
            this._chkboxUsarCoe.Text = "Usar coetaneidad para Valores Habituales";
            this._chkboxUsarCoe.UseVisualStyleBackColor = true;
            this._chkboxUsarCoe.CheckedChanged += new System.EventHandler(this.chkboxUsarCoe_CheckedChanged);
            // 
            // lblSerieNatDiaria
            // 
            this.lblSerieNatDiaria.AutoSize = true;
            this.lblSerieNatDiaria.BackColor = System.Drawing.Color.White;
            this.lblSerieNatDiaria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSerieNatDiaria.Location = new System.Drawing.Point(90, 50);
            this.lblSerieNatDiaria.Name = "lblSerieNatDiaria";
            this.lblSerieNatDiaria.Size = new System.Drawing.Size(18, 15);
            this.lblSerieNatDiaria.TabIndex = 34;
            this.lblSerieNatDiaria.Text = "---";
            // 
            // lblAltNombreEstatico
            // 
            this.lblAltNombreEstatico.AutoSize = true;
            this.lblAltNombreEstatico.Location = new System.Drawing.Point(6, 148);
            this.lblAltNombreEstatico.Name = "lblAltNombreEstatico";
            this.lblAltNombreEstatico.Size = new System.Drawing.Size(46, 13);
            this.lblAltNombreEstatico.TabIndex = 33;
            this.lblAltNombreEstatico.Text = "Descrip.";
            // 
            // lblCodigoAlt
            // 
            this.lblCodigoAlt.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCodigoAlt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCodigoAlt.Location = new System.Drawing.Point(53, 147);
            this.lblCodigoAlt.Name = "lblCodigoAlt";
            this.lblCodigoAlt.Size = new System.Drawing.Size(141, 21);
            this.lblCodigoAlt.TabIndex = 32;
            this.lblCodigoAlt.Text = "---";
            this.lblCodigoAlt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIDMensual
            // 
            this.lblIDMensual.AutoSize = true;
            this.lblIDMensual.BackColor = System.Drawing.Color.White;
            this.lblIDMensual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblIDMensual.Location = new System.Drawing.Point(86, 197);
            this.lblIDMensual.Name = "lblIDMensual";
            this.lblIDMensual.Size = new System.Drawing.Size(18, 15);
            this.lblIDMensual.TabIndex = 31;
            this.lblIDMensual.Text = "---";
            // 
            // lblIDDiaria
            // 
            this.lblIDDiaria.AutoSize = true;
            this.lblIDDiaria.BackColor = System.Drawing.Color.White;
            this.lblIDDiaria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblIDDiaria.Location = new System.Drawing.Point(86, 171);
            this.lblIDDiaria.Name = "lblIDDiaria";
            this.lblIDDiaria.Size = new System.Drawing.Size(18, 15);
            this.lblIDDiaria.TabIndex = 30;
            this.lblIDDiaria.Text = "---";
            // 
            // lblASerieMensual
            // 
            this.lblASerieMensual.AutoSize = true;
            this.lblASerieMensual.Location = new System.Drawing.Point(6, 197);
            this.lblASerieMensual.Name = "lblASerieMensual";
            this.lblASerieMensual.Size = new System.Drawing.Size(74, 13);
            this.lblASerieMensual.TabIndex = 29;
            this.lblASerieMensual.Text = "Serie Mensual";
            // 
            // lblASerieDiaria
            // 
            this.lblASerieDiaria.AutoSize = true;
            this.lblASerieDiaria.Location = new System.Drawing.Point(6, 171);
            this.lblASerieDiaria.Name = "lblASerieDiaria";
            this.lblASerieDiaria.Size = new System.Drawing.Size(61, 13);
            this.lblASerieDiaria.TabIndex = 28;
            this.lblASerieDiaria.Text = "Serie Diaria";
            // 
            // lblADiario
            // 
            this.lblADiario.AutoSize = true;
            this.lblADiario.Location = new System.Drawing.Point(6, 128);
            this.lblADiario.Name = "lblADiario";
            this.lblADiario.Size = new System.Drawing.Size(40, 13);
            this.lblADiario.TabIndex = 27;
            this.lblADiario.Text = "Código";
            // 
            // lblMensual
            // 
            this.lblMensual.AutoSize = true;
            this.lblMensual.Location = new System.Drawing.Point(6, 75);
            this.lblMensual.Name = "lblMensual";
            this.lblMensual.Size = new System.Drawing.Size(74, 13);
            this.lblMensual.TabIndex = 26;
            this.lblMensual.Text = "Serie Mensual";
            // 
            // lblDiario
            // 
            this.lblDiario.AutoSize = true;
            this.lblDiario.Location = new System.Drawing.Point(6, 50);
            this.lblDiario.Name = "lblDiario";
            this.lblDiario.Size = new System.Drawing.Size(61, 13);
            this.lblDiario.TabIndex = 25;
            this.lblDiario.Text = "Serie Diaria";
            // 
            // lblAñosCoeMensual
            // 
            this.lblAñosCoeMensual.AutoSize = true;
            this.lblAñosCoeMensual.Location = new System.Drawing.Point(482, 327);
            this.lblAñosCoeMensual.Name = "lblAñosCoeMensual";
            this.lblAñosCoeMensual.Size = new System.Drawing.Size(39, 13);
            this.lblAñosCoeMensual.TabIndex = 24;
            this.lblAñosCoeMensual.Text = "Label7";
            // 
            // lblAñosAltMensual
            // 
            this.lblAñosAltMensual.AutoSize = true;
            this.lblAñosAltMensual.Location = new System.Drawing.Point(376, 327);
            this.lblAñosAltMensual.Name = "lblAñosAltMensual";
            this.lblAñosAltMensual.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltMensual.TabIndex = 23;
            this.lblAñosAltMensual.Text = "Label6";
            // 
            // lblAñosAltDiario
            // 
            this.lblAñosAltDiario.AutoSize = true;
            this.lblAñosAltDiario.Location = new System.Drawing.Point(638, 327);
            this.lblAñosAltDiario.Name = "lblAñosAltDiario";
            this.lblAñosAltDiario.Size = new System.Drawing.Size(39, 13);
            this.lblAñosAltDiario.TabIndex = 20;
            this.lblAñosAltDiario.Text = "Label3";
            // 
            // lblAñosNatDiario
            // 
            this.lblAñosNatDiario.AutoSize = true;
            this.lblAñosNatDiario.Location = new System.Drawing.Point(535, 327);
            this.lblAñosNatDiario.Name = "lblAñosNatDiario";
            this.lblAñosNatDiario.Size = new System.Drawing.Size(39, 13);
            this.lblAñosNatDiario.TabIndex = 19;
            this.lblAñosNatDiario.Text = "Label2";
            // 
            // lblAñosHidro
            // 
            this.lblAñosHidro.AutoSize = true;
            this.lblAñosHidro.Location = new System.Drawing.Point(224, 327);
            this.lblAñosHidro.Name = "lblAñosHidro";
            this.lblAñosHidro.Size = new System.Drawing.Size(39, 13);
            this.lblAñosHidro.TabIndex = 18;
            this.lblAñosHidro.Text = "Label1";
            // 
            // lblTotalAños
            // 
            this.lblTotalAños.AutoSize = true;
            this.lblTotalAños.Location = new System.Drawing.Point(155, 327);
            this.lblTotalAños.Name = "lblTotalAños";
            this.lblTotalAños.Size = new System.Drawing.Size(45, 13);
            this.lblTotalAños.TabIndex = 17;
            this.lblTotalAños.Text = "TOTAL:";
            // 
            // _cmbListaAlteradasDiarias
            // 
            this._cmbListaAlteradasDiarias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbListaAlteradasDiarias.FormattingEnabled = true;
            this._cmbListaAlteradasDiarias.Location = new System.Drawing.Point(53, 125);
            this._cmbListaAlteradasDiarias.Name = "_cmbListaAlteradasDiarias";
            this._cmbListaAlteradasDiarias.Size = new System.Drawing.Size(141, 21);
            this._cmbListaAlteradasDiarias.TabIndex = 13;
            this._cmbListaAlteradasDiarias.SelectedIndexChanged += new System.EventHandler(this.cmbListaAlteradasDiarias_SelectedIndexChanged);
            // 
            // lblListasAlteradas
            // 
            this.lblListasAlteradas.AutoSize = true;
            this.lblListasAlteradas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListasAlteradas.Location = new System.Drawing.Point(6, 107);
            this.lblListasAlteradas.Name = "lblListasAlteradas";
            this.lblListasAlteradas.Size = new System.Drawing.Size(160, 15);
            this.lblListasAlteradas.TabIndex = 12;
            this.lblListasAlteradas.Text = "Series de Valores Alterados:";
            // 
            // lblListasNaturales
            // 
            this.lblListasNaturales.AutoSize = true;
            this.lblListasNaturales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListasNaturales.Location = new System.Drawing.Point(6, 31);
            this.lblListasNaturales.Name = "lblListasNaturales";
            this.lblListasNaturales.Size = new System.Drawing.Size(162, 15);
            this.lblListasNaturales.TabIndex = 11;
            this.lblListasNaturales.Text = "Series de Valores Naturales:";
            // 
            // XPTablaListas
            // 
            this.XPTablaListas.ColumnModel = this.ColumnModel1;
            this.XPTablaListas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XPTablaListas.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XPTablaListas.Location = new System.Drawing.Point(200, 37);
            this.XPTablaListas.Name = "XPTablaListas";
            this.XPTablaListas.NoItemsText = "Seleccione un punto de la lista para analizar sus series asociadas.";
            this.XPTablaListas.Size = new System.Drawing.Size(613, 277);
            this.XPTablaListas.TabIndex = 9;
            this.XPTablaListas.TableModel = this.TableModel1;
            this.XPTablaListas.Text = "XPTablaListas";
            // 
            // ColumnModel1
            // 
            this.ColumnModel1.HeaderHeight = 40;
            // 
            // _btnCalcular
            // 
            this._btnCalcular.Location = new System.Drawing.Point(847, 19);
            this._btnCalcular.Name = "_btnCalcular";
            this._btnCalcular.Size = new System.Drawing.Size(154, 171);
            this._btnCalcular.TabIndex = 4;
            this._btnCalcular.Text = "Calcular";
            this._btnCalcular.UseVisualStyleBackColor = true;
            this._btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // gbInforme
            // 
            this.gbInforme.Controls.Add(this.lstBoxInformes);
            this.gbInforme.Controls.Add(this._btnCalcular);
            this.gbInforme.Location = new System.Drawing.Point(12, 432);
            this.gbInforme.Name = "gbInforme";
            this.gbInforme.Size = new System.Drawing.Size(1013, 197);
            this.gbInforme.TabIndex = 10;
            this.gbInforme.TabStop = false;
            this.gbInforme.Text = "Informes a realizar:";
            // 
            // lstBoxInformes
            // 
            this.lstBoxInformes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBoxInformes.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstBoxInformes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstBoxInformes.ColumnWidth = 400;
            this.lstBoxInformes.FormattingEnabled = true;
            this.lstBoxInformes.Location = new System.Drawing.Point(12, 19);
            this.lstBoxInformes.MultiColumn = true;
            this.lstBoxInformes.Name = "lstBoxInformes";
            this.lstBoxInformes.Size = new System.Drawing.Size(829, 171);
            this.lstBoxInformes.TabIndex = 5;
            // 
            // gbProyecto
            // 
            this.gbProyecto.Controls.Add(this.lblDescripcion);
            this.gbProyecto.Controls.Add(this.lblProyectoDesc);
            this.gbProyecto.Controls.Add(this._cbProyectos);
            this.gbProyecto.Location = new System.Drawing.Point(12, 33);
            this.gbProyecto.Name = "gbProyecto";
            this.gbProyecto.Size = new System.Drawing.Size(181, 90);
            this.gbProyecto.TabIndex = 11;
            this.gbProyecto.TabStop = false;
            this.gbProyecto.Text = "Proyecto";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(9, 37);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(63, 13);
            this.lblDescripcion.TabIndex = 47;
            this.lblDescripcion.Text = "Descripción";
            // 
            // lblProyectoDesc
            // 
            this.lblProyectoDesc.BackColor = System.Drawing.Color.White;
            this.lblProyectoDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProyectoDesc.Location = new System.Drawing.Point(12, 50);
            this.lblProyectoDesc.Name = "lblProyectoDesc";
            this.lblProyectoDesc.Size = new System.Drawing.Size(163, 33);
            this.lblProyectoDesc.TabIndex = 46;
            // 
            // _cbProyectos
            // 
            this._cbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbProyectos.FormattingEnabled = true;
            this._cbProyectos.Location = new System.Drawing.Point(12, 14);
            this._cbProyectos.Name = "_cbProyectos";
            this._cbProyectos.Size = new System.Drawing.Size(160, 21);
            this._cbProyectos.TabIndex = 0;
            this._cbProyectos.SelectedIndexChanged += new System.EventHandler(this.cbProyectos_SelectedIndexChanged);
            // 
            // FormInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1036, 635);
            this.Controls.Add(this.gbProyecto);
            this.Controls.Add(this.gbListas);
            this.Controls.Add(this.gbInforme);
            this.Controls.Add(this.gbPuntos);
            this.Controls.Add(this.MenuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MenuStrip1;
            this.MaximizeBox = false;
            this.Name = "FormInicial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IAHRIS (Índices de Alteración hidrológica en RIoS) ";
            this.Activated += new System.EventHandler(this.FormInicial_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInicial_FormClosing);
            this.Load += new System.EventHandler(this.FormInicial_Load);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.gbPuntos.ResumeLayout(false);
            this.gbPuntos.PerformLayout();
            this.gbListas.ResumeLayout(false);
            this.gbListas.PerformLayout();
            this.grpboxLeyenda.ResumeLayout(false);
            this.grpboxLeyenda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XPTablaListas)).EndInit();
            this.gbInforme.ResumeLayout(false);
            this.gbProyecto.ResumeLayout(false);
            this.gbProyecto.PerformLayout();
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

        internal GroupBox gbPuntos;
        internal Label lblPuntoNListas;
        internal Label lblPuntoClave;
        internal Label lblPuntoNombre;
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

        internal GroupBox gbListas;
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

        internal Label lblListasAlteradas;
        internal Label lblListasNaturales;
        internal Label lblAñosCoeMensual;
        internal Label lblAñosAltMensual;
        internal Label lblAñosNatMensual;
        internal Label lblAñosAltDiario;
        internal Label lblAñosNatDiario;
        internal Label lblAñosHidro;
        internal Label lblTotalAños;
        internal Label lblADiario;
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
        internal Label lblPuntosNListas;
        internal Label lblPuntosNombre;
        internal Label lblPuntosClave;
        internal Label lblLeyenda3;
        internal Label lblLeyenda7;
        internal Label lblLeyenda1;
        internal Label lblLeyenda2;
        internal Label lblLeyenda4;
        internal Label lblLeyenda5;
        internal Label lblLeyenda6;
        internal ToolStripMenuItem IdiomasToolStripMenuItem;

        //internal ToolStripMenuItem ManualesToolStripMenuItem
        //{
        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    get
        //    {
        //        return _ManualesToolStripMenuItem;
        //    }

        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    set
        //    {
        //        if (_ManualesToolStripMenuItem != null)
        //        {
        //            _ManualesToolStripMenuItem.Click -= ManualesToolStripMenuItem_Click;
        //        }

        //        _ManualesToolStripMenuItem = value;
        //        if (_ManualesToolStripMenuItem != null)
        //        {
        //            _ManualesToolStripMenuItem.Click += ManualesToolStripMenuItem_Click;
        //        }
        //    }
        //}

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

        private ComboBox _cmbMesInicio;

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

        internal Label lblMesInicio;
    }
}