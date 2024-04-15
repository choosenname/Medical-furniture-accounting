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
    /// Логика взаимодействия для Materials.xaml
    /// </summary>
    public partial class MaterialPage : Page
    {
            private readonly ApplicationDBContext _context;

            public ObservableCollection<Material> Materials { get; set; }

            public MaterialPage(ApplicationDBContext context)
            {

                InitializeComponent();
                FillMaterialFilterComboBox();
                _context = context;
                LoadCategories();
            }

            private void LoadCategories()
            {
                Materials = new ObservableCollection<Material>(_context.Materials.ToList());
                DataContext = this;
            }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchBox.Text;
            ICollectionView view = CollectionViewSource.GetDefaultView(materialListView.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
            {
                view.Filter = item =>
                {
                    // Здесь вы можете настроить поиск по нужным полям категории
                    // Например, если хотите искать по имени, замените "CategoryId" на "Name"
                    return ((Material)item).Name.ToString().Contains(searchText);
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
            ICollectionView view = CollectionViewSource.GetDefaultView(materialListView.ItemsSource);
            view.Filter = null; // Установите фильтр на null, чтобы отобразить все элементы
        }

        private void SortByNameButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(materialListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void SortByIDButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(materialListView.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("MaterialId", ListSortDirection.Ascending));
            }
        }
        private void FillMaterialFilterComboBox()
        {
            using (var context = new ApplicationDBContext()) // Поменяйте ApplicationDBContext на ваш контекст базы данных
            {
                var materialNames = context.Materials.Select(material => material.Name).ToList();
                materialFilterComboBox.ItemsSource = materialNames;
            }
        }

        private void MaterialFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedMaterial = materialFilterComboBox.SelectedItem as string;

            ICollectionView view = CollectionViewSource.GetDefaultView(materialListView.ItemsSource);
            if (view != null)
            {
                if (!string.IsNullOrEmpty(selectedMaterial))
                {
                    view.Filter = item =>
                    {
                        if (item is Material itemType) // Замените YourItemType на ваш тип данных для элементов списка
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

        private void AddMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddMaterialModal();
            if (addWindow.ShowDialog() == true)
            {
                string name = addWindow.Name;
                var newMaterial = new Material() { Name = name };
                _context.Materials.Add(newMaterial);
                _context.SaveChanges();

                Materials.Add(newMaterial);
                DataContext = null;
                DataContext = this;
            }
        }
    }

    

}
