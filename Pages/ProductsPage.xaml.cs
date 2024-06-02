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
    private readonly Storekeeper user;


    public ProductsPage(ApplicationDBContext context, Storekeeper user)
    {
        InitializeComponent();
        _context = context;
        this.user = user;
        LoadCategories();
    }

    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<Material> Materials { get; set; }
    public ObservableCollection<Shelving> Shelving { get; set; }
    public ObservableCollection<Cell> Cell { get; set; }
    public ObservableCollection<Supply> Supplies { get; set; }

    public int TotalProductCount
    {
        get { return Products.Sum(product => product.Count); }
    }

    private void LoadCategories()
    {
        Products = new ObservableCollection<Product>(_context.Products.ToList());
        FillProductFilterComboBox();
        Categories = new ObservableCollection<Category>(_context.Categories.ToList());
        Materials = new ObservableCollection<Material>(_context.Materials.ToList());
        Shelving = new ObservableCollection<Shelving>(_context.Shelving.ToList());
        Cell = new ObservableCollection<Cell>(_context.Cell.ToList());
        Supplies = new ObservableCollection<Supply>(_context.Supplies.ToList());

        Products.CollectionChanged += (sender, e) => { OnPropertyChanged(nameof(TotalProductCount)); };

        DataContext = this;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        var addCategoryWindow = new AddCategoryModal();
        if (addCategoryWindow.ShowDialog() == true)
        {
            var categoryName = addCategoryWindow.CategoryName;
            var allowedPrice = addCategoryWindow.Allowance;

            var newCategory = new Category { Name = categoryName, Allowance = allowedPrice };
            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            Categories.Add(newCategory);
            DataContext = null;
            DataContext = this;
        }
    }


    private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        // Get the category to be deleted
        var deleteButton = sender as Button;
        if (deleteButton != null)
        {
            var categoryToDelete = deleteButton.DataContext as Category;
            if (categoryToDelete != null)
            {
                // Confirm deletion
                var result = MessageBox.Show($"Вы уверены, что хотите удалить категорию '{categoryToDelete.Name}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove from database
                    _context.Categories.Remove(categoryToDelete);
                    _context.SaveChanges();

                    // Remove from observable collection
                    Categories.Remove(categoryToDelete);
                }
            }
        }
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
                    // Remove from database
                    _context.Products.Remove(productToDelete);
                    _context.SaveChanges();

                    // Remove from observable collection
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
                var changeShelvingWindow = new ChangeShelvingWindow(_context);
                if (changeShelvingWindow.ShowDialog() == true)
                    if (changeShelvingWindow.NewShelvingId.HasValue)
                    {
                        productToUpdate.ShelvingId = changeShelvingWindow.NewShelvingId.Value;
                        _context.SaveChanges();
                        productsListView.Items.Refresh();
                        MessageBox.Show("Номер стелажа успешно обновлен.", "Успех", MessageBoxButton.OK,
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

    private void SortByIDButton_Click(object sender, RoutedEventArgs e)
    {
        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
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
        var selectedMaterial = productsFilterComboBox.SelectedItem as string;

        var view = CollectionViewSource.GetDefaultView(productsListView.ItemsSource);
        if (view != null)
        {
            if (!string.IsNullOrEmpty(selectedMaterial))
                view.Filter = item =>
                {
                    if (item is Product itemType) // Замените YourItemType на ваш тип данных для элементов списка
                        return itemType.Name.Equals(
                            selectedMaterial); // Здесь нужно заменить на свойство, по которому вы хотите фильтровать
                    return false;
                };
            else
                view.Filter = null; // Если материал не выбран, отключаем фильтрацию
        }
    }

    private void AddProductButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddProductModal(_context);
        if (addWindow.ShowDialog() == true)
        {
            var newModel = addWindow.Product;

            // Проверяем, существует ли товар с таким же именем
            var existingProduct = Products.FirstOrDefault(p => p.Name == newModel.Name);
            if (existingProduct != null)
            {
                // Если товар существует, обновляем его свойства
                existingProduct.Count += newModel.Count;
                existingProduct.Width = newModel.Width;
                existingProduct.Height = newModel.Height;
                existingProduct.Length = newModel.Length;
                existingProduct.Weight = newModel.Weight;
                existingProduct.Price = newModel.Price;
                existingProduct.Description = newModel.Description;
                existingProduct.Category = newModel.Category;
                existingProduct.Material = newModel.Material;
                existingProduct.Shelving = newModel.Shelving;
                existingProduct.Suppply = existingProduct.Suppply.Union(newModel.Suppply).ToList();

                _context.SaveChanges();
            }
            else
            {
                // Если товар не существует, добавляем новый товар в коллекцию
                _context.Products.Add(newModel);
                _context.SaveChanges();
                Products.Add(newModel);
            }

            DataContext = null;
            DataContext = this;
        }
    }

    private void ChangeCountButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToUpdate != null)
            {
                var changeCountWindow = new ChangeCountWindow();
                if (changeCountWindow.ShowDialog() == true)
                    if (changeCountWindow.NewCount.HasValue)
                    {
                        productToUpdate.Count = changeCountWindow.NewCount.Value;
                        _context.SaveChanges();
                        productsListView.Items.Refresh();
                        MessageBox.Show("Количество товара успешно обновлено.", "Успех", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
            }
        }
    }

    private void ShowInventoryList_Click(object sender, RoutedEventArgs e)
    {
        var inventoryWindow = new InventoryWindow(_context, user);
        inventoryWindow.ShowDialog();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var productId = (int)button.Tag;
        var productToUpdate = Products.FirstOrDefault(p => p.ProductId == productId);
        var newWindow = new LabelProductWindow(productToUpdate, user);
        newWindow.ShowDialog();
    }

    private void AddMaterialButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddMaterialModal();
        if (addWindow.ShowDialog() == true)
        {
            var newMaterial = addWindow.Material;
            _context.Materials.Add(newMaterial);
            _context.SaveChanges();

            Materials.Add(newMaterial);
            DataContext = null;
            DataContext = this;
        }
    }

    private void DeleteMaterialButton_Click(object sender, RoutedEventArgs e)
    {
        // Get the material to be deleted
        var deleteButton = sender as Button;
        if (deleteButton != null)
        {
            var materialToDelete = deleteButton.DataContext as Material;
            if (materialToDelete != null)
            {
                // Confirm deletion
                var result = MessageBox.Show($"Вы уверены, что хотите удалить материал '{materialToDelete.Name}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove from database
                    _context.Materials.Remove(materialToDelete);
                    _context.SaveChanges();

                    // Remove from observable collection
                    Materials.Remove(materialToDelete);
                }
            }
        }
    }

    private void AddShelvingButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddShelvingModal(_context);
        if (addWindow.ShowDialog() == true)
        {
            var name = addWindow.MaxWeight;
            var cell = addWindow.Cell;
            var newModel = new Shelving { MaxWeight = name, Cell = cell };
            _context.Shelving.Add(newModel);
            _context.SaveChanges();

            Shelving.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
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
                    if (changeCellWindow.NewCellId.HasValue)
                    {
                        shelvingToUpdate.CellId = changeCellWindow.NewCellId.Value;
                        _context.SaveChanges();
                        shelvingListView.Items.Refresh();
                        MessageBox.Show("Ячейка успешно обновлена.", "Успех", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
            }
        }
    }

    private void AddCellButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddCellModal();
        if (addWindow.ShowDialog() == true)
        {
            var name = addWindow.Number;
            var newModel = new Cell { Number = name };
            _context.Cell.Add(newModel);
            _context.SaveChanges();

            Cell.Add(newModel);
            DataContext = null;
            DataContext = this;
        }
    }
}