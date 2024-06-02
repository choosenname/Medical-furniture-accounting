using MedicalFurnitureAccounting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MedicalFurnitureAccounting.Modals;

namespace MedicalFurnitureAccounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для SupplierPage.xaml
    /// </summary>
    public partial class SupplierPage : Page
    {
        private readonly ApplicationDBContext _context;

        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ObservableCollection<Supply> Supplies { get; set; }

        public SupplierPage(ApplicationDBContext context)
        {
            InitializeComponent();
            _context = context;
            LoadCategories();
        }

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
                string name = addWindow.Name;
                string phone = addWindow.Phone;
                string email =addWindow.Email;
                string registrationNumber = addWindow.RegistrationNumber;
                string addres = addWindow.Addres;
                string country = addWindow.Country;

                var newModel = new Supplier(name: name, phone: phone, email: email,
                    registrationNumber: registrationNumber, addres: addres, country: country);
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
                    var result = MessageBox.Show($"Вы уверены, что хотите удалить поставщика '{supplierToDelete.Name}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
}
