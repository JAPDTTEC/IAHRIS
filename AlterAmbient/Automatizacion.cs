using IAHRIS.Rellenar;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTable.Models;
using IAHRIS.Calculo;
using static IAHRIS.FormBienvenida;

namespace IAHRIS
{
    /// <summary>
    /// Clase encargada de cargar y exportar datos desde iahris a traves de parámetros de texto.
    /// </summary>
    public class Automatizacion
    {
        public MultiLangXML.MultiIdiomasXML _traductor;
        public BBDD.OleDbDataBase _cMDB;
        string _tabla;

        

        CabeceraCSV _cabecera;
        public TestFechas _tFechas;
        
        /// <summary>
        /// Struct de los datos formateados(fecha y caudal) que se extraen del csv.
        /// </summary>
        public struct DatosCSV
        {
            public DateTime fechas;
            public float valores;
        }

        /// <summary>
        /// Carga los datos de un CSV en un punto de un proyecto. Si el punto y/o el proyecto no existen los crea.
        /// </summary>
        /// <param name="nombrePunto">Nombre del punto</param>
        /// <param name="descripcion">Descripción del punto</param>
        /// <param name="nombreProyecto">Nombre del proyecto</param>
        /// <param name="descripcionProyecto">Descripción del proyecto</param>
        /// <param name="nombreRegimen">Nombre del régimen del punto</param>
        /// <param name="abrevRegimen">Abreviatura del régimen del punto</param>
        /// <param name="csv">Ruta donde se encuentra el CSV del cual se cargarán los datos</param>      
        /// <param name="mesInicio">Mes de inicio donde comienza el año a efectos de cálculo</param>      
        public void CrearYCargarEnPunto(string nombrePunto, string descripcion,
            string nombreProyecto, string descripcionProyecto, string nombreRegimen, string abrevRegimen, string csv, string mesInicio)
        {
            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfoProcessCommand")); 
            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPoint") +
                ":" + nombrePunto + "\n"+ _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strProject") + 
                ":" + nombreProyecto + "\n"+ _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCSVData") 
                + ":" + csv);

            //Comprobamos si existe el proyecto y si no lo creamos 
            if (!ExisteProyecto(nombreProyecto))
            {

                if (nombreProyecto.Length > 20)
                {
                    throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataCM2"));
                }

                var valores = new[] { nombreProyecto, descripcionProyecto, };
                var campos = new[] { "nombre", "descripcion" };
                _cMDB.InsertarRegistro("Proyecto", campos, valores);
                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedProject"));
            }


            //Buscamos el ID del proyecto para añadirle el punto.
            string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + nombreProyecto + "'";
            var ds = _cMDB.RellenarDataSet("Proyecto", str);

            string idProyecto = "";


            //TODO: No es necesario el foreach por que la ID es unica. SIMPLIFICAR
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                idProyecto = item[0].ToString();
            }

            string[] s;
            _tabla = "Punto";
            s = new string[6];
            s[0] = "Clave_punto";
            s[1] = "nombre";
            s[2] = "ID_proyecto";
            s[3] = "mesInicio";
            s[4] = "nombreRegimen";
            s[5] = "abreviaturaRegimen";

            string[] t = new[] { nombrePunto.ToUpperInvariant(), descripcion, idProyecto, mesInicio, nombreRegimen, abrevRegimen };

            //Comprobamos que el codigo del punto no existe ya y si no lo creamos

            if (Conversions.ToBoolean(ExisteCodigo(_tabla, t[0], Convert.ToInt32(t[2]))))
            {
                if (_cMDB.InsertarRegistro(_tabla, s, t))
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedPoint"));
                }
                else
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPointNotCreated"));
                }
            }

            //Seleccionamos el ID_Punto del punto para cargarle el csv
            str = "SELECT ID_Punto FROM [Punto] WHERE Clave_Punto='" + nombrePunto.ToUpperInvariant() + "'";

            ds = _cMDB.RellenarDataSet("Punto", str);
            int idPunto = 0;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                idPunto = Convert.ToInt32(item[0]);
            }


            //Cargamos el csv
            ComboItem punto = new ComboItem(".", idPunto);

            if (csv != "")
            {
                LeerCSV(csv, punto);
                AnadirCSV(csv, punto);
            }            
        }
        /// <summary>
        /// Carga los datos de un CSV en una alteración de un proyecto. Si la alteración y/o el proyecto no existen los crea. El punto
        /// debe existir. Si no existe el método no lo crea y debe crearse por separado.
        /// </summary>
        /// <param name="nombrePunto">Nombre del punto</param>
        /// <param name="nombreAlteracion">Nombre de la alteración</param>
        /// <param name="descripcion">Descripción de la alteración</param>
        /// <param name="nombreProyecto">Nombre del proyecto</param>
        /// <param name="descripcionProyecto">Descripción del proyecto</param>
        /// <param name="nombreRegimen">Nombre del régimen de la alteración</param>
        /// <param name="abrevRegimen">Abreviatura del régimen de la alteración</param>
        /// <param name="csv">Ruta donde se encuentra el CSV del cual se cargarán los datos</param>      
        public void CrearYCargarEnAlteracion(string nombrePunto, string nombreAlteracion, string descripcion,
            string nombreProyecto, string descripcionProyecto, string nombreRegimen, string abrevRegimen, string csv)
        {

            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfoProcessCommand"));
            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPoint") + ":" + nombrePunto +
                "\n" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAlt") +":" + nombreAlteracion + 
                "\n" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strProject") + ":" + nombreProyecto +               
                "\n" + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strCSVData") + ":" + csv);

            //Comprobamos si existe el proyecto y si no lo creamos
            if (!ExisteProyecto(nombreProyecto))
            {
                if (nombreProyecto.Length > 20)
                {
                    throw new Exception("El nombre del proyecto es demasiado largo");
                }

                var valores = new[] { nombreProyecto, descripcionProyecto, };
                var campos = new[] { "nombre", "descripcion" };
                if (_cMDB.InsertarRegistro("Proyecto", campos, valores))
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedProject"));
                }
                else
                {
                    throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDataCM2"));
                }

            }
            else
            {

            }

            //Buscamos la id del proyecto para asignarle el punto y la alteracion
            string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + nombreProyecto + "'";
            var ds = _cMDB.RellenarDataSet("Proyecto", str);
            string idProyecto = "";
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                idProyecto = item[0].ToString();
            }
            _tabla = "Punto";



            if (Conversions.ToBoolean(ExisteCodigo(_tabla, nombrePunto, Convert.ToInt32(idProyecto))))
            {
                throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPointMustBeCreated"));
            }

            //Buscamos el id del punto para asignarle la alteracion y cargarle el csv si es necesario

            str = "SELECT ID_Punto FROM [Punto] WHERE Clave_punto='" + nombrePunto.ToUpperInvariant() + "'";

            ds = _cMDB.RellenarDataSet("Punto", str);
            int idPunto = 0;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                idPunto = Convert.ToInt32(item[0]);
            }

            _tabla = "Alteracion";
            string[] s2;

            s2 = new string[5];
            s2[0] = "COD_Alteracion";
            s2[1] = "nombre";
            s2[2] = "ID_Punto";
            s2[3] = "nombreRegimen";
            s2[4] = "abreviaturaRegimen";

            string[] t2 = new[] { nombreAlteracion.ToUpperInvariant(), descripcion, idPunto.ToString(), nombreRegimen, abrevRegimen };

            //Comprobamos si existe la alteración y si no lo creamos.
            if (Conversions.ToBoolean(ExisteCodigo(_tabla, t2[0], Convert.ToInt32(t2[2]))))
            {
                if (_cMDB.InsertarRegistro(_tabla, s2, t2))
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedAlt"));
                }
                else
                {
                    throw new Exception(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorAltNotCreated"));
                }
            }

            //Buscamos el id de la alteracion para cargarle el csv
            str = "SELECT ID_Alteracion FROM [Alteracion] WHERE COD_Alteracion='" + nombreAlteracion.ToUpperInvariant() + "' AND ID_Punto=" + idPunto;

            ds = _cMDB.RellenarDataSet("Alteracion", str);
            int idAlteracion = 0;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                idAlteracion = Convert.ToInt32(item[0]);
            }

            ComboItem punto = new ComboItem(".", idPunto);
            ComboItem alteracion = new ComboItem(".", idAlteracion);
            if (csv != "")
            {
                LeerCSV(csv, alteracion);
                AnadirCSV(csv, punto, alteracion);
            }
        }
        /// <summary>
        /// Exporta el excel de un punto de un proyecto a la ruta indicada.
        /// </summary>
        /// <param name="nombrePunto">Nombre del punto</param>       
        /// <param name="nombreProyecto">Nombre del proyecto</param>
        /// <param name="exportacion">Ruta donde se exportará el excel</param>      
        public void ExportarExceldePunto(string nombrePunto, string nombreProyecto, string exportacion)
        {
            var fInicio = new FormInicial();
            fInicio.Show();

            //Comprobamos si existe el directorio donde exportamos el informe
            if (Directory.Exists(exportacion))
            {

                int index1;
                int indexPro;
                indexPro = fInicio.cbProyectos.FindStringExact(nombreProyecto);
                if (indexPro != -1)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strExportPoint") + nombrePunto);

                    fInicio.cbProyectos.SelectedIndex = indexPro;

                    index1 = fInicio.lstboxPuntos.FindStringExact(nombrePunto);
                    if (index1 != -1)
                    {
                        fInicio.lstboxPuntos.SetSelected(index1, true);
                        if (fInicio.btnCalcular.Enabled)
                        {
                            fInicio.btnCalcular.PerformClick();
                        }
                        else if (Convert.ToInt32(fInicio.lblAñosHidro.Text) < 7)
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotEnoughYears"));
                        }
                        else
                        {

                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed"));
                        }
                        fInicio.Close();        
                    }
                    else
                    {
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointHasntBeenFound"));
                        fInicio.Close();
                    }
                }
                else
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                    fInicio.Close();
                }


            }
            else
            {
                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPathCmd"));
                fInicio.Close();
            }

        }
        /// <summary>
        /// Exporta el excel de una alteración de un proyecto a la ruta indicada.
        /// </summary>
        /// <param name="nombrePunto">Nombre del punto</param> 
        /// <param name="nombreAlteracion">Nombre del punto</param> 
        /// <param name="nombreProyecto">Nombre del proyecto</param>
        /// <param name="exportacion">Ruta donde se exportará el excel</param>
        /// <param name="cvh">Parámetro de coetaneidad para valores habituales. 0 para no, 1 para si.</param>  
        /// <param name="cas">Parámetro de coetaneidad de avenidas y sequías. 0 para no, 1 para si.</param>  
        /// <remarks>Si IAHRIS no puede cambiar el uso de coetaneidad ignorará los parámetros de coetaneidad.</remarks>
        public void ExportarExceldeAlteracion(string nombrePunto, string nombreAlteracion,
            string nombreProyecto, string exportacion, bool cvh, bool cas)
        {
            var fInicio = new FormInicial();
            fInicio.Show();

            //Comprobamos si el directorio donde exportar el informe existe
            if (Directory.Exists(exportacion))
            {

                int index1;
                int indexPro;

                indexPro = fInicio.cbProyectos.FindStringExact(nombreProyecto);
                if (indexPro != -1)
                {
                    fInicio.cbProyectos.SelectedIndex = indexPro;

                    index1 = fInicio.lstboxPuntos.FindStringExact(nombrePunto);
                    if (index1 != -1)
                    {
                        fInicio.lstboxPuntos.SetSelected(index1, true);

                        int index2 = fInicio.cmbListaAlteradasDiarias.FindStringExact(nombreAlteracion);
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strExportAlt") + nombreAlteracion);

                        if (index2 != -1)
                        {
                            fInicio.cmbListaAlteradasDiarias.SelectedIndex = index2;
                            if (cvh)
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

                            if (cas)
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




                            Console.WriteLine(fInicio.btnCalcular.Text);
                            if (fInicio.btnCalcular.Enabled) { fInicio.btnCalcular.PerformClick(); }
                            else if (Convert.ToInt32(fInicio.lblAñosHidro.Text) < 7)
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strNotEnoughYears"));
                            }
                            else { Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed")); }

                            fInicio.Close();
                        }
                        else
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltHasntBeenFound"));
                            fInicio.Close();
                        }
                    }
                }
                else
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                    fInicio.Close();
                }

            }
        }
        /// <summary>
        /// Comunica los parametros recibidos por linea de comandos en la aplicacion con el metodo adecuado para la tarea seleccionada
        /// </summary>
        /// <param name="nombrePunto">Nombre del punto</param> 
        /// <param name="nombreAlteracion">Nombre de la alteración</param> 
        /// <param name="descripcion">Descripcion del punto o de la alteración</param>
        /// <param name="nombreProyecto">Nombre del proyecto</param>
        /// <param name="descripcionProyecto">Descripción del proyecto</param>  
        /// <param name="nombreRegimen">Nombre del regimen del punto o de la alteracion</param>  
        /// <param name="abrevRegimen">Abreviatura del regimen del punto o de la alteracion</param> 
        /// <param name="csv">Ruta del archivo csv</param> 
        /// <param name="exportacion">Ruta a la cual se exportará el excel</param>
        /// <param name="cvh">Parámetro de coetaneidad para valores habituales. 0 para no, 1 para si.</param>
        /// <param name="cas">Parámetro de coetaneidad de avenidas y sequías. 0 para no, 1 para si.</param>  
        /// <param name="cm">Selector de proceso. CD para Crear y Carga, GIS para Exportar Excel</param>  
        /// <param name="pa">Selector de punto/alteracion. P para punto, A para Alteración</param>  
        /// <param name="mesInicio">Mes de inicio donde comienza el año a efectos de cálculo</param>      
        /// <remarks>Si IAHRIS no puede cambiar el uso de coetaneidad ignorará los parámetros de coetaneidad.</remarks>
        public void SeleccionProceso( string nombrePunto,  string nombreAlteracion,
             string descripcion,  string nombreProyecto,  string descripcionProyecto,  string nombreRegimen,  string abrevRegimen,
             string csv,  string exportacion,  bool cvh,  bool cas,  string cm,  string pa, string mesInicio)
        {
            if (cm == "CD")
            {
                if (pa == "P")
                {
                    CrearYCargarEnPunto(nombrePunto, descripcion, nombreProyecto, descripcionProyecto, nombreRegimen, abrevRegimen, csv, mesInicio);
                }
                else if (pa == "A")
                {
                    CrearYCargarEnAlteracion(nombrePunto, nombreAlteracion, descripcion, nombreProyecto, descripcionProyecto,
                        nombreRegimen, abrevRegimen, csv);
                }
            }
            else if (cm == "GIS")
            {
                if (pa == "P")
                {
                    ExportarExceldePunto(nombrePunto, nombreProyecto, exportacion);
                }
                else if (pa == "A")
                {
                    ExportarExceldeAlteracion(nombrePunto, nombreAlteracion, nombreProyecto, exportacion, cvh, cas);
                }
            }
        }


/*
        public void AccesoPorParametros(FormBienvenida formBienvenida, string nombrePunto, string nombreAlteracion, string descripcion, 
            string nombreProyecto, string descripcionProyecto, string nombreRegimen, string abrevRegimen, string csv, string exportacion)
        {
            _traductor = formBienvenida._traductor;
            _cMDB = formBienvenida._cMDB;

            _tabla = formBienvenida._tabla;
            _cabecera = formBienvenida._cabecera;
            _tFechas = formBienvenida._tFechas;











          
            if (parametros.ContainsKey("CD"))
            {
                string nombrePunto;
                string nombreAlteracion;
                string descripcion;
                string nombreProyecto;
                string descripcionProyecto;
                string nombreRegimen;
                string abrevRegimen;
                string csv;


                parametros.TryGetValue("/t", out string pa);

                if (pa == "P")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }
                    if (!parametros.TryGetValue("/d", out descripcion))
                    {
                        throw new Exception("Parámetro /d no encontrado");
                    }
                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/dp", out descripcionProyecto))
                    {
                        throw new Exception("Parámetro /dp no encontrado");
                    }
                    if (!parametros.TryGetValue("/rn", out nombreRegimen))
                    {
                        throw new Exception("Parámetro /rn no encontrado");
                    }
                    if (!parametros.TryGetValue("/ra", out abrevRegimen))
                    {
                        throw new Exception("Parámetro /ra no encontrado");
                    }
                    if (!parametros.TryGetValue("/fe", out csv))
                    {
                        throw new Exception("Parámetro /fe no encontrado");
                    }


                    Console.WriteLine("Comenzando el procesado del comando.");
                    Console.WriteLine("Punto:" + nombrePunto + "\nProyecto:" + nombreProyecto + "\nDatosCSV:" + csv);

                    //Comprobamos si existe el proyecto y si no lo creamos 
                    if (!ExisteProyecto(nombreProyecto))
                    {

                        if (nombreProyecto.Length > 20)
                        {
                            throw new Exception("El nombre del proyecto es demasiado largo");
                        }

                        var valores = new[] { nombreProyecto, descripcionProyecto, };
                        var campos = new[] { "nombre", "descripcion" };
                        _cMDB.InsertarRegistro("Proyecto", campos, valores);
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedProject"));
                    }


                    //Buscamos el ID del proyecto para añadirle el punto.
                    string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + nombreProyecto + "'";
                    var ds = _cMDB.RellenarDataSet("Proyecto", str);

                    string idProyecto = "";


                    //TODO: No es necesario el foreach por que la ID es unica. SIMPLIFICAR
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        idProyecto = item[0].ToString();
                    }

                    string[] s;
                    _tabla = "Punto";
                    s = new string[6];
                    s[0] = "Clave_punto";
                    s[1] = "nombre";
                    s[2] = "ID_proyecto";
                    s[3] = "mesInicio";
                    s[4] = "nombreRegimen";
                    s[5] = "abreviaturaRegimen";

                    string[] t = new[] { nombrePunto.ToUpperInvariant(), descripcion, idProyecto, "10", nombreRegimen, abrevRegimen };

                    //Comprobamos que el codigo del punto no existe ya y si no lo creamos

                    if (Conversions.ToBoolean(ExisteCodigo(_tabla, t[0], Convert.ToInt32(t[2]))))
                    {
                        if (_cMDB.InsertarRegistro(_tabla, s, t))
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedPoint"));
                        }
                        else
                        {
                            Console.WriteLine("No se ha podido crear el punto");
                        }
                    }

                    //Seleccionamos el ID_Punto del punto para cargarle el csv
                    str = "SELECT ID_Punto FROM [Punto] WHERE Clave_Punto='" + nombrePunto.ToUpperInvariant() + "'";

                    ds = _cMDB.RellenarDataSet("Punto", str);
                    int idPunto = 0;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        idPunto = Convert.ToInt32(item[0]);
                    }


                    //Cargamos el csv
                    ComboItem punto = new ComboItem(".", idPunto);

                    if (csv != "")
                    {
                        LeerCSV(csv, punto);
                        AnadirCSV(csv, punto);
                    }
                    formBienvenida.Dispose();
                }
                else if (pa == "A")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }
                    if (!parametros.TryGetValue("/na", out nombreAlteracion))
                    {
                        throw new Exception("Parámetro /na no encontrado");
                    }
                    if (!parametros.TryGetValue("/d", out descripcion))
                    {
                        throw new Exception("Parámetro /d no encontrado");
                    }
                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/dp", out descripcionProyecto))
                    {
                        throw new Exception("Parámetro /dp no encontrado");
                    }
                    if (!parametros.TryGetValue("/rn", out nombreRegimen))
                    {
                        throw new Exception("Parámetro /rn no encontrado");
                    }
                    if (!parametros.TryGetValue("/ra", out abrevRegimen))
                    {
                        throw new Exception("Parámetro /ra no encontrado");
                    }
                    if (!parametros.TryGetValue("/fe", out csv))
                    {
                        throw new Exception("Parámetro /fe no encontrado");
                    }

                    Console.WriteLine("Comenzando el procesado del comando.");
                    Console.WriteLine("Punto:" + nombrePunto + "\nAlteración:" + nombreAlteracion + "\nProyecto:" + nombreProyecto + "\nDatosCSV:" + csv);



                    //Comprobamos si existe el proyecto y si no lo creamos
                    if (!ExisteProyecto(nombreProyecto))
                    {
                        if (nombreProyecto.Length > 20)
                        {
                            throw new Exception("El nombre del proyecto es demasiado largo");
                        }

                        var valores = new[] { nombreProyecto, descripcionProyecto, };
                        var campos = new[] { "nombre", "descripcion" };
                        if (_cMDB.InsertarRegistro("Proyecto", campos, valores))
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedProject"));
                        }
                        else
                        {
                            throw new Exception("No se ha podido crear el proyecto.");
                        }

                    }
                    else
                    {

                    }

                    //Buscamos la id del proyecto para asignarle el punto y la alteracion
                    string str = "SELECT ID_Proyecto FROM [Proyecto] WHERE nombre='" + nombreProyecto + "'";
                    var ds = _cMDB.RellenarDataSet("Proyecto", str);
                    string idProyecto = "";
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        idProyecto = item[0].ToString();
                    }
                    _tabla = "Punto";



                    if (Conversions.ToBoolean(ExisteCodigo(_tabla, nombrePunto, Convert.ToInt32(idProyecto))))
                    {
                        throw new Exception("El punto no existe. Se debe crear primero el punto.");
                    }

                    //Buscamos el id del punto para asignarle la alteracion y cargarle el csv si es necesario

                    str = "SELECT ID_Punto FROM [Punto] WHERE Clave_punto='" + nombrePunto.ToUpperInvariant() + "'";

                    ds = _cMDB.RellenarDataSet("Punto", str);
                    int idPunto = 0;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        idPunto = Convert.ToInt32(item[0]);
                    }

                    _tabla = "Alteracion";
                    string[] s2;

                    s2 = new string[5];
                    s2[0] = "COD_Alteracion";
                    s2[1] = "nombre";
                    s2[2] = "ID_Punto";
                    s2[3] = "nombreRegimen";
                    s2[4] = "abreviaturaRegimen";

                    string[] t2 = new[] { nombreAlteracion.ToUpperInvariant(), descripcion, idPunto.ToString(), nombreRegimen, abrevRegimen };

                    //Comprobamos si existe la alteración y si no lo creamos.
                    if (Conversions.ToBoolean(ExisteCodigo(_tabla, t2[0], Convert.ToInt32(t2[2]))))
                    {
                        if (_cMDB.InsertarRegistro(_tabla, s2, t2))
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddedAlt"));
                        }
                        else
                        {
                            throw new Exception("No se ha podido crear la alteración.");
                        }
                    }

                    //Buscamos el id de la alteracion para cargarle el csv
                    str = "SELECT ID_Alteracion FROM [Alteracion] WHERE COD_Alteracion='" + nombreAlteracion.ToUpperInvariant() + "' AND ID_Punto=" + idPunto;

                    ds = _cMDB.RellenarDataSet("Alteracion", str);
                    int idAlteracion = 0;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        idAlteracion = Convert.ToInt32(item[0]);
                    }

                    ComboItem punto = new ComboItem(".", idPunto);
                    ComboItem alteracion = new ComboItem(".", idAlteracion);
                    if (csv != "")
                    {
                        LeerCSV(csv, alteracion);
                        AnadirCSV(csv, punto, alteracion);
                    }
                    formBienvenida.Dispose();
                }
                else
                {
                    throw new Exception("Parámetro /t incorrecto");
                }

            }
            else if (parametros.ContainsKey("GIS"))
            {

                string nombrePunto;
                string nombreAlteracion;
                string nombreProyecto;
                string exportacion;

                parametros.TryGetValue("/t", out string pa);
                if (pa == "P")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }

                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/fs", out exportacion))
                    {
                        throw new Exception("Parámetro /fs no encontrado");
                    }


                    var fInicio = new FormInicial();
                    fInicio.Show();

                    //Comprobamos si existe el directorio donde exportamos el informe
                    if (Directory.Exists(exportacion))
                    {

                        int index1;
                        int indexPro;
                        indexPro = fInicio.cbProyectos.FindStringExact(nombreProyecto);
                        if (indexPro != -1)
                        {
                            Console.WriteLine("Exportar Punto :  " + nombrePunto);

                            fInicio.cbProyectos.SelectedIndex = indexPro;

                            index1 = fInicio.lstboxPuntos.FindStringExact(nombrePunto);
                            if (index1 != -1)
                            {
                                fInicio.lstboxPuntos.SetSelected(index1, true);
                                if (fInicio.btnCalcular.Enabled)
                                {
                                    fInicio.btnCalcular.PerformClick();
                                }
                                else if (Convert.ToInt32(fInicio.lblAñosHidro.Text) < 7)
                                {
                                    Console.WriteLine("Los años disponibles en el punto/alteracion no son suficientes para ejecutar el calculo");
                                }
                                else
                                {

                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed"));
                                }
                                fInicio.Close();
                                formBienvenida.Dispose();
                            }
                            else
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strPointHasntBeenFound"));
                                fInicio.Close();
                                formBienvenida.Dispose();
                            }
                        }
                        else
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                            fInicio.Close();
                            formBienvenida.Dispose();
                        }


                    }
                    else
                    {
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPathCmd"));
                        fInicio.Close();
                        formBienvenida.Dispose();
                    }

                }
                else if (pa == "A")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }
                    if (!parametros.TryGetValue("/na", out nombreAlteracion))
                    {
                        throw new Exception("Parámetro /na no encontrado");
                    }
                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/fs", out exportacion))
                    {
                        throw new Exception("Parámetro /fs no encontrado");
                    }

                    var fInicio = new FormInicial();
                    fInicio.Show();

                    //Comprobamos si el directorio donde exportar el informe existe
                    if (Directory.Exists(exportacion))
                    {

                        int index1;
                        int indexPro;

                        indexPro = fInicio.cbProyectos.FindStringExact(nombreProyecto);
                        if (indexPro != -1)
                        {
                            fInicio.cbProyectos.SelectedIndex = indexPro;

                            index1 = fInicio.lstboxPuntos.FindStringExact(nombrePunto);
                            if (index1 != -1)
                            {
                                fInicio.lstboxPuntos.SetSelected(index1, true);

                                int index2 = fInicio.cmbListaAlteradasDiarias.FindStringExact(nombreAlteracion);
                                Console.WriteLine("Exportar Alteración: " + nombreAlteracion);

                                if (index2 != -1)
                                {
                                    fInicio.cmbListaAlteradasDiarias.SelectedIndex = index2;
                                    if (parametros.ContainsKey("-cvh"))
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

                                    if (parametros.ContainsKey("-cas"))
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




                                    Console.WriteLine(fInicio.btnCalcular.Text);
                                    if (fInicio.btnCalcular.Enabled) { fInicio.btnCalcular.PerformClick(); }
                                    else if (Convert.ToInt32(fInicio.lblAñosHidro.Text) < 7)
                                    {
                                        Console.WriteLine("Los años disponibles en el punto/alteracion no son suficientes para ejecutar el calculo");
                                    }
                                    else { Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strCalcCantBePerformed")); }

                                    fInicio.Close();
                                    formBienvenida.Dispose();
                                }
                                else
                                {
                                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strAltHasntBeenFound"));
                                    fInicio.Close();
                                    formBienvenida.Dispose();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strProjectHasntBeenFound"));
                            fInicio.Close();
                            formBienvenida.Dispose();
                        }

                    }
                    else
                    {
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPathCmd"));
                        fInicio.Close();
                        formBienvenida.Dispose();
                    }
                }
                else
                {
                    throw new Exception("Parámetro /t incorrecto");
                }
                formBienvenida.Dispose();
            }
        }
*/

        /// <summary>
        /// Funcion para manejar puntos y alteraciones y comprobar si ya existen o se pueden añadir a un proyecto.
        /// </summary>
        /// <param name="tabla">Parametro para seleccionar la tabla de los puntos o la de las alteraciones</param>
        /// <param name="codigo">Clave del punto o de la alteracion</param>
        /// <param name="id">Parametro para seleccionar el proyecto a comprobar o la id del punto</param>
        /// <returns>Devuelve true si el punto o alteracion se puede añadir y false y hay algun error.</returns>
        private object ExisteCodigo(string tabla, string codigo, int id)
        {
            DataSet ds;
            DataSet ds2;
            DataRow dr;
            DataRow dr2;
            if (tabla == "Punto")
            {
                ds = _cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM Punto WHERE clave_punto='" + codigo + "' AND ID_proyecto=" + id.ToString());
                ds2 = _cMDB.RellenarDataSet("existe2", "SELECT COUNT(*) FROM Proyecto WHERE ID_Proyecto=" + id.ToString());
                dr = ds.Tables[0].Rows[0];
                dr2 = ds2.Tables[0].Rows[0];

                if (Convert.ToInt32(dr2[0]) == 1 && Convert.ToInt32(dr[0]) == 0)
                {
                    return true;

                }
                else if (Convert.ToInt32(dr2[0]) == 1 && Convert.ToInt32(dr[0]) == 1)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPointAlreadyExistCmd"));
                    return false;
                }
                else if (Convert.ToInt32(dr2[0]) == 0 && Convert.ToInt32(dr[0]) == 0)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorProjectDontExistCmd"));
                    return false;
                }
                else
                {
                    return false;
                }

            }
            else if (tabla == "Alteracion")
            {
                ds = _cMDB.RellenarDataSet("existe", "SELECT COUNT(*) FROM Vista_alteracionFull WHERE COD_Alteracion='" + codigo + "' AND ID_Punto=" + id);
                dr = ds.Tables[0].Rows[0];
                ds2 = _cMDB.RellenarDataSet("existe2", "SELECT COUNT(*) FROM Punto WHERE ID_punto=" + id);
                dr2 = ds2.Tables[0].Rows[0];

                if (Convert.ToInt32(dr[0]) == 0 && Convert.ToInt32(dr2[0]) == 1)
                {
                    return true;
                }
                else if (Convert.ToInt32(dr[0]) == 1 && Convert.ToInt32(dr2[0]) == 1)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorAltAlreadyExistCmd"));
                    return false;
                }

                else if (Convert.ToInt32(dr2[0]) == 0)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorPointDontExistCmd"));
                    return false;
                }

                else
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strError"));

                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool ExisteProyecto(string nombreProyecto)
        {
            DataSet ds;
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

        /// <summary>
        /// Función para comprobar el CSV a añadir.
        /// </summary>
        /// <param name="_rutafichero">Ruta del CSV</param>
        /// <param name="punto">Clave del punto o de la alteración a la que añadir el CSV</param>
        private void LeerCSV(string _rutafichero, ComboItem punto)
        {
            string[] fields;
            string delimiter = ";";

            if (File.Exists(_rutafichero))
            {
                using (var parser = new TextFieldParser(_rutafichero))
                {
                    parser.SetDelimiters(delimiter);
                    fields = parser.ReadFields();

                    // CABECERA
                    // La cabecera puede tener entre 3 o 4 campos
                    // --- 4 Campos: Alterada
                    // --- 3 Campos: Natural
                    // ------------------------------------------------
                    if (fields.Length > 4 | fields.Length < 3)
                    {
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeader"));
                        return;
                    }

                    try
                    {
                        if (fields[0] == "DIARIO")
                        {
                            _cabecera.TipoFechas = true;
                            _cabecera.FormatoFecha = "dd/MM/yyyy";
                        }
                        else if (fields[0] == "MENSUAL")
                        {
                            _cabecera.TipoFechas = false;
                            _cabecera.FormatoFecha = "MM/yyyy";
                        }
                        else
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderDate"));

                            return;
                        }

                        if (fields[1] == "NATURAL")
                        {
                            _cabecera.IsNatural = true;
                        }
                        else if (fields[1] == "ALTERADO")
                        {
                            _cabecera.IsNatural = false;
                        }
                        else
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderType"));

                            return;
                        }

                        _cabecera.FilePoint = fields[2];

                        if (_cabecera.IsNatural == false & fields.Length != 4)
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderAlt"));
                            return;
                        }

                        if (_cabecera.IsNatural == false)
                        {
                            _cabecera.FileAlt = fields[3];
                            _cabecera.Clave_Alteracion = punto;
                        }
                        else
                        {
                            _cabecera.Clave_Alteracion = null;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVHeaderGeneral"));
                        return;
                    }

                    fields = parser.ReadFields();

                    // Testear que la alteracion (si es alteracion) pertenece al punto

                    var dt = default(DateTime);
                    // Testear fechas
                    if (_cabecera.TipoFechas)
                    {

                        if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, fields[0], ref dt))
                        {
                            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormatDaily"));
                            return;
                        }
                    }
                    else if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, fields[1] + "/" + fields[0], ref dt))
                    {
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormatMonthly"));
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Función para añadir el CSV a un punto o una alteración
        /// </summary>
        /// <param name="_rutafichero">Ruta del fichero CSV</param>
        /// <param name="punto">Clave del punto al que añadir el CSV</param>
        /// <param name="alt">Clave de la alteración a la que añadir el CSV, si no se asigna un valor toma null para añadir a un punto.</param>
        private void AnadirCSV(string _rutafichero, ComboItem punto, ComboItem alt = null)
        {
            string[] fields;
            string delimiter = ";";
            DatosCSV[] datos;
            string sFechas;
            string sValor;
            int linea;
            long lonDatos;
            var fechaINI = default(DateTime);
            var fechaFIN = default(DateTime);
            linea = 1;
            lonDatos = 0L;
            datos = null;

            _cabecera.Clave_Punto = punto;
            _cabecera.Clave_Alteracion = alt;

            // Errores y su gestion
            // --------------------
            int nErrores = 0;
            var strErrores = new ArrayList();

            using (var parser = new TextFieldParser(_rutafichero))
            {
                parser.SetDelimiters(delimiter);
                DataSet dsMesInicio;
                var mesInicio = default(int);
                while (!parser.EndOfData)
                {

                    // Read in the fields for the current line
                    fields = parser.ReadFields();
                    if (linea > 1)
                    {
                        // Campo de datos
                        Array.Resize(ref datos, (int)(lonDatos + 1));
                        if (_cabecera.TipoFechas)
                        {
                            sFechas = fields[0];
                            sValor = fields[1];
                        }
                        else
                        {
                            sFechas = fields[1] + "/" + fields[0];
                            sValor = fields[2];
                        }


                        if (linea == 2)
                        {

                            bool okfecha = _tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref fechaINI);
                            if (!okfecha)
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") + linea.ToString());

                                return;
                            }

                            okfecha = _tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref fechaFIN);
                            if (!okfecha)
                            {
                                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCSVDateFormat") + linea.ToString());

                                return;
                            }

                            // ----------------------------------------
                            // Adecuar fecha INICIAL al año hidrologico
                            // ----------------------------------------


                            dsMesInicio = _cMDB.RellenarDataSet("Puntos", "SELECT mesInicio FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id);

                            mesInicio = Conversions.ToInteger(dsMesInicio.Tables[0].Rows[0][0]);



                            if (fechaINI.Month != mesInicio | fechaINI.Day != 1)
                            {
                                if (fechaINI.Month < mesInicio)
                                {
                                    fechaINI = new DateTime(fechaINI.Year - 1, mesInicio, 1);
                                }
                                else
                                {
                                    fechaINI = new DateTime(fechaINI.Year, mesInicio, 1);
                                }
                            }
                        }

                        if (!_tFechas.ComprobarFechasCSV(_cabecera.TipoFechas, sFechas, ref datos[(int)lonDatos].fechas))
                        {


                            nErrores = nErrores + 1;
                            strErrores.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorLine") + linea + ": " + sFechas + 
                                _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDateIsNotValid"));
                            continue;
                        }

                        if (DateTime.Compare(fechaINI, datos[(int)lonDatos].fechas) > 0)
                        {
                            fechaINI = datos[(int)lonDatos].fechas;
                        }

                        if (DateTime.Compare(fechaFIN, datos[(int)lonDatos].fechas) < 0)
                        {
                            fechaFIN = datos[(int)lonDatos].fechas;
                        }


                        NumberFormatInfo ni = new NumberFormatInfo();
                        ni.NumberDecimalSeparator = ".";

                       
                        try { 
                            if (!float.TryParse(sValor, NumberStyles.Float, ni, out datos[(int)lonDatos].valores))
                            {

                                nErrores = nErrores + 1;
                                strErrores.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorLine") + linea + ": " + sValor + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorValueIsNotValid"));
                                continue;
                            }
                            else if (datos[(int)lonDatos].valores < 0f)
                            {
                                nErrores = nErrores + 1;
                                strErrores.Add(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorLine") + linea + ": " + sValor + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorValueIsNotValid"));
                                continue;
                            }
                        }catch (Exception ex) {
                            Console.WriteLine(ex.Message);
                        }
                        lonDatos = lonDatos + 1L;
                    }

                    linea = linea + 1;
                }

                // Si hay error muestro un mensaje de error
                if (nErrores > 0)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorFound") + nErrores + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorCannotContinue"));
                    foreach (var item in strErrores)
                    {
                        Console.WriteLine(item.ToString());
                    }
                    nErrores = 0;
                    strErrores = null;
                }

                int mesFin;
                mesFin = mesInicio - 1;
                if (mesFin <= 0)
                {
                    mesFin = 12;
                }

                int diaFin = DateTime.DaysInMonth(fechaFIN.Year, mesFin);


                if (fechaFIN.Month != mesFin)
                {
                    if (fechaFIN.Month > mesFin)
                    {
                        diaFin = DateTime.DaysInMonth(fechaFIN.Year + 1, mesFin);
                        fechaFIN = new DateTime(fechaFIN.Year + 1, mesFin, diaFin);
                    }
                    else
                    {
                        diaFin = DateTime.DaysInMonth(fechaFIN.Year, mesFin);
                        fechaFIN = new DateTime(fechaFIN.Year, mesFin, diaFin);
                    }
                }
            }

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++++ Revisar por si hay errores o duplicidades +++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            DataSet dsPunto;
            DataSet dsAlt;
            // Dim dsNat As DataSet

            var idAlt = default(int);

            string nombrelista;

            // Se comienza ha hacer la transaccion
            _cMDB.ComenzarTransaccion();

            // Se comprueba que el punto existe y la relación con la alteración (si es una alteración) tambien existe.
            if (!ComprobarSerie())
            {
                return;
            }

            // Sacar nombre y id
            dsPunto = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id);
            var drAux = dsPunto.Tables[0].Rows[0];

            nombrelista = _cabecera.Clave_Punto.ToString();

            // Comprobaciones que dependen si es una lista Alterada o Natural
            if (_cabecera.IsNatural == false)
            {
                nombrelista = nombrelista + "Alt";
                if (_cabecera.TipoFechas == true)
                {
                    nombrelista = nombrelista + "Diario";
                }
                else
                {
                    nombrelista = nombrelista + "Mensual";
                }

                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE ID_Alteracion =" + _cabecera.Clave_Alteracion.Id);
                var drAlt = dsAlt.Tables[0].Rows[0];
                idAlt = Conversions.ToInteger(drAlt[0]);
            }
            else
            {
                nombrelista = nombrelista + "Nat";
                if (_cabecera.TipoFechas == true)
                {
                    nombrelista = nombrelista + "Diario";
                }
                else
                {
                    nombrelista = nombrelista + "Mensual";
                }
            }

            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++++ Insertar en la LISTA ++++++++++++++++++++++++++++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            DataRow dr;
            dr = dsPunto.Tables[0].Rows[0];
            int i;
            var campos = new string[] { "ID_punto", "Tipo_lista", "ID_Alteracion", "Tipo_fechas", "fecha_INI", "fecha_FIN", "formato_fecha", "Nombre" };
            var valores = new string[] { dr["id_punto"].ToString(), _cabecera.IsNatural.ToString(), idAlt.ToString(), _cabecera.TipoFechas.ToString(), fechaINI.ToString(_cabecera.FormatoFecha), fechaFIN.ToString(_cabecera.FormatoFecha), _cabecera.FormatoFecha, nombrelista + idAlt.ToString() };
            if (!_cMDB.InsertarRegistro("Lista", campos, valores))
            {
                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDB"));

                _cMDB.TerminarTransaccion(false);
                return;
            }

            // Truco: Saco la ultima que he insertado, asi se cual la lista a la que le doy valores
            dsPunto = _cMDB.RellenarDataSet("Lista", "SELECT TOP 1 id_lista FROM Lista ORDER BY id_lista DESC"); // Ultima que acabo de meter
            var id_lista = dsPunto.Tables[0].Rows[0]["id_lista"];
            campos = new string[3];
            valores = new string[3];
            string[][] mDatos;    // Lo preparo para la funcion de la base de datos
            mDatos = new string[(int)(lonDatos - 1L + 1)][];
            campos[0] = "ID_lista";
            campos[1] = "Valor";
            campos[2] = "Fecha";

            // Lista de datos a montar
            var loopTo1 = (int)(lonDatos - 1L);
            for (i = 0; i <= loopTo1; i++)
            {
                Array.Resize(ref mDatos[i], 3);
                mDatos[i][0] = Conversions.ToString(id_lista);
                mDatos[i][1] = datos[i].valores.ToString();
                mDatos[i][2] = Conversions.ToString(datos[i].fechas);
            }

            // Insercion de los valores en la base datos
            if (!_cMDB.InsertarRegistros("Valor", campos, mDatos))
            {
                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorDBExt"));
                _cMDB.TerminarTransaccion(false);

                return;
            }

            _cMDB.TerminarTransaccion(true);

            Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAddListOK_1") + "\n");


        }

        /// <summary>
        /// Comprobaciones sobre puntos y alteraciones
        /// </summary>
        /// <returns>Devuelve true si esta todo correcto y false si hay algun error</returns>
        private bool ComprobarSerie()
        {

            // Si no hay nada, no hago nada :)
            if (_cabecera.Clave_Punto == null)
            {
                return false;
            }

            // PRIMERA COMPROBACION: ¿Esta el punto?
            // +++++++++++++++++++++++++++++++++++++
            DataSet dsPunto;
            DataSet dsAlt;
            DataRow drAux;
            int idPunto;
            var idAlt = default(int);
            dsPunto = _cMDB.RellenarDataSet("Puntos", "SELECT id_punto FROM [Punto] WHERE Id_punto =" + _cabecera.Clave_Punto.Id);
            if (dsPunto.Tables[0].Rows.Count == 0)
            {
                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strError") + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strPointNotInDB") + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNotInDB"));
                return false;
            }
            else
            {
                drAux = dsPunto.Tables[0].Rows[0];
                idPunto = Conversions.ToInteger(drAux[0]);
            }

            if (_cabecera.Clave_Alteracion != null)
            {

                // SEGUNDA COMPROBACION: ¿Existe la alteracion?
                // ++++++++++++++++++++++++++++++++++++++++++++
                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE ID_Alteracion =" + _cabecera.Clave_Alteracion.Id);
                if (dsAlt.Tables[0].Rows.Count != 1)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strError") + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAltNotInDB") + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNotInDB"));
                    return false;
                }
                else
                {
                    var drAlt = dsAlt.Tables[0].Rows[0];
                    idAlt = Conversions.ToInteger(drAlt[0]);
                }

                // TERCERA COMPROBACION: ¿Concuerdan Punto-Alteracion?
                // +++++++++++++++++++++++++++++++++++++++++++++++++++
                // Comprobar que la alteración pertence al punto
                dsAlt = _cMDB.RellenarDataSet("Alteraciones", "SELECT ID_Alteracion FROM [Alteracion] WHERE id_alteracion=" + idAlt + " AND id_punto=" + idPunto);
                if (dsAlt.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strError") + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAltNotInDB") + _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNotAsociated"));
                    return false;
                }
            }

            return true;
        }
    }
}
