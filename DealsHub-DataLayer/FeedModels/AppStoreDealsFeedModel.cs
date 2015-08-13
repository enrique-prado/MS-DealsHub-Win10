using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MSDealsDataLayer.FeedModels
{
    [XmlRoot("Tr")]
    public class AppStoreDealsFeedModel : IAppStoreDealsFeedModel
    {
        public AppStoreDealsFeedModel()
        {
            //Items = new ObservableCollection<AppStoreItem>();
        }

        public AppStoreDealsFeedModel(string topicId, string topicName, string description)
        {
            TopicId = topicId;
            TopicName = topicName;
            TopicDescription = description;
            Items = new ObservableCollection<AppStoreItem>();
        }

        [XmlElement(ElementName = "I", Type = typeof(string))]
        public string TopicId { get; set; }
        [XmlElement(ElementName = "T", Type = typeof(string))]
        public string TopicName { get; set; }
        [XmlElement(ElementName = "D", Type = typeof(string))]
        public string TopicDescription { get; set; }
        [XmlArray("Pts")]
        [XmlArrayItem("Pt", Type = typeof(AppStoreItem))]
        public ObservableCollection<AppStoreItem> Items { get; set; }
    }
}
