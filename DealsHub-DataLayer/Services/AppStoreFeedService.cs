using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MSDealsDataLayer.FeedModels;
using Windows.ApplicationModel.Resources;

namespace MSDealsDataLayer.Services
{
    public class AppStoreFeedService : IAppStoreFeedService
    {
        //Empty Ctor necessary for XML Deserializer
        public AppStoreFeedService()
        {

        }

        public async Task<AppStoreDealsFeedModel> GetDealsHubCollectionFeedDataAsync()
        {
            var resources = new ResourceLoader();
            var baseFeedUrl = resources.GetString("DealsHubStoreFeedUrl_Phone");

            return await GetAppStoreFeedDataAsync(baseFeedUrl);
        }

        public Task<AppStoreDealsFeedModel> GetRedStripeDealsCollectionFeedDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppStoreDealsFeedModel> GetMusicLoversCollectionFeedDataAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<AppStoreDealsFeedModel> GetAppStoreFeedDataAsync(string url)
        {
            AppStoreDealsFeedModel feedData = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "application/xml";
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                Debug.WriteLine("Deserializing: " + url);

                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = XmlReader.Create(responseStream);
                    var serializer = new XmlSerializer(typeof(AppStoreDealsFeedModel));

                    if (!serializer.CanDeserialize(reader))
                        throw new FormatException(string.Format("Xml feed not formatted properly: {0}", url));

                    feedData = (AppStoreDealsFeedModel)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception while downloading feed: " + ex.Message);
            }

            return feedData;
        }
    }
}
