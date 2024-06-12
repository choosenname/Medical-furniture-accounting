using System.IO;
using System.Windows;
using MedicalFurnitureAccounting.Models;
using Word = Microsoft.Office.Interop.Word;

namespace MedicalFurnitureAccounting;

internal class WordHelper
{
    private readonly FileInfo _fileInfo;

    public WordHelper(string fileName)
    {
        if (File.Exists(fileName))
            _fileInfo = new FileInfo(fileName);
        else
            MessageBox.Show("FiIe not found");
    }

    internal bool Process(Dictionary<string, string> items)
    {
        Word.Application app = null;
        try
        {
            app = new Word.Application();
            object file = _fileInfo.FullName;

            var missing = Type.Missing;

            app.Documents.Open(file);

            foreach (var item in items)
            {
                var find = app.Selection.Find;
                find.Text = item.Key;
                find.Replacement.Text = item.Value;

                object wrap = Word.WdFindWrap.wdFindContinue;
                object replace = Word.WdReplace.wdReplaceAll;

                find.Execute(Type.Missing,
                    false,
                    false,
                    false,
                    missing,
                    false,
                    true,
                    wrap,
                    false,
                    missing, replace);
            }

            object newFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                DateTime.Now.ToString("yyyyMMdd HHmmss ") + _fileInfo.Name);
            app.ActiveDocument.SaveAs2(newFileName);
            app.ActiveDocument.Close();
            MessageBox.Show($"Файл успешно сохранен по пути: {newFileName}", "Сохранение завершено",
                MessageBoxButton.OK, MessageBoxImage.Information);

            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            if (app != null) app.Quit();
        }

        return false;
    }

    internal bool AddTableAndReplaceData(Dictionary<string, string> items, ICollection<Product> products)
    {
        Word.Application app = null;
        try
        {
            app = new Word.Application();
            object file = _fileInfo.FullName;

            var missing = Type.Missing;

            app.Documents.Open(file);

            foreach (var item in items)
            {
                var find = app.Selection.Find;
                find.Text = item.Key;
                find.Replacement.Text = item.Value;

                object wrap = Word.WdFindWrap.wdFindContinue;
                object replace = Word.WdReplace.wdReplaceAll;

                find.Execute(Type.Missing,
                    false,
                    false,
                    false,
                    missing,
                    false,
                    true,
                    wrap,
                    false,
                    missing, replace);
            }

            // Добавление таблицы в конец документа
            var range = app.ActiveDocument.Content;
            range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            range.InsertParagraphAfter(); // Вставляем параграф перед таблицей
            range.InsertParagraphAfter(); // Вставляем параграф перед таблицей

            // Создаем таблицу
            var rowCount = products.Count + 1; // +1 для заголовка таблицы
            var columnCount = 5; // Количество столбцов
            var table = app.ActiveDocument.Tables.Add(range, rowCount, columnCount);

            table.Borders.Enable = 1; // Включаем границы таблицы
            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle; // Стиль линии внутренних границ
            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle; // Стиль линии внешних границ

            // Устанавливаем цвет границ
            table.Borders.InsideColor = Word.WdColor.wdColorBlack; // Черный цвет для внутренних границ
            table.Borders.OutsideColor = Word.WdColor.wdColorBlack; // Черный цвет для внешних границ

            // Заполнение заголовка таблицы
            table.Cell(1, 1).Range.Text = "Номер продукта";
            table.Cell(1, 2).Range.Text = "Наименование";
            table.Cell(1, 3).Range.Text = "Цена";
            table.Cell(1, 4).Range.Text = "Стеллаж";
            table.Cell(1, 5).Range.Text = "Количество";

            // Заполнение данных в таблице
            var rowIndex = 2; // Начинаем с 2-й строки, т.к. первая строка занята заголовком
            foreach (var product in products)
            {
                table.Cell(rowIndex, 1).Range.Text = product.ProductId.ToString();
                table.Cell(rowIndex, 2).Range.Text = product.Name;
                table.Cell(rowIndex, 3).Range.Text = product.Material.Price.ToString();
                table.Cell(rowIndex, 4).Range.Text = product.ShelvingId.ToString();
                table.Cell(rowIndex, 5).Range.Text = product.Count.ToString();
                rowIndex++;
            }

            // Сохранение и закрытие документа
            object newFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                DateTime.Now.ToString("yyyyMMdd HHmmss ") + _fileInfo.Name);
            app.ActiveDocument.SaveAs2(newFileName);
            app.ActiveDocument.Close();
            MessageBox.Show($"Файл успешно сохранен по пути: {newFileName}", "Сохранение завершено",
                MessageBoxButton.OK, MessageBoxImage.Information);

            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            if (app != null) app.Quit();
        }

        return false;
    }
}