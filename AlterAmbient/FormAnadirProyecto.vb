Public Class FormAnadirProyecto

    Dim _cMDB As BBDD.OleDbDataBase
    Private _traductor As MultiLangXML.MultiIdiomasXML

    Public Sub New(ByVal MDB As BBDD.OleDbDataBase)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._cMDB = MDB

        ' -------------------------------------
        ' ---- Traducir formulario ------------
        ' -------------------------------------
        Me._traductor = New MultiLangXML.MultiIdiomasXML(Me)
        Me._traductor.traducirFormPorConf(Application.StartupPath, "\conf.xml")
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If (Me.txtNombre.Text <> "" And Me.txtDescripcion.Text <> "") Then
            If (Me.txtNombre.Text.Length > 20) Then
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongName"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                'MessageBox.Show("El campo NOMBRE no puede ser mayor de 20 caracteres", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim valores As String() = {Me.txtNombre.Text, Me.txtDescripcion.Text}
            Dim campos As String() = {"nombre", "descripcion"}
            If (Me._cMDB.InsertarRegistro("Proyecto", campos, valores)) Then
                Me.Dispose()
            Else
                MessageBox.Show(Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertProj"), _
                                Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

        End If
    End Sub
End Class