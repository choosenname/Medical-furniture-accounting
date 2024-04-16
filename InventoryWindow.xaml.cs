using System.IO;
using System.Windows;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting;

public partial class InventoryWindow : Window
{
    private readonly ApplicationDBContext _dbContext;

    public InventoryWindow(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    public ICollection<Product> Products { get; private set; }

    private void LoadSuppliers()
    {
        Products = _dbContext.Products.ToList();
        DataContext = this;
    }

    private void ExportToWordButton_Click(object sender, RoutedEventArgs e)
    {
        // Создаем новый документ Word
        var fileName = "InventoryList.docx";
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

        using (var wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
        {
            var mainPart = wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            // Добавляем заголовки
            AddParagraph(body, "ИНВЕНТАРИЗАЦИОННАЯ ОПИСЬ", true, 48, true);
            AddParagraph(body, "товарно-материальных ценностей", true, 24, true);
            AddParagraph(body, "РАСПИСКА", true, 24, true);
            AddParagraph(body,
                "К началу проведения инвентаризации все расходные и приходные документы на товарно-материальные ценности сданы в бухгалтерию и все товарно-материальные ценности, поступившие на мою (нашу) ответственность, оприходованы, а выбывшие списаны в расход.",
                false, 24, true);

            // Добавляем таблицу с данными из ListView
            var products = (List<Product>)productsListView.ItemsSource;
            var table = new Table();
            var tableWidth = new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct };
            var tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                ),
                tableWidth
            );
            table.AppendChild(tableProperties);

            // Заголовки столбцов
            var headerRow = new TableRow();
            AddTableCell(headerRow, "Product ID", true);
            AddTableCell(headerRow, "Name", true);
            AddTableCell(headerRow, "Count", true);
            AddTableCell(headerRow, "Price", true);
            table.AppendChild(headerRow);

            // Данные из ListView
            foreach (var product in products)
            {
                var dataRow = new TableRow();
                AddTableCell(dataRow, product.ProductId.ToString());
                AddTableCell(dataRow, product.Name);
                AddTableCell(dataRow, product.Count.ToString());
                AddTableCell(dataRow, product.Material.Price.ToString());
                table.AppendChild(dataRow);
            }

            body.Append(table);
        }

        MessageBox.Show($"Документ успешно экспортирован в {filePath}");
    }

    private void AddParagraph(Body body, string text, bool bold = false, int fontSize = 12, bool centerAligned = false)
    {
        var paragraph = new Paragraph();
        var run = new Run();
        var runProperties = new RunProperties();

        // Устанавливаем размер шрифта
        var fontSizeElement = new FontSize { Val = new StringValue(fontSize.ToString()) };
        runProperties.Append(fontSizeElement);

        // Устанавливаем жирность текста
        if (bold)
        {
            var boldElement = new Bold();
            runProperties.Append(boldElement);
        }

        // Добавляем текст в объект Run
        var textElement = new Text(text);
        run.Append(runProperties);
        run.Append(textElement);

        // Добавляем объект Run в абзац
        paragraph.Append(run);

        // Устанавливаем выравнивание по центру
        if (centerAligned)
        {
            var paragraphProperties = new ParagraphProperties();
            var justification = new Justification { Val = JustificationValues.Center };
            paragraphProperties.Append(justification);
            paragraph.ParagraphProperties = paragraphProperties;
        }

        // Добавляем абзац в тело документа
        body.Append(paragraph);
    }

    private void AddTableCell(TableRow row, string text, bool bold = false)
    {
        var cell = new TableCell();
        var paragraph = new Paragraph(new Run(new Text(text)));

        // Создаем объект RunProperties для установки свойств текста
        var runProperties = new RunProperties();

        // Устанавливаем жирность текста, если это требуется
        if (bold)
        {
            var boldProperty = new Bold();
            runProperties.Append(boldProperty);
        }

        // Устанавливаем свойства текста для объекта Run
        paragraph.AppendChild(runProperties);

        // Добавляем параграф в ячейку
        cell.Append(paragraph);

        // Добавляем ячейку в строку таблицы
        row.Append(cell);
    }
}