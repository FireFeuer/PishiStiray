using PishiStiray.Data;
using PishiStiray.Model;
using PishiStiray.Servieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PishiStiray.ViewModel
{
    public class registration_ViewModel
    {
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public string registration_txt { get; set; }

        public string enterOrCancel_Command { get; set; }

        public registration_ViewModel() 
        { 
             if(Global.role == "none" || Global.role == "user" || Global.role == "manager")
             { 
                registration_txt = "Зарегистрироваться";
                enterOrCancel_Command = "Или войти";
             }
             if(Global.role == "admin")
            {
                enterOrCancel_Command = "Отмена";
                registration_txt = "Зарегистрировать";
            }
        }

        private RelayCommand registrationCommand;
        public RelayCommand RegistrationCommand
        {
            get
            {
                return registrationCommand ??
                  (registrationCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (Surname != null && Patronymic != null && Login != null && Password != null && Username != null)
                          {
                              if (Username.Replace(" ", "") != "" && Surname.Replace(" ", "") != "" && Patronymic.Replace(" ", "") != "" && Login.Replace(" ", "") != "" && Password.Replace(" ", "") != "")
                              {
                                  using (TradeContext db = new TradeContext())
                                  {
                                      var users = db.Usertrues;

                                      if (Global.role_registration == "пользователь")
                                      {
                                          Usertrue user = new Usertrue(Surname, Username, Patronymic, Login, Password, 1);
                                          users.Add(user);
                                      }
                                      if (Global.role_registration == "менеджер")
                                      {
                                          Usertrue user = new Usertrue(Surname, Username, Patronymic, Login, Password, 2);
                                          users.Add(user);
                                      }
                                      if (Global.role_registration == "администратор")
                                      {
                                          Usertrue user = new Usertrue(Surname, Username, Patronymic, Login, Password, 3);
                                          users.Add(user);
                                      }
                                      db.SaveChanges();
                                  }
                                  NavigateViewModel navigateViewModel = new NavigateViewModel();
                                  navigateViewModel.Navigate("Views/autorizationPage.xaml");
                              }
                              else
                              {
                                  MessageBox.Show("Все данные должны быть заполнены");
                              }
                          }
                          else
                          {
                              MessageBox.Show("Все данные должны быть заполнены");
                          }

                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show("Пользователь с данным логином уже зарегистрирован");
                      }



                  }));
            }
        }

        private RelayCommand signInCommand;
        public RelayCommand SignInCommand
        {
            get
            {
                return signInCommand ??
                  (signInCommand = new RelayCommand(obj =>
                  {
                      if(Global.role == "none" || Global.role == "user" || Global.role == "manager")
                      {
                          NavigateViewModel navigateViewModel = new NavigateViewModel();
                          navigateViewModel.Navigate("Views/autorizationPage.xaml");
                      }
                      if (Global.role == "admin")
                      {
                          NavigateViewModel navigateViewModel = new NavigateViewModel();
                          navigateViewModel.Navigate("Views/tradePage.xaml");
                      }
                  }));
            }
        }
    }
}
