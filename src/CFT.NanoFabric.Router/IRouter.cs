using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.NanoFabric.Router
{
    public interface IRouter<T> 
        where T :class
    {
        T Choose(IList<T> instances);
    }
}
