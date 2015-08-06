using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSDealsDataLayer.Resources;

namespace MSDealsDataLayer.Models
{
    public class Album
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string SubGenre { get; set; }
        public string Label { get; set; }
        public bool IsExplicit { get; set; }
        public string Duration { get; set; }
        public string ImageUrl { get; set; }
        public string ItemId { get; set; }
        public string LongDescription { get; set; }
        public int Rank { get; set; }
        public string BingId { get; set; }
        public int ReleasedYear { get; set; }

        /* for Deal */

        public string Price
        {
            get { return _deal != null ? _deal.Price : SharedResource.UnknownPrice; }
        }

        public DateTime OfferStartDate
        {
            get { return _deal != null ? _deal.OfferStartDate : new DateTime(); }
            
        }

        public DateTime OfferEndDate
        {
            get { return _deal != null ? _deal.OfferEndDate : new DateTime(); }
        }

        private Deal _deal;
        private bool _isDealUpdated;

        public async Task UpdateDeal()
        {
            if (_isDealUpdated)
            {
                return;
            }
            try
            {
                _deal = await DataLayer.GetDealAsync(BingId);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine("Gettting Deal Exception: " + ex.Message);
            }
            if (_deal != null)
            {
                _isDealUpdated = true;
            }
        }

        public Album()
        {
            _isDealUpdated = false;
            _deal = null;
        }
    }
}
