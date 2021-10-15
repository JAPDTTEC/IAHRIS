using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IAHRIS.Calculo;
using Microsoft.VisualBasic.CompilerServices;

namespace IAHRIS
{
    public partial class FormCalculo
    {
        public FormCalculo()
        {
            InitializeComponent();
        }

        public FormCalculo(TestFechas.Simulacion simulacion, TestFechas.GeneracionInformes informes, BBDD.OleDbDataBase cMDB)
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.

            // Poner las cosas en su sitio
            _simulacion = simulacion;
            _cMDB = cMDB;
            _informes = informes;
            var pb = new PictureBox();
            pb.Size = new Size(50, 50);
            pb.Location = new Point(20, 20);
            pb.Image = new Bitmap(My.Resources.Resources.wait30trans);
            Controls.Add(pb);
            Form argform = this;
            _traductor = new MultiLangXML.MultiIdiomasXML(ref argform);
            _traductor.traducirFormPorConf(Application.StartupPath, @"\conf.xml");
        }

        private TestFechas.Simulacion _simulacion;
        private BBDD.OleDbDataBase _cMDB;
        private Calculo.ReportController _reportController;
        private Calculo.DatosCalculo _datos;
        private TestFechas.GeneracionInformes _informes;
        private MultiLangXML.MultiIdiomasXML _traductor;

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // ++++++++ Realizar Calculos ++++++++++++++++++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            My.MyProject.Application.DoEvents();
            _reportController = new Calculo.ReportController(_datos, _simulacion.usarCoeDiara, _traductor);

            // La cabecera se escribe al final, para que siempre aparezca la cabecera al abrir fichero Excel
            // Me._calculo.EscribirCabecera(Me._simulacion, Me._informes)

            if (_informes.inf1)
            {
                _reportController.GenerarInforme1();
            }

            if (_informes.inf1a)
            {
                _reportController.GenerarInforme1a();
            }
            if (_informes.inf1b)
            {
                _reportController.GenerarInforme1b();
            }

            if (_informes.inf2a)
            {
                _reportController.GenerarInforme2a();
            }


            if (_informes.inf2)
            {
                _reportController.GenerarInforme2();
            }
            if (_informes.inf3a)
            {
                _reportController.GenerarInforme3a();
            }

            if (_informes.inf3)
            {
                _reportController.generarInforme3();
            }
            
            if (_informes.inf3b)
            {
                _reportController.GenerarInforme3b();
            }
            if (_informes.inf4)
            {
                _reportController.GenerarInforme4();
            }

            if (_informes.inf4a)
            {
                _reportController.GenerarInforme4a();
            }

            //if (_informes.inf4b)
            //{
            //    _reportController.GenerarInforme4b();
            //}

          
            if (_informes.inf5)
            {
                _reportController.GenerarInforme5();
            }

            if (_informes.inf5a)
            {
                _reportController.GenerarInforme5a();
            }

            if (_informes.inf5b)
            {
                _reportController.GenerarInforme5b();
            }

            //if (_informes.inf5c)
            //{
            //    _reportController.GenerarInforme5c();
            //}

            if (_informes.inf6)
            {
                _reportController.GenerarInforme6();
            }

            if (_informes.inf6a)
            {
                _reportController.GenerarInforme6a();
            }
            if (_informes.inf6b)
            {
                _reportController.GenerarInforme6b();
            }
            if (_informes.inf6c)
            {
                _reportController.GenerarInforme6c();
            }
            if (_informes.inf6d)
            {
                _reportController.GenerarInforme6d();
            }
            if (_informes.inf6e)
            {
                _reportController.GenerarInforme6e();
            }

            if (_informes.inf7a)
            {
                _reportController.GenerarInforme7a();
            }

            if (_informes.inf7b)
            {
                _reportController.GenerarInforme7b();
            }

            if (_informes.inf7c)
            {
                _reportController.GenerarInforme7c();
            }

            if (_informes.inf7d)
            {
                _reportController.GenerarInforme7d();
            }

            if (_informes.inf8)
            {
                _reportController.GenerarInforme8();
            }

            if (_informes.inf8a)
            {
                _reportController.GenerarInforme8a();
            }

            if (_informes.inf8b)
            {
                _reportController.GenerarInforme8b();
            }

            if (_informes.inf8c)
            {
                _reportController.GenerarInforme8c();
            }

            if (_informes.inf8d)
            {
                _reportController.GenerarInforme8d();
            }

            if (_informes.inf9)
            {
                _reportController.GenerarInforme9();
            }

            if (_informes.inf9a)
            {
                _reportController.GenerarInforme9a();
            }
            if (_informes.inf9b)
            {
                _reportController.GenerarInforme9b();
            }
            if (_informes.inf10a)
            {
                _reportController.GenerarInforme10a();
            }
            if (_informes.inf10b)
            {
                _reportController.GenerarInforme10b();
            }
            if (_informes.inf10c)
            {
                _reportController.GenerarInforme10c();
            }
            if (_informes.inf10d)
            {
                _reportController.GenerarInforme10d();
            }
            // ----------------------------------
            // Borrar hojas no usadas
            // ----------------------------------

            if (!_informes.inf10d)
            {
                _reportController.BorrarInforme10d();
            }
            if (!_informes.inf10c)
            {
                _reportController.BorrarInforme10c();
            }
            if (!_informes.inf10b)
            {
                _reportController.BorrarInforme10b();
            }
            if (!_informes.inf10a)
            {
                _reportController.BorrarInforme10a();
            }
            if (!_informes.inf9b)
            {
                _reportController.BorrarInforme9b();
            }
            if (!_informes.inf9a)
            {
                _reportController.BorrarInforme9a();
            }

            if (!_informes.inf9)
            {
                _reportController.BorrarInforme9();
            }

            if (!_informes.inf8d)
            {
                _reportController.BorrarInforme8d();
            }

            if (!_informes.inf8c)
            {
                _reportController.BorrarInforme8c();
            }

            if (!_informes.inf8b)
            {
                _reportController.BorrarInforme8b();
            }

            if (!_informes.inf8a)
            {
                _reportController.BorrarInforme8a();
            }

            if (!_informes.inf8)
            {
                _reportController.BorrarInforme8();
            }

            if (!_informes.inf7d)
            {
                _reportController.BorrarInforme7d();
            }

            if (!_informes.inf7c)
            {
                _reportController.BorrarInforme7c();
            }

            if (!_informes.inf7b)
            {
                _reportController.BorrarInforme7b();
            }

            if (!_informes.inf7a)
            {
                _reportController.BorrarInforme7a();
            }

            if (!_informes.inf6e)
            {
                _reportController.BorrarInforme6e();
            }
            if (!_informes.inf6d)
            {
                _reportController.BorrarInforme6d();
            }
            if (!_informes.inf6c)
            {
                _reportController.BorrarInforme6c();
            }
            if (!_informes.inf6b)
            {
                _reportController.BorrarInforme6b();
            }

            if (!_informes.inf6a)
            {
                _reportController.BorrarInforme6a();
            }

            if (!_informes.inf6)
            {
                _reportController.BorrarInforme6();
            }

            //if (!_informes.inf5c)
            //{
            //    _reportController.BorrarInforme5c();
            //}

            if (!_informes.inf5b)
            {
                _reportController.BorrarInforme5b();
            }

            if (!_informes.inf5a)
            {
                _reportController.BorrarInforme5a();
            }

            if (!_informes.inf5)
            {
                _reportController.BorrarInforme5();
            }

            //if (!_informes.inf4b)
            //{
            //    _reportController.BorrarInforme4b();
            //}

            if (!_informes.inf4a)
            {
                _reportController.BorrarInforme4a();
            }

            if (!_informes.inf4)
            {
                _reportController.BorrarInforme4();
            }
            if (!_informes.inf3b)
            {
                _reportController.BorrarInforme3b();
            }
            if (!_informes.inf3a)
            {
                _reportController.BorrarInforme3a();
            }

            if (!_informes.inf3)
            {
                _reportController.BorrarInforme3();
            }
            if (!_informes.inf2a)
            {
                _reportController.BorrarInforme2a();
            }
            if (!_informes.inf2)
            {
                _reportController.BorrarInforme2();
            }
            if (!_informes.inf1b)
            {
                _reportController.BorrarInforme1b();
            }
            if (!_informes.inf1a)
            {
                _reportController.BorrarInforme1a();
            }
            if (!_informes.inf1)
            {
                _reportController.BorrarInforme1();
            }
            _reportController.EscribirCabecera(_simulacion, _informes);


            // Esto escribe realmente el fichero Excel nuevo
            _reportController.EscribirFichero(_simulacion.usarCoeDiara, _simulacion.usarCoe);
            _reportController = null;
            Close();
        }

        private void FormCalculo_Load(object sender, EventArgs e)
        {

            // Centrar el formulario
            Left = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Width - Width));
            Top = (int)(0.5d * (Screen.PrimaryScreen.Bounds.Height - Height));
            Cursor = Cursors.WaitCursor;

            // ---------------------------------------------------------------------------------------
            // ----------- Rellenar las listas de datos ----------------------------------------------
            // ---------------------------------------------------------------------------------------
            // ----- Se lee la simulacion que nos indica que hay que leer
            // ----- Se lee de la BBDD en access en cualquier caso
            // ---------------------------------------------------------------------------------------
            int año;
            var datos = default(Calculo.DatosCalculo);
            datos.SerieNatDiaria.nAños = 0;
            datos.SerieAltDiaria.nAños = 0;
            datos.SerieNatMensual.nAños = 0;
            datos.SerieAltMensual.nAños = 0;
            datos.SerieAltDiaria.dia = null;
            datos.SerieAltDiaria.caudalDiaria = null;
            datos.SerieNatDiaria.dia = null;
            datos.SerieNatDiaria.caudalDiaria = null;
            datos.SerieAltMensual.mes = null;
            datos.SerieAltMensual.caudalMensual = null;
            datos.SerieNatMensual.mes = null;
            datos.SerieNatMensual.caudalMensual = null;
            int posNatINI;
            int posNatFIN;
            int posAltINI;
            int posAltFIN;
            datos.sNombre = _simulacion.sNombre;
            datos.sAlteracion = _simulacion.sAlteracion;
            datos.mesInicio = _simulacion.mesInicio;
            datos._simulacion= _simulacion;
            // ----------------------------------------------------------------
            // Recorrer los años de la simulacion
            // ----------------------------------
            // -- Hay que quitar un año porque indica el ultimo año que tiene 
            // -- fechas en el sistema no el años hidrologico
            // 
            // -- "simulacion": Datos almacenado al testear
            // -- "datos": Datos que voy a pasar al calculo
            // -----------------------------------------------------------------
            var loopTo = _simulacion.fechaFIN - 1;
            for (año = _simulacion.fechaINI; año <= loopTo; año++)
            {
                // +++++++++++++++++++++++++++++++++++++++++
                // +++ Serie NATURAL DIARIA ++++++++++++++++
                // +++++++++++++++++++++++++++++++++++++++++
                // +++ ¿Hay datos validos? +++++++++++++++++
                // +++ ¿Se usan los años en el calculo? ++++
                // +++++++++++++++++++++++++++++++++++++++++
                if (_simulacion.listas[0].nValidos > 0 & _simulacion.añosParaCalculo[0].nAños > 0)
                {
                    // Busco si el año se va a usar en el calculo
                    int pos = Array.BinarySearch(_simulacion.añosParaCalculo[0].año, año);
                    // Si se cumple este año entra en el calculo
                    if (pos >= 0)
                    {
                        DataSet ds;

                        // Definir el año hidrologico
                        // ------------------------
                        // Dim fechai As Date = New  Date(año, 10, 1)
                        // Dim fechaf As Date = New Date(año + 1, 9, 30)

                        int mesfinal;
                        int añofinal;
                        if (datos.mesInicio == 1)
                        {
                            mesfinal = 12;
                            añofinal = año;
                        }
                        else
                        {
                            mesfinal = datos.mesInicio - 1;
                            añofinal = año + 1;
                        }

                        var fechai = new DateTime(año, datos.mesInicio, 1);
                        var fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));

                        // Sacar los datos el año hidrologico de la lista 
                        ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + _simulacion.idListas[0] + " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");


                        // Esto es para la posible interpolacion. 
                        // Me indica el inicio del año en la lista y su fin
                        if (datos.SerieNatDiaria.dia is null)
                        {
                            posNatINI = 0;
                        }
                        else
                        {
                            posNatINI = datos.SerieNatDiaria.dia.Length;
                        }

                        // Meter los datos del año
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (datos.SerieNatDiaria.dia is null)
                            {
                                datos.SerieNatDiaria.caudalDiaria = new float[1];
                                datos.SerieNatDiaria.dia = new DateTime[1];
                            }
                            else
                            {
                                Array.Resize(ref datos.SerieNatDiaria.caudalDiaria, datos.SerieNatDiaria.caudalDiaria.Length + 1);
                                Array.Resize(ref datos.SerieNatDiaria.dia, datos.SerieNatDiaria.dia.Length + 1);
                            }

                            datos.SerieNatDiaria.caudalDiaria[datos.SerieNatDiaria.caudalDiaria.Length - 1] = float.Parse(Conversions.ToString(dr["valor"]));
                            datos.SerieNatDiaria.dia[datos.SerieNatDiaria.caudalDiaria.Length - 1] = DateTime.Parse(Conversions.ToString(dr["fecha"]));
                        }

                        datos.SerieNatDiaria.nAños = datos.SerieNatDiaria.nAños + 1;

                        // Esto es para la posible interpolacion. 
                        // Me indica el inicio del año en la lista y su fin
                        posNatFIN = datos.SerieNatDiaria.dia.Length - 1;
                    }
                }
                // +++++++++++++++++++++++++++++++++++++++++
                // +++ Serie ALTERADA DIARIA +++++++++++++++
                // +++++++++++++++++++++++++++++++++++++++++
                // +++ ¿Hay datos validos? +++++++++++++++++
                // +++ ¿Se usan los años en el calculo? ++++
                // +++++++++++++++++++++++++++++++++++++++++
                if (_simulacion.listas[1].nValidos > 0 & _simulacion.añosParaCalculo[1].nAños > 0)
                {
                    // Buscar si el año se incluye en el calculo
                    int pos = Array.BinarySearch(_simulacion.añosParaCalculo[1].año, año);
                    if (pos >= 0)
                    {
                        DataSet ds;

                        // Dim fechai As Date = New Date(año, 10, 1)
                        // Dim fechaf As Date = New Date(año + 1, 9, 30)

                        // Definir el año hidrológica
                        // ---------------------------
                        int mesfinal;
                        int añofinal;
                        if (datos.mesInicio == 1)
                        {
                            mesfinal = 12;
                            añofinal = año;
                        }
                        else
                        {
                            mesfinal = datos.mesInicio - 1;
                            añofinal = año + 1;
                        }

                        var fechai = new DateTime(año, datos.mesInicio, 1);
                        var fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));

                        // Sacar los datos el año hidrologico de la lista 
                        ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + _simulacion.idListas[1] + " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");


                        // Esto es para la posible interpolacion. 
                        // Me indica el inicio del año en la lista y su fin
                        if (datos.SerieAltDiaria.dia is null)
                        {
                            posAltINI = 0;
                        }
                        else
                        {
                            posAltINI = datos.SerieAltDiaria.dia.Length;
                        }

                        // Meter los datos asociados a los años
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (datos.SerieAltDiaria.dia is null)
                            {
                                datos.SerieAltDiaria.caudalDiaria = new float[1];
                                datos.SerieAltDiaria.dia = new DateTime[1];
                            }
                            else
                            {
                                Array.Resize(ref datos.SerieAltDiaria.caudalDiaria, datos.SerieAltDiaria.caudalDiaria.Length + 1);
                                Array.Resize(ref datos.SerieAltDiaria.dia, datos.SerieAltDiaria.dia.Length + 1);
                            }

                            datos.SerieAltDiaria.caudalDiaria[datos.SerieAltDiaria.caudalDiaria.Length - 1] = float.Parse(Conversions.ToString(dr["valor"]));
                            datos.SerieAltDiaria.dia[datos.SerieAltDiaria.caudalDiaria.Length - 1] = DateTime.Parse(Conversions.ToString(dr["fecha"]));
                        }

                        datos.SerieAltDiaria.nAños = datos.SerieAltDiaria.nAños + 1;

                        // Esto es para la posible interpolacion. 
                        // Me indica el inicio del año en la lista y su fin
                        posAltFIN = datos.SerieAltDiaria.dia.Length - 1;
                    }
                }

                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // ++++++++++++++ ATENCION: Los valores mensuales son APORTACIONES __NO__ CAUDALES +++++++++
                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                // +++++++++++++++++++++++++++++++++++++++++
                // +++ Serie NATURAL MENSUAL +++++++++++++++
                // +++++++++++++++++++++++++++++++++++++++++
                // +++ Tengo que mirar: Es valido?
                // +++   Si no -> Se puede interpolar?
                // +++    Si no se cumple, pues vacio
                // +++++++++++++++++++++++++++++++++++++++++
                if (_simulacion.añosParaCalculo[2].nAños > 0)
                {
                    int pos;
                    int posInt;
                    bool necesitaInterpolar = false;
                    pos = Array.BinarySearch(_simulacion.añosParaCalculo[2].año, año);
                    if (pos >= 0)
                    {
                        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        // +++ Comprobar si el dato viene de interpolar o de la BBDD
                        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        if (_simulacion.listas[2].nValidos > 0)
                        {
                            pos = Array.BinarySearch(_simulacion.listas[2].Año, año);
                            // No lo tengo, tengo que interpolar
                            if (pos < 0)
                            {
                                necesitaInterpolar = true;
                            }
                        }
                        else
                        {
                            necesitaInterpolar = true;
                        }

                        if (_simulacion.añosInterNat is null)
                        {
                            posInt = -1;
                        }
                        else
                        {
                            posInt = Array.BinarySearch(_simulacion.añosInterNat, año);
                        }

                        // Si necesita interpolar y esta en la lista de interpoladas
                        if (necesitaInterpolar & posInt >= 0)
                        {
                            // Realizar interpolacion desde la lista NATURAL DIARIA --> ERROR Ya que no sabemos si la tenemos en
                            // el sistema metida.
                            // Hay que leer de la BBDD y interpolar
                            float acum = 0f;

                            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            // +++++++++ ITERPOLACION DIRECTA DE LA BBDD ++++++++++++++
                            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            DataSet ds;

                            // Dim fechai As Date = New Date(año, 10, 1)
                            // Dim fechaf As Date = New Date(año + 1, 9, 30)

                            // Definir el año hidrológica
                            // ---------------------------
                            int mesfinal;
                            int añofinal;
                            if (datos.mesInicio == 1)
                            {
                                mesfinal = 12;
                                añofinal = año;
                            }
                            else
                            {
                                mesfinal = datos.mesInicio - 1;
                                añofinal = año + 1;
                            }

                            var fechai = new DateTime(año, datos.mesInicio, 1);
                            var fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));


                            // Sacar los datos el año hidrologico de la lista 
                            ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + _simulacion.idListas[0] + " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");


                            // ----------------------------------------------
                            // Meter los datos naturales del año mensualmente
                            // ----------------------------------------------
                            int mes = datos.mesInicio;
                            int añoAux = año;
                            DateTime fechaDia;
                            float valor;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                fechaDia = DateTime.Parse(Conversions.ToString(dr["fecha"]));
                                valor = float.Parse(Conversions.ToString(dr["valor"]));
                                // Saco todos los datos y se van acumulando en "acum".
                                // Cuando cambia el mes, se calcula la aportacion, y
                                // se almacena en la serie correspondiente.
                                if (mes != fechaDia.Month)
                                {
                                    // ¿La serie esta vacia o ya tiene datos?
                                    if (datos.SerieNatMensual.mes is null)
                                    {
                                        datos.SerieNatMensual.caudalMensual = new float[1];
                                        datos.SerieNatMensual.mes = new DateTime[1];
                                    }
                                    else
                                    {
                                        Array.Resize(ref datos.SerieNatMensual.caudalMensual, datos.SerieNatMensual.caudalMensual.Length + 1);
                                        Array.Resize(ref datos.SerieNatMensual.mes, datos.SerieNatMensual.mes.Length + 1);
                                    }
                                    // Almaceno los datos
                                    datos.SerieNatMensual.nMeses = datos.SerieNatMensual.nMeses + 1;
                                    datos.SerieNatMensual.caudalMensual[datos.SerieNatMensual.caudalMensual.Length - 1] = 86400f * acum / 1000000f;
                                    datos.SerieNatMensual.mes[datos.SerieNatMensual.mes.Length - 1] = new DateTime(añoAux, mes, 1);
                                    // Preparo el nuevo mes a almacenar
                                    acum = 0f;
                                    añoAux = fechaDia.Year;
                                    mes = fechaDia.Month;
                                }

                                acum = acum + valor;
                            }

                            Array.Resize(ref datos.SerieNatMensual.caudalMensual, datos.SerieNatMensual.caudalMensual.Length + 1);
                            Array.Resize(ref datos.SerieNatMensual.mes, datos.SerieNatMensual.mes.Length + 1);
                            datos.SerieNatMensual.nMeses = datos.SerieNatMensual.nMeses + 1;
                            datos.SerieNatMensual.caudalMensual[datos.SerieNatMensual.caudalMensual.Length - 1] = 86400f * acum / 1000000f;
                            datos.SerieNatMensual.mes[datos.SerieNatMensual.mes.Length - 1] = new DateTime(añoAux, mes, 1);
                            datos.SerieNatMensual.nAños = datos.SerieNatMensual.nAños + 1;
                        }
                        else if (!necesitaInterpolar)
                        {
                            // Leer de la BBDD
                            DataSet ds;

                            // Dim fechai As Date = New Date(año, 10, 1)
                            // Dim fechaf As Date = New Date(año + 1, 9, 30)

                            // Definir el año hidrológica
                            // ---------------------------
                            int mesfinal;
                            int añofinal;
                            if (datos.mesInicio == 1)
                            {
                                mesfinal = 12;
                                añofinal = año;
                            }
                            else
                            {
                                mesfinal = datos.mesInicio - 1;
                                añofinal = año + 1;
                            }

                            var fechai = new DateTime(año, datos.mesInicio, 1);
                            var fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));
                            ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + _simulacion.idListas[2] + " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (datos.SerieNatMensual.mes is null)
                                {
                                    datos.SerieNatMensual.caudalMensual = new float[1];
                                    datos.SerieNatMensual.mes = new DateTime[1];
                                }
                                else
                                {
                                    Array.Resize(ref datos.SerieNatMensual.caudalMensual, datos.SerieNatMensual.caudalMensual.Length + 1);
                                    Array.Resize(ref datos.SerieNatMensual.mes, datos.SerieNatMensual.mes.Length + 1);
                                }

                                datos.SerieNatMensual.nMeses = datos.SerieNatMensual.nMeses + 1;
                                datos.SerieNatMensual.caudalMensual[datos.SerieNatMensual.caudalMensual.Length - 1] = float.Parse(Conversions.ToString(dr["valor"]));
                                datos.SerieNatMensual.mes[datos.SerieNatMensual.mes.Length - 1] = DateTime.Parse(Conversions.ToString(dr["fecha"]));
                            }

                            datos.SerieNatMensual.nAños = datos.SerieNatMensual.nAños + 1;
                        } // Interpolar o no
                    } // Es año participante
                } // Es año valido

                // +++++++++++++++++++++++++++++++++++++++++
                // +++ Serie ALTERADA MENSUAL +++++++++++++++
                // +++++++++++++++++++++++++++++++++++++++++
                // +++ Tengo que mirar: Es valido?
                // +++   Si no -> Se puede interpolar?
                // +++    Si no se cumple, pues vacio
                // +++++++++++++++++++++++++++++++++++++++++
                if (_simulacion.añosParaCalculo[3].nAños > 0)
                {
                    int pos;
                    int posInt;
                    bool necesitaInterpolar = false;
                    pos = Array.BinarySearch(_simulacion.añosParaCalculo[3].año, año);
                    if (pos >= 0)
                    {
                        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        // +++ Comprobar si el dato viene de interpolar o de la BBDD
                        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        if (_simulacion.listas[3].nValidos > 0)
                        {
                            pos = Array.BinarySearch(_simulacion.listas[3].Año, año);
                            // No lo tengo, tengo que interpolar
                            if (pos < 0)
                            {
                                necesitaInterpolar = true;
                            }
                        }
                        else
                        {
                            necesitaInterpolar = true;
                        }

                        if (_simulacion.añosInterAlt is null)
                        {
                            posInt = -1;
                        }
                        else
                        {
                            posInt = Array.BinarySearch(_simulacion.añosInterAlt, año);
                        }

                        // Si necesita interpolar y esta en la lista de interpoladas
                        if (necesitaInterpolar & posInt >= 0)
                        {
                            // Realizar interpolacion desde la lista NATURAL DIARIA
                            float acum = 0f;

                            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            // +++++++++ ITERPOLACION DIRECTA DE LA BBDD ++++++++++++++
                            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            DataSet ds;

                            // Dim fechai As Date = New Date(año, 10, 1)
                            // Dim fechaf As Date = New Date(año + 1, 9, 30)

                            // Definir el año hidrológica
                            // ---------------------------
                            int mesfinal;
                            int añofinal;
                            if (datos.mesInicio == 1)
                            {
                                mesfinal = 12;
                                añofinal = año;
                            }
                            else
                            {
                                mesfinal = datos.mesInicio - 1;
                                añofinal = año + 1;
                            }

                            var fechai = new DateTime(año, datos.mesInicio, 1);
                            var fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));

                            // Sacar los datos el año hidrologico de la lista 
                            ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + _simulacion.idListas[1] + " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");


                            // ----------------------------------------------
                            // Meter los datos naturales del año mensualmente
                            // ----------------------------------------------
                            int mes = datos.mesInicio;
                            int añoAux = año;
                            DateTime fechaDia;
                            float valor;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                fechaDia = DateTime.Parse(Conversions.ToString(dr["fecha"]));
                                valor = float.Parse(Conversions.ToString(dr["valor"]));
                                if (mes != fechaDia.Month)
                                {
                                    if (datos.SerieAltMensual.mes is null)
                                    {
                                        datos.SerieAltMensual.caudalMensual = new float[1];
                                        datos.SerieAltMensual.mes = new DateTime[1];
                                    }
                                    else
                                    {
                                        Array.Resize(ref datos.SerieAltMensual.caudalMensual, datos.SerieAltMensual.caudalMensual.Length + 1);
                                        Array.Resize(ref datos.SerieAltMensual.mes, datos.SerieAltMensual.mes.Length + 1);
                                    }

                                    datos.SerieAltMensual.nMeses = datos.SerieAltMensual.nMeses + 1;
                                    datos.SerieAltMensual.caudalMensual[datos.SerieAltMensual.caudalMensual.Length - 1] = 86400f * acum / 1000000f;
                                    datos.SerieAltMensual.mes[datos.SerieAltMensual.mes.Length - 1] = new DateTime(añoAux, mes, 1);
                                    acum = 0f;
                                    añoAux = fechaDia.Year;
                                    mes = fechaDia.Month;
                                }

                                acum = acum + valor;
                            }

                            Array.Resize(ref datos.SerieAltMensual.caudalMensual, datos.SerieAltMensual.caudalMensual.Length + 1);
                            Array.Resize(ref datos.SerieAltMensual.mes, datos.SerieAltMensual.mes.Length + 1);
                            datos.SerieAltMensual.nMeses = datos.SerieAltMensual.nMeses + 1;
                            datos.SerieAltMensual.caudalMensual[datos.SerieAltMensual.caudalMensual.Length - 1] = 86400f * acum / 1000000f;
                            datos.SerieAltMensual.mes[datos.SerieAltMensual.mes.Length - 1] = new DateTime(añoAux, mes, 1);
                            datos.SerieAltMensual.nAños = datos.SerieAltMensual.nAños + 1;
                        }
                        else if (!necesitaInterpolar)
                        {
                            // Leer de la BBDD
                            DataSet ds;

                            // Dim fechai As Date = New Date(año, 10, 1)
                            // Dim fechaf As Date = New Date(año + 1, 9, 30)

                            // Definir el año hidrológica
                            // ---------------------------
                            int mesfinal;
                            int añofinal;
                            if (datos.mesInicio == 1)
                            {
                                mesfinal = 12;
                                añofinal = año;
                            }
                            else
                            {
                                mesfinal = datos.mesInicio - 1;
                                añofinal = año + 1;
                            }

                            var fechai = new DateTime(año, datos.mesInicio, 1);
                            var fechaf = new DateTime(añofinal, mesfinal, DateTime.DaysInMonth(añofinal, mesfinal));
                            ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + _simulacion.idListas[3] + " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (datos.SerieAltMensual.mes is null)
                                {
                                    datos.SerieAltMensual.caudalMensual = new float[1];
                                    datos.SerieAltMensual.mes = new DateTime[1];
                                }
                                else
                                {
                                    Array.Resize(ref datos.SerieAltMensual.caudalMensual, datos.SerieAltMensual.caudalMensual.Length + 1);
                                    Array.Resize(ref datos.SerieAltMensual.mes, datos.SerieAltMensual.mes.Length + 1);
                                }

                                datos.SerieAltMensual.nMeses = datos.SerieAltMensual.nMeses + 1;
                                datos.SerieAltMensual.caudalMensual[datos.SerieAltMensual.caudalMensual.Length - 1] = float.Parse(Conversions.ToString(dr["valor"]));
                                datos.SerieAltMensual.mes[datos.SerieAltMensual.mes.Length - 1] = DateTime.Parse(Conversions.ToString(dr["fecha"]));
                            }

                            datos.SerieAltMensual.nAños = datos.SerieAltMensual.nAños + 1;
                        } // Interpolar o no
                    } // Es año participante
                } // Es año valido
            }

            if(_simulacion.Tipologia== TestFechas.Tipologias.Tipo6B || _simulacion.Tipologia == TestFechas.Tipologias.Tipo8B )
            {
                //Puede darse el caso de que los cálculos se realicen con mensual, pero existan datos diarios. Esta función comprueba eso y recupera los datos diarios de necesitarse:
                if (_datos.SerieAltDiaria.dia == null)
                {

                    
                    //BBDD.OleDbDataBase _cMDB;
                    //string _RutaBBDD = Application.StartupPath + @"\IAHRISv2.mdb";
                    //_cMDB = new BBDD.OleDbDataBase("Base", _RutaBBDD);
                    System.Data.DataSet ds;
                    var fechai = datos.SerieAltMensual.mes[0];
                    var fechaf = datos.SerieAltMensual.mes[datos.SerieAltMensual.mes.Count() - 1];
                    //Convertir fechaf a último día del mes
                    DateTime tmpfecha;
                    tmpfecha = fechaf.AddDays(31);
                    if(tmpfecha.Month!=fechaf.Month)
                    {
                        tmpfecha = fechaf.AddDays(30);
                        if (tmpfecha.Month != fechaf.Month)
                        {
                            tmpfecha = fechaf.AddDays(29);
                            if (tmpfecha.Month != fechaf.Month)
                            {
                                tmpfecha = fechaf.AddDays(28);
                            }
                        }
                    }
                    fechaf = tmpfecha;
                    // Sacar los datos el año hidrologico de la lista 
                    ds = _cMDB.RellenarDataSet("listas", "SELECT * FROM [Valor] WHERE id_Lista=" + _simulacion.idListas[1] + " AND fecha BETWEEN #" + fechai.ToString("yyyy-MM-dd") + "# AND #" + fechaf.ToString("yyyy-MM-dd") + "# ORDER BY fecha ASC");



                    // Meter los datos asociados a los años
                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {
                        //Comprobar que el mes y el año están incluídos en la serie
                        DateTime datFecha = DateTime.Parse(Conversions.ToString(dr["fecha"]));

                        if (datos.SerieAltMensual.mes.Where(x => x.Month == datFecha.Month & x.Year == datFecha.Year).Count() > 0)
                        {
                            if (datos.SerieAltDiaria.dia is null)
                            {
                                datos.SerieAltDiaria.caudalDiaria = new float[1];
                                datos.SerieAltDiaria.dia = new DateTime[1];
                            }
                            else
                            {
                                Array.Resize(ref datos.SerieAltDiaria.caudalDiaria, datos.SerieAltDiaria.caudalDiaria.Length + 1);
                                Array.Resize(ref datos.SerieAltDiaria.dia, datos.SerieAltDiaria.dia.Length + 1);
                            }

                            datos.SerieAltDiaria.caudalDiaria[datos.SerieAltDiaria.caudalDiaria.Length - 1] = float.Parse(Conversions.ToString(dr["valor"]));
                            datos.SerieAltDiaria.dia[datos.SerieAltDiaria.caudalDiaria.Length - 1] = DateTime.Parse(Conversions.ToString(dr["fecha"]));
                        }
                    }

                    datos.SerieAltDiaria.nAños = datos.SerieAltMensual.nAños;


                }
            }

            datos.nAnyosCoe = _simulacion.añosCoetaneosTotales;
            
            _datos = datos;
        }

        private void FormCalculo_Shown(object sender, EventArgs e)
        {
            My.MyProject.Application.DoEvents();
            btnCancelar_Click(null, null);
            Cursor = Cursors.Default;
        }
    }
}