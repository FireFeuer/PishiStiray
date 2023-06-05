using DevExpress.Mvvm.Native;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PishiStiray.Model;
using PishiStiray.Servieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Reflection;
using TradeContext = PishiStiray.Data.TradeContext;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Windows.Media;
using System.Security.Cryptography.Xml;
using PishiStiray.Views;
using static System.Math;
using PishiStiray.Entities;
using System.Security.Cryptography.X509Certificates;
using DevExpress.Mvvm;
using iTextSharp.text.pdf;
using static iTextSharp.text.Document;
using static iTextSharp.text.pdf.PdfWriter;
using System.Windows.Controls;
using System.Diagnostics;
using iTextSharp.text;

namespace PishiStiray.ViewModel
{
    public class userListTrade_ViewModel : INotifyPropertyChanged
    {
        public ProductService productService = new ProductService();

        public string SpointIssue { get; set; }

        public string Total_products { get; set; } // Общее количество продуктов в корзине у пользователя 
        private int total_products_;
        public int total_products
        {
            get
            {
                return total_products_;
            }
            set
            {
                total_products_ = value;
                Total_products = "Товара в корзине: " + Convert.ToString(total_products_);
            }
        }


        public string Total_price { get; set; } // общая цена заказа без скидки
        public string Discounted_price { get; set; } // цена заказа с учётом скидки
        public string Total_discount { get; set; } // скидка в денежном виде
        private double total_price = 0;
        private double discounted_price = 0;
        private double total_discount = 0;

        public string basket_visible { get; set; } // Visible для корзины

        public string Count_products { get; set; } // Тект показа количества товара в показе из всего количества товара
        private int all_count; // Товара всего
        private int now_count; // Нужен для показа количества товара в списке продуктов (Кол-во показано / Всего количетсво) Например: 11 из 33 --- 11 здесь это now_count
        public int Now_count
        {
            get
            {
                return now_count;
            }
            set
            {
                now_count = value;
                Count_products = $"{now_count} из {all_count}";
            }
        }

        public ProductE SelectedItem { get; set; } // Выбранный элемент (Товар)

        public Order1E SelectedItem_orders { get; set; } // Выбранный элемент (Заказ)

        public string Message { get; set; } // для отправки сообщения об его ошибках пользователю 

        public string orders_visible { get; set; } // Для отображения кнопки заказов разным группам пользователей

        public string visibility_addItem { get; set; } // Для отображения кнопки в меню добавления нового товара в список продуктов магазина администратором

        public string visibility_changeItem { get; set; } // Для отображения кнопки в меню изменения товара в списке продуктов магазина администратором

        public string visibility_removeItem { get; set; } // Для отображения кнопки в меню удаления товара в списке продуктов магазина администратором

        public string pointIssue_visible { get; set; } // Для отображения выбора пунктов выдачи при формировании заказа в корзине

        public string order_visible { get; set; } // Для отображения кнопки формирования заказа в корзине

        public ProductE SelectedItem1 { get; set; } // Выбранный элемент (Товар) в корзине

        public string FullName { get; set; } // Для отображения имени пользователя

        public string visibility { get; set; } // Visible для списка товаров магазина
        public string visibility1 { get; set; } // Visible для списка товаров в корзине
        public string visibility2 { get; set; } // Visible для списка заказов
        public string Menu_Vision { get; set; } // Visible для верхнего меню

        public string order_filter_vision { get; set; } 

        public string adm_reg { get; set; } // Visible для добавления менеджера и админа

        

        public string visible_line { get; set; } // Перечёркивающая линия первоначальной цены, при наличии скидки


        public ObservableCollection<String> Delivereds { get; set; } // Список выбора фильтра на наличие доставки
        public ObservableCollection<String> Discounts { get; set; } // Список выбора фильтра на наличие скидки
        public ObservableCollection<String> Costs { get; set; } // Список для выбора типа сортировки по цене
        public ObservableCollection<String> pointIssue { get; set; } // Список выбора пункта выдачи
        public ObservableCollection<ProductE> Products { get; set; } // Список товаров магазина
        public ObservableCollection<ProductE> Products1 { get; set; } // Резервный список товаров магазина, нужен для работы кода
        public ObservableCollection<ProductE> Products2 { get; set; } // Список товаров корзины 
        public ObservableCollection<ProductE> Products3 { get; set; } // Резервный список товаров корзины, нужен для работы кода
        public ObservableCollection<ProductE> Products_order { get; set; } // Список товаров списка заказов

        public ObservableCollection<Order1E> Orders_list { get; set; } // Список заказов
        public ObservableCollection<Order1E> Orders_list1 { get; set; } // Список заказов

        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.Product> products; // бд товары
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.IssueTable> pointIssues; // бд пункты выдачи
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.ManufacturersTable> manufacturersTables; // бд поставщики
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.SuppliersTable> providerTables;
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.TypeOfAccessoryTable> accessoryTables;

        private string search; // Привязанный текст поиска товаров в поисковике
        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                if (visibility == "Visible")
                {
                    if (search == null)
                    {
                        search = "";
                    }
                    string Search1 = Search.ToUpper();
                    Products = productService.updateProducts(sDiscounts, Products, Products1, Search1);
                    Products = productService.sCostsMeth(sCosts, Products);
                    Products2 = productService.sCostsMeth(sCosts, Products2);
                    Now_count = Products.Count;
                }
                if (visibility == "Hidden")
                {
                    string Search1 = Search.ToUpper();
                    Products2 = productService.updateProducts(sDiscounts, Products2, Products3, Search1);
                    Products1 = productService.sCostsMeth(sCosts, Products1);
                    Products2 = productService.sCostsMeth(sCosts, Products2);
                    Now_count = Products2.Count;
                }
            }
        }

        private string sDelivered;
        public string SDelivered
        {
            get { return sDelivered; }
            set 
            { 
                sDelivered = value;
                if (sDelivered == "Все заказы")
                {
                    Orders_list = Orders_list1;
                }
                if (sDelivered == "Не доставлено")
                {
                    Orders_list = new ObservableCollection<Order1E>(Orders_list1.Where(i => i.Status == "Не доставлено"));
                }
                if (sDelivered == "Доставлено")
                {
                    Orders_list = new ObservableCollection<Order1E>(Orders_list1.Where(i => i.Status == "Доставлено"));
                }
              
            }
        }

        private string sDiscounts; // Список для фильтрации по скидке
        public string SDiscounts
        {
            get { return sDiscounts; }
            set 
            {
                sDiscounts = value;
                if (visibility == "Visible")
                {
                    if (search == null)
                    {
                        search = "";
                    }
                    string Search1 = Search.ToUpper();
                    Products = productService.updateProducts(sDiscounts, Products, Products1, Search1);
                    Products = productService.sCostsMeth(sCosts, Products);
                    Products2 = productService.sCostsMeth(sCosts, Products2);
                    Now_count = Products.Count;
                }
                if (visibility == "Hidden")
                {
                    string Search1 = Search.ToUpper();
                    Products2 = productService.updateProducts(sDiscounts, Products2, Products3, Search1);
                    Products = productService.sCostsMeth(sCosts, Products);
                    Products2 = productService.sCostsMeth(sCosts, Products2);
                    Now_count = Products2.Count;
                }
            }
        }

        private string sCosts; // Список для сортировки по цене
        public string SCosts
        {
            get { return sCosts; }
            set
            {
                sCosts = value;
                Products = productService.sCostsMeth(sCosts, Products);
                Products2 = productService.sCostsMeth(sCosts, Products2);
            }
        }
        public userListTrade_ViewModel()
        {
            sCosts = "По возрастанию";
             sDiscounts = "Все диапазоны";

            Products = new ObservableCollection<ProductE>();

            order_visible = "Hidden";

            pointIssue_visible = "Hidden";

            visible_line = "Visible";

            visibility = "Visible";
            visibility1 = "Hidden";
            visibility2 = "Hidden";

            basket_visible = "Hidden";

            order_filter_vision = "Hidden";


            FullName = Global.name;

            if (Global.role == "user")
            {
                orders_visible = "Hidden";
                visibility_removeItem = "Collapsed";
                visibility_addItem = "Collapsed";
                visibility_changeItem = "Collapsed";
                adm_reg = "Collapsed";
             
            }
            if (Global.role == "manager")
            {
                orders_visible = "Visible";
                visibility_removeItem = "Collapsed";
                visibility_addItem = "Collapsed";
                visibility_changeItem = "Collapsed";
                adm_reg = "Collapsed";
              
            }
            if (Global.role == "admin")
            {
                orders_visible = "Visible";
                visibility_removeItem = "Visible";
                visibility_addItem = "Visible";
                visibility_changeItem = "Visible";
                adm_reg = "Visible";           
            }

            using (TradeContext db = new TradeContext())
            {
                products = db.Products;
                manufacturersTables = db.ManufacturersTables;
                providerTables = db.SuppliersTables;
                accessoryTables = db.TypeOfAccessoryTables;
                List<string> ascry = new List<string>();
                List<string> mnf = new List<string>();
                List<string> prvdr = new List<string>();
                int i = 0;
                int j = 0;
                int k = 0;
                foreach (ManufacturersTable m in manufacturersTables)
                {
                    mnf.Add(m.Name);
                    i++;
                }
                foreach (SuppliersTable p in providerTables)
                {
                    prvdr.Add(p.Name);
                    j++;
                }
                foreach (TypeOfAccessoryTable a in accessoryTables)
                {
                    ascry.Add(a.Name);
                    k++;
                }
                foreach (Product product in products)
                {
                    double cost = product.Cost;
                    double nowdiscount = product.NowDiscount;
                    string path = "";
                    if (product.Picture == "")
                    {
                        path = "picture.png";
                    }
                   
                    if (product.Picture != "")
                    {
                        path = product.Picture;
                    }
                    string m = "";
                    for (int ii = 0; ii < mnf.Count; ii++)
                    {
                        if (Convert.ToInt32(product.Manufacturer) == ii + 1)
                        {
                            m = mnf[ii];
                        }
                    }
                    string p = "";
                    for (int ii = 0; ii < prvdr.Count; ii++)
                    {
                        if (product.Provider == ii + 1)
                        {
                            p = prvdr[ii];
                        }
                    }

                    string a = "";
                    for (int ii = 0; ii < ascry.Count; ii++)
                    {
                        if (product.ProductCategory == ii + 1)
                        {
                            a = ascry[ii];
                        }
                    }

                    if (product.Status == 0)
                    {
                        if (product.NowDiscount > 0 && System.IO.Path.GetFullPath(path) != path)
                        {
                            visible_line = "Visible";

                            Products.Add(new ProductE
                            {
                                Cost = "Старая цена: " + product.Cost.ToString() + "Р",
                                Description = product.Description,
                                Discount = "Скидка: " + product.Discount,
                                Manufacturer = "Новая цена!: " + (cost - (cost / 100 * nowdiscount)) + " Р" + "\nПроизводитель: " + m,
                                NowDiscount = product.NowDiscount,
                                Picture = System.IO.Path.GetFullPath("Resources/" + path),
                                ProductArticleNumber = "Артикль: " + product.ProductArticleNumber,
                                ProductCategory = "Категория товара: " + a,
                                ProductName = product.ProductName,
                                Provider = "Поставщик: " + p,
                                QuantityStock = "Товара на складе: " + product.QuantityStock.ToString(),
                                UnitOfMeasurement = product.UnitOfMeasurement,
                                Status = product.Status,
                                AmountCart = 1.ToString(),
                                amd_reg1 = adm_reg,
                                manufacturer_global = m,
                                provider_number = product.Provider
                            });
                        }
                        else if(product.NowDiscount > 0 && System.IO.Path.GetFullPath(path) == path)
                        {
                            visible_line = "Visible";

                            Products.Add(new ProductE
                            {
                                Cost = "Старая цена: " + product.Cost.ToString() + "Р",
                                Description = product.Description,
                                Discount = product.Discount,
                                Manufacturer = "Новая цена!: " + (cost - (cost / 100 * nowdiscount)) + " Р" + "\nПроизводитель: " + m,
                                NowDiscount = product.NowDiscount,
                                Picture = path,
                                ProductArticleNumber = "Артикль: " + product.ProductArticleNumber,
                                ProductCategory = "Категория товара: " + a,
                                ProductName = product.ProductName,
                                Provider = "Поставщик: " + p,
                                QuantityStock = "Товара на складе: " + product.QuantityStock.ToString(),
                                UnitOfMeasurement = product.UnitOfMeasurement,
                                Status = product.Status,
                                AmountCart = 1.ToString(),
                                amd_reg1 = adm_reg,
                                manufacturer_global = m,
                                provider_number = product.Provider
                            });
                        }
                        else
                        {
                            visible_line = "Hidden";
                            Products.Add(new ProductE
                            {
                                Cost = "Старая цена: " + product.Cost.ToString() + "Р",
                                Description = product.Description,
                                Discount = product.Discount,
                                Manufacturer = m,
                                NowDiscount = product.NowDiscount,
                                Picture = System.IO.Path.GetFullPath("Resources/" + path),
                                ProductArticleNumber = "Артикль: " + product.ProductArticleNumber,
                                ProductCategory = "Категория товара: " + a,
                                ProductName = product.ProductName,
                                Provider = "Поставщик: " + p,
                                QuantityStock = "Товара на складе: " + product.QuantityStock.ToString(),
                                UnitOfMeasurement = product.UnitOfMeasurement,
                                AmountCart = 1.ToString(),
                                amd_reg1 = adm_reg,
                                manufacturer_global = m,
                                provider_number = product.Provider
                            });
                        }
                    }
                }
                Products3 = Products2;
                Products1 = Products;
                Products2 = new ObservableCollection<ProductE>();
                Products_order = new ObservableCollection<ProductE>();
                Orders_list = new ObservableCollection<Order1E>();
                Orders_list1 = Orders_list;
                all_count = Products.Count;
                Now_count = all_count;
            }

            Costs = new ObservableCollection<String>()
            {
                "По возрастанию",
                "По убыванию"
            };

            Discounts = new ObservableCollection<String>()
            {
                "Все диапазоны",
                "0 - 9%",
                "10 - 14%",
                "15% и более"
            };

        
        Delivereds = new ObservableCollection<String>()
            {
                "Все заказы",
                "Доставлено",
                "Не доставлено"
            };

            pointIssue = new ObservableCollection<String>();
            using (TradeContext db = new TradeContext())
            {
                pointIssues = db.IssueTables;
                foreach (IssueTable it in pointIssues)
                {
                    pointIssue.Add(it.Index + " " + it.City + " " + it.Issue + " " + it.House);
                }
            }
        }

        private RelayCommand basketCommand; // Команда перехода в корзину
        public RelayCommand BasketCommand
        {
            get
            {
                return basketCommand ??
                  (basketCommand = new RelayCommand(obj =>
                  {
                      using (TradeContext db = new TradeContext())
                      {

                          Search = "";
                          if (visibility == "Visible")
                          {
                              visibility = "Hidden";
                              visibility1 = "Visible";
                              visibility2 = "Hidden";
                              orders_visible = "Hidden";
                              Total_price = "Ваша цена без скидки: " + total_price + "Р";
                              Total_discount = "Ваша цена со скидкой: " + total_discount + "Р";
                              Discounted_price = "Ваша скидка: " + discounted_price + "Р";
                          }
                          else
                          {
                              visibility = "Visible";
                              visibility1 = "Hidden";
                              visibility2 = "Hidden";
                              if (Global.role == "manager" || Global.role == "admin")
                              {
                                  orders_visible = "Visible";
                              }                               
                          }

                          if (visibility == "Visible")
                          {
                              // start
                              Products.Clear();
                              Products1.Clear();
                              products = db.Products;
                              manufacturersTables = db.ManufacturersTables;
                              providerTables = db.SuppliersTables;
                              accessoryTables = db.TypeOfAccessoryTables;
                              List<string> ascry = new List<string>();
                              List<string> mnf = new List<string>();
                              List<string> prvdr = new List<string>();
                              int i = 0;
                              int j = 0;
                              int k = 0;
                              foreach (ManufacturersTable m in manufacturersTables)
                              {
                                  mnf.Add(m.Name);
                                  i++;
                              }
                              foreach (SuppliersTable p in providerTables)
                              {
                                  prvdr.Add(p.Name);
                                  j++;
                              }
                              foreach (TypeOfAccessoryTable a in accessoryTables)
                              {
                                  ascry.Add(a.Name);
                                  k++;
                              }
                              foreach (Product product in products)
                              {
                                  double cost = product.Cost;
                                  double nowdiscount = product.NowDiscount;
                                  string path = "";
                                  if (product.Picture == "")
                                  {
                                      path = "picture.png";
                                  }

                                  if (product.Picture != "")
                                  {
                                      path = product.Picture;
                                  }
                                  string m = "";
                                  for (int ii = 0; ii < mnf.Count; ii++)
                                  {
                                      if (int.Parse(product.Manufacturer) == ii + 1)
                                      {
                                          m = mnf[ii];
                                      }
                                  }
                                  string p = "";
                                  for (int ii = 0; ii < prvdr.Count; ii++)
                                  {
                                      if (product.Provider == ii + 1)
                                      {
                                          p = prvdr[ii];
                                      }
                                  }

                                  string a = "";
                                  for (int ii = 0; ii < ascry.Count; ii++)
                                  {
                                      if (product.ProductCategory == ii + 1)
                                      {
                                          a = ascry[ii];
                                      }
                                  }

                                  if (product.Status == 0)
                                  {
                                      if (product.NowDiscount > 0 && System.IO.Path.GetFullPath(path) != path)
                                      {
                                          visible_line = "Visible";

                                          Products.Add(new ProductE
                                          {
                                              Cost = "Старая цена: " + product.Cost.ToString() + "Р",
                                              Description = product.Description,
                                              Discount = "Скидка: " + product.Discount,
                                              Manufacturer = "Новая цена!: " + (cost - (cost / 100 * nowdiscount)) + " Р" + "\nПроизводитель: " + m,
                                              NowDiscount = product.NowDiscount,
                                              Picture = System.IO.Path.GetFullPath("Resources/" + path),
                                              ProductArticleNumber = "Артикль: " + product.ProductArticleNumber,
                                              ProductCategory = "Категория товара: " + a,
                                              ProductName = product.ProductName,
                                              Provider = "Поставщик: " + p,
                                              QuantityStock = "Товара на складе: " + product.QuantityStock.ToString(),
                                              UnitOfMeasurement = product.UnitOfMeasurement,
                                              Status = product.Status,
                                              AmountCart = 1.ToString(),
                                              amd_reg1 = adm_reg,
                                              manufacturer_global = m,
                                              provider_number = product.Provider
                                          });
                                      }
                                      else if (product.NowDiscount > 0 && System.IO.Path.GetFullPath(path) == path)
                                      {
                                          visible_line = "Visible";

                                          Products.Add(new ProductE
                                          {
                                              Cost = "Старая цена: " + product.Cost.ToString() + "Р",
                                              Description = product.Description,
                                              Discount = product.Discount,
                                              Manufacturer = "Новая цена!: " + (cost - (cost / 100 * nowdiscount)) + " Р" + "\nПроизводитель: " + m,
                                              NowDiscount = product.NowDiscount,
                                              Picture = path,
                                              ProductArticleNumber = "Артикль: " + product.ProductArticleNumber,
                                              ProductCategory = "Категория товара: " + a,
                                              ProductName = product.ProductName,
                                              Provider = "Поставщик: " + p,
                                              QuantityStock = "Товара на складе: " + product.QuantityStock.ToString(),
                                              UnitOfMeasurement = product.UnitOfMeasurement,
                                              Status = product.Status,
                                              AmountCart = 1.ToString(),
                                              amd_reg1 = adm_reg,
                                              manufacturer_global = m,
                                              provider_number = product.Provider
                                          });
                                      }
                                      else
                                      {
                                          visible_line = "Hidden";
                                          Products.Add(new ProductE
                                          {
                                              Cost = "Старая цена: " + product.Cost.ToString() + "Р",
                                              Description = product.Description,
                                              Discount = product.Discount,
                                              Manufacturer = m,
                                              NowDiscount = product.NowDiscount,
                                              Picture = System.IO.Path.GetFullPath("Resources/" + path),
                                              ProductArticleNumber = "Артикль: " + product.ProductArticleNumber,
                                              ProductCategory = "Категория товара: " + a,
                                              ProductName = product.ProductName,
                                              Provider = "Поставщик: " + p,
                                              QuantityStock = "Товара на складе: " + product.QuantityStock.ToString(),
                                              UnitOfMeasurement = product.UnitOfMeasurement,
                                              AmountCart = 1.ToString(),
                                              amd_reg1 = adm_reg,
                                              manufacturer_global = m,
                                              provider_number = product.Provider
                                          });
                                      }
                                  }
                              }
                              Products3 = Products2;
                              Products1 = Products;                      
                              
                              all_count = Products.Count;
                              Now_count = all_count;

                              pointIssue_visible = "Hidden";
                              order_visible = "Hidden";
                              if (Products2 != null && Products3 != null)
                              {
                                
                                  Products = productService.updateProducts(sDiscounts, Products, Products1, "");
                                  Products = productService.sCostsMeth(sCosts, Products);
                                  Products2 = productService.sCostsMeth(sCosts, Products2);

                                  all_count = Products1.Count;
                                  Now_count = Products.Count;
                              }
                          }
                          if (visibility == "Hidden")
                          {                         
                            
                              pointIssue_visible = "Visible";
                              order_visible = "Visible";
                              if (Products2 != null && Products3 != null)
                              {

                                  Products2 = productService.updateProducts(sDiscounts, Products2, Products3, "");
                                  Products = productService.sCostsMeth(sCosts, Products);
                                  Products2 = productService.sCostsMeth(sCosts, Products2);
                                  NotifyPropertyChanged("Serch");
                                  all_count = Products3.Count;
                                  Now_count = Products2.Count;                                 
                              }
                          }
                          if (Products3.Count == 0)
                          {
                              basket_visible = "Hidden";
                          }
                      }
                  }));
            }
        }

        private RelayCommand addItem; // Команда добавления товара в корзину
        public RelayCommand AddItem
        {
            get
            {
                return addItem ??
                  (addItem = new RelayCommand(obj =>
                  {
                      if (SelectedItem != null)
                      {
                          bool check_quantityStock = true;
                          bool check = true;
                          foreach (ProductE product in Products2)
                          {
                              if (product == SelectedItem)
                              {
                                  check = false;
                              }                                                                                  
                          }
                          foreach(ProductE product in Products)
                          {
                              if ((int.Parse(product.QuantityStock.Replace("Товара на складе: ", "")) < 1) && (product == SelectedItem))
                              {
                                  MessageBox.Show("Товар закончился");
                                  check_quantityStock = false;
                                  break;
                              }
                          }
                         
                          if (check == true && check_quantityStock == true)
                          {
                              basket_visible = "Visible";
                              string productCost = SelectedItem.Cost.Replace("Старая цена: ", "");
                              total_price += int.Parse(productCost.Replace("Р", ""));
                              total_discount += Convert.ToDouble(productCost.Replace("Р", "")) - ((Convert.ToDouble(productCost.Replace("Р", "")) / 100 * Convert.ToDouble(SelectedItem.NowDiscount)));
                              total_discount = Round(total_discount, 2);
                              discounted_price = Convert.ToDouble(total_price) - Convert.ToDouble(total_discount);
                              discounted_price = Round(discounted_price, 2);

                              SelectedItem.AmountCart = 1.ToString();
                              Products2.Add(SelectedItem);
                              Products3 = Products2;
                              total_products++;
                          }
                          if (check == false)
                          {
                              MessageBox.Show("Товар уже в корзине");
                          }
                      }
                  }));
            }
        }

        private RelayCommand changeORD; 
        public RelayCommand ChangeORD
        {
            get
            {
                return changeORD ??
                  (changeORD = new RelayCommand(obj =>
                  {
                      Global.orderCode = SelectedItem_orders.Code;
                      Global.orderId = SelectedItem_orders.OrderId;
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/ChangeOrder_Window.xaml");
                  }));
            }
        }

        private RelayCommand order_command; // Команда оформления заказа
        public RelayCommand Order_command
        {
            get
            {
                return order_command ??
                  (order_command = new RelayCommand(obj =>
                  {
                      if (SpointIssue != null && total_products != 0)
                      {
                          bool three_or_six = false;
                          int count_products = 0;
                          int id = 0;
                          int index = 0;
                          int code = 0;
                          foreach (string s in pointIssue)
                          {
                              if (SpointIssue == s)
                              {
                                  break;
                              }
                              index++;
                          }
                          using (TradeContext db = new TradeContext())
                          {
                              products = db.Products;
                              foreach (ProductE product in Products2)
                              {
                                  foreach (Product productDB in products)
                                  {
                                      if (productDB.ProductArticleNumber == product.ProductArticleNumber.Replace("Артикль: ", ""))
                                      {
                                          productDB.QuantityStock = productDB.QuantityStock - int.Parse(product.AmountCart);
                                      }
                                  }
                              }
                              int key = 0;
                              var orders = db.Order1s;
                              foreach (var order in orders)
                              {
                                  id = order.OrderId;
                                  code = order.Code;
                              }
                              var orderProducts = db.Orderproducttrues;
                              foreach (var op in orderProducts)
                              {
                                  key = op.Key;
                              }
                              List<Orderproducttrue> orderproducttrues = new List<Orderproducttrue>();
                              foreach (var product in Products2)
                              {
                                  orderproducttrues.Add(new Orderproducttrue(id + 1, product.ProductArticleNumber.Replace("Артикль: ", ""), product.AmountCart.ToString(), key + 1));
                                  key++;
                                  string qnty_stck = product.QuantityStock.Replace("Товара на складе: ", "");
                                  count_products += int.Parse(qnty_stck);
                              }
                              if (count_products < 3)
                              {
                                  three_or_six = false;
                              }
                              if (count_products >= 3)
                              {
                                  three_or_six = true;
                              }
                              if (three_or_six == false)
                              {
                                  Order1 order1 = new Order1(DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.AddDays(3).ToString("dd.MM.yyyy") + $"\nКод: {index + 1}", index + 1, Global.name, code + 1, 1, count_products);
                                  orders.Add(order1);
                              }
                              if (three_or_six == true)
                              {
                                  Order1 order1 = new Order1(DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.AddDays(6).ToString("dd.MM.yyyy") + $"\nКод: {index+1}", index + 1, Global.name, code + 1, 1, count_products);
                                  orders.Add(order1);
                              }

                              foreach (var product in orderproducttrues)
                              {
                                  db.Set<Orderproducttrue>().AsNoTracking();
                                  orderProducts.Add(product);
                                  db.SaveChanges();
                              }
                              productService.pdfCreate(id, three_or_six, SelectedItem, Products2, Products3, Discounted_price, Total_discount, SpointIssue, code);
                          }                   
                      }
                      if (SpointIssue == null)
                      {
                          MessageBox.Show("Выберите пункт выдачи");
                      }
                      if (total_products == 0)
                      {
                          MessageBox.Show("В корзине должен быть хотя бы один товар для оформления заказа");
                      }
                  }));
                 
            }
        }


        private RelayCommand signOutCommand; // Команда выхода из аккаунта/страницы
        public RelayCommand SignOutCommand
        {
            get
            {
                return signOutCommand ??
                  (signOutCommand = new RelayCommand(obj =>
                  {
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/autorizationPage.xaml");
                  }));
            }
        }

        private RelayCommand removeItemAdm; // Команда удаления товара из БД
        public RelayCommand RemoveItemAdm
        {
            get
            {
                return removeItemAdm ??
                (removeItemAdm = new RelayCommand(obj =>
                {
                    using (TradeContext db = new TradeContext())
                    {
                        products = db.Products;
                        foreach (Product product in products.ToList())
                        {
                            if (SelectedItem.ProductArticleNumber == product.ProductArticleNumber)
                            {
                                product.Status = 1;
                                db.SaveChanges();
                            }
                        }
                        Products.Remove(SelectedItem);
                        Products1 = Products;
                    }
                }));
            }
        }

        private RelayCommand addItemAdm; // Переход на окно с возможностью добавления нового товара в БД
        public RelayCommand AddItemAdm
        {
            get
            {
                return addItemAdm ??
                  (addItemAdm = new RelayCommand(obj =>
                  {
                      Global.add_or_change = "Добавление товара";

                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/Add_or_Change_Window.xaml");
                  }));
            }
        }

        private RelayCommand changeItemAdm; // Переход на окно с возможностью изменения товара в БД
        public RelayCommand ChangeItemAdm
        {
            get
            {
                return changeItemAdm ??
                  (changeItemAdm = new RelayCommand(obj =>
                  {
                      Global.add_or_change = "Изменение товара";

                      Global.productArticleNumber = SelectedItem.ProductArticleNumber;
                      Global.productName = SelectedItem.ProductName;
                      Global.unitOfMeasurement = SelectedItem.UnitOfMeasurement;
                      string productCost = SelectedItem.Cost.Replace("Старая цена: ", "");
                      Global.cost = int.Parse(productCost.Replace("Р", ""));
                      Global.discount = SelectedItem.Discount;
                      Global.manufacturer = SelectedItem.manufacturer_global;
                      Global.provider = SelectedItem.Provider.Replace("Поставщик: ", "");
                      Global.productCategory = SelectedItem.ProductCategory.Replace("Категория товара: ", "");
                      Global.quantityStock = SelectedItem.QuantityStock;
                      Global.description = SelectedItem.Description; 
                      Global.picture = SelectedItem.Picture;
                      Global.nowDiscount = SelectedItem.NowDiscount;
                      Global.provider_number = SelectedItem.provider_number;
             
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/Add_or_Change_Window.xaml");
                  }));
            }

        }
        private RelayCommand manufacurer_command;
        public RelayCommand Manufacurer_command
        {
            get
            {
                return manufacurer_command ??
                  (manufacurer_command = new RelayCommand(obj =>
                  {
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/AddManOrProvider_Window.xaml");
                  }));
            }

        }

        private RelayCommand provider_command;
        public RelayCommand Provider_command
        {
            get
            {
                return provider_command ??
                  (provider_command = new RelayCommand(obj =>
                  {
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/AddProvider_Window.xaml");
                  }));
            }

        }

        private RelayCommand orders_command; // Переход на страницу с заказами
        public RelayCommand Orders_command
        {
            get
            {
                return orders_command ??
                  (orders_command = new RelayCommand(obj =>
                  {
                      Orders_list.Clear();
                      Orders_list1.Clear();
                      if (visibility == "Hidden")
                      {
                          visibility = "Visible";
                          visibility1 = "Hidden";
                          visibility2 = "Hidden";
                          if (Products3 != null && Products3.Count != 0) 
                          {
                              basket_visible = "Visible";
                          }                        
                          Menu_Vision = "Visible";
                          order_filter_vision = "Hidden";
                      }
                      else
                      {
                          visibility = "Hidden";
                          visibility1 = "Hidden";
                          visibility2 = "Visible";
                          basket_visible = "Hidden";
                          Menu_Vision = "Hidden";
                          order_filter_vision = "Visible";
                      }
                      pointIssue = new ObservableCollection<String>();
                      using (TradeContext db = new TradeContext())
                      {
                          pointIssues = db.IssueTables;
                          foreach (IssueTable it in pointIssues)
                          {
                              pointIssue.Add(it.Index + " " + it.City + " " + it.Issue + " " + it.House);
                          }
                      }
                      using (TradeContext db = new TradeContext())
                      {
                          var issues = db.IssueTables;
                          List<string> issues_txt = new List<string>();
                          foreach (var issue in issues)
                          {
                              issues_txt.Add(issue.Index + " " + issue.City + " " + issue.Issue + " " + issue.House);
                          }
                          var orders = db.Order1s;
                          foreach (Order1 order in orders)
                          {
                              string txt = "";
                              for (int ii = 0; ii < issues_txt.Count; ii++)
                              {
                                  if (order.PointIssue == ii + 1)
                                  {
                                      txt = issues_txt[ii];
                                  }
                              }
                              string _status = "";
                              if (order.Status == 1)
                              {
                                  _status = "Не доставлено";
                              }
                              if (order.Status == 2)
                              {
                                  _status = "Доставлено";
                              }
                              Order1E order1E = new Order1E(order.OrderId, "Дата оформления заказа: " + order.OrderDate, "Дата получения заказа: " + order.DeliveryDate,
                                  txt, order.Fio, order.Code, _status, order.Count);
                              Orders_list.Add(order1E);
                              Orders_list1 = Orders_list;
                          }
                      }
                  }));
            }
        }

        private RelayCommand view_product; // Просмотреть товары в заказе админу
        public RelayCommand View_product
        {
            get
            {
                return view_product ??
                  (view_product = new RelayCommand(obj =>
                  {
                      if (SelectedItem_orders != null)
                      {
                          Products_order = productService.viewProductsFromOrder(Products_order, SelectedItem_orders, Products1, visible_line);
                      }
                      else{}                   
                  }));
            }
        }

        private RelayCommand plusCart;
        public RelayCommand PlusCart
        {
            get
            {
                return plusCart ??
                  (plusCart = new RelayCommand(obj =>
                  {
                      using (TradeContext db = new TradeContext())
                      {
                          products = db.Products;
                          if (SelectedItem1 == null)
                          {
                              MessageBox.Show("Выберите элемент");
                          }
                          if (SelectedItem1 != null)
                          {
                              if (int.Parse(SelectedItem1.AmountCart) != int.Parse(SelectedItem1.QuantityStock.Replace("Товара на складе: ", "")))
                              {
                                  SelectedItem1.AmountCart = (int.Parse(SelectedItem1.AmountCart) + 1).ToString();
                                  total_products++;
                                  string productCost = SelectedItem1.Cost.Replace("Старая цена: ", "");
                                  total_price += int.Parse(productCost.Replace("Р", ""));
                                  total_discount += Convert.ToDouble(productCost.Replace("Р", "")) - ((Convert.ToDouble(productCost.Replace("Р", "")) / 100 * Convert.ToDouble(SelectedItem1.NowDiscount)));
                                  total_discount = Round(total_discount, 2);
                                  discounted_price = Convert.ToDouble(total_price) - Convert.ToDouble(total_discount);
                                  discounted_price = Round(discounted_price, 2);

                                  Total_price = "Ваша цена без скидки: " + total_price + "Р";
                                  Total_discount = "Ваша цена со скидкой: " + total_discount + "Р";
                                  Discounted_price = "Ваша скидка: " + discounted_price + "Р";
                              }
                          }
                          Products2 = Products3;
                      }

                      if (search == null)
                      {
                          search = "";
                      }
                      string Search1 = Search.ToUpper();

                      if (sDiscounts == "")
                      {
                          sDiscounts = "Все диапазоны";
                      }

                      if ((Products2 != null) && (Products3 != null))
                      {
                          Products2 = productService.updateProducts(sDiscounts, Products2, Products3, Search1);
                      }
                    
                      Products = productService.sCostsMeth(sCosts, Products);
                      Products2 = productService.sCostsMeth(sCosts, Products2);

                  }));
            }
        }


        private RelayCommand minusCart;
        public RelayCommand MinusCart
        {
            get
            {
                return minusCart ??
                  (minusCart = new RelayCommand(obj =>
                  {
                      using (TradeContext db = new TradeContext())
                      {
                          products = db.Products;
                          if (SelectedItem1 == null)
                          {
                              MessageBox.Show("Выберите элемент");
                          }
                          if (SelectedItem1 != null)
                          {
                              SelectedItem1.AmountCart = (int.Parse(SelectedItem1.AmountCart) - 1).ToString();
                              total_products--;
                              string productCost = SelectedItem1.Cost.Replace("Старая цена: ", "");
                              total_price = total_price - int.Parse(productCost.Replace("Р", ""));
                              total_discount = total_discount - (Convert.ToDouble(productCost.Replace("Р", "")) - ((Convert.ToDouble(productCost.Replace("Р", "")) / 100 * Convert.ToDouble(SelectedItem1.NowDiscount))));
                              total_discount = Round(total_discount, 2);
                              discounted_price = Convert.ToDouble(total_price) - Convert.ToDouble(total_discount);
                              discounted_price = Round(discounted_price, 2);

                              Total_price = "Ваша цена без скидки: " + total_price + "Р";
                              Total_discount = "Ваша цена со скидкой: " + total_discount + "Р";
                              Discounted_price = "Ваша скидка: " + discounted_price + "Р";
                              if (int.Parse(SelectedItem1.AmountCart) == 0)
                              {
                                  Products2.Remove(SelectedItem1);
                                  Products3 = Products2;
                                  all_count = Products3.Count;
                                  Now_count = Products2.Count;
                              }
                          }

                      }

                      if (search == null)
                      {
                          search = "";
                      }
                      string Search1 = Search.ToUpper();

                      if (sDiscounts == "")
                      {
                          sDiscounts = "Все диапазоны";
                      }

                      if ((Products2 != null) && (Products3 != null))
                      {
                         Products2 = productService.updateProducts(sDiscounts, Products2, Products3, Search1);
                      }                
                      Products = productService.sCostsMeth(sCosts, Products);
                      Products2 = productService.sCostsMeth(sCosts, Products2);
                  }));
            }
        }

        private RelayCommand registrationManager_command;
        public RelayCommand RegistrationManager_command
        {
            get
            {
                return registrationManager_command ??
                  (registrationManager_command = new RelayCommand(obj =>
                  {
                      Global.role_registration = "менеджер";
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/registrationPage.xaml");
                  }));
            }
        }

        private RelayCommand registrationAdmin_command;
        public RelayCommand RegistrationAdmin_command
        {
            get
            {
                return registrationAdmin_command ??
                  (registrationAdmin_command = new RelayCommand(obj =>
                  {
                      Global.role_registration = "администратор";
                      NavigateViewModel navigateViewModel = new NavigateViewModel();
                      navigateViewModel.Navigate("Views/registrationPage.xaml");
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