using CargaPorLotes.Logica;
using DatosSimpa.DTO;
using DatosSimpa.Logica;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatosSimpa
{
    public partial class FormGMaps : Form
    {
        private List<EstacionAforoDTO> listaAforos;
        private List<string> listaCuencas;
        private readonly MotorDatos _motorDatos;
        private readonly GestorDescargas _gestorDescargas;
        public static ProgresoDatosForm _progresoDatosForm = new ProgresoDatosForm();
        private UTMCoordinate coordenadasSeleccionadas;


        public FormGMaps()
        {
            InitializeComponent();
            _motorDatos = new MotorDatos();
            _gestorDescargas = new GestorDescargas();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGMap_FormClosing);
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

            WMSProvider customMap = WMSProvider.Instance;
            PureProjection prj = MercatorProjection.Instance;


            mapaHidro.Position = new PointLatLng(40.4427, -3.6309); // Madrid como ubicación inicial

            mapaHidro.CanDragMap = true;
            mapaHidro.DragButton = MouseButtons.Left;
            mapaHidro.MinZoom = 0;
            mapaHidro.MaxZoom = 29;
            mapaHidro.Zoom = 6;
            mapaHidro.AutoScroll = true;

            mapaHidro.MapProvider = GMapProviders.GoogleTerrainMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;

            //PureImageLayer customLayer = AñadirImagenWMS(customMap, prj);

            //mapaHidro.Overlays.Add(AñadirMarcasMapa());
            //mapaHidro.Overlays.Add(customLayer);

            mapaHidro.Refresh();


            this.lblZoom.Text = "Coordenadas: ";


        }

        private GMapOverlay CrearMarca(PointLatLng punto, EstacionAforoDTO estacion = null)
        {
            GMapOverlay markers = new GMapOverlay("markers");

            GMarkerGoogleType tipoMarca = GMarkerGoogleType.red;
            string tooltipText = string.Format("Punto seleccionado \n {0}",
                                               Coordenadas.ConvertLatLngToUTM(punto.Lat, punto.Lng));


            if (estacion != null)
            {
                tipoMarca = GMarkerGoogleType.blue;
                tooltipText = string.Format("{0} \nX:{1}   Y:{2} \nEstado: {3}",
                                                       estacion.Situacion_Estacion + " " + estacion.Nombre,
                                                       estacion.UTMY_H30_ETRS89,
                                                       estacion.UTMX_H30_ETRS89,
                                                       estacion.Estado);
            }

            GMapMarker marca = new GMarkerGoogle(punto, tipoMarca)
            {
                ToolTipMode = MarkerTooltipMode.OnMouseOver,
                ToolTipText = tooltipText,
            };

            marca.ToolTip = new GMapToolTip(marca)
            {
                Fill = new SolidBrush(Color.Blue),
                Foreground = new SolidBrush((Color.White)),
                Offset = new Point(100, -50),
                Stroke = new Pen(new SolidBrush(Color.Red)),
            };

            markers.Markers.Add(marca);

            return markers;
        }


        //Método no utilizado
        /*
        private PureImageLayer AñadirImagenWMS(WMSProvider customMap, PureProjection prj)
        {
            PureImageLayer customLayer = new PureImageLayer();

            // current area - Metida manualmente. (lat, long, widht, height)
            RectLatLng area = new RectLatLng(44.55352, -13.708288, 20.784, 8.875944);

            int zoom = (int)mapaHidro.Zoom;

            GPoint topLeftPx = prj.FromLatLngToPixel(area.LocationTopLeft, zoom);
            GPoint rightButtomPx = prj.FromLatLngToPixel(area.Bottom, area.Right, zoom);

            customMap.pos1 = topLeftPx;
            customMap.pos2 = rightButtomPx;

            GMapImage imagenMapa = (GMapImage)customMap.GetTileImage(topLeftPx, zoom);

            customLayer.Image = (Bitmap)imagenMapa.Img;
            customLayer.IsZoomSignificant = true;

            return customLayer;
        }
        */
   
        //Método no utilizado
        /*
        public void mapaHidro_OnMapZoomChanged()
        {
            WMSProvider customMap = WMSProvider.Instance;
            GMapImage a = (GMapImage)customMap.GetTileImage(new GPoint(1, 1), (int)mapaHidro.Zoom);

            PureImageLayer customLayer = new PureImageLayer();
            customLayer.Image = (Bitmap)a.Img;
            customLayer.IsZoomSignificant = false;
        }
        */

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
            if (tb_BuscadorAforos.Text == "Buscar Estación...")
                tb_BuscadorAforos.Clear();

            return;
        }

        private void cbCuencaHidrografica_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<EstacionAforoDTO> filtroCuencas = listaAforos;

            cb_ListaAforos.DataSource = filtroCuencas;
            cb_ListaAforos.ResetText();

            string filtroAforos = tb_BuscadorAforos.Text;
            if (tb_BuscadorAforos.Text.Equals("Buscar Estación..."))
                filtroAforos = "";

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
                Form1 formulario1 = new Form1();
                DialogResult isOK = formulario1.ShowDialog();

                TextBox ctrl_tbCodigoPunto = (TextBox)formulario1.Controls.Find("tbCodigoPunto", false).First();
                TextBox ctrl_tbProyecto = (TextBox)formulario1.Controls.Find("tbProyecto", false).First();              


                DateTimePicker fechaIni = (DateTimePicker)formulario1.Controls.Find("dtPickerFechaIni", false).First();
                DateTimePicker fechaFin = (DateTimePicker)formulario1.Controls.Find("dtPickerFechaFin", false).First();

                DateTime fechaInicio = DateTime.Parse(fechaIni.Value.Month + "/" + fechaIni.Value.Year);
                DateTime fechaFinal = DateTime.Parse(fechaFin.Value.Month + "/" + fechaFin.Value.Year);


                //obtener todas las rutas de ficheros rar.
                List<string> rutasFicheros = _motorDatos.ObtenerFicheros(fechaInicio, fechaFinal);

                //Comprobar si cada fichero está actualizado.
                await CheckFicherosActualizados(rutasFicheros);

                foreach (var rutaFichero in rutasFicheros)
                    await Descomprimir.DescomprimirRar(rutaFichero);


                Descomprimir._progresoDescompresionForm.Hide();

                Dictionary<string, DateTime> filePathsAndDates = new Dictionary<string, DateTime>();
                string pathData = Path.Combine(Directory.GetCurrentDirectory(), "DATA");

                string[] files = Directory.GetFiles(pathData, "*.asc", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                    filePathsAndDates.Add(file, DateTime.Parse(file.Split('\\')[file.Split('\\').Length - 1].Remove(0, 6).Split('.')[0].Replace('_', '/')));

                filePathsAndDates = filePathsAndDates.Where(kv => kv.Value >= fechaInicio && kv.Value <= fechaFinal).ToDictionary(pair => pair.Key, pair => pair.Value);
                
                filePathsAndDates = filePathsAndDates.OrderBy(kv => kv.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                int completed = 0;
                _progresoDatosForm.Show();
                _progresoDatosForm.UseWaitCursor = true;

                List<DatosSimpaDTO> datosSimpaDTOs = new List<DatosSimpaDTO>();

                foreach (var item in filePathsAndDates)
                {

                    datosSimpaDTOs.Add(
                        new DatosSimpaDTO(item.Value.Year, item.Value.Month, _motorDatos.ObtAportaciones(coordenadasSeleccionadas, item.Key))
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

                string rutaFicheroBat = AppDomain.CurrentDomain.BaseDirectory + "temp.bat";
                GenerarFicheroDatos _generarFicheroDatos = new GenerarFicheroDatos();

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Listas de datos (*.csv)|*.csv";
                saveFileDialog1.Title = "Guardar archivo CSV";
                saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                saveFileDialog1.FileName = ctrl_tbCodigoPunto.Text + "_DatosSimpa_" + datosSimpaDTOs.First().Mes + "-" + datosSimpaDTOs.First().Año + "_" + datosSimpaDTOs.Last().Mes + "-" + datosSimpaDTOs.Last().Año + ".csv";


                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string rutaFicheroDatos = _generarFicheroDatos.Exportar(datosSimpaDTOs, ctrl_tbCodigoPunto.Text, "csv", saveFileDialog1.FileName);

                    GenerarFicherosBAT generarFicheroBat = new GenerarFicherosBAT(rutaFicheroBat, true, false);

                    ArchivoDatos punto = ArchivoDatos.GetPunto(rutaFicheroDatos, ctrl_tbCodigoPunto.Text.ToUpperInvariant(), "Punto Datos Simpa");
                    


                    List<ArchivoDatos> listaPuntos = new List<ArchivoDatos>() { punto };

                    generarFicheroBat.CrearFicheroBat(listaPuntos, ctrl_tbProyecto.Text.ToUpperInvariant(), "Proy. Datos Simpa", false, false, true);

                    try
                    {
                        var procesoEjecutar = EjecutarFicheroBat.Ejecutar(rutaFicheroBat);
                        File.Delete(rutaFicheroBat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR Procesando fichero. " + ex.Message);
                        return;
                    }


                    MessageBox.Show("Proceso Finalizado Correctamente. \n Fichero de datos csv: " + rutaFicheroDatos);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }

        }

        private async Task CheckFicherosActualizados(List<string> rutasFicheros)
        {
            List<string> ficherosSinActualizar = new List<string>();
            DialogResult result = DialogResult.None;

            try
            {
                foreach (string ruta in rutasFicheros)
                    if (!_gestorDescargas.FicheroActualizado(ruta))
                        ficherosSinActualizar.Add(ruta);

                //Si hay ficheros sin actualizar, preguntar si se desea actualizarlos.
                if (ficherosSinActualizar.Count > 0)
                {
                    string mensajeFicheros = "";

                    foreach (var item in ficherosSinActualizar)
                    {
                        int i = item.LastIndexOf('\\');
                        mensajeFicheros += item.Substring(i + 1) + "\n";
                        //mensajeFicheros += item.ToString()+"\n";
                    }


                    result = MessageBox.Show("Ficheros no actualizados:\n" + mensajeFicheros +
                                    "¿Desea Actualizarlos?", "Información", MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Information);
                }

                //Si pulsa OK -> actualiza ficheros.
                //Si cancela, continua con ficheros desactualizados.
                if (result == DialogResult.OK)
                    await Descargar(ficherosSinActualizar);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private async Task Descargar(List<string> archivos)
        {
            foreach (var item in archivos)
                await _gestorDescargas.IniciarDescarga(item);


            GestorDescargas._progresoDescargaForm.Dispose();
        }

        // Clase para la capa PureImage personalizada
        public class PureImageLayer : GMapOverlay
        {
            public Bitmap Image { get; set; }

            public override void OnRender(Graphics g)
            {
                if (Image != null)
                {
                    g.DrawImageUnscaled(Image, -400, -200);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.Blue, ButtonBorderStyle.Dashed);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.Blue, ButtonBorderStyle.Dashed);
        }

        private void btnBuscarCoord_Click(object sender, EventArgs e)
        {
            //Validar campos de coordenadas
            string res = ValidarCoordendasUTM();
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show(res, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            double coordenadaX = double.Parse(this.tbCoord_X.Text);
            double coordenadaY = double.Parse(this.tbCoord_Y.Text);
            int zona = int.Parse(this.tbZona.Text);
            PointLatLng puntoLatLng = Coordenadas.ConvertUTMToLatLng(coordenadaX, coordenadaY, 30, true);
            mapaHidro.Position = new PointLatLng(puntoLatLng.Lat, puntoLatLng.Lng);
            coordenadasSeleccionadas = new UTMCoordinate(coordenadaX, coordenadaY, zona);

            mapaHidro.Overlays.Clear();
            mapaHidro.Overlays.Add(CrearMarca(puntoLatLng));
        }

        private string ValidarCoordendasUTM()
        {

            if (!double.TryParse(this.tbCoord_X.Text, out double x))
                return "La coordenada X es incorrecta";

            if (!double.TryParse(this.tbCoord_Y.Text, out double y))
                return "La coordenada Y es incorrecta";

            if (!int.TryParse(this.tbZona.Text, out int z))
                return "la Zona es incorrecta";

            return "";
        }

        private void btnBuscarEst_Click(object sender, EventArgs e)
        {
            EstacionAforoDTO estacionSeleccionada = (EstacionAforoDTO)cb_ListaAforos.SelectedItem;

            if (estacionSeleccionada == null)
                return;

            UTMCoordinate coordenaUTM_Estacion = estacionSeleccionada.ObtenerCoordenadasUTM();
            PointLatLng puntoEstacion = Coordenadas.ConvertUTMToLatLng(coordenaUTM_Estacion.X, coordenaUTM_Estacion.Y, coordenaUTM_Estacion.Zona, true);

            coordenadasSeleccionadas = coordenaUTM_Estacion;

            mapaHidro.Overlays.Clear();
            mapaHidro.Overlays.Add(CrearMarca(puntoEstacion, estacionSeleccionada));

            mapaHidro.Position = new PointLatLng(puntoEstacion.Lat, puntoEstacion.Lng);

            
        }

        private void FormGMap_FormClosing(Object sender, FormClosingEventArgs e)
        {
            string pathData = Path.Combine(Directory.GetCurrentDirectory(), "DATA");

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
            _progresoDatosForm.Dispose();
        }
    }

}
