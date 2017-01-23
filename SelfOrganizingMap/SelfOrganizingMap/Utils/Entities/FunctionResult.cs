using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfOrganizingMap.Utils.Entities
{
    public class FunctionResult<T>
    {
        private T _result;
        public T Result
        {
            get
            {
                return _result;
            }
        }

        public FunctionResult(T result)
        {
            _result = result;
        }
    }
}
