using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PishiStiray.Model;
using PishiStiray.Servieces;
using PishiStiray.ViewModel;
using TradeContext = PishiStiray.Data.TradeContext;

namespace PishiStiray.Servieces
{
    public class SignInService
    {
        public async         Task
SignIn(string Username, string Password)
        {
            using (TradeContext db = new TradeContext())
            {
                var users = db.Usertrues;
                foreach (Usertrue user in users)
                {
                    if (user.Login == Username && user.Password == Password)
                    {
                        NavigateViewModel navigateViewModel = new NavigateViewModel();
                        navigateViewModel.Navigate("Views/tradePage.xaml");
                        Global.name = user.Name + " " + user.Surname;
                        if (user.Role == 1)
                        {
                            Global.role = "user";
                        }
                        if (user.Role == 2)
                        {
                            Global.role = "manager";
                        }
                        if (user.Role == 3)
                        {
                            Global.role = "admin";
                        }
                    }
                  
                }
            }
            await Task.Delay(0);           
        }
    }
}
