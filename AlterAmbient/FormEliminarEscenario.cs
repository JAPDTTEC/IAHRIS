using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Calculo.CaudalesEcologicos;
using IAHRIS.Rellenar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IAHRIS.Calculo;

namespace IAHRIS
{
    public partial class FormEliminarEscenario : Form
    {
        
        Escenarios _escenarios;
        
        BBDD.OleDbDataBase _cMDB;

        public FormEliminarEscenario()
        {
            InitializeComponent();
        }
        public FormEliminarEscenario(BBDD.OleDbDataBase MDB, int idPunto, int idAlteracion)
        {
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

            DialogResult result = MessageBox.Show("¿Está seguro de que desea borrar el escenario seleccionado?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                _escenarios.EliminarEscenarioBD(escSelec.Id);
                // Si la respuesta es Sí
                MessageBox.Show("Escenario borrado.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                // Aquí puedes agregar la lógica para borrar
            }
            
            Dispose();
        }
    }
}
