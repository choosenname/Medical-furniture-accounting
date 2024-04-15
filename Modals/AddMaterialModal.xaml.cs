using System.Windows;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddMaterialModal : Window
{
    public string Name { get; private set; }

    public AddMaterialModal()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем имя категории из текстового поля
        Name = MaterialNameTextBox.Text;
        // Закрываем окно
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}