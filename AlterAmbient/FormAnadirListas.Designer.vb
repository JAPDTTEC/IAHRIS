<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAnadirListas
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnCargarLista = New System.Windows.Forms.Button
        Me.txtRuta = New System.Windows.Forms.TextBox
        Me.btnExaminar = New System.Windows.Forms.Button
        Me.grpboxDetalles = New System.Windows.Forms.GroupBox
        Me.lblInfo = New System.Windows.Forms.Label
        Me.lblSeleccion = New System.Windows.Forms.Label
        Me.grpboxDetalles.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCargarLista
        '
        Me.btnCargarLista.Location = New System.Drawing.Point(236, 32)
        Me.btnCargarLista.Name = "btnCargarLista"
        Me.btnCargarLista.Size = New System.Drawing.Size(101, 87)
        Me.btnCargarLista.TabIndex = 0
        Me.btnCargarLista.Text = "Cargar"
        Me.btnCargarLista.UseVisualStyleBackColor = True
        '
        'txtRuta
        '
        Me.txtRuta.Location = New System.Drawing.Point(12, 25)
        Me.txtRuta.Name = "txtRuta"
        Me.txtRuta.Size = New System.Drawing.Size(242, 20)
        Me.txtRuta.TabIndex = 1
        '
        'btnExaminar
        '
        Me.btnExaminar.Location = New System.Drawing.Point(260, 18)
        Me.btnExaminar.Name = "btnExaminar"
        Me.btnExaminar.Size = New System.Drawing.Size(95, 32)
        Me.btnExaminar.TabIndex = 2
        Me.btnExaminar.Text = "Examinar..."
        Me.btnExaminar.UseVisualStyleBackColor = True
        '
        'grpboxDetalles
        '
        Me.grpboxDetalles.Controls.Add(Me.lblInfo)
        Me.grpboxDetalles.Controls.Add(Me.btnCargarLista)
        Me.grpboxDetalles.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.grpboxDetalles.Location = New System.Drawing.Point(12, 56)
        Me.grpboxDetalles.Name = "grpboxDetalles"
        Me.grpboxDetalles.Size = New System.Drawing.Size(343, 125)
        Me.grpboxDetalles.TabIndex = 3
        Me.grpboxDetalles.TabStop = False
        Me.grpboxDetalles.Text = "Detalles del Fichero"
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(15, 32)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(85, 13)
        Me.lblInfo.TabIndex = 1
        Me.lblInfo.Text = "INFORMACION:"
        '
        'lblSeleccion
        '
        Me.lblSeleccion.AutoSize = True
        Me.lblSeleccion.Location = New System.Drawing.Point(12, 9)
        Me.lblSeleccion.Name = "lblSeleccion"
        Me.lblSeleccion.Size = New System.Drawing.Size(222, 13)
        Me.lblSeleccion.TabIndex = 4
        Me.lblSeleccion.Text = "Seleccione la Serie de datos en formato CSV:"
        '
        'FormAnadirListas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(365, 185)
        Me.Controls.Add(Me.lblSeleccion)
        Me.Controls.Add(Me.grpboxDetalles)
        Me.Controls.Add(Me.btnExaminar)
        Me.Controls.Add(Me.txtRuta)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "FormAnadirListas"
        Me.ShowInTaskbar = False
        Me.Text = "Añadir Series de Datos"
        Me.grpboxDetalles.ResumeLayout(False)
        Me.grpboxDetalles.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCargarLista As System.Windows.Forms.Button
    Friend WithEvents txtRuta As System.Windows.Forms.TextBox
    Friend WithEvents btnExaminar As System.Windows.Forms.Button
    Friend WithEvents grpboxDetalles As System.Windows.Forms.GroupBox
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents lblSeleccion As System.Windows.Forms.Label
End Class
