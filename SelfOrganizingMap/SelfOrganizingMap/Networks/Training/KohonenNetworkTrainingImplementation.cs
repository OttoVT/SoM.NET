using SelfOrganizingMap.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Networks.Training
{
    public class KohonenNetworkTraining : KohonenNetworkTraining<double>
    {
        //protected override double CalculateError(IEnumerable<Vector<double>> trainingData)
        //{
        //    var size = trainingData.Count();
        //    double error = 0;
        //    double difference = 0;
        //    foreach (var set in trainingData)
        //    {
        //        var winnerNeuronPosition = _network.ConcurrencyFunction(set);
        //        var weights = _network.NeuronLayer[winnerNeuronPosition].Weights;
        //        difference += System.Math.Pow(set - weights, 2);
        //    }

        //    error = difference / size;
        //    return error;
        //}
        public KohonenNetworkTraining(KohonenNetwork<double> network) : base(network)
        {
        }

        protected override double CalculateError(IEnumerable<Vector<double>> trainingData)
        {
            var size = trainingData.Count();
            double error = 0;
            double difference = 0;
            foreach (var set in trainingData)
            {
                var winnerNeuronPosition = _network.ConcurrencyFunction(set);
                var weights = _network.NeuronLayer[winnerNeuronPosition].Weights;
                difference += Calculator.CalculateDistance(set, weights).Result;
            }

            error = difference / size;
            return error;

        }
    }
}
