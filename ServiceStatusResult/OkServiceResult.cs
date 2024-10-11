using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStatusResult
{
    public class OkServiceResult<T>:ServiceResultBase<T>
    {
     
        public OkServiceResult() :base(200,null,default)
        {

        }
        public OkServiceResult(T result):base(200,null,result)
        {
            
        }

        public OkServiceResult(string message, T result):base(200,message,result) 
        {
            
        }
    }
}
