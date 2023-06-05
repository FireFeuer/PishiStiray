using DevExpress.Mvvm;
using PishiStiray.Servieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PishiStiray
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationSetup();
            NavClass.mainFrame = mainFrame;
            mainFrame.NavigationService.Navigate(new Uri("/views/autorizationPage.xaml", UriKind.Relative));
        }
        void NavigationSetup()
        {
            Messenger.Default.Register<NavigateArgs>(this, (x) =>
            {
                mainFrame.Navigate(new Uri(x.Url, UriKind.Relative));
            });
        }
    }
}
