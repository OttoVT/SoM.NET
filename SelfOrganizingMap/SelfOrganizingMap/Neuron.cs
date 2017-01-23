using SelfOrganizingMap.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap
{
    public abstract class Neuron<T> where T : IComparable, IComparable<T>
    {
        protected T _bias;
        protected Vector<T> _weights;
        public Vector<T> Weights
        {
            get
            {
                return _weights;
            }
            set
            {
                if (value.Size == _weights.Size)
                    _weights = value;
            }
        }

        public Neuron(Vector<T> weights)
        {
            _weights = weights;
        }
        public abstract T LinearWeightedAdderFunction(Vector<T> vector);
        public abstract T Distance(Vector<T> vector);
    }
}