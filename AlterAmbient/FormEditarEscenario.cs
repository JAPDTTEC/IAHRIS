using IAHRIS.Calculo;
using IAHRIS.Calculo.CaudalesEcologicos;
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using IAHRIS.Rellenar;
using MultiLangXML;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormEditarEscenario : Form
    {
        EscenarioDTO _escenario;
        Escenarios _escenarios;
        SerieRCE _serieRCE;
        BBDD.OleDbDataBase _cMDB;
        private MultiIdiomasXML _traductor;

        public FormEditarEscenario()
        {
            _traductor = MultiIdiomasXML.Instancia;
            InitializeComponent();
        }
        public FormEditarEscenario(BBDD.OleDbDataBase MDB, SerieRCE serieRCE, int idPunto, int idAlteracion)
        {
            _traductor = MultiIdiomasXML.Instancia;
            _escenarios = new Escenarios();

            _escenario = new EscenarioDTO();

            _serieRCE = serieRCE;

            _cMDB = MDB;
            InitializeComponent();
            
            _escenario.Id_Punto_Ref = idPunto;
            _escenario.Id_Alteracion_Ref = idAlteracion;

            btnEditarEscenario.Enabled = false;

            var ds = _cMDB.RellenarDataSet("Escenario", "SELECT Nombre_Escenario, Id_Escenario FROM [Escenario] WHERE Id_Punto = " + idPunto + " AND Id_Alteracion = " + idAlteracion + "AND Por_Defecto = False");

            for (int i = 0; i < 12; i++)
            {

                Label lblMes = Controls.Find("lblMes" + i.ToString(), true).FirstOrDefault() as Label;
                if (lblMes != null)
                {
                    int mes = (serieRCE.Mes_Inicio + i) % 12;
                    lblMes.Text = Utiles.ObtenerMes(mes == 0 ? 12 : mes);
                }
            }
       
            cmbEscenario.Items.Clear();
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ComboItem it = new ComboItem(dr["Nombre_Escenario"].ToString(), (int)dr["Id_Escenario"]);
                cmbEscenario.Items.Add(it);
                
            }
        }

        private void cmbEscenario_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem escSelec = (ComboItem)cmbEscenario.SelectedItem;
            var ds = _cMDB.RellenarDataSet("Escenario", "SELECT Nombre_Escenario, Descripcion_Escenario, Mes0, Mes1, Mes2, Mes3, Mes4, Mes5, Mes6, Mes7, Mes8, Mes9, Mes10, Mes11 FROM [Escenario] WHERE Id_Escenario = "+ escSelec.Id);

            
            txtNombre.Text = ds.Tables[0].Rows[0][0].ToString().Substring(2);
            txtDescripcion.Text = ds.Tables[0].Rows[0][1].ToString();


            for (int i = 0; i < 12; i++)
            {

                TextBox txtMes = Controls.Find("txtMes" + i.ToString(), true).FirstOrDefault() as TextBox;
                if (txtMes != null)
                {
                    double caudal = Convert.ToDouble(ds.Tables[0].Rows[0][2 + i]);
                    txtMes.Text = caudal.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
                    
                }
            }

            if(cmbEscenario.SelectedIndex != -1)
            {
                btnEditarEscenario.Enabled = true;
            }
        }

        private void btnEditarEscenario_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorSinNombreYDescripcion"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (txtNombre.Text.Length > 4)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorNomEscenarioMaxCaracteres"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtDescripcion.Text.Length > 50)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDescEscenarioMaxCaracteres"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ComboItem escSelec = (ComboItem)cmbEscenario.SelectedItem;
            _escenario.Id_Escenario = escSelec.Id;
            _escenario.Nombre = "R_" + txtNombre.Text;
            _escenario.Descripcion = txtDescripcion.Text;
            _escenario.Caudales_Ecologicos = new double[12];
            

            for (int i = 0; i < 12; i++)
            {
                System.Windows.Forms.TextBox txtMes = Controls.Find("txtMes" + i.ToString(), true).FirstOrDefault() as System.Windows.Forms.TextBox;

                if (!double.TryParse(txtMes.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _escenario.Caudales_Ecologicos[i]))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorNumeroValidos"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMes.Clear();
                    return;
                }

                string[] partes = txtMes.Text.Split('.');
                if (partes.Length == 2 && partes[1].Length > 3)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorNumeroSolo3Decimales"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMes.Clear(); // Limpiar el TextBox
                    return;
                }
                string[] partes2 = txtMes.Text.Split(',');
                if (partes2.Length == 2 && partes2[1].Length > 3)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorNumeroSolo3Decimales"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMes.Clear(); // Limpiar el TextBox
                    return;
                }
            }

            double[][] caudales;

            if (_escenario.Id_Alteracion_Ref != 0)
            {
                caudales = _serieRCE.Caudales_R_ALT.ToArray();
            }
            else
            {
                caudales = _serieRCE.Caudales_R_NAT.ToArray();
            }



            _escenario.Puntuacion = _escenarios.CalcularPuntuacion(_escenario, _serieRCE.Mes_Inicio, caudales);


            _escenario.Demanda_Ambiental = _escenarios.CalcularDemandaAmbiental(_escenario, _serieRCE.Mes_Inicio, caudales);
            _escenario.Eficiencia = _escenarios.CalcularEficiencia(_escenario, _serieRCE.Mes_Inicio);

            
            _escenarios.EditarEscenarioBD(_escenario);

            Dispose();
        }
    }
}
