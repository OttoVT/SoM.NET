using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Utils
{
    public class KohonenMapConstants
    {
        private readonly double _learningRateDecay;
        private readonly double _neighborhoodDecay;
        private readonly double _startLearningRate;
        private readonly double _startNeighborhood;

        public double LearningRateDecay { get { return _learningRateDecay; }}
        public double NeighborhoodDecay { get { return _neighborhoodDecay; }}
        public double StartLearningRate { get { return _startLearningRate; }}

        public KohonenMapConstants(double mapSize,
            double startLearningRate, double learningRateDecay, double iterationAmount)
        {
            _neighborhoodDecay = iterationAmount / System.Math.Log(mapSize);
            _startLearningRate = startLearningRate;
            _learningRateDecay = learningRateDecay;
        }
    }
}
