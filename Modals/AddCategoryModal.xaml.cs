using System.Windows;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddCategoryModal : Window
{
    public string CategoryName { get; private set; }

    public AddCategoryModal()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем имя категории из текстового поля
        CategoryName = CategoryNameTextBox.Text;
        // Закрываем окно
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}