using MedicalFurnitureAccounting.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MedicalFurnitureAccounting
{
    /// <summary>
    /// Логика взаимодействия для ChangeShelvingWindow.xaml
    /// </summary>
    public partial class ChangeShelvingWindow : Window
    {
        public int? NewShelvingId { get; private set; }
        private readonly ApplicationDBContext _dbContext;

        public ChangeShelvingWindow(ApplicationDBContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadSuppliers();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelvingComboBox.SelectedItem != null)
            {
                NewShelvingId = ((Shelving)SelvingComboBox.SelectedItem).ShelvingId;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный номер стелажа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSuppliers()
        {

            var shelving = _dbContext.Shelving.ToList();
            SelvingComboBox.ItemsSource = shelving;
            SelvingComboBox.DisplayMemberPath = "ShelvingId";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
