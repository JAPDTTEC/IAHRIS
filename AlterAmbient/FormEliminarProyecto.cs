using System;
using System.Windows.Forms;
using IAHRIS.BBDD;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    public partial class FormEliminarProyecto
    {
        public FormEliminarProyecto()
        {
            InitializeComponent();
            _cbProyectos.Name = "cbProyectos";
            _btnAceptar.Name = "btnAceptar";
        }

        public FormEliminarProyecto(OleDbDataBase MDB)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            _cMDB = MDB;
            _rellenar = new Rellenar.RellenarForm(_cMDB);
            var argcombo = cbProyectos;
            _rellenar.RellenarProyectos(ref argcombo);
            cbProyectos = argcombo;

            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _cbProyectos.Name = "cbProyectos";
            _btnAceptar.Name = "btnAceptar";
        }

        private BBDD.OleDbDataBase _cMDB;
        private Rellenar.RellenarForm _rellenar;
        private MultiLangXML.MultiIdiomasXML _traductor;

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cbProyectos.SelectedIndex == -1)
            {
                // Me.lblPuntos.Text = 0
                return;
            }

            Enabled = false;
            var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");
            var dr = ds.Tables[0].Rows[cbProyectos.SelectedIndex];
            // ds = Me._cMDB.RellenarDataSet("Puntos", "DELETE FROM [Proyecto] WHERE ID_proyecto=" & dr(0))
            int filas = _cMDB.EjecutarSQL(Conversions.ToString(Operators.ConcatenateObject("DELETE FROM [Proyecto] WHERE ID_proyecto=", dr[0])));
            if (filas == -1)
            {
                Enabled = true;
                return;
            }

            lblPuntos.Text = "";
            var argcombo = cbProyectos;
            _rellenar.RellenarProyectos(ref argcombo);
            cbProyectos = argcombo;
            Enabled = true;
            MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDeleteProject"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOperationEnd"), MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void cbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProyectos.SelectedIndex == -1)
            {
                lblPuntos.Text = 0.ToString();
                return;
            }

            var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");
            var dr = ds.Tables[0].Rows[cbProyectos.SelectedIndex];
            ds = _cMDB.RellenarDataSet("Puntos", Conversions.ToString(Operators.ConcatenateObject("SELECT COUNT(*) FROM [Punto] WHERE ID_proyecto=", dr[0])));
            dr = ds.Tables[0].Rows[0];
            lblPuntos.Text = Conversions.ToString(dr[0]);
        }
    }
}