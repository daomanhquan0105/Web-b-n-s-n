using TongkhosonVn.Models;

namespace TongkhosonVn.ViewModel
{
    public class ProductViewModel
    {
        public PagedList.IPagedList<Product> Products { get; set; }
        public TradeMark TradeMark { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}