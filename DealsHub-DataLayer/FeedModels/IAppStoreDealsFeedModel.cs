using System.Collections.ObjectModel;

namespace MSDealsDataLayer.FeedModels
{
    public interface IAppStoreDealsFeedModel
    {
        string TopicId { get; set; }
        string TopicName { get; set; }
        string TopicDescription { get; set; }
        ObservableCollection<AppStoreItem> Items { get; set; }
    }
}
