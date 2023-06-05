using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishiStiray.Entities
{
    public class Order1E
    {
        public int OrderId { get; set; }

        public string OrderDate { get; set; } = null!;

        public string DeliveryDate { get; set; } = null!;

        public string PointIssue { get; set; } = null!;

        public string? Fio { get; set; }

        public int Code { get; set; }

        public int Count { get; set; }

        public string Status { get; set; } = null!;

        public Order1E(int OrderId, string OrderDate, string DeliveryDate, string PointIssue, string Fio, int Code, string Status, int Count)
        {
            this.OrderId = OrderId;
            this.OrderDate = OrderDate;
            this.DeliveryDate = DeliveryDate;
            this.PointIssue = PointIssue;
            this.Fio = Fio;
            this.Code = Code;
            this.Status = Status;
            this.Count = Count;
        }

    }

  

}
