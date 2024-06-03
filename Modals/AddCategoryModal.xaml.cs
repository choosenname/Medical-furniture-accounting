using System.Windows;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddCategoryModal
{
    public AddCategoryModal()
    {
        InitializeComponent();
    }

    public string CategoryName { get; private set; }
    public double Allowance { get; }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем имя категории из текстового поля
        CategoryName = CategoryNameTextBox.Text.Trim();

        // Проверка, что имя категории не пустое
        if (string.IsNullOrEmpty(CategoryName))
        {
            MessageBox.Show("Имя категории не может быть пустым.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}