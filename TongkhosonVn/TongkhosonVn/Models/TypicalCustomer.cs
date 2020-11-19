using System.ComponentModel.DataAnnotations;

namespace TongkhosonVn.Models
{
    public class TypicalCustomer
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Họ và tên"), StringLength(100, ErrorMessage = "Không được vượt quá 100 ký tự"), UIHint("TextBox")]
        public string FullName { get; set; }
        [Display(Name = "Hình ảnh"),StringLength(200,ErrorMessage ="Không được vượt quá 200 ký tự")]
        public string Avatar { get; set; }
        [Display(Name = "Miêu tả ngắn"), StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự"),UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
    }
}