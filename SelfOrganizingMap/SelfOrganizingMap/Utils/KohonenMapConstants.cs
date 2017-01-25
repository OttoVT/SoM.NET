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
        public double StartNeighborhood { get { return _startNeighborhood; } }

        public KohonenMapConstants(double startNeighborhood,
            double startLearningRate, double learningRateDecay, double iterationAmount)
        {
            _startNeighborhood = startNeighborhood;
            _neighborhoodDecay = iterationAmount / System.Math.Log(startNeighborhood);
            _startLearningRate = startLearningRate;
            _learningRateDecay = learningRateDecay;
        }
    }
}
