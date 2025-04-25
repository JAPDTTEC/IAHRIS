namespace DatosSimpa
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbCodigoPunto = new System.Windows.Forms.TextBox();
            this.dtPickerFechaIni = new System.Windows.Forms.DateTimePicker();
            this.dtPickerFechaFin = new System.Windows.Forms.DateTimePicker();
            this.lblFechaIni = new System.Windows.Forms.Label();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.lblPuntoX = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnRecuperarDatos = new System.Windows.Forms.Button();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.tbProyecto = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbCodigoPunto
            // 
            this.tbCodigoPunto.Location = new System.Drawing.Point(136, 115);
            this.tbCodigoPunto.MaxLength = 12;
            this.tbCodigoPunto.Name = "tbCodigoPunto";
            this.tbCodigoPunto.Size = new System.Drawing.Size(333, 20);
            this.tbCodigoPunto.TabIndex = 0;
            // 
            // dtPickerFechaIni
            // 
            this.dtPickerFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPickerFechaIni.Location = new System.Drawing.Point(137, 69);
            this.dtPickerFechaIni.MaxDate = new System.DateTime(3000, 9, 30, 0, 0, 0, 0);
            this.dtPickerFechaIni.MinDate = new System.DateTime(1940, 10, 1, 0, 0, 0, 0);
            this.dtPickerFechaIni.Name = "dtPickerFechaIni";
            this.dtPickerFechaIni.Size = new System.Drawing.Size(200, 20);
            this.dtPickerFechaIni.TabIndex = 2;
            this.dtPickerFechaIni.Value = new System.DateTime(1940, 10, 1, 0, 0, 0, 0);
            // 
            // dtPickerFechaFin
            // 
            this.dtPickerFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPickerFechaFin.Location = new System.Drawing.Point(433, 69);
            this.dtPickerFechaFin.MaxDate = new System.DateTime(3000, 9, 30, 0, 0, 0, 0);
            this.dtPickerFechaFin.MinDate = new System.DateTime(1940, 10, 1, 0, 0, 0, 0);
            this.dtPickerFechaFin.Name = "dtPickerFechaFin";
            this.dtPickerFechaFin.Size = new System.Drawing.Size(200, 20);
            this.dtPickerFechaFin.TabIndex = 3;
            this.dtPickerFechaFin.Value = new System.DateTime(2020, 9, 30, 0, 0, 0, 0);
            // 
            // lblFechaIni
            // 
            this.lblFechaIni.AutoSize = true;
            this.lblFechaIni.Location = new System.Drawing.Point(45, 72);
            this.lblFechaIni.Name = "lblFechaIni";
            this.lblFechaIni.Size = new System.Drawing.Size(86, 13);
            this.lblFechaIni.TabIndex = 4;
            this.lblFechaIni.Text = "Fecha de Inicio: ";
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Location = new System.Drawing.Point(352, 72);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(75, 13);
            this.lblFechaFin.TabIndex = 5;
            this.lblFechaFin.Text = "Fecha de Fin: ";
            // 
            // lblPuntoX
            // 
            this.lblPuntoX.AutoSize = true;
            this.lblPuntoX.Location = new System.Drawing.Point(45, 118);
            this.lblPuntoX.Name = "lblPuntoX";
            this.lblPuntoX.Size = new System.Drawing.Size(74, 13);
            this.lblPuntoX.TabIndex = 7;
            this.lblPuntoX.Text = "Código Punto:";
            // 
            // btnCerrar
            // 
            this.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCerrar.Location = new System.Drawing.Point(49, 202);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(138, 41);
            this.btnCerrar.TabIndex = 9;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            // 
            // btnRecuperarDatos
            // 
            this.btnRecuperarDatos.Location = new System.Drawing.Point(433, 202);
            this.btnRecuperarDatos.Name = "btnRecuperarDatos";
            this.btnRecuperarDatos.Size = new System.Drawing.Size(138, 41);
            this.btnRecuperarDatos.TabIndex = 10;
            this.btnRecuperarDatos.Text = "Continuar";
            this.btnRecuperarDatos.UseVisualStyleBackColor = true;
            this.btnRecuperarDatos.Click += new System.EventHandler(this.btnRecuperarDatos_Click);
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Location = new System.Drawing.Point(31, 149);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(92, 13);
            this.lblProyecto.TabIndex = 15;
            this.lblProyecto.Text = "Nombre Proyecto:";
            // 
            // tbProyecto
            // 
            this.tbProyecto.Location = new System.Drawing.Point(136, 146);
            this.tbProyecto.Name = "tbProyecto";
            this.tbProyecto.Size = new System.Drawing.Size(333, 20);
            this.tbProyecto.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 24);
            this.label1.TabIndex = 17;
            this.label1.Text = "Serie a importar";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnRecuperarDatos;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new System.Drawing.Size(653, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbProyecto);
            this.Controls.Add(this.lblProyecto);
            this.Controls.Add(this.btnRecuperarDatos);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.lblPuntoX);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.lblFechaIni);
            this.Controls.Add(this.dtPickerFechaFin);
            this.Controls.Add(this.dtPickerFechaIni);
            this.Controls.Add(this.tbCodigoPunto);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurar archivo.csv de importación";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCodigoPunto;
        private System.Windows.Forms.DateTimePicker dtPickerFechaIni;
        private System.Windows.Forms.DateTimePicker dtPickerFechaFin;
        private System.Windows.Forms.Label lblFechaIni;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.Label lblPuntoX;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnRecuperarDatos;
        private System.Windows.Forms.Label lblProyecto;
        private System.Windows.Forms.TextBox tbProyecto;
        private System.Windows.Forms.Label label1;
    }
}

