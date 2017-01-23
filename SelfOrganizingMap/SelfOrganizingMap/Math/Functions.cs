using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Math
{
    public static class Functions
    {
        /// <summary>
        /// Returns Normalized double Vector
        /// </summary>
        /// <param name="vector"></param>
        public static Vector<double> NormalizeVector(Vector<double> vector)
        {
            double[] array = new double[vector.Size];

            var normValue = System.Math.Sqrt(vector.Object.Aggregate(1.0, (x, y) =>
            {
                x += y * y;
                return x;
            }));

            for (int i = 0; i < array.Count(); i++)
            {
                array[i] = vector[i] / normValue;
            }

            return new Vector<double>(array);
        }
    }
}
