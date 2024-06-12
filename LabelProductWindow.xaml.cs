using System.Globalization;
using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting;

/// <summary>
///     Логика взаимодействия для LabelProduct.xaml
/// </summary>
public partial class LabelProductWindow
{
    private readonly Storekeeper _user;

    public LabelProductWindow(Product product, Storekeeper user)
    {
        InitializeComponent();
        Product = product;
        DataContext = Product;
        this._user = user;
    }

    public Product Product { get; set; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var helper = new WordHelper("Docs/Lable.doc");

        var suppply = Product.SupplyItems.LastOrDefault().Supply;

        var items = new Dictionary<string, string>
        {
            { "<NAME>", Product.Name },
            { "<COUNT>", suppply?.Count.ToString() ?? "" },
            { "<MATERIAL>", Product.Material.Name },
            { "<CATEGORY>", Product.Category.Name },
            { "<ID>", Product.ProductId.ToString() },
            { "<ORGANIZATION>", suppply?.Supplier.Name ?? "" },
            { "<ADDRESS>", suppply?.Supplier.Addres ?? "" },
            { "<PHONE>", suppply?.Supplier.Phone ?? "" },
            { "<EMAIL>", suppply?.Supplier.Email ?? "" },
            { "<WIDTH>", Product.Width.ToString() },
            { "<LENGTH>", Product.Length.ToString() },
            { "<HEIGHT>", Product.Height.ToString() },
            { "<WEIGHT>", Product.Weight.ToString() },
            { "<PRICE>", Product.Price.ToString() },
            { "<USER_NAME>", _user.Name },
            { "<DATE_NOW>", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
        };

        helper.Process(items);
    }
}