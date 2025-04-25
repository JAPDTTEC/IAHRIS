Imports System.Xml.XPath
Imports System.Xml
Imports System.Xml.Schema
Imports System.Windows.Forms

Namespace MultiLangXML

    Public Class MultiIdiomasXML
        Dim _form As System.Windows.Forms.Form

        Dim _appPath As String
        Dim _rutaConf As String
        Dim _rutaXML As String

        Dim _rutaExcel As String

        Dim _OK As Boolean

        Public Enum TIPO_MENSAJE
            M_ERROR = 0
            M_INFO = 1
            M_OTHER = 2
            M_TABLE = 3
            M_MONTH = 4
        End Enum

        ''' <summary>
        ''' Creación de la clase
        ''' </summary>
        ''' <param name="form">Formulario a traducir</param>
        ''' <remarks></remarks>
        Public Sub New(ByRef form As System.Windows.Forms.Form)
            Me._form = form
        End Sub

        Public Sub ValidationHandler(ByVal sender As Object, ByVal args As ValidationEventArgs)

            Console.WriteLine("Validation has encounted errors.......................")
            If (args.Severity = XmlSeverityType.Error) Then
                Console.WriteLine("Severity:{0}", args.Severity)
                Console.WriteLine("Message:{0}", args.Message)
                Me._OK = False
            End If


        End Sub


        ''' <summary>
        ''' Traducir formulario
        ''' </summary>
        ''' <param name="rutaXML">Ruta al XML donde se encuentra la traducción</param>
        ''' <param name="rutaXSD">Ruta donde se encuentra el XSD de validación del XML</param>
        ''' <returns>Si todo ha ido bien</returns>
        ''' <remarks>El fichero XSD no se puede modificar y esta unido a cada versión de la librería</remarks>
        Public Function traducirForm(ByVal rutaXML As String, ByVal rutaXSD As String) As Boolean

            Dim xmldoc As XPathDocument
            Dim xmlnav As XPathNavigator

            Try
                xmldoc = New XPathDocument(rutaXML)
                xmlnav = xmldoc.CreateNavigator()
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show(ex.Message.ToString(), "Error")
                Return False
            End Try

            ' Traducir el formulario
            Dim iterador As XPathNodeIterator

            iterador = xmlnav.Select("//forms/form[@id=""" + Me._form.Name + """]")
            If (iterador.MoveNext) Then
                Dim node As XPathNavigator = iterador.Current
                Me._form.Text = node.GetAttribute("string", "")
            End If
            ' Traducir los controles
            For Each ctrl As Control In Me._form.Controls
                Me.traducirControl(ctrl, xmldoc, xmlnav)
            Next

            xmlnav = Nothing
            xmldoc = Nothing

            Return True
        End Function

        Public Function traducirFormPorConf(ByVal rutaApp As String, ByVal rutaXML As String) As Boolean
            Dim xmldoc As XPathDocument
            Dim xmlnav As XPathNavigator
            Dim iterador As XPathNodeIterator
            Dim rutaLangXML As String

            Try
                Me._rutaConf = rutaApp & rutaXML
                Me._appPath = rutaApp
                xmldoc = New XPathDocument(Me._rutaConf)
                xmlnav = xmldoc.CreateNavigator()
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show("No se encuentra el fichero XML", "Error")
                Return False
            End Try

            iterador = xmlnav.Select("configuracion/idioma")
            If (iterador.MoveNext) Then
                rutaLangXML = iterador.Current.Value
                Me._rutaXML = Me._appPath & "\" & rutaLangXML

            Else
                xmlnav = Nothing
                xmldoc = Nothing
                Return False
            End If

            ' Sacar el excel
            xmldoc = New XPathDocument(Me._rutaXML)
            xmlnav = xmldoc.CreateNavigator()
            iterador = xmlnav.Select("/language/excelFile")
            If (iterador.MoveNext) Then
                Me._rutaExcel = Me._appPath & "\Report\" & iterador.Current.Value
                xmlnav = Nothing
                xmldoc = Nothing

            Else
                Me._rutaExcel = ""
                xmlnav = Nothing
                xmldoc = Nothing
                Return False
            End If

            Return traducirForm(rutaApp & "\" & rutaLangXML, "")

        End Function

        ''' <summary>
        ''' Traducir un control
        ''' </summary>
        ''' <param name="ctrlObj">Control a traducir</param>
        ''' <param name="xmldoc">XML que se usa para traducir</param>
        ''' <param name="xmlnav">El navegador de XML</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function traducirControl(ByVal ctrlObj As Object, ByVal xmldoc As XPathDocument, ByVal xmlnav As XPathNavigator) As Boolean

            Dim iterador As XPathNodeIterator

            If TypeOf ctrlObj Is MenuStrip Then
                Dim menu As System.Windows.Forms.MenuStrip = TryCast(ctrlObj, System.Windows.Forms.MenuStrip)
                If menu.Items.Count > 0 Then
                    For Each ctrlAux As System.Windows.Forms.ToolStripItem In menu.Items
                        Me.traducirControl(ctrlAux, xmldoc, xmlnav)
                    Next
                End If
            ElseIf TypeOf ctrlObj Is ToolStripMenuItem Then
                Dim ctrl As ToolStripMenuItem = TryCast(ctrlObj, System.Windows.Forms.ToolStripMenuItem)
                ' Cambiar el TEXT si existe en nuestro XML
                iterador = xmlnav.Select("//forms/form[@id=""" + Me._form.Name + """]/control[@id=""" + ctrl.Name + """]")
                If (iterador.MoveNext) Then
                    ctrl.Text = iterador.Current.Value
                End If

                If (ctrl.DropDownItems.Count > 0) Then
                    For Each ctrlAux As ToolStripMenuItem In ctrl.DropDownItems

                        Me.traducirControl(ctrlAux, xmldoc, xmlnav)

                    Next
                End If
            ElseIf TypeOf ctrlObj Is ComboBox Then
                Dim ctrl As ComboBox = TryCast(ctrlObj, System.Windows.Forms.ComboBox)
                iterador = xmlnav.Select("//forms/form[@id=""" + Me._form.Name + """]/control[@id=""" + ctrl.Name + """]/item")
                If (iterador.Count = 0) Then
                    Return False
                End If
                ' Liberar los items anteriores o por defecto
                ctrl.Items.Clear()
                While (iterador.MoveNext)
                    ctrl.Items.Add(iterador.Current.Value)
                End While
                ctrl.SelectedIndex = 0
            Else
                Dim ctrl As Control = TryCast(ctrlObj, System.Windows.Forms.Control)
                ' Cambiar el TEXT si existe en nuestro XML
                iterador = xmlnav.Select("//forms/form[@id=""" + Me._form.Name + """]/control[@id=""" + ctrl.Name + """]")
                If (iterador.MoveNext) Then

                    Dim stSalida As String = iterador.Current.InnerXml
                    If (iterador.Current.InnerXml.Contains("<br/>") Or iterador.Current.InnerXml.Contains("<br />")) Then
                        stSalida = stSalida.Replace("<br/>", vbCrLf)
                        stSalida = stSalida.Replace("<br />", vbCrLf)
                    Else
                        stSalida = iterador.Current.Value
                    End If

                    ctrl.Text = stSalida
                End If

                ' Comprobar si el control contiene a otros controles
                If (ctrl.Controls.Count > 0) Then
                    For Each ctrlAux As Control In ctrl.Controls

                        Me.traducirControl(ctrlAux, xmldoc, xmlnav)

                    Next
                End If
            End If


            Return True
        End Function

        Public Function traducirMensaje(ByVal tipo As TIPO_MENSAJE, ByVal strID As String) As String
            Dim xmldoc As XPathDocument
            Dim xmlnav As XPathNavigator

            Try
                xmldoc = New XPathDocument(Me._rutaXML)
                xmlnav = xmldoc.CreateNavigator()
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show("No se encuentra el fichero XML", "Error")
                Return ""
            End Try

            Dim iterador As XPathNodeIterator

            Dim stTipo As String = Nothing
            Select Case tipo
                Case TIPO_MENSAJE.M_ERROR
                    stTipo = "//errors/error"
                Case TIPO_MENSAJE.M_INFO
                    stTipo = "//infos/info"
                Case TIPO_MENSAJE.M_OTHER
                    stTipo = "//others/other"
                Case TIPO_MENSAJE.M_TABLE
                    stTipo = "//tables/column"
                Case TIPO_MENSAJE.M_MONTH
                    stTipo = "//months/month"
            End Select

            If (stTipo <> Nothing) Then
                iterador = xmlnav.Select(stTipo + "[@id=""" + strID + """]")
                If (iterador.MoveNext) Then
                    Dim stSalida As String = iterador.Current.InnerXml
                    If (iterador.Current.InnerXml.Contains("<br/>") Or iterador.Current.InnerXml.Contains("<br />")) Then
                        stSalida = stSalida.Replace("<br/>", vbCrLf)
                        stSalida = stSalida.Replace("<br />", vbCrLf)
                    Else
                        stSalida = iterador.Current.Value
                    End If
                    Return stSalida
                Else
                    Return ""
                End If
            Else
                Return ""
            End If


        End Function

        Public Function cambiarIdioma(ByVal ruta As String) As Boolean
            Dim rutaXML As String
            Dim sepStr() As String = {Me._appPath & "\"}
            rutaXML = ruta.Split(sepStr, StringSplitOptions.RemoveEmptyEntries)(0)
            Try
                Dim myXmlDocument As XmlDocument = New XmlDocument()
                myXmlDocument.Load(Me._rutaConf)

                Dim node As XmlNode
                node = myXmlDocument.DocumentElement

                'Dim node2 As XmlNode
                For Each node In node.ChildNodes
                    'Buscar el nodo secundario precio. 
                    'For Each node2 In node.ChildNodes
                    If node.Name = "idioma" Then
                        '                    
                        node.InnerText = rutaXML
                        Exit For
                    End If
                    'Next
                Next
                myXmlDocument.Save(Me._rutaConf)

                'Marcar internamente este cambio
                Me._rutaXML = ruta
                ' Cambiar el excel
                Dim xmldoc As XPathDocument
                Dim xmlnav As XPathNavigator
                Dim iterador As XPathNodeIterator
                xmldoc = New XPathDocument(Me._rutaXML)
                xmlnav = xmldoc.CreateNavigator()
                iterador = xmlnav.Select("/language/excelFile")
                If (iterador.MoveNext) Then
                    Me._rutaExcel = Me._appPath & "\Report\" & iterador.Current.Value
                    xmlnav = Nothing
                    xmldoc = Nothing

                Else
                    Me._rutaExcel = ""
                    System.Windows.Forms.MessageBox.Show("Error al intentar cambiar de idioma, el fichero de informe no existe", "Error")
                    xmlnav = Nothing
                    xmldoc = Nothing
                    Return False
                End If

                Return True

            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show("No se encuentra el fichero XML." & vbCrLf & ex.Message.ToString(), "Error")
                Return False
            End Try
        End Function

        Public Function testFormatXML(ByVal rutaXML As String, ByRef strIdioma As String, ByVal rutaExcel As String) As Boolean
            Dim xmldoc As XPathDocument
            Dim xmlnav As XPathNavigator
            Dim iterador As XPathNodeIterator

            ' Set the validation settings.
            Dim settings As XmlReaderSettings = New XmlReaderSettings()
            settings.ValidationType = ValidationType.Schema
            settings.ValidationFlags = settings.ValidationFlags Or XmlSchemaValidationFlags.ProcessInlineSchema
            settings.ValidationFlags = settings.ValidationFlags Or XmlSchemaValidationFlags.ReportValidationWarnings

            Me._OK = True

            AddHandler settings.ValidationEventHandler, AddressOf ValidationHandler

            ' Create the XmlReader object.
            Dim reader As XmlReader = XmlReader.Create(rutaXML, settings)

            ' Parse the file. 
            While reader.Read()
            End While

            reader.Close()
            settings = Nothing
            reader = Nothing

            If (Not Me._OK) Then
                System.Windows.Forms.MessageBox.Show("El XML no es válido, no tiene el formato correcto", "Error")
                Return False
            End If

            '-----------------------------------------
            Try
                xmldoc = New XPathDocument(rutaXML)
                xmlnav = xmldoc.CreateNavigator()
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show("No se encuentra el fichero XML", "Error")
                Return False
            End Try

            iterador = xmlnav.Select("/language/excelFile")
            If (iterador.MoveNext) Then
                rutaExcel = iterador.Current.Value
            Else
                rutaExcel = ""
                Return False
            End If

            iterador = xmlnav.Select("/language/idString")
            If (iterador.MoveNext) Then
                strIdioma = iterador.Current.Value
                xmlnav = Nothing
                xmldoc = Nothing
                Return True
            Else
                strIdioma = ""
                xmlnav = Nothing
                xmldoc = Nothing
                Return False
            End If
        End Function

        Public ReadOnly Property getRutaExcel() As String
            Get
                Return Me._rutaExcel
            End Get
        End Property


    End Class


End Namespace
