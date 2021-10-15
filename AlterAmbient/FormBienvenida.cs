using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormBienvenida
    {
        public FormBienvenida()
        {
            InitializeComponent();

            Form argform = this;
            MultiLangXML.MultiIdiomasXML _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");

            _pbPrograma.Name = "pbPrograma";
            DateTime fileDate = new FileInfo(Application.ExecutablePath).LastWriteTime;
            _traductor.cambiarIdioma(Application.StartupPath+@"\lang\english.xml");
            lblVersionEN.Text = Application.ProductVersion +"                 " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, fileDate.Month.ToString()) + " " + fileDate.Year.ToString();
            _traductor.cambiarIdioma(Application.StartupPath + @"\lang\spanish.xml");
            lblVersionES.Text = Application.ProductVersion + "                 " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, fileDate.Month.ToString()) + " " + fileDate.Year.ToString(); 
        }


        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void pbPrograma_Click(object sender, EventArgs e)
        {
            var fInicio = new FormInicial();
            fInicio.Show();
            Hide();
        }

        private void pbPrograma_MouseHover(object sender, EventArgs e)
        {
            pbPrograma.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbPrograma_MouseLeave(object sender, EventArgs e)
        {
            pbPrograma.BorderStyle = BorderStyle.None;
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void pbManual_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@".\Manual\Manual Usuario.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Private Sub pbManual_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs)
        // Me.pbManual.BorderStyle = BorderStyle.Fixed3D
        // End Sub

        // Private Sub pbManual_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
        // Me.pbManual.BorderStyle = BorderStyle.None
        // End Sub
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void pbReferencia_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@".\Manual\Manual Referencia.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Private Sub pbReferencia_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs)
        // Me.pbReferencia.BorderStyle = BorderStyle.Fixed3D
        // End Sub

        // Private Sub pbReferencia_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
        // Me.pbReferencia.BorderStyle = BorderStyle.None
        // End Sub
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

        private void pbAcercaDe_Click(object sender, EventArgs e)
        {
            var fAcerca = new FormAcercaDe();
            fAcerca.ShowDialog();
        }
    }
}