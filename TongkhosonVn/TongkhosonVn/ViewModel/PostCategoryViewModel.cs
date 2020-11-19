using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TongkhosonVn.Models;

namespace TongkhosonVn.ViewModel
{
    public class ListPostCategoryViewModel
    {
        public List<PostCategory> PostCategories { get; set; }
        public List<TradeMark> TradeMarks { get; set; }
    }
}