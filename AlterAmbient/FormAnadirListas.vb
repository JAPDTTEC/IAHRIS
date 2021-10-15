Imports Microsoft.VisualBasic.FileIO
Imports IAHRIS.TestFechas
Public Class FormAnadirListas

    Structure CabeceraCSV
        Public TipoFechas As Boolean
        Public Clave_Punto As String
        Public TipoLista As Boolean
        'Public Ordinal As Integer
        Public Clave_Alteracion As String
        'Public Nombre As String
        Public FormatoFecha As String
    End Structure

    Structure DatosCSV
        Public fechas As Date
        Public valores As Single
    End Structure


    Dim _cMDB As BBDD.OleDbDataBase
    Dim _tFechas As TestFechas.TestFechas

    Dim _rutafichero As String
    Dim _cabecera As CabeceraCSV

    Private _traductor As MultiLangXML.MultiIdiomasXML

    Private Sub FormAnadirListas_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        ' Centrar el form
        Me.Left = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width)
        Me.Top = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height)
    End Sub

    Public Sub New(ByVal MDB As BBDD.OleDbDataBase)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._cMDB = MDB
        Me._tFechas = New TestFechas.TestFechas(MDB)

        ' -------------------------------------
        ' ---- Traducir formulario ------------
        ' -------------------------------------
        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")
    End Sub

    Private Sub FormAnadirListas_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F1 Then
            'Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, "Añadir serie")
        End If
    End Sub
    Private Sub FormAnadirListas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.grpboxDetalles.Enabled = False
    End Sub

#Region "Cargar Series"
    Private Sub btnExaminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExaminar.Click
        Dim openFileDialog1 As New OpenFileDialog()
        Dim fields As String()
        Dim delimiter As String = ";"

        openFileDialog1.Filter = "Listas de datos (*.csv)|*.csv|Todos los ficheros (*.*)|*.*"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            Me._rutafichero = openFileDialog1.FileName

            ' ----------------------------------------------------------------
            ' Esto tendria que sacarlo a una clase o modulo auxiliar...
            ' Es mucho codigo para tan poco que hacer.
            ' ----------------------------------------------------------------
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++++++++++ Leer CVS ++++++++++++++++++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Using parser As New TextFieldParser(Me._rutafichero)

                parser.SetDelimiters(delimiter)

                fields = parser.ReadFields()

                ' CABECERA
                ' La cabecera puede tener entre 3 o 4 campos
                ' --- 4 Campos: Alterada
                ' --- 3 Campos: Natural
                ' ------------------------------------------------
                If ((fields.Length > 4) Or (fields.Length < 3)) Then
                    MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeader"), _
                                    Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), _
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Try
                    If (fields(0) = "DIARIO") Then
                        Me._cabecera.TipoFechas = True
                        Me._cabecera.FormatoFecha = "dd/MM/yyyy"
                    ElseIf (fields(0) = "MENSUAL") Then
                        Me._cabecera.TipoFechas = False
                        Me._cabecera.FormatoFecha = "MM/yyyy"
                    Else
                        'MessageBox.Show("Error en el campo de definición de tipo de fecha (Mensual/Diaria).")
                        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderDate"), _
                                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), _
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    If (fields(1) = "NATURAL") Then
                        Me._cabecera.TipoLista = True
                    ElseIf (fields(1) = "ALTERADO") Then
                        Me._cabecera.TipoLista = False
                    Else
                        'MessageBox.Show("Error en el campo de tipo de valores (Natural/Alterado).")
                        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderType"), _
                                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), _
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Me._cabecera.Clave_Punto = fields(2)

                    If ((Me._cabecera.TipoLista = False) And (fields.Length <> 4)) Then
                        'MessageBox.Show("Error en la cabecera. No se encuentran todos los campos para" & vbCrLf _
                        '                                    & "una serie alterada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderAlt"), _
                                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), _
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    If Me._cabecera.TipoLista = False Then
                        Me._cabecera.Clave_Alteracion = fields(3)
                    Else
                        Me._cabecera.Clave_Alteracion = ""
                    End If
                Catch ex As Exception
                    'MessageBox.Show("Error en la cabecera, el formato de los campos no es correcto.", "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderGeneral"), _
                                    Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), _
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                Me.grpboxDetalles.Enabled = True

                'Me.lblInfo.Text = "INFORMACIÓN: " & vbCrLf & vbCrLf & _
                '                    "Tipo: " & fields(1) & vbCrLf & _
                '                    "Periodicidad: " & fields(0) & vbCrLf & _
                '                    "Punto: " & fields(2) & vbCrLf & vbCrLf
                Me.lblInfo.Text = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo").ToUpper() & ": " & _
                                  vbCrLf & vbCrLf & _
                                  Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strType") & ": " & fields(1) & vbCrLf & _
                                  Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPerio") & ": " & fields(0) & vbCrLf & _
                                  Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPoint") & ": " & fields(2) & _
                                  vbCrLf & vbCrLf
                If (Me._cabecera.TipoLista = False) Then
                    Me.lblInfo.Text = Me.lblInfo.Text & _
                                      Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAlt") & ": " & _
                                      fields(3)
                End If

                fields = parser.ReadFields()


                ' Testear que la alteracion (si es alteracion) pertenece al punto

                Dim dt As Date
                ' Testear fechas
                If (Me._cabecera.TipoFechas) Then
                    If (Not Me._tFechas.ComprobarFechasCSV(Me._cabecera.TipoFechas, fields(0), dt)) Then
                        'MessageBox.Show("Formato de la fechas DIARIAS no es correcto.")
                        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormatDaily"), _
                                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), _
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                Else
                    If (Not Me._tFechas.ComprobarFechasCSV(Me._cabecera.TipoFechas, fields(1) & "/" & fields(0), dt)) Then
                        'MessageBox.Show("Formato de la fechas MENSUAL no es correcto.")
                        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormatMonthly"), _
                                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorRead"), _
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                End If

                ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' +++++ COMPROBAR SI LAS LISTAS ESTAN YA EN EL SISTEMA ++++++++++++++++++++
                ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                Me.txtRuta.Text = Me._rutafichero

            End Using
        End If
    End Sub
    Private Sub btnCargarLista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargarLista.Click

        Dim fields As String()
        Dim delimiter As String = ";"

        Dim datos() As DatosCSV

        Dim sFechas As String
        Dim sValor As String

        Dim linea As Integer
        Dim lonDatos As Long

        Dim fechaINI As Date
        Dim fechaFIN As Date

        linea = 1
        lonDatos = 0

        datos = Nothing


        ' Errores y su gestion
        ' --------------------
        Dim nErrores As Integer = 0
        Dim strErrores As ArrayList = New ArrayList()


        Me.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        ' ----------------------------------------------------------------
        ' Esto tendria que sacarlo a una clase o modulo auxiliar...
        ' Es mucho codigo para tan poco que hacer.
        ' ----------------------------------------------------------------
        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        ' ++++++++++++++ Leer CVS ++++++++++++++++++++++++++++++++++++++++
        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        Using parser As New TextFieldParser(Me._rutafichero)
            parser.SetDelimiters(delimiter)

            Dim dsMesInicio As DataSet
            Dim mesInicio As Integer

            While Not parser.EndOfData

                ' Read in the fields for the current line
                fields = parser.ReadFields()

                If (linea > 1) Then
                    ' Campo de datos
                    ReDim Preserve datos(lonDatos)

                    If (Me._cabecera.TipoFechas) Then
                        sFechas = fields(0)
                        sValor = fields(1)
                    Else
                        sFechas = fields(1) & "/" & fields(0)
                        sValor = fields(2)
                    End If

                    'Dim s As String
                    's = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator
                    'Dim iValor As Single
                    '' Para que no de error por errores de punto/coma como separador decimal
                    'If (s = ".") Then
                    '    iValor = Single.Parse(sValor.Replace(",", "."))
                    'Else
                    '    iValor = Single.Parse(sValor.Replace(".", ","))
                    'End If

                    ' ----------------------------------------------------------
                    '    ----- Comprobar que el valor es válido  -----
                    ' ----------------------------------------------------------
                    'If (iValor < 0) Then
                    'MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVValueNegative") & linea.ToString(), _
                    '                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                    '                MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'Me.Enabled = True
                    'Me.Cursor = Cursors.Default
                    'Return

                    'nErrores = nErrores + 1
                    'strErrores.Add("Error Line " + linea + ": " + sValor + " value is not valid.")
                    'End If

                    If (linea = 2) Then
                        ' -------------------------------------------------------------------------------
                        ' Generar tanto fechaINI como fechaFIN que son los inicios y fin TEORICOS.
                        '   más adelante vamos a tener que comprobar que tenemos todos los datos reales.
                        ' -------------------------------------------------------------------------------
                        Dim okfecha As Boolean = Me._tFechas.ComprobarFechasCSV(Me._cabecera.TipoFechas, sFechas, fechaINI)
                        If (Not okfecha) Then
                            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") & linea.ToString(), _
                                                                    Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                            'MessageBox.Show("Error en la linea " & linea.ToString() & ". La fecha no esta en el formato correcto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Me.Enabled = True
                            Me.Cursor = Cursors.Default

                            Return
                        End If
                        okfecha = Me._tFechas.ComprobarFechasCSV(Me._cabecera.TipoFechas, sFechas, fechaFIN)
                        If (Not okfecha) Then
                            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") & linea.ToString(), _
                                                                    Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                            'MessageBox.Show("Error en la linea " & linea.ToString() & ". La fecha no esta en el formato correcto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Me.Enabled = True
                            Me.Cursor = Cursors.Default

                            Return
                        End If

                        ' ----------------------------------------
                        ' Adecuar fecha INICIAL al año hidrologico
                        ' ----------------------------------------
                        dsMesInicio = Me._cMDB.RellenarDataSet("Puntos", "SELECT mesInicio FROM [Punto] WHERE Clave_punto ='" & Me._cabecera.Clave_Punto & "'")
                        mesInicio = dsMesInicio.Tables(0).Rows(0)(0)

                        'If (fechaINI.Month <> 10 Or fechaINI.Day <> 1) Then
                        '    If (fechaINI.Month < 10) Then
                        '        fechaINI = New Date(fechaINI.Year - 1, 10, 1)
                        '    Else
                        '        fechaINI = New Date(fechaINI.Year, 10, 1)
                        '    End If
                        'End If

                        If (fechaINI.Month <> mesInicio Or fechaINI.Day <> 1) Then
                            If (fechaINI.Month < mesInicio) Then
                                fechaINI = New Date(fechaINI.Year - 1, mesInicio, 1)
                            Else
                                fechaINI = New Date(fechaINI.Year, mesInicio, 1)
                            End If
                        End If


                    End If

                    If (Not Me._tFechas.ComprobarFechasCSV(Me._cabecera.TipoFechas, sFechas, datos(lonDatos).fechas)) Then
                        'MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") & linea.ToString(), _
                        '                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                        '                MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'MessageBox.Show("Error en la linea " & linea.ToString() & ". La fecha no esta en el formato correcto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        'Me.Enabled = True
                        'Me.Cursor = Cursors.Default

                        'Return

                        nErrores = nErrores + 1
                        strErrores.Add("Error Line " & linea & ": " & sFechas & " date is not valid.")
                        Continue While
                    End If

                    If (Date.Compare(fechaINI, datos(lonDatos).fechas) > 0) Then
                        fechaINI = datos(lonDatos).fechas
                    End If
                    If (Date.Compare(fechaFIN, datos(lonDatos).fechas) < 0) Then
                        fechaFIN = datos(lonDatos).fechas
                    End If

                    ' Hace un try para controlar una posible excepción
                    If (Not Single.TryParse(sValor, datos(lonDatos).valores)) Then
                        'MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVValueError") & linea.ToString(), _
                        '               Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                        '               MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'MessageBox.Show("El formato no es correcto. Hay caracteres no reconocidos dentro del valor de la linea" & linea, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'Me.Enabled = True
                        'Me.Cursor = Cursors.Default
                        'Return
                        nErrores = nErrores + 1
                        strErrores.Add("Error Line " & linea & ": " & sValor & " value is not valid.")
                        Continue While
                    Else
                        If (datos(lonDatos).valores < 0) Then
                            nErrores = nErrores + 1
                            strErrores.Add("Error Line " & linea & ": " & sValor & " value is not valid.")
                            Continue While
                        End If
                    End If
                    lonDatos = lonDatos + 1
                End If
                linea = linea + 1
            End While

            ' Si hay error muestro un mensaje de error
            If (nErrores > 0) Then
                If (MessageBox.Show("Se han encontrado " & nErrores & ". No se puede continuar. " & vbCrLf & "¿Desea un volcado con la información de los errores?", _
                                                       Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Error) = Windows.Forms.DialogResult.Yes) Then
                    Dim ofd As SaveFileDialog = New SaveFileDialog()
                    ofd.AddExtension = True
                    ofd.Filter = "Listas de datos (*.log)|*.log|Todos los ficheros (*.*)|*.*"
                    If (ofd.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                        Dim fichLog As System.IO.StreamWriter = New System.IO.StreamWriter(ofd.OpenFile())
                        For ind As Integer = 0 To strErrores.Count - 1
                            fichLog.WriteLine(strErrores(ind))
                        Next
                        fichLog.Flush()
                        fichLog.Close()
                    End If
                End If
                nErrores = 0
                strErrores = Nothing
                Me.Enabled = True
                Me.Cursor = Cursors.Default
                Return
            End If

            ' ----------------------------------------
            ' Adecuar fecha FINAL al año hidrologico
            ' ----------------------------------------
            Dim mesFin As Integer
            mesFin = mesInicio - 1
            If (mesFin <= 0) Then
                mesFin = 12
            End If

            Dim diaFin As Integer = Date.DaysInMonth(fechaFIN.Year, mesFin)

            'If (fechaFIN.Month <> 9 Or fechaFIN.Day <> 30) Then
            '    If (fechaFIN.Month > 10) Then
            '        fechaFIN = New Date(fechaFIN.Year + 1, 9, 30)
            '    Else
            '        fechaFIN = New Date(fechaFIN.Year, 9, 30)
            '    End If
            'End If

            If (fechaFIN.Month <> mesFin) Then
                If (fechaFIN.Month > mesFin) Then
                    diaFin = Date.DaysInMonth(fechaFIN.Year + 1, mesFin)
                    fechaFIN = New Date(fechaFIN.Year + 1, mesFin, diaFin)
                Else
                    diaFin = Date.DaysInMonth(fechaFIN.Year, mesFin)
                    fechaFIN = New Date(fechaFIN.Year, mesFin, diaFin)
                End If
            End If


        End Using

        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        ' ++++++++++ Revisar por si hay errores o duplicidades +++++++++++++++++++
        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        Dim dsPunto As DataSet
        Dim dsAlt As DataSet
        'Dim dsNat As DataSet

        Dim idAlt As Integer
        Dim idPunto As Integer
        Dim nombrelista As String

        ' Se comienza ha hacer la transaccion
        Me._cMDB.ComenzarTransaccion()

        ' Se comprueba que el punto existe y la relación con la alteración (si es una alteración) tambien existe.
        If (Not Me.ComprobarSerie()) Then
            Me.Enabled = True
            Me.Cursor = Cursors.Default
            Return
        End If

        ' Sacar nombre y id
        dsPunto = Me._cMDB.RellenarDataSet("Puntos", "SELECT id_punto FROM [Punto] WHERE Clave_punto ='" & Me._cabecera.Clave_Punto & "'")
        Dim drAux As DataRow = dsPunto.Tables(0).Rows(0)
        idPunto = drAux(0)

        nombrelista = Me._cabecera.Clave_Punto

        ' Comprobaciones que dependen si es una lista Alterada o Natural
        If (Me._cabecera.TipoLista = False) Then
            nombrelista = nombrelista & "Alt"
            If (Me._cabecera.TipoFechas = True) Then
                nombrelista = nombrelista & "Diario"
            Else
                nombrelista = nombrelista & "Mensual"
            End If
            dsAlt = Me._cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE COD_Alteracion ='" & Me._cabecera.Clave_Alteracion & "'")

            Dim drAlt As DataRow = dsAlt.Tables(0).Rows(0)
            idAlt = drAlt(0)
        Else
            nombrelista = nombrelista & "Nat"
            If (Me._cabecera.TipoFechas = True) Then
                nombrelista = nombrelista & "Diario"
            Else
                nombrelista = nombrelista & "Mensual"
            End If
        End If

        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        ' ++++++++++ Insertar en la LISTA ++++++++++++++++++++++++++++++++++++++++
        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        Dim dr As DataRow
        dr = dsPunto.Tables(0).Rows(0)

        Dim i As Integer


        Dim campos() As String = {"ID_punto", "Tipo_lista", "ID_Alteracion", "Tipo_fechas", "fecha_INI", "fecha_FIN", "formato_fecha", "Nombre"}
        'if (
        Dim valores() As String = {dr("id_punto"), Me._cabecera.TipoLista.ToString(), idAlt, Me._cabecera.TipoFechas.ToString(), fechaINI.ToString(Me._cabecera.FormatoFecha), fechaFIN.ToString(Me._cabecera.FormatoFecha), Me._cabecera.FormatoFecha, nombrelista & idAlt.ToString()}
        If (Not Me._cMDB.InsertarRegistro("Lista", campos, valores)) Then
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDB") & linea.ToString(), _
                            Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)

            'MessageBox.Show("Base de datos no accesible en este momento.")
            Me._cMDB.TerminarTransaccion(False)
            Me.Enabled = True
            Me.Cursor = Cursors.Default
            Return
        End If

        ' Truco: Saco la ultima que he insertado, asi se cual la lista a la que le doy valores
        dsPunto = Me._cMDB.RellenarDataSet("Lista", "SELECT TOP 1 id_lista FROM Lista ORDER BY id_lista DESC") ' Ultima que acabo de meter
        Dim id_lista = dsPunto.Tables(0).Rows(0)("id_lista")

        ReDim campos(2)
        ReDim valores(2)
        Dim mDatos As String()()    ' Lo preparo para la funcion de la base de datos

        ReDim mDatos(lonDatos - 1)

        campos(0) = "ID_lista"
        campos(1) = "Valor"
        campos(2) = "Fecha"

        ' Lista de datos a montar
        For i = 0 To lonDatos - 1
            ReDim Preserve mDatos(i)(2)

            mDatos(i)(0) = id_lista
            mDatos(i)(1) = datos(i).valores
            mDatos(i)(2) = datos(i).fechas
        Next

        ' Insercion de los valores en la base datos
        If (Not Me._cMDB.InsertarRegistros("Valor", campos, mDatos)) Then
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDB") & linea.ToString(), _
                            Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            'MessageBox.Show("Restaurando la base de datos al estado anterior a la importación.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me._cMDB.TerminarTransaccion(False)
            Me.Enabled = True
            Me.Cursor = Cursors.Default
            Return
        End If

        Me._cMDB.TerminarTransaccion(True)

        Me.Enabled = True

        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_1") _
                        & vbCrLf & vbCrLf & _
                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_2") & ": " & linea & vbCrLf & _
                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_3") & ": " & lonDatos, _
                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), _
                        MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "Funciones Auxiliares"
    ''' <summary>
    ''' Comprobaciones de existencia de la lista, y si es alteracion si pertence al punto
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ComprobarSerie() As Boolean

        ' Si no hay nada, no hago nada :)
        If (Me._cabecera.Clave_Punto = "") Then
            Return False
        End If

        ' PRIMERA COMPROBACION: ¿Esta el punto?
        ' +++++++++++++++++++++++++++++++++++++
        Dim dsPunto As DataSet
        Dim dsAlt As DataSet
        Dim drAux As DataRow
        Dim idPunto As Integer
        Dim idAlt As Integer

        dsPunto = Me._cMDB.RellenarDataSet("Puntos", "SELECT id_punto FROM [Punto] WHERE Clave_punto ='" & Me._cabecera.Clave_Punto & "'")
        If (dsPunto.Tables(0).Rows.Count = 0) Then
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointNotInDB") & _
                            " '" & Me._cabecera.Clave_Punto.ToString() & "' " & _
                            Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotInDB"), _
                            Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Else
            drAux = dsPunto.Tables(0).Rows(0)
            idPunto = drAux(0)
        End If


        If (Me._cabecera.Clave_Alteracion <> "") Then

            ' SEGUNDA COMPROBACION: ¿Existe la alteracion?
            ' ++++++++++++++++++++++++++++++++++++++++++++
            dsAlt = Me._cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE COD_Alteracion ='" & Me._cabecera.Clave_Alteracion & "'")
            If (dsAlt.Tables(0).Rows.Count <> 1) Then
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltNotInDB") & _
                                " '" & Me._cabecera.Clave_Alteracion.ToString() & "' " & _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotInDB"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            Else
                Dim drAlt As DataRow = dsAlt.Tables(0).Rows(0)
                idAlt = drAlt(0)
            End If

            ' TERCERA COMPROBACION: ¿Concuerdan Punto-Alteracion?
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++
            'Comprobar que la alteración pertence al punto
            dsAlt = Me._cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE id_alteracion=" & idAlt & " AND id_punto=" & idPunto)
            If (dsAlt.Tables(0).Rows.Count = 0) Then
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltNotInDB") & _
                                " '" & Me._cabecera.Clave_Alteracion.ToString() & "' " & _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotAsociated") & _
                                " '" & Me._cabecera.Clave_Punto.ToString() & "'", _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

        End If

        ' CUARTA COMPROBACION: ¿Existe ya la lista en el sistema?
        ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
        dsAlt = Me._cMDB.RellenarDataSet("LISTA", "SELECT id_lista FROM [Lista] WHERE id_alteracion=" & idAlt & " AND id_punto=" & idPunto & " AND Tipo_fechas=" & Me._cabecera.TipoFechas & " AND Tipo_lista=" & Me._cabecera.TipoLista)
        If (dsAlt.Tables(0).Rows.Count <> 0) Then
            If (MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strSerieOverwrite"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), _
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel) Then
                Return False
            Else
                Dim drAlt As DataRow = dsAlt.Tables(0).Rows(0)
                Me._cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_lista = " & drAlt(0).ToString)
            End If
        End If



        Return True
    End Function
#End Region


End Class