<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAnadirPunto
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
        Me.btnAceptar = New System.Windows.Forms.Button
        Me.txtClave = New System.Windows.Forms.TextBox
        Me.txtNombre = New System.Windows.Forms.TextBox
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.gbAnadirPunto = New System.Windows.Forms.GroupBox
        Me.cmbPuntos = New System.Windows.Forms.ComboBox
        Me.lblProyecto = New System.Windows.Forms.Label
        Me.lblDescrip = New System.Windows.Forms.Label
        Me.lblCodigo = New System.Windows.Forms.Label
        Me.gbAnadirPunto.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(12, 135)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(116, 42)
        Me.btnAceptar.TabIndex = 0
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'txtClave
        '
        Me.txtClave.Location = New System.Drawing.Point(132, 22)
        Me.txtClave.Name = "txtClave"
        Me.txtClave.Size = New System.Drawing.Size(164, 20)
        Me.txtClave.TabIndex = 6
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(132, 45)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(164, 20)
        Me.txtNombre.TabIndex = 7
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(205, 135)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(113, 42)
        Me.btnCancelar.TabIndex = 8
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'gbAnadirPunto
        '
        Me.gbAnadirPunto.Controls.Add(Me.cmbPuntos)
        Me.gbAnadirPunto.Controls.Add(Me.lblProyecto)
        Me.gbAnadirPunto.Controls.Add(Me.lblDescrip)
        Me.gbAnadirPunto.Controls.Add(Me.lblCodigo)
        Me.gbAnadirPunto.Controls.Add(Me.txtClave)
        Me.gbAnadirPunto.Controls.Add(Me.txtNombre)
        Me.gbAnadirPunto.Location = New System.Drawing.Point(12, 12)
        Me.gbAnadirPunto.Name = "gbAnadirPunto"
        Me.gbAnadirPunto.Size = New System.Drawing.Size(306, 117)
        Me.gbAnadirPunto.TabIndex = 10
        Me.gbAnadirPunto.TabStop = False
        Me.gbAnadirPunto.Text = "Datos del "
        '
        'cmbPuntos
        '
        Me.cmbPuntos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPuntos.FormattingEnabled = True
        Me.cmbPuntos.Location = New System.Drawing.Point(132, 80)
        Me.cmbPuntos.Name = "cmbPuntos"
        Me.cmbPuntos.Size = New System.Drawing.Size(164, 21)
        Me.cmbPuntos.TabIndex = 11
        '
        'lblProyecto
        '
        Me.lblProyecto.AutoSize = True
        Me.lblProyecto.Location = New System.Drawing.Point(6, 83)
        Me.lblProyecto.Name = "lblProyecto"
        Me.lblProyecto.Size = New System.Drawing.Size(81, 13)
        Me.lblProyecto.TabIndex = 10
        Me.lblProyecto.Text = "Punto asociado"
        '
        'lblDescrip
        '
        Me.lblDescrip.AutoSize = True
        Me.lblDescrip.Location = New System.Drawing.Point(6, 48)
        Me.lblDescrip.Name = "lblDescrip"
        Me.lblDescrip.Size = New System.Drawing.Size(69, 13)
        Me.lblDescrip.TabIndex = 9
        Me.lblDescrip.Text = "Descripción  "
        '
        'lblCodigo
        '
        Me.lblCodigo.AutoSize = True
        Me.lblCodigo.Location = New System.Drawing.Point(6, 25)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(46, 13)
        Me.lblCodigo.TabIndex = 8
        Me.lblCodigo.Text = "Código  "
        '
        'FormAnadirPunto
        '
        Me.AcceptButton = Me.btnAceptar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(322, 185)
        Me.ControlBox = False
        Me.Controls.Add(Me.gbAnadirPunto)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Name = "FormAnadirPunto"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FormAnadirPunto"
        Me.gbAnadirPunto.ResumeLayout(False)
        Me.gbAnadirPunto.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents txtClave As System.Windows.Forms.TextBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents gbAnadirPunto As System.Windows.Forms.GroupBox
    Friend WithEvents lblDescrip As System.Windows.Forms.Label
    Friend WithEvents lblCodigo As System.Windows.Forms.Label
    Friend WithEvents cmbPuntos As System.Windows.Forms.ComboBox
    Friend WithEvents lblProyecto As System.Windows.Forms.Label
End Class
