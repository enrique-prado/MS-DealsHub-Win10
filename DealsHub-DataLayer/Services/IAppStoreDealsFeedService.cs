using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.FeedModels;

namespace MSDealsDataLayer.Services
{
    public interface IAppStoreDealsFeedService
    {
        Task<AppStoreDealsFeedModel> GetAppStoreDealsFeedDataAsync();
        Task<AppStoreDealsFeedModel> GetAppStoreDealsFeedDataAsync(string url);
    }
}
