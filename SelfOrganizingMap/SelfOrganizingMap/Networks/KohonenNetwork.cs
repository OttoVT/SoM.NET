using SelfOrganizingMap.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Networks
{
    public class KohonenNetwork<T> where T : IComparable, IComparable<T>
    {
        private readonly Neuron<T>[] _neuronLayer;
        private T _minValue;

        public Neuron<T>[] NeuronLayer
        {
            get
            {
                return _neuronLayer;
            }
        }

        public KohonenNetwork(Neuron<T>[] neuronLayer, T minValue)
        {
            _neuronLayer = neuronLayer;
            _minValue = minValue;
        }

        /// <summary>
        /// Concurrency function for calculating winner neuron
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Returns winner neuron position</returns>
        public virtual int ConcurrencyFunction(Vector<T> vector)
        {
            int size = _neuronLayer.Count();
            int winnerPosition = 0;
            T max = default(T);
            for (int i = 0; i < size; i++)
            {
                var neuron = _neuronLayer[i];
                var result = neuron.LinearWeightedAdderFunction(vector);
                if (max.CompareTo(result) < 0)
                {
                    max = result;
                    winnerPosition = i;
                }
            }

            return winnerPosition;
        }

        public virtual int ConcurrencyFunction(Vector<T> vector, double[] priorities, double pMin)
        {
            int size = _neuronLayer.Count();
            int winnerPosition = 0;
            T min = _minValue;
            for (int i = 0; i < size; i++)
            {
                if (priorities[i] < pMin)
                    continue;

                var neuron = _neuronLayer[i];
                var result = neuron.Distance(vector);
                if (min.CompareTo(result) > 0)
                {
                    min = result;
                    winnerPosition = i;
                }
            }

            return winnerPosition;
        }
    }
}
