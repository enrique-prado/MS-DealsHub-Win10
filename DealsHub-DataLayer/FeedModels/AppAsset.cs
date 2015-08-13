using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSDealsDataLayer.FeedModels
{
    public class AppAsset
    {
        public AppAsset()
        {
            
        }

        [XmlArray(ElementName = "Imgs")]
        [XmlArrayItem(ElementName = "Img", Type = typeof(AppImage))]
        public List<AppImage> Images { get; set; }
    }

    public sealed class AppImage
    {
        public AppImage()
        {
            
        }

        [XmlElement(ElementName = "T")]
        public int Size { get; set; }
        [XmlElement(ElementName = "U")]
        public string Url { get; set; }
    }
}
