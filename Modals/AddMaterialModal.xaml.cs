using System.Windows;
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
        };

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}