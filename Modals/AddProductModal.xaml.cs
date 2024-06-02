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
            ShelvingComboBox.SelectedItem == null)
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

        // Создание нового объекта Product
        Product = new Product
        {
            Name = ProductNameTextBox.Text.Trim(),
            Material = selectedMaterial,
            Shelving = selectedShelving,
            Count = count,
            Description = ProductDescriptionTextBox.Text.Trim(),
            Width = width,
            Height = height,
            Length = length,
            Weight = weight,
            Price = price
        };

        GenerateAcceptanceAct(Product);

        DialogResult = true;
    }

    private void GenerateAcceptanceAct(Product product)
    {
        // Создание страницы акта приема-передачи
        var acceptanceActPage = new AcceptanceActPage(product)
        {
            DataContext = product
        };

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
            var newModel = new Shelving() { MaxWeight = name, Cell = cell };
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
}