using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Math
{

    public class Vector<T> where T: IComparable, IComparable<T>
    {
        private readonly T[] _vector;

        public int Size
        {
            get
            {
                return _vector.Count();
            }
        }

        public T[] Object
        {
            get
            {
                return _vector;
            }
        }

        public T this[int index]
        {
            get
            {
                return _vector[index];
            }
        }

        public Vector(T[] vector)
        {
            this._vector = vector;
        }

        public static Vector<T> operator  -(Vector<T> v1, Vector<T> v2)
        {
            Contract.Requires(v1.Size == v2.Size, "Vectors should of the same size");

            int size = v1.Size;
            T[] array = new T[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = (dynamic)v1[i] - (dynamic)v2[i];
            }

            return new Vector<T>(array);
        }

        public static Vector<T> operator +(Vector<T> v1, Vector<T> v2)
        {
            Contract.Requires(v1.Size == v2.Size, "Vectors should of the same size");

            int size = v1.Size;
            T[] array = new T[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = (dynamic)v1[i] + (dynamic)v2[i];
            }

            return new Vector<T>(array);
        }

        public static Vector<T> operator *(Vector<T> v1, double d)
        {
            int size = v1.Size;
            T[] array = new T[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = (dynamic)v1[i] * d;
            }

            return new Vector<T>(array);
        }
    }
}
