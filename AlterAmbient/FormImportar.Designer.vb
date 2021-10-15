<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormImportar
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
        Me.lblSeleccion = New System.Windows.Forms.Label
        Me.btnExaminar = New System.Windows.Forms.Button
        Me.txtRuta = New System.Windows.Forms.TextBox
        Me.gbPuntos = New System.Windows.Forms.GroupBox
        Me.lblProyectoOrigen = New System.Windows.Forms.Label
        Me.btnCrearProy = New System.Windows.Forms.Button
        Me.cmbProyPuntos = New System.Windows.Forms.ComboBox
        Me.chklstPuntos = New System.Windows.Forms.CheckedListBox
        Me.btnImportarPuntos = New System.Windows.Forms.Button
        Me.gbProyectos = New System.Windows.Forms.GroupBox
        Me.btnImportarProyecto = New System.Windows.Forms.Button
        Me.cmbProyectos = New System.Windows.Forms.ComboBox
        Me.lblInformacion = New System.Windows.Forms.Label
        Me.lbInfo = New System.Windows.Forms.ListBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.gbPuntos.SuspendLayout()
        Me.gbProyectos.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSeleccion
        '
        Me.lblSeleccion.AutoSize = True
        Me.lblSeleccion.Location = New System.Drawing.Point(12, 13)
        Me.lblSeleccion.Name = "lblSeleccion"
        Me.lblSeleccion.Size = New System.Drawing.Size(196, 13)
        Me.lblSeleccion.TabIndex = 7
        Me.lblSeleccion.Text = "Seleccione la Base de Datos a importar:"
        '
        'btnExaminar
        '
        Me.btnExaminar.Location = New System.Drawing.Point(260, 51)
        Me.btnExaminar.Name = "btnExaminar"
        Me.btnExaminar.Size = New System.Drawing.Size(95, 20)
        Me.btnExaminar.TabIndex = 6
        Me.btnExaminar.Text = "Examinar..."
        Me.btnExaminar.UseVisualStyleBackColor = True
        '
        'txtRuta
        '
        Me.txtRuta.Location = New System.Drawing.Point(15, 29)
        Me.txtRuta.Name = "txtRuta"
        Me.txtRuta.ReadOnly = True
        Me.txtRuta.Size = New System.Drawing.Size(340, 20)
        Me.txtRuta.TabIndex = 5
        '
        'gbPuntos
        '
        Me.gbPuntos.Controls.Add(Me.lblProyectoOrigen)
        Me.gbPuntos.Controls.Add(Me.btnCrearProy)
        Me.gbPuntos.Controls.Add(Me.cmbProyPuntos)
        Me.gbPuntos.Controls.Add(Me.chklstPuntos)
        Me.gbPuntos.Controls.Add(Me.btnImportarPuntos)
        Me.gbPuntos.Location = New System.Drawing.Point(12, 128)
        Me.gbPuntos.Name = "gbPuntos"
        Me.gbPuntos.Size = New System.Drawing.Size(343, 170)
        Me.gbPuntos.TabIndex = 8
        Me.gbPuntos.TabStop = False
        Me.gbPuntos.Text = "Importar Puntos a un Proyecto:"
        '
        'lblProyectoOrigen
        '
        Me.lblProyectoOrigen.AutoSize = True
        Me.lblProyectoOrigen.Location = New System.Drawing.Point(6, 15)
        Me.lblProyectoOrigen.Name = "lblProyectoOrigen"
        Me.lblProyectoOrigen.Size = New System.Drawing.Size(125, 13)
        Me.lblProyectoOrigen.TabIndex = 4
        Me.lblProyectoOrigen.Text = "Proyecto donde importar:"
        '
        'btnCrearProy
        '
        Me.btnCrearProy.Location = New System.Drawing.Point(6, 55)
        Me.btnCrearProy.Name = "btnCrearProy"
        Me.btnCrearProy.Size = New System.Drawing.Size(119, 34)
        Me.btnCrearProy.TabIndex = 3
        Me.btnCrearProy.Text = "Crear Proyecto Nuevo"
        Me.btnCrearProy.UseVisualStyleBackColor = True
        '
        'cmbProyPuntos
        '
        Me.cmbProyPuntos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProyPuntos.FormattingEnabled = True
        Me.cmbProyPuntos.Location = New System.Drawing.Point(6, 31)
        Me.cmbProyPuntos.Name = "cmbProyPuntos"
        Me.cmbProyPuntos.Size = New System.Drawing.Size(119, 21)
        Me.cmbProyPuntos.TabIndex = 2
        '
        'chklstPuntos
        '
        Me.chklstPuntos.FormattingEnabled = True
        Me.chklstPuntos.Location = New System.Drawing.Point(131, 22)
        Me.chklstPuntos.Name = "chklstPuntos"
        Me.chklstPuntos.Size = New System.Drawing.Size(203, 139)
        Me.chklstPuntos.TabIndex = 1
        '
        'btnImportarPuntos
        '
        Me.btnImportarPuntos.Location = New System.Drawing.Point(6, 124)
        Me.btnImportarPuntos.Name = "btnImportarPuntos"
        Me.btnImportarPuntos.Size = New System.Drawing.Size(119, 36)
        Me.btnImportarPuntos.TabIndex = 0
        Me.btnImportarPuntos.Text = "Importar"
        Me.btnImportarPuntos.UseVisualStyleBackColor = True
        '
        'gbProyectos
        '
        Me.gbProyectos.Controls.Add(Me.btnImportarProyecto)
        Me.gbProyectos.Controls.Add(Me.cmbProyectos)
        Me.gbProyectos.Location = New System.Drawing.Point(12, 71)
        Me.gbProyectos.Name = "gbProyectos"
        Me.gbProyectos.Size = New System.Drawing.Size(343, 51)
        Me.gbProyectos.TabIndex = 9
        Me.gbProyectos.TabStop = False
        Me.gbProyectos.Text = "Importar un Proyecto completo"
        '
        'btnImportarProyecto
        '
        Me.btnImportarProyecto.Location = New System.Drawing.Point(212, 19)
        Me.btnImportarProyecto.Name = "btnImportarProyecto"
        Me.btnImportarProyecto.Size = New System.Drawing.Size(125, 21)
        Me.btnImportarProyecto.TabIndex = 1
        Me.btnImportarProyecto.Text = "Importar"
        Me.btnImportarProyecto.UseVisualStyleBackColor = True
        '
        'cmbProyectos
        '
        Me.cmbProyectos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProyectos.FormattingEnabled = True
        Me.cmbProyectos.Location = New System.Drawing.Point(6, 19)
        Me.cmbProyectos.Name = "cmbProyectos"
        Me.cmbProyectos.Size = New System.Drawing.Size(176, 21)
        Me.cmbProyectos.TabIndex = 0
        '
        'lblInformacion
        '
        Me.lblInformacion.AutoSize = True
        Me.lblInformacion.Location = New System.Drawing.Point(12, 314)
        Me.lblInformacion.Name = "lblInformacion"
        Me.lblInformacion.Size = New System.Drawing.Size(68, 13)
        Me.lblInformacion.TabIndex = 10
        Me.lblInformacion.Text = "Información: "
        '
        'lbInfo
        '
        Me.lbInfo.FormattingEnabled = True
        Me.lbInfo.HorizontalScrollbar = True
        Me.lbInfo.Location = New System.Drawing.Point(12, 331)
        Me.lbInfo.Name = "lbInfo"
        Me.lbInfo.Size = New System.Drawing.Size(343, 108)
        Me.lbInfo.TabIndex = 11
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.IAHRIS.My.Resources.Resources.wait30trans
        Me.PictureBox1.Location = New System.Drawing.Point(173, 298)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(30, 30)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'FormImportar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 445)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lbInfo)
        Me.Controls.Add(Me.lblInformacion)
        Me.Controls.Add(Me.gbProyectos)
        Me.Controls.Add(Me.gbPuntos)
        Me.Controls.Add(Me.lblSeleccion)
        Me.Controls.Add(Me.btnExaminar)
        Me.Controls.Add(Me.txtRuta)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormImportar"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FormImportar"
        Me.gbPuntos.ResumeLayout(False)
        Me.gbPuntos.PerformLayout()
        Me.gbProyectos.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSeleccion As System.Windows.Forms.Label
    Friend WithEvents btnExaminar As System.Windows.Forms.Button
    Friend WithEvents txtRuta As System.Windows.Forms.TextBox
    Friend WithEvents gbPuntos As System.Windows.Forms.GroupBox
    Friend WithEvents chklstPuntos As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnImportarPuntos As System.Windows.Forms.Button
    Friend WithEvents gbProyectos As System.Windows.Forms.GroupBox
    Friend WithEvents cmbProyectos As System.Windows.Forms.ComboBox
    Friend WithEvents btnImportarProyecto As System.Windows.Forms.Button
    Friend WithEvents lblInformacion As System.Windows.Forms.Label
    Friend WithEvents lbInfo As System.Windows.Forms.ListBox
    Friend WithEvents lblProyectoOrigen As System.Windows.Forms.Label
    Friend WithEvents btnCrearProy As System.Windows.Forms.Button
    Friend WithEvents cmbProyPuntos As System.Windows.Forms.ComboBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
