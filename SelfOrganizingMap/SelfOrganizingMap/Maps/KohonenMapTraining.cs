using SelfOrganizingMap.Math;
using SelfOrganizingMap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Maps
{
    public class KohonenMapTraining
    {
        private KohonenMap _map;
        private int _mapSize;
        private readonly KohonenMapConstants _constants;
        private double _maxErrorToStop;
        private double _currentError = 100;
        private bool _stopOnEpoch;

        public KohonenMapTraining(KohonenMap map, KohonenMapConstants constants, double maxErrorToStop,
            bool stopOnEpoch)
            : this(map, constants)
        {
            _maxErrorToStop = maxErrorToStop;
            _currentError = maxErrorToStop * 100;
            _stopOnEpoch = stopOnEpoch;
        }

        public KohonenMapTraining(KohonenMap map, KohonenMapConstants constants)
        {
            _map = map;
            _mapSize = _map.Map.GetLength(0);
            _constants = constants;
        }


        public double TrainOnce(Vector<double> vector, int iteration)
        {
            double error = 0;
            Neuron<double>[,] outputs = _map.Map;
            Point winnerCoordinate = _map.ConcurrencyFunction(vector);
            double neighborhood = DecayNeighborhood(iteration);

            for (int i = 0; i < _mapSize; i++)
                for (int j = 0; j < _mapSize; j++)
                {
                    var topologicalDistance = System.Math.Sqrt(System.Math.Pow((winnerCoordinate.X - i), 2) +
                        System.Math.Pow((winnerCoordinate.Y - j), 2));
                    if (topologicalDistance < neighborhood)
                    {
                        var neuron = outputs[i, j];
                        Point point = new Point(i, j);

                        error += ModifyWeights(neuron, vector, winnerCoordinate, point, iteration);
                    }
                }
            error = System.Math.Abs(error / (_mapSize * _mapSize));

            return error;
        }

        public void FullTrain(List<Vector<double>> patterns, int iterationsCount)
        {
            double currentError = double.MaxValue;
            int ieration = 0;

            while (true)
            {
                List<Vector<double>> TrainingSet = new List<Vector<double>>(patterns);

                currentError = 0;

                for (int i = 0; i < patterns.Count; i++)
                {
                    Vector<double> pattern = TrainingSet[new Random(DateTime.Now.Millisecond).Next(patterns.Count - i)];

                    currentError = TrainOnce(pattern, ieration);
                    TrainingSet.Remove(pattern);
                }

                ieration++;

                if (_stopOnEpoch && ieration >= iterationsCount)
                    break;
                else if (!_stopOnEpoch && currentError <= _maxErrorToStop)
                    break;
            }
        }

        #region Private
        private double ModifyWeights(Neuron<double> neuron,
            Vector<double> vector,
            Point winnerCoordinate,
            Point neuronCoordinate,
            int iteration)
        {
            var weights = neuron.Weights;
            double avgDelta = 0;
            double modificationValue = 0;
            double learningRate = DecayLearningRate(iteration);
            double influence = DecayInfluence(winnerCoordinate, neuronCoordinate, iteration);

            for (int i = 0; i < vector.Size; i++)
            {
                modificationValue = learningRate *
                    influence *
                    (vector[i] - weights[i]);
                weights.Object[i] += modificationValue;
                avgDelta += modificationValue;
            }

            avgDelta = avgDelta / vector.Size;

            return avgDelta;
        }

        private double DecayNeighborhood(int iteration)
        {
            var power = -iteration / _constants.NeighborhoodDecay;
            //var calulatedNeighborhood = _constants.StartNeighborhood * System.Math.Exp(power);
            var calulatedNeighborhood = _mapSize * System.Math.Exp(power);
            return calulatedNeighborhood;
        }

        private double DecayLearningRate(int iteration)
        {
            var power = -iteration / _constants.LearningRateDecay;
            var calulatedLearningRate = _constants.StartLearningRate * System.Math.Exp(power);
            return calulatedLearningRate;
        }

        private double DecayInfluence(Point winnerCoordinate, Point neuron, int iteration)
        {
            double result = 0;
            double distance = 0;
            double neighborhood = DecayNeighborhood(iteration);

            distance = System.Math.Sqrt(System.Math.Pow((winnerCoordinate.X - neuron.X), 2)
                    + System.Math.Pow((winnerCoordinate.Y - neuron.Y), 2));
            result = System.Math.Exp(-(distance * distance) / (System.Math.Pow(neighborhood, 2)));

            return result;

        }

        private Dictionary<Point, Neuron<double>> GetNeighborhood(int neighborhood, Point bmuCoordinats)
        {
            int mapSize = _map.Map.GetLength(0);
            var lisOfNeighborhs = new Dictionary<Point, Neuron<double>>(mapSize);
            int x = bmuCoordinats.X;
            int y = bmuCoordinats.Y;
            int leftBorder = x - neighborhood;
            int rightBorder = x + neighborhood;
            int topBorder = y - neighborhood;
            int bottomBorder = y + neighborhood;
            leftBorder = leftBorder < 0 ? 0 : leftBorder;
            rightBorder = rightBorder > mapSize ? mapSize : rightBorder;
            topBorder = topBorder < 0 ? 0 : topBorder;
            bottomBorder = bottomBorder > mapSize ? mapSize : bottomBorder;

            for (int i = topBorder; i < bottomBorder; i++)
            {
                for (int j = leftBorder; j < rightBorder; j++)
                {
                    lisOfNeighborhs.Add(new Point(i, j), _map.Map[i, j]);
                }
            }

            return lisOfNeighborhs;
        }

        #endregion
    }
}
