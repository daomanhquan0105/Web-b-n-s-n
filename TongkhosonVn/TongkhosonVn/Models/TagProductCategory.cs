using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TongkhosonVn.Models
{
    public class TagProductCategory
    {
        [Key]
        [Column(Order =0)]
        public int TradeMarkID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ProductCategoryID { get; set; }
        public string TagName { get; set; }
        [DisplayName("Thứ tự hiện thị"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }
        public bool Active { get; set; }

        [ForeignKey("TradeMarkID")]
        public virtual TradeMark TradeMark { get; set; }

        [ForeignKey("ProductCategoryID")]
        public virtual ProductCategory ProductCategory { get; set; }
        public TagProductCategory()
        {
            DisplayOrder = 1;
            Active = true;
        }
    }
}