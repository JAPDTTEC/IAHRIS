<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormEliminarLista
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
        Me.lstboxListas = New System.Windows.Forms.ListBox
        Me.btnAltDiaria = New System.Windows.Forms.Button
        Me.btnBorrarNatDiaria = New System.Windows.Forms.Button
        Me.btnBorrarNatMensual = New System.Windows.Forms.Button
        Me.cmboxAlt = New System.Windows.Forms.ComboBox
        Me.btnAltMensual = New System.Windows.Forms.Button
        Me.gbNaturales = New System.Windows.Forms.GroupBox
        Me.gbAlteradas = New System.Windows.Forms.GroupBox
        Me.lblSelecAlt = New System.Windows.Forms.Label
        Me.lblSeleccion = New System.Windows.Forms.Label
        Me.gbNaturales.SuspendLayout()
        Me.gbAlteradas.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstboxListas
        '
        Me.lstboxListas.FormattingEnabled = True
        Me.lstboxListas.Location = New System.Drawing.Point(12, 53)
        Me.lstboxListas.Name = "lstboxListas"
        Me.lstboxListas.Size = New System.Drawing.Size(118, 212)
        Me.lstboxListas.TabIndex = 0
        '
        'btnAltDiaria
        '
        Me.btnAltDiaria.Location = New System.Drawing.Point(20, 67)
        Me.btnAltDiaria.Name = "btnAltDiaria"
        Me.btnAltDiaria.Size = New System.Drawing.Size(132, 37)
        Me.btnAltDiaria.TabIndex = 6
        Me.btnAltDiaria.Text = "Borrar Serie Alteración Diaria"
        Me.btnAltDiaria.UseVisualStyleBackColor = True
        '
        'btnBorrarNatDiaria
        '
        Me.btnBorrarNatDiaria.Location = New System.Drawing.Point(20, 27)
        Me.btnBorrarNatDiaria.Name = "btnBorrarNatDiaria"
        Me.btnBorrarNatDiaria.Size = New System.Drawing.Size(132, 38)
        Me.btnBorrarNatDiaria.TabIndex = 9
        Me.btnBorrarNatDiaria.Text = "Borrar Serie Natural Diaria"
        Me.btnBorrarNatDiaria.UseVisualStyleBackColor = True
        '
        'btnBorrarNatMensual
        '
        Me.btnBorrarNatMensual.Location = New System.Drawing.Point(158, 27)
        Me.btnBorrarNatMensual.Name = "btnBorrarNatMensual"
        Me.btnBorrarNatMensual.Size = New System.Drawing.Size(132, 38)
        Me.btnBorrarNatMensual.TabIndex = 10
        Me.btnBorrarNatMensual.Text = "Borrar Serie Natural Mensual"
        Me.btnBorrarNatMensual.UseVisualStyleBackColor = True
        '
        'cmboxAlt
        '
        Me.cmboxAlt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmboxAlt.FormattingEnabled = True
        Me.cmboxAlt.Location = New System.Drawing.Point(158, 28)
        Me.cmboxAlt.Name = "cmboxAlt"
        Me.cmboxAlt.Size = New System.Drawing.Size(132, 21)
        Me.cmboxAlt.TabIndex = 11
        '
        'btnAltMensual
        '
        Me.btnAltMensual.Location = New System.Drawing.Point(158, 67)
        Me.btnAltMensual.Name = "btnAltMensual"
        Me.btnAltMensual.Size = New System.Drawing.Size(132, 37)
        Me.btnAltMensual.TabIndex = 12
        Me.btnAltMensual.Text = "Borrar Serie Alteración Mensual"
        Me.btnAltMensual.UseVisualStyleBackColor = True
        '
        'gbNaturales
        '
        Me.gbNaturales.Controls.Add(Me.btnBorrarNatDiaria)
        Me.gbNaturales.Controls.Add(Me.btnBorrarNatMensual)
        Me.gbNaturales.Location = New System.Drawing.Point(150, 40)
        Me.gbNaturales.Name = "gbNaturales"
        Me.gbNaturales.Size = New System.Drawing.Size(309, 81)
        Me.gbNaturales.TabIndex = 13
        Me.gbNaturales.TabStop = False
        Me.gbNaturales.Text = "Series Naturales"
        '
        'gbAlteradas
        '
        Me.gbAlteradas.Controls.Add(Me.lblSelecAlt)
        Me.gbAlteradas.Controls.Add(Me.cmboxAlt)
        Me.gbAlteradas.Controls.Add(Me.btnAltMensual)
        Me.gbAlteradas.Controls.Add(Me.btnAltDiaria)
        Me.gbAlteradas.Location = New System.Drawing.Point(150, 145)
        Me.gbAlteradas.Name = "gbAlteradas"
        Me.gbAlteradas.Size = New System.Drawing.Size(309, 120)
        Me.gbAlteradas.TabIndex = 14
        Me.gbAlteradas.TabStop = False
        Me.gbAlteradas.Text = "Series Alteradas"
        '
        'lblSelecAlt
        '
        Me.lblSelecAlt.AutoSize = True
        Me.lblSelecAlt.Location = New System.Drawing.Point(26, 31)
        Me.lblSelecAlt.Name = "lblSelecAlt"
        Me.lblSelecAlt.Size = New System.Drawing.Size(109, 13)
        Me.lblSelecAlt.TabIndex = 13
        Me.lblSelecAlt.Text = "Seleccione alteración"
        '
        'lblSeleccion
        '
        Me.lblSeleccion.Location = New System.Drawing.Point(12, 9)
        Me.lblSeleccion.Name = "lblSeleccion"
        Me.lblSeleccion.Size = New System.Drawing.Size(118, 41)
        Me.lblSeleccion.TabIndex = 15
        Me.lblSeleccion.Text = "Seleccione Punto al que eliminar series asociadas:"
        '
        'FormEliminarLista
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(471, 289)
        Me.Controls.Add(Me.lblSeleccion)
        Me.Controls.Add(Me.gbAlteradas)
        Me.Controls.Add(Me.gbNaturales)
        Me.Controls.Add(Me.lstboxListas)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "FormEliminarLista"
        Me.ShowInTaskbar = False
        Me.Text = "Eliminar Serie"
        Me.gbNaturales.ResumeLayout(False)
        Me.gbAlteradas.ResumeLayout(False)
        Me.gbAlteradas.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstboxListas As System.Windows.Forms.ListBox
    Friend WithEvents btnAltDiaria As System.Windows.Forms.Button
    Friend WithEvents btnBorrarNatDiaria As System.Windows.Forms.Button
    Friend WithEvents btnBorrarNatMensual As System.Windows.Forms.Button
    Friend WithEvents cmboxAlt As System.Windows.Forms.ComboBox
    Friend WithEvents btnAltMensual As System.Windows.Forms.Button
    Friend WithEvents gbNaturales As System.Windows.Forms.GroupBox
    Friend WithEvents gbAlteradas As System.Windows.Forms.GroupBox
    Friend WithEvents lblSeleccion As System.Windows.Forms.Label
    Friend WithEvents lblSelecAlt As System.Windows.Forms.Label
End Class
