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
    /// Логика взаимодействия для ChangeCountWindow.xaml
    /// </summary>
    public partial class ChangeCountWindow : Window
    {
        public int? NewCount { get; private set; }

        public ChangeCountWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CountTextBox.Text, out int newCount) && newCount >= 0)
            {
                NewCount = newCount;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
