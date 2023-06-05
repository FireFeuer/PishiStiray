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
using Mono.Cecil.Cil;


namespace PishiStiray.Servieces
{
    public  class ProductService
    {
        public ObservableCollection<ProductE> sCostsMeth(string sCosts, ObservableCollection<ProductE> Products) // сортровка
        {          
                if (sCosts == "По возрастанию")
                {
                    Products = new ObservableCollection<ProductE>(Products.OrderBy(i => i.Cost));
                }
                if (sCosts == "По убыванию")
                {
                    Products = new ObservableCollection<ProductE>(Products.OrderByDescending(i => i.Cost));
                }
            return Products;            
        }

        public ObservableCollection<ProductE> updateProducts(string sDiscounts, ObservableCollection<ProductE> Products, ObservableCollection<ProductE> Products1,
            string Search1)
        {
            if (sDiscounts == "Все диапазоны")
            {
                Products = new ObservableCollection<ProductE>(Products1.Where(i => i.ProductName.ToUpper().Contains(Search1)));
            }
            if (sDiscounts == "0 - 9%")
            {
                Products = new ObservableCollection<ProductE>(Products1.Where(i => i.NowDiscount >= 0 && i.NowDiscount < 10 && i.ProductName.ToUpper().Contains(Search1)));
            }
            if (sDiscounts == "10 - 14%")
            {
                Products = new ObservableCollection<ProductE>(Products1.Where(i => i.NowDiscount >= 10 && i.NowDiscount < 15 && i.ProductName.ToUpper().Contains(Search1)));
            }
            if (sDiscounts == "15% и более")
            {
                Products = new ObservableCollection<ProductE>(Products1.Where(i => i.NowDiscount >= 15 && i.ProductName.ToUpper().Contains(Search1)));
            }
            return Products;
        }
        public ObservableCollection<ProductE> viewProductsFromOrder(ObservableCollection<ProductE> Products_order, Order1E SelectedItem_orders, ObservableCollection<ProductE> Products1, string visible_line)
        {
            Products_order.Clear();
            using (TradeContext db = new TradeContext())
            {
                var orderProducts = db.Orderproducttrues;
                List<List<string>> articlePlusSum = new List<List<string>>();
                foreach (var orderProduct in orderProducts)
                {
                    if (SelectedItem_orders.OrderId == orderProduct.Id)
                    {
                        List<string> item = new List<string>();
                        item.Add(orderProduct.Article);
                        item.Add(orderProduct.Sum);
                        articlePlusSum.Add(item);
                    }
                }
                foreach (var product in Products1)
                {
                    string productCost = product.Cost.Replace("Старая цена: ", "");
                    double cost = int.Parse(productCost.Replace("Р", ""));
                    double nowdiscount = product.NowDiscount;
                    foreach (var item in articlePlusSum)
                    {
                        if (item[0] == product.ProductArticleNumber.Replace("Артикль: ", ""))
                        {
                            if (product.Status == 0)
                            {
                                if (product.NowDiscount > 0)
                                {
                                    visible_line = "Visible";
                                    Products_order.Add(new ProductE
                                    {
                                        Cost = (cost - (cost / 100 * nowdiscount)).ToString() + "Р",
                                        Description = product.Description,
                                        Discount = product.Discount,
                                        Manufacturer = "",
                                        NowDiscount = product.NowDiscount,
                                        Picture = product.Picture,
                                        ProductArticleNumber = product.ProductArticleNumber,
                                        ProductCategory = "sad",
                                        ProductName = product.ProductName,
                                        Provider = "sad",
                                        QuantityStock = "sad",
                                        UnitOfMeasurement = product.UnitOfMeasurement,
                                        Status = product.Status,
                                        AmountCart = "Кол-во товара: " + item[1]
                                    });
                                }
                                else
                                {
                                    visible_line = "Hidden";
                                    Products_order.Add(new ProductE
                                    {
                                        Cost = (cost - (cost / 100 * nowdiscount)).ToString().Replace("Р", "") + "Р",
                                        Description = product.Description,
                                        Discount = product.Discount,
                                        Manufacturer = "",
                                        NowDiscount = product.NowDiscount,
                                        Picture = product.Picture,
                                        ProductArticleNumber = product.ProductArticleNumber,
                                        ProductCategory = "sad",
                                        ProductName = product.ProductName,
                                        Provider = "sad",
                                        QuantityStock = "sad",
                                        UnitOfMeasurement = product.UnitOfMeasurement,
                                        AmountCart = "Кол-во товара: " + item[1]
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return Products_order;
        }
        public void pdfCreate(int id, bool three_or_six, ProductE SelectedItem, ObservableCollection<ProductE> Products2, ObservableCollection<ProductE> Products3, string Discounted_price, string Total_discount, string SpointIssue, int code)
        {
            FileStream fs = new FileStream("Product receipt.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font f = new Font(bf, 9, Font.NORMAL);
            doc.Add(new iTextSharp.text.Paragraph("Товарный чек: " + id, f));
            doc.Add(new iTextSharp.text.Paragraph("Дата заказа: " + DateTime.Now.ToString("dd.MM.yyyy"), f));
            if (three_or_six == true)
            {
                doc.Add(new iTextSharp.text.Paragraph("Дата выдачи: " + DateTime.Now.AddDays(6).ToString("dd.MM.yyyy"), f));
            }
            if (three_or_six == false)
            {
                doc.Add(new iTextSharp.text.Paragraph("Дата выдачи: " + DateTime.Now.AddDays(3).ToString("dd.MM.yyyy"), f));
            }
            id = id + 1;
            string productCost = SelectedItem.Cost.Replace("Старая цена: ", "");
            doc.Add(new iTextSharp.text.Paragraph("", f));
            doc.Add(new iTextSharp.text.Paragraph("Состав заказа:", f));
            foreach (var product in Products2)
            {
                doc.Add(new iTextSharp.text.Paragraph("• Название товара: " + product.ProductName + "\n     Описание товара: " + product.Description
                    + "\n     Цена за товар: " + productCost.Replace("Р", "") + "Р" + "\n     Кол-во товара: " + product.AmountCart + "\n     Цена: " + int.Parse(productCost.Replace("Р", "")) * int.Parse(product.AmountCart) + "Р", f));
            }
            doc.Add(new iTextSharp.text.Paragraph("", f));
            doc.Add(new iTextSharp.text.Paragraph(Discounted_price, f));
            doc.Add(new iTextSharp.text.Paragraph(Total_discount, f));
            doc.Add(new iTextSharp.text.Paragraph("Пункт выдачи: " + SpointIssue, f));
            doc.Add(new iTextSharp.text.Paragraph("Код получения: " + code, f));


            doc.Close();
            Products2.Clear();
            Products3.Clear();
            try
            {
                using (Process process = new())
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = "Product receipt.pdf";
                    process.Start();
                }
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    MessageBox.Show(noBrowser.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
