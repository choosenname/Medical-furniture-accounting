using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Pages;

/// <summary>
/// Логика взаимодействия для ProductsPage.xaml
/// </summary>
public partial class ProductsPage : Page
{
    private readonly ApplicationDBContext _context;
    private readonly Storekeeper _user;

    public ProductsPage(ApplicationDBContext context, Storekeeper user)
    {
        InitializeComponent();
        _context = context;
        _user = user;
        LoadData();
    }

    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<Material> Materials { get; set; }
    public ObservableCollection<Supplier> Suppliers { get; set; }

    private void LoadData()
    {
        Products = new ObservableCollection<Product>(_context.Products.ToList());
        Categories = new ObservableCollection<Category>(_context.Categories.ToList());
        Materials = new ObservableCollection<Material>(_context.Materials.ToList());
        Suppliers = new ObservableCollection<Supplier>(_context.Suppliers.ToList());

        DataContext = this;
        categoryFilter.ItemsSource = Categories;
        materialFilter.ItemsSource = Materials;
        supplierFilter.ItemsSource = Suppliers;
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        var searchText = searchBox.Text.ToLower();
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
        if (!string.IsNullOrEmpty(searchText) && searchText != "поиск")
        {
            view.Filter = item =>
            {
                var product = (Product)item;
                return product.Name.ToLower().Contains(searchText);
            };
        }
        else
        {
            view.Filter = null;
        }
    }

    private void ShowAllButton_Click(object sender, RoutedEventArgs e)
    {
        searchBox.Text = "Поиск";
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
        view.Filter = null;
    }

    private void SortByNameButton_Click(object sender, RoutedEventArgs e)
    {
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
        if (view != null)
        {
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
    }

    private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void MaterialFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void SupplierFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
        view.Filter = item =>
        {
            var product = (Product)item;
            bool categoryMatch = categoryFilter.SelectedItem == null || product.CategoryId == (int)categoryFilter.SelectedValue;
            bool materialMatch = materialFilter.SelectedItem == null || product.MaterialId == (int)materialFilter.SelectedValue;
            bool supplierMatch = supplierFilter.SelectedItem == null || product.SupplyItems.Any(si => si.Supply.SupplierId == (int)supplierFilter.SelectedValue);
            return categoryMatch && materialMatch && supplierMatch;
        };
    }

    private void DeleteProductsButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            var productToDelete = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToDelete != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить продукт '{productToDelete.Name}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
                {
                    if (changeShelvingWindow.NewShelvingId.HasValue)
                    {
                        productToUpdate.ShelvingId = changeShelvingWindow.NewShelvingId.Value;
                        _context.SaveChanges();
                        productsListView.Items.Refresh();
                        MessageBox.Show("Номер стеллажа успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
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
