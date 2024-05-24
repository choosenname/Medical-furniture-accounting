using System.Windows;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddCategoryModal : Window
{
    public string CategoryName { get; private set; }
    public double Allowance { get; private set; }

    public AddCategoryModal()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем имя категории из текстового поля
        CategoryName = CategoryNameTextBox.Text;

        // Пытаемся преобразовать текст из AllowanceNameTextBox в double
        if (double.TryParse(AllowanceNameTextBox.Text, out double allowance))
        {
            Allowance = allowance;
            // Закрываем окно
            DialogResult = true;
        }
        else
        {
            // Обработка ошибки: введено недопустимое значение
            MessageBox.Show("Введите корректное числовое значение для надбавки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}