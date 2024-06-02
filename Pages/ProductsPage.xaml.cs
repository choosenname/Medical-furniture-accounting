﻿using System.Collections.ObjectModel;
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
}