using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IAHRIS.Calculo
{
    public class Reports
    {
        private DatosCalculo _datos;
        private IAHRISDataSet _dataSet;
        // Manejo del Excel
        private string _rutaExcel;
        private MultiLangXML.MultiIdiomasXML _traductor;
        private ExcelPackage m_Excel;
        ExcelWorkbook objWorkbook;
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public Reports(DatosCalculo datos, MultiLangXML.MultiIdiomasXML traductor, IAHRISDataSet dataset)
        {
            _datos = datos;
            _traductor = traductor;
            _rutaExcel = _traductor.getRutaExcel;
            _dataSet = dataset;

        }
        ~Reports()
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
            catch (Exception ex)
            {
            }
        }

        public void AbrirExcel()
        {
            string strRutaExcel = _rutaExcel;
            
            FileInfo fExcel = new FileInfo(strRutaExcel);

            m_Excel = new ExcelPackage(fExcel);
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            objWorkbook = m_Excel.Workbook;



        }
        public void CerrarExcel(bool coeD, bool coeM)
        {

            string ruta = "";
            string nombre = "";
            var FolderBrowserDialog1 = new FolderBrowserDialog();


            nombre = _datos.sNombre;
            nombre = nombre.Substring(0, nombre.LastIndexOf("-"));
            if (_datos.SerieAltMensual.nAños != 0 | _datos.SerieAltDiaria.nAños != 0)
            {
                nombre = nombre + "_" + _datos.sAlteracion;
                nombre = nombre.Substring(0, nombre.LastIndexOf("-"));
                if (_datos.SerieAltMensual.nAños != 0)
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

                if (_datos.SerieAltDiaria.nAños != 0)
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


  

        /// <summary>
        /// Escribe la primera pagina del informe
        /// </summary>
        /// <remarks>Tiene que ser publica para recibir la simulacion</remarks>
        public void EscribirCabecera(TestFechas.Simulacion sim, TestFechas.GeneracionInformes informes) //Recolocado (Manual)
        {
            ExcelRange range;
            if (m_Excel is null)
            {
                AbrirExcel();
            }


            ExcelWorksheet objSheet = objWorkbook.Worksheets[0];

            objSheet.Cells["E7"].Value = _datos.sNombre;
             objSheet.Cells["E8"].Value = _datos.sAlteracion;
             objSheet.Cells["E9"].Value = DateTime.Now.ToShortDateString();
             objSheet.Cells["K12"].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, sim.mesInicio.ToString());

            objSheet.Cells["C2"].Value = Application.ProductVersion;
            DateTime fileDate = new FileInfo(Application.ExecutablePath).LastWriteTime;
            objSheet.Cells["C3"].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, fileDate.Month.ToString()) + " " + fileDate.Year.ToString();
            int i, j;
            int pos;

            //Lista que contiene los informes a escribir, con su posición en la hoja excel
            Dictionary<string, string> inflist = new Dictionary<string, string>();
            int row = 14;
            string col = "B";
            string secCol = "H";

            if (informes.inf1) { inflist.Add("informe1", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf1a) { inflist.Add("informe1a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf1b) { inflist.Add("informe1b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf2) { inflist.Add("informe2", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf2a) { inflist.Add("informe2a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf3) { inflist.Add("informe3", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf3a) { inflist.Add("informe3a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf3b) { inflist.Add("informe3b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf4) { inflist.Add("informe4", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf4a) { inflist.Add("informe4a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf5) { inflist.Add("informe5", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf5a) { inflist.Add("informe5a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf5b) { inflist.Add("informe5b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf6) { inflist.Add("informe6", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf6a) { inflist.Add("informe6a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf6b) { inflist.Add("informe6b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf6c) { inflist.Add("informe6c", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf6d) { inflist.Add("informe6d", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf6e) { inflist.Add("informe6e", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf7a) { inflist.Add("informe7a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf7b) { inflist.Add("informe7b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf7c) { inflist.Add("informe7c", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf7d) { inflist.Add("informe7d", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf8) { inflist.Add("informe8", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf8a) { inflist.Add("informe8a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf8b) { inflist.Add("informe8b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf8c) { inflist.Add("informe8c", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf8d) { inflist.Add("informe8d", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf9) { inflist.Add("informe9", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf9a) { inflist.Add("informe9a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf9b) { inflist.Add("informe9b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf10a) { inflist.Add("informe10a", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf10b) { inflist.Add("informe10b", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf10c) { inflist.Add("informe10c", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };
            if (informes.inf10d) { inflist.Add("informe10d", col + row.ToString()); row++; if (row > 25) { col = secCol; row = 14; } };

            foreach (KeyValuePair<string, string> entry in inflist )
            {
                objSheet.Cells[entry.Value].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, entry.Key);
            }

            //// Escribir informes
            //// -----------------
            //int fila = 14;
            //if (informes.inf1)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1");
            //    fila = fila + 1;
            //}

            //if (informes.inf1a)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe1a");
            //    fila = fila + 1;
            //}

            //if (informes.inf2)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe2");
            //    fila = fila + 1;
            //}
            //if (informes.inf2a)
            //{
            //    objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe2a");
            //    fila = fila + 1;
            //}


            //if (informes.inf3)
            //{
            //    objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe3");
            //    fila = fila + 1;
            //}
            //if (informes.inf3a)
            //{
            //    objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe3a");
            //    fila = fila + 1;
            //}

            //if (informes.inf4)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4");
            //    fila = fila + 1;
            //}

            //if (informes.inf4a)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4a");
            //    fila = fila + 1;
            //}

            ////if (informes.inf4b)
            ////{
            ////     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe4b");
            ////    fila = fila + 1;
            ////}

            //if (informes.inf5)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5");
            //    fila = fila + 1;
            //}

            //if (informes.inf5a)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5a");
            //    fila = fila + 1;
            //}

            //if (informes.inf5b)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5b");
            //    fila = fila + 1;
            //}

            ////if (informes.inf5c)
            ////{
            ////     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe5c");
            ////    fila = fila + 1;
            ////}

            //if (informes.inf6)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6");
            //    fila = fila + 1;
            //}

            //if (informes.inf6a)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe6a");
            //    fila = fila + 1;
            //}

            //if (informes.inf7a)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7a");
            //    fila = fila + 1;
            //}

            //if (informes.inf7b)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7b");
            //    fila = fila + 1;
            //}

            //if (informes.inf7c)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7c");
            //    fila = fila + 1;
            //}

            //if (informes.inf7d)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe7d");
            //    fila = fila + 1;
            //}

            //if (informes.inf8)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8");
            //    fila = fila + 1;
            //}

            //if (informes.inf8a)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8a");
            //    fila = fila + 1;
            //}

            //if (informes.inf8b)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8b");
            //    fila = fila + 1;
            //}

            //if (informes.inf8c)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8c");
            //    fila = fila + 1;
            //}

            //if (informes.inf8d)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe8d");
            //    fila = fila + 1;
            //}

            //if (informes.inf9)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9");
            //    fila = fila + 1;
            //}

            //if (informes.inf9a)
            //{
            //     objSheet.Cells["B" + fila.ToString()].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_OTHER, "informe9a");
            //    fila = fila + 1;
            //}

            // (ExcelRange)objSheet.Cells["B32"].Offset Offset(0,0, sim.fechaFIN - sim.fechaINI, 11);

            // objSheet.Cells[32, 2, sim.fechaFIN - sim.fechaINI, 11];

            int offsetRow = 31;
            int offsetCol = 1;


            //range. .get_Resize(sim.fechaFIN - sim.fechaINI, 11);
            var loopTo = 31 + sim.fechaFIN - 1;
            for (i = 31 + sim.fechaINI; i <= loopTo; i++)
            {
                int year = i - 31;
                int indice = i - sim.fechaINI;
                objSheet.Cells[indice + 1, 2].Value = year.ToString() + "-" + (year + 1).ToString().Substring(2);


                // MENSUALES NATURAL
                if (sim.listas[2].Año is null)
                {
                    if (sim.añosInterNat != null)
                    {
                        var loopTo1 = sim.añosInterNat.Length - 1;
                        for (j = 0; j <= loopTo1; j++)
                        {
                            if (year == sim.añosInterNat[j])
                            {
                                objSheet.Cells[indice + 1, 3].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 2) = "SD"
                            }
                        }
                    }
                }
                else
                {
                    pos = Array.BinarySearch(sim.listas[2].Año, year);
                    if (pos >= 0)
                    {
                        if (sim.listas[2].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 3].Value = "X";
                            // Else
                            // range(indice + 1, 2) = "NC"
                        }
                        // Else
                        // range(indice + 1, 2) = "SD"
                    }

                    if (sim.añosInterNat != null)
                    {
                        var loopTo2 = sim.añosInterNat.Length - 1;
                        for (j = 0; j <= loopTo2; j++)
                        {
                            if (year == sim.añosInterNat[j])
                            {
                                objSheet.Cells[indice + 1, 3].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 2) = "SD"
                            }
                        }
                    }
                }

                if ((objSheet.Cells[indice + 1, 3]).Value != null)
                {
                    if (sim.añosParaCalculo[2].año != null)
                    {
                        var loopTo3 = sim.añosParaCalculo[2].año.Length - 1;
                        for (j = 0; j <= loopTo3; j++)
                        {
                            if (year == sim.añosParaCalculo[2].año[j])
                            {
                                objSheet.Cells[indice + 1, 4].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // MENSUALES ALTERADO
                if (sim.listas[3].Año is null)
                {
                    if (sim.añosInterAlt != null)
                    {
                        var loopTo4 = sim.añosInterAlt.Length - 1;
                        for (j = 0; j <= loopTo4; j++)
                        {
                            if (year == sim.añosInterAlt[j])
                            {
                                objSheet.Cells[indice + 1, 5].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 4) = "SD"
                            }
                        }
                    }
                }
                else
                {
                    pos = Array.BinarySearch(sim.listas[3].Año, year);
                    if (pos >= 0)
                    {
                        if (sim.listas[3].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 5].Value = "X";
                            // Else
                            // range(indice + 1, 4) = "NC"
                        }
                        // Else
                        // range(indice + 1, 4) = "SD"
                    }

                    if (sim.añosInterAlt != null)
                    {
                        var loopTo5 = sim.añosInterAlt.Length - 1;
                        for (j = 0; j <= loopTo5; j++)
                        {
                            if (year == sim.añosInterAlt[j])
                            {
                                objSheet.Cells[indice + 1, 5].Value = "X";
                                break;
                                // Else
                                // range(indice + 1, 4) = "SD"
                            }
                        }
                    }
                }

                if ((objSheet.Cells[indice + 1, 5].Value) != null)
                {
                    if (sim.añosParaCalculo[3].año != null)
                    {
                        var loopTo6 = sim.añosParaCalculo[3].año.Length - 1;
                        for (j = 0; j <= loopTo6; j++)
                        {
                            if (year == sim.añosParaCalculo[3].año[j])
                            {
                                objSheet.Cells[indice + 1, 6].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // COETANEOS MENSUALES
                if (objSheet.Cells[indice + 1, 4].Value != null & (objSheet.Cells[indice + 1, 6].Value != null))
                {
                    objSheet.Cells[indice + 1, 7].Value = "X";
                }

                // DIARIO NATURAL
                if (sim.listas[0].Año is null)
                {
                }
                // range(indice + 1, 7) = "SD"
                else
                {
                    pos = Array.BinarySearch(sim.listas[0].Año, year);
                    if (pos >= 0)
                    {
                        if (sim.listas[0].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 8].Value = "X";
                            // Else
                            // range(indice + 1, 7) = "NC"
                        }
                        // Else
                        // range(indice + 1, 7) = "SD"
                    }
                }

                if ((objSheet.Cells[indice + 1, 8].Value) != null)
                {
                    if (sim.añosParaCalculo[0].año != null)
                    {
                        var loopTo7 = sim.añosParaCalculo[0].año.Length - 1;
                        for (j = 0; j <= loopTo7; j++)
                        {
                            if (year == sim.añosParaCalculo[0].año[j])
                            {
                                objSheet.Cells[indice + 1, 9].Value = "X";
                                break;
                            }
                        }
                    }
                }

                // DIARIO ALTERADO
                if (sim.listas[1].Año is null)
                {
                }
                // range(indice + 1, 9) = "SD"
                else
                {
                    pos = Array.BinarySearch(sim.listas[1].Año, year);
                    if (pos >= 0)
                    {
                        if (sim.listas[1].validos[pos])
                        {
                            objSheet.Cells[indice + 1, 10].Value = "X";
                            // Else
                            // range(indice + 1, 9) = "NC"
                        }
                        // Else
                        // range(indice + 1, 9) = "SD"
                    }
                }

                if ((objSheet.Cells[indice + 1, 10].Value) != null)
                {
                    if (sim.añosParaCalculo[1].año != null)
                    {
                        var loopTo8 = sim.añosParaCalculo[1].año.Length - 1;
                        for (j = 0; j <= loopTo8; j++)
                        {
                            if (year == sim.añosParaCalculo[1].año[j])
                            {
                                objSheet.Cells[indice + 1, 11].Value = "X";
                                break;
                            }
                        }
                    }
                }


                // COETANEOS DIARIOS
                if ((objSheet.Cells[indice + 1, 9].Value) != null & (objSheet.Cells[indice + 1, 11].Value) != null)
                {
                    objSheet.Cells[indice + 1, 12].Value = "X";
                }
            }

            // Escribir resumen con años
            int filaNumAños = 32 + (sim.fechaFIN - sim.fechaINI);
            // objSheet.Cells["B" + filaNumAños];
            // (ExcelRange)range.Offset(0,0,1, 11);
            // objSheet.Cells[filaNumAños, 2, 1, 11];
            objSheet.Cells[filaNumAños, 2].Value = "Total";
            // Mensuales > Natural 
            if (sim.añosInterNat != null)
            {
                objSheet.Cells[filaNumAños, 3].Value = (sim.listas[2].nValidos + sim.añosInterNat.Length).ToString();
            }
            else
            {
                objSheet.Cells[filaNumAños, 3].Value = sim.listas[2].nValidos.ToString();
            }

            objSheet.Cells[filaNumAños, 4].Value = sim.añosParaCalculo[2].nAños;
            // Mensual > Alterado
            if (sim.añosInterAlt != null)
            {
                objSheet.Cells[filaNumAños, 5].Value = (sim.listas[3].nValidos + sim.añosInterAlt.Length).ToString();
            }
            else
            {
                objSheet.Cells[filaNumAños, 5].Value = sim.listas[3].nValidos.ToString();
            }

            objSheet.Cells[filaNumAños, 6].Value = sim.añosParaCalculo[3].nAños;
            // Mensuales > Coetaniedad
            if (sim.añosInterCoe != null)
            {
                objSheet.Cells[filaNumAños, 7].Value = (sim.coe[1].nCoetaneos + sim.añosInterCoe.Length).ToString();
            }
            else
            {
                objSheet.Cells[filaNumAños, 7].Value = sim.coe[1].nCoetaneos.ToString();
            }

            objSheet.Cells[filaNumAños, 8].Value = sim.listas[0].nValidos.ToString();
            objSheet.Cells[filaNumAños, 9].Value = sim.añosParaCalculo[0].nAños.ToString();
            objSheet.Cells[filaNumAños, 10].Value = sim.listas[1].nValidos.ToString();
            objSheet.Cells[filaNumAños, 11].Value = sim.añosParaCalculo[1].nAños.ToString();
            objSheet.Cells[filaNumAños, 12].Value = sim.coe[0].nCoetaneos.ToString();

            // Me.lblAñosHidro.Text = (Me._simulacion.fechaFIN - Me._simulacion.fechaINI).ToString()
            // Me.lblAñosNatDiario.Text = Me._simulacion.listas(0).nValidos.ToString()
            // Me.lblAñosAltDiario.Text = Me._simulacion.listas(1).nValidos.ToString()
            // Me.lblAñosCoeDiaria.Text = Me._simulacion.coe(0).nCoetaneos.ToString()

            // If (Not Me._simulacion.añosInterNat Is Nothing) Then
            // Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos + Me._simulacion.añosInterNat.Length).ToString()
            // Else
            // Me.lblAñosNatMensual.Text = (Me._simulacion.listas(2).nValidos).ToString()
            // End If
            // If (Not Me._simulacion.añosInterAlt Is Nothing) Then
            // Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos + Me._simulacion.añosInterAlt.Length).ToString()
            // Else
            // Me.lblAñosAltMensual.Text = (Me._simulacion.listas(3).nValidos).ToString()
            // End If
            // If (Not Me._simulacion.añosInterCoe Is Nothing) Then
            // Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos + Me._simulacion.añosInterCoe.Length).ToString()
            // Else
            // Me.lblAñosCoeMensual.Text = (Me._simulacion.coe(1).nCoetaneos).ToString()
            // End If

            // Me.lblAñosNatDiarioUSO.Text = Me._simulacion.añosParaCalculo(0).nAños
            // Me.lblAñosNatMensualUSO.Text = Me._simulacion.añosParaCalculo(2).nAños
            // Me.lblAñosAltDiarioUSO.Text = Me._simulacion.añosParaCalculo(1).nAños
            // Me.lblAñosAltMensualUSO.Text = Me._simulacion.añosParaCalculo(3).nAños



            string sRango;
            ((ExcelWorksheet)m_Excel.Workbook.Worksheets[0]).Select();
            ExcelWorksheet wrk = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[0]);
            // m_Excel.Sheets.Select(objSheet)

            //m_Excel. get_Range("B32:L32").Select();
            //((ExcelRange)m_Excel.Selection.Copy();


            //TODO: USADO PARA COPIAR FORMATO.  REPROGRAMARLO CORRECTAMENTE.

            string filaFin = (32 + (sim.fechaFIN - sim.fechaINI)).ToString();
            sRango = "B32:L";
            sRango = sRango + filaFin;

            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            // sRango = sRango & ":L"
            // sRango = sRango & (14 + i - 1).ToString()
            //m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //   wrk.Cells["B32:L32"].Copy(wrk.Cells[sRango]); 
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

            // Poner una linea mas gruesa
             objSheet.Cells["B" + filaFin + ":L" + filaFin].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;// Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
            //m_Excel.Cells["A1").Select();
        }

        internal void EscribirInforme10d()
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[35]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }

        internal void EscribirInforme10c()
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[34]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }

        internal void EscribirInforme10b()
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[33]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }

        internal void EscribirInforme10a()
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[32]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }

        internal void EscribirInforme9b()
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[31]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }

        public void EscribirInforme1() //Recolocado (offset)
        {
            
            ExcelRange range;
            

            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            
            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[1]);


             objSheet.Cells["E5"].Value = _datos.sNombre;

             objSheet.Cells["E6"].Value = _datos.sAlteracion;

             objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 11;
            int offsetCol = 1;

            var loopTo = _dataSet._AportacionNatAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _dataSet._AportacionNatAnualOrdAños.año[i].ToString() + "-" + (_dataSet._AportacionNatAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = _dataSet._AportacionNatAnualOrdAños.aportacion[i];
            }

            

            string sRango = "B12:C";
            sRango = sRango + (12 + _dataSet._AportacionNatAnualOrdAños.año.Length - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Escribir los limites
             //objSheet.Cells["J13"].Value = _dataSet._limHumNat;
             //objSheet.Cells["J14"].Value = _dataSet._limHumNat;
             //objSheet.Cells["L14"].Value = _dataSet._limSecNat;
             //objSheet.Cells["J15"].Value = _dataSet._limSecNat;


            // -----------------------
            int nAñosH = 0;
            int nAñosM = 0;
            int nAñosS = 0;
            int[] añosH = null;
            int[] añosM = null;
            int[] añosS = null;
            float[] apH = null;
            float[] apM = null;
            float[] apS = null;
            var loopTo1 = _dataSet._AportacionNatAnual.año.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                if (_dataSet._AportacionNatAnual.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    if (nAñosH == 0)
                    {
                        nAñosH = 1;
                        añosH = new int[1];
                        apH = new float[1];
                        añosH[0] = _dataSet._AportacionNatAnual.año[i];
                        apH[0] = _dataSet._AportacionNatAnual.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosH, nAñosH + 1);
                        Array.Resize(ref apH, nAñosH + 1);
                        añosH[nAñosH] = _dataSet._AportacionNatAnual.año[i];
                        apH[nAñosH] = _dataSet._AportacionNatAnual.aportacion[i];
                        nAñosH = nAñosH + 1;
                    }
                }
                else if (_dataSet._AportacionNatAnual.tipo[i] == TIPOAÑO.MEDIO)
                {
                    if (nAñosM == 0)
                    {
                        nAñosM = 1;
                        añosM = new int[1];
                        apM = new float[1];
                        añosM[0] = _dataSet._AportacionNatAnual.año[i];
                        apM[0] = _dataSet._AportacionNatAnual.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosM, nAñosM + 1);
                        Array.Resize(ref apM, nAñosM + 1);
                        añosM[nAñosM] = _dataSet._AportacionNatAnual.año[i];
                        apM[nAñosM] = _dataSet._AportacionNatAnual.aportacion[i];
                        nAñosM = nAñosM + 1;
                    }
                }
                else if (nAñosS == 0)
                {
                    nAñosS = 1;
                    añosS = new int[1];
                    apS = new float[1];
                    añosS[0] = _dataSet._AportacionNatAnual.año[i];
                    apS[0] = _dataSet._AportacionNatAnual.aportacion[i];
                }
                else
                {
                    Array.Resize(ref añosS, nAñosS + 1);
                    Array.Resize(ref apS, nAñosS + 1);
                    añosS[nAñosS] = _dataSet._AportacionNatAnual.año[i];
                    apS[nAñosS] = _dataSet._AportacionNatAnual.aportacion[i];
                    nAñosS = nAñosS + 1;
                }
            }

            Array.Sort(añosH, apH);
            Array.Sort(añosM, apM);
            Array.Sort(añosS, apS);
            
            offsetCol = 13;
            offsetRow = 13;
            var loopTo2 = añosH.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosH[i] + "-" + (añosH[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apH[i];
            }

           //  objSheet.Cells[16, 16, 16 + añosM.Length, 18];
            offsetCol = 15;
            // range.get_Resize(añosM.Length, 2);
            var loopTo3 = añosM.Length - 1;
            for (i = 0; i <= loopTo3; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosM[i] + "-" + (añosM[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apM[i];
            }

             //objSheet.Cells[16, 18, 16 + añosS.Length, 20];
            offsetCol = 17;
            // range.get_Resize(añosS.Length, 2);
            var loopTo4 = añosS.Length - 1;
            for (i = 0; i <= loopTo4; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosS[i] + "-" + (añosS[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apS[i];
            }

            objSheet.Calculate();

            ExcelChart ec = (ExcelChart)objSheet.Drawings[1];

            ec.Series[0].Series = "'Informe nº1'!$X$12:$X$" + (11 + _dataSet._AportacionNatAnualOrdAños.año.Length);
            ec.Series[0].XSeries = "'Informe nº1'!$W$12:$W$" + (11 + _dataSet._AportacionNatAnualOrdAños.año.Length);

            ec.Series[1].Series = "'Informe nº1'!$Y$12:$Y$" + (11 + _dataSet._AportacionNatAnualOrdAños.año.Length);
            ec.Series[1].XSeries = "'Informe nº1'!$W$12:$W$" + (11 + _dataSet._AportacionNatAnualOrdAños.año.Length);

            ec.Series[2].Series = "'Informe nº1'!$Z$12:$Z$" + (11 + _dataSet._AportacionNatAnualOrdAños.año.Length);
            ec.Series[2].XSeries = "'Informe nº1'!$W$12:$W$" + (11 + _dataSet._AportacionNatAnualOrdAños.año.Length);



            //ExcelChart ec = objSheet.Drawings.AddBoxWhiskerChart("aportacion mensual");
            //ec.SetPosition(30, 0, 6, 0);
            //ec.Series.Add("='Informe nº1'!$C$12:$C$612");

            //((Range)m_Excel.Selection).Copy();
            sRango = "N14:O";
            sRango = sRango + (14 + nAñosH - 1).ToString();
            //objSheet.Cells["N16:O16"].Copy(objSheet.Cells[sRango]);
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            /*m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);*/



            //((Worksheet)m_Excel.Sheets[2]).Select();
            // m_Excel.Sheets.Select(objSheet)
            /*m_Excel.Cells["P16:Q16").Select();
            ((Range)m_Excel.Selection).Copy();
            sRango = "P16:Q";
            sRango = sRango + (16 + nAñosM - 1).ToString();
            m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);*/
            sRango = "P14:Q";
            sRango = sRango + (14 + nAñosM - 1).ToString();

            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //((Worksheet)m_Excel.Sheets[2]).Select();
            // m_Excel.Sheets.Select(objSheet)
            /* m_Excel.Cells["R16:S16").Select();
             ((Range)m_Excel.Selection).Copy();
             sRango = "R16:S";
             sRango = sRango + (16 + nAñosS - 1).ToString();
             m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
             m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);
             m_Excel.Cells["A1").Select();*/
            sRango = "R14:S";
            sRango = sRango + (14 + nAñosS - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        public void EscribirInforme1a() //Recolocado (offset)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(3);
            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[2]);

            // Escribir cabecera
             objSheet.Cells["E5"].Value = _datos.sNombre;
             objSheet.Cells["E6"].Value = _datos.sAlteracion;
             objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir aportaciones mensuales
            // objSheet.Cells["C14");
            // range.get_Resize(_dataSet._AportacionAltAnualOrdAños.año.Length, 2);
            // objSheet.Cells[14, 3, 14 + _dataSet._AportacionAltAnualOrdAños.año.Length, 5];

            int offsetRow = 11;
            int offsetCol = 1;

            var loopTo = _dataSet._AportacionAltAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _dataSet._AportacionAltAnualOrdAños.año[i] + "-" + (_dataSet._AportacionAltAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
            }

            //((Worksheet)m_Excel.Sheets[3]).Select();
            //        // m_Excel.Sheets.Select(objSheet)
            //        m_Excel.Cells["C14:D14").Select();
            //        ((Range)m_Excel.Selection).Copy();
            //        string sRango = "C14:D";
            //        sRango = sRango + (14 + _dataSet._AportacionAltAnualOrdAños.año.Length - 1).ToString();
            //        m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //        m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

            string sRango = "B12:C"; 
            sRango = sRango + (12 + _dataSet._AportacionAltAnualOrdAños.año.Length - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


            // Escribir los años por tipos
            int nAñosH = 0;
            int nAñosM = 0;
            int nAñosS = 0;
            int[] añosH = null;
            int[] añosM = null;
            int[] añosS = null;
            float[] apH = null;
            float[] apM = null;
            float[] apS = null;


            // Esto solo se calculo con las series mensuales COETANEOS -> Pueso usar NAT o ALt indistintamente
            var loopTo1 = _dataSet._AportacionAltAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                if (_dataSet._AportacionAltAnualOrdAños.tipo!=null &&_dataSet._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.HUMEDO)
                {
                    if (nAñosH == 0)
                    {
                        nAñosH = 1;
                        añosH = new int[1];
                        apH = new float[1];
                        añosH[0] = _dataSet._AportacionAltAnualOrdAños.año[i];
                        apH[0] = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosH, nAñosH + 1);
                        Array.Resize(ref apH, nAñosH + 1);
                        añosH[nAñosH] = _dataSet._AportacionAltAnualOrdAños.año[i];
                        apH[nAñosH] = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                        nAñosH = nAñosH + 1;
                    }
                }
                else if (_dataSet._AportacionAltAnualOrdAños.tipo!=null&&_dataSet._AportacionAltAnualOrdAños.tipo[i] == TIPOAÑO.MEDIO)
                {
                    if (nAñosM == 0)
                    {
                        nAñosM = 1;
                        añosM = new int[1];
                        apM = new float[1];
                        añosM[0] = _dataSet._AportacionAltAnualOrdAños.año[i];
                        apM[0] = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                    }
                    else
                    {
                        Array.Resize(ref añosM, nAñosM + 1);
                        Array.Resize(ref apM, nAñosM + 1);
                        añosM[nAñosM] = _dataSet._AportacionAltAnualOrdAños.año[i];
                        apM[nAñosM] = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                        nAñosM = nAñosM + 1;
                    }
                }
                else if (nAñosS == 0)
                {
                    nAñosS = 1;
                    añosS = new int[1];
                    apS = new float[1];
                    añosS[0] = _dataSet._AportacionAltAnualOrdAños.año[i];
                    apS[0] = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                }
                else
                {
                    Array.Resize(ref añosS, nAñosS + 1);
                    Array.Resize(ref apS, nAñosS + 1);
                    añosS[nAñosS] = _dataSet._AportacionAltAnualOrdAños.año[i];
                    apS[nAñosS] = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
                    nAñosS = nAñosS + 1;
                }
            }

            Array.Sort(añosH, apH);
            Array.Sort(añosM, apM);
            Array.Sort(añosS, apS);


            // objSheet.Cells["N14");
            // range.get_Resize(añosH.Length, 2);
             //objSheet.Cells[14, 14, 14 + añosH.Length, 16];
            offsetCol = 13;
            offsetRow = 13;

            var loopTo2 = añosH.Length - 1;
            for (i = 0; i <= loopTo2; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosH[i] + "-" + (añosH[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apH[i];
            }

            // objSheet.Cells["P14");
            // range.get_Resize(añosM.Length, 2);
             //objSheet.Cells[14, 16, 14 + añosM.Length, 18];
            offsetCol = 15;
            var loopTo3 = añosM.Length - 1;
            for (i = 0; i <= loopTo3; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosM[i] + "-" + (añosM[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apM[i];
            }

            // objSheet.Cells["R14");
            // range.get_Resize(añosS.Length, 2);
             //objSheet.Cells[14, 18, 14 + añosS.Length, 20];
            offsetCol = 17;
            var loopTo4 = añosS.Length - 1;
            for (i = 0; i <= loopTo4; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = añosS[i] + "-" + (añosS[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = apS[i];
            }

            objSheet.Calculate();

            ExcelChart ec = (ExcelChart)objSheet.Drawings[1];

            ec.Series[0].Series = "'Informe nº1a'!$Z$12:$Z$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[0].XSeries = "'Informe nº1a'!$Y$12:$Y$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            ec.Series[1].Series = "'Informe nº1a'!$AA$12:$AA$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[1].XSeries = "'Informe nº1a'!$Y$12:$Y$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            ec.Series[2].Series = "'Informe nº1a'!$AB$12:$AB$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[2].XSeries = "'Informe nº1a'!$Y$12:$Y$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            //ec.Series[3].Series = "'Informe nº1a'!$AD$12:$AD$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            //ec.Series[3].XSeries = "'Informe nº1a'!$AC$12:$AC$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            //ec.Series[4].Series = "'Informe nº1a'!$AE$12:$AE$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            //ec.Series[4].XSeries = "'Informe nº1a'!$AC$12:$AC$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            //ec.Series[5].Series = "'Informe nº1a'!$AF$12:$AF$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            //ec.Series[5].XSeries = "'Informe nº1a'!$AC$12:$AC$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            //((Worksheet)m_Excel.Sheets[3]).Select();
            // m_Excel.Sheets.Select(objSheet)
            //m_Excel.Cells["N14:O14").Select();
            //((Range)m_Excel.Selection).Copy();
            //sRango = "N14:O";
            //sRango = sRango + (14 + nAñosH - 1).ToString();
            //m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);
            //((Worksheet)m_Excel.Sheets[3]).Select();

            sRango = "N14:O";
            sRango = sRango + (14 + nAñosH - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            // m_Excel.Sheets.Select(objSheet)
            //m_Excel.Cells["P14:Q14").Select();
            //((Range)m_Excel.Selection).Copy();
            //sRango = "P14:Q";
            //sRango = sRango + (14 + nAñosM - 1).ToString();
            //m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);
            //((Worksheet)m_Excel.Sheets[3]).Select();

            sRango = "P14:Q";
            sRango = sRango + (14 + nAñosM - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            //// m_Excel.Sheets.Select(objSheet)
            //m_Excel.Cells["R14:S14").Select();
            //((Range)m_Excel.Selection).Copy();
            //sRango = "R14:S";
            //sRango = sRango + (14 + nAñosS - 1).ToString();
            //m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);
            //m_Excel.Cells["A1").Select();

            sRango = "R14:S";
            sRango = sRango + (14 + nAñosS - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

     

        }

        public void EscribirInforme1b() 
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(3);
            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[3]);

            // Escribir cabecera
            objSheet.Cells["F5"].Value = _datos.sNombre;
            objSheet.Cells["F6"].Value = _datos.sAlteracion;
            objSheet.Cells["F7"].Value = DateTime.Now.ToShortDateString();


            //1
            int offsetRow = 11;
            int offsetCol = 1;

            var loopTo = _dataSet._AportacionNatAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _dataSet._AportacionNatAnualOrdAños.año[i].ToString() + "-" + (_dataSet._AportacionNatAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = _dataSet._AportacionNatAnualOrdAños.aportacion[i];
            }



            string sRango = "B12:C";
            sRango = sRango + (12 + _dataSet._AportacionNatAnualOrdAños.año.Length - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            //1b

            offsetRow = 11;
            offsetCol = 1;

             loopTo = _dataSet._AportacionAltAnualOrdAños.año.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _dataSet._AportacionAltAnualOrdAños.año[i] + "-" + (_dataSet._AportacionAltAnualOrdAños.año[i] + 1).ToString().Substring(2);
                objSheet.Cells[offsetRow + i + 1, offsetCol + 3].Value = _dataSet._AportacionAltAnualOrdAños.aportacion[i];
            }

            //((Worksheet)m_Excel.Sheets[3]).Select();
            //        // m_Excel.Sheets.Select(objSheet)
            //        m_Excel.Cells["C14:D14").Select();
            //        ((Range)m_Excel.Selection).Copy();
            //        string sRango = "C14:D";
            //        sRango = sRango + (14 + _dataSet._AportacionAltAnualOrdAños.año.Length - 1).ToString();
            //        m_Excel.Cells[sRango).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //        m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

            sRango = "B12:D";
            sRango = sRango + (12 + _dataSet._AportacionAltAnualOrdAños.año.Length - 1).ToString();
            objSheet.Cells[sRango].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            objSheet.Cells[sRango].Style.Numberformat.Format = "0.000";
            objSheet.Cells[sRango].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


            objSheet.Calculate();

            ExcelChart ec = (ExcelChart)objSheet.Drawings[1];

            ec.Series[0].Series = "'Informe nº 1b'!$AA$12:$AA$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[0].XSeries = "'Informe nº 1b'!$Z$12:$Z$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            ec.Series[1].Series = "'Informe nº 1b'!$AB$12:$AB$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[1].XSeries = "'Informe nº 1b'!$Z$12:$Z$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            ec.Series[2].Series = "'Informe nº 1b'!$AC$12:$AC$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[2].XSeries = "'Informe nº 1b'!$Z$12:$Z$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            ec.Series[3].Series = "'Informe nº 1b'!$AD$12:$AD$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[3].XSeries = "'Informe nº 1b'!$Z$12:$Z$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            ec.Series[4].Series = "'Informe nº 1b'!$AE$12:$AE$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[4].XSeries = "'Informe nº 1b'!$Z$12:$Z$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);

            ec.Series[5].Series = "'Informe nº 1b'!$AF$12:$AF$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
            ec.Series[5].XSeries = "'Informe nº 1b'!$Z$12:$Z$" + (11 + _dataSet._AportacionAltAnualOrdAños.año.Length);
        }

        public void EscribirInforme2() //Recolocado (Offset)
        {
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }


            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[4]);

             objSheet.Cells["E5"].Value = _datos.sNombre;
             objSheet.Cells["E6"].Value = _datos.sAlteracion;
             objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir los meses
            int offsetRow = 10;
            int offsetCol = 2;


            for (i = 0; i <= 11; i++)
               objSheet.Cells[offsetRow + 1, offsetCol +i+ 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0,3);


            int years = 0;
            for(i=0;i<_dataSet._AportacionNatMen.aportacion.Length;i++)
            {
                int month = _dataSet._AportacionNatMen.mes[i].Month;
                int year = _dataSet._AportacionNatMen.mes[i].Year;
                int posm = month - _datos.mesInicio;
                
                if (posm < 0) posm = 12 + posm;

                string hyear = "";
                if (month==_datos.mesInicio)
                {
                    years++;
                    if (_datos.mesInicio == 1)
                        hyear = year.ToString();
                    else
                        hyear = year.ToString() + "-" + (year + 1).ToString();
                    objSheet.Cells[offsetRow +2+(i/12), offsetCol].Value = hyear;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.Fill.SetBackground(Color.FromArgb(220, 230, 241), OfficeOpenXml.Style.ExcelFillStyle.Solid);
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    int line = years + 11;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + 13].Formula = "= MAX(C" + line + ": N" + line + ") - MIN(C" + line + ": N" + line + ")";


                }
                objSheet.Cells[offsetRow + 2+(i / 12), offsetCol + posm + 1].Value = _dataSet._AportacionNatMen.aportacion[i];
                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Style.Border.Bottom.Color.SetColor(Color.FromArgb(79, 129, 189));


            }
            objSheet.Cells["B12:N" + (11 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            objSheet.Cells["B12:B" + (11 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            objSheet.Cells["O12:O" + (11 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);

            offsetRow = 17;
            offsetCol = 18;


            for (i = 0; i <= 11; i++)
            {
             
                objSheet.Cells[offsetRow , offsetCol + i + 1].Value = _dataSet._HabEstacionalidadMensualNat[0].ndias[i];
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _dataSet._HabEstacionalidadMensualNat[1].ndias[i];
            }

        }

        public void EscribirInforme2a() //Recolocado (Offset)
        {
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }


            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[5]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir los meses
            int offsetRow = 15;
            int offsetCol = 2;


            for (i = 0; i <= 11; i++)
            {
                // ((i + _datos.mesInicio - 1) % 12) = Los datos en la estructura están almacenados por mes en orden natural, siendo el índice 0 Enero y el 11 Diciembre. Esta 
                //conversión permite obtener el mes natural correcto a partir del índice i, que representa el mes ordenado en función al mes de inicio (Normalmente octubre=0).
                //La fórmula normal sería  ((i + _datos.mesInicio - 1) % 12 + 1), pero se elimina el último +1 debido a que el índice es base 0, no base 1.

                if(float.IsNaN(_dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].MedianaHumeda))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = _dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].MedianaHumeda;
                }
                if (float.IsNaN(_dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].MedianaMedia ))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = _dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].MedianaMedia;
                }
                if (float.IsNaN(_dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].MedianaSeca ))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 3].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 3].Value = _dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].MedianaSeca;
                }

                objSheet.Cells[offsetRow+i, offsetCol+8].Value = _dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].MedianaPonderada;
                if (_dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].DistribucionAtipica)
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 10].Value = _dataSet.MensualCaracterizadaNatural[((i + _datos.mesInicio - 1) % 12)].DistrAtipDescriptor;
                }
            }

        }
        
            public void EscribirInforme3() //Recolocado (offset) //TODO: Probar
        {
           
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[6]);

            // Escribir cabecera
             objSheet.Cells["E5"].Value = _datos.sNombre;
             objSheet.Cells["E6"].Value = _datos.sAlteracion;
             objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir los meses
            int offsetRow = 10;
            int offsetCol = 2;


            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);

            int years = 0;
            for (i = 0; i < _dataSet._AportacionAltMen.aportacion.Length; i++)
            {
                int month = _dataSet._AportacionAltMen.mes[i].Month;
                int year = _dataSet._AportacionAltMen.mes[i].Year;
                int posm = month - _datos.mesInicio;

                if (posm < 0) posm = 12 + posm;

                string hyear = "";
                if (month == _datos.mesInicio)
                {
                    years++;
                    if (_datos.mesInicio == 1)
                        hyear = year.ToString();
                    else
                        hyear = year.ToString() + "-" + (year + 1).ToString();
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Value = hyear;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.Fill.SetBackground(Color.FromArgb(220, 230, 241), OfficeOpenXml.Style.ExcelFillStyle.Solid);
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    int line = years + 11;
                    objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + 13].Formula = "= MAX(C" + line + ": N" + line + ") - MIN(C" + line + ": N" + line + ")";


                }

                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Value = _dataSet._AportacionAltMen.aportacion[i];
                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                objSheet.Cells[offsetRow + 2 + (i / 12), offsetCol + posm + 1].Style.Border.Bottom.Color.SetColor(Color.FromArgb(79, 129, 189));


            }
            objSheet.Cells["B12:N" + (11 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            objSheet.Cells["B12:B" + (11 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            objSheet.Cells["O12:O" + (11 + years).ToString()].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);

            offsetRow = 17;
            offsetCol = 19;


            for (i = 0; i <= 11; i++)
            {

                objSheet.Cells[offsetRow, offsetCol + i + 1].Value = _dataSet._HabEstacionalidadMensualAlt[0].ndias[i];
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _dataSet._HabEstacionalidadMensualAlt[1].ndias[i];
            }

 
        }

        public void EscribirInforme3a() //Recolocado (Offset)
        {
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }


            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[7]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribir los meses
            int offsetRow = 15;
            int offsetCol = 2;


            for (i = 0; i <= 11; i++)
            {
                // ((i + _datos.mesInicio - 1) % 12) = Los datos en la estructura están almacenados por mes en orden natural, siendo el índice 0 Enero y el 11 Diciembre. Esta 
                //conversión permite obtener el mes natural correcto a partir del índice i, que representa el mes ordenado en función al mes de inicio (Normalmente octubre=0).
                //La fórmula normal sería  ((i + _datos.mesInicio - 1) % 12 + 1), pero se elimina el último +1 debido a que el índice es base 0, no base 1.

                if (float.IsNaN(_dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].MedianaHumeda))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 1].Value = _dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].MedianaHumeda;
                }
                if (float.IsNaN(_dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].MedianaMedia))
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = "#";
                }
                else
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 2].Value = _dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].MedianaMedia;
                }
                if (float.IsNaN(_dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].MedianaSeca))
                { objSheet.Cells[offsetRow + i, offsetCol + 3].Value = "#"; }
                else { objSheet.Cells[offsetRow + i, offsetCol + 3].Value = _dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].MedianaSeca; }

                objSheet.Cells[offsetRow + i, offsetCol + 8].Value = _dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].MedianaPonderada;
                if (_dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].DistribucionAtipica)
                {
                    objSheet.Cells[offsetRow + i, offsetCol + 10].Value = _dataSet.MensualCaracterizadaAlterada[((i + _datos.mesInicio - 1) % 12)].DistrAtipDescriptor;
                }

            }

        }
        public void EscribirInforme3b()
        {
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }


            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[8]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }

        public void EscribirInforme4() //Recolocado (offset)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            int pos;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(6);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[9]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribo los Valores Habituales Mensuales
            // objSheet.Cells["Q13");
            // range.get_Resize(11, 1);

            //objSheet.Cells[17, 13, 28, 14];
            int offsetRow = 12;
            int offsetCol = 16;
            pos = offsetRow;
            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabMagnitudNat[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabVariabilidadNat[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 2; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabEstacionalidadNat[i];
                pos = pos + 1;
            }

            // Escribo los habituales diarios
            // objSheet.Cells["Q24");
            // range.get_Resize(2, 1);

            //objSheet.Cells[24,17,26,18];
            offsetRow = 23;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._HabVariabilidadDiaraNat[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._HabVariabilidadDiaraNat[1];

            // AVENIDAS
            // objSheet.Cells["Q26");
            // range.get_Resize(4, 1);

            //objSheet.Cells[26, 17, 30, 18];
            offsetRow = 25;

            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._AveMagnitudNat[0];
            if (_dataSet._AveMagnitudNat[1] == -9999)
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._AveMagnitudNat[1];
            }

            objSheet.Cells[offsetRow + 3, offsetCol + 1].Value = _dataSet._AveMagnitudNat[2];
            objSheet.Cells[offsetRow + 4, offsetCol + 1].Value = _dataSet._AveMagnitudNat[3];

            // Escribir el periodo de retorno
            // objSheet.Cells["R27"];
            //objSheet.Cells[offsetRow+1, offsetCol + 1].Value = Math.Round(_dataSet._Ave2TNat / 2f);
            // objSheet.Cells["R28"];
            //objSheet.Cells[offsetRow+1, offsetCol + 1].Value = Math.Round(_dataSet._Ave2TNat);
            objSheet.Cells["R27"].Value = Math.Round(_dataSet._Ave2TNat / 2f);
            objSheet.Cells["R28"].Value = Math.Round(_dataSet._Ave2TNat);

            // objSheet.Cells["Q30"];
            // range.get_Resize(2, 1);

            //objSheet.Cells[30, 17, 32, 18];
            offsetRow = 29;

            if (_dataSet._AveVariabilidadNat[0] < 0f)
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._AveVariabilidadNat[0];
            }

            if (_dataSet._AveVariabilidadNat[1] < 0f)
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._AveVariabilidadNat[1];
            }

            objSheet.Cells["Q33"].Value = _dataSet._AveDuracionNat;

            // objSheet.Cells["E44"];
            // range.get_Resize(1, 12);
            //objSheet.Cells[44, 5, 45, 17];

            offsetRow = 43;
            offsetCol = 4;
            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, i + offsetCol + 1].Value = _dataSet._AveEstacionalidadNat.ndias[i];
            // Escribir la media del año
            //  objSheet.Range("Q44")
            // range.Value = Me._dataSet._AveEstacionalidadNat.mediaAño

            // SEQUIA
            // objSheet.Cells["Q34");
            // range.get_Resize(2, 1);

            //objSheet.Cells[34,17,36,18];
            offsetRow = 33;
            offsetCol = 16;

            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._SeqMagnitudNat[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._SeqMagnitudNat[1];

            // objSheet.Cells["Q36");
            // range.get_Resize(2, 1);

            //objSheet.Cells[36, 17, 38, 18];
            offsetRow = 35;

            if (_dataSet._SeqVariabilidadNat[0] < 0f)
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._SeqVariabilidadNat[0];
            }

            if (_dataSet._SeqVariabilidadNat[1] < 0f)
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._SeqVariabilidadNat[1];
            }

            // objSheet.Cells["E45");
            // range.get_Resize(1, 12);

            //objSheet.Cells[45, 5, 46, 17];
            offsetRow = 44;
            offsetCol = 4;

            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _dataSet._SeqEstacionalidadNat.ndias[i];
            // Escribir la media dell año
            //  objSheet.Range("Q45")
            // range.Value = Me._dataSet._SeqEstacionalidadNat.mediaAño

            // objSheet.Cells["Q39");
            // range.get_Resize(2, 1);
            //objSheet.Cells[39, 17, 41, 18];
            offsetRow = 38;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._SeqDuracionNat[0];
            // range(2, 1) = Me._dataSet._SeqDuracionNat(1)


            // Lista de los Q = 0 por meses
            // objSheet.Cells["E46");
            // range.get_Resize(1, 12);

            //objSheet.Cells[46, 5, 47, 17];

            offsetRow = 45;
            offsetCol = 4;

            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _dataSet._SeqDuracionCerosMesNat.ndias[i];

            // Escribir los meses
            // objSheet.Cells["E43");
            // range.get_Resize(1, 12);

            //objSheet.Cells[43, 5, 44, 17];
            offsetRow = 42;
            for (i = 0; i <= 11; i++)
                // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
        }


        public void EscribirInforme4a() //Recolocado (ad-hoc) //TODO:TEST
        {
            // Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;

            int i;
            int pos;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(7);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[10]);

             objSheet.Cells["E5"].Value = _datos.sNombre;
             objSheet.Cells["E6"].Value = _datos.sAlteracion;
             objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();


            // objSheet.Cells["Q14"];
            // range.get_Resize(11, 1);

             //objSheet.Cells[14, 17, 25, 18];

            pos = 12;
            int offsetCol = 16;
            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabMagnitudNat[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabVariabilidadNat[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 2; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabEstacionalidadNat[i];
                pos = pos + 1;
            }
        }

        //public void EscribirInforme4b()//Recolocado (ad-hoc) 
        //{ 
        //   // Microsoft.Office.Interop.Excel.Range range;
        //    ExcelRange range;

        //    if (m_Excel is null)
        //    {
        //        AbrirExcel();
        //    }

        //    //var objSheets = m_Excel.Worksheets;
        //    //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(8);

        //    ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[11]);

        //     objSheet.Cells["E5"].Value=_datos.sNombre;
        //     objSheet.Cells["E6"].Value=_datos.sAlteracion;
        //     objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();

        //    // objSheet.Cells["Q14"];
        //    // range.get_Resize(3, 1);

        //     //objSheet.Cells[14, 17, 17, 18];
        //    int offsetRow = 13;
        //    int offsetCol = 16;
        //    objSheet.Cells[offsetRow+1, offsetCol+ 1].Value = _dataSet._HabMagnitudAnualNat[0];
        //    //// Se quitan estos valores y se deben imprimir otros
        //    // range(2, 1) = Me._dataSet._HabVariabilidadAlt(0)
        //    objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._HabMagnitudAnualNat[1];
        //    // range(3, 1) = Me._dataSet._HabEstacionalidadAlt(0) '¿Que coño va aqui?
        //    objSheet.Cells[offsetRow+3, offsetCol + 1].Value = _dataSet._HabMagnitudAnualNat[2];

        //    //objSheet.Cells["Q20"];
        //    // range.get_Resize(1, 1);

        //    objSheet.Cells["Q20"].Value = _dataSet._HabMagnitudMensualNat[0];



        //    int i;
        //    // Lista media por meses
        //    // objSheet.Cells["E26");
        //    // range.get_Resize(5, 12);

        //     //objSheet.Cells[26,5,31,17];
        //    offsetRow = 25;
        //    offsetCol = 4;
        //    for (i = 0; i <= 11; i++)
        //    {
        //        objSheet.Cells[offsetRow+1, i + offsetCol + 1].Value = _dataSet._HabMagnitudMensualTablaNat[0].ndias[i];
        //        objSheet.Cells[offsetRow+2, offsetCol + i + 1].Value = _dataSet._HabMagnitudMensualTablaNat[1].ndias[i];
        //        objSheet.Cells[offsetRow+3, offsetCol + i + 1].Value = _dataSet._HabMagnitudMensualTablaNat[2].ndias[i];
        //        objSheet.Cells[offsetRow+4, offsetCol + i + 1].Value = _dataSet._HabEstacionalidadMensualNat[0].ndias[i];
        //        objSheet.Cells[offsetRow+5, offsetCol + i + 1].Value = _dataSet._HabEstacionalidadMensualNat[1].ndias[i];
        //    }

        //    // Escribir los meses
        //    // objSheet.Cells["E25");
        //    // range.get_Resize(1, 12);

        //     //objSheet.Cells[25, 5, 26, 17];
        //    offsetRow = 24;
            
        //    for (i = 0; i <= 11; i++)
        //        // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
        //        objSheet.Cells[offsetRow+1, offsetCol + i + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
        //}

      
        public void EscribirInforme5() //Recolocado (offset)
        {
           
            int i;
            int pos;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(9);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[11]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();

            // Escribo los Valores Habituales Mensuales
            // objSheet.Cells["Q13");
            // range.get_Resize(11, 1);

             //objSheet.Cells[13, 17, 24, 18];
            int offsetRow = 12;
            int offsetCol = 16;
            pos = offsetRow;
            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol+ 1].Value = _dataSet._HabMagnitudAlt[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabVariabilidadAlt[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 2; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabEstacionalidadAlt[i];
                pos = pos + 1;
            }

            // Escribo los habituales diarios
            // objSheet.Cells["Q24");
            // range.get_Resize(2, 1);

             //objSheet.Cells[24, 17, 26, 18];
            offsetRow = 23;
            objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._HabVariabilidadDiaraAlt[0];
            objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._HabVariabilidadDiaraAlt[1];

            // AVENIDAS
            // objSheet.Cells["Q26");
            // range.get_Resize(4, 1);
             //objSheet.Cells[26, 17, 30, 18];
            offsetRow = 25;
            objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[0];
            if (_dataSet._AveMagnitudAlt[1] == -9999)
            {
                objSheet.Cells[offsetRow+2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[1];
            }
            // range(2, 1) = Me._dataSet._AveMagnitudAlt(1)
            objSheet.Cells[offsetRow+3, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[2];
            objSheet.Cells[offsetRow+4, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[3];


            // objSheet.Cells["Q30");
            // range.get_Resize(2, 1);

             //objSheet.Cells[30, 17, 32, 18];
            offsetRow = 29;
            // range(1, 1) = Me._dataSet._AveVariabilidadAlt(0)
            // range(2, 1) = Me._dataSet._AveVariabilidadAlt(1)
            if (_dataSet._AveVariabilidadAlt[0] < 0f)
            {
                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._AveVariabilidadAlt[0];
            }

            if (_dataSet._AveVariabilidadAlt[1] < 0f)
            {
                objSheet.Cells[offsetRow+2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._AveVariabilidadAlt[1];
            }

             objSheet.Cells["Q33"].Value=_dataSet._AveDuracionAlt;

            // objSheet.Cells["E44");
            // range.get_Resize(1, 12);

             //objSheet.Cells[44, 5, 45, 17];
            offsetRow = 43;
            offsetCol = 4;
            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow+1, offsetCol + i + 1].Value = _dataSet._AveEstacionalidadAlt.ndias[i];
            //  objSheet.Range("Q44")
            // range.Value = Me._dataSet._AveEstacionalidadAlt.mediaAño

            // SEQUIAS
            // objSheet.Cells["Q34");
            // range.get_Resize(2, 1);

             //objSheet.Cells[34, 17, 36, 18];
            offsetRow = 33;
            offsetCol = 16;
            objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._SeqMagnitudAlt[0];
            objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._SeqMagnitudAlt[1];

            // objSheet.Cells["Q36");
            // range.get_Resize(2, 1);

             //objSheet.Cells[36, 17, 38, 18];
            offsetRow = 35;
            if (_dataSet._SeqVariabilidadAlt[0] < 0f)
            {
                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._SeqVariabilidadAlt[0];
            }

            if (_dataSet._SeqVariabilidadAlt[1] < 0f)
            {
                objSheet.Cells[offsetRow+2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._SeqVariabilidadAlt[1];
            }

            // objSheet.Cells["E45");
            // range.get_Resize(1, 12);

             //objSheet.Cells[45,5,46,17];
            offsetRow = 44;
            offsetCol = 4;
            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow+1, offsetCol + i + 1].Value = _dataSet._SeqEstacionalidadAlt.ndias[i];
            //  objSheet.Range("Q45")
            // range.Value = Me._dataSet._SeqEstacionalidadAlt.mediaAño

            // objSheet.Cells["Q39");
            // range.get_Resize(2, 1);

             //objSheet.Cells[39, 17, 41, 18];
            offsetRow = 38;
            offsetCol = 16;
            objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._SeqDuracionAlt[0];
            // range(2, 1) = Me._dataSet._SeqDuracionAlt(1)

            // Lista de los Q = 0 por meses
            // objSheet.Cells["E46");
            // range.get_Resize(1, 12);

             //objSheet.Cells[46,5,47,17];

            offsetRow = 45;
            offsetCol = 4;

            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow+1, offsetCol + i + 1].Value = _dataSet._SeqDuracionCerosMesAlt.ndias[i];

            // Escribir los meses
            // objSheet.Cells["E43");
            // range.get_Resize(1, 12);

             //objSheet.Cells[43,5,44,17];

            offsetRow = 42;

            for (i = 0; i <= 11; i++)
                // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                objSheet.Cells[offsetRow+1, offsetCol + i + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
        }

      
        public void EscribirInforme5a() 
        {
            int i;
            int pos;
            if (m_Excel is null)
            {
                AbrirExcel();
            }


            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[12]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();

            int offsetRow = 12;
            int offsetCol = 16;

            pos = 0;
            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow+pos + 1, offsetCol+ 1].Value = _dataSet._HabMagnitudAlt[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow+pos + 1, offsetCol + 1].Value = _dataSet._HabVariabilidadAlt[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 2; i++)
            {
                objSheet.Cells[offsetRow+pos + 1, offsetCol + 1].Value = _dataSet._HabEstacionalidadAlt[i];
                pos = pos + 1;
            }
        }

        public void EscribirInforme5b() //Recolocado (offset)
        {

            int i;
            int pos;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(9);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[13]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            // Escribo los Valores Habituales Mensuales
            // objSheet.Cells["Q13");
            // range.get_Resize(11, 1);

            //objSheet.Cells[13, 17, 24, 18];
            int offsetRow = 12;
            int offsetCol = 16;
            pos = offsetRow;
            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabMagnitudAlt[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabVariabilidadAlt[i];
                pos = pos + 1;
            }

            for (i = 0; i <= 2; i++)
            {
                objSheet.Cells[pos + 1, offsetCol + 1].Value = _dataSet._HabEstacionalidadAlt[i];
                pos = pos + 1;
            }

            // Escribo los habituales diarios
            // objSheet.Cells["Q24");
            // range.get_Resize(2, 1);

            //objSheet.Cells[24, 17, 26, 18];
            offsetRow = 23;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._HabVariabilidadDiaraAlt[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._HabVariabilidadDiaraAlt[1];

            // AVENIDAS
            // objSheet.Cells["Q26");
            // range.get_Resize(4, 1);
            //objSheet.Cells[26, 17, 30, 18];
            offsetRow = 25;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[0];
            if (_dataSet._AveMagnitudAlt[1] == -9999)
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[1];
            }
            // range(2, 1) = Me._dataSet._AveMagnitudAlt(1)
            objSheet.Cells[offsetRow + 3, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[2];
            objSheet.Cells[offsetRow + 4, offsetCol + 1].Value = _dataSet._AveMagnitudAlt[3];


            // objSheet.Cells["Q30");
            // range.get_Resize(2, 1);

            //objSheet.Cells[30, 17, 32, 18];
            offsetRow = 29;
            // range(1, 1) = Me._dataSet._AveVariabilidadAlt(0)
            // range(2, 1) = Me._dataSet._AveVariabilidadAlt(1)
            if (_dataSet._AveVariabilidadAlt[0] < 0f)
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._AveVariabilidadAlt[0];
            }

            if (_dataSet._AveVariabilidadAlt[1] < 0f)
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._AveVariabilidadAlt[1];
            }

            objSheet.Cells["Q33"].Value = _dataSet._AveDuracionAlt;

            // objSheet.Cells["E44");
            // range.get_Resize(1, 12);

            //objSheet.Cells[44, 5, 45, 17];
            offsetRow = 43;
            offsetCol = 4;
            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _dataSet._AveEstacionalidadAlt.ndias[i];
            //  objSheet.Range("Q44")
            // range.Value = Me._dataSet._AveEstacionalidadAlt.mediaAño

            // SEQUIAS
            // objSheet.Cells["Q34");
            // range.get_Resize(2, 1);

            //objSheet.Cells[34, 17, 36, 18];
            offsetRow = 33;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._SeqMagnitudAlt[0];
            objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._SeqMagnitudAlt[1];

            // objSheet.Cells["Q36");
            // range.get_Resize(2, 1);

            //objSheet.Cells[36, 17, 38, 18];
            offsetRow = 35;
            if (_dataSet._SeqVariabilidadAlt[0] < 0f)
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._SeqVariabilidadAlt[0];
            }

            if (_dataSet._SeqVariabilidadAlt[1] < 0f)
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = "**";
            }
            else
            {
                objSheet.Cells[offsetRow + 2, offsetCol + 1].Value = _dataSet._SeqVariabilidadAlt[1];
            }

            // objSheet.Cells["E45");
            // range.get_Resize(1, 12);

            //objSheet.Cells[45,5,46,17];
            offsetRow = 44;
            offsetCol = 4;
            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _dataSet._SeqEstacionalidadAlt.ndias[i];
            //  objSheet.Range("Q45")
            // range.Value = Me._dataSet._SeqEstacionalidadAlt.mediaAño

            // objSheet.Cells["Q39");
            // range.get_Resize(2, 1);

            //objSheet.Cells[39, 17, 41, 18];
            offsetRow = 38;
            offsetCol = 16;
            objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._SeqDuracionAlt[0];
            // range(2, 1) = Me._dataSet._SeqDuracionAlt(1)

            // Lista de los Q = 0 por meses
            // objSheet.Cells["E46");
            // range.get_Resize(1, 12);

            //objSheet.Cells[46,5,47,17];

            offsetRow = 45;
            offsetCol = 4;

            for (i = 0; i <= 11; i++)
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _dataSet._SeqDuracionCerosMesAlt.ndias[i];

            // Escribir los meses
            // objSheet.Cells["E43");
            // range.get_Resize(1, 12);

            //objSheet.Cells[43,5,44,17];

            offsetRow = 42;

            for (i = 0; i <= 11; i++)
                // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                objSheet.Cells[offsetRow + 1, offsetCol + i + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
        }



        public void EscribirInforme6() //Recolocado
        {
            
            if (m_Excel is null)
            {
                AbrirExcel();
            }

           
            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[14]);

             objSheet.Cells["D5"].Value=_datos.sNombre;
             objSheet.Cells["D6"].Value=_datos.sAlteracion;
             objSheet.Cells["D7"].Value=DateTime.Now.ToShortDateString();

            int offsetRow = 15;
            int offsetCol = 11;

            for (int i = 0; i <= 364; i++)
                objSheet.Cells[offsetRow+i + 1, offsetCol+ 1].Value = _dataSet._TablaCQCNat.añomedio[i];
           
        }
        public void EscribirInforme6a() //Recolocado (offset)
        {
            
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[15]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
            
            int offsetRow = 15;
            int offsetCol = 11;

            for (int i = 0; i <= 364; i++)
                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _dataSet._TablaCQCAlt.añomedio[i];
        }

        public void EscribirInforme6b() //Recolocado (offset)
        {
            

            if (m_Excel is null)
            {
                AbrirExcel();
            }

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[16]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
            
            int offsetRow = 15;
            int offsetCol = 11;

            //for (int i = 0; i <= 364; i++)
            //    objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _dataSet._TablaCQCAlt.añomedio[i];
        }
        public void EscribirInforme6c() //Recolocado (offset)
        {
           
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            
            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[17]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 15;
            int offsetCol = 19;

            for (int m = 0; m < 12; m++)
            {
                int mpos = (m + 3) % 12;
                for (int k = 0; k < 31; k++)
                {
                    try
                    {
                        objSheet.Cells[offsetRow + k + 1, offsetCol + mpos + 1].Value =
                            _dataSet.CurvaClasificadaMensualCaudalNatural.SerieEstructurada[m].SerieClasificada.ValoresK[k].Media;
                    }
                    catch (ArgumentOutOfRangeException e)
                    { }
                }
            }
        }
        public void EscribirInforme6d() //Recolocado (offset)
        {


            if (m_Excel is null)
            {
                AbrirExcel();
            }

          
            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[18]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            int offsetRow = 15;
            int offsetCol = 19;

            for (int m = 0; m < 12; m++)
            {
                int mpos = (m + 3) % 12;
                for (int k = 0; k < 31; k++)
                {
                    try
                    {
                        objSheet.Cells[offsetRow + k + 1, offsetCol + mpos + 1].Value =
                            _dataSet.CurvaClasificadaMensualCaudalAlterada.SerieEstructurada[m].SerieClasificada.ValoresK[k].Media;
                    }
                    catch (ArgumentOutOfRangeException e)
                    { }
                }
            }
        }
        public void EscribirInforme6e() //Recolocado (offset)
        {
          
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(14);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[19]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();
        }
        public void EscribirInforme7a() //Recolocado (offset)
        {
          
            if (m_Excel is null)
            {
                AbrirExcel();
            }

           ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[20]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 1: Magnitud aportaciones anuales +++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D15");
            // range.get_Resize(1, 2);

             //objSheet.Cells[15,4,16,6];

            int offsetRow = 14;
            int offsetCol = 3;

            string s = "";
            if (_dataSet._IndicesHabituales[0].calculado)
            {
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "15").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "15"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "15"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol+1].Value = _dataSet._IndicesHabituales[0].valor[0];
                if (_dataSet._IndicesHabituales[0].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[0].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // --------------------------------------------------------------------
                // objSheet.Cells["D21");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[21,4,22,6];
                offsetRow = 20;
                
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "21").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "21"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "21"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[0].valor[1];
                if (_dataSet._IndicesHabituales[0].invertido[1])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[0].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ---------------------------------------------------------------------
                // objSheet.Cells["D27");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[27,4,28,6];
                offsetRow = 26;
                
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "27").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "27"]);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "27"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[0].valor[2];
                if (_dataSet._IndicesHabituales[0].invertido[2])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[0].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ----------------------------------------------------------------------
                // objSheet.Cells["D33");
                // range.get_Resize(1, 2);


                 ///objSheet.Cells[33,4,34,6];
                offsetRow = 32;

                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "33").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);
                
                objSheet.Cells[offsetRow+1,offsetCol+ 1].Value = _dataSet._IndicesHabituales[0].valor[3];

                //objSheet.Cells[].Copy(objSheet.Cells[]);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "33"]);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "33"].Value = null;

                // Cambio #289 - Ponderados no llevan signo
                // If (Me._dataSet._IndicesHabituales(0).invertido(3)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(0).indeterminacion(3)) Then
                // s = s & "**"
                // End If
                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
            }
            else
            {
                 objSheet.Cells["E15"].Value=("#");
                 objSheet.Cells["E21"].Value=("#");
                 objSheet.Cells["E27"].Value=("#");
                 objSheet.Cells["E33"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 2: Magnitud aportaciones mensuales +
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D16");
            // range.get_Resize(1, 2);


             //objSheet.Cells[16,4,17,6];
            offsetRow = 15;
            
            s = "";
            if (_dataSet._IndicesHabituales[1].calculado)
            {
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "16").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "16"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "16"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[0];
                if (_dataSet._IndicesHabituales[1].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[1].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // -------------------------------------------------------------------------
                // objSheet.Cells["D22");
                // range.get_Resize(1, 2);

                //objSheet.Cells[22,4,23,6];
                offsetRow = 21;

                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "22").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "22"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "22"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[1];

                if (_dataSet._IndicesHabituales[1].invertido[1])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[1].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // --------------------------------------------------------------------------
                // objSheet.Cells["D28");
                // range.get_Resize(1, 2);

                //objSheet.Cells[28,4,29,6];
                offsetRow = 27;

                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "28").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "28"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "28"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[2];
                if (_dataSet._IndicesHabituales[1].invertido[2])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[1].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
                // ---------------------------------------------------------------------------
                // objSheet.Cells["D34");
                // range.get_Resize(1, 2);


                //objSheet.Cells[34,4,35,6];
                offsetRow = 33;


                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "34").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "34"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "34"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[3];
                // Cambio #289 - Ponderados no llevan signo
                // If (Me._dataSet._IndicesHabituales(1).invertido(3)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(1).indeterminacion(3)) Then
                // s = s & "**"
                // End If
                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;

                offsetCol = 21;
                offsetRow = 16;


                for (int i = 0; i <= 11; i++)
                {
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Hum.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 3].Value = GetInf7Sign(_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Hum.Inverso, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Hum.Indeterminado, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Hum.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 4].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Med.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 5].Value = GetInf7Sign(_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Med.Inverso, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Med.Indeterminado, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Med.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 6].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Sec.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 7].Value = GetInf7Sign(_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Sec.Inverso, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Sec.Indeterminado, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Sec.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 8].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Pond.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 9].Value = (_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12 ].IAH2Pond.DistribucionAtipica ? "$" : "");


                }
            }
            else
            {
                objSheet.Cells["E16"].Value = ("#");
                objSheet.Cells["E22"].Value = ("#");
                objSheet.Cells["E28"].Value = ("#");
                objSheet.Cells["E34"].Value = ("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 3: Variabilidad Habitual  ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D17");
            // range.get_Resize(1, 2);
             //objSheet.Cells[17,4,18,6];
            offsetRow = 16;
            offsetCol = 3;
            s = "";
            if (_dataSet._IndicesHabituales[2].calculado)
            {
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[0]) + "17").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[0]) + "17"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[0]) + "17"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[2].valor[0];
                if (_dataSet._IndicesHabituales[2].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[2].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // --------------------------------------------------------------------------
                // objSheet.Cells["D23");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[23,4,24,6];
                offsetRow = 22;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[1]) + "23").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[1]) + "23"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[1]) + "23"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[2].valor[1];
                if (_dataSet._IndicesHabituales[2].invertido[1])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[2].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ----------------------------------------------------------------------------
                // objSheet.Cells["D29");
                // range.get_Resize(1, 2);
                 //objSheet.Cells[29,4,30,6];
                offsetRow = 28;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[2]) + "29").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[2]) + "29"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[2]) + "29"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[2].valor[2];
                if (_dataSet._IndicesHabituales[2].invertido[2])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[2].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ------------------------------------------------------------------------------
                // objSheet.Cells["D35");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[35,4,36,6];
                offsetRow = 34;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[3]) + "35").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[3]) + "35"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[2].valor[3]) + "35"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[2].valor[3];
                // Cambio #289 - Ponderados no llevan signo
                // If (Me._dataSet._IndicesHabituales(2).invertido(3)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(2).indeterminacion(3)) Then
                // s = s & "**"
                // End If
                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
            }
            else
            {
                 objSheet.Cells["E17"].Value=("#");
                 objSheet.Cells["E23"].Value=("#");
                 objSheet.Cells["E29"].Value=("#");
                 objSheet.Cells["E35"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 4: Variabilidad Extrema  +++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D18");
            // range.get_Resize(1, 2);

             //objSheet.Cells[18,4,19,6];
            offsetRow = 17;
            s = "";
            if (_dataSet._IndicesHabituales[3].calculado)
            {
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "18").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "18"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "18"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[3].valor[0];
                if (_dataSet._IndicesHabituales[3].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[3].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // --------------------------------------------------------------------
                // objSheet.Cells["D24");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[24,4,25,6];
                offsetRow = 23;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "24").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "24"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "24"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[3].valor[1];
                if (_dataSet._IndicesHabituales[3].invertido[1])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[3].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ---------------------------------------------------------------------
                // objSheet.Cells["D30");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[30,4,31,6];
                offsetRow = 29;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "30").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "30"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "30"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol+ 1].Value = _dataSet._IndicesHabituales[3].valor[2];
                if (_dataSet._IndicesHabituales[3].invertido[2])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[3].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // -----------------------------------------------------------------------
                // objSheet.Cells["D36");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[36,4,37,6];
                offsetRow = 35;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "36").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "36"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "36"].Value = null;


                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[3].valor[3];
                // Cambio #289 - Ponderados no llevan signo
                // If (Me._dataSet._IndicesHabituales(3).invertido(3)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(3).indeterminacion(3)) Then
                // s = s & "**"
                // End If
                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
            }
            else
            {
                 objSheet.Cells["E18"].Value=("#");
                 objSheet.Cells["E24"].Value=("#");
                 objSheet.Cells["E30"].Value=("#");
                 objSheet.Cells["E36"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ [IAH5] Indice 6: Estacionalidad Maximos ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D19");
            // range.get_Resize(1, 2);

             //objSheet.Cells[19,4,20,6];
            offsetRow = 18;
            // s = ""
            if (_dataSet._IndicesHabituales[5].calculado)
            {
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "19").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "19"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "19"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[0];
                // If (Me._dataSet._IndicesHabituales(5).invertido(0)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(5).indeterminacion(0)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s

                // --------------------------------------------------------------------
                // objSheet.Cells["D25");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[25,4,26,6];
                offsetRow = 24;
                 //s = ""

                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "25").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "25"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "25"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[1];
                // If (Me._dataSet._IndicesHabituales(5).invertido(1)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(5).indeterminacion(1)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s
                // --------------------------------------------------------------------------
                // objSheet.Cells["D31");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[31,4,32,6];
                offsetRow = 30;
                // s = ""

                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "31").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "31"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "31"].Value = null;
                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[2];
                // If (Me._dataSet._IndicesHabituales(5).invertido(2)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(5).indeterminacion(2)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s
                // -----------------------------------------------------------------------------
                // objSheet.Cells["D37");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[37,4,38,6];
                offsetRow = 36;
                // s = ""

                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "37").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "37"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "37"].Value = null;
                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[3];
            }
            // If (Me._dataSet._IndicesHabituales(5).invertido(3)) Then
            // s = "*"
            // End If
            // If (Me._dataSet._IndicesHabituales(5).indeterminacion(3)) Then
            // s = s & "**"
            // End If
            // range(1, 2) = s
            else
            {
                 objSheet.Cells["E19"].Value=("#");
                 objSheet.Cells["E25"].Value=("#");
                 objSheet.Cells["E31"].Value=("#");
                 objSheet.Cells["E37"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ [IAH6] Indice 7: Estacionalidad Minimos ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D20");
            // range.get_Resize(1, 2);

             //objSheet.Cells[20,4,21,6];
            offsetRow = 19;
            s = "";
            if (_dataSet._IndicesHabituales[6].calculado)
            {
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "20").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "20"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "20"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[0];
                // If (Me._dataSet._IndicesHabituales(6).invertido(0)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(6).indeterminacion(0)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s
                // -------------------------------------------------------------------------------
                // objSheet.Cells["D26");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[26,4,27,6];
                offsetRow = 25;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "26").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "26"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "26"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[1];
                // If (Me._dataSet._IndicesHabituales(6).invertido(1)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(6).indeterminacion(1)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s
                // ---------------------------------------------------------------------------------
                // objSheet.Cells["D32");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[32,4,33,6];
                offsetRow = 31;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "32").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "32"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "32"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[2];
                // If (Me._dataSet._IndicesHabituales(6).invertido(2)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(6).indeterminacion(2)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s
                // ------------------------------------------------------------------------
                // objSheet.Cells["D38");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[38,4,39,6];
                offsetRow = 37;
                s = "";
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "38").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "38"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "38"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[3];
            }
            // If (Me._dataSet._IndicesHabituales(6).invertido(3)) Then
            // s = "*"
            // End If
            // If (Me._dataSet._IndicesHabituales(6).indeterminacion(3)) Then
            // s = s & "**"
            // End If
            // range(1, 2) = s
            else
            {
                 objSheet.Cells["E20"].Value=("#");
                 objSheet.Cells["E26"].Value=("#");
                 objSheet.Cells["E32"].Value=("#");
                 objSheet.Cells["E38"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++
            // +++++ Indices IAG +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["G43");
            // range.get_Resize(4, 1);


             //objSheet.Cells[43,7,47,8];
            offsetRow = 42;
            offsetCol = 6;

            int auxCol = 43;
            for (int i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value= _dataSet._IndiceIAG[i];
                //((Worksheet)m_Excel.Sheets[15]).Select();
                //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + (auxCol + i).ToString()).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + "14"].Copy(objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + (auxCol + i).ToString()]);
                objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + (auxCol + i).ToString()].Value = null;
            }
        }

        public void EscribirInforme7b() //Recolocado (offset)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            // Dim pos As Integer

            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(16);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[21]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 1: Magnitud aportaciones anuales +++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D15");
            // range.get_Resize(1, 2);

             //objSheet.Cells[15,4,16,6];
            int offsetRow = 14;
            int offsetCol = 3;

            string s = "";
            if (_dataSet._IndicesHabituales[0].calculado)
            {
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "15").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "15"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[0]) + "15"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol+1].Value = _dataSet._IndicesHabituales[0].valor[0];
                if (_dataSet._IndicesHabituales[0].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[0].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ------------------------------------------------------------------
                // objSheet.Cells["D20");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[20,4,21,6];
                offsetRow = 19;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "20").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "20"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[1]) + "20"].Value = null;


                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[0].valor[1];
                if (_dataSet._IndicesHabituales[0].invertido[1])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[0].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // -------------------------------------------------------------------
                // objSheet.Cells["D25");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[25,4,26,6];
                offsetRow = 24;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "25").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "25"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[2]) + "25"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[0].valor[2];
                if (_dataSet._IndicesHabituales[0].invertido[2])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[0].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // -------------------------------------------------------------------
                // objSheet.Cells["D30");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[30,4,31,6];
                offsetRow = 29;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "30").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "30"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[0].valor[3]) + "30"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[0].valor[3];
                // Cambio #289 - Ponderados no llevan signo
                // If (Me._dataSet._IndicesHabituales(0).invertido(3)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(0).indeterminacion(3)) Then
                // s = s & "**"
                // End If
                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
            }
            else
            {
                 objSheet.Cells["E15"].Value=("#");
                 objSheet.Cells["E20"].Value=("#");
                 objSheet.Cells["E25"].Value=("#");
                 objSheet.Cells["E30"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 2: Magnitud aportaciones mensuales +
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D16");
            // range.get_Resize(1, 2);

             //objSheet.Cells[16,4,17,6];
            offsetRow = 15;
            s = "";
            if (_dataSet._IndicesHabituales[1].calculado)
            {
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "16").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "16"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[0]) + "16"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[0];
                if (_dataSet._IndicesHabituales[1].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[1].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // -----------------------------------------------------------------
                // objSheet.Cells["D21");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[21,4,22,6];
                offsetRow =20;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "21").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "21"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[1]) + "21"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[1];
                if (_dataSet._IndicesHabituales[1].invertido[1])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[1].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ------------------------------------------------------------------
                // objSheet.Cells["D26");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[26,4,27,6];
                offsetRow = 25;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "26").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "26"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[2]) + "26"].Value = null;


                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[2];
                if (_dataSet._IndicesHabituales[1].invertido[2])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[1].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // --------------------------------------------------------------------
                // objSheet.Cells["D31");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[31,4,32,6];
                offsetRow = 30;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "31").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "31"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[1].valor[3]) + "31"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[1].valor[3];
                // Cambio #289 - Ponderados no llevan signo
                // If (Me._dataSet._IndicesHabituales(1).invertido(3)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(1).indeterminacion(3)) Then
                // s = s & "**"
                // End If
                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;

                offsetCol = 21;
                offsetRow = 16;


                for (i = 0; i <= 11; i++)
                {
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Hum.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 3].Value = GetInf7Sign(_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Hum.Inverso, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Hum.Indeterminado, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Hum.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 4].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Med.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 5].Value = GetInf7Sign(_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Med.Inverso, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Med.Indeterminado, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Med.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 6].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Sec.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 7].Value = GetInf7Sign(_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Sec.Inverso, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Sec.Indeterminado, _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Sec.NoCalculado);
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 8].Value = _dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Pond.IAH2Ratio;
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 9].Value = (_dataSet.IAH2.Mensual[(i + _datos.mesInicio - 1) % 12].IAH2Pond.DistribucionAtipica ? "$" : "");


                }
            }
            else
            {
                 objSheet.Cells["E16"].Value=("#");
                 objSheet.Cells["E21"].Value=("#");
                 objSheet.Cells["E26"].Value=("#");
                 objSheet.Cells["E31"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 3: Variabilidad Habitual  ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // If (Me._dataSet._IndicesHabituales(2).calculado) Then
            // Else
            //  objSheet.Range("E17")
            // range.Value = "#"
            //  objSheet.Range("E23")
            // range.Value = "#"
            //  objSheet.Range("E29")
            // range.Value = "#"
            //  objSheet.Range("E35")
            // range.Value = "#"
            // End If

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 4: Variabilidad Extrema  +++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D17");
            // range.get_Resize(1, 2);

             //objSheet.Cells[17,4,18,6];
            offsetRow = 16;
            offsetCol = 3;
            s = "";
            if (_dataSet._IndicesHabituales[3].calculado)
            {
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "17").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "17"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[0]) + "17"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[3].valor[0];
                if (_dataSet._IndicesHabituales[3].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[3].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // -----------------------------------------------------------------
                // objSheet.Cells["D22");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[22,4,23,6];
                offsetRow = 21;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "22").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "22"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[1]) + "22"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[3].valor[1];
                if (_dataSet._IndicesHabituales[3].invertido[1])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[3].indeterminacion[1])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // ------------------------------------------------------------------
                // objSheet.Cells["D27");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[27,4,28,6];
                offsetRow = 26;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "27").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "27"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[2]) + "27"].Value = null;


                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[3].valor[2];
                if (_dataSet._IndicesHabituales[3].invertido[2])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabituales[3].indeterminacion[2])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
                // --------------------------------------------------------------------
                // objSheet.Cells["D32");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[32,4,33,6];
                offsetRow = 31;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "32").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "32"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[3].valor[3]) + "32"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[3].valor[3];
                // Cambio #289 - Ponderados no llevan signo
                // If (Me._dataSet._IndicesHabituales(3).invertido(3)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(3).indeterminacion(3)) Then
                // s = s & "**"
                // End If
                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = s;
            }
            else
            {
                 objSheet.Cells["E17"].Value=("#");
                 objSheet.Cells["E22"].Value=("#");
                 objSheet.Cells["E27"].Value=("#");
                 objSheet.Cells["E32"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 6: Estacionalidad Maximos ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D18");
            // range.get_Resize(1, 2);

             //objSheet.Cells[18,4,19,6];
            offsetRow = 17;
            // s = ""
            if (_dataSet._IndicesHabituales[5].calculado)
            {
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "18").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "18"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[0]) + "18"].Value = null;


                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[0];
                // If (Me._dataSet._IndicesHabituales(5).invertido(0)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(5).indeterminacion(0)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s

                // -----------------------------------------------------------------------
                // objSheet.Cells["D23");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[23,4,24,6];
                offsetRow = 22;
                // s = ""

                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "23").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "23"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[1]) + "23"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[1];
                // If (Me._dataSet._IndicesHabituales(5).invertido(1)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(5).indeterminacion(1)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s

                // ------------------------------------------------------------------------
                // objSheet.Cells["D28");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[28,4,29,6];
                offsetRow = 27;
                // s = ""

                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "28").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "28"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[2]) + "28"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[2];
                // If (Me._dataSet._IndicesHabituales(5).invertido(2)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(5).indeterminacion(2)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s

                // -----------------------------------------------------------------------
                // objSheet.Cells["D33");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[33,4,34,6];
                offsetRow = 32;
                // s = ""

                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "33").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "33"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[5].valor[3]) + "33"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[5].valor[3];
            }
            // If (Me._dataSet._IndicesHabituales(5).invertido(3)) Then
            // s = "*"
            // End If
            // If (Me._dataSet._IndicesHabituales(5).indeterminacion(3)) Then
            // s = s & "**"
            // End If
            // range(1, 2) = s
            else
            {
                 objSheet.Cells["E18"].Value=("#");
                 objSheet.Cells["E23"].Value=("#");
                 objSheet.Cells["E28"].Value=("#");
                 objSheet.Cells["E33"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++++++
            // +++ Indice 7: Estacionalidad Minimos ++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["D19");
            // range.get_Resize(1, 2);

             //objSheet.Cells[19,4,20,6];
            offsetRow = 18;
            s = "";
            if (_dataSet._IndicesHabituales[6].calculado)
            {
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "19").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "19"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[0]) + "19"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[0];
                // If (Me._dataSet._IndicesHabituales(6).invertido(0)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(6).indeterminacion(0)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s

                // -------------------------------------------------------------------------
                // objSheet.Cells["D24");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[24,4,25,6];
                offsetRow = 23;
                //s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "24").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "24"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[1]) + "24"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[1];
                // If (Me._dataSet._IndicesHabituales(6).invertido(1)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(6).indeterminacion(1)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s

                // --------------------------------------------------------------------------
                // objSheet.Cells["D29");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[29,4,30,6];
                offsetRow = 28;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "29").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "29"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[2]) + "29"].Value = null;


                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[2];
                // If (Me._dataSet._IndicesHabituales(6).invertido(2)) Then
                // s = "*"
                // End If
                // If (Me._dataSet._IndicesHabituales(6).indeterminacion(2)) Then
                // s = s & "**"
                // End If
                // range(1, 2) = s

                // ------------------------------------------------------------------------------
                // objSheet.Cells["D34");
                // range.get_Resize(1, 2);

                 //objSheet.Cells[34,4,35,6];
                offsetRow = 33;
                s = "";
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "34").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "34"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabituales[6].valor[3]) + "34"].Value = null;

                objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._IndicesHabituales[6].valor[3];
            }
            // If (Me._dataSet._IndicesHabituales(6).invertido(3)) Then
            // s = "*"
            // End If
            // If (Me._dataSet._IndicesHabituales(6).indeterminacion(3)) Then
            // s = s & "**"
            // End If
            // range(1, 2) = s
            else
            {
                 objSheet.Cells["E19"].Value=("#");
                 objSheet.Cells["E24"].Value=("#");
                 objSheet.Cells["E29"].Value=("#");
                 objSheet.Cells["E34"].Value=("#");
            }

            // +++++++++++++++++++++++++++++++++++++++++++
            // +++++ Indices IAG +++++++++++++++++++++++++
            // +++++++++++++++++++++++++++++++++++++++++++
            // objSheet.Cells["G39");
            // range.get_Resize(4, 1);

             //objSheet.Cells[39,7,43,8];
            offsetRow = 38;
            offsetCol = 6;
            int auxCol = 39;
            for (i = 0; i <= 3; i++)
            {
                objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _dataSet._IndiceIAG[i];
                //((Worksheet)m_Excel.Sheets[16]).Select();
                //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + (auxCol + i).ToString()).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + "14"].Copy(objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + (auxCol + i).ToString()]);
                objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG[i]) + (auxCol + i).ToString()].Value = null;
            }
        }

        public void EscribirInforme7c() //Recolocado (offset)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            // Dim pos As Integer
            string s;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(17);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[22]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();
            // objSheet.Cells["D15");
            // range.get_Resize(7, 2);

             //objSheet.Cells[15,4,22,6];

            int offsetRow = 14;
            int offsetCol = 3;

            if (_dataSet._IndicesHabitualesAgregados[0].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[0].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[0].valor[0]) + "15").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[0].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[0].valor[0]) + "15"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[0].valor[0]) + "15"].Value = null;

                objSheet.Cells[offsetRow + 1, offsetCol + 1].Value = _dataSet._IndicesHabitualesAgregados[0].valor[0];
                if (_dataSet._IndicesHabitualesAgregados[0].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabitualesAgregados[0].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 1, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow+1, offsetCol + 2].Value = "#";
            }

            if (_dataSet._IndicesHabitualesAgregados[1].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[1].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[1].valor[0]) + "16").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[1].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[1].valor[0]) + "16"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[1].valor[0]) + "16"].Value = null;

                objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._IndicesHabitualesAgregados[1].valor[0];
                if (_dataSet._IndicesHabitualesAgregados[1].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabitualesAgregados[1].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+2, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow+2, offsetCol + 2].Value = "#";
            }

            if (_dataSet._IndicesHabitualesAgregados[2].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[2].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[2].valor[0]) + "17").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[2].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[2].valor[0]) + "17"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[2].valor[0]) + "17"].Value = null;

                objSheet.Cells[offsetRow+3, offsetCol + 1].Value = _dataSet._IndicesHabitualesAgregados[2].valor[0];
                if (_dataSet._IndicesHabitualesAgregados[2].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabitualesAgregados[2].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+3, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow+3, offsetCol + 2].Value = "#";
            }

            if (_dataSet._IndicesHabitualesAgregados[3].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[3].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[3].valor[0]) + "18").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[3].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[3].valor[0]) + "18"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[3].valor[0]) + "18"].Value = null;


                objSheet.Cells[offsetRow+4, offsetCol + 1].Value = _dataSet._IndicesHabitualesAgregados[3].valor[0];
                if (_dataSet._IndicesHabitualesAgregados[3].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabitualesAgregados[3].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow+4, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow+4, offsetCol + 2].Value = "#";
            }

            if (_dataSet._IndicesHabitualesAgregados[4].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[4].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[4].valor[0]) + "19").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[4].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[4].valor[0]) + "19"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[4].valor[0]) + "19"].Value = null;

                objSheet.Cells[offsetRow + 5, offsetCol + 1].Value = _dataSet._IndicesHabitualesAgregados[4].valor[0];
                if (_dataSet._IndicesHabitualesAgregados[4].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabitualesAgregados[4].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 5, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 5, offsetCol + 2].Value = "#";
            }

            if (_dataSet._IndicesHabitualesAgregados[5].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[5].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[5].valor[0]) + "20").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[5].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[5].valor[0]) + "20"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[5].valor[0]) + "20"].Value = null;

                objSheet.Cells[offsetRow + 6, offsetCol + 1].Value = _dataSet._IndicesHabitualesAgregados[5].valor[0];
                if (_dataSet._IndicesHabitualesAgregados[5].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabitualesAgregados[5].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 6, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 6, offsetCol + 2].Value = "#";
            }

            if (_dataSet._IndicesHabitualesAgregados[6].calculado)
            {
                s = "";
                //((Worksheet)m_Excel.Sheets[17]).Select();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[6].valor[0]) + "14").Select();
                //((Range)m_Excel.Selection).Copy();
                //m_Excel.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[6].valor[0]) + "21").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[6].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[6].valor[0]) + "21"]);
                objSheet.Cells[DarColumna(_dataSet._IndicesHabitualesAgregados[6].valor[0]) + "21"].Value = null;

                objSheet.Cells[offsetRow + 7, offsetCol + 1].Value = _dataSet._IndicesHabitualesAgregados[6].valor[0];
                if (_dataSet._IndicesHabitualesAgregados[6].invertido[0])
                {
                    s = "*";
                }

                if (_dataSet._IndicesHabitualesAgregados[6].indeterminacion[0])
                {
                    s = s + "**";
                }

                objSheet.Cells[offsetRow + 7, offsetCol + 2].Value = s;
            }
            else
            {
                objSheet.Cells[offsetRow + 7, offsetCol + 2].Value = "#";
            }

             objSheet.Cells["G44"].Value = _dataSet._IndiceIAG_Agregados;
            //((Worksheet)m_Excel.Sheets[17]).Select();
            //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Agregados) + "14").Select();
            //((Range)m_Excel.Selection).Copy();
            //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Agregados) + "44").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

            objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Agregados) + "14"].Copy(objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Agregados) + "44"]);
            objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Agregados) + "44"].Value = null;

            // objSheet.Cells["F27");
            // range.get_Resize(12, 3);

             //objSheet.Cells[27,6,39,9];
            offsetRow = 26;
            offsetCol = 5;
            for (i = 0; i <= 11; i++)
            {
                s = "";
                if (_dataSet._IndiceM3Agregados.invertido[i] == true)
                {
                    s += "*";
                }

                if (_dataSet._IndiceM3Agregados.indeterminacion[i] == true)
                {
                    s += "**";
                }

                objSheet.Cells[offsetRow + i + 1, offsetCol + 1].Value = _dataSet._IndiceM3Agregados.valor[i];
                if (s != "")
                {
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 2].Value = s;
                }
                
                    s = "";
                if (_dataSet._IndiceV3Agregados.invertido[i] == true)
                {
                    s += "*";
                }
 
                if (_dataSet._IndiceV3Agregados.indeterminacion[i] == true)
                {
                    s += "**";
                }
                objSheet.Cells[offsetRow + i + 1, offsetCol + 3].Value = _dataSet._IndiceV3Agregados.valor[i];

                if (s != "")
                {
                    objSheet.Cells[offsetRow + i + 1, offsetCol + 4].Value = s;
                
                }
            }

            // Escribir los meses
            // objSheet.Cells["D27");
            // range.get_Resize(12, 1);

             //objSheet.Cells[27,4,39,5];
            
            offsetCol = 3;
            for (i = 0; i <= 11; i++)
                // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
                objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);
        }

        public void EscribirInforme7d() //Recolocado (ad-hoc)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            int pos;
            int posCelda;
            string s;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(18);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[23]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();
            pos = 0;
            posCelda = 15;
            for (i = 0; i <= 7; i++)
            {
                // objSheet.Cells["D" + posCelda.ToString());
                // range.get_Resize(1, 2);

                // objSheet.Cells["D" + posCelda.ToString()+":"+"E"+(posCelda+2).ToString()];

                if (_dataSet._IndicesAvenidas[pos].valor[0] >= 0f)
                {
                    s = "";
                    if (_dataSet._IndicesAvenidas[pos].invertido[0])
                    {
                        s = "*";
                    }

                    if (_dataSet._IndicesAvenidas[pos].indeterminacion[0])
                    {
                        s = s + "**";
                    }
                    
                    //objSheet.Cells[1, 2].Value = s;
                    objSheet.Cells["E" + posCelda.ToString()].Value = s;
                    //((Worksheet)m_Excel.Sheets[18]).Select();
                    //m_Excel.Cells[DarColumna(_dataSet._IndicesAvenidas[pos].valor[0]) + "14").Select();
                    //((Range)m_Excel.Selection).Copy();
                    //m_Excel.Cells[DarColumna(_dataSet._IndicesAvenidas[pos].valor[0]) + posCelda.ToString()).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                    //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                    objSheet.Cells[DarColumna(_dataSet._IndicesAvenidas[pos].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesAvenidas[pos].valor[0]) + posCelda.ToString()]);
                    objSheet.Cells[DarColumna(_dataSet._IndicesAvenidas[pos].valor[0]) + posCelda.ToString()].Value = null;

                    objSheet.Cells["D" + posCelda.ToString() ].Value = _dataSet._IndicesAvenidas[pos].valor[0];
                }
                else
                {
                    objSheet.Cells["E" + posCelda.ToString()].Value = "#";
                }

                pos = pos + 1;
                posCelda = posCelda + 1;
            }

            pos = 0;
            posCelda = 23;
            for (i = 0; i <= 6; i++)
            {
                // objSheet.Cells["D" + posCelda.ToString());
                // range.get_Resize(1, 2);
                 //objSheet.Cells["D" + posCelda.ToString() + ":" + "E" + (posCelda + 2).ToString()];

                if (_dataSet._IndicesSequias[pos].valor[0] >= 0f)
                {
                    s = "";
                    if (_dataSet._IndicesSequias[pos].invertido[0])
                    {
                        s = "*";
                    }

                    if (_dataSet._IndicesSequias[pos].indeterminacion[0])
                    {
                        s = s + "**";
                    }

                    objSheet.Cells["E" + posCelda.ToString()].Value = s;
                    //((Worksheet)m_Excel.Sheets[18]).Select();
                    //m_Excel.Cells[DarColumna(_dataSet._IndicesSequias[pos].valor[0]) + "14").Select();
                    //((Range)m_Excel.Selection).Copy();
                    //m_Excel.Cells[DarColumna(_dataSet._IndicesSequias[pos].valor[0]) + posCelda.ToString()).PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
                    //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

                    objSheet.Cells[DarColumna(_dataSet._IndicesSequias[pos].valor[0]) + "14"].Copy(objSheet.Cells[DarColumna(_dataSet._IndicesSequias[pos].valor[0]) + posCelda.ToString()]);
                    objSheet.Cells[DarColumna(_dataSet._IndicesSequias[pos].valor[0]) + posCelda.ToString()].Value = null;

                    objSheet.Cells["D" + posCelda.ToString()].Value = _dataSet._IndicesSequias[pos].valor[0];
                }
                else
                {
                    objSheet.Cells["E" + posCelda.ToString()].Value = "#";
                }

                pos = pos + 1;
                posCelda = posCelda + 1;
            }

            int aux = 34;
            for (i = 0; i <= 11; i++)
            {
                // objSheet.Cells["K" + aux);
                // range.get_Resize(1, 3);
                 //objSheet.Cells["K" + aux+":"+"L"+(aux+3).ToString()];
                // If (Me._dataSet._IndicesAvenidasI16MesesInversos(i)) Then
                // range(1, 1) = Me._dataSet._IndicesAvenidasI16Meses(i) & " *"
                // Else
                objSheet.Cells["K" + aux].Value = _dataSet._IndicesAvenidasI16Meses[i];
                // End If

                // If (Me._dataSet._IndicesSequiasI23MesesInversos(i)) Then
                // range(1, 2) = Me._dataSet._IndicesSequiasI23Meses(i) & " *"
                // Else
                objSheet.Cells["L" + aux].Value = _dataSet._IndicesSequiasI23Meses[i];
                // End If
                // If (Me._dataSet._IndicesSequiasI24MesesInversos(i)) Then
                // range(1, 3) = Me._dataSet._IndicesSequiasI24Meses(i) & " *"
                // Else
                objSheet.Cells["M" + aux].Value = _dataSet._IndicesSequiasI24Meses[i];
                // End If


                aux = aux + 1;
            }

             objSheet.Cells["G49"].Value=_dataSet._IndiceIAG_Ave;

            //((Worksheet)m_Excel.Sheets[18]).Select();
            //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Ave) + "14").Select();
            //((Range)m_Excel.Selection).Copy();
            //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Ave) + "49").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

            objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Ave) + "14"].Copy(objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Ave) + "49"]);
            objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Ave) + "49"].Value = null;

             objSheet.Cells["G50"].Value=_dataSet._IndiceIAG_Seq;

            //((Worksheet)m_Excel.Sheets[18]).Select();
            //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Seq) + "14").Select();
            //((Range)m_Excel.Selection).Copy();
            //m_Excel.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Seq) + "50").PasteSpecial(Paste: (Microsoft.Office.Interop.Excel.XlPasteType)IAHRISConstants.xlPasteFormats, Operation: (Microsoft.Office.Interop.Excel.XlPasteSpecialOperation)IAHRISConstants.xlNone, SkipBlanks: false, Transpose: false);
            //m_Excel.CutCopyMode = (Microsoft.Office.Interop.Excel.XlCutCopyMode)Conversions.ToInteger(false);

            objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Seq) + "14"].Copy(objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Seq) + "50"]);
            objSheet.Cells[DarColumnaGlobales(_dataSet._IndiceIAG_Seq) + "50"].Value = null;

            // Escribir los meses
            // objSheet.Cells["I34");
            // range.get_Resize(12, 1);

             //objSheet.Cells[34,9,46,10];
            int offsetRow = 33;
            int offsetCol = 8;
            for (i = 0; i <= 11; i++)
                // Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                objSheet.Cells[offsetRow+i + 1,offsetCol+ 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString());
        }

        public void EscribirInforme8() //Recolocado (offset)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(19);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[24]);

            // Escribir cabecera
             objSheet.Cells["D5"].Value=_datos.sNombre;
             objSheet.Cells["D6"].Value = _datos.sAlteracion;
             objSheet.Cells["D7"].Value=DateTime.Now.ToShortDateString();
            // Escribir años usados
            // objSheet.Cells["B36");
            // range.get_Resize(1, 3);

             //objSheet.Cells[36,2,37,5];
            int offsetRow = 35;
            int offsetCol = 1;
            objSheet.Cells[offsetRow+1, offsetCol+ 1].Value = _dataSet._AportacionNatAnual.año.Length;
            objSheet.Cells[offsetRow+1, offsetCol + 2].Value = _dataSet._AportacionAltAnual.año.Length;
            objSheet.Cells[offsetRow+1, offsetCol + 3].Value = _datos.nAnyosCoe;

            // Escribir las regimen mensual
            // objSheet.Cells["C14");
            // range.get_Resize(12, 3);

             //objSheet.Cells[14,3,26,6];
            //offsetRow = 13;
            //offsetCol = 2;
            //for (i = 0; i <= 11; i++)
            //{
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _dataSet._percentil10[i];
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 2].Value = _dataSet._medianaMenNat[i];
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 3].Value = _dataSet._percentil90[i];
            //}

            // objSheet.Cells["F14");
            // range.get_Resize(12, 2);

             //objSheet.Cells[14,6,26,8];
            
            //offsetCol = 5;

            //for (i = 0; i <= 11; i++)
            //{
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _dataSet._medianaMenAlt[i];
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 2].Value = _dataSet._mesesQueCumplen[i];
            //}

            //// Escribir regimen anual
            //// objSheet.Cells["C32");
            //// range.get_Resize(1, 5);

            // //objSheet.Cells[32,3,33,8];
            //offsetRow = 31;
            //offsetCol = 2;

            //objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._percentil10Anual;
            //objSheet.Cells[offsetRow+1, offsetCol + 2].Value = _dataSet._medianaAnualNat;
            //objSheet.Cells[offsetRow+1, offsetCol + 3].Value = _dataSet._percentil90Anual;
            //objSheet.Cells[offsetRow+1, offsetCol + 4].Value = _dataSet._medianaAnualAlt;
            //objSheet.Cells[offsetRow+1, offsetCol + 5].Value = _dataSet._anyosQueCumplen;

            //// Leer los resultados y rellenar los colores e etiquetas
            //// objSheet.Cells["I14");
            //// range.get_Resize(12, 2);

            // //objSheet.Cells[14,9,26,11];
            //offsetRow = 13;
            //offsetCol = 8;
            //objSheet.Calculate();
            //for (i = 0; i <= 11; i++)
            //{
               
            //        if ((double)(objSheet.Cells[offsetRow + (i + 1), offsetCol + 1].Value) > 50)
            //        {
            //            //((Range)objSheet.Cells[offsetRow+i + 1, 2]).Interior.Color = ColorTranslator.ToWin32(Color.Green);
            //            (objSheet.Cells[offsetRow + i + 1, offsetCol + 2]).Style.Fill.SetBackground(Color.Green);
            //        }
               
            //}
            //  objSheet.Range("I26")
            //  range.Resize(2, 2)
            // If (range(1, 1).Value < 50) Then
            // range(1, 2).Value = "MUY ALTERADA"
            // End If
            //  objSheet.Range("I32")
            //  range.Resize(2, 2)
            // If (range(1, 1).Value < 50) Then
            // range(1, 2).Value = "MUY ALTERADA"
            // End If

            // Escribir los meses
            // objSheet.Cells["B14");
            // range.get_Resize(12, 1);

             //objSheet.Cells[14,2,26,3];
            offsetRow = 13;
            offsetCol = 1;

            for (i = 0; i <= 11; i++)
                // Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
                objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString());
            objSheet.Protection.IsProtected = true; // .Protect(DrawingObjects: true, Contents: true, Scenarios: true);
            //objSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
            objSheet.Protection.AllowSelectLockedCells = false;
        }

        public void EscribirInforme8a() //Revisado (Void)
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(20);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[25]);

            //objSheet.Protect(DrawingObjects: true, Contents: true, Scenarios: true);
            //objSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
            objSheet.Protection.IsProtected = true; 
            objSheet.Protection.AllowSelectLockedCells = false;
        }

        public void EscribirInforme8b() //Revisado (void)
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            
            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(21);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[26]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            //objSheet.Protect(DrawingObjects: true, Contents: true, Scenarios: true);
            //objSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
            objSheet.Protection.IsProtected = true;
            objSheet.Protection.AllowSelectLockedCells = false;
        }

        public void EscribirInforme8c() //Revisado (void)
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(22);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[27]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            //objSheet.Protect(DrawingObjects: true, Contents: true, Scenarios: true);
            //objSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
            objSheet.Protection.IsProtected = true;
            objSheet.Protection.AllowSelectLockedCells = false;
        }

        public void EscribirInforme8d() //Revisado (void)
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(23);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[28]);

            objSheet.Cells["E5"].Value = _datos.sNombre;
            objSheet.Cells["E6"].Value = _datos.sAlteracion;
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            //objSheet.Protect(DrawingObjects: true, Contents: true, Scenarios: true);
            //objSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
            objSheet.Protection.IsProtected = true;
            objSheet.Protection.AllowSelectLockedCells = false;
        }

        public void EscribirInforme9() //Revisado (offset)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(24);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[29]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();

            // Escribir las aportaciones
            // objSheet.Cells["C16");
            // range.get_Resize(12, 3);

             //objSheet.Cells[16,3,28,6];

            int offsetRow = 15;
            int offsetCol = 2;

            //for (i = 0; i <= 11; i++)
            //{
            //    objSheet.Cells[offsetRow+i + 1, offsetCol+ 1].Value = _dataSet._IntraAnualNat[i][0];
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 2].Value = _dataSet._IntraAnualNat[i][1];
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 3].Value = _dataSet._IntraAnualNat[i][2];
            //}

            // Escribir las aportaciones
            // objSheet.Cells["C98");
            // range.get_Resize(12, 1);

             //objSheet.Cells[98,3,110,4];

            offsetRow = 97;

            objSheet.Cells[offsetRow+1, offsetCol + 1].Value = _dataSet._1QMin;
            objSheet.Cells[offsetRow+2, offsetCol + 1].Value = _dataSet._7QMin;
            objSheet.Cells[offsetRow+3, offsetCol + 1].Value = _dataSet._15QMin;
            objSheet.Cells[offsetRow+4, offsetCol + 1].Value = _dataSet._7QRetorno[0];
            objSheet.Cells[offsetRow+5, offsetCol + 1].Value = _dataSet._7QRetorno[1];
            objSheet.Cells[offsetRow+6, offsetCol + 1].Value = _dataSet._7QRetorno[2];
            objSheet.Cells[offsetRow+7, offsetCol + 1].Value = _dataSet._10QRetorno[0];
            objSheet.Cells[offsetRow+8, offsetCol + 1].Value = _dataSet._10QRetorno[1];
            objSheet.Cells[offsetRow+9, offsetCol + 1].Value = _dataSet._10QRetorno[2];
            objSheet.Cells[offsetRow+10, offsetCol + 1].Value = _dataSet._mnQ[0];
            objSheet.Cells[offsetRow+11, offsetCol + 1].Value = _dataSet._mnQ[1];
            objSheet.Cells[offsetRow+12, offsetCol + 1].Value = _dataSet._mnQ[2];

            // Escribir los meses
            // objSheet.Cells["B16");
            // range.get_Resize(12, 1);

             //objSheet.Cells[16,2,28,3];
            //offsetRow = 15;
            //offsetCol = 1;
            //for (i = 0; i <= 11; i++)
            //    // Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString());

            //// Escribir los meses
            //// objSheet.Cells["L16");
            //// range.get_Resize(12, 1);

            // //objSheet.Cells[16,12,28,13];
            //offsetCol = 11;
            //for (i = 0; i <= 11; i++)
            //    // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString());
            ////objSheet.Protect(DrawingObjects: true, Contents: true, Scenarios: true);
            ////objSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;

            objSheet.Protection.IsProtected = true;
            objSheet.Protection.AllowSelectLockedCells = false;
        }

        public void EscribirInforme9a() //Revisado (offset)
        {
            //Microsoft.Office.Interop.Excel.Range range;
            ExcelRange range;
            int i;
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //var objSheets = m_Excel.Worksheets;
            //Microsoft.Office.Interop.Excel._Worksheet objSheet = (Microsoft.Office.Interop.Excel._Worksheet)objSheets.get_Item(25);

            ExcelWorksheet objSheet = ((ExcelWorksheet)m_Excel.Workbook.Worksheets[30]);

             objSheet.Cells["E5"].Value=_datos.sNombre;
             objSheet.Cells["E6"].Value=_datos.sAlteracion;
             objSheet.Cells["E7"].Value=DateTime.Now.ToShortDateString();

            // Escribir las aportaciones
            // objSheet.Cells["C16");
            // range.get_Resize(12, 3);

             //objSheet.Cells[16,3,28,6];
            //int offsetRow = 15;
            //int offsetCol = 2;
            //for (i = 0; i <= 11; i++)
            //{
            //    objSheet.Cells[offsetRow+i + 1, offsetCol+ 1].Value = _dataSet._IntraAnualNat[i][0];
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 2].Value = _dataSet._IntraAnualNat[i][1];
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 3].Value = _dataSet._IntraAnualNat[i][2];
            //}

            //// Escribir las aportaciones
            ////  objSheet.Range("C98")
            ////  range.Resize(12, 1)
            //// range(1, 1) = Me._1QMin
            //// range(2, 1) = Me._7QMin
            //// range(3, 1) = Me._1QMin
            //// range(4, 1) = Me._7QRetorno(0)
            //// range(5, 1) = Me._7QRetorno(1)
            //// range(6, 1) = Me._7QRetorno(2)
            //// range(7, 1) = Me._10QRetorno(0)
            //// range(8, 1) = Me._10QRetorno(1)
            //// range(9, 1) = Me._10QRetorno(2)
            //// range(10, 1) = Me._mnQ(0)
            //// range(11, 1) = Me._mnQ(1)
            //// range(12, 1) = Me._mnQ(2)

            //// Escribir los meses
            //// objSheet.Cells["B16");
            //// range.get_Resize(12, 1);

            // //objSheet.Cells[16,2,28,3];
            //offsetCol = 1;

            //for (i = 0; i <= 11; i++)
            //    // Dim strmes As STRING_MES_COMPLETO = (i + Me._datos.mesInicio - 1) Mod 12
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString());

            //// Escribir los meses
            //// objSheet.Cells["L16");
            //// range.get_Resize(12, 1);

            // //objSheet.Cells[16,12,28,13];
            //offsetCol = 11;
            //for (i = 0; i <= 11; i++)
            //    // Dim strmes As STRING_MES_ORD = (i + Me._datos.mesInicio - 1) Mod 12
            //    objSheet.Cells[offsetRow+i + 1, offsetCol + 1].Value = _traductor.traducirMensaje(MultiLangXML.MultiIdiomasXML.TIPO_MENSAJE.M_MONTH, ((i + _datos.mesInicio - 1) % 12 + 1).ToString());
            ////objSheet.Protect(DrawingObjects: true, Contents: true, Scenarios: true);
            ////objSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
            objSheet.Protection.IsProtected = true;
            objSheet.Protection.AllowSelectLockedCells = false;
        }

        private string DarColumna(float valor)
        {
            if (valor > 0.8d)
            {
                return "I";
            }
            else if (valor > 0.6d)
            {
                return "J";
            }
            else if (valor > 0.4d)
            {
                return "K";
            }
            else if (valor > 0.2d)
            {
                return "L";
            }
            else
            {
                return "M";
            }
        }

        private string DarColumnaGlobales(float valor)
        {
            if (valor > 0.64d)
            {
                return "I";
            }
            else if (valor > 0.36d)
            {
                return "J";
            }
            else if (valor > 0.16d)
            {
                return "K";
            }
            else if (valor > 0.04d)
            {
                return "L";
            }
            else
            {
                return "M";
            }
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
   
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */

        public void DeleteReport(int index)
        {
            if (m_Excel is null)
            {
                AbrirExcel();
            }

            //((Worksheet)m_Excel.Sheets[index]).Select();
            //m_Excel.ActiveWindow.SelectedSheets.Delete();
            m_Excel.Workbook.Worksheets.Delete(index);
           
        }

        private string GetInf7Sign(bool inverso, bool indeterminado, bool noCalculado)
        {
            if (noCalculado) return "#";
            if (inverso & !indeterminado) return "*";
            if (!inverso & indeterminado) return "**";
            if (inverso & indeterminado) return "***";
            return "";
        }
    }

    }
