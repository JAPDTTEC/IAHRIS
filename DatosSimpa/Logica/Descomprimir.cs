
using DatosSimpa.DTO;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatosSimpa.Logica
{
    public class Descomprimir
    {
        public static ProgresoDescompresionForm _progresoDescompresionForm = new ProgresoDescompresionForm();
        public Descomprimir() { 
        
        }
        public async static Task DescomprimirRar(string ruta)
        {

            _progresoDescompresionForm.Show();
            _progresoDescompresionForm.UseWaitCursor = true;

            
           
            
                using (var archive = RarArchive.Open(ruta))
                {
                    IArchive rar = RarArchive.Open(new FileInfo(ruta));
                    string directory = Path.Combine(Directory.GetCurrentDirectory(), "DATA");

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // Calculate the total extraction size.
                double totalSize = rar.Entries.Where(e => !e.IsDirectory).Sum(e => e.Size);
                    long completed = 0;

                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                    {
                        

                        entry.WriteToDirectory(directory, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                        completed += entry.Size;
                        var percentage = completed / totalSize;
                        _progresoDescompresionForm.ActualizarEtiquetaArchivo(ruta.Split('\\')[ruta.Split('\\').Length-1]);
                        _progresoDescompresionForm.ActualizarEtiquetaPorcentaje((int)(percentage*100) + "%");
                        _progresoDescompresionForm.ActualizarEtiquetaDescompresion((completed / 1024 / 1024).ToString() + "MB/" + ((int)(totalSize / 1024 / 1024)).ToString() + "MB");
                        _progresoDescompresionForm.progressBarDesc.Maximum = (int)(totalSize / 1024);
                        _progresoDescompresionForm.progressBarDesc.Value = (int)(completed / 1024);
                        _progresoDescompresionForm.Update();
                    }
                }



            
            
        }



        public static List<FicheroSimpaDTO> DescomprimirRarCache(string ruta)
        {
            string[] filePaths = Directory.GetFiles(ruta, "*.rar", SearchOption.TopDirectoryOnly);
            List<FicheroSimpaDTO> ficherosRAR = new List<FicheroSimpaDTO>();
            string patronAño = @"(\d+)_\d+";

            
            foreach (string arch in filePaths)
            {
                using (RarArchive archive = RarArchive.Open(arch))
                {
                    using (MemoryStream memoryStream = new MemoryStream()) { 
                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                        {
                        
                            if (!Directory.Exists(ruta + "\\" + arch.Split('.')[0]) &&
                                string.Equals(Path.GetExtension(entry.Key), ".asc"))
                            {
                                Match match = Regex.Match(entry.Key, patronAño);
                                if (!match.Success)
                                    continue;

                                int año = int.Parse(match.Value.Split('_')[0]);
                                int mes = int.Parse(match.Value.Split('_')[1]);

                            FicheroSimpaDTO fichero = new FicheroSimpaDTO()
                            {
                                NombreFichero = entry.Key,
                                Fecha_Actualizacion = entry.LastModifiedTime.Value,
                                Año = año,
                                Mes = mes,
                                ContenidoRaster = memoryStream
                                };
                                entry.WriteTo(fichero.ContenidoRaster);
                                ficherosRAR.Add(fichero);
                                memoryStream.Flush();
                                
                            }
                            
                        }
                    }
                   
                }


            }

            return ficherosRAR;
        }

       
        public static double DatosArchivos(string filePath, UTMCoordinate punto)
        {
            string[] numerosString;
            
            List<List<double>> archivo = new List<List<double>>();

            StreamReader sr = new StreamReader(filePath);
            int i = 1;
            string lineaTexto = sr.ReadLine();
                while (lineaTexto != null)
                {  
                    List<double> linea = new List<double>();
                    if (i>6) 
                    { 
                        numerosString = lineaTexto.Split(' ');
                        foreach (var item in numerosString)
                        {
                           if (item!="")
                           {
                               linea.Add(double.Parse(item, CultureInfo.InvariantCulture));
                            
                           } 
                        }
                    archivo.Add(linea);
                }
                    i++;
                    
                    lineaTexto = sr.ReadLine();                   
                }              
                sr.Close();


            return archivo.ElementAt((int)punto.X-7).ElementAt((int)punto.Y-1);
            
        }
    }
}
