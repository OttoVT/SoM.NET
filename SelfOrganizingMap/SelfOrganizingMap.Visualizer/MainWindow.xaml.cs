using SelfOrganizingMap.Maps;
using SelfOrganizingMap.Math;
using SelfOrganizingMap.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SelfOrganizingMap.Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KohonenMap _map;
        static Random _random = new Random();
        public MainWindow()
        {
            InitializeComponent();
            DrawMap();

            this.SizeChanged += (sender, args) =>
            {
                DrawMap();
            };
        }

        private void DrawMap()
        {
            var count = 2;
            var neuronLayer = new Neuron[count, count];
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    var vector = CreateRandom(2);
                    neuronLayer[i, j] = new Neuron(vector);
                };
            }

            _map = new KohonenMap(neuronLayer);

            KohonenMapConstants constants = new KohonenMapConstants(2, 0.1, 100, 100);
            KohonenMapTraining training = new KohonenMapTraining(_map, constants, 0.000001, true);
            List<Vector<double>> patterns = GetPatterns();

            grid.Width = this.Width;
            grid.Height = this.Height;
            grid.Children.Clear();
            training.FullTrain(patterns, 100);
            var length = _map.Map.GetLength(0);
            var xCenter = grid.Width / 2;
            var yCenter = grid.Height / 2;
            var clustersPosition = new List<Tuple<System.Windows.Point, Ellipse>>();
            var polyline = new Polyline();
            polyline.Stroke = Brushes.Blue;


            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var point = _map.Map[i, j];
                    Debug.WriteLine($"{point.Weights[0]}-{point.Weights[1]}");
                    var coordinate = new System.Windows.Point(xCenter + point.Weights[0] * 10, yCenter - point.Weights[1] * 10);
                    var ellipse = CreatEllipse(Colors.Blue);
                    clustersPosition.Add(Tuple.Create(coordinate, ellipse));
                    polyline.Points.Add(new System.Windows.Point(coordinate.X + ellipse.Width / 2.0, coordinate.Y + ellipse.Height / 2.0));
                }
            }

            polyline.Points.Add(new System.Windows.Point(clustersPosition[0].Item1.X + clustersPosition[0].Item2.Width / 2.0, 
                clustersPosition[0].Item1.Y + clustersPosition[0].Item2.Height / 2.0));

            var xAxis = new Line();
            xAxis.Stroke = System.Windows.Media.Brushes.Black;
            xAxis.X1 = 0;
            xAxis.X2 = grid.Width;
            xAxis.Y1 = grid.Height / 2;
            xAxis.Y2 = grid.Height / 2;
            xAxis.HorizontalAlignment = HorizontalAlignment.Left;
            xAxis.VerticalAlignment = VerticalAlignment.Center;
            xAxis.StrokeThickness = 1;

            var yAxis = new Line();
            yAxis.Stroke = System.Windows.Media.Brushes.Black;
            yAxis.X1 = grid.Width / 2;
            yAxis.X2 = grid.Width / 2;
            yAxis.Y1 = 0;
            yAxis.Y2 = grid.Height;
            yAxis.HorizontalAlignment = HorizontalAlignment.Left;
            yAxis.VerticalAlignment = VerticalAlignment.Center;
            yAxis.StrokeThickness = 1;

            grid.Children.Add(xAxis);
            grid.Children.Add(yAxis);
            grid.Children.Add(polyline);

            for (int i = 0; i < clustersPosition.Count; i++)
            {
                var coordinate = clustersPosition[i].Item1;
                var ellipse = clustersPosition[i].Item2;
                grid.Children.Add(ellipse);
                Canvas.SetTop(ellipse, coordinate.Y);
                Canvas.SetLeft(ellipse, coordinate.X);
            }

            for (int i = 0; patterns.Count > i; i++)
            {
                var coordinate = new System.Windows.Point(xCenter + patterns[i][0] * 10, yCenter - patterns[i][1] * 10);
                var ellipse = CreatEllipse(Colors.Black);
                grid.Children.Add(ellipse);
                Canvas.SetTop(ellipse, coordinate.Y);
                Canvas.SetLeft(ellipse, coordinate.X);
            }
        }

        private static List<Vector<double>> GetPatterns()
        {
            return new List<Vector<double>>()
            {
                new Vector<double>(new double[] { 1, 1}),
                new Vector<double>(new double[] { 1, 2}),
                new Vector<double>(new double[] { 2, 1}),
                new Vector<double>(new double[] { 4, 1}),
                new Vector<double>(new double[] { 5, 1}),
                new Vector<double>(new double[] { 5, 2}),
                new Vector<double>(new double[] { 4, 5}),
                new Vector<double>(new double[] { 5, 5}),
                new Vector<double>(new double[] { 5, 4}),
                new Vector<double>(new double[] { 1, 5}),
                new Vector<double>(new double[] { 1, 4}),
                new Vector<double>(new double[] { 2, 5}),
             };
        }

        private Vector<double> CreateRandom(int count)
        {
            var array = new double[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = count * _random.NextDouble();
            }

            return new Vector<double>(array);
        }

        private Ellipse CreatEllipse(Color color)
        {
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = color;
            myEllipse.Stroke = mySolidColorBrush;
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.Width = 6;
            myEllipse.Height = 6;

            return myEllipse;
        }
    }
}
