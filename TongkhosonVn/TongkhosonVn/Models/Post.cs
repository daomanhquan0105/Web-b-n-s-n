using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TongkhosonVn.Models
{ 
    public class Post
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "Tiêu đề", Description = "Tiêu đề dài tối đa 100 ký tự"), Required(ErrorMessage = "Hãy nhập tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Subject { get; set; }

        [Display(Name = "Trích dẫn ngắn"), Required(ErrorMessage = "Hãy nhập trích dẫn ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"),UIHint("TextArea")]        
        public string Description { get; set; }

        [Display(Name = "Nội dung"), UIHint("EditorBox"), Required(ErrorMessage = "Hãy nhập nội dung cho bài đăng")]
        public string Content { get; set; }

        [Display(Name = "Hình ảnh đại diện")]
        public string Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}"), UIHint("DatetimePicker")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int DisPlayOrder { get; set; }
        public int View { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        [Display(Name = "Hiện trang chủ")]
        public bool FlagHome { get; set; }
        [Display(Name = "Danh mục")]
        public int PostCategoryID { get; set; }
        [StringLength(500)]
        public string Url { get; set; }
        [Display(Name = "Thẻ Title"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string MetaTitle { get; set; }
        [Display(Name = "Thẻ Description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string MetaDescription { get; set; }

        [ForeignKey("PostCategoryID")]
        public virtual PostCategory PostCategory { get; set; }

        public Post()
        {
            CreateDate = DateTime.Now;
            View = 1;
            FlagHome = false;
            Active = true;
        }
    }
}