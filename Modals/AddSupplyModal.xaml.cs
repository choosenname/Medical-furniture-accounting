﻿using System.Collections.ObjectModel;
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
    public ObservableCollection<Product> Products { get; set; }

    public Supply Supply { get; private set; }

    private void LoadSuppliers()
    {
        Suppliers = new ObservableCollection<Supplier>(_dbContext.Suppliers.ToList());
        SupplierComboBox.ItemsSource = Suppliers;
        SupplierComboBox.DisplayMemberPath = "Name";

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

        var selectedSupplier = (Supplier)SupplierComboBox.SelectedItem;
        var selectedProduct = (Product)ProductComboBox.SelectedItem;

        Supply = new Supply
        (
            date: (DateTime)DatePicker.Value,
            count: Int32.Parse(CountTextBox.Text),
            supplier: selectedSupplier,
            product: selectedProduct
        );

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
        if (ProductComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedProduct = (Product)ProductComboBox.SelectedItem;

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