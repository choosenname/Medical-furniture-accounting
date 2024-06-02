using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting;

/// <summary>
///     Логика взаимодействия для ChangeCellWindow.xaml
/// </summary>
public partial class ChangeCellWindow
{
    private readonly ApplicationDBContext _dbContext;

    public ChangeCellWindow(ApplicationDBContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        LoadCells();
    }

    public int? NewCellId { get; private set; }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (CellComboBox.SelectedItem != null)
        {
            NewCellId = ((Cell)CellComboBox.SelectedItem).CellId;
            DialogResult = true;
        }
        else
        {
            MessageBox.Show("Пожалуйста, выберите корректную ячейку.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void LoadCells()
    {
        var cells = _dbContext.Cell.ToList();
        CellComboBox.ItemsSource = cells;
        CellComboBox.DisplayMemberPath = "Number";
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}