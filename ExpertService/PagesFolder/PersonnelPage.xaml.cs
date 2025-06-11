using ExpertService.DataBase;
using ExpertService.WindowsFolder;
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
using System.Data.Entity;

namespace ExpertService.PagesFolder
{
    public partial class PersonnelPage : Page
    {
        public PersonnelPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var context = RepairServiceDBEntities.GetContext();
            UsersDataGrid.ItemsSource = context.Users.Include(u => u.Role).ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно для добавления нового пользователя
            AddEditUserWindow addWindow = new AddEditUserWindow(new User());
            addWindow.ShowDialog();
            LoadUsers(); // Обновляем список после закрытия окна
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                // Открываем то же окно, но передаем ему существующего пользователя для редактирования
                AddEditUserWindow editWindow = new AddEditUserWindow(selectedUser);
                editWindow.ShowDialog();
                LoadUsers(); // Обновляем список
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User userToDelete)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника '{userToDelete.FullName}'?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = RepairServiceDBEntities.GetContext();
                        // Находим пользователя в текущем контексте для корректного удаления
                        var userInDb = context.Users.Find(userToDelete.UserID);
                        if (userInDb != null)
                        {
                            context.Users.Remove(userInDb);
                            context.SaveChanges();
                            MessageBox.Show("Сотрудник успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadUsers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}