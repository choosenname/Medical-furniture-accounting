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
    private readonly Storekeeper user;

    public MainWindow(ApplicationDBContext dbContext, Storekeeper user)
    {
        InitializeComponent();
        _dbContext = dbContext;
        this.user = user;
        MainFrame.Navigate(new ProductsPage(_dbContext, user));
    }
    private void OpenAddProductModal()
    {
        AddProductModal addProductModal = new AddProductModal(_dbContext); // �������� ���������� _dbContext � �����������
        addProductModal.ShowDialog();
        // �������� ������ �� NavigationService � AddProductModal
        addProductModal.NavigationService = MainFrame.NavigationService;
    }
    private void Button_Click_Product_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ProductsPage(_dbContext, user));
    } 
    private void Button_Click_Supplier_Page(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new SupplierPage(_dbContext));
    }

    private ObservableCollection<Product> GetProductsForExport()
    {
        // �������� �������� ���������� (Frame) �� �������� ����
        System.Windows.Controls.Frame mainFrame = MainFrame;

        // �����������, ��� � ��� ���� �������� "��������" � ��� �������� ������ ���������
        ProductsPage productsPage = mainFrame.Content as ProductsPage;

        // ���������, ���� �� �������� "��������" ���������
        if (productsPage != null)
        {
            // �������� ������ � ��������� ��������� �� �������� "��������"
            return productsPage.Products;
        }

        // ���� �������� "��������" �� ��������� ��� �� �������, ������ ������ ���������
        return new ObservableCollection<Product>();
    }


   



}