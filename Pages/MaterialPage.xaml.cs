using MedicalFurnitureAccounting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MedicalFurnitureAccounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Materials.xaml
    /// </summary>
    public partial class MaterialPage : Page
    {
            private readonly ApplicationDBContext _context;

            public ObservableCollection<Material> Materials { get; set; }

            public MaterialPage(ApplicationDBContext context)
            {
                InitializeComponent();
                _context = context;
                LoadCategories();
            }

            private void LoadCategories()
            {
                Materials = new ObservableCollection<Material>(_context.Materials.ToList());
                DataContext = this;
            }
    }
}
