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
    public partial class OrdersPage : Page
    {
        private readonly string _masterLogin;

        public OrdersPage(string masterLogin)
        {
            InitializeComponent();
            _masterLogin = masterLogin;
            LoadOrdersGrid();
        }

        private void LoadOrdersGrid()
        {
            var context = RepairServiceDBEntities.GetContext();
            var orders = context.Orders
                .Where(o => o.Master.User.Login == _masterLogin)
                .OrderByDescending(o => o.DateCreated)
                .Select(o => new
                {
                    o.OrderID,
                    DeviceInfo = o.Device.Manufacturer + " " + o.Device.Model,
                    StatusName = o.OrderStatus.StatusName,
                    o.DateCreated
                }).ToList();
            OrdersDataGrid.ItemsSource = orders;
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem == null)
            {
                DetailsPanel.Visibility = Visibility.Collapsed;
                PlaceholderTextBlock.Visibility = Visibility.Visible;
                return;
            }
            int orderId = ((dynamic)OrdersDataGrid.SelectedItem).OrderID;
            RefreshDetailsPanel(orderId);
        }

        private void RefreshDetailsPanel(int orderId)
        {
            var context = RepairServiceDBEntities.GetContext();
            var selectedOrder = context.Orders.AsNoTracking()
                .Include(o => o.Client).Include(o => o.Device)
                .Include(o => o.RepairLogs.Select(rl => rl.Master.User))
                .Include(o => o.UsedParts.Select(up => up.SparePart))
                .FirstOrDefault(o => o.OrderID == orderId);

            if (selectedOrder == null) return;

            ClientNameTextBlock.Text = selectedOrder.Client.FullName;
            ClientPhoneTextBlock.Text = selectedOrder.Client.PhoneNumber;
            DeviceInfoTextBlock.Text = $"{selectedOrder.Device.Manufacturer} {selectedOrder.Device.Model}";
            ProblemDescriptionTextBlock.Text = selectedOrder.ProblemDescription;

            StatusComboBox.ItemsSource = context.OrderStatuses.ToList();
            StatusComboBox.SelectedValuePath = "StatusID";
            StatusComboBox.SelectedValue = selectedOrder.StatusID;

            SparePartsComboBox.ItemsSource = context.SpareParts.Where(p => p.QuantityInStock > 0).ToList();
            SparePartsComboBox.SelectedValuePath = "PartID";
            SparePartsComboBox.DisplayMemberPath = "PartName";

            RepairLogListView.ItemsSource = selectedOrder.RepairLogs
                .OrderByDescending(rl => rl.LogDate)
                .Select(rl => $"[{rl.LogDate:g}] {rl.Master.User.FullName}: {rl.LogText}")
                .ToList();
            UsedPartsListView.ItemsSource = selectedOrder.UsedParts
                .Select(up => $"{up.SparePart.PartName} - {up.QuantityUsed} шт.")
                .ToList();

            DetailsPanel.Visibility = Visibility.Visible;
            PlaceholderTextBlock.Visibility = Visibility.Collapsed;
        }

        // --- ЕДИНЫЙ МЕТОД СОХРАНЕНИЯ ВСЕХ ИЗМЕНЕНИЙ ---
        private void SaveAllChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem == null) return;

            int orderId = ((dynamic)OrdersDataGrid.SelectedItem).OrderID;

            try
            {
                var context = RepairServiceDBEntities.GetContext();

                // ШАГ 1: Находим заказ в текущем контексте
                var orderToUpdate = context.Orders.Find(orderId);
                if (orderToUpdate == null)
                {
                    MessageBox.Show("Заказ не найден. Возможно, он был удален.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // ШАГ 2: Обновляем статус
                if (StatusComboBox.SelectedValue != null)
                {
                    orderToUpdate.StatusID = (int)StatusComboBox.SelectedValue;
                }

                // ШАГ 3: Добавляем новый комментарий, если он есть
                if (!string.IsNullOrWhiteSpace(NewCommentTextBox.Text))
                {
                    var master = context.Masters.FirstOrDefault(m => m.User.Login == _masterLogin);
                    if (master != null)
                    {
                        var newLog = new RepairLog { OrderID = orderId, MasterID = master.MasterID, LogText = NewCommentTextBox.Text, LogDate = DateTime.Now };
                        context.RepairLogs.Add(newLog);
                    }
                }

                // ШАГ 4: Списываем запчасть, если она выбрана
                if (SparePartsComboBox.SelectedValue != null && int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
                {
                    int partId = (int)SparePartsComboBox.SelectedValue;
                    var partToUse = context.SpareParts.Find(partId);

                    if (partToUse != null && partToUse.QuantityInStock >= quantity)
                    {
                        partToUse.QuantityInStock -= quantity;
                        var newUsedPart = new UsedPart { OrderID = orderId, PartID = partId, QuantityUsed = quantity };
                        context.UsedParts.Add(newUsedPart);
                    }
                    else
                    {
                        MessageBox.Show($"Недостаточно запчастей '{partToUse?.PartName}'. В наличии: {partToUse?.QuantityInStock} шт. Изменение не будет сохранено.", "Ошибка склада", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                // ШАГ 5: Сохраняем ВСЕ изменения ОДНИМ вызовом
                context.SaveChanges();

                MessageBox.Show("Все изменения успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Очищаем поля ввода
                NewCommentTextBox.Clear();
                QuantityTextBox.Text = "1";
                SparePartsComboBox.SelectedIndex = -1;

                // Обновляем интерфейс
                LoadOrdersGrid();
                RefreshDetailsPanel(orderId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла критическая ошибка при сохранении: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}