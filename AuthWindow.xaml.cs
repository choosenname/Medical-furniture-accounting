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

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        string username = NewUsernameTextBox.Text;
        string password = NewPasswordBox.Password;

        // Проверяем, существует ли пользователь с таким же именем
        if (_dbContext.Suppliers.Any(u => u.Name == username))
        {
            MessageBox.Show("User with this username already exists.");
            return;
        }

        // Создаем нового пользователя и добавляем его в базу данных
        var newUser = new Supplier() { Name = username, Password = password };
        _dbContext.Suppliers.Add(newUser);
        _dbContext.SaveChanges();

        MessageBox.Show("Registration successful! Username: " + username);
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameTextBox.Text;
        string password = PasswordBox.Password;

        // Проверяем, существует ли пользователь с указанными учетными данными
        var user = _dbContext.Suppliers
            .FirstOrDefault(u => u.Name == username && u.Password == password);

        if (user != null)
        {
            MainWindow mainWindow = new MainWindow(_dbContext);
            mainWindow.Show();

            this.Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password.");
        }
    }
}