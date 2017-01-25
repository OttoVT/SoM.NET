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
        private double _minErrorToStop;
        private double _currentError = 100;

        public KohonenMapTraining(KohonenMap map, KohonenMapConstants constants, double minErrorToStop) 
            : this(map, constants)
        {
            _minErrorToStop = minErrorToStop;
            _currentError = minErrorToStop * 100;
        }

        public KohonenMapTraining(KohonenMap map, KohonenMapConstants constants)
        {
            _map = map;
            _mapSize = _map.Map.GetLength(0);
            _constants = constants;
        }

        public void TrainOnce(Vector<double> vector, int iteration)
        {
            Point BMU = _map.ConcurrencyFunction(vector);
            double neighborhood = DecayNeighborhood(iteration);

            Dictionary<Point, Neuron<double>> neighborhs =
                GetNeighborhood((int)neighborhood, BMU);

            foreach (var kvp in neighborhs)
            {
                var neuron = kvp.Value;
                var point = kvp.Key;
                _currentError += ModifyWeights(neuron, vector, BMU, point, iteration);
            }

            _currentError = System.Math.Abs(_currentError / (_mapSize * _mapSize));
        }

        public void TrainOnSet(Vector<double>[] trainingSet, int iteration)
        {
            for (int j = 0; j < trainingSet.Count(); j++)
            {
                var set = trainingSet[j];
                TrainOnce(set, iteration);
            }
        }

        public void TrainOnSetNTimes(Vector<double>[] trainingSet, int iterationsCount)
        {

            for (int i = 0; i < iterationsCount ; i++) //&& _minErrorToStop < _currentError
            {
                TrainOnSet(trainingSet, i);
            }
        }

        public void TrainOnSetsNTimes(List<Vector<double>> patterns, int iterationsCount)
        {
            int i = 0;
            while (i <= iterationsCount)
            {
                List<Vector<double>> patternsToLearn = new List<Vector<double>>(patterns.Count);
                foreach (Vector<double> pArray in patterns)
                    patternsToLearn.Add(pArray);
                Random randomPattern = new Random();
                Vector<double> pattern;
                for (int j = 0; j < patterns.Count; j++)
                {
                    pattern = patternsToLearn[randomPattern.Next(patterns.Count - j)];

                    TrainOnce(pattern, i);

                    patternsToLearn.Remove(pattern);
                }
                i++;
            }
        }

        private double ModifyWeights(Neuron<double> neuron, 
            Vector<double> vector,
            Point winnerCoordinate,
            Point neuronCoordinate,
            int iteration)
        {
            var weights = neuron.Weights;
            double avgDelta = 0;
            double modificationValue = 0;
            for (int i = 0; i < vector.Size; i++)
            {
                modificationValue = DecayLearningRate(iteration) *
                    DecayInfluence(winnerCoordinate, neuronCoordinate, iteration) * (vector[i] - weights[i]);
                weights.Object[i] += modificationValue;
                avgDelta += modificationValue;
            }
            avgDelta = avgDelta / vector.Size;
            return avgDelta;
        }

        private double DecayNeighborhood(int iterataion)
        {
            var power = -iterataion / _constants.NeighborhoodDecay;
            var calulatedNeighborhood = _constants.StartNeighborhood * System.Math.Exp(power);
            return calulatedNeighborhood;
        }

        private double DecayLearningRate(int iterataion)
        {
            var power = -iterataion / _constants.LearningRateDecay;
            var calulatedLearningRate = _constants.StartLearningRate * System.Math.Exp(power);
            return calulatedLearningRate;
        }

        private double DecayInfluence(Point winnerCoordinate, Point neuron, int iteration)
        {
            double result = 0;
            double distance = 0;
            distance = System.Math.Sqrt(System.Math.Pow((winnerCoordinate.X - neuron.X), 2)
                    + System.Math.Pow((winnerCoordinate.Y - neuron.Y), 2));
            result = System.Math.Exp(-(distance * distance) / (System.Math.Pow(DecayNeighborhood(iteration), 2)));

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
    }
}
