using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router
{
    public interface IRouter<T> 
        where T :class
    {
        T Choose(IList<T> instances);
    }
}
