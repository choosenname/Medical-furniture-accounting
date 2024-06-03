using System.Collections.ObjectModel;
using System.Windows;
using MedicalFurnitureAccounting.Modals;
using MedicalFurnitureAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalFurnitureAccounting;

/// <summary>
///     Логика взаимодействия для ChangeShelvingWindow.xaml
/// </summary>
public partial class ChangeShelvingWindow
{
    private readonly ApplicationDBContext _dbContext;
    private readonly Product _product;
    private ObservableCollection<Shelving> Shelving { get; set; }
    public ObservableCollection<StoreHistory> StoreHistories { get; set; }

    public ChangeShelvingWindow(ApplicationDBContext dbContext, Product product)
    {
        InitializeComponent();
        _dbContext = dbContext;
        _product = product;
        LoadSuppliers();

        StoreHistories = new ObservableCollection<StoreHistory>(product.StoreHistory);

        DataContext = this;
    }

    public int? NewShelvingId { get; private set; }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (ShelvingComboBox.SelectedItem != null)
        {
            NewShelvingId = ((Shelving)ShelvingComboBox.SelectedItem).ShelvingId;
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
        Shelving = new ObservableCollection<Shelving>(_dbContext.Shelving.ToList());
        ShelvingComboBox.ItemsSource = Shelving;
        ShelvingComboBox.DisplayMemberPath = "ShelvingId";
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void AddShelvingButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddShelvingModal(_dbContext);
        if (addWindow.ShowDialog() == true)
        {
            var name = addWindow.MaxWeight;
            var cell = addWindow.Cell;
            var newModel = new Shelving( maxWeight: name, cell: cell );
            _dbContext.StoreHistories.Add(new StoreHistory(_product, newModel));
            _dbContext.Shelving.Add(newModel);
            _dbContext.SaveChanges();

            Shelving.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }

    private void DeleteShelvingButton_Click(object sender, RoutedEventArgs e)
    {
        if (ShelvingComboBox.SelectedItem == null)
        {
            MessageBox.Show("Пожалуйста, выберите стеллаж.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var selectedShelving = (Shelving)ShelvingComboBox.SelectedItem;

        var result = MessageBox.Show($"Вы уверены, что хотите удалить стеллаж '{selectedShelving.ShelvingId}'?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _dbContext.Shelving.Remove(selectedShelving);
            _dbContext.SaveChanges();

            Shelving.Remove(selectedShelving);
        }
    }
}