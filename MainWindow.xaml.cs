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
        AddProductModal addProductModal = new AddProductModal(_dbContext); // �������� ���������� _dbContext � �����������
        addProductModal.ShowDialog();
        // �������� ������ �� NavigationService � AddProductModal
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


    private void ExportToWord_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // ������� ������ ���������� Word
            Word.Application wordApp = new Word.Application();

            // ������� ����� �������� Word
            Word.Document doc = wordApp.Documents.Add();

            // ��������� ���������
            Word.Paragraph title = doc.Content.Paragraphs.Add();
            title.Range.Text = "Medical Furniture Accounting Report";
            title.Range.Font.Bold = 1;
            title.Range.Font.Size = 16;
            title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            // ��������� ����������
            Word.Paragraph content = doc.Content.Paragraphs.Add();
            // ����� �������� ����������, ������� �� ������ ��������������
            content.Range.Text = "Sample content to export to Word document.";

            // ��������� ��� ����� � ������� �����
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string filename = @"C:\Users\xxsam\source\repos\medical-furniture-accounting\Act_" + currentDate + ".docx";

            // ��������� �������� Word
            doc.SaveAs2(filename);

            // ��������� �������� Word � ��������� ������ ����������
            doc.Close();
            wordApp.Quit();

            // ����������� �� �������� �������� ���������
            MessageBox.Show("�������� ������� ������ � �������� �� ���������� ����:\n" + filename, "�����", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            // ��������� ������
            MessageBox.Show("������ ��� �������� ���������: " + ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



}