using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class SuppliersTable
{
    public string Name { get; set; } = null!;

    public int Number { get; set; }

    public SuppliersTable(string Name) 
    {
        this.Name = Name;
    }
}
