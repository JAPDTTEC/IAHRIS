using System;
using System.Data;
using System.Windows.Forms;
using IAHRIS.BBDD;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    /// <summary>
    /// Formulario para eliminar los puntos de BBDD
    /// </summary>
    public partial class FormEliminarPunto
    {
        public FormEliminarPunto()
        {
            InitializeComponent();
            _lstboxPuntos.Name = "lstboxPuntos";
            _lstboxAlt.Name = "lstboxAlt";
            _btnBorrar.Name = "btnBorrar";
        }

        /// <summary>
        /// Eliminar un punto de la base de datos.
        /// </summary>
        /// <param name="MDB">Base de datos</param>
        /// <param name="tipo">Punto o Alteracion</param>
        public FormEliminarPunto(OleDbDataBase MDB, string tipo)
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Centrar el formulario
            Left = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Width - Width));
            Top = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Height - Height));

            _cMDB = MDB;
            _tipo = tipo;
         
            if (_tipo == "Punto")
            {
                lstboxAlt.Enabled = false;
            }
            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            if (tipo == "Punto")
            {
                Name = "FormEliminarPunto";
            }
            else
            {
                Name = "FormEliminarAlteracion";
            }           

            btnBorrar.Enabled = false;
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _lstboxPuntos.Name = "lstboxPuntos";
            _lstboxAlt.Name = "lstboxAlt";
            _btnBorrar.Name = "btnBorrar";

             _Rellenar = new Rellenar.RellenarForm(_cMDB);
            var argcombo = cmbProyectos;
            _Rellenar.RellenarProyectos(ref argcombo);

        }

        private OleDbDataBase _cMDB;
        private RellenarForm _Rellenar;
        private int _idpunto;
        private int _idalt;
        private string _nombrePunto;
        private string _nombreAlteracion;
        private MultiLangXML.MultiIdiomasXML _traductor;
        private string _tipo;

        private void FormEliminarAlteracion_KeyDown(object sender, KeyEventArgs e)
        {
            string strText;
            if (e.KeyCode == Keys.F1)
            {
                if (Text == "Borrar Punto")
                {
                    strText = "Eliminar punto";
                }
                else
                {
                    strText = "Borrar alteración";
                }
                // Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, strText)
            }
        }

        private void FormEliminarAlteracion_Load(object sender, EventArgs e)
        {
            
        }

        private void lstboxPuntos_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds;
            DataRow dr;

            if (lstboxPuntos.SelectedItem is null)
                return;

            ds = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto, nombre FROM Punto WHERE Id_Punto=" + ((ComboItem)lstboxPuntos.SelectedItem).Id);
            dr = ds.Tables[0].Rows[0];
            _idpunto = Conversions.ToInteger(dr["id_punto"]);
            _nombrePunto = Conversions.ToString(dr["nombre"]);
            var arglistbox = lstboxAlt;
            _Rellenar.RellenarAlteraciones(ref arglistbox, _idpunto);
            lstboxAlt = arglistbox;
            if (_tipo == "Punto")
            {
                btnBorrar.Enabled = true;
            }
            else
            {
                btnBorrar.Enabled = false;
            }
        }

        private void lstboxAlt_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds;
            DataRow dr;

            ds = _cMDB.RellenarDataSet("Alt", "SELECT nombre, id_alteracion FROM [Alteracion] WHERE Id_Alteracion=" + ((ComboItem)lstboxAlt.SelectedItem).Id);
            dr = ds.Tables[0].Rows[0];
            _idalt = Conversions.ToInteger(dr["id_alteracion"]);
            _nombreAlteracion = Conversions.ToString(dr["nombre"]);
            btnBorrar.Enabled = true;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (_tipo == "Punto")
            {
                DialogResult result = MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDeletePointAdv"),
                    "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    _cMDB.EjecutarSQL("DELETE * FROM [Punto] WHERE id_punto = " + _idpunto.ToString());
                    _cMDB.EjecutarSQL("DELETE * FROM [Alteracion] WHERE id_punto = " + _idpunto.ToString());
                }
                cmbProyectos_SelectedIndexChanged(null, null);
            }
            else
            {
                DialogResult result = MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDeleteAltAdv"),
                    "Atención", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    // Eliminar Series
                    _cMDB.EjecutarSQL("DELETE * FROM [Lista] WHERE id_punto = " + _idpunto.ToString() + " AND id_alteracion=" + _idalt.ToString());

                    // Eliminar Alteracion
                    _cMDB.EjecutarSQL("DELETE * FROM [Alteracion] WHERE id_punto = " + _idpunto.ToString() + " AND id_alteracion=" + _idalt.ToString());
                }
                cmbProyectos_SelectedIndexChanged(null, null);
            }

            btnBorrar.Enabled = false;
        }

        private void cmbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstboxAlt.Items.Clear();
            var arglistbox = lstboxPuntos;
            _Rellenar.RellenarPuntos(ref arglistbox, ((ComboItem)cmbProyectos.SelectedItem).Id);
            lstboxPuntos = arglistbox;
        }
    }
}