using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class Order1
{
    public int OrderId { get; set; }

    public string OrderDate { get; set; } = null!;

    public string DeliveryDate { get; set; } = null!;

    public int PointIssue { get; set; }

    public string? Fio { get; set; }

    public int Code { get; set; }

    public int Status { get; set; }

    public int Count { get; set; }

    public Order1(string OrderDate, string DeliveryDate, int PointIssue, string Fio, int Code, int Status, int Count)
    {
        this.OrderDate = OrderDate;
        this.DeliveryDate = DeliveryDate;
        this.PointIssue = PointIssue;
        this.Fio = Fio;
        this.Code = Code;
        this.Status = Status;
        this.Count = Count;
    }

    public virtual ICollection<Product> ProductArticleNumbers { get; } = new List<Product>();
}
