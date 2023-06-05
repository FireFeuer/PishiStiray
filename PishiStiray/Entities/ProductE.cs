using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishiStiray.Entities
{
    public class ProductE
    {
        public string ProductArticleNumber { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public int UnitOfMeasurement { get; set; }

        public string Cost { get; set; }

        public string Discount { get; set; }

        public string Manufacturer { get; set; }

        public string Provider { get; set; }

        public string ProductCategory { get; set; }

        public string QuantityStock { get; set; }

        public string Description { get; set; } = null!;

        public string Picture { get; set; } = null!;

        public int NowDiscount { get; set; }

        public int Status { get; set; }

        public string AmountCart { get; set; }

        public string?  amd_reg1 {get; set; }

        public string? manufacturer_global { get; set; }

        public int provider_number { get; set; }
    }
}
