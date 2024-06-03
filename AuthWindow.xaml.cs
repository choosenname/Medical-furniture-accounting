using System.Windows;

namespace MedicalFurnitureAccounting;

public partial class AuthWindow
{
    private readonly ApplicationDBContext _dbContext;

    public AuthWindow()
    {
        InitializeComponent();
        _dbContext = new ApplicationDBContext();
    }


    private void Login_Click(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Password;

        // Проверяем, существует ли пользователь с указанными учетными данными
        var user = _dbContext.Storekeepers
            .FirstOrDefault(u => u.Name == username && u.Password == password);

        if (user != null)
        {
            var mainWindow = new MainWindow(_dbContext, user);
            mainWindow.Show();

            Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password.");
        }
    }
}