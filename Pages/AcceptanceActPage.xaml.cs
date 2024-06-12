using System.Windows;
using System.Windows.Controls;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Pages;

/// <summary>
///     Логика взаимодействия для AcceptanceActPage.xaml
/// </summary>
public partial class AcceptanceActPage : Page
{
    public readonly Supply Supply;

    public AcceptanceActPage(Supply supply)
    {
        InitializeComponent();
        Supply = supply;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрытие текущего окна
        Window.GetWindow(this).Close();
    }

    public void ExportAcceptanceAct_Click(object sender, RoutedEventArgs e)
    {
        var helper = new WordHelper("Docs/AcceptanceAct.docx");

        var items = new Dictionary<string, string>
        {
            { "<DATE_NOW>", Supply.Date.ToString("dd-MM-yyyy HH:mm") },
            { "<SUPPLIER>", Supply.Supplier.Name },
            // { "<NAME>", Supply.Product.Name },
            { "<COUNT>", Supply.Count.ToString() },
            { "<NUM>", Supply.SupplyId.ToString() },
            { "<DATE>", Supply.Date.ToString("dd-MM-yyyy HH:mm") }
        };

        helper.Process(items);
    }
}