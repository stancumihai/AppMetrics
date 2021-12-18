using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleApp1;
using ConsoleApp1.meters;
using ConsoleApp1.reporters;

// ReSharper disable All

namespace AppMetricsCSharp.Views
{
    public partial class LineChartView
    {
        private const double XAxisStart = 100;
        private const double YAxisStart = 100;
        private const double Interval = 100;
        private List<Holder> _holders;

        private Polyline _chartPolyline;

        private Point _origin;
        private Line _xAxisLine, _yAxisLine;

        private static List<Value> _values { get; set; }
        private static List<Value> _values1 { get; set; }
        private static List<Value> _values2 { get; set; }
        private static List<Value> _values3 { get; set; }
        private static List<Value> toBeShown { get; set; }
        private static List<Timer> timers { get; set; }

        public static void initTimers()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                timers.Add(metricsRegistry.Timering(metricsRegistryKey));
            }
        }

        public static void initSum()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                double valueToEnter = timers[i].Store.Sum / 10e6f;
                _values.Add(new Value(i * 100, 100 * i));
                i++;
            }
        }

        public static void initAverage()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                double valueToEnter = timers[i].Store.GetMean() / 10e6f;
                _values1.Add(new Value(i * 100, Math.Round(valueToEnter)));
                i++;
            }
        }

        public static void initMin()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                double valueToEnter = timers[i].Store.Min / 10e6f;
                _values2.Add(new Value(i * 100, Math.Round(valueToEnter)));
                i++;
            }
        }

        public static void initMax()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                double valueToEnter = timers[i].Store.Max / 10e6f;
                _values3.Add(new Value(i * 100, Math.Round(valueToEnter)));
                i++;
            }
        }


        public void initGraph()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            _values = new();
            _values1 = new();
            _values2 = new();
            _values3 = new();
            timers = new();
            initTimers();
            initSum();
            initAverage();
            initMin();
            initMax();
            toBeShown = _values;
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
                    foreach (var value in toBeShown)
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

        private void changeUi(object sender, SelectionChangedEventArgs e)
        {
            string text = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (text == "Time")
            {
                toBeShown = _values;
                Paint();
            }
            else if (text == "Average")
            {
                toBeShown = _values1;
                Paint();
            }
            else if (text == "Min")
            {
                toBeShown = _values2;
                Paint();
            }
            else if (text == "Max")
            {
                toBeShown = _values3;
                Paint();
            }
            else
            {
                if (toBeShown == null)
                {
                    initSum();
                }
                else
                {
                    toBeShown = _values;
                    Paint();
                }
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