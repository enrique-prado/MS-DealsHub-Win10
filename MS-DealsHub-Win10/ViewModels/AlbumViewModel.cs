using System;
using System.Collections.Generic;
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
    [SecuritySafeCritical]
    public class AlbumViewModel : INotifyPropertyChanged
    {
        [SecuritySafeCritical]
        public AlbumViewModel(Album album, string categoryTitle)
        {
            _album = album;
            _categoryTitle = categoryTitle;
            _progressValue = 0;
            _timeLeftString = "";
        }
        
        public string Title
        {
            get { return _album.Title; }
        }

        public string Artist
        {
            get { return _album.Artist; }
        }

        public string ImageUrl
        {
            get { return _album.ImageUrl; }
        }

        public bool IsExplicit
        {
            get { return _album.IsExplicit; }
        }

        public string Genre
        {
            get { return _album.Genre; }
        }

        public int Rank
        {
            get { return _album.Rank; }
        }

        public string CategoryTitle
        {
            get { return _categoryTitle; }
        }

        public string Label
        {
            get { return _album.Label; }
        }

        public int ReleaseYear
        {
            get { return _album.ReleasedYear; }
        }

        public string Price
        {
            get { return _album.Price; }
        }

        public string TimeLeft
        {
            get
            {
                UpdateTimeLeft();
                return _timeLeftString;
            }
        }

        public bool HasValidTimeLeft
        {
            get { return _hasValidTimeLeft; }
        }

        [SecuritySafeCritical]
        private void UpdateTimeLeftString()
        {
            string _prevTimeLeftStr = _timeLeftString;
            if (_timeLeft.TotalDays > 1)
            {
                // Display days.
                _timeLeftString = (int)_timeLeft.TotalDays + SharedResource.Days;
            } else if (_timeLeft.TotalSeconds < 0)
            {
                if (!_isDealEnded)
                {
                    _isDealEnded = true;
                    OnPropertyChanged("IsDealEnded");
                }
                _timeLeftString = SharedResource.DealEnded;
            }
            else
            {
                // Display hours and minutes
                _timeLeftString = (int) _timeLeft.TotalHours + SharedResource.Hours +
                                  (int) (_timeLeft.TotalMinutes - ((int) _timeLeft.TotalHours*60)) +
                                  SharedResource.Minutes;
            }
            if (!_prevTimeLeftStr.Equals(_timeLeftString))
            {
                OnPropertyChanged("TimeLeft");
            }
        }

        public int ProgressValue
        {
            get
            {
               return _progressValue;
            }

        }

        public bool IsDealEndingSoon
        {
            get { return _isDealEndingSoon; }
        }

        public bool IsDealEnded
        {
            get { return _isDealEnded; }
        }

        public string BingId
        {
            get { return _album.BingId; }
        }

        public string ItemId
        {
            get { return _album.ItemId; }
        }

        public string UpsellLink
        {
            get { return GetRedirectLink(); }
        }
        [SecuritySafeCritical]
        private string GetRedirectLink()
        {
            var buyUrl = "http://music.xbox.com/Album/";
            var queryStr = "?action=buy&target=app";
            return buyUrl + ItemId + queryStr;
        }

        public bool IsFree
        {
            get { return _album.Price.StartsWith("$0.00"); }
        }

        public DateTime OfferEndDateTime
        {
            get { return _album.OfferEndDate; }
        }

        [SecuritySafeCritical]
        public void UpdateTimeLeft()
        {
            var currentTime = DateTime.Now;
            _timeLeft = (_album.OfferEndDate - currentTime) + TimeSpan.FromHours(1);
            _totalTime = _album.OfferEndDate - _album.OfferStartDate;
            var prevProgressValue = _progressValue;
            if (_totalTime.TotalDays > 0 && _timeLeft.TotalDays > 0)
            {
                _progressValue = (int)((_timeLeft.TotalDays / _totalTime.TotalDays) * 100);
            }
            else if (_totalTime.TotalHours > 0 && _timeLeft.TotalHours > 0)
            {
                _progressValue = (int)((_timeLeft.TotalHours/_totalTime.TotalHours) * 100);
            }
            else
            {
                _progressValue = 0;
            }
            if (prevProgressValue != _progressValue)
            {
                OnPropertyChanged("ProgressValue");
            }

            if (_progressValue < 10 && !_isDealEndingSoon)
            {
                _isDealEndingSoon = true;
                OnPropertyChanged("IsDealEndingSoon");
            }
            if (_totalTime.TotalDays > 365)
            {
                _hasValidTimeLeft = false;
                OnPropertyChanged("HasValidTimeLeft");
            }
            UpdateTimeLeftString();

        }

        private TimeSpan _timeLeft;
        private TimeSpan _totalTime;
        private Album _album;
        private string _categoryTitle;
        private bool _isDealEndingSoon;
        private bool _isDealEnded;
        private int _progressValue;
        private string _timeLeftString;
        private bool _hasValidTimeLeft = true;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
