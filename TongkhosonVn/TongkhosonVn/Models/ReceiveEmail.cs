using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TongkhosonVn.Models
{
    public class ReceiveEmail
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Hãy nhập email của bạn")]
        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Email không hợp lệ"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string Email { get; set; }
    }
}