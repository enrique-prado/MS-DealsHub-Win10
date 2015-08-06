using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.Annotations;
using MSDealsDataLayer.Models;
using MSDealsDataLayer.Resources;

namespace MSDealsDataLayer.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public CategoryViewModel(Category category)
        {
            _category = category;
            _albumViewModels = null;
            _imageUrl1 = category.ImageUrl;
            _imageUrl2 = category.ImageUrl;
            _imageUrl3 = category.ImageUrl;
            _imageUrl4 = category.ImageUrl;
            _carousalImageUrl = category.ImageUrl;
            _carousalAlbumViewModel = null;
            _carousalIndex = 0;
            _carousalInterval = CarousalInterval.None;
        }

        /// <summary>
        /// Constructor that "converts" AppElementViewModels to a CategoryViewModel
        /// </summary>
        /// <param name="appsViewModels"></param>
        public CategoryViewModel(IList<AppElementViewModel> appsViewModels)
        {
            AppViewModels = appsViewModels;
            _category = null;
            _albumViewModels = null;

            _title = "Music Apps"; //TODO: Put in resource file
            _rank = 10; // Some high number to guarantee is last hub
            _text = "Other cool apps for music";
            _appCount = appsViewModels.Count;
            var maxIndex = _appCount - 1;
            int i = 0;

            if (i <= maxIndex)
                _imageUrl1 = appsViewModels[i].ThumbnailUrl;
            if (++i <= maxIndex)
                _imageUrl2 = appsViewModels[i].ThumbnailUrl;
            if (++i <= maxIndex)
                _imageUrl3 = appsViewModels[i].ThumbnailUrl;
            if (++i <= maxIndex)
                _imageUrl4 = appsViewModels[i].ThumbnailUrl;

            _carousalImageUrl = null;
            _carousalAlbumViewModel = null;
            _carousalIndex = 0;
            _carousalInterval = CarousalInterval.None;
        }

        /// <summary>
        /// Update the list of AlbumViewModels
        /// </summary>
        public async Task Update(bool justForLiveTile = false)
        {
            await _category.UpdateAlbumsAsync(justForLiveTile);
            if (_albumViewModels != null && _albumViewModels.Count > 0)
            {
                var firstOne = _albumViewModels[0];
                firstOne.PropertyChanged -= AlbumPropertyChanged;
                _albumViewModels.Clear();
            }
            else
            {
                _albumViewModels = new ObservableCollection<AlbumViewModel>();
            }
            foreach (var album in _category.Albums)
            {
                var albumViewModel = new AlbumViewModel(album, _category.Title);
                _albumViewModels.Add(albumViewModel);
                switch (album.Rank)
                {
                    case 0:
                        _imageUrl1 = album.ImageUrl;
                        break;
                    case 1:
                        _imageUrl2 = album.ImageUrl;
                        break;
                    case 2:
                        _imageUrl3 = album.ImageUrl;
                        break;
                    case 3:
                        _imageUrl4 = album.ImageUrl;
                        break;
                }
            }
            if (HasAlbum())
            {
                var firstAlbum = _albumViewModels[0];
                firstAlbum.PropertyChanged += AlbumPropertyChanged;
                UpdateCarousalItem(PickCarousalAlbumIndex());
            }
            OnPropertyChanged("AlbumViewModels");
        }

        public void UpdateDealEnd()
        {
            if (HasAlbum())
            {
                _albumViewModels[0].UpdateTimeLeft();
            }
        }

        public string Title
        {
            get
            {
                if (null != _category)
                {
                    return _category.Title;
                }
                else
                {
                    return _title;
                }
            }
        }

        public string Text
        {
            get
            {
                if (null != _category) 
                    return _category.Text;

                return _text;
            }
        }

        public string ImageUrl
        {
            get { return _category.ImageUrl; }
        }

        public int Rank
        {
            get
            {
                if (null != _category)
                    return _category.Rank;

                return _rank;
            }
        }

        public string Genre
        {
            get
            {
                if (HasAlbum())
                {
                    return _albumViewModels[0].Genre;
                }
                else
                {
                    return "Unknown";
                }
            }
        }

        public string ImageUrl1
        {
            get { return _imageUrl1; }
        }

        public string ImageUrl2
        {
            get { return _imageUrl2; }
        }

        public string ImageUrl3
        {
            get { return _imageUrl3; }
        }

        public string ImageUrl4
        {
            get { return _imageUrl4; }
        }

        public string TimeLeft
        {
            get
            {
                if (HasAlbum())
                {
                    if (HasCarousalItem())
                    {
                        return _carousalAlbumViewModel.TimeLeft;
                    }

                    return _albumViewModels[0].TimeLeft;
                }
                    
                return "";
            }
        }

        public bool HasValidTimeLeft
        {
            get
            {
                if (HasAlbum())
                {
                    return _albumViewModels[0].HasValidTimeLeft;
                }
                else
                {
                    return false;
                }
            }
        }

        public int ProgressValue
        {
            get
            {
                if (HasAlbum())
                {
                    if (HasCarousalItem())
                    {
                        return _carousalAlbumViewModel.ProgressValue;
                    }

                    return _albumViewModels[0].ProgressValue;
                }
                return 0;
            }
        }

        public bool IsDealEndingSoon
        {
            get
            {
                if (HasAlbum())
                {
                    if (HasCarousalItem())
                    {
                        return _carousalAlbumViewModel.IsDealEndingSoon;
                    }
                    return _albumViewModels[0].IsDealEndingSoon;
                }
                return false;
            }
        }

        public bool IsDealEnded
        {
            get
            {
                if (HasAlbum())
                {
                    if (HasCarousalItem())
                    {
                        return _carousalAlbumViewModel.IsDealEnded;
                    }
                    return _albumViewModels[0].IsDealEnded;
                }
                return false;
            }
        }

        public bool DisplayTimeLeft
        {
            get { return _category.Rank != 3 && HasValidTimeLeft; }
        }

        public string UserMessage
        {
            get
            {
                if (_category.Rank == 3)
                {
                    return SharedResource.VerizonUserMessage ;
                }
                return "";
            }
        }

        public string MoreLabel
        {
            get
            {
                if (HasAlbum())
                {
                    if (_albumViewModels.Count > 3)
                    {
                        return "+" + (_albumViewModels.Count - 3) + SharedResource.More;
                    }
                    else
                    {
                        return SharedResource.ViewAll;
                    }
                }

                if (_appCount > 3)
                {
                    return "+" + (_appCount - 3) + SharedResource.More;
                }
                if (_appCount > 0)
                {
                    return SharedResource.ViewAll;
                }

                return "";
            }
        }

        public string MoreLabelForTwo
        {
            get
            {
                if (HasAlbum())
                {
                    if (_albumViewModels.Count > 1)
                    {
                        return "+" + (_albumViewModels.Count - 1) + SharedResource.More;
                    }
                    else
                    {
                        return SharedResource.ViewAll;
                    }
                }
                return "";
            }
        }

        public int AlbumCount
        {
            get
            {
                return HasAlbum() ? _albumViewModels.Count : 0;
            }
        }

        public ObservableCollection<AlbumViewModel> AlbumViewModels
        {
            get { return _albumViewModels; }
            set { _albumViewModels = value; }
        }

        public string CarousalImageUrl
        {
            get { return _carousalImageUrl; }
            set { _carousalImageUrl = value; }
        }

        public string CarousalTitle
        {
            get { return _carousalTitle; }
            set { _carousalTitle = value; }
        }

        public string CarousalArtist
        {
            get { return _carousalArtist; }
            set { _carousalArtist = value; }
        }

        public IList<AppElementViewModel> AppViewModels { get; set; } // used for music apps only


        public enum CarousalInterval
        {
            Daily = 0,
            Frequent =1,
            None = 2
        }

        public void SetCarousalInterval(CarousalInterval interval)
        {
            _carousalInterval = interval;
        }

        public void MoveToNextCarousalItem()
        {
            if (!HasAlbum())
                return;
            var albumIndex = _carousalIndex + 1;
            if (_albumViewModels.Count <= albumIndex)
            {
                albumIndex = 0;
            }
            UpdateCarousalItem(albumIndex);
        }

        private int PickRandomAlbumIndex()
        {
            var random = new Random();
            int randomIndex = random.Next(0, _albumViewModels.Count);
            return randomIndex;
        }

        
        private int PickCarousalAlbumIndex()
        {
            if (_carousalInterval == CarousalInterval.Daily)
            {
                var dayOfWeek = (int)DateTime.Now.DayOfWeek;
                if (dayOfWeek >= _albumViewModels.Count)
                {
                    return PickRandomAlbumIndex();
                }

                return dayOfWeek;
            }
            else if (_carousalInterval == CarousalInterval.Frequent)
            {
                return PickRandomAlbumIndex();
            }
            else
            {
                return 0;
            }
        }

        private void UpdateCarousalItem(int albumIndex)
        {
            var albumViewModel = _albumViewModels[albumIndex];
            _carousalIndex = albumIndex;
            if (_carousalAlbumViewModel != null)
            {
                _carousalAlbumViewModel.PropertyChanged -= AlbumPropertyChanged;
            }
            _carousalAlbumViewModel = albumViewModel;
            _carousalAlbumViewModel.PropertyChanged += AlbumPropertyChanged;
            _carousalImageUrl = _carousalAlbumViewModel.ImageUrl;
            _carousalTitle = _carousalAlbumViewModel.Title;
            _carousalArtist = _carousalAlbumViewModel.Artist;
            OnPropertyChanged("CarousalImageUrl");
            OnPropertyChanged("CarousalTitle");
            OnPropertyChanged("CarousalArtist");
        }

        private CarousalInterval _carousalInterval;

        private AlbumViewModel _carousalAlbumViewModel;
        private Category _category;
        private ObservableCollection<AlbumViewModel> _albumViewModels;
        private string _imageUrl1;
        private string _imageUrl2;
        private string _imageUrl3;
        private string _imageUrl4;
        private string _carousalImageUrl;
        private string _carousalTitle;
        private string _carousalArtist;
        private int _carousalIndex;
        private string _title; // used for music apps only
        private int _rank; // used for music apps only
        private string _text; // used for music apps only
        private int _appCount; // used for music apps only

        public bool HasAlbum()
        {
            return _albumViewModels != null && _albumViewModels.Count > 0;
        }

        private bool HasCarousalItem()
        {
            return _carousalAlbumViewModel != null;
        }

        private void AlbumPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.PropertyName + " has changed.");    
            OnPropertyChanged(e.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
