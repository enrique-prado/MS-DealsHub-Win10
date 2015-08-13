using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.FeedModels;
using MSDealsDataLayer.Models;
using MSDealsDataLayer.Services;
using MSDealsDataLayer.Utils;

namespace MSDealsDataLayer
{
    public static class DataLayer
    {

        public static void SetDefaultFeedService()
        {
            _feedService = new MusicDealFeedService();
        }

        public static void SetFeedService(IMusicDealFeedService feedService)
        {
            _feedService = feedService;
        }

        public static async Task<List<Category>> GetCategoriesAsync()
        {
            EnsureFeedService();
            var discoveryContentList = await _feedService.GetDiscoveryContentListAsync();
            var categories = new List<Category>();
            if (discoveryContentList == null)
            {
                throw new DataLayerFeedException();
            }
            for (int i = 0; i < discoveryContentList.Items.Length; i++)
            {
                var item = discoveryContentList.Items[i];
                if (item.ItemType == DiscovertyItemConst.DiscoveryItemFlexHub)
                {
                    var category = CreateCategoryFromDiscoveryItem(item);
                    categories.Add(category);
                }
            }
            return categories;
        }

        public static async Task<List<Album>> GetAlbumsAsync(string categoryActionTarget)
        {
            var albums = new List<Album>();
            EnsureFeedService();
            var discoveryContentList = await _feedService.GetDiscoveryContentListAsync(categoryActionTarget);
            if (discoveryContentList == null)
            {
                return albums;
            }
            for (int i = 0; i < discoveryContentList.Items.Length; i++)
            {
                var item = discoveryContentList.Items[i];
                if (item.ItemType == DiscovertyItemConst.DiscoveryItemAlbum)
                {
                    var album = CreateAlbumFromDiscoveryItem(item);
                    albums.Add(album);
                }
            }
            return albums;
        }


        public static async Task<Deal> GetDealAsync(string albumBingId)
        {
            EnsureFeedService();
            var edsResponse = await _feedService.GetEdsInfo(albumBingId);
            var edsOfferInstance = FindOffer(edsResponse);

            return await CreateDealFromEdsOfferInstance(edsOfferInstance);
        }

        private static EdsOfferInstance FindOffer(EdsResponse edsResponse)
        {
            if (edsResponse == null || edsResponse.Items == null)
            {
                return null;
            }
            if (edsResponse.Items.Length > 0)
            {
                var edsItem = edsResponse.Items[0];
                if (edsItem.Providers != null && edsItem.Providers.Length > 0)
                {
                    var edsProvider = edsItem.Providers[0];
                    if (edsProvider.ProviderContents != null && edsProvider.ProviderContents.Length > 0)
                    {
                        var edsContent = edsProvider.ProviderContents[0];
                        if (edsContent.OfferInstances != null &&
                            edsContent.OfferInstances.Length > 0)
                        {
                            var edsOffer = edsContent.OfferInstances[0];
                            return edsOffer;
                        }
                    }
                }
            }
            return null;
        }

        private static Category CreateCategoryFromDiscoveryItem(DiscoveryItem di)
        {
            var category = new Category()
            {
                ActionTarget = di.ActionTarget,
                ImageId = di.ImageId,
                ImageUrl = di.ImageUrl,
                Rank = di.Rank,
                Text = di.Text,
                Title = di.Title,
            };
            category.Albums = new List<Album>();

            return category;
        }

        private static Album CreateAlbumFromDiscoveryItem(DiscoveryItem di)
        {
            var album = new Album()
            {
                Genre = di.Genre,
                SubGenre = di.SubGenre,
                IsExplicit = di.IsExplicit,
                Label = di.Label,
                Title = di.Title,
                Artist = di.Text,
                Duration = di.Duration,
                ImageUrl = di.ImageUrl,
                ItemId = di.ItemId,
                LongDescription = di.LongDescription,
                Rank = di.Rank,
                BingId = di.BingId
            };
            var releasedDate = Convert.ToDateTime(di.ReleaseDate);
            album.ReleasedYear = releasedDate.Year;
            return album;
        }

        private static async Task<Deal> CreateDealFromEdsOfferInstance(EdsOfferInstance edsOfferInstance)
        {
            if (edsOfferInstance == null)
            {
                return new Deal()
                {
                    OfferEndDate = DateTime.Now,
                    OfferStartDate = DateTime.Now,
                    Price = "-99"
                };
            }
            var deal = new Deal()
            {
                OfferEndDate = Convert.ToDateTime(edsOfferInstance.EndDate),
                OfferStartDate = Convert.ToDateTime(edsOfferInstance.StartDate)
                    
            };

            if (edsOfferInstance.EndDate == "" )
            {
                deal.OfferEndDate = DateTime.Now;
                Debug.WriteLine("No Deal End Dates");
            }

            if (edsOfferInstance.StartDate == "")
            {
                deal.OfferStartDate = DateTime.Now;
                Debug.WriteLine("No Deal Start Dates");
            }

            if (edsOfferInstance.OfferDisplay != "")
            {
                var offerDisplay = await GetPriceFromString(edsOfferInstance.OfferDisplay);
                deal.CurrencyCode = offerDisplay.currencyCode;
                deal.Price = offerDisplay.displayPrice;
            }
            else
            {
                deal.Price = "-99";
                Debug.WriteLine("NO Offer detail!!!");
            }
            return deal;
        }

        private static async Task<EdsOfferDisplay> GetPriceFromString(string offerDisplay)
        {
            return await Serialization.DeserializeJSON<EdsOfferDisplay>(offerDisplay);
        }

        private static void EnsureFeedService()
        {
            if (_feedService == null)
            {
                SetDefaultFeedService();
            }
        }

        private static IMusicDealFeedService _feedService;
    }
}
