using ExpertService.DataBase;
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

namespace ExpertService.WindowsFolder
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PassBox.Password;

            var userToLogin = RepairServiceDBEntities.GetContext().Users.FirstOrDefault(p => p.Login == login);

            if (userToLogin == null)
            {
                MessageBox.Show("Пользователь с таким логином не найден.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (userToLogin.PasswordHash != password)
            {
                MessageBox.Show("Неверный пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (userToLogin.IsActive == false)
            {
                MessageBox.Show("Ваша учетная запись деактивирована. Обратитесь к администратору.", "Доступ заблокирован", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MainWindow mainWindow = new MainWindow(userToLogin);
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
