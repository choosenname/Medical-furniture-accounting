using System.Windows;
using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;

namespace MedicalFurnitureAccounting;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly ApplicationDBContext _dbContext;
    private readonly Storekeeper _user;

    public MainWindow(ApplicationDBContext dbContext, Storekeeper user)
    {
        InitializeComponent();
        _dbContext = dbContext;
        this._user = user;
        MainFrame.Navigate(new ProductsPage(_dbContext, user));
    }

    private void Button_Click_Product_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ProductsPage(_dbContext, _user));
    }

    private void Button_Click_Supplier_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new SupplierPage(_dbContext));
    }
}