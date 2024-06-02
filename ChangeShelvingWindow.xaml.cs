using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting;

/// <summary>
///     Логика взаимодействия для ChangeShelvingWindow.xaml
/// </summary>
public partial class ChangeShelvingWindow
{
    private readonly ApplicationDBContext _dbContext;

    public ChangeShelvingWindow(ApplicationDBContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        LoadSuppliers();
    }

    public int? NewShelvingId { get; private set; }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (SelvingComboBox.SelectedItem != null)
        {
            NewShelvingId = ((Shelving)SelvingComboBox.SelectedItem).ShelvingId;
            DialogResult = true;
        }
        else
        {
            MessageBox.Show("Пожалуйста, введите корректный номер стелажа.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void LoadSuppliers()
    {
        var shelving = _dbContext.Shelving.ToList();
        SelvingComboBox.ItemsSource = shelving;
        SelvingComboBox.DisplayMemberPath = "ShelvingId";
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}