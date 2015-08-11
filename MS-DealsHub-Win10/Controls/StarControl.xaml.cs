using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MSDealsWin10App.Controls
{
    public sealed partial class StarControl : UserControl
    {
        private string _starControlVisualState;

        public StarControl()
        {
            this.InitializeComponent();
        }

        public string StarControlVisualState
        {
            get { return _starControlVisualState; }
            set {_starControlVisualState = value; }
        }

        public double StarRating
        {
            get 
            {
                return Convert.ToDouble(GetValue(StarRatingProperty), CultureInfo.InvariantCulture);
            }
            set 
            {
                SetValue(StarRatingProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for StarRating.  This enables animation, styling, binding, etc...
        private static DependencyProperty StarRatingProperty =
            DependencyProperty.Register("StarRating", typeof(double), typeof(StarControl), new PropertyMetadata(0, OnStarRatingPropertyChanged));


        private static void OnStarRatingPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            StarControl control = source as StarControl;
            control.SetStarState(control.StarRating);
        }


        private void SetStarState(double rating)
        {
            if (rating < 0.25)
            {
                StarControlVisualState = "ZeroStar";
            }
            else if (rating < 0.75)
            {
                StarControlVisualState = "HalfStar";
            }
            else if (rating < 1.25)
            {
                StarControlVisualState = "OneStar";
            }
            else if (rating < 1.75)
            {
                StarControlVisualState = "OneHalfStar";
            }
            else if (rating < 2.25)
            {
                StarControlVisualState = "TwoStar";
            }
            else if (rating < 2.75)
            {
                StarControlVisualState = "TwoHalfStar";
            }
            else if (rating < 3.25)
            {
                StarControlVisualState = "ThreeStar";
            }
            else if (rating < 3.75)
            {
                StarControlVisualState = "ThreeHalfStar";
            }
            else if (rating < 4.25)
            {
                StarControlVisualState = "FourStar";
            }
            else if (rating < 4.75)
            {
                StarControlVisualState = "FourHalfStar";
            }
            else
            {
                StarControlVisualState = "FiveStar";
            }

            VisualStateManager.GoToState(this, StarControlVisualState, false);
        }

    }
}
