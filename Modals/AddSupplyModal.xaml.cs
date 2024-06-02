using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals
{
    public partial class AddSupplyModal : Window
    {
        private readonly ApplicationDBContext _dbContext;

        public AddSupplyModal(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            LoadSuppliers();
        }

        public Supply Supply { get; private set; }

        private void LoadSuppliers()
        {
            var suppliers = _dbContext.Suppliers.ToList();
            SupplierComboBox.ItemsSource = suppliers;
            SupplierComboBox.DisplayMemberPath = "Name";

            var categories = _dbContext.Categories.ToList();
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "Name";

            var product = _dbContext.Products.ToList();
            ProductComboBox.ItemsSource = product;
            ProductComboBox.DisplayMemberPath = "Name";
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что дата выбрана
            if (DatePicker.Value == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату поставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
