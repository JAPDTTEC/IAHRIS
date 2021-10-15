Public Class FormImportar

    Dim _rutaMDB As String
    Dim _cMDBImportar As BBDD.OleDbDataBase
    Dim _cMDB As BBDD.OleDbDataBase
    Dim _sError As String = ""
    Dim _rellenarImportar As Rellenar.RellenarForm
    Dim _rellenar As Rellenar.RellenarForm

    Dim _id_proy_selec As Integer
    Dim _id_proy_selec_punto As Integer

    Private _traductor As MultiLangXML.MultiIdiomasXML

    Private Enum TIPO_BBDD
        VERSION_OK
        VERSION_ANT_V1
        VERSION_DESC
    End Enum

    Public Sub New(ByVal MDB As BBDD.OleDbDataBase)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me._cMDB = MDB

        ' -------------------------------------
        ' ---- Traducir formulario ------------
        ' -------------------------------------
        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")

        

    End Sub

    Private Sub btnExaminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExaminar.Click
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.Filter = "Base de datos (*.mdb)|*.mdb"
        openFileDialog1.FilterIndex = 0
        openFileDialog1.RestoreDirectory = True

        Me.cmbProyectos.Items.Clear()
        Me.chklstPuntos.Items.Clear()

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            Me._rutaMDB = openFileDialog1.FileName
            Me._cMDBImportar = New BBDD.OleDbDataBase("BaseImportar", Me._rutaMDB)
            Me.txtRuta.Text = Me._rutaMDB

            Me._rellenarImportar = New Rellenar.RellenarForm(Me._cMDBImportar)
            Me._rellenar = New Rellenar.RellenarForm(Me._cMDB)

            Try
                Dim tipobbdd As TIPO_BBDD
                tipobbdd = ValidarVersionBBDD(Me._sError)

                If (tipobbdd = TIPO_BBDD.VERSION_DESC) Then
                    MessageBox.Show(Me._sError, _
                                    Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                ElseIf (tipobbdd = TIPO_BBDD.VERSION_OK) Then
                    ' Importar por proyectos/puntos
                    Me._rellenarImportar.RellenarProyectos(Me.cmbProyectos)
                    Me._rellenar.RellenarProyectos(Me.cmbProyPuntos)
                    Me.gbProyectos.Enabled = True
                    Me.gbPuntos.Enabled = True
                ElseIf (tipobbdd = TIPO_BBDD.VERSION_ANT_V1) Then
                    ' Importar por puntos
                    ' Se deshabilita los proyectos ya que no existen en la versión 1
                    Me.gbProyectos.Enabled = False
                    Me.gbPuntos.Enabled = True
                    MessageBox.Show(Me._sError, "Actualizar", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me._rellenarImportar.RellenarPuntos(Me.chklstPuntos)
                    Me._rellenar.RellenarProyectos(Me.cmbProyPuntos)
                End If

            Catch ex As Exception
                MessageBox.Show(Me._sError & vbCrLf & ex.Message, _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try


        End If


    End Sub

    Private Function GetVersionBBDD() As Integer
        Dim sVer As String

        Try
            Dim ds As DataSet = Me._cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Punto]")
            ds = Me._cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Lista]")
            ds = Me._cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Alteracion]")
            ds = Me._cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Valor]")
        Catch ex As Exception
            Return -1
        End Try

        Try
            Dim ds As DataSet = Me._cMDBImportar.RellenarDataSet("Conf", "SELECT * FROM [Configuracion]")
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            sVer = dr("version")
        Catch ex As Exception
            sVer = 1
        End Try
        Return sVer
    End Function

    ''' <summary>
    ''' Esta función nos dice que tipo de versión tenemos al importar la bbdd
    ''' </summary>
    ''' <param name="message">Mensaje de error o de información</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidarVersionBBDD(ByRef message As String) As TIPO_BBDD
        Select Case (Me.GetVersionBBDD())
            Case 1
                message = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDBv1_2")
                Return TIPO_BBDD.VERSION_ANT_V1
            Case 2
                message = ""
                Return TIPO_BBDD.VERSION_OK
            Case Else
                message = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDBNotValid")
                Return TIPO_BBDD.VERSION_DESC
        End Select

    End Function

    Private Sub FormImportar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.gbPuntos.Enabled = False
        Me.gbProyectos.Enabled = False

        Me.PictureBox1.Visible = False
        Application.DoEvents()

    End Sub

    Private Sub cmbProyectos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbProyectos.SelectedIndexChanged
        Me._id_proy_selec = Me._rellenarImportar.RellenarProyectosDesc(Nothing, Me.cmbProyectos.SelectedIndex)
        Me._rellenarImportar.RellenarPuntos(Me.chklstPuntos, Me._id_proy_selec)
    End Sub

    Private Sub btnImportarProyecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportarProyecto.Click

        If (Me.cmbProyectos.SelectedIndex = -1) Then
            Return
        End If

        Me.Cursor = Cursors.WaitCursor
        
        Me._cMDB.ComenzarTransaccion()

        ' Deshabilitar formulario
        'Me.Enabled = False
        Me.btnExaminar.Enabled = False
        Me.gbProyectos.Enabled = False
        Me.gbPuntos.Enabled = False
        Me.PictureBox1.Visible = True



        Dim ds As DataSet = Me._cMDB.RellenarDataSet("Proyectos", "SELECT COUNT(*) FROM [Proyecto] WHERE nombre=""" & Me.cmbProyectos.SelectedItem.ToString() & """")
        Dim nombreProy As String = Me.cmbProyectos.SelectedItem.ToString()

        ' Comprobar que el proyecto no tiene el nombre de otro ya en la bbdd
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If (dr(0) <> 0) Then
            Dim newName As String = InputBox(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNameProject"), _
                                    Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), _
                                    Me.cmbProyectos.SelectedItem & "_" & DateTime.Now().ToShortDateString())

            If (newName = "") Then
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strInvalidName"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me._cMDB.TerminarTransaccion(False)
                Me.btnExaminar.Enabled = True
                Me.gbProyectos.Enabled = True
                Me.gbPuntos.Enabled = True
                Me.PictureBox1.Visible = False
                Me.Cursor = Cursors.Default
                Return
            End If

            nombreProy = newName
        End If

        Me.lbInfo.Items.Add("[INFO] Importando de " & Me._rutaMDB)
        Me.lbInfo.Items.Add(vbTab & "el proyecto " & nombreProy)
        Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
        Application.DoEvents()

        ' Insertar el proyecto
        ' --------------------
        ds = Me._cMDBImportar.RellenarDataSet("Proyectos", "SELECT ID_Proyecto, nombre, descripcion FROM [Proyecto] WHERE nombre=""" & Me.cmbProyectos.SelectedItem.ToString() & """")
        dr = ds.Tables(0).Rows(0)
        Dim ok As Boolean = Me._cMDB.InsertarRegistro("Proyecto", New String() {"nombre", "descripcion"}, New String() {nombreProy, dr("descripcion")})
        If (Not ok) Then
            Me.lbInfo.Items.Add("[ERROR] El nombre del proyecto ya existe en la base de datos")
            Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
            Me.btnExaminar.Enabled = True
            Me.gbProyectos.Enabled = True
            Me.gbPuntos.Enabled = True
            Me.PictureBox1.Visible = False
            Me._cMDB.TerminarTransaccion(False)
            Me.Cursor = Cursors.Default
            Return
        End If
        Dim idproyectoImportar As Integer = dr("ID_Proyecto")

        ' Insertar los puntos
        ' -------------------
        ' Sacar el id de proyecto del nuevo proyecto
        ds = Me._cMDB.RellenarDataSet("Proyectos", "SELECT TOP 1 ID_Proyecto FROM [Proyecto] ORDER BY ID_Proyecto DESC")
        dr = ds.Tables(0).Rows(0)
        Dim idproyecto As Integer = dr("ID_Proyecto")

        Me.lbInfo.Items.Add("[INFO] Proyecto importado con id " & idproyecto)
        Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
        Application.DoEvents()

        ds = Me._cMDBImportar.RellenarDataSet("Puntos", "SELECT * FROM [Punto] WHERE ID_Proyecto=" & idproyectoImportar)

        Dim dsPunto As DataSet
        Dim dsListaImportar As DataSet
        Dim dsLista As DataSet
        Dim dsAlteracionImportar As DataSet
        Dim dsAlteracion As DataSet
        Dim dsValorImportar As DataSet

        Dim camposPunto() As String = New String() {"Clave_punto", "Nombre", "ID_proyecto", "mesInicio"}
        Dim camposLista() As String = New String() {"Tipo_Lista", "Nombre", "Tipo_fechas", "Fecha_Ini", "Fecha_Fin", "Formato_Fecha", "ID_Punto", "ID_Alteracion"}
        Dim camposAlteracion() As String = New String() {"COD_Alteracion", "Nombre", "ID_Punto"}
        Dim camposValor() As String = New String() {"ID_Lista", "Valor", "Fecha"}
        Dim valoresPunto() As String
        Dim valoresAlteracion() As String
        Dim valoresLista() As String
        Dim valoresValor()() As String = Nothing

        Dim clavePuntoImportar As String
        Dim idPuntoImportar As String
        Dim idPunto As String
        Dim claveAlteracion As String
        Dim idAlteracion As String
        Dim idLista As String
        Dim idListaImportar As String


        Me.lbInfo.Items.Add("[INFO] Se importan " & ds.Tables(0).Rows.Count & " puntos asociados al proyecto")
        Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
        Application.DoEvents()

        ' ¿Como meter la información asociada?:
        ' Se mete un punto -> luego una lista -> si es alteracion -> luego valores
        '                  -> Volvemos a empezar
        For Each row As DataRow In ds.Tables(0).Rows
            ' ¿El punto ya tiene un nombre igual al que se va ha insertar?
            dsPunto = Me._cMDB.RellenarDataSet("Punto", "SELECT Count(*) FROM [Punto] WHERE Clave_Punto=""" & row("Clave_punto") & """")

            clavePuntoImportar = row("Clave_punto")
            dr = dsPunto.Tables(0).Rows(0)
            If (dr(0) <> 0) Then
                clavePuntoImportar &= "_" & idproyecto
            End If

            

            valoresPunto = New String() {clavePuntoImportar, row("Nombre"), idproyecto.ToString(), "1"}
            ok = Me._cMDB.InsertarRegistro("Punto", camposPunto, valoresPunto)
            If (Not ok) Then
                Me.lbInfo.Items.Add("[ERROR] Fallo al insertar el punto.")
                Me._cMDB.TerminarTransaccion(False)
                Me.btnExaminar.Enabled = True
                Me.gbProyectos.Enabled = True
                Me.gbPuntos.Enabled = True
                Me.PictureBox1.Visible = False
                Me.Cursor = Cursors.Default
                Return
            End If

            dsPunto = Me._cMDB.RellenarDataSet("Proyectos", "SELECT TOP 1 ID_Punto FROM [Punto] ORDER BY ID_Punto DESC")
            dr = dsPunto.Tables(0).Rows(0)
            idPunto = dr("ID_Punto")
            idPuntoImportar = row("ID_Punto")

            Me.lbInfo.Items.Add("[INFO] Punto " & clavePuntoImportar & " esta siendo importado")
            Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
            Application.DoEvents()

            ' Importar las listas
            ' Tengo que comprobar si es o no una alteracion
            dsListaImportar = Me._cMDBImportar.RellenarDataSet("Lista", "SELECT * FROM Lista WHERE ID_Punto=" & idPuntoImportar & " ORDER BY ID_Lista DESC")

            Me.lbInfo.Items.Add("[INFO] Punto " & clavePuntoImportar & " tiene asociadas " & dsListaImportar.Tables(0).Rows.Count & " series")
            Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
            Application.DoEvents()

            ' Sacar el mes de inicio de simulación que será el usado en la simulación 
            Dim fechaIni As Date = Date.Parse(dsListaImportar.Tables(0).Rows(0)("Fecha_Ini"))

            ok = Me._cMDB.EjecutarSQL("UPDATE [Punto] SET mesInicio=" & fechaIni.Month & " WHERE ID_Punto=" & idPunto)
            If (ok) Then
            Else
                Me.lbInfo.Items.Add("[ERROR] Al actualizar el mes de inicio de año del punto.")
                Me._cMDB.TerminarTransaccion(False)
                Me.btnExaminar.Enabled = True
                Me.gbProyectos.Enabled = True
                Me.gbPuntos.Enabled = True
                Me.PictureBox1.Visible = False
                Me.Cursor = Cursors.Default
                Return
            End If

            For Each rowLista As DataRow In dsListaImportar.Tables(0).Rows

                ' Lista original
                idListaImportar = rowLista("ID_Lista")
                idAlteracion = "0"

                ' Es una alteración o no. Se tiene que meter la alteracion
                If (rowLista("Tipo_lista").ToString().ToLower() <> "true") Then

                    dsAlteracionImportar = Me._cMDBImportar.RellenarDataSet("Alteracion", _
                                   "SELECT * FROM [Alteracion] WHERE ID_Punto=" & idPuntoImportar & " AND ID_Alteracion=" & rowLista("ID_Alteracion"))

                    Dim drAlt As DataRow = dsAlteracionImportar.Tables(0).Rows(0)
                    claveAlteracion = drAlt("COD_Alteracion")

                    dsAlteracion = Me._cMDB.RellenarDataSet("Alteracion", "SELECT Count(*) FROM [Alteracion] WHERE COD_Alteracion=""" & drAlt("COD_Alteracion") & """")

                    If (dsAlteracion.Tables(0).Rows(0)(0) <> 0) Then
                        claveAlteracion &= "_" & idproyecto
                    End If

                    valoresAlteracion = New String() {claveAlteracion, _
                                                      dsAlteracionImportar.Tables(0).Rows(0)("Nombre"), _
                                                      idPunto}

                    ok = Me._cMDB.InsertarRegistro("Alteracion", camposAlteracion, valoresAlteracion)
                    If (Not ok) Then
                        Me.lbInfo.Items.Add("[ERROR] Fallo al insertar la alteración.")
                        Me._cMDB.TerminarTransaccion(False)
                        Me.btnExaminar.Enabled = True
                        Me.gbProyectos.Enabled = True
                        Me.gbPuntos.Enabled = True
                        Me.PictureBox1.Visible = False
                        Me.Cursor = Cursors.Default
                        Return
                    End If

                    dsAlteracion = Me._cMDB.RellenarDataSet("Alteracion", "SELECT TOP 1 ID_Alteracion FROM [Alteracion] ORDER BY ID_Alteracion DESC")

                    idAlteracion = dsAlteracion.Tables(0).Rows(0)(0)

                    Me.lbInfo.Items.Add("[INFO] La serie que se importa es una serie alterada")
                    Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
                    Application.DoEvents()

                End If

                ' Insertar la lista
                valoresLista = New String() {rowLista("Tipo_lista"), rowLista("Nombre") & idproyecto, rowLista("Tipo_fechas"), _
                                             rowLista("Fecha_Ini"), rowLista("Fecha_Fin"), rowLista("Formato_Fecha"), _
                                             idPunto, idAlteracion}
                ok = Me._cMDB.InsertarRegistro("Lista", camposLista, valoresLista)
                If (Not ok) Then
                    Me.lbInfo.Items.Add("[ERROR] Fallo al insertar la lista.")
                    Me._cMDB.TerminarTransaccion(False)
                    Me.btnExaminar.Enabled = True
                    Me.gbProyectos.Enabled = True
                    Me.gbPuntos.Enabled = True
                    Me.PictureBox1.Visible = False
                    Me.Cursor = Cursors.Default
                    Return
                End If
                dsLista = Me._cMDB.RellenarDataSet("Lista", "SELECT TOP 1 ID_Lista FROM [Lista] ORDER BY ID_Lista DESC")
                idLista = dsLista.Tables(0).Rows(0)(0)

                ' Incluir los valores de la lista
                dsValorImportar = Me._cMDBImportar.RellenarDataSet("Valor", "SELECT * FROM [Valor] WHERE ID_Lista=" & idListaImportar & " ORDER BY ID_Valor ASC")
                valoresValor = Nothing

                Me.lbInfo.Items.Add("[INFO] La serie tiene " & dsValorImportar.Tables(0).Rows.Count & " valores")
                Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
                Application.DoEvents()


                Dim inc As Integer = 0
                Dim resto As Integer = 0

                For Each rowValor As DataRow In dsValorImportar.Tables(0).Rows

                    If (valoresValor Is Nothing) Then
                        ReDim valoresValor(0)
                    Else
                        ReDim Preserve valoresValor(valoresValor.Length)
                    End If
                    valoresValor(valoresValor.Length - 1) = New String() {idLista, rowValor("Valor"), rowValor("Fecha")}


                    inc += 1
                    Math.DivRem(inc, 100, resto)
                    If (resto = 0) Then
                        Me.PictureBox1.Invalidate()
                        Me.PictureBox1.Refresh()
                        Application.DoEvents()
                    End If

                Next

                ok = Me._cMDB.InsertarRegistros("Valor", camposValor, valoresValor)
                If (Not ok) Then
                    Me.lbInfo.Items.Add("[ERROR] Fallo al insertar los valores.")
                    Me._cMDB.TerminarTransaccion(False)
                    Me.btnExaminar.Enabled = True
                    Me.gbProyectos.Enabled = True
                    Me.gbPuntos.Enabled = True
                    Me.PictureBox1.Visible = False
                    Me.Cursor = Cursors.Default
                    Return
                End If
            Next
            Me.lbInfo.Items.Add("[INFO] Punto " & clavePuntoImportar & " importado")
            Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
            Application.DoEvents()
        Next


        Me._cMDB.TerminarTransaccion(True)

        Me.Cursor = Cursors.Default

        Application.DoEvents()

        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strImportProjEnd") & """" & nombreProy & """", _
                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOperationEnd"), _
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
        'Me.Enabled = True

        Me.btnExaminar.Enabled = True
        Me.gbProyectos.Enabled = True
        Me.gbPuntos.Enabled = True
        Me.PictureBox1.Visible = False
        

        Me.Close()

    End Sub

    Private Sub btnImportarPuntos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportarPuntos.Click

        Me.Cursor = Cursors.WaitCursor

        Me._cMDB.ComenzarTransaccion()

        'Me.Enabled = False
        Me.btnExaminar.Enabled = False
        Me.gbProyectos.Enabled = False
        Me.gbPuntos.Enabled = False
        Me.PictureBox1.Visible = True

        Dim ptosSelec As String() = Nothing

        For i As Integer = 0 To Me.chklstPuntos.Items.Count - 1
            If (Me.chklstPuntos.GetItemCheckState(i) = CheckState.Checked) Then
                If (ptosSelec Is Nothing) Then
                    ReDim ptosSelec(0)
                Else
                    ReDim Preserve ptosSelec(ptosSelec.Length)
                End If

                ptosSelec(ptosSelec.Length - 1) = Me.chklstPuntos.Items(i)

            End If
        Next

        If (ptosSelec Is Nothing) Then
            Me.lbInfo.Items.Add("[ERROR] No hay punto seleccionado.")
            Me._cMDB.TerminarTransaccion(False)
            Me.btnExaminar.Enabled = True
            Me.gbProyectos.Enabled = True
            Me.gbPuntos.Enabled = True
            Me.PictureBox1.Visible = False
            Me.Cursor = Cursors.Default
            Return
        End If

        Me._id_proy_selec_punto = Me._rellenar.RellenarProyectosDesc(Nothing, Me.cmbProyPuntos.SelectedIndex)

        Dim ok As Boolean

        Dim camposPunto() As String = New String() {"Clave_punto", "Nombre", "ID_proyecto", "mesInicio"}
        Dim camposLista() As String = New String() {"Tipo_Lista", "Nombre", "Tipo_fechas", "Fecha_Ini", "Fecha_Fin", "Formato_Fecha", "ID_Punto", "ID_Alteracion"}
        Dim camposAlteracion() As String = New String() {"COD_Alteracion", "Nombre", "ID_Punto"}
        Dim camposValor() As String = New String() {"ID_Lista", "Valor", "Fecha"}
        Dim valoresPunto() As String
        Dim valoresAlteracion() As String
        Dim valoresLista() As String
        Dim valoresValor()() As String = Nothing

        Dim dr As DataRow
        Dim drImportar As DataRow

        Dim dsPunto As DataSet
        Dim dsPuntoImportar As DataSet
        Dim dsLista As DataSet
        Dim dsListaImportar As DataSet
        Dim dsAlteracion As DataSet
        Dim dsAlteracionImportar As DataSet
        Dim dsValorImportar As DataSet

        Dim clavePuntoImportar As String
        Dim claveAlteracion As String

        Dim idPunto As Integer
        Dim idPuntoImportar As Integer
        Dim idAlteracion As Integer
        Dim idLista As Integer
        Dim idListaImportar As Integer

        ' Importar cada uno de los puntos
        For Each strPunto As String In ptosSelec
            dsPunto = Me._cMDB.RellenarDataSet("Punto", "SELECT Count(*) FROM [Punto] WHERE Clave_Punto=""" & strPunto & """")
            dsPuntoImportar = Me._cMDBImportar.RellenarDataSet("Punto", "SELECT * FROM [Punto] WHERE Clave_Punto=""" & strPunto & """")

            drImportar = dsPuntoImportar.Tables(0).Rows(0)
            clavePuntoImportar = strPunto

            dr = dsPunto.Tables(0).Rows(0)
            If (dr(0) <> 0) Then
                clavePuntoImportar &= "_" & Me._id_proy_selec_punto
                If (clavePuntoImportar.Length > 12) Then
                    clavePuntoImportar = clavePuntoImportar.Substring(0, 10) & "_" & Me._id_proy_selec_punto
                End If
            End If

            valoresPunto = New String() {clavePuntoImportar, drImportar("Nombre"), Me._id_proy_selec_punto, "1"}
            ok = Me._cMDB.InsertarRegistro("Punto", camposPunto, valoresPunto)
            If (Not ok) Then
                Me.lbInfo.Items.Add("[ERROR] Error al insertar el punto.")
                Me._cMDB.TerminarTransaccion(False)
                Me.btnExaminar.Enabled = True
                Me.gbProyectos.Enabled = True
                Me.gbPuntos.Enabled = True
                Me.PictureBox1.Visible = False
                Me.Cursor = Cursors.Default
                Return
            End If

            dsPunto = Me._cMDB.RellenarDataSet("Proyectos", "SELECT TOP 1 ID_Punto FROM [Punto] ORDER BY ID_Punto DESC")
            dr = dsPunto.Tables(0).Rows(0)
            idPunto = dr("ID_Punto")
            idPuntoImportar = drImportar("ID_Punto")

            Me.lbInfo.Items.Add("[INFO] Punto " & clavePuntoImportar & " esta siendo importado")
            Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
            Application.DoEvents()

            ' Importar las listas
            ' Tengo que comprobar si es o no una alteracion
            dsListaImportar = Me._cMDBImportar.RellenarDataSet("Lista", "SELECT * FROM Lista WHERE ID_Punto=" & idPuntoImportar & " ORDER BY ID_Lista DESC")

            Me.lbInfo.Items.Add("[INFO] Punto " & clavePuntoImportar & " tiene asociadas " & dsListaImportar.Tables(0).Rows.Count & " series")
            Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
            Application.DoEvents()

            For Each rowLista As DataRow In dsListaImportar.Tables(0).Rows

                ' Lista original
                idListaImportar = rowLista("ID_Lista")
                idAlteracion = "0"

                ' Es una alteración o no. Se tiene que meter la alteracion
                If (rowLista("Tipo_lista").ToString().ToLower() <> "true") Then

                    dsAlteracionImportar = Me._cMDBImportar.RellenarDataSet("Alteracion", _
                                   "SELECT * FROM [Alteracion] WHERE ID_Punto=" & idPuntoImportar & " AND ID_Alteracion=" & rowLista("ID_Alteracion"))

                    Dim drAlt As DataRow = dsAlteracionImportar.Tables(0).Rows(0)
                    claveAlteracion = drAlt("COD_Alteracion")

                    dsAlteracion = Me._cMDB.RellenarDataSet("Alteracion", "SELECT Count(*) FROM [Alteracion] WHERE COD_Alteracion=""" & drAlt("COD_Alteracion") & """")

                    If (dsAlteracion.Tables(0).Rows(0)(0) <> 0) Then
                        claveAlteracion &= "_" & Me._id_proy_selec_punto
                    End If

                    valoresAlteracion = New String() {claveAlteracion, _
                                                      dsAlteracionImportar.Tables(0).Rows(0)("Nombre"), _
                                                      idPunto}

                    ok = Me._cMDB.InsertarRegistro("Alteracion", camposAlteracion, valoresAlteracion)
                    If (Not ok) Then
                        Me.lbInfo.Items.Add("[ERROR] Fallo al insertar la alteración.")
                        Me._cMDB.TerminarTransaccion(False)
                        Me.btnExaminar.Enabled = True
                        Me.gbProyectos.Enabled = True
                        Me.gbPuntos.Enabled = True
                        Me.PictureBox1.Visible = False
                        Me.Cursor = Cursors.Default
                        Return
                    End If

                    dsAlteracion = Me._cMDB.RellenarDataSet("Alteracion", "SELECT TOP 1 ID_Alteracion FROM [Alteracion] ORDER BY ID_Alteracion DESC")

                    idAlteracion = dsAlteracion.Tables(0).Rows(0)(0)

                    Me.lbInfo.Items.Add("[INFO] La serie que se importa es una serie alterada")
                    Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
                    Application.DoEvents()

                End If

                ' Insertar la lista
                valoresLista = New String() {rowLista("Tipo_lista"), rowLista("Nombre") & Me._id_proy_selec_punto, rowLista("Tipo_fechas"), _
                                             rowLista("Fecha_Ini"), rowLista("Fecha_Fin"), rowLista("Formato_Fecha"), _
                                             idPunto, idAlteracion}
                ok = Me._cMDB.InsertarRegistro("Lista", camposLista, valoresLista)
                If (Not ok) Then
                    Me.lbInfo.Items.Add("[ERROR] Fallo al insertar la lista.")
                    Me._cMDB.TerminarTransaccion(False)
                    Me.btnExaminar.Enabled = True
                    Me.gbProyectos.Enabled = True
                    Me.gbPuntos.Enabled = True
                    Me.PictureBox1.Visible = False
                    Me.Cursor = Cursors.Default
                    Return
                End If
                dsLista = Me._cMDB.RellenarDataSet("Lista", "SELECT TOP 1 ID_Lista FROM [Lista] ORDER BY ID_Lista DESC")
                idLista = dsLista.Tables(0).Rows(0)(0)

                ' Incluir los valores de la lista
                dsValorImportar = Me._cMDBImportar.RellenarDataSet("Valor", "SELECT * FROM [Valor] WHERE ID_Lista=" & idListaImportar & " ORDER BY ID_Valor ASC")
                valoresValor = Nothing

                Me.lbInfo.Items.Add("[INFO] La serie tiene " & dsValorImportar.Tables(0).Rows.Count & " valores")
                Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
                Application.DoEvents()

                Dim inc As Integer = 0
                Dim resto As Integer

                For Each rowValor As DataRow In dsValorImportar.Tables(0).Rows

                    If (valoresValor Is Nothing) Then
                        ReDim valoresValor(0)
                    Else
                        ReDim Preserve valoresValor(valoresValor.Length)
                    End If

                    valoresValor(valoresValor.Length - 1) = New String() {idLista, rowValor("Valor"), rowValor("Fecha")}

                    inc += 1
                    Math.DivRem(inc, 100, resto)
                    If (resto = 0) Then
                        Me.PictureBox1.Invalidate()
                        Me.PictureBox1.Refresh()
                        Application.DoEvents()
                    End If

                Next

                ok = Me._cMDB.InsertarRegistros("Valor", camposValor, valoresValor)
                If (Not ok) Then
                    Me.lbInfo.Items.Add("[ERROR] Fallo al insertar la valores.")
                    Me._cMDB.TerminarTransaccion(False)
                    Me.btnExaminar.Enabled = True
                    Me.gbProyectos.Enabled = True
                    Me.gbPuntos.Enabled = True
                    Me.PictureBox1.Visible = False
                    Me.Cursor = Cursors.Default
                    Return
                End If
            Next
            Me.lbInfo.Items.Add("[INFO] Punto " & clavePuntoImportar & " importado")
            Me.lbInfo.TopIndex = Me.lbInfo.Items.Count - 1
            Application.DoEvents()

        Next

        Me._cMDB.TerminarTransaccion(True)
        Me.Cursor = Cursors.Default

        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strImportPointsEnd") & """" & Me.cmbProyPuntos.SelectedItem.ToString() & """", _
                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOperationEnd"), _
                        MessageBoxButtons.OK, MessageBoxIcon.Information)

        'Me.Enabled = True
        Me.btnExaminar.Enabled = True
        Me.gbProyectos.Enabled = True
        Me.gbPuntos.Enabled = True
        Me.PictureBox1.Visible = False

        Me.Close()

    End Sub

    Private Sub btnCrearProy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrearProy.Click
        Dim fanadirproyecto As New FormAnadirProyecto(Me._cMDB)
        fanadirproyecto.ShowDialog()

        Me._rellenarImportar.RellenarPuntos(Me.chklstPuntos)
        Me._rellenar.RellenarProyectos(Me.cmbProyPuntos)
    End Sub
End Class