using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStatusResult
{
    public class ProblemServiceResult <T>:ServiceResultBase<T>
    {
        public ProblemServiceResult():base(500,null,default){}

        public ProblemServiceResult(T result):base(500,null,result) {}

        public ProblemServiceResult(string message, T result=default):base(500,message,result){}
    }
}
