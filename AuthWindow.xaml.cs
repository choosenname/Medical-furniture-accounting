using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting;

public partial class AuthWindow : Window
{
    private readonly ApplicationDBContext _dbContext;

    public AuthWindow()
    {
        InitializeComponent();
        _dbContext = new ApplicationDBContext();
    }

    

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameTextBox.Text;
        string password = PasswordBox.Password;

        // Проверяем, существует ли пользователь с указанными учетными данными
        var user = _dbContext.Storekeepers
            .FirstOrDefault(u => u.Name == username && u.Password == password);

        if (user != null)
        {
            MainWindow mainWindow = new MainWindow(_dbContext, user);
            mainWindow.Show();

            this.Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password.");
        }
    }
}