using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDealsDataLayer
{
    public class DataLayerFeedException : Exception
    {
        public DataLayerFeedException()
        {
            
        }

        public DataLayerFeedException(string message) : base(message)
        {
            
        }
    }
}
