using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CargaPorLotes.Logica
{
    public class ExportarExcel
    {
        public static void GenerarExcel(string rutaArchivo,List<ArchivoDatos> archivoDato)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileStream fs = new FileStream(rutaArchivo, FileMode.Create);
           
            ExcelPackage ep = new ExcelPackage(fs);
        
            ep.Workbook.Worksheets.Add("Informe");
            
            ExcelWorksheet ew1 = ep.Workbook.Worksheets[0];

            ew1.Cells[1, 1].Value = "Nombre Fichero";
            ew1.Cells[1, 2].Value = "Punto";
            ew1.Cells[1, 3].Value = "Alteración";
            ew1.Cells[1, 4].Value = "Estado";
            ew1.Cells[1, 5].Value = "Error";
            ew1.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ew1.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            int offsetRow = 2;
            int offsetCol = 1;


            foreach (ArchivoDatos dato in archivoDato)
            {
                string[] datosGrid = new string[5];

                datosGrid[0] = string.IsNullOrEmpty(dato.Ruta) ? "Alteración sin punto" : Path.GetFileName(dato.Ruta);
                datosGrid[1] = dato.NombrePunto;

                if (dato.Alteraciones != null)
                    foreach (ArchivoDatos alt in dato.Alteraciones)
                        datosGrid[2] += alt.NombrePunto + Environment.NewLine;

                datosGrid[3] = dato.Procesado ? "Procesado" : "No Procesado";
                datosGrid[4] = dato.Resultado;

                ew1.Cells[offsetRow, offsetCol].Value = datosGrid[0];
                ew1.Cells[offsetRow, offsetCol + 1].Value = datosGrid[1];
                ew1.Cells[offsetRow, offsetCol + 2].Value = datosGrid[2];
                ew1.Cells[offsetRow, offsetCol + 3].Value = datosGrid[3];
                ew1.Cells[offsetRow, offsetCol + 4].Value = datosGrid[4];
                offsetRow++;
            }
            ew1.Cells[1, 1, offsetRow, 5].AutoFitColumns();
            
            ep.Save();
            
            fs.Close();
            fs.Dispose();

            ep.Dispose();

        }
        
        
    }
}
