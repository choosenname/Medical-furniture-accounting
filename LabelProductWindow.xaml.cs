using MedicalFurnitureAccounting.Models;
using MedicalFurnitureAccounting.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Word;
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
    /// Логика взаимодействия для LabelProduct.xaml
    /// </summary>
    public partial class LabelProductWindow : System.Windows.Window
    {
        public Product Product { get; set; }

        public LabelProductWindow(Product product)
        {
            InitializeComponent();
            Product = product;
            DataContext = Product;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создание приложения Word
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = true;

                // Создание нового документа Word
                Microsoft.Office.Interop.Word.Document wordDoc = wordApp.Documents.Add();

                // Добавление заголовка
                Microsoft.Office.Interop.Word.Paragraph title = wordDoc.Paragraphs.Add();
                title.Range.Text = "Ярлык товара";
                title.Range.Bold = 1;
                title.Range.Font.Size = 24;
                title.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.InsertParagraphAfter();

                // Добавление данных о товаре
                Microsoft.Office.Interop.Word.Paragraph dataParagraph = wordDoc.Paragraphs.Add();
                dataParagraph.Range.Text = $"Наименование товара: {Product.Name}\n" +
                                            $"Количество: {Product.Count}\n" +
                                            $"Материал: {Product.Material.Name}\n" +
                                            $"Категория: {Product.Category.Name}";
                dataParagraph.Range.InsertParagraphAfter();

                var fileName = $"LabelProduct_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.docx";
                var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                wordDoc.SaveAs2(filePath);

                // Освобождение ресурсов
                wordDoc.Close();
                wordApp.Quit();

                MessageBox.Show($"Документ успешно сохранен по пути: {filePath}", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте данных в Word: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
