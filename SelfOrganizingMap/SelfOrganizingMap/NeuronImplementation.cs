using SelfOrganizingMap.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap
{
    public class Neuron : Neuron<double>
    {
        public Neuron(Vector<double> weights) : base(weights)
        {
        }

        public override double LinearWeightedAdderFunction(Vector<double> vector)
        {
            Contract.Requires(Weights.Size == vector.Size,
                "vector and weights should have the same size property");

            double summ = _bias;
            int size = vector.Size;
            for (int i = 0; i < size; i++)
            {
                summ += vector[i] * _weights[i];
            }

            return summ;
        }

        public override double Distance(Vector<double> vector)
        {
            Contract.Requires(Weights.Size == vector.Size,
                "vector and weights should have the same size property");

            double summ = Calculator.CalculateDistance(vector, _weights).Result;

            return summ;
        }

    }
}
