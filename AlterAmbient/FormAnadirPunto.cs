using System;
using System.Data;
using System.Windows.Forms;
using IAHRIS.BBDD;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    public partial class FormAnadirPunto
    {
        public FormAnadirPunto()
        {
            InitializeComponent();
            _btnAceptar.Name = "btnAceptar";
            _txtClave.Name = "txtClave";
            _txtNombre.Name = "txtNombre";
            _btnCancelar.Name = "btnCancelar";
        }

        public FormAnadirPunto(OleDbDataBase MDB, string tipo)
        {


            // This call is required by the Windows Form Designer.
            InitializeComponent();
            if (tipo == "Punto")
            {
                Name = "FormAnadirPunto";
            }
            else
            {
                Name = "FormAnadirAlteracion";
            }

            // -------------------------------------
            // ---- Traducir formulario ------------
            // -------------------------------------
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _error = false;

            // Add any initialization after the InitializeComponent() call.
            _cMDB = MDB;
            if (tipo == "Punto")
            {
                gbAnadirPunto.Text = "Datos del Punto";
            }
            else
            {
                gbAnadirPunto.Text = "Datos de la Alteración";
            }

            //lblCodigo.Text = lblCodigo.Text + tipo;
            //lblDescrip.Text = lblDescrip.Text + tipo;
            if (tipo == "Alteración")
            {

                // Me.gbAnadirPunto.Name = "gbAnadirAlt"
                // Me.lblCodigo.Name = "lblCodigoAlt"
                // Me.lblDescrip.Name = "lblDescripAlt"
                // Me.lblProyecto.Name = "lblProyectoAlt"
               
                _tabla = "Alteracion";

                _rellenar = new Rellenar.RellenarForm(_cMDB);
                var argcombo = cmbProyectos;
                _rellenar.RellenarProyectos(ref argcombo);


                // Dim dr As DataRow
                cmbPuntos.Items.Clear(); // Limpiar el listbox
               // foreach (DataRow dr in ds.Tables[0].Rows)
                    // Console.WriteLine(dr("nombre").ToString())
                 //   cmbPuntos.Items.Add(dr["Clave_punto"].ToString());
               // cmbPuntos.SelectedIndex = 0;
            }
            else
            {
                lblPunto.Visible = false;
                cmbPuntos.Visible = false;
                _tabla = "Punto";
                //lblPunto.Text = "Proyecto asociado";
                _rellenar = new Rellenar.RellenarForm(_cMDB);
                var argcombo = cmbProyectos;
                _rellenar.RellenarProyectos(ref argcombo);
               // cmbPuntos = argcombo;

                // Me.Label1.Visible = False
                // Me.cmbPuntos.Visible = False
            }

            _btnAceptar.Name = "btnAceptar";
            _txtClave.Name = "txtClave";
            _txtNombre.Name = "txtNombre";
            _btnCancelar.Name = "btnCancelar";
        }

        private BBDD.OleDbDataBase _cMDB;
        private string _tabla;
        private Rellenar.RellenarForm _rellenar;
        private MultiLangXML.MultiIdiomasXML _traductor;
        private bool _error;

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClave.Text) | string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strEmptyValues"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            string[] s;
            if (_tabla == "Alteracion")
            {
                s = new string[3];
                s[0] = "COD_Alteracion";
                s[1] = "nombre";
                s[2] = "ID_Punto";
                //var ds = _cMDB.RellenarDataSet("Puntos", "Select id_punto From Punto where clave_punto='" + cmbPuntos.SelectedItem.ToString() + "'");
                //var dr = ds.Tables[0].Rows[0];
                var t = new[] { txtClave.Text.ToUpperInvariant(), txtNombre.Text, ((ComboItem)cmbPuntos.SelectedItem).Id.ToString()};
                if (Conversions.ToBoolean(existeCodigo("alteracion", txtClave.Text.ToUpperInvariant(), ((ComboItem)cmbProyectos.SelectedItem).Id)))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertAltDupId"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                if (_cMDB.InsertarRegistro(_tabla, s, t))
                {
                    Dispose();
                }
                else
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertAlt"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
            else
            {
                s = new string[4];
                s[0] = "Clave_punto";
                s[1] = "nombre";
                s[2] = "ID_proyecto";
                s[3] = "mesInicio";
                var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT ID_Proyecto FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");
                //var dr = ds.Tables[0].Rows[cmbPuntos.SelectedIndex];
                if (Conversions.ToBoolean(existeCodigo("punto", txtClave.Text.ToUpperInvariant(),((ComboItem)cmbProyectos.SelectedItem).Id)))
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertPointDupId"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                string[] t = new string[] { txtClave.Text.ToUpperInvariant(), txtNombre.Text, ((ComboItem)cmbProyectos.SelectedItem).Id.ToString(), "10" };
                if (_cMDB.InsertarRegistro(_tabla, s, t))
                {
                    Dispose();
                }
                else
                {
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorInsertPoint"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Dispose();
        }

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
            // Centrar el formulario
            // Me.Left = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - Me.Width)
            // Me.Top = 0.5 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Me.Height)

            // If Me._tabla = "Punto" Then
            // Me.Text = "Añadir " & Me._tabla
            // Else
            // Me.Text = "Añadir Alteración"
            // End If

            // Help.ShowHelp(Me, "AlterHidrol.chm", HelpNavigator.KeywordIndex, "Añadir punto")
            cmbProyectos_SelectedIndexChanged(null, e);
        }

        private void txtClave_TextChanged(object sender, EventArgs e)
        {
            if (Strings.Len(txtClave.Text) > 12)
            {
                // MsgBox("", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR DE CÓDIGO")
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongCode"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtClave.Text = Strings.Left(txtClave.Text, 12);
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (Strings.Len(txtNombre.Text) > 20)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strTooLongName"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                // MsgBox("", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR DE DESCRIPCIÓN")
                txtNombre.Text = Strings.Left(txtNombre.Text, 20);
            }
        }

        private void FormAnadirPunto_Shown(object sender, EventArgs e)
        {
            if (_error)
            {
                Close();
            }
        }

        private object existeCodigo(string tabla, string codigo, int idProyecto)
        {
            DataSet ds;
            DataRow dr;
            if (tabla == "punto")
            {
                ds = _cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM punto WHERE clave_punto='" + codigo + "' AND ID_proyecto=" + idProyecto.ToString());
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
            else if (tabla == "alteracion")
            {
                ds = _cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM Vista_alteracionFull WHERE COD_Alteracion='" + codigo + "' AND ID_proyecto=" + idProyecto.ToString());
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

            return false;
        }

        private void cmbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tabla == "Alteracion")
            {
                ComboItem cbi = (ComboItem) cmbProyectos.SelectedItem;
                // Rellenar con los puntos del sistema
                var ds = _cMDB.RellenarDataSet("Puntos", "SELECT * FROM Punto WHERE ID_Proyecto = " +cbi.Id  + " ORDER BY clave_punto ASC");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    // Dim strError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPoint", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")
                    // Dim strCaptionError As String = Me._traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError", "D:\Trabajos\1720-IAHRIS v2\xml idiomas\spanish.xml", "")

                    // MessageBox.Show(strError, strCaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNoPointDefined"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _error = true;
                    return;
                }
                else
                {


                    _rellenar.RellenarPuntos(ref cmbPuntos,cbi.Id);
                }
            }
        }
    }
}