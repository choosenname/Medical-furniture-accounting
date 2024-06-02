using System.Windows;
using System.Windows.Controls;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Pages;

/// <summary>
///     Логика взаимодействия для AcceptanceActPage.xaml
/// </summary>
public partial class AcceptanceActPage : Page
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
        Window.GetWindow(this).Close();
    }

    public void ExportAcceptanceAct_Click(object sender, RoutedEventArgs e)
    {
        var helper = new WordHelper("AcceptanceAct.docx");

        var items = new Dictionary<string, string>
        {
            { "<DATE_NOW>", DateTime.Now.ToString("dd-MM-yyyy HH:mm") },
            { "<SUPPLIER>", _product.LastSupply.Supplier.Name },
            { "<NAME>", _product.Name },
            { "<COUNT>", _product.LastSupply.Count.ToString() },
            { "<NUM>", _product.LastSupply.SupplyId.ToString() },
            { "<DATE>", _product.LastSupply.Date.ToString("dd-MM-yyyy HH:mm") }
        };

        helper.Process(items);
    }
}