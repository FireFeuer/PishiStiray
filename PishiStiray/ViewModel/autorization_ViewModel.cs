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
using TradeContext = PishiStiray.Data.TradeContext;


namespace PishiStiray.ViewModel
{
    internal class autorization_ViewModel : TradeContext, INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessageButton { get; set; }

        public autorization_ViewModel()
        {
            Global.role = "none";
        }

        private RelayCommand signInCommand;
        public RelayCommand SignInCommand
        {
            get
            {
                return signInCommand ??
                  (signInCommand = new RelayCommand(obj =>
                  {
                      SignInService signInService = new SignInService();
                      SignIn();
                      ErrorMessageButton = "Неверный логие или пароль";
                  }));
            }

        }   
        public async Task SignIn()
        {
            SignInService signInService = new SignInService();
            await signInService.SignIn(Username, Password);
        }

        private RelayCommand signInLaterCommand;
        public RelayCommand SignInLaterCommand
        {
            get
            {
                return signInLaterCommand ??
                  (signInLaterCommand = new RelayCommand(obj =>
                  {
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/tradePage.xaml");
                      Global.name = "Гость";
                      Global.role = "user";
                  }));
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
                      Global.role_registration = "пользователь";
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/registrationPage.xaml");
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
