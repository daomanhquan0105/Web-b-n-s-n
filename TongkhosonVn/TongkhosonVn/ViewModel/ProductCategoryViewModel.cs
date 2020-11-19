using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TongkhosonVn.Models;

namespace TongkhosonVn.ViewModel
{
    public class ListProductCategoryViewModel
    {
        public List<ProductCategory> productCategories { get; set; }
        public List<TagProductCategory> tagProductCategories { get; set; }
    }
}