using System.Windows;
using System.Windows.Input;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddCellModal : Window
{
    public AddCellModal()
    {
        InitializeComponent();
    }

    public int Number { get; private set; }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверяем, что текстовое поле не пустое
        if (string.IsNullOrEmpty(NumberTextBox.Text))
        {
            MessageBox.Show("Номер ячейки не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Пытаемся преобразовать текст в int
        if (int.TryParse(NumberTextBox.Text, out var number))
        {
            // Проверка, что номер ячейки находится в допустимом диапазоне
            if (number <= 0)
            {
                MessageBox.Show("Номер ячейки должен быть положительным числом.", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            Number = number;
            // Закрываем окно
            DialogResult = true;
        }
        else
        {
            // Обработка ошибки: введено недопустимое значение
            MessageBox.Show("Введите корректное числовое значение для номера ячейки.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
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

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}