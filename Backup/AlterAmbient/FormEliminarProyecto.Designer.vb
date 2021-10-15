<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormEliminarProyecto
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.gbProyectos = New System.Windows.Forms.GroupBox
        Me.lblPuntos = New System.Windows.Forms.Label
        Me.lblPuntosAso = New System.Windows.Forms.Label
        Me.lblProyecto = New System.Windows.Forms.Label
        Me.cbProyectos = New System.Windows.Forms.ComboBox
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.gbProyectos.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbProyectos
        '
        Me.gbProyectos.Controls.Add(Me.lblPuntos)
        Me.gbProyectos.Controls.Add(Me.lblPuntosAso)
        Me.gbProyectos.Controls.Add(Me.lblProyecto)
        Me.gbProyectos.Controls.Add(Me.cbProyectos)
        Me.gbProyectos.Location = New System.Drawing.Point(12, 12)
        Me.gbProyectos.Name = "gbProyectos"
        Me.gbProyectos.Size = New System.Drawing.Size(289, 70)
        Me.gbProyectos.TabIndex = 0
        Me.gbProyectos.TabStop = False
        Me.gbProyectos.Text = "GroupBox1"
        '
        'lblPuntos
        '
        Me.lblPuntos.AutoSize = True
        Me.lblPuntos.Location = New System.Drawing.Point(102, 48)
        Me.lblPuntos.Name = "lblPuntos"
        Me.lblPuntos.Size = New System.Drawing.Size(13, 13)
        Me.lblPuntos.TabIndex = 3
        Me.lblPuntos.Text = "0"
        '
        'lblPuntosAso
        '
        Me.lblPuntosAso.AutoSize = True
        Me.lblPuntosAso.Location = New System.Drawing.Point(6, 48)
        Me.lblPuntosAso.Name = "lblPuntosAso"
        Me.lblPuntosAso.Size = New System.Drawing.Size(94, 13)
        Me.lblPuntosAso.TabIndex = 2
        Me.lblPuntosAso.Text = "Puntos asociados:"
        '
        'lblProyecto
        '
        Me.lblProyecto.AutoSize = True
        Me.lblProyecto.Location = New System.Drawing.Point(6, 22)
        Me.lblProyecto.Name = "lblProyecto"
        Me.lblProyecto.Size = New System.Drawing.Size(57, 13)
        Me.lblProyecto.TabIndex = 1
        Me.lblProyecto.Text = "Proyectos:"
        '
        'cbProyectos
        '
        Me.cbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbProyectos.FormattingEnabled = True
        Me.cbProyectos.Location = New System.Drawing.Point(105, 19)
        Me.cbProyectos.Name = "cbProyectos"
        Me.cbProyectos.Size = New System.Drawing.Size(178, 21)
        Me.cbProyectos.TabIndex = 0
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(12, 88)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 1
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(226, 88)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 2
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'FormEliminarProyecto
        '
        Me.AcceptButton = Me.btnAceptar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(313, 113)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.gbProyectos)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormEliminarProyecto"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "FormEliminarProyecto"
        Me.gbProyectos.ResumeLayout(False)
        Me.gbProyectos.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbProyectos As System.Windows.Forms.GroupBox
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblPuntos As System.Windows.Forms.Label
    Friend WithEvents lblPuntosAso As System.Windows.Forms.Label
    Friend WithEvents lblProyecto As System.Windows.Forms.Label
    Friend WithEvents cbProyectos As System.Windows.Forms.ComboBox
End Class
