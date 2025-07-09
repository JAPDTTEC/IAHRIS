using IAHRIS.Calculo;
using IAHRIS.Calculo.CaudalesEcologicos;
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using MultiLangXML;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormAnadirEscenarioRNORM : Form
    {
        
        EscenarioDTO _escenario;
        Escenarios _escenarios;
        SerieRCE _serieRCE;
        bool _edit = false;
        private MultiIdiomasXML _traductor;

        public FormAnadirEscenarioRNORM()
        {
            _traductor = MultiIdiomasXML.Instancia;
            InitializeComponent();
        }

        public FormAnadirEscenarioRNORM(BBDD.OleDbDataBase MDB, SerieRCE serieRCE, int idPunto, int idAlteracion, EscenarioDTO escenario)
        {
            _traductor = MultiIdiomasXML.Instancia;


            foreach (double caudal in escenario.Caudales_Ecologicos) { 
                if(caudal!= 0) {  _edit = true; break; }
            }
            
            string titulo = "";
            string boton = "";

            if (_edit)
            {
                titulo = "Editar R_NORM";
                boton = "Editar Escenario";
            }
            else
            {
                titulo = "Añadir R_NORM";
                boton = "Crear Escenario";
            }

            init(MDB, serieRCE, idPunto, idAlteracion, escenario,titulo, boton);


        }

        public FormAnadirEscenarioRNORM(BBDD.OleDbDataBase MDB, SerieRCE serieRCE, int idPunto, int idAlteracion)
        {
            _traductor = MultiIdiomasXML.Instancia;


            EscenarioDTO escenario = new EscenarioDTO();

          

            escenario.Id_Punto_Ref = idPunto;
            escenario.Id_Alteracion_Ref = idAlteracion;
            escenario.Por_Defecto = false;
            escenario.Caudales_Ecologicos = new double[12];

            string titulo = "Añadir R_NORM";
            string boton = "Crear Escenario";
            init(MDB, serieRCE, idPunto, idAlteracion, escenario,titulo,boton);
        }


        private void init(BBDD.OleDbDataBase MDB, SerieRCE serieRCE, int idPunto, int idAlteracion, EscenarioDTO escenario, string titulo,string boton)
        {
            _escenarios = new Escenarios();
            _serieRCE = serieRCE;

            InitializeComponent();

            this.Text = titulo;
            this.btnCrearEscenario.Text = boton;

            for (int i = 0; i < 12; i++)
            {

                Label lblMes = Controls.Find("lblMes" + i.ToString(), true).FirstOrDefault() as Label;
                TextBox txtMes = Controls.Find("txtMes" + i.ToString(), true).FirstOrDefault() as TextBox;
                if (lblMes != null)
                {
                    int mes = (serieRCE.Mes_Inicio + i) % 12;
                    lblMes.Text = Utiles.ObtenerMes(mes == 0 ? 12 : mes);
                    if(escenario.Caudales_Ecologicos!=null)
                        txtMes.Text = escenario.Caudales_Ecologicos[i].ToString();
                }
            }

            _escenario = escenario;
        }
        private void btnCrearEscenario_Click(object sender, EventArgs e)
        {
            NumberFormatInfo nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string SeparadorDecimal = nfi.NumberDecimalSeparator;
            for (int i = 0; i < 12; i++)
            {
                System.Windows.Forms.TextBox txtMes = Controls.Find("txtMes" + i.ToString(), true).FirstOrDefault() as System.Windows.Forms.TextBox;

                try
                {
                    
                
                    txtMes.Text = txtMes.Text.Replace('.',SeparadorDecimal[0]);
                    txtMes.Text = txtMes.Text.Replace(',', SeparadorDecimal[0]);

                    _escenario.Caudales_Ecologicos[i] = Convert.ToDouble(txtMes.Text);
                }catch(Exception ex)
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

            //if (_escenario.Id_Alteracion_Ref != 0)
            //{
            //    caudales = _serieRCE.Caudales_R_ALT.ToArray();
            //}
            //else
            {
                caudales = _serieRCE.Caudales_R_NAT.ToArray();
            }
            _escenario.Puntuacion = _escenarios.CalcularPuntuacion(_escenario, _serieRCE.Mes_Inicio, caudales);
            _escenario.Demanda_Ambiental = _escenarios.CalcularDemandaAmbiental(_escenario, _serieRCE.Mes_Inicio, caudales);
            _escenario.Eficiencia = _escenarios.CalcularEficiencia(_escenario, _serieRCE.Mes_Inicio);
            if (_edit)
                _escenarios.EditarRNORM_BD(_escenario);
            else
            {
                _escenario.Nombre = "R_NORM";
                _escenarios.GuardaEscenarioBD(_escenario);
            }
            
            Dispose();
        }
    }
}
