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
using System.Windows.Shapes;
using PishiStiray.ViewModel;

namespace PishiStiray.Views
{
    /// <summary>
    /// Логика взаимодействия для AddManOrProvider_Window.xaml
    /// </summary>
    public partial class AddManOrProvider_Window : Page
    {
        public AddManOrProvider_Window()
        {
            InitializeComponent();
            DataContext = new AddManOrProvider_ViewModel();
        }
    }
}
