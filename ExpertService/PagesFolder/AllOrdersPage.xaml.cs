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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

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
            LoadAllOrders();
        }
        private void LoadAllOrders()
        {
            try
            {
                var context = RepairServiceDBEntities.GetContext();

                var allOrders = context.Orders
                    .Include(o => o.Device)
                    .Include(o => o.Client)
                    .Include(o => o.OrderStatus)
                    .Include(o => o.Master.User) // Включаем данные о мастере и его пользователе
                    .OrderByDescending(o => o.DateCreated)
                    .Select(o => new
                    {
                        o.OrderID,
                        DeviceInfo = o.Device.Manufacturer + " " + o.Device.Model,
                        // Проверяем, назначен ли мастер, чтобы избежать ошибки
                        MasterName = o.Master != null ? o.Master.User.FullName : "Не назначен",
                        ClientName = o.Client.FullName,
                        StatusName = o.OrderStatus.StatusName,
                        o.DateCreated
                    })
                    .ToList();

                OrdersDataGrid.ItemsSource = allOrders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
