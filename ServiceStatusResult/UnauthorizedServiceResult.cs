using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStatusResult
{
    public class UnauthorizedServiceResult<T> : ServiceResultBase<T>
    {
        public UnauthorizedServiceResult() : base(401,null,default){}

        public UnauthorizedServiceResult(T result) : base(401,null, result){}

        public UnauthorizedServiceResult(string message, T result=default) : base(401, message, result){}
    }
}
