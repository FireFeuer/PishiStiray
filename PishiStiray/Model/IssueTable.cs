using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class IssueTable
{
    public string Issue { get; set; } = null!;

    public int NumberIssue { get; set; }

    public string? City { get; set; }

    public int? Index { get; set; }

    public int? House { get; set; }
}
