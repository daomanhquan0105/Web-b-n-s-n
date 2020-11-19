using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TongkhosonVn.Models
{
    public class TradeMark
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Tên danh mục gốc"), UIHint("TextBox")]
        [Required(ErrorMessage = "Hãy nhập tên cho danh mục gốc")]
        [StringLength(250)]
        public string Name { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [DisplayName("Ảnh đại diện/Logo")]
        [StringLength(250)]
        public string Avatar { get; set; }
        [DisplayName("Thứ tự hiện thị"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        [Required(ErrorMessage = "Hãy chọn thứ tự hiển thị"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }
        [Display(Name ="Hiển thị")]
        public bool Active { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool FlagHome { get; set; }

        [StringLength(500)]
        public string Url { get; set; }
        [Display(Name = "Thẻ Title"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string MetaTitle { get; set; }
        [Display(Name = "Thẻ Description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string MetaDescription { get; set; }

        public virtual List<TagProductCategory> ListTagProductCategory { get; set; }
        public virtual List<Product> Products { get; set; }
        public TradeMark()
        {
            Active = true;
            FlagHome = true;
        }
    }
}
