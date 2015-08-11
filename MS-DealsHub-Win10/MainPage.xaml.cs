using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MSDealsWin10App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public static Frame RootFrame = null;

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            RootFrame = rootFrame;
        }

        private ObservableCollection<NavLink> _navLinks = new ObservableCollection<NavLink>()
            {
                new NavLink() { Label = "Featured", Symbol = Windows.UI.Xaml.Controls.Symbol.Favorite  },
                new NavLink() { Label = "Apps & Games", Symbol = Windows.UI.Xaml.Controls.Symbol.AllApps },
                new NavLink() { Label = "Games", Symbol = Windows.UI.Xaml.Controls.Symbol.People },
                new NavLink() { Label = "Music", Symbol = Windows.UI.Xaml.Controls.Symbol.MusicInfo },
                new NavLink() { Label = "TV & Movies", Symbol = Windows.UI.Xaml.Controls.Symbol.Video }
            };

        public ObservableCollection<NavLink> NavLinks
        {
            get { return _navLinks; }
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var navLabel = (e.ClickedItem as NavLink).Label;
            //content.Text =  navLabel + " Page";

            if (navLabel.Contains("Apps"))
            {
                this.rootFrame.Navigate(typeof(AppsGamesPage));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

    }

    public class NavLink
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
    }
}
