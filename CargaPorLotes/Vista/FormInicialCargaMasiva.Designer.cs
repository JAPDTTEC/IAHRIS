namespace CargaPorLotes.Vista
{
    partial class FormInicialCargaMasiva
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInicialCargaMasiva));
            this.lblTituloDirCSV = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.cbSubDirectorios = new System.Windows.Forms.CheckBox();
            this.btnGenerarBat = new System.Windows.Forms.Button();
            this.gridLogs = new System.Windows.Forms.DataGridView();
            this.NomFichero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Punto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alteracion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCargarDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lblUbicacionCSV = new System.Windows.Forms.Label();
            this.lblFicherosProc = new System.Windows.Forms.Label();
            this.lblResFicherosProc = new System.Windows.Forms.Label();
            this.lblResPuntosProc = new System.Windows.Forms.Label();
            this.lblPuntosProc = new System.Windows.Forms.Label();
            this.lblResAltProc = new System.Windows.Forms.Label();
            this.lblAltProc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblResErroneos = new System.Windows.Forms.Label();
            this.lblErroneos = new System.Windows.Forms.Label();
            this.btnBorrarTodo = new System.Windows.Forms.Button();
            this.btnGuardarReporte = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTituloDirCSV
            // 
            this.lblTituloDirCSV.AutoSize = true;
            this.lblTituloDirCSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloDirCSV.Location = new System.Drawing.Point(58, 31);
            this.lblTituloDirCSV.Name = "lblTituloDirCSV";
            this.lblTituloDirCSV.Size = new System.Drawing.Size(170, 15);
            this.lblTituloDirCSV.TabIndex = 0;
            this.lblTituloDirCSV.Text = "Dirección del fichero CSV";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(201, 75);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(437, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnExaminar
            // 
            this.btnExaminar.Location = new System.Drawing.Point(652, 72);
            this.btnExaminar.Margin = new System.Windows.Forms.Padding(4);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(106, 23);
            this.btnExaminar.TabIndex = 2;
            this.btnExaminar.Text = "Examinar...";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // cbSubDirectorios
            // 
            this.cbSubDirectorios.AutoSize = true;
            this.cbSubDirectorios.Location = new System.Drawing.Point(217, 107);
            this.cbSubDirectorios.Margin = new System.Windows.Forms.Padding(4);
            this.cbSubDirectorios.Name = "cbSubDirectorios";
            this.cbSubDirectorios.Size = new System.Drawing.Size(124, 17);
            this.cbSubDirectorios.TabIndex = 3;
            this.cbSubDirectorios.Text = "Incluir Subdirectorios";
            this.cbSubDirectorios.UseVisualStyleBackColor = true;
            // 
            // btnGenerarBat
            // 
            this.btnGenerarBat.Enabled = false;
            this.btnGenerarBat.Location = new System.Drawing.Point(652, 444);
            this.btnGenerarBat.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerarBat.Name = "btnGenerarBat";
            this.btnGenerarBat.Size = new System.Drawing.Size(106, 38);
            this.btnGenerarBat.TabIndex = 5;
            this.btnGenerarBat.Text = "Generar Fichero";
            this.btnGenerarBat.UseVisualStyleBackColor = true;
            this.btnGenerarBat.Click += new System.EventHandler(this.btnGenerarBat_Click);
            // 
            // gridLogs
            // 
            this.gridLogs.AllowUserToAddRows = false;
            this.gridLogs.AllowUserToDeleteRows = false;
            this.gridLogs.AllowUserToResizeColumns = false;
            this.gridLogs.AllowUserToResizeRows = false;
            this.gridLogs.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridLogs.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.gridLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NomFichero,
            this.Punto,
            this.Alteracion,
            this.Estado,
            this.Error});
            this.gridLogs.Location = new System.Drawing.Point(81, 225);
            this.gridLogs.Margin = new System.Windows.Forms.Padding(4);
            this.gridLogs.MultiSelect = false;
            this.gridLogs.Name = "gridLogs";
            this.gridLogs.ReadOnly = true;
            this.gridLogs.RowHeadersVisible = false;
            this.gridLogs.RowHeadersWidth = 100;
            this.gridLogs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLogs.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.gridLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLogs.Size = new System.Drawing.Size(691, 150);
            this.gridLogs.TabIndex = 7;
            // 
            // NomFichero
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NomFichero.DefaultCellStyle = dataGridViewCellStyle2;
            this.NomFichero.HeaderText = "Nombre Fichero";
            this.NomFichero.Name = "NomFichero";
            this.NomFichero.ReadOnly = true;
            this.NomFichero.Width = 200;
            // 
            // Punto
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Punto.DefaultCellStyle = dataGridViewCellStyle3;
            this.Punto.HeaderText = "Punto";
            this.Punto.Name = "Punto";
            this.Punto.ReadOnly = true;
            // 
            // Alteracion
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Alteracion.DefaultCellStyle = dataGridViewCellStyle4;
            this.Alteracion.HeaderText = "Alteración";
            this.Alteracion.Name = "Alteracion";
            this.Alteracion.ReadOnly = true;
            // 
            // Estado
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Estado.DefaultCellStyle = dataGridViewCellStyle5;
            this.Estado.HeaderText = "Estado";
            this.Estado.Name = "Estado";
            this.Estado.ReadOnly = true;
            // 
            // Error
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Error.DefaultCellStyle = dataGridViewCellStyle6;
            this.Error.HeaderText = "Error";
            this.Error.Name = "Error";
            this.Error.ReadOnly = true;
            this.Error.Width = 190;
            // 
            // btnCargarDir
            // 
            this.btnCargarDir.Enabled = false;
            this.btnCargarDir.Location = new System.Drawing.Point(652, 107);
            this.btnCargarDir.Margin = new System.Windows.Forms.Padding(4);
            this.btnCargarDir.Name = "btnCargarDir";
            this.btnCargarDir.Size = new System.Drawing.Size(106, 50);
            this.btnCargarDir.TabIndex = 4;
            this.btnCargarDir.Text = "Procesar Directorio";
            this.btnCargarDir.UseVisualStyleBackColor = true;
            this.btnCargarDir.Click += new System.EventHandler(this.btnCargarDir_Click);
            // 
            // lblUbicacionCSV
            // 
            this.lblUbicacionCSV.AutoSize = true;
            this.lblUbicacionCSV.Location = new System.Drawing.Point(84, 64);
            this.lblUbicacionCSV.Name = "lblUbicacionCSV";
            this.lblUbicacionCSV.Size = new System.Drawing.Size(61, 13);
            this.lblUbicacionCSV.TabIndex = 11;
            this.lblUbicacionCSV.Text = "Ubicación: ";
            // 
            // lblFicherosProc
            // 
            this.lblFicherosProc.AutoSize = true;
            this.lblFicherosProc.Enabled = false;
            this.lblFicherosProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFicherosProc.Location = new System.Drawing.Point(55, 360);
            this.lblFicherosProc.Name = "lblFicherosProc";
            this.lblFicherosProc.Size = new System.Drawing.Size(128, 15);
            this.lblFicherosProc.TabIndex = 12;
            this.lblFicherosProc.Text = "Ficheros Procesados: ";
            // 
            // lblResFicherosProc
            // 
            this.lblResFicherosProc.AutoSize = true;
            this.lblResFicherosProc.Enabled = false;
            this.lblResFicherosProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResFicherosProc.Location = new System.Drawing.Point(176, 360);
            this.lblResFicherosProc.Name = "lblResFicherosProc";
            this.lblResFicherosProc.Size = new System.Drawing.Size(14, 15);
            this.lblResFicherosProc.TabIndex = 13;
            this.lblResFicherosProc.Text = "0";
            // 
            // lblResPuntosProc
            // 
            this.lblResPuntosProc.AutoSize = true;
            this.lblResPuntosProc.Enabled = false;
            this.lblResPuntosProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResPuntosProc.Location = new System.Drawing.Point(313, 360);
            this.lblResPuntosProc.Name = "lblResPuntosProc";
            this.lblResPuntosProc.Size = new System.Drawing.Size(14, 15);
            this.lblResPuntosProc.TabIndex = 15;
            this.lblResPuntosProc.Text = "0";
            // 
            // lblPuntosProc
            // 
            this.lblPuntosProc.AutoSize = true;
            this.lblPuntosProc.Enabled = false;
            this.lblPuntosProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPuntosProc.Location = new System.Drawing.Point(201, 360);
            this.lblPuntosProc.Name = "lblPuntosProc";
            this.lblPuntosProc.Size = new System.Drawing.Size(119, 15);
            this.lblPuntosProc.TabIndex = 14;
            this.lblPuntosProc.Text = "Puntos Procesados: ";
            // 
            // lblResAltProc
            // 
            this.lblResAltProc.AutoSize = true;
            this.lblResAltProc.Enabled = false;
            this.lblResAltProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResAltProc.Location = new System.Drawing.Point(478, 360);
            this.lblResAltProc.Name = "lblResAltProc";
            this.lblResAltProc.Size = new System.Drawing.Size(14, 15);
            this.lblResAltProc.TabIndex = 17;
            this.lblResAltProc.Text = "0";
            // 
            // lblAltProc
            // 
            this.lblAltProc.AutoSize = true;
            this.lblAltProc.Enabled = false;
            this.lblAltProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAltProc.Location = new System.Drawing.Point(337, 360);
            this.lblAltProc.Name = "lblAltProc";
            this.lblAltProc.Size = new System.Drawing.Size(148, 15);
            this.lblAltProc.TabIndex = 16;
            this.lblAltProc.Text = "Alteraciones Procesadas: ";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(38, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(720, 2);
            this.label2.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(38, 351);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(720, 2);
            this.label1.TabIndex = 36;
            // 
            // lblResErroneos
            // 
            this.lblResErroneos.AutoSize = true;
            this.lblResErroneos.Enabled = false;
            this.lblResErroneos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResErroneos.Location = new System.Drawing.Point(559, 360);
            this.lblResErroneos.Name = "lblResErroneos";
            this.lblResErroneos.Size = new System.Drawing.Size(14, 15);
            this.lblResErroneos.TabIndex = 38;
            this.lblResErroneos.Text = "0";
            // 
            // lblErroneos
            // 
            this.lblErroneos.AutoSize = true;
            this.lblErroneos.Enabled = false;
            this.lblErroneos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErroneos.Location = new System.Drawing.Point(501, 360);
            this.lblErroneos.Name = "lblErroneos";
            this.lblErroneos.Size = new System.Drawing.Size(60, 15);
            this.lblErroneos.TabIndex = 37;
            this.lblErroneos.Text = "Erróneos:";
            // 
            // btnBorrarTodo
            // 
            this.btnBorrarTodo.Location = new System.Drawing.Point(494, 444);
            this.btnBorrarTodo.Margin = new System.Windows.Forms.Padding(4);
            this.btnBorrarTodo.Name = "btnBorrarTodo";
            this.btnBorrarTodo.Size = new System.Drawing.Size(106, 38);
            this.btnBorrarTodo.TabIndex = 39;
            this.btnBorrarTodo.Text = "Borrar Todo";
            this.btnBorrarTodo.UseVisualStyleBackColor = true;
            this.btnBorrarTodo.Click += new System.EventHandler(this.btnBorrarTodo_Click);
            // 
            // btnGuardarReporte
            // 
            this.btnGuardarReporte.Enabled = false;
            this.btnGuardarReporte.Location = new System.Drawing.Point(800, 438);
            this.btnGuardarReporte.Margin = new System.Windows.Forms.Padding(4);
            this.btnGuardarReporte.Name = "btnGuardarReporte";
            this.btnGuardarReporte.Size = new System.Drawing.Size(95, 26);
            this.btnGuardarReporte.TabIndex = 40;
            this.btnGuardarReporte.Text = "Guardar Reporte";
            this.btnGuardarReporte.UseVisualStyleBackColor = true;
            this.btnGuardarReporte.Click += new System.EventHandler(this.btnGuardarReporte_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(38, 385);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(720, 2);
            this.label3.TabIndex = 41;
            // 
            // FormInicialCargaMasiva
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(798, 495);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGuardarReporte);
            this.Controls.Add(this.btnBorrarTodo);
            this.Controls.Add(this.lblResErroneos);
            this.Controls.Add(this.lblErroneos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblResAltProc);
            this.Controls.Add(this.lblAltProc);
            this.Controls.Add(this.lblResPuntosProc);
            this.Controls.Add(this.lblPuntosProc);
            this.Controls.Add(this.lblResFicherosProc);
            this.Controls.Add(this.lblFicherosProc);
            this.Controls.Add(this.lblUbicacionCSV);
            this.Controls.Add(this.btnCargarDir);
            this.Controls.Add(this.gridLogs);
            this.Controls.Add(this.btnGenerarBat);
            this.Controls.Add(this.cbSubDirectorios);
            this.Controls.Add(this.btnExaminar);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblTituloDirCSV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormInicialCargaMasiva";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carga Masiva";
            this.Load += new System.EventHandler(this.FormInicialCargaMasiva_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTituloDirCSV;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.CheckBox cbSubDirectorios;
        private System.Windows.Forms.Button btnGenerarBat;
        private System.Windows.Forms.DataGridView gridLogs;
        private System.Windows.Forms.Button btnCargarDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblUbicacionCSV;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomFichero;
        private System.Windows.Forms.DataGridViewTextBoxColumn Punto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alteracion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estado;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.Label lblFicherosProc;
        private System.Windows.Forms.Label lblResFicherosProc;
        private System.Windows.Forms.Label lblResPuntosProc;
        private System.Windows.Forms.Label lblPuntosProc;
        private System.Windows.Forms.Label lblResAltProc;
        private System.Windows.Forms.Label lblAltProc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblResErroneos;
        private System.Windows.Forms.Label lblErroneos;
        private System.Windows.Forms.Button btnBorrarTodo;
        private System.Windows.Forms.Button btnGuardarReporte;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
    }
}