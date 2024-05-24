using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;

namespace MedicalFurnitureAccounting.Modals
{
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
            CategoryComboBox.DisplayMemberPath = "Name";

            var materials = _dbContext.Materials.ToList();
            MaterialComboBox.ItemsSource = materials;
            MaterialComboBox.DisplayMemberPath = "Name";

            var shelving = _dbContext.Shelving.ToList();
            SelvingComboBox.ItemsSource = shelving;
            SelvingComboBox.DisplayMemberPath = "ShelvingId";
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
            if (SupplyComboBox.SelectedItem == null ||
                CategoryComboBox.SelectedItem == null ||
                MaterialComboBox.SelectedItem == null ||
                SelvingComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите все необходимые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка, что числовые поля введены корректно
            if (!int.TryParse(ProductCountTextBox.Text, out int count) ||
                !int.TryParse(ProductWidthTextBox.Text, out int width) ||
                !int.TryParse(ProductHeightTextBox.Text, out int height) ||
                !int.TryParse(ProductLengthTextBox.Text, out int length) ||
                !int.TryParse(ProductWeightTextBox.Text, out int weight) ||
                !int.TryParse(ProductPriceTextBox.Text, out int price))
            {
                MessageBox.Show("Введите корректные числовые значения для всех полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedSupply = (Supply)SupplyComboBox.SelectedItem;
            var selectedCategory = (Category)CategoryComboBox.SelectedItem;
            var selectedMaterial = (Material)MaterialComboBox.SelectedItem;
            var selectedShelving = (Shelving)SelvingComboBox.SelectedItem;

            // Создание нового объекта Product
            Product = new Product
            {
                Name = ProductNameTextBox.Text.Trim(),
                Suppply = selectedSupply,
                Category = selectedCategory,
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
            AcceptanceActPage acceptanceActPage = new AcceptanceActPage(product)
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
            return supplier ?? "Unknown";
        }
    }
}
