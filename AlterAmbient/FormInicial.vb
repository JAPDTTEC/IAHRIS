Imports IAHRIS.BBDD.OleDbDataBase
Imports IAHRIS.Rellenar.RellenarForm
Imports IAHRIS.TestFechas.TestFechas
Imports XPTable
Imports XPTable.Models
Imports System.Collections.ObjectModel
Public Class FormInicial

    Private _RutaBBDD As String
    Private _cMDB As BBDD.OleDbDataBase
    Private _rellenar As Rellenar.RellenarForm
    Private _testFechas As TestFechas.TestFechas
    Private _simulacion As TestFechas.TestFechas.Simulacion
    Private _informes As GeneracionInformes
    Private _PtoSeleccionado As String
    Private _AltSeleccionada As String

    Private _strPto As String
    Private _strAlt As String

    Private _sVersion As String = "v2.0 BETA"

    Private _id_proy_selec As Integer = -1

    ' Para poder centrar el formulario
    Private __alto As Integer
    Private __ancho As Integer

    Dim _traductor As MultiLangXML.MultiIdiomasXML

    Dim _rutaXML As String

    ' Ñapa para no refrescar todo el formulario
    
    Private Enum TIPO_BBDD
        VERSION_OK
        VERSION_ANT_V1
        VERSION_DESC
    End Enum


#Region "Eventos formulario"
    Public Sub New()

        ' Llamada necesaria para el Diseñador de Windows Forms.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub
    Private Sub FormInicial_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Text = "IAHRIS (Índices de Alteración Hidrológica de RÍoS)                        [" & Me._sVersion & "]"

        Me.__alto = Me.Height
        Me.__ancho = Me.Width

        
        ' -------------------------------------
        ' ---- Traducir formulario ------------
        ' -------------------------------------
        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")

        

        ' Testear el fichero modelo
        If (Not My.Computer.FileSystem.FileExists(Me._traductor.getRutaExcel)) Then
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoFoundXLS") & Me._traductor.getRutaExcel, _
                            Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFatal"), _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Me.Dispose()
            Return
        End If

        'Testear que hay una base de datos en el directorio.
        If (Not My.Computer.FileSystem.FileExists(".\IAHRISv2.mdb")) Then
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoFoundDB") & Application.ExecutablePath, _
                            Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFatal"), _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Me.Dispose()
            Return
        Else
            Me._RutaBBDD = Application.StartupPath & "\IAHRISv2.mdb"

            Me._cMDB = New BBDD.OleDbDataBase("Base", Me._RutaBBDD)

            Me._rellenar = New Rellenar.RellenarForm(Me._cMDB)

            Me._rellenar.RellenarProyectos(Me.cbProyectos)

            'Me._rellenar.RellenarPuntos(Me.lstboxPuntos)

            Me._testFechas = New TestFechas.TestFechas(Me._cMDB)
        End If

        Me.Cursor = Cursors.WaitCursor
        ' Cargar el menu de idiomas
        Dim files As ReadOnlyCollection(Of String)
        files = My.Computer.FileSystem.GetFiles(Application.StartupPath & "\lang", FileIO.SearchOption.SearchTopLevelOnly, "*.xml")

        For i As Integer = 0 To files.Count - 1
            Dim strIdioma As String = ""
            Dim ruta As String = ""
            If (Me._traductor.testFormatXML(files(i), strIdioma, ruta)) Then
                Dim mnuIdioma As New ToolStripMenuItem(strIdioma)
                mnuIdioma.Tag = files(i)
                AddHandler mnuIdioma.Click, AddressOf OpcionMenu_Click
                Me.IdiomasToolStripMenuItem.DropDownItems.Add(mnuIdioma)
            Else
                If (ruta = "") Then
                    MessageBox.Show("Error al cargar: " & files(i) & vbCrLf & "Error no puedo encontrar el fichero excel", "Error al cargar idioma")
                ElseIf (strIdioma = "") Then
                    MessageBox.Show("Error al cargar: " & files(i) & vbCrLf & "Error encuentro el identificador de idioma correcto", "Error al cargar idioma")
                Else
                    MessageBox.Show("Error al cargar: " & files(i), "Error al cargar idioma")
                End If
            End If
        Next

        Me.Cursor = Cursors.Default


        ' ----------------------------------------
        ' ------- Tabla de datos -----------------
        ' ----------------------------------------
        Me.XPTablaListas.ColumnResizing = False
        Me.XPTablaListas.HeaderRenderer.Trimming = StringTrimming.None

        Me.XPTablaListas.HeaderRenderer.Font = New Font(Me.XPTablaListas.HeaderRenderer.Font.FontFamily, 7.5)

        Me.XPTablaListas.BeginUpdate()

        'Me.XPTablaListas.NoItemsText = "Seleccione un punto de la lista para analizar sus series asociadas."
        Me.XPTablaListas.NoItemsText = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "noitem")

        ' Columna de los años
        Dim colTexto As TextColumn = New TextColumn()
        colTexto.Editable = False
        colTexto.Sortable = True
        colTexto.Width = 67
        'colTexto.Text = "Año" & vbCrLf & "hidrológico"
        colTexto.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "year")

        Dim imgColND As ImageColumn = New ImageColumn()
        imgColND.Editable = False
        imgColND.Sortable = False
        imgColND.Width = 55
        'imgColND.Text = "Natural" & vbCrLf & "Diaria" & vbCrLf & "Serie"
        imgColND.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynat")

        Dim chkColND As CheckBoxColumn = New CheckBoxColumn()
        chkColND.Editable = False
        chkColND.Sortable = False
        chkColND.Width = 50
        'chkColND.Text = "Natural" & vbCrLf & "Diaria" & vbCrLf & "Cálculo"
        chkColND.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynatinter")


        Dim imgColAD As ImageColumn = New ImageColumn()
        imgColAD.Editable = False
        imgColAD.Sortable = False
        imgColAD.ImageOnRight = True
        imgColAD.Width = 55
        'imgColAD.Text = "Alterada" & vbCrLf & "Diaria" & vbCrLf & "Serie"
        imgColAD.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyalt")

        Dim chkColAD As CheckBoxColumn = New CheckBoxColumn()
        chkColAD.Editable = False
        chkColAD.Sortable = False
        chkColAD.Width = 50
        chkColAD.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyaltinter")

        Dim imgColCD As ImageColumn = New ImageColumn()
        imgColCD.Editable = False
        imgColCD.Sortable = False
        imgColCD.ImageOnRight = True
        imgColCD.Width = 50
        imgColCD.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coedaily")

        Dim imgColNM As ImageColumn = New ImageColumn()
        imgColNM.Editable = False
        imgColNM.Sortable = False
        imgColNM.ImageOnRight = True
        imgColNM.Width = 55
        imgColNM.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynat")

        Dim chkColNM As CheckBoxColumn = New CheckBoxColumn()
        chkColNM.Editable = False
        chkColNM.Sortable = False
        chkColNM.Width = 50
        chkColNM.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynatinter")

        Dim imgColAM As ImageColumn = New ImageColumn()
        imgColAM.Editable = False
        imgColAM.Sortable = False
        imgColAM.ImageOnRight = True
        imgColAM.Width = 55
        imgColAM.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyalt")

        Dim chkColAM As CheckBoxColumn = New CheckBoxColumn()
        chkColAM.Editable = False
        chkColAM.Sortable = False
        chkColAM.Width = 50
        chkColAM.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyaltinter")

        Dim imgColCM As ImageColumn = New ImageColumn()
        imgColCM.Editable = False
        imgColCM.Sortable = False
        imgColCM.ImageOnRight = True
        imgColCM.Width = 50
        imgColCM.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coemonthly")

        Dim cols() As Column = {colTexto, imgColNM, chkColNM, imgColAM, chkColAM, imgColCM, imgColND, chkColND, imgColAD, chkColAD, imgColCD}
        Me.ColumnModel1.Columns.AddRange(cols)
        Me.XPTablaListas.EndUpdate()


        ' Cambiar los label de los años
        Me.lblAñosHidro.Text = 0
        Me.lblAñosCoeDiaria.Text = 0
        Me.lblAñosCoeMensual.Text = 0
        Me.lblAñosNatDiario.Text = 0
        Me.lblAñosNatDiarioUSO.Text = 0
        Me.lblAñosNatMensual.Text = 0
        Me.lblAñosNatMensualUSO.Text = 0
        Me.lblAñosAltDiario.Text = 0
        Me.lblAñosAltDiarioUSO.Text = 0
        Me.lblAñosAltMensual.Text = 0
        Me.lblAñosAltMensualUSO.Text = 0

        Me.btnCalcular.Enabled = False

        Dim myBuildInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)

        Me.Text &= " - v" & myBuildInfo.FileMajorPart & "." & myBuildInfo.FileMinorPart

    End Sub
    Private Sub FormInicial_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Try
            Me._cMDB.Desconectar()
            Me._cMDB = Nothing
        Catch ex As Exception

        End Try

        Application.OpenForms.Item("FormBienvenida").Close()

    End Sub
    Private Sub FormInicial_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        ' Refrescar listas cuando se pone el form activo

        Me._rellenar.RellenarProyectos(Me.cbProyectos, Me._id_proy_selec)
        Me._rellenar.RellenarPuntos(Me.lstboxPuntos, Me._id_proy_selec)

        '' Centrar el form
        'Me.Left = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.__ancho)
        'Me.Top = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.__alto)
    End Sub
#End Region

#Region "Acciones de los Menus"
    Private Sub AñadirProyectoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AñadirProyectoToolStripMenuItem.Click
        Me.ResetearFormPrincipal()

        Dim fanadirproyecto As New FormAnadirProyecto(Me._cMDB)
        fanadirproyecto.ShowDialog()

    End Sub
    Private Sub EliminarProyectoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminarProyectoToolStripMenuItem.Click
        Me.ResetearFormPrincipal()

        Dim feliminarproyecto As New FormEliminarProyecto(Me._cMDB)
        feliminarproyecto.ShowDialog()

        Me.cbProyectos_SelectedIndexChanged(Nothing, Nothing)

        Me._cMDB.Desconectar()
        DataBaseUtils.DataBaseUtils.CompactAccessDB("Base", Me._RutaBBDD)
        Me._cMDB = New BBDD.OleDbDataBase("Base", Me._RutaBBDD)
    End Sub
    Private Sub AñadirPuntoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AñadirPuntoToolStripMenuItem.Click

        Me.ResetearFormPrincipal()

        ' Enseño el formulario como un dialog
        Dim fanadir As New FormAnadirPunto(Me._cMDB, "Punto", Me._id_proy_selec)
        fanadir.ShowDialog()

    End Sub

    Private Sub AñadirAlteraciónToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AñadirAlteraciónToolStripMenuItem.Click

        Me.ResetearFormPrincipal()

        ' Enseño el formulario como un dialog
        Dim fanadir As New FormAnadirPunto(Me._cMDB, "Alteración", Me._id_proy_selec)
        fanadir.ShowDialog()

    End Sub

    Private Sub AñadirListaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AñadirListaToolStripMenuItem.Click

        Me.ResetearFormPrincipal()

        Dim fanadir As New FormAnadirListas(Me._cMDB)
        fanadir.ShowDialog()

        Me._cMDB.Desconectar()
        DataBaseUtils.DataBaseUtils.CompactAccessDB("Base", Me._RutaBBDD)
        Me._cMDB = New BBDD.OleDbDataBase("Base", Me._RutaBBDD)

    End Sub

    Private Sub EliminarListaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminarListaToolStripMenuItem.Click

        Me.ResetearFormPrincipal()


        Dim feliminar As New FormEliminarLista(Me._cMDB)
        feliminar.ShowDialog()

        Me._cMDB.Desconectar()
        DataBaseUtils.DataBaseUtils.CompactAccessDB("Base", Me._RutaBBDD)
        Me._cMDB = New BBDD.OleDbDataBase("Base", Me._RutaBBDD)
    End Sub

    Private Sub EliminarPuntoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminarPuntoToolStripMenuItem.Click

        Me.ResetearFormPrincipal()

        Dim feliminar As New FormEliminarPunto(Me._cMDB, "Punto", Me._id_proy_selec)
        feliminar.ShowDialog()

        Me._cMDB.Desconectar()
        DataBaseUtils.DataBaseUtils.CompactAccessDB("Base", Me._RutaBBDD)
        Me._cMDB = New BBDD.OleDbDataBase("Base", Me._RutaBBDD)
    End Sub

    Private Sub EliminarAlteraciónToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminarAlteraciónToolStripMenuItem.Click

        Me.ResetearFormPrincipal()

        Dim feliminar As New FormEliminarPunto(Me._cMDB, "Alteracion", Me._id_proy_selec)
        feliminar.ShowDialog()

        Me._cMDB.Desconectar()
        DataBaseUtils.DataBaseUtils.CompactAccessDB("Base", Me._RutaBBDD)
        Me._cMDB = New BBDD.OleDbDataBase("Base", Me._RutaBBDD)
    End Sub

    Private Sub ImportarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportarToolStripMenuItem.Click
        Dim fimportar As New FormImportar(Me._cMDB)
        fimportar.ShowDialog()
    End Sub

    Private Sub ExportarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportarToolStripMenuItem.Click
        Dim sfd As SaveFileDialog = New SaveFileDialog()

        Me._cMDB.Desconectar()

        sfd.Filter = "Base de datos ACCESS (*.mdb)|*.mdb"
        sfd.FilterIndex = 1
        sfd.CheckFileExists = False
        sfd.AddExtension = True
        sfd.OverwritePrompt = True

        Try
            If sfd.ShowDialog() = DialogResult.OK Then
                My.Computer.FileSystem.CopyFile(Me._RutaBBDD, sfd.FileName, True)
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strExportacion") & vbCrLf & sfd.FileName, _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorExport") & vbCrLf & ex.Message, _
                            Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

        Me._cMDB = New BBDD.OleDbDataBase("Base", Me._RutaBBDD)

    End Sub

    Private Sub OpcionMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim OpcionSeleccionada As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

        ' OpcionSeleccionada.Tag
        If (Not Me._traductor.cambiarIdioma(OpcionSeleccionada.Tag)) Then
            Return
        End If
        Me._traductor.traducirForm(OpcionSeleccionada.Tag, "")


        Me.XPTablaListas.BeginUpdate()
        Me.XPTablaListas.NoItemsText = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "noitem")
        Me.XPTablaListas.ColumnModel.Columns(0).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "year")
        Me.XPTablaListas.ColumnModel.Columns(6).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynat")
        Me.XPTablaListas.ColumnModel.Columns(7).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailynatinter")
        Me.XPTablaListas.ColumnModel.Columns(8).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyalt")
        Me.XPTablaListas.ColumnModel.Columns(9).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "dailyaltinter")
        Me.XPTablaListas.ColumnModel.Columns(10).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coedaily")
        Me.XPTablaListas.ColumnModel.Columns(1).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynat")
        Me.XPTablaListas.ColumnModel.Columns(2).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlynatinter")
        Me.XPTablaListas.ColumnModel.Columns(3).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyalt")
        Me.XPTablaListas.ColumnModel.Columns(4).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "monthlyaltinter")
        Me.XPTablaListas.ColumnModel.Columns(5).Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_TABLE, "coemonthly")
        Me.XPTablaListas.EndUpdate()

        If (Me.lstBoxInformes.Items.Count > 0) Then
            Me.RellenarInformes()
        End If

        Dim myBuildInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
        Me.Text &= " - v" & myBuildInfo.FileMajorPart & "." & myBuildInfo.FileMinorPart

    End Sub

#End Region

#Region "Acciones sobre los proyectos"
    Private Sub cbProyectos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbProyectos.SelectedIndexChanged

        Me.lblProyectoDesc.Text = ""
        Me._id_proy_selec = Me._rellenar.RellenarProyectosDesc(Me.lblProyectoDesc, Me.cbProyectos.SelectedIndex)
        Me._rellenar.RellenarPuntos(Me.lstboxPuntos, Me._id_proy_selec)
        Me.lstboxPuntos_SelectedIndexChanged(Nothing, Nothing)
        ' Borro la tabla
        Me.XPTablaListas.TableModel.Rows.Clear()

        Me.cmbListaAlteradasDiarias_SelectedIndexChanged(Nothing, Nothing)

    End Sub
#End Region

#Region "Acciones sobre puntos"
    Private Sub lstboxPuntos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstboxPuntos.SelectedIndexChanged

        Dim id_punto As Long
        Dim nombre As String
        Dim mesInicio As Integer
        Dim nListas As Integer
        Dim nAlt As Integer
        Dim ds As DataSet
        Dim dr As DataRow
        Dim clavePunto As String

        If (Me.lstboxPuntos.SelectedItem <> Nothing) Then
            ' Clave del punto
            clavePunto = Me.lstboxPuntos.SelectedItem.ToString()

            Me._PtoSeleccionado = clavePunto

            ' Sacar nombre y id
            ds = Me._cMDB.RellenarDataSet("Puntos", "SELECT id_punto, nombre, mesInicio FROM Punto WHERE Clave_Punto='" & clavePunto & "'")
            dr = ds.Tables(0).Rows(0)
            id_punto = dr("id_punto")
            nombre = dr("nombre")
            mesInicio = dr("mesInicio")

            Me._strPto = Me._PtoSeleccionado & "-" & nombre

            ' Sacar el numero de alteraciones
            ds = Me._cMDB.RellenarDataSet("Alt", "SELECT DISTINCT id_alteracion FROM [alteracion] WHERE id_alteracion > 0 AND id_punto=" & id_punto)
            nAlt = ds.Tables(0).Rows.Count

            ' Sacar el numero de listas
            ds = Me._cMDB.RellenarDataSet("Listas", "Select Count(*) FROM [Lista] WHERE id_punto=" & id_punto & "")
            dr = ds.Tables(0).Rows(0)
            nListas = Integer.Parse(dr(0))

            ' Rellenar nombres de las series en el combo lista
            Dim hayDiaria, hayMensual As Boolean
            Dim stNone As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone")

            Me._rellenar.RellenarListas(hayDiaria, hayMensual, Me.cmbListaAlteradasDiarias, id_punto, stNone)


            Dim stSI As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "yes").ToUpper()
            Dim stNO As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "no").ToUpper()

            If (hayDiaria) Then
                Me.lblSerieNatDiaria.Text = stSI
            Else
                Me.lblSerieNatDiaria.Text = stNO
            End If
            If (hayMensual) Then
                Me.lblSerieNatMensual.Text = stSI
            Else
                Me.lblSerieNatMensual.Text = stNO
            End If

            Me.lblPuntosClave.Text = clavePunto
            Me.lblPuntosNombre.Text = nombre
            Me.lblPuntosNListas.Text = nAlt
            Me.cmbMesInicio.SelectedIndex = mesInicio - 1

            Me._simulacion.mesInicio = mesInicio

        Else
            Me.lblSerieNatDiaria.Text = "--"
            Me.lblSerieNatMensual.Text = "--"
            Me.lblPuntosClave.Text = ""
            Me.lblPuntosNombre.Text = ""
            Me.lblPuntosNListas.Text = ""
            Me.cmbListaAlteradasDiarias.Items.Clear()
        End If

    End Sub
#End Region

#Region "Acciones sobre listas"
    Private Sub chkboxUsarCoe_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkboxUsarCoe.CheckedChanged
        Dim ordD As Integer
        Dim idpunto As Integer
        Dim idAlteracion As Integer
        Dim mesInicio As Integer
        Dim ds As DataSet
        Dim dr As DataRow

        Dim stNone As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone")

        If (Me.cmbListaAlteradasDiarias.SelectedItem() <> stNone) Then
            ds = Me._cMDB.RellenarDataSet("Listas", "Select id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" & Me.cmbListaAlteradasDiarias.SelectedItem.ToString() & "'")
            dr = ds.Tables(0).Rows(0)
            idAlteracion = dr(0)
        Else
            idAlteracion = -1
        End If

        If (Me._PtoSeleccionado <> Nothing) Then
            ds = Me._cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Clave_punto='" & Me._PtoSeleccionado & "'")
            dr = ds.Tables(0).Rows(0)
            idpunto = dr("id_punto")
            mesInicio = dr("mesInicio")
        Else
            ordD = -1
        End If

        ' Rellenar la tabla con los años y los totales
        Me._simulacion = Me._rellenar.RellenarXPTable(Me.XPTablaListas, idAlteracion, idpunto, Me.chkboxUsarCoe.Checked, Me.chkboxUsarCoeDiaria.Checked)
        Me._simulacion.mesInicio = mesInicio
        Me.lblAñosHidro.Text = (Me._simulacion.fechaFIN - Me._simulacion.fechaINI).ToString()
        Me.lblAñosNatDiario.Text = Me._simulacion.listas(0).nValidos.ToString()
        Me.lblAñosAltDiario.Text = Me._simulacion.listas(1).nValidos.ToString()
        Me.lblAñosCoeDiaria.Text = Me._simulacion.coe(0).nCoetaneos.ToString()

        If (Not Me._simulacion.añosInterNat Is Nothing) Then
            Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos + Me._simulacion.añosInterNat.Length).ToString()
        Else
            Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos).ToString()
        End If
        If (Not Me._simulacion.añosInterAlt Is Nothing) Then
            Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos + Me._simulacion.añosInterAlt.Length).ToString()
        Else
            Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos).ToString()
        End If
        If (Not Me._simulacion.añosInterCoe Is Nothing) Then
            Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos + Me._simulacion.añosInterCoe.Length).ToString()
        Else
            Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos).ToString()
        End If

        ' Cambiar colores
        If Integer.Parse(Me.lblAñosNatMensual.Text) < 15 And Integer.Parse(Me.lblAñosNatMensual.Text) <> 0 Then
            Me.lblAñosNatMensual.ForeColor = Color.Red
        Else
            Me.lblAñosNatMensual.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosAltMensual.Text) < 15 And Integer.Parse(Me.lblAñosAltMensual.Text) <> 0 Then
            Me.lblAñosAltMensual.ForeColor = Color.Red
        Else
            Me.lblAñosAltMensual.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosNatDiario.Text) < 15 And Integer.Parse(Me.lblAñosNatDiario.Text) <> 0 Then
            Me.lblAñosNatDiario.ForeColor = Color.Red
        Else
            Me.lblAñosNatDiario.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosAltDiario.Text) < 15 And Integer.Parse(Me.lblAñosAltDiario.Text) <> 0 Then
            Me.lblAñosAltDiario.ForeColor = Color.Red
        Else
            Me.lblAñosAltDiario.ForeColor = Color.Black
        End If
        Me.lblAñosNatDiarioUSO.Text = Me._simulacion.añosParaCalculo(0).nAños
        Me.lblAñosNatMensualUSO.Text = Me._simulacion.añosParaCalculo(2).nAños
        Me.lblAñosAltDiarioUSO.Text = Me._simulacion.añosParaCalculo(1).nAños
        Me.lblAñosAltMensualUSO.Text = Me._simulacion.añosParaCalculo(3).nAños

        Me.RellenarInformes()

    End Sub
    Private Sub chkboxUsarCoeDiaria_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkboxUsarCoeDiaria.CheckedChanged
        Dim ordD As Integer
        Dim idpunto As Integer
        Dim idAlteracion As Integer
        Dim mesInicio As Integer
        Dim ds As DataSet
        Dim dr As DataRow

        Dim stNone As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone")

        If (Me.cmbListaAlteradasDiarias.SelectedItem() <> stNone) Then
            ds = Me._cMDB.RellenarDataSet("Listas", "Select id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" & Me.cmbListaAlteradasDiarias.SelectedItem.ToString() & "'")
            dr = ds.Tables(0).Rows(0)
            idAlteracion = dr(0)
        Else
            idAlteracion = -1
        End If

        If (Me._PtoSeleccionado <> Nothing) Then
            ds = Me._cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Clave_punto='" & Me._PtoSeleccionado & "'")
            dr = ds.Tables(0).Rows(0)
            idpunto = dr("id_punto")
            mesInicio = dr("mesInicio")
        Else
            ordD = -1
        End If

        ' Rellenar la tabla con los años y los totales
        Me._simulacion = Me._rellenar.RellenarXPTable(Me.XPTablaListas, idAlteracion, idpunto, Me.chkboxUsarCoe.Checked, Me.chkboxUsarCoeDiaria.Checked)
        Me._simulacion.mesInicio = mesInicio
        Me.lblAñosHidro.Text = (Me._simulacion.fechaFIN - Me._simulacion.fechaINI).ToString()
        Me.lblAñosNatDiario.Text = Me._simulacion.listas(0).nValidos.ToString()
        Me.lblAñosAltDiario.Text = Me._simulacion.listas(1).nValidos.ToString()
        Me.lblAñosCoeDiaria.Text = Me._simulacion.coe(0).nCoetaneos.ToString()
        If (Not Me._simulacion.añosInterNat Is Nothing) Then
            Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos + Me._simulacion.añosInterNat.Length).ToString()
        Else
            Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos).ToString()
        End If
        If (Not Me._simulacion.añosInterAlt Is Nothing) Then
            Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos + Me._simulacion.añosInterAlt.Length).ToString()
        Else
            Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos).ToString()
        End If
        If (Not Me._simulacion.añosInterCoe Is Nothing) Then
            Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos + Me._simulacion.añosInterCoe.Length).ToString()
        Else
            Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos).ToString()
        End If
        Me.lblAñosNatDiarioUSO.Text = Me._simulacion.añosParaCalculo(0).nAños
        Me.lblAñosNatMensualUSO.Text = Me._simulacion.añosParaCalculo(2).nAños
        Me.lblAñosAltDiarioUSO.Text = Me._simulacion.añosParaCalculo(1).nAños
        Me.lblAñosAltMensualUSO.Text = Me._simulacion.añosParaCalculo(3).nAños

        ' Cambiar colores
        If Integer.Parse(Me.lblAñosNatMensual.Text) < 15 And Integer.Parse(Me.lblAñosNatMensual.Text) <> 0 Then
            Me.lblAñosNatMensual.ForeColor = Color.Red
        Else
            Me.lblAñosNatMensual.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosAltMensual.Text) < 15 And Integer.Parse(Me.lblAñosAltMensual.Text) <> 0 Then
            Me.lblAñosAltMensual.ForeColor = Color.Red
        Else
            Me.lblAñosAltMensual.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosNatDiario.Text) < 15 And Integer.Parse(Me.lblAñosNatDiario.Text) <> 0 Then
            Me.lblAñosNatDiario.ForeColor = Color.Red
        Else
            Me.lblAñosNatDiario.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosAltDiario.Text) < 15 And Integer.Parse(Me.lblAñosAltDiario.Text) <> 0 Then
            Me.lblAñosAltDiario.ForeColor = Color.Red
        Else
            Me.lblAñosAltDiario.ForeColor = Color.Black
        End If

        Me.RellenarInformes()

    End Sub
    Private Sub cmbListaAlteradasDiarias_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbListaAlteradasDiarias.SelectedIndexChanged

        Dim sItem As String
        Dim nAlt As Integer

        Dim ds As DataSet
        Dim dr As DataRow

        If (Me.cmbListaAlteradasDiarias.SelectedIndex = -1) Then
            Me.btnCalcular.Enabled = False
            Me.lblCodigoAlt.Text = "---"
            Me.lblIDDiaria.Text = "---"
            Me.lblIDMensual.Text = "---"
            Me._strAlt = ""
            Me.lblAñosHidro.Text = ""
            Me.lblAñosNatDiario.Text = ""
            Me.lblAñosAltDiario.Text = ""
            Me.lblAñosCoeDiaria.Text = ""
            Me.lblAñosNatMensual.Text = ""
            Me.lblAñosAltMensual.Text = ""
            Me.lblAñosCoeMensual.Text = ""
            Me.lblAñosNatDiarioUSO.Text = ""
            Me.lblAñosNatMensualUSO.Text = ""
            Me.lblAñosAltDiarioUSO.Text = ""
            Me.lblAñosAltMensualUSO.Text = ""
            Me.chkboxUsarCoe.Enabled = False
            Me.chkboxUsarCoeDiaria.Enabled = False
            Me.lstBoxInformes.Items.Clear()
            Return
        End If

        sItem = Me.cmbListaAlteradasDiarias.SelectedItem.ToString()

        Me._AltSeleccionada = sItem

        Dim stNone As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNone")

        If (sItem <> stNone) Then

            Dim COD As String

            'sAux = sItem.Split(" ")
            'nAlt = Integer.Parse(sAux(sAux.Length - 1))

            ' Saco el codigo de la alteracion
            ds = Me._cMDB.RellenarDataSet("Alt", "SELECT nombre, id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" & sItem & "'")
            dr = ds.Tables(0).Rows(0)

            COD = sItem

            Me.lblCodigoAlt.Text = dr(0)

            Me._strAlt = sItem & "-" & dr(0)

            nAlt = dr(1)

            Dim stSI As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "yes").ToUpper()
            Dim stNO As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "no").ToUpper()

            ds = Me._cMDB.RellenarDataSet("Alt", "SELECT Nombre FROM [Lista] WHERE ID_Alteracion=" & nAlt & " AND Tipo_fechas=True")
            If (ds.Tables(0).Rows.Count > 0) Then
                Me.lblIDDiaria.Text = stSI
            Else
                Me.lblIDDiaria.Text = stNO
            End If

            ds = Me._cMDB.RellenarDataSet("Alt", "SELECT Nombre FROM [Lista] WHERE ID_Alteracion=" & nAlt & " AND Tipo_fechas=false")
            If (ds.Tables(0).Rows.Count > 0) Then
                Me.lblIDMensual.Text = stSI
            Else
                Me.lblIDMensual.Text = stNO
            End If


        Else
            Me.lblCodigoAlt.Text = "---"
            Me.lblIDDiaria.Text = "---"
            Me.lblIDMensual.Text = "---"
            Me._strAlt = ""
        End If

        ' ++++++++++++++++++++++++++++++++++++++++++
        ' +++++++ Rellenar la XP table +++++++++++++
        ' ++++++++++++++++++++++++++++++++++++++++++
        Dim ordD As Integer
        Dim idpunto As Integer
        Dim idAlteracion As Integer
        Dim mesInicio As Integer

        If (Me.cmbListaAlteradasDiarias.SelectedItem() <> stNone) Then
            ds = Me._cMDB.RellenarDataSet("Listas", "Select id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" & Me.cmbListaAlteradasDiarias.SelectedItem.ToString() & "'")
            dr = ds.Tables(0).Rows(0)
            idAlteracion = dr(0)
        Else
            idAlteracion = -1
        End If

        If (Me._PtoSeleccionado <> Nothing) Then
            ds = Me._cMDB.RellenarDataSet("Listas", "Select id_punto, mesInicio FROM [Punto] WHERE Clave_punto='" & Me._PtoSeleccionado & "'")
            dr = ds.Tables(0).Rows(0)
            idpunto = dr("id_punto")
            mesInicio = dr("mesInicio")
        Else
            ordD = -1
        End If

        ' Rellenar la tabla con los años y los totales
        Me._simulacion = Me._rellenar.RellenarXPTable(Me.XPTablaListas, idAlteracion, idpunto, Me.chkboxUsarCoe.Checked, Me.chkboxUsarCoeDiaria.Checked)
        Me._simulacion.mesInicio = mesInicio
        Me.lblAñosHidro.Text = (Me._simulacion.fechaFIN - Me._simulacion.fechaINI).ToString()
        Me.lblAñosNatDiario.Text = Me._simulacion.listas(0).nValidos.ToString()
        Me.lblAñosAltDiario.Text = Me._simulacion.listas(1).nValidos.ToString()
        Me.lblAñosCoeDiaria.Text = Me._simulacion.coe(0).nCoetaneos.ToString()
        If (Not Me._simulacion.añosInterNat Is Nothing) Then
            Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos + Me._simulacion.añosInterNat.Length).ToString()
        Else
            Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos).ToString()
        End If
        If (Not Me._simulacion.añosInterAlt Is Nothing) Then
            Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos + Me._simulacion.añosInterAlt.Length).ToString()
        Else
            Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos).ToString()
        End If
        If (Not Me._simulacion.añosInterCoe Is Nothing) Then
            Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos + Me._simulacion.añosInterCoe.Length).ToString()
        Else
            Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos).ToString()
        End If
        Me.lblAñosNatDiarioUSO.Text = Me._simulacion.añosParaCalculo(0).nAños
        Me.lblAñosNatMensualUSO.Text = Me._simulacion.añosParaCalculo(2).nAños
        Me.lblAñosAltDiarioUSO.Text = Me._simulacion.añosParaCalculo(1).nAños
        Me.lblAñosAltMensualUSO.Text = Me._simulacion.añosParaCalculo(3).nAños

        ' Cambiar colores
        If Integer.Parse(Me.lblAñosNatMensual.Text) < 15 And Integer.Parse(Me.lblAñosNatMensual.Text) <> 0 Then
            Me.lblAñosNatMensual.ForeColor = Color.Red
        Else
            Me.lblAñosNatMensual.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosAltMensual.Text) < 15 And Integer.Parse(Me.lblAñosAltMensual.Text) <> 0 Then
            Me.lblAñosAltMensual.ForeColor = Color.Red
        Else
            Me.lblAñosAltMensual.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosNatDiario.Text) < 15 And Integer.Parse(Me.lblAñosNatDiario.Text) <> 0 Then
            Me.lblAñosNatDiario.ForeColor = Color.Red
        Else
            Me.lblAñosNatDiario.ForeColor = Color.Black
        End If

        If Integer.Parse(Me.lblAñosAltDiario.Text) < 15 And Integer.Parse(Me.lblAñosAltDiario.Text) <> 0 Then
            Me.lblAñosAltDiario.ForeColor = Color.Red
        Else
            Me.lblAñosAltDiario.ForeColor = Color.Black
        End If

        If ((Me._simulacion.coe(1).nCoetaneos >= 15 _
            And Me._simulacion.coe(1).nCoetaneos < Integer.Parse(Me.lblAñosNatMensual.Text))) Then

            Me.chkboxUsarCoe.Enabled = True
            Me.chkboxUsarCoe.Checked = True

        Else
            Me.chkboxUsarCoe.Enabled = False
            If (Me._simulacion.coe(1).nCoetaneos = Integer.Parse(Me.lblAñosNatMensual.Text) And Me._simulacion.coe(1).nCoetaneos > 0) Then
                Me.chkboxUsarCoe.Checked = True
            Else
                Me.chkboxUsarCoe.Checked = False
            End If
        End If

        If (Me._simulacion.coe(0).nCoetaneos >= 15 _
        And Me._simulacion.coe(0).nCoetaneos < Integer.Parse(Me.lblAñosNatDiario.Text)) Then
            Me.chkboxUsarCoeDiaria.Enabled = True
            Me.chkboxUsarCoeDiaria.Checked = False
        Else
            Me.chkboxUsarCoeDiaria.Enabled = False
            If (Me._simulacion.coe(0).nCoetaneos = Integer.Parse(Me.lblAñosNatDiario.Text) And Me._simulacion.coe(0).nCoetaneos > 0) Then
                Me.chkboxUsarCoeDiaria.Checked = True
            Else
                Me.chkboxUsarCoeDiaria.Checked = False
            End If

        End If

        Me.RellenarInformes()

    End Sub
#End Region

#Region "Calculos"
    Private Sub btnCalcular_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalcular.Click

        Me._simulacion.sNombre = Me._strPto
        Me._simulacion.sAlteracion = Me._strAlt
        Me._simulacion.añosCoetaneosTotales = Me.lblAñosCoeMensual.Text
        Dim fCalcular As New FormCalculo(Me._simulacion, Me._informes, Me._cMDB)
        Me.Enabled = False

        Me.Cursor = Cursors.WaitCursor

        fCalcular.ShowDialog()
        Me.Enabled = True

        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "Func. Auxiliares"

    Sub RellenarInformes()

        Me.lstBoxInformes.Items.Clear()

        Dim nAñosIntNat As Integer
        Dim nAñosIntAlt As Integer
        Dim okNatComp As Boolean = False
        Dim okAltComp As Boolean = False
        Dim okNatDComp As Boolean = False
        Dim okAltDComp As Boolean = False
        Dim usarCoeMen As Boolean = False
        Dim usarCoeDia As Boolean = False

        Me._informes.inf1 = False
        Me._informes.inf1b = False
        Me._informes.inf2 = False
        Me._informes.inf3 = False
        Me._informes.inf4 = False
        Me._informes.inf4a = False
        Me._informes.inf5 = False
        Me._informes.inf5a = False
        Me._informes.inf5b = False
        Me._informes.inf5c = False
        Me._informes.inf6 = False
        Me._informes.inf6a = False
        Me._informes.inf7d = False
        'Me._informes.inf7_3 = False
        Me._informes.inf7a = False
        Me._informes.inf7b = False
        Me._informes.inf7c = False
        Me._informes.inf8 = False
        Me._informes.inf9 = False

        If (Me._simulacion.añosInterNat Is Nothing) Then
            nAñosIntNat = 0
        Else
            nAñosIntNat = Me._simulacion.añosInterNat.Length
        End If

        If (Me._simulacion.añosInterAlt Is Nothing) Then
            nAñosIntAlt = 0
        Else
            nAñosIntAlt = Me._simulacion.añosInterAlt.Length
            Me._informes.inf8 = True
        End If

        If ((Me._simulacion.listas(2).nValidos + nAñosIntNat) >= 15) Then
            okNatComp = True
            ' apartado 2
            If ((Me._simulacion.listas(3).nValidos + nAñosIntAlt) >= 15) Then
                okAltComp = True
                If (Me._simulacion.usarCoe) Then
                    usarCoeMen = True
                End If
            End If

            ' apartado 3
            If (Me._simulacion.listas(0).nValidos >= 15) Then
                okNatDComp = True
                ' apartado 4
                If (Me._simulacion.listas(1).nValidos >= 15) Then
                    okAltDComp = True
                    If (Me._simulacion.usarCoeDiara) Then
                        usarCoeDia = True
                    End If
                End If
            End If
        End If

        Me.ResetInformes()
        ' Mensuales
        If (okNatComp) Then
            Me.btnCalcular.Enabled = True
            Me._informes.inf1 = True
            Me._informes.inf2 = True
            Me._informes.inf4a = True
            Me._informes.inf9a = True ' Nuevo
            If (okAltComp) Then
                Me._informes.inf8 = True ' Nuevo
                If (usarCoeMen) Then
                    Me._informes.inf1b = True
                    Me._informes.inf3 = True
                    Me._informes.inf5b = True
                    Me._informes.inf7b = True
                    Me._informes.inf8c = True ' Nuevo
                Else
                    Me._informes.inf4b = True ' Nuevo
                    Me._informes.inf5a = True
                    Me._informes.inf7c = True
                    Me._informes.inf8d = True ' Nuevo
                End If
            End If
            ' Diarios
            If (okNatDComp) Then
                Me._informes.inf4 = True
                Me._informes.inf4a = False
                Me._informes.inf6a = True
                Me._informes.inf9a = False
                Me._informes.inf9 = True
                If (okAltDComp) Then

                    Me._informes.inf6a = False
                    Me._informes.inf6 = True
                    Me._informes.inf7d = True
                    'Me._informes.inf5b = False

                    If (Me._informes.inf5a) Then
                        Me._informes.inf5a = False
                        Me._informes.inf5c = True
                    End If
                    If (Me._informes.inf5b) Then
                        Me._informes.inf5b = False
                        Me._informes.inf5 = True
                    End If
                    If (Me._informes.inf7b) Then
                        Me._informes.inf7b = False
                        Me._informes.inf7a = True
                    End If
                    If (Me._informes.inf8c) Then
                        Me._informes.inf8c = False
                        Me._informes.inf8a = True
                    End If
                    If (Me._informes.inf8d) Then
                        Me._informes.inf8d = False
                        Me._informes.inf8b = True
                    End If

                End If
            End If

            'If (okNatDComp) Then
            '    'Me.ResetInformes()
            '    Me._informes.inf1 = True
            '    Me._informes.inf2 = True
            '    Me._informes.inf4 = True
            '    Me._informes.inf6a = True
            '    Me._informes.inf9 = True
            '    If (okAltDComp) Then
            '        Me._informes.inf6a = False
            '        Me._informes.inf6 = True
            '        Me._informes.inf7d = True
            '        Me._informes.inf8 = True
            '        If (usarCoeDia) Then
            '            Me._informes.inf1b = True
            '            Me._informes.inf3 = True
            '            Me._informes.inf5 = True
            '            Me._informes.inf7a = True
            '            Me._informes.inf8a = True
            '        Else
            '            Me._informes.inf4b = True
            '            Me._informes.inf5c = True
            '            Me._informes.inf7c = True
            '            Me._informes.inf8b = True
            '        End If
            '    End If
            'End If
        Else
            Me.btnCalcular.Enabled = False
        End If

        If (Me._informes.inf1) Then
            'Me.lstBoxInformes.Items.Add("INFORME 1: VARIABILIDAD INTERANUAL RÉGIMEN NATURAL")
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1"))
        End If
        If (Me._informes.inf1b) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1a"))
        End If
        If (Me._informes.inf2) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe2"))
        End If
        If (Me._informes.inf3) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe3"))
        End If
        If (Me._informes.inf4) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4"))
        End If
        If (Me._informes.inf4a) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4a"))
        End If
        If (Me._informes.inf4b) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4b"))
        End If
        If (Me._informes.inf5) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5"))
        End If
        If (Me._informes.inf5a) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5a"))
        End If
        If (Me._informes.inf5b) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5b"))
        End If
        If (Me._informes.inf5c) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5c"))
        End If
        If (Me._informes.inf6) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6"))
        End If
        If (Me._informes.inf6a) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6a"))
        End If
        If (Me._informes.inf7a) Then
            'Me.lstBoxInformes.Items.Add("INFORME 7a: ÍNDICES HABITUALES POR TIPOS")
            'ElseIf (Me._informes.inf7a And Me._informes.inf7_3) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7a"))
        End If
        If (Me._informes.inf7b) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7b"))
            'ElseIf (Me._informes.inf7b And Me._informes.inf7_3) Then
            'Me.lstBoxInformes.Items.Add("INFORME 7b: ÍNDICES HABITUALES + INDICE VARIABILIDAD")
        End If
        If (Me._informes.inf7c) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7c"))
        End If
        If (Me._informes.inf7d) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7d"))
        End If
        If (Me._informes.inf8) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8"))
        End If
        If (Me._informes.inf8a) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8a"))
        End If
        If (Me._informes.inf8b) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8b"))
        End If
        If (Me._informes.inf8c) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8c"))
        End If
        If (Me._informes.inf8d) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8d"))
        End If
        If (Me._informes.inf9) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9"))
        End If
        If (Me._informes.inf9a) Then
            Me.lstBoxInformes.Items.Add(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9a"))
        End If
    End Sub

    Sub ResetInformes()
        ' Reset de los informes por seguridad
        Me._informes.inf1 = False
        Me._informes.inf1b = False
        Me._informes.inf2 = False
        Me._informes.inf3 = False
        Me._informes.inf4 = False
        Me._informes.inf4a = False
        Me._informes.inf4b = False
        Me._informes.inf5 = False
        Me._informes.inf5a = False
        Me._informes.inf5b = False
        Me._informes.inf5c = False
        Me._informes.inf6 = False
        Me._informes.inf6a = False
        Me._informes.inf7a = False
        Me._informes.inf7b = False
        Me._informes.inf7c = False
        Me._informes.inf7d = False
        Me._informes.inf8 = False
        Me._informes.inf8a = False
        Me._informes.inf8b = False
        Me._informes.inf8c = False
        Me._informes.inf8d = False
        Me._informes.inf9 = False
        Me._informes.inf9a = False
    End Sub
    Sub ResetearFormPrincipal()
        Me.XPTablaListas.TableModel.Rows.Clear()
        Me.lstboxPuntos.SelectedIndex = -1
        Me.cmbListaAlteradasDiarias.Items.Clear()
        Me.chkboxUsarCoe.Enabled = False
        Me.chkboxUsarCoeDiaria.Enabled = False
        Me.btnCalcular.Enabled = False
        Me.lstBoxInformes.Items.Clear()
        Me.cbProyectos.Items.Clear()
    End Sub
    Private Function GetVersionBBDD() As Integer
        Dim sVer As String

        Try
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Conf", "SELECT * FROM [Punto]")
        Catch ex As Exception
            Return -1
        End Try

        Try
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Conf", "SELECT * FROM [Configuracion]")
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            sVer = dr("version")
        Catch ex As Exception
            sVer = 1
        End Try
        Return sVer
    End Function
    Private Function ValidarVersionBBDD(ByRef message As String) As TIPO_BBDD
        Select Case (Me.GetVersionBBDD())
            Case 1
                message = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDBv1")
                Return TIPO_BBDD.VERSION_ANT_V1
            Case 2
                message = ""
                Return TIPO_BBDD.VERSION_OK
            Case Else
                message = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDBNotValid")
                Return TIPO_BBDD.VERSION_DESC
        End Select

    End Function

#End Region

#Region "Ayuda"
    'Private Sub AcercaDeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IdiomasToolStripMenuItem.Click
    '    Dim fAcercaDe As New FormAcercaDe()

    '    fAcercaDe.ShowDialog()
    'End Sub

    'Private Sub ManualDeReferenciaDeUsuarioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        System.Diagnostics.Process.Start(".\Manual\Manual Usuario.pdf")
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub ManualDeReferenicaMetodológicaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        System.Diagnostics.Process.Start(".\Manual\Manual Referencia.pdf")
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub ManualesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualesToolStripMenuItem.Click
        Dim sInfo As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo("http://ambiental.cedex.es/caudales-ambientales.php")
        System.Diagnostics.Process.Start(sInfo)
    End Sub
#End Region

    Private Sub cmbMesInicio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMesInicio.SelectedIndexChanged
        Try

            ' 3 Pasos:

            ' 1) Actualizar el mes de inicio del punto
            ' -----------------------------------------
            Dim clavePunto As String = Me.lstboxPuntos.SelectedItem.ToString()
            clavePunto = Me._PtoSeleccionado
            Me._cMDB.EjecutarSQL("UPDATE [Punto] SET mesInicio = " & (Me.cmbMesInicio.SelectedIndex + 1).ToString() & " WHERE Clave_punto ='" & clavePunto & "'")

            ' 2) Actualizar las fechas de inicio y fin de las listas
            ' -------------------------------------------------------
            Dim dtaux As DataTable = Me._cMDB.GetTablaSQL("SELECT ID_punto, mesInicio FROM [Punto] WHERE Clave_punto = '" & clavePunto & "'")
            Dim dtauxLista As DataTable
            Dim idpunto As Integer = dtaux.Rows(0)("ID_punto")
            Dim mesInicio As Integer = dtaux.Rows(0)("mesInicio")
            dtaux = Me._cMDB.GetTablaSQL("SELECT * FROM [Lista] WHERE ID_Punto = " & idpunto)

            Dim dr As DataRow
            For i As Integer = 0 To dtaux.Rows.Count - 1
                dr = dtaux.Rows(i)

                Dim idLista As Integer = dr("ID_Lista")
                dtauxLista = Me._cMDB.GetTablaSQL("SELECT TOP 1 Fecha FROM [Valor] WHERE ID_Lista = " & idLista & " ORDER BY Fecha DESC")
                Dim fechaFIN As Date = Date.Parse(dtauxLista.Rows(0)(0))
                dtauxLista = Me._cMDB.GetTablaSQL("SELECT TOP 1 Fecha FROM [Valor] WHERE ID_Lista = " & idLista & " ORDER BY Fecha ASC")
                Dim fechaINI As Date = Date.Parse(dtauxLista.Rows(0)(0))

                If (fechaINI.Month <> mesInicio Or fechaINI.Day <> 1) Then
                    If (fechaINI.Month < mesInicio) Then
                        fechaINI = New Date(fechaINI.Year - 1, mesInicio, 1)
                    Else
                        fechaINI = New Date(fechaINI.Year, mesInicio, 1)
                    End If
                End If

                Dim mesFin As Integer
                mesFin = mesInicio - 1
                If (mesFin <= 0) Then
                    mesFin = 12
                End If

                Dim diaFin As Integer

                If (fechaFIN.Month <> mesFin) Then
                    If (fechaFIN.Month > mesFin) Then
                        diaFin = Date.DaysInMonth(fechaFIN.Year + 1, mesFin)
                        fechaFIN = New Date(fechaFIN.Year + 1, mesFin, diaFin)
                    Else
                        diaFin = Date.DaysInMonth(fechaFIN.Year, mesFin)
                        fechaFIN = New Date(fechaFIN.Year, mesFin, diaFin)
                    End If
                    'fechaFIN = fechaINI.AddYears(1)
                    'fechaFIN = fechaFIN.AddDays(-1)
                End If

                Me._cMDB.EjecutarSQL("UPDATE [Lista] SET Fecha_Ini = #" & fechaINI.ToString("yyyy-MM-dd") & "#, Fecha_Fin = #" & fechaFIN.ToString("yyyy-MM-dd") & "# WHERE ID_Lista =" & idLista)


            Next

            ' 3) Refrescar la XPTable
            ' -----------------------
            Me.lstboxPuntos_SelectedIndexChanged(Nothing, Nothing)
            'Me.lstboxPuntos.SelectedIndex = Me.lstboxPuntos.SelectedIndex

        Catch ex As Exception

        End Try
    End Sub

    
End Class

