using ExpertService.ClassFolder;
using ExpertService.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace ExpertService.PagesFolder
{
    /// <summary>
    /// Interaction logic for AllOrdersPage.xaml
    /// </summary>
    public partial class AllOrdersPage : Page
    {
        public AllOrdersPage()
        {
            InitializeComponent();
            UpdateData();
        }
        private void UpdateData()
        {
            // 1. Берем все заказы + Клиентов + Устройства
            var currentOrders = RepairServiceDBEntities.GetContext().Orders
                .Include(o => o.Client)
                .Include(o => o.Device)
                .ToList();

            // 2. Если в поиске что-то написано, фильтруем список
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                // Приводим к нижнему регистру (ToLower), чтобы поиск не зависел от регистра букв
                string searchText = TxtSearch.Text.ToLower();

                currentOrders = currentOrders.Where(o =>
                    o.OrderID.ToString().Contains(searchText) || // Поиск по номеру
                    (o.Client != null && o.Client.FullName.ToLower().Contains(searchText)) || // Поиск по ФИО
                    (o.Device != null && o.Device.Model.ToLower().Contains(searchText)) // Поиск по модели
                ).ToList();
            }

            // 3. Закидываем отфильтрованный список в таблицу
            OrdersDataGrid.ItemsSource = currentOrders;
        }

        // Событие: когда пользователь печатает в строке поиска
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData(); // Просто обновляем таблицу
        }

        // --- ЭКСПОРТ В EXCEL ---
        private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Берем именно те данные, которые сейчас видны в таблице (с учетом поиска!)
                var ordersToExport = OrdersDataGrid.ItemsSource as System.Collections.Generic.List<Order>;

                if (ordersToExport == null || ordersToExport.Count == 0)
                {
                    MessageBox.Show("Нет данных для экспорта!");
                    return;
                }

                ExcelHelper excelHelper = new ExcelHelper();
                excelHelper.GenerateReport(ordersToExport);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        // --- УДАЛЕНИЕ ЗАКАЗА ---
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, выбрал ли пользователь строку
            var selectedOrder = OrdersDataGrid.SelectedItem as Order;

            if (selectedOrder == null)
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления!");
                return;
            }

            // 2. Спрашиваем подтверждение (защита от случайного клика)
            var result = MessageBox.Show($"Вы точно хотите удалить заказ №{selectedOrder.OrderID}?",
                                         "Подтверждение",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // 3. Удаляем из базы
                    RepairServiceDBEntities.GetContext().Orders.Remove(selectedOrder);
                    RepairServiceDBEntities.GetContext().SaveChanges(); // Сохраняем изменения

                    // 4. Обновляем таблицу
                    MessageBox.Show("Заказ удален!");
                    UpdateData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении. Возможно, есть связанные записи.\n" + ex.Message);
                }
            }
        }
    }
}
