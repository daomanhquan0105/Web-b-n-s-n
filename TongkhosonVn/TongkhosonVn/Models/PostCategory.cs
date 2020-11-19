using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TongkhosonVn.Models
{
    public class PostCategory
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên danh mục"), Required(ErrorMessage = "Hãy nhập tên danh mục"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string CategoryName { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }
        [DisplayName("Hoạt động")]
        public bool Active { get; set; }
        [DisplayName("Top menu")]
        public bool ShowMenu { get; set; }
        [DisplayName("Danh mục cha")]
        public int? ParentCategoryId { get; set; }
        [Display(Name = "Loại bài viết")]
        public TypeCategory TypeCategory { get; set; }
        
        [Display(Name = "Đường dẫn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thẻ Title"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string MetaTitle { get; set; }
        [Display(Name = "Thẻ Description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string MetaDescription { get; set; }

        public virtual List<Post> Posts { get; set; }

        public virtual PostCategory ParentCategory { get; set; }

        public PostCategory()
        {
            Active = true;
        }
    }

    public enum TypeCategory
    {
        [Display(Name = "Tin tức")]
        News,
        [Display(Name = "Báo giá")]
        Quote,
        [Display(Name = "Bảng màu")]
        PaintColor,
        [Display(Name = "Dịch vụ")]
        Service,
        [Display(Name = "Chân trang")]
        Footer
    }
}