using MedicalFurnitureAccounting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для SupplyPage.xaml
    /// </summary>
    public partial class SupplyPage : Page
    {
        private readonly ApplicationDBContext _context;

        public ObservableCollection<Supply> Supplies { get; set; }

        public SupplyPage(ApplicationDBContext context)
        {
            InitializeComponent();
            _context = context;
            LoadCategories();
        }

        private void LoadCategories()
        {
            Supplies = new ObservableCollection<Supply>(_context.Supplies.ToList());
            DataContext = this;
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

        private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int supplyId)
            {
                var supplyToDelete = Supplies.FirstOrDefault(p => p.SupplyId == supplyId);
                if (supplyToDelete != null)
                {
                    var result = MessageBox.Show($"Вы уверены, что хотите удалить поставщика '{supplyToDelete.Date}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
    }
}
