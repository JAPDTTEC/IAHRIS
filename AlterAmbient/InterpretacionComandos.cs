using IAHRIS.Rellenar;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTable.Models;

namespace IAHRIS
{
    internal class InterpretacionComandos
    {
        public static void LecturaComandos(Dictionary<string,string> parametros, out string nombrePunto, out string nombreAlteracion, 
            out string descripcion, out string nombreProyecto, out string descripcionProyecto, out string nombreRegimen, out string abrevRegimen,
            out string csv, out string exportacion, out bool cvh, out bool cas, out string cm, out string pa, out string mesInicio)
        {

            nombrePunto = "";
            nombreAlteracion = "";
            descripcion = "";
            nombreProyecto = "";
            descripcionProyecto = "";
            nombreRegimen = "";
            abrevRegimen = "";
            csv = "";
            exportacion = "";
            cvh = false;
            cas = false;
            cm = "";
            pa = "";
            mesInicio = "";

            if (parametros.ContainsKey("CD"))
            {
                cm = "CD";
                parametros.TryGetValue("/t", out pa);

                if (pa == "P")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }
                    if (!parametros.TryGetValue("/d", out descripcion))
                    {
                        throw new Exception("Parámetro /d no encontrado");
                    }
                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/dp", out descripcionProyecto))
                    {
                        throw new Exception("Parámetro /dp no encontrado");
                    }
                    if (!parametros.TryGetValue("/rn", out nombreRegimen))
                    {
                        throw new Exception("Parámetro /rn no encontrado");
                    }
                    if (!parametros.TryGetValue("/ra", out abrevRegimen))
                    {
                        throw new Exception("Parámetro /ra no encontrado");
                    }
                    if (!parametros.TryGetValue("/fe", out csv))
                    {
                        throw new Exception("Parámetro /fe no encontrado");
                    }
                    if (!parametros.TryGetValue("/mi", out mesInicio))
                    {
                        throw new Exception("Parámetro /mi no encontrado");
                    }
                }
                else if (pa == "A")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }
                    if (!parametros.TryGetValue("/na", out nombreAlteracion))
                    {
                        throw new Exception("Parámetro /na no encontrado");
                    }
                    if (!parametros.TryGetValue("/d", out descripcion))
                    {
                        throw new Exception("Parámetro /d no encontrado");
                    }
                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/dp", out descripcionProyecto))
                    {
                        throw new Exception("Parámetro /dp no encontrado");
                    }
                    if (!parametros.TryGetValue("/rn", out nombreRegimen))
                    {
                        throw new Exception("Parámetro /rn no encontrado");
                    }
                    if (!parametros.TryGetValue("/ra", out abrevRegimen))
                    {
                        throw new Exception("Parámetro /ra no encontrado");
                    }
                    if (!parametros.TryGetValue("/fe", out csv))
                    {
                        throw new Exception("Parámetro /fe no encontrado");
                    }
                }
            }
            else if (parametros.ContainsKey("GIS"))
            {

                cm = "GIS";
                parametros.TryGetValue("/t", out pa);
                if (pa == "P")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }

                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/fs", out exportacion))
                    {
                        throw new Exception("Parámetro /fs no encontrado");
                    }

                }
                else if (pa == "A")
                {
                    if (!parametros.TryGetValue("/np", out nombrePunto))
                    {
                        throw new Exception("Parámetro /np no encontrado");
                    }
                    if (!parametros.TryGetValue("/na", out nombreAlteracion))
                    {
                        throw new Exception("Parámetro /na no encontrado");
                    }
                    if (!parametros.TryGetValue("/p", out nombreProyecto))
                    {
                        throw new Exception("Parámetro /p no encontrado");
                    }
                    if (!parametros.TryGetValue("/fs", out exportacion))
                    {
                        throw new Exception("Parámetro /fs no encontrado");
                    }
                    if (parametros.ContainsKey("-cvh"))
                    {
                        cvh = true;
                    }
                    if (parametros.ContainsKey("-cas"))
                    {
                        cas = true;
                    }
                }
                else
                {
                    throw new Exception("Parámetro /t incorrecto");
                }
                
            }
        }
    }
}
