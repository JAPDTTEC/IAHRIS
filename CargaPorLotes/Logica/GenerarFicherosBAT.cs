using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CargaPorLotes.Logica
{
	public class GenerarFicherosBAT
	{
		private readonly StringBuilder _contenidoFichero;
		private readonly string _rutaFicheroBat;
		private readonly string _rutaExpInformes;
		private readonly bool _sobrescribirInformes;
		private readonly bool _exportar;
		private readonly string _root;

		public GenerarFicherosBAT(string ubicacionBat, bool sobrescribirInformes, bool exportar, string rutaExportar = "")
		{
			_contenidoFichero = new StringBuilder();
			_rutaFicheroBat = ubicacionBat;
			_rutaExpInformes = rutaExportar;
			_sobrescribirInformes = sobrescribirInformes;
			_exportar = exportar;


            _root = Application.StartupPath;

			//Validar Ruta de IAHRIS
			if (string.IsNullOrEmpty(_root) || !Directory.Exists(_root))
				throw new Exception("Ruta de IAHRIS no encontrada");
		}


		public bool CrearFicheroBat(List<ArchivoDatos> ficheros, string nombreProyecto, string descrProyecto, bool coeValHab, bool coeAveSeq, bool logs)
		{
			StringBuilder lineasCargaDatos = new StringBuilder();
			StringBuilder lineasExportar = new StringBuilder();

			string fichSalida = string.Format("salida_{0}.txt", DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" +
				DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

			//Obtener Lineas de Carga de Datos 
			foreach (ArchivoDatos punto in ficheros)
			{

                lineasCargaDatos.AppendLine(AñadirLineaCargaDatos(punto, nombreProyecto, descrProyecto, logs, fichSalida, punto.NombrePunto));

				//Alteraciones del punto.
                foreach (ArchivoDatos alt in punto.Alteraciones)
                    lineasCargaDatos.AppendLine(AñadirLineaCargaDatos(alt, nombreProyecto, descrProyecto, logs, fichSalida, punto.NombrePunto));


				if (_exportar)
					lineasExportar.AppendLine(AñadirLineasExportar(punto, nombreProyecto, coeValHab, coeAveSeq, _rutaExpInformes, logs, fichSalida));

			}

			//Crear fichero
			CrearFichero(lineasCargaDatos, lineasExportar);

			return true;

		}

		private void CrearFichero(StringBuilder lineasCargaDatos, StringBuilder lineasExportar)
		{
			_contenidoFichero.AppendLine("cd " + _root);
			_contenidoFichero.AppendLine("chcp 65001");
			_contenidoFichero.AppendLine(":: Lineas de Carga de Datos.");
			_contenidoFichero.AppendLine(lineasCargaDatos.ToString());
			_contenidoFichero.AppendLine(":: Lineas para Generar informe de Salida.");
			_contenidoFichero.AppendLine(lineasExportar.ToString());


            using (StreamWriter file = new StreamWriter(_rutaFicheroBat))
			{
				file.WriteLine(_contenidoFichero.ToString());

			}
		}

		private string AñadirLineasExportar(ArchivoDatos fichero, string nombreProyecto, bool coeValHab, 
												bool coeAveSeq, string rutaExportar, bool logs, string ficheroSalida)
		{
			StringBuilder linea = new StringBuilder();
			StringBuilder lineaPunto = new StringBuilder();
			StringBuilder lineasAlteraciones = new StringBuilder();
            
			string opcionesLineaPunto = "";
			string opcionesLineaAlter = "";

            if (_sobrescribirInformes)
                opcionesLineaPunto += "-n ";
			
            if (logs)
                opcionesLineaPunto += ">> \"" + Path.Combine(Path.GetDirectoryName(_rutaFicheroBat), ficheroSalida) + "\"";

			string lineaAuxPunto = string.Format("IAHRIS.exe GIS /t:P /np:\"{0}\" /p:\"{1}\" /fs:\"{2}\" ",
													fichero.NombrePunto, nombreProyecto, rutaExportar);

            if (opcionesLineaPunto != "")
                lineaAuxPunto += opcionesLineaPunto;

			if (fichero.Alteraciones.Count == 0)
				lineaPunto.AppendLine(lineaAuxPunto);


            foreach (ArchivoDatos alter in fichero.Alteraciones)
			{
                string lineAuxAlter = string.Format("IAHRIS.exe GIS /t:A /np:\"{0}\" /na:\"{1}\" /p:\"{2}\" /fs:\"{3}\" ",
                                    fichero.NombrePunto, alter.NombrePunto, nombreProyecto, rutaExportar);
                opcionesLineaAlter = "";

                if (_sobrescribirInformes)
                    opcionesLineaAlter += "-n ";

                if (coeValHab)
                    opcionesLineaAlter += "-cvh ";

                if (coeAveSeq)
                    opcionesLineaAlter += "-cas ";

                if (logs)
                    opcionesLineaAlter += ">> \"" + Path.Combine(Path.GetDirectoryName(_rutaFicheroBat), ficheroSalida) + "\"";

                if (opcionesLineaAlter != "")
                    lineAuxAlter += opcionesLineaAlter;

                lineasAlteraciones.AppendLine(lineAuxAlter);

                
            }

			linea.Append(lineaPunto.ToString());

			if (lineasAlteraciones.ToString() != "")
				linea.Append(lineasAlteraciones.ToString());


            return linea.ToString();

		}

		private string AñadirLineaCargaDatos(ArchivoDatos fichero, string nombreProyecto, string descrProyecto, bool logs, string ficheroSalida, string nombrePunto)
		{
			string comando;

			if (!fichero.EsAlteracion)
				comando = string.Format("IAHRIS.exe CD /t:P /np:\"{0}\" /d:\"{1}\" /p:\"{2}\" /dp:\"{3}\" /rn:\"{4}\" /ra:\"{5}\" /fe:\"{6}\" /mi:10",
					fichero.NombrePunto, fichero.Descripcion, nombreProyecto, descrProyecto, fichero.NombreRegimen, fichero.AbreviaturaRegimen, fichero.Ruta);
			else
			{
                comando = string.Format("IAHRIS.exe CD /t:A /np:\"{0}\" /na:\"{1}\" /d:\"{2}\" /p:\"{3}\" /dp:\"{4}\" /rn:\"{5}\" /ra:\"{6}\" /fe:\"{7}\" ",
                    nombrePunto, fichero.NombrePunto, fichero.Descripcion, nombreProyecto, descrProyecto, fichero.NombreRegimen, fichero.AbreviaturaRegimen, fichero.Ruta);				
            }

            if (logs)
				comando += ">> \"" + Path.Combine(Path.GetDirectoryName(_rutaFicheroBat), ficheroSalida) +"\"";

			return comando;
		}

	}
}

