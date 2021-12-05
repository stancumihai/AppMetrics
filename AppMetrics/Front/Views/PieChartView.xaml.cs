using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleApp1;
using ConsoleApp1.reporters;
using static System.Windows.Media.ColorConverter;

// ReSharper disable All


namespace AppMetricsCSharp.Views
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public partial class PieChartView
    {
        public void initGraph()
        {
            SolidColorBrush[] brushes = new[]
            {
                new SolidColorBrush((Color)ConvertFromString("#4472C4")),
                new SolidColorBrush((Color)ConvertFromString("#ED7D31")),
                new SolidColorBrush((Color)ConvertFromString("#FFC000")),
                new SolidColorBrush((Color)ConvertFromString("#5B9BD5"))
            };
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int i = 0;
            Categories = new();
            foreach (String metricsRegistryKey in metricsRegistry.Keys)
            {
                Categories.Add(new Category(20, metricsRegistryKey, brushes[i]));
                i++;
            }
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

        public void Paint()
        {
            float pieWidth = 650,
                pieHeight = 650,
                centerX = pieWidth / 2,
                centerY = pieHeight / 2,
                radius = pieWidth / 2;
            MainCanvas.Width = pieWidth;
            MainCanvas.Height = pieHeight;

            DetailsItemsControl.ItemsSource = Categories;

            float angle = 0, prevAngle = 0;
            foreach (var category in Categories)
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

        private List<Category> Categories { get; set; }
    }

    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public class Category
    {
        public Category()
        {
        }

        public Category(float percentage, string title, Brush colorBrush)
        {
            Percentage = percentage;
            Title = title;
            ColorBrush = colorBrush;
        }

        public float Percentage { get; set; }
        public string Title { get; set; }
        public Brush ColorBrush { get; set; }
    }
}