using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TongkhosonVn.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Tài khoản"),StringLength(50, ErrorMessage ="Tối đa 50 ký tự"),Required(ErrorMessage ="Hãy nhập tài khoản")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu"), StringLength(120, ErrorMessage = "Tối đa 50 ký tự"), Required(ErrorMessage = "Hãy nhập mật khẩu"),UIHint("Password")]
        public string PassWord { get; set; }
        public bool Active { get; set; }
        public UserRole()
        {
            Active = true; 
        }
    } 
}
