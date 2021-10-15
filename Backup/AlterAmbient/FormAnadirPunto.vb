Imports VB = Microsoft.VisualBasic
Public Class FormAnadirPunto

    Private _cMDB As BBDD.OleDbDataBase
    Private _tabla As String
    Private _rellenar As Rellenar.RellenarForm
    Private _traductor As MultiLangXML.MultiIdiomasXML
    Private _error As Boolean
    Public Sub New(ByVal MDB As BBDD.OleDbDataBase, ByVal tipo As String, ByVal id_proyecto As Integer)


        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        If (tipo = "Punto") Then
            Me.Name = "FormAnadirPunto"
        Else
            Me.Name = "FormAnadirAlteracion"
        End If

        ' -------------------------------------
        ' ---- Traducir formulario ------------
        ' -------------------------------------
        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")

        Me._error = False

        ' Add any initialization after the InitializeComponent() call.
        Me._cMDB = MDB

        If tipo = "Punto" Then
            Me.gbAnadirPunto.Text = "Datos del Punto"
        Else
            Me.gbAnadirPunto.Text = "Datos de la Alteración"
        End If
        Me.lblCodigo.Text = Me.lblCodigo.Text & tipo
        Me.lblDescrip.Text = Me.lblDescrip.Text & tipo
        If (tipo = "Alteración") Then

            'Me.gbAnadirPunto.Name = "gbAnadirAlt"
            'Me.lblCodigo.Name = "lblCodigoAlt"
            'Me.lblDescrip.Name = "lblDescripAlt"
            'Me.lblProyecto.Name = "lblProyectoAlt"

            Me._tabla = "Alteracion"

            ' Rellenar con los puntos del sistema
            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Puntos", "SELECT * FROM Punto WHERE ID_Proyecto = " & id_proyecto & " ORDER BY clave_punto ASC")

            If (ds.Tables(0).Rows.Count = 0) Then
                'Dim strError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPoint", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")
                'Dim strCaptionError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")

                'MessageBox.Show(strError, strCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)

                Me._error = True
                Return
            End If

            'Dim dr As DataRow
            Me.cmbPuntos.Items.Clear() ' Limpiar el listbox
            For Each dr As DataRow In ds.Tables(0).Rows
                Me.cmbPuntos.Items.Add(dr("Clave_punto").ToString())
                'Console.WriteLine(dr("nombre").ToString())
            Next

            Me.cmbPuntos.SelectedIndex = 0

        Else
            Me._tabla = "Punto"
            Me.lblProyecto.Text = "Proyecto asociado"

            Me._rellenar = New Rellenar.RellenarForm(Me._cMDB)
            Me._rellenar.RellenarProyectos(Me.cmbPuntos)

            'Me.Label1.Visible = False
            'Me.cmbPuntos.Visible = False
        End If


        

    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If (Me.txtClave.Text = "") Or (Me.txtNombre.Text = "") Then
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strEmptyValues"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim s() As String
        If (Me._tabla = "Alteracion") Then
            ReDim s(2)
            s(0) = "COD_Alteracion"
            s(1) = "nombre"
            s(2) = "ID_Punto"

            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Puntos", "Select id_punto From Punto where clave_punto='" & Me.cmbPuntos.SelectedItem.ToString() & "'")
            Dim dr As DataRow = ds.Tables(0).Rows(0)

            Dim t As String() = {Me.txtClave.Text.ToUpperInvariant(), Me.txtNombre.Text, dr(0).ToString()}

            If (existeCodigo("alteracion", Me.txtClave.Text.ToUpperInvariant())) Then
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertAltDupId"), _
                                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If (Me._cMDB.InsertarRegistro(Me._tabla, s, t)) Then
                Me.Dispose()
            Else
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertAlt"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If


        Else
            ReDim s(3)
            s(0) = "Clave_punto"
            s(1) = "nombre"
            s(2) = "ID_proyecto"
            s(3) = "mesInicio"

            Dim ds As DataSet = Me._cMDB.RellenarDataSet("Proyectos", "SELECT ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC")

            Dim dr As DataRow = ds.Tables(0).Rows(Me.cmbPuntos.SelectedIndex)

            If (existeCodigo("punto", Me.txtClave.Text.ToUpperInvariant())) Then
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertPointDupId"), _
                                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim t As String() = {Me.txtClave.Text.ToUpperInvariant(), Me.txtNombre.Text, dr(0), 10}

            If (Me._cMDB.InsertarRegistro(Me._tabla, s, t)) Then
                Me.Dispose()
            Else
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertPoint"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

        End If

    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Me.Dispose()
    End Sub
    Private Sub FormAnadirPunto_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim strText As String
        If e.KeyCode = Keys.F1 Then
            If Me.Text = "Añadir Punto" Then
                strText = "Añadir punto"
            Else
                strText = "Añadir alteración"
            End If
            'Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, strText)
        End If
    End Sub
    Private Sub FormAnadirPunto_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Centrar el formulario
        'Me.Left = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width)
        'Me.Top = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height)

        'If Me._tabla = "Punto" Then
        '    Me.Text = "Añadir " & Me._tabla
        'Else
        '    Me.Text = "Añadir Alteración"
        'End If

        'Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, "Añadir punto")
    End Sub
    
    Private Sub txtClave_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtClave.TextChanged
        If Len(txtClave.Text) > 12 Then
            'MsgBox("", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR DE CÓDIGO")
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongCode"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtClave.Text = VB.Left(txtClave.Text, 12)
        End If
    End Sub
    Private Sub txtNombre_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombre.TextChanged
        If Len(txtNombre.Text) > 20 Then
            MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongName"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            'MsgBox("", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR DE DESCRIPCIÓN")
            txtNombre.Text = VB.Left(txtNombre.Text, 20)
        End If
    End Sub

    Private Sub FormAnadirPunto_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If (Me._error) Then
            Me.Close()
        End If
    End Sub

    Private Function existeCodigo(ByVal tabla As String, ByVal codigo As String)

        Dim ds As DataSet
        Dim dr As DataRow
        If (tabla = "punto") Then
            ds = Me._cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM punto WHERE clave_punto='" & codigo & "'")
            dr = ds.Tables(0).Rows(0)

            If (dr(0) > 0) Then
                Return True
            Else
                Return False
            End If
        ElseIf (tabla = "alteracion") Then
            ds = Me._cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM alteracion WHERE COD_Alteracion='" & codigo & "'")
            dr = ds.Tables(0).Rows(0)

            If (dr(0) > 0) Then
                Return True
            Else
                Return False
            End If
        End If
        Return False
        
    End Function
End Class
