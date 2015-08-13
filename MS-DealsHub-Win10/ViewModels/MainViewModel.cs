using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.Annotations;
using MSDealsDataLayer.FeedModels;
using MSDealsDataLayer.Models;
using MSDealsDataLayer.Services;

namespace MSDealsDataLayer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel(IMusicDealFeedService feedService, bool usePhoneFeed)
        {
            _usePhoneFeed = usePhoneFeed;
            _categoryViewModels = null;
            _feedService = feedService;
            _isDataLoaded = false;
            _mainImage = "";
            _appStoreService = new AppStoreDealsFeedService();
        }

        public bool IsUpdateRequested { get; set; }

        /// <summary>
        /// Update data properties in the view model by syncing with the server data.
        /// </summary>
       public async Task Update(bool justForLiveTile = false)
        {
            IsUpdateRequested = true;
            _gotResponseEver = false;
            _isDataLoadedPartially = false;
            Debug.WriteLine("!!! Updating feed data: MainVM.Update");
            // Set up the Feed Service.
            if (_feedService != null)
            {
                DataLayer.SetFeedService(_feedService);
            }
            else
            {
                DataLayer.SetDefaultFeedService();
            }
            var categoryModels = await DataLayer.GetCategoriesAsync();
            var appDealsCategory = await GetAppDealsCategory();
            var categoryVMs = new ObservableCollection<CategoryViewModel>();
            if (categoryModels.Count > 0)
            {
                _gotResponseEver = true;
                Debug.WriteLine("Got response ever");
            }
            try
            {
                for (int i = 0; i < categoryModels.Count; i++)
                {
                    var category = categoryModels[i];
                    var categoryViewModel = new CategoryViewModel(category);
                    //
                    // TODO:Carousal feature is disabled in V1. Enable if we want the carousal functionality.
                    //
                    //if (i == 0)
                    //{
                    //    // Only the first module, we want the carousal

                    //    categoryViewModel.SetCarousalInterval(CategoryViewModel.CarousalInterval.Daily);
                    //}
                    await categoryViewModel.Update(justForLiveTile);
                    if (categoryViewModel.HasAlbum())
                    {
                        categoryViewModel.PropertyChanged += CategoryPropertyChanged;
                        categoryVMs.Add(categoryViewModel);
                    }

                }
            }
            catch (DataLayerFeedException ex)
            {
                // Notify UI that this is partial data.
                _isDataLoadedPartially = true;
            }

            _mainImage = categoryModels[0].ImageUrl;
            // Clean up old Category VMs and event handlers.
            if (_categoryViewModels != null && _categoryViewModels.Count > 0)
            {
                foreach (var categoryViewModel in _categoryViewModels)
                {
                    categoryViewModel.PropertyChanged -= CategoryPropertyChanged;
                }
                _categoryViewModels.Clear();
            }
            //Append Music Apps category to collection
            if (null != appDealsCategory) categoryVMs.Add(appDealsCategory);

            _categoryViewModels = categoryVMs;
            _isDataLoaded = true;
            _isDataLoadedPartially = false;
            Debug.WriteLine("Main VM - Categories are updated");
            OnPropertyChanged("CategoryViewModels");
            OnPropertyChanged("IsDataLoaded");
        }

       private async Task<CategoryViewModel> GetAppDealsCategory()
       {
           var appDealsData = await _appStoreService.GetAppStoreDealsFeedDataAsync();

           //Populate view model properties
           if (appDealsData != null)
           {
               var appViewModels = new ObservableCollection<AppElementViewModel>();
               foreach (var item in appDealsData.Items)
               {
                   var appVM = new AppElementViewModel(item, _usePhoneFeed);
                   appViewModels.Add(appVM);
               }

               var categoryVM = new CategoryViewModel(appViewModels);
               return categoryVM;
           }

           return null;
       }

        // Display whatever we have now, even though that's not all data.
        public void DisplayAvailableData()
        {
            _isDataLoaded = true;
            _isDataLoadedPartially = true;
            OnPropertyChanged("IsDataLoaded");
        }

       private void CategoryPropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           Debug.WriteLine(e.PropertyName + " has changed from Category.");
           OnPropertyChanged(e.PropertyName);
       }

        public void UpdateDealEnd()
        {
            if (_categoryViewModels != null && _categoryViewModels.Count > 0)
            {
                foreach (var categoryViewModel in _categoryViewModels)
                {
                    categoryViewModel.UpdateDealEnd();
                }
            }
        }

       public ObservableCollection<CategoryViewModel> CategoryViewModels
        {
           get
           {
               return _categoryViewModels;
           }
        }

        public bool IsDataLoaded
        {
            get
            {
                return _isDataLoaded;
            }
        }

        public bool IsDataLoadedPartially
        {
            get { return _isDataLoadedPartially; }
        }

        public bool GotResponseEver
        {
            get { return _gotResponseEver; }
            set { _gotResponseEver = value; }
        }

        public void LoadData()
        {
            Update();
        }

        public string MainImage
        {
            get { return _mainImage; }
            set { _mainImage = value; }
        }

        public void UpdateCarousalItem(int[] categoryIndices)
        {
            foreach (var categoryIndex in categoryIndices)
            {
                if (categoryIndex < _categoryViewModels.Count)
                {
                    _categoryViewModels[categoryIndex].MoveToNextCarousalItem();
                }
            }
        }

        public string SponsorLinkImagePhone
        {
            get { return _sponsorLinkImagePhone; }
        }

        public string SponsorLinkTargetPhone
        {
            get { return _sponsorLinkTargetPhone; }
        }

        public string SponsorLinkImageWindows
        {
            get { return _sponsorLinkImageWindows; }
        }

        public string SponsorLinkTargetWindows
        {
            get { return _sponsorLinkTargetWindows; }
        }

        private ObservableCollection<CategoryViewModel> _categoryViewModels;
        private IMusicDealFeedService _feedService;
        private IAppStoreDealsFeedService _appStoreService = null;
        private bool _usePhoneFeed;
        private bool _isDataLoaded;
        private string _mainImage;
        private bool _gotResponseEver;
        private bool _isDataLoadedPartially;
        private string _sponsorLinkImagePhone;
        private string _sponsorLinkTargetPhone;
        private string _sponsorLinkImageWindows;
        private string _sponsorLinkTargetWindows;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
