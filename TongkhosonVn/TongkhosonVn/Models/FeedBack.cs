using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TongkhosonVn.Models
{
    public class FeedBack
    {
        [Key]
        public long ID { get; set; }
        [DisplayName("Tên khách hàng phản hồi"),UIHint("TextBox")]
        public string FullNname { get; set; }
        [DisplayName("Ảnh đại diện"), UIHint("NumberBox")]
        public string Avatar { get; set; }

        [Column(TypeName = "ntext"), UIHint("EditorBox"),Display(Name ="Nội dung")]
        [Required(ErrorMessage ="Hãy nhập nội dung")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Hãy nhập nội dung ngắn")]
        [StringLength(500), UIHint("TextArea")]
        public string Description { get; set; }

        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }

        [DisplayName("Ghi chú thêm"), UIHint("TextBox")]
        public string NoteContent { get; set; }

        [DisplayName("Hiện thị")]
        public bool Active { get; set; }

        [DisplayName("Hiện trang chủ")]
        public bool FlagHome { get; set; }
    }
}