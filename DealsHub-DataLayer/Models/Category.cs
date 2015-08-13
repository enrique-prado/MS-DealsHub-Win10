using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDealsDataLayer.Models
{
    public class Category
    {
        public int Rank { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string ActionTarget { get; set; }
        public string ImageId { get; set; }

        public bool IsAlbumUpdated
        {
            get { return _albumUpdated; }
        }

        public List<Album> Albums
        {
            get
            {
                return _albums;
            }
            set { _albums = value; }
        }

        private List<Album> _albums; 
        public async Task UpdateAlbumsAsync(bool justForLiveTile = false)
        {
            _albums = await DataLayer.GetAlbumsAsync(ActionTarget);
            if (_albums.Count > 0)
            {
                await _albums[0].UpdateDeal();
                if (!justForLiveTile)
                {
                    // Update prices of the rest of albums.
                    var priceUpdateTask = new Task(UpdateAlbumDeals);
                    priceUpdateTask.Start();
                }
                else
                {
                    _albumUpdated = true;
                }
            }

        }

        private async void UpdateAlbumDeals()
        {

            for (int i = 1; i < _albums.Count; i++)
            {
                var album = _albums[i];
                album.UpdateDeal(); // don't wait for non-first album.
            }
            _albumUpdated = true;
        }

        private bool _albumUpdated = false;
    }
}
