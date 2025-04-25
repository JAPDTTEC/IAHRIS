using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.WindowsForms;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;
using System.Drawing;
using OfficeOpenXml;
using System.Windows.Forms;
using TickStyle = OxyPlot.Axes.TickStyle;
using HorizontalAlignment = OxyPlot.HorizontalAlignment;

namespace IAHRIS
    {
    /// <summary>
    /// Clase encargada de dibujar los gráficos de Caudales Ecológicos.
    /// </summary>
    internal class DibujoGraficos
    {
        /// <summary>
        /// Carga una serie de puntos y de lineas y las dibuja en un gráfico
        /// </summary>
        /// <param name="plot1">El gráfico</param>
        /// <param name="puntos">List de Escenarios de los puntos del gráfico</param>
        /// <param name="lineas">List de Escenarios de las lineas de puntuación del gráfico</param>

        public static void GraficoValoracionesEscenario(PlotView plot1, List<EscenarioDTO> puntos, List<EscenarioDTO> lineas)
        {

            var plotModel = new PlotModel { };

            ScatterSeries scatterSeries;

            //Dibujado de lineas de puntuación

            for (int i = 0; i < lineas.Count; i++)
            {
                var lineSeries = new LineSeries();
                lineSeries.Points.Add(new DataPoint(-1000, lineas[i].Demanda_Ambiental));
                lineSeries.Points.Add(new DataPoint(1000, lineas[i].Demanda_Ambiental));
                lineSeries.TrackerFormatString = "Valor: " + lineas[i].Demanda_Ambiental + "\n" + lineas[i].Nombre;
                AddPointLabel(plotModel, new ScatterPoint(-100, lineas[i].Demanda_Ambiental), lineas[i].Nombre);
                plotModel.Series.Add(lineSeries);

            }

            //Dibujado de puntos

            for (int i = 0; i < puntos.Count; i++)
            {
                if (puntos[i].Por_Defecto == true)
                {
                    scatterSeries = new ScatterSeries
                    { MarkerType = MarkerType.Square, MarkerFill = OxyColor.FromRgb(255, 0, 255), MarkerStroke = OxyColor.FromRgb(0, 0, 0) };
                }

                else if (puntos[i].Por_Defecto == false)
                {
                    if ((puntos[i].Puntuacion.Puntuacion_Estacionalidad.Puntuacion_Parcial < 0 || puntos[i].Puntuacion.Puntuacion_Magnitud.Puntuacion_Parcial < 0) || puntos[i].Puntuacion.Puntuacion_Variabilidad.Puntuacion_Parcial < 0)
                    {
                        scatterSeries = new ScatterSeries
                        { MarkerType = MarkerType.Square, MarkerFill = OxyColor.FromRgb(255, 204, 0), MarkerStroke = OxyColor.FromRgb(255, 0, 0) };
                    }
                    else
                    {
                        scatterSeries = new ScatterSeries
                        { MarkerType = MarkerType.Square, MarkerFill = OxyColor.FromRgb(255, 204, 0), MarkerStroke = OxyColor.FromRgb(0, 0, 0) };
                    }
                }

                else
                {
                    scatterSeries = new ScatterSeries
                    { MarkerType = MarkerType.Square, MarkerFill = OxyColor.FromRgb(139, 0, 0), MarkerStroke = OxyColor.FromRgb(0, 0, 0) };
                }

                ScatterPoint scatterPoint = new ScatterPoint(puntos[i].Puntuacion.Puntuacion_Total, puntos[i].Demanda_Ambiental);
                scatterPoint.Size = 3;
                scatterSeries.Points.Add(scatterPoint);
                AddPointLabel(plotModel, new ScatterPoint(puntos[i].Puntuacion.Puntuacion_Total + 2.5, puntos[i].Demanda_Ambiental - 0.015), puntos[i].Nombre);
                scatterSeries.TrackerFormatString = "Puntuación: " + puntos[i].Puntuacion.Puntuacion_Total +
                    "\nDemanda Ambiental: " + puntos[i].Demanda_Ambiental + "\n" + puntos[i].Nombre;

                plotModel.Series.Add(scatterSeries);
            }



            //Dibujado de ejes del gráfico.

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                AxisTickToLabelDistance = 9,
                TickStyle = TickStyle.Crossing,
                LabelFormatter = x => null,
                MajorStep = 0.1,
                IsPanEnabled = false,
                IsZoomEnabled = false

            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1,
                Title = "Demanda Ambiental",
                AxisTickToLabelDistance = 9,
                MajorStep = 0.1,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -100,
                Maximum = 100,
                Title = "Puntuación",
                MajorStep = 10,
                MinorStep = 5,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });

            plot1.Model = plotModel;
        }

        /// <summary>
        ///Carga una etiquta en el modelo de un gráfico.
        /// </summary>
        /// <param name="plotModel">El modelo del gráfico</param>
        /// <param name="point">Las coordenadas de la etiqueta</param>
        /// <param name="label">El texto de la etiqueta</param>
        private static void AddPointLabel(PlotModel plotModel, ScatterPoint point, string label)
        {
            var annotation = new TextAnnotation
            {
                Text = label,
                TextPosition = new DataPoint(point.X, point.Y), // Posición del punto
                TextHorizontalAlignment = HorizontalAlignment.Left,
                //TextVerticalAlignment = VerticalAlignment.Bottom,          
                FontSize = 8,
                Stroke = OxyColor.FromArgb(0, 0, 0, 0)

            };

            plotModel.Annotations.Add(annotation);
        }
        public static void ResetZoomGrafico(PlotView plot1)
        {
            plot1.Model.Axes.Clear();


            plot1.Model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                AxisTickToLabelDistance = 9,
                TickStyle = TickStyle.Crossing,
                LabelFormatter = x => null,
                MajorStep = 0.1,
                IsPanEnabled = false,
                IsZoomEnabled = false

            });
            plot1.Model.Axes.Add(new LinearAxis
            {      
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1,
                Title = "Demanda Ambiental",
                AxisTickToLabelDistance = 9,
                MajorStep = 0.1,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });
            plot1.Model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -100,
                Maximum = 100,
                Title = "Puntuación",
                MajorStep = 10,
                MinorStep = 5,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });

            plot1.InvalidatePlot(true);

        }
        
     

        

        public static void ZoomInX(PlotView plotView)
        {
            
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var xAxis = plotModel.Axes[2] as LinearAxis;
            if (xAxis == null)
                return;

            xAxis.IsZoomEnabled = true;

            double range = xAxis.ActualMaximum - xAxis.ActualMinimum;
            double zoomAmount = range * 0.1;

            xAxis.Zoom(xAxis.ActualMinimum + zoomAmount, xAxis.ActualMaximum - zoomAmount);

            plotView.InvalidatePlot(true);

            xAxis.IsZoomEnabled = false;
        }

        public static void ZoomOutX(PlotView plotView)
        {
            
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var xAxis = plotModel.Axes[2] as LinearAxis;
            if (xAxis == null)
                return;
            xAxis.IsZoomEnabled = true;
            double range = xAxis.ActualMaximum - xAxis.ActualMinimum;
            double zoomAmount = range * 0.1;

            xAxis.Zoom(xAxis.ActualMinimum - zoomAmount, xAxis.ActualMaximum + zoomAmount);

            plotView.InvalidatePlot(true);
            xAxis.IsZoomEnabled = false;
        }

        public static void ZoomInY(PlotView plotView)
        {
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var yAxis = plotModel.Axes[0] as LinearAxis;
            if (yAxis == null)
                return;
            var yAxis2 = plotModel.Axes[1] as LinearAxis;
            if (yAxis2 == null)
                return;

            yAxis.IsZoomEnabled = true;
            yAxis2.IsZoomEnabled = true;

            double range = yAxis.ActualMaximum - yAxis.ActualMinimum;
            double zoomAmount = range * 0.1;

            yAxis.Zoom(yAxis.ActualMinimum + zoomAmount, yAxis.ActualMaximum - zoomAmount);
            yAxis2.Zoom(yAxis2.ActualMinimum + zoomAmount, yAxis2.ActualMaximum - zoomAmount);

            plotView.InvalidatePlot(true);
            yAxis.IsZoomEnabled = false;
            yAxis2.IsZoomEnabled = false;
        }

        public static void ZoomOutY(PlotView plotView)
        {
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var yAxis = plotModel.Axes[0] as LinearAxis;
            if (yAxis == null)
                return;
            var yAxis2 = plotModel.Axes[1] as LinearAxis;
            if (yAxis2 == null)
                return;

            yAxis.IsZoomEnabled = true;
            yAxis2.IsZoomEnabled = true;

            double range = yAxis.ActualMaximum - yAxis.ActualMinimum;
            double zoomAmount = range * 0.1;

            yAxis.Zoom(yAxis.ActualMinimum - zoomAmount, yAxis.ActualMaximum + zoomAmount);
            yAxis2.Zoom(yAxis.ActualMinimum - zoomAmount, yAxis2.ActualMaximum + zoomAmount);

            plotView.InvalidatePlot(true);
            yAxis.IsZoomEnabled = false;
            yAxis2.IsZoomEnabled = false;
        }

        public static void MoveViewUp(PlotView plotView)
        {
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var yAxis = plotModel.Axes[0] as LinearAxis;
            if (yAxis == null)
                return;
            var yAxis2 = plotModel.Axes[1] as LinearAxis;
            if (yAxis2 == null)
                return;
            yAxis.IsPanEnabled = true;
            yAxis2.IsPanEnabled = true;

            double moveAmount = 10;

            yAxis.Pan(moveAmount);
            yAxis2.Pan(moveAmount);
            plotView.InvalidatePlot(true);
            yAxis.IsPanEnabled = false;
            yAxis2.IsPanEnabled = false;
        }

        public static void MoveViewDown(PlotView plotView)
        {
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var yAxis = plotModel.Axes[0] as LinearAxis;
            if (yAxis == null)
                return;
            var yAxis2 = plotModel.Axes[1] as LinearAxis;
            if (yAxis2 == null)
                return;
            yAxis.IsPanEnabled = true;
            yAxis2.IsPanEnabled = true;
            double moveAmount = 10;

            yAxis.Pan(-moveAmount);
            yAxis2.Pan(-moveAmount);

            plotView.InvalidatePlot(true);
            yAxis.IsPanEnabled = false;
            yAxis2.IsPanEnabled = false;
        }

        public static void MoveViewRight(PlotView plotView)
        {
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var xAxis = plotModel.Axes[2] as LinearAxis;
            if (xAxis == null)
                return;
            xAxis.IsPanEnabled = true;
            double moveAmount = 10;

            xAxis.Pan(-moveAmount);

            plotView.InvalidatePlot(true);
            xAxis.IsPanEnabled = false;
        }

        public static void MoveViewLeft(PlotView plotView)
        {
            var plotModel = plotView.Model;
            if (plotModel == null)
                return;

            var xAxis = plotModel.Axes[2] as LinearAxis;
            if (xAxis == null)
                return;
            xAxis.IsPanEnabled = true;
            double moveAmount = 10;

            xAxis.Pan(moveAmount);

            plotView.InvalidatePlot(true);
            xAxis.IsPanEnabled = false;
        }

        
    }
}
