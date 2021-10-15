using System;
using global::System.Windows.Forms;
using global::System.Xml;
using global::System.Xml.Schema;
using global::System.Xml.XPath;
using Microsoft.VisualBasic;

namespace MultiLangXML
{
    public class MultiIdiomasXML
    {
        private Form _form;
        private string _appPath;
        private string _rutaConf;
        private string _rutaXML;
        private string _rutaExcel;
        private bool _OK;

        public enum TIPO_MENSAJE
        {
            M_ERROR = 0,
            M_INFO = 1,
            M_OTHER = 2,
            M_TABLE = 3,
            M_MONTH = 4
        }

        /// <summary>
    /// Creación de la clase
    /// </summary>
    /// <param name="form">Formulario a traducir</param>
    /// <remarks></remarks>
        public MultiIdiomasXML(ref Form form)
        {
            _form = form;
        }

        public void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine("Validation has encounted errors.......................");
            if (args.Severity == XmlSeverityType.Error)
            {
                Console.WriteLine("Severity:{0}", args.Severity);
                Console.WriteLine("Message:{0}", args.Message);
                _OK = false;
            }
        }


        /// <summary>
    /// Traducir formulario
    /// </summary>
    /// <param name="rutaXML">Ruta al XML donde se encuentra la traducción</param>
    /// <param name="rutaXSD">Ruta donde se encuentra el XSD de validación del XML</param>
    /// <returns>Si todo ha ido bien</returns>
    /// <remarks>El fichero XSD no se puede modificar y esta unido a cada versión de la librería</remarks>
        public bool traducirForm(string rutaXML, string rutaXSD)
        {
            XPathDocument xmldoc;
            XPathNavigator xmlnav;
            try
            {
                xmldoc = new XPathDocument(rutaXML);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
                return false;
            }

            // Traducir el formulario
            XPathNodeIterator iterador;
            iterador = xmlnav.Select("//forms/form[@id=\"" + _form.Name + "\"]");
            if (iterador.MoveNext())
            {
                var node = iterador.Current;
                _form.Text = node.GetAttribute("string", "");
            }
            // Traducir los controles
            foreach (Control ctrl in _form.Controls)
                traducirControl(ctrl, xmldoc, xmlnav);
            xmlnav = null;
            xmldoc = null;
            return true;
        }

        public bool traducirFormPorConf(string rutaApp, string rutaXML)
        {
            XPathDocument xmldoc;
            XPathNavigator xmlnav;
            XPathNodeIterator iterador;
            string rutaLangXML;
            try
            {
                _rutaConf = rutaApp + rutaXML;
                _appPath = rutaApp;
                xmldoc = new XPathDocument(_rutaConf);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encuentra el fichero XML", "Error");
                return false;
            }

            iterador = xmlnav.Select("configuracion/idioma");
            if (iterador.MoveNext())
            {
                rutaLangXML = iterador.Current.Value;
                _rutaXML = _appPath + @"\" + rutaLangXML;
            }
            else
            {
                xmlnav = null;
                xmldoc = null;
                return false;
            }

            // Sacar el excel
            xmldoc = new XPathDocument(_rutaXML);
            xmlnav = xmldoc.CreateNavigator();
            iterador = xmlnav.Select("/language/excelFile");
            if (iterador.MoveNext())
            {
                _rutaExcel = _appPath + @"\Report\" + iterador.Current.Value;
                xmlnav = null;
                xmldoc = null;
            }
            else
            {
                _rutaExcel = "";
                xmlnav = null;
                xmldoc = null;
                return false;
            }

            return traducirForm(rutaApp + @"\" + rutaLangXML, "");
        }

        /// <summary>
    /// Traducir un control
    /// </summary>
    /// <param name="ctrlObj">Control a traducir</param>
    /// <param name="xmldoc">XML que se usa para traducir</param>
    /// <param name="xmlnav">El navegador de XML</param>
    /// <returns></returns>
    /// <remarks></remarks>
        private bool traducirControl(object ctrlObj, XPathDocument xmldoc, XPathNavigator xmlnav)
        {
            XPathNodeIterator iterador;
            if (ctrlObj is MenuStrip)
            {
                MenuStrip menu = ctrlObj as MenuStrip;
                if (menu.Items.Count > 0)
                {
                    foreach (ToolStripItem ctrlAux in menu.Items)
                        traducirControl(ctrlAux, xmldoc, xmlnav);
                }
            }
            else if (ctrlObj is ToolStripMenuItem)
            {
                ToolStripMenuItem ctrl = ctrlObj as ToolStripMenuItem;
                // Cambiar el TEXT si existe en nuestro XML
                iterador = xmlnav.Select("//forms/form[@id=\"" + _form.Name + "\"]/control[@id=\"" + ctrl.Name + "\"]");
                if (iterador.MoveNext())
                {
                    ctrl.Text = iterador.Current.Value;
                }

                if (ctrl.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem ctrlAux in ctrl.DropDownItems)
                        traducirControl(ctrlAux, xmldoc, xmlnav);
                }
            }
            else if (ctrlObj is ComboBox)
            {
                ComboBox ctrl = ctrlObj as ComboBox;
                iterador = xmlnav.Select("//forms/form[@id=\"" + _form.Name + "\"]/control[@id=\"" + ctrl.Name + "\"]/item");
                if (iterador.Count == 0)
                {
                    return false;
                }
                // Liberar los items anteriores o por defecto
                ctrl.Items.Clear();
                while (iterador.MoveNext())
                    ctrl.Items.Add(iterador.Current.Value);
                ctrl.SelectedIndex = 0;
            }
            else
            {
                Control ctrl = ctrlObj as Control;
                // Cambiar el TEXT si existe en nuestro XML
                iterador = xmlnav.Select("//forms/form[@id=\"" + _form.Name + "\"]/control[@id=\"" + ctrl.Name + "\"]");
                if (iterador.MoveNext())
                {
                    string stSalida = iterador.Current.InnerXml;
                    if (iterador.Current.InnerXml.Contains("<br/>") | iterador.Current.InnerXml.Contains("<br />"))
                    {
                        stSalida = stSalida.Replace("<br/>", Constants.vbCrLf);
                        stSalida = stSalida.Replace("<br />", Constants.vbCrLf);
                    }
                    else
                    {
                        stSalida = iterador.Current.Value;
                    }

                    ctrl.Text = stSalida;
                }

                // Comprobar si el control contiene a otros controles
                if (ctrl.Controls.Count > 0)
                {
                    foreach (Control ctrlAux in ctrl.Controls)
                        traducirControl(ctrlAux, xmldoc, xmlnav);
                }
            }

            return true;
        }

        public string traducirMensaje(TIPO_MENSAJE tipo, string strID)
        {
            XPathDocument xmldoc;
            XPathNavigator xmlnav;
            try
            {
                xmldoc = new XPathDocument(_rutaXML);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encuentra el fichero XML", "Error");
                return "";
            }

            XPathNodeIterator iterador;
            string stTipo = null;
            switch (tipo)
            {
                case TIPO_MENSAJE.M_ERROR:
                    {
                        stTipo = "//errors/error";
                        break;
                    }

                case TIPO_MENSAJE.M_INFO:
                    {
                        stTipo = "//infos/info";
                        break;
                    }

                case TIPO_MENSAJE.M_OTHER:
                    {
                        stTipo = "//others/other";
                        break;
                    }

                case TIPO_MENSAJE.M_TABLE:
                    {
                        stTipo = "//tables/column";
                        break;
                    }

                case TIPO_MENSAJE.M_MONTH:
                    {
                        stTipo = "//months/month";
                        break;
                    }
            }

            if (stTipo != null)
            {
                iterador = xmlnav.Select(stTipo + "[@id=\"" + strID + "\"]");
                if (iterador.MoveNext())
                {
                    string stSalida = iterador.Current.InnerXml;
                    if (iterador.Current.InnerXml.Contains("<br/>") | iterador.Current.InnerXml.Contains("<br />"))
                    {
                        stSalida = stSalida.Replace("<br/>", Constants.vbCrLf);
                        stSalida = stSalida.Replace("<br />", Constants.vbCrLf);
                    }
                    else
                    {
                        stSalida = iterador.Current.Value;
                    }

                    return stSalida;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public bool cambiarIdioma(string ruta)
        {
            string rutaXML;
            var sepStr = new string[] { _appPath + @"\" };
            rutaXML = ruta.Split(sepStr, StringSplitOptions.RemoveEmptyEntries)[0];
            try
            {
                var myXmlDocument = new XmlDocument();
                myXmlDocument.Load(_rutaConf);
                XmlNode node;
                node = myXmlDocument.DocumentElement;

                // Dim node2 As XmlNode
                foreach (XmlNode currentNode in node.ChildNodes)
                {
                    node = currentNode;
                    // Buscar el nodo secundario precio. 
                    // For Each node2 In node.ChildNodes
                    if (node.Name == "idioma")
                    {
                        // 
                        node.InnerText = rutaXML;
                        break;
                    }
                    // Next
                }

                myXmlDocument.Save(_rutaConf);

                // Marcar internamente este cambio
                _rutaXML = ruta;
                // Cambiar el excel
                XPathDocument xmldoc;
                XPathNavigator xmlnav;
                XPathNodeIterator iterador;
                xmldoc = new XPathDocument(_rutaXML);
                xmlnav = xmldoc.CreateNavigator();
                iterador = xmlnav.Select("/language/excelFile");
                if (iterador.MoveNext())
                {
                    _rutaExcel = _appPath + @"\Report\" + iterador.Current.Value;
                    xmlnav = null;
                    xmldoc = null;
                }
                else
                {
                    _rutaExcel = "";
                    MessageBox.Show("Error al intentar cambiar de idioma, el fichero de informe no existe", "Error");
                    xmlnav = null;
                    xmldoc = null;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encuentra el fichero XML." + Constants.vbCrLf + ex.Message.ToString(), "Error");
                return false;
            }
        }

        public bool testFormatXML(string rutaXML, ref string strIdioma, string rutaExcel)
        {
            XPathDocument xmldoc;
            XPathNavigator xmlnav;
            XPathNodeIterator iterador;

            // Set the validation settings.
            var settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            _OK = true;
            settings.ValidationEventHandler += ValidationHandler;

            // Create the XmlReader object.
            var reader = XmlReader.Create(rutaXML, settings);

            // Parse the file. 
            while (reader.Read())
            {
            }

            reader.Close();
            settings = null;
            reader = null;
            if (!_OK)
            {
                MessageBox.Show("El XML no es válido, no tiene el formato correcto", "Error");
                return false;
            }

            // -----------------------------------------
            try
            {
                xmldoc = new XPathDocument(rutaXML);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encuentra el fichero XML", "Error");
                return false;
            }

            iterador = xmlnav.Select("/language/excelFile");
            if (iterador.MoveNext())
            {
                rutaExcel = iterador.Current.Value;
            }
            else
            {
                rutaExcel = "";
                return false;
            }

            iterador = xmlnav.Select("/language/idString");
            if (iterador.MoveNext())
            {
                strIdioma = iterador.Current.Value;
                xmlnav = null;
                xmldoc = null;
                return true;
            }
            else
            {
                strIdioma = "";
                xmlnav = null;
                xmldoc = null;
                return false;
            }
        }

        public string getRutaExcel
        {
            get
            {
                return _rutaExcel;
            }
        }
    }
}