Imports IAHRIS.TestFechas.TestFechas
Imports IAHRIS.BBDD.OleDbDataBase
Imports IAHRIS.Calculo.Calculo

Public Class FormCalculo

    Dim _simulacion As TestFechas.TestFechas.Simulacion
    Dim _cMDB As BBDD.OleDbDataBase
    Dim _calculo As Calculo.Calculo
    Dim _datos As Calculo.Calculo.DatosCalculo
    Dim _informes As GeneracionInformes
    Dim _traductor As MultiLangXML.MultiIdiomasXML


    Public Sub New(ByVal simulacion As TestFechas.TestFechas.Simulacion, ByVal informes As GeneracionInformes, ByVal cMDB As BBDD.OleDbDataBase)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Poner las cosas en su sitio
        Me._simulacion = simulacion
        Me._cMDB = cMDB
        Me._informes = informes

        Dim pb As PictureBox = New PictureBox()
        pb.Size = New Size(50, 50)
        pb.Location = New Point(20, 20)
        pb.Image = New Bitmap(My.Resources.wait30trans)

        Me.Controls.Add(pb)

        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        ' ++++++++ Realizar Calculos ++++++++++++++++++++++++++++++++++++++++
        ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        My.Application.DoEvents()

        Me._calculo = New Calculo.Calculo(Me._datos, Me._simulacion.usarCoeDiara, Me._traductor)

        ' La cabecera se escribe al final, para que siempre aparezca la cabecera al abrir fichero Excel
        '        Me._calculo.EscribirCabecera(Me._simulacion, Me._informes)

        If (Me._informes.inf1) Then
            Me._calculo.GenerarInforme1()
        End If

        If (Me._informes.inf1b) Then
            Me._calculo.GenerarInforme1b()
         End If

        If (Me._informes.inf2) Then
            Me._calculo.GenerarInforme2()
        End If

        If (Me._informes.inf3) Then
            Me._calculo.generarInforme3()
        End If

        If (Me._informes.inf4a) Then
            Me._calculo.GenerarInforme4a()
        End If

        If (Me._informes.inf4b) Then
            Me._calculo.GenerarInforme4b()
        End If

        If (Me._informes.inf4) Then
            Me._calculo.GenerarInforme4()
        End If

        If (Me._informes.inf5) Then
            Me._calculo.GenerarInforme5()
        End If

        If (Me._informes.inf5a) Then
            Me._calculo.GenerarInforme5a()
        End If

        If (Me._informes.inf5b) Then
            Me._calculo.GenerarInforme5b()
        End If

        If (Me._informes.inf5c) Then
            Me._calculo.GenerarInforme5c()
        End If

        If (Me._informes.inf6) Then
            Me._calculo.GenerarInforme6()
        End If

        If (Me._informes.inf6a) Then
            Me._calculo.GenerarInforme6a()
        End If

        If (Me._informes.inf7a) Then
            Me._calculo.GenerarInforme7a()
        End If

        If (Me._informes.inf7b) Then
            Me._calculo.GenerarInforme7b()
        End If

        If (Me._informes.inf7c) Then
            Me._calculo.GenerarInforme7c()
        End If

        If (Me._informes.inf7d) Then
            Me._calculo.GenerarInforme7d()
        End If

        If (Me._informes.inf8) Then
            Me._calculo.GenerarInforme8()
        End If

        If (Me._informes.inf8a) Then
            Me._calculo.GenerarInforme8a()
        End If

        If (Me._informes.inf8b) Then
            Me._calculo.GenerarInforme8b()
        End If

        If (Me._informes.inf8c) Then
            Me._calculo.GenerarInforme8c()
        End If

        If (Me._informes.inf8d) Then
            Me._calculo.GenerarInforme8d()
        End If

        If (Me._informes.inf9) Then
            Me._calculo.GenerarInforme9()
        End If

        If (Me._informes.inf9a) Then
            Me._calculo.GenerarInforme9a()
        End If

        ' ----------------------------------
        ' Borrar hojas no usadas
        ' ----------------------------------

        

        If (Not Me._informes.inf9a) Then
            Me._calculo.BorrarInforme9a()
        End If

        If (Not Me._informes.inf9) Then
            Me._calculo.BorrarInforme9()
        End If

        If (Not Me._informes.inf8d) Then
            Me._calculo.BorrarInforme8d()
        End If

        If (Not Me._informes.inf8c) Then
            Me._calculo.BorrarInforme8c()
        End If

        If (Not Me._informes.inf8b) Then
            Me._calculo.BorrarInforme8b()
        End If

        If (Not Me._informes.inf8a) Then
            Me._calculo.BorrarInforme8a()
        End If

        If (Not Me._informes.inf8) Then
            Me._calculo.BorrarInforme8()
        End If

        If (Not Me._informes.inf7d) Then
            Me._calculo.BorrarInforme7d()
        End If

        If (Not Me._informes.inf7c) Then
            Me._calculo.BorrarInforme7c()
        End If

        If (Not Me._informes.inf7b) Then
            Me._calculo.BorrarInforme7b()
        End If

        If (Not Me._informes.inf7a) Then
            Me._calculo.BorrarInforme7a()
        End If

        If (Not Me._informes.inf6a) Then
            Me._calculo.BorrarInforme6a()
        End If

        If (Not Me._informes.inf6) Then
            Me._calculo.BorrarInforme6()
        End If

        If (Not Me._informes.inf5c) Then
            Me._calculo.BorrarInforme5c()
        End If

        If (Not Me._informes.inf5b) Then
            Me._calculo.BorrarInforme5b()
        End If

        If (Not Me._informes.inf5a) Then
            Me._calculo.BorrarInforme5a()
        End If

        If (Not Me._informes.inf5) Then
            Me._calculo.BorrarInforme5()
        End If

        If (Not Me._informes.inf4b) Then
            Me._calculo.BorrarInforme4b()
        End If

        If (Not Me._informes.inf4a) Then
            Me._calculo.BorrarInforme4a()
        End If

        If (Not Me._informes.inf4) Then
            Me._calculo.BorrarInforme4()
        End If
        
        If (Not Me._informes.inf3) Then
            Me._calculo.BorrarInforme3()
        End If

        If (Not Me._informes.inf1b) Then
            Me._calculo.BorrarInforme1b()
        End If

        Me._calculo.EscribirCabecera(Me._simulacion, Me._informes)


        ' Esto escribe realmente el fichero Excel nuevo
        Me._calculo.EscribirFichero(Me._simulacion.usarCoeDiara, Me._simulacion.usarCoe)

        Me._calculo = Nothing
        Me.Close()

    End Sub

    Private Sub FormCalculo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Centrar el formulario
        Me.Left = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width)
        Me.Top = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height)

        Me.Cursor = Cursors.WaitCursor

        ' ---------------------------------------------------------------------------------------
        ' ----------- Rellenar las listas de datos ----------------------------------------------
        ' ---------------------------------------------------------------------------------------
        ' ----- Se lee la simulacion que nos indica que hay que leer
        ' ----- Se lee de la BBDD en access en cualquier caso
        ' ---------------------------------------------------------------------------------------
        Dim año As Integer

        Dim datos As Calculo.Calculo.DatosCalculo
        datos.SerieNatDiaria.nAños = 0
        datos.SerieAltDiaria.nAños = 0
        datos.SerieNatMensual.nAños = 0
        datos.SerieAltMensual.nAños = 0
        datos.SerieAltDiaria.dia = Nothing
        datos.SerieAltDiaria.caudalDiaria = Nothing
        datos.SerieNatDiaria.dia = Nothing
        datos.SerieNatDiaria.caudalDiaria = Nothing
        datos.SerieAltMensual.mes = Nothing
        datos.SerieAltMensual.caudalMensual = Nothing
        datos.SerieNatMensual.mes = Nothing
        datos.SerieNatMensual.caudalMensual = Nothing

        Dim posNatINI As Integer
        Dim posNatFIN As Integer
        Dim posAltINI As Integer
        Dim posAltFIN As Integer

        datos.sNombre = Me._simulacion.sNombre
        datos.sAlteracion = Me._simulacion.sAlteracion
        datos.mesInicio = Me._simulacion.mesInicio

        ' ----------------------------------------------------------------
        ' Recorrer los años de la simulacion
        ' ----------------------------------
        ' -- Hay que quitar un año porque indica el ultimo año que tiene 
        ' -- fechas en el sistema no el años hidrologico
        '
        ' -- "simulacion": Datos almacenado al testear
        ' -- "datos": Datos que voy a pasar al calculo
        ' -----------------------------------------------------------------
        For año = Me._simulacion.fechaINI To Me._simulacion.fechaFIN - 1
            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ Serie NATURAL DIARIA ++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ ¿Hay datos validos? +++++++++++++++++
            ' +++ ¿Se usan los años en el calculo? ++++
            ' +++++++++++++++++++++++++++++++++++++++++
            If (Me._simulacion.listas(0).nValidos > 0 And Me._simulacion.añosParaCalculo(0).nAños > 0) Then
                ' Busco si el año se va a usar en el calculo
                Dim pos As Integer = Array.BinarySearch(Me._simulacion.añosParaCalculo(0).año, año)
                ' Si se cumple este año entra en el calculo
                If (pos >= 0) Then
                    Dim ds As DataSet

                    ' Definir el año hidrologico
                    ' ------------------------
                    'Dim fechai As Date = New  Date(año, 10, 1)
                    'Dim fechaf As Date = New Date(año + 1, 9, 30)

                    Dim mesfinal As Integer
                    Dim añofinal As Integer

                    If (datos.mesInicio = 1) Then
                        mesfinal = 12
                        añofinal = año
                    Else
                        mesfinal = datos.mesInicio - 1
                        añofinal = año + 1
                    End If

                    Dim fechai As Date = New Date(año, datos.mesInicio, 1)
                    Dim fechaf As Date = New Date(añofinal, mesfinal, Date.DaysInMonth(añofinal, mesfinal))

                    ' Sacar los datos el año hidrologico de la lista 
                    ds = Me._cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" & Me._simulacion.idListas(0) & _
                                                                           " AND fecha BETWEEN #" & fechai.ToString("yyyy-MM-dd") & "# AND #" & _
                                                                           fechaf.ToString("yyyy-MM-dd") & "# ORDER BY fecha ASC")

                    ' Esto es para la posible interpolacion. 
                    ' Me indica el inicio del año en la lista y su fin
                    If (datos.SerieNatDiaria.dia Is Nothing) Then
                        posNatINI = 0
                    Else
                        posNatINI = datos.SerieNatDiaria.dia.Length
                    End If

                    ' Meter los datos del año
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If (datos.SerieNatDiaria.dia Is Nothing) Then
                            ReDim datos.SerieNatDiaria.caudalDiaria(0)
                            ReDim datos.SerieNatDiaria.dia(0)
                        Else
                            ReDim Preserve datos.SerieNatDiaria.caudalDiaria(datos.SerieNatDiaria.caudalDiaria.Length)
                            ReDim Preserve datos.SerieNatDiaria.dia(datos.SerieNatDiaria.dia.Length)
                        End If
                        datos.SerieNatDiaria.caudalDiaria(datos.SerieNatDiaria.caudalDiaria.Length - 1) = Single.Parse(dr("valor"))
                        datos.SerieNatDiaria.dia(datos.SerieNatDiaria.caudalDiaria.Length - 1) = Date.Parse(dr("fecha"))
                    Next
                    datos.SerieNatDiaria.nAños = datos.SerieNatDiaria.nAños + 1

                    ' Esto es para la posible interpolacion. 
                    ' Me indica el inicio del año en la lista y su fin
                    posNatFIN = datos.SerieNatDiaria.dia.Length - 1

                End If
            End If
            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ Serie ALTERADA DIARIA +++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ ¿Hay datos validos? +++++++++++++++++
            ' +++ ¿Se usan los años en el calculo? ++++
            ' +++++++++++++++++++++++++++++++++++++++++
            If (Me._simulacion.listas(1).nValidos > 0 And Me._simulacion.añosParaCalculo(1).nAños > 0) Then
                ' Buscar si el año se incluye en el calculo
                Dim pos As Integer = Array.BinarySearch(Me._simulacion.añosParaCalculo(1).año, año)
                If (pos >= 0) Then
                    Dim ds As DataSet

                    'Dim fechai As Date = New Date(año, 10, 1)
                    'Dim fechaf As Date = New Date(año + 1, 9, 30)

                    ' Definir el año hidrológica
                    '---------------------------
                    Dim mesfinal As Integer
                    Dim añofinal As Integer

                    If (datos.mesInicio = 1) Then
                        mesfinal = 12
                        añofinal = año
                    Else
                        mesfinal = datos.mesInicio - 1
                        añofinal = año + 1
                    End If

                    Dim fechai As Date = New Date(año, datos.mesInicio, 1)
                    Dim fechaf As Date = New Date(añofinal, mesfinal, Date.DaysInMonth(añofinal, mesfinal))

                    ' Sacar los datos el año hidrologico de la lista 
                    ds = Me._cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" & Me._simulacion.idListas(1) & _
                                                                           " AND fecha BETWEEN #" & fechai.ToString("yyyy-MM-dd") & "# AND #" & _
                                                                           fechaf.ToString("yyyy-MM-dd") & "# ORDER BY fecha ASC")

                    ' Esto es para la posible interpolacion. 
                    ' Me indica el inicio del año en la lista y su fin
                    If (datos.SerieAltDiaria.dia Is Nothing) Then
                        posAltINI = 0
                    Else
                        posAltINI = datos.SerieAltDiaria.dia.Length
                    End If

                    ' Meter los datos asociados a los años
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If (datos.SerieAltDiaria.dia Is Nothing) Then
                            ReDim datos.SerieAltDiaria.caudalDiaria(0)
                            ReDim datos.SerieAltDiaria.dia(0)
                        Else
                            ReDim Preserve datos.SerieAltDiaria.caudalDiaria(datos.SerieAltDiaria.caudalDiaria.Length)
                            ReDim Preserve datos.SerieAltDiaria.dia(datos.SerieAltDiaria.dia.Length)
                        End If
                        datos.SerieAltDiaria.caudalDiaria(datos.SerieAltDiaria.caudalDiaria.Length - 1) = Single.Parse(dr("valor"))
                        datos.SerieAltDiaria.dia(datos.SerieAltDiaria.caudalDiaria.Length - 1) = Date.Parse(dr("fecha"))
                    Next
                    datos.SerieAltDiaria.nAños = datos.SerieAltDiaria.nAños + 1

                    ' Esto es para la posible interpolacion. 
                    ' Me indica el inicio del año en la lista y su fin
                    posAltFIN = datos.SerieAltDiaria.dia.Length - 1

                End If
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++++++++++ ATENCION: Los valores mensuales son APORTACIONES __NO__ CAUDALES +++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ Serie NATURAL MENSUAL +++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ Tengo que mirar: Es valido?
            ' +++   Si no -> Se puede interpolar?
            ' +++    Si no se cumple, pues vacio
            ' +++++++++++++++++++++++++++++++++++++++++
            If (Me._simulacion.añosParaCalculo(2).nAños > 0) Then

                Dim pos As Integer
                Dim posInt As Integer
                Dim necesitaInterpolar As Boolean = False

                pos = Array.BinarySearch(Me._simulacion.añosParaCalculo(2).año, año)
                If (pos >= 0) Then
                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ' +++ Comprobar si el dato viene de interpolar o de la BBDD
                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    If (Me._simulacion.listas(2).nValidos > 0) Then
                        pos = Array.BinarySearch(Me._simulacion.listas(2).Año, año)
                        ' No lo tengo, tengo que interpolar
                        If (pos < 0) Then
                            necesitaInterpolar = True
                        End If
                    Else
                        necesitaInterpolar = True
                    End If
                    If (Me._simulacion.añosInterNat Is Nothing) Then
                        posInt = -1
                    Else
                        posInt = Array.BinarySearch(Me._simulacion.añosInterNat, año)
                    End If

                    ' Si necesita interpolar y esta en la lista de interpoladas
                    If (necesitaInterpolar And posInt >= 0) Then
                        ' Realizar interpolacion desde la lista NATURAL DIARIA --> ERROR Ya que no sabemos si la tenemos en
                        ' el sistema metida.
                        ' Hay que leer de la BBDD y interpolar
                        Dim acum As Single = 0

                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        ' +++++++++ ITERPOLACION DIRECTA DE LA BBDD ++++++++++++++
                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        Dim ds As DataSet

                        'Dim fechai As Date = New Date(año, 10, 1)
                        'Dim fechaf As Date = New Date(año + 1, 9, 30)

                        ' Definir el año hidrológica
                        '---------------------------
                        Dim mesfinal As Integer
                        Dim añofinal As Integer

                        If (datos.mesInicio = 1) Then
                            mesfinal = 12
                            añofinal = año
                        Else
                            mesfinal = datos.mesInicio - 1
                            añofinal = año + 1
                        End If

                        Dim fechai As Date = New Date(año, datos.mesInicio, 1)
                        Dim fechaf As Date = New Date(añofinal, mesfinal, Date.DaysInMonth(añofinal, mesfinal))


                        ' Sacar los datos el año hidrologico de la lista 
                        ds = Me._cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" & Me._simulacion.idListas(0) & _
                                                                               " AND fecha BETWEEN #" & fechai.ToString("yyyy-MM-dd") & "# AND #" & _
                                                                               fechaf.ToString("yyyy-MM-dd") & "# ORDER BY fecha ASC")

                        ' ----------------------------------------------
                        ' Meter los datos naturales del año mensualmente
                        ' ----------------------------------------------
                        Dim mes As Integer = datos.mesInicio
                        Dim añoAux As Integer = año
                        Dim fechaDia As Date
                        Dim valor As Single
                        For Each dr As DataRow In ds.Tables(0).Rows
                            fechaDia = Date.Parse(dr("fecha"))
                            valor = Single.Parse(dr("valor"))
                            ' Saco todos los datos y se van acumulando en "acum".
                            ' Cuando cambia el mes, se calcula la aportacion, y
                            ' se almacena en la serie correspondiente.
                            If (mes <> fechaDia.Month) Then
                                ' ¿La serie esta vacia o ya tiene datos?
                                If (datos.SerieNatMensual.mes Is Nothing) Then
                                    ReDim datos.SerieNatMensual.caudalMensual(0)
                                    ReDim datos.SerieNatMensual.mes(0)
                                Else
                                    ReDim Preserve datos.SerieNatMensual.caudalMensual(datos.SerieNatMensual.caudalMensual.Length)
                                    ReDim Preserve datos.SerieNatMensual.mes(datos.SerieNatMensual.mes.Length)
                                End If
                                ' Almaceno los datos
                                datos.SerieNatMensual.nMeses = datos.SerieNatMensual.nMeses + 1
                                datos.SerieNatMensual.caudalMensual(datos.SerieNatMensual.caudalMensual.Length - 1) = (86400 * acum) / 1000000
                                datos.SerieNatMensual.mes(datos.SerieNatMensual.mes.Length - 1) = New Date(añoAux, mes, 1)
                                ' Preparo el nuevo mes a almacenar
                                acum = 0
                                añoAux = fechaDia.Year
                                mes = fechaDia.Month
                            End If
                            acum = acum + valor
                        Next
                        ReDim Preserve datos.SerieNatMensual.caudalMensual(datos.SerieNatMensual.caudalMensual.Length)
                        ReDim Preserve datos.SerieNatMensual.mes(datos.SerieNatMensual.mes.Length)
                        datos.SerieNatMensual.nMeses = datos.SerieNatMensual.nMeses + 1
                        datos.SerieNatMensual.caudalMensual(datos.SerieNatMensual.caudalMensual.Length - 1) = (86400 * acum) / 1000000
                        datos.SerieNatMensual.mes(datos.SerieNatMensual.mes.Length - 1) = New Date(añoAux, mes, 1)
                        datos.SerieNatMensual.nAños = datos.SerieNatMensual.nAños + 1
                    ElseIf (Not necesitaInterpolar) Then
                        ' Leer de la BBDD
                        Dim ds As DataSet

                        'Dim fechai As Date = New Date(año, 10, 1)
                        'Dim fechaf As Date = New Date(año + 1, 9, 30)

                        ' Definir el año hidrológica
                        '---------------------------
                        Dim mesfinal As Integer
                        Dim añofinal As Integer

                        If (datos.mesInicio = 1) Then
                            mesfinal = 12
                            añofinal = año
                        Else
                            mesfinal = datos.mesInicio - 1
                            añofinal = año + 1
                        End If

                        Dim fechai As Date = New Date(año, datos.mesInicio, 1)
                        Dim fechaf As Date = New Date(añofinal, mesfinal, Date.DaysInMonth(añofinal, mesfinal))

                        ds = Me._cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" & Me._simulacion.idListas(2) & _
                                                                               " AND fecha BETWEEN #" & fechai.ToString("yyyy-MM-dd") & "# AND #" & _
                                                                               fechaf.ToString("yyyy-MM-dd") & "# ORDER BY fecha ASC")
                        For Each dr As DataRow In ds.Tables(0).Rows

                            If (datos.SerieNatMensual.mes Is Nothing) Then
                                ReDim datos.SerieNatMensual.caudalMensual(0)
                                ReDim datos.SerieNatMensual.mes(0)
                            Else
                                ReDim Preserve datos.SerieNatMensual.caudalMensual(datos.SerieNatMensual.caudalMensual.Length)
                                ReDim Preserve datos.SerieNatMensual.mes(datos.SerieNatMensual.mes.Length)
                            End If
                            datos.SerieNatMensual.nMeses = datos.SerieNatMensual.nMeses + 1
                            datos.SerieNatMensual.caudalMensual(datos.SerieNatMensual.caudalMensual.Length - 1) = Single.Parse(dr("valor"))
                            datos.SerieNatMensual.mes(datos.SerieNatMensual.mes.Length - 1) = Date.Parse(dr("fecha"))
                        Next
                        datos.SerieNatMensual.nAños = datos.SerieNatMensual.nAños + 1
                    End If ' Interpolar o no
                End If ' Es año participante
            End If ' Es año valido

            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ Serie ALTERADA MENSUAL +++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++
            ' +++ Tengo que mirar: Es valido?
            ' +++   Si no -> Se puede interpolar?
            ' +++    Si no se cumple, pues vacio
            ' +++++++++++++++++++++++++++++++++++++++++
            If (Me._simulacion.añosParaCalculo(3).nAños > 0) Then

                Dim pos As Integer
                Dim posInt As Integer
                Dim necesitaInterpolar As Boolean = False

                pos = Array.BinarySearch(Me._simulacion.añosParaCalculo(3).año, año)
                If (pos >= 0) Then
                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ' +++ Comprobar si el dato viene de interpolar o de la BBDD
                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    If (Me._simulacion.listas(3).nValidos > 0) Then
                        pos = Array.BinarySearch(Me._simulacion.listas(3).Año, año)
                        ' No lo tengo, tengo que interpolar
                        If (pos < 0) Then
                            necesitaInterpolar = True
                        End If
                    Else
                        necesitaInterpolar = True
                    End If
                    If (Me._simulacion.añosInterAlt Is Nothing) Then
                        posInt = -1
                    Else
                        posInt = Array.BinarySearch(Me._simulacion.añosInterAlt, año)
                    End If

                    ' Si necesita interpolar y esta en la lista de interpoladas
                    If (necesitaInterpolar And posInt >= 0) Then
                        ' Realizar interpolacion desde la lista NATURAL DIARIA
                        Dim acum As Single = 0

                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        ' +++++++++ ITERPOLACION DIRECTA DE LA BBDD ++++++++++++++
                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        Dim ds As DataSet

                        'Dim fechai As Date = New Date(año, 10, 1)
                        'Dim fechaf As Date = New Date(año + 1, 9, 30)

                        ' Definir el año hidrológica
                        '---------------------------
                        Dim mesfinal As Integer
                        Dim añofinal As Integer

                        If (datos.mesInicio = 1) Then
                            mesfinal = 12
                            añofinal = año
                        Else
                            mesfinal = datos.mesInicio - 1
                            añofinal = año + 1
                        End If

                        Dim fechai As Date = New Date(año, datos.mesInicio, 1)
                        Dim fechaf As Date = New Date(añofinal, mesfinal, Date.DaysInMonth(añofinal, mesfinal))

                        ' Sacar los datos el año hidrologico de la lista 
                        ds = Me._cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" & Me._simulacion.idListas(1) & _
                                                                               " AND fecha BETWEEN #" & fechai.ToString("yyyy-MM-dd") & "# AND #" & _
                                                                               fechaf.ToString("yyyy-MM-dd") & "# ORDER BY fecha ASC")

                        ' ----------------------------------------------
                        ' Meter los datos naturales del año mensualmente
                        ' ----------------------------------------------
                        Dim mes As Integer = datos.mesInicio
                        Dim añoAux As Integer = año
                        Dim fechaDia As Date
                        Dim valor As Single
                        For Each dr As DataRow In ds.Tables(0).Rows
                            fechaDia = Date.Parse(dr("fecha"))
                            valor = Single.Parse(dr("valor"))
                            If (mes <> fechaDia.Month) Then
                                If (datos.SerieAltMensual.mes Is Nothing) Then
                                    ReDim datos.SerieAltMensual.caudalMensual(0)
                                    ReDim datos.SerieAltMensual.mes(0)
                                Else
                                    ReDim Preserve datos.SerieAltMensual.caudalMensual(datos.SerieAltMensual.caudalMensual.Length)
                                    ReDim Preserve datos.SerieAltMensual.mes(datos.SerieAltMensual.mes.Length)
                                End If
                                datos.SerieAltMensual.nMeses = datos.SerieAltMensual.nMeses + 1
                                datos.SerieAltMensual.caudalMensual(datos.SerieAltMensual.caudalMensual.Length - 1) = (86400 * acum) / 1000000
                                datos.SerieAltMensual.mes(datos.SerieAltMensual.mes.Length - 1) = New Date(añoAux, mes, 1)
                                acum = 0
                                añoAux = fechaDia.Year
                                mes = fechaDia.Month
                            End If
                            acum = acum + valor
                        Next
                        ReDim Preserve datos.SerieAltMensual.caudalMensual(datos.SerieAltMensual.caudalMensual.Length)
                        ReDim Preserve datos.SerieAltMensual.mes(datos.SerieAltMensual.mes.Length)
                        datos.SerieAltMensual.nMeses = datos.SerieAltMensual.nMeses + 1
                        datos.SerieAltMensual.caudalMensual(datos.SerieAltMensual.caudalMensual.Length - 1) = (86400 * acum) / 1000000
                        datos.SerieAltMensual.mes(datos.SerieAltMensual.mes.Length - 1) = New Date(añoAux, mes, 1)
                        datos.SerieAltMensual.nAños = datos.SerieAltMensual.nAños + 1
                    ElseIf (Not necesitaInterpolar) Then
                        ' Leer de la BBDD
                        Dim ds As DataSet

                        'Dim fechai As Date = New Date(año, 10, 1)
                        'Dim fechaf As Date = New Date(año + 1, 9, 30)

                        ' Definir el año hidrológica
                        '---------------------------
                        Dim mesfinal As Integer
                        Dim añofinal As Integer

                        If (datos.mesInicio = 1) Then
                            mesfinal = 12
                            añofinal = año
                        Else
                            mesfinal = datos.mesInicio - 1
                            añofinal = año + 1
                        End If

                        Dim fechai As Date = New Date(año, datos.mesInicio, 1)
                        Dim fechaf As Date = New Date(añofinal, mesfinal, Date.DaysInMonth(añofinal, mesfinal))

                        ds = Me._cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" & Me._simulacion.idListas(3) & _
                                                                               " AND fecha BETWEEN #" & fechai.ToString("yyyy-MM-dd") & "# AND #" & _
                                                                               fechaf.ToString("yyyy-MM-dd") & "# ORDER BY fecha ASC")
                        For Each dr As DataRow In ds.Tables(0).Rows
                            If (datos.SerieAltMensual.mes Is Nothing) Then
                                ReDim datos.SerieAltMensual.caudalMensual(0)
                                ReDim datos.SerieAltMensual.mes(0)
                            Else
                                ReDim Preserve datos.SerieAltMensual.caudalMensual(datos.SerieAltMensual.caudalMensual.Length)
                                ReDim Preserve datos.SerieAltMensual.mes(datos.SerieAltMensual.mes.Length)
                            End If
                            datos.SerieAltMensual.nMeses = datos.SerieAltMensual.nMeses + 1
                            datos.SerieAltMensual.caudalMensual(datos.SerieAltMensual.caudalMensual.Length - 1) = Single.Parse(dr("valor"))
                            datos.SerieAltMensual.mes(datos.SerieAltMensual.mes.Length - 1) = Date.Parse(dr("fecha"))
                        Next
                        datos.SerieAltMensual.nAños = datos.SerieAltMensual.nAños + 1
                    End If ' Interpolar o no
                End If ' Es año participante
            End If ' Es año valido
        Next

        datos.nAnyosCoe = Me._simulacion.añosCoetaneosTotales
        Me._datos = datos


    End Sub

    Private Sub FormCalculo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        My.Application.DoEvents()

        Me.btnCancelar_Click(Nothing, Nothing)

        Me.Cursor = Cursors.Default
    End Sub
End Class