using System;
using global::System.Windows.Forms;
using global::System.Xml;
using global::System.Xml.Schema;
using global::System.Xml.XPath;
using Microsoft.VisualBasic;

namespace MultiLangXML
{
    /// <summary>
    /// Clase encargada de traducir los literales de la aplicación
    /// </summary>
    public class MultiIdiomasXML
    {
        private Form _form;
        private string _appPath;
        private string _rutaConf;
        private string _rutaXML;
        private string _rutaExcel;
        private string _rutaExcelCE;
        private bool _OK;

        /// <summary>
        /// Enumeración con Tipos de Mensaje
        /// </summary>
        public enum TIPO_MENSAJE
        {
            /// <summary>
            /// Mensajes de error
            /// </summary>
            M_ERROR = 0,
            /// <summary>
            /// Mensajes de informacion
            /// </summary>
            M_INFO = 1,
            /// <summary>
            /// Nombres de informes
            /// </summary>
            M_OTHER = 2,
            /// <summary>
            /// Tabla de Informacion
            /// </summary>
            M_TABLE = 3,
            /// <summary>
            /// Nombres de meses
            /// </summary>
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
            
            if (args.Severity == XmlSeverityType.Error)
            {
                Console.WriteLine("Validation has encounted errors.......................");
                Console.Write("Severity:{0}", args.Severity);
                Console.Write("-Message:{0}", args.Message);
                _OK = false;
            }
        }


        /// <summary>
        /// Traducir formulario
        /// </summary>
        /// <param name="rutaXML">Ruta al XML donde se encuentra la traducción</param>
        /// <param name="rutaXSD">Ruta donde se encuentra el XSD de validación del XML</param>
        /// <returns>Devuelve true si la traduccion ha sido correcta y false si ha habido algun error</returns>
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
                MessageBox.Show(ex.Message.ToString(), "Error Traducir\nError Translate", MessageBoxButtons.OK, MessageBoxIcon.Error);              
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
        /// <summary>
        /// Traducir formulario
        /// </summary>
        /// <param name="rutaApp">Ruta a la carpeta donde se encuentra la aplicación</param>
        /// <param name="rutaXML">Ruta al XML donde se encuentra la traducción</param>
        /// <returns>Si todo ha ido bien</returns>

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
            catch (Exception)
            {
                MessageBox.Show("No se encuentra el fichero XML\nXML file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
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

            // Sacar el excel
            xmldoc = new XPathDocument(_rutaXML);
            xmlnav = xmldoc.CreateNavigator();
            iterador = xmlnav.Select("/language/excelCEFile");
            if (iterador.MoveNext())
            {
                _rutaExcelCE = _appPath + @"\Report\" + iterador.Current.Value;
                xmlnav = null;
                xmldoc = null;
            }
            else
            {
                _rutaExcelCE = "";
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

        /// <summary>
        /// Traducir un literal
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="strID"></param>
        /// <returns>Devuelve un string con el mensaje en el idioma que esta seleccionado</returns>
        public string traducirMensaje(TIPO_MENSAJE tipo, string strID)
        {
            XPathDocument xmldoc;
            XPathNavigator xmlnav;
            try
            {
                xmldoc = new XPathDocument(_rutaXML);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception)
            {
                MessageBox.Show("No se encuentra el fichero XML\nXML file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
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

        /// <summary>
        /// Cambiar el idioma
        /// </summary>
        /// <param name="ruta">ruta del xml de la traduccion</param>
        /// <returns>devuelve true si el cambio ha sido correcto y false si ha habido algun error.</returns>
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
                    MessageBox.Show("Error al intentar cambiar de idioma, el fichero no existe\ncChanging language Error, the does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                  
                    xmlnav = null;
                    xmldoc = null;
                    return false;
                }

                xmldoc = new XPathDocument(_rutaXML);
                xmlnav = xmldoc.CreateNavigator();
                iterador = xmlnav.Select("/language/excelCEFile");
                if (iterador.MoveNext())
                {
                    _rutaExcelCE = _appPath + @"\Report\" + iterador.Current.Value;
                    xmlnav = null;
                    xmldoc = null;
                }
                else
                {
                    _rutaExcelCE = "";
                    MessageBox.Show("Error al intentar cambiar de idioma, el fichero no existe\ncChanging language Error, the does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    xmlnav = null;
                    xmldoc = null;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encuentra el fichero XML.\nXML file not found" + Constants.vbCrLf + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("El XML no es válido, no tiene el formato correcto\nThe XML file is not valid, it does not have the correct format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);               
                return false;
            }

            // -----------------------------------------
            try
            {
                xmldoc = new XPathDocument(rutaXML);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception)
            {
                MessageBox.Show("No se encuentra el fichero XML\nXML file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);               
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

            iterador = xmlnav.Select("/language/excelCEFile");
            if (iterador.MoveNext())
            {
                _rutaExcelCE = iterador.Current.Value;
            }
            else
            {
                _rutaExcelCE = "";
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

        /// <summary>
        /// Obtener ruta del excel
        /// </summary>
        public string getRutaExcel
        {
            get
            {
                return _rutaExcel;
            }
        }

        public string getRutaExcelCE
        {
            get { return _rutaExcelCE; }
        }
    }
}