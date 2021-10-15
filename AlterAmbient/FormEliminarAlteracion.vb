Public Class FormEliminarAlteracion

    Dim _cMDB As BBDD.OleDbDataBase
    Dim _Rellenar As Rellenar.RellenarForm

    Dim _idpunto As Integer
    Dim _idalt As Integer
    Dim _nombrePunto As String
    Dim _nombreAlteracion As String

    Dim _tipo As String

    Public Sub New(ByVal MDB As BBDD.OleDbDataBase, ByVal tipo As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._cMDB = MDB
        Me._tipo = tipo

        Me._Rellenar = New Rellenar.RellenarForm(Me._cMDB)

        If (Me._tipo = "Punto") Then
            Me.Text = "Borrar Punto"
            Me.lstboxAlt.Enabled = False
            Me.lblAdvertencia.Text = "Atención:" & vbCrLf & "Al eliminar el punto, se eliminaran las" & _
                                                   vbCrLf & "alteraciones asociadas y todas las series" & _
                                                   vbCrLf & "asociadas al punto y la alteración."
        Else
            Me.Text = "Borrar Alteracion"
            Me.lblAdvertencia.Text = "Atención:" & vbCrLf & "Al eliminar la alteración, se eliminaran" & _
                                                   vbCrLf & "las series asociadas."
        End If

        Me.btnBorrar.Enabled = False
    End Sub
    Private Sub FormEliminarAlteracion_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim strText As String
        If e.KeyCode = Keys.F1 Then
            If Me.Text = "Borrar Punto" Then
                strText = "Eliminar punto"
            Else
                strText = "Borrar alteración"
            End If
            'Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, strText)
        End If
    End Sub


    Private Sub FormEliminarAlteracion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Centrar el formulario
        Me.Left = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width)
        Me.Top = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height)

        Me._Rellenar.RellenarPuntos(Me.lstboxPuntos)
    End Sub

    Private Sub lstboxPuntos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstboxPuntos.SelectedIndexChanged

        Dim ds As DataSet
        Dim dr As DataRow
        'Dim id_punto As Integer
        'Dim nombre As String

        Dim clavePunto As String = Me.lstboxPuntos.SelectedItem.ToString()

        ds = Me._cMDB.RellenarDataSet("Puntos", "SELECT id_punto, nombre FROM Punto WHERE Clave_Punto='" & clavePunto & "'")
        dr = ds.Tables(0).Rows(0)
        Me._idpunto = dr("id_punto")
        Me._nombrePunto = dr("nombre")

        Me._Rellenar.RellenarAlteraciones(Me.lstboxAlt, Me._idpunto)

        If (Me._tipo = "Punto") Then
            Me.btnBorrar.Enabled = True
        Else
            Me.btnBorrar.Enabled = False
        End If

    End Sub

    Private Sub lstboxAlt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstboxAlt.SelectedIndexChanged
        Dim ds As DataSet
        Dim dr As DataRow

        Dim claveAlt As String = Me.lstboxAlt.SelectedItem.ToString()

        ds = Me._cMDB.RellenarDataSet("Alt", "SELECT nombre, id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" & claveAlt & "'")
        dr = ds.Tables(0).Rows(0)

        Me._idalt = dr("id_alteracion")
        Me._nombreAlteracion = dr("nombre")

        Me.btnBorrar.Enabled = True

    End Sub

    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click

        If (Me._tipo = "Punto") Then
            Me._cMDB.EjecutarSQL("DELETE * FROM [Punto] WHERE id_punto = " & Me._idpunto.ToString())
            Me._cMDB.EjecutarSQL("DELETE * FROM [Alteracion] WHERE id_punto = " & Me._idpunto.ToString())
            Me._Rellenar.RellenarPuntos(Me.lstboxPuntos)
            Me.lstboxAlt.Items.Clear()
        Else
            ' Eliminar Series
            Me._cMDB.EjecutarSQL("DELETE * FROM [Lista] WHERE id_punto = " & Me._idpunto.ToString() & " AND id_alteracion=" & Me._idalt.ToString())
            ' Eliminar Alteracion
            Me._cMDB.EjecutarSQL("DELETE * FROM [Alteracion] WHERE id_punto = " & Me._idpunto.ToString() & " AND id_alteracion=" & Me._idalt.ToString())
            Me._Rellenar.RellenarAlteraciones(Me.lstboxAlt, Me._idpunto)
        End If

        Me.btnBorrar.Enabled = False
    End Sub
End Class