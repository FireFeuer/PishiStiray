using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class ProfessionTable
{
    public string Name { get; set; } = null!;

    public int Number { get; set; }

    public virtual ICollection<Usertrue> Usertrues { get; } = new List<Usertrue>();
}
