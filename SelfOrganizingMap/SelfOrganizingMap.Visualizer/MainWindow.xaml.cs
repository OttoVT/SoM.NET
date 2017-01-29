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
        static new Random _random = new Random();
        public MainWindow()
        {
            InitializeComponent();

            KohonenMap _map;

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

            var patterns = new List<Vector<double>>()
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

            //training.TrainOnSetsNTimes(patterns, 100);
            training.FullTrain(patterns, 100);
            var length = _map.Map.GetLength(0);
            var xCenter = grid.Width / 2;
            var yCenter = grid.Height / 2;
            var polyline = new Polyline();
            polyline.Stroke = Brushes.Black;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var point = _map.Map[i, j];
                    Debug.WriteLine($"{point.Weights[0]}-{point.Weights[1]}");
                    polyline.Points.Add(
                        new System.Windows.Point(xCenter + point.Weights[0] * 10, yCenter - point.Weights[1] * 10));
                }
            }

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

            var poly = new Polyline();
            poly.Stroke = Brushes.Black;
            var ind = 0;
            for (int i = 0; ; i++)
            {
                if (ind == 3)
                {
                    ind = 0;
                    grid.Children.Add(poly);
                    poly = new Polyline();
                    poly.Stroke = Brushes.Black;

                    if (i >= patterns.Count)
                    {
                        break;
                    }
                }

                var pat = patterns[i];

                poly.Points.Add(new System.Windows.Point(xCenter + pat[0] * 10, yCenter - pat[1] * 10));
                ind++;
            }
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
    }
}
