using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddMaterialModal : Window
{
    public AddMaterialModal()
    {
        InitializeComponent();
    }

    public Material Material { get; private set; }
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {

        Material = new Material
        {
            Name = MaterialNameTextBox.Text,
            Price = Convert.ToInt32(MaterialPriceTextBox.Text),
        };

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }

    private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        foreach (char c in e.Text)
        {
            if (!char.IsDigit(c))
            {
                e.Handled = true; // Отменяем ввод символа, если он не является цифрой
                break;
            }
        }
    }
}