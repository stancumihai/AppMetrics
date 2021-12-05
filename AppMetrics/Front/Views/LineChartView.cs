using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleApp1;
using ConsoleApp1.reporters;

// ReSharper disable All

namespace AppMetricsCSharp.Views
{
    public partial class LineChartView
    {
        private const double XAxisStart = 100;
        private const double YAxisStart = 100;
        private const double Interval = 50;
        private List<Holder> _holders;
        private List<Value> _values;
        private Polyline _chartPolyline;

        private Point _origin;
        private Line _xAxisLine, _yAxisLine;

        public void initGraph()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            _values = new();
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                _values.Add(new Value(i * 100, 100 * i));
                i++;
            }
        }

        public LineChartView()
        {
            ConsoleUtility.AllocConsole();
            InitializeComponent();
            TaskClass.TimerForAllTasks();
            initGraph();

            _holders = new List<Holder>();
            initGraph();
            Paint();
            ConsoleReporter consoleReporter = new ConsoleReporter();
            consoleReporter.Report();
            SizeChanged += (_, _) => Paint();
        }

        private void Paint()
        {
            try
            {
                if (ActualWidth > 0 && ActualHeight > 0)
                {
                    ChartCanvas.Children.Clear();
                    _holders.Clear();

                    // axis lines
                    _xAxisLine = new Line
                    {
                        X1 = XAxisStart,
                        Y1 = ActualHeight - YAxisStart,
                        X2 = ActualWidth - XAxisStart,
                        Y2 = ActualHeight - YAxisStart,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 1
                    };
                    _yAxisLine = new Line
                    {
                        X1 = XAxisStart,
                        Y1 = YAxisStart - 50,
                        X2 = XAxisStart,
                        Y2 = ActualHeight - YAxisStart,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 1
                    };

                    ChartCanvas.Children.Add(_xAxisLine);
                    ChartCanvas.Children.Add(_yAxisLine);

                    _origin = new Point(_xAxisLine.X1, _yAxisLine.Y2);

                    var xTextBlock0 = new TextBlock { Text = "{0}" };
                    ChartCanvas.Children.Add(xTextBlock0);
                    Canvas.SetLeft(xTextBlock0, _origin.X);
                    Canvas.SetTop(xTextBlock0, _origin.Y + 5);

                    // y axis lines
                    var xValue = XAxisStart;
                    var xPoint = _origin.X + Interval;
                    while (xPoint < _xAxisLine.X2)
                    {
                        var line = new Line
                        {
                            X1 = xPoint,
                            Y1 = YAxisStart - 50,
                            X2 = xPoint,
                            Y2 = ActualHeight - YAxisStart,
                            Stroke = Brushes.LightGray,
                            StrokeThickness = 10,
                            Opacity = 1
                        };

                        ChartCanvas.Children.Add(line);

                        var textBlock = new TextBlock { Text = $"{xValue}" };

                        ChartCanvas.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, xPoint - 12.5);
                        Canvas.SetTop(textBlock, line.Y2 + 5);

                        xPoint += Interval;
                        xValue += Interval;
                    }


                    var yTextBlock0 = new TextBlock { Text = "{0}" };
                    ChartCanvas.Children.Add(yTextBlock0);
                    Canvas.SetLeft(yTextBlock0, _origin.X - 20);
                    Canvas.SetTop(yTextBlock0, _origin.Y - 10);

                    // x axis lines
                    var yValue = YAxisStart;
                    var yPoint = _origin.Y - Interval;
                    while (yPoint > _yAxisLine.Y1)
                    {
                        var line = new Line
                        {
                            X1 = XAxisStart,
                            Y1 = yPoint,
                            X2 = ActualWidth - XAxisStart,
                            Y2 = yPoint,
                            Stroke = Brushes.LightGray,
                            StrokeThickness = 10,
                            Opacity = 1
                        };

                        ChartCanvas.Children.Add(line);

                        var textBlock = new TextBlock { Text = $"{yValue}" };
                        ChartCanvas.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, line.X1 - 30);
                        Canvas.SetTop(textBlock, yPoint - 10);

                        yPoint -= Interval;
                        yValue += Interval;
                    }

                    // connections
                    double x = 0, y = 0;
                    xPoint = _origin.X;
                    yPoint = _origin.Y;
                    while (xPoint < _xAxisLine.X2)
                    {
                        while (yPoint > _yAxisLine.Y1)
                        {
                            var holder = new Holder
                            {
                                X = x,
                                Y = y,
                                Point = new Point(xPoint, yPoint)
                            };

                            _holders.Add(holder);

                            yPoint -= Interval;
                            y += Interval;
                        }

                        xPoint += Interval;
                        yPoint = _origin.Y;
                        x += 100;
                        y = 0;
                    }

                    // polyline
                    _chartPolyline = new Polyline
                    {
                        Stroke = new SolidColorBrush(Color.FromRgb(68, 114, 196)),
                        StrokeThickness = 10
                    };
                    ChartCanvas.Children.Add(_chartPolyline);

                    // showing where are the connections points
                    foreach (var holder in _holders)
                    {
                        var oEllipse = new Ellipse
                        {
                            Fill = Brushes.Aqua,
                            Width = 10,
                            Height = 10,
                            Opacity = 0
                        };

                        ChartCanvas.Children.Add(oEllipse);
                        Canvas.SetLeft(oEllipse, holder.Point.X - 5);
                        Canvas.SetTop(oEllipse, holder.Point.Y - 5);
                    }

                    // add connection points to polyline
                    foreach (var value in _values)
                    {
                        var TOLERANCE = 0.01;
                        var holder = _holders.FirstOrDefault(h =>
                            Math.Abs(h.X - value.X) < TOLERANCE && Math.Abs(h.Y - value.Y) < TOLERANCE);
                        if (holder != null)
                            _chartPolyline.Points.Add(holder.Point);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }

    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public class Holder
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point Point { get; set; }
    }

    public class Value
    {
        public Value()
        {
        }

        public Value(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}