using System.ComponentModel.DataAnnotations;
using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddSupplierModal : Window
{
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string RegistrationNumber { get; private set; }
    public string Addres { get; private set; }
    public string Country { get; private set; }

    public AddSupplierModal()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем имя категории из текстового поля
        Name = SupplierNameTextBox.Text;
        Phone = SupplierPhoneTextBox.Text;
        Email = SupplierEmailTextBox.Text;
        RegistrationNumber = SupplierRegistrationNumberTextBox.Text;
        Addres = SupplierAddresTextBox.Text;
        Country = SupplierCountryTextBox.Text;
        // Закрываем окно
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}