using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MedicalFurnitureAccounting.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MedicalFurnitureAccounting.Pages;

public partial class CategoryPage : Page
{
    private readonly ApplicationDBContext _context;

    public ObservableCollection<Category> Categories { get; set; }

    public CategoryPage(ApplicationDBContext context)
    {
        InitializeComponent();
        _context = context;
        LoadCategories();
    }

    private void LoadCategories()
    {
        Categories = new ObservableCollection<Category>(_context.Categories.ToList());
        FillCategoryFilterComboBox();
        DataContext = this;
    }

    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        // Создание новой категории
        Category newCategory = new Category() { Name = "New Category" };
        _context.Categories.Add(newCategory);
        _context.SaveChanges();

        // Добавление категории к коллекции и обновление DataContext
        Categories.Add(newCategory);
        DataContext = null;
        DataContext = this;
    }
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string searchText = searchBox.Text;
        ICollectionView view = CollectionViewSource.GetDefaultView(categoryListView.ItemsSource);

        if (!string.IsNullOrEmpty(searchText))
        {
            view.Filter = item =>
            {
                // Здесь вы можете настроить поиск по нужным полям категории
                // Например, если хотите искать по имени, замените "CategoryId" на "Name"
                return ((Category)item).Name.ToString().Contains(searchText);
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
        ICollectionView view = CollectionViewSource.GetDefaultView(categoryListView.ItemsSource);
        view.Filter = null; // Установите фильтр на null, чтобы отобразить все элементы
    }

    private void SortByNameButton_Click(object sender, RoutedEventArgs e)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(categoryListView.ItemsSource);
        if (view != null)
        {
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
    }

    private void SortByIDButton_Click(object sender, RoutedEventArgs e)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(categoryListView.ItemsSource);
        if (view != null)
        {
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("CategoryId", ListSortDirection.Ascending));
        }
    }

    private void FillCategoryFilterComboBox()
    {
        using (var context = new ApplicationDBContext()) // Замените YourDbContext на ваш контекст базы данных
        {
            var categoryNames = context.Categories.Select(category => category.Name).ToList();
            categoryFilterComboBox.ItemsSource = categoryNames;
        }
    }


    private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedCategory = categoryFilterComboBox.SelectedItem as string;

        ICollectionView view = CollectionViewSource.GetDefaultView(categoryListView.ItemsSource);
        if (view != null)
        {
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                view.Filter = item =>
                {
                    if (item is Category category) // Предполагается, что тип элементов вашего списка - это Category
                    {
                        return category.Name.Equals(selectedCategory);
                    }
                    return false;
                };
            }
            else
            {
                view.Filter = null; // Если категория не выбрана, отключаем фильтрацию
            }
        }
    }




}