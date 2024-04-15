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

        public SupplierPage(ApplicationDBContext context)
        {
            InitializeComponent();
            FillSupplierFilterComboBox();
            _context = context;
            LoadCategories();
        }

        private void LoadCategories()
        {
            Suppliers = new ObservableCollection<Supplier>(_context.Suppliers.ToList());
            DataContext = this;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchBox.Text;
            ICollectionView view = CollectionViewSource.GetDefaultView(supplierListView.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
            {
                view.Filter = item =>
                {
                    // Здесь вы можете настроить поиск по нужным полям категории
                    // Например, если хотите искать по имени, замените "CategoryId" на "Name"
                    return ((Supplier)item).Name.ToString().Contains(searchText);
                };
            }
            else
            {
                view.Filter = null;
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "Search")
            {
                searchBox.Text = "";
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                searchBox.Text = "Search";
            }
        }
        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(supplierListView.ItemsSource);
            view.Filter = null; // Установите фильтр на null, чтобы отобразить все элементы
        }

        private void SortByNameButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(supplierListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void SortByIDButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(supplierListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("SupplierId", ListSortDirection.Ascending));
            }
        }

        private void FillSupplierFilterComboBox()
        {
            using (var context = new ApplicationDBContext()) // Поменяйте ApplicationDBContext на ваш контекст базы данных
            {
                var productNames = context.Suppliers.Select(product => product.Name).ToList();
                supplierFilterComboBox.ItemsSource = productNames;
            }
        }

        private void SupplierFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedMaterial = supplierFilterComboBox.SelectedItem as string;

            ICollectionView view = CollectionViewSource.GetDefaultView( supplierListView.ItemsSource);
            if (view != null)
            {
                if (!string.IsNullOrEmpty(selectedMaterial))
                {
                    view.Filter = item =>
                    {
                        if (item is Supplier itemType) // Замените YourItemType на ваш тип данных для элементов списка
                        {
                            return itemType.Name.Equals(selectedMaterial); // Здесь нужно заменить на свойство, по которому вы хотите фильтровать
                        }
                        return false;
                    };
                }
                else
                {
                    view.Filter = null; // Если материал не выбран, отключаем фильтрацию
                }
            }
        }

        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddSupplierModal();
            if (addWindow.ShowDialog() == true)
            {
                string name = addWindow.Name;
                var newModel = new Supplier() { Name = name };
                _context.Suppliers.Add(newModel);
                _context.SaveChanges();

                Suppliers.Add(newModel);
                DataContext = null;
                DataContext = this;
            }
        }
    }
}
