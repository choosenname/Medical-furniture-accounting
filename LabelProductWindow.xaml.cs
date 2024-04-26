﻿using MedicalFurnitureAccounting.Models;
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

        public LabelProductWindow(Product product)
        {
            InitializeComponent();
            Product = product;
            DataContext = Product;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var helper = new WordHalper("Lable.docx");

            var items = new Dictionary<string, string>
            {
                {"<NAME>", Product.Name  },
                { "<COUNT>", Product.Count.ToString()},
                { "<MATERIAL>",Product.Material.Name},
                { "<CATEGORY>", Product.Category.Name},
                { "<ID>", Product.ProductId.ToString()}
            };

            helper.Process(items);
        }
    }
}
