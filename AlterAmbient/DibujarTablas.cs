using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAHRIS
    {/// <summary>
    /// Clase para dibujar tablas de los datos de los puntos, alteraciones y proyectos
    /// </summary>
    internal class DibujarTablas
    {
        //Ancho estandar
        public static int tableWidth = 90;

        /// <summary>
        /// Para imprimir lineas separadoras intermedias.
        /// </summary>
        /// <param name="col">Número de columnas</param>
        public static void PrintLine(int col) {

            if (col==2)
            {
                Console.WriteLine("╠" + new string('═', (tableWidth - 1) / 2) + "╬" + new string('═', (tableWidth - 1)/2) + "╣"); 
            }
            else if (col==4)
            {
                Console.WriteLine("╠" + new string('═', (tableWidth - 1) / 4) + "╬" + new string('═', (tableWidth - 1) / 4) + "╬" + new string('═', (tableWidth - 1) / 4) + "╬" + new string('═', (tableWidth - 1) / 4) + "╣");
            }
            else if (col == 6)
            {
                Console.WriteLine("╠" + new string('═', (tableWidth - 1) / 6) + "╬" + new string('═', (tableWidth - 1) / 6) + "╬" + new string('═', (tableWidth - 1) / 6) + "╬" + new string('═', (tableWidth - 1) / 6) + "╬" + new string('═', (tableWidth - 1) / 6) + "╬" + new string('═', (tableWidth - 1) / 6) + "╣");
            }
            else
            {
                Console.WriteLine("╠" + new string('═', tableWidth - 1)  + "╣");
            }

            
        }

        /// <summary>
        /// Para imprimir la primera linea de una tabla.
        /// </summary>
        /// <param name="col">Número de columnas</param>
        public static void PrintLineInicio(int col)
        {
            if (col == 2)
            {
                Console.WriteLine("╔" + new string('═', (tableWidth - 1) / 2) + "╦" + new string('═', (tableWidth - 1) / 2) + "╗");
            }
            else if (col == 4)
            {
                Console.WriteLine("╔" + new string('═', (tableWidth - 1) / 4) + "╦" + new string('═', (tableWidth - 1) / 4) + "╦" + new string('═', (tableWidth - 1) / 4) + "╦" + new string('═', (tableWidth - 1) / 4) + "╗");
            }
            else if (col == 6)
            {
                Console.WriteLine("╔" + new string('═', (tableWidth - 1) / 6) + "╦" + new string('═', (tableWidth - 1) / 6) + "╦" + new string('═', (tableWidth - 1) / 6) + "╦" + new string('═', (tableWidth - 1) / 6) + "╦" + new string('═', (tableWidth - 1) / 6) + "╦" + new string('═', (tableWidth - 1) / 6) + "╗");
            }
            else
            {
                Console.WriteLine("╔" + new string('═', tableWidth - 1) + "╗");
            }

            
        }
        /// <summary>
        /// Para imprimir la última linea de una tabla.
        /// </summary>
        /// <param name="col">Número de columnas</param>
        public static void PrintLineFinal(int col)
        {
            if (col == 2)
            {
                Console.WriteLine("╚" + new string('═', (tableWidth - 1) / 2) + "╩" + new string('═', (tableWidth - 1) / 2) + "╝");
            }
            else if (col == 4)
            {
                Console.WriteLine("╚" + new string('═', (tableWidth - 1) / 4) + "╩" + new string('═', (tableWidth - 1) / 4) + "╩" + new string('═', (tableWidth - 1) / 4) + "╩" + new string('═', (tableWidth - 1) / 4) + "╝");
            }
            else if (col == 6)
            {
                Console.WriteLine("╚" + new string('═', (tableWidth - 1) / 6) + "╩" + new string('═', (tableWidth - 1) / 6) + "╩" + new string('═', (tableWidth - 1) / 6) + "╩" + new string('═', (tableWidth - 1) / 6) + "╩" + new string('═', (tableWidth - 1) / 6) + "╩" + new string('═', (tableWidth - 1) / 6) + "╝");
            }
            else
            {
                Console.WriteLine("╚" + new string('═', tableWidth - 1) + "╝");
            }



            
        }

        /// <summary>
        /// Para imprimir los datos de una fila.
        /// </summary>
        /// <param name="columns">Array de string con los datos de una fila de la tabla</param>
        public static void PrintRow(params string[] columns) 
        {
            if (columns.Length != 6)
            {
                int width = (tableWidth - columns.Length) / columns.Length;
                string row = "║";
                foreach (string column in columns)
                {
                    row += AlignCentre(column, width) + "║";
                }
                Console.WriteLine(row);
            }
            else
            {
                int width = ((tableWidth+6) - columns.Length) / columns.Length;
                string row = "║";
                foreach (string column in columns)
                {
                    row += AlignCentre(column, width) + "║";
                }
                Console.WriteLine(row);
            }

        }

        /// <summary>
        /// Funcion para alinear el texto de una celda
        /// </summary>
        /// <param name="text">Texto de la celda</param>
        /// <param name="width">Ancho de la celda</param>
        /// <returns></returns>
        public static string AlignCentre(string text, int width) 
        { 
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text; 
            if (string.IsNullOrEmpty(text)) 
            { 
                return new string(' ', width); 
            } else 
            { 
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width); 
            } 
        }
    }
}
