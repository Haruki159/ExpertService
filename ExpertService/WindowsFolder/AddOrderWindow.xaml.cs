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
using System.Data.Entity;

namespace ExpertService.WindowsFolder
{
    public partial class AddOrderWindow : Window
    {
        private readonly RepairServiceDBEntities _context;

        public AddOrderWindow()
        {
            InitializeComponent();
            _context = RepairServiceDBEntities.GetContext();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Загружаем существующих клиентов
                ClientsComboBox.ItemsSource = _context.Clients.ToList();

                // Загружаем АКТУАЛЬНЫЙ список мастеров
                // Включаем User, чтобы получить FullName
                // Фильтруем по роли "Мастер" (RoleID = 2) и по статусу IsActive
                MastersComboBox.ItemsSource = _context.Masters
                                                .Include(m => m.User)
                                                .Where(m => m.User.RoleID == 2 && m.User.IsActive)
                                                .ToList();

                // Указываем, какое свойство показывать пользователю
                MastersComboBox.DisplayMemberPath = "User.FullName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // --- Валидация ввода ---
            if ((NewClientCheckBox.IsChecked == false && ClientsComboBox.SelectedItem == null) ||
                (NewClientCheckBox.IsChecked == true && (string.IsNullOrWhiteSpace(NewClientNameTextBox.Text) || string.IsNullOrWhiteSpace(NewClientPhoneTextBox.Text))))
            {
                MessageBox.Show("Пожалуйста, выберите клиента или введите данные нового.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(DeviceTypeTextBox.Text) || string.IsNullOrWhiteSpace(DeviceModelTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, укажите тип и модель устройства.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(ProblemDescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, опишите проблему.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Используем транзакцию для обеспечения целостности данных
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // --- Создание сущностей ---
                    // 1. Клиент
                    Client orderClient;
                    if (NewClientCheckBox.IsChecked == true)
                    {
                        orderClient = new Client
                        {
                            FullName = NewClientNameTextBox.Text,
                            PhoneNumber = NewClientPhoneTextBox.Text
                        };
                        _context.Clients.Add(orderClient);
                    }
                    else
                    {
                        orderClient = ClientsComboBox.SelectedItem as Client;
                    }

                    // 2. Устройство
                    var newDevice = new Device
                    {
                        DeviceType = DeviceTypeTextBox.Text,
                        Manufacturer = DeviceManufacturerTextBox.Text,
                        Model = DeviceModelTextBox.Text,
                        SerialNumber = DeviceSerialTextBox.Text
                    };
                    _context.Devices.Add(newDevice);

                    // 3. Заказ
                    var newOrder = new Order
                    {
                        Client = orderClient,
                        Device = newDevice,
                        ProblemDescription = ProblemDescriptionTextBox.Text,
                        DateCreated = DateTime.Now,
                        StatusID = 1 // ID статуса "Принят в ремонт" (или найдите его по имени)
                    };

                    // Назначаем мастера, если он выбран
                    if (MastersComboBox.SelectedItem is Master selectedMaster)
                    {
                        newOrder.MasterID = selectedMaster.MasterID;
                    }

                    _context.Orders.Add(newOrder);

                    // Сохраняем все изменения
                    _context.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show("Новый заказ успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Произошла ошибка при сохранении заказа: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Обработчики для CheckBox
        private void NewClientCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ClientsComboBox.IsEnabled = false;
            NewClientPanel.Visibility = Visibility.Visible;
        }

        private void NewClientCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ClientsComboBox.IsEnabled = true;
            NewClientPanel.Visibility = Visibility.Collapsed;
        }
    }
}
