Imports System.Math
Imports Microsoft.Office.Interop
Imports IAHRIS.TestFechas.TestFechas

Namespace Calculo



    Public Class Calculo

#Region "Constantes EXCEL"
        Const xlDiagonalDown = 5
        Const xlPasteFormats = -4122
        Const xlCenter = -4108
        Const xlGeneral = 1
        Const xlBottom = -4107

        Const xlUnderlineStyleNone = -4142
        Const xlAutomatic = -4105
        Const xlNone = -4142
        Const xlContinuous = 1
        Const xlThin = 2
        Const xlDiagonalUp = 6
        Const xlEdgeLeft = 7
        Const xlEdgeTop = 8
        Const xlEdgeBottom = 9
        Const xlEdgeRight = 10
        Const xlInsideVertical = 11
#End Region

        Enum TIPOAÑO
            HUMEDO
            MEDIO
            SECO
        End Enum

        Enum STRING_MES
            OCT = 0
            NOV
            DIC
            ENE
            FEB
            MAR
            ABR
            MAY
            JUN
            JUL
            AGO
            SEP
        End Enum

        Enum STRING_MES_ORD
            ENE = 0
            FEB
            MAR
            ABR
            MAY
            JUN
            JUL
            AGO
            SEP
            OCT
            NOV
            DIC
        End Enum

        Enum STRING_MES_COMPLETO
            Enero = 0
            Febrero
            Marzo
            Abril
            Mayo
            Junio
            Julio
            Agosto
            Septiembre
            Octubre
            Noviembre
            Diciembre
        End Enum

        Structure SerieAnual
            Public año() As Integer
            Public caudalAnual() As Single
            Public nAños As Integer
        End Structure

        Structure SerieMensual
            Public mes() As Date
            Public caudalMensual() As Single
            Public nMeses As Integer
            Public nAños As Integer
        End Structure

        Structure SerieDiaria
            Public dia() As Date
            Public caudalDiaria() As Single
            Public nAños As Integer
        End Structure

        Structure AportacionMensual
            Public mes() As Date
            Public aportacion() As Single
        End Structure

        Structure AportacionAnual
            Public año() As Integer
            Public aportacion() As Single
            Public tipo() As TIPOAÑO
        End Structure

        Structure GraficoAlterada
            Public año As Integer
            Public tipo As TIPOAÑO
            Public apNat As Single
            Public apAlt As Single
            Public porcentaje As Single
        End Structure

        Structure Indices
            Public valor() As Single
            Public invertido() As Boolean
            Public indeterminacion() As Boolean
            Public calculado As Boolean
        End Structure

        ''' <summary>
        ''' Datos necesarios para el analisis Kruskal-Wallis
        ''' </summary>
        ''' <remarks></remarks>
        Structure DatosKS
            Public datosMes()() As Single
            Public tipoMes()() As TIPOAÑO
            Public tipoMesKS()() As TIPOAÑO
        End Structure

        ''' <summary>
        ''' Datos iniciales para realizar el calculo
        ''' </summary>
        ''' <remarks></remarks>
        Structure DatosCalculo
            Public sNombre As String
            Public sAlteracion As String
            Public SerieNatDiaria As SerieDiaria
            Public SerieNatMensual As SerieMensual
            Public SerieAltDiaria As SerieDiaria
            Public SerieAltMensual As SerieMensual
            Public nAnyosCoe As Integer
            Public mesInicio As Integer
        End Structure

        ''' <summary>
        ''' Tabla donde se almacena la tabla de calculos asociados a los percentiles
        ''' </summary>
        ''' <remarks></remarks>
        Structure TablaCQC
            Public pe() As Single
            Public dia() As Single
            Public caudales()() As Single
            Public añomedio() As Single
        End Structure

        Structure TablaFrecuencias
            Public nat() As Single
            Public alt() As Single
            Public minNat() As Single
            Public minAlt() As Single
            Public posMaxNat() As Boolean
            Public posMinNat() As Boolean
            Public posMaxAlt() As Boolean
            Public posMinAlt() As Boolean
        End Structure

        ''' <summary>
        ''' Tabla que representa la tabla que hay que mostrar en estacionalidad
        ''' </summary>
        ''' <remarks></remarks>
        Structure TablaEstacionalidad
            Public ndias() As Single
            Public mes() As String
            Public mediaAño As Single
        End Structure

#Region "Datos, Aportaciones, Series,..."

        ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        ' +++++++++++++++++++++ Variables Privadas de la Clase ++++++++++++++++++++++++++++++++++
        ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        ' Datos de entrada del modulo
        Dim _datos As DatosCalculo
        Dim _usarCoe As Boolean

        ' Series y aportaciones
        Dim _SerieNatAnual As SerieAnual
        Dim _SerieNatMensualCalculada As SerieMensual   ' Si no hay es la interpolacion. Si no es la que nos dan.
        Dim _SerieAltMensualCalculada As SerieMensual
        ' Aportaciones
        Dim _AportacionNatAnual As AportacionAnual
        Dim _AportacionNatAnualOrdAños As AportacionAnual
        Dim _AportacionAltAnual As AportacionAnual
        Dim _AportacionAltAnualOrdAños As AportacionAnual
        Dim _AportacionNatMen As AportacionMensual
        Dim _AportacionAltMen As AportacionMensual

        ' Limites para  InterAnual
        Dim _limHumNat As Single
        Dim _limSecNat As Single
        Dim _limHumAlt As Single
        Dim _limSecAlt As Single
        ' Interanual 
        Dim _graficaInterAlt() As GraficoAlterada
        ' Intranual
        Dim _IntraAnualNat()() As Single
        Dim _IntraAnualAlt()() As Single
#End Region

#Region "Parametros"
        ' Parametros Habituales
        Dim _HabMagnitudNat() As Single
        Dim _HabVariabilidadNat() As Single
        Dim _HabEstacionalidadNat() As String
        Dim _HabMagnitudNatReducido As Single
        Dim _HabVariabilidadNatReducido As Single
        Dim _HabEstacionalidadNatReducido As Single
        Dim _HabVariabilidadDiaraNat() As Single

        Dim _HabMagnitudAnualNat() As Single
        Dim _HabMagnitudMensualNat() As Single
        Dim _HabMagnitudMensualTablaNat() As TablaEstacionalidad
        Dim _HabEstacionalidadMensualNat() As TablaEstacionalidad

        ' Parametros Alterados 
        Dim _HabMagnitudAnualAlt() As Single
        Dim _HabMagnitudMensualAlt() As Single
        Dim _HabMagnitudMensualTablaAlt() As TablaEstacionalidad
        Dim _HabEstacionalidadMensualAlt() As TablaEstacionalidad
        Dim _HabMagnitudAlt() As Single
        Dim _HabVariabilidadAlt() As Single
        Dim _HabEstacionalidadAlt() As String
        Dim _HabVariabilidadDiaraAlt() As Single

        ' Parametros de Avenidas
        Dim _AveMagnitudNat() As Single
        Dim _AveVariabilidadNat() As Single
        Dim _AveEstacionalidadNat As TablaEstacionalidad ' Esto es una tabla de 12 meses
        Dim _AveDuracionNat As Single
        '
        Dim _AveMagnitudAlt() As Single
        Dim _AveVariabilidadAlt() As Single
        Dim _AveEstacionalidadAlt As TablaEstacionalidad ' Esto es una tabla de 12 meses
        Dim _AveDuracionAlt As Single
        ' Extras
        Dim _Ave2TNat As Single
        Dim _Ave2TAlt As Single

        ' Parametros de Sequia
        Dim _SeqMagnitudNat() As Single
        Dim _SeqVariabilidadNat() As Single
        Dim _SeqEstacionalidadNat As TablaEstacionalidad
        Dim _SeqDuracionNat() As Single
        Dim _SeqDuracionCerosMesNat As TablaEstacionalidad
        '
        Dim _SeqMagnitudAlt() As Single
        Dim _SeqVariabilidadAlt() As Single
        Dim _SeqEstacionalidadAlt As TablaEstacionalidad
        Dim _SeqDuracionAlt() As Single
        Dim _SeqDuracionCerosMesAlt As TablaEstacionalidad
#End Region

#Region "Indices"
        ' Indices Habituales
        Dim _IndicesHabituales() As Indices
        Dim _IndicesHabitualesAgregados() As Indices
        Dim _IndiceM3Agregados As Indices
        Dim _IndiceV3Agregados As Indices
        Dim _IndicesAvenidas() As Indices
        Dim _IndicesAvenidasI16Meses() As Single
        Dim _IndicesAvenidasI16MesesInversos() As Single
        Dim _IndicesSequias() As Indices
        Dim _IndicesSequiasI23Meses() As Single
        Dim _IndicesSequiasI23MesesInversos() As Single
        Dim _IndicesSequiasI24Meses() As Single
        Dim _IndicesSequiasI24MesesInversos() As Single

        'Dim _IndiceHabitualI3 As Indices
        Dim _IndiceIAG() As Single
        Dim _IndiceIAG_Agregados As Single
        Dim _IndiceIAG_Ave As Single
        Dim _IndiceIAG_Seq As Single
#End Region

#Region "Regímenes"
        Dim _percentil10() As Single
        Dim _percentil90() As Single
        Dim _medianaMenNat() As Single
        Dim _medianaMenAlt() As Single
        Dim _mesesQueCumplen() As Integer

        Dim _percentil10Anual As Single
        Dim _percentil90Anual As Single
        Dim _medianaAnualNat As Single
        Dim _medianaAnualAlt As Single

        Dim _anyosQueCumplen As Integer
#End Region

#Region "Ambiental"
        Dim _1QMin As Single
        Dim _7QMin As Single
        Dim _15QMin As Single

        Dim _7QRetorno() As Single
        Dim _10QRetorno() As Single

        Dim _mnQ() As Single
#End Region

#Region "Variables Auxiliares"
        ' Datos auxiliares
        Dim _TablaCQCNat As TablaCQC
        Dim _TablaCQCAlt As TablaCQC
        'Dim _TablaEstaNat As TablaEstacionalidad
        Dim _nDiasNulosNat() As Integer
        Dim _nDiasNulosAlt() As Integer
        ' Para calcular el i23 modificado
        'Dim _i23Simplificado As Boolean = False
        ' Para los calculos de indices agregados
        Dim _TablaFrecuenciaMaxMin As TablaFrecuencias

        ' Manejo del Excel
        Dim _rutaExcel As String
        Dim _traductor As MultiLangXML.MultiIdiomasXML
        Dim m_Excel As Microsoft.Office.Interop.Excel.Application
        Dim objWorkbook As Microsoft.Office.Interop.Excel.Workbook
#End Region

#Region "Construccion y destruccion"
        Public Sub New(ByVal datos As DatosCalculo, ByVal usarCoeDiaria As Boolean, ByVal traductor As MultiLangXML.MultiIdiomasXML)
            Me._datos = datos
            Me._usarCoe = usarCoeDiaria
            Me._traductor = traductor
            Me._rutaExcel = Me._traductor.getRutaExcel
        End Sub

        Protected Overrides Sub Finalize()

            'Eliminamos la instancia de Excel de memoria
            Try
                If Not m_Excel Is Nothing Then
                    m_Excel.Quit()
                    m_Excel = Nothing
                End If
            Catch ex As Exception

            End Try
            MyBase.Finalize()
        End Sub
#End Region

#Region "Calculo de las series INTERAnuales"
        ''' <summary>
        ''' Series interanuales
        ''' </summary>
        ''' <remarks>Informe 1</remarks>
        Public Sub CalcularINTERAnual()
            Dim aux As SerieMensual
            Dim i, j As Integer

            Dim años As Integer
            'Dim desp As Integer

            If (Me._datos.SerieNatMensual.caudalMensual Is Nothing) Then
                Me._SerieNatMensualCalculada = CalcularSerieMENSUAL()
                aux = Me._SerieNatMensualCalculada
            Else
                'Me._SerieAltMensualCalculada = Me._datos.SerieNatMensual
                aux = Me._datos.SerieNatMensual
            End If

            CalcularAportacionMENSUAL()

            'años = (aux.caudalMensual.Length / 12)
            años = aux.nAños

            ReDim Me._AportacionNatAnual.aportacion(años - 1)
            ReDim Me._AportacionNatAnual.año(años - 1)
            ReDim Me._AportacionNatAnual.tipo(años - 1)

            ' Sacar la aportacion anual
            For i = 0 To años - 1
                Me._AportacionNatAnual.año(i) = aux.mes(i * 12).Year
                For j = 0 To 11
                    Me._AportacionNatAnual.aportacion(i) = Me._AportacionNatAnual.aportacion(i) + (Me._AportacionNatMen.aportacion((i * 12) + j))
                Next
            Next

            ReDim Me._AportacionNatAnualOrdAños.año(Me._AportacionNatAnual.año.Length - 1)
            ReDim Me._AportacionNatAnualOrdAños.aportacion(Me._AportacionNatAnual.aportacion.Length - 1)
            ReDim Me._AportacionNatAnualOrdAños.tipo(Me._AportacionNatAnual.tipo.Length - 1)

            ' Salvaguarda de las aportaciones por años
            Array.Copy(Me._AportacionNatAnual.aportacion, Me._AportacionNatAnualOrdAños.aportacion, Me._AportacionNatAnual.aportacion.Length)

            ' Ordenar
            Array.Sort(Me._AportacionNatAnual.aportacion, Me._AportacionNatAnual.año)

            ' Clasificar

            Dim pos25p As Single
            Dim pos75p As Single

            pos25p = 0.25 * (años + 1)
            pos75p = 0.75 * (años + 1)

            Dim p(años - 1) As Single

            For i = 0 To años - 1
                p(i) = 1 - (i + 1) / (años + 1)
                If (p(i) <= 0.25) Then
                    Me._AportacionNatAnual.tipo(i) = TIPOAÑO.HUMEDO
                ElseIf (p(i) >= 0.75) Then
                    Me._AportacionNatAnual.tipo(i) = TIPOAÑO.SECO
                Else
                    Me._AportacionNatAnual.tipo(i) = TIPOAÑO.MEDIO
                End If
            Next

            ' Salvaguarda de las aportaciones ordenadas por años.
            Array.Copy(Me._AportacionNatAnual.año, Me._AportacionNatAnualOrdAños.año, Me._AportacionNatAnual.año.Length)
            Array.Copy(Me._AportacionNatAnual.tipo, Me._AportacionNatAnualOrdAños.tipo, Me._AportacionNatAnual.tipo.Length)
            Array.Sort(Me._AportacionNatAnualOrdAños.año, Me._AportacionNatAnualOrdAños.tipo) ' Para ordenar por años

            ' Interpolacion lineal
            Dim v0, v1 As Single
            Dim p0, p1 As Single
            Dim x0, x1 As Integer

            x0 = Int(pos25p - 1)
            x1 = x0 + 1

            v0 = Me._AportacionNatAnual.aportacion(x0)
            v1 = Me._AportacionNatAnual.aportacion(x1)
            p0 = p(x0)
            p1 = p(x1)

            Dim aportacion25p As Single = v0 + (v1 - v0) * ((0.75 - p0) / (p1 - p0))


            x0 = Int(pos75p - 1)
            x1 = x0 + 1

            v0 = Me._AportacionNatAnual.aportacion(x0)
            v1 = Me._AportacionNatAnual.aportacion(x1)
            p0 = p(x0)
            p1 = p(x1)

            Dim aportacion75p As Single = v0 + (v1 - v0) * ((0.25 - p0) / (p1 - p0))


            Me._limHumNat = aportacion75p
            Me._limSecNat = aportacion25p

        End Sub
        ''' <summary>
        ''' Series interanuales alterada
        ''' </summary>
        ''' <remarks>Informe 1b</remarks>
        Public Sub CalcularINTERAnualAlterada()
            Dim aux As SerieMensual
            Dim i, j As Integer

            Dim años As Integer
            'Dim desp As Integer

            If (Me._datos.SerieAltMensual.caudalMensual Is Nothing) Then
                Me._SerieAltMensualCalculada = CalcularSerieMENSUALAlterada()     ' Funcion dependiente
                aux = Me._SerieAltMensualCalculada
            Else
                'Me._SerieAltMensualCalculada = Me._datos.SerieNatMensual
                aux = Me._datos.SerieAltMensual
            End If

            CalcularAportacionMENSUALAlterada()     ' Funcion dependiente

            años = aux.nAños

            ReDim Me._AportacionAltAnual.aportacion(años - 1)
            ReDim Me._AportacionAltAnual.año(años - 1)
            ReDim Me._AportacionAltAnual.tipo(años - 1)

            ' Sacar la aportacion anual
            For i = 0 To años - 1
                Me._AportacionAltAnual.año(i) = aux.mes(i * 12).Year
                For j = 0 To 11
                    Me._AportacionAltAnual.aportacion(i) = Me._AportacionAltAnual.aportacion(i) + (Me._AportacionAltMen.aportacion((i * 12) + j))
                Next
            Next

            ReDim Me._AportacionAltAnualOrdAños.año(Me._AportacionAltAnual.año.Length - 1)
            ReDim Me._AportacionAltAnualOrdAños.aportacion(Me._AportacionAltAnual.aportacion.Length - 1)
            ReDim Me._AportacionAltAnualOrdAños.tipo(Me._AportacionAltAnual.tipo.Length - 1)

            For i = 0 To años - 1
                Me._AportacionAltAnualOrdAños.año(i) = Me._AportacionAltAnual.año(i)
                Me._AportacionAltAnualOrdAños.aportacion(i) = Me._AportacionAltAnual.aportacion(i)
                Me._AportacionAltAnualOrdAños.tipo(i) = Me._AportacionAltAnualOrdAños.tipo(i)
            Next

        End Sub
        ''' <summary>
        ''' Se usa en el caso 3
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CalcularINTERAnualAltGrafica()
            Dim aux As SerieMensual
            Dim i, j As Integer

            Dim años As Integer
            'Dim desp As Integer

            If (Me._datos.SerieAltMensual.caudalMensual Is Nothing) Then
                Me._SerieAltMensualCalculada = CalcularSerieMENSUALAlterada()     ' Funcion dependiente
                aux = Me._SerieAltMensualCalculada
            Else
                'Me._SerieAltMensualCalculada = Me._datos.SerieNatMensual
                aux = Me._datos.SerieAltMensual
            End If

            CalcularAportacionMENSUALAlterada()     ' Funcion dependiente

            años = (aux.caudalMensual.Length / 12)

            ReDim Me._AportacionAltAnual.aportacion(años - 1)
            ReDim Me._AportacionAltAnual.año(años - 1)
            ReDim Me._AportacionAltAnual.tipo(años - 1)

            ' Sacar la aportacion anual
            For i = 0 To años - 1
                Me._AportacionAltAnual.año(i) = aux.mes(i * 12).Year
                For j = 0 To 11
                    Me._AportacionAltAnual.aportacion(i) = Me._AportacionAltAnual.aportacion(i) + (Me._AportacionAltMen.aportacion((i * 12) + j))
                Next
            Next

            ReDim Me._AportacionAltAnualOrdAños.año(Me._AportacionAltAnual.año.Length - 1)
            ReDim Me._AportacionAltAnualOrdAños.aportacion(Me._AportacionAltAnual.aportacion.Length - 1)
            ReDim Me._AportacionAltAnualOrdAños.tipo(Me._AportacionAltAnual.tipo.Length - 1)

            ' Salvagurada de las aportaciones ordenadas por año
            Array.Copy(Me._AportacionAltAnual.aportacion, Me._AportacionAltAnualOrdAños.aportacion, Me._AportacionAltAnual.aportacion.Length)

            ' Ordenar
            Array.Sort(Me._AportacionAltAnual.aportacion, Me._AportacionAltAnual.año)

            ' Clasificar

            Dim pos25p As Single
            Dim pos75p As Single

            pos25p = 0.25 * (años + 1)
            pos75p = 0.75 * (años + 1)

            Dim p(años - 1) As Single

            For i = 0 To años - 1
                p(i) = 1 - (i + 1) / (años + 1)
                If (p(i) <= 0.25) Then
                    Me._AportacionAltAnual.tipo(i) = TIPOAÑO.HUMEDO
                ElseIf (p(i) >= 0.75) Then
                    Me._AportacionAltAnual.tipo(i) = TIPOAÑO.SECO
                Else
                    Me._AportacionAltAnual.tipo(i) = TIPOAÑO.MEDIO
                End If
            Next

            ' Salvaguarda de las aportaciones ordenadas por años.
            Array.Copy(Me._AportacionAltAnual.año, Me._AportacionAltAnualOrdAños.año, Me._AportacionAltAnual.año.Length)
            'Array.Copy(Me._AportacionAltAnual.aportacion, Me._AportacionAltAnualOrdAños.aportacion, Me._AportacionAltAnual.aportacion.Length)
            Array.Copy(Me._AportacionAltAnual.tipo, Me._AportacionAltAnualOrdAños.tipo, Me._AportacionAltAnual.tipo.Length)
            Array.Sort(Me._AportacionAltAnualOrdAños.año, Me._AportacionAltAnualOrdAños.tipo)

            ReDim Me._graficaInterAlt(Me._AportacionAltAnualOrdAños.año.Length)

            For i = 0 To Me._AportacionAltAnualOrdAños.año.Length - 1
                Me._graficaInterAlt(i).año = Me._AportacionAltAnualOrdAños.año(i)
                Me._graficaInterAlt(i).tipo = Me._AportacionAltAnualOrdAños.tipo(i)
                Me._graficaInterAlt(i).apAlt = Me._AportacionAltAnualOrdAños.aportacion(i)
                Me._graficaInterAlt(i).apNat = Me._AportacionNatAnualOrdAños.aportacion(i)
                Me._graficaInterAlt(i).porcentaje = (Me._graficaInterAlt(i).apAlt / Me._graficaInterAlt(i).apNat) * 100
            Next


            For i = 0 To años - 1
                Me._AportacionAltAnual.tipo(i) = Me._AportacionNatAnualOrdAños.tipo(Array.BinarySearch(Me._AportacionNatAnualOrdAños.año, Me._AportacionAltAnual.año(i)))
            Next


            ' Interpolacion lineal
            'Dim v0, v1 As Single
            'Dim p0, p1 As Single
            'Dim x0, x1 As Integer

            'x0 = Int(pos25p)
            'x1 = x0 + 1

            'v0 = Me._AportacionAltAnual.aportacion(x0)
            'v1 = Me._AportacionAltAnual.aportacion(x1)
            'p0 = p(x0 - 1)
            'p1 = p(x1 - 1)

            'Dim aportacion25p As Single = v0 + (v1 - v0) * ((0.75 - p0) / (p1 - p0))


            'x0 = Int(pos75p)
            'x1 = x0 + 1

            'v0 = Me._AportacionAltAnual.aportacion(x0)
            'v1 = Me._AportacionAltAnual.aportacion(x1)
            'p0 = p(x0 - 1)
            'p1 = p(x1 - 1)

            'Dim aportacion75p As Single = v0 + (v1 - v0) * ((0.25 - p0) / (p1 - p0))

        End Sub
#End Region

#Region "Calculo de las series INTRAnual"
        ''' <summary>
        ''' Series INTRAnual
        ''' </summary>
        ''' <param name="alterada">Si usamos los datos alterados</param>
        ''' <remarks>Informe 2 y 3</remarks>
        Public Sub calcularINTRAnual(ByVal alterada As Boolean)

            Dim i, j As Integer
            Dim longLista As Integer

            ' Valores divididos en su tipo
            Dim valoresMenH(11)() As Single
            Dim valoresMenM(11)() As Single
            Dim valoresMenS(11)() As Single

            For i = 0 To 11
                ReDim valoresMenH(i)(0)
                ReDim valoresMenM(i)(0)
                ReDim valoresMenS(i)(0)
            Next

            ' ------------------------------------------------------------------------------------------
            ' -------- Meter los datos segun natural/alterada ------------------------------------------
            ' ------------------------------------------------------------------------------------------
            ' Meter los valores por tipo de año.
            If (Not alterada) Then
                longLista = Me._AportacionNatAnual.año.Length
                ' Calcular las medianas de cada año.
                ' Tengo que recorrer los 12 meses de cada año y meterlo en su sitio
                For i = 0 To Me._AportacionNatAnualOrdAños.año.Length - 1
                    For j = 0 To 11
                        If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                            valoresMenH(j)(valoresMenH(j).Length - 1) = Me._AportacionNatMen.aportacion((i * 12) + j)
                            ReDim Preserve valoresMenH(j)(valoresMenH(j).Length)
                        ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                            valoresMenM(j)(valoresMenM(j).Length - 1) = Me._AportacionNatMen.aportacion((i * 12) + j)
                            ReDim Preserve valoresMenM(j)(valoresMenM(j).Length)
                        Else
                            valoresMenS(j)(valoresMenS(j).Length - 1) = Me._AportacionNatMen.aportacion((i * 12) + j)
                            ReDim Preserve valoresMenS(j)(valoresMenS(j).Length)
                        End If

                    Next
                Next
            Else
                ' -------------------------------------------
                ' Segun el caso 3:
                ' SE USAN LOS TIPO CALCULADOS EN REG. NATURAL
                ' -------------------------------------------
                longLista = Me._AportacionAltAnual.año.Length
                ' Calcular las medianas de cada año.
                ' Tengo que recorrer los 12 meses de cada año y meterlo en su sitio
                For i = 0 To Me._AportacionAltAnualOrdAños.año.Length - 1
                    For j = 0 To 11
                        If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                            valoresMenH(j)(valoresMenH(j).Length - 1) = Me._AportacionAltMen.aportacion((i * 12) + j)
                            ReDim Preserve valoresMenH(j)(valoresMenH(j).Length)
                        ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                            valoresMenM(j)(valoresMenM(j).Length - 1) = Me._AportacionAltMen.aportacion((i * 12) + j)
                            ReDim Preserve valoresMenM(j)(valoresMenM(j).Length)
                        Else
                            valoresMenS(j)(valoresMenS(j).Length - 1) = Me._AportacionAltMen.aportacion((i * 12) + j)
                            ReDim Preserve valoresMenS(j)(valoresMenS(j).Length)
                        End If

                    Next
                Next

            End If


            ' Ñapa por reservar memoria de mas... pero funciona.
            For i = 0 To 11
                ReDim Preserve valoresMenH(i)(valoresMenH(i).Length - 2)
                ReDim Preserve valoresMenM(i)(valoresMenM(i).Length - 2)
                ReDim Preserve valoresMenS(i)(valoresMenS(i).Length - 2)
            Next

            ' Ordenar cada mes
            For i = 0 To 11
                Array.Sort(valoresMenH(i))
                Array.Sort(valoresMenM(i))
                Array.Sort(valoresMenS(i))
            Next

            ' Test K-S
            ' --------
            ' Tengo que ordenar todas las aportaciones de un mes. Luego guardar el ordinal, y 
            ' sumar nº de orden
            Dim dKS As DatosKS

            ' Genero unas estructuras de aportaciones de cada mes a lo largo de los años, y con
            ' el tipo que es el año asociado.
            ReDim dKS.datosMes(11)
            ReDim dKS.tipoMes(11)
            ReDim dKS.tipoMesKS(11)

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++++++++++++++++ PASO 1 +++++++++++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            For i = 0 To 11
                'ReDim Preserve dKS.datosMes(i)(Me._AportacionNatAnual.año.Length - 1)
                'ReDim Preserve dKS.tipoMes(i)(Me._AportacionNatAnual.año.Length - 1)
                'ReDim Preserve dKS.tipoMesKS(i)(Me._AportacionNatAnual.año.Length - 1)
                ReDim Preserve dKS.datosMes(i)(longLista - 1)
                ReDim Preserve dKS.tipoMes(i)(longLista - 1)
                ReDim Preserve dKS.tipoMesKS(i)(longLista - 1)

                Dim pos As Integer = 0

                For j = 0 To valoresMenH(i).Length - 1
                    dKS.datosMes(i)(pos) = valoresMenH(i)(j)
                    dKS.tipoMes(i)(pos) = TIPOAÑO.HUMEDO

                    pos = pos + 1
                Next

                For j = 0 To valoresMenM(i).Length - 1
                    dKS.datosMes(i)(pos) = valoresMenM(i)(j)
                    dKS.tipoMes(i)(pos) = TIPOAÑO.MEDIO

                    pos = pos + 1
                Next

                For j = 0 To valoresMenS(i).Length - 1
                    dKS.datosMes(i)(pos) = valoresMenS(i)(j)
                    dKS.tipoMes(i)(pos) = TIPOAÑO.SECO

                    pos = pos + 1
                Next

                ' Ordeno segun la cantidad de aportacion mensual
                ' el primero el mas pequeño
                Array.Sort(dKS.datosMes(i), dKS.tipoMes(i))

            Next


            ' +++++++++++++++++++++++++++++++++++++
            ' tabla de medianas y su inicializacion
            ' +++++++++++++++++++++++++++++++++++++
            ' 0 -> medianas de meses Humedos
            ' 1 -> medianas de meses Medios
            ' 2 -> medianas de meses Secos
            Dim tablamedianas(11)() As Single
            For i = 0 To 11
                ReDim tablamedianas(i)(2)
            Next

            ' Calculo del estadistico Q
            Dim RH, RM, RS As Single

            ' Recalcular las listas para los nuevos tipos segun KS
            Dim valorH()(), valorM()(), valorS()() As Single
            ReDim valorH(11)
            ReDim valorM(11)
            ReDim valorS(11)

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++++++ PASO 2 +++++++++++++++++++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++++++ Calculamos Rh, Rm y Rs ++++++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            '
            For i = 0 To 11
                ' Vacio las variables
                RH = 0
                RM = 0
                RS = 0

                ' Calculo RH, RM, RS para le estadistico Q
                For j = 0 To dKS.datosMes(i).Length - 1
                    If (dKS.tipoMes(i)(j) = TIPOAÑO.HUMEDO) Then
                        RH = RH + (j + 1)
                    ElseIf (dKS.tipoMes(i)(j) = TIPOAÑO.MEDIO) Then
                        RM = RM + (j + 1)
                    Else
                        RS = RS + (j + 1)
                    End If
                Next

                ' Calcular estadistico
                'Dim N As Integer = Me._AportacionNatAnual.año.Length
                Dim N As Integer = longLista
                Dim Q As Single = 12 / (N * (N + 1)) * ((RH * RH) / valoresMenH(i).Length + (RM * RM) / valoresMenM(i).Length + (RS * RS) / valoresMenS(i).Length) - 3 * (N + 1)

                ' +++++++++++++++++++++++++++++++++
                ' ++++++++ Hipotesis H0 +++++++++++
                ' +++++++++++++++++++++++++++++++++
                ' Hipotesis H0 -> medianas de H,M,S tienen una medianas sin cambios significativos
                If (Q < 5.991) Then ' ACEPTAMOS LA HIPOTESIS 0
                    ' Calcular las medianas segun la ordenacion de dKS

                    ' Obtener el tipo de Mes segun el analisis de KS
                    Dim l25p = 0.25 * dKS.datosMes(i).Length
                    Dim l75p = 0.75 * dKS.datosMes(i).Length
                    'For i = 0 To 11
                    Dim PE As Single
                    For j = 0 To dKS.datosMes(i).Length - 1
                        PE = 1 - (j + 1) / (dKS.datosMes(i).Length + 1)
                        If (PE <= 0.25) Then
                            dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO
                        ElseIf (PE >= 0.75) Then
                            dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO
                        Else
                            dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO
                        End If
                    Next

                    'For j = 0 To Int(l25p) - 1
                    '    dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO
                    'Next
                    'For j = Int(l25p) To Int(l75p) - 1
                    '    dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO
                    'Next
                    'For j = Int(l75p) To dKS.datosMes(i).Length - 1
                    '    dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO
                    'Next
                    'Next

                    'For i = 0 To 11
                    For j = 0 To dKS.datosMes(i).Length - 1
                        If (dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO) Then
                            If (valorH(i) Is Nothing) Then
                                ReDim valorH(i)(0)
                            Else
                                ReDim Preserve valorH(i)(valorH(i).Length)
                            End If

                            valorH(i)(valorH(i).Length - 1) = dKS.datosMes(i)(j)

                        ElseIf (dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO) Then
                            If (valorM(i) Is Nothing) Then
                                ReDim valorM(i)(0)
                            Else
                                ReDim Preserve valorM(i)(valorM(i).Length)
                            End If

                            valorM(i)(valorM(i).Length - 1) = dKS.datosMes(i)(j)

                        ElseIf (dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO) Then
                            If (valorS(i) Is Nothing) Then
                                ReDim valorS(i)(0)
                            Else
                                ReDim Preserve valorS(i)(valorS(i).Length)
                            End If

                            valorS(i)(valorS(i).Length - 1) = dKS.datosMes(i)(j)

                        End If
                    Next
                    'Next

                    ' Sacar las medianas
                    'For i = 0 To 11
                    Array.Sort(valorH(i))
                    Array.Sort(valorM(i))
                    Array.Sort(valorS(i))

                    ' -------------------
                    ' Calculo de medianas
                    ' -------------------
                    Dim medianaH, medianaM, medianaS As Single
                    ' Mediana de Humedo
                    If ((valorH(i).Length Mod 2) <> 0) Then
                        medianaH = valorH(i)((valorH(i).Length - 1) / 2)
                    Else
                        Dim p0 As Integer
                        Dim p1 As Integer
                        p0 = ((valorH(i).Length) / 2) - 1
                        p1 = p0 + 1
                        medianaH = (valorH(i)(p0) + valorH(i)(p1)) / 2
                    End If
                    ' Mediana de Medio
                    If ((valorM(i).Length Mod 2) <> 0) Then
                        medianaM = valorM(i)((valorM(i).Length - 1) / 2)
                    Else
                        Dim p0 As Integer
                        Dim p1 As Integer
                        p0 = ((valorM(i).Length) / 2) - 1
                        p1 = p0 + 1
                        medianaM = (valorM(i)(p0) + valorM(i)(p1)) / 2
                    End If
                    ' Mediana de Seco
                    If ((valorS(i).Length Mod 2) <> 0) Then
                        medianaS = valorS(i)((valorS(i).Length - 1) / 2)
                    Else
                        Dim p0 As Integer
                        Dim p1 As Integer
                        p0 = ((valorS(i).Length) / 2) - 1
                        p1 = p0 + 1
                        medianaS = (valorS(i)(p0) + valorS(i)(p1)) / 2
                    End If

                    tablamedianas(i)(0) = medianaH
                    tablamedianas(i)(1) = medianaM
                    tablamedianas(i)(2) = medianaS
                    'Next
                    ' Tablas medianas son el resultado!!!!

                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ' +++++++++++++ Hipotesis H02 y H03 ++++++++++++++++++++++++++++++
                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                Else
                    ' Hipotesis H02 y H03
                    ' Ahora hay dos alternativas que calcular:
                    '   - Agrupar H-M
                    '   - Agrupar M-S
                    Dim N_HyM, N_MyS As Single
                    Dim H02, H03 As Boolean

                    ' Hay que recalcular los indices ya que ahora no se tiene en cuenta la serie H en un caso
                    ' y S en otro

                    ' Valores de la hipotesis.
                    H02 = False
                    H03 = False

                    ' Recalcular N_HyM y N_MyS
                    ' ------------------------
                    Dim RH02, RM02, RM03, RS03 As Single

                    RH02 = 0
                    RM02 = 0
                    RM03 = 0
                    RS03 = 0

                    ' Calcular RH02, RM02
                    Dim ordinal As Integer = 0
                    For j = 0 To dKS.datosMes(i).Length - 1
                        If (dKS.tipoMes(i)(j) = TIPOAÑO.HUMEDO) Then
                            RH02 = RH02 + (ordinal + 1)
                            ordinal = ordinal + 1
                        ElseIf (dKS.tipoMes(i)(j) = TIPOAÑO.MEDIO) Then
                            RM02 = RM02 + (ordinal + 1)
                            ordinal = ordinal + 1
                        End If
                    Next

                    N_HyM = ordinal

                    ' Calcular RM03, RS03
                    ordinal = 0
                    For j = 0 To dKS.datosMes(i).Length - 1
                        If (dKS.tipoMes(i)(j) = TIPOAÑO.MEDIO) Then
                            RM03 = RM03 + (ordinal + 1)
                            ordinal = ordinal + 1
                        ElseIf (dKS.tipoMes(i)(j) = TIPOAÑO.SECO) Then
                            RS03 = RS03 + (ordinal + 1)
                            ordinal = ordinal + 1
                        End If
                    Next

                    N_MyS = ordinal

                    'N_HyM = RH02 + RM02
                    'N_MyS = RM03 + RS03

                    Dim Q_HyM As Single = 12 / (N_HyM * (N_HyM + 1)) * ((RH02 * RH02) / valoresMenH(i).Length + (RM02 * RM02) / valoresMenM(i).Length) - 3 * (N_HyM + 1)
                    Dim Q_MyS As Single = 12 / (N_MyS * (N_MyS + 1)) * ((RM03 * RM03) / valoresMenM(i).Length + (RS03 * RS03) / valoresMenS(i).Length) - 3 * (N_MyS + 1)

                    If (Q_HyM < 3.841) Then
                        H02 = True
                    End If
                    If (Q_MyS < 3.841) Then
                        H03 = True
                    End If

                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ' +++++++++++ Analisis de los estadisticos +++++++++++++++++++++++
                    ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    If (H02 And H03) Then
                        ' Igual que el la otra rama del IF (copiar o hacer una funcion)
                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        Dim PE As Single
                        For j = 0 To dKS.datosMes(i).Length - 1
                            PE = 1 - (j + 1) / (dKS.datosMes(i).Length + 1)
                            If (PE <= 0.25) Then
                                dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO
                            ElseIf (PE >= 0.75) Then
                                dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO
                            Else
                                dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO
                            End If
                        Next
                        For j = 0 To dKS.datosMes(i).Length - 1
                            If (dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO) Then
                                If (valorH(i) Is Nothing) Then
                                    ReDim valorH(i)(0)
                                Else
                                    ReDim Preserve valorH(i)(valorH(i).Length)
                                End If

                                valorH(i)(valorH(i).Length - 1) = dKS.datosMes(i)(j)

                            ElseIf (dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO) Then
                                If (valorM(i) Is Nothing) Then
                                    ReDim valorM(i)(0)
                                Else
                                    ReDim Preserve valorM(i)(valorM(i).Length)
                                End If

                                valorM(i)(valorM(i).Length - 1) = dKS.datosMes(i)(j)

                            ElseIf (dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO) Then
                                If (valorS(i) Is Nothing) Then
                                    ReDim valorS(i)(0)
                                Else
                                    ReDim Preserve valorS(i)(valorS(i).Length)
                                End If

                                valorS(i)(valorS(i).Length - 1) = dKS.datosMes(i)(j)

                            End If
                        Next
                        Array.Sort(valorH(i))
                        Array.Sort(valorM(i))
                        Array.Sort(valorS(i))

                        Dim medianaH, medianaM, medianaS As Single
                        ' Mediana de Humedo
                        If ((valorH(i).Length Mod 2) <> 0) Then
                            medianaH = valorH(i)((valorH(i).Length - 1) / 2)
                        Else
                            Dim p0 As Integer
                            Dim p1 As Integer
                            p0 = ((valorH(i).Length) / 2) - 1
                            p1 = p0 + 1
                            medianaH = (valorH(i)(p0) + valorH(i)(p1)) / 2
                        End If
                        ' Mediana de Medio
                        If ((valorM(i).Length Mod 2) <> 0) Then
                            medianaM = valorM(i)((valorM(i).Length - 1) / 2)
                        Else
                            Dim p0 As Integer
                            Dim p1 As Integer
                            p0 = ((valorM(i).Length) / 2) - 1
                            p1 = p0 + 1
                            medianaM = (valorM(i)(p0) + valorM(i)(p1)) / 2
                        End If
                        ' Mediana de Seco
                        If ((valorS(i).Length Mod 2) <> 0) Then
                            medianaS = valorS(i)((valorS(i).Length - 1) / 2)
                        Else
                            Dim p0 As Integer
                            Dim p1 As Integer
                            p0 = ((valorS(i).Length) / 2) - 1
                            p1 = p0 + 1
                            medianaS = (valorS(i)(p0) + valorS(i)(p1)) / 2
                        End If

                        tablamedianas(i)(0) = medianaH
                        tablamedianas(i)(1) = medianaM
                        tablamedianas(i)(2) = medianaS

                        ' --------------------------------------
                        ' --- 
                        ' --------------------------------------
                    ElseIf (Not (H02) And Not (H03)) Then
                        'For i = 0 To 11
                        'ReDim tablamedianas(i)(2)

                        If ((valoresMenH(i).Length Mod 2) = 0) Then ' Es par
                            Dim p0 As Integer = Int(valoresMenH(i).Length / 2) - 1
                            Dim p1 As Integer = p0 + 1 'Int(valoresMenH(i).Length / 2)
                            tablamedianas(i)(0) = (valoresMenH(i)(p0) + valoresMenH(i)(p1)) / 2
                        Else
                            tablamedianas(i)(0) = valoresMenH(i)((valoresMenH(i).Length - 1) / 2)
                        End If

                        If ((valoresMenM(i).Length Mod 2) = 0) Then ' Es par
                            Dim p0 As Integer = Int(valoresMenM(i).Length / 2) - 1
                            Dim p1 As Integer = p0 + 1 'Int(valoresMenM(i).Length / 2) + 1
                            tablamedianas(i)(1) = (valoresMenM(i)(p0) + valoresMenM(i)(p1)) / 2
                        Else
                            tablamedianas(i)(1) = valoresMenM(i)((valoresMenM(i).Length - 1) / 2)
                        End If

                        If ((valoresMenS(i).Length Mod 2) = 0) Then ' Es par
                            Dim p0 As Integer = Int(valoresMenS(i).Length / 2) - 1
                            Dim p1 As Integer = p0 + 1 'Int(valoresMenS(i).Length / 2) + 1
                            tablamedianas(i)(2) = (valoresMenS(i)(p0) + valoresMenS(i)(p1)) / 2
                        Else
                            tablamedianas(i)(2) = valoresMenS(i)((valoresMenS(i).Length - 1) / 2)
                        End If

                        'Next

                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        ' +++++++++++ H02 es FALSO y H03 es VERDADERO ++++++++++++++++++++
                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ElseIf (Not (H02) And H03) Then
                        ' Agrupar M y S, calcular medianas de grupos
                        'For i = 0 To 11
                        ' Ahora solo tendran esta longitud ya que H no se contempla
                        ReDim Preserve dKS.datosMes(i)(valoresMenS(i).Length + valoresMenM(i).Length - 1)
                        ReDim Preserve dKS.tipoMes(i)(valoresMenS(i).Length + valoresMenM(i).Length - 1)
                        ReDim Preserve dKS.tipoMesKS(i)(valoresMenS(i).Length + valoresMenM(i).Length - 1)

                        Dim pos As Integer = 0

                        'For j = 0 To valoresMenH(i).Length - 1
                        '    dKS.datosMes(i)(pos) = valoresMenH(i)(j)
                        '    dKS.tipoMes(i)(pos) = TIPOAÑO.HUMEDO
                        '    pos = pos + 1
                        'Next

                        For j = 0 To valoresMenM(i).Length - 1
                            dKS.datosMes(i)(pos) = valoresMenM(i)(j)
                            dKS.tipoMes(i)(pos) = TIPOAÑO.MEDIO

                            pos = pos + 1
                        Next

                        For j = 0 To valoresMenS(i).Length - 1
                            dKS.datosMes(i)(pos) = valoresMenS(i)(j)
                            dKS.tipoMes(i)(pos) = TIPOAÑO.SECO

                            pos = pos + 1
                        Next
                        ' Ordeno segun la cantidad de aportacion mensual
                        ' el primero el mas pequeño
                        Array.Sort(dKS.datosMes(i), dKS.tipoMes(i))
                        'Next

                        'For i = 0 To 11
                        Dim PE As Single
                        For j = 0 To dKS.datosMes(i).Length - 1
                            PE = 1 - (j + 1) / (dKS.datosMes(i).Length + 1)
                            If (PE < 0.66) Then
                                dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO
                            ElseIf (PE >= 0.66) Then
                                dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO
                            End If
                        Next

                        ' Mediana de las H
                        ' Saco directamente la mediana de H ya que no se tiene
                        ' En cuenta. Es una forma de abreviar
                        If ((valoresMenH(i).Length Mod 2) = 0) Then ' Es par
                            Dim p0 As Integer = Int(valoresMenH(i).Length / 2) - 1
                            Dim p1 As Integer = p0 + 1 'Int(valoresMenH(i).Length / 2)
                            tablamedianas(i)(0) = (valoresMenH(i)(p0) + valoresMenH(i)(p1)) / 2
                        Else
                            tablamedianas(i)(0) = valoresMenH(i)((valoresMenH(i).Length - 1) / 2)
                        End If
                        'Next

                        ' Calcular mediana de los nuevos tipos sacados de KS
                        'Dim valorM()(), valorS()() As Single
                        'ReDim valorH(11)
                        'ReDim valorM(11)
                        'ReDim valorS(11)

                        'For i = 0 To 11
                        For j = 0 To dKS.datosMes(i).Length - 1
                            'If (dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO) Then
                            '    If (valorH(i) Is Nothing) Then
                            '        ReDim valorH(i)(0)
                            '    Else
                            '        ReDim Preserve valorH(i)(valorH(i).Length)
                            '    End If

                            '    valorH(i)(valorH(i).Length - 1) = dKS.datosMes(i)(j)
                            'Else
                            If (dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO) Then
                                If (valorM(i) Is Nothing) Then
                                    ReDim valorM(i)(0)
                                Else
                                    ReDim Preserve valorM(i)(valorM(i).Length)
                                End If

                                valorM(i)(valorM(i).Length - 1) = dKS.datosMes(i)(j)

                            ElseIf (dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO) Then
                                If (valorS(i) Is Nothing) Then
                                    ReDim valorS(i)(0)
                                Else
                                    ReDim Preserve valorS(i)(valorS(i).Length)
                                End If

                                valorS(i)(valorS(i).Length - 1) = dKS.datosMes(i)(j)

                            End If
                        Next
                        'Next ' For de 0 a 11

                        'For i = 0 To 11
                        'Array.Sort(valorH(i))
                        Array.Sort(valorM(i))
                        Array.Sort(valorS(i))

                        Dim medianaM, medianaS As Single

                        ' Mediana de Humedo
                        'If ((valorH(i).Length Mod 2) <> 0) Then
                        '    medianaH = valorH(i)((valorH(i).Length - 1) / 2)
                        'Else
                        '    Dim p0 As Integer
                        '    Dim p1 As Integer
                        '    p0 = ((valorH(i).Length) / 2) - 1
                        '    p1 = p0 + 1
                        '    medianaH = (valorH(i)(p0) + valorH(i)(p1)) / 2
                        'End If
                        '' Mediana de Medio
                        If ((valorM(i).Length Mod 2) <> 0) Then
                            medianaM = valorM(i)((valorM(i).Length - 1) / 2)
                        Else
                            Dim p0 As Integer
                            Dim p1 As Integer
                            p0 = ((valorM(i).Length) / 2) - 1
                            p1 = p0 + 1
                            medianaM = (valorM(i)(p0) + valorM(i)(p1)) / 2
                        End If
                        ' Mediana de Seco
                        If ((valorS(i).Length Mod 2) <> 0) Then
                            medianaS = valorS(i)((valorS(i).Length - 1) / 2)
                        Else
                            Dim p0 As Integer
                            Dim p1 As Integer
                            p0 = ((valorS(i).Length) / 2) - 1
                            p1 = p0 + 1
                            medianaS = (valorS(i)(p0) + valorS(i)(p1)) / 2
                        End If

                        'tablamedianas(i)(0) = medianaH
                        tablamedianas(i)(1) = medianaM
                        tablamedianas(i)(2) = medianaS

                        ' Next

                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        ' +++++++++++ H02 es VERDADERO y H03 es FALSO ++++++++++++++++++++
                        ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Else
                        ' Agrupar M y S, calcular medianas de grupos
                        'For i = 0 To 11
                        ' Ahora solo tendran esta longitud ya que H no se contempla
                        ReDim Preserve dKS.datosMes(i)(valoresMenS(i).Length + valoresMenM(i).Length - 1)
                        ReDim Preserve dKS.tipoMes(i)(valoresMenS(i).Length + valoresMenM(i).Length - 1)
                        ReDim Preserve dKS.tipoMesKS(i)(valoresMenS(i).Length + valoresMenM(i).Length - 1)

                        Dim pos As Integer = 0

                        For j = 0 To valoresMenH(i).Length - 1
                            dKS.datosMes(i)(pos) = valoresMenH(i)(j)
                            dKS.tipoMes(i)(pos) = TIPOAÑO.HUMEDO
                            pos = pos + 1
                        Next

                        For j = 0 To valoresMenM(i).Length - 1
                            dKS.datosMes(i)(pos) = valoresMenM(i)(j)
                            dKS.tipoMes(i)(pos) = TIPOAÑO.MEDIO

                            pos = pos + 1
                        Next

                        'For j = 0 To valoresMenS(i).Length - 1
                        '    dKS.datosMes(i)(pos) = valoresMenS(i)(j)
                        '    dKS.tipoMes(i)(pos) = TIPOAÑO.SECO

                        '    pos = pos + 1
                        'Next
                        ' Ordeno segun la cantidad de aportacion mensual
                        ' el primero el mas pequeño
                        Array.Sort(dKS.datosMes(i), dKS.tipoMes(i))
                        'Next

                        'For i = 0 To 11
                        Dim PE As Single
                        For j = 0 To dKS.datosMes(i).Length - 1
                            PE = 1 - (j + 1) / (dKS.datosMes(i).Length + 1)
                            If (PE <= 0.33) Then
                                dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO
                            ElseIf (PE > 0.33) Then
                                dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO
                            End If
                        Next

                        ' Mediana de las H
                        ' Saco directamente la mediana de H ya que no se tiene
                        ' En cuenta. Es una forma de abreviar
                        If ((valoresMenS(i).Length Mod 2) = 0) Then ' Es par
                            Dim p0 As Integer = Int(valoresMenS(i).Length / 2) - 1
                            Dim p1 As Integer = p0 + 1 'Int(valoresMenS(i).Length / 2) + 1
                            tablamedianas(i)(2) = (valoresMenS(i)(p0) + valoresMenS(i)(p1)) / 2
                        Else
                            tablamedianas(i)(2) = valoresMenS(i)(valoresMenS(i).Length / 2)
                        End If
                        'Next

                        ' Calcular mediana de los nuevos tipos sacados de KS
                        'Dim valorM()(), valorS()() As Single
                        'ReDim valorH(11)
                        'ReDim valorM(11)
                        'ReDim valorS(11)

                        'For i = 0 To 11
                        For j = 0 To dKS.datosMes(i).Length - 1
                            If (dKS.tipoMesKS(i)(j) = TIPOAÑO.HUMEDO) Then
                                If (valorH(i) Is Nothing) Then
                                    ReDim valorH(i)(0)
                                Else
                                    ReDim Preserve valorH(i)(valorH(i).Length)
                                End If

                                valorH(i)(valorH(i).Length - 1) = dKS.datosMes(i)(j)
                            ElseIf (dKS.tipoMesKS(i)(j) = TIPOAÑO.MEDIO) Then
                                If (valorM(i) Is Nothing) Then
                                    ReDim valorM(i)(0)
                                Else
                                    ReDim Preserve valorM(i)(valorM(i).Length)
                                End If

                                valorM(i)(valorM(i).Length - 1) = dKS.datosMes(i)(j)

                                'ElseIf (dKS.tipoMesKS(i)(j) = TIPOAÑO.SECO) Then
                                '    If (valorS(i) Is Nothing) Then
                                '        ReDim valorS(i)(0)
                                '    Else
                                '        ReDim Preserve valorS(i)(valorS(i).Length)
                                '    End If

                                '    valorS(i)(valorS(i).Length - 1) = dKS.datosMes(i)(j)

                            End If
                        Next
                        'Next ' For de 0 a 11

                        'For i = 0 To 11
                        Array.Sort(valorH(i))
                        Array.Sort(valorM(i))
                        'Array.Sort(valorS(i))

                        Dim medianaH, medianaM As Single

                        ' Mediana de Humedo
                        If ((valorH(i).Length Mod 2) <> 0) Then
                            medianaH = valorH(i)((valorH(i).Length - 1) / 2)
                        Else
                            Dim p0 As Integer
                            Dim p1 As Integer
                            p0 = ((valorH(i).Length) / 2) - 1
                            p1 = p0 + 1
                            medianaH = (valorH(i)(p0) + valorH(i)(p1)) / 2
                        End If
                        '' Mediana de Medio
                        If ((valorM(i).Length Mod 2) <> 0) Then
                            medianaM = valorM(i)((valorM(i).Length - 1) / 2)
                        Else
                            Dim p0 As Integer
                            Dim p1 As Integer
                            p0 = ((valorM(i).Length) / 2) - 1
                            p1 = p0 + 1
                            medianaM = (valorM(i)(p0) + valorM(i)(p1)) / 2
                        End If
                        ' Mediana de Seco
                        'If ((valorS(i).Length Mod 2) <> 0) Then
                        '    medianaS = valorS(i)((valorS(i).Length - 1) / 2)
                        'Else
                        '    Dim p0 As Integer
                        '    Dim p1 As Integer
                        '    p0 = ((valorS(i).Length) / 2) - 1
                        '    p1 = p0 + 1
                        '    medianaS = (valorS(i)(p0) + valorS(i)(p1)) / 2
                        'End If

                        tablamedianas(i)(0) = medianaH
                        tablamedianas(i)(1) = medianaM
                        'tablamedianas(i)(2) = medianaS

                        ' Next

                    End If

                End If

            Next ' Fin de bucle de meses

            ' Asignacion en las variables que necesito segun el calculo que realizo.
            If (Not alterada) Then
                Me._IntraAnualNat = tablamedianas
            Else
                Me._IntraAnualAlt = tablamedianas
            End If
        End Sub
#End Region

#Region "Calculo de parametros"
        ''' <summary>
        ''' Parametro Habituales de regimen alterado
        ''' </summary>
        ''' <remarks>Informe 5</remarks>
        Public Sub CalcularParametrosHabitualesAlterados()

            Dim i, j As Integer

            ' +++++++++++++++++++++++++++++++++++++++
            ' ++++++++++ Calculo de la Magnitud +++++
            ' +++++++++++++++++++++++++++++++++++++++
            ReDim Me._HabMagnitudAlt(3) ' Redimension de las variables donde almacenaré los resultados
            Dim nAñoH, nAñoM, nAñoS As Integer

            nAñoH = 0
            nAñoM = 0
            nAñoS = 0

            ' Hay que usar los tipos de los años Naturales
            For i = 0 To Me._AportacionAltAnual.año.Length - 1
                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    Me._HabMagnitudAlt(0) = Me._HabMagnitudAlt(0) + Me._AportacionAltAnual.aportacion(i)
                    nAñoH = nAñoH + 1
                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    Me._HabMagnitudAlt(1) = Me._HabMagnitudAlt(1) + Me._AportacionAltAnual.aportacion(i)
                    nAñoM = nAñoM + 1
                Else
                    Me._HabMagnitudAlt(2) = Me._HabMagnitudAlt(2) + Me._AportacionAltAnual.aportacion(i)
                    nAñoS = nAñoS + 1
                End If
            Next

            Me._HabMagnitudAlt(3) = (Me._HabMagnitudAlt(0) + Me._HabMagnitudAlt(1) + Me._HabMagnitudAlt(2)) / (nAñoH + nAñoM + nAñoS)

            Me._HabMagnitudAlt(0) = Me._HabMagnitudAlt(0) / nAñoH
            Me._HabMagnitudAlt(1) = Me._HabMagnitudAlt(1) / nAñoM
            Me._HabMagnitudAlt(2) = Me._HabMagnitudAlt(2) / nAñoS

            'Me._HabMagnitudAlt(0) = Me._HabMagnitudAlt(0) / nAñoH
            'Me._HabMagnitudAlt(1) = Me._HabMagnitudAlt(1) / nAñoM
            'Me._HabMagnitudAlt(2) = Me._HabMagnitudAlt(2) / nAñoS

            'Me._HabMagnitudAlt(3) = (Me._HabMagnitudAlt(0) + Me._HabMagnitudAlt(1) + Me._HabMagnitudAlt(2)) / 3

            ' +++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++ Calculo de la Variabilidad ++++++
            ' +++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._HabVariabilidadAlt(3)

            ' diferencias
            Dim dif() As Single
            ReDim dif(Me._AportacionAltAnualOrdAños.año.Length - 1)

            Dim acH As Single
            Dim acM As Single
            Dim acS As Single

            acH = 0
            acM = 0
            acS = 0

            For i = 0 To Me._AportacionAltAnualOrdAños.año.Length - 1
                Dim min, max As Single

                min = 99999
                max = -1

                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    For j = 0 To 11
                        Dim apor As Single
                        apor = Me._AportacionAltMen.aportacion((i * 12) + j)
                        If (apor >= max) Then
                            max = apor
                        End If
                        If (apor <= min) Then
                            min = apor
                        End If
                    Next
                    dif(i) = max - min
                    acH = acH + dif(i)
                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    For j = 0 To 11
                        Dim apor As Single
                        apor = Me._AportacionAltMen.aportacion((i * 12) + j)
                        If (apor >= max) Then
                            max = apor
                        End If
                        If (apor <= min) Then
                            min = apor
                        End If
                    Next
                    dif(i) = max - min
                    acM = acM + dif(i)
                Else
                    For j = 0 To 11
                        Dim apor As Single
                        apor = Me._AportacionAltMen.aportacion((i * 12) + j)
                        If (apor >= max) Then
                            max = apor
                        End If
                        If (apor <= min) Then
                            min = apor
                        End If
                    Next
                    dif(i) = max - min
                    acS = acS + dif(i)
                End If
            Next

            Me._HabVariabilidadAlt(3) = (acH + acM + acS) / (nAñoS + nAñoM + nAñoH)

            Me._HabVariabilidadAlt(0) = acH / nAñoH
            Me._HabVariabilidadAlt(1) = acM / nAñoM
            Me._HabVariabilidadAlt(2) = acS / nAñoS

            'Me._HabVariabilidadAlt(0) = acH / 12
            'Me._HabVariabilidadAlt(1) = acM / 12
            'Me._HabVariabilidadAlt(2) = acS / 12
            'Me._HabVariabilidadAlt(3) = (Me._HabVariabilidadAlt(0) + Me._HabVariabilidadAlt(1) + Me._HabVariabilidadAlt(2)) / 3


            ' +++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++ Calculo de la Estacionalidad ++++
            ' +++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._HabEstacionalidadAlt(2)

            Dim minH, maxH, minM, maxM, minS, maxS As Single
            Dim mesMinH, mesMaxH, mesMinM, mesMaxM, mesMinS, mesMaxS As STRING_MES_ORD

            minH = 99999
            maxH = -1
            minM = 99999
            maxM = -1
            minS = 99999
            maxS = -1

            mesMinH = 99999
            mesMaxH = -1
            mesMinM = 99999
            mesMaxM = -1
            mesMinS = 99999
            mesMaxS = -1

            For i = 0 To Me._IntraAnualAlt.Length - 1
                If (maxH < Me._IntraAnualAlt(i)(0)) Then
                    maxH = Me._IntraAnualAlt(i)(0)
                    mesMaxH = (i + Me._datos.mesInicio - 1) Mod 12
                End If
                If (minH > Me._IntraAnualAlt(i)(0)) Then
                    minH = Me._IntraAnualAlt(i)(0)
                    mesMinH = (i + Me._datos.mesInicio - 1) Mod 12
                End If

                If (maxM < Me._IntraAnualAlt(i)(1)) Then
                    maxM = Me._IntraAnualAlt(i)(1)
                    mesMaxM = (i + Me._datos.mesInicio - 1) Mod 12
                End If
                If (minM > Me._IntraAnualAlt(i)(1)) Then
                    minM = Me._IntraAnualAlt(i)(1)
                    mesMinM = (i + Me._datos.mesInicio - 1) Mod 12
                End If

                If (maxS < Me._IntraAnualAlt(i)(2)) Then
                    maxS = Me._IntraAnualAlt(i)(2)
                    mesMaxS = (i + Me._datos.mesInicio - 1) Mod 12
                End If
                If (minS > Me._IntraAnualAlt(i)(2)) Then
                    minS = Me._IntraAnualAlt(i)(2)
                    mesMinS = (i + Me._datos.mesInicio - 1) Mod 12
                End If
            Next

            'Me._HabEstacionalidadAlt(0) = mesMaxH.ToString() + "-" + mesMinH.ToString()
            'Me._HabEstacionalidadAlt(1) = mesMaxM.ToString() + "-" + mesMinM.ToString()
            'Me._HabEstacionalidadAlt(2) = mesMaxS.ToString() + "-" + mesMinS.ToString()

            Me._HabEstacionalidadAlt(0) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMaxH + 1).Substring(0, 3).ToUpper() + _
                                          "-" + _
                                          Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMinH + 1).Substring(0, 3).ToUpper()
            Me._HabEstacionalidadAlt(1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMaxM + 1).Substring(0, 3).ToUpper() + _
                                          "-" + _
                                          Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMinM + 1).Substring(0, 3).ToUpper()
            Me._HabEstacionalidadAlt(2) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMaxS + 1).Substring(0, 3).ToUpper() + _
                                          "-" + _
                                          Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMinS + 1).Substring(0, 3).ToUpper()

        End Sub
        ''' <summary>
        ''' Parámetros Habituales
        ''' </summary>
        ''' <remarks>Informe 4a y 4</remarks>
        Public Sub CalcularParametrosHabitualesCASO1()

            Dim i, j As Integer

            ReDim Me._HabMagnitudNat(3)
            Dim nAñoH, nAñoM, nAñoS As Integer

            ' +++++++++++++++++++++++++++++++++++++++
            ' ++++++++++ Calculo de la Magnitud +++++
            ' +++++++++++++++++++++++++++++++++++++++
            nAñoH = 0
            nAñoM = 0
            nAñoS = 0

            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                If (Me._AportacionNatAnual.tipo(i) = TIPOAÑO.HUMEDO) Then
                    Me._HabMagnitudNat(0) = Me._HabMagnitudNat(0) + Me._AportacionNatAnual.aportacion(i)
                    nAñoH = nAñoH + 1
                ElseIf (Me._AportacionNatAnual.tipo(i) = TIPOAÑO.MEDIO) Then
                    Me._HabMagnitudNat(1) = Me._HabMagnitudNat(1) + Me._AportacionNatAnual.aportacion(i)
                    nAñoM = nAñoM + 1
                Else
                    Me._HabMagnitudNat(2) = Me._HabMagnitudNat(2) + Me._AportacionNatAnual.aportacion(i)
                    nAñoS = nAñoS + 1
                End If
            Next

            Me._HabMagnitudNat(3) = (Me._HabMagnitudNat(0) + Me._HabMagnitudNat(1) + Me._HabMagnitudNat(2)) / (nAñoH + nAñoM + nAñoS)

            Me._HabMagnitudNat(0) = Me._HabMagnitudNat(0) / nAñoH
            Me._HabMagnitudNat(1) = Me._HabMagnitudNat(1) / nAñoM
            Me._HabMagnitudNat(2) = Me._HabMagnitudNat(2) / nAñoS



            ' +++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++ Calculo de la Variabilidad ++++++
            ' +++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._HabVariabilidadNat(3)

            ' diferencias
            Dim dif() As Single
            ReDim dif(Me._AportacionNatAnualOrdAños.año.Length - 1)

            Dim acH As Single
            Dim acM As Single
            Dim acS As Single

            acH = 0
            acM = 0
            acS = 0

            For i = 0 To Me._AportacionNatAnualOrdAños.año.Length - 1
                Dim min, max As Single

                min = 999
                max = -1

                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    For j = 0 To 11
                        Dim apor As Single
                        apor = Me._AportacionNatMen.aportacion((i * 12) + j)
                        If (apor >= max) Then
                            max = apor
                        End If
                        If (apor <= min) Then
                            min = apor
                        End If
                    Next
                    dif(i) = max - min
                    acH = acH + dif(i)
                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    For j = 0 To 11
                        Dim apor As Single
                        apor = Me._AportacionNatMen.aportacion((i * 12) + j)
                        If (apor >= max) Then
                            max = apor
                        End If
                        If (apor <= min) Then
                            min = apor
                        End If
                    Next
                    dif(i) = max - min
                    acM = acM + dif(i)
                Else
                    For j = 0 To 11
                        Dim apor As Single
                        apor = Me._AportacionNatMen.aportacion((i * 12) + j)
                        If (apor >= max) Then
                            max = apor
                        End If
                        If (apor <= min) Then
                            min = apor
                        End If
                    Next
                    dif(i) = max - min
                    acS = acS + dif(i)
                End If
            Next

            Me._HabVariabilidadNat(3) = (acH + acM + acS) / (nAñoS + nAñoM + nAñoH)

            Me._HabVariabilidadNat(0) = acH / nAñoH
            Me._HabVariabilidadNat(1) = acM / nAñoM
            Me._HabVariabilidadNat(2) = acS / nAñoS

            ' +++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++ Calculo de la Estacionalidad ++++
            ' +++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._HabEstacionalidadNat(2)

            Dim minH, maxH, minM, maxM, minS, maxS As Single
            Dim mesMinH, mesMaxH, mesMinM, mesMaxM, mesMinS, mesMaxS As STRING_MES_ORD

            minH = 999999
            maxH = -1
            minM = 999999
            maxM = -1
            minS = 999999
            maxS = -1

            mesMinH = 999
            mesMaxH = -1
            mesMinM = 999
            mesMaxM = -1
            mesMinS = 999
            mesMaxS = -1

            For i = 0 To Me._IntraAnualNat.Length - 1
                If (maxH < Me._IntraAnualNat(i)(0)) Then
                    maxH = Me._IntraAnualNat(i)(0)
                    mesMaxH = (i + Me._datos.mesInicio - 1) Mod 12
                End If
                If (minH > Me._IntraAnualNat(i)(0)) Then
                    minH = Me._IntraAnualNat(i)(0)
                    mesMinH = (i + Me._datos.mesInicio - 1) Mod 12
                End If

                If (maxM < Me._IntraAnualNat(i)(1)) Then
                    maxM = Me._IntraAnualNat(i)(1)
                    mesMaxM = (i + Me._datos.mesInicio - 1) Mod 12
                End If
                If (minM > Me._IntraAnualNat(i)(1)) Then
                    minM = Me._IntraAnualNat(i)(1)
                    mesMinM = (i + Me._datos.mesInicio - 1) Mod 12
                End If

                If (maxS < Me._IntraAnualNat(i)(2)) Then
                    maxS = Me._IntraAnualNat(i)(2)
                    mesMaxS = (i + Me._datos.mesInicio - 1) Mod 12
                End If
                If (minS > Me._IntraAnualNat(i)(2)) Then
                    minS = Me._IntraAnualNat(i)(2)
                    mesMinS = (i + Me._datos.mesInicio - 1) Mod 12
                End If
            Next

            'Me._HabEstacionalidadNat(0) = mesMaxH.ToString() + "-" + mesMinH.ToString()
            'Me._HabEstacionalidadNat(1) = mesMaxM.ToString() + "-" + mesMinM.ToString()
            'Me._HabEstacionalidadNat(2) = mesMaxS.ToString() + "-" + mesMinS.ToString()

            Me._HabEstacionalidadNat(0) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMaxH + 1).Substring(0, 3).ToUpper() + _
                                          "-" + _
                                          Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMinH + 1).Substring(0, 3).ToUpper()
            Me._HabEstacionalidadNat(1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMaxM + 1).Substring(0, 3).ToUpper() + _
                                          "-" + _
                                          Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMinM + 1).Substring(0, 3).ToUpper()
            Me._HabEstacionalidadNat(2) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMaxS + 1).Substring(0, 3).ToUpper() + _
                                          "-" + _
                                          Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, mesMinS + 1).Substring(0, 3).ToUpper()


        End Sub
        Public Sub CalcularParametrosHabitualesCASO3()

            ' Los valores habituales de las series naturales se calculan igual
            Me.CalcularParametrosHabitualesCASO1()

            ' Calculo de los alterados
            Me.CalcularParametrosHabitualesAlterados()

        End Sub
        ''' <summary>
        ''' Calculo de los parametros habituales Alterados sin cotaeniedad
        ''' </summary>
        ''' <remarks>Se calcula en el informe 5a y 5c</remarks>
        Private Sub CalcularParametrosHabitualesReducidos()

            Dim i, j As Integer

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++ CAMBIOS 9/1/08 ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            '   Se cambian los informes 5A y 5C   +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._HabMagnitudAnualAlt(2)
            ReDim Me._HabMagnitudMensualAlt(1)
            ReDim Me._HabMagnitudMensualTablaAlt(2)
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' ++++++++++++++++++++++++++++++++++++
            ' ++++++++++ Magnitud Anual ++++++++++
            ' ++++++++++++++++++++++++++++++++++++
            ' ---- Alterados -----
            ReDim Me._HabMagnitudAlt(0)
            Dim acum As Single = 0

            ' Comprobar que la lista de aportaciones anuales alteradas existe
            If (Me._AportacionAltAnual.año Is Nothing) Then
                Me.CalcularINTERAnualAlterada()
            End If

            For i = 0 To Me._AportacionAltAnual.año.Length - 1
                acum = acum + Me._AportacionAltAnual.aportacion(i)
            Next

            ' Media de aportaciones anuales
            ' -----------------------------
            Me._HabMagnitudAlt(0) = acum / Me._AportacionAltAnual.año.Length

            ' ---- Naturales ----
            acum = 0
            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                acum = acum + Me._AportacionNatAnual.aportacion(i)
            Next
            Me._HabMagnitudNatReducido = acum / Me._AportacionNatAnual.año.Length

            ' +++++++++++++++++++++++++++++++++++++++
            ' +++++ CAMBIOS 9/1/08 ++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++
            '   Se cambian los informes 5A y 5C   +++
            ' +++++++++++++++++++++++++++++++++++++++

            ' +++++++++++++ Parametros Habituales Alterados ANUALES +++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' Dejo la variable donde se espera y me la copio en un nuevo array para
            ' poder luego representarla.
            Me._HabMagnitudAnualAlt(0) = Me._HabMagnitudAlt(0)

            ' +++++++ Mediana +++++++
            ' +++++++++++++++++++++++
            Dim aporOrd() As Single

            aporOrd = Me._AportacionAltAnual.aportacion.Clone()

            Array.Sort(aporOrd)

            If ((aporOrd.Length Mod 2) = 0) Then
                Me._HabMagnitudAnualAlt(1) = (aporOrd((aporOrd.Length / 2) - 1) + aporOrd(aporOrd.Length / 2)) / 2
            Else
                Me._HabMagnitudAnualAlt(1) = aporOrd((aporOrd.Length - 1) / 2)
            End If

            ' +++++ Desv Estan +++++++
            ' ++++++++++++++++++++++++
            Dim aux1, aux2 As Single
            Dim desvEst As Single

            aux1 = 0
            aux2 = 0
            For i = 0 To aporOrd.Length - 1
                aux1 = aux1 + Pow(aporOrd(i), 2)
                aux2 = aux2 + aporOrd(i)
            Next

            desvEst = Sqrt((aporOrd.Length * aux1 - Pow(aux2, 2)) / (aporOrd.Length * (aporOrd.Length - 1)))

            ' +++++ Coe de Variación +++++
            ' ++++++++++++++++++++++++++++
            Me._HabMagnitudAnualAlt(2) = desvEst / Me._HabMagnitudAnualAlt(0)

            ' +++++++++++++ Parametros Habituales Alterados MENSUALES +++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._HabMagnitudMensualTablaAlt(0).ndias(11)
            ReDim Me._HabMagnitudMensualTablaAlt(1).ndias(11)
            ReDim Me._HabMagnitudMensualTablaAlt(2).ndias(11)

            Dim auxMes As Integer
            Dim auxMediana()() As Single
            Dim auxDesv() As Single

            ReDim auxMediana(11)
            ReDim auxDesv(11)

            For i = 0 To 11
                Me._HabMagnitudMensualTablaAlt(0).ndias(11) = 0
                Me._HabMagnitudMensualTablaAlt(1).ndias(11) = 0
                Me._HabMagnitudMensualTablaAlt(2).ndias(11) = 0
                ReDim auxMediana(i)(Me._datos.SerieAltMensual.nAños - 1)
            Next

            ' ++++++ Calculo de la media ++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++
            For i = 0 To Me._AportacionAltMen.aportacion.Length - 1
                auxMes = Me._AportacionAltMen.mes(i).Month - 1
                Me._HabMagnitudMensualTablaAlt(0).ndias(auxMes) = Me._HabMagnitudMensualTablaAlt(0).ndias(auxMes) + Me._AportacionAltMen.aportacion(i)
            Next

            For i = 0 To 11
                Me._HabMagnitudMensualTablaAlt(0).ndias(i) = Me._HabMagnitudMensualTablaAlt(0).ndias(i) / Me._datos.SerieAltMensual.nAños
            Next

            ' +++++ Calculo de la mediana +++++++++++
            ' +++++++++++++++++++++++++++++++++++++++
            Dim añoIni As Integer
            añoIni = Me._AportacionAltMen.mes(0).Year
            j = 0
            For i = 0 To Me._AportacionAltMen.aportacion.Length - 1
                If ((añoIni <> Me._AportacionAltMen.mes(i).Year) And (Me._AportacionAltMen.mes(i).Month = Me._datos.mesInicio)) Then
                    j = j + 1
                End If
                auxMediana(Me._AportacionAltMen.mes(i).Month - 1)(j) = Me._AportacionAltMen.aportacion(i)
            Next
            For i = 0 To 11
                Array.Sort(auxMediana(i))
                If ((Me._datos.SerieAltMensual.nAños) = 0) Then
                    Me._HabMagnitudMensualTablaAlt(1).ndias(i) = (auxMediana(i)((Me._datos.SerieAltMensual.nAños / 2) - 1) + (Me._datos.SerieAltMensual.nAños / 2)) / 2
                Else
                    Me._HabMagnitudMensualTablaAlt(1).ndias(i) = auxMediana(i)((Me._datos.SerieAltMensual.nAños - 1) / 2)
                End If
            Next


            ' +++++ Calculo del coeficiente +++++++++
            ' +++++++++++++++++++++++++++++++++++++++
            ' PREVIO: Calculo de las desv. estandar
            For i = 0 To 11
                aux1 = 0
                aux2 = 0
                For j = 0 To Me._datos.SerieAltMensual.nAños - 1
                    aux1 = aux1 + Pow(auxMediana(i)(j), 2)
                    aux2 = aux2 + auxMediana(i)(j)
                Next
                auxDesv(i) = Sqrt((Me._datos.SerieAltMensual.nAños * aux1 - Pow(aux2, 2)) / (Me._datos.SerieAltMensual.nAños * (Me._datos.SerieAltMensual.nAños - 1)))
            Next
            ' Calculo del coeficiente
            For i = 0 To 11
                Me._HabMagnitudMensualTablaAlt(2).ndias(i) = auxDesv(i) / Me._HabMagnitudMensualTablaAlt(0).ndias(i)
            Next


            ' +++++++++++++++++++++++++++++++
            ' +++++ Estacionalidad ++++++++++
            ' +++++++++++++++++++++++++++++++
            '
            ReDim Me._HabEstacionalidadMensualAlt(1)

            ' ----------------------------------------------------------------
            '  > Valores habituales > Aportaciones mensuales > Estacionalidad
            ' ----------------------------------------------------------------
            ' Busqueda de aparicion de maximo / minimos en los meses del año
            ReDim Me._HabEstacionalidadMensualAlt(0).ndias(11)
            ReDim Me._HabEstacionalidadMensualAlt(1).ndias(11)

            Dim auxMes1() As Integer
            Dim auxMes2() As Integer

            ReDim auxMes1(0)
            ReDim auxMes2(0)

            Dim numMaximo As Integer = 0
            Dim numMinimo As Integer = 0

            añoIni = Me._AportacionAltMen.mes(0).Year
            aux1 = -1
            aux2 = 99999

            Dim nveces As Integer = 0
            For i = 0 To Me._AportacionAltMen.aportacion.Length - 1
                ' Se acaba el año, y meto todo los max/min encontrados en la lista final
                If ((añoIni <> Me._AportacionAltMen.mes(i).Year) And (Me._AportacionAltMen.mes(i).Month = Me._datos.mesInicio)) Then
                    añoIni = Me._AportacionAltMen.mes(i).Year
                    aux1 = -1
                    aux2 = 99999

                    nveces += 1

                    For j = 0 To auxMes1.Length - 2
                        Me._HabEstacionalidadMensualAlt(0).ndias(auxMes1(j) - 1) = Me._HabEstacionalidadMensualAlt(0).ndias(auxMes1(j) - 1) + 1
                    Next
                    For j = 0 To auxMes2.Length - 2
                        Me._HabEstacionalidadMensualAlt(1).ndias(auxMes2(j) - 1) = Me._HabEstacionalidadMensualAlt(1).ndias(auxMes2(j) - 1) + 1
                    Next

                    ' Contabilizar el numero total de maximos/minimos que se encuentran a lo largo de los años
                    numMaximo += auxMes1.Length - 1
                    numMinimo += auxMes2.Length - 1

                End If

                If (aux1 <= Me._AportacionAltMen.aportacion(i)) Then
                    ' Si es igual lo añado a la lista
                    If (aux1 = Me._AportacionAltMen.aportacion(i)) Then
                        auxMes1(auxMes1.Length - 1) = Me._AportacionAltMen.mes(i).Month
                        ReDim Preserve auxMes1(auxMes1.Length)
                    Else
                        ReDim auxMes1(1)
                        auxMes1(0) = Me._AportacionAltMen.mes(i).Month
                    End If
                    aux1 = Me._AportacionAltMen.aportacion(i)
                End If

                If (aux2 >= Me._AportacionAltMen.aportacion(i)) Then
                    If (aux2 = Me._AportacionAltMen.aportacion(i)) Then
                        auxMes2(auxMes2.Length - 1) = Me._AportacionAltMen.mes(i).Month
                        ReDim Preserve auxMes2(auxMes2.Length)
                    Else
                        ReDim auxMes2(1)
                        auxMes2(0) = Me._AportacionAltMen.mes(i).Month
                    End If
                    aux2 = Me._AportacionAltMen.aportacion(i)
                End If
            Next

            ' ERROR DOC 27/08/09 - CA XXX
            ' -- Fallo en caso 6: Guadiana
            ' ----------------------------
            ' Ultima ejecución
            For j = 0 To auxMes1.Length - 2
                Me._HabEstacionalidadMensualAlt(0).ndias(auxMes1(j) - 1) = Me._HabEstacionalidadMensualAlt(0).ndias(auxMes1(j) - 1) + 1
            Next
            For j = 0 To auxMes2.Length - 2
                Me._HabEstacionalidadMensualAlt(1).ndias(auxMes2(j) - 1) = Me._HabEstacionalidadMensualAlt(1).ndias(auxMes2(j) - 1) + 1
            Next

            numMaximo += auxMes1.Length - 1
            numMinimo += auxMes2.Length - 1

            For i = 0 To 11
                'Me._HabEstacionalidadMensualAlt(0).ndias(i) = Me._HabEstacionalidadMensualAlt(0).ndias(i) / Me._datos.SerieAltMensual.nAños
                'Me._HabEstacionalidadMensualAlt(1).ndias(i) = Me._HabEstacionalidadMensualAlt(1).ndias(i) / Me._datos.SerieAltMensual.nAños
                Me._HabEstacionalidadMensualAlt(0).ndias(i) = Me._HabEstacionalidadMensualAlt(0).ndias(i) / numMaximo
                Me._HabEstacionalidadMensualAlt(1).ndias(i) = Me._HabEstacionalidadMensualAlt(1).ndias(i) / numMinimo
            Next

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' Apor Mensuales > Magnitud > Variabilidad extrema 
            ' ++++++++++++++++++++++++++++++++++++++++++++++++

            ' ---- Alterados -----
            ReDim Me._HabVariabilidadAlt(0)
            Dim nMaximosAlt(11) As Integer
            Dim nMaximosNat(11) As Integer
            Dim nMinimosAlt(11) As Integer
            Dim nMinimosNat(11) As Integer

            For i = 0 To 11
                nMaximosAlt(i) = 0
                nMaximosNat(i) = 0
                nMinimosAlt(i) = 0
                nMinimosNat(i) = 0
            Next

            Dim max As Integer
            Dim min As Integer

            Dim pos As Integer
            acum = 0

            For i = 0 To Me._AportacionAltAnual.año.Length - 1
                max = -1
                min = 999999999
                pos = i * 12
                For j = 0 To 11
                    If (max < Me._AportacionAltMen.aportacion(pos + j)) Then
                        max = Me._AportacionAltMen.aportacion(pos + j)
                        nMaximosAlt(j) = nMaximosAlt(j) + 1
                    End If
                    If (min > Me._AportacionAltMen.aportacion(pos + j)) Then
                        min = Me._AportacionAltMen.aportacion(pos + j)
                        nMinimosAlt(j) = nMinimosAlt(j) + 1
                    End If
                Next
                acum = acum + (max - min)
            Next
            Me._HabVariabilidadAlt(0) = acum / Me._AportacionAltAnual.año.Length

            ' ---- Naturales -----
            acum = 0

            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                max = -1
                min = 999999999
                pos = i * 12
                For j = 0 To 11
                    If (max < Me._AportacionNatMen.aportacion(pos + j)) Then
                        max = Me._AportacionNatMen.aportacion(pos + j)
                        nMaximosNat(j) = nMaximosNat(j) + 1
                    End If
                    If (min > Me._AportacionNatMen.aportacion(pos + j)) Then
                        min = Me._AportacionNatMen.aportacion(pos + j)
                    End If
                Next
                acum = acum + (max - min)
            Next
            Me._HabVariabilidadNatReducido = acum / Me._AportacionNatAnual.año.Length

            ' +++++++ Estacionalidad ++++++++++
            ' +++++++++++++++++++++++++++++++++

            ' ¿Que hay que hacer aqui?
            Dim sMax, sMin As STRING_MES_ORD
            Dim EstMax, EstMin As Single

            ' Calcular la tabla de frecuencias

            ReDim Me._TablaFrecuenciaMaxMin.nat(11)
            ReDim Me._TablaFrecuenciaMaxMin.alt(11)
            ReDim Me._TablaFrecuenciaMaxMin.minNat(11)
            ReDim Me._TablaFrecuenciaMaxMin.minAlt(11)
            ReDim Me._TablaFrecuenciaMaxMin.posMaxAlt(11)
            ReDim Me._TablaFrecuenciaMaxMin.posMaxNat(11)
            ReDim Me._TablaFrecuenciaMaxMin.posMinNat(11)
            ReDim Me._TablaFrecuenciaMaxMin.posMinAlt(11)

            'Dim max As Single = 0
            'Dim pos As Single = 0

            For i = 0 To 11
                Me._TablaFrecuenciaMaxMin.nat(i) = 0
                Me._TablaFrecuenciaMaxMin.alt(i) = 0
                Me._TablaFrecuenciaMaxMin.minNat(i) = 0
                Me._TablaFrecuenciaMaxMin.minAlt(i) = 0
                Me._TablaFrecuenciaMaxMin.posMaxAlt(i) = False
                Me._TablaFrecuenciaMaxMin.posMaxNat(i) = False
                Me._TablaFrecuenciaMaxMin.posMinNat(i) = False
                Me._TablaFrecuenciaMaxMin.posMinAlt(i) = False
            Next


            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                max = 0
                For j = 0 To 11
                    If (max < Me._AportacionNatMen.aportacion(i * 12 + j)) Then
                        max = Me._AportacionNatMen.aportacion(i * 12 + j)
                        pos = j
                    End If

                Next
                Me._TablaFrecuenciaMaxMin.nat(pos) = Me._TablaFrecuenciaMaxMin.nat(pos) + 1
            Next

            EstMax = 0
            EstMin = 99999
            For i = 0 To Me._AportacionAltAnual.año.Length - 1
                max = 0
                For j = 0 To 11
                    If (max < Me._AportacionAltMen.aportacion(i * 12 + j)) Then
                        max = Me._AportacionAltMen.aportacion(i * 12 + j)
                        pos = j
                        If (max > EstMax) Then
                            EstMax = max
                            sMax = (j + Me._datos.mesInicio - 1) Mod 12
                        End If
                    End If
                    If (Me._AportacionAltMen.aportacion(i * 12 + j) < EstMin) Then
                        EstMin = Me._AportacionAltMen.aportacion(i * 12 + j)
                        sMin = (j + Me._datos.mesInicio - 1) Mod 12
                    End If
                Next
                Me._TablaFrecuenciaMaxMin.alt(pos) = Me._TablaFrecuenciaMaxMin.alt(pos) + 1
            Next

            ' Tratamiento de mínimos
            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                min = 999999
                For j = 0 To 11
                    If (min > Me._AportacionNatMen.aportacion(i * 12 + j)) Then
                        min = Me._AportacionNatMen.aportacion(i * 12 + j)
                        pos = j
                    End If
                Next
                Me._TablaFrecuenciaMaxMin.minNat(pos) = Me._TablaFrecuenciaMaxMin.minNat(pos) + 1

            Next

            For i = 0 To Me._AportacionAltAnual.año.Length - 1
                min = 999999
                For j = 0 To 11
                    If (min > Me._AportacionAltMen.aportacion(i * 12 + j)) Then
                        min = Me._AportacionAltMen.aportacion(i * 12 + j)
                        pos = j
                    End If
                Next
                Me._TablaFrecuenciaMaxMin.minAlt(pos) = Me._TablaFrecuenciaMaxMin.minAlt(pos) + 1
            Next

            max = 0
            'min = 9999999
            min = 0
            For i = 0 To 11
                If (max < Me._TablaFrecuenciaMaxMin.nat(i)) Then
                    max = Me._TablaFrecuenciaMaxMin.nat(i)
                End If
                'If (min > Me._TablaFrecuenciaMaxMin.nat(i)) Then
                ' min = Me._TablaFrecuenciaMaxMin.nat(i)
                'End If
                If (min < Me._TablaFrecuenciaMaxMin.minNat(i)) Then
                    min = Me._TablaFrecuenciaMaxMin.minNat(i)
                End If
            Next
            For i = 0 To 11
                If (max = Me._TablaFrecuenciaMaxMin.nat(i)) Then
                    Me._TablaFrecuenciaMaxMin.posMaxNat(i) = True
                End If
                'If (min = Me._TablaFrecuenciaMaxMin.nat(i)) Then
                If (min = Me._TablaFrecuenciaMaxMin.minNat(i)) Then
                    Me._TablaFrecuenciaMaxMin.posMinNat(i) = True
                End If
            Next
            max = 0
            'min = 9999999
            min = 0
            For i = 0 To 11
                If (max < Me._TablaFrecuenciaMaxMin.alt(i)) Then
                    max = Me._TablaFrecuenciaMaxMin.alt(i)
                End If
                'If (min > Me._TablaFrecuenciaMaxMin.alt(i)) Then
                'min = Me._TablaFrecuenciaMaxMin.alt(i)
                'End If
                If (min < Me._TablaFrecuenciaMaxMin.minAlt(i)) Then
                    min = Me._TablaFrecuenciaMaxMin.minAlt(i)
                End If
            Next
            For i = 0 To 11
                If (max = Me._TablaFrecuenciaMaxMin.alt(i)) Then
                    Me._TablaFrecuenciaMaxMin.posMaxAlt(i) = True
                End If
                'If (min = Me._TablaFrecuenciaMaxMin.alt(i)) Then
                If (min = Me._TablaFrecuenciaMaxMin.minAlt(i)) Then
                    Me._TablaFrecuenciaMaxMin.posMinAlt(i) = True
                End If
            Next

            ReDim Me._HabEstacionalidadAlt(0)

            'Me._HabEstacionalidadAlt(0) = sMax.ToString() + "-" + sMin.ToString()
            Me._HabEstacionalidadAlt(0) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, sMax + 1).Substring(0, 3).ToUpper() + _
                                          "-" + _
                                          Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, sMin + 1).Substring(0, 3).ToUpper()

        End Sub
        ''' <summary>
        ''' Calculo de los parametros habituales Naturales sin cotaeniedad
        ''' </summary>
        ''' <remarks>Se calcula en el informe 4b</remarks>
        Private Sub CalcularParametrosNaturalesHabitualesReducidos()

            ' Este calculo es una copia de lo que se hace para el informe 5

            Dim i, j As Integer

            ' Definir las variables
            ReDim Me._HabMagnitudAnualNat(2)
            ReDim Me._HabMagnitudMensualNat(0)
            ReDim Me._HabMagnitudMensualTablaNat(2)
            ReDim Me._HabEstacionalidadMensualNat(1)

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++ Parametros Habituales Naturales ANUALES ++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' Comprobar que la lista de aportaciones anuales NATURALES existe
            If (Me._AportacionNatAnual.año Is Nothing) Then
                'Me.CalcularINTERAnualAlterada()
                Me.CalcularINTERAnual()
            End If

            ' +++++ Apor Anuales > Magnitud > Media +++++
            ' +++++++++++++++++++++++++++++++++++++++++++
            'ReDim Me._HabMagnitudNat(0)
            Dim acum As Single = 0

            ' Saco la media (ya esta calculada previamente pero se vuelve a calcular)
            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                acum = acum + Me._AportacionNatAnual.aportacion(i)
            Next
            'Me._HabMagnitudNat(0) = acum / Me._AportacionAltAnual.año.Length
            Me._HabMagnitudAnualNat(0) = acum / Me._AportacionNatAnual.año.Length

            ' Extra que se usará en un futuro
            acum = 0
            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                acum = acum + Me._AportacionNatAnual.aportacion(i)
            Next
            Me._HabMagnitudNatReducido = acum / Me._AportacionNatAnual.año.Length

            ' +++++++ Aport Anuales > Magnitud >Mediana +++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++
            Dim aporOrd() As Single

            aporOrd = Me._AportacionNatAnual.aportacion.Clone()

            Array.Sort(aporOrd)

            If ((aporOrd.Length Mod 2) = 0) Then
                Me._HabMagnitudAnualNat(1) = (aporOrd((aporOrd.Length / 2) - 1) + aporOrd(aporOrd.Length / 2)) / 2
            Else
                Me._HabMagnitudAnualNat(1) = aporOrd((aporOrd.Length - 1) / 2)
            End If

            ' +++++ Aport Anuales > Magnitud > Coe de Variación +++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' Calculo la desviacion estandar
            Dim aux1, aux2 As Single
            Dim desvEst As Single
            aux1 = 0
            aux2 = 0
            For i = 0 To aporOrd.Length - 1
                aux1 = aux1 + Pow(aporOrd(i), 2)
                aux2 = aux2 + aporOrd(i)
            Next
            desvEst = Sqrt((aporOrd.Length * aux1 - Pow(aux2, 2)) / (aporOrd.Length * (aporOrd.Length - 1)))
            Me._HabMagnitudAnualNat(2) = desvEst / Me._HabMagnitudAnualNat(0)

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++ Parametros Habituales Alterados MENSUALES ++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++

            ' Defino las variables
            ReDim Me._HabMagnitudMensualTablaNat(0).ndias(11)
            ReDim Me._HabMagnitudMensualTablaNat(1).ndias(11)
            ReDim Me._HabMagnitudMensualTablaNat(2).ndias(11)

            Dim auxMes As Integer
            Dim auxMediana()() As Single
            Dim auxDesv() As Single

            ReDim auxMediana(11)
            ReDim auxDesv(11)

            For i = 0 To 11
                Me._HabMagnitudMensualTablaNat(0).ndias(11) = 0
                Me._HabMagnitudMensualTablaNat(1).ndias(11) = 0
                Me._HabMagnitudMensualTablaNat(2).ndias(11) = 0
                ReDim auxMediana(i)(Me._datos.SerieNatMensual.nAños - 1)
            Next

            ' ++++ Aport Mensual > Magnitud > media ++++
            ' ++++++++++++++++++++++++++++++++++++++++++
            For i = 0 To Me._AportacionNatMen.aportacion.Length - 1
                auxMes = Me._AportacionNatMen.mes(i).Month - 1
                Me._HabMagnitudMensualTablaNat(0).ndias(auxMes) = Me._HabMagnitudMensualTablaNat(0).ndias(auxMes) + Me._AportacionNatMen.aportacion(i)
            Next

            For i = 0 To 11
                Me._HabMagnitudMensualTablaNat(0).ndias(i) = Me._HabMagnitudMensualTablaNat(0).ndias(i) / Me._datos.SerieNatMensual.nAños
            Next

            ' ++++ Aport Mensual > Magnitud > mediana ++++
            ' ++++++++++++++++++++++++++++++++++++++++++++
            Dim añoIni As Integer
            añoIni = Me._AportacionNatMen.mes(0).Year
            j = 0
            For i = 0 To Me._AportacionNatMen.aportacion.Length - 1
                If ((añoIni <> Me._AportacionNatMen.mes(i).Year) And (Me._AportacionNatMen.mes(i).Month = Me._datos.mesInicio)) Then
                    j = j + 1
                End If
                auxMediana(Me._AportacionNatMen.mes(i).Month - 1)(j) = Me._AportacionNatMen.aportacion(i)
            Next
            For i = 0 To 11
                Array.Sort(auxMediana(i))
                If ((Me._datos.SerieNatMensual.nAños) = 0) Then
                    Me._HabMagnitudMensualTablaNat(1).ndias(i) = (auxMediana(i)((Me._datos.SerieNatMensual.nAños / 2) - 1) + (Me._datos.SerieNatMensual.nAños / 2)) / 2
                Else
                    Me._HabMagnitudMensualTablaNat(1).ndias(i) = auxMediana(i)((Me._datos.SerieNatMensual.nAños - 1) / 2)
                End If
            Next


            ' +++ Aport Mensual > Magnitud > Coef. de var. +++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' PREVIO: Calculo de las desv. estandar
            For i = 0 To 11
                aux1 = 0
                aux2 = 0
                For j = 0 To Me._datos.SerieNatMensual.nAños - 1
                    aux1 = aux1 + Pow(auxMediana(i)(j), 2)
                    aux2 = aux2 + auxMediana(i)(j)
                Next
                auxDesv(i) = Sqrt((Me._datos.SerieNatMensual.nAños * aux1 - Pow(aux2, 2)) / (Me._datos.SerieNatMensual.nAños * (Me._datos.SerieNatMensual.nAños - 1)))
            Next
            ' Calculo del coeficiente
            For i = 0 To 11
                Me._HabMagnitudMensualTablaNat(2).ndias(i) = auxDesv(i) / Me._HabMagnitudMensualTablaNat(0).ndias(i)
            Next


            ' +++++++++++++++++++++++++++++++
            ' +++++ Estacionalidad ++++++++++
            ' +++++++++++++++++++++++++++++++
            '
            ' ----------------------------------------------------------------
            '  > Valores habituales > Aportaciones mensuales > Estacionalidad
            ' ----------------------------------------------------------------
            ' Busqueda de aparicion de maximo / minimos en los meses del año
            ReDim Me._HabEstacionalidadMensualNat(0).ndias(11)
            ReDim Me._HabEstacionalidadMensualNat(1).ndias(11)

            Dim auxMes1() As Integer
            Dim auxMes2() As Integer

            ReDim auxMes1(0)
            ReDim auxMes2(0)

            Dim numMaximo As Integer = 0
            Dim numMinimo As Integer = 0

            añoIni = Me._AportacionNatMen.mes(0).Year
            aux1 = -1
            aux2 = 99999

            Dim nveces As Integer = 0
            For i = 0 To Me._AportacionNatMen.aportacion.Length - 1
                ' Se acaba el año, y meto todo los max/min encontrados en la lista final
                If ((añoIni <> Me._AportacionNatMen.mes(i).Year) And (Me._AportacionNatMen.mes(i).Month = Me._datos.mesInicio)) Then
                    añoIni = Me._AportacionNatMen.mes(i).Year
                    aux1 = -1
                    aux2 = 99999

                    nveces += 1

                    For j = 0 To auxMes1.Length - 2
                        Me._HabEstacionalidadMensualNat(0).ndias(auxMes1(j) - 1) = Me._HabEstacionalidadMensualNat(0).ndias(auxMes1(j) - 1) + 1
                    Next
                    For j = 0 To auxMes2.Length - 2
                        Me._HabEstacionalidadMensualNat(1).ndias(auxMes2(j) - 1) = Me._HabEstacionalidadMensualNat(1).ndias(auxMes2(j) - 1) + 1
                    Next

                    ' Contabilizar el numero total de maximos/minimos que se encuentran a lo largo de los años
                    numMaximo += auxMes1.Length - 1
                    numMinimo += auxMes2.Length - 1

                End If

                If (aux1 <= Me._AportacionNatMen.aportacion(i)) Then
                    ' Si es igual lo añado a la lista
                    If (aux1 = Me._AportacionNatMen.aportacion(i)) Then
                        auxMes1(auxMes1.Length - 1) = Me._AportacionNatMen.mes(i).Month
                        ReDim Preserve auxMes1(auxMes1.Length)
                    Else
                        ReDim auxMes1(1)
                        auxMes1(0) = Me._AportacionNatMen.mes(i).Month
                    End If
                    aux1 = Me._AportacionNatMen.aportacion(i)
                End If

                If (aux2 >= Me._AportacionNatMen.aportacion(i)) Then
                    If (aux2 = Me._AportacionNatMen.aportacion(i)) Then
                        auxMes2(auxMes2.Length - 1) = Me._AportacionNatMen.mes(i).Month
                        ReDim Preserve auxMes2(auxMes2.Length)
                    Else
                        ReDim auxMes2(1)
                        auxMes2(0) = Me._AportacionNatMen.mes(i).Month
                    End If
                    aux2 = Me._AportacionNatMen.aportacion(i)
                End If
            Next

            ' ERROR DOC 27/08/09 - CA XXX
            ' -- Fallo en caso 6: Guadiana
            ' ----------------------------
            ' Ultima ejecución
            For j = 0 To auxMes1.Length - 2
                Me._HabEstacionalidadMensualNat(0).ndias(auxMes1(j) - 1) = Me._HabEstacionalidadMensualNat(0).ndias(auxMes1(j) - 1) + 1
            Next
            For j = 0 To auxMes2.Length - 2
                Me._HabEstacionalidadMensualNat(1).ndias(auxMes2(j) - 1) = Me._HabEstacionalidadMensualNat(1).ndias(auxMes2(j) - 1) + 1
            Next

            numMaximo += auxMes1.Length - 1
            numMinimo += auxMes2.Length - 1

            For i = 0 To 11
                'Me._HabEstacionalidadMensualAlt(0).ndias(i) = Me._HabEstacionalidadMensualAlt(0).ndias(i) / Me._datos.SerieAltMensual.nAños
                'Me._HabEstacionalidadMensualAlt(1).ndias(i) = Me._HabEstacionalidadMensualAlt(1).ndias(i) / Me._datos.SerieAltMensual.nAños
                Me._HabEstacionalidadMensualNat(0).ndias(i) = Me._HabEstacionalidadMensualNat(0).ndias(i) / numMaximo
                Me._HabEstacionalidadMensualNat(1).ndias(i) = Me._HabEstacionalidadMensualNat(1).ndias(i) / numMinimo
            Next

            ' +++ Aport Mensual > Magnitud > Varia. extrema +++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++
            ' ---- Alterados -----
            ReDim Me._HabVariabilidadAlt(0)
            'Dim nMaximosAlt(11) As Integer
            Dim nMaximosNat(11) As Integer
            'Dim nMinimosAlt(11) As Integer
            Dim nMinimosNat(11) As Integer

            For i = 0 To 11
                'nMaximosAlt(i) = 0
                nMaximosNat(i) = 0
                'nMinimosAlt(i) = 0
                nMinimosNat(i) = 0
            Next

            Dim max As Integer
            Dim min As Integer

            Dim pos As Integer
            acum = 0

            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                max = -1
                min = 999999999
                pos = i * 12
                For j = 0 To 11
                    If (max < Me._AportacionNatMen.aportacion(pos + j)) Then
                        max = Me._AportacionNatMen.aportacion(pos + j)
                        nMaximosNat(j) = nMaximosNat(j) + 1
                    End If
                    If (min > Me._AportacionNatMen.aportacion(pos + j)) Then
                        min = Me._AportacionNatMen.aportacion(pos + j)
                        nMinimosNat(j) = nMinimosNat(j) + 1
                    End If
                Next
                acum = acum + (max - min)
            Next
            'Me._HabVariabilidadAlt(0) = acum / Me._AportacionAltAnual.año.Length
            Me._HabMagnitudMensualNat(0) = acum / Me._AportacionNatAnual.año.Length

            '' ---- Naturales -----
            'acum = 0

            'For i = 0 To Me._AportacionNatAnual.año.Length - 1
            '    max = -1
            '    min = 999999999
            '    pos = i * 12
            '    For j = 0 To 11
            '        If (max < Me._AportacionNatMen.aportacion(pos + j)) Then
            '            max = Me._AportacionNatMen.aportacion(pos + j)
            '            nMaximosNat(j) = nMaximosNat(j) + 1
            '        End If
            '        If (min > Me._AportacionNatMen.aportacion(pos + j)) Then
            '            min = Me._AportacionNatMen.aportacion(pos + j)
            '        End If
            '    Next
            '    acum = acum + (max - min)
            'Next
            'Me._HabVariabilidadNatReducido = acum / Me._AportacionNatAnual.año.Length

            '' +++++++ Estacionalidad ++++++++++
            '' +++++++++++++++++++++++++++++++++

            '' ¿Que hay que hacer aqui?
            'Dim sMax, sMin As STRING_MES_ORD
            'Dim EstMax, EstMin As Single

            '' Calcular la tabla de frecuencias

            'ReDim Me._TablaFrecuenciaMaxMin.nat(11)
            'ReDim Me._TablaFrecuenciaMaxMin.alt(11)
            'ReDim Me._TablaFrecuenciaMaxMin.minNat(11)
            'ReDim Me._TablaFrecuenciaMaxMin.minAlt(11)
            'ReDim Me._TablaFrecuenciaMaxMin.posMaxAlt(11)
            'ReDim Me._TablaFrecuenciaMaxMin.posMaxNat(11)
            'ReDim Me._TablaFrecuenciaMaxMin.posMinNat(11)
            'ReDim Me._TablaFrecuenciaMaxMin.posMinAlt(11)

            ''Dim max As Single = 0
            ''Dim pos As Single = 0

            'For i = 0 To 11
            '    Me._TablaFrecuenciaMaxMin.nat(i) = 0
            '    Me._TablaFrecuenciaMaxMin.alt(i) = 0
            '    Me._TablaFrecuenciaMaxMin.minNat(i) = 0
            '    Me._TablaFrecuenciaMaxMin.minAlt(i) = 0
            '    Me._TablaFrecuenciaMaxMin.posMaxAlt(i) = False
            '    Me._TablaFrecuenciaMaxMin.posMaxNat(i) = False
            '    Me._TablaFrecuenciaMaxMin.posMinNat(i) = False
            '    Me._TablaFrecuenciaMaxMin.posMinAlt(i) = False
            'Next


            'For i = 0 To Me._AportacionNatAnual.año.Length - 1
            '    max = 0
            '    For j = 0 To 11
            '        If (max < Me._AportacionNatMen.aportacion(i * 12 + j)) Then
            '            max = Me._AportacionNatMen.aportacion(i * 12 + j)
            '            pos = j
            '        End If

            '    Next
            '    Me._TablaFrecuenciaMaxMin.nat(pos) = Me._TablaFrecuenciaMaxMin.nat(pos) + 1
            'Next

            'EstMax = 0
            'EstMin = 99999
            'For i = 0 To Me._AportacionAltAnual.año.Length - 1
            '    max = 0
            '    For j = 0 To 11
            '        If (max < Me._AportacionAltMen.aportacion(i * 12 + j)) Then
            '            max = Me._AportacionAltMen.aportacion(i * 12 + j)
            '            pos = j
            '            If (max > EstMax) Then
            '                EstMax = max
            '                sMax = (j + Me._datos.mesInicio - 1) Mod 12
            '            End If
            '        End If
            '        If (Me._AportacionAltMen.aportacion(i * 12 + j) < EstMin) Then
            '            EstMin = Me._AportacionAltMen.aportacion(i * 12 + j)
            '            sMin = (j + Me._datos.mesInicio - 1) Mod 12
            '        End If
            '    Next
            '    Me._TablaFrecuenciaMaxMin.alt(pos) = Me._TablaFrecuenciaMaxMin.alt(pos) + 1
            'Next

            '' Tratamiento de mínimos
            'For i = 0 To Me._AportacionNatAnual.año.Length - 1
            '    min = 999999
            '    For j = 0 To 11
            '        If (min > Me._AportacionNatMen.aportacion(i * 12 + j)) Then
            '            min = Me._AportacionNatMen.aportacion(i * 12 + j)
            '            pos = j
            '        End If
            '    Next
            '    Me._TablaFrecuenciaMaxMin.minNat(pos) = Me._TablaFrecuenciaMaxMin.minNat(pos) + 1

            'Next

            'For i = 0 To Me._AportacionAltAnual.año.Length - 1
            '    min = 999999
            '    For j = 0 To 11
            '        If (min > Me._AportacionAltMen.aportacion(i * 12 + j)) Then
            '            min = Me._AportacionAltMen.aportacion(i * 12 + j)
            '            pos = j
            '        End If
            '    Next
            '    Me._TablaFrecuenciaMaxMin.minAlt(pos) = Me._TablaFrecuenciaMaxMin.minAlt(pos) + 1
            'Next

            'max = 0
            ''min = 9999999
            'min = 0
            'For i = 0 To 11
            '    If (max < Me._TablaFrecuenciaMaxMin.nat(i)) Then
            '        max = Me._TablaFrecuenciaMaxMin.nat(i)
            '    End If
            '    'If (min > Me._TablaFrecuenciaMaxMin.nat(i)) Then
            '    ' min = Me._TablaFrecuenciaMaxMin.nat(i)
            '    'End If
            '    If (min < Me._TablaFrecuenciaMaxMin.minNat(i)) Then
            '        min = Me._TablaFrecuenciaMaxMin.minNat(i)
            '    End If
            'Next
            'For i = 0 To 11
            '    If (max = Me._TablaFrecuenciaMaxMin.nat(i)) Then
            '        Me._TablaFrecuenciaMaxMin.posMaxNat(i) = True
            '    End If
            '    'If (min = Me._TablaFrecuenciaMaxMin.nat(i)) Then
            '    If (min = Me._TablaFrecuenciaMaxMin.minNat(i)) Then
            '        Me._TablaFrecuenciaMaxMin.posMinNat(i) = True
            '    End If
            'Next
            'max = 0
            ''min = 9999999
            'min = 0
            'For i = 0 To 11
            '    If (max < Me._TablaFrecuenciaMaxMin.alt(i)) Then
            '        max = Me._TablaFrecuenciaMaxMin.alt(i)
            '    End If
            '    'If (min > Me._TablaFrecuenciaMaxMin.alt(i)) Then
            '    'min = Me._TablaFrecuenciaMaxMin.alt(i)
            '    'End If
            '    If (min < Me._TablaFrecuenciaMaxMin.minAlt(i)) Then
            '        min = Me._TablaFrecuenciaMaxMin.minAlt(i)
            '    End If
            'Next
            'For i = 0 To 11
            '    If (max = Me._TablaFrecuenciaMaxMin.alt(i)) Then
            '        Me._TablaFrecuenciaMaxMin.posMaxAlt(i) = True
            '    End If
            '    'If (min = Me._TablaFrecuenciaMaxMin.alt(i)) Then
            '    If (min = Me._TablaFrecuenciaMaxMin.minAlt(i)) Then
            '        Me._TablaFrecuenciaMaxMin.posMinAlt(i) = True
            '    End If
            'Next

            'ReDim Me._HabEstacionalidadAlt(0)

            'Me._HabEstacionalidadAlt(0) = sMax.ToString() + "-" + sMin.ToString()
            ''Me._HabEstacionalidadAlt(0) = 0

        End Sub
        ''' <summary>
        ''' Parametros Variabilidad Diaria Habitual
        ''' </summary>
        ''' <remarks>Informe 4</remarks>
        Private Sub CalculoParametrosVariabilidadDIARIAHabitual()
            Dim i As Integer
            ReDim Me._HabVariabilidadDiaraNat(1)

            Me.CalcularTablaCQC(False) ' Esto calcula la Tabla CQC

            ' Buscar el 5%, 10%, 90% y 95%
            i = 0
            While (Me._TablaCQCNat.pe(i) < 10)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._HabVariabilidadDiaraNat(0) = (10 - Me._TablaCQCNat.pe(i - 1)) / (Me._TablaCQCNat.pe(i) - Me._TablaCQCNat.pe(i - 1)) * (Me._TablaCQCNat.añomedio(i) - Me._TablaCQCNat.añomedio(i - 1)) + Me._TablaCQCNat.añomedio(i - 1)

            i = 0
            While (Me._TablaCQCNat.pe(i) < 90)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._HabVariabilidadDiaraNat(1) = (90 - Me._TablaCQCNat.pe(i - 1)) / (Me._TablaCQCNat.pe(i) - Me._TablaCQCNat.pe(i - 1)) * (Me._TablaCQCNat.añomedio(i) - Me._TablaCQCNat.añomedio(i - 1)) + Me._TablaCQCNat.añomedio(i - 1)

        End Sub
        ''' <summary>
        ''' Parametros Variabilidad Diaria Habitual Alterada
        ''' </summary>
        ''' <remarks>Informe 5</remarks>
        Private Sub CalculoParametrosVariabilidadDIARIAHabitualAlterada()
            Dim i As Integer
            ReDim Me._HabVariabilidadDiaraAlt(1)

            Me.CalcularTablaCQC(True) ' Esto calcula la Tabla CQC

            ' Buscar el 5%, 10%, 90% y 95%
            i = 0
            While (Me._TablaCQCAlt.pe(i) < 10)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._HabVariabilidadDiaraAlt(0) = (10 - Me._TablaCQCAlt.pe(i - 1)) / (Me._TablaCQCAlt.pe(i) - Me._TablaCQCAlt.pe(i - 1)) * (Me._TablaCQCAlt.añomedio(i) - Me._TablaCQCAlt.añomedio(i - 1)) + Me._TablaCQCAlt.añomedio(i - 1)

            i = 0
            While (Me._TablaCQCAlt.pe(i) < 90)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._HabVariabilidadDiaraAlt(1) = (90 - Me._TablaCQCAlt.pe(i - 1)) / (Me._TablaCQCAlt.pe(i) - Me._TablaCQCAlt.pe(i - 1)) * (Me._TablaCQCAlt.añomedio(i) - Me._TablaCQCAlt.añomedio(i - 1)) + Me._TablaCQCAlt.añomedio(i - 1)

        End Sub

        ''' <summary>
        ''' Parametros de avenidas 
        ''' </summary>
        ''' <remarks>Informe 4</remarks>
        Public Sub CalculoParametrosAvenidasCASO4()

            Dim nAños As Integer = Me._datos.SerieNatDiaria.nAños
            Dim i, j As Integer

            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++ Sacar la lista Qc +++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' Media de los maximos caudales diarios anuales
            Dim listaMaxDiarios() As Single
            ReDim listaMaxDiarios(nAños - 1)

            Dim añoActual As Integer
            Dim pos As Integer

            pos = 0

            ' Primer año
            añoActual = Me._datos.SerieNatDiaria.dia(0).Year
            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                ' Si el año es diferente cambio donde almacenar el maximo
                If (Me._datos.SerieNatDiaria.dia(i).Day = 1 And Me._datos.SerieNatDiaria.dia(i).Month = Me._datos.mesInicio And Me._datos.SerieNatDiaria.dia(i).Year <> añoActual) Then
                    pos = pos + 1
                    listaMaxDiarios(pos) = 0
                    añoActual = Me._datos.SerieNatDiaria.dia(i).Year
                End If
                If (listaMaxDiarios(pos) < Me._datos.SerieNatDiaria.caudalDiaria(i)) Then
                    listaMaxDiarios(pos) = Me._datos.SerieNatDiaria.caudalDiaria(i)
                End If
            Next

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular la media (Qc)  ++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' Sin problema de no poderse calcular
            ReDim Me._AveMagnitudNat(3)

            Me._AveMagnitudNat(0) = 0

            For i = 0 To nAños - 1
                Me._AveMagnitudNat(0) = Me._AveMagnitudNat(0) + listaMaxDiarios(i)
            Next

            Me._AveMagnitudNat(0) = Me._AveMagnitudNat(0) / nAños

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Generador de Lecho (Qgl) ++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' CON problema de no poderse calcular
            Dim desv As Single
            Dim media As Single
            Dim aux1, aux2 As Single

            media = Me._AveMagnitudNat(0) ' La media es el parametro 1

            If (media = 0) Then
                Me._AveMagnitudNat(1) = -9999
            Else
                aux1 = 0
                aux2 = 0
                For i = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaMaxDiarios(i), 2)
                    aux2 = aux2 + listaMaxDiarios(i)
                Next
                aux2 = Pow(aux2, 2)

                desv = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))

                Me._AveMagnitudNat(1) = Me._AveMagnitudNat(0) * (0.7 + 0.6 * (desv / media))
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Caudal Conectividad (Qconec) +++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' Sin problema de no poderse calcular
            Dim alfa As Single
            Dim mu As Single

            Dim T As Single

            alfa = (Sqrt(6) / PI) * desv
            mu = media - 0.5772 * alfa

            ' Aux1 == F(X)
            aux1 = Exp(-Exp(-(Me._AveMagnitudNat(1) - mu) / alfa))

            T = 1 / (1 - aux1)

            ' Calculo el nuevo periodo de retorno
            T = 2 * T
            ' Para calculos posteriores
            Me._Ave2TNat = T

            aux1 = 1 - 1 / T

            Me._AveMagnitudNat(2) = mu - alfa * Log(-Log(aux1))


            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Avenida Habitual (Q5%) ++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' Sin problema de no poderse calcular
            Me.CalcularTablaCQC(False) ' Esto calcula la Tabla CQC

            ' Buscar el 5%, 10%, 90% y 95%
            i = 0
            While (Me._TablaCQCNat.pe(i) < 5)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._AveMagnitudNat(3) = (5 - Me._TablaCQCNat.pe(i - 1)) / (Me._TablaCQCNat.pe(i) - Me._TablaCQCNat.pe(i - 1)) * (Me._TablaCQCNat.añomedio(i) - Me._TablaCQCNat.añomedio(i - 1)) + Me._TablaCQCNat.añomedio(i - 1)


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Coe de variacion de los maximos CV(Qc) ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' CON problema de no poderse calcular
            ReDim Me._AveVariabilidadNat(1)

            Dim mediaMax As Single = 0
            Dim desvMax As Single = 0

            For i = 0 To nAños - 1
                mediaMax = mediaMax + listaMaxDiarios(i)
            Next
            mediaMax = mediaMax / nAños

            If (mediaMax = 0) Then
                Me._AveVariabilidadNat(0) = 0
            Else
                aux1 = 0
                aux2 = 0
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaMaxDiarios(j), 2)
                    aux2 = aux2 + listaMaxDiarios(j)
                Next
                aux2 = Pow(aux2, 2)

                desvMax = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))

                Me._AveVariabilidadNat(0) = desvMax / mediaMax
            End If



            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Coe de variacion de la serie CV(Q5) ++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            Dim listaQ5() As Single
            ReDim listaQ5(nAños - 1)

            i = 0
            While (Me._TablaCQCNat.pe(i) < 5)
                i = i + 1
            End While

            For j = 0 To nAños - 1
                listaQ5(j) = (5 - Me._TablaCQCNat.pe(i - 1)) / (Me._TablaCQCNat.pe(i) - Me._TablaCQCNat.pe(i - 1)) * (Me._TablaCQCNat.caudales(j)(i) - Me._TablaCQCNat.caudales(j)(i - 1)) + Me._TablaCQCNat.caudales(j)(i - 1)
            Next

            Dim mediaQ5 As Single
            For j = 0 To nAños - 1
                mediaQ5 = mediaQ5 + listaQ5(j)
            Next

            mediaQ5 = mediaQ5 / nAños

            ' ===== ¿SE PUEDE CALCULAR? =====
            If (mediaQ5 = 0) Then
                Me._AveVariabilidadNat(1) = 0
            Else
                Dim desvQ5 As Single

                aux1 = 0
                aux2 = 0
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaQ5(j), 2)
                    aux2 = aux2 + listaQ5(j)
                Next
                aux2 = Pow(aux2, 2)

                desvQ5 = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))

                Me._AveVariabilidadNat(1) = desvQ5 / mediaQ5
            End If



            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calculo de la duracion        +++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Dim diasSeguidosMaxQ5(nAños - 1) As Integer

            Dim ordEnAños()() As Single
            ReDim ordEnAños(nAños - 1)

            Dim acum As Integer

            ' Relleno los caudales
            acum = 0
            For i = 0 To nAños - 1
                Dim posibleBisiesto As Integer
                If (Me._datos.mesInicio > 2) Then
                    posibleBisiesto = Me._datos.SerieNatDiaria.dia(acum).Year + 1
                Else
                    posibleBisiesto = Me._datos.SerieNatDiaria.dia(acum).Year
                End If

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    ReDim Preserve ordEnAños(i)(365)
                Else
                    ReDim Preserve ordEnAños(i)(364)
                End If

                For j = 0 To ordEnAños(i).Length - 1
                    ordEnAños(i)(j) = Me._datos.SerieNatDiaria.caudalDiaria(acum + j)
                Next

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    acum = acum + 366
                Else
                    acum = acum + 365
                End If
            Next


            Dim seguido As Boolean
            Dim nSeguido As Integer

            For i = 0 To nAños - 1
                seguido = False
                nSeguido = 0
                diasSeguidosMaxQ5(i) = 0
                For j = 0 To 364
                    ' Si el caudal del dia es mayor que Q5%
                    If (ordEnAños(i)(j) > Me._AveMagnitudNat(3)) Then
                        nSeguido = nSeguido + 1
                        If (nSeguido > diasSeguidosMaxQ5(i)) Then
                            diasSeguidosMaxQ5(i) = nSeguido
                        End If
                        'nSeguido = nSeguido + 1
                    Else
                        nSeguido = 0
                    End If
                Next
            Next

            Me._AveDuracionNat = 0
            For i = 0 To nAños - 1
                Me._AveDuracionNat = Me._AveDuracionNat + diasSeguidosMaxQ5(i)
            Next
            Me._AveDuracionNat = Me._AveDuracionNat / nAños


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._AveEstacionalidadNat.ndias(11)
            ReDim Me._AveEstacionalidadNat.mes(11)

            Dim max As Integer
            Dim mesAct As Integer
            Dim posArray As Integer

            For i = 0 To 11
                Me._AveEstacionalidadNat.ndias(i) = 0
                Me._AveEstacionalidadNat.mes(i) = (i + Me._datos.mesInicio) Mod 12
                If (Me._AveEstacionalidadNat.mes(i) = 0) Then
                    Me._AveEstacionalidadNat.mes(i) = 12
                End If
            Next

            mesAct = Me._datos.SerieNatDiaria.dia(0).Month
            max = 0
            posArray = 0
            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                If (mesAct <> Me._datos.SerieNatDiaria.dia(i).Month) Then
                    Me._AveEstacionalidadNat.ndias(posArray) = Me._AveEstacionalidadNat.ndias(posArray) + max
                    mesAct = Me._datos.SerieNatDiaria.dia(i).Month
                    max = 0
                    posArray = posArray + 1
                    If (posArray = 12) Then
                        posArray = 0
                    End If
                End If
                If (Me._datos.SerieNatDiaria.caudalDiaria(i) > Me._AveMagnitudNat(3)) Then  ' AveMagNat(3) = Q5
                    max = max + 1
                End If
            Next

            For i = 0 To 11
                Me._AveEstacionalidadNat.ndias(i) = Me._AveEstacionalidadNat.ndias(i) / nAños
            Next

            Me._AveEstacionalidadNat.mediaAño = 0
            For i = 0 To 11
                Me._AveEstacionalidadNat.mediaAño = Me._AveEstacionalidadNat.mediaAño + Me._AveEstacionalidadNat.ndias(i)
            Next
            Me._AveEstacionalidadNat.mediaAño = Me._AveEstacionalidadNat.mediaAño / 12

        End Sub
        ''' <summary>
        ''' Parametros de sequias
        ''' </summary>
        ''' <remarks>Informe 4</remarks>
        Private Sub CalculoParametrosSequiasCASO4()
            Dim nAños As Integer = Me._datos.SerieNatDiaria.nAños
            Dim i, j As Integer

            ReDim Me._SeqMagnitudNat(1)

            ' Calcular Qs
            Dim listaMinDiarios() As Single

            ReDim listaMinDiarios(nAños - 1)

            Dim añoActual As Integer
            Dim pos As Integer

            pos = 0

            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++ Sacar la lista Qs +++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' Media de los minimos caudales diarios anuales
            listaMinDiarios(0) = 999999999
            añoActual = Me._datos.SerieNatDiaria.dia(0).Year
            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                ' Si el año es diferente cambio donde almacenar el maximo
                If (Me._datos.SerieNatDiaria.dia(i).Day = 1 And Me._datos.SerieNatDiaria.dia(i).Month = Me._datos.mesInicio And Me._datos.SerieNatDiaria.dia(i).Year <> añoActual) Then
                    pos = pos + 1
                    listaMinDiarios(pos) = 999999999
                    añoActual = Me._datos.SerieNatDiaria.dia(i).Year
                End If
                If (listaMinDiarios(pos) > Me._datos.SerieNatDiaria.caudalDiaria(i)) Then
                    listaMinDiarios(pos) = Me._datos.SerieNatDiaria.caudalDiaria(i)
                End If
            Next
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular la media (Qs)  ++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            Me._SeqMagnitudNat(0) = 0

            For i = 0 To nAños - 1
                Me._SeqMagnitudNat(0) = Me._SeqMagnitudNat(0) + listaMinDiarios(i)
            Next

            Me._SeqMagnitudNat(0) = Me._SeqMagnitudNat(0) / nAños

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Sequia Habitual (Q95%) ++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            If (Me._TablaCQCNat.caudales Is Nothing) Then
                Me.CalcularTablaCQC(False) ' Esto calcula la Tabla CQC
            End If

            ' Buscar el 5%, 10%, 90% y 95%
            i = 0
            While (Me._TablaCQCNat.pe(i) < 95)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._SeqMagnitudNat(1) = (95 - Me._TablaCQCNat.pe(i - 1)) / (Me._TablaCQCNat.pe(i) - Me._TablaCQCNat.pe(i - 1)) * (Me._TablaCQCNat.añomedio(i) - Me._TablaCQCNat.añomedio(i - 1)) + Me._TablaCQCNat.añomedio(i - 1)


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Coe de variacion de los minimos CV(Qs) ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._SeqVariabilidadNat(1)

            Dim mediaMin As Single = 0
            Dim desvMin As Single = 0
            Dim aux1 As Single = 0
            Dim aux2 As Single = 0

            For i = 0 To nAños - 1
                mediaMin = mediaMin + listaMinDiarios(i)
            Next
            mediaMin = mediaMin / nAños
            ' Si no se puede calcular lo marco
            If (mediaMin = 0) Then
                Me._SeqVariabilidadNat(0) = 0
                ' Cambio realizado por Eduardo 21/11/07
                ' -------------------------------------
                ' Me._SeqVariabilidadNat(0) = -9999
            Else
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaMinDiarios(j), 2)
                    aux2 = aux2 + listaMinDiarios(j)
                Next
                aux2 = Pow(aux2, 2)

                desvMin = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))
                Me._SeqVariabilidadNat(0) = desvMin / mediaMin
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Coe de variacion de la serie CV(Q95) +++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            Dim listaQ95() As Single
            ReDim listaQ95(nAños - 1)

            i = 0
            While (Me._TablaCQCNat.pe(i) < 95)
                i = i + 1
            End While

            For j = 0 To nAños - 1
                listaQ95(j) = (95 - Me._TablaCQCNat.pe(i - 1)) / (Me._TablaCQCNat.pe(i) - Me._TablaCQCNat.pe(i - 1)) * (Me._TablaCQCNat.caudales(j)(i) - Me._TablaCQCNat.caudales(j)(i - 1)) + Me._TablaCQCNat.caudales(j)(i - 1)
            Next


            Dim mediaQ95 As Single
            For j = 0 To nAños - 1
                mediaQ95 = mediaQ95 + listaQ95(j)
            Next
            mediaQ95 = mediaQ95 / nAños

            If (mediaQ95 = 0) Then
                Me._SeqVariabilidadNat(1) = 0
            Else
                Dim desvQ95 As Single
                aux1 = 0
                aux2 = 0
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaQ95(j), 2)
                    aux2 = aux2 + listaQ95(j)
                Next
                aux2 = Pow(aux2, 2)

                desvQ95 = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))
                Me._SeqVariabilidadNat(1) = desvQ95 / mediaQ95
            End If


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            '
            ' INFORME 4
            ' Valores extremos > Caudales minimos (sequias) > Estacionalidad
            '
            ReDim Me._SeqEstacionalidadNat.ndias(11)
            ReDim Me._SeqEstacionalidadNat.mes(11)

            Dim max As Integer
            Dim mesAct As Integer
            Dim posArray As Integer

            For i = 0 To 11
                Me._SeqEstacionalidadNat.ndias(i) = 0
                Me._SeqEstacionalidadNat.mes(i) = (i + Me._datos.mesInicio) Mod 12
                If (Me._SeqEstacionalidadNat.mes(i) = 0) Then
                    Me._SeqEstacionalidadNat.mes(i) = 12
                End If
            Next

            mesAct = Me._datos.SerieNatDiaria.dia(0).Month
            max = 0
            posArray = 0
            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                If (mesAct <> Me._datos.SerieNatDiaria.dia(i).Month) Then
                    Me._SeqEstacionalidadNat.ndias(posArray) = Me._SeqEstacionalidadNat.ndias(posArray) + max
                    mesAct = Me._datos.SerieNatDiaria.dia(i).Month
                    max = 0
                    posArray = posArray + 1
                    If (posArray = 12) Then
                        posArray = 0
                    End If
                End If
                ' 
                ' ERROR DOC 27/08/09 - CA XXX
                ' -- Fallo en caso 5: Guadiana
                ' ----------------------------
                'If (Me._datos.SerieNatDiaria.caudalDiaria(i) < Me._SeqMagnitudNat(1)) Then
                If (Me._datos.SerieNatDiaria.caudalDiaria(i) <= Me._SeqMagnitudNat(1)) Then
                    max = max + 1
                End If
            Next

            For i = 0 To 11
                Me._SeqEstacionalidadNat.ndias(i) = Me._SeqEstacionalidadNat.ndias(i) / nAños
            Next

            Me._SeqEstacionalidadNat.mediaAño = 0
            For i = 0 To 11
                Me._SeqEstacionalidadNat.mediaAño = Me._SeqEstacionalidadNat.mediaAño + Me._SeqEstacionalidadNat.ndias(i)
            Next
            Me._SeqEstacionalidadNat.mediaAño = Me._SeqEstacionalidadNat.mediaAño / 12

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calculo de la duracion        +++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            '
            ' INFORME 4
            ' Valores extremos > Caudales minimos (sequias) > Duración
            '
            ReDim Me._SeqDuracionNat(1)

            Dim diasSeguidosMinQ95(nAños - 1) As Integer
            Dim ordEnAños()() As Single
            ReDim ordEnAños(nAños - 1)

            Dim acum As Integer

            ' Relleno los caudales
            acum = 0
            For i = 0 To nAños - 1
                Dim posibleBisiesto As Integer
                If (Me._datos.mesInicio > 2) Then
                    posibleBisiesto = Me._datos.SerieNatDiaria.dia(acum).Year + 1
                Else
                    posibleBisiesto = Me._datos.SerieNatDiaria.dia(acum).Year
                End If

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    ReDim Preserve ordEnAños(i)(365)
                Else
                    ReDim Preserve ordEnAños(i)(364)
                End If
                For j = 0 To ordEnAños(i).Length - 1
                    ordEnAños(i)(j) = Me._datos.SerieNatDiaria.caudalDiaria(acum + j)
                Next
                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    acum = acum + 366
                Else
                    acum = acum + 365
                End If
            Next

            Dim seguido As Boolean
            Dim nSeguido As Integer

            For i = 0 To nAños - 1
                seguido = False
                nSeguido = 0
                diasSeguidosMinQ95(i) = 0
                For j = 0 To 364
                    ' Si el caudal del dia es mayor que Q5%
                    ' 
                    ' ERROR DOC 27/08/09 - CA XXX
                    ' -- Fallo en caso 5: Guadiana
                    ' ----------------------------
                    'If (ordEnAños(i)(j) < Me._SeqMagnitudNat(1)) Then
                    If (ordEnAños(i)(j) <= Me._SeqMagnitudNat(1)) Then
                        nSeguido = nSeguido + 1
                        If (nSeguido > diasSeguidosMinQ95(i)) Then
                            diasSeguidosMinQ95(i) = nSeguido
                        End If
                        'nSeguido = nSeguido + 1
                    Else
                        nSeguido = 0
                    End If
                Next
            Next

            Me._SeqDuracionNat(0) = 0
            For i = 0 To nAños - 1
                Me._SeqDuracionNat(0) = Me._SeqDuracionNat(0) + diasSeguidosMinQ95(i)
            Next
            Me._SeqDuracionNat(0) = Me._SeqDuracionNat(0) / nAños

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++++ Calculo de la Duracion (dias a nulo) +++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Dim acumNulos As Integer = 0
            'Dim acumAUX As Integer = 0
            'ReDim Me._nDiasNulosNat(nAños)

            acum = 0
            For i = 0 To nAños - 1
                Dim posibleBisiesto As Integer
                If (Me._datos.mesInicio > 2) Then
                    posibleBisiesto = Me._datos.SerieNatDiaria.dia(acum).Year + 1
                Else
                    posibleBisiesto = Me._datos.SerieNatDiaria.dia(acum).Year
                End If
                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    For j = 0 To 365
                        If (Me._datos.SerieNatDiaria.caudalDiaria(acum + j) = 0) Then
                            acumNulos = acumNulos + 1
                        End If
                    Next
                    acum = acum + 366
                Else
                    For j = 0 To 364
                        If (Me._datos.SerieNatDiaria.caudalDiaria(acum + j) = 0) Then
                            acumNulos = acumNulos + 1
                        End If
                    Next
                    ' ¿Posible error?
                    'acum = acum + 366
                    acum = acum + 365
                End If
                ' Para calculos posteriores
                'Me._nDiasNulosNat(i) = acumNulos
                'acumAUX = 0
            Next
            Me._SeqDuracionNat(1) = acumNulos / nAños

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++ Calculo por MES de los dias nulos +++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._SeqDuracionCerosMesNat.ndias(11)
            ReDim Me._SeqDuracionCerosMesNat.mes(11)


            For i = 0 To 11
                Me._SeqDuracionCerosMesNat.ndias(i) = 0
                Me._SeqDuracionCerosMesNat.mes(i) = (i + Me._datos.mesInicio) Mod 12
                If (Me._SeqDuracionCerosMesNat.mes(i) = 0) Then
                    Me._SeqDuracionCerosMesNat.mes(i) = 12
                End If
            Next

            mesAct = Me._datos.SerieNatDiaria.dia(0).Month
            posArray = 0

            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                If (mesAct <> Me._datos.SerieNatDiaria.dia(i).Month) Then
                    mesAct = Me._datos.SerieNatDiaria.dia(i).Month
                    posArray = posArray + 1
                    If (posArray = 12) Then
                        posArray = 0
                    End If
                End If
                If (Me._datos.SerieNatDiaria.caudalDiaria(i) = 0) Then
                    Me._SeqDuracionCerosMesNat.ndias(posArray) = Me._SeqDuracionCerosMesNat.ndias(posArray) + 1
                End If
            Next

            For i = 0 To 11
                Me._SeqDuracionCerosMesNat.ndias(i) = Me._SeqDuracionCerosMesNat.ndias(i) / nAños
            Next
        End Sub

        ''' <summary>
        ''' Parametros Avenidas Alterada
        ''' </summary>
        ''' <remarks>Informe 5</remarks>
        Private Sub CalculoParametrosAvenidasAlteradosCASO6()

            Dim nAños As Integer = Me._datos.SerieAltDiaria.nAños
            Dim i, j As Integer

            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++ Sacar la lista Qc +++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++

            Dim listaMaxDiarios() As Single
            ReDim listaMaxDiarios(nAños - 1)

            Dim añoActual As Integer
            Dim pos As Integer

            pos = 0

            ' Primer año
            añoActual = Me._datos.SerieAltDiaria.dia(0).Year
            For i = 0 To Me._datos.SerieAltDiaria.dia.Length - 1
                ' Si el año es diferente cambio donde almacenar el maximo
                If (Me._datos.SerieAltDiaria.dia(i).Day = 1 And _
                    Me._datos.SerieAltDiaria.dia(i).Month = Me._datos.mesInicio And _
                    Me._datos.SerieAltDiaria.dia(i).Year <> añoActual) Then

                    pos = pos + 1
                    listaMaxDiarios(pos) = 0
                    añoActual = Me._datos.SerieAltDiaria.dia(i).Year
                End If
                If (listaMaxDiarios(pos) < Me._datos.SerieAltDiaria.caudalDiaria(i)) Then
                    listaMaxDiarios(pos) = Me._datos.SerieAltDiaria.caudalDiaria(i)
                End If
            Next

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular la media (Qc)  ++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++

            ReDim Me._AveMagnitudAlt(3)
            Me._AveMagnitudAlt(0) = 0

            For i = 0 To nAños - 1
                Me._AveMagnitudAlt(0) = Me._AveMagnitudAlt(0) + listaMaxDiarios(i)
            Next
            Me._AveMagnitudAlt(0) = Me._AveMagnitudAlt(0) / nAños

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Generador de Lecho (Qgl) ++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            Dim desv As Single
            Dim media As Single
            Dim aux1, aux2 As Single

            media = Me._AveMagnitudAlt(0) ' La media es el parametro 1

            ' ======= Puede que no se pueda calcular ======
            If (media = 0) Then
                Me._AveMagnitudAlt(1) = -9999
            Else
                aux1 = 0
                aux2 = 0
                For i = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaMaxDiarios(i), 2)
                    aux2 = aux2 + listaMaxDiarios(i)
                Next
                aux2 = Pow(aux2, 2)

                desv = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))

                Me._AveMagnitudAlt(1) = Me._AveMagnitudAlt(0) * (0.7 + 0.6 * (desv / media))
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Caudal Conectividad (Qconec) +++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++

            Dim alfa As Single
            Dim mu As Single

            Dim T As Single

            alfa = (Sqrt(6) / PI) * desv
            mu = media - 0.5772 * alfa

            ' Saco F(X)
            aux1 = Exp(-Exp(-(Me._AveMagnitudAlt(1) - mu) / alfa))

            T = 1 / (1 - aux1)
            ' Calculo el nuevo periodo de retorno
            T = 2 * T

            ' Para calculos posteriores
            Me._Ave2TAlt = T

            aux1 = 1 - 1 / T

            Me._AveMagnitudAlt(2) = mu - alfa * Math.Log(-Math.Log(aux1))

            ' Este cambio viene para el IAH9 que ha cambiado
            ' En este caso se guarda T[Qconec ALT]GUMBEL ALT
            ' pero ahora se hace     T[Qconec NAT]CUMBEL ALT
            ' Saco F(X)
            aux1 = Exp(-Exp(-(Me._AveMagnitudNat(2) - mu) / alfa))
            T = 1 / (1 - aux1)
            ' Calculo el nuevo periodo de retorno
            'T = 2 * T
            ' Para calculos posteriores
            Me._Ave2TAlt = T


            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Avenida Habitual (Q5%) ++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++

            Me.CalcularTablaCQC(True) ' Esto calcula la Tabla CQC

            ' Buscar el 5%, 10%, 90% y 95%
            i = 0
            While (Me._TablaCQCAlt.pe(i) < 5)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._AveMagnitudAlt(3) = (5 - Me._TablaCQCAlt.pe(i - 1)) / (Me._TablaCQCAlt.pe(i) - Me._TablaCQCAlt.pe(i - 1)) * (Me._TablaCQCAlt.añomedio(i) - Me._TablaCQCAlt.añomedio(i - 1)) + Me._TablaCQCAlt.añomedio(i - 1)


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Coe de variacion de los maximos CV(Qc) ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._AveVariabilidadAlt(1)

            Dim mediaMax As Single = 0
            Dim desvMax As Single = 0

            For i = 0 To nAños - 1
                mediaMax = mediaMax + listaMaxDiarios(i)
            Next
            mediaMax = mediaMax / nAños

            If (mediaMax = 0) Then
                Me._AveVariabilidadAlt(0) = 0
            Else
                aux1 = 0
                aux2 = 0
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaMaxDiarios(j), 2)
                    aux2 = aux2 + listaMaxDiarios(j)
                Next
                aux2 = Pow(aux2, 2)


                desvMax = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))

                Me._AveVariabilidadAlt(0) = desvMax / mediaMax
            End If



            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Coe de variacion de la serie CV(Q5) ++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            Dim listaQ5() As Single
            ReDim listaQ5(nAños - 1)

            i = 0
            While (Me._TablaCQCAlt.pe(i) < 5)
                i = i + 1
            End While

            For j = 0 To nAños - 1
                listaQ5(j) = (5 - Me._TablaCQCAlt.pe(i - 1)) / (Me._TablaCQCAlt.pe(i) - Me._TablaCQCAlt.pe(i - 1)) * (Me._TablaCQCAlt.caudales(j)(i) - Me._TablaCQCAlt.caudales(j)(i - 1)) + Me._TablaCQCAlt.caudales(j)(i - 1)
            Next


            Dim mediaQ5 As Single
            For j = 0 To nAños - 1
                mediaQ5 = mediaQ5 + listaQ5(j)
            Next

            mediaQ5 = mediaQ5 / nAños

            If (mediaQ5 = 0) Then
                Me._AveVariabilidadAlt(1) = 0
            Else
                Dim desvQ5 As Single

                aux1 = 0
                aux2 = 0
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaQ5(j), 2)
                    aux2 = aux2 + listaQ5(j)
                Next
                aux2 = Pow(aux2, 2)


                desvQ5 = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))

                Me._AveVariabilidadAlt(1) = desvQ5 / mediaQ5
            End If



            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calculo de la duracion        +++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Dim diasSeguidosMaxQ5(nAños - 1) As Integer

            Dim ordEnAños()() As Single
            ReDim ordEnAños(nAños - 1)

            Dim acum As Integer

            ' Relleno los caudales
            acum = 0
            For i = 0 To nAños - 1
                Dim posibleBisiesto As Integer
                If (Me._datos.mesInicio > 2) Then
                    posibleBisiesto = Me._datos.SerieAltDiaria.dia(acum).Year + 1
                Else
                    posibleBisiesto = Me._datos.SerieAltDiaria.dia(acum).Year
                End If

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    ReDim Preserve ordEnAños(i)(365)
                Else
                    ReDim Preserve ordEnAños(i)(364)
                End If

                For j = 0 To ordEnAños(i).Length - 1
                    ordEnAños(i)(j) = Me._datos.SerieAltDiaria.caudalDiaria(acum + j)
                Next

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    acum = acum + 366
                Else
                    acum = acum + 365
                End If
            Next


            Dim seguido As Boolean
            Dim nSeguido As Integer

            For i = 0 To nAños - 1
                seguido = False
                nSeguido = 0
                diasSeguidosMaxQ5(i) = 0
                For j = 0 To 364
                    ' Si el caudal del dia es mayor que Q5%
                    ' OJO QUE ES LA NATURAL
                    If (ordEnAños(i)(j) > Me._AveMagnitudNat(3)) Then
                        nSeguido = nSeguido + 1
                        If (nSeguido > diasSeguidosMaxQ5(i)) Then
                            diasSeguidosMaxQ5(i) = nSeguido
                        End If
                        'nSeguido = nSeguido + 1
                    Else
                        nSeguido = 0
                    End If
                Next
            Next

            Me._AveDuracionAlt = 0
            For i = 0 To nAños - 1
                Me._AveDuracionAlt = Me._AveDuracionAlt + diasSeguidosMaxQ5(i)
            Next
            Me._AveDuracionAlt = Me._AveDuracionAlt / nAños


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' 
            ' ATENCION: El Q5 se usa el de las series NATURALES
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            '
            ReDim Me._AveEstacionalidadAlt.ndias(11)
            ReDim Me._AveEstacionalidadAlt.mes(11)

            Dim max As Integer
            Dim mesAct As Integer
            Dim posArray As Integer

            For i = 0 To 11
                Me._AveEstacionalidadAlt.ndias(i) = 0
                Me._AveEstacionalidadAlt.mes(i) = (i + Me._datos.mesInicio) Mod 12
                If (Me._AveEstacionalidadAlt.mes(i) = 0) Then
                    Me._AveEstacionalidadAlt.mes(i) = 12
                End If
            Next

            mesAct = Me._datos.SerieAltDiaria.dia(0).Month
            max = 0
            posArray = 0
            For i = 0 To Me._datos.SerieAltDiaria.dia.Length - 1
                If (mesAct <> Me._datos.SerieAltDiaria.dia(i).Month) Then
                    Me._AveEstacionalidadAlt.ndias(posArray) = Me._AveEstacionalidadAlt.ndias(posArray) + max
                    mesAct = Me._datos.SerieAltDiaria.dia(i).Month
                    max = 0
                    posArray = posArray + 1
                    If (posArray = 12) Then
                        posArray = 0
                    End If
                End If
                ' OJO -> Aqui va el cambio: Mira la Q5 Natural
                If (Me._datos.SerieAltDiaria.caudalDiaria(i) > Me._AveMagnitudNat(3)) Then
                    max = max + 1
                End If
            Next

            For i = 0 To 11
                Me._AveEstacionalidadAlt.ndias(i) = Me._AveEstacionalidadAlt.ndias(i) / nAños
            Next

            Me._AveEstacionalidadAlt.mediaAño = 0
            For i = 0 To 11
                Me._AveEstacionalidadAlt.mediaAño = Me._AveEstacionalidadAlt.mediaAño + Me._AveEstacionalidadAlt.ndias(i)
            Next
            Me._AveEstacionalidadAlt.mediaAño = Me._AveEstacionalidadAlt.mediaAño / 12

        End Sub
        ''' <summary>
        ''' Parametros Sequias Alterada
        ''' </summary>
        ''' <remarks>Informe 5</remarks>
        Private Sub CalculoParametrosSequiasAlteradosCASO6()
            Dim nAños As Integer = Me._datos.SerieAltDiaria.nAños
            Dim i, j As Integer

            ReDim Me._SeqMagnitudAlt(1)

            ' Calcular Qc
            Dim listaMinDiarios() As Single

            ReDim listaMinDiarios(nAños - 1)

            Dim añoActual As Integer
            Dim pos As Integer

            pos = 0

            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++ Sacar la lista Qs +++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ' Primer año
            listaMinDiarios(0) = 999999999
            añoActual = Me._datos.SerieAltDiaria.dia(0).Year
            For i = 0 To Me._datos.SerieAltDiaria.dia.Length - 1
                ' Si el año es diferente cambio donde almacenar el maximo
                If (Me._datos.SerieAltDiaria.dia(i).Day = 1 And Me._datos.SerieAltDiaria.dia(i).Month = Me._datos.mesInicio And Me._datos.SerieAltDiaria.dia(i).Year <> añoActual) Then
                    pos = pos + 1
                    listaMinDiarios(pos) = 999999999
                    añoActual = Me._datos.SerieAltDiaria.dia(i).Year
                End If
                If (listaMinDiarios(pos) > Me._datos.SerieAltDiaria.caudalDiaria(i)) Then
                    listaMinDiarios(pos) = Me._datos.SerieAltDiaria.caudalDiaria(i)
                End If
            Next
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular la media (Qs)  ++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            Me._SeqMagnitudAlt(0) = 0

            For i = 0 To nAños - 1
                Me._SeqMagnitudAlt(0) = Me._SeqMagnitudAlt(0) + listaMinDiarios(i)
            Next
            Me._SeqMagnitudAlt(0) = Me._SeqMagnitudAlt(0) / nAños

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Sequia Habitual (Q95%) ++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++
            If (Me._TablaCQCAlt.caudales Is Nothing) Then
                Me.CalcularTablaCQC(True) ' Esto calcula la Tabla CQC
            End If

            ' Buscar el 5%, 10%, 90% y 95%
            i = 0
            While (Me._TablaCQCAlt.pe(i) < 95)
                i = i + 1
            End While
            ' Interpolar el valor
            Me._SeqMagnitudAlt(1) = (95 - Me._TablaCQCAlt.pe(i - 1)) / (Me._TablaCQCAlt.pe(i) - Me._TablaCQCAlt.pe(i - 1)) * (Me._TablaCQCAlt.añomedio(i) - Me._TablaCQCAlt.añomedio(i - 1)) + Me._TablaCQCAlt.añomedio(i - 1)


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Coe de variacion de los minimos CV(Qs) ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._SeqVariabilidadAlt(1)

            Dim mediaMin As Single = 0
            Dim desvMin As Single = 0
            Dim aux1 As Single = 0
            Dim aux2 As Single = 0

            For i = 0 To nAños - 1
                mediaMin = mediaMin + listaMinDiarios(i)
            Next
            mediaMin = mediaMin / nAños
            ' Si no se puede calcular lo marco
            If (mediaMin = 0) Then
                Me._SeqVariabilidadAlt(0) = 0
            Else
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaMinDiarios(j), 2)
                    aux2 = aux2 + listaMinDiarios(j)
                Next
                aux2 = Pow(aux2, 2)

                desvMin = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))
                Me._SeqVariabilidadAlt(0) = desvMin / mediaMin
            End If


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calcular Coe de variacion de la serie CV(Q95) +++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++
            Dim listaQ95() As Single
            ReDim listaQ95(nAños - 1)

            i = 0
            While (Me._TablaCQCAlt.pe(i) < 95)
                i = i + 1
            End While

            For j = 0 To nAños - 1
                listaQ95(j) = (95 - Me._TablaCQCAlt.pe(i - 1)) / (Me._TablaCQCAlt.pe(i) - Me._TablaCQCAlt.pe(i - 1)) * (Me._TablaCQCAlt.caudales(j)(i) - Me._TablaCQCAlt.caudales(j)(i - 1)) + Me._TablaCQCAlt.caudales(j)(i - 1)
            Next


            Dim mediaQ95 As Single
            For j = 0 To nAños - 1
                mediaQ95 = mediaQ95 + listaQ95(j)
            Next
            mediaQ95 = mediaQ95 / nAños

            If (mediaQ95 = 0) Then
                Me._SeqVariabilidadAlt(1) = 0
            Else
                Dim desvQ95 As Single
                aux1 = 0
                aux2 = 0
                For j = 0 To nAños - 1
                    aux1 = aux1 + Pow(listaQ95(j), 2)
                    aux2 = aux2 + listaQ95(j)
                Next
                aux2 = Pow(aux2, 2)

                desvQ95 = Sqrt((nAños * aux1 - aux2) / (nAños * (nAños - 1)))
                Me._SeqVariabilidadAlt(1) = desvQ95 / mediaQ95
            End If


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++ Calcular Estacionalidad +++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' INFORME 5 y 5c
            ' Valores extremos > Caudales minimos (sequias) > Estacionalidad
            '
            ' OJO: Cambio el Q95 por el de NATURAL
            '++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            '
            ReDim Me._SeqEstacionalidadAlt.ndias(11)
            ReDim Me._SeqEstacionalidadAlt.mes(11)

            Dim max As Integer
            Dim mesAct As Integer
            Dim posArray As Integer

            For i = 0 To 11
                Me._SeqEstacionalidadAlt.ndias(i) = 0
                Me._SeqEstacionalidadAlt.mes(i) = (i + Me._datos.mesInicio) Mod 12
                If (Me._SeqEstacionalidadAlt.mes(i) = 0) Then
                    Me._SeqEstacionalidadAlt.mes(i) = 12
                End If
            Next

            mesAct = Me._datos.SerieAltDiaria.dia(0).Month
            max = 0
            posArray = 0
            For i = 0 To Me._datos.SerieAltDiaria.dia.Length - 1
                If (mesAct <> Me._datos.SerieAltDiaria.dia(i).Month) Then
                    Me._SeqEstacionalidadAlt.ndias(posArray) = Me._SeqEstacionalidadAlt.ndias(posArray) + max
                    mesAct = Me._datos.SerieAltDiaria.dia(i).Month
                    max = 0
                    posArray = posArray + 1
                    If (posArray = 12) Then
                        posArray = 0
                    End If
                End If
                ' OJO: Se usa el Q95 NATURAL
                ' ---------------------------
                ' 
                ' ERROR DOC 27/08/09 - CA XXX
                ' -- Fallo en caso 5: Guadiana
                ' ----------------------------
                ' If (Me._datos.SerieAltDiaria.caudalDiaria(i) < Me._SeqMagnitudNat(1)) Then
                If (Me._datos.SerieAltDiaria.caudalDiaria(i) <= Me._SeqMagnitudNat(1)) Then
                    max = max + 1
                End If
            Next

            For i = 0 To 11
                Me._SeqEstacionalidadAlt.ndias(i) = Me._SeqEstacionalidadAlt.ndias(i) / nAños
            Next

            Me._SeqEstacionalidadAlt.mediaAño = 0
            For i = 0 To 11
                Me._SeqEstacionalidadAlt.mediaAño = Me._SeqEstacionalidadAlt.mediaAño + Me._SeqEstacionalidadAlt.ndias(i)
            Next
            Me._SeqEstacionalidadAlt.mediaAño = Me._SeqEstacionalidadAlt.mediaAño / 12

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Calculo de la duracion        +++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            '
            ' Informe 5 y 5c
            ' Valores extremos > Caudales minimos (sequias) > Duracion > "máximo dias consecutivos < Q95%"
            '
            ReDim Me._SeqDuracionAlt(1)

            Dim diasSeguidosMinQ95(nAños - 1) As Integer
            Dim ordEnAños()() As Single
            ReDim ordEnAños(nAños - 1)

            Dim acum As Integer

            ' Relleno los caudales
            acum = 0
            For i = 0 To nAños - 1
                Dim posibleBisiesto As Integer
                If (Me._datos.mesInicio > 2) Then
                    posibleBisiesto = Me._datos.SerieAltDiaria.dia(acum).Year + 1
                Else
                    posibleBisiesto = Me._datos.SerieAltDiaria.dia(acum).Year
                End If

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    ReDim Preserve ordEnAños(i)(365)
                Else
                    ReDim Preserve ordEnAños(i)(364)
                End If
                For j = 0 To ordEnAños(i).Length - 1
                    ordEnAños(i)(j) = Me._datos.SerieAltDiaria.caudalDiaria(acum + j)
                Next
                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    acum = acum + 366
                Else
                    acum = acum + 365
                End If
            Next

            Dim seguido As Boolean
            Dim nSeguido As Integer

            For i = 0 To nAños - 1
                seguido = False
                nSeguido = 0
                diasSeguidosMinQ95(i) = 0
                For j = 0 To 364
                    ' Si el caudal del dia es mayor que Q95%
                    ' ERROR DOC 27/08/09 - CA XXX
                    ' -- Fallo en caso 5: Guadiana
                    ' ----------------------------
                    ' If (ordEnAños(i)(j) < Me._SeqMagnitudNat(1)) Then
                    If (ordEnAños(i)(j) <= Me._SeqMagnitudNat(1)) Then
                        nSeguido = nSeguido + 1
                        If (nSeguido > diasSeguidosMinQ95(i)) Then
                            diasSeguidosMinQ95(i) = nSeguido
                        End If
                        'nSeguido = nSeguido + 1
                    Else
                        nSeguido = 0
                    End If
                Next
            Next

            Me._SeqDuracionAlt(0) = 0
            For i = 0 To nAños - 1
                Me._SeqDuracionAlt(0) = Me._SeqDuracionAlt(0) + diasSeguidosMinQ95(i)
            Next
            Me._SeqDuracionAlt(0) = Me._SeqDuracionAlt(0) / nAños

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++++ Calculo de la Duracion (dias a nulo) +++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._nDiasNulosAlt(nAños - 1)

            Dim acumNulos As Integer = 0
            Dim acumAUX As Integer = 0

            acum = 0
            For i = 0 To nAños - 1
                If (Date.IsLeapYear(Me._datos.SerieAltDiaria.dia(acum).Year + 1) = True) Then
                    For j = 0 To 365
                        If (Me._datos.SerieAltDiaria.caudalDiaria(j) = 0) Then
                            acumNulos = acumNulos + 1
                            acumAUX = acumAUX + 1
                        End If
                    Next
                    acum = acum + 366
                Else
                    For j = 0 To 364
                        If (Me._datos.SerieAltDiaria.caudalDiaria(j) = 0) Then
                            acumNulos = acumNulos + 1
                            acumAUX = acumAUX + 1
                        End If
                    Next
                    acum = acum + 366
                End If
                ' Para calculos posteriores
                Me._nDiasNulosAlt(i) = acumAUX
                acumAUX = 0
            Next
            Me._SeqDuracionAlt(1) = acumNulos / nAños


            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++ Calculo por MES de los dias nulos +++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._SeqDuracionCerosMesAlt.ndias(11)
            ReDim Me._SeqDuracionCerosMesAlt.mes(11)


            For i = 0 To 11
                Me._SeqDuracionCerosMesAlt.ndias(i) = 0
                Me._SeqDuracionCerosMesAlt.mes(i) = (i + Me._datos.mesInicio) Mod 12
                If (Me._SeqDuracionCerosMesAlt.mes(i) = 0) Then
                    Me._SeqDuracionCerosMesAlt.mes(i) = 12
                End If
            Next

            mesAct = Me._datos.SerieAltDiaria.dia(0).Month
            posArray = 0

            For i = 0 To Me._datos.SerieAltDiaria.dia.Length - 1
                If (mesAct <> Me._datos.SerieAltDiaria.dia(i).Month) Then
                    mesAct = Me._datos.SerieAltDiaria.dia(i).Month
                    posArray = posArray + 1
                    If (posArray = 12) Then
                        posArray = 0
                    End If
                End If
                If (Me._datos.SerieAltDiaria.caudalDiaria(i) = 0) Then
                    Me._SeqDuracionCerosMesAlt.ndias(posArray) = Me._SeqDuracionCerosMesAlt.ndias(posArray) + 1
                End If
            Next

            For i = 0 To 11
                Me._SeqDuracionCerosMesAlt.ndias(i) = Me._SeqDuracionCerosMesAlt.ndias(i) / nAños
            Next
        End Sub
#End Region

#Region "Calculo de los indices"
        ''' <summary>
        ''' Valores de Indices habituales
        ''' </summary>
        ''' <remarks>Informes 7a y 7b</remarks>
        Private Sub CalcularIndicesHabitualesCASO3()

            Dim i, j As Integer

            ReDim Me._IndicesHabituales(6)
            Me._IndicesHabituales(0).calculado = False
            Me._IndicesHabituales(1).calculado = False
            Me._IndicesHabituales(2).calculado = False
            Me._IndicesHabituales(3).calculado = False
            Me._IndicesHabituales(4).calculado = False
            Me._IndicesHabituales(5).calculado = False
            Me._IndicesHabituales(6).calculado = False

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++ IAH1 - Indice 1: Magnitud de las aportaciones Anuales ++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Dim acH, acM, acS As Single
            Dim nAñosH As Integer
            Dim nAñosM As Integer
            Dim nAñosS As Integer

            Dim nCalInvertidosH As Integer = 0
            Dim nCalInvertidosM As Integer = 0
            Dim nCalInvertidosS As Integer = 0

            acH = 0
            acM = 0
            acS = 0

            nAñosH = 0
            nAñosM = 0
            nAñosS = 0

            Dim nAños = Me._AportacionNatAnualOrdAños.año.Length

            ReDim Preserve Me._IndicesHabituales(0).invertido(3)
            ReDim Preserve Me._IndicesHabituales(0).indeterminacion(3)

            For i = 0 To 3
                Me._IndicesHabituales(0).invertido(i) = False
                Me._IndicesHabituales(0).indeterminacion(i) = False
            Next

            ' Cambio importante: Los indices no se marcan como inverso hasta que el 50% de los
            '                    años no sean inversos
            For i = 0 To nAños - 1
                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    nAñosH = nAñosH + 1
                    If (Me._AportacionNatAnualOrdAños.aportacion(i) = 0) Then
                        Me._IndicesHabituales(0).indeterminacion(0) = True
                        If (Me._AportacionAltAnualOrdAños.aportacion(i) = 0) Then
                            acH = acH + 1
                        Else
                            acH = acH + 0
                        End If
                    Else
                        If (Me._AportacionAltAnualOrdAños.aportacion(i) > Me._AportacionNatAnualOrdAños.aportacion(i)) Then
                            acH = acH + (Me._AportacionNatAnualOrdAños.aportacion(i) / Me._AportacionAltAnualOrdAños.aportacion(i))
                            Me._IndicesHabituales(0).invertido(0) = True
                            nCalInvertidosH += 1
                        Else
                            acH = acH + (Me._AportacionAltAnualOrdAños.aportacion(i) / Me._AportacionNatAnualOrdAños.aportacion(i))
                        End If
                    End If
                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    nAñosM = nAñosM + 1
                    If (Me._AportacionNatAnualOrdAños.aportacion(i) = 0) Then
                        Me._IndicesHabituales(0).indeterminacion(1) = True
                        If (Me._AportacionAltAnualOrdAños.aportacion(i) = 0) Then
                            acM = acM + 1
                        Else
                            acM = acM + 0
                        End If
                    Else
                        If (Me._AportacionAltAnualOrdAños.aportacion(i) > Me._AportacionNatAnualOrdAños.aportacion(i)) Then
                            acM = acM + (Me._AportacionNatAnualOrdAños.aportacion(i) / Me._AportacionAltAnualOrdAños.aportacion(i))
                            Me._IndicesHabituales(0).invertido(1) = True
                            nCalInvertidosM += 1
                        Else
                            acM = acM + (Me._AportacionAltAnualOrdAños.aportacion(i) / Me._AportacionNatAnualOrdAños.aportacion(i))
                        End If
                    End If

                Else
                    nAñosS = nAñosS + 1
                    If (Me._AportacionNatAnualOrdAños.aportacion(i) = 0) Then
                        Me._IndicesHabituales(0).indeterminacion(2) = True
                        If (Me._AportacionAltAnualOrdAños.aportacion(i) = 0) Then
                            acS = acS + 1
                        Else
                            acS = acS + 0
                        End If
                    Else
                        If (Me._AportacionAltAnualOrdAños.aportacion(i) > Me._AportacionNatAnualOrdAños.aportacion(i)) Then
                            acS = acS + (Me._AportacionNatAnualOrdAños.aportacion(i) / Me._AportacionAltAnualOrdAños.aportacion(i))
                            Me._IndicesHabituales(0).invertido(2) = True
                            nCalInvertidosS += 1
                        Else
                            acS = acS + (Me._AportacionAltAnualOrdAños.aportacion(i) / Me._AportacionNatAnualOrdAños.aportacion(i))
                        End If
                    End If
                End If
            Next

            acH = acH / nAñosH
            acM = acM / nAñosM
            acS = acS / nAñosS

            ' Ver si es invertido cada uno de los indices
            If ((nCalInvertidosH / nAñosH) > 0.5) Then
                Me._IndicesHabituales(0).invertido(0) = True
            Else
                Me._IndicesHabituales(0).invertido(0) = False
            End If
            If ((nCalInvertidosM / nAñosM) > 0.5) Then
                Me._IndicesHabituales(0).invertido(1) = True
            Else
                Me._IndicesHabituales(0).invertido(1) = False
            End If
            If ((nCalInvertidosS / nAñosS) > 0.5) Then
                Me._IndicesHabituales(0).invertido(2) = True
            Else
                Me._IndicesHabituales(0).invertido(2) = False
            End If

            Me._IndicesHabituales(0).invertido(3) = Me._IndicesHabituales(0).invertido(0) Or _
                                                    Me._IndicesHabituales(0).invertido(1) Or _
                                                    Me._IndicesHabituales(0).invertido(2)
            Me._IndicesHabituales(0).indeterminacion(3) = Me._IndicesHabituales(0).indeterminacion(0) Or _
                                                          Me._IndicesHabituales(0).indeterminacion(1) Or _
                                                          Me._IndicesHabituales(0).indeterminacion(2)

            ReDim Preserve Me._IndicesHabituales(0).valor(3)

            Me._IndicesHabituales(0).valor(0) = acH
            Me._IndicesHabituales(0).valor(1) = acM
            Me._IndicesHabituales(0).valor(2) = acS

            Me._IndicesHabituales(0).valor(3) = 0.25 * acH + 0.5 * acM + 0.25 * acS
            Me._IndicesHabituales(0).calculado = True


            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++ IAH2 - Indice 2: magnitud de aportaciones mensuales +++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


            Dim acHm, acMm, acSm As Single

            acH = 0
            acM = 0
            acS = 0

            acHm = 0
            acSm = 0
            acMm = 0

            nCalInvertidosH = 0
            nCalInvertidosM = 0
            nCalInvertidosS = 0

            nAñosH = 0
            nAñosM = 0
            nAñosS = 0

            ReDim Preserve Me._IndicesHabituales(1).invertido(3)
            ReDim Preserve Me._IndicesHabituales(1).indeterminacion(3)

            For i = 0 To 3
                Me._IndicesHabituales(1).invertido(i) = False
                Me._IndicesHabituales(1).indeterminacion(i) = False
            Next

            'Me._IndicesHabituales(1).invertido = False

            For i = 0 To nAños - 1

                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    For j = 0 To 11
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) = 0) Then
                            Me._IndicesHabituales(1).indeterminacion(0) = True
                            If (Me._AportacionAltMen.aportacion((i * 12) + j) = 0) Then
                                acHm = acHm + 1
                            Else
                                acHm = acHm + 0
                            End If
                        Else
                            If (Me._AportacionAltMen.aportacion((i * 12) + j) > Me._AportacionNatMen.aportacion((i * 12) + j)) Then
                                acHm = acHm + (Me._AportacionNatMen.aportacion((i * 12) + j) / Me._AportacionAltMen.aportacion((i * 12) + j))
                                Me._IndicesHabituales(1).invertido(0) = True
                                nCalInvertidosH += 1
                            Else
                                acHm = acHm + (Me._AportacionAltMen.aportacion((i * 12) + j) / Me._AportacionNatMen.aportacion((i * 12) + j))
                            End If
                        End If
                    Next
                    nAñosH = nAñosH + 1

                    acHm = acHm / 12
                    acH = acH + acHm
                    acHm = 0

                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    For j = 0 To 11
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) = 0) Then
                            Me._IndicesHabituales(1).indeterminacion(1) = True
                            If (Me._AportacionAltMen.aportacion((i * 12) + j) = 0) Then
                                acMm = acMm + 1
                            Else
                                acMm = acMm + 0
                            End If
                        Else
                            If (Me._AportacionAltMen.aportacion((i * 12) + j) > Me._AportacionNatMen.aportacion((i * 12) + j)) Then
                                acMm = acMm + (Me._AportacionNatMen.aportacion((i * 12) + j) / Me._AportacionAltMen.aportacion((i * 12) + j))
                                Me._IndicesHabituales(1).invertido(1) = True
                                nCalInvertidosM += 1
                            Else
                                acMm = acMm + (Me._AportacionAltMen.aportacion((i * 12) + j) / Me._AportacionNatMen.aportacion((i * 12) + j))
                            End If
                        End If
                    Next
                    nAñosM = nAñosM + 1

                    acMm = acMm / 12
                    acM = acM + acMm
                    acMm = 0

                Else
                    For j = 0 To 11
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) = 0) Then
                            Me._IndicesHabituales(1).indeterminacion(2) = True
                            If (Me._AportacionAltMen.aportacion((i * 12) + j) = 0) Then
                                acSm = acSm + 1
                            Else
                                acSm = acSm + 0
                            End If
                        Else
                            If (Me._AportacionAltMen.aportacion((i * 12) + j) > Me._AportacionNatMen.aportacion((i * 12) + j)) Then
                                acSm = acSm + (Me._AportacionNatMen.aportacion((i * 12) + j) / Me._AportacionAltMen.aportacion((i * 12) + j))
                                Me._IndicesHabituales(1).invertido(2) = True
                                nCalInvertidosS += 1
                            Else
                                acSm = acSm + (Me._AportacionAltMen.aportacion((i * 12) + j) / Me._AportacionNatMen.aportacion((i * 12) + j))
                            End If
                        End If
                    Next
                    nAñosS = nAñosS + 1

                    acSm = acSm / 12
                    acS = acS + acSm
                    acSm = 0

                End If

            Next

            acH = acH / nAñosH
            acM = acM / nAñosM
            acS = acS / nAñosS

            ' Ver si es invertido cada uno de los indices
            If ((nCalInvertidosH / nAñosH) > 0.5) Then
                Me._IndicesHabituales(1).invertido(0) = True
            Else
                Me._IndicesHabituales(1).invertido(0) = False
            End If
            If ((nCalInvertidosM / nAñosM) > 0.5) Then
                Me._IndicesHabituales(1).invertido(1) = True
            Else
                Me._IndicesHabituales(1).invertido(1) = False
            End If
            If ((nCalInvertidosS / nAñosS) > 0.5) Then
                Me._IndicesHabituales(1).invertido(2) = True
            Else
                Me._IndicesHabituales(1).invertido(2) = False
            End If

            Me._IndicesHabituales(1).invertido(3) = Me._IndicesHabituales(1).invertido(0) Or _
                                                    Me._IndicesHabituales(1).invertido(1) Or _
                                                    Me._IndicesHabituales(1).invertido(2)
            Me._IndicesHabituales(1).indeterminacion(3) = Me._IndicesHabituales(1).indeterminacion(0) Or _
                                                          Me._IndicesHabituales(1).indeterminacion(1) Or _
                                                          Me._IndicesHabituales(1).indeterminacion(2)

            ReDim Preserve Me._IndicesHabituales(1).valor(3)

            Me._IndicesHabituales(1).valor(0) = acH
            Me._IndicesHabituales(1).valor(1) = acM
            Me._IndicesHabituales(1).valor(2) = acS

            Me._IndicesHabituales(1).valor(3) = 0.25 * acH + 0.5 * acM + 0.25 * acS
            Me._IndicesHabituales(1).calculado = True

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++ Indice 3 : Variabilidad habitual++++++++++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' NO SE CALCULA AQUI -> Tiene su propia funcion

            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++ IAH 4 - Indice 4 : Variabilidad extrema ++++++++++++++++++++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Preserve Me._IndicesHabituales(3).invertido(3)
            ReDim Preserve Me._IndicesHabituales(3).indeterminacion(3)

            For i = 0 To 3
                Me._IndicesHabituales(3).invertido(i) = False
                Me._IndicesHabituales(3).indeterminacion(i) = False
            Next

            Dim maxN, minN, maxA, minA As Single

            acH = 0
            acM = 0
            acS = 0

            'acHm = 0
            'acSm = 0
            'acMm = 0

            nCalInvertidosH = 0
            nCalInvertidosM = 0
            nCalInvertidosS = 0

            nAñosH = 0
            nAñosM = 0
            nAñosS = 0

            ReDim Preserve Me._IndicesHabituales(3).invertido(3)
            ReDim Preserve Me._IndicesHabituales(3).indeterminacion(3)


            For i = 0 To nAños - 1

                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    maxN = -1
                    minN = 99999
                    maxA = -1
                    minA = 99999
                    For j = 0 To 11
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) > maxN) Then
                            maxN = Me._AportacionNatMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) < minN) Then
                            minN = Me._AportacionNatMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionAltMen.aportacion((i * 12) + j) > maxA) Then
                            maxA = Me._AportacionAltMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionAltMen.aportacion((i * 12) + j) < minA) Then
                            minA = Me._AportacionAltMen.aportacion((i * 12) + j)
                        End If
                    Next

                    If ((maxN - minN) = 0) Then
                        Me._IndicesHabituales(3).invertido(0) = True
                        nCalInvertidosH += 1
                        If ((maxA - minA) = 0) Then
                            acH = acH + 1
                        Else
                            acH = acH
                        End If
                    Else
                        If ((maxA - minA) > (maxN - minN)) Then
                            Me._IndicesHabituales(3).indeterminacion(0) = True
                            acH = acH + ((maxN - minN) / (maxA - minA))
                        Else
                            acH = acH + ((maxA - minA) / (maxN - minN))
                        End If

                    End If

                    'acH = acH + ((maxA - minA) / (maxN - minN))

                    nAñosH = nAñosH + 1

                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    maxN = -1
                    minN = 99999
                    maxA = -1
                    minA = 99999
                    For j = 0 To 11
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) > maxN) Then
                            maxN = Me._AportacionNatMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) < minN) Then
                            minN = Me._AportacionNatMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionAltMen.aportacion((i * 12) + j) > maxA) Then
                            maxA = Me._AportacionAltMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionAltMen.aportacion((i * 12) + j) < minA) Then
                            minA = Me._AportacionAltMen.aportacion((i * 12) + j)
                        End If
                    Next

                    If ((maxN - minN) = 0) Then
                        Me._IndicesHabituales(3).invertido(1) = True
                        nCalInvertidosM += 1
                        If ((maxA - minA) = 0) Then
                            acM = acM + 1
                        Else
                            acM = acM
                        End If
                    Else
                        If ((maxA - minA) > (maxN - minN)) Then
                            Me._IndicesHabituales(3).indeterminacion(1) = True
                            acM = acM + ((maxN - minN) / (maxA - minA))
                        Else
                            acM = acM + ((maxA - minA) / (maxN - minN))
                        End If

                    End If
                    'acM = acM + ((maxA - minA) / (maxN - minN))

                    nAñosM = nAñosM + 1

                Else
                    maxN = -1
                    minN = 99999
                    maxA = -1
                    minA = 99999
                    For j = 0 To 11
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) > maxN) Then
                            maxN = Me._AportacionNatMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionNatMen.aportacion((i * 12) + j) < minN) Then
                            minN = Me._AportacionNatMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionAltMen.aportacion((i * 12) + j) > maxA) Then
                            maxA = Me._AportacionAltMen.aportacion((i * 12) + j)
                        End If
                        If (Me._AportacionAltMen.aportacion((i * 12) + j) < minA) Then
                            minA = Me._AportacionAltMen.aportacion((i * 12) + j)
                        End If
                    Next

                    If ((maxN - minN) = 0) Then
                        Me._IndicesHabituales(3).invertido(2) = True
                        nCalInvertidosS += 1
                        If ((maxA - minA) = 0) Then
                            acS = acS + 1
                        Else
                            acS = acS
                        End If
                    Else
                        If ((maxA - minA) > (maxN - minN)) Then
                            Me._IndicesHabituales(3).indeterminacion(2) = True
                            acS = acS + ((maxN - minN) / (maxA - minA))
                        Else
                            acS = acS + ((maxA - minA) / (maxN - minN))
                        End If

                    End If

                    'acS = acS + ((maxA - minA) / (maxN - minN))

                    nAñosS = nAñosS + 1

                End If
            Next

            acH = acH / nAñosH
            acM = acM / nAñosM
            acS = acS / nAñosS

            ' Ver si es invertido cada uno de los indices
            If ((nCalInvertidosH / nAñosH) > 0.5) Then
                Me._IndicesHabituales(3).invertido(0) = True
            Else
                Me._IndicesHabituales(3).invertido(0) = False
            End If
            If ((nCalInvertidosM / nAñosM) > 0.5) Then
                Me._IndicesHabituales(3).invertido(1) = True
            Else
                Me._IndicesHabituales(3).invertido(1) = False
            End If
            If ((nCalInvertidosS / nAñosS) > 0.5) Then
                Me._IndicesHabituales(3).invertido(2) = True
            Else
                Me._IndicesHabituales(3).invertido(2) = False
            End If

            Me._IndicesHabituales(3).invertido(3) = Me._IndicesHabituales(3).invertido(0) Or _
                                                    Me._IndicesHabituales(3).invertido(1) Or _
                                                    Me._IndicesHabituales(3).invertido(2)
            Me._IndicesHabituales(3).indeterminacion(3) = Me._IndicesHabituales(3).indeterminacion(0) Or _
                                                          Me._IndicesHabituales(3).indeterminacion(1) Or _
                                                          Me._IndicesHabituales(3).indeterminacion(2)


            ReDim Preserve Me._IndicesHabituales(3).valor(3)

            Me._IndicesHabituales(3).valor(0) = acH
            Me._IndicesHabituales(3).valor(1) = acM
            Me._IndicesHabituales(3).valor(2) = acS

            Me._IndicesHabituales(3).valor(3) = 0.25 * acH + 0.5 * acM + 0.25 * acS
            Me._IndicesHabituales(3).calculado = True


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++ Indice 5: +++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' No se calcula

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++ IAH 5 - Indice 6: Estacionalidad de Máximos+++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++            

            ' Señalar los maximos y minimos de cada año

            ' Array de todos los meses
            Dim mesesMaximosNat() As Boolean
            ReDim mesesMaximosNat(nAños * 12 - 1)

            Dim mesesMaximosAlt() As Boolean
            ReDim mesesMaximosAlt(nAños * 12 - 1)

            For i = 0 To (nAños * 12) - 1
                mesesMaximosNat(i) = False
                mesesMaximosAlt(i) = False
            Next

            Dim maximo As Single
            Dim pos As Integer
            Dim mesesMax() As Integer = Nothing
            Dim diferencia As Integer

            pos = 0
            acH = 0
            acM = 0
            acS = 0

            ' ++++++++++ Max Naturales ++++++++++++++++
            ' Se busca el maximo anual (unico) para cada año
            For i = 0 To nAños - 1
                maximo = -1
                For j = 0 To 11
                    If (maximo < Me._AportacionNatMen.aportacion(i * 12 + j)) Then
                        maximo = Me._AportacionNatMen.aportacion(i * 12 + j)
                        ' Guardar y borrar los posibles anteriores maximos
                        ReDim mesesMax(0)
                        mesesMax(0) = i * 12 + j
                    ElseIf (maximo = Me._AportacionNatMen.aportacion(i * 12 + j)) Then
                        ReDim Preserve mesesMax(mesesMax.Length)
                        mesesMax(mesesMax.Length - 1) = i * 12 + j
                    End If
                Next
                'For j = 0 To mesesMax.Length - 1
                mesesMaximosNat(mesesMax(0)) = True
                'Next
            Next

            ReDim mesesMax(0) ' Por seguridad
            pos = 0

            ' ++++++++++ Max Alteradas ++++++++++++++++
            ' Se busca el maximo anual (multiple) para cada año
            For i = 0 To nAños - 1
                maximo = -1
                For j = 0 To 11
                    If (maximo < Me._AportacionAltMen.aportacion(i * 12 + j)) Then
                        maximo = Me._AportacionAltMen.aportacion(i * 12 + j)
                        ' Guardar y borrar los posibles anteriores maximos
                        ReDim mesesMax(0)
                        mesesMax(0) = i * 12 + j
                    ElseIf (maximo = Me._AportacionAltMen.aportacion(i * 12 + j)) Then
                        ReDim Preserve mesesMax(mesesMax.Length)
                        mesesMax(mesesMax.Length - 1) = i * 12 + j
                    End If
                Next
                For j = 0 To mesesMax.Length - 1
                    mesesMaximosAlt(mesesMax(j)) = True
                Next
            Next


            ' +++++++ Determinacion de ventanas
            For i = 0 To mesesMaximosNat.Length - 1

                ' Encuentro maximo en los naturales
                If (mesesMaximosNat(i)) Then
                    Dim limInf As Integer = -1
                    Dim limSup As Integer = -1

                    Dim posMaxVentA As Integer = -1
                    Dim posMaxVentB As Integer = -1

                    Dim fechaMax As Date = Me._AportacionNatMen.mes(i)  ' Saco la fecha del mes maximo
                    Dim fechaVentA As Date = fechaMax.AddMonths(-6)     ' Inicio de la ventana
                    Dim fechaVentB As Date = fechaMax.AddMonths(6)      ' Fin de la ventana

                    ' Encontrar en que posicion en el array de meses de las fechas de los limites de ventana
                    For j = 0 To Me._AportacionNatMen.mes.Length - 1
                        If (Date.Compare(Me._AportacionNatMen.mes(j), fechaVentA) = 0) Then
                            limInf = j
                        End If
                        If (Date.Compare(Me._AportacionNatMen.mes(j), fechaVentB) = 0) Then
                            limSup = j
                        End If
                    Next

                    ' Si no se han encontrado los limites es que no tenemos datos. Se pone el año como defecto.
                    '  -> Inf 1/mesInicio       -> Sup dia/mesInicio-1
                    If (limInf < 0) Then
                        If (fechaMax.Month > Me._datos.mesInicio - 1) Then 'If (fechaMax.Month > 9) Then
                            'limInf = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMax.Year, 10, 1))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                If (Date.Compare(Me._AportacionNatMen.mes(j), New Date(fechaMax.Year, Me._datos.mesInicio, 1)) = 0) Then
                                    limInf = j
                                End If
                            Next
                        Else
                            'limInf = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMax.Year - 1, 10, 1))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                If (Date.Compare(Me._AportacionNatMen.mes(j), New Date(fechaMax.Year - 1, Me._datos.mesInicio, 1)) = 0) Then
                                    limInf = j
                                End If
                            Next
                        End If

                    End If

                    If (limSup < 0) Then
                        Dim fechaSup As Date
                        Dim mesSup As Integer
                        If (Me._datos.mesInicio = 1) Then
                            mesSup = 12
                        Else
                            mesSup = Me._datos.mesInicio - 1
                        End If
                        If (fechaMax.Month > Me._datos.mesInicio - 1) Then 'If (fechaMax.Month > 9) Then
                            'limSup = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMax.Year + 1, 9, 30))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                fechaSup = New Date(fechaMax.Year + 1, mesSup, Date.DaysInMonth(fechaMax.Year + 1, mesSup))
                                If (Date.Compare(Me._AportacionNatMen.mes(j), fechaSup) = 0) Then
                                    limSup = j
                                End If
                            Next
                        Else
                            'limSup = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMax.Year, 9, 30))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                fechaSup = New Date(fechaMax.Year, mesSup, Date.DaysInMonth(fechaMax.Year, mesSup))
                                If (Date.Compare(Me._AportacionNatMen.mes(j), fechaSup) = 0) Then
                                    limSup = j
                                End If
                            Next
                        End If

                    End If

                    ' Buscar en las ventanas definidas
                    For j = limInf To i
                        If (mesesMaximosAlt(j)) Then
                            posMaxVentA = j
                            Exit For
                        End If
                    Next
                    For j = i To limSup
                        If (mesesMaximosAlt(j)) Then
                            posMaxVentB = j
                            Exit For
                        End If
                    Next


                    If ((posMaxVentA <> -1) And (posMaxVentB <> -1)) Then
                        ' i es el mes del maximo natural
                        If ((i - posMaxVentA) > (posMaxVentB - i)) Then
                            diferencia = i - posMaxVentA
                        Else
                            diferencia = posMaxVentB - i
                        End If
                    ElseIf ((posMaxVentA <> -1) And (posMaxVentB = -1)) Then
                        diferencia = i - posMaxVentA
                    ElseIf ((posMaxVentA = -1) And (posMaxVentB <> -1)) Then ' La mas usual
                        diferencia = posMaxVentB - i
                    Else
                        diferencia = 6
                    End If

                    ' El año almacenado en la lista es el i, no el i+1
                    Dim añoBuscar As Integer
                    If (fechaMax.Month > Me._datos.mesInicio - 1) Then
                        añoBuscar = fechaMax.Year
                    Else
                        añoBuscar = fechaMax.Year - 1
                    End If

                    ' Posicion del año en la lista.
                    Dim posTipo As Integer = Array.BinarySearch(Me._AportacionNatAnualOrdAños.año, añoBuscar)


                    ' Mirar el tipo del año señalado en la lista.
                    If (Me._AportacionNatAnualOrdAños.tipo(posTipo) = TIPOAÑO.HUMEDO) Then
                        acH = acH + diferencia
                    ElseIf (Me._AportacionNatAnualOrdAños.tipo(posTipo) = TIPOAÑO.MEDIO) Then
                        acM = acM + diferencia
                    Else
                        acS = acS + diferencia
                    End If

                End If ' Encontrado maximo
            Next

            acH = acH / nAñosH
            acM = acM / nAñosM
            acS = acS / nAñosS

            acH = 1 - (1 / 6) * acH
            acM = 1 - (1 / 6) * acM
            acS = 1 - (1 / 6) * acS

            ReDim Preserve Me._IndicesHabituales(5).valor(3)

            Me._IndicesHabituales(5).valor(0) = acH
            Me._IndicesHabituales(5).valor(1) = acM
            Me._IndicesHabituales(5).valor(2) = acS

            Me._IndicesHabituales(5).calculado = True
            'Me._IndicesHabituales(5).invertido = False
            Me._IndicesHabituales(5).valor(3) = 0.25 * acH + 0.5 * acM + 0.25 * acS


            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++ IAH 6 - Indice 7: Estacionalidad de Mínimos ++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            ' Señalar los maximos y minimos de cada año

            ' Array de todos los meses
            Dim mesesMinimosNat() As Boolean
            ReDim mesesMinimosNat(nAños * 12 - 1)

            Dim mesesMinimosAlt() As Boolean
            ReDim mesesMinimosAlt(nAños * 12 - 1)

            For i = 0 To (nAños * 12) - 1
                mesesMinimosNat(i) = False
                mesesMinimosAlt(i) = False
            Next

            Dim minimo As Single
            'Dim pos As Integer
            Dim mesesMin() As Integer = Nothing
            'Dim diferencia As Integer

            acH = 0
            acM = 0
            acS = 0

            pos = 0

            ' ++++++++++ Min Naturales ++++++++++++++++
            For i = 0 To nAños - 1
                minimo = 9999999
                For j = 0 To 11
                    If (minimo > Me._AportacionNatMen.aportacion(i * 12 + j)) Then
                        minimo = Me._AportacionNatMen.aportacion(i * 12 + j)
                        ' Guardar y borrar los posibles anteriores maximos
                        ReDim mesesMin(0)
                        mesesMin(0) = i * 12 + j
                    ElseIf (minimo = Me._AportacionNatMen.aportacion(i * 12 + j)) Then
                        ReDim Preserve mesesMin(mesesMin.Length)
                        mesesMin(mesesMin.Length - 1) = i * 12 + j
                    End If
                Next
                'For j = 0 To mesesMin.Length - 1
                mesesMinimosNat(mesesMin(0)) = True
                'Next
            Next

            ReDim mesesMin(0) ' Por seguridad
            pos = 0

            ' ++++++++++ Min Alteradas ++++++++++++++++
            For i = 0 To nAños - 1
                minimo = 999999
                For j = 0 To 11
                    If (minimo > Me._AportacionAltMen.aportacion(i * 12 + j)) Then
                        minimo = Me._AportacionAltMen.aportacion(i * 12 + j)
                        ' Guardar y borrar los posibles anteriores maximos
                        ReDim mesesMin(0)
                        mesesMin(0) = i * 12 + j
                    ElseIf (minimo = Me._AportacionAltMen.aportacion(i * 12 + j)) Then
                        ReDim Preserve mesesMin(mesesMin.Length)
                        mesesMin(mesesMin.Length - 1) = i * 12 + j
                    End If
                Next
                For j = 0 To mesesMin.Length - 1
                    mesesMinimosAlt(mesesMin(j)) = True
                Next
            Next


            ' +++++++ Determinacion de ventanas
            For i = 0 To mesesMinimosNat.Length - 1
                ' Encuentro maximo en los naturales
                If (mesesMinimosNat(i)) Then


                    Dim limInf As Integer = -1
                    Dim limSup As Integer = -1

                    Dim posMaxVentA As Integer = -1
                    Dim posMaxVentB As Integer = -1

                    Dim fechaMin As Date = Me._AportacionNatMen.mes(i) ' Saco la fecha del mes maximo
                    Dim fechaVentA As Date = fechaMin.AddMonths(-6)
                    Dim fechaVentB As Date = fechaMin.AddMonths(6)

                    ' Buscar los limites de la ventana en la lista de meses validos
                    For j = 0 To Me._AportacionNatMen.mes.Length - 1
                        If (Date.Compare(Me._AportacionNatMen.mes(j), fechaVentA) = 0) Then
                            limInf = j
                        End If
                        If (Date.Compare(Me._AportacionNatMen.mes(j), fechaVentB) = 0) Then
                            limSup = j
                        End If
                    Next

                    ' Comprobar que el mes existe en mi lista (si no esta es que no es año valido/coetaneo) 
                    If (limInf < 0) Then
                        If (fechaMin.Month > Me._datos.mesInicio - 1) Then 'If (fechaMin.Month > 9) Then
                            'limInf = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMin.Year, 10, 1))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                If (Date.Compare(Me._AportacionNatMen.mes(j), New Date(fechaMin.Year, Me._datos.mesInicio, 1)) = 0) Then
                                    limInf = j
                                End If
                            Next
                        Else
                            'limInf = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMin.Year - 1, 10, 1))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                If (Date.Compare(Me._AportacionNatMen.mes(j), New Date(fechaMin.Year - 1, Me._datos.mesInicio, 1)) = 0) Then
                                    limInf = j
                                End If
                            Next
                        End If

                    End If

                    If (limSup < 0) Then
                        Dim fechaSup As Date
                        Dim mesSup As Integer
                        If (Me._datos.mesInicio = 1) Then
                            mesSup = 12
                        Else
                            mesSup = Me._datos.mesInicio - 1
                        End If
                        If (fechaMin.Month > Me._datos.mesInicio - 1) Then 'If (fechaMin.Month > 9) Then
                            'limSup = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMin.Year + 1, 9, 30))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                fechaSup = New Date(fechaMin.Year + 1, mesSup, Date.DaysInMonth(fechaMin.Year + 1, mesSup))
                                If (Date.Compare(Me._AportacionNatMen.mes(j), fechaSup) = 0) Then
                                    limSup = j
                                End If
                            Next
                        Else
                            'limSup = Array.BinarySearch(Me._AportacionNatMen.mes, New Date(fechaMin.Year, 9, 30))
                            For j = 0 To Me._AportacionNatMen.mes.Length - 1
                                fechaSup = New Date(fechaMin.Year, mesSup, Date.DaysInMonth(fechaMin.Year, mesSup))
                                If (Date.Compare(Me._AportacionNatMen.mes(j), fechaSup) = 0) Then
                                    limSup = j
                                End If
                            Next
                        End If

                    End If

                    ' Buscar en las ventanas definidas
                    For j = limInf To i
                        If (mesesMinimosAlt(j)) Then
                            posMaxVentA = j
                            Exit For
                        End If
                    Next
                    For j = i To limSup
                        If (mesesMinimosAlt(j)) Then
                            posMaxVentB = j
                            'Exit For
                        End If
                    Next


                    If ((posMaxVentA <> -1) And (posMaxVentB <> -1)) Then
                        ' i es el mes del maximo natural
                        If ((i - posMaxVentA) > (posMaxVentB - i)) Then
                            diferencia = i - posMaxVentA
                        Else
                            diferencia = posMaxVentB - i
                        End If
                    ElseIf ((posMaxVentA <> -1) And (posMaxVentB = -1)) Then
                        diferencia = i - posMaxVentA
                    ElseIf ((posMaxVentA = -1) And (posMaxVentB <> -1)) Then ' La mas usual
                        diferencia = posMaxVentB - i
                    Else
                        diferencia = 6
                    End If

                    ' El año almacenado en la lista es el i, no el i+1
                    Dim añoBuscar As Integer
                    If (fechaMin.Month > Me._datos.mesInicio - 1) Then
                        añoBuscar = fechaMin.Year
                    Else
                        añoBuscar = fechaMin.Year - 1
                    End If

                    ' Posicion del año en la lista.
                    Dim posTipo As Integer = Array.BinarySearch(Me._AportacionNatAnualOrdAños.año, añoBuscar)

                    ' Mirar el tipo del año señalado en la lista.
                    If (Me._AportacionNatAnualOrdAños.tipo(posTipo) = TIPOAÑO.HUMEDO) Then
                        acH = acH + diferencia
                    ElseIf (Me._AportacionNatAnualOrdAños.tipo(posTipo) = TIPOAÑO.MEDIO) Then
                        acM = acM + diferencia
                    Else
                        acS = acS + diferencia
                    End If

                End If ' Encontrado maximo
            Next


            acH = acH / nAñosH
            acM = acM / nAñosM
            acS = acS / nAñosS

            acH = 1 - (1 / 6) * acH
            acM = 1 - (1 / 6) * acM
            acS = 1 - (1 / 6) * acS

            ReDim Preserve Me._IndicesHabituales(6).valor(3)

            Me._IndicesHabituales(6).valor(0) = acH
            Me._IndicesHabituales(6).valor(1) = acM
            Me._IndicesHabituales(6).valor(2) = acS

            Me._IndicesHabituales(6).calculado = True
            'Me._IndicesHabituales(6).invertido = False
            Me._IndicesHabituales(6).valor(3) = 0.25 * acH + 0.5 * acM + 0.25 * acS

            ' +++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++++ Calculo de indice IAG +++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++++
            'ReDim Me._IndiceIAG(3)
            '' Para Humedo, Medio y Seco
            'For i = 0 To 2
            '    Me._IndiceIAG(i) = (Pow(Me._IndicesHabituales(0).valor(i) + Me._IndicesHabituales(1).valor(i) + Me._IndicesHabituales(3).valor(i) + Me._IndicesHabituales(5).valor(i) + Me._IndicesHabituales(6).valor(i), 2) - _
            '                        (Pow(Me._IndicesHabituales(0).valor(i), 2) + Pow(Me._IndicesHabituales(1).valor(i), 2) + Pow(Me._IndicesHabituales(3).valor(i), 2) + Pow(Me._IndicesHabituales(5).valor(i), 2) + Pow(Me._IndicesHabituales(6).valor(i), 2))) / 20
            'Next

            'Me._IndiceIAG(3) = (Me._IndiceIAG(0) + Me._IndiceIAG(1) + Me._IndiceIAG(2)) / 3

        End Sub
        Private Sub CalcularIndiceHabitual_I3()
            Dim i As Integer
            Dim pos10 As Integer
            Dim pos90 As Integer

            Dim Q10(2) As Single
            Dim Q90(2) As Single
            Dim acum(2) As Single
            Dim naños(2) As Integer

            Dim nCalInvertidosH As Integer = 0
            Dim nCalInvertidosM As Integer = 0
            Dim nCalInvertidosS As Integer = 0

            If (Me._IndicesHabituales Is Nothing) Then
                ReDim Me._IndicesHabituales(6)
            End If
            ' Redimension del indice
            ReDim Me._IndicesHabituales(2).valor(3)
            ReDim Me._IndicesHabituales(2).invertido(3)
            ReDim Me._IndicesHabituales(2).indeterminacion(3)

            pos10 = 0
            While (Me._TablaCQCNat.pe(pos10) < 10)
                pos10 = pos10 + 1
            End While
            pos90 = 0
            While (Me._TablaCQCNat.pe(pos90) < 90)
                pos90 = pos90 + 1
            End While

            For i = 0 To 2
                acum(i) = 0
                naños(i) = 0
            Next

            For i = 0 To 3
                Me._IndicesHabituales(2).invertido(i) = False
                Me._IndicesHabituales(2).invertido(i) = False
            Next

            ' ERROR DOC 27/08/09 - CA 001
            ' -- Fallo en caso 2: Guadarrama
            ' ----------------------------
            'For i = 0 To Me._datos.SerieAltDiaria.nAños - 1
            For i = 0 To Me._AportacionNatAnualOrdAños.año.Length - 1
                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    Q10(0) = (10 - Me._TablaCQCNat.pe(pos10 - 1)) / _
                            (Me._TablaCQCNat.pe(pos10) - Me._TablaCQCNat.pe(pos10 - 1)) * _
                            (Me._TablaCQCNat.caudales(i)(pos10) - Me._TablaCQCNat.caudales(i)(pos10 - 1)) _
                            + Me._TablaCQCNat.caudales(i)(pos10 - 1)
                    Q90(0) = (90 - Me._TablaCQCNat.pe(pos90 - 1)) / _
                            (Me._TablaCQCNat.pe(pos90) - Me._TablaCQCNat.pe(pos90 - 1)) * _
                            (Me._TablaCQCNat.caudales(i)(pos90) - Me._TablaCQCNat.caudales(i)(pos90 - 1)) _
                            + Me._TablaCQCNat.caudales(i)(pos90 - 1)

                    Q10(1) = (10 - Me._TablaCQCAlt.pe(pos10 - 1)) / _
                            (Me._TablaCQCAlt.pe(pos10) - Me._TablaCQCAlt.pe(pos10 - 1)) * _
                            (Me._TablaCQCAlt.caudales(i)(pos10) - Me._TablaCQCAlt.caudales(i)(pos10 - 1)) _
                            + Me._TablaCQCAlt.caudales(i)(pos10 - 1)
                    Q90(1) = (90 - Me._TablaCQCAlt.pe(pos90 - 1)) / _
                            (Me._TablaCQCAlt.pe(pos90) - Me._TablaCQCAlt.pe(pos90 - 1)) * _
                            (Me._TablaCQCAlt.caudales(i)(pos90) - Me._TablaCQCAlt.caudales(i)(pos90 - 1)) _
                            + Me._TablaCQCAlt.caudales(i)(pos90 - 1)
                    If ((Q10(0) - Q90(0)) = 0) And ((Q10(1) - Q90(1)) = 0) Then
                        Me._IndicesHabituales(2).indeterminacion(0) = True
                        acum(0) = acum(0) + 1
                    ElseIf ((Q10(0) - Q90(0)) = 0) And ((Q10(1) - Q90(1)) <> 0) Then
                        Me._IndicesHabituales(2).indeterminacion(0) = True
                        acum(0) = acum(0) + 0
                    ElseIf ((Q10(0) - Q90(0)) < (Q10(1) - Q90(1))) Then
                        Me._IndicesHabituales(2).invertido(0) = True
                        nCalInvertidosH += 1
                        acum(0) = acum(0) + (Q10(0) - Q90(0)) / (Q10(1) - Q90(1))
                    Else
                        acum(0) = acum(0) + (Q10(1) - Q90(1)) / (Q10(0) - Q90(0))
                    End If
                    naños(0) = naños(0) + 1
                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    Q10(0) = (10 - Me._TablaCQCNat.pe(pos10 - 1)) / _
                            (Me._TablaCQCNat.pe(pos10) - Me._TablaCQCNat.pe(pos10 - 1)) * _
                            (Me._TablaCQCNat.caudales(i)(pos10) - Me._TablaCQCNat.caudales(i)(pos10 - 1)) _
                            + Me._TablaCQCNat.caudales(i)(pos10 - 1)
                    Q90(0) = (90 - Me._TablaCQCNat.pe(pos90 - 1)) / _
                            (Me._TablaCQCNat.pe(pos90) - Me._TablaCQCNat.pe(pos90 - 1)) * _
                            (Me._TablaCQCNat.caudales(i)(pos90) - Me._TablaCQCNat.caudales(i)(pos90 - 1)) _
                            + Me._TablaCQCNat.caudales(i)(pos90 - 1)

                    Q10(1) = (10 - Me._TablaCQCAlt.pe(pos10 - 1)) / _
                            (Me._TablaCQCAlt.pe(pos10) - Me._TablaCQCAlt.pe(pos10 - 1)) * _
                            (Me._TablaCQCAlt.caudales(i)(pos10) - Me._TablaCQCAlt.caudales(i)(pos10 - 1)) _
                            + Me._TablaCQCAlt.caudales(i)(pos10 - 1)
                    Q90(1) = (90 - Me._TablaCQCAlt.pe(pos90 - 1)) / _
                            (Me._TablaCQCAlt.pe(pos90) - Me._TablaCQCAlt.pe(pos90 - 1)) * _
                            (Me._TablaCQCAlt.caudales(i)(pos90) - Me._TablaCQCAlt.caudales(i)(pos90 - 1)) _
                            + Me._TablaCQCAlt.caudales(i)(pos90 - 1)
                    If ((Q10(0) - Q90(0)) = 0) And ((Q10(1) - Q90(1)) = 0) Then
                        Me._IndicesHabituales(2).indeterminacion(1) = True
                        acum(1) = acum(1) + 1
                    ElseIf ((Q10(0) - Q90(0)) = 0) And ((Q10(1) - Q90(1)) <> 0) Then
                        Me._IndicesHabituales(2).indeterminacion(1) = True
                        acum(1) = acum(1) + 0
                    ElseIf ((Q10(0) - Q90(0)) < (Q10(1) - Q90(1))) Then
                        Me._IndicesHabituales(2).invertido(1) = True
                        nCalInvertidosM += 1
                        acum(1) = acum(1) + (Q10(0) - Q90(0)) / (Q10(1) - Q90(1))
                    Else
                        acum(1) = acum(1) + (Q10(1) - Q90(1)) / (Q10(0) - Q90(0))
                    End If
                    naños(1) = naños(1) + 1
                Else
                    Q10(0) = (10 - Me._TablaCQCNat.pe(pos10 - 1)) / _
                            (Me._TablaCQCNat.pe(pos10) - Me._TablaCQCNat.pe(pos10 - 1)) * _
                            (Me._TablaCQCNat.caudales(i)(pos10) - Me._TablaCQCNat.caudales(i)(pos10 - 1)) _
                            + Me._TablaCQCNat.caudales(i)(pos10 - 1)
                    Q90(0) = (90 - Me._TablaCQCNat.pe(pos90 - 1)) / _
                            (Me._TablaCQCNat.pe(pos90) - Me._TablaCQCNat.pe(pos90 - 1)) * _
                            (Me._TablaCQCNat.caudales(i)(pos90) - Me._TablaCQCNat.caudales(i)(pos90 - 1)) _
                            + Me._TablaCQCNat.caudales(i)(pos90 - 1)

                    Q10(1) = (10 - Me._TablaCQCAlt.pe(pos10 - 1)) / _
                            (Me._TablaCQCAlt.pe(pos10) - Me._TablaCQCAlt.pe(pos10 - 1)) * _
                            (Me._TablaCQCAlt.caudales(i)(pos10) - Me._TablaCQCAlt.caudales(i)(pos10 - 1)) _
                            + Me._TablaCQCAlt.caudales(i)(pos10 - 1)
                    Q90(1) = (90 - Me._TablaCQCAlt.pe(pos90 - 1)) / _
                            (Me._TablaCQCAlt.pe(pos90) - Me._TablaCQCAlt.pe(pos90 - 1)) * _
                            (Me._TablaCQCAlt.caudales(i)(pos90) - Me._TablaCQCAlt.caudales(i)(pos90 - 1)) _
                            + Me._TablaCQCAlt.caudales(i)(pos90 - 1)
                    If ((Q10(0) - Q90(0)) = 0) And ((Q10(1) - Q90(1)) = 0) Then
                        Me._IndicesHabituales(2).indeterminacion(2) = True
                        acum(2) = acum(2) + 1
                    ElseIf ((Q10(0) - Q90(0)) = 0) And ((Q10(1) - Q90(1)) <> 0) Then
                        Me._IndicesHabituales(2).indeterminacion(2) = True
                        acum(2) = acum(2) + 0
                    ElseIf ((Q10(0) - Q90(0)) < (Q10(1) - Q90(1))) Then
                        Me._IndicesHabituales(2).invertido(2) = True
                        nCalInvertidosS += 1
                        acum(2) = acum(2) + (Q10(0) - Q90(0)) / (Q10(1) - Q90(1))
                    Else
                        acum(2) = acum(2) + (Q10(1) - Q90(1)) / (Q10(0) - Q90(0))
                    End If
                    ' ¿Porque no se hacia como el resto?
                    ' ERROR DOC 27/08/09 - CA 003
                    ' -- Fallo en caso 1: La cierva
                    ' ----------------------------
                    naños(2) = naños(2) + 1
                End If
            Next

            ' Ver si es invertido cada uno de los indices
            If ((nCalInvertidosH / naños(0)) > 0.5) Then
                Me._IndicesHabituales(2).invertido(0) = True
            Else
                Me._IndicesHabituales(2).invertido(0) = False
            End If
            If ((nCalInvertidosM / naños(1)) > 0.5) Then
                Me._IndicesHabituales(2).invertido(1) = True
            Else
                Me._IndicesHabituales(2).invertido(1) = False
            End If
            If ((nCalInvertidosS / naños(2)) > 0.5) Then
                Me._IndicesHabituales(2).invertido(2) = True
            Else
                Me._IndicesHabituales(2).invertido(2) = False
            End If

            Me._IndicesHabituales(2).calculado = True
            ' Generar los i3 finales
            For i = 0 To 2
                Me._IndicesHabituales(2).valor(i) = (1 / naños(i)) * acum(i)
            Next

            Me._IndicesHabituales(2).invertido(3) = Me._IndicesHabituales(2).invertido(0) Or _
                                                    Me._IndicesHabituales(2).invertido(1) Or _
                                                    Me._IndicesHabituales(2).invertido(2)
            Me._IndicesHabituales(2).indeterminacion(3) = Me._IndicesHabituales(2).indeterminacion(0) Or _
                                                          Me._IndicesHabituales(2).indeterminacion(1) Or _
                                                          Me._IndicesHabituales(2).indeterminacion(2)

            Me._IndicesHabituales(2).valor(3) = (Me._IndicesHabituales(2).valor(0) + Me._IndicesHabituales(2).valor(1) + Me._IndicesHabituales(2).valor(2)) / 3

        End Sub
        ''' <summary>
        ''' Indices M1,M2,M3,V1,V2,V3
        ''' </summary>
        ''' <remarks>Informe 7c</remarks>
        Private Sub CalcularIndicesHabitualesAgregados()

            Dim i, j As Integer

            ReDim Me._IndicesHabitualesAgregados(6)

            ' +++++++++++++++++++++++++++++++
            ' ++++ Magnitud +++++++++++++++++
            ' +++++++++++++++++++++++++++++++

            ' +++++++++++++++++++  M1  ++++++++++++++++++++
            ' +++ Magnitud de las aportaciones anuales ++++
            ' +++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesHabitualesAgregados(0).valor(0)
            ReDim Me._IndicesHabitualesAgregados(0).invertido(0)
            ReDim Me._IndicesHabitualesAgregados(0).indeterminacion(0)

            If (Me._HabMagnitudAlt(0) = 0) And (Me._HabMagnitudNatReducido = 0) Then
                Me._IndicesHabitualesAgregados(0).indeterminacion(0) = True
                Me._IndicesHabitualesAgregados(0).valor(0) = 1
            ElseIf (Me._HabMagnitudAlt(0) <> 0) And (Me._HabMagnitudNatReducido = 0) Then
                Me._IndicesHabitualesAgregados(0).indeterminacion(0) = True
                Me._IndicesHabitualesAgregados(0).valor(0) = 0
            ElseIf (Me._HabMagnitudAlt(0) > Me._HabMagnitudNatReducido) Then
                Me._IndicesHabitualesAgregados(0).invertido(0) = True
                Me._IndicesHabitualesAgregados(0).valor(0) = Me._HabMagnitudNatReducido / Me._HabMagnitudAlt(0)
            Else
                Me._IndicesHabitualesAgregados(0).valor(0) = Me._HabMagnitudAlt(0) / Me._HabMagnitudNatReducido
            End If

            Me._IndicesHabitualesAgregados(0).calculado = True

            ' +++++++++++++++++++++ M2+M3 ++++++++++++++++++++
            ' +++ Magnitud de las aportaciones mensuales +++++
            ' +++ Magnitud de las aportaciones por mes   +++++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesHabitualesAgregados(1).valor(0)
            ReDim Me._IndicesHabitualesAgregados(1).invertido(0)
            ReDim Me._IndicesHabitualesAgregados(1).indeterminacion(0)

            ReDim Me._IndiceM3Agregados.valor(11)
            ReDim Me._IndiceM3Agregados.invertido(11)
            ReDim Me._IndiceM3Agregados.indeterminacion(11)

            For i = 0 To 11
                Me._IndiceM3Agregados.invertido(i) = False
                Me._IndiceM3Agregados.indeterminacion(i) = False
            Next


            Dim acum As Single
            Dim mediasMesNat() As Single
            ReDim mediasMesNat(11)
            Dim mediasMesAlt() As Single
            ReDim mediasMesAlt(11)

            acum = 0

            For i = 0 To Me._AportacionNatAnual.aportacion.Length - 1
                For j = 0 To 11
                    mediasMesNat(j) = mediasMesNat(j) + Me._AportacionNatMen.aportacion(i * 12 + j)
                Next
            Next
            For i = 0 To 11
                mediasMesNat(i) = mediasMesNat(i) / 12
            Next
            For i = 0 To Me._AportacionAltAnual.aportacion.Length - 1
                For j = 0 To 11
                    mediasMesAlt(j) = mediasMesAlt(j) + Me._AportacionAltMen.aportacion(i * 12 + j)
                Next
            Next
            For i = 0 To 11
                mediasMesAlt(i) = mediasMesAlt(i) / 12
            Next

            ' Calculo del M3
            For i = 0 To 11
                If (mediasMesAlt(i) = 0) And (mediasMesNat(i) = 0) Then
                    Me._IndiceM3Agregados.indeterminacion(i) = True
                    Me._IndiceM3Agregados.valor(i) = 1
                ElseIf (mediasMesAlt(i) <> 0) And (mediasMesNat(i) = 0) Then
                    Me._IndiceM3Agregados.indeterminacion(i) = True
                    Me._IndiceM3Agregados.valor(i) = 0
                ElseIf (mediasMesAlt(i) > mediasMesNat(i)) Then
                    Me._IndiceM3Agregados.valor(i) = mediasMesNat(i) / mediasMesAlt(i)
                    Me._IndiceM3Agregados.invertido(i) = True
                Else
                    Me._IndiceM3Agregados.valor(i) = mediasMesAlt(i) / mediasMesNat(i)
                End If
            Next

            ' Calculo del M2
            Dim nCalInvertidos As Integer = 0
            For i = 0 To 11
                If (mediasMesAlt(i) = 0) And (mediasMesNat(i) = 0) Then
                    Me._IndicesHabitualesAgregados(1).indeterminacion(0) = True
                    acum = acum + 1
                ElseIf (mediasMesAlt(i) <> 0) And (mediasMesNat(i) = 0) Then
                    Me._IndicesHabitualesAgregados(1).indeterminacion(0) = True
                    acum = acum + 0
                ElseIf (mediasMesAlt(i) > mediasMesNat(i)) Then
                    'Me._IndicesHabitualesAgregados(1).invertido(0) = True
                    nCalInvertidos += 1
                    acum = acum + mediasMesNat(i) / mediasMesAlt(i)
                Else
                    acum = acum + mediasMesAlt(i) / mediasMesNat(i)
                End If
            Next

            ' Ver si es invertido cada uno de los indices
            If ((nCalInvertidos / 12) > 0.5) Then
                Me._IndicesHabitualesAgregados(1).invertido(0) = True
            Else
                Me._IndicesHabitualesAgregados(1).invertido(0) = False
            End If

            Me._IndicesHabitualesAgregados(1).valor(0) = acum / 12
            Me._IndicesHabitualesAgregados(1).calculado = True


            ' ++++++++++++++++++++++++++++++
            ' ++++++ Variabilidad ++++++++++
            ' ++++++++++++++++++++++++++++++

            ' +++++++++++++++++++ V1 +++++++++++++++++++++++++
            ' +++ Variabilidad de las aportaciones anuales +++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++

            ReDim Me._IndicesHabitualesAgregados(2).valor(0)
            ReDim Me._IndicesHabitualesAgregados(2).invertido(0)
            ReDim Me._IndicesHabitualesAgregados(2).indeterminacion(0)

            Dim aux1, aux2 As Single
            Dim CVNat, CVAlt As Single
            Dim naños As Integer = Me._AportacionNatAnual.aportacion.Length

            aux1 = 0
            aux2 = 0
            CVNat = 0
            For i = 0 To naños - 1
                aux1 = aux1 + Pow(Me._AportacionNatAnual.aportacion(i), 2)
                aux2 = aux2 + Me._AportacionNatAnual.aportacion(i)
            Next

            CVNat = (naños * aux1 - Pow(aux2, 2)) / (naños * (naños - 1))
            CVNat = Sqrt(CVNat)

            naños = Me._AportacionAltAnual.aportacion.Length
            aux1 = 0
            aux2 = 0
            CVAlt = 0
            For i = 0 To naños - 1
                aux1 = aux1 + Pow(Me._AportacionAltAnual.aportacion(i), 2)
                aux2 = aux2 + Me._AportacionAltAnual.aportacion(i)
            Next

            CVAlt = (naños * aux1 - Pow(aux2, 2)) / (naños * (naños - 1))
            CVAlt = Sqrt(CVAlt)

            If (CVAlt = 0) And (CVNat = 0) Then
                Me._IndicesHabitualesAgregados(2).indeterminacion(0) = True
                Me._IndicesHabitualesAgregados(2).valor(0) = 1
            ElseIf (CVAlt <> 0) And (CVNat = 0) Then
                Me._IndicesHabitualesAgregados(2).indeterminacion(0) = True
                Me._IndicesHabitualesAgregados(2).valor(0) = 0
            ElseIf (CVAlt > CVNat) Then
                Me._IndicesHabitualesAgregados(2).invertido(0) = True
                Me._IndicesHabitualesAgregados(2).valor(0) = CVNat / CVAlt
            Else
                Me._IndicesHabitualesAgregados(2).valor(0) = CVAlt / CVNat
            End If

            Me._IndicesHabitualesAgregados(2).calculado = True

            ' +++++++++++++++++++++ V2+ V3 +++++++++++++++++++++
            ' +++ Variabilidad de las aportaciones mensuales +++
            ' +++ Variabilidad de las aportaciones por mes   +++
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesHabitualesAgregados(3).valor(0)
            ReDim Me._IndicesHabitualesAgregados(3).invertido(0)
            ReDim Me._IndicesHabitualesAgregados(3).indeterminacion(0)

            ReDim Me._IndiceV3Agregados.valor(11)
            ReDim Me._IndiceV3Agregados.invertido(11)
            ReDim Me._IndiceV3Agregados.indeterminacion(11)

            For i = 0 To 11
                Me._IndiceV3Agregados.invertido(i) = False
                Me._IndiceV3Agregados.indeterminacion(i) = False
            Next

            Dim desviacionMesNat(11) As Single
            Dim desviacionMesAlt(11) As Single

            Dim CVMesNat(11) As Single
            Dim CVMesAlt(11) As Single

            Dim auxLista1(11) As Single
            Dim auxLista2(11) As Single

            For i = 0 To 11
                auxLista1(i) = 0
                auxLista2(i) = 0
            Next
            naños = Me._AportacionNatAnual.aportacion.Length
            For i = 0 To naños - 1
                For j = 0 To 11
                    auxLista1(j) = auxLista1(j) + Pow(Me._AportacionNatMen.aportacion(i * 12 + j), 2)
                    auxLista2(j) = auxLista2(j) + Me._AportacionNatMen.aportacion(i * 12 + j)
                Next
            Next
            For i = 0 To 11
                desviacionMesNat(i) = (naños * auxLista1(i) + Pow(auxLista2(i), 2)) / (naños * (naños - 1))
                desviacionMesNat(i) = Sqrt(desviacionMesNat(i))
            Next

            For i = 0 To 11
                auxLista1(i) = 0
                auxLista2(i) = 0
            Next
            naños = Me._AportacionAltAnual.aportacion.Length
            For i = 0 To naños - 1
                For j = 0 To 11
                    auxLista1(j) = auxLista1(j) + Pow(Me._AportacionAltMen.aportacion(i * 12 + j), 2)
                    auxLista2(j) = auxLista2(j) + Me._AportacionAltMen.aportacion(i * 12 + j)
                Next
            Next
            For i = 0 To 11
                desviacionMesAlt(i) = (naños * auxLista1(i) + Pow(auxLista2(i), 2)) / (naños * (naños - 1))
                desviacionMesAlt(i) = Sqrt(desviacionMesAlt(i))
            Next

            For i = 0 To 11
                CVMesNat(i) = desviacionMesNat(i) / mediasMesNat(i)
                CVMesAlt(i) = desviacionMesAlt(i) / mediasMesAlt(i)
            Next

            ' Calculo de V3
            For i = 0 To 11
                If (CVMesAlt(i) = 0) And (CVMesNat(i) = 0) Then
                    Me._IndiceV3Agregados.indeterminacion(i) = True
                    Me._IndiceV3Agregados.valor(i) = 1
                ElseIf (CVMesAlt(i) <> 0) And (CVMesNat(i) = 0) Then
                    Me._IndiceV3Agregados.indeterminacion(i) = True
                    Me._IndiceV3Agregados.valor(i) = 0
                ElseIf (CVMesAlt(i) > CVMesNat(i)) Then
                    Me._IndiceV3Agregados.invertido(i) = True
                    Me._IndiceV3Agregados.valor(i) = CVMesNat(i) / CVMesAlt(i)
                Else
                    Me._IndiceV3Agregados.valor(i) = CVMesAlt(i) / CVMesNat(i)
                End If
            Next

            ' Calculo del V2
            acum = 0
            nCalInvertidos = 0
            For i = 0 To 11
                If (CVMesAlt(i) = 0) And (CVMesNat(i) = 0) Then
                    Me._IndicesHabitualesAgregados(3).indeterminacion(0) = True
                    acum = acum + 1
                ElseIf (CVMesAlt(i) <> 0) And (CVMesNat(i) = 0) Then
                    Me._IndicesHabitualesAgregados(3).indeterminacion(0) = True
                    acum = acum + 0
                ElseIf (CVMesAlt(i) > CVMesNat(i)) Then
                    Me._IndicesHabitualesAgregados(3).invertido(0) = True
                    nCalInvertidos += 1
                    acum = acum + CVMesNat(i) / CVMesAlt(i)
                Else
                    acum = acum + CVMesAlt(i) / CVMesNat(i)
                End If
            Next

            ' Ver si es invertido cada uno de los indices
            If ((nCalInvertidos / 12) > 0.5) Then
                Me._IndicesHabitualesAgregados(3).invertido(0) = True
            Else
                Me._IndicesHabitualesAgregados(3).invertido(0) = False
            End If

            Me._IndicesHabitualesAgregados(3).valor(0) = acum / 12
            Me._IndicesHabitualesAgregados(3).calculado = True

            ' ++++  V4 ++++++++
            ReDim Me._IndicesHabitualesAgregados(4).valor(0)
            ReDim Me._IndicesHabitualesAgregados(4).invertido(0)
            ReDim Me._IndicesHabitualesAgregados(4).indeterminacion(0)

            If (Me._HabVariabilidadAlt(0) = 0) And (Me._HabVariabilidadNatReducido = 0) Then
                Me._IndicesHabitualesAgregados(4).indeterminacion(0) = True
                Me._IndicesHabitualesAgregados(4).valor(0) = 1
            ElseIf (Me._HabVariabilidadAlt(0) <> 0) And (Me._HabVariabilidadNatReducido = 0) Then
                Me._IndicesHabitualesAgregados(4).indeterminacion(0) = True
                Me._IndicesHabitualesAgregados(4).valor(0) = 0
            ElseIf (Me._HabVariabilidadAlt(0) > Me._HabVariabilidadNatReducido) Then
                Me._IndicesHabitualesAgregados(4).invertido(0) = True
                Me._IndicesHabitualesAgregados(4).valor(0) = Me._HabVariabilidadNatReducido / Me._HabVariabilidadAlt(0)
            Else
                Me._IndicesHabitualesAgregados(4).valor(0) = Me._HabVariabilidadAlt(0) / Me._HabVariabilidadNatReducido
            End If

            Me._IndicesHabitualesAgregados(4).calculado = True

            ' +++++++++++++++++++++++++++++++++
            ' +++++++++ Estacionalidad ++++++++
            ' +++++++++++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++
            ' ++++++++ E1 (Estacionalidad de Maximos ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesHabitualesAgregados(5).valor(0)
            ReDim Me._IndicesHabitualesAgregados(5).invertido(0)
            ReDim Me._IndicesHabitualesAgregados(5).indeterminacion(0)

            Dim desfase As Single
            For i = 0 To 11
                If (Me._TablaFrecuenciaMaxMin.posMaxNat(i)) Then
                    For j = 0 To i
                        If (Me._TablaFrecuenciaMaxMin.posMaxAlt(j)) Then
                            desfase = i - j
                            Exit For
                        End If
                    Next
                    For j = i To 11
                        If (Me._TablaFrecuenciaMaxMin.posMaxAlt(j)) Then
                            If (desfase < (j - i)) Then
                                desfase = j - i
                            End If
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next

            If (desfase > 6) Then
                desfase = 6
            End If
            desfase = 1 - (desfase / 6)

            'If (desfase = 0) Then
            '    desfase = 1
            'ElseIf (desfase = 1) Then
            '    desfase = 0.83
            'ElseIf (desfase = 2) Then
            '    desfase = 0.67
            'ElseIf (desfase = 3) Then
            '    desfase = 0.5
            'ElseIf (desfase = 4) Then
            '    desfase = 0.33
            'ElseIf (desfase = 5) Then
            '    desfase = 0.17
            'Else
            '    desfase = 0
            'End If



            Me._IndicesHabitualesAgregados(5).valor(0) = desfase

            Me._IndicesHabitualesAgregados(5).calculado = True

            ' +++++++++++++++++++++++++++++++++
            ' ++++++++ E2 (Estacionalidad++++++++++
            ReDim Me._IndicesHabitualesAgregados(6).valor(0)
            ReDim Me._IndicesHabitualesAgregados(6).invertido(0)
            ReDim Me._IndicesHabitualesAgregados(6).indeterminacion(0)

            'Dim desfase As Integer
            For i = 0 To 11
                If (Me._TablaFrecuenciaMaxMin.posMinNat(i)) Then
                    For j = 0 To i
                        If (Me._TablaFrecuenciaMaxMin.posMinAlt(j)) Then
                            desfase = i - j
                            Exit For
                        End If
                    Next
                    For j = i To 11
                        If (Me._TablaFrecuenciaMaxMin.posMinAlt(j)) Then
                            If (desfase < (j - i)) Then
                                desfase = j - i
                            End If
                            'Exit For
                        End If
                    Next
                    Exit For
                End If
            Next

            If (desfase > 6) Then
                desfase = 6
            End If
            desfase = 1 - (desfase / 6)

            'If (desfase = 0) Then
            '    desfase = 1
            'ElseIf (desfase = 1) Then
            '    desfase = 0.83
            'ElseIf (desfase = 2) Then
            '    desfase = 0.67
            'ElseIf (desfase = 3) Then
            '    desfase = 0.5
            'ElseIf (desfase = 4) Then
            '    desfase = 0.33
            'ElseIf (desfase = 5) Then
            '    desfase = 0.17
            'Else
            '    desfase = 0
            'End If

            Me._IndicesHabitualesAgregados(6).valor(0) = desfase

            Me._IndicesHabitualesAgregados(6).calculado = True

        End Sub

        ''' <summary>
        ''' Indices en avenidas
        ''' </summary>
        ''' <remarks>Informe 7d</remarks>
        Private Sub CalcularIndicesAvenidasCASO6()
            ' Casi todos los calculos se basan en parametros, por lo que apenas hay que hacer calculos salvo saber
            ' que datos anteriores usar.

            ReDim Me._IndicesAvenidas(7)
            Dim i As Integer

            ' +++++++++++++++++++++++++++++++++++++
            ' +++++++ IAH7 - I8 +++++++++++++++++++
            ' +++ Magnitud de avenidas maximas ++++
            ' +++++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesAvenidas(0).valor(0)
            ReDim Me._IndicesAvenidas(0).invertido(0)
            ReDim Me._IndicesAvenidas(0).indeterminacion(0)

            For i = 0 To 7
                Me._IndicesAvenidas(i).calculado = False
            Next

            Me._IndicesAvenidas(0).invertido(0) = False
            Me._IndicesAvenidas(0).indeterminacion(0) = False
            Me._IndicesAvenidas(0).calculado = True

            If (Me._AveMagnitudAlt(0) = 0) And (Me._AveMagnitudNat(0) = 0) Then
                Me._IndicesAvenidas(0).valor(0) = 1
                Me._IndicesAvenidas(0).indeterminacion(0) = True
            ElseIf (Me._AveMagnitudAlt(0) <> 0) And (Me._AveMagnitudNat(0) = 0) Then
                Me._IndicesAvenidas(0).valor(0) = 0
                Me._IndicesAvenidas(0).indeterminacion(0) = True
            ElseIf (Me._AveMagnitudAlt(0) > Me._AveMagnitudNat(0)) Then
                Me._IndicesAvenidas(0).valor(0) = Me._AveMagnitudNat(0) / Me._AveMagnitudAlt(0)
                Me._IndicesAvenidas(0).invertido(0) = True
            Else
                Me._IndicesAvenidas(0).valor(0) = Me._AveMagnitudAlt(0) / Me._AveMagnitudNat(0)
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++
            ' +++++++ IAH 8 - I9 ++++++++++++++++++++++++++
            ' +++ Magnitud de caudal generador de lecho +++
            ' +++++++++++++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesAvenidas(1).valor(0)
            ReDim Me._IndicesAvenidas(1).invertido(0)
            ReDim Me._IndicesAvenidas(1).indeterminacion(0)

            Me._IndicesAvenidas(1).invertido(0) = False
            Me._IndicesAvenidas(1).indeterminacion(0) = False
            Me._IndicesAvenidas(1).calculado = True

            ' OJO que puede que no se calcule nada
            If (Me._AveMagnitudNat(1) = -9999) Then
                Me._IndicesAvenidas(1).valor(0) = -9999
                Me._IndicesAvenidas(1).calculado = False
            Else
                If (Me._AveMagnitudAlt(1) = 0) And (Me._AveMagnitudNat(1) = 0) Then
                    Me._IndicesAvenidas(1).valor(0) = 1
                    Me._IndicesAvenidas(1).indeterminacion(0) = True
                ElseIf (Me._AveMagnitudAlt(1) <> 0) And (Me._AveMagnitudNat(1) = 0) Then
                    Me._IndicesAvenidas(1).valor(0) = 0
                    Me._IndicesAvenidas(1).indeterminacion(0) = True
                ElseIf (Me._AveMagnitudAlt(1) > Me._AveMagnitudNat(1)) Then
                    Me._IndicesAvenidas(1).valor(0) = Pow(Me._AveMagnitudNat(1) / Me._AveMagnitudAlt(1), 0.5)
                    Me._IndicesAvenidas(1).invertido(0) = True
                Else
                    Me._IndicesAvenidas(1).valor(0) = Pow(Me._AveMagnitudAlt(1) / Me._AveMagnitudNat(1), 0.5)
                End If
            End If

            ' +++++++++++++++++++++++++++++++
            ' ++++++++ IAH 9 - I10 ++++++++++
            ' +++++++++++++++++++++++++++++++
            ' Frecuencia de caudal de conectividad

            ' T[Qconec NAT] en ALT
            ' ---------------------
            ' T[Qconec NAT] en NAT

            ReDim Me._IndicesAvenidas(2).valor(0)
            ReDim Me._IndicesAvenidas(2).invertido(0)
            ReDim Me._IndicesAvenidas(2).indeterminacion(0)

            Me._IndicesAvenidas(2).invertido(0) = False
            Me._IndicesAvenidas(2).indeterminacion(0) = False
            Me._IndicesAvenidas(2).calculado = True

            If (Me._Ave2TAlt = 0) And (Me._Ave2TNat = 0) Then
                Me._IndicesAvenidas(2).valor(0) = 1
                Me._IndicesAvenidas(2).indeterminacion(0) = True
            ElseIf (Me._Ave2TAlt <> 0) And (Me._Ave2TNat = 0) Then
                Me._IndicesAvenidas(2).valor(0) = 0
                Me._IndicesAvenidas(2).indeterminacion(0) = True
            ElseIf (Me._Ave2TAlt > Me._Ave2TNat) Then
                Me._IndicesAvenidas(2).valor(0) = Me._Ave2TNat / Me._Ave2TAlt
                Me._IndicesAvenidas(2).invertido(0) = True
            Else
                Me._IndicesAvenidas(2).valor(0) = Me._Ave2TAlt / Me._Ave2TNat
            End If

            ' +++++++++++++++++++++++
            ' ++++++++ I11 ++++++++++
            ' +++++++++++++++++++++++
            ReDim Me._IndicesAvenidas(3).valor(0)
            ReDim Me._IndicesAvenidas(3).invertido(0)
            ReDim Me._IndicesAvenidas(3).indeterminacion(0)

            Me._IndicesAvenidas(3).invertido(0) = False
            Me._IndicesAvenidas(3).indeterminacion(0) = False
            Me._IndicesAvenidas(3).calculado = True

            If (Me._AveMagnitudAlt(3) = 0) And (Me._AveMagnitudNat(3) = 0) Then
                Me._IndicesAvenidas(3).valor(0) = 1
                Me._IndicesAvenidas(3).indeterminacion(0) = True
            ElseIf (Me._AveMagnitudAlt(3) <> 0) And (Me._AveMagnitudNat(3) = 0) Then
                Me._IndicesAvenidas(3).valor(0) = 0
                Me._IndicesAvenidas(3).indeterminacion(0) = True
            ElseIf (Me._AveMagnitudAlt(3) > Me._AveMagnitudNat(3)) Then
                Me._IndicesAvenidas(3).valor(0) = Me._AveMagnitudNat(3) / Me._AveMagnitudAlt(3)
                Me._IndicesAvenidas(3).invertido(0) = True
            Else
                Me._IndicesAvenidas(3).valor(0) = Me._AveMagnitudAlt(3) / Me._AveMagnitudNat(3)
            End If

            ' +++++++++++++++++++++++
            ' ++++++++ I12 ++++++++++
            ' +++++++++++++++++++++++
            ReDim Me._IndicesAvenidas(4).valor(0)
            ReDim Me._IndicesAvenidas(4).invertido(0)
            ReDim Me._IndicesAvenidas(4).indeterminacion(0)

            Me._IndicesAvenidas(4).invertido(0) = False
            Me._IndicesAvenidas(4).indeterminacion(0) = False
            Me._IndicesAvenidas(4).calculado = True

            If (Me._AveVariabilidadAlt(0) < 0 Or Me._AveVariabilidadNat(0) < 0) Then
                Me._IndicesAvenidas(4).valor(0) = -9999
                Me._IndicesAvenidas(4).calculado = False
            Else
                If (Me._AveVariabilidadAlt(0) = 0) And (Me._AveVariabilidadNat(0) = 0) Then
                    Me._IndicesAvenidas(4).valor(0) = 1
                    Me._IndicesAvenidas(4).indeterminacion(0) = True
                ElseIf (Me._AveVariabilidadAlt(0) <> 0) And (Me._AveVariabilidadNat(0) = 0) Then
                    Me._IndicesAvenidas(4).valor(0) = 0
                    Me._IndicesAvenidas(4).indeterminacion(0) = True
                ElseIf (Me._AveVariabilidadAlt(0) > Me._AveVariabilidadNat(0)) Then
                    Me._IndicesAvenidas(4).valor(0) = Me._AveVariabilidadNat(0) / Me._AveVariabilidadAlt(0)
                    Me._IndicesAvenidas(4).invertido(0) = True
                Else
                    Me._IndicesAvenidas(4).valor(0) = Me._AveVariabilidadAlt(0) / Me._AveVariabilidadNat(0)
                End If
            End If

            ' +++++++++++++++++++++++
            ' ++++++++ I13 ++++++++++
            ' +++++++++++++++++++++++
            ReDim Me._IndicesAvenidas(5).valor(0)
            ReDim Me._IndicesAvenidas(5).invertido(0)
            ReDim Me._IndicesAvenidas(5).indeterminacion(0)

            Me._IndicesAvenidas(5).invertido(0) = False
            Me._IndicesAvenidas(5).indeterminacion(0) = False
            Me._IndicesAvenidas(5).calculado = True

            If (Me._AveVariabilidadAlt(1) < 0 Or Me._AveVariabilidadNat(1) < 0) Then
                Me._IndicesAvenidas(5).valor(0) = -9999
                Me._IndicesAvenidas(5).calculado = False
            Else
                If (Me._AveVariabilidadAlt(1) = 0) And (Me._AveVariabilidadNat(1) = 0) Then
                    Me._IndicesAvenidas(5).valor(0) = 1
                    Me._IndicesAvenidas(5).indeterminacion(0) = True
                ElseIf (Me._AveVariabilidadAlt(1) <> 0) And (Me._AveVariabilidadNat(1) = 0) Then
                    Me._IndicesAvenidas(5).valor(0) = 0
                    Me._IndicesAvenidas(5).indeterminacion(0) = True
                ElseIf (Me._AveVariabilidadAlt(1) > Me._AveVariabilidadNat(1)) Then
                    Me._IndicesAvenidas(5).valor(0) = Me._AveVariabilidadNat(1) / Me._AveVariabilidadAlt(1)
                    Me._IndicesAvenidas(5).invertido(0) = True
                Else
                    Me._IndicesAvenidas(5).valor(0) = Me._AveVariabilidadAlt(1) / Me._AveVariabilidadNat(1)
                End If
            End If


            ' +++++++++++++++++++++++
            ' ++++++++ I15 ++++++++++
            ' +++++++++++++++++++++++
            ReDim Me._IndicesAvenidas(6).valor(0)
            ReDim Me._IndicesAvenidas(6).invertido(0)
            ReDim Me._IndicesAvenidas(6).indeterminacion(0)

            Me._IndicesAvenidas(6).invertido(0) = False
            Me._IndicesAvenidas(6).indeterminacion(0) = False
            Me._IndicesAvenidas(6).calculado = True

            If (Me._AveDuracionAlt = 0) And (Me._AveDuracionNat = 0) Then
                Me._IndicesAvenidas(6).valor(0) = 1
                Me._IndicesAvenidas(6).indeterminacion(0) = True
            ElseIf (Me._AveDuracionAlt <> 0) And (Me._AveDuracionNat = 0) Then
                Me._IndicesAvenidas(6).valor(0) = 0
                Me._IndicesAvenidas(6).indeterminacion(0) = True
            ElseIf (Me._AveDuracionAlt > Me._AveDuracionNat) Then
                Me._IndicesAvenidas(6).valor(0) = Me._AveDuracionNat / Me._AveDuracionAlt
                Me._IndicesAvenidas(6).invertido(0) = True
            Else
                Me._IndicesAvenidas(6).valor(0) = Me._AveDuracionAlt / Me._AveDuracionNat
            End If

            ' ++++++++++++++++++++++++++++++++++
            ' ++++++++ IAH14 - I16 +++++++++++++
            ' ++++++++++++++++++++++++++++++++++
            ' ++ Nº medio de dias con Q > Q5% ++
            ' ++++++++++++++++++++++++++++++++++
            ' +++ Modificacion del 7/2/08: Cambiar umbral de 10 a 5
            ' ++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesAvenidas(7).valor(0)
            ReDim Me._IndicesAvenidas(7).invertido(0)
            ReDim Me._IndicesAvenidas(7).indeterminacion(0)

            Me._IndicesAvenidas(7).invertido(0) = False
            Me._IndicesAvenidas(7).indeterminacion(0) = False
            Me._IndicesAvenidas(7).calculado = True

            ' +++++++++++++++++++++
            ' Modificación Enero 08
            ' +++++++++++++++++++++
            ReDim Me._IndicesAvenidasI16Meses(11)
            ReDim Me._IndicesAvenidasI16MesesInversos(11)

            Dim acum As Single
            acum = 0
            For i = 0 To 11
                'If (Me._AveEstacionalidadAlt.ndias(i) = 0) And (Me._AveEstacionalidadNat.ndias(i) = 0) Then
                '    acum = acum + 1
                '    Me._IndicesAvenidas(7).indeterminacion(0) = True
                '    'ElseIf (Me._AveEstacionalidadAlt.ndias(i) <> 0) And (Me._AveEstacionalidadNat.ndias(i) = 0) Then
                '    '    acum = acum + 0
                '    '    Me._IndicesAvenidas(7).indeterminacion(0) = True
                '    'ElseIf (Me._AveEstacionalidadAlt.ndias(i) > Me._AveEstacionalidadNat.ndias(i)) Then
                '    'acum = acum + Me._AveEstacionalidadNat.ndias(i) / Me._AveEstacionalidadAlt.ndias(i)
                '    'Me._IndicesAvenidas(7).invertido(0) = True
                'Else
                ' +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
                ' +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
                ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' acum = acum + Me._AveEstacionalidadAlt.ndias(i) / Me._AveEstacionalidadNat.ndias(i)
                ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                Dim dif As Single = Abs(Me._AveEstacionalidadNat.ndias(i) - Me._AveEstacionalidadAlt.ndias(i))

                If (Me._AveEstacionalidadAlt.ndias(i) > Me._AveEstacionalidadNat.ndias(i)) Then
                    Me._IndicesAvenidasI16MesesInversos(i) = True
                Else
                    Me._IndicesAvenidasI16MesesInversos(i) = False
                End If

                ' Cambio Enero 08
                Me._IndicesAvenidasI16Meses(i) = 0

                If (dif <= 5) Then
                    Me._IndicesAvenidasI16Meses(i) = ((5 - dif) / 5)
                    acum = acum + ((5 - dif) / 5)
                End If
                'End If
            Next

            Me._IndicesAvenidas(7).valor(0) = 1 / 12 * acum

        End Sub
        Private Sub CalcularIndicesSequiasCASO6()

            ReDim Me._IndicesSequias(6)
            Dim i As Integer

            ' +++++++++++++++++++++
            ' +++++++ I17 +++++++++
            ' +++++++++++++++++++++
            ReDim Me._IndicesSequias(0).valor(0)
            ReDim Me._IndicesSequias(0).invertido(0)
            ReDim Me._IndicesSequias(0).indeterminacion(0)

            For i = 0 To 6
                Me._IndicesSequias(i).calculado = False
            Next

            Me._IndicesSequias(0).invertido(0) = False
            Me._IndicesSequias(0).indeterminacion(0) = False
            Me._IndicesSequias(0).calculado = True

            If (Me._SeqMagnitudAlt(0) = 0) And (Me._SeqMagnitudNat(0) = 0) Then
                Me._IndicesSequias(0).valor(0) = 1
                Me._IndicesSequias(0).indeterminacion(0) = True
            ElseIf (Me._SeqMagnitudAlt(0) <> 0) And (Me._SeqMagnitudNat(0) = 0) Then
                Me._IndicesSequias(0).valor(0) = 0
                Me._IndicesSequias(0).indeterminacion(0) = True
            ElseIf (Me._SeqMagnitudAlt(0) > Me._SeqMagnitudNat(0)) Then
                Me._IndicesSequias(0).valor(0) = Me._SeqMagnitudNat(0) / Me._SeqMagnitudAlt(0)
                Me._IndicesSequias(0).invertido(0) = True
            Else
                Me._IndicesSequias(0).valor(0) = Me._SeqMagnitudAlt(0) / Me._SeqMagnitudNat(0)
            End If

            ' +++++++++++++++++++++
            ' +++++++ I18 +++++++++
            ' +++++++++++++++++++++
            ReDim Me._IndicesSequias(1).valor(0)
            ReDim Me._IndicesSequias(1).invertido(0)
            ReDim Me._IndicesSequias(1).indeterminacion(0)


            Me._IndicesSequias(1).invertido(0) = False
            Me._IndicesSequias(1).indeterminacion(0) = False
            Me._IndicesSequias(1).calculado = True

            If (Me._SeqMagnitudAlt(1) = 0) And (Me._SeqMagnitudNat(1) = 0) Then
                Me._IndicesSequias(1).valor(0) = 1
                Me._IndicesSequias(1).indeterminacion(0) = True
            ElseIf (Me._SeqMagnitudAlt(1) <> 0) And (Me._SeqMagnitudNat(1) = 0) Then
                Me._IndicesSequias(1).valor(0) = 0
                Me._IndicesSequias(1).indeterminacion(0) = True
            ElseIf (Me._SeqMagnitudAlt(1) > Me._SeqMagnitudNat(1)) Then
                Me._IndicesSequias(1).valor(0) = Me._SeqMagnitudNat(1) / Me._SeqMagnitudAlt(1)
                Me._IndicesSequias(1).invertido(0) = True
            Else
                Me._IndicesSequias(1).valor(0) = Me._SeqMagnitudAlt(1) / Me._SeqMagnitudNat(1)
            End If

            ' +++++++++++++++++++++
            ' +++++++ I19 +++++++++
            ' +++++++++++++++++++++
            ReDim Me._IndicesSequias(2).valor(0)
            ReDim Me._IndicesSequias(2).invertido(0)
            ReDim Me._IndicesSequias(2).indeterminacion(0)

            Me._IndicesSequias(2).invertido(0) = False
            Me._IndicesSequias(2).indeterminacion(0) = False
            Me._IndicesSequias(2).calculado = True

            If (Me._SeqVariabilidadAlt(0) <> -9999 And Me._SeqVariabilidadNat(0) <> -9999) Then
                If (Me._SeqVariabilidadAlt(0) = 0) And (Me._SeqVariabilidadNat(0) = 0) Then
                    Me._IndicesSequias(2).valor(0) = 1
                    Me._IndicesSequias(2).indeterminacion(0) = True
                ElseIf (Me._SeqVariabilidadAlt(0) <> 0) And (Me._SeqVariabilidadNat(0) = 0) Then
                    Me._IndicesSequias(2).valor(0) = 0
                    Me._IndicesSequias(2).indeterminacion(0) = True
                ElseIf (Me._SeqVariabilidadAlt(0) > Me._SeqVariabilidadNat(0)) Then
                    Me._IndicesSequias(2).valor(0) = Me._SeqVariabilidadNat(0) / Me._SeqVariabilidadAlt(0)
                    Me._IndicesSequias(2).invertido(0) = True
                Else
                    Me._IndicesSequias(2).valor(0) = Me._SeqVariabilidadAlt(0) / Me._SeqVariabilidadNat(0)
                End If
            Else
                Me._IndicesSequias(2).valor(0) = -9999
                Me._IndicesSequias(2).calculado = False
            End If

            ' +++++++++++++++++++++
            ' +++++++ I20 +++++++++
            ' +++++++++++++++++++++
            ReDim Me._IndicesSequias(3).valor(0)
            ReDim Me._IndicesSequias(3).invertido(0)
            ReDim Me._IndicesSequias(3).indeterminacion(0)

            Me._IndicesSequias(3).invertido(0) = False
            Me._IndicesSequias(3).indeterminacion(0) = False
            Me._IndicesSequias(3).calculado = True

            If (Me._SeqVariabilidadAlt(1) <> -9999 And Me._SeqVariabilidadNat(1) <> -9999) Then
                If (Me._SeqVariabilidadAlt(1) = 0) And (Me._SeqVariabilidadNat(1) = 0) Then
                    Me._IndicesSequias(3).valor(0) = 1
                    Me._IndicesSequias(3).indeterminacion(0) = True
                ElseIf (Me._SeqVariabilidadAlt(1) <> 0) And (Me._SeqVariabilidadNat(1) = 0) Then
                    Me._IndicesSequias(3).valor(0) = 0
                    Me._IndicesSequias(3).indeterminacion(0) = True
                ElseIf (Me._SeqVariabilidadAlt(1) > Me._SeqVariabilidadNat(1)) Then
                    Me._IndicesSequias(3).valor(0) = Me._SeqVariabilidadNat(1) / Me._SeqVariabilidadAlt(1)
                    Me._IndicesSequias(3).invertido(0) = True
                Else
                    Me._IndicesSequias(3).valor(0) = Me._SeqVariabilidadAlt(1) / Me._SeqVariabilidadNat(1)
                End If
            Else
                Me._IndicesSequias(3).valor(0) = -9999
                Me._IndicesSequias(3).calculado = False
            End If


            ' +++++++++++++++++++++
            ' +++++++ I22 +++++++++
            ' +++++++++++++++++++++
            ReDim Me._IndicesSequias(4).valor(0)
            ReDim Me._IndicesSequias(4).invertido(0)
            ReDim Me._IndicesSequias(4).indeterminacion(0)



            Me._IndicesSequias(4).invertido(0) = False
            Me._IndicesSequias(4).indeterminacion(0) = False
            Me._IndicesSequias(4).calculado = True

            If (Me._SeqDuracionAlt(0) = 0) And (Me._SeqDuracionNat(0) = 0) Then
                Me._IndicesSequias(4).valor(0) = 1
                Me._IndicesSequias(4).indeterminacion(0) = True
            ElseIf (Me._SeqDuracionAlt(0) <> 0) And (Me._SeqDuracionNat(0) = 0) Then
                Me._IndicesSequias(4).valor(0) = 0
                Me._IndicesSequias(4).indeterminacion(0) = True
            ElseIf (Me._SeqDuracionAlt(0) > Me._SeqDuracionNat(0)) Then
                Me._IndicesSequias(4).valor(0) = Me._SeqDuracionNat(0) / Me._SeqDuracionAlt(0)
                Me._IndicesSequias(4).invertido(0) = True
            Else
                Me._IndicesSequias(4).valor(0) = Me._SeqDuracionAlt(0) / Me._SeqDuracionNat(0)
            End If

            ' ++++++++++++++++++++++++++++
            ' +++++++ IAH 20 - I23 +++++++
            ' ++++++++++++++++++++++++++++
            ' +++ Modificacion del 7/2/08: Cambiar umbral de 10 a 5
            ' ++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesSequias(5).valor(0)
            ReDim Me._IndicesSequias(5).invertido(0)
            ReDim Me._IndicesSequias(5).indeterminacion(0)

            Me._IndicesSequias(5).invertido(0) = False
            Me._IndicesSequias(5).indeterminacion(0) = False
            Me._IndicesSequias(5).calculado = True

            ReDim Me._IndicesSequiasI23Meses(11)
            ReDim Me._IndicesSequiasI23MesesInversos(11)

            'If (Not Me._usarCoe) Then
            '    If ((365 - Me._SeqDuracionAlt(1)) = 0) And ((365 - Me._SeqDuracionNat(1)) = 0) Then
            '        Me._IndicesSequias(5).valor(0) = 1
            '        Me._IndicesSequias(5).indeterminacion(0) = True
            '    ElseIf ((365 - Me._SeqDuracionAlt(1)) <> 0) And ((365 - Me._SeqDuracionNat(1)) = 0) Then
            '        Me._IndicesSequias(5).valor(0) = 0
            '        Me._IndicesSequias(5).indeterminacion(0) = True
            '    ElseIf ((365 - Me._SeqDuracionAlt(1)) > (365 - Me._SeqDuracionNat(1))) Then
            '        Me._IndicesSequias(5).valor(0) = (365 - Me._SeqDuracionNat(1)) / (365 - Me._SeqDuracionAlt(1))
            '        Me._IndicesSequias(5).invertido(0) = True
            '    Else
            '        Me._IndicesSequias(5).valor(0) = (365 - Me._SeqDuracionAlt(1)) / (365 - Me._SeqDuracionNat(1))
            '    End If
            'Else
            '    Dim sumNulos As Integer

            '    sumNulos = 0
            '    For i = 0 To Me._datos.SerieNatDiaria.nAños - 1
            '        If ((365 - Me._nDiasNulosAlt(i)) = 0) And ((365 - Me._nDiasNulosNat(i)) = 0) Then
            '            Me._IndicesSequias(5).valor(0) = 1
            '            Me._IndicesSequias(5).indeterminacion(0) = True
            '        ElseIf ((365 - Me._nDiasNulosAlt(i)) <> 0) And ((365 - Me._nDiasNulosNat(i)) = 0) Then
            '            Me._IndicesSequias(5).valor(0) = 0
            '            Me._IndicesSequias(5).indeterminacion(0) = True
            '        ElseIf ((365 - Me._nDiasNulosAlt(i)) > (365 - Me._nDiasNulosNat(i))) Then
            '            sumNulos = sumNulos + (365 - Me._nDiasNulosNat(i)) / (365 - Me._nDiasNulosAlt(i))
            '            Me._IndicesSequias(5).invertido(0) = True
            '        Else
            '            sumNulos = sumNulos + (365 - Me._nDiasNulosAlt(i)) / (365 - Me._nDiasNulosNat(i))
            '        End If
            '        'sumNulos = sumNulos + (365 - Me._nDiasNulosAlt(i)) / (365 - Me._nDiasNulosNat(i))
            '    Next

            '    Me._IndicesSequias(5).valor(0) = (1 / Me._datos.SerieNatDiaria.nAños) * sumNulos
            'End If

            ' +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
            ' +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
            ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Dim acum As Single
            For i = 0 To 11
                'If (Me._SeqDuracionCerosMesAlt.ndias(i) = 0) And (Me._SeqDuracionCerosMesNat.ndias(i) = 0) Then
                '    acum = acum + 1
                '    Me._IndicesSequias(5).indeterminacion(0) = True
                'ElseIf (Me._SeqDuracionCerosMesAlt.ndias(i) <> 0) And (Me._SeqDuracionCerosMesNat.ndias(i) = 0) Then
                '    acum = acum + 0
                '    Me._IndicesSequias(5).indeterminacion(0) = True
                'Else
                ' +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
                ' +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
                ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                Dim dif As Single = Abs(Me._SeqDuracionCerosMesNat.ndias(i) - Me._SeqDuracionCerosMesAlt.ndias(i))

                If (Me._SeqDuracionCerosMesAlt.ndias(i) > Me._SeqDuracionCerosMesNat.ndias(i)) Then
                    Me._IndicesSequiasI23MesesInversos(i) = True
                Else
                    Me._IndicesSequiasI23MesesInversos(i) = False
                End If

                Me._IndicesSequiasI23Meses(i) = 0

                If (dif <= 5) Then
                    Me._IndicesSequiasI23Meses(i) = ((5 - dif) / 5)
                    acum = acum + ((5 - dif) / 5)
                End If
                'End If
            Next

            Me._IndicesSequias(5).valor(0) = 1 / 12 * acum

            ' +++++++++++++++++++
            ' +++++++ I24 +++++++
            ' +++++++++++++++++++
            ' +++ Modificacion del 7/2/08: Cambiar umbral de 10 a 5
            ' ++++++++++++++++++++++++++++++++++
            ReDim Me._IndicesSequias(6).valor(0)
            ReDim Me._IndicesSequias(6).invertido(0)
            ReDim Me._IndicesSequias(6).indeterminacion(0)

            Me._IndicesSequias(6).invertido(0) = False
            Me._IndicesSequias(6).indeterminacion(0) = False
            Me._IndicesSequias(6).calculado = True

            ReDim Me._IndicesSequiasI24Meses(11)
            ReDim Me._IndicesSequiasI24MesesInversos(11)

            acum = 0
            For i = 0 To 11
                ' If (Me._SeqEstacionalidadAlt.ndias(i) = 0) And (Me._SeqEstacionalidadNat.ndias(i) = 0) Then
                '    acum = acum + 1
                '    Me._IndicesSequias(6).indeterminacion(0) = True
                '    'ElseIf (Me._SeqEstacionalidadAlt.ndias(i) <> 0) And (Me._SeqEstacionalidadNat.ndias(i) = 0) Then
                '    '    acum = acum + 0
                '    '    Me._IndicesSequias(6).indeterminacion(0) = True
                '    'ElseIf (Me._SeqEstacionalidadAlt.ndias(i) > Me._SeqEstacionalidadNat.ndias(i)) Then
                '    'acum = acum + Me._SeqEstacionalidadNat.ndias(i) / Me._SeqEstacionalidadAlt.ndias(i)
                '    'Me._IndicesSequias(6).invertido(0) = True
                'Else
                ' +++++++ CAMBIO: 22/11/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' +++ Se nos manda un doc con los cambios en los indices 16, 24, 23
                ' +++ Este cambio afecta a la operacion que se va acumulando en cada uno de los meses
                ' ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                ' +++++++ CAMBIO: 28/12/07 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' +++ Otro documento, en que se cambia el umbral de 20 por otro de 10
                ' +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                Dim dif As Single = Abs(Me._SeqEstacionalidadNat.ndias(i) - Me._SeqEstacionalidadAlt.ndias(i))

                If (Me._SeqEstacionalidadAlt.ndias(i) > Me._SeqEstacionalidadNat.ndias(i)) Then
                    Me._IndicesSequiasI24MesesInversos(i) = True
                Else
                    Me._IndicesSequiasI24MesesInversos(i) = False
                End If

                Me._IndicesSequiasI24Meses(i) = 0

                'If (dif <= 20) Then
                'acum = acum + ((20 - dif) / 20)
                'End If
                If (dif <= 5) Then
                    Me._IndicesSequiasI24Meses(i) = ((5 - dif) / 5)
                    acum = acum + ((5 - dif) / 5)
                End If

                'End If
            Next

            Me._IndicesSequias(6).valor(0) = 1 / 12 * acum
        End Sub
        ''' <summary>
        ''' Esto es el IAG
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CalcularIndiceAlteracionGlobalHabituales()
            Dim i, j As Integer
            Dim aux1(), aux2() As Single
            Dim nCal As Integer

            ReDim aux1(2)
            ReDim aux2(2)
            ReDim Me._IndiceIAG(3)

            For i = 0 To 2
                aux1(i) = 0
                aux2(i) = 0
            Next

            nCal = 0
            For i = 0 To 6
                If (Me._IndicesHabituales(i).calculado) Then
                    nCal = nCal + 1
                    For j = 0 To 2
                        aux1(j) = aux1(j) + Me._IndicesHabituales(i).valor(j)
                        aux2(j) = aux2(j) + Math.Pow(Me._IndicesHabituales(i).valor(j), 2)
                    Next
                End If
            Next

            For i = 0 To 2
                Me._IndiceIAG(i) = (Pow(aux1(i), 2) - aux2(i)) / (nCal * (nCal - 1))
            Next
            Me._IndiceIAG(3) = (Me._IndiceIAG(0) + Me._IndiceIAG(1) + Me._IndiceIAG(2)) / 3
        End Sub
        Private Sub CalcularIndiceAlteracionGlobalHabitualesAgregados()
            Dim i As Integer
            Dim aux1, aux2 As Single
            Dim nCal As Integer

            nCal = 0
            aux1 = 0
            aux2 = 0
            For i = 0 To 6
                If (Me._IndicesHabitualesAgregados(i).calculado) Then
                    nCal = nCal + 1
                    'For j = 0 To 2
                    aux1 = aux1 + Me._IndicesHabitualesAgregados(i).valor(0)
                    aux2 = aux2 + Math.Pow(Me._IndicesHabitualesAgregados(i).valor(0), 2)
                    'Next
                End If
            Next

            Me._IndiceIAG_Agregados = (Pow(aux1, 2) - aux2) / (nCal * (nCal - 1))

        End Sub
        Private Sub CalcularIndiceAlteracionGlobalAvenidas()
            Dim i As Integer
            Dim aux1, aux2 As Single
            Dim nCal As Integer

            nCal = 0
            For i = 0 To 6
                If (Me._IndicesAvenidas(i).calculado) Then
                    nCal = nCal + 1
                    aux1 = aux1 + Me._IndicesAvenidas(i).valor(0)
                    aux2 = aux2 + Math.Pow(Me._IndicesAvenidas(i).valor(0), 2)
                End If
            Next

            Me._IndiceIAG_Ave = (Pow(aux1, 2) - aux2) / (nCal * (nCal - 1))
        End Sub
        Private Sub CalcularIndiceAlteracionGlobalSequias()
            Dim i As Integer
            Dim aux1, aux2 As Single
            Dim nCal As Integer

            nCal = 0
            For i = 0 To 6
                If (Me._IndicesSequias(i).calculado) Then
                    nCal = nCal + 1
                    aux1 = aux1 + Me._IndicesSequias(i).valor(0)
                    aux2 = aux2 + Math.Pow(Me._IndicesSequias(i).valor(0), 2)
                End If
            Next

            Me._IndiceIAG_Seq = (Pow(aux1, 2) - aux2) / (nCal * (nCal - 1))
        End Sub
#End Region

#Region "Calculo de Régimen"

        Private Sub CalcularRegimenNatural()

            Dim percentil10() As Single
            Dim percentil90() As Single
            Dim mediana() As Single

            Dim aportacionesMen()() As Single
            ReDim aportacionesMen(11)

            For i As Integer = 0 To 11
                ReDim aportacionesMen(i)(Me._AportacionNatAnual.año.Length - 1)
            Next

            For i As Integer = 0 To Me._AportacionNatAnual.año.Length - 1
                For j As Integer = 0 To 11
                    aportacionesMen(j)(i) = Me._AportacionNatMen.aportacion(i * 12 + j)
                Next
            Next

            For i As Integer = 0 To 11
                Array.Sort(aportacionesMen(i))
                Array.Reverse(aportacionesMen(i))
            Next

            Dim ultimos As ArrayList = New ArrayList()
            Dim primeros As ArrayList = New ArrayList()

            ' 10 es el mes de inicio, lo que nos permite 
            ' variar el mes de inicio del año hidrologico
            For i As Integer = 0 To 12 - Me._datos.mesInicio
                ultimos.Add(aportacionesMen(i))
            Next

            For i As Integer = (12 - Me._datos.mesInicio + 1) To 11
                primeros.Add(aportacionesMen(i))
            Next

            primeros.AddRange(ultimos)

            Dim p10 As Integer = -1
            Dim p90 As Integer = -1

            Dim aux As Single
            Dim aux2 As Single
            For i As Integer = 0 To Me._AportacionNatAnual.año.Length - 1
                aux = (i / Me._AportacionNatAnual.año.Length) * 100
                If (p10 = -1) Then
                    If (aux >= 10) Then
                        p10 = i - 1
                    End If
                End If
                If (aux >= 90) Then
                    p90 = i - 1
                    Exit For
                End If
            Next

            ReDim percentil10(11)
            ReDim percentil90(11)
            ReDim mediana(11)
            Dim p0 As Integer
            Dim p1 As Integer

            For i As Integer = 0 To 11
                aux = (p10 / Me._AportacionNatAnual.año.Length) * 100
                aux2 = ((p10 + 1) / Me._AportacionNatAnual.año.Length) * 100
                percentil10(i) = (10 - aux) / (aux2 - aux) * (aportacionesMen(i)(p10 + 1) - aportacionesMen(i)(p10)) + aportacionesMen(i)(p10)
                aux = (p90 / Me._AportacionNatAnual.año.Length) * 100
                aux2 = ((p90 + 1) / Me._AportacionNatAnual.año.Length) * 100
                percentil90(i) = (90 - aux) / (aux2 - aux) * (aportacionesMen(i)(p90 + 1) - aportacionesMen(i)(p90)) + aportacionesMen(i)(p90)

                p0 = (Me._AportacionNatAnual.año.Length / 2) - 1
                p1 = p0 + 1
                If ((Me._AportacionNatAnual.año.Length Mod 2) <> 0) Then
                    mediana(i) = aportacionesMen(i)(Me._AportacionNatAnual.año.Length / 2)
                Else
                    mediana(i) = (aportacionesMen(i)(p0) + aportacionesMen(i)(p1)) / 2
                End If

            Next

            Me._percentil10 = percentil10
            Me._percentil90 = percentil90
            Me._medianaMenNat = mediana

        End Sub

        Private Sub CalcularRegimenAlterado()
            Dim mediana() As Single
            Dim cumple() As Integer

            Dim aportacionesMen()() As Single
            ReDim aportacionesMen(11)

            For i As Integer = 0 To 11
                ReDim aportacionesMen(i)(Me._AportacionAltAnual.año.Length - 1)
            Next

            For i As Integer = 0 To Me._AportacionAltAnual.año.Length - 1
                For j As Integer = 0 To 11
                    aportacionesMen(j)(i) = Me._AportacionAltMen.aportacion(i * 12 + j)
                Next
            Next

            For i As Integer = 0 To 11
                Array.Sort(aportacionesMen(i))
                Array.Reverse(aportacionesMen(i))
            Next

            Dim ultimos As ArrayList = New ArrayList()
            Dim primeros As ArrayList = New ArrayList()

            ' 10 es el mes de inicio, lo que nos permite 
            ' variar el mes de inicio del año hidrologico
            For i As Integer = 0 To 12 - Me._datos.mesInicio
                ultimos.Add(aportacionesMen(i))
            Next

            For i As Integer = (12 - Me._datos.mesInicio + 1) To 11
                primeros.Add(aportacionesMen(i))
            Next

            primeros.AddRange(ultimos)

            ReDim mediana(11)

            Dim p0 As Integer
            Dim p1 As Integer

            For i As Integer = 0 To 11

                p0 = (Me._AportacionAltAnual.año.Length / 2) - 1
                p1 = p0 + 1
                If ((Me._AportacionAltAnual.año.Length Mod 2) <> 0) Then
                    mediana(i) = aportacionesMen(i)(Me._AportacionAltAnual.año.Length / 2)
                Else
                    mediana(i) = (aportacionesMen(i)(p0) + aportacionesMen(i)(p1)) / 2
                End If
            Next

            ReDim cumple(11)
            For i As Integer = 0 To Me._AportacionAltAnual.año.Length - 1
                For j As Integer = 0 To 11
                    If ((Me._percentil10(j) >= aportacionesMen(j)(i)) And (aportacionesMen(j)(i) >= Me._percentil90(j))) Then
                        cumple(j) += 1
                    End If
                Next
            Next

            Me._mesesQueCumplen = cumple
            Me._medianaMenAlt = mediana

        End Sub

        Private Sub CalcularRegimenNaturalAnual()

            Dim medianaNat As Single
            Dim percentil10 As Single
            Dim percentil90 As Single

            Dim aporOrd() As Single
            aporOrd = Me._AportacionNatAnual.aportacion.Clone()
            Array.Sort(aporOrd)
            Array.Reverse(aporOrd)

            'If ((aporOrd.Length Mod 2) = 0) Then
            '    medianaNat = (aporOrd((aporOrd.Length / 2) - 1) + aporOrd(aporOrd.Length / 2)) / 2
            'Else
            '    medianaNat = aporOrd((aporOrd.Length - 1) / 2)
            'End If

            Dim p10 As Integer = -1
            Dim p90 As Integer = -1

            Dim aux As Single
            Dim aux2 As Single
            For i As Integer = 0 To aporOrd.Length - 1
                aux = (i / aporOrd.Length) * 100
                If (p10 = -1) Then
                    If (aux >= 10) Then
                        p10 = i - 1
                    End If
                End If
                If (aux >= 90) Then
                    p90 = i - 1
                    Exit For
                End If
            Next

            aux = (p10 / aporOrd.Length) * 100
            aux2 = ((p10 + 1) / aporOrd.Length) * 100
            percentil10 = (10 - aux) / (aux2 - aux) * (aporOrd(p10 + 1) - aporOrd(p10)) + aporOrd(p10)
            aux = (p90 / aporOrd.Length) * 100
            aux2 = ((p90 + 1) / aporOrd.Length) * 100
            percentil90 = (90 - aux) / (aux2 - aux) * (aporOrd(p90 + 1) - aporOrd(p90)) + aporOrd(p90)

            Dim p0 As Integer
            Dim p1 As Integer
            p0 = (aporOrd.Length / 2) - 1
            p1 = p0 + 1
            If ((Me._AportacionNatAnual.año.Length Mod 2) <> 0) Then
                medianaNat = aporOrd(Me._AportacionNatAnual.año.Length / 2)
            Else
                medianaNat = (aporOrd(p0) + aporOrd(p1)) / 2
            End If

            Me._percentil10Anual = percentil10
            Me._percentil90Anual = percentil90
            Me._medianaAnualNat = medianaNat

        End Sub

        Private Sub CalcularRegimenAlteradoAnual()
            Dim medianaAlt As Single
            Dim cumple As Integer

            Dim aporOrd() As Single
            aporOrd = Me._AportacionAltAnual.aportacion.Clone()
            Array.Sort(aporOrd)
            Array.Reverse(aporOrd)

            If ((aporOrd.Length Mod 2) = 0) Then
                medianaAlt = (aporOrd((aporOrd.Length / 2) - 1) + aporOrd(aporOrd.Length / 2)) / 2
            Else
                medianaAlt = aporOrd((aporOrd.Length - 1) / 2)
            End If

            cumple = 0

            For i As Integer = 0 To aporOrd.Length - 1

                If ((Me._percentil10Anual >= aporOrd(i)) And (aporOrd(i) >= Me._percentil90Anual)) Then
                    cumple += 1
                End If
            Next

            Me._anyosQueCumplen = cumple
            Me._medianaAnualAlt = medianaAlt

        End Sub

#End Region

#Region "Regimen ambiental"

        Private Sub CalcularReferencias()
            Dim nAños As Integer = Me._datos.SerieNatDiaria.nAños
            'Dim _1QMin As Single
            'Dim _7QMin As Single
            'Dim _10QMin As Single

            ' Calcular Minimos
            Dim listaMinDiarios() As Single
            ReDim listaMinDiarios(nAños - 1)

            Dim añoActual As Integer
            Dim pos As Integer

            ' ---------------------------------------------------------
            ' -------  Calculo de Qmin  -------------------------------
            ' ---------------------------------------------------------

            ' Calcular 1Qmin
            ' --------------
            pos = 0
            listaMinDiarios(0) = 999999999
            añoActual = Me._datos.SerieNatDiaria.dia(0).Year
            Dim i As Integer
            Dim j As Integer
            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                ' --------------------------------------------------------
                ' Usa la ventana de 1 dia
                ' Si el año es diferente cambio donde almacenar el maximo
                ' --------------------------------------------------------
                If (Me._datos.SerieNatDiaria.dia(i).Day = 1 And Me._datos.SerieNatDiaria.dia(i).Month = Me._datos.mesInicio And Me._datos.SerieNatDiaria.dia(i).Year <> añoActual) Then
                    pos = pos + 1
                    listaMinDiarios(pos) = 999999999
                    añoActual = Me._datos.SerieNatDiaria.dia(i).Year
                End If
                If (listaMinDiarios(pos) > Me._datos.SerieNatDiaria.caudalDiaria(i)) Then
                    listaMinDiarios(pos) = Me._datos.SerieNatDiaria.caudalDiaria(i)
                End If
            Next

            _1QMin = 999999999
            For i = 0 To listaMinDiarios.Length - 1
                If (_1QMin > listaMinDiarios(i)) Then
                    _1QMin = listaMinDiarios(i)
                End If
            Next

            ' Calcular 7Qmin
            ' --------------
            ' Ahora se usan ventanas de 7 dias para calcular el minimo
            pos = 0
            Dim listaMinDiarios7() As Single
            ReDim listaMinDiarios7(nAños - 1)
            listaMinDiarios7(0) = 999999999
            añoActual = Me._datos.SerieNatDiaria.dia(0).Year
            Dim media As Single
            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                If (i + 6 > Me._datos.SerieNatDiaria.dia.Length - 1) Then
                    Exit For
                End If
                ' Si el año es diferente al final de la ventana cambio donde almacenar el maximo
                If (Me._datos.SerieNatDiaria.dia(i + 6).Day = 1 And Me._datos.SerieNatDiaria.dia(i + 6).Month = Me._datos.mesInicio And Me._datos.SerieNatDiaria.dia(i + 6).Year <> añoActual) Then
                    pos = pos + 1
                    listaMinDiarios7(pos) = 999999999
                    añoActual = Me._datos.SerieNatDiaria.dia(i).Year
                End If
                ' Hado la media de la ventana de 7 dias
                media = 0
                For j = 0 To 6
                    media += Me._datos.SerieNatDiaria.caudalDiaria(i + j)
                Next
                media = media / 7

                If (listaMinDiarios7(pos) > media) Then
                    listaMinDiarios7(pos) = media
                End If
            Next

            _7QMin = 999999999
            For i = 0 To listaMinDiarios7.Length - 1
                If (_7QMin > listaMinDiarios7(i)) Then
                    _7QMin = listaMinDiarios7(i)
                End If
            Next

            ' Calcular 15Qmin
            ' --------------
            ' Ahora se usan ventanas de 10 dias para calcular el minimo
            pos = 0
            Dim listaMinDiarios15() As Single
            ReDim listaMinDiarios15(nAños - 1)
            listaMinDiarios15(0) = 999999999
            añoActual = Me._datos.SerieNatDiaria.dia(0).Year
            'Dim media As Single
            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                If (i + 14 > Me._datos.SerieNatDiaria.dia.Length - 1) Then
                    Exit For
                End If
                ' Si el año es diferente al final de la ventana cambio donde almacenar el maximo
                If (Me._datos.SerieNatDiaria.dia(i + 14).Day = 1 And Me._datos.SerieNatDiaria.dia(i + 14).Month = Me._datos.mesInicio And Me._datos.SerieNatDiaria.dia(i + 14).Year <> añoActual) Then
                    pos = pos + 1
                    listaMinDiarios15(pos) = 999999999
                    añoActual = Me._datos.SerieNatDiaria.dia(i).Year
                End If
                ' Hado la media de la ventana de 15 dias
                media = 0
                For j = 0 To 14
                    media += Me._datos.SerieNatDiaria.caudalDiaria(i + j)
                Next
                media = media / 14

                If (listaMinDiarios15(pos) > media) Then
                    listaMinDiarios15(pos) = media
                End If
            Next

            _15QMin = 999999999
            For i = 0 To listaMinDiarios15.Length - 1
                If (_15QMin > listaMinDiarios15(i)) Then
                    _15QMin = listaMinDiarios15(i)
                End If
            Next

            ' -----------------------------------------------------
            ' ---- Calculo de Q de retorno ------------------------
            ' -----------------------------------------------------
            'Dim _7QRetorno() As Single
            'Dim _10QRetorno() As Single
            ReDim _7QRetorno(0)
            ReDim _10QRetorno(0)

            Me.AjusteLogPearsonIII(listaMinDiarios7.Length, listaMinDiarios7, _7QRetorno)
            Me.AjusteLogPearsonIII(listaMinDiarios15.Length, listaMinDiarios15, _10QRetorno)

            ' Arreglar el tema de la posicion 0 del array
            _7QRetorno(0) = _7QRetorno(1)
            _7QRetorno(1) = _7QRetorno(2)
            _7QRetorno(2) = _7QRetorno(3)
            ReDim Preserve _7QRetorno(2)
            _10QRetorno(0) = _10QRetorno(1)
            _10QRetorno(1) = _10QRetorno(2)
            _10QRetorno(2) = _10QRetorno(3)
            ReDim Preserve _10QRetorno(2)

            ' ---------------------------------------------------------
            ' ------- Calculo de MnQMin -------------------------------
            ' ---------------------------------------------------------
            'Dim _mnQ() As Single
            ReDim _mnQ(0)
            'Dim _mnQ5 As Single
            'Dim _mnQ10 As Single

            Dim _mnAños() As Single
            ReDim _mnAños(Me._AportacionNatAnualOrdAños.año.Length - 1)
            Dim min As Single
            Dim iMin As Integer
            Dim iDias As Integer

            Dim esCoe As Integer

            For i = 0 To Me._AportacionNatAnualOrdAños.año.Length - 1
                'For i = 0 To Me._datos.SerieNatDiaria.nAños - 1
                esCoe = False
                For aux As Integer = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                    If (Me._datos.SerieNatDiaria.dia(aux).Year = Me._AportacionNatAnualOrdAños.año(i) And _
                        Me._datos.SerieNatDiaria.dia(aux).Month = Me._datos.mesInicio) Then
                        esCoe = True
                        Exit For
                    End If
                Next
                If (Not esCoe) Then
                    Continue For
                End If
                min = 9999999
                ' Mes de menor aportacion
                For j = 0 To 11
                    If min > Me._AportacionNatMen.aportacion((i * 12) + j) Then
                        min = Me._AportacionNatMen.aportacion((i * 12) + j)
                        iMin = j
                    End If
                Next
                ' Ya conozco el mes minimo
                ' 
                ' Ahora busco el mes/año dentro de la lista de datos diarios (que es una ristra de numeros)
                iDias = 0
                While (Me._datos.SerieNatDiaria.dia(iDias).Year <> Me._AportacionNatAnualOrdAños.año(i))
                    iDias += 1
                End While
                While (Me._datos.SerieNatDiaria.dia(iDias).Month <> Me._AportacionNatMen.mes((i * 12) + iMin).Month)
                    iDias += 1
                End While
                ' Saco los valores diarios (28-29-30-31 dias) posibles de un mes
                Dim valoresMesMinimo As ArrayList = New ArrayList()
                While (Me._datos.SerieNatDiaria.dia(iDias).Month = Me._AportacionNatMen.mes((i * 12) + iMin).Month)
                    valoresMesMinimo.Add(Me._datos.SerieNatDiaria.caudalDiaria(iDias))
                    iDias += 1
                    If (iDias = Me._datos.SerieNatDiaria.dia.Length) Then
                        Exit While
                    End If
                End While
                ' Hallar la mediana
                Dim posMediana As Integer = valoresMesMinimo.Count / 2
                _mnAños(i) = valoresMesMinimo.Item(posMediana)
            Next

            Me.AjusteLogPearsonIII(_mnAños.Length, _mnAños, _mnQ)
            _mnQ(0) = _mnQ(1)
            _mnQ(1) = _mnQ(2)
            _mnQ(2) = _mnQ(3)
            ReDim Preserve _mnQ(2)

        End Sub
#End Region


#Region "Funciones auxiliares"
        ''' <summary>
        ''' Calcula la aportacion Mensual de los caudales
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CalcularAportacionMENSUAL()
            Dim aux As SerieMensual
            Dim i As Integer
            If (Me._datos.SerieNatMensual.caudalMensual Is Nothing) Then
                aux = Me._SerieNatMensualCalculada
            Else
                aux = Me._datos.SerieNatMensual
            End If

            ReDim Me._AportacionNatMen.mes(aux.mes.Length - 1)
            ReDim Me._AportacionNatMen.aportacion(aux.caudalMensual.Length - 1)

            For i = 0 To aux.mes.Length - 1
                Me._AportacionNatMen.mes(i) = aux.mes(i)
                Me._AportacionNatMen.aportacion(i) = aux.caudalMensual(i) '(86400 * aux.caudalMensual(i)) / 1000000
            Next

        End Sub

        ''' <summary>
        ''' Calcula la aportacion Mensual de los caudales Alterada
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CalcularAportacionMENSUALAlterada()
            Dim aux As SerieMensual
            Dim i As Integer
            If (Me._datos.SerieAltMensual.caudalMensual Is Nothing) Then
                aux = Me._SerieAltMensualCalculada
            Else
                aux = Me._datos.SerieAltMensual
            End If

            ReDim Me._AportacionAltMen.mes(aux.mes.Length - 1)
            ReDim Me._AportacionAltMen.aportacion(aux.caudalMensual.Length - 1)

            For i = 0 To aux.mes.Length - 1
                Me._AportacionAltMen.mes(i) = aux.mes(i)
                Me._AportacionAltMen.aportacion(i) = aux.caudalMensual(i) '(86400 * aux.caudalMensual(i)) / 1000000
            Next
        End Sub

        ''' <summary>
        ''' Interpola los valores de caudal mensuales
        ''' </summary>
        ''' <returns>Devuelve una serie mensual</returns>
        ''' <remarks>Espere que datos tenga al menos los diarios</remarks>
        Private Function CalcularSerieMENSUAL() As SerieMensual
            Dim salida As SerieMensual
            Dim i As Integer

            'Dim diasMes As Integer
            Dim mes As Integer = -1
            Dim año As Integer = -1
            Dim pos As Integer = 0      ' Posicion en la lista de mese
            Dim d As Date

            salida = Nothing

            For i = 0 To Me._datos.SerieNatDiaria.dia.Length - 1
                ' Cambio de mes
                If (mes <> Me._datos.SerieNatDiaria.dia(i).Month) Then

                    mes = Me._datos.SerieNatDiaria.dia(i).Month

                    If (salida.caudalMensual Is Nothing) Then
                        ReDim Preserve salida.caudalMensual(0)
                        ReDim Preserve salida.mes(0)
                    Else
                        ReDim Preserve salida.caudalMensual(salida.caudalMensual.Length)
                        ReDim Preserve salida.mes(salida.mes.Length)
                        'pos = pos + 1
                    End If
                    salida.nMeses = salida.nMeses + 1
                    pos = salida.caudalMensual.Length - 1

                    If (año <> Me._datos.SerieNatDiaria.dia(i).Year) Then
                        año = Me._datos.SerieNatDiaria.dia(i).Year
                        salida.nAños = salida.nAños + 1
                    End If

                    d = New Date(año, mes, 1)

                    salida.mes(pos) = d
                    salida.caudalMensual(pos) = 0
                End If

                salida.caudalMensual(pos) = salida.caudalMensual(pos) + Me._datos.SerieNatDiaria.caudalDiaria(i) ' / Date.DaysInMonth(Me._datos.SerieNatDiaria.dia(i).Year, Me._datos.SerieNatDiaria.dia(i).Month)
            Next
            Return salida
        End Function

        ''' <summary>
        ''' Interpola los valores de caudal mensuales Alterados
        ''' </summary>
        ''' <returns>Devuelve una serie mensual</returns>
        ''' <remarks>Espere que datos tenga al menos los diarios</remarks>
        Private Function CalcularSerieMENSUALAlterada() As SerieMensual
            Dim salida As SerieMensual
            Dim i As Integer

            'Dim diasMes As Integer
            Dim mes As Integer = -1
            Dim año As Integer = -1
            Dim pos As Integer = 0      ' Posicion en la lista de mese
            Dim d As Date

            salida = Nothing

            For i = 0 To Me._datos.SerieAltDiaria.dia.Length - 1
                ' Cambio de mes
                If (mes <> Me._datos.SerieAltDiaria.dia(i).Month) Then

                    mes = Me._datos.SerieAltDiaria.dia(i).Month

                    If (salida.caudalMensual Is Nothing) Then
                        ReDim Preserve salida.caudalMensual(0)
                        ReDim Preserve salida.mes(0)
                    Else
                        ReDim Preserve salida.caudalMensual(salida.caudalMensual.Length)
                        ReDim Preserve salida.mes(salida.mes.Length)
                        'pos = pos + 1
                    End If
                    salida.nMeses = salida.nMeses + 1
                    pos = salida.caudalMensual.Length - 1

                    If (año <> Me._datos.SerieAltDiaria.dia(i).Year) Then
                        año = Me._datos.SerieAltDiaria.dia(i).Year
                        salida.nAños = salida.nAños + 1
                    End If

                    d = New Date(año, mes, 1)

                    salida.mes(pos) = d
                    salida.caudalMensual(pos) = 0
                End If

                salida.caudalMensual(pos) = salida.caudalMensual(pos) + Me._datos.SerieAltDiaria.caudalDiaria(i) ' / Date.DaysInMonth(Me._datos.SerieNatDiaria.dia(i).Year, Me._datos.SerieNatDiaria.dia(i).Month)
            Next
            Return salida
        End Function

        ''' <summary>
        ''' Calcula la tabla CQC
        ''' </summary>
        ''' <param name="usarAlterados">Si es para alterados o para naturales</param>
        ''' <remarks>NO comprueba si esta calculada anteriormente</remarks>
        Private Sub CalcularTablaCQC(ByVal usarAlterados As Boolean)

            Dim i, j As Integer
            Dim nAños As Integer

            ' Se elige la serie que se va a usar
            Dim serieAux As SerieDiaria
            If (usarAlterados) Then
                serieAux = Me._datos.SerieAltDiaria
                nAños = Me._datos.SerieAltDiaria.nAños
            Else
                serieAux = Me._datos.SerieNatDiaria
                nAños = Me._datos.SerieNatDiaria.nAños
            End If

            ' Tabla auxiliar donde vamos a almacenar
            Dim tablaAux As TablaCQC

            ' Definicion de la estructura donde almacenaré la tabla CQC
            ReDim tablaAux.pe(364)
            ReDim tablaAux.dia(364)
            ReDim tablaAux.añomedio(364)
            ReDim tablaAux.caudales(nAños - 1)

            ' Relleno los dias incluyendo los bisiestos
            For j = 0 To 364
                tablaAux.pe(j) = (j + 1) / 365 * 100
                tablaAux.dia(j) = j + 1
            Next

            Dim acum As Integer

            ' Relleno los caudales
            acum = 0
            For i = 0 To nAños - 1
                Dim posibleBisiesto As Integer
                If (Me._datos.mesInicio > 2) Then
                    posibleBisiesto = serieAux.dia(acum).Year + 1
                Else
                    posibleBisiesto = serieAux.dia(acum).Year
                End If

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    ReDim Preserve tablaAux.caudales(i)(365)
                Else
                    ReDim Preserve tablaAux.caudales(i)(364)
                End If

                For j = 0 To tablaAux.caudales(i).Length - 1
                    tablaAux.caudales(i)(j) = serieAux.caudalDiaria(acum + j)
                Next

                If (Date.IsLeapYear(posibleBisiesto) = True) Then
                    acum = acum + 366
                Else
                    acum = acum + 365
                End If
            Next

            ' Ordenar los caudales
            For i = 0 To nAños - 1
                Array.Sort(tablaAux.caudales(i))
                Array.Reverse(tablaAux.caudales(i))
            Next

            ' Generar la media
            For i = 0 To 364
                tablaAux.añomedio(i) = 0
                For j = 0 To nAños - 1
                    tablaAux.añomedio(i) = tablaAux.añomedio(i) + tablaAux.caudales(j)(i)
                Next
                tablaAux.añomedio(i) = tablaAux.añomedio(i) / nAños
            Next

            If (usarAlterados) Then
                Me._TablaCQCAlt = tablaAux
            Else
                Me._TablaCQCNat = tablaAux
            End If

        End Sub

        ''' <summary>
        ''' Calcula el ajuste de LGIII
        ''' </summary>
        ''' <returns>Si todo esta correcto</returns>
        ''' <remarks></remarks>
        Private Function AjusteLogPearsonIII(ByVal nAños As Integer, ByVal val() As Single, ByRef valAjus() As Single) As Boolean

            Dim ut(3) As Single, t(3) As Single, ck(3) As Single
            Dim PROB() As Single, ESC() As Single, x() As Single
            Dim i As Integer ', j As Integer, k As Integer
            Dim xm As Single, xs As Single, xg As Single, cst As Single, Vn As Single, ckk As Single

            ' OJO que no usa la posicion 0 de los arrays.

            'blnajus = True
            ut(1) = 0.0#
            ut(2) = -0.842
            ut(3) = -1.282
            t(1) = 2.0#
            t(2) = 5.0#
            t(3) = 10.0#
            ReDim PROB(nAños), ESC(nAños), x(nAños)
            Dim ii As Integer = 1
            For i = 0 To nAños - 1
                x(ii) = miLog10(val(i))
                ii += 1
            Next i
            Vn = nAños
            xm = 0.0#
            xs = 0.0#
            xg = 0.0#
            For i = 1 To nAños
                xm = xm + x(i) / nAños
                xs = xs + x(i) ^ 2 / nAños
            Next i
            xs = xs - xm ^ 2
            xs = xs * Vn / (Vn - 1.0#)
            For i = 1 To nAños
                xg = xg + (x(i) - xm) ^ 3
            Next i
            If xs <> 0.0# Then
                cst = (xg * Vn) / ((Vn - 1.0#) * (Vn - 2.0#) * xs ^ 1.5)
            Else
                cst = 0.0#
            End If
            ReDim valAjus(3)
            For i = 1 To 3
                Call kt(ut(i), cst, ckk)
                valAjus(i) = xm + ckk * xs ^ 0.5
                valAjus(i) = 10.0# ^ valAjus(i)
                If cst = 0.0# Then
                    If xm <> 0.0# Then
                        valAjus(i) = 10.0# ^ xm
                    Else
                        valAjus(i) = 0.0#
                    End If
                End If
            Next i
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ut"></param>
        ''' <param name="xg"></param>
        ''' <param name="dk"></param>
        ''' <remarks></remarks>
        Private Sub kt(ByVal ut As Single, ByVal xg As Single, ByRef dk As Single)
            Dim Dk1 As Single, Dk2 As Single
            Dk1 = ut + (ut ^ 2 - 1.0#) * (xg / 6) + (1 / 3) * (ut ^ 3 - 6 * ut) * (xg / 6) ^ 2
            Dk2 = -(ut ^ 2 - 1.0#) * (xg / 6) ^ 3 + ut * (xg / 6) ^ 4 + (1 / 3) * (xg / 6) ^ 5
            dk = Dk1 + Dk2
        End Sub

        Private Function miLog10(ByVal x As Single) As Single
            Dim valor As Single
            If x > 0.0# Then
                valor = System.Math.Log(x) / System.Math.Log(10.0#)
            Else
                valor = 0.0#
            End If
            Return valor
        End Function
#End Region

#Region "Tipos de Calculo"
        Public Sub GenerarInforme1()
            CalcularINTERAnual()
            EscribirInforme1()
        End Sub
        Public Sub GenerarInforme1b()
            Me.CalcularINTERAnualAlterada()
            Me.EscribirInforme1b()
        End Sub
        Public Sub GenerarInforme2()
            Me.calcularINTRAnual(False)
            Me.EscribirInforme2()
        End Sub
        Public Sub generarInforme3()
            Me.calcularINTRAnual(True)
            Me.EscribirInforme3()
        End Sub
        Public Sub GenerarInforme4a()
            Me.CalcularParametrosHabitualesCASO1()
            Me.EscribirInforme4a()
        End Sub
        Public Sub GenerarInforme4b()
            Me.CalcularParametrosNaturalesHabitualesReducidos()
            Me.EscribirInforme4b()
        End Sub
        Public Sub GenerarInforme4()
            Me.CalcularParametrosHabitualesCASO1()
            Me.CalculoParametrosVariabilidadDIARIAHabitual()
            Me.CalculoParametrosAvenidasCASO4()
            Me.CalculoParametrosSequiasCASO4()
            Me.EscribirInforme4()
        End Sub
        Public Sub GenerarInforme5()
            Me.CalcularParametrosHabitualesAlterados()
            Me.CalculoParametrosVariabilidadDIARIAHabitualAlterada()
            Me.CalculoParametrosAvenidasAlteradosCASO6()
            Me.CalculoParametrosSequiasAlteradosCASO6()
            Me.EscribirInforme5()

            'Me._i23Simplificado = False
        End Sub
        Public Sub GenerarInforme5a()
            Me.CalcularParametrosHabitualesReducidos()
            Me.EscribirInforme5a()

            'Me._i23Simplificado = True
        End Sub
        Public Sub GenerarInforme5b()
            Me.CalcularParametrosHabitualesAlterados()
            Me.EscribirInforme5b()

            'Me._i23Simplificado = False
        End Sub
        Public Sub GenerarInforme5c()
            Me.CalcularParametrosHabitualesReducidos()
            Me.CalculoParametrosVariabilidadDIARIAHabitualAlterada()
            Me.CalculoParametrosAvenidasAlteradosCASO6()
            Me.CalculoParametrosSequiasAlteradosCASO6()

            Me.EscribirInforme5c()

            'Me._i23Simplificado = True
        End Sub
        Public Sub GenerarInforme6a()
            Me.CalcularTablaCQC(False)
            Me.EscribirInforme6a()
        End Sub
        Public Sub GenerarInforme6()
            Me.CalcularTablaCQC(False)
            Me.CalcularTablaCQC(True)
            Me.EscribirInforme6()
        End Sub
        Public Sub GenerarInforme7a()
            Me.CalcularIndicesHabitualesCASO3()
            Me.CalcularIndiceHabitual_I3()
            Me.CalcularIndiceAlteracionGlobalHabituales()
            Me.EscribirInforme7a()
        End Sub
        Public Sub GenerarInforme7b()
            Me.CalcularIndicesHabitualesCASO3()
            Me.CalcularIndiceAlteracionGlobalHabituales()
            Me.EscribirInforme7b()
        End Sub
        Public Sub GenerarInforme7c()
            Me.CalcularIndicesHabitualesAgregados()
            Me.CalcularIndiceAlteracionGlobalHabitualesAgregados()
            Me.EscribirInforme7c()
        End Sub
        Public Sub GenerarInforme7d()
            Me.CalcularIndicesAvenidasCASO6()
            Me.CalcularIndicesSequiasCASO6()
            Me.CalcularIndiceAlteracionGlobalAvenidas()
            Me.CalcularIndiceAlteracionGlobalSequias()
            Me.EscribirInforme7d()
        End Sub
        Public Sub GenerarInforme8()
            Me.CalcularRegimenNatural()
            Me.CalcularRegimenAlterado()
            Me.CalcularRegimenNaturalAnual()
            Me.CalcularRegimenAlteradoAnual()

            Me.EscribirInforme8()
        End Sub
        Public Sub GenerarInforme8a()
            Me.EscribirInforme8a()
        End Sub
        Public Sub GenerarInforme8b()
            Me.EscribirInforme8b()
        End Sub
        Public Sub GenerarInforme8c()
            Me.EscribirInforme8c()
        End Sub
        Public Sub GenerarInforme8d()
            Me.EscribirInforme8d()
        End Sub
        Public Sub GenerarInforme9()
            Me.CalcularReferencias()

            Me.EscribirInforme9()
        End Sub
        Public Sub GenerarInforme9a()
            Me.EscribirInforme9a()
        End Sub

        Public Sub EscribirFichero(ByVal coeD As Boolean, ByVal coe As Boolean)
            Me.CerrarExcel(coeD, coe)
        End Sub
#End Region

#Region "Borrar Informes"
        Public Sub BorrarInforme1b()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            'm_Excel.Sheets("Informe nº 1a").Select()
            m_Excel.Sheets(3).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme3()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(5).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme4a()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(7).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme4b()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(8).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme4()
            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(6).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme5()
            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(9).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme5a()
            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(10).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme5b()
            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(11).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme5c()
            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(12).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme6()
            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(13).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme6a()
            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(14).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme7a()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(15).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme7b()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(16).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme7c()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(17).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme7d()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(18).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme8()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(19).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme8a()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(20).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme8b()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(21).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme8c()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(22).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme8d()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(23).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme9()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(24).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
        Public Sub BorrarInforme9a()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            m_Excel.Sheets(25).Select()
            m_Excel.ActiveWindow.SelectedSheets.Delete()
        End Sub
#End Region

#Region "Escribir informes"
        Private Sub AbrirExcel()

            'Dim strRutaExcel As String = My.Application.Info.DirectoryPath & "\FicheroModelo.xls"
            Dim strRutaExcel As String = Me._rutaExcel

            Me.m_Excel = CreateObject("Excel.Application")
            Me.objWorkbook = Me.m_Excel.Workbooks.Open(strRutaExcel)
            Me.m_Excel.Visible = False 'Dejamos el libro oculto
            Me.m_Excel.DisplayAlerts = False

        End Sub
        Private Sub CerrarExcel(ByVal coeD As Boolean, ByVal coeM As Boolean)

            'Dim coeD As Boolean
            'Dim coeM As Boolean
            Dim ruta As String = ""
            Dim nombre As String = ""

            Dim FolderBrowserDialog1 As FolderBrowserDialog = New FolderBrowserDialog()

            ' Construir el nombre del fichero
            'If ((Not Me._datos.SerieAltDiaria.dia Is Nothing) And (Not Me._datos.SerieNatDiaria.dia Is Nothing)) Then
            '    If (Me._datos.SerieAltDiaria.dia.Length = Me._datos.SerieNatDiaria.dia.Length) Then
            '        coeD = True
            '    Else
            '        coeD = False
            '    End If
            'Else
            '    coeD = False
            'End If
            'If ((Not Me._datos.SerieAltMensual.mes Is Nothing) And (Not Me._datos.SerieNatMensual.mes Is Nothing)) Then
            '    If (Me._datos.SerieAltMensual.mes.Length = Me._datos.SerieNatMensual.mes.Length) Then
            '        coeM = True
            '    Else
            '        coeM = False
            '    End If
            'Else
            '    coeM = False
            'End If


            nombre = Me._datos.sNombre

            nombre = nombre.Substring(0, nombre.LastIndexOf("-"))

            If (Me._datos.SerieAltMensual.nAños <> 0 Or Me._datos.SerieAltDiaria.nAños <> 0) Then
                nombre = nombre & "_" & Me._datos.sAlteracion

                nombre = nombre.Substring(0, nombre.LastIndexOf("-"))

                If (Me._datos.SerieAltMensual.nAños <> 0) Then
                    If (coeM) Then
                        nombre = nombre & "_COEMSI"
                    Else
                        nombre = nombre & "_COEMNO"
                    End If
                End If
                If (Me._datos.SerieAltDiaria.nAños <> 0) Then
                    If (coeD) Then
                        nombre = nombre & "_COEDSI"
                    Else
                        nombre = nombre & "_COEDNO"
                    End If
                End If
            End If

            nombre = nombre & ".xls"

            Try
                ' Configuración del FolderBrowserDialog  
                'With FolderBrowserDialog1

                '    '.Reset() ' resetea  
                '    ' leyenda  
                '    .Description = "Seleccionar una carpeta donde guardar el fichero de resultados"
                '    ' Path " Mis documentos "  
                '    '.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)


                '    ' deshabilita el botón " crear nueva carpeta "  
                '    .ShowNewFolderButton = True
                '    '.RootFolder = Environment.SpecialFolder.Desktop
                '    '.RootFolder = Environment.CurrentDirectory

                '    Dim ret As DialogResult = .ShowDialog ' abre el diálogo  

                '    ' si se presionó el botón aceptar ...  
                '    If ret = Windows.Forms.DialogResult.OK Then
                '        ruta = .SelectedPath.ToString()
                '    Else
                '        Return
                '    End If

                '    .Dispose()

                'End With

                Dim SaveFileDialog1 As New SaveFileDialog()

                'openFileDialog1.InitialDirectory = "c:\"
                SaveFileDialog1.Filter = "Archivos Microsoft Excel (*.xls)|*.xls|Todos los ficheros (*.*)|*.*"
                SaveFileDialog1.FilterIndex = 1
                SaveFileDialog1.FileName = nombre
                SaveFileDialog1.OverwritePrompt = False
                'openFileDialog1.RestoreDirectory = True

                If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                    ruta = SaveFileDialog1.FileName
                Else
                    Exit Sub
                End If


            Catch oe As Exception
                MsgBox(oe.Message, MsgBoxStyle.Critical)
            End Try

            If My.Computer.FileSystem.FileExists(ruta) Then
                ' Alerta por sobreescritura
                If (MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOverwrite"), _
                                    Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAttention"), _
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No) Then
                    If Not m_Excel Is Nothing Then
                        m_Excel.Quit()
                        m_Excel = Nothing
                    End If
                    Exit Sub
                Else
                    Try
                        My.Computer.FileSystem.DeleteFile(ruta)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If


            ' Tiene que salvarse como  Punto-Alt-COE-SI-SI.xls
            'If (ruta.EndsWith("\")) Then
            Try
                m_Excel.Application.ActiveWorkbook.SaveAs(ruta)
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strReportOk") & vbCrLf & ruta, _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNotSaveFile") & vbCrLf & ruta & vbCrLf & ex.Message, _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            'Else
            'm_Excel.Application.ActiveWorkbook.SaveAs(ruta & "\" & nombre)
            'MessageBox.Show("Fichero salvado correctamente en: " & vbCrLf & ruta & "\" & nombre, "Fichero salvado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'End If

            'Eliminamos la instancia de Excel de memoria
            If Not m_Excel Is Nothing Then
                m_Excel.Quit()
                m_Excel = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Escribe la primera pagina del informe
        ''' </summary>
        ''' <remarks>Tiene que ser publica para recibir la simulacion</remarks>
        Public Sub EscribirCabecera(ByVal sim As Simulacion, ByVal informes As GeneracionInformes)

            Dim range As Excel.Range

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(1)

            range = objSheet.Range("E7")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E8")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E9")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("K12")
            range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, sim.mesInicio)

            Dim i, j As Integer
            Dim pos As Integer


            ' Escribir informes
            ' -----------------
            Dim fila As Integer = 14
            If (informes.inf1) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1")
                fila = fila + 1
            End If
            If (informes.inf1b) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1a")
                fila = fila + 1
            End If
            If (informes.inf2) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe2")
                fila = fila + 1
            End If
            If (informes.inf3) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe3")
                fila = fila + 1
            End If
            If (informes.inf4) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4")
                fila = fila + 1
            End If
            If (informes.inf4a) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4a")
                fila = fila + 1
            End If
            If (informes.inf4b) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4b")
                fila = fila + 1
            End If
            If (informes.inf5) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5")
                fila = fila + 1
            End If
            If (informes.inf5a) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5a")
                fila = fila + 1
            End If
            If (informes.inf5b) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5b")
                fila = fila + 1
            End If
            If (informes.inf5c) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5c")
                fila = fila + 1
            End If
            If (informes.inf6) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6")
                fila = fila + 1
            End If
            If (informes.inf6a) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6a")
                fila = fila + 1
            End If
            If (informes.inf7a) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7a")
                fila = fila + 1
            End If
            If (informes.inf7b) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7b")
                fila = fila + 1
            End If
            If (informes.inf7c) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7c")
                fila = fila + 1
            End If
            If (informes.inf7d) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7d")
                fila = fila + 1
            End If
            If (informes.inf8) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8")
                fila = fila + 1
            End If
            If (informes.inf8a) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8a")
                fila = fila + 1
            End If
            If (informes.inf8b) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8b")
                fila = fila + 1
            End If
            If (informes.inf8c) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8c")
                fila = fila + 1
            End If
            If (informes.inf8d) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8d")
                fila = fila + 1
            End If
            If (informes.inf9) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9")
                fila = fila + 1
            End If
            If (informes.inf9a) Then
                range = objSheet.Range("B" & fila.ToString())
                range.Value = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9a")
                fila = fila + 1
            End If


            range = objSheet.Range("B32")
            range = range.Resize(sim.fechaFIN - sim.fechaINI, 11)

            For i = sim.fechaINI To sim.fechaFIN - 1
                Dim indice As Integer = i - sim.fechaINI
                range(indice + 1, 1) = i.ToString() & "-" & (i + 1).ToString().Substring(2)


                ' MENSUALES NATURAL
                If (sim.listas(2).Año Is Nothing) Then
                    If (Not sim.añosInterNat Is Nothing) Then
                        For j = 0 To sim.añosInterNat.Length - 1
                            If (i = sim.añosInterNat(j)) Then
                                range(indice + 1, 2) = "X"
                                Exit For
                                'Else
                                'range(indice + 1, 2) = "SD"
                            End If
                        Next
                    End If
                Else
                    pos = Array.BinarySearch(sim.listas(2).Año, i)
                    If (pos >= 0) Then
                        If (sim.listas(2).validos(pos)) Then
                            range(indice + 1, 2) = "X"
                            'Else
                            'range(indice + 1, 2) = "NC"
                        End If
                        'Else
                        'range(indice + 1, 2) = "SD"
                    End If

                    If (Not sim.añosInterNat Is Nothing) Then
                        For j = 0 To sim.añosInterNat.Length - 1
                            If (i = sim.añosInterNat(j)) Then
                                range(indice + 1, 2) = "X"
                                Exit For
                                'Else
                                'range(indice + 1, 2) = "SD"
                            End If
                        Next
                    End If
                End If
                If (Not range(indice + 1, 2).Value Is Nothing) Then
                    If (Not sim.añosParaCalculo(2).año Is Nothing) Then
                        For j = 0 To sim.añosParaCalculo(2).año.Length - 1
                            If (i = sim.añosParaCalculo(2).año(j)) Then
                                range(indice + 1, 3) = "X"
                                Exit For
                            End If
                        Next
                    End If
                End If

                ' MENSUALES ALTERADO
                If (sim.listas(3).Año Is Nothing) Then
                    If (Not sim.añosInterAlt Is Nothing) Then
                        For j = 0 To sim.añosInterAlt.Length - 1
                            If (i = sim.añosInterAlt(j)) Then
                                range(indice + 1, 4) = "X"
                                Exit For
                                'Else
                                'range(indice + 1, 4) = "SD"
                            End If
                        Next
                    End If
                Else
                    pos = Array.BinarySearch(sim.listas(3).Año, i)
                    If (pos >= 0) Then
                        If (sim.listas(3).validos(pos)) Then
                            range(indice + 1, 4) = "X"
                            'Else
                            'range(indice + 1, 4) = "NC"
                        End If
                        'Else
                        'range(indice + 1, 4) = "SD"
                    End If

                    If (Not sim.añosInterAlt Is Nothing) Then
                        For j = 0 To sim.añosInterAlt.Length - 1
                            If (i = sim.añosInterAlt(j)) Then
                                range(indice + 1, 4) = "X"
                                Exit For
                                'Else
                                'range(indice + 1, 4) = "SD"
                            End If
                        Next
                    End If
                End If
                If (Not range(indice + 1, 4).Value Is Nothing) Then
                    If (Not sim.añosParaCalculo(3).año Is Nothing) Then
                        For j = 0 To sim.añosParaCalculo(3).año.Length - 1
                            If (i = sim.añosParaCalculo(3).año(j)) Then
                                range(indice + 1, 5) = "X"
                                Exit For
                            End If
                        Next
                    End If
                End If

                ' COETANEOS MENSUALES
                If ((Not range(indice + 1, 2).Value Is Nothing) And (Not range(indice + 1, 4).Value Is Nothing)) Then
                    range(indice + 1, 6) = "X"
                End If

                ' DIARIO NATURAL
                If (sim.listas(0).Año Is Nothing) Then
                    'range(indice + 1, 7) = "SD"
                Else
                    pos = Array.BinarySearch(sim.listas(0).Año, i)
                    If (pos >= 0) Then
                        If (sim.listas(0).validos(pos)) Then
                            range(indice + 1, 7) = "X"
                            'Else
                            'range(indice + 1, 7) = "NC"
                        End If
                        'Else
                        'range(indice + 1, 7) = "SD"
                    End If
                End If
                If (Not range(indice + 1, 7).Value Is Nothing) Then
                    If (Not sim.añosParaCalculo(0).año Is Nothing) Then
                        For j = 0 To sim.añosParaCalculo(0).año.Length - 1
                            If (i = sim.añosParaCalculo(0).año(j)) Then
                                range(indice + 1, 8) = "X"
                                Exit For
                            End If
                        Next
                    End If
                End If

                ' DIARIO ALTERADO
                If (sim.listas(1).Año Is Nothing) Then
                    'range(indice + 1, 9) = "SD"
                Else
                    pos = Array.BinarySearch(sim.listas(1).Año, i)
                    If (pos >= 0) Then
                        If (sim.listas(1).validos(pos)) Then
                            range(indice + 1, 9) = "X"
                            'Else
                            '    range(indice + 1, 9) = "NC"
                        End If
                        'Else
                        'range(indice + 1, 9) = "SD"
                    End If
                End If
                If (Not range(indice + 1, 9).Value Is Nothing) Then
                    If (Not sim.añosParaCalculo(1).año Is Nothing) Then
                        For j = 0 To sim.añosParaCalculo(1).año.Length - 1
                            If (i = sim.añosParaCalculo(1).año(j)) Then
                                range(indice + 1, 10) = "X"
                                Exit For
                            End If
                        Next
                    End If
                End If


                ' COETANEOS DIARIOS
                If ((Not range(indice + 1, 7).Value Is Nothing) And (Not range(indice + 1, 9).Value Is Nothing)) Then
                    range(indice + 1, 11) = "X"
                End If
            Next

            ' Escribir resumen con años
            Dim filaNumAños As Integer = 32 + (sim.fechaFIN - sim.fechaINI)
            range = objSheet.Range("B" & filaNumAños)
            range = range.Resize(1, 11)

            range(1, 1) = "Total"
            ' Mensuales > Natural 
            If (Not sim.añosInterNat Is Nothing) Then
                range(1, 2) = (sim.listas(2).nValidos + sim.añosInterNat.Length).ToString()
            Else
                range(1, 2) = sim.listas(2).nValidos.ToString()
            End If
            range(1, 3) = sim.añosParaCalculo(2).nAños
            ' Mensual > Alterado
            If (Not sim.añosInterAlt Is Nothing) Then
                range(1, 4) = (sim.listas(3).nValidos + sim.añosInterAlt.Length).ToString()
            Else
                range(1, 4) = sim.listas(3).nValidos.ToString()
            End If
            range(1, 5) = sim.añosParaCalculo(3).nAños
            ' Mensuales > Coetaniedad
            If (Not sim.añosInterCoe Is Nothing) Then
                range(1, 6) = (sim.coe(1).nCoetaneos + sim.añosInterCoe.Length).ToString()
            Else
                range(1, 6) = sim.coe(1).nCoetaneos.ToString()
            End If

            range(1, 7) = sim.listas(0).nValidos.ToString()
            range(1, 8) = sim.añosParaCalculo(0).nAños.ToString()
            range(1, 9) = sim.listas(1).nValidos.ToString()
            range(1, 10) = sim.añosParaCalculo(1).nAños.ToString()
            range(1, 11) = sim.coe(0).nCoetaneos.ToString()

            'Me.lblAñosHidro.Text = (Me._simulacion.fechaFIN - Me._simulacion.fechaINI).ToString()
            'Me.lblAñosNatDiario.Text = Me._simulacion.listas(0).nValidos.ToString()
            'Me.lblAñosAltDiario.Text = Me._simulacion.listas(1).nValidos.ToString()
            'Me.lblAñosCoeDiaria.Text = Me._simulacion.coe(0).nCoetaneos.ToString()

            'If (Not Me._simulacion.añosInterNat Is Nothing) Then
            '    Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos + Me._simulacion.añosInterNat.Length).ToString()
            'Else
            '    Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos).ToString()
            'End If
            'If (Not Me._simulacion.añosInterAlt Is Nothing) Then
            '    Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos + Me._simulacion.añosInterAlt.Length).ToString()
            'Else
            '    Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos).ToString()
            'End If
            'If (Not Me._simulacion.añosInterCoe Is Nothing) Then
            '    Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos + Me._simulacion.añosInterCoe.Length).ToString()
            'Else
            '    Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos).ToString()
            'End If

            'Me.lblAñosNatDiarioUSO.Text = Me._simulacion.añosParaCalculo(0).nAños
            'Me.lblAñosNatMensualUSO.Text = Me._simulacion.añosParaCalculo(2).nAños
            'Me.lblAñosAltDiarioUSO.Text = Me._simulacion.añosParaCalculo(1).nAños
            'Me.lblAñosAltMensualUSO.Text = Me._simulacion.añosParaCalculo(3).nAños



            Dim sRango As String
            m_Excel.Sheets(1).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("B32:L32").Select()
            m_Excel.Selection.Copy()

            Dim filaFin As String = (32 + (sim.fechaFIN - sim.fechaINI)).ToString()
            sRango = "B32:L"
            sRango = sRango & filaFin
            'sRango = sRango & ":L"
            'sRango = sRango & (14 + i - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            ' Poner una linea mas gruesa
            range = objSheet.Range("B" & filaFin & ":L" & filaFin)
            range.Borders.Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlDouble

            m_Excel.Range("A1").Select()

        End Sub

        Private Sub EscribirInforme1()

            Dim range As Excel.Range
            'Dim sPos As String

            Dim i As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(2)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()


            ' Escribir las aportaciones
            range = objSheet.Range("C16")
            range = range.Resize(Me._AportacionNatAnualOrdAños.año.Length, 2)
            For i = 0 To Me._AportacionNatAnualOrdAños.año.Length - 1
                range(i + 1, 1) = Me._AportacionNatAnualOrdAños.año(i).ToString & "-" & (Me._AportacionNatAnualOrdAños.año(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = Me._AportacionNatAnualOrdAños.aportacion(i)
            Next

            m_Excel.Sheets(2).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("C16:D16").Select()
            m_Excel.Selection.Copy()
            Dim sRango As String = "C16:D"
            sRango = sRango & (16 + Me._AportacionNatAnualOrdAños.año.Length - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            ' Escribir los limites
            range = objSheet.Range("J13")
            range.Value = Me._limHumNat
            range = objSheet.Range("J14")
            range.Value = Me._limHumNat
            range = objSheet.Range("L14")
            range.Value = Me._limSecNat
            range = objSheet.Range("J15")
            range.Value = Me._limSecNat

            ' Para realizar las graficas del informe 1b
            'range = objSheet.Range("A100")
            'range = range.Resize(Me._AportacionNatAnualOrdAños.año.Length, 2)
            'For i = 0 To Me._AportacionNatAnualOrdAños.año.Length - 1
            '    range(i + 1, 1) = Me._limHumNat
            '    range(i + 1, 2) = Me._limSecNat
            'Next

            'm_Excel.Sheets("Informe nº 1").Select()
            ''m_Excel.Sheets.Select(objSheet)
            'sPos = (100 + Me._AportacionNatAnualOrdAños.año.Length).ToString()
            'm_Excel.Range("A100:B" & sPos).Select()
            'With m_Excel.Selection.Font
            '    .Name = "Arial"
            '    .FontStyle = "Normal"
            '    .Size = 10
            '    .Strikethrough = False
            '    .Superscript = False
            '    .Subscript = False
            '    .OutlineFont = False
            '    .Shadow = False
            '    .Underline = xlUnderlineStyleNone
            '    .ColorIndex = 2
            'End With

            ' -----------------------
            Dim nAñosH As Integer = 0
            Dim nAñosM As Integer = 0
            Dim nAñosS As Integer = 0
            Dim añosH() As Integer = Nothing
            Dim añosM() As Integer = Nothing
            Dim añosS() As Integer = Nothing
            Dim apH() As Single = Nothing
            Dim apM() As Single = Nothing
            Dim apS() As Single = Nothing

            For i = 0 To Me._AportacionNatAnual.año.Length - 1
                If (Me._AportacionNatAnual.tipo(i) = TIPOAÑO.HUMEDO) Then
                    If (nAñosH = 0) Then
                        nAñosH = 1
                        ReDim añosH(0)
                        ReDim apH(0)
                        añosH(0) = Me._AportacionNatAnual.año(i)
                        apH(0) = Me._AportacionNatAnual.aportacion(i)
                    Else
                        ReDim Preserve añosH(nAñosH)
                        ReDim Preserve apH(nAñosH)
                        añosH(nAñosH) = Me._AportacionNatAnual.año(i)
                        apH(nAñosH) = Me._AportacionNatAnual.aportacion(i)
                        nAñosH = nAñosH + 1
                    End If
                ElseIf (Me._AportacionNatAnual.tipo(i) = TIPOAÑO.MEDIO) Then
                    If (nAñosM = 0) Then
                        nAñosM = 1
                        ReDim añosM(0)
                        ReDim apM(0)
                        añosM(0) = Me._AportacionNatAnual.año(i)
                        apM(0) = Me._AportacionNatAnual.aportacion(i)
                    Else
                        ReDim Preserve añosM(nAñosM)
                        ReDim Preserve apM(nAñosM)
                        añosM(nAñosM) = Me._AportacionNatAnual.año(i)
                        apM(nAñosM) = Me._AportacionNatAnual.aportacion(i)
                        nAñosM = nAñosM + 1
                    End If
                Else
                    If (nAñosS = 0) Then
                        nAñosS = 1
                        ReDim añosS(0)
                        ReDim apS(0)
                        añosS(0) = Me._AportacionNatAnual.año(i)
                        apS(0) = Me._AportacionNatAnual.aportacion(i)
                    Else
                        ReDim Preserve añosS(nAñosS)
                        ReDim Preserve apS(nAñosS)
                        añosS(nAñosS) = Me._AportacionNatAnual.año(i)
                        apS(nAñosS) = Me._AportacionNatAnual.aportacion(i)
                        nAñosS = nAñosS + 1
                    End If
                End If
            Next

            Array.Sort(añosH, apH)
            Array.Sort(añosM, apM)
            Array.Sort(añosS, apS)

            range = objSheet.Range("N16")
            range = range.Resize(añosH.Length, 2)
            For i = 0 To añosH.Length - 1
                range(i + 1, 1) = añosH(i) & "-" & (añosH(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = apH(i)
            Next
            range = objSheet.Range("P16")
            range = range.Resize(añosM.Length, 2)
            For i = 0 To añosM.Length - 1
                range(i + 1, 1) = añosM(i) & "-" & (añosM(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = apM(i)
            Next
            range = objSheet.Range("R16")
            range = range.Resize(añosS.Length, 2)
            For i = 0 To añosS.Length - 1
                range(i + 1, 1) = añosS(i) & "-" & (añosS(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = apS(i)
            Next

            m_Excel.Sheets(2).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("N16:O16").Select()
            m_Excel.Selection.Copy()
            sRango = "N16:O"
            sRango = sRango & (16 + nAñosH - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            m_Excel.Sheets(2).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("P16:Q16").Select()
            m_Excel.Selection.Copy()
            sRango = "P16:Q"
            sRango = sRango & (16 + nAñosM - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            m_Excel.Sheets(2).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("R16:S16").Select()
            m_Excel.Selection.Copy()
            sRango = "R16:S"
            sRango = sRango & (16 + nAñosS - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            m_Excel.Range("A1").Select()
        End Sub
        Private Sub EscribirInforme1b()
            Dim range As Excel.Range

            Dim i As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(3)


            ' Escribir cabecera
            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribir aportaciones mensuales
            range = objSheet.Range("C14")
            range = range.Resize(Me._AportacionAltAnualOrdAños.año.Length, 2)
            For i = 0 To Me._AportacionAltAnualOrdAños.año.Length - 1
                range(i + 1, 1) = Me._AportacionAltAnualOrdAños.año(i) & "-" & (Me._AportacionAltAnualOrdAños.año(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = Me._AportacionAltAnualOrdAños.aportacion(i)
            Next

            m_Excel.Sheets(3).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("C14:D14").Select()
            m_Excel.Selection.Copy()
            Dim sRango As String = "C14:D"
            sRango = sRango & (14 + Me._AportacionAltAnualOrdAños.año.Length - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            ' Escribir los años por tipos
            Dim nAñosH As Integer = 0
            Dim nAñosM As Integer = 0
            Dim nAñosS As Integer = 0
            Dim añosH() As Integer = Nothing
            Dim añosM() As Integer = Nothing
            Dim añosS() As Integer = Nothing
            Dim apH() As Single = Nothing
            Dim apM() As Single = Nothing
            Dim apS() As Single = Nothing


            ' Esto solo se calculo con las series mensuales COETANEOS -> Pueso usar NAT o ALt indistintamente
            For i = 0 To Me._AportacionAltAnualOrdAños.año.Length - 1
                If (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.HUMEDO) Then
                    If (nAñosH = 0) Then
                        nAñosH = 1
                        ReDim añosH(0)
                        ReDim apH(0)
                        añosH(0) = Me._AportacionAltAnualOrdAños.año(i)
                        apH(0) = Me._AportacionAltAnualOrdAños.aportacion(i)
                    Else
                        ReDim Preserve añosH(nAñosH)
                        ReDim Preserve apH(nAñosH)
                        añosH(nAñosH) = Me._AportacionAltAnualOrdAños.año(i)
                        apH(nAñosH) = Me._AportacionAltAnualOrdAños.aportacion(i)
                        nAñosH = nAñosH + 1
                    End If
                ElseIf (Me._AportacionNatAnualOrdAños.tipo(i) = TIPOAÑO.MEDIO) Then
                    If (nAñosM = 0) Then
                        nAñosM = 1
                        ReDim añosM(0)
                        ReDim apM(0)
                        añosM(0) = Me._AportacionAltAnualOrdAños.año(i)
                        apM(0) = Me._AportacionAltAnualOrdAños.aportacion(i)
                    Else
                        ReDim Preserve añosM(nAñosM)
                        ReDim Preserve apM(nAñosM)
                        añosM(nAñosM) = Me._AportacionAltAnualOrdAños.año(i)
                        apM(nAñosM) = Me._AportacionAltAnualOrdAños.aportacion(i)
                        nAñosM = nAñosM + 1
                    End If
                Else
                    If (nAñosS = 0) Then
                        nAñosS = 1
                        ReDim añosS(0)
                        ReDim apS(0)
                        añosS(0) = Me._AportacionAltAnualOrdAños.año(i)
                        apS(0) = Me._AportacionAltAnualOrdAños.aportacion(i)
                    Else
                        ReDim Preserve añosS(nAñosS)
                        ReDim Preserve apS(nAñosS)
                        añosS(nAñosS) = Me._AportacionAltAnualOrdAños.año(i)
                        apS(nAñosS) = Me._AportacionAltAnualOrdAños.aportacion(i)
                        nAñosS = nAñosS + 1
                    End If
                End If
            Next

            Array.Sort(añosH, apH)
            Array.Sort(añosM, apM)
            Array.Sort(añosS, apS)

            range = objSheet.Range("N14")
            range = range.Resize(añosH.Length, 2)
            For i = 0 To añosH.Length - 1
                range(i + 1, 1) = añosH(i) & "-" & (añosH(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = apH(i)
            Next
            range = objSheet.Range("P14")
            range = range.Resize(añosM.Length, 2)
            For i = 0 To añosM.Length - 1
                range(i + 1, 1) = añosM(i) & "-" & (añosM(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = apM(i)
            Next
            range = objSheet.Range("R14")
            range = range.Resize(añosS.Length, 2)
            For i = 0 To añosS.Length - 1
                range(i + 1, 1) = añosS(i) & "-" & (añosS(i) + 1).ToString().Substring(2)
                range(i + 1, 2) = apS(i)
            Next

            m_Excel.Sheets(3).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("N14:O14").Select()
            m_Excel.Selection.Copy()
            sRango = "N14:O"
            sRango = sRango & (14 + nAñosH - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            m_Excel.Sheets(3).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("P14:Q14").Select()
            m_Excel.Selection.Copy()
            sRango = "P14:Q"
            sRango = sRango & (14 + nAñosM - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            m_Excel.Sheets(3).Select()
            'm_Excel.Sheets.Select(objSheet)
            m_Excel.Range("R14:S14").Select()
            m_Excel.Selection.Copy()
            sRango = "R14:S"
            sRango = sRango & (14 + nAñosS - 1).ToString()
            m_Excel.Range(sRango).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            m_Excel.Range("A1").Select()

            ' -----------------------------------------------------
            ' ----- Realizar el grafico dinamico ------------------
            ' -----------------------------------------------------

            'm_Excel.Range("F" & (16 + Me._AportacionAltAnualOrdAños.año.Length).ToString()).Select()
            'm_Excel.Charts.Add()
            'm_Excel.ActiveChart.ChartType = Excel.XlChartType.xlLineMarkers  'xlLineMarkers
            'm_Excel.ActiveChart.SetSourceData(Source:=m_Excel.Sheets("Informe nº 1a").Range("F" & (16 + Me._AportacionAltAnualOrdAños.año.Length).ToString()))
            'm_Excel.ActiveChart.SeriesCollection.NewSeries()
            'm_Excel.ActiveChart.SeriesCollection.NewSeries()
            'm_Excel.ActiveChart.SeriesCollection.NewSeries()
            'm_Excel.ActiveChart.SeriesCollection.NewSeries()
            'm_Excel.ActiveChart.SeriesCollection(1).XValues = _
            '    "='Informe nº 1'!R16C3:R" &  & "C3"
            'ActiveChart.SeriesCollection(1).Values = _
            '    "='Informe nº 1'!R16C4:R30C4"
            'ActiveChart.SeriesCollection(1).Name = "=""Serie Natural"""
            'ActiveChart.SeriesCollection(2).Values = _
            '    "='Informe nº 1a'!R14C4:R28C4"
            'ActiveChart.SeriesCollection(2).Name = "=""Serie Alterada"""
            'ActiveChart.SeriesCollection(3).Values = _
            '    "='Informe nº 1'!R100C1:R114C1"
            'ActiveChart.SeriesCollection(3).Name = "=""LimHum"""
            'ActiveChart.SeriesCollection(4).Values = _
            '    "='Informe nº 1'!R100C2:R114C2"
            'ActiveChart.SeriesCollection(4).Name = "=""LimSec"""
            'ActiveChart.Location(Where:=xlLocationAsObject, Name:= _
            '    "Informe nº 1a")
            'With ActiveChart
            '    .HasTitle = True
            '    .ChartTitle.Characters.Text = "Series Aportaciones"
            '    .Axes(xlCategory, xlPrimary).HasTitle = True
            '    .Axes(xlCategory, xlPrimary).AxisTitle.Characters.Text = "Años Hidraulicos"
            '    .Axes(xlValue, xlPrimary).HasTitle = True
            '    .Axes(xlValue, xlPrimary).AxisTitle.Characters.Text = "Hm3"
            'End With
            'ActiveSheet.Shapes("Gráfico 1").ScaleWidth(0.58, msoFalse, msoScaleFromTopLeft)
            'ActiveSheet.Shapes("Gráfico 1").ScaleHeight(0.75, msoFalse, _
            '    msoScaleFromTopLeft)
            'ActiveSheet.Shapes("Gráfico 1").IncrementTop(188.25)
            'ActiveSheet.Shapes("Gráfico 1").ScaleWidth(1.66, msoFalse, msoScaleFromTopLeft)
            'Windows("VADO-E. El Vado_VADO1-Régimen actual_COEMSI_COEDSI.xls").SmallScroll( _
            '    Down:=6)
            'ActiveSheet.Shapes("Gráfico 1").ScaleWidth(1.31, msoFalse, msoScaleFromTopLeft)

        End Sub
        Private Sub EscribirInforme2()

            Dim range As Excel.Range
            Dim i As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(4)

            range = objSheet.Range("E5")

            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribir los meses
            range = objSheet.Range("B15")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                'range(i + 1, 1) = strmes.ToString()
                'Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

            ' Escribir las aportaciones
            range = objSheet.Range("C15")
            range = range.Resize(12, 3)
            For i = 0 To 11
                range(i + 1, 1) = Me._IntraAnualNat(i)(0)
                range(i + 1, 2) = Me._IntraAnualNat(i)(1)
                range(i + 1, 3) = Me._IntraAnualNat(i)(2)
            Next

        End Sub
        Private Sub EscribirInforme3()
            Dim range As Excel.Range
            Dim i As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(5)

            ' Escribir cabecera
            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribir los meses
            range = objSheet.Range("B15")
            range = range.Resize(12, 1)
            For i = 0 To 11
                Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

            ' Escribir las aportaciones
            range = objSheet.Range("C15")
            range = range.Resize(12, 3)
            For i = 0 To 11
                range(i + 1, 1) = Me._IntraAnualAlt(i)(0)
                range(i + 1, 2) = Me._IntraAnualAlt(i)(1)
                range(i + 1, 3) = Me._IntraAnualAlt(i)(2)
            Next
        End Sub
        Private Sub EscribirInforme4a()
            Dim range As Excel.Range
            Dim i As Integer
            Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(7)

            range = objSheet.Range("E5")

            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("Q14")
            range = range.Resize(11, 1)
            pos = 0
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabMagnitudNat(i)
                pos = pos + 1
            Next
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabVariabilidadNat(i)
                pos = pos + 1
            Next
            For i = 0 To 2
                range(pos + 1, 1) = Me._HabEstacionalidadNat(i)
                pos = pos + 1
            Next
        End Sub
        Private Sub EscribirInforme4b()
            Dim range As Excel.Range

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(8)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("Q14")
            range = range.Resize(3, 1)

            range(1, 1) = Me._HabMagnitudAnualNat(0)
            ' Se quitan estos valores y se deben imprimir otros
            'range(2, 1) = Me._HabVariabilidadAlt(0)
            range(2, 1) = Me._HabMagnitudAnualNat(1)
            'range(3, 1) = Me._HabEstacionalidadAlt(0) '¿Que coño va aqui?
            range(3, 1) = Me._HabMagnitudAnualNat(2)


            range = objSheet.Range("Q20")
            range = range.Resize(1, 1)
            range(1, 1) = Me._HabMagnitudMensualNat(0)


            Dim i As Integer
            ' Lista media por meses
            range = objSheet.Range("E26")
            range = range.Resize(5, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._HabMagnitudMensualTablaNat(0).ndias(i)
                range(2, i + 1) = Me._HabMagnitudMensualTablaNat(1).ndias(i)
                range(3, i + 1) = Me._HabMagnitudMensualTablaNat(2).ndias(i)
                range(4, i + 1) = Me._HabEstacionalidadMensualNat(0).ndias(i)
                range(5, i + 1) = Me._HabEstacionalidadMensualNat(1).ndias(i)
            Next

            ' Escribir los meses
            range = objSheet.Range("E25")
            range = range.Resize(1, 12)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(1, i + 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1).Substring(0, 3)
            Next

        End Sub
        Private Sub EscribirInforme4()
            Dim range As Excel.Range
            Dim i As Integer
            Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(6)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribo los Valores Habituales Mensuales
            range = objSheet.Range("Q13")
            range = range.Resize(11, 1)
            pos = 0
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabMagnitudNat(i)
                pos = pos + 1
            Next
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabVariabilidadNat(i)
                pos = pos + 1
            Next
            For i = 0 To 2
                range(pos + 1, 1) = Me._HabEstacionalidadNat(i)
                pos = pos + 1
            Next

            ' Escribo los habituales diarios
            range = objSheet.Range("Q24")
            range = range.Resize(2, 1)
            range(1, 1) = Me._HabVariabilidadDiaraNat(0)
            range(2, 1) = Me._HabVariabilidadDiaraNat(1)

            ' AVENIDAS
            range = objSheet.Range("Q26")
            range = range.Resize(4, 1)
            range(1, 1) = Me._AveMagnitudNat(0)
            If (Me._AveMagnitudNat(1) = -9999) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._AveMagnitudNat(1)
            End If
            range(3, 1) = Me._AveMagnitudNat(2)
            range(4, 1) = Me._AveMagnitudNat(3)

            ' Escribir el periodo de retorno
            range = objSheet.Range("R27")
            range(1, 1) = Math.Round(Me._Ave2TNat / 2)
            range = objSheet.Range("R28")
            range(1, 1) = Math.Round(Me._Ave2TNat)

            range = objSheet.Range("Q30")
            range = range.Resize(2, 1)
            If (Me._AveVariabilidadNat(0) < 0) Then
                range(1, 1) = "**"
            Else
                range(1, 1) = Me._AveVariabilidadNat(0)
            End If
            If (Me._AveVariabilidadNat(1) < 0) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._AveVariabilidadNat(1)
            End If

            range = objSheet.Range("Q33")
            range.Value = Me._AveDuracionNat

            range = objSheet.Range("E44")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._AveEstacionalidadNat.ndias(i)
            Next
            ' Escribir la media del año
            'range = objSheet.Range("Q44")
            'range.Value = Me._AveEstacionalidadNat.mediaAño

            ' SEQUIA
            range = objSheet.Range("Q34")
            range = range.Resize(2, 1)
            range(1, 1) = Me._SeqMagnitudNat(0)
            range(2, 1) = Me._SeqMagnitudNat(1)

            range = objSheet.Range("Q36")
            range = range.Resize(2, 1)
            If (Me._SeqVariabilidadNat(0) < 0) Then
                range(1, 1) = "**"
            Else
                range(1, 1) = Me._SeqVariabilidadNat(0)
            End If
            If (Me._SeqVariabilidadNat(1) < 0) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._SeqVariabilidadNat(1)
            End If

            range = objSheet.Range("E45")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._SeqEstacionalidadNat.ndias(i)
            Next
            ' Escribir la media dell año
            'range = objSheet.Range("Q45")
            'range.Value = Me._SeqEstacionalidadNat.mediaAño

            range = objSheet.Range("Q39")
            range = range.Resize(2, 1)
            range(1, 1) = Me._SeqDuracionNat(0)
            'range(2, 1) = Me._SeqDuracionNat(1)


            ' Lista de los Q = 0 por meses
            range = objSheet.Range("E46")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._SeqDuracionCerosMesNat.ndias(i)
            Next

            ' Escribir los meses
            range = objSheet.Range("E43")
            range = range.Resize(1, 12)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(1, i + 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1).Substring(0, 3)
            Next

        End Sub
        Private Sub EscribirInforme5()
            Dim range As Excel.Range
            Dim i As Integer
            Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(9)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribo los Valores Habituales Mensuales
            range = objSheet.Range("Q13")
            range = range.Resize(11, 1)
            pos = 0
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabMagnitudAlt(i)
                pos = pos + 1
            Next
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabVariabilidadAlt(i)
                pos = pos + 1
            Next
            For i = 0 To 2
                range(pos + 1, 1) = Me._HabEstacionalidadAlt(i)
                pos = pos + 1
            Next

            ' Escribo los habituales diarios
            range = objSheet.Range("Q24")
            range = range.Resize(2, 1)
            range(1, 1) = Me._HabVariabilidadDiaraAlt(0)
            range(2, 1) = Me._HabVariabilidadDiaraAlt(1)

            ' AVENIDAS
            range = objSheet.Range("Q26")
            range = range.Resize(4, 1)
            range(1, 1) = Me._AveMagnitudAlt(0)
            If (Me._AveMagnitudAlt(1) = -9999) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._AveMagnitudAlt(1)
            End If
            'range(2, 1) = Me._AveMagnitudAlt(1)
            range(3, 1) = Me._AveMagnitudAlt(2)
            range(4, 1) = Me._AveMagnitudAlt(3)

            range = objSheet.Range("Q30")
            range = range.Resize(2, 1)
            'range(1, 1) = Me._AveVariabilidadAlt(0)
            'range(2, 1) = Me._AveVariabilidadAlt(1)
            If (Me._AveVariabilidadAlt(0) < 0) Then
                range(1, 1) = "**"
            Else
                range(1, 1) = Me._AveVariabilidadAlt(0)
            End If
            If (Me._AveVariabilidadAlt(1) < 0) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._AveVariabilidadAlt(1)
            End If

            range = objSheet.Range("Q33")
            range.Value = Me._AveDuracionAlt

            range = objSheet.Range("E44")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._AveEstacionalidadAlt.ndias(i)
            Next
            'range = objSheet.Range("Q44")
            'range.Value = Me._AveEstacionalidadAlt.mediaAño

            ' SEQUIAS
            range = objSheet.Range("Q34")
            range = range.Resize(2, 1)
            range(1, 1) = Me._SeqMagnitudAlt(0)
            range(2, 1) = Me._SeqMagnitudAlt(1)

            range = objSheet.Range("Q36")
            range = range.Resize(2, 1)
            If (Me._SeqVariabilidadAlt(0) < 0) Then
                range(1, 1) = "**"
            Else
                range(1, 1) = Me._SeqVariabilidadAlt(0)
            End If
            If (Me._SeqVariabilidadAlt(1) < 0) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._SeqVariabilidadAlt(1)
            End If

            range = objSheet.Range("E45")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._SeqEstacionalidadAlt.ndias(i)
            Next
            'range = objSheet.Range("Q45")
            'range.Value = Me._SeqEstacionalidadAlt.mediaAño

            range = objSheet.Range("Q39")
            range = range.Resize(2, 1)
            range(1, 1) = Me._SeqDuracionAlt(0)
            'range(2, 1) = Me._SeqDuracionAlt(1)

            ' Lista de los Q = 0 por meses
            range = objSheet.Range("E46")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._SeqDuracionCerosMesAlt.ndias(i)
            Next

            ' Escribir los meses
            range = objSheet.Range("E43")
            range = range.Resize(1, 12)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(1, i + 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1).Substring(0, 3)
            Next

        End Sub
        Private Sub EscribirInforme5a()
            Dim range As Excel.Range

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(10)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("Q14")
            range = range.Resize(3, 1)

            range(1, 1) = Me._HabMagnitudAlt(0)
            ' Se quitan estos valores y se deben imprimir otros
            'range(2, 1) = Me._HabVariabilidadAlt(0)
            range(2, 1) = Me._HabMagnitudAnualAlt(1)
            'range(3, 1) = Me._HabEstacionalidadAlt(0) '¿Que coño va aqui?
            range(3, 1) = Me._HabMagnitudAnualAlt(2)


            range = objSheet.Range("Q20")
            range = range.Resize(1, 1)
            range(1, 1) = Me._HabVariabilidadAlt(0)


            Dim i As Integer
            ' Lista media por meses
            range = objSheet.Range("E26")
            range = range.Resize(5, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._HabMagnitudMensualTablaAlt(0).ndias(i)
                range(2, i + 1) = Me._HabMagnitudMensualTablaAlt(1).ndias(i)
                range(3, i + 1) = Me._HabMagnitudMensualTablaAlt(2).ndias(i)
                range(4, i + 1) = Me._HabEstacionalidadMensualAlt(0).ndias(i)
                range(5, i + 1) = Me._HabEstacionalidadMensualAlt(1).ndias(i)
            Next

            ' Escribir los meses
            range = objSheet.Range("E25")
            range = range.Resize(1, 12)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(1, i + 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1).Substring(0, 3)
            Next

        End Sub
        Private Sub EscribirInforme5b()
            Dim range As Excel.Range
            Dim i As Integer
            Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(11)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("Q14")
            range = range.Resize(11, 1)
            pos = 0
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabMagnitudAlt(i)
                pos = pos + 1
            Next
            For i = 0 To 3
                range(pos + 1, 1) = Me._HabVariabilidadAlt(i)
                pos = pos + 1
            Next
            For i = 0 To 2
                range(pos + 1, 1) = Me._HabEstacionalidadAlt(i)
                pos = pos + 1
            Next

        End Sub
        Private Sub EscribirInforme5c()
            Dim range As Excel.Range
            Dim i As Integer
            'Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(12)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribo los Valores Habituales Mensuales
            range = objSheet.Range("O13")
            range = range.Resize(3, 1)

            range(1, 1) = Me._HabMagnitudAlt(0)
            ' Se quitan dos resultados anteriores
            'range(2, 1) = Me._HabVariabilidadAlt(0)
            'range(3, 1) = Me._HabEstacionalidadAlt(0)
            ' Y se deben habilitar otros dos nuevos
            ' <<<<<<<<< aquí >>>>>>>>>>
            range(2, 1) = Me._HabMagnitudAnualAlt(1)
            range(3, 1) = Me._HabMagnitudAnualAlt(2)


            ' Falta la variabilidad extrema
            range = objSheet.Range("O19")
            range = range.Resize(1, 1)
            range(1, 1) = Me._HabVariabilidadAlt(0)


            ' Escribo los habituales diarios, más abajo que antes
            range = objSheet.Range("Q22")
            range = range.Resize(2, 1)
            range(1, 1) = Me._HabVariabilidadDiaraAlt(0)
            range(2, 1) = Me._HabVariabilidadDiaraAlt(1)

            ' AVENIDAS
            range = objSheet.Range("Q24")
            range = range.Resize(4, 1)
            range(1, 1) = Me._AveMagnitudAlt(0)
            If (Me._AveMagnitudAlt(1) = -9999) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._AveMagnitudAlt(1)
            End If
            'range(2, 1) = Me._AveMagnitudAlt(1)
            range(3, 1) = Me._AveMagnitudAlt(2)
            range(4, 1) = Me._AveMagnitudAlt(3)

            range = objSheet.Range("Q28")
            range = range.Resize(2, 1)
            'range(1, 1) = Me._AveVariabilidadAlt(0)
            'range(2, 1) = Me._AveVariabilidadAlt(1)
            If (Me._AveVariabilidadAlt(0) < 0) Then
                range(1, 1) = "**"
            Else
                range(1, 1) = Me._AveVariabilidadAlt(0)
            End If
            If (Me._AveVariabilidadAlt(1) < 0) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._AveVariabilidadAlt(1)
            End If

            range = objSheet.Range("O31")
            range.Value = Me._AveDuracionAlt

            range = objSheet.Range("E47")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._AveEstacionalidadAlt.ndias(i)
            Next
            'range = objSheet.Range("Q36")
            'range.Value = Me._AveEstacionalidadAlt.mediaAño

            ' Sequia
            range = objSheet.Range("Q32")
            range = range.Resize(2, 1)
            range(1, 1) = Me._SeqMagnitudAlt(0)
            range(2, 1) = Me._SeqMagnitudAlt(1)

            range = objSheet.Range("Q34")
            range = range.Resize(2, 1)
            If (Me._SeqVariabilidadAlt(0) < 0) Then
                range(1, 1) = "**"
            Else
                range(1, 1) = Me._SeqVariabilidadAlt(0)
            End If
            If (Me._SeqVariabilidadAlt(1) < 0) Then
                range(2, 1) = "**"
            Else
                range(2, 1) = Me._SeqVariabilidadAlt(1)
            End If

            range = objSheet.Range("E48")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._SeqEstacionalidadAlt.ndias(i)
            Next
            'range = objSheet.Range("Q37")
            'range.Value = Me._SeqEstacionalidadAlt.mediaAño

            range = objSheet.Range("O37")
            range = range.Resize(2, 1)
            range(1, 1) = Me._SeqDuracionAlt(0)
            'range(2, 1) = Me._SeqDuracionAlt(1)

            ' Lista de los Q = 0 por meses
            range = objSheet.Range("E49")
            range = range.Resize(1, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._SeqDuracionCerosMesAlt.ndias(i)
            Next

            ' Lista media por meses
            range = objSheet.Range("E42")
            range = range.Resize(5, 12)
            For i = 0 To 11
                range(1, i + 1) = Me._HabMagnitudMensualTablaAlt(0).ndias(i)
                range(2, i + 1) = Me._HabMagnitudMensualTablaAlt(1).ndias(i)
                range(3, i + 1) = Me._HabMagnitudMensualTablaAlt(2).ndias(i)
                range(4, i + 1) = Me._HabEstacionalidadMensualAlt(0).ndias(i)
                range(5, i + 1) = Me._HabEstacionalidadMensualAlt(1).ndias(i)
            Next

            ' Escribir los meses
            range = objSheet.Range("E41")
            range = range.Resize(1, 12)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(1, i + 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1).Substring(0, 3)
            Next

        End Sub
        Private Sub EscribirInforme6a()
            Dim range As Excel.Range
            Dim i As Integer
            'Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(14)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("F67")
            range = range.Resize(365, 1)
            For i = 0 To 364
                range(i + 1, 1) = Me._TablaCQCNat.añomedio(i)
            Next
        End Sub
        Private Sub EscribirInforme6()
            Dim range As Excel.Range
            Dim i As Integer
            'Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(13)

            range = objSheet.Range("D5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("D6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("D7")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("E67")
            range = range.Resize(365, 1)
            For i = 0 To 364
                range(i + 1, 1) = Me._TablaCQCNat.añomedio(i)
            Next
            range = objSheet.Range("F67")
            range = range.Resize(365, 1)
            For i = 0 To 364
                range(i + 1, 1) = Me._TablaCQCAlt.añomedio(i)
            Next

        End Sub
        Private Sub EscribirInforme7a()
            Dim range As Excel.Range
            Dim i As Integer
            'Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(15)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 1: Magnitud aportaciones anuales +++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D15")
            range = range.Resize(1, 2)
            Dim s As String = ""
            If (Me._IndicesHabituales(0).calculado) Then

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(0)) & "15").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(0)
                If (Me._IndicesHabituales(0).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(0).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' --------------------------------------------------------------------
                range = objSheet.Range("D21")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(1)) & "21").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(1)
                If (Me._IndicesHabituales(0).invertido(1)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(0).indeterminacion(1)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ---------------------------------------------------------------------
                range = objSheet.Range("D27")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(2)) & "27").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(2)
                If (Me._IndicesHabituales(0).invertido(2)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(0).indeterminacion(2)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ----------------------------------------------------------------------
                range = objSheet.Range("D33")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(3)) & "33").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(3)
                ' Cambio #289 - Ponderados no llevan signo
                'If (Me._IndicesHabituales(0).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(0).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                range(1, 2) = s
            Else
                range = objSheet.Range("E15")
                range.Value = "#"
                range = objSheet.Range("E21")
                range.Value = "#"
                range = objSheet.Range("E27")
                range.Value = "#"
                range = objSheet.Range("E33")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 2: Magnitud aportaciones mensuales +
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D16")
            range = range.Resize(1, 2)
            s = ""
            If (Me._IndicesHabituales(1).calculado) Then

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(0)) & "16").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(0)
                If (Me._IndicesHabituales(1).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(1).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' -------------------------------------------------------------------------
                range = objSheet.Range("D22")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(1)) & "22").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(1)
                If (Me._IndicesHabituales(1).invertido(1)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(1).indeterminacion(1)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' --------------------------------------------------------------------------
                range = objSheet.Range("D28")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(2)) & "28").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(2)
                If (Me._IndicesHabituales(1).invertido(2)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(1).indeterminacion(2)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ---------------------------------------------------------------------------
                range = objSheet.Range("D34")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(3)) & "34").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(3)
                ' Cambio #289 - Ponderados no llevan signo
                'If (Me._IndicesHabituales(1).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(1).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                range(1, 2) = s
            Else
                range = objSheet.Range("E16")
                range.Value = "#"
                range = objSheet.Range("E22")
                range.Value = "#"
                range = objSheet.Range("E28")
                range.Value = "#"
                range = objSheet.Range("E34")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 3: Variabilidad Habitual  ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D17")
            range = range.Resize(1, 2)
            s = ""
            If (Me._IndicesHabituales(2).calculado) Then

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(0)) & "17").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(2).valor(0)
                If (Me._IndicesHabituales(2).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(2).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' --------------------------------------------------------------------------
                range = objSheet.Range("D23")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(1)) & "23").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(2).valor(1)
                If (Me._IndicesHabituales(2).invertido(1)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(2).indeterminacion(1)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ----------------------------------------------------------------------------
                range = objSheet.Range("D29")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(2)) & "29").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(2).valor(2)
                If (Me._IndicesHabituales(2).invertido(2)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(2).indeterminacion(2)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ------------------------------------------------------------------------------
                range = objSheet.Range("D35")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(2).valor(3)) & "35").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(2).valor(3)
                ' Cambio #289 - Ponderados no llevan signo
                'If (Me._IndicesHabituales(2).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(2).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                range(1, 2) = s
            Else
                range = objSheet.Range("E17")
                range.Value = "#"
                range = objSheet.Range("E23")
                range.Value = "#"
                range = objSheet.Range("E29")
                range.Value = "#"
                range = objSheet.Range("E35")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 4: Variabilidad Extrema  +++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D18")
            range = range.Resize(1, 2)
            s = ""
            If (Me._IndicesHabituales(3).calculado) Then

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(0)) & "18").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(0)
                If (Me._IndicesHabituales(3).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(3).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' --------------------------------------------------------------------
                range = objSheet.Range("D24")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(1)) & "24").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(1)
                If (Me._IndicesHabituales(3).invertido(1)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(3).indeterminacion(1)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ---------------------------------------------------------------------
                range = objSheet.Range("D30")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(2)) & "30").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(2)
                If (Me._IndicesHabituales(3).invertido(2)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(3).indeterminacion(2)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' -----------------------------------------------------------------------
                range = objSheet.Range("D36")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(3)) & "36").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(3)
                ' Cambio #289 - Ponderados no llevan signo
                'If (Me._IndicesHabituales(3).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(3).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                range(1, 2) = s
            Else
                range = objSheet.Range("E18")
                range.Value = "#"
                range = objSheet.Range("E24")
                range.Value = "#"
                range = objSheet.Range("E30")
                range.Value = "#"
                range = objSheet.Range("E36")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ [IAH5] Indice 6: Estacionalidad Maximos ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D19")
            range = range.Resize(1, 2)
            's = ""
            If (Me._IndicesHabituales(5).calculado) Then

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(0)) & "19").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(0)
                'If (Me._IndicesHabituales(5).invertido(0)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(0)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s

                ' --------------------------------------------------------------------
                range = objSheet.Range("D25")
                range = range.Resize(1, 2)
                's = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(1)) & "25").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(1)
                'If (Me._IndicesHabituales(5).invertido(1)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(1)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
                ' --------------------------------------------------------------------------
                range = objSheet.Range("D31")
                range = range.Resize(1, 2)
                's = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(2)) & "31").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(2)
                'If (Me._IndicesHabituales(5).invertido(2)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(2)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
                ' -----------------------------------------------------------------------------
                range = objSheet.Range("D37")
                range = range.Resize(1, 2)
                's = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(3)) & "37").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(3)
                'If (Me._IndicesHabituales(5).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
            Else
                range = objSheet.Range("E19")
                range.Value = "#"
                range = objSheet.Range("E25")
                range.Value = "#"
                range = objSheet.Range("E31")
                range.Value = "#"
                range = objSheet.Range("E37")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ [IAH6] Indice 7: Estacionalidad Minimos ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D20")
            range = range.Resize(1, 2)
            s = ""
            If (Me._IndicesHabituales(6).calculado) Then

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(0)) & "20").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(0)
                'If (Me._IndicesHabituales(6).invertido(0)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(0)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
                ' -------------------------------------------------------------------------------
                range = objSheet.Range("D26")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(1)) & "26").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(1)
                'If (Me._IndicesHabituales(6).invertido(1)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(1)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
                ' ---------------------------------------------------------------------------------
                range = objSheet.Range("D32")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(2)) & "32").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(2)
                'If (Me._IndicesHabituales(6).invertido(2)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(2)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
                ' ------------------------------------------------------------------------
                range = objSheet.Range("D38")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(3)) & "38").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(3)
                'If (Me._IndicesHabituales(6).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
            Else
                range = objSheet.Range("E20")
                range.Value = "#"
                range = objSheet.Range("E26")
                range.Value = "#"
                range = objSheet.Range("E32")
                range.Value = "#"
                range = objSheet.Range("E38")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++
            ' +++++ Indices IAG +++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("G43")
            range = range.Resize(4, 1)

            Dim auxCol As Integer = 43

            For i = 0 To 3
                range(i + 1, 1) = Me._IndiceIAG(i)

                m_Excel.Sheets(15).Select()
                m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG(i)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG(i)) & (auxCol + i).ToString()).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

            Next

        End Sub
        Private Sub EscribirInforme7b()
            Dim range As Excel.Range
            Dim i As Integer
            'Dim pos As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(16)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 1: Magnitud aportaciones anuales +++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D15")
            range = range.Resize(1, 2)
            Dim s As String = ""
            If (Me._IndicesHabituales(0).calculado) Then

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(0)) & "15").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(0)
                If (Me._IndicesHabituales(0).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(0).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ------------------------------------------------------------------
                range = objSheet.Range("D20")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(1)) & "20").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(1)
                If (Me._IndicesHabituales(0).invertido(1)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(0).indeterminacion(1)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' -------------------------------------------------------------------
                range = objSheet.Range("D25")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(2)) & "25").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(2)
                If (Me._IndicesHabituales(0).invertido(2)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(0).indeterminacion(2)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' -------------------------------------------------------------------
                range = objSheet.Range("D30")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(0).valor(3)) & "30").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(0).valor(3)
                ' Cambio #289 - Ponderados no llevan signo
                'If (Me._IndicesHabituales(0).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(0).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                range(1, 2) = s
            Else
                range = objSheet.Range("E15")
                range.Value = "#"
                range = objSheet.Range("E20")
                range.Value = "#"
                range = objSheet.Range("E25")
                range.Value = "#"
                range = objSheet.Range("E30")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 2: Magnitud aportaciones mensuales +
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D16")
            range = range.Resize(1, 2)
            s = ""
            If (Me._IndicesHabituales(1).calculado) Then

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(0)) & "16").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(0)
                If (Me._IndicesHabituales(1).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(1).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' -----------------------------------------------------------------
                range = objSheet.Range("D21")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(1)) & "21").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(1)
                If (Me._IndicesHabituales(1).invertido(1)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(1).indeterminacion(1)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ------------------------------------------------------------------
                range = objSheet.Range("D26")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(2)) & "26").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(2)
                If (Me._IndicesHabituales(1).invertido(2)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(1).indeterminacion(2)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' --------------------------------------------------------------------
                range = objSheet.Range("D31")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(1).valor(3)) & "31").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(1).valor(3)
                ' Cambio #289 - Ponderados no llevan signo
                'If (Me._IndicesHabituales(1).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(1).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                range(1, 2) = s
            Else
                range = objSheet.Range("E16")
                range.Value = "#"
                range = objSheet.Range("E21")
                range.Value = "#"
                range = objSheet.Range("E26")
                range.Value = "#"
                range = objSheet.Range("E31")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 3: Variabilidad Habitual  ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            'If (Me._IndicesHabituales(2).calculado) Then
            'Else
            '    range = objSheet.Range("E17")
            '    range.Value = "#"
            '    range = objSheet.Range("E23")
            '    range.Value = "#"
            '    range = objSheet.Range("E29")
            '    range.Value = "#"
            '    range = objSheet.Range("E35")
            '    range.Value = "#"
            'End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 4: Variabilidad Extrema  +++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D17")
            range = range.Resize(1, 2)
            s = ""
            If (Me._IndicesHabituales(3).calculado) Then

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(0)) & "17").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(0)
                If (Me._IndicesHabituales(3).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(3).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' -----------------------------------------------------------------
                range = objSheet.Range("D22")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(1)) & "22").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(1)
                If (Me._IndicesHabituales(3).invertido(1)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(3).indeterminacion(1)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' ------------------------------------------------------------------
                range = objSheet.Range("D27")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(2)) & "27").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(2)
                If (Me._IndicesHabituales(3).invertido(2)) Then
                    s = "*"
                End If
                If (Me._IndicesHabituales(3).indeterminacion(2)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
                ' --------------------------------------------------------------------
                range = objSheet.Range("D32")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(3).valor(3)) & "32").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(3).valor(3)
                ' Cambio #289 - Ponderados no llevan signo
                'If (Me._IndicesHabituales(3).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(3).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                range(1, 2) = s
            Else
                range = objSheet.Range("E17")
                range.Value = "#"
                range = objSheet.Range("E22")
                range.Value = "#"
                range = objSheet.Range("E27")
                range.Value = "#"
                range = objSheet.Range("E32")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 6: Estacionalidad Maximos ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D18")
            range = range.Resize(1, 2)
            's = ""
            If (Me._IndicesHabituales(5).calculado) Then

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(0)) & "18").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(0)
                'If (Me._IndicesHabituales(5).invertido(0)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(0)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s

                ' -----------------------------------------------------------------------
                range = objSheet.Range("D23")
                range = range.Resize(1, 2)
                's = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(1)) & "23").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(1)
                'If (Me._IndicesHabituales(5).invertido(1)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(1)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s

                ' ------------------------------------------------------------------------
                range = objSheet.Range("D28")
                range = range.Resize(1, 2)
                's = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(2)) & "28").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(2)
                'If (Me._IndicesHabituales(5).invertido(2)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(2)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s

                ' -----------------------------------------------------------------------
                range = objSheet.Range("D33")
                range = range.Resize(1, 2)
                's = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(5).valor(3)) & "33").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(5).valor(3)
                'If (Me._IndicesHabituales(5).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(5).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
            Else
                range = objSheet.Range("E18")
                range.Value = "#"
                range = objSheet.Range("E23")
                range.Value = "#"
                range = objSheet.Range("E28")
                range.Value = "#"
                range = objSheet.Range("E33")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++++++
            ' +++ Indice 7: Estacionalidad Minimos ++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("D19")
            range = range.Resize(1, 2)
            s = ""
            If (Me._IndicesHabituales(6).calculado) Then

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(0)) & "19").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(0)
                'If (Me._IndicesHabituales(6).invertido(0)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(0)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s

                ' -------------------------------------------------------------------------
                range = objSheet.Range("D24")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(1)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(1)) & "24").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(1)
                'If (Me._IndicesHabituales(6).invertido(1)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(1)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s

                ' --------------------------------------------------------------------------
                range = objSheet.Range("D29")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(2)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(2)) & "29").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(2)
                'If (Me._IndicesHabituales(6).invertido(2)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(2)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s

                ' ------------------------------------------------------------------------------
                range = objSheet.Range("D34")
                range = range.Resize(1, 2)
                s = ""

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(3)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabituales(6).valor(3)) & "34").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabituales(6).valor(3)
                'If (Me._IndicesHabituales(6).invertido(3)) Then
                '    s = "*"
                'End If
                'If (Me._IndicesHabituales(6).indeterminacion(3)) Then
                '    s = s & "**"
                'End If
                'range(1, 2) = s
            Else
                range = objSheet.Range("E19")
                range.Value = "#"
                range = objSheet.Range("E24")
                range.Value = "#"
                range = objSheet.Range("E29")
                range.Value = "#"
                range = objSheet.Range("E34")
                range.Value = "#"
            End If

            ' +++++++++++++++++++++++++++++++++++++++++++
            ' +++++ Indices IAG +++++++++++++++++++++++++
            ' +++++++++++++++++++++++++++++++++++++++++++
            range = objSheet.Range("G39")
            range = range.Resize(4, 1)

            Dim auxCol As Integer = 39

            For i = 0 To 3
                range(i + 1, 1) = Me._IndiceIAG(i)

                m_Excel.Sheets(16).Select()
                m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG(i)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG(i)) & (auxCol + i).ToString()).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

            Next

        End Sub
        Private Sub EscribirInforme7c()
            Dim range As Excel.Range
            Dim i As Integer
            'Dim pos As Integer
            Dim s As String

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(17)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            range = objSheet.Range("D15")
            range = range.Resize(7, 2)

            If (Me._IndicesHabitualesAgregados(0).calculado) Then
                s = ""

                m_Excel.Sheets(17).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(0).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(0).valor(0)) & "15").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(1, 1) = Me._IndicesHabitualesAgregados(0).valor(0)
                If (Me._IndicesHabitualesAgregados(0).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabitualesAgregados(0).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(1, 2) = s
            Else
                range(1, 2) = "#"
            End If
            If (Me._IndicesHabitualesAgregados(1).calculado) Then
                s = ""

                m_Excel.Sheets(17).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(1).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(1).valor(0)) & "16").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(2, 1) = Me._IndicesHabitualesAgregados(1).valor(0)
                If (Me._IndicesHabitualesAgregados(1).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabitualesAgregados(1).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(2, 2) = s
            Else
                range(2, 2) = "#"
            End If
            If (Me._IndicesHabitualesAgregados(2).calculado) Then
                s = ""

                m_Excel.Sheets(17).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(2).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(2).valor(0)) & "17").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(3, 1) = Me._IndicesHabitualesAgregados(2).valor(0)
                If (Me._IndicesHabitualesAgregados(2).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabitualesAgregados(2).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(3, 2) = s
            Else
                range(3, 2) = "#"
            End If
            If (Me._IndicesHabitualesAgregados(3).calculado) Then
                s = ""

                m_Excel.Sheets(17).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(3).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(3).valor(0)) & "18").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(4, 1) = Me._IndicesHabitualesAgregados(3).valor(0)
                If (Me._IndicesHabitualesAgregados(3).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabitualesAgregados(3).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(4, 2) = s
            Else
                range(4, 2) = "#"
            End If
            If (Me._IndicesHabitualesAgregados(4).calculado) Then
                s = ""

                m_Excel.Sheets(17).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(4).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(4).valor(0)) & "19").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(5, 1) = Me._IndicesHabitualesAgregados(4).valor(0)
                If (Me._IndicesHabitualesAgregados(4).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabitualesAgregados(4).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(5, 2) = s
            Else
                range(5, 2) = "#"
            End If
            If (Me._IndicesHabitualesAgregados(5).calculado) Then
                s = ""

                m_Excel.Sheets(17).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(5).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(5).valor(0)) & "20").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(6, 1) = Me._IndicesHabitualesAgregados(5).valor(0)
                If (Me._IndicesHabitualesAgregados(5).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabitualesAgregados(5).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(6, 2) = s
            Else
                range(6, 2) = "#"
            End If
            If (Me._IndicesHabitualesAgregados(6).calculado) Then
                s = ""

                m_Excel.Sheets(17).Select()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(6).valor(0)) & "14").Select()
                m_Excel.Selection.Copy()
                m_Excel.Range(Me.DarColumna(Me._IndicesHabitualesAgregados(6).valor(0)) & "21").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                m_Excel.CutCopyMode = False

                range(7, 1) = Me._IndicesHabitualesAgregados(6).valor(0)
                If (Me._IndicesHabitualesAgregados(6).invertido(0)) Then
                    s = "*"
                End If
                If (Me._IndicesHabitualesAgregados(6).indeterminacion(0)) Then
                    s = s & "**"
                End If
                range(7, 2) = s
            Else
                range(7, 2) = "#"
            End If

            range = objSheet.Range("G44")
            range.Value = Me._IndiceIAG_Agregados

            m_Excel.Sheets(17).Select()
            m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG_Agregados) & "14").Select()
            m_Excel.Selection.Copy()
            m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG_Agregados) & "44").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False


            range = objSheet.Range("F27")
            range = range.Resize(12, 3)
            For i = 0 To 11
                s = ""
                If (Me._IndiceM3Agregados.invertido(i) = True) Then
                    s &= "*"
                End If
                If (Me._IndiceM3Agregados.indeterminacion(i) = True) Then
                    s &= "**"
                End If
                range(i + 1, 1) = s & " " & Me._IndiceM3Agregados.valor(i).ToString("F2")
                s = ""
                If (Me._IndiceV3Agregados.invertido(i) = True) Then
                    s &= "*"
                End If
                If (Me._IndiceV3Agregados.indeterminacion(i) = True) Then
                    s &= "**"
                End If
                range(i + 1, 3) = s & " " & Me._IndiceV3Agregados.valor(i).ToString("F2")
            Next

            ' Escribir los meses
            range = objSheet.Range("D27")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1).Substring(0, 3)
            Next

        End Sub
        Private Sub EscribirInforme7d()
            Dim range As Excel.Range
            Dim i As Integer
            Dim pos As Integer
            Dim posCelda As Integer
            Dim s As String

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(18)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre

            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion

            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            pos = 0
            posCelda = 15

            For i = 0 To 7
                range = objSheet.Range("D" & posCelda.ToString())
                range = range.Resize(1, 2)
                If (Me._IndicesAvenidas(pos).valor(0) >= 0) Then
                    s = ""
                    If (Me._IndicesAvenidas(pos).invertido(0)) Then
                        s = "*"
                    End If
                    If (Me._IndicesAvenidas(pos).indeterminacion(0)) Then
                        s = s & "**"
                    End If
                    range(1, 2) = s

                    m_Excel.Sheets(18).Select()
                    m_Excel.Range(Me.DarColumna(Me._IndicesAvenidas(pos).valor(0)) & "14").Select()
                    m_Excel.Selection.Copy()
                    m_Excel.Range(Me.DarColumna(Me._IndicesAvenidas(pos).valor(0)) & posCelda.ToString()).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                    m_Excel.CutCopyMode = False

                    range(1, 1) = Me._IndicesAvenidas(pos).valor(0)
                Else
                    range(1, 2) = "#"
                End If
                pos = pos + 1
                posCelda = posCelda + 1
            Next

            pos = 0
            posCelda = 23
            For i = 0 To 6

                range = objSheet.Range("D" & posCelda.ToString())
                range = range.Resize(1, 2)
                If (Me._IndicesSequias(pos).valor(0) >= 0) Then
                    s = ""
                    If (Me._IndicesSequias(pos).invertido(0)) Then
                        s = "*"
                    End If
                    If (Me._IndicesSequias(pos).indeterminacion(0)) Then
                        s = s & "**"
                    End If
                    range(1, 2) = s

                    m_Excel.Sheets(18).Select()
                    m_Excel.Range(Me.DarColumna(Me._IndicesSequias(pos).valor(0)) & "14").Select()
                    m_Excel.Selection.Copy()
                    m_Excel.Range(Me.DarColumna(Me._IndicesSequias(pos).valor(0)) & posCelda.ToString()).PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
                    m_Excel.CutCopyMode = False

                    range(1, 1) = Me._IndicesSequias(pos).valor(0)
                Else
                    range(1, 2) = "#"
                End If
                pos = pos + 1
                posCelda = posCelda + 1
            Next

            Dim aux As Integer = 34
            For i = 0 To 11
                range = objSheet.Range("K" & aux)
                range = range.Resize(1, 3)
                'If (Me._IndicesAvenidasI16MesesInversos(i)) Then
                '    range(1, 1) = Me._IndicesAvenidasI16Meses(i) & " *"
                'Else
                range(1, 1) = Me._IndicesAvenidasI16Meses(i)
                'End If

                'If (Me._IndicesSequiasI23MesesInversos(i)) Then
                '    range(1, 2) = Me._IndicesSequiasI23Meses(i) & " *"
                'Else
                range(1, 2) = Me._IndicesSequiasI23Meses(i)
                'End If
                'If (Me._IndicesSequiasI24MesesInversos(i)) Then
                '    range(1, 3) = Me._IndicesSequiasI24Meses(i) & " *"
                'Else
                range(1, 3) = Me._IndicesSequiasI24Meses(i)
                'End If


                aux = aux + 1
            Next

            range = objSheet.Range("G49")
            range.Value = Me._IndiceIAG_Ave
            m_Excel.Sheets(18).Select()
            m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG_Ave) & "14").Select()
            m_Excel.Selection.Copy()
            m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG_Ave) & "49").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            range = objSheet.Range("G50")
            range.Value = Me._IndiceIAG_Seq
            m_Excel.Sheets(18).Select()
            m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG_Seq) & "14").Select()
            m_Excel.Selection.Copy()
            m_Excel.Range(Me.DarColumnaGlobales(Me._IndiceIAG_Seq) & "50").PasteSpecial(Paste:=xlPasteFormats, Operation:=xlNone, SkipBlanks:=False, Transpose:=False)
            m_Excel.CutCopyMode = False

            ' Escribir los meses
            range = objSheet.Range("I34")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

        End Sub
        Private Sub EscribirInforme8()
            Dim range As Excel.Range
            Dim i As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(19)

            ' Escribir cabecera
            range = objSheet.Range("D5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("D6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("D7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribir años usados
            range = objSheet.Range("B36")
            range = range.Resize(1, 3)
            range(1, 1) = Me._AportacionNatAnual.año.Length
            range(1, 2) = Me._AportacionAltAnual.año.Length
            range(1, 3) = Me._datos.nAnyosCoe

            ' Escribir las regimen mensual
            range = objSheet.Range("C14")
            range = range.Resize(12, 3)
            For i = 0 To 11
                range(i + 1, 1) = Me._percentil10(i)
                range(i + 1, 2) = Me._medianaMenNat(i)
                range(i + 1, 3) = Me._percentil90(i)
            Next
            range = objSheet.Range("F14")
            range = range.Resize(12, 2)
            For i = 0 To 11
                range(i + 1, 1) = Me._medianaMenAlt(i)
                range(i + 1, 2) = Me._mesesQueCumplen(i)
            Next

            ' Escribir regimen anual
            range = objSheet.Range("C32")
            range = range.Resize(1, 5)
            range(1, 1) = Me._percentil10Anual
            range(1, 2) = Me._medianaAnualNat
            range(1, 3) = Me._percentil90Anual
            range(1, 4) = Me._medianaAnualAlt
            range(1, 5) = Me._anyosQueCumplen

            ' Leer los resultados y rellenar los colores e etiquetas
            range = objSheet.Range("I14")
            range = range.Resize(12, 2)
            For i = 0 To 11
                If (range(i + 1, 1).Value > 50) Then
                    range(i + 1, 2).Interior.Color = System.Drawing.ColorTranslator.ToWin32(Color.Green)
                End If
            Next
            'range = objSheet.Range("I26")
            'range = range.Resize(2, 2)
            'If (range(1, 1).Value < 50) Then
            '    range(1, 2).Value = "MUY ALTERADA"
            'End If
            'range = objSheet.Range("I32")
            'range = range.Resize(2, 2)
            'If (range(1, 1).Value < 50) Then
            '    range(1, 2).Value = "MUY ALTERADA"
            'End If

            ' Escribir los meses
            range = objSheet.Range("B14")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

            objSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            objSheet.EnableSelection = Excel.XlEnableSelection.xlNoSelection
        End Sub
        Private Sub EscribirInforme8a()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(20)

            objSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            objSheet.EnableSelection = Excel.XlEnableSelection.xlNoSelection
        End Sub
        Private Sub EscribirInforme8b()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(21)

            objSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            objSheet.EnableSelection = Excel.XlEnableSelection.xlNoSelection
        End Sub
        Private Sub EscribirInforme8c()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(22)

            objSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            objSheet.EnableSelection = Excel.XlEnableSelection.xlNoSelection
        End Sub
        Private Sub EscribirInforme8d()

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(23)

            objSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            objSheet.EnableSelection = Excel.XlEnableSelection.xlNoSelection
        End Sub
        Private Sub EscribirInforme9()
            Dim range As Excel.Range
            Dim i As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(24)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribir las aportaciones
            range = objSheet.Range("C16")
            range = range.Resize(12, 3)
            For i = 0 To 11
                range(i + 1, 1) = Me._IntraAnualNat(i)(0)
                range(i + 1, 2) = Me._IntraAnualNat(i)(1)
                range(i + 1, 3) = Me._IntraAnualNat(i)(2)
            Next

            ' Escribir las aportaciones
            range = objSheet.Range("C98")
            range = range.Resize(12, 1)
            range(1, 1) = Me._1QMin
            range(2, 1) = Me._7QMin
            range(3, 1) = Me._15QMin
            range(4, 1) = Me._7QRetorno(0)
            range(5, 1) = Me._7QRetorno(1)
            range(6, 1) = Me._7QRetorno(2)
            range(7, 1) = Me._10QRetorno(0)
            range(8, 1) = Me._10QRetorno(1)
            range(9, 1) = Me._10QRetorno(2)
            range(10, 1) = Me._mnQ(0)
            range(11, 1) = Me._mnQ(1)
            range(12, 1) = Me._mnQ(2)

            ' Escribir los meses
            range = objSheet.Range("B16")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

            ' Escribir los meses
            range = objSheet.Range("L16")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

            objSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            objSheet.EnableSelection = Excel.XlEnableSelection.xlNoSelection
        End Sub
        Private Sub EscribirInforme9a()
            Dim range As Excel.Range
            Dim i As Integer

            If (Me.m_Excel Is Nothing) Then
                AbrirExcel()
            End If

            Dim objSheets As Excel.Sheets = Me.m_Excel.Worksheets
            Dim objSheet As Excel._Worksheet = objSheets.Item(25)

            range = objSheet.Range("E5")
            range.Value = Me._datos.sNombre
            range = objSheet.Range("E6")
            range.Value = Me._datos.sAlteracion
            range = objSheet.Range("E7")
            range.Value = Date.Now().ToShortDateString()

            ' Escribir las aportaciones
            range = objSheet.Range("C16")
            range = range.Resize(12, 3)
            For i = 0 To 11
                range(i + 1, 1) = Me._IntraAnualNat(i)(0)
                range(i + 1, 2) = Me._IntraAnualNat(i)(1)
                range(i + 1, 3) = Me._IntraAnualNat(i)(2)
            Next

            ' Escribir las aportaciones
            'range = objSheet.Range("C98")
            'range = range.Resize(12, 1)
            'range(1, 1) = Me._1QMin
            'range(2, 1) = Me._7QMin
            'range(3, 1) = Me._1QMin
            'range(4, 1) = Me._7QRetorno(0)
            'range(5, 1) = Me._7QRetorno(1)
            'range(6, 1) = Me._7QRetorno(2)
            'range(7, 1) = Me._10QRetorno(0)
            'range(8, 1) = Me._10QRetorno(1)
            'range(9, 1) = Me._10QRetorno(2)
            'range(10, 1) = Me._mnQ(0)
            'range(11, 1) = Me._mnQ(1)
            'range(12, 1) = Me._mnQ(2)

            ' Escribir los meses
            range = objSheet.Range("B16")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

            ' Escribir los meses
            range = objSheet.Range("L16")
            range = range.Resize(12, 1)
            For i = 0 To 11
                'Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                range(i + 1, 1) = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, (i + Me._datos.mesInicio - 1) Mod 12 + 1)
            Next

            objSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True)
            objSheet.EnableSelection = Excel.XlEnableSelection.xlNoSelection
        End Sub

        Private Function DarColumna(ByVal valor As Single) As String
            If (valor > 0.8) Then
                Return "I"
            ElseIf (valor > 0.6) Then
                Return "J"
            ElseIf (valor > 0.4) Then
                Return "K"
            ElseIf (valor > 0.2) Then
                Return "L"
            Else
                Return "M"
            End If
        End Function
        Private Function DarColumnaGlobales(ByVal valor As Single) As String
            If (valor > 0.64) Then
                Return "I"
            ElseIf (valor > 0.36) Then
                Return "J"
            ElseIf (valor > 0.16) Then
                Return "K"
            ElseIf (valor > 0.04) Then
                Return "L"
            Else
                Return "M"
            End If
        End Function
#End Region

    End Class
End Namespace
