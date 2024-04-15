﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddProductModal : Window
{
    private readonly ApplicationDBContext _dbContext;
    public NavigationService NavigationService { get; set; }

    public AddProductModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    public Product Product { get; private set; }

    private void LoadSuppliers()
    {
        var supplies = _dbContext.Supplies.ToList();

        SupplyComboBox.ItemsSource = supplies;
        SupplyComboBox.DisplayMemberPath = "Date";

        var categories = _dbContext.Categories.ToList();

        CategoryComboBox.ItemsSource = categories;
        CategoryComboBox.DisplayMemberPath = "Name";

        var materials = _dbContext.Materials.ToList();

        MaterialComboBox.ItemsSource = materials;
        MaterialComboBox.DisplayMemberPath = "Name";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductNameTextBox == null) return;

        var selectedSupply = (Supply)SupplyComboBox.SelectedItem;
        var selectedCategory = (Category)CategoryComboBox.SelectedItem;
        var materialCategory = (Material)MaterialComboBox.SelectedItem;

        Product = new Product
        {
            Name = ProductNameTextBox.Text,
            Suppply = selectedSupply,
            Category = selectedCategory,
            Material = materialCategory,
            Count = Convert.ToInt32(ProductCountTextBox.Text),
            Room = ProductRoomTextBox.Text,
        };

        DialogResult = true;
    }

    private void GenerateAcceptanceAct(int supplyId, Product product)
    {
        Supply supply = _dbContext.Supplies.FirstOrDefault(s => s.SupplyId == supplyId);
        if (supply != null)
        {
            string supplierName = supply.Supplier != null ? supply.Supplier.Name : "Unknown";

            AcceptanceAct acceptanceAct = new AcceptanceAct
            {
                Date = supply.Date,
                ProductName = product.Name,
                Count = product.Count,
                Room = product.Room,
                SupplierName = supplierName
            };

            // Создание страницы акта приема-передачи
            AcceptanceActPage acceptanceActPage = new AcceptanceActPage(acceptanceAct);
            // Установка DataContext страницы на созданный акт приема-передачи
            acceptanceActPage.DataContext = acceptanceAct;

            // Отображение страницы в диалоговом окне
            var dialog = new Window
            {
                Content = acceptanceActPage,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                Title = "Acceptance Act"
            };

            dialog.ShowDialog();
        }
        else
        {
            MessageBox.Show("Ошибка при создании акта. Информация о поставке не найдена.");
        }
    }





    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
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