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
    public partial class ShelvingPage : Page
    {
        private readonly ApplicationDBContext _context;

        public ObservableCollection<Shelving> Shelving { get; set; }

        public ShelvingPage(ApplicationDBContext context)
        {
            InitializeComponent();
            FillSupplierFilterComboBox();
            _context = context;
            LoadCategories();
        }

        private void LoadCategories()
        {
            Shelving = new ObservableCollection<Shelving>(_context.Shelving.ToList());
            DataContext = this;
        }

        private void ChangeCellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int shelvingId)
            {
                var shelvingToUpdate = Shelving.FirstOrDefault(s => s.ShelvingId == shelvingId);
                if (shelvingToUpdate != null)
                {
                    var changeCellWindow = new ChangeCellWindow(_context);
                    if (changeCellWindow.ShowDialog() == true)
                    {
                        if (changeCellWindow.NewCellId.HasValue)
                        {
                            shelvingToUpdate.CellId = changeCellWindow.NewCellId.Value;
                            _context.SaveChanges();
                            supplierListView.Items.Refresh();
                            MessageBox.Show("Ячейка успешно обновлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
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
                    // Например, если хотите искать по имени, замените "CategoryId" на "MaxWeight"
                    return ((Shelving)item).MaxWeight.ToString().Contains(searchText);
                };
            }
            else
            {
                view.Filter = null;
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "Поиск")
            {
                searchBox.Text = "";
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                searchBox.Text = "Поиск";
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
                view.SortDescriptions.Add(new SortDescription("MaxWeight", ListSortDirection.Ascending));
            }
        }

        private void SortByIDButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(supplierListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("ShelvingId", ListSortDirection.Ascending));
            }
        }

        private void FillSupplierFilterComboBox()
        {
            using (var context = new ApplicationDBContext()) // Поменяйте ApplicationDBContext на ваш контекст базы данных
            {
                var productNames = context.Shelving.Select(product => product.MaxWeight).ToList();
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
                        if (item is Shelving itemType) // Замените YourItemType на ваш тип данных для элементов списка
                        {
                            return itemType.MaxWeight.Equals(selectedMaterial); // Здесь нужно заменить на свойство, по которому вы хотите фильтровать
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
            var addWindow = new AddShelvingModal(_context);
            if (addWindow.ShowDialog() == true)
            {
                var name = addWindow.MaxWeight;
                var cell = addWindow.Cell;
                var newModel = new Shelving() { MaxWeight = name, Cell = cell };
                _context.Shelving.Add(newModel);
                _context.SaveChanges();

                Shelving.Add(newModel);
                DataContext = null;
                DataContext = this;
            }
        }
    }
}
