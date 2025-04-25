using IAHRIS.Calculo;
using IAHRIS.Calculo.CaudalesEcologicos;
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
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
using static IAHRIS.Calculo.TestFechas;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IAHRIS
{
    public partial class FormAnadirEscenario : Form
    {

        EscenarioDTO _escenario;
        Escenarios _escenarios;
        SerieRCE _serieRCE;

        public FormAnadirEscenario()
        {
            InitializeComponent();
        }

        public FormAnadirEscenario(BBDD.OleDbDataBase MDB, SerieRCE serieRCE, int idPunto, int idAlteracion)
        {
            _escenarios = new Escenarios();

            _escenario = new EscenarioDTO();

            _serieRCE = serieRCE;

            InitializeComponent();

            for (int i = 0; i < 12; i++)
            {
                
                Label lblMes = Controls.Find("lblMes" + i.ToString(), true).FirstOrDefault() as Label;
                if (lblMes != null)
                {
                    int mes = (serieRCE.Mes_Inicio + i) % 12;
                    lblMes.Text = Utiles.ObtenerMes(mes == 0 ? 12 : mes);
                }
            }

            _escenario.Id_Punto_Ref = idPunto;
            _escenario.Id_Alteracion_Ref = idAlteracion;
            _escenario.Por_Defecto = false;
            _escenario.Caudales_Ecologicos = new double[12];



        }

        private void btnCrearEscenario_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor, complete el campo del nombre y la descripcion del Escenario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (txtNombre.Text.Length > 4)
            {
                MessageBox.Show("El nombre del escenario tiene un tamaño máximo de 4 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtDescripcion.Text.Length > 50)
            {
                MessageBox.Show("La descripcion del escenario tiene un tamaño máximo de 50 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            _escenario.Nombre = "R_" + txtNombre.Text;
            _escenario.Descripcion = txtDescripcion.Text;

            NumberFormatInfo nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string SeparadorDecimal = nfi.NumberDecimalSeparator;

            for (int i = 0; i < 12; i++)
            {
                System.Windows.Forms.TextBox txtMes = Controls.Find("txtMes" + i.ToString(), true).FirstOrDefault() as System.Windows.Forms.TextBox;

                txtMes.Text = txtMes.Text.Replace('.', SeparadorDecimal[0]);
                txtMes.Text = txtMes.Text.Replace(',', SeparadorDecimal[0]);

                if (!double.TryParse(txtMes.Text,  out _escenario.Caudales_Ecologicos[i])) 
                {
                    MessageBox.Show("Por favor, ingrese solo números válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMes.Clear();
                    return;
                }

                string[] partes = txtMes.Text.Split('.');
                if (partes.Length == 2 && partes[1].Length > 3)
                {
                    MessageBox.Show("El número no puede tener más de 3 decimales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMes.Clear(); // Limpiar el TextBox
                    return;
                }
                string[] partes2 = txtMes.Text.Split(',');
                if (partes2.Length == 2 && partes2[1].Length > 3)
                {
                    MessageBox.Show("El número no puede tener más de 3 decimales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMes.Clear(); // Limpiar el TextBox
                    return;
                }
            }

            double[][] caudales;

            //if(_escenario.Id_Alteracion_Ref != 0)
            //{
            //    caudales = _serieRCE.Caudales_R_ALT.ToArray();
            //}
            //else
            //{
                caudales = _serieRCE.Caudales_R_NAT.ToArray();
            //}

           

            _escenario.Puntuacion =  _escenarios.CalcularPuntuacion(_escenario, _serieRCE.Mes_Inicio,  caudales);


            _escenario.Demanda_Ambiental = _escenarios.CalcularDemandaAmbiental(_escenario, _serieRCE.Mes_Inicio, caudales);
            _escenario.Eficiencia = _escenarios.CalcularEficiencia(_escenario, _serieRCE.Mes_Inicio);

            _escenarios.GuardaEscenarioBD(_escenario);

            Dispose();
        }
    }
}
