using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TongkhosonVn.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name = "Tài khoản"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Required(ErrorMessage = "Hãy nhập tài khoản")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu"), StringLength(120, ErrorMessage = "Tối đa 50 ký tự"), Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string PassWord { get; set; }
    }
    public class ChangPasswordViewModel
    {
        [DisplayName("Mật khẩu cũ"), Required(ErrorMessage = "Hãy nhập mật khẩu cũ"), StringLength(60, ErrorMessage = "Tối đa 60 ký tự"), UIHint("Password")]
        public string OldPassWord { get; set; }
        [DisplayName("Mật khẩu mới"), Required(ErrorMessage = "Hãy nhập mật khẩu mới"), StringLength(60, ErrorMessage = "Tối đa 60 ký tự"), UIHint("Password")]
        public string NewPassWord { get; set; }
        [DisplayName("Xác nhận mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu xác nhận"), StringLength(60, ErrorMessage = "Tối đa 60 ký tự"), UIHint("Password")]
        public string ConfimPassword { get; set; }
    }
}