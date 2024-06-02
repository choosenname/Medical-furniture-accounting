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

    public ObservableCollection<Supplier> Suppliers { get; set; }
    public ObservableCollection<Supply> Supplies { get; set; }

    private void LoadCategories()
    {
        Suppliers = new ObservableCollection<Supplier>(_context.Suppliers.ToList());
        Supplies = new ObservableCollection<Supply>(_context.Supplies.ToList());
        DataContext = this;
    }

    private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddSupplierModal();
        if (addWindow.ShowDialog() == true)
        {
            var name = addWindow.Name;
            var phone = addWindow.Phone;
            var email = addWindow.Email;
            var registrationNumber = addWindow.RegistrationNumber;
            var addres = addWindow.Addres;
            var country = addWindow.Country;

            var newModel = new Supplier(name, phone, email,
                registrationNumber, addres, country);
            _context.Suppliers.Add(newModel);
            _context.SaveChanges();

            Suppliers.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }


    private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int supplierId)
        {
            var supplierToDelete = Suppliers.FirstOrDefault(p => p.SupplierId == supplierId);
            if (supplierToDelete != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить поставщика '{supplierToDelete.Name}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove from database
                    _context.Suppliers.Remove(supplierToDelete);
                    _context.SaveChanges();

                    // Remove from observable collection
                    Suppliers.Remove(supplierToDelete);
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