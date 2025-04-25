using OfficeOpenXml;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public abstract class  Informe 
    {

        

        public bool Active;
        public int Index;
        public string NombreInformeXML;
        public string NombrePestaña;

        public Informe(bool active, int index, string nombreInformeXML, string nombrePestaña)
        {
            Active = active;
            Index = index;
            NombreInformeXML = nombreInformeXML;
            NombrePestaña = nombrePestaña;
        }   

        public  void Borrar(ExcelPackage excel)
        {
            excel.Workbook.Worksheets.Delete(NombrePestaña);
        }
        
        public abstract void Escribir(ExcelPackage excel, ref DatosCalculo datos, ref IAHRISDataSet dataset, MultiLangXML.MultiIdiomasXML traductor); 
        

    }
}
