﻿using System;
using System.Linq;
using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals
{
    public partial class AddSupplyModal : Window
    {
        private readonly ApplicationDBContext _dbContext;

        public AddSupplyModal(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            LoadSuppliers();
        }

        public Supply Supply { get; private set; }

        private void LoadSuppliers()
        {
            // Получаем список всех поставщиков из базы данных
            var suppliers = _dbContext.Suppliers.ToList();

            // Заполняем ComboBox списком поставщиков
            SupplierComboBox.ItemsSource = suppliers;
            SupplierComboBox.DisplayMemberPath = "Name"; // Указываем, какое свойство использовать для отображения
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что дата выбрана
            if (DatePicker.Value == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату поставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка, что выбран поставщик
            if (SupplierComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите поставщика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получаем выбранного поставщика из ComboBox
            var selectedSupplier = (Supplier)SupplierComboBox.SelectedItem;

            // Создаем новую поставку
            Supply = new Supply
            {
                Date = (DateTime)DatePicker.Value,
                Supplier = selectedSupplier
            };

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
