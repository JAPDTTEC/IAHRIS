using IAHRIS.Calculo;
using IAHRIS.Rellenar;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI.Design.WebControls;
using System.Windows.Forms;

namespace IAHRIS
{
    public partial class FormBienvenida
    {

        /// <summary>
        /// Formulario con la pantalla de bienvenida.
        /// </summary>
        public FormBienvenida()
        {
            CultureInfo culture = new CultureInfo("es-ES");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Console.Title = "IAHRIS " + Application.ProductVersion + " " + new FileInfo(Application.ExecutablePath).LastWriteTime.Year.ToString();
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _cMDB = new IAHRIS.BBDD.OleDbDataBase("Base", Application.StartupPath + @"\IAHRISv2.mdb");
            _tFechas = new TestFechas(new IAHRIS.BBDD.OleDbDataBase("Base", Application.StartupPath + @"\IAHRISv2.mdb"));

            string[] args = Environment.GetCommandLineArgs();


            //Si no hay comandos por consola se ejecuta el modo GUI
            if (args.Length == 1)
            {
                InitializeComponent();
                _pbPrograma.Name = "pbPrograma";
                DateTime fileDate = new FileInfo(Application.ExecutablePath).LastWriteTime;
                _traductor.cambiarIdioma(Application.StartupPath + @"\lang\english.xml");
                lblVersionEN.Text = Application.ProductVersion + "                 " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, fileDate.Month.ToString()) + " " + fileDate.Year.ToString();
                _traductor.cambiarIdioma(Application.StartupPath + @"\lang\spanish.xml");
                lblVersionES.Text = Application.ProductVersion + "                 " + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, fileDate.Month.ToString()) + " " + fileDate.Year.ToString();
            }
            else
            {
                this.Shown += new EventHandler(FormBienvenida_Shown);
            }
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

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

        private void pbAcercaDe_Click(object sender, EventArgs e)
        {
            var fAcerca = new FormAcercaDe();
            fAcerca.ShowDialog();
        }

        public BBDD.OleDbDataBase _cMDB;
        public string _tabla;

        /// <summary>
        /// Estructura con los Datos de CSV
        /// </summary>
        public struct DatosCSV
        {
            /// <summary>
            /// fechas del fichero
            /// </summary>
            public DateTime fechas;

            /// <summary>
            /// Valores del fichero
            /// </summary>
            public float valores;
        }

        /// <summary>
        /// Estructura con la cabecera del CSV
        /// </summary>
        public struct CabeceraCSV
        {
            public bool TipoFechas;
            public ComboItem Clave_Punto;
            public bool IsNatural;
            public ComboItem Clave_Alteracion;
            public string FilePoint;
            public string FormatoFecha;
            public string FileAlt { get; internal set; }
        }

        public CabeceraCSV _cabecera;
        public TestFechas _tFechas;
        public readonly MultiLangXML.MultiIdiomasXML _traductor;


        /// <summary>
        /// Se ejecuta la primera vez que el formulario se muestra y contiene los metodos para ejecutar por consola.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormBienvenida_Shown(Object sender, EventArgs e)
        {
            try
            {

                //Ocultar el formulario para mostrar solo la consola
                Hide();

                //Obtiene los argumentos introducidos por consola
                string[] args = Environment.GetCommandLineArgs();
                
                foreach (var item in args)
                {
                    Console.WriteLine(item);
                }


                Dictionary<string, string> parametros = new Dictionary<string, string>();

                List<string> argsList = args.ToList();

                argsList.RemoveAt(0);
                parametros.Add(argsList[0], "");
                argsList.RemoveAt(0);

                foreach (string arg in argsList)
                {
                    if (arg.StartsWith("/"))
                    {
                        string[] parametro = arg.Split(':');
                        if (parametro.Length == 3 && parametro[1].Length == 1 && parametro[2].StartsWith("\\"))
                        {
                            parametros.Add(parametro[0], parametro[1] + ":" + parametro[2]);
                        }
                        else if (parametro.Length != 2)
                        {
                            throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorParamCmd"));
                        }
                        else
                        {
                            parametros.Add(parametro[0], parametro[1]);
                        }


                    }
                    else if (arg.StartsWith("-"))
                    {
                        parametros.Add(arg, "");
                    }
                    else
                    {
                        throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorParamCmd"));
                    }
                }

                string nombrePunto;
                string nombreAlteracion;
                string descripcion;
                string nombreProyecto;
                string descripcionProyecto;              
                string nombreRegimen;
                string abrevRegimen;
                string csv;
                string exportacion;
                bool cvh;
                bool cas;
                string cm;
                string pa;
                string mesInicio;

                InterpretacionComandos.LecturaComandos(parametros,out nombrePunto,out nombreAlteracion,out descripcion,out nombreProyecto,
                    out descripcionProyecto,out nombreRegimen, out abrevRegimen, out csv, out exportacion, out cvh, out cas, out cm, out pa, out mesInicio);
                
                Automatizacion auto = new Automatizacion();

                auto._traductor = _traductor;
                auto._cMDB = _cMDB;
                auto._tFechas = _tFechas;

                auto.SeleccionProceso(nombrePunto, nombreAlteracion, descripcion, nombreProyecto, descripcionProyecto, nombreRegimen, abrevRegimen,
                csv, exportacion, cvh, cas, cm, pa, mesInicio);

                Dispose();


                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error. " + ex.Message);
                string[] args = Environment.GetCommandLineArgs();
                foreach (var item in args)
                {
                    Console.WriteLine(item);
                }

                
                Dispose();
            }
            
            /*
            try
            {


                if (args[1] == "CM")
                {
                    //          0    1  2       3           4              5             6               7                 8                9        10
                    //    punto +++ CM Pun Clave_Punto Descripcion Nombre_Proyecto Nombre_Régimen Abreviatura_Regimen Ruta_Ficherocsv Ruta_Informe (-s/n)
                    // Añade un punto por linea de comandos, le carga un csv y exporta el informe a la ruta indicada.

                    string[] s;
                    string[] s2;
                   

                    if (args[2] == "Pun") {
                        //Comprobamos si el numero de argumentos es correcto
                        if (args.Length != 11)
                        {
                            throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                        }

                        string[] aux = args[8].Split('\\');
                        string datosCsv = aux[aux.Length - 1];
                       

                        Console.WriteLine("Comenzando el procesado del comando.");
                        Console.WriteLine("Punto:" + args[3] + "\nProyecto:" + args[5] +"\nDatosCSV:"+datosCsv+"\n");
                        
                        //Comprobamos si existe el proyecto y si no lo creamos 
                        if (!ExisteProyecto(args[5]))
                        {
                            
                            if (args[5].Length > 20)
                            {
                                throw new Exception("El nombre del proyecto es demasiado largo");
                            }

                            var valores = new[] { args[5], "Proyecto generado con la herramienta de carga masiva", };
                            var campos = new[] { "nombre", "descripcion" };
                            _cMDB.InsertarRegistro("Proyecto", campos, valores);
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedProject")); 
                        }

                       
                        //Buscamos el ID del proyecto para añadirle el punto.
                        string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + args[5] + "'";
                        var ds = _cMDB.RellenarDataSet("Proyecto", str);

                        string idProyecto="";

                        
                        //TODO: No es necesario el foreach por que la ID es unica. SIMPLIFICAR
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {                            
                            idProyecto = item[0].ToString();
                        }


                        _tabla = "Punto";
                        s = new string[6];
                        s[0] = "Clave_punto";
                        s[1] = "nombre";
                        s[2] = "ID_proyecto";
                        s[3] = "mesInicio";
                        s[4] = "nombreRegimen";
                        s[5] = "abreviaturaRegimen";

                        string[] t = new[] { args[3].ToUpperInvariant(), args[4], idProyecto, "10", args[6], args[7] };

                        //Comprobamos que el codigo del punto no existe ya y si no lo creamos

                        if (Conversions.ToBoolean(ExisteCodigo(_tabla, t[0], Convert.ToInt32(t[2]))))
                        {
                            if(_cMDB.InsertarRegistro(_tabla, s, t))
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedPoint"));
                            }
                            else
                            {
                                Console.WriteLine("No se ha podido crear el punto");
                            }
                        }
                        
                        //Seleccionamos el ID_Punto del punto para cargarle el csv
                        str = "SELECT ID_Punto FROM [Punto] WHERE Clave_Punto='" + args[3].ToUpperInvariant()+"'";

                        ds = _cMDB.RellenarDataSet("Punto", str);
                        int idPunto = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            idPunto = Convert.ToInt32(item[0]);
                        }

                         
                        //Cargamos el csv
                        ComboItem punto = new ComboItem(".", idPunto);

                        LeerCSV(args[8], punto);
                        AnadirCSV(args[8], punto);                       
                        

                        //Si no aportamos ruta de exportación no hacemos nada.
                        if (args[9]!="")
                        {
                            var fInicio = new FormInicial();
                            fInicio.Show();

                            //Comprobamos si existe el directorio donde exportamos el informe
                            if (Directory.Exists(args[9]))
                            {
                                //Comprobamos si el parametro de si o no sobreescribir es correcto.
                                if (args[10] == "-s" || args[10] == "-n")
                                {
                                    int index1;
                                    int indexPro;


                                    indexPro = fInicio.cbProyectos.FindStringExact(args[5]);
                                    if (indexPro != -1)
                                    {
                                        fInicio.cbProyectos.SelectedIndex = indexPro;

                                        index1 = fInicio.lstboxPuntos.FindStringExact(args[3]);
                                        if (index1 != -1)
                                        {
                                            fInicio.lstboxPuntos.SetSelected(index1, true);
                                            if (fInicio.btnCalcular.Enabled) { fInicio.btnCalcular.PerformClick(); }
                                            else { Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed")); }


                                            fInicio.Close();
                                            Dispose();
                                        }
                                        else
                                        {
                                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointHasntBeenFound"));
                                            fInicio.Close();
                                            Dispose();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                                        fInicio.Close();
                                        Dispose();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorOverwriteParam"));
                                    fInicio.Close();
                                    Dispose();
                                }
                            }
                            else
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPathCmd"));
                                fInicio.Close();
                                Dispose();
                            }
                        }
                        else
                        {
                            Dispose();

                        }
                        Console.WriteLine("Finalizado el procesado del comando.");
                        Console.WriteLine("****************************************************");
                    }
                    //               0   1   2       3           4               5              6               7                    8                      9                      10               11                      12                13        14       15       16
                    //   alteracion +++ CM Alt COD_Alteracion Descripcion Nombre_Proyecto Nombre_Punto Nombre_RégimenPunto Abreviatura_RegimenPunto Nombre_RégimenAlter Abreviatura_RegimenAlter Ruta_ficherocsvpunto Ruta_ficherocsvalt rutainformes (-s/-n) CoeValHab CoeAvSq
                    // Añade un punto y una alteracion asociada por linea de comandos. le carga un csv y exporta el informe a la ruta indicada
                    else if (args[2] == "Alt")
                    {

                        //Comprobamos si el numero de argumentos es correcto
                        if (args.Length != 17)
                        {
                            throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                        }

                        string[] aux = args[11].Split('\\');
                        string datosCsvPunto = aux[aux.Length - 1];

                        string[] aux2 = args[12].Split('\\');
                        string datosCsvAlteracion = aux2[aux2.Length - 1];

                        

                        Console.WriteLine("Comenzando el procesado del comando.");
                        Console.WriteLine("Punto:" + args[6] + "\nAlteración:" + args[3] + "\nProyecto:" + args[5] + "\nDatosCSVPunto:" + datosCsvPunto + "\nDatosCSVAlteración:" + datosCsvAlteracion+"\n");



                        //Comprobamos si existe el proyecto y si no lo creamos
                        if (!ExisteProyecto(args[5]))
                        {
                            if (args[5].Length > 20)
                            {
                                throw new Exception("El nombre del proyecto es demasiado largo");
                            }

                            var valores = new[] { args[5], "Proyecto generado autom.", };
                            var campos = new[] { "nombre", "descripcion" };
                            if (_cMDB.InsertarRegistro("Proyecto", campos, valores)) { 
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedProject"));
                            }
                            else
                            {
                                Console.WriteLine("No se ha podido crear el proyecto.");
                            }
                            
                        }
                        else
                        {
                            
                        }

                        //Buscamos la id del proyecto para asignarle el punto y la alteracion
                        string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + args[5] + "'";
                        var ds = _cMDB.RellenarDataSet("Proyecto", str);

                        string idProyecto = "";

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            idProyecto = item[0].ToString();
                        }
                        
                        _tabla = "Punto";
                        s = new string[6];
                        s[0] = "Clave_punto";
                        s[1] = "nombre";
                        s[2] = "ID_proyecto";
                        s[3] = "mesInicio";
                        s[4] = "nombreRegimen";
                        s[5] = "abreviaturaRegimen";

                        string[] t = new[] { args[6].ToUpperInvariant(), "PCM.", idProyecto, "10", args[7], args[8] };
                       
                        //Comprobamos si existe el punto y si no lo creamos.
                        if (Conversions.ToBoolean(ExisteCodigo(_tabla, t[0], Convert.ToInt32(t[2]))))
                        {
                            if(_cMDB.InsertarRegistro(_tabla, s, t))
                            {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedPoint"));
                            }
                            else
                            {
                                Console.WriteLine("No se ha podido crear el punto");
                            }
                            
                        }

                        //Buscamos el id del punto para asignarle la alteracion y cargarle el csv si es necesario

                        str = "SELECT ID_Punto FROM [Punto] WHERE Clave_punto='" + args[6].ToUpperInvariant() + "'";

                        ds = _cMDB.RellenarDataSet("Punto", str);
                        int idPunto = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            idPunto = Convert.ToInt32(item[0]);
                        }
                        
                        _tabla = "Alteracion";

                        s2 = new string[5];
                        s2[0] = "COD_Alteracion";
                        s2[1] = "nombre";
                        s2[2] = "ID_Punto";
                        s2[3] = "nombreRegimen";
                        s2[4] = "abreviaturaRegimen";

                        string[] t2 = new[] { args[3].ToUpperInvariant(), args[4], idPunto.ToString(), args[9], args[10] };

                        //Comprobamos si existe la alteración y si no lo creamos.
                        if (Conversions.ToBoolean(ExisteCodigo(_tabla, t[0], Convert.ToInt32(t2[2]))))
                        {
                            if(_cMDB.InsertarRegistro(_tabla, s2, t2))
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedAlt"));
                            }
                            else
                            {
                                Console.WriteLine("No se ha podido crear la alteración.");
                            }
                        }

                        //Buscamos el id de la alteracion para cargarle el csv
                        str = "SELECT ID_Alteracion FROM [Alteracion] WHERE COD_Alteracion='" + args[3].ToUpperInvariant() + "' AND ID_Punto=" + idPunto;

                        ds = _cMDB.RellenarDataSet("Alteracion", str);
                        int idAlteracion = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            idAlteracion = Convert.ToInt32(item[0]);
                        }
                        
                        ComboItem punto = new ComboItem(".", idPunto);
                        
                        //Si nos han dado un csv para el punto se lo cargamos, si no solo a la alteracion
                        if (args[11] != "") { 
                            LeerCSV(args[11], punto);
                            AnadirCSV(args[11], punto);
                        }
                        ComboItem alteracion = new ComboItem(".", idAlteracion);
                        
                        LeerCSV(args[12], alteracion);
                        AnadirCSV(args[12], punto, alteracion);                  
                                               

                        //Si no nos pasan ruta de exportación no hacemos nada.
                        if (args[13]!="")
                        {
                            var fInicio = new FormInicial();
                            fInicio.Show();

                            //Comprobamos si el directorio donde exportar el informe existe
                            if (Directory.Exists(args[13]))
                            {
                                //Comprobamos que el parametro de sobreescribir el csv sea correcto.
                                if (args[14] == "-s" || args[14] == "-n")
                                {
                                    int index1;
                                    int indexPro;

                                    indexPro = fInicio.cbProyectos.FindStringExact(args[5]);
                                    if (indexPro != -1)
                                    {
                                        fInicio.cbProyectos.SelectedIndex = indexPro;

                                        index1 = fInicio.lstboxPuntos.FindStringExact(args[6]);
                                        if (index1 != -1)
                                        {

                                            fInicio.lstboxPuntos.SetSelected(index1, true);

                                            int index2 = fInicio.cmbListaAlteradasDiarias.FindStringExact(args[3]);

                                            if (index2 != -1)
                                            {
                                                fInicio.cmbListaAlteradasDiarias.SelectedIndex = index2;
                                                if (args[15] == "0")
                                                {
                                                    if (fInicio.chkboxUsarCoe.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoe.Checked = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_1"));
                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoe.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_2") + chckd);
                                                    }

                                                }
                                                else if (args[15] == "1")
                                                {
                                                    if (fInicio.chkboxUsarCoe.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoe.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_1"));

                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoe.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_2") + chckd);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCoetaneity_1"));
                                                    fInicio.Close();
                                                    Dispose();
                                                }
                                                
                                                if (args[16] == "0")
                                                {
                                                    if (fInicio.chkboxUsarCoeDiaria.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoeDiaria.Checked = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_3"));
                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoeDiaria.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_4") + chckd);
                                                    }

                                                }
                                                else if (args[16] == "1")
                                                {
                                                    if (fInicio.chkboxUsarCoeDiaria.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoeDiaria.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_3"));
                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoeDiaria.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_4") + chckd);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCoetaneity_2"));
                                                    fInicio.Close();
                                                    Dispose();
                                                }
                                                

                                                Console.WriteLine(fInicio.btnCalcular.Text);
                                                if (fInicio.btnCalcular.Enabled) { fInicio.btnCalcular.PerformClick(); }
                                                else { Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed")); }

                                                fInicio.Close();
                                                Dispose();
                                            }
                                            else
                                            {
                                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltHasntBeenFound"));
                                                fInicio.Close();
                                                Dispose();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                                        fInicio.Close();
                                        Dispose();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorOverwriteParam"));
                                    fInicio.Close();
                                    Dispose();
                                }
                            }
                            else
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPathCmd"));
                                fInicio.Close();
                                Dispose();
                            }
                        }
                        else
                        {
                            Dispose();
                            
                        }
                        Console.WriteLine("Finalizado el procesado del comando.");
                        Console.WriteLine("****************************************************");

                    }                
                }

                
                // Añadir Puntos/Alteraciones/Proyectos
                else if (args[1] == "A")
                {
                    string[] s;

                    //Para añadir Alteraciones
                    if (args[2] == "Alt")
                    {
                        if (args.Length != 8)
                        {
                            throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                        }

                        _tabla = "Alteracion";

                        s = new string[5];
                        
                        s[0] = "COD_Alteracion";
                        s[1] = "nombre";
                        s[2] = "ID_Punto";
                        s[3] = "nombreRegimen";
                        s[4] = "abreviaturaRegimen";


                        string str = "SELECT ID_Punto FROM [Punto] WHERE Clave_punto='" + args[5].ToUpperInvariant() + "'";

                        DataSet ds = _cMDB.RellenarDataSet("Punto", str);
                        int idPunto = 0;
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            idPunto = Convert.ToInt32(item[0]);
                        }



                        string[] t = new[] { args[3].ToUpperInvariant(), args[4], idPunto.ToString(), args[6], args[7] };

                        if (Conversions.ToBoolean(ExisteCodigo(_tabla, t[0], Convert.ToInt32(t[2]))))
                        {

                            _cMDB.InsertarRegistro(_tabla, s, t);
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedAlt"));
                        }
                    }

                    // Para añadir Puntos
                    else if (args[2] == "Pun")
                    {
                        if (args.Length != 8)
                        {
                            throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                        }

                        string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + args[5] + "'";
                        var ds = _cMDB.RellenarDataSet("Proyecto", str);

                        string idProyecto = "";

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            idProyecto = item[0].ToString();
                        }



                        _tabla = "Punto";
                        s = new string[6];
                        s[0] = "Clave_punto";
                        s[1] = "nombre";
                        s[2] = "ID_proyecto";
                        s[3] = "mesInicio";
                        s[4] = "nombreRegimen";
                        s[5] = "abreviaturaRegimen";

                        string[] t = new[] { args[3].ToUpperInvariant(), args[4], idProyecto, "10", args[6], args[7] };

                        if (Conversions.ToBoolean(ExisteCodigo(_tabla, t[0], Convert.ToInt32(t[2]))))
                        {
                            _cMDB.InsertarRegistro(_tabla, s, t);
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedPoint"));
                        }
                    }

                    //Para añadir Proyectos
                    else if (args[2] == "Pro")
                    {
                        if (args.Length != 5)
                        {
                            throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                        }
                        else if (args[3].Length > 20)
                        {
                            throw new Exception("El nombre del proyecto es demasiado largo");
                        }

                        var valores = new[] { args[3], args[4], };
                        var campos = new[] { "nombre", "descripcion" };

                        if (!ExisteProyecto(valores[0]))
                        {
                            _cMDB.InsertarRegistro("Proyecto", campos, valores);
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedProject"));
                        }
                        else
                        {
                            throw new Exception("Ya existe un proyecto con ese nombre.");
                        }
                    }
                    else
                    {

                        throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArguments"));
                    }
                    Dispose();

                }
                //Mostrar datos de proyectos
                else if (args[1] == "MP")
                {
                    if (args.Length != 2)
                    {
                        throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                    }

                    var ds = _cMDB.RellenarDataSet("Proyectos", "SELECT nombre, descripcion FROM [Proyecto] ORDER BY nombre, ID_Proyecto ASC");

                    DibujarTablas.tableWidth = 60;
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProyectsList"));
                    DibujarTablas.PrintLineInicio(2);

                    string[] titulo = { "Nombre", "Descripción" };
                    DibujarTablas.PrintRow(titulo);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        DibujarTablas.PrintLine(2);
                        string[] fila = { item[0].ToString(), item[1].ToString() };
                        DibujarTablas.PrintRow(fila);

                    }
                    DibujarTablas.PrintLineFinal(2);
                    Dispose();

                }
                //Mostar datos de puntos
                else if (args[1] == "MCP")
                {
                    if (args.Length != 2 && args.Length != 3)
                    {
                        throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                    }

                    DataSet ds;
                    if (args.Length == 2)
                    {
                        ds = _cMDB.RellenarDataSet("Puntos", "SELECT ID_punto, Clave_punto, Nombre, ID_proyecto, nombreRegimen, abreviaturaRegimen FROM [Punto] ORDER BY nombre, ID_punto ASC");
                    }
                    else
                    {
                        string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + args[2] + "'";
                        DataSet ds2 = _cMDB.RellenarDataSet("Proyecto", str);

                        string idProyecto = "";

                        foreach (DataRow item in ds2.Tables[0].Rows)
                        {
                            idProyecto = item[0].ToString();
                        }




                        ds = _cMDB.RellenarDataSet("Puntos", "SELECT ID_punto, Clave_punto, Nombre, ID_proyecto, nombreRegimen, abreviaturaRegimen FROM [Punto] WHERE ID_proyecto =" + idProyecto +" ORDER BY nombre, ID_punto ASC");
                    }

                    DibujarTablas.tableWidth = 104;
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointsList"));
                    DibujarTablas.PrintLineInicio(6);



                    string[] titulo = { "", "Nombre Punto", "Descripción", "Nombre Proyecto", "Nombre Régimen", "Abrev Régimen" };
                    DibujarTablas.PrintRow(titulo);
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        string str3 = "SELECT nombre FROM [Proyecto] WHERE ID_Proyecto=" + item[3];
                        DataSet ds3 = _cMDB.RellenarDataSet("Proyecto", str3);

                        string nombreProyecto = "";

                        foreach (DataRow item3 in ds3.Tables[0].Rows)
                        {
                            nombreProyecto = item3[0].ToString();
                        }

                        DibujarTablas.PrintLine(6);
                        string[] fila = { i.ToString(), item[1].ToString(), item[2].ToString(), nombreProyecto, item[4].ToString(), item[5].ToString() };
                        DibujarTablas.PrintRow(fila);
                        i++;
                    }
                    DibujarTablas.PrintLineFinal(6);

                    Dispose();
                }
                //Mostrar datos de alteraciones
                else if (args[1] == "MA")
                {
                    if (args.Length != 2 && args.Length != 3)
                    {
                        throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                    }

                    DataSet ds;
                    if (args.Length == 2)
                    {
                        ds = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion, COD_Alteracion, Nombre, ID_Punto, nombreRegimen, abreviaturaRegimen FROM [Alteracion] ORDER BY Nombre, COD_Alteracion ASC");
                    }
                    else
                    {

                        string str = "SELECT ID_Punto FROM [Punto] WHERE Clave_punto='" + args[2].ToUpperInvariant() + "'";

                        DataSet ds2 = _cMDB.RellenarDataSet("Punto", str);
                        int idPunto = 0;
                        foreach (DataRow item in ds2.Tables[0].Rows)
                        {
                            idPunto = Convert.ToInt32(item[0]);
                        }

                        ds = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion, COD_Alteracion, Nombre, ID_Punto, nombreRegimen, abreviaturaRegimen FROM [Alteracion] WHERE ID_Punto = " + idPunto + " ORDER BY Nombre, COD_Alteracion ASC");

                    }

                    DibujarTablas.tableWidth = 104;
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltList"));
                    DibujarTablas.PrintLineInicio(6);

                    string[] titulo = { "", "Nombre Alteración", "Descripción", "Nombre Punto", "Nombre Régimen", "Abrev Régimen" };
                    DibujarTablas.PrintRow(titulo);

                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        string str3 = "SELECT Clave_punto FROM [Punto] WHERE ID_Punto=" + item[3];

                        DataSet ds3 = _cMDB.RellenarDataSet("Punto", str3);
                        string nombrePunto = "";
                        foreach (DataRow item3 in ds3.Tables[0].Rows)
                        {
                            nombrePunto = item3[0].ToString();
                        }






                        DibujarTablas.PrintLine(6);
                        string[] fila = { i.ToString(), item[1].ToString(), item[2].ToString(), nombrePunto, item[4].ToString(), item[5].ToString() };
                        DibujarTablas.PrintRow(fila);
                        i++;
                    }
                    DibujarTablas.PrintLineFinal(6);

                    Dispose();
                }
                //Añadir CSV
                else if (args[1] == "AC")
                {
                    if (args.Length < 4 || args.Length > 5)
                    {
                        throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                    }

                    string str = "SELECT ID_Punto FROM [Punto] WHERE Clave_punto='" + args[3].ToUpperInvariant() + "'";

                    DataSet ds = _cMDB.RellenarDataSet("Punto", str);
                    int idPunto = 0;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        idPunto = Convert.ToInt32(item[0]);
                    }



                    ComboItem punto = new ComboItem(".", idPunto);

                    if (args.Length == 4)
                    {
                        LeerCSV(args[2], punto);
                        AnadirCSV(args[2], punto);
                    }
                    if (args.Length == 5)
                    {
                        string str2 = "SELECT ID_Alteracion FROM [Alteracion] WHERE COD_Alteracion='" + args[4].ToUpperInvariant() + "'";

                        DataSet ds2 = _cMDB.RellenarDataSet("Alteracion", str2);
                        int idAlteracion = 0;
                        foreach (DataRow item in ds2.Tables[0].Rows)
                        {
                            idAlteracion = Convert.ToInt32(item[0]);
                        }



                        ComboItem alt = new ComboItem(".", idAlteracion);
                        LeerCSV(args[2], punto);
                        AnadirCSV(args[2], punto, alt);
                    }
                    Dispose();

                }
                //Exportar Excel
                else if (args[1] == "E")
                {
                    if (args.Length != 6 && args.Length != 9)
                    {
                        throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                    }
                    else
                    {
                        var fInicio = new FormInicial();
                        fInicio.Show();
                        if (Directory.Exists(args[3]))
                        {
                            if (args[2] == "-s" || args[2] == "-n")
                            {
                                int index1;
                                int indexPro;
                                if (args.Length < 6 || (args.Length > 6 && args.Length < 9) || args.Length > 9)
                                {
                                    throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strWrongArgumentsNumber"));
                                }
                                else if (args.Length == 6)
                                {

                                    indexPro = fInicio.cbProyectos.FindStringExact(args[4]);
                                    if (indexPro != -1)
                                    {
                                        fInicio.cbProyectos.SelectedIndex = indexPro;

                                        index1 = fInicio.lstboxPuntos.FindStringExact(args[5]);
                                        if (index1 != -1)
                                        {
                                            fInicio.lstboxPuntos.SetSelected(index1, true);
                                            if (fInicio.btnCalcular.Enabled) { fInicio.btnCalcular.PerformClick(); }
                                            else { Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed")); }


                                            fInicio.Close();
                                            Dispose();
                                        }
                                        else
                                        {
                                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointHasntBeenFound"));
                                            fInicio.Close();
                                            Dispose();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                                        fInicio.Close();
                                        Dispose();
                                    }
                                }
                                else if (args.Length == 9)
                                {
                                    indexPro = fInicio.cbProyectos.FindStringExact(args[4]);
                                    if (indexPro != -1)
                                    {
                                        fInicio.cbProyectos.SelectedIndex = indexPro;

                                        index1 = fInicio.lstboxPuntos.FindStringExact(args[5]);
                                        if (index1 != -1)
                                        {

                                            fInicio.lstboxPuntos.SetSelected(index1, true);

                                            int index2 = fInicio.cmbListaAlteradasDiarias.FindStringExact(args[6]);

                                            if (index2 != -1)
                                            {
                                                fInicio.cmbListaAlteradasDiarias.SelectedIndex = index2;
                                                if (args[7] == "0")
                                                {
                                                    if (fInicio.chkboxUsarCoe.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoe.Checked = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_1"));
                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoe.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_2") + chckd);
                                                    }

                                                }
                                                else if (args[7] == "1")
                                                {
                                                    if (fInicio.chkboxUsarCoe.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoe.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_1"));

                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoe.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_2") + chckd);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCoetaneity_1"));
                                                    fInicio.Close();
                                                    Dispose();
                                                }

                                                if (args[8] == "0")
                                                {
                                                    if (fInicio.chkboxUsarCoeDiaria.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoeDiaria.Checked = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_3"));
                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoeDiaria.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_4") + chckd);
                                                    }

                                                }
                                                else if (args[8] == "1")
                                                {
                                                    if (fInicio.chkboxUsarCoeDiaria.Enabled == true)
                                                    {
                                                        fInicio.chkboxUsarCoeDiaria.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_3"));
                                                        string chckd;
                                                        if (fInicio.chkboxUsarCoeDiaria.Checked == true) { chckd = "SI"; }
                                                        else { chckd = "NO"; }

                                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCantChooseCoetaneity_4") + chckd);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCoetaneity_2"));
                                                    fInicio.Close();
                                                    Dispose();
                                                }


                                                Console.WriteLine(fInicio.btnCalcular.Text);
                                                if (fInicio.btnCalcular.Enabled) { fInicio.btnCalcular.PerformClick(); }
                                                else { Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed")); }

                                                fInicio.Close();
                                                Dispose();
                                            }
                                            else
                                            {
                                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltHasntBeenFound"));
                                                fInicio.Close();
                                                Dispose();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                                        fInicio.Close();
                                        Dispose();
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorOverwriteParam"));
                                fInicio.Close();
                                Dispose();
                            }
                        }
                        else
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPathCmd"));
                            fInicio.Close();
                            Dispose();
                        }
                    }

                }
                //Ayuda
                else if (args[1] == "--help" || args[1] == "--HELP" || args[1] == "help" || args[1] == "HELP" || args[1] == "-H" ||
                    args[1] == "H" || args[1] == "h" || args[1] == "-h" || args[1] == "?" || args[1] == "-?")
                {

                    //para conseguir año y mes y centrarlo.
                    DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;

                    string nombreMes = formatoFecha.GetMonthName(new FileInfo(Application.ExecutablePath).LastWriteTime.Month);
                    string version = "Versión " + Application.ProductVersion + " " + nombreMes.ToUpper() + " " +
                        new FileInfo(Application.ExecutablePath).LastWriteTime.Year.ToString();
                    string lado = String.Concat(Enumerable.Repeat(" ", (113 - version.Length) / 2));
                    string linea = "║" + lado + version + lado;
                    string final = String.Concat(Enumerable.Repeat(" ", 115 - linea.Length - 1));
                    string lineaVersion = linea + final + "║\r\n";



                    //Mensaje
                    Console.WriteLine("╔═════════════════════════════════════ÍNDICES DE ALTERACIÓN HIDROLÓGICA EN RÍOS═══════════════════════════════════╗\r\n" + lineaVersion +
                                      "║          _____          _    _ _____  _____  _____    _____ __  __ _____    _    _ ______ _      _____          ║\r\n" +
                                      "║         |_   _|   /\\   | |  | |  __ \\|_   _|/ ____|  / ____|  \\/  |  __ \\  | |  | |  ____| |    |  __ \\         ║\r\n" +
                                      "║           | |    /  \\  | |__| | |__) | | | | (___   | |    | \\  / | |  | | | |__| | |__  | |    | |__) |        ║\r\n" +
                                      "║           | |   / /\\ \\ |  __  |  _  /  | |  \\___ \\  | |    | |\\/| | |  | | |  __  |  __| | |    |  ___/         ║\r\n" +
                                      "║          _| |_ / ____ \\| |  | | | \\ \\ _| |_ ____) | | |____| |  | | |__| | | |  | | |____| |____| |             ║\r\n" +
                                      "║         |_____/_/    \\_\\_|  |_|_|  \\_\\_____|_____/   \\_____|_|  |_|_____/  |_|  |_|______|______|_|             ║\r\n" +
                                      "║                                                                                                                 ║\r\n" +
                                      "║                                                                                                                 ║");
                    Console.WriteLine("╠═════╦═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║ A   ║ AÑADIR PUNTOS,ALTERACIONES O PROYECTOS                                                                    ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║     ║ Formato añadir punto: A Pun Clave_Punto Descripcion Nombre_proyecto Nombre_Régimen Abreviatura_Regimen    ║");
                    Console.WriteLine("║     ║ Formato añadir alteración: A Alt COD_Altera Descripcion Nombre_Punto Nombre_Régimen Abreviatura_Regimen   ║");
                    Console.WriteLine("║     ║ Formato añadir proyecto: A Pro nombre descripción                                                         ║");
                    Console.WriteLine("║     ║ Utilice MP para consultar los ID de Proyecto, MCP para consultar los ID de los Puntos                     ║");
                    Console.WriteLine("║     ║         y MA para consultar los ID de las Alteraciones.                                                   ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║ MP  ║ MOSTRAR DATOS DE LOS PROYECTOS                                                                            ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║ MCP ║ MOSTRAR DATOS DE LOS PUNTOS                                                                               ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║     ║ Si se escribe solo muestra todos los puntos, si se escribe seguido del nombre de un                       ║");
                    Console.WriteLine("║     ║ Proyecto se muestran solo los puntos pertenecientes a un proyecto.                                        ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║ MA  ║ MOSTRAR DATOS DE LAS ALTERACIONES                                                                         ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║     ║ Si se escribe solo muestra todas las alteraciones, si se escribe seguido del nombre de un                 ║");
                    Console.WriteLine("║     ║ Punto se muestran solo las alteraciones pertenecientes a un punto.                                        ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║ AC  ║ AÑADIR CSV                                                                                                ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║     ║ La ruta ha de escribirse entre comillas                                                                   ║");
                    Console.WriteLine("║     ║ Formato añadir csv a un punto: AC Ruta_csv nombre_punto                                                   ║");
                    Console.WriteLine("║     ║ Formato añadir csv a una alteracion: AC Ruta_csv nombre_punto nombre_alteracion                           ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║ E   ║ EXPORTAR EXCEL                                                                                            ║");
                    Console.WriteLine("╠═════╬═══════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine("║     ║ Las rutas y codigos han de escribirse entre comillas                                                      ║");
                    Console.WriteLine("║     ║ Formato guardar excel de un punto:                                                                        ║");
                    Console.WriteLine("║     ║         E (-s/-n) ruta_archivo nombre_proyecto nombre_punto                                               ║");
                    Console.WriteLine("║     ║ Formato guardar excel de una alteracion:                                                                  ║");
                    Console.WriteLine("║     ║ E (-s/-n) ruta_archivo nombre_proyecto nombre_punto nombre_alteracion CoeValHab CoeAvSq                   ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("║     ║     -s para sobreescribir si ya existe el archivo, -n para no sobreescribir                               ║");
                    Console.WriteLine("║     ║     CoeValHab: Coetaneidad Valores Habituales          0:NO 1:SI                                          ║");
                    Console.WriteLine("║     ║     CoeAvSq: Coetaneidad Avenidas y Sequias            0:NO 1:SI                                          ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("║     ║     Ej: E -s \"C:\\Nueva carpeta\" \"Ejemplos\" \"EJEMPLO1\" \"EJEM1_ALTERA\" 1 0                                  ║");
                    Console.WriteLine("║     ║                                                                                                           ║");
                    Console.WriteLine("╚═════╩═══════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
                    Console.WriteLine();

                    Dispose();
                }
                //Comandos no validos
                else if (args[1]== "P"){
                    //SIMPA.Main();
                    
                }
                else
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorParamCmd"));
                    Dispose();
                }
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error. " + ex.Message);
                Dispose();
            }
            */
        }

        private void FormBienvenida_Load(object sender, EventArgs e)
        {
            pbMITECO.BackColor = Color.FromArgb(234, 200, 30);

        }
    }
}