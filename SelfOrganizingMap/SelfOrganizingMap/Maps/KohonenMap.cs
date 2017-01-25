using SelfOrganizingMap.Math;
using SelfOrganizingMap.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Maps
{
    public class KohonenMap
    {
        private readonly Neuron<double>[,] _map;
        private readonly int _sizeOfMap;

        public Neuron<double>[,] Map { get { return _map; } }

        public KohonenMap(Neuron<double>[,] map)
        {
            Contract.Requires(_map.GetLength(0) == _map.GetLength(1), "map should be presented as square matrice");
            _map = map;
            _sizeOfMap = _map.GetLength(0);
        }

        public virtual Point ConcurrencyFunction(Vector<double> vector)
        {
            int size = _map.GetLength(0);
            int winnerPosition = 0;
            double min = _map[0, 0].Distance(vector);
            int neuronAmont = size * size;
            for (int i = 1; i < neuronAmont; i++)
            {
                var row = i / size;
                var column = i % size;

                var neuron = _map[row, column];
                var result = neuron.Distance(vector);
                if (min.CompareTo(result) > 0)
                {
                    min = result;
                    winnerPosition = i;
                }
            }

            return new Point(winnerPosition / size, winnerPosition % size);
        }
    }
}
