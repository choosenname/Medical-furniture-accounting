using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MedicalFurnitureAccounting.Pages;

namespace MedicalFurnitureAccounting;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ApplicationDBContext _dbContext;
    public MainWindow(ApplicationDBContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new CategoryPage(_dbContext));
    } 
    private void Button_Click_Material_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new MaterialPage(_dbContext));
    }
    private void Button_Click_Product_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ProductsPage(_dbContext));
    } 
    private void Button_Click_Supplier_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new SupplierPage(_dbContext));
    }
    private void Button_Click_Supply_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new SupplyPage(_dbContext));
    }

}