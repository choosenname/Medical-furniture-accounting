using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;
using System.Linq;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddSupplyModal : Window
{
    private readonly ApplicationDBContext _dbContext;
    private ObservableCollection<SupplyItem> _selectedProducts;

    public AddSupplyModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
        _selectedProducts = new ObservableCollection<SupplyItem>();
        SelectedProductsDataGrid.ItemsSource = _selectedProducts;
    }

    public ObservableCollection<Supplier> Suppliers { get; set; }
    public ObservableCollection<Product> Products { get; set; }

    public Supply Supply { get; private set; }

    private void LoadSuppliers()
    {
        Suppliers = new ObservableCollection<Supplier>(_dbContext.Suppliers.ToList());
        SupplierComboBox.ItemsSource = Suppliers;
        SupplierComboBox.DisplayMemberPath = "Name";

        Products = new ObservableCollection<Product>(_dbContext.Products.ToList());
        ProductListBox.ItemsSource = Products;
        ProductListBox.DisplayMemberPath = "Name";
    }

    private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        foreach (var c in e.Text)
            if (!char.IsDigit(c))
            {
                e.Handled = true; // Отменяем ввод символа, если он не является цифрой
                break;
            }
    }

    private void AddProductToSupplyButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedProducts = ProductListBox.SelectedItems.Cast<Product>().ToList();
        if (!selectedProducts.Any())
        {
            MessageBox.Show("Пожалуйста, выберите хотя бы один товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrEmpty(CountTextBox.Text) || !int.TryParse(CountTextBox.Text, out int count) || count <= 0)
        {
            MessageBox.Show("Пожалуйста, введите корректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        foreach (var product in selectedProducts)
        {
            var existingItem = _selectedProducts.FirstOrDefault(p => p.Product.ProductId == product.ProductId);
            if (existingItem != null)
            {
                existingItem.Count += count;
            }
            else
            {
                _selectedProducts.Add(new SupplyItem { Product = product, Count = count });
            }
        }

        ProductListBox.UnselectAll();
        CountTextBox.Clear();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (DatePicker.Value == null)
        {
            MessageBox.Show("Пожалуйста, выберите дату поставки.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        if (SupplierComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите поставщика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!_selectedProducts.Any())
        {
            MessageBox.Show("Пожалуйста, добавьте хотя бы один товар в поставку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedSupplier = (Supplier)SupplierComboBox.SelectedItem;

        Supply = new Supply
        {
            Date = (DateTime)DatePicker.Value,
            Supplier = selectedSupplier,
            SupplyItems = _selectedProducts.ToList()
        };

        GenerateAcceptanceAct(Supply);

        DialogResult = true;
    }

    private void GenerateAcceptanceAct(Supply supply)
    {
        // Создание страницы акта приема-передачи
        AcceptanceActPage acceptanceActPage = new AcceptanceActPage(supply);

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

    private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddSupplierModal();
        if (addWindow.ShowDialog() == true)
        {
            var name = addWindow.Name;
            var phone = addWindow.Phone;
            var email = addWindow.Email;
            var registrationNumber = addWindow.RegistrationNumber;
            var addres = addWindow.Addres;

            var newModel = new Supplier(name, phone, email,
                registrationNumber, addres);
            _dbContext.Suppliers.Add(newModel);
            _dbContext.SaveChanges();

            Suppliers.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }

    private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
    {
        if (SupplierComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите поставщика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedSupplier = (Supplier)SupplierComboBox.SelectedItem;

        var result = MessageBox.Show($"Вы уверены, что хотите удалить поставщика '{selectedSupplier.Name}'?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _dbContext.Suppliers.Remove(selectedSupplier);
            _dbContext.SaveChanges();

            Suppliers.Remove(selectedSupplier);
        }
    }

    private void AddProductButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddProductModal(_dbContext);
        if (addWindow.ShowDialog() == true)
        {
            var newModel = addWindow.Product;
            Products.Add(newModel);

            DataContext = null;
            DataContext = this;
        }
    }

    private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductListBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedProduct = (Product)ProductListBox.SelectedItem;

        var result = MessageBox.Show($"Вы уверены, что хотите удалить товар '{selectedProduct.Name}'?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _dbContext.Products.Remove(selectedProduct);
            _dbContext.SaveChanges();

            Products.Remove(selectedProduct);
        }
    }
}
