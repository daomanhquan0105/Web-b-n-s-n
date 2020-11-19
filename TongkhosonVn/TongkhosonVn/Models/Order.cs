using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TongkhosonVn.Models
{
    public class Order
    {
        public long ID { get; set; }
        public long MemberID { get; set; }
        public long ProductID { get; set; }

        [DisplayName("Ngày đặt")]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; } 

        [DisplayName("Thành tiền")]
        public decimal Price { get; set; }

        [DisplayName("Số lượng mua"),UIHint("NumberBox"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương")]
        public int Quantity { get; set; }

        [DisplayName("Ngày giao")]
        [Column(TypeName = "date"), UIHint("DateTimePicker")]
        public DateTime ShipDate { get; set; }

        [DisplayName("Tình trang?")]
        public bool Status { get; set; }
        [DisplayName("Mã đơn hàng"),StringLength(50,ErrorMessage ="Tối đa 50 ký tự")]
        public string OrderCode { get; set; }
        public bool Remove { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Member { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        public Order()
        {
            CreateDate = DateTime.Now;
            ShipDate = DateTime.Now.AddDays(5);
        }
    }
}
