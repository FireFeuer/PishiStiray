using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class Product
{
    public string ProductArticleNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int UnitOfMeasurement { get; set; }

    public int Cost { get; set; }

    public string Discount { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public int Provider { get; set; }

    public int ProductCategory { get; set; }

    public int QuantityStock { get; set; }

    public string Description { get; set; } = null!;

    public string Picture { get; set; } = null!;

    public int NowDiscount { get; set; }

    public int Status { get; set; }

    public Product(string ProductArticleNumber, string ProductName, int UnitOfMeasurement, int Cost, string Discount, string Manufacturer, int Provider, 
        int ProductCategory, int QuantityStock, string Description, string Picture, int NowDiscount, int Status)
    {
        this.ProductArticleNumber = ProductArticleNumber;
        this.ProductName = ProductName;
        this.UnitOfMeasurement = UnitOfMeasurement;
        this.Cost = Cost;
        this.Discount = Discount;
        this.Manufacturer = Manufacturer;
        this.Provider = Provider;
        this.ProductCategory = ProductCategory;
        this.QuantityStock = QuantityStock;
        this.Description = Description;
        this.Picture = Picture;
        this.NowDiscount = NowDiscount;
        this.Status = Status;

    }

    public virtual ICollection<Order1> Orders { get; } = new List<Order1>();
}
