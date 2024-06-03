using System.Windows;

namespace MedicalFurnitureAccounting;

/// <summary>
///     Логика взаимодействия для ChangeCountWindow.xaml
/// </summary>
public partial class ChangeCountWindow
{
    public ChangeCountWindow()
    {
        InitializeComponent();
    }

    public int? NewCount { get; private set; }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(CountTextBox.Text, out var newCount) && newCount >= 0)
        {
            NewCount = newCount;
            DialogResult = true;
        }
        else
        {
            MessageBox.Show("Пожалуйста, введите корректное количество.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}