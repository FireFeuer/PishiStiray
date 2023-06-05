using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class Orderproducttrue
{
    public int Id { get; set; }

    public string? Article { get; set; }

    public string? Sum { get; set; }

    public int Key { get; set; }

    public Orderproducttrue(int Id, string Article, string Sum, int Key)
    {
        this.Id = Id;
        this.Article = Article;
        this.Sum = Sum;
        this.Key = Key;
    }
}
