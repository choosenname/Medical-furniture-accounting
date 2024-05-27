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
    /// Логика взаимодействия для ChangeCellWindow.xaml
    /// </summary>
    public partial class ChangeCellWindow : Window
    {
        public int? NewCellId { get; private set; }
        private readonly ApplicationDBContext _dbContext;

        public ChangeCellWindow(ApplicationDBContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadCells();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CellComboBox.SelectedItem != null)
            {
                NewCellId = ((Models.Cell)CellComboBox.SelectedItem).CellId;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите корректную ячейку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCells()
        {
            var cells = _dbContext.Cell.ToList();
            CellComboBox.ItemsSource = cells;
            CellComboBox.DisplayMemberPath = "Number";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
