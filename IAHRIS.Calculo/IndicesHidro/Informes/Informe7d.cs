using MathNet.Numerics;
using MultiLangXML;
using OfficeOpenXml;
using System;
using System.Drawing;

namespace IAHRIS.Calculo.IndicesHidro.Informes
{
    public class Informe7d : Informe
    {
        public Informe7d(bool active, int index, string nombreInformeXML, string nombrePestaña) : base(active, index, nombreInformeXML, nombrePestaña)
        {
        }

        public override void Escribir(ExcelPackage excel, ref DatosCalculo datos,   ref IAHRISDataSet dataset, MultiIdiomasXML traductor)
        {
            Calculations calculations = new Calculations(datos, traductor, dataset);
            calculations.CalculoParametrosAvenidasAlteradosCASO6();
            calculations.CalculoParametrosSequiasAlteradosCASO6();
            calculations.CalcularIndicesAvenidasCASO6();
            calculations.CalcularIndicesSequiasCASO6();
            calculations.CalcularIndiceAlteracionGlobalAvenidas();
            calculations.CalcularIndiceAlteracionGlobalSequias();

            ExcelWorksheet objSheet = ((ExcelWorksheet)excel.Workbook.Worksheets[Index]);

            
            objSheet.Cells["E7"].Value = DateTime.Now.ToShortDateString();

            //AVENIDAS
            
            int filaIndiceAv = 17;
            for (int pos = 0; pos <= 7; pos++)
            {
                string s = "";
                string columnaNivel = DarColumna(dataset._IndicesAvenidas[pos].valor[0]);

                if (dataset._IndicesAvenidas[pos].valor[0] < 0f)
                {
                    objSheet.Cells["E" + filaIndiceAv.ToString()].Value = "#";
                    continue;
                }

                if (dataset._IndicesAvenidas[pos].invertido[0]) 
                    s = "*";

                if (dataset._IndicesAvenidas[pos].indeterminacion[0])
                    s += "**";

                objSheet.Cells["D" + filaIndiceAv.ToString()].Value = dataset._IndicesAvenidas[pos].valor[0];
                objSheet.Cells["E" + filaIndiceAv.ToString()].Value = s;
                objSheet.Cells[columnaNivel + "16"].Copy(objSheet.Cells[columnaNivel + filaIndiceAv.ToString()]);
                objSheet.Cells[columnaNivel + filaIndiceAv.ToString()].Value = null;

                filaIndiceAv++;

            }

            //SEQUIAS
            int filaIndiceSeq = 25;
            for (int pos = 0; pos <= 6; pos++)
            {

                if (dataset._IndicesSequias[pos].valor[0] < 0f)
                {
                    objSheet.Cells["E" + filaIndiceSeq.ToString()].Value = "#";
                    continue;
                }

                string s = "";
                string colNivelSeq = DarColumna(dataset._IndicesSequias[pos].valor[0]);

                if (dataset._IndicesSequias[pos].invertido[0])                    
                    s = "*";                    

                if (dataset._IndicesSequias[pos].indeterminacion[0])                    
                    s += "**";                    

                objSheet.Cells["E" + filaIndiceSeq.ToString()].Value = s;
                objSheet.Cells[colNivelSeq + "16"].Copy(objSheet.Cells[colNivelSeq + filaIndiceSeq.ToString()]);
                objSheet.Cells[colNivelSeq + filaIndiceSeq.ToString()].Value = null;

                objSheet.Cells["D" + filaIndiceSeq.ToString()].Value = dataset._IndicesSequias[pos].valor[0];

                filaIndiceSeq ++;
            }

            //Indices de Alteración Mensuales
            //TO DO: Revisar esto, es el metodo para añadir "&" en los indices IAH14 IAH20 e IAH21 pero no tengo claro cual es el criterio.

            int filaIndiceAltMensual = 36;
            for (int i = 0; i <= 11; i++)
            {
                objSheet.Cells["N" + filaIndiceAltMensual].Value = traductor.traducirMensaje(MultiIdiomasXML.TIPO_MENSAJE.M_MONTH,
                                                                                ((i + datos.mesInicio - 1) % 12 + 1).ToString()).Substring(0, 3);


                objSheet.Cells["P" + filaIndiceAltMensual].Value = dataset._IndicesAvenidasI16Meses[i];
                /*
                if (dataset._AveEstacionalidadAlt.ndias[i] > dataset._AveEstacionalidadNat.ndias[i])
                {
                    objSheet.Cells["Q" + filaIndiceAltMensual].Value = "&";
                    objSheet.Cells["Q" + filaIndiceAltMensual].Style.Font.Color.SetColor(Color.Red);
                }
                */
               
                //[18/10/2023]
                //Aqui se comprueba esencialmente el número de días de cada parámetro (avenida, sequia y cero) y se pone & si Alterado es mayor que natural. 
                if (dataset._AveEstacionalidadNat.ndias[i].Round(1) < dataset._AveEstacionalidadAlt.ndias[i].Round(1)) //numero de dias de avenida
                {
                    objSheet.Cells["Q" + filaIndiceAltMensual].Value = "&";
                    objSheet.Cells["Q" + filaIndiceAltMensual].Style.Font.Color.SetColor(Color.Red);
                }


                objSheet.Cells["R" + filaIndiceAltMensual].Value = dataset._IndicesSequiasI23Meses[i];

                if (dataset._SeqDuracionCerosMesNat.ndias[i].Round(1) < dataset._SeqDuracionCerosMesAlt.ndias[i].Round(1)) //numero de dias de caudal cero.
                {
                    objSheet.Cells["S" + filaIndiceAltMensual].Value = "&";
                    objSheet.Cells["S" + filaIndiceAltMensual].Style.Font.Color.SetColor(Color.Red);
                }

                objSheet.Cells["T" + filaIndiceAltMensual].Value = dataset._IndicesSequiasI24Meses[i];

                if (dataset._SeqEstacionalidadNat.ndias[i].Round(1) < dataset._SeqEstacionalidadAlt.ndias[i].Round(1))  //numero de dias de sequia
                {
                    objSheet.Cells["U" + filaIndiceAltMensual].Value = "&";
                    objSheet.Cells["U" + filaIndiceAltMensual].Style.Font.Color.SetColor(Color.Red);
                }

                filaIndiceAltMensual++;
            }

            //Indices de Alteración Global (IAG)
            string celdaNivelAvenidaIAG = DarColumnaGlobales(dataset._IndiceIAG_Ave);
            string celdaNivelSequiaIAG = DarColumnaGlobales(dataset._IndiceIAG_Seq);

            objSheet.Cells["G51"].Value = dataset._IndiceIAG_Ave;
            objSheet.Cells[celdaNivelAvenidaIAG + "16"].Copy(objSheet.Cells[celdaNivelAvenidaIAG + "51"]);
            objSheet.Cells[celdaNivelAvenidaIAG + "51"].Value = null;

            objSheet.Cells["G52"].Value = dataset._IndiceIAG_Seq;
            objSheet.Cells[celdaNivelSequiaIAG + "16"].Copy(objSheet.Cells[celdaNivelSequiaIAG + "52"]);
            objSheet.Cells[celdaNivelSequiaIAG + "52"].Value = null;

        }
        private string DarColumna(float valor)
        {
            if (valor > 0.8d)  return "I";            
            if (valor > 0.6d)  return "J";       
            if (valor > 0.4d)  return "K";    
            if (valor > 0.2d)  return "L";
            
            return "M";            
        }

        private string DarColumnaGlobales(float valor)
        {
            if (valor > 0.64d)  return "I";            
            if (valor > 0.36d)  return "J";            
            if (valor > 0.16d)  return "K";           
            if (valor > 0.04d)  return "L";
                            
            return "M";            
        }
    }
}
