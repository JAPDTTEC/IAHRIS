using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CargaPorLotes.Logica
{
    public static class EjecutarFicheroBat
    {
        /// <summary>
        /// Lanza el proceso de ejecución del .bat de forma síncrona.
        /// </summary>
        /// <returns></returns>
        public static bool Ejecutar(string fileName)
        {
            try
            {
                ////Validar Ruta de IAHRIS
                //if (string.IsNullOrEmpty(fileName) || !Directory.Exists(fileName))                
                //    throw new Exception("Ruta de IAHRIS no encontrada");                

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = false;
                psi.CreateNoWindow = false;
                psi.WindowStyle = ProcessWindowStyle.Normal;
                psi.FileName = fileName;

                Process process = Process.Start(psi);

                process.WaitForExit();
                process.Close();

                return true;
            }
            catch (Exception ex) 
            {
                throw ex;
            }

        }

        /// <summary>
        /// Lanza el proceso de ejecución del .bat en asincrono.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Task<bool> EjecutarAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<bool>();

            var process = new Process
            {
                StartInfo = { FileName = fileName },
                EnableRaisingEvents = true,
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(false);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }




    }
}
