using IAHRIS.BBDD;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormAnadirProyecto
    {
        int _idProyecto;
        private BBDD.OleDbDataBase _cMDB;
        private MultiLangXML.MultiIdiomasXML _traductor;

        public FormAnadirProyecto()
        {
            InitializeComponent();
            _btnAceptar.Name = "btnAceptar";
        }

        public FormAnadirProyecto(OleDbDataBase MDB, bool editar, int p=0)
        {
            Form argform = this;

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            _cMDB = MDB;

            _btnEditar.Name = "btnEditar";
            _btnAceptar.Name = "btnAceptar";

            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            _traductor = MultiLangXML.MultiIdiomasXML.Instancia;
            _traductor.TraducirForm(this);

            _btnEditar.Enabled = false;

            if (editar == true) 
            {
                _btnEditar.Enabled = true;
                _btnAceptar.Enabled = false;
                _idProyecto = p;
                
                //obtener proyecto seleccionado y rellenar campos                
                string str = "SELECT * FROM [Proyecto] WHERE ID_Proyecto = " + p;
                var ds = _cMDB.RellenarDataSet("Proyectos", str);
                var dr = ds.Tables[0].Rows[0];

                txtNombre.Text = dr[1].ToString();
                txtDescripcion.Text = dr[2].ToString();      
            }
        }       


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var valores = new[] { txtNombre.Text, txtDescripcion.Text };
            var campos = new[] { "nombre", "descripcion" };

            //Validaciones
            if (string.IsNullOrEmpty(txtNombre.Text) & !string.IsNullOrEmpty(txtDescripcion.Text))            
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorValidationProj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
          
            if (txtNombre.Text.Length > 20)           
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongName"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);




            //Insertar

            if (!ExisteProyecto(valores[0]))
            {
                if (!_cMDB.InsertarRegistro("Proyecto", campos, valores))        
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR,
                                    "strErrorInsertProj"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, 
                                    "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorProjectAlreadyExist"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            Dispose();
            return;
                     
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {           
            string str = "UPDATE [Proyecto] " +
                         "SET nombre = '" + txtNombre.Text + "', descripcion = '" + txtDescripcion.Text +  "' " +
                         "WHERE ID_Proyecto = "+ _idProyecto;

            if (_cMDB.EjecutarSQL(str) == -1)                    
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorUpdateProj"),
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

            Dispose();
            return;
            
        }

        private bool ExisteProyecto(string nombreProyecto)
        {
            DataSet ds = null;
            DataRow dr;

            ds = _cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM Proyecto WHERE nombre='" + nombreProyecto + "'");

            dr = ds.Tables[0].Rows[0];
            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectGreater(dr[0], 0, false)))
            {
                return true;
            }            
            else
            {
                return false;
            }
        }
    }
}