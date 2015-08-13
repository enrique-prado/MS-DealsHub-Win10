using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.Services;

namespace MSDealsDataLayer.ViewModels
{
    public class AppsPageViewModel
    {
        private IAppStoreFeedService _dataService = null;
        private bool _usePhoneFeed = false;

        public AppsPageViewModel(IAppStoreFeedService dataService, bool usePhoneFeed)
        {
            _usePhoneFeed = usePhoneFeed;
            if (dataService == null)
            {
                if (usePhoneFeed)
                {
                //_dataService = new PhoneStoreDealsFeedService();
                }
                else
                {
                _dataService = new AppStoreFeedService();
                //_dataService = new SampleStoreDealsFeedService();
                }
            }
            else
            {
                _dataService = dataService; // use injected data service
            }
        }

        // Deals Hub top level info
        public string CollectionName { get; set; }
        public string CollectionSummary { get; set; }

        // App collection
        public ObservableCollection<AppElementViewModel> AppViewModels { get; set; }

        public async Task<AppsPageViewModel> GetHomePageViewModel()
        {
            if (_dataService != null)
            {
                var appDealsData = await _dataService.GetDealsHubCollectionFeedDataAsync();

                //Populate view model properties
                if (appDealsData != null)
                {
                    CollectionName = appDealsData.TopicName;
                    CollectionSummary = appDealsData.TopicDescription;
                    AppViewModels = new ObservableCollection<AppElementViewModel>();
                    foreach (var item in appDealsData.Items)
                    {
                        var appVM = new AppElementViewModel(item, _usePhoneFeed);
                        AppViewModels.Add(appVM);
                    }
                }
            }

            return this;
        }
    }
}
