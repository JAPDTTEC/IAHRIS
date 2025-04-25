using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using MultiLangXML;
using OfficeOpenXml;
using System.Collections.Generic;

namespace IAHRIS.Calculo.CaudalesEcologicos.Informes
{
    public abstract class Informe_RCE
    {
        public bool Active;
        public int Index;
        public string NombreInformeXML;
        public string NombrePestaña;

        public Informe_RCE(bool active, int index, string nombreInformeXML, string nombrePestaña)
        {
            Active = active;
            Index = index;
            NombreInformeXML = nombreInformeXML;
            NombrePestaña = nombrePestaña;
        }

        public void Borrar(ExcelPackage excel)
        {
            excel.Workbook.Worksheets.Delete(NombrePestaña);
        }

        public abstract void Escribir(ExcelPackage excel, SerieRCE serie);


    }
}
