using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Pages;

public partial class CategoryPage : Page
{
    private readonly ApplicationDBContext _dbContext;

    public ObservableCollection<Category> Categories { get; set; }

    public CategoryPage(ApplicationDBContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        LoadCategories();
    }

    private void LoadCategories()
    {
        Categories = new ObservableCollection<Category>(_dbContext.Categories.ToList());
        DataContext = this;
    }

    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        // Создание новой категории
        Category newCategory = new Category() { Name = "New Category" };
        _dbContext.Categories.Add(newCategory);
        _dbContext.SaveChanges();

        // Добавление категории к коллекции и обновление DataContext
        Categories.Add(newCategory);
        DataContext = null;
        DataContext = this;
    }
}