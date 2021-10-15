<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormEliminarPunto
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
        Me.lstboxPuntos = New System.Windows.Forms.ListBox
        Me.lstboxAlt = New System.Windows.Forms.ListBox
        Me.btnBorrar = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblPunto = New System.Windows.Forms.Label
        Me.lblAlteracion = New System.Windows.Forms.Label
        Me.lblAdvertencia = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lstboxPuntos
        '
        Me.lstboxPuntos.FormattingEnabled = True
        Me.lstboxPuntos.Location = New System.Drawing.Point(12, 37)
        Me.lstboxPuntos.Name = "lstboxPuntos"
        Me.lstboxPuntos.Size = New System.Drawing.Size(95, 199)
        Me.lstboxPuntos.TabIndex = 0
        '
        'lstboxAlt
        '
        Me.lstboxAlt.FormattingEnabled = True
        Me.lstboxAlt.Location = New System.Drawing.Point(153, 37)
        Me.lstboxAlt.Name = "lstboxAlt"
        Me.lstboxAlt.Size = New System.Drawing.Size(96, 199)
        Me.lstboxAlt.TabIndex = 1
        '
        'btnBorrar
        '
        Me.btnBorrar.Location = New System.Drawing.Point(275, 154)
        Me.btnBorrar.Name = "btnBorrar"
        Me.btnBorrar.Size = New System.Drawing.Size(202, 83)
        Me.btnBorrar.TabIndex = 2
        Me.btnBorrar.Text = "Borrar"
        Me.btnBorrar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(113, 119)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "->"
        '
        'lblPunto
        '
        Me.lblPunto.AutoSize = True
        Me.lblPunto.Location = New System.Drawing.Point(9, 15)
        Me.lblPunto.Name = "lblPunto"
        Me.lblPunto.Size = New System.Drawing.Size(95, 13)
        Me.lblPunto.TabIndex = 4
        Me.lblPunto.Text = "_____Punto_____"
        '
        'lblAlteracion
        '
        Me.lblAlteracion.AutoSize = True
        Me.lblAlteracion.Location = New System.Drawing.Point(150, 15)
        Me.lblAlteracion.Name = "lblAlteracion"
        Me.lblAlteracion.Size = New System.Drawing.Size(102, 13)
        Me.lblAlteracion.TabIndex = 5
        Me.lblAlteracion.Text = "____Alteración____"
        '
        'lblAdvertencia
        '
        Me.lblAdvertencia.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblAdvertencia.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAdvertencia.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblAdvertencia.Location = New System.Drawing.Point(275, 37)
        Me.lblAdvertencia.Name = "lblAdvertencia"
        Me.lblAdvertencia.Size = New System.Drawing.Size(202, 114)
        Me.lblAdvertencia.TabIndex = 6
        Me.lblAdvertencia.Text = "Label4"
        Me.lblAdvertencia.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FormEliminarPunto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 249)
        Me.Controls.Add(Me.lblAdvertencia)
        Me.Controls.Add(Me.lblAlteracion)
        Me.Controls.Add(Me.lblPunto)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnBorrar)
        Me.Controls.Add(Me.lstboxAlt)
        Me.Controls.Add(Me.lstboxPuntos)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "FormEliminarPunto"
        Me.ShowInTaskbar = False
        Me.Text = "FormEliminarAlteracion"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstboxPuntos As System.Windows.Forms.ListBox
    Friend WithEvents lstboxAlt As System.Windows.Forms.ListBox
    Friend WithEvents btnBorrar As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblPunto As System.Windows.Forms.Label
    Friend WithEvents lblAlteracion As System.Windows.Forms.Label
    Friend WithEvents lblAdvertencia As System.Windows.Forms.Label
End Class
