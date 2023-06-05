using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class ManufacturersTable
{
    public string Name { get; set; } = null!;

    public int Number { get; set; }

    public ManufacturersTable(string Name, int Number)
    {
        this.Name = Name;   
        this.Number = Number;
    }
}
