using System.Collections.Generic;
using TongkhosonVn.Models;

namespace TongkhosonVn.ViewModel
{
    public class HomeViewModel
    {
        public List<TradeMark> TradeMarks { get; set; }
        public List<Banner> Banners { get; set; }
        public LibraryImage LibraryImage { get; set; }
        public List<FeedBack> FeedBacks { get; set; }
    }

    public class HeaderViewModel
    {
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<PostCategory> PostCategories { get; set; }
    }

    public class AlumbDetailViewModel
    {
        public LibraryImage Album { get; set; }
        public IEnumerable<LibraryImage> Albums { get; set; }
    }
}