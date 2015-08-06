using System.Xml.Serialization;

namespace MSDealsDataLayer.FeedModels
{
    public sealed class AppCategory
    {
        public AppCategory()
        {
            
        }

        [XmlElement(ElementName = "I")]
        public string Id { get; set; }
        [XmlElement(ElementName = "N")]
        public string Name { get; set; }

    }
}
