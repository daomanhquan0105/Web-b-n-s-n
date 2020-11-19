using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TongkhosonVn.Models
{
    public class ProductCategory
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Tên loại sản phẩm"), UIHint("TextBox")]
        [Required(ErrorMessage = "Hãy nhập tên cho loại sản phẩm")]
        [StringLength(250)]
        public string Name { get; set; }
        [DisplayName("Thứ tự hiện thị"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        [Required(ErrorMessage = "Hãy chọn thứ tự hiển thị"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }
        [DisplayName("Ngày tạo"), UIHint("DateTimePicker")]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [DisplayName("Show Menu ngang")]
        public bool ShowMenuVertical{ get; set; }
        [DisplayName("Show trang chủ")]
        public bool FlagHome { get; set; }
        [DisplayName("Danh mục cha")]
        public int? ParentProductCategoryId { get; set; }
        [StringLength(500)]
        public string Url { get; set; }
        [Display(Name = "Thẻ Title"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string MetaTitle { get; set; }
        [Display(Name = "Thẻ Description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string MetaDescription { get; set; }

        public virtual ProductCategory ParentProductCategory { get; set; }
        public virtual List<TagProductCategory> ListTagProductCategory { get; set; }
        public virtual List<Product> Products { get; set; }
        public ProductCategory()
        {
            CreateDate = DateTime.Now;
            Active = true;
            FlagHome = true;
        }
    }
}
