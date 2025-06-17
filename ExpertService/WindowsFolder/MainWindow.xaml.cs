using ExpertService.ClassFolder;
using ExpertService.DataBase;
using ExpertService.PagesFolder;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _currentUser;

        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            Manager.MainFrame = MainFrame;
            Manager.MainFrame.Navigate(new OrdersPage(_currentUser.Login));
            DisplayUserInfo();
            CheckUserRole();
        }

        private void CheckUserRole()
        {
            // RoleID 3 - это Менеджер
            if (_currentUser.RoleID == 3 || _currentUser.RoleID == 1)
            {
                AddOrderButton.Visibility = Visibility.Visible;
            }
            if (_currentUser.RoleID == 1)
            {
                PersonnelButton.Visibility = Visibility.Visible;
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из системы?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new OrdersPage(_currentUser.Login));
        }

        private void PartsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PartsPage());
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new HelpPage());
        }

        private void DisplayUserInfo()
        {
            if (_currentUser != null)
            {
                MasterNameTextBlock.Text = _currentUser.FullName; //TextBox для имени значением из свойства Name текущего пользователя
            }
            else
            {
                MasterNameTextBlock.Text = "Ошибка";
            }
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addOrderWindow = new AddOrderWindow();
            addOrderWindow.ShowDialog(); // ShowDialog открывает окно модально
        }

        private void DisplayInitialPage()
        {
            // Администратор по умолчанию видит персонал
            if (_currentUser.RoleID == 1)
            {
                Manager.MainFrame.Navigate(new PersonnelPage());
            }
            else // Мастера и менеджеры видят свои заказы
            {
                Manager.MainFrame.Navigate(new OrdersPage(_currentUser.Login));
            }
        }

        // --- НОВЫЙ ОБРАБОТЧИК ---
        private void PersonnelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(Manager.MainFrame.Content is PersonnelPage))
            {
                Manager.MainFrame.Navigate(new PersonnelPage());
            }
        }
    }
}
