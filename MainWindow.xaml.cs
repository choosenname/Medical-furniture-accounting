using System.Collections.ObjectModel;
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
using MedicalFurnitureAccounting.Modals;
using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;
using Word = Microsoft.Office.Interop.Word;


namespace MedicalFurnitureAccounting;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : System.Windows.Window
{
    private readonly ApplicationDBContext _dbContext;
    public MainWindow(ApplicationDBContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
    }
    private void OpenAddProductModal()
    {
        AddProductModal addProductModal = new AddProductModal(_dbContext); // Передача экземпляра _dbContext в конструктор
        addProductModal.ShowDialog();
        // Передача ссылки на NavigationService в AddProductModal
        addProductModal.NavigationService = MainFrame.NavigationService;
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

    private ObservableCollection<Product> GetProductsForExport()
    {
        // Получаем основное содержимое (Frame) из главного окна
        System.Windows.Controls.Frame mainFrame = MainFrame;

        // Предположим, что у вас есть страница "Продукты" и она содержит список продуктов
        ProductsPage productsPage = mainFrame.Content as ProductsPage;

        // Проверяем, была ли страница "Продукты" загружена
        if (productsPage != null)
        {
            // Получаем доступ к коллекции продуктов из страницы "Продукты"
            return productsPage.Products;
        }

        // Если страница "Продукты" не загружена или не найдена, вернем пустую коллекцию
        return new ObservableCollection<Product>();
    }


   



}