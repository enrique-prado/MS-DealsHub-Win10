using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDealsDataLayer.Models
{
    public class Deal
    {
        public string CurrencyCode { get; set; }
        public string Price { get; set; }
        public DateTime OfferStartDate { get; set; }
        public DateTime OfferEndDate { get; set; }
    }
}
