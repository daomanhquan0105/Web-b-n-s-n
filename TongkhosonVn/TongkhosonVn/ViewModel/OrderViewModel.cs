using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TongkhosonVn.Models;

namespace TongkhosonVn.ViewModel
{
    public class OrderViewModel
    {
        public Member Member { get; set; }
        [DisplayName("Số lượng mua"), UIHint("NumberBox"), Required(ErrorMessage = "Hãy nhập số lượng"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        public int Quantity { get; set; }
        
        public Product Product { get; set; }
        
        public long ProductID { get; set; }
    }
}