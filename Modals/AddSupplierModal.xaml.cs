using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddSupplierModal : Window
{
    public AddSupplierModal()
    {
        InitializeComponent();
    }

    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string RegistrationNumber { get; private set; }
    public string Addres { get; private set; }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем данные из текстовых полей
        Name = SupplierNameTextBox.Text.Trim();
        Phone = SupplierPhoneTextBox.Text.Trim();
        Email = SupplierEmailTextBox.Text.Trim();
        RegistrationNumber = SupplierRegistrationNumberTextBox.Text.Trim();
        Addres = SupplierAddresTextBox.Text.Trim();

        // Проверка на пустые значения
        if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Email) ||
            string.IsNullOrEmpty(RegistrationNumber) || string.IsNullOrEmpty(Addres))
        {
            MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Проверка корректности номера телефона
        if (!IsValidPhone(Phone))
        {
            MessageBox.Show("Введите корректный номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Проверка корректности email
        if (!IsValidEmail(Email))
        {
            MessageBox.Show("Введите корректный email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Проверка корректности адреса
        if (!IsValidAddress(Addres))
        {
            MessageBox.Show("Введите корректный адрес.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Закрываем окно
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
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

    private bool IsValidPhone(string phone)
    {
        // Проверка номера телефона с использованием регулярного выражения
        var phonePattern = @"^\+?\d{10,15}$";
        return Regex.IsMatch(phone, phonePattern);
    }

    private bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }

    private bool IsValidAddress(string address)
    {
        // Пример простой проверки адреса (можно улучшить при необходимости)
        var addressPattern = @"^[a-zA-Z0-9\s,.-]+$";
        return Regex.IsMatch(address, addressPattern);
    }
}
