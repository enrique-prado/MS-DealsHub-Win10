using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSDealsDataLayer.FeedModels
{
    public class AppStoreItem : IAppStoreItem
    {
        public AppStoreItem()
        {
        }

        public AppStoreItem(string id, string name, string imageUrl, double rating, int numRevs, string category,
            string currencySym, double price, string currCode)
        {
            //Set imageUrl
            Assets = new List<AppAsset>();
            var asset = new AppAsset {Images = new List<AppImage>()};
            var image = new AppImage {Size = 1, Url = imageUrl};
            asset.Images.Add(image);
            Assets.Add(asset);

            // set the rest of the properties
            Id = id;
            Language = "en-US"; //TODO: do not hardcode
            Name = name;
            Rating = rating;
            NumOfReviews = numRevs;
            Category = new AppCategory {Id = "id_category", Name = category};
            CurrencySymbol = currencySym;
            Price = Convert.ToString(price);
            CurrencyCode = currCode;
        }

        [XmlElement("I")]
        public string Id { get; set; } //Used to generate link to Store app (Phone only)
        [XmlElement("Pfn")]
        public string PackageFamilyName { get; set; } //Used to generate link to Store app (Tablet only)
        [XmlElement("L")]
        public string Language { get; set; }
        [XmlElement("T")]
        public string Name { get; set; }
        [XmlElement("Sr", Type = typeof(double))]
        public double Rating { get; set; }
        [XmlElement("Src", Type = typeof(int))]
        public int NumOfReviews { get; set; }
        [XmlElement("C")]
        public AppCategory Category { get; set; }
        [XmlElement("Cs")]
        public string CurrencySymbol { get; set; }
        [XmlElement("P", Type = typeof(double))] 
        public double FullPrice { get; set; }
        [XmlElement("Pp")] // This is the discounted price. 
        public string Price { get; set; } // This is type string to fix MSD-22 (See view model for full fix)
        [XmlElement("Cc")]
        public string CurrencyCode { get; set; }
        [XmlArray("Ats")]
        [XmlArrayItem("At", Type = typeof(AppAsset))]
        public List<AppAsset> Assets { get; set; }  
    }
}