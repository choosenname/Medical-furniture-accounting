using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MedicalFurnitureAccounting.Modals;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Pages;

/// <summary>
///     Логика взаимодействия для SupplierPage.xaml
/// </summary>
public partial class SupplierPage : Page
{
    private readonly ApplicationDBContext _context;

    public SupplierPage(ApplicationDBContext context)
    {
        InitializeComponent();
        _context = context;
        LoadCategories();
    }

    public ObservableCollection<Supply> Supplies { get; set; }

    private void LoadCategories()
    {
        Supplies = new ObservableCollection<Supply>(_context.Supplies.ToList());
        DataContext = this;
    }


    private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int suppliesId)
        {
            var supplyToDelete = Supplies.FirstOrDefault(p => p.SupplyId == suppliesId);
            if (supplyToDelete != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить поставку '{supplyToDelete.SupplyId}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove from database
                    _context.Supplies.Remove(supplyToDelete);
                    _context.SaveChanges();

                    // Remove from observable collection
                    Supplies.Remove(supplyToDelete);
                }
            }
        }
    }

    private void AddSupplyButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddSupplyModal(_context);
        if (addWindow.ShowDialog() == true)
        {
            var newModel = addWindow.Supply;
            _context.Supplies.Add(newModel);
            _context.SaveChanges();

            Supplies.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }
}