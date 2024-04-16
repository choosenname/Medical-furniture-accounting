using System.Windows;

namespace MedicalFurnitureAccounting.Modals;

public partial class ChangeRoomModal : Window
{
    public string NewRoom { get; private set; }

    public ChangeRoomModal()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        NewRoom = NewRoomTextBox.Text;
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}