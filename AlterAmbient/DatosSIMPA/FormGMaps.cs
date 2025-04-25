using CargaPorLotes.Logica;
using DatosSimpa.DTO;
using DatosSimpa.Logica;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using IAHRIS;
using IAHRIS.Calculo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DatosSimpa
{
    public partial class FormGMaps : Form
    {
        public IAHRIS.BBDD.OleDbDataBase _cMDB;
        public string _tabla;
        public TestFechas _tFechas;
        public  MultiLangXML.MultiIdiomasXML _traductor;

        private List<EstacionAforoDTO> listaAforos;
        private List<string> listaCuencas;
        private  MotorDatos _motorDatos;
        private  GestorDescargas _gestorDescargas;
        private static ProgresoDatosForm _progresoDatosForm;
        private UTMCoordinate coordenadasSeleccionadas;
        private  ProgresoDescargaForm _progresoDescargaForm;



        public FormGMaps()
        {
            InitializeComponent();
           

            
        }

        private void FormGMaps_Load(object sender, EventArgs e)
        {

            listaAforos = EstacionAforoDTO.CargarListaXml();

            this.cb_ListaAforos.DataSource = listaAforos;
            listaCuencas = listaAforos.Select(x => x.Confederacion).Distinct().ToList();
            listaCuencas.Add("Todas");
            listaCuencas.Sort();

            this.cbCuencaHidrografica.DataSource = listaCuencas;
            this.cbCuencaHidrografica.SelectedItem = "Todas";

            Form argform = this;

            _progresoDescargaForm = new ProgresoDescargaForm();
            _progresoDatosForm = new ProgresoDatosForm();
            _motorDatos = new MotorDatos();
            _gestorDescargas = new GestorDescargas();
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
            _cMDB = new IAHRIS.BBDD.OleDbDataBase("Base", Application.StartupPath + @"\IAHRISv2.mdb");
            _tFechas = new TestFechas(new IAHRIS.BBDD.OleDbDataBase("Base", Application.StartupPath + @"\IAHRISv2.mdb"));

            mapaHidro.Position = new PointLatLng(40.4427, -3.6309); // Madrid como ubicación inicial
            mapaHidro.CanDragMap = true;
            mapaHidro.DragButton = MouseButtons.Left;
            mapaHidro.MinZoom = 0;
            mapaHidro.MaxZoom = 29;
            mapaHidro.Zoom = 6;
            mapaHidro.AutoScroll = true;
            mapaHidro.MapProvider = GMapProviders.GoogleTerrainMap;

            GMaps.Instance.Mode = AccessMode.ServerOnly;

            mapaHidro.Refresh();


            this.lblZoom.Text = "Coordenadas Seleccionadas: ";
        }

        private GMapOverlay CrearMarca(PointLatLng punto, EstacionAforoDTO estacion = null)
        {
            GMapOverlay markers = new GMapOverlay("markers");
            UTMCoordinate coord = Coordenadas.ConvertLatLngToUTM(punto.Lat, punto.Lng);

            coord.X = Math.Round(coord.X);
            coord.Y = Math.Round(coord.Y);

            GMarkerGoogleType tipoMarca = GMarkerGoogleType.red_dot;
            string tooltipText = string.Format("Punto seleccionado Manualmente \nUTM ETRS 89   X:{1} Y:{0} Huso 30N", coord.Y, coord.X);


            if (estacion != null)
            {
                tipoMarca = GMarkerGoogleType.blue_dot;
                tooltipText = string.Format("Estación de Aforos {0} en {4}\nUTM ETRS 89   X:{2} Y:{1} Huso 30N\n  Estado: {3}",
                                                       estacion.Nombre,
                                                       estacion.UTMY_H30_ETRS89,
                                                       estacion.UTMX_H30_ETRS89,
                                                       estacion.Estado,
                                                       estacion.Situacion_Estacion);
            }

            GMapMarker marca = new GMarkerGoogle(punto, tipoMarca)
            {
                ToolTipMode = MarkerTooltipMode.OnMouseOver,
                ToolTipText = tooltipText
            };

            marca.ToolTip = new GMapToolTip(marca)
            {
                Fill = new SolidBrush(Color.LightBlue),
                Foreground = new SolidBrush((Color.Black)),
                //Offset = new Point(100, -50),
                Stroke = new Pen(new SolidBrush(Color.Red)),
            };

            markers.Markers.Add(marca);

            return markers;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            mapaHidro.Zoom = trackBar1.Value;
        }

        private void mapaHidro_ZoomChanged()
        {
            trackBar1.Value = (int)mapaHidro.Zoom;
        }


        private void tb_BuscadorAforos_TextChanged(object sender, EventArgs e)
        {
            List<EstacionAforoDTO> listaFiltrada = listaAforos;
            string prefix = tb_BuscadorAforos.Text;

            if (!cbCuencaHidrografica.SelectedItem.Equals("Todas"))
                listaFiltrada = listaFiltrada.Where(x => x.Confederacion.Equals(cbCuencaHidrografica.SelectedItem)).ToList();

            listaFiltrada = listaFiltrada.Where(x => x.Nombre.ToLower().StartsWith(prefix.ToLower()) || 
                                                     x.Codigo.ToString().Contains(prefix)).ToList();


            if (listaFiltrada.Count < 1)
            {
                cb_ListaAforos.DataSource = null;
                cb_ListaAforos.Items.Clear();
                return;
            }

            cb_ListaAforos.DataSource = listaFiltrada;

        }

        private void tb_BuscadorAforos_Enter(object sender, EventArgs e)
        {
            //if (tb_BuscadorAforos.Text == "Buscar Estación...")
            //    tb_BuscadorAforos.Clear();

            //return;
        }

        private void cbCuencaHidrografica_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<EstacionAforoDTO> filtroCuencas = listaAforos;

            cb_ListaAforos.DataSource = filtroCuencas;
            cb_ListaAforos.ResetText();

            string filtroAforos = tb_BuscadorAforos.Text;
            //if (tb_BuscadorAforos.Text.Equals("Buscar Estación..."))
            //    filtroAforos = "";

            if (cbCuencaHidrografica.SelectedItem.Equals("Todas"))
                filtroCuencas = listaAforos;
            else
                filtroCuencas = listaAforos.Where(x => x.Confederacion.Equals(cbCuencaHidrografica.SelectedItem)).ToList();

            cb_ListaAforos.DataSource = filtroCuencas.Where(x => x.Nombre.ToLower().StartsWith(filtroAforos.ToLower()) ||
                                                                 x.Codigo.ToString().Contains(filtroAforos)).ToList();
        }

        private async void btnGetSimpa_Click(object sender, EventArgs e)
        {
            try
            {
                Automatizacion automatizacion = new Automatizacion();
                Form1 formulario1 = new Form1();
                Dictionary<string, DateTime> filePathsAndDates = new Dictionary<string, DateTime>();
                GenerarFicheroDatos _generarFicheroDatos = new GenerarFicheroDatos();
                List<DatosSimpaDTO> datosSimpaDTOs = new List<DatosSimpaDTO>();

                DialogResult isOK = formulario1.ShowDialog();

                if (isOK != DialogResult.OK)
                    return;

                HabilitarForm(false);

                //Recuperar datos del formulario de Configuración del punto
                System.Windows.Forms.TextBox ctrl_tbCodigoPunto = (System.Windows.Forms.TextBox)formulario1.Controls.Find("tbCodigoPunto", false).First();
                System.Windows.Forms.TextBox ctrl_tbProyecto = (System.Windows.Forms.TextBox)formulario1.Controls.Find("tbProyecto", false).First();                 
                DateTimePicker fechaIni = (DateTimePicker)formulario1.Controls.Find("dtPickerFechaIni", false).First();
                DateTimePicker fechaFin = (DateTimePicker)formulario1.Controls.Find("dtPickerFechaFin", false).First();
                DateTime fechaInicio = DateTime.Parse(fechaIni.Value.Month + "/" + fechaIni.Value.Year);
                DateTime fechaFinal = DateTime.Parse(fechaFin.Value.Month + "/" + fechaFin.Value.Year);

                //obtener todas las rutas de ficheros rar que queremos descargar.
                List<string> rutasFicheros = _motorDatos.ObtenerFicheros(fechaInicio, fechaFinal);
                try
                {
                    await DescargarFicherosMiteco(rutasFicheros);
                }catch (OutOfMemoryException ex)
                {
                    HabilitarForm(true);
                    return;
                }catch(FileNotFoundException fnfex)
                {
                    MessageBox.Show("El rango de fechas no está soportado por SIMPA", "Rango de fechas erroneo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    HabilitarForm(true);
                    return;
                    
                }
                //Descomprime todos los ficheros rar necesarios
                string pathDestino = Path.Combine(Directory.GetCurrentDirectory(), "DATA");
                string[] ficherosDescomprimidos = await DescomprimirFichero(rutasFicheros, pathDestino);

                //Añadir a cada fichero descomprimido las fechas de las que contiene datos
                foreach (var file in ficherosDescomprimidos)
                    filePathsAndDates.Add(file, DateTime.Parse(file.Split('\\')[file.Split('\\').Length - 1].Remove(0, 6).Split('.')[0].Replace('_', '/')));

                //Obtener ficheros comprendidos entre las fechas indicadas por el usuario.
                filePathsAndDates = filePathsAndDates.Where(kv => kv.Value >= fechaInicio && kv.Value <= fechaFinal).ToDictionary(pair => pair.Key, pair => pair.Value);              
                filePathsAndDates = filePathsAndDates.OrderBy(kv => kv.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                
                int completed = 0;
                _progresoDatosForm.Show();
                _progresoDatosForm.UseWaitCursor = true;


                foreach (var item in filePathsAndDates)
                {
                    //Crea una lista con los datos obtenidos de los ficheros descomprimidos
                    datosSimpaDTOs.Add(
                        new DatosSimpaDTO(item.Value.Year, item.Value.Month, _motorDatos.GetAportacionesRaster(coordenadasSeleccionadas, item.Key))
                      );

                    double percentage = (double)completed / filePathsAndDates.Count;

                    completed++;

                    _progresoDatosForm.ActualizarEtiquetaArchivo(item.Key.Split('\\')[item.Key.Split('\\').Length - 1]);
                    _progresoDatosForm.ActualizarEtiquetaPorcentaje((int)(percentage * 100) + "%");
                    _progresoDatosForm.ActualizarEtiquetaDatos(completed.ToString() + "/" + filePathsAndDates.Count.ToString());
                    _progresoDatosForm.progressBarDatos.Maximum = filePathsAndDates.Count;
                    _progresoDatosForm.progressBarDatos.Value = (int)(completed);
                    _progresoDatosForm.Update();

                }
                _progresoDatosForm.Hide();

                //Ruta guardar fichero csv con datos obtenidos.
                DialogResult resp = DialogResult.Cancel;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog()
                {
                    Filter = "Listas de datos (*.csv)|*.csv",
                    Title = "Guardar datos del punto en CSV",
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    FileName = ctrl_tbCodigoPunto.Text + "_DatosSimpa_" + datosSimpaDTOs.First().Mes + "-" + datosSimpaDTOs.First().Año + "_" + datosSimpaDTOs.Last().Mes + "-" + datosSimpaDTOs.Last().Año + ".csv"
                };
                
                while (saveFileDialog1.ShowDialog() != DialogResult.OK)
                {              
                    resp = MessageBox.Show("¿Está seguro que desea cancelar el proceso?", "Cancelar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (resp == DialogResult.OK)
                    {
                        MessageBox.Show("Proceso Cancelado Correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HabilitarForm(true);
                        return;
                    }                    
                }

                //Crear fichero de datos de aportaciones en csv
                string rutaFicheroDatos = _generarFicheroDatos.Exportar(datosSimpaDTOs, ctrl_tbCodigoPunto.Text, "csv", saveFileDialog1.FileName);    
                string nombrePunto = ctrl_tbCodigoPunto.Text.ToUpperInvariant();
                string descPunto = "Punto Datos SIMPA";

                    //GenerarFicherosBAT generarFicheroBat = new GenerarFicherosBAT(rutaFicheroBat, true, false);

                    ArchivoDatos punto = ArchivoDatos.GetPunto(rutaFicheroDatos, ctrl_tbCodigoPunto.Text.ToUpperInvariant(), "Punto Datos SIMPA");



                    List<ArchivoDatos> listaPuntos = new List<ArchivoDatos>() { punto };

                    Dictionary<string,string> parametros = new Dictionary<string,string>();

                    parametros.Add("CD","");
                    parametros.Add("/t", "P");
                    parametros.Add("/np", punto.NombrePunto);
                    parametros.Add("/d", punto.Descripcion);
                    parametros.Add("/p", ctrl_tbProyecto.Text.ToUpperInvariant());
                    parametros.Add("/dp", "Proyecto Datos SIMPA");
                    parametros.Add("/rn", "Natural");
                    parametros.Add("/ra", "nat");
                    parametros.Add("/fe", rutaFicheroDatos);
                    parametros.Add("/mi", "10");



                automatizacion._traductor = _traductor;
                automatizacion._cMDB = _cMDB;
                automatizacion._tFechas = _tFechas;


                    automatizacion.CrearYCargarEnPunto(punto.NombrePunto, punto.Descripcion, ctrl_tbProyecto.Text.ToUpperInvariant(),
                        "Proyecto Datos SIMPA","Natural","nat", rutaFicheroDatos,"10");


                //Termina y habilita formulario.
                MessageBox.Show("Proceso Finalizado Correctamente. \n Fichero de datos csv: " + rutaFicheroDatos, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);     
                HabilitarForm(true);

            }
            catch(System.IO.IOException ex)
            {
                MessageBox.Show("Espacio en disco insuficiente. La descarga de estos datos requiere varios gigabits libres en disco","Espacio insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                HabilitarForm(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ":" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                HabilitarForm(true);
            }

        }

        private async Task<string[]> DescomprimirFichero(List<string> rutasFicheros, string pathDestino)
        {
            try
            {
                Descomprimir._progresoDescompresionForm.Show();
            }catch(Exception E)
            {
                Descomprimir._progresoDescompresionForm = new ProgresoDescompresionForm();
                Descomprimir._progresoDescompresionForm.Show();
            }
            Descomprimir._progresoDescompresionForm.UseWaitCursor = true;

            foreach (var rutaFichero in rutasFicheros)
            {
                Descomprimir._progresoDescompresionForm.ActualizarEtiquetaArchivo(rutaFichero.Split('\\')[rutaFichero.Split('\\').Length - 1]);
         
                await Descomprimir.DescomprimirRar(rutaFichero);
            }

            Descomprimir._progresoDescompresionForm.Hide();

            return Directory.GetFiles(pathDestino, "*.asc", SearchOption.TopDirectoryOnly);

        }


        private void HabilitarForm(bool habilitar)
        {
            foreach (Control c in this.Controls)
                c.Enabled = habilitar;
        }

        private async Task DescargarFicherosMiteco(List<string> rutasFicheros)
        {
            List<string> ficherosSinActualizar = new List<string>();
            DialogResult result = DialogResult.None;

            try
            {
                //Comprobar si cada fichero está actualizado. Sino, pregunta si queremos actualizarlos 
                foreach (string ruta in rutasFicheros)
                    if (!_gestorDescargas.FicheroActualizado(ruta))
                        ficherosSinActualizar.Add(ruta);

                if (ficherosSinActualizar.Count > 0)
                {
                    string mensajeFicheros = "";

                    foreach (var item in ficherosSinActualizar)
                    {
                        int i = item.LastIndexOf('\\');
                        mensajeFicheros += item.Substring(i + 1) + "\n";
                    }

                    result = MessageBox.Show("Ficheros no actualizados:\n" + mensajeFicheros +
                                    "¿Desea Actualizarlos?", "Información", MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Information);
                }

                //Si pulsa OK -> actualiza ficheros. Si cancela, continua con ficheros desactualizados.
                if (result == DialogResult.OK)
                    try
                    {
                        await Descargar(ficherosSinActualizar);
                    }catch(OutOfMemoryException e)
                    {
                        throw e;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Método encargada de llamar al metodo descargar fichero del gestor de descargas.
        private async Task Descargar(List<string> archivos)
        {
            try
            {
                WebClient myWebClient = new WebClient();

                _progresoDescargaForm.Show();
                _progresoDescargaForm.UseWaitCursor = true;

                myWebClient.DownloadProgressChanged += _progresoDescargaForm.eDownloadProgressChanged;

                foreach (var item in archivos)
                {
                    _progresoDescargaForm.ActualizarEtiquetaArchivo("Descargando: " + item.Split('\\').Last());
                    try
                    {
                        await _gestorDescargas.IniciarDescarga(item, myWebClient);
                    }catch (OutOfMemoryException e)
                    {
                        _progresoDescargaForm.Hide();
                        throw e;
                    }
                }

                _progresoDescargaForm.Hide();

            }
            catch(Exception ex) 
            {
                if (typeof(OutOfMemoryException) != ex.GetType())
                {
                    MessageBox.Show(ex.StackTrace + " : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else { throw ex; }
            }

        }


        //private void panel1_Paint(object sender, PaintEventArgs e)
        //{
        //    ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.Blue, ButtonBorderStyle.Dashed);
        //}

        //private void panel2_Paint(object sender, PaintEventArgs e)
        //{
        //    ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.Blue, ButtonBorderStyle.Dashed);
        //}


        private void btnBuscarEst_Click(object sender, EventArgs e)
        {
            try
            {
                EstacionAforoDTO estacionSeleccionada = (EstacionAforoDTO)cb_ListaAforos.SelectedItem;

                if (estacionSeleccionada == null)
                    return;

                UTMCoordinate coordenaUTM_Estacion = estacionSeleccionada.ObtenerCoordenadasUTM();
                PointLatLng puntoEstacion = Coordenadas.ConvertUTMToLatLng(coordenaUTM_Estacion.X, coordenaUTM_Estacion.Y, coordenaUTM_Estacion.Zona, true);


                mapaHidro.Overlays.Clear();
                mapaHidro.Overlays.Add(CrearMarca(puntoEstacion, estacionSeleccionada));

                mapaHidro.Position = new PointLatLng(puntoEstacion.Lat, puntoEstacion.Lng);


                coordenadasSeleccionadas = coordenaUTM_Estacion;
                this.lblZoom.Text = string.Format("Coordenadas UTM ETRS 89 seleccionadas: X:{1} Y:{0} Huso:{2}N -  {3}",
                                                        coordenadasSeleccionadas.Y,
                                                        coordenadasSeleccionadas.X,
                                                        coordenadasSeleccionadas.Zona,
                                                        "Estación Seleccionada");
                this.btnGetSimpa.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + " : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnBuscarCoord_Click(object sender, EventArgs e)
        {
            //Validar campos de coordenadas
            string res = ValidarCoordendasUTM();
            if (!string.IsNullOrEmpty(res))
            {
                MessageBox.Show(res, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            double coordenadaX = double.Parse(this.tbCoord_X.Text);
            double coordenadaY = double.Parse(this.tbCoord_Y.Text);
            int zona = int.Parse(this.tbZona.Text);
            PointLatLng puntoLatLng = Coordenadas.ConvertUTMToLatLng(coordenadaX, coordenadaY, zona, true);

            

            mapaHidro.Overlays.Clear();
            mapaHidro.Overlays.Add(CrearMarca(puntoLatLng));

            mapaHidro.Position = new PointLatLng(puntoLatLng.Lat, puntoLatLng.Lng);

            coordenadasSeleccionadas = new UTMCoordinate(coordenadaX, coordenadaY, zona);
            this.lblZoom.Text = string.Format("Coordenadas UTM ETRS 89 seleccionadas: X:{1} Y:{0} Huso:{2}N -  {3}",
                                                    coordenadasSeleccionadas.Y,
                                                    coordenadasSeleccionadas.X,
                                                    coordenadasSeleccionadas.Zona,
                                                    "Punto seleccionado Manualmente");
            this.btnGetSimpa.Enabled = true;


        }


        private string ValidarCoordendasUTM()
        {

            if (!double.TryParse(this.tbCoord_X.Text, out double x))
                return "La coordenada X es incorrecta";

            if (!double.TryParse(this.tbCoord_Y.Text, out double y))
                return "La coordenada Y es incorrecta";

            if (!int.TryParse(this.tbZona.Text, out int z))
                return "El Huso es incorrecto";

            return "";
        }

        private void FormGMap_FormClosing(Object sender, FormClosingEventArgs e)
        {
            string pathData = Path.Combine(Directory.GetCurrentDirectory(), "DATA");
            try
            {
                string[] filePathsASC = Directory.GetFiles(pathData, "*.asc", SearchOption.AllDirectories);
                string[] filePathsPRJ = Directory.GetFiles(pathData, "*.prj", SearchOption.AllDirectories);
                foreach (string filePath in filePathsASC)
                {
                    File.Delete(filePath);
                }
                foreach (string filePath in filePathsPRJ)
                {
                    File.Delete(filePath);
                }
                Descomprimir._progresoDescompresionForm.Dispose();
            }
            catch { }
            _progresoDatosForm.Dispose();
        }
    }

}
