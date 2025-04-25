using IAHRIS.BBDD;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormAnadirPunto
    {
        int _idp;
        private OleDbDataBase _cMDB;
        private string _tabla;
        private RellenarForm _rellenar;
        private MultiLangXML.MultiIdiomasXML _traductor;
        private bool _error;

        /// <summary>
        /// Constructor
        /// </summary>
        public FormAnadirPunto()
        {
            InitializeComponent();
            _btnAceptar.Name = "btnAceptar";
            _txtClave.Name = "txtClave";
            _txtNombre.Name = "txtNombre";
            _btnCancelar.Name = "btnCancelar";
        }

        /// <summary>
        /// Formulario Añadir Punto
        /// </summary>
        /// <param name="MDB">Base de Datos</param>
        /// <param name="tipo">Punto o Alteración</param>
        /// <param name="editar">True si es formulario de Editar</param>
        /// <param name="p"></param>
        public FormAnadirPunto(OleDbDataBase MDB, string tipo, bool editar, int p = 0)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();


            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _error = false;

            // Add any initialization after the InitializeComponent() call.
            _cMDB = MDB;

            if (editar == false)
                AñadirElemento(tipo, p);
            else
                EditarElemento(tipo, p);
        }

        private void EditarElemento(string tipo, int p)
        {

            if (tipo == "Punto")
            {
                this.Name = "FormEditarPunto";
                Text = "Editar Punto";
                lblCodigo.Text = "Código Punto";
                lblDescrip.Text = "Descripción Punto";
                _txtNombreReg.Text = "Natural";
                _txtAbreviatura.Text = "nat";
                gbAnadirPunto.Text = "Datos del Punto";

                _tabla = "Punto";

                this.cmbProyectos.Visible = false;
                this.lblProyecto.Visible = false;
            }
            else
            {
                this.Name = "FormEditarAlteracion";
                Text = "Editar Alteración";
                lblCodigo.Text = "Código Alteración";
                lblDescrip.Text = "Descripción Alteración";
                _txtNombreReg.Text = "Alterado";
                _txtAbreviatura.Text = "alt";
                gbAnadirPunto.Text = "Datos de la Alteración";

                _tabla = "Alteracion";

                this.cmbProyectos.Visible = false;
                this.lblProyecto.Visible = false;

                cmbPuntos.Items.Clear(); // Limpiar el listbox

            }

            lblPunto.Visible = false;
            cmbPuntos.Visible = false;

            _btnEditar.Enabled = true;
            _btnAceptar.Enabled = false;

            _idp = p;
            string str;
            if (tipo == "Alteración")
            {
                str = "SELECT * FROM [Alteracion] WHERE ID_Alteracion = " + p;
                var ds = _cMDB.RellenarDataSet("Alteraciones", str);
                var dr = ds.Tables[0].Rows[0];

                _txtClave.Text = dr[0].ToString();
                _txtNombre.Text = dr[1].ToString();

                if (dr[4].ToString() == "Alterado" && dr[5].ToString() == "R alt")
                {
                    _chkboxPersReg.Checked = false;
                }
                else
                {
                    _chkboxPersReg.Checked = true;
                    _txtNombreReg.Text = dr[4].ToString();
                    _txtAbreviatura.Text = dr[5].ToString();
                }

            }
            else
            {
                str = "SELECT * FROM [Punto] WHERE ID_Punto = " + p;
                var ds = _cMDB.RellenarDataSet("Proyectos", str);
                var dr = ds.Tables[0].Rows[0];


                _txtClave.Text = dr[0].ToString();
                _txtNombre.Text = dr[1].ToString();

                if (dr[6].ToString() == "Natural" && dr[5].ToString() == "R nat")
                {
                    _chkboxPersReg.Checked = false;
                }
                else
                {
                    _chkboxPersReg.Checked = true;

                    _txtNombreReg.Text = dr[6].ToString();
                    _txtAbreviatura.Text = dr[5].ToString();
                }
            }

            _btnAceptar.Name = "btnAceptar";
            _txtClave.Name = "txtClave";
            _txtNombre.Name = "txtNombre";
            _btnCancelar.Name = "btnCancelar";
        }

        private void AñadirElemento(string tipo, int p)
        {
            if (tipo == "Punto")
            {
                Name = "FormAnadirPunto";
                Text = "Añadir Punto";
                lblCodigo.Text = "Codigo Punto";
                lblDescrip.Text = "Descripción Punto";
                _txtNombreReg.Text = "Natural";
                _txtAbreviatura.Text = "nat";

                gbAnadirPunto.Text = "Datos del Punto";

                lblPunto.Visible = false;
                cmbPuntos.Visible = false;
                _tabla = "Punto";
                _rellenar = new RellenarForm(_cMDB);
                var argcombo = cmbProyectos;
                _rellenar.RellenarProyectos(ref argcombo);
            }
            else
            {
                Name = "FormAnadirAlteracion";
                Text = "Añadir Alteracion";
                lblCodigo.Text = "Codigo Alteración";
                lblDescrip.Text = "Descripción Alteración";
                _txtNombreReg.Text = "Alterado";
                _txtAbreviatura.Text = "alt";
                gbAnadirPunto.Text = "Datos de la Alteración";

                _tabla = "Alteracion";

                _rellenar = new Rellenar.RellenarForm(_cMDB);
                var argcombo = cmbProyectos;
                _rellenar.RellenarProyectos(ref argcombo, p);


                cmbPuntos.Items.Clear(); // Limpiar el listbox
            }

            _btnEditar.Enabled = false;

            _btnAceptar.Name = "btnAceptar";
            _txtClave.Name = "txtClave";
            _txtNombre.Name = "txtNombre";

            _btnCancelar.Name = "btnCancelar";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClave.Text) | string.IsNullOrEmpty(txtNombre.Text) | string.IsNullOrEmpty(_txtAbreviatura.Text))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strEmptyValues"),
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            string[] s;
            if (_tabla == "Alteracion")
            {
                s = new string[] { "COD_Alteracion", "nombre", "ID_Punto", "nombreRegimen", "abreviaturaRegimen" };

                var t = new[] { txtClave.Text.ToUpperInvariant(), txtNombre.Text,
                                ((ComboItem)cmbPuntos.SelectedItem).Id.ToString(), 
                                _txtNombreReg.Text, _txtAbreviatura.Text 
                               };

                if (Conversions.ToBoolean(existeCodigo("alteracion", txtClave.Text.ToUpperInvariant(), ((ComboItem)cmbProyectos.SelectedItem).Id)))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertAltDupId"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!_cMDB.InsertarRegistro(_tabla, s, t))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertAlt"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Dispose();
            }
            else
            {
                s = new string[] { "Clave_punto", "nombre", "ID_proyecto", "mesInicio", "nombreRegimen" , "abreviaturaRegimen" };

                if (Conversions.ToBoolean(existeCodigo("punto", txtClave.Text.ToUpperInvariant(),((ComboItem)cmbProyectos.SelectedItem).Id)))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertPointDupId"), 
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] t = new string[] { txtClave.Text.ToUpperInvariant(), txtNombre.Text,
                                            ((ComboItem)cmbProyectos.SelectedItem).Id.ToString(), "10" ,
                                            _txtNombreReg.Text ,_txtAbreviatura.Text };
                
                if (!_cMDB.InsertarRegistro(_tabla, s, t))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertPoint"),
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Dispose();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Este método no hace nada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void FormAnadirPunto_KeyDown(object sender, KeyEventArgs e)
        {
            string strText;
            if (e.KeyCode == Keys.F1)
            {
                if (Text == "Añadir Punto")
                {
                    strText = "Añadir punto";
                }
                else
                {
                    strText = "Añadir alteración";
                }
                // Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, strText)
            }
        }

        private void FormAnadirPunto_Load(object sender, EventArgs e)
        {
            cmbProyectos_SelectedIndexChanged(null, e);
        }

        private void txtClave_TextChanged(object sender, EventArgs e)
        {
            if (Strings.Len(txtClave.Text) > 12)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongCode"),
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtClave.Text = Strings.Left(txtClave.Text, 12);
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (Strings.Len(txtNombre.Text) > 20)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongName"),
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtNombre.Text = Strings.Left(txtNombre.Text, 20);
            }
        }
        private void txtAbreviatura_TextChanged(object sender, EventArgs e)
        {
            if (Strings.Len(_txtAbreviatura.Text) > 5)
            {
                
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongAbrev"),
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);


                _txtAbreviatura.Text = Strings.Left(_txtAbreviatura.Text, 5);
            }
        }
        private void txtNombreReg_TextChanged(object sender, EventArgs e)
        {
            if (Strings.Len(_txtNombreReg.Text) > 255)
            {

                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongNameReg"),
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);


                _txtNombreReg.Text = Strings.Left(_txtNombreReg.Text, 255);
            }
        }

        private void FormAnadirPunto_Shown(object sender, EventArgs e)
        {
            if (_error)  Close();           
        }

        private object existeCodigo(string tabla, string codigo, int idProyecto)
        {
            DataSet ds = null;
            DataRow dr;

            if (tabla != "punto" && tabla != "alteracion")
                return false;

            if (tabla == "punto")
                ds = _cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM punto WHERE clave_punto='" + codigo + "' AND ID_proyecto=" + idProyecto.ToString());
            
            if (tabla == "alteracion")
                ds = _cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM Vista_alteracionFull WHERE COD_Alteracion='" + codigo + "' AND ID_proyecto=" + idProyecto.ToString());

            dr = ds.Tables[0].Rows[0];
            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectGreater(dr[0], 0, false)))                
                return true;    

            return false;
        }

        private void cmbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tabla == "Alteracion")
            {
                ComboItem cbi = (ComboItem) cmbProyectos.SelectedItem;

                if (cbi == null)
                    return;

                // Rellenar con los puntos del sistema
                var ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM Punto WHERE ID_Proyecto = " +cbi.Id  + " ORDER BY clave_punto ASC");

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"),
                                    _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _error = true;
                    return;
                }

                _rellenar.RellenarPuntos(ref cmbPuntos,cbi.Id);
                cmbPuntos.SelectedIndex = 0;
                
            }
        }

        private void _chkboxPersReg_CheckedChanged(object sender, EventArgs e)
        {
            if(_chkboxPersReg.Checked == true) 
            {
                _txtNombreReg.Enabled = true;
                _txtAbreviatura.Enabled = true;
            }
            else
            {
                if (_tabla == "Punto")
                {
                    _txtNombreReg.Text = "Natural";
                    _txtAbreviatura.Text = "nat";
                }
                else
                {
                    _txtNombreReg.Text = "Alterado";
                    _txtAbreviatura.Text = "alt";
                }         

                _txtNombreReg.Enabled = false;
                _txtAbreviatura.Enabled = false;
                
            }
        }

        private void _btnEditar_Click(object sender, EventArgs e)
        {
            string consulta;
            if (_tabla == "Alteracion")
                consulta = "UPDATE [Alteracion] SET COD_Alteracion = '" + txtClave.Text.ToUpperInvariant() + "', Nombre = '" + txtNombre.Text + "', nombreRegimen = '" + _txtNombreReg.Text + "', abreviaturaRegimen = '" + _txtAbreviatura.Text + "' WHERE ID_Alteracion = " + _idp; 
            else
                consulta = "UPDATE [Punto] SET Clave_punto= '" + txtClave.Text.ToUpperInvariant() + "', Nombre = '" + txtNombre.Text + "', nombreRegimen = '" + _txtNombreReg.Text + "', abreviaturaRegimen = '" + _txtAbreviatura.Text + "' WHERE ID_Punto = " + _idp;


            if (_cMDB.EjecutarSQL(consulta) == -1)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorUpdateProj"),
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Dispose();
        }
    }
    
}