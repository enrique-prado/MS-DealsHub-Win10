using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDealsDataLayer.FeedModels
{
    public class EdsResponse
    {
        public EdsItem[] Items { get; set; }
        public string ImpressionGuid { get; set; }
    }

    public class EdsItem
    {
        public string ID { get; set; }
        public string ReleaseDate { get; set; }
        public EdsProvider[] Providers { get; set; }
}

    public class EdsProvider
    {
        public string ProductId { get; set; }
        public EdsProviderContent[] ProviderContents { get; set; } 
    }

    public class EdsProviderContent
    {
        public EdsOfferInstance[] OfferInstances { get; set; }
    }

    public class EdsOfferInstance
    {
        private string _endDate = "";
        private string _startDate = "";
        private string _offerDisplay = "";

        public string EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public string OfferId { get; set; }
        public string OfferInstanceId { get; set; }

        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public string OfferDisplay
        {
            get { return _offerDisplay; }
            set { _offerDisplay = value; }
        }
    }

    public class EdsOfferDisplay
    {
        public string currencyCode { get; set; }
        public string displayPrice { get; set; }
    }
}
