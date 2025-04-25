Public Class FormEliminarProyecto

    Dim _cMDB As BBDD.OleDbDataBase
    Dim _rellenar As Rellenar.RellenarForm
    Private _traductor As MultiLangXML.MultiIdiomasXML

    Public Sub New(ByVal MDB As BBDD.OleDbDataBase)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._cMDB = MDB

        Me._rellenar = New Rellenar.RellenarForm(Me._cMDB)
        Me._rellenar.RellenarProyectos(Me.cbProyectos)

        ' -------------------------------------
        ' ---- Traducir formulario ------------
        ' -------------------------------------
        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

        If (Me.cbProyectos.SelectedIndex = -1) Then
            'Me.lblPuntos.Text = 0
            Return
        End If

        Me.Enabled = False
        Dim ds As DataSet = Me._cMDB.RellenarDataSet("Proyectos", "SELECT ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC")
        Dim dr As DataRow = ds.Tables(0).Rows(Me.cbProyectos.SelectedIndex)
        'ds = Me._cMDB.RellenarDataSet("Puntos", "DELETE FROM [Proyecto] WHERE ID_proyecto=" & dr(0))
        Dim filas As Integer = Me._cMDB.EjecutarSQL("DELETE FROM [Proyecto] WHERE ID_proyecto=" & dr(0))
        If (filas = -1) Then
            Me.Enabled = True
            Return
        End If

        Me.lblPuntos.Text = ""

        Me._rellenar.RellenarProyectos(Me.cbProyectos)
        Me.Enabled = True
        MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDeleteProject"), _
                        Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOperationEnd"), _
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cbProyectos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbProyectos.SelectedIndexChanged

        If (Me.cbProyectos.SelectedIndex = -1) Then
            Me.lblPuntos.Text = 0
            Return
        End If

        Dim ds As DataSet = Me._cMDB.RellenarDataSet("Proyectos", "SELECT ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC")
        Dim dr As DataRow = ds.Tables(0).Rows(Me.cbProyectos.SelectedIndex)
        ds = Me._cMDB.RellenarDataSet("Puntos", "SELECT COUNT(*) FROM [Punto] WHERE ID_proyecto=" & dr(0))
        dr = ds.Tables(0).Rows(0)
        Me.lblPuntos.Text = dr(0)
    End Sub
End Class