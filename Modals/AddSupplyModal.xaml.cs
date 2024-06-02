using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddSupplyModal : Window
{
    private readonly ApplicationDBContext _dbContext;

    public AddSupplyModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    public ObservableCollection<Supplier> Suppliers { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<Product> Products { get; set; }

    public Supply Supply { get; private set; }

    private void LoadSuppliers()
    {
        Suppliers = new ObservableCollection<Supplier>(_dbContext.Suppliers.ToList());
        SupplierComboBox.ItemsSource = Suppliers;
        SupplierComboBox.DisplayMemberPath = "Name";

        Categories = new ObservableCollection<Category>(_dbContext.Categories.ToList());
        CategoryComboBox.ItemsSource = Categories;
        CategoryComboBox.DisplayMemberPath = "Name";

        Products = new ObservableCollection<Product>(_dbContext.Products.ToList());
        ProductComboBox.ItemsSource = Products;
        ProductComboBox.DisplayMemberPath = "Name";
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

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверка, что дата выбрана
        if (DatePicker.Value == null)
        {
            MessageBox.Show("Пожалуйста, выберите дату поставки.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        // Проверка, что выбран поставщик
        if (SupplierComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите поставщика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Получаем выбранного поставщика из ComboBox
        var selectedSupplier = (Supplier)SupplierComboBox.SelectedItem;

        // Создаем новую поставку
        Supply = new Supply
        {
            Date = (DateTime)DatePicker.Value,
            Supplier = selectedSupplier
        };

        DialogResult = true;
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
            var country = addWindow.Country;

            var newModel = new Supplier(name, phone, email,
                registrationNumber, addres, country);
            _dbContext.Suppliers.Add(newModel);
            _dbContext.SaveChanges();

            Suppliers.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }

    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        var addCategoryWindow = new AddCategoryModal();
        if (addCategoryWindow.ShowDialog() == true)
        {
            var categoryName = addCategoryWindow.CategoryName;
            var allowedPrice = addCategoryWindow.Allowance;

            var newCategory = new Category { Name = categoryName, Allowance = allowedPrice };
            _dbContext.Categories.Add(newCategory);
            _dbContext.SaveChanges();

            Categories.Add(newCategory);
            DataContext = null;
            DataContext = this;
        }
    }

    private void AddProductButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddProductModal(_dbContext);
        if (addWindow.ShowDialog() == true)
        {
            var newModel = addWindow.Product;

            // Если товар не существует, добавляем новый товар в коллекцию
            _dbContext.Products.Add(newModel);
            _dbContext.SaveChanges();
            Products.Add(newModel);

            DataContext = null;
            DataContext = this;
        }
    }
}