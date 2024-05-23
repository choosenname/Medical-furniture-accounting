using MedicalFurnitureAccounting.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace MedicalFurnitureAccounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AcceptanceActPage.xaml
    /// </summary>
    public partial class AcceptanceActPage : System.Windows.Controls.Page
    {
        private readonly Product _product;

        public AcceptanceActPage(Product product)
        {
            InitializeComponent();
            _product = product;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие текущего окна
            System.Windows.Window.GetWindow(this).Close();
        }
        public void ExportAcceptanceAct_Click(object sender, RoutedEventArgs e)
        {
            var helper = new WordHalper("AcceptanceAct.docx");

            var items = new Dictionary<string, string>
            {
                {"<DATE_NOW>", DateTime.Now.ToString("dd-MM-yyyy HH:mm")  },
                { "<SUPPLIER>", _product.Suppply.Supplier.Name},
                { "<NAME>", _product.Name},
                { "<COUNT>", _product.Count.ToString()},
                { "<NUM>", _product.Suppply.SupplyId.ToString()},
                { "<DATE>", _product.Suppply.Date.ToString("dd-MM-yyyy HH:mm")}
            };

            helper.Process(items);
        }

    }
}
