using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MedicalFurnitureAccounting.Modals;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Pages;

/// <summary>
///     Логика взаимодействия для ProductsPage.xaml
/// </summary>
public partial class ProductsPage : Page
{
    private readonly ApplicationDBContext _context;
    private readonly Storekeeper _user;


    public ProductsPage(ApplicationDBContext context, Storekeeper user)
    {
        InitializeComponent();
        _context = context;
        this._user = user;
        LoadCategories();
    }

    public ObservableCollection<Product> Products { get; set; }

    private void LoadCategories()
    {
        Products = new ObservableCollection<Product>(_context.Products.ToList());

        DataContext = this;
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        var searchText = searchBox.Text;
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);

        if (!string.IsNullOrEmpty(searchText))
            view.Filter = item => { return ((Product)item).Name.ToString().Contains(searchText); };
        else
            view.Filter = null;
    }


    private void DeleteProductsButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            var productToDelete = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToDelete != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить продукт '{productToDelete.Name}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Products.Remove(productToDelete);
                    _context.SaveChanges();

                    Products.Remove(productToDelete);
                }
            }
        }
    }

    private void ChangeShelvingButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToUpdate != null)
            {
                var changeShelvingWindow = new ChangeShelvingWindow(_context, productToUpdate);
                if (changeShelvingWindow.ShowDialog() == true)
                    if (changeShelvingWindow.NewShelvingId.HasValue)
                    {
                        productToUpdate.ShelvingId = changeShelvingWindow.NewShelvingId.Value;
                        _context.SaveChanges();
                        productsListView.Items.Refresh();
                        MessageBox.Show("Номер стеллажа успешно обновлен.", "Успех", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
            }
        }
    }


    private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (searchBox.Text == "Поиск") searchBox.Text = "";
    }

    private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(searchBox.Text)) searchBox.Text = "Поиск";
    }

    private void ShowAllButton_Click(object sender, RoutedEventArgs e)
    {
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
        view.Filter = null; // Установите фильтр на null, чтобы отобразить все элементы
    }

    private void SortByNameButton_Click(object sender, RoutedEventArgs e)
    {
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
        if (view != null)
        {
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("MaxWeight", ListSortDirection.Ascending));
        }
    }

    private void ShowInventoryList_Click(object sender, RoutedEventArgs e)
    {
        var inventoryWindow = new InventoryWindow(_context, _user);
        inventoryWindow.ShowDialog();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var productId = (int)button.Tag;
        var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
        var newWindow = new LabelProductWindow(productToUpdate, _user);
        newWindow.ShowDialog();
    }
}