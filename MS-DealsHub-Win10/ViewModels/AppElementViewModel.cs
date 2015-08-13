using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.FeedModels;
using Windows.ApplicationModel.Resources;

namespace MSDealsDataLayer.ViewModels
{
    public sealed class AppElementViewModel
    {
        //private readonly string _baseStoreUrl = "http://apps.microsoft.com/webpdp/app/";
        private const string _idPrefix = "urn:uuid:";

        //Win Phone Store urls
        private const string _baseStoreUrlPhone = "ms-windows-store:navigate?appid={0}";
        private const string _storeImagesUrlPhone = "http://cdn.marketplaceimages.windowsphone.com/v8/images/";

        // Windows Tablet Store urls
        private const string _baseStoreUrl = "ms-windows-store:PDP?PFN={0}";
        private const string _storeImagesUrl = "http://wscont2.apps.microsoft.com/winstore/1x/";

        private string _currencySymbol = "";
        private bool _usePhoneFeed = false;

        private ResourceLoader _resloader = new ResourceLoader();


        public AppElementViewModel()
        {
        }

        public AppElementViewModel(IAppStoreItem appItem, bool usePhoneFeed)
        {
            if (appItem == null)
                throw new ArgumentNullException("appItem");

            _usePhoneFeed = usePhoneFeed;



            AppStoreId = ExtractAppId(appItem.Id);
            PackageFamilyName = appItem.PackageFamilyName;
            ThumbnailUrl = GetThumbnailUrl(appItem);
            AppName = appItem.Name;

            //Price value from feed comes in en-US culture format so we need to make sure it works when OS is set to a different culture.
            CultureInfo culture = CultureInfo.InvariantCulture;
            NumberStyles style = NumberStyles.AllowDecimalPoint;
            double price;
            Double.TryParse(appItem.Price, style, culture, out price);

            if (usePhoneFeed)
            {
                Rating = appItem.Rating / 2; // Converting rating score to [0 to 5 stars] range.
                Price = price;
            }
            else
            {
                Rating = appItem.Rating; //Rating range is [0 to 5] stars
                if (string.IsNullOrEmpty(appItem.Price))
                {
                    //Discounted price is missing so use full price
                    Price = appItem.FullPrice;
                }
                else
                {
                    Price = price;
                }
            }

            NumRatings = appItem.NumOfReviews;
            _currencySymbol = appItem.CurrencySymbol;
            Category = GetCatogoryName(appItem);

            if (usePhoneFeed)
            {
                //Remove leading characters from app Id
                AppStoreLink = string.Format(_baseStoreUrlPhone, AppStoreId);
            }
            else
            {
                AppStoreLink = string.Format(_baseStoreUrl, appItem.PackageFamilyName);
            }
        }

        private string GetCatogoryName(IAppStoreItem appItem)
        {
            if (null != appItem && null != appItem.Category && null != appItem.Category.Name)
            {
                return appItem.Category.Name;
            }
            else
            {
                return _resloader.GetString("NoCategory");
            }
        }

        private string GetThumbnailUrl(IAppStoreItem appItem)
        {
            if (null != appItem && null != appItem.Assets && appItem.Assets.Count > 0)
            {
                var asset = appItem.Assets.FirstOrDefault();
                if (null != asset && null != asset.Images && asset.Images.Count > 0)
                {
                    foreach (var image in asset.Images)
                    {
                        // Check image size to see if it's the square tile
                        if (image.Size == 1)
                        {
                            if (image.Url == string.Empty)
                            {
                                return "Assets/LightGray.png"; // Renturn place holder image
                            }
                            if (_usePhoneFeed)
                            {
                                return _storeImagesUrlPhone + ExtractAppId(image.Url);
                            }
                            else
                            {
                                return _storeImagesUrl + ExtractAppId(image.Url);
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        private string ExtractAppId(string idFromFeed)
        {
            if (idFromFeed.StartsWith(_idPrefix)) // The phone store feed inserts extra characters for some reason, let's remove them.
            {
                //remove prefix from id string and return it
                var id = idFromFeed.Substring(_idPrefix.Length);
                return id;
            }
            return idFromFeed;
        }

        public string FormattedPrice
        {
            get
            {
                if (Equals(Price, 0.0))
                {
                    return _resloader.GetString("FreePrice");
                }
                else
                {
                    return string.Format(CultureInfo.CurrentCulture, "{0:C}", Price);
                }
            }
        }

        public string AppStoreId { get; set; }
        public string PackageFamilyName { get; set; }
        public string ThumbnailUrl { get; set; }
        public string AppName { get; set; }
        public double Rating { get; set; }
        public int NumRatings { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string AppStoreLink { get; set; }
    }
}
