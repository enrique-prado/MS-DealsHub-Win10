using System.Collections.Generic;

namespace MSDealsDataLayer.FeedModels
{
    public interface IAppStoreItem
    {
        string Id { get; set; } 
        string PackageFamilyName { get; set; }
        string Language { get; set; }
        string Name { get; set; }
        double Rating { get; set; }
        int NumOfReviews { get; set; }
        AppCategory Category { get; set; }
        string CurrencySymbol { get; set; }
        double FullPrice { get; set; }
        string Price { get; set; }
        string CurrencyCode { get; set; }
        List<AppAsset> Assets { get; set; }
    }
}
