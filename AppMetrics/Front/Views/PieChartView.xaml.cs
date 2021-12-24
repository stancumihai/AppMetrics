using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleApp1;
using ConsoleApp1.meters;
using ConsoleApp1.reporters;
using static System.Windows.Media.ColorConverter;

// ReSharper disable All


namespace AppMetricsCSharp.Views
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public partial class PieChartView
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
            double sum = 0;

            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                sum += timer.Store.Sum / Constants.SECONDS;
                i++;
            }

            i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                double valueToEnter =
                    Convert.ToDouble(String.Format("{0:0.000}", timer.Store.Sum * 100 / (Constants.SECONDS * sum)));
                Categories.Add(new Category(valueToEnter, metricsRegistryKey, brushes[i]));
                i++;
            }
        }

        public static void initAverage()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;

            int i = 0;
            double sum = 0;

            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                sum += timer.Store.GetMean() / Constants.SECONDS;
                i++;
            }

            i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                double valueToEnter =
                    Convert.ToDouble(
                        String.Format("{0:0.000}", timer.Store.GetMean() * 100 / (Constants.SECONDS * sum)));
                Categories1.Add(new Category(valueToEnter, metricsRegistryKey, brushes[i]));
                i++;
            }
        }

        public static void initMin()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;

            int i = 0;
            double sum = 0;

            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                sum += timer.Store.Min / Constants.SECONDS;
                i++;
            }

            i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                double valueToEnter =
                    Convert.ToDouble(String.Format("{0:0.000}", timer.Store.Min * 100 / (Constants.SECONDS * sum)));
                Categories2.Add(new Category(valueToEnter, metricsRegistryKey, brushes[i]));
                i++;
            }
        }

        public static void initMax()
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;

            int i = 0;
            double sum = 0;

            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                sum += timer.Store.Max / Constants.SECONDS;
                i++;
            }

            i = 0;
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Timer timer = metricsRegistry.Timering(metricsRegistryKey);
                double valueToEnter =
                    Convert.ToDouble(String.Format("{0:0.000}", timer.Store.Max * 100 / (Constants.SECONDS * sum)));
                Categories3.Add(new Category(valueToEnter, metricsRegistryKey, brushes[i]));
                i++;
            }
        }

        public void initGraph()
        {
            Categories = new();
            Categories1 = new();
            Categories2 = new();
            Categories3 = new();
            timers = new();
            initTimers();
            initSum();
            initAverage();
            initMin();
            initMax();
            toBeShown = Categories;
        }

        public PieChartView()
        {
            ConsoleUtility.AllocConsole();
            InitializeComponent();
            TaskClass.TimerForAllTasks();
            initGraph();
            ConsoleReporter consoleReporter = new ConsoleReporter();
            consoleReporter.Report();
            Paint();
        }

        private static List<Category> Categories { get; set; }
        private static List<Category> Categories1 { get; set; }
        private static List<Category> Categories2 { get; set; }
        private static List<Category> Categories3 { get; set; }
        private static List<Timer> timers { get; set; }

        private static List<Category> toBeShown { get; set; }

        private static SolidColorBrush[] brushes = new[]
        {
            new SolidColorBrush((Color)ConvertFromString("#4472C4")),
            new SolidColorBrush((Color)ConvertFromString("#ED7D31")),
            new SolidColorBrush((Color)ConvertFromString("#FFC000")),
            new SolidColorBrush((Color)ConvertFromString("#5B9BD5")),
            new SolidColorBrush((Color)ConvertFromString("#FFC000")),
            new SolidColorBrush((Color)ConvertFromString("#ED7D31"))
        };


        public void Paint()
        {
            float pieWidth = 650,
                pieHeight = 650,
                centerX = pieWidth / 2,
                centerY = pieHeight / 2,
                radius = pieWidth / 2;
            MainCanvas.Width = pieWidth;
            MainCanvas.Height = pieHeight;

            DetailsItemsControl.ItemsSource = toBeShown;

            double angle = 0, prevAngle = 0;
            foreach (var category in toBeShown)
            {
                var line1X = radius * Math.Cos(angle * Math.PI / 180) + centerX;
                var line1Y = radius * Math.Sin(angle * Math.PI / 180) + centerY;

                angle = category.Percentage * 360 / 100 + prevAngle;
                Debug.WriteLine(angle);

                var arcX = radius * Math.Cos(angle * Math.PI / 180) + centerX;
                var arcY = radius * Math.Sin(angle * Math.PI / 180) + centerY;

                var line1Segment = new LineSegment(new Point(line1X, line1Y), false);
                double arcWidth = radius, arcHeight = radius;
                var isLargeArc = category.Percentage > 50;
                var arcSegment = new ArcSegment
                {
                    Size = new Size(arcWidth, arcHeight),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = isLargeArc
                };
                var line2Segment = new LineSegment(new Point(centerX, centerY), false);

                var pathFigure = new PathFigure(
                    new Point(centerX, centerY),
                    new List<PathSegment>
                    {
                        line1Segment,
                        arcSegment,
                        line2Segment
                    },
                    true);

                var pathFigures = new List<PathFigure> { pathFigure };
                var pathGeometry = new PathGeometry(pathFigures);
                var path = new Path
                {
                    Fill = category.ColorBrush,
                    Data = pathGeometry
                };
                MainCanvas.Children.Add(path);

                prevAngle = angle;

                // draw outlines
                var outline1 = new Line
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = line1Segment.Point.X,
                    Y2 = line1Segment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5
                };
                var outline2 = new Line
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = arcSegment.Point.X,
                    Y2 = arcSegment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5
                };

                MainCanvas.Children.Add(outline1);
                MainCanvas.Children.Add(outline2);
            }
        }


        private void changeUi(object sender, SelectionChangedEventArgs e)
        {
            string text = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (text == "Time")
            {
                toBeShown = Categories;
                Paint();
            }
            else if (text == "Average")
            {
                toBeShown = Categories1;
                Paint();
            }
            else if (text == "Min")
            {
                toBeShown = Categories2;
                Paint();
            }
            else if (text == "Max")
            {
                toBeShown = Categories3;
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
                    toBeShown = Categories;
                    Paint();
                }
            }
        }
    }
}

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class Category
{
    public Category()
    {
    }

    public Category(double percentage, string title, Brush colorBrush)
    {
        Percentage = percentage;
        Title = title;
        ColorBrush = colorBrush;
    }

    public double Percentage { get; set; }
    public string Title { get; set; }
    public Brush ColorBrush { get; set; }
}