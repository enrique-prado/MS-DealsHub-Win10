using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MSDealsDataLayer.FeedModels
{
    public class DiscoveryContentList
    {
        public string CreationDate { get; set; }
        public string Title { get; set; }

        public DiscoveryItem[] Items { get; set; }
    }

    public class DiscoveryItem
    {
        public string ItemType { get; set; }
        public string ItemId { get; set; }
        public int Rank { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }
        public string ActionTarget { get; set; }
        public bool IsExplicit { get; set; }
        public string ImageId { get; set; }

        /* for Albums */
        public string AlbumType { get; set; }
        public string BingId { get; set; }
        public DiscoveryItem[] Contributors { get; set; }
        public string Duration { get; set; }
        public string Genre { get; set; }
        public string Label { get; set; }
        public string ReleaseDate { get; set; }
        public string SubGenre { get; set; }
        public int TrackCount { get; set; }

    }

    public static class DiscovertyItemConst
    {
        public static string DiscoveryItemFlexHub = "FlexHub";
        public static string DiscoveryItemAlbum = "Album";
        public static string DiscoveryItemArtist = "Artist";
    }
}
