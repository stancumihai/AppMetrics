using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleApp1;
using ConsoleApp1.reporters;

// ReSharper disable All

namespace AppMetricsCSharp.Views
{
    public partial class ColumnChartView
    {

        public static void initGraph()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            Items = new();
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Items.Add(new Item(metricsRegistryKey, 100));
            }
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

        private static List<Item> Items { get; set; }

        private void Paint()
        {
            try
            {
                float
                    chartWidth = 1200,
                    chartHeight = 700,
                    axisMargin = 100,
                    yAxisInterval = 50,
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
                        Text = $"{yValue}",
                        Foreground = Brushes.Black,
                        FontSize = 16
                    };
                    MainCanvas.Children.Add(yAxisTextBlock);

                    Canvas.SetLeft(yAxisTextBlock, origin.X - 35);
                    Canvas.SetTop(yAxisTextBlock, yAxisValue - 12.5);


                    yAxisValue -= yAxisInterval;
                    yValue += yAxisInterval;
                }


                var margin = origin.X + blockMargin;
                foreach (var item in Items)
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