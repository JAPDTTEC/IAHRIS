using System;
using System.Data;
using System.Windows.Forms;
using IAHRIS.BBDD;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    public partial class FormEliminarPunto
    {
        public FormEliminarPunto()
        {
            InitializeComponent();
            _lstboxPuntos.Name = "lstboxPuntos";
            _lstboxAlt.Name = "lstboxAlt";
            _btnBorrar.Name = "btnBorrar";
        }

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

        private BBDD.OleDbDataBase _cMDB;
        private Rellenar.RellenarForm _Rellenar;
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
            // Dim id_punto As Integer
            // Dim nombre As String

            //string clavePunto = lstboxPuntos.SelectedItem.ToString();
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
            //string claveAlt = lstboxAlt.SelectedItem.ToString();
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
                _cMDB.EjecutarSQL("DELETE * FROM [Punto] WHERE id_punto = " + _idpunto.ToString());
                _cMDB.EjecutarSQL("DELETE * FROM [Alteracion] WHERE id_punto = " + _idpunto.ToString());
                //var arglistbox = lstboxPuntos;
                //_Rellenar.RellenarPuntos(ref arglistbox);
                //lstboxPuntos = arglistbox;
                //lstboxAlt.Items.Clear();
                cmbProyectos_SelectedIndexChanged(null, null);
            }
            else
            {
                // Eliminar Series
                _cMDB.EjecutarSQL("DELETE * FROM [Lista] WHERE id_punto = " + _idpunto.ToString() + " AND id_alteracion=" + _idalt.ToString());
                // Eliminar Alteracion
                _cMDB.EjecutarSQL("DELETE * FROM [Alteracion] WHERE id_punto = " + _idpunto.ToString() + " AND id_alteracion=" + _idalt.ToString());
                //var arglistbox1 = lstboxAlt;
                //_Rellenar.RellenarAlteraciones(ref arglistbox1, _idpunto);
                //lstboxAlt = arglistbox1;
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