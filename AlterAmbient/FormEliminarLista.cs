using System;
using System.Data;
using System.Windows.Forms;
using IAHRIS.BBDD;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    public partial class FormEliminarLista
    {
        public FormEliminarLista()
        {
            InitializeComponent();
            _lstboxListas.Name = "lstboxListas";
            _btnAltDiaria.Name = "btnAltDiaria";
            _btnBorrarNatDiaria.Name = "btnBorrarNatDiaria";
            _btnBorrarNatMensual.Name = "btnBorrarNatMensual";
            _cmboxAlt.Name = "cmboxAlt";
            _btnAltMensual.Name = "btnAltMensual";
        }

        public FormEliminarLista(OleDbDataBase MDB)
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            _cMDB = MDB;
            _Rellenar = new Rellenar.RellenarForm(_cMDB);

            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _lstboxListas.Name = "lstboxListas";
            _btnAltDiaria.Name = "btnAltDiaria";
            _btnBorrarNatDiaria.Name = "btnBorrarNatDiaria";
            _btnBorrarNatMensual.Name = "btnBorrarNatMensual";
            _cmboxAlt.Name = "cmboxAlt";
            _btnAltMensual.Name = "btnAltMensual";

            
           
        }

        private Rellenar.RellenarForm _Rellenar;
        private BBDD.OleDbDataBase _cMDB;
        private int _idpunto;
        private int _codAlt;
        private MultiLangXML.MultiIdiomasXML _traductor;

        private void FormEliminarLista_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                // Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, "Eliminar serie")
            }
        }

        private void FormEliminarLista_Load(object sender, EventArgs e)
        {
            var argcombo = cmbProyectos;
            _Rellenar.RellenarProyectos(ref argcombo);
            
        }

        private void lboxListas_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds;
            DataRow draux;
            btnBorrarNatDiaria.Enabled = false;
            btnBorrarNatMensual.Enabled = false;
            btnAltDiaria.Enabled = false;
            btnAltMensual.Enabled = false;
            //ds = _cMDB.RellenarDataSet("Valor", "SELECT id_punto FROM [Punto] WHERE Id_Punto=" + ((ComboItem)lstboxListas.SelectedItem).Id );
            //draux = ds.Tables[0].Rows[0];
            _idpunto = ((ComboItem)lstboxListas.SelectedItem).Id;//Conversions.ToInteger(draux[0]);
            // Sacar nombre y id
            if (lstboxListas.SelectedIndex != -1)
            {
                ds = _cMDB.RellenarDataSet("Valor", "SELECT * FROM [Lista] WHERE ID_punto=" + _idpunto.ToString());
                // dr = ds.Tables(0).Rows(0)
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // Borrar lista natural diaria
                    if (Conversions.ToBoolean(Operators.AndObject(dr["Tipo_Lista"], dr["Tipo_Fechas"])))
                    {
                        btnBorrarNatDiaria.Enabled = true;
                    }
                    else if (Conversions.ToBoolean(Operators.AndObject(dr["Tipo_Lista"], !(bool)dr["Tipo_Fechas"])))
                    {
                        btnBorrarNatMensual.Enabled = true;
                    }
                }
            }

            var argcombo = cmboxAlt;
            _Rellenar.RellenarAlteraciones(ref argcombo, _idpunto);
            cmboxAlt = argcombo;
        }

        private void cmboxAlt_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds;
            DataRow draux;
            btnAltDiaria.Enabled = false;
            btnAltMensual.Enabled = false;
            //ds = _cMDB.RellenarDataSet("Valor", "SELECT id_alteracion FROM [Alteracion] WHERE COD_Alteracion='" + cmboxAlt.SelectedItem.ToString() + "'");
            //draux = ds.Tables[0].Rows[0];
            _codAlt = ((ComboItem)cmboxAlt.SelectedItem).Id;// Conversions.ToInteger(draux[0]);
            ds = _cMDB.RellenarDataSet("Valor", "SELECT * FROM [Lista] WHERE ID_punto=" + _idpunto.ToString() + " AND id_alteracion=" + _codAlt.ToString());
            // dr = ds.Tables(0).Rows(0)
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // Borrar lista natural diaria
                if (Conversions.ToBoolean(dr["Tipo_Fechas"]))
                {
                    btnAltDiaria.Enabled = true;
                }
                else if (Conversions.ToBoolean(!(bool)dr["Tipo_Fechas"]))
                {
                    btnAltMensual.Enabled = true;
                }
            }
        }

        private void btnBorrarNatDiaria_Click(object sender, EventArgs e)
        {
            _cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " + _idpunto + " AND Tipo_Lista=TRUE AND Tipo_fechas=TRUE AND id_alteracion=0");
            lboxListas_SelectedIndexChanged(lstboxListas, null);
        }

        private void btnBorrarNatMensual_Click(object sender, EventArgs e)
        {
            _cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " + _idpunto + " AND Tipo_Lista=TRUE AND Tipo_fechas=FALSE AND id_alteracion=0");
            lboxListas_SelectedIndexChanged(lstboxListas, null);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Dim idLista As Integer

            // Dim ds As DataSet
            // Dim dr As DataRow

            // ' Sacar nombre y id
            // If (Me.lstboxListas.SelectedIndex <> -1) Then
            // ds = Me._cMDB.RellenarDataSet("Valor", "SELECT id_lista FROM Lista WHERE nombre='" & Me.lstboxListas.SelectedItem.ToString() & "'")

            // dr = ds.Tables(0).Rows(0)

            // idLista = Integer.Parse(dr("id_lista"))

            // Me._cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_lista = " & idLista)

            // Me._Rellenar.RellenarListas(Me.lstboxListas, -1)
            // End If
            _cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " + _idpunto + " AND Tipo_Lista=FALSE AND Tipo_fechas=TRUE AND id_alteracion=" + _codAlt.ToString());
            lboxListas_SelectedIndexChanged(lstboxListas, null);
        }

        private void btnAltMensual_Click(object sender, EventArgs e)
        {
            _cMDB.EjecutarSQL("DELETE * FROM Lista WHERE id_punto = " + _idpunto + " AND Tipo_Lista=FALSE AND Tipo_fechas=FALSE AND id_alteracion=" + _codAlt.ToString());
            lboxListas_SelectedIndexChanged(lstboxListas, null);
        }

        private void cmbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var arglistbox = lstboxListas;
            _Rellenar.RellenarPuntos(ref arglistbox, ((ComboItem)cmbProyectos.SelectedItem).Id);
            lstboxListas = arglistbox;
            btnBorrarNatDiaria.Enabled = false;
            btnBorrarNatMensual.Enabled = false;
            btnAltDiaria.Enabled = false;
            btnAltMensual.Enabled = false;
            Left = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Width - Width));
            Top = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Height - Height));
        }
    }
}