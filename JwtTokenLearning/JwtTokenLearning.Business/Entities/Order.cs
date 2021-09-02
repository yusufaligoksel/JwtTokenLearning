using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtTokenLearning.Business.Entities
{
    public class Order:BaseEntity
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
