using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MedicalFurnitureAccounting.Modals;
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
        var addCategoryWindow = new AddCategoryModal();
        if (addCategoryWindow.ShowDialog() == true)
        {
            string categoryName = addCategoryWindow.CategoryName;
            double allowedPrice = addCategoryWindow.Allowance;

            Category newCategory = new Category() { Name = categoryName, Allowance = allowedPrice };
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
        Button deleteButton = sender as Button;
        if (deleteButton != null)
        {
            Category categoryToDelete = deleteButton.DataContext as Category;
            if (categoryToDelete != null)
            {
                // Confirm deletion
                var result = MessageBox.Show($"Вы уверены, что хотите удалить категорию '{categoryToDelete.Name}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
        string searchText = SearchBox.Text;
        ICollectionView view = CollectionViewSource.GetDefaultView(CategoryListView.ItemsSource);

        if (!string.IsNullOrEmpty(searchText))
        {
            view.Filter = item =>
            {
                // Здесь вы можете настроить поиск по нужным полям категории
                // Например, если хотите искать по имени, замените "CategoryId" на "MaxWeight"
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
        if (SearchBox.Text == "Поиск")
        {
            SearchBox.Text = "";
        }
    }

    private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchBox.Text))
        {
            SearchBox.Text = "Поиск";
        }
    }
    private void ShowAllButton_Click(object sender, RoutedEventArgs e)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(CategoryListView.ItemsSource);
        view.Filter = null; // Установите фильтр на null, чтобы отобразить все элементы
    }

    private void SortByNameButton_Click(object sender, RoutedEventArgs e)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(CategoryListView.ItemsSource);
        if (view != null)
        {
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("MaxWeight", ListSortDirection.Ascending));
        }
    }

    private void SortByIDButton_Click(object sender, RoutedEventArgs e)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(CategoryListView.ItemsSource);
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
            CategoryFilterComboBox.ItemsSource = categoryNames;
        }
    }


    private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedCategory = CategoryFilterComboBox.SelectedItem as string;

        ICollectionView view = CollectionViewSource.GetDefaultView(CategoryListView.ItemsSource);
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

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void CategoryFilterComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {

    }
}