using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MS_DealsHub_Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<NavLink> _navLinks = new ObservableCollection<NavLink>()
            {
                new NavLink() { Label = "Featured", Symbol = Windows.UI.Xaml.Controls.Symbol.Favorite  },
                new NavLink() { Label = "Apps", Symbol = Windows.UI.Xaml.Controls.Symbol.AllApps },
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
            content.Text = (e.ClickedItem as NavLink).Label + " Page";
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
