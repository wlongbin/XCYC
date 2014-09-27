using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE
{
    public class ExceptionHandle : ApplicationException
    {
        public ExceptionHandle() { }
        public ExceptionHandle(string message) : base(message) { }
        public ExceptionHandle(string message, Exception inner) : base(message, inner) { }
    }
}