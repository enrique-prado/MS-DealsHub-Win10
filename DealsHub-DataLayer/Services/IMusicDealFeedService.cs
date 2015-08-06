using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.FeedModels;
using MSDealsDataLayer.Models;

namespace MSDealsDataLayer.Services
{
    public interface IMusicDealFeedService
    {
        Task<DiscoveryContentList> GetDiscoveryContentListAsync();
        Task<DiscoveryContentList> GetDiscoveryContentListAsync(string actionTarget);
        Task<EdsResponse> GetEdsInfo(string bingId);
    }
}
