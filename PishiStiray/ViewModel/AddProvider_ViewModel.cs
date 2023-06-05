using PishiStiray.Data;
using PishiStiray.Model;
using PishiStiray.Servieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PishiStiray.ViewModel
{
    public class AddProvider_ViewModel : INotifyPropertyChanged
    {
        private string provider_txt;
        public string Provider_txt
        {
            get
            {
                return provider_txt;
            }
            set
            {
                provider_txt = value;
                NotifyPropertyChanged("Provider_txt");
            }
        }
        private string provider;
        public string Provider
        {
            get
            {
                return provider;
            }
            set
            {

                provider = value;
                provider_txt = provider;
                NotifyPropertyChanged("Provider_txt");
            }
        }

        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.SuppliersTable> providersDB;
        public ObservableCollection<String> providers { get; set; }


        public AddProvider_ViewModel()
        {
            providers = new ObservableCollection<String>();
            using (TradeContext db = new TradeContext())
            {
                providersDB = db.SuppliersTables;
                foreach (SuppliersTable mnf in providersDB)
                {
                    providers.Add(mnf.Name);
                }
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

        private RelayCommand addProdvider;
        public RelayCommand AddProdvider
        {
            get
            {
                return addProdvider ??
                  (addProdvider = new RelayCommand(obj =>
                  {
                      if(provider_txt != null)
                      {
                          if (provider_txt.Replace(" ", "") != "")
                          {
                              using (TradeContext db = new TradeContext())
                              {
                                  bool check = true;
                                  int num = 0;
                                  var providersDB = db.SuppliersTables;
                                  foreach (var prvdr in providersDB)
                                  {
                                      if (prvdr.Name == provider_txt)
                                      {
                                          check = false;
                                          MessageBox.Show("Данный поставщик уже существует");
                                          break;
                                      }
                                  }
                                  if (check == true)
                                  {
                                      SuppliersTable providersTable = new SuppliersTable(provider_txt);
                                      providersDB.Add(providersTable);
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
        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string props)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(props));
        }
    }
}
