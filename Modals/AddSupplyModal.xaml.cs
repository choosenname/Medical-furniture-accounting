using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals
{
    public partial class AddSupplyModal : Window
    {
        private readonly ApplicationDBContext _dbContext;
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public AddSupplyModal(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            LoadSuppliers();
        }

        public Supply Supply { get; private set; }

        private void LoadSuppliers()
        {
            Suppliers = new ObservableCollection<Supplier>(_dbContext.Suppliers.ToList());
            SupplierComboBox.ItemsSource = Suppliers;
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

        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddSupplierModal();
            if (addWindow.ShowDialog() == true)
            {
                string name = addWindow.Name;
                string phone = addWindow.Phone;
                string email =addWindow.Email;
                string registrationNumber = addWindow.RegistrationNumber;
                string addres = addWindow.Addres;
                string country = addWindow.Country;

                var newModel = new Supplier(name: name, phone: phone, email: email,
                    registrationNumber: registrationNumber, addres: addres, country: country);
                _dbContext.Suppliers.Add(newModel);
                _dbContext.SaveChanges();

                Suppliers.Add(newModel);
                DataContext = null;
                DataContext = this;
            }
        }
    }
}
