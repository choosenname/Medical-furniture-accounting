using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddShelvingModal : Window
{
    private readonly ApplicationDBContext _dbContext;
    private ObservableCollection<Cell> Cells { get; set; }

    public AddShelvingModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadCell();
    }

    public int MaxWeight { get; private set; }
    public Cell Cell { get; private set; }

    private void LoadCell()
    {
        // Получаем список всех ячеек из базы данных
        Cells = new ObservableCollection<Cell>(_dbContext.Cell.ToList());

        // Заполняем ComboBox списком ячеек
        CellComboBox.ItemsSource = Cells;
        CellComboBox.DisplayMemberPath = "Number"; // Указываем, какое свойство использовать для отображения
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверка, что MaxWeight введен корректно
        if (!int.TryParse(MaxWeightTextBox.Text, out var maxWeight) || maxWeight <= 0)
        {
            MessageBox.Show("Введите корректное положительное числовое значение для максимального веса.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Проверка, что выбрана ячейка
        if (CellComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите ячейку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MaxWeight = maxWeight;
        Cell = (Cell)CellComboBox.SelectedItem;

        // Закрываем окно
        DialogResult = true;
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

    private void AddCellButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddCellModal();
        if (addWindow.ShowDialog() == true)
        {
            var name = addWindow.Number;
            var newModel = new Cell() { Number = name };
            _dbContext.Cell.Add(newModel);
            _dbContext.SaveChanges();

            Cells.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }

    private void DeleteCellButton_Click(object sender, RoutedEventArgs e)
    {
        if (CellComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите ячейку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedCell = (Cell)CellComboBox.SelectedItem;

        var result = MessageBox.Show($"Вы уверены, что хотите удалить ячейку '{selectedCell.Number}'?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _dbContext.Cell.Remove(selectedCell);
            _dbContext.SaveChanges();

            Cells.Remove(selectedCell);
        }
    }
}