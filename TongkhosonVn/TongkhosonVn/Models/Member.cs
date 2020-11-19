using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TongkhosonVn.Models
{
    public class Member
    {
        [Key]
        public long Id { get; set; }  

        [StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), Display(Name = "Họ và tên"), Required(ErrorMessage = "Hãy nhập Họ và tên"), UIHint("TextBox")]
        public string FullName { get; set; }

        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), EmailAddress(ErrorMessage = "Email không chính xác"), UIHint("TextBox")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), Display(Name = "Địa chỉ"),UIHint("TextBox")]
        public string Address { get; set; }

        [RegularExpression(@"0\d{7,11}", ErrorMessage = "Chỉ nhập số, bắt đầu bằng số 0, dài từ 7 đến 11 ký tự"),Display(Name = "Điện thoại"), UIHint("TextBox")]
        public string Phone { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreateDate { get; set; } 

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        public virtual List<Order> Orders { get; set; }
        public Member()
        {
            CreateDate = DateTime.Now;
            Active = true;
        }
    }
}