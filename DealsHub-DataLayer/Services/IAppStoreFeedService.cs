using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.FeedModels;

namespace MSDealsDataLayer.Services
{
    public interface IAppStoreFeedService
    {
        Task<AppStoreDealsFeedModel> GetDealsHubCollectionFeedDataAsync();
        Task<AppStoreDealsFeedModel> GetRedStripeDealsCollectionFeedDataAsync();
        Task<AppStoreDealsFeedModel> GetMusicLoversCollectionFeedDataAsync();

        Task<AppStoreDealsFeedModel> GetAppStoreFeedDataAsync(string url);
    }

}
