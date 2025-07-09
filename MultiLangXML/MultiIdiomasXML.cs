using global::System.Windows.Forms;
using global::System.Xml;
using global::System.Xml.Schema;
using global::System.Xml.XPath;
using Microsoft.VisualBasic;
using System;

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


        public string RutaXMLConfiguracion
        {
            get { return _rutaConf; }
            private set { _rutaConf = value; }
        }

        private string _rutaExcel;
        private string _rutaExcelCE;
        private string rutaXMLIdioma;
        private bool _OK;
        private static bool _configurado = false;
        private static MultiIdiomasXML _instancia;

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

        public MultiIdiomasXML()
        {

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
        public bool TraducirForm(Form formulario)
        {
            XPathDocument xmldoc;
            XPathNavigator xmlnav;
            try
            {
                xmldoc = new XPathDocument(rutaXMLIdioma);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), traducirMensaje(TIPO_MENSAJE.M_ERROR, "XmlnoEncontrado"), MessageBoxButtons.OK, MessageBoxIcon.Error);              
                return false;
            }

            // Traducir el formulario
            XPathNodeIterator iterador;
            iterador = xmlnav.Select("//forms/form[@id=\"" + formulario.Name + "\"]");
            if (iterador.MoveNext())
            {
                var node = iterador.Current;
                formulario.Text = node.GetAttribute("string", "");
            }
            // Traducir los controles
            foreach (Control ctrl in formulario.Controls)
                TraducirControl(ctrl, xmldoc, xmlnav, formulario.Name);

            xmlnav = null;
            xmldoc = null;
            return true;
        }
       
        public static void ConfigurarTraductor(string rutaApp, string rutaXMLConf)
        {
            XPathDocument xmldoc;
            XPathNavigator xmlnav;
            XPathNodeIterator iterador;
            string rutaLangXML;
            try
            {
                _instancia = new MultiIdiomasXML();
                _instancia._rutaConf = rutaXMLConf;
                _instancia._rutaConf = rutaApp + rutaXMLConf;
                _instancia._appPath = rutaApp;
                xmldoc = new XPathDocument(_instancia._rutaConf);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception)
            {
                MessageBox.Show(_instancia.traducirMensaje(TIPO_MENSAJE.M_ERROR, "XmlnoEncontrado"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            iterador = xmlnav.Select("configuracion/idioma");
            if (iterador.MoveNext())
            {
                rutaLangXML = iterador.Current.Value;
                _instancia.SetFicheroIdioma(_instancia._appPath + @"\" + rutaLangXML);
            }
            else
            {
                xmlnav = null;
                xmldoc = null;
                return;
            }

            // Sacar el excel
            xmldoc = new XPathDocument(_instancia.rutaXMLIdioma);
            xmlnav = xmldoc.CreateNavigator();
            iterador = xmlnav.Select("/language/excelFile");
            if (iterador.MoveNext())
            {
                _instancia._rutaExcel = _instancia._appPath + @"\Report\" + iterador.Current.Value;
                xmlnav = null;
                xmldoc = null;
            }
            else
            {
                _instancia._rutaExcel = "";
                xmlnav = null;
                xmldoc = null;
                return;
            }

            // Obtener ruta excel segun idioma.
            xmldoc = new XPathDocument(_instancia.rutaXMLIdioma);
            xmlnav = xmldoc.CreateNavigator();
            iterador = xmlnav.Select("/language/excelCEFile");
            if (iterador.MoveNext())
            {
                _instancia._rutaExcelCE = _instancia._appPath + @"\Report\" + iterador.Current.Value;
                xmlnav = null;
                xmldoc = null;
            }
            else
            {
                _instancia._rutaExcelCE = "";
                xmlnav = null;
                xmldoc = null;
                return;
            }

            _configurado = true;

        }


        /// <summary>
        /// Traducir un control
        /// </summary>
        /// <param name="ctrlObj">Control a traducir</param>
        /// <param name="xmldoc">XML que se usa para traducir</param>
        /// <param name="xmlnav">El navegador de XML</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool TraducirControl(object ctrlObj, XPathDocument xmldoc, XPathNavigator xmlnav, string nombreFormulario)
        {
            if (ctrlObj == null)
                return false;


            switch (ctrlObj)
            {
                case MenuStrip menu when menu.Items.Count > 0:
                    foreach (ToolStripItem item in menu.Items)
                        TraducirControl(item, xmldoc, xmlnav, nombreFormulario);
                    break;

                case ToolStripMenuItem menuItem:
                    var traduccion = ObtenerTextoPorNombreEtiqueta(menuItem.Name, xmlnav, nombreFormulario);

                    if (!string.IsNullOrEmpty(traduccion)) //Si no encuentra el valor mantiene el valor por defecto.
                        menuItem.Text = traduccion;


                    foreach (ToolStripItem item in menuItem.DropDownItems)
                        TraducirControl(item, xmldoc, xmlnav, nombreFormulario);
                    break;

                case ComboBox comboBox:
                    XPathNodeIterator itemsIterator = xmlnav.Select($"//forms/form[@id='{nombreFormulario}']/control[@id='{comboBox.Name}']/item");

                    if (itemsIterator.Count == 0)
                        return false;

                    comboBox.Items.Clear();
                    while (itemsIterator.MoveNext())
                        comboBox.Items.Add(itemsIterator.Current.Value);

                    comboBox.SelectedIndex = 0;
                    break;

                case DataGridView dgv:
                    foreach (DataGridViewColumn column in dgv.Columns)
                        TraducirControl(column, xmldoc, xmlnav, nombreFormulario);
                    break;

                case Control control:
                    traduccion = ObtenerTextoPorNombreEtiqueta(control.Name, xmlnav, nombreFormulario);
                    
                    if (!string.IsNullOrEmpty(traduccion)) //Si no encuentra el valor mantiene el valor por defecto.
                        control.Text = traduccion;

                    foreach (Control child in control.Controls)
                        TraducirControl(child, xmldoc, xmlnav, nombreFormulario);
                    break;


                case DataGridViewColumn column:
                    column.HeaderText = ObtenerTextoPorNombreEtiqueta(column.Name, xmlnav, nombreFormulario);
                    break;


            }

            return true;
        }




        /// <summary>
        /// Devuelve el texto de la etiqueta recibida en el fichero xml de idioma
        /// </summary>
        /// <param name="nombreEtiqueta">nombre de la etiqueta de la que obtener el texto traducido.</param>
        /// <param name="xmlnav">xml de idioma seleccionado.</param>
        /// <returns></returns>
        public static string ObtenerTextoPorNombreEtiqueta(string nombreEtiqueta, XPathNavigator xmlnav, string nombreForm)
        {
            XPathNodeIterator iterator = xmlnav.Select($"//forms/form[@id='{nombreForm}']/control[@id='{nombreEtiqueta}']");

            if (!iterator.MoveNext())
                return "";

            string content = iterator.Current.InnerXml;
            if (content.Contains("<br/>") || content.Contains("<br />"))
            {
                content = content.Replace("<br/>", Environment.NewLine).Replace("<br />", Environment.NewLine);
            }
            else
            {
                content = iterator.Current.Value;
            }

            return content;
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
                xmldoc = new XPathDocument(_instancia.rutaXMLIdioma);
                xmlnav = xmldoc.CreateNavigator();
            }
            catch (Exception)
            {
                MessageBox.Show(traducirMensaje(TIPO_MENSAJE.M_ERROR, "XmlnoEncontrado"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// <param name="nombreFicheroXml">nombre del xml de la traduccion</param>
        /// <returns>devuelve true si el cambio ha sido correcto y false si ha habido algun error.</returns>
        public bool CambiarIdioma(string nombreFicheroXml)
        {

            var sepStr = new string[] { _appPath + @"\" };
            string idioma = nombreFicheroXml.Split(sepStr, StringSplitOptions.RemoveEmptyEntries)[0];

            try
            {
                var myXmlDocument = new XmlDocument();
                myXmlDocument.Load(_rutaConf);
                XmlNode node;
                node = myXmlDocument.DocumentElement;


                //// Crear instancia del documento XML y cargar el archivo
                // Obtener el nodo <idioma>
                XmlNode idiomaNode = myXmlDocument.SelectSingleNode("configuracion/idioma");

                if (idiomaNode != null)
                {
                    idiomaNode.InnerText = idioma; // nuevo valor
                    myXmlDocument.Save(_rutaConf);
                }
                else
                {
                    Console.WriteLine("Nodo <idioma> no encontrado.");
                }

                _configurado = false;
                _instancia = Instancia;


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(traducirMensaje(TIPO_MENSAJE.M_ERROR, "XmlnoEncontrado"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(traducirMensaje(TIPO_MENSAJE.M_ERROR, "XmlnoValido"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);               
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
                MessageBox.Show(traducirMensaje(TIPO_MENSAJE.M_ERROR, "XmlnoEncontrado"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);               
                return false;
            }

            iterador = xmlnav.Select("/language/excelFile");
            if (!iterador.MoveNext())
            {
                rutaExcel = "";
                return false;
            }

            iterador = xmlnav.Select("/language/excelCEFile");
            if (!iterador.MoveNext())
            {
                _rutaExcelCE = "";
                return false;
            }

            

            iterador = xmlnav.Select("/language/idString");
            if (!iterador.MoveNext())
            { 
                strIdioma = "";
                xmlnav = null;
                xmldoc = null;
                return false;
            }

            return true;
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


        public string GetFicheroConfiguracion
        {
            get { return _rutaConf; }
        }

        public void SetFicheroIdioma(string rutaConfiguracion)
        {
            rutaXMLIdioma = rutaConfiguracion;  
        }


        public static MultiIdiomasXML Instancia
        {
            get
            {
                if (!_configurado)
                {
                    ConfigurarTraductor(Application.StartupPath, @"\conf.xml");
                    return _instancia;
                }

                return _instancia;
            }
        }
    }
}