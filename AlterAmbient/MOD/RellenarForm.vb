Imports IAHRIS.BBDD.OleDbDataBase
Imports System.Data.DataSet
Imports IAHRIS.TestFechas.TestFechas

Imports XPTable
Imports XPTable.Models

Namespace Rellenar

    Class RellenarForm
        Private _cMDB As BBDD.OleDbDataBase
        Private _Fechas As TestFechas.TestFechas


        Public Sub New(ByVal MDB As BBDD.OleDbDataBase)
            Me._cMDB = MDB
            Me._Fechas = New TestFechas.TestFechas(Me._cMDB)
        End Sub

        Public Sub RellenarAlteraciones(ByRef combo As System.Windows.Forms.ComboBox, ByVal id As Integer)
            ' Sacar nombre de las listas
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Listas", "Select COD_alteracion FROM [Alteracion] WHERE id_punto=" & id & " order by cod_alteracion ASC")
            combo.Items.Clear()
            combo.Text = String.Empty

            For Each dr As DataRow In ds.Tables(0).Rows
                combo.Items.Add(dr(0).ToString())
            Next
            If (combo.Items.Count > 0) Then
                combo.SelectedIndex = 0
            End If
        End Sub

        Public Sub RellenarAlteraciones(ByRef listbox As System.Windows.Forms.ListBox, ByVal id As Integer)
            ' Sacar nombre de las listas
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Listas", "Select COD_alteracion FROM [Alteracion] WHERE id_punto=" & id & " order by cod_alteracion ASC")
            listbox.Items.Clear()
            'listbox.Text = String.Empty

            For Each dr As DataRow In ds.Tables(0).Rows
                listbox.Items.Add(dr(0).ToString())
            Next
            'If (listbox.Items.Count > 0) Then
            'listbox.SelectedIndex = 0
            'End If
        End Sub

        Public Sub RellenarPuntos(ByRef listbox As System.Windows.Forms.ListBox)
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Puntos", "Select * From Punto order by clave_punto asc")

            'Dim dr As DataRow
            listbox.Items.Clear() ' Limpiar el listbox
            For Each dr As DataRow In ds.Tables(0).Rows
                listbox.Items.Add(dr("Clave_punto").ToString())
                'Console.WriteLine(dr("nombre").ToString())
            Next

        End Sub

        Public Sub RellenarPuntos(ByRef checklistbox As System.Windows.Forms.CheckedListBox)
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Puntos", "Select * From Punto order by clave_punto asc")

            'Dim dr As DataRow
            checklistbox.Items.Clear() ' Limpiar el listbox
            For Each dr As DataRow In ds.Tables(0).Rows
                checklistbox.Items.Add(dr("Clave_punto").ToString())
                'Console.WriteLine(dr("nombre").ToString())
            Next

            

        End Sub

        Public Sub RellenarPuntos(ByRef listbox As System.Windows.Forms.ListBox, ByVal id_proy As Integer)

            Dim ds As DataSet
            If (id_proy = -1) Then
                ds = Me._cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] ORDER BY clave_punto ASC")
            Else
                ds = Me._cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] WHERE ID_proyecto=" & id_proy & " ORDER BY clave_punto ASC")
            End If


            listbox.Items.Clear() ' Limpiar el listbox
            For Each dr As DataRow In ds.Tables(0).Rows
                listbox.Items.Add(dr("Clave_punto").ToString())
            Next

        End Sub

        Public Sub RellenarPuntos(ByRef clistbox As System.Windows.Forms.CheckedListBox, ByVal id_proy As Integer)

            Dim ds As DataSet
            If (id_proy = -1) Then
                ds = Me._cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] ORDER BY clave_punto ASC")
            Else
                ds = Me._cMDB.RellenarDataSet("Puntos", "SELECT * FROM [Punto] WHERE ID_proyecto=" & id_proy & " ORDER BY clave_punto ASC")
            End If


            clistbox.Items.Clear() ' Limpiar el listbox
            For Each dr As DataRow In ds.Tables(0).Rows
                clistbox.Items.Add(dr("Clave_punto").ToString())
            Next

        End Sub

        Public Sub RellenarListas(ByRef combo As System.Windows.Forms.ComboBox, ByVal id As Integer)
            ' Sacar nombre de las listas
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista] WHERE id_punto=" & id & "")
            combo.Items.Clear()
            combo.Text = String.Empty

            For Each dr As DataRow In ds.Tables(0).Rows
                combo.Items.Add(dr("nombre"))
            Next
            If (combo.Items.Count > 0) Then
                combo.SelectedIndex = 0
            End If
        End Sub

        Public Sub RellenarListas(ByRef listbox As System.Windows.Forms.ListBox, ByVal id As Integer)
            ' Sacar nombre de las listas
            Dim ds As DataSet
            If (id <> -1) Then
                ds = Me._cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista] WHERE id_punto=" & id & "")
            Else
                ds = Me._cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista]")
            End If

            listbox.Items.Clear()

            For Each dr As DataRow In ds.Tables(0).Rows
                listbox.Items.Add(dr("nombre"))
            Next

        End Sub

        Public Sub RellenarListas(ByRef haySerieDiaria As Boolean, ByRef haySerieMensual As Boolean, ByRef comboAltD As System.Windows.Forms.ComboBox, ByVal id As Integer, ByVal stNone As String)

            ' Sacar que listas Naturales hay
            Dim dsNat As DataSet = Me._cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista] WHERE id_punto=" & id & " AND tipo_lista=true AND tipo_fechas=true")
            If (dsNat.Tables(0).Rows.Count > 0) Then
                haySerieDiaria = True
            Else
                haySerieDiaria = False
            End If

            ' Sacar que lista Alteradas hay
            dsNat = Me._cMDB.RellenarDataSet("Listas", "Select nombre FROM [Lista] WHERE id_punto=" & id & " AND tipo_lista=true AND tipo_fechas=false")
            If (dsNat.Tables(0).Rows.Count > 0) Then
                haySerieMensual = True
            Else
                haySerieMensual = False
            End If


            ' Sacar alteraciones y rellenar el combobox
            'Dim dsAlt As DataSet = Me._cMDB.RellenarDataSet("Alt", "SELECT DISTINCT id_alteracion FROM [Lista] WHERE id_punto=" & id & " AND tipo_lista=false")
            Dim dsAlt As DataSet = Me._cMDB.RellenarDataSet("Alt", "SELECT DISTINCT id_alteracion, COD_Alteracion FROM [alteracion] WHERE id_punto=" & id)


            Dim idAlt As Integer
            'Dim dsAltaux As DataSet
            'Dim dralt As DataRow

            comboAltD.Items.Clear()
            comboAltD.Items.Add(stNone)

            For Each dr As DataRow In dsAlt.Tables(0).Rows

                idAlt = Integer.Parse(dr("id_alteracion"))
                'dsAltaux = Me._cMDB.RellenarDataSet("AltAux", "SELECT COD_Alteracion FROM [Alteracion] WHERE id_alteracion=" & idAlt)
                ' OJO ERROR
                'If (dsAltaux.Tables(0).Rows.Count = 0) Then
                'MessageBox.Show("No se encuentra la alteración en el sistema, pero si existe una lista en ella... (?)")
                'Exit Sub
                'End If

                'dralt = dsAltaux.Tables(0).Rows(0)
                comboAltD.Items.Add(dr("COD_Alteracion").ToString())
            Next
            If (comboAltD.Items.Count > 0) Then
                comboAltD.SelectedIndex = 0
            End If

        End Sub

        Public Function RellenarXPTable(ByRef tabla As XPTable.Models.Table, ByVal idalt As Integer, ByVal idpunto As Integer, ByVal usarCoe As Boolean, ByVal usarCoeDiaria As Boolean) As TestFechas.TestFechas.Simulacion

            Dim dsAltD As DataSet
            Dim dsAltM As DataSet
            Dim dsNatD As DataSet
            Dim dsNatM As DataSet

            Dim fechaI As Date
            Dim fechaF As Date
            Dim añoMin As Integer = 99999
            Dim añoMax As Integer = 0

            Dim i, j As Integer

            Dim añosInterAlt() As Integer = Nothing
            Dim añosInterNat() As Integer = Nothing
            Dim añosInterCoe() As Integer = Nothing

            Dim validosND As TestFechas.TestFechas.EstadoLista = Nothing
            Dim validosNM As TestFechas.TestFechas.EstadoLista = Nothing
            Dim validosAD As TestFechas.TestFechas.EstadoLista = Nothing
            Dim validosAM As TestFechas.TestFechas.EstadoLista = Nothing

            Dim coeD As TestFechas.TestFechas.Coetaniedad = Nothing
            Dim coeM As TestFechas.TestFechas.Coetaniedad = Nothing

            Dim salida As TestFechas.TestFechas.Simulacion = Nothing


            ReDim salida.idListas(3)

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++ Logica de Calculo +++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            validosND.Año = Nothing
            validosND.validos = Nothing
            validosNM.Año = Nothing
            validosNM.validos = Nothing

            ' Sacar datos validos en diarios
            dsNatD = Me._cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" & idpunto & " AND Tipo_Lista=true AND tipo_fechas=true")
            If (dsNatD.Tables(0).Rows.Count > 0) Then
                Dim dr As DataRow = dsNatD.Tables(0).Rows(0)

                fechaI = Date.Parse(dr("fecha_ini"))
                fechaF = Date.Parse(dr("fecha_fin"))
                If (añoMin > fechaI.Year) Then
                    añoMin = fechaI.Year
                End If
                If (añoMax < fechaF.Year) Then
                    añoMax = fechaF.Year
                End If

                validosND = Me._Fechas.TestDiasAno("Valor", "fecha", fechaI, fechaF, dr("id_lista"))
                salida.idListas(0) = dr("id_lista")

            End If

            ' Sacar años validos mensuales
            dsNatM = Me._cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" & idpunto & " AND Tipo_Lista=true AND tipo_fechas=false")
            If (dsNatM.Tables(0).Rows.Count > 0) Then
                Dim dr As DataRow = dsNatM.Tables(0).Rows(0)

                fechaI = Date.Parse(dr("fecha_ini"))
                fechaF = Date.Parse(dr("fecha_fin"))
                If (añoMin > fechaI.Year) Then
                    añoMin = fechaI.Year
                End If
                If (añoMax < fechaF.Year) Then
                    añoMax = fechaF.Year
                End If

                validosNM = Me._Fechas.TestMesAño("Valor", "fecha", fechaI, fechaF, dr("id_lista"))
                salida.idListas(2) = dr("id_lista")
            End If

            ' Calcular posibles interpolaciones de diario a mensual y rellanar la lista que nos
            ' indica que hay posibles interpolaciones
            ' ----
            ' Si no hay validos en diario no hay nada que interpolar
            If (validosND.nValidos <> 0) Then
                ' No hay datos mensuales, interpolo todo
                If (validosNM.nValidos = 0) Then
                    For i = 0 To validosND.Año.Length - 1
                        If (validosND.validos(i)) Then
                            If (añosInterNat Is Nothing) Then
                                ReDim añosInterNat(0)
                                añosInterNat(0) = validosND.Año(i)
                            Else
                                ReDim Preserve añosInterNat(añosInterNat.Length)
                                añosInterNat(añosInterNat.Length - 1) = validosND.Año(i)
                            End If
                        End If
                    Next
                Else
                    ' Hay datos mensuales asi que tengo que comprobar si es necesario interpolar
                    For i = 0 To validosND.Año.Length - 1
                        Dim pos As Integer = Array.BinarySearch(validosNM.Año, validosND.Año(i))
                        ' Si el año no es valido o no existe en la lista de Natural Mensual
                        If (pos < 0) Then
                            If (validosND.validos(i)) Then
                                If (añosInterNat Is Nothing) Then
                                    ReDim añosInterNat(0)
                                    añosInterNat(0) = validosND.Año(i)
                                Else
                                    ReDim Preserve añosInterNat(añosInterNat.Length)
                                    añosInterNat(añosInterNat.Length - 1) = validosND.Año(i)
                                End If
                            End If
                        ElseIf (Not validosNM.validos(pos)) Then
                            If (validosND.validos(i)) Then
                                If (añosInterNat Is Nothing) Then
                                    ReDim añosInterNat(0)
                                    añosInterNat(0) = validosND.Año(i)
                                Else
                                    ReDim Preserve añosInterNat(añosInterNat.Length)
                                    añosInterNat(añosInterNat.Length - 1) = validosND.Año(i)
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            If idalt <> -1 Then

                dsAltD = Me._cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" & idpunto & " AND Tipo_Lista=false AND tipo_fechas=true AND id_alteracion=" & idalt)
                dsAltM = Me._cMDB.RellenarDataSet("Listas", "Select * FROM [Lista] WHERE id_punto=" & idpunto & " AND Tipo_Lista=false AND tipo_fechas=false AND id_alteracion=" & idalt)


                Dim dr As DataRow

                If (dsAltD.Tables(0).Rows.Count > 0) Then
                    dr = dsAltD.Tables(0).Rows(0)

                    fechaI = Date.Parse(dr("fecha_ini"))
                    fechaF = Date.Parse(dr("fecha_fin"))
                    If (añoMin > fechaI.Year) Then
                        añoMin = fechaI.Year
                    End If
                    If (añoMax < fechaF.Year) Then
                        añoMax = fechaF.Year
                    End If

                    validosAD = Me._Fechas.TestDiasAno("Valor", "fecha", fechaI, fechaF, dr("id_lista"))
                    salida.idListas(1) = dr("id_lista")

                End If

                If (dsAltM.Tables(0).Rows.Count > 0) Then

                    dr = dsAltM.Tables(0).Rows(0)

                    fechaI = Date.Parse(dr("fecha_ini"))
                    fechaF = Date.Parse(dr("fecha_fin"))
                    If (añoMin > fechaI.Year) Then
                        añoMin = fechaI.Year
                    End If
                    If (añoMax < fechaF.Year) Then
                        añoMax = fechaF.Year
                    End If


                    validosAM = Me._Fechas.TestMesAño("Valor", "fecha", fechaI, fechaF, dr("id_lista"))
                    salida.idListas(3) = dr("id_lista")

                End If

                ' Buscar posibles interpolaciones
                ' 
                If (validosAD.nValidos <> 0) Then
                    ' No hay datos mensuales, interpolo todo
                    If (validosAM.nValidos = 0) Then
                        For i = 0 To validosAD.Año.Length - 1
                            If (validosAD.validos(i)) Then
                                If (añosInterAlt Is Nothing) Then
                                    ReDim añosInterAlt(0)
                                    añosInterAlt(0) = validosAD.Año(i)
                                Else
                                    ReDim Preserve añosInterAlt(añosInterAlt.Length)
                                    añosInterAlt(añosInterAlt.Length - 1) = validosAD.Año(i)
                                End If
                            End If
                        Next
                    Else
                        ' Hay datos mensuales asi que tengo que comprobar si es necesario interpolar
                        For i = 0 To validosAD.Año.Length - 1
                            Dim pos As Integer = Array.BinarySearch(validosAM.Año, validosAD.Año(i))
                            ' Si el año no es valido o no existe en la lista de Natural Mensual
                            If (pos < 0) Then
                                If (validosAD.validos(i)) Then
                                    If (añosInterAlt Is Nothing) Then
                                        ReDim añosInterAlt(0)
                                        añosInterAlt(0) = validosAD.Año(i)
                                    Else
                                        ReDim Preserve añosInterAlt(añosInterAlt.Length)
                                        añosInterAlt(añosInterAlt.Length - 1) = validosAD.Año(i)
                                    End If
                                End If
                            ElseIf (Not validosAM.validos(pos)) Then
                                If (validosAD.validos(i)) Then
                                    If (añosInterAlt Is Nothing) Then
                                        ReDim añosInterAlt(0)
                                        añosInterAlt(0) = validosAD.Año(i)
                                    Else
                                        ReDim Preserve añosInterAlt(añosInterAlt.Length)
                                        añosInterAlt(añosInterAlt.Length - 1) = validosAD.Año(i)
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If

            End If

            ' Calcular coetaniedad
            coeD = Me._Fechas.calcularcoetaniedad(validosND, validosAD, Nothing, Nothing)
            coeM = Me._Fechas.calcularcoetaniedad(validosNM, validosAM, añosInterNat, añosInterAlt)


            salida.añosInterAlt = añosInterAlt
            salida.añosInterNat = añosInterNat
            'salida.añosInterCoe = añosInterCoe

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++++ Rellenar el XPTable +++++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' Años que usare en el calculo
            ReDim salida.añosParaCalculo(3)
            For i = 0 To 3
                salida.añosParaCalculo(i).nAños = 0
            Next

            ' Borro la tabla
            tabla.TableModel.Rows.Clear()

            ' -----------------------------
            ' Bucle de años
            ' -----------------------------
            For i = añoMin To añoMax - 1

                ' Definicion de las celdas
                Dim cells() As Cell
                ReDim cells(10)

                For j = 0 To 10
                    cells(j) = New Cell("")
                    cells(j).Checked = False
                Next

                cells(0) = New Cell(i.ToString() & "-" & (i + 1).ToString()) ' Celda del año


                ' +++++++++++++++++
                ' Naturales diarios
                ' +++++++++++++++++
                If (validosND.Año Is Nothing) Then
                    cells(6) = New Cell("NS", My.Resources.delete2)
                Else
                    Dim pos As Integer = Array.BinarySearch(validosND.Año, i)

                    If (pos > -1) Then
                        If (validosND.validos(pos)) Then
                            cells(6) = New Cell("C", My.Resources.check)
                        Else
                            cells(6) = New Cell("I", My.Resources.no_information)
                        End If
                    Else
                        cells(6) = New Cell("SD", My.Resources.delete2)
                    End If
                End If

                ' ++++++++++++++++
                ' Alterados Diario
                ' ++++++++++++++++
                If (validosAD.Año Is Nothing) Then
                    cells(8) = New Cell("NS", My.Resources.delete2)
                Else
                    Dim pos As Integer = Array.BinarySearch(validosAD.Año, i)

                    If (pos > -1) Then
                        If (validosAD.validos(pos)) Then
                            cells(8) = New Cell("C", My.Resources.check)
                        Else
                            cells(8) = New Cell("I", My.Resources.no_information)
                        End If
                    Else
                        cells(8) = New Cell("SD", My.Resources.delete2)
                    End If
                End If

                ' +++++++++++++++++++++++++++
                ' +++++++ Coe Diarios +++++++
                ' +++++++++++++++++++++++++++
                If (coeD.nCoetaneos = 0) Then
                    cells(10) = New Cell("", My.Resources.delete2)
                Else
                    Dim pos As Integer = Array.BinarySearch(coeD.añosCoetaneos, i)
                    If (pos > -1) Then
                        cells(10) = New Cell("CO", My.Resources.check)
                    Else
                        cells(10) = New Cell("NCO", My.Resources.delete2)
                    End If
                End If

                ' +++++++++++++++++++
                ' Naturales mensuales
                ' +++++++++++++++++++
                ' Incluido la posible interpolacion de los diarios!!!
                If (validosNM.Año Is Nothing) Then
                    If (cells(6).Text = "C") Then
                        'If (añosInterNat Is Nothing) Then
                        '    ReDim añosInterNat(0)
                        '    añosInterNat(0) = i
                        'Else
                        '    ReDim Preserve añosInterNat(añosInterNat.Length)
                        '    añosInterNat(añosInterNat.Length - 1) = i
                        'End If
                        cells(1) = New Cell("CD", My.Resources.check)
                    Else
                        cells(1) = New Cell("SD", My.Resources.delete2)
                    End If

                Else
                    Dim pos As Integer = Array.BinarySearch(validosNM.Año, i)

                    If (pos > -1) Then
                        If (validosNM.validos(pos)) Then
                            cells(1) = New Cell("C", My.Resources.check)
                        Else
                            If (Not validosND.validos Is Nothing) Then
                                pos = Array.BinarySearch(validosND.Año, i)
                                If (pos > -1) Then
                                    If (validosND.validos(pos)) Then
                                        'If (añosInterNat Is Nothing) Then
                                        '    ReDim añosInterNat(0)
                                        '    añosInterNat(0) = i
                                        'Else
                                        '    ReDim Preserve añosInterNat(añosInterNat.Length)
                                        '    añosInterNat(añosInterNat.Length - 1) = i
                                        'End If
                                        cells(1) = New Cell("CD", My.Resources.check)
                                    Else
                                        cells(1) = New Cell("I", My.Resources.no_information)
                                    End If
                                Else
                                    cells(1) = New Cell("I", My.Resources.no_information)
                                End If
                            Else
                                cells(1) = New Cell("SD", My.Resources.delete2)
                            End If
                        End If
                    Else
                        If (cells(6).Text = "C") Then
                            'If (añosInterNat Is Nothing) Then
                            '    ReDim añosInterNat(0)
                            '    añosInterNat(0) = i
                            'Else
                            '    ReDim Preserve añosInterNat(añosInterNat.Length)
                            '    añosInterNat(añosInterNat.Length - 1) = i
                            'End If
                            cells(1) = New Cell("CD", My.Resources.check)
                        Else
                            cells(1) = New Cell("SD", My.Resources.delete2)
                        End If
                    End If
                End If

                ' +++++++++++++++++
                ' Alterados Mensual
                ' +++++++++++++++++
                If (validosAM.Año Is Nothing) Then
                    If (cells(8).Text = "C") Then
                        'If (añosInterAlt Is Nothing) Then
                        '    ReDim añosInterAlt(0)
                        '    añosInterAlt(0) = i
                        'Else
                        '    ReDim Preserve añosInterAlt(añosInterAlt.Length)
                        '    añosInterAlt(añosInterAlt.Length - 1) = i
                        'End If
                        cells(3) = New Cell("CD", My.Resources.check)
                    Else
                        cells(3) = New Cell("SD", My.Resources.delete2)
                    End If

                Else
                    Dim pos As Integer = Array.BinarySearch(validosAM.Año, i)

                    If (pos > -1) Then
                        If (validosAM.validos(pos)) Then
                            cells(3) = New Cell("C", My.Resources.check)
                        Else
                            'If (Not validosND.validos Is Nothing) Then
                            '    pos = Array.BinarySearch(validosND.Año, i)
                            If (Not validosAD.validos Is Nothing) Then
                                pos = Array.BinarySearch(validosAD.Año, i)
                                If (pos > -1) Then
                                    If (validosAD.validos(pos)) Then
                                        'If (añosInterAlt Is Nothing) Then
                                        '    ReDim añosInterAlt(0)
                                        '    añosInterAlt(0) = i
                                        'Else
                                        '    ReDim Preserve añosInterAlt(añosInterAlt.Length)
                                        '    añosInterAlt(añosInterAlt.Length - 1) = i
                                        'End If
                                        cells(3) = New Cell("CD", My.Resources.check)
                                    Else
                                        cells(3) = New Cell("I", My.Resources.no_information)
                                    End If
                                Else
                                    cells(3) = New Cell("I", My.Resources.no_information)
                                End If
                            Else
                                cells(3) = New Cell("SD", My.Resources.delete2)
                            End If
                        End If
                    Else
                        'pos = Array.BinarySearch(validosAD.Año, i)
                        If (cells(8).Text = "C") Then
                            'If (validosAD.validos(pos)) Then
                            'If (añosInterAlt Is Nothing) Then
                            '    ReDim añosInterAlt(0)
                            '    añosInterAlt(0) = i
                            'Else
                            '    ReDim Preserve añosInterAlt(añosInterAlt.Length)
                            '    añosInterAlt(añosInterAlt.Length - 1) = i
                            'End If
                            cells(3) = New Cell("CD", My.Resources.check)
                        Else
                            cells(3) = New Cell("SD", My.Resources.delete2)
                        End If
                        'Else
                        'cells(8) = New Cell("Sin datos", My.Resources.delete2)
                        'End If
                    End If
                End If


                ' +++++++++++++++++++
                ' Coetaneos mensuales
                ' +++++++++++++++++++
                If (coeM.nCoetaneos = 0) Then
                    If (cells(1).Text = "CD" And cells(3).Text = "CD") Then
                        'If (añosInterCoe Is Nothing) Then
                        '    ReDim añosInterCoe(0)
                        '    añosInterCoe(0) = i
                        'Else
                        '    ReDim Preserve añosInterCoe(añosInterCoe.Length)
                        '    añosInterCoe(añosInterCoe.Length - 1) = i
                        'End If
                        cells(5) = New Cell("CO", My.Resources.check)

                    ElseIf (cells(1).Text = "C" And cells(3).Text = "CD") Then
                        'If (añosInterCoe Is Nothing) Then
                        '    ReDim añosInterCoe(0)
                        '    añosInterCoe(0) = i
                        'Else
                        '    ReDim Preserve añosInterCoe(añosInterCoe.Length)
                        '    añosInterCoe(añosInterCoe.Length - 1) = i
                        'End If
                        cells(5) = New Cell("CO", My.Resources.check)

                    ElseIf (cells(1).Text = "CD" And cells(3).Text = "C") Then
                        'If (añosInterCoe Is Nothing) Then
                        '    ReDim añosInterCoe(0)
                        '    añosInterCoe(0) = i
                        'Else
                        '    ReDim Preserve añosInterCoe(añosInterCoe.Length)
                        '    añosInterCoe(añosInterCoe.Length - 1) = i
                        'End If
                        cells(5) = New Cell("CO", My.Resources.check)

                    Else
                        cells(5) = New Cell("", My.Resources.delete2)
                    End If
                Else
                    Dim pos As Integer = Array.BinarySearch(coeM.añosCoetaneos, i)

                    If (pos > -1) Then
                        cells(5) = New Cell("CO", My.Resources.check)
                    Else
                        cells(5) = New Cell("NCO", My.Resources.delete2)
                    End If
                End If

                ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' +++++++++++ Ahora estudiar que años se usan +++++++++++++++++++++
                ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                Dim nAñosIntN As Integer = 0
                Dim nAñosIntA As Integer = 0
                If (Not añosInterNat Is Nothing) Then
                    nAñosIntN = añosInterNat.Length
                End If
                If (Not añosInterAlt Is Nothing) Then
                    nAñosIntA = añosInterAlt.Length
                End If

                ' La serie donde se almacenan los años que participan en el calculo
                ' tienen el siguiente orden:
                ' 0 -> Nat diaria
                ' 1 -> Alt diaria
                ' 2 -> Nat mensual
                ' 3 -> Alt mensual

                If ((validosNM.nValidos + nAñosIntN) >= 15) Then

                    If ((validosND.nValidos) >= 15) Then
                        If (usarCoeDiaria) Then
                            If (cells(10).Text = "CO") Then
                                If (cells(6).Text = "C") Then
                                    cells(7).Text = ""
                                    cells(7).Checked = True
                                    ' Añadir a la lista de años para calcular
                                    ReDim Preserve salida.añosParaCalculo(0).año(salida.añosParaCalculo(0).nAños)
                                    salida.añosParaCalculo(0).año(salida.añosParaCalculo(0).nAños) = i
                                    salida.añosParaCalculo(0).nAños = salida.añosParaCalculo(0).nAños + 1
                                End If
                                If ((validosAD.nValidos) >= 15) Then
                                    If (cells(8).Text = "C") Then
                                        cells(9).Text = ""
                                        cells(9).Checked = True
                                        ' Añadir a la lista de años para calcular
                                        ReDim Preserve salida.añosParaCalculo(1).año(salida.añosParaCalculo(1).nAños)
                                        salida.añosParaCalculo(1).año(salida.añosParaCalculo(1).nAños) = i
                                        salida.añosParaCalculo(1).nAños = salida.añosParaCalculo(1).nAños + 1
                                    End If
                                End If

                            End If
                        Else
                            If (cells(6).Text = "C") Then
                                cells(7).Text = ""
                                cells(7).Checked = True
                                ' Añadir a la lista de años para calcular
                                ReDim Preserve salida.añosParaCalculo(0).año(salida.añosParaCalculo(0).nAños)
                                salida.añosParaCalculo(0).año(salida.añosParaCalculo(0).nAños) = i
                                salida.añosParaCalculo(0).nAños = salida.añosParaCalculo(0).nAños + 1
                            End If
                            If ((validosAD.nValidos) >= 15) Then
                                If (cells(8).Text = "C") Then
                                    cells(9).Text = ""
                                    cells(9).Checked = True
                                    ' Añadir a la lista de años para calcular
                                    ReDim Preserve salida.añosParaCalculo(1).año(salida.añosParaCalculo(1).nAños)
                                    salida.añosParaCalculo(1).año(salida.añosParaCalculo(1).nAños) = i
                                    salida.añosParaCalculo(1).nAños = salida.añosParaCalculo(1).nAños + 1
                                End If
                            End If
                        End If
                    End If

                    If (usarCoe) Then
                        If (cells(5).Text = "CO") Then
                            If (cells(1).Text = "C" Or cells(1).Text = "CD") Then
                                cells(2).Text = ""
                                cells(2).Checked = True
                                ' Añadir a la lista de años para calcular
                                ReDim Preserve salida.añosParaCalculo(2).año(salida.añosParaCalculo(2).nAños)
                                salida.añosParaCalculo(2).año(salida.añosParaCalculo(2).nAños) = i
                                salida.añosParaCalculo(2).nAños = salida.añosParaCalculo(2).nAños + 1
                            End If

                            If ((validosAM.nValidos + nAñosIntA) >= 15) Then
                                If (cells(3).Text = "C" Or cells(3).Text = "CD") Then
                                    cells(4).Text = ""
                                    cells(4).Checked = True
                                    ' Añadir a la lista de años para calcular
                                    ReDim Preserve salida.añosParaCalculo(3).año(salida.añosParaCalculo(3).nAños)
                                    salida.añosParaCalculo(3).año(salida.añosParaCalculo(3).nAños) = i
                                    salida.añosParaCalculo(3).nAños = salida.añosParaCalculo(3).nAños + 1
                                End If
                            End If

                        End If
                    Else
                        If (cells(1).Text = "C" Or cells(1).Text = "CD") Then
                            cells(2).Text = ""
                            cells(2).Checked = True
                            ' Añadir a la lista de años para calcular
                            ReDim Preserve salida.añosParaCalculo(2).año(salida.añosParaCalculo(2).nAños)
                            salida.añosParaCalculo(2).año(salida.añosParaCalculo(2).nAños) = i
                            salida.añosParaCalculo(2).nAños = salida.añosParaCalculo(2).nAños + 1
                        End If

                        If ((validosAM.nValidos + nAñosIntA) >= 15) Then
                            If (cells(3).Text = "C" Or cells(3).Text = "CD") Then
                                cells(4).Text = ""
                                cells(4).Checked = True
                                ' Añadir a la lista de años para calcular
                                ReDim Preserve salida.añosParaCalculo(3).año(salida.añosParaCalculo(3).nAños)
                                salida.añosParaCalculo(3).año(salida.añosParaCalculo(3).nAños) = i
                                salida.añosParaCalculo(3).nAños = salida.añosParaCalculo(3).nAños + 1
                            End If
                        End If

                    End If

                End If

                'cells(1).BackColor = Color.LightGray
                'cells(2).BackColor = Color.LightGray
                'cells(3).BackColor = Color.LightGray
                'cells(4).BackColor = Color.LightGray
                'cells(5).BackColor = Color.LightGray

                tabla.TableModel.Rows.Add(New Row(cells))

            Next

            ' Crear la salida de la simulacion
            salida.fechaINI = añoMin
            salida.fechaFIN = añoMax
            salida.usarCoe = usarCoe
            salida.usarCoeDiara = usarCoeDiaria
            Dim c() As TestFechas.TestFechas.Coetaniedad = {coeD, coeM}
            Dim v() As TestFechas.TestFechas.EstadoLista = {validosND, validosAD, validosNM, validosAM}


            salida.coe = c
            salida.listas = v

            Return salida
        End Function


        Public Sub RellenarProyectos(ByRef combo As System.Windows.Forms.ComboBox)
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Proyectos", "SELECT nombre FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC")

            combo.Items.Clear()

            For Each dr As DataRow In ds.Tables(0).Rows
                combo.Items.Add(dr(0).ToString())
            Next

            If (combo.Items.Count > 0) Then
                combo.SelectedIndex = 0
            End If
        End Sub

        Public Sub RellenarProyectos(ByRef combo As System.Windows.Forms.ComboBox, ByVal id As Integer)
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Proyectos", "SELECT nombre, ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC")
            Dim index As Integer
            Dim i As Integer

            combo.Items.Clear()

            index = 0
            i = 0
            For Each dr As DataRow In ds.Tables(0).Rows
                combo.Items.Add(dr("nombre").ToString())
                If (dr("ID_Proyecto") = id) Then
                    index = i
                End If
                i += 1
            Next

            If (combo.Items.Count > 0) Then
                combo.SelectedIndex = index
            End If
        End Sub

        Public Function RellenarProyectosDesc(ByRef label As System.Windows.Forms.Label, ByVal id As Integer) As Integer

            If (id = -1) Then
                Return -1
            End If

            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Proyectos", "SELECT descripcion, ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC")

            Dim dr As DataRow = ds.Tables(0).Rows(id)

            If (Not label Is Nothing) Then
                label.Text = dr(0).ToString()
            End If

            Return dr(1)

        End Function
    End Class

End Namespace
