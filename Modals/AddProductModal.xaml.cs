using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddProductModal : Window
{
    private readonly ApplicationDBContext _dbContext;
    private ObservableCollection<Material> Materials { get; set; }
    private ObservableCollection<Shelving> Shelving { get; set; }
    public ObservableCollection<Category> Categories { get; set; }

    public Product Product { get; private set; }


    public AddProductModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    private void LoadSuppliers()
    {
        Materials = new ObservableCollection<Material>(_dbContext.Materials.ToList());
        MaterialComboBox.ItemsSource = Materials;
        MaterialComboBox.DisplayMemberPath = "Name";

        Shelving = new ObservableCollection<Shelving>(_dbContext.Shelving.ToList());
        ShelvingComboBox.ItemsSource = Shelving;
        ShelvingComboBox.DisplayMemberPath = "ShelvingId";

        Categories = new ObservableCollection<Category>(_dbContext.Categories.ToList());
        CategoryComboBox.ItemsSource = Categories;
        CategoryComboBox.DisplayMemberPath = "Name";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверка, что имя продукта не пустое
        if (string.IsNullOrEmpty(ProductNameTextBox.Text.Trim()))
        {
            MessageBox.Show("Имя продукта не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Проверка, что выбраны все обязательные поля
        if (MaterialComboBox.SelectedItem == null ||
            ShelvingComboBox.SelectedItem == null || CategoryComboBox == null)
        {
            MessageBox.Show("Пожалуйста, выберите все необходимые поля.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        // Проверка, что числовые поля введены корректно
        if (!int.TryParse(ProductCountTextBox.Text, out var count) ||
            !int.TryParse(ProductWidthTextBox.Text, out var width) ||
            !int.TryParse(ProductHeightTextBox.Text, out var height) ||
            !int.TryParse(ProductLengthTextBox.Text, out var length) ||
            !int.TryParse(ProductWeightTextBox.Text, out var weight) ||
            !int.TryParse(ProductPriceTextBox.Text, out var price))
        {
            MessageBox.Show("Введите корректные числовые значения для всех полей.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var selectedMaterial = (Material)MaterialComboBox.SelectedItem;
        var selectedShelving = (Shelving)ShelvingComboBox.SelectedItem;
        var selectedCategory = (Category)CategoryComboBox.SelectedItem;

        // Создание нового объекта Product
        Product = new Product
        (
            name: ProductNameTextBox.Text.Trim(),
            material: selectedMaterial,
            shelving: selectedShelving,
            category: selectedCategory,
            description: ProductDescriptionTextBox.Text.Trim(),
            width: width,
            height: height,
            length: length,
            weight: weight,
            price: price
        );

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
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

    private void AddMaterialButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddMaterialModal();
        if (addWindow.ShowDialog() == true)
        {
            var newMaterial = addWindow.Material;
            _dbContext.Materials.Add(newMaterial);
            _dbContext.SaveChanges();

            Materials.Add(newMaterial);
            DataContext = null;
            DataContext = this;
        }
    }
    
    private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
    {
        if (MaterialComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите материал.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedMaterial = (Material)MaterialComboBox.SelectedItem;

        var result = MessageBox.Show($"Вы уверены, что хотите удалить материал '{selectedMaterial.Name}'?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _dbContext.Materials.Remove(selectedMaterial);
            _dbContext.SaveChanges();

            Materials.Remove(selectedMaterial);
        }
    }

    private void AddShelvingButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddShelvingModal(_dbContext);
        if (addWindow.ShowDialog() == true)
        {
            var name = addWindow.MaxWeight;
            var cell = addWindow.Cell;
            var newModel = new Shelving( maxWeight: name, cell: cell );
            _dbContext.Shelving.Add(newModel);
            _dbContext.SaveChanges();

            Shelving.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }

    private void DeleteShelvingButton_Click(object sender, RoutedEventArgs e)
    {
        if (ShelvingComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите стеллаж.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedShelving = (Shelving)ShelvingComboBox.SelectedItem;

        var result = MessageBox.Show($"Вы уверены, что хотите удалить стеллаж '{selectedShelving.ShelvingId}'?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _dbContext.Shelving.Remove(selectedShelving);
            _dbContext.SaveChanges();

            Shelving.Remove(selectedShelving);
        }
    }

    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        var addCategoryWindow = new AddCategoryModal();
        if (addCategoryWindow.ShowDialog() == true)
        {
            var categoryName = addCategoryWindow.CategoryName;
            var allowedPrice = addCategoryWindow.Allowance;

            var newCategory = new Category(name: categoryName);
            _dbContext.Categories.Add(newCategory);
            _dbContext.SaveChanges();

            Categories.Add(newCategory);
            DataContext = null;
            DataContext = this;
        }
    }

    private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (CategoryComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedCategory = (Category)CategoryComboBox.SelectedItem;

        var result = MessageBox.Show($"Вы уверены, что хотите удалить категорию '{selectedCategory.Name}'?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _dbContext.Categories.Remove(selectedCategory);
            _dbContext.SaveChanges();

            Categories.Remove(selectedCategory);
        }
    }
}