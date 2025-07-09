using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Rellenar;
using MultiLangXML;
using System;
using System.Data;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormEliminarEscenario : Form
    {
        
        Escenarios _escenarios;
        MultiIdiomasXML _traductor;
        
        BBDD.OleDbDataBase _cMDB;

        public FormEliminarEscenario()
        {
            _traductor = MultiIdiomasXML.Instancia;
            InitializeComponent();
        }
        public FormEliminarEscenario(BBDD.OleDbDataBase MDB, int idPunto, int idAlteracion)
        {
            _traductor = MultiIdiomasXML.Instancia;
            _escenarios = new Escenarios();     

            _cMDB = MDB;
            InitializeComponent();
            btnEliminarEscenario.Enabled = false;

            var ds = _cMDB.RellenarDataSet("Escenario", "SELECT Nombre_Escenario, Id_Escenario FROM [Escenario] WHERE Id_Punto = " + idPunto + " AND Id_Alteracion = " + idAlteracion + "AND Por_Defecto = False");

            cmbEscenario.Items.Clear();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr["Nombre_Escenario"].ToString(), (int)dr["Id_Escenario"]);
                cmbEscenario.Items.Add(it);

            }
        }
        private void cmbEscenario_SelectedIndexChanged(object sender, EventArgs e)
        {
  
            if (cmbEscenario.SelectedIndex != -1)
            {
                btnEliminarEscenario.Enabled = true;
            }
        }

        private void btnEliminarEscenario_Click(object sender, EventArgs e)
        {
            ComboItem escSelec = (ComboItem)cmbEscenario.SelectedItem;

            DialogResult result = MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strDeseaBorrarEscenario"), _traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strConfirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                _escenarios.EliminarEscenarioBD(escSelec.Id);
                // Si la respuesta es Sí
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "srtEscenarioBorrado"), "Info" ,
                                                            MessageBoxButtons.OK,MessageBoxIcon.Information);
                // Aquí puedes agregar la lógica para borrar
            }
            
            Dispose();
        }
    }
}
