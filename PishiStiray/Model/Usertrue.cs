using System;
using System.Collections.Generic;

namespace PishiStiray.Model;

public partial class Usertrue
{
    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }

    public Usertrue(string Surname, string Name, string Patronymic, string Login, string Password, int Role)
    {
        this.Surname = Surname;
        this.Name = Name;
        this.Patronymic = Patronymic;
        this.Login = Login;
        this.Password = Password;
        this.Role = Role;
    }

    public virtual ProfessionTable RoleNavigation { get; set; } = null!;
}
