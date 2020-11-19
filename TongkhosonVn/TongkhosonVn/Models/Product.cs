using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TongkhosonVn.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "Tên sơn"),Required(ErrorMessage ="Hãy nhập tên cho sản phẩm"),StringLength(100,ErrorMessage ="Không được vượt quá 100 ký tự"),UIHint("TextBox")]
        public string Name { get; set; }

        [DisplayName("Mã sản phẩm"), UIHint("TextBox")]
        [Required(ErrorMessage = "Hãy nhập mã cho sản phẩm")]
        [StringLength(20)]
        public string Code { get; set; }
        
        [DisplayName("Danh sách hình ảnh")]
        public string Images { get; set; }

        [DisplayName("Đơn giá"), UIHint("NumberBox"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        [Required(ErrorMessage = "Hãy nhập giá cho sản phẩm")]
        public decimal Price { get; set; }

        [DisplayName("Giá gốc"), UIHint("NumberBox"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        public decimal OriginalPrice { get; set; }

        [DisplayName("Số lượng tồn"), UIHint("NumberBox")]
        [Required(ErrorMessage = "Nhập số lượng cho sản phẩm")]
        public int Quantity { get; set; }

        [DisplayName("Miêu tả ngắn"), UIHint("TextArea")]
        [Required(ErrorMessage = "Hãy miêu tả ngắn về sản phẩm")]
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Miêu tả chi tiết"), UIHint("EditorBox"), Required(ErrorMessage = "Hãy miêu tả chi tiết cho sản phẩm")]
        public string Detail { get; set; }
        
        [DisplayName("Thời gian bảo hành"), UIHint("NumberBox"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        [Required(ErrorMessage = "Hãy nhập thời gian bảo hành cho sản phẩm")]
        public int Warranty { get; set; }

        [DisplayName("Ngày tạo"), UIHint("DateTimePicker")]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Thứ tự hiển thị"), UIHint("NumberBox"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        [Required(ErrorMessage = "Hãy nhập thứ tự hiện thị cho sản phẩm")]
        public int DisplayOrder { get; set; } 

        [DisplayName("Hiển thị")]
        public bool Active { get; set; }

        [DisplayName("Hiện trang chủ")]
        public bool FlagHome { get; set; }
        public int View { get; set; }
        [Display(Name ="Danh mục sản phẩm")]
        public int ProductCategoryID { get; set; }
        [Display(Name ="Nhãn hiệu sơn")]
        public int? TradeMarkID { get; set; }

        [StringLength(500)]
        public string Url { get; set; }
        [Display(Name = "Thẻ Title"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string MetaTitle { get; set; }
        [Display(Name = "Thẻ Description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string MetaDescription { get; set; }


        [ForeignKey("ProductCategoryID")]
        public virtual ProductCategory ProductCategory { get; set; }

        [ForeignKey("TradeMarkID")]
        public virtual TradeMark TradeMark { get; set; }
        public Product()
        {
            CreateDate = DateTime.Now;
            Active = true;
            FlagHome = true;
            View = 0;
        }
    }
}
