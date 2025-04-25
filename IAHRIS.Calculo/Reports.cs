
using IAHRIS.Calculo.CaudalesEcologicos;
using IAHRIS.Calculo.CaudalesEcologicos.Informes;
using IAHRIS.Calculo.IndicesHidro.Informes;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS.Calculo
{
    public class Reports
    {
        private string _rutaExcel;
        private string _rutaExcelCE;
        private MultiLangXML.MultiIdiomasXML _traductor;
        private ExcelPackage m_Excel;
        ExcelWorkbook objWorkbook;



        public Reports(MultiLangXML.MultiIdiomasXML traductor)
        {
            _traductor = traductor;
            _rutaExcel = _traductor.getRutaExcel;
            _rutaExcelCE = _traductor.getRutaExcelCE;
        }


        public Reports()
        {
            // Eliminamos la instancia de Excel de memoria
            try
            {
                if (m_Excel != null)
                {
                    m_Excel.Dispose();
                    m_Excel = null;
                }
            }
            catch (Exception)
            {
            }
        }

        public void AbrirExcel()
        {

            FileInfo fExcel = new FileInfo(_rutaExcel);

            m_Excel = new ExcelPackage(fExcel);
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            objWorkbook = m_Excel.Workbook;



        }

        public ExcelPackage AbrirExcelCE()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fExcel = new FileInfo(_rutaExcelCE);

            m_Excel = new ExcelPackage(fExcel);
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:



            objWorkbook = m_Excel.Workbook;
            return m_Excel;
        }

        public void CerrarExcel(bool coeD, bool coeM, DatosCalculo datos)
        {

            string ruta = "";
            string nombre = "";
            var FolderBrowserDialog1 = new FolderBrowserDialog();


            nombre = datos.sNombre;
            nombre = nombre.Substring(0, nombre.LastIndexOf("-"));
            if (datos.SerieAltMensual.nAños != 0 | datos.SerieAltDiaria.nAños != 0)
            {
                nombre = nombre + "_" + datos.sAlteracion;
                nombre = nombre.Substring(0, nombre.LastIndexOf("-"));
                if (datos.SerieAltMensual.nAños != 0)
                {
                    if (coeM)
                    {
                        nombre = nombre + "_COEMSI";
                    }
                    else
                    {
                        nombre = nombre + "_COEMNO";
                    }
                }

                if (datos.SerieAltDiaria.nAños != 0)
                {
                    if (coeD)
                    {
                        nombre = nombre + "_COEDSI";
                    }
                    else
                    {
                        nombre = nombre + "_COEDNO";
                    }
                }
            }

            nombre = nombre + ".xlsx";
            try
            {
                // Configuración del FolderBrowserDialog  
                // With FolderBrowserDialog1


                var SaveFileDialog1 = new SaveFileDialog
                {

                    // openFileDialog1.InitialDirectory = "c:\"
                    Filter = "Archivos Office Open XML Hoja de Cálculo (*.xlsx)|*.xlsx|Todos los ficheros (*.*)|*.*",
                    FilterIndex = 1,
                    FileName = nombre,
                    OverwritePrompt = false
                };
                // openFileDialog1.RestoreDirectory = True

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ruta = SaveFileDialog1.FileName;
                }
                else
                {
                    return;
                }
            }
            catch (Exception oe)
            {
                Interaction.MsgBox(oe.Message, MsgBoxStyle.Critical);
            }

            if (File.Exists(ruta))
            {
                // Alerta por sobreescritura
                if (MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strOverwrite"), _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strAttention"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

                {
                    if (m_Excel != null)
                    {
                        m_Excel.Dispose();
                        m_Excel = null;
                    }

                    return;
                }
                else
                {
                    try
                    {
                        File.Delete(ruta);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


            // Tiene que salvarse como  Punto-Alt-COE-SI-SI.xls
            try
            {
                m_Excel.SaveAs(new FileInfo(ruta));
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strReportOk") + Microsoft.VisualBasic.Constants.vbCrLf + ruta, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNotSaveFile") + Microsoft.VisualBasic.Constants.vbCrLf + ruta + Microsoft.VisualBasic.Constants.vbCrLf + ex.Message, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            // Eliminamos la instancia de Excel de memoria
            if (m_Excel != null)
            {
                m_Excel.Dispose();
                m_Excel = null;
            }
        }

        public void CerrarExcelCmd(bool coeD, bool coeM, DatosCalculo datos, string ruta)
        {


            string nombre;



            nombre = datos.sNombre;
            nombre = nombre.Substring(0, nombre.LastIndexOf("-"));
            if (datos.SerieAltMensual.nAños != 0 | datos.SerieAltDiaria.nAños != 0)
            {
                nombre = nombre + "_" + datos.sAlteracion;
                nombre = nombre.Substring(0, nombre.LastIndexOf("-"));
                if (datos.SerieAltMensual.nAños != 0)
                {
                    if (coeM)
                    {
                        nombre = nombre + "_COEMSI";
                    }
                    else
                    {
                        nombre = nombre + "_COEMNO";
                    }
                }

                if (datos.SerieAltDiaria.nAños != 0)
                {
                    if (coeD)
                    {
                        nombre = nombre + "_COEDSI";
                    }
                    else
                    {
                        nombre = nombre + "_COEDNO";
                    }
                }
            }

            nombre = nombre + ".xlsx";

            ruta = ruta + "\\" + nombre;


            string[] args = Environment.GetCommandLineArgs();
            List<string> argsList = args.ToList();
            

          
                if (File.Exists(ruta))
                {
                    // Alerta por sobreescritura
                    if (argsList.Contains("-n"))

                    {
                        if (m_Excel != null)
                        {
                            m_Excel.Dispose();
                            m_Excel = null;
                        }
                        Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_ERROR, "strErrorOverwriteExcel"));
                        return;
                    }
                    else
                    {
                        try
                        {
                            File.Delete(ruta);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message.ToString() + "Error");
                        }
                    }
                }
            
            

            // Tiene que salvarse como  Punto-Alt-COE-SI-SI.xls
            try
            {
                m_Excel.SaveAs(new FileInfo(ruta));
                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strReportOk") + Microsoft.VisualBasic.Constants.vbCrLf + ruta, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strInfo"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                Console.WriteLine(_traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strNotSaveFile") + Microsoft.VisualBasic.Constants.vbCrLf + ruta + Microsoft.VisualBasic.Constants.vbCrLf + ex.Message, _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_INFO, "strError"), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            // Eliminamos la instancia de Excel de memoria
            if (m_Excel != null)
            {
                m_Excel.Dispose();
                m_Excel = null;
            }
        }


        /// <summary>
        /// Escribe la primera pagina del informe
        /// </summary>
        /// <remarks>Tiene que ser publica para recibir la simulacion</remarks>
        public ExcelPackage EscribirCabecera(ref DatosCalculo datos, ref IAHRISDataSet dataset)
        {

            if (m_Excel is null)
            {
                AbrirExcel();
            }

            Cabecera cabecera = new Cabecera(true, 0, "Caratula", "Carátula");
            cabecera.Escribir(m_Excel, ref datos, ref dataset, _traductor);

            return m_Excel;
        }

        public ExcelPackage EscribirCabeceraCaudalesEcologicos(SerieRCE serie, Simulacion simulacion)
        {

          //  if (m_Excel is null)
            {
                AbrirExcelCE();

            }
            double[][]caudales = new double[1][];
            CabeceraRCE cabeceraRCE = new CabeceraRCE(true, 0, "Caratula", "Carátula");
            cabeceraRCE.Escribir(m_Excel, serie, simulacion);

            ExcelWorksheet objSheet = objWorkbook.Worksheets[0];

            
            return m_Excel;
        }

    }

}
