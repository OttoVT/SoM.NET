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
        private readonly KohonenMapConstants _constants;
        public KohonenMapTraining(KohonenMap map, KohonenMapConstants constants)
        {
            _map = map;
            _constants = constants;
        }

        public void TrainOnce(Vector<double> vector, int iteration)
        {
            Tuple<int, int> BMU = _map.ConcurrencyFunction(vector);
            Neuron<Double> bmuNeuron = _map.Map[BMU.Item1, BMU.Item2];
            //var weights = bmuNeuron.Weights;
            double learningRateValue = DecayLearningRate(iteration);
            double neighborhood = DecayNeighborhood(iteration);
            #region Update BMU

            //Vector<double> vectorDifference = vector - weights;
            //Vector<double> result = weights + (dynamic)influenceValue * learningRateValue * vectorDifference;
            //neuron.Weights = result;
            #endregion

            var neighborhs = GetNeighborhood((int)System.Math.Floor(neighborhood), BMU);

            foreach (var neuron in neighborhs)
            {
                double distance = neuron.Distance(vector);
                double influenceValue = DecayInfluence(iteration, distance, neighborhood);
                var weights = neuron.Weights;
                Vector<double> vectorDifference = vector - weights;
                Vector<double> result = weights + vectorDifference * (dynamic)learningRateValue;
                neuron.Weights = result;
            }
        }

        public void TrainOnSetNTimes(Vector<double>[] trainingSet, int iterationsCount)
        {
            for (int i = 0; i < iterationsCount; i++)
            {
                for (int j = 0; j < trainingSet.Count(); j++)
                {
                    var set = trainingSet[j];
                    TrainOnce(set, i);
                }
            }
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
            var calulatedNeighborhood = _constants.StartLearningRate * System.Math.Exp(power);
            return calulatedNeighborhood;
        }

        private double DecayInfluence(int iterataion, double distance, double neighborhood)
        {
            //var neighborhood = DecayNeighborhood(iterataion);
            var power = -(distance * distance) / (2 * neighborhood * neighborhood);
            var calulatedInfluence = System.Math.Exp(power);
            return calulatedInfluence;
        }

        private IEnumerable<Neuron<double>> GetNeighborhood(int neighborhood, Tuple<int,int> bmuCoordinats)
        {
            int mapSize = _map.Map.GetLength(0);
            var lisOfNeighborhs = new List<Neuron<double>>(mapSize);
            int x = bmuCoordinats.Item1;
            int y = bmuCoordinats.Item2;
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
                    lisOfNeighborhs.Add(_map.Map[i, j]);
                }
            }

            return lisOfNeighborhs;
        }
    }
}
