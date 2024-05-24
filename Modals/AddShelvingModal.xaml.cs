using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MedicalFurnitureAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalFurnitureAccounting.Modals
{
    public partial class AddShelvingModal : Window
    {
        public int MaxWeight { get; private set; }
        public Cell Cell { get; private set; }

        private readonly ApplicationDBContext _dbContext;

        public AddShelvingModal(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            LoadCell();
        }

        private void LoadCell()
        {
            // Получаем список всех ячеек из базы данных
            var cells = _dbContext.Cell.ToList();

            // Заполняем ComboBox списком ячеек
            CellComboBox.ItemsSource = cells;
            CellComboBox.DisplayMemberPath = "Number"; // Указываем, какое свойство использовать для отображения
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что MaxWeight введен корректно
            if (!int.TryParse(MaxWeightTextBox.Text, out int maxWeight) || maxWeight <= 0)
            {
                MessageBox.Show("Введите корректное положительное числовое значение для максимального веса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка, что выбрана ячейка
            if (CellComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите ячейку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MaxWeight = maxWeight;
            Cell = (Cell)CellComboBox.SelectedItem;

            // Закрываем окно
            DialogResult = true;
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно
            DialogResult = false;
        }
    }
}
