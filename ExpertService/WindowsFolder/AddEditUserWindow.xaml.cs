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
    public partial class AddEditUserWindow : Window
    {
        private User _user;
        private readonly RepairServiceDBEntities _context;

        // Конструктор принимает пользователя. Если это новый пользователь, его ID будет 0.
        public AddEditUserWindow(User user)
        {
            InitializeComponent();
            _context = RepairServiceDBEntities.GetContext();
            _user = user;

            // Загружаем данные для ComboBox
            RoleComboBox.ItemsSource = _context.Roles.ToList();

            // Если это редактирование существующего пользователя, заполняем поля
            if (_user.UserID != 0)
            {
                WindowTitle.Text = "Редактирование сотрудника";
                FullNameTextBox.Text = _user.FullName;
                LoginTextBox.Text = _user.Login;
                RoleComboBox.SelectedValue = _user.RoleID;
                IsActiveCheckBox.IsChecked = _user.IsActive;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Переносим данные из формы в объект _user
            _user.FullName = FullNameTextBox.Text;
            _user.Login = LoginTextBox.Text;
            _user.RoleID = (int)RoleComboBox.SelectedValue;
            _user.IsActive = IsActiveCheckBox.IsChecked ?? false;

            // Обработка пароля. Меняем, только если поле пароля не пустое.
            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                // В реальном приложении здесь должно быть хэширование!
                // Например: _user.PasswordHash = YourHashingFunction(PasswordBox.Password);
                _user.PasswordHash = PasswordBox.Password; // Временная заглушка без хэширования
            }

            try
            {
                // Если пользователь новый (ID=0), добавляем его
                if (_user.UserID == 0)
                {
                    _context.Users.Add(_user);
                }
                else // Иначе - редактируем
                {
                    // Находим пользователя в контексте и обновляем его
                    var userInDb = _context.Users.Find(_user.UserID);
                    if (userInDb != null)
                    {
                        // Копируем свойства. Attach и EntityState.Modified могут быть сложными с синглтоном.
                        _context.Entry(userInDb).CurrentValues.SetValues(_user);
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}