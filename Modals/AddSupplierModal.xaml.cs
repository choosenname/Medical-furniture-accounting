using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddSupplierModal : Window
{
    public string Name { get; private set; }

    public AddSupplierModal()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем имя категории из текстового поля
        Name = SupplierNameTextBox.Text;
        // Закрываем окно
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}