using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public partial class ColumnChartView
    {
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
                double value = Convert.ToDouble(String.Format("{0:0.000}", timers[i].Store.Sum / 10e6));
                Items.Add(new Item(metricsRegistryKey, value));
                i++;
            }
        }

        public static void initAverage()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                double value = Convert.ToDouble(String.Format("{0:0.000}", timers[i].Store.GetMean() / 10e6));
                Items1.Add(new Item(metricsRegistryKey, value));
                i++;
            }
        }

        public static void initMin()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                double value = Convert.ToDouble(String.Format("{0:0.000}", timers[i].Store.Min / 10e6));
                Items2.Add(new Item(metricsRegistryKey, value));
                i++;
            }
        }

        public static void initMax()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                double value = Convert.ToDouble(String.Format("{0:0.000}", timers[i].Store.Max / 10e6));
                Items3.Add(new Item(metricsRegistryKey, value));
                i++;
            }
        }

        public static void initGraph()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            Items = new();
            Items1 = new();
            Items2 = new();
            Items3 = new();
            timers = new();

            initTimers();
            initSum();
            initAverage();
            initMin();
            initMax();
            toBeShown = Items;
        }

        public ColumnChartView()
        {
            ConsoleUtility.AllocConsole();
            InitializeComponent();
            TaskClass.TimerForAllTasks();
            initGraph();
            ConsoleReporter consoleReporter = new ConsoleReporter();
            consoleReporter.Report();
            Paint();
        }

        private static List<Timer> timers { get; set; }
        private static List<Item> Items { get; set; }
        private static List<Item> Items1 { get; set; }
        private static List<Item> Items2 { get; set; }
        private static List<Item> Items3 { get; set; }

        private static List<Item> toBeShown { get; set; }

        private void Paint()
        {
            try
            {
                float
                    chartWidth = 1366,
                    chartHeight = 800,
                    axisMargin = 100,
                    yAxisInterval = 25,
                    blockWidth = 50,
                    blockMargin = 35;

                MainCanvas.Width = chartWidth;
                MainCanvas.Height = chartHeight;

                var yAxisEndPoint = new Point(axisMargin, axisMargin);
                var origin = new Point(axisMargin, chartHeight - axisMargin);
                var xAxisEndPoint = new Point(chartWidth - axisMargin, chartHeight - axisMargin);

                var yAxisEndPointEllipse = new Ellipse
                {
                    Fill = Brushes.Aqua,
                    Width = 10,
                    Height = 10
                };
                MainCanvas.Children.Add(yAxisEndPointEllipse);
                Canvas.SetLeft(yAxisEndPointEllipse, yAxisEndPoint.X - 5);
                Canvas.SetTop(yAxisEndPointEllipse, yAxisEndPoint.Y - 5);

                var originEllipse = new Ellipse
                {
                    Fill = Brushes.Aqua,
                    Width = 10,
                    Height = 10
                };
                MainCanvas.Children.Add(originEllipse);
                Canvas.SetLeft(originEllipse, origin.X - 5);
                Canvas.SetTop(originEllipse, origin.Y - 5);

                var xAxisEndPointEllipse = new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = 10,
                    Height = 10
                };
                MainCanvas.Children.Add(xAxisEndPointEllipse);
                Canvas.SetLeft(xAxisEndPointEllipse, xAxisEndPoint.X - 5);
                Canvas.SetTop(xAxisEndPointEllipse, xAxisEndPoint.Y - 5);

                var yAxisStartLine = new Line
                {
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1,
                    X1 = yAxisEndPoint.X,
                    Y1 = yAxisEndPoint.Y,
                    X2 = origin.X,
                    Y2 = origin.Y
                };
                MainCanvas.Children.Add(yAxisStartLine);

                var yAxisEndLine = new Line
                {
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1,
                    X1 = xAxisEndPoint.X,
                    Y1 = xAxisEndPoint.Y,
                    X2 = xAxisEndPoint.X,
                    Y2 = yAxisEndPoint.Y
                };
                MainCanvas.Children.Add(yAxisEndLine);


                double yValue = 0;
                var yAxisValue = origin.Y;
                while (yAxisValue >= yAxisEndPoint.Y)
                {
                    var lEllipse = new Ellipse
                    {
                        Fill = Brushes.Aqua,
                        Width = 10,
                        Height = 10
                    };

                    var rEllipse = new Ellipse
                    {
                        Fill = Brushes.Blue,
                        Width = 10,
                        Height = 10
                    };

                    MainCanvas.Children.Add(lEllipse);
                    MainCanvas.Children.Add(rEllipse);

                    Canvas.SetLeft(lEllipse, origin.X - 5);
                    Canvas.SetTop(lEllipse, yAxisValue - 5);

                    Canvas.SetLeft(rEllipse, xAxisEndPoint.X - 5);
                    Canvas.SetTop(rEllipse, yAxisValue - 5);
                    var yLine = new Line
                    {
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 1,
                        X1 = origin.X,
                        Y1 = yAxisValue,
                        X2 = xAxisEndPoint.X,
                        Y2 = yAxisValue
                    };
                    MainCanvas.Children.Add(yLine);

                    var yAxisTextBlock = new TextBlock
                    {
                        Text = $"{yValue / 100}s",
                        Foreground = Brushes.Black,
                        FontSize = 16
                    };
                    MainCanvas.Children.Add(yAxisTextBlock);

                    Canvas.SetLeft(yAxisTextBlock, origin.X - 50);
                    Canvas.SetTop(yAxisTextBlock, yAxisValue - 12.5);


                    yAxisValue -= yAxisInterval;
                    yValue += yAxisInterval;
                }


                var margin = origin.X + blockMargin;
                foreach (var item in toBeShown)
                {
                    var block = new Rectangle
                    {
                        Fill = Brushes.Navy,
                        Width = blockWidth,
                        Height = item.Value
                    };

                    MainCanvas.Children.Add(block);
                    Canvas.SetLeft(block, margin);
                    Canvas.SetTop(block, origin.Y - block.Height);

                    var blockHeader = new TextBlock
                    {
                        Text = item.Header,
                        FontSize = 16,
                        Foreground = Brushes.Black
                    };
                    MainCanvas.Children.Add(blockHeader);
                    Canvas.SetLeft(blockHeader, margin + 10);
                    Canvas.SetTop(blockHeader, origin.Y + 5);

                    margin += blockWidth + blockMargin;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        private void changeUi(object sender, SelectionChangedEventArgs e)
        {
            string text = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (text == "Time")
            {
                toBeShown = Items;
                MainCanvas.Children.RemoveRange(1, MainCanvas.Children.Capacity - 1);
                Paint();
            }
            else if (text == "Average")
            {
                toBeShown = Items1;
                MainCanvas.Children.RemoveRange(1, MainCanvas.Children.Capacity - 1);
                Paint();
            }
            else if (text == "Min")
            {
                toBeShown = Items2;
                MainCanvas.Children.RemoveRange(1, MainCanvas.Children.Capacity - 1);
                Paint();
            }
            else if (text == "Max")
            {
                toBeShown = Items3;
                MainCanvas.Children.RemoveRange(1, MainCanvas.Children.Capacity - 1);
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
                    toBeShown = Items;
                    Paint();
                }
            }
        }
    }

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public class Item
    {
        public Item(string header, double value)
        {
            this.Header = header;
            this.Value = value;
        }

        public string Header { get; set; }
        public double Value { get; set; }
    }
}