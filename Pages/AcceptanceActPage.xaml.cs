using System.Windows;
using System.Windows.Controls;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Pages;

/// <summary>
///     Логика взаимодействия для AcceptanceActPage.xaml
/// </summary>
public partial class AcceptanceActPage : Page
{
    public Supply Supply { get; }

    public AcceptanceActPage(Supply supply)
    {
        Supply = supply;
        InitializeComponent();
        DataContext = this; // Устанавливаем DataContext
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрытие текущего окна
        Window.GetWindow(this).Close();
    }

    private void ExportAcceptanceAct_Click(object sender, RoutedEventArgs e)
    {
        var helper = new WordHelper("Docs/AcceptanceAct.docx");

        var items = new Dictionary<string, string>
        {
            { "<DATE_NOW>", Supply.Date.ToString("dd-MM-yyyy") },
            { "<SUPPLIER>", Supply.Supplier.Name },
            { "<NAME>", Supply.ProductsString },
            { "<COUNT>", Supply.CountString },
            { "<NUM>", Supply.SupplyId.ToString() },
            { "<DATE>", Supply.Date.ToString("dd-MM-yyyy") }
        };

        helper.Process(items);
    }
}