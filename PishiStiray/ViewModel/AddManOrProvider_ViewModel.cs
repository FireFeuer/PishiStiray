using PishiStiray.Data;
using PishiStiray.Model;
using PishiStiray.Servieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PishiStiray.ViewModel
{
    public class AddManOrProvider_ViewModel : INotifyPropertyChanged
    {
        private int count_man;
        private string manufacturer_txt;
        public string Manufacturer_txt 
        {
            get
            {
                return manufacturer_txt;
            }
            set
            {
                manufacturer_txt = value;
                NotifyPropertyChanged("Manufacturer_txt");
            }
        }
        private string manufacturer;
        public string Manufacturer
        {
            get
            {
                return manufacturer;
            }
            set
            {
              
                manufacturer = value;
                manufacturer_txt = manufacturer;
                NotifyPropertyChanged("Manufacturer_txt");
            }
        }
         
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.ManufacturersTable> manufacturersDB;
        public ObservableCollection<String> manufacturers { get; set; }


        public AddManOrProvider_ViewModel()
        {
            manufacturers = new ObservableCollection<String>();
            using (TradeContext db = new TradeContext())
            {
                manufacturersDB = db.ManufacturersTables;
                foreach (ManufacturersTable mnf in manufacturersDB)
                {
                    manufacturers.Add(mnf.Name);
                   
                }
                count_man = manufacturers.Count;
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      if(manufacturer_txt != null)
                      {
                          if (manufacturer_txt.Replace(" ", "") != "")
                          {
                              using (TradeContext db = new TradeContext())
                              {
                                  bool check = true;
                                  int num = 0;
                                  var manufacturersDB = db.ManufacturersTables;
                                  foreach (var mnf in manufacturersDB)
                                  {
                                      if (mnf.Name == manufacturer_txt)
                                      {
                                          check = false;
                                          MessageBox.Show("Данный производитель уже существует");
                                          break;

                                      }
                                  }
                                  if (check == true)
                                  {
                                      ManufacturersTable manufacturersTable = new ManufacturersTable(manufacturer_txt, count_man + 1);
                                      manufacturersDB.Add(manufacturersTable);
                                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                                      navigateViewModel.Navigate("Views/tradePage.xaml");
                                  }
                                  db.SaveChanges();
                              }
                          }
                          else
                          {
                              MessageBox.Show("Все поля должны быть заполнены");
                          }
                      }
                      else
                      {
                          MessageBox.Show("Все поля должны быть заполнены");
                      }


                  }));
            }
        }

        private RelayCommand closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ??
                  (closeCommand = new RelayCommand(obj =>
                  {
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/tradePage.xaml");
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string props)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(props));
        }
    }
}
