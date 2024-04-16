using MedicalFurnitureAccounting.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace MedicalFurnitureAccounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AcceptanceActPage.xaml
    /// </summary>
    public partial class AcceptanceActPage : System.Windows.Controls.Page
    {
        private readonly AcceptanceAct _acceptanceAct;

        public AcceptanceActPage(AcceptanceAct acceptanceAct)
        {
            InitializeComponent();
            _acceptanceAct = acceptanceAct;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие текущего окна
            System.Windows.Window.GetWindow(this).Close();
        }
        public void ExportAcceptanceAct_Click(object sender, RoutedEventArgs e)
        {
            // Создание приложения Word
            Word.Application wordApp = new Word.Application();
            wordApp.Visible = true;

            // Создание нового документа Word
            Word.Document wordDoc = wordApp.Documents.Add();

            // Добавление заголовка
            Word.Paragraph title = wordDoc.Paragraphs.Add();
            title.Range.Text = "Акт приема-передачи";
            title.Range.Bold = 1;
            title.Range.Font.Size = 24;
            title.Format.SpaceAfter = 12;
            title.Range.InsertParagraphAfter();

            // Создание таблицы и заполнение ее данными
            Word.Table table = wordDoc.Tables.Add(wordDoc.Paragraphs[2].Range, 7, 2);
            table.Borders.Enable = 1;

            table.Cell(1, 1).Range.Text = "Дата:";
            table.Cell(1, 2).Range.Text = _acceptanceAct.Date.ToShortDateString();

            table.Cell(2, 1).Range.Text = "Наименование товара:";
            table.Cell(2, 2).Range.Text = _acceptanceAct.ProductName;

            table.Cell(3, 1).Range.Text = "Количество:";
            table.Cell(3, 2).Range.Text = _acceptanceAct.Count.ToString();

            table.Cell(4, 1).Range.Text = "Помещение:";
            table.Cell(4, 2).Range.Text = _acceptanceAct.Room;

            table.Cell(5, 1).Range.Text = "Категория:";
            table.Cell(5, 2).Range.Text = _acceptanceAct.Category;

            table.Cell(6, 1).Range.Text = "Поставщик:";
            table.Cell(6, 2).Range.Text = _acceptanceAct.SupplierName;

            // Добавление места для подписи
            Word.Paragraph signature = wordDoc.Paragraphs.Add();
            signature.Range.Text = "Место росписи: ________________________";
            signature.Format.SpaceAfter = 12;
            signature.Range.InsertParagraphAfter();

            // Сохранение документа Word
            var fileName = $"Act_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.docx";
            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            wordDoc.SaveAs2(filePath);

            // Освобождение ресурсов
            wordDoc.Close();
            wordApp.Quit();

            // Вывод уведомления о сохранении файла
            MessageBox.Show($"Файл успешно сохранен по пути: {filePath}", "Сохранение завершено", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
