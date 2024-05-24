using System;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals
{
    public partial class AddMaterialModal : Window
    {
        public AddMaterialModal()
        {
            InitializeComponent();
        }

        public Material Material { get; private set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что имя материала не пустое
            if (string.IsNullOrEmpty(MaterialNameTextBox.Text.Trim()))
            {
                MessageBox.Show("Имя материала не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка, что цена материала введена корректно
            if (!int.TryParse(MaterialPriceTextBox.Text, out int price) || price <= 0)
            {
                MessageBox.Show("Введите корректное положительное числовое значение для цены материала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создание нового объекта Material
            Material = new Material
            {
                Name = MaterialNameTextBox.Text.Trim(),
                Price = price,
            };

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно
            DialogResult = false;
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; // Отменяем ввод символа, если он не является цифрой
                    break;
                }
            }
        }
    }
}
