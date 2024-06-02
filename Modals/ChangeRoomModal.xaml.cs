using System.Windows;

namespace MedicalFurnitureAccounting.Modals;

public partial class ChangeRoomModal : Window
{
    public ChangeRoomModal()
    {
        InitializeComponent();
    }

    public string NewRoom { get; private set; }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверка, что введено название новой комнаты
        if (string.IsNullOrWhiteSpace(NewRoomTextBox.Text))
        {
            MessageBox.Show("Пожалуйста, введите название новой комнаты.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        NewRoom = NewRoomTextBox.Text;
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}