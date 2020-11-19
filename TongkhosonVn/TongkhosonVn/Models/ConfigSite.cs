using System.ComponentModel.DataAnnotations;

namespace TongkhosonVn.Models
{
    public class ConfigSite
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Thẻ title"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Title { get; set; }
        [Display(Name = "Thẻ description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Mã nhúng Header"), UIHint("TextArea")]
        public string HeaderCode { get; set; }
        [Display(Name = "Mã nhúng Footer"), UIHint("TextArea")]
        public string FooterCode { get; set; }

        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Facebook"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Facebook { get; set; } 

        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Youtube"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Youtube { get; set; }

        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Instagram"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Instagram { get; set; }

        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Telegram"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Twitter { get; set; }

        [Display(Name = "Mã nhúng Google maps"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string GoogleMaps { get; set; }

        [Display(Name = "Địa chỉ"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại"), StringLength(15, ErrorMessage = "Tối đa 15s ký tự"), UIHint("TextBox")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Email không hợp lệ"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Email { get; set; }

        [Display(Name = "Hình ảnh"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự")]
        public string LogoTop { get; set; }

        [Display(Name = "Hình ảnh"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự")]
        public string LogoBottom { get; set; }

        [Display(Name = "Hướng dẫn thanh toán"), UIHint("EditorBox")]
        public string Payment { get; set; }

        [Display(Name = "Liên hệ"),  UIHint("EditorBox")]
        public string Contact { get; set; }

        [Display(Name = "Giúp đỡ và tư vấn"), UIHint("EditorBox")]
        public string Helper { get; set; }

        [Display(Name = "Vận chuyển và trả hàng"), UIHint("EditorBox")]
        public string  Transport { get; set; }

        [Display(Name = "Báo giá sơn"),  UIHint("EditorBox")]
        public string PricePaint { get; set; }

        [Display(Name = "Chính sách hoàn tiền"),UIHint("EditorBox")]
        public string RefundPolicy { get; set; } 

        [Display(Name = "Nội dung chân trang"), UIHint("TextBox")]
        public string CopyRight { get; set; }

        public bool Active { get; set; }
    }
}