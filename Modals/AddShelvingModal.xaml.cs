using System.Windows;
using System.Windows.Input;
using DocumentFormat.OpenXml.InkML;
using MedicalFurnitureAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddShelvingModal : Window
{
    public int MaxWeight { get; private set; }
    public Cell Cell { get; private set; }

    private readonly ApplicationDBContext _dbContext;
    public AddShelvingModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadCell();
    }

    private void LoadCell()
    {
        // Получаем список всех поставщиков из базы данных
        var suppliers = _dbContext.Cell.ToList();

        // Заполняем ComboBox списком поставщиков
        CellComboBox.ItemsSource = suppliers;
        CellComboBox.DisplayMemberPath = "Number"; // Указываем, какое свойство использовать для отображения
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем имя категории из текстового поля
        MaxWeight = Int32.Parse(SupplierNameTextBox.Text);

        Cell = (Cell)CellComboBox.SelectedItem;

        // Закрываем окно
        DialogResult = true;
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

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}