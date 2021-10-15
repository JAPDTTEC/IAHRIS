Public Class FormBienvenida


#Region "Boton programa"
    Private Sub pbPrograma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbPrograma.Click
        Dim fInicio As New FormInicial()
        fInicio.Show()
        Me.Hide()
    End Sub

    Private Sub pbPrograma_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbPrograma.MouseHover
        Me.pbPrograma.BorderStyle = BorderStyle.Fixed3D
    End Sub

    Private Sub pbPrograma_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbPrograma.MouseLeave
        Me.pbPrograma.BorderStyle = BorderStyle.None
    End Sub
#End Region

#Region "Boton Manual"
    Private Sub pbManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            System.Diagnostics.Process.Start(".\Manual\Manual Usuario.pdf")
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub pbManual_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Me.pbManual.BorderStyle = BorderStyle.Fixed3D
    'End Sub

    'Private Sub pbManual_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Me.pbManual.BorderStyle = BorderStyle.None
    'End Sub
#End Region

#Region "Boton Referencia"
    Private Sub pbReferencia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            System.Diagnostics.Process.Start(".\Manual\Manual Referencia.pdf")
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub pbReferencia_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Me.pbReferencia.BorderStyle = BorderStyle.Fixed3D
    'End Sub

    'Private Sub pbReferencia_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Me.pbReferencia.BorderStyle = BorderStyle.None
    'End Sub
#End Region


    Private Sub pbAcercaDe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim fAcerca As New FormAcercaDe()

        fAcerca.ShowDialog()
    End Sub
End Class