using MedicalFurnitureAccounting.Models;
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

namespace MedicalFurnitureAccounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AcceptanceActPage.xaml
    /// </summary>
    public partial class AcceptanceActPage : Page
    {
        private readonly AcceptanceAct _acceptanceAct;

        public AcceptanceActPage(AcceptanceAct acceptanceAct)
        {
            InitializeComponent();
            _acceptanceAct = acceptanceAct;
        }
    }
}
