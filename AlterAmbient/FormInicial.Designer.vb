<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormInicial
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormInicial))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.Gesti�nDeProyectosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.A�adirProyectoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminarProyectoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Gesti�nDePuntosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.A�adirPuntoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminarPuntoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Gesti�nDeAlteracionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.A�adirAlteraci�nToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminarAlteraci�nToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Gesti�nDeListasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.A�adirListaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EliminarListaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GestBBDDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ImportarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExportarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ManualesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.IdiomasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lstboxPuntos = New System.Windows.Forms.ListBox
        Me.gbPuntos = New System.Windows.Forms.GroupBox
        Me.cmbMesInicio = New System.Windows.Forms.ComboBox
        Me.lblMesInicio = New System.Windows.Forms.Label
        Me.lblPuntosNListas = New System.Windows.Forms.Label
        Me.lblPuntosNombre = New System.Windows.Forms.Label
        Me.lblPuntosClave = New System.Windows.Forms.Label
        Me.lblPuntoNListas = New System.Windows.Forms.Label
        Me.lblPuntoClave = New System.Windows.Forms.Label
        Me.lblPuntoNombre = New System.Windows.Forms.Label
        Me.gbListas = New System.Windows.Forms.GroupBox
        Me.lblDatosDiarios = New System.Windows.Forms.Label
        Me.lblDatosMensuales = New System.Windows.Forms.Label
        Me.lblA�osAltDiarioUSO = New System.Windows.Forms.Label
        Me.lblA�osNatDiarioUSO = New System.Windows.Forms.Label
        Me.lblA�osAltMensualUSO = New System.Windows.Forms.Label
        Me.lblA�osNatMensualUSO = New System.Windows.Forms.Label
        Me.lblA�osCoeDiaria = New System.Windows.Forms.Label
        Me.lblA�osNatMensual = New System.Windows.Forms.Label
        Me.chkboxUsarCoeDiaria = New System.Windows.Forms.CheckBox
        Me.grpboxLeyenda = New System.Windows.Forms.GroupBox
        Me.lblLeyenda6 = New System.Windows.Forms.Label
        Me.lblLeyenda5 = New System.Windows.Forms.Label
        Me.lblLeyenda4 = New System.Windows.Forms.Label
        Me.lblLeyenda2 = New System.Windows.Forms.Label
        Me.lblLeyenda3 = New System.Windows.Forms.Label
        Me.lblLeyenda7 = New System.Windows.Forms.Label
        Me.lblLeyenda1 = New System.Windows.Forms.Label
        Me.lblSerieNatMensual = New System.Windows.Forms.Label
        Me.chkboxUsarCoe = New System.Windows.Forms.CheckBox
        Me.lblSerieNatDiaria = New System.Windows.Forms.Label
        Me.lblAltNombreEstatico = New System.Windows.Forms.Label
        Me.lblCodigoAlt = New System.Windows.Forms.Label
        Me.lblIDMensual = New System.Windows.Forms.Label
        Me.lblIDDiaria = New System.Windows.Forms.Label
        Me.lblASerieMensual = New System.Windows.Forms.Label
        Me.lblASerieDiaria = New System.Windows.Forms.Label
        Me.lblADiario = New System.Windows.Forms.Label
        Me.lblMensual = New System.Windows.Forms.Label
        Me.lblDiario = New System.Windows.Forms.Label
        Me.lblA�osCoeMensual = New System.Windows.Forms.Label
        Me.lblA�osAltMensual = New System.Windows.Forms.Label
        Me.lblA�osAltDiario = New System.Windows.Forms.Label
        Me.lblA�osNatDiario = New System.Windows.Forms.Label
        Me.lblA�osHidro = New System.Windows.Forms.Label
        Me.lblTotalA�os = New System.Windows.Forms.Label
        Me.cmbListaAlteradasDiarias = New System.Windows.Forms.ComboBox
        Me.lblListasAlteradas = New System.Windows.Forms.Label
        Me.lblListasNaturales = New System.Windows.Forms.Label
        Me.XPTablaListas = New XPTable.Models.Table
        Me.ColumnModel1 = New XPTable.Models.ColumnModel
        Me.TableModel1 = New XPTable.Models.TableModel
        Me.btnCalcular = New System.Windows.Forms.Button
        Me.gbInforme = New System.Windows.Forms.GroupBox
        Me.lstBoxInformes = New System.Windows.Forms.ListBox
        Me.gbProyecto = New System.Windows.Forms.GroupBox
        Me.lblDescripcion = New System.Windows.Forms.Label
        Me.lblProyectoDesc = New System.Windows.Forms.Label
        Me.cbProyectos = New System.Windows.Forms.ComboBox
        Me.MenuStrip1.SuspendLayout()
        Me.gbPuntos.SuspendLayout()
        Me.gbListas.SuspendLayout()
        Me.grpboxLeyenda.SuspendLayout()
        CType(Me.XPTablaListas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbInforme.SuspendLayout()
        Me.gbProyecto.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Gesti�nDeProyectosToolStripMenuItem, Me.Gesti�nDePuntosToolStripMenuItem, Me.Gesti�nDeAlteracionesToolStripMenuItem, Me.Gesti�nDeListasToolStripMenuItem, Me.GestBBDDToolStripMenuItem, Me.ManualesToolStripMenuItem, Me.IdiomasToolStripMenuItem})
        Me.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1036, 21)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'Gesti�nDeProyectosToolStripMenuItem
        '
        Me.Gesti�nDeProyectosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.A�adirProyectoToolStripMenuItem, Me.EliminarProyectoToolStripMenuItem})
        Me.Gesti�nDeProyectosToolStripMenuItem.Name = "Gesti�nDeProyectosToolStripMenuItem"
        Me.Gesti�nDeProyectosToolStripMenuItem.Size = New System.Drawing.Size(121, 17)
        Me.Gesti�nDeProyectosToolStripMenuItem.Text = "Gesti�n de proyectos"
        '
        'A�adirProyectoToolStripMenuItem
        '
        Me.A�adirProyectoToolStripMenuItem.Name = "A�adirProyectoToolStripMenuItem"
        Me.A�adirProyectoToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.A�adirProyectoToolStripMenuItem.Text = "A�adir proyecto"
        '
        'EliminarProyectoToolStripMenuItem
        '
        Me.EliminarProyectoToolStripMenuItem.Name = "EliminarProyectoToolStripMenuItem"
        Me.EliminarProyectoToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.EliminarProyectoToolStripMenuItem.Text = "Eliminar proyecto"
        '
        'Gesti�nDePuntosToolStripMenuItem
        '
        Me.Gesti�nDePuntosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.A�adirPuntoToolStripMenuItem, Me.EliminarPuntoToolStripMenuItem})
        Me.Gesti�nDePuntosToolStripMenuItem.Name = "Gesti�nDePuntosToolStripMenuItem"
        Me.Gesti�nDePuntosToolStripMenuItem.Size = New System.Drawing.Size(106, 17)
        Me.Gesti�nDePuntosToolStripMenuItem.Text = "Gesti�n de Puntos"
        '
        'A�adirPuntoToolStripMenuItem
        '
        Me.A�adirPuntoToolStripMenuItem.Name = "A�adirPuntoToolStripMenuItem"
        Me.A�adirPuntoToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.A�adirPuntoToolStripMenuItem.Text = "A�adir Punto"
        '
        'EliminarPuntoToolStripMenuItem
        '
        Me.EliminarPuntoToolStripMenuItem.Name = "EliminarPuntoToolStripMenuItem"
        Me.EliminarPuntoToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EliminarPuntoToolStripMenuItem.Text = "Eliminar Punto"
        '
        'Gesti�nDeAlteracionesToolStripMenuItem
        '
        Me.Gesti�nDeAlteracionesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.A�adirAlteraci�nToolStripMenuItem, Me.EliminarAlteraci�nToolStripMenuItem})
        Me.Gesti�nDeAlteracionesToolStripMenuItem.Name = "Gesti�nDeAlteracionesToolStripMenuItem"
        Me.Gesti�nDeAlteracionesToolStripMenuItem.Size = New System.Drawing.Size(132, 17)
        Me.Gesti�nDeAlteracionesToolStripMenuItem.Text = "Gesti�n de Alteraciones"
        '
        'A�adirAlteraci�nToolStripMenuItem
        '
        Me.A�adirAlteraci�nToolStripMenuItem.Name = "A�adirAlteraci�nToolStripMenuItem"
        Me.A�adirAlteraci�nToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.A�adirAlteraci�nToolStripMenuItem.Text = "A�adir Alteraci�n"
        '
        'EliminarAlteraci�nToolStripMenuItem
        '
        Me.EliminarAlteraci�nToolStripMenuItem.Name = "EliminarAlteraci�nToolStripMenuItem"
        Me.EliminarAlteraci�nToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.EliminarAlteraci�nToolStripMenuItem.Text = "Eliminar Alteraci�n"
        '
        'Gesti�nDeListasToolStripMenuItem
        '
        Me.Gesti�nDeListasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.A�adirListaToolStripMenuItem, Me.EliminarListaToolStripMenuItem})
        Me.Gesti�nDeListasToolStripMenuItem.Name = "Gesti�nDeListasToolStripMenuItem"
        Me.Gesti�nDeListasToolStripMenuItem.Size = New System.Drawing.Size(102, 17)
        Me.Gesti�nDeListasToolStripMenuItem.Text = "Gesti�n de Series"
        '
        'A�adirListaToolStripMenuItem
        '
        Me.A�adirListaToolStripMenuItem.Name = "A�adirListaToolStripMenuItem"
        Me.A�adirListaToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.A�adirListaToolStripMenuItem.Text = "A�adir Serie"
        '
        'EliminarListaToolStripMenuItem
        '
        Me.EliminarListaToolStripMenuItem.Name = "EliminarListaToolStripMenuItem"
        Me.EliminarListaToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.EliminarListaToolStripMenuItem.Text = "Eliminar Serie"
        '
        'GestBBDDToolStripMenuItem
        '
        Me.GestBBDDToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImportarToolStripMenuItem, Me.ExportarToolStripMenuItem})
        Me.GestBBDDToolStripMenuItem.Name = "GestBBDDToolStripMenuItem"
        Me.GestBBDDToolStripMenuItem.Size = New System.Drawing.Size(126, 17)
        Me.GestBBDDToolStripMenuItem.Text = "Gesti�n base de datos"
        '
        'ImportarToolStripMenuItem
        '
        Me.ImportarToolStripMenuItem.Name = "ImportarToolStripMenuItem"
        Me.ImportarToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.ImportarToolStripMenuItem.Text = "Importar"
        '
        'ExportarToolStripMenuItem
        '
        Me.ExportarToolStripMenuItem.Name = "ExportarToolStripMenuItem"
        Me.ExportarToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.ExportarToolStripMenuItem.Text = "Exportar"
        '
        'ManualesToolStripMenuItem
        '
        Me.ManualesToolStripMenuItem.Name = "ManualesToolStripMenuItem"
        Me.ManualesToolStripMenuItem.Size = New System.Drawing.Size(64, 17)
        Me.ManualesToolStripMenuItem.Text = "Manuales"
        '
        'IdiomasToolStripMenuItem
        '
        Me.IdiomasToolStripMenuItem.Name = "IdiomasToolStripMenuItem"
        Me.IdiomasToolStripMenuItem.Size = New System.Drawing.Size(102, 17)
        Me.IdiomasToolStripMenuItem.Text = "Idioma/Language"
        '
        'lstboxPuntos
        '
        Me.lstboxPuntos.FormattingEnabled = True
        Me.lstboxPuntos.Location = New System.Drawing.Point(12, 19)
        Me.lstboxPuntos.Name = "lstboxPuntos"
        Me.lstboxPuntos.ScrollAlwaysVisible = True
        Me.lstboxPuntos.Size = New System.Drawing.Size(160, 121)
        Me.lstboxPuntos.Sorted = True
        Me.lstboxPuntos.TabIndex = 1
        Me.lstboxPuntos.TabStop = False
        '
        'gbPuntos
        '
        Me.gbPuntos.Controls.Add(Me.cmbMesInicio)
        Me.gbPuntos.Controls.Add(Me.lblMesInicio)
        Me.gbPuntos.Controls.Add(Me.lblPuntosNListas)
        Me.gbPuntos.Controls.Add(Me.lblPuntosNombre)
        Me.gbPuntos.Controls.Add(Me.lblPuntosClave)
        Me.gbPuntos.Controls.Add(Me.lstboxPuntos)
        Me.gbPuntos.Controls.Add(Me.lblPuntoNListas)
        Me.gbPuntos.Controls.Add(Me.lblPuntoClave)
        Me.gbPuntos.Controls.Add(Me.lblPuntoNombre)
        Me.gbPuntos.Location = New System.Drawing.Point(12, 122)
        Me.gbPuntos.Name = "gbPuntos"
        Me.gbPuntos.Size = New System.Drawing.Size(182, 304)
        Me.gbPuntos.TabIndex = 2
        Me.gbPuntos.TabStop = False
        Me.gbPuntos.Text = "Datos del Punto"
        '
        'cmbMesInicio
        '
        Me.cmbMesInicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMesInicio.FormattingEnabled = True
        Me.cmbMesInicio.Location = New System.Drawing.Point(12, 273)
        Me.cmbMesInicio.Name = "cmbMesInicio"
        Me.cmbMesInicio.Size = New System.Drawing.Size(160, 21)
        Me.cmbMesInicio.TabIndex = 49
        '
        'lblMesInicio
        '
        Me.lblMesInicio.AutoSize = True
        Me.lblMesInicio.Location = New System.Drawing.Point(9, 257)
        Me.lblMesInicio.Name = "lblMesInicio"
        Me.lblMesInicio.Size = New System.Drawing.Size(165, 13)
        Me.lblMesInicio.TabIndex = 48
        Me.lblMesInicio.Text = "Mes inicio a�o hidro. para c�lculo"
        '
        'lblPuntosNListas
        '
        Me.lblPuntosNListas.BackColor = System.Drawing.Color.White
        Me.lblPuntosNListas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPuntosNListas.Location = New System.Drawing.Point(12, 234)
        Me.lblPuntosNListas.Name = "lblPuntosNListas"
        Me.lblPuntosNListas.Size = New System.Drawing.Size(160, 18)
        Me.lblPuntosNListas.TabIndex = 47
        '
        'lblPuntosNombre
        '
        Me.lblPuntosNombre.BackColor = System.Drawing.Color.White
        Me.lblPuntosNombre.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPuntosNombre.Location = New System.Drawing.Point(12, 196)
        Me.lblPuntosNombre.Name = "lblPuntosNombre"
        Me.lblPuntosNombre.Size = New System.Drawing.Size(160, 18)
        Me.lblPuntosNombre.TabIndex = 46
        '
        'lblPuntosClave
        '
        Me.lblPuntosClave.BackColor = System.Drawing.Color.White
        Me.lblPuntosClave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPuntosClave.Location = New System.Drawing.Point(12, 158)
        Me.lblPuntosClave.Name = "lblPuntosClave"
        Me.lblPuntosClave.Size = New System.Drawing.Size(160, 18)
        Me.lblPuntosClave.TabIndex = 45
        '
        'lblPuntoNListas
        '
        Me.lblPuntoNListas.AutoSize = True
        Me.lblPuntoNListas.Location = New System.Drawing.Point(9, 219)
        Me.lblPuntoNListas.Name = "lblPuntoNListas"
        Me.lblPuntoNListas.Size = New System.Drawing.Size(145, 13)
        Me.lblPuntoNListas.TabIndex = 5
        Me.lblPuntoNListas.Text = "N� de alteraciones asociadas"
        '
        'lblPuntoClave
        '
        Me.lblPuntoClave.AutoSize = True
        Me.lblPuntoClave.Location = New System.Drawing.Point(9, 143)
        Me.lblPuntoClave.Name = "lblPuntoClave"
        Me.lblPuntoClave.Size = New System.Drawing.Size(88, 13)
        Me.lblPuntoClave.TabIndex = 4
        Me.lblPuntoClave.Text = "C�digo del Punto"
        '
        'lblPuntoNombre
        '
        Me.lblPuntoNombre.AutoSize = True
        Me.lblPuntoNombre.Location = New System.Drawing.Point(9, 181)
        Me.lblPuntoNombre.Name = "lblPuntoNombre"
        Me.lblPuntoNombre.Size = New System.Drawing.Size(111, 13)
        Me.lblPuntoNombre.TabIndex = 3
        Me.lblPuntoNombre.Text = "Descripci�n del Punto"
        '
        'gbListas
        '
        Me.gbListas.Controls.Add(Me.lblDatosDiarios)
        Me.gbListas.Controls.Add(Me.lblDatosMensuales)
        Me.gbListas.Controls.Add(Me.lblA�osAltDiarioUSO)
        Me.gbListas.Controls.Add(Me.lblA�osNatDiarioUSO)
        Me.gbListas.Controls.Add(Me.lblA�osAltMensualUSO)
        Me.gbListas.Controls.Add(Me.lblA�osNatMensualUSO)
        Me.gbListas.Controls.Add(Me.lblA�osCoeDiaria)
        Me.gbListas.Controls.Add(Me.lblA�osNatMensual)
        Me.gbListas.Controls.Add(Me.chkboxUsarCoeDiaria)
        Me.gbListas.Controls.Add(Me.grpboxLeyenda)
        Me.gbListas.Controls.Add(Me.lblSerieNatMensual)
        Me.gbListas.Controls.Add(Me.chkboxUsarCoe)
        Me.gbListas.Controls.Add(Me.lblSerieNatDiaria)
        Me.gbListas.Controls.Add(Me.lblAltNombreEstatico)
        Me.gbListas.Controls.Add(Me.lblCodigoAlt)
        Me.gbListas.Controls.Add(Me.lblIDMensual)
        Me.gbListas.Controls.Add(Me.lblIDDiaria)
        Me.gbListas.Controls.Add(Me.lblASerieMensual)
        Me.gbListas.Controls.Add(Me.lblASerieDiaria)
        Me.gbListas.Controls.Add(Me.lblADiario)
        Me.gbListas.Controls.Add(Me.lblMensual)
        Me.gbListas.Controls.Add(Me.lblDiario)
        Me.gbListas.Controls.Add(Me.lblA�osCoeMensual)
        Me.gbListas.Controls.Add(Me.lblA�osAltMensual)
        Me.gbListas.Controls.Add(Me.lblA�osAltDiario)
        Me.gbListas.Controls.Add(Me.lblA�osNatDiario)
        Me.gbListas.Controls.Add(Me.lblA�osHidro)
        Me.gbListas.Controls.Add(Me.lblTotalA�os)
        Me.gbListas.Controls.Add(Me.cmbListaAlteradasDiarias)
        Me.gbListas.Controls.Add(Me.lblListasAlteradas)
        Me.gbListas.Controls.Add(Me.lblListasNaturales)
        Me.gbListas.Controls.Add(Me.XPTablaListas)
        Me.gbListas.Location = New System.Drawing.Point(200, 33)
        Me.gbListas.Name = "gbListas"
        Me.gbListas.Size = New System.Drawing.Size(825, 393)
        Me.gbListas.TabIndex = 3
        Me.gbListas.TabStop = False
        Me.gbListas.Text = "Datos de Series asociadas al Punto"
        '
        'lblDatosDiarios
        '
        Me.lblDatosDiarios.AutoSize = True
        Me.lblDatosDiarios.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatosDiarios.Location = New System.Drawing.Point(533, 17)
        Me.lblDatosDiarios.Name = "lblDatosDiarios"
        Me.lblDatosDiarios.Size = New System.Drawing.Size(251, 13)
        Me.lblDatosDiarios.TabIndex = 44
        Me.lblDatosDiarios.Text = "                                Datos diarios                             "
        Me.lblDatosDiarios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDatosMensuales
        '
        Me.lblDatosMensuales.AutoSize = True
        Me.lblDatosMensuales.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatosMensuales.Location = New System.Drawing.Point(277, 17)
        Me.lblDatosMensuales.Name = "lblDatosMensuales"
        Me.lblDatosMensuales.Size = New System.Drawing.Size(238, 13)
        Me.lblDatosMensuales.TabIndex = 43
        Me.lblDatosMensuales.Text = "                         Datos mensuales                         "
        Me.lblDatosMensuales.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblA�osAltDiarioUSO
        '
        Me.lblA�osAltDiarioUSO.AutoSize = True
        Me.lblA�osAltDiarioUSO.Location = New System.Drawing.Point(692, 327)
        Me.lblA�osAltDiarioUSO.Name = "lblA�osAltDiarioUSO"
        Me.lblA�osAltDiarioUSO.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osAltDiarioUSO.TabIndex = 42
        Me.lblA�osAltDiarioUSO.Text = "Label3"
        '
        'lblA�osNatDiarioUSO
        '
        Me.lblA�osNatDiarioUSO.AutoSize = True
        Me.lblA�osNatDiarioUSO.Location = New System.Drawing.Point(589, 327)
        Me.lblA�osNatDiarioUSO.Name = "lblA�osNatDiarioUSO"
        Me.lblA�osNatDiarioUSO.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osNatDiarioUSO.TabIndex = 41
        Me.lblA�osNatDiarioUSO.Text = "Label3"
        '
        'lblA�osAltMensualUSO
        '
        Me.lblA�osAltMensualUSO.AutoSize = True
        Me.lblA�osAltMensualUSO.Location = New System.Drawing.Point(429, 327)
        Me.lblA�osAltMensualUSO.Name = "lblA�osAltMensualUSO"
        Me.lblA�osAltMensualUSO.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osAltMensualUSO.TabIndex = 40
        Me.lblA�osAltMensualUSO.Text = "Label3"
        '
        'lblA�osNatMensualUSO
        '
        Me.lblA�osNatMensualUSO.AutoSize = True
        Me.lblA�osNatMensualUSO.Location = New System.Drawing.Point(324, 327)
        Me.lblA�osNatMensualUSO.Name = "lblA�osNatMensualUSO"
        Me.lblA�osNatMensualUSO.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osNatMensualUSO.TabIndex = 39
        Me.lblA�osNatMensualUSO.Text = "Label3"
        '
        'lblA�osCoeDiaria
        '
        Me.lblA�osCoeDiaria.AutoSize = True
        Me.lblA�osCoeDiaria.Location = New System.Drawing.Point(745, 327)
        Me.lblA�osCoeDiaria.Name = "lblA�osCoeDiaria"
        Me.lblA�osCoeDiaria.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osCoeDiaria.TabIndex = 38
        Me.lblA�osCoeDiaria.Text = "Label3"
        '
        'lblA�osNatMensual
        '
        Me.lblA�osNatMensual.AutoSize = True
        Me.lblA�osNatMensual.Location = New System.Drawing.Point(273, 327)
        Me.lblA�osNatMensual.Name = "lblA�osNatMensual"
        Me.lblA�osNatMensual.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osNatMensual.TabIndex = 22
        Me.lblA�osNatMensual.Text = "Label5"
        '
        'chkboxUsarCoeDiaria
        '
        Me.chkboxUsarCoeDiaria.AutoSize = True
        Me.chkboxUsarCoeDiaria.Enabled = False
        Me.chkboxUsarCoeDiaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkboxUsarCoeDiaria.Location = New System.Drawing.Point(542, 359)
        Me.chkboxUsarCoeDiaria.Name = "chkboxUsarCoeDiaria"
        Me.chkboxUsarCoeDiaria.Size = New System.Drawing.Size(229, 17)
        Me.chkboxUsarCoeDiaria.TabIndex = 37
        Me.chkboxUsarCoeDiaria.Text = "Usar coetaneidad para Avenidas y Sequ�as"
        Me.chkboxUsarCoeDiaria.UseVisualStyleBackColor = True
        '
        'grpboxLeyenda
        '
        Me.grpboxLeyenda.Controls.Add(Me.lblLeyenda6)
        Me.grpboxLeyenda.Controls.Add(Me.lblLeyenda5)
        Me.grpboxLeyenda.Controls.Add(Me.lblLeyenda4)
        Me.grpboxLeyenda.Controls.Add(Me.lblLeyenda2)
        Me.grpboxLeyenda.Controls.Add(Me.lblLeyenda3)
        Me.grpboxLeyenda.Controls.Add(Me.lblLeyenda7)
        Me.grpboxLeyenda.Controls.Add(Me.lblLeyenda1)
        Me.grpboxLeyenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpboxLeyenda.Location = New System.Drawing.Point(9, 272)
        Me.grpboxLeyenda.Name = "grpboxLeyenda"
        Me.grpboxLeyenda.Size = New System.Drawing.Size(140, 112)
        Me.grpboxLeyenda.TabIndex = 36
        Me.grpboxLeyenda.TabStop = False
        Me.grpboxLeyenda.Text = "Leyenda"
        '
        'lblLeyenda6
        '
        Me.lblLeyenda6.AutoSize = True
        Me.lblLeyenda6.Location = New System.Drawing.Point(9, 82)
        Me.lblLeyenda6.Name = "lblLeyenda6"
        Me.lblLeyenda6.Size = New System.Drawing.Size(100, 12)
        Me.lblLeyenda6.TabIndex = 6
        Me.lblLeyenda6.Text = "NCO: A�o no coet�neo"
        '
        'lblLeyenda5
        '
        Me.lblLeyenda5.AutoSize = True
        Me.lblLeyenda5.Location = New System.Drawing.Point(9, 69)
        Me.lblLeyenda5.Name = "lblLeyenda5"
        Me.lblLeyenda5.Size = New System.Drawing.Size(81, 12)
        Me.lblLeyenda5.TabIndex = 5
        Me.lblLeyenda5.Text = "CO: A�o coet�neo"
        '
        'lblLeyenda4
        '
        Me.lblLeyenda4.AutoSize = True
        Me.lblLeyenda4.Location = New System.Drawing.Point(9, 56)
        Me.lblLeyenda4.Name = "lblLeyenda4"
        Me.lblLeyenda4.Size = New System.Drawing.Size(79, 12)
        Me.lblLeyenda4.TabIndex = 4
        Me.lblLeyenda4.Text = "SD: A�o sin datos"
        '
        'lblLeyenda2
        '
        Me.lblLeyenda2.AutoSize = True
        Me.lblLeyenda2.Location = New System.Drawing.Point(9, 29)
        Me.lblLeyenda2.Name = "lblLeyenda2"
        Me.lblLeyenda2.Size = New System.Drawing.Size(78, 12)
        Me.lblLeyenda2.TabIndex = 3
        Me.lblLeyenda2.Text = "I: A�o Incompleto"
        '
        'lblLeyenda3
        '
        Me.lblLeyenda3.AutoSize = True
        Me.lblLeyenda3.Location = New System.Drawing.Point(9, 43)
        Me.lblLeyenda3.Name = "lblLeyenda3"
        Me.lblLeyenda3.Size = New System.Drawing.Size(111, 12)
        Me.lblLeyenda3.TabIndex = 2
        Me.lblLeyenda3.Text = "CD: Calculado con diarios"
        '
        'lblLeyenda7
        '
        Me.lblLeyenda7.AutoSize = True
        Me.lblLeyenda7.Location = New System.Drawing.Point(9, 96)
        Me.lblLeyenda7.Name = "lblLeyenda7"
        Me.lblLeyenda7.Size = New System.Drawing.Size(93, 12)
        Me.lblLeyenda7.TabIndex = 1
        Me.lblLeyenda7.Text = "NS: Sin serie cargada"
        '
        'lblLeyenda1
        '
        Me.lblLeyenda1.AutoSize = True
        Me.lblLeyenda1.Location = New System.Drawing.Point(9, 16)
        Me.lblLeyenda1.Name = "lblLeyenda1"
        Me.lblLeyenda1.Size = New System.Drawing.Size(76, 12)
        Me.lblLeyenda1.TabIndex = 0
        Me.lblLeyenda1.Text = "C: A�o Completo"
        '
        'lblSerieNatMensual
        '
        Me.lblSerieNatMensual.AutoSize = True
        Me.lblSerieNatMensual.BackColor = System.Drawing.Color.White
        Me.lblSerieNatMensual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSerieNatMensual.Location = New System.Drawing.Point(90, 75)
        Me.lblSerieNatMensual.Name = "lblSerieNatMensual"
        Me.lblSerieNatMensual.Size = New System.Drawing.Size(18, 15)
        Me.lblSerieNatMensual.TabIndex = 35
        Me.lblSerieNatMensual.Text = "---"
        '
        'chkboxUsarCoe
        '
        Me.chkboxUsarCoe.AutoSize = True
        Me.chkboxUsarCoe.Enabled = False
        Me.chkboxUsarCoe.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkboxUsarCoe.Location = New System.Drawing.Point(293, 359)
        Me.chkboxUsarCoe.Name = "chkboxUsarCoe"
        Me.chkboxUsarCoe.Size = New System.Drawing.Size(222, 17)
        Me.chkboxUsarCoe.TabIndex = 10
        Me.chkboxUsarCoe.Text = "Usar coetaneidad para Valores Habituales"
        Me.chkboxUsarCoe.UseVisualStyleBackColor = True
        '
        'lblSerieNatDiaria
        '
        Me.lblSerieNatDiaria.AutoSize = True
        Me.lblSerieNatDiaria.BackColor = System.Drawing.Color.White
        Me.lblSerieNatDiaria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSerieNatDiaria.Location = New System.Drawing.Point(90, 50)
        Me.lblSerieNatDiaria.Name = "lblSerieNatDiaria"
        Me.lblSerieNatDiaria.Size = New System.Drawing.Size(18, 15)
        Me.lblSerieNatDiaria.TabIndex = 34
        Me.lblSerieNatDiaria.Text = "---"
        '
        'lblAltNombreEstatico
        '
        Me.lblAltNombreEstatico.AutoSize = True
        Me.lblAltNombreEstatico.Location = New System.Drawing.Point(6, 148)
        Me.lblAltNombreEstatico.Name = "lblAltNombreEstatico"
        Me.lblAltNombreEstatico.Size = New System.Drawing.Size(46, 13)
        Me.lblAltNombreEstatico.TabIndex = 33
        Me.lblAltNombreEstatico.Text = "Descrip."
        '
        'lblCodigoAlt
        '
        Me.lblCodigoAlt.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.lblCodigoAlt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCodigoAlt.Location = New System.Drawing.Point(53, 147)
        Me.lblCodigoAlt.Name = "lblCodigoAlt"
        Me.lblCodigoAlt.Size = New System.Drawing.Size(141, 21)
        Me.lblCodigoAlt.TabIndex = 32
        Me.lblCodigoAlt.Text = "---"
        Me.lblCodigoAlt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblIDMensual
        '
        Me.lblIDMensual.AutoSize = True
        Me.lblIDMensual.BackColor = System.Drawing.Color.White
        Me.lblIDMensual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblIDMensual.Location = New System.Drawing.Point(86, 197)
        Me.lblIDMensual.Name = "lblIDMensual"
        Me.lblIDMensual.Size = New System.Drawing.Size(18, 15)
        Me.lblIDMensual.TabIndex = 31
        Me.lblIDMensual.Text = "---"
        '
        'lblIDDiaria
        '
        Me.lblIDDiaria.AutoSize = True
        Me.lblIDDiaria.BackColor = System.Drawing.Color.White
        Me.lblIDDiaria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblIDDiaria.Location = New System.Drawing.Point(86, 171)
        Me.lblIDDiaria.Name = "lblIDDiaria"
        Me.lblIDDiaria.Size = New System.Drawing.Size(18, 15)
        Me.lblIDDiaria.TabIndex = 30
        Me.lblIDDiaria.Text = "---"
        '
        'lblASerieMensual
        '
        Me.lblASerieMensual.AutoSize = True
        Me.lblASerieMensual.Location = New System.Drawing.Point(6, 197)
        Me.lblASerieMensual.Name = "lblASerieMensual"
        Me.lblASerieMensual.Size = New System.Drawing.Size(74, 13)
        Me.lblASerieMensual.TabIndex = 29
        Me.lblASerieMensual.Text = "Serie Mensual"
        '
        'lblASerieDiaria
        '
        Me.lblASerieDiaria.AutoSize = True
        Me.lblASerieDiaria.Location = New System.Drawing.Point(6, 171)
        Me.lblASerieDiaria.Name = "lblASerieDiaria"
        Me.lblASerieDiaria.Size = New System.Drawing.Size(61, 13)
        Me.lblASerieDiaria.TabIndex = 28
        Me.lblASerieDiaria.Text = "Serie Diaria"
        '
        'lblADiario
        '
        Me.lblADiario.AutoSize = True
        Me.lblADiario.Location = New System.Drawing.Point(6, 128)
        Me.lblADiario.Name = "lblADiario"
        Me.lblADiario.Size = New System.Drawing.Size(40, 13)
        Me.lblADiario.TabIndex = 27
        Me.lblADiario.Text = "C�digo"
        '
        'lblMensual
        '
        Me.lblMensual.AutoSize = True
        Me.lblMensual.Location = New System.Drawing.Point(6, 75)
        Me.lblMensual.Name = "lblMensual"
        Me.lblMensual.Size = New System.Drawing.Size(74, 13)
        Me.lblMensual.TabIndex = 26
        Me.lblMensual.Text = "Serie Mensual"
        '
        'lblDiario
        '
        Me.lblDiario.AutoSize = True
        Me.lblDiario.Location = New System.Drawing.Point(6, 50)
        Me.lblDiario.Name = "lblDiario"
        Me.lblDiario.Size = New System.Drawing.Size(61, 13)
        Me.lblDiario.TabIndex = 25
        Me.lblDiario.Text = "Serie Diaria"
        '
        'lblA�osCoeMensual
        '
        Me.lblA�osCoeMensual.AutoSize = True
        Me.lblA�osCoeMensual.Location = New System.Drawing.Point(482, 327)
        Me.lblA�osCoeMensual.Name = "lblA�osCoeMensual"
        Me.lblA�osCoeMensual.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osCoeMensual.TabIndex = 24
        Me.lblA�osCoeMensual.Text = "Label7"
        '
        'lblA�osAltMensual
        '
        Me.lblA�osAltMensual.AutoSize = True
        Me.lblA�osAltMensual.Location = New System.Drawing.Point(376, 327)
        Me.lblA�osAltMensual.Name = "lblA�osAltMensual"
        Me.lblA�osAltMensual.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osAltMensual.TabIndex = 23
        Me.lblA�osAltMensual.Text = "Label6"
        '
        'lblA�osAltDiario
        '
        Me.lblA�osAltDiario.AutoSize = True
        Me.lblA�osAltDiario.Location = New System.Drawing.Point(638, 327)
        Me.lblA�osAltDiario.Name = "lblA�osAltDiario"
        Me.lblA�osAltDiario.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osAltDiario.TabIndex = 20
        Me.lblA�osAltDiario.Text = "Label3"
        '
        'lblA�osNatDiario
        '
        Me.lblA�osNatDiario.AutoSize = True
        Me.lblA�osNatDiario.Location = New System.Drawing.Point(535, 327)
        Me.lblA�osNatDiario.Name = "lblA�osNatDiario"
        Me.lblA�osNatDiario.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osNatDiario.TabIndex = 19
        Me.lblA�osNatDiario.Text = "Label2"
        '
        'lblA�osHidro
        '
        Me.lblA�osHidro.AutoSize = True
        Me.lblA�osHidro.Location = New System.Drawing.Point(224, 327)
        Me.lblA�osHidro.Name = "lblA�osHidro"
        Me.lblA�osHidro.Size = New System.Drawing.Size(39, 13)
        Me.lblA�osHidro.TabIndex = 18
        Me.lblA�osHidro.Text = "Label1"
        '
        'lblTotalA�os
        '
        Me.lblTotalA�os.AutoSize = True
        Me.lblTotalA�os.Location = New System.Drawing.Point(155, 327)
        Me.lblTotalA�os.Name = "lblTotalA�os"
        Me.lblTotalA�os.Size = New System.Drawing.Size(45, 13)
        Me.lblTotalA�os.TabIndex = 17
        Me.lblTotalA�os.Text = "TOTAL:"
        '
        'cmbListaAlteradasDiarias
        '
        Me.cmbListaAlteradasDiarias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbListaAlteradasDiarias.FormattingEnabled = True
        Me.cmbListaAlteradasDiarias.Location = New System.Drawing.Point(53, 125)
        Me.cmbListaAlteradasDiarias.Name = "cmbListaAlteradasDiarias"
        Me.cmbListaAlteradasDiarias.Size = New System.Drawing.Size(141, 21)
        Me.cmbListaAlteradasDiarias.TabIndex = 13
        '
        'lblListasAlteradas
        '
        Me.lblListasAlteradas.AutoSize = True
        Me.lblListasAlteradas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblListasAlteradas.Location = New System.Drawing.Point(6, 107)
        Me.lblListasAlteradas.Name = "lblListasAlteradas"
        Me.lblListasAlteradas.Size = New System.Drawing.Size(160, 15)
        Me.lblListasAlteradas.TabIndex = 12
        Me.lblListasAlteradas.Text = "Series de Valores Alterados:"
        '
        'lblListasNaturales
        '
        Me.lblListasNaturales.AutoSize = True
        Me.lblListasNaturales.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblListasNaturales.Location = New System.Drawing.Point(6, 31)
        Me.lblListasNaturales.Name = "lblListasNaturales"
        Me.lblListasNaturales.Size = New System.Drawing.Size(162, 15)
        Me.lblListasNaturales.TabIndex = 11
        Me.lblListasNaturales.Text = "Series de Valores Naturales:"
        '
        'XPTablaListas
        '
        Me.XPTablaListas.ColumnModel = Me.ColumnModel1
        Me.XPTablaListas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.XPTablaListas.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.XPTablaListas.Location = New System.Drawing.Point(200, 37)
        Me.XPTablaListas.Name = "XPTablaListas"
        Me.XPTablaListas.NoItemsText = "Seleccione un punto de la lista para analizar sus series asociadas."
        Me.XPTablaListas.Size = New System.Drawing.Size(613, 277)
        Me.XPTablaListas.TabIndex = 9
        Me.XPTablaListas.TableModel = Me.TableModel1
        Me.XPTablaListas.Text = "XPTablaListas"
        '
        'ColumnModel1
        '
        Me.ColumnModel1.HeaderHeight = 40
        '
        'btnCalcular
        '
        Me.btnCalcular.Location = New System.Drawing.Point(847, 19)
        Me.btnCalcular.Name = "btnCalcular"
        Me.btnCalcular.Size = New System.Drawing.Size(154, 171)
        Me.btnCalcular.TabIndex = 4
        Me.btnCalcular.Text = "Calcular"
        Me.btnCalcular.UseVisualStyleBackColor = True
        '
        'gbInforme
        '
        Me.gbInforme.Controls.Add(Me.lstBoxInformes)
        Me.gbInforme.Controls.Add(Me.btnCalcular)
        Me.gbInforme.Location = New System.Drawing.Point(12, 432)
        Me.gbInforme.Name = "gbInforme"
        Me.gbInforme.Size = New System.Drawing.Size(1013, 197)
        Me.gbInforme.TabIndex = 10
        Me.gbInforme.TabStop = False
        Me.gbInforme.Text = "Informes a realizar:"
        '
        'lstBoxInformes
        '
        Me.lstBoxInformes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstBoxInformes.BackColor = System.Drawing.Color.WhiteSmoke
        Me.lstBoxInformes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstBoxInformes.FormattingEnabled = True
        Me.lstBoxInformes.Location = New System.Drawing.Point(12, 19)
        Me.lstBoxInformes.MultiColumn = True
        Me.lstBoxInformes.Name = "lstBoxInformes"
        Me.lstBoxInformes.Size = New System.Drawing.Size(829, 171)
        Me.lstBoxInformes.TabIndex = 5
        '
        'gbProyecto
        '
        Me.gbProyecto.Controls.Add(Me.lblDescripcion)
        Me.gbProyecto.Controls.Add(Me.lblProyectoDesc)
        Me.gbProyecto.Controls.Add(Me.cbProyectos)
        Me.gbProyecto.Location = New System.Drawing.Point(12, 33)
        Me.gbProyecto.Name = "gbProyecto"
        Me.gbProyecto.Size = New System.Drawing.Size(181, 90)
        Me.gbProyecto.TabIndex = 11
        Me.gbProyecto.TabStop = False
        Me.gbProyecto.Text = "Proyecto"
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(9, 37)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(63, 13)
        Me.lblDescripcion.TabIndex = 47
        Me.lblDescripcion.Text = "Descripci�n"
        '
        'lblProyectoDesc
        '
        Me.lblProyectoDesc.BackColor = System.Drawing.Color.White
        Me.lblProyectoDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProyectoDesc.Location = New System.Drawing.Point(12, 50)
        Me.lblProyectoDesc.Name = "lblProyectoDesc"
        Me.lblProyectoDesc.Size = New System.Drawing.Size(163, 33)
        Me.lblProyectoDesc.TabIndex = 46
        '
        'cbProyectos
        '
        Me.cbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbProyectos.FormattingEnabled = True
        Me.cbProyectos.Location = New System.Drawing.Point(12, 14)
        Me.cbProyectos.Name = "cbProyectos"
        Me.cbProyectos.Size = New System.Drawing.Size(160, 21)
        Me.cbProyectos.TabIndex = 0
        '
        'FormInicial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange
        Me.ClientSize = New System.Drawing.Size(1036, 635)
        Me.Controls.Add(Me.gbProyecto)
        Me.Controls.Add(Me.gbListas)
        Me.Controls.Add(Me.gbInforme)
        Me.Controls.Add(Me.gbPuntos)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "FormInicial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IAHRIS (�ndices de Alteraci�n hidrol�gica en RIoS) "
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.gbPuntos.ResumeLayout(False)
        Me.gbPuntos.PerformLayout()
        Me.gbListas.ResumeLayout(False)
        Me.gbListas.PerformLayout()
        Me.grpboxLeyenda.ResumeLayout(False)
        Me.grpboxLeyenda.PerformLayout()
        CType(Me.XPTablaListas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbInforme.ResumeLayout(False)
        Me.gbProyecto.ResumeLayout(False)
        Me.gbProyecto.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents Gesti�nDePuntosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents A�adirPuntoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lstboxPuntos As System.Windows.Forms.ListBox
    Friend WithEvents Gesti�nDeListasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents A�adirListaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gbPuntos As System.Windows.Forms.GroupBox
    Friend WithEvents lblPuntoNListas As System.Windows.Forms.Label
    Friend WithEvents lblPuntoClave As System.Windows.Forms.Label
    Friend WithEvents lblPuntoNombre As System.Windows.Forms.Label
    Friend WithEvents EliminarPuntoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminarListaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gbListas As System.Windows.Forms.GroupBox
    Friend WithEvents XPTablaListas As XPTable.Models.Table
    Friend WithEvents ColumnModel1 As XPTable.Models.ColumnModel
    Friend WithEvents TableModel1 As XPTable.Models.TableModel
    Friend WithEvents btnCalcular As System.Windows.Forms.Button
    Friend WithEvents cmbListaAlteradasDiarias As System.Windows.Forms.ComboBox
    Friend WithEvents lblListasAlteradas As System.Windows.Forms.Label
    Friend WithEvents lblListasNaturales As System.Windows.Forms.Label
    Friend WithEvents lblA�osCoeMensual As System.Windows.Forms.Label
    Friend WithEvents lblA�osAltMensual As System.Windows.Forms.Label
    Friend WithEvents lblA�osNatMensual As System.Windows.Forms.Label
    Friend WithEvents lblA�osAltDiario As System.Windows.Forms.Label
    Friend WithEvents lblA�osNatDiario As System.Windows.Forms.Label
    Friend WithEvents lblA�osHidro As System.Windows.Forms.Label
    Friend WithEvents lblTotalA�os As System.Windows.Forms.Label
    Friend WithEvents lblADiario As System.Windows.Forms.Label
    Friend WithEvents lblMensual As System.Windows.Forms.Label
    Friend WithEvents lblDiario As System.Windows.Forms.Label
    Friend WithEvents gbInforme As System.Windows.Forms.GroupBox
    Friend WithEvents lblASerieMensual As System.Windows.Forms.Label
    Friend WithEvents lblASerieDiaria As System.Windows.Forms.Label
    Friend WithEvents Gesti�nDeAlteracionesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents A�adirAlteraci�nToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminarAlteraci�nToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblIDMensual As System.Windows.Forms.Label
    Friend WithEvents lblIDDiaria As System.Windows.Forms.Label
    Friend WithEvents lblAltNombreEstatico As System.Windows.Forms.Label
    Friend WithEvents lblCodigoAlt As System.Windows.Forms.Label
    Friend WithEvents chkboxUsarCoe As System.Windows.Forms.CheckBox
    Friend WithEvents lblSerieNatMensual As System.Windows.Forms.Label
    Friend WithEvents lblSerieNatDiaria As System.Windows.Forms.Label
    Friend WithEvents grpboxLeyenda As System.Windows.Forms.GroupBox
    Friend WithEvents chkboxUsarCoeDiaria As System.Windows.Forms.CheckBox
    Friend WithEvents lblA�osCoeDiaria As System.Windows.Forms.Label
    Friend WithEvents lblA�osAltDiarioUSO As System.Windows.Forms.Label
    Friend WithEvents lblA�osNatDiarioUSO As System.Windows.Forms.Label
    Friend WithEvents lblA�osAltMensualUSO As System.Windows.Forms.Label
    Friend WithEvents lblA�osNatMensualUSO As System.Windows.Forms.Label
    Friend WithEvents lblDatosDiarios As System.Windows.Forms.Label
    Friend WithEvents lblDatosMensuales As System.Windows.Forms.Label
    Friend WithEvents lstBoxInformes As System.Windows.Forms.ListBox
    Friend WithEvents lblPuntosNListas As System.Windows.Forms.Label
    Friend WithEvents lblPuntosNombre As System.Windows.Forms.Label
    Friend WithEvents lblPuntosClave As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda3 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda7 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda1 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda2 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda4 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda5 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda6 As System.Windows.Forms.Label
    Friend WithEvents IdiomasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ManualesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gbProyecto As System.Windows.Forms.GroupBox
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents lblProyectoDesc As System.Windows.Forms.Label
    Friend WithEvents cbProyectos As System.Windows.Forms.ComboBox
    Friend WithEvents Gesti�nDeProyectosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents A�adirProyectoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminarProyectoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GestBBDDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbMesInicio As System.Windows.Forms.ComboBox
    Friend WithEvents lblMesInicio As System.Windows.Forms.Label

End Class
