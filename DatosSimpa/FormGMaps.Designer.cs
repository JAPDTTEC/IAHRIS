using System;
using System.Windows.Forms;

namespace DatosSimpa
{
    partial class FormGMaps
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
            this.mapaHidro = new GMap.NET.WindowsForms.GMapControl();
            this.lblZoom = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.labelZoom = new System.Windows.Forms.Label();
            this.tb_BuscadorAforos = new System.Windows.Forms.TextBox();
            this.cb_ListaAforos = new System.Windows.Forms.ComboBox();
            this.cbCuencaHidrografica = new System.Windows.Forms.ComboBox();
            this.btnGetSimpa = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCuencaH = new System.Windows.Forms.Label();
            this.lblEstaciones = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBuscarEst = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCoord_X = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBuscarCoord = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblHuso = new System.Windows.Forms.Label();
            this.tbZona = new System.Windows.Forms.TextBox();
            this.tbCoord_Y = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapaHidro
            // 
            this.mapaHidro.Bearing = 0F;
            this.mapaHidro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapaHidro.CanDragMap = true;
            this.mapaHidro.EmptyTileColor = System.Drawing.Color.Navy;
            this.mapaHidro.GrayScaleMode = false;
            this.mapaHidro.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.mapaHidro.LevelsKeepInMemory = 5;
            this.mapaHidro.Location = new System.Drawing.Point(24, 64);
            this.mapaHidro.MarkersEnabled = true;
            this.mapaHidro.MaxZoom = 20;
            this.mapaHidro.MinZoom = 2;
            this.mapaHidro.MouseWheelZoomEnabled = true;
            this.mapaHidro.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.mapaHidro.Name = "mapaHidro";
            this.mapaHidro.NegativeMode = false;
            this.mapaHidro.PolygonsEnabled = true;
            this.mapaHidro.RetryLoadTile = 0;
            this.mapaHidro.RoutesEnabled = true;
            this.mapaHidro.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.mapaHidro.SelectedAreaFillColor = System.Drawing.Color.White;
            this.mapaHidro.ShowTileGridLines = false;
            this.mapaHidro.Size = new System.Drawing.Size(773, 355);
            this.mapaHidro.TabIndex = 0;
            this.mapaHidro.Zoom = 0D;
            this.mapaHidro.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.mapaHidro_ZoomChanged);
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Location = new System.Drawing.Point(17, 497);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(76, 13);
            this.lblZoom.TabIndex = 1;
            this.lblZoom.Text = "Coordenadas: ";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(214, 425);
            this.trackBar1.Maximum = 30;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(419, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // labelZoom
            // 
            this.labelZoom.AutoSize = true;
            this.labelZoom.Location = new System.Drawing.Point(168, 432);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(40, 13);
            this.labelZoom.TabIndex = 3;
            this.labelZoom.Text = "Zoom: ";
            // 
            // tb_BuscadorAforos
            // 
            this.tb_BuscadorAforos.Location = new System.Drawing.Point(13, 24);
            this.tb_BuscadorAforos.Name = "tb_BuscadorAforos";
            this.tb_BuscadorAforos.Size = new System.Drawing.Size(283, 20);
            this.tb_BuscadorAforos.TabIndex = 4;
            this.tb_BuscadorAforos.Text = "Buscar Estación...";
            this.tb_BuscadorAforos.TextChanged += new System.EventHandler(this.tb_BuscadorAforos_TextChanged);
            this.tb_BuscadorAforos.Enter += new System.EventHandler(this.tb_BuscadorAforos_Enter);
            // 
            // cb_ListaAforos
            // 
            this.cb_ListaAforos.FormattingEnabled = true;
            this.cb_ListaAforos.Location = new System.Drawing.Point(13, 131);
            this.cb_ListaAforos.Name = "cb_ListaAforos";
            this.cb_ListaAforos.Size = new System.Drawing.Size(283, 21);
            this.cb_ListaAforos.TabIndex = 5;
            // 
            // cbCuencaHidrografica
            // 
            this.cbCuencaHidrografica.FormattingEnabled = true;
            this.cbCuencaHidrografica.Location = new System.Drawing.Point(13, 77);
            this.cbCuencaHidrografica.Name = "cbCuencaHidrografica";
            this.cbCuencaHidrografica.Size = new System.Drawing.Size(283, 21);
            this.cbCuencaHidrografica.TabIndex = 6;
            this.cbCuencaHidrografica.SelectedIndexChanged += new System.EventHandler(this.cbCuencaHidrografica_SelectedIndexChanged);
            // 
            // btnGetSimpa
            // 
            this.btnGetSimpa.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetSimpa.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetSimpa.Location = new System.Drawing.Point(857, 425);
            this.btnGetSimpa.Name = "btnGetSimpa";
            this.btnGetSimpa.Size = new System.Drawing.Size(190, 81);
            this.btnGetSimpa.TabIndex = 7;
            this.btnGetSimpa.Text = "Obtener Aportación de SIMPA";
            this.btnGetSimpa.UseVisualStyleBackColor = true;
            this.btnGetSimpa.Click += new System.EventHandler(this.btnGetSimpa_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Recuperar Datos de Simpa";
            // 
            // lblCuencaH
            // 
            this.lblCuencaH.AutoSize = true;
            this.lblCuencaH.Location = new System.Drawing.Point(814, 94);
            this.lblCuencaH.Name = "lblCuencaH";
            this.lblCuencaH.Size = new System.Drawing.Size(107, 13);
            this.lblCuencaH.TabIndex = 9;
            this.lblCuencaH.Text = "Cuenca Hidrógrafica:";
            // 
            // lblEstaciones
            // 
            this.lblEstaciones.AutoSize = true;
            this.lblEstaciones.Location = new System.Drawing.Point(10, 115);
            this.lblEstaciones.Name = "lblEstaciones";
            this.lblEstaciones.Size = new System.Drawing.Size(62, 13);
            this.lblEstaciones.TabIndex = 10;
            this.lblEstaciones.Text = "Estaciones:";
            this.lblEstaciones.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnBuscarEst);
            this.panel1.Controls.Add(this.tb_BuscadorAforos);
            this.panel1.Controls.Add(this.cbCuencaHidrografica);
            this.panel1.Controls.Add(this.lblEstaciones);
            this.panel1.Controls.Add(this.cb_ListaAforos);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(803, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 205);
            this.panel1.TabIndex = 19;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnBuscarEst
            // 
            this.btnBuscarEst.Location = new System.Drawing.Point(195, 165);
            this.btnBuscarEst.Name = "btnBuscarEst";
            this.btnBuscarEst.Size = new System.Drawing.Size(99, 29);
            this.btnBuscarEst.TabIndex = 5;
            this.btnBuscarEst.Text = "Buscar";
            this.btnBuscarEst.UseVisualStyleBackColor = true;
            this.btnBuscarEst.Click += new System.EventHandler(this.btnBuscarEst_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(817, 298);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "X:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(927, 298);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Y:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCoord_X
            // 
            this.tbCoord_X.Location = new System.Drawing.Point(37, 31);
            this.tbCoord_X.Name = "tbCoord_X";
            this.tbCoord_X.Size = new System.Drawing.Size(68, 20);
            this.tbCoord_X.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(814, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Buscar por Coordenada UTM: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBuscarCoord
            // 
            this.btnBuscarCoord.Location = new System.Drawing.Point(150, 91);
            this.btnBuscarCoord.Name = "btnBuscarCoord";
            this.btnBuscarCoord.Size = new System.Drawing.Size(146, 29);
            this.btnBuscarCoord.TabIndex = 11;
            this.btnBuscarCoord.Text = "Buscar Por Coordenada";
            this.btnBuscarCoord.UseVisualStyleBackColor = true;
            this.btnBuscarCoord.Click += new System.EventHandler(this.btnBuscarCoord_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblHuso);
            this.panel2.Controls.Add(this.tbZona);
            this.panel2.Controls.Add(this.tbCoord_Y);
            this.panel2.Controls.Add(this.btnBuscarCoord);
            this.panel2.Controls.Add(this.tbCoord_X);
            this.panel2.Location = new System.Drawing.Point(803, 264);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(311, 139);
            this.panel2.TabIndex = 26;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // lblHuso
            // 
            this.lblHuso.AutoSize = true;
            this.lblHuso.Location = new System.Drawing.Point(228, 34);
            this.lblHuso.Name = "lblHuso";
            this.lblHuso.Size = new System.Drawing.Size(38, 13);
            this.lblHuso.TabIndex = 27;
            this.lblHuso.Text = "Zona: ";
            // 
            // tbZona
            // 
            this.tbZona.Location = new System.Drawing.Point(266, 31);
            this.tbZona.Name = "tbZona";
            this.tbZona.Size = new System.Drawing.Size(30, 20);
            this.tbZona.TabIndex = 26;
            // 
            // tbCoord_Y
            // 
            this.tbCoord_Y.Location = new System.Drawing.Point(147, 31);
            this.tbCoord_Y.Name = "tbCoord_Y";
            this.tbCoord_Y.Size = new System.Drawing.Size(68, 20);
            this.tbCoord_Y.TabIndex = 22;
            // 
            // FormGMaps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 518);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCuencaH);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGetSimpa);
            this.Controls.Add(this.labelZoom);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.lblZoom);
            this.Controls.Add(this.mapaHidro);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FormGMaps";
            this.Text = "Obtener Datos Simpa";
            this.Load += new System.EventHandler(this.FormGMaps_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl mapaHidro;
        private System.Windows.Forms.Label lblZoom;
        private TrackBar trackBar1;
        private Label labelZoom;
        private TextBox tb_BuscadorAforos;
        private ComboBox cb_ListaAforos;
        private ComboBox cbCuencaHidrografica;
        private Button btnGetSimpa;
        private Label label1;
        private Label lblCuencaH;
        private Label lblEstaciones;
        private Panel panel1;
        private Button btnBuscarEst;
        private Label label3;
        private Label label4;
        private TextBox tbCoord_X;
        private Label label2;
        private Button btnBuscarCoord;
        private Panel panel2;
        private Label lblHuso;
        private TextBox tbZona;
        private TextBox tbCoord_Y;
    }
}