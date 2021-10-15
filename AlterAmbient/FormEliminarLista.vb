Imports IAHRIS.Rellenar.RellenarForm

Public Class FormEliminarLista

    Dim _Rellenar As Rellenar.RellenarForm
    Dim _cMDB As BBDD.OleDbDataBase

    Dim _idpunto As Integer
    Dim _codAlt As Integer

    Private _traductor As MultiLangXML.MultiIdiomasXML

    Public Sub New(ByVal MDB As BBDD.OleDbDataBase)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._cMDB = MDB

        Me._Rellenar = New Rellenar.RellenarForm(Me._cMDB)

        ' -------------------------------------
        ' ---- Traducir formulario ------------
        ' -------------------------------------
        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")
    End Sub

    Private Sub FormEliminarLista_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F1 Then
            'Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, "Eliminar serie")
        End If
    End Sub

    Private Sub FormEliminarLista_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me._Rellenar.RellenarPuntos(Me.lstboxListas)
        Me.btnBorrarNatDiaria.Enabled = False
        Me.btnBorrarNatMensual.Enabled = False
        Me.btnAltDiaria.Enabled = False
        Me.btnAltMensual.Enabled = False

        Me.Left = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width)
        Me.Top = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height)
    End Sub

    Private Sub lboxListas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstboxListas.SelectedIndexChanged

        Dim ds As DataSet
        Dim draux As DataRow

        Me.btnBorrarNatDiaria.Enabled = False
        Me.btnBorrarNatMensual.Enabled = False
        Me.btnAltDiaria.Enabled = False
        Me.btnAltMensual.Enabled = False

        ds = Me._cMDB.RellenarDataSet("Valor", "SELECT id_punto FROM [Punto] WHERE Clave_Punto='" & Me.lstboxListas.SelectedItem.ToString() & "'")

        draux = ds.Tables(0).Rows(0)
        Me._idpunto = draux(0)
        ' Sacar nombre y id
        If (Me.lstboxListas.SelectedIndex <> -1) Then
            ds = Me._cMDB.RellenarDataSet("Valor", "SELECT * FROM [Lista] WHERE ID_punto=" & Me._idpunto.ToString())
            'dr = ds.Tables(0).Rows(0)
            For Each dr As DataRow In ds.Tables(0).Rows
                ' Borrar lista natural diaria
                If (dr("Tipo_Lista") And dr("Tipo_Fechas")) Then

                    Me.btnBorrarNatDiaria.Enabled = True

                ElseIf (dr("Tipo_Lista") And (Not dr("Tipo_Fechas"))) Then

                    Me.btnBorrarNatMensual.Enabled = True

                End If
            Next
        End If

        Me._Rellenar.RellenarAlteraciones(Me.cmboxAlt, Me._idpunto)

    End Sub

    Private Sub cmboxAlt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmboxAlt.SelectedIndexChanged
        Dim ds As DataSet

        Dim draux As DataRow

        Me.btnAltDiaria.Enabled = False
        Me.btnAltMensual.Enabled = False

        ds = Me._cMDB.RellenarDataSet("Valor", "SELECT id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" & Me.cmboxAlt.SelectedItem.ToString() & "'")

        draux = ds.Tables(0).Rows(0)

        Me._codAlt = draux(0)

        ds = Me._cMDB.RellenarDataSet("Valor", "SELECT * FROM [Lista] WHERE ID_punto=" & Me._idpunto.ToString() & " AND id_alteracion=" & Me._codAlt.ToString())
        'dr = ds.Tables(0).Rows(0)
        For Each dr As DataRow In ds.Tables(0).Rows
            ' Borrar lista natural diaria
            If (dr("Tipo_Fechas")) Then
                Me.btnAltDiaria.Enabled = True
            ElseIf (Not dr("Tipo_Fechas")) Then
                Me.btnAltMensual.Enabled = True
            End If
        Next
    End Sub

    Private Sub btnBorrarNatDiaria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrarNatDiaria.Click
        Me._cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " & Me._idpunto & " AND Tipo_Lista=TRUE AND Tipo_fechas=TRUE AND id_alteracion=0")
        lboxListas_SelectedIndexChanged(Me.lstboxListas, Nothing)
    End Sub

    Private Sub btnBorrarNatMensual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrarNatMensual.Click
        Me._cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " & Me._idpunto & " AND Tipo_Lista=TRUE AND Tipo_fechas=FALSE AND id_alteracion=0")
        lboxListas_SelectedIndexChanged(Me.lstboxListas, Nothing)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAltDiaria.Click
        'Dim idLista As Integer

        'Dim ds As DataSet
        'Dim dr As DataRow

        '' Sacar nombre y id
        'If (Me.lstboxListas.SelectedIndex <> -1) Then
        '    ds = Me._cMDB.RellenarDataSet("Valor", "SELECT id_lista FROM Lista WHERE nombre='" & Me.lstboxListas.SelectedItem.ToString() & "'")

        '    dr = ds.Tables(0).Rows(0)

        '    idLista = Integer.Parse(dr("id_lista"))

        '    Me._cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_lista = " & idLista)

        '    Me._Rellenar.RellenarListas(Me.lstboxListas, -1)
        'End If
        Me._cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " & Me._idpunto & " AND Tipo_Lista=FALSE AND Tipo_fechas=TRUE AND id_alteracion=" & Me._codAlt.ToString())
        lboxListas_SelectedIndexChanged(Me.lstboxListas, Nothing)
    End Sub

    Private Sub btnAltMensual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAltMensual.Click
        Me._cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " & Me._idpunto & " AND Tipo_Lista=FALSE AND Tipo_fechas=FALSE AND id_alteracion=" & Me._codAlt.ToString())
        lboxListas_SelectedIndexChanged(Me.lstboxListas, Nothing)
    End Sub
End Class