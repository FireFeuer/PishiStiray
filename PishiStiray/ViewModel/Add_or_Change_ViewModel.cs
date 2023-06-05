using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PishiStiray.Data;
using PishiStiray.Servieces;
using PishiStiray.Views;
using PishiStiray.Model;
using System.Windows;
using DevExpress.Mvvm.POCO;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using iTextSharp.text.pdf.parser;
using System.IO;
using System.ComponentModel;
using Humanizer.Localisation;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;

namespace PishiStiray.ViewModel
{
    public class Add_or_Change_ViewModel : INotifyPropertyChanged
    {
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.ManufacturersTable> manufacturersDB;
        public Microsoft.EntityFrameworkCore.DbSet<PishiStiray.Model.SuppliersTable> providersDB;



        public ObservableCollection<String> unitOfMeasurements { get; set; }
        public ObservableCollection<String> productCategorys { get; set; }
        public ObservableCollection<String> manufacturers { get; set; }
        public ObservableCollection<String> providers { get; set; }

        public string  article_visibility { get; set; }

        public string add_or_change { get; set; }
        public string article { get; set; } // hint для артикля
        public string add_or_change_Button { get; set; } 

        public string productArticleNumber { get; set; }
        public string productName { get; set; }
        public string unitOfMeasurement { get; set; }
        public string cost { get; set; }
        public string discount { get; set; }
        public string manufacturer { get; set; }
        public string provider { get; set; }
        public int provider_number { get; set; }
        public string productCategory { get; set; }
        public string quantityStock { get; set; }
        public string description { get; set; }
        public string nowDiscount { get; set; }
        private string picture;
        public string Picture
        {
            get
            {
                return picture;
            }
            set
            {
                picture = value;
                NotifyPropertyChanged("Picture");
            }
        }



        public Add_or_Change_ViewModel() 
        {
            if (add_or_change == "Добавление товара")
            {
                article_visibility = "Visible";
            }
            if (add_or_change == "Изменение товара")
            {
                article_visibility = "Collapsed";
            }

                picture = "picture.png";
            unitOfMeasurements = new ObservableCollection<String>()
            {
                "уп.",
                "шт.",
            };

            productCategorys = new ObservableCollection<String>()
            {
                "Бумага офисная",
                "Для офиса",
                "Тетради школьные",
                "Школьные принадлежности"
            };

            manufacturers = new ObservableCollection<String>();
            using (TradeContext db = new TradeContext())
            {
                manufacturersDB = db.ManufacturersTables;
                foreach (ManufacturersTable mnf in manufacturersDB)
                {
                    manufacturers.Add(mnf.Name);
                }
            }

            providers = new ObservableCollection<String>();
            using (TradeContext db = new TradeContext())
            {
                providersDB = db.SuppliersTables;
                foreach (SuppliersTable prv in providersDB)
                {
                    providers.Add(prv.Name);
                }
            }

            add_or_change = Global.add_or_change;
            if(add_or_change == "Добавление товара")
            {
                article = "Укажите новый артикль";
                add_or_change_Button = "Добавить товар";
            }
            if(add_or_change == "Изменение товара")
            {
                article = "Укажите артикль товара, который хотите изменить";
                add_or_change_Button = "Изменить товар";

                productArticleNumber = Global.productArticleNumber.Replace("Артикль: ", "");
                productName = Global.productName;
                unitOfMeasurement ="шт.";
                cost = Global.cost.ToString();
                discount = Global.discount.Replace("Скидка: ", "");
                manufacturer = Global.manufacturer;
                provider = Global.provider;
                productCategory = Global.productCategory.ToString();
                quantityStock = Global.quantityStock.ToString().Replace("Товара на складе: ", "");
                description = Global.description;
                picture = Global.picture;
                nowDiscount = Global.nowDiscount.ToString();
                provider_number = Global.provider_number;
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
                      int prdctCategory = 1;
                      int unitOfMeas = 1;
                      using (TradeContext db = new TradeContext())
                      {
                          if(add_or_change == "Добавление товара")
                          {

                              int mnf_num = 0;
                                  manufacturersDB = db.ManufacturersTables;
                                  foreach (ManufacturersTable mnf in manufacturersDB)
                                  {
                                      if(manufacturer == mnf.Name)
                                    {
                                      mnf_num = mnf.Number;
                                    }
                                  }

                              int prv_num = 0;
                              providersDB = db.SuppliersTables;
                              foreach (SuppliersTable prv in providersDB)
                              {
                                  if (provider == prv.Name)
                                  {
                                      prv_num = prv.Number;
                                  }
                              }

                              
                              if(unitOfMeasurement == "уп.") { unitOfMeas = 1; }
                              if (unitOfMeasurement == "шт.") { unitOfMeas = 2; }

                          
                              if(productCategory == "Бумага офисная") { prdctCategory = 1; }
                              if (productCategory == "Для офиса") { prdctCategory = 2; }
                              if (productCategory == "Тетради школьные") { prdctCategory = 3; }
                              if (productCategory == "Школьные принадлежности") { prdctCategory = 4; }
                          if(productArticleNumber != null && productName != null && cost != null && discount != null && quantityStock != null &&
                              description != null && nowDiscount != null && unitOfMeasurement != null && manufacturer != null && provider != null 
                                  && productCategory != null)
                              {
                                  if (productArticleNumber.Replace(" ", "") != "" && productName.Replace(" ", "") != "" && cost.Replace(" ", "") != ""
                         && discount.Replace(" ", "") != "" && quantityStock.Replace(" ", "") != "" && description.Replace(" ", "") != "" &&
                             nowDiscount.Replace(" ", "") != "")
                                  {
                                      if (IsDigitsOnly(cost) && IsDigitsOnly(discount) && IsDigitsOnly(quantityStock) && IsDigitsOnly(nowDiscount))
                                      {
                                          try
                                          {
                                              var products = db.Products;
                                              Product product = new Product(productArticleNumber, productName, unitOfMeas, int.Parse(cost), discount, mnf_num.ToString(), prv_num, prdctCategory,
                                                  int.Parse(quantityStock), description, GetRelative(picture), int.Parse(nowDiscount), 0);
                                              products.Add(product);
                                              db.SaveChanges();

                                              NavigateViewModel navigateViewModel = new NavigateViewModel();
                                              navigateViewModel.Navigate("Views/tradePage.xaml");
                                          }
                                          catch
                                          {
                                              MessageBox.Show("Данный артикль уже существует");
                                          }

                                      }
                                      if (!IsDigitsOnly(cost))
                                      {
                                          MessageBox.Show("Корректно введите цену товара");
                                      }
                                      if (!IsDigitsOnly(discount))
                                      {
                                          MessageBox.Show("Корректно введите скидку товара");
                                      }
                                      if (!IsDigitsOnly(quantityStock))
                                      {
                                          MessageBox.Show("Корректно введите количество товара");
                                      }
                                      if (!IsDigitsOnly(nowDiscount))
                                      {
                                          MessageBox.Show("Корректно введите текущую скидку товара");
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
                          }
                          if (add_or_change == "Изменение товара") 
                          {
                              var products = db.Products;
                              var manufacturersTables = db.ManufacturersTables;
                              var providerTables = db.SuppliersTables;
                              string mnfct = "";
                              string provider1 = "";
                              if (productCategory == "Бумага офисная") { prdctCategory = 1; }
                              if (productCategory == "Для офиса") { prdctCategory = 2; }
                              if (productCategory == "Тетради школьные") { prdctCategory = 3; }
                              if (productCategory == "Школьные принадлежности") { prdctCategory = 4; }
                              foreach (ManufacturersTable manufacturer_index in manufacturersTables)
                              {
                                  if (manufacturer == manufacturer_index.Name)
                                  {
                                      mnfct = manufacturer_index.Number.ToString();
                                  }
                              }
                              foreach (SuppliersTable provider2 in providerTables)
                              {
                                  if (provider == provider2.Name)
                                  {
                                      provider1 = provider2.Number.ToString();
                                  }
                              }
                              if (productArticleNumber != null && productName != null && cost != null && discount != null && quantityStock != null &&
                           description != null && nowDiscount != null && unitOfMeasurement != null && manufacturer != null && provider != null
                               && productCategory != null)
                              {
                                  if (productArticleNumber.Replace(" ", "") != "" && productName.Replace(" ", "") != "" && cost.Replace(" ", "") != ""
                                                       && discount.Replace(" ", "") != "" && quantityStock.Replace(" ", "") != "" && description.Replace(" ", "") != "" &&
                                                           nowDiscount.Replace(" ", "") != "")
                                  {
                                      if(IsDigitsOnly(cost) && IsDigitsOnly(discount) && IsDigitsOnly(quantityStock) && IsDigitsOnly(nowDiscount))
                                      {
                                          foreach (Product product in products)
                                          {
                                              if (product.ProductArticleNumber == productArticleNumber)
                                              {


                                                  product.ProductName = productName;
                                                  product.UnitOfMeasurement = unitOfMeas;
                                                  product.Cost = int.Parse(cost);
                                                  product.Discount = discount;
                                                  product.Manufacturer = mnfct;
                                                  product.Provider = int.Parse(provider1);
                                                  product.ProductCategory = prdctCategory;
                                                  product.QuantityStock = int.Parse(quantityStock);
                                                  product.Description = description;
                                                  product.Picture = GetRelative(picture);
                                                  product.NowDiscount = int.Parse(nowDiscount);
                                              }
                                              NavigateViewModel navigateViewModel = new NavigateViewModel();
                                              navigateViewModel.Navigate("Views/tradePage.xaml");
                                          }
                                      }
                                      if(!IsDigitsOnly(cost))
                                      {
                                          MessageBox.Show("Корректно введите цену товара");
                                      }
                                      if (!IsDigitsOnly(discount))
                                      {
                                          MessageBox.Show("Корректно введите скидку товара");
                                      }
                                      if (!IsDigitsOnly(quantityStock))
                                      {
                                          MessageBox.Show("Корректно введите количество товара");
                                      }
                                      if (!IsDigitsOnly(nowDiscount))
                                      {
                                          MessageBox.Show("Корректно введите текущую скидку товара");
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
                            
                              db.SaveChanges();
                          }
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

        private RelayCommand set_image;
        public RelayCommand Set_image
        {
            get
            {
                return set_image ??
                  (set_image = new RelayCommand(obj =>
                  {
                      string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                      const string ImageFolderName = "Resources";
                      string ImageFolderFullName = System.IO.Path.Combine(BaseDirectory, ImageFolderName);

                      OpenFileDialog openFileDialog = new OpenFileDialog();
                      openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                      

                      if (openFileDialog.ShowDialog() == true) {
                      picture = openFileDialog.FileName;
                      NotifyPropertyChanged("Picture");

               
                          var asmName = new AssemblyNameDefinition("DynamicAssembly", new Version(1, 0, 0, 0));
                          var assembly = AssemblyDefinition.CreateAssembly(asmName, "<Module>", ModuleKind.Dll);

                          string imageFilePath = picture;
                          byte[] imageData = File.ReadAllBytes(imageFilePath);

                          var imageResource = new EmbeddedResource(GetRelative(picture), ManifestResourceAttributes.Private, imageData);
                          assembly.MainModule.Resources.Add(imageResource);

                          string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                          string assemblyPath = System.IO.Path.Combine(desktopDirectory, "DynamicAssembly.dll");

                          assembly.Write(assemblyPath);



                          if(!Directory.Exists(ImageFolderFullName))
                          {
                              Directory.CreateDirectory(ImageFolderFullName);
                              MessageBox.Show("Cоздал папку, которой не было");
                          }
                          string savepath = System.IO.Path.Combine(ImageFolderFullName, openFileDialog.SafeFileName);
                          File.Copy(openFileDialog.FileName, savepath, true);
                      }
                  }));
            }

        }

                      public string GetRelative(string path)
        {
            bool check = false;
          
            path = ReverseArrayFramework(path);
            for (int index = 0; index < path.Length; index++)
            {
                if(path[index] == '\\')
                {
                   check = true;
                }
                if (check)
                {
                    path = path.Remove(index);
                }

            }
            path = ReverseArrayFramework(path);
            return path;
        }
        static string ReverseArrayFramework(string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new String(arr);
        }
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string props)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(props));
        }
    }
}
