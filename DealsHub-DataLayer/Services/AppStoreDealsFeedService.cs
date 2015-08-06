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
using MSDealsDataLayer.Resources;
using MSDealsDataLayer.WinPhoneExtensions;

namespace MSDealsDataLayer.Services
{
    class AppStoreDealsFeedService : IAppStoreDealsFeedService
    {
        private static string _baseApiUrl = SharedResource.BaseUrlAppsStoreFeed_Tablet;

        //Empty Ctor necessary for XML Deserializer
        public AppStoreDealsFeedService()
        {

        }

        public async Task<AppStoreDealsFeedModel> GetAppStoreDealsFeedDataAsync()
        {
            return await GetAppStoreDealsFeedDataAsync(_baseApiUrl);
        }

        public async Task<AppStoreDealsFeedModel> GetAppStoreDealsFeedDataAsync(string url)
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
