using IAHRIS.Calculo.CaudalesEcologicos;
using IAHRIS.Calculo.CaudalesEcologicos.Metodos;
using IAHRIS.Calculo.CaudalesEcologicos.Tipologias;
using IAHRIS.Calculo.Tipologias;
using MultiLangXML;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using static IAHRIS.Calculo.TestFechas;

namespace IAHRIS.Calculo
{
    public class ReportController
    {
        Reports _reports;
        DatosCalculo _datos;
        IAHRISDataSet _dataset;
        MultiIdiomasXML _traductor;

        public ReportController(MultiIdiomasXML traductor)
        {
            _dataset = new IAHRISDataSet();
            _reports = new Reports(traductor);
            _traductor = traductor;
        }


        public void GenerarInforme(Tipologia tipologia, DatosCalculo datos, string ruta)
        {
            _datos = datos;

            bool coeDiara = _datos._simulacion.usarCoeDiara;
            bool usarCoe = _datos._simulacion.usarCoe;

            ExcelPackage m_Excel = _reports.EscribirCabecera(ref _datos, ref _dataset);
            tipologia.ProcesarInformes(m_Excel, _traductor, _datos, ref _dataset);

            //Generar fichero
            EscribirFichero(coeDiara, usarCoe, datos, ruta);

        }

        public void GenerarInformeCE(TipologiaCE tipologia, Simulacion simulacion, SerieRCE serie,  string ruta)
        { 
            ExcelPackage excel = _reports.EscribirCabeceraCaudalesEcologicos(serie, simulacion);           
            tipologia.ProcesarInformes(excel, serie);

            //Generar fichero
            EscribirFicheroCE(excel, ruta);



        }



        private void EscribirFichero(bool coeD, bool coe, DatosCalculo datos,  string ruta=null)
        {
            if (ruta==null)
            {
                _reports.CerrarExcel(coeD, coe, datos);
            }
            else
            {
                _reports.CerrarExcelCmd(coeD, coe, datos, ruta);
            }
            
        }

        private void EscribirFicheroCE (ExcelPackage excel, string ruta) 
        {
            // Eliminamos la instancia de Excel de memoria
            if (excel != null)
            {
                excel.SaveAs(new FileInfo(ruta));
                
            }

        }
    }
}
