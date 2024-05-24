using MedicalFurnitureAccounting.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MedicalFurnitureAccounting.Modals;
using System.Windows.Navigation;

namespace MedicalFurnitureAccounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private readonly ApplicationDBContext _context;
        private readonly Storekeeper user;

        public ObservableCollection<Product> Products { get; set; }

        public ProductsPage(ApplicationDBContext context, Storekeeper user)
        {
            InitializeComponent();
            _context = context;
            this.user = user;
            LoadCategories();
        }

        private void LoadCategories()
        {
            Products = new ObservableCollection<Product>(_context.Products.ToList());
            FillProductFilterComboBox();
            DataContext = this;
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchBox.Text;
            ICollectionView view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
            {
                view.Filter = item =>
                {
                    return ((Product)item).Name.ToString().Contains(searchText);
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
            ICollectionView view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
            view.Filter = null; // Установите фильтр на null, чтобы отобразить все элементы
        }

        private void SortByNameButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("MaxWeight", ListSortDirection.Ascending));
            }
        }

        private void SortByIDButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("ProductId", ListSortDirection.Ascending));
            }
        }

        private void FillProductFilterComboBox()
        {
            using (var context = new ApplicationDBContext()) // Поменяйте ApplicationDBContext на ваш контекст базы данных
            {
                var productNames = context.Products.Select(product => product.Name).ToList();
                productsFilterComboBox.ItemsSource = productNames;
            }
        }

        private void ProductsFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedMaterial = productsFilterComboBox.SelectedItem as string;

            ICollectionView view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
            if (view != null)
            {
                if (!string.IsNullOrEmpty(selectedMaterial))
                {
                    view.Filter = item =>
                    {
                        if (item is Product itemType) // Замените YourItemType на ваш тип данных для элементов списка
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

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductModal(_context);
            if (addWindow.ShowDialog() == true)
            {
                var newModel = addWindow.Product;
                _context.Products.Add(newModel);
                _context.SaveChanges();

                Products.Add(newModel);
                DataContext = null;
                DataContext = this;
            }
        }

        private void ShowInventoryList_Click(object sender, RoutedEventArgs e)
        {
            var inventoryWindow = new InventoryWindow(_context, user);
            inventoryWindow.ShowDialog();
        }

        private void ChangeRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int productId = (int)button.Tag;

            var dialog = new ChangeRoomModal();
            if (dialog.ShowDialog() == true)
            {
                // Пользователь нажал OK, обновить комнату для выбранного продукта
                string newRoom = dialog.NewRoom;
                // Здесь вы можете обновить комнату для выбранного продукта

                var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
                if (productToUpdate != null)
                {
                    // Обновите комнату для выбранного продукта
                    productToUpdate.Room = newRoom;
                    _context.SaveChanges();
                    // Здесь вы можете выполнить дополнительные действия по обновлению данных в вашем приложении или базе данных

                    DataContext = null;
                    DataContext = this;
                }
                else
                {
                    MessageBox.Show("Product not found.");
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int productId = (int)button.Tag;
            var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
            var newWindow = new LabelProductWindow(productToUpdate);
            newWindow.ShowDialog(); 
        }
    }
}
