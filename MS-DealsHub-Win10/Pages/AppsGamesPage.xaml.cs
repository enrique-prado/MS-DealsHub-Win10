

namespace MSDealsWin10App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppsGamesPage : Page
    {
        private const string taskName = "TileSchedulerTask";
        private const string taskEntryPoint = "LiveTileBkgTask.WinRT.TileSchedulerTask";
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public AppsGamesPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the DefaultViewModel. This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!IsInternetConnected())
            {
                //Show alert
                var noInternetDlg =
                    new MessageDialog(
                        "Deals Hub requires an internet connection to get latest deals. Please check your connection and try again later.");
                await noInternetDlg.ShowAsync();
                Application.Current.Exit();
            }
            else
            {
                var mainViewModel = new MainPageViewModel(null, false);
                var vmData = await mainViewModel.GetHomePageViewModel();
                this.DefaultViewModel["Home"] = vmData;
                this.DefaultViewModel["Items"] = vmData.AppViewModels;

                spinner.IsActive = false;

                //Create Live Tile Background task
                CreateLiveTileBackgroundTask();
            }
        }

        private bool IsInternetConnected()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool isConnected = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return isConnected;
        }

        private static async void CreateLiveTileBackgroundTask()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                result == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder
                {
                    Name = taskName,
                    TaskEntryPoint = taskEntryPoint
                };

                taskBuilder.SetTrigger(new TimeTrigger(720, false)); // 720 min = 12 hours
                var registration = taskBuilder.Register();
                Debug.WriteLine("Background Task Registered");
            }
        }

        private async void ItemGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var appElementVM = (AppElementViewModel)e.ClickedItem;
            if (!string.IsNullOrEmpty(appElementVM.AppStoreLink))
            {
                await Launcher.LaunchUriAsync(new Uri(appElementVM.AppStoreLink));
            }
        }
    }
}