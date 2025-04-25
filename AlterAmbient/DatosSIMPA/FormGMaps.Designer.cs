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
            this.label5 = new System.Windows.Forms.Label();
            this.btnBuscarEst = new System.Windows.Forms.Button();
            this.lblUTMTitle = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lblHuso = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbZona = new System.Windows.Forms.TextBox();
            this.tbCoord_Y = new System.Windows.Forms.TextBox();
            this.btnBuscarCoord = new System.Windows.Forms.Button();
            this.tbCoord_X = new System.Windows.Forms.TextBox();
            this.lblUTMWarning = new System.Windows.Forms.Label();
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
            this.mapaHidro.Location = new System.Drawing.Point(24, 47);
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
            this.mapaHidro.Size = new System.Drawing.Size(773, 372);
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
            this.trackBar1.TabIndex = 9;
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
            this.tb_BuscadorAforos.TabIndex = 1;
            this.tb_BuscadorAforos.TextChanged += new System.EventHandler(this.tb_BuscadorAforos_TextChanged);
            this.tb_BuscadorAforos.Enter += new System.EventHandler(this.tb_BuscadorAforos_Enter);
            // 
            // cb_ListaAforos
            // 
            this.cb_ListaAforos.FormattingEnabled = true;
            this.cb_ListaAforos.Location = new System.Drawing.Point(13, 131);
            this.cb_ListaAforos.Name = "cb_ListaAforos";
            this.cb_ListaAforos.Size = new System.Drawing.Size(283, 21);
            this.cb_ListaAforos.TabIndex = 3;
            // 
            // cbCuencaHidrografica
            // 
            this.cbCuencaHidrografica.FormattingEnabled = true;
            this.cbCuencaHidrografica.Location = new System.Drawing.Point(13, 77);
            this.cbCuencaHidrografica.Name = "cbCuencaHidrografica";
            this.cbCuencaHidrografica.Size = new System.Drawing.Size(283, 21);
            this.cbCuencaHidrografica.TabIndex = 2;
            this.cbCuencaHidrografica.SelectedIndexChanged += new System.EventHandler(this.cbCuencaHidrografica_SelectedIndexChanged);
            // 
            // btnGetSimpa
            // 
            this.btnGetSimpa.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetSimpa.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetSimpa.Enabled = false;
            this.btnGetSimpa.Location = new System.Drawing.Point(806, 425);
            this.btnGetSimpa.Name = "btnGetSimpa";
            this.btnGetSimpa.Size = new System.Drawing.Size(308, 81);
            this.btnGetSimpa.TabIndex = 10;
            this.btnGetSimpa.Text = "Obtener Aportaciones (hm3/mes) SIMPA";
            this.btnGetSimpa.UseVisualStyleBackColor = true;
            this.btnGetSimpa.Click += new System.EventHandler(this.btnGetSimpa_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Importar Datos SIMPA";
            // 
            // lblCuencaH
            // 
            this.lblCuencaH.AutoSize = true;
            this.lblCuencaH.Location = new System.Drawing.Point(11, 61);
            this.lblCuencaH.Name = "lblCuencaH";
            this.lblCuencaH.Size = new System.Drawing.Size(133, 13);
            this.lblCuencaH.TabIndex = 9;
            this.lblCuencaH.Text = "Demarcación Hidrógrafica:";
            // 
            // lblEstaciones
            // 
            this.lblEstaciones.AutoSize = true;
            this.lblEstaciones.Location = new System.Drawing.Point(10, 115);
            this.lblEstaciones.Name = "lblEstaciones";
            this.lblEstaciones.Size = new System.Drawing.Size(93, 13);
            this.lblEstaciones.TabIndex = 10;
            this.lblEstaciones.Text = "Estación de aforo:";
            this.lblEstaciones.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnBuscarEst);
            this.panel1.Controls.Add(this.tb_BuscadorAforos);
            this.panel1.Controls.Add(this.cbCuencaHidrografica);
            this.panel1.Controls.Add(this.lblCuencaH);
            this.panel1.Controls.Add(this.lblEstaciones);
            this.panel1.Controls.Add(this.cb_ListaAforos);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(803, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 211);
            this.panel1.TabIndex = 19;
            
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Código o nombre:";
            // 
            // btnBuscarEst
            // 
            this.btnBuscarEst.Location = new System.Drawing.Point(195, 165);
            this.btnBuscarEst.Name = "btnBuscarEst";
            this.btnBuscarEst.Size = new System.Drawing.Size(99, 29);
            this.btnBuscarEst.TabIndex = 4;
            this.btnBuscarEst.Text = "Buscar";
            this.btnBuscarEst.UseVisualStyleBackColor = true;
            this.btnBuscarEst.Click += new System.EventHandler(this.btnBuscarEst_Click);
            // 
            // lblUTMTitle
            // 
            this.lblUTMTitle.AutoSize = true;
            this.lblUTMTitle.Location = new System.Drawing.Point(800, 275);
            this.lblUTMTitle.Name = "lblUTMTitle";
            this.lblUTMTitle.Size = new System.Drawing.Size(152, 13);
            this.lblUTMTitle.TabIndex = 21;
            this.lblUTMTitle.Text = "Buscar por Coordenada UTM: ";
            this.lblUTMTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(803, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Buscar por estación de aforo:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblUTMWarning);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.lblHuso);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.tbZona);
            this.panel2.Controls.Add(this.tbCoord_Y);
            this.panel2.Controls.Add(this.btnBuscarCoord);
            this.panel2.Controls.Add(this.tbCoord_X);
            this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Location = new System.Drawing.Point(803, 295);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(311, 124);
            this.panel2.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(287, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 44;
            this.label7.Text = "N";
            // 
            // lblHuso
            // 
            this.lblHuso.AutoSize = true;
            this.lblHuso.Location = new System.Drawing.Point(210, 28);
            this.lblHuso.Name = "lblHuso";
            this.lblHuso.Size = new System.Drawing.Size(35, 13);
            this.lblHuso.TabIndex = 43;
            this.lblHuso.Text = "Huso:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "X:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 42;
            this.label4.Text = "Y:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbZona
            // 
            this.tbZona.Location = new System.Drawing.Point(251, 25);
            this.tbZona.Name = "tbZona";
            this.tbZona.Size = new System.Drawing.Size(30, 20);
            this.tbZona.TabIndex = 39;
            // 
            // tbCoord_Y
            // 
            this.tbCoord_Y.Location = new System.Drawing.Point(136, 25);
            this.tbCoord_Y.Name = "tbCoord_Y";
            this.tbCoord_Y.Size = new System.Drawing.Size(68, 20);
            this.tbCoord_Y.TabIndex = 38;
            // 
            // btnBuscarCoord
            // 
            this.btnBuscarCoord.Location = new System.Drawing.Point(162, 70);
            this.btnBuscarCoord.Name = "btnBuscarCoord";
            this.btnBuscarCoord.Size = new System.Drawing.Size(132, 29);
            this.btnBuscarCoord.TabIndex = 40;
            this.btnBuscarCoord.Text = "Buscar Por Coordenada";
            this.btnBuscarCoord.UseVisualStyleBackColor = true;
            this.btnBuscarCoord.Click += new System.EventHandler(this.btnBuscarCoord_Click);
            // 
            // tbCoord_X
            // 
            this.tbCoord_X.Location = new System.Drawing.Point(33, 25);
            this.tbCoord_X.Name = "tbCoord_X";
            this.tbCoord_X.Size = new System.Drawing.Size(68, 20);
            this.tbCoord_X.TabIndex = 37;
            // 
            // lblUTMWarning
            // 
            this.lblUTMWarning.Location = new System.Drawing.Point(11, 54);
            this.lblUTMWarning.Name = "lblUTMWarning";
            this.lblUTMWarning.Size = new System.Drawing.Size(145, 57);
            this.lblUTMWarning.TabIndex = 28;
            this.lblUTMWarning.Text = "El usuario debe verificar en el mapa la correcta ubicación del punto respecto a l" +
    "a red hidrográfica";
            this.lblUTMWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormGMaps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 518);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblUTMTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGetSimpa);
            this.Controls.Add(this.labelZoom);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.lblZoom);
            this.Controls.Add(this.mapaHidro);
            this.Controls.Add(this.panel1);
            this.Name = "FormGMaps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Importar Datos SIMPA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGMap_FormClosing);
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
        private Label lblUTMTitle;
        private Label label5;
        private Label label6;
        private Panel panel2;
        private Label label7;
        private Label lblHuso;
        private Label label3;
        private Label label4;
        private TextBox tbZona;
        private TextBox tbCoord_Y;
        private Button btnBuscarCoord;
        private TextBox tbCoord_X;
        private Label lblUTMWarning;
    }
}