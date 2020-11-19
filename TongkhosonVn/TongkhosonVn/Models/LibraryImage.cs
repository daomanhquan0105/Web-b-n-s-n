using System.ComponentModel.DataAnnotations;

namespace TongkhosonVn.Models
{
    public class LibraryImage
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), Display(Name = "Tên album"), Required(ErrorMessage = "Bạn chưa nhập tên album"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Avatar { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }
        [Display(Name ="Hiển thị")]
        public bool Active { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool FlagHome { get; set; }
    }
}