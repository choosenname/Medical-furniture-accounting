using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddProductModal : Window
{
    private readonly ApplicationDBContext _dbContext;
    public NavigationService NavigationService { get; set; }

    public AddProductModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    public Product Product { get; private set; }

    private void LoadSuppliers()
    {
        var supplies = _dbContext.Supplies.ToList();

        SupplyComboBox.ItemsSource = supplies;
        SupplyComboBox.DisplayMemberPath = "Date";

        var categories = _dbContext.Categories.ToList();

        CategoryComboBox.ItemsSource = categories;
        CategoryComboBox.DisplayMemberPath = "MaxWeight";

        var materials = _dbContext.Materials.ToList();

        MaterialComboBox.ItemsSource = materials;
        MaterialComboBox.DisplayMemberPath = "MaxWeight";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductNameTextBox == null) return;

        var selectedSupply = (Supply)SupplyComboBox.SelectedItem;
        var selectedCategory = (Category)CategoryComboBox.SelectedItem;
        var materialCategory = (Material)MaterialComboBox.SelectedItem;

        Product = new Product
        {
            Name = ProductNameTextBox.Text,
            Suppply = selectedSupply,
            Category = selectedCategory,
            Material = materialCategory,
            Count = Convert.ToInt32(ProductCountTextBox.Text),
            Description = ProductDescriptionTextBox.Text,
            Width = Convert.ToInt32(ProductWidthTextBox.Text),
            Height = Convert.ToInt32(ProductHeightTextBox.Text),
            Length = Convert.ToInt32(ProductLengthTextBox.Text),
            Weight = Convert.ToInt32(ProductWeightTextBox.Text),
            Price = Convert.ToInt32(ProductPriceTextBox.Text)


        };
        GenerateAcceptanceAct(Product);

        DialogResult = true;
    }


    private void GenerateAcceptanceAct(Product product)
    {

            // Создание страницы акта приема-передачи
            AcceptanceActPage acceptanceActPage = new AcceptanceActPage(product);
            // Установка DataContext страницы на созданный акт приема-передачи
            acceptanceActPage.DataContext = product;

            // Отображение страницы в диалоговом окне
            var dialog = new Window
            {
                Content = acceptanceActPage,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                Title = "Acceptance Act"
            };

            dialog.ShowDialog();
    }





    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        foreach (char c in e.Text)
        {
            if (!char.IsDigit(c))
            {
                e.Handled = true; // Отменяем ввод символа, если он не является цифрой
                break;
            }
        }
    }
    public string GetSupplierNameById(int supplierId)
    {
        var supplier = _dbContext.Supplies
                                .Where(s => s.SupplierId == supplierId)
                                .Select(s => s.Supplier.Name)
                                .FirstOrDefault();
        return supplier != null ? supplier : "Unknown";
    }



}