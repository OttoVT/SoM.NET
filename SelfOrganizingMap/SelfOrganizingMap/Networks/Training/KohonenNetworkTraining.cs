using SelfOrganizingMap.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Networks.Training
{
    public abstract class KohonenNetworkTraining<T> where T : IComparable, IComparable<T>
    {
        protected readonly KohonenNetwork<T> _network;
        protected readonly double[] _priorities;
        public double MinimalPriority { get; set; }
        public T TrainingSpeed { get; set; }
        public KohonenNetworkTraining(KohonenNetwork<T> network)
        {
            _network = network;
            int size = _network.NeuronLayer.Count();
            _priorities = new double[size];
            for (int i = 0; i < size; i++)
            {
                _priorities[i] = 1.0 / size;
            }
        }

        public void TrainOnce(Vector<T> vector)
        {
            var winnerNeuronPosition = _network.ConcurrencyFunction(vector, _priorities, MinimalPriority);
            var weights = _network.NeuronLayer[winnerNeuronPosition].Weights;
            //Apply Kohonens rule
            weights = weights +  (vector - weights) * (dynamic)TrainingSpeed;
            _network.NeuronLayer[winnerNeuronPosition].Weights = weights;
            //Change Priorities
            var size = _network.NeuronLayer.Count();
            var diff = 1.0 / size;
            for (int i = 0; i < size; i++)
            {
                if (winnerNeuronPosition != i)
                {
                    _priorities[i] += diff;
                }
                else
                {
                    _priorities[i] -= MinimalPriority;
                }
            }
        }

        public void TrainOnSet(IEnumerable<Vector<T>> trainingData)
        {
            foreach (var set in trainingData)
            {
                TrainOnce(set);
            }
        }

        public void TrainOnSetNTimes(IEnumerable<Vector<T>> trainingData, int iterationAmount)
        {
            for (int i = 0; i < iterationAmount; i++)
            {
                TrainOnSet(trainingData);
                var error = CalculateError(trainingData);
            }
        }
        protected abstract T CalculateError(IEnumerable<Vector<T>> trainingData);
        //protected T CalculateError(IEnumerable<Vector<T>> trainingData)
        //{
        //    var size = trainingData.Count();
        //    T error = default(T);
        //    foreach (var set in trainingData)
        //    {
        //        var winnerNeuronPosition = _network.ConcurrencyFunction(set);
        //        var weights = _network.NeuronLayer[winnerNeuronPosition].Weights;
        //        var difference += (dynamic)System.Math.Pow(set - weights, 2);
        //    }

        //    error = difference / size
        //    return error;
        //}
    }
}
