namespace CargaPorLotes.Vista
{
    partial class FormGenerarFicheroBAT
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGenerarFicheroBAT));
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnExaminarRutaExp = new System.Windows.Forms.Button();
            this.tbRutaExportar = new System.Windows.Forms.TextBox();
            this.lblRutaExportar = new System.Windows.Forms.Label();
            this.cbCoeValoresHabituales = new System.Windows.Forms.CheckBox();
            this.cbCoeAvenidasSequias = new System.Windows.Forms.CheckBox();
            this.btnEjecutar = new System.Windows.Forms.Button();
            this.lblCoetaneidad = new System.Windows.Forms.Label();
            this.lblRutaFichero = new System.Windows.Forms.Label();
            this.tbNomProyecto = new System.Windows.Forms.TextBox();
            this.lblNomProyecto = new System.Windows.Forms.Label();
            this.cbExportar = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBr_RutaFichero = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cbLogs = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSobreescribir = new System.Windows.Forms.CheckBox();
            this.lblOpcionesSalida = new System.Windows.Forms.Label();
            this.tbDescrProyecto = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(612, 440);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(4);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(149, 65);
            this.btnGuardar.TabIndex = 6;
            this.btnGuardar.Text = "Guardar ";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnExaminarRutaExp
            // 
            this.btnExaminarRutaExp.Location = new System.Drawing.Point(835, 245);
            this.btnExaminarRutaExp.Margin = new System.Windows.Forms.Padding(4);
            this.btnExaminarRutaExp.Name = "btnExaminarRutaExp";
            this.btnExaminarRutaExp.Size = new System.Drawing.Size(141, 28);
            this.btnExaminarRutaExp.TabIndex = 3;
            this.btnExaminarRutaExp.Text = "Examinar...";
            this.btnExaminarRutaExp.UseVisualStyleBackColor = true;
            this.btnExaminarRutaExp.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbRutaExportar
            // 
            this.tbRutaExportar.Location = new System.Drawing.Point(208, 247);
            this.tbRutaExportar.Margin = new System.Windows.Forms.Padding(4);
            this.tbRutaExportar.Name = "tbRutaExportar";
            this.tbRutaExportar.Size = new System.Drawing.Size(612, 22);
            this.tbRutaExportar.TabIndex = 2;
            this.tbRutaExportar.TextChanged += new System.EventHandler(this.tbRutaExportar_TextChanged);
            // 
            // lblRutaExportar
            // 
            this.lblRutaExportar.AutoSize = true;
            this.lblRutaExportar.Location = new System.Drawing.Point(81, 251);
            this.lblRutaExportar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRutaExportar.Name = "lblRutaExportar";
            this.lblRutaExportar.Size = new System.Drawing.Size(90, 16);
            this.lblRutaExportar.TabIndex = 23;
            this.lblRutaExportar.Text = "Ruta exportar:";
            // 
            // cbCoeValoresHabituales
            // 
            this.cbCoeValoresHabituales.AutoSize = true;
            this.cbCoeValoresHabituales.Checked = true;
            this.cbCoeValoresHabituales.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCoeValoresHabituales.Location = new System.Drawing.Point(129, 329);
            this.cbCoeValoresHabituales.Margin = new System.Windows.Forms.Padding(4);
            this.cbCoeValoresHabituales.Name = "cbCoeValoresHabituales";
            this.cbCoeValoresHabituales.Size = new System.Drawing.Size(222, 20);
            this.cbCoeValoresHabituales.TabIndex = 4;
            this.cbCoeValoresHabituales.Text = "Coetaneidad Valores Habituales";
            this.cbCoeValoresHabituales.UseVisualStyleBackColor = true;
            // 
            // cbCoeAvenidasSequias
            // 
            this.cbCoeAvenidasSequias.AutoSize = true;
            this.cbCoeAvenidasSequias.Checked = true;
            this.cbCoeAvenidasSequias.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCoeAvenidasSequias.Location = new System.Drawing.Point(129, 359);
            this.cbCoeAvenidasSequias.Margin = new System.Windows.Forms.Padding(4);
            this.cbCoeAvenidasSequias.Name = "cbCoeAvenidasSequias";
            this.cbCoeAvenidasSequias.Size = new System.Drawing.Size(246, 20);
            this.cbCoeAvenidasSequias.TabIndex = 5;
            this.cbCoeAvenidasSequias.Text = "Coetaneidad de Avenidas y Sequías";
            this.cbCoeAvenidasSequias.UseVisualStyleBackColor = true;
            // 
            // btnEjecutar
            // 
            this.btnEjecutar.Location = new System.Drawing.Point(834, 440);
            this.btnEjecutar.Margin = new System.Windows.Forms.Padding(4);
            this.btnEjecutar.Name = "btnEjecutar";
            this.btnEjecutar.Size = new System.Drawing.Size(149, 65);
            this.btnEjecutar.TabIndex = 7;
            this.btnEjecutar.Text = "Guardar y Ejecutar";
            this.btnEjecutar.UseVisualStyleBackColor = true;
            this.btnEjecutar.Click += new System.EventHandler(this.btnEjecutar_Click);
            // 
            // lblCoetaneidad
            // 
            this.lblCoetaneidad.AutoSize = true;
            this.lblCoetaneidad.Location = new System.Drawing.Point(81, 300);
            this.lblCoetaneidad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCoetaneidad.Name = "lblCoetaneidad";
            this.lblCoetaneidad.Size = new System.Drawing.Size(88, 16);
            this.lblCoetaneidad.TabIndex = 29;
            this.lblCoetaneidad.Text = "Coetaneidad:";
            // 
            // lblRutaFichero
            // 
            this.lblRutaFichero.AutoSize = true;
            this.lblRutaFichero.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRutaFichero.Location = new System.Drawing.Point(65, 29);
            this.lblRutaFichero.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRutaFichero.Name = "lblRutaFichero";
            this.lblRutaFichero.Size = new System.Drawing.Size(0, 17);
            this.lblRutaFichero.TabIndex = 30;
            this.lblRutaFichero.Click += new System.EventHandler(this.lblRutaFichero_Click);
            // 
            // tbNomProyecto
            // 
            this.tbNomProyecto.Location = new System.Drawing.Point(208, 63);
            this.tbNomProyecto.Margin = new System.Windows.Forms.Padding(4);
            this.tbNomProyecto.Name = "tbNomProyecto";
            this.tbNomProyecto.Size = new System.Drawing.Size(409, 22);
            this.tbNomProyecto.TabIndex = 1;
            // 
            // lblNomProyecto
            // 
            this.lblNomProyecto.AutoSize = true;
            this.lblNomProyecto.Location = new System.Drawing.Point(65, 66);
            this.lblNomProyecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNomProyecto.Name = "lblNomProyecto";
            this.lblNomProyecto.Size = new System.Drawing.Size(116, 16);
            this.lblNomProyecto.TabIndex = 31;
            this.lblNomProyecto.Text = "Nombre Proyecto:";
            // 
            // cbExportar
            // 
            this.cbExportar.AutoSize = true;
            this.cbExportar.Checked = true;
            this.cbExportar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExportar.Location = new System.Drawing.Point(68, 199);
            this.cbExportar.Margin = new System.Windows.Forms.Padding(4);
            this.cbExportar.Name = "cbExportar";
            this.cbExportar.Size = new System.Drawing.Size(123, 20);
            this.cbExportar.TabIndex = 33;
            this.cbExportar.Text = "Exportar Informe";
            this.cbExportar.UseVisualStyleBackColor = true;
            this.cbExportar.CheckedChanged += new System.EventHandler(this.cbExportar_CheckedChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(52, 177);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(933, 2);
            this.label2.TabIndex = 34;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.White;
            this.progressBar1.Enabled = false;
            this.progressBar1.ForeColor = System.Drawing.Color.Green;
            this.progressBar1.Location = new System.Drawing.Point(64, 523);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(920, 16);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 35;
            // 
            // cbLogs
            // 
            this.cbLogs.AutoSize = true;
            this.cbLogs.Location = new System.Drawing.Point(208, 153);
            this.cbLogs.Margin = new System.Windows.Forms.Padding(4);
            this.cbLogs.Name = "cbLogs";
            this.cbLogs.Size = new System.Drawing.Size(226, 20);
            this.cbLogs.TabIndex = 36;
            this.cbLogs.Text = "Crear fichero de salida de errores";
            this.cbLogs.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(60, 421);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(933, 2);
            this.label1.TabIndex = 37;
            // 
            // cbSobreescribir
            // 
            this.cbSobreescribir.AutoSize = true;
            this.cbSobreescribir.Checked = true;
            this.cbSobreescribir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSobreescribir.Location = new System.Drawing.Point(645, 329);
            this.cbSobreescribir.Margin = new System.Windows.Forms.Padding(4);
            this.cbSobreescribir.Name = "cbSobreescribir";
            this.cbSobreescribir.Size = new System.Drawing.Size(154, 20);
            this.cbSobreescribir.TabIndex = 39;
            this.cbSobreescribir.Text = "Sobreescribir Informe";
            this.cbSobreescribir.UseVisualStyleBackColor = true;
            // 
            // lblOpcionesSalida
            // 
            this.lblOpcionesSalida.AutoSize = true;
            this.lblOpcionesSalida.Location = new System.Drawing.Point(625, 300);
            this.lblOpcionesSalida.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOpcionesSalida.Name = "lblOpcionesSalida";
            this.lblOpcionesSalida.Size = new System.Drawing.Size(110, 16);
            this.lblOpcionesSalida.TabIndex = 40;
            this.lblOpcionesSalida.Text = "Opciones Salida.";
            // 
            // tbDescrProyecto
            // 
            this.tbDescrProyecto.Location = new System.Drawing.Point(208, 109);
            this.tbDescrProyecto.Margin = new System.Windows.Forms.Padding(4);
            this.tbDescrProyecto.Name = "tbDescrProyecto";
            this.tbDescrProyecto.Size = new System.Drawing.Size(727, 22);
            this.tbDescrProyecto.TabIndex = 41;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(65, 112);
            this.lblDescripcion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(142, 16);
            this.lblDescripcion.TabIndex = 42;
            this.lblDescripcion.Text = "Descripción Proyecto: ";
            // 
            // FormGenerarFicheroBAT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1052, 559);
            this.Controls.Add(this.tbDescrProyecto);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.lblOpcionesSalida);
            this.Controls.Add(this.cbSobreescribir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLogs);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbExportar);
            this.Controls.Add(this.tbNomProyecto);
            this.Controls.Add(this.lblNomProyecto);
            this.Controls.Add(this.lblRutaFichero);
            this.Controls.Add(this.lblCoetaneidad);
            this.Controls.Add(this.btnEjecutar);
            this.Controls.Add(this.cbCoeAvenidasSequias);
            this.Controls.Add(this.cbCoeValoresHabituales);
            this.Controls.Add(this.btnExaminarRutaExp);
            this.Controls.Add(this.tbRutaExportar);
            this.Controls.Add(this.lblRutaExportar);
            this.Controls.Add(this.btnGuardar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormGenerarFicheroBAT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generar Fichero BAT";
            this.Load += new System.EventHandler(this.FormGenerarFicheroBAT_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnExaminarRutaExp;
        private System.Windows.Forms.TextBox tbRutaExportar;
        private System.Windows.Forms.Label lblRutaExportar;
        private System.Windows.Forms.CheckBox cbCoeValoresHabituales;
        private System.Windows.Forms.CheckBox cbCoeAvenidasSequias;
        private System.Windows.Forms.Button btnEjecutar;
        private System.Windows.Forms.Label lblCoetaneidad;
        private System.Windows.Forms.Label lblRutaFichero;
        private System.Windows.Forms.TextBox tbNomProyecto;
        private System.Windows.Forms.Label lblNomProyecto;
        private System.Windows.Forms.CheckBox cbExportar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBr_RutaFichero;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox cbLogs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbSobreescribir;
        private System.Windows.Forms.Label lblOpcionesSalida;
        private System.Windows.Forms.TextBox tbDescrProyecto;
        private System.Windows.Forms.Label lblDescripcion;
    }
}