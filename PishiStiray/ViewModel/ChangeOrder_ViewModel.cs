using PishiStiray.Data;
using PishiStiray.Servieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishiStiray.ViewModel
{
    public class ChangeOrder_ViewModel
    {
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.Order1> ordersDB;

        public ObservableCollection<String> statuts { get; set; }

        public string status { get; set; }

        public DateTime CurrentDate { get;set;}

        public ChangeOrder_ViewModel() 
        {
            statuts = new ObservableCollection<String>()
            {
                "Получен",
                "Не получен",
            };

            using (TradeContext db = new TradeContext())
            {
                ordersDB = db.Order1s;
                foreach (var order in ordersDB)
                {
                    if(order.OrderId == Global.orderId)
                    {
                        if(order.Status == 1)
                        {
                            status = "Не получен";
                        }
                        if(order.Status == 2)
                        {
                            status = "Получен";
                        }
                    }
                    //CurrentDate = Convert.ToDateTime(order.DeliveryDate);
                  //  CurrentDate = DateTime.ParseExact(order.DeliveryDate, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);

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

        private RelayCommand сhangeOrder;
        public RelayCommand ChangeOrder
        {
            get
            {
                return сhangeOrder ??
                  (сhangeOrder = new RelayCommand(obj =>
                  {
                        using (TradeContext db = new TradeContext())
                        {
                          ordersDB = db.Order1s;
                          foreach(var order in ordersDB)
                          {
                              if(order.OrderId == Global.orderId)
                              {
                                  if(status == "Не получен")
                                  {
                                      order.Status = 1;
                                  }
                                  if (status == "Получен")
                                  {
                                      order.Status = 2;
                                  }
                                  order.DeliveryDate = CurrentDate.ToString("dd.MM.yyyy");
                              }
                          }
                          NavigateViewModel navigateViewModel = new NavigateViewModel();
                          navigateViewModel.Navigate("Views/tradePage.xaml");
                          db.SaveChanges();
                        }
                  }));
            }

        }
    }
}
