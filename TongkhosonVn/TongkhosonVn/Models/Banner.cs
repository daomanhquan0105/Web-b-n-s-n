using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TongkhosonVn.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên banner"), Required(ErrorMessage = "Hãy nhập tên banner"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string BannerName { get; set; }

        [Display(Name = "Hình ảnh"), StringLength(500)]
        public string Image { get; set; }

        [Display(Name = "Chiều rộng"), Required(ErrorMessage = "Hãy nhập chiều rộng"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên"), UIHint("NumberBox")]
        public int Width { get; set; }

        [Display(Name = "Chiều rộng"), Required(ErrorMessage = "Hãy nhập chiều rộng"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên"), UIHint("NumberBox")]
        public int Height { get; set; }

        [Display(Name = "Vị trí quảng cáo"), Required(ErrorMessage = "Hãy chọn vị trí quảng cáo"), UIHint("GroupId")]
        public LocationEnum Location { get; set; }

        [Display(Name = "Đường dẫn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string Url { get; set; }

        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên"), UIHint("NumberBox")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
    }
    public enum LocationEnum
    {
        [Display(Name = "Slide chính (900 x 400)")]
        HeroSlide,
        [Display(Name = "Banner Left (300 x auto)")]
        LeftSide
    }
}