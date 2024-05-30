using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Word;
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

namespace MedicalFurnitureAccounting
{
    /// <summary>
    /// Логика взаимодействия для LabelProduct.xaml
    /// </summary>
    public partial class LabelProductWindow : System.Windows.Window
    {
        public Product Product { get; set; }
        private readonly Storekeeper user;

        public LabelProductWindow(Product product, Storekeeper user)
        {
            InitializeComponent();
            Product = product;
            DataContext = Product;
            this.user = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var helper = new WordHalper("Lable.doc");

            var items = new Dictionary<string, string>
            {
                {"<NAME>", Product.Name  },
                { "<COUNT>", Product.Count.ToString()},
                { "<MATERIAL>",Product.Material.Name},
                { "<CATEGORY>", Product.Category.Name},
                { "<ID>", Product.ProductId.ToString()},
                /*{ "<ORGANIZATION>", Product.Suppply.Supplier.Name},
                { "<ADRESS>", Product.Suppply.Supplier.Addres},
                { "<PHONE>", Product.Suppply.Supplier.Phone},
                { "<EMAIL>", Product.Suppply.Supplier.Email},*/
                { "<WIDTH>", Product.Width.ToString()},
                { "<LENGTH>", Product.Length.ToString()},
                { "<HEIGHT>", Product.Height.ToString()},
                { "<WEIGHT>", Product.Weight.ToString()},
                { "<PRICE>", Product.Price.ToString()},
                { "<USER_NAME>", user.Name },
                { "<DATE_NOW>", DateTime.Now.ToString() },
            };

            helper.Process(items);
        }
    }
}
