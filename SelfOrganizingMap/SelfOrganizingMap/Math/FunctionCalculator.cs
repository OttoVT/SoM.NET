using SelfOrganizingMap.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Math
{
    //public abstract class FunctionCalculator<T> where T : IComparable, IComparable<T>
    //{
    //    public abstract FunctionResult<T> CalculateDistance(Vector<T> x, Vector<T> y);
    //}

    public static class Calculator //: FunctionCalculator<double>
    {
        public static FunctionResult<double> CalculateDistance(Vector<double> x, Vector<double> y)
        {
            Contract.Requires(x.Size != 0,
                "Vector Size property should not be equal zero");
            Contract.Requires(x.Size == y.Size, 
                "To compute distance between two vectors both of them should have equal Size property");

            if (x == y)
            {
                return new FunctionResult<double>(0);
            }
            
            double distance = 0;
            int size = x.Size;

            for (int i = 0; i < size; i++)
            {
                distance += System.Math.Pow(x[i] - y[i], 2);
            }

            distance = System.Math.Sqrt(distance);

            return new FunctionResult<double>(distance);
        }
    }
}
