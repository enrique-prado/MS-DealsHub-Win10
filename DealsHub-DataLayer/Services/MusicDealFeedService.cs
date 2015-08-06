using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using MSDealsDataLayer.FeedModels;
#if WINDOWS_PHONE
using MusicDealShared.WinPhoneExtensions;
#endif
using MSDealsDataLayer.Resources;
using MSDealsDataLayer.Utils;
using MSDealsDataLayer.WinPhoneExtensions;

namespace MSDealsDataLayer.Services
{
    class MusicDealFeedService : IMusicDealFeedService
    {
        private static string _baseUrl = SharedResource.Shared_BaseURL;
        private static string _baseEdsUrl = SharedResource.Eds_BaseURL;

        public async Task<DiscoveryContentList> GetDiscoveryContentListAsync()
        {
            return await GetDiscoveryContentListAsync(_baseUrl);
        }

        public async Task<DiscoveryContentList> GetDiscoveryContentListAsync(string actionTarget)
        {
            DiscoveryContentList contentList = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionTarget);
            request.Accept = "application/json";
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
                Debug.WriteLine("Deserializing: " + actionTarget);
                contentList = await Serialization.DeserializeJSON<DiscoveryContentList>(response.GetResponseStream());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WebException- " + ex.Message);
                //throw new DataLayerFeedException(actionTarget);
            }
            return contentList;
        }

        public async Task<EdsResponse> GetEdsInfo(string Id)
        {
            string edsRequestUrl = _baseEdsUrl + SharedResource.ClientLocale + "/details?ids=" + Id + "&mediaGroup=MusicType&fields=OfferDisplay&targetDevices=WindowsPC&firstPartyOnly=true";
            EdsResponse edsResponse = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(edsRequestUrl);
            request.Headers["x-xbl-build-version"] = "current";
            request.Headers["x-xbl-client-type"] = "X13";
            request.Headers["x-xbl-client-version"] = "2.2.931.0";
            request.Headers["x-xbl-contract-version"] = "3.2";
            request.Headers["x-xbl-device-type"] = "Windows8_1PC";
            
            request.Accept = "application/json";
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
                Debug.WriteLine("Deserializing: " + edsRequestUrl);
                edsResponse = await Serialization.DeserializeJSON<EdsResponse>(response.GetResponseStream());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WebException- " + ex.Message);
            }
            return edsResponse;
        }
    }
}
