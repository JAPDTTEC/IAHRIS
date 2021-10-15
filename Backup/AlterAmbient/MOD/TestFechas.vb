Imports IAHRIS.BBDD.OleDbDataBase
Imports System.Data
Namespace TestFechas

    Public Class TestFechas

        Private _cMDB As BBDD.OleDbDataBase

        Structure EstadoLista
            Public validos() As Boolean
            Public nValidos As Integer
            Public A�o() As Integer
        End Structure

        Structure Coetaniedad
            Public a�oINI As Integer
            Public a�oFIN As Integer
            Public tama�o As Integer
            Public nCoetaneos As Integer
            Public a�osCoetaneos() As Integer
        End Structure

        Structure a�osCalculo
            Public a�o() As Integer
            Public nA�os As Integer
        End Structure

        Structure Simulacion
            Public sNombre As String
            Public sAlteracion As String
            Public fechaINI As Integer
            Public fechaFIN As Integer
            Public listas() As EstadoLista
            Public idListas() As Integer
            Public coe() As Coetaniedad
            Public a�osInterNat() As Integer
            Public a�osInterAlt() As Integer
            Public a�osInterCoe() As Integer
            Public usarCoe As Boolean
            Public usarCoeDiara As Boolean
            Public a�osParaCalculo() As a�osCalculo
            Public a�osCoetaneosTotales As Integer
            Public mesInicio As Integer
        End Structure

        Structure GeneracionInformes
            Public inf1 As Boolean
            Public inf1b As Boolean
            Public inf2 As Boolean
            Public inf3 As Boolean
            Public inf4a As Boolean
            Public inf4b As Boolean ' Nuevo 12/4/10
            Public inf4 As Boolean
            Public inf5a As Boolean
            Public inf5b As Boolean
            Public inf5c As Boolean
            Public inf5 As Boolean
            Public inf6a As Boolean
            Public inf6 As Boolean
            Public inf7a As Boolean
            Public inf7b As Boolean
            Public inf7c As Boolean
            Public inf7d As Boolean
            Public inf7_3 As Boolean
            Public inf8 As Boolean
            Public inf8a As Boolean ' Nuevo 12/4/10
            Public inf8b As Boolean ' Nuevo 12/4/10
            Public inf8c As Boolean ' Nuevo 12/4/10
            Public inf8d As Boolean ' Nuevo 12/4/10
            Public inf9 As Boolean
            Public inf9a As Boolean ' Nuevo 12/4/10
        End Structure

        Public Sub New(ByVal MDB As BBDD.OleDbDataBase)
            Me._cMDB = MDB
        End Sub

        Public Function TestDiasAno(ByVal tabla As String, ByVal campo As String, ByVal fechaInicial As Date, ByVal fechaFinal As Date, ByVal idlista As Integer) As EstadoLista

            Dim i As Integer
            'Dim dr As DataRow
            Dim fechaINI As Date
            Dim fechaFIN As Date
            Dim nRevisar As Integer

            Dim a�o() As Integer
            Dim valido() As Boolean

            Dim estado As EstadoLista

            Dim diasEnAno As Integer


            ' �Cuantos a�os hay que mirar?
            nRevisar = fechaFinal.Year - fechaInicial.Year
            ReDim a�o(nRevisar - 1)
            ReDim valido(nRevisar - 1)


            ' Calculo las primeras fechas
            fechaFIN = fechaInicial.AddDays(-1)

            estado.nValidos = 0

            For i = 0 To nRevisar - 1

                fechaINI = fechaFIN.AddDays(1)
                fechaFIN = fechaFIN.AddYears(1)

                If (Date.IsLeapYear(fechaFIN.Year) And fechaFIN.Month = 2) Then
                    fechaFIN = fechaFIN.AddDays(1)
                End If
                'fechaFIN = fechaFIN.AddDays(-1)

                diasEnAno = 365

                ' quitar fecha y poner campo
                Dim sSQL As String = "SELECT " & campo & " FROM " & tabla & " WHERE " & campo & " BETWEEN #" & fechaINI.ToString("yyyy-MM-dd") & "# and #" & fechaFIN.ToString("yyyy-MM-dd") & "# AND id_lista=" & idlista & " ORDER BY " & campo & " DESC"
                Dim ds As DataSet = Me._cMDB.RellenarDataSet("tabla", sSQL)

                ' Si la fecha es de a�o Bisiesto, se suma un dia
                ' ----------------------------------------------
                Dim posibleA�oBisiesto As Integer
                If (fechaINI.Month <= 2) Then
                    posibleA�oBisiesto = fechaINI.Year
                Else
                    posibleA�oBisiesto = fechaFIN.Year
                End If

                If (Date.IsLeapYear(posibleA�oBisiesto)) Then
                    diasEnAno = 366
                End If
                a�o(i) = fechaINI.Year
                If (ds.Tables(0).Rows.Count = diasEnAno) Then
                    valido(i) = True
                    estado.nValidos = estado.nValidos + 1
                Else
                    valido(i) = False
                End If
            Next

            estado.A�o = a�o
            estado.validos = valido

            Return estado
        End Function
        Public Function TestMesA�o(ByVal tabla As String, ByVal campo As String, ByVal fechaInicial As Date, ByVal fechaFinal As Date, ByVal idlista As Integer) As EstadoLista

            Dim i As Integer

            Dim estado As EstadoLista
            Dim nRevisar As Integer
            Dim a�o() As Integer
            Dim valido() As Boolean
            Dim fechaINI As Date
            Dim fechaFIN As Date
            'Dim a�oAct As Integer
            'Dim a�oAnt As Integer
            'Dim mesAct As Integer
            'Dim ok As Boolean

            estado = Nothing

            nRevisar = fechaFinal.Year - fechaInicial.Year
            ReDim a�o(nRevisar - 1)
            ReDim valido(nRevisar - 1)

            fechaFIN = fechaInicial.AddDays(-1)


            estado.nValidos = 0

            For i = 0 To nRevisar - 1

                fechaINI = fechaFIN.AddDays(1)
                fechaFIN = fechaFIN.AddYears(1)

                ' quitar fecha y poner campo
                Dim sSQL As String = "SELECT " & campo & " FROM " & tabla & " WHERE " & campo & " BETWEEN #" & fechaINI.ToString("yyyy-MM-dd") & "# and #" & fechaFIN.ToString("yyyy-MM-dd") & "# AND id_lista=" & idlista & " ORDER BY " & campo & " DESC"
                Dim ds As DataSet = Me._cMDB.RellenarDataSet("tabla", sSQL)

                If (ds.Tables(0).Rows.Count <> 12) Then
                    valido(i) = False
                Else
                    valido(i) = True
                    estado.nValidos = estado.nValidos + 1
                End If

                a�o(i) = fechaINI.Year

            Next

            estado.A�o = a�o
            estado.validos = valido

            Return estado
        End Function
        Public Function ComprobarFechasCSV(ByVal tipoFechas As Boolean, ByVal fecha As String, ByRef fechaDate As Date) As Boolean

            Dim dt As Date

            If (tipoFechas) Then
                Try
                    dt = Date.ParseExact(fecha, "dd/MM/yyyy", Nothing)
                Catch ex As Exception
                    Return False
                End Try
            Else
                Try
                    dt = Date.ParseExact(fecha, "MM/yyyy", Nothing)
                Catch ex As Exception
                    Try
                        dt = Date.ParseExact(fecha, "M/yyyy", Nothing)
                    Catch ext As Exception
                        Return False
                    End Try
                End Try
            End If

            fechaDate = dt

            Return True
        End Function

        Public Function calcularcoetaniedad(ByVal l1 As EstadoLista, ByVal l2 As EstadoLista, ByVal interNat() As Integer, ByVal interAlt() As Integer) As Coetaniedad
            Dim salida As Coetaniedad
            Dim i As Integer

            'If (l1.A�o Is Nothing) Or (l2.A�o Is Nothing) Then
            '    salida.nCoetaneos = 0
            '    salida.tama�o = 0
            '    salida.a�osCoetaneos = Nothing
            '    Return salida
            'End If

            salida.a�oINI = 99999
            salida.a�oFIN = 0
            salida.nCoetaneos = 0
            salida.tama�o = 0
            salida.a�osCoetaneos = Nothing

            If (Not l1.A�o Is Nothing) Then
                If (l1.A�o(l1.A�o.Length - 1) > salida.a�oFIN) Then
                    salida.a�oFIN = l1.A�o(l1.A�o.Length - 1)
                End If
                If (l1.A�o(0) < salida.a�oINI) Then
                    salida.a�oINI = l1.A�o(0)
                End If
            End If
            If (Not l2.A�o Is Nothing) Then
                If (l2.A�o(l2.A�o.Length - 1) > salida.a�oFIN) Then
                    salida.a�oFIN = l2.A�o(l2.A�o.Length - 1)
                End If
                If (l2.A�o(0) < salida.a�oINI) Then
                    salida.a�oINI = l2.A�o(0)
                End If
            End If
            If (Not interNat Is Nothing) Then
                If (interNat(interNat.Length - 1) > salida.a�oFIN) Then
                    salida.a�oFIN = interNat(interNat.Length - 1)
                End If
                If (interNat(0) < salida.a�oINI) Then
                    salida.a�oINI = interNat(0)
                End If
            End If
            If (Not interAlt Is Nothing) Then
                If (interAlt(interAlt.Length - 1) > salida.a�oFIN) Then
                    salida.a�oFIN = interAlt(interAlt.Length - 1)
                End If
                If (interAlt(0) < salida.a�oINI) Then
                    salida.a�oINI = interAlt(0)
                End If
            End If

            'If (l1.A�o(0) > l2.A�o(0)) Then
            '    salida.a�oINI = l2.A�o(0)
            'Else
            '    salida.a�oINI = l1.A�o(0)
            'End If

            'If (l1.A�o(l1.A�o.Length - 1) > l2.A�o(l1.A�o.Length - 1)) Then
            '    salida.a�oFIN = l1.A�o(l1.A�o.Length - 1)
            'Else
            '    salida.a�oFIN = l2.A�o(l1.A�o.Length - 1)
            'End If

            salida.tama�o = salida.a�oFIN - salida.a�oINI + 1

            'ReDim salida.a�osCoetaneos(salida.tama�o - 1)
            salida.nCoetaneos = 0

            For i = salida.a�oINI To salida.a�oFIN

                Dim escoe As Boolean = False

                Dim pos As Integer
                If (Not l1.A�o Is Nothing) Then
                    pos = Array.BinarySearch(l1.A�o, i)
                Else
                    pos = -1
                End If
                Dim pos2 As Integer
                If (Not l2.A�o Is Nothing) Then
                    pos2 = Array.BinarySearch(l2.A�o, i)
                Else
                    pos2 = -1
                End If
                ' �Es el a�o NAT interpolado?
                Dim pos3 As Integer
                If (Not interNat Is Nothing) Then
                    pos3 = Array.BinarySearch(interNat, i)
                Else
                    pos3 = -1
                End If
                ' �Es el a�o ALT interpolado?
                Dim pos4 As Integer
                If (Not interAlt Is Nothing) Then
                    pos4 = Array.BinarySearch(interAlt, i)
                Else
                    pos4 = -1
                End If

                If ((pos > -1) And (pos2 > -1)) Then
                    If ((l1.validos(pos)) And (l2.validos(pos2))) Then
                        escoe = True
                    End If
                ElseIf ((pos3 > -1) And (pos2 > -1)) Then
                    If (l2.validos(pos2)) Then
                        escoe = True
                    End If
                ElseIf ((pos > -1) And (pos4 > -1)) Then
                    If (l1.validos(pos)) Then
                        escoe = True
                    End If
                ElseIf ((pos3 > -1) And (pos4 > -1)) Then
                    escoe = True
                End If

                If (escoe) Then
                    ReDim Preserve salida.a�osCoetaneos(salida.nCoetaneos)
                    salida.a�osCoetaneos(salida.nCoetaneos) = i
                    salida.nCoetaneos = salida.nCoetaneos + 1
                End If
            Next


            Return salida
        End Function
        Public Function CalcularCoetaniedad(ByVal idPunto As Integer) As Coetaniedad

            ' Saco las lista asociadas a ese punto
            Dim sSQL As String = "SELECT id_Lista, fecha_ini, fecha_fin FROM [Lista] WHERE id_punto=" & idPunto
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("listas", sSQL)

            Dim fechaINI As Date
            Dim fechaFIN As Date
            Dim idlista As Integer

            Dim salida As Coetaniedad = Nothing

            Dim estado() As EstadoLista

            Dim i As Integer = 0, j As Integer = 0

            ReDim estado(ds.Tables(0).Rows.Count - 1)

            For Each dr As DataRow In ds.Tables(0).Rows
                fechaINI = Date.Parse(dr("fecha_ini"))
                fechaFIN = Date.Parse(dr("fecha_fin"))
                idlista = Integer.Parse(dr("id_lista"))
                estado(i) = Me.TestDiasAno("valor", "fecha", fechaINI, fechaFIN, idlista)
                i = i + 1
            Next


            Dim tama�o As Integer = 0
            Dim a�oIni As Integer = 99999
            Dim a�oFin As Integer = 0
            For i = 0 To estado.Length - 1
                If (a�oIni > estado(i).A�o(0)) Then
                    a�oIni = estado(i).A�o(0)
                End If

                If (a�oFin < estado(i).A�o(estado(i).A�o.Length - 1)) Then
                    a�oFin = estado(i).A�o(estado(i).A�o.Length - 1)
                End If
            Next
            tama�o = a�oFin - a�oIni + 1

            salida.a�oINI = a�oIni
            salida.a�oFIN = a�oFin
            salida.tama�o = tama�o


            For i = 0 To tama�o - 1 ' Recorro todos los a�os

                Dim coe As Boolean = True

                For j = 0 To estado.Length - 1
                    Dim pos As Integer = Array.BinarySearch(estado(j).A�o, a�oIni)

                    If (pos = -1) Then
                        coe = False
                        Exit For
                    Else
                        coe = coe And estado(j).validos(pos)
                        If (coe = False) Then
                            Exit For
                        End If
                    End If
                Next
                If (coe) Then
                    ReDim Preserve salida.a�osCoetaneos(salida.nCoetaneos)
                    salida.nCoetaneos = salida.a�osCoetaneos.Length
                    salida.a�osCoetaneos(salida.a�osCoetaneos.Length - 1) = a�oIni
                End If
                a�oIni = a�oIni + 1
            Next

            Return salida
        End Function
    End Class
End Namespace
